var rutaImagenes = siteRoot + "Content/Images/";
var controller = siteRoot + "CPPA/RegistroParametrosAjuste/";
var dtEmpresasLibres
var popLibre

$(document).ready(function () {
    $('#cboPopEmpresaLibre').multipleSelect({
        single: true,
        filter: true
    });

    $('#btnGenNuevoLibre').on('click', function () {
        popLibre = $('#popupNuevaEmpresaLibre').bPopup({
            easing: 'easeOutBack',
            speed: 350,
            transition: 'fadeIn',
            modalClose: false
        },
            function () {
                $.ajax({
                    type: 'POST',
                    url: controller + 'EmpresasDisponibles',
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify({
                        tipo: 'U',
                        revision: $('#indexIdRevision').val()
                    }),
                    datatype: 'json',
                    traditional: true,
                    success: function (result) {
                        RefillDropDownListMultiple($('#cboPopEmpresaLibre'), result.ListEmpresasLibres, 'Emprcodi', 'EmprnombConcatenado');
                    },
                    error: function () {
                        alert("Ha ocurrido un problema...");
                    }
                });
            }
        )
    });

    $('#btnPopCancelarLibre').on('click', function () {
        $("#popupNuevaEmpresaLibre").bPopup().close();
    });

    $('#btnPopGuardarLibre').on('click', function () {
        registraEmpresaLibre();
    });

    $('#btnGenConsultarLibre').on('click', function () {
        consultaEmpresasLibres();
    });

    $('#btnGenExportarLibre').on('click', function () {
        var myTable = $("#dtListaEmpresasLibres").DataTable();
        var form_data = myTable.rows().data();
        var datos = [];
        for (var i = 0; i < form_data.length; i++) {
            var fila = [];
            fila.push(form_data[i].Emprnomb);
            fila.push(form_data[i].Cpaempestado);
            fila.push(form_data[i].Cpaempusucreacion);
            var fechaCreacion = form_data[i].Cpaempfeccreacion;
            fila.push(fechaCreacion ? formatDate(fechaCreacion) : null);
            fila.push(form_data[i].Cpaempusumodificacion);
            var fechaModificacion = form_data[i].Cpaempfecmodificacion;
            fila.push(fechaModificacion ? formatDate(fechaModificacion) : null);
            datos.push(fila);
        }
        exportarEmpresasLibres(datos);
    });

    consultaEmpresasLibres();
});

