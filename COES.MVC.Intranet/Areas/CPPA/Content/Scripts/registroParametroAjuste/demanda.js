var dtMaximaDemandaPreciosPotencia, dtHistorico
var popMaxDemanda, popHistorico
var flag, id;

const meses = [
    "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
    "Julio", "Agosto", "Setiembre", "Octubre", "Noviembre", "Diciembre"
];

$(document).ready(function () {
    $('#txtPopMaxDemandaMes').Zebra_DatePicker({
        format: 'F',
        view: 'month',
        onSelect: function (date) {
            $('#txtPopMaxDemandaFecha').val('');
            var monthNames = ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
                "Julio", "Agosto", "Setiembre", "Octubre", "Noviembre", "Diciembre"];
            var selectedMonth = monthNames.indexOf(date);
            var selectedYear = $('#indexFecha').val() - 1;

            var firstDay = new Date(selectedYear, selectedMonth, 1);
            var lastDay = new Date(selectedYear, selectedMonth + 1, 0);
            var firstDayFormatted = (firstDay.getDate() < 10 ? '0' : '') + firstDay.getDate() + '/' + (firstDay.getMonth() + 1 < 10 ? '0' : '') + (firstDay.getMonth() + 1) + '/' + firstDay.getFullYear();
            var lastDayFormatted = (lastDay.getDate() < 10 ? '0' : '') + lastDay.getDate() + '/' + (lastDay.getMonth() + 1 < 10 ? '0' : '') + (lastDay.getMonth() + 1) + '/' + lastDay.getFullYear();
            $('#txtPopMaxDemandaFecha').Zebra_DatePicker('destroy');
            $('#txtPopMaxDemandaFecha').Zebra_DatePicker({
                format: 'd/m/Y',
                direction: [firstDayFormatted, lastDayFormatted],
            });
        }
    });


    $('#txtPopMaxDemandaHora').timepicker({
        timeFormat: 'HH:mm',
        interval: 15,
        dynamic: false,
        dropdown: true,
        scrollbar: true
    });

    $('#btnDemNuevo').on('click', function () {
        flag = 1;//1 para grabar
        document.getElementById('txtPopMaxDemandaPrecio').style.backgroundColor = '';
        const anio = $('#indexFecha').val() - 1;
        document.getElementById('txtPopMaxDemandaTipoRegistro').value = 'P';
        document.getElementById('txtPopMaxDemandaAnio').value = anio;

        document.getElementById('txtPopMaxDemandaMes').disabled = false;
        document.getElementById('txtPopMaxDemandaFecha').disabled = false;
        document.getElementById('txtPopMaxDemandaHora').disabled = false;
        document.getElementById('txtPopMaxDemandaPrecio').disabled = false;
        document.getElementById('txtPopMaxDemandaTipoRegistro').disabled = true;

        document.getElementById('txtPopMaxDemandaMes').value = '';
        document.getElementById('txtPopMaxDemandaFecha').value = '';
        document.getElementById('txtPopMaxDemandaHora').value = '';
        document.getElementById('txtPopMaxDemandaTipoCambio').value = '';
        document.getElementById('txtPopMaxDemandaPrecio').value = '';

        popMaxDemanda = $('#popupMaxDemanda').bPopup({
            easing: 'easeOutBack',
            speed: 350,
            transition: 'fadeIn',
            modalClose: false,
            zIndex: 1000
        },
        )
    });

    $('#btnPopMaxDemandaGuardar').on('click', function () {
        if (flag === 1) {
            registrarParametros();
        } else {
            editarParametros();
        }
    });

    $('#btnPopMaxDemandaCancelar').on('click', function () {
        $("#popupMaxDemanda").bPopup().close();
    });

    $('#btnDemCopiar').on('click', function () {
        copiarParametros();
    });

    $('#btnDemExportar').on('click', function () {
        var myTable = $("#dtListaMaximaDemandaPreciosPotencia").DataTable();
        var form_data = myTable.rows().data();
        var datos = [];
        for (var i = 0; i < form_data.length; i++) {
            var fila = [];
            let estado = form_data[i].Cpaprmestado;
            fila.push(estado === 'A'? 'Activo' : 'Anulado');
            fila.push(form_data[i].Aniomes);
            fila.push(form_data[i].Cpaprmtipomd);
            var fecha = form_data[i].Cpaprmfechamd;
            fila.push(fecha ? formatDate(fecha) : null);
            fila.push(fecha ? formatTime(fecha) : null);
            fila.push(form_data[i].Cpaprmcambio);
            fila.push(form_data[i].Cpaprmprecio);

            fila.push(form_data[i].Cpaprmusucreacion);
            var fechaCreacion = form_data[i].Cpaprmfeccreacion;
            fila.push(fechaCreacion ? formatDate(fechaCreacion) : null);

            fila.push(form_data[i].Cpaprmusumodificacion);
            var fechaActualizacion = form_data[i].Cpaprmfecmodificacion;
            fila.push(fechaActualizacion ? formatDate(fechaActualizacion) : null);
            datos.push(fila);
        }
        exportarParametros(datos);
    });

    $('#btnDemConsultar').on('click', function () {
        consultaMaximaDemandaPreciosPotencia();
    });

    $('#popbtnHistoricoCancelar').on('click', function () {
        $("#popupHistoricoDemanda").bPopup().close();
    });

    $('#txtPopMaxDemandaHora').on('focus', function () {
        setTimeout(function () {
            $('.ui-timepicker-container.ui-timepicker-standard').css('z-index', '2002');
        }, 10);
    });

    consultaMaximaDemandaPreciosPotencia();
});

