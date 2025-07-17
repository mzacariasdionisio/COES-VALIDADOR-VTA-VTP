var ht, pop_ht, hc, dataParametros;
var isActive = false, isValid = false;
var arrayBloques = [];

var data_SDiario = [];
var test;

$(document).ready(function () {
    //Aplica la lib. Zebra_Datepicker
    $('.f-fecha').Zebra_DatePicker({
        onSelect: function () {
            $.ajax({
                url: LoadVersion(),
                success: function () {
                    LoadData();
                }
            });
        }
    });

    $('.f-fecha-reporte').Zebra_DatePicker({
        onSelect: function () {            
            LoadVersionReporte();
        }
    });

    //Assetec 20220927
    $('#chk-negativo').on('change', function () {
        const e = $(this).is(":checked");
        $('#id-negativo').prop('disabled', !e);
    });

    //Assetec 20220224
    $('input[type=radio][name="id-tipoFuente"]').change(function () {
        GetDataCalendar();
    });

    //Assetec 20220224
    $('.toolHistorico').Zebra_DatePicker({
        onSelect: function () {
            const m = {
                id: this.attr('id'),
                fecha: this.val()
            };
            actualizarMedicion(m);
        }
    });

    //Aplica la lib. Zebra_Datepicker al calendario del popup ejecutar
    $('#id-fecha-ejecutar').Zebra_DatePicker();

    //Aplica la lib. MultiSelect
    $('.f-select').each(function () {
        var element = this;
        $(element).multipleSelect({
            name: element.id,
            filter: true,
            single: true,
            placeholder: 'Seleccione',
            onClose: function () {
                const e = document.getElementById(this.name);
                if (e.id == 'id-barra')
                    $('#id-agrupacion').multipleSelect('uncheckAll');
                if (e.id == 'id-agrupacion')
                    $('#id-barra').multipleSelect('uncheckAll');
                LoadData();
            }
        });
    });

    $('.f-pop-select').each(function () {
        var element = this;
        $(element).multipleSelect({
            name: element.id,
            filter: true,
            single: true,
            onClose: function () {
                test = this.name;
                if (this.name == 'id-tipo') {
                    dta = GetDataSelect($('#id-tipo').val());
                    RefillDropDowList($('#id-nro'), dta, 'value', 'text', null);
                }
            }
        });
    });

    $('#id-exportar-version').multipleSelect({        
        filter: true,
        single: true,
        placeholder: 'Seleccione'        
    });

    $('#id-pop-barra').multipleSelect({
        name: 'id-pop-barra',
        filter: true,
        single: false,
        placeholder: 'Seleccione',
        onCheckAll: function () {
            var e = document.getElementById(this.name);
            $(e).val(null);
        }
    });

    $('.cls-bloque').on('change', function () {
        var id = this.id;
        var valor = parseFloat(this.value);
        if (!(isNaN(valor))) {
            EditarPorBloque(id, valor);
        }
    });

    $('#btn-guardar').on('click', function () {
        if (!isValid) {
            SetMessage('#message',
                'Debe ejecutar el proceso de cálculo del pronóstico de la demanda para realizar ajustes',
                'warning');
            return false;
        }
        //Obtiene datos para el registro
        var dataAjuste = FormatMedicion(ht.getDataAtProp('ajuste'));
        var dataBase = FormatMedicion(ht.getDataAtProp('base'));
        //Función para el registro
        Save(dataAjuste, dataBase);
    });

    $('#btn-pronostico').on('click', function () {
        pop = $('#popup').bPopup({
            easing: 'easeOutBack',
            speed: 350,
            transition: 'fadeIn',
            modalClose: false,
            onOpen: function () {
                SetMessage('#pop-mensaje',
                    '• Seleccione la configuración para el cálculo del pronóstico antes de "Ejecutar".<br>' +
                    '• Se iniciará el pronóstico a partir de la "Fecha de referencia", esta no sera incluida.<br>' +
                    '• El pronóstico se ejecutará para todas las "Barras CP" registradas en la versión activa.<br>' +
                    '• Dependiendo del nro. de dias o semanas el proceso puede tardar varios minutos.',
                    'info');

                //Carga la lista desplegable de nros
                dta = GetDataSelect($('#id-tipo').val());
                RefillDropDowList($('#id-nro'), dta, 'value', 'text', null);
            },
            onClose: function () {
                $('#id-pop-barra').multipleSelect('uncheckAll');
            }
        });
    });

    $('#btn-export').on('click', function () {
        pop = $('#popupExport').bPopup({
            easing: 'easeOutBack',
            speed: 350,
            transition: 'fadeIn',
            modalClose: false,
            onOpen: function () {
                SetMessage('#pop-mensaje-export',
                    '• Seleccione el rango de fechas del pronóstico antes de "Exportar".<br>' +
                    '• Se iniciará el pronóstico a partir de la "Fecha de inicio", esta si sera incluida.<br>' +
                    '• El reporte se ejecutará para todas las "Barras CP" registradas en la versión activa.<br>' +
                    '• Se finalizará el pronóstico con la "Fecha de fin", esta si sera incluida.',
                    'info');
            }
        });
    });

    $('#btn-pop-ejecutar').on('click', function () {        
        const idFuente = $('input[name=id-fuente]:checked').val();

        const version = $('#id-nom-version').val();        
        if (version.trim() == '')
            return alert("El nombre de la versión no puede estar vacío... ");

        let valNegativo = -1;
        const chkNegativo = $('#chk-negativo').is(":checked");
        if (chkNegativo) {
            valNegativo = $('#id-negativo').val();
            if (!valNegativo || valNegativo < 0)
                return alert("El valor de corrección no puede ser vacio o menor a 0");
        }

        $.ajax({
            type: 'POST',
            url: controller + 'PronosticoPorBarrasEjecutar',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({
                regFecha: $('#id-fecha-ejecutar').val(),
                numIteraciones: $('#id-nro').val(),
                idTipo: $('#id-tipo').val(),
                selBarras: $('#id-pop-barra').val(),
                nomVersion: $('#id-nom-version').val(),
                idMetodo: $('#id-metodo').val(),
                idFuente: idFuente,
                nroDiaAporte: $('#id-dias-aporte').val(),
                valNegativo: valNegativo,
            }),
            datatype: 'json',
            traditional: true,
            success: function (result) {
                pop.close();
                SetMessage('#message', result.dataMsg, result.typeMsg);
                if (result.typeMsg != 'error') {
                    LoadData();
                    LoadVersion();
                    LoadVersionReporte();
                }
            },
            error: function () {
                alert("Ha ocurrido un problema...");
            }
        });
    });

    $('#btn-pop-exportar').on('click', function () {
        $.ajax({
            type: 'POST',
            url: controller + 'PronosticoPorBarrasExportar',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({
                fechaInicio: $('#id-fecha-inicio').val(),
                fechaFin: $('#id-fecha-fin').val(),
                idVersion: $('#id-exportar-version').val()
            }),
            datatype: 'json',
            traditional: true,
            success: function (result) {
                $("#popupExport").bPopup().close();
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
    });

    //Aplica la lib. Zebra_Datepicker al calendario del popup duplicar
    $('#id-duplicar-fecha').Zebra_DatePicker();

    $('#btn-duplicar').on('click', function () {
        pop = $('#popupDuplicar').bPopup({
            easing: 'easeOutBack',
            speed: 350,
            transition: 'fadeIn',
            modalClose: false,
            onOpen: function () {
                SetMessage('#pop-duplicar-alert',
                    'Ingrese un nombre para la nueva versión',
                    'info', false);
            },
            onClose: function () {
                $('#id-duplicar-nombre').val('');
            }
        });
    });

    $('#btn-duplicar-guardar').on('click', function () {
        ValidarDuplicarVersion();
    });

    LoadData();
});

//Duplica los pronósticos de la versión seleccionada
function ValidarDuplicarVersion() {
    const nomVersion = $('#id-duplicar-nombre').val();    
    const nomVersiones = $('#id-version option')
        .toArray()
        .map(x => x.innerText);
    if (!nomVersion) {
        SetMessage('#pop-duplicar-alert',
            'El campo de "Versión" es obligatorio',
            'error', false);
        return false;
    }

    if (nomVersiones.includes(nomVersion)) {
        $('#btn-duplicar-guardar').toggle();

        SetMessage('#pop-duplicar-alert',
            'La versión ingresada ya existe, ¿Desea reemplazarla? ' +
            '<button id="btn-duplicar-confirmar" style="margin-right:5px;" onclick="reemplazarConfirmar()">Si</button>' +
            '<button id="btn-duplicar-cancelar" onclick="reemplazarCancelar()">No</button>',
            'warning', false);
        return false;
    }
    DuplicarVersion(false);
}

function DuplicarVersion(actualizar) {
    $.ajax({
        type: 'POST',
        url: controller + 'PronosticoPorBarrasDuplicarVersion',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            refVersion: $('#id-version').val(),
            refFecha: $('#id-fecha').val(),
            nomVersion: $('#id-duplicar-nombre').val(),
            regFecha: $('#id-duplicar-fecha').val(),
            flgActualizar: actualizar
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {            
            LoadVersion();
            SetMessage('#message', result.dataMsg, result.typeMsg);
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        },
        complete: function () {
            pop.close();
        }
    });
}

function reemplazarConfirmar() {
    DuplicarVersion(true);
}

function reemplazarCancelar() {
    $('#btn-duplicar-guardar').toggle();
    SetMessage('#pop-duplicar-alert',
        'Ingrese un nombre para la nueva versión',
        'info', false);
}

//Assetec 20220224
//Carga los datos del modulo
function LoadData() {
    const idBarra = $('#id-barra').val();
    const idAgrupacion = $('#id-agrupacion').val();
    const idVersion = $('#id-version').val();

    $.ajax({
        type: 'POST',
        url: controller + 'PronosticoPorBarrasDatos',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            idBarra: (idBarra) ? idBarra : -1,
            regFecha: $('#id-fecha').val(),
            idVersion: (idVersion) ? idVersion : -1,
            idAgrupacion: (idAgrupacion) ? idAgrupacion : -1,
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            //Flag
            isValid = result.isValid;
            //Recarga los componentes
            Reload();
            //Crea el handsontable
            var ht_model = FormatHandson(result.data);
            GetHanson(ht_model);
            //Crea el highchart
            var hc_title = $("#id-barra option:selected").html();
            var hc_model = FormatHighchart(result.data);
            GetHighchart(hc_model, hc_title);

            SetMessage('#message', result.dataMsg, result.typeMsg);

            //Crea los calendarios            
            GetDataCalendar();
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function LoadVersion() {
    $.ajax({
        type: 'POST',
        url: controller + 'PronosticoPorBarrasActualizarVersion',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            fechaIni: $('#id-fecha').val(),
            fechaFin: $('#id-fecha').val(),
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            RefillDropDowList($('#id-version'),
                result,
                'Vergrpcodi',
                'Vergrpnomb');
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function LoadVersionReporte() {
    $.ajax({
        type: 'POST',
        url: controller + 'PronosticoPorBarrasActualizarVersion',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            fechaIni: $('#id-fecha-inicio').val(),
            fechaFin: $('#id-fecha-fin').val(),
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {            
            RefillDropDowList($('#id-exportar-version'),
                result,
                'Vergrpcodi',
                'Vergrpnomb');
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

//Envia la información modificada para su registro
function Save(dataAjuste, dataBase) {
    const idBarra = $('#id-barra').val();
    const idAgrupacion = $('#id-agrupacion').val();

    $.ajax({
        type: 'POST',
        url: controller + 'PronosticoPorBarrasSave',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            idBarra: (idBarra) ? idBarra : -1,
            idAgrupacion: (idAgrupacion) ? idAgrupacion : -1,
            regFecha: $('#id-fecha').val(),
            idVersion: $('#id-version').val(),
            dataMedicion: dataAjuste,
            dataBase: dataBase
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            SetMessage('#message', result.dataMsg, result.typeMsg, true);
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
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
                col['readOnly'] = true;
                break;
            case 'patron':
                col['type'] = 'numeric';
                col['format'] = '0.00';
                col['readOnly'] = true;
                col['renderer'] = PatronColumnRenderer;
                break;
            case 'edit':
                col['type'] = 'numeric';
                col['format'] = '0.00';
                col['readOnly'] = false;
                col['allowInvalid'] = false;
                col['allowEmpty'] = false;
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

//Crea los calendarios correspondientes a las mediciones historicas
//que conforman el perfil patrón
function GetCalendars(model) {
    //valida la carga inicial
    //if (id == -1) return false;

    //Limpia los remanentes del Zebra_DatePicker
    var zd_m = $('.med-m');
    $.each(zd_m, function (i, item) {
        var x = $(item).data('Zebra_DatePicker');
        x.destroy();
    });

    var zd_t = $('.med-t');
    $.each(zd_t, function (i, item) {
        var x = $(item).data('Zebra_DatePicker');
        x.destroy();
    });

    //Limpia el contenedor
    $('#zd').html('');

    $.each(model, function (i, item) {
        if (item.id.includes('med')) {
            //Contenedor del grupo
            var ctn = $('<div class="simple-box"><div>');
            ctn.css('margin-bottom', '10px');
            ctn.css('padding', '5px 0px 5px 9px');
            ctn.css('border-radius', '4px');

            //Crea el calendario "intervalo completo" o "parcial mañana"
            var itp_m = $('<input type="text" class="med-m"/>');
            itp_m.data('id', item.id);//Id correspondiente a la medición

            itp_m.css('width', '100px');
            itp_m.css('margin-bottom', '5px');
            itp_m.val(item.label);
            ctn.append(itp_m);

            //Crea el calendario "parcial tarde"
            var itp_t = $('<input type="text" class="med-t"/>');
            itp_t.data('id', item.id);//Id correspondiente a la medición

            itp_t.css('width', '100px');
            if (!item.slunes) itp_t.css('display', 'none');
            itp_t.val(item.label2);
            ctn.append(itp_t);

            $('#zd').append(ctn);
        }
    });

    //Agrega Zebra_Datepicker a los calendarios y sus eventos
    $('.med-m').Zebra_DatePicker({
        onSelect: function () {
            var s_lunes = false;
            var s_dia = GetDia(this.val());
            var elem_t = this.parents('.simple-box').find('.med-t');

            //Valida si se debe o no mostrar el calendario "parcial tarde"
            if (s_dia == 0) {
                s_lunes = true;
                var s_date = $('#id-fecha').val();
                if (GetDia(s_date) != 0) elem_t.val(this.val());
                elem_t.css('display', 'inline-block');
                elem_t.parent().css('display', 'inline-block');
            }
            else {
                elem_t.css('display', 'none');
                elem_t.parent().css('display', 'none');
            }

            //Actualiza el perfil patrón
            var meds = GetMediciones(this.data('id'));
            UpdatePatron(this.data('id'), this.val(), elem_t.val(), s_lunes, meds);
        }
    });

    $('.med-t').Zebra_DatePicker({
        onSelect: function () {
            var elem_m = this.parents('.simple-box').find('.med-m');
            var meds = GetMediciones(this.data('id'));
            UpdatePatron(this.data('id'), elem_m.val(), this.val(), true, meds);
        }
    });
}

//Crea el Handsontable
function GetHanson(model) {
    var container = document.getElementById('ht');
    ht = new Handsontable(container, {
        data: model.data,
        fillHandle: true,
        stretchH: 'all',
        maxCols: model.maxCols,
        maxRows: model.maxRows,
        minSpareCols: 0,
        minSpareRows: 0,
        columns: model.columns,
        colHeaders: model.colHeaders,
        afterChange: function (changes, source) {
            if (ValidarEventosHandsontable(source)) {
                htBaseIndex = ht.propToCol('base');//Setea la columna base (Handsontable)
                htDestinoIndex = ht.propToCol('final');//Setea la columna final (Handsontable)

                hcSerieIndex = hc.get('final').index;//Setea el indice de la linea que se modificara (Highchart)

                for (var i = 0; i < changes.length; i++) {
                    htRowIndex = changes[i][0]; htRowData = ht.getDataAtRow(htRowIndex);

                    //Agrega el ajuste realizado
                    htSuma = parseFloat(changes[i][3]);

                    //Suma el valor de los bloques
                    if (arrayBloques[htRowIndex] != null) {
                        htSuma += parseFloat(arrayBloques[htRowIndex]);
                    }

                    //Suma el valor de "Entrada"
                    if (htRowData[htBaseIndex] != null) {
                        htSuma += parseFloat(htRowData[htBaseIndex]);
                    }

                    //Realiza la modificación
                    ht.setDataAtCell(htRowIndex, htDestinoIndex, htSuma, 'sum');

                    //El evento "grafico" modifica la grafica previamente
                    if (source != 'grafico') {
                        hc.series[hcSerieIndex].data[htRowIndex].update(htSuma);
                    }
                }
            }
        },
        afterInit: function () {
            isActive = true;
        }
    });
}

//Formatea el modelo de datos para el Highchart
function FormatHighchart(model) {
    var highchart = {};
    var categories;
    var series = [];
    var rule = ['categoria', 'no'];
    $.each(model, function (i, item) {
        var serie = {};
        switch (item.hcrender) {
            case 'categoria':
                categories = item.data;
                break;
            case 'normal':
                serie['id'] = item.id;
                serie['name'] = item.label;
                serie['data'] = item.data;
                serie['marker'] = { enabled: true, symbol: 'circle' };
                break;
            case 'patron':
                serie['id'] = item.id;
                serie['name'] = item.label;
                serie['data'] = item.data;
                serie['lineWidth'] = 6;
                serie['marker'] = { enabled: true, symbol: 'circle' };
                break;
            case 'margen':
                serie['id'] = item.id;
                serie['name'] = item.label;
                serie['data'] = item.data;
                serie['color'] = '#000000';
                serie['dashStyle'] = 'dot';
                serie['marker'] = { enabled: false, symbol: 'circle' };
                break;
            case 'final':
                serie['id'] = item.id;
                serie['name'] = item.label;
                serie['data'] = item.data;
                serie['lineWidth'] = 6;
                serie['marker'] = { enabled: true, symbol: 'circle' };
                serie['draggableY'] = true;
                break;
            case 'final_noedit':
                serie['id'] = item.id;
                serie['name'] = item.label;
                serie['data'] = item.data;
                serie['lineWidth'] = 6;
                serie['marker'] = { enabled: true, symbol: 'circle' };
                break;
        }

        if (!(rule.includes(item.hcrender))) series.push(serie);
    });

    highchart['categories'] = categories;
    highchart['series'] = series;
    return highchart;
}

//Crea el Highchart
function GetHighchart(model, title) {
    hc = Highcharts.chart('hc', {
        chart: {
            type: 'spline',
            backgroundColor: 'transparent',
            plotShadow: false,
            zoomType: 'xy',
        },
        title: {
            text: title
        },
        credits: {
            enabled: false
        },
        line: {
            cursor: 'ns-resize'
        },
        legend: {
            itemDistance: 15,
            verticalAlign: 'top',
            symbolWidth: 10,
            itemStyle: {
                fontWeight: 'normal'
            },
            y: 0
        },
        tooltip: { enabled: true },
        plotOptions: {
            series: {
                stickyTracking: false,
                marker: { enabled: true, radius: 3 },
                point: {
                    events: {
                        drop: function () {//Actualiza la columna Ajuste del Handsontable                            
                            var a = ht.getDataAtRowProp(this.x, 'ajuste');
                            var hc_ajuste = Highcharts.numberFormat(this.y - this.dragStart.y + a, 2);
                            ht.setDataAtRowProp(this.x, 'ajuste', hc_ajuste, 'grafico');
                        }
                    }
                }
            }
        },
        xAxis: {
            tickInterval: 1,
            categories: model.categories,
            labels: { rotation: 90 }
        },
        yAxis: {
            title: ''
        },
        tooltip: {
            crosshairs: true,
            shared: true
        },
        series: model.series
    });
}

//Convierte una fecha al formato YYYY-MM-DD
function GetDia(f) {
    var f_dia;

    var f_array = f.split('/');
    var f_convert = f_array[2] + '-' + f_array[1] + '-' + f_array[0];

    var f_date = new Date(Date.parse(f_convert));
    f_dia = f_date.getDay();

    return f_dia;
}

//Obtiene las mediciones historicas que forman el perfil patrón
function GetMediciones(s_med) {
    if (!hc) return false;

    var res = [];
    var meds = hc.userOptions.series;
    $.each(meds, function (i, item) {
        if (item.id.includes('med')) {
            if (item.id != s_med) res.push(item.data);
        }
    });

    return res;
}

//Permite ajustar los intervalos definidos en los rangos bloques
function EditarPorBloque(id, valor) {
    var limIni, limFin;
    var blqSuma, blqFila;
    var blqCambios = [];
    var htAjusteIndex = ht.propToCol('ajuste');
    var factor = 1;

    switch (id) {
        case 'base':
            limIni = 0 * factor;
            limFin = 15 * factor;
            break;
        case 'media':
            limIni = 15 * factor;
            limFin = 35 * factor;
            break;
        case 'punta':
            limIni = 35 * factor;
            limFin = 48 * factor;
            break;
    }

    for (var i = limIni; i < limFin; i++) {
        arrayBloques[i] = valor;
        dataAjuste = ht.getDataAtRowProp(i, 'ajuste');
        blqFila = [];
        blqFila = [i, 'ajuste', dataAjuste];
        blqCambios.push(blqFila);
    }
    ht.setDataAtRowProp(blqCambios);
}

//Valida los eventos permitidos en el Handsontable
function ValidarEventosHandsontable(evento) {
    var valid = false;
    var ListEventos = ['edit', 'autofill', 'paste', 'grafico'];

    for (var i = 0; i < ListEventos.length; i++) {
        if (evento == ListEventos[i]) valid = true;
    }
    return valid;
}

//Desactiva el flag "isActive" del handsontable
//Limpia el contenedor "ht"
//Limpia los bloques
//muestra u oculta el panel "workspace"
function Reload() {
    $('#ht').html('');
    arrayBloques = [];
    $('.cls-bloque').val('');
}

function HtGetDataAsProp() {
    if (!isActive) return false;

    var res = [];
    var s_max = ht.countRows();
    var s_schema = ht.getSchema();
    for (var i = 0; i < s_max; i++) {
        var s_row = {};
        $.each(s_schema, function (prop, value) {
            s_row[prop] = ht.getDataAtRowProp(i, prop);
        });
        res.push(s_row);
    }
    return res;
}

//Formatea los datos de un arreglo a un modelo
//tipo Medición por intervalos(H1, H2, ...)
function FormatMedicion(data) {
    var res = {};
    $.each(data, function (i, item) {
        var s = i + 1;
        var prop = 'H' + s;
        res[prop] = item;
    });

    //Agrega los valores de los bloques al ajuste
    $.each(arrayBloques, function (i, item) {
        var s = i + 1;
        var prop = 'H' + s;
        res[prop] += item;
    });

    return res;
}

function GetDataSelect(type) {
    data = [];
    if (type == 'D') {
        for (var i = 1; i < 7; i++) {
            obj = {};
            obj.text = i;
            obj.value = i;
            data.push(obj);
        }
    }
    if (type == 'S') {
        for (var i = 1; i < 4; i++) {
            obj = {};
            obj.text = i;
            obj.value = i;
            data.push(obj);
        }
    }

    return data;
}

//Estilos de las columnas del Handsontable
function HoraColumnRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.fontWeight = 'bold';
    td.style.background = '#F2F2F2';
}

function PatronColumnRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.NumericRenderer.apply(this, arguments);
    td.style.background = '#edf5fc';
}
//**

//Assetec 20220224
function actualizarMedicion(medicion) {
    const idBarra = $('#id-barra').val();
    const idAgrupacion = $('#id-agrupacion').val();
    const idGrafica = $('input[name=id-tipoFuente]:checked').val();

    $.ajax({
        type: 'POST',
        url: controller + 'ActualizarMedicionPronosticoBarra',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            idBarra: (idBarra) ? idBarra : -1,
            idVersion: $('#id-version').val(),
            idAgrupacion: (idAgrupacion) ? idAgrupacion : -1,
            fecha: medicion.fecha,
            grafica: idGrafica
        }),
        datatype: 'json',
        traditional: true,
        success: function (modelo) {
            const testHt = modelo.dataht;
            const testHc = modelo.datahc;

            hc.get(medicion.id).
                setData(testHc);
            hc.get(medicion.id)
                .update({ name: medicion.fecha });

            const htData = HtGetDataAsProp();
            htData.forEach((e, index) => {
                e[medicion.id] = testHt[index];
            });

            let htHeader = ht.getColHeader();
            const colIndex = ht.propToCol(medicion.id);
            htHeader[colIndex] = medicion.fecha + "(H)";
            ht.updateSettings({
                data: htData,
                colHeaders: htHeader
            });
        },
        error: function () {
            alert('Ha ocurrido un error...');
        }
    });
}

function GetDataCalendar() {
    $('#fh div').each(function () {
        var val = $(this).find('input').val();
        var id = $(this).find('input').attr('id');

        if (val) {
            const m = {
                id: id,
                fecha: val
            };
            actualizarMedicion(m);
        }
    })
}