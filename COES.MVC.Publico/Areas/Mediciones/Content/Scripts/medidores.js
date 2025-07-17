var controlador = siteRoot + 'mediciones/medidoresgeneracion/'

$(function () {

    
    $('#txtFechaInicial').change(function () {
        validarParametro();
    });

    $('#txtFechaFinal').change(function () {
        validarParametro();
    });

    $('#txtExportarDesde').change({
        pair: $('#txtExportarHasta'),        
        onSelect: function (date) {        
            var date1 = getFecha(date);
            var date2 = getFecha($('#txtExportarHasta').val());

            if (date1 > date2)
            {
                $('#txtExportarHasta').val(date);
            }
        }
    });

    $('#txtExportarHasta').change({
        direction: true
    });
    
    $('#btnBuscar').click(function () {
        iniciar();
    });

    $('#btnExportar').click(function () {
        openExportar();
    });

    $('#cbTipoGeneracion').multipleSelect({
        width: '100%',
        filter: true       
    });

    $('#cbTipoEmpresa').multipleSelect({
        //width: '150px',
        filter: true,
        onClick: function (view) {
            cargarEmpresas();
        },
        onClose: function (view) {
            cargarEmpresas();
        }
    });
    
    $('#cbParametroExportar').multipleSelect({
       /* width: '100%'*/   
    });

    $('#btnProcesarFile').click(function () {

        $('#mensajeExportar').removeClass();
        $('#mensajeExportar').html("");
        var mensaje = validarFechas(GetDateNormalFormat($('#txtExportarDesde').val()), GetDateNormalFormat($('#txtExportarHasta').val()));
        if (mensaje == "") {
            exportarFormato();
        }
        else {
            mostrarMensaje('mensajeExportar', 'alert', mensaje);
        }

        
    });
      
    cargarPrevio();
    cargarEmpresas();   
    iniciar();    
});

cargarPrevio = function()
{
    $('#cbTipoGeneracion').multipleSelect('checkAll');
    $('#cbTipoEmpresa').multipleSelect('checkAll');
}

iniciar = function ()
{
    //$('#mensaje').removeClass();
    $('#mensaje').html("");
    var mensaje = validarFechas(GetDateNormalFormat($('#txtFechaInicial').val()), GetDateNormalFormat($('#txtFechaFinal').val()));
    if (mensaje == "") {
        pintarPaginado();
        buscar(1);
    }
    else {
        mostrarMensaje('mensaje', 'alert', mensaje);
    }
}

validarFechas = function (txtFechaInicio, txtFechaFin) {
    
    var mensaje = "<ul style='margin-bottom:0px;'>";
    var flag = true;
    if (txtFechaInicio != "" && txtFechaFin != "") {

        var fecInicio = getFecha(txtFechaInicio);
        var fecFin = getFecha(txtFechaFin);
        var fecActualInicio = getFecha(GetDateNormalFormat($('#hfFechaActualInicio').val()));
        var fecActualFin = getFecha(GetDateNormalFormat($('#hfFechaActualFin').val()));
        var fecAnteriorInicio = getFecha(GetDateNormalFormat($('#hfFechaAnteriorInicio').val()));
        var fecAnteriorFin = getFecha(GetDateNormalFormat($('#hfFechaAnteriorFin').val()));
        
        if (fecInicio >= fecActualInicio) {
            mensaje = mensaje + "<li style='margin-bottom:0px;'>Solo se pueden consultar fechas anteriores al mes actual.</li>";
            flag = false;
        }
        else if (fecInicio >= fecAnteriorInicio && fecInicio <= fecAnteriorFin) {
            if (fecFin >= fecActualInicio) {
                mensaje = mensaje + "<li style='margin-bottom:0px;'>Para el mes anterior, el rango de fecha debe estar incluido en el mes.</li>";
                flag = false;
            }
        }
        else {
            if (fecFin >= fecAnteriorInicio) {
                mensaje = mensaje + "<li style='margin-bottom:0px;'>La fecha de fin no debe estar ni ser superior al mes anterior.</li>";
                flag = false;
            }
        }
    }
    else {
        mensaje = mensaje + "<li style='margin-bottom:0px;'>Seleccione la fecha de inicio y fin</li>";
        flag = false;
    }

    mensaje = mensaje + "</ul>";

    if (flag) {
        mensaje = "";
    }

    return mensaje;

}

cargarEmpresas = function ()
{
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
            //validarParametro();
			
            setTimeout(() => {
                validarParametro();
            }, 500);
        },
        error: function () {
            mostrarError();
        }
    });

}

