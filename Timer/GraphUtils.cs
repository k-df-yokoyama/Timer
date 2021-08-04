using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Timer
{
    static class GraphUtils
    {
        internal static Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");

        /// <summary>
        /// https://www.atmarkit.co.jp/fdotnet/dotnettips/1001aspchartpie/aspchartpie.html
        /// [開始時間 hh:mm]と[終了時間 hh:mm]を渡してドーナッツグラフを描画する
        /// <param name="chart">グラフ</param> 
        /// <param name="startTime">開始時間</param> 
        /// <param name="endTime">終了時間</param>
        /// </summary>
        internal static int DrawChartDoughnut(Chart chart, string startTime, string endTime)
        {
            int intStartHh, intStartMm, intEndHh, intEndMm;

            if (Utils.GetApproximateIntHhAndMm(startTime, endTime, out intStartHh, out intStartMm, out intEndHh, out intEndMm) < 0)
            {
                return -1;
            }

            //グラフ描画
            chart.Palette = ChartColorPalette.BrightPastel;
            chart.ApplyPaletteColors();
            chart.Series.Clear();  //グラフ初期化

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

            chart.Series.Add(series);

#if NOTDEF
            chart.Legends.Add(new Legend("Legend2"));
            chart.Legends["Legend2"].BackColor = Color.Transparent;
            chart.Series["Series"].Legend = "Legend2";
            chart.Series["Series"].Name = "Name";
            //chart.Series.Legend = "Legend2";
            series.IsVisibleInLegend = true;
#endif

            return 0;
        }

        /// <summary>
        /// ドーナッツグラフを描画する
        /// <param name="chart">グラフ</param> 
        /// </summary>
        internal static void DrawChartDoughnut(Chart chart)
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
            chart.Series.Clear();  //グラフ初期化
            //chart.Width = 200;
            //chart.Height = 130;

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

            chart.Series.Add(series);
#endif
        }


        /// <summary>
        /// 積み上げ縦棒グラフを描画する
        /// <param name="chart">グラフ</param> 
        /// </summary>
        internal static void ShowChartStackedColumn(Chart chart)
        {
            string[] legends = new string[] { "グラフ１", "グラフ２", "グラフ３" }; //凡例

            chart.Series.Clear();  //グラフ初期化

            foreach (var item in legends)
            {
                chart.Series.Add(item);    //グラフ追加
                //グラフの種類を指定（StackedColumnは積み上げ縦棒グラフ）
                chart.Series[item].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn;
                //chart.Series[item].LegendText = item;  //凡例に表示するテキストを指定
            }

            //string[] xValues = new string[] { "A", "B", "C", "D", "E" };    //X軸のデータ
            //int[,] yValues = new int[,] {{ 10, 20, 30, 40, 50 }, {20, 40, 60, 80, 100} };    //Y軸のデータ

            string[] xValues = new string[] { "予", "実" };    //X軸のデータ
            int[,] yValues = new int[,] { { 10, 5 }, { 20, 10 }, { 30, 40 } };    //Y軸のデータ

            for (int i = 0; i < xValues.Length; i++)
            {
                for (int j = 0; j < yValues.GetLength(0); j++)
                {
                    //グラフに追加するデータクラスを生成
                    System.Windows.Forms.DataVisualization.Charting.DataPoint dp =
                        new System.Windows.Forms.DataVisualization.Charting.DataPoint();
                    dp.SetValueXY(xValues[i], yValues[j, i]);  //XとYの値を設定
                    dp.IsValueShownAsLabel = true;  //グラフに値を表示するように指定
                    chart.Series[legends[j]].Points.Add(dp);   //グラフにデータ追加
                }
            }
        }

        /// <summary>
        /// 棒グラフを描画する
        /// <param name="chart1">グラフ</param>
        /// <param name="axisLabel">軸ラベル</param> 
        /// <param name="startTime">開始時間</param> 
        /// <param name="endTime">終了時間</param>
        /// </summary>
        internal static int ShowChartColumn(Chart chart1, string axisLabel, string startTime, string endTime)
        {
            int intStartHh, intStartMm, intEndHh, intEndMm;

            if (Utils.GetApproximateIntHhAndMm(startTime, endTime, out intStartHh, out intStartMm, out intEndHh, out intEndMm) < 0)
            {
                return -1;
            }
            if (intStartHh == intEndHh && intStartMm == intEndMm)
            {
                return 0;
            }

            chart1.ChartAreas.Clear(); //ChartArea初期化
            chart1.Series.Clear();     //Series初期化

            // ChartにChartAreaを追加
            string chart_area1 = "Area1";
            chart1.ChartAreas.Add(new ChartArea(chart_area1));

            // ChartにSeriesを追加
            //string legend0 = "Plan";
            string legend1 = "Actual";
            chart1.Series.Add(legend1);

            // ChartTypeを設定
            chart1.Series[legend1].ChartType = SeriesChartType.Column;
            //System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;

            //double[] y_values = new double[5] { 1.0, 1.2, 0.8, 1.8, 0.2 };
            double[] y_values = new double[] { (intEndHh + intEndMm / 60.0) - (intStartHh + intStartMm / 60.0) };

            for (int i = 0; i < y_values.Length; i++)
            {
                chart1.Series[legend1].Points.AddY(y_values[i]);
                chart1.Series[legend1].Points[i].AxisLabel = axisLabel;
            }
            return 0;
        }


        /// <summary>
        /// 円グラフを描画する
        /// <param name="chart">グラフ</param> 
        /// </summary>
        //https://www.atmarkit.co.jp/fdotnet/dotnettips/1001aspchartpie/aspchartpie.html
        internal static void ShowChartPie(Chart chart)
        {
            chart.Series.Clear();  //グラフ初期化
            //chart.Width = 200;
            //chart.Height = 130;

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

            chart.Series.Add(series);

            //ChartArea area = new ChartArea();
            //area.AxisX.IsLabelAutoFit = true;
            //area.AxisY.IsLabelAutoFit = true;
            //chart.ChartAreas.Add(area);
        }

        /// <summary>
        /// https://www.atmarkit.co.jp/fdotnet/dotnettips/1001aspchartpie/aspchartpie.html
        /// DataGridViewを渡してドーナッツグラフを描画する
        /// <param name="chart">グラフ</param> 
        /// <param name="dataGridView"></param>
        /// </summary>
        internal static int DrawChartDoughnut(Chart chart, DataGridView dataGridView)
        {
            //グラフ描画
            chart.Palette = ChartColorPalette.BrightPastel;
            chart.ApplyPaletteColors();
            chart.Series.Clear();  //グラフ初期化

            Series series = new Series();
            //series.Name = "Series";
#if SHOW_LEGEND
            series.LegendText = "Task";
#endif
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

            string lastValidStartTime = "";
            string lastValidEndTime = "";
            //DrawRange drawRange;

#if NOTDEF
            foreach (DataGridViewRow row in dataGridView.Rows) {
                //DGVから1行分のデータ取得
                if (dataGridView[0, row.Index].Value == null || dataGridView[1, row.Index].Value == null) {
                    break;
                }
                lastValidEndTime = dataGridView[1, row.Index].Value.ToString();
            }
#else
            //最後の最も妥当な開始・終了時間を取得
            int idx;
            for (idx = dataGridView.Rows.Count - 1; idx >= 0; idx--)
            {
                if (idx == dataGridView.NewRowIndex)
                {
                    continue;
                }
                if (dataGridView[0, idx].Value == null || dataGridView[1, idx].Value == null)
                {
                    continue;
                }
                lastValidStartTime = dataGridView[0, idx].Value.ToString();
                lastValidEndTime = dataGridView[1, idx].Value.ToString();
                break;
            }
#endif

#if STILL_NOT_USED
            //最後の妥当な開始時間から描画範囲を特定する
//            if (GetDrawRange(lastValidStartTime, lastValidEndTime, out drawRange) < 0) {
            if (GetDrawRange(lastValidStartTime, out drawRange) < 0) {
                return(-1);
            }
#endif

#if DEBUG
            SaveChartInput("DrawChartDoughnut()");
#endif
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                taskIdx++;

                //DGVから1行分のデータ取得
                if (dataGridView[0, row.Index].Value == null || dataGridView[1, row.Index].Value == null)
                {
                    break;
                }
                crntStartTime = dataGridView[0, row.Index].Value.ToString();
                crntEndTime = dataGridView[1, row.Index].Value.ToString();

                //startTime/endTime1からintStratHh,intStartMm,intEndHh,intEndMmを取得
                intCrntStartHh = intCrntStartMm = intCrntEndHh = intCrntEndMm = -1;
                if (Utils.GetApproximateIntHhAndMm2(crntStartTime, crntEndTime, out intCrntStartHh, out intCrntStartMm,
                    out intCrntEndHh, out intCrntEndMm) < 0)
                {
                    return (-1);
                }

                //直前のタスクの終了時間から時間間隔が空いている場合
                if (intPrevEndHh != intCrntStartHh || intPrevEndMm != intCrntStartMm)
                {
                    //時間を埋める
                    point = new DataPoint();
#if SHOW_LEGEND
                    point.LegendText = "Task " + taskIdx.ToString();
#endif
                    //point.LegendText = dataGridView[2, row.Index].Value.ToString();
                    point.XValue = 0;
                    //point.YValues = new double[] { (intPrevEndHh * 60 + intStartMm) }; // 円グラフに占める割合
                    point.YValues = new double[] { (intCrntStartHh * 60 + intCrntStartMm) - (intPrevEndHh * 60 + intPrevEndMm) }; // 円グラフに占める割合
                    series.Points.Add(point);
#if DEBUG
                    SaveChartInput("[blank]");
                    SaveChartInput(intPrevEndHh.ToString());
                    SaveChartInput(intPrevEndMm.ToString());
                    SaveChartInput(crntStartTime);
                    SaveChartInput(point.YValues[0].ToString());
#endif

                    taskIdx++;
                }
                //直前のタスクの終了時間から時間間隔が空いていない場合
                else
                {
                    //時間を埋めない
                }

                point = new DataPoint();
                //point.LegendText = "Task " + taskIdx.ToString();
                point.LegendText = dataGridView[2, row.Index].Value.ToString();
                point.XValue = 0;
                //point.YValues = new double[] { (intCrntStartHh * 60 + intCrntStartMm) }; // 円グラフに占める割合
                point.YValues = new double[] { (intCrntEndHh * 60 + intCrntEndMm) - (intCrntStartHh * 60 + intCrntStartMm) }; // 円グラフに占める割合
                series.Points.Add(point);
#if DEBUG
                SaveChartInput("[task]");
                SaveChartInput(crntStartTime);
                SaveChartInput(crntEndTime);
                SaveChartInput(point.YValues[0].ToString());
#endif

                //intPrevStartHh = intCrntStartHh;
                //intPrevStartMm = intCrntStartMm;
                intPrevEndHh = intCrntEndHh;
                intPrevEndMm = intCrntEndMm;
            }

            taskIdx++;

            //直前のタスクの終了時間から時間間隔が空いている場合
            if (intPrevEndHh != 0 || intPrevEndMm != 0)
            {
                point = new DataPoint();
#if SHOW_LEGEND
                point.LegendText = "Task " + taskIdx.ToString();
#endif
                point.XValue = 0;
                point.YValues = new double[] { 60 * 24 - (intPrevEndHh * 60 + intPrevEndMm) }; // 円グラフに占める割合
                point.Color = System.Drawing.Color.Silver;
                //point.Color = "BFBFBF"
                series.Points.Add(point);
#if DEBUG
                SaveChartInput("[empty]");
                SaveChartInput(intPrevEndHh.ToString());
                SaveChartInput(intPrevEndMm.ToString());
                SaveChartInput(point.YValues[0].ToString());
#endif
            }

            chart.Series.Add(series);

            return 0;
        }

        /// <summary>
        /// 入力文字列をChartInputファイルに追記する。
        /// <param name="inString">入力文字列</param>
        /// </summary>
        private static void SaveChartInput(string inString)
        {
            string strChartInputFilePath;
            strChartInputFilePath = ".\\timer_chartinput.txt";

            //（1）テキスト・ファイルを開く、もしくは作成する
            StreamWriter sw = new StreamWriter(@strChartInputFilePath, true, sjisEnc);
            //（2）テキスト内容を書き込む
            sw.WriteLine(inString);
            //（3）テキスト・ファイルを閉じる
            sw.Close();
        }
    }
}
