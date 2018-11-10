function ConfirmDelete(url)
{
    var r = confirm("Подтвердите удаление");
    if (r) {
        window.location.href = url;
    }
}

function ConfirmSetBalance(url) {
	var r = confirm("Указанное значение будет принято как новый баланс склада. Вы подтверждаете действие?");
	if (r) {
		window.location.href = url;
	}
}

function logout() {
    try {
        document.execCommand("ClearAuthenticationCache");
        var agt = navigator.userAgent.toLowerCase();
        if (agt.indexOf("msie") !== -1) {
            document.execCommand("ClearAuthenticationCache", "false");
        }
        //window.crypto is defined in Chrome, but it has no logout function
        else if (window.crypto && typeof window.crypto.logout !== "function") {
            window.crypto.logout();
        }
        else {
            //window.location = "/page/to/instruct/the/user/to/close/the/browser";
        }
        window.close();
        open(location, '_self').close();
    }
    catch (e) {
        window.close();
        open(location, '_self').close();
    }
}

$(function () {
	$('#datetimepicker').datetimepicker({
		locale: 'ru',
        //daysOfWeekDisabled: [], //Заполнить дефолтную дату
		format: "YYYY-MM-DD hh:mm"
    });
});

function InsuficientRights() {
    alert("Недостаточно прав доступа");
}
