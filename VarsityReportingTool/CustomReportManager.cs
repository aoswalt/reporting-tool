using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VarsityReportingTool {

    class CustomReportManager {
        private enum HeaderType { String, Integer, Decimal, Date }
        private enum Header {
            [HeaderInfo(HeaderType.String, "Cut House")]    HOUSE,
            [HeaderInfo(HeaderType.Date, "Sch Date")]       SCHDATE,
            [HeaderInfo(HeaderType.Decimal, "Size")]        SIZE
        }
        private enum StringCompare { 
            [CompareInfo("LIKE")]       LIKE,
            [CompareInfo("NOT LIKE")]   NOTLIKE,
            [CompareInfo("IN")]         IN,
            [CompareInfo("NOT IN")]     NOTIN
        }
        private enum NumberCompare {
            [CompareInfo("=")]      EQUALS,
            [CompareInfo("<>")]     NOTEQUALS,
            [CompareInfo(">")]      GREATER,
            [CompareInfo(">=")]     GREATEREQUALS,
            [CompareInfo("<")]      LESS,
            [CompareInfo("<=")]     LESSEQUALS,
            [CompareInfo("IN")]     IN,
            [CompareInfo("NOT IN")] NOTIN
        }
        private List<Header> availableColumnHeaders = new List<Header>(Enum.GetValues(typeof(Header)).Cast<Header>());
        private FlowLayoutPanel columnsPanel;
        private List<Column> customReportColumns = new List<Column>();

        public CustomReportManager(FlowLayoutPanel columnsPanel) {
            this.columnsPanel = columnsPanel;
        }

        public void AddColumn() {
            customReportColumns.Add(new Column(ref availableColumnHeaders, customReportColumns.Count));
            columnsPanel.Controls.Add(customReportColumns[customReportColumns.Count - 1].getPanel());
        }

        public void RemoveColumn() {

        }

        private class HeaderInfoAttribute : Attribute {
            public HeaderInfoAttribute(HeaderType type, string label) {
                this.Type = type;
                this.Label = label;
            }

            public HeaderType Type { get; private set; }
            public string Label { get; private set; }
        }

        private class CompareInfoAttribute : Attribute {
            public CompareInfoAttribute(string queryValue) {
                this.QueryValue = queryValue;
            }

            public string QueryValue { get; private set; }
        }

        private class Column {
            private Header header;
            private FlowLayoutPanel panel;
            private Label label;
            private ComboBox headerComboBox;
            private ComboBox comparisonComboBox;
            private Control entryField;
            private Button moveUpButton;
            private Button moveDownButton;

            public Column(ref List<Header> availableColumnHeaders, int id) {
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
                this.panel.Controls.Add(this.label);

                // header dropdown
                this.headerComboBox = new ComboBox();
                this.headerComboBox.FormattingEnabled = true;
                this.headerComboBox.Size = new System.Drawing.Size(95, 21);
                // TODO: unique values only?
                //this.headerComboBox.DataSource = availableColumnHeaders;
                this.headerComboBox.DataSource = Enum.GetValues(typeof(Header));
                this.headerComboBox.SelectedIndexChanged += new System.EventHandler(headerComboBox_SelectedIndexChanged);
                this.panel.Controls.Add(this.headerComboBox);

                // compare dropdown
                this.comparisonComboBox = new ComboBox();
                this.comparisonComboBox.FormattingEnabled = true;
                this.comparisonComboBox.Size = new System.Drawing.Size(74, 21);
                this.comparisonComboBox.DataSource = Enum.GetValues(typeof(NumberCompare));
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
                this.panel.Controls.Add(this.moveUpButton);

                // move down button
                this.moveDownButton = new Button();
                this.moveDownButton.Size = new System.Drawing.Size(23, 23);
                this.moveDownButton.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
                this.moveDownButton.Text = "▼";
                this.moveDownButton.UseVisualStyleBackColor = true;
                this.moveDownButton.Click += new System.EventHandler(moveDownButton_Click);
                this.panel.Controls.Add(this.moveDownButton);
            }

            public FlowLayoutPanel getPanel() {
                return panel;
            }

            private void headerComboBox_SelectedIndexChanged(object sender, EventArgs e) {
                Header newHeader = ((Header)this.headerComboBox.SelectedValue);

                HeaderType headerType = EnumExtensions.GetAttribute<HeaderInfoAttribute>(header).Type;
                HeaderType newHeaderType = EnumExtensions.GetAttribute<HeaderInfoAttribute>(newHeader).Type;
                if(headerType != newHeaderType) {
                    switch(newHeaderType) {
                        case HeaderType.Date:
                            this.comparisonComboBox.DataSource = Enum.GetValues(typeof(NumberCompare));
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
                            this.comparisonComboBox.DataSource = Enum.GetValues(typeof(NumberCompare));
                            this.panel.Controls.Remove(this.entryField);
                            this.entryField = new TextBox();
                            this.entryField.Size = new System.Drawing.Size(118, 20);
                            this.entryField.KeyPress += new System.Windows.Forms.KeyPressEventHandler(entryField_Decimal_KeyPress);
                            this.panel.Controls.Add(this.entryField);
                            this.panel.Controls.SetChildIndex(this.entryField, this.panel.Controls.GetChildIndex(this.comparisonComboBox) + 1);
                            break;
                        case HeaderType.Integer:
                            this.comparisonComboBox.DataSource = Enum.GetValues(typeof(NumberCompare));
                            this.panel.Controls.Remove(this.entryField);
                            this.entryField = new TextBox();
                            this.entryField.Size = new System.Drawing.Size(118, 20);
                            this.entryField.KeyPress += new System.Windows.Forms.KeyPressEventHandler(entryField_Integer_KeyPress);
                            this.panel.Controls.Add(this.entryField);
                            this.panel.Controls.SetChildIndex(this.entryField, this.panel.Controls.GetChildIndex(this.comparisonComboBox) + 1);
                            break;
                        case HeaderType.String:
                            this.comparisonComboBox.DataSource = Enum.GetValues(typeof(StringCompare));
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

            }

            private void moveDownButton_Click(object sender, EventArgs e) {

            }
        }
    }

    static class EnumExtensions {
        public static TAttribute GetAttribute<TAttribute>(this Enum value) where TAttribute : Attribute {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            System.Reflection.FieldInfo fieldInfo = type.GetField(name);
            return fieldInfo.GetCustomAttributes(false).OfType<TAttribute>().SingleOrDefault();
        }
    }
}
