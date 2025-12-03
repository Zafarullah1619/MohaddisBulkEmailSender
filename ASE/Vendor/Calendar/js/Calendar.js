function InitializeAvailability(CalendarSelector, Q) {
    InitializeQuarterCalendar(CalendarSelector,4,null, Q);
}

function InitializeAvailabilityCalendar(CalendarSelector, SelectedList, Q) {
    InitializeQuarterCalendar(CalendarSelector, 4, SelectedList, Q);
}
function InitializeQuarterCalendar(CalendarSelector, Quarters, SelectedList, Q) {

    /**************** Start Making Calender***********************/
    if (SelectedList == null)
    {
    var SelectedStartQuarter = Q * 9 + 1; //09:00 AM
    var SelectedEndQuarter = Q * 17 + 1; //17:00 PM
    }
    else
    {
            var SelectedStartQuarter = -1; //SELECTION FROM PROVIDED LIST
            var SelectedEndQuarter = -1; //SELECTION FROM PROVIDED LIST
    }

    /*if (!$(CalendarSelector).hasClass("row"))
    {
        $(CalendarSelector).addClass("row")
    }*/

    if (!$(CalendarSelector).hasClass("CalendarFont")) {
        $(CalendarSelector).addClass("CalendarFont")
    }

    var TimeClasses = [];
    var TimeMinutes = [];
    var sub = 0;
    var AddCount = 0;
    if (parseInt(Q) == 4)
    {
        TimeClasses[0] = "TimeFifteen";
        TimeClasses[1] = "TimeThirty";
        TimeClasses[2] = "TimeFourtyFive";
        TimeClasses[3] = "TimeSixty";
        TimeMinutes[0] = 15;
        TimeMinutes[1] = 30;
        TimeMinutes[2] = 45;
        TimeMinutes[3] = 60;
        sub = 15;
        AddCount = 1;
    }
    if (parseInt(Q) == 3) {
        TimeClasses[0] = "TimeFourtyFive";
        TimeClasses[1] = "";
        TimeClasses[2] = "";
        TimeClasses[3] = "TimeNinety";
        
        TimeMinutes[0] = 45;
        TimeMinutes[1] = 0;
        TimeMinutes[2] = 0;
        TimeMinutes[3] = 45;
        sub = 15;
        AddCount = 1;
    }
    else if (parseInt(Q) == 2) {
        TimeClasses[0] = "TimeThirty";
        TimeClasses[1] = "";
        TimeClasses[2] = "TimeSixty";
        TimeMinutes[0] = 30;
        TimeMinutes[1] = 0;
        TimeMinutes[2] = 60;
        sub = 30;
        AddCount = 2;
    }
    else if (parseInt(Q) == 1) {
        TimeClasses[0] = "TimeSixty";
        TimeMinutes[0] = 60;
        sub = 60;
        AddCount = 4;
    }
    //alert(TimeMinutes);
    $(CalendarSelector).html("<div class='col-sm-12 MainColumn'></div>");

    $(CalendarSelector + " .MainColumn").html('<div class="row FilterRow hold-date-div"></div><div class="row LabelRow"></div>');
    $(CalendarSelector + " .LabelRow").append('<div class="col-sm-3 LabelCell mainRowwithSelector"><div class="col-sm-12 text-center TimeListing"></div></div></div>');
    $(CalendarSelector + " .LabelRow").append('<div class="col-sm-1 daysRow LabelCell"><strong>Sunday</strong></div>');
    $(CalendarSelector + " .LabelRow").append('<div class="col-sm-1 daysRow LabelCell"><strong>Monday</strong></div>');
    $(CalendarSelector + " .LabelRow").append('<div class="col-sm-1 daysRow LabelCell"><strong>Tuesday</strong></div>');
    $(CalendarSelector + " .LabelRow").append('<div class="col-sm-1 daysRow LabelCell"><strong>Wednesday</strong></div>');
    $(CalendarSelector + " .LabelRow").append('<div class="col-sm-1 daysRow LabelCell"><strong>Thursday</strong></div>');
    $(CalendarSelector + " .LabelRow").append('<div class="col-sm-1 daysRow LabelCell"><strong>Friday</strong></div>');
    $(CalendarSelector + " .LabelRow").append('<div class="col-sm-1 daysRow LabelCell"><strong>Saturday</strong></div>');
    $(CalendarSelector + " .MainColumn").append('<div class="row"><div class="col-sm-12 TimeRows"></div></div>');

    $(CalendarSelector + " .FilterRow").html('<div class="col-sm-5"><label class="divNameFrom">From</label><select class="selectpicker TimeList TimeListFrom form-control" data-live-search="true"></select></div><div class="col-sm-5"><label class="divNameTo">To</label><select class="selectpicker TimeList TimeListTo form-control" data-live-search="true"></select></div><div class="col-sm-2"><a href="javascript: void(0)" class="ApplyTimes btn btn-sm btn-block btn-success">Filter</a></div>');
    $(CalendarSelector + " .TimeListing").html('<i class="fa fa-clock-o"></i>');

    var RowCount = 1;
   
    for (HourRow = 1; HourRow <= 24; HourRow++)
    {
        for (QuarterRow = 0; QuarterRow < 4; QuarterRow = QuarterRow + (AddCount))
        {
            var TimeClass = TimeClasses[QuarterRow];
            
            var EndTimeMinute = TimeMinutes[QuarterRow];
            var StartTimeMinute = EndTimeMinute - sub;
            
            //alert("EndMinute "+EndTimeMinute);
            //alert("StartMinute " + StartTimeMinute);
            var StartTimeHour = HourRow - 1;
            var EndTimeHour = StartTimeHour;
            
            
            EndTimeMinute = EndTimeMinute % 60;
            //alert(EndTimeMinute);
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
            Data += '<div class="timeRow col-sm-3 LabelCell text-center" data-StartTimeHour = "' + StartTimeHour + '" data-StartTimeMinute = "' + StartTimeMinute + '" data-EndTimeHour = "' + EndTimeHour + '" data-EndTimeMinute = "' + EndTimeMinute + '" data-row="0" data-col="0">' + '<div class="starttimebox">' + StartTimeHour + ':' + StartTimeMinute + '</div>' /*+ ' - ' + '<div  class="endtimebox">' + EndTimeHour + ':' + EndTimeMinute + '</div>'*/ + '</div>';
            var SelectedCellClass = "";

            for (var i = 1; i <= 7; i++)
            {
                if(SelectedList != null)
                {
                    var SelectedTime = findTimes(SelectedList[i - 1], RowCount, StartTimeHour, StartTimeMinute, EndTimeHour, EndTimeMinute);

                    if (SelectedTime != null && SelectedTime.length > 0 && SelectedTime[0].key > 0 && SelectedTime[0].key == RowCount)
                    {
                        SelectedTime = SelectedTime[0];
                    }
                    
                    if (SelectedTime != null && SelectedStartQuarter < 0 && SelectedTime.key > 0)
                    {
                        SelectedStartQuarter = SelectedTime.key;
                    }

                    if (SelectedTime != null && SelectedTime.key > 0 && SelectedEndQuarter < SelectedTime.key) {
                        SelectedEndQuarter = SelectedTime.key;
                    }
                }
                else
                {
                    var SelectedTime = null;
                }
                
                if ((RowCount >= SelectedStartQuarter && RowCount < SelectedEndQuarter) || (SelectedTime != null && SelectedTime.key == RowCount))
                {
                    SelectedCellClass = "SelectedCell ";
                }
                else
                {
                    SelectedCellClass = "";
                }
                Data += '<div class="col-sm-1 TimeCell ' + SelectedCellClass + TimeClass + '" data-StartTimeHour = "' + StartTimeHour + '" data-StartTimeMinute = "' + StartTimeMinute + '" data-EndTimeHour = "' + EndTimeHour + '" data-EndTimeMinute = "' + EndTimeMinute + '" data-row="' + RowCount + '" data-col="' + i + '"></div>';
                
            }

            /*Data += '<div class="col-sm-1 TimeCell ' + TimeClass + '" data-StartTimeHour = "' + StartTimeHour + '" data-StartTimeMinute = "' + StartTimeMinute + '" data-EndTimeHour = "' + EndTimeHour + '" data-EndTimeMinute = "' + EndTimeMinute + '" data-row="' + RowCount + '" data-col="1"></div>';

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

            Data += '<div class="col-sm-1 TimeCell ' + TimeClass + '" data-StartTimeHour = "' + StartTimeHour + '" data-StartTimeMinute = "' + StartTimeMinute + '" data-EndTimeHour = "' + EndTimeHour + '" data-EndTimeMinute = "' + EndTimeMinute + '" data-row="' + RowCount + '" data-col="7"></div>';*/
            Data += '</div>';



            $(CalendarSelector + " .TimeRows").last().append(Data);

            $(CalendarSelector + " .TimeList").append('<option value = "' + RowCount + '" data-tokens="' + StartTimeHour + ':' + StartTimeMinute + '">' + StartTimeHour + ':' + StartTimeMinute + '</option>');

            RowCount++;
        }
    }

    $(CalendarSelector + " .TimeListTo").find("option[value=1]").attr("value", 96);

    if (SelectedStartQuarter < 0)
    {
        SelectedStartQuarter = 37; //09:00 AM
    }

    if (SelectedEndQuarter < 0) {
        SelectedEndQuarter = 69; //17:00 PM
    }

    if (SelectedStartQuarter - Q >= 1)
    {
        $(CalendarSelector + " .TimeListFrom").val(SelectedStartQuarter - Q); //Add 4 Quarters
    }
    else
    {
        $(CalendarSelector + " .TimeListFrom").val(1); //Min Time
    }
    if (SelectedEndQuarter + parseInt(Q) <= 96) {
        $(CalendarSelector + " .TimeListTo").val(SelectedEndQuarter + parseInt(Q)); //Add 4 Quarters
    }
    else {
        $(CalendarSelector + " .TimeListTo").val(96);   //Max Time
    }

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
        console.log("TimeFrom "+TimeFrom);
        console.log("TimeTo "+TimeTo);

        var CurrentFromIndex = $(CalendarSelector + " .TimeBar[data-time = " + TimeFrom + "]").index();
        var CurrentToIndex = $(CalendarSelector + " .TimeBar[data-time = " + TimeTo + "]").index();
        console.log("CurrentFromIndex " + CurrentFromIndex);
        console.log("CurrentToIndex "+CurrentToIndex);

        $(CalendarSelector + " .TimeBar").css("display", "block");

        $(CalendarSelector + " .TimeBar:lt(" + CurrentFromIndex + ")").css("display", "none");
        $(CalendarSelector + " .TimeBar:gt(" + CurrentToIndex + ")").css("display", "none");

        $(CalendarSelector + " .TimeBar:lt(" + CurrentFromIndex + ")").find(".SelectedCell").removeClass("SelectedCell");
        $(CalendarSelector + " .TimeBar:lt(" + CurrentFromIndex + ")").find(".SelectedHover").removeClass("SelectedHover");

        $(CalendarSelector + " .TimeBar:gt(" + CurrentToIndex + ")").find(".SelectedCell").removeClass("SelectedCell");
        $(CalendarSelector + " .TimeBar:gt(" + CurrentToIndex + ")").find(".SelectedHover").removeClass("SelectedHover");
        console.log("CalendarSelector " + CalendarSelector);
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

function findTimes(listObjects, key, StartTimeHour, StartTimeMinute, EndTimeHour, EndTimeMinute) {
    return $.grep(listObjects, function (n) {
        return (n.key == key && n.StartTimeHour == StartTimeHour && n.StartTimeMinute == StartTimeMinute && n.EndTimeHour == EndTimeHour && n.EndTimeMinute == EndTimeMinute);
    });
};

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