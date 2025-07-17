var controlador = siteRoot + 'mercadomayorista/costosmarginales/'

$(function () {
    $('#txtFecha').change(      
       function (date) {
            verificarFecha();
            cargarProcesos(0);
        }
	);
    
    cargarProcesosInicio();


    $('#btnConsultar').click(function () {
        consultar();
    });

    $('#btnExportar').click(function () {
        exportar();
    });

    $('#btnExportarMasivo').on('click', function () {
        openExportar();
    });

    $('#btnOkExportarMasivo').on('click', function () {
        exportarMasivo();
    });

    $('#btnActualizar').click(function () {
        actualizar();
    });

    $('#divMapa').on('click', function () {
        $("#divMapa").addClass("active");
        $("#divDatos").removeClass("active");
        $("#divArchivos").removeClass("active");
        $("#btnMapa").css("background-color", "var(--bs-gray-500)");
        $("#btnDatos").css("background-color", "");
        $("#btnArchivos").css("background-color", "");
        verificarFecha();
        consultar();
    });

    $('#divDatos').on('click', function () {
        $("#divMapa").removeClass("active");
        $("#divDatos").addClass("active");
        $("#divArchivos").removeClass("active");
        $("#btnMapa").css("background-color", "");
        $("#btnDatos").css("background-color", "var(--bs-gray-500)");
        $("#btnArchivos").css("background-color", "");
        $('#valoresPreliminares').hide();
        listar();   
    });

    $('#divArchivos').on('click', function () {
        $("#divMapa").removeClass("active");
        $("#divDatos").removeClass("active");
        $("#divArchivos").addClass("active");
        $("#btnMapa").css("background-color", "");
        $("#btnDatos").css("background-color", "");
        $("#btnArchivos").css("background-color", "var(--bs-gray-500)");
        $('#valoresPreliminares').hide();
        verArchivo();
    });

    //$('.item-change-dashboard').click(function () {
    //    $('.item-change-dashboard').removeClass('active');
    //    $(this).addClass('active');
    //    var item = $(this).attr('data-fuente');

    //    if (item == "mapa") {
    //        verificarFecha();
    //        consultar();           
    //    }
    //    else if (item == "datos") {
    //        $('#valoresPreliminares').hide();
    //        listar();            
    //    }
    //    else if (item == "archivo") {
    //        $('#valoresPreliminares').hide();
    //        verArchivo();
    //    }
    //});

    $('#cbSelectAll').click(function (e) {
        var table = $(e.target).closest('table');
        $('td input:checkbox', table).prop('checked', this.checked);
    });

    $('#txtExportarDesde').change({
        pair: $('#txtExportarHasta'),
        onSelect: function (date) {
            var date1 = getFecha(date);
            var date2 = getFecha($('#txtExportarHasta').val());

            if (date1 > date2) {
                $('#txtExportarHasta').val(date);
            }
        }
    });

    $('#txtExportarHasta').change({
        direction: true
    });

    $('#btnAceptarComunicado').on('click', function () {
        $('#popupMensaje').bPopup().close();
    });

    

});

openComunicado = function () {    
    $('#popupMensaje').bPopup({
        autoClose: false
    });
}

verificarFecha = function () {
    var fecha = getFecha(GetDateNormalFormat($('#txtFecha').val()));
    var fechaInicio = getFecha("22/10/2017");
    var fechaFin = getFecha("11/12/2017");

    $('#valoresPreliminares').hide();

    
    if ((fecha <= fechaInicio || fecha >= fechaFin))
    {        
        $('#valoresPreliminares').show();
    }
}

