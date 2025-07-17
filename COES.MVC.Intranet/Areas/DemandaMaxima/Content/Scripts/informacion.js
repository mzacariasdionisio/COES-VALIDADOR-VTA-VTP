var controlador = siteRoot + 'demandaMaxima/Informacion/';
var uploader;
var totNoNumero = 0;
var totLimSup = 0;
var totLimInf = 0;
var listErrores = [];
var listDescripErrores = ["BLANCO", "NÚMERO", "No es número", "Valor negativo", "Supera el valor máximo"];
var listValInf = [];
var listValSup = [];
var validaInicial = true;
var hot;
var hotOptions;
var evtHot;

$(function () {

    $('#txtMes').Zebra_DatePicker({
        format: 'm Y'
    });

    $('#txtMesFin').Zebra_DatePicker({
        format: 'm Y'
    });

    $('#cbEmpresa').multipleSelect({
        width: '200px',
        filter: true,
        onClick: function (view) {
        },
        onClose: function (view) {
        }
    });

    $('#btnConsultar').click(function () {
        //- pr16.HDT - Inicio 01/04/2018: Cambio para atender el requerimiento.
        //pintarPaginado();
        //pintarBusqueda(1);
        var pasaValidacion = true;
        if ($('#cbPeriodoSicli').is(":visible")) {
            if ($('#cbPeriodoSicli').val() == null || $('#cbPeriodoSicli').val() == '') {
                alert('No ha seleccionado ningún periodo Sicli');
                pasaValidacion = false;
            }
        }
        if (pasaValidacion) {
            pintarPaginado();
            pintarBusqueda(1);
        }
        //- HDT Fin
    });

    $('#btnExportar').click(function () {
        //- pr16.HDT - Inicio 01/04/2018: Cambio para atender el requerimiento.
        //exportarInformacion();
        //pintarBusqueda(1);
        var pasaValidacion = true;
        if ($('#cbPeriodoSicli').is(":visible")) {
            if ($('#cbPeriodoSicli').val() == null || $('#cbPeriodoSicli').val() == '') {
                alert('No ha seleccionado ningún periodo Sicli');
                pasaValidacion = false;
            }
        }
        if (pasaValidacion) {
            exportarInformacion();
        }
        //- HDT Fin
    });

    $("#cbTipoEmpresa").change(function () {
        changeTipoEmpresa();
    });

    //- pr16.HDT - Inicio 01/04/2018: Cambio para atender el requerimiento.
    $("#cbPeriodoIni").change(function () {
        changePeriodo();
    });

    if ($("#cbPeriodoIni").val() != null) {
        changePeriodo();
    }

    $("#cbNivel").change(function () {
        changeNivel();
    });

    asegurarCoherenciaVisibilidadPerSicli();
    //- HDT fin
});

//- pr16.HDT - Inicio 01/04/2018: Cambio para atender el requerimiento.
function changePeriodo() {

    if ($("#cbPeriodoIni").val() != null && $("#cbPeriodoIni").val() != '') {

        $.ajax({
            type: "POST",
            url: controlador + "obtenerMaximaDemanda",
            data: {
                periodoInicial: $("#cbPeriodoIni").val()
            },
            dataType: "json",
            success: function (resultado) {
                $("#fechaMD").val(resultado.FechaMD);
                $("#horaMD").val(resultado.HoraMD);
                $("#valorMD").val(resultado.ValorMD);
            },
            error: function (xhr) {
                mostrarError("Ocurrio un problema >> readyState: " + xhr.readyState + " | status: " + xhr.status + " | responseText: " + xhr.responseText);
            }
        });
    }
}

//- pr16.HDT - Inicio 01/04/2018: Cambio para atender el requerimiento.
function asegurarCoherenciaVisibilidadPerSicli() {

    if ($('#cbTipoEmpresa').val() == "2") {
        $("#tdPeriodoSicliEtiqueta").css('display', 'none');
        $("#cbPeriodoSicli").css('display', 'none');
    }
    else if ($('#cbTipoEmpresa').val() == "4") {
        $("#tdPeriodoSicliEtiqueta").css('display', 'block');
        $("#cbPeriodoSicli").css('display', 'block');
    }
}

