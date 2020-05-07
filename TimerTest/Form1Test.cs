using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Timer;

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
    }
}
