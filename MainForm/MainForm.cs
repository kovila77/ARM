﻿using MainForm.DGV;
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
        private DGVBuildingsResourcesConsumeHandle _dGVBuildingsResourcesConsumeHandle;
        private DGVBuildingsResourcesProduceHandle _dGVBuildingsResourcesProduceHandle;
        private DGVStorageResourcesHandle _dGVStorageResourcesHandle;

        private DGVBuildingsResourcesHandle _dGVBuildingsResourcesHandle;
        private DGVPoorResourcesHandle _dGVPoorResourcesHandle;
        private DGVRichOutpostsHandle _dGVRichOutpostsHandle;

        OutpostDataTableHandler outpostDataTableHandler = new OutpostDataTableHandler();
        BuildingsDataTableHandler buildingsDataTableHandler = new BuildingsDataTableHandler();
        ResourcesDataTableHandler resourcesDataTableHandler = new ResourcesDataTableHandler();

        private int currentTab;
        private int userId;
        private string _userRole = null;
        private fUsersView _userControl = null;

        public MainForm(string userRole, int userId)
        {
            _userRole = userRole;
            this.userId = userId;
            if ((!(new List<string> { "admin", "user", "guest" }.Contains(_userRole.ToLower()))))
            {
                this.Close();
            }
            InitializeComponent();
            ДефицитныеРесурсыToolStripMenuItem.ToolTipText = "Вывод 10 ресурсов, количество которых на складах форпостов, которые хранят данные ресурсы, наименьшее, по отношению к остальным ресурсам.";
            RichOutpostsToolStripMenuItem.ToolTipText = "Cписок форпостов, которые имеют хотя бы 3 ресурса с количеством в хранилище хотя бы 100.";
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (_userRole.ToLower() != "admin")
            {
                tmiTools.Enabled = false;
                tmiTools.Visible = false;
            }
            else
                this.Text += " - Администратор";
            if (_userRole.ToLower() == "guest")
            {
                foreach (TabPage tp in tabControl.TabPages)
                {
                    var dgv = ((DataGridView)tp.Controls[0]);
                    dgv.ReadOnly = true;
                    dgv.AllowUserToDeleteRows = false;
                }
                this.Text += " - Гость";
            }
            currentTab = tabControl.SelectedIndex;

            _dGVOutpostHandle = new DGVOutpostHandle(dgvO, ref outpostDataTableHandler);
            _dGVResourcesHandle = new DGVResourcesHandle(dgvR, ref resourcesDataTableHandler);
            _dGVBuildingsHandle = new DGVBuildingsHandle(dgvB, outpostDataTableHandler.CreateComboBoxColumnOutposts(), ref buildingsDataTableHandler);
            _dGVBuildingsResourcesHandle = new DGVBuildingsResourcesHandle(dgvBR, buildingsDataTableHandler.CreateComboBoxColumnBuildings(), resourcesDataTableHandler.CreateComboBoxColumnResources());
            _dGVBuildingsResourcesConsumeHandle = new DGVBuildingsResourcesConsumeHandle(dgvBRC, buildingsDataTableHandler.CreateComboBoxColumnBuildings(), resourcesDataTableHandler.CreateComboBoxColumnResources());
            _dGVBuildingsResourcesProduceHandle = new DGVBuildingsResourcesProduceHandle(dgvBRP, buildingsDataTableHandler.CreateComboBoxColumnBuildings(), resourcesDataTableHandler.CreateComboBoxColumnResources());
            _dGVStorageResourcesHandle = new DGVStorageResourcesHandle(dgvSR, outpostDataTableHandler.CreateComboBoxColumnOutposts(), resourcesDataTableHandler.CreateComboBoxColumnResources());
            _dGVPoorResourcesHandle = new DGVPoorResourcesHandle(dgvPoorRes);
            _dGVRichOutpostsHandle = new DGVRichOutpostsHandle(dgvRichOutposts);
            dgvO.Tag = _dGVOutpostHandle;
            dgvR.Tag = _dGVResourcesHandle;
            dgvB.Tag = _dGVBuildingsHandle;
            dgvBR.Tag = _dGVBuildingsResourcesHandle;
            dgvBRC.Tag = _dGVBuildingsResourcesConsumeHandle;
            dgvBRP.Tag = _dGVBuildingsResourcesProduceHandle;
            dgvSR.Tag = _dGVStorageResourcesHandle;
            _dGVOutpostHandle.Initialize();
            _dGVResourcesHandle.Initialize();
            _dGVBuildingsHandle.Initialize();
            _dGVBuildingsResourcesHandle.Initialize();
            _dGVBuildingsResourcesConsumeHandle.Initialize();
            _dGVBuildingsResourcesProduceHandle.Initialize();
            _dGVStorageResourcesHandle.Initialize();

            foreach (TabPage tabPage in tabControl.TabPages)
            {
                DataGridView dgv = tabPage.Controls[0] as DataGridView;
                if (dgv == null) continue;
                dgv.ColumnHeaderMouseClick += ColumnHeaderMouseClick;
            }
        }

        private void InitializeDGV()
        {
            foreach (TabPage tabPage in tabControl.TabPages)
            {
                DataGridView dgv = tabPage.Controls[0] as DataGridView;
                if (dgv == null) continue;
                (dgv.Tag as DGVHandle)?.Initialize();
            }
        }

        public void Reload(object sender, EventArgs e)
        {
            DataGridView dgv = tabControl.SelectedTab.Controls[0] as DataGridView;
            if (dgv == null) return;
            (dgv.Tag as DGVHandle)?.Initialize();
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
                _userControl = new fUsersView(_userRole.ToLower(), userId);
            }
            _userControl.ShowDialog();
        }

        private void tabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            DataGridView dgv = tabControl.TabPages[e.TabPageIndex].Controls[0] as DataGridView;
            if (dgv != null && dgv == dgvBR)
            {
                (dgv.Tag as DGVHandle)?.Initialize();
            }
            currentTab = tabControl.SelectedIndex;
        }

        private void PoorRes_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!tabControl.Contains(tpPoorRes))
                tabControl.Controls.Add(tpPoorRes);

            _dGVPoorResourcesHandle.Initialize();

            tabControl.SelectedTab = tpPoorRes;
        }
        private void CoolOutpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!tabControl.Contains(tpRichOutposts))
                tabControl.Controls.Add(tpRichOutposts);

            _dGVRichOutpostsHandle.Initialize();

            tabControl.SelectedTab = tpRichOutposts;
        }

        private void ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if (dgv == null) return;

            if (dgv.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection == SortOrder.Ascending)
            {
                if (dgv.Columns[e.ColumnIndex].CellType != typeof(DataGridViewComboBoxCell) && dgv.Columns[e.ColumnIndex].ValueType == typeof(Int32))
                    dgv.Sort(new RowComparerForInt(SortOrder.Descending, e.ColumnIndex));
                else
                    dgv.Sort(new RowComparerFormattedValue(SortOrder.Descending, e.ColumnIndex));
                dgv.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = SortOrder.Descending;
            }
            else
            {
                if (dgv.Columns[e.ColumnIndex].CellType != typeof(DataGridViewComboBoxCell) && dgv.Columns[e.ColumnIndex].ValueType == typeof(Int32))
                    dgv.Sort(new RowComparerForInt(SortOrder.Ascending, e.ColumnIndex));
                else
                    dgv.Sort(new RowComparerFormattedValue(SortOrder.Ascending, e.ColumnIndex));
                dgv.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = SortOrder.Ascending;
            }

            foreach (DataGridViewColumn column in dgv.Columns)
            {
                if (column.Index == e.ColumnIndex) continue;
                column.HeaderCell.SortGlyphDirection = SortOrder.None;
            }
        }

        

        private void DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }
    }
}
