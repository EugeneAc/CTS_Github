//Для совместимости с IE
String.prototype.includes = function (str) {
    var returnValue = false;

    if (this.indexOf(str) !== -1) {
        returnValue = true;
    }

    return returnValue;
}

function getUrlParameter(sParam, url) {
    var sPageURL = decodeURIComponent(url),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return sParameterName[1] === undefined ? true : sParameterName[1];
        }
    }
}

function setnewdate(element, todate, newtodate, fromdate, newfromdate) {
    var url = $(element).attr('src');
    url = url.replace('to=' + +todate, 'to=' + +newtodate);
    url = url.replace('from=' + +fromdate, 'from=' + +newfromdate);
    $(element).attr('src', url);
}



$('#dashboardrange').on('apply.daterangepicker', function (ev, picker) {
    var newfromdate = picker.startDate;
    var newtodate = picker.endDate;
    $.cookie('fromdate', newfromdate, { path: '/' });
    $.cookie('todate', newtodate, { path: '/' });
    $('iframe').each(function () {
        var fromdate = new Date(+getUrlParameter('from', $(this).attr('src')));
        var todate = new Date(+getUrlParameter('to', $(this).attr('src')));
        setnewdate(this, +todate, +newtodate, +fromdate, +newfromdate);
    });
    var url = window.location.href;
    if (url.includes('Mnemonic')) {
        window.location.href = url;
    }
});


// When the user scrolls down 20px from the top of the document, show the button
window.onscroll = function () { scrollFunction() };

function scrollFunction() {
    var upBtn = document.getElementById("goTopBtn");
    if (upBtn !== null) {
    if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
        document.getElementById("goTopBtn").style.display = "block";
    } else {
        document.getElementById("goTopBtn").style.display = "none";
        }
    }
}

// When the user clicks on the button, scroll to the top of the document
function topFunction() {
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
}

$('#FilterManualInput').click(setupFilters); 
$('#OrderByTransferTimeStampAsc').click(setupFilters); 

function setupFilters() {
    var addUrl = '';
    if ($('#FilterManualInput').is(':checked')) {
        addUrl += '&FilterManualInput=true';
    }
    if ($('#OrderByTransferTimeStampAsc').is(':checked')) {
        addUrl += '&OrderByTransferTimeStampAsc=true';
    }
    window.location.href = $(this).parent().data('url') + addUrl;
}