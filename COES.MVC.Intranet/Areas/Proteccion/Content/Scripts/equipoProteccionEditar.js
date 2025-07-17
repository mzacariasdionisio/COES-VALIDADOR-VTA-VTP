//Declarando las variables 
let controlador = siteRoot + 'EquipoProteccion/';
let controladorUbicacionCoes = siteRoot + 'UbicacionCOES/';
let controladorEquipoCoes = siteRoot + 'EquipoCoes/';
let hot;
let hotOptions;
let evtHot;
let mensajeSalir = "¿Está seguro de salir del formulario?";
$(function () {



    let accion = $("#accion").val();
    let e = $("#equicodi").val();

    inputSoloNumerosDecimales([
        'tensionRele',
        'rtcp',
        'rtcs',
        'rttp',
        'rtts',
        'asTension',
        'asAngulo',
        'asFrecuencia',
        'controlUmbral',
        'astU',
        'astT',
        'astUU',
        'astTT',
    ]);
   
    switch (accion) {
        case "EDITAR":
            $("#tituloEditar").html("Actualización de Relé (" + e + ")")
            if ($("#mCalculo").val() == "") {
                $("#hdnMemoricaCalculo").val(0);
            } else {
                $("#hdnMemoricaCalculo").val(1);
            }
            $("#idProyecto").prop('disabled', true);
            break;
        case "NUEVO":
            $("#tituloEditar").html("Creación de Relé");
            $("#hdnMemoricaCalculo").val(0);
            break;
        case "CONSULTAR":
            $("#tituloEditar").html("Consulta de Relé (" + e + ")");
            break;
    }

    if (accion == "NUEVO") {
        $("#divLineaTiempo").hide();
    }

    activarChecks();
    activarTipoUso($("#idTipoUso").val());

    $('#idSubestacion').change(function () {
        let idSubEstacion = $(this).val();

        if ($('#idSubestacion').val() == '') {
            $("#fechaCreacion").val('');
            $("#fechaActualizacion").val('');
            $("#zona").val('');
            $("#idCelda").empty();
            $("#idCelda").append('<option value="">SELECCIONAR</option>');

            $("#asInterruptor").empty();
            $("#asInterruptor").append('<option value="">SELECCIONAR</option>');
            $("#codigoInterruptor").empty();
            $("#codigoInterruptor").append('<option value="">SELECCIONAR</option>');
            return;
        }

        consultarCelda(idSubEstacion, '');
        consultarInterruptor(idSubEstacion);
        seleccionarSubEstacion(idSubEstacion);

    });
    $('#idProyecto').change(function () {
        let idProyecto = $(this).val();
        consultarProyecto(idProyecto)
    });

    $('#asActivo').change(function () {
        if ($(this).is(':checked')) {
            activarDesactivarControlesAS(true)
        } else {
            $("#asInterruptor").val("");
            $("#asTension").val("");
            $("#asAngulo").val("");
            $("#asFrecuencia").val("");
            activarDesactivarControlesAS(false)
        }
    });
    $('#astActivo').change(function () {
        if ($(this).is(':checked')) {
            activarDesactivarControlesAST(true);
        } else {
            $("#astU").val("");
            $("#astT").val("");
            $("#astUU").val("");
            $("#astTT").val("");
            activarDesactivarControlesAST(false);
        }
    });
    $('#checkUmbral').change(function () {
        if ($(this).is(':checked')) {
            activarDesactivarControlesUmbral(true);
        } else {
            $("#controlUmbral").val("");
            activarDesactivarControlesUmbral(false);
        }
    });

    $('#checkAsaPmu').change(function () {
        if ($(this).is(':checked')) {
            activarDesactivarControlesPMU(true);
        } else {
            $("#controlAsaPmu").val("");
            activarDesactivarControlesPMU(false);
        }
    });

    $('#idTipoUso').change(function () {

        let idTipoUso = $(this).val();
        activarTipoUso(idTipoUso);

    });

    $('#btnAgregarCelda').click(function () {
        const url = siteRoot + 'Proteccion/EquipoCoes/Index';
        window.open(url, '_blank');
    });

    $('#btnActualizarCelda').click(function () {
        let idCeldaHdn = $('#idCelda').val()
        consultarCelda($('#idSubestacion').val(), idCeldaHdn);
    });

    $('#btnAgregarSE').click(function () {
        const url = siteRoot + 'Proteccion/UbicacionCOES/Index';
        window.open(url, '_blank');
    });

    $('#btnActualizarSE').click(function () {
        let idSubestacionHdn = $('#idSubestacion').val()
        consultarSubestacion(idSubestacionHdn);
    });

    consultarProyecto($("#idProyecto").val());

    consultarLineaTiempo(e);

    if (accion == "CONSULTAR") {
        soloConsulta();
    }

    $('#btnGrabar').click(function () {
        guardar($("#equicodi").val());
    });

    $('#btnRegresar').click(function () {
        regresar();
    });

    let fullDate = new Date();
    let twoDigitMonth = ((fullDate.getMonth().length) == 1) ? '0' + (fullDate.getMonth() + 1) : (fullDate.getMonth() + 1);
    let sFecha = fullDate.getFullYear().toString() + twoDigitMonth.toString() + fullDate.getDate().toString() + fullDate.getHours().toString() + fullDate.getMinutes().toString() + fullDate.getSeconds().toString();
    let uploaderP1 = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnUpload',
        container: document.getElementById('container'),
        multipart_params: {
            "equicodi": $('#equicodi').val(),
        },
        url: siteRoot + 'proteccion/equipoproteccion/upload',
        flash_swf_url: '~/Content/Scripts/Moxie.swf',
        silverlight_xap_url: '~/Content/Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '30mb',
            mime_types: [
                { title: "Archivos Zip .zip", extensions: "zip" },
                { title: "Archivos Pdf .pdf", extensions: "pdf" },
                { title: "Archivos Rar .rar", extensions: "rar" },
                { title: "Archivos Excel .xlsx", extensions: "xlsx" },
                { title: "Archivos Excel .xls", extensions: "xls" },
                { title: "Archivos Word .docx", extensions: "docx" },
                { title: "Archivos Word .doc", extensions: "doc" },
                { title: "Archivos Imagen .jpg", extensions: "jpg" },
                { title: "Archivos Imagen .gif", extensions: "gif" }
            ]
        },

        init: {
            PostInit: function () {
                document.getElementById('btnCargar').onclick = function () {
                    if (uploaderP1.files.length > 0) {
                        uploaderP1.start();
                    }
                    else
                        loadValidacionFile("Seleccione archivo.");
                    return false;
                };
            },
            FilesAdded: function (up, files) {
                if (uploaderP1.files.length == 2) {
                    uploaderP1.removeFile(uploaderP1.files[0]);
                }
                plupload.each(files, function (file) {
                    $('#mCalculo').val(file.name);


                });
                up.refresh();
            },
            UploadProgress: function (up, file) {
            },

            UploadComplete: function (up, file) {
                console.log("UploadComplete", up)
            },
            Error: function (up, err) {
                alert("Ocurrió un error al cargar el archivo")
                loadValidacionFile(err.code + "-" + err.message);
            }
        }
    });
    uploaderP1.init();
    uploaderP1.bind('FileUploaded', function (up, file, response) {
        var serverResponse = JSON.parse(response.response);

        if (serverResponse.estado == 1) {
            $("#hdnMemoricaCalculo").val(1);
            $('#mCalculo').val(serverResponse.nombreArchivoTexto);
            $('#hdnMemoricaCalculoText').val(serverResponse.nombreArchivo);
            hdnMemoricaCalculoText
            alert("Archivo subido con éxito.");
        } else {
            $("#hdnMemoricaCalculo").val(0);
            alert("Error al subir el archivo");
        }
    });

});

