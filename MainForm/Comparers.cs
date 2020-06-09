using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainForm
{

    public class RowComparerFormattedValue : System.Collections.IComparer
    {
        private static int sortOrderModifier = 1;
        private int columnIndex;

        public RowComparerFormattedValue(SortOrder sortOrder, int columnIndex)
        {
            this.columnIndex = columnIndex;
            if (sortOrder == SortOrder.Descending)
            {
                sortOrderModifier = -1;
            }
            else if (sortOrder == SortOrder.Ascending)
            {
                sortOrderModifier = 1;
            }
        }

        public int Compare(object x, object y)
        {
            DataGridViewRow row1 = (DataGridViewRow)x;
            DataGridViewRow row2 = (DataGridViewRow)y;

            int CompareResult = System.String.Compare(
                row1.Cells[columnIndex].FormattedValue.ToString(),
                row2.Cells[columnIndex].FormattedValue.ToString());

            return CompareResult * sortOrderModifier;
        }
    }

    public class RowComparerForInt : System.Collections.IComparer
    {
        private static int sortOrderModifier = 1;
        private int columnIndex;

        public RowComparerForInt(SortOrder sortOrder, int columnIndex)
        {
            this.columnIndex = columnIndex;
            if (sortOrder == SortOrder.Descending)
            {
                sortOrderModifier = -1;
            }
            else if (sortOrder == SortOrder.Ascending)
            {
                sortOrderModifier = 1;
            }
        }

        public int Compare(object x, object y)
        {
            DataGridViewRow row1 = (DataGridViewRow)x;
            DataGridViewRow row2 = (DataGridViewRow)y;
            int v1 = (int)row1.Cells[columnIndex].Value;
            int v2 = (int)row2.Cells[columnIndex].Value;

            int CompareResult = v1 > v2 ? 1 : (v1 == v2 ? 0 : -1);

            return CompareResult * sortOrderModifier;
        }
    }
}