buscar = function (nroPagina)
{
    var empresa = $('#cbEmpresas').multipleSelect('getSelects');
    var tipoGeneracion = $('#cbTipoGeneracion').multipleSelect('getSelects');
    var tipoEmpresa = $('#cbTipoEmpresa').multipleSelect('getSelects');

    if (empresa == "[object Object]") empresa = "";
      
    $('#hfEmpresa').val(empresa);
    $('#hfTipoGeneracion').val(tipoGeneracion);
    $('#hfTipoEmpresa').val(tipoEmpresa);
    
    if ($('#txtFechaInicial').val() != "" && $('#txtFechaFinal').val() != "")
    {
        $.ajax({
            type: 'POST',
            url: controlador + "lista",
            data: {
                fechaInicial: GetDateNormalFormat($('#txtFechaInicial').val()),
                fechaFinal: GetDateNormalFormat($('#txtFechaFinal').val()),
                tiposEmpresa : $('#hfTipoEmpresa').val(),
                empresas: $('#hfEmpresa').val(),
                tiposGeneracion: $('#hfTipoGeneracion').val(),
                central: $('#cbCentral').val(),
                parametro: $('#cbParametro').val(),
                nroPagina:nroPagina
            },
            success: function (evt) {
                //$('#listado').css("width", $('#mainLayout').width() + "px");
                $('#listado').html(evt);
                $('#tabla').dataTable({
                    "scrollY": 480,
                    "scrollX": true,                   
                    "sDom": 't',
                    "ordering": false,
                    "bDestroy":true,
                    "bPaginate": false,
                    "iDisplayLength": 50,
                    "className": 'table table-hover'
                });                
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else
    {
        mostrarAlerta("Seleccione rango de fechas.");
    }
}

pintarPaginado = function()
{
    var empresa = $('#cbEmpresas').multipleSelect('getSelects');
    var tipoGeneracion = $('#cbTipoGeneracion').multipleSelect('getSelects');
    var tipoEmpresa = $('#cbTipoEmpresa').multipleSelect('getSelects');

    if (empresa == "[object Object]") empresa = "";

    $('#hfEmpresa').val(empresa);
    $('#hfTipoGeneracion').val(tipoGeneracion);
    $('#hfTipoEmpresa').val(tipoEmpresa);

    if ($('#txtFechaInicial').val() != "" && $('#txtFechaFinal').val() != "")
    {
        $.ajax({
            type: 'POST',
            url: controlador + "paginado",
            data: {
                fechaInicial: GetDateNormalFormat($('#txtFechaInicial').val()),
                fechaFinal: GetDateNormalFormat($('#txtFechaFinal').val()),
                tiposEmpresa: $('#hfTipoEmpresa').val(),
                empresas: $('#hfEmpresa').val(), tiposGeneracion: $('#hfTipoGeneracion').val(),
                central: $('#cbCentral').val(), parametro: $('#cbParametro').val()
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
    else
    {
        mostrarAlerta("Seleccione rango de fechas.");
    }
}

pintarBusqueda = function(nroPagina) {
    buscar(nroPagina);
}

openExportar = function ()
{
    //$('#divExportar').css('display', 'block');

    $('#txtExportarDesde').val($('#txtFechaInicial').val());
    $('#txtExportarHasta').val($('#txtFechaFinal').val());

    $('#divExportar').bPopup({
        follow: [true, true],
        position: ['auto', 'auto'],
        positionStyle: 'fixed',
    });
}

closeExportar = function ()
{
    $('#divExportar').css('display', 'none');
}

exportarFormato = function () {

    var formato = $("input:radio[name='rbFormato']:checked").val();
    var parametros = $('#cbParametroExportar').multipleSelect('getSelects');
    var empresa = $('#cbEmpresas').multipleSelect('getSelects');
    var tipoGeneracion = $('#cbTipoGeneracion').multipleSelect('getSelects');
    var tipoEmpresa = $('#cbTipoEmpresa').multipleSelect('getSelects');

    if (parametros == "") {
        var options = $('#cbParametroExportar option');
        if (options.length > 0) {
            for (var i = 0; i < options.length; i++) {
                parametros.push(options[i].value);
            }
        }
    }

    if (empresa == "[object Object]") empresa = "";

    $('#hfEmpresa').val(empresa);
    $('#hfTipoGeneracion').val(tipoGeneracion);
    $('#hfParametro').val(parametros);
    $('#hfTipoEmpresa').val(tipoEmpresa);

    if ($('#txtExportarDesde').val() != "" && $('#txtExportarHasta').val() != "") {

        $.ajax({
            type: 'POST',
            url: controlador + "validarexportacion",
            dataType: 'json',
            data: {
                formato: formato,
                fechaInicial: GetDateNormalFormat($('#txtExportarDesde').val()),
                fechaFinal: GetDateNormalFormat($('#txtExportarHasta').val()),
                parametros: $('#hfParametro').val()
            },
            success: function (result) {
                if (result == 1) {

                    $.ajax({
                        type: 'POST',
                        url: controlador + "exportar",
                        dataType: 'json',
                        data: {
                            fechaInicial: GetDateNormalFormat($('#txtExportarDesde').val()),
                            fechaFinal: GetDateNormalFormat($('#txtExportarHasta').val()),
                            tiposEmpresa: $('#hfTipoEmpresa').val(),
                            empresas: $('#hfEmpresa').val(),
                            tiposGeneracion: $('#hfTipoGeneracion').val(),
                            central: $('#cbCentral').val(),
                            parametros: $('#hfParametro').val(),
                            tipo: formato
                        },
                        success: function (result) {
                            if (result == "1") {
                                window.location = controlador + 'descargar?tipo=' + formato;
                            }
                            else
                            {
                                alert(result);
                            }
                            /*if (result == -1) {
                                mostrarError();
                            }*/
                        },
                        error: function () {
                            mostrarError()
                        }
                    });
                }
                if (result == 2) {
                    mostrarAlerta("El lapso de tiempo no puede ser mayor a 1 meses.");
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

mostrarAlerta = function (mensaje)
{
    alert(mensaje);
}

mostrarError = function () {
    alert("Ha ocurrido un error.");
}

getFecha = function(date) {
    var parts = date.split("/");
    var date = new Date(parts[1] + "/" + parts[0] + "/" + parts[2]);
    return date.getTime();
}

mostrarMensaje = function (id, tipo, mensaje) {

    $('#' + id).removeClass();
    $('#' + id).css("display", "flex");
    if (tipo === 'exito') {
        $('#' + id).addClass('coes-form-item--info coes-form-item coes-box coes-box--content mt-3 pt-3 pe-3 ps-3 mb-3 pb-3');
    } else {
        $('#' + id).addClass('coes-form-item--error coes-form-item coes-box coes-box--content mt-3 pt-3 pe-3 ps-3 mb-3 pb-3');
    }
    $('#' + id).html(mensaje);

    //$('#' + id).removeClass();
    //$('#' + id).addClass('action-' + tipo);
    //$('#' + id).html(mensaje);
}



validarParametro = function () {
    var empresa = $('#cbEmpresas').multipleSelect('getSelects');
    var tipoGeneracion = $('#cbTipoGeneracion').multipleSelect('getSelects');
    var tipoEmpresa = $('#cbTipoEmpresa').multipleSelect('getSelects');

    if (empresa == "[object Object]") empresa = "";

    $('#hfEmpresa').val(empresa);
    $('#hfTipoGeneracion').val(tipoGeneracion);
    $('#hfTipoEmpresa').val(tipoEmpresa);

    if ($('#txtFechaInicial').val() != "" && $('#txtFechaFinal').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + "ValidarParametro",
            data: {
                fechaInicial: GetDateNormalFormat($('#txtFechaInicial').val()),
                fechaFinal: GetDateNormalFormat($('#txtFechaFinal').val()),
                tiposEmpresa: $('#hfTipoEmpresa').val(),
                empresas: $('#hfEmpresa').val(), tiposGeneracion: $('#hfTipoGeneracion').val(),
                central: $('#cbCentral').val(), parametro: $('#cbParametro').val()
            },
            success: function (evt) {
                $("#cbParametroExportar").empty();
                $("#cbParametro").empty();
                if (evt == "-1") {
                    mostrarError();
                } else {
                    for (var i = 0; i < evt.length; i++) {
                        $('#cbParametroExportar').append('<option value=' + evt[i].EstadoCodigo + '>' + evt[i].EstadoDescripcion + '</option>');
                        $('#cbParametro').append('<option value=' + evt[i].EstadoCodigo + '>' + evt[i].EstadoDescripcion + '</option>');
                    }
                    /*
                    if (contador == 0) {
                        iniciar();
                    }
                    contador += 1;*/
                }
                $("#cbParametroExportar").multipleSelect();
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

// Convierte formato dd/mm/yyyy a yyyy-mm-dd
GetISODate = function (fechaFormatoNormal) {
    let fechaISO = fechaFormatoNormal.substring(6, 10) + "-" + fechaFormatoNormal.substring(3, 5) + "-" + fechaFormatoNormal.substring(0, 2);
    return fechaISO;
}

// Convierte formato yyyy-mm-dd a dd/mm/yyyy
GetDateNormalFormat = function (fechaFormatoISO) {
    let fechaNormal = fechaFormatoISO.substring(8, 10) + "/" + fechaFormatoISO.substring(5, 7) + "/" + fechaFormatoISO.substring(0, 4);
    return fechaNormal;
}