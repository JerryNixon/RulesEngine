using RuleEngine.Models;
using System;

namespace RuleEngine.Queue
{
    public class TelemetryQueue : QueueBase<Telemetry>
    {
        // save telemetry to database

        public TelemetryQueue(Action<Telemetry> listener)
            : base(listener)
        {
            // empty
        }

        public TelemetryQueue()
            : base()
        {
            // empty
        }
    }
}