let consultarLineaTiempo = function (e) {
    setTimeout(function () {
        $.ajax({
            type: 'POST',
            url: controlador + 'LineaTiempo',
            data: {
                equicodi: e
            },
            success: function (evt) {
                $('#lineaTiempo').html(evt);
                $('.timeline').timeline({
                    forceVerticalMode: 800,
                    mode: 'vertical',
                    visibleItems: 4
                });

            },
            error: function () {
                mostrarMensaje('Ha ocurrido un error.');
            }
        });
    }, 100);
};


function validarGuardar() {
    let campos = [];
    if ($('#idCelda').val() == '') campos.push('Celda o Generador');
    if ($('#idProyecto').val() == '') campos.push('Proyecto');
    if ($('#codigoRele').val() == '') campos.push('Nombre');
    if ($('#idTitular').val() == '' || $('#idTitular').val() == '0') campos.push('Titular');
    if ($('#tensionRele').val() == '') campos.push('Tensión (KV)');
    if ($('#idSistemaRele').val() == '') campos.push('Sistema de Relé');
    if ($('#idMarca').val() == '') campos.push('Marca');
    if ($('#modeloRele').val() == '') campos.push('Modelo');
    if ($('#idTipoUso').val() == '') campos.push('Tipo de uso');

    if (campos.length > 0) {
        
        mensajeValidador(campos);
        return false;
    }

    return true;
}

