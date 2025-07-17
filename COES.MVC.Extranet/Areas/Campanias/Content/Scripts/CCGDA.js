var seccionCCGDA1 = 22;
var seccionCCGDA2 = 23;
var catObjProyecto = 52;
var catTituloHab = 64;
var catEstadoOperacion = 63;
var catPerfil = 16;
var catPreFacltibilidad = 17;
var catFacltibilidad = 18;
var catEstudDef = 19;
var catEia = 20;
var catRecursoUsado = 30;
var catTecnologia = 31;
$(function () {
    // Inicializa Combo 1
    //cargarOpciones('cmbDepartamento1', controladorFichas + 'ListarDepartamentos');
    // Inicializa Combo 2
    //cargarOpciones('cmbDepartamento2', controladorFichas + 'ListarDepartamentos');
    cargarDepartamentosCCGDA();
    cargarDepartamentosCCGDA2();
    //$('#fechaAdjudicactem').val(obtenerFechaActual());
    //$('#fechaAdjutitulo').val(obtenerFechaActual());
    //$('#fechaInicioConst').val(obtenerFechaActualMesAnio());
    //$('#FechaOpeComercial').val(obtenerFechaActualMesAnio());

    $('#fechaAdjudicactem').Zebra_DatePicker({
        format: 'd/m/Y',
        onSelect: function () {
            cambiosRealizados = true;
        }
    });

    $('#fechaAdjutitulo').Zebra_DatePicker({
        format: 'd/m/Y',
        onSelect: function () {
            cambiosRealizados = true;
        }
    });
    $('#fechaInicioConst').Zebra_DatePicker({
        format: 'm/Y',
        onSelect: function () {
            cambiosRealizados = true;
        }
    });
    $('#FechaOpeComercial').Zebra_DatePicker({
        format: 'm/Y',
        onSelect: function () {
            cambiosRealizados = true;
        }
    });

    var ruta_interna = obtenerRutaProyecto();  
    crearUploaderGeneral('btnSubirUbicacionCCGDA1', '#tablaCCGDA1', seccionCCGDA1, null, ruta_interna);
    crearUploaderGeneral('btnSubirUbicacionCCGDA2', '#tablaCCGDA2', seccionCCGDA2, null, ruta_interna);

    var formularioccgda = document.getElementById('CCGDA');
    $(formularioccgda).off('change').on('change', function () {
        cambiosRealizados = true;
    });
});

getDataCCGDA = function () {
    var param = {};
    param.CcgdaCodi = parseInt($("#ccgdaCodi").val());
    param.ProyCodi = parseInt($("#proyCodi").val());
    param.NombreUnidad = $("#nombreUnidad").val();
    param.DistritoCodi = $("#distritosSelectA").val();
    param.NombreDistribuidor = $("#nombreDistribuidor").val();
    param.Propietario = $("#propietario").val();
    param.SocioOperador = $("#socioOperador").val();
    param.SocioInversionista = $("#socioInversionista").val();
    param.ObjetivoProyecto = $("#objetivoProyecto").val();
    param.OtroObjetivo = $("#otroObjetivo").val();
    //param.IncluidoPlanTrans = $("#rbIncluidoPlanTrans").val();
    param.IncluidoPlanTrans = $('input[name="rbIncluidoPlanTrans"]:checked').val();
    param.EstadoOperacion = $("#estadoOperacion").val();
    param.CargaRedDistribucion = $('input[name="rbCargaRedDistribucion"]:checked').val();
    //param.CargaRedDistribucion = $("#cargaRedDistribucion").val();
    //param.ConexionTemporal = $("#rbConcesionTemporal").val();
    param.ConexionTemporal = $('input[name="rbConcesionTemporal"]:checked').val();
    param.TipoTecnologia = $("#tipoTecnologia").val();
    param.FechaAdjudicactem =$("#fechaAdjudicactem").val();
    param.FechaAdjutitulo =$("#fechaAdjutitulo").val();
    param.Perfil = $("#perfil").val();
    param.Prefactibilidad = $("#prefactibilidad").val();
    param.Factibilidad = $("#factibilidad").val();
    param.EstDefinitivo = $("#estDefinitivo").val();
    param.Eia = $("#eia").val();
    param.FechaInicioConst = $("#fechaInicioConst").val();
    param.PeriodoConst = $("#periodoConst").val();
    param.FechaOpeComercial = $("#FechaOpeComercial").val();
    param.PotInstalada = $("#potInstalada").val();
    param.RecursoUsada = $("#recursoUsada").val();
    param.Tecnologia = $("#tecnologia").val();
    param.TecOtro = $("#tecOtro").val();
    param.BarraConexion = $("#barraConexion").val();
    param.NivelTension = $("#nivelTension").val();
    //param.IncluidoPlanTransGD = $("#rbIncluidoPlanTransGD").val();
    param.IncluidoPlanTransGD = $('input[name="rbIncluidoPlanTransgd"]:checked').val();
    param.NombreProyectoGD = $("#nombreProyectoGD").val();
    param.DistritoGDCodi = $("#distritosSelectA1").val();
    param.NomDistribuidorGD = $("#nomDistribuidorGD").val();
    param.PropietarioGD = $("#propietarioGD").val();
    param.SocioOperadorGD = $("#socioOperadorGD").val();
    param.SocioInversionistaGD = $("#socioInversionistaGD").val();
    param.EstadoOperacionGD = $("#estadoOperacionGD").val();
    //param.CargaRedDistribucionGD = $("#rbCargaRedDistribucionGD").val();
    param.CargaRedDistribucionGD = $('input[name="rbCargaRedDistribucionGD"]:checked').val();
    param.BarraConexionGD = $("#barraConexionGD").val();
    param.NivelTensionGD = $("#nivelTensionGD").val();
    param.Comentarios = $("#comentarios").val();
    console.log(param);

    return param;
}

