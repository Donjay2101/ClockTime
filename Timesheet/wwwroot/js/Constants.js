var constants = (function () {

    ClockedIn = "Clock In";
    ClocedOut = "Clock Out";
    Lunch = "Lunch start";
    Lunchout = "Lunch end";
    NoStatus = "No Status";

    BaseUrl = "https://localhost:44361/";



    var combination = {
        ClockedIn: "clockedIn",
        ClockedOut: "clockedout",
        Lunch: "lunchStart",
        Lunchout: "LunchStart"
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