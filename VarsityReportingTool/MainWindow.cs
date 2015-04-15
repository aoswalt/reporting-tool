﻿using System;
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
        private enum ReportType { All, Cut_Letters, RH_TS, Sublm_Inline, Sublm_Cust };

        public MainWindow() {
            InitializeComponent();
            this.comboboxReportType.DataSource = Enum.GetNames(typeof(ReportType));
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
        // Main Window
        // ====================

        private void reportTabControl_SelectedIndexChanged(object sender, EventArgs e) {
            switch(((TabControl)sender).SelectedIndex) {
                case 2:     // query tab
                    this.AcceptButton = btnRunQuery;
                    this.CancelButton = btnClearOrderEntries;
                    break;
                case 1:     // order report tab
                default:
                    this.AcceptButton = btnRunOrderReport;
                    break;
            }
        }


        // ====================
        // Menu
        // ====================

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            this.Close();
        }


        // ====================
        // Order Page
        // ====================

        private void btnRunOrderReport_Click(object sender, EventArgs e) {
            btnRunOrderReport.Enabled = false;

            // assemble query from filled components
            string query = @"
                SELECT det.dhous, det.scdat, det.endat, det.ordnr, det.orvch, 
                    det.ditem, det.dlsiz, siz.letwid, nam.letname, 
                    det.dlwr1, det.dlwr2, det.dlwr3, det.dlwr4, det.dclr1, det.dclr2, det.dclr3, det.dclr4, det.rudat
                FROM (
                    SELECT d.dhous,
                            CASE WHEN d.dscmo = 0 THEN NULL ELSE DATE(d.dsccy||d.dscyr||'-'||RIGHT('00'||d.dscmo, 2)||'-'||RIGHT('00'||d.dscda, 2)) END AS scdat,
                            DATE(d.dorcy||d.doryr||'-'||RIGHT('00'||d.dormo, 2)||'-'||RIGHT('00'||d.dorda, 2)) AS endat,
                            d.ordnr, d.orvch, d.dpvch, d.ditem, d.dlsiz, 
                            d.dlwr1, d.dlwr2, d.dlwr3, d.dlwr4, d.dclr1, d.dclr2, d.dclr3, d.dclr4,
                            CASE d.drumo WHEN 0 THEN NULL ELSE DATE(d.drucy||d.druyr||'-'||RIGHT('00'||d.drumo, 2)||'-'||RIGHT('00'||d.druda, 2)) END AS rudat

                    FROM VARSITYF.DETAIL AS d

                    WHERE ";

            // enter dates
            if(datePickerEnterDateStart.Checked || datePickerEnterDateEnd.Checked) {
                if(datePickerEnterDateStart.Checked && !datePickerEnterDateEnd.Checked) {
                    // start and no end
                    DateTime date = datePickerEnterDateStart.Value;
                    query += String.Format(@"(d.dorcy = {0} AND d.doryr = {1} AND d.dormo = {2} AND d.dorda = {3}) AND ", 
                                           date.Year / 100, date.Year % 100, date.Month, date.Day);
                } else if(!datePickerEnterDateStart.Checked && datePickerEnterDateEnd.Checked) {
                    // end and no start
                    DateTime date = datePickerEnterDateEnd.Value;
                    query += String.Format(@"(d.dorcy = {0} AND d.doryr = {1} AND d.dormo = {2} AND d.dorda = {3}) AND ",
                                           date.Year / 100, date.Year % 100, date.Month, date.Day);
                } else {
                    // using both
                    DateTime start = datePickerEnterDateStart.Value;
                    DateTime end = datePickerEnterDateEnd.Value;

                    if(start > end) {
                        query += "(";
                        DateTime date = start;
                        while(date > end) {
                            query += String.Format(@"(d.dorcy = {0} AND d.doryr = {1} AND d.dormo = {2} AND d.dorda = {3}) OR ",
                                                   date.Year / 100, date.Year % 100, date.Month, date.Day);
                            date = date.AddDays(-1);
                        }
                        query += String.Format(@"(d.dorcy = {0} AND d.doryr = {1} AND d.dormo = {2} AND d.dorda = {3})",
                                               end.Year / 100, end.Year % 100, end.Month, end.Day);
                        query += ") AND ";
                    } else if(end > start) {
                        query += "(";
                        DateTime date = end;
                        while(date > start) {
                            query += String.Format(@"(d.dorcy = {0} AND d.doryr = {1} AND d.dormo = {2} AND d.dorda = {3}) OR ",
                                                   date.Year / 100, date.Year % 100, date.Month, date.Day);
                            date = date.AddDays(-1);
                        }
                        query += String.Format(@"(d.dorcy = {0} AND d.doryr = {1} AND d.dormo = {2} AND d.dorda = {3})",
                                               start.Year / 100, start.Year % 100, start.Month, start.Day);
                        query += ") AND ";
                    } else {    // equal
                        DateTime date = start;
                        query += String.Format(@"(d.dorcy = {0} AND d.doryr = {1} AND d.dormo = {2} AND d.dorda = {3}) AND ",
                                               date.Year / 100, date.Year % 100, date.Month, date.Day);
                    }
                }
            }

            // schedule dates
            if(datePickerScheduleDateStart.Checked || datePickerScheduleDateEnd.Checked) {
                if(datePickerScheduleDateStart.Checked && !datePickerScheduleDateEnd.Checked) {
                    // start and no end
                    DateTime date = datePickerScheduleDateStart.Value;
                    query += String.Format(@"(d.dsccy = {0} AND d.dscyr = {1} AND d.dscmo = {2} AND d.dscda = {3}) AND ",
                                           date.Year / 100, date.Year % 100, date.Month, date.Day);
                } else if(!datePickerScheduleDateStart.Checked && datePickerScheduleDateEnd.Checked) {
                    // end and no start
                    DateTime date = datePickerScheduleDateEnd.Value;
                    query += String.Format(@"(d.dsccy = {0} AND d.dscyr = {1} AND d.dscmo = {2} AND d.dscda = {3}) AND ",
                                           date.Year / 100, date.Year % 100, date.Month, date.Day);
                } else {
                    // using both
                    DateTime start = datePickerScheduleDateStart.Value;
                    DateTime end = datePickerScheduleDateEnd.Value;

                    if(start > end) {
                        query += "(";
                        DateTime date = start;
                        while(date > end) {
                            query += String.Format(@"(d.dsccy = {0} AND d.dscyr = {1} AND d.dscmo = {2} AND d.dscda = {3}) OR ",
                                                   date.Year / 100, date.Year % 100, date.Month, date.Day);
                            date = date.AddDays(-1);
                        }
                        query += String.Format(@"(d.dsccy = {0} AND d.dscyr = {1} AND d.dscmo = {2} AND d.dscda = {3})",
                                               end.Year / 100, end.Year % 100, end.Month, end.Day);
                        query += ") AND ";
                    } else if(end > start) {
                        query += "(";
                        DateTime date = end;
                        while(date > start) {
                            query += String.Format(@"(d.dsccy = {0} AND d.dscyr = {1} AND d.dscmo = {2} AND d.dscda = {3}) OR ",
                                                   date.Year / 100, date.Year % 100, date.Month, date.Day);
                            date = date.AddDays(-1);
                        }
                        query += String.Format(@"(d.dsccy = {0} AND d.dscyr = {1} AND d.dscmo = {2} AND d.dscda = {3})",
                                               start.Year / 100, start.Year % 100, start.Month, start.Day);
                        query += ") AND ";
                    } else {    // equal
                        DateTime date = start;
                        query += String.Format(@"(d.dsccy = {0} AND d.dscyr = {1} AND d.dscmo = {2} AND d.dscda = {3}) AND ",
                                               date.Year / 100, date.Year % 100, date.Month, date.Day);
                    }
                }
            }

            // order number
            if(!string.IsNullOrWhiteSpace(txtOrderNumber.Text)) {
                query += String.Format("(d.ordnr = {0}) AND ", txtOrderNumber.Text);
            }

            // voucher
            if(!string.IsNullOrWhiteSpace(txtVoucher.Text)) {
                query += String.Format("(d.orvch = {0}) AND ", txtVoucher.Text);
            }

            // house
            if(!string.IsNullOrWhiteSpace(txtHouse.Text)) {
                query += String.Format("(TRIM(d.dhous) LIKE '{0}') AND ", txtHouse.Text.ToUpper());
            }

            // style code
            if(!string.IsNullOrWhiteSpace(txtStyleCode.Text)) {
                query += String.Format("(TRIM(d.ditem) LIKE '{0}') AND ", txtStyleCode.Text.ToUpper());
            }

            // size
            if(!string.IsNullOrWhiteSpace(txtSize.Text)) {
                query += String.Format("(d.dlsiz = {0}) AND ", txtSize.Text);
            }

            switch((ReportType)Enum.Parse(typeof(ReportType), comboboxReportType.SelectedValue.ToString())) {
                case ReportType.All:
                    break;
                case ReportType.Cut_Letters:
                    query += @"
                        (d.dclas IN ('041', '049', '04C', '04D', '04Y', 'F09', 'PS3', 'L02', 'L05', 'L10', 'S03', 'SKL', 'VTT')) AND 
                        (d.ditem NOT LIKE 'OZ%') AND ";
                    break;
                case ReportType.RH_TS:
                    query += @"
                        (d.dclas IN ('04U', '04V', '04W', 'L01', 'L03', 'L04', 'L09', 'F09', 'PS3', 'RSC', 'RSO')) AND 
                        ((d.ditem LIKE 'RH%') OR (d.ditem LIKE 'TS%') OR (d.ditem LIKE 'RST%')) AND ";
                    break;
                case ReportType.Sublm_Inline:
                    query += @"
                        (d.dclas IN ('04G')) AND 
                        (d.ditem NOT LIKE 'IDC%') AND (d.ditem NOT LIKE 'COZ%') AND ";
                    break;
                case ReportType.Sublm_Cust:
                    query += @"
                        (d.dclas IN ('04G')) AND 
                        (d.ditem LIKE 'IDC%') AND (d.ditem NOT LIKE 'COZ%') AND ";
                    break;
                default:
                    break;
            }

            query +=                    
                    @"(d.dscda > 0)
                ) AS det

                LEFT JOIN 
                            DJLIBR.ORD_NAM_C 
                AS nam
                ON det.ordnr = nam.ordnr AND det.orvch = nam.orvch AND nam.letname <> ''

                LEFT JOIN (
                            SELECT DISTINCT s.ordnr, s.orvch, s.letwid
                            FROM VARSITYF.HLDSIZ AS s
                ) AS siz
                ON det.ordnr = siz.ordnr AND det.dpvch = siz.orvch ";
            
            // spec
            if(!string.IsNullOrWhiteSpace(txtSpec.Text)) {
                query += String.Format("WHERE (siz.letwid = {0}) ", txtSpec.Text);
            }

            // default sort by style code
            query +=
              @"ORDER BY det.ditem";

            // row limit option
            if(chkOrderLimitRows.Checked) {
                query += String.Format(@" FETCH FIRST {0} ROWS ONLY", RowLimitAmount);
            }

            runQuery(query);

            btnRunOrderReport.Enabled = true;
        }

        private void btnClearOrderEntries_Click(object sender, EventArgs e) {
            datePickerEnterDateStart.Value = DateTime.Today;
            datePickerEnterDateStart.Checked = false;
            datePickerEnterDateEnd.Value = DateTime.Today;
            datePickerEnterDateEnd.Checked = false;
            datePickerScheduleDateStart.Value = DateTime.Today;
            datePickerScheduleDateStart.Checked = false;
            datePickerScheduleDateEnd.Value = DateTime.Today;
            datePickerScheduleDateEnd.Checked = false;

            txtOrderNumber.Text = "";
            txtVoucher.Text = "";
            txtHouse.Text = "";
            txtStyleCode.Text = "";
            txtSize.Text = "";
            txtSpec.Text = "";

            comboboxReportType.SelectedIndex = 0;
            chkOrderLimitRows.Checked = true;
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
                this.dataGrid.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
                Clipboard.SetDataObject(dataGrid.GetClipboardContent(), true);
                this.dataGrid.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithAutoHeaderText;
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