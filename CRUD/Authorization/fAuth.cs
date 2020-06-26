using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Registration;

namespace Authentication
{
    public partial class fAuth : Form
    {
        private readonly Regex _loginRegex = new Regex(DBUsersHandler.DBUsersHandler.regularOfCorrectLogin);
        private DBUsersHandler.DBUsersHandler _dbConrol;
        private int _userId = -1;
        private string _role = null;

        public enum TypeLoad
        {
            Test,
            Authentication
        }
        private TypeLoad _typeLoad;

        public int UserId { get { return _userId; } }
        public string UserRole { get { return _role; } }

        public fAuth(TypeLoad tl)
        {
            _typeLoad = tl;
            InitializeComponent();
            InitializeDBUserClass();
        }

        private void InitializeDBUserClass()
        {
            _dbConrol = new DBUsersHandler.DBUsersHandler();
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
            RefreshBtAuth();
        }

        private void btAuthentication_Click(object sender, EventArgs e)
        {
            btAuthentication.Enabled = false;
            if (!checkBox.Checked) {
                _role = _dbConrol.Authentication(tbLogin.Text, tbPassword.Text, out _userId);
            }
            else
                _role = "Guest";
            if (_role != null)
            {
                if (_typeLoad == TypeLoad.Test)
                    MessageBox.Show($@"Вы успешно аутентифицированны как {(_role == "Guest" ? "Гость" : _role)}");
                else
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Вы не аутентифицированны!!!");
            }
            RefreshBtAuth();
        }

        private void tbPassword_TextChanged(object sender, EventArgs e)
        {
            RefreshBtAuth();
        }

        private void RefreshBtAuth()
        {
            if (tbPassword.Text != "" && tbLogin.Text != "" || checkBox.Checked)
            {
                btAuthentication.Enabled = true;
            }
            else
            {
                btAuthentication.Enabled = false;
            }
        }

        private void btBeginReg_Click(object sender, EventArgs e)
        {
            fReg reg = new fReg(fReg.FormType.Registration);
            reg.ShowDialog();
        }

        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            RefreshBtAuth();
        }
    }
}
