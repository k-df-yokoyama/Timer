using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Timer
{
    class TaskList
    {
        public List<Task> tasks;
        private static readonly string taskListFname = "timer_task_list.txt";
        private static readonly string strActivityLogFilePath = ".\\" + taskListFname;
        internal Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");

        public TaskList()
        {
            tasks = new List<Task>();
        }

        public void Add(Task task)
        {
            tasks.Add(task);
        }

        /// <summary>
        /// ActivityLogファイルに上書き保存する。
        /// </summary>
        public void SaveTaskList()
        {
            // テキスト・ファイルを開く、もしくは作成する
            StreamWriter sw = new StreamWriter(@strActivityLogFilePath, false, sjisEnc);
            // テキスト内容を書き込む
            //sw.Write(textValue);

            foreach (Task t in tasks)
            {
                sw.Write(t.taskName);
                sw.Write("\r\n");
            }

            // テキスト・ファイルを閉じる
            sw.Close();
        }
    }
}
