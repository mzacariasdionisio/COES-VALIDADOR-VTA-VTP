var prueba;
var tipoEmpresa = 1;
var punto, dat1, dat2;
var DtGridSelec, DtGridTodos;
var DtFila = { id: '', nombre: '' };

$(document).ready(function () {
    DtGridSelec = $('#DtSeleccionado').DataTable({
        columnDefs:
            [
                { targets: [0], type: 'num' },
                { targets: [2], visible: true, searchable: false, orderable: false }
            ],
        searching: true,
        bLengthChange: false,
        bSort: true,
        destroy: true,
        paging: false,
        info: false
    });

    DtGridTodos = $('#DtTodos').DataTable({
        columnDefs:
            [
                { targets: [0], type: 'num' },
                { targets: [2], visible: true, searchable: false, orderable: false }
            ],
        searching: true,
        bLengthChange: false,
        bSort: true,
        destroy: true,
        paging: false,
        info: false
    });

    $('.rdevt').click(function () {
        if ($(this).is(':checked')) {
            var x = $(this).val();
            if (x == 1) {
                tipoEmpresa = x;//Distribuidoras
                $('#tabAgrupaciones').hide();
                $('#tabDistribuidores').show();
                ReiniciarListas(x);
            }
            if (x == 2) {
                tipoEmpresa = x;//Usuarios Libres
                $('#tabDistribuidores').hide();
                $('#tabAgrupaciones').show();
                ReiniciarListas(x);
            }
            console.log("funca los check");
        }
    });

    $('.clsFiltros').multipleSelect({
        filter: true,
        single: true
    });

    $('#cboArea').change(function () {
        var r = $('#cboArea').val();
        if (r != 0) {
            $('#cboEmpresa').val('0');
            FormulaUpdateList(1);
            console.log("funca area");
        }
    });

    $('#cboEmpresa').change(function () {
        var r = $('#cboEmpresa').val();
        if (r != 0) {
            $('#cboPunto').val('0');
            FormulaUpdateList(2);
            console.log("funca empresa");
        }
    });

    $('#cboAreaUL').change(function () {
        var r = $('#cboAreaUL').val();
        if (r != 0) {
            $('#cboEmpresaUL').val('0');
            console.log("funca area ul");
            FormulaUpdateListUL(1);
            
        }
    });

    $('#cboEmpresaUL').change(function () {
        var r = $('#cboEmpresaUL').val();
        if (r != 0) {
            $('#cboAgrupacion').val('0');
            FormulaUpdateListUL(2);
            console.log("funca empresa ul");
        }
    });

    $('#cboPunto').change(function () {
        var x = $('#cboPunto').val();
        var t = $("#cboPunto option:selected").text();
        if (x != 0) {
            punto = x;
            $('#txtPtomedicion').val(t);
            FormulaDetalle();
            console.log("entro a detalle");
        }
        else {
            DtGridSelec.clear().draw();
            DtGridTodos.clear().draw();
            $('#txtPtomedicion').val('');
            console.log("no entro a detalle");
        }
    });


    $('#cboAgrupacion').change(function () {
        var x = $('#cboAgrupacion').val();
        var t = $("#cboAgrupacion option:selected").text();
        if (x != 0) {
            punto = x;
            $('#txtPtomedicion').val(t);
            FormulaDetalle();
            console.log("agrupacion detalle");
        }
        else {
            DtGridSelec.clear().draw();
            DtGridTodos.clear().draw();
            $('#txtPtomedicion').val('');
            console.log("no agrupacion detalle");
        }
    });

    $('#btn-guardar').on('click', function () {
        var array = [];
        var arrayChecks = $('.chkEstado:checkbox:checked');
        if (arrayChecks.length != 0) {
            arrayChecks.each(function () {
                console.log(this, 'this');
                var objCheck = this;
                var obj = { Ptomedicodicalc: 0, Prfrelfactor: 0 };
                obj.Ptomedicodicalc = parseInt(objCheck.value);
                array.push(obj);
            });

            if (punto != 0) {
                FormulaSave(array);
            }
        }
        else {
            alert('Debe relacionar al menos una formula...');
        }
    });

    //$('#btnGrabar').on('click', function () {
       
    //});

    $('#DtSeleccionado tbody').on('change', '.chkEstado', function () {
        var row = $(this).closest('tr');
        var r = DtGridSelec.row(row).data();

        DtFila.id = r[0];
        DtFila.nombre = r[1];

        DtGridSelec.row(row).remove().draw();
        Intercambio(1);
    });

    $('#DtTodos tbody').on('change', '.chkEstado', function () {
        var row = $(this).closest('tr');
        var r = DtGridTodos.row(row).data();

        DtFila.id = r[0];
        DtFila.nombre = r[1];

        DtGridTodos.row(row).remove().draw();
        Intercambio(2);
    });

});

