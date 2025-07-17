var isActive = false;
var arrayBloques = [];
var tempData = {};
var dataParametros;
var pop_tempData = {};

// Variables globales para captura de datos de la bitacora
var idEmpresa;
var ptoMedicion;
var area;
var tipoDemanda;
var tipoEmpresa;

$(document).ready(function () {
    //Aplica la lib. Zebra_Datepicker
    $('.f-fecha').Zebra_DatePicker({
        onSelect: function () {
            dt.ajax.reload();
            QuickLoadData();
        }
    });

    $('.f-reporte-fecha').Zebra_DatePicker({
        onSelect: function () {//Evento onChange
            var a = $('#id-fecha-inicio').val();
            var b = $('#id-fecha-fin').val();
            if (ValidateDates(a, b) == false) {
                b = AddDays(a);
                $('#id-fecha-fin').val(b);
            }
        }
    });

    //Aplica la lib. MultiSelect
    $('.f-select-s').each(function () {
        var element = this;
        $(element).multipleSelect({
            name: element.id,
            filter: true,
            single: true,
            onClose: function () {
                dt.ajax.reload();
                QuickLoadData();
            }
        });
    });

    $('.f-select-m').each(function () {
        var element = this;
        $(element).multipleSelect({
            name: element.id,
            filter: true,
            single: false,
            placeholder: 'Seleccione',
            onClose: function () {
                var e = document.getElementById(this.name);
                UpdateFilters(e.id);
                dt.ajax.reload();
            },
            onCheckAll: function () {
                var e = document.getElementById(this.name);
                $(e).val(null);
            }
        });
    });
    
    $('.f-select-l').each(function () {
        var element = this;
        $(element).multipleSelect({
            name: element.id,
            filter: false,
            single: false,
            placeholder: 'Seleccione',
            onClose: function () {
                dt.ajax.reload();
            },
            onCheckAll: function () {
                var e = document.getElementById(this.name);
                $(e).val(null);
            }
        });
    });

    $('#id-pronostico').on('change', function () {
        dt.ajax.reload();
    });
    
    $('#id-orden').multipleSelect({
        filter: false,
        single: true,
        onClose: function () {
            dt.ajax.reload();
        }
    });

    $('#id-pop-tpempresa').multipleSelect({
        filter: false,
        single: true
    });

    $('#id-tpempresa').multipleSelect({
        filter: true,
        single: true,
        onClose: function () {
            UpdateFilters('id-tpempresa');
            dt.ajax.reload();
        }
    });

    $('.cls-bloque').on('change', function () {
        var id = this.id;
        var valor = parseFloat(this.value);
        if (!(isNaN(valor))) {
            EditarPorBloque(id, valor);
        }
    });

    dt = $('#dt').DataTable({
        serverSide: true,
        ajax: {
            type: "POST",
            url: controller + 'List',
            contentType: 'application/json; charset=utf-8',
            data: function (d) {
                d.idModulo = $('#main').data('mdl');
                d.dataFiltros = GetFiltersValues();
                d.regFecha = $('#id-fecha').val();
                return JSON.stringify(d);
            },
            datatype: 'json',
            traditional: true
        },
        columns: [
            { title: 'Prioridad', data: 'Prnclsclasificacion' },
            { title: 'Perfil', data: 'Prnclsperfil' },
            { title: 'Estado de depuración', data: 'Prnmestado' },
            { title: 'Justificaciones', data: 'Subcausadesc' },
            { title: 'Energía(MW)', data: 'Meditotal' },
            { title: 'Δ Energía(MW)', data: 'Prnclsvariacion' },
            { title: 'Punto de medición', data: 'Ptomedicodi' },
            { title: 'Ubicación', data: 'Areacodi' },
            { title: 'Empresa', data: 'Emprnomb' },
            { title: 'Equipo', data: 'Equinomb' },
            { title: '', data: null }
        ],
        columnDefs: [
            {
                //Color de columna "Prioridad" segun criterio
                targets: [0],
                createdCell: function (td, cellData, rowData, row, col) {
                    switch (rowData.Prnclsclasificacion) {
                        case 1:
                            $(td).html('Muy alta');
                            $(td).css('background', '#FF9595');
                            $(td).css('font-weight', 'bold');
                            break;
                        case 2:
                            $(td).html('Alta');
                            $(td).css('background', '#FFC895');
                            $(td).css('font-weight', 'bold');
                            break;
                        case 3:
                            $(td).html('Media');
                            $(td).css('background', '#FFED95');
                            $(td).css('font-weight', 'bold');
                            break;
                        case 4:
                            $(td).html('Baja');
                            $(td).css('background', '#95D7FF');
                            $(td).css('font-weight', 'bold');
                            break;
                        default:
                            $(td).html('--');
                            $(td).css('font-weight', 'bold');
                            break;
                    }
                }
            },
            {
                //Contenido de la columna "Perfil" segun criterio
                targets: [1],
                createdCell: function (td, cellData, rowData, row, col) {
                    switch (rowData.Prnclsperfil) {
                        case -1:
                            $(td).html('No Procesado');
                            $(td).css('font-weight', 'bold');
                            $(td).css('color', '#FF3434');
                            break;
                        case 1:
                            $(td).html('Normal');
                            $(td).css('font-weight', 'bold');
                            break;
                        case 2:
                            $(td).html('Baja de carga');
                            $(td).css('font-weight', 'bold');
                            break;
                        case 3:
                            $(td).html('Subida o baja de carga puntual');
                            $(td).css('font-weight', 'bold');
                            break;
                        case 4:
                            $(td).html('Congelados y fuera del patrón');
                            $(td).css('font-weight', 'bold');
                            break;
                        default:
                            $(td).html('--');
                            $(td).css('font-weight', 'bold');
                            break;
                    }
                }
            },
            {
                //Contenido de la columna "Estado" segun criterio
                targets: [2],
                createdCell: function (td, cellData, rowData, row, col) {
                    switch (rowData.Prnmestado) {
                        case 1:
                            $(td).html('No depurado');
                            $(td).css('font-weight', 'bold');
                            break;
                        case 2:
                            $(td).html('Procesado');
                            $(td).css('font-weight', 'bold');
                            break;
                        case 3:
                            $(td).html('Manual');
                            $(td).css('font-weight', 'bold');
                            break;
                    }
                }
            },
            {
                //Descripciones compuestas y modificadas
                targets: '_all',
                createdCell: function (td, cellData, rowData, row, col) {
                    var str;
                    switch (col) {
                        case 3:
                            str = '';
                            counts = {};
                            ar_data = rowData.Subcausadesc.split(',');
                            ar_data.forEach(function (x) { counts[x] = (counts[x] || 0) + 1; });

                            //Print
                            $.each(counts, function (key, value) {
                                str += '<p>' + key + '(' + value + ')' + '</p>';
                            });
                            $(td).html(str);
                            break;
                        case 4:
                            console.log(rowData, 'data_1');
                            cal = rowData.Meditotal / 2;
                            $(td).html(cal);
                            break;
                        case 5:
                            console.log(rowData, 'data_2');
                            cal = rowData.Prnclsvariacion / 2;
                            $(td).html(cal);
                            break;
                        case 6:
                            //Punto de medición
                            str = '[' + rowData.Famnomb + '] ' + rowData.Equinomb + ' (' + rowData.Ptomedicodi + ')';
                            $(td).html(str);
                            break;
                        case 7:
                            //Ubicación
                            str = rowData.Tareaabrev + ' ' + rowData.Areanomb;
                            $(td).html(str);
                            break;
                    }
                }
            },
            {
                //Botones
                targets: -1,
                defaultContent: '<img src="' + imageRoot + 'btn-open.png" class="tb-btn-ver" title="Ver detalle"/>' +
                    '<img src="' + imageRoot + 'btn-ok.png" class="tb-btn-ajustes" title="Tomar valores reportados"/>'
            }
        ],
        initComplete: function () {
            $('#dt').css('width', '100%');
        },
        drawCallback: function () {
            $('#dt').css('width', '100%');
        },
        searching: false,
        bLengthChange: false,
        bSort: false,
        destroy: true,
        paging: true,
        pageLength: 10,
        info: false,
    });

    $('#btn-guardar').on('click', function () {
        //Obtiene datos necesarios para las validaciones
        var id = tempData.id;

        //Valida que el Handsontable este activo
        if (!isActive) {
            SetMessage('#message',
                'Debe seleccionar un punto de medición!',
                'warning', true);
            return false;
        }

        //Valida que se haya seleccionado un punto de medición
        if (!id) {
            SetMessage('#message',
                'Debe seleccionar un punto de medición!',
                'warning', true);
            return false;
        }

        //Obtiene datos para el registro
        var date = $('#id-fecha').val();
        var type = $('#id-tpdemanda').val();
        var data = FormatMedicion(ht.getDataAtProp('ajuste'));
        
        //Función para el registro
        Save(id, type, date, data);
    });

    $('#btn-reporte').on('click', function () {
        pop_reporte = $('#pop-reporte').bPopup({
            easing: 'easeOutBack',
            speed: 350,
            transition: 'fadeIn',
            modalClose: false,
            onOpen: function () {
                SetMessage('#pop-reporte-mensaje',
                    '• Seleccione el rango de fechas del pronóstico antes de "Exportar".<br>' +
                    '• Se iniciará el pronóstico a partir de la "Fecha de inicio", esta si sera incluida.<br>' +
                    '• El reporte se ejecutará para todos los PM que participan en pronóstico por barras.<br>' +
                    '• Dependiendo del nro. de dias o semanas el proceso puede tardar varios minutos.',
                    'info');
            }
        });
    });

    $('#btn-pop-reporte-exportar').on('click', function () {
        $.ajax({
            type: 'POST',
            url: controller + 'ExportarReporte',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({
                fecIni: $('#id-fecha-inicio').val(),
                fecFin: $('#id-fecha-fin').val(),
                idTipoEmpresa: $('#id-pop-tpempresa').val()
            }),
            datatype: 'json',
            traditional: true,
            success: function (result) {
                pop_reporte.close();
                if (result != "-1") {
                    window.location = controller + 'abrirarchivo?formato=' + 1 + '&file=' + result;
                    SetMessage('#message', 'Archivo generado exitosamente', 'success', true);
                }
                else {
                    SetMessage('#message', 'Ha ocurrido un problema con la exportación', 'error', true);
                }
            },
            error: function () {
                alert("Ha ocurrido un problema...");
            }
        });
    });

    $('.pop-alert-btn').on('click', function () {
        if (this.value == 'Si') {
            $.ajax({
                type: 'POST',
                url: controller + 'EliminarAjustes',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({
                    idPunto: pop_tempData.id_punto,
                    idLectura: pop_tempData.id_lectura,
                    regFecha: pop_tempData.fecha
                }),
                datatype: 'json',
                traditional: true,
                success: function (result) {
                    if (result.valid) {
                        SetMessage('#message', result.dataMsg, result.typeMsg, true);
                        if (isActive) {
                            LoadData(pop_tempData.id_punto,
                                pop_tempData.id_lectura,
                                pop_tempData.id_tpempresa,
                                pop_tempData.fecha,
                                pop_tempData.title);
                        }
                    }
                    else {
                        SetMessage('#message', result.dataMsg, result.typeMsg, true);
                    }
                },
                error: function () {
                    alert("Ha ocurrido un problema...");
                }
            });
            
            pop.close();
        }
        else {
            pop.close();
        }        
    });
    
    $('#btn-bitacora3').on('click', function () {
        const data = obtenerCeldasSeleccionadas();

        // Validamos si se ha seleccionado datos en la grilla
        if (!data) {
            alert("No ha seleccionado datos en la cuadricula.");
            return;
        }

        // Obtiene datos para la bitacora (Estas variables deberian declararse con let y no con var para evitar hoisting)
        var emprcodi = idEmpresa;
        var medifecha = $('#id-fecha').val();
        var arrIntervaloHras = data.selIntervalos;
        var arrConstipico = data.selDataPatron;//selDataFinal
        var arrConsprevisto = data.selDataFinal;
        var ptomedicodi = ptoMedicion;

        var lectcodi = $('#id-tpdemanda').val();
        var tipoemprcodi = $('#id-tpempresa').val();
        //var lectcodi = tipoDemanda; 
        //var tipoemprcodi = tipoEmpresa;

        var valor = area;

        // Función para el registro
        SaveBitacora(emprcodi,
            medifecha,
            arrIntervaloHras,
            arrConstipico,
            arrConsprevisto,
            ptomedicodi,
            lectcodi,
            tipoemprcodi,
            valor);
    });
});

