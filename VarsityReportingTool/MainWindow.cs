using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace VarsityReportingTool {
    public partial class MainWindow : Form {
        private static string ConnectionString = "Driver={iSeries Access ODBC Driver}; System=USC; SignOn=4;";    // using Kerberos
        private static int RowLimitAmount = 1000;

        public MainWindow() {
            InitializeComponent();
        }

        private void runQuery(string query) {
            try {
                using(OdbcConnection conn = new OdbcConnection(ConnectionString)) {
                    conn.Open();

                    OdbcCommand command = new OdbcCommand(query);
                    command.Connection = conn;

                    // handle prompting for values
                    int parameterCount = query.Count(c => c == '?');    // linq counting for ?
                    if(parameterCount > 0) {
                        command.Prepare();

                        for(int i = 0; i != parameterCount; ++i) {
                            command.Parameters.Add(new OdbcParameter("Param " + i, OdbcType.VarChar));
                        }

                        // do not run command if no parameters entered
                        if(ParameterPrompt.PromptForParameterValues(ref command) == DialogResult.Cancel) { return; }
                    }

                    OdbcDataAdapter adapter = new OdbcDataAdapter(command);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    dataGrid.DataSource = table;
                }

                dataGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);   // autosize and allow resize

                lblRowCount.Text = dataGrid.RowCount + " Rows Found";
            } catch(OdbcException ex) {
                string errors = "";

                for(int i = 0; i != ex.Errors.Count; ++i) {
                    errors += "Error " + (i + 1) + " of " + ex.Errors.Count + "\n";
                    errors += "SQLState:  " + ex.Errors[i].SQLState + "\n";    // IM002 for no driver
                    errors += "NativErr:  " + ex.Errors[i].NativeError + "\n";
                    errors += "EMessage:  " + ex.Errors[i].Message + "\n";
                    errors += "ESource:   " + ex.Errors[i].Source + "\n\n";
                }

                MessageBox.Show(errors, "Errors");
            } catch(Exception ex) {
                MessageBox.Show("Exception: " + ex.Message, "Unhandled Exception");
            }
        }


        // ====================
        // Order Page
        // ====================

        private void btnRunOrderReport_Click(object sender, EventArgs e) {
            btnRunOrderReport.Enabled = false;

            string query = @"SELECT d.ordnr, d.orvch, d.ditem, d.dlsiz, d.dlwr1, d.dlwr2, d.dlwr3, d.dlwr4 
                             FROM VARSITYF.DETAIL AS d ";

            DateTime date = DateTime.Today.AddDays(-1);
            query += @"WHERE ";
            query += String.Format(@"(d.dorcy = {0} AND d.doryr = {1} AND d.dormo = {2} AND d.dorda = {3})",
                                   date.Year / 100, date.Year % 100, date.Month, date.Day);

            if(chkOrderLimitRows.Checked) {
                query += String.Format(@" FETCH FIRST {0} ROWS ONLY", RowLimitAmount);
            }

            runQuery(query);

            btnRunOrderReport.Enabled = true;
        }


        // ====================
        // Query Page
        // ====================

        private void btnRunQuery_Click(object sender, EventArgs e) {
            btnRunQuery.Enabled = false;

            string query = txtQuery.Text;

            if(chkQueryLimitRows.Checked) {
                query += String.Format(@" FETCH FIRST {0} ROWS ONLY", RowLimitAmount);
            }

            runQuery(query);

            btnRunQuery.Enabled = true;
        }


        // ====================
        // Data Grid
        // ====================

        // allow ctrl-A to select all
        private void txtQuery_KeyDown(object sender, KeyEventArgs e) {
            if(e.Control && e.KeyCode == Keys.A) {
                txtQuery.SelectAll();
                e.Handled = true;
            }
        }

        private void btnCopyAll_Click(object sender, EventArgs e) {
            dataGrid.SelectAll();
            Clipboard.SetDataObject(dataGrid.GetClipboardContent(), true);
            dataGrid.ClearSelection();

            dataGrid.CurrentCell.Selected = true;
        }

        private void btnCopySelection_Click(object sender, EventArgs e) {
            Clipboard.SetDataObject(dataGrid.GetClipboardContent(), true);
        }

        private void btnOpenInExcel_Click(object sender, EventArgs e) {
            btnOpenInExcel.Enabled = false;

            try {
                Excel.Application xlApp = new Excel.Application();
                Excel.Workbook xlWorkBook = xlApp.Workbooks.Add();


                dataGrid.SelectAll();
                Clipboard.SetDataObject(dataGrid.GetClipboardContent(), true);
                dataGrid.ClearSelection();

                dataGrid.CurrentCell.Selected = true;

                xlApp.Worksheets.get_Item(1).PasteSpecial();

                xlApp.Columns.WrapText = false;
                xlApp.Columns.AutoFit();
                xlApp.Rows.AutoFit();
                xlApp.Visible = true;
            } catch(Exception ex) {
                MessageBox.Show("Error opening in Excel.\n\n" + ex.Message, "Error");
            }

            btnOpenInExcel.Enabled = true;
        }
    }
}
