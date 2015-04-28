namespace VarsityReportingTool {
    partial class MainWindow {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.reportTabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.comboboxReportType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkOrderLimitRows = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnRunOrderReport = new System.Windows.Forms.Button();
            this.btnClearOrderEntries = new System.Windows.Forms.Button();
            this.datePickerEnterDateEnd = new System.Windows.Forms.DateTimePicker();
            this.datePickerScheduleDateStart = new System.Windows.Forms.DateTimePicker();
            this.datePickerScheduleDateEnd = new System.Windows.Forms.DateTimePicker();
            this.datePickerEnterDateStart = new System.Windows.Forms.DateTimePicker();
            this.txtOrderNumber = new System.Windows.Forms.TextBox();
            this.txtVoucher = new System.Windows.Forms.TextBox();
            this.txtHouse = new System.Windows.Forms.TextBox();
            this.txtStyleCode = new System.Windows.Forms.TextBox();
            this.txtSize = new System.Windows.Forms.TextBox();
            this.txtSpec = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txtQueryPrompts = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.chkQueryLimitRows = new System.Windows.Forms.CheckBox();
            this.txtQuery = new System.Windows.Forms.RichTextBox();
            this.btnRunQuery = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.btnRemoveColumn = new System.Windows.Forms.Button();
            this.btnAddColumn = new System.Windows.Forms.Button();
            this.chkCustomLimitRows = new System.Windows.Forms.CheckBox();
            this.cutomReportColumnsFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnRunCustomReport = new System.Windows.Forms.Button();
            this.btnClearCustomEntries = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lblRowCount = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCopySelection = new System.Windows.Forms.Button();
            this.btnOpenInExcel = new System.Windows.Forms.Button();
            this.btnCopyAll = new System.Windows.Forms.Button();
            this.dataGrid = new System.Windows.Forms.DataGridView();
            this.menuStrip1.SuspendLayout();
            this.reportTabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(564, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(89, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(381, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Cut House";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(222, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Schedule Date Start";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(222, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Schedule Date End";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(406, 102);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Spec";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(231, 102);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(27, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Size";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(31, 102);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "Style Code";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(211, 76);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(47, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "Voucher";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(16, 76);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(73, 13);
            this.label10.TabIndex = 9;
            this.label10.Text = "Order Number";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 35);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(80, 13);
            this.label11.TabIndex = 10;
            this.label11.Text = "Enter Date End";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 9);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(83, 13);
            this.label12.TabIndex = 11;
            this.label12.Text = "Enter Date Start";
            // 
            // reportTabControl
            // 
            this.reportTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reportTabControl.Controls.Add(this.tabPage1);
            this.reportTabControl.Controls.Add(this.tabPage2);
            this.reportTabControl.Controls.Add(this.tabPage3);
            this.reportTabControl.Location = new System.Drawing.Point(0, 0);
            this.reportTabControl.Name = "reportTabControl";
            this.reportTabControl.SelectedIndex = 0;
            this.reportTabControl.Size = new System.Drawing.Size(564, 197);
            this.reportTabControl.TabIndex = 12;
            this.reportTabControl.TabStop = false;
            this.reportTabControl.SelectedIndexChanged += new System.EventHandler(this.reportTabControl_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.comboboxReportType);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.chkOrderLimitRows);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Controls.Add(this.datePickerEnterDateEnd);
            this.tabPage1.Controls.Add(this.datePickerScheduleDateStart);
            this.tabPage1.Controls.Add(this.datePickerScheduleDateEnd);
            this.tabPage1.Controls.Add(this.datePickerEnterDateStart);
            this.tabPage1.Controls.Add(this.txtOrderNumber);
            this.tabPage1.Controls.Add(this.txtVoucher);
            this.tabPage1.Controls.Add(this.txtHouse);
            this.tabPage1.Controls.Add(this.txtStyleCode);
            this.tabPage1.Controls.Add(this.txtSize);
            this.tabPage1.Controls.Add(this.txtSpec);
            this.tabPage1.Controls.Add(this.label12);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.label11);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label10);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label9);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(556, 171);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Order";
            // 
            // comboboxReportType
            // 
            this.comboboxReportType.FormattingEnabled = true;
            this.comboboxReportType.Location = new System.Drawing.Point(463, 147);
            this.comboboxReportType.Name = "comboboxReportType";
            this.comboboxReportType.Size = new System.Drawing.Size(81, 21);
            this.comboboxReportType.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(391, 150);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 31;
            this.label4.Text = "Report Type";
            // 
            // chkOrderLimitRows
            // 
            this.chkOrderLimitRows.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkOrderLimitRows.AutoSize = true;
            this.chkOrderLimitRows.Checked = true;
            this.chkOrderLimitRows.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOrderLimitRows.Location = new System.Drawing.Point(8, 149);
            this.chkOrderLimitRows.Name = "chkOrderLimitRows";
            this.chkOrderLimitRows.Size = new System.Drawing.Size(120, 17);
            this.chkOrderLimitRows.TabIndex = 11;
            this.chkOrderLimitRows.Text = "Limit To 1000 Rows";
            this.chkOrderLimitRows.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnRunOrderReport);
            this.panel1.Controls.Add(this.btnClearOrderEntries);
            this.panel1.Location = new System.Drawing.Point(171, 140);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(203, 31);
            this.panel1.TabIndex = 29;
            // 
            // btnRunOrderReport
            // 
            this.btnRunOrderReport.Location = new System.Drawing.Point(3, 5);
            this.btnRunOrderReport.Name = "btnRunOrderReport";
            this.btnRunOrderReport.Size = new System.Drawing.Size(90, 23);
            this.btnRunOrderReport.TabIndex = 20;
            this.btnRunOrderReport.Text = "Run Report";
            this.btnRunOrderReport.UseVisualStyleBackColor = true;
            this.btnRunOrderReport.Click += new System.EventHandler(this.btnRunOrderReport_Click);
            // 
            // btnClearOrderEntries
            // 
            this.btnClearOrderEntries.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClearOrderEntries.Location = new System.Drawing.Point(109, 5);
            this.btnClearOrderEntries.Name = "btnClearOrderEntries";
            this.btnClearOrderEntries.Size = new System.Drawing.Size(90, 23);
            this.btnClearOrderEntries.TabIndex = 21;
            this.btnClearOrderEntries.Text = "Clear Entries";
            this.btnClearOrderEntries.UseVisualStyleBackColor = true;
            this.btnClearOrderEntries.Click += new System.EventHandler(this.btnClearOrderEntries_Click);
            // 
            // datePickerEnterDateEnd
            // 
            this.datePickerEnterDateEnd.Checked = false;
            this.datePickerEnterDateEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datePickerEnterDateEnd.Location = new System.Drawing.Point(95, 32);
            this.datePickerEnterDateEnd.Name = "datePickerEnterDateEnd";
            this.datePickerEnterDateEnd.ShowCheckBox = true;
            this.datePickerEnterDateEnd.Size = new System.Drawing.Size(100, 20);
            this.datePickerEnterDateEnd.TabIndex = 1;
            // 
            // datePickerScheduleDateStart
            // 
            this.datePickerScheduleDateStart.Checked = false;
            this.datePickerScheduleDateStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datePickerScheduleDateStart.Location = new System.Drawing.Point(331, 6);
            this.datePickerScheduleDateStart.Name = "datePickerScheduleDateStart";
            this.datePickerScheduleDateStart.ShowCheckBox = true;
            this.datePickerScheduleDateStart.Size = new System.Drawing.Size(100, 20);
            this.datePickerScheduleDateStart.TabIndex = 2;
            this.datePickerScheduleDateStart.Value = new System.DateTime(2015, 4, 10, 10, 35, 29, 0);
            // 
            // datePickerScheduleDateEnd
            // 
            this.datePickerScheduleDateEnd.Checked = false;
            this.datePickerScheduleDateEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datePickerScheduleDateEnd.Location = new System.Drawing.Point(331, 32);
            this.datePickerScheduleDateEnd.Name = "datePickerScheduleDateEnd";
            this.datePickerScheduleDateEnd.ShowCheckBox = true;
            this.datePickerScheduleDateEnd.Size = new System.Drawing.Size(100, 20);
            this.datePickerScheduleDateEnd.TabIndex = 3;
            // 
            // datePickerEnterDateStart
            // 
            this.datePickerEnterDateStart.Checked = false;
            this.datePickerEnterDateStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datePickerEnterDateStart.Location = new System.Drawing.Point(95, 6);
            this.datePickerEnterDateStart.Name = "datePickerEnterDateStart";
            this.datePickerEnterDateStart.ShowCheckBox = true;
            this.datePickerEnterDateStart.Size = new System.Drawing.Size(100, 20);
            this.datePickerEnterDateStart.TabIndex = 0;
            // 
            // txtOrderNumber
            // 
            this.txtOrderNumber.Location = new System.Drawing.Point(95, 73);
            this.txtOrderNumber.Name = "txtOrderNumber";
            this.txtOrderNumber.Size = new System.Drawing.Size(100, 20);
            this.txtOrderNumber.TabIndex = 4;
            this.txtOrderNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtOrderNumber_KeyPress);
            // 
            // txtVoucher
            // 
            this.txtVoucher.Location = new System.Drawing.Point(264, 73);
            this.txtVoucher.Name = "txtVoucher";
            this.txtVoucher.Size = new System.Drawing.Size(100, 20);
            this.txtVoucher.TabIndex = 5;
            this.txtVoucher.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtVoucher_KeyPress);
            // 
            // txtHouse
            // 
            this.txtHouse.Location = new System.Drawing.Point(444, 73);
            this.txtHouse.Name = "txtHouse";
            this.txtHouse.Size = new System.Drawing.Size(100, 20);
            this.txtHouse.TabIndex = 6;
            // 
            // txtStyleCode
            // 
            this.txtStyleCode.Location = new System.Drawing.Point(95, 99);
            this.txtStyleCode.Name = "txtStyleCode";
            this.txtStyleCode.Size = new System.Drawing.Size(100, 20);
            this.txtStyleCode.TabIndex = 7;
            // 
            // txtSize
            // 
            this.txtSize.Location = new System.Drawing.Point(264, 99);
            this.txtSize.Name = "txtSize";
            this.txtSize.Size = new System.Drawing.Size(100, 20);
            this.txtSize.TabIndex = 8;
            this.txtSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSize_KeyPress);
            // 
            // txtSpec
            // 
            this.txtSpec.Location = new System.Drawing.Point(444, 99);
            this.txtSpec.Name = "txtSpec";
            this.txtSpec.Size = new System.Drawing.Size(100, 20);
            this.txtSpec.TabIndex = 9;
            this.txtSpec.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSpec_KeyPress);
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.txtQueryPrompts);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.chkQueryLimitRows);
            this.tabPage2.Controls.Add(this.txtQuery);
            this.tabPage2.Controls.Add(this.btnRunQuery);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(556, 171);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Query";
            // 
            // txtQueryPrompts
            // 
            this.txtQueryPrompts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtQueryPrompts.Location = new System.Drawing.Point(43, 119);
            this.txtQueryPrompts.Name = "txtQueryPrompts";
            this.txtQueryPrompts.Size = new System.Drawing.Size(513, 20);
            this.txtQueryPrompts.TabIndex = 35;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(-3, 122);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 34;
            this.label5.Text = "Prompts:";
            // 
            // chkQueryLimitRows
            // 
            this.chkQueryLimitRows.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkQueryLimitRows.AutoSize = true;
            this.chkQueryLimitRows.Checked = true;
            this.chkQueryLimitRows.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkQueryLimitRows.Location = new System.Drawing.Point(8, 149);
            this.chkQueryLimitRows.Name = "chkQueryLimitRows";
            this.chkQueryLimitRows.Size = new System.Drawing.Size(120, 17);
            this.chkQueryLimitRows.TabIndex = 33;
            this.chkQueryLimitRows.Text = "Limit To 1000 Rows";
            this.chkQueryLimitRows.UseVisualStyleBackColor = true;
            // 
            // txtQuery
            // 
            this.txtQuery.AcceptsTab = true;
            this.txtQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtQuery.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtQuery.Location = new System.Drawing.Point(0, 0);
            this.txtQuery.Name = "txtQuery";
            this.txtQuery.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtQuery.Size = new System.Drawing.Size(556, 113);
            this.txtQuery.TabIndex = 32;
            this.txtQuery.Text = "";
            this.txtQuery.WordWrap = false;
            // 
            // btnRunQuery
            // 
            this.btnRunQuery.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnRunQuery.Location = new System.Drawing.Point(227, 145);
            this.btnRunQuery.Name = "btnRunQuery";
            this.btnRunQuery.Size = new System.Drawing.Size(90, 23);
            this.btnRunQuery.TabIndex = 25;
            this.btnRunQuery.Text = "Run Query";
            this.btnRunQuery.UseVisualStyleBackColor = true;
            this.btnRunQuery.Click += new System.EventHandler(this.btnRunQuery_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage3.Controls.Add(this.btnRemoveColumn);
            this.tabPage3.Controls.Add(this.btnAddColumn);
            this.tabPage3.Controls.Add(this.chkCustomLimitRows);
            this.tabPage3.Controls.Add(this.cutomReportColumnsFlowPanel);
            this.tabPage3.Controls.Add(this.panel3);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(556, 171);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Custom";
            // 
            // btnRemoveColumn
            // 
            this.btnRemoveColumn.Enabled = false;
            this.btnRemoveColumn.Location = new System.Drawing.Point(11, 31);
            this.btnRemoveColumn.Name = "btnRemoveColumn";
            this.btnRemoveColumn.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveColumn.TabIndex = 38;
            this.btnRemoveColumn.Text = "Remove";
            this.btnRemoveColumn.UseVisualStyleBackColor = true;
            this.btnRemoveColumn.Click += new System.EventHandler(this.btnRemoveColumn_Click);
            // 
            // btnAddColumn
            // 
            this.btnAddColumn.Location = new System.Drawing.Point(11, 6);
            this.btnAddColumn.Name = "btnAddColumn";
            this.btnAddColumn.Size = new System.Drawing.Size(75, 23);
            this.btnAddColumn.TabIndex = 32;
            this.btnAddColumn.Text = "Add";
            this.btnAddColumn.UseVisualStyleBackColor = true;
            this.btnAddColumn.Click += new System.EventHandler(this.btnAddColumn_Click);
            // 
            // chkCustomLimitRows
            // 
            this.chkCustomLimitRows.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkCustomLimitRows.AutoSize = true;
            this.chkCustomLimitRows.Checked = true;
            this.chkCustomLimitRows.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCustomLimitRows.Location = new System.Drawing.Point(8, 149);
            this.chkCustomLimitRows.Name = "chkCustomLimitRows";
            this.chkCustomLimitRows.Size = new System.Drawing.Size(120, 17);
            this.chkCustomLimitRows.TabIndex = 30;
            this.chkCustomLimitRows.Text = "Limit To 1000 Rows";
            this.chkCustomLimitRows.UseVisualStyleBackColor = true;
            // 
            // cutomReportColumnsFlowPanel
            // 
            this.cutomReportColumnsFlowPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.cutomReportColumnsFlowPanel.AutoScroll = true;
            this.cutomReportColumnsFlowPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.cutomReportColumnsFlowPanel.Location = new System.Drawing.Point(151, 0);
            this.cutomReportColumnsFlowPanel.Name = "cutomReportColumnsFlowPanel";
            this.cutomReportColumnsFlowPanel.Size = new System.Drawing.Size(405, 139);
            this.cutomReportColumnsFlowPanel.TabIndex = 0;
            this.cutomReportColumnsFlowPanel.WrapContents = false;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panel3.Controls.Add(this.btnRunCustomReport);
            this.panel3.Controls.Add(this.btnClearCustomEntries);
            this.panel3.Location = new System.Drawing.Point(171, 140);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(203, 31);
            this.panel3.TabIndex = 31;
            // 
            // btnRunCustomReport
            // 
            this.btnRunCustomReport.Location = new System.Drawing.Point(3, 5);
            this.btnRunCustomReport.Name = "btnRunCustomReport";
            this.btnRunCustomReport.Size = new System.Drawing.Size(90, 23);
            this.btnRunCustomReport.TabIndex = 20;
            this.btnRunCustomReport.Text = "Run Report";
            this.btnRunCustomReport.UseVisualStyleBackColor = true;
            this.btnRunCustomReport.Click += new System.EventHandler(this.btnRunCustomReport_Click);
            // 
            // btnClearCustomEntries
            // 
            this.btnClearCustomEntries.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClearCustomEntries.Location = new System.Drawing.Point(109, 5);
            this.btnClearCustomEntries.Name = "btnClearCustomEntries";
            this.btnClearCustomEntries.Size = new System.Drawing.Size(90, 23);
            this.btnClearCustomEntries.TabIndex = 21;
            this.btnClearCustomEntries.Text = "Clear Entries";
            this.btnClearCustomEntries.UseVisualStyleBackColor = true;
            this.btnClearCustomEntries.Click += new System.EventHandler(this.btnClearCustomEntries_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.reportTabControl);
            this.splitContainer1.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer1.Panel1MinSize = 200;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lblRowCount);
            this.splitContainer1.Panel2.Controls.Add(this.panel2);
            this.splitContainer1.Panel2.Controls.Add(this.dataGrid);
            this.splitContainer1.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer1.Size = new System.Drawing.Size(564, 542);
            this.splitContainer1.SplitterDistance = 200;
            this.splitContainer1.TabIndex = 13;
            this.splitContainer1.TabStop = false;
            // 
            // lblRowCount
            // 
            this.lblRowCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblRowCount.AutoSize = true;
            this.lblRowCount.Location = new System.Drawing.Point(12, 312);
            this.lblRowCount.Name = "lblRowCount";
            this.lblRowCount.Size = new System.Drawing.Size(0, 13);
            this.lblRowCount.TabIndex = 31;
            // 
            // panel2
            // 
            this.panel2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.panel2.Controls.Add(this.btnCopySelection);
            this.panel2.Controls.Add(this.btnOpenInExcel);
            this.panel2.Controls.Add(this.btnCopyAll);
            this.panel2.Location = new System.Drawing.Point(132, 302);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(292, 33);
            this.panel2.TabIndex = 30;
            // 
            // btnCopySelection
            // 
            this.btnCopySelection.Location = new System.Drawing.Point(195, 5);
            this.btnCopySelection.Name = "btnCopySelection";
            this.btnCopySelection.Size = new System.Drawing.Size(90, 23);
            this.btnCopySelection.TabIndex = 24;
            this.btnCopySelection.Text = "Copy Selection";
            this.btnCopySelection.UseVisualStyleBackColor = true;
            this.btnCopySelection.Click += new System.EventHandler(this.btnCopySelection_Click);
            // 
            // btnOpenInExcel
            // 
            this.btnOpenInExcel.Location = new System.Drawing.Point(3, 5);
            this.btnOpenInExcel.Name = "btnOpenInExcel";
            this.btnOpenInExcel.Size = new System.Drawing.Size(90, 23);
            this.btnOpenInExcel.TabIndex = 22;
            this.btnOpenInExcel.Text = "Open In Excel";
            this.btnOpenInExcel.UseVisualStyleBackColor = true;
            this.btnOpenInExcel.Click += new System.EventHandler(this.btnOpenInExcel_Click);
            // 
            // btnCopyAll
            // 
            this.btnCopyAll.Location = new System.Drawing.Point(99, 5);
            this.btnCopyAll.Name = "btnCopyAll";
            this.btnCopyAll.Size = new System.Drawing.Size(90, 23);
            this.btnCopyAll.TabIndex = 23;
            this.btnCopyAll.Text = "Copy All";
            this.btnCopyAll.UseVisualStyleBackColor = true;
            this.btnCopyAll.Click += new System.EventHandler(this.btnCopyAll_Click);
            // 
            // dataGrid
            // 
            this.dataGrid.AllowUserToAddRows = false;
            this.dataGrid.AllowUserToDeleteRows = false;
            this.dataGrid.AllowUserToOrderColumns = true;
            this.dataGrid.AllowUserToResizeRows = false;
            this.dataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGrid.Location = new System.Drawing.Point(4, 3);
            this.dataGrid.Name = "dataGrid";
            this.dataGrid.ReadOnly = true;
            this.dataGrid.RowHeadersVisible = false;
            this.dataGrid.Size = new System.Drawing.Size(556, 298);
            this.dataGrid.TabIndex = 0;
            // 
            // MainWindow
            // 
            this.AcceptButton = this.btnRunOrderReport;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClearOrderEntries;
            this.ClientSize = new System.Drawing.Size(564, 566);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(575, 500);
            this.Name = "MainWindow";
            this.Text = "Reporting Tool";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.reportTabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TabControl reportTabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DateTimePicker datePickerEnterDateStart;
        private System.Windows.Forms.TextBox txtOrderNumber;
        private System.Windows.Forms.TextBox txtVoucher;
        private System.Windows.Forms.TextBox txtHouse;
        private System.Windows.Forms.TextBox txtStyleCode;
        private System.Windows.Forms.TextBox txtSize;
        private System.Windows.Forms.TextBox txtSpec;
        private System.Windows.Forms.DateTimePicker datePickerEnterDateEnd;
        private System.Windows.Forms.DateTimePicker datePickerScheduleDateStart;
        private System.Windows.Forms.DateTimePicker datePickerScheduleDateEnd;
        private System.Windows.Forms.Button btnClearOrderEntries;
        private System.Windows.Forms.Button btnRunOrderReport;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dataGrid;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCopySelection;
        private System.Windows.Forms.Button btnOpenInExcel;
        private System.Windows.Forms.Button btnCopyAll;
        private System.Windows.Forms.Button btnRunQuery;
        private System.Windows.Forms.RichTextBox txtQuery;
        private System.Windows.Forms.CheckBox chkOrderLimitRows;
        private System.Windows.Forms.CheckBox chkQueryLimitRows;
        private System.Windows.Forms.Label lblRowCount;
        private System.Windows.Forms.ComboBox comboboxReportType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtQueryPrompts;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.CheckBox chkCustomLimitRows;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnRunCustomReport;
        private System.Windows.Forms.Button btnClearCustomEntries;
        private System.Windows.Forms.FlowLayoutPanel cutomReportColumnsFlowPanel;
        private System.Windows.Forms.Button btnRemoveColumn;
        private System.Windows.Forms.Button btnAddColumn;
    }
}

