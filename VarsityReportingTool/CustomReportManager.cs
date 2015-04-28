using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VarsityReportingTool {
    class CustomReportManager {
        private enum HeaderType { String, Integer, Decimal, Date }
        private enum HeaderId { HOUSE, SCHDATE, SIZE }
        private static Dictionary<HeaderId, ColumnHeader> headers = new Dictionary<HeaderId, ColumnHeader>();
        private static List<string> stringComparisons = new List<string>(new string[] { "LIKE", "NOT LIKE", "IN", "NOT IN" });
        private static List<string> numberComparisons = new List<string>(new string[] { "=", "<>", ">", ">=", "<", "<=", "IN", "NOT IN" });
        private static List<string> dateComparisons = new List<string>(new string[] { "=", "<>", ">", ">=", "<", "<=" });
        private static Dictionary<HeaderType, List<string>> comparisons = new Dictionary<HeaderType, List<string>>();

        static CustomReportManager() {
            headers.Add(HeaderId.HOUSE, new ColumnHeader(HeaderId.HOUSE, "House", HeaderType.String));
            headers.Add(HeaderId.SCHDATE, new ColumnHeader(HeaderId.SCHDATE, "Sch Date", HeaderType.Date));
            headers.Add(HeaderId.SIZE, new ColumnHeader(HeaderId.SIZE, "Size", HeaderType.Decimal));

            comparisons.Add(HeaderType.Date, dateComparisons);
            comparisons.Add(HeaderType.Decimal, numberComparisons);
            comparisons.Add(HeaderType.Integer, numberComparisons);
            comparisons.Add(HeaderType.String, stringComparisons);
        }

        private FlowLayoutPanel columnsPanel;
        private List<Column> customReportColumns = new List<Column>();
        private Column selectedColumn;

        public CustomReportManager(FlowLayoutPanel columnsPanel) {
            this.columnsPanel = columnsPanel;
        }

        public void AddColumn() {
            customReportColumns.Add(new Column(this, customReportColumns.Count));
            columnsPanel.Controls.Add(customReportColumns.Last().getPanel());
            UpdateColumns();
        }

        public void RemoveColumn() {
            if(selectedColumn != null) {
                customReportColumns.Remove(selectedColumn);
                columnsPanel.Controls.Remove(selectedColumn.getPanel());
                selectedColumn = null;
                ((Button)this.columnsPanel.Parent.Controls["btnRemoveColumn"]).Enabled = false;
            }
            UpdateColumns();
        }

        public void UpdateColumns() {
            int count = this.customReportColumns.Count;
            for(int i = 0; i != count; ++i) {
                Column column = this.customReportColumns[i];
                int columnIndexInPanel = this.columnsPanel.Controls.GetChildIndex(column.getPanel());
                column.setId(columnIndexInPanel + 1);
                column.setUpButtonEnabled(columnIndexInPanel > 0);
                column.setDownButtonEnabled(columnIndexInPanel < (count - 1));
                column.setLabelColor();
            }
        }

        private void SelectColumn(Column column) {
            if(selectedColumn != null) {
                selectedColumn.setSelected(false);
                if(column != selectedColumn) {
                    selectedColumn = column;
                    selectedColumn.setSelected(true);
                    ((Button)this.columnsPanel.Parent.Controls["btnRemoveColumn"]).Enabled = true;
                } else {
                    selectedColumn = null;
                    ((Button)this.columnsPanel.Parent.Controls["btnRemoveColumn"]).Enabled = false;
                }
            } else {
                selectedColumn = column;
                selectedColumn.setSelected(true);
                ((Button)this.columnsPanel.Parent.Controls["btnRemoveColumn"]).Enabled = true;
            }
            UpdateColumns();
        }

        private string getEntryValue(Column column) {
            string entry = "";
            string comparisonValue = ((string)column.comparisonComboBox.SelectedValue);

            switch(headers[column.headerId].Type) {
                case HeaderType.Date: {
                    entry = ((DateTimePicker)column.entryField).Value.Date.ToString("yyyy-MM-dd");
                    entry = '\'' + entry + '\'';
                    } break;
                case HeaderType.Decimal:
                case HeaderType.Integer: {
                    entry = ((TextBox)column.entryField).Text.ToUpper();

                    if(comparisonValue.Contains("IN")) {
                        entry = '(' + entry + ')';
                    }
                    } break;
                case HeaderType.String: {
                    entry = ((TextBox)column.entryField).Text.ToUpper();

                    if(!entry.Contains('\'')) {
                        entry = '\'' + entry.Replace(",", "','") + '\'';
                    }

                    if(comparisonValue.Contains("IN")) {
                        entry = '(' + entry + ')';
                    }
                    } break;
                default:
                    break;
            }

            return entry;
        }

        public string GenerateQuery() {
            List<HeaderId> includedHeaders = new List<HeaderId>();
            List<string> selectColumns = new List<string>();
            List<string> innerSelectColumns = new List<string>();
            List<string> whereClauses = new List<string>();
            List<string> joins = new List<string>();

            foreach(Column column in customReportColumns) {
                string comparisonValue = ((string)column.comparisonComboBox.SelectedValue);
                string entryValue = getEntryValue(column);

                switch(column.headerId) {
                    case HeaderId.HOUSE: 
                        if(!includedHeaders.Contains(column.headerId)) {
                            selectColumns.Add("det.dhous");
                            innerSelectColumns.Add("d.dhous");
                            includedHeaders.Add(column.headerId);
                        }

                        if(column.entryField.Text != "") {
                            whereClauses.Add(String.Format("(UPPER(d.dhous) {0} {1})", comparisonValue, entryValue));
                        }
                        break;
                    case HeaderId.SCHDATE:
                        if(!includedHeaders.Contains(column.headerId)) {
                            selectColumns.Add("det.scdat");
                            innerSelectColumns.Add("DATE(d.dsccy||d.dscyr||'-'||RIGHT('00'||d.dscmo, 2)||'-'||RIGHT('00'||d.dscda, 2)) AS scdat");
                            includedHeaders.Add(column.headerId);
                        }

                        if(((DateTimePicker)column.entryField).Checked) {
                            whereClauses.Add(String.Format(@"(CASE WHEN d.dscda = 0 THEN NULL ELSE 
                                                              DATE(d.dsccy||d.dscyr||'-'||RIGHT('00'||d.dscmo, 2)||'-'||RIGHT('00'||d.dscda, 2)) END) {0} DATE({1}) ", 
                                                            comparisonValue, entryValue));
                        }
                        break;
                    case HeaderId.SIZE:
                        if(!includedHeaders.Contains(column.headerId)) {
                            selectColumns.Add("det.dlsiz");
                            innerSelectColumns.Add("d.dlsiz");
                            includedHeaders.Add(column.headerId);
                        }

                        if(column.entryField.Text != "") {
                            whereClauses.Add(String.Format("(d.dlsiz {0} {1})", comparisonValue, entryValue));
                        }
                        break;
                    default:
                        break;
                }
            }

            string combinedSelectColumns = selectColumns[0];
            for(int i = 1; i != selectColumns.Count; ++i) {
                combinedSelectColumns += ", " + selectColumns[i];
            }

            string combinedInnerSelectColumns = innerSelectColumns[0];
            for(int i = 1; i != innerSelectColumns.Count; ++i) {
                combinedInnerSelectColumns += ", " + innerSelectColumns[i];
            }

            string combinedWhereClause = whereClauses[0];
            for(int i = 1; i != whereClauses.Count; ++i) {
                combinedWhereClause += " AND " + whereClauses[i];
            }

            string query = String.Format("SELECT {0} FROM ( SELECT {1} FROM VARSITYF.DETAIL AS d WHERE {2}) AS det {3}",
                                         combinedSelectColumns, combinedInnerSelectColumns, combinedWhereClause, "" /* joins*/);

            // default sort by style code
            //query += " ORDER BY det.ditem";
            return query;
        }

        private class ColumnHeader {
            public HeaderId Id { get; private set; }
            public string Description { get; private set; }
            public HeaderType Type { get; private set; }

            public ColumnHeader(HeaderId id, string description, HeaderType headerType) {
                this.Id = id;
                this.Description = description;
                this.Type = headerType;
            }
        }

        private class Column {
            public HeaderId headerId;
            public ComboBox comparisonComboBox;
            public Control entryField;
            private CustomReportManager manager;
            private FlowLayoutPanel panel;
            private Label label;
            private ComboBox headerComboBox;
            private Button moveUpButton;
            private Button moveDownButton;
            private bool selected;

            public Column(CustomReportManager manager, int id) {
                this.manager = manager;

                // containing panel
                this.panel = new FlowLayoutPanel();
                this.panel.Size = new System.Drawing.Size(380, 28);
                this.panel.WrapContents = false;

                // number label
                this.label = new Label();
                this.label.Size = new System.Drawing.Size(21, 15);
                this.label.Margin = new System.Windows.Forms.Padding(3, 6, 0, 3);
                this.label.ForeColor = System.Drawing.Color.Black;
                this.label.BackColor = System.Drawing.Color.White;
                this.label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                this.label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                this.label.Text = (id + 1).ToString();
                this.label.Click += new System.EventHandler(columnLabel_click);
                this.panel.Controls.Add(this.label);

                // headerId dropdown
                this.headerComboBox = new ComboBox();
                this.headerComboBox.FormattingEnabled = true;
                this.headerComboBox.Size = new System.Drawing.Size(95, 21);
                // TODO: unique values only?
                this.headerComboBox.DataSource = headers.Values.ToList();
                this.headerComboBox.DisplayMember = "Description";
                this.headerComboBox.ValueMember = "Id";
                this.headerComboBox.SelectedIndexChanged += new System.EventHandler(headerComboBox_SelectedIndexChanged);
                this.panel.Controls.Add(this.headerComboBox);

                // compare dropdown
                this.comparisonComboBox = new ComboBox();
                this.comparisonComboBox.Size = new System.Drawing.Size(74, 21);
                this.comparisonComboBox.DataSource = comparisons[HeaderType.String].ToList();
                this.panel.Controls.Add(this.comparisonComboBox);

                // entry field
                this.entryField = new TextBox();
                this.entryField.Size = new System.Drawing.Size(118, 20);
                this.panel.Controls.Add(this.entryField);

                // move up button
                this.moveUpButton = new Button();
                this.moveUpButton.Size = new System.Drawing.Size(23, 23);
                this.moveUpButton.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
                this.moveUpButton.Text = "▲";
                this.moveUpButton.UseVisualStyleBackColor = true;
                this.moveUpButton.Click += new System.EventHandler(moveUpButton_Click);
                this.moveUpButton.Enabled = (id > 0);
                this.panel.Controls.Add(this.moveUpButton);

                // move down button
                this.moveDownButton = new Button();
                this.moveDownButton.Size = new System.Drawing.Size(23, 23);
                this.moveDownButton.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
                this.moveDownButton.Text = "▼";
                this.moveDownButton.UseVisualStyleBackColor = true;
                this.moveDownButton.Click += new System.EventHandler(moveDownButton_Click);
                this.moveDownButton.Enabled = false;
                this.panel.Controls.Add(this.moveDownButton);
            }

            public FlowLayoutPanel getPanel() {
                return panel;
            }

            public void setId(int id) {
                label.Text = id.ToString();
            }

            public void setUpButtonEnabled(bool value) {
                moveUpButton.Enabled = value;
            }

            public void setDownButtonEnabled(bool value) {
                moveDownButton.Enabled = value;
            }

            public void setSelected(bool value) {
                this.selected = value;
            }

            public void setLabelColor() {
                if(this.selected) {
                    this.label.ForeColor = System.Drawing.Color.White;
                    this.label.BackColor = System.Drawing.Color.Blue;
                } else {
                    this.label.ForeColor = System.Drawing.Color.Black;
                    this.label.BackColor = System.Drawing.Color.White;
                }
            }

            private void columnLabel_click(object sender, EventArgs e) {
                manager.SelectColumn(this);
            }

            private void headerComboBox_SelectedIndexChanged(object sender, EventArgs e) {
                HeaderId newHeaderId = (HeaderId)Enum.Parse(typeof(HeaderId), this.headerComboBox.SelectedValue.ToString());

                if(headerId != newHeaderId) {
                    switch(headers[newHeaderId].Type) {
                        case HeaderType.Date:
                            this.comparisonComboBox.DataSource = comparisons[headers[newHeaderId].Type].ToList();
                            this.panel.Controls.Remove(this.entryField);
                            this.entryField = new DateTimePicker();
                            this.entryField.Size = new System.Drawing.Size(118, 20);
                            ((DateTimePicker)this.entryField).Format = System.Windows.Forms.DateTimePickerFormat.Short;
                            ((DateTimePicker)this.entryField).ShowCheckBox = true;
                            ((DateTimePicker)this.entryField).Checked = false;
                            this.panel.Controls.Add(this.entryField);
                            this.panel.Controls.SetChildIndex(this.entryField, this.panel.Controls.GetChildIndex(this.comparisonComboBox) + 1);
                            break;
                        case HeaderType.Decimal:
                            this.comparisonComboBox.DataSource = comparisons[headers[newHeaderId].Type].ToList();
                            this.panel.Controls.Remove(this.entryField);
                            this.entryField = new TextBox();
                            this.entryField.Size = new System.Drawing.Size(118, 20);
                            this.entryField.KeyPress += new System.Windows.Forms.KeyPressEventHandler(entryField_Decimal_KeyPress);
                            this.panel.Controls.Add(this.entryField);
                            this.panel.Controls.SetChildIndex(this.entryField, this.panel.Controls.GetChildIndex(this.comparisonComboBox) + 1);
                            break;
                        case HeaderType.Integer:
                            this.comparisonComboBox.DataSource = comparisons[headers[newHeaderId].Type].ToList();
                            this.panel.Controls.Remove(this.entryField);
                            this.entryField = new TextBox();
                            this.entryField.Size = new System.Drawing.Size(118, 20);
                            this.entryField.KeyPress += new System.Windows.Forms.KeyPressEventHandler(entryField_Integer_KeyPress);
                            this.panel.Controls.Add(this.entryField);
                            this.panel.Controls.SetChildIndex(this.entryField, this.panel.Controls.GetChildIndex(this.comparisonComboBox) + 1);
                            break;
                        case HeaderType.String:
                            this.comparisonComboBox.DataSource = comparisons[headers[newHeaderId].Type].ToList();
                            this.panel.Controls.Remove(this.entryField);
                            this.entryField = new TextBox();
                            this.entryField.Size = new System.Drawing.Size(118, 20);
                            this.panel.Controls.Add(this.entryField);
                            this.panel.Controls.SetChildIndex(this.entryField, this.panel.Controls.GetChildIndex(this.comparisonComboBox) + 1);
                            break;
                        default:
                            MessageBox.Show("Error: HeaderType not defined for entry: " + headers[newHeaderId].Type);
                            break;
                    }
                }

                headerId = newHeaderId;
            }

            // allow only numbers, comma, or period in entry field
            private void entryField_Decimal_KeyPress(object sender, KeyPressEventArgs e) {
                if(!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar) && !(e.KeyChar == '.') && !(e.KeyChar == ',')) {
                    e.Handled = true;
                    System.Media.SystemSounds.Beep.Play();
                }
            }

            // allow only numbers or comma in entry field
            private void entryField_Integer_KeyPress(object sender, KeyPressEventArgs e) {
                if(!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar) && !(e.KeyChar == ',')) {
                    e.Handled = true;
                    System.Media.SystemSounds.Beep.Play();
                }
            }

            private void moveUpButton_Click(object sender, EventArgs e) {
                this.panel.Parent.Controls.SetChildIndex(this.panel, this.panel.Parent.Controls.GetChildIndex(this.panel) - 1);
                manager.UpdateColumns();
            }

            private void moveDownButton_Click(object sender, EventArgs e) {
                this.panel.Parent.Controls.SetChildIndex(this.panel, this.panel.Parent.Controls.GetChildIndex(this.panel) + 1);
                manager.UpdateColumns();
            }
        }
    }
}
