const controlador = siteRoot + 'CPPA/RegistroParametrosAjuste/';
const imageRoot = siteRoot + "Content/Images/";

$(document).ready(function () {
    $('.nt-header-option.options').click(function () {
        const id = this.id;
        $(this).addClass('active')
            .siblings()
            .removeClass('active');
        ValidateSelectedOption(id);
    });

    $('#indexFecha').Zebra_DatePicker({
        format: 'Y',
        view: 'years'
    });

    $('#btnRegresar').on('click', function () {
        var url = siteRoot + `CPPA/AjustePresupuestal/index/`;
        window.location.href = url
    });

    Start();
});

//Inicia el modulo pre-configurado
function Start() {
    const arrayMenu = $('.options');
    $.each(arrayMenu, function (i, item) {
        if ($(item).hasClass('active')) {
            ValidateSelectedOption(item.id);
            return false;
        }
    });
}

//Valida que modulo se va a mostrar
function ValidateSelectedOption(option) {
    const rev = $('#indexIdRevision').val();

    const module = `${option.split("opt-")[1]}`;//nombre PartialActionResult a mostrar
    $.ajax({
        type: 'POST',
        url: controlador + module,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            revision: rev
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            //$('.Zebra_DatePicker').remove();//Elimina los remanentes del Zebra_DatePicker al cambiar de partialView
            $('.general-popup').remove();//Elimina los remanentes del Popup al cambiar de partialView
            $('#module').html(result);//Carga el html del nuevo partialView
            //SetMessage('#message', $('#main').data('msg'), $('#main').data('tpo'));
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
    if (del) $(container).show(0).delay(15000).hide(0);//15 Segundos
    else $(container).show();
}