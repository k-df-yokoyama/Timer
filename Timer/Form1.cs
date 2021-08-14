using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms.DataVisualization.Charting;

namespace Timer
{

    public enum DrawRange
    {
        SeveralDays,
        SingleDay,
        Am,
        Pm,
        None
    }

    public partial class FormTimer : Form
    {
        private static readonly string inputHistoryFname = "timer_act_input_history.txt";
        private static readonly string inputLogFname = "timer_act_input.log";
        private static readonly string rawActivityListFname = "timer_raw_act_list.txt";
        private static readonly string reviewedActivityListFname = "timer_reviewed_act_list.log";

        int currEndTime; // 現在のタスク終了までの予定時間（[時間設定]TextBoxから取得した値）
        int prevEndTime = 1500; // 前回のタスク終了までの予定時間のデフォルト値
        //ToDo: prevEndTimeに設定されているマジックナンバー(1500)を定数にする
        int nowTime; // 経過時間（残り時間を計算するために使用）
        //internal bool isTimeCounting; // 時間のカウント中かの判定
        bool isTimeCounting; // 時間のカウント中かの判定
        string desktopDirectory;
        string inputHistoryFilePath, inputLogFilePath, rawActivityListFilePath, reviewedActivityListFilePath;
        internal Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");
        //ArrayList al = new ArrayList();
        List<string> stringList = new List<string>();
        bool isPieChart; // true=PieChart、false=StackedColumn
        //
        internal System.Windows.Forms.TextBox myTextBox1;

        //List<Task> taskList = new List<Task>();
        TaskList taskList = new TaskList();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormTimer()
        {
            InitializeComponent();

            // 変数の初期化（変数：時間のカウント中かの判定）
            isTimeCounting = false;
            // 変数の初期化（変数：経過時間）
            nowTime = 0;
            desktopDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            inputHistoryFilePath = ".\\" + inputHistoryFname;
            inputLogFilePath = desktopDirectory + "\\" + inputLogFname;
            rawActivityListFilePath = ".\\" + rawActivityListFname;
            reviewedActivityListFilePath = ".\\" + reviewedActivityListFname;

            // 変数の初期化（変数：時間のカウント中かの判定）
            isPieChart = true;

            myTextBox1 = textBox1;

            // 履歴ファイルから値を読み取る
            ReadInputHistory();

            // コンボボックスに値を設定する
//            for (int i = 0; i < stringList.Count; i++) {
//                bool isWhiteSpaceOnly = !Regex.IsMatch(al[i], "[^ 　]");
//                if (!isWhiteSpaceOnly) { comboBox1.Items.Add(al[i]); }
//            }
            foreach (string s in stringList)
            {
                bool isWhiteSpaceOnly = !Regex.IsMatch(s, "[^ 　]");
                //入力値の重複チェック（アイテム内に無いときは-1が返る）
                int idx = comboBox1.Items.IndexOf(s);
                if (!isWhiteSpaceOnly && idx == -1) 
                { 
                    comboBox1.Items.Add(s); 
                    //treeView1.Nodes.Add(s); 

                    // 文字列sを:で分割する
                    //string[] ary = s.Split(':');
                    string[] separators = { ":", "：" };
                    string[] ary = s.Split(separators, StringSplitOptions.None);
                    //三番目の要素(C)を出力
                    //Console.WriteLine(ary[0]);
                    RecursiveAddTree(treeView1.Nodes, ary, 0);
                    // 分割した先頭を取得
                    // 指定された親の子を１つ取得する。
                        // 先頭が既存の先頭と同じ場合→既存の先頭の子供として２番目を追加
                    // 先頭が既存の先頭と違う場合→小としてAddし、２番目以降も追加する。
                }
            }

            // RawActivityListファイルの中身をActivityListのGUIに読み込む
            RawActivityList2TextBox();
            // RawActivityListファイルの中身をTaskListに読み込む
            RawActivityList2TaskList();

            //Hide developping controls
            btnAddNode.Visible = false;
            btnRemoveNode.Visible = false;
            label8.Visible = true;
            label9.Visible = true;
            label10.Visible = true;
            label11.Visible = true;
            label12.Visible = false;
            label13.Visible = false;
            textLastStopTime1.Visible = false;
            textLastStopTime2.Visible = false;
            panel1.Visible = false;
            btnShowGraph.Visible = false;
            btnAddPanel.Visible = false;
            this.Width = Constants.MainFormWidthFormal;
        }

