var controlador = siteRoot + 'demandabarras/consulta/'
var hot;
var numeroTotal = 31;

$(function () {

    $('#txtFechaInicio').change({
        pair: $('#txtFechaFin'),
        onSelect: function (date) {
            $('#txtFechaInicio').val(date);
            var date1 = getFecha(date);
            var date2 = getFecha($('#txtFechaFin').val());
            //if (date1 > date2) {
                $('#txtFechaFin').val(date);
            //}
        }
    });

    $('#txtFechaFin').change({
        direction: true        
    });       

    $('#btnConsultar').click(function () {
        consultar();
    });

    $('#btnExportar').click(function () {
        exportar();
    });
    
    $('#cbEmpresas').multipleSelect({
        width: '200px'
    });

    //(function setFixedMultiselects() {
    //    $('select.multiselect').each(function (index) {
    //        var $button = $(this).next().children().first();
    //        var $dropdown = $button.next();
    //        if ($dropdown.css('display') != 'none') {
    //            var offset = $button.offset();
    //            $dropdown.css({
    //                'position': 'fixed',
    //                'top': (offset.top + $button.outerHeight()) + "px",
    //                'left': offset.left + "px",
    //                'width': '300px'
    //            });
    //        }
    //    });
    //    setTimeout(setFixedMultiselects, 20);
    //})();

    $('#cbEmpresas').multipleSelect('checkAll');

    $('input[name="rbTipoDemanda"]').click(function () {
        cargarTipoDemanda($("input:radio[name='rbTipoDemanda']:checked").val());
    });

    consultar();
        
});

cargarTipoDemanda = function (tipo) {
    if (tipo == "103") {
        $('#txtFechaInicio').val($('#hfHistoricoDesde').val());
        $('#txtFechaFin').val($('#hfHistoricoHasta').val());
    }
    else if (tipo == "110") {
        $('#txtFechaInicio').val($('#hfDiarioDesde').val());
        $('#txtFechaFin').val($('#hfDiariopHasta').val());
    }
    else if (tipo == "102") {
        $('#txtFechaInicio').val($('#hfSemanalDesde').val());
        $('#txtFechaFin').val($('#hfSemanalHasta').val());
    }
}

consultar = function () {    
    let empresa = $('#cbEmpresas').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "";      
    $('#hfEmpresa').val(empresa);

    if ($('#txtFechaInicio').val() != "" && $('#txtFechaFin').val() != "") {
        let strFecha1 = GetDateNormalFormat($('#txtFechaInicio').val());
        let fecha1 = getFecha(strFecha1);
        let strFecha2 = GetDateNormalFormat($('#txtFechaFin').val());
        let fecha2 = getFecha(strFecha2);
        let diferencia = numeroDias(fecha1, fecha2);

        let fechaInicio = $('#txtFechaInicio').val();
        let fechaFin = $('#txtFechaFin').val();
        fechaInicio = GetDateNormalFormat($('#txtFechaInicio').val());
        fechaFin = GetDateNormalFormat($('#txtFechaFin').val())
        
        if (diferencia <= numeroTotal) {

            if ($('#hfEmpresa').val() != "") {
                $.ajax({
                    type: 'POST',
                    url: controlador + 'grilla',
                    data: {
                        lectcodi: $("input:radio[name='rbTipoDemanda']:checked").val(),
                        empresas: $('#hfEmpresa').val(),
                        fechaInicio: fechaInicio,
                        fechaFin: fechaFin,
                    },
                    dataType: 'json',
                    success: function (result) {

                        if (result != 0) {
                            if (typeof hot != 'undefined') {
                                hot.destroy();
                            }

                            function headerRender(instance, td, row, col, prop, value, cellProperties) {
                                Handsontable.renderers.TextRenderer.apply(this, arguments);
                              /*  td.style.fontWeight = 'bold';*/
                                td.style.fontSize = '11px';
                               td.style.color = 'black';
                                td.style.background = '#F3F5F7';
                            }

                            function normalRender(instance, td, row, col, prop, value, cellProperties) {
                                Handsontable.renderers.TextRenderer.apply(this, arguments);
                                td.style.fontSize = '11px';
                                td.style.color = 'black';
                                td.style.background = '#F3F5F7';
                            }

                            function oddRender(instance, td, row, col, prop, value, cellProperties) {
                                Handsontable.renderers.TextRenderer.apply(this, arguments);
                                td.style.fontSize = '11px';
                                td.style.color = 'black';
                                td.style.background = '#F3F5F7';
                            }

                            function contentRender(instance, td, row, col, prop, value, cellProperties) {
                                Handsontable.renderers.TextRenderer.apply(this, arguments);
                                td.style.fontSize = '11px';
                                td.style.textAlign = 'right';
                                td.style.color = 'black';
                            }

                            container = document.getElementById('detalleFormato');
                            hotOptions = {
                                data: result,
                                height: 600,
                                maxRows: result.length,
                                maxCols: result[0].length,
                                colHeaders: true,
                                rowHeaders: true,
                                fillHandle: true,
                                columnSorting: false,
                                className: "htCenter",
                                readOnly: true,
                                fixedRowsTop: 5,
                                fixedColumnsLeft: 1,
                                cells: function (row, col, prop) {
                                    var cellProperties = {};

                                    if (row == 0) {
                                        cellProperties.renderer = headerRender;
                                    }
                                    else if (row == 1 || row == 3) {
                                        cellProperties.renderer = normalRender;
                                    }
                                    else if (row == 2 || row == 4) {
                                        cellProperties.renderer = oddRender;
                                    }
                                    else if (row > 4 && col == 0) {
                                        cellProperties.renderer = normalRender;
                                    }
                                    else {
                                        cellProperties.renderer = contentRender;
                                    }
                                    return cellProperties;
                                }
                            };
                            hot = new Handsontable(container, hotOptions);

                            $('#textoMensaje').removeClass();                            
                            $('#textoMensaje').text("");
                        }
                        else
                        {
                            $('#detalleFormato').html("");
                            $('#textoMensaje').removeClass();
                            $('#textoMensaje').addClass('action-alert');
                            $('#textoMensaje').text("No existen datos para los filtros seleccionados.");
                        }
                    },
                    error: function (error) {
                        alert("Error." + error);
                    }
                });
            }
            else {
                alert("Seleccione al menos una empresa");
            }
        }
        else {
            alert("Seleccione rango menor a un mes");
        }
    }
    else {
        alert("Seleccione rango de fechas");
    }
}

