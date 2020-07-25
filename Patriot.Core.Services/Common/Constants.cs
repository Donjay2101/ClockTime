using System;
using System.Collections.Generic;
using System.Text;

namespace Patriot.Core.Services.Common
{
    public static class Constants
    {
        public static string ClockedIn { get; } = "Clocked In";
        public static string ClockedOut { get; } = "Clocked Out";
        public static string Lunch { get; } = "Lunch Start";
        public static string Lunchout { get; } = "Lunch End";
        public static string NoStatus { get; } = "No Status";
        public static string OverDue { get; } = "Over Due";

        public static string[] Columns = new string[] { "clockedin", "clockedout", "lunchstart", "lunchend" };

        
    }

    public static class Status
    {
        public static string InProgress { get; } = "Inprogress";
        public static string Pending { get; } = "Pending";
        public static string Submitted { get; } = "Submitted";
        public static string Approved { get; } = "Approved";
    }


    public static class ResponseMessages
    {
        public static string TimeSheetSubmitted { get; set; } = " Timesheet submitted successfully";
        public static string TimesheetAlreadySubmitted { get; set; } = " Timesheet has been submitted previously for this time period";
    }
}
