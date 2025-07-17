const controlador = siteRoot + 'DemandaPO/CargaDatos/'
const imageRoot = siteRoot + "Content/Images/";
const controladorHilo = siteRoot + 'DemandaPO/Hilo/'

$(document).ready(function () {
    $('#filtroFecha').Zebra_DatePicker();
    $('#txtFecha').Zebra_DatePicker();
    
    $('#txtFecMes').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime",
        hourFormat: "24"
    });

    $('#txtFecProcess').inputmask({
        mask: "1/2/y h:s",
        placeholder: "dd/mm/yyyy hh:mm",
        alias: "datetime",
        hourFormat: "24"
    });

    $('#txtFecProcessHora').inputmask({
        mask: "1/2/y h:s",
        placeholder: "dd/mm/yyyy hh:mm",
        alias: "datetime",
        hourFormat: "24"
    });

    $('#txtFecProcess30').inputmask({
        mask: "1/2/y h:s",
        placeholder: "dd/mm/yyyy hh:mm",
        alias: "datetime",
        hourFormat: "24"
    });

    $('#btnProcesarAutomaticamente1Mes').click(function () {
        const date = $('#txtFecMes').val();
        if (!date) {
            alert("Debe ingresar una fecha");
            return false;
        }

        procesarAutomaticamente1Mes();
    });

    $('#btnProcesarAutomaticamente1').click(function () {
        const date = $('#txtFecProcess').val();
        if (!date) {
            alert("Debe ingresar una fecha");
            return false;
        }

        procesarAutomaticamentePor1Minuto();
    });

    $('#btnProcesarAutomaticamente1hora').click(function () {
        const date = $('#txtFecProcessHora').val();
        if (!date) {
            alert("Debe ingresar una fecha");
            return false;
        }

        procesarAutomaticamentePor1Hora();
    });

    $('#btnProcesarAutomaticamente30').click(function () {
        const date = $('#txtFecProcess30').val();
        if (!date) {
            alert("Debe ingresar una fecha");
            return false;
        }

        procesarAutomaticamentePor30Minutos();
    });

    $('#btnConsultar').click(function () {
        const date = $('#filtroFecha').val();
        if (!date) {
            alert("Debe ingresar una fecha");
            return false;
        }

        consultarEstados();
    });

    $('#btnProcesarManualmente').click(function () {
        const date = $('#txtFecha').val();
        if (!date) {
            alert("Debe ingresar una fecha");
            return false;
        }

        const dir = $('#txtDireccion').val();
        if (!dir) {
            alert("Debe ingresar una dirección valida");
            return false;
        }
        procesarManualmente();
    });

    $('#btnExportar').click(function () {
        generarExcelRawsNoProcesados();
    });

    $('#btnCargaManual').on('click', function () {
        popAgregar = $('#popup').bPopup({
            easing: 'easeOutBack',
            speed: 350,
            transition: 'fadeIn',
            modalClose: false,
            onOpen: function () {

            }
        });

    });

    $('#popCancelar').click(function () {
        $('#popup').bPopup().close();
    });

    $('#btnVerificarDiaCompleto').click(function () {
        const date = $('#filtroFecha').val();
        if (!date) {
            alert("Debe ingresar una fecha");
            return false;
        }

        verificarDiaCompleto();
    });

    $('#btnUpdateRAW').click(function () {
        const date = $('#filtroFecha').val();
        if (!date) {
            alert("Debe ingresar una fecha, para obtener el mes");
            return false;
        }
        updateRAW();
    });

    $('#btnUpdateTMP').click(function () {
        updateTMP();
    });

    $('#btnDeleteEstimadorRawTemporal').click(function () {
        const date = $('#filtroFecha').val();
        if (!date) {
            alert("Debe ingresar una fecha, para obtener el mes");
            return false;
        }
        deleteEstimadorRawTemporal();
    });

    $('#btnExportarHora').on('click', function () {
        popHora = $('#popupHora').bPopup({
            easing: 'easeOutBack',
            speed: 350,
            transition: 'fadeIn',
            modalClose: false,            
        });
    });

    $('#popSelFecha').Zebra_DatePicker();
    $('#btnPopExportarxHora').click(function () {
        exportarDatosxHora();
    });
});

function procesarAutomaticamentePor1Minuto () {
    const date = $('#txtFecProcess').val();

    $.ajax({
        type: 'POST',
        url: controladorHilo + 'ProcesarAutomaticamentePorMinuto',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            strFecProcess: date
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result == "1") {

                alert("El proceso se ejecuto correctamente para el dia seleccionado");
                console.log(result, '_result');
            }
            else {
                alert(result);
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

function procesarAutomaticamentePor30Minutos() {
    const date = $('#txtFecProcess30').val();

    $.ajax({
        type: 'POST',
        url: controladorHilo + 'ProcesarAutomaticamentePor30Minutos',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            strFecProcess: date
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result == "1") {

                alert("El proceso se ejecuto correctamente para el dia seleccionado");
                console.log(result, '_result');
            }
            else {
                alert(result);
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

function procesarManualmente() {
    const date = $('#txtFecha').val();
    const dir = $('#txtDireccion').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'ProcesarManualmente',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            fechaImportacion: date,
            direccion: dir
        }),
        datatype: 'json',
        traditional: true,
        success: function (res) {
            console.log(res, '_res');
            SetMessage('#message', res.msg, res.type, false);
        },
        error: function () {
            mostrarError();
        }
    });
}

function consultarEstados() {
    const date = $('#filtroFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'consultarEstados',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            fecProceso: date
        }),
        datatype: 'json',
        traditional: true,
        success: function (res) {
            crearBarraEstados(res);
        },
        error: function () {
            mostrarError();
        }
    });
}

