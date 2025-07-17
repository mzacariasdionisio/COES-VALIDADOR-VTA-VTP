var controlador = siteRoot + "Transferencias/" + "Concepto/";
var rutaImagenes = siteRoot + "Content/Images/";
var tablaConcepto;
var tablaDetalle;

$(document).ready(function () {

    $('#btnNuevo').on('click', function () {
        Nuevo();
    });

    $('#btnGrabar').on('click', function () {
        Grabar();
    });

    $('#btnCancelar').on('click', function () {
        $("#popNuevo").bPopup().close();
    });

    Detalle();

});

$(document).on('click', '#tbConceptos tr td .clsVersion', function (e) {
    var row = $(this).closest('tr');
    var r = tablaConcepto.row(row).data();
    DetConcepto(r.Infadicodi);
});

$(document).on('click', '#tbConceptos tr td .clsEditar', function (e) {
    var row = $(this).closest('tr');
    var r = tablaConcepto.row(row).data();
    Edit(r.Infadicodi);
});

$(document).on('click', '#tbConceptos tr td .clsEliminar', function (e) {
    var row = $(this).closest('tr');
    var r = tablaConcepto.row(row).data();
    Delete(r.Infadicodi);
});

function clearNuevo() {
    $('#Infadicodi').val(0);
    $('#txtConcepto').val("");
    $('#txtCodOsinergmin').val("");
}

