var seccionUbicacionCCTT = 1;
var seccionEquipamientoCCTT = 3;
var catPerfil = 16;
var catPreFacltibilidad = 17;
var catFacltibilidad = 18;
var catEstudDef = 19;
var catEia = 20;
var catPodCalInf = 54;
var catPodCalSup = 55;
var catCComb = 56;
var catCTratComb = 57;
var catCTranspComb = 58;
var catCVarNComb = 59;
var catCInvIni = 60;
var catRendPl = 61;
var catConsEspCon = 62;
$(function () {
    //$('#txtFechaAdjudicacionActual').val(obtenerFechaActual());
    //$('#fechaInicioContruccion').val(obtenerFechaActualMesAnio());
    //console.log('fecha ini',$('#fechaInicioContruccion').val());
    //$('#fechaOperacionComercial').val(obtenerFechaActualMesAnio());

    $('#txtFechaAdjudicacionActual').Zebra_DatePicker({
        format: 'd/m/Y',
        onSelect: function () {
            cambiosRealizados = true; // Marcar que hubo un cambio
        }
    });
    $('#fechaInicioContruccion').Zebra_DatePicker({
        format: 'm/Y',
        onSelect: function () {
            cambiosRealizados = true; // Marcar que hubo un cambio
        }
    });
    $('#fechaOperacionComercial').Zebra_DatePicker({
        format: 'm/Y',
        onSelect: function () {
            cambiosRealizados = true; // Marcar que hubo un cambio
        }
    });
    cargarDepartamentos();
    var ruta_interna = obtenerRutaProyecto();  
    crearUploaderGeneral('btnSubirUbicacionCCTT', '#tablaUbicacionCCTT', seccionUbicacionCCTT, null, ruta_interna);
    crearUploaderGeneral('btnSubirFormatoEquipamiento', '#tablaEquipamientoFile', seccionEquipamientoCCTT, null, ruta_interna);
    //cargarCatalogo(catPropietario, "#propietarioSelect");
    //cargarCatalogo(catConcesionActual, "#concesionActSelect");
    //cargarCatalogo(catCombustible, "#combusTipoNombreSelect");

    //cargarCatalogoSubestacion("#txtNombreSubestacionTC");
    //cargarCatalogo(catPerfil, "#perfilSelectNE");
    //cargarCatalogo(catPreFacltibilidad, "#prefactibilidadNE");
    //cargarCatalogo(catFacltibilidad, "#factibilidadNE");
    //cargarCatalogo(catEstudDef, "#estudioDefiniticoNE");
    //cargarCatalogo(catEia, "#EiaNE");

    //cargarCatalogo(catPodCalInf, "#undPoderCalorifInferiorSelect");
    //cargarCatalogo(catPodCalSup, "#undPoderCalorifSuperiorSelect");
    //cargarCatalogo(catCComb, "#undCostoCombustibleSelect");
    //cargarCatalogo(catCTratComb, "#undCostoTratamCombustibleSelect");
    //cargarCatalogo(catCTranspComb, "#undCostoTranspCombustibleSelect");
    //cargarCatalogo(catCVarNComb, "#undCostoVarNoCombustibleSelect");
    //cargarCatalogo(catCInvIni, "#undCostoInversionInicioSelect");
    //cargarCatalogo(catRendPl, "#undRendimientoPlantaSelect");
    //cargarCatalogo(catConsEspCon, "#undConsEspecCondicionesSelect");

    var formularioa = document.getElementById('TFichaA');
    formularioa.addEventListener('change', function () {
        cambiosRealizados = true;
    });
    
    $('#btnGrabarFichaCCTTA').on('click', function () {
        grabarFichaCCTTA();
    });
});


