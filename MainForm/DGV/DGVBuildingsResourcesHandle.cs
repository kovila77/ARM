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
        private DataGridViewComboBoxColumn _cbcResorces = new DataGridViewComboBoxColumn()
        {
            Name = "resources_id",
            HeaderText = "Ресурс",
            DisplayMember = "resources_name",
            ValueMember = "resources_id",
            DataPropertyName = "resources_id",
            FlatStyle = FlatStyle.Flat
        };
        private DataGridViewComboBoxColumn _cbcBuldings = new DataGridViewComboBoxColumn()
        {
            Name = "building_id",
            HeaderText = "Здание",
            DisplayMember = "building_name",
            ValueMember = "building_id",
            DataPropertyName = "building_id",
            FlatStyle = FlatStyle.Flat
        };

        public DGVBuildingsResourcesHandle(DataGridView dgv, DataTable dtBuildings, DataTable dtResources) : base(dgv)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["OutpostDataContext"].ConnectionString;
            using (var c = new NpgsqlConnection(connectionString))
            {
                c.Open();
                var comm = new NpgsqlCommand()
                {
                    Connection = c,
                    CommandText = @"SELECT building_id, resources_id, consume_speed, produce_speed
                                                                                FROM public.buildings_resources"
                };
                var r = comm.ExecuteReader();

                _cbcResorces.DataSource = dtResources;
                _cbcBuldings.DataSource = dtBuildings;

                dataTable.Columns.Add("building_id", typeof(int));
                dataTable.Columns.Add("resources_id", typeof(int));
                dataTable.Columns.Add("consume_speed", typeof(int));
                dataTable.Columns.Add("produce_speed", typeof(int));
                dataTable.Columns.Add("Source");
                while (r.Read())
                    dataTable.Rows.Add(r["building_id"], r["resources_id"], r["consume_speed"], r["produce_speed"], 1);

                _dgv.Columns.Add(_cbcBuldings);
                _dgv.Columns.Add(_cbcResorces);
                _cbcResorces.ReadOnly = _cbcBuldings.ReadOnly = true;
                _dgv.DataSource = dataTable;
            }
            MakeThisColumnVisible(new string[] { "building_id", "resources_id", "consume_speed", "produce_speed" });
        }

        public override void UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            e.Cancel = true;
        }

        protected override void Insert(DataGridViewRow row)
        {
            throw new NotImplementedException();
        }

        protected override bool RowReady(DataGridViewRow row)
        {
            return false;
        }

        protected override void Update(DataGridViewRow row)
        {
            throw new NotImplementedException();
        }
    }
}
