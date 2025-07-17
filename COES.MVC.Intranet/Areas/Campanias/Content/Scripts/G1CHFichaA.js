var seccionUbicacion = 1;
var seccionInfoBasica = 2;
var seccionTurbo = 3;

$(function () {
    var ruta_interna = obtenerRutaProyecto();  
    //$('#txtFechaAdjudicacionTemporal').val(obtenerFechaActual());
    //$('#txtFechaAdjudicacionActual').val(obtenerFechaActual());
    //$('#fechaInicioContruccion').val(obtenerFechaActualMesAnio());
    //$('#fechaOperacionComercial').val(obtenerFechaActualMesAnio());

    $('#txtFechaAdjudicacionTemporal').Zebra_DatePicker({
        format: 'd/m/Y',
        onSelect: function () {
            cambiosRealizados = true; // Marcar que hubo un cambio
        }
    });

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
    //crearPuploadUbicacionA();
    //crearPuploadInfBas();
    //crearPuploadUnTurbo();
    //cargarCatalogo(catPropietario, "#propietarioSelect");
    //cargarCatalogo(catConcesionTemporal, "#concesionTempSelect");
    //cargarCatalogo(catConcesionActual, "#concesionActSelect");

    //cargarCatalogo(catTipoPPL, "#tipoTunelSelect");
    //cargarCatalogo(catSE, "#tipoTuberiaFSelect");
    //cargarCatalogo(catSE, "#tipoCasaMaquinasSelect");
    //cargarCatalogo(catPerfil, "#perfilSelectNE");
    //cargarCatalogo(catPreFacltibilidad, "#prefactibilidadNE");
    //cargarCatalogo(catFacltibilidad, "#factibilidadNE");
    //cargarCatalogo(catEstudDef, "#estudioDefiniticoNE");
    //cargarCatalogo(catEia, "#EiaNE");

    crearUploaderGeneral('btnSubirUbicacionHidro', '#tablaUbicacionA', seccionUbicacion, null, ruta_interna);
    crearUploaderGeneral('btnSubirFormatoInfBasica', '#tablaInfBasica', seccionInfoBasica, null, ruta_interna);
    crearUploaderGeneral('btnSubirFormatoUnTurbo', '#tablaUnTurbo', seccionTurbo, null, ruta_interna);

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

// Selecciona todos los elementos con la clase 'irfichaD'
const irfichaDElements = document.querySelectorAll('.irfichaD');

// Recorre cada elemento y agrega un evento de clic
irfichaDElements.forEach(element => {
    element.addEventListener('click', () => {
        // Selecciona el elemento con el id 'tabfichad' y le agrega la clase 'active'
        document.getElementById('tabfichad').classList.add('active');
    });
});


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


//function crearPuploadUbicacionA() {
//    uploaderUbicacionA = new plupload.Uploader({
//        runtimes: 'html5,flash,silverlight,html4',
//        browse_button: 'btnSubirFormatoUbicacionA',
//        url: controladorFichas + 'UploadArchivoGeneral',
//        flash_swf_url: 'Scripts/Moxie.swf',
//        silverlight_xap_url: 'Scripts/Moxie.xap',
//        multi_selection: false,
//        filters: {
//            max_file_size: '5mb',
//            mime_types: [
//                //{ title: "Imagenes", extensions: "jpg,jpeg,gif,png" },
//                { title: "Archivos comprimidos", extensions: "zip,rar,kmz" },
//                //{ title: "Documentos", extensions: "pdf,doc,docx,xls,xlsx,ppt,pptx,csv,msg" }
//            ]
//        },
//        init: {
//            FilesAdded: function (up, files) {
//                if (uploaderUbicacionA.files.length == 2) {
//                    uploaderUbicacionA.removeFile(uploaderUbicacionA.files[0]);
//                }
//                uploaderUbicacionA.start();
//                up.refresh();
//            },
//            UploadProgress: function (up, file) {
//                mostrarMensaje('mensajeFicha', 'alert', "Por favor espere ...(<strong>" + file.percent + "%</strong>)");
//            },
//            FileUploaded: function (up, file, response) {
//                var json = JSON.parse(response.response);
//                if (json.indicador == 1) {
//                    up.removeFile(file);
//                    procesarArchivoUbicacionA(json.fileNameNotPath, json.nombreReal, json.extension)
//                }
//            },
//            UploadComplete: function (up, file) {
//                mostrarMensaje('mensajeFicha', 'alert', "Subida completada. <strong>Por favor espere...</strong>");

//            },
//            Error: function (up, err) {
//                mostrarMensaje('mensajeFicha', 'error', err.code + "-" + err.message);
//            }
//        }
//    });
//    uploaderUbicacionA.init();
//}


function procesarArchivoUbicacionA(fileNameNotPath, nombreReal, tipo) {
    param = {};
    param.SeccCodi = seccionUbicacion;
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
                agregarFilaUbicacionA(nombreReal, result.id);
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


function agregarFilaUbicacionA(nombre, id) {
    // Obtener el cuerpo de la tabla
    var nuevaFila = '<tr id="fila' + id + '">' +
        '<td>' + nombre + '</td>' +
        '<td></td>' +
        '</tr>';
    var tabla = $('#tablaUbicacionA');
    tabla.append(nuevaFila);
}




//function crearPuploadInfBas() {
//    uploaderInfBas = new plupload.Uploader({
//        runtimes: 'html5,flash,silverlight,html4',
//        browse_button: 'btnSubirFormatoInfBasica',
//        url: controladorFichas + 'UploadArchivoGeneral',
//        flash_swf_url: 'Scripts/Moxie.swf',
//        silverlight_xap_url: 'Scripts/Moxie.xap',
//        multi_selection: false,
//        filters: {
//            max_file_size: '5mb',
//            mime_types: [
//                { title: "Imagenes", extensions: "jpg,jpeg,gif,png" },
//                { title: "Archivos comprimidos", extensions: "zip,rar" },
//                { title: "Documentos", extensions: "pdf,doc,docx,xls,xlsx,ppt,pptx,csv,msg" }
//            ]
//        },
//        init: {
//            FilesAdded: function (up, files) {
//                if (uploaderInfBas.files.length == 2) {
//                    uploaderInfBas.removeFile(uploaderInfBas.files[0]);
//                }
//                uploaderInfBas.start();
//                up.refresh();
//            },
//            UploadProgress: function (up, file) {
//                mostrarMensaje('mensajeFicha', 'alert', "Por favor espere ...(<strong>" + file.percent + "%</strong>)");
//            },
//            FileUploaded: function (up, file, response) {
//                var json = JSON.parse(response.response);
//                if (json.indicador == 1) {
//                    up.removeFile(file);
//                    procesarArchivoInfBas(json.fileNameNotPath, json.nombreReal, json.extension)
//                }
//            },
//            UploadComplete: function (up, file) {
//                mostrarMensaje('mensajeFicha', 'alert', "Subida completada. <strong>Por favor espere...</strong>");

//            },
//            Error: function (up, err) {
//                mostrarMensaje('mensajeFicha', 'error', err.code + "-" + err.message);
//            }
//        }
//    });
//    uploaderInfBas.init();
//}

//function crearPuploadUnTurbo() {
//    uploaderUnTurbo = new plupload.Uploader({
//        runtimes: 'html5,flash,silverlight,html4',
//        browse_button: 'btnSubirFormatoUnTurbo',
//        url: controladorFichas + 'UploadArchivoGeneral',
//        flash_swf_url: 'Scripts/Moxie.swf',
//        silverlight_xap_url: 'Scripts/Moxie.xap',
//        multi_selection: false,
//        filters: {
//            max_file_size: '5mb',
//            mime_types: [
//                { title: "Imagenes", extensions: "jpg,jpeg,gif,png" },
//                { title: "Archivos comprimidos", extensions: "zip,rar" },
//                { title: "Documentos", extensions: "pdf,doc,docx,xls,xlsx,ppt,pptx,csv,msg" }
//            ]
//        },
//        init: {
//            FilesAdded: function (up, files) {
//                if (uploaderUnTurbo.files.length == 2) {
//                    uploaderUnTurbo.removeFile(uploaderUnTurbo.files[0]);
//                }
//                uploaderUnTurbo.start();
//                up.refresh();
//            },
//            UploadProgress: function (up, file) {
//                mostrarMensaje('mensajeFicha', 'alert', "Por favor espere ...(<strong>" + file.percent + "%</strong>)");
//            },
//            FileUploaded: function (up, file, response) {
//                var json = JSON.parse(response.response);
//                if (json.indicador == 1) {
//                    up.removeFile(file);
//                    procesarArchivoUnTurbo(json.fileNameNotPath, json.nombreReal, json.extension)
//                }
//            },
//            UploadComplete: function (up, file) {
//                mostrarMensaje('mensajeFicha', 'alert', "Subida completada. <strong>Por favor espere...</strong>");

//            },
//            Error: function (up, err) {
//                mostrarMensaje('mensajeFicha', 'error', err.code + "-" + err.message);
//            }
//        }
//    });
//    uploaderUnTurbo.init();
//}

//function agregarFilaInfBas(nombre, id) {
//    // Obtener el cuerpo de la tabla
//    var nuevaFila = '<tr id="fila' + id + '">' +
//        '<td>' + nombre + '</td>' +
//        '<td> <a href="#"><img src="' + siteRoot + 'Content/Images/eliminar.png" class="btnEliminar" onclick="eliminarFile(' + id + ')" title="Eliminar archivo"/></a></td>' +
//        '</tr>';
//    var tabla = $('#tablaInfBasica');
//    tabla.append(nuevaFila);
//}



//function procesarArchivoInfBas(fileNameNotPath, nombreReal, tipo) {
//    param = {};
//    param.SeccCodi = seccionInfoBasica;
//    param.ProyCodi = $("#txtPoyCodi").val();;
//    param.ArchNombre = nombreReal;
//    param.ArchNombreGenerado = fileNameNotPath;
//    param.ArchTipo = tipo;
//    $.ajax({
//        type: 'POST',
//        url: controladorFichas + 'GrabarRegistroArchivo',
//        data: param,
        
//        success: function (result) {
//            if (result.responseResult > 0) {
//                mostrarMensaje('mensajeFicha', 'exito', 'Archivo registrado correctamente.');
//                console.log("Id Archivo:" + result.id)
//                agregarFilaInfBas(nombreReal, result.id);
//            }
//            else {
//                mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error.');
//            }
//        },
//        error: function () {
//            mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error.');
//        }
//    });
//}

function procesarArchivoUnTurbo(fileNameNotPath, nombreReal, tipo) {
    param = {};
    param.SeccCodi = seccionTurbo;
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
                agregarFilaUnTurbo(nombreReal, result.id);
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

//function agregarFilaInfBas(nombre, id) {
//    // Obtener el cuerpo de la tabla
//    var nuevaFila = '<tr id="fila' + id + '">' +
//        '<td>' + nombre + '</td>' +
//        '<td> <a href="#"><img src="' + siteRoot + 'Content/Images/eliminar.png" class="btnEliminar" onclick="eliminarFile(' + id + ')" title="Eliminar archivo"/></a></td>' +
//        '</tr>';
//    var tabla = $('#tablaInfBasica');
//    tabla.append(nuevaFila);
//}

function agregarFilaUnTurbo(nombre, id) {
    // Obtener el cuerpo de la tabla
    var nuevaFila = '<tr id="fila' + id + '">' +
        '<td>' + nombre + '</td>' +
        '<td> </td>' +
        '</tr>';
    var tabla = $('#tablaUnTurbo');
    tabla.append(nuevaFila);
}

// Función para parsear fechas del formato /Date(1730350800000)/ a DD/MM/YYYY
function parseDateString(jsonDate) {
    if (!jsonDate) return "";
    // Extrae la marca de tiempo en milisegundos del formato "/Date(1730350800000)/"
    const timestamp = parseInt(jsonDate.match(/\/Date\((\d+)\)\//)[1], 10);
    // Convierte a objeto Date y formatea con moment.js
    return moment(timestamp).format("DD/MM/YYYY");
}
// Función para convertir fechas a "1/MM/YYYY 00:00:00"
function convertirFechaCompleta(fechaAnoMes) {
    if (!fechaAnoMes) {
        return '';
    }

    // Suponiendo que la fecha de entrada está en formato `yyyy/mm`
    let [anio, mes] = fechaAnoMes.split('/');

    // Si falta el año o el mes, retorna vacío
    if (!anio || !mes) {
        return '';
    }

    // Asigna el día 01 y la hora 00:00:00 por defecto
    return `01/${mes}/${anio} 00:00:00`;
}
getHojaA = function () {

    var param = {};
    param.Centralnombre = $("#txtNombreCentral").val();
    param.Distrito = $("#distritosSelect").val();
    param.Cuenca = $("#txtCuenca").val();
    param.Rio = $("#txtRio").val();
    param.Propietario = $("#propietarioSelect").val();
    param.Sociooperador = $("#txtOperador").val();
    param.Socioinversionista = $("#txtInversionista").val();
    param.Concesiontemporal = $("#concesionTempSelect").val();
    //param.Fechaconcesiontemporal = moment($("#txtFechaAdjudicacionTemporal").val(), "DD/MM/YYYY").format("DD/MM/YYYY hh:mm:ss A");
    // Formateo de fecha en la asignación
    param.Fechaconcesiontemporal =$("#txtFechaAdjudicacionTemporal").val();
    param.Tipoconcesionactual = $("#concesionActSelect").val();
    param.Fechaconcesionactual = $("#txtFechaAdjudicacionActual").val();
    param.Nombreestacion = $("#txtNombreEstacion").val();
    param.Numestacion = $("#txtNumEstacion").val();
    param.Periodohistorica = $('input[name="rbPeriodoHistorica"]:checked').val();
    param.Periodonaturalizada = $('input[name="rbPeriodoNaturalizada"]:checked').val();
    param.Demandaagua = $('input[name="rbDemandaAgua"]:checked').val();
    param.Estudiogeologico = $('input[name="rbEstudioGeologico"]:checked').val();
    param.Perfodiamantinas = $("#txtPerforacionDiam").val();
    param.Numcalicatas = $("#txtNumCalicatas").val();
    param.EstudioTopografico = $('input[name="rbEstudioTopografico"]:checked').val();
    param.Levantamientotopografico = $("#txtLevantamientoTopo").val();
    param.Alturabruta = $("#txtAlturaBruta").val();
    param.Alturaneta = $("#txtAlturaNeta").val();
    param.Caudaldiseno = $("#txtCaudalDisenio").val();
    param.Potenciainstalada = $("#txtPotenciaInstalada").val();
    param.Conduccionlongitud = $("#txtLongitudConduccion").val();
    param.Tunelarea = $("#txtAreaConduccion").val();
    param.Tuneltipo = $("#tipoTunelSelect").val();
    param.Tuberialongitud = $("#txtLongitudTuberiaF").val();
    param.Tuberiadiametro = $("#txtDiametroTuberiaF").val();
    param.Tuberiatipo = $("#tipoTuberiaFSelect").val();
    param.Maquinatipo = $("#tipoCasaMaquinasSelect").val();

    param.Maquinaaltitud = $("#txtAltCasaMaquinas").val();
    param.Regestacionalvbruto = $("#txtVolBrRegulEsta").val();
    param.Regestacionalvutil = $("#txtVolUtilRegulEsta").val();
    param.Regestacionalhpresa = $("#txtHPresaRegulEsta").val();
    param.Reghorariavutil = $("#txtVolUtilRegulHora").val();
    param.Reghorariahpresa = $("#txtHPresaReguHora").val();
    param.Reghorariaubicacion = $("#txtUbicacionRegHora").val();
    param.Energhorapunta = $("#txtHoraPuntaEnerg").val();
    param.Energfuerapunta = $("#txtFueraPuntaEnerg").val();
    param.Tipoturbina = $("#txtTipoTurbina").val();
    param.Velnomrotacion = $("#txtVelocidadRotacion").val();
    param.Potturbina = $("#txtPotenciaTurbina").val();
    param.Numturbinas = $("#txtNumTurbinas").val();
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

setHojaA = function (param) {
  
    $("#txtNombreCentral").val(param.Centralnombre);
    cargarUbicacionCHA(param.Distrito);
    $("#txtCuenca").val(param.Cuenca);
    $("#txtRio").val(param.Rio);
    $("#propietarioSelect").val(param.Propietario);
    $("#txtOperador").val(param.Sociooperador);
    $("#txtInversionista").val(param.Socioinversionista);
    $("#concesionTempSelect").val(param.Concesiontemporal);
     
    $("#concesionActSelect").val(param.Tipoconcesionactual);
    setFechaPickerGlobal("#txtFechaAdjudicacionTemporal", param.Fechaconcesiontemporal, "dd/mm/aaaa");
    setFechaPickerGlobal("#txtFechaAdjudicacionActual", param.Fechaconcesionactual, "dd/mm/aaaa");
    $("#txtNombreEstacion").val(param.Nombreestacion);
    $("#txtNumEstacion").val(param.Numestacion);
    $(`#rbPeriodoHistorica${param.Periodohistorica}`).prop("checked", true);
    $(`#rbPeriodoNaturalizada${param.Periodonaturalizada}`).prop("checked", true);
    $(`#rbDemandaAgua${param.Demandaagua}`).prop("checked", true);
    $(`#rbEstudioGeologico${param.Estudiogeologico}`).prop("checked", true);
    $("#txtPerforacionDiam").val(param.Perfodiamantinas);
    $("#txtNumCalicatas").val(param.Numcalicatas);
    $(`#rbEstudioTopografico${param.EstudioTopografico}`).prop("checked", true);
    $("#txtLevantamientoTopo").val(param.Levantamientotopografico);
    $("#txtAlturaBruta").val(param.Alturabruta);
    $("#txtAlturaNeta").val(param.Alturaneta);
    $("#txtCaudalDisenio").val(param.Caudaldiseno);
    $("#txtPotenciaInstalada").val(param.Potenciainstalada);
    $("#txtLongitudConduccion").val(param.Conduccionlongitud);
    $("#txtAreaConduccion").val(param.Tunelarea);
    $("#tipoTunelSelect").val(param.Tuneltipo);
    $("#txtLongitudTuberiaF").val(param.Tuberialongitud);
    $("#txtDiametroTuberiaF").val(param.Tuberiadiametro);
    $("#tipoTuberiaFSelect").val(param.Tuberiatipo);
    $("#tipoCasaMaquinasSelect").val(param.Maquinatipo);
    $("#txtAltCasaMaquinas").val(param.Maquinaaltitud);
    $("#txtVolBrRegulEsta").val(param.Regestacionalvbruto);
    $("#txtVolUtilRegulEsta").val(param.Regestacionalvutil);
    $("#txtHPresaRegulEsta").val(param.Regestacionalhpresa);
    $("#txtVolUtilRegulHora").val(param.Reghorariavutil);
    $("#txtHPresaReguHora").val(param.Reghorariahpresa);
    $("#txtUbicacionRegHora").val(param.Reghorariaubicacion);
    $("#txtHoraPuntaEnerg").val(param.Energhorapunta);
    $("#txtFueraPuntaEnerg").val(param.Energfuerapunta);
    $("#txtTipoTurbina").val(param.Tipoturbina);
    $("#txtVelocidadRotacion").val(param.Velnomrotacion);
    $("#txtPotenciaTurbina").val(param.Potturbina);
    $("#txtNumTurbinas").val(param.Numturbinas);
    $("#txtPotenciaNom").val(param.Potgenerador);
    $("#txtNumeroGeneradores").val(param.Numgeneradores);
    $("#txtTensionGeneracion").val(param.Tensiongeneracion);
    $("#txtRendimientoGenerador").val(param.Rendimientogenerador);
    $("#txtTensionTC").val(param.Tensionkv);
    $("#txtLongitudTC").val(param.Longitudkm);
    $("#txtNTemasTC").val(param.Numternas);
    $("#txtNombreSubestacionTC").val(param.Nombresubestacion);
    $("#perfilSelectNE").val(param.Perfil);
    $("#prefactibilidadNE").val(param.Prefactibilidad);
    $("#factibilidadNE").val(param.Factibilidad);
    $("#estudioDefiniticoNE").val(param.Estudiodefinitivo);
    $("#EiaNE").val(param.Eia);
    $("#fechaInicioContruccion").val(param.Fechainicioconstruccion);
    $("#fechaPeriodoContruccion").val(param.Periodoconstruccion);
    $("#fechaOperacionComercial").val(param.Fechaoperacioncomercial);
    $("#txtComentarioA").val(param.Comentarios);
    cargarArchivosRegistrados(seccionUbicacion, '#tablaUbicacionA');
    cargarArchivosRegistrados(seccionInfoBasica, '#tablaInfBasica');
    cargarArchivosRegistrados(seccionTurbo, '#tablaUnTurbo');

    if (modoModel == "consultar") {
        desactivarCamposFormulario('HFichaA');
        $('#btnGrabarFichaA').hide();
    }

}

async function cargarDatosA() {
    limpiarCombos('#propietarioSelect, #concesionTempSelect, #concesionActSelect, #tipoTunelSelect, #tipoTuberiaFSelect, #tipoCasaMaquinasSelect, #perfilSelectNE, #prefactibilidadNE, #factibilidadNE, #estudioDefiniticoNE, #EiaNE');

    console.log("Cargando catálogos...");

    // Asegurarse de que todos los catálogos estén listos antes de continuar
    await cargarCatalogosYAsignarValoresA({});

    console.log("Todos los catálogos han sido cargados. Procediendo a obtener los datos del proyecto...");

    if (modoModel === "editar" || modoModel === "consultar") {
        var idProyecto = $("#txtPoyCodi").val();

        try {
            const response = await $.ajax({
                type: 'POST',
                url: controladorFichas + 'GetChHojaA',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ id: idProyecto }),
            });

            console.log("Respuesta del servidor:", response);

            var hojaAData = response.responseResult;

            // Convertir las fechas antes de asignarlas
            

            if (hojaAData.Proycodi == 0) {
                //hojaAData.Fechaconcesiontemporal = obtenerFechaActual();
                //hojaAData.Fechaconcesionactual = obtenerFechaActual();
                //hojaAData.Fechainicioconstruccion = obtenerFechaActualMesAnio();
                //hojaAData.Fechaoperacioncomercial = obtenerFechaActualMesAnio();
            } else {
                hojaAData.Fechaconcesiontemporal = convertirFechaGlobal(hojaAData.Fechaconcesiontemporal);
                hojaAData.Fechaconcesionactual = convertirFechaGlobal(hojaAData.Fechaconcesionactual);

            }
            // Llamar a setHojaA solo después de que todos los catálogos estén listos
            await setHojaA(hojaAData);
            cambiosRealizados = false;

        } catch (error) {
            console.error("Error en la carga de datos:", error);
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error al cargar los datos del proyecto.');
        }
    }
}

async function cargarCatalogosYAsignarValoresA(param) {
    console.log("Verificando y cargando catálogos...");

    let promesas = [
        esperarCatalogo(catPropietario, "#propietarioSelect"),
        esperarCatalogo(catConcesionTemporal, "#concesionTempSelect"),
        esperarCatalogo(catConcesionActual, "#concesionActSelect"),
        esperarCatalogo(catTipoPPL, "#tipoTunelSelect"),
        esperarCatalogo(catSE, "#tipoTuberiaFSelect"),
        esperarCatalogo(catSE, "#tipoCasaMaquinasSelect"),
        esperarCatalogo(catPerfil, "#perfilSelectNE"),
        esperarCatalogo(catPreFacltibilidad, "#prefactibilidadNE"),
        esperarCatalogo(catFacltibilidad, "#factibilidadNE"),
        esperarCatalogo(catEstudDef, "#estudioDefiniticoNE"),
        esperarCatalogo(catEia, "#EiaNE"),
        esperarCatalogoSubestacion("#txtNombreSubestacionTC", false) // Se añade el catálogo de subestaciones
    ];

    // Asegurar que todas las promesas de carga se completen antes de continuar
    await Promise.all(promesas);

    console.log("Todos los catálogos han sido cargados. Asignando valores...");

    function asignarSiExiste(selectId, valor) {
        let select = $(selectId);
        if (select.length > 0 && valor !== null && valor !== undefined) {
            if (select.find("option").length === 0) {
                console.log(`El select ${selectId} estaba vacío. Se recargó.`);
                select.append(new Option(valor, valor));
            }
            select.val(valor);
            console.log(`Asignado: ${valor} a ${selectId}`);
        }
    }

    asignarSiExiste("#propietarioSelect", param.Propietario);
    asignarSiExiste("#concesionTempSelect", param.Concesiontemporal);
    asignarSiExiste("#concesionActSelect", param.Tipoconcesionactual);
    asignarSiExiste("#tipoTunelSelect", param.Tuneltipo);
    asignarSiExiste("#tipoTuberiaFSelect", param.Tuberiatipo);
    asignarSiExiste("#tipoCasaMaquinasSelect", param.Maquinatipo);
    asignarSiExiste("#perfilSelectNE", param.Perfil);
    asignarSiExiste("#prefactibilidadNE", param.Prefactibilidad);
    asignarSiExiste("#factibilidadNE", param.Factibilidad);
    asignarSiExiste("#estudioDefiniticoNE", param.Estudiodefinitivo);
    asignarSiExiste("#EiaNE", param.Eia);

    console.log("Valores asignados correctamente.");
}

//function esperarCatalogo(id, selectHtml) {
//    return new Promise((resolve) => {
//        $.ajax({
//            type: 'POST',
//            url: controlador + 'ListarCatalogo',
//            contentType: "application/json; charset=utf-8",
//            data: JSON.stringify({ id: id }),
//            success: function (eData) {
//                cargarListaParametros(eData, selectHtml); // Llenar el select con los datos
//                console.log(`Catálogo cargado: ${selectHtml}`);
//                resolve(); // Indicar que el catálogo ya respondió
//            },
//            error: function (err) {
//                console.warn(`⚠️ Error al cargar catálogo: ${selectHtml}. Se considera válido.`);
//                resolve(); // Asegurar que se resuelva incluso con error
//            }
//        });
//    });
//}

//function esperarCatalogoSubestacion(selectHtml) {
//    return new Promise((resolve) => {
//        $.ajax({
//            type: 'POST',
//            url: controlador + 'ListarSubestacion',
//            contentType: "application/json; charset=utf-8",
//            success: function (eData) {
//                cargarListaParamSubestacion(eData, selectHtml);
//                console.log(`Catálogo de subestaciones cargado: ${selectHtml}`);
//                resolve(); // Se resuelve la promesa cuando la carga finaliza
//            },
//            error: function (err) {
//              //  console.warn(`Error al cargar el catálogo de subestaciones (${selectHtml}). Se considera válido.`);
//                resolve(); // También resolvemos la promesa aunque haya error para evitar bloqueos
//            }
//        });
//    });
//}



function convertirAnoMes(fechaStr) {
    // Verificar si la fecha es null o vacía
    if (!fechaStr) {
        return '';
    }

    // Usar una expresión regular para extraer el día, mes y año de la fecha en formato "dd/mm/yyyy hh:mm:ss"
    const match = fechaStr.match(/^(\d{2})\/(\d{2})\/(\d{4})/);
    if (!match) {
        return ''; // Retorna vacío si el formato no coincide
    }

    // Extraer año y mes del resultado de la expresión regular
    const [, dia, mes, anio] = match;

    // Retornar el formato "yyyy/mm"
    return `${anio}/${mes}`;
}



function grabarFichaA() {
    scrollToTop();
    var param = {};
    param.TransmisionProyectoDTO = getCabeceraProyecto();
    var idProyecto = $("#txtPoyCodi").val();
    //cargarLoading();
    if (idProyecto != "0") {
        param.ProyCodi = idProyecto;
        param.RegHojaADTO = getHojaA();
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarFichaA',
            data: param,
            //
            success: function (result) {
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

function cargarUbicacionCHA(idDistrito) {
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




