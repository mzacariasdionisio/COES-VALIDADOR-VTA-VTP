var CargaVirtual = "CargaVirtual/";
var controler = siteRoot + "ReportesFrecuencia/" + CargaVirtual;

//Funciones de busqueda
$(document).ready(function () {

    $('#tabla').dataTable({
        "sPaginationType": "full_numbers",
        "destroy": "true",
        "aaSorting": [[0, "asc"],[1, "asc"], [2, "asc"]]
    });

    $('#frmCargaVirtual').submit(function (event) {
        if ($('#frmCargaVirtual').valid()) {
            $.ajax({
                type: 'POST',
                url: controler + 'Save',
                data: $('#frmCargaVirtual').serialize(),
                success: function (data) {
                    if (data.sError == '') {
                        location.href = controler + 'index'
                    }
                    else {
                        $('#sError').html(data.sError)
                    }
                }
            })
        }
        event.preventDefault();
        return false;
    })

    $('#frmCargaVirtualExterno').submit(function (event) {
        var file = document.getElementById('Entidad_ArchivoCarga');
        formData = new FormData();
        num_files = 0;
        if (file.files.length > 0) {
            for (var i = 0; i < file.files.length; i++) {
                formData.append('file-' + i, file.files[i]);
                num_files++;
            }
        }
        if (num_files == 0) {
            alert('Debe indicar obligatoriamente el archivo a cargar.');
            return false;
        }
        console.log(formData);

        if ($('#frmCargaVirtualExterno').valid()) {
            $.ajax({
                type: 'POST',
                url: controler + 'SaveExterno',
                data: {
                    modelo: $('#frmCargaVirtualExterno').serialize(),
                    files: formData,
                },
                success: function (data) {
                    if (data.sError == '') {
                        location.href = controler + 'index'
                    }
                    else {
                        $('#sError').html(data.sError)
                    }
                }
            })
        }
        event.preventDefault();
        return false;
    })

    $('#Entidad_FechaCargaInicio').Zebra_DatePicker({
    });

    $('#Entidad_FechaCargaFin').Zebra_DatePicker({
    });

    $('#Entidad_CodEmpresa').change((event) => {
        $('#Entidad_CodCentral').find('option:not(:first-child)').remove();
        let loadCombobox = $('#Entidad_CodCentral');
        loadCombobox.find('option:first-child').text('Cargando....');
        loadCombobox.prop('disabled', true);
        //let genemprcodi = $('#Entidad_CodEmpresa').val() == "" ? 0 : $('#Entidad_CodEmpresa').val();
        let codiEmpresa = $(event.currentTarget).val() == "" ? 0 : $(event.currentTarget).val();
        //console.log('genemprcodi:' + genemprcodi);
        cargarComboCentralPorEmpresa(codiEmpresa).then((data) => {
            $('#Entidad_CodCentral').find('option:not(:first-child)').remove();
            data.map((item) => {
                let option = $(`<option value="${item.NombreCentral}">${item.NombreCentral}</option>`);
                option.data('itemCentral', item)
                $('#Entidad_CodCentral').append(option);
            })

            loadCombobox.prop('disabled', false);
            loadCombobox.find('option:first-child').text('--Seleccione--');
        }).catch((error) => {
            loadCombobox.prop('disabled', false);
            loadCombobox.find('option:first-child').text('--Seleccione--');
            alert(error)
        })
    })

    $('#Entidad_CodCentral').change((event) => {
        $('#Entidad_CodUnidad').find('option:not(:first-child)').remove();
        let loadCombobox = $('#Entidad_CodUnidad');
        loadCombobox.find('option:first-child').text('Cargando....');
        loadCombobox.prop('disabled', true);
        let codiEmpresa = $('#Entidad_CodEmpresa').val() == "" ? 0 : $('#Entidad_CodEmpresa').val();
        let codiCentral = $(event.currentTarget).val() == "" ? 0 : $(event.currentTarget).val();
        cargarComboUnidadPorCentralEmpresa(codiEmpresa, codiCentral).then((data) => {
            $('#Entidad_CodUnidad').find('option:not(:first-child)').remove();
            data.map((item) => {
                let option = $(`<option value="${item.IdUnidad}">${item.NombreUnidad}</option>`);
                option.data('itemCentral', item)
                $('#Entidad_CodUnidad').append(option);
            })

            loadCombobox.prop('disabled', false);
            loadCombobox.find('option:first-child').text('--Seleccione--');
        }).catch((error) => {
            loadCombobox.prop('disabled', false);
            loadCombobox.find('option:first-child').text('--Seleccione--');
            alert(error)
        })
    })


    

});

