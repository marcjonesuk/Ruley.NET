﻿using System.Diagnostics;

namespace Ruley.NET
{
    public class PerfCounterStage : InlineStage
    {
        [Primary]
        public Property<string> Counter { get; set; }
        public Property<string> Category { get; set; }
        public Property<string> Instance { get; set; }
        public Property<string> Destination { get; set; }

        private PerformanceCounter myCounter = new System.Diagnostics.PerformanceCounter();

        public override void OnFirst(Event e)
        {
            myCounter.CategoryName = Category.Get(e);
            myCounter.CounterName = Counter.Get(e);
            myCounter.InstanceName = Instance.Get(e);
        }

        public override Event Apply(Event e)
        {
            float raw = myCounter.NextValue();
            e[Destination.Get(e)] = raw;
            return e;
        }
    }
}
