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
        private DateTime dateReg;

        private FormType frmType;
        public enum FormType
        {
            SelfMade,
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

        private void fReg_Load(object sender, EventArgs e)
        {
            if (frmType == FormType.Update)
            {
                tbLogin.Text = userLogin;
                dtpDate.Value = dateReg;
                tbLogin.Tag = true;
                tbPassword.Tag = true;
            }
        }

        public fReg(FormType frmType)
        {
            InitializeComponent();
            InitializeDBUserClass();
            this.frmType = frmType;
            tbLogin.Tag = false;
            tbPassword.Tag = false;

            switch (frmType)
            {
                case FormType.SelfMade:
                    btRegister.Text = "Зарегистрировать";
                    this.Text = "Регистрация";
                    dtpDate.Value = DateTime.Today;
                    dtpDate.Enabled = false;
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
            if (!loginCheckRegex.IsMatch(tbLogin.Text))
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
                case FormType.SelfMade:

                    _dbConrol.AddNewUser(tbLogin.Text, tbPassword.Text);
                    MessageBox.Show("Регистрация прошла успешно");
                    epMain.SetError(tbLogin, "Логин уже занят");
                    RefreshBtReg();
                    break;

                case FormType.Insert:
                    userLogin = _dbConrol.RemoveExtraSpaces(tbLogin.Text);
                    _dbConrol.AddNewUser(tbLogin.Text, tbPassword.Text, out userId, out userSalt, out userPassword, out dateReg);
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
                    _dbConrol.SetNewData(userId, newLogin, userSalt, userPassword, newDate);
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
            btRegister.Enabled = (bool)tbPassword.Tag && (bool)tbLogin.Tag && !(frmType == FormType.Update && !(epMain.GetError(dtpDate) != "" || epMain.GetError(tbLogin) != "" || epMain.GetError(tbPassword) != ""));
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
    }
}
