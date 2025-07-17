var name = "RelacionAreas/";
var controller = siteRoot + "PronosticoDemanda/" + name;
var rutaImagenes = siteRoot + "Content/Images/";

var prueba;//Entidad multiusos para debug
var gridTabla, gridTbA, gridTbB, gridHistorial, gridTbABarra, gridTbBBarra;//Almacenan los DataTables
var dataRow;//Almacena los datos de la fila seleccionada de la tabla principal
var noChange = false;//Flag que evita la recarga innecesaria de las listas desplegables
var nivel, areacodi, nombre;
var pop;

$(document).ready(function () {

    $('#cboNivel').change(function () {
        nivel = $('#cboNivel').val();
        Detalle(nivel);
    });

    $('#btn-pop-guardar').on('click', function () {
        var idsubestacion = $('#idSubestacion').val();
        var areacodi = $('#cboAreaOperativa').val();
        RelacionSubestacionUpdate(areacodi, idsubestacion);
    });

    $('#cboNivel').multipleSelect({
        filter: false,
        single: true,
        placeholder: 'Seleccione'
    });
});

$(document).on('change', '#tbA tr td .isRadio', function (e) {
    var row = $(this).closest('tr');
    var r = gridTbA.row(row).data();
    var f = 1;
    gridTbA.row(row).remove().draw();
    Intercambio(1, '', r[0], r[2], r[3]);
});

$(document).on('change', '#tbABarra tr td .isRadio', function (e) {
    var row = $(this).closest('tr');
    var r = gridTbABarra.row(row).data();
    //var f = 1;
    gridTbABarra.row(row).remove().draw();
    IntercambioBarra(1, '', r[0], r[1], r[2], r[4], r[5]);
});

$(document).on('change', '#tbB tr td .isRadio', function (e) {
    console.log(this,"elemento");
    var row = $(this).closest('tr');
    var r = gridTbB.row(row).data();

    if (r[3] != 0 && r[3] != areacodi) {
        var msj = confirm("Esta Subestacion ya esta tomada, esta seguro que desea moverla?")
        if (msj == true) {
            var f = this.closest('tr').lastChild.firstChild.value;
            gridTbB.row(row).remove().draw();
            Intercambio(2, 'checked', r[0], r[2], r[3]);
        }
        else {
            $(this).prop('checked', false);
        }

    }
    else {
        var f = this.closest('tr').lastChild.firstChild.value;
        gridTbB.row(row).remove().draw();
        Intercambio(2, 'checked', r[0], r[2], r[3]);
    }

});

$(document).on('change', '#tbBBarra tr td .isRadio', function (e) {
    var row = $(this).closest('tr');
    var r = gridTbBBarra.row(row).data();

    if (r[0] != areacodi && r[0] != -1) {
        var msj = confirm("Esta Subestacion ya esta tomada, esta seguro que desea moverla?")
        if (msj == true) {
            var f = this.closest('tr').lastChild.firstChild.value;
            gridTbBBarra.row(row).remove().draw();
            IntercambioBarra(2, 'checked', r[0], r[1], r[2], r[4], r[5]);
        }
        else {
            $(this).prop('checked', false);
        }

    }
    else {
        var f = this.closest('tr').lastChild.firstChild.value;
        gridTbBBarra.row(row).remove().draw();
        IntercambioBarra(2, 'checked', r[0], r[1], r[2], r[4], r[5]);
    }

});

function Intercambio(dt, est, codi, nombre, padre) {
    var f;
        if (dt == 1) {
            f = GetRow(est, codi, nombre, padre)
            gridTbB.row.add(f).draw();
            gridTbB.order([2, 'asc']).draw();
        }
        if (dt == 2) {
            f = GetRow(est, codi, nombre, padre)
            gridTbA.row.add(f).draw();
            gridTbA.order([2, 'asc']).draw();
        }
}

