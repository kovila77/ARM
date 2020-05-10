using MainForm.DGV;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainForm
{
    class DGVOutpostHandle : DGVHandle
    {
        private OutpostDataTableHandler _outpostDataTableHandler;

        public DGVOutpostHandle(DataGridView dgv,
            ref OutpostDataTableHandler outpostDataTableHandler) : base(dgv)
        {
            this._outpostDataTableHandler = outpostDataTableHandler;
        }

        public override void Initialize()
        {
            _dgv.CancelEdit();
            _dgv.Rows.Clear();
            _dgv.Columns.Clear();
            _outpostDataTableHandler.InitializeDataTableOutpost();

            _dgv.Columns.Add(MyHelper.strOutpostName, "Название");
            _dgv.Columns.Add(MyHelper.strOutpostEconomicValue, "Экономическая ценность");
            _dgv.Columns.Add(MyHelper.strOutpostCoordinateX, "Координата x");
            _dgv.Columns.Add(MyHelper.strOutpostCoordinateY, "Координата y");
            _dgv.Columns.Add(MyHelper.strOutpostCoordinateZ, "Координата z");
            _dgv.Columns.Add(MyHelper.strOutpostId, "id");
            _dgv.Columns.Add(MyHelper.strSource, "src");

            foreach (DataGridViewColumn column in _dgv.Columns)
                column.SortMode = DataGridViewColumnSortMode.Programmatic;

            _dgv.Columns[MyHelper.strOutpostName].ValueType = typeof(string);
            _dgv.Columns[MyHelper.strOutpostEconomicValue].ValueType = typeof(int);
            _dgv.Columns[MyHelper.strOutpostCoordinateX].ValueType = typeof(int);
            _dgv.Columns[MyHelper.strOutpostCoordinateY].ValueType = typeof(int);
            _dgv.Columns[MyHelper.strOutpostCoordinateZ].ValueType = typeof(int);
            _dgv.Columns[MyHelper.strSource].ValueType = typeof(outpost);

            _dgv.Columns[MyHelper.strOutpostId].Visible = false;
            _dgv.Columns[MyHelper.strSource].Visible = false;

            try
            {
                using (var ctx = new OutpostDataContext())
                {
                    foreach (var otpst in ctx.outposts)
                    {
                        _dgv.Rows.Add(otpst.outpost_name,
                                      otpst.outpost_economic_value,
                                      otpst.outpost_coordinate_x,
                                      otpst.outpost_coordinate_y,
                                      otpst.outpost_coordinate_z,
                                      otpst.outpost_id,
                                      otpst);
                        _outpostDataTableHandler.Add(otpst.outpost_id,
                                         otpst.outpost_name,
                                         otpst.outpost_coordinate_x,
                                         otpst.outpost_coordinate_y,
                                         otpst.outpost_coordinate_z);
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
        //            "outpost_name",
        //            "outpost_economic_value",
        //            "outpost_coordinate_x",
        //            "outpost_coordinate_y",
        //            "outpost_coordinate_z"
        //        });
        //}

        protected override bool ChekRowAndSayReady(DataGridViewRow row)
        {
            var cellsWithPotentialErrors = new List<DataGridViewCell> {
                                                   row.Cells[MyHelper.strOutpostName],
                                                   row.Cells[MyHelper.strOutpostEconomicValue],
                                                   row.Cells[MyHelper.strOutpostCoordinateX],
                                                   row.Cells[MyHelper.strOutpostCoordinateY],
                                                   row.Cells[MyHelper.strOutpostCoordinateZ],
                                                 };
            foreach (var cellWithPotentialError in cellsWithPotentialErrors)
            {
                if (cellWithPotentialError.FormattedValue.ToString().RmvExtrSpaces() == "")
                {
                    cellWithPotentialError.ErrorText = MyHelper.strEmptyCell;
                    row.ErrorText = MyHelper.strBadRow;
                }
                else
                {
                    cellWithPotentialError.ErrorText = "";
                }
            }
            if (cellsWithPotentialErrors.FirstOrDefault(cellWithPotentialError => cellWithPotentialError.ErrorText.Length > 0) == null)
                row.ErrorText = "";
            else
                return false;
            return true;
        }

        protected override void Insert(DataGridViewRow row)
        {
            try
            {
                using (var ctx = new OutpostDataContext())
                {
                    string new_outpost_name = row.Cells[MyHelper.strOutpostName].Value.ToString();
                    int new_outpost_economic_value = (int)row.Cells[MyHelper.strOutpostEconomicValue].Value;
                    int new_outpost_coordinate_x = (int)row.Cells[MyHelper.strOutpostCoordinateX].Value;
                    int new_outpost_coordinate_y = (int)row.Cells[MyHelper.strOutpostCoordinateY].Value;
                    int new_outpost_coordinate_z = (int)row.Cells[MyHelper.strOutpostCoordinateZ].Value;

                    if (ctx.outposts.AsEnumerable().FirstOrDefault(outpst =>
                        outpst.outpost_name == new_outpost_name
                        && outpst.outpost_coordinate_x == new_outpost_coordinate_x
                        && outpst.outpost_coordinate_y == new_outpost_coordinate_y
                        && outpst.outpost_coordinate_z == new_outpost_coordinate_z) != null)
                    {
                        string eo = $"Форпост {new_outpost_name} - {new_outpost_coordinate_x};{new_outpost_coordinate_y};{new_outpost_coordinate_z} идентичен существующему!";
                        MessageBox.Show(eo);
                        row.ErrorText = MyHelper.strBadRow + " " + eo;
                        return;
                    }
                    var o = new outpost();
                    o.outpost_name = new_outpost_name;
                    o.outpost_economic_value = new_outpost_economic_value;
                    o.outpost_coordinate_x = new_outpost_coordinate_x;
                    o.outpost_coordinate_y = new_outpost_coordinate_y;
                    o.outpost_coordinate_z = new_outpost_coordinate_z;

                    ctx.outposts.Add(o);
                    ctx.SaveChanges();

                    row.Cells[MyHelper.strOutpostId].Value = o.outpost_id;
                    row.Cells[MyHelper.strSource].Value = o;
                    _outpostDataTableHandler.Add(o.outpost_id, o.outpost_name, o.outpost_coordinate_x, o.outpost_coordinate_y, o.outpost_coordinate_z);
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        protected override void Update(DataGridViewRow row)
        {
            try
            {
                using (var ctx = new OutpostDataContext())
                {
                    outpost o = ctx.outposts.Find((int)row.Cells[MyHelper.strOutpostId].Value);

                    string new_outpost_name = row.Cells[MyHelper.strOutpostName].Value.ToString();
                    int new_outpost_economic_value = (int)row.Cells[MyHelper.strOutpostEconomicValue].Value;
                    int new_outpost_coordinate_x = (int)row.Cells[MyHelper.strOutpostCoordinateX].Value;
                    int new_outpost_coordinate_y = (int)row.Cells[MyHelper.strOutpostCoordinateY].Value;
                    int new_outpost_coordinate_z = (int)row.Cells[MyHelper.strOutpostCoordinateZ].Value;

                    if (ctx.outposts.AsEnumerable().FirstOrDefault(outpst =>
                        outpst.outpost_id != o.outpost_id
                        && outpst.outpost_name == new_outpost_name
                        && outpst.outpost_coordinate_x == new_outpost_coordinate_x
                        && outpst.outpost_coordinate_y == new_outpost_coordinate_y
                        && outpst.outpost_coordinate_z == new_outpost_coordinate_z) != null)
                    {
                        string eo = $"Форпост {new_outpost_name} - {new_outpost_coordinate_x};{new_outpost_coordinate_y};{new_outpost_coordinate_z} идентичен существующему!";
                        MessageBox.Show(eo);
                        row.ErrorText = MyHelper.strBadRow + " " + eo;
                        return;
                    }

                    o.outpost_name = new_outpost_name;
                    o.outpost_economic_value = new_outpost_economic_value;
                    o.outpost_coordinate_x = new_outpost_coordinate_x;
                    o.outpost_coordinate_y = new_outpost_coordinate_y;
                    o.outpost_coordinate_z = new_outpost_coordinate_z;

                    //ctx.Entry(o).State = EntityState.Modified;
                    ctx.SaveChanges();
                    _outpostDataTableHandler.Change(o.outpost_id, o.outpost_name, o.outpost_coordinate_x, o.outpost_coordinate_y, o.outpost_coordinate_z);
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
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
                        var o = ctx.outposts.Find(e.Row.Cells[MyHelper.strOutpostId].Value);

                        if (o.buildings.Count > 0
                                || o.storage_resources.Count > 0)
                        {
                            MessageBox.Show($"Вы не можете удалить Форпост {o.outpost_name} - {o.outpost_coordinate_x};{o.outpost_coordinate_y};{o.outpost_coordinate_z}, так как он используется");
                            e.Cancel = true;
                            return;
                        }

                        if (MessageBox.Show($"Вы уверены, что хотите удалить {o.outpost_name} - {o.outpost_coordinate_x};{o.outpost_coordinate_y};{o.outpost_coordinate_z}?", "Предупреждение!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            ctx.outposts.Remove(o);
                            ctx.SaveChanges();
                            _outpostDataTableHandler.Remove(o.outpost_id);
                        }
                        else
                        {
                            e.Cancel = true;
                            return;
                        }
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
            }
        }
    }
}
