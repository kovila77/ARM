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

        public DGVBuildingsResourcesConsumeHandle(DataGridView dgv, ref DataGridViewComboBoxColumnBuildings cbcBuilsings, ref DataGridViewComboBoxColumnResources cbcResources) : base(dgv)
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
            return row.Cells["building_id"].Value != DBNull.Value
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