function mensajeValidador(campos) {
    let texto = "Los campos: \n";

    for (const campo of campos) {
        texto += campo + "(*) \n";
    }

    alert(texto + " son requeridos");
}

let loadValidacionFile = function (mensaje) {
    alert(mensaje);
}


let regresar = function () {
    if ($("#accion").val() != "CONSULTAR") {
        if (confirm(mensajeSalir)) {
            window.location.href = siteRoot + 'Proteccion/equipoproteccion/Index';
        } 
    } else {
        window.location.href = siteRoot + 'Proteccion/equipoproteccion/Index';
    }
}


let limpiarArray = function (arrayControles) {
    for (let control of arrayControles) {
        $('#' + control).val('');
    }
}

function obtenerValorCheck(control) {
    if ($('#' + control).prop('checked')) {
        return "S"
    }
    return "N"
}

let guardar = function (id) {
    if (validarGuardar()) {
        let idSubestacion = $('#idSubestacion').val();
        let zona = $('#zona').val();
        let fechaCreacion = $('#fechaCreacion').val();
        let fechaActualizacion = $('#fechaActualizacion').val();
        let idCelda = $('#idCelda').val();
        let idProyecto = $('#idProyecto').val();
        let codigoRele = $('#codigoRele').val();
        let fechaRele = $('#fechaRele').val();
        let idEstado = $('#idEstado').val();

        let idTitular = $('#idTitular').val();
        let tensionRele = $('#tensionRele').val();
        let idSistemaRele = $('#idSistemaRele').val();
        let idMarca = $('#idMarca').val();
        let modeloRele = $('#modeloRele').val();

        let idTipoUso = $('#idTipoUso').val();
        let rtcp = $('#rtcp').val();
        let rtcs = $('#rtcs').val();
        let rttp = $('#rttp').val();
        let rtts = $('#rtts').val();
        let pCoordinables = $('#pCoordinables').val();

        let mCalculo = $('#hdnMemoricaCalculoText').val();

        let asActivo = obtenerValorCheck('asActivo');
        let checkUmbral = obtenerValorCheck('checkUmbral');
        let astActivo = obtenerValorCheck('astActivo');
        let checkAsaPmu = obtenerValorCheck('checkAsaPmu');

        let codigoInterruptor = $('#codigoInterruptor').val();
        let asInterruptor = $('#asInterruptor').val();
        let asTension = $('#asTension').val();
        let asAngulo = $('#asAngulo').val();
        let asFrecuencia = $('#asFrecuencia').val();
        let controlUmbral = $('#controlUmbral').val();
        let astU = $('#astU').val();
        let astT = $('#astT').val();
        let astUU = $('#astUU').val();
        let astTT = $('#astTT').val();
        let controlAsaPmu = $('#controlAsaPmu').val();
        let accion = $('#accion').val();
        let equicodi = $('#equicodi').val();
        
        let idReleTorcionalImpl = $('#idReleTorcionalImpl').val();
        
        let medidaMitigacion = $('#medidaMitigacion').val();
        let idMandoSincronizado = $('#idMandoSincronizado').val();

        let pmuAccion = $('#pmuAccion').val();
        let idRelePmuImpl = $('#idRelePmuImpl').val();
        

        let textoConfirmación = '¿Está seguro de agregar Relé?'
        if (accion =="EDITAR") {
            textoConfirmación = '¿Está seguro de editar Relé?'
        }

        if (confirm(textoConfirmación)) {

            let datos = {
                idSubestacion: idSubestacion,
                zona: zona,
                fechaCreacion: fechaCreacion,
                fechaActualizacion: fechaActualizacion,
                idCelda: idCelda,
                idProyecto: idProyecto,
                codigoRele: codigoRele,
                fechaRele: fechaRele,
                idEstado: idEstado,
                idTitular: idTitular,
                tensionRele: tensionRele,
                idSistemaRele: idSistemaRele,
                idMarca: idMarca,
                modeloRele: modeloRele,
                idTipoUso: idTipoUso,
                rtcp: rtcp,
                rtcs: rtcs,
                rttp: rttp,
                rtts: rtts,
                pCoordinables: pCoordinables,
                mCalculo: mCalculo,
                asActivo: asActivo,
                asInterruptor: asInterruptor,
                asTension: asTension,
                asAngulo: asAngulo,
                asFrecuencia: asFrecuencia,
                checkUmbral: checkUmbral,
                controlUmbral: controlUmbral,
                astActivo: astActivo,
                astU: astU,
                astT: astT,
                astUU: astUU,
                astTT: astTT,
                checkAsaPmu: checkAsaPmu,
                controlAsaPmu: controlAsaPmu,
                accion: accion,
                equicodi:id,
                medidaMitigacion: medidaMitigacion,
                idReleTorcionalImpl: idReleTorcionalImpl,
                idMandoSincronizado: idMandoSincronizado,
                codigoInterruptor: codigoInterruptor,
                pmuAccion: pmuAccion,
                idRelePmuImpl: idRelePmuImpl

            };

            $.ajax({
                type: 'POST',
                url: controlador + 'GuardarEquipo',
                dataType: 'json',
                data: datos,
                cache: false,
                success: function (resultado) {
                    if (resultado == "") {
                        window.location.href = siteRoot + 'Proteccion/equipoproteccion/Index';
                    } else
                        alert(resultado)
                       
                },
                error: function () {
                    mostrarError();
                }
            });
        }
    }
};

