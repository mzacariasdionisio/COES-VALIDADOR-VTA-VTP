var rutaImagenes = siteRoot + "Content/Images/";
var controller = siteRoot + "CPPA/RegistroParametrosAjuste/";
var dtCentrales
var popNuevaCentral

$(document).ready(function () {
    $('#cboPopCentral').multipleSelect({
        single: true,
        filter: true
    });

    $('#btnCenNuevo').on('click', function () {
        var labelTitleNuevaCentral = document.getElementById('popupTitleNuevaCentral');
        if (labelTitleNuevaCentral) {
            const empresa = $('#indexEmpresa').val();
            labelTitleNuevaCentral.textContent = "Nueva Central de empresa - " + empresa;
        }

        popNuevaCentral = $('#popupNuevaCentral').bPopup({
            easing: 'easeOutBack',
            speed: 350,
            transition: 'fadeIn',
            modalClose: false,
        },
            function () {
                $.ajax({
                    type: 'POST',
                    url: controller + 'CentralesDisponibles',
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify({
                        revision: $('#indexIdRevision').val(),
                        empresa: $('#indexIdEmpresa').val()
                    }),
                    datatype: 'json',
                    traditional: true,
                    success: function (result) {
                        RefillDropDownListMultiple($('#cboPopCentral'), result.ListCentralesGeneradoras, 'Equicodi', 'EquinombConcatenado');
                    },
                    error: function () {
                        alert("Ha ocurrido un problema...");
                    }
                });
            }
        )
    });

    $('#btnCenConsultar').on('click', function () {
        listaCentralesIntegrantes();
    });

    $('#btnCenRegresar').on('click', function () {
        const idRevision = $('#indexIdRevision').val();
        const revision = $('#indexRevision').val();
        const ajuste = $('#indexAjuste').val();
        const anio = $('#indexFecha').val();

        var url = siteRoot + `CPPA/RegistroParametrosAjuste/index/?fecha=${anio}&ajuste=${ajuste}&revision=${revision}&idRevision=${idRevision}`;
        console.log(url, 'url');
        window.location.href = url
    });

    $('#btnPopCancelarCentral').on('click', function () {
        $("#popupNuevaCentral").bPopup().close();
    });

    $('#btnPopGuardarCentral').on('click', function () {
        registraCentralGeneradora();
    });

    $('#btnCenExportar').on('click', function () {
        var myTable = $("#dtCentrales").DataTable();
        var form_data = myTable.rows().data();
        var datos = [];
        for (var i = 0; i < form_data.length; i++) {
            var fila = [];
            fila.push(form_data[i].Equinomb);
            fila.push(form_data[i].Cpacntestado);
            fila.push(form_data[i].Cpacntusucreacion);
            var fechaCreacion = form_data[i].Cpacntfeccreacion;
            fila.push(fechaCreacion ? formatDate(fechaCreacion) : null);
            fila.push(form_data[i].Cpacntusumodificacion);
            var fechaModificacion = form_data[i].Cpaempfecmodificacion;
            fila.push(fechaModificacion ? formatDate(Cpacntfecmodificacion) : null);
            datos.push(fila);
        }
        exportarCentrales(datos);
    });

    listaCentralesIntegrantes();
});

