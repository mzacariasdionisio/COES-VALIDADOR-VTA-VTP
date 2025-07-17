var controlador = siteRoot + 'demandabarras/cumplimiento/'

$(function () {

    $('#txtFechaInicial').Zebra_DatePicker({
        format: 'm Y',
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
        format: 'm Y',
        direction: true
    });     

    $('#btnConsultar').click(function () {
        buscar();
    });

    $('#btnExportar').click(function () {
        exportarFormato();
    });
    
    $('#cbEmpresas').multipleSelect({
        width: '250px',
        filter: true
    });

    $('#cbEmpresas').multipleSelect('checkAll');    

    buscar();
});


buscar = function () {
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
                empresas: $('#hfEmpresa').val()
            },
            success: function (evt) {
                $('#listado').css("width", $('#mainLayout').width() + "px");
                $('#listado').html(evt);
                $('#tabla').dataTable({
                   
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

exportarFormato = function () {  
    var empresa = $('#cbEmpresas').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "";
    $('#hfEmpresa').val(empresa);
    
    if ($('#txtFechaInicial').val() != "" && $('#txtFechaFinal').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + "exportar",
            dataType: 'json',
            data: {
                fechaInicial: $('#txtFechaInicial').val(),
                fechaFinal: $('#txtFechaFinal').val(),
                empresas: $('#hfEmpresa').val()
            },
            success: function (result) {
                if (result == "1") {
                    window.location = controlador + 'descargar';
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
    else {
        mostrarAlerta("Seleccione rango de fechas.");
    }
}

verEnvios = function (idEmpresa) {
    
    if ($('#txtFechaInicial').val() != "" && $('#txtFechaFinal').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + "envios",
            global:false,
            data: {
                idEmpresa: idEmpresa,
                fechaInicial: $('#txtFechaInicial').val(),
                fechaFinal: $('#txtFechaFinal').val()                
            },
            success: function (evt) {

                $('#contenedorEnvios').html(evt);
                $('#tablaEnvio').dataTable({
                });

                setTimeout(
                    $('#popupEnvios').bPopup({
                        easing: 'easeOutBack',
                        speed: 450,
                        transition: 'slideDown'
                    }), 50)
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