function IntercambioBarra(dt, est, areacodi, barracodi, grupocodi, gruponomb, catecodi) {
    var f;
    if (dt == 1) {
        f = GetRowBarra(est, areacodi, barracodi, grupocodi, gruponomb, catecodi)
        gridTbBBarra.row.add(f).draw();
        gridTbBBarra.order([2, 'asc']).draw();
    }
    if (dt == 2) {
        f = GetRowBarra(est, areacodi, barracodi, grupocodi, gruponomb, catecodi)
        gridTbABarra.row.add(f).draw();
        gridTbABarra.order([2, 'asc']).draw();
    }
}

function Detalle(nivel) {

    $('#dtDetalle').html("");

    $.ajax({
        type: 'POST',
        url: controller + 'DetalleList',
        data: {
            idNivel: nivel
        },
        datatype: 'json',
        success: function (result) {
            dt = $('#dtDetalle').DataTable({
                data: result,
                columns: [
                    { data: "areacodi", title: "ID", visible: false },
                    { data: "areapadre", title: "PADRE", visible: false },
                    { data: "anivelcodi", title: "NIVEL", visible: false },
                    { data: "areaabrev", title: "ABREVIATURA" },
                    { data: "areanomb", title: "NOMBRE" },
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
                            targets: [5], width: 70,
                            defaultContent: '<input type="button" class="clsEditar" value="Relacionar SSEE - AO"/> '

                        },
                        {
                            targets: [6], width: 70,
                            defaultContent: '<input type="button" class="clsBarra" value="Relacionar SSEE - BARRA"/> '
                        }
                    ],
                searching: true,
                bLengthChange: false,
                bSort: true,
                destroy: true,
                paging: true,
                pageLength: 25,
                info: false,
            });

            if (nivel != 0) {
                dt.column(6).visible(false);
            }
        }
    });
}

$(document).on('click', '#dtDetalle tr td .clsEditar', function (e) {

    var row = $(this).closest('tr');
    var r = dt.row(row).data();
    areacodi = r.areacodi;
    nombre = r.areanomb;
    console.log(areacodi, nombre);

    //Si se selcciona Subestacion
    if (nivel == 0) {
        pop = $('#popupSubestacion').bPopup({
            easing: 'easeOutBack',
            speed: 350,
            transition: 'fadeIn',
            modalClose: false
        },
            function () {
                $.ajax({
                    type: 'POST',                 
                    url: controller + "SubestacionAO",
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify({
                        areacodi: areacodi
                    }),
                    datatype: 'json',
                    traditional: true,                  
                    success: function (result) {
                        $("#nombreSubestacion").val(nombre);
                        $("#cboAreaOperativa").val(result.Area.Areacodi);
                        $("#idSubestacion").val(areacodi);
                    },
                    error: function () {
                        alert("Ha ocurrido un problema...");
                    }
                });
            })

    }

    //Si se selecciona Area Operativa
    if (nivel == 1) {
        pop = $('#popupMantenimiento').bPopup({
            easing: 'easeOutBack',
            speed: 350,
            transition: 'fadeIn',
            modalClose: false
        },
            function () {
                $.ajax({
                    type: 'POST',
                    url: controller + "DetalleUpdate",
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify({
                        areapadre: areacodi,
                        relpadre: areacodi,
                        relnivel: nivel
                    }),
                    datatype: 'json',
                    traditional: true,                  
                    success: function (result) {
                        document.getElementById('titleMantenimiento').innerHTML = 'AO - SSEE';

                        $('#gridMantenimiento').empty();
                        $('#gridMantenimiento').html(result);
                        $('#txtNombre').val(nombre);

                        $('#btnRegistrar').on('click', function () {
                            var rowDt = gridTbA.rows().data();
                            if (rowDt.length != 0) {

                                var arrayData = [];
                                for (var i = 0; i < rowDt.length; i++) {
                                    var objRow = rowDt[i];
                                    var obj = { Areacodi: 0, Areapadre: 0 };
                                    obj.Areacodi = parseInt(objRow[0]);
                                    obj.Areapadre = parseInt(objRow[3]);
                                    arrayData.push(obj);
                                }
                                Grabar(areacodi, arrayData);
                            }
                        });


                        gridTbA = $('#tbA').DataTable({
                            columnDefs:
                                [
                                    { targets: [0], visible: false, searchable: false, orderable: false },
                                    { targets: [1], visible: true, searchable: false, orderable: false },
                                    { targets: [2], visible: true, searchable: true, orderable: false },
                                    { targets: [3], visible: false, searchable: false, orderable: false },
                                ],

                            searching: true,
                            bLengthChange: false,
                            bSort: true,
                            destroy: true,
                            paging: false,
                            info: false,
                            scrollY: 150,
                            scrollCollapse: true
                        });

                        gridTbB = $('#tbB').DataTable({
                            columnDefs:
                                [
                                    { targets: [0], visible: false, searchable: false, orderable: false },
                                    { targets: [1], visible: true, searchable: false, orderable: false },
                                    {
                                        targets: [2],
                                        visible: true, searchable: true, orderable: false,
                                        render: function (data, type, row) {
                                            if (row[3] > 0) {
                                                return '<font color="red">' + data + '</font>';
                                            }
                                            else {
                                                return data;
                                            }
                                        }
                                    },
                                    { targets: [3], visible: false, searchable: false, orderable: false },
                                ],
                            searching: true,
                            bLengthChange: false,
                            bSort: true,
                            destroy: true,
                            paging: false,
                            info: false,
                            scrollY: 150,
                            scrollCollapse: true
                        });


                        var dataSel = $('#dbSeleccionados').data('seleccionados');
                        var dataDis = $('#dbDisponibles').data('disponibles');
                        var tbSel = GetDataTable(dataSel, false);
                        var tbDis = GetDataTable(dataDis, true);
                        gridTbA.clear();
                        gridTbB.clear();
                        gridTbA.rows.add(tbSel).draw();
                        gridTbB.rows.add(tbDis).draw();
                        noChange = false;
                    }
                });
            })

    }
});

