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
        private DGVBuildingsResourcesHandle _dGVBuildingsResourcesHandle;

        public MainForm()
        {
            InitializeComponent();
            DataTable dt = new DataTable();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _dGVOutpostHandle = new DGVOutpostHandle(dgvO);
            //_dGVBuildingsResourcesHandle = new DGVBuildingsResourcesHandle(dgvBR);

        }   
    }
}