        /// <summary>
        /// Constructor to show developping controls
        /// <param name="args">起動引数</param>
        /// </summary>
        public FormTimer(string[] args):this()
        {
            if (args.Length >= 1 && args[0] == "-showdev") {
                btnAddNode.Visible = true;
                btnRemoveNode.Visible = true;
                label8.Visible = true;
                label9.Visible = true;
                label10.Visible = true;
                label11.Visible = true;
                label12.Visible = true;
                label13.Visible = true;
                textLastStopTime1.Visible = true;
                textLastStopTime2.Visible = true;
                panel1.Visible = true;
                btnShowGraph.Visible = true;
                btnAddPanel.Visible = true;
                this.Width = Constants.MainFormWidthDevelopping;
            }
        }

        /// <summary>
        /// [スタート]/[ストップ]ボタンクリック時の処理
        /// <param name="sender">イベントを送信したオブジェクト</param>
        /// <param name="e">イベントに関わる引数</param>
        /// </summary>
        internal void btnStart_Click(object sender, EventArgs e)
        {
            // 時間のカウント中でない場合（＝ストップ状態でスタートボタンがクリックされた場合）
            if (!isTimeCounting)
            {
                // [時間設定]TextBoxの値を変数に格納（変数：タスク終了までの予定時間）
                if (!int.TryParse(textSetTime.Text, out currEndTime))
                {
                    currEndTime = 1;
                }
                // タスク終了までの予定時間の設定値が前回と異なる場合リセットしてスタートする
                if (prevEndTime != currEndTime)
                {
                    // リセットしたことを知らせる
                    MessageBox.Show("[時間設定]の値が変更されました。リセットしてスタートします");
                    // 変数の初期化（変数：経過時間）
                    nowTime = 0;
                    // 時間設定のTextBoxの内容を残り時間のTextBoxの内容に設定
                    textRemainingTime.Text = textSetTime.Text;
                }
                // 作業内容の履歴の保存
                // ToDo: UnitTest候補
                //SaveInputHistory(textBox1.Text);
                SaveInputHistory(comboBox1.Text);
                // 作業内容の履歴の保存
                AddRawActivityList(comboBox1.Text);
                // タスク終了までの予定時間の格納
                prevEndTime = currEndTime;
                // タイマースタート
                timerControl.Start();
                isTimeCounting = true;
                // スタート／ストップボタンの表示をストップにする
                btnStart.Text = "ストップ";
                WriteInputLog("スタート," + comboBox1.Text);
                // 作業内容のテキストボックスを読み取り専用にする
                this.comboBox1.Enabled = false;
            }
            // 時間のカウント中の場合（＝スタート状態でストップボタンがクリックされた場合）
            else
            {
                // タイマーストップ
                timerControl.Stop();
                isTimeCounting = false;
                // スタート／ストップボタンの表示をスタート！にする
                btnStart.Text = "スタート！";
                WriteInputLog("ストップ," + comboBox1.Text);
                // コンボボックスのリストに値を追加する
                // ...空白文字のみの場合、追加しない
                bool isWhiteSpaceOnly = !Regex.IsMatch(comboBox1.Text, "[^ 　]");
                int idx = comboBox1.Items.IndexOf(comboBox1.Text);
                if (!isWhiteSpaceOnly && idx == -1) { comboBox1.Items.Insert(0,comboBox1.Text); }
                // 作業内容のテキストボックスの読み取り専用を解除する
                this.comboBox1.Enabled = true;
            }
            // 前回停止時間のTextBoxの内容を現在の時間の内容に設定
            textLastStopTime1.Text = textLastStopTime2.Text;
            textLastStopTime2.Text = DateTime.Now.ToString();

            // 作業内容のテキストから開始時間と終了時間を取得する
            string startTime = "00:00", endTime = "00:00";
            if (Utils.GetStartAndEndTimeFromTrailing(comboBox1.Text, out startTime, out endTime) == 0) {
                string task = "";
                Utils.GetTaskFromInputText(comboBox1.Text, out task);
                // グラフを再描画する
                //GraphUtils.ShowChartColumn(chart1, task, startTime, endTime);
                GraphUtils.ShowChartColumn(chart1, task, taskList);
            }
        }

