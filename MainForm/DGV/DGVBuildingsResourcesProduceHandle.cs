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

        public DGVBuildingsResourcesProduceHandle(DataGridView dgv, 
            DataGridViewComboBoxColumnBuildings cbcBuilsings, 
            DataGridViewComboBoxColumnResources cbcResources) : base(dgv)
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
            _dgv.Columns.Add(MyHelper.strProduceSpeed, "Скорость производства");
            _dgv.Columns.Add(MyHelper.strSource, "");

            _dgv.Columns[MyHelper.strBuildingId].ValueType = typeof(int);
            _dgv.Columns[MyHelper.strResourceId].ValueType = typeof(int);
            _dgv.Columns[MyHelper.strProduceSpeed].ValueType = typeof(int);
            _dgv.Columns[MyHelper.strSource].ValueType = typeof(buildings_resources_produce);

            _dgv.Columns[MyHelper.strSource].Visible = false;

            try
            {
                using (var ctx = new OutpostDataContext())
                {
                    foreach (var brc in ctx.buildings_resources_produce)
                    {
                        _dgv.Rows.Add(brc.building_id, brc.resources_id, brc.produce_speed, brc);
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
        //            "produce_speed"
        //        });
        //}

        protected override bool ChekRowAndSayReady(DataGridViewRow row)
        {
            var cellsWithPotentialErrors = new List<DataGridViewCell> {
                                                   row.Cells[MyHelper.strBuildingId],
                                                   row.Cells[MyHelper.strResourceId],
                                                   row.Cells[MyHelper.strProduceSpeed],
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
                    int new_produce_speed = (int)row.Cells[MyHelper.strProduceSpeed].Value;

                    if (ctx.buildings_resources_consume.AsEnumerable().FirstOrDefault(brp =>
                                brp.building_id == new_building_id
                                && brp.resources_id == new_resource_id) != null)
                    {
                        string eo = $"Для здания {row.Cells["building_id"].FormattedValue} " +
                                    $"производимый ресурс {row.Cells["resources_id"].FormattedValue} " +
                                    $"уже существует! Измените или удалите текущую строку!";
                        MessageBox.Show(eo);
                        row.ErrorText = MyHelper.strBadRow + " " + eo;
                        return;
                    }

                    var new_brp = new buildings_resources_produce();
                    new_brp.resources_id = new_resource_id;
                    new_brp.building_id = new_building_id;
                    new_brp.produce_speed = new_produce_speed;

                    ctx.buildings_resources_produce.Add(new_brp);

                    ctx.SaveChanges();

                    row.Cells[MyHelper.strSource].Value = new_brp;
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
                    var new_brp = (buildings_resources_produce)row.Cells[MyHelper.strSource].Value;
                    ctx.buildings_resources_produce.Attach(new_brp);

                    int new_building_id = (int)row.Cells[MyHelper.strBuildingId].Value;
                    int new_resource_id = (int)row.Cells[MyHelper.strResourceId].Value;
                    int new_produce_speed = (int)row.Cells[MyHelper.strProduceSpeed].Value;

                    if (ctx.buildings_resources_produce.AsEnumerable().FirstOrDefault(brp =>
                                brp != new_brp
                                && brp.building_id == new_building_id
                                && brp.resources_id == new_resource_id) != null)
                    {
                        string eo = $"Для здания {row.Cells["building_id"].FormattedValue} " +
                                    $"производимый ресурс {row.Cells["resources_id"].FormattedValue} " +
                                    $"уже существует! Измените или удалите текущую строку!";
                        MessageBox.Show(eo);
                        row.ErrorText = MyHelper.strBadRow + " " + eo;
                        return;
                    }

                    if (new_brp.resources_id != new_resource_id || new_brp.building_id != new_building_id)
                    {
                        ctx.buildings_resources_produce.Remove(new_brp);
                        ctx.SaveChanges();
                        new_brp = new buildings_resources_produce();

                        new_brp.resources_id = new_resource_id;
                        new_brp.building_id = new_building_id;
                        new_brp.produce_speed = new_produce_speed;
                        ctx.buildings_resources_produce.Add(new_brp);
                    }
                    else
                    {
                        new_brp.produce_speed = new_produce_speed;
                    }

                    ctx.SaveChanges();
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
                        var brp = (buildings_resources_produce)e.Row.Cells[MyHelper.strSource].Value;
                        ctx.buildings_resources_produce.Attach(brp);
                        ctx.buildings_resources_produce.Remove(brp);
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