function Detalle() {
    $.ajax({
        type: 'POST',
        url: controlador + "detalle",
        dataType: 'json',
        traditional: true,
        success: function (result) {
            tablaConcepto = $('#tbConceptos').DataTable({
                data: result,
                columns: [
                    { data: null, title: ''},
                    { data: 'Infadicodi', title: 'Id' },
                    { data: 'Infadinomb', title: 'Concepto' },
                    { data: 'Infadicodosinergmin', title: 'Cod. Osinergmin' }, 
                    { data: 'Tipoemprcodi', title: '' },
                    { data: 'Tipoemprdesc', title: 'Tipo de Empresa' },
                    { data: 'Emprcodi', title: '' },
                    { data: 'Emprnomb', title: 'Empresa Relacionada' },
                    { data: 'Fechacortedesc', title: 'Fecha Vigencia' }
                    
                ],
                columnDefs:
                    [
                       { targets: [4, 6], visible: false, searchable: false, sorting: false },
                       {
                           targets: [0], width: 75,
                           className: 'dt-body-justify',
                           defaultContent: '<img class="clsVersion" src="' + rutaImagenes + 'btn-reporte.png" style="cursor: pointer;" title="Versiones" /> ' +
                                           '<img class="clsEditar" src="' + rutaImagenes + 'btn-edit.png" style="cursor: pointer;" title="Editar" /> ' +
                                           '<img class="clsEliminar" src="' + rutaImagenes + 'btn-cancel.png" style="cursor: pointer;" title="Eliminar"/> ' 
                       }
                    ],
                searching: true,
                bLengthChange: false,
                bSort: false,
                destroy: true,
                paging: true,
                info: false
            });
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });

}

function Nuevo() {
    clearNuevo();
    $.ajax({
        type: 'POST',
        url: controlador + "nuevo",
        dataType: 'json',
        traditional: true,
        success: function (result) {
            $('#txtConcepto').empty();
            $('#txtCodOsinergmin').empty();
            $('#cboEmpresa').empty();
            var selector = document.getElementById('cboEmpresa');
            if (result.ListaEmpresa.length != 0) {
                for (var i = 0; i < result.ListaEmpresa.length; ++i) {
                    selector.options[selector.options.length] = new Option(result.ListaEmpresa[i].EmprNombre, result.ListaEmpresa[i].EmprCodi);
                }
            }

            $('#cboTipoEmpresa').empty();
            var selector = document.getElementById('cboTipoEmpresa');
            if (result.ListaTipoEmpresa.length != 0) {
                for (var i = 0; i < result.ListaTipoEmpresa.length; ++i) {
                    selector.options[selector.options.length] = new Option(result.ListaTipoEmpresa[i].TipoEmprDesc, result.ListaTipoEmpresa[i].TipoEmprCodi);
                }
            }
            setTimeout(function () {
                $('#popNuevo').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'

                });
            }, 100);

        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function DetConcepto(id) {
    clearNuevo();
    $.ajax({
        type: 'POST',
        url: controlador + "detalleConcepto",
        data: {
            infadicodi: id
        },
        dataType: 'json',
        traditional: true,
        success: function (result) {
            tablaDetalle = $('#tbDetalleVersiones').DataTable({
                data: result,
                columns: [
                    { data: 'Infadicodi', title: 'Id' },
                    { data: 'Infadinomb', title: 'Concepto' },
                    { data: 'Infadicodosinergmin', title: 'Cod. Osinergmin' }, 
                    { data: 'Tipoemprcodi', title: '' },
                    { data: 'Tipoemprdesc', title: 'Tipo de Empresa' },
                    { data: 'Emprcodi', title: '' },
                    { data: 'Emprnomb', title: 'Empresa Relacionada' },
                    { data: 'Fechacortedesc', title: 'Fecha Vigencia' },
                    { data: 'UsuCreacion', title: 'Registrado por' }

                ],
                columnDefs:
                    [
                        { targets: [3, 5], visible: false, searchable: false, sorting: false },
                    ],
                searching: false,
                bLengthChange: false,
                bSort: false,
                destroy: true,
                paging: false,
                info: false
            });
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function Edit(id) {
    clearNuevo();
    $.ajax({
        type: 'POST',
        url: controlador + "edit",
        data: {
            infadicodi: id
        },
        dataType: 'json',
        traditional: true,
        success: function (result) {
            $('#Infadicodi').val(result.EntidadConcepto.Infadicodi);
            $('#txtConcepto').val(result.EntidadConcepto.Infadinomb);
            $('#txtCodOsinergmin').val(result.EntidadConcepto.Infadicodosinergmin);

            $('#cboEmpresa').empty();
            var selector = document.getElementById('cboEmpresa');
            if (result.ListaEmpresa.length != 0) {
                for (var i = 0; i < result.ListaEmpresa.length; ++i) {
                    var bSelected = false;
                    if (result.ListaEmpresa[i].EmprCodi == result.EntidadConcepto.Emprcodi)
                        bSelected = true;
                    selector.options[selector.options.length] = new Option(result.ListaEmpresa[i].EmprNombre, result.ListaEmpresa[i].EmprCodi, bSelected, bSelected);
                }
            }

            $('#cboTipoEmpresa').empty();
            var selector = document.getElementById('cboTipoEmpresa');
            if (result.ListaTipoEmpresa.length != 0) {
                for (var i = 0; i < result.ListaTipoEmpresa.length; ++i) {
                    var bSelected = false;
                    if (result.ListaTipoEmpresa[i].TipoEmprCodi == result.EntidadConcepto.Tipoemprcodi)
                        bSelected = true;
                    selector.options[selector.options.length] = new Option(result.ListaTipoEmpresa[i].TipoEmprDesc, result.ListaTipoEmpresa[i].TipoEmprCodi, bSelected, bSelected);
                }
            }
            setTimeout(function () {
                $('#popNuevo').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'

                });
            }, 100);

        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function Grabar() {
    $.ajax({
        type: 'POST',
        url: controlador + "Grabar",
        data: {
            infadicodi: $('#Infadicodi').val(),
            infadinomb: $('#txtConcepto').val(),
            Infadicodosinergmin: $('#txtCodOsinergmin').val(),
            tipoemprcodi: $('#cboTipoEmpresa').val(),
            emprcodi: $('#cboEmpresa').val()
            
        },
        dataType: 'json',
        traditional: true,
        success: function (result) {
            $("#popNuevo").bPopup().close();
            Detalle();
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function Delete(id) {
    $.ajax({
        type: 'POST',
        url: controlador + "Delete",
        data: {
            infadicodi: id
        },
        dataType: 'json',
        traditional: true,
        success: function (result) {
            alert("Registro Eliminado");
            Detalle();
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}