function exportarEmpresasLibres(datos) {
    const revision = $('#indexRevision').val();
    const ajuste = $('#indexAjuste').val();
    const anio = $('#indexFecha').val();

    $.ajax({
        type: 'POST',
        url: controller + 'ExportarEmpresas',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            form: datos,
            revision: revision,
            ajuste: ajuste,
            anio: anio,
            tipo: "USUARIOS LIBRES"
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result != -1) {
                window.location = controller + 'abrirarchivo?formato=' + 1 + '&file=' + result;
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function consultaEmpresasLibres() {

    const revision = $('#indexIdRevision').val();
    const estado = $('input[name="genEstadoLibre"]:checked').val();

    $.ajax({
        type: 'POST',
        url: controller + 'ListaEmpresasIntegrantes',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            revision: revision,
            estado: estado,
            tipo: 3
        }),
        datatype: 'json',
        success: function (result) {
            dtEmpresasLibres = $('#dtListaEmpresasLibres').DataTable({
                data: result.ListEmpresasIntegrantes,
                columns: [
                    {
                        title: 'Acciones',
                        data: null,
                        defaultContent: ''
                    },
                    { title: 'Cpaempcodi', data: 'Cpaempcodi', visible: false },
                    { title: 'Cparcodi', data: 'Cparcodi', visible: false },
                    { title: 'Emprcodi', data: 'Emprcodi', visible: false },
                    {
                        title: 'Empresa Generadora',
                        data: 'Emprnomb',
                        render: function (data) {
                            return '<div style="text-align: left; padding-left: 10px;">' + data + '</div>';
                        }
                    },
                    { title: 'Tipo', data: 'Cpaemptipo', visible: false },
                    { title: 'Estado', data: 'Cpaempestado' },
                    {
                        title: 'Usuario creación',
                        data: 'Cpaempusucreacion',
                        render: function (data) {
                            return '<div style="text-align: left; padding-left: 10px;">' + data + '</div>';
                        }
                    },
                    {
                        title: 'Fecha creación',
                        data: 'Cpaempfeccreacion',
                        render: function (data) {
                            return formatDateComplete(data);
                        }
                    },
                    {
                        title: 'Usuario actualización',
                        data: 'Cpaempusumodificacion',
                        render: function (data) {
                            if (data) {
                                return '<div style="text-align: left; padding-left: 10px;">' + data + '</div>';
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        title: 'Fecha actualización',
                        data: 'Cpaempfecmodificacion',
                        render: function (data) {
                            if (data) {
                                return formatDateComplete(data);
                            } else {
                                return data
                            }
                        }
                    }
                ],
                //columnDefs: [
                //    {
                //        targets: [0],
                //        width: 100,
                //        render: function (data, type, row) {
                //            var disabledClass = row.Cpaempestado === 'Anulado' ? 'disabled' : '';
                //            return `<img class="clsEliminar ${disabledClass}" src="${rutaImagenes}btn-cancel.png" style="cursor: ${disabledClass ? 'not-allowed' : 'pointer'};" title="Anular"/>`;
                //        }
                //    }
                //],
                columnDefs: [
                    {
                        targets: [0],
                        width: 100,
                        render: function (data, type, row) {
                            if (row.Cpaempestado === 'Anulado') {
                                return '';
                            }

                            var disabledClass = row.Cpaempestado === 'Anulado' ? 'disabled' : '';
                            var eliminarBtn = `<img class="clsEliminar ${disabledClass}" src="${rutaImagenes}btn-cancel.png" style="cursor: ${disabledClass ? 'not-allowed' : 'pointer'};" title="Anular"/>`;
                            return eliminarBtn;
                        }
                    }
                ],
                createdRow: function (row, data, index) {

                },
                searching: true,
                bLengthChange: false,
                bSort: false,
                destroy: true,
                paging: false,
                info: false,
                scrollY: '250px',
            });
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });

}

$(document).off('click', '#dtListaEmpresasLibres tr td .clsEliminar:not(.disabled)')
    .on('click', '#dtListaEmpresasLibres tr td .clsEliminar:not(.disabled)', function (e) {
        e.stopPropagation();

        var data = dtEmpresasLibres.row($(this).parents('tr')).data();
        var confirmacion = confirm('¿Estás seguro de que deseas anular: ' + data.Emprnomb + '?');

        if (confirmacion) {
            anulaEmpresaLibre(data.Cpaempcodi, data.Emprcodi, data.Cparcodi);
        }

        return false;
    });

function registraEmpresaLibre() {
    const revision = $('#indexIdRevision').val();
    const empresa = $('#cboPopEmpresaLibre').val();

    $.ajax({
        type: 'POST',
        url: controller + 'RegistraEmpresaIntegrante',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            revision: revision,
            empresa: empresa,
            tipo: 3
        }),
        datatype: 'json',
        success: function (result) {
            $("#popupNuevaEmpresaLibre").bPopup().close();
            if (result.sResultado === '1') {
                SetMessage('#message',
                    result.sMensaje,
                    'success', true);
                consultaEmpresasLibres();
            }
            else if (result.sResultado === '2') {
                SetMessage('#message',
                    result.sMensaje,
                    'warning', true);
                consultaEmpresasLibres();
            }
            else {
                SetMessage('#message',
                    result.sMensaje + ' - ' + result.sDetalle,
                    'error', true);
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function anulaEmpresaLibre(codigo, empresa, revision) {

    $.ajax({
        type: 'POST',
        url: controller + 'AnulaEmpresaIntegrante',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            codigo: codigo,
            empresa: empresa,
            revision: revision,
            tipo: 'U'
        }),
        datatype: 'json',
        success: function (result) {
            consultaEmpresasLibres();
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}