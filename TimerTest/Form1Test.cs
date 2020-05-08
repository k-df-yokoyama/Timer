using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Timer;
using System.IO;
using System.Linq;

namespace TimerTest
{
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
            string outString = "Task:00:00-00:15";
            formTimer.writeLog(outString);
            //ログファイルを開いて書き込まれていることを確認
            string last = File.ReadLines(@formTimer.strLogFilePath).Last();
            string loggedString = last.Substring(last.IndexOf(",") + 1);
            Assert.IsTrue(outString.Equals(loggedString));
        }
    }
}