        /// <summary>
        /// [時間設定]の時間が経過したらダイアログ[時間になりました]を表示する。
        /// <param name="sender">イベントを送信したオブジェクト</param>
        /// <param name="e">イベントに関わる引数</param>
        /// </summary>
        private void timerControl_Tick(object sender, EventArgs e)
        {
            int remainingTime; // 残り時間の変数を整数型で定義
            // 経過時間に1秒を加える
            nowTime++;
            // 残り時間を計算して表示
            remainingTime = currEndTime - nowTime;
            textRemainingTime.Text = remainingTime.ToString();
            textElaspedTimeMinutes.Text = (nowTime/60).ToString();
            textElaspedTimeSeconds.Text = (nowTime%60).ToString();
            // <判定>タスク終了までの予定時間になった？
            // ...「Yes」の場合の処理
            if (currEndTime == nowTime)
            {
                // ...タイマーを止める
                //timerControl.Stop();
                // ...終了時間になったことを知らせる
                MessageBox.Show("時間になりました！");
            }
            // <判定>タスク終了までの予定時間になった？
            // ...「No」の場合の処理
            else
            {
                // なにもしない
            }
        }

        /// <summary>
        /// [リセット]ボタンクリック時の処理
        /// <param name="sender">イベントを送信したオブジェクト</param>
        /// <param name="e">イベントに関わる引数</param>
        /// </summary>
        private void btnReset_Click(object sender, EventArgs e)
        {
            // (1)リセット実施可否確認
            DialogResult result = MessageBox.Show("リセットしてもよいですか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            //「いいえ」が選択された時
            if (result == DialogResult.No)
            {
                return;
            }

            // (2)タイマーストップ
            timerControl.Stop();
            isTimeCounting = false;
            // (3)経過時間を0に変更
            nowTime = 0;
            // (4)時間設定のTextBoxの内容を残り時間のTextBoxの内容に設定
            textRemainingTime.Text = textSetTime.Text;
            // (5)前回停止時間のTextBoxの内容を現在の時間の内容に設定
            textLastStopTime1.Text = textLastStopTime2.Text;
            textLastStopTime2.Text = DateTime.Now.ToString();
            // (6)スタート／ストップボタンの表示をスタート！にする
            btnStart.Text = "スタート！";
            WriteInputLog("リセット," + comboBox1.Text);
            // (7)作業内容の履歴の保存
            // ToDo: UnitTest候補
            SaveInputHistory(comboBox1.Text);
            // (7)-2作業内容の履歴の保存
            AddRawActivityList(comboBox1.Text);
            // (8)コンボボックスのリストに値を追加する
            // ...空白文字のみの場合、追加しない
            bool isWhiteSpaceOnly = !Regex.IsMatch(comboBox1.Text, "[^ 　]");
            int idx = comboBox1.Items.IndexOf(comboBox1.Text);
            if (!isWhiteSpaceOnly && idx == -1) { comboBox1.Items.Insert(0, comboBox1.Text); }
            // (9)作業内容のテキストボックスの読み取り専用を解除する
            this.comboBox1.Enabled = true;
            // (10)リセットしたことを知らせる
            //MessageBox.Show("リセットしました！");

            // 作業内容のテキストから開始時間と終了時間を取得する
            string startTime = "00:00", endTime = "00:00";
            if (Utils.GetStartAndEndTimeFromTrailing(comboBox1.Text, out startTime, out endTime) == 0)
            {
                string task = "";
                Utils.GetTaskFromInputText(comboBox1.Text, out task);
                // グラフを再描画する
                //GraphUtils.ShowChartColumn(chart1, task, startTime, endTime);
                GraphUtils.ShowChartColumn(chart1, task, taskList);
            }
        }

        /// <summary>
        /// xxxされた時の処理
        /// <param name="sender">イベントを送信したオブジェクト</param>
        /// <param name="e">イベントに関わる引数</param>
        /// </summary>
        private void textSetTime_TextChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("リセットしました！");
            //btnReset_Click(sender, e);
        }

        //Todo: UnitTest候補
        //Todo: stringListは引数渡しにした方がよい
        /// <summary>
        /// 履歴ファイルから値を読み取る
        /// </summary>
        private void ReadInputHistory()
        {
            string line = "";

            // ファイルがあるか確認する

            // ファイルがない場合→終了する

            // ファイルがある場合
            try {
                using (StreamReader sr = new StreamReader(
                    @inputHistoryFilePath, sjisEnc)) {
                    while ((line = sr.ReadLine()) != null) {
                        //al.Add(line);
                        stringList.Add(line);
                    }
                }
            } catch (Exception e) {
                // 何もしない
            }

            stringList.Sort();

            //for (int i = 0; i < al.Count; i++) {
                //MessageBox.Show(al[i]);
            //}

            //（1）テキスト・ファイルを開く
            //StreamReader sr = new StreamReader(@inputHistoryFilePath, sjisEnc);
            //（2）テキスト内容を読み込む
            //string text = sr.ReadToEnd();
            //（3）テキスト・ファイルを閉じる
            //sr.Close();

        }
        
