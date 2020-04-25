using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainForm.DGV
{
    abstract class DGVHandle
    {
        protected DataGridView _dgv;
        protected bool isCurrentRowDirty;
        //protected bool isNewRow;
        public DataTable dataTable = new DataTable();

        //protected int NewRowIndex { get { return _dgv.AllowUserToAddRows ? _dgv.Rows.Count - 1 : _dgv.Rows.Count; } }

        public DGVHandle(DataGridView dgv)
        {
            _dgv = dgv;
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
        }

        public void CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            // if (e.RowIndex == _dgv.Rows.Count - 1) return;
            if (_dgv.Rows[e.RowIndex].IsNewRow) return;
            if (_dgv.Columns[e.ColumnIndex].CellType == typeof(DataGridViewComboBoxCell))
            {
                if (_dgv[e.ColumnIndex, e.RowIndex].Value == null) CancelEdit();
            }
            else
            if (_dgv.Columns[e.ColumnIndex].ValueType == typeof(Int32))
            {
                int t;
                if (e.FormattedValue.ToString().Trim() != "")
                {
                    //_dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value
                    if (!int.TryParse(e.FormattedValue.ToString(), out t) || t < 0)
                    {
                        CancelEdit();
                    }
                }
            }
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
                if (RowHaveSource(_dgv.Rows[e.RowIndex]))
                {
                    Update(_dgv.Rows[e.RowIndex]);
                }
                else
                {
                    Insert(_dgv.Rows[e.RowIndex]);
                }
        }

        public abstract void UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e);

        public void CancelEdit()
        {
            _dgv.CancelEdit();
        }

        public bool RowHaveSource(DataGridViewRow row)
        {
            return !(row.Cells["Source"].Value == null || row.Cells["Source"].Value == DBNull.Value);
        }
    }
}
