using System.Windows;
using AddinEngine;

namespace WorkBenchContract
{
    public class StatusBarContentlPointBulder : ClassPointBuilder<StatusBarItem>
    {
        public const string StatusBarContentlPoint = "WorkBench.StatusBarContentlPoint";

        public override string ExensionPoint
        {
            get { return StatusBarContentlPoint; }
        }


    }

    public class StatusBarItem
    {
        public bool IsLeft { get; set; }

        public FrameworkElement Content { get; set; }
    }
}
