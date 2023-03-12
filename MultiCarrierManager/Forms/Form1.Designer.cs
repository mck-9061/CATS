using System.Windows.Forms;

namespace MultiCarrierManager {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] { "Refuel", "Available" }, -1);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] { "Repair", "Available" }, -1);
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem(new string[] { "Rearm", "Available" }, -1);
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem(new string[] { "Shipyard", "Available" }, -1);
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem(new string[] { "Outfitting", "Available" }, -1);
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem(new string[] { "Black Market", "Available" }, -1);
            System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem(new string[] { "Voucher Redemption", "Available" }, -1);
            System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem(new string[] { "Universal Cartographics", "Available" }, -1);
            System.Windows.Forms.ListViewItem listViewItem9 = new System.Windows.Forms.ListViewItem(new string[] { "Bartender", "Available" }, -1);
            System.Windows.Forms.ListViewItem listViewItem10 = new System.Windows.Forms.ListViewItem(new string[] { "Vista Genomics", "Available" }, -1);
            System.Windows.Forms.ListViewItem listViewItem11 = new System.Windows.Forms.ListViewItem(new string[] { "Pioneer Supplies", "Available" }, -1);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.managerTab = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader9 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader12 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader11 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader10 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader13 = new System.Windows.Forms.ColumnHeader();
            this.catsButton = new System.Windows.Forms.Button();
            this.statusLabel = new System.Windows.Forms.Label();
            this.refreshButton = new System.Windows.Forms.Button();
            this.AddCarrierButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.carrierTemplate = new System.Windows.Forms.TabPage();
            this.buyList1 = new System.Windows.Forms.ListView();
            this.buyCommHeader = new System.Windows.Forms.ColumnHeader();
            this.buyAmountHeader = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.buyPriceHeader = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.buyLabel1 = new System.Windows.Forms.Label();
            this.sellList1 = new System.Windows.Forms.ListView();
            this.sellCommHeader = new System.Windows.Forms.ColumnHeader();
            this.sellAmountHeader = new System.Windows.Forms.ColumnHeader();
            this.sellPriceHeader = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.sellLabel1 = new System.Windows.Forms.Label();
            this.commodityLabel1 = new System.Windows.Forms.Label();
            this.commodityList1 = new System.Windows.Forms.ListView();
            this.commodityHeader = new System.Windows.Forms.ColumnHeader();
            this.amountHeader = new System.Windows.Forms.ColumnHeader();
            this.averageHeader = new System.Windows.Forms.ColumnHeader();
            this.servicesList1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.servicesLabel1 = new System.Windows.Forms.Label();
            this.spaceLabel1 = new System.Windows.Forms.Label();
            this.accessLabel1 = new System.Windows.Forms.Label();
            this.fuelLabel1 = new System.Windows.Forms.Label();
            this.creditsLabel1 = new System.Windows.Forms.Label();
            this.systemLabel1 = new System.Windows.Forms.Label();
            this.callsignLabel1 = new System.Windows.Forms.Label();
            this.nameLabel1 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.managerTab.SuspendLayout();
            this.carrierTemplate.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.managerTab);
            this.tabControl1.Controls.Add(this.carrierTemplate);
            this.tabControl1.Location = new System.Drawing.Point(0, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(991, 563);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl1_Selected);
            // 
            // managerTab
            // 
            this.managerTab.Controls.Add(this.label2);
            this.managerTab.Controls.Add(this.listView1);
            this.managerTab.Controls.Add(this.catsButton);
            this.managerTab.Controls.Add(this.statusLabel);
            this.managerTab.Controls.Add(this.refreshButton);
            this.managerTab.Controls.Add(this.AddCarrierButton);
            this.managerTab.Controls.Add(this.label1);
            this.managerTab.Location = new System.Drawing.Point(4, 22);
            this.managerTab.Name = "managerTab";
            this.managerTab.Size = new System.Drawing.Size(983, 537);
            this.managerTab.TabIndex = 0;
            this.managerTab.Text = "Manager";
            this.managerTab.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 461);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(966, 23);
            this.label2.TabIndex = 6;
            this.label2.Text = "label2";
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { this.columnHeader7, this.columnHeader8, this.columnHeader9, this.columnHeader12, this.columnHeader11, this.columnHeader10, this.columnHeader13 });
            this.listView1.Location = new System.Drawing.Point(8, 92);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(966, 366);
            this.listView1.TabIndex = 5;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Commander";
            this.columnHeader7.Width = 104;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Liquid Credits";
            this.columnHeader8.Width = 107;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "Credits in Carrier";
            this.columnHeader9.Width = 116;
            // 
            // columnHeader12
            // 
            this.columnHeader12.Text = "Ships";
            this.columnHeader12.Width = 124;
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "Carrier cargo (spent)";
            this.columnHeader11.Width = 134;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "Credits spent on carrier + services";
            this.columnHeader10.Width = 196;
            // 
            // columnHeader13
            // 
            this.columnHeader13.Text = "Total";
            this.columnHeader13.Width = 179;
            // 
            // catsButton
            // 
            this.catsButton.Location = new System.Drawing.Point(8, 487);
            this.catsButton.Name = "catsButton";
            this.catsButton.Size = new System.Drawing.Size(154, 41);
            this.catsButton.TabIndex = 4;
            this.catsButton.Text = "Open Traversal System";
            this.catsButton.UseVisualStyleBackColor = true;
            this.catsButton.Click += new System.EventHandler(this.catsButton_Click);
            // 
            // statusLabel
            // 
            this.statusLabel.Location = new System.Drawing.Point(8, 66);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(966, 23);
            this.statusLabel.TabIndex = 3;
            // 
            // refreshButton
            // 
            this.refreshButton.Location = new System.Drawing.Point(168, 22);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(154, 41);
            this.refreshButton.TabIndex = 2;
            this.refreshButton.Text = "Pull Stats";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // AddCarrierButton
            // 
            this.AddCarrierButton.Location = new System.Drawing.Point(8, 22);
            this.AddCarrierButton.Name = "AddCarrierButton";
            this.AddCarrierButton.Size = new System.Drawing.Size(154, 41);
            this.AddCarrierButton.TabIndex = 1;
            this.AddCarrierButton.Text = "Add Carrier";
            this.AddCarrierButton.UseVisualStyleBackColor = true;
            this.AddCarrierButton.Click += new System.EventHandler(this.AddCarrierButton_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(966, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "No carriers added.";
            // 
            // carrierTemplate
            // 
            this.carrierTemplate.Controls.Add(this.buyList1);
            this.carrierTemplate.Controls.Add(this.buyLabel1);
            this.carrierTemplate.Controls.Add(this.sellList1);
            this.carrierTemplate.Controls.Add(this.sellLabel1);
            this.carrierTemplate.Controls.Add(this.commodityLabel1);
            this.carrierTemplate.Controls.Add(this.commodityList1);
            this.carrierTemplate.Controls.Add(this.servicesList1);
            this.carrierTemplate.Controls.Add(this.servicesLabel1);
            this.carrierTemplate.Controls.Add(this.spaceLabel1);
            this.carrierTemplate.Controls.Add(this.accessLabel1);
            this.carrierTemplate.Controls.Add(this.fuelLabel1);
            this.carrierTemplate.Controls.Add(this.creditsLabel1);
            this.carrierTemplate.Controls.Add(this.systemLabel1);
            this.carrierTemplate.Controls.Add(this.callsignLabel1);
            this.carrierTemplate.Controls.Add(this.nameLabel1);
            this.carrierTemplate.Location = new System.Drawing.Point(4, 22);
            this.carrierTemplate.Name = "carrierTemplate";
            this.carrierTemplate.Size = new System.Drawing.Size(983, 537);
            this.carrierTemplate.TabIndex = 1;
            this.carrierTemplate.Text = "Template";
            this.carrierTemplate.UseVisualStyleBackColor = true;
            // 
            // buyList1
            // 
            this.buyList1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { this.buyCommHeader, this.buyAmountHeader, this.columnHeader5, this.buyPriceHeader, this.columnHeader6, this.columnHeader4 });
            this.buyList1.Location = new System.Drawing.Point(440, 414);
            this.buyList1.Name = "buyList1";
            this.buyList1.Size = new System.Drawing.Size(534, 118);
            this.buyList1.TabIndex = 14;
            this.buyList1.UseCompatibleStateImageBehavior = false;
            this.buyList1.View = System.Windows.Forms.View.Details;
            // 
            // buyCommHeader
            // 
            this.buyCommHeader.Text = "Commodity";
            this.buyCommHeader.Width = 79;
            // 
            // buyAmountHeader
            // 
            this.buyAmountHeader.Text = "Total Amount";
            this.buyAmountHeader.Width = 75;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Outstanding Amount";
            this.columnHeader5.Width = 108;
            // 
            // buyPriceHeader
            // 
            this.buyPriceHeader.Text = "Price/commodity";
            this.buyPriceHeader.Width = 94;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Credits Allocated";
            this.columnHeader6.Width = 92;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Black Market?";
            this.columnHeader4.Width = 82;
            // 
            // buyLabel1
            // 
            this.buyLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buyLabel1.Location = new System.Drawing.Point(440, 381);
            this.buyLabel1.Name = "buyLabel1";
            this.buyLabel1.Size = new System.Drawing.Size(290, 30);
            this.buyLabel1.TabIndex = 13;
            this.buyLabel1.Text = "Buy orders:";
            // 
            // sellList1
            // 
            this.sellList1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { this.sellCommHeader, this.sellAmountHeader, this.sellPriceHeader, this.columnHeader3 });
            this.sellList1.Location = new System.Drawing.Point(440, 261);
            this.sellList1.Name = "sellList1";
            this.sellList1.Size = new System.Drawing.Size(534, 117);
            this.sellList1.TabIndex = 12;
            this.sellList1.UseCompatibleStateImageBehavior = false;
            this.sellList1.View = System.Windows.Forms.View.Details;
            // 
            // sellCommHeader
            // 
            this.sellCommHeader.Text = "Commodity";
            this.sellCommHeader.Width = 147;
            // 
            // sellAmountHeader
            // 
            this.sellAmountHeader.Text = "Stock";
            this.sellAmountHeader.Width = 83;
            // 
            // sellPriceHeader
            // 
            this.sellPriceHeader.Text = "Price/commodity";
            this.sellPriceHeader.Width = 110;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Black Market?";
            this.columnHeader3.Width = 120;
            // 
            // sellLabel1
            // 
            this.sellLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sellLabel1.Location = new System.Drawing.Point(440, 228);
            this.sellLabel1.Name = "sellLabel1";
            this.sellLabel1.Size = new System.Drawing.Size(290, 30);
            this.sellLabel1.TabIndex = 11;
            this.sellLabel1.Text = "Sell offers:";
            // 
            // commodityLabel1
            // 
            this.commodityLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.commodityLabel1.Location = new System.Drawing.Point(440, 0);
            this.commodityLabel1.Name = "commodityLabel1";
            this.commodityLabel1.Size = new System.Drawing.Size(290, 30);
            this.commodityLabel1.TabIndex = 10;
            this.commodityLabel1.Text = "Commodities:";
            // 
            // commodityList1
            // 
            this.commodityList1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { this.commodityHeader, this.amountHeader, this.averageHeader });
            this.commodityList1.Location = new System.Drawing.Point(440, 30);
            this.commodityList1.Name = "commodityList1";
            this.commodityList1.Size = new System.Drawing.Size(534, 195);
            this.commodityList1.TabIndex = 9;
            this.commodityList1.UseCompatibleStateImageBehavior = false;
            this.commodityList1.View = System.Windows.Forms.View.Details;
            // 
            // commodityHeader
            // 
            this.commodityHeader.Text = "Commodity";
            this.commodityHeader.Width = 158;
            // 
            // amountHeader
            // 
            this.amountHeader.Text = "Amount";
            this.amountHeader.Width = 76;
            // 
            // averageHeader
            // 
            this.averageHeader.Text = "Credits you paid";
            this.averageHeader.Width = 105;
            // 
            // servicesList1
            // 
            this.servicesList1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { this.columnHeader1, this.columnHeader2 });
            this.servicesList1.Items.AddRange(new System.Windows.Forms.ListViewItem[] { listViewItem1, listViewItem2, listViewItem3, listViewItem4, listViewItem5, listViewItem6, listViewItem7, listViewItem8, listViewItem9, listViewItem10, listViewItem11 });
            this.servicesList1.Location = new System.Drawing.Point(8, 243);
            this.servicesList1.Name = "servicesList1";
            this.servicesList1.ShowGroups = false;
            this.servicesList1.Size = new System.Drawing.Size(383, 289);
            this.servicesList1.TabIndex = 8;
            this.servicesList1.UseCompatibleStateImageBehavior = false;
            this.servicesList1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 168;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Availability";
            this.columnHeader2.Width = 192;
            // 
            // servicesLabel1
            // 
            this.servicesLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.servicesLabel1.Location = new System.Drawing.Point(0, 210);
            this.servicesLabel1.Name = "servicesLabel1";
            this.servicesLabel1.Size = new System.Drawing.Size(290, 30);
            this.servicesLabel1.TabIndex = 7;
            this.servicesLabel1.Text = "Services:";
            // 
            // spaceLabel1
            // 
            this.spaceLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.spaceLabel1.Location = new System.Drawing.Point(0, 180);
            this.spaceLabel1.Name = "spaceLabel1";
            this.spaceLabel1.Size = new System.Drawing.Size(290, 30);
            this.spaceLabel1.TabIndex = 6;
            this.spaceLabel1.Text = "Free space:";
            // 
            // accessLabel1
            // 
            this.accessLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.accessLabel1.Location = new System.Drawing.Point(0, 150);
            this.accessLabel1.Name = "accessLabel1";
            this.accessLabel1.Size = new System.Drawing.Size(290, 30);
            this.accessLabel1.TabIndex = 5;
            this.accessLabel1.Text = "Docking access:";
            // 
            // fuelLabel1
            // 
            this.fuelLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fuelLabel1.Location = new System.Drawing.Point(0, 120);
            this.fuelLabel1.Name = "fuelLabel1";
            this.fuelLabel1.Size = new System.Drawing.Size(290, 30);
            this.fuelLabel1.TabIndex = 4;
            this.fuelLabel1.Text = "Fuel:";
            // 
            // creditsLabel1
            // 
            this.creditsLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.creditsLabel1.Location = new System.Drawing.Point(0, 90);
            this.creditsLabel1.Name = "creditsLabel1";
            this.creditsLabel1.Size = new System.Drawing.Size(290, 30);
            this.creditsLabel1.TabIndex = 3;
            this.creditsLabel1.Text = "Credits:";
            // 
            // systemLabel1
            // 
            this.systemLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.systemLabel1.Location = new System.Drawing.Point(0, 60);
            this.systemLabel1.Name = "systemLabel1";
            this.systemLabel1.Size = new System.Drawing.Size(290, 30);
            this.systemLabel1.TabIndex = 2;
            this.systemLabel1.Text = "Current system:";
            // 
            // callsignLabel1
            // 
            this.callsignLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.callsignLabel1.Location = new System.Drawing.Point(0, 30);
            this.callsignLabel1.Name = "callsignLabel1";
            this.callsignLabel1.Size = new System.Drawing.Size(290, 30);
            this.callsignLabel1.TabIndex = 1;
            this.callsignLabel1.Text = "Callsign:";
            this.callsignLabel1.Click += new System.EventHandler(this.label2_Click);
            // 
            // nameLabel1
            // 
            this.nameLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameLabel1.Location = new System.Drawing.Point(0, 0);
            this.nameLabel1.Name = "nameLabel1";
            this.nameLabel1.Size = new System.Drawing.Size(290, 30);
            this.nameLabel1.TabIndex = 0;
            this.nameLabel1.Text = "Name:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(990, 564);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "ED Multi Carrier Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.managerTab.ResumeLayout(false);
            this.carrierTemplate.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label label2;

        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.ColumnHeader columnHeader12;
        private System.Windows.Forms.ColumnHeader columnHeader13;

        private System.Windows.Forms.ListView listView1;

        private System.Windows.Forms.Button catsButton;

        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;

        private System.Windows.Forms.ColumnHeader columnHeader4;

        private System.Windows.Forms.ColumnHeader columnHeader3;

        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.Label statusLabel;

        private System.Windows.Forms.ColumnHeader buyCommHeader;
        private System.Windows.Forms.ColumnHeader buyAmountHeader;
        private System.Windows.Forms.ColumnHeader buyPriceHeader;

        private System.Windows.Forms.ColumnHeader sellCommHeader;
        private System.Windows.Forms.ColumnHeader sellAmountHeader;
        private System.Windows.Forms.ColumnHeader sellPriceHeader;

        private System.Windows.Forms.ColumnHeader commodityHeader;
        private System.Windows.Forms.ColumnHeader amountHeader;
        private System.Windows.Forms.ColumnHeader averageHeader;

        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;

        private System.Windows.Forms.Label buyLabel1;
        private System.Windows.Forms.ListView buyList1;

        private System.Windows.Forms.Label commodityLabel1;
        private System.Windows.Forms.Label sellLabel1;
        private System.Windows.Forms.ListView servicesList1;

        private System.Windows.Forms.ListView commodityList1;

        private System.Windows.Forms.Label systemLabel1;
        private System.Windows.Forms.Label creditsLabel1;
        private System.Windows.Forms.Label fuelLabel1;
        private System.Windows.Forms.Label accessLabel1;
        private System.Windows.Forms.Label spaceLabel1;
        private System.Windows.Forms.Label servicesLabel1;
        private System.Windows.Forms.ListView sellList1;

        private System.Windows.Forms.Label nameLabel1;
        private System.Windows.Forms.Label callsignLabel1;

        private System.Windows.Forms.TabPage managerTab;

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button AddCarrierButton;

        private System.Windows.Forms.TabPage carrierTemplate;

        private System.Windows.Forms.TabControl tabControl1;

        #endregion
    }
}