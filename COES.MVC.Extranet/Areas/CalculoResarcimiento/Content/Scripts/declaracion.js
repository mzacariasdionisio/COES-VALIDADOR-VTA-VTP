var controlador = siteRoot + 'calculoresarcimiento/declaracion/';

var tblEnvio;

$(function () {

    $('#txtAnio').Zebra_DatePicker({
        format: 'Y',
        onSelect: function (date) {
            cargarPeriodos(date)
        }
    });

    $('#btnConsultar').on('click', function () {
        mostrarDeclaracion(null);
    });

    $('#btnEnviarDatos').on('click', function () {
        enviarDatos();
    });

    $('#spanCambiarEmpresa').on('click', function () {
        cambiarEmpresa();
    });

    $("#btnVerEnvios").click(function () {
        abrirPopup("enviosDeclaracion");
    });

    tblEnvio = $('#tablalenvio').DataTable({
        data: [],
        columns: [
            { data: "Reenvcodi", width: "30%" },            
            { data: "Reenvusucreacion", width: "30%" },
            { data: "ReenvfechaDesc", width: "40%" },
            { data: "ReenvindicadorDesc", width: "30%" }
        ],
        rowCallback: function (row, data) { },
        filter: false,
        info: false,
        processing: true,
        scrollCollapse: true,
        paging: false,
        autoWidth: true,
        ordering: false
    });

    $('#tablalenvio tbody').on('click', 'tr', function () {
        var data = tblEnvio.row(this).data();
        var idEnvio = data.Reenvcodi;
        cerrarPopup('enviosDeclaracion');
        mostrarDeclaracion(idEnvio);
    });

});

function mostrarDeclaracion(declaracionId) {
    limpiarBarraMensaje("mensaje");
    var defecto = -1;
    if (declaracionId == null) {  //Desde boton Consultar
        if ($('#hfIdEmpresa').val() != "") {
            if ($('#cbPeriodo').val() != "-2") {

                $('#hdEmpresa').val($('#hfIdEmpresa').val());
                $('#hdPeriodo').val($('#cbPeriodo').val());
                consultar(defecto)
            }
            else {
                mostrarMensaje('mensaje', 'alert', 'Debe seleccionar periodo.');
                habilitarEnvio(false);
            }
        }
        else {
            mostrarMensaje('mensaje', 'alert', 'Su usuario no se encuentra asociado a una empresa.');
            habilitarEnvio(false);
        }
    } else {  //Desde Ver Envios
        $('#hdEmpresa').val(defecto);
        $('#hdPeriodo').val(defecto);
        consultar(declaracionId);
    }
}

function consultar(declaracionId) {
    limpiarBarraMensaje("mensaje");

    var emp = $('#hdEmpresa').val();
    var per = $('#hdPeriodo').val();
    $.ajax({
        type: 'POST',
        url: controlador + 'consultar',
        data: {
            empresa: emp,
            periodo: per,
            declaracionId: declaracionId
        },
        dataType: 'json',
        global: false,
        success: function (evt) {
            if (evt.Resultado == "1") {

                //habilito los botones y la declaracion
                habilitarEnvio(true);

                //limpio y seteo el valor del indicador                
                var indicador = evt.Indicador;
                if (indicador == "") {
                    clearRadioButtons();
                } else {

                    if (indicador == "S") {
                        let radion = document.getElementById("rbt_si");
                        radion.checked = true;
                    } else {


                        if (indicador == "N") {
                            let radion = document.getElementById("rbt_no");
                            radion.checked = true;
                        }
                    }
                }
                
                //lleno los envios
                var lstEnvio = evt.ListaEnvios;
                tblEnvio.clear().draw();
                tblEnvio.rows.add(lstEnvio).draw();
            }
            else {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
        
}

function clearRadioButtons() {
    var ele = document.querySelectorAll("input[type=radio]");
    for (var i = 0; i < ele.length; i++) {
        ele[i].checked = false;
    }
}

function cargarPeriodos(anio) {
    $.ajax({
        type: 'POST',
        url: controlador + 'obtenerperiodos',
        data: {
            anio: anio
        },
        dataType: 'json',
        global: false,
        success: function (result) {
            if (result != -1) {
                $('#cbPeriodo').get(0).options.length = 0;
                $('#cbPeriodo').get(0).options[0] = new Option("--SELECCIONE--", "-2");
                $.each(result, function (i, item) {
                    $('#cbPeriodo').get(0).options[$('#cbPeriodo').get(0).options.length] = new Option(item.Repernombre, item.Repercodi);
                });
            }
            else {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

function cambiarEmpresa() {
    limpiarBarraMensaje("mensaje");
    $.ajax({
        type: 'POST',
        url: controlador + 'empresa',
        success: function (evt) {
            $('#contenidoEmpresa').html(evt);            

            abrirPopup("popupEmpresa");

            $('#btnAceptar').on("click", function () {
                seleccionarEmpresa();
            });

            $('#btnCancelar').on("click", function () {
                cerrarPopup('popupEmpresa');
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

function seleccionarEmpresa() {
    console.log($('#cbEmpresa').val());
    if ($('#cbEmpresa').val() != "") {
        $('#hfIdEmpresa').val($('#cbEmpresa').val());
        $('#lbEmpresa').text($("#cbEmpresa option:selected").text());
        cerrarPopup('popupEmpresa');        

        $('#cbPeriodo').val("-2");
        habilitarEnvio(false);        
    }
    else {
        mostrarMensaje('mensajeEdicion', 'alert', 'Debe seleccionar empresa.');
    }
}

function habilitarEnvio(flag) {    
    if (flag) {
        $('#divEnviar').show();
        $('#divVerEnvios').show();
        $('#bloquedeclaracion').show();        
    }
    else {
        $('#divEnviar').hide();
        $('#divVerEnvios').hide();
        $('#bloquedeclaracion').hide();
    }
}


function enviarDatos() {
    limpiarBarraMensaje("mensaje");
    var check = document.querySelector('input[name="declaracionValor"]:checked');

    if (check != null) {
        var valEscogido = check.value;

        $.ajax({
            type: 'POST',
            url: controlador + 'enviar',
            data: {
                empresa: $('#hfIdEmpresa').val(),
                periodo: $('#cbPeriodo').val(),
                valor: valEscogido
            },
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado == "1") {                    
                    mostrarMensaje('mensaje', 'exito', 'Se guardó la información correctamente.');
                    var lstEnvio = evt.ListaEnvios;
                    tblEnvio.clear().draw();
                    tblEnvio.rows.add(lstEnvio).draw();
                }
                else {
                    mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'Debe seleccionar una opción a la consulta planteada.');
    }
}

////////////////////////////////////////////////
// Util
////////////////////////////////////////////////
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