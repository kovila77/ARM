using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainForm.DGV
{
    class DGVBuildingsResourcesHandle : DGVHandle
    {
        public DGVBuildingsResourcesHandle(DataGridView dgv) : base(dgv)
        {
            _dgv = dgv;
            using (var ctx = new OutpostDataContext())
            {
                //ctx.Configuration.ProxyCreationEnabled = false;
                ctx.outposts.Load();
                _dgv.DataSource = ctx.buildings_resources.Local.ToBindingList();
            }
            List<DataGridViewColumn> columnsToSee = new List<DataGridViewColumn>
            {
                _dgv.Columns["building_id"],
                _dgv.Columns["resources_id"],
                _dgv.Columns["consume_speed"],
                _dgv.Columns["produce_speed"],
            };
            foreach (DataGridViewColumn c in _dgv.Columns) if (!columnsToSee.Contains(c)) c.Visible = false;
        }

        public override void CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == _dgv.Rows.Count - 1) return;
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
