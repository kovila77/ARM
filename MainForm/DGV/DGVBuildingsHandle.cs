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

        //public DataGridViewComboBoxColumn cbcOutpost = new DataGridViewComboBoxColumn()
        //{
        //    Name = "outpost_id",
        //    HeaderText = "Форпост",
        //    DisplayMember = "outpost_name",
        //    ValueMember = "outpost_id",
        //    DataPropertyName = "outpost_id",
        //    FlatStyle = FlatStyle.Flat
        //};
        private DataGridViewComboBoxColumnOutpost _cbcOutpost;
        private BuildingsDataTableHandler _buildingsDataTableHandler;

        private ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
        private ToolStripMenuItem убратьЗначениеToolStripMenuItem = new ToolStripMenuItem();
        DataGridViewCellEventArgs mouseLocation;

        public DGVBuildingsHandle(DataGridView dgv, DataGridViewComboBoxColumnOutpost cbcOutpost, ref BuildingsDataTableHandler buildingsDataTableHandler) : base(dgv)
        {
            this._cbcOutpost = cbcOutpost;
            this._buildingsDataTableHandler = buildingsDataTableHandler;

            _dgv.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(CellMouseEnter);
            contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { убратьЗначениеToolStripMenuItem });
            contextMenuStrip.Name = "contextMenuStrip";
            contextMenuStrip.Size = new System.Drawing.Size(181, 48);
            contextMenuStrip.Opening += ContextMenuStrip_Opening;
            убратьЗначениеToolStripMenuItem.Name = "убратьЗначениеToolStripMenuItem";
            убратьЗначениеToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            убратьЗначениеToolStripMenuItem.Text = "Убрать значение";
            убратьЗначениеToolStripMenuItem.Click += new System.EventHandler(this.SetNull);
        }

        public override void Initialize()
        {
            _dgv.CancelEdit();
            _dgv.Rows.Clear();
            _dgv.Columns.Clear();
            _buildingsDataTableHandler.InitializeDataTableBuildings();

            _dgv.Columns.Add(MyHelper.strBuildingName, "Название здания");
            //_dgv.Columns.Add(MyHelper.strOutpostId, "Форпост");
            _dgv.Columns.Add(_cbcOutpost);
            _dgv.Columns.Add(MyHelper.strBuildingId, "id");
            _dgv.Columns.Add(MyHelper.strSource, "src");

            foreach (DataGridViewColumn column in _dgv.Columns)
                column.SortMode = DataGridViewColumnSortMode.Programmatic;

            _dgv.Columns[MyHelper.strBuildingName].ValueType = typeof(string);
            _dgv.Columns[MyHelper.strOutpostId].ValueType = typeof(int);
            _dgv.Columns[MyHelper.strBuildingId].ValueType = typeof(int);
            _dgv.Columns[MyHelper.strSource].ValueType = typeof(building);

            _dgv.Columns[MyHelper.strBuildingId].Visible = false;
            _dgv.Columns[MyHelper.strSource].Visible = false;

            try
            {
                using (var ctx = new OutpostDataContext())
                {
                    foreach (var build in ctx.buildings)
                    {
                        _dgv.Rows.Add(build.building_name, build.outpost_id, build.building_id, build);
                        _buildingsDataTableHandler.Add(build.building_id, build.building_name, build.outpost_id);
                    }
                }
                _dgv.Columns[MyHelper.strOutpostId].ContextMenuStrip = contextMenuStrip;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        //protected void HideColumns()
        //{
        //    MakeThisColumnVisible(new string[] {
        //            "building_name",
        //            "outpost_id",
        //        });
        //}

        protected override bool ChekRowAndSayReady(DataGridViewRow row)
        {
            var cellsWithPotentialErrors = new List<DataGridViewCell> {
                                                   row.Cells[MyHelper.strBuildingName],
                                                 };
            foreach (var cellWithPotentialError in cellsWithPotentialErrors)
            {
                if (cellWithPotentialError.FormattedValue.ToString().RmvExtrSpaces() == "")
                {
                    cellWithPotentialError.ErrorText = MyHelper.strEmptyCell;
                    row.ErrorText = MyHelper.strBadRow;
                }
                else
                {
                    cellWithPotentialError.ErrorText = "";
                }
            }
            if (cellsWithPotentialErrors.FirstOrDefault(cellWithPotentialError => cellWithPotentialError.ErrorText.Length > 0) == null)
                row.ErrorText = "";
            else
                return false;
            return true;
        }

        protected override void Insert(DataGridViewRow row)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            _dgv_RowValidating(_dgv, new DataGridViewCellCancelEventArgs(mouseLocation.ColumnIndex, mouseLocation.RowIndex));
        }
    }
}
