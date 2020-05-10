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
            return row.Cells["building_id"].Value != DBNull.Value
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
                var newbuilding_id = (int)row.Cells["building_id"].Value;
                var newresources_id = (int)row.Cells["resources_id"].Value;
                if (brp.building_id != newbuilding_id
                    || brp.resources_id != newresources_id)
                {
                    var brpExisting = ctx.buildings_resources_produce.Find(newbuilding_id, newresources_id);
                    if (brpExisting != null)
                    {
                        MessageBox.Show($"Для здания {row.Cells["building_id"].FormattedValue} " +
                            $"потребляемый ресурс {row.Cells["resources_id"].FormattedValue} " +
                            $"уже существует! Измените или удалите текущую строку!");
                        row.ErrorText = "Ошибка!";
                        return;
                    }
                    ctx.buildings_resources_produce.Remove(brp);
                    ctx.SaveChanges();
                    brp = new buildings_resources_produce
                    {
                        building_id = newbuilding_id,
                        resources_id = newresources_id,
                        produce_speed = (int)row.Cells["produce_speed"].Value
                    };
                    ctx.buildings_resources_produce.Add(brp);
                }
                else
                {
                    brp.produce_speed = (int)row.Cells["produce_speed"].Value;
                }
                row.Cells["Source"].Value = brp;
                row.ErrorText = "";
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
            row.Cells["Source"].Value = DBNull.Value;
        }
    }
}
