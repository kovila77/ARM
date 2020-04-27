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
        public DataTable dataTable;

        public DGVHandle(DataGridView dgv)
        {
            dataTable = new DataTable();
            _dgv = dgv;
            //if (_dgv.Rows.Count > 0)
            //_dgv.Rows.Clear();
            if (_dgv.Columns.Count > 0)
                _dgv.Columns.Clear();
            _dgv.CellValidating += CellValidating;
            _dgv.CellEndEdit += CellEndEdit;
            _dgv.UserDeletingRow += UserDeletingRow;
        }

        public void MakeThisColumnVisible(string[] columnNames)
        {
            List<DataGridViewColumn> columnsToSee = new List<DataGridViewColumn>();
            foreach (var cn in columnNames)
                columnsToSee.Add(_dgv.Columns[cn]);
            foreach (DataGridViewColumn c in _dgv.Columns)
                c.Visible = columnsToSee.Contains(c);
            foreach (var column in columnsToSee)
            {
                switch (column.Name)
                {
                    case "outpost_id":
                        column.HeaderText = "Форпост"; break;
                    case "building_id":
                        column.HeaderText = "Здание"; break;
                    case "resources_id":
                        column.HeaderText = "Ресурс"; break;
                    case "produce_speed":
                        column.HeaderText = "Скорость производства"; break;
                    case "consume_speed":
                        column.HeaderText = "Скорость потребления"; break;
                    case "count":
                        column.HeaderText = "Количество"; break;
                    case "resources_name":
                        column.HeaderText = "Ресурс"; break;
                    case "building_name":
                        column.HeaderText = "Здание"; break;
                    case "outpost_name":
                        column.HeaderText = "Форпост"; break;
                    case "outpost_economic_value":
                        column.HeaderText = "Экономическая ценность"; break;
                    case "outpost_coordinate_x":
                        column.HeaderText = "Координата x"; break;
                    case "outpost_coordinate_y":
                        column.HeaderText = "Координата y"; break;
                    case "outpost_coordinate_z":
                        column.HeaderText = "Координата z"; break;
                    case "accumulation_speed":
                        column.HeaderText = "Скорость накопления"; break;
                }
            }
        }

        public void CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (_dgv.Rows[e.RowIndex].IsNewRow) return;
            bool canCommit = true;
            var cell = _dgv[e.ColumnIndex, e.RowIndex];
            if (cell.OwningColumn.Visible)
            {

                if (_dgv.Columns[e.ColumnIndex].CellType == typeof(DataGridViewComboBoxCell))
                {
                    //if (_dgv[e.ColumnIndex, e.RowIndex].Value == null || _dgv[e.ColumnIndex, e.RowIndex].Value == DBNull.Value)
                    if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
                    {
                        if (!(this.GetType() == typeof(DGVBuildingsHandle) && _dgv.Columns[e.ColumnIndex].Name == "outpost_id"))
                            canCommit = false;
                    }
                }
                else
                {
                    if (_dgv.Columns[e.ColumnIndex].ValueType == typeof(Int32))
                    {
                        int t;
                        if (e.FormattedValue.ToString().Trim() == "" || !int.TryParse(e.FormattedValue.ToString(), out t) || t < 0)
                        {
                            canCommit = false;
                        }
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(e.FormattedValue.ToString().Trim()))
                        {
                            canCommit = false;
                        }
                    }
                }
            }
            if (!canCommit) CancelEdit();
            isCurrentRowDirty = _dgv.IsCurrentRowDirty;
        }

        protected virtual bool RowReady(DataGridViewRow row)
        {
            return true;
        }

        protected abstract void Insert(DataGridViewRow row);

        protected abstract void Update(DataGridViewRow row);

        public void CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (_dgv.Rows[e.RowIndex].IsNewRow || !isCurrentRowDirty) return;

            if (RowReady(_dgv.Rows[e.RowIndex]))
            {
                if (RowHaveSource(_dgv.Rows[e.RowIndex]))
                {
                    Update(_dgv.Rows[e.RowIndex]);
                }
                else
                {
                    Insert(_dgv.Rows[e.RowIndex]);
                }
                if (_dgv.Rows[e.RowIndex].ErrorText.Contains("Строка не зафиксирована!"))
                    _dgv.Rows[e.RowIndex].ErrorText = "";
                //_dgv.Rows[e.RowIndex].ErrorText.Replace("Строка не зафиксирована!", "");
            }
            else
            {
                _dgv.Rows[e.RowIndex].ErrorText = "Строка не зафиксирована!";
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

        public bool RowHaveSource(DataGridViewRow row)
        {
            return !(row.Cells["Source"].Value == null || row.Cells["Source"].Value == DBNull.Value);
        }

        public virtual void Dispose()
        {
            _dgv.CellValidating -= CellValidating;
            _dgv.CellEndEdit -= CellEndEdit;
            _dgv.UserDeletingRow -= UserDeletingRow;
        }
    }
}