        /// <summary>
        /// 入力文字列を履歴ファイルに追記する。
        /// inStringのフォーマットは以下。
        /// "Task:00:00-"
        /// "Task:00:00-00:15"
        /// "Task:00:00-00:15-"
        /// "Task:00:00-00:15-00:30"
        /// 履歴ファイルに出力するフォーマットは以下。
        /// "Task"
        /// <param name="inString">入力文字列</param>
        /// </summary>
        private void SaveInputHistory(string inString)
        {
            //（1）テキスト・ファイルを開く、もしくは作成する
            StreamWriter sw = new StreamWriter(@inputHistoryFilePath, true, sjisEnc);
            //ToDo:書き込もうとしている値とファイル中の値が重複していないか確認
            //（2）テキスト内容を書き込む
            //ToDo:時間の部分は削除する
            string outString = "";
            Utils.RemoveTimeString(inString, out outString);
            //sw.WriteLine(DateTime.Now + "," + outString);
            sw.WriteLine(outString);
            //（3）テキスト・ファイルを閉じる
            sw.Close();
        }
        
        /// <summary>
        /// Output Log
        /// Log format: datetime,(スタート|ストップ|リセット),task[:starttime-endtime]
        /// <param name="outString">出力文字列:=(スタート|ストップ|リセット),task[:starttime-endtime]</param>
        /// </summary>
        private void WriteInputLog(string outString)
        {
            //（1）テキスト・ファイルを開く、もしくは作成する
            StreamWriter writer = new StreamWriter(@inputLogFilePath, true, sjisEnc);
            //（2）テキスト内容を書き込む
            //writer.WriteLine("テスト書き込みです。");
            writer.WriteLine(DateTime.Now + "," + outString);
            //（3）テキスト・ファイルを閉じる
            writer.Close();
        }

#if NOTDEF
        /// <summary>
        /// ホワイトスペースかを判定する
        /// <param name="inString">入力文字列</param>
        /// </summary>
        private bool isWhiteSpace(string inString)
        {
            return true;
        }
#endif

        /// <summary>
        /// [ログ表示]ボタンがクリックされた時の処理
        /// <param name="sender">イベントを送信したオブジェクト</param>
        /// <param name="e">イベントに関わる引数</param>
        /// </summary>
        private void buttonShowLog_Click(object sender, EventArgs e)
        {
            //Form2クラスのインスタンスを作成する
            FormShowLog f = new FormShowLog();
            //Form2を表示する
            //ここではモーダルダイアログボックスとして表示する
            //オーナーウィンドウにthisを指定する
            f.ShowDialog(this);
            //フォームが必要なくなったところで、Disposeを呼び出す
            f.Dispose();
        }

        /// <summary>
        /// xxxされた時の処理
        /// <param name="sender">イベントを送信したオブジェクト</param>
        /// <param name="e">イベントに関わる引数</param>
        /// </summary>
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //MessageBox.Show(e.Node.Text);
            comboBox1.Text = e.Node.Text;
            TreeNode ctn,ptn;
            ctn = treeView1.SelectedNode;
            while ((ptn = ctn.Parent) != null) {
                comboBox1.Text = ptn.Text + "：" + comboBox1.Text;
                ctn = ptn;
            }
        }

        /// <summary>
        /// [AddNode]ボタンがクリックされた時の処理
        /// <param name="sender">イベントを送信したオブジェクト</param>
        /// <param name="e">イベントに関わる引数</param>
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Add("company");
            treeView1.Nodes.Add("dpartment");
            treeView1.Nodes.Add("group");
            treeView1.Nodes.Add("01.group");
            treeView1.Nodes.Add("02.group");
            treeView1.Nodes.Add("03.group");
            treeView1.Nodes.Add("04.group");
            treeView1.Nodes.Add("05.group");