let mensajeExito = function () {
    $('#popupFormatoEnviadoOk').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
}

let cerrarpopupFormatoEnviadoOk = function () {
    $('#popupFormatoEnviadoOk').bPopup().close();
}

let mostrarError = function () {
    $('#mensaje').removeClass("action-exito");
    $('#mensaje').addClass("action-error");
    $('#mensaje').text("Ha ocurrido un error");
    $('#mensaje').css("display", "block");
};


let consultarCelda = function (idSubEstacion, idCeldaHdn) {
    setTimeout(function () {
        $.ajax({
            type: 'POST',
            url: controlador + 'ListaCelda',
            data: { idSubEstacion: idSubEstacion },
            success: function (evt) {
                let seleccion = $('#idCelda');
                seleccion.empty();
                seleccion.append('<option value="">SELECCIONAR</option>');
                $.each(evt, function (index, item) {
                    seleccion.append('<option value="' + item.Equicodi + '">' + item.Equinomb + '</option>');
                });
                if (idCeldaHdn != "") {
                    $('#idCelda').val(idCeldaHdn);
                }
            },
            error: function () {
                mostrarMensaje('Ha ocurrido un error.');
            }
        });
    }, 100);
};

let consultarSubestacion = function (idSubestacionHdn) {
    setTimeout(function () {
        $.ajax({
            type: 'POST',
            url: controlador + 'ConsultarSubestacion',
            success: function (evt) {
                let select = $('#idSubestacion');
                select.empty();
                select.append('<option value="">SELECCIONAR</option>');
                $.each(evt.listaSubestacion, function (index, item) {
                    select.append('<option value="' + item.Areacodi + '">' + item.Areanomb + '</option>');
                });
                if (idSubestacionHdn != "") {
                    $('#idSubestacion').val(idSubestacionHdn);
                }
            },
            error: function () {
                mostrarMensaje('Ha ocurrido un error.');
            }
        });
    }, 100);
};

let consultarInterruptor = function (idSubEstacion) {
    setTimeout(function () {
        $.ajax({
            type: 'POST',
            url: controlador + 'ListaInterruptor',
            data: { idSubEstacion: idSubEstacion },
            success: function (evt) {
                let seleccion = $('#codigoInterruptor');
                seleccion.empty();
                seleccion.append('<option value="">SELECCIONAR</option>');
                $.each(evt, function (index, item) {
                    seleccion.append('<option value="' + item.Equicodi + '">' + item.Equinomb + '</option>');
                });

                let select = $('#asInterruptor');
                select.empty();
                select.append('<option value="">SELECCIONAR</option>');
                $.each(evt, function (index, item) {
                    select.append('<option value="' + item.Equicodi + '">' + item.Equinomb + '</option>');
                });
            },
            error: function () {
                mostrarMensaje('Ha ocurrido un error.');
            }
        });
    }, 100);
};

let seleccionarSubEstacion = function (idSubEstacion) {

    setTimeout(function () {
        $.ajax({
            type: 'POST',
            url: controlador + 'SeleccionarSubestacion',
            data: { idSubEstacion: idSubEstacion },
            success: function (evt) {
                $("#fechaCreacion").val(evt.Epareafeccreacion);
                $("#fechaActualizacion").val(evt.Epareafecmodificacion);
                $("#zona").val(evt.Zona);
            },
            error: function () {
                mostrarMensaje('Ha ocurrido un error.');
            }
        });
    }, 100);

};

function mostrarMensaje(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-message');
}

function activarDesactivarControlesAS(activo) {
    $("#asInterruptor").prop('disabled', !activo);
    $("#asTension").prop('disabled', !activo);
    $("#asAngulo").prop('disabled', !activo);
    $("#asFrecuencia").prop('disabled', !activo);
}

function activarDesactivarControlesUmbral(activo) {
    $("#controlUmbral").prop('disabled', !activo);
}

