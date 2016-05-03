using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Timer
{
    public partial class FormShowLog : Form
    {
        string strDesktopDirectory;
        string strLogFilePath;
        Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");

        public FormShowLog()
        {
            InitializeComponent();

            strDesktopDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            strLogFilePath = strDesktopDirectory + "\\timer.log";

            // 履歴ファイルから値を読み取る
            readLog();
        }

        private void readLog()
        {
/*
            string line = "";

            // ファイルがあるか確認する

            // ファイルがない場合→終了する

            // ファイルがある場合
            try
            {
                using (StreamReader sr = new StreamReader(
                    @strLogFilePath, Encoding.GetEncoding("Shift_JIS")))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        //al.Add(line);
                        stringList.Add(line);
                    }
                }
            }
            catch (Exception e)
            {
                // 何もしない
            }

            //for (int i = 0; i < al.Count; i++) {
            //MessageBox.Show(al[i]);
            //}
*/

            //（1）テキスト・ファイルを開く
            StreamReader sr = new StreamReader(@strLogFilePath, sjisEnc);
            //（2）テキスト内容を読み込む
            //string text = sr.ReadToEnd();
            textBox1.Text = sr.ReadToEnd();
            //（3）テキスト・ファイルを閉じる
            sr.Close();
        }

/*
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
*/
    }
}
