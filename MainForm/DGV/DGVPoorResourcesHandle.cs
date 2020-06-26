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
    class DGVPoorResourcesHandle : DGVHandle
    {

        public DGVPoorResourcesHandle(DataGridView dgv) : base(dgv)
        {
            _dgv.ReadOnly = true;
            _dgv.AllowUserToAddRows = false;
        }

        public override void Initialize()
        {
            _dgv.CancelEdit();
            _dgv.Rows.Clear();
            _dgv.Columns.Clear();

            _dgv.Columns.Add(MyHelper.strOutpostName, "Форпост");
            _dgv.Columns.Add(MyHelper.strResourceName, "Ресурс");
            _dgv.Columns.Add(MyHelper.strCount, "Количество");
            _dgv.Columns.Add(MyHelper.strAccumulationSpeed, "Скорость накопления");

            foreach (DataGridViewColumn column in _dgv.Columns)
                column.SortMode = DataGridViewColumnSortMode.NotSortable;

            _dgv.Columns[MyHelper.strOutpostName].ValueType = typeof(string);
            _dgv.Columns[MyHelper.strResourceName].ValueType = typeof(string);
            _dgv.Columns[MyHelper.strCount].ValueType = typeof(int);
            _dgv.Columns[MyHelper.strAccumulationSpeed].ValueType = typeof(int);

            var connectionString = ConfigurationManager.ConnectionStrings["OutpostDataContext"].ConnectionString;
            using (var c = new NpgsqlConnection(connectionString))
            {
                c.Open();
                var comm = new NpgsqlCommand()
                {
                    Connection = c,
                    CommandText = @"
                SELECT outposts.outpost_name,
                       resources.resources_name,
                       count,
                       accumulation_speed,
                       outposts.outpost_coordinate_x,
                       outposts.outpost_coordinate_y,
                       outposts.outpost_coordinate_z
                FROM outposts
                         JOIN storage_resources ON outposts.outpost_id = storage_resources.outpost_id
                         JOIN resources ON storage_resources.resources_id = resources.resources_id
                ORDER BY count, accumulation_speed
                LIMIT 10"
                };
                var r = comm.ExecuteReader();

                while (r.Read())
                    _dgv.Rows.Add(
                        ((string)r[MyHelper.strOutpostName] + " — "
                                            + ((int)r[MyHelper.strOutpostCoordinateX]).ToString() + ";"
                                             + ((int)r[MyHelper.strOutpostCoordinateY]).ToString() + ";"
                                               + ((int)r[MyHelper.strOutpostCoordinateZ]).ToString()),
                        r[MyHelper.strResourceName],
                        r[MyHelper.strCount],
                        r[MyHelper.strAccumulationSpeed]);
            }
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