function cargarDepartamentos() {
    $.ajax({
        type: 'POST',
        url: controladorFichas + 'ListarDepartamentos',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(''),
        success: function (eData) {
            console.log(eData);
            cargarListaDepartamento(eData);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarDatosA() {
    limpiarCombos('#propietarioSelect, #concesionActSelect, #combusTipoNombreSelect, #txtNombreSubestacionTC, #perfilSelectNE, #prefactibilidadNE, #factibilidadNE, #estudioDefiniticoNE, #EiaNE, #undPoderCalorifInferiorSelect, #undPoderCalorifSuperiorSelect, #undCostoCombustibleSelect, #undCostoTratamCombustibleSelect, #undCostoTranspCombustibleSelect, #undCostoVarNoCombustibleSelect, #undCostoInversionInicioSelect, #undRendimientoPlantaSelect, #undConsEspecCondicionesSelect');

    console.log("Editar");
    if (modoModel == "editar" || modoModel == "consultar") {
        var idProyecto = $("#txtPoyCodi").val();
        $.ajax({
            type: 'POST',
            url: controladorFichas + 'GetRegHojaCCTTA',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: idProyecto }),
            
            success: function (response) {
                console.log(response);
                var hojaAData = response.responseResult;

                if (hojaAData.Proycodi == 0) {
                    //hojaAData.Fechaconcesionactual = obtenerFechaActual();
                    //hojaAData.Fechainicioconstruccion = obtenerFechaActualMesAnio();
                    //hojaAData.Fechaoperacioncomercial = obtenerFechaActualMesAnio();
                } else {
                    hojaAData.Fechaconcesionactual = convertirFechaGlobal(hojaAData.Fechaconcesionactual);
                }
                setHojaA(hojaAData);
                cambiosRealizados = false;
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}
function verificarValor(valor) {
    return valor;
}


async function cargarCatalogosYAsignarValores(param) {
    console.log("🛠️ Verificando y cargando catálogos...");

    let promesas = [
        esperarCatalogoSubestacion("#txtNombreSubestacionTC", false),
        esperarCatalogo(catPropietario, "#propietarioSelect"),
        esperarCatalogo(catConcesionActual, "#concesionActSelect"),
        esperarCatalogo(catCombustible, "#combusTipoNombreSelect"),
        esperarCatalogo(catPerfil, "#perfilSelectNE"),
        esperarCatalogo(catPreFacltibilidad, "#prefactibilidadNE"),
        esperarCatalogo(catFacltibilidad, "#factibilidadNE"),
        esperarCatalogo(catEstudDef, "#estudioDefiniticoNE"),
        esperarCatalogo(catEia, "#EiaNE"),
        esperarCatalogo(catPodCalInf, "#undPoderCalorifInferiorSelect"),
        esperarCatalogo(catPodCalSup, "#undPoderCalorifSuperiorSelect"),
        esperarCatalogo(catCComb, "#undCostoCombustibleSelect"),
        esperarCatalogo(catCTratComb, "#undCostoTratamCombustibleSelect"),
        esperarCatalogo(catCTranspComb, "#undCostoTranspCombustibleSelect"),
        esperarCatalogo(catCVarNComb, "#undCostoVarNoCombustibleSelect"),
        esperarCatalogo(catCInvIni, "#undCostoInversionInicioSelect"),
        esperarCatalogo(catRendPl, "#undRendimientoPlantaSelect"),
        esperarCatalogo(catConsEspCon, "#undConsEspecCondicionesSelect")
    ];

    await Promise.all(promesas);
    console.log(" Todos los catálogos han cargado. Verificando y asignando valores...");

    function asignarSiExiste(selectId, valor) {
        let select = $(selectId);
        if (select.length > 0 && valor !== null && valor !== undefined) {
            let opciones = select.find("option").length;

            //  Si el `select` no tiene opciones, lo recarga.
            if (opciones === 0) {
                console.log(`⚠️ El select ${selectId} estaba vacío. Se recargó.`);
                select.append(new Option(valor, valor));
            }

            //  Si la opción no existe, la agrega.
            if (select.find(`option[value="${valor}"]`).length === 0) {
                select.append(new Option(valor, valor));
            }

            //  Asigna el valor y evita re-cargar si ya estaba asignado.
            select.val(valor);
            console.log(` Asignado: ${valor} a ${selectId}`);
        }
    }

    //  Asignar valores evitando recargas innecesarias
    asignarSiExiste("#propietarioSelect", param.Propietario);
    asignarSiExiste("#concesionActSelect", param.Tipoconcesionactual);
    asignarSiExiste("#combusTipoNombreSelect", param.Combustibletipo);
    asignarSiExiste("#txtNombreSubestacionTC", param.Nombresubestacion);
    asignarSiExiste("#perfilSelectNE", param.Perfil);
    asignarSiExiste("#prefactibilidadNE", param.Prefactibilidad);
    asignarSiExiste("#factibilidadNE", param.Factibilidad);
    asignarSiExiste("#estudioDefiniticoNE", param.Estudiodefinitivo);
    asignarSiExiste("#EiaNE", param.Eia);
    asignarSiExiste("#undPoderCalorifInferiorSelect", param.Undpci);
    asignarSiExiste("#undPoderCalorifSuperiorSelect", param.Undpcs);
    asignarSiExiste("#undCostoCombustibleSelect", param.Undcomb);
    asignarSiExiste("#undCostoTratamCombustibleSelect", param.Undtrtcomb);
    asignarSiExiste("#undCostoTranspCombustibleSelect", param.Undtrnspcomb);
    asignarSiExiste("#undCostoVarNoCombustibleSelect", param.Undvarncmb);
    asignarSiExiste("#undCostoInversionInicioSelect", param.Undinvinic);
    asignarSiExiste("#undRendimientoPlantaSelect", param.Undrendcnd);
    asignarSiExiste("#undConsEspecCondicionesSelect", param.Undconscp);

    console.log(" Valores asignados correctamente.");
}

async function setHojaA(param) { 
    $("#txtNombreCentral").val(param.Centralnombre);
    cargarUbicacionCCTTA(param.Distrito);
    $("#txtOperador").val(param.Sociooperador);
    $("#txtInversionista").val(param.Socioinversionista);
    setFechaPickerGlobal("#txtFechaAdjudicacionActual", param.Fechaconcesionactual, "dd/mm/aaaa");
    $("#txtPotenciaInstalada").val(verificarValor(param.Potenciainstalada));
    $("#txtPotenciaMaxima").val(verificarValor(param.Potenciamaxima));
    $("#txtPotenciaMinima").val(verificarValor(param.Potenciaminima));
    $("#txtCombustibletipoOtro").val(param.CombustibletipoOtro);
    $("#txtPoderCalorificoInferior").val(verificarValor(param.Podercalorificoinferior));
    $("#txtPoderCalorificoSuperior").val(verificarValor(param.Podercalorificosuperior));
    $("#txtCostoCombustible").val(verificarValor(param.Costocombustible));
    $("#txtCostoTratamCombustible").val(verificarValor(param.Costotratamientocombustible));
    $("#txtCostoTranspCombustible").val(verificarValor(param.Costotransportecombustible));
    $("#txtCostoVarNoCombustible").val(verificarValor(param.Costovariablenocombustible));
    $("#txtCostoInversionInicio").val(verificarValor(param.Costoinversioninicial));
    $("#txtRendimientoPlanta").val(verificarValor(param.Rendimientoplantacondicion));
    $("#txtConsEspecCondiciones").val(verificarValor(param.Consespificacondicion));
    $("#txtTipoMotorTermico").val(param.Tipomotortermico);
    $("#txtVelNomRotacion").val(verificarValor(param.Velnomrotacion));
    $("#txtPotMotorTermico").val(verificarValor(param.Potmotortermico));
    $("#txtNumMotoresTermicos").val(param.Nummotorestermicos);
    $("#txtPotenciaNom").val(verificarValor(param.Potgenerador));
    $("#txtNumeroGeneradores").val(param.Numgeneradores);
    $("#txtTensionGeneracion").val(verificarValor(param.Tensiongeneracion));
    $("#txtRendimientoGenerador").val(verificarValor(param.Rendimientogenerador));
    $("#txtTensionTC").val(verificarValor(param.Tensionkv));
    $("#txtLongitudTC").val(verificarValor(param.Longitudkm));
    $("#txtNTemasTC").val(param.Numternas);
    $("#txtComentarioA").val(param.Comentarios);
    $("#fechaPeriodoContruccion	").val(param.Periodoconstruccion);
    $("#fechaOperacionComercial").val(param.Fechaoperacioncomercial);
    $("#fechaInicioContruccion").val(param.Fechainicioconstruccion);
    console.log(" Valores de campos de texto asignados. Cargando catálogos...");
    //  Llamar a la función para cargar catálogos y asignar valores
    await cargarCatalogosYAsignarValores(param);
    setupDropdownToggle('combusTipoNombreSelect', 'txtCombustibletipoOtro');
    //  Cargar archivos registrados
    cargarArchivosRegistrados(seccionUbicacionCCTT, '#tablaUbicacionCCTT');
    cargarArchivosRegistrados(seccionEquipamientoCCTT, '#tablaEquipamientoFile');

    //  Si el modo es "consultar", desactivar campos y ocultar botón de grabar
    if (modoModel === "consultar") {
        desactivarCamposFormulario('TFichaA');
        $('#btnGrabarFichaCCTTA').hide();
    }
}

function cargarListaDepartamento(departamentos) {
    var selectDepartamentos = $('#departamentosSelect');
    $.each(departamentos, function (index, departamento) {
        // Crear la opci�n
        var option = $('<option>', {
            value: departamento.Id,
            text: departamento.Nombre
        });

        // Agregar la opci�n al select
        selectDepartamentos.append(option);
    });

    selectDepartamentos.change(function () {
        // Obtener el id del departamento seleccionado
        var idSeleccionado = $(this).val();
        // Llamar a la funci�n con el id del departamento
        console.log(idSeleccionado);
        cargarProvincia(idSeleccionado);
    });

}

getDataHojaCCTTA = function () {

    var param = {};
    param.Centralnombre = $("#txtNombreCentral").val();
    param.Distrito = $("#distritosSelect").val();
    param.Propietario = $("#propietarioSelect").val();
    param.Sociooperador = $("#txtOperador").val();
    param.Socioinversionista = $("#txtInversionista").val();
    param.Tipoconcesionactual = $("#concesionActSelect").val();
    param.Fechaconcesionactual =$("#txtFechaAdjudicacionActual").val();
    param.Potenciainstalada = $("#txtPotenciaInstalada").val();
    param.Potenciamaxima = $("#txtPotenciaMaxima").val();
    param.Potenciaminima = $("#txtPotenciaMinima").val();
    param.Combustibletipo = $("#combusTipoNombreSelect").val();
    param.CombustibletipoOtro = $("#txtCombustibletipoOtro").val();
    param.Podercalorificoinferior = $("#txtPoderCalorificoInferior").val();
    param.Undpci = $("#undPoderCalorifInferiorSelect").val();
    param.Podercalorificosuperior = $("#txtPoderCalorificoSuperior").val();
    param.Undpcs = $("#undPoderCalorifSuperiorSelect").val();
    param.Costocombustible = $("#txtCostoCombustible").val();
    param.Undcomb = $("#undCostoCombustibleSelect").val();
    param.Costotratamientocombustible = $("#txtCostoTratamCombustible").val();
    param.Undtrtcomb = $("#undCostoTratamCombustibleSelect").val();
    param.Costotransportecombustible = $("#txtCostoTranspCombustible").val();
    param.Undtrnspcomb = $("#undCostoTranspCombustibleSelect").val();
    param.Costovariablenocombustible = $("#txtCostoVarNoCombustible").val();
    param.Undvarncmb = $("#undCostoVarNoCombustibleSelect").val();
    param.Costoinversioninicial = $("#txtCostoInversionInicio").val();
    param.Undinvinic = $("#undCostoInversionInicioSelect").val();
    param.Rendimientoplantacondicion = $("#txtRendimientoPlanta").val();
    param.Undrendcnd = $("#undRendimientoPlantaSelect").val();
    param.Consespificacondicion = $("#txtConsEspecCondiciones").val();
    param.Undconscp = $("#undConsEspecCondicionesSelect").val();
    param.Tipomotortermico = $("#txtTipoMotorTermico").val();
    param.Velnomrotacion = $("#txtVelNomRotacion").val();
    param.Potmotortermico = $("#txtPotMotorTermico").val();
    param.Nummotorestermicos = $("#txtNumMotoresTermicos").val();
    param.Potgenerador = $("#txtPotenciaNom").val();
    param.Numgeneradores = $("#txtNumeroGeneradores").val();
    param.Tensiongeneracion = $("#txtTensionGeneracion").val();
    param.Rendimientogenerador = $("#txtRendimientoGenerador").val();
    param.Tensionkv = $("#txtTensionTC").val();
    param.Longitudkm = $("#txtLongitudTC").val();
    param.Numternas = $("#txtNTemasTC").val();
    param.Nombresubestacion = $("#txtNombreSubestacionTC").val();
    param.Perfil = $("#perfilSelectNE").val();
    param.Prefactibilidad = $("#prefactibilidadNE").val();
    param.Factibilidad = $("#factibilidadNE").val();
    param.Estudiodefinitivo = $("#estudioDefiniticoNE").val();
    param.Eia = $("#EiaNE").val();
    param.Fechainicioconstruccion = $("#fechaInicioContruccion").val();
    param.Periodoconstruccion = $("#fechaPeriodoContruccion").val();
    param.Fechaoperacioncomercial = $("#fechaOperacionComercial").val();
    param.Comentarios = $("#txtComentarioA").val();
    console.log(param);

    return param;
}

function eliminarFile(id) {
    document.getElementById("contenidoPopup").innerHTML = '¿Estás seguro de realizar esta operación?';
    $('#popupProyectoGeneral').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
    $('#btnConfirmarPopup').off('click').on('click', function() {
        $.ajax({
            type: 'POST',
            url: controladorFichas + 'EliminarFile',
            data: {
                id: id,
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    $("#fila" + id).remove();
                    mostrarMensaje('mensajeFicha', 'exito', 'El archivo se eliminó correctamente.');
                }
                else {
                    mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error.');
            }
        });
        popupClose('popupProyectoGeneral');
    });
}

function agregarFilaEquipamiento(nombre, id) {
    // Obtener el cuerpo de la tabla
    var nuevaFila = '<tr id="fila' + id + '">' +
        '<td>' + nombre + '</td>' +
        '<td> <a href="#"><img src="' + siteRoot + 'Content/Images/eliminar.png" class="btnEliminar" onclick="eliminarFile(' + id + ')" title="Eliminar archivo"/></a></td>' +
        '</tr>';
    var tabla = $('#tablaEquipamientoFile');
    tabla.append(nuevaFila);
}

function agregarFilaUbicacion(nombre, id) {
    // Obtener el cuerpo de la tabla
    var nuevaFila = '<tr id="fila' + id + '">' +
        '<td>' + nombre + '</td>' +
        '<td> <a href="#"><img src="' + siteRoot + 'Content/Images/eliminar.png" class="btnEliminar" onclick="eliminarFile(' + id + ')" title="Eliminar archivo"/></a></td>' +
        '</tr>';
    var tabla = $('#tablaUbicacion');
    tabla.append(nuevaFila);
}


function cargarProvincia(id, callback) {
    $.ajax({
        type: 'POST',
        url: controladorFichas + 'ListarProvincias',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: id }),
        success: function (eData) {
            console.log(eData);
            cargarListaProvincia(eData);
            if (typeof callback === 'function') {
                callback();
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarListaProvincia(provincias) {
    var selectProvincias = $('#provinciasSelect');
    selectProvincias.empty();
    var optionDefault = $('<option>', {
        value: "",
        text: "Seleccione una provincia"
    });
    selectProvincias.append(optionDefault);
    cargarListaDistritos([]);
    $.each(provincias, function (index, provincia) {
        // Crear la opci�n
        var option = $('<option>', {
            value: provincia.Id,
            text: provincia.Nombre
        });

        // Agregar la opci�n al select
        selectProvincias.append(option);
    });

    selectProvincias.change(function () {
        // Obtener el id del departamento seleccionado
        var idSeleccionado = $(this).val();
        // Llamar a la funci�n con el id del departamento
        cargarDistrito(idSeleccionado);
    });

}

function cargarDistrito(id, callback) {
    $.ajax({
        type: 'POST',
        url: controladorFichas + 'ListarDistritos',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: id }),
        success: function (eData) {
            console.log(eData);
            cargarListaDistritos(eData);
            if (typeof callback === 'function') {
                callback();
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarListaDistritos(distritos) {
    var selectDistritos = $('#distritosSelect');
    selectDistritos.empty();
    var optionDefault = $('<option>', {
        value: "",
        text: "Seleccione un distrito"
    });
    selectDistritos.append(optionDefault);
    $.each(distritos, function (index, distrito) {
        // Crear la opci�n
        var option = $('<option>', {
            value: distrito.Id,
            text: distrito.Nombre
        });

        // Agregar la opci�n al select
        selectDistritos.append(option);
    });

}


function crearPuploadCCTTA() {
    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSubirFormato',
        url: controladorFichas + 'UploadArchivoUbicacion',
        flash_swf_url: 'Scripts/Moxie.swf',
        silverlight_xap_url: 'Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '5mb',
            mime_types: [
                { title: "Imagenes", extensions: "jpg,jpeg,gif,png" },
                { title: "Archivos comprimidos", extensions: "zip,rar,kmz" },
                { title: "Documentos", extensions: "pdf,doc,docx,xls,xlsx,ppt,pptx,csv,msg" }
            ]
        },
        init: {
            FilesAdded: function (up, files) {
                if (uploader.files.length == 2) {
                    uploader.removeFile(uploader.files[0]);
                }
                uploader.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarMensaje('mensajeFicha', 'alert', "Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            FileUploaded: function (up, file, response) {
                var json = JSON.parse(response.response);
                if (json.indicador == 1) {
                    up.removeFile(file);
                    procesarArchivoUbicacion(json.fileNameNotPath, json.nombreReal, json.extension)
                }
            },
            UploadComplete: function (up, file) {
                mostrarMensaje('mensajeFicha', 'alert', "Subida completada. <strong>Por favor espere...</strong>");

            },
            Error: function (up, err) {
                mostrarMensaje('mensajeFicha', 'error', err.code + "-" + err.message);
            }
        }
    });
    uploader.init();
}

function procesarArchivoUbicacion(fileNameNotPath, nombreReal, tipo) {
    param = {};
    param.SeccCodi = 5;
    param.ProyCodi = $("#txtPoyCodi").val();;
    param.ArchNombre = nombreReal;
    param.ArchNombreGenerado = fileNameNotPath;
    param.ArchTipo = tipo;
    $.ajax({
        type: 'POST',
        url: controladorFichas + 'GrabarRegistroArchivo',
        data: param,
        
        success: function (result) {
            if (result.responseResult > 0) {
                mostrarMensaje('mensajeFicha', 'exito', 'Archivo registrado correctamente.');
                console.log("Id Archivo:" + result.id)
                agregarFilaUbicacion(nombreReal, result.id);
            }
            else {
                mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error.');
        }
    });
}

function grabarFichaCCTTA() {
    var param = {};
    param.TransmisionProyectoDTO = getCabeceraProyecto();
    var idProyecto = $("#txtPoyCodi").val();
    if (idProyecto != "0") {
        param.ProyCodi = idProyecto;
        param.RegHojaCCTTADTO = getDataHojaCCTTA();
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarFichaCCTTA',
            data: param,
            
            success: function (result) {
                console.log(result);
                if (result) {
                    mostrarMensaje('mensajeFicha', 'exito', 'Los datos se grabaron correctamente.')
                }
                else {

                    mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error.');
            }
        });
    }
    cambiosRealizados = false;
}

function cargarUbicacionCCTTA(idDistrito) {
    $.ajax({
        type: 'POST',
        url: controladorFichas + 'UbicacionByDistro',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: idDistrito }),
        success: function (ubicacion) {
            $('#departamentosSelect').val(ubicacion.DepartamentoId);
            cargarProvincia(ubicacion.DepartamentoId, function () {
                $('#provinciasSelect').val(ubicacion.ProvinciaId);

                cargarDistrito(ubicacion.ProvinciaId, function () {
                    $('#distritosSelect').val(ubicacion.DistritoId);

                });
            });
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

// Función de validación
function validarInputNumero(input) {
    // Remover cualquier carácter que no sea un número o un punto decimal
    input.value = input.value.replace(/[^0-9.]/g, '');

    // Permitir solo un punto decimal
    const parts = input.value.split('.');
    if (parts.length > 2) {
        input.value = parts[0] + '.' + parts.slice(1).join('');
    }

    // Remover los ceros iniciales
    if (input.value.length > 1 && input.value[0] === '0' && input.value[1] !== '.') {
        input.value = input.value.substring(1);
    }

    // Si el valor es negativo, establecerlo en vacío
    if (parseFloat(input.value) < 0) {
        input.value = '';
    }
}

// Aplicar validación en el evento 'input' y en cambios del valor del input
document.querySelectorAll('.txtnumber').forEach(input => {
    input.addEventListener('input', function () {
        validarInputNumero(this);
    });

    // Observador para cambios en el valor del input
    const observer = new MutationObserver(() => validarInputNumero(input));

    // Configurar el observador para detectar cambios de atributos
    observer.observe(input, { attributes: true, attributeFilter: ['value'] });
});

document.querySelectorAll('.txtinteger').forEach(input => {
    input.addEventListener('input', function () {
        // Permitir solo dígitos numéricos
        this.value = this.value.replace(/[^0-9]/g, '');

        // Remover los ceros iniciales
        if (this.value.length > 1 && this.value[0] === '0') {
            this.value = this.value.substring(1);
        }

        // Si el valor es negativo, establecerlo en vacío (si se desea evitar negativos)
        if (this.value < 0) {
            this.value = '';
        }
    });
});

