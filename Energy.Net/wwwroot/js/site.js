///////////////////////////////////////////////////////////////////////////
// MISC JAVASCRIPT FUNCTIONS
//////////////////////////////////////////////////////////////////////////

// Page Loading
$(document).ready(function () {
    window.onload = function () {
        $("#pageLoader").fadeOut("fast", function () { $("#pageLoader").remove(); });
    }
});

// Flash Messages
$(".alert-info").fadeOut(4500);
$(".alert-danger").fadeOut(6500);