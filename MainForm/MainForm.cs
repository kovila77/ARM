﻿using MainForm.DGV;
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
        private DGVBuildingsResourcesConsumeHandle _dGVBuildingsResourcesConsumeHandle;
        private DGVBuildingsResourcesProduceHandle _dGVBuildingsResourcesProduceHandle;
        private DGVStorageResourcesHandle _dGVStorageResourcesHandle;
        private DataTable _dtR;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            InitializeDGV();
        }

        private void InitializeDGV()
        {
            _dGVOutpostHandle = new DGVOutpostHandle(dgvO);
            _dGVResourcesHandle = new DGVResourcesHandle(dgvR);
            _dGVBuildingsHandle = new DGVBuildingsHandle(dgvB, _dGVOutpostHandle.dataTable);
            _dGVBuildingsResourcesHandle = new DGVBuildingsResourcesHandle(dgvBR,
                                                                    _dGVBuildingsHandle.dataTable,
                                                                    _dGVResourcesHandle.dataTable);
            _dGVBuildingsResourcesConsumeHandle = new DGVBuildingsResourcesConsumeHandle(dgvBRC,
                                                                    _dGVBuildingsHandle.dataTable,
                                                                    _dGVResourcesHandle.dataTable);
            _dGVBuildingsResourcesProduceHandle = new DGVBuildingsResourcesProduceHandle(dgvBRP,
                                                                    _dGVBuildingsHandle.dataTable,
                                                                    _dGVResourcesHandle.dataTable);
            _dGVStorageResourcesHandle = new DGVStorageResourcesHandle(dgvS,
                                                                    _dGVOutpostHandle.dataTable,
                                                                    _dGVResourcesHandle.dataTable);
        }

        public void ReloadO()
        {
            _dGVOutpostHandle = new DGVOutpostHandle(dgvO);
            _dGVBuildingsHandle.cbcOutpost.DataSource = _dGVOutpostHandle.dataTable;
            _dGVStorageResourcesHandle.cbcOutpost.DataSource = _dGVOutpostHandle.dataTable;
        }
        public void ReloadR()
        {
            _dGVResourcesHandle = new DGVResourcesHandle(dgvO);
            _dGVBuildingsResourcesHandle.cbcResorces.DataSource = _dGVResourcesHandle.dataTable;
            _dGVBuildingsResourcesConsumeHandle.cbcResorces.DataSource = _dGVResourcesHandle.dataTable;
            _dGVBuildingsResourcesProduceHandle.cbcResorces.DataSource = _dGVResourcesHandle.dataTable;
            _dGVStorageResourcesHandle.cbcResorces.DataSource = _dGVResourcesHandle.dataTable;
        }
        public void ReloadB()
        {
            _dGVBuildingsHandle = new DGVBuildingsHandle(dgvB, _dGVOutpostHandle.dataTable);
            _dGVBuildingsResourcesHandle.cbcResorces.DataSource = _dGVBuildingsHandle.dataTable;
            _dGVBuildingsResourcesConsumeHandle.cbcResorces.DataSource = _dGVBuildingsHandle.dataTable;
            _dGVBuildingsResourcesProduceHandle.cbcResorces.DataSource = _dGVBuildingsHandle.dataTable;
        }
        public void ReloadBR()
        {
            _dGVBuildingsResourcesHandle = new DGVBuildingsResourcesHandle(dgvBR,
                                                                    _dGVBuildingsHandle.dataTable,
                                                                    _dGVResourcesHandle.dataTable);
        }
        public void ReloadBRC()
        {
            _dGVBuildingsResourcesConsumeHandle = new DGVBuildingsResourcesConsumeHandle(dgvBRC,
                                                                    _dGVBuildingsHandle.dataTable,
                                                                    _dGVResourcesHandle.dataTable);
        }
        public void ReloadBRP()
        {
            _dGVBuildingsResourcesProduceHandle = new DGVBuildingsResourcesProduceHandle(dgvBRP,
                                                                    _dGVBuildingsHandle.dataTable,
                                                                    _dGVResourcesHandle.dataTable);
        }
        public void ReloadSR()
        {
            _dGVStorageResourcesHandle = new DGVStorageResourcesHandle(dgvS,
                                                                    _dGVOutpostHandle.dataTable,
                                                                    _dGVResourcesHandle.dataTable);
        }
        public void Reload(object sender, EventArgs e)
        {

        }

        private void FullReload(object sender, EventArgs e)
        {
            InitializeComponent();
        }
    }
}
