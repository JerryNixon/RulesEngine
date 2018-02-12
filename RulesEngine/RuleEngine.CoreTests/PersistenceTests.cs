using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RuleEngine.Models;
using RuleEngine.Persistence;

namespace RuleEngine.Tests
{
    public class TestConfiguration : IConfiguration
    {
        public TestConfiguration()
        {
            DatabasePath = "database.db";
        }

        public string DatabasePath { get; set; }

        public static PersistenceService CreateService()
        {
            return new PersistenceService(new TestConfiguration());
        }

        public static Record CreateRecord()
        {
            return new Record
            {
                Id = Guid.NewGuid().ToString(),
                Key = Guid.Empty.ToString(),
                DateTime = DateTime.Now
            };
        }
    }

    [TestClass]
    public class PersistenceTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            var service = TestConfiguration.CreateService();
            service.DeleteDatabase();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            var service = TestConfiguration.CreateService();
            service.DeleteDatabase();
        }

        [TestMethod]
        public void AddOneRecord()
        {
            var context = TestConfiguration.CreateService();
            if (context.TryAddRecord(TestConfiguration.CreateRecord()))
            {
                Assert.AreEqual(1, context.RecordCount());
            }
            else
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void AddTenRecords()
        {
            var context = TestConfiguration.CreateService();
            for (var i = 0; i < 10; i++)
            {
                var record = TestConfiguration.CreateRecord();
                record.DateTime = DateTime.Now.AddHours(-i);
                if (context.TryAddRecord(record))
                {
                    Assert.AreEqual(i + 1, context.RecordCount());
                }
                else
                {
                    Assert.Fail();
                }
            }
        }

        [TestMethod]
        public void GetZeroRecords()
        {
            AddOneRecord();
            var context = TestConfiguration.CreateService();
            if (context.TryGetRecords("JERRYNIXON", DateTime.Now.AddDays(-1), out var records))
            {
                Assert.AreEqual(0, records.Count());
            }
            else
            {
                Assert.Fail("TryGetRecords");
            }
        }

        [TestMethod]
        public void GetOneRecord()
        {
            AddOneRecord();
            var context = TestConfiguration.CreateService();
            if (context.TryGetRecords(Guid.Empty.ToString(), DateTime.Now.AddDays(-1), out var records))
            {
                Assert.AreEqual(1, records.Count());
            }
            else
            {
                Assert.Fail("TryGetRecords");
            }
        }

        [TestMethod]
        public void GetTenRecords()
        {
            AddTenRecords();
            var context = TestConfiguration.CreateService();
            if (context.TryGetRecords(Guid.Empty.ToString(), DateTime.Now.AddDays(-1), out var records))
            {
                Assert.AreEqual(10, records.Count());
            }
            else
            {
                Assert.Fail("TryGetRecords");
            }
        }

        [TestMethod]
        public void GetRangeRecords()
        {
            AddTenRecords();
            var context = TestConfiguration.CreateService();
            if (context.TryGetRecords(Guid.Empty.ToString(), DateTime.Now.AddHours(-3), out var records))
            {
                Assert.AreEqual(3, records.Count());
            }
            else
            {
                Assert.Fail();
            }
        }
    }
}
