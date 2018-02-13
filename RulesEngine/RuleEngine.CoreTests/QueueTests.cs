using Microsoft.VisualStudio.TestTools.UnitTesting;
using RuleEngine.Queue;
using System;
using System.Threading;

namespace RuleEngine.Tests
{
    [TestClass]
    [TestCategory("Queue")]
    public class QueueTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            var queue = new TelemetryQueue();
            queue.Clear();
        }

        [TestMethod]
        public void SendOne()
        {
            var telemetry = CreateTelemetry();
            var queue = new TelemetryQueue();
            queue.Send(telemetry);
            Assert.AreEqual(1, queue.Count);
        }

        [TestMethod]
        public void SendTen()
        {
            var queue = new TelemetryQueue();
            for (var i = 0; i < 10; i++)
            {
                var telemetry = CreateTelemetry();
                queue.Send(telemetry);
                Assert.AreEqual(i + 1, queue.Count);
            }
        }

        [TestMethod]
        public void ReceiveOne()
        {
            var trigger = new ManualResetEvent(false);

            var telemetry = CreateTelemetry();
            var queue = new TelemetryQueue(message =>
            {
                Assert.AreSame(telemetry, message);
                trigger.Set();
            });

            queue.Send(telemetry);
            Assert.AreEqual(1, queue.Count);

            trigger.WaitOne();
            Assert.AreEqual(0, queue.Count);
        }

        public static Models.Telemetry CreateTelemetry()
        {
            return new Models.Telemetry();
        }
    }
}
