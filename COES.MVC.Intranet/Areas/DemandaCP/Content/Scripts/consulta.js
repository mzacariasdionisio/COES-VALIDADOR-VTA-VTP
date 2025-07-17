var controlador = siteRoot + 'demandabarras/consulta/'

$(function () {

    $('#txtFechaInicial').Zebra_DatePicker({
        pair: $('#txtFechaFinal'),
        onSelect: function (date) {
            $('#txtExportarDesde').val(date);
            var date1 = getFecha(date);
            var date2 = getFecha($('#txtFechaFinal').val());

            if (date1 > date2) {
                $('#txtFechaFinal').val(date);
            }
        }
    });

    $('#txtFechaFinal').Zebra_DatePicker({
        direction: true,
        onSelect: function (date) {
            $('#txtExportarHasta').val(date);
        }
    });

    $('#txtExportarDesde').Zebra_DatePicker({
        pair: $('#txtExportarHasta'),
        onSelect: function (date) {
            var date1 = getFecha(date);
            var date2 = getFecha($('#txtExportarHasta').val());

            if (date1 > date2) {
                $('#txtExportarHasta').val(date);
            }
        }
    });

    $('#txtExportarHasta').Zebra_DatePicker({
        direction: true
    });

    $('#btnConsultar').click(function () {
        iniciar();
    });

    $('#btnExportar').click(function () {
        openExportar();
    });

   
    $('#cbEmpresas').multipleSelect({
        width: '250px',
        filter: true
    });

    $('#cbEmpresas').multipleSelect('checkAll');
        
    $('#btnProcesarFile').click(function () {
        exportarFormato();
    });
      
    iniciar();
});



iniciar = function () {
    pintarPaginado();
    buscar(1);
}

buscar = function (nroPagina) {
    var empresa = $('#cbEmpresas').multipleSelect('getSelects');    
    if (empresa == "[object Object]") empresa = "";
    $('#hfEmpresa').val(empresa);    

    if ($('#txtFechaInicial').val() != "" && $('#txtFechaFinal').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + "lista",
            data: {
                fechaInicial: $('#txtFechaInicial').val(),
                fechaFinal: $('#txtFechaFinal').val(),                
                empresas: $('#hfEmpresa').val(),                
                nroPagina: nroPagina,
                frecuencia: $('#cbTipo').val()
            },
            success: function (evt) {
                $('#listado').css("width", $('#mainLayout').width() + "px");
                $('#listado').html(evt);
                $('#tabla').dataTable({
                    "scrollY": 480,
                    "scrollX": true,
                    "sDom": 't',
                    "ordering": false,
                    "bDestroy": true,
                    "bPaginate": false,
                    "iDisplayLength": 50
                });
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else {
        mostrarAlerta("Seleccione rango de fechas.");
    }
}

pintarPaginado = function () {
    var empresa = $('#cbEmpresas').multipleSelect('getSelects');  
    if (empresa == "[object Object]") empresa = "";
    $('#hfEmpresa').val(empresa);   

    if ($('#txtFechaInicial').val() != "" && $('#txtFechaFinal').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + "paginado",
            data: {
                fechaInicial: $('#txtFechaInicial').val(),
                fechaFinal: $('#txtFechaFinal').val(),               
                empresas: $('#hfEmpresa').val()
            },
            success: function (evt) {
                $('#paginado').html(evt);
                mostrarPaginado();
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
    else {
        mostrarAlerta("Seleccione rango de fechas.");
    }
}

pintarBusqueda = function (nroPagina) {
    buscar(nroPagina);
}

openExportar = function () {
    $('#divExportar').css('display', 'block');
}

closeExportar = function () {
    $('#divExportar').css('display', 'none');
}


exportarFormato = function () {

    var formato = $("input:radio[name='rbFormato']:checked").val();  
    var empresa = $('#cbEmpresas').multipleSelect('getSelects');   
    if (empresa == "[object Object]") empresa = "";
    $('#hfEmpresa').val(empresa);
    

    if ($('#txtExportarDesde').val() != "" && $('#txtExportarHasta').val() != "") {

        $.ajax({
            type: 'POST',
            url: controlador + "validarexportacion",
            dataType: 'json',
            data: {
                formato: formato,
                fechaInicial: $('#txtExportarDesde').val(),
                fechaFinal: $('#txtExportarHasta').val()                
            },
            success: function (result) {
                if (result == 1) {

                    $.ajax({
                        type: 'POST',
                        url: controlador + "exportar",
                        dataType: 'json',
                        data: {
                            fechaInicial: $('#txtExportarDesde').val(),
                            fechaFinal: $('#txtExportarHasta').val(),                            
                            empresas: $('#hfEmpresa').val(),
                            tipo: formato,
                            frecuencia: $('#cbTipo').val()
                        },
                        success: function (result) {
                            if (result == "1") {
                                window.location = controlador + 'descargar?tipo=' + formato;
                            }
                            else {
                                alert(result);
                            }                            
                        },
                        error: function () {
                            mostrarError()
                        }
                    });
                }
                if (result == 2) {
                    mostrarAlerta("El lapso de tiempo no puede ser mayor a 3 meses.");
                }
                if (result == 3) {
                    mostrarAlerta("Para la exportación a CSV solo debe seleccionar un parámetro.");
                }
                if (result == 4) {
                    mostrarAlerta("Seleccione un parámetro a exportar.");
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
    else {
        mostrarAlerta("Seleccione rango de fechas.");
    }
}

mostrarAlerta = function (mensaje) {
    alert(mensaje);
}

mostrarError = function () {
    alert("Ha ocurrido un error.");
}



getFecha = function (date) {
    var parts = date.split("/");
    var date = new Date(parts[1] + "/" + parts[0] + "/" + parts[2]);
    return date.getTime();
}