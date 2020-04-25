using MainForm.DGV;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainForm
{
    class DGVOutpostHandle: DGVHandle
    {
        public DGVOutpostHandle(DataGridView dgv): base (dgv)
        {
            //_dgv = dgvO;
            using (var ctx = new OutpostDataContext())
            {
                //ctx.Configuration.ProxyCreationEnabled = false;
                ctx.outposts.Load();
                _dgv.DataSource = ctx.outposts.Local.ToBindingList();
            }
            List<DataGridViewColumn> columnsToSee = new List<DataGridViewColumn>
            {
                _dgv.Columns["outpost_name"],
                _dgv.Columns["outpost_economic_value"],
                _dgv.Columns["outpost_coordinate_x"],
                _dgv.Columns["outpost_coordinate_y"],
                _dgv.Columns["outpost_coordinate_z"],
            };
            foreach (DataGridViewColumn c in _dgv.Columns) if (!columnsToSee.Contains(c)) c.Visible = false;
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
