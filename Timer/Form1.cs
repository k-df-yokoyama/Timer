﻿using System;
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
                    treeView1.Nodes.Add(s); 
                }
            }


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
    }
}
