using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainForm.DGV
{
    abstract class DGVHandle : IDisposable
    {
        protected DataGridView _dgv;
        protected bool isCurrentRowDirty;
        //public DataTable dataTable;

        public DGVHandle(DataGridView dgv)
        {
            //dataTable = new DataTable();
            _dgv = dgv;
            //if (_dgv.Rows.Count > 0)
            //_dgv.Rows.Clear();

            _dgv.DefaultCellStyle.NullValue = null;
            _dgv.CellValidating += CellValidating;
            _dgv.CellEndEdit += CellEndEdit;
            _dgv.UserDeletingRow += UserDeletingRow;
            _dgv.RowValidating += _dgv_RowValidating;
        }

        public abstract void Initialize();

        protected abstract bool ChekRowAndSayReady(DataGridViewRow row);

        protected abstract void Insert(DataGridViewRow row);

        protected abstract void Update(DataGridViewRow row);

        //public void MakeThisColumnVisible(string[] columnNames)
        //{
        //    List<DataGridViewColumn> columnsToSee = new List<DataGridViewColumn>();
        //    foreach (var cn in columnNames)
        //        columnsToSee.Add(_dgv.Columns[cn]);
        //    foreach (DataGridViewColumn c in _dgv.Columns)
        //        c.Visible = columnsToSee.Contains(c);
        //    foreach (var column in columnsToSee)
        //    {
        //        switch (column.Name)
        //        {
        //            case "outpost_id":
        //                column.HeaderText = "Форпост"; break;
        //            case "building_id":
        //                column.HeaderText = "Здание"; break;
        //            case "resources_id":
        //                column.HeaderText = "Ресурс"; break;
        //            case "produce_speed":
        //                column.HeaderText = "Скорость производства"; break;
        //            case "consume_speed":
        //                column.HeaderText = "Скорость потребления"; break;
        //            case "count":
        //                column.HeaderText = "Количество"; break;
        //            case "resources_name":
        //                column.HeaderText = "Ресурс"; break;
        //            case "building_name":
        //                column.HeaderText = "Здание"; break;
        //            case "outpost_name":
        //                column.HeaderText = "Форпост"; break;
        //            case "outpost_economic_value":
        //                column.HeaderText = "Экономическая ценность"; break;
        //            case "outpost_coordinate_x":
        //                column.HeaderText = "Координата x"; break;
        //            case "outpost_coordinate_y":
        //                column.HeaderText = "Координата y"; break;
        //            case "outpost_coordinate_z":
        //                column.HeaderText = "Координата z"; break;
        //            case "accumulation_speed":
        //                column.HeaderText = "Скорость накопления"; break;
        //        }
        //    }
        //}

        public void CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            //var row = dgvOutposts.Rows[e.RowIndex];
            if (_dgv.Rows[e.RowIndex].IsNewRow || !_dgv[e.ColumnIndex, e.RowIndex].IsInEditMode) return;

            var cell = _dgv[e.ColumnIndex, e.RowIndex];
            var cellFormatedValue = e.FormattedValue.ToString().RmvExtrSpaces();
            int t = -1;

            if (_dgv.Columns[e.ColumnIndex].CellType != typeof(DataGridViewComboBoxCell)
                && _dgv.Columns[e.ColumnIndex].ValueType == typeof(Int32)
                && !int.TryParse(cellFormatedValue, out t))
            {
                if (cellFormatedValue == "" || MessageBox.Show(MyHelper.strUncorrectIntValueCell + $"\n\"{cellFormatedValue}\"\nОтменить изменения?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    _dgv.CancelEdit();
                else
                    e.Cancel = true;
                return;
            }
            else if (cell.OwningColumn.Name == MyHelper.strConsumeSpeed && t < 0)
            {
                if (MessageBox.Show(MyHelper.strUncorrectIntValueZeroCell + $"\n\"{cellFormatedValue}\"\nОтменить изменения?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    _dgv.CancelEdit();
                else
                    e.Cancel = true;
                return;
            }
            else
            {
                cell.ErrorText = "";
            }
        }


        public void CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (_dgv.Rows[e.RowIndex].IsNewRow
                || !_dgv.Columns[e.ColumnIndex].Visible) return;

            //var row = dgvResources.Rows[e.RowIndex];
            var cell = _dgv[e.ColumnIndex, e.RowIndex];
            var cellFormatedValue = cell.FormattedValue.ToString().RmvExtrSpaces();

            if (cell.ValueType == typeof(String))
            {
                cell.Value = cellFormatedValue;
            }
        }

        protected void _dgv_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            var row = _dgv.Rows[e.RowIndex];
            if (row.IsNewRow || !_dgv.IsCurrentRowDirty) return;
            //var cell = dgvOutposts[e.ColumnIndex, e.RowIndex];
            //var cellFormatedValue = cell.FormattedValue.ToString().RmvExtrSpaces();

            // Проверка можно ли фиксировать строку
            if (!ChekRowAndSayReady(row)) return;

            if (row.HaveSource())
            {
                Update(row);
            }
            else
            {
                Insert(row);
            }
        }

        public abstract void UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e);

        public void CancelEdit()
        {
            _dgv.CancelEdit();
        }

        public void ClearChanges()
        {
            foreach (DataGridViewRow row in _dgv.Rows)
            {
                if (row.ErrorText != "")
                {
                    if (row.Cells["Source"].Value == DBNull.Value)
                        _dgv.Rows.Remove(row);
                    else
                    {
                        var src = row.Cells["Source"].Value;
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            if (cell.OwningColumn.Visible)
                            {
                                cell.Value = src.GetType().GetProperty(cell.OwningColumn.Name).GetValue(src);
                                row.ErrorText = "";
                            }
                        }
                    }
                }
            }
        }

        //public bool RowHaveSource(DataGridViewRow row)
        //{
        //    return !(row.Cells["Source"].Value == null || row.Cells["Source"].Value == DBNull.Value);
        //}

        public virtual void Dispose()
        {
            _dgv.CellValidating -= CellValidating;
            _dgv.CellEndEdit -= CellEndEdit;
            _dgv.UserDeletingRow -= UserDeletingRow;
            _dgv.RowValidating -= _dgv_RowValidating;
        }
    }
}
