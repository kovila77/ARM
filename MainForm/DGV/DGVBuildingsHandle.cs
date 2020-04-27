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

        public DataGridViewComboBoxColumn cbcOutpost = new DataGridViewComboBoxColumn()
        {
            Name = "outpost_id",
            HeaderText = "Форпост",
            DisplayMember = "outpost_name",
            ValueMember = "outpost_id",
            DataPropertyName = "outpost_id",
            FlatStyle = FlatStyle.Flat
        };

        private ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
        private ToolStripMenuItem убратьЗначениеToolStripMenuItem = new ToolStripMenuItem();
        DataGridViewCellEventArgs mouseLocation;

        public DGVBuildingsHandle(DataGridView dgv, DataTable dtOutpost) : base(dgv)
        {
            using (var ctx = new OutpostDataContext())
            {
                ctx.buildings.Load();

                cbcOutpost.DataSource = dtOutpost;
                dataTable.Columns.Add("building_name", typeof(string));
                dataTable.Columns.Add("outpost_id", typeof(int));
                dataTable.Columns.Add("building_id", typeof(int));
                dataTable.Columns.Add("Source", typeof(building));
                ctx.buildings.ToList().ForEach(x => dataTable.Rows.Add(x.building_name, x.outpost_id, x.building_id, x));

                _dgv.Columns.Add(cbcOutpost);
                _dgv.DataSource = dataTable;
            }
            HideColumns();

            dgv.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(CellMouseEnter);
            contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { убратьЗначениеToolStripMenuItem });
            contextMenuStrip.Name = "contextMenuStrip";
            contextMenuStrip.Size = new System.Drawing.Size(181, 48);
            contextMenuStrip.Opening += ContextMenuStrip_Opening;
            убратьЗначениеToolStripMenuItem.Name = "убратьЗначениеToolStripMenuItem";
            убратьЗначениеToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            убратьЗначениеToolStripMenuItem.Text = "Убрать значение";
            убратьЗначениеToolStripMenuItem.Click += new System.EventHandler(this.SetNull);
            _dgv.Columns["outpost_id"].ContextMenuStrip = contextMenuStrip;
        }


        protected void HideColumns()
        {
            MakeThisColumnVisible(new string[] {
                    "building_name",
                    "outpost_id",
                });
        }

        protected override bool RowReady(DataGridViewRow row)
        {
            return base.RowReady(row)
                && row.Cells["building_name"].Value != DBNull.Value
                ;
        }

        protected override void Insert(DataGridViewRow row)
        {
            using (var ctx = new OutpostDataContext())
            {
                building b = new building
                {
                    building_name = row.Cells["building_name"].Value.ToString(),
                };
                if (row.Cells["outpost_id"].Value != DBNull.Value)
                    b.outpost_id = (int)row.Cells["outpost_id"].Value;

                ctx.buildings.Add(b);
                ctx.SaveChanges();

                row.Cells["building_id"].Value = b.building_id;
                row.Cells["Source"].Value = b;
            }
        }

        protected override void Update(DataGridViewRow row)
        {
            using (var ctx = new OutpostDataContext())
            {
                building b = (building)row.Cells["Source"].Value;
                if (row.Cells["outpost_id"].Value != DBNull.Value)
                    b.outpost_id = (int)row.Cells["outpost_id"].Value;
                else
                    b.outpost_id = null;
                b.building_name = row.Cells["building_name"].Value.ToString();
                ctx.Entry(b).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public override void UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            using (var ctx = new OutpostDataContext())
            {
                var row = e.Row;
                building b = (building)row.Cells["Source"].Value;

                if (MessageBox.Show($"Вы уверены, что хотите удалить информацию о здании {b.building_name}?", "Предупреждение!", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    ctx.buildings.Attach(b);
                    ctx.buildings.Remove(b);
                    ctx.SaveChanges();
                    row.Cells["Source"].Value = DBNull.Value;
                }
                else
                {
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_dgv.ReadOnly) e.Cancel = true;
            _dgv.ClearSelection();
            _dgv[mouseLocation.ColumnIndex, mouseLocation.RowIndex].Selected = true;
        }

        private void CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            mouseLocation = e;
        }

        private void SetNull(object sender, EventArgs e)
        {
            _dgv[mouseLocation.ColumnIndex, mouseLocation.RowIndex].Value = DBNull.Value;
            isCurrentRowDirty = true;
            CellEndEdit(sender, new DataGridViewCellEventArgs(mouseLocation.ColumnIndex, mouseLocation.RowIndex));
        }
    }
}
