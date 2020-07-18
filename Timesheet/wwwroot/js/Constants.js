var constants = (function () {

    ClockedIn = "Clocked In";
    ClocedOut = "Clocked Out";
    Lunch = "Lunch started";
    Lunchout = "Lunch ended";
    NoStatus = "No Status";

    BaseUrl = "https://localhost:44361/";



    var combination = {
        clockedin: "clockedin",
        clockedout: "clockedout",
        lunchstart: "lunchstart",
        lunchout: "lunchend"
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
        getLocalStorage
    }
})();