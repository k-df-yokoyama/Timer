﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Timer;
using System.IO;
using System.Linq;

namespace Timer.Tests
{
    //MSTestでprivateメソッドをテストする
    //https://www.gesource.jp/weblog/?p=7742
    //C#でprivateメソッドをテストする時の便利な書き方(実装の都合をテストコードから排除)
    //https://qiita.com/kojimadev/items/92c1ac05a6ec4afb35a3

    [TestClass]
    public class Form1Test
    {
        [TestMethod]
        public void TestMethod1()
        {
            FormTimer formTimer = new FormTimer();
            //var pbObj = new PrivateObject(formTimer);
            //formTimer.isCounting = true;
            formTimer.buttonStart_Click(null, null);
            Assert.IsTrue(true);
            //Assert.Fail("failed");
        }

        [TestMethod]
        public void Test_writeLog()
        {
            FormTimer formTimer = new FormTimer();
            var pbFormTimer = new PrivateObject(formTimer);
            string outString = "Start,Task:00:00-00:15";
            pbFormTimer.Invoke("WriteLog", outString);
            //ログファイルを開いて書き込まれていることを確認
            var strLogFilePath = (string)pbFormTimer.GetField("strLogFilePath");
            string last = File.ReadLines(@strLogFilePath).Last();
            string loggedString = last.Substring(last.IndexOf(",") + 1);
            Assert.IsTrue(outString.Equals(loggedString));
        }

        [TestMethod]
        public void Test_removeTimeString()
        {
            FormTimer formTimer = new FormTimer();
            string inString = "Start,Task:00:00-00:15-00:30";
            string outString = "";
            formTimer.RemoveTimeString(inString, out outString);

            Assert.IsTrue(outString.Equals("Start,Task"));

            inString = "Start,Task:00:00-00:15-";
            outString = "";
            formTimer.RemoveTimeString(inString, out outString);

            Assert.IsTrue(outString.Equals("Start,Task"));

            inString = "Start,Task:00:00-00:15";
            outString = "";
            formTimer.RemoveTimeString(inString, out outString);

            Assert.IsTrue(outString.Equals("Start,Task"));

            inString = "Start,Task:00:00-";
            outString = "";
            formTimer.RemoveTimeString(inString, out outString);

            Assert.IsTrue(outString.Equals("Start,Task"));
        }

        [TestMethod]
        public void Test_getStartAndEndTime()
        {
            int ret;
            string startTime = "00:00", endTime = "00:00";

            FormTimer formTimer = new FormTimer();
            ret = formTimer.GetStartAndEndTime("Task:03:00-05:00", out startTime, out endTime);
            Assert.IsTrue(ret == 0);
            Assert.IsTrue(startTime.Equals("03:00"));
            Assert.IsTrue(endTime.Equals("05:00"));

            ret = formTimer.GetStartAndEndTime("Task:05:00-03:00", out startTime, out endTime);
            Assert.IsTrue(ret == 0);
            Assert.IsTrue(startTime.Equals("05:00"));
            Assert.IsTrue(endTime.Equals("03:00"));

            ret = formTimer.GetStartAndEndTime("Task:0:00-05:00", out startTime, out endTime);
            Assert.IsTrue(ret == -1);

            ret = formTimer.GetStartAndEndTime("Task:00:00-5:00", out startTime, out endTime);
            Assert.IsTrue(ret == -1);

            ret = formTimer.GetStartAndEndTime("Task:00:60-05:00", out startTime, out endTime);
            Assert.IsTrue(ret == -1);

            ret = formTimer.GetStartAndEndTime("Task:00:00-05:60", out startTime, out endTime);
            Assert.IsTrue(ret == -1);

            ret = formTimer.GetStartAndEndTime("Task:00:00-12:00", out startTime, out endTime);
            Assert.IsTrue(ret == 0);
            Assert.IsTrue(startTime.Equals("00:00"));
            Assert.IsTrue(endTime.Equals("12:00"));

            ret = formTimer.GetStartAndEndTime("Task:12:00-24:00", out startTime, out endTime);
            Assert.IsTrue(ret == 0);
            Assert.IsTrue(startTime.Equals("12:00"));
            Assert.IsTrue(endTime.Equals("24:00"));

            ret = formTimer.GetStartAndEndTime("Task:12:00-24:01", out startTime, out endTime);
            Assert.IsTrue(ret == -1);

            ret = formTimer.GetStartAndEndTime("Task:12:00-25:00", out startTime, out endTime);
            Assert.IsTrue(ret == -1);

            ret = formTimer.GetStartAndEndTime("Task:03:00-05:00a", out startTime, out endTime);
            Assert.IsTrue(ret == -1);

            ret = formTimer.GetStartAndEndTime("Task:03:00", out startTime, out endTime);
            Assert.IsTrue(ret == -1);

            ret = formTimer.GetStartAndEndTime("Task:03:00-", out startTime, out endTime);
            Assert.IsTrue(ret == -1);

            ret = formTimer.GetStartAndEndTime("Task:01:00-02:00-", out startTime, out endTime);
            Assert.IsTrue(ret == -1);

            ret = formTimer.GetStartAndEndTime("Task:01:00-02:00-03:00", out startTime, out endTime);
            Assert.IsTrue(ret == 0);
            Assert.IsTrue(startTime.Equals("01:00"));
            Assert.IsTrue(endTime.Equals("03:00"));
        }

