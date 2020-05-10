using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainForm.DGVComboBoxColumn
{
    class DataGridViewComboBoxColumnOutpost : DataGridViewComboBoxColumn
    {
        public DataGridViewComboBoxColumnOutpost(OutpostDataTableHandler dataTableHandler) : base()
        {
            this.Name = MyHelper.strOutpostId;
            this.HeaderText = "Форпост";
            this.DisplayMember = MyHelper.strOutpostName;
            this.ValueMember = MyHelper.strOutpostId;
            this.DataPropertyName = MyHelper.strOutpostId;
            this.FlatStyle = FlatStyle.Flat;
            this.DataSource = dataTableHandler.DtOutposts;
            dataTableHandler.OutpostDataTableChanged += OnDataSourceChanged;
        }

        private void OnDataSourceChanged(DataTable dt)
        {
            this.DataSource = dt;
        }
    }
}
