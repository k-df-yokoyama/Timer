using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timer.Tests
{
    [TestClass]
    public class UtilsTest
    {
        [TestMethod]
        public void Test_removeTimeString()
        {
            string inString = "Start,Task:00:00-00:15-00:30";
            string outString = "";
            Utils.RemoveTimeString(inString, out outString);

            Assert.IsTrue(outString.Equals("Start,Task"));

            inString = "Start,Task:00:00-00:15-";
            outString = "";
            Utils.RemoveTimeString(inString, out outString);

            Assert.IsTrue(outString.Equals("Start,Task"));

            inString = "Start,Task:00:00-00:15";
            outString = "";
            Utils.RemoveTimeString(inString, out outString);

            Assert.IsTrue(outString.Equals("Start,Task"));

            inString = "Start,Task:00:00-";
            outString = "";
            Utils.RemoveTimeString(inString, out outString);

            Assert.IsTrue(outString.Equals("Start,Task"));
        }

        [TestMethod]
        public void Test_approximateMm()
        {
            int ret;
            int intMm = 0;

            //FormTimer Utils = new FormTimer();
            //var pbFormTimer = new PrivateObject(Utils);
            //ret = pbFormTimer.Invoke("GetApproximateIntMm", "00", intMm);
            ret = Utils.GetApproximateIntMm("aa", out intMm);
            Assert.IsTrue(ret == -1);

            ret = Utils.GetApproximateIntMm("0", out intMm);
            Assert.IsTrue(ret == -1);

            ret = Utils.GetApproximateIntMm("60", out intMm);
            Assert.IsTrue(ret == -1);

            ret = Utils.GetApproximateIntMm("00", out intMm);
            Assert.IsTrue(ret == 0);
            Assert.IsTrue(intMm == 0);

            ret = Utils.GetApproximateIntMm("07", out intMm);
            Assert.IsTrue(ret == 0);
            Assert.IsTrue(intMm == 0);

            ret = Utils.GetApproximateIntMm("08", out intMm);
            Assert.IsTrue(ret == 0);
            Assert.IsTrue(intMm == 15);

            ret = Utils.GetApproximateIntMm("22", out intMm);
            Assert.IsTrue(ret == 0);
            Assert.IsTrue(intMm == 15);

            ret = Utils.GetApproximateIntMm("23", out intMm);
            Assert.IsTrue(ret == 0);
            Assert.IsTrue(intMm == 30);

            ret = Utils.GetApproximateIntMm("37", out intMm);
            Assert.IsTrue(ret == 0);
            Assert.IsTrue(intMm == 30);

            ret = Utils.GetApproximateIntMm("52", out intMm);
            Assert.IsTrue(ret == 0);
            Assert.IsTrue(intMm == 45);

            ret = Utils.GetApproximateIntMm("53", out intMm);
            Assert.IsTrue(ret == 0);
            Assert.IsTrue(intMm == 60);

            ret = Utils.GetApproximateIntMm("59", out intMm);
            Assert.IsTrue(ret == 0);
            Assert.IsTrue(intMm == 60);
        }

        [TestMethod]
        public void Test_GetApproximateIntHhAndMm()
        {
            int ret;
            int intStartHh, intStartMm, intEndHh, intEndMm;

            //FormTimer formTimer = new FormTimer();
            ret = Utils.GetApproximateIntHhAndMm("00:00", "01:00", out intStartHh, out intStartMm, out intEndHh, out intEndMm);
            Assert.IsTrue(ret == 0);
            Assert.IsTrue(intStartHh == 0);
            Assert.IsTrue(intStartMm == 0);
            Assert.IsTrue(intEndHh == 1);
            Assert.IsTrue(intEndMm == 0);

            ret = Utils.GetApproximateIntHhAndMm("00:10", "02:20", out intStartHh, out intStartMm, out intEndHh, out intEndMm);
            Assert.IsTrue(ret == 0);
            Assert.IsTrue(intStartHh == 0);
            Assert.IsTrue(intStartMm == 15);
            Assert.IsTrue(intEndHh == 2);
            Assert.IsTrue(intEndMm == 15);

            ret = Utils.GetApproximateIntHhAndMm("12:00", "13:00", out intStartHh, out intStartMm, out intEndHh, out intEndMm);
            Assert.IsTrue(ret == 0);
            Assert.IsTrue(intStartHh == 0);
            Assert.IsTrue(intStartMm == 0);
            Assert.IsTrue(intEndHh == 1);
            Assert.IsTrue(intEndMm == 0);

            ret = Utils.GetApproximateIntHhAndMm("12:10", "13:20", out intStartHh, out intStartMm, out intEndHh, out intEndMm);
            Assert.IsTrue(ret == 0);
            Assert.IsTrue(intStartHh == 0);
            Assert.IsTrue(intStartMm == 15);
            Assert.IsTrue(intEndHh == 1);
            Assert.IsTrue(intEndMm == 15);

            ret = Utils.GetApproximateIntHhAndMm("00:10", "13:20", out intStartHh, out intStartMm, out intEndHh, out intEndMm);
            Assert.IsTrue(ret == 0);
            Assert.IsTrue(intStartHh == 0);
            Assert.IsTrue(intStartMm == 0);
            Assert.IsTrue(intEndHh == 1);
            Assert.IsTrue(intEndMm == 15);
        }

    }
}
