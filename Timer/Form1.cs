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
#if DEBUG
        public string strHistoryFilePath, strLogFilePath;
#else
        string strHistoryFilePath, strLogFilePath;
#endif
        Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");
        //ArrayList al = new ArrayList();
        List<string> stringList = new List<string>();
        bool isPieChart; // true=PieChart、false=StackedColumn

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

            // 変数の初期化（変数：時間のカウント中かの判定）
            isPieChart = true;

            // 履歴ファイルから値を読み取る
            readWorkHistory();

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

            //ShowChartPie();
            //ShowChartStackedColumn();

            //Hide developping controls
            btnAddNode.Visible = false;
            btnRemoveNode.Visible = false;
            label8.Visible = false;
            label9.Visible = false;
            label10.Visible = false;
            label11.Visible = false;
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
                //saveWorkHistory(textBox1.Text);
                saveWorkHistory(comboBox1.Text);
                // タスク終了までの予定時間の格納
                prevEndTime = currEndTime;
                // タイマースタート
                timerControl.Start();
                isTimeCounting = true;
                // スタート／ストップボタンの表示をストップにする
                btnStart.Text = "ストップ";
                writeLog("スタート," + comboBox1.Text);
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
                writeLog("ストップ," + comboBox1.Text);
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
            writeLog("リセット," + comboBox1.Text);
            // (7)コンボボックスのリストに値を追加する
            // ...空白文字のみの場合、追加しない
            bool isWhiteSpaceOnly = !Regex.IsMatch(comboBox1.Text, "[^ 　]");
            int idx = comboBox1.Items.IndexOf(comboBox1.Text);
            if (!isWhiteSpaceOnly && idx == -1) { comboBox1.Items.Insert(0, comboBox1.Text); }
            // (8)作業内容のテキストボックスの読み取り専用を解除する
            this.comboBox1.Enabled = true;
            // (9)リセットしたことを知らせる
            //MessageBox.Show("リセットしました！");
        }

        private void textSetTime_TextChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("リセットしました！");
            //buttonReset_Click(sender, e);
        }

        //Todo: UnitTest候補
        private void readWorkHistory()
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
        
        // ToDo: outStrigのフォーマットを記載
        private void saveWorkHistory(string outString)
        {
            //（1）テキスト・ファイルを開く、もしくは作成する
            StreamWriter sw = new StreamWriter(@strHistoryFilePath, true, sjisEnc);
            //（2）テキスト内容を書き込む
            //sw.WriteLine(DateTime.Now + "," + outString);
            sw.WriteLine(outString);
            //（3）テキスト・ファイルを閉じる
            sw.Close();
        }
        
        // Output Log
        // Log format: datetime,(スタート|ストップ|リセット),task[:starttime-endtime]
#if DEBUG
        public void writeLog(string outString)
#else
        private void writeLog(string outString)
#endif
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
        private void button2_Click(object sender, EventArgs e)
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


