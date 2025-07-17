var name = "RelacionBarra/";
var controller = siteRoot + "DemandaPO/" + name;
var imageRoot = siteRoot + "Content/Images/";
var dt;
var htItems, ht, htUpdate;

$(document).ready(function () {
    $('#selVersion').multipleSelect({
        single: true,
        placeholder: 'Seleccione una Versión...',
    });

    $('#selVersion').on('change', function () {
        let version = $('#selVersion').val();
        ListarBarrasxVersion(version);
    });

    $('#btnRegistrarBarra').on('click', function () {
        let element = document.getElementById('selPopBarras');
        const ids = $(element).multipleSelect('getSelects');
        grabarNuevasBarrasSPL(ids);
    });

    $('#btnRegistrarVersion').on('click', function () {
        let element = document.getElementById('selPopBarrasSPL');
        const ids = $(element).multipleSelect('getSelects');
        const version = $('#txtPopNombre').val();
        grabarNuevaVersion(version, ids);
    });

    $('#btnActualizarVersion').on('click', function () {
        let element = document.getElementById('selPopBarrasUpdateSPL');
        const ids = $(element).multipleSelect('getSelects');
        const version = $('#selVersion').val();
        actualizarVersion(version, ids);
    });

    $('#btnEliminar').on('click', function () {
        let version = $('#selVersion').val();
        if (version) {
            EliminarVersion(version);
        } else {
            SetMessage('#message', 'Debe seleccionar una version antes de hacer clic...', 'warning', true);
        }
    });

    $('#btnNuevo').on('click', function () {
        objPopup = $('#popup-nueva-version').bPopup({
            easing: 'easeOutBack',
            speed: 350,
            transition: 'fadeIn',
            modalClose: false,
            onClose: function () {
                $('#txtPopNombre').val('');
                htItems.updateSettings({
                    data: [],
                    columns: ColumnsHandsonBarras()
                });
            }
        }, function () {
            generarRelacion();
        });
    });

    $('#btnActualizar').on('click', function () {
        objPopupUpdate = $('#popup-update-version').bPopup({
            easing: 'easeOutBack',
            speed: 350,
            transition: 'fadeIn',
            modalClose: false,
            onClose: function () {
                $('#txtPopNombreVersion').val('');
                htUpdate.updateSettings({
                    data: [],
                    columns: ColumnsHandsonBarrasSPL()
                });
            }
        }, function () {
            cargarRelacion();
        });
    });

    $('#btnNuevoBarraSPL').on('click', function () {
        objPopupBarra = $('#popup-nueva-barra').bPopup({
            easing: 'easeOutBack',
            speed: 350,
            transition: 'fadeIn',
            modalClose: false,
            onClose: function () {
                ht.updateSettings({
                    data: [],
                    columns: ColumnsHandsonBarras()
                });
            }
        }, function () {
            generarBarraSPL();
        });
    });

    $('.filtroVersionSave').each(function () {
        let element = this;
        $(element).multipleSelect({
            filter: true,
            selectAll: false,
            onClose: function () {
                const ids = $(element).multipleSelect('getSelects');
                const nombs = $(element).multipleSelect('getSelects', 'text');
                if (ids.length == 0) {
                    htItems.updateSettings({
                        data: [],
                        columns: ColumnsHandsonBarras()
                    });
                    return false;
                }

                let newData = ids.map((e, index) => [`${nombs[index].trim()}[${e}]`, ""]);
                console.log(ids, "ids");
                console.log(nombs, "nombs");
                const htData = htItems.getData();
                if (htData.length > 0) {
                    newData.forEach(e => {
                        htData.forEach(f => {
                            if (f[0] == e[0]) e[1] = f[1];
                        });
                    });
                }
                htItems.updateSettings({
                    data: newData,
                    columns: ColumnsHandsonBarras()
                });
            }
        });
    });

    $('.filtroBarraSave').each(function () {
        let element = this;
        $(element).multipleSelect({
            filter: true,
            selectAll: false,
            onClose: function () {
                const ids = $(element).multipleSelect('getSelects');
                const nombs = $(element).multipleSelect('getSelects', 'text');
                if (ids.length == 0) {
                    ht.updateSettings({
                        data: [],
                        columns: ColumnsHandsonBarrasSPL()
                    });
                    return false;
                }

                let newData = ids.map((e, index) => [`${e}`, `${nombs[index].trim()}`]);
                const htData = ht.getData();
                if (htData.length > 0) {
                    newData.forEach(e => {
                        htData.forEach(f => {
                            if (f[0] == e[0]) e[1] = f[1];
                        });
                    });
                }
                ht.updateSettings({
                    data: newData,
                    columns: ColumnsHandsonBarrasSPL()
                });
            }
        });
    });

    $('.filtroVersionUpdate').each(function () {
        let element = this;
        $(element).multipleSelect({
            filter: true,
            selectAll: false,
            onClose: function () {
                const ids = $(element).multipleSelect('getSelects');
                const nombs = $(element).multipleSelect('getSelects', 'text');
                if (ids.length == 0) {
                    htUpdate.updateSettings({
                        data: [],
                        columns: ColumnsHandsonBarrasSPL()
                    });
                    return false;
                }

                let newData = ids.map((e, index) => [`${e}`, `${nombs[index].trim()}`]);
                const htData = htUpdate.getData();
                if (htData.length > 0) {
                    newData.forEach(e => {
                        htData.forEach(f => {
                            if (f[0] == e[0]) e[1] = f[1];
                        });
                    });
                }
                htUpdate.updateSettings({
                    data: newData,
                    columns: ColumnsHandsonBarrasSPL()
                });
            }
        });
    });

    //ListarBarrasxVersion(0);
    iniciarHandsonBarras();
    iniciarHandsonBarrasSPL();
    iniciarHandsonBarraSPLUpdate();
});