exportar = function () {
    var empresa = $('#cbEmpresas').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "";
    $('#hfEmpresa').val(empresa);

    if ($('#txtFechaInicio').val() != "" && $('#txtFechaFin').val() != "") {

        var fecha1 = getFecha(GetDateNormalFormat($('#txtFechaInicio').val()));
        var fecha2 = getFecha(GetDateNormalFormat($('#txtFechaFin').val()));
        var diferencia = numeroDias(fecha1, fecha2);

        if (diferencia <= numeroTotal) {

            if ($('#hfEmpresa').val() != "") {
                $.ajax({
                    type: 'POST',
                    url: controlador + 'exportar',
                    data: {
                        lectcodi: $("input:radio[name='rbTipoDemanda']:checked").val(),
                        empresas: $('#hfEmpresa').val(),
                        fechaInicio: $('#txtFechaInicio').val(),
                        fechaFin: $('#txtFechaFin').val(),
                    },
                    dataType: 'json',
                    success: function (result) {
                        if (result != "-1" && result != "2") {
                            $('#textoMensaje').removeClass();
                            $('#textoMensaje').text("");
                            document.location.href = controlador + 'descargar?file=' + result;
                        }
                        else
                        {
                            if (result == 2)
                            {
                                $('#detalleFormato').html("");
                                $('#textoMensaje').removeClass();
                                $('#textoMensaje').addClass('action-alert');
                                $('#textoMensaje').text("No existen datos para los filtros seleccionados.");
                            }
                        }
                    },
                    error: function () {
                        alert("Error.");
                    }
                });
            }
            else {
                alert("Seleccione al menos una empresa");
            }
        }
        else {
            alert("Seleccione rango menor a un mes");
        }
    }
    else {
        alert("Seleccione rango de fechas");
    }
}

getFecha = function (date) {
    var parts = date.split("/");
    var date = new Date(parts[1] + "/" + parts[0] + "/" + parts[2]);
    return date.getTime();
}

numeroDias = function(inicio, final) {
    return Math.round((final - inicio) / (1000 * 60 * 60 * 24));
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

CambiarMenu = function (tipoMenu) {
    if (tipoMenu === 'Distribuidores') {
        $('#menuDistribuidores').removeClass();
        $('#menuUsuariosLibres').removeClass();
        $('#menuDistribuidores').addClass('sidebar-nav--link sidebar-nav--link-active');
        $('#menuUsuariosLibres').addClass('sidebar-nav--link');
    } else {
        $('#menuDistribuidores').removeClass();
        $('#menuUsuariosLibres').removeClass();
        $('#menuDistribuidores').addClass('sidebar-nav--link');
        $('#menuUsuariosLibres').addClass('sidebar-nav--link sidebar-nav--link-active');
    }
}