        [TestMethod]
        public void Test_drawChartDoughnut()
        {
            int ret;

            FormTimer formTimer = new FormTimer();
            ret = formTimer.DrawChartDoughnut("00:00", "10:00");
            Assert.IsTrue(ret == 0);

            ret = formTimer.DrawChartDoughnut("03:00", "12:00");
            Assert.IsTrue(ret == 0);

            ret = formTimer.DrawChartDoughnut("12:00", "14:00");
            Assert.IsTrue(ret == 0);

            ret = formTimer.DrawChartDoughnut("0:00", "03:00");
            Assert.IsTrue(ret == -1);

            ret = formTimer.DrawChartDoughnut("00:0", "03:00");
            Assert.IsTrue(ret == -1);

            ret = formTimer.DrawChartDoughnut("aa:00", "03:00");
            Assert.IsTrue(ret == -1);

            ret = formTimer.DrawChartDoughnut("00:00", "00:00");
            Assert.IsTrue(ret == 0);

            ret = formTimer.DrawChartDoughnut("02:00", "01:00");
            Assert.IsTrue(ret == 0);
        }

        [TestMethod]
        public void Test_approximateMm()
        {
            int ret;
            int intMm = 0;

            FormTimer formTimer = new FormTimer();
            //var pbFormTimer = new PrivateObject(formTimer);
            //ret = pbFormTimer.Invoke("ApproximateMm", "00", intMm);
            ret = formTimer.ApproximateMm("aa", out intMm);
            Assert.IsTrue(ret == -1);

            ret = formTimer.ApproximateMm("0", out intMm);
            Assert.IsTrue(ret == -1);

            ret = formTimer.ApproximateMm("60", out intMm);
            Assert.IsTrue(ret == -1);

            ret = formTimer.ApproximateMm("00", out intMm);
            Assert.IsTrue(ret == 0);
            Assert.IsTrue(intMm == 0);

            ret = formTimer.ApproximateMm("07", out intMm);
            Assert.IsTrue(ret == 0);
            Assert.IsTrue(intMm == 0);

            ret = formTimer.ApproximateMm("08", out intMm);
            Assert.IsTrue(ret == 0);
            Assert.IsTrue(intMm == 15);

            ret = formTimer.ApproximateMm("22", out intMm);
            Assert.IsTrue(ret == 0);
            Assert.IsTrue(intMm == 15);

            ret = formTimer.ApproximateMm("23", out intMm);
            Assert.IsTrue(ret == 0);
            Assert.IsTrue(intMm == 30);

            ret = formTimer.ApproximateMm("37", out intMm);
            Assert.IsTrue(ret == 0);
            Assert.IsTrue(intMm == 30);

            ret = formTimer.ApproximateMm("52", out intMm);
            Assert.IsTrue(ret == 0);
            Assert.IsTrue(intMm == 45);

            ret = formTimer.ApproximateMm("53", out intMm);
            Assert.IsTrue(ret == 0);
            Assert.IsTrue(intMm == 60);

            ret = formTimer.ApproximateMm("59", out intMm);
            Assert.IsTrue(ret == 0);
            Assert.IsTrue(intMm == 60);
        }