function EliminarVersion(version) {
    $.ajax({
        type: 'POST',
        url: controller + 'EliminarVersion',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            id: version
        }),
        datatype: 'json',
        traditional: true,
        success: function (model) {
            RefillDropDownList($('#selVersion'), model.ListaVersiones,
                'Dposplcodi', 'Dposplnombre');
            ListarBarrasxVersion(version);
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function ListarBarrasxVersion(version) {
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
                    { title: 'Area Demanda', data: 'Splfrmareanombre' },
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
                        targets: 3,
                        defaultContent: '<select class="dt-select sel-f-area"></select>',
                    },
                    {
                        targets: -1,
                        defaultContent:
                            '<div class="dt-col-options">' +
                            //`<img src="${imageRoot}btn-edit.png" class="dt-ico-editar" title="Editar registro" />` +
                            `<img src="${imageRoot}prnsave.png" class="dt-ico-editar" title="Guardar registro" />` +
                            `<img src="${imageRoot}prn-ico-limpiar.png" class="dt-ico-limpiar" title="Elimina formulas y area" />` +
                            `<img src="${imageRoot}btn-cancel.png" class="dt-ico-eliminar" title="Eliminar registro" />` +
                            '</div>',
                    },
                ],
                createdRow: function (row, data, index) {
                    console.log(data, 'data');
                    const _objVeg = $(row).find('.sel-f-veg');
                    const _objInd = $(row).find('.sel-f-ind');
                    const _objArea = $(row).find('.sel-f-area');

                    //Obtiene los datos de la vista
                    const _veg = $('#_dataFormulaVeg option').clone();
                    const _ind = $('#_dataFormulaInd option').clone();
                    const _area = $('#_dataAreaDemanda option').clone();

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
                    _objArea.append(_area);

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
                    _objArea.multipleSelect({
                        single: true,
                        placeholder: 'Seleccione una Area...',
                        onClose: function () {
                            const id = $(_objArea).multipleSelect('getSelects');
                            const nomb = $(_objArea).multipleSelect('getSelects', 'text');
                            //data.Nombindustrial = nomb[0];
                            data.Splfrmarea = id[0];
                            data.Splareanombre = nomb[0];
                        },
                    });
                    //Clic en editar
                    $(row).find('.dt-ico-editar').on('click', function () {
                        var row = $(this).closest('tr');
                        var r = dt.row(row).data();
                        actualizarFila(r, 1);
                    });

                    //Clic en eliminar
                    $(row).find('.dt-ico-eliminar').on('click', function () {
                        var row = $(this).closest('tr');
                        var r = dt.row(row).data();
                        eliminarFila(r);
                    });

                    //Clic en limpiar
                    $(row).find('.dt-ico-limpiar').on('click', function () {
                        var row = $(this).closest('tr');
                        var r = dt.row(row).data();
                        actualizarFila(r, 2);
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
                    const _arryArea = [];
                    _arryArea.push(data.Splfrmarea);
                    if (data.formula_area != -1) {
                        _objArea.multipleSelect('setSelects', _arryArea);
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

function actualizarFila(r, f) {
    const version = $('#selVersion').val();
    $.ajax({
        type: 'POST',
        url: controller + 'ActualizarFila',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            entity: r,
            flag: f
        }),
        datatype: 'json',
        traditional: true,
        success: function (model) {
            ListarBarrasxVersion(version);
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function eliminarFila(r) {
    const version = $('#selVersion').val();
    $.ajax({
        type: 'POST',
        url: controller + 'EliminarFila',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            id: r.Splfrmcodi
        }),
        datatype: 'json',
        traditional: true,
        success: function (model) {
            ListarBarrasxVersion(version);
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function cargarRelacion() {
    $.ajax({
        type: 'POST',
        url: controller + 'CargarRelacion',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            version: $('#selVersion').val()
        }),
        datatype: 'json',
        traditional: true,
        success: function (model) {
            const nomb = document.querySelectorAll('#selVersion option:checked');
            document.getElementById("txtPopNombreVersion").value = nomb[0].text;
            RefillDropDownList($('#selPopBarrasUpdateSPL'), model.ListaBarrasSPL,
                'Grupocodi', 'Gruponomb');
            var grupos = [];
            var lista = [];
            model.ListaBarras.forEach(function (obj) {
                grupos.push(obj['Grupocodi']);
                lista.push([obj['Grupocodi'], obj['Gruponomb']]);
            });

            htUpdate.updateSettings({
                data: lista,
                columns: ColumnsHandsonBarrasSPL()
            });

            $('#selPopBarrasUpdateSPL')
                .multipleSelect('setSelects', grupos);
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function generarRelacion() {
    $.ajax({
        type: 'POST',
        url: controller + 'GenerarRelacion',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        traditional: true,
        success: function (model) {
            RefillDropDownList($('#selPopBarrasSPL'), model.ListaBarrasSPL,
                'Barsplcodi', 'Gruponomb');
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function grabarNuevaVersion(version, barras) {
    $.ajax({
        type: 'POST',
        url: controller + 'RegistrarNuevaVersion',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            nombVersion: version,
            idBarras: barras
        }),
        datatype: 'json',
        traditional: true,
        success: function (model) {
            $("#popup-nueva-version").bPopup().close();
            RefillDropDownList($('#selVersion'), model.ListaVersiones,
                'Dposplcodi', 'Dposplnombre');
            ListarBarrasxVersion(version);
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function actualizarVersion(idVersion, idBarras) {
    $.ajax({
        type: 'POST',
        url: controller + 'ActualizarVersion',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            version: idVersion,
            barras: idBarras
        }),
        datatype: 'json',
        traditional: true,
        success: function (model) {
            $("#popup-update-version").bPopup().close();
            //RefillDropDownList($('#selVersion'), model.ListaVersiones,
            //   'Dposplcodi', 'Dposplnombre');
            ListarBarrasxVersion(idVersion);
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function grabarNuevasBarrasSPL(barras) {
    $.ajax({
        type: 'POST',
        url: controller + 'RegistrarBarrasSPL',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            idBarras: barras
        }),
        datatype: 'json',
        traditional: true,
        success: function (model) {
            $("#popup-nueva-barra").bPopup().close();
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function generarBarraSPL() {
    $.ajax({
        type: 'POST',
        url: controller + 'GenerarBarraSPL',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        traditional: true,
        success: function (model) {
            RefillDropDownList($('#selPopBarras'), model.ListaBarrasGrupo,
                'Grupocodi', 'Gruponomb');
            var grupos = [];
            var lista = [];
            model.ListaBarrasSPL.forEach(function (obj) {
                grupos.push(obj['Grupocodi']);
                lista.push([obj['Grupocodi'], obj['Gruponomb']]);
            });

            ht.updateSettings({
                data: lista,
                columns: ColumnsHandsonBarrasSPL()
            });

            $('#selPopBarras')
                .multipleSelect('setSelects', grupos);
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function iniciarHandsonBarras() {
    const contenedor = document.getElementById('htBarrasSPL');
    htItems = new Handsontable(contenedor, {
        data: [],
        fillHandle: true,
        width: '100%',
        colWidths: 250,
        minSpareCols: 0,
        minSpareRows: 0,
        columns: ColumnsHandsonBarras()
    });
}

function iniciarHandsonBarrasSPL() {
    const contenedor = document.getElementById('htBarras');
    ht = new Handsontable(contenedor, {
        data: [],
        fillHandle: true,
        width: '100%',
        colWidths: 250,
        minSpareCols: 0,
        minSpareRows: 0,
        columns: ColumnsHandsonBarrasSPL()
    });
}

function iniciarHandsonBarraSPLUpdate() {
    const contenedor = document.getElementById('htBarrasUpdateSPL');
    htUpdate = new Handsontable(contenedor, {
        data: [],
        fillHandle: true,
        width: '100%',
        colWidths: 250,
        minSpareCols: 0,
        minSpareRows: 0,
        columns: ColumnsHandsonBarrasSPL()
    });
}

function ColumnsHandsonBarras() {
    return [
        {
            type: 'text',
            title: 'Barras SPL',
            readOnly: true,
            className: 'htCenter'
        }
    ];
}

function ColumnsHandsonBarrasSPL() {
    return [
        {
            type: 'text',
            title: 'Id',
            readOnly: true,
            className: 'htCenter',
            data: 0
        },
        {
            type: 'text',
            title: 'Barras SPL',
            readOnly: true,
            className: 'htCenter',
            data: 1
        }
    ];
}

function RefillDropDownList(element, data, data_id, data_name) {
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