var name = "ReduccionRed/";
var controller = siteRoot + "PronosticoDemanda/" + name;
var rutaImagenes = siteRoot + "Content/Images/";

var httable, httableperdidas;
var version = -1, listaTemporal;
var pop, listPm, flag;

$(document).ready(function () {

    $('.f-select').multipleSelect({
        filter: true,
        single: true,
        placeholder: 'Seleccione'
    });

    $('.fcp-select').multipleSelect({
        filter: true,
        single: false,
        placeholder: 'Seleccione',
        selectAll: false
    });

    //fcp-select-defecto
    $('.fpm-select').each(function () {
        var element = this;
        $(element).multipleSelect({
            name: element.id,
            filter: true,
            single: false,
            placeholder: 'Seleccione',
            selectAll: false,
            onClose: function () {
                var e = document.getElementById(this.name);
                document.getElementById('lblgauss').style.visibility = 'visible';
                document.getElementById('lblperdida').style.visibility = 'visible';
                ListBarrasPopup(e);

            },
            onCheckAll: function () {
                var e = document.getElementById(this.name);
                $(e).val(null);

            }
        });
    });

    $('.ftp-select').each(function () {
        var element = this;
        $(element).multipleSelect({
            name: element.id,
            filter: true,
            single: false,
            placeholder: 'Seleccione',
            selectAll: true,
            onClose: function () {
                var e = document.getElementById(this.name);
                var v = $('#cboVersion').val();
                var t = $('#cboTipo').val();
                ListaVersion(v, t);
            }
        });
    });

    $('#cboVersion').change(function () {
        version = $('#cboVersion').val();
        if (version > 0) {
            document.getElementById('btnNuevo').style.visibility = 'visible';
            document.getElementById('redTipo').style.visibility = 'visible';
            document.getElementById('btnCpToPm').style.visibility = 'visible';
            document.getElementById('btnActualizarVersion').style.visibility = 'visible';
            document.getElementById('btnExportar').style.visibility = 'visible';
        }
        $('#cboTipo option:selected').removeAttr("selected");
        ListaVersion(version, 0);
    });

    $('#cboBarrascp').change(function () {
        var Barracp = $('#cboBarrascp').val();
        $('#httable').empty();
        $('#httableperdidas').empty();
        document.getElementById('lblgauss').style.visibility = 'hidden';
        document.getElementById('lblperdida').style.visibility = 'hidden';

        UpdatePopupPM(Barracp, version);

    });

    $('#btnNuevo').on('click', function () {
        $('#httable').empty();
        $('#httableperdidas').empty();
        document.getElementById('lblgauss').style.visibility = 'hidden';
        document.getElementById('lblperdida').style.visibility = 'hidden';

        pop = $('#popupNuevo').bPopup({
            easing: 'easeOutBack',
            speed: 350,
            transition: 'fadeIn',
            modalClose: false
        },
            function () {
                $.ajax({
                    type: 'POST',
                    url: controller + 'RefreshListCbo',
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify({
                        version: $('#cboVersion').val()
                    }),
                    datatype: 'json',
                    traditional: true,
                    success: function (result) {
                        RefillDropDowList($('#cboBarrascp'), result.ListBarraPopCP, 'Grupocodi', 'Gruponomb');
                        RefillDropDowList($('#cboBarraspm'), result.ListBarraPopPM, 'Grupocodi', 'Gruponomb');
                    },
                    error: function () {
                        alert("Ha ocurrido un problema...");
                    }
                });
            }) 
    });

    $('#btnActualizarVersion').on('click', function () {

        pop = $('#popupEditVersion').bPopup({
            easing: 'easeOutBack',
            speed: 350,
            transition: 'fadeIn',
            modalClose: false
        },
            function () {
                $.ajax({
                    type: 'POST',
                    url: controller + 'ListVersion',
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify({
                        codigo: $('#cboVersion').val()
                    }),
                    datatype: 'json',
                    traditional: true,
                    success: function (result) {
                            document.getElementById('cboActualizarEstado').disabled = false;
                        $("#txtActualizarNombre").val(result.Prnvernomb);
                        if (result.Prnverestado == "A") {
                            document.getElementById('cboActualizarEstado').disabled = true;
                        }
                        $("#cboActualizarEstado").val(result.Prnverestado);
                    },
                    error: function () {
                        alert("Ha ocurrido un problema...");
                    }
                });
            }) 
    });

    $('#btnEditVersion').on('click', function () {
        var nombreVersion = $('#txtActualizarNombre').val(); 
        var estadoVersion = $('#cboActualizarEstado').val();
        SaveVersion(version, nombreVersion, estadoVersion);

    });

    $('#btn-pop-Defecto').on('click', function () {
        SaveDefecto();
    });

    $('#btnCpToPm').on('click', function () {

        flag = -1;

        pop = $('#popupDefecto').bPopup({
            easing: 'easeOutBack',
            speed: 350,
            transition: 'fadeIn',
            modalClose: false
        },
            function () {
                $.ajax({
                    type: 'POST',
                    url: controller + 'RefreshListDefecto',
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify({
                        version: $('#cboVersion').val()
                    }),
                    datatype: 'json',
                    traditional: true,
                    success: function (result) {
                        RefillDropDowList($('#cboBarraDefecto'), result.ListBarraDefecto, 'Grupocodi', 'Gruponomb');
                        document.getElementById("txtperdidaDefecto").value = "0";
                        document.getElementById("txtgaussDefecto").value = "1";
                        $("#txtNombreDefecto").val('');
                    },
                    error: function () {
                        alert("Ha ocurrido un problema...");
                    }
                });
            }) 
    });

    $('#btn-pop-editar').on('click', function () {
        var tablaEdit = httableedit.getData();
        var cp= [];
        var cpId = $('#cboBarracpedit').val();

        var pm = $('#lblidbarrapm').text();

        var sumUno = 0;
        if (document.getElementById("chkValUnoEdit").checked == true) {
            sumUno = 1;
        }
        else {
            sumUno = 2;
        }

        UpdateReduccionRed(tablaEdit, cpId, pm, sumUno);
    });

    $('#btnGrabarVersion').on('click', function () {
        var nombreVersion = $('#txtNombreVersion').val();
        var estadoVersion = $('#cboEstadoVersion').val();
        SaveVersion(-1, nombreVersion, estadoVersion, 1);
    });

    $('#btnNuevaVersion').on('click', function () {
        document.getElementById('cboEstadoVersion').disabled = false;

        if ($("#cboVersion").children().length == 0) {
            document.getElementById('cboEstadoVersion').disabled = true;
        }

        $('#txtNombreVersion').val('');
        $("#popupNuevaVersion").bPopup({
            modalClose: false
        });
    });

    $('#btn-pop-guardar').on('click', function () {

        if ($('#cboBarrascp').val() != null && $('#cboBarraspm').val() != null) {

            var barracp = $('#cboBarrascp').val();
            var barrapm = $('#cboBarraspm').val();

            var datareduccion = httable.getData();
            var dataperdida = httableperdidas.getData();

            version = $('#cboVersion').val();

            var x = 0;

            //Validacion del -1 para gauss
            //for (var i = 0; i < barrapm.length; i++) {
            //    for (var j = 0; j < barracp.length; j++) {
            //        if (Number(datareduccion[j][i + 1]) < 0) {
            //            alert("No se pueden ingresar valores negativos");
            //            x = 1;
            //            break;
            //        }
            //    }
            //}

            var sumUno = 0;
            if (document.getElementById("chkValUno").checked == true) {
                sumUno = 1;
            }
            else {
                sumUno = 2;
            }

            if (x != 1) {
                SaveReduccionRed(datareduccion, dataperdida, barracp, barrapm, version, sumUno);
            }
        }
        else {
            alert("Debe seleccionar como minimo 1 Barra PM y CP");
        }
    });

    $('#cboBarracpedit').multipleSelect({
        filter: true,
        single: false,
        placeholder: 'Seleccione',
        selectAll: false,
        onClose: function () {

            var selectedCp = document.querySelectorAll('#cboBarracpedit option:checked');
            var listaNombres = [];
            listCp = $('#cboBarracpedit').val();

            for (var i = 0; i < listCp.length; i++) {
                listaNombres.push(selectedCp[i].text);
            }

            $('#httableedit').html('');
            
            var lista = [];

            $.each(listCp, function (i, item) {
                var c = 0;

                for (var i = 0; i < listaTemporal.barracpId.length; i++) {

                    var datos = [];

                    if (item == listaTemporal.barracpId[i]) {
                        datos.push(listaTemporal.barraGauss[i]);
                        datos.push(listaTemporal.barraPerdida[i]);
                        lista.push(datos);
                        c++;
                    }

                }
                if (c == 0) {
                    datos.push(0);
                    datos.push(0);
                    lista.push(datos);
                }

            });

            containercp = document.getElementById('httableedit');

            httableedit = new Handsontable(containercp, {
                rowHeaders: listaNombres,
                colHeaders: ['GAUSS (#)', 'PERDIDA (%)'],
                fillHandle: false,
                stretchH: 'all',
                rowHeaderWidth: 150
            });
            httableedit.loadData(lista);
        }
    });

    $('#btnExportar').on('click', function () {
        var myTable = $("#dtDetalle").DataTable();
        var form_data = myTable.rows().data();
        console.log(form_data, 'zzz');
        var datos = [];
        for (var i = 0; i < form_data.length; i++) {
            var fila = [];
            fila.push(form_data[i].nombre[0]);
            fila.push(form_data[i].barrapmNombre.join("\r\n"));
            fila.push(form_data[i].barracpNombre.join("\r\n"));
            fila.push(form_data[i].puntoNombre.join("\r\n"));
            fila.push(form_data[i].puntoId.join("\r\n"));
            fila.push(form_data[i].barraGauss.join("\r\n"));
            fila.push(form_data[i].barraPerdida.join("\r\n"));
            datos.push(fila);
        }
        console.log(datos, 'exp');
        Exportar(datos);
    });

});

