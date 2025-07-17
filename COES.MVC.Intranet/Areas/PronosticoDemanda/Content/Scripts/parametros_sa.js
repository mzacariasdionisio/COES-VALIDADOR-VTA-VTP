var httable;
$(document).ready(function () {
    //Aplica la lib. MultiSelect
    $('.f-selectcp').each(function () {
        var element = this;
        $(element).multipleSelect({
            name: element.id,
            filter: true,
            single: false,
            placeholder: 'Seleccione',
            onClose: function () {
                var e = document.getElementById(this.name);

                var Barracp = $('#id-barracp').val();
                ParametrosSAUpdateBarraPM(Barracp);
                ListParametrosSA(e);

            },
             onCheckAll: function () {
                var e = document.getElementById(this.name);
                 $(e).val(null);
                
            }
        });
    });

    $('.f-selectpm').each(function () {
        var element = this;
        $(element).multipleSelect({
            name: element.id,
            filter: true,
            single: false,
            placeholder: 'Seleccione',
            onClose: function () {
                var e = document.getElementById(this.name);
                ListParametrosSA(e);

            },
            onCheckAll: function () {
                var e = document.getElementById(this.name);
                $(e).val(null);

            }
        });
    });

    //$('#id-barracp').change(function () {
    //    var Barracp = $('#id-barracp').val();

    //    ParametrosSAUpdateBarraPM(Barracp);
    //});

    $('#btn-guardar').on('click', function () {
        var hs = {};
        var data = [], id = [];
        var x = ht.getSchema();
        var listaBarras;

        Object.keys(x).forEach(function (key) {
          // console.log(key);
            id.push(key);
            data.push(ht.getDataAtProp(key));
        })
        hs['id'] = id;
        hs['data'] = data;

        SaveParametrosSA(hs);
    });
});

function SaveParametrosSA(hs) {
    $.ajax({
        type: 'POST',
        url: controller + 'ParametrosSASave',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            dataFiltros: hs
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            alert("Se han registrado los datos ......");
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function ListParametrosSA(e) {
    $.ajax({
        type: 'POST',
        url: controller + 'ParametrosSAList',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            dataFiltros: GetFiltersValues()
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            //Crea el handsontable
            var ht_model = FormatHandson(result.data);
            GetHanson(ht_model);
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}
function GetTestData(num_barras) {
    var table = {};
    var tb_header = [];
    var tb_body = [];

    var i = 0;
    tb_header.push('Hora');
    while (i < num_barras) {
        var nhead = 'BARRAPM-' + (i + 1).toString();
        tb_header.push(nhead);
        i++;
    }

    var j = 0;
    while (j < 48) {
        i = 0;
        var tb_row = [];
        tb_row.push(time_hours[j]);

        while (i < num_barras) {
            tb_row.push(0);
            i++;
        }

        tb_body.push(tb_row);
        j++;
    }

    table['header'] = tb_header;
    table['body'] = tb_body;

    return table;
}

function HoraColumnRenderer(instance, td, row, col, prop, value, cellProperties) {
    cellProperties.readOnly = true;
    cellProperties.className = 'htCenter';
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.fontWeight = 'bold';
    td.style.background = '#F2F2F2';
}

function GetFiltersValues() {
    var model = {};
    model['SelById'] = $('#id-byid').val();
    model['SelAreas'] = $('#id-area').val();
    model['SelBarraPM'] = $('#id-barrapm').val();
    model['SelBarraCP'] = $('#id-barracp').val();
    return model;
}

//Crea el Handsontable
function GetHanson(model) {
    $('#httable').html('');
    var container = document.getElementById('httable');
    ht = new Handsontable(container, {
        data: model.data,
        fillHandle: true,
        stretchH: 'all',
        maxCols: model.maxCols,
        maxRows: model.maxRows,
        minSpareCols: 0,
        minSpareRows: 0,
        columns: model.columns,
        colHeaders: model.colHeaders
    });
}

//Formatea el modelo de datos para el Handsontable
function FormatHandson(model) {
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
        //col['title'] = item.label;

        switch (item.htrender) {
            case 'hora':
                col['type'] = 'text';
                col['className'] = 'htCenter';
                col['readOnly'] = true;
                col['renderer'] = HoraColumnRenderer;
                break;
            case 'normal':
                col['type'] = 'numeric';
                col['format'] = '0.00';
                col['readOnly'] = false;
                col['allowInvalid'] = false;
                col['allowEmpty'] = false;
                break;
            case 'patron':
                col['type'] = 'numeric';
                col['format'] = '0.00';
                col['readOnly'] = true;
                col['renderer'] = PatronColumnRenderer;
                break;
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

function ParametrosSAUpdateBarraPM(barracp) {
    $.ajax({
        type: 'POST',
        url: controller + 'ParametrosSAUpdateBarraPM',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            barracp: barracp
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {

            RefillDropDowList($('#id-barrapm'), result, 'Grupocodi', 'Gruponomb');

        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

//Llena el contenido de una lista desplegable
function RefillDropDowList(element, data, data_id, data_name) {
    //Vacia el elemento
    element.empty();
    //Carga el elemento
    $.each(data, function (i, item) {
        var n_value = i, n_html = item;
        if (data_id) n_value = item[data_id];
        if (data_name) n_html = item[data_name];
        element.append($('<option></option>').val(n_value).html(n_html));
    });
    //Actualiza la lib.multipleselect
    element.multipleSelect('refresh');
}