$(document).on('click', '#dt tr td .tb-btn-ver', function () {
    var row = $(this).closest('tr');
    var r = dt.row(row).data();

    var s_id = r.Ptomedicodi;
    var s_date = $('#id-fecha').val();
    var s_type_dem = $('#id-tpdemanda').val();
    var s_type_emp = $('#id-tpempresa').val();
    var title = '[' + r.Famnomb + '] ' + r.Equinomb + ' (' + r.Ptomedicodi + ')';

    //Temporal selected data
    tempData['id'] = r.Ptomedicodi;
    tempData['title'] = title;

    // Capturamos los datos de la deputacion para la bitacora
    idEmpresa = r.Emprcodi;
    ptoMedicion = r.Ptomedicodi;
    area = r.Areanomb;

    tipoDemanda = r.lectcodi // Esta lanzando nulo
    tipoEmpresa = r.tipoemprcodi // Esta lanzando nulo

    //Carga los datos
    LoadData(s_id, s_type_dem, s_type_emp, s_date, title);
});

$(document).on('click', '#dt tr td .tb-btn-ajustes', function () {
    var row = $(this).closest('tr');
    var r = dt.row(row).data();

    pop_tempData = {};
    pop_tempData['id_punto'] = r.Ptomedicodi;
    pop_tempData['id_lectura'] = $('#id-tpdemanda').val();
    pop_tempData['id_tpempresa'] = $('#id-tpempresa').val();
    pop_tempData['fecha'] = $('#id-fecha').val();
    pop_tempData["title"] = '[' + r.Famnomb + '] ' + r.Equinomb + ' (' + r.Ptomedicodi + ')';
    
    pop = $('#pop-alert').bPopup({
        easing: 'easeOutBack',
        speed: 350,
        transition: 'fadeIn',
        modalClose: false
    });
});

