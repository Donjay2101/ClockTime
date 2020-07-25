// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(document).ready(function (e) {





    $('#actClockIn').click(function () {
        $('#clockTitle').html("Would you like to Clock In ?");
        $('#btnClockIn').html(constants.ClockedIn);
        constants.setLocalStorage('band', constants.combination.clockedin);
        $('#clockIn').modal('show');
    });


    $('#actClockOut').click(function () {
        $('#clockTitle').html("Would you like to Clock out ?");
        $('#btnClockIn').html(constants.ClocedOut);
        constants.setLocalStorage('band', constants.combination.clockedout);
        $('#clockIn').modal('show');
    });


    $('#actLunchStart').click(function () {
        $('#clockTitle').html("Would you like to add description?");
        $('#btnClockIn').html(constants.Lunch);
        constants.setLocalStorage('band', constants.combination.lunchstart);
        $('#clockIn').modal('show');
    });

    $('#actLunchEnd').click(function () {
        $('#clockTitle').html("Would you like to description?");
        $('#btnClockIn').html(constants.Lunchout);
        constants.setLocalStorage('band', constants.combination.lunchout);
        $('#clockIn').modal('show');
    });


    $('#actSubmit').click(function () {
        $('#clockInPrompt').modal('show');

    });

    $('#btnOkay').click(function () {
        punchLog.submitTimeSheet();
    });

    $('#btnClockIn').click(function () {
        debugger;
        var name = constants.getLocalStorage('band')
        var obj = { description: $('#txtDescription').val(), PunchName: name, PunchTime: new Date().toJSON() };
        punchLog.insertPunchLog(obj);

        //  punchTime.clockIn(obj,name);
        
    });


    $('#btnEditOkay').click(function () {
        var newlogTime = $('#editDate').val();
        var logID = $('#editLogId').val();
        var status = $('#editStatusLog').val();
        var table = constants.getLocalStorage('table');
        console.log(table);
        if (constants.isValid(logID)) {
            punchLog.editPunchLog(logID, newlogTime, status);
        }

    });
});
function openEditLog(time,id,status) {
    debugger;    
    $('#editLogId').val(id);
    $('#editStatusLog').val(status);
    var date = constants.stringToDate(time, "mm/dd/yyyy HH:mm", "/"); 
    //$('#editDate').val(date);
    $('#modalEditLog').modal('show'); 
    var table = $(this).closest('table');
    constants.setLocalStorage('table', table);
    $('#datetimepicker1').datetimepicker({
        // Formats
        // follow MomentJS docs: https://momentjs.com/docs/#/displaying/format/
        useCurrent: false,
        format: 'DD/MM/YYYY HH:mm',
        defaultDate: date
    });

   // $("#datetimepicker1").data('DateTimePicker').setLocalDate(date);
}