function cargarComboCentralPorEmpresa(emprcodi) {
    var pro = new Promise((resolve, reject) => {
        $.ajax({
            type: 'POST',
            url: controler + "ListarCentralPorEmpresa",
            data: {
                CodEmpresa: emprcodi
            },
            success: function (evt) {
                resolve(evt)
            },
            error: function () {
                $('#loading').removeClass('cancel')
                reject("Lo sentimos, ha ocurrido un error inesperado");
            }
        });
    })
    return pro;
}

function cargarComboUnidadPorCentralEmpresa(emprcodi, centralcod) {
    var pro = new Promise((resolve, reject) => {
        $.ajax({
            type: 'POST',
            url: controler + "ListarUnidadPorCentralEmpresa",
            data: {
                CodEmpresa: emprcodi,
                Central: centralcod
            },
            success: function (evt) {
                resolve(evt)
            },
            error: function () {
                $('#loading').removeClass('cancel')
                reject("Lo sentimos, ha ocurrido un error inesperado");
            }
        });
    })
    return pro;
}

function mostrarArchivosCargaInicial() {
    var vFechaInic = $('#Entidad_FechaCargaInicio').val();
    $('#lblArchivoInicial').html("Archivo " + vFechaInic.replaceAll('/', '-'));
}

function mostrarArchivosCargaFinal() {
    console.log('mostrarArchivosCargarFinal');
    var vFechaInic = $('#Entidad_FechaCargaInicio').val();
    var vFechaFin = $('#Entidad_FechaCargaFin').val();
    var FechaInicioArray = vFechaInic.split("/");
    var FechaFinArray = vFechaFin.split("/");
    var fechaInicio = new Date(FechaInicioArray[2] + "-" + FechaInicioArray[1] + "-" + FechaInicioArray[0]).getTime();
    var fechaFin = new Date(FechaFinArray[2] + "-" + FechaFinArray[1] + "-" + FechaFinArray[0]).getTime();

    var diff = fechaFin - fechaInicio;
    var numDias = diff / (1000 * 60 * 60 * 24);

    console.log(numDias + 1);
    let tbody = $('#tblFormArchivos tbody');
    tbody.html('');
    for (i = 1; i <= numDias; i++) {
        //fechaActual = fechaInicio.getDate() + i;
        //console.log(fechaActual);
        var result = new Date(fechaInicio);
        result.setDate(result.getDate() + i);
        var diaActual = result.getUTCDate();
        var mesActual = result.getUTCMonth() + 1;
        var anioActual = result.getUTCFullYear();
        if (diaActual < 10) {
            diaActual = "0" + diaActual;
        }
        if (mesActual < 10) {
            mesActual = "0" + mesActual;
        }
        var fecActual = diaActual + "-" + mesActual + "-" + anioActual;
        console.log(fecActual);

        let trRowAdd = $('<tr></tr>')
        var tdNombreArchivo = $('<td>Archivo ' + fecActual +' (*)</td>');
        var td = $('<td></td>');
        let input = $(`<input class="input-file" id="Entidad_ArchivoCarga_` + fecActual +`" name="Entidad.ArchivoCarga" type="file" required="required">`)
        let valorInput = input.clone();
        //valorInput.find('input').val(data.TrnPctHpMwFija);
        td.append(valorInput)
        trRowAdd.append(tdNombreArchivo);
        trRowAdd.append(td);
        tbody.append(trRowAdd);
    }
    
}

function limpiarMensajeError() {
    $('#sError').html('');
}

function cancelarCargaReportePR21() {
    $('#Entidad_GPSCodi').val('');
    $('#Entidad_CodEmpresa').val('');
    $('#Entidad_CodEmpresa').change();
    $('#Entidad_CodCentral').change();
    $('#Entidad_FechaCargaInicio').val('');
    $('#Entidad_FechaCargaFin').val('');
}

function cancelarCargaArchivoExterno() {
    $('#Entidad_GPSCodi').val('');
    $('#Entidad_ArchivoCarga').val('');
}
