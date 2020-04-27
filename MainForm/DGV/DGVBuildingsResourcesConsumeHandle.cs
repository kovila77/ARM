using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;
using System.Data;

namespace MainForm.DGV
{
    class DGVBuildingsResourcesConsumeHandle : DGVHandle
    {
        public DataGridViewComboBoxColumn cbcResorces = new DataGridViewComboBoxColumn()
        {
            Name = "resources_id",
            HeaderText = "Ресурс",
            DisplayMember = "resources_name",
            ValueMember = "resources_id",
            DataPropertyName = "resources_id",
            FlatStyle = FlatStyle.Flat
        };
        public DataGridViewComboBoxColumn cbcBuldings = new DataGridViewComboBoxColumn()
        {
            Name = "building_id",
            HeaderText = "Здание",
            DisplayMember = "building_name",
            ValueMember = "building_id",
            DataPropertyName = "building_id",
            FlatStyle = FlatStyle.Flat
        };
        public DGVBuildingsResourcesConsumeHandle(DataGridView dgv, DataTable dtBuildings, DataTable dtResources) : base(dgv)
        {
            using (var ctx = new OutpostDataContext())
            {
                ctx.buildings_resources_consume.Load();

                cbcResorces.DataSource = dtResources;
                cbcBuldings.DataSource = dtBuildings;

                dataTable.Columns.Add("building_id", typeof(int));
                dataTable.Columns.Add("resources_id", typeof(int));
                dataTable.Columns.Add("consume_speed", typeof(int));
                dataTable.Columns.Add("Source", typeof(buildings_resources_consume));
                ctx.buildings_resources_consume.ToList().ForEach(x => dataTable.Rows.Add(x.building_id, x.resources_id, x.consume_speed, x));

                _dgv.Columns.Add(cbcBuldings);
                _dgv.Columns.Add(cbcResorces);
                _dgv.DataSource = dataTable;
            }
            HideColumns();
            //_dgv.CellBeginEdit += CellBeginEdit;
        }
        protected void HideColumns()
        {
            MakeThisColumnVisible(new string[] {
                    "building_id",
                    "resources_id",
                    "consume_speed"
                });
        }

        protected override bool RowReady(DataGridViewRow row)
        {
            return base.RowReady(row)
                 && row.Cells["building_id"].Value != DBNull.Value
                 && row.Cells["resources_id"].Value != DBNull.Value
                 && row.Cells["consume_speed"].Value != DBNull.Value
                 ;
        }

        protected override void Insert(DataGridViewRow row)
        {
            using (var ctx = new OutpostDataContext())
            {
                buildings_resources_consume brc = new buildings_resources_consume
                {
                    building_id = (int)row.Cells["building_id"].Value,
                    resources_id = (int)row.Cells["resources_id"].Value,
                    consume_speed = (int)row.Cells["consume_speed"].Value,
                };
                if (ctx.buildings_resources_consume.Find(brc.building_id, brc.resources_id) != null)
                {
                    MessageBox.Show($"Для здания {row.Cells["building_id"].FormattedValue} " +
                        $"потребляемый ресурс {row.Cells["resources_id"].FormattedValue} " +
                        $"уже существует! Измените или удалите текущую строку!");
                    row.ErrorText = "Ошибка!";
                    return;
                }
                row.ErrorText = "";
                ctx.buildings_resources_consume.Add(brc);
                ctx.SaveChanges();

                row.Cells["Source"].Value = brc;
            }
        }

        protected override void Update(DataGridViewRow row)
        {
            using (var ctx = new OutpostDataContext())
            {
                buildings_resources_consume brc = (buildings_resources_consume)row.Cells["Source"].Value;
                ctx.buildings_resources_consume.Attach(brc);
                var newbuilding_id = (int)row.Cells["building_id"].Value;
                var newresources_id = (int)row.Cells["resources_id"].Value;
                if (brc.building_id != newbuilding_id
                    || brc.resources_id != newresources_id)
                {
                    var brcExisting = ctx.buildings_resources_consume.Find(newbuilding_id, newresources_id);
                    if (brcExisting != null)
                    {
                        MessageBox.Show($"Для здания {row.Cells["building_id"].FormattedValue} " +
                            $"потребляемый ресурс {row.Cells["resources_id"].FormattedValue} " +
                            $"уже существует! Измените или удалите текущую строку!");
                        row.ErrorText = "Ошибка!";
                        return;
                    }
                    ctx.buildings_resources_consume.Remove(brc);
                    ctx.SaveChanges();
                    brc = new buildings_resources_consume
                    {
                        building_id = newbuilding_id,
                        resources_id = newresources_id,
                        consume_speed = (int)row.Cells["consume_speed"].Value
                    };
                    ctx.buildings_resources_consume.Add(brc);
                }
                else
                {
                    brc.consume_speed = (int)row.Cells["consume_speed"].Value;
                }
                row.Cells["Source"].Value = brc;
                row.ErrorText = "";
                ctx.SaveChanges();
            }
        }

        public override void UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            var row = e.Row;
            if (row.Cells["Source"].Value == DBNull.Value) return;
            buildings_resources_produce brc = (buildings_resources_produce)row.Cells["Source"].Value;
            using (var ctx = new OutpostDataContext())
            {
                ctx.buildings_resources_produce.Attach(brc);
                ctx.buildings_resources_produce.Remove(brc);
                ctx.SaveChanges();
            }
            row.Cells["Source"].Value = DBNull.Value;
        }
    }
}
