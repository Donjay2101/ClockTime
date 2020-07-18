var punchLog = (function () {


    var loadPunchLog = () => {
        $.ajax({
            method: "Get",
            url: constants.BaseUrl + "home/schedule1",
            success: function (response) {
                $('#data').html(response);
            },
            error: function (err) {
                $('#data').html("data is not availble for the day");
                console.log(err);
            },
            complete: function () {

            }
        });
    };

    var InsertPunchLog = (obj) => {
        $.ajax({
            method: "POST",
            data: { log: obj },
            content: "application/json",         
            url: constants.BaseUrl + "home/schedule1",
            success: function (response) {
               // $('#data').html(response);
                loadPunchLog();
                updateStatus(obj.PunchName);
            },
            error: function (err) {
                $('#data').html("data is not availble for the day");
                console.log(err);
                alert('something went wrong try after sometime');
            },
            complete: function () {

            }
        });
    };

    updateStatus = (status) => {
        console.log(status);
        switch (status) {
            case constants.combination.ClockedIn:
                punchTime.enableButtons();
                punchTime.disableButton('actClockIn');
                //enableButton('actClockOut');
                $('#spnStatus').html(constants.ClockedIn);
                $('#spnStatus').prop('class', 'text-success');
                break;
            case constants.combination.ClockedOut:
                punchTime.enableButtons();
                //enableButton('actClockIn');
                punchTime.disableButton('actClockOut');
                $('#spnStatus').html(constants.ClocedOut);
                $('#spnStatus').prop('class', 'text-danger');
                break;
            case constants.combination.Lunch:
                punchTime.enableButtons();
                punchTime.disableButton('actLunchStart');
                // enableButton('actLunchEnd');
                $('#spnStatus').html(constants.Lunch);
                $('#spnStatus').prop('class', 'text-success');
                break;
            case constants.combination.Lunchout:
                punchTime.enableButtons();
                punchTime.disableButton('actLunchEnd');
                //enableButton('actLunchStart');
                $('#spnStatus').html(constants.Lunchout);
                $('#spnStatus').prop('class', 'text-danger');
                break;
            default:
                $('#spnStatus').html(constants.NoStatus);
                $('#spnStatus').prop('class', 'text-secondary');
                break;
        }
    }


    return {
        loadPunchLog,
        InsertPunchLog
    }

})();