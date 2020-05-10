using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainForm
{
    class DataGridViewComboBoxColumnBuildings : DataGridViewComboBoxColumn
    {
        private DataTable _dtBuildings = null;

        public DataGridViewComboBoxColumnBuildings(BuildingsDataTableHandler buildingsDataTableHandler) : base()
        {
            this.Name = MyHelper.strBuildingId;
            this.HeaderText = "Здание";
            this.DisplayMember = MyHelper.strBuildingName;
            this.ValueMember = MyHelper.strBuildingId;
            this.DataPropertyName = MyHelper.strBuildingId;
            this.FlatStyle = FlatStyle.Flat;
            this.DataSource = buildingsDataTableHandler.DtBuildings;
            buildingsDataTableHandler.BuildingsDataTableChanged += OnDataSourceChanged;
            //MainForm.BuildingAdded += Add;
            //MainForm.BuildingChanged += Change;
            //MainForm.BuildingDeleted += Remove;
        }

        private void OnDataSourceChanged(DataTable dt)
        {
            this.DataSource = dt;
        }
    }
}
