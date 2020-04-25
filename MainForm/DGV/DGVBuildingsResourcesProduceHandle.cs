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
    class DGVBuildingsResourcesProduceHandle : DGVHandle
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

        public DGVBuildingsResourcesProduceHandle(DataGridView dgv, DataTable dtBuildings, DataTable dtResources) : base(dgv)
        {
            using (var ctx = new OutpostDataContext())
            {
                ctx.buildings_resources_produce.Load();

                cbcResorces.DataSource = dtResources;
                cbcBuldings.DataSource = dtBuildings;

                dataTable.Columns.Add("building_id", typeof(int));
                dataTable.Columns.Add("resources_id", typeof(int));
                dataTable.Columns.Add("produce_speed", typeof(int));
                dataTable.Columns.Add("Source", typeof(buildings_resources_produce));
                ctx.buildings_resources_produce.ToList().ForEach(x => dataTable.Rows.Add(x.building_id, x.resources_id, x.produce_speed, x));

                _dgv.Columns.Add(cbcBuldings);
                _dgv.Columns.Add(cbcResorces);
                _dgv.DataSource = dataTable;
            }
            MakeThisColumnVisible(new string[] {
                    "building_id",
                    "resources_id",
                    "produce_speed"
                });
            _dgv.CellBeginEdit += CellBeginEdit;
            //_dgv.UserAddedRow += UserAddedRow;
        }

        protected override bool RowReady(DataGridViewRow row)
        {
            return base.RowReady(row)
                 && row.Cells["building_id"].Value != DBNull.Value
                 && row.Cells["resources_id"].Value != DBNull.Value
                 && row.Cells["produce_speed"].Value != DBNull.Value
                 ;
        }

        protected override void Insert(DataGridViewRow row)
        {
            using (var ctx = new OutpostDataContext())
            {
                buildings_resources_produce brp = new buildings_resources_produce
                {
                    building_id = (int)row.Cells["building_id"].Value,
                    resources_id = (int)row.Cells["resources_id"].Value,
                    produce_speed = (int)row.Cells["produce_speed"].Value,
                };
                if (ctx.buildings_resources_produce.Find(brp.building_id, brp.resources_id) != null)
                {
                    MessageBox.Show($"Для здания {row.Cells["building_id"].FormattedValue} " +
                        $"производимый ресурс {row.Cells["resources_id"].FormattedValue} " +
                        $"уже существует! Измените или удалите текущую строку!");
                    row.ErrorText = "Ошибка!";
                    return;
                }
                row.ErrorText = "";
                ctx.buildings_resources_produce.Add(brp);
                ctx.SaveChanges();

                row.Cells["Source"].Value = brp;
            }
        }

        protected override void Update(DataGridViewRow row)
        {
            using (var ctx = new OutpostDataContext())
            {
                buildings_resources_produce brp = (buildings_resources_produce)row.Cells["Source"].Value;
                ctx.buildings_resources_produce.Attach(brp);

                brp.building_id = (int)row.Cells["outpost_id"].Value;
                brp.resources_id = (int)row.Cells["resources_id"].Value;
                brp.produce_speed = (int)row.Cells["produce_speed"].Value;

                row.ErrorText = "";
                ctx.Entry(brp).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public override void UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            var row = e.Row;
            if (row.Cells["Source"].Value == DBNull.Value) return;
            buildings_resources_produce brp = (buildings_resources_produce)row.Cells["Source"].Value;
            using (var ctx = new OutpostDataContext())
            {
                ctx.buildings_resources_produce.Attach(brp);
                ctx.buildings_resources_produce.Remove(brp);
                ctx.SaveChanges();
            }
        }

        private void CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (_dgv.Columns[e.ColumnIndex].CellType == typeof(DataGridViewComboBoxCell) && RowHaveSource(_dgv.Rows[e.RowIndex]))
            {
                e.Cancel = true;
                MessageBox.Show("Вы не не можете поменять эту информацию таким образом. Создайте другую строку!");
            }
        }
    }
}
