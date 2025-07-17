var controlador = siteRoot + 'CalculoResarcimiento/Periodo/';

$(function () {
    
    $('#btnRegresar').click(function () {
        var url = siteRoot + 'CalculoResarcimiento/Periodo/Index';
        window.open(url, "_self").focus();
    });

    $('#btnGrabar').click(function () {
        guardarHabilitacion();
    });
    
    $('#tablaHabilitacion').dataTable({
        "scrollY": 430,
        "scrollX": false,
        "sDom": 'ft',
        "ordering": false,
        "iDisplayLength": -1
    });

    //Toda la columna cambia (al escoger casilla cabecera)
    $('input[type=checkbox][name^="checkTodo_IS"]').unbind();
    $('input[type=checkbox][name^="checkTodo_IS"]').change(function () {        
        var valorCheck = $(this).prop('checked');        
        $("input[type=checkbox][id^=checkIS_]").each(function () {
            $("#" + this.id).prop("checked", valorCheck);
        });
    });

    //Toda la columna cambia (al escoger casilla cabecera)
    $('input[type=checkbox][name^="checkTodo_RC"]').unbind();
    $('input[type=checkbox][name^="checkTodo_RC"]').change(function () {        
        var valorCheck = $(this).prop('checked');        
        $("input[type=checkbox][id^=checkRC_]").each(function () {
            $("#" + this.id).prop("checked", valorCheck);
        });
    });

    //Verifico si el check cabecera debe pintarse o no al editar cualquier check hijo
    $('input[type=checkbox][name^="checkRC_"]').change(function () {
        verificarCheckGrupalRC();
    });

     //Verifico si el check cabecera debe pintarse o no al editar cualquier check hijo
    $('input[type=checkbox][name^="checkIS_"]').change(function () {
        verificarCheckGrupalPE();
    });
    
    verificarCheckGrupalPE();

    verificarCheckGrupalRC();
    

});

function verificarCheckGrupalPE() {
    //Empresas Interrupcion Suministro con check
    var val1 = 0;
    $("input[type=checkbox][id^=checkIS_]").each(function () {
        var IsCheckedIS = $("#" + this.id)[0].checked;
        if (IsCheckedIS) {
            
        } else {
            val1 = val1 + 1;
        }
    });

    var v = true;
    if (val1 > 0)
        v = false;

    $("#checkTodo_IS").prop("checked", v);
}

function verificarCheckGrupalRC() {
    var val2 = 0;
    $("input[type=checkbox][id^=checkRC_]").each(function () {
        var IsCheckedRC = $("#" + this.id)[0].checked;
        if (IsCheckedRC) {
            //val2 = ;
        } else {
            val2 = val2 + 1;
        }
    });

    var v = true;
    if (val2 > 0)
        v = false;

    $("#checkTodo_RC").prop("checked", v);
}

function guardarHabilitacion() {
    limpiarBarraMensaje("mensaje");

    var datos = {};
    datos = getDatos();
    
    $.ajax({
        type: 'POST',
        url: controlador + 'guardarHabilitacion',
        contentType: "application/json",
        dataType: 'json',
        data: JSON.stringify({
            repercodi: datos.PeriodoId,
            lstIS: datos.ListaIS,
            lstRC: datos.ListaRC
        }
        ),
        cache: false,
        success: function (evt) {
            if (evt.Resultado != "-1") {
                mostrarMensaje('mensaje', 'exito', "Los datos fueron grabados correctamente.");
            } else {
                mostrarMensaje('mensaje', 'error', evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.' + evt.Mensaje);
        }
    });
    
}

function getDatos() {
    var obj = {};
    var lstIS = [];
    var lstRC = [];

    //Empresas Interrupcion Suministro con check
    $("input[type=checkbox][id^=checkIS_]").each(function () {
        var IsCheckedIS = $("#" + this.id)[0].checked;
        if (IsCheckedIS) {

            var miId = this.id;
            const bloque1 = miId.split('_');
            lstIS.push(bloque1[1]);
        }        
    });

    //Empresas Rechazo Carga con check
    $("input[type=checkbox][id^=checkRC_]").each(function () {
        var IsCheckedRC = $("#" + this.id)[0].checked;
        if (IsCheckedRC) {

            var miId = this.id;
            const bloque1 = miId.split('_');
            lstRC.push(bloque1[1]);
        }
    });


    obj.PeriodoId = $("#hdPeriodoId").val();
    obj.ListaIS = lstIS.join('/');
    obj.ListaRC = lstRC.join('/');


    return obj;
}


function cerrarPopup(id) {
    $("#" + id).bPopup().close()
}

function abrirPopup(contentPopup) {
    setTimeout(function () {
        $("#" + contentPopup).bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
}


function mostrarMensaje(id, tipo, mensaje) {
    $("#" + id).css("display", "block");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);

}

function limpiarBarraMensaje(id) {
    $('#' + id).css("display", "none");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-message');
    $('#' + id).html('');
}