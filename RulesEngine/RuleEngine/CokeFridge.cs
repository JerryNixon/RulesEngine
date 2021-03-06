﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RuleEngine
{
    public class CokeFridge : IRule
    {
        private readonly Func<Telemetry, Severity?, IEvent> _eventGenerator;

        public CokeFridge(int threshold, Severity severityLevel, Func<Telemetry, Severity?, IEvent> eventGenerator)
        {
            Name = "CokeFridge";
            Threshold = threshold;
            SeverityLevel = severityLevel;
            _eventGenerator = eventGenerator;
        }

        public int Id {get; set;}
        public string Name {get; set;}
        public int Threshold {get; set;}
        public int WindowOfTime {get; set;}
        public Severity? SeverityLevel {get; set;}

        public bool isMatch(Telemetry telemetry)
        {
            return (telemetry.FridgeType == Name && telemetry.value >= Threshold) ? true : false;
        }

        public IEvent CalculateEvent(Telemetry telemetry)
        {
            return _eventGenerator(telemetry, SeverityLevel);
        }
    }


}
