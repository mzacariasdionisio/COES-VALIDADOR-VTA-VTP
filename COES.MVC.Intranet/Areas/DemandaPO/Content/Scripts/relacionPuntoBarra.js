var name = "RelacionPuntoBarra/";
var controller = siteRoot + "DemandaPO/" + name;
var imageRoot = siteRoot + "Content/Images/";
var dt;

$(document).ready(function () {

    $('#btnConsultar').click(function () {
        consultarData();
    });

    $('#btnExportar').click(function () {
        exportarData();
    });

    $('#selVersion').multipleSelect({
        single: true,
        filter: true,
        placeholder: 'Seleccione...',
    });

    $('#selBarra').multipleSelect({
        single: true,
        filter: true,
        placeholder: 'Seleccione...',
    });

    $('#selVersion').change(function () {
        const version = $('#selVersion').val();
        recargaFiltroBarra(version);
        //listaBarrasVersion()
    });
    //listarBarrasxVersion();
});


function recargaFiltroBarra(version) {
    $.ajax({
        type: 'POST',
        url: controller + 'recargaBarra',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            version: version
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            RefillDropDowList($('#selBarra'), result.ListaBarrasSPL, 'Barsplcodi', 'Gruponomb');

            dt = $('#dt').DataTable({
                data: result.ListaBarrasGrilla,//datos,
                columns: [
                    { title: 'Id relacion punto - barra', data: 'Ptobarcodi', visible: false },
                    { title: 'Id relacion barra - formula', data: 'Splfrmcodi', visible: false },
                    { title: 'Version', data: 'Dposplnombre' },
                    { title: 'Barra SPL', data: 'Gruponomb' },
                    { title: 'Fórmula Vegetativa', data: 'Nombvegetativa' },
                    { title: 'Fórmula Industrial', data: 'Nombindustrial' },
                    { title: 'Punto TNA', data: null },
                    { title: '', data: null },
                ],
                columnDefs: [
                    {
                        targets: 6,
                        defaultContent: '<select class="dt-select sel-p-tna"></select>',
                    },
                    {
                        targets: -1,
                        defaultContent:
                            '<div class="dt-col-options">' +
                            `<img src="${imageRoot}prnsave.png" class="dt-ico-editar" title="Guardar registro" />` +
                            `<img src="${imageRoot}btn-cancel.png" class="dt-ico-eliminar" title="Eliminar registro" />` +
                            '</div>',
                    },
                ],
                createdRow: function (row, data, index) {
                    //_dataPuntoTna
                    const _objTna = $(row).find('.sel-p-tna');

                    //Obtiene los datos de la vista
                    const _tna = $('#_dataPuntoTna option').clone();

                    _objTna.append(_tna);
                    //if (data.VegNoDisponible) {
                    //    const _reVeg = _veg.map((index, item) => {
                    //        if (!data.VegNoDisponible.includes(parseInt(item.value))) return item;
                    //    });
                    //    _objVeg.append(_reVeg);
                    //} else {
                    //    _objVeg.append(_veg);
                    //}

                    //Aplica mascara de multipleSelect
                    _objTna.multipleSelect({
                        single: true,
                        filter: true,
                        placeholder: 'Seleccione un Punto...',
                        onClose: function () {
                            const id = $(_objTna).multipleSelect('getSelects');
                            const nomb = $(_objTna).multipleSelect('getSelects', 'text');
                            data.Ptomedidesc = nomb[0];
                            data.Ptomedicodi = id[0];
                        },
                    });

                    //Clic en editar
                    $(row).find('.dt-ico-editar').on('click', function () {
                        var row = $(this).closest('tr');
                        var r = dt.row(row).data();
                        let pb = (r.Ptobarcodi) ? r.Ptobarcodi : -1
                        actualizarFila(r.Splfrmcodi, r.Ptomedicodi, pb);
                    });

                    //Clic en eliminar
                    $(row).find('.dt-ico-eliminar').on('click', function () {
                        var row = $(this).closest('tr');
                        var r = dt.row(row).data();
                        let pb = (r.Ptobarcodi) ? r.Ptobarcodi : -1
                        eliminarFila(pb);
                    });

                    //Setea el valor seleccionado de existir
                    const _arryTna = [];
                    _arryTna.push(data.Ptomedicodi);
                    if (data.punto_tna != -1) {
                        _objTna.multipleSelect('setSelects', _arryTna);
                    }

                },
                searching: false,
                bLengthChange: false,
                bSort: false,
                destroy: true,
                info: false,
            });
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}


function listaBarrasVersion(version) {
    $.ajax({
        type: 'POST',
        url: controller + "ListaBarrasxVersion",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            idVersion: version
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            dt = $('#dt').DataTable({
                data: result.ListaBarras,//datos,
                columns: [
                    { title: 'Barra SPL', data: 'Gruponomb' },
                    { title: 'Fórmula Vegetativa', data: null },
                    { title: 'Fórmula Industrial', data: null },
                    { title: '', data: null },
                ],
                columnDefs: [
                    {
                        targets: 1,
                        defaultContent: '<select class="dt-select sel-f-veg"></select>',
                    },
                    {
                        targets: 2,
                        defaultContent: '<select class="dt-select sel-f-ind"></select>',
                    },
                    {
                        targets: -1,
                        defaultContent:
                            '<div class="dt-col-options">' +
                            `<img src="${imageRoot}btn-edit.png" class="dt-ico-editar" title="Editar registro" />` +
                            `<img src="${imageRoot}btn-cancel.png" class="dt-ico-eliminar" title="Eliminar registro" />` +
                            '</div>',
                    },
                ],
                createdRow: function (row, data, index) {
                    const _objVeg = $(row).find('.sel-f-veg');
                    const _objInd = $(row).find('.sel-f-ind');

                    //Obtiene los datos de la vista
                    const _veg = $('#_dataFormulaVeg option').clone();
                    const _ind = $('#_dataFormulaInd option').clone();

                    if (data.VegNoDisponible) {
                        const _reVeg = _veg.map((index, item) => {
                            if (!data.VegNoDisponible.includes(parseInt(item.value))) return item;
                        });
                        _objVeg.append(_reVeg);
                    } else {
                        _objVeg.append(_veg);
                    }
                    if (data.UliNoDisponible) {
                        const _reInd = _ind.map((index, item) => {
                            if (!data.UliNoDisponible.includes(parseInt(item.value))) return item;
                        });
                        _objInd.append(_reInd);
                    } else {
                        _objInd.append(_ind);
                    }

                    //Aplica mascara de multipleSelect
                    _objVeg.multipleSelect({
                        single: true,
                        placeholder: 'Seleccione una Formula...',
                        onClose: function () {
                            const id = $(_objVeg).multipleSelect('getSelects');
                            const nomb = $(_objVeg).multipleSelect('getSelects', 'text');
                            data.Nombvegetativa = nomb[0];
                            data.Ptomedicodifveg = id[0];
                        },
                    });
                    _objInd.multipleSelect({
                        single: true,
                        placeholder: 'Seleccione una Formula...',
                        onClose: function () {
                            const id = $(_objInd).multipleSelect('getSelects');
                            const nomb = $(_objInd).multipleSelect('getSelects', 'text');
                            data.Nombindustrial = nomb[0];
                            data.Ptomedicodiful = id[0];
                        },
                    });

                    //Clic en editar
                    $(row).find('.dt-ico-editar').on('click', function () {
                        var row = $(this).closest('tr');
                        var r = dt.row(row).data();
                        let pb = (r.Ptobarcodi) ? r.Ptobarcodi : -1;
                        let pto = (r.Ptomedicodi) ? r.Ptomedicodi : -1;
                        actualizarFila(r.Splfrmcodi, pto, pb);
                    });

                    //Clic en eliminar
                    $(row).find('.dt-ico-eliminar').on('click', function () {
                        var row = $(this).closest('tr');
                        var r = dt.row(row).data();
                        eliminarFila(r);
                    });

                    //Setea el valor seleccionado de existir
                    const _arryVeg = [];
                    _arryVeg.push(data.Ptomedicodifveg);
                    if (data.formula_veg != -1) {
                        _objVeg.multipleSelect('setSelects', _arryVeg);
                    }
                    const _arryInd = [];
                    _arryInd.push(data.Ptomedicodiful);
                    if (data.formula_ind != -1) {
                        _objInd.multipleSelect('setSelects', _arryInd);
                    }
                },
                searching: false,
                bLengthChange: false,
                bSort: false,
                destroy: true,
                info: false,
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', "Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

function actualizarFila(relacion, punto, pb) {
    const version = $('#selVersion').val();
    $.ajax({
        type: 'POST',
        url: controller + 'ActualizarFila',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            relacion: relacion,
            punto: punto,
            codigo: pb,
            version: version
        }),
        datatype: 'json',
        traditional: true,
        success: function (model) {
            SetMessage('#message', model.Mensaje, model.TipoMensaje, true);
            if (model.ListaBarrasGrilla) {
                dt.clear().rows.add(model.ListaBarrasGrilla).draw();
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function eliminarFila(pb) {
    const version = $('#selVersion').val();
    $.ajax({
        type: 'POST',
        url: controller + 'EliminarFila',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            codigo: pb,
            version: version
        }),
        datatype: 'json',
        traditional: true,
        success: function (model) {
            SetMessage('#message', model.Mensaje, model.TipoMensaje, true);
            if (model.ListaBarrasGrilla) {
                dt.clear().rows.add(model.ListaBarrasGrilla).draw();
            }
        },
        complete: function () {
            listaBarrasVersion(version);
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

//Agrega una dia a una fecha(resultado en string)
function AddDays(value) {
    var ndias = 1;
    var dt = ConvertStringToDate(value);
    dt.setDate(dt.getDate() + ndias);

    var dd = (dt.getDate() < 10 ? '0' : '') + dt.getDate();
    var MM = ((dt.getMonth() + 1) < 10 ? '0' : '') + (dt.getMonth() + 1);
    var yyyy = dt.getFullYear();

    var dtf = dd + '/' + MM + '/' + yyyy;

    return dtf;
}

//Valida las fechas ingresadas
function ValidateDates(fecini, fecfin) {
    var valid = false;
    var fini = ConvertStringToDate(fecini);
    var ffin = ConvertStringToDate(fecfin);

    if (ffin.getTime() >= fini.getTime()) {
        valid = true;
    }

    return valid;
}

//Convierte un tipo string a tipo date
function ConvertStringToDate(value) {
    var pattern = /^(\d{1,2})\/(\d{1,2})\/(\d{4})$/;
    var arrayDate = value.match(pattern);
    var dt = new Date(arrayDate[3], arrayDate[2] - 1, arrayDate[1]);

    return dt;
}

mostrarError = function () {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-error');
    $('#mensaje').text("Ha ocurrido un error.");
}

function exportarData() {
    
    let cabecera = $('#dtDatos thead th').map(function () {
        return $(this).text();
    }).get();

    let dato = dt.data().toArray();

    $.ajax({
        type: 'POST',
        url: controller + 'ExportarData',
        contentType: 'application/json',
        data: JSON.stringify({
            cabecera: cabecera,
            dato: dato
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result != -1) {
                window.location = controller + 'abrirarchivo?formato=' + 1 + '&file=' + result;
                //mostrarMensaje('mensaje', 'exito', "Felicidades, el archivo se descargo correctamente...!");
                SetMessage('#message', 'El archivo se descargo correctamente...', 'success', false);
            }
            else {
                //mostrarMensaje('mensaje', 'error', "Lo sentimos, ha ocurrido un error inesperado");
                SetMessage('#message', 'Lo sentimos, ha ocurrido un error inesperado...', 'warning', false);
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

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

function SetMessage(container, msg, tpo, del) {//{Contenedor, mensaje(string), tipoMensaje(string), delay(bool)}
    var new_class = "msg-" + tpo;//Identifica la nueva clase css
    $(container).removeClass($(container).attr('class'));//Quita la clase css previa
    $(container).addClass(new_class);
    $(container).html(msg);//Carga el mensaje
    //$(container).show();

    //Focus to message
    $('html, body').animate({ scrollTop: 0 }, 5);

    //Mensaje con delay o no
    if (del) $(container).show(0).delay(5000).hide(0);//5 Segundos
    else $(container).show();
}