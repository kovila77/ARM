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

        public DGVResourcesHandle(DataGridView dgv) : base(dgv)
        {
            using (var ctx = new OutpostDataContext())
            {
                ctx.resources.Load();

                dataTable.Columns.Add("resources_id", typeof(int));
                dataTable.Columns.Add("resources_name", typeof(string));
                dataTable.Columns.Add("Source", typeof(resource));
                ctx.resources.ToList().ForEach(x => dataTable.Rows.Add(x.resources_id, x.resources_name, x));
                _dgv.DataSource = dataTable;
            }
            MakeThisColumnVisible(new string[] {
                    "resources_name"
                });
            //_dgv.UserAddedRow += UserAddedRow;
        }

        protected override void Insert(DataGridViewRow row)
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

        protected override void Update(DataGridViewRow row)
        {
            using (var ctx = new OutpostDataContext())
            {
                resource r = (resource)row.Cells["Source"].Value;
                ctx.Entry(r).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public override void UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            using (var ctx = new OutpostDataContext())
            {
                var row = e.Row;
                resource r = (resource)row.Cells["Source"].Value;

                ctx.resources.Attach(r);

                if (r.buildings_resources_consume.Count > 0
                    || r.buildings_resources_produce.Count > 0
                    || r.storage_resources.Count > 0
                    || r.machines_resources_consume.Count > 0)
                {
                    MessageBox.Show("Вы не можете удалить данный ресурс, так как он используется");
                    return; 
                }
                ctx.resources.Remove(r);
                ctx.SaveChanges();
            }
        }
    }
}
