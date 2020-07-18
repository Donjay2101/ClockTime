using System;
using System.Collections.Generic;
using System.Text;

namespace Patriot.Core.Services.Common
{
    public static class Constants
    {
        public static string ClockedIn { get; } = "Clocked In";
        public static string ClocedOut { get; } = "Clocked Out";
        public static string Lunch { get; } = "Lunch Start";
        public static string Lunchout { get; } = "Lunch End";
        public static string NoStatus { get; } = "No Status";
        public static string[] Columns = new string[] { "clockedin", "Clockedout", "lunchstart", "lunchend" };
    }
}
