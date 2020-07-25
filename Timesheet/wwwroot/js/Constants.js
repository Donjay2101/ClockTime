var constants = (function () {

    BaseUrl = "https://localhost:44361/";


    function stringToDate(_date, _format, _delimiter) {
        var formatLowerCase = _format.toLowerCase();
        var items = formatLowerCase.split(" ");
        var formatItems = items[0].split(_delimiter);        
        var date = _date.split(" ");
        var dateItems = date[0].split(_delimiter);
        if (date.length > 1) {
            var timeItems = date[1].split(":");
        }
        

        var monthIndex = formatItems.indexOf("mm");
        var dayIndex = formatItems.indexOf("dd");
        var yearIndex = formatItems.indexOf("yyyy");
        if (items.length > 1) {
            var timeFormatITems = items[1].split(":");
            var hours = timeFormatITems.indexOf("hh");
            var minutes = timeFormatITems.indexOf("mm");
        }
        

        var month = parseInt(dateItems[monthIndex]);
        month -= 1;
        var formatedDate = new Date(dateItems[yearIndex], month, dateItems[dayIndex], timeItems[hours], timeItems[minutes]);
        return formatedDate;
    }


    function isValid(val) {
        if (val != null && val != undefined && val != "") {
            return true;
        }
        return false;
    }

    //Urls ---------------- 
    var ajaxUrls= {
        loadDailyLogs: BaseUrl + "home/DailySchedule",
        insertLog: BaseUrl + "home/InsertLog",
        submitTimeSheet : BaseUrl + "home/SubmitTimesheet",
        editLog: BaseUrl + "home/EditLog",
        getHours: BaseUrl + "home/GetHours"
    }
    //Urls ---------------- 



    ClockedIn = "Clocked In";
    ClocedOut = "Clocked Out";
    Lunch = "Lunch started";
    Lunchout = "Lunch ended";
    NoStatus = "No Status";
    OverDue = "Overdue";
   



    var combination = {
        clockedin: "clockedin",
        clockedout: "clockedout",
        lunchstart: "lunchstart",
        lunchout: "lunchend",
        overdue: "overdue",
        nostatus: "notstatus"
    }
      
    var setLocalStorage = (loc, val) => {
        window.localStorage.setItem(loc, val);
    }

    var getLocalStorage = (loc) => {
        return window.localStorage.getItem(loc);
    }
    

    return{
        ClockedIn,
        ClocedOut,
        Lunch,
        Lunchout,
        NoStatus,
        BaseUrl,
        combination,
        setLocalStorage,
        getLocalStorage,
        ajaxUrls,
        stringToDate,
        isValid
    }
})();