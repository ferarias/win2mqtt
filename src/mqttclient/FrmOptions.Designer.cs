namespace Win2Mqtt.Client
{
    partial class FrmOptions
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
            CmdSave = new Button();
            CmdClose = new Button();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            groupBox2 = new GroupBox();
            button1 = new Button();
            label10 = new Label();
            textBox1 = new TextBox();
            label7 = new Label();
            txtMqttTimerInterval = new TextBox();
            label4 = new Label();
            txtmqtttopic = new TextBox();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            txtmqttpassword = new TextBox();
            txtmqttusername = new TextBox();
            txtmqttserver = new TextBox();
            tabPage2 = new TabPage();
            textBox4 = new TextBox();
            groupBox6 = new GroupBox();
            ChkComputerUsed = new CheckBox();
            chkVolumeSensor = new CheckBox();
            chkCpuSensor = new CheckBox();
            chkMemorySensor = new CheckBox();
            ChkDiskSensor = new CheckBox();
            ChkBatterySensor = new CheckBox();
            tabPage3 = new TabPage();
            groupBox3 = new GroupBox();
            ChkProcesses = new CheckBox();
            ChkMonitor = new CheckBox();
            chktoast = new CheckBox();
            chkReboot = new CheckBox();
            chkHibernate = new CheckBox();
            chkShutdown = new CheckBox();
            chkSuspend = new CheckBox();
            tabPage5 = new TabPage();
            tabPage7 = new TabPage();
            chkMinimizeToTray = new CheckBox();
            chkStartUp = new CheckBox();
            tabPage8 = new TabPage();
            CmdDiscovery = new Button();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            groupBox2.SuspendLayout();
            tabPage2.SuspendLayout();
            groupBox6.SuspendLayout();
            tabPage3.SuspendLayout();
            groupBox3.SuspendLayout();
            tabPage7.SuspendLayout();
            tabPage8.SuspendLayout();
            SuspendLayout();
            // 
            // CmdSave
            // 
            CmdSave.Location = new Point(722, 751);
            CmdSave.Margin = new Padding(4, 3, 4, 3);
            CmdSave.Name = "CmdSave";
            CmdSave.Size = new Size(111, 27);
            CmdSave.TabIndex = 42;
            CmdSave.Text = "Save and close";
            CmdSave.UseVisualStyleBackColor = true;
            CmdSave.Click += CmdSave_Click;
            // 
            // CmdClose
            // 
            CmdClose.Location = new Point(604, 751);
            CmdClose.Margin = new Padding(4, 3, 4, 3);
            CmdClose.Name = "CmdClose";
            CmdClose.Size = new Size(111, 27);
            CmdClose.TabIndex = 43;
            CmdClose.Text = "Close";
            CmdClose.UseVisualStyleBackColor = true;
            CmdClose.Click += CmdClose_Click;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage5);
            tabControl1.Controls.Add(tabPage7);
            tabControl1.Controls.Add(tabPage8);
            tabControl1.Location = new Point(1, 14);
            tabControl1.Margin = new Padding(4, 3, 4, 3);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(819, 707);
            tabControl1.TabIndex = 53;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(groupBox2);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Margin = new Padding(4, 3, 4, 3);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(4, 3, 4, 3);
            tabPage1.Size = new Size(811, 679);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Connection";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(button1);
            groupBox2.Controls.Add(label10);
            groupBox2.Controls.Add(textBox1);
            groupBox2.Controls.Add(label7);
            groupBox2.Controls.Add(txtMqttTimerInterval);
            groupBox2.Controls.Add(label4);
            groupBox2.Controls.Add(txtmqtttopic);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(label1);
            groupBox2.Controls.Add(txtmqttpassword);
            groupBox2.Controls.Add(txtmqttusername);
            groupBox2.Controls.Add(txtmqttserver);
            groupBox2.Location = new Point(7, 7);
            groupBox2.Margin = new Padding(4, 3, 4, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(4, 3, 4, 3);
            groupBox2.Size = new Size(332, 245);
            groupBox2.TabIndex = 35;
            groupBox2.TabStop = false;
            groupBox2.Text = "MQTT client options";
            // 
            // button1
            // 
            button1.Location = new Point(113, 211);
            button1.Margin = new Padding(4, 3, 4, 3);
            button1.Name = "button1";
            button1.Size = new Size(130, 27);
            button1.TabIndex = 29;
            button1.Text = "test connection";
            button1.UseVisualStyleBackColor = true;
            button1.Click += Button1_Click_1;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(7, 121);
            label10.Margin = new Padding(4, 0, 4, 0);
            label10.Name = "label10";
            label10.Size = new Size(29, 15);
            label10.TabIndex = 28;
            label10.Text = "Port";
            label10.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(113, 118);
            textBox1.Margin = new Padding(4, 3, 4, 3);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(191, 23);
            textBox1.TabIndex = 4;
            textBox1.Text = "1883";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(7, 157);
            label7.Margin = new Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new Size(81, 15);
            label7.TabIndex = 26;
            label7.Text = "Timer inverval";
            label7.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtMqttTimerInterval
            // 
            txtMqttTimerInterval.Location = new Point(113, 153);
            txtMqttTimerInterval.Margin = new Padding(4, 3, 4, 3);
            txtMqttTimerInterval.Name = "txtMqttTimerInterval";
            txtMqttTimerInterval.Size = new Size(191, 23);
            txtMqttTimerInterval.TabIndex = 5;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(7, 187);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(35, 15);
            label4.TabIndex = 24;
            label4.Text = "Topic";
            // 
            // txtmqtttopic
            // 
            txtmqtttopic.Location = new Point(113, 183);
            txtmqtttopic.Margin = new Padding(4, 3, 4, 3);
            txtmqtttopic.Name = "txtmqtttopic";
            txtmqtttopic.Size = new Size(191, 23);
            txtmqtttopic.TabIndex = 6;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(7, 91);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(57, 15);
            label3.TabIndex = 21;
            label3.Text = "Password";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(7, 61);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(60, 15);
            label2.TabIndex = 20;
            label2.Text = "Username";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(8, 25);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(39, 15);
            label1.TabIndex = 19;
            label1.Text = "Server";
            // 
            // txtmqttpassword
            // 
            txtmqttpassword.Location = new Point(113, 88);
            txtmqttpassword.Margin = new Padding(4, 3, 4, 3);
            txtmqttpassword.Name = "txtmqttpassword";
            txtmqttpassword.PasswordChar = '*';
            txtmqttpassword.Size = new Size(191, 23);
            txtmqttpassword.TabIndex = 3;
            // 
            // txtmqttusername
            // 
            txtmqttusername.Location = new Point(113, 53);
            txtmqttusername.Margin = new Padding(4, 3, 4, 3);
            txtmqttusername.Name = "txtmqttusername";
            txtmqttusername.Size = new Size(192, 23);
            txtmqttusername.TabIndex = 2;
            // 
            // txtmqttserver
            // 
            txtmqttserver.Location = new Point(113, 22);
            txtmqttserver.Margin = new Padding(4, 3, 4, 3);
            txtmqttserver.Name = "txtmqttserver";
            txtmqttserver.Size = new Size(192, 23);
            txtmqttserver.TabIndex = 1;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(textBox4);
            tabPage2.Controls.Add(groupBox6);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Margin = new Padding(4, 3, 4, 3);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(4, 3, 4, 3);
            tabPage2.Size = new Size(811, 679);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Sensors";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // textBox4
            // 
            textBox4.Location = new Point(31, 350);
            textBox4.Margin = new Padding(4, 3, 4, 3);
            textBox4.Multiline = true;
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(444, 177);
            textBox4.TabIndex = 42;
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(ChkComputerUsed);
            groupBox6.Controls.Add(chkVolumeSensor);
            groupBox6.Controls.Add(chkCpuSensor);
            groupBox6.Controls.Add(chkMemorySensor);
            groupBox6.Controls.Add(ChkDiskSensor);
            groupBox6.Controls.Add(ChkBatterySensor);
            groupBox6.Location = new Point(7, 7);
            groupBox6.Margin = new Padding(4, 3, 4, 3);
            groupBox6.Name = "groupBox6";
            groupBox6.Padding = new Padding(4, 3, 4, 3);
            groupBox6.Size = new Size(713, 325);
            groupBox6.TabIndex = 41;
            groupBox6.TabStop = false;
            groupBox6.Text = "Sensors";
            // 
            // ChkComputerUsed
            // 
            ChkComputerUsed.AutoSize = true;
            ChkComputerUsed.Location = new Point(111, 81);
            ChkComputerUsed.Margin = new Padding(4, 3, 4, 3);
            ChkComputerUsed.Name = "ChkComputerUsed";
            ChkComputerUsed.Size = new Size(117, 19);
            ChkComputerUsed.TabIndex = 5;
            ChkComputerUsed.Text = "Is computer used";
            ChkComputerUsed.UseVisualStyleBackColor = true;
            // 
            // chkVolumeSensor
            // 
            chkVolumeSensor.AutoSize = true;
            chkVolumeSensor.Location = new Point(12, 53);
            chkVolumeSensor.Margin = new Padding(4, 3, 4, 3);
            chkVolumeSensor.Name = "chkVolumeSensor";
            chkVolumeSensor.Size = new Size(66, 19);
            chkVolumeSensor.TabIndex = 4;
            chkVolumeSensor.Text = "Volume";
            chkVolumeSensor.UseVisualStyleBackColor = true;
            // 
            // chkCpuSensor
            // 
            chkCpuSensor.AutoSize = true;
            chkCpuSensor.Location = new Point(111, 53);
            chkCpuSensor.Margin = new Padding(4, 3, 4, 3);
            chkCpuSensor.Name = "chkCpuSensor";
            chkCpuSensor.Size = new Size(48, 19);
            chkCpuSensor.TabIndex = 3;
            chkCpuSensor.Text = "Cpu";
            chkCpuSensor.UseVisualStyleBackColor = true;
            // 
            // chkMemorySensor
            // 
            chkMemorySensor.AutoSize = true;
            chkMemorySensor.Location = new Point(12, 80);
            chkMemorySensor.Margin = new Padding(4, 3, 4, 3);
            chkMemorySensor.Name = "chkMemorySensor";
            chkMemorySensor.Size = new Size(71, 19);
            chkMemorySensor.TabIndex = 2;
            chkMemorySensor.Text = "Memory";
            chkMemorySensor.UseVisualStyleBackColor = true;
            // 
            // ChkDiskSensor
            // 
            ChkDiskSensor.AutoSize = true;
            ChkDiskSensor.Location = new Point(111, 27);
            ChkDiskSensor.Margin = new Padding(4, 3, 4, 3);
            ChkDiskSensor.Name = "ChkDiskSensor";
            ChkDiskSensor.Size = new Size(53, 19);
            ChkDiskSensor.TabIndex = 1;
            ChkDiskSensor.Text = "Disks";
            ChkDiskSensor.UseVisualStyleBackColor = true;
            // 
            // ChkBatterySensor
            // 
            ChkBatterySensor.AutoSize = true;
            ChkBatterySensor.Location = new Point(12, 27);
            ChkBatterySensor.Margin = new Padding(4, 3, 4, 3);
            ChkBatterySensor.Name = "ChkBatterySensor";
            ChkBatterySensor.Size = new Size(59, 19);
            ChkBatterySensor.TabIndex = 0;
            ChkBatterySensor.Text = "Power";
            ChkBatterySensor.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(groupBox3);
            tabPage3.Location = new Point(4, 24);
            tabPage3.Margin = new Padding(4, 3, 4, 3);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(4, 3, 4, 3);
            tabPage3.Size = new Size(811, 679);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Presets";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(ChkProcesses);
            groupBox3.Controls.Add(ChkMonitor);
            groupBox3.Controls.Add(chktoast);
            groupBox3.Controls.Add(chkReboot);
            groupBox3.Controls.Add(chkHibernate);
            groupBox3.Controls.Add(chkShutdown);
            groupBox3.Controls.Add(chkSuspend);
            groupBox3.Location = new Point(4, 7);
            groupBox3.Margin = new Padding(4, 3, 4, 3);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new Padding(4, 3, 4, 3);
            groupBox3.Size = new Size(723, 610);
            groupBox3.TabIndex = 36;
            groupBox3.TabStop = false;
            groupBox3.Text = "Enable Presets";
            // 
            // ChkProcesses
            // 
            ChkProcesses.AutoSize = true;
            ChkProcesses.Location = new Point(156, 102);
            ChkProcesses.Margin = new Padding(4, 3, 4, 3);
            ChkProcesses.Name = "ChkProcesses";
            ChkProcesses.Size = new Size(77, 19);
            ChkProcesses.TabIndex = 33;
            ChkProcesses.Text = "Processes";
            ChkProcesses.UseVisualStyleBackColor = true;
            // 
            // ChkMonitor
            // 
            ChkMonitor.AutoSize = true;
            ChkMonitor.Location = new Point(234, 23);
            ChkMonitor.Margin = new Padding(4, 3, 4, 3);
            ChkMonitor.Name = "ChkMonitor";
            ChkMonitor.Size = new Size(69, 19);
            ChkMonitor.TabIndex = 32;
            ChkMonitor.Text = "Monitor";
            ChkMonitor.UseVisualStyleBackColor = true;
            // 
            // chktoast
            // 
            chktoast.AutoSize = true;
            chktoast.Location = new Point(156, 23);
            chktoast.Margin = new Padding(4, 3, 4, 3);
            chktoast.Name = "chktoast";
            chktoast.Size = new Size(53, 19);
            chktoast.TabIndex = 31;
            chktoast.Text = "Toast";
            chktoast.UseVisualStyleBackColor = true;
            // 
            // chkReboot
            // 
            chkReboot.AutoSize = true;
            chkReboot.Location = new Point(156, 50);
            chkReboot.Margin = new Padding(4, 3, 4, 3);
            chkReboot.Name = "chkReboot";
            chkReboot.Size = new Size(64, 19);
            chkReboot.TabIndex = 18;
            chkReboot.Text = "Reboot";
            chkReboot.UseVisualStyleBackColor = true;
            // 
            // chkHibernate
            // 
            chkHibernate.AutoSize = true;
            chkHibernate.Location = new Point(156, 77);
            chkHibernate.Margin = new Padding(4, 3, 4, 3);
            chkHibernate.Name = "chkHibernate";
            chkHibernate.Size = new Size(78, 19);
            chkHibernate.TabIndex = 16;
            chkHibernate.Text = "Hibernate";
            chkHibernate.UseVisualStyleBackColor = true;
            // 
            // chkShutdown
            // 
            chkShutdown.AutoSize = true;
            chkShutdown.Location = new Point(28, 102);
            chkShutdown.Margin = new Padding(4, 3, 4, 3);
            chkShutdown.Name = "chkShutdown";
            chkShutdown.Size = new Size(80, 19);
            chkShutdown.TabIndex = 15;
            chkShutdown.Text = "Shutdown";
            chkShutdown.UseVisualStyleBackColor = true;
            // 
            // chkSuspend
            // 
            chkSuspend.AutoSize = true;
            chkSuspend.Location = new Point(28, 76);
            chkSuspend.Margin = new Padding(4, 3, 4, 3);
            chkSuspend.Name = "chkSuspend";
            chkSuspend.Size = new Size(71, 19);
            chkSuspend.TabIndex = 14;
            chkSuspend.Text = "Suspend";
            chkSuspend.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            tabPage5.Location = new Point(4, 24);
            tabPage5.Margin = new Padding(4, 3, 4, 3);
            tabPage5.Name = "tabPage5";
            tabPage5.Padding = new Padding(4, 3, 4, 3);
            tabPage5.Size = new Size(811, 679);
            tabPage5.TabIndex = 4;
            tabPage5.Text = "Commands";
            tabPage5.UseVisualStyleBackColor = true;
            // 
            // tabPage7
            // 
            tabPage7.Controls.Add(chkMinimizeToTray);
            tabPage7.Controls.Add(chkStartUp);
            tabPage7.Location = new Point(4, 24);
            tabPage7.Margin = new Padding(4, 3, 4, 3);
            tabPage7.Name = "tabPage7";
            tabPage7.Size = new Size(811, 679);
            tabPage7.TabIndex = 6;
            tabPage7.Text = "Application";
            tabPage7.UseVisualStyleBackColor = true;
            // 
            // chkMinimizeToTray
            // 
            chkMinimizeToTray.AutoSize = true;
            chkMinimizeToTray.Location = new Point(8, 40);
            chkMinimizeToTray.Margin = new Padding(4, 3, 4, 3);
            chkMinimizeToTray.Name = "chkMinimizeToTray";
            chkMinimizeToTray.Size = new Size(112, 19);
            chkMinimizeToTray.TabIndex = 43;
            chkMinimizeToTray.Text = "Minimize to tray";
            chkMinimizeToTray.UseVisualStyleBackColor = true;
            // 
            // chkStartUp
            // 
            chkStartUp.AutoSize = true;
            chkStartUp.Location = new Point(8, 3);
            chkStartUp.Margin = new Padding(4, 3, 4, 3);
            chkStartUp.Name = "chkStartUp";
            chkStartUp.Size = new Size(86, 19);
            chkStartUp.TabIndex = 42;
            chkStartUp.Text = "Run at start";
            chkStartUp.UseVisualStyleBackColor = true;
            chkStartUp.CheckedChanged += ChkStartUp_CheckedChanged;
            // 
            // tabPage8
            // 
            tabPage8.Controls.Add(CmdDiscovery);
            tabPage8.Location = new Point(4, 24);
            tabPage8.Margin = new Padding(4, 3, 4, 3);
            tabPage8.Name = "tabPage8";
            tabPage8.Padding = new Padding(4, 3, 4, 3);
            tabPage8.Size = new Size(811, 679);
            tabPage8.TabIndex = 7;
            tabPage8.Text = "Discovery";
            tabPage8.UseVisualStyleBackColor = true;
            // 
            // CmdDiscovery
            // 
            CmdDiscovery.Enabled = false;
            CmdDiscovery.Location = new Point(47, 28);
            CmdDiscovery.Margin = new Padding(4, 3, 4, 3);
            CmdDiscovery.Name = "CmdDiscovery";
            CmdDiscovery.Size = new Size(181, 27);
            CmdDiscovery.TabIndex = 0;
            CmdDiscovery.Text = "Send discovery";
            CmdDiscovery.UseVisualStyleBackColor = true;
            // 
            // FrmOptions
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(845, 797);
            Controls.Add(tabControl1);
            Controls.Add(CmdClose);
            Controls.Add(CmdSave);
            Margin = new Padding(4, 3, 4, 3);
            Name = "FrmOptions";
            Text = "Options";
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            groupBox6.ResumeLayout(false);
            groupBox6.PerformLayout();
            tabPage3.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            tabPage7.ResumeLayout(false);
            tabPage7.PerformLayout();
            tabPage8.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Button CmdSave;
        private System.Windows.Forms.Button CmdClose;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtMqttTimerInterval;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtmqtttopic;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtmqttpassword;
        private System.Windows.Forms.TextBox txtmqttusername;
        private System.Windows.Forms.TextBox txtmqttserver;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.CheckBox ChkComputerUsed;
        private System.Windows.Forms.CheckBox chkVolumeSensor;
        private System.Windows.Forms.CheckBox chkCpuSensor;
        private System.Windows.Forms.CheckBox chkMemorySensor;
        private System.Windows.Forms.CheckBox ChkDiskSensor;
        private System.Windows.Forms.CheckBox ChkBatterySensor;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox ChkProcesses;
        private System.Windows.Forms.CheckBox ChkMonitor;
        private System.Windows.Forms.CheckBox chktoast;
        private System.Windows.Forms.CheckBox chkReboot;
        private System.Windows.Forms.CheckBox chkHibernate;
        private System.Windows.Forms.CheckBox chkShutdown;
        private System.Windows.Forms.CheckBox chkSuspend;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.CheckBox chkMinimizeToTray;
        private System.Windows.Forms.CheckBox chkStartUp;
        private System.Windows.Forms.TabPage tabPage8;
        private System.Windows.Forms.Button CmdDiscovery;
        private System.Windows.Forms.TextBox textBox4;
    }
}