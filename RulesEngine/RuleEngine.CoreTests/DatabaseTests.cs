using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RuleEngine.Database;
using RuleEngine.Models;

namespace RuleEngine.Tests
{
    [TestClass]
    [TestCategory("Database")]
    public class DatabaseTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            var service = CreateService();
            service.DeleteDatabase();
        }

        [TestMethod]
        public void AddOneRecord()
        {
            var context = CreateService();
            if (context.TryAddRecord(CreateRecord()))
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
            var context = CreateService();
            for (var i = 0; i < 10; i++)
            {
                var record = CreateRecord();
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
            var context = CreateService();
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
            var context = CreateService();
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
            var context = CreateService();
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
            var context = CreateService();
            if (context.TryGetRecords(Guid.Empty.ToString(), DateTime.Now.AddHours(-3), out var records))
            {
                Assert.AreEqual(3, records.Count());
            }
            else
            {
                Assert.Fail();
            }
        }

        public static DatabaseService CreateService()
        {
            return new DatabaseService(new TestConfiguration());
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
}