$(document).on('click', '#dtDetalle tr td .clsBarra', function (e) {
    var row = $(this).closest('tr');
    var r = dt.row(row).data();
    areacodi = r.areacodi;
    nombre = r.areanomb;

    pop = $('#popupBarra').bPopup({
        easing: 'easeOutBack',
        speed: 350,
        transition: 'fadeIn',
        modalClose: false
    },
        function () {
            $.ajax({
                type: 'POST',
                url: controller + 'RelacionSubestacionBarra',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({
                    areacodi: areacodi
                }),
                datatype: 'json',
                traditional: true,
                success: function (result) {
                    document.getElementById('titleBarra').innerHTML = 'SSEE - BARRA';

                    $('#gridBarra').empty();
                    $('#gridBarra').html(result);
                    $('#txtSubNombre').val(nombre);

                    $('#btnRegistrarBarra').on('click', function () {
                        var rowDtA = gridTbABarra.rows().data();
                        var rowDtB = gridTbBBarra.rows().data();

                        if (rowDtA.length != 0) {

                            var arrayDataBarraA = [];
                            for (var i = 0; i < rowDtA.length; i++) {
                                var objRowA = rowDtA[i];
                                arrayDataBarraA.push(parseInt(objRowA[2]));
                            }
                            GrabarBarraA(areacodi, arrayDataBarraA);
                        }

                        if (rowDtB.length != 0) {

                            var arrayDataBarraB = [];
                            for (var i = 0; i < rowDtB.length; i++) {
                                var objRowB = rowDtB[i];
                                if (objRowB[0] == areacodi) {
                                    arrayDataBarraB.push(parseInt(objRowB[2]));
                                }
                            }
                            GrabarBarraB(arrayDataBarraB);
                        }
                    });


                    gridTbABarra = $('#tbABarra').DataTable({
                        columnDefs:
                            [
                                { targets: [0], visible: false, searchable: false, orderable: false },
                                { targets: [1], visible: false, searchable: false, orderable: false },
                                { targets: [2], visible: false, searchable: false, orderable: false },
                                { targets: [3], visible: true, searchable: false, orderable: false },
                                { targets: [4], visible: true, searchable: false, orderable: false },
                                { targets: [5], visible: false, searchable: false, orderable: false },
                            ],

                        searching: true,
                        bLengthChange: false,
                        bSort: true,
                        destroy: true,
                        paging: false,
                        info: false,
                        scrollY: 150,
                        scrollCollapse: true
                    });

                    gridTbBBarra = $('#tbBBarra').DataTable({
                        columnDefs:
                            [
                                { targets: [0], visible: false, searchable: false, orderable: false },
                                { targets: [1], visible: false, searchable: false, orderable: false },
                                { targets: [2], visible: false, searchable: false, orderable: false },
                                { targets: [3], visible: true, searchable: false, orderable: false },
                                {
                                    targets: [4],
                                    visible: true, searchable: true, orderable: false,
                                    render: function (data, type, row) {
                                        if (row[0] != -1 && row[0] != areacodi) {
                                            return '<font color="red">' + data + '</font>';
                                        }
                                        else {
                                            return data;
                                        }
                                    }
                                },
                                { targets: [5], visible: false, searchable: false, orderable: false },
                            ],
                        searching: true,
                        bLengthChange: false,
                        bSort: true,
                        destroy: true,
                        paging: false,
                        info: false,
                        scrollY: 150,
                        scrollCollapse: true
                    });


                    var dataSelBarra = $('#dbBarrasSeleccionados').data('seleccionadosbarra');
                    var dataDisBarra = $('#dbBarrasDisponibles').data('disponiblesbarra');
                    var tbSelBarra = GetDataTableBarra(dataSelBarra, false);
                    var tbDisBarra = GetDataTableBarra(dataDisBarra, true);
                    gridTbABarra.clear();
                    gridTbBBarra.clear();
                    gridTbABarra.rows.add(tbSelBarra).draw();
                    gridTbBBarra.rows.add(tbDisBarra).draw();
                    noChange = false;
                }
            });
        })
    
});

