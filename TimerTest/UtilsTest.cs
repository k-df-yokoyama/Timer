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
    }
}
