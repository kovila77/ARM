using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainForm
{
    class DataGridViewComboBoxColumnResources : DataGridViewComboBoxColumn
    {
        public DataGridViewComboBoxColumnResources(ResourcesDataTableHandler resourcesDataTableHandler) : base()
        {
            this.Name = MyHelper.strResourceId;
            this.HeaderText = "Ресурс";
            this.DisplayMember = MyHelper.strResourceName;
            this.ValueMember = MyHelper.strResourceId;
            this.DataPropertyName = MyHelper.strResourceId;
            this.FlatStyle = FlatStyle.Flat;
            this.DataSource = resourcesDataTableHandler.DtResources;
            resourcesDataTableHandler.ResourcesDataTableChanged += OnDataSourceChanged;
        }

        private void OnDataSourceChanged(DataTable dt)
        {
            this.DataSource = dt;
        }
    }
}
