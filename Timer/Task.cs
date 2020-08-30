using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Timer
{
    class Task
    {
        public string startTime, endTime;
        public string taskName;

/*
        string input, output;
        string howToImprove;
  
        bool isStories;
        bool isTaxes;
        bool isSpikes;
        bool isTechnicalDebt;
        bool isBreak;
        bool isSkillUp;
        bool isContribution;
        bool isProactive;
        bool isGroup;
        bool isTime;
*/

        public Task(string taskAndTime)
        {
            bool isFormatOK = true;
            startTime = "";
            endTime = "";
            taskName = "";

            //入力値のフォーマットチェック
            if (Regex.IsMatch(taskAndTime, @"[:：](0[0-9]|1[0-9]|2[0-3])[:：][0-5][0-9]-(0[0-9]|1[0-9]|2[0-3])[:：][0-5][0-9]$") ||
                Regex.IsMatch(taskAndTime, @"[:：](0[0-9]|1[0-9]|2[0-3])[:：][0-5][0-9]-24[:：]00$") ||
                Regex.IsMatch(taskAndTime, @"[:：]24[:：]00-(0[0-9]|1[0-9]|2[0-3])[:：][0-5][0-9]$") ||
                Regex.IsMatch(taskAndTime, @"[:：]24[:：]00-24[:：]00$"))
            {
                isFormatOK = true;
            }
            else
            {
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
            else
            {
                isFormatOK = false;
            }

            if (!isFormatOK)
            {
                //そのままActivityLogのテキストボックス(textBox1)に追加
                taskName = taskAndTime;
            }
            else
            {
                Utils.GetStartAndEndTimeFromTrailing(taskAndTime, out startTime, out endTime);

                //フォーマットを変更してActivityLogのテキストボックス(textBox1)に追加
                //taskAndTimeから時間文字列を取得
                Match timeString = Regex.Match(taskAndTime, @"(0[0-9]|1[0-9]|2[0-4])[:：][0-5][0-9]-.*-?(0[0-9]|1[0-9]|2[0-4])[:：][0-5][0-9]$");

                string taskString;
                Utils.RemoveTimeString(taskAndTime, out taskString);

                taskName =  taskString;
            }
        }


    }
}
