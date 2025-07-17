$(document).ready(function () {
    //Control de las sub-opciones
    $('.suboptions').click(function () {
        var id = this.lastElementChild.id;
        $(this).addClass('selected').siblings().removeClass('selected');
        ValidateSelectedSuboption(id);
    });

    StartSuboption();
});

//Valida que opción se va a mostrar
function ValidateSelectedSuboption(opt) {
    var option = 'parametros' + opt;//nombre ActionResult a mostrar
    $.ajax({
        type: 'POST',
        url: controller + option,
        success: function (result) {
            $('.Zebra_DatePicker').remove();//Elimina los remanentes del Zebra_DatePicker al cambiar de partialView
            $('.general-popup').remove();//Elimina los remanentes del Popup al cambiar de partialView
            $('#submodule').html(result);//Carga el html del nuevo partialView
            SetMessage('#message', $('#main').data('msg'), $('#main').data('tpo'));
        },
        error: function () {
            SetMessage('#message', 'Ha ocurrido un problema...', 'error');
            $('#submodule').html('');
        }
    });
}

function StartSuboption() {
    var arrayMenu = $('.suboptions');
    $.each(arrayMenu, function (i, item) {
        if ($(item).hasClass('selected')) {
            ValidateSelectedSuboption(item.lastElementChild.id);
            return false;
        }
    });
}

//Agrega una dia a una fecha(resultado en string)
function AddDays(value) {
    var ndias = 1;
    var dt = ConvertStringToDate(value);
    dt.setDate(dt.getDate() + ndias);

    var dd = (dt.getDate() < 10 ? '0' : '') + dt.getDate();
    var MM = ((dt.getMonth() + 1) < 10 ? '0' : '') + (dt.getMonth() + 1);
    var yyyy = dt.getFullYear();

    var dtf = dd + '/' + MM + '/' + yyyy;

    return dtf;
}

//Valida las fechas ingresadas
function ValidateDates(fecini, fecfin) {
    var valid = false;
    var fini = ConvertStringToDate(fecini);
    var ffin = ConvertStringToDate(fecfin);

    if (ffin.getTime() >= fini.getTime()) {
        valid = true;
    }

    return valid;
}

//Convierte un tipo string a tipo date
function ConvertStringToDate(value) {
    var pattern = /^(\d{1,2})\/(\d{1,2})\/(\d{4})$/;
    var arrayDate = value.match(pattern);
    var dt = new Date(arrayDate[3], arrayDate[2] - 1, arrayDate[1]);

    return dt;
}

//Convierte un arreglo de Strings a String
function ConvertStringArrayToString(list) {
    console.log(list, 'da');
    var res = '0';
    if (list) {
        $.each(list, function (i, item) {
            res += item + ',';
        });
        res = res.slice(0, -1);
    }
    return res;
}




