using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms;
using System.Data.Entity;
using System.Data;

namespace MainForm.DGV
{
    class DGVBuildingsResourcesHandle : DGVHandle
    {
        //public DataGridViewComboBoxColumn cbcResorces = new DataGridViewComboBoxColumn()
        //{
        //    Name = "resources_id",
        //    HeaderText = "Ресурс",
        //    DisplayMember = "resources_name",
        //    ValueMember = "resources_id",
        //    DataPropertyName = "resources_id",
        //    FlatStyle = FlatStyle.Flat
        //};
        //public DataGridViewComboBoxColumn cbcBuldings = new DataGridViewComboBoxColumn()
        //{
        //    Name = "building_id",
        //    HeaderText = "Здание",
        //    DisplayMember = "building_name",
        //    ValueMember = "building_id",
        //    DataPropertyName = "building_id",
        //    FlatStyle = FlatStyle.Flat
        //};
        private DataGridViewComboBoxColumnBuildings _cbcBuilsings;
        private DataGridViewComboBoxColumnResources _cbcResources;

        public DGVBuildingsResourcesHandle(DataGridView dgv, ref DataGridViewComboBoxColumnBuildings cbcBuilsings, ref DataGridViewComboBoxColumnResources cbcResources) : base(dgv)
        {
            this._cbcBuilsings = cbcBuilsings;
            this._cbcResources = cbcResources;
            _dgv.ReadOnly = true;
        }

        public override void Initialize()
        {
            _dgv.CancelEdit();
            _dgv.Rows.Clear();
            _dgv.Columns.Clear();

            var connectionString = ConfigurationManager.ConnectionStrings["OutpostDataContext"].ConnectionString;
            using (var c = new NpgsqlConnection(connectionString))
            {
                c.Open();
                var comm = new NpgsqlCommand()
                {
                    Connection = c,
                    CommandText = @"
                SELECT  buildings.building_id,
                        resources.resources_id,
                        COALESCE(buildings_resources_consume.consume_speed, 0) AS consume_speed,
                        COALESCE(buildings_resources_produce.produce_speed, 0) AS produce_speed
                FROM buildings
                CROSS JOIN resources
                FULL JOIN buildings_resources_produce ON buildings.building_id = buildings_resources_produce.building_id AND
                                                        resources.resources_id = buildings_resources_produce.resources_id
                FULL JOIN buildings_resources_consume ON buildings.building_id = buildings_resources_consume.building_id AND
                                                                            resources.resources_id = buildings_resources_consume.resources_id;"
                };
                var r = comm.ExecuteReader();

                _dgv.Columns.Add(_cbcBuilsings);
                _dgv.Columns.Add(_cbcResources);
                _dgv.Columns.Add(MyHelper.strConsumeSpeed, "Скорость потребления");
                _dgv.Columns.Add(MyHelper.strProduceSpeed, "Скорость производства");

                _dgv.Columns[MyHelper.strBuildingId].ValueType = typeof(int);
                _dgv.Columns[MyHelper.strResourceId].ValueType = typeof(int);
                _dgv.Columns[MyHelper.strConsumeSpeed].ValueType = typeof(int);
                _dgv.Columns[MyHelper.strProduceSpeed].ValueType = typeof(int);

                while (r.Read())
                    _dgv.Rows.Add(r["building_id"], r["resources_id"], r["consume_speed"], r["produce_speed"], 1);

            }
        }

        //protected void HideColumns()
        //{
        //    MakeThisColumnVisible(new string[] { "building_id", "resources_id", "consume_speed", "produce_speed" });
        //}
        public override void UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            e.Cancel = true;
        }

        protected override void Insert(DataGridViewRow row)
        {
            return;
            //throw new NotImplementedException();
        }

        protected override bool ChekRowAndSayReady(DataGridViewRow row)
        {
            return false;
            //return false;
        }

        protected override void Update(DataGridViewRow row)
        {
            return;
            //throw new NotImplementedException();
        }
    }
}
