using MainForm.DGV;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Authentication;
using CRUD;

namespace MainForm
{
    public partial class MainForm : Form
    {
        private DGVOutpostHandle _dGVOutpostHandle;
        private DGVResourcesHandle _dGVResourcesHandle;
        private DGVBuildingsHandle _dGVBuildingsHandle;
        private DGVBuildingsResourcesHandle _dGVBuildingsResourcesHandle;
        private DGVBuildingsResourcesConsumeHandle _dGVBuildingsResourcesConsumeHandle;
        private DGVBuildingsResourcesProduceHandle _dGVBuildingsResourcesProduceHandle;
        private DGVStorageResourcesHandle _dGVStorageResourcesHandle;

        private int currentTab;

        private string _userRole = null;

        private fUsersView _userControl = null;

        public MainForm(string userRole)
        {
            _userRole = userRole;
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            InitializeDGV();
            if (_userRole.ToLower() != "admin")
            {
                tmiTools.Enabled = false;
                tmiTools.Visible = false;
            }
            if (_userRole.ToLower() == "guest")
            {
                foreach (TabPage tp in tabControl.TabPages)
                {
                    ((DataGridView)tp.Controls[0]).ReadOnly = true;
                }
            }
            currentTab = tabControl.SelectedIndex;
        }

        private void InitializeDGV()
        {
            //dgvO.DefaultCellStyle.NullValue = DBNull.Value;
            //dgvO.DefaultCellStyle.
            _dGVOutpostHandle = new DGVOutpostHandle(dgvO);
            dgvO.Tag = _dGVOutpostHandle;

            //dgvR.DefaultCellStyle.NullValue = DBNull.Value;
            _dGVResourcesHandle = new DGVResourcesHandle(dgvR);
            dgvR.Tag = _dGVResourcesHandle;

            //dgvB.DefaultCellStyle.NullValue =DBNull.Value;
            _dGVBuildingsHandle = new DGVBuildingsHandle(dgvB, _dGVOutpostHandle.dataTable);
            dgvB.Tag = _dGVBuildingsHandle;

            //dgvBR.DefaultCellStyle.NullValue = DBNull.Value;
            _dGVBuildingsResourcesHandle = new DGVBuildingsResourcesHandle(dgvBR,
                                                                    _dGVBuildingsHandle.dataTable,
                                                                    _dGVResourcesHandle.dataTable);
            dgvBR.Tag = _dGVBuildingsResourcesHandle;

            //dgvBRC.DefaultCellStyle.NullValue = DBNull.Value;
            _dGVBuildingsResourcesConsumeHandle = new DGVBuildingsResourcesConsumeHandle(dgvBRC,
                                                                    _dGVBuildingsHandle.dataTable,
                                                                    _dGVResourcesHandle.dataTable);
            dgvBRC.Tag = _dGVBuildingsResourcesConsumeHandle;

            //dgvBRP.DefaultCellStyle.NullValue = DBNull.Value;
            _dGVBuildingsResourcesProduceHandle = new DGVBuildingsResourcesProduceHandle(dgvBRP,
                                                                    _dGVBuildingsHandle.dataTable,
                                                                    _dGVResourcesHandle.dataTable);
            dgvBRP.Tag = _dGVBuildingsResourcesProduceHandle;

            //dgvSR.DefaultCellStyle.NullValue = DBNull.Value;
            _dGVStorageResourcesHandle = new DGVStorageResourcesHandle(dgvSR,
                                                                    _dGVOutpostHandle.dataTable,
                                                                    _dGVResourcesHandle.dataTable);
            dgvSR.Tag = _dGVStorageResourcesHandle;
        }

