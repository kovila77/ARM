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

namespace CRUD
{
    public partial class fUsersView : Form
    {
        private DBUsersHandler.DBUsersHandler _dbConrol = new DBUsersHandler.DBUsersHandler();

        public fUsersView()
        {
            InitializeComponent();
            InitializeLVUsers();
        }

        private void InitializeLVUsers()
        {
            lvUsers.Clear();
            lvUsers.Columns.Add("Логин");
            lvUsers.Columns.Add("Соль");
            lvUsers.Columns.Add("Пароль");
            lvUsers.Columns.Add("Дата регистрации");
            using (var extractor = _dbConrol.CreateDataExtractor())
            {
                string login;
                byte[] salt;
                byte[] password;
                DateTime createDate;
                int id;
                while (extractor.Read())
                {
                    extractor.TakeRow(out id, out login, out salt, out password, out createDate);
                    var lvi = new ListViewItem(new[]
                    {
                        login,
                        Convert.ToBase64String(salt),
                        Convert.ToBase64String(password),
                        createDate.ToLongDateString()
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
                };
                if (formUpdating.ShowDialog() == DialogResult.OK)
                {
                    selectedItem.SubItems[0].Text = formUpdating.UserLogin;
                    if (formUpdating.UserSalt != null) selectedItem.SubItems[1].Text = Convert.ToBase64String(formUpdating.UserSalt);
                    if (formUpdating.UserPassword != null) selectedItem.SubItems[2].Text = Convert.ToBase64String(formUpdating.UserPassword);
                    selectedItem.SubItems[3].Text = formUpdating.DateReg.ToLongDateString();
                    selectedItem.Tag = Tuple.Create(formUpdating.UserId, formUpdating.DateReg);
                }
            }
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _dbConrol.DeleteUsers((from item
                                   in lvUsers.SelectedItems.Cast<ListViewItem>()
                                   select ((Tuple<int, DateTime>)item.Tag).Item1).ToArray());

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
