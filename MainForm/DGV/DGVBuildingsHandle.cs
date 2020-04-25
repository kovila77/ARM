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
    class DGVBuildingsHandle : DGVHandle
    {

        private DataGridViewComboBoxColumn _cbcOutpost = new DataGridViewComboBoxColumn()
        {
            Name = "outpost_id",
            HeaderText = "Форпост",
            DisplayMember = "outpost_name",
            ValueMember = "outpost_id",
            DataPropertyName = "outpost_id",
            FlatStyle = FlatStyle.Flat
        };

        public DGVBuildingsHandle(DataGridView dgv, DataTable dtOutpost) : base(dgv)
        {
            using (var ctx = new OutpostDataContext())
            {
                ctx.buildings.Load();

                _cbcOutpost.DataSource = dtOutpost;
                dataTable.Columns.Add("building_id", typeof(int));
                dataTable.Columns.Add("building_name", typeof(string));
                dataTable.Columns.Add("outpost_id", typeof(int));
                dataTable.Columns.Add("Source", typeof(building));
                ctx.buildings.ToList().ForEach(x => dataTable.Rows.Add(x.building_id, x.building_name, x.outpost_id, x));

                _dgv.Columns.Add(_cbcOutpost);
                _dgv.DataSource = dataTable;
            }
            MakeThisColumnVisible(new string[] {
                    "building_name",
                    "outpost_id",
                });
        }

        public override void UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void Insert(DataGridViewRow row)
        {
            throw new NotImplementedException();

        }

        protected override bool RowReady(DataGridViewRow row)
        {
            throw new NotImplementedException();
        }

        protected override void Update(DataGridViewRow row)
        {
            using (var ctx = new OutpostDataContext())
            {
                building b = ctx.buildings.Find((int)row.Cells["building_id"].Value);
                b.outpost_id = (int?)row.Cells["outpost_id"].Value;
                b.building_name = (string)row.Cells["building_name"].FormattedValue;
                //ctx.Entry(b).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }
    }
}
