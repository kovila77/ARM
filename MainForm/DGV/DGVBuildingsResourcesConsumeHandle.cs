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

        public DGVBuildingsResourcesConsumeHandle(DataGridView dgv, DataGridViewComboBoxColumnBuildings cbcBuilsings, DataGridViewComboBoxColumnResources cbcResources) : base(dgv)
        {
            this._cbcBuilsings = cbcBuilsings;
            this._cbcResources = cbcResources;
            //_dgv.CellBeginEdit += CellBeginEdit;
        }

        public override void Initialize()
        {
            _dgv.CancelEdit();
            _dgv.Rows.Clear();
            _dgv.Columns.Clear();

            _dgv.Columns.Add(_cbcBuilsings);
            _dgv.Columns.Add(_cbcResources);
            _dgv.Columns.Add(MyHelper.strConsumeSpeed, "Скорость потребления");
            _dgv.Columns.Add(MyHelper.strSource, "");

            foreach (DataGridViewColumn column in _dgv.Columns)
                column.SortMode = DataGridViewColumnSortMode.Programmatic;

            _dgv.Columns[MyHelper.strBuildingId].ValueType = typeof(int);
            _dgv.Columns[MyHelper.strResourceId].ValueType = typeof(int);
            _dgv.Columns[MyHelper.strConsumeSpeed].ValueType = typeof(int);
            _dgv.Columns[MyHelper.strSource].ValueType = typeof(buildings_resources_consume);

            _dgv.Columns[MyHelper.strSource].Visible = false;

            try
            {
                using (var ctx = new OutpostDataContext())
                {
                    foreach (var brc in ctx.buildings_resources_consume)
                    {
                        _dgv.Rows.Add(brc.building_id, brc.resources_id, brc.consume_speed, brc);
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        //protected void HideColumns()
        //{
        //    MakeThisColumnVisible(new string[] {
        //            "building_id",
        //            "resources_id",
        //            "consume_speed"
        //        });
        //}

        protected override bool ChekRowAndSayReady(DataGridViewRow row)
        {
            var cellsWithPotentialErrors = new List<DataGridViewCell> {
                                                   row.Cells[MyHelper.strBuildingId],
                                                   row.Cells[MyHelper.strResourceId],
                                                   row.Cells[MyHelper.strConsumeSpeed],
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
                    int new_building_id = (int)row.Cells[MyHelper.strBuildingId].Value;
                    int new_resource_id = (int)row.Cells[MyHelper.strResourceId].Value;
                    int new_consume_speed = (int)row.Cells[MyHelper.strConsumeSpeed].Value;

                    if (ctx.buildings_resources_consume.AsEnumerable().FirstOrDefault(brc =>
                                brc.building_id == new_building_id
                                && brc.resources_id == new_resource_id) != null)
                    {
                        string eo = $"Для здания {row.Cells["building_id"].FormattedValue} " +
                                    $"потребляемый ресурс {row.Cells["resources_id"].FormattedValue} " +
                                    $"уже существует! Измените или удалите текущую строку!";
                        MessageBox.Show(eo);
                        row.ErrorText = MyHelper.strBadRow + " " + eo;
                        return;
                    }

                    var new_brc = new buildings_resources_consume();
                    new_brc.resources_id = new_resource_id;
                    new_brc.building_id = new_building_id;
                    new_brc.consume_speed = new_consume_speed;

                    ctx.buildings_resources_consume.Add(new_brc);

                    ctx.SaveChanges();

                    row.Cells[MyHelper.strSource].Value = new_brc;
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
                    var new_brc = (buildings_resources_consume)row.Cells[MyHelper.strSource].Value;
                    //var tmp = (buildings_resources_consume)row.Cells[MyHelper.strSource].Value;
                    //var new_brc = ctx.buildings_resources_consume.Find(tmp.building_id, tmp.resources_id);
                    ctx.buildings_resources_consume.Attach(new_brc);

                    int new_building_id = (int)row.Cells[MyHelper.strBuildingId].Value;
                    int new_resource_id = (int)row.Cells[MyHelper.strResourceId].Value;
                    int new_consume_speed = (int)row.Cells[MyHelper.strConsumeSpeed].Value;

                    if (ctx.buildings_resources_consume.AsEnumerable().FirstOrDefault(brc =>
                                brc != new_brc
                                && brc.building_id == new_building_id
                                && brc.resources_id == new_resource_id) != null)
                    {
                        string eo = $"Для здания {row.Cells["building_id"].FormattedValue} " +
                                    $"потребляемый ресурс {row.Cells["resources_id"].FormattedValue} " +
                                    $"уже существует! Измените или удалите текущую строку!";
                        MessageBox.Show(eo);
                        row.ErrorText = MyHelper.strBadRow + " " + eo;
                        return;
                    }

                    if (new_brc.resources_id != new_resource_id || new_brc.building_id != new_building_id)
                    {
                        ctx.buildings_resources_consume.Remove(new_brc);
                        ctx.SaveChanges();
                        new_brc = new buildings_resources_consume();

                        new_brc.resources_id = new_resource_id;
                        new_brc.building_id = new_building_id;
                        new_brc.consume_speed = new_consume_speed;
                        ctx.buildings_resources_consume.Add(new_brc);
                    }
                    else
                    {
                        new_brc.consume_speed = new_consume_speed;
                    }

                    ctx.SaveChanges();
                    row.Cells[MyHelper.strSource].Value = new_brc;
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
                        var brc = (buildings_resources_consume)e.Row.Cells[MyHelper.strSource].Value;
                        ctx.buildings_resources_consume.Attach(brc);
                        ctx.buildings_resources_consume.Remove(brc);
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
