var name = "DemandaBarras/";
var controller = siteRoot + "PronosticoDemanda/" + name;
var imageRoot = siteRoot + "Content/Images/";

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
    //Mensaje con delay o no
    if (del) $(container).show(0).delay(5000).hide(0);//5 Segundos
    else $(container).show();
}

//Redimensiona el elemento highchart
function ResizeCharts() {
    if (!hc) return;
    hc.reflow();
}

//Llena el contenido de una lista desplegable
function RefillDropDowList(element, data, data_id, data_name, sel_id) {
    //Vacia el elemento
    element.empty();
    //Carga el elemento
    $.each(data, function (i, item) {
        var n_value = i, n_html = item;
        if (data_id) n_value = item[data_id];
        if (data_name) n_html = item[data_name];

        if (sel_id == null) {
            if (i == 0)
                element.append($('<option selected></option>').val(n_value).html(n_html));
            else
                element.append($('<option></option>').val(n_value).html(n_html));
        }
        else {
            if (item[data_id] == sel_id)
                element.append($('<option selected></option>').val(n_value).html(n_html));
            else
                element.append($('<option></option>').val(n_value).html(n_html));
        }
    });

    //Actualiza la lib.multipleselect
    element.multipleSelect('refresh');
}