//Для совместимости с IE
String.prototype.includes = function (str) {
    var returnValue = false;

    if (this.indexOf(str) !== -1) {
        returnValue = true;
    }

    return returnValue;
};

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

function setNewdateToIframes(element, todate, newtodate, fromdate, newfromdate) {
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
        setNewdateToIframes(this, +todate, +newtodate, +fromdate, +newfromdate);
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


// Фильтры на 3 уровне
$('#FilterManualInput').click(setupFilters); 
$('#OrderByTransferTimeStampAsc').click(setupFilters); 
$('#wagonSearchFilter').click(setupFilters); 
$('#ApplyWagonSearchLocationFilter').click(setupFilters); 
$('#WagonNumberFilter').keypress(function (e) {
    if (e.which === 13) {
        setupFilters();
    }
});

function setupFilters() {
    var url = $('#FilterContainer').data('url');
    if ($('#FilterManualInput').is(':checked')) {
        url = setUrlParameter('FilterManualInput', 'true', url);
    }

    if ($('#OrderByTransferTimeStampAsc').is(':checked')) {
        url = setUrlParameter('OrderByTransferTimeStampAsc', 'true', url);
    }

    var searchtext = $('#WagonNumberFilter').val();
    if (searchtext !== "") {
        url = setUrlParameter('&wagonNumberFilter', searchtext, url);
    }

    if ($('#WagonSearchLocationFilter #Mines :selected').length > 0) {
        url = setUrlParameter('locations', $('#WagonSearchLocationFilter #Mines').val(), url);
    }

    window.location.href = url;
}

function setUrlParameter(key, value, url) {

    var baseUrl = url.split('?')[0],
        urlQueryString = '?' + url.split('?')[1],
        newParam = key + '=' + value,
        params = '?' + newParam;

    // If the "search" string exists, then build params from it
    if (urlQueryString !== "?undefined") {
        var updateRegex = new RegExp('([\?&])' + key + '[^&]*');
        var removeRegex = new RegExp('([\?&])' + key + '=[^&;]+[&;]?');

        if (typeof value === 'undefined' || value === null || value === '') { // Remove param if value is empty
            params = urlQueryString.replace(removeRegex, "$1");
            params = params.replace(/[&;]$/, "");

        }  else { // Otherwise, add it to end of query string
            params = urlQueryString + '&' + newParam;
        }
    }

    // no parameter was set so we don't need the question mark
    params = params === '?' ? '' : params;

    return baseUrl + params;
}