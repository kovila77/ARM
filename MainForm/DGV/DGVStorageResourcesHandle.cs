using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainForm.DGV
{
    class DGVStorageResourcesHandle : DGVHandle
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
        private DataGridViewComboBoxColumn _cbcResorces = new DataGridViewComboBoxColumn()
        {
            Name = "resources_id",
            HeaderText = "Ресурс",
            DisplayMember = "resources_name",
            ValueMember = "resources_id",
            DataPropertyName = "resources_id",
            FlatStyle = FlatStyle.Flat
        };

        public DGVStorageResourcesHandle(DataGridView dgv, DataTable dtOutpost, DataTable dtResources) : base(dgv)
        {
            using (var ctx = new OutpostDataContext())
            {
                ctx.storage_resources.Load();

                _cbcResorces.DataSource = dtResources;
                _cbcOutpost.DataSource = dtOutpost;

                dataTable.Columns.Add("outpost_id", typeof(int));
                dataTable.Columns.Add("resources_id", typeof(int));
                dataTable.Columns.Add("count", typeof(int));
                dataTable.Columns.Add("accumulation_speed", typeof(int));
                dataTable.Columns.Add("Source");
                ctx.storage_resources.ToList().ForEach(x => dataTable.Rows.Add(x.outpost_id, x.resources_id, x.count, x.accumulation_speed, x));

                _dgv.Columns.Add(_cbcResorces);
                _dgv.Columns.Add(_cbcOutpost);
                _dgv.DataSource = dataTable;
            }
            MakeThisColumnVisible(new string[] {
                    "outpost_id",
                    "resources_id",
                    "count",
                    "accumulation_speed",
                });
            //_dgv.UserAddedRow += UserAddedRow;
        }

        public override void UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void Insert(DataGridViewRow row)
        {
            using (var ctx = new OutpostDataContext())
            {
                storage_resources sr = new storage_resources {
                    outpost_id = (int)row.Cells["outpost_id"].Value ,
                    resources_id = (int)row.Cells["resources_id"].Value ,
                    count = (int)row.Cells["count"].Value ,
                    accumulation_speed = (int)row.Cells["accumulation_speed"].Value,
                };
                ctx.storage_resources.Add(sr);
                ctx.SaveChanges();

                row.Cells["Source"].Value = sr;
            }
        }

        protected override void Update(DataGridViewRow row)
        {
            throw new NotImplementedException();
        }
    }
}
