using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Timer
{
    static class Utils
    {
        /// <summary>
        /// inString から時間部分を削除した文字列を作成する。
        /// inString、outStringのフォーマットは以下。
        /// IN:  "Task:00:00-"
        /// IN:  "Task:00:00-00:15"
        /// IN:  "Task:00:00-00:15-"
        /// IN:  "Task:00:00-00:15-00:30"
        /// OUT: "Task"
        /// <param name="inString">入力文字列</param>
        /// <param name="outString">出力文字列</param>
        /// </summary>
        internal static void RemoveTimeString(string inString, out string outString)
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
        
        /// <summary>
        /// 入力値から[開始時間 hh:mm]と[終了時間 hh:mm]を取得する
        /// 処理できる時間部分のフォーマットと取得結果は以下。
        /// "Start,Task:00:00-"
        ///   return -1
        /// "Start,Task:00:00-00:15"
        ///   開始時間：00:00
        ///   終了時間：00:15
        /// "Start,Task:00:00-00:15-"
        ///   return -1
        /// "Start,Task:00:00-00:15-00:30"
        ///   開始時間：00:00
        ///   終了時間：00:30
        /// <param name="taskAndTime">タスクと時間</param>
        /// <param name="startTime">開始時間</param> 
        /// <param name="endTime">終了時間</param>
        /// </summary>
        internal static int GetStartAndEndTimeFromTrailing(string taskAndTime, out string startTime, out string endTime)
        {
            bool isFormatOK = true;

            //入力値のフォーマットチェック
            if (Regex.IsMatch(taskAndTime, @"[:：](0[0-9]|1[0-9]|2[0-3])[:：][0-5][0-9]-(0[0-9]|1[0-9]|2[0-3])[:：][0-5][0-9]$") ||
                Regex.IsMatch(taskAndTime, @"[:：](0[0-9]|1[0-9]|2[0-3])[:：][0-5][0-9]-24[:：]00$") ||
                Regex.IsMatch(taskAndTime, @"[:：]24[:：]00-(0[0-9]|1[0-9]|2[0-3])[:：][0-5][0-9]$") ||
                Regex.IsMatch(taskAndTime, @"[:：]24[:：]00-24[:：]00$")) {
                isFormatOK = true;
            }
            else {
                isFormatOK = false;
            }
            if (isFormatOK ||
                Regex.IsMatch(taskAndTime, @"[:：](0[0-9]|1[0-9]|2[0-3])[:：][0-5][0-9]-.*-(0[0-9]|1[0-9]|2[0-3])[:：][0-5][0-9]$") ||
                Regex.IsMatch(taskAndTime, @"[:：](0[0-9]|1[0-9]|2[0-3])[:：][0-5][0-9]-.*-24[:：]00$") ||
                Regex.IsMatch(taskAndTime, @"[:：]24[:：]00-.*-(0[0-9]|1[0-9]|2[0-3])[:：][0-5][0-9]$") ||
                Regex.IsMatch(taskAndTime, @"[:：]24[:：]00-.*-24[:：]00$"))
            {
                isFormatOK = true;
            }
            else {
                isFormatOK = false;
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

        /// <summary>
        /// 入力値から[開始時間 hh:mm]、[終了時間 hh:mm]、[タスク名]を取得する
        /// 処理できる時間部分のフォーマットと取得結果は以下。
        /// ""00:00-  Task"
        ///   return -1
        /// "00:00-00:15  Task"
        ///   開始時間：00:00
        ///   終了時間：00:15
        /// "00:00-00:15-  Task"
        ///   return -1
        /// "00:00-00:15-00:30  Task"
        ///   開始時間：00:00
        ///   終了時間：00:30
        /// <param name="taskAndTime">タスクと時間</param>
        /// <param name="startTime">開始時間</param> 
        /// <param name="endTime">終了時間</param>
        /// <param name="task">タスク</param>
        /// </summary>
        internal static int GetStartAndEndTimeAndTaskFromHeading(string taskAndTime, out string startTime, out string endTime, out string task)
        {
            bool isFormatOK = true;

            //入力値のフォーマットチェック
            if (Regex.IsMatch(taskAndTime, @"^(0[0-9]|1[0-9]|2[0-3])[:：][0-5][0-9]-(0[0-9]|1[0-9]|2[0-3])[:：][0-5][0-9]  ") ||
                Regex.IsMatch(taskAndTime, @"^(0[0-9]|1[0-9]|2[0-3])[:：][0-5][0-9]-24[:：]00  ") ||
                Regex.IsMatch(taskAndTime, @"^24[:：]00-(0[0-9]|1[0-9]|2[0-3])[:：][0-5][0-9]  ") ||
                Regex.IsMatch(taskAndTime, @"^24[:：]00-24[:：]00  ")) {
                isFormatOK = true;
            }
            else {
                isFormatOK = false;
            }
            if (isFormatOK ||
                Regex.IsMatch(taskAndTime, @"^(0[0-9]|1[0-9]|2[0-3])[:：][0-5][0-9]-.*-(0[0-9]|1[0-9]|2[0-3])[:：][0-5][0-9]  ") ||
                Regex.IsMatch(taskAndTime, @"^(0[0-9]|1[0-9]|2[0-3])[:：][0-5][0-9]-.*-24[:：]00  ") ||
                Regex.IsMatch(taskAndTime, @"^24[:：]00-.*-(0[0-9]|1[0-9]|2[0-3])[:：][0-5][0-9]  ") ||
                Regex.IsMatch(taskAndTime, @"^24[:：]00-.*-24[:：]00  "))
            {
                isFormatOK = true;
            }
            else {
                isFormatOK = false;
            }

            if (!isFormatOK) {
                startTime = null;
                endTime = null;
                task = null;
                return -1;
            }

            Match matchedObj = Regex.Match(taskAndTime, @"^(0[0-9]|1[0-9]|2[0-4])[:：][0-5][0-9]-.*-?(0[0-9]|1[0-9]|2[0-4])[:：][0-5][0-9]  ");

            Match startTimeObj = Regex.Match(matchedObj.Value, @"^(0[0-9]|1[0-9]|2[0-4])[:：][0-5][0-9]");
            Match endTimeObjWithDelimiter = Regex.Match(matchedObj.Value, @"(0[0-9]|1[0-9]|2[0-4])[:：][0-5][0-9]  ");
            Match endTimeObj = Regex.Match(endTimeObjWithDelimiter.Value, @"^(0[0-9]|1[0-9]|2[0-4])[:：][0-5][0-9]");

            startTime = startTimeObj.Value;
            endTime = endTimeObj.Value;
            task = Regex.Replace(taskAndTime, @"^(0[0-9]|1[0-9]|2[0-4])[:：][0-5][0-9]-.*-?(0[0-9]|1[0-9]|2[0-4])[:：][0-5][0-9]  ", "");

            return 0;
        }

        /// <summary>
        /// 開始時間と終了時間を15分刻みの時間に変換する
        /// <param name="strMm">開始時間</param> 
        /// <param name="intMm">終了時間</param>
        /// </summary>
        internal static int GetApproximateIntMm(string strMm, out int intMm)
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
    }
}
