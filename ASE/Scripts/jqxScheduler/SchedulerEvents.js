
$("#SchedulerID").on('editDialogCreate', function (event) {
    var args = event.args;
    var dialog = args.dialog;
    var appointment = args.appointment;
    var fields = args.fields;
});

$("#SchedulerID").on('editDialogOpen', function (event) {
    var args = event.args;
    var dialog = args.dialog;
    var appointment = args.appointment;
});

$("#SchedulerID").on('editDialogClose', function (event) {
    var args = event.args;
    var dialog = args.dialog;
    var appointment = args.appointment;
    var fields = args.fields;
});

$("#SchedulerID").on('dateChange', function (event) {
    var args = event.args;
    var from = args.from;
    var to = args.to;
    var date = args.date;
});

$("#SchedulerID").on('appointmentDelete', function (event) {
    var args = event.args;
    var appointment = args.appointment;
    var Id = appointment.originalData.id;
});

$("#SchedulerID").on('cellClick', function (event) {
    var args = event.args;
    var appointment = args.appointment;
    var Id = appointment.originalData.id;
    //$(".jqx-menu-ul").find('#createAppointment').remove();
});

$("#SchedulerID").on('contextMenuOpen', function (event) {
    var args = event.args;
    var appointment = args.appointment;
    var Id = appointment.originalData.id;
    //$(".jqx-menu-ul").find('#createAppointment').remove();
});

$('#SchedulerID').on('cellDoubleClick', function (event) {
    var args = event.args;
    var appointment = args.appointment;
    var Id = appointment.originalData.id;
    //appointment.jqxAppointment.timeZone=null;
    if (Id == null || Id == '') {
        $('#SchedulerID').jqxSchedulerID('closeDialog');
    }

});

$("#SchedulerID").on('contextMenuOpen', function (event) {
    var args = event.args;
    var menu = args.menu;
    var appointment = args.appointment;

    console.log("contextMenuOpen is raised");
});

$("#SchedulerID").on('contextMenuClose', function (event) {
    var args = event.args;
    var menu = args.menu;
    var appointment = args.appointment;

    console.log("contextMenuClose is raised");
});

$("#SchedulerID").on('contextMenuItemClick', function (event) {
    var args = event.args;
    var menu = args.menu;
    var appointment = args.appointment;
    var item = args.item;

    console.log("contextMenuItemClick is raised");
});

$("#SchedulerID").on('viewChange', function (event) {
    var args = event.args;
    var from = args.from;
    var to = args.to;
    var date = args.date;
    console.log("viewChange is raised");
});

$("#SchedulerID").on('dateChange', function (event) {
    var args = event.args;
    var from = args.from;
    var to = args.to;
    var date = args.date;
    console.log("dateChange is raised");
});

$("#SchedulerID").on('contextMenuCreate', function (event) {
    var args = event.args;
    var menu = args.menu;
    var appointment = args.appointment;
    var item = args.item;

    console.log("contextMenuCreate is raised");
});

$("#SchedulerID").on('editRecurrenceDialogOpen', function (event) {
    var args = event.args;
    var dialog = args.dialog;
    var appointment = args.appointment;

    console.log("editRecurrenceDialogOpen is raised");
});

$("#SchedulerID").on('editRecurrenceDialogClose', function (event) {
    var args = event.args;
    var dialog = args.dialog;
    var appointment = args.appointment;

    console.log("editRecurrenceDialogClose is raised");
});

$("#SchedulerID").on('appointmentChange', function (event) {
    var args = event.args;
    var appointment = args.appointment;
    // appointment fields
    // originalData - the bound data.
    // from - jqxDate object which returns when appointment starts.
    // to - jqxDate objet which returns when appointment ends.
    // status - String which returns the appointment's status("busy", "tentative", "outOfOffice", "free", "").
    // resourceId - String which returns the appointment's resouzeId
    // hidden - Boolean which returns whether the appointment is visible.
    // allDay - Boolean which returns whether the appointment is allDay Appointment.
    // resiable - Boolean which returns whether the appointment is resiable Appointment.
    // draggable - Boolean which returns whether the appointment is resiable Appointment.
    // id - String or Number which returns the appointment's ID.
    // subject - String which returns the appointment's subject.
    // location - String which returns the appointment's location.
    // description - String which returns the appointment's description.
    // tooltip - String which returns the appointment's tooltip.

    console.log("appointmentChange is raised");
});

$("#SchedulerID").on('appointmentClick', function (event) {
    var args = event.args;
    var appointment = args.appointment;
    // appointment fields
    // originalData - the bound data.
    // from - jqxDate object which returns when appointment starts.
    // to - jqxDate objet which returns when appointment ends.
    // status - String which returns the appointment's status("busy", "tentative", "outOfOffice", "free", "").
    // resourceId - String which returns the appointment's resouzeId
    // hidden - Boolean which returns whether the appointment is visible.
    // allDay - Boolean which returns whether the appointment is allDay Appointment.
    // resiable - Boolean which returns whether the appointment is resiable Appointment.
    // draggable - Boolean which returns whether the appointment is resiable Appointment.
    // id - String or Number which returns the appointment's ID.
    // subject - String which returns the appointment's subject.
    // location - String which returns the appointment's location.
    // description - String which returns the appointment's description.
    // tooltip - String which returns the appointment's tooltip.

    console.log("appointmentClick is raised");
});

$("#SchedulerID").on('appointmentDoubleClick', function (event) {
    var args = event.args;
    var appointment = args.appointment;
    // appointment fields
    // originalData - the bound data.
    // from - jqxDate object which returns when appointment starts.
    // to - jqxDate objet which returns when appointment ends.
    // status - String which returns the appointment's status("busy", "tentative", "outOfOffice", "free", "").
    // resourceId - String which returns the appointment's resouzeId
    // hidden - Boolean which returns whether the appointment is visible.
    // allDay - Boolean which returns whether the appointment is allDay Appointment.
    // resiable - Boolean which returns whether the appointment is resiable Appointment.
    // draggable - Boolean which returns whether the appointment is resiable Appointment.
    // id - String or Number which returns the appointment's ID.
    // subject - String which returns the appointment's subject.
    // location - String which returns the appointment's location.
    // description - String which returns the appointment's description.
    // tooltip - String which returns the appointment's tooltip.

    console.log("appointmentDoubleClick is raised");
});

$("#SchedulerID").on('appointmentAdd', function (event) {
    var args = event.args;
    var appointment = args.appointment;
    var Subject = appointment.subject;
    var StartDate = moment(appointment.originalData.start).format($("#MomentDateTimeFormat").val());
    var EndDate = moment(appointment.originalData.end).format($("#MomentDateTimeFormat").val());
    var Description = appointment.originalData.description;
    var Id = appointment.originalData.id;
    var ResourceId = appointment.resourceId;
    var IsAllDay=appointment.jqxAppointment.allDay;
    var Location = appointment.jqxAppointment.location;
    var RecurrenceException = appointment.jqxAppointment.recurrenceException;
    var Status = appointment.jqxAppointment.status;
    var TimeZone = appointment.jqxAppointment.timeZone;

});