        [TestMethod()]
        public void Test_readActivityLog()
        {
            FormTimer formTimer = new FormTimer();
            var pbFormTimer = new PrivateObject(formTimer);
            formTimer.ReadActivityLog();
            //ログファイルを開いて読み込まれていることを確認
            var strActivityLogFilePath = (string)pbFormTimer.GetField("strActivityLogFilePath");
            string textFromTextBox = formTimer.myTextBox1.Text;
            //byte[] bytesUTF8 = System.Text.Encoding.Default.GetBytes(textFromTextBox);
            //string textFromTextBoxSJIS = System.Text.Encoding.UTF8.GetString(bytesUTF8);
            string textFromLogFile = File.ReadAllText(@strActivityLogFilePath, (System.Text.Encoding)pbFormTimer.GetField("sjisEnc"));
            Assert.IsTrue(textFromTextBox.Equals(textFromLogFile));
        }

        [TestMethod()]
        public void Test_btnSaveActivityLog_Click()
        {
            FormTimer formTimer = new FormTimer();
            var pbFormTimer = new PrivateObject(formTimer);
            formTimer.btnSaveActivityLog_Click(null, null);
            //string outString = "Start,Task:00:00-00:15";
            //pbFormTimer.Invoke("WriteLog", outString);
            //ログファイルを開いて書き込まれていることを確認
            var strActivityLogFilePath = (string)pbFormTimer.GetField("strActivityLogFilePath");
            string textFromTextBox = formTimer.myTextBox1.Text;
            //byte[] bytesUTF8 = System.Text.Encoding.Default.GetBytes(textFromTextBox);
            //string textFromTextBoxSJIS = System.Text.Encoding.UTF8.GetString(bytesUTF8);
            string textFromLogFile = File.ReadAllText(@strActivityLogFilePath, (System.Text.Encoding)pbFormTimer.GetField("sjisEnc"));
            Assert.IsTrue(textFromTextBox.Equals(textFromLogFile));
        }

        [TestMethod]
        public void Test_getApproximateIntHhAndMm()
        {
            int ret;
            int intStartHh, intStartMm, intEndHh, intEndMm;

            FormTimer formTimer = new FormTimer();
            ret = formTimer.getApproximateIntHhAndMm("00:00", "01:00", out intStartHh, out intStartMm, out intEndHh, out intEndMm);
            Assert.IsTrue(ret == 0);
            Assert.IsTrue(intStartHh == 0);
            Assert.IsTrue(intStartMm == 0);
            Assert.IsTrue(intEndHh == 1);
            Assert.IsTrue(intEndMm == 0);

            ret = formTimer.getApproximateIntHhAndMm("00:10", "02:20", out intStartHh, out intStartMm, out intEndHh, out intEndMm);
            Assert.IsTrue(ret == 0);
            Assert.IsTrue(intStartHh == 0);
            Assert.IsTrue(intStartMm == 15);
            Assert.IsTrue(intEndHh == 2);
            Assert.IsTrue(intEndMm == 15);

            ret = formTimer.getApproximateIntHhAndMm("12:00", "13:00", out intStartHh, out intStartMm, out intEndHh, out intEndMm);
            Assert.IsTrue(ret == 0);
            Assert.IsTrue(intStartHh == 0);
            Assert.IsTrue(intStartMm == 0);
            Assert.IsTrue(intEndHh == 1);
            Assert.IsTrue(intEndMm == 0);

            ret = formTimer.getApproximateIntHhAndMm("12:10", "13:20", out intStartHh, out intStartMm, out intEndHh, out intEndMm);
            Assert.IsTrue(ret == 0);
            Assert.IsTrue(intStartHh == 0);
            Assert.IsTrue(intStartMm == 15);
            Assert.IsTrue(intEndHh == 1);
            Assert.IsTrue(intEndMm == 15);

            ret = formTimer.getApproximateIntHhAndMm("00:10", "13:20", out intStartHh, out intStartMm, out intEndHh, out intEndMm);
            Assert.IsTrue(ret == 0);
            Assert.IsTrue(intStartHh == 0);
            Assert.IsTrue(intStartMm == 0);
            Assert.IsTrue(intEndHh == 1);
            Assert.IsTrue(intEndMm == 15);
        }
    }
}