function RelacionSubestacionBarra(areacodi, nombre) {
    $.ajax({
        type: 'POST',
        url: controller + "RelacionSubestacionBarra",
        data: {
            areacodi: areacodi
        },
        success: function (result) {
            $('#gridBarra').empty();
            $('#gridBarra').html(result);

            $('#txtSubNombre').val(nombre);

            $('#btnRegistrarBarra').on('click', function () {
                var rowDtA = gridTbABarra.rows().data();
                var rowDtB = gridTbBBarra.rows().data();
                
                if (rowDtA.length != 0) {

                    var arrayDataBarraA = [];
                    for (var i = 0; i < rowDtA.length; i++) {
                        var objRowA = rowDtA[i];
                        arrayDataBarraA.push(parseInt(objRowA[2]));
                    }
                    GrabarBarraA(areacodi, arrayDataBarraA);
                }

                if (rowDtB.length != 0) {

                    var arrayDataBarraB = [];
                    for (var i = 0; i < rowDtB.length; i++) {
                        var objRowB = rowDtB[i];
                        if (objRowB[0] == areacodi) {
                            arrayDataBarraB.push(parseInt(objRowB[2]));
                        }
                    }
                    GrabarBarraB(arrayDataBarraB);
                }
            });


            gridTbABarra = $('#tbABarra').DataTable({
                columnDefs:
                    [
                        { targets: [0], visible: false, searchable: false, orderable: false },
                        { targets: [1], visible: false, searchable: false, orderable: false },
                        { targets: [2], visible: false, searchable: false, orderable: false },
                        { targets: [3], visible: true, searchable: false, orderable: false },
                        { targets: [4], visible: true, searchable: false, orderable: false },
                        { targets: [5], visible: false, searchable: false, orderable: false },
                    ],

                searching: true,
                bLengthChange: false,
                bSort: true,
                destroy: true,
                paging: false,
                info: false,
                scrollY: 150,
                scrollCollapse: true
            });

            gridTbBBarra = $('#tbBBarra').DataTable({
                columnDefs:
                    [
                        { targets: [0], visible: false, searchable: false, orderable: false },
                        { targets: [1], visible: false, searchable: false, orderable: false },
                        { targets: [2], visible: false, searchable: false, orderable: false},
                        { targets: [3], visible: true, searchable: false, orderable: false },
                        {
                            targets: [4],
                            visible: true, searchable: true, orderable: false,
                            render: function (data, type, row) {
                                if (row[0] != -1 && row[0] != areacodi) {
                                    return '<font color="red">' + data + '</font>';
                                }
                                else {
                                    return data;
                                }
                            }
                        },
                        { targets: [5], visible: false, searchable: false, orderable: false },
                    ],
                searching: true,
                bLengthChange: false,
                bSort: true,
                destroy: true,
                paging: false,
                info: false,
                scrollY: 150,
                scrollCollapse: true
            });


            var dataSelBarra = $('#dbBarrasSeleccionados').data('seleccionadosbarra');
            var dataDisBarra = $('#dbBarrasDisponibles').data('disponiblesbarra');
            var tbSelBarra = GetDataTableBarra(dataSelBarra, false);
            var tbDisBarra = GetDataTableBarra(dataDisBarra, true);
            gridTbABarra.clear();
            gridTbBBarra.clear();
            gridTbABarra.rows.add(tbSelBarra).draw();
            gridTbBBarra.rows.add(tbDisBarra).draw();
            noChange = false;
        }
    });
}

