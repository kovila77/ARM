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
                        _buildingsDataTableHandler.Add(build.building_id, build.building_name);
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
            var cellWithPotentialError = _dgv[MyHelper.strBuildingName, row.Index];
            if (cellWithPotentialError.FormattedValue.ToString().RmvExtrSpaces() == "")
            {
                cellWithPotentialError.ErrorText = MyHelper.strEmptyCell;
                row.ErrorText = MyHelper.strBadRow;
                return false;
            }
            else
            {
                cellWithPotentialError.ErrorText = "";
                row.ErrorText = "";
                return true;
            }
        }

        protected override void Insert(DataGridViewRow row)
        {
            try
            {
                using (var ctx = new OutpostDataContext())
                {
                    string new_building_name = (string)row.Cells[MyHelper.strBuildingName].Value;
                    var new_outpost_id = row.Cells[MyHelper.strOutpostId].Value;

                    if (ctx.buildings.AsEnumerable().FirstOrDefault(b => b.building_name.ToLower() == new_building_name.ToLower()) != null)
                    {
                        string eo = $"Здание {new_building_name} уже существует!";
                        MessageBox.Show(eo);
                        row.ErrorText = MyHelper.strBadRow + " " + eo;
                        return;
                    }

                    var new_building = new building();
                    new_building.building_name = new_building_name;
                    new_building.outpost_id = (int?)new_outpost_id;

                    ctx.buildings.Add(new_building);
                    ctx.SaveChanges();

                    row.Cells[MyHelper.strSource].Value = new_building;
                    row.Cells[MyHelper.strBuildingId].Value = new_building.building_id;
                    _buildingsDataTableHandler.Add(new_building.building_id, new_building.building_name);
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        protected override void Update(DataGridViewRow row)
        {
            try
            {
                using (var ctx = new OutpostDataContext())
                {
                    var new_building = ctx.buildings.Find((int)row.Cells[MyHelper.strBuildingId].Value);

                    string new_building_name = (string)row.Cells[MyHelper.strBuildingName].Value;
                    var new_outpost_id = row.Cells[MyHelper.strOutpostId].Value;

                    if (ctx.buildings.AsEnumerable().FirstOrDefault(b => b.building_id != new_building.building_id && b.building_name.ToLower() == new_building_name.ToLower()) != null)
                    {
                        string eo = $"Здание {new_building_name} уже существует!";
                        MessageBox.Show(eo);
                        row.ErrorText = MyHelper.strBadRow + " " + eo;
                        return;
                    }

                    new_building.building_name = new_building_name;
                    new_building.outpost_id = new_outpost_id == null || new_outpost_id == DBNull.Value ? new int?() : new int?((int)new_outpost_id);//(int?)new_outpost_id;

                    ctx.SaveChanges();
                    _buildingsDataTableHandler.Change(new_building.building_id, new_building.building_name);
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        public override void UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (e.Row.HaveSource())
            {
                try
                {
                    using (var ctx = new OutpostDataContext())
                    {
                        var building = ctx.buildings.Find(((building)e.Row.Cells[MyHelper.strSource].Value).building_id);

                        if (building.buildings_resources_consume.Count > 0
                            || building.buildings_resources_produce.Count > 0)
                        {
                            MessageBox.Show($"Вы не можете удалить здание {building.building_name}, так как оно используется в других таблицах");
                            e.Cancel = true;
                            return;
                        }

                        if (MessageBox.Show($"Вы уверены, что хотите удалить информацию о здании {building.building_name}?", "Предупреждение!", MessageBoxButtons.OKCancel) == DialogResult.OK)
                        {
                            ctx.buildings.Attach(building);
                            ctx.buildings.Remove(building);
                            ctx.SaveChanges();
                            _buildingsDataTableHandler.Remove(building.building_id);
                        }
                        else
                        {
                            e.Cancel = true;
                            return;
                        }
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
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
            _dgv_RowValidating(contextMenuStrip, new DataGridViewCellCancelEventArgs(mouseLocation.ColumnIndex, mouseLocation.RowIndex));
        }
    }
}
