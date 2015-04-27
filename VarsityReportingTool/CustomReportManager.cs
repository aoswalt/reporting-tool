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

        public CustomReportManager(FlowLayoutPanel columnsPanel) {
            this.columnsPanel = columnsPanel;
        }

        public void AddColumn() {
            customReportColumns.Add(new Column(this, customReportColumns.Count));
            columnsPanel.Controls.Add(customReportColumns.Last().getPanel());
            updateColumns();
        }

        public void RemoveColumn() {

        }

        public void updateColumns() {
            int count = this.customReportColumns.Count;
            for(int i = 0; i != count; ++i) {
                Column column = this.customReportColumns[i];
                int columnIndexInPanel = this.columnsPanel.Controls.GetChildIndex(column.getPanel());
                column.setId(columnIndexInPanel + 1);
                column.setUpButtonEnabled(columnIndexInPanel > 0);
                column.setDownButtonEnabled(columnIndexInPanel < (count - 1));
            }
        }

        private class Column {
            private CustomReportManager manager;
            private Header header;
            private FlowLayoutPanel panel;
            private Label label;
            private ComboBox headerComboBox;
            private ComboBox comparisonComboBox;
            private Control entryField;
            private Button moveUpButton;
            private Button moveDownButton;

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
                manager.updateColumns();
            }

            private void moveDownButton_Click(object sender, EventArgs e) {
                this.panel.Parent.Controls.SetChildIndex(this.panel, this.panel.Parent.Controls.GetChildIndex(this.panel) + 1);
                manager.updateColumns();
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
