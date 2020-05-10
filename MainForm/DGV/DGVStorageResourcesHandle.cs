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
        //public DataGridViewComboBoxColumn cbcOutpost = new DataGridViewComboBoxColumn()
        //{
        //    Name = "outpost_id",
        //    HeaderText = "Форпост",
        //    DisplayMember = "outpost_name",
        //    ValueMember = "outpost_id",
        //    DataPropertyName = "outpost_id",
        //    FlatStyle = FlatStyle.Flat
        //};
        //public DataGridViewComboBoxColumn cbcResorces = new DataGridViewComboBoxColumn()
        //{
        //    Name = "resources_id",
        //    HeaderText = "Ресурс",
        //    DisplayMember = "resources_name",
        //    ValueMember = "resources_id",
        //    DataPropertyName = "resources_id",
        //    FlatStyle = FlatStyle.Flat
        //};
        private DataGridViewComboBoxColumnResources _cbcResources;
        private DataGridViewComboBoxColumnOutpost _cbcOutposts;

        public DGVStorageResourcesHandle(DataGridView dgv,
                ref DataGridViewComboBoxColumnOutpost cbcOutposts,
                ref DataGridViewComboBoxColumnResources cbcResources) : base(dgv)
        {
            this._cbcResources = cbcResources;
            this._cbcOutposts = cbcOutposts;
        }

        public override void Initialize()
        {
            _dgv.CancelEdit();
            _dgv.Rows.Clear();
            _dgv.Columns.Clear();

            _dgv.Columns.Add(_cbcOutposts);
            _dgv.Columns.Add(_cbcResources);
            _dgv.Columns.Add(MyHelper.strCount, "Количество");
            _dgv.Columns.Add(MyHelper.strAccumulationSpeed, "Скорость накопления");
            _dgv.Columns.Add(MyHelper.strSource, "src");

            _dgv.Columns[MyHelper.strOutpostId].ValueType = typeof(int);
            _dgv.Columns[MyHelper.strResourceId].ValueType = typeof(int);
            _dgv.Columns[MyHelper.strCount].ValueType = typeof(int);
            _dgv.Columns[MyHelper.strAccumulationSpeed].ValueType = typeof(int);
            _dgv.Columns[MyHelper.strSource].ValueType = typeof(storage_resources);

            _dgv.Columns[MyHelper.strSource].Visible = false;

            using (var ctx = new OutpostDataContext())
            {
                foreach (var sr in ctx.storage_resources)
                {
                    _dgv.Rows.Add(sr.outpost_id, sr.resources_id, sr.count, sr.accumulation_speed, sr);
                }
            }
        }

        //protected void HideColumns()
        //{
        //    MakeThisColumnVisible(new string[] {
        //            "outpost_id",
        //            "resources_id",
        //            "count",
        //            "accumulation_speed",
        //        });
        //}

        protected override bool ChekRowAndSayReady(DataGridViewRow row)
        {
            return row.Cells["outpost_id"].Value != DBNull.Value
                && row.Cells["resources_id"].Value != DBNull.Value
                && row.Cells["count"].Value != DBNull.Value
                && row.Cells["accumulation_speed"].Value != DBNull.Value
                ;
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
                    row.ErrorText = "Ошибка!";
                    return;
                }
                row.ErrorText = "";
                ctx.storage_resources.Add(sr);
                ctx.SaveChanges();

                row.Cells["Source"].Value = sr;
            }
        }

        protected override void Update(DataGridViewRow row)
        {
            using (var ctx = new OutpostDataContext())
            {
                storage_resources sr = (storage_resources)row.Cells["Source"].Value;
                ctx.storage_resources.Attach(sr);
                var newoutpost_id = (int)row.Cells["outpost_id"].Value;
                var newresources_id = (int)row.Cells["resources_id"].Value;
                if (sr.outpost_id != newoutpost_id
                    || sr.resources_id != newresources_id)
                {
                    var srExisting = ctx.storage_resources.Find(newoutpost_id, newresources_id);
                    if (srExisting != null)
                    {
                        MessageBox.Show($"Для форпоста {row.Cells["outpost_id"].FormattedValue} " +
                            $"запись для ресурса {row.Cells["resources_id"].FormattedValue} " +
                            $"уже существует! Измените или удалите текущую строку!");
                        row.ErrorText = "Ошибка!";
                        return;
                    }
                    ctx.storage_resources.Remove(sr);
                    ctx.SaveChanges();
                    sr = new storage_resources
                    {
                        outpost_id = newoutpost_id,
                        resources_id = newresources_id,
                        count = (int)row.Cells["count"].Value,
                        accumulation_speed = (int)row.Cells["accumulation_speed"].Value,
                    };
                    ctx.storage_resources.Add(sr);
                }
                else
                {
                    sr.count = (int)row.Cells["count"].Value;
                    sr.accumulation_speed = (int)row.Cells["accumulation_speed"].Value;
                }
                row.Cells["Source"].Value = sr;
                row.ErrorText = "";
                ctx.SaveChanges();
            }
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
            row.Cells["Source"].Value = DBNull.Value;
        }

    }
}
