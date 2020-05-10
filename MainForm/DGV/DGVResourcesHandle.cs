using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;
using System.ComponentModel;
using System.Data;
using System.Collections.ObjectModel;

namespace MainForm.DGV
{
    class DGVResourcesHandle : DGVHandle
    {
        private ResourcesDataTableHandler _resourcesDataTableHandler;

        public DGVResourcesHandle(DataGridView dgv, 
            ref ResourcesDataTableHandler resourcesDataTableHandler) : base(dgv)
        {
            this._resourcesDataTableHandler = resourcesDataTableHandler;
        }

        public override void Initialize()
        {
            _dgv.CancelEdit();
            _dgv.Rows.Clear();
            _dgv.Columns.Clear();
            _resourcesDataTableHandler.InitializeDataTableResources();

            _dgv.Columns.Add(MyHelper.strResourceName, "Название ресурса");
            _dgv.Columns.Add(MyHelper.strResourceId, "id");
            _dgv.Columns.Add(MyHelper.strSource, "");

            _dgv.Columns[MyHelper.strResourceName].ValueType = typeof(string);
            _dgv.Columns[MyHelper.strResourceId].ValueType = typeof(int);
            _dgv.Columns[MyHelper.strSource].ValueType = typeof(resource);

            _dgv.Columns[MyHelper.strResourceId].Visible = false;
            _dgv.Columns[MyHelper.strSource].Visible = false;

            try
            {
                using (var ctx = new OutpostDataContext())
                {
                    foreach (var res in ctx.resources)
                    {
                        _dgv.Rows.Add(res.resources_name, res.resources_id, res);
                        _resourcesDataTableHandler.Add(res.resources_id, res.resources_name);
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        //protected void HideColumns()
        //{
        //    MakeThisColumnVisible(new string[] {
        //            "resources_name"
        //        });
        //}

        protected override void Insert(DataGridViewRow row)
        {
            try
            {
                using (var ctx = new OutpostDataContext())
                {
                    string new_resources_name = (string)row.Cells[MyHelper.strResourceName].Value;

                    if (ctx.resources.AsEnumerable().FirstOrDefault(res => res.resources_name.ToLower() == new_resources_name.ToLower()) != null)
                    {
                        string eo = $"Ресурс {new_resources_name} уже существует!";
                        MessageBox.Show(eo);
                        row.ErrorText = MyHelper.strBadRow + " " + eo;
                        return;
                    }

                    var new_res = new resource();
                    new_res.resources_name = new_resources_name;
                    ctx.resources.Add(new_res);
                    ctx.SaveChanges();
                    row.Cells[MyHelper.strSource].Value = new_res;
                    row.Cells[MyHelper.strResourceId].Value = new_res.resources_id;
                    _resourcesDataTableHandler.Add(new_res.resources_id, new_res.resources_name);
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                row.ErrorText = MyHelper.strError + err.Message;
            }
        }

        protected override void Update(DataGridViewRow row)
        {
            try
            {
                using (var ctx = new OutpostDataContext())
                {
                    var new_res = ctx.resources.Find((int)row.Cells[MyHelper.strResourceId].Value);

                    string new_resources_name = (string)row.Cells[MyHelper.strResourceName].Value;

                    if (ctx.resources.AsEnumerable().FirstOrDefault(res => res.resources_id != new_res.resources_id && res.resources_name.ToLower() == new_resources_name.ToLower()) != null)
                    {
                        string eo = $"Ресурс {new_resources_name} уже существует!";
                        MessageBox.Show(eo);
                        row.ErrorText = MyHelper.strBadRow + " " + eo;
                        return;
                    }

                    new_res.resources_name = new_resources_name;

                    ctx.SaveChanges();
                    _resourcesDataTableHandler.Change(new_res.resources_id, new_res.resources_name);
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                row.ErrorText = MyHelper.strError + err.Message;
            }
        }

        public override void UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (e.Row.HaveSource())
            {
                try
                {
                    using (var ctx = new OutpostDataContext())
                    {
                        var res = ctx.resources.Find(((resource)e.Row.Cells[MyHelper.strSource].Value).resources_id);

                        if (res.buildings_resources_consume.Count > 0
                            || res.buildings_resources_produce.Count > 0
                            || res.storage_resources.Count > 0
                            || res.machines_resources_consume.Count > 0)
                        {
                            MessageBox.Show($"Вы не можете удалить ресурс {res.resources_name}, так как он используется");
                            e.Cancel = true;
                            return;
                        }

                        ctx.resources.Remove(res);
                        ctx.SaveChanges();
                        _resourcesDataTableHandler.Remove(res.resources_id);
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                    e.Row.ErrorText = MyHelper.strError + err.Message;
                    e.Cancel = true;
                }
            }
        }

        protected override bool ChekRowAndSayReady(DataGridViewRow row)
        {
            var cellWithPotentialError = _dgv[MyHelper.strResourceName, row.Index];
            if (cellWithPotentialError.FormattedValue.ToString().RmvExtrSpaces() == "")
            {
                cellWithPotentialError.ErrorText = MyHelper.strEmptyCell;
                row.ErrorText = MyHelper.strBadRow;
                return false;
            }
            else
            {
                cellWithPotentialError.ErrorText = "";
                row.ErrorText = "";
                return true;
            }
        }
    }
}
