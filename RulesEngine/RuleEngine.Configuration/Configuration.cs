using System;

namespace RuleEngine
{
    public interface IConfiguration
    {
        string DatabasePath { get; set; }
    }

    public class Configuration : IConfiguration
    {
        public string DatabasePath { get; set; }
    }
}
