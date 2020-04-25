using MainForm.DGV;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainForm
{
    public partial class MainForm : Form
    {
        private DGVOutpostHandle _dGVOutpostHandle;
        private DGVResourcesHandle _dGVResourcesHandle;
        private DGVBuildingsHandle _dGVBuildingsHandle;
        private DGVBuildingsResourcesHandle _dGVBuildingsResourcesHandle;
        private DataTable _dtR;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _dGVOutpostHandle = new DGVOutpostHandle(dgvO);
            _dGVResourcesHandle = new DGVResourcesHandle(dgvR);
            _dGVBuildingsHandle = new DGVBuildingsHandle(dgvB, _dGVOutpostHandle.dataTable);
            _dGVBuildingsResourcesHandle = new DGVBuildingsResourcesHandle(dgvBR,
                                                                  _dGVBuildingsHandle.dataTable,
                                                                   _dGVResourcesHandle.dataTable);

        }

        private void Reload(object sender, EventArgs e)
        {

        }


        //private void dgvR_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        //{
        //    if (isC)
        //        return;
        //    using (var ctx = new OutpostDataContext())
        //    {
        //        var r = (resource)dgvR.Rows[e.Row.Index].DataBoundItem;
        //        if (e.Row.IsNewRow)
        //        {
        //            ctx.resources.Add(r);
        //        }
        //        else
        //        {
        //            ctx.Entry(r).State = EntityState.Modified;
        //        }
        //        ctx.SaveChanges();
        //    }
        //}
    }
}
