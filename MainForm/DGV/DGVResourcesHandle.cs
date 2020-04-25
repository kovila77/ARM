using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;

namespace MainForm.DGV
{
    class DGVResourcesHandle : DGVHandle
    {
        public DGVResourcesHandle(DataGridView dgv) : base(dgv)
        {
            //_dgv = dgvO;
            using (var ctx = new OutpostDataContext())
            {
                //ctx.Configuration.ProxyCreationEnabled = false;
                ctx.outposts.Load();
                _dgv.DataSource = ctx.outposts.Local.ToBindingList();
            }
            MakeThisColumnVisible(new string[] {
                    "outpost_name",
                    "outpost_economic_value",
                    "outpost_coordinate_x",
                    "outpost_coordinate_y",
                    "outpost_coordinate_z"
                });
        }

        public override void CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (_dgv.Rows[e.RowIndex].IsNewRow) return;
            using (var ctx = new OutpostDataContext())
            {
                var o = (outpost)_dgv.Rows[e.RowIndex].DataBoundItem;
                ctx.outposts.Attach(o);
                ctx.Entry(o).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }
    }
}
