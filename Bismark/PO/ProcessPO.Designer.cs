namespace Bismark.GUI
{
    partial class ProcessPO
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtQuantity = new System.Windows.Forms.TextBox();
            this.txtPrice = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnDeny = new System.Windows.Forms.Button();
            this.lblReason = new System.Windows.Forms.Label();
            this.txtReason = new System.Windows.Forms.TextBox();
            this.btnApprove = new System.Windows.Forms.Button();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.lblSource = new System.Windows.Forms.Label();
            this.txtJustification = new System.Windows.Forms.TextBox();
            this.txtSource = new System.Windows.Forms.TextBox();
            this.lblJustification = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lblOrderItems = new System.Windows.Forms.Label();
            this.lblOrders = new System.Windows.Forms.Label();
            this.txtGrandTotal = new System.Windows.Forms.TextBox();
            this.lblGrandTotal = new System.Windows.Forms.Label();
            this.txtTaxTotal = new System.Windows.Forms.TextBox();
            this.lblTaxTotal = new System.Windows.Forms.Label();
            this.txtSubTotal = new System.Windows.Forms.TextBox();
            this.lblSubTotal = new System.Windows.Forms.Label();
            this.gdvItems = new System.Windows.Forms.DataGridView();
            this.gdvOrders = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoOrderId = new System.Windows.Forms.RadioButton();
            this.rdoEmployee = new System.Windows.Forms.RadioButton();
            this.rdoDepartment = new System.Windows.Forms.RadioButton();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblFilter = new System.Windows.Forms.Label();
            this.cboFilter = new System.Windows.Forms.ComboBox();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblDateFrom = new System.Windows.Forms.Label();
            this.lblTo = new System.Windows.Forms.Label();
            this.btnGo = new System.Windows.Forms.Button();
            this.chkDateRange = new System.Windows.Forms.CheckBox();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.lblLastName = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.txtOrderId = new System.Windows.Forms.TextBox();
            this.lblOrderId = new System.Windows.Forms.Label();
            this.gbxItemDetails = new System.Windows.Forms.GroupBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gdvItems)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdvOrders)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.txtQuantity);
            this.panel1.Controls.Add(this.txtPrice);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.btnDeny);
            this.panel1.Controls.Add(this.lblReason);
            this.panel1.Controls.Add(this.txtReason);
            this.panel1.Controls.Add(this.btnApprove);
            this.panel1.Controls.Add(this.lblQuantity);
            this.panel1.Controls.Add(this.lblSource);
            this.panel1.Controls.Add(this.txtJustification);
            this.panel1.Controls.Add(this.txtSource);
            this.panel1.Controls.Add(this.lblJustification);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtName);
            this.panel1.Controls.Add(this.txtDescription);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.lblOrderItems);
            this.panel1.Controls.Add(this.lblOrders);
            this.panel1.Controls.Add(this.txtGrandTotal);
            this.panel1.Controls.Add(this.lblGrandTotal);
            this.panel1.Controls.Add(this.txtTaxTotal);
            this.panel1.Controls.Add(this.lblTaxTotal);
            this.panel1.Controls.Add(this.txtSubTotal);
            this.panel1.Controls.Add(this.lblSubTotal);
            this.panel1.Controls.Add(this.gdvItems);
            this.panel1.Controls.Add(this.gdvOrders);
            this.panel1.Controls.Add(this.gbxItemDetails);
            this.panel1.Location = new System.Drawing.Point(0, 136);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1007, 434);
            this.panel1.TabIndex = 0;
            // 
            // txtQuantity
            // 
            this.txtQuantity.BackColor = System.Drawing.SystemColors.Control;
            this.txtQuantity.ForeColor = System.Drawing.SystemColors.GrayText;
            this.txtQuantity.Location = new System.Drawing.Point(695, 163);
            this.txtQuantity.Name = "txtQuantity";
            this.txtQuantity.ReadOnly = true;
            this.txtQuantity.Size = new System.Drawing.Size(42, 20);
            this.txtQuantity.TabIndex = 67;
            this.txtQuantity.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ModifyItemProperty);
            this.txtQuantity.Validating += new System.ComponentModel.CancelEventHandler(this.ValidateInput);
            // 
            // txtPrice
            // 
            this.txtPrice.BackColor = System.Drawing.SystemColors.Control;
            this.txtPrice.ForeColor = System.Drawing.SystemColors.GrayText;
            this.txtPrice.Location = new System.Drawing.Point(577, 163);
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.ReadOnly = true;
            this.txtPrice.Size = new System.Drawing.Size(57, 20);
            this.txtPrice.TabIndex = 66;
            this.txtPrice.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ModifyItemProperty);
            this.txtPrice.Validating += new System.ComponentModel.CancelEventHandler(this.ValidateInput);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.SlateGray;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(403, 221);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(87, 25);
            this.button1.TabIndex = 65;
            this.button1.Text = "CLOSE ORDER";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.CloseOrder);
            // 
            // btnDeny
            // 
            this.btnDeny.BackColor = System.Drawing.Color.SlateGray;
            this.btnDeny.FlatAppearance.BorderSize = 0;
            this.btnDeny.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeny.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeny.ForeColor = System.Drawing.Color.White;
            this.btnDeny.Location = new System.Drawing.Point(886, 286);
            this.btnDeny.Name = "btnDeny";
            this.btnDeny.Size = new System.Drawing.Size(75, 25);
            this.btnDeny.TabIndex = 64;
            this.btnDeny.Text = "DENY";
            this.btnDeny.UseVisualStyleBackColor = false;
            this.btnDeny.Click += new System.EventHandler(this.Process);
            // 
            // lblReason
            // 
            this.lblReason.AutoSize = true;
            this.lblReason.Location = new System.Drawing.Point(775, 166);
            this.lblReason.Name = "lblReason";
            this.lblReason.Size = new System.Drawing.Size(47, 13);
            this.lblReason.TabIndex = 63;
            this.lblReason.Text = "Reason:";
            // 
            // txtReason
            // 
            this.txtReason.BackColor = System.Drawing.SystemColors.Control;
            this.txtReason.ForeColor = System.Drawing.SystemColors.GrayText;
            this.txtReason.Location = new System.Drawing.Point(825, 163);
            this.txtReason.Name = "txtReason";
            this.txtReason.ReadOnly = true;
            this.txtReason.Size = new System.Drawing.Size(160, 20);
            this.txtReason.TabIndex = 62;
            this.txtReason.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ModifyItemProperty);
            this.txtReason.Validating += new System.ComponentModel.CancelEventHandler(this.ValidateInput);
            // 
            // btnApprove
            // 
            this.btnApprove.BackColor = System.Drawing.Color.SlateGray;
            this.btnApprove.FlatAppearance.BorderSize = 0;
            this.btnApprove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApprove.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApprove.ForeColor = System.Drawing.Color.White;
            this.btnApprove.Location = new System.Drawing.Point(886, 257);
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Size = new System.Drawing.Size(75, 25);
            this.btnApprove.TabIndex = 26;
            this.btnApprove.Text = "APPROVE";
            this.btnApprove.UseVisualStyleBackColor = false;
            this.btnApprove.Click += new System.EventHandler(this.Process);
            // 
            // lblQuantity
            // 
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.Location = new System.Drawing.Point(640, 166);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(49, 13);
            this.lblQuantity.TabIndex = 61;
            this.lblQuantity.Text = "Quantity:";
            // 
            // lblSource
            // 
            this.lblSource.AutoSize = true;
            this.lblSource.Location = new System.Drawing.Point(775, 140);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(44, 13);
            this.lblSource.TabIndex = 60;
            this.lblSource.Text = "Source:";
            // 
            // txtJustification
            // 
            this.txtJustification.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtJustification.ForeColor = System.Drawing.SystemColors.GrayText;
            this.txtJustification.Location = new System.Drawing.Point(823, 51);
            this.txtJustification.Multiline = true;
            this.txtJustification.Name = "txtJustification";
            this.txtJustification.ReadOnly = true;
            this.txtJustification.Size = new System.Drawing.Size(160, 80);
            this.txtJustification.TabIndex = 58;
            // 
            // txtSource
            // 
            this.txtSource.BackColor = System.Drawing.SystemColors.Control;
            this.txtSource.ForeColor = System.Drawing.SystemColors.GrayText;
            this.txtSource.Location = new System.Drawing.Point(825, 137);
            this.txtSource.Name = "txtSource";
            this.txtSource.ReadOnly = true;
            this.txtSource.Size = new System.Drawing.Size(160, 20);
            this.txtSource.TabIndex = 59;
            this.txtSource.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ModifyItemProperty);
            this.txtSource.Validating += new System.ComponentModel.CancelEventHandler(this.ValidateInput);
            // 
            // lblJustification
            // 
            this.lblJustification.AutoSize = true;
            this.lblJustification.Location = new System.Drawing.Point(754, 54);
            this.lblJustification.Name = "lblJustification";
            this.lblJustification.Size = new System.Drawing.Size(65, 13);
            this.lblJustification.TabIndex = 57;
            this.lblJustification.Text = "Justification:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(533, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 51;
            this.label2.Text = "Name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(537, 166);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 55;
            this.label3.Text = "Price:";
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.SystemColors.Control;
            this.txtName.ForeColor = System.Drawing.SystemColors.GrayText;
            this.txtName.Location = new System.Drawing.Point(577, 51);
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.Size = new System.Drawing.Size(160, 20);
            this.txtName.TabIndex = 52;
            // 
            // txtDescription
            // 
            this.txtDescription.BackColor = System.Drawing.SystemColors.Control;
            this.txtDescription.ForeColor = System.Drawing.SystemColors.GrayText;
            this.txtDescription.Location = new System.Drawing.Point(577, 77);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ReadOnly = true;
            this.txtDescription.Size = new System.Drawing.Size(160, 80);
            this.txtDescription.TabIndex = 54;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(508, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 53;
            this.label4.Text = "Description:";
            // 
            // lblOrderItems
            // 
            this.lblOrderItems.AutoSize = true;
            this.lblOrderItems.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOrderItems.ForeColor = System.Drawing.Color.Black;
            this.lblOrderItems.Location = new System.Drawing.Point(15, 238);
            this.lblOrderItems.Name = "lblOrderItems";
            this.lblOrderItems.Size = new System.Drawing.Size(220, 16);
            this.lblOrderItems.TabIndex = 9;
            this.lblOrderItems.Text = "PURCHASE ORDER DETAILS:";
            // 
            // lblOrders
            // 
            this.lblOrders.AutoSize = true;
            this.lblOrders.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOrders.ForeColor = System.Drawing.Color.Black;
            this.lblOrders.Location = new System.Drawing.Point(18, 18);
            this.lblOrders.Name = "lblOrders";
            this.lblOrders.Size = new System.Drawing.Size(163, 16);
            this.lblOrders.TabIndex = 8;
            this.lblOrders.Text = "PURCHASE ORDERS:";
            // 
            // txtGrandTotal
            // 
            this.txtGrandTotal.BackColor = System.Drawing.SystemColors.Control;
            this.txtGrandTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGrandTotal.ForeColor = System.Drawing.Color.Black;
            this.txtGrandTotal.Location = new System.Drawing.Point(390, 195);
            this.txtGrandTotal.Name = "txtGrandTotal";
            this.txtGrandTotal.ReadOnly = true;
            this.txtGrandTotal.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtGrandTotal.Size = new System.Drawing.Size(100, 20);
            this.txtGrandTotal.TabIndex = 7;
            // 
            // lblGrandTotal
            // 
            this.lblGrandTotal.AutoSize = true;
            this.lblGrandTotal.ForeColor = System.Drawing.Color.Black;
            this.lblGrandTotal.Location = new System.Drawing.Point(324, 197);
            this.lblGrandTotal.Name = "lblGrandTotal";
            this.lblGrandTotal.Size = new System.Drawing.Size(66, 13);
            this.lblGrandTotal.TabIndex = 6;
            this.lblGrandTotal.Text = "Grand Total:";
            // 
            // txtTaxTotal
            // 
            this.txtTaxTotal.BackColor = System.Drawing.SystemColors.Control;
            this.txtTaxTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTaxTotal.ForeColor = System.Drawing.Color.Black;
            this.txtTaxTotal.Location = new System.Drawing.Point(224, 195);
            this.txtTaxTotal.Name = "txtTaxTotal";
            this.txtTaxTotal.ReadOnly = true;
            this.txtTaxTotal.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtTaxTotal.Size = new System.Drawing.Size(100, 20);
            this.txtTaxTotal.TabIndex = 5;
            // 
            // lblTaxTotal
            // 
            this.lblTaxTotal.AutoSize = true;
            this.lblTaxTotal.ForeColor = System.Drawing.Color.Black;
            this.lblTaxTotal.Location = new System.Drawing.Point(169, 197);
            this.lblTaxTotal.Name = "lblTaxTotal";
            this.lblTaxTotal.Size = new System.Drawing.Size(55, 13);
            this.lblTaxTotal.TabIndex = 4;
            this.lblTaxTotal.Text = "Tax Total:";
            // 
            // txtSubTotal
            // 
            this.txtSubTotal.BackColor = System.Drawing.SystemColors.Control;
            this.txtSubTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSubTotal.ForeColor = System.Drawing.Color.Black;
            this.txtSubTotal.Location = new System.Drawing.Point(69, 195);
            this.txtSubTotal.Name = "txtSubTotal";
            this.txtSubTotal.ReadOnly = true;
            this.txtSubTotal.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtSubTotal.Size = new System.Drawing.Size(100, 20);
            this.txtSubTotal.TabIndex = 3;
            // 
            // lblSubTotal
            // 
            this.lblSubTotal.AutoSize = true;
            this.lblSubTotal.ForeColor = System.Drawing.Color.Black;
            this.lblSubTotal.Location = new System.Drawing.Point(13, 197);
            this.lblSubTotal.Name = "lblSubTotal";
            this.lblSubTotal.Size = new System.Drawing.Size(56, 13);
            this.lblSubTotal.TabIndex = 2;
            this.lblSubTotal.Text = "Sub Total:";
            // 
            // gdvItems
            // 
            this.gdvItems.AllowUserToAddRows = false;
            this.gdvItems.AllowUserToDeleteRows = false;
            this.gdvItems.BackgroundColor = System.Drawing.Color.LightSlateGray;
            this.gdvItems.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gdvItems.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.gdvItems.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.gdvItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gdvItems.Location = new System.Drawing.Point(15, 257);
            this.gdvItems.Name = "gdvItems";
            this.gdvItems.ReadOnly = true;
            this.gdvItems.RowHeadersVisible = false;
            this.gdvItems.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gdvItems.Size = new System.Drawing.Size(865, 131);
            this.gdvItems.TabIndex = 1;
            this.gdvItems.SelectionChanged += new System.EventHandler(this.gdvItems_SelectionChanged);
            // 
            // gdvOrders
            // 
            this.gdvOrders.AllowUserToAddRows = false;
            this.gdvOrders.AllowUserToDeleteRows = false;
            this.gdvOrders.BackgroundColor = System.Drawing.Color.LightSlateGray;
            this.gdvOrders.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gdvOrders.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.gdvOrders.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.gdvOrders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gdvOrders.Location = new System.Drawing.Point(15, 39);
            this.gdvOrders.MultiSelect = false;
            this.gdvOrders.Name = "gdvOrders";
            this.gdvOrders.ReadOnly = true;
            this.gdvOrders.RowHeadersVisible = false;
            this.gdvOrders.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gdvOrders.Size = new System.Drawing.Size(475, 150);
            this.gdvOrders.TabIndex = 0;
            this.gdvOrders.VirtualMode = true;
            this.gdvOrders.SelectionChanged += new System.EventHandler(this.DisplayOrderItems);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdoOrderId);
            this.groupBox1.Controls.Add(this.rdoEmployee);
            this.groupBox1.Controls.Add(this.rdoDepartment);
            this.groupBox1.ForeColor = System.Drawing.Color.Black;
            this.groupBox1.Location = new System.Drawing.Point(25, 37);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(151, 93);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search Type";
            // 
            // rdoOrderId
            // 
            this.rdoOrderId.AutoSize = true;
            this.rdoOrderId.ForeColor = System.Drawing.Color.Black;
            this.rdoOrderId.Location = new System.Drawing.Point(16, 66);
            this.rdoOrderId.Name = "rdoOrderId";
            this.rdoOrderId.Size = new System.Drawing.Size(80, 17);
            this.rdoOrderId.TabIndex = 2;
            this.rdoOrderId.Text = "PO Number";
            this.rdoOrderId.UseVisualStyleBackColor = true;
            this.rdoOrderId.CheckedChanged += new System.EventHandler(this.SetSearchType);
            // 
            // rdoEmployee
            // 
            this.rdoEmployee.AutoSize = true;
            this.rdoEmployee.ForeColor = System.Drawing.Color.Black;
            this.rdoEmployee.Location = new System.Drawing.Point(16, 43);
            this.rdoEmployee.Name = "rdoEmployee";
            this.rdoEmployee.Size = new System.Drawing.Size(102, 17);
            this.rdoEmployee.TabIndex = 1;
            this.rdoEmployee.Text = "Employee Name";
            this.rdoEmployee.UseVisualStyleBackColor = true;
            this.rdoEmployee.CheckedChanged += new System.EventHandler(this.SetSearchType);
            // 
            // rdoDepartment
            // 
            this.rdoDepartment.AutoSize = true;
            this.rdoDepartment.ForeColor = System.Drawing.Color.Black;
            this.rdoDepartment.Location = new System.Drawing.Point(16, 20);
            this.rdoDepartment.Name = "rdoDepartment";
            this.rdoDepartment.Size = new System.Drawing.Size(125, 17);
            this.rdoDepartment.TabIndex = 0;
            this.rdoDepartment.Text = "All Of My Department";
            this.rdoDepartment.UseVisualStyleBackColor = true;
            this.rdoDepartment.CheckedChanged += new System.EventHandler(this.SetSearchType);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(269, 25);
            this.lblTitle.TabIndex = 10;
            this.lblTitle.Text = "Process Purchase Order";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.Black;
            this.btnClose.Location = new System.Drawing.Point(970, 9);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(25, 25);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "X";
            this.btnClose.UseVisualStyleBackColor = false;
            // 
            // lblFilter
            // 
            this.lblFilter.AutoSize = true;
            this.lblFilter.ForeColor = System.Drawing.Color.Black;
            this.lblFilter.Location = new System.Drawing.Point(730, 70);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(32, 13);
            this.lblFilter.TabIndex = 12;
            this.lblFilter.Text = "Filter:";
            // 
            // cboFilter
            // 
            this.cboFilter.BackColor = System.Drawing.Color.SlateGray;
            this.cboFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboFilter.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboFilter.ForeColor = System.Drawing.Color.White;
            this.cboFilter.FormattingEnabled = true;
            this.cboFilter.Location = new System.Drawing.Point(768, 68);
            this.cboFilter.Name = "cboFilter";
            this.cboFilter.Size = new System.Drawing.Size(121, 21);
            this.cboFilter.TabIndex = 13;
            // 
            // lblFirstName
            // 
            this.lblFirstName.AutoSize = true;
            this.lblFirstName.ForeColor = System.Drawing.Color.Black;
            this.lblFirstName.Location = new System.Drawing.Point(326, 72);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(60, 13);
            this.lblFirstName.TabIndex = 14;
            this.lblFirstName.Text = "First Name:";
            // 
            // dtpFrom
            // 
            this.dtpFrom.CalendarMonthBackground = System.Drawing.Color.LightSlateGray;
            this.dtpFrom.Location = new System.Drawing.Point(557, 68);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(167, 20);
            this.dtpFrom.TabIndex = 16;
            // 
            // dtpTo
            // 
            this.dtpTo.CalendarMonthBackground = System.Drawing.Color.LightSlateGray;
            this.dtpTo.Location = new System.Drawing.Point(557, 94);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(167, 20);
            this.dtpTo.TabIndex = 17;
            // 
            // lblDateFrom
            // 
            this.lblDateFrom.AutoSize = true;
            this.lblDateFrom.ForeColor = System.Drawing.Color.Black;
            this.lblDateFrom.Location = new System.Drawing.Point(518, 72);
            this.lblDateFrom.Name = "lblDateFrom";
            this.lblDateFrom.Size = new System.Drawing.Size(33, 13);
            this.lblDateFrom.TabIndex = 18;
            this.lblDateFrom.Text = "From:";
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.ForeColor = System.Drawing.Color.Black;
            this.lblTo.Location = new System.Drawing.Point(528, 97);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(23, 13);
            this.lblTo.TabIndex = 19;
            this.lblTo.Text = "To:";
            // 
            // btnGo
            // 
            this.btnGo.BackColor = System.Drawing.Color.SlateGray;
            this.btnGo.FlatAppearance.BorderSize = 0;
            this.btnGo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGo.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGo.ForeColor = System.Drawing.Color.White;
            this.btnGo.Location = new System.Drawing.Point(910, 77);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(75, 25);
            this.btnGo.TabIndex = 21;
            this.btnGo.Text = "GO";
            this.btnGo.UseVisualStyleBackColor = false;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // chkDateRange
            // 
            this.chkDateRange.AutoSize = true;
            this.chkDateRange.Location = new System.Drawing.Point(772, 93);
            this.chkDateRange.Name = "chkDateRange";
            this.chkDateRange.Size = new System.Drawing.Size(117, 17);
            this.chkDateRange.TabIndex = 22;
            this.chkDateRange.Text = "Within Date Range";
            this.chkDateRange.UseVisualStyleBackColor = true;
            // 
            // txtFirstName
            // 
            this.txtFirstName.Location = new System.Drawing.Point(392, 68);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(121, 20);
            this.txtFirstName.TabIndex = 23;
            // 
            // txtLastName
            // 
            this.txtLastName.Location = new System.Drawing.Point(392, 93);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(121, 20);
            this.txtLastName.TabIndex = 25;
            // 
            // lblLastName
            // 
            this.lblLastName.AutoSize = true;
            this.lblLastName.ForeColor = System.Drawing.Color.Black;
            this.lblLastName.Location = new System.Drawing.Point(326, 96);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(61, 13);
            this.lblLastName.TabIndex = 24;
            this.lblLastName.Text = "Last Name:";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // txtOrderId
            // 
            this.txtOrderId.Location = new System.Drawing.Point(206, 93);
            this.txtOrderId.Name = "txtOrderId";
            this.txtOrderId.Size = new System.Drawing.Size(114, 20);
            this.txtOrderId.TabIndex = 27;
            // 
            // lblOrderId
            // 
            this.lblOrderId.AutoSize = true;
            this.lblOrderId.ForeColor = System.Drawing.Color.Black;
            this.lblOrderId.Location = new System.Drawing.Point(203, 72);
            this.lblOrderId.Name = "lblOrderId";
            this.lblOrderId.Size = new System.Drawing.Size(96, 13);
            this.lblOrderId.TabIndex = 26;
            this.lblOrderId.Text = "Purchase Order Id:";
            // 
            // gbxItemDetails
            // 
            this.gbxItemDetails.Location = new System.Drawing.Point(498, 32);
            this.gbxItemDetails.Name = "gbxItemDetails";
            this.gbxItemDetails.Size = new System.Drawing.Size(495, 159);
            this.gbxItemDetails.TabIndex = 68;
            this.gbxItemDetails.TabStop = false;
            this.gbxItemDetails.Text = "Item Details";
            // 
            // ProcessPO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(1007, 570);
            this.Controls.Add(this.txtOrderId);
            this.Controls.Add(this.lblOrderId);
            this.Controls.Add(this.txtLastName);
            this.Controls.Add(this.lblLastName);
            this.Controls.Add(this.txtFirstName);
            this.Controls.Add(this.chkDateRange);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTo);
            this.Controls.Add(this.lblDateFrom);
            this.Controls.Add(this.dtpTo);
            this.Controls.Add(this.dtpFrom);
            this.Controls.Add(this.lblFirstName);
            this.Controls.Add(this.cboFilter);
            this.Controls.Add(this.lblFilter);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.panel1);
            this.Name = "ProcessPO";
            this.Text = "Process Purchase Order";
            this.Load += new System.EventHandler(this.ProcessPO_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gdvItems)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdvOrders)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.ComboBox cboFilter;
        private System.Windows.Forms.Label lblFirstName;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.Label lblDateFrom;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdoEmployee;
        private System.Windows.Forms.RadioButton rdoDepartment;
        private System.Windows.Forms.DataGridView gdvOrders;
        private System.Windows.Forms.DataGridView gdvItems;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.TextBox txtGrandTotal;
        private System.Windows.Forms.Label lblGrandTotal;
        private System.Windows.Forms.TextBox txtTaxTotal;
        private System.Windows.Forms.Label lblTaxTotal;
        private System.Windows.Forms.TextBox txtSubTotal;
        private System.Windows.Forms.Label lblSubTotal;
        private System.Windows.Forms.Label lblOrderItems;
        private System.Windows.Forms.Label lblOrders;
        private System.Windows.Forms.CheckBox chkDateRange;
        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.Label lblLastName;
        private System.Windows.Forms.Label lblSource;
        private System.Windows.Forms.TextBox txtJustification;
        private System.Windows.Forms.TextBox txtSource;
        private System.Windows.Forms.Label lblJustification;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblQuantity;
        private System.Windows.Forms.Button btnApprove;
        private System.Windows.Forms.Label lblReason;
        private System.Windows.Forms.TextBox txtReason;
        private System.Windows.Forms.Button btnDeny;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RadioButton rdoOrderId;
        private System.Windows.Forms.TextBox txtOrderId;
        private System.Windows.Forms.Label lblOrderId;
        private System.Windows.Forms.TextBox txtQuantity;
        private System.Windows.Forms.TextBox txtPrice;
        private System.Windows.Forms.GroupBox gbxItemDetails;
    }
}