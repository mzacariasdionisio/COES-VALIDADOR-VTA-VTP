var seccionUbicacion = 4;
var seccionInfBasica = 5;
var seccionEquipamiento = 6;
var catPerfil = 16;
var catPrefactibilidad = 17;
var catFactibilidad = 18;
var catEstudDefinitivo = 19;
var catEia = 20;
var catRadSolar = 35;
var catSegSolar = 36;

$(function () {
   
    $('#btnGrabarSolFichaA').on('click', function () {
        grabarFichaAG4();
    });


    $('#fecConsesionTemporal').Zebra_DatePicker({
        format: 'd/m/Y',
        onSelect: function () {
            cambiosRealizados = true; // Marcar que hubo un cambio
        }
    });

    $('#fecConsesionActual').Zebra_DatePicker({
        format: 'd/m/Y',
        onSelect: function () {
            cambiosRealizados = true; // Marcar que hubo un cambio
        }
    });
    $('#fechInicioConst').Zebra_DatePicker({
        format: 'm/Y',
        onSelect: function () {
            cambiosRealizados = true; // Marcar que hubo un cambio
        }
    });
    $('#fecOperacionComercial').Zebra_DatePicker({
        format: 'm/Y',
        onSelect: function () {
            cambiosRealizados = true; // Marcar que hubo un cambio
        }
    });

    var formularioA = document.getElementById('FichaA');
    formularioA.addEventListener('change', function () {
        cambiosRealizados = true;
        console.log(cambiosRealizados);
    });


    cargarDepartamentos();
    var ruta_interna = obtenerRutaProyecto();  
    crearUploaderGeneral('btnSubirUbicacionSol', '#tablaUbicacionSA', seccionUbicacion, null, ruta_interna);
    crearUploaderGeneral('btnSubirFormatoInfBasica', '#tablaInfBasicaSA', seccionInfBasica, null, ruta_interna);
    crearUploaderGeneral('btnSubirFormatoEquipamiento', '#tablaEquipamiento', seccionEquipamiento, null, ruta_interna);

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
    var selectDepartamentos = $('#cmbDepartamento');
    selectDepartamentos.empty(); // Limpiar opciones previas
    selectDepartamentos.append('<option value="">Seleccione un departamento</option>'); // Opción predeterminada

    $.each(departamentos, function (index, departamento) {
        var option = $('<option>', {
            value: departamento.Id,
            text: departamento.Nombre
        });

        selectDepartamentos.append(option);
    });

    selectDepartamentos.change(function () {
        // Limpiar las opciones de provincia y distrito al cambiar el departamento
        $('#cmbProvincia').empty().append('<option value="">Seleccione una provincia</option>');
        $('#cmbDistrito').empty().append('<option value="">Seleccione un distrito</option>');

        var idSeleccionado = $(this).val();
        if (idSeleccionado) {
            cargarProvincia(idSeleccionado);
        }
    });
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
    var selectProvincias = $('#cmbProvincia');
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
    var selectDistritos = $('#cmbDistrito');
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

function getSolHojaA() {
    var param = {};
    param.Centralnombre = $("#centralNombre").val();
    param.Distrito = $("#cmbDistrito").val();
    param.Propietario = $("#cmbPropietario").val();
    param.Otro = $("#otro").val();
    param.Sociooperador = $("#txtOperador").val();
    param.Socioinversionista = $("#txtInversionista").val();
    param.Concesiontemporal = $("#cmbConsesionTem").val();
    param.Tipoconcesionact = $("#cmbtipoConsesionActual").val();
    param.Fechaconcesiontem = $("#fecConsesionTemporal").val();
    param.Fechaconcesionact = $("#fecConsesionActual").val();

    param.Nomestacion = $("#nomEstacion").val();
    param.Serieradiacion = $("#cmbSerieRadiacion").val();
    param.Potinstnom = $("#potenciaInstala").val();
    param.Ntotalmodfv = $("#numTotalModulos").val();
    param.Horutilequ = $("#horUtilizadasEq").val();
    param.Eneestanual = $("#energiaEstimada").val();


    param.Facplantaact = $("#factorPlanta").val();
    param.Tecnologia = $("#tecnologia").val();
    param.Potenciapico = $("#potenciaPico").val();
    param.Nivelradsol = $("#nivelRadiacion").val();
    param.Seguidorsol = $("#cmbSeguidorSolar").val();
    param.Volpunmax = $("#voltagePunto").val();
    param.Intpunmax = $("#intensidadPuntoMax").val();
    param.Modelo = $("#modelo").val();

    param.Entpotmax = $("#entradaPotencia").val();
    param.Salpotmax = $("#salidaPotencia").val();
    param.Siscontro = $("#sistemaControl").val();
    param.Baterias = $("#cmbTieneBateria").val();
    param.Enemaxbat = $("#energiaMax").val();
    param.Potmaxbat = $("#potenciaMax").val();
    param.Eficargamax = $("#eficienciaCarga").val();
    param.Efidesbat = $("#efiDescarga").val();
    param.Timmaxreg = $("#tiempoMax").val();
    param.Rampascardes = $("#rampaCarga").val();
    param.Tension = $("#tension").val();

    param.Longitud = $("#longitud").val();
    param.Numternas = $("#numTemas").val();
    param.Nombsubest = $("#nomSubestacion").val();
    param.Perfil = $("#cmbPerfil").val();
    param.Prefact = $("#cmbPrefactibilidad").val();
    param.Factibilidad = $("#cmbFactibilidad").val();
    param.Estdefinitivo = $("#cmbEstudioDef").val();

    param.Eia = $("#cmbEia").val();
    param.Fecinicioconst = $("#fechInicioConst").val();
    param.Perconstruccion = $("#periodoConstruccion").val();
    param.Fecoperacioncom = $("#fecOperacionComercial").val();
    param.Comentarios = $("#txtComentarioA").val();
    return param;
}

function eliminarFile(id) {
    document.getElementById("contenidoPopup").innerHTML = '¿Está seguro de realizar esta operación?';
    $('#popupProyectoGeneral').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
    $('#btnConfirmarPopup').off('click').on('click', function () {
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

function crearPupload() {
    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSubirFormato',
        url: controladorFichas + 'UploadArchivoGeneral',
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

function crearPuploadInfBas() {
    uploaderInfBas = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSubirFormatoInfBasica',
        url: controladorFichas + 'UploadArchivoGeneral',
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
                if (uploaderInfBas.files.length == 2) {
                    uploaderInfBas.removeFile(uploaderInfBas.files[0]);
                }
                uploaderInfBas.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarMensaje('mensajeFicha', 'alert', "Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            FileUploaded: function (up, file, response) {
                var json = JSON.parse(response.response);
                if (json.indicador == 1) {
                    up.removeFile(file);
                    procesarArchivoInfBas(json.fileNameNotPath, json.nombreReal, json.extension)
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
    uploaderInfBas.init();
}

function crearPuploadEquipamiento() {
    uploaderEquipamiento = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSubirFormatoEquipamiento',
        url: controladorFichas + 'UploadArchivoGeneral',
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
                if (uploaderEquipamiento.files.length == 2) {
                    uploaderEquipamiento.removeFile(uploaderEquipamiento.files[0]);
                }
                uploaderEquipamiento.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarMensaje('mensajeFicha', 'alert', "Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            FileUploaded: function (up, file, response) {
                var json = JSON.parse(response.response);
                if (json.indicador == 1) {
                    up.removeFile(file);
                    procesarArchivoEquipamiento(json.fileNameNotPath, json.nombreReal, json.extension)
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
    uploaderEquipamiento.init();
}

function procesarArchivoInfBas(fileNameNotPath, nombreReal, tipo) {
    param = {};
    param.SeccCodi = seccionInfBasica;
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
                agregarFilaInfBas(nombreReal, result.id);
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

function procesarArchivoEquipamiento(fileNameNotPath, nombreReal, tipo) {
    param = {};
    param.SeccCodi = seccionEquipamiento;
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
                agregarFilaEquipamiento(nombreReal, result.id);
                cambiosRealizados = false;
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

function procesarArchivoUbicacion(fileNameNotPath, nombreReal, tipo) {
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
                agregarFilaUbicacion(nombreReal, result.id);
                cambiosRealizados = false;
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

function agregarFilaUbicacion(nombre, id) {
    // Obtener el cuerpo de la tabla
    var nuevaFila = '<tr id="fila' + id + '">' +
        '<td>' + nombre + '</td>' +
        '<td> <a href="#"><img src="' + siteRoot + 'Content/Images/eliminar.png" class="btnEliminar" onclick="eliminarFile(' + id + ')" title="Eliminar archivo"/></a></td>' +
        '</tr>';
    var tabla = $('#tablaUbicacionSA');
    tabla.append(nuevaFila);
}

function agregarFilaInfBas(nombre, id) {
    // Obtener el cuerpo de la tabla
    var nuevaFila = '<tr id="fila' + id + '">' +
        '<td>' + nombre + '</td>' +
        '<td> <a href="#"><img src="' + siteRoot + 'Content/Images/eliminar.png" class="btnEliminar" onclick="eliminarFile(' + id + ')" title="Eliminar archivo"/></a></td>' +
        '</tr>';
    var tabla = $('#tablaInfBasicaSA');
    tabla.append(nuevaFila);
}

function agregarFilaEquipamiento(nombre, id) {
    // Obtener el cuerpo de la tabla
    var nuevaFila = '<tr id="fila' + id + '">' +
        '<td>' + nombre + '</td>' +
        '<td> <a href="#"><img src="' + siteRoot + 'Content/Images/eliminar.png" class="btnEliminar" onclick="eliminarFile(' + id + ')" title="Eliminar archivo"/></a></td>' +
        '</tr>';
    var tabla = $('#tablaEquipamiento');
    tabla.append(nuevaFila);
}

function grabarFichaAG4() {
    var param = {};
    param.TransmisionProyectoDTO = getCabeceraProyecto();
    var idProyecto = $("#txtPoyCodi").val();
    if (idProyecto != "0") {
        param.ProyCodi = idProyecto;
        param.SolHojaADTO = getSolHojaA();
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarSolHojaA',
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
function cargarUbicacionBioAG4(idDistrito) {
    $.ajax({
        type: 'POST',
        url: controladorFichas + 'UbicacionByDistro',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: idDistrito }),
        success: function (ubicacion) {
            $('#cmbDepartamento').val(ubicacion.DepartamentoId);

            cargarProvincia(ubicacion.DepartamentoId, function () {
                $('#cmbProvincia').val(ubicacion.ProvinciaId);

                cargarDistrito(ubicacion.ProvinciaId, function () {
                    $('#cmbDistrito').val(ubicacion.DistritoId);

                });
            });
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}
// Ejecutar al cargar la página para el caso de "Propietario"
$(document).ready(function () {
   // toggleInputGlobal("cmbPropietario", "otro", "0"); // Aplicar la lógica al iniciar

    // Agregar evento para detectar cambios manuales en el select
    $("#cmbPropietario").on("change", function () {
        toggleInputGlobal("cmbPropietario", "otro", "0");
    });
});


function setSolHojaA(param) {
    $("#centralNombre").val(param.Centralnombre);
    cargarUbicacionBioAG4(param.Distrito);
    $("#cmbPropietario").val(param.Propietario);
    $("#otro").val(param.Otro);
    toggleInputGlobal("cmbPropietario", "otro", "0");
    $("#txtOperador").val(param.Sociooperador);
    $("#txtInversionista").val(param.Socioinversionista);
    $("#cmbConsesionTem").val(param.Concesiontemporal);
    $("#cmbtipoConsesionActual").val(param.Tipoconcesionact);
    setFechaPickerGlobal("#fecConsesionTemporal", param.Fechaconcesiontem, "dd/mm/aaaa");
    setFechaPickerGlobal("#fecConsesionActual", param.Fechaconcesionact, "dd/mm/aaaa");
    $("#nomEstacion").val(param.Nomestacion);
    $("#cmbSerieRadiacion").val(param.Serieradiacion);
    $("#potenciaInstala").val(param.Potinstnom);
    $("#numTotalModulos").val(param.Ntotalmodfv);
    $("#horUtilizadasEq").val(param.Horutilequ);
    $("#energiaEstimada").val(param.Eneestanual);

    $("#factorPlanta").val(param.Facplantaact);
    $("#tecnologia").val(param.Tecnologia);
    $("#potenciaPico").val(param.Potenciapico);
    $("#nivelRadiacion").val(param.Nivelradsol);
    $("#cmbSeguidorSolar").val(param.Seguidorsol);
    $("#voltagePunto").val(param.Volpunmax);
    $("#intensidadPuntoMax").val(param.Intpunmax);
    $("#modelo").val(param.Modelo);

    $("#entradaPotencia").val(param.Entpotmax);
    $("#salidaPotencia").val(param.Salpotmax);
    $("#sistemaControl").val(param.Siscontro);
    $("#cmbTieneBateria").val(param.Baterias);
    $("#energiaMax").val(param.Enemaxbat);
    $("#potenciaMax").val(param.Potmaxbat);
    $("#eficienciaCarga").val(param.Eficargamax);
    $("#efiDescarga").val(param.Efidesbat);
    $("#tiempoMax").val(param.Timmaxreg);
    $("#rampaCarga").val(param.Rampascardes);
    $("#tension").val(param.Tension);

    $("#longitud").val(param.Longitud);
    $("#numTemas").val(param.Numternas);
    $("#nomSubestacion").val(param.Nombsubest);
    $("#cmbPerfil").val(param.Perfil);
    $("#cmbPrefactibilidad").val(param.Prefact);
    $("#cmbFactibilidad").val(param.Factibilidad);
    $("#cmbEstudioDef").val(param.Estdefinitivo);

    $("#cmbEia").val(param.Eia);
    $("#fechInicioConst").val(param.Fecinicioconst);
    $("#periodoConstruccion").val(param.Perconstruccion);
    $("#fecOperacionComercial").val(param.Fecoperacioncom);
    $("#txtComentarioA").val(param.Comentarios);

    cargarArchivosRegistrados(seccionUbicacion, '#tablaUbicacionSA');
    cargarArchivosRegistrados(seccionInfBasica, '#tablaInfBasicaSA');
    cargarArchivosRegistrados(seccionEquipamiento, '#tablaEquipamiento');
    if (modoModel == "consultar") {
        desactivarCamposFormulario('FichaA');
        $("#btnGrabarSolFichaA").hide();
    }

}

// Validar los campos para aceptar solo números positivos
document.querySelectorAll('.txtnumber').forEach(input => {
    input.addEventListener('input', function () {
        // Remover cualquier carácter que no sea un número o un punto decimal
        this.value = this.value.replace(/[^0-9.]/g, '');

        // Permitir solo un punto decimal
        const parts = this.value.split('.');
        if (parts.length > 2) {
            this.value = parts[0] + '.' + parts.slice(1).join('');
        }

        // Remover los ceros iniciales
        if (this.value.length > 1 && this.value[0] === '0' && this.value[1] !== '.') {
            this.value = this.value.substring(1);
        }

        // Si el valor es negativo, establecerlo en vacío
        if (parseFloat(this.value) < 0) {
            this.value = '';
        }
    });
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

async function cargarCatalogosYAsignarValoresAG4(param) {

 


    console.log("Verificando y cargando catálogos...");

    let promesas = [

    esperarCatalogo(catPropietario, "#cmbPropietario"),
    esperarCatalogo(catConcesionTemporal, "#cmbConsesionTem"),
    esperarCatalogo(catConcesionActual, "#cmbtipoConsesionActual"),
    esperarCatalogo(catPerfil, "#cmbPerfil"),
    esperarCatalogo(catPrefactibilidad, "#cmbPrefactibilidad"),
    esperarCatalogo(catFactibilidad, "#cmbFactibilidad"),
    esperarCatalogo(catEstudDefinitivo, "#cmbEstudioDef"),
    esperarCatalogo(catEia, "#cmbEia"),
    esperarCatalogo(catSegSolar, "#cmbSeguidorSolar"),
    esperarCatalogo(catRadSolar, "#cmbSerieRadiacion")


    ];

    // Asegurar que todas las promesas de carga se completen antes de continuar
    await Promise.all(promesas);

    console.log("Todos los catálogos han sido cargados. Asignando valores...");



    asignarSiExisteGlobal("#cmbPropietario", param.Propietario);
    asignarSiExisteGlobal("#cmbConsesionTem", param.Concesiontemporal);
    asignarSiExisteGlobal("#cmbtipoConsesionActual", param.Tipoconcesionact);
    asignarSiExisteGlobal("#cmbPerfil", param.Perfil);
    asignarSiExisteGlobal("#cmbPrefactibilidad", param.Prefact);
    asignarSiExisteGlobal("#cmbFactibilidad", param.Factibilidad);
    asignarSiExisteGlobal("#cmbEstudioDef", param.Estdefinitivo);
    asignarSiExisteGlobal("#cmbEia", param.cmbEia);
    asignarSiExisteGlobal("#cmbSeguidorSolar", param.Seguidorsol);
    asignarSiExisteGlobal("#cmbSerieRadiacion", param.Serieradiacion);
    console.log("Valores asignados correctamente.");
}


async function cargarDatosA() {
    limpiarCombos('#cmbPropietario, #cmbConsesionTem, #cmbtipoConsesionActual, #cmbPerfil, #cmbPrefactibilidad, #cmbFactibilidad, #cmbEstudioDef, #cmbEia, #cmbSeguidorSolar, #cmbSerieRadiacion');

    // Asegurarse de que todos los catálogos estén listos antes de continuar
    await cargarCatalogosYAsignarValoresAG4({});

    console.log("Editar");
    if (modoModel == "editar" || modoModel == "consultar") {
        var idProyecto = $("#txtPoyCodi").val();
        $.ajax({
            type: 'POST',
            url: controladorFichas + 'GetSolHojaA',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: idProyecto }),

            success: function (response) {
                console.log(response);
                var hojaAData = response.responseResult;

                if (hojaAData.Proycodi == 0) {
                    //hojaAData.Fechaconcesiontem = obtenerFechaActual();
                    //hojaAData.Fechaconcesionact = obtenerFechaActual();
                    //hojaAData.Fecinicioconst = obtenerFechaActualMesAnio();
                    //hojaAData.Fecoperacioncom = obtenerFechaActualMesAnio();
                } else {
                    hojaAData.Fechaconcesiontem = convertirFechaGlobal(hojaAData.Fechaconcesiontem);
                    hojaAData.Fechaconcesionact = convertirFechaGlobal(hojaAData.Fechaconcesionact);

                }

                setSolHojaA(hojaAData);
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}
