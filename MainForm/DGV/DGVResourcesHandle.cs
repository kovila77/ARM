using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;
using System.ComponentModel;
using System.Data;
using System.Collections.ObjectModel;

namespace MainForm.DGV
{
    class DGVResourcesHandle : DGVHandle
    {
        //BindingList<resource> bl = new BindingList<resource>();

        DataTable dt = new DataTable();

        public DGVResourcesHandle(DataGridView dgv) : base(dgv)
        {
            using (var ctx = new OutpostDataContext())
            {
                //ctx.Configuration.ProxyCreationEnabled = false;
                ctx.resources.Load();

                dt.Columns.Add("resources_id", typeof(int));
                dt.Columns.Add("resources_name", typeof(string));
                dt.Columns.Add("Source", typeof(resource));

                //var bs = new BindingSource();
                //bs.DataSource = ctx.resources.Local.ToList();
                //ctx.resources.ToList().ForEach(x => bl.Add(x));
                ctx.resources.ToList().ForEach(x => dt.Rows.Add(x.resources_id, x.resources_name, x));
                //foreach (var r in ctx.resources)
                //{
                //    dt.Rows.Add(r.resources_id,r.resources_name);
                //}
                _dgv.DataSource = dt;
            }
            MakeThisColumnVisible(new string[] {
                    "resources_name"
                });
            //_dgv.UserAddedRow += UserAddedRow;
        }


        private void Insert(DataGridViewRow row)
        {
            using (var ctx = new OutpostDataContext())
            {
                resource res = new resource { resources_name = row.Cells["resources_name"].Value.ToString() };
                ctx.resources.Add(res);
                ctx.SaveChanges();

                row.Cells["resources_id"].Value = res.resources_id;
                row.Cells["Source"].Value = res;
            }
        }

        private void Update(DataGridViewRow row)
        {
            using (var ctx = new OutpostDataContext())
            {
                resource r = (resource)row.Cells["Source"].Value;
                ctx.Entry(r).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public override void CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (_dgv.Rows[e.RowIndex].IsNewRow || !isCurrentRowDirty) return;


            //if (RowHaveSource(_dgv.Rows[e.RowIndex]))
            //{
            //    Update(_dgv.Rows[e.RowIndex]);
            //}
            //else
            //{
            //    Insert(_dgv.Rows[e.RowIndex]);
            //}
        }
    }
}