function Mantenimiento(selTipo, nombre) {
    $.ajax({
        type: 'POST',
        url: controller + "DetalleUpdate",
        data: {
            areapadre: selTipo,
            relpadre: selTipo,
            relnivel: nivel
        },
        success: function (result) {
            $('#gridMantenimiento').empty();
            $('#gridMantenimiento').html(result);

            $('#txtNombre').val(nombre);


            $('#btnRegistrar').on('click', function () {
                var rowDt = gridTbA.rows().data();
                if (rowDt.length != 0) {

                    var arrayData = [];
                    for (var i = 0; i < rowDt.length; i++) {
                        var objRow = rowDt[i];
                        var obj = { Areacodi: 0, Areapadre:0};
                        obj.Areacodi = parseInt(objRow[0]);
                        obj.Areapadre = parseInt(objRow[3]);
                        arrayData.push(obj);
                    }
                    Grabar(selTipo, arrayData);
                }
            });


            gridTbA = $('#tbA').DataTable({
                columnDefs:
                    [
                        { targets: [0], visible: false, searchable: false, orderable: false },
                        { targets: [1], visible: true, searchable: false, orderable: false },
                        { targets: [2], visible: true, searchable: true, orderable: false },
                        { targets: [3], visible: false, searchable: false, orderable: false },
                    ],

                searching: true,
                bLengthChange: false,
                bSort: true,
                destroy: true,
                paging: false,
                info: false,
                scrollY: 150,
                scrollCollapse: true
            });

            gridTbB = $('#tbB').DataTable({
                columnDefs:
                    [
                        { targets: [0], visible: false, searchable: false, orderable: false },
                        { targets: [1], visible: true, searchable: false, orderable: false },
                        {
                            targets: [2],
                            visible: true, searchable: true, orderable: false,
                            render: function (data, type, row) {
                                if (row[3] > 0) {
                                    return '<font color="red">' + data + '</font>';
                                }
                                else {
                                    return data;
                                }
                            }
                        },
                        { targets: [3], visible: false, searchable: false, orderable: false},
                    ],
                searching: true,
                bLengthChange: false,
                bSort: true,
                destroy: true,
                paging: false,
                info: false,
                scrollY: 150,
                scrollCollapse: true
            });

           
            var dataSel = $('#dbSeleccionados').data('seleccionados');
            var dataDis = $('#dbDisponibles').data('disponibles');
            var tbSel = GetDataTable(dataSel, false);
            var tbDis = GetDataTable(dataDis, true);
            gridTbA.clear();
            gridTbB.clear();
            gridTbA.rows.add(tbSel).draw();
            gridTbB.rows.add(tbDis).draw();
            noChange = false;
        }
    });
}

