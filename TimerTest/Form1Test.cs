using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Timer;
using System.IO;
using System.Linq;

namespace TimerTest
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
            pbFormTimer.Invoke("writeLog", outString);
            //ログファイルを開いて書き込まれていることを確認
            var strLogFilePath = (string)pbFormTimer.GetField("strLogFilePath");
            string last = File.ReadLines(@strLogFilePath).Last();
            string loggedString = last.Substring(last.IndexOf(",") + 1);
            Assert.IsTrue(outString.Equals(loggedString));
        }

        [TestMethod]
        public void Test_approximateMm()
        {
            int ret;
            int intMm = 0;

            FormTimer formTimer = new FormTimer();
            //var pbFormTimer = new PrivateObject(formTimer);
            //ret = pbFormTimer.Invoke("ApproximateMm", "00", intMm);
            ret = formTimer.ApproximateMm("aa", ref intMm);
            Assert.IsTrue(ret == -1);

            ret = formTimer.ApproximateMm("0", ref intMm);
            Assert.IsTrue(ret == -1);

            ret = formTimer.ApproximateMm("00", ref intMm);
            Assert.IsTrue(ret == 0);
            Assert.IsTrue(intMm == 0);

            ret = formTimer.ApproximateMm("07", ref intMm);
            Assert.IsTrue(ret == 0);
            Assert.IsTrue(intMm == 0);

            ret = formTimer.ApproximateMm("08", ref intMm);
            Assert.IsTrue(ret == 0);
            Assert.IsTrue(intMm == 15);

            ret = formTimer.ApproximateMm("22", ref intMm);
            Assert.IsTrue(ret == 0);
            Assert.IsTrue(intMm == 15);

            ret = formTimer.ApproximateMm("23", ref intMm);
            Assert.IsTrue(ret == 0);
            Assert.IsTrue(intMm == 30);

            ret = formTimer.ApproximateMm("37", ref intMm);
            Assert.IsTrue(ret == 0);
            Assert.IsTrue(intMm == 30);

            ret = formTimer.ApproximateMm("52", ref intMm);
            Assert.IsTrue(ret == 0);
            Assert.IsTrue(intMm == 45);

            ret = formTimer.ApproximateMm("53", ref intMm);
            Assert.IsTrue(ret == 0);
            Assert.IsTrue(intMm == 0);

            ret = formTimer.ApproximateMm("59", ref intMm);
            Assert.IsTrue(ret == 0);
            Assert.IsTrue(intMm == 0);
        }
    }
}
