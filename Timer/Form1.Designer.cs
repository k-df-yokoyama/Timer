namespace Timer
{
    partial class FormTimer
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea8 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend8 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series8 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint22 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(0D, 10D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint23 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(0D, 20D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint24 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(0D, 30D);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textSetTime = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.textRemainingTime = new System.Windows.Forms.TextBox();
            this.timerControl = new System.Windows.Forms.Timer(this.components);
            this.btnReset = new System.Windows.Forms.Button();
            this.textElaspedTimeMinutes = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textElaspedTimeSeconds = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnShowLog = new System.Windows.Forms.Button();
            this.btnAddNode = new System.Windows.Forms.Button();
            this.btnRemoveNode = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.textLastStopTime1 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textLastStopTime2 = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.btnShowGraph = new System.Windows.Forms.Button();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnAddPanel = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnSaveActivityLog = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.btnSaveReviewedActivityLog = new System.Windows.Forms.Button();
            this.btnShowGraphReviewedActivityLog = new System.Windows.Forms.Button();
            this.label23 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(773, 26);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "時間設定";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1083, 26);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "秒";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(773, 108);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 18);
            this.label3.TabIndex = 2;
            this.label3.Text = "残り時間";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1083, 108);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 18);
            this.label4.TabIndex = 3;
            this.label4.Text = "秒";
            // 
            // textSetTime
            // 
            this.textSetTime.Location = new System.Drawing.Point(906, 22);
            this.textSetTime.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.textSetTime.Name = "textSetTime";
            this.textSetTime.Size = new System.Drawing.Size(164, 25);
            this.textSetTime.TabIndex = 4;
            this.textSetTime.Text = "1500";
            this.textSetTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textSetTime.TextChanged += new System.EventHandler(this.textSetTime_TextChanged);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(773, 60);
            this.btnStart.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(142, 34);
            this.btnStart.TabIndex = 5;
            this.btnStart.Text = "スタート！";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // textRemainingTime
            // 
            this.textRemainingTime.Location = new System.Drawing.Point(906, 103);
            this.textRemainingTime.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.textRemainingTime.Name = "textRemainingTime";
            this.textRemainingTime.ReadOnly = true;
            this.textRemainingTime.Size = new System.Drawing.Size(164, 25);
            this.textRemainingTime.TabIndex = 6;
            this.textRemainingTime.Text = "1500";
            this.textRemainingTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // timerControl
            // 
            this.timerControl.Interval = 1000;
            this.timerControl.Tick += new System.EventHandler(this.timerControl_Tick);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(950, 60);
            this.btnReset.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(142, 34);
            this.btnReset.TabIndex = 7;
            this.btnReset.Text = "リセット";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // textElaspedTimeMinutes
            // 
            this.textElaspedTimeMinutes.Location = new System.Drawing.Point(906, 142);
            this.textElaspedTimeMinutes.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.textElaspedTimeMinutes.Name = "textElaspedTimeMinutes";
            this.textElaspedTimeMinutes.ReadOnly = true;
            this.textElaspedTimeMinutes.Size = new System.Drawing.Size(52, 25);
            this.textElaspedTimeMinutes.TabIndex = 8;
            this.textElaspedTimeMinutes.Text = "0";
            this.textElaspedTimeMinutes.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(23, 267);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(1088, 26);
            this.comboBox1.Sorted = true;
            this.comboBox1.TabIndex = 9;
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(23, 22);
            this.treeView1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(729, 236);
            this.treeView1.TabIndex = 10;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(773, 146);
            this.label5.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 18);
            this.label5.TabIndex = 11;
            this.label5.Text = "経過時間";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(978, 146);
            this.label6.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(26, 18);
            this.label6.TabIndex = 12;
            this.label6.Text = "分";
            // 
            // textElaspedTimeSeconds
            // 
            this.textElaspedTimeSeconds.Location = new System.Drawing.Point(1021, 142);
            this.textElaspedTimeSeconds.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.textElaspedTimeSeconds.Name = "textElaspedTimeSeconds";
            this.textElaspedTimeSeconds.ReadOnly = true;
            this.textElaspedTimeSeconds.Size = new System.Drawing.Size(49, 25);
            this.textElaspedTimeSeconds.TabIndex = 13;
            this.textElaspedTimeSeconds.Text = "0";
            this.textElaspedTimeSeconds.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(1083, 146);
            this.label7.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(26, 18);
            this.label7.TabIndex = 14;
            this.label7.Text = "秒";
            // 
            // btnShowLog
            // 
            this.btnShowLog.Location = new System.Drawing.Point(1124, 629);
            this.btnShowLog.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnShowLog.Name = "btnShowLog";
            this.btnShowLog.Size = new System.Drawing.Size(143, 34);
            this.btnShowLog.TabIndex = 15;
            this.btnShowLog.Text = "ログ表示";
            this.btnShowLog.UseVisualStyleBackColor = true;
            this.btnShowLog.Click += new System.EventHandler(this.buttonShowLog_Click);
            // 
            // btnAddNode
            // 
            this.btnAddNode.Location = new System.Drawing.Point(1546, 587);
            this.btnAddNode.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnAddNode.Name = "btnAddNode";
            this.btnAddNode.Size = new System.Drawing.Size(143, 34);
            this.btnAddNode.TabIndex = 16;
            this.btnAddNode.Text = "AddNode";
            this.btnAddNode.UseVisualStyleBackColor = true;
            this.btnAddNode.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnRemoveNode
            // 
            this.btnRemoveNode.Location = new System.Drawing.Point(1546, 629);
            this.btnRemoveNode.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnRemoveNode.Name = "btnRemoveNode";
            this.btnRemoveNode.Size = new System.Drawing.Size(143, 34);
            this.btnRemoveNode.TabIndex = 17;
            this.btnRemoveNode.Text = "RemoveNode";
            this.btnRemoveNode.UseVisualStyleBackColor = true;
            this.btnRemoveNode.Click += new System.EventHandler(this.btnRemoveNode_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(1121, 410);
            this.label8.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(100, 18);
            this.label8.TabIndex = 18;
            this.label8.Text = "1.ToDoリスト";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(1121, 439);
            this.label9.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(156, 18);
            this.label9.TabIndex = 19;
            this.label9.Text = "2.手順（ワークフロー）";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(1121, 470);
            this.label10.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(106, 18);
            this.label10.TabIndex = 20;
            this.label10.Text = "3.チェックリスト";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(1121, 500);
            this.label11.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(158, 18);
            this.label11.TabIndex = 21;
            this.label11.Text = "4.テンプレート（雛形）";
            // 
            // textLastStopTime1
            // 
            this.textLastStopTime1.Location = new System.Drawing.Point(921, 184);
            this.textLastStopTime1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.textLastStopTime1.Name = "textLastStopTime1";
            this.textLastStopTime1.ReadOnly = true;
            this.textLastStopTime1.Size = new System.Drawing.Size(187, 25);
            this.textLastStopTime1.TabIndex = 23;
            this.textLastStopTime1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(773, 188);
            this.label12.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(125, 18);
            this.label12.TabIndex = 2;
            this.label12.Text = "前回操作時刻1";
            // 
            // textLastStopTime2
            // 
            this.textLastStopTime2.Location = new System.Drawing.Point(921, 223);
            this.textLastStopTime2.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.textLastStopTime2.Name = "textLastStopTime2";
            this.textLastStopTime2.ReadOnly = true;
            this.textLastStopTime2.Size = new System.Drawing.Size(187, 25);
            this.textLastStopTime2.TabIndex = 25;
            this.textLastStopTime2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(773, 228);
            this.label13.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(125, 18);
            this.label13.TabIndex = 24;
            this.label13.Text = "前回操作時刻2";
            // 
            // btnShowGraph
            // 
            this.btnShowGraph.Location = new System.Drawing.Point(1699, 629);
            this.btnShowGraph.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnShowGraph.Name = "btnShowGraph";
            this.btnShowGraph.Size = new System.Drawing.Size(143, 34);
            this.btnShowGraph.TabIndex = 15;
            this.btnShowGraph.Text = "ShowGraph";
            this.btnShowGraph.UseVisualStyleBackColor = true;
            this.btnShowGraph.Click += new System.EventHandler(this.buttonShowGraph_Click);
            // 
            // chart1
            // 
            chartArea8.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea8);
            legend8.Name = "Legend1";
            this.chart1.Legends.Add(legend8);
            this.chart1.Location = new System.Drawing.Point(1124, 22);
            this.chart1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.chart1.Name = "chart1";
            series8.ChartArea = "ChartArea1";
            series8.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Doughnut;
            series8.Legend = "Legend1";
            series8.Name = "Series1";
            series8.Points.Add(dataPoint22);
            series8.Points.Add(dataPoint23);
            series8.Points.Add(dataPoint24);
            this.chart1.Series.Add(series8);
            this.chart1.Size = new System.Drawing.Size(363, 270);
            this.chart1.TabIndex = 22;
            this.chart1.Text = "chart1";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(1513, 22);
            this.panel1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(329, 271);
            this.panel1.TabIndex = 26;
            // 
            // btnAddPanel
            // 
            this.btnAddPanel.Location = new System.Drawing.Point(1699, 587);
            this.btnAddPanel.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnAddPanel.Name = "btnAddPanel";
            this.btnAddPanel.Size = new System.Drawing.Size(143, 34);
            this.btnAddPanel.TabIndex = 27;
            this.btnAddPanel.Text = "AddPanel";
            this.btnAddPanel.UseVisualStyleBackColor = true;
            this.btnAddPanel.Click += new System.EventHandler(this.buttonAddPanel_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(23, 304);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(1088, 359);
            this.textBox1.TabIndex = 28;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1124, 975);
            this.button1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(143, 34);
            this.button1.TabIndex = 29;
            this.button1.Text = "表示";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btnSaveActivityLog
            // 
            this.btnSaveActivityLog.Location = new System.Drawing.Point(1124, 587);
            this.btnSaveActivityLog.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnSaveActivityLog.Name = "btnSaveActivityLog";
            this.btnSaveActivityLog.Size = new System.Drawing.Size(143, 34);
            this.btnSaveActivityLog.TabIndex = 15;
            this.btnSaveActivityLog.Text = "保存";
            this.btnSaveActivityLog.UseVisualStyleBackColor = true;
            this.btnSaveActivityLog.Click += new System.EventHandler(this.btnSaveActivityLog_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(23, 678);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 27;
            this.dataGridView1.Size = new System.Drawing.Size(1819, 332);
            this.dataGridView1.TabIndex = 30;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(1121, 307);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(79, 18);
            this.label14.TabIndex = 31;
            this.label14.Text = "ムリ：計画";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(1121, 340);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(81, 18);
            this.label15.TabIndex = 32;
            this.label15.Text = "ムダ：実行";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(1121, 370);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(80, 18);
            this.label16.TabIndex = 33;
            this.label16.Text = "ムラ：評価";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(1300, 304);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(113, 18);
            this.label17.TabIndex = 34;
            this.label17.Text = "改善の方向性";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(1300, 340);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(190, 18);
            this.label18.TabIndex = 35;
            this.label18.Text = "やるべきことからずれている";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(1300, 370);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(188, 18);
            this.label19.TabIndex = 36;
            this.label19.Text = "もっとうまくやる方法ないか";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(1300, 410);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(122, 18);
            this.label20.TabIndex = 37;
            this.label20.Text = "取り巻くシステム";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(1300, 439);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(110, 18);
            this.label21.TabIndex = 38;
            this.label21.Text = "仕事の進め方";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(1300, 470);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(142, 18);
            this.label22.TabIndex = 39;
            this.label22.Text = "無駄をなくせないか";
            // 
            // btnSaveReviewedActivityLog
            // 
            this.btnSaveReviewedActivityLog.Location = new System.Drawing.Point(1318, 587);
            this.btnSaveReviewedActivityLog.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnSaveReviewedActivityLog.Name = "btnSaveReviewedActivityLog";
            this.btnSaveReviewedActivityLog.Size = new System.Drawing.Size(143, 34);
            this.btnSaveReviewedActivityLog.TabIndex = 40;
            this.btnSaveReviewedActivityLog.Text = "RAL保存";
            this.btnSaveReviewedActivityLog.UseVisualStyleBackColor = true;
            this.btnSaveReviewedActivityLog.Click += new System.EventHandler(this.btnSaveReviewedActivityLog_Click);
            // 
            // btnShowGraphReviewedActivityLog
            // 
            this.btnShowGraphReviewedActivityLog.Location = new System.Drawing.Point(1318, 629);
            this.btnShowGraphReviewedActivityLog.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnShowGraphReviewedActivityLog.Name = "btnShowGraphReviewedActivityLog";
            this.btnShowGraphReviewedActivityLog.Size = new System.Drawing.Size(143, 34);
            this.btnShowGraphReviewedActivityLog.TabIndex = 41;
            this.btnShowGraphReviewedActivityLog.Text = "RALグラフ表示";
            this.btnShowGraphReviewedActivityLog.UseVisualStyleBackColor = true;
            this.btnShowGraphReviewedActivityLog.Click += new System.EventHandler(this.btnShowGraphReviewedActivityLog_Click);
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(1510, 307);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(62, 18);
            this.label23.TabIndex = 42;
            this.label23.Text = "効率化";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(1510, 340);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(80, 18);
            this.label24.TabIndex = 43;
            this.label24.Text = "人材育成";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(1510, 370);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(169, 18);
            this.label25.TabIndex = 44;
            this.label25.Text = "人の集まる場所を作る";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(1510, 410);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(121, 18);
            this.label26.TabIndex = 45;
            this.label26.Text = "Code of Values";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(1510, 439);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(261, 18);
            this.label27.TabIndex = 46;
            this.label27.Text = "視線は外向き。未来を見通すように";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(1510, 470);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(264, 18);
            this.label28.TabIndex = 47;
            this.label28.Text = "思考はシンプル。戦略を示せるように";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(1510, 500);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(249, 18);
            this.label29.TabIndex = 48;
            this.label29.Text = "心は情熱的。自らやり遂げるように";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(1510, 529);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(277, 18);
            this.label30.TabIndex = 49;
            this.label30.Text = "行動はスピード。チャンスを逃さぬように";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(1510, 559);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(296, 18);
            this.label31.TabIndex = 50;
            this.label31.Text = "組織はオープン。全員が成長できるように";
            // 
            // FormTimer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2478, 1444);
            this.Controls.Add(this.label31);
            this.Controls.Add(this.label30);
            this.Controls.Add(this.label29);
            this.Controls.Add(this.label28);
            this.Controls.Add(this.label27);
            this.Controls.Add(this.label26);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.label24);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.btnShowGraphReviewedActivityLog);
            this.Controls.Add(this.btnSaveReviewedActivityLog);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnAddPanel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.textLastStopTime2);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.textLastStopTime1);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btnRemoveNode);
            this.Controls.Add(this.btnAddNode);
            this.Controls.Add(this.btnShowGraph);
            this.Controls.Add(this.btnSaveActivityLog);
            this.Controls.Add(this.btnShowLog);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textElaspedTimeSeconds);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.textElaspedTimeMinutes);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.textRemainingTime);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.textSetTime);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "FormTimer";
            this.Text = "タイマー";
            this.Load += new System.EventHandler(this.FormTimer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textSetTime;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox textRemainingTime;
        private System.Windows.Forms.Timer timerControl;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.TextBox textElaspedTimeMinutes;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textElaspedTimeSeconds;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnShowLog;
        private System.Windows.Forms.Button btnAddNode;
        private System.Windows.Forms.Button btnRemoveNode;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textLastStopTime1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textLastStopTime2;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnShowGraph;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnAddPanel;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnSaveActivityLog;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Button btnSaveReviewedActivityLog;
        private System.Windows.Forms.Button btnShowGraphReviewedActivityLog;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label31;
    }
}

