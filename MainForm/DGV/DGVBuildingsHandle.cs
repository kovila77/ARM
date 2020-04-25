using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;

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
        };

        public DGVBuildingsHandle(DataGridView dgv) : base(dgv)
        {
            using (var ctx = new OutpostDataContext())
            {
                //ctx.Configuration.ProxyCreationEnabled = false;
                ctx.buildings.Load();
                ctx.outposts.Load();

                _cbcOutpost.DataSource = ctx.outposts.Local.ToBindingList();
                _dgv.Columns.Add(_cbcOutpost);

                _dgv.DataSource = ctx.buildings.Local.ToBindingList();
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
