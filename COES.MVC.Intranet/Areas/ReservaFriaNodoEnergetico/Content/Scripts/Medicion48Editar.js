var controlador = siteRoot + 'reservafrianodoenergetico/medicion48/';

$(function () {

    $('#txtMedifecha').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"        
    });

    $('#txtMedifecha').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtMedifecha').val(date);
        }
    });

    $('#txtLastdate').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"        
    });

    $('#txtLastdate').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtLastdate').val(date);
        }
    });

    $('#btnCancelar').click(function () {
        document.location.href = controlador;
    });
    
    $(document).ready(function () {
        if ($('#hfAccion').val() == 0) {
            $('#btnGrabar').hide();
        }
    });

    $('#btnGrabar').click(function () {
        grabar();
    });

    $(document).ready(function () {
        
        $('#cbxTipoinfocodi').val($('#hfTipoinfocodi').val());
        $('#cbxTipoinfocodi').prop("disabled", true);

        cargarGrilla();

        $('#cbxLectcodi').val($('#hfLectcodi').val());
        $('#cbxTipoinfocodi').val($('#hfTipoinfocodi').val());
        $('#cbxPtomedicodi').val($('#hfPtomedicodi').val());
        $('#cbxPtomedicodi').val($('#hfPtomedicodi').val());
        
        if ($('#hfPtomedicodi').val() != -1) {
            $('#cbxTipoinfocodi').prop("disabled", true);
            $('#cbxLectcodi').prop("disabled", true);
            $('#cbxPtomedicodi').prop("disabled", true);
            $('#txtMedifecha').prop("disabled", true);
        }
    });

});

mostrarAlerta = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-alert');
    $('#mensaje').html(mensaje);
}

mostrarExito = function () {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-exito');
    $('#mensaje').html('La operación se realizó con éxito.');
}

validarRegistro = function () {

    var mensaje = "<ul>";
    var flag = true;

    $('#hfLectcodi').val($('#cbxLectcodi').val());
    $('#hfTipoinfocodi').val($('#cbxTipoinfocodi').val());
    $('#hfPtomedicodi').val($('#cbxPtomedicodi').val());
    $('#hfPtomedicodi').val($('#cbxPtomedicodi').val());
    
    if ($('#cbxLectcodi').val() == "-1" || $('#cbxLectcodi').val() == null || $('#cbxLectcodi').val() == undefined) {
        mensaje += "<li>Debe seleccionar lectura</li>";//\n";
        flag = false;
    }

    if ($('#cbxPtomedicodi').val() == "-1" || $('#cbxPtomedicodi').val() == null || $('#cbxPtomedicodi').val() == undefined) {
        mensaje += "<li>Debe seleccionar descripción (punto)</li>";//\n";
        flag = false;
    }

    if (flag) mensaje = "";
    return mensaje;
}

cargarGrilla = function () {

    var idlect = $('#hfLectcodi').val();
    var fecha = $('#txtMedifecha').val();
    var idtipoinfo = $('#cbxTipoinfocodi').val();
    var id = $('#hfPtomedicodi').val();


    $.ajax({
        type: 'POST',
        url: controlador + 'obtenerpotenciadetalle',
        datatype: 'json',
        data: {
            idlect: idlect,
            fecha: fecha,
            idtipoinfo: idtipoinfo,
            id: id
        },
        success: function(result) {
            errors = [];
           

            var container = document.getElementById('contenedor');

            merge = result.Merge;
            titulos = result.IndicesTitulo;
            subTitulos = result.IndicesSubtitulo;
            validaciones = result.Validaciones;
            agrupaciones = result.IndicesAgrupacion;
            comentarios = result.IndicesComentario;

            var tituloRenderer = function(instance, td, row, col, prop, value, cellProperties) {
                Handsontable.renderers.TextRenderer.apply(this, arguments);
                td.style.fontWeight = 'bold';
                td.style.textAlign = 'left';
                td.style.color = '#fff';
                td.style.backgroundColor = '#1991B5';
            };

            var subTituloRenderer = function(instance, td, row, col, prop, value, cellProperties) {
                Handsontable.renderers.TextRenderer.apply(this, arguments);
                td.style.backgroundColor = '#D7EFEF';
                td.style.textAlign = 'center';
                td.style.fontWeight = 'bold';
                td.style.color = '#1C91AE';
                td.style.verticalAlign = 'middle';
            };

            var data = result.Datos;



            var titulos = result.IndicesTitulo;
            
            var defaultRenderer = function(instance, td, row, col, prop, value, cellProperties) {
                Handsontable.renderers.TextRenderer.apply(this, arguments);
                td.style.backgroundColor = '#FFF';
                td.style.fontSize = '11px';
            };

            
            var hotOptions = {
                data: data,
                colHeaders: true,
                colHeaders: ['Hora', 'MW'],
                columns: [
                    {
                        type: 'time',
                        timeFormat: 'HH:mm',
                        correctFormat: true
                    },
                    { type: 'numeric' }
                ],
                rowHeaders: true,
                comments: true,
                width: 400,
                height: 570,
                colWidths: [70, 110, 110, 110, 110, 110, 110],
                mergeCells: merge,
                afterCreateRow: function(index, amount) {
                    hotOptions.mergeCells = merge;
                    hot.mergeCells = new Handsontable.MergeCells(hotOptions.mergeCells);
                    hot.updateSettings(hotOptions);
                },
                afterRemoveRow: function(index, amount) {

                    for (var i in subTitulos) {
                        if (index < subTitulos[i] && index > 2) {
                            subTitulos[i] = subTitulos[i] - 1;
                        }
                    }
                },
                afterLoadData: function() {
                    this.render();
                },
                afterValidate: function(isValid, value, row, prop, source) {
                    if (value != "") {
                        if (!isValid) {
                            errors.push({ row: row, column: prop });
                        } else {
                            for (var j in errors) {
                                if (errors[j]['row'] == row && errors[j]['column'] == prop) {
                                    errors.splice(j, 1);
                                }
                            }
                        }
                    } else {
                        for (var j in errors) {
                            if (errors[j]['row'] == row && errors[j]['column'] == prop) {
                                errors.splice(j, 1);
                            }
                        }
                        return true;
                    }
                }
            };

            hot = new Handsontable(container, hotOptions);
        },
        error: function() {
            mostrarError();
        }
    });
}

grabar = function () {
    var mensaje = validarRegistro();
    
    var idlect = $('#hfLectcodi').val();
    var fecha = $('#txtMedifecha').val();
    var idtipoinfo = $('#cbxTipoinfocodi').val();
    var id = $('#hfPtomedicodi').val();

    var datos = hot.getData();
    $('#hfDatos').val(datos);

    if (mensaje == "") {

        $.ajax({
            type: 'POST',
            url: controlador + "grabar",
            dataType: 'json',
            data: {
                idlect: idlect,
                fecha: fecha,
                idtipoinfo: idtipoinfo,
                id: id,
                dataExcel: $('#hfDatos').val()
            },
            success: function (result) {
                if (result != "-1") {
                   
                    mostrarExito();
					$('#hfLectcodi').val(result);
					$('#hfMedifecha').val(result);
					$('#hfTipoinfocodi').val(result);
					$('#hfPtomedicodi').val(result);
					document.location.href = controlador;

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
    else {
        mostrarAlerta(mensaje);
    }
}

mostrarError = function () {
    alert("Ha ocurrido un error");

}

