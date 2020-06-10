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
using System.Drawing;

namespace MainForm.DGV
{
    class DGVRichOutpostsHandle : DGVHandle
    {

        public DGVRichOutpostsHandle(DataGridView dgv) : base(dgv)
        {
            _dgv.ReadOnly = true;
        }

        public override void Initialize()
        {
            _dgv.CancelEdit();
            _dgv.Rows.Clear();
            _dgv.Columns.Clear();

            _dgv.Columns.Add(MyHelper.strOutpostName, "Форпост");
            _dgv.Columns.Add(MyHelper.strOutpostEconomicValue, "Экономическая ценность");
            _dgv.Columns.Add(MyHelper.strResourceName, "Ресурс");
            _dgv.Columns.Add(MyHelper.strCount, "Количество");

            foreach (DataGridViewColumn column in _dgv.Columns)
                column.SortMode = DataGridViewColumnSortMode.NotSortable;

            _dgv.Columns[MyHelper.strOutpostName].ValueType = typeof(string);
            _dgv.Columns[MyHelper.strOutpostEconomicValue].ValueType = typeof(int);
            _dgv.Columns[MyHelper.strResourceName].ValueType = typeof(string);
            _dgv.Columns[MyHelper.strCount].ValueType = typeof(int);

            var connectionString = ConfigurationManager.ConnectionStrings["OutpostDataContext"].ConnectionString;
            using (var c = new NpgsqlConnection(connectionString))
            {
                c.Open();
                var comm = new NpgsqlCommand()
                {
                    Connection = c,
                    CommandText = @"
                WITH res AS (SELECT resources.resources_id, resources_name, outpost_id, count
                             FROM storage_resources
                                      JOIN resources ON storage_resources.resources_id = resources.resources_id
                             WHERE count > 99)
                SELECT outposts.outpost_id,
                       outposts.outpost_name,
                       outposts.outpost_economic_value,
                       res.resources_name,
                       res.count,
                       outposts.outpost_coordinate_x,
                       outposts.outpost_coordinate_y,
                       outposts.outpost_coordinate_z
                FROM outposts
                         JOIN res ON res.outpost_id = outposts.outpost_id
                WHERE exists(SELECT 1
                             FROM res
                             WHERE res.outpost_id = outposts.outpost_id
                             GROUP BY res.outpost_id
                             HAVING count(*) > 2)
                  AND outposts.outpost_economic_value > 0
                ORDER BY outposts.outpost_economic_value DESC, outposts.outpost_id, res.count DESC"
                };
                var r = comm.ExecuteReader();

                int outpost = -1;
                while (r.Read())
                {
                    if (outpost == (int)r[MyHelper.strOutpostId])
                    {
                        _dgv.Rows.Add(DBNull.Value,
                                      DBNull.Value,
                                      r[MyHelper.strResourceName],
                                      r[MyHelper.strCount]);
                        // _dgv.Rows[_dgv.Rows.Count - 1].DividerHeight = 10;
                    }
                    else
                    {
                        if (outpost != -1)
                        {
                            var row = new DataGridViewRow();
                            row.Height = 10;
                            row.DefaultCellStyle.BackColor = Color.LightGray;
                            _dgv.Rows.Add(row);
                        }
                        outpost = (int)r[MyHelper.strOutpostId];
                        _dgv.Rows.Add(
                            ((string)r[MyHelper.strOutpostName] + " — "
                                                + ((int)r[MyHelper.strOutpostCoordinateX]).ToString() + ";"
                                                 + ((int)r[MyHelper.strOutpostCoordinateY]).ToString() + ";"
                                                   + ((int)r[MyHelper.strOutpostCoordinateZ]).ToString()),
                            r[MyHelper.strOutpostEconomicValue],
                            r[MyHelper.strResourceName],
                            r[MyHelper.strCount]);
                    }
                }
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
