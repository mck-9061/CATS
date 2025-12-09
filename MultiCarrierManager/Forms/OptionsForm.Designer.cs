using System.ComponentModel;

namespace MultiCarrierManager
{
    partial class OptionsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxDarkMode = new System.Windows.Forms.CheckBox();
            this.checkBoxPreInteractionAlert = new System.Windows.Forms.CheckBox();
            this.labelAppearance = new System.Windows.Forms.Label();
            this.labelRefuelThreshold = new System.Windows.Forms.Label();
            this.trackBarRefuelThreshold = new System.Windows.Forms.TrackBar();
            this.labelRefuelThresholdValue = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRefuelThreshold)).BeginInit();
            this.SuspendLayout();
            // 
            // checkBox2
            // 
            this.checkBox2.Location = new System.Drawing.Point(12, 12);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(333, 24);
            this.checkBox2.TabIndex = 1;
            this.checkBox2.Text = "Automatically plot jumps";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.Location = new System.Drawing.Point(12, 42);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(333, 24);
            this.checkBox3.TabIndex = 2;
            this.checkBox3.Text = "Open CATS to traversal system instead of admin interface";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox4
            // 
            this.checkBox4.Location = new System.Drawing.Point(12, 72);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(333, 24);
            this.checkBox4.TabIndex = 3;
            this.checkBox4.Text = "Determine Tritium requirements when automatically finding route";
            this.checkBox4.UseVisualStyleBackColor = true;
            //
            // button1
            //
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button1.Location = new System.Drawing.Point(12, 300);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(344, 40);
            this.button1.TabIndex = 12;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            //
            // checkBox5
            //
            this.checkBox5.Location = new System.Drawing.Point(12, 102);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(333, 24);
            this.checkBox5.TabIndex = 5;
            this.checkBox5.Text = "Disable automatic refuel";
            this.checkBox5.UseVisualStyleBackColor = true;
            this.checkBox5.CheckedChanged += new System.EventHandler(this.checkBox5_CheckedChanged);
            //
            // labelRefuelThreshold
            //
            this.labelRefuelThreshold.Location = new System.Drawing.Point(12, 129);
            this.labelRefuelThreshold.Name = "labelRefuelThreshold";
            this.labelRefuelThreshold.Size = new System.Drawing.Size(200, 17);
            this.labelRefuelThreshold.TabIndex = 13;
            this.labelRefuelThreshold.Text = "Refuel when tritium below:";
            //
            // trackBarRefuelThreshold
            //
            this.trackBarRefuelThreshold.Location = new System.Drawing.Point(12, 146);
            this.trackBarRefuelThreshold.Maximum = 1000;
            this.trackBarRefuelThreshold.Minimum = 50;
            this.trackBarRefuelThreshold.Name = "trackBarRefuelThreshold";
            this.trackBarRefuelThreshold.Size = new System.Drawing.Size(280, 45);
            this.trackBarRefuelThreshold.TabIndex = 14;
            this.trackBarRefuelThreshold.TickFrequency = 100;
            this.trackBarRefuelThreshold.Value = 200;
            this.trackBarRefuelThreshold.Scroll += new System.EventHandler(this.trackBarRefuelThreshold_Scroll);
            //
            // labelRefuelThresholdValue
            //
            this.labelRefuelThresholdValue.Location = new System.Drawing.Point(298, 150);
            this.labelRefuelThresholdValue.Name = "labelRefuelThresholdValue";
            this.labelRefuelThresholdValue.Size = new System.Drawing.Size(50, 17);
            this.labelRefuelThresholdValue.TabIndex = 15;
            this.labelRefuelThresholdValue.Text = "200";
            //
            // checkBox1
            //
            this.checkBox1.Location = new System.Drawing.Point(12, 191);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(333, 24);
            this.checkBox1.TabIndex = 6;
            this.checkBox1.Text = "Power saving mode";
            this.checkBox1.UseVisualStyleBackColor = true;
            //
            // comboBox1
            //
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] { "Personal (First 8 items)", "Personal (After 8 items)", "Squadron" });
            this.comboBox1.Location = new System.Drawing.Point(12, 247);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(344, 21);
            this.comboBox1.TabIndex = 7;
            //
            // label1
            //
            this.label1.Location = new System.Drawing.Point(12, 227);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 17);
            this.label1.TabIndex = 8;
            this.label1.Text = "Refuelling mode";
            //
            // labelAppearance
            //
            this.labelAppearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAppearance.Location = new System.Drawing.Point(12, 280);
            this.labelAppearance.Name = "labelAppearance";
            this.labelAppearance.Size = new System.Drawing.Size(150, 17);
            this.labelAppearance.TabIndex = 9;
            this.labelAppearance.Text = "Appearance && Alerts";
            //
            // checkBoxDarkMode
            //
            this.checkBoxDarkMode.Location = new System.Drawing.Point(12, 300);
            this.checkBoxDarkMode.Name = "checkBoxDarkMode";
            this.checkBoxDarkMode.Size = new System.Drawing.Size(333, 24);
            this.checkBoxDarkMode.TabIndex = 10;
            this.checkBoxDarkMode.Text = "Dark mode (requires restart)";
            this.checkBoxDarkMode.UseVisualStyleBackColor = true;
            //
            // checkBoxPreInteractionAlert
            //
            this.checkBoxPreInteractionAlert.Location = new System.Drawing.Point(12, 330);
            this.checkBoxPreInteractionAlert.Name = "checkBoxPreInteractionAlert";
            this.checkBoxPreInteractionAlert.Size = new System.Drawing.Size(333, 24);
            this.checkBoxPreInteractionAlert.TabIndex = 11;
            this.checkBoxPreInteractionAlert.Text = "Alert before plotting/refueling";
            this.checkBoxPreInteractionAlert.UseVisualStyleBackColor = true;
            //
            // button1
            //
            this.button1.Location = new System.Drawing.Point(12, 365);
            //
            // OptionsForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 420);
            this.Controls.Add(this.labelRefuelThresholdValue);
            this.Controls.Add(this.trackBarRefuelThreshold);
            this.Controls.Add(this.labelRefuelThreshold);
            this.Controls.Add(this.checkBoxPreInteractionAlert);
            this.Controls.Add(this.checkBoxDarkMode);
            this.Controls.Add(this.labelAppearance);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.checkBox5);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkBox4);
            this.Controls.Add(this.checkBox3);
            this.Controls.Add(this.checkBox2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "OptionsForm";
            this.Text = "Options";
            this.Load += new System.EventHandler(this.OptionsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRefuelThreshold)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label label1;

        private System.Windows.Forms.ComboBox comboBox1;

        private System.Windows.Forms.CheckBox checkBox1;

        private System.Windows.Forms.CheckBox checkBox5;

        private System.Windows.Forms.Button button1;

        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.CheckBox checkBoxDarkMode;
        private System.Windows.Forms.CheckBox checkBoxPreInteractionAlert;
        private System.Windows.Forms.Label labelAppearance;
        private System.Windows.Forms.Label labelRefuelThreshold;
        private System.Windows.Forms.TrackBar trackBarRefuelThreshold;
        private System.Windows.Forms.Label labelRefuelThresholdValue;

        #endregion
    }
}
