var controlador = siteRoot + 'admin/solicitud/'
var container = document.getElementById('excelWeb');
var hot;
var validacionLogin = false;
var validacionExistencia = false;

$(document).ready(function () {
       
    $('#btnCancelar').click(function () {
        document.location.href = controlador + "index";
    });


    if ($('#hfTipoSolicitud').val() != "") {
        cargarConfiguracion($('#hfTipoSolicitud').val());
    }

    
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

cargarExcel = function () {
        
    $.ajax({
        type: 'POST',
        url: controlador + "edicionexcel",
        dataType: 'json',
        data: {            
            idSolicitud: $('#hfIdSolicitud').val()
        },
        traditional: true,
        success: function (result) {
            $('#contenedor').html("");            

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
                                       
                    cellProperties.readOnly = true;
                    
                    return cellProperties;
                },
                mergeCells: [
                  { row: 0, col: 0, rowspan: 1, colspan: 3 },
                  { row: 0, col: 3, rowspan: 1, colspan: result.Columnas.length }
                ],
                afterRender: function () {
                    render_color(this);
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
    $.ajax({
        type: 'POST',
        url: controlador + 'usuarios',
        data: {            
            idSolicitud: $('#hfIdSolicitud').val()
        },
        success: function (evt) {
            $('#contenedor').html(evt);
        },
        error: function () {
            mostrarError();
        }
    });
}

mostrarError = function ()
{
    alert("Error");
}