function copiarParametros() {
    const revision = $('#indexIdRevision').val();
    const anio = $('#indexFecha').val() - 1;

    $.ajax({
        type: 'POST',
        url: controller + 'CopiarParametros',
        contentType: 'application/json; ',
        data: JSON.stringify({
            revision: revision,
            anio: anio
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result.sResultado != -1) {
                SetMessage('#message',
                    result.sMensaje,
                    'success', true);
                consultaMaximaDemandaPreciosPotencia();
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

function exportarParametros(datos) {
    const revision = $('#indexRevision').val();
    const ajuste = $('#indexAjuste').val();
    const anio = $('#indexFecha').val();

    $.ajax({
        type: 'POST',
        url: controller + 'ExportarParametros',
        contentType: 'application/json; ',
        data: JSON.stringify({
            form: datos,
            revision: revision,
            ajuste: ajuste,
            anio: anio
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result != -1) {
                window.location = controller + 'abrirarchivoparametros?formato=' + 1 + '&file=' + result;
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function historicoParametros(codigo) {

    popHistorico = $('#popupHistoricoDemanda').bPopup({
        easing: 'easeOutBack',
        speed: 350,
        transition: 'fadeIn',
        modalClose: false
    },
        function () {
            $.ajax({
                type: 'POST',
                url: controller + 'HistoricoParametros',
                contentType: 'application/json; ',
                data: JSON.stringify({
                    codigo: codigo
                }),
                datatype: 'json',
                traditional: true,
                success: function (result) {

                    dtHistorico = $('#popupdtHistorico').DataTable({
                        data: result.GrillaHistoricoParametros,
                        columns: [
                            { title: 'Usuario', data: 'Cpaphsusuario' },
                            {
                                title: 'Fecha',
                                data: 'Cpaphsfecha',
                                render: function (data) {
                                    return formatDate(data);
                                }
                            },
                            {
                                title: 'Hora',
                                data: 'Cpaphsfecha',
                                render: function (data) {
                                    return formatTime(data);
                                }
                            },
                            { title: 'Tipo', data: 'Cpaphstipo' }
                        ],
                        initComplete: function () {
                            $('#popupdtHistorico').css('width', '100%');
                            $('.dataTables_scrollHeadInner').css('width', '100%');
                            $('.dataTables_scrollHeadInner table').css('width', '100%');
                            //$('.dataTables_paginate').css('width', '20%');
                        },
                        columnDefs: [
                        ],
                        createdRow: function (row, data, index) {

                        },
                        searching: false,
                        bLengthChange: false,
                        bSort: false,
                        destroy: true,
                        paging: false,
                        info: false,
                        //pageLength: 5
                        scrollY: '250px'
                    });
                },
                error: function () {
                    alert("Ha ocurrido un problema...");
                }
            });
        }

    )
}

function anularParametros(codigo) {

    $.ajax({
        type: 'POST',
        url: controller + 'AnularParametros',
        contentType: 'application/json; ',
        data: JSON.stringify({
            codigo: codigo
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            consultaMaximaDemandaPreciosPotencia();
            SetMessage('#message',
                result.sMensaje,
                'success', true);
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function registrarParametros() {
    const revision = $('#indexIdRevision').val();
    const anio = $('#txtPopMaxDemandaAnio').val();
    const mesName = $('#txtPopMaxDemandaMes').val().trim();
    const mesNumero = meses.indexOf(mesName) + 1;
    const registro = $('#txtPopMaxDemandaTipoRegistro').val();
    const fecha = $('#txtPopMaxDemandaFecha').val();
    const hora = $('#txtPopMaxDemandaHora').val();
    const cambio = $('#txtPopMaxDemandaTipoCambio').val();
    const precio = $('#txtPopMaxDemandaPrecio').val();

    if (!anio || !mesName || !fecha || !hora || !cambio || !precio) {
        SetMessage('#message-popup',
            'Se debe ingresar todos los datos...',
            'warning', true);
        return;
    }

    $.ajax({
        type: 'POST',
        url: controller + 'RegistrarParametros',
        contentType: 'application/json; ',
        data: JSON.stringify({
            revision: revision,
            anio: anio,
            mes: mesNumero,
            registro: registro,
            fecha: fecha,
            hora: hora,
            cambio: cambio,
            precio: precio
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result.sTipo != "success") {
                SetMessage('#message-popup',
                    result.sMensaje,
                    'error', true);
            }
            else {
                $("#popupMaxDemanda").bPopup().close();
                consultaMaximaDemandaPreciosPotencia();
                SetMessage('#message',
                    result.sMensaje,
                    result.sTipo, true);
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}


function editarParametros() {
    const anio = $('#txtPopMaxDemandaAnio').val();
    const mesName = $('#txtPopMaxDemandaMes').val().trim();
    console.log(mesName, 'mesName');
    const mes = meses.indexOf(mesName) + 1;
    console.log(mes, 'mes');
    const registro = $('#txtPopMaxDemandaTipoRegistro').val();
    const cambio = $('#txtPopMaxDemandaTipoCambio').val();
    const fecha = $('#txtPopMaxDemandaFecha').val();
    const hora = $('#txtPopMaxDemandaHora').val();
    const precio = $('#txtPopMaxDemandaPrecio').val();

    if (!anio || !mesName || !fecha || !hora || !cambio || !precio) {
        SetMessage('#message-popup',
            'Se debe ingresar todos los datos...',
            'warning', true);
        return;
    }

    $.ajax({
        type: 'POST',
        url: controller + 'EditarParametros',
        contentType: 'application/json; ',
        data: JSON.stringify({
            codigo: id,
            anio: anio,
            mes: mes,
            registro: registro,
            fecha: fecha,
            hora: hora,
            cambio: cambio,
            precio: precio
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result.sTipo != "success") {
                SetMessage('#message-popup',
                    result.sMensaje,
                    'error', true);
            }
            else {
                $("#popupMaxDemanda").bPopup().close();
                consultaMaximaDemandaPreciosPotencia();
                SetMessage('#message',
                    result.sMensaje,
                    result.sTipo, true);
            }

        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function consultaMaximaDemandaPreciosPotencia() {

    const revision = $('#indexIdRevision').val();
    const checkboxes = document.querySelectorAll('input[name="demEstado"]:checked');
    var estado = [];
    checkboxes.forEach(function (checkbox) {
        estado.push(checkbox.value);
    });
    if (estado.length > 0) {
        estado.join(', ');
    } else {
        SetMessage('#message',
            'Se debe seleccionar al menos un estado...',
            'warning', true);
        return;
    }
    const anio = $('#indexFecha').val() - 1;

    $.ajax({
        type: 'POST',
        url: controller + 'ListaParametrosRegistrados',
        contentType: 'application/json; ',//charset=utf-8
        data: JSON.stringify({
            revision: revision,
            estado: estado,
            anio: anio
        }),
        datatype: 'json',
        success: function (result) {
            dtMaximaDemandaPreciosPotencia = $('#dtListaMaximaDemandaPreciosPotencia').DataTable({
                data: result.GrillaPrincipalParametros,
                columns: [
                    {
                        title: 'Acciones',
                        data: null,
                        defaultContent: ''
                    },
                    {
                        title: 'Estado',
                        data: 'Cpaprmestado',
                        render: function (data) {
                            if (data === 'A')
                                return 'Activo';
                            else
                                return 'Anulado';
                        }
                    },
                    {
                        title: 'Año-Mes Ejercicio anterior',
                        data: 'Aniomes',
                        render: function (data) {
                            return '<div style="text-align: left; padding-left: 10px;">' + data + '</div>';
                        }
                    },
                    { title: 'Tipo registro MD', data: 'Cpaprmtipomd' },
                    {
                        title: 'Fecha MD',
                        data: 'Cpaprmfechamd',
                        render: function (data) {
                            return formatDate(data);
                        }
                    },
                    {
                        title: 'Hora MD',
                        data: 'Cpaprmfechamd',
                        render: function (data) {
                            return formatTime(data);
                        }
                    },
                    {
                        title: 'Tipo de cambio S/ - U$$',
                        data: 'Cpaprmcambio',
                        render: function (data, type, row) {
                            return type === 'display' && !isNaN(data) ? parseFloat(data).toFixed(3) : data;
                        }
                    },
                    {
                        title: 'Precio Potencia S/ kW-mes',
                        data: 'Cpaprmprecio',
                        render: function (data, type, row) {
                            return type === 'display' && !isNaN(data) ? parseFloat(data).toFixed(2) : data;
                        }
                    },
                    { title: 'Usuario creación', data: 'Cpaprmusucreacion' },
                    {
                        title: 'Fecha creación',
                        data: 'Cpaprmfeccreacion',
                        render: function (data) {
                            if (data) {
                                return formatDate(data);
                            } else {
                                return data
                            }
                        }
                    },
                    { title: 'Usuario actualización', data: 'Cpaprmusumodificacion' },
                    {
                        title: 'Fecha actualización',
                        data: 'Cpaprmfecmodificacion',
                        render: function (data) {
                            if (data) {
                                return formatDate(data);
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
                            var eliminarClass = (row.Cpaprmestado === 'A' && row.Cpaprmtipomd === 'Proyectado') ? '' : 'hidden';
                            var editarClass = row.Cpaprmestado === 'X' ? 'hidden' : '';

                            return '<img class="clsEditar ' + editarClass + '" src="' + rutaImagenes + 'btn-edit.png" style="cursor: pointer;" title="Editar"/> ' +
                                '<img class="clsCambios" src="' + rutaImagenes + 'envios.png" style="cursor: pointer;" title="Ver Cambios"/> ' +
                                '<img class="clsEliminar ' + eliminarClass + '" src="' + rutaImagenes + 'btn-cancel.png" style="cursor: pointer;" title="Eliminar"/> ';
                            
                        }
                    }
                ],
                createdRow: function (row, data, index) {

                },
                searching: false,
                bLengthChange: false,
                bSort: false,
                destroy: true,
                paging: false,
                info: false,
                scrollY: '350px',
            });
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });

}

$(document).on('click', '#dtListaMaximaDemandaPreciosPotencia tr td .clsEditar:not(.disabled)', function (e) {

    var row = $(this).closest('tr');
    var data = dtMaximaDemandaPreciosPotencia.row(row).data();
    const anio = data.Cpaprmanio;
    const mes = data.Cpaprmmes;
    const tipo = data.Cpaprmtipomd;
    const fecha = formatDate(data.Cpaprmfechamd);
    const hora = formatTime(data.Cpaprmfechamd);
    const cambio = data.Cpaprmcambio;
    const precio = data.Cpaprmprecio;
    flag = 2;//para editar
    id = data.Cpaprmcodi;;
    const listaMeses = ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Setiembre', 'Octubre', 'Noviembre', 'Diciembre'];

    $('#txtPopMaxDemandaFecha').val('');
    var selectedMonth = mes - 1;
    var selectedYear = anio;
    var firstDay = new Date(selectedYear, selectedMonth, 1);   
    var lastDay = new Date(selectedYear, selectedMonth + 1, 0);

    var firstDayFormatted = (firstDay.getDate() < 10 ? '0' : '') + firstDay.getDate() + '/' + (firstDay.getMonth() + 1 < 10 ? '0' : '') + (firstDay.getMonth() + 1) + '/' + firstDay.getFullYear();
    var lastDayFormatted = (lastDay.getDate() < 10 ? '0' : '') + lastDay.getDate() + '/' + (lastDay.getMonth() + 1 < 10 ? '0' : '') + (lastDay.getMonth() + 1) + '/' + lastDay.getFullYear();

    $('#txtPopMaxDemandaFecha').Zebra_DatePicker('destroy');
    $('#txtPopMaxDemandaFecha').Zebra_DatePicker({
        format: 'd/m/Y',
        direction: [firstDayFormatted, lastDayFormatted],
    });

    document.getElementById('txtPopMaxDemandaAnio').value = anio;
    document.getElementById('txtPopMaxDemandaMes').value = listaMeses[mes - 1];
    document.getElementById('txtPopMaxDemandaTipoRegistro').value = tipo[0];
    document.getElementById('txtPopMaxDemandaFecha').value = fecha;
    document.getElementById('txtPopMaxDemandaHora').value = hora;
    document.getElementById('txtPopMaxDemandaTipoCambio').value = cambio;
    document.getElementById('txtPopMaxDemandaPrecio').style.backgroundColor = ''
    document.getElementById('txtPopMaxDemandaPrecio').value = precio;

    if (tipo === 'Proyectado') {
        document.getElementById('txtPopMaxDemandaMes').disabled = true;
        document.getElementById('txtPopMaxDemandaFecha').disabled = false;
        document.getElementById('txtPopMaxDemandaHora').disabled = false;
        document.getElementById('txtPopMaxDemandaPrecio').disabled = false;
        document.getElementById('txtPopMaxDemandaTipoCambio').disabled = false;
        document.getElementById('txtPopMaxDemandaTipoRegistro').disabled = true;
    } else {
        document.getElementById('txtPopMaxDemandaMes').disabled = true;
        document.getElementById('txtPopMaxDemandaFecha').disabled = true;
        document.getElementById('txtPopMaxDemandaHora').disabled = true;
        document.getElementById('txtPopMaxDemandaPrecio').style.backgroundColor = '#e0e0e0';
        document.getElementById('txtPopMaxDemandaPrecio').disabled = true;
        document.getElementById('txtPopMaxDemandaTipoCambio').disabled = false;
        document.getElementById('txtPopMaxDemandaTipoRegistro').disabled = true;
    }

    popMaxDemanda = $('#popupMaxDemanda').bPopup({
        easing: 'easeOutBack',
        speed: 350,
        transition: 'fadeIn',
        modalClose: false,
        zIndex: 1000
    },
    )

});

$(document).on('click', '#dtListaMaximaDemandaPreciosPotencia tr td .clsEliminar:not(.disabled)', function (e) {

    var row = $(this).closest('tr');
    var data = dtMaximaDemandaPreciosPotencia.row(row).data();

    const codigo = data.Cpaprmcodi;
    var confirmacion = confirm('¿Estás seguro de que deseas anular el registro seleccionado?');

    if (confirmacion) {
        anularParametros(codigo);
    }
});

$(document).on('click', '#dtListaMaximaDemandaPreciosPotencia tr td .clsCambios:not(.disabled)', function (e) {

    var row = $(this).closest('tr');
    var data = dtMaximaDemandaPreciosPotencia.row(row).data();

    const codigo = data.Cpaprmcodi;
    historicoParametros(codigo);
});

function formatTime(time) {

    const milliseconds = parseInt(time.match(/(\d+)/)[0], 10);
    const date = new Date(milliseconds);
    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');

    return `${hours}:${minutes}`;
}