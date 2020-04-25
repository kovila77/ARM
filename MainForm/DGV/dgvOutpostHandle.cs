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
    class DGVOutpostHandle : DGVHandle
    {
        public DGVOutpostHandle(DataGridView dgv) : base(dgv)
        {
            using (var ctx = new OutpostDataContext())
            {
                //ctx.Configuration.ProxyCreationEnabled = false;
                ctx.outposts.Load();
                _dgv.DataSource = ctx.outposts.Local.ToBindingList();
            }
            using (var ctx = new OutpostDataContext())
            {
                ctx.outposts.Load();

                dataTable.Columns.Add("outpost_id", typeof(int));
                dataTable.Columns.Add("outpost_name", typeof(string));
                dataTable.Columns.Add("outpost_economic_value", typeof(int));
                dataTable.Columns.Add("outpost_coordinate_x", typeof(int));
                dataTable.Columns.Add("outpost_coordinate_y", typeof(int));
                dataTable.Columns.Add("outpost_coordinate_z", typeof(int));
                dataTable.Columns.Add("Source", typeof(outpost));
                ctx.outposts.ToList().ForEach(otpst => dataTable.Rows.Add(
                                                        otpst.outpost_id,
                                                        otpst.outpost_name,
                                                        otpst.outpost_economic_value,
                                                        otpst.outpost_coordinate_x,
                                                        otpst.outpost_coordinate_y,
                                                        otpst.outpost_coordinate_z,
                                                        otpst));
                _dgv.DataSource = dataTable;
            }
            MakeThisColumnVisible(new string[] {
                    "outpost_name",
                    "outpost_economic_value",
                    "outpost_coordinate_x",
                    "outpost_coordinate_y",
                    "outpost_coordinate_z"
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
            throw new NotImplementedException();
        }

        //public override void CellEndEdit(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (_dgv.Rows[e.RowIndex].IsNewRow || !isCurrentRowDirty) return;
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
