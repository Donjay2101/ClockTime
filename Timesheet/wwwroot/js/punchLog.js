var punchLog = (function () {
    var loadPunchLog = () => {
        console.log(constants.ajaxUrls.loadDailyLogs);
        $.ajax({
            method: "Get",
            url: constants.ajaxUrls.loadDailyLogs,
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

    var loadHours = () => {
        debugger;
        $.ajax({
            method: "Get",
            url: constants.ajaxUrls.getHours,
            success: function (response) {
                if (response.statusCode == 200) {
                    var result = response.result.split("|");
                    if (constants.isValid(result)) {
                        
                        $('#spnCurrentHours').html(result[0]);                 
                        $('#spnTotalHours').html(result[1]);
                        $('#spnLunchHours').html(result[2]);
                    }
                }
            },
            error: function (err) {                
                console.log(err);
            },
            complete: function () {

            }
        });
    };

    var insertPunchLog = (obj) => {
        $.ajax({
            method: "POST",
            data: { log: obj },
            content: "application/json",         
            url: constants.ajaxUrls.insertLog,
            success: function (response) {
               // $('#data').html(response);
                loadPunchLog();
                updateStatus(obj.PunchName);
                $('#clockIn').modal('hide');
                //$('#')
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

    var submitTimeSheet = () => {     
        $.ajax({
            url: constants.ajaxUrls.submitTimeSheet,
            method: "POST",
            contentType: "application/json",
            success: function (response) {
                alert(response);
                updateStatus(constants.combination.nostatus);
                loadPunchLog();
            },
            error: function (error) {
                console.log(error);
                alert('something went wrong try to submit after sometime');
            },
            complete: function () {

            }
        });

    }

    disableButtons = () => {
        punchTime.disableButton('actSubmit', 'btn-success');
        punchTime.disableButton('actLunchStart', 'btn-success');
        punchTime.disableButton('actLunchEnd', 'btn-danger');
        punchTime.disableButton('actClockOut', 'btn-danger');

        punchTime.enableButton('actClockIn', 'btn-success');   
    }

    var updateStatus = (status) => {
        
        status = status.toLowerCase();
        console.log(status);
        constants.combination[status.toLowerCase()];     
        switch (status) {
            case constants.combination.clockedin:            
                punchTime.disableButton('actSubmit','btn-success');
                punchTime.disableButton('actClockIn', 'btn-success');               
                punchTime.disableButton('actLunchEnd', 'btn-danger');


                punchTime.enableButton('actLunchStart', 'btn-success');
                punchTime.enableButton('actClockOut', 'btn-danger');                
                $('#spnStatus').html(constants.ClockedIn);
                $('#spnStatus').prop('class', 'text-success');
                break;
            case constants.combination.clockedout:        
                punchTime.disableButton('actClockOut', 'btn-danger');
                punchTime.disableButton('actLunchEnd', 'btn-danger');
                punchTime.disableButton('actLunchStart', 'btn-success');

                punchTime.enableButton('actClockIn', 'btn-success');              
                punchTime.enableButton('actSubmit','btn-success');
                
                $('#spnStatus').html(constants.ClocedOut);
                $('#spnStatus').prop('class', 'text-danger');
                break;
            case constants.combination.lunchstart:

                punchTime.disableButton('actClockIn', 'btn-success');  
                punchTime.disableButton('actLunchStart', 'btn-success');
                punchTime.disableButton('actClockOut', 'btn-danger');
                punchTime.disableButton('actSubmit', 'btn-success');               
                             
                punchTime.enableButton('actLunchEnd', 'btn-danger');    
                
                $('#spnStatus').html(constants.Lunch);
                $('#spnStatus').prop('class', 'text-success');
                break;
            case constants.combination.lunchout:
                punchTime.disableButton('actClockIn','btn-success');                
                punchTime.disableButton('actLunchEnd','btn-danger');      
                punchTime.disableButton('actSubmit','btn-success');

                punchTime.enableButton('actClockOut', 'btn-danger');    
                punchTime.enableButton('actLunchStart', 'btn-success'); 

                $('#spnStatus').html(constants.Lunchout);
                $('#spnStatus').prop('class', 'text-danger');
                break;
            case constants.combination.overdue:
                punchTime.disableButton('actClockIn', 'btn-success');
                punchTime.disableButton('actLunchStart', 'btn-success');
                punchTime.disableButton('actLunchEnd', 'btn-danger');
                punchTime.disableButton('actClockOut', 'btn-danger');

                
                punchTime.enableButton('actSubmit', 'btn-success');

                $('#spnStatus').html(constants.OverDue);
                $('#spnStatus').prop('class', 'text-danger');
                break;
            default:
                disableButtons();
                $('#spnStatus').prop('class', 'text-secondary');
                break;
        }
    }

    var editPunchLog = (logID, logTime,status) => {
        console.log(logID);
        console.log(status);    
        var date = constants.stringToDate(logTime, "dd/mm/yyyy HH:mm", "/"); 
        if (date > new Date()) {
            alert("date cannot be greater then todays's date");
            return;
        }
        var obj = {
            id: logID,
            punchTime: logTime,
            punchName: status       
        };
        $.ajax({
            url: constants.ajaxUrls.editLog,
            type: "POST",
            data: { dailyLog:obj} ,
            success: (response) => {                
                alert(response.result);
                if (response.statusCode == 200) {
                    $('#modalEditLog').modal('hide');
                    loadPunchLog();
                }
                
            },
            error: (err) => {
                console.log("edit error:", err);
            },
            complete: () => {
              
            }
        });
    }


    var clock = () => {
        setInterval(() => {
            $('#spnCurrentDate').html(moment().format('MM/DD/YYYY, HH:mm:ss'));
        }, 1000);
    }


  

    return {
        loadPunchLog,
        insertPunchLog,
        updateStatus,
        submitTimeSheet,
        editPunchLog,
        clock,
        loadHours
    }

})();