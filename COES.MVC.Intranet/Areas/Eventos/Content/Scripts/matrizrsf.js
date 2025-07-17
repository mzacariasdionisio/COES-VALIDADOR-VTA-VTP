var controlador = siteRoot + 'eventos/'

$(function () {

    $('#FechaConsulta').Zebra_DatePicker({
        onSelect: function () {            
            consultar();
        }
    });

    $('#FechaInicio').Zebra_DatePicker({
    });

    $('#FechaFin').Zebra_DatePicker({
    });

    $('#btnConsultar').click(function () {
        consultar();
    });

    $('#btnGrabar').click(function () {
        grabar();
    });

    $('#btnEquipoQuitar').click(function () {
        quitarEquipo();
    });

    $('#btnHoraQuitar').click(function () {
        quitarHora();
    });

    $('#btnExportar').click(function () {
        exportar();
    });

    $('#btnExportarHora').click(function () {
        exportarMediaHora();
    });

    $('#btnAceptarExportacion').click(function () {
        exportarReservaAsignada();
    });

    $('#btnExportarReserva').click(function () {
        $('#popupExportacion').css('display', 'block');
    });

    $('#btnEquipo').click(function () {
        $('#busquedaEquipo').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown'
        });
    });

    $('#btnHora').click(function () {
        cargarHora();      
    });

    cargarBusqueda();
    consultar();
   
});

function closePopupExportacion()
{
    $('#popupExportacion').css('display', 'none');
}

function cargarBusqueda() {
    $.ajax({
        type: "POST",
        url: siteRoot + "eventos/busqueda/index",
        data: {},
        success: function (evt) {
            $('#busquedaEquipo').html(evt);
        },
        error: function (req, status, error) {
            mostrarError();
        }
    });
}

function cargarHora() {     

    $.ajax({
        type: "POST",
        url: controlador + "horasoperacion/cargarhora",        
        success: function (evt) {
            $('#hora').html(evt);
            setTimeout(function () {
                mostrarCargaHora();
            }, 50);
        },
        error: function (req, status, error) {
            mostrarError();
        }
    });

   
}

function mostrarCargaHora()
{
    $('#seleccionHora').bPopup({
        easing: 'easeOutBack',
        speed: 200,
        transition: 'slideDown'
    });
}

function seleccionarEquipo(idEquipo, substacion, equipo, empresa, idEmpresa) {           

    $('#busquedaEquipo').bPopup().close();

    $.ajax({
        type: "POST",        
        url: controlador + "horasoperacion/agregar",
        data: {idEquipo: idEquipo, central: substacion, nombre: equipo, empresa : empresa, idEmpresa: idEmpresa},
        success: function (evt) {
            $('#listado').html(evt);
        },
        error: function (req, status, error) {
            mostrarError();
        }
    });
}

function agregarHora()
{
    if (validateTime($('#txtHoraInicio').val()) && validateTime($('#txtHoraFin').val())) {

        $.ajax({
            type: "POST",
            url: controlador + "horasoperacion/agregarhora",
            data: {
                fecha: $('#FechaConsulta').val(), horaInicio: $('#txtHoraInicio').val(), horaFin: $('#txtHoraFin').val(),
                indicador: $('#hfIndicadorEdicion').val(), indice: $('#hfIndice').val()
            },
            success: function (evt) {
                $('#listado').html(evt);
                $('#seleccionHora').bPopup().close();
            },
            error: function (req, status, error) {
                mostrarError();
            }
        });
    }
    else
    {
        alert("Ingrese fecha correcta.");
    }
}