function FormulaDetalle() {
    $.ajax({
        type: 'POST',
        url: controller + "FormulaDetalle",
        data: {
            idPunto: punto
        },
        dataType: 'json',
        traditional: true,
        success: function (result) {
            dat1 = GetDtSeleccionados(result.DtSeleccionado);
            dat2 = GetDtTodos(result.DtTodos);

            DtGridSelec.clear();
            DtGridTodos.clear();
            DtGridSelec.rows.add(dat1).draw();
            DtGridTodos.rows.add(dat2).draw();
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function Intercambio(dt) {
    var f;
    if (dt == 1) {
        f = [DtFila.id, DtFila.nombre, GetEstado(-1, DtFila.id)];
        DtGridTodos.row.add(f).draw();
        DtGridTodos.order([0, 'asc']).draw();
    }
    if (dt == 2) {
        f = [DtFila.id, DtFila.nombre, GetEstado(1, DtFila.id)];
        DtGridSelec.row.add(f).draw();
        DtGridSelec.order([0, 'asc']).draw();
    }
}

function FormulaSave(lista) {
    $.ajax({
        type: 'POST',
        contentType: 'application/json',
        url: controller + "FormulaSave",
        data: JSON.stringify({
            idPunto: punto,
            listaFormulas: lista
        }),
        dataType: 'json',
        traditional: true,
        success: function (result) {
            alert(result);
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function FormulaUpdateList(opcion) {
    $.ajax({
        type: 'POST',
        url: controller + "FormulaUpdateList",
        data: {
            idArea: $('#cboArea').val(),
            idEmpresa: $('#cboEmpresa').val(),
            idTipo: tipoEmpresa
        },
        success: function (result) {
            if (opcion == 1) {
                $('#cboEmpresa').empty();
                var selector = document.getElementById('cboEmpresa');
                selector.options[selector.options.length] = new Option('-Seleccione-', 0);
                selector.options[0].setAttribute('selected', 'selected');
                if (result.ListaEmpresa.length != 0) {

                    for (var i = 0; i < result.ListaEmpresa.length; ++i) {
                        selector.options[selector.options.length] = new Option(result.ListaEmpresa[i].Emprnomb, result.ListaEmpresa[i].Emprcodi);
                    }
                }
                $('#cboEmpresa').multipleSelect('refresh');
            }
            if (opcion == 1 || opcion == 2) {
                $('#cboPunto').empty();
                var selector = document.getElementById('cboPunto');
                selector.options[selector.options.length] = new Option('-Seleccione-', 0);
                selector.options[0].setAttribute('selected', 'selected');
                if (result.ListaPtomedicion.length != 0) {

                    for (var i = 0; i < result.ListaPtomedicion.length; ++i) {
                        selector.options[selector.options.length] = new Option(
                            result.ListaPtomedicion[i].Famnomb + '-' + result.ListaPtomedicion[i].Equinomb + ' (' + result.ListaPtomedicion[i].Ptomedicodi + ')',
                            result.ListaPtomedicion[i].Ptomedicodi);
                    }
                }
                $('#cboPunto').multipleSelect('refresh');
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function FormulaUpdateListUL(opcion) {
    $.ajax({
        type: 'POST',
        url: controller + "FormulaUpdateList",
        data: {
            idArea: $('#cboAreaUL').val(),
            idEmpresa: $('#cboEmpresaUL').val(),
            idTipo: tipoEmpresa
        },
        success: function (result) {
            if (opcion == 1) {
                $('#cboEmpresaUL').empty();
                var selector = document.getElementById('cboEmpresaUL');
                selector.options[selector.options.length] = new Option('-Seleccione-', 0);
                selector.options[0].setAttribute('selected', 'selected');
                if (result.ListaEmpresa.length != 0) {

                    for (var i = 0; i < result.ListaEmpresa.length; ++i) {
                        selector.options[selector.options.length] = new Option(result.ListaEmpresa[i].Emprnomb, result.ListaEmpresa[i].Emprcodi);
                    }
                }
                $('#cboEmpresaUL').multipleSelect('refresh');
            }
            if (opcion == 1 || opcion == 2) {
                $('#cboAgrupacion').empty();
                var selector = document.getElementById('cboAgrupacion');
                selector.options[selector.options.length] = new Option('-Seleccione-', 0);
                selector.options[0].setAttribute('selected', 'selected');
                if (result.ListaAgrupacion.length != 0) {

                    for (var i = 0; i < result.ListaAgrupacion.length; ++i) {
                        selector.options[selector.options.length] = new Option(
                            result.ListaAgrupacion[i].Ptomedidesc + '-' + ' (' + result.ListaAgrupacion[i].Ptomedicodi + ')',
                            result.ListaAgrupacion[i].Ptomedicodi);
                    }
                }
                $('#cboAgrupacion').multipleSelect('refresh');
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function ReiniciarListas(tipo) {
    if (tipo == 1) {
        var selCbo = document.getElementById('cboPunto');
        selCbo.selectedIndex = 0;
        $('#cboPunto').multipleSelect('refresh');
    }
    else {
        var selCbo = document.getElementById('cboAgrupacion');
        selCbo.selectedIndex = 0;
        $('#cboAgrupacion').multipleSelect('refresh');
    }

    DtGridSelec.clear().draw();
    DtGridTodos.clear().draw();
    $('#txtPtomedicion').val('');
}

function GetDtSeleccionados(tabla) {
    var dt = [];
    for (var i = 0; i < tabla.length; i++) {
        var t = [tabla[i].Ptomedicodicalc, tabla[i].Ptomedidesc, GetEstado(tabla[i].Prnselect, tabla[i].Ptomedicodicalc)];
        dt.push(t);
    }

    return dt;
}

function GetDtTodos(tabla) {
    var dt = [];
    for (var i = 0; i < tabla.length; i++) {
        var t = [tabla[i].Ptomedicodicalc, tabla[i].Ptomedidesc, GetEstado(tabla[i].Prnselect, tabla[i].Ptomedicodicalc)];
        dt.push(t);
    }

    return dt;
}

function GetEstado(parametro, codigo) {
    var c = '<input type="checkbox" class="chkEstado" ';
    c += 'value="' + codigo.toString() + '" ';
    if (parametro == 1) {
        c += 'checked />';
    }
    if (parametro == -1) {
        c += '/>';
    }

    return c;
}

function GetDataToSave() {
    var d = [];

    DtGridSelec.rows().eq(0).each(function (index) {
        var x = DtGridSelec.row(index).data();
        var r = [x[0], $('#fct_' + x[0]).val()];
        d.push(r);
    });

    return d;
}