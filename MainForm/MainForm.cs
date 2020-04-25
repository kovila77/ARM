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
            var _sConnStr = new NpgsqlConnectionStringBuilder
            {
                Host = "192.168.88.101",
                Port = 5432,
                Database = "outpost",
                Username = "postgres",
                Password = "kovila77",
                AutoPrepareMinUsages = 2,
                MaxAutoPrepare = 10,
            }.ConnectionString;


            //using (var c = new NpgsqlConnection (_sConnStr))
            //{
            //    c.Open();
            //    var comm = new NpgsqlCommand() { Connection = c, CommandText = @"select building_id as bi from public.buildings_resources" };
            //    var r = comm.ExecuteReader();
            //    dgvBR.Columns.Add("bi","bi");
            //    while (r.Read())
            //    {
            //        dgvBR.Rows.Add(r["bi"]);
            //    }
            //}
            //_dGVOutpostHandle = new DGVOutpostHandle(dgvBR);
            _dGVBuildingsResourcesHandle = new DGVBuildingsResourcesHandle(dgvBR);
        }
    }
}
