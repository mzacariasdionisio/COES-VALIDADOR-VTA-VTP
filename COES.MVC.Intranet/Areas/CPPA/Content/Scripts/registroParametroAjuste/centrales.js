var dtCentralesGeneracion, dtCentralAgregada
var popEditar
var cpacntcodi, finComercial
$(document).ready(function () {

    $('#popupGeneracionCentral').multipleSelect({
        single: true,
        filter: true
    });

    $('#popupGeneracionBarra').multipleSelect({
        single: true,
        filter: true
    });

    $('#popupGeneracionTransferencia').multipleSelect({
        single: true,
        filter: true
    });

    $('#btnExportar').on('click', function () {
        var myTable = $("#dtListaCentralesGeneracion").DataTable();
        var form_data = myTable.rows().data();
        var datos = [];
        for (var i = 0; i < form_data.length; i++) {
            var fila = [];
            fila.push(form_data[i].Equinomb);
            fila.push(form_data[i].Emprnomb);
            var ejecInicio = form_data[i].Cpacntfecejecinicio;
            fila.push(ejecInicio ? formatDate(ejecInicio) : null);
            var ejecFin= form_data[i].Cpacntfecejecfin;
            fila.push(ejecFin ? formatDate(ejecFin) : null);
            var progInicio = form_data[i].Cpacntfecproginicio;
            fila.push(progInicio ? formatDate(progInicio) : null);
            var progFin = form_data[i].Cpacntfecprogfin;
            fila.push(progFin ? formatDate(progFin) : null);
            fila.push(form_data[i].Barrbarratransferencia);
            fila.push(form_data[i].Ptomedidesc);
            fila.push(form_data[i].Cpacntusumodificacion);
            var fechaModificacion = form_data[i].Cpacntfecmodificacion;
            fila.push(fechaModificacion ? formatDate(fechaModificacion) : null);
            fila.push(form_data[i].Centralespmpo);
            datos.push(fila);
        }
        exportarCentralesPMPO(datos);
    });


    dtCentralAgregada = $('#popupdtCentralAgregada').DataTable({
        columns: [
            { title: '', data: null },
            { title: 'Central', data: 'Ptomedicodi', visible: false },
            { title: 'Central', data: 'Ptomedidesc' }
        ],
        initComplete: function () {
            $('#popupdtCentralAgregada').css('width', '100%');
            $('.dataTables_scrollHeadInner').css('width', '100%');
            $('.dataTables_scrollHeadInner table').css('width', '98.84%');
            $('.dataTables_paginate').css('width', '90%');
        },
        columnDefs: [
            {
                targets: [0],
                width: 50,
                defaultContent: '<img class="clsEliminar" src="' + rutaImagenes + 'btn-cancel.png" style="cursor: pointer;" title="Eliminar"/> '
            }
        ],
        searching: false,
        bLengthChange: false,
        bSort: false,
        destroy: true,
        paging: false,
        /*searching: true,*/
        pageLength: 5,
        info: false
    });

    //Aplica la lib. Zebra_Datepicker
    $('.popupFecha').Zebra_DatePicker({
    });

    $('.filtro-select').change(function () {
        const currentSelect = $(this);
        $('.filtro-select').each(function () {
            if ($(this).is(currentSelect)) {
                return;
            }
            $(this).val('-1'); 
        });
    });

    //limpiar fechas
    $('#btnFechaInicioEjecutada').on('click', function () {
        document.getElementById('popFechaInicioEjecutada').value = '';
    });

    $('#btnFechaFinEjecutada').on('click', function () {
        document.getElementById('popFechaFinEjecutada').value = '';
    });
    $('#btnFechaInicioProgramada').on('click', function () {
        document.getElementById('popFechaInicioProgramada').value = '';
    });

    $('#btnFechaFinProgramada').on('click', function () {
        document.getElementById('popFechaFinProgramada').value = '';
    });
    //limpiar fechas
    $('#btnConsultar').on('click', function () {
        listaGrillaPrincipal();
    });

    $('#popbtnCancelar').on('click', function () {
        $("#popupEditarCentral").bPopup().close();
    });
    
    $('#popbtnRefrescar').on('click', function () {
        limpioComponentes();
    });

    $('#btnRefrescar').on('click', function () {
        $('.filtro-select').each(function () {
            $(this).val('-1');
        });

        listaGrillaPrincipal();
    });

    $('#popupGeneracionAgregar').on('click', function () {
        var centralGeneracion = $('#popupGeneracionCentral option:selected').text();
        var centralGeneracionId = $('#popupGeneracionCentral').val();

        if (centralGeneracionId === '-1') {
            SetMessage('#message-popupEditarCentral',
                'Se debe seleccionar una central...',
                'error', true);
            return; 
        }

        if (centralGeneracionId) {
            dtCentralAgregada.row.add({
                "Ptomedicodi": centralGeneracionId,
                "Ptomedidesc": centralGeneracion
            }).draw();
            $('#popupGeneracionCentral option:selected').remove();
        } else {
            alert("No tiene mas centrales disponibles....");
        }

    }); 

    $('#popbtnGrabar').on('click', function () {
        var trans = $('#popupGeneracionTransferencia').val()
        var barr = $('#popupGeneracionBarra').val()

        if (trans === "-1") {
            SetMessage('#message-popupEditarCentral',
                'Se debe seleccionar una barra de transferencia',
                'error', true);
            return;
        }
        if (barr === "-1") {
            SetMessage('#message-popupEditarCentral',
                'Se debe seleccionar una barra PMPO',
                'error', true);
            return;
        }
        recogeInformacionPMPO();
    });

    listaGrillaPrincipal();
});

