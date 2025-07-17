var name = "CumplimientoDiario/";
var controller = siteRoot + "IND/" + name;
var imageRoot = siteRoot + "Content/Images/";
var dt_cumplimiento, objHtVer, objHtEditar;
var popVer, popEditar;
const textValidator = /^[SN]/;
$(document).ready(function () {
    $('#btnConsultar').on('click', function () {
        listarCumplimiento();
    });

    $('#btn-pop-ejecutar').on('click', function () {
        setTimeout(() => {
            grabarCumplimiento();
        }, 200);
    });

    //$('#btnReporte').on('click', function () {
    //    reporte();
    //});

    listarCumplimiento();
});

//function reporte() {
//    $.ajax({
//        type: 'POST',
//        url: controller + 'reporte',
//        contentType: 'application/json; charset=utf-8',
//        data: JSON.stringify({
//            reporte: 1
//        }),
//        datatype: 'json',
//        success: function (result) {
//        },
//        error: function () {
//            alert("Ha ocurrido un problema...");
//        }
//    });
//}

function listarCumplimiento() {
    $('#dt-cumplimiento').html("");

    $.ajax({
        type: 'POST',
        url: controller + 'ListaCumplimiento',
        data: {
            periodo: $('#cboPeriodo').val(),
            empresa: $('#cboEmpresa').val(),
            estado: $('#cboEstado').val(),
        },
        datatype: 'json',
        success: function (result) {
            dt_cumplimiento = $('#dt-cumplimiento').DataTable({
                data: result.ListaCumplimiento,
                columns: [
                    { title: '', data: null },
                    { title: '', data: null },
                    { title: '', data: null },
                    { title: 'Cod. Envio', data: 'Crdsgdcodi' },
                    { title: 'Empresa', data: 'Emprnomb' },
                    { title: 'Central', data: 'Equinombcentral' },
                    { title: 'Undidad', data: 'Equinombunidad' },
                    { title: 'Fecha envio', data: 'Indcbrfecha' },
                    { title: 'Usuario', data: 'Indcbrusucreacion' },
                    { title: 'Cumplimiento', data: 'Cumplimiento' },
                    { title: '%', data: 'Porcentaje' },
                ],
                initComplete: function () {
                    $('#dt-cumplimiento').css('width', '100%');
                    $('.dataTables_scrollHeadInner').css('width', '100%');
                    $('.dataTables_scrollHeadInner table').css('width', '98.84%');
                },
                columnDefs:
                    [
                        {
                            targets: [0], width: 30,
                            defaultContent: '<img class="clsEditar" src="' + imageRoot + 'btn-edit.png" style="cursor: pointer;" title="Editar"/> '

                        },
                        {
                            targets: [1], width: 30,
                            defaultContent: '<img class="clsVer" src="' + imageRoot + 'btn-open.png" style="cursor: pointer;" title="Ver"/> '

                        },
                        {
                            targets: [2], width: 30,
                            defaultContent: '<img class="clsExportar" src="' + imageRoot + 'downloadExcel.png" style="cursor: pointer;" title="Exportar"/> '

                        }
                    ],
                searching: false,
                bLengthChange: false,
                bSort: true,
                destroy: true,
                paging: true,
                pageLength: 25,
                info: false,
            });
        }
    });
}

