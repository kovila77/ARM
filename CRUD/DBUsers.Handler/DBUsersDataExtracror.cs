using System;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBUsersHandler
{
    public class DBUsersDataExtracror : IDisposable
    {
        private NpgsqlConnection _sConn;
        private NpgsqlDataReader reader;

        private DBUsersDataExtracror() { }

        public DBUsersDataExtracror(string sConnStr)
        {
            _sConn = new NpgsqlConnection(sConnStr);
            _sConn.Open();
            var sCOmmand = new NpgsqlCommand
            {
                Connection = _sConn,
                CommandText = @"
                        SELECT uid, ulogin, usalt, upassword, udateregistration
                        FROM users;"
            };
            reader = sCOmmand.ExecuteReader();
        }

        public bool Read()
        {
            return reader.Read();
        }

        public void TakeRow(out int id, out string login, out byte[] salt, out byte[] password, out DateTime createDate)
        {
            id = (int)reader["uid"];
            login = (string)reader["ulogin"];
            salt = (byte[])reader["usalt"];
            password = (byte[])reader["upassword"];
            createDate = (DateTime)reader["udateregistration"];
        }

        ~DBUsersDataExtracror()
        {
            Dispose();
        }

        public void Dispose()
        {
            _sConn?.Close();
        }
    }
}
