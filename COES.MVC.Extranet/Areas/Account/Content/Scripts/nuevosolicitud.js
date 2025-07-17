var controlador = siteRoot + 'account/solicitar/'
var container = document.getElementById('excelWeb');
var hot;
var validacionLogin = false;
var validacionExistencia = false;

$(document).ready(function () {
       
    $('#btnCancelar').click(function () {
        document.location.href = controlador + "index";
    });

    $('#btnGrabar').click(function () {
        
        if ($("input:radio[name='rblTipoSolicitud']:checked").val() != null) {
            if (confirm("¿Está seguro de enviar la solicitud?")) {
                grabarSolicitud();
            }
        }
        else {
            alert("Seleccione el tipo de solicitud.");
        }
    });

    $('input[name="rblTipoSolicitud"][value="' + $('#hfTipoSolicitud').val() + '"]').attr('checked', true);

    if ($('#hfTipoSolicitud').val() != "") {
        cargarConfiguracion($('#hfTipoSolicitud').val());
    }

    $('input[name="rblTipoSolicitud"]').click(function () {
        
        cargarConfiguracion($("input:radio[name='rblTipoSolicitud']:checked").val());
    });
});

cargarConfiguracion = function (elemento) {

    if (elemento == "N") {
        cargarExcel(elemento);
    }
    else if (elemento == "B") {
        cargarUsuarios();
    }
    else if (elemento == "M") {
        cargarExcel(elemento);
    }
}

var headerColor = '#459CD6';
var headerRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.fontWeight = 'bold';
    td.style.color = '#fff';
    td.style.backgroundColor = headerColor;
    td.style.textAlign = "center";
};

var tituloRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.fontWeight = 'bold';
    td.style.color = '#A8A8A8';
    td.style.backgroundColor = "#FFF0B7";
    td.style.textAlign = "center";
    td.style.verticalAlign = "middle";
};

cargarExcel = function (elemento) {

    var accion = (elemento == "N") ? "pintarexcel" : "pintarexceledicion";
    var idSolicitud = $('#hfIdSolicitud').val();
    if (idSolicitud > 0) accion = "edicionexcel";
    $.ajax({
        type: 'POST',
        url: controlador + accion,
        dataType: 'json',
        data: {
            idEmpresa: $('#hfEmpresa').val(),
            idSolicitud: idSolicitud           
        },
        traditional: true,
        success: function (result) {

            $('#contenedor').html("");
            $('#mensaje').css("display", "block");
            $('#mensaje').removeClass();
            $('#mensaje').addClass("action-message");
            
            if (idSolicitud > 0)
            {
                $('#contentTipo').css('display', 'none');
                $('#mensaje').css('display', 'none');
                $('#btnGrabar').css('display', 'none');
            }
            else {
                if (elemento == "N") {
                    $('#mensaje').html("<strong>2.</strong> Ingrese por cada usuario<strong> nombre completo, correo institucional  y marque con una X los módulos a los que accederá.");
                }
                else {
                    $('#mensaje').html("<strong>2.</strong> Modifique los datos de los usuarios, borre las filas de registros que no se modificarán.");
                }
            }

            var iconColumn = [
                {
                    type: "text",
                    readOnly: false
                },
                {
                    type: "text",
                    readOnly: false
                },
                {
                    type: "text",
                    readOnly: false
                }
            ];

            var iconHeader = ["Usuario", "Correo", "Telefono"];
            var iconWidth = [250, 250, 130];
            var columns = iconColumn.concat(result.Columnas);
            var widths = iconWidth.concat(result.Widths);
            var container = document.getElementById('contenedor');

            hot = new Handsontable(container, {
                data: result.Data,
                maxRows: result.Data.length,
                maxCols: result.Data[0].length,
                colWidths: widths,
                minSpareRows: 1,
                columns: columns,
                colWidths: widths,
                cells: function (row, col, prop) {
                    var cellProperties = {};

                    if (row == 0 || row == 1) {
                        cellProperties.renderer = headerRenderer;
                        cellProperties.readOnly = true;
                    }

                    if (row == 1 && col <= 3) {
                        cellProperties.renderer = tituloRenderer;                       
                    }

                    if (row == 1 && col >= 3) {
                        cellProperties.renderer = "html";
                        cellProperties.readOnly = true;
                    }

                    if (row > 1 && col >= 3) {
                        cellProperties.type = 'dropdown';
                        cellProperties.source = ["X"];
                    }

                    if (idSolicitud > 0) {
                        cellProperties.readOnly = true;
                    }

                    return cellProperties;
                },
                mergeCells: [
                  { row: 0, col: 0, rowspan: 1, colspan: 3 },
                  { row: 0, col: 3, rowspan: 1, colspan: result.Columnas.length }
                ],
                afterRender: function () {
                    render_color(this);
                },
                beforeKeyDown: function (e) {
                    if (e.keyCode < 37 || e.keyCode > 40) return;  
                    var edit = hot.getActiveEditor(); 
                    if (!edit || !edit._opened) return; 
                    if (e.keyCode == 37 && edit.TEXTAREA.selectionStart > 0) return; 
                    if (e.keyCode == 39 && edit.TEXTAREA.selectionEnd < edit.TEXTAREA.value.length) return; 
                    var selection = hot.getSelected(); if (selection && selection.length > 1) { var row = selection[0]; 
                        var col = selection[1]; 
                        hot.selectCell(row, col);
                    }
                }
            });
            hot.render();

            function render_color(ht) {
                for (var i = 0; i < result.Columnas.length ; i++) {
                    cell_color = "#000";
                    font_color = "#fff";
                    $(ht.getCell(1, i + 3)).css(
                        {
                            "color": "#999999",
                            "background-color": "#E8F6FF",
                            "line-height": "12px",
                            "font-size": "11px",
                            "font-weight": "bold",
                            "text-align": "center"
                        })
                }
            }
        },
        error: function () {
            mostrarError()
        }
    });
}

