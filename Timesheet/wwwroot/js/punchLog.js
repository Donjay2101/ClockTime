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
                $('#clockIn').modal('hide');
                $('#clockInSuccess').modal('show');
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
        
        status = status.toLowerCase();
        console.log(status);
        constants.combination[status.toLowerCase()];
        switch (status) {
            case constants.combination.clockedin:
                punchTime.enableButtons();
                punchTime.disableButton('actClockIn');
                //enableButton('actClockOut');
                $('#spnStatus').html(constants.ClockedIn);
                $('#spnStatus').prop('class', 'text-success');
                break;
            case constants.combination.clockedout:
                punchTime.enableButtons();
                //enableButton('actClockIn');
                punchTime.disableButton('actClockOut');                
                punchTime.disableButton('actLunchEnd');                
                punchTime.disableButton('actLunchStart');
                $('#spnStatus').html(constants.ClocedOut);
                $('#spnStatus').prop('class', 'text-danger');
                break;
            case constants.combination.lunchstart:
                punchTime.enableButtons();
                punchTime.disableButton('actLunchStart');
                // enableButton('actLunchEnd');
                $('#spnStatus').html(constants.Lunch);
                $('#spnStatus').prop('class', 'text-success');
                break;
            case constants.combination.lunchout:
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
        InsertPunchLog,
        updateStatus
    }

})();