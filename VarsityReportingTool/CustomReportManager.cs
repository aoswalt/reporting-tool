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
        private enum HeaderId { HOUSE, SCHDATE, ORDDATE, ORDERNUM, VOUCHER, STYLECODE, SIZE, SPEC, NAME, 
                                WORD1, WORD2, WORD3, WORD4, COLOR1, COLOR2, COLOR3, COLOR4, ITEMCLASS, LATEREASON }
        private static Dictionary<HeaderId, ColumnHeader> headers = new Dictionary<HeaderId, ColumnHeader>();
        private static List<string> stringComparisons = new List<string>(new string[] { "LIKE", "NOT LIKE", "IN", "NOT IN" });
        private static List<string> numberComparisons = new List<string>(new string[] { "=", "<>", ">", ">=", "<", "<=", "IN", "NOT IN" });
        private static List<string> dateComparisons = new List<string>(new string[] { "=", "<>", ">", ">=", "<", "<=" });
        private static Dictionary<HeaderType, List<string>> comparisons = new Dictionary<HeaderType, List<string>>();

        static CustomReportManager() {
            headers.Add(HeaderId.HOUSE, new ColumnHeader(HeaderId.HOUSE, "House", HeaderType.String));
            headers.Add(HeaderId.SCHDATE, new ColumnHeader(HeaderId.SCHDATE, "Schedule Date", HeaderType.Date));
            headers.Add(HeaderId.ORDDATE, new ColumnHeader(HeaderId.ORDDATE, "Order Date", HeaderType.Date));
            headers.Add(HeaderId.ORDERNUM, new ColumnHeader(HeaderId.ORDERNUM, "Order Number", HeaderType.Integer));
            headers.Add(HeaderId.VOUCHER, new ColumnHeader(HeaderId.VOUCHER, "Voucher", HeaderType.Integer));
            headers.Add(HeaderId.STYLECODE, new ColumnHeader(HeaderId.STYLECODE, "Style Code", HeaderType.String));
            headers.Add(HeaderId.SIZE, new ColumnHeader(HeaderId.SIZE, "Size", HeaderType.Decimal));
            headers.Add(HeaderId.SPEC, new ColumnHeader(HeaderId.SPEC, "Spec", HeaderType.Decimal));
            headers.Add(HeaderId.NAME, new ColumnHeader(HeaderId.NAME, "Name", HeaderType.String));
            headers.Add(HeaderId.WORD1, new ColumnHeader(HeaderId.WORD1, "Word 1", HeaderType.String));
            headers.Add(HeaderId.WORD2, new ColumnHeader(HeaderId.WORD2, "Word 2", HeaderType.String));
            headers.Add(HeaderId.WORD3, new ColumnHeader(HeaderId.WORD3, "Word 3", HeaderType.String));
            headers.Add(HeaderId.WORD4, new ColumnHeader(HeaderId.WORD4, "Word 4", HeaderType.String));
            headers.Add(HeaderId.COLOR1, new ColumnHeader(HeaderId.COLOR1, "Color 1", HeaderType.String));
            headers.Add(HeaderId.COLOR2, new ColumnHeader(HeaderId.COLOR2, "Color 2", HeaderType.String));
            headers.Add(HeaderId.COLOR3, new ColumnHeader(HeaderId.COLOR3, "Color 3", HeaderType.String));
            headers.Add(HeaderId.COLOR4, new ColumnHeader(HeaderId.COLOR4, "Color 4", HeaderType.String));
            headers.Add(HeaderId.ITEMCLASS, new ColumnHeader(HeaderId.ITEMCLASS, "Class", HeaderType.String));
            headers.Add(HeaderId.LATEREASON, new ColumnHeader(HeaderId.LATEREASON, "Late Reason", HeaderType.String));

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

            // sort columns based on order on form
            customReportColumns.Sort(
                (c1, c2) => 
                    (c1.getPanel().Parent.Controls.GetChildIndex(c1.getPanel()).CompareTo(c2.getPanel().Parent.Controls.GetChildIndex(c2.getPanel()))));

            foreach(Column column in customReportColumns) {
                string comparisonValue = ((string)column.comparisonComboBox.SelectedValue);
                string entryValue = getEntryValue(column);

                switch(column.headerId) {
                    case HeaderId.HOUSE: 
                        if(!includedHeaders.Contains(column.headerId)) {
                            selectColumns.Add(String.Format("det.dhous AS \"{0}\"", headers[column.headerId].Description));
                            innerSelectColumns.Add("d.dhous");
                            includedHeaders.Add(column.headerId);
                        }

                        if(column.entryField.Text != "") {
                            whereClauses.Add(String.Format("(UPPER(TRIM(d.dhous)) {0} {1})", comparisonValue, entryValue));
                        }
                        break;
                    case HeaderId.SCHDATE:
                        if(!includedHeaders.Contains(column.headerId)) {
                            selectColumns.Add(String.Format("det.scdat AS \"{0}\"", headers[column.headerId].Description));
                            innerSelectColumns.Add("DATE(d.dsccy||d.dscyr||'-'||RIGHT('00'||d.dscmo, 2)||'-'||RIGHT('00'||d.dscda, 2)) AS scdat");
                            includedHeaders.Add(column.headerId);
                        }

                        if(((DateTimePicker)column.entryField).Checked) {
                            whereClauses.Add(String.Format(@"(CASE WHEN d.dscda = 0 THEN NULL ELSE 
                                                              DATE(d.dsccy||d.dscyr||'-'||RIGHT('00'||d.dscmo, 2)||'-'||RIGHT('00'||d.dscda, 2)) END) {0} DATE({1}) ", 
                                                            comparisonValue, entryValue));
                        }
                        break;
                    case HeaderId.ORDDATE:
                        if(!includedHeaders.Contains(column.headerId)) {
                            selectColumns.Add(String.Format("det.endat AS \"{0}\"", headers[column.headerId].Description));
                            innerSelectColumns.Add("DATE(d.dorcy||d.doryr||'-'||RIGHT('00'||d.dormo, 2)||'-'||RIGHT('00'||d.dorda, 2)) AS endat");
                            includedHeaders.Add(column.headerId);
                        }

                        if(((DateTimePicker)column.entryField).Checked) {
                            whereClauses.Add(String.Format(@"(CASE WHEN d.dscda = 0 THEN NULL ELSE 
                                                              DATE(d.dorcy||d.doryr||'-'||RIGHT('00'||d.dormo, 2)||'-'||RIGHT('00'||d.dorda, 2)) END) {0} DATE({1}) ",
                                                            comparisonValue, entryValue));
                        }
                        break;
                    case HeaderId.ORDERNUM:
                        if(!includedHeaders.Contains(column.headerId)) {
                            selectColumns.Add(String.Format("det.ordnr AS \"{0}\"", headers[column.headerId].Description));
                            if(!innerSelectColumns.Contains("d.ordnr")) { innerSelectColumns.Add("d.ordnr"); }
                            includedHeaders.Add(column.headerId);
                        }

                        if(column.entryField.Text != "") {
                            whereClauses.Add(String.Format("(d.ordnr {0} {1})", comparisonValue, entryValue));
                        }
                        break;
                    case HeaderId.VOUCHER:
                        if(!includedHeaders.Contains(column.headerId)) {
                            selectColumns.Add(String.Format("det.orvch AS \"{0}\"", headers[column.headerId].Description));
                            if(!innerSelectColumns.Contains("d.orvch")) { innerSelectColumns.Add("d.orvch"); }
                            includedHeaders.Add(column.headerId);
                        }

                        if(column.entryField.Text != "") {
                            whereClauses.Add(String.Format("(d.orvch {0} {1})", comparisonValue, entryValue));
                        }
                        break;
                    case HeaderId.STYLECODE:
                        if(!includedHeaders.Contains(column.headerId)) {
                            selectColumns.Add(String.Format("det.ditem AS \"{0}\"", headers[column.headerId].Description));
                            innerSelectColumns.Add("d.ditem");
                            includedHeaders.Add(column.headerId);
                        }

                        if(column.entryField.Text != "") {
                            whereClauses.Add(String.Format("(UPPER(TRIM(d.ditem)) {0} {1})", comparisonValue, entryValue));
                        }
                        break;
                    case HeaderId.SIZE:
                        if(!includedHeaders.Contains(column.headerId)) {
                            selectColumns.Add(String.Format("det.dlsiz AS \"{0}\"", headers[column.headerId].Description));
                            innerSelectColumns.Add("d.dlsiz");
                            includedHeaders.Add(column.headerId);
                        }

                        if(column.entryField.Text != "") {
                            whereClauses.Add(String.Format("(d.dlsiz {0} {1})", comparisonValue, entryValue));
                        }
                        break;
                    case HeaderId.SPEC:
                        if(!includedHeaders.Contains(column.headerId)) {
                            if(!innerSelectColumns.Contains("d.ordnr")) { innerSelectColumns.Add("d.ordnr"); }
                            if(!innerSelectColumns.Contains("d.orvch")) { innerSelectColumns.Add("d.orvch"); }
                            if(!innerSelectColumns.Contains("d.dpvch")) { innerSelectColumns.Add("d.dpvch"); }
                            selectColumns.Add(String.Format("siz.letwid AS \"{0}\"", headers[column.headerId].Description));
                            joins.Add(@"
                                LEFT JOIN (
                                            SELECT DISTINCT s.ordnr, s.orvch, s.letwid
                                            FROM VARSITYF.HLDSIZ AS s
                                ) AS siz
                                ON det.ordnr = siz.ordnr AND det.dpvch = siz.orvch");
                            includedHeaders.Add(column.headerId);
                        }

                        if(column.entryField.Text != "") {
                            whereClauses.Add(String.Format("(d.dlsiz {0} {1})", comparisonValue, entryValue));
                        }
                        break;
                    case HeaderId.NAME:
                        if(!includedHeaders.Contains(column.headerId)) {
                            if(!innerSelectColumns.Contains("d.ordnr")) { innerSelectColumns.Add("d.ordnr"); }
                            if(!innerSelectColumns.Contains("d.orvch")) { innerSelectColumns.Add("d.orvch"); }
                            selectColumns.Add(String.Format("nam.letname AS \"{0}\"", headers[column.headerId].Description));
                            joins.Add(@"
                                LEFT JOIN 
                                            DJLIBR.ORD_NAM_C 
                                 AS nam
                                ON det.ordnr = nam.ordnr AND det.orvch = nam.orvch AND nam.letname <> ''");
                            includedHeaders.Add(column.headerId);
                        }

                        if(column.entryField.Text != "") {
                            whereClauses.Add(String.Format("(d.dlsiz {0} {1})", comparisonValue, entryValue));
                        }
                        break;
                    case HeaderId.WORD1:
                        if(!includedHeaders.Contains(column.headerId)) {
                            selectColumns.Add(String.Format("det.dlwr1 AS \"{0}\"", headers[column.headerId].Description));
                            innerSelectColumns.Add("d.dlwr1");
                            includedHeaders.Add(column.headerId);
                        }

                        if(column.entryField.Text != "") {
                            whereClauses.Add(String.Format("(UPPER(TRIM(d.dlwr1)) {0} {1})", comparisonValue, entryValue));
                        }
                        break;
                    case HeaderId.WORD2:
                        if(!includedHeaders.Contains(column.headerId)) {
                            selectColumns.Add(String.Format("det.dlwr2 AS \"{0}\"", headers[column.headerId].Description));
                            innerSelectColumns.Add("d.dlwr2");
                            includedHeaders.Add(column.headerId);
                        }

                        if(column.entryField.Text != "") {
                            whereClauses.Add(String.Format("(UPPER(TRIM(d.dlwr2)) {0} {1})", comparisonValue, entryValue));
                        }
                        break;
                    case HeaderId.WORD3:
                        if(!includedHeaders.Contains(column.headerId)) {
                            selectColumns.Add(String.Format("det.dlwr3 AS \"{0}\"", headers[column.headerId].Description));
                            innerSelectColumns.Add("d.dlwr3");
                            includedHeaders.Add(column.headerId);
                        }

                        if(column.entryField.Text != "") {
                            whereClauses.Add(String.Format("(UPPER(TRIM(d.dlwr3)) {0} {1})", comparisonValue, entryValue));
                        }
                        break;
                    case HeaderId.WORD4:
                        if(!includedHeaders.Contains(column.headerId)) {
                            selectColumns.Add(String.Format("det.dlwr4 AS \"{0}\"", headers[column.headerId].Description));
                            innerSelectColumns.Add("d.dlwr4");
                            includedHeaders.Add(column.headerId);
                        }

                        if(column.entryField.Text != "") {
                            whereClauses.Add(String.Format("(UPPER(TRIM(d.dlwr4)) {0} {1})", comparisonValue, entryValue));
                        }
                        break;
                    case HeaderId.COLOR1:
                        if(!includedHeaders.Contains(column.headerId)) {
                            selectColumns.Add(String.Format("det.dclr1 AS \"{0}\"", headers[column.headerId].Description));
                            innerSelectColumns.Add("d.dclr1");
                            includedHeaders.Add(column.headerId);
                        }

                        if(column.entryField.Text != "") {
                            whereClauses.Add(String.Format("(UPPER(TRIM(d.dclr1)) {0} {1})", comparisonValue, entryValue));
                        }
                        break;
                    case HeaderId.COLOR2:
                        if(!includedHeaders.Contains(column.headerId)) {
                            selectColumns.Add(String.Format("det.dclr2 AS \"{0}\"", headers[column.headerId].Description));
                            innerSelectColumns.Add("d.dclr2");
                            includedHeaders.Add(column.headerId);
                        }

                        if(column.entryField.Text != "") {
                            whereClauses.Add(String.Format("(UPPER(TRIM(d.dclr2)) {0} {1})", comparisonValue, entryValue));
                        }
                        break;
                    case HeaderId.COLOR3:
                        if(!includedHeaders.Contains(column.headerId)) {
                            selectColumns.Add(String.Format("det.dclr3 AS \"{0}\"", headers[column.headerId].Description));
                            innerSelectColumns.Add("d.dclr3");
                            includedHeaders.Add(column.headerId);
                        }

                        if(column.entryField.Text != "") {
                            whereClauses.Add(String.Format("(UPPER(TRIM(d.dclr3)) {0} {1})", comparisonValue, entryValue));
                        }
                        break;
                    case HeaderId.COLOR4:
                        if(!includedHeaders.Contains(column.headerId)) {
                            selectColumns.Add(String.Format("det.dclr4 AS \"{0}\"", headers[column.headerId].Description));
                            innerSelectColumns.Add("d.dclr4");
                            includedHeaders.Add(column.headerId);
                        }

                        if(column.entryField.Text != "") {
                            whereClauses.Add(String.Format("(UPPER(TRIM(d.dclr4)) {0} {1})", comparisonValue, entryValue));
                        }
                        break;
                    case HeaderId.ITEMCLASS:
                        if(!includedHeaders.Contains(column.headerId)) {
                            selectColumns.Add(String.Format("det.dclas AS \"{0}\"", headers[column.headerId].Description));
                            innerSelectColumns.Add("d.dclas");
                            includedHeaders.Add(column.headerId);
                        }

                        if(column.entryField.Text != "") {
                            whereClauses.Add(String.Format("(UPPER(TRIM(d.dclas)) {0} {1})", comparisonValue, entryValue));
                        }
                        break;
                    case HeaderId.LATEREASON:
                        if(!includedHeaders.Contains(column.headerId)) {
                            selectColumns.Add(String.Format("det.dlrea AS \"{0}\"", headers[column.headerId].Description));
                            innerSelectColumns.Add("d.dlrea");
                            includedHeaders.Add(column.headerId);
                        }

                        if(column.entryField.Text != "") {
                            whereClauses.Add(String.Format("(UPPER(TRIM(d.dlrea)) {0} {1})", comparisonValue, entryValue));
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

            string combinedWhereClause = "";
            if(whereClauses.Count > 0) {
                combinedWhereClause = "WHERE " + whereClauses[0];
                for(int i = 1; i != whereClauses.Count; ++i) {
                    combinedWhereClause += " AND " + whereClauses[i];
                }
            }

            string combinedJoins = "";
            foreach(string j in joins) {
                combinedJoins += j + '\n';
            }

            string query = String.Format("SELECT {0} FROM ( SELECT {1} FROM VARSITYF.DETAIL AS d {2}) AS det {3}",
                                         combinedSelectColumns, combinedInnerSelectColumns, combinedWhereClause, combinedJoins);

            // TODO(adam): allow setting sort field(s)
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
                List<ColumnHeader> headerList = headers.Values.ToList();
                headerList.Sort((h1, h2) => (h1.Description.CompareTo(h2.Description)));
                this.headerComboBox = new ComboBox();
                this.headerComboBox.FormattingEnabled = true;
                this.headerComboBox.Size = new System.Drawing.Size(95, 21);
                this.headerComboBox.DataSource = headerList;
                this.headerComboBox.DisplayMember = "Description";
                this.headerComboBox.ValueMember = "Id";
                this.headerComboBox.SelectedIndexChanged += new System.EventHandler(headerComboBox_SelectedIndexChanged);
                this.panel.Controls.Add(this.headerComboBox);

                // compare dropdown
                this.comparisonComboBox = new ComboBox();
                this.comparisonComboBox.Size = new System.Drawing.Size(74, 21);
                this.comparisonComboBox.DataSource = comparisons[HeaderType.String].ToList();
                this.comparisonComboBox.SelectedIndexChanged += new System.EventHandler(comparisonComboBox_SelectedIndexChanged);
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
                panel.Focus();
            }

            private void comparisonComboBox_SelectedIndexChanged(object sender, EventArgs e) {
                panel.Focus();
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
