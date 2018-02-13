using System;

namespace RuleEngine.Models
{
    public class Telemetry
    {
    }

    public class Rule
    {
    }

    public class Record
    {
        public string Id { get; set; } = Guid.Empty.ToString();
        public string Key { get; set; } = Guid.Empty.ToString();
        public DateTime DateTime { get; set; } = DateTime.Now;
    }
}

