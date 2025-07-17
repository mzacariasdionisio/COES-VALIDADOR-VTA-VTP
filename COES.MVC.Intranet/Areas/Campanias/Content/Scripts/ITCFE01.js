var seccionUnifiliarActual = 7;
var seccionUnifiliarExpancion = 8;
var seccionBaseDatosModelo = 9;


$(function () {
    var ruta_interna = obtenerRutaProyecto();  
    crearUploaderGeneral("btnPDFoDWG1", "#tablaUnifamiliarActual", seccionUnifiliarActual, null, ruta_interna);
    crearUploaderGeneral("btnPDFoDWG2", "#tablaUnifamiliarExpancion", seccionUnifiliarExpancion, null, ruta_interna);
    crearUploaderGeneral("btnPDFoXLS", "#tablaModeloRed", seccionBaseDatosModelo, null, ruta_interna);

});

function procesarArchivoFE01(fileNameNotPath, nombreReal, tipo, tabla, seccionCodigo) {
    param = {};
    param.SeccCodi = seccionCodigo;
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
                agregarFilaTabla(nombreReal, result.id, tabla);
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


function agregarFilaTabla(nombre, id, tabla) {
    // Obtener el cuerpo de la tabla
    var nuevaFila = '<tr id="fila' + id + '">' +
        '<td>' + nombre + '</td>' +
        '<td> </td>' +
        '</tr>';
    var tabla = $(tabla);
    tabla.append(nuevaFila);
}


function cargarFe01() {
    console.log("Editar");
    if (modoModel == "editar" || modoModel == "consultar") {
        cargarArchivosRegistrados(seccionUnifiliarActual, '#tablaUnifamiliarActual');
        cargarArchivosRegistrados(seccionUnifiliarExpancion, '#tablaUnifamiliarExpancion');
        cargarArchivosRegistrados(seccionBaseDatosModelo, '#tablaModeloRed');
    }
}


function cargarFe01() {
    console.log("Editar");
    if (modoModel === "editar" || modoModel === "consultar") {
        var idProyecto = $("#txtPoyCodi").val();
        $.ajax({
            type: 'POST',
            url: controladorFichas + 'GetITCFE01',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: idProyecto }),
            success: function (response) {
                console.log(response);
                var hojaAData = response.responseResult;
                setComentarioITCFE01(hojaAData);
                cargarArchivosRegistrados(seccionUnifiliarActual, '#tablaUnifamiliarActual');
                cargarArchivosRegistrados(seccionUnifiliarExpancion, '#tablaUnifamiliarExpancion');
                cargarArchivosRegistrados(seccionBaseDatosModelo, '#tablaModeloRed');
                cambiosRealizados = false;
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}


function getComentarioITCFE01(){
    var param = {};
    param.Comentarios = $("#txtComentarioA").val();
    return param;
}

function setComentarioITCFE01(resultado) {

    $("#txtComentarioA").val(resultado.Comentarios);
    if (modoModel == "consultar") {
        desactivarCamposFormulario('fe01');
        $("#btnGrabarFichaitcde10").hide();
        $("#btnPDFoDWG1").remove();
        $("#btnPDFoDWG2").remove();
        $("#btnPDFoXLS").remove();
    }
}

function guardarComentario() {
    scrollToTop();
    var param = {};
    var idProyecto = $("#txtPoyCodi").val();
    if (idProyecto != "0") {
        param.ProyCodi = idProyecto;
        param.ITCFE01DTO = getComentarioITCFE01();
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarITCFE01',
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
}

// Funci�n general para inicializar el uploader
