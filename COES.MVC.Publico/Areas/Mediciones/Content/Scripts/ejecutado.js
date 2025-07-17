var controlador = siteRoot + 'mediciones/ejecutado/'

$(function () {

    $('#txtFechaInicial').Zebra_DatePicker({
        pair: $('#txtFechaFinal'),
        onSelect: function (date) {            
            var date1 = getFecha(date);
            var date2 = getFecha($('#txtFechaFinal').val());

            if (date1 > date2) {
                $('#txtFechaFinal').val(date);
            }
        }
    });

    $('#txtFechaFinal').Zebra_DatePicker({
        direction: true
    });   

    $('#btnBuscar').click(function () {
        iniciar();
    });

    $('#btnExportar').click(function () {
        exportar();
    });     

    $('#cbTipoEmpresa').multipleSelect({
        width: '150px',
        filter: true,
        onClick: function (view) {
            cargarEmpresas();
        },
        onClose: function (view) {
            cargarEmpresas();
        }
    });

    $('#cbTipoGeneracion').multipleSelect({
        width: '150px',
        filter: true
    });
        
    cargarPrevio();
    cargarEmpresas();
    iniciar();
});

cargarPrevio = function () {    
    $('#cbTipoEmpresa').multipleSelect('checkAll');
    $('#cbTipoGeneracion').multipleSelect('checkAll');   
}

iniciar = function () {
    pintarPaginado();
    buscar(1);
}

cargarEmpresas = function () {
    var tipoEmpresa = $('#cbTipoEmpresa').multipleSelect('getSelects');
    $('#hfTipoEmpresa').val(tipoEmpresa);

    $.ajax({
        type: 'POST',
        url: controlador + "empresas",
        data: {
            tiposEmpresa: $('#hfTipoEmpresa').val()
        },
        success: function (evt) {
            $('#empresas').html(evt);
        },
        error: function () {
            mostrarError();
        }
    });
}

buscar = function (nroPagina) {
    var empresa = $('#cbEmpresas').multipleSelect('getSelects');    
    var tipoEmpresa = $('#cbTipoEmpresa').multipleSelect('getSelects');
    var tipoGeneracion = $('#cbTipoGeneracion').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "";
    $('#hfEmpresa').val(empresa);    
    $('#hfTipoEmpresa').val(tipoEmpresa);
    $('#hfTipoGeneracion').val(tipoGeneracion);

    if ($('#txtFechaInicial').val() != "" && $('#txtFechaFinal').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + "lista",
            data: {
                fechaInicial: $('#txtFechaInicial').val(),
                fechaFinal: $('#txtFechaFinal').val(),
                tiposEmpresa: $('#hfTipoEmpresa').val(),
                empresas: $('#hfEmpresa').val(),
                tiposGeneracion: $('#hfTipoGeneracion').val(),
                nroPagina: nroPagina
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
    var tipoEmpresa = $('#cbTipoEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "";
    var tipoGeneracion = $('#cbTipoGeneracion').multipleSelect('getSelects');
    $('#hfEmpresa').val(empresa);    
    $('#hfTipoEmpresa').val(tipoEmpresa);
    $('#hfTipoGeneracion').val(tipoGeneracion);

    if ($('#txtFechaInicial').val() != "" && $('#txtFechaFinal').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + "paginado",
            data: {
                fechaInicial: $('#txtFechaInicial').val(),
                fechaFinal: $('#txtFechaFinal').val(),
                tiposEmpresa: $('#hfTipoEmpresa').val(),
                empresas: $('#hfEmpresa').val(),
                tiposGeneracion: $('#hfTipoGeneracion').val()
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

exportar = function () {    
    var empresa = $('#cbEmpresas').multipleSelect('getSelects');    
    var tipoEmpresa = $('#cbTipoEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "";
    var tipoGeneracion = $('#cbTipoGeneracion').multipleSelect('getSelects');
    $('#hfEmpresa').val(empresa);    
    $('#hfTipoEmpresa').val(tipoEmpresa);
    $('#hfTipoGeneracion').val(tipoGeneracion);

    if ($('#txtFechaInicial').val() != "" && $('#txtFechaFinal').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + "exportar",
            dataType: 'json',
            data: {
                fechaInicial: $('#txtFechaInicial').val(),
                fechaFinal: $('#txtFechaFinal').val(),
                tiposEmpresa: $('#hfTipoEmpresa').val(),
                empresas: $('#hfEmpresa').val(),
                tiposGeneracion: $('#hfTipoGeneracion').val(),
            },
            success: function (result) {
                if (result == 1) {
                    window.location = controlador + 'descargar';
                }
                else if (result == 2) {
                    mostrarAlerta("Solo se puede exportar data de máximo un año.");
                }
                else if (result == -1) {
                    mostrarError();
                }
            },
            error: function () {
                mostrarError()
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