            treeView1.Nodes[0].Nodes.Add("company1");
        }

        /// <summary>
        /// [RemoveNode]ボタンがクリックされた時の処理
        /// <param name="sender">イベントを送信したオブジェクト</param>
        /// <param name="e">イベントに関わる引数</param>
        /// </summary>
        private void btnRemoveNode_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
                treeView1.SelectedNode.Remove();
            //treeView1.Nodes.Clear();
        }

        /// <summary>
        /// 
        /// <param name="tnc"></param>
        /// <param name="ary"></param>
        /// <param name="aryIdx"></param>
        /// </summary>
        private void RecursiveAddTree(TreeNodeCollection tnc, string[] ary, int aryIdx)
        {
            // 文字列sを:で分割する
            //string[] ary = s.Split(':');
            //三番目の要素(C)を出力
            //Console.WriteLine(ary[0]);

            // 指定された親の子を１つ取得する。
            foreach (TreeNode tn in tnc)
            {
                // 先頭が既存の先頭と同じ場合→既存の先頭の子供として２番目を追加
                if (tn.Text == ary[aryIdx])
                {
                    if (ary.Length - aryIdx >= 2)
                    {
                        //tn.Nodes.Add(ary[1]);
                        RecursiveAddTree(tn.Nodes, ary, aryIdx + 1);
                    }
                    return;
                }
            }

            // 先頭が既存の先頭と違う場合→小としてAddし、２番目以降も追加する。
 //           for (int i = aryIdx; i < ary.Length; i++)
 //           {
 //               tnc.Add(ary[i]);
 //           }
            tnc.Add(ary[aryIdx]);
            RecursiveAddTree(tnc, ary, aryIdx);
        }

        /// <summary>
        /// [ShowGraph]ボタンがクリックされた時の処理
        /// <param name="sender">イベントを送信したオブジェクト</param>
        /// <param name="e">イベントに関わる引数</param>
        /// </summary>
        private void buttonShowGraph_Click(object sender, EventArgs e)
        {
            if (isPieChart) {
                GraphUtils.ShowChartStackedColumn(chart1);
                isPieChart = false;
            }
            else {
                GraphUtils.ShowChartPie(chart1);
                isPieChart = true;
            }
        }


        /// <summary>
        /// 引数で指定された文字列(タスクと時間の情報)をActivityListのGUI(textBox1)に追加する
        /// 引数で指定された文字列に時間の情報が含まれている場合には、時間の情報を行頭に移動して追加する
        /// <param name="taskAndTime">タスクと時間</param>
        /// </summary>
        private void AddRawActivityList(string taskAndTime)
        {
            Task addedTask = new Task(taskAndTime);

            //taskList.tasks.Add(addedTask);
            taskList.Add(addedTask);

            if (addedTask.startTime.Equals("") || addedTask.endTime.Equals("")) {
                textBox1.Text = textBox1.Text + "\r\n" + addedTask.taskName;
            }
            else {
                textBox1.Text = textBox1.Text + "\r\n" +
                    addedTask.startTime + "-" + addedTask.endTime + "  " + addedTask.taskName;
            }

            //画面上に表示されているActivityListの内容を
            //ActivityListファイルに上書き保存する。
            SaveRawActivityList(textBox1.Text);
            taskList.SaveTaskList();

            //画面上に表示されているActivityListの内容を
            //ReviewedActivityListにコピーする。
            CopyActivityListToReviewedActivityList(textBox1.Text);
        }

        /// <summary>
        /// RawActivityListファイルの中身をActivityListのGUI(textBox1)に読み込む
        /// </summary>
        internal void RawActivityList2TextBox()
        {
            if (!File.Exists(@rawActivityListFilePath))
            {
                StreamWriter sw = new StreamWriter(@rawActivityListFilePath, true, sjisEnc);
                sw.Close();
            }

            string textFromLogFile = File.ReadAllText(@rawActivityListFilePath, sjisEnc);
            textBox1.Text = textFromLogFile;
        }

        /// <summary>
        /// RawActivityListファイルの中身をTaskListに読み込む
        /// </summary>
        internal void RawActivityList2TaskList()
        {
            if (!File.Exists(@rawActivityListFilePath))
            {
                StreamWriter sw = new StreamWriter(@rawActivityListFilePath, true, sjisEnc);
                sw.Close();
            }

            StreamReader sr = new StreamReader(@rawActivityListFilePath, sjisEnc);
            while (sr.EndOfStream == false) {
                string line = sr.ReadLine();
                Task addedTask = new Task(line);

                taskList.Add(addedTask);
            }
            sr.Close();
        }

        /// <summary>
        /// dataGridView1の列の定義と初期値の設定
        /// <param name="sender">イベントを送信したオブジェクト</param>
        /// <param name="e">イベントに関わる引数</param>
        /// </summary>
        internal void FormTimer_Load(object sender, EventArgs e)
        {
#if NOTDEF
            // カラム数を指定
            //dataGridView1.ColumnCount = 5;

            // カラム名を指定
            dataGridView1.Columns[0].HeaderText = "From";
            dataGridView1.Columns[1].HeaderText = "To";
            dataGridView1.Columns[2].HeaderText = "Story/Task";
            dataGridView1.Columns[3].HeaderText = "Output";
            dataGridView1.Columns[4].HeaderText = "スキルアップ";

            dataGridView1.Columns[0].Width = 50;
            dataGridView1.Columns[1].Width = 50;
            dataGridView1.Columns[2].Width = 100;
            dataGridView1.Columns[3].Width = 80;
            dataGridView1.Columns[4].Width = 100;
#else
            DataGridViewColumn colA = CreateDataGridViewTextBoxColumn("From", "From", 50, typeof(string));
            DataGridViewColumn colB = CreateDataGridViewTextBoxColumn("To", "To", 50, typeof(string));
            DataGridViewColumn colC = CreateDataGridViewTextBoxColumn("Task", "Task/Story", 100, typeof(string));
            DataGridViewColumn colD = CreateDataGridViewTextBoxColumn("Output", "Output", 80, typeof(string));
            DataGridViewColumn colE = CreateDataGridViewTextBoxColumn("HowToImprove", "How to improve", 120, typeof(string));
            DataGridViewColumn colF = CreateDataGridViewCheckBoxColumn("Stories", "機能実現", 60);
            DataGridViewColumn colG = CreateDataGridViewCheckBoxColumn("Taxes", "税", 40);
            DataGridViewColumn colH = CreateDataGridViewCheckBoxColumn("Spikes", "スパイク", 60);
            DataGridViewColumn colI = CreateDataGridViewCheckBoxColumn("Technical Debt", "前提条件", 60);
            DataGridViewColumn colJ = CreateDataGridViewCheckBoxColumn("Break", "休憩", 40);
            DataGridViewColumn colK = CreateDataGridViewCheckBoxColumn("SkillUp", "緊急", 40);
            DataGridViewColumn colL = CreateDataGridViewCheckBoxColumn("Contribution", "貢献", 40);
            DataGridViewColumn colM = CreateDataGridViewCheckBoxColumn("Proactive", "自発的", 50);
            DataGridViewColumn colN = CreateDataGridViewCheckBoxColumn("Group", "グループ", 60);
            DataGridViewColumn colO = CreateDataGridViewCheckBoxColumn("Time", "時間", 40);
            //DataGridViewColumn colP = CreateDataGridViewCheckBoxColumn("Relative", "関係者", 50);

            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add(colA);
            dataGridView1.Columns.Add(colB);
            dataGridView1.Columns.Add(colC);
            dataGridView1.Columns.Add(colD);
            dataGridView1.Columns.Add(colE);
            dataGridView1.Columns.Add(colF);
            dataGridView1.Columns.Add(colG);
            dataGridView1.Columns.Add(colH);
            dataGridView1.Columns.Add(colI);
            dataGridView1.Columns.Add(colJ);
            dataGridView1.Columns.Add(colK);
            dataGridView1.Columns.Add(colL);
            dataGridView1.Columns.Add(colM);
            dataGridView1.Columns.Add(colN);
            dataGridView1.Columns.Add(colO);
#endif

            // データを追加
            dataGridView1.Rows.Add("08:30", "09:00", "朝会", "ToDo.txt更新", "", false, false, false, false, false, false, false, false, false, false);
            dataGridView1.Rows.Add("09:00", "10:00", "引継ぎ資料作成", "引継ぎ資料", "", true, false, false, false, false, false, false, false, false, false);
            dataGridView1.Rows.Add("10:00", "12:00", "引継ぎ会議", "議事メモ", "", false, false, false, false, false, false, false, false, false, false);
        }

        /// <summary>
        /// DataGridViewにテキストボックスの列を追加する。
        /// <param name="name"></param>
        /// <param name="header"></param>
        /// <param name="width"></param>
        /// <param name="type"></param>
        /// </summary>
        private DataGridViewColumn CreateDataGridViewTextBoxColumn(string name, string header, int width, Type type)
        {
            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
            col.Name = name;
            col.DataPropertyName = name;
            col.HeaderText = header;
            col.ValueType = type;
            col.Width = width;
            return col;
        }

        /// <summary>
        /// DataGridViewにチェックボックスの列を追加する。
        /// <param name="name"></param>
        /// <param name="header"></param>
        /// <param name="width"></param>
        /// </summary>
        private DataGridViewColumn CreateDataGridViewCheckBoxColumn(string name, string header, int width)
        {
            DataGridViewCheckBoxColumn col = new DataGridViewCheckBoxColumn();
            col.Name = name;
            col.DataPropertyName = name; // データソースの name をバインドする
            col.HeaderText = header;
            //checkBoxCol.SortMode = DataGridViewColumnSortMode.NotSortable;
            //checkBoxCol.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //checkBoxCol.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //col.ValueType = type;
            col.Width = width;
            return col;
        }

        /// <summary>
        /// [RALグラフ表示](btnShowGraphReviewedActivityLog)ボタンがクリックされた時の処理
        /// <param name="sender">イベントを送信したオブジェクト</param>
        /// <param name="e">イベントに関わる引数</param>
        /// </summary>
        private void btnShowGraphReviewedActivityList_Click(object sender, EventArgs e)
        {
            GraphUtils.DrawChartDoughnut(chart1, dataGridView1);
        }