//23032020
function UpdateReduccionRed(tablaEdit, cp, pm, sumUno) {
    $.ajax({
        type: 'POST',
        url: controller + 'UpdateReduccionRed',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            data: tablaEdit,
            cp: cp,
            pm: pm,
            version: version,
            barraCpOld: listaTemporal.barracpId,
            nombre: $('#txtNombreEdit').val(),
            valSum: sumUno
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result.flagPop == 1) {
                alert(result.Mensaje);
                $("#popupEditar").bPopup().close();
                $('#httableedit').empty();
                ListaVersion(version,0);
            }
            else {
                alert(result.Mensaje);
            }

        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function SaveVersion(id, nombreVersion, estadoVersion) {
    $.ajax({
        type: 'POST',
        url: controller + 'SaveVersion',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            codigo: id,
            nombre: nombreVersion,
            estado: estadoVersion
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            alert("Registro exitoso....");
            $("#popupNuevaVersion").bPopup().close();
            $("#popupEditVersion").bPopup().close();
            document.getElementById('btnNuevo').style.visibility = 'hidden';
            document.getElementById('redTipo').style.visibility = 'hidden';
            document.getElementById('btnCpToPm').style.visibility = 'hidden';
            document.getElementById('btnActualizarVersion').style.visibility = 'visible';
            document.getElementById('btnExportar').style.visibility = 'visible';
            $('#dtDetalle').empty();
            RefreshVersion();
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function RefreshVersion(version) {
    $.ajax({
        type: 'POST',
        url: controller + 'RefreshVersion',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        traditional: true,
        success: function (result) {
            RefillDropDowList($('#cboVersion'), result, 'Prnvercodi', 'Prnvernomb');
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function ListaVersion(version, flag) {

    $('#dtDetalle').html("");

    $.ajax({
        type: 'POST',
        url: controller + 'ReduccionRedList',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            version: version,
            tipo: flag
        }),
        datatype: 'json',
        success: function (result) {
            dt = $('#dtDetalle').DataTable({
                data: result,
                columns: [
                    { data: "id", title: "ID", visible: false },
                    { data: "nombre", title: "NOMBRE" },
                    { data: "barrapmId", title: "PM ID", visible: false },
                    { data: "barrapmNombre", title: "BARRA PM" },
                    { data: "barracpId", title: "CP ID", visible: false },
                    { data: "barracpNombre", title: "BARRA CP" },
                    { data: "puntoNombre", title: "Pto.Medicion" },
                    { data: "puntoId", title: "Pto.Medicion(id)" },
                    { data: "barraTipo", title: "TIPO", visible: false },
                    { data: "barraGauss", title: "GAUSS (#)" },
                    { data: "barraPerdida", title: "PERDIDA (%)" },
                    { data: null, title: '' },
                    { data: null, title: '' }
                ],
                initComplete: function () {
                    $('#dtDetalle').css('width', '100%');
                    $('.dataTables_scrollHeadInner').css('width', '100%');
                    $('.dataTables_scrollHeadInner table').css('width', '98.84%');
                },
                columnDefs:
                    [
                        {
                            targets: [0],
                            createdCell: function (td, cellData, rowData, row, col) {
                                var acumulado = '';
                                $.each(rowData.id, function (i, item) {
                                    var s = '';
                                    s = '<p>' + item + '</p>';
                                    acumulado += s;
                                });

                                $(td).html(acumulado);

                            }
                        },
                        {
                            targets: [4],
                            createdCell: function (td, cellData, rowData, row, col) {
                                var acumulado = '';
                                $.each(rowData.barracpId, function (i, item) {
                                    var s = '';
                                    s = '<p>' + item + '</p>';
                                    acumulado += s;
                                });

                                $(td).html(acumulado);

                            }
                        },
                        {
                            targets: [5],
                            createdCell: function (td, cellData, rowData, row, col) {
                                var acumulado = '';
                                $.each(rowData.barracpNombre, function (i, item) {
                                    var s = '';
                                    s = '<p>' + item + '</p>';
                                    acumulado += s;
                                });

                                $(td).html(acumulado);

                            }
                        },
                        {
                            targets: [6],
                            createdCell: function (td, cellData, rowData, row, col) {
                                var acumulado = '';
                                $.each(rowData.puntoNombre, function (i, item) {
                                    var s = '';
                                    if (item.substring(0, 1) == "P") {
                                        //r.html('• ' + item.substring(1).fontcolor("3352FF"));
                                        s = '<p>' + item.substring(1).fontcolor("3352FF") + '</p>';
                                    } else {
                                        s = '<p>' + item.substring(1).fontcolor("FF3C33") + '</p>';
                                    }
                                    acumulado += s;
                                });

                                $(td).html(acumulado);

                            }
                        },
                        {
                            targets: [7],
                            createdCell: function (td, cellData, rowData, row, col) {
                                var acumulado = '';
                                $.each(rowData.puntoId, function (i, item) {
                                    var s = '';
                                    s = '<p>' + item + '</p>';
                                    acumulado += s;
                                });

                                $(td).html(acumulado);

                            }
                        },
                        {
                            targets: [9],
                            createdCell: function (td, cellData, rowData, row, col) {
                                var acumulado = '';
                                $.each(rowData.barraGauss, function (i, item) {
                                    var s = '';
                                    s = '<p>' + item + '</p>';
                                    acumulado += s;
                                });

                                $(td).html(acumulado);

                            }
                        },
                        {
                            targets: [10],
                            createdCell: function (td, cellData, rowData, row, col) {
                                var acumulado = '';
                                $.each(rowData.barraPerdida, function (i, item) {
                                    var s = '';
                                    s = '<p>' + item + '</p>';
                                    acumulado += s;
                                });

                                $(td).html(acumulado);

                            }
                        },
                        {
                            targets: [11], width: 70,
                            defaultContent: '<img class="clsEditar" src="' + rutaImagenes + 'btn-edit.png" style="cursor: pointer;" title="Editar"/> '

                        },
                        {
                            targets: [12], width: 70,
                            defaultContent: '<img class="clsEliminar" src="' + rutaImagenes + 'btn-cancel.png" style="cursor: pointer;" title="Eliminar"/> '

                        }
                    ],
                searching: true,
                bLengthChange: false,
                bSort: true,
                destroy: true,
                paging: true,
                pageLength: 25,
                info: false,
                order: [[1,'desc']],
            });
        }
    });
}

$(document).on('click', '#dtDetalle tr td .clsEliminar', function (e) {
    var row = $(this).closest('tr');
    var r = dt.row(row).data();

    DeleteReduccionRed(r.barrapmId, version);
});

$(document).on('click', '#dtDetalle tr td .clsEditar', function (e) {
    $('#httableedit').html('');
    $("#txtNombreEdit").val('');

    var row = $(this).closest('tr');
    var r = dt.row(row).data();

    console.log(r, "R");

    if (r.barraTipo == 'R') {
        document.getElementById('lblnombbarrapm').innerHTML = r.barrapmNombre;
        document.getElementById('lblidbarrapm').innerHTML = r.barrapmId;
        $("#txtNombreEdit").val(r.nombre[0]);
        document.getElementById("chkValUnoEdit").checked = true;

        $("#popupEditar").bPopup({
            modalClose: false
        });

        $('#cboBarracpedit').multipleSelect('setSelects', r.barracpId);
        var lista = [];

        var cpId = $('#cboBarracpedit').val();
        $.each(cpId, function (i, item) {
            for (var i = 0; i < r.barracpId.length; i++) {
                if (parseInt(item) == r.barracpId[i]) {
                    var datos = [];
                    datos.push(r.barraGauss[i]);
                    datos.push(r.barraPerdida[i]);
                    lista.push(datos);
                }
            }
        });

        var cpName = [];
        $("#cboBarracpedit :selected").each(function (i, sel) {
            cpName.push($(sel).text().trim());
        });
        //for (var i = 0; i < r.barracpNombre.length; i++) {
        //    var datos = [];
        //    datos.push(r.barraGauss[i]);
        //    datos.push(r.barraPerdida[i]);
        //    lista.push(datos);
        //}

        containercp = document.getElementById('httableedit');

        httableedit = new Handsontable(containercp, {
            rowHeaders: cpName,
            colHeaders: ['GAUSS (#)', 'PERDIDA (%)'],
            fillHandle: false,
            stretchH: 'all',
            rowHeaderWidth: 150
        });
        httableedit.loadData(lista);

        listaTemporal = r;
    }
    else {
        $("#popupDefecto").bPopup({
            modalClose: false
        });
        $("#txtNombreDefecto").val(r.nombre[0]);
        $('#cboBarraDefecto').multipleSelect('setSelects', r.barracpId);
        $("#txtperdidaDefecto").val(r.barraPerdida);
        $("#txtgaussDefecto").val(r.barraGauss);
        flag = r.id[0];
    }
});

//Elimina un registro de la reduccion de red
function DeleteReduccionRed(id, version) {
    $.ajax({
        type: 'POST',
        url: controller + 'DeleteReduccionRed',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            reduccionred: id,
            version: version
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            alert(result);
            version = $('#cboVersion').val();
            ListaVersion(version,0);
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function UpdatePopupPM(barracp, version) {
    $.ajax({
        type: 'POST',
        url: controller + 'UpdatePopupPM',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            barracp: barracp,
            version: version
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {

            RefillDropDowList($('#cboBarraspm'), result, 'Grupocodi', 'Gruponomb');

        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function SaveReduccionRed(datareduccion, dataperdida, barracp, barrapm, version, sumUno) {
    $.ajax({
        type: 'POST',
        url: controller + 'SaveReduccionRed',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            datareduccion: datareduccion,
            dataperdida: dataperdida,
            barracp: barracp,
            barrapm: barrapm,
            version: version,
            nombre: $('#txtNombreSave').val(),
            valSum: sumUno
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result.flagPop == 1) {
                alert("Registro exitoso....");
                $('#httable').html('');
                $('#httableperdidas').html('');
                $("#popupNuevo").bPopup().close();
                ListaVersion(version, 0);
                $('#cboTipo option:selected').removeAttr("selected");
            }
            if (result.flagPop == 0) {
                alert("Algunas Barras PM suman mas de 1....");
            }
            if (result.flagPop == 2) {
                alert(result.Mensaje);
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function SaveDefecto() {
    $.ajax({
        type: 'POST',
        url: controller + 'SaveReduccionDefecto',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            version: $('#cboVersion').val(),
            barra: $('#cboBarraDefecto').val(),
            gauss: $('#txtgaussDefecto').val(),
            perdida: $('#txtperdidaDefecto').val(),
            nombre: $('#txtNombreDefecto').val(),
            id: flag
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            alert("Se realizo el registro...");
            $("#popupDefecto").bPopup().close();
            $('#cboTipo option:selected').removeAttr("selected");
            ListaVersion(version, 0);
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

function ListBarrasPopup(e) {
    $('#httable').html('');
    $('#httableperdidas').html('');

    var barrasPm = $('#cboBarraspm').val();
    var barrasCp = $('#cboBarrascp').val();
    var selectedPm = document.querySelectorAll('#cboBarraspm option:checked');
    var selectedCp = document.querySelectorAll('#cboBarrascp option:checked');
    var listaNombres = [];

    listaNombres.push('');
    for (var i = 0; i < barrasPm.length; i++) {
        listaNombres.push(selectedPm[i].text);
    }

    var data = [];

    for (var i = 0; i < barrasCp.length; i++) {
        var t = [];
        t.push(selectedCp[i].text);
        for (var j = 0; j < barrasPm.length; j++) {
            t.push(0);
        }
        data.push(t);
    }

    //Grilla para Reducciones de red
    container = document.getElementById('httable');

    httable = new Handsontable(container, {
        rowHeaders: false,
        colHeaders: listaNombres,
        cells: function (row, col, prop) {
            var cellProperties = {};
            if (col == 0) {
                cellProperties.readOnly = 'true'
            }
            return cellProperties
        },
        fillHandle: false,
        stretchH: 'all'
    });

    httable.loadData(data);

    //Grilla para Perdidas
    var dataperdidas = [];

    for (var i = 0; i < barrasCp.length; i++) {
        var t = [];
        t.push(selectedCp[i].text);
        for (var j = 0; j < barrasPm.length; j++) {
            t.push(0);
        }
        dataperdidas.push(t);
    }

    containerperdidas = document.getElementById('httableperdidas');

    httableperdidas = new Handsontable(containerperdidas, {
        rowHeaders: false,
        colHeaders: listaNombres,
        cells: function (row, col, prop) {
            var cellProperties = {};
            if (col == 0) {
                cellProperties.readOnly = 'true'
            }
            return cellProperties
        },
        fillHandle: false,
        stretchH: 'all'
    });

    httableperdidas.loadData(dataperdidas);
}

function Exportar(datos) {
    $.ajax({
        type: 'POST',
        url: controller + 'Exportar',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            form: datos
        }),
        datatype: 'json',
        traditional: true,
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

//function PopUpDefectoEdit(nombre, barra, gauss, perdida) {
//    pop = $('#popupDefecto').bPopup({
//        easing: 'easeOutBack',
//        speed: 350,
//        transition: 'fadeIn',
//        modalClose: false
//    },
//        //function () {
//        //    $.ajax({
//        //        type: 'POST',
//        //        url: controller + 'RefreshListDefecto',
//        //        contentType: 'application/json; charset=utf-8',
//        //        data: JSON.stringify({
//        //            version: $('#cboVersion').val(),
//        //            nombre: nombre,
//        //            barra: barra,
//        //            gaus: gauss,
//        //            perdida: perdida
//        //        }),
//        //        datatype: 'json',
//        //        traditional: true,
//        //        success: function (result) {
//        //            RefillDropDowList($('#cboBarraDefecto'), result.ListBarraDefecto, 'Grupocodi', 'Gruponomb');
//        //            document.getElementById("txtperdidaDefecto").value = "0";
//        //            document.getElementById("txtgaussDefecto").value = "1";
//        //        },
//        //        error: function () {
//        //            alert("Ha ocurrido un problema...");
//        //        }
//        //    });
//        //}) 
//}
