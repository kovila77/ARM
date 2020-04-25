using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms;
using System.Data.Entity;

namespace MainForm.DGV
{
    class DGVBuildingsResourcesHandle : DGVHandle
    {
        private DataGridViewComboBoxColumn cbcResorces = new DataGridViewComboBoxColumn()
        {
            Name = "rId",
            HeaderText = "Ресурс",
            DisplayMember = "name",
            ValueMember = "id"
        };
        private DataGridViewComboBoxColumn cbcBuldings = new DataGridViewComboBoxColumn()
        {
            Name = "bId",
            HeaderText = "Здание",
            DisplayMember = "name",
            ValueMember = "id"
        };

        public DGVBuildingsResourcesHandle(DataGridView dgv, BindingSource bsBuild, BindingSource bsResources) : base(dgv)
        {
            //_bsB = bsBuild;
            //_bsR = bsResources; 
            cbcResorces.DataSource = bsBuild;
            cbcBuldings.DataSource = bsResources;
            var connectionString = ConfigurationManager.ConnectionStrings["OutpostDataContext"].ConnectionString;
            using (var c = new NpgsqlConnection(connectionString))
            {
                c.Open();
                var comm = new NpgsqlCommand() { Connection = c, CommandText = @"SELECT building_id, resources_id, consume_speed, produce_speed
                                                                                FROM public.buildings_resources" };
                var r = comm.ExecuteReader();
                _dgv.Columns.Add(cbcBuldings);
                //_dgv.Columns.Add("building_id", "Здание");
                _dgv.Columns.Add(cbcResorces);
                //_dgv.Columns.Add("resources_id", "Ресурс");
                _dgv.Columns.Add("consume_speed", "Скорость потребления");
                _dgv.Columns.Add("produce_speed", "Скорость произовдства");
                while (r.Read())
                    _dgv.Rows.Add(r["building_id"], r["resources_id"], r["consume_speed"], r["produce_speed"]);
            }
        }

        public override void CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == _dgv.Rows.Count - 1) return;
            using (var ctx = new OutpostDataContext())
            {
                var o = (outpost)_dgv.Rows[e.RowIndex].DataBoundItem;
                ctx.outposts.Attach(o);
                ctx.Entry(o).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }
    }
}