        public void ReloadO()
        {
            _dGVOutpostHandle = new DGVOutpostHandle(dgvO);
            _dGVBuildingsHandle.cbcOutpost.DataSource = _dGVOutpostHandle.dataTable;
            _dGVStorageResourcesHandle.cbcOutpost.DataSource = _dGVOutpostHandle.dataTable;
            dgvO.Tag = _dGVOutpostHandle;
        }
        public void ReloadR()
        {
            _dGVResourcesHandle = new DGVResourcesHandle(dgvO);
            //_dGVBuildingsResourcesHandle.cbcResorces.DataSource = _dGVResourcesHandle.dataTable;
            _dGVBuildingsResourcesConsumeHandle.cbcResorces.DataSource = _dGVResourcesHandle.dataTable;
            _dGVBuildingsResourcesProduceHandle.cbcResorces.DataSource = _dGVResourcesHandle.dataTable;
            _dGVStorageResourcesHandle.cbcResorces.DataSource = _dGVResourcesHandle.dataTable;
            dgvR.Tag = _dGVResourcesHandle;
        }
        public void ReloadB()
        {
            _dGVBuildingsHandle = new DGVBuildingsHandle(dgvB, _dGVOutpostHandle.dataTable);
            //_dGVBuildingsResourcesHandle.cbcResorces.DataSource = _dGVBuildingsHandle.dataTable;
            _dGVBuildingsResourcesConsumeHandle.cbcResorces.DataSource = _dGVBuildingsHandle.dataTable;
            _dGVBuildingsResourcesProduceHandle.cbcResorces.DataSource = _dGVBuildingsHandle.dataTable;
            dgvB.Tag = _dGVBuildingsHandle;
        }
        public void ReloadBR()
        {
            _dGVBuildingsResourcesHandle = new DGVBuildingsResourcesHandle(dgvBR,
                                                                    _dGVBuildingsHandle.dataTable,
                                                                    _dGVResourcesHandle.dataTable);
            dgvBR.Tag = _dGVBuildingsResourcesHandle;
        }
        public void ReloadBRC()
        {
            _dGVBuildingsResourcesConsumeHandle = new DGVBuildingsResourcesConsumeHandle(dgvBRC,
                                                                    _dGVBuildingsHandle.dataTable,
                                                                    _dGVResourcesHandle.dataTable);
            dgvBRC.Tag = _dGVBuildingsResourcesConsumeHandle;
        }
        public void ReloadBRP()
        {
            _dGVBuildingsResourcesProduceHandle = new DGVBuildingsResourcesProduceHandle(dgvBRP,
                                                                    _dGVBuildingsHandle.dataTable,
                                                                    _dGVResourcesHandle.dataTable);
            dgvBRP.Tag = _dGVBuildingsResourcesProduceHandle;
        }
        public void ReloadSR()
        {
            _dGVStorageResourcesHandle = new DGVStorageResourcesHandle(dgvSR,
                                                                    _dGVOutpostHandle.dataTable,
                                                                    _dGVResourcesHandle.dataTable);
            dgvSR.Tag = _dGVStorageResourcesHandle;
        }
        public void Reload(object sender, EventArgs e)
        {
            var dgv = tabControl.SelectedTab.Controls[0];
            if (dgv == dgvB) ReloadB();
            if (dgv == dgvR) ReloadR();
            if (dgv == dgvBRC) ReloadBRC();
            if (dgv == dgvBRP) ReloadBRP();
            if (dgv == dgvO) ReloadO();
            if (dgv == dgvBR) ReloadBR();
            if (dgv == dgvSR) ReloadSR();
        }

        private void FullReload(object sender, EventArgs e)
        {
            InitializeDGV();
        }

        private void управлениеПользователямиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_userRole.ToLower() != "admin")
            {
                MessageBox.Show("У вас недостаточно прав!");
                return;
            }
            if (_userControl == null)
            {
                _userControl = new fUsersView();
                _userControl.ShowDialog();
            }
        }

        private void tabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            DataGridView dgv = (DataGridView)tabControl.TabPages[currentTab].Controls[0];
            bool haveUncommitedChanges = false;
            foreach (DataGridViewRow row in dgv.Rows) if (row.ErrorText != "") haveUncommitedChanges = true;
            if (haveUncommitedChanges)
                if (MessageBox.Show("Имееются несохранённые изменения!\nПри переключении на другую вкладку изменения будут сброшены.", "Предупреждение", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    ((DGVHandle)dgv.Tag).ClearChanges();
                else
                {
                    e.Cancel = true;
                    return;
                }
            //if (dgv == dgvB) ReloadB();
            //if (dgv == dgvR) ReloadR();
            //if (dgv == dgvBRC) ReloadBRC();
            //if (dgv == dgvBRP) ReloadBRP();
            //if (dgv == dgvO) ReloadO();
            //if (dgv == dgvSR) ReloadSR();
            if (tabControl.TabPages[tabControl.SelectedIndex].Controls[0] == dgvBR) ReloadBR();
            currentTab = tabControl.SelectedIndex;
        }

        private void сброситьРедактированиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridView dgv = (DataGridView)tabControl.TabPages[currentTab].Controls[0];
            ((DGVHandle)dgv.Tag).ClearChanges();
        }        
    }
}