cargarUsuarios = function ()
{
    idSolicitud = $('#hfIdSolicitud').val()
    $.ajax({
        type: 'POST',
        url: controlador + 'usuarios',
        data: {
            idEmpresa: $('#hfEmpresa').val(),
            idSolicitud: idSolicitud
        },
        success: function (evt) {
            $('#contenedor').html(evt);

            if (idSolicitud == 0) {
                $('#tabla').dataTable({
                });

                $('#mensaje').css("display", "block");
                $('#mensaje').html('<strong>2.</strong> Seleccione los usuarios a los que se dará de baja y presione <strong>"Enviar solicitud" </strong>.')
                $('#mensaje').removeClass();
                $('#mensaje').addClass("action-message");
            }
            else {
                $('#btnGrabar').css('display', 'none');
                $('#contentTipo').css('display', 'none');
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

grabarSolicitud = function () {

    var indicador = $("input:radio[name='rblTipoSolicitud']:checked").val();

    if (indicador == "N" || indicador == "M") {
        solicitarCreacion(indicador);
    }
    else if (indicador == "B"){
        solicitarBaja();
    }   
}

solicitarCreacion = function (indicador) {

    var datos = hot.getData(2, 0, hot.countRows() - 1, hot.countCols() - 1);
    validacionLogin = true;
    validacionExistencia = false;
    var validacion = validarDatos(datos);

    if (validacion.length == 0 && validacionLogin) {

        if (validacionExistencia) {

            $.ajax({
                type: "POST",
                url: controlador + 'grabarsolicitud',
                dataType: "json",
                contentType: 'application/json',
                traditional: true,
                data: JSON.stringify({
                    datos: datos,
                    idEmpresa: $('#hfEmpresa').val(),
                    indicador: indicador
                }),
                success: function (result) {
                    if (result > 0) {
                        $('#mensaje').removeClass();
                        $('#mensaje').addClass("action-exito");
                        $('#mensaje').html("La solicitud se envío correctamente. Un representante del COES atenderá su solicitud. Nro Solicitud: <strong style='color:red'>" + result + "</strong>");

                        $('#btnGrabar').css('display', 'none');
                        $('#contentTipo').css('display', 'none');

                        hot.updateSettings({
                            cells: function (row, col, prop) {
                                var cellProperties = {};

                                if (row == 0 || row == 1) {
                                    cellProperties.renderer = headerRenderer;
                                    cellProperties.readOnly = true;
                                }

                                if (row == 1 && col <= 3) {
                                    cellProperties.renderer = tituloRenderer;
                                }

                                if (row == 1 && col >= 3) {
                                    cellProperties.renderer = "html";
                                    cellProperties.readOnly = true;
                                }
                                if (row > 1) {
                                    cellProperties.readOnly = true;
                                }
                                return cellProperties;
                            }
                        });

                        hot.render();
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
            $('#mensaje').removeClass();
            $('#mensaje').addClass("action-alert");
            $('#mensaje').text("Ingrese los datos de al menos un usuario.");
        }
    }
    else {
        for (i = 0; i < validacion.length; i++) {
            for (j = 0; j < hot.countCols() ; j++) {
                $(hot.getCell(parseInt(validacion[i]) + 2, j)).css({
                    "background-color": "#FF9393"
                })
            }
        }

        var mensaje = "";
        if (validacion.length > 0 && !validacionLogin) mensaje = "Revise las filas de rojo: Debe ingresar : Nombres, Email Válido y módulos a los que accederá, y <strong> no debe repetir el email.</strong>";
        else if (validacion.length > 0 && validacionLogin) mensaje = "Revise las filas de rojo: Debe ingresar : Nombres, Email Válido y módulos a los que accederá";
        
        $('#mensaje').html(mensaje);
        $('#mensaje').removeClass();
        $('#mensaje').addClass('action-alert');
    }
}

solicitarBaja = function () {
    
    var usuarios = "";
    var count = 0;
    $('#tabla input:checked').each(function () {
        usuarios = usuarios + $(this).val() + ",";
        count++;
    });

    if (count > 0) {
        $.ajax({
            type: 'POST',
            url: controlador + 'grabarbaja',
            data: {
                usuarios: usuarios,
                idEmpresa: $('#hfEmpresa').val()
            },
            dataType: 'json',
            cache: false,
            success: function (resultado) {
                if (resultado > 0) {
                    $('#mensaje').removeClass();
                    $('#mensaje').addClass("action-exito");
                    $('#mensaje').html("La solicitud se envío correctamente. Un representante del COES atenderá su solicitud. Nro Solicitud: <strong style='color:red'>" + resultado + "</strong>");

                    $('#btnGrabar').css('display', 'none');
                    $('#contentTipo').css('display', 'none');

                    $("#tabla").find("*").attr("disabled", "disabled");
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
    else{
        $('#mensaje').text("Seleccione los usuarios que solicitará dar de baja");
        $('#mensaje').removeClass();
        $('#mensaje').addClass("action-alert");
    }
}

validarDatos = function (datos) {
    var lista = [];
    var emails = [];
    for (var row in datos) {
        var flagNombre = false;
        var flagEmail = false;
        var flagValidEmail = false;
        var flagModulo = false;

        if (datos[row][0] != "") {
            flagNombre = true;
        }
        if (datos[row][1] != "") {
            flagEmail = true;
            if (validarEmail(datos[row][1])) {
                flagValidEmail = true;
            }           
        }
        for (var i = 3; i < datos[row].length; i++)
        {
            if (datos[row][i] != "") {
                flagModulo = true;
            }
        }
       
        if (flagNombre && flagValidEmail && flagModulo) {
           
            if (flagValidEmail) {
                if (emails.indexOf(datos[row][1]) > -1) {
                    validacionLogin = false;
                    lista.push(row);
                }
                else {
                    emails.push(datos[row][1]);
                    validacionExistencia = true;
                }
            }
        }
        else if (flagNombre || flagEmail || flagModulo || flagValidEmail) {
            lista.push(row);
        }
        else if (!(flagNombre && flagEmail && flagModulo))
        {
            //no se considera en el grabado
        }
    }
    return lista;
}

mostrarError = function ()
{
    alert("Error");
}

validarEmail = function (email) {
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
}