function grabar() {

    $.ajax({
        type: 'POST',
        url: controlador + 'horasoperacion/grabar',
        data: { fecha: $('#FechaConsulta').val() },
        dataType: 'json',
        cache: false,
        success: function (resultado) {
            if (resultado == 1) {
                mostrarExito();
                consultar();
            }
            else {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });
}


function validar()
{
    var reserva = $('#txtReserva').val();
    var sumas = $('#hfSumas').val();

    var flag = true;

    if (sumas != null) {
        var items = sumas.slice(0, -1);
        var arr = items.split('$');

        for (i = 0; i < arr.length; i++) {
            if (parseFloat(reserva) != parseFloat(arr[i])) {
                flag = false;
                var indice = 1 + i * 4;
                $("#tablaMatriz>tbody tr:last td:eq(" + (indice) + ")").css('background-color', '#FF0000')
                $("#tablaMatriz>tbody tr:last td:eq(" + (indice + 1) + ")").css('background-color', '#FF0000')
                $("#tablaMatriz>tbody tr:last td:eq(" + (indice + 2) + ")").css('background-color', '#FF0000')
                $("#tablaMatriz>tbody tr:last td:eq(" + (indice + 3) + ")").css('background-color', '#FF0000')
            }
            else {
                var indice = 1 + i * 4;
                $("#tablaMatriz>tbody tr:last td:eq(" + (indice) + ")").css('background-color', '#E8F6FF')
                $("#tablaMatriz>tbody tr:last td:eq(" + (indice + 1) + ")").css('background-color', '#E8F6FF')
                $("#tablaMatriz>tbody tr:last td:eq(" + (indice + 2) + ")").css('background-color', '#E8F6FF')
                $("#tablaMatriz>tbody tr:last td:eq(" + (indice + 3) + ")").css('background-color', '#E8F6FF')
            }
        }
    }
}

function consultar()
{
    $.ajax({
        type: "POST",
        url: controlador + "horasoperacion/consultar",
        data: { fecha: $('#FechaConsulta').val() },
        success: function (evt) {
            $('#listado').html(evt);
        },
        error: function (req, status, error) {
            mostrarError();
        }
    });
}

function agregarItem(tipo, id, columna, valor)
{
    var indicador = (tipo == "man") ? 1 : 0;  

    $.ajax({
        type: 'POST',
        url: controlador + 'horasoperacion/actualizarvalor',
        dataType: 'json',
        data: {
            indicador: indicador, idEquipo: id, columna:columna, valor:valor
        },
        cache: false,
        success: function (resultado) {
            if (resultado != 1) {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

function checkIndicador(e, id)
{
    var check = "N";
    if(e.checked == true)
    {
        check = "S";
    }
    var arr = id.split('-');
    var indicador = (arr[0] == "man") ? 1 : 0;

    $.ajax({
        type: 'POST',
        url: controlador + 'horasoperacion/actualizarindicador',
        dataType: 'json',
        data: {
            indicador: indicador, idEquipo: arr[1], columna: arr[2], check:check
        },
        cache: false,
        success: function (resultado) {
            if (resultado != 1) {
                mostrarError();
            }                
        },
        error: function () {
            mostrarError();
        }
    });
}

function quitarEquipo()
{
    if (confirm('¿Está seguro de quitar los equipos seleccionados?')) {

        var codigos = "";
        $("#tablaMatriz tbody tr td:first-child").each(function (i) {
            $checkbox = $(this).find('input:checked');
            if ($checkbox.is(':checked')) {
                codigos = codigos + $checkbox.val() + ",";
            }
        });

        codigos = codigos + "0";

        $.ajax({
            type: 'POST',
            url: controlador + 'horasoperacion/quitarequipo',
            data: {
                codigos: codigos
            },
            cache: false,
            success: function (evt) {
                $('#listado').html(evt);
            },
            error: function () {
                mostrarError();
            }
        });
    }
}

function editarHora(horaInicio, horaFin, index)
{
    $.ajax({
        type: 'POST',
        url: controlador + 'horasoperacion/editarhora',
        data: {
            horaInicio:horaInicio, horaFin: horaFin, indice:index
        },
        cache: false,
        success: function (evt) {
            $('#hora').html(evt);
            setTimeout(function () {
                mostrarCargaHora();
            }, 50);

        },
        error: function () {
            mostrarError();
        }
    });
}

function quitarHora()
{
    if (confirm('¿Está seguro de quitar las horas seleccionadas?')) {

        var codigos = "";
        $("#tablaMatriz thead tr th").each(function (i) {
            $checkbox = $(this).find('input:checked');
            if ($checkbox.is(':checked')) {
                codigos = codigos + $checkbox.val() + ",";
            }
        });

        codigos = codigos + "-1";
        
        $.ajax({
            type: 'POST',
            url: controlador + 'horasoperacion/quitarhora',
            data: {
                codigos: codigos
            },
            cache: false,
            success: function (evt) {
                $('#listado').html(evt);
            },
            error: function () {
                mostrarError();
            }
        });
    }
}


function exportar()
{
    $.ajax({
        type: 'POST',
        url: controlador + 'horasoperacion/generar',
        data: { fecha: $('#FechaConsulta').val() },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controlador + "horasoperacion/exportar";
            }
            if (result == -1) {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

function exportarMediaHora()
{
    $.ajax({
        type: 'POST',
        url: controlador + 'horasoperacion/generarmediahora',
        data: { fecha: $('#FechaConsulta').val() },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controlador + "horasoperacion/exportarmediahora";
            }
            if (result == -1) {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });
}


function exportarReservaAsignada()
{
    $.ajax({
        type: 'POST',
        url: controlador + 'horasoperacion/generarreservaasignada',
        data: { inicio: $('#FechaInicio').val(), fin: $('#FechaFin').val() },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controlador + "horasoperacion/exportarreservaasignada";
            }
            if (result == -1) {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

function mostrarError()
{
    alert("Ha ocurrido un error.");
}

function mostrarExito()
{
    $('#mensaje').removeClass("action-alert");
    $('#mensaje').addClass("action-exito");
    $('#mensaje').text("Se grabaron los datos correctamente...!");
}



function validateTime(inputStr) {
    if (!inputStr || inputStr.length < 1) { return false; }
    var time = inputStr.split(':');
    return (time.length === 2
           && parseInt(time[0], 10) >= 0
           && parseInt(time[0], 10) <= 23
           && parseInt(time[1], 10) >= 0
           && parseInt(time[1], 10) <= 59) ||
           (time.length === 3
           && parseInt(time[0], 10) >= 0
           && parseInt(time[0], 10) <= 23
           && parseInt(time[1], 10) >= 0
           && parseInt(time[1], 10) <= 59
           && parseInt(time[2], 10) >= 0
           && parseInt(time[2], 10) <= 59)
}
