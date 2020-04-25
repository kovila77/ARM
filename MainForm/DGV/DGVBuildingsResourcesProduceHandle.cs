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
        private DataGridViewComboBoxColumn _cbcResorces = new DataGridViewComboBoxColumn()
        {
            Name = "resources_id",
            HeaderText = "Ресурс",
            DisplayMember = "resources_name",
            ValueMember = "resources_id",
            DataPropertyName = "resources_id",
            FlatStyle = FlatStyle.Flat
        };
        private DataGridViewComboBoxColumn _cbcBuldings = new DataGridViewComboBoxColumn()
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

                _cbcResorces.DataSource = dtResources;
                _cbcBuldings.DataSource = dtBuildings;

                dataTable.Columns.Add("building_id", typeof(int));
                dataTable.Columns.Add("resources_id", typeof(int));
                dataTable.Columns.Add("produce_speed", typeof(int));
                dataTable.Columns.Add("Source");
                ctx.buildings_resources_produce.ToList().ForEach(x => dataTable.Rows.Add(x.building_id, x.resources_id, x.produce_speed, x));

                _dgv.Columns.Add(_cbcBuldings);
                _dgv.Columns.Add(_cbcResorces);
                _dgv.DataSource = dataTable;
            }
            MakeThisColumnVisible(new string[] {
                    "building_id",
                    "resources_id",
                    "produce_speed"
                });
            //_dgv.UserAddedRow += UserAddedRow;
        }

        public override void UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void Insert(DataGridViewRow row)
        {
            throw new NotImplementedException();
        }

        protected override void Update(DataGridViewRow row)
        {
            throw new NotImplementedException();
        }

        //public override void CellEndEdit(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (_dgv.Rows[e.RowIndex].IsNewRow) return;
        //    using (var ctx = new OutpostDataContext())
        //    {
        //        var o = (outpost)_dgv.Rows[e.RowIndex].DataBoundItem;
        //        ctx.outposts.Attach(o);
        //        ctx.Entry(o).State = EntityState.Modified;
        //        ctx.SaveChanges();
        //    }
        //}
    }
}
