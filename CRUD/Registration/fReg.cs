using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Registration
{
    public partial class fReg : Form
    {
        private readonly Regex loginCheckRegex = new Regex(DBUsersHandler.DBUsersHandler.regularOfCorrectLogin);
        private DBUsersHandler.DBUsersHandler _dbConrol = new DBUsersHandler.DBUsersHandler();

        private int userId = -1;
        private string userLogin = null;
        private byte[] userSalt = null;
        private byte[] userPassword = null;
        private string role = "user";
        private int userEditingUser = -1;
        private string oldRole = "user";
        private DateTime dateReg;

        private FormType frmType;
        public enum FormType
        {
            Registration,
            Insert,
            Update
        }
        public FormType FrmType { get { return frmType; } }

        public int UserId
        {
            get { return userId; }
            set
            {
                if (frmType == FormType.Update)
                {
                    userId = value;
                }
            }
        }
        public string UserLogin
        {
            get { return userLogin; }
            set
            {
                if (frmType == FormType.Update)
                {
                    userLogin = value;
                }
            }
        }
        public byte[] UserSalt
        {
            get { return userSalt; }
            set
            {
                if (frmType == FormType.Update)
                {
                    userSalt = value;
                }
            }
        }
        public byte[] UserPassword
        {
            get { return userPassword; }
            set
            {
                if (frmType == FormType.Update)
                {
                    userPassword = value;
                }
            }
        }
        public string Role
        {
            get { return role; }
            set
            {
                if (frmType == FormType.Update)
                {
                    role = value;
                }
            }
        }
        public DateTime DateReg
        {
            get { return dateReg; }
            set
            {
                if (frmType == FormType.Update)
                {
                    dateReg = value;
                }
            }
        }

        public int UserEditingUserId { get { return userEditingUser; } set { userEditingUser = value; } }

        private void fReg_Load(object sender, EventArgs e)
        {
            if (frmType == FormType.Update)
            {
                tbLogin.Text = userLogin;
                dtpDate.Value = dateReg;
                tbLogin.Tag = true;
                tbPassword.Tag = true;
            }
            cbRole.Tag = true;
            oldRole = role;
            //cbRole.SelectedIndex = (role == "user" ? 0 : 1);
            cbRole.SelectedItem = role;
        }

        public fReg(FormType frmType)
        {
            InitializeComponent();
            InitializeDBUserClass();
            this.frmType = frmType;
            tbLogin.Tag = false;
            tbPassword.Tag = false;
            cbRole.Tag = false;

            cbRole.DataSource = new string[] { "user", "admin" };

            switch (frmType)
            {
                case FormType.Registration:
                    btRegister.Text = "Зарегистрировать";
                    this.Text = "Регистрация";
                    dtpDate.Value = DateTime.Today;
                    dtpDate.Enabled = false;

                    cbRole.Enabled = false;
                    break;
                case FormType.Insert:

                    btRegister.Text = "Добавить";
                    this.Text = "Добавление нового пользователя";
                    dtpDate.Value = DateTime.Today;
                    dtpDate.Enabled = false;
                    break;
                case FormType.Update:
                    btRegister.Text = "Изменить";
                    this.Text = "Изменение пользователя";
                    break;
                default:
                    break;
            }
        }

        private void InitializeDBUserClass()
        {
            try
            {
                _dbConrol.TryConnection();
            }
            catch (Exception e)
            {
                MessageBox.Show($"Trouble with DB\n{e.Message}");
                throw e;
            }
        }

        private void tbLogin_TextChanged(object sender, EventArgs e)
        {
            if (!loginCheckRegex.IsMatch(tbLogin.Text.Trim()))
            {
                epMain.SetError(tbLogin, "Некорректный логин: В логине могут быть использованы символы, изображённые на классической русско-английской раскладке клавиатуре. Длина логина от 6 до 50 символов");
                tbLogin.Tag = false;
            }
            else
            {
                if (frmType == FormType.Update && _dbConrol.RemoveExtraSpaces(tbLogin.Text) == userLogin)
                {
                    epMain.SetError(tbLogin, "");
                    tbLogin.Tag = true;
                }
                else
                {
                    if (_dbConrol.IsExistsInDBLogin(tbLogin.Text, !(frmType == FormType.Update)))
                    {
                        epMain.SetError(tbLogin, "Логин уже занят");
                        tbLogin.Tag = false;
                    }
                    else
                    {
                        if (frmType == FormType.Update)
                        {
                            epMain.SetError(tbLogin, "Логин будет изменён!");
                        }
                        else
                        {
                            epMain.SetError(tbLogin, "");
                        }
                        tbLogin.Tag = true;
                    }
                }
            }

            RefreshBtReg();
        }

        private void btRegister_Click(object sender, EventArgs e)
        {
            btRegister.Enabled = false;
            switch (frmType)
            {
                case FormType.Registration:

                    _dbConrol.AddNewUser(tbLogin.Text, tbPassword.Text, role);
                    MessageBox.Show("Регистрация прошла успешно");
                    this.DialogResult = DialogResult.OK;
                    epMain.SetError(tbLogin, "Логин уже занят");
                    RefreshBtReg();
                    break;

                case FormType.Insert:
                    userLogin = _dbConrol.RemoveExtraSpaces(tbLogin.Text);
                    _dbConrol.AddNewUser(tbLogin.Text, tbPassword.Text, role, out userId, out userSalt, out userPassword, out dateReg);
                    this.DialogResult = DialogResult.OK;
                    break;
                case FormType.Update:
                    string newPaswr = _dbConrol.RemoveExtraSpaces(tbPassword.Text);
                    string newLogin = _dbConrol.RemoveExtraSpaces(tbLogin.Text);
                    DateTime newDate = dtpDate.Value;
                    if (newPaswr != "")
                    {
                        userSalt = PasswordHandler.PasswordHandler.CreateSalt();
                        userPassword = PasswordHandler.PasswordHandler.HashPassword(newPaswr, userSalt);
                    }
                    if (userLogin != newLogin)
                    {
                        userLogin = newLogin;
                    }
                    if (dateReg != newDate)
                    {
                        dateReg = newDate;
                    }
                    _dbConrol.SetNewData(userId, newLogin, userSalt, userPassword, role, newDate);
                    this.DialogResult = DialogResult.OK;
                    break;
                default:
                    break;
            }
        }

        private void tbPassword_TextChanged(object sender, EventArgs e)
        {
            if (frmType == FormType.Update && _dbConrol.RemoveExtraSpaces(tbPassword.Text) == "") { epMain.SetError(tbPassword, ""); tbPassword.Tag = true; return; }
            else
            {
                if (!PasswordHandler.PasswordHandler.IsStrongPassword(tbPassword.Text, new List<string> { tbLogin.Text }))
                {
                    epMain.SetError(tbPassword, "Слабый пороль");
                    tbPassword.Tag = false;
                }
                else
                {
                    if (frmType == FormType.Update)
                    {
                        epMain.SetError(tbPassword, "Пароль будет изменён!");
                    }
                    else
                    {
                        epMain.SetError(tbPassword, "");
                    }
                    tbPassword.Tag = true;
                }
                RefreshBtReg();
            }
        }

        private void RefreshBtReg()
        {
            //if ((bool)tbPassword.Tag && (bool)tbLogin.Tag)
            //{
            //    if (frmType == FormType.Update && !(epMain.GetError(dtpDate) != "" || epMain.GetError(tbLogin) != "" || epMain.GetError(tbPassword) != ""))
            //        btRegister.Enabled = false;
            //    else
            //        btRegister.Enabled = true;
            //}
            //else
            //{
            //    btRegister.Enabled = false;
            //}
            btRegister.Enabled = (bool)cbRole.Tag && (bool)tbPassword.Tag && (bool)tbLogin.Tag
                && !(frmType == FormType.Update && !(epMain.GetError(dtpDate) != "" || epMain.GetError(tbLogin) != "" || epMain.GetError(tbPassword) != "" || epMain.GetError(cbRole) != ""));
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (frmType != FormType.Update) return;
            if (dtpDate.Value == dateReg)
            {
                epMain.SetError(dtpDate, "");
            }
            else
            {
                epMain.SetError(dtpDate, "Дата будет измененна!");
            }
            RefreshBtReg();
        }

        private void cbRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            role = cbRole.SelectedItem.ToString();

            if (oldRole == role)
            {
                epMain.SetError(cbRole, "");
                cbRole.Tag = true;
            }
            else
            {
                if (UserEditingUserId == UserId && role != oldRole)
                {
                    epMain.SetError(cbRole, "Вы не можете изменять свою роль! Сделайте это с другой записи администратора!");
                    cbRole.Tag = false;
                }
                else
                {
                    epMain.SetError(cbRole, "Роль будет изменена!");
                    cbRole.Tag = true;
                }
            }

            RefreshBtReg();
        }
    }
}