pintarPaginado = function () {

    var inp1 = document.getElementById('cbPeriodoIni').value;
    var inp2 = document.getElementById('cbPeriodoSicli').value;
    // convierte las fechas a yyyymmdd
    //tmp = inp1.split('/');
    //fini = tmp[2] + tmp[1] + tmp[0];
    //tmp = inp2.split('/');
    //ffin = tmp[2] + tmp[1] + tmp[0];

    var validacion = false;
    if ($('#cbPeriodoSicli').is(":visible")) {
        // convierte las fechas a yyyymmdd
        var tmp = inp1.split('/');
        fini = tmp[2] + tmp[1] + tmp[0];
        tmp = inp2.split('/');
        ffin = tmp[2] + tmp[1] + tmp[0];

        if (fini > ffin) {
            validacion = true;
        }
    } else {
        inp2 = 0;
    }
    // la comparación
    //if (fini > ffin) {
    if (validacion) {
        alert("El periodo inicial no puede ser mayor al periodo final");
    } else {
        var empresa = $('#cbEmpresa').multipleSelect('getSelects');
        if (empresa == "[object Object]") empresa = "";
        $('#hfEmpresa').val(empresa);
        if ($('#hfCheck').val() == "") {
            $('#hfCheck').val(0);
        }
        if ($('#cbPeriodoIni').val() != "" && $('#cbPeriodoSicli').val() != "") {
            /*
            $.ajax({
                type: 'POST',
                url: controlador + "paginado",
                data: {
                    empresas: $('#hfEmpresa').val(),
                    tipos: $('#cbTipoEmpresa').val(),
                    ini: $('#cbPeriodoIni').val(),
                    //periodoSicli: $('#cbPeriodoSicli').val(),
                    periodoSicli: inp2,
                    max: $('#hfCheck').val(),
                    nivel: $('#cbNivel').val()
                },
                success: function (evt) {
                    $('#paginado').html(evt);
                    mostrarPaginado();
                },
                error: function () {
                    alert("Ha ocurrido un error");
                }
            });*/

        }
        else {
            mostrarAlerta("Seleccione rango de fechas del periodo.");
        }
    }
}

function changeTipoEmpresa() {
    asegurarCoherenciaVisibilidadPerSicli();

    var x = document.getElementById("cbTipoEmpresa").value;
    if (x != "") {
        $.ajax({
            type: 'POST',
            url: controlador + "ObtenerListaEmpresas",
            dataType: 'json',
            data: {
                tipoemprcodi: x
            },
            success: function (result) {
                $('#cbEmpresa').empty().multipleSelect('refresh');
                $(result).each(function (i, v) { // indice, valor

                    var $option = $('<option></option>')
                        .attr('value', v.Emprcodi)
                        .text(v.Etiqueta)
                        .prop('selected', false);
                    $('#cbEmpresa').append($option).change();
                })
                $('#cbEmpresa').multipleSelect('refresh')
            },
            error: function () {
                alert("Error al cargar la Lista de Empresas.");
            }
        });
    }
    else {
        $('#cbEmpresa').empty().multipleSelect('refresh');
    }
}

function changeNivel() {
    var x = document.getElementById("cbNivel").value;
    if (x == "30") {//Mediciones cada 30 min desactivar Máxima demanda        
        document.getElementById("lblMaxDemanda").style.display = "none";
        document.getElementById("lblHP").style.display = "none";
        document.getElementById("lblHFP").style.display = "none";
        $('#hfCheck').val(0)
        document.getElementById("chkMaxima").checked = false;
    }
    else {
        document.getElementById("lblMaxDemanda").style.display = "";
    }
}

function toggleCheckbox(element) {
    var chk = element.checked;
    if (chk == true) {
        document.getElementById("lblHP").style.display = "";
        document.getElementById("lblHFP").style.display = "";
        $('#hfCheck').val(1);
    }
    else {
        document.getElementById("lblHP").style.display = "none";
        document.getElementById("lblHFP").style.display = "none";
        $('#hfCheck').val(0);
    }
}


function mostrarExito(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-exito');
}

function mostrarError(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-error');
}

