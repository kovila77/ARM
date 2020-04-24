using DBUsers.Handler;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DBUsersHandler
{
    public class DBUsersHandler
    {
        /// <summary>
        /// regular expression that the username must match
        /// </summary>
        public static readonly string regularOfCorrectLogin = @"^[A-Za-z0-9_\-А-Яа-яёЁ~`!@#$%^&*()+\\\[\]{}:;'""|<>,.\s\/?=№]{6,50}$";

        private readonly Regex _loginRegex = new Regex(regularOfCorrectLogin);
        private readonly string _sConnStr;

        public DBUsersHandler()
        {
            _sConnStr = new NpgsqlConnectionStringBuilder
            {
                Host = Database.Default.Host,
                Port = Database.Default.Port,
                Database = Database.Default.Name,
                Username = Environment.GetEnvironmentVariable("POSTGRESQL_USERNAME"),
                Password = Environment.GetEnvironmentVariable("POSTGRESQL_PASSWORD"),
                AutoPrepareMinUsages = 2,
                MaxAutoPrepare = 10,
            }.ConnectionString;
        }

        /// <summary>
        /// Use param to create Databse connection
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="database"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public DBUsersHandler(string host, int port, string database, string username, string password)
        {
            _sConnStr = new NpgsqlConnectionStringBuilder
            {
                Host = host,
                Port = port,
                Database = database,
                Username = username,
                Password = password,
                AutoPrepareMinUsages = 2,
                MaxAutoPrepare = 10,
            }.ConnectionString;
        }

        /// <summary>
        /// try coonect to the database
        /// </summary>
        public void TryConnection()
        {
            using (var sConn = new NpgsqlConnection(_sConnStr))
            {
                sConn.Open();
            }
        }

        public string RemoveExtraSpaces(string str)
        {
            str = str.Trim();
            str = Regex.Replace(str, @"\s+", " ");
            return str;
        }

        /// <summary>
        /// checks whether there is a login in the database
        /// </summary>
        /// <param name="newLogin"></param>
        /// <returns></returns>
        public bool IsExistsInDBLogin(string newLogin, bool considerRegister = true)
        {
            newLogin = RemoveExtraSpaces(newLogin);

            if (!_loginRegex.IsMatch(newLogin)) return false;

            using (var sConn = new NpgsqlConnection(_sConnStr))
            {
                sConn.Open();
                var sCommand = new NpgsqlCommand
                {
                    Connection = sConn,
                    CommandText = @"
                        SELECT count(*)
                        FROM users
                        WHERE lower(ulogin) = lower(@newLogin)"
                };
                if (considerRegister)
                    sCommand.CommandText = @"
                        SELECT count(*)
                        FROM users
                        WHERE lower(ulogin) = lower(@newLogin)";
                else
                    sCommand.CommandText = @"
                        SELECT count(*)
                        FROM users
                        WHERE ulogin = @newLogin";
                sCommand.Parameters.AddWithValue("@newLogin", newLogin);
                if ((long)sCommand.ExecuteScalar() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsExistsInDBLogin(string Login, out byte[] salt)
        {
            Login = RemoveExtraSpaces(Login);
            salt = null;

            if (!_loginRegex.IsMatch(Login)) return false;

            using (var sConn = new NpgsqlConnection(_sConnStr))
            {
                sConn.Open();
                var sCommand = new NpgsqlCommand
                {
                    Connection = sConn,
                    CommandText = @"
                        SELECT usalt AS salt
                        FROM users
                        WHERE users.ulogin = @Login"
                };
                sCommand.Parameters.AddWithValue("@Login", Login);
                using (var reader = sCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        salt = (byte[])reader["salt"];
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public bool IsExistsInDBUser(string uLogin, string uPassword)
        {
            uPassword = uPassword.Trim();

            uLogin = RemoveExtraSpaces(uLogin);

            if (!_loginRegex.IsMatch(uLogin)) return false;

            byte[] uSalt;

            if (!IsExistsInDBLogin(uLogin, out uSalt)) return false;

            using (var sConn = new NpgsqlConnection(_sConnStr))
            {
                sConn.Open();
                var sCommand = new NpgsqlCommand
                {
                    Connection = sConn,
                    CommandText = @"
                        SELECT count(*)
                        FROM users
                        WHERE lower(ulogin) = lower(@uLogin)
                          AND upassword = @uPassword;"
                };

                sCommand.Parameters.AddWithValue("@uLogin", uLogin);
                sCommand.Parameters.AddWithValue("@uPassword", PasswordHandler.PasswordHandler.HashPassword(uPassword, uSalt));
                if ((long)sCommand.ExecuteScalar() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// add new user to databse
        /// </summary>
        /// <param name="newLogin"></param>
        /// <param name="newPassword"></param>
        public void AddNewUser(string newLogin, string newPassword)
        {
            newPassword = newPassword.Trim();

            newLogin = RemoveExtraSpaces(newLogin);

            if (!_loginRegex.IsMatch(RemoveExtraSpaces(newLogin)))
            {
                throw new ArgumentException("Bad login");
            }
            var salt = PasswordHandler.PasswordHandler.CreateSalt();
            using (var sConn = new NpgsqlConnection(_sConnStr))
            {
                sConn.Open();
                var sCommand = new NpgsqlCommand
                {
                    Connection = sConn,
                    CommandText = @"
                        INSERT INTO users(ulogin, usalt, upassword)
                        VALUES (@newLogin, @salt, @passwordHash)"
                };
                sCommand.Parameters.AddWithValue("@newLogin", newLogin);
                sCommand.Parameters.AddWithValue("@salt", salt);
                sCommand.Parameters.AddWithValue("@passwordHash", PasswordHandler.PasswordHandler.HashPassword(newPassword, salt));
                sCommand.ExecuteNonQuery();
            }
        }

        public void AddNewUser(string newLogin, string newPassword, out int id, out byte[] salt, out byte[] hash, out DateTime dateReg)
        {
            string passwordCorrected = newPassword.Trim();

            string loginCorrected = RemoveExtraSpaces(newLogin);

            if (!_loginRegex.IsMatch(RemoveExtraSpaces(loginCorrected)))
            {
                throw new ArgumentException("Bad login");
            }
            salt = PasswordHandler.PasswordHandler.CreateSalt();
            hash = PasswordHandler.PasswordHandler.HashPassword(passwordCorrected, salt);
            using (var sConn = new NpgsqlConnection(_sConnStr))
            {
                sConn.Open();
                var sCommand = new NpgsqlCommand
                {
                    Connection = sConn,
                    CommandText = @"
                        INSERT INTO users(ulogin, usalt, upassword)
                        VALUES (@newLogin, @salt, @passwordHash)
                        RETURNING uid, udateregistration"
                };
                sCommand.Parameters.AddWithValue("@newLogin", newLogin);
                sCommand.Parameters.AddWithValue("@salt", salt);
                sCommand.Parameters.AddWithValue("@passwordHash", hash);
                var reader = sCommand.ExecuteReader();
                if (reader.Read())
                {
                    id = (int)reader["uid"];
                    dateReg = (DateTime)reader["udateregistration"];
                }
                else { throw new ArgumentException("База данных не вернула необходимые значения, возможно вставка нового пользователя провалилась!"); }
            }
        }

        public void SetNewData(int id, string login, byte[] salt, byte[] hash, DateTime date)
        {
            login = RemoveExtraSpaces(login);
            if (!_loginRegex.IsMatch(RemoveExtraSpaces(login)))
            {
                throw new ArgumentException("Bad login");
            }
            using (var sConn = new NpgsqlConnection(_sConnStr))
            {
                sConn.Open();
                var sCommand = new NpgsqlCommand
                {
                    Connection = sConn,
                };

                if (hash != null && salt != null)
                {
                    sCommand.Parameters.AddWithValue("@salt", salt);
                    sCommand.Parameters.AddWithValue("@passwordHash", hash);
                    sCommand.CommandText = @"UPDATE users
                                    SET ulogin = @login, usalt = @salt, upassword = @passwordHash, udateregistration = @date
                                    WHERE uid IN(SELECT uid FROM users WHERE uid = @id)";
                }
                else
                {
                    sCommand.CommandText = @"UPDATE users
                                    SET ulogin = @login, udateregistration = @date
                                    WHERE uid IN(SELECT uid FROM users WHERE uid = @id)";
                }
                sCommand.Parameters.AddWithValue("@id", id);
                sCommand.Parameters.AddWithValue("@login", login);
                sCommand.Parameters.AddWithValue("@date", date);
                sCommand.ExecuteNonQuery();
            }
        }

        public void DeleteUsers(int[] userIds)
        {
            using (var sConn = new NpgsqlConnection(_sConnStr))
            {
                sConn.Open();
                var sCommand = new NpgsqlCommand
                {
                    Connection = sConn,
                    CommandText = @"DELETE FROM users WHERE uid = ANY(@userIds)"
                };

                sCommand.Parameters.AddWithValue("@userIds", userIds);
                sCommand.ExecuteNonQuery();
            }
        }

        public DBUsersDataExtracror CreateDataExtractor()
        {
            return new DBUsersDataExtracror(_sConnStr);
        }
    }
}