//Carga los datos del modulo
function LoadData(s_id, s_type_dem, s_type_emp, s_date, title) {
    $.ajax({
        type: 'POST',
        url: controller + 'Datos',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            idPunto: s_id,
            idTipoDemanda: s_type_dem,
            idTipoEmpresa: s_type_emp,
            idModulo: $('#main').data('mdl'),
            regFecha: s_date
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (Reload(result.valid)) {
                //Setea los parámetros de configuración
                dataParametros = result.cfg;

                //Crea el handsontable
                var ht_model = FormatHandson(result.data);
                GetHanson(ht_model);

                //Crea el highchart
                var hc_model = FormatHighchart(result.data);
                GetHighchart(hc_model, title);

                //Crea los calendarios
                GetCalendars(s_id, result.data);
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

//Envia la información modificada para su registro
function Save(s_pto, s_type_dem, s_dte, s_dta) {
    $.ajax({
        type: 'POST',
        url: controller + 'Save',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            idPunto: s_pto,
            idTipoDemanda: s_type_dem,
            regFecha: s_dte,
            dataMedicion: s_dta
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

//Actualiza los filtros de busqueda
function UpdateFilters(id) {
    $.ajax({
        type: 'POST',
        url: controller + 'UpdateFiltros',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            idModulo: $('#main').data('mdl'),
            dataFiltros: GetFiltersValues()
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            switch (id) {
                case 'main':

                    break;
                case 'id-ubicacion':
                    //Actualiza la lista de puntos de medición
                    RefillDropDowList($('#id-punto'), result.ListPtomedicion, 'Ptomedicodi', 'Ptomedidesc');
                    break;
                case 'id-tpempresa':
                    //Actualiza la lista de empresas
                    RefillDropDowList($('#id-empresa'), result.ListEmpresa, 'Emprcodi', 'Emprnomb');
                    //Actualiza la lista de puntos de medición
                    RefillDropDowList($('#id-punto'), result.ListPtomedicion, 'Ptomedicodi', 'Ptomedidesc');
                    break;
                case 'id-empresa':
                    //Actualiza la lista de puntos de medición
                    RefillDropDowList($('#id-punto'), result.ListPtomedicion, 'Ptomedicodi', 'Ptomedidesc');
                    break;
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function UpdatePatron(s_id, s_date_a, s_date_b, slunes, meds) {
    var s_mdl = $('#main').data('mdl');
    $.ajax({
        type: 'POST',
        url: controller + 'UpdatePatron',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            idModulo: s_mdl,
            idPunto: tempData.id,
            regFechaA: s_date_a,
            regFechaB: s_date_b,
            esLunes: slunes,
            tipoPatron: dataParametros.Prncfgtipopatron,
            dsvPatron: dataParametros.Prncfgporcdsvptrn,
            dataMediciones: meds
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            //Actualiza las series del hc
            hc.get(s_id).setData(result.medicion);//medición
            hc.get('patron').setData(result.patron);//patrón
            hc.get('rmin').setData(result.emin);//emin
            hc.get('rmax').setData(result.emax);//emax

            //Actualiza la leyenda de la medición en el hc
            hc.get(s_id).update({ name: s_date_a });

            //Actualiza las columnas del ht
            var ht_data = HtGetDataAsProp();
            $.each(ht_data, function (i, item) {
                item['patron'] = result.patron[i];
            });
            
            ht.updateSettings({ data: ht_data });
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
function GetCalendars(id, model) {
    //valida la carga inicial
    if (id == -1) return false;

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

        // modificacion de las propiedades del HansonTable para capturar datos seleccionados
        selectionMode: 'range',
        outsideClickDeselects: false,

        afterChange: function (changes, source) {
            if (ValidarEventosHandsontable(source)) {
                htBaseIndex = ht.propToCol('base');//Setea la columna base (Handsontable)
                htDestinoIndex = ht.propToCol('final');//Setea la columna final (Handsontable)
                htExtraIndex = ht.propToCol('auto');//Setea la columna A.Auto (Handsontable)

                hcSerieIndex = hc.get('final').index;//Setea el indice de la linea que se modificara (Highchart)

                for (var i = 0; i < changes.length; i++) {
                    htRowIndex = changes[i][0]; htRowData = ht.getDataAtRow(htRowIndex);

                    htSuma = parseFloat(changes[i][3]);//Agrega el ajuste realizado

                    if (htRowData[htExtraIndex] != null) {
                        htSuma += parseFloat(htRowData[htExtraIndex]);//Suma el valor de "A.Auto"
                    }

                    if (arrayBloques[htRowIndex] != null) {
                        htSuma += parseFloat(arrayBloques[htRowIndex]);//Suma el valor de los bloques
                    }
                    
                    if (htRowData[htBaseIndex] != null) {
                        htSuma += parseFloat(htRowData[htBaseIndex]);//Suma el valor de "Entrada"
                    }

                    ht.setDataAtCell(htRowIndex, htDestinoIndex, htSuma, 'sum');//Realiza la modificación

                    if (source != 'grafico') {//El evento "grafico" modifica la grafica previamente
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
            case 'defecto':
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

//Setea el modelo de envio de información
function GetFiltersValues() {
    var model = {};
    model['SelById'] = $('#id-byid').val();
    model['SelTipoDemanda'] = $('#id-tpdemanda').val();
    model['SelTipoEmpresa'] = $('#id-tpempresa').val();
    model['SelUbicacion'] = $('#id-ubicacion').val();
    model['SelEmpresa'] = $('#id-empresa').val();
    model['SelPuntos'] = $('#id-punto').val();
    model['SelPerfil'] = $('#id-perfil').val();
    model['SelClasificacion'] = $('#id-clasificacion').val();
    model['SelAreaOperativa'] = $('#id-areaoperativa').val();
    model['SelOrden'] = $('#id-orden').val();
    model['SelJustificacion'] = $('#id-justificacion').val();
    model['SelBarra'] = $('#id-pronostico').prop('checked');
    return model;
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
function Reload(valid) {
    isActive = false;
    $('#ht').html('');
    arrayBloques = [];
    $('.cls-bloque').val('');
    if (valid) $('#workspace').css('display', 'flex');
    else $('#workspace').css('display', 'none');
    return valid;
}

//Actualiza los datos del punto seleccionado segun criterios (algunos) si el ht se encuentra activo
function QuickLoadData() {
    //Validaciones
    if (!isActive) {
        return false;
    }
      
    //Carga los datos
    var s_date = $('#id-fecha').val();
    var s_type_dem = $('#id-tpdemanda').val();
    var s_type_emp = $('#id-tpempresa').val();
    LoadData(tempData.id, s_type_dem, s_type_emp, s_date, tempData.title);
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

//**
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

function SaveBitacora(emprcodi,
    medifecha,
    arrIntervaloHras,
    arrConstipico,
    arrConsprevisto,
    ptomedicodi,
    lectcodi,
    tipoemprcodi,
    valor) {

    $.ajax({
        type: 'POST',
        url: controller + 'SaveBitacora',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            Emprcodi: emprcodi,
            Medifecha: medifecha,
            ArrIntervalohras: arrIntervaloHras,
            ArrConstipico: arrConstipico,
            ArrConsprevisto: arrConsprevisto,
            Ptomedicodi: ptomedicodi,
            Lectcodi: lectcodi,
            Tipoemprcodi: tipoemprcodi,
            Valor: valor
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

//Obtener celdas seleccionadas
function obtenerCeldasSeleccionadas() {
    const selRange = ht.getSelected() || [];

    if (selRange.length == 0)
        return false;

    const [rIni, cIni, rFin, cFin] = selRange;

    // Modificado x correcciones 21-04-2022
    const inxFinal = ht.propToCol('final');

    if (!([cIni, cFin].includes(inxFinal))) // logica dinamica
        return false;

    const colIntervalos = ht.getDataAtProp('intervalos'); // la otra columna es 'intervalos'
    const selIntervalos = colIntervalos.slice(rIni, (rFin + 1));

    const colDataFinal = ht.getDataAtProp('final'); // la otra columna es 'patron'
    const selDataFinal = colDataFinal.slice(rIni, (rFin + 1));

    const colDataPatron = ht.getDataAtProp('patron'); // la otra columna es 'patron'
    const selDataPatron = colDataPatron.slice(rIni, (rFin + 1));

    return {
        selIntervalos,
        selDataFinal,
        selDataPatron
    };
}