function exportarCentralesPMPO(datos) {
    const revision = $('#indexRevision').val();
    const ajuste = $('#indexAjuste').val();
    const anio = $('#indexFecha').val();

    $.ajax({
        type: 'POST',
        url: controller + 'ExportarCentralesPMPO',
        contentType: 'application/json; charset=utf-8',
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
                window.location = controller + 'abrirarchivo?formato=' + 1 + '&file=' + result;
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}



function recogeInformacionPMPO() {
    const centralTransferencia = $('#popupGeneracionTransferencia').val();
    const barraPMPO = $('#popupGeneracionBarra').val();
    const centralesPMPO = dtCentralAgregada.rows().data().toArray();
    const ejecInicio = $('#popFechaInicioEjecutada').val();
    const ejecFin = $('#popFechaFinEjecutada').val();
    const progInicio = $('#popFechaInicioProgramada').val();
    const progFin = $('#popFechaFinProgramada').val();
    const ajuste = $('#indexAjuste').val();
    const revision = $('#indexIdRevision').val();

    if (!barraPMPO) {
        SetMessage('#message-popupEditarCentral',
            'Se debe seleccionar una barra PMPO',
            'error', true);
        return false;
    }

    if (!centralTransferencia) {
        SetMessage('#message-popupEditarCentral',
            'Se debe seleccionar una central de transferencia',
            'error', true);
        return false;
    }

    //if (centralesPMPO.length <= 0) {
    //    SetMessage('#message-popupEditarCentral',
    //        'Se debe agregar al menos un central PMPO',
    //        'error', true);
    //    return false;
    //}

    if (finComercial) {
        registrarParametrosCentralPMPO(revision, ajuste, centralTransferencia, barraPMPO, centralesPMPO, ejecInicio, ejecFin, progInicio, progFin);
    }
    else {
        if (validarFechas(ejecInicio, ejecFin, progInicio, progFin)) {
            registrarParametrosCentralPMPO(revision, ajuste, centralTransferencia, barraPMPO, centralesPMPO, ejecInicio, ejecFin, progInicio, progFin);
        }
    }
}

function validarFechas(eInicio, eFin, pInicio, pFin) {

    // Convertir cadenas a objetos Date
    const ejecInicio = convertirFecha(eInicio);
    const ejecFin = convertirFecha(eFin);
    const progInicio = convertirFecha(pInicio);
    const progFin = convertirFecha(pFin);

    //Armando fecha minima para el ejecutado
    let anio = $('#indexFecha').val() - 1;
    const fechaObj = new Date(anio, 0, 1);
    //Validando que existe inicio
    if (eInicio) {
        if (ejecInicio < fechaObj) {
            SetMessage('#message-popupEditarCentral',
                'Si se ingresa una Fecha de inicio ejecutada, esta debe ser mayor o igual a ' + '01/01/' + anio,
                'error', true);
            return false;
        }
    }

    // Validación para ejecInicio y ejecFin
    if (eInicio && !eFin) {
        SetMessage('#message-popupEditarCentral',
            'Si se ingresa una Fecha de inicio ejecutada, es obligatorio ingresar una de fin.',
            'error', true);
        return false;
    }
    if (!eInicio && eFin) {
        SetMessage('#message-popupEditarCentral',
            'Si se ingresa una Fecha de fin ejecutada, es obligatorio ingresar una de inicio.',
            'error', true);
        return false;
    }
    if (ejecInicio > ejecFin) {
        SetMessage('#message-popupEditarCentral',
            'La fecha de inicio ejecutada debe ser menor que la fecha fin.',
            'error', true);
        return false;
    }
    if (pInicio) {
        if (!eInicio) {
            SetMessage('#message-popupEditarCentral',
                'Si se ingresa una Fecha programada, se debe ingresar una Fecha ejecutada...',
                'error', true);
            return false;
        }
    }

    // Validación para progInicio y progFin
    if (pInicio && !pFin) {
        SetMessage('#message-popupEditarCentral',
            'Si se ingresa una Fecha de inicio programada, es obligatorio ingresar una de fin.',
            'error', true);
        return false;
    }
    if (!pInicio && pFin) {
        SetMessage('#message-popupEditarCentral',
            'Si se ingresa una Fecha de fin programada, es obligatorio ingresar una de inicio.',
            'error', true);
        return false;
    }
    if (progInicio > progFin) {
        SetMessage('#message-popupEditarCentral',
            'La fecha de inicio programada debe ser menor que la fecha fin',
            'error', true);
        return false;
    }
    if (eFin && pInicio) {
        const fechaEsperadaInicio = new Date(ejecFin);
        fechaEsperadaInicio.setDate(fechaEsperadaInicio.getDate() + 1);
        if (progInicio.getFullYear() === fechaEsperadaInicio.getFullYear() &&
            progInicio.getMonth() === fechaEsperadaInicio.getMonth() &&
            progInicio.getDate() === fechaEsperadaInicio.getDate()) {
        } else {
            SetMessage('#message-popupEditarCentral',
                'La fecha de inicio programada debe ser el día siguiente a la fecha de fin de ejecución.',
                'error', true);
            return false;
        }
    }

    const nuevaFecha = sumarUnAnioYUnDia(eInicio, -1, 1);
    if (pFin && eInicio) {
        if (progFin > nuevaFecha) {
            SetMessage('#message-popupEditarCentral',
                'Recuerde que el maximo es un año...1',
                'error', true);
            return false;
        }
    }
    if (!pFin && eInicio) {
        if (ejecFin > nuevaFecha) {
            SetMessage('#message-popupEditarCentral',
                'Recuerde que el maximo es un año...2',
                'error', true);
            return false;
        }
    }
    return true;
}

function listaGrillaPrincipal() {

    const revision = $('#indexIdRevision').val();
    const central = $('#cbofiltroCentral').val();
    const empresa = $('#cbofiltroEmpresa').val();
    const barraTrans = $('#cbofiltroBarraTransferencia').val();

    $.ajax({
        type: 'POST',
        url: controller + 'ListaGrillaPrincipal',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            revision: revision,
            central: central,
            empresa: empresa,
            barraTrans: barraTrans
        }),
        datatype: 'json',
        success: function (result) {
            dtCentralesGeneracion = $('#dtListaCentralesGeneracion').DataTable({
                data: result.GrillaPrincipal,
                columns: [
                    {
                        title: 'Acciones',
                        data: null,
                        defaultContent: ''
                    },
                    { title: 'Cpacntcodi', data: 'Cpacntcodi', visible: false },
                    { title: 'Cpaempcodi', data: 'Cpaempcodi', visible: false },
                    { title: 'Cparcodi', data: 'Cparcodi', visible: false },
                    { title: 'Equicodi', data: 'Equicodi', visible: false },
                    {
                        title: 'Central',
                        data: 'Equinomb',
                        render: function (data) {
                            return '<div style="text-align: left; padding-left: 10px;">' + data + '</div>';
                        }
                    }, 
                    {
                        title: 'InicioComercial',
                        data: 'Equifechiniopcom',
                        visible: false,
                        render: function (data) {
                            if (data) {
                                return formatDate(data);
                            } else {
                                return data
                            }
                        }
                    },
                    {
                        title: 'FinCormercial',
                        data: 'Equifechfinopcom',
                        visible: false,
                        render: function (data) {
                            if (data) {
                                return formatDate(data);
                            } else {
                                return data
                            }
                        }
                    },
                    { title: 'Emprcodi', data: 'Emprcodi', visible: false },
                    {
                        title: 'Empresa',
                        data: 'Emprnomb',
                        render: function (data) {
                            return '<div style="text-align: left; padding-left: 10px;">' + data + '</div>';
                        }
                    },

                    {
                        title: 'Fecha inicio',
                        data: 'Cpacntfecejecinicio',
                        render: function (data) {
                            if (data) {
                                return formatDate(data);
                            } else {
                                return data
                            }
                        }
                    },
                    {
                        title: 'Fecha fin',
                        data: 'Cpacntfecejecfin',
                        render: function (data) {
                            if (data) {
                                return formatDate(data);
                            } else {
                                return data
                            }
                        }
                    },
                    {
                        title: 'Fecha inicio',
                        data: 'Cpacntfecproginicio',
                        render: function (data) {
                            if (data) {
                                return formatDate(data);
                            } else {
                                return data
                            }
                        }
                    },
                    {
                        title: 'Fecha inicio',
                        data: 'Cpacntfecprogfin',
                        render: function (data) {
                            if (data) {
                                return formatDate(data);
                            } else {
                                return data
                            }
                        }
                    },
                    { title: 'Barrcodi', data: 'Barrcodi', visible: false },
                    {
                        title: 'Barra Transferencia',
                        data: 'Barrbarratransferencia',
                        render: function (data) {
                            if (data) {
                                return '<div style="text-align: left; padding-left: 10px;">' + data + '</div>';
                            } else {
                                return data;
                            }
                        }
                    },
                    { title: 'Ptomedicodi', data: 'Ptomedicodi', visible: false },
                    {
                        title: 'Barra PMPO',
                        data: 'Ptomedidesc',
                        render: function (data) {
                            if (data) {
                                return '<div style="text-align: left; padding-left: 10px;">' + data + '</div>';
                            } else {
                                return data;
                            }
                        }
                    },
                    { title: 'Tipo', data: 'Cpacnttipo', visible: false },
                    { title: 'Estado', data: 'Cpacntestado', visible: false },
                    {
                        title: 'Usuario actualización',
                        data: 'Cpacntusumodificacion',
                        render: function (data) {
                            if (data) {
                                return '<div style="text-align: left; padding-left: 10px;">' + data + '</div>';
                            }else{
                                return data;
                            }
                        }
                    },
                    {
                        title: 'Fecha actualización',
                        data: 'Cpacntfecmodificacion',
                        render: function (data) {
                            if (data) {
                                return formatDate(data);
                            } else {
                                return data
                            }
                        }
                    },
                    { title: 'CentralesPMPO', data: 'Centralespmpo', visible: false }
                ],
                columnDefs: [
                    {
                        targets: [0],
                        width: 100,
                        render: function (data, type, row) {
                            return '<img class="clsEditarCentral" src="' + rutaImagenes + 'btn-edit.png" style="cursor: pointer;" title="Editar"/> ';
                        }
                    }
                ],
                createdRow: function (row, data, index) {
                    $(row).find('.clsEditarCentral').on('click', function () {
                        var labelAjuste = document.getElementById('popupDescripcionAjuste');
                        var labelCentral = document.getElementById('popupDescripcionCentral');
                        var labelEmpresa = document.getElementById('popupDescripcionEmpresa');

                        const revision = $('#indexRevision').val();
                        const ajuste = $('#indexAjuste').val();
                        let anio = $('#indexFecha').val();

                        // Comprobar si tiene fecha de cierre comercial
                        if (data.Equifechfinopcom) {
                            const fechaObj = new Date(anio - 1, 0, 1);
                            const fechaComercial = convertirFecha(formatDate(data.Equifechfinopcom));

                            if (fechaComercial < fechaObj) {
                                SetMessage('#message',
                                    'No se puede registrar ya que su fecha de cierre comercial es ' + formatDate(data.Equifechfinopcom),
                                    'error', true);
                            } else {
                                cpacntcodi = data.Cpacntcodi;
                                finComercial = data.Equifechfinopcom;

                                labelAjuste.textContent = "Ajuste " + anio + "-" + ajuste + "-" + revision;
                                labelCentral.textContent = "Central de generación - " + data.Equinomb;
                                labelEmpresa.textContent = "Empresa - " + data.Emprnomb;

                                listaCentralesAgregadas(data.Emprcodi, data.Cpacntcodi, data.Cpacntfecejecinicio, data.Cpacntfecejecfin, data.Cpacntfecproginicio, data.Cpacntfecprogfin, data.Barrcodi, data.Ptomedicodi);
                            }
                        } else {
                            cpacntcodi = data.Cpacntcodi;
                            finComercial = data.Equifechfinopcom;

                            labelAjuste.textContent = "Ajuste " + anio + "-" + ajuste + "-" + revision;
                            labelCentral.textContent = "Central de generación - " + data.Equinomb;
                            labelEmpresa.textContent = "Empresa - " + data.Emprnomb;

                            listaCentralesAgregadas(data.Emprcodi, data.Cpacntcodi, data.Cpacntfecejecinicio, data.Cpacntfecejecfin, data.Cpacntfecproginicio, data.Cpacntfecprogfin, data.Barrcodi, data.Ptomedicodi);
                        }
                    });
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

function listaCentralesAgregadas(empresa, central, eInicio, eFin, pInicio, pFin, barraTrans, barraPMPO) {
    //$('#popupdtCentralAgregada').html("");
    dtCentralAgregada.clear();

    popEditar = $('#popupEditarCentral').bPopup({
        easing: 'easeOutBack',
        speed: 350,
        transition: 'fadeIn',
        modalClose: false
    },
        function () {
            $.ajax({
                type: 'POST',
                url: controller + 'ListaCentralesAgregadas',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({
                    empresa: empresa,
                    central: central
                }),
                datatype: 'json',
                traditional: true,
                success: function (result) {
                    limpioComponentes();
                    estadoComponentes(true);
                    estadoBotones('none');
                    RefillDropDownListv2($('#popupGeneracionTransferencia'), result.popupfiltroBarrasTransferencia, 'BarrCodi', 'BarrNombreConcatenado');
                    RefillDropDownListv2($('#popupGeneracionCentral'), result.popupfiltroCentralesPMPO, 'Ptomedicodi', 'Ptomedidesc');
                    RefillDropDownListv2($('#popupGeneracionBarra'), result.popupfiltroBarrasPMPO, 'Ptomedicodi', 'Ptomedidesc');
                    dtCentralAgregada.rows.add(result.popupGrillaCentralesPMPO).draw();

                    if (finComercial) {
                        let anio = $('#indexFecha').val() - 1;
                        const fechaObj = new Date(anio, 0, 1);
                        const fechaComercial = convertirFecha(formatDate(finComercial));//formatDate(finComercial)
                        if (fechaComercial > fechaObj) {
                            document.getElementById('popbtnGrabar').disabled = false;
                            document.getElementById('popFechaInicioEjecutada').value = formatearFecha(fechaObj);
                            document.getElementById('popFechaFinEjecutada').value = formatearFecha(sumarUnAnioYUnDia(formatDate(finComercial), -1, 0));
                        }
                    }
                    else {
                        estadoBotones('inline-block');
                        estadoComponentes(false);
                        limpioComponentes();
                        setearDatos(eInicio, eFin, pInicio, pFin, barraTrans, barraPMPO);
                    }
                },
                error: function () {
                    alert("Ha ocurrido un problema...");
                }
            });
        }
    )
}

$('#popupdtCentralAgregada tbody').on('click', '.clsEliminar', function () {
    //dtCentralAgregada.row($(this).parents('tr')).remove().draw();
    var row = dtCentralAgregada.row($(this).parents('tr'));
    var data = row.data();
    $('#popupGeneracionCentral').append(
        $('<option>', {
            value: data.Ptomedicodi,
            text: data.Ptomedidesc
        })
    );
    row.remove().draw();
});

function convertirFecha(fechaStr) {
    const partes = fechaStr.split('/'); // Suponiendo que el formato es DD/MM/YYYY
    if (partes.length !== 3) {
        return null;
    }
    const dia = parseInt(partes[0], 10);
    const mes = parseInt(partes[1], 10) - 1;
    const año = parseInt(partes[2], 10);
    return new Date(año, mes, dia);
}

function sumarUnAnioYUnDia(fechaStr, sdia, sanio) {
    // Suponiendo que la fecha está en formato DD/MM/YYYY
    const partes = fechaStr.split('/');
    if (partes.length !== 3) {
        return null;
    }
    const dia = parseInt(partes[0], 10);
    const mes = parseInt(partes[1], 10) - 1;
    const año = parseInt(partes[2], 10);

    const fecha = new Date(año, mes, dia);
    fecha.setFullYear(fecha.getFullYear() + sanio);
    fecha.setDate(fecha.getDate() + sdia);

    // Formatear la fecha de nuevo a DD/MM/YYYY
    const nuevoDia = String(fecha.getDate()).padStart(2, '0');
    const nuevoMes = String(fecha.getMonth()).padStart(2, '0');
    const nuevoAño = fecha.getFullYear();

    return new Date(nuevoAño, nuevoMes, nuevoDia);
}

function formatearFecha(fecha) {
    const dia = String(fecha.getDate()).padStart(2, '0');
    const mes = String(fecha.getMonth() + 1).padStart(2, '0'); 
    const anio = fecha.getFullYear(); 
    return `${dia}/${mes}/${anio}`;
}

function estadoComponentes(estado) {
    document.getElementById('popFechaInicioEjecutada').disabled = estado;
    document.getElementById('popFechaFinEjecutada').disabled = estado;
    document.getElementById('popFechaInicioProgramada').disabled = estado;
    document.getElementById('popFechaFinProgramada').disabled = estado;
    document.getElementById('popbtnGrabar').disabled = estado;
}

function estadoBotones(estado) {
    document.getElementById('btnFechaInicioEjecutada').style.display = estado;
    document.getElementById('btnFechaFinEjecutada').style.display = estado;
    document.getElementById('btnFechaInicioProgramada').style.display = estado;
    document.getElementById('btnFechaFinProgramada').style.display = estado;
}

function limpioComponentes() {
    document.getElementById('popFechaInicioEjecutada').value = '';
    document.getElementById('popFechaFinEjecutada').value = '';
    document.getElementById('popFechaInicioProgramada').value = '';
    document.getElementById('popFechaFinProgramada').value = '';
}

function registrarParametrosCentralPMPO(revision, ajuste, centralTransferencia, barraPMPO, centralesPMPO, ejecInicio, ejecFin, progInicio, progFin) {
    $.ajax({
        type: 'POST',
        url: controller + 'RegistraEquiposPMPO',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            revision: revision,
            ajuste: ajuste,
            id: cpacntcodi,
            transferencia: centralTransferencia,
            barraPMPO: barraPMPO,
            centralesPMPO: centralesPMPO,
            ejecInicio: ejecInicio,
            ejecFin: ejecFin,
            progInicio: progInicio,
            progFin: progFin
        }),
        datatype: 'json',
        success: function (result) {
            $("#popupEditarCentral").bPopup().close();
            if (result.sResultado != -1) {
                SetMessage('#message',
                    result.sMensaje,
                    'success', true);
                listaGrillaPrincipal();
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

function setearDatos(eInicio, eFin, pInicio, pFin, barraTrans, barraPMPO) {
    if (eInicio) {
        document.getElementById('popFechaInicioEjecutada').value = formatDate(eInicio);
        document.getElementById('popFechaFinEjecutada').value = formatDate(eFin);
    }
    if (pInicio) {
        document.getElementById('popFechaInicioProgramada').value = formatDate(pInicio);
        document.getElementById('popFechaFinProgramada').value = formatDate(pFin);
    }
    if (barraTrans) {
        $('#popupGeneracionTransferencia').multipleSelect('setSelects', [barraTrans]);
    } else {
        $('#popupGeneracionTransferencia').multipleSelect('setSelects', [-1]);
    }
    if (barraPMPO) {
        $('#popupGeneracionBarra').multipleSelect('setSelects', [barraPMPO]);
    } else {
        $('#popupGeneracionBarra').multipleSelect('setSelects', [-1]);
    }
    $('#popupGeneracionCentral').multipleSelect('setSelects', [-1]);
}

function RefillDropDownListv2(element, data, data_id, data_name) {
    //Vacia el elemento
    element.empty();
    //Carga el elemento
    element.append($('<option></option>').val("-1").html("Seleccione"));
    $.each(data, function (i, item) {
        var n_value = i, n_html = item;
        if (data_id) n_value = item[data_id];
        if (data_name) n_html = item[data_name];
        element.append($('<option></option>').val(n_value).html(n_html));
    });
    element.multipleSelect('refresh');
}