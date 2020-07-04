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
    public partial class FormTimer : Form
    {
        int currEndTime; // 現在のタスク終了までの予定時間（[時間設定]TextBoxから取得した値）
        int prevEndTime = 1500; // 前回のタスク終了までの予定時間のデフォルト値
        //ToDo: prevEndTimeに設定されているマジックナンバー(1500)を定数にする
        int nowTime; // 経過時間（残り時間を計算するために使用）
        //internal bool isTimeCounting; // 時間のカウント中かの判定
        bool isTimeCounting; // 時間のカウント中かの判定
        string strDesktopDirectory;
        string strHistoryFilePath, strLogFilePath, strActivityLogFilePath, strReviewedActivityLogFilePath;
        internal Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");
        //ArrayList al = new ArrayList();
        List<string> stringList = new List<string>();
        bool isPieChart; // true=PieChart、false=StackedColumn
        //
        internal System.Windows.Forms.TextBox myTextBox1;

        public FormTimer()
        {
            InitializeComponent();

            // 変数の初期化（変数：時間のカウント中かの判定）
            isTimeCounting = false;
            // 変数の初期化（変数：経過時間）
            nowTime = 0;
            strDesktopDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            strHistoryFilePath = ".\\timer_history.txt"; 
            strLogFilePath = strDesktopDirectory + "\\timer.log";
            strActivityLogFilePath = ".\\timer_activity.log";
            strReviewedActivityLogFilePath = ".\\timer_reviewed_activity.log";

            // 変数の初期化（変数：時間のカウント中かの判定）
            isPieChart = true;

            myTextBox1 = textBox1;

            // 履歴ファイルから値を読み取る
            ReadWorkHistory();

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

            // テキストボックスに値を設定する
            ReadActivityLog();

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

        //Constructor to show developping controls
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

        // [スタート]/[ストップ]ボタンクリック時の処理
        internal void buttonStart_Click(object sender, EventArgs e)
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
                //SaveWorkHistory(textBox1.Text);
                SaveWorkHistory(comboBox1.Text);
                // 作業内容の履歴の保存
                AddActivityLog(comboBox1.Text);
                // タスク終了までの予定時間の格納
                prevEndTime = currEndTime;
                // タイマースタート
                timerControl.Start();
                isTimeCounting = true;
                // スタート／ストップボタンの表示をストップにする
                btnStart.Text = "ストップ";
                WriteLog("スタート," + comboBox1.Text);
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
                WriteLog("ストップ," + comboBox1.Text);
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
            if (GetStartAndEndTime(comboBox1.Text, out startTime, out endTime) == 0) {
                // ドーナッツグラフを再描画する
                DrawChartDoughnut(startTime, endTime);
            }
        }

        // [時間設定]の時間が経過したらダイアログ[時間になりました]を表示する。
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

        // [リセット]ボタンクリック時の処理
        private void buttonReset_Click(object sender, EventArgs e)
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
            WriteLog("リセット," + comboBox1.Text);
            // (7)作業内容の履歴の保存
            // ToDo: UnitTest候補
            SaveWorkHistory(comboBox1.Text);
            // (7)-2作業内容の履歴の保存
            AddActivityLog(comboBox1.Text);
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
            if (GetStartAndEndTime(comboBox1.Text, out startTime, out endTime) == 0) {
                // ドーナッツグラフを再描画する
                DrawChartDoughnut(startTime, endTime);
            }
        }

        private void textSetTime_TextChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("リセットしました！");
            //buttonReset_Click(sender, e);
        }

        //Todo: UnitTest候補
        private void ReadWorkHistory()
        {
            string line = "";

            // ファイルがあるか確認する

            // ファイルがない場合→終了する

            // ファイルがある場合
            try {
                using (StreamReader sr = new StreamReader(
                    @strHistoryFilePath, Encoding.GetEncoding("Shift_JIS"))) {
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
            //StreamReader sr = new StreamReader(@strHistoryFilePath, sjisEnc);
            //（2）テキスト内容を読み込む
            //string text = sr.ReadToEnd();
            //（3）テキスト・ファイルを閉じる
            //sr.Close();

        }
        
        // inString から時間部分を削除した文字列を作成する。
        // inString、outStringのフォーマットは以下。
        // IN:  "Start,Task:00:00-"
        // IN:  "Start,Task:00:00-00:15"
        // IN:  "Start,Task:00:00-00:15-"
        // IN:  "Start,Task:00:00-00:15-00:30"
        // OUT: "Start,Task"
        internal void RemoveTimeString(string inString, out string outString)
        {
            Regex re = new Regex("[:：][0-9][0-9][:：][0-9][0-9]-[0-9][0-9][:：][0-9][0-9]-[0-9][0-9][:：][0-9][0-9]$", RegexOptions.Singleline);
            outString = re.Replace(inString, "");
            if (!outString.Equals(inString)) return;

            re = new Regex("[:：][0-9][0-9][:：][0-9][0-9]-[0-9][0-9][:：][0-9][0-9]-$", RegexOptions.Singleline);
            outString = re.Replace(inString, "");
            if (!outString.Equals(inString)) return;

            re = new Regex("[:：][0-9][0-9][:：][0-9][0-9]-[0-9][0-9][:：][0-9][0-9]$", RegexOptions.Singleline);
            outString = re.Replace(inString, "");
            if (!outString.Equals(inString)) return;

            re = new Regex("[:：][0-9][0-9][:：][0-9][0-9]-$", RegexOptions.Singleline);
            outString = re.Replace(inString, "");
            if (!outString.Equals(inString)) return;
        }

        // inStringのフォーマットは以下。
        // "Start,Task:00:00-"
        // "Start,Task:00:00-00:15"
        // "Start,Task:00:00-00:15-"
        // "Start,Task:00:00-00:15-00:30"
        private void SaveWorkHistory(string inString)
        {
            //（1）テキスト・ファイルを開く、もしくは作成する
            StreamWriter sw = new StreamWriter(@strHistoryFilePath, true, sjisEnc);
            //ToDo:書き込もうとしている値とファイル中の値が重複していないか確認
            //（2）テキスト内容を書き込む
            //ToDo:時間の部分は削除する
            string outString = "";
            RemoveTimeString(inString, out outString);
            //sw.WriteLine(DateTime.Now + "," + outString);
            sw.WriteLine(outString);
            //（3）テキスト・ファイルを閉じる
            sw.Close();
        }
        
        // Output Log
        // Log format: datetime,(スタート|ストップ|リセット),task[:starttime-endtime]
        private void WriteLog(string outString)
        {
            //（1）テキスト・ファイルを開く、もしくは作成する
            StreamWriter writer = new StreamWriter(@strLogFilePath, true, sjisEnc);
            //（2）テキスト内容を書き込む
            //writer.WriteLine("テスト書き込みです。");
            writer.WriteLine(DateTime.Now + "," + outString);
            //（3）テキスト・ファイルを閉じる
            writer.Close();
        }

        private bool isWhiteSpace(string outString)
        {
            return true;
        }

        // [ログ表示]ボタンをクリック
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

        // [AddNode]ボタンをクリック
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

        // [RemoveNode]ボタンをクリック
        private void btnRemoveNode_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
                treeView1.SelectedNode.Remove();
            //treeView1.Nodes.Clear();
        }

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

        // [ShowGraph]ボタンをクリック
        private void buttonShowGraph_Click(object sender, EventArgs e)
        {
            if (isPieChart) {
                ShowChartStackedColumn();
                isPieChart = false;
            }
            else {
                ShowChartPie();
                isPieChart = true;
            }
        }

        // 作業内容のテキストボックスの入力値から<開始時間(hh:mm)>と<終了時間(hh:mm)>を取得する
        // 処理できる時間部分のフォーマットと取得結果は以下。
        // "Start,Task:00:00-"
        //   return -1
        // "Start,Task:00:00-00:15"
        //   開始時間：00:00
        //   終了時間：00:15
        // "Start,Task:00:00-00:15-"
        //   return -1
        // "Start,Task:00:00-00:15-00:30"
        //   開始時間：00:00
        //   終了時間：00:30
        internal int GetStartAndEndTime(string taskAndTime, out string startTime, out string endTime)
        {
            bool isFormatOK = true;

            //入力値のフォーマットチェック
            if (!Regex.IsMatch(taskAndTime, @"[:：](0[0-9]|1[0-9]|2[0-3])[:：][0-5][0-9]-(0[0-9]|1[0-9]|2[0-3])[:：][0-5][0-9]$") &&
                !Regex.IsMatch(taskAndTime, @"[:：](0[0-9]|1[0-9]|2[0-3])[:：][0-5][0-9]-24[:：]00$") &&
                !Regex.IsMatch(taskAndTime, @"[:：]24[:：]00-(0[0-9]|1[0-9]|2[0-3])[:：][0-5][0-9]$") &&
                !Regex.IsMatch(taskAndTime, @"[:：]24[:：]00-24[:：]00$")) {
                isFormatOK = false;
            }
            else {
                isFormatOK = true;
            }
            if (!isFormatOK &&
                !Regex.IsMatch(taskAndTime, @"[:：](0[0-9]|1[0-9]|2[0-3])[:：][0-5][0-9]-.*-(0[0-9]|1[0-9]|2[0-3])[:：][0-5][0-9]$") &&
                !Regex.IsMatch(taskAndTime, @"[:：](0[0-9]|1[0-9]|2[0-3])[:：][0-5][0-9]-.*-24[:：]00$") &&
                !Regex.IsMatch(taskAndTime, @"[:：]24[:：]00-.*-(0[0-9]|1[0-9]|2[0-3])[:：][0-5][0-9]$") &&
                !Regex.IsMatch(taskAndTime, @"[:：]24[:：]00-.*-24[:：]00$"))
            {
                isFormatOK = false;
            }
            else {
                isFormatOK = true;
            }
            if (!isFormatOK) {
                startTime = null;
                endTime = null;
                return -1;
            }

            Match matchedObj = Regex.Match(taskAndTime, @"(0[0-9]|1[0-9]|2[0-4])[:：][0-5][0-9]-.*-?(0[0-9]|1[0-9]|2[0-4])[:：][0-5][0-9]$");

            Match startTimeObj = Regex.Match(matchedObj.Value, @"^(0[0-9]|1[0-9]|2[0-4])[:：][0-5][0-9]");
            Match endTimeObj = Regex.Match(matchedObj.Value, @"(0[0-9]|1[0-9]|2[0-4])[:：][0-5][0-9]$");

            startTime = startTimeObj.Value;
            endTime = endTimeObj.Value;

            //入力値を時間と分に分割
            //int found = 0;
            var startHh = startTime.Substring(0, 2);
            var startMm = startTime.Substring(3, 2);
            var endHh = endTime.Substring(0, 2);
            var endMm = endTime.Substring(3, 2);

            return 0;
        }

		//入力フォーマット：hh:mm|hh：mm、00:00から24:00まで可、9:00や10:1は不可
        internal int GetApproximateIntHhAndMm(string startTime, string endTime, out int intStartHh, out int intStartMm,
            out int intEndHh, out int intEndMm)
        {
            intStartHh = -1;
            intStartMm = -1;
            intEndHh = -1;
            intEndMm = -1;

            //入力値のフォーマットチェック
            if (!Regex.IsMatch(startTime, @"(0[0-9]|1[0-9]|2[0-4])[:：][0-5][0-9]")) {
                return -1;
            }
            if (!Regex.IsMatch(endTime, @"(0[0-9]|1[0-9]|2[0-4])[:：][0-5][0-9]")) {
                return -1;
            }

            //入力値を時間と分に分割
            var startHh = startTime.Substring(0, 2);
            var startMm = startTime.Substring(3, 2);
            var endHh = endTime.Substring(0, 2);
            var endMm = endTime.Substring(3, 2);

            //開始時間と終了時間を15分刻みの時間に変換する
            if (GetApproximateIntMm(startMm, out intStartMm) < 0) {
                return -1;
            }
            if (GetApproximateIntMm(endMm, out intEndMm) < 0) {
                return -1;
            }

            //入力値に応じた処理
            //...開始時間の方が終了時間より大きい
            if (int.Parse(startHh) > int.Parse(endHh)) {
                if (int.Parse(endHh) < 12) {
                    startHh = "00";
                    startMm = "00";
                    intStartMm = 0;
                }
                else {
                    startHh = "12";
                    startMm = "00";
                    intStartMm = 0;
                }
            }
            //...開始時間と終了時間が12時をまたぐ
            if (!(int.Parse(endHh) == 12 && int.Parse(endMm) == 0) && int.Parse(startHh) < 12 && 12 <= int.Parse(endHh))
            {
                startHh = "12";
                startMm = "00";
                intStartMm = 0;
            }

            intStartHh = int.Parse(startHh);
            intEndHh = int.Parse(endHh);
            if ((intStartHh == 12 && intStartMm == 0) && (intEndHh == 12 && intEndMm == 0))
            {
                intStartHh -= 12;
                intEndHh -= 12;
            }
            if (intStartHh >= 12) {
                intStartHh -= 12;
            }
            if (intEndHh >= 12 && !(intEndHh ==12 && intEndMm == 0)) {
                intEndHh -= 12;
            }

            return 0;
        }

        //https://www.atmarkit.co.jp/fdotnet/dotnettips/1001aspchartpie/aspchartpie.html
        //<開始時間(hh:mm)>と<終了時間(hh:mm)>を渡してドーナッツグラフを描画する
        internal int DrawChartDoughnut(string startTime, string endTime)
        {
            int intStartHh, intStartMm, intEndHh, intEndMm;

            if (GetApproximateIntHhAndMm(startTime, endTime, out intStartHh, out intStartMm, out intEndHh, out intEndMm) < 0) {
                return -1;
            }

            //グラフ描画
            chart1.Palette = ChartColorPalette.BrightPastel;
            chart1.ApplyPaletteColors();
            chart1.Series.Clear();  //グラフ初期化

            Series series = new Series();
            //series.Name = "Series";
            series.LegendText = "Task";
            series.ChartType = SeriesChartType.Doughnut;
            series["DoughnutRadius"] = "60";
            series["PieStartAngle"] = "270";

            DataPoint point = new DataPoint();
            point.LegendText = "Task 1";
            point.XValue = 0;
            point.YValues = new double[] { (intStartHh * 60 + intStartMm) }; // 円グラフに占める割合
            //point.BackSecondaryColor = System.Drawing.Color.DarkRed;
            //point.BackGradientStyle = GradientStyle.DiagonalLeft;
            series.Points.Add(point);

            point = new DataPoint();
            point.LegendText = "Task 2";
            point.XValue = 0;
            point.YValues = new double[] { (intEndHh * 60 + intEndMm) - (intStartHh * 60 + intStartMm) }; // 円グラフに占める割合
            series.Points.Add(point);

            point = new DataPoint();
            point.LegendText = "Task 3";
            point.XValue = 0;
            point.YValues = new double[] { 60 * 12 - (intEndHh * 60 + intEndMm) }; // 円グラフに占める割合
            point.Color = System.Drawing.Color.Silver;
            //point.Color = "BFBFBF"
            series.Points.Add(point);

#if NOTDEF
            point = new DataPoint();
            point.LegendText = "Task 4";
            point.XValue = 0;
            point.YValues = new double[] { 60 }; // 円グラフに占める割合
            series.Points.Add(point);

            point = new DataPoint();
            point.LegendText = "Task 5";
            point.XValue = 0;
            point.YValues = new double[] { 60 }; // 円グラフに占める割合
            series.Points.Add(point);

            point = new DataPoint();
            point.LegendText = "Task 6";
            point.XValue = 0;
            point.YValues = new double[] { 60 }; // 円グラフに占める割合
            series.Points.Add(point);

            point = new DataPoint();
            point.LegendText = "Task 7";
            point.XValue = 0;
            point.YValues = new double[] { 60 }; // 円グラフに占める割合
            series.Points.Add(point);

            point = new DataPoint();
            point.LegendText = "Task 8";
            point.XValue = 0;
            point.YValues = new double[] { 60 }; // 円グラフに占める割合
            series.Points.Add(point);

            point = new DataPoint();
            point.LegendText = "Task 9";
            point.XValue = 0;
            point.YValues = new double[] { 60 }; // 円グラフに占める割合
            series.Points.Add(point);
#endif

            chart1.Series.Add(series);

#if NOTDEF
            chart1.Legends.Add(new Legend("Legend2"));
            chart1.Legends["Legend2"].BackColor = Color.Transparent;
            chart1.Series["Series"].Legend = "Legend2";
            chart1.Series["Series"].Name = "Name";
            //chart1.Series.Legend = "Legend2";
            series.IsVisibleInLegend = true;
#endif

            return 0;
        }

        //開始時間と終了時間を15分刻みの時間に変換する
        internal int GetApproximateIntMm(string strMm, out int intMm)
        {
            //入力値のフォーマットチェック
            if (!Regex.IsMatch(strMm, @"[0-5][0-9]")) {
                intMm = -1;
                return -1;
            }

            if (int.Parse(strMm) >= 0 && int.Parse(strMm) < 8) {
                intMm = 0;
            }
            else if (int.Parse(strMm) >= 8 && int.Parse(strMm) < 15 + 8) {
                intMm = 15;
            }
            else if (int.Parse(strMm) >= 15 + 8 && int.Parse(strMm) < 30 + 8) {
                intMm = 30;
            }
            else if (int.Parse(strMm) >= 30 + 8 && int.Parse(strMm) < 45 + 8) {
                intMm = 45;
            }
            else if (int.Parse(strMm) >= 45 + 8 && int.Parse(strMm) < 60) {
                intMm = 60;
            }
            else {
                intMm = -1;
                return -1;
            }

            return 0;
        }

        private void DrawChartDoughnut()
        {
#if NOTDEF
/*
            //ToDo
            //How to read a text file reversely with iterator in C#
            //https://stackoverflow.com/questions/452902/how-to-read-a-text-file-reversely-with-iterator-in-c-sharp
            //
            //テキストファイルの最終行を読む
            //https://www.it-swarm.dev/ja/c%23/%E3%83%86%E3%82%AD%E3%82%B9%E3%83%88%E3%83%95%E3%82%A1%E3%82%A4%E3%83%AB%E3%81%AE%E6%9C%80%E7%B5%82%E8%A1%8C%E3%82%92%E8%AA%AD%E3%82%80/1068377715/
            //
            //Logの最終行を取得する
            string last = File.ReadLines(@strLogFilePath).Last();
            //Logのフォーマットを確認する
            string loggedString = last.Substring(last.IndexOf(",") + 1);
            //...Logのフォーマットが以下の場合、ドーナッツグラフの再描画を行う
            //...<日時>,(スタート|ストップ|リセット),<タスク文字列>:<開始時間(hh:mm)>-<終了時間(hh:mm)>
*/
            //最後のデータの終了時間からAM/PMを判断し、その結果を格納する
            //bool isAM = true;

            //データを後ろから1行読む
                //データがなかったらループを抜ける

                //開始時間と終了時間を15分刻みの時間に変換する

                //データの終了時間からAM/PMを判断する

                //AMかPMかの判断結果が最後のデータから判断したAM/PMと異なっていたらループを抜ける

                //データを描画対象として処理する
                //...開始時間が0:00/12:00の場合、0から描画を始める
                //...開始時間が0:00以外の場合、...する

                //開始時間と終了時間のAM/PMが異なっていたらループを抜ける

#else
            chart1.Series.Clear();  //グラフ初期化
            //chart1.Width = 200;
            //chart1.Height = 130;

            Series series = new Series();
            series.ChartType = SeriesChartType.Doughnut;
            series["PieStartAngle"] = "0";

            DataPoint point = new DataPoint();
            point.XValue = 0;
            point.YValues = new double[] { 30 }; // 円グラフに占める割合
            point.Color = System.Drawing.Color.Red;
            point.BackSecondaryColor = System.Drawing.Color.DarkRed;
            point.BackGradientStyle = GradientStyle.DiagonalLeft;
            series.Points.Add(point);

            point = new DataPoint();
            point.XValue = 0;
            point.YValues = new double[] { 20 }; // 円グラフに占める割合
            point.Color = System.Drawing.Color.Khaki;
            series.Points.Add(point);

            point = new DataPoint();
            point.XValue = 0;
            point.YValues = new double[] { 10 }; // 円グラフに占める割合
            point.Color = System.Drawing.Color.Blue;
            series.Points.Add(point);

            chart1.Series.Add(series);
#endif
        }

        private void ShowChartStackedColumn()
        {
            string[] legends = new string[] { "グラフ１", "グラフ２", "グラフ３" }; //凡例

            chart1.Series.Clear();  //グラフ初期化

            foreach (var item in legends)
            {
                chart1.Series.Add(item);    //グラフ追加
                //グラフの種類を指定（Columnは積み上げ縦棒グラフ）
                chart1.Series[item].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn;
//                chart1.Series[item].LegendText = item;  //凡例に表示するテキストを指定
            }  

//            string[] xValues = new string[] { "A", "B", "C", "D", "E" };    //X軸のデータ
//            int[,] yValues = new int[,] {{ 10, 20, 30, 40, 50 }, {20, 40, 60, 80, 100} };    //Y軸のデータ

            string[] xValues = new string[] { "予", "実" };    //X軸のデータ
            int[,] yValues = new int[,] {{10, 5}, {20, 10}, {30, 40} };    //Y軸のデータ

            for (int i = 0; i < xValues.Length; i++)
            {
                for (int j = 0; j < yValues.GetLength(0); j++)
                {
                    //グラフに追加するデータクラスを生成
                    System.Windows.Forms.DataVisualization.Charting.DataPoint dp = new System.Windows.Forms.DataVisualization.Charting.DataPoint();
                    dp.SetValueXY(xValues[i], yValues[j,i]);  //XとYの値を設定
                    dp.IsValueShownAsLabel = true;  //グラフに値を表示するように指定
                    chart1.Series[legends[j]].Points.Add(dp);   //グラフにデータ追加
                }
            }
        }

        //https://www.atmarkit.co.jp/fdotnet/dotnettips/1001aspchartpie/aspchartpie.html
        private void ShowChartPie()
        {
            chart1.Series.Clear();  //グラフ初期化
            //chart1.Width = 200;
            //chart1.Height = 130;

            Series series = new Series();
            series.ChartType = SeriesChartType.Pie;
            series["PieStartAngle"] = "270";

            DataPoint point = new DataPoint();
            point.XValue = 0;
            point.YValues = new double[] { 10 }; // 円グラフに占める割合
            point.Color = System.Drawing.Color.Red;
            point.BackSecondaryColor = System.Drawing.Color.DarkRed;
            point.BackGradientStyle = GradientStyle.DiagonalLeft;
            series.Points.Add(point);

            point = new DataPoint();
            point.XValue = 0;
            point.YValues = new double[] { 20 }; // 円グラフに占める割合
            point.Color = System.Drawing.Color.Khaki;
            series.Points.Add(point);

            point = new DataPoint();
            point.XValue = 0;
            point.YValues = new double[] { 30 }; // 円グラフに占める割合
            point.Color = System.Drawing.Color.Blue;
            series.Points.Add(point);

            chart1.Series.Add(series);

            //ChartArea area = new ChartArea();
            //area.AxisX.IsLabelAutoFit = true;
            //area.AxisY.IsLabelAutoFit = true;
            //chart1.ChartAreas.Add(area);
        }

        private void AddActivityLog(string taskAndTime)
        {
            //タスクと時間に分割する
            bool isFormatOK = true;

            //入力値のフォーマットチェック
            if (!Regex.IsMatch(taskAndTime, @"[:：](0[0-9]|1[0-9]|2[0-3])[:：][0-5][0-9]-(0[0-9]|1[0-9]|2[0-3])[:：][0-5][0-9]$") &&
                !Regex.IsMatch(taskAndTime, @"[:：](0[0-9]|1[0-9]|2[0-3])[:：][0-5][0-9]-24[:：]00$") &&
                !Regex.IsMatch(taskAndTime, @"[:：]24[:：]00-(0[0-9]|1[0-9]|2[0-3])[:：][0-5][0-9]$") &&
                !Regex.IsMatch(taskAndTime, @"[:：]24[:：]00-24[:：]00$")) {
                isFormatOK = false;
            }
            else {
                isFormatOK = true;
            }
            if (!isFormatOK &&
                !Regex.IsMatch(taskAndTime, @"[:：](0[0-9]|1[0-9]|2[0-3])[:：][0-5][0-9]-.*-(0[0-9]|1[0-9]|2[0-3])[:：][0-5][0-9]$") &&
                !Regex.IsMatch(taskAndTime, @"[:：](0[0-9]|1[0-9]|2[0-3])[:：][0-5][0-9]-.*-24[:：]00$") &&
                !Regex.IsMatch(taskAndTime, @"[:：]24[:：]00-.*-(0[0-9]|1[0-9]|2[0-3])[:：][0-5][0-9]$") &&
                !Regex.IsMatch(taskAndTime, @"[:：]24[:：]00-.*-24[:：]00$"))
            {
                isFormatOK = false;
            }
            else {
                isFormatOK = true;
            }
            if (!isFormatOK) {
                textBox1.Text = textBox1.Text + "\r\n" + taskAndTime;
            }
            else {
                Match matchedObj = Regex.Match(taskAndTime, @"(0[0-9]|1[0-9]|2[0-4])[:：][0-5][0-9]-.*-?(0[0-9]|1[0-9]|2[0-4])[:：][0-5][0-9]$");
                string taskString = "";
                RemoveTimeString(taskAndTime, out taskString);

                textBox1.Text = textBox1.Text + "\r\n" + matchedObj.Value + "  " + taskString;
            }

            btnSaveActivityLog_Click(null, null);
        }

        internal void ReadActivityLog()
        {
            if (!File.Exists(@strActivityLogFilePath))
            {
                StreamWriter sw = new StreamWriter(@strActivityLogFilePath, true, sjisEnc);
                sw.Close();
            }

            string textFromLogFile = File.ReadAllText(@strActivityLogFilePath, sjisEnc);
            textBox1.Text = textFromLogFile;
        }

        private void FormTimer_Load(object sender, EventArgs e)
        {
            // カラム数を指定
            dataGridView1.ColumnCount = 5;

            // カラム名を指定

#if NOTDEF
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
            DataGridViewColumn colF = CreateDataGridViewCheckBoxColumn("Stries", "機能実現", 60);
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

        private void btnShowGraphReviewedActivityLog_Click(object sender, EventArgs e)
        {
            DrawChartDoughnut(dataGridView1);
        }

        //https://www.atmarkit.co.jp/fdotnet/dotnettips/1001aspchartpie/aspchartpie.html
        //DataGridViewを渡してドーナッツグラフを描画する
        internal int DrawChartDoughnut(DataGridView dataGridView)
        {
            //グラフ描画
            chart1.Palette = ChartColorPalette.BrightPastel;
            chart1.ApplyPaletteColors();
            chart1.Series.Clear();  //グラフ初期化

            Series series = new Series();
            //series.Name = "Series";
            series.LegendText = "Task";
            series.ChartType = SeriesChartType.Doughnut;
            series["DoughnutRadius"] = "60";
            series["PieStartAngle"] = "270";

            //円グラフのデータを作成する。
            int taskIdx = 0;
            string crntStartTime;
            string crntEndTime;
            int intPrevEndHh = 0, intPrevEndMm = 0; //直前のタスクの終了時間を00:00に設定する。
            int intCrntStartHh = -1, intCrntStartMm = -1, intCrntEndHh = -1, intCrntEndMm = -1;
            DataPoint point;

            foreach (DataGridViewRow row in dataGridView1.Rows) {
                taskIdx++;

                //DGVから1行分のデータ取得
                if (dataGridView1[0, row.Index].Value == null || dataGridView1[1, row.Index].Value == null) {
                    break;
                }
                crntStartTime = dataGridView1[0, row.Index].Value.ToString();
                crntEndTime = dataGridView1[1, row.Index].Value.ToString();

                //startTime/endTime1からintStratHh,intStartMm,intEndHh,intEndMmを取得
                intCrntStartHh = intCrntStartMm = intCrntEndHh = intCrntEndMm = -1;
                if (GetApproximateIntHhAndMm(crntStartTime, crntEndTime, out intCrntStartHh, out intCrntStartMm,
                    out intCrntEndHh, out intCrntEndMm) < 0) {
                    return(-1);
                }

                //直前のタスクの終了時間から時間間隔が空いている場合
                if (intPrevEndHh != intCrntStartHh || intPrevEndMm != intCrntStartMm) {
                    //時間を埋める
                    point = new DataPoint();
                    point.LegendText = "Task " + taskIdx.ToString();
                    point.XValue = 0;
                    //point.YValues = new double[] { (intPrevEndHh * 60 + intStartMm) }; // 円グラフに占める割合
                    point.YValues = new double[] { (intCrntStartHh * 60 + intCrntStartMm) - (intPrevEndHh * 60 + intPrevEndMm) }; // 円グラフに占める割合
                    series.Points.Add(point);

                    taskIdx++;
                }
                //直前のタスクの終了時間から時間間隔が空いていない場合
                else {
                    //時間を埋めない
                }

                point = new DataPoint();
                point.LegendText = "Task " + taskIdx.ToString();
                point.XValue = 0;
                //point.YValues = new double[] { (intCrntStartHh * 60 + intCrntStartMm) }; // 円グラフに占める割合
                point.YValues = new double[] { (intCrntEndHh * 60 + intCrntEndMm) - (intCrntStartHh * 60 + intCrntStartMm) }; // 円グラフに占める割合
                series.Points.Add(point);

                //intPrevStartHh = intCrntStartHh;
                //intPrevStartMm = intCrntStartMm;
                intPrevEndHh = intCrntEndHh;
                intPrevEndMm = intCrntEndMm;
            }

            taskIdx++;

            //直前のタスクの終了時間から時間間隔が空いている場合
            if (intPrevEndHh != 0 || intPrevEndMm != 0) {
                point = new DataPoint();
                point.LegendText = "Task " + taskIdx.ToString();
                point.XValue = 0;
                point.YValues = new double[] { 60 * 12 - (intPrevEndHh * 60 + intPrevEndMm) }; // 円グラフに占める割合
                point.Color = System.Drawing.Color.Silver;
                //point.Color = "BFBFBF"
                series.Points.Add(point);
            }

            chart1.Series.Add(series);

            return 0;
        }

        private void btnSaveReviewedActivityLog_Click(object sender, EventArgs e)
        {
            //（1）テキスト・ファイルを開く、もしくは作成する
            StreamWriter sw = new StreamWriter(@strReviewedActivityLogFilePath, false, sjisEnc);

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

        internal void btnSaveActivityLog_Click(object sender, EventArgs e)
        {
            //（1）テキスト・ファイルを開く、もしくは作成する
            StreamWriter sw = new StreamWriter(@strActivityLogFilePath, false, sjisEnc);
            //（2）テキスト内容を書き込む
            //string outString = myTextBox1.Text;
            sw.Write(textBox1.Text);
            //（3）テキスト・ファイルを閉じる
            sw.Close();
        }

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