function grabarCCGDA() {
    var param = {};
    param.TransmisionProyectoDTO = getCabeceraProyecto();
    var idProyecto = $("#txtPoyCodi").val();
    if (idProyecto != "0") {
        param.ProyCodi = idProyecto;
        param.CcgdaDTO = getDataCCGDA();
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarCCGDA',
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
                //$('#popupProyecto').bPopup().close()
                mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error.');
            }
        });
    }
    cambiosRealizados = false;
}


setDataCCGDA = function (param) {
    $("#ccgdaCodi").val(param.CcgdaCodi);
    $("#proyCodi").val(param.ProyCodi);
    $("#nombreUnidad").val(param.NombreUnidad);
    //$("#distritoCodi").val(param.DistritoCodi);
    cargarUbicacionCCGDA(param.DistritoCodi);
    $("#nombreDistribuidor").val(param.NombreDistribuidor);
    $("#propietario").val(param.Propietario);
    $("#socioOperador").val(param.SocioOperador);
    $("#socioInversionista").val(param.SocioInversionista);
    $("#objetivoProyecto").val(param.ObjetivoProyecto);
    $("#otroObjetivo").val(param.OtroObjetivo);
    //$("#rbIncluidoPlanTrans").val(param.IncluidoPlanTrans);
    $(`#rbIncluidoPlanTrans${param.IncluidoPlanTrans}`).prop("checked", true);
    $("#estadoOperacion").val(param.EstadoOperacion);
    //$("#rbCargaRedDistribucion").val(param.CargaRedDistribucion);
    $(`#rbCargaRedDistribucion${param.CargaRedDistribucion}`).prop("checked", true);
    //$("#rbConcesionTemporal").val(param.ConexionTemporal);
    $(`#rbConcesionTemporal${param.ConexionTemporal}`).prop("checked", true);
    $("#tipoTecnologia").val(param.TipoTecnologia);
    $("#fechaAdjudicactem").val(param.FechaAdjudicactem);
    $("#fechaAdjutitulo").val(param.FechaAdjutitulo);
    $("#perfil").val(param.Perfil);
    $("#prefactibilidad").val(param.Prefactibilidad);
    $("#factibilidad").val(param.Factibilidad);
    $("#estDefinitivo").val(param.EstDefinitivo);
    $("#eia").val(param.Eia);
    $("#fechaInicioConst").val(param.FechaInicioConst);
    $("#periodoConst").val(param.PeriodoConst);
    $("#FechaOpeComercial").val(param.FechaOpeComercial);
    $("#potInstalada").val(param.PotInstalada);
    $("#recursoUsada").val(param.RecursoUsada);
    $("#tecnologia").val(param.Tecnologia);
    $("#tecOtro").val(param.TecOtro);
    $("#barraConexion").val(param.BarraConexion);
    $("#nivelTension").val(param.NivelTension);
    //$("#rbIncluidoPlanTransGD").val(param.IncluidoPlanTransGD);
    $(`#rbIncluidoPlanTransgd${param.IncluidoPlanTransGD}`).prop("checked", true);
    $("#nombreProyectoGD").val(param.NombreProyectoGD);
    //$("#distritoGDCodi").val(param.DistritoGDCodi);
    cargarUbicacionCCGDA2(param.DistritoGDCodi);
    $("#nomDistribuidorGD").val(param.NomDistribuidorGD);
    $("#propietarioGD").val(param.PropietarioGD);
    $("#socioOperadorGD").val(param.SocioOperadorGD);
    $("#socioInversionistaGD").val(param.SocioInversionistaGD);
    $("#estadoOperacionGD").val(param.EstadoOperacionGD);
    $(`#rbCargaRedDistribucionGD${param.CargaRedDistribucionGD}`).prop("checked", true);
    //$("#cargaRedDistribucionGD").val(param.CargaRedDistribucionGD);
    $("#barraConexionGD").val(param.BarraConexionGD);
    $("#nivelTensionGD").val(param.NivelTensionGD);
    $("#comentarios").val(param.Comentarios);
    setupDropdownToggle('objetivoProyecto', 'otroObjetivo');
    setupDropdownToggle('tecnologia', 'tecOtro');
    cargarArchivosRegistrados(seccionCCGDA1, '#tablaCCGDA1');
    cargarArchivosRegistrados(seccionCCGDA2, '#tablaCCGDA2');
    if (modoModel == "consultar") {
        desactivarCamposFormulario('CCGDA');
        $('#btnGrabarCCGDA').hide();
    }
}
async function cargarCatalogosYAsignarValoresAGDA(param) {
    console.log("Verificando y cargando catálogos...");

    let promesas = [
        esperarCatalogo(catObjProyecto, "#objetivoProyecto"),
        esperarCatalogo(catTituloHab, "#tipoTecnologia"),
        esperarCatalogo(catEstadoOperacion, "#estadoOperacion"),
        esperarCatalogo(catPerfil, "#perfil"),
        esperarCatalogo(catPreFacltibilidad, "#prefactibilidad"),
        esperarCatalogo(catFacltibilidad, "#factibilidad"),
        esperarCatalogo(catEstudDef, "#estDefinitivo"),
        esperarCatalogo(catEia, "#eia"),
        esperarCatalogo(catRecursoUsado, "#recursoUsada"),
        esperarCatalogo(catTecnologia, "#tecnologia"),
        esperarCatalogo(catEstadoOperacion, "#estadoOperacionGD")
    ];

    // Asegurar que todas las promesas de carga se completen antes de continuar
    await Promise.all(promesas);

    console.log("Todos los catálogos han sido cargados. Asignando valores...");


    asignarSiExisteGlobal("#objetivoProyecto", param.ObjetivoProyecto);
    asignarSiExisteGlobal("#tipoTecnologia", param.TipoTecnologia);
    asignarSiExisteGlobal("#estadoOperacion", param.EstadoOperacion);
    asignarSiExisteGlobal("#perfil", param.Perfil);
    asignarSiExisteGlobal("#prefactibilidad", param.Prefactibilidad);
    asignarSiExisteGlobal("#factibilidad", param.Factibilidad);
    asignarSiExisteGlobal("#estDefinitivo", param.EstDefinitivo);
    asignarSiExisteGlobal("#eia", param.Eia);
    asignarSiExisteGlobal("#recursoUsada", param.RecursoUsada);
    asignarSiExisteGlobal("#tecnologia", param.Tecnologia);
    asignarSiExisteGlobal("#estadoOperacionGD", param.EstadoOperacionGD);

    console.log("Valores asignados correctamente.");
}

