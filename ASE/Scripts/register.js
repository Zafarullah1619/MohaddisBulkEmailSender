/**
 * PayGenie - Digital Product NetWork
 * Copyright 2016 PayGenie.com
 *
 */
$(function () {
    $(".js-source-states").select2();

    $("#Password").on("keyup", function () {
        var currentString = $(this).val().trim();
        if (currentString.length >= 6) {
            CheckList("Min6");
        }
        else {
            CrossList("Min6");
        }

        if (currentString.match("[A-Z]")) {
            CheckList("Cap1");
        }
        else {
            CrossList("Cap1");
        }

        if (currentString.match("[a-z]")) {
            CheckList("Small1");
        }
        else {
            CrossList("Small1");
        }

        if (/^[a-zA-Z0-9- ]*$/.test(currentString) == false) {
            CheckList("Special1");
        }
        else {
            CrossList("Special1");
        }

        if (currentString.match("[0-9]")) {
            CheckList("Digit1");
        }
        else {
            CrossList("Digit1");
        }

        MatchFields("Password", "ConfirmPassword", "PasswordMatch");
    });

    $("#ConfirmPassword").on("keyup", function () {
        MatchFields("Password", "ConfirmPassword", "PasswordMatch");
    });

    function MatchFields(FirstField, SecondField, ListItem)
    {
        if ($("#" + FirstField).val() == $("#" + SecondField).val() && $("#" + SecondField).val().length > 0)
        {
            CheckList(ListItem);
        }
        else
        {
            CrossList(ListItem);
        }
    }

    function CheckList(id)
    {
        if ($("#" + id).hasClass("crossList")) {
            $("#" + id).removeClass("crossList");
        }
        if (!$("#" + id).hasClass("checkList")) {
            $("#" + id).addClass("checkList");
        }
    }

    function CrossList(id) {
        if (!$("#" + id).hasClass("crossList")) {
            $("#" + id).addClass("crossList");
        }
        if ($("#" + id).hasClass("checkList")) {
            $("#" + id).removeClass("checkList");
        }
    }

    jQuery.validator.addMethod("ValidEmail", function (value, element) {
        //return value.indexOf("+") < 0 && value != "" && value.indexOf(" ") < 0;
        var pattern = /^([a-z\d\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
        return pattern.test(value);
    }, "(Please enter a valid Email Address)");

    $("#registerForm").validate({
        rules: {
            FirstName: {
                required: true,
                maxlength: 256
            },
            LastName: {
                required: true,
                maxlength: 256
            },
            Email: {
                required: true,
                maxlength: 512,
                ValidEmail: true
            },
            Password: {
                required: true,
                minlength: 6,
                maxlength: 512
            },
            ConfirmPassword: {
                required: true,
                maxlength: 512
            },
            PhoneNumber: {
                required: true
            },
            CountryId: {
                required: true
            }
        },
        messages: {
            FirstName: {
                required: "(First Name is Required)"
            },
            LastName: {
                required: "(Last Name is Required)"
            },
            Email: {
                required: "(Email is Required)"
            },
            Password: {
                required: "(Password is Required)",
                minlength: "(Minimum 6 characters are required)"
            },
            ConfirmPassword: {
                required: "(Confirm Password is Required)"
            },
            PhoneNumber: {
                required: "(Phone number is required)"
            },
            CountryId: {
                required: "(Country is Required)"
            }
        },
        submitHandler: function (form) {
            var PayPalAgreement = $("#PayPalAgreement").is(':checked');
            var PayGenieAgreement = $("#PayGenieAgreement").is(':checked');

            if (PayPalAgreement) {

            } else {
                // Show notification
                if (!PayGenieAgreement) {
                    swal({
                        title: "Error!",
                        text: "You have to accept PayGenie and PayPal Agreement for Registration.",
                        type: "error"
                    });
                    return false;
                }
                swal({
                    title: "Error!",
                    text: "You have to accept PayPal Agreement for Registration.",
                    type: "error"
                });
                return false;
            }

            if (PayGenieAgreement) {

            } else {
                // Show notification
                swal({
                    title: "Error!",
                    text: "You have to accept PayGenie Terms and Conditions for Registration.",
                    type: "error"
                });
                return false;
            }
            form.submit();
        },
        errorPlacement: function (error, element) {
            $(element)
                    .closest("form")
                    .find("label[for='" + element.attr("id") + "']")
                    .append(error);
        },
        errorElement: "span",
    });
});