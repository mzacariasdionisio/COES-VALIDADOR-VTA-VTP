var name = "Depuracion/";
var controller = siteRoot + "PronosticoDemanda/" + name;
var imageRoot = siteRoot + "Content/Images/";
var test;
var pop;
var pop_reporte;

$(document).ready(function () {
    ////Control del menu de opciones
    $('.options').click(function () {
        var id = this.lastElementChild.id;
        $(this).addClass('selected').siblings().removeClass('selected');
        ValidateSelectedOption(id);
    });

    Start();
});

//Inicia el modulo pre-configurado
function Start() {
    var arrayMenu = $('.options');
    $.each(arrayMenu, function (i, item) {
        if ($(item).hasClass('selected')) {
            ValidateSelectedOption(item.lastElementChild.id);
            return false;
        }
    });

    $('#btnOcultarMenu').on('click', function () {
        ResizeCharts();
    });
}

//Valida que modulo se va a mostrar
function ValidateSelectedOption(opt) {
    var module = opt.split("opt-")[1];//nombre ActionResult a mostrar
    $.ajax({
        type: 'POST',
        url: controller + module,
        success: function (result) {
            $('.Zebra_DatePicker').remove();//Elimina los remanentes del Zebra_DatePicker al cambiar de partialView
            $('.general-popup').remove();//Elimina los remanentes del Popup al cambiar de partialView
            $('#module').html(result);//Carga el html del nuevo partialView
            SetMessage('#message', $('#main').data('msg'), $('#main').data('tpo'));
        },
        error: function () {
            SetMessage('#message', 'Ha ocurrido un problema...', 'error');
            $('#module').html('');
        }
    });
}

//Configura el mensaje principal
function SetMessage(container, msg, tpo, del) {//{Contenedor, mensaje(string), tipoMensaje(string), delay(bool)}
    var new_class = "msg-" + tpo;//Identifica la nueva clase css
    $(container).removeClass($(container).attr('class'));//Quita la clase css previa
    $(container).addClass(new_class);
    $(container).html(msg);//Carga el mensaje
    //$(container).show();

    //Focus to message
    $('html, body').animate({ scrollTop: 0 }, 5);

    //Mensaje con delay o no
    if (del) $(container).show(0).delay(5000).hide(0);//5 Segundos
    else $(container).show();
}

//Redimensiona el elemento highchart
function ResizeCharts() {
    if (!hc) return;
    hc.reflow();
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