using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VarsityReportingTool {
    public enum HeaderType { String, Integer, Decimal, Date }
    public enum Header {
        [HeaderInfo(HeaderType.String, "Cut House")]    HOUSE,
        [HeaderInfo(HeaderType.Date, "Sch Date")]       SCHDATE,
        [HeaderInfo(HeaderType.Decimal, "Size")]        SIZE
    }
    public enum StringCompare {
        [CompareInfo("LIKE")]       LIKE,
        [CompareInfo("NOT LIKE")]   NOTLIKE,
        [CompareInfo("IN")]         IN,
        [CompareInfo("NOT IN")]     NOTIN
    }
    public enum NumberCompare {
        [CompareInfo("=")]      EQUALS,
        [CompareInfo("<>")]     NOTEQUALS,
        [CompareInfo(">")]      GREATER,
        [CompareInfo(">=")]     GREATEREQUALS,
        [CompareInfo("<")]      LESS,
        [CompareInfo("<=")]     LESSEQUALS,
        [CompareInfo("IN")]     IN,
        [CompareInfo("NOT IN")] NOTIN
    }

    class HeaderInfoAttribute : Attribute {
        public HeaderInfoAttribute(HeaderType type, string label) {
            this.Type = type;
            this.Label = label;
        }

        public HeaderType Type { get; private set; }
        public string Label { get; private set; }
    }

    class CompareInfoAttribute : Attribute {
        public CompareInfoAttribute(string queryValue) {
            this.QueryValue = queryValue;
        }

        public string QueryValue { get; private set; }
    }

    class CustomReportManager {
        private List<Header> availableColumnHeaders = new List<Header>(Enum.GetValues(typeof(Header)).Cast<Header>());
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

        public string GenerateQuery() {
            List<Header> includedHeaders = new List<Header>();
            string columns = "";
            string innerColumns = "";
            string where = "";

            foreach(Column column in customReportColumns) {
                switch(column.header) {
                    case Header.HOUSE: 
                        if(!includedHeaders.Contains(column.header)) {
                            columns += "det.dhous ";
                            innerColumns += "d.dhous ";
                            includedHeaders.Add(column.header);
                        }

                        if(column.entryField.Text != "") {
                            if(where == "") where += "WHERE ";
                            where += "d.dhous " + EnumExtensions.GetAttribute<CompareInfoAttribute>(EnumExtensions.GetCompareEnumFromString<StringCompare>((string)column.comparisonComboBox.SelectedValue)).QueryValue +
                                      " " + column.entryField.Text + " ";
                        }
                        break;
                    case Header.SCHDATE:
                        break;
                    case Header.SIZE:
                        break;
                    default:
                        break;
                }
            }

            string query = String.Format("SELECT {0} FROM ( SELECT {1} FROM VARSITYF.DETAIL AS d {2} ) AS det", columns, innerColumns, where);

            //query += where;

            // default sort by style code
            //query += " ORDER BY det.ditem";
            return query;
        }

        private class Column {
            public Header header;
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

                // header dropdown
                this.headerComboBox = new ComboBox();
                this.headerComboBox.FormattingEnabled = true;
                this.headerComboBox.Size = new System.Drawing.Size(95, 21);
                // TODO: unique values only?
                //this.headerComboBox.DataSource = availableColumnHeaders;
                //this.headerComboBox.DataSource = Enum.GetValues(typeof(Header));
                this.headerComboBox.DataSource = EnumExtensions.GetLabels(typeof(Header));
                this.headerComboBox.DisplayMember = "Label";
                this.headerComboBox.SelectedIndexChanged += new System.EventHandler(headerComboBox_SelectedIndexChanged);
                this.panel.Controls.Add(this.headerComboBox);

                // compare dropdown
                this.comparisonComboBox = new ComboBox();
                this.comparisonComboBox.FormattingEnabled = true;
                this.comparisonComboBox.Size = new System.Drawing.Size(74, 21);
                //this.comparisonComboBox.DataSource = Enum.GetValues(typeof(StringCompare));
                this.comparisonComboBox.DataSource = EnumExtensions.GetQueryValues(typeof(StringCompare));
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
                //Header newHeader = ((Header)this.headerComboBox.SelectedValue);
                Header newHeader = EnumExtensions.GetHeaderEnumFromString<Header>((string)this.headerComboBox.SelectedValue);

                HeaderType headerType = EnumExtensions.GetAttribute<HeaderInfoAttribute>(header).Type;
                HeaderType newHeaderType = EnumExtensions.GetAttribute<HeaderInfoAttribute>(newHeader).Type;
                if(headerType != newHeaderType) {
                    switch(newHeaderType) {
                        case HeaderType.Date:
                            //this.comparisonComboBox.DataSource = Enum.GetValues(typeof(NumberCompare));
                            this.comparisonComboBox.DataSource = EnumExtensions.GetQueryValues(typeof(NumberCompare));
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
                            //this.comparisonComboBox.DataSource = Enum.GetValues(typeof(NumberCompare));
                            this.comparisonComboBox.DataSource = EnumExtensions.GetQueryValues(typeof(NumberCompare));
                            this.panel.Controls.Remove(this.entryField);
                            this.entryField = new TextBox();
                            this.entryField.Size = new System.Drawing.Size(118, 20);
                            this.entryField.KeyPress += new System.Windows.Forms.KeyPressEventHandler(entryField_Decimal_KeyPress);
                            this.panel.Controls.Add(this.entryField);
                            this.panel.Controls.SetChildIndex(this.entryField, this.panel.Controls.GetChildIndex(this.comparisonComboBox) + 1);
                            break;
                        case HeaderType.Integer:
                            //this.comparisonComboBox.DataSource = Enum.GetValues(typeof(NumberCompare));
                            this.comparisonComboBox.DataSource = EnumExtensions.GetQueryValues(typeof(NumberCompare));
                            this.panel.Controls.Remove(this.entryField);
                            this.entryField = new TextBox();
                            this.entryField.Size = new System.Drawing.Size(118, 20);
                            this.entryField.KeyPress += new System.Windows.Forms.KeyPressEventHandler(entryField_Integer_KeyPress);
                            this.panel.Controls.Add(this.entryField);
                            this.panel.Controls.SetChildIndex(this.entryField, this.panel.Controls.GetChildIndex(this.comparisonComboBox) + 1);
                            break;
                        case HeaderType.String:
                            //this.comparisonComboBox.DataSource = Enum.GetValues(typeof(StringCompare));
                            this.comparisonComboBox.DataSource = EnumExtensions.GetQueryValues(typeof(StringCompare));
                            this.panel.Controls.Remove(this.entryField);
                            this.entryField = new TextBox();
                            this.entryField.Size = new System.Drawing.Size(118, 20);
                            this.panel.Controls.Add(this.entryField);
                            this.panel.Controls.SetChildIndex(this.entryField, this.panel.Controls.GetChildIndex(this.comparisonComboBox) + 1);
                            break;
                        default:
                            MessageBox.Show("Error: HeaderType not defined for entry: " + newHeaderType);
                            break;
                    }
                }

                header = newHeader;
            }

            // allow only numbers or period in entry field
            private void entryField_Decimal_KeyPress(object sender, KeyPressEventArgs e) {
                if(!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar) && !(e.KeyChar == '.')) {
                    e.Handled = true;
                    System.Media.SystemSounds.Beep.Play();
                }
            }

            // allow only numbers in entry field
            private void entryField_Integer_KeyPress(object sender, KeyPressEventArgs e) {
                if(!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar)) {
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

    static class EnumExtensions {
        public static T GetCompareEnumFromString<T>(string stringValue) where T : struct {
            foreach(object e in Enum.GetValues(typeof(T))) {
                if(EnumExtensions.GetAttribute<CompareInfoAttribute>((Enum)e).QueryValue.Equals(stringValue)) {
                    return (T)e;
                }
            }
            return default(T);
        }

        public static T GetHeaderEnumFromString<T>(string stringValue) where T : struct {
            foreach(object e in Enum.GetValues(typeof(T))) {
                if(EnumExtensions.GetAttribute<HeaderInfoAttribute>((Enum)e).Label.Equals(stringValue)) {
                    return (T)e;
                }
            }
            return default(T);
        }

        public static TAttribute GetAttribute<TAttribute>(this Enum value) where TAttribute : Attribute {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            System.Reflection.FieldInfo fieldInfo = type.GetField(name);
            return fieldInfo.GetCustomAttributes(false).OfType<TAttribute>().SingleOrDefault();
        }

        public static IEnumerable<string> GetLabels(Type enumType) {
            Collection<string> labels = new Collection<string>();
            foreach(Enum e in Enum.GetValues(enumType)) {
                labels.Add(GetAttribute<HeaderInfoAttribute>(e).Label);
            }
            return labels;
        }

        public static IEnumerable<string> GetQueryValues(Type enumType) {
            Collection<string> labels = new Collection<string>();
            foreach(Enum e in Enum.GetValues(enumType)) {
                labels.Add(GetAttribute<CompareInfoAttribute>(e).QueryValue);
            }
            return labels;
        }
    }
}
