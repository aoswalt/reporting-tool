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

namespace VarsityReportingTool {
    public partial class MainWindow : Form {
        private static string ConnectionString = "Driver={iSeries Access ODBC Driver}; System=USC; SignOn=4;";    // using Kerberos
        private static int RowLimitAmount = 1000;

        public MainWindow() {
            InitializeComponent();
        }

        private DataTable runQuery(string query) {
            try {
                using(OdbcConnection conn = new OdbcConnection(ConnectionString)) {
                    conn.Open();


                    OdbcCommand command = new OdbcCommand(query);
                    command.Connection = conn;
                    OdbcDataAdapter adapter = new OdbcDataAdapter(command);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    conn.Close();
                    return table;
                }
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
                return null;
            } catch(Exception ex) {
                MessageBox.Show("Exception: " + ex.Message, "Unhandled Exception");
                return null;
            }
        }

        private void setDataGrid(string query) {
            dataGrid.DataSource = runQuery(query);
            dataGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);   // autosize and allow resize

            lblRowCount.Text = dataGrid.RowCount + " Rows Found";
        }


        // ====================
        // Order Page
        // ====================

        private void btnRunOrderReport_Click(object sender, EventArgs e) {
            btnRunOrderReport.Enabled = false;

            string query = @"SELECT d.ordnr, d.orvch, d.ditem, d.dlsiz, d.dlwr1, d.dlwr2, d.dlwr3, d.dlwr4 
                             FROM VARSITYF.DETAIL AS d ";

            //string query = queryHeader + @"WHERE d.dorcy = 20 AND d.doryr = 15 AND d.dormo = 4 AND d.dorda = 2 ";
            DateTime date = DateTime.Today.AddDays(-1);
            query += @"WHERE ";
            query += String.Format(@"(d.dorcy = {0} AND d.doryr = {1} AND d.dormo = {2} AND d.dorda = {3})",
                                   date.Year / 100, date.Year % 100, date.Month, date.Day);

            if(chkOrderLimitRows.Checked) {
                query += String.Format(@" FETCH FIRST {0} ROWS ONLY", RowLimitAmount);
            }

            setDataGrid(query);

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

            setDataGrid(query);

            btnRunQuery.Enabled = true;
        }

        // allow ctrl-A to select all
        private void txtQuery_KeyDown(object sender, KeyEventArgs e) {
            if(e.Control && e.KeyCode == Keys.A) {
                txtQuery.SelectAll();
                e.Handled = true;
            }
        }
    }
}
