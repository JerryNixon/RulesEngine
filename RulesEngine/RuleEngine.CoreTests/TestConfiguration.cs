using System;
using RuleEngine.Models;
using RuleEngine.Database;

namespace RuleEngine.Tests
{
    public class TestConfiguration : IConfiguration
    {
        public TestConfiguration()
        {
            DatabasePath = "database.db";
        }

        public string DatabasePath { get; set; }
    }
}
