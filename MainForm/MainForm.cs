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

        //public delegate void OutpostAddedHandler(int outpost_id, string outpost_name, int outpost_coordinate_x, int outpost_coordinate_y, int outpost_coordinate_z);
        //public delegate void OutpostChangedHandler(int outpost_id, string outpost_name, int outpost_coordinate_x, int outpost_coordinate_y, int outpost_coordinate_z);
        //public delegate void OutpostDeletedHandler(int outpost_id);
        //public static event OutpostAddedHandler OutpostAdded;
        //public static event OutpostChangedHandler OutpostChanged;
        //public static event OutpostDeletedHandler OutpostDeleted;

        //public delegate void BuildingAddedHandler(int building_id, string building_name, int? outpost_id);
        //public delegate void BuildingChangedHandler(int building_id, string building_name, int? outpost_id = null);
        //public delegate void BuildingDeletedHandler(int building_id);
        //public static event BuildingAddedHandler BuildingAdded;
        //public static event BuildingChangedHandler BuildingChanged;
        //public static event BuildingDeletedHandler BuildingDeleted;

        //public delegate void ResourceAddedHandler(int resource_id, string resource_name);
        //public delegate void ResourceChangedHandler(int resource_id, string resource_name);
        //public delegate void ResourceDeletedHandler(int resource_id);
        //public static event ResourceAddedHandler ResourceAdded;
        //public static event ResourceChangedHandler ResourceChanged;
        //public static event ResourceDeletedHandler ResourceDeleted;

        OutpostDataTableHandler outpostDataTableHandler = new OutpostDataTableHandler();
        BuildingsDataTableHandler buildingsDataTableHandler = new BuildingsDataTableHandler();
        ResourcesDataTableHandler resourcesDataTableHandler = new ResourcesDataTableHandler();

        private int currentTab;
        private string _userRole = null;
        private fUsersView _userControl = null;

        public MainForm(string userRole)
        {
            _userRole = userRole;
            if ((!(new List<string> { "admin", "user", "guest" }.Contains(_userRole.ToLower()))))
            {
                this.Close();
            }
            InitializeComponent();
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
                    ((DataGridView)tp.Controls[0]).ReadOnly = true;
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
            (dgv.Tag as DGVHandle).Initialize();
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
                _userControl = new fUsersView(_userRole.ToLower());
                _userControl.ShowDialog();
            }
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

        private class RowComparerFormattedValue : System.Collections.IComparer
        {
            private static int sortOrderModifier = 1;
            private int columnIndex;

            public RowComparerFormattedValue(SortOrder sortOrder, int columnIndex)
            {
                this.columnIndex = columnIndex;
                if (sortOrder == SortOrder.Descending)
                {
                    sortOrderModifier = -1;
                }
                else if (sortOrder == SortOrder.Ascending)
                {
                    sortOrderModifier = 1;
                }
            }

            public int Compare(object x, object y)
            {
                DataGridViewRow row1 = (DataGridViewRow)x;
                DataGridViewRow row2 = (DataGridViewRow)y;

                int CompareResult = System.String.Compare(
                    row1.Cells[columnIndex].FormattedValue.ToString(),
                    row2.Cells[columnIndex].FormattedValue.ToString());

                return CompareResult * sortOrderModifier;
            }
        }

        private class RowComparerForInt : System.Collections.IComparer
        {
            private static int sortOrderModifier = 1;
            private int columnIndex;

            public RowComparerForInt(SortOrder sortOrder, int columnIndex)
            {
                this.columnIndex = columnIndex;
                if (sortOrder == SortOrder.Descending)
                {
                    sortOrderModifier = -1;
                }
                else if (sortOrder == SortOrder.Ascending)
                {
                    sortOrderModifier = 1;
                }
            }

            public int Compare(object x, object y)
            {
                DataGridViewRow row1 = (DataGridViewRow)x;
                DataGridViewRow row2 = (DataGridViewRow)y;
                int v1 = (int)row1.Cells[columnIndex].Value;
                int v2 = (int)row2.Cells[columnIndex].Value;

                int CompareResult = v1 > v2 ? 1 : (v1 == v2 ? 0 : -1);

                return CompareResult * sortOrderModifier;
            }
        }
    }
}