function grabarCumplimiento() {
    $.ajax({
        type: 'POST',
        url: controller + 'GrabaCumplimiento',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            htEditar: objHtEditar.getData()[0],
            codigo: $('#crdCodigo').val(),
        }),
        datatype: 'json',
        success: function (result) {
            const mensaje = result.Mensaje;
            console.log(mensaje, 'mensaje ....');
            if (mensaje.msjTipo == 0) {
                popEditar.close();
                SetMessage('#message', mensaje.msjData, 'success', true);
                listarCumplimiento();
            } else {
                alert(mensaje.msjData);
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function exportarCumplimiento(row) {
    console.log(row, 'fila');
    $.ajax({
        type: 'POST',
        url: controller + 'ExportaCumplimiento',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            datos: row
        }),
        datatype: 'json',
        success: function (result) {
            if (result != -1) {
                window.location = controller + 'abrirarchivo?formato=' + 1 + '&file=' + result;
                mostrarMensaje('mensaje', 'exito', "Felicidades, el archivo se descargo correctamente...!");
            }
            else {
                mostrarMensaje('mensaje', 'error', "Lo sentimos, ha ocurrido un error inesperado");
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

$(document).on('click', '#dt-cumplimiento tr td .clsVer', function (e) {
    var row = $(this).closest('tr');
    var r = dt_cumplimiento.row(row).data();

    popVer = $('#popupVer').bPopup({
        easing: 'easeOutBack',
        speed: 350,
        transition: 'fadeIn',
        modalClose: false,
        onOpen: function () {
            SetMessage('#pop-mensaje-ver',
                '• La S indica que se reporto en Plazo.<br>' +
                '• La N indica que se reporto Fuera de Plazo.',
                'info');
        }
    },
        function () {
            $.ajax({
                type: 'POST',
                url: controller + 'EstadoInformacionPlazo',
                dataType: 'json',
                data: {
                    periodo: r.Ipericodi,//$('#cboPeriodo').val(),
                    codigo: r.Crdsgdcodi,
                    tipo: 'ver'
                },
                success: function (result) {
                    //listarCuadros();
                    var ht_ver = FormatoHandson(result.CrdEstado.data);
                    obtenerHandsonVer(ht_ver);
                    //mostrarMensaje(result.dataMsg, result.typeMsg);
                },
                error: function () {
                    alert("Ha ocurrido un problema...");
                }
            });
        }
    );
})

$(document).on('click', '#dt-cumplimiento tr td .clsEditar', function (e) {
    var row = $(this).closest('tr');
    var r = dt_cumplimiento.row(row).data();

    popEditar = $('#popupEditar').bPopup({
        easing: 'easeOutBack',
        speed: 350,
        transition: 'fadeIn',
        modalClose: false,
        onOpen: function () {
            SetMessage('#pop-mensaje-editar',
                '• La S indica que se reporto en Plazo.<br>' +
                '• La N indica que se reporto Fuera de Plazo.',
                'info');
        }
    },
        function () {
            $.ajax({
                type: 'POST',
                url: controller + 'EstadoInformacionPlazo',
                dataType: 'json',
                data: {
                    periodo: r.Ipericodi,//$('#cboPeriodo').val(),
                    codigo: r.Crdsgdcodi,
                    tipo: 'edit'
                },
                success: function (result) {
                    //listarCuadros();
                    document.getElementById("crdCodigo").value = result.CrdEstado.codigo;
                    var ht_editar = FormatoHandson(result.CrdEstado.data);
                    obtenerHandsonEditar(ht_editar);
                    //mostrarMensaje(result.dataMsg, result.typeMsg);
                },
                error: function () {
                    alert("Ha ocurrido un problema...");
                }
            });
        }
    );
})

$(document).on('click', '#dt-cumplimiento tr td .clsExportar', function (e) {
    var row = $(this).closest('tr');
    var r = dt_cumplimiento.row(row).data();

    exportarCumplimiento(r);
})

function SetMessage(container, msg, tpo, del) {//{Contenedor, mensaje(string), tipoMensaje(string), delay(bool)}
    var new_class = "msg-" + tpo;//Identifica la nueva clase css
    $(container).removeClass($(container).attr('class'));//Quita la clase css previa
    $(container).addClass(new_class);
    $(container).html(msg);//Carga el mensaje
    //$(container).show();
    //Mensaje con delay o no
    if (del) $(container).show(0).delay(5000).hide(0);//5 Segundos
    else $(container).show();
}

function obtenerHandsonVer(model) {
    $('#htVer').html('');
    var containerVer= document.getElementById('htVer');
    objHtVer = new Handsontable(containerVer, {
        data: model.data,
        fillHandle: true,
        stretchH: 'all',
        height: 100,
        width: 800,
        maxCols: model.maxCols,
        maxRows: model.maxRows,
        minSpareCols: 0,
        minSpareRows: 0,
        columns: model.columns,
        colHeaders: model.colHeaders
    });
}

function obtenerHandsonEditar(model) {
    $('#htEditar').html('');
    var containerEditar = document.getElementById('htEditar');
    objHtEditar = new Handsontable(containerEditar, {
        data: model.data,
        fillHandle: true,
        stretchH: 'all',
        height: 100,
        width: 800,
        maxCols: model.maxCols,
        maxRows: model.maxRows,
        minSpareCols: 0,
        minSpareRows: 0,
        columns: model.columns,
        colHeaders: model.colHeaders
    });
}

//Formatea el modelo de datos para el Handsontable
function FormatoHandson(model) {
    var handson = {};
    var data = [], columns = [], headers = [];
    var rule = ['no'];

    $.each(model, function (i, item) {
        //crea la propiedad "colHeaders"
        if (!(rule.includes(item.htrender))) {
            headers.push(item.label);
        }

        //Crea la propiedad "columns"
        var col = {};
        col['data'] = item.id;

        switch (item.htrender) {
            case 'hora':
                col['type'] = 'text';
                col['className'] = 'htCenter';
                col['readOnly'] = true;
                col['renderer'] = HoraColumnRenderer;
                break;
            case 'ver':
                col['type'] = 'text';
                //col['format'] = '0.00';
                col['readOnly'] = true;
                break;
            case 'patron':
                col['type'] = 'numeric';
                col['format'] = '0.00';
                col['readOnly'] = true;
                col['renderer'] = PatronColumnRenderer;
                break;
            case 'edit':
                col['type'] = 'text';
                col['readOnly'] = false;
                col['allowInvalid'] = true;
                col['allowEmpty'] = false;
                col['validator'] = valueValidator;//textValidator;
                break;
            case 'final':
                col['type'] = 'numeric';
                col['format'] = '0.00';
                col['readOnly'] = true;
                col['renderer'] = PatronColumnRenderer;
        }

        if (!(rule.includes(item.htrender))) {
            columns.push(col);

            //Crea la propiedad "data"
            $.each(item.data, function (j, value) {
                var row = {};
                if (data[j]) {
                    data[j][item.id] = value;
                }
                else {
                    row[item.id] = value;
                    data.push(row);
                }
            });
        }
    });

    handson['colHeaders'] = headers;
    handson['columns'] = columns;
    handson['data'] = data;
    handson['maxCols'] = columns.length;
    handson['maxRows'] = data.length;
    return handson;
}

const valueValidator = (value, callback) => {
    setTimeout(() => {
        let longitud = value.length;
        console.log(value, 'value');

        if (value == 'S' || value == 'N') {
            callback(true);
        } else {
            callback(false);
        }
    }, 200);
};