/*
public enum Season
{
    Spring,
    Summer,
    Autumn,
    Winter
}
*/

        /// <summary>
        /// 描画範囲の指定
        /// <param name="timeStr"></param>
        /// <param name="drawRange"></param>
        /// </summary>
//        internal int GetDrawRange(string startTime, string endTime, out DrawRange drawRange)
        internal int GetDrawRange(string timeStr, out DrawRange drawRange)
        {
/*
            int intStartHh, intStartMm, intEndHh, intEndMm;
            if (Utils.GetApproximateIntHhAndMm(startTime, endTime, out intStartHh, out intStartMm, out intEndHh, out intEndMm) < 0)
            {
                drawRange = DrawRange.None;
                return (-1);
            }
*/
            string hhStr, mmStr;
            if (GetHhAndMmStr(timeStr, out hhStr, out mmStr) < 0)
            {
                drawRange = DrawRange.None;
                return (-1);
            }

//            if (int.Parse(strMm) >= 0 && int.Parse(strMm) < 8) {

            if (int.Parse(hhStr) >= 0 && int.Parse(hhStr) < 12) {
                drawRange = DrawRange.Am;
            }
            else {
                drawRange = DrawRange.Pm;
            }

            return(0);
        }

        /// <summary>
        /// 時間文字列から時と分の文字列を取得する
        /// <param name="timeStr"></param>
        /// <param name="hhStr"></param>
        /// <param name="mmStr"></param>
        /// </summary>
        internal int GetHhAndMmStr(string timeStr, out string hhStr, out string mmStr)
        {
            //入力値のフォーマットチェック
            if (!Regex.IsMatch(timeStr, @"(0[0-9]|1[0-9]|2[0-4])[:：][0-5][0-9]")) {
                hhStr = null;
                mmStr = null;
                return -1;
            }

            //入力値を時間と分に分割
            hhStr = timeStr.Substring(0, 2);
            mmStr = timeStr.Substring(3, 2);

            return 0;
        }

        /// <summary>
        /// [RAL保存](btnSaveReviewedActivityList)ボタンがクリックされた時の処理
        /// <param name="sender">イベントを送信したオブジェクト</param>
        /// <param name="e">イベントに関わる引数</param>
        /// </summary>
        private void btnSaveReviewedActivityList_Click(object sender, EventArgs e)
        {
            //（1）テキスト・ファイルを開く、もしくは作成する
            StreamWriter sw = new StreamWriter(@reviewedActivityListFilePath, false, sjisEnc);

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    //（2）テキスト内容を書き込む
                    if (column.Index != 0) sw.Write(",");
                    sw.Write(dataGridView1[column.Index, row.Index].Value);
                }
                sw.Write("\r\n");
            }

            //（3）テキスト・ファイルを閉じる
            sw.Close();
        }


        /*
        // ▼チェックボックス型式
        DataGridViewCheckBoxColumn checkBoxCol = new DataGridViewCheckBoxColumn();
        // データソースの "ColSelect" をバインドする
        checkBoxCol.DataPropertyName = "ColSelect";
        // 名前とヘッダーを設定
        checkBoxCol.Name = "ColSelect";
        checkBoxCol.HeaderText = "選択";
        checkBoxCol.SortMode = DataGridViewColumnSortMode.NotSortable;
        checkBoxCol.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
        checkBoxCol.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        checkBoxCol.Width = 50;
        */

        /// <summary>
        /// [保存](btnSaveActivityLog)ボタンがクリックされた時の処理
        /// ...画面上に表示されているActivityListの内容をActivityListファイルに上書き保存する。
        /// ...画面上に表示されているActivityListの内容をReviewedActivityListにコピーする。
        /// <param name="sender">イベントを送信したオブジェクト</param>
        /// <param name="e">イベントに関わる引数</param>
        /// </summary>
        internal void btnSaveActivityLog_Click(object sender, EventArgs e)
        {
            //ActivityListファイルに上書き保存する。
            SaveRawActivityList(textBox1.Text);

            //ReviewedActivityListにコピーする。
            CopyActivityListToReviewedActivityList(textBox1.Text);
        }

        /*
        private void btnStart_Click(object sender, EventArgs e)
        {

        }

        private void btnReset_Click(object sender, EventArgs e)
        {

        }
        */

        /// <summary>
        /// ActivityListファイルに上書き保存する。
        /// </summary>
        private void SaveRawActivityList(string textValue)
        {
            // テキスト・ファイルを開く、もしくは作成する
            StreamWriter sw = new StreamWriter(@rawActivityListFilePath, false, sjisEnc);
            // テキスト内容を書き込む
            sw.Write(textValue);
            // テキスト・ファイルを閉じる
            sw.Close();
        }

        /// <summary>
        /// ActivityListの値をReviewedActivityListにコピーする
        /// </summary>
        private void CopyActivityListToReviewedActivityList(string textValue)
        {
            //ReviewedActivityListの内容をクリアする
            dataGridView1.Rows.Clear();

            //ActivityListのテキストボックスの内容を取得する
            var lineList = textValue.Replace("\r\n","\n").Split(new[]{'\n','\r'});
            foreach(var line in lineList)
            {
                //System.Console.WriteLine($"<{line}>");

                //ActivityListのテキストボックスの内容を1行ずつReviewedActivityListに書き込む
                string startTime, endTime, task;
                //...startTime, endTime, Taskを取得
                //(ActivityListのテキストボックスの内容が書き込み可能かを判定)
                if (Utils.GetStartAndEndTimeAndTaskFromHeading(line, out startTime, out endTime, out task) == 0) {
                    //...startTime, endTime, Taskの書き込み
                    dataGridView1.Rows.Add(startTime, endTime, task, "", "", false, false, false, false, false, false, false, false, false, false);
                }
            }
            return;
        }

        /// <summary>
        /// [AddPanel](btnAddPanel)ボタンがクリックされた時の処理
        /// <param name="sender">イベントを送信したオブジェクト</param>
        /// <param name="e">イベントに関わる引数</param>
        /// </summary>
        private void buttonAddPanel_Click(object sender, EventArgs e)
        {
            foreach(Panel p in panel1.Controls)
            {
                p.Top += 40;
            }

            Panel c = new Panel();
            //c.Width = 120;
            //c.Height = 30;
            //c.Top = 10;
            //c.BackColor = Color.Black;

            c.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            // 相対位置
            c.Location = new System.Drawing.Point(24, 32);
            c.Size = new System.Drawing.Size(120, 30);

            panel1.Controls.Add(c);
        }

    }
}