function generarExcelRawsNoProcesados() {
    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarExcelRawsNoProcesados',
        contentType: 'application/json; charset=utf-8',
        data: {},
        dataType: 'json',
        success: function (result) {
            if (result == -2) {
                alert("No se encuentra datos a exportar!")
            }
            else if (result != -1) {
                document.location.href = controlador + 'Descargar?file=' + result;
            }
            else {
                alert("Ha ocurrido un error");
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function crearBarraEstados(data) {
    data.forEach(e => {
        const elem = $(`#${e.id}`);

        elem.html("");

        e.array
            .forEach((item, index) => {
                let className = 'status-bar-item';

                if (item === "0")
                    className += ' status-error';
                if (item === "1")
                    className += ' status-done';
                if (item === "2")
                    className += ' status-pending';

                const str = `<div id="${e.tipo}_${(index + 1)}"`
                    + `class="${className}"`
                    + `title="${(e.arrayNombre[index])}"`
                    + `style="width:1px;"`
                    + `></div>`;

                elem.append(str);
            });
    });
}

function mostrarError() {
    alert("Ha ocurrido un error.....");
}

//Configura el mensaje principal
function SetMessage(container, msg, tpo, del) {
    //{Contenedor, mensaje(string), tipoMensaje(string), delay(bool)}
    var new_class = "msg-" + tpo; //Identifica la nueva clase css
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

function verificarDiaCompleto() {
    const date = $('#filtroFecha').val();
    $.ajax({
        type: 'POST',
        url: controladorHilo + 'verificarFaltantesDia',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            fecProceso: date
        }),
        datatype: 'json',
        traditional: true,
        success: function (res) {
            if (res > 0) {
                let opcion = confirm("Faltan " + res + " rango de hora, desea completar las horas faltantes, Esta operación puede tardar varios minutos.");
                if (opcion == true) {
                    completarDia(date);
                }
            }
            else
                alert("No existen rango de horas pendiente de migración.");
        },
        error: function () {
            mostrarError();
        }
    });
}

function completarDia(date) {
    $.ajax({
        type: 'POST',
        url: controladorHilo + 'completarDia',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            fecProceso: date
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result > 0) {
                alert("Se procesaron correctamente " + result + " rangos para el dia seleccionado");
            }
            else {
                alert(result);
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

function procesarAutomaticamentePor1Hora() {
    const date = $('#txtFecProcessHora').val();

    $.ajax({
        type: 'POST',
        url: controladorHilo + 'ProcesarAutomaticamentePorHora',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            strFecProcessHora: date
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result == "1") {
                alert("El proceso se ejecuto correctamente para la hora seleccionada");
                console.log(result, '_result');
            }
            else {
                alert(result);
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

function updateRAW() {
    const date = $('#filtroFecha').val();
    $.ajax({
        type: 'POST',
        url: controladorHilo + 'updateRAW',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            fecProceso: date
        }),
        datatype: 'json',
        traditional: true,
        success: function (res) {
            if (res == "OK") {
                alert("La solicitud sae ejecuto correctamente.");
            }
            else
                alert(res);
        },
        error: function () {
            mostrarError();
        }
    });
}

function updateTMP() {
    $.ajax({
        type: 'POST',
        url: controladorHilo + 'updateTMP',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        traditional: true,
        success: function (res) {
            if (res == "OK") {
                alert("La solicitud sae ejecuto correctamente.");
            }
            else
                alert(res);
        },
        error: function () {
            mostrarError();
        }
    });
}

function deleteEstimadorRawTemporal() {
    const date = $('#filtroFecha').val();
    $.ajax({
        type: 'POST',
        url: controladorHilo + 'DeleteEstimadorRawTemporal',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            fecProceso: date
        }),
        datatype: 'json',
        traditional: true,
        success: function (res) {
            if (res == "OK") {
                alert("La solicitud sae ejecuto correctamente.");
            }
            else
                alert(res);
        },
        error: function () {
            mostrarError();
        }
    });
}

function procesarAutomaticamente1Mes() {
    const date = $('#txtFecMes').val();

    $.ajax({
        type: 'POST',
        url: controladorHilo + 'ProcesarAutomaticamentePorMes',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            strFecProcess: date
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result == "1") {
                alert("El proceso se ejecuto correctamente para el mes seleccionado");
                console.log(result, '_result');
            }
            else {
                alert(result);
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

function exportarDatosxHora() {
    const fecha = $('#popSelFecha').val();
    const hora = $('#popSelHora').val();
    const tipo = $('#popSelTipo').val();

    if (!fecha) return;
    if (!hora) return;

    $.ajax({
        type: 'POST',
        url: controlador + 'exportarDatosxHora',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            regFecha: fecha,
            regHora: hora,
            selTipo: tipo,
        }),
        datatype: 'json',
        traditional: true,
        success: function (res) {
            if (res.valid === 1) {
                window.location = `${controlador}Descargar?file=${res.response}`;   
            }                
            if (res.valid === -1) {
                alert(res.response);
            }                   
        },
        error: function () {
            alert('Ha ocurrido un error...');
        }
    });


}