async function cargarCCGDA() {

    limpiarCombos('#objetivoProyecto ,#tipoTecnologia ,#estadoOperacion ,#perfil ,#prefactibilidad ,#factibilidad ,#estDefinitivo ,#eia ,#recursoUsada ,#tecnologia ,#estadoOperacionGD ');

    // Asegurarse de que todos los catálogos estén listos antes de continuar
    await cargarCatalogosYAsignarValoresAGDA({});

    console.log("Editar");
    if (modoModel == "editar" || modoModel == "consultar") {
        var idProyecto = $("#txtPoyCodi").val();
        $.ajax({
            type: 'POST',
            url: controladorFichas + 'GetCCGDA',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: idProyecto }),
            
            success: function (response) {
                console.log(response);
                var ccgdaData = response.responseResult;

                if (ccgdaData.Proycodi == 0) {
                    //ccgdaData.FechaAdjudicactem = obtenerFechaActual();
                    //ccgdaData.FechaAdjutitulo = obtenerFechaActual();
                    //ccgdaData.FechaInicioConst = obtenerFechaActualMesAnio();
                    //ccgdaData.FechaOpeComercial = obtenerFechaActualMesAnio();
                } else {
                    ccgdaData.FechaAdjudicactem = convertirFechaGlobal(ccgdaData.FechaAdjudicactem);
                    ccgdaData.FechaAdjutitulo = convertirFechaGlobal(ccgdaData.FechaAdjutitulo);

                }

                setDataCCGDA(ccgdaData);
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function cargarOpciones(selectId, url, id) {
    $.ajax({
        type: 'POST',
        url: url,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(id ? { id: id } : ''),
        success: function (eData) {
            console.log(eData);
            cargarLista(selectId, eData);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarLista(selectId, opciones) {
    var select = $('#' + selectId);
    select.empty(); // Limpiar opciones anteriores

    var optionEmpty = $('<option>', {
        value: '',
        text: 'Seleccione'
    });
    select.append(optionEmpty);
    $.each(opciones, function (index, opcion) {
        // Crear la opci�n
        var option = $('<option>', {
            value: opcion.Id,
            text: opcion.Nombre
        });

        // Agregar la opci�n al select
        select.append(option);
    });

    select.change(function () {
        var idSeleccionado = $(this).val();
        if (selectId.includes('Departamento')) {
            // Si el select es de departamento, cargar provincias correspondientes
            cargarOpciones(selectId.replace('Departamento', 'Provincia'), controladorFichas + 'ListarProvincias', idSeleccionado);
        } else if (selectId.includes('Provincia')) {
            // Si el select es de provincia, cargar distritos correspondientes
            cargarOpciones(selectId.replace('Provincia', 'Distrito'), controladorFichas + 'ListarDistritos', idSeleccionado);
        }
    });
}


function convertirFecha(fechaJSON) {

    let timestamp = parseInt(fechaJSON.match(/\d+/)[0]);

    let fecha = new Date(timestamp);

    let dia = fecha.getDate().toString().padStart(2, '0');
    let mes = (fecha.getMonth() + 1).toString().padStart(2, '0');
    let anio = fecha.getFullYear();

    let fechaFormateada = `${dia}-${mes}-${anio}`;
    return fechaFormateada;
}


function cargarDepartamentosCCGDA() {
    $.ajax({
        type: 'POST',
        url: controladorFichas + 'ListarDepartamentos',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(''),
        success: function (eData) {
            cargarListaDepartamentoCCGDA(eData);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}
function cargarListaDepartamentoCCGDA(departamentos) {
    var selectDepartamentos = $('#departamentosSelectA');
    $.each(departamentos, function (index, departamento) {
        var option = $('<option>', {
            value: departamento.Id,
            text: departamento.Nombre
        });
        selectDepartamentos.append(option);
    });
    selectDepartamentos.change(function () {
        var idSeleccionado = $(this).val();
        cargarProvinciaCCGDA(idSeleccionado);
    });

}
function cargarProvinciaCCGDA(id, callback) {
    $.ajax({
        type: 'POST',
        url: controladorFichas + 'ListarProvincias',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: id }),
        success: function (eData) {
            cargarListaProvinciaCCGDA(eData);
            if (typeof callback === 'function') {
                callback();
            }

        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}
function cargarListaProvinciaCCGDA(provincias) {
    var selectProvincias = $('#provinciasSelectA');
    selectProvincias.empty();
    var optionDefault = $('<option>', {
        value: "",
        text: "Seleccione una provincia"
    });
    selectProvincias.append(optionDefault);
    cargarListaDistritosCCGDA([]);
    $.each(provincias, function (index, provincia) {
        var option = $('<option>', {
            value: provincia.Id,
            text: provincia.Nombre
        });
        selectProvincias.append(option);
    });

    selectProvincias.change(function () {
        var idSeleccionado = $(this).val();
        cargarDistritoCCGDA(idSeleccionado);
    });

}
function cargarDistritoCCGDA(id, callback) {
    $.ajax({
        type: 'POST',
        url: controladorFichas + 'ListarDistritos',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: id }),
        success: function (eData) {
            console.log(eData);
            cargarListaDistritosCCGDA(eData);
            if (typeof callback === 'function') {
                callback();
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}
function cargarListaDistritosCCGDA(distritos) {
    var selectDistritos = $('#distritosSelectA');
    selectDistritos.empty();
    var optionDefault = $('<option>', {
        value: "",
        text: "Seleccione un distrito"
    });
    selectDistritos.append(optionDefault);
    $.each(distritos, function (index, distrito) {
        var option = $('<option>', {
            value: distrito.Id,
            text: distrito.Nombre
        });
        selectDistritos.append(option);
    });

}
function cargarUbicacionCCGDA(idDistrito) {
    $.ajax({
        type: 'POST',
        url: controladorFichas + 'UbicacionByDistro',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: idDistrito }),
        success: function (ubicacion) {
            $('#departamentosSelectA').val(ubicacion.DepartamentoId);

            cargarProvinciaCCGDA(ubicacion.DepartamentoId, function () {
                $('#provinciasSelectA').val(ubicacion.ProvinciaId);

                cargarDistritoCCGDA(ubicacion.ProvinciaId, function () {
                    $('#distritosSelectA').val(ubicacion.DistritoId);

                });
            });
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarDepartamentosCCGDA2() {
    $.ajax({
        type: 'POST',
        url: controladorFichas + 'ListarDepartamentos',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(''),
        success: function (eData) {
            cargarListaDepartamentoCCGDA2(eData);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}
function cargarListaDepartamentoCCGDA2(departamentos) {
    var selectDepartamentos = $('#departamentosSelectA1');
    $.each(departamentos, function (index, departamento) {
        var option = $('<option>', {
            value: departamento.Id,
            text: departamento.Nombre
        });
        selectDepartamentos.append(option);
    });
    selectDepartamentos.change(function () {
        var idSeleccionado = $(this).val();
        cargarProvinciaCCGDA2(idSeleccionado);
    });

}
function cargarProvinciaCCGDA2(id, callback) {
    $.ajax({
        type: 'POST',
        url: controladorFichas + 'ListarProvincias',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: id }),
        success: function (eData) {
            cargarListaProvinciaCCGDA2(eData);
            if (typeof callback === 'function') {
                callback();
            }

        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}
function cargarListaProvinciaCCGDA2(provincias) {
    var selectProvincias = $('#provinciasSelectA1');
    selectProvincias.empty();
    var optionDefault = $('<option>', {
        value: "",
        text: "Seleccione una provincia"
    });
    selectProvincias.append(optionDefault);
    cargarListaDistritosCCGDA2([]);
    $.each(provincias, function (index, provincia) {
        var option = $('<option>', {
            value: provincia.Id,
            text: provincia.Nombre
        });
        selectProvincias.append(option);
    });

    selectProvincias.change(function () {
        var idSeleccionado = $(this).val();
        cargarDistritoCCGDA2(idSeleccionado);
    });

}
function cargarDistritoCCGDA2(id, callback) {
    $.ajax({
        type: 'POST',
        url: controladorFichas + 'ListarDistritos',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: id }),
        success: function (eData) {
            console.log(eData);
            cargarListaDistritosCCGDA2(eData);
            if (typeof callback === 'function') {
                callback();
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}
function cargarListaDistritosCCGDA2(distritos) {
    var selectDistritos = $('#distritosSelectA1');
    selectDistritos.empty();
    var optionDefault = $('<option>', {
        value: "",
        text: "Seleccione un distrito"
    });
    selectDistritos.append(optionDefault);
    $.each(distritos, function (index, distrito) {
        var option = $('<option>', {
            value: distrito.Id,
            text: distrito.Nombre
        });
        selectDistritos.append(option);
    });

}
function cargarUbicacionCCGDA2(idDistrito) {
    $.ajax({
        type: 'POST',
        url: controladorFichas + 'UbicacionByDistro',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: idDistrito }),
        success: function (ubicacion) {
            $('#departamentosSelectA1').val(ubicacion.DepartamentoId);

            cargarProvinciaCCGDA2(ubicacion.DepartamentoId, function () {
                $('#provinciasSelectA1').val(ubicacion.ProvinciaId);

                cargarDistritoCCGDA2(ubicacion.ProvinciaId, function () {
                    $('#distritosSelectA1').val(ubicacion.DistritoId);

                });
            });
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

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

