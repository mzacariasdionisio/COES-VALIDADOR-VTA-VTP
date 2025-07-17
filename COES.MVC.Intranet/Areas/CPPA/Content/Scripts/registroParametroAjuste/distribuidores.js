var rutaImagenes = siteRoot + "Content/Images/";
var controller = siteRoot + "CPPA/RegistroParametrosAjuste/";
var dtEmpresasDistribuidoras
var popDistribuidora

$(document).ready(function () {
    //Aplica la lib. MultiSelect
    $('#cboPopEmpresaDistribuidora').multipleSelect({
        single: true,
        filter: true
    });

    $('#btnGenNuevoDistribuidora').on('click', function () {
        popDistribuidora = $('#popupNuevaEmpresaDistribuidora').bPopup({
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
                        tipo: 'D',
                        revision: $('#indexIdRevision').val()
                    }),
                    datatype: 'json',
                    traditional: true,
                    success: function (result) {
                        RefillDropDownListMultiple($('#cboPopEmpresaDistribuidora'), result.ListEmpresasDistribucion, 'Emprcodi', 'EmprnombConcatenado');
                    },
                    error: function () {
                        alert("Ha ocurrido un problema...");
                    }
                });
            }
        )
    });

    $('#btnPopCancelarDistribuidora').on('click', function () {
        $("#popupNuevaEmpresaDistribuidora").bPopup().close();
    });

    $('#btnPopGuardarDistribuidora').on('click', function () {
        registraEmpresaDistribuidora();
    });

    $('#btnGenConsultarDistribuidora').on('click', function () {
        consultaEmpresasDistribuidoras();
    });

    $('#btnGenExportarDistribuidora').on('click', function () {
        var myTable = $("#dtListaEmpresasDistribuidoras").DataTable();
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
        exportarEmpresasDistribuidoras(datos);
    });

    consultaEmpresasDistribuidoras();
});

function exportarEmpresasDistribuidoras(datos) {
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
            tipo: "DISTRIBUIDORAS"
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

function consultaEmpresasDistribuidoras() {

    const revision = $('#indexIdRevision').val();
    const estado = $('input[name="genEstadoDistribuidor"]:checked').val();

    $.ajax({
        type: 'POST',
        url: controller + 'ListaEmpresasIntegrantes',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            revision: revision,
            estado: estado,
            tipo: 2
        }),
        datatype: 'json',
        success: function (result) {
            dtEmpresasDistribuidoras = $('#dtListaEmpresasDistribuidoras').DataTable({
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

$(document).off('click', '#dtListaEmpresasDistribuidoras tr td .clsEliminar:not(.disabled)')
    .on('click', '#dtListaEmpresasDistribuidoras tr td .clsEliminar:not(.disabled)', function (e) {
        e.stopPropagation();

        var data = dtEmpresasDistribuidoras.row($(this).parents('tr')).data();
        var confirmacion = confirm('¿Estás seguro de que deseas anular: ' + data.Emprnomb + '?');

        if (confirmacion) {
            anulaEmpresaDistribuidora(data.Cpaempcodi, data.Emprcodi, data.Cparcodi);
        }

        return false;
    });


function registraEmpresaDistribuidora() {
    const revision = $('#indexIdRevision').val();
    const empresa = $('#cboPopEmpresaDistribuidora').val();

    $.ajax({
        type: 'POST',
        url: controller + 'RegistraEmpresaIntegrante',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            revision: revision,
            empresa: empresa,
            tipo: 2
        }),
        datatype: 'json',
        success: function (result) {
            $("#popupNuevaEmpresaDistribuidora").bPopup().close();
            if (result.sResultado === '1') {
                SetMessage('#message',
                    result.sMensaje,
                    'success', true);
                consultaEmpresasDistribuidoras();
            }
            else if (result.sResultado === '2') {
                SetMessage('#message',
                    result.sMensaje,
                    'warning', true);
                consultaEmpresasDistribuidoras();
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

function anulaEmpresaDistribuidora(codigo, empresa, revision) {
    console.log(codigo, 'dsdsdsds');
    $.ajax({
        type: 'POST',
        url: controller + 'AnulaEmpresaIntegrante',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            codigo: codigo,
            empresa: empresa,
            revision: revision,
            tipo: 'D'
        }),
        datatype: 'json',
        success: function (result) {
            consultaEmpresasDistribuidoras();
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