function exportarCentrales(datos) {
    const revision = $('#indexRevision').val();
    const ajuste = $('#indexAjuste').val();
    const anio = $('#indexFecha').val();
    const empresa = $('#indexEmpresa').val();

    $.ajax({
        type: 'POST',
        url: controller + 'ExportarCentrales',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            form: datos,
            revision: revision,
            ajuste: ajuste,
            anio: anio,
            empresa: empresa
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result != -1) {
                window.location = controller + 'abrirarchivo?formato=' + 1 + '&file=' + result;
                //mostrarMensaje('mensaje', 'exito', "Felicidades, el archivo se descargo correctamente...!");
            }
            else {
                //mostrarMensaje('mensaje', 'error', "Lo sentimos, ha ocurrido un error inesperado");
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function listaCentralesIntegrantes() {
    console.log(1);
    const idRevision = $('#indexIdRevision').val();
    const idEmpresa = $('#indexIdEmpresa').val();
    const estado = $('input[name="cenEstado"]:checked').val();
    console.log(controller, 'controlador');
    $.ajax({
        type: 'POST',
        url: controller + 'ListaCentralesIntegrantes',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            empresa: idEmpresa,
            revision: idRevision,
            estado: estado
        }),
        datatype: 'json',
        success: function (result) {

            var labelAjuste = document.getElementById('descripcionAjuste');
            var labelEmpresa = document.getElementById('descripcionEmpresa');

            const revision = $('#indexRevision').val();
            const anio = $('#indexFecha').val();
            const ajuste = $('#indexAjuste').val();
            const empresa = $('#indexEmpresa').val();

            if (labelAjuste) {
                labelAjuste.textContent = "Ajuste " + anio + "-" + ajuste + "-" + revision;
            }
            if (labelEmpresa) {
                labelEmpresa.textContent = "Listado de centrales de generacion - " + empresa;
            }

            dtCentrales = $('#dtCentrales').DataTable({
                data: result.ListCentralesIntegrantes,
                columns: [
                    {
                        title: 'Acciones',
                        data: null,
                        //render: function (data, type, row) {
                        //    // Verifica si el estado es "anulado"
                        //    if (row.Cpacntestado === 'Anulado') {
                        //        return '<img class="clsEliminar disabled" src="' + rutaImagenes + 'btn-cancel.png" style="cursor: not-allowed;" title="Anulado"/>';
                        //    }
                        //    return '<img class="clsEliminar" src="' + rutaImagenes + 'btn-cancel.png" style="cursor: pointer;" title="Anular" />';
                        //}
                    },
                    { title: 'Cpacntcodi', data: 'Cpacntcodi', visible: false },
                    { title: 'Cpaempcodi', data: 'Cpaempcodi', visible: false },
                    { title: 'Cparcodi', data: 'Cparcodi', visible: false },
                    { title: 'Equicodi', data: 'Equicodi', visible: false },
                    { title: 'Central', data: 'Equinomb' },
                    { title: 'Estado', data: 'Cpacntestado' },
                    { title: 'Usuario creacion', data: 'Cpacntusucreacion' },
                    {
                        title: 'Fecha creacion',
                        data: 'Cpacntfeccreacion',
                        render: function (data) {
                            return formatDate(data);
                        }
                    },
                    { title: 'Usuario actualizacion', data: 'Cpacntusumodificacion' },
                    {
                        title: 'Fecha actualizacion',
                        data: 'Cpacntfecmodificacion',
                        render: function (data) {
                            if (data) {
                                return formatDate(data);
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
                //    }
                //],
                columnDefs: [
                    {
                        targets: [0],
                        width: 100,
                        render: function (data, type, row) {
                            if (row.Cpacntestado === 'Anulado') {
                                return '';
                            }

                            var disabledClass = row.Cpacntestado === 'Anulado' ? 'disabled' : '';
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
                scrollY: '380px',
            });
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });

}

$(document).on('click', '#dtCentrales tr td .clsEliminar:not(.disabled)', function (e) {

    var row = $(this).closest('tr');
    var data = dtCentrales.row(row).data();

    if (confirm('¿Estás seguro de que deseas realizar esta acción en: ' + data.Equinomb + '?')) {
        anulaCentralGeneradora(data.Cpacntcodi, data.Cpaempcodi);
    }
});

function registraCentralGeneradora() {
    const revision = $('#indexIdRevision').val();
    const ajuste = $('#indexAjuste').val();
    const anio = $('#indexFecha').val();
    const central = $('#cboPopCentral').val();
    const idEmpresa = $('#indexIdEmpresa').val();

    $.ajax({
        type: 'POST',
        url: controller + 'RegistraCentralIntegrante',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            anio: anio,
            empresa: idEmpresa,
            revision: revision,
            central: central,
            ajuste: ajuste
        }),
        datatype: 'json',
        success: function (result) {
            $("#popupNuevaCentral").bPopup().close();
            if (result.sResultado === '1') {
                SetMessage('#message',
                    result.sMensaje,
                    'success', true);
                listaCentralesIntegrantes();
            }
            else if (result.sResultado === '2') {
                SetMessage('#message',
                    result.sMensaje,
                    'warning', true);
                listaCentralesIntegrantes();
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

function anulaCentralGeneradora(central, empresa) {

    $.ajax({
        type: 'POST',
        url: controller + 'AnulaCentralIntegrante',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            central: central,
            empresa: empresa
        }),
        datatype: 'json',
        success: function (result) {
            listaCentralesIntegrantes();
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function formatDate(dateString) {
    // Extrae el número de milisegundos
    const milliseconds = parseInt(dateString.replace(/\/Date\((\d+)\)\//, '$1'));
    const date = new Date(milliseconds);

    // Formato dd/mm/yyyy
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0'); // Meses son 0-indexados
    const year = date.getFullYear();

    return `${day}/${month}/${year}`;
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

//Configura el mensaje principal
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