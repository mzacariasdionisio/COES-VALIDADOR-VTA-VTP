var controlador = siteRoot + 'PrimasRER/SolicitudEDI/';
const imageRoot = `${siteRoot}PrimasRER/Content/Images/`;

$(document).ready(function () {

    $("#selEmpresa").multipleSelect({
        filter: false,
        single: true,
        placeholder: 'Seleccione...',
        onClose: function () {
            console.log("_close");
        }
    });

    $("#selPeriodo").multipleSelect({
        filter: false,
        single: true,
        placeholder: 'Mes - Año',
        onClose: function () {
            console.log("_close");
        }
    });

    $('#cbPeriodo').change(function () {
        ipericodi = $(this).val();
        limpiarMensaje();
        validarPeriodo($("#cbPeriodo").val());
    });

    $('#btnNuevo').click(function () {
        nuevo();
    });

    validarPeriodo(ipericodi);
});

//Seleccionar / Cambiar Empresa
seleccionarEmpresa = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "EscogerEmpresa",
        success: function (evt) {
            $('#popup').html(evt);
            setTimeout(function () {
                $('#popup').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
        },
        error: function () {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

accesoBotonNuevo = function (flag) {
    if (flag) {
        $("#btnNuevo").show();
    } else {
        $("#btnNuevo").hide();
    }
}

mostrarListado = function () {
    if (sEmprnomb !== null && sEmprnomb !== undefined && sEmprnomb !== '') {
        $.ajax({
            type: 'POST',
            url: controlador + "listado",
            data: {
                ipericodi: $('#cbPeriodo').val()
            },
            success: function (evt) {
                $('#listado').css("width", $('#mainLayout').width() + "px");
                $('#listado').html(evt);
                addDeleteEvent();
                viewEvent();
                $('#tabla').dataTable({
                    scrollY: 430,
                    scrollX: true,
                    ordering: false,
                    paging: true,
                    pageLength: 15,
                    searching: true,
                    searchDelay: 500,
                    info: true,
                    iDisplayLength: 50
                });
            },
            error: function () {
                mostrarError("Lo sentimos, ha ocurrido un error inesperado");
            }
        });
    } else {
        mostrarMensaje("Por favor seleccione una empresa");
    }
}

nuevo = function () {
    window.location.href = controlador + "new?ipericodi=" + ipericodi;
}

//Funciones de eliminado de registro
addDeleteEvent = function () {
    $(".delete").click(function (e) {
        e.preventDefault();
        if (confirm("¿Está seguro que desea eliminar la solicitud EDI?")) {
            rersedcodi = $(this).attr("id").split("_")[1];
            $.ajax({
                type: "post",
                dataType: "text",
                url: controlador + "Delete",
                data: addAntiForgeryToken({ rersedcodi: rersedcodi }),
                success: function (resultado) {
                    if (resultado == "true") {
                        $("#fila_" + rersedcodi + "_").remove();
                        mostrarExito("Se ha eliminado correctamente el registro");
                    }
                    else {
                        mostrarError("No se ha logrado eliminar el registro: ");
                    }
                }
            });
        }
    });
};

//Funciones de vista detalle
viewEvent = function () {
    $('.view').click(function () {
        rersedcodi = $(this).attr("id").split("_")[1];
        abrirPopup(rersedcodi);
    });
};

abrirPopup = function (rersedcodi) {
    $.ajax({
        type: 'POST',
        url: controlador + "View",
        data: { rersedcodi: rersedcodi },
        success: function (evt) {
            $('#popup').html(evt);
            setTimeout(function () {
                $('#popup').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
        },
        error: function () {
            mostrarError("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

addAntiForgeryToken = function (data) {
    data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
    return data;
};

function validarPeriodo(ipericodi) {
    $.ajax({
        type: 'POST',
        url: controlador + 'ValidarPeriodo',
        dataType: 'json',
        data: {
            ipericodi: ipericodi
        },
        success: function (result) {
            const sMensajeError = result.sMensajeError;
            const bAccion = result.bAccion;
            if (sMensajeError != null && sMensajeError != "") {
                mostrarAlerta(sMensajeError);
                accesoBotonNuevo(false);
            } else {
                accesoBotonNuevo(bAccion);
            }
            mostrarListado();
        },
        error: function () {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

function descargarFormato() {
    limpiarMensaje();
    let iMesPeriodoEDI = parseInt($("#iperimes").val(), 10);
    let iAnioPeriodoEDI = parseInt($("#iperianio").val(), 10); 
    const fechainicio = $('#Fechainicio').val().split(" ")[0];
    const horainicio = $('#Fechainicio').val().split(" ")[1];
    const fechafin = $('#Fechafin').val().split(" ")[0];
    const horafin = $('#Fechafin').val().split(" ")[1];
    $.ajax({
        type: 'POST',
        url: controlador + 'ExportarEnergiaUnidadExcel',
        contentType: 'application/json;',
        data: JSON.stringify({
            iMesPeriodoEDI: iMesPeriodoEDI,
            iAnioPeriodoEDI: iAnioPeriodoEDI, 
            rersedcodi: $('#EntidadSolicitudEDI_Rersedcodi').val(),
            rercencodi: $('#EntidadSolicitudEDI_Rercencodi').val(),
            fechainicio: fechainicio,
            horainicio: horainicio,
            fechafin: fechafin,
            horafin: horafin
        }),
        datatype: 'json',
        success: function (result) {
            if (result.sMensajeError != "") {
                mostrarError(result.sMensajeError);
            }
            else {
                window.location = controlador + 'abrirarchivo?nombreArchivo=' + result.nombreArchivo;
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function descargarSustento() {
    limpiarMensaje();
    let nomSustento = $('#EntidadSolicitudEDI_Rersedsustento').val();
    if (nomSustento === "") {
        alert("No se ha encontrado ningún archivo");
    } else {
        window.location = controlador + 'abrirarchivo?nombreArchivo=' + nomSustento + '&tipoRuta=' + 2;
    }
}

//Funciones para mostrar mensajes
mostrarExito = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-exito');
}

mostrarError = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-error');
}

mostrarAlerta = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-alert');
}

mostrarMensaje = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-message');
}

limpiarMensaje = function () {
    $('#mensaje').removeClass();
    $('#mensaje').html("");
}