/*
http://blog.hiros-dot.net/?p=2099
*/
/*
http://nogusa.hateblo.jp/entry/20101004/1286183197
*/

/*
private void Form1_Load(object sender, EventArgs e)          
    {
        // ラベルを読みやすく表示するには、明るい色を利用します。
        c1Chart1.ColorGeneration = ColorGeneration.Flow;  
                
        // ChartArea を最大化します。
        c1Chart1.ChartArea.Margins.SetMargins(0, 0, 0, 0);   
        c1Chart1.ChartArea.Style.Border.BorderStyle = BorderStyleEnum.None;
        // グラフの種類を設定します。 
        c1Chart1.ChartArea.Inverted = true;   
        c1Chart1.ChartGroups[0].ChartType = C1.Win.C1Chart.Chart2DTypeEnum.Pie;
        // 以前のデータをクリアします。 
        c1Chart1.ChartGroups[0].ChartData.SeriesList.Clear();  
        // データを追加します。 
                
        string[] ProductNames = { "Mortgage", "Car", "Food", "Day Care", "Other", "Savings","Utilities" };    
        int[] PriceX = {2000, 1200, 500, 500, 450, 400, 350 };   
                
        // シリーズのコレクションを取得します。   
        ChartDataSeriesCollection dscoll = c1Chart1.ChartGroups[0].ChartData.SeriesList;  
        // シリーズにデータを挿入します。   
        for (int i = 0; i < PriceX.Length; i++)    
        { 
            ChartDataSeries series = dscoll.AddNewSeries();   
            // 円をひとつ表示するには、1つの点を追加します。   
            series.PointData.Length = 1;  
            // Y データ系列に価格を割り当てます。   
            series.Y[0] = PriceX[i];   
            // 凡例上の製品名と製品価格の書式設定します。
            series.Label = string.Format("{0} ({1:c})", ProductNames[i], PriceX[i]);  
            series.DataLabel.Text = "{#TEXT}\r\n{#YVAL} ({%YVAL:%})";
            series.DataLabel.Compass = LabelCompassEnum.RadialText; 
            series.DataLabel.Offset = -5;
            series.DataLabel.Visible = true;         
        }   
        // 円の凡例を表示します。   
        c1Chart1.Legend.Visible = true;
        // グラフの凡例にタイトルを追加します。    
        c1Chart1.Legend.Text = "Monthly Expenses";     
    }
*/