function GetDataTable(lista, disponible) {
    var tb = [];
    for (var i = 0; i < lista.length; i++) {
        if (disponible) {
            var r = GetRow('', lista[i].Areacodi, lista[i].Areanomb, lista[i].Areapadre);
        }
        else {
            var r = GetRow('checked', lista[i].Areacodi, lista[i].Areanomb, lista[i].Areapadre);
        }//AreaCodi
        tb.push(r);
    }

    return tb;
}

function GetDataTableBarra(lista, disponible) {
    var tb = [];
    for (var i = 0; i < lista.length; i++) {
        if (disponible) {
            var r = GetRowBarra('', lista[i].Areacodi, lista[i].Barracodi, lista[i].Grupocodi, lista[i].Gruponomb, lista[i].Catecodi);
        }
        else {
            var r = GetRowBarra('checked', lista[i].Areacodi, lista[i].Barracodi, lista[i].Grupocodi, lista[i].Gruponomb, lista[i].Catecodi);
        }//AreaCodi
        tb.push(r);
    }

    return tb;
}

function GetRow(est, areacodi, areanomb, areapadre){
    var radio = '<input type="checkbox" class="isRadio" ' + est + ' />';
    var row = [areacodi, radio, areanomb, areapadre];
    return row;
}

function GetRowBarra(est, areacodi, barracodi, grupocodi, gruponomb, catecodi) {
    var radio = '<input type="checkbox" class="isRadio" ' + est + ' />';
    var row = [areacodi, barracodi, grupocodi, radio, gruponomb, catecodi];
    return row;
}

function SubestacionAO(area, nombre) {
    $.ajax({
        type: 'POST',
        contentType: 'application/json',
        url: controller + "SubestacionAO",
        data: JSON.stringify({
            idArea: area
        }),
        dataType: 'json',
        traditional: true,
        success: function (result) {

            $("#nombreSubestacion").val(nombre);
            $("#cboAreaOperativa").val(result.Area.Areacodi);
            $("#idSubestacion").val(area);
            //$("#popupSubestacion").bPopup({
            //    modalClose: false
            //});
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function Grabar(area, puntos) {
    $.ajax({
        type: 'POST',
        contentType: 'application/json',
        url: controller + "Grabar",
        data: JSON.stringify({
            idArea: area,
            listPuntos: puntos
        }),
        dataType: 'json',
        traditional: true,
        success: function (result) {
            if (result.Valido != -1) {
                $("#popupMantenimiento").bPopup().close();
                Detalle(nivel);
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

function GrabarBarraA(codigo, BarrasA) {
    console.log(BarrasA);
    console.log(codigo);
    $.ajax({
        type: 'POST',
        contentType: 'application/json',
        url: controller + "GrabarBarraSeleccionados",
        data: JSON.stringify({
            subestacion: codigo,
            listBarrasA: BarrasA
        }),
        dataType: 'json',
        traditional: true,
        success: function (result) {
            if (result.Valido != -1) {
                alert(result.Mensaje);
                //$("#popupBarra").bPopup().close();
                Detalle(nivel);
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

function GrabarBarraB(BarrasB) {
    $.ajax({
        type: 'POST',
        contentType: 'application/json',
        url: controller + "GrabarBarraDisponibles",
        data: JSON.stringify({
            listBarrasB: BarrasB
        }),
        dataType: 'json',
        traditional: true,
        success: function (result) {
            if (result.Valido != -1) {
                 $("#popupBarra").bPopup().close();
                 Detalle(nivel);
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

function RelacionSubestacionUpdate(areacodi, idsubestacion) {
    $.ajax({
        type: 'POST',
        contentType: 'application/json',
        url: controller + "RelacionSubestacionUpdate",
        data: JSON.stringify({
            idArea: areacodi,
            idSubestacion: idsubestacion
        }),
        dataType: 'json',
        traditional: true,
        success: function (result) {
            alert(result.Mensaje);
            $("#popupSubestacion").bPopup().close();

            
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}