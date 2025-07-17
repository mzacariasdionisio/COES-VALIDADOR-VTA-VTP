var rutaImagenes = siteRoot + "Content/Images/";
var controller = siteRoot + "CPPA/RegistroParametrosAjuste/";
var dtEmpresasGeneradoras
var popNuevaEmpGen
var id

$(document).ready(function () {

    //Aplica la lib. MultiSelect
    $('#cboPopEmpresaGeneradora').multipleSelect({
        single: true,
        filter: true
    });

    $('#btnGenNuevo').on('click', function () {
        popNuevaEmpGen = $('#popupNuevaEmpresa').bPopup({
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
                        tipo: 'G',
                        revision: $('#indexIdRevision').val()
                    }),
                    datatype: 'json',
                    traditional: true,
                    success: function (result) {
                        RefillDropDownListMultiple($('#cboPopEmpresaGeneradora'), result.ListEmpresasGeneracion, 'Emprcodi', 'EmprnombConcatenado');
                    },
                    error: function () {
                        alert("Ha ocurrido un problema...");
                    }
                });
            }
        )
    });

    $('#btnPopCancelar').on('click', function () {
        $("#popupNuevaEmpresa").bPopup().close();
    });

    $('#btnPopGuardar').on('click', function () {
        registraEmpresaGeneradora();
    });

    $('#btnGenConsultar').on('click', function () {
        consultaEmpresasGeneradoras();
    });

    $('#btnGenExportar').on('click', function () {
        var myTable = $("#dtListaEmpresasGeneradoras").DataTable();
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
        exportarEmpresasGeneradoras(datos);
    });

    consultaEmpresasGeneradoras();
});

function exportarEmpresasGeneradoras(datos) {
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
            tipo: "GENERADORAS"
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            console.log(result);
            if (result != -1) {
                console.log('entrooo');
                window.location = controller + 'abrirarchivo?formato=' + 1 + '&file=' + result;
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function consultaEmpresasGeneradoras() {

    const revision = $('#indexIdRevision').val();
    const estado = $('input[name="genEstado"]:checked').val();

    $.ajax({
        type: 'POST',
        url: controller + 'ListaEmpresasIntegrantes',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            revision: revision,
            estado: estado,
            tipo: 1
        }),
        datatype: 'json',
        success: function (result) {
            dtEmpresasGeneradoras = $('#dtListaEmpresasGeneradoras').DataTable({
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
                    {
                        title: 'Estado', data: 'Cpaempestado' },
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
                            var editarBtn = `<img class="clsEditar ${disabledClass}" src="${rutaImagenes}btn-edit.png" style="cursor: ${disabledClass ? 'not-allowed' : 'pointer'};" title="Centrales de generador"/>`;

                            return editarBtn + ' ' + eliminarBtn;
                        }
                    }
                ],
                createdRow: function (row, data, index) {

                },
                bLengthChange: false,
                bSort: false,
                destroy: true,
                paging: false,
                info: false,
                searching: true,
                scrollY: '250px',
            });
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });

}

$(document).on('click', '#dtListaEmpresasGeneradoras tr td .clsEditar:not(.disabled)', function () {
    var data = dtEmpresasGeneradoras.row($(this).parents('tr')).data();
    consultaCentrales(data.Cpaempcodi, data.Emprnomb)
});

$(document).off('click', '#dtListaEmpresasGeneradoras tr td .clsEliminar:not(.disabled)')
    .on('click', '#dtListaEmpresasGeneradoras tr td .clsEliminar:not(.disabled)', function (e) {
        e.stopPropagation();

        var data = dtEmpresasGeneradoras.row($(this).parents('tr')).data();
        var confirmacion = confirm('¿Estás seguro de que deseas anular: ' + data.Emprnomb + '?');

        if (confirmacion) {
            anulaEmpresaGeneradora(data.Cpaempcodi, data.Emprcodi, data.Cparcodi);
        }

        return false;
    });


function consultaCentrales(cpaEmpresa, emprnomb) {
    const idRevision = $('#indexIdRevision').val();
    const revision = $('#indexRevision').val();
    const ajuste = $('#indexAjuste').val();
    const anio = $('#indexFecha').val();

    var url = siteRoot + `CPPA/RegistroParametrosAjuste/ListaCentrales/?fecha=${anio}&ajuste=${ajuste}&revision=${revision}&idRevision=${idRevision}&cpaEmpresa=${cpaEmpresa}&nombEmpresa=${emprnomb}`;
    window.location.href = url
}

function registraEmpresaGeneradora() {
    const revision = $('#indexIdRevision').val();
    const empresa = $('#cboPopEmpresaGeneradora').val();

    $.ajax({
        type: 'POST',
        url: controller + 'RegistraEmpresaIntegrante',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            revision: revision,
            empresa: empresa,
            tipo: 1
        }),
        datatype: 'json',
        success: function (result) {
            $("#popupNuevaEmpresa").bPopup().close();
            if (result.sResultado === '1') {
                SetMessage('#message',
                    result.sMensaje,
                    'success', true);
                consultaEmpresasGeneradoras();
            }
            else if (result.sResultado === '2') {
                SetMessage('#message',
                    result.sMensaje,
                    'warning', true);
                consultaEmpresasGeneradoras();
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

function anulaEmpresaGeneradora(codigo, empresa, revision) {

    $.ajax({
        type: 'POST',
        url: controller + 'AnulaEmpresaIntegrante',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            codigo: codigo,
            empresa: empresa,
            revision: revision,
            tipo: 'G'
        }),
        datatype: 'json',
        success: function (result) {
            //consultaEmpresasGeneradoras();
            if (result.sResultado != -1) {
                SetMessage('#message',
                    result.sMensaje,
                    result.sTipo, true);
                consultaEmpresasGeneradoras();
            } else {
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

function RefillDropDownListMultiple(element, data, data_id, data_name) {
    //Vacia el elemento
    element.empty();
    //Carga el elemento
    $.each(data, function (i, item) {
        var n_value = i, n_html = item;
        if (data_id) n_value = item[data_id];
        if (data_name) n_html = item[data_name];
        element.append($('<option></option>').val(n_value).html(n_html));
        if (i === 0) {
            primerElementoValor = n_value;
        }
    });

    element.multipleSelect('refresh');
    if (primerElementoValor !== null) {
        element.multipleSelect('setSelects', [primerElementoValor]);
    }
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
}