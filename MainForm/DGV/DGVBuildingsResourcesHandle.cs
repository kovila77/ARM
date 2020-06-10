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
        private DataGridViewComboBoxColumnBuildings _cbcBuilsings;
        private DataGridViewComboBoxColumnResources _cbcResources;

        public DGVBuildingsResourcesHandle(DataGridView dgv, DataGridViewComboBoxColumnBuildings cbcBuilsings, DataGridViewComboBoxColumnResources cbcResources) : base(dgv)
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

            _dgv.Columns.Add(_cbcBuilsings);
            _dgv.Columns.Add(_cbcResources);
            _dgv.Columns.Add(MyHelper.strConsumeSpeed, "Скорость потребления");
            _dgv.Columns.Add(MyHelper.strProduceSpeed, "Скорость производства");

            foreach (DataGridViewColumn column in _dgv.Columns)
                column.SortMode = DataGridViewColumnSortMode.Programmatic;

            _dgv.Columns[MyHelper.strBuildingId].ValueType = typeof(int);
            _dgv.Columns[MyHelper.strResourceId].ValueType = typeof(int);
            _dgv.Columns[MyHelper.strConsumeSpeed].ValueType = typeof(int);
            _dgv.Columns[MyHelper.strProduceSpeed].ValueType = typeof(int);

            using (var ctx = new OutpostDataContext())
            {
                var query = from building in ctx.buildings
                            from resource in ctx.resources

                            join brc in ctx.buildings_resources_consume
                                on new { building.building_id, resource.resources_id } equals new { brc.building_id, brc.resources_id } into leftJoin1
                            from brc1 in leftJoin1.DefaultIfEmpty()

                            join brp in ctx.buildings_resources_produce
                                on new { building.building_id, resource.resources_id } equals new { brp.building_id, brp.resources_id } into leftJoin2
                            from brp1 in leftJoin2.DefaultIfEmpty()

                            select new
                            {
                                building_id = building.building_id,
                                resources_id = resource.resources_id,
                                consume_speed = (int?)brc1.consume_speed,
                                produce_speed = (int?)brp1.produce_speed
                            };
                foreach (var item in query)
                {
                    _dgv.Rows.Add(item.building_id, item.resources_id, item.consume_speed.HasValue ? item.consume_speed : 0, item.produce_speed.HasValue ? item.produce_speed : 0);
                }
            }


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

        public override void UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            e.Cancel = true;
        }

        protected override void Insert(DataGridViewRow row)
        {
            return;
        }

        protected override bool ChekRowAndSayReady(DataGridViewRow row)
        {
            return false;
        }

        protected override void Update(DataGridViewRow row)
        {
            return;
        }
    }
}
