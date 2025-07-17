var name = "ConfiguracionEstimador/";
var controller = siteRoot + "PronosticoDemanda/" + name;
var imageRoot = siteRoot + "Content/Images/";

$(document).ready(function () {
    ////Control del menu de opciones
    $('.options').click(function () {
        const id = this.lastElementChild.id;
        $(this).addClass('selected').siblings().removeClass('selected');
        ValidateSelectedOption(id);
    });

    Start();
});

//Inicia el modulo pre-configurado
function Start() {
    const arrayMenu = $('.options');
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
function ValidateSelectedOption(option) {
    const module = `por${option.split("opt-")[1]}`;//nombre ActionResult a mostrar
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