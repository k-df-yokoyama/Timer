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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textSetTime = new System.Windows.Forms.TextBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.textRemainingTime = new System.Windows.Forms.TextBox();
            this.timerControl = new System.Windows.Forms.Timer(this.components);
            this.buttonReset = new System.Windows.Forms.Button();
            this.textElaspedTimeMinutes = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textElaspedTimeSeconds = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.buttonShowLog = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "時間設定";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(185, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "秒";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "残り時間";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(185, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "秒";
            // 
            // textSetTime
            // 
            this.textSetTime.Location = new System.Drawing.Point(79, 12);
            this.textSetTime.Name = "textSetTime";
            this.textSetTime.Size = new System.Drawing.Size(100, 19);
            this.textSetTime.TabIndex = 4;
            this.textSetTime.Text = "1500";
            this.textSetTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textSetTime.TextChanged += new System.EventHandler(this.textSetTime_TextChanged);
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(12, 37);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(85, 23);
            this.buttonStart.TabIndex = 5;
            this.buttonStart.Text = "スタート！";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // textRemainingTime
            // 
            this.textRemainingTime.Location = new System.Drawing.Point(79, 66);
            this.textRemainingTime.Name = "textRemainingTime";
            this.textRemainingTime.ReadOnly = true;
            this.textRemainingTime.Size = new System.Drawing.Size(100, 19);
            this.textRemainingTime.TabIndex = 6;
            this.textRemainingTime.Text = "1500";
            this.textRemainingTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // timerControl
            // 
            this.timerControl.Interval = 1000;
            this.timerControl.Tick += new System.EventHandler(this.timerControl_Tick);
            // 
            // buttonReset
            // 
            this.buttonReset.Location = new System.Drawing.Point(118, 37);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(85, 23);
            this.buttonReset.TabIndex = 7;
            this.buttonReset.Text = "リセット";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // textElaspedTimeMinutes
            // 
            this.textElaspedTimeMinutes.Location = new System.Drawing.Point(79, 108);
            this.textElaspedTimeMinutes.Name = "textElaspedTimeMinutes";
            this.textElaspedTimeMinutes.ReadOnly = true;
            this.textElaspedTimeMinutes.Size = new System.Drawing.Size(33, 19);
            this.textElaspedTimeMinutes.TabIndex = 8;
            this.textElaspedTimeMinutes.Text = "0";
            this.textElaspedTimeMinutes.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(12, 147);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(281, 20);
            this.comboBox1.Sorted = true;
            this.comboBox1.TabIndex = 9;
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(229, 15);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(327, 97);
            this.treeView1.TabIndex = 10;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 111);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 11;
            this.label5.Text = "経過時間";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(122, 111);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 12);
            this.label6.TabIndex = 12;
            this.label6.Text = "分";
            // 
            // textElaspedTimeSeconds
            // 
            this.textElaspedTimeSeconds.Location = new System.Drawing.Point(148, 108);
            this.textElaspedTimeSeconds.Name = "textElaspedTimeSeconds";
            this.textElaspedTimeSeconds.ReadOnly = true;
            this.textElaspedTimeSeconds.Size = new System.Drawing.Size(31, 19);
            this.textElaspedTimeSeconds.TabIndex = 13;
            this.textElaspedTimeSeconds.Text = "0";
            this.textElaspedTimeSeconds.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(185, 111);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(17, 12);
            this.label7.TabIndex = 14;
            this.label7.Text = "秒";
            // 
            // buttonShowLog
            // 
            this.buttonShowLog.Location = new System.Drawing.Point(302, 144);
            this.buttonShowLog.Name = "buttonShowLog";
            this.buttonShowLog.Size = new System.Drawing.Size(85, 23);
            this.buttonShowLog.TabIndex = 15;
            this.buttonShowLog.Text = "ログ表示";
            this.buttonShowLog.UseVisualStyleBackColor = true;
            this.buttonShowLog.Click += new System.EventHandler(this.buttonShowLog_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(562, 15);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 16;
            this.button1.Text = "AddNode";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(562, 45);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 17;
            this.button2.Text = "RemoveNode";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // FormTimer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(660, 179);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonShowLog);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textElaspedTimeSeconds);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.textElaspedTimeMinutes);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.textRemainingTime);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.textSetTime);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "FormTimer";
            this.Text = "タイマー";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textSetTime;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.TextBox textRemainingTime;
        private System.Windows.Forms.Timer timerControl;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.TextBox textElaspedTimeMinutes;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textElaspedTimeSeconds;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button buttonShowLog;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}

