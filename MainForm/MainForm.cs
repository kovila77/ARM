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

        private void запрос1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //_dgv.CancelEdit();
            //_dgv.Rows.Clear();
            //_dgv.Columns.Clear();

            //_dgv.Columns.Add(_cbcBuilsings);
            //_dgv.Columns.Add(_cbcResources);
            //_dgv.Columns.Add(MyHelper.strConsumeSpeed, "Скорость потребления");
            //_dgv.Columns.Add(MyHelper.strProduceSpeed, "Скорость производства");

            //foreach (DataGridViewColumn column in _dgv.Columns)
            //    column.SortMode = DataGridViewColumnSortMode.Programmatic;

            //_dgv.Columns[MyHelper.strBuildingId].ValueType = typeof(int);
            //_dgv.Columns[MyHelper.strResourceId].ValueType = typeof(int);
            //_dgv.Columns[MyHelper.strConsumeSpeed].ValueType = typeof(int);
            //_dgv.Columns[MyHelper.strProduceSpeed].ValueType = typeof(int);

            //using (var ctx = new OutpostDataContext())
            //{
            //    var query = from building in ctx.buildings
            //                from resource in ctx.resources

            //                join brc in ctx.buildings_resources_consume
            //                    on new { building.building_id, resource.resources_id } equals new { brc.building_id, brc.resources_id } into leftJoin1
            //                from brc1 in leftJoin1.DefaultIfEmpty()

            //                join brp in ctx.buildings_resources_produce
            //                    on new { building.building_id, resource.resources_id } equals new { brp.building_id, brp.resources_id } into leftJoin2
            //                from brp1 in leftJoin2.DefaultIfEmpty()

            //                select new
            //                {
            //                    building_id = building.building_id,
            //                    resources_id = resource.resources_id,
            //                    consume_speed = (int?)brc1.consume_speed,
            //                    produce_speed = (int?)brp1.produce_speed
            //                };
            //    foreach (var item in query)
            //    {
            //        _dgv.Rows.Add(item.building_id, item.resources_id, item.consume_speed.HasValue ? item.consume_speed : 0, item.produce_speed.HasValue ? item.produce_speed : 0);
            //    }
            //}


            //return;
            //var connectionString = ConfigurationManager.ConnectionStrings["OutpostDataContext"].ConnectionString;
            //using (var c = new NpgsqlConnection(connectionString))
            //{
            //    c.Open();
            //    var comm = new NpgsqlCommand()
            //    {
            //        Connection = c,
            //        CommandText = @"
            //    SELECT  buildings.building_id,
            //            resources.resources_id,
            //            COALESCE(buildings_resources_consume.consume_speed, 0) AS consume_speed,
            //            COALESCE(buildings_resources_produce.produce_speed, 0) AS produce_speed
            //    FROM buildings
            //    CROSS JOIN resources
            //    FULL JOIN buildings_resources_produce ON buildings.building_id = buildings_resources_produce.building_id AND
            //                                            resources.resources_id = buildings_resources_produce.resources_id
            //    FULL JOIN buildings_resources_consume ON buildings.building_id = buildings_resources_consume.building_id AND
            //                                                                resources.resources_id = buildings_resources_consume.resources_id;"
            //    };
            //    var r = comm.ExecuteReader();

            //    while (r.Read())
            //        _dgv.Rows.Add(r["building_id"], r["resources_id"], r["consume_speed"], r["produce_speed"], 1);

            //}
        }

        private void запрос2ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
