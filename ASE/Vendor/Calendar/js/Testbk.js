function InitializeAvailabilityCalendar(CalendarSelector) {
    InitializeQuarterCalendar(CalendarSelector, 4);
}
function InitializeQuarterCalendar(CalendarSelector, Quarters) {

    /**************** Start Making Calender***********************/
    var SelectedStartQuarter = 37; //09:00 AM
    var SelectedEndQuarter = 69; //17:00 PM

    if (!$(CalendarSelector).hasClass("row"))
    {
        $(CalendarSelector).addClass("row")
    }

    if (!$(CalendarSelector).hasClass("CalendarFont")) {
        $(CalendarSelector).addClass("CalendarFont")
    }

    var TimeClasses = [];
    TimeClasses[0] = "TimeFifteen";
    TimeClasses[1] = "TimeThirty";
    TimeClasses[2] = "TimeFourtyFive";
    TimeClasses[3] = "TimeSixty";

    var TimeMinutes = [];
    TimeMinutes[0] = 15;
    TimeMinutes[1] = 30;
    TimeMinutes[2] = 45;
    TimeMinutes[3] = 60;

    $(CalendarSelector).html("<div class='col-sm-12 MainColumn'></div>");

    $(CalendarSelector + " .MainColumn").html('<div class="row LabelRow"></div>');
    $(CalendarSelector + " .LabelRow").append('<div class="col-sm-3 LabelCell mainRowwithSelector"><div class="row"><div class = "date-label text-center"><strong>Days / Times</strong></div><div class="col-sm-12 text-center TimeListing"></div></div></div>');
    $(CalendarSelector + " .LabelRow").append('<div class="col-sm-1 daysRow LabelCell"><strong>Sunday</strong></div>');
    $(CalendarSelector + " .LabelRow").append('<div class="col-sm-1 daysRow LabelCell"><strong>Monday</strong></div>');
    $(CalendarSelector + " .LabelRow").append('<div class="col-sm-1 daysRow LabelCell"><strong>Tuesday</strong></div>');
    $(CalendarSelector + " .LabelRow").append('<div class="col-sm-1 daysRow LabelCell"><strong>Wednesday</strong></div>');
    $(CalendarSelector + " .LabelRow").append('<div class="col-sm-1 daysRow LabelCell"><strong>Thursday</strong></div>');
    $(CalendarSelector + " .LabelRow").append('<div class="col-sm-1 daysRow LabelCell"><strong>Friday</strong></div>');
    $(CalendarSelector + " .LabelRow").append('<div class="col-sm-1 daysRow LabelCell"><strong>Saturday</strong></div>');
    $(CalendarSelector + " .MainColumn").append('<div class="row"><div class="col-sm-12 TimeRows"></div></div>');

    $(CalendarSelector + " .TimeListing").html('<div class="hold-date-div"><span class="divNameFrom">From</span><select class="selectpicker TimeList TimeListFrom form-control" data-live-search="true"></select> <span class="divNameTo">To</span> <select class="selectpicker TimeList TimeListTo form-control" data-live-search="true"></select> <a href="javascript: void(0)" class="ApplyTimes"><i class="fa fa-search"></i></a></div>');

    var RowCount = 1;
    for (HourRow = 1; HourRow <= 24; HourRow++)
    {
        for(QuarterRow = 0; QuarterRow < 4; QuarterRow++)
        {
            var TimeClass = TimeClasses[QuarterRow];

            var EndTimeMinute = TimeMinutes[QuarterRow];
            var StartTimeMinute = EndTimeMinute - 15;

            var StartTimeHour = HourRow - 1;
            var EndTimeHour = StartTimeHour;

            EndTimeMinute = EndTimeMinute % 60;

            if (EndTimeMinute == 0) {
                EndTimeHour = EndTimeHour + 1;
            }

            EndTimeHour = EndTimeHour % 24;

            if (StartTimeHour < 10) {
                StartTimeHour = '0' + StartTimeHour;
            }

            if (EndTimeHour < 10) {
                EndTimeHour = '0' + EndTimeHour;
            }

            if (StartTimeMinute < 10) {
                StartTimeMinute = '0' + StartTimeMinute;
            }

            if (EndTimeMinute < 10) {
                EndTimeMinute = '0' + EndTimeMinute;
            }
            
            

            var Data = '<div class="row TimeBar" data-time="' + RowCount + '">'
            Data += '<div class="timeRow col-sm-3 LabelCell text-center" data-StartTimeHour = "' + StartTimeHour + '" data-StartTimeMinute = "' + StartTimeMinute + '" data-EndTimeHour = "' + EndTimeHour + '" data-EndTimeMinute = "' + EndTimeMinute + '" data-row="0" data-col="0">' + StartTimeHour + ':' + StartTimeMinute + ' - ' + EndTimeHour + ':' + EndTimeMinute + '</div>';
            Data += '<div class="col-sm-1 TimeCell ' + TimeClass + '" data-StartTimeHour = "' + StartTimeHour + '" data-StartTimeMinute = "' + StartTimeMinute + '" data-EndTimeHour = "' + EndTimeHour + '" data-EndTimeMinute = "' + EndTimeMinute + '" data-row="' + RowCount + '" data-col="1"></div>';

            if (RowCount >= SelectedStartQuarter && RowCount < SelectedEndQuarter) {
                TimeClass = "SelectedCell " + TimeClass;
            }

            Data += '<div class="col-sm-1 TimeCell ' + TimeClass + '" data-StartTimeHour = "' + StartTimeHour + '" data-StartTimeMinute = "' + StartTimeMinute + '" data-EndTimeHour = "' + EndTimeHour + '" data-EndTimeMinute = "' + EndTimeMinute + '" data-row="' + RowCount + '" data-col="2"></div>';
            Data += '<div class="col-sm-1 TimeCell ' + TimeClass + '" data-StartTimeHour = "' + StartTimeHour + '" data-StartTimeMinute = "' + StartTimeMinute + '" data-EndTimeHour = "' + EndTimeHour + '" data-EndTimeMinute = "' + EndTimeMinute + '" data-row="' + RowCount + '" data-col="3"></div>';
            Data += '<div class="col-sm-1 TimeCell ' + TimeClass + '" data-StartTimeHour = "' + StartTimeHour + '" data-StartTimeMinute = "' + StartTimeMinute + '" data-EndTimeHour = "' + EndTimeHour + '" data-EndTimeMinute = "' + EndTimeMinute + '" data-row="' + RowCount + '" data-col="4"></div>';
            Data += '<div class="col-sm-1 TimeCell ' + TimeClass + '" data-StartTimeHour = "' + StartTimeHour + '" data-StartTimeMinute = "' + StartTimeMinute + '" data-EndTimeHour = "' + EndTimeHour + '" data-EndTimeMinute = "' + EndTimeMinute + '" data-row="' + RowCount + '" data-col="5"></div>';
            Data += '<div class="col-sm-1 TimeCell ' + TimeClass + '" data-StartTimeHour = "' + StartTimeHour + '" data-StartTimeMinute = "' + StartTimeMinute + '" data-EndTimeHour = "' + EndTimeHour + '" data-EndTimeMinute = "' + EndTimeMinute + '" data-row="' + RowCount + '" data-col="6"></div>';

            if (RowCount >= SelectedStartQuarter && RowCount < SelectedEndQuarter) {
                TimeClass = TimeClasses[QuarterRow];
            }

            Data += '<div class="col-sm-1 TimeCell ' + TimeClass + '" data-StartTimeHour = "' + StartTimeHour + '" data-StartTimeMinute = "' + StartTimeMinute + '" data-EndTimeHour = "' + EndTimeHour + '" data-EndTimeMinute = "' + EndTimeMinute + '" data-row="' + RowCount + '" data-col="7"></div>';
            Data += '</div>';

            

            $(CalendarSelector + " .TimeRows").last().append(Data);

            $(CalendarSelector + " .TimeList").append('<option value = "' + RowCount + '" data-tokens="' + StartTimeHour + ':' + StartTimeMinute + '">' + StartTimeHour + ':' + StartTimeMinute + '</option>');

            RowCount++;
        }
    }

    $(CalendarSelector + " .TimeListTo").find("option[value=1]").attr("value", 96);

    $(CalendarSelector + " .TimeListFrom").val(SelectedStartQuarter - 4); //08:00 AM
    $(CalendarSelector + " .TimeListTo").val(SelectedEndQuarter + 4);   //06:00 PM
    /**************** End Making Calender***********************/                    

    var MouseDown = false;
    var SelectCells = true;
    var StartRow = 0;
    var StartCol = 0;

    $(document).on("change", CalendarSelector + " .TimeList", function () {
        var CurrentVal = parseInt($(this).val());
        var OtherVal = 0;
        var NewVal = 0;

        if ($(this).hasClass("TimeListFrom"))
        {
            var OtherElement = $(this).closest(".TimeListing").find(".TimeListTo");
            OtherVal = parseInt(OtherElement.val());

            if(CurrentVal >= OtherVal)
            {
                NewVal = CurrentVal + 1;
                OtherElement.val(NewVal);
            }
        }

        if ($(this).hasClass("TimeListTo")) {
            var OtherElement = $(this).closest(".TimeListing").find(".TimeListFrom");
            OtherVal = parseInt(OtherElement.val());

            if (CurrentVal <= OtherVal) {
                NewVal = CurrentVal - 1;
                OtherElement.val(NewVal);
            }
        }
    });

    $(document).on("click", CalendarSelector + " .ApplyTimes", function () {
        var TimeFrom = $(CalendarSelector + " .TimeListFrom").val();
        var TimeTo = $(CalendarSelector + " .TimeListTo").val();

        var CurrentFromIndex = $(CalendarSelector + " .TimeBar[data-time = " + TimeFrom + "]").index();
        var CurrentToIndex = $(CalendarSelector + " .TimeBar[data-time = " + TimeTo + "]").index() - 1;

        $(CalendarSelector + " .TimeBar").css("display", "block");

        $(CalendarSelector + " .TimeBar:lt(" + CurrentFromIndex + ")").css("display", "none");
        $(CalendarSelector + " .TimeBar:gt(" + CurrentToIndex + ")").css("display", "none");

        $(CalendarSelector + " .TimeBar:lt(" + CurrentFromIndex + ")").find(".SelectedCell").removeClass("SelectedCell");
        $(CalendarSelector + " .TimeBar:lt(" + CurrentFromIndex + ")").find(".SelectedHover").removeClass("SelectedHover");

        $(CalendarSelector + " .TimeBar:gt(" + CurrentToIndex + ")").find(".SelectedCell").removeClass("SelectedCell");
        $(CalendarSelector + " .TimeBar:gt(" + CurrentToIndex + ")").find(".SelectedHover").removeClass("SelectedHover");
    });

    $(document).on("mousedown", CalendarSelector + " .TimeCell", function () {
        MouseDown = true;

        if ($(this).hasClass("SelectedCell")) {
            SelectCells = false;
        }
        else {
            SelectCells = true;
        }

        if (!$(this).hasClass("SelectedHover") && MouseDown) {
            $(this).addClass("SelectedHover")
        }


        StartRow = parseInt($(this).attr("data-row"));
        StartCol = parseInt($(this).attr("data-col"));
    });

    $(document).on("mouseup", CalendarSelector + " .TimeCell", function () {

        MouseDown = false;

        $(".SelectedHover").each(function () {
            if (!$(this).hasClass("SelectedCell") && SelectCells) {
                $(this).addClass("SelectedCell");
            }
            else if ($(this).hasClass("SelectedCell") && !SelectCells) {
                $(this).removeClass("SelectedCell");
            }
            $(this).removeClass("SelectedHover");
        });

        StartRow = 0;
        StartCol = 0;

    });

    $(document).on("mouseenter", CalendarSelector + " .TimeCell", function () {
        var CurrentRow = parseInt($(this).attr("data-row"));
        var CurrentCol = parseInt($(this).attr("data-col"));

        var StartRowNumber = StartRow;
        var EndRowNumber = CurrentRow;

        var StartColNumber = StartCol;
        var EndColNumber = CurrentCol;

        if (StartRow > CurrentRow) {
            StartRowNumber = CurrentRow;
            EndRowNumber = StartRow;
        }

        if (StartCol > CurrentCol) {
            StartColNumber = CurrentCol;
            EndColNumber = StartCol;
        }

        var RowDifference = 1;
        var ColDifference = 1;


        if (StartColNumber > 0 && EndColNumber > 0 && StartRowNumber > 0 && EndRowNumber > 0 && RowDifference > 0 && ColDifference > 0) {
            $(".SelectedHover").removeClass("SelectedHover");
            for (var rowNo = StartRowNumber; rowNo <= EndRowNumber; rowNo = rowNo + RowDifference) {

                for (var colNo = StartColNumber; colNo <= EndColNumber; colNo = colNo + ColDifference) {

                    var CurrentCell = $(".TimeCell[data-row='" + rowNo + "'][data-col='" + colNo + "']");

                    if (!$(CurrentCell).hasClass("SelectedHover") && MouseDown) {
                        $(CurrentCell).addClass("SelectedHover")
                    }
                }
                if (rowNo > 100) {
                    break;
                }
            }
        }


    });

    $(document).on("mouseleave", CalendarSelector + " .TimeCell", function () {
        var CurrentRow = parseInt($(this).attr("data-row"));
        var CurrentCol = parseInt($(this).attr("data-col"));

        if ($(this).hasClass("SelectedHover") && MouseDown) {
            //$(this).removeClass("SelectedHover")
        }
    });

    $(CalendarSelector + " .ApplyTimes").click();
}

function GetDayTimes(CalendarSelector, DayOfWeek)
{
    var Times = [];
    var Count = 0;

    $(CalendarSelector + " .SelectedCell[data-col=" + DayOfWeek + "]").each(function () {

        var RowValue = $(this).data("row");

        var starttimehour = $(this).data("starttimehour");
        var starttimeminute = $(this).data("starttimeminute");

        var endtimehour = $(this).data("endtimehour");
        var endtimeminute = $(this).data("endtimeminute");

        Times.push(
            {
                key: RowValue,
                StartTimeHour: starttimehour,
                StartTimeMinute: starttimeminute,
                EndTimeHour: endtimehour,
                EndTimeMinute: endtimeminute
            });
        Count++;
    });

    return Times;
}

function GetSelectedTimes(CalendarSelector) {
    var AllTimes = [];
    var Count = 0;

    for (Count = 1; Count <= 7; Count++)
    {
        AllTimes.push(GetDayTimes(CalendarSelector, Count));
    }

    return AllTimes;
}