cargarProcesos = function (indicador) {
    if ($('#txtFecha').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'listaproceso',
            data: {
                fecha: GetDateNormalFormat($('#txtFecha').val())
            },
            dataType: 'json',
            success: function (result) {
                $("#cbHoras").empty();
                for (var i in result) {
                    $("#cbHoras").append('<option value="' + result[i].Cmgncorrelativo + '">' + result[i].FechaProceso + '</option>');
                }

                if (indicador == 1)
                {
                    consultar();
                    verificarFecha();

                    openComunicado();
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'Seleccione fecha.');
    }
}

cargarProcesosInicio = function () {
    $("#divMapa").addClass("active");
    $("#divDatos").removeClass("active");
    $("#divArchivos").removeClass("active");
    $("#btnMapa").css("background-color", "var(--bs-gray-500)");
    $("#btnDatos").css("background-color", "");
    $("#btnArchivos").css("background-color", "");
    cargarProcesos(1);
}

consultar = function () {
    if ($("#cbHoras").val() != "" && $("#cbHoras").val() != null)
    {
        
        $.ajax({
            type: 'POST',
            url: controlador + 'mapa',
            data: {
                correlativo: $('#cbHoras').val(),
                defecto: $('#cbDefecto').val()
            },            
            success: function (evt) {
                $('#contenedorMapa').html(evt);
                $('#folder').hide();
                $('#resultado').show();
                $('#leyenda').show();

                //$('.item-change-dashboard').removeClass('active');
                //$('#item-inicial').addClass('active');
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'Seleccione fecha y hora');
    }
}

actualizar = function () {

    var colores = "";
    var countColores = 0;
    $('#tablaColores tbody input:checked').each(function () {
        colores = colores + $(this).val() + ",";
        countColores++;
    });
   
    if (countColores > 0) {

        $.ajax({
            type: 'POST',
            url: controlador + 'mapafiltro',
            data: {
                correlativo: $('#cbHoras').val(),
                colores: colores,
                defecto: $('#cbDefecto').val()
            },
            success: function (evt) {
                $('#contenedorMapa').html(evt);
                $('#folder').hide();
                $('#resultado').show();
                $('#leyenda').show();

                mostrarMensaje('mensaje', 'noexiste', '');
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });        
    }
    else {
        mostrarMensaje('mensaje', 'error', 'Seleccióne al menos un elemento.');
    }
}

listar = function ()
{
    if ($("#cbHoras").val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'lista',
            data: {
                correlativo: $('#cbHoras').val(),
                defecto: $('#cbDefecto').val()
            },
            success: function (evt) {
                $('#contenedorMapa').html(evt);
                $('#tablaResultado').dataTable({                    
                    "iDisplayLength": 25
                });
                $('#folder').hide();
                $('#resultado').show();
                $('#leyenda').hide();
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'Seleccione fecha y hora');
    }
}

exportar = function () {

    if (GetDateNormalFormat($('#txtFecha').val()) != "" && $("#cbHoras").val() != "") {

        $.ajax({
            type: 'POST',
            url: controlador + "exportar",
            data: {
                fecha: GetDateNormalFormat($('#txtFecha').val()),
                correlativo: $("#cbHoras").val(),
                defecto: $('#cbDefecto').val()
            },
            dataType: 'json',
            cache: false,
            success: function (resultado) {
                if (resultado == 1) {
                    location.href = controlador + "descargar";
                }
                else {
                    mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'Seleccione fecha y hora.');
    }
    
}

openExportar = function () {
    $('#divExportar').css('display', 'block');
}

closeExportar = function () {
    $('#divExportar').css('display', 'none');
}

exportarMasivo = function () {
    if ($('#txtExportarDesde').val() != "" && $('#txtExportarHasta').val() != "") {

        var fechaInicio = getFecha(GetDateNormalFormat($('#txtExportarDesde').val()));
        var fechaFin = getFecha(GetDateNormalFormat($('#txtExportarHasta').val()));
        var days = (fechaFin - fechaInicio) / (1000 * 60 * 60 * 24);
        if (days <= 31) {

            $.ajax({
                type: 'POST',
                url: controlador + "exportarmasivo",
                data: {
                    fechaInicio: GetDateNormalFormat($('#txtExportarDesde').val()),
                    fechaFin: GetDateNormalFormat($('#txtExportarHasta').val()),
                    defecto: $('#cbDefecto').val()
                },
                dataType: 'json',
                cache: false,
                success: function (resultado) {
                    if (resultado == 1) {
                        location.href = controlador + "descargarmasivo";
                    }
                    else {
                        mostrarMensaje('mensajeExportar', 'error', 'Ha ocurrido un error.');
                    }
                },
                error: function () {
                    mostrarMensaje('mensajeExportar', 'error', 'Ha ocurrido un error.');
                }
            });
        }
        else {
            mostrarMensaje('mensajeExportar', 'alert', 'Solo se permite seleccionar como máximo 30 días.');
        }
    }
    else {
        mostrarMensaje('mensajeExportar', 'alert', 'Seleccione el rango de fechas..');
    }
}

verArchivo = function () {
    
    if ($('#txtFecha').val() != "" && $("#cbHoras").val() != "") {

        $.ajax({
            type: 'POST',
            url: controlador + 'folder',
            data: {
                fecha: GetDateNormalFormat($('#txtFecha').val()),
                correlativo: $("#cbHoras").val()
            },
            dataType: 'json',
            success: function (result) {
                $('#hfBaseDirectory').val("");
                $('#hfRelativeDirectory').val(result.PathResultado);
                browser();
                $('.bread-crumb').hide();
                $('#folder').show();
                $('#resultado').hide();
                $('#leyenda').hide();

            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'Seleccione fecha y hora.');
    }

}

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}

getFecha = function (date) {
    var parts = date.split("/");
    var date = new Date(parts[1] + "/" + parts[0] + "/" + parts[2]);
    return date.getTime();
}

GetDateNormalFormat = function (fechaFormatoISO) {
    let fechaNormal = fechaFormatoISO.substring(8, 10) + "/" + fechaFormatoISO.substring(5, 7) + "/" + fechaFormatoISO.substring(0, 4);
    return fechaNormal;
}