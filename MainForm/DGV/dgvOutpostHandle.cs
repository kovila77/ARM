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
            using (var ctx = new OutpostDataContext())
            {
                outpost o = new outpost
                {
                    outpost_name = row.Cells["outpost_name"].Value.ToString(),
                    outpost_economic_value = (int)row.Cells["outpost_economic_value"].Value,
                    outpost_coordinate_x = (int)row.Cells["outpost_coordinate_x"].Value,
                    outpost_coordinate_y = (int)row.Cells["outpost_coordinate_y"].Value,
                    outpost_coordinate_z = (int)row.Cells["outpost_coordinate_z"].Value,
                };

                foreach (var x in ctx.outposts)
                {
                    if (x.outpost_name == o.outpost_name
                        && x.outpost_economic_value == o.outpost_economic_value
                        && x.outpost_coordinate_x == o.outpost_coordinate_x
                        && x.outpost_coordinate_y == o.outpost_coordinate_y
                        && x.outpost_coordinate_z == o.outpost_coordinate_z
                        )
                    {
                        MessageBox.Show("Форпост идентичен существующему!");
                        row.ErrorText = "Ошибка!";
                        return;
                    }
                }
                row.ErrorText = "";

                ctx.outposts.Add(o);
                ctx.SaveChanges();

                row.Cells["outpost_id"].Value = o.outpost_id;
                row.Cells["Source"].Value = o;
            }
        }

        protected override void Update(DataGridViewRow row)
        {
            using (var ctx = new OutpostDataContext())
            {
                outpost o = ctx.outposts.Find((int)row.Cells["outpost_id"].Value);

                foreach (var x in ctx.outposts)
                {
                    if (x.outpost_id == o.outpost_id)
                        continue;
                    if (x.outpost_name == row.Cells["outpost_name"].Value.ToString()
                        && x.outpost_economic_value == (int)row.Cells["outpost_economic_value"].Value
                        && x.outpost_coordinate_x == (int)row.Cells["outpost_coordinate_x"].Value
                        && x.outpost_coordinate_y == (int)row.Cells["outpost_coordinate_y"].Value
                        && x.outpost_coordinate_z == (int)row.Cells["outpost_coordinate_z"].Value
                        )
                    {
                        MessageBox.Show("Форпост идентичен существующему!");
                        row.ErrorText = "Ошибка!";
                        return;
                    }
                }
                row.ErrorText = "";

                o.outpost_name = row.Cells["outpost_name"].Value.ToString();
                o.outpost_economic_value = (int)row.Cells["outpost_economic_value"].Value;
                o.outpost_coordinate_x = (int)row.Cells["outpost_coordinate_x"].Value;
                o.outpost_coordinate_y = (int)row.Cells["outpost_coordinate_y"].Value;
                o.outpost_coordinate_z = (int)row.Cells["outpost_coordinate_z"].Value;

                ctx.Entry(o).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public override void UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            var row = e.Row;
            if (row.Cells["Source"].Value == DBNull.Value) return;
            using (var ctx = new OutpostDataContext())
            {
                outpost o = (outpost)row.Cells["Source"].Value;


                if (MessageBox.Show($"Вы уверены, что хотите удалить ВСЮ информацию о {o.outpost_name}?", "Предупреждение!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ctx.outposts.Attach(o);
                    ctx.outposts.Remove(o);
                    ctx.SaveChanges();
                }
                else
                {
                    e.Cancel = true;
                    return;
                }
            }
            row.Cells["Source"].Value = DBNull.Value;
        }
    }
}
