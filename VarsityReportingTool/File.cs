using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VarsityReportingTool {
    class File {
        private enum Section { NONE, TAB, QUERY, PROMPTS, CUSTOM };

        public static int LoadReport(RichTextBox txtQuery, TextBox txtQueryPrompts, CustomReportManager customReports) {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openDialog.Filter = "Report File (*.rpt)|*.rpt";
            openDialog.RestoreDirectory = true;

            if(openDialog.ShowDialog(MainWindow.GetReference()) == DialogResult.OK) {
                string errors = "";

                using(StreamReader sr = new StreamReader(openDialog.FileName)) {
                    bool firstRunQuery = true;
                    bool firstRunPrompts = true;
                    bool firstRunCustom = true;

                    int lineNumber = 0;
                    string line;
                    Section activeSection = Section.NONE;
                    int tab = 0;

                    while(sr.Peek() > 1) {
                        line = sr.ReadLine().Trim();
                        ++lineNumber;

                        // skipe empty lines and comments
                        if(line.Length == 0 || line[0] == '#') continue;

                        // set section
                        if(line[0] == '>') {
                            line = line.Substring(1).ToUpper();
                            if(Enum.IsDefined(typeof(Section), line)) {
                                activeSection = (Section)Enum.Parse(typeof(Section), line);
                            } else {
                                errors += String.Format("Line {0} - Section not defined: {1}\n", lineNumber, line);
                            }
                            continue;
                        }

                        // tab
                        if(activeSection == Section.TAB) {
                            tab = int.Parse(line);
                        }

                        // query
                        if(activeSection == Section.QUERY) {
                            if(firstRunQuery) {
                                txtQuery.Clear();
                                txtQueryPrompts.Clear();    // to prevent confusion
                                firstRunQuery = false;
                            }

                            txtQuery.Text += line + '\n';
                        }

                        // prompts
                        if(activeSection == Section.PROMPTS) {
                            if(firstRunPrompts) {
                                txtQueryPrompts.Clear();
                                firstRunPrompts = false;
                            }

                            txtQueryPrompts.Text = line + ((txtQueryPrompts.Text.Length > 0) ? "," : "");
                        }

                        // custom report
                        if(activeSection == Section.CUSTOM) {
                            if(firstRunCustom) {
                                customReports.ClearColumns();
                                firstRunCustom = false;
                            }

                            string[] tokens = line.Split(':');
                            string headerId = tokens[0];
                            string comparison = ((tokens.Length > 1) ? tokens[1] : "");
                            string entry = ((tokens.Length > 2) ? tokens[2] : "");

                            customReports.InsertColumn(headerId, comparison, entry);
                        }
                    }

                    if(errors.Length > 0) {
                        MessageBox.Show(errors);
                    }

                    return tab;     // return tab number for switching
                }
            }

            return -1;
        }

        public static void SaveReport(int tab, RichTextBox txtQuery, TextBox txtQueryPrompts, CustomReportManager customReports) {
            if(txtQuery.Text.Trim().Length == 0 && customReports.GetColumnCount() == 0) {
                MessageBox.Show("No custom report to save.", "Error");
                return;
            }

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            saveDialog.Filter = "Report File (*.rpt)|*.rpt";
            saveDialog.RestoreDirectory = true;

            if(saveDialog.ShowDialog() == DialogResult.OK) {
                using(StreamWriter sw = new StreamWriter(saveDialog.FileName)) {
                    sw.WriteLine("#Tab to swtich to");
                    sw.WriteLine("# 0-Order, 1-Query, 2-Custom");
                    sw.WriteLine(">Tab");
                    sw.WriteLine(tab.ToString() + '\n');

                    if(txtQuery.Text.Trim().Length > 0) {
                        sw.WriteLine(">Query");
                        sw.WriteLine(txtQuery.Text.Trim() + '\n');

                        sw.WriteLine(">Prompts");
                        sw.WriteLine(txtQueryPrompts.Text.Trim() + '\n');
                    }

                    if(customReports.GetColumnCount() > 0) {
                        sw.WriteLine(">Custom");
                        for(int index = 0; index != customReports.GetColumnCount(); ++index) {
                            sw.WriteLine(customReports.GetColumnData(index));
                        }
                    }
                }
            }
        }
    }
}
