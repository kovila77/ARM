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
        //protected bool isCurrentRowDirty;

        public DGVHandle(DataGridView dgv)
        {
            _dgv = dgv;

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
            if ((row.IsNewRow || !_dgv.IsCurrentRowDirty) && sender.GetType() != typeof(ContextMenuStrip)) return;
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

        public virtual void Dispose()
        {
            _dgv.CellValidating -= CellValidating;
            _dgv.CellEndEdit -= CellEndEdit;
            _dgv.UserDeletingRow -= UserDeletingRow;
            _dgv.RowValidating -= _dgv_RowValidating;
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

    }
}
