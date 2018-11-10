namespace DataEmulator
{
  partial class CTS_Emulator
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
      this.WagonTransfers = new System.Windows.Forms.CheckBox();
      this.label1 = new System.Windows.Forms.Label();
      this.wqntfrom = new System.Windows.Forms.NumericUpDown();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.wqntto = new System.Windows.Forms.NumericUpDown();
      this.label4 = new System.Windows.Forms.Label();
      this.wbruttoto = new System.Windows.Forms.NumericUpDown();
      this.label5 = new System.Windows.Forms.Label();
      this.wbruttofrom = new System.Windows.Forms.NumericUpDown();
      this.label6 = new System.Windows.Forms.Label();
      this.label7 = new System.Windows.Forms.Label();
      this.wtareto = new System.Windows.Forms.NumericUpDown();
      this.label8 = new System.Windows.Forms.Label();
      this.wtarefrom = new System.Windows.Forms.NumericUpDown();
      this.label9 = new System.Windows.Forms.Label();
      this.VehiTransfers = new System.Windows.Forms.CheckBox();
      this.SkipTransfers = new System.Windows.Forms.CheckBox();
      this.label13 = new System.Windows.Forms.Label();
      this.bqntto = new System.Windows.Forms.NumericUpDown();
      this.label14 = new System.Windows.Forms.Label();
      this.bqntfrom = new System.Windows.Forms.NumericUpDown();
      this.label15 = new System.Windows.Forms.Label();
      this.BeltTransfers = new System.Windows.Forms.CheckBox();
      this.label19 = new System.Windows.Forms.Label();
      this.tqntto = new System.Windows.Forms.NumericUpDown();
      this.label20 = new System.Windows.Forms.Label();
      this.tqntfrom = new System.Windows.Forms.NumericUpDown();
      this.label21 = new System.Windows.Forms.Label();
      this.button1 = new System.Windows.Forms.Button();
      this.label22 = new System.Windows.Forms.Label();
      this.timer1 = new System.Windows.Forms.Timer(this.components);
      this.label16 = new System.Windows.Forms.Label();
      this.vtareto = new System.Windows.Forms.NumericUpDown();
      this.label17 = new System.Windows.Forms.Label();
      this.vtarefrom = new System.Windows.Forms.NumericUpDown();
      this.label18 = new System.Windows.Forms.Label();
      this.label23 = new System.Windows.Forms.Label();
      this.vbruttoto = new System.Windows.Forms.NumericUpDown();
      this.label24 = new System.Windows.Forms.Label();
      this.vbruttofrom = new System.Windows.Forms.NumericUpDown();
      this.label25 = new System.Windows.Forms.Label();
      this.label26 = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.wqntfrom)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.wqntto)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.wbruttoto)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.wbruttofrom)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.wtareto)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.wtarefrom)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.bqntto)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.bqntfrom)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.tqntto)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.tqntfrom)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.vtareto)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.vtarefrom)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.vbruttoto)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.vbruttofrom)).BeginInit();
      this.SuspendLayout();
      // 
      // WagonTransfers
      // 
      this.WagonTransfers.AutoSize = true;
      this.WagonTransfers.Location = new System.Drawing.Point(13, 20);
      this.WagonTransfers.Name = "WagonTransfers";
      this.WagonTransfers.Size = new System.Drawing.Size(105, 17);
      this.WagonTransfers.TabIndex = 0;
      this.WagonTransfers.Text = "WagonTransfers";
      this.WagonTransfers.UseVisualStyleBackColor = true;
      this.WagonTransfers.CheckedChanged += new System.EventHandler(this.WagonTransfers_CheckedChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(13, 48);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(110, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Количество вагонов";
      // 
      // wqntfrom
      // 
      this.wqntfrom.Enabled = false;
      this.wqntfrom.Location = new System.Drawing.Point(36, 74);
      this.wqntfrom.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.wqntfrom.Name = "wqntfrom";
      this.wqntfrom.Size = new System.Drawing.Size(43, 20);
      this.wqntfrom.TabIndex = 2;
      this.wqntfrom.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(13, 79);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(18, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "от";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(91, 79);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(19, 13);
      this.label3.TabIndex = 5;
      this.label3.Text = "до";
      // 
      // wqntto
      // 
      this.wqntto.Enabled = false;
      this.wqntto.Location = new System.Drawing.Point(114, 74);
      this.wqntto.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.wqntto.Name = "wqntto";
      this.wqntto.Size = new System.Drawing.Size(43, 20);
      this.wqntto.TabIndex = 4;
      this.wqntto.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(91, 138);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(19, 13);
      this.label4.TabIndex = 10;
      this.label4.Text = "до";
      // 
      // wbruttoto
      // 
      this.wbruttoto.Enabled = false;
      this.wbruttoto.Location = new System.Drawing.Point(114, 133);
      this.wbruttoto.Maximum = new decimal(new int[] {
            110,
            0,
            0,
            0});
      this.wbruttoto.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.wbruttoto.Name = "wbruttoto";
      this.wbruttoto.Size = new System.Drawing.Size(43, 20);
      this.wbruttoto.TabIndex = 9;
      this.wbruttoto.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(13, 138);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(18, 13);
      this.label5.TabIndex = 8;
      this.label5.Text = "от";
      // 
      // wbruttofrom
      // 
      this.wbruttofrom.Enabled = false;
      this.wbruttofrom.Location = new System.Drawing.Point(36, 133);
      this.wbruttofrom.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.wbruttofrom.Name = "wbruttofrom";
      this.wbruttofrom.Size = new System.Drawing.Size(43, 20);
      this.wbruttofrom.TabIndex = 7;
      this.wbruttofrom.Value = new decimal(new int[] {
            70,
            0,
            0,
            0});
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(13, 107);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(70, 13);
      this.label6.TabIndex = 6;
      this.label6.Text = "Брутто, тонн";
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(91, 199);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(19, 13);
      this.label7.TabIndex = 15;
      this.label7.Text = "до";
      // 
      // wtareto
      // 
      this.wtareto.Enabled = false;
      this.wtareto.Location = new System.Drawing.Point(114, 194);
      this.wtareto.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.wtareto.Name = "wtareto";
      this.wtareto.Size = new System.Drawing.Size(43, 20);
      this.wtareto.TabIndex = 14;
      this.wtareto.Value = new decimal(new int[] {
            45,
            0,
            0,
            0});
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(13, 199);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(18, 13);
      this.label8.TabIndex = 13;
      this.label8.Text = "от";
      // 
      // wtarefrom
      // 
      this.wtarefrom.Enabled = false;
      this.wtarefrom.Location = new System.Drawing.Point(36, 194);
      this.wtarefrom.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.wtarefrom.Name = "wtarefrom";
      this.wtarefrom.Size = new System.Drawing.Size(43, 20);
      this.wtarefrom.TabIndex = 12;
      this.wtarefrom.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.Location = new System.Drawing.Point(13, 168);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(61, 13);
      this.label9.TabIndex = 11;
      this.label9.Text = "Тара, тонн";
      // 
      // VehiTransfers
      // 
      this.VehiTransfers.AutoSize = true;
      this.VehiTransfers.Location = new System.Drawing.Point(222, 20);
      this.VehiTransfers.Name = "VehiTransfers";
      this.VehiTransfers.Size = new System.Drawing.Size(91, 17);
      this.VehiTransfers.TabIndex = 16;
      this.VehiTransfers.Text = "VehiTransfers";
      this.VehiTransfers.UseVisualStyleBackColor = true;
      this.VehiTransfers.CheckedChanged += new System.EventHandler(this.VehiTransfers_CheckedChanged);
      // 
      // SkipTransfers
      // 
      this.SkipTransfers.AutoSize = true;
      this.SkipTransfers.Location = new System.Drawing.Point(222, 194);
      this.SkipTransfers.Name = "SkipTransfers";
      this.SkipTransfers.Size = new System.Drawing.Size(91, 17);
      this.SkipTransfers.TabIndex = 22;
      this.SkipTransfers.Text = "SkipTransfers";
      this.SkipTransfers.UseVisualStyleBackColor = true;
      this.SkipTransfers.CheckedChanged += new System.EventHandler(this.SkipTransfers_CheckedChanged);
      // 
      // label13
      // 
      this.label13.AutoSize = true;
      this.label13.Location = new System.Drawing.Point(94, 303);
      this.label13.Name = "label13";
      this.label13.Size = new System.Drawing.Size(19, 13);
      this.label13.TabIndex = 33;
      this.label13.Text = "до";
      // 
      // bqntto
      // 
      this.bqntto.Enabled = false;
      this.bqntto.Location = new System.Drawing.Point(117, 298);
      this.bqntto.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.bqntto.Name = "bqntto";
      this.bqntto.Size = new System.Drawing.Size(43, 20);
      this.bqntto.TabIndex = 32;
      this.bqntto.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
      // 
      // label14
      // 
      this.label14.AutoSize = true;
      this.label14.Location = new System.Drawing.Point(16, 303);
      this.label14.Name = "label14";
      this.label14.Size = new System.Drawing.Size(18, 13);
      this.label14.TabIndex = 31;
      this.label14.Text = "от";
      // 
      // bqntfrom
      // 
      this.bqntfrom.Enabled = false;
      this.bqntfrom.Location = new System.Drawing.Point(39, 298);
      this.bqntfrom.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.bqntfrom.Name = "bqntfrom";
      this.bqntfrom.Size = new System.Drawing.Size(43, 20);
      this.bqntfrom.TabIndex = 30;
      this.bqntfrom.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
      // 
      // label15
      // 
      this.label15.AutoSize = true;
      this.label15.Location = new System.Drawing.Point(16, 272);
      this.label15.Name = "label15";
      this.label15.Size = new System.Drawing.Size(95, 13);
      this.label15.TabIndex = 29;
      this.label15.Text = "Количество, тонн";
      // 
      // BeltTransfers
      // 
      this.BeltTransfers.AutoSize = true;
      this.BeltTransfers.Location = new System.Drawing.Point(16, 244);
      this.BeltTransfers.Name = "BeltTransfers";
      this.BeltTransfers.Size = new System.Drawing.Size(88, 17);
      this.BeltTransfers.TabIndex = 28;
      this.BeltTransfers.Text = "BeltTransfers";
      this.BeltTransfers.UseVisualStyleBackColor = true;
      this.BeltTransfers.CheckedChanged += new System.EventHandler(this.BeltTransfers_CheckedChanged);
      // 
      // label19
      // 
      this.label19.AutoSize = true;
      this.label19.Location = new System.Drawing.Point(95, 379);
      this.label19.Name = "label19";
      this.label19.Size = new System.Drawing.Size(19, 13);
      this.label19.TabIndex = 38;
      this.label19.Text = "до";
      // 
      // tqntto
      // 
      this.tqntto.Location = new System.Drawing.Point(118, 374);
      this.tqntto.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
      this.tqntto.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.tqntto.Name = "tqntto";
      this.tqntto.Size = new System.Drawing.Size(43, 20);
      this.tqntto.TabIndex = 37;
      this.tqntto.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
      // 
      // label20
      // 
      this.label20.AutoSize = true;
      this.label20.Location = new System.Drawing.Point(17, 379);
      this.label20.Name = "label20";
      this.label20.Size = new System.Drawing.Size(18, 13);
      this.label20.TabIndex = 36;
      this.label20.Text = "от";
      // 
      // tqntfrom
      // 
      this.tqntfrom.Location = new System.Drawing.Point(40, 374);
      this.tqntfrom.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
      this.tqntfrom.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.tqntfrom.Name = "tqntfrom";
      this.tqntfrom.Size = new System.Drawing.Size(43, 20);
      this.tqntfrom.TabIndex = 35;
      this.tqntfrom.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
      // 
      // label21
      // 
      this.label21.AutoSize = true;
      this.label21.Location = new System.Drawing.Point(17, 348);
      this.label21.Name = "label21";
      this.label21.Size = new System.Drawing.Size(126, 13);
      this.label21.TabIndex = 34;
      this.label21.Text = "Периодичность, секунд";
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(223, 312);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(109, 47);
      this.button1.TabIndex = 39;
      this.button1.Text = "Start";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // label22
      // 
      this.label22.AutoSize = true;
      this.label22.Location = new System.Drawing.Point(224, 373);
      this.label22.Name = "label22";
      this.label22.Size = new System.Drawing.Size(125, 13);
      this.label22.TabIndex = 40;
      this.label22.Text = "Эмуляция остановлена";
      // 
      // label16
      // 
      this.label16.AutoSize = true;
      this.label16.Location = new System.Drawing.Point(302, 140);
      this.label16.Name = "label16";
      this.label16.Size = new System.Drawing.Size(19, 13);
      this.label16.TabIndex = 50;
      this.label16.Text = "до";
      // 
      // vtareto
      // 
      this.vtareto.Enabled = false;
      this.vtareto.Location = new System.Drawing.Point(325, 135);
      this.vtareto.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
      this.vtareto.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
      this.vtareto.Name = "vtareto";
      this.vtareto.Size = new System.Drawing.Size(43, 20);
      this.vtareto.TabIndex = 49;
      this.vtareto.Value = new decimal(new int[] {
            45,
            0,
            0,
            0});
      // 
      // label17
      // 
      this.label17.AutoSize = true;
      this.label17.Location = new System.Drawing.Point(224, 140);
      this.label17.Name = "label17";
      this.label17.Size = new System.Drawing.Size(18, 13);
      this.label17.TabIndex = 48;
      this.label17.Text = "от";
      // 
      // vtarefrom
      // 
      this.vtarefrom.Enabled = false;
      this.vtarefrom.Location = new System.Drawing.Point(247, 135);
      this.vtarefrom.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
      this.vtarefrom.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
      this.vtarefrom.Name = "vtarefrom";
      this.vtarefrom.Size = new System.Drawing.Size(43, 20);
      this.vtarefrom.TabIndex = 47;
      this.vtarefrom.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
      // 
      // label18
      // 
      this.label18.AutoSize = true;
      this.label18.Location = new System.Drawing.Point(224, 109);
      this.label18.Name = "label18";
      this.label18.Size = new System.Drawing.Size(61, 13);
      this.label18.TabIndex = 46;
      this.label18.Text = "Тара, тонн";
      // 
      // label23
      // 
      this.label23.AutoSize = true;
      this.label23.Location = new System.Drawing.Point(302, 79);
      this.label23.Name = "label23";
      this.label23.Size = new System.Drawing.Size(19, 13);
      this.label23.TabIndex = 45;
      this.label23.Text = "до";
      // 
      // vbruttoto
      // 
      this.vbruttoto.Enabled = false;
      this.vbruttoto.Location = new System.Drawing.Point(325, 74);
      this.vbruttoto.Maximum = new decimal(new int[] {
            110,
            0,
            0,
            0});
      this.vbruttoto.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
      this.vbruttoto.Name = "vbruttoto";
      this.vbruttoto.Size = new System.Drawing.Size(43, 20);
      this.vbruttoto.TabIndex = 44;
      this.vbruttoto.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
      // 
      // label24
      // 
      this.label24.AutoSize = true;
      this.label24.Location = new System.Drawing.Point(224, 79);
      this.label24.Name = "label24";
      this.label24.Size = new System.Drawing.Size(18, 13);
      this.label24.TabIndex = 43;
      this.label24.Text = "от";
      // 
      // vbruttofrom
      // 
      this.vbruttofrom.Enabled = false;
      this.vbruttofrom.Location = new System.Drawing.Point(247, 74);
      this.vbruttofrom.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
      this.vbruttofrom.Name = "vbruttofrom";
      this.vbruttofrom.Size = new System.Drawing.Size(43, 20);
      this.vbruttofrom.TabIndex = 42;
      this.vbruttofrom.Value = new decimal(new int[] {
            70,
            0,
            0,
            0});
      // 
      // label25
      // 
      this.label25.AutoSize = true;
      this.label25.Location = new System.Drawing.Point(224, 48);
      this.label25.Name = "label25";
      this.label25.Size = new System.Drawing.Size(70, 13);
      this.label25.TabIndex = 41;
      this.label25.Text = "Брутто, тонн";
      // 
      // label26
      // 
      this.label26.AutoSize = true;
      this.label26.Location = new System.Drawing.Point(224, 394);
      this.label26.Name = "label26";
      this.label26.Size = new System.Drawing.Size(117, 13);
      this.label26.TabIndex = 51;
      this.label26.Text = "Количество циклов: 0";
      // 
      // CTS_Emulator
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(482, 416);
      this.Controls.Add(this.label26);
      this.Controls.Add(this.label16);
      this.Controls.Add(this.vtareto);
      this.Controls.Add(this.label17);
      this.Controls.Add(this.vtarefrom);
      this.Controls.Add(this.label18);
      this.Controls.Add(this.label23);
      this.Controls.Add(this.vbruttoto);
      this.Controls.Add(this.label24);
      this.Controls.Add(this.vbruttofrom);
      this.Controls.Add(this.label25);
      this.Controls.Add(this.label22);
      this.Controls.Add(this.button1);
      this.Controls.Add(this.label19);
      this.Controls.Add(this.tqntto);
      this.Controls.Add(this.label20);
      this.Controls.Add(this.tqntfrom);
      this.Controls.Add(this.label21);
      this.Controls.Add(this.label13);
      this.Controls.Add(this.bqntto);
      this.Controls.Add(this.label14);
      this.Controls.Add(this.bqntfrom);
      this.Controls.Add(this.label15);
      this.Controls.Add(this.BeltTransfers);
      this.Controls.Add(this.SkipTransfers);
      this.Controls.Add(this.VehiTransfers);
      this.Controls.Add(this.label7);
      this.Controls.Add(this.wtareto);
      this.Controls.Add(this.label8);
      this.Controls.Add(this.wtarefrom);
      this.Controls.Add(this.label9);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.wbruttoto);
      this.Controls.Add(this.label5);
      this.Controls.Add(this.wbruttofrom);
      this.Controls.Add(this.label6);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.wqntto);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.wqntfrom);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.WagonTransfers);
      this.Name = "CTS_Emulator";
      this.Text = "CTS Emulator";
      ((System.ComponentModel.ISupportInitialize)(this.wqntfrom)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.wqntto)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.wbruttoto)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.wbruttofrom)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.wtareto)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.wtarefrom)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.bqntto)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.bqntfrom)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.tqntto)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.tqntfrom)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.vtareto)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.vtarefrom)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.vbruttoto)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.vbruttofrom)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.CheckBox WagonTransfers;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.NumericUpDown wqntfrom;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.NumericUpDown wqntto;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.NumericUpDown wbruttoto;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.NumericUpDown wbruttofrom;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.NumericUpDown wtareto;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.NumericUpDown wtarefrom;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.CheckBox VehiTransfers;
    private System.Windows.Forms.CheckBox SkipTransfers;
    private System.Windows.Forms.Label label13;
    private System.Windows.Forms.NumericUpDown bqntto;
    private System.Windows.Forms.Label label14;
    private System.Windows.Forms.NumericUpDown bqntfrom;
    private System.Windows.Forms.Label label15;
    private System.Windows.Forms.CheckBox BeltTransfers;
    private System.Windows.Forms.Label label19;
    private System.Windows.Forms.NumericUpDown tqntto;
    private System.Windows.Forms.Label label20;
    private System.Windows.Forms.NumericUpDown tqntfrom;
    private System.Windows.Forms.Label label21;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Label label22;
    private System.Windows.Forms.Timer timer1;
    private System.Windows.Forms.Label label16;
    private System.Windows.Forms.NumericUpDown vtareto;
    private System.Windows.Forms.Label label17;
    private System.Windows.Forms.NumericUpDown vtarefrom;
    private System.Windows.Forms.Label label18;
    private System.Windows.Forms.Label label23;
    private System.Windows.Forms.NumericUpDown vbruttoto;
    private System.Windows.Forms.Label label24;
    private System.Windows.Forms.NumericUpDown vbruttofrom;
    private System.Windows.Forms.Label label25;
    private System.Windows.Forms.Label label26;
  }
}

