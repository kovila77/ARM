using DBUsersHandler;
using Registration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Authentication;

namespace CRUD
{
    public partial class fUsersView : Form
    {
        private DBUsersHandler.DBUsersHandler _dbConrol = new DBUsersHandler.DBUsersHandler();

        private string _userRole;

        public fUsersView(string userRole)
        {
            _userRole = userRole;
            if (_userRole == null || _userRole.Trim().ToLower() != "admin")
            {
                fAuth f = new fAuth(fAuth.TypeLoad.Authentication);
                if (f.ShowDialog() != DialogResult.OK)
                    this.Close();
                else
                {
                    if (f.UserRole == null || f.UserRole.Trim().ToLower() != "admin")
                    {
                        MessageBox.Show($"Пользователь с ролью {f.UserRole} не может использовать данное приложение");
                        this.Close();
                    }
                }
            }
            InitializeComponent();
            InitializeLVUsers();
        }

        private void InitializeLVUsers()
        {
            lvUsers.Clear();
            lvUsers.Columns.Add("Логин");
            lvUsers.Columns.Add("Соль");
            lvUsers.Columns.Add("Пароль");
            lvUsers.Columns.Add("Роль");
            lvUsers.Columns.Add("Дата регистрации");
            using (var extractor = _dbConrol.CreateDataExtractor())
            {
                string login;
                byte[] salt;
                byte[] password;
                DateTime createDate;
                string role;
                int id;
                while (extractor.Read())
                {
                    extractor.TakeRow(out id, out login, out salt, out password, out role, out createDate);
                    var lvi = new ListViewItem(new[]
                    {
                        login,
                        Convert.ToBase64String(salt),
                        Convert.ToBase64String(password),
                        role,
                        createDate.ToLongDateString(),
                    })
                    {
                        Tag = Tuple.Create(id, createDate)
                    };
                    lvUsers.Items.Add(lvi);
                }
            }

            lvUsers.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            lvUsers.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var formRegistration = new fReg(fReg.FormType.Insert);
            if (formRegistration.ShowDialog() == DialogResult.OK)
            {
                var lvi = new ListViewItem(new[]
                {
                        formRegistration.UserLogin,
                        Convert.ToBase64String(formRegistration.UserSalt),
                        Convert.ToBase64String(formRegistration.UserPassword),
                        formRegistration.Role,
                        formRegistration.DateReg.ToLongDateString()
                    })
                {
                    Tag = Tuple.Create(formRegistration.UserId, formRegistration.DateReg)
                };
                lvUsers.Items.Add(lvi);
            }
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void изменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem selectedItem in lvUsers.SelectedItems)
            {
                var selectedItemTagAsTuple = (Tuple<int, DateTime>)selectedItem.Tag;
                var formUpdating = new fReg(fReg.FormType.Update)
                {
                    UserId = selectedItemTagAsTuple.Item1,
                    DateReg = selectedItemTagAsTuple.Item2,
                    UserLogin = selectedItem.SubItems[0].Text,
                    Role = selectedItem.SubItems[3].Text,
                };
                if (formUpdating.ShowDialog() == DialogResult.OK)
                {
                    selectedItem.SubItems[0].Text = formUpdating.UserLogin;
                    if (formUpdating.UserSalt != null) selectedItem.SubItems[1].Text = Convert.ToBase64String(formUpdating.UserSalt);
                    if (formUpdating.UserPassword != null) selectedItem.SubItems[2].Text = Convert.ToBase64String(formUpdating.UserPassword);
                    selectedItem.SubItems[3].Text = formUpdating.Role;
                    selectedItem.SubItems[4].Text = formUpdating.DateReg.ToLongDateString();
                    selectedItem.Tag = Tuple.Create(formUpdating.UserId, formUpdating.DateReg);
                }
            }
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var usersForDel = new List<int>();
            foreach (ListViewItem item in lvUsers.SelectedItems)
            {
                if (item.SubItems[3].Text.ToLower() != "admin")
                {
                    usersForDel.Add(((Tuple<int, DateTime>)item.Tag).Item1);
                }
                else
                {
                    MessageBox.Show("Нельзя удалить Администратора из списка пользователей!");
                }
            }

            if (usersForDel.Count < 1)
                return;
            _dbConrol.DeleteUsers(usersForDel.ToArray());

            foreach (ListViewItem selectedItem in lvUsers.SelectedItems)
            {
                lvUsers.Items.Remove(selectedItem);
            }
        }

        private void выровнятьСтолбцыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lvUsers.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            lvUsers.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void загрузитьДанныеЗановоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitializeLVUsers();
        }

        private void lvUsers_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            изменитьToolStripMenuItem1.Enabled =
                     удалитьToolStripMenuItem1.Enabled = lvUsers.SelectedItems.Count > 0;
        }

        private void правкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            изменитьToolStripMenuItem.Enabled =
                                удалитьToolStripMenuItem.Enabled = lvUsers.SelectedItems.Count > 0;
        }
    }
}