function activarDesactivarControlesAST(activo) {
    $("#astU").prop('disabled', !activo);
    $("#astT").prop('disabled', !activo);
    $("#astUU").prop('disabled', !activo);
    $("#astTT").prop('disabled', !activo);
}

function activarDesactivarControlesPMU(activo) {
    $("#controlAsaPmu").prop('disabled', !activo);
}

function activarChecks() {

    if ($('#asActivo').prop('checked')) {
        activarDesactivarControlesAS(true)
    } else {
        activarDesactivarControlesAS(false)
    }

    if ($('#astActivo').prop('checked')) {
        activarDesactivarControlesAST(true)
    } else {
        activarDesactivarControlesAST(false)
    }

    if ($('#checkUmbral').prop('checked')) {
        activarDesactivarControlesUmbral(true);
    } else {
        activarDesactivarControlesUmbral(false);
    }

    if ($('#checkAsaPmu').prop('checked')) {
        activarDesactivarControlesPMU(true);
    } else {
        activarDesactivarControlesPMU(false);
    }
}

function activarTipoUso(idTipoUso) {
    switch (idTipoUso) {
        case "101":
            $("#divGeneralRele").show();
            $("#divMandoRele").hide();
            $("#divTorcionalRele").hide();
            $("#divPmuRele").hide();
            break;
        case "102":
            $("#divMandoRele").show();
            $("#divGeneralRele").hide();
            $("#divTorcionalRele").hide();
            $("#divPmuRele").hide();
            break;
        case "103":
            $("#divTorcionalRele").show();
            $("#divMandoRele").hide();
            $("#divGeneralRele").hide();
            $("#divPmuRele").hide();
            break;
        case "104":
            $("#divPmuRele").show();
            $("#divMandoRele").hide();
            $("#divTorcionalRele").hide();
            $("#divGeneralRele").hide();
            break;
        case "":
            $("#divGeneralRele").hide();
            $("#divMandoRele").hide();
            $("#divTorcionalRele").hide();
            $("#divPmuRele").hide();
            break;
    }
}

function soloConsulta() {
    desactivarControles([
        'idSubestacion',
        'idCelda',
        'idProyecto',
        'fechaRele',
        'codigoRele',
        'idTitular',
        'modeloRele',
        'tensionRele',
        'idSistemaRele',
        'idMarca',
        'idTipoUso',
        'rtcp',
        'rtcs',
        'rttp',
        'rtts',
        'pCoordinables',
        'asActivo',
        'asInterruptor',
        'asTension',
        'asAngulo',
        'asFrecuencia',
        'checkUmbral',
        'controlUmbral',
        'astActivo',
        'astU',
        'astT',
        'astUU',
        'astTT',
        'checkAsaPmu',
        'controlAsaPmu',
        'codigoInterruptor',
        'idMandoSincronizado',
        'medidaMitigacion',
        'idReleTorcionalImpl',
        'pmuAccion',
        'idRelePmuImpl',
        'mCalculo'
    ]);
    $("#btnUpload").hide();
    $("#btnCargar").hide();
    $("#btnGrabar").hide();
    $("#btnActualizarCelda").hide();
    $("#btnAgregarCelda").hide();
    $("#btnActualizarSE").hide();
    $("#btnAgregarSE").hide();
}

function desactivarControles(arrayControles) {
    for (let control of arrayControles) {
        $('#' + control).prop('disabled', true);
    }
}

function consultarProyecto(idProyecto) {
        if (idProyecto == 0) {
            $("#fechaRele").val('');
            return;
        }
        setTimeout(function () {
            $.ajax({
                type: 'POST',
                url: controlador + 'SeleccionarProyecto',
                data: { idProyecto: idProyecto },
                success: function (evt) {
                    if (evt) {
                        $("#fechaRele").val(evt.Epproyfecregistro);
                    } else {
                        $("#fechaRele").val('');
                    }
                },
                error: function () {
                    mostrarMensaje('Ha ocurrido un error.');
                }
            });
        }, 100); 
}

function habilitarInput(id) {
    $('#' + id).css('cssText', 'background-color: #FFFFFF!important');
    $('#' + id).prop("disabled", false);
}
function deshabilitarInput(id) {
    $('#' + id).css('cssText', 'background-color: #F2F4F3!important');
    $('#' + id).prop("disabled", true);
}
function abrirPopup(contentPopup) {
    setTimeout(function () {
        $("#" + contentPopup).bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
};

function cerrarPopup(id) {
    $("#" + id).bPopup().close()
}

function day_of_the_month(d) {
    return (d.getDate() < 10 ? '0' : '') + d.getDate();
}