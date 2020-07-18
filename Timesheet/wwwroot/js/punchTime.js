// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var punchTime = (function () {

    var url = constants.BaseUrl;
   
    console.log(url);
    var loadPunchLog = () => {
        $.ajax({
            method: "Get",
            url: constants.BaseUrl + "home/schedule",
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

    var insertStatus = (obj,status) => {
        $.ajax({
            method: "POST",
            data: { log: obj },
            content: "application/json",
            url: constants.BaseUrl + "home/schedule",
            success: function (response) {
                $('#clockIn').modal('hide');
               // getCurrentStatus();
                loadPunchLog();
                updateStatus(status);
                
                $('#clockInSuccess').modal('show');
            },
            error: function (err) {
                alert('something went wrong try again');
                console.log(err);
            },
            complete: function () {

            }
        });
    }
    
    var getCurrentStatus = () => {
        $.ajax({
            type: "GET",
            url: constants.BaseUrl + "home/Status",
            success: function (response) {
                $('#spnStatus').html(response);
            },
            error: function (err) {
                alert('something went wrong try again');
                console.log(err);
            },
            complete: function () {

            }
        });
    }

    var clockIn = (obj) => {
        insertStatus(obj, obj.PunchName);
    };

    var clockOut = (obj) => {
        insertStatus(obj, constants.ClockedOut);
    };

    var lunchIn= (obj) => {
        insertStatus(obj, constants.lunchIn);
    };

    var lunchOut= (obj) => {
        insertStatus(obj, constants.lunchOut);
    };


    updateStatus = (status) => {
        console.log(status);
        switch (status) {
            case constants.ClockedIn:
                enableButtons();
                disableButton('actClockIn');
                //enableButton('actClockOut');
                $('#spnStatus').html(constants.ClockedIn);
                $('#spnStatus').prop('class', 'text-success');
                break;
            case constants.ClocedOut:
                enableButtons();
                //enableButton('actClockIn');
                disableButton('actClockOut');
                $('#spnStatus').html(constants.ClocedOut);
                $('#spnStatus').prop('class', 'text-danger');
                break;
            case constants.Lunch:
                enableButtons();
                disableButton('actLunchStart');
               // enableButton('actLunchEnd');
                $('#spnStatus').html(constants.Lunch);
                $('#spnStatus').prop('class', 'text-success');
                break;
            case constants.Lunchout:
                enableButtons();
                disableButton('actLunchEnd');
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

    var disableButton = (btn) => {
        $('#' + btn).prop('disabled',true);
    }

    var enableButton = (btn) => {
        $('#' + btn).prop('disabled', false);
    }

    var enableButtons = () => {
        $('#actLunchStart').prop('disabled', false);
        $('#actLunchEnd').prop('disabled', false);
        $('#actClockIn').prop('disabled', true);
        $('#actClockOut').prop('disabled', false);
    }

    return {
        loadPunchLog,
        clockIn,
        clockOut,
        lunchIn,
        lunchOut,
        disableButton,
        enableButton,
        updateStatus,
        enableButtons,
        disableButton,
        enableButton
    }


})();