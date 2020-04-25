using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainForm.DGV
{
    class DGVStorageResourcesHandle : DGVHandle
    {
        public DataGridViewComboBoxColumn cbcOutpost = new DataGridViewComboBoxColumn()
        {
            Name = "outpost_id",
            HeaderText = "Форпост",
            DisplayMember = "outpost_name",
            ValueMember = "outpost_id",
            DataPropertyName = "outpost_id",
            FlatStyle = FlatStyle.Flat
        };
        public DataGridViewComboBoxColumn cbcResorces = new DataGridViewComboBoxColumn()
        {
            Name = "resources_id",
            HeaderText = "Ресурс",
            DisplayMember = "resources_name",
            ValueMember = "resources_id",
            DataPropertyName = "resources_id",
            FlatStyle = FlatStyle.Flat
        };

        public DGVStorageResourcesHandle(DataGridView dgv, DataTable dtOutpost, DataTable dtResources) : base(dgv)
        {
            using (var ctx = new OutpostDataContext())
            {
                ctx.storage_resources.Load();

                cbcResorces.DataSource = dtResources;
                cbcOutpost.DataSource = dtOutpost;

                dataTable.Columns.Add("outpost_id", typeof(int));
                dataTable.Columns.Add("resources_id", typeof(int));
                dataTable.Columns.Add("count", typeof(int));
                dataTable.Columns.Add("accumulation_speed", typeof(int));
                dataTable.Columns.Add("Source", typeof(storage_resources));
                ctx.storage_resources.ToList().ForEach(x => dataTable.Rows.Add(x.outpost_id, x.resources_id, x.count, x.accumulation_speed, x));

                _dgv.Columns.Add(cbcResorces);
                _dgv.Columns.Add(cbcOutpost);
                _dgv.DataSource = dataTable;
                _dgv.Columns["count"].HeaderText = "Количество";
                _dgv.Columns["accumulation_speed"].HeaderText = "Скорость накопления";
            }
            MakeThisColumnVisible(new string[] {
                    "outpost_id",
                    "resources_id",
                    "count",
                    "accumulation_speed",
                });
            _dgv.CellBeginEdit += CellBeginEdit;
            //_dgv.UserAddedRow += UserAddedRow;
        }

        public override void UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            var row = e.Row;
            if (row.Cells["Source"].Value == DBNull.Value) return;
            storage_resources sr = (storage_resources)row.Cells["Source"].Value;
            using (var ctx = new OutpostDataContext())
            {
                ctx.storage_resources.Attach(sr);

                ctx.storage_resources.Remove(sr);
                ctx.SaveChanges();
            }
        }

        protected override void Insert(DataGridViewRow row)
        {
            using (var ctx = new OutpostDataContext())
            {
                storage_resources sr = new storage_resources
                {
                    outpost_id = (int)row.Cells["outpost_id"].Value,
                    resources_id = (int)row.Cells["resources_id"].Value,
                    count = (int)row.Cells["count"].Value,
                    accumulation_speed = (int)row.Cells["accumulation_speed"].Value,
                };
                if (ctx.storage_resources.Find(sr.outpost_id, sr.resources_id) != null)
                {
                    MessageBox.Show($"Для форпоста {row.Cells["outpost_id"].FormattedValue} " +
                        $"ресурс {row.Cells["resources_id"].FormattedValue} " +
                        $"уже существует! Измените или удалите текущую строку!");
                    ctx.Dispose();
                    return;
                }
                ctx.storage_resources.Add(sr);
                ctx.SaveChanges();

                row.Cells["Source"].Value = sr;
            }
        }

        protected override bool RowReady(DataGridViewRow row)
        {
            return base.RowReady(row)
                && row.Cells["outpost_id"].Value != DBNull.Value
                && row.Cells["resources_id"].Value != DBNull.Value
                && row.Cells["count"].Value != DBNull.Value
                && row.Cells["accumulation_speed"].Value != DBNull.Value
                ;
        }

        protected override void Update(DataGridViewRow row)
        {
            using (var ctx = new OutpostDataContext())
            {
                storage_resources sr = (storage_resources)row.Cells["Source"].Value;
                ctx.storage_resources.Attach(sr);

                sr.outpost_id = (int)row.Cells["outpost_id"].Value;
                sr.resources_id = (int)row.Cells["resources_id"].Value;
                sr.count = (int)row.Cells["count"].Value;
                sr.accumulation_speed = (int)row.Cells["accumulation_speed"].Value;

                ctx.Entry(sr).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }
        private void CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (_dgv.Columns[e.ColumnIndex].CellType == typeof(DataGridViewComboBoxCell) && RowHaveSource(_dgv.Rows[e.RowIndex]))
            {
                e.Cancel = true;
            }
        }
    }
}
