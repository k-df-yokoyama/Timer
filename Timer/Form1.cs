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
        int currEndTime; // 終了時間（今、時間設定のTextBoxから取得した内容）
        int prevEndTime = 1500; // 終了時間
        int nowTime; // 経過時間
        bool isCounting; // 時間のカウント中か
        string strDesktopDirectory;
        string strHistoryFilePath, strLogFilePath;
        Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");
        //ArrayList al = new ArrayList();
        List<string> stringList = new List<string>();

        public FormTimer()
        {
            InitializeComponent();

            // 時間のカウント中かを記録する変数を初期化
            isCounting = false;
            // 残り時間を計算するため経過時間の変数を0で初期化
            nowTime = 0;
            strDesktopDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            strHistoryFilePath = ".\\timer_history.txt"; 
            strLogFilePath = strDesktopDirectory + "\\timer.log";

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
            ShowChartStackedColumn();

        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            // 時間のカウント中でない場合
            if (!isCounting)
            {
                // 時間設定のTextBoxの内容を終了時間の変数に取得
                if (!int.TryParse(textSetTime.Text, out currEndTime))
                {
                    currEndTime = 1;
                }
                if (prevEndTime != currEndTime)
                {
                    // リセットしたことを知らせる
                    MessageBox.Show("時刻設定の値が変更されました。リセットしてスタートします");
                    // 残り時間を計算するため経過時間の変数を0で初期化
                    nowTime = 0;
                    // 時間設定のTextBoxの内容を残り時間のTextBoxの内容に設定
                    textRemainingTime.Text = textSetTime.Text;
                }
                // 作業内容の履歴の保存
                //saveWorkHistory(textBox1.Text);
                saveWorkHistory(comboBox1.Text);
                // 終了時間の保存
                prevEndTime = currEndTime;
                // タイマースタート
                timerControl.Start();
                isCounting = true;
                // スタート／ストップボタンの表示をストップにする
                buttonStart.Text = "ストップ";
                writeLog("スタート," + comboBox1.Text);
                // 作業内容のテキストボックスを読み取り専用にする
                this.comboBox1.Enabled = false;
            }
            // 時間のカウント中の場合
            else
            {
                // タイマーストップ
                timerControl.Stop();
                isCounting = false;
                // スタート／ストップボタンの表示をスタート！にする
                buttonStart.Text = "スタート！";
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
            // <判定>設定時間になった？
            if (currEndTime == nowTime)
            {
                // 「Yes」の場合の処理
                // タイマーを止める
                //timerControl.Stop();
                // 終了時間になったことを知らせる
                MessageBox.Show("時間になりました！");
            }
            else
            {
                // 「No」の場合の処理
            }
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            // タイマーストップ
            DialogResult result = MessageBox.Show("リセットしてもよいですか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            //「いいえ」が選択された時
            if (result == DialogResult.No)
            {
                return;
            }

            // タイマーストップ
            timerControl.Stop();
            isCounting = false;
            // 残り時間を計算するため経過時間の変数を0で初期化
            nowTime = 0;
            // 時間設定のTextBoxの内容を残り時間のTextBoxの内容に設定
            textRemainingTime.Text = textSetTime.Text;
            // 前回停止時間のTextBoxの内容を現在の時間の内容に設定
            textLastStopTime1.Text = textLastStopTime2.Text;
            textLastStopTime2.Text = DateTime.Now.ToString();
            // スタート／ストップボタンの表示をスタート！にする
            buttonStart.Text = "スタート！";
            writeLog("リセット," + comboBox1.Text);
            // コンボボックスのリストに値を追加する
            // ...空白文字のみの場合、追加しない
            bool isWhiteSpaceOnly = !Regex.IsMatch(comboBox1.Text, "[^ 　]");
            int idx = comboBox1.Items.IndexOf(comboBox1.Text);
            if (!isWhiteSpaceOnly && idx == -1) { comboBox1.Items.Insert(0, comboBox1.Text); }
            // 作業内容のテキストボックスの読み取り専用を解除する
            this.comboBox1.Enabled = true;
            // リセットしたことを知らせる
            //MessageBox.Show("リセットしました！");
        }

        private void textSetTime_TextChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("リセットしました！");
            //buttonReset_Click(sender, e);
        }

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
        
        private void writeLog(string outString)
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

        private void button2_Click(object sender, EventArgs e)
        {
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

        private void buttonShowGraph_Click(object sender, EventArgs e)
        {
            //ShowChartPie();
            ShowChartStackedColumn();
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
            c.Location = new System.Drawing.Point(24, 32);
            c.Size = new System.Drawing.Size(120, 30);

            panel1.Controls.Add(c);
        }

/*
        private void ShowChartPie()
        {
            chart1.Series.Clear();  //グラフ初期化
            chart1.Width = 200;
            chart1.Height = 130;

            Series series = new Series();
            series.ChartType = SeriesChartType.Pie;
            series["PieStartAngle"] = "270";

            DataPoint point = new DataPoint();
            point.XValue = 0;
            point.YValues = new double[] { 65 };
            point.Color = System.Drawing.Color.Red;
            point.BackSecondaryColor = System.Drawing.Color.DarkRed;
            point.BackGradientStyle = GradientStyle.DiagonalLeft;
            series.Points.Add(point);

            point = new DataPoint();
            point.XValue = 0;
            point.YValues = new double[] { 35 };
            point.Color = System.Drawing.Color.Khaki;

            series.Points.Add(point);

            chart1.Series.Add(series);

            ChartArea area = new ChartArea();
            area.AxisX.IsLabelAutoFit = true;
            area.AxisY.IsLabelAutoFit = true;
            chart1.ChartAreas.Add(area);
        }
*/

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