function mostrarAlerta(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-alert');
}

function mostrarMensaje(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-message');
}

function mostrarPaginado() {
    /*$.ajax({
        type: 'POST',
        url: controlador + 'paginado',
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            alert("Error al cargar el paginado");
        }
    });*/
}

function pintarBusqueda(id) {
    var inp1 = document.getElementById('cbPeriodoIni').value;
    var inp2 = document.getElementById('cbPeriodoSicli').value;
    // convierte las fechas a yyyymmdd
    //tmp = inp1.split('/');
    //fini = tmp[2] + tmp[1] + tmp[0];
    //tmp = inp2.split('/');
    //ffin = tmp[2] + tmp[1] + tmp[0];
    var validacion = false;
    if ($('#cbPeriodoSicli').is(":visible")) {
        // convierte las fechas a yyyymmdd
        var tmp = inp1.split('/');
        fini = tmp[2] + tmp[1] + tmp[0];
        tmp = inp2.split('/');
        ffin = tmp[2] + tmp[1] + tmp[0];

        if (fini > ffin) {
            validacion = true;
        }
    } else {
        inp2 = 0;
    }


    // la comparación
    //if (fini > ffin) {
    if (validacion) {
        alert("El periodo inicial no puede ser mayor al periodo final");
    } else {
        var control = "";
        if ($('#cbNivel').val() == "15") {
            control = "ListarReporteInformacion15min";
        } else {
            control = "ListarReporteInformacion30min";
        }
        var empresa = $('#cbEmpresa').multipleSelect('getSelects');
        if (empresa == "[object Object]") empresa = "";
        $('#hfEmpresa').val(empresa);
        if ($('#hfCheck').val() == "") {
            $('#hfCheck').val(0)
        }
        if ($('#cbPeriodoIni').val() != "" && $('#cbPeriodoSicli').val() != "") {

            $.ajax({
                type: 'POST',
                url: controlador + control,
                data: {
                    empresas: $('#hfEmpresa').val(),
                    tipos: $('#cbTipoEmpresa').val(),
                    ini: $('#cbPeriodoIni').val(),
                    //periodoSicli: $('#cbPeriodoSicli').val(),
                    periodoSicli: inp2,
                    max: $('#hfCheck').val(),
                    nroPagina: id
                },
                success: function (evt) {
                    $('#listado').css("width", $('#mainLayout').width() + "px");
                    $('#listado').html(evt);
                    $('#tabla').dataTable({
                        "scrollY": 480,
                        "scrollX": true,
                        "sDom": 't',
                        "ordering": false,
                        "bDestroy": true,
                        "bPaginate": false,
                        "iDisplayLength": 50
                    });
                },
                error: function () {
                    alert("Error al obtener la consulta");
                }
            });
        }
        else {
            mostrarAlerta("Seleccione rango de fechas del periodo.");
        }
    }
}

function exportarInformacion() {
    //formato = 1 [Excel]
    var formato = "1";
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "";
    $('#hfEmpresa').val(empresa);
    var cumplimie = $('#cbCumplimiento').multipleSelect('getSelects');
    if (cumplimie == "[object Object]") cumplimie = "";
    $('#hfCumplimiento').val(cumplimie);

    var periodoSicli = $('#cbPeriodoSicli').val();

    if (!($('#cbPeriodoSicli').is(":visible"))) {
        periodoSicli = 0;
    }

    if ($('#cbPeriodoIni').val() != "" && $('#cbPeriodoSicli').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'exportar',
            data: {
                empresas: $('#hfEmpresa').val(),
                tipos: $('#cbTipoEmpresa').val(),
                ini: $('#cbPeriodoIni').val(),
                //periodoSicli: $('#cbPeriodoSicli').val(),
                periodoSicli: periodoSicli,
                nivel: $('#cbNivel').val(),
                max: $('#hfCheck').val()
            },
            dataType: 'json',
            success: function (result) {
                if (result != -1) {
                    document.location.href = controlador + 'descargar?formato=' + formato + '&file=' + result
                }
                else {
                    mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
            }
        });
    }
    else {
        mostrarAlerta("Seleccione rango de fechas del periodo.");
    }
}