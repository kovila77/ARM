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
                DataGridViewComboBoxColumnOutpost cbcOutposts,
                DataGridViewComboBoxColumnResources cbcResources) : base(dgv)
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

            foreach (DataGridViewColumn column in _dgv.Columns)
                column.SortMode = DataGridViewColumnSortMode.Programmatic;

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
            var cellsWithPotentialErrors = new List<DataGridViewCell> {
                                                   row.Cells[MyHelper.strOutpostId],
                                                   row.Cells[MyHelper.strResourceId],
                                                   row.Cells[MyHelper.strCount],
                                                   row.Cells[MyHelper.strAccumulationSpeed],
                                                 };
            foreach (var cellWithPotentialError in cellsWithPotentialErrors)
            {
                if (cellWithPotentialError.FormattedValue.ToString().RmvExtrSpaces() == "")
                {
                    cellWithPotentialError.ErrorText = MyHelper.strEmptyCell;
                    row.ErrorText = MyHelper.strBadRow;
                }
                else
                {
                    cellWithPotentialError.ErrorText = "";
                }
            }
            if (cellsWithPotentialErrors.FirstOrDefault(cellWithPotentialError => cellWithPotentialError.ErrorText.Length > 0) == null)
                row.ErrorText = "";
            else
                return false;
            return true;
        }

        protected override void Insert(DataGridViewRow row)
        {
            try
            {
                using (var ctx = new OutpostDataContext())
                {
                    int new_outpost_id = (int)row.Cells[MyHelper.strOutpostId].Value;
                    int new_resource_id = (int)row.Cells[MyHelper.strResourceId].Value;
                    int new_count = (int)row.Cells[MyHelper.strCount].Value;
                    int new_accumulation_speed = (int)row.Cells[MyHelper.strAccumulationSpeed].Value;

                    if (ctx.storage_resources.AsEnumerable().FirstOrDefault(sr =>
                                sr.outpost_id == new_outpost_id
                                && sr.resources_id == new_resource_id) != null)
                    {
                        string eo = $"Для форпоста {row.Cells[MyHelper.strOutpostId].FormattedValue} " +
                                    $"ресурс {row.Cells[MyHelper.strResourceId].FormattedValue} " +
                                    $"уже существует! Измените или удалите текущую строку!";
                        MessageBox.Show(eo);
                        row.ErrorText = MyHelper.strBadRow + " " + eo;
                        return;
                    }

                    var new_sr = new storage_resources();
                    new_sr.outpost_id = new_outpost_id;
                    new_sr.resources_id = new_resource_id;
                    new_sr.count = new_count;
                    new_sr.accumulation_speed = new_accumulation_speed;

                    ctx.storage_resources.Add(new_sr);

                    ctx.SaveChanges();

                    row.Cells[MyHelper.strSource].Value = new_sr;
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        protected override void Update(DataGridViewRow row)
        {
            try
            {
                using (var ctx = new OutpostDataContext())
                {
                    var new_sr = (storage_resources)row.Cells[MyHelper.strSource].Value;
                    ctx.storage_resources.Attach(new_sr);

                    int new_outpost_id = (int)row.Cells[MyHelper.strOutpostId].Value;
                    int new_resource_id = (int)row.Cells[MyHelper.strResourceId].Value;
                    int new_count = (int)row.Cells[MyHelper.strCount].Value;
                    int new_accumulation_speed = (int)row.Cells[MyHelper.strAccumulationSpeed].Value;

                    if (ctx.storage_resources.AsEnumerable().FirstOrDefault(sr =>
                                sr != new_sr
                                && sr.outpost_id == new_outpost_id
                                && sr.resources_id == new_resource_id) != null)
                    {
                        string eo = $"Для форпоста {row.Cells[MyHelper.strOutpostId].FormattedValue} " +
                                    $"ресурс {row.Cells[MyHelper.strResourceId].FormattedValue} " +
                                    $"уже существует! Измените или удалите текущую строку!";
                        MessageBox.Show(eo);
                        row.ErrorText = MyHelper.strBadRow + " " + eo;
                        return;
                    }

                    if (new_sr.resources_id != new_resource_id || new_sr.outpost_id != new_outpost_id)
                    {
                        ctx.storage_resources.Remove(new_sr);
                        ctx.SaveChanges();
                        new_sr = new storage_resources();

                        new_sr.outpost_id = new_outpost_id;
                        new_sr.resources_id = new_resource_id;
                        new_sr.count = new_count;
                        new_sr.accumulation_speed = new_accumulation_speed;
                        ctx.storage_resources.Add(new_sr);
                    }
                    else
                    {
                        new_sr.count = new_count;
                        new_sr.accumulation_speed = new_accumulation_speed;
                    }

                    ctx.SaveChanges();
                    row.Cells[MyHelper.strSource].Value = new_sr;
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        public override void UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (e.Row.HaveSource())
            {
                try
                {
                    using (var ctx = new OutpostDataContext())
                    {
                        var sr = (storage_resources)e.Row.Cells[MyHelper.strSource].Value;
                        ctx.storage_resources.Attach(sr);
                        ctx.storage_resources.Remove(sr);
                        ctx.SaveChanges();
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
            }
        }
    }
}
