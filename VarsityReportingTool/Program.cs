using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VarsityReportingTool {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            try {
                using(OdbcConnection conn = new OdbcConnection(MainWindow.ConnectionString)) {
                    conn.Open();

                    OdbcCommand command = new OdbcCommand("SELECT * FROM VARSITYF.DETAIL WHERE 1 = 0");
                    command.Connection = conn;
                    command.ExecuteNonQuery();
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainWindow());
            } catch(OdbcException ex) {
                if(ex.Errors[0].SQLState == "IM002") {
                    MessageBox.Show("Driver not found.\n\nPlease contact the IT Department to install the ODBC Driver for IBM iSeries Access.", 
                                    "Driver Not Found");
                } else {
                    string errors = "";

                    for(int i = 0; i != ex.Errors.Count; ++i) {
                        errors += "Error " + (i + 1) + " of " + ex.Errors.Count + "\n";
                        errors += "SQLState:  " + ex.Errors[i].SQLState + "\n";    // IM002 for no driver
                        errors += "NativErr:  " + ex.Errors[i].NativeError + "\n";
                        errors += "Message:   " + ex.Errors[i].Message + "\n\n";
                    }

                    MessageBox.Show(errors, "Errors");
                }
            } catch(Exception ex) {
                MessageBox.Show("Exception: " + ex.Message, "Unhandled Exception");
            }
        }
    }
}
