var controlador = siteRoot + 'campanias/plantransmision/';
var controladorFichas = siteRoot + 'campanias/fichasproyecto/';
var catPropietario = 2;
var catConcesionTemporal = 3;
var catConcesionActual = 4;
var catRequisitoCentralHidro = 7;
var catRequisitoCentralTermo = 7;
var horizonteInicio;
var horizonteFin;
var anioPeriodo;
var selectPeriodo;
var periodosLista = [];
var listaProyectosTipo = [];
let isLoading = false;
var layaoutView = '';
var formularioSeleccionado = null;
var cambiosRealizados = false;

$(function () {

    $('#btnGrabar').on('click', function () {
        grabar();
    });
  
    $('#contenido-fichas').hide();
    $('#areaDemandaTitulos').hide();
    $('#txtAreaDemanda').hide();
    cargarDatos().then(() => {
        if (modoModel != null && (modoModel === 'editar' || modoModel === 'consultar')) {
            getProyecto(proyectoModel);
            
        } else {
            $("#txtPoyCodi").val("0");
            if (codPeriodo != 0)  {
                $("#periodoProyectoSelect").val(codPeriodo);
                const selectElement = document.getElementById('periodoProyectoSelect');
                obtenerAnioPeriodo(selectElement);
            }
            if (codEmpresa != null) $("#empresaProyectoSelect").val(codEmpresa);
            
        }
        var planTransion = $("#txtCodPlanTransmision").val();
        if (planTransion != 0 && modoModel != null && modoModel === 'agregar') {
            obtenerEmpresa(planTransion);
            
        }
    });
 
});
function stopLoading() {
    if (isLoading) {
        $('#loadingProyecto').bPopup().close();
        isLoading = false;
    }
}

function cargarDatos() {
    return Promise.all([cargarEmpresaProyecto(), cargarPeriodoProyecto()])
        .then(cargarProyecto);
    
}

function grabarFicha(fichaSeleccionada) {
    if (modoModel == "consultar") {
        return;
    }
    if (fichaSeleccionada == "HFichaD") {
        if (entre) {
            guardarDetalleTabla();
        }
        grabarFichaD();
    } else if (fichaSeleccionada == "HFichaB") {
        grabarFichaB();
    } else if (fichaSeleccionada == "HFichaC") {
        grabarFichaC();
    } else if (fichaSeleccionada == "HFichaA") {
        grabarFichaA();
    } else if (fichaSeleccionada == "DGPFormatoD1A") {
        grabarDForm();
    } else if (fichaSeleccionada == "DGPFormatoD1B") {
        grabarDFormatoB();
    } else if (fichaSeleccionada == "DGPFormatoD1C") {
        grabarDFormatoC();
    } else if (fichaSeleccionada == "DGPFormatoD1D") {
        grabarFormatoD()
    } else if (fichaSeleccionada == "TFichaA") {
        grabarFichaCCTTA();
    } else if (fichaSeleccionada == "TFichaB") {
        grabarFichaCCTTB();
    } else if (fichaSeleccionada == "TFichaC") {
        grabarFichaCCTTC();
    } else if (fichaSeleccionada == "GLFichaA") {
        grabarFichaLineaA();
    } else if (fichaSeleccionada == "GLFichaB") {
        grabarFichaB();
    } else if (fichaSeleccionada == "GSFichaA") {
        grabarFicha1()
    } else if (fichaSeleccionada == "BFichaA") {
        grabarFichaA();
    } else if (fichaSeleccionada == "BFichaB") {
        grabarBioFichaB();
    } else if (fichaSeleccionada == "BFichaC") {
        grabarBioHojaC();    
    } else if (fichaSeleccionada == "FichaT2A") {
        grabarFichaT2A();
    } else if (fichaSeleccionada == "G4FichaA") {
        grabarFichaAG4();
    } else if (fichaSeleccionada == "G4FichaB") {
        grabarFichaBG4();
    } else if (fichaSeleccionada == "G4FichaC") {
        grabarFichaCG4();
    } else if (fichaSeleccionada == "TLFichaA") {
        grabart1FichaLineaA();
    } else if (fichaSeleccionada == "FichaT3A") {
        grabarCroFicha1();
    } else if(fichaSeleccionada == "GrabarEolHojaA"){
        grabarEolHojaA();
    } else if(fichaSeleccionada == "GrabarEolHojaB"){
        btnGrabarEolicaHojaB();
    } else if(fichaSeleccionada == "GrabarEolHojaC"){
        grabarEolHojaC();
    } else if(fichaSeleccionada == "CCGDA"){
        grabarCCGDA();
    } else if(fichaSeleccionada == "CCGDB"){
        grabarCCGDB();
    } else if(fichaSeleccionada == "CCGDC"){
        grabarCCGDC();
    } else if(fichaSeleccionada == "CCGDD"){
        grabarCCGDD();
    } else if(fichaSeleccionada == "CCGDE"){
        grabarCCGDE();
    } else if(fichaSeleccionada == "CCGDF"){
        grabarCCGDF();
    } else if (fichaSeleccionada == "H2VA") {
        grabarCuestionarioH2VA();
    } else if (fichaSeleccionada == "H2VB") {
        grabarCuestionarioH2VB();
    } else if (fichaSeleccionada == "H2VC") {
        grabarCH2VC();
    } else if (fichaSeleccionada == "H2VG") {
        grabarCH2VG();
    } else if (fichaSeleccionada == "H2VE") {
        grabarCH2VE();
    } else if (fichaSeleccionada == "H2VF") {
        btnGrabarCH2VF();
    } else if (fichaSeleccionada == "HVAD") {
        grabarCuestionarioH2VF();
    } else if (fichaSeleccionada == "fprm1") {
        grabarPrm1();
    } else if (fichaSeleccionada == "fprm2") {
        grabarPrm2();
    } else if (fichaSeleccionada == "fred1") {
        grabarRed1();
    } else if (fichaSeleccionada == "fred2") {
        grabarRed2();
    } else if (fichaSeleccionada == "fred3") {
        grabarRed3();
    } else if (fichaSeleccionada == "fred4") {
        grabarRed4();
    } else if (fichaSeleccionada == "fred5") {
        grabarRed5();
    } else if (fichaSeleccionada == "f104") {
        grabarItcdf104();
    } else if (fichaSeleccionada == "f108") {
        grabarItcdf108();
    } else if (fichaSeleccionada == "fp101") {
        grabarItcdf011();
    } else if (fichaSeleccionada == "fp102") {
        grabarItcdfp012();
    } else if (fichaSeleccionada == "fp103") {
        grabarItcdf013();
    } else if (fichaSeleccionada == "f110") {
        grabarItcdf110();
    } else if (fichaSeleccionada == "f116") {
        grabarItcdf116();
    } else if (fichaSeleccionada == "f121") {
        grabarItcdf121();
    } else if (fichaSeleccionada == "f123") {
        grabarItcdf123();
    } else if (fichaSeleccionada == "fe01") {
        guardarComentario();
    }

    cambiosRealizados = false;
    popupClose('popupGuardarFicha');
}

function popupClose(id) {
    $(`#${id}`).bPopup().close();
}

function popupCloseGuardado(id) {
    $(`#${id}`).bPopup().close();
    popupGuardadoVisible = false;
}

function cargarEmpresaProyecto() {
    return new Promise((resolve, reject) => {
        $.ajax({
            type: 'POST',
            url: controlador + 'ListarEmpresas',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(''),
            success: function (eData) {
                cargarListaEmpresasProyecto(eData);
                resolve();
            },
            error: function (err) {
                alert("Ha ocurrido un error");
                reject(err);
            }
        });
    });
}

function cargarListaEmpresasProyecto(empresas) {
    var selectEmpresa = $('#empresaProyectoSelect');
    $.each(empresas, function (index, empresa) {
        // Crear la opción
        var option = $('<option>', {
            value: empresa.Emprcodi,
            text: empresa.Emprnomb
        });

        // Agregar la opción al select
        selectEmpresa.append(option);
    });
}

function cargarPeriodoProyecto() {
    return new Promise((resolve, reject) => {
        $.ajax({
            type: 'POST',
            url: controlador + 'ListarPeriodos',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(''),
            success: function (eData) {
                periodosLista = eData;
                cargarListaPeriodoProyecto(eData);
                resolve();
            },
            error: function (err) {
                alert("Ha ocurrido un error");
                reject(err);
            }
        });
    });
}

function obtenerAnioPeriodo(periodoSelect) {
    var idPeriInt = parseInt(periodoSelect.value, 10);
    var periodoObj = periodosLista.filter(function (a) {
        return a.PeriCodigo === idPeriInt;
    });

    if (periodoObj.length>0) {
        var date = new Date(parseInt(periodoObj[0].PeriFechaInicio.replace("/Date(", "").replace(")/", ""), 10));
        anioPeriodo = date.getFullYear();
        horizonteInicio = date.getFullYear() - periodoObj[0].PeriHorizonteAtras;
        horizonteFin = date.getFullYear() + periodoObj[0].PeriHorizonteAdelante;
        var anio = date.getFullYear();
        $('#txtPoyAnio').val(anio);
        //cargarCatalogoAnio();
        //cargarCatalogoTabla(catRequisitoCentralHidro, "#tablaRequisitoCentralHidro");
    }

}

function obtenerAnioPeriodoEditar(periodoSelect) {
    var idPeriInt = parseInt(periodoSelect, 10);
    var periodoObj = periodosLista.filter(function (a) {
        return a.PeriCodigo === idPeriInt;
    });

    if (periodoObj.length > 0) {
        var date = new Date(parseInt(periodoObj[0].PeriFechaInicio.replace("/Date(", "").replace(")/", ""), 10));
        anioPeriodo = date.getFullYear();
        horizonteInicio = date.getFullYear() - periodoObj[0].PeriHorizonteAtras;
        horizonteFin = date.getFullYear() + periodoObj[0].PeriHorizonteAdelante;

        var anio = date.getFullYear();
        $('#txtPoyAnio').val(anio);
        //cargarCatalogoAnio();
        //cargarCatalogoTabla(catRequisitoCentralHidro, "#tablaRequisitoCentralHidro");
    }

}

function cargarListaPeriodoProyecto(periodos) {
    var selectPeriodo = $('#periodoProyectoSelect');
    $.each(periodos, function (index, periodo) {
        // Crear la opción
        var option = $('<option>', {
            value: periodo.PeriCodigo,
            text: periodo.PeriNombre
        });

        // Agregar la opción al select
        selectPeriodo.append(option);
    });
}

getCabeceraProyecto = function () {
    var param = {};
    param.EmpresaCodi = $("#empresaProyectoSelect").val();
    param.EmpresaNom = $("#empresaProyectoSelect option:selected").text();
    param.Pericodi = $("#periodoProyectoSelect").val();
    param.Proynombre = $("#txtNombreProyecto").val();
    param.Proydescripcion = $("#txtdetalleProyecto").val();
    param.Tipocodi = $("#proyectoSelect").val();
    param.Tipoficodi = $("#subtipoProyectoSelect").val();
    param.Areademanda = $("#txtAreaDemanda").val();
    var checkbox = document.getElementById("checkConfidencial");
    var esConfidencial = checkbox.checked ? "S" : "N";
    param.Proyconfidencial = esConfidencial;
    return param;

    
}

function grabar() {
    modoModel = 'editar';
    var validacion = validarCebecera();
    if (validacion == "") {
        var param = {};
        param.TransmisionProyectoDTO = getCabeceraProyecto();
        param.CodPlanTransmision = $("#txtCodPlanTransmision").val();
        var idProyecto = $("#txtPoyCodi").val();
        if (idProyecto == "0") {
            $.ajax({
                type: 'POST',
                url: controlador + 'GrabarCabecera',
                data: param,
                
                success: function (result) {
                    if (result.success) {
                        hojasRegistrar = result.hojas;
                        mostrarMensaje('mensajeProyecto', 'exito', 'Los datos se grabaron correctamente.');
                        $("#txtPoyCodi").val(result.proycodi);
                        $('#contenido-fichas').show();
                        $("#txtCodPlanTransmision").val(result.codPlanTransmision)
                        deshabilitarInput();
                        ObtenerListadoProyecto(result.codPlanTransmision);
                        $("#empresaSelect").val(param.TransmisionProyectoDTO.EmpresaCodi).prop("disabled", true);
                        $("#periodoSelect").val(param.TransmisionProyectoDTO.Pericodi).prop("disabled", true);
                        cargarFichasAfterSave();
                    }
                    else {
                        $('#contenido-fichas').hide();
                        $("#txtPoyCodi").val("0");
                        mostrarMensaje('mensajeProyecto', 'error', 'Se ha producido un error.');
                    }
                },
                error: function () {
                    mostrarMensaje('mensajeProyecto', 'error', 'Se ha producido un error.');
                }
            });
        }

    } else {
        mostrarMensaje('mensajeProyecto', 'alert', validacion);
    }
}

function cargarCatalogo(id, selectHtml) {
    $.ajax({
        type: 'POST',
        url: controlador + 'ListarCatalogo',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: id }),
        success: function (eData) {
            cargarListaParametros(eData, selectHtml);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}
function limpiarCombos(selectores) {
    $(selectores).each(function () {
        $(this).empty().append('<option value="">Seleccione</option>');
    });
}
function limpiarCombos2(selectores) {
    $(selectores).each(function () {
        $(this).empty().append('<option value="">Seleccione</option><option value="0">Otro</option>');
    });
}


function esperarCatalogo(id, selectHtml) {
    return new Promise((resolve) => {
        $.ajax({
            type: 'POST',
            url: controlador + 'ListarCatalogo',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: id }),
            success: function (eData) {
                cargarListaParametros(eData, selectHtml); // Llenar el select con los datos
                console.log(`Catálogo cargado: ${selectHtml}`);
                resolve(); // Indicar que el catálogo ya respondió
            },
            error: function (err) {
               // console.warn(` Error al cargar catálogo: ${selectHtml}. Se considera válido.`);
                resolve(); // Asegurar que se resuelva incluso con error
            }
        });
    });
}

function esperarCatalogoSubestacion(selectHtml, optionOtro) {
    return new Promise((resolve) => {
        $.ajax({
            type: 'POST',
            url: controlador + 'ListarSubestacion',
            contentType: "application/json; charset=utf-8",
            success: function (eData) {
                cargarListaParamSubestacion(eData, selectHtml, optionOtro);
                console.log(`Catálogo de subestaciones cargado: ${selectHtml}`);
                resolve(); // Se resuelve la promesa cuando la carga finaliza
            },
            error: function (err) {
               // console.warn(` Error al cargar el catálogo de subestaciones (${selectHtml}). Se considera válido.`);
                resolve(); // También resolvemos la promesa aunque haya error para evitar bloqueos
            }
        });
    });
}

function cargarCatalogoSubestacion(selectHtml, optionOtro) {
    $.ajax({
        type: 'POST',
        url: controlador + 'ListarSubestacion',
        contentType: "application/json; charset=utf-8",
        success: function (eData) {
            cargarListaParamSubestacion(eData, selectHtml, optionOtro);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarListaParametros(listaValores, selectHtml) {
    var selectData = $(selectHtml);
    $.each(listaValores, function (index, catalogo) {
        // Crear la opción
        var option = $('<option>', {
            value: catalogo.Valor,
            text: catalogo.DescortaDatacat
        });

        // Agregar la opción al select
        selectData.append(option);
    });

}

function cargarListaParamSubestacion(listaValores, selectHtml, optionOtro) {
    var selectData = $(selectHtml);
    $.each(listaValores, function (index, catalogo) {
        // Crear la opción
        var option = $('<option>', {
            value: catalogo.Equicodi,
            text: catalogo.Equinomb
        });

        // Agregar la opción al select
        selectData.append(option);
    });
    if(optionOtro){
        var option = $('<option>', {
            value: "otro",
            text: "Otros"
        });
        selectData.append(option);
    }

}

function cargarProyecto() {
    return new Promise((resolve, reject) => {
        $.ajax({
            type: 'POST',
            url: controlador + 'ListarProyectos',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(''),
            success: function (eData) {
                cargarListaProyecto(eData);
                listaProyectosTipo = eData;
                resolve();
            },
            error: function (err) {
                alert("Ha ocurrido un error");
                reject(err);
            }
        });
    });
}

function cargarListaProyecto(proyectos) {
    var selectProyecto = $('#proyectoSelect');
    $.each(proyectos, function (index, proyecto) {
        // Crear la opción
        var option = $('<option>', {
            value: proyecto.TipoCodigo,
            text: proyecto.TipoNombre
        });

        // Agregar la opción al select
        selectProyecto.append(option);
    });
    
}

function cargarListaSubTipoProyecto(proyectos) {
    var selectProyecto = $('#subtipoProyectoSelect');
    var option1 = $('<option>', {
        value: "",
        text: "Seleccion un Sub Tipo"
    });
    selectProyecto.append(option1);
    $.each(proyectos, function (index, proyecto) {
        if (proyecto.SubTipoCodigo == 1) {
            var option = $('<option>', {
                value: proyecto.TipoFiCodigo,
                text: proyecto.TipoFiNombre
            });

            selectProyecto.append(option);
        }
    });
    
}

function cargarTipoFichaProyecto(obProyecto) {

    var tipoProyectoEncontrado = listaProyectosTipo.find(a => a.TipoCodigo == obProyecto.value);
    var ulProyecto = $('#subtipoProyectoSelect');
    ulProyecto.empty();
    var ulProyectoTitu = $('#subtipoTitulos');
    ulProyecto.hide();
    //area de demanda
    var ulArea = $('#txtAreaDemanda');
    ulArea.empty();
    var ulAreaTitu = $('#areaDemandaTitulos');
    ulArea.hide();
    ulAreaTitu.hide();
    if (obProyecto.value == 1) {
        ulProyecto.show();
        ulProyectoTitu.show();
        if (tipoProyectoEncontrado) {
            var listaFiltrada = tipoProyectoEncontrado.ListaTipoFichaProyecto;
            cargarListaSubTipoProyecto(listaFiltrada);
            
        }
    } else if (obProyecto.value == 3){
        ulArea.show();
        ulAreaTitu.show();
        ulProyecto.hide();
        ulProyectoTitu.hide();
        //if (tipoProyectoEncontrado) {
        //    var listaFiltrada = tipoProyectoEncontrado.ListaTipoFichaProyecto;
        //    cargarListaTipoProyecto(listaFiltrada);
            
        //}
    } else {
        ulProyecto.hide();
        ulProyectoTitu.hide();
        //if (tipoProyectoEncontrado) {
        //    var listaFiltrada = tipoProyectoEncontrado.ListaTipoFichaProyecto;
        //    cargarListaTipoProyecto(listaFiltrada);
            
        //}
    }
    
}

function cargarFichasAfterSave() {
    var obProyecto = $("#proyectoSelect").val();
    var tipoProyectoEncontrado = listaProyectosTipo.find(a => a.TipoCodigo == obProyecto);
    if (obProyecto != 1) {
        if (tipoProyectoEncontrado) {
            var listaFiltrada = tipoProyectoEncontrado.ListaTipoFichaProyecto;
            cargarListaTipoProyecto(listaFiltrada);

        }
    } else {
        var subtipoProyectos = document.getElementById("subtipoProyectoSelect");
        cargarListaxSubTipoProyecto(subtipoProyectos);
    }
}


function cargarTipoFichaProyectoEditar(idProyecto) {
    
    var ulProyecto = $('#subtipoProyectoSelect');
    var ulProyectoTitu = $('#subtipoTitulos');
    var ulArea = $('#txtAreaDemanda');
    var ulAreaTitu = $('#areaDemandaTitulos');
    ulProyecto.hide();
    ulProyectoTitu.hide();
    if(idProyecto == 3){
        ulArea.show();
        ulAreaTitu.show();
    } else {
        ulArea.hide();
        ulAreaTitu.hide();
    }
    var tipoProyectoEncontrado = listaProyectosTipo.find(a => a.TipoCodigo == idProyecto);
    if (tipoProyectoEncontrado) {
        var listaFiltrada = tipoProyectoEncontrado.ListaTipoFichaProyecto;
        cargarListaTipoProyecto(listaFiltrada);
    }
    
}

function cargarTipoFichaProyectoEditarGeneracion(idProyecto, idsubtipo) {
    var ulProyecto = $('#subtipoProyectoSelect');
    ulProyecto.empty();
    var tipoProyectoEncontrado = listaProyectosTipo.find(a => a.TipoCodigo == idProyecto);
    if (tipoProyectoEncontrado) {
            var listaFiltrada = tipoProyectoEncontrado.ListaTipoFichaProyecto;
        cargarListaSubTipoProyecto(listaFiltrada);
        var subtipoProyectos =  document.getElementById("subtipoProyectoSelect");
        subtipoProyectos.value = idsubtipo;
        cargarListaxSubTipoProyecto(subtipoProyectos);
    }
    //$('#etabsTipoProyecto li:first-child a').click();
    

}

function cargarListaTipoProyecto(tipoProyectos) {
    
    var ulProyecto = $('#etabsTipoProyecto');

    // Limpiar la lista
    ulProyecto.empty();

    // Agregar los nuevos elementos <li>
    $.each(tipoProyectos, function (index, dapro) {
        // Crear el elemento <li>
        var li = $('<li>', {
            class: 'tab'  // Usa la clase 'tab' según tu estilo
        });

        // Crear el enlace <a>
        var a = $('<a>', {
            click: function () {
                if (cambiosRealizados) {
                    popupGuardadoAutomatico();
                      cambiosRealizados = false;
                    return;
                }
                ObtenerContenidoFicha(dapro.ContrHtml);
                highlightTab(this, 'tab-container-1');
            }
        });

        // Crear la etiqueta <label> si es necesario
        var label = $('<label>', {
            text: dapro.TipoFiNombre,
            id: 'tab' + (index + 1)
        });

        // Agregar la etiqueta al enlace
        a.append(label);

        // Agregar el enlace al elemento <li>
        li.append(a);

        // Agregar el elemento <li> a la lista <ul>
        ulProyecto.append(li);
    });
    //$('#etabsTipoProyecto li:first-child a').click();
    var firstTab = $('#etabsTipoProyecto li:first-child a');
    if (firstTab.length > 0) {
        firstTab.click();
    } else {
        console.warn('No se encontraron pestañas para activar.');
    }

    
}

function cargarListaxSubTipoProyecto(tipoProyectos) {
    
    var ulProyecto = $('#etabsTipoProyecto');
    var ulProyecto1 = $('#containerFicha');
    var tipoProyectoEncontrado = listaProyectosTipo.find(a => a.TipoCodigo == 1);
    var listaFiltrada = tipoProyectoEncontrado.ListaTipoFichaProyecto;
    var objetosFiltrado = listaFiltrada.filter(b => b.TipoFiCodigo == tipoProyectos.value || b.SubTipoCodigo == 0);
    //var tipoProyectoEncontrado = listaProyectosTipo.find(a => a.TipoCodigo == obProyecto.value);

    //// Limpiar la lista
    ulProyecto.empty();
    ulProyecto1.empty();

    //// Agregar los nuevos elementos <li>
    $.each(objetosFiltrado, function (index, dapro) {
        // Crear el elemento <li>
        var li = $('<li>', {
            class: 'tab'
        });

        // Crear el enlace <a>
        var a = $('<a>', {
            click: function () {
                if (cambiosRealizados) {
                    popupGuardadoAutomatico();
                    cambiosRealizados = false;
                    return;
                }
                ObtenerContenidoFicha(dapro.ContrHtml);
                highlightTab(this, 'tab-container-1');
            }
        });

        // Crear la etiqueta <label> si es necesario
        var label = $('<label>', {
            text: dapro.TipoFiNombre,
            id: 'tab' + (1)
        });

        // Agregar la etiqueta al enlace
        a.append(label);

        // Agregar el enlace al elemento <li>
        li.append(a);

        // Agregar el elemento <li> a la lista <ul>
        ulProyecto.append(li);
    });
    var firstTab = $('#etabsTipoProyecto li:first-child a');
    if (firstTab.length > 0) {
        firstTab.click();
    } else {
        console.warn('No se encontraron pestañas para activar.');
    }

    
}

function ObtenerContenidoFicha(tipoProy) {
      $("#containerFicha").html('');

    $.ajax({
        type: 'POST',
        url: controladorFichas + tipoProy,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(''),
        success: function (eData) {
            $('#containerFicha').html(eData);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function grabarFichaASubestacion() {
    var param = {};
    param.TransmisionProyectoDTO = getCabeceraProyecto();
    var idProyecto = $("#txtPoyCodi").val();
    if (idProyecto != "0") {
        param.ProyCodi = idProyecto;
        param.RegHojaASubestDTO = getDataHojaASubest();
        param.DetRegHojaASubestDTO = getDataDetHojaASubest();
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarFichaASubest',
            data: param,
            
            success: function (result) {
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
}

function getProyecto(param) {
    $("#txtPoyCodi").val(param.Proycodi);
    $("#empresaProyectoSelect").val(param.EmpresaCodi);
    $("#periodoProyectoSelect").val(param.Pericodi);
    $("#txtNombreProyecto").val(param.Proynombre);
    $("#txtdetalleProyecto").val(param.Proydescripcion);
    $("#proyectoSelect").val(param.Tipocodi);
    $("#txtAreaDemanda").val(param.Areademanda);
    // Chequear o deschequear el checkbox basado en el valor "S" o "N" y deshabilitarlo
    if (param.Proyconfidencial === "S") {
        $("#checkConfidencial").prop("checked", true);
    } else {
        $("#checkConfidencial").prop("checked", false);
    }
    $('#contenido-fichas').show(); // Mostrar el div
    if (param.Tipocodi != 1) {
        cargarTipoFichaProyectoEditar(param.Tipocodi);
    } else {
      cargarTipoFichaProyectoEditarGeneracion(param.Tipocodi, param.Tipoficodi);
    }
    deshabilitarInput();
    obtenerAnioPeriodoEditar($("#periodoProyectoSelect").val());
}

function deshabilitarInput() {
    $("#empresaProyectoSelect").prop("disabled", true);
    $("#periodoProyectoSelect").prop("disabled", true);
    $("#txtNombreProyecto").prop("readonly", true);
    $("#txtdetalleProyecto").prop("readonly", true);
    $("#proyectoSelect").prop("disabled", true);
    $("#subtipoProyectoSelect").prop("disabled", true);
    $('#btnGrabar').hide();
    $("#checkConfidencial").prop("disabled", true);
    $("#txtAreaDemanda").prop("readonly", true);
}

function validarCebecera() {
    var mensaje = "<ul>";
    var flag = true;

    if ($("#empresaProyectoSelect").val().length < 1) {
        mensaje = mensaje + "<li>Seleccione una empresa</li>";
        flag = false;
    }

    if ($("#periodoProyectoSelect").val().length < 1) {
        mensaje = mensaje + "<li>Seleccione un periodo</li>";
        flag = false;
    }

    if ($("#txtdetalleProyecto").val().length < 1) {
        mensaje = mensaje + "<li>Ingrese un detalle</li>";
        flag = false;
    }

    if ($("#proyectoSelect").val().length < 1) {
        mensaje = mensaje + "<li>Seleccione un proyecto</li>";
        flag = false;
    }

    if ($("#txtNombreProyecto").val().length < 1) {
        mensaje = mensaje + "<li>Ingrese un nombre</li>";
        flag = false;
    }

    if ($("#subtipoProyectoSelect").val()  != null && $("#subtipoProyectoSelect").val().length < 1 && $("#proyectoSelect").val() == 1 && $("#subtipoProyectoSelect").val().length < 1 ) {
        mensaje = mensaje + "<li>Seleccione un sub tipo de proyecto</li>";
        flag = false;
    }

    if (($("#txtAreaDemanda").val().length < 1 || $("#txtAreaDemanda").val() == 0) && $("#proyectoSelect").val() == 3) {
        mensaje = mensaje + "<li>Ingrese un area de demanda</li>";
        flag = false;
    }

    mensaje = mensaje + "</ul>";

    if (flag) mensaje = "";

    return mensaje;
}

mostrarMensaje = function (id, tipo, mensaje, mostrarX = false) {
    const elemento = $('#' + id);
    elemento.removeClass();
    elemento.addClass('action-' + tipo);

    // Agregamos el mensaje con la opción de cerrar si mostrarX es true
    let contenido = mensaje;
    if (mostrarX) {
        contenido += ' <span class="cerrar-mensaje">X</span>';
    }

    elemento.html(contenido);

    // Si mostrarX es true, añadimos el evento de clic para eliminar completamente el elemento del DOM
    if (mostrarX) {
        elemento.find('.cerrar-mensaje').on('click', function () {
            elemento.removeClass().empty();
        });
    }

    scrollTopModal();
}

limpiarMensaje = function (id) {
    $('#' + id).removeClass(); // Elimina todas las clases
    $('#' + id).html(''); // Limpia el contenido del mensaje
}


function highlightTab(element, containerId) {
    var container = document.getElementById(containerId);
    var tabs = container.querySelectorAll('li a');

    // Remueve la clase 'active' de todas las tabs en el contenedor
    tabs.forEach(function (tab) {
        tab.classList.remove('active');
        tab.parentElement.classList.remove('active'); // Remueve también de <li>
    });

    // Agrega la clase 'active' a la tab clickeada
    element.classList.add('active');
    element.parentElement.classList.add('active'); // Agrega también a <li>
}

function obtenerEmpresa(id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerEmpresaPlan',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: id }),
        success: function (eData) {
            $("#empresaProyectoSelect").val(eData.Codempresa).prop("disabled", true);
            $("#periodoProyectoSelect").val(eData.Pericodi).prop("disabled", true);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}
let popupGuardadoVisible = false;
function popupGuardadoAutomatico() {
    if (modoModel == "consultar") {
        return;
    }
    const popups = $('[id="popupGuardarFicha"]'); 
    if (popups.length > 1) {
        popups.slice(1).remove(); 
    }

    if (popupGuardadoVisible) return; // Salir si el popup ya está visible

    popupGuardadoVisible = true; // Marcar que el popup está visible

    $('#popupGuardarFicha').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown',
        onClose: function () {
            popupGuardadoVisible = false; // Restablecer estado cuando se cierre el popup
        }
    });
}

 
function cargarArchivosRegistrados(seccion, idTabla) {

    var param = {};
    var idProyecto = $("#txtPoyCodi").val();
    param.ProyCodi = idProyecto;
    param.Secccodi = seccion;
    $.ajax({
        type: 'POST',
        url: controladorFichas + 'ListarArchivosFichas',
        data: param,
        
        success: function (result) {
            var tabla = $(idTabla);
            var tablaBody = $(idTabla + ' tbody');
            tablaBody.empty();
            var archivosGuardados = result.responseResult;
            for (var i = 0; i < archivosGuardados.length; i++) {
                var nuevaFila = '<tr id="fila' + archivosGuardados[i].ArchCodi + '">' +
                    '<td><a href="#" onclick="descargarFileCampania(\'' + archivosGuardados[i].ArchNombreGenerado + '\')">' + archivosGuardados[i].ArchNombre + '</a></td>' +
                    '<td> <a href="#"><img src="' + siteRoot + 'Content/Images/eliminar.png" class="btnEliminar" onclick="eliminarFile(' + archivosGuardados[i].ArchCodi + ')" title="Eliminar archivo"/></a></td>' +
                    '</tr>';
                tabla.append(nuevaFila);
            }


        },
        error: function () {
            mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error.');
        }
    });
}



function cargarArchivosRegistradosDesc(seccion, idTabla) {
    
    var param = {};
    var idProyecto = $("#txtPoyCodi").val();
    param.ProyCodi = idProyecto;
    param.Secccodi = seccion;
    $.ajax({
        type: 'POST',
        url: controladorFichas + 'ListarArchivosFichas',
        data: param,
        
        success: function (result) {
            var tabla = $(idTabla);
            var tablaBody = $(idTabla + ' tbody');
            tablaBody.empty();
            var archivosGuardados = result.responseResult;
            for (var i = 0; i < archivosGuardados.length; i++) {


                if (modoModel == "consultar") {
                var nuevaFila = '<tr id="fila' + archivosGuardados[i].ArchCodi + '">' +
                    '<td>' + (archivosGuardados[i].Descripcion !== null ? archivosGuardados[i].Descripcion : '')  + '</td>' +
                    '<td><a href="#" onclick="descargarFileCampania(\'' + archivosGuardados[i].ArchNombreGenerado + '\')">' + archivosGuardados[i].ArchNombre + '</td>' +
                    '<td><img src="' + siteRoot + 'Content/Images/eliminar.png" style="width: auto;height: 18px;"  /></td>' +
                    '</tr>';
              } else {
                    var nuevaFila = '<tr id="fila' + archivosGuardados[i].ArchCodi + '">' +
                        '<td>' + (archivosGuardados[i].Descripcion !== null ? archivosGuardados[i].Descripcion : '') + '</td>' +
                        '<td><a href="#" onclick="descargarFileCampania(\'' + archivosGuardados[i].ArchNombreGenerado + '\')">' + archivosGuardados[i].ArchNombre + '</td>' +
                        '<td> <a href="#"><img src="' + siteRoot + 'Content/Images/eliminar.png" class="btnEliminar" onclick="eliminarFile(' + archivosGuardados[i].ArchCodi + ')" title="Eliminar archivo"/></a></td>' +
                        '</tr>';
                }
                tabla.append(nuevaFila);
            }


        },
        error: function () {
            mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error.');
        }
    });
}

function desactivarCamposFormulario(formu) {
    var formulario = document.getElementById(formu);
    var elementos = formulario.querySelectorAll('input, button, textarea, select, a');

    elementos.forEach(function (elemento) {
        if (elemento.tagName === 'INPUT') {
            if (['text', 'email', 'password', 'number'].includes(elemento.type)) {
                elemento.readOnly = true;
            } else {
                if (!elemento.classList.contains('btnActive')) {
                    elemento.disabled = true;
                }
            }
            // Deshabilitar Zebra_DatePicker
            if (elemento.classList.contains('Zebra_DatePicker_Icon') || elemento.parentElement.querySelector('.Zebra_DatePicker_Icon')) {
                elemento.disabled = true;
                elemento.nextElementSibling?.classList.add('Zebra_DatePicker_Icon_Disabled');
            }
        } else if (['TEXTAREA', 'SELECT'].includes(elemento.tagName)) {
            elemento.disabled = true;
        } else if (elemento.tagName === 'BUTTON') {
            if (!elemento.classList.contains('btnActive')) {
                elemento.disabled = true;
            }
        } else if (elemento.tagName === 'A') {
            if (!elemento.classList.contains('btnActive')) {
                elemento.addEventListener('click', function (event) {
                    event.preventDefault();
                });
                elemento.style.pointerEvents = 'none'; // Desactiva el click visualmente
            }
        }
    });

    // Desactivar tablas por clases específicas
    var tablas = formulario.querySelectorAll('.wtHider');
    tablas.forEach(function (tabla) {
        // Opcional: bloquear eventos y aplicar estilo deshabilitado
        tabla.style.pointerEvents = 'none';
        tabla.style.opacity = '0.9'; // Para dar un efecto visual de desactivación
        tabla.style.userSelect = 'none'; // Desactiva la selección de texto en la tabla
    });
}

// Función para aplicar bordes a todas las celdas en una hoja
function aplicarBordes(ws, maxWidth = 50) {
    const range = XLSX.utils.decode_range(ws["!ref"]);
    const columnWidths = new Array(range.e.c + 1).fill(10);

    for (let R = range.s.r; R <= range.e.r; ++R) {
        for (let C = range.s.c; C <= range.e.c; ++C) {
            const cellAddress = XLSX.utils.encode_cell({ r: R, c: C });

            if (!ws[cellAddress]) {
                ws[cellAddress] = { v: "" };
            }

            // Conservar estilos existentes (fondo gris, alineación, etc.)
            if (!ws[cellAddress].s) ws[cellAddress].s = {};

            // Agregar bordes sin sobrescribir otros estilos
            ws[cellAddress].s.border = {
                top: { style: "thin", color: { rgb: "000000" } },
                bottom: { style: "thin", color: { rgb: "000000" } },
                left: { style: "thin", color: { rgb: "000000" } },
                right: { style: "thin", color: { rgb: "000000" } }
            };

            // Asegurar alineación sin eliminar otros estilos
            ws[cellAddress].s.alignment = {
                horizontal: "left",
                vertical: "left",
                wrapText: true
            };

            // Ajuste de ancho de columna según el contenido
            const cellValue = ws[cellAddress].v ? ws[cellAddress].v.toString() : "";
            let calculatedWidth = cellValue.length + 2;
            columnWidths[C] = Math.min(Math.max(columnWidths[C], calculatedWidth), maxWidth);
        }
    }

    ws["!cols"] = columnWidths.map(width => ({ wch: width }));
}
function exportarTablaExceCheckBox(hotTable, fileName = "HojaC_Datos", sheetName = "HojaC") {
    var aniosCabecera = [];
    var headerCabe = [];
    var merges = []; // Almacenar las combinaciones de celdas

    aniosCabecera.push("");
    aniosCabecera.push("");
    aniosCabecera.push("");

    var inicioan = anioPeriodo;
    var finan = horizonteFin;
    var anioTotal = finan - inicioan + 1;
    var colIndex = 3; // Comienza después de las primeras 3 columnas

    for (var i = 0; i < anioTotal; i++) {
        aniosCabecera.push(inicioan + i + "\nTRIMESTRE"); // Solo una vez, ya que se combinará
        aniosCabecera.push("");
        aniosCabecera.push("");
        aniosCabecera.push("");

        // Agregar merge para cada año cubriendo sus 4 trimestres
        merges.push({
            s: { r: 0, c: colIndex-1 }, // Celda de inicio (fila 0, columna actual)
            e: { r: 0, c: colIndex-1 + 3 } // Celda final (misma fila, columna +3)
        });

        colIndex += 4;
    }

    headerCabe.push("");
    headerCabe.push("AÑO");
    headerCabe.push("AÑO " + (inicioan - 1) + " o antes");

    for (var i = 0; i < anioTotal; i++) {
        for (var j = 1; j <= 4; j++) {
            headerCabe.push(" " + j + " ");
        }
    }

    var data = hotTable.getData();
    var transformedData = data.map(row =>
        row.slice(1).map(cell =>
            (cell === true || cell === 'true') ? 1 :
                ((cell === false || cell === 'false') ? 0 : cell)
        )
    );

    var aniosCabeceraWithoutFirst = aniosCabecera.slice(1);
    var headerCabeWithoutFirst = headerCabe.slice(1);

    var dataWithHeaders = [
        aniosCabeceraWithoutFirst,
        headerCabeWithoutFirst
    ].concat(transformedData);

    var ws = XLSX.utils.aoa_to_sheet(dataWithHeaders);
    aplicarBordes(ws);

    // Aplicar las combinaciones de celdas para los años
    ws["!merges"] = merges;

    // Ajustar la altura de la celda para mostrar los títulos correctamente
    if (!ws["!rows"]) ws["!rows"] = [];
    ws["!rows"][0] = { hpx: 30 }; // Altura de la fila de los años
   // ws["!rows"][1] = { hpx: 20 }; // Altura de la fila de los trimestres

    // Aplicar centrado en toda la cabecera
    Object.keys(ws).forEach(cell => {
        if (ws[cell] && ws[cell].v && typeof ws[cell].v === "string") {
            if (!ws[cell].s) ws[cell].s = {};
            ws[cell].s.alignment = {
                wrapText: true, // Permitir ajuste de texto en las celdas
                horizontal: "center", // Centrar horizontalmente
                vertical: "center" // Centrar verticalmente
            };
        }
    });
    for (var i = 2; i < colIndex; i++) {
        ws["!cols"][i] = { wpx: 50 }; // Ajustar ancho de la columna desde la 2 en adelante
    }
    var wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, sheetName);

    XLSX.writeFile(wb, `${fileName}.xlsx`);
}

function importarExcelCheckBox(hotTable) {
    var fileInput = document.createElement("input");
    fileInput.type = "file";
    fileInput.accept = ".xlsx, .xls";
    fileInput.onchange = function (e) {
        var file = e.target.files[0];
        var reader = new FileReader();

        reader.onload = function (e) {
            var data = new Uint8Array(e.target.result);
            var workbook = XLSX.read(data, { type: "array" });

            // Leer la primera hoja del archivo Excel
            var firstSheet = workbook.Sheets[workbook.SheetNames[0]];

            // Convertir la hoja de Excel a un formato JSON
            var jsonData = XLSX.utils.sheet_to_json(firstSheet, { header: 1 });

            // Actualizar la tabla Handsontable con los datos importados
            updateTableFromExcelChechkbox(jsonData, hotTable);
            cambiosRealizados = true;
        };

        reader.readAsArrayBuffer(file);
    };

    fileInput.click();
}

function updateTableFromExcelChechkbox(jsonData, hotTable) {
    const currentData = hotTable.getData();
    const numColumns = currentData[0]?.length || 0;
    var headers = jsonData.slice(0, 2); // Las dos primeras filas son las cabeceras
    var data = jsonData.slice(2); // El resto son los datos
    // Convertir 1/0 a true/false en los datos importados
    var reorganizedData = data.map((row, index) => {
        var firstColumn = currentData[index]?.[0] || '';
        var secondColumn = currentData[index]?.[1] || '';
        return [firstColumn, secondColumn, ...row.slice(1)];
    });
    reorganizedData = reorganizedData.map(row => {
        if (row.length < numColumns) {
            return [...row, ...Array(numColumns - row.length).fill(null)];
        } else if (row.length > numColumns) {
            return row.slice(0, numColumns);
        }
        return row;
    });
    var transformedData = reorganizedData.map(row => {
        return row.map((cell, cellIndex) => {
            if (cellIndex === 0 || cellIndex === 1) {
                return cell;
            }
            return cell === 1 ? true : (cell === 0 ? false : false);
        });
    });
    // Cargar los datos en la tabla Handsontable
    hotTable.loadData(transformedData);
}

function convertirFechaMesAnio(fechaJSON) {
    let fecha = null;

    // Si fechaJSON es null o undefined, usar vacio
    if (!fechaJSON) {
        return ""; // vacio
    } else if (/\/Date\(-?\d+\)\//.test(fechaJSON)) {
        // Formato /Date(...)
        let timestamp = parseInt(fechaJSON.match(/\d+/)[0]);
        fecha = new Date(timestamp);
    } else if (/^\d{2}\/\d{2}\/\d{4} \d{2}:\d{2}:\d{2}$/.test(fechaJSON)) {
        // Formato DD/MM/YYYY HH:mm:ss
        let [dia, mes, anioHora] = fechaJSON.split('/');
        let [anio, hora] = anioHora.split(' ');
        let [hh, mm, ss] = hora.split(':');

        fecha = new Date(
            parseInt(anio),
            parseInt(mes) - 1,
            parseInt(dia),
            parseInt(hh),
            parseInt(mm),
            parseInt(ss)
        );
    } else if (/^\d{2}-\d{4}$/.test(fechaJSON)) {
        // Formato MM-YYYY
        let [mes, anio] = fechaJSON.split('-').map(Number);

        if (mes < 1 || mes > 12) {
            throw new Error("Mes no válido en el formato MM-YYYY");
        }
        fecha = new Date(anio, mes - 1, 1);
    } else {
        throw new Error("Formato de fecha no válido");
    }

    // Validar si la fecha es válida
    if (isNaN(fecha.getTime())) {
        throw new Error("Fecha no válida");
    }

    // Convertir la fecha a MM-YYYY
    let mes = (fecha.getMonth() + 1).toString().padStart(2, '0');
    let anio = fecha.getFullYear();
    let fechaFormateada = `${mes}/${anio}`;

    return fechaFormateada;
}

function convertirFecha(fechaJSON) {
    let fecha;

    if (!fechaJSON) {
        fecha = new Date(); 
    } else {
        let timestamp = parseInt(fechaJSON.match(/\d+/)?.[0]);
        if (isNaN(timestamp)) {
            throw new Error("Formato de fecha JSON no válido");
        }
        fecha = new Date(timestamp);
    }

    if (isNaN(fecha.getTime())) {
        throw new Error("Fecha no válida");
    }

    let dia = fecha.getDate().toString().padStart(2, '0');
    let mes = (fecha.getMonth() + 1).toString().padStart(2, '0');
    let anio = fecha.getFullYear();

    let fechaFormateada = `${dia}-${mes}-${anio}`;
    return fechaFormateada;
}

function toggleInputGlobal(selectId, inputId, valueToShow) {
    var selectElement = $("#" + selectId);
    var inputElement = $("#" + inputId);
    if (inputId != 'OtroSubEstacion') {
    var labelElement = $(".labelOtro"); // Seleccionar el label asociado al input
    }
    if (selectElement.val() === valueToShow) {
        inputElement.show();  // Mostrar input
        if (inputId != 'OtroSubEstacion') {
        labelElement.show();  // Mostrar label
        }
    } else {
            inputElement.hide();  // Ocultar input
            if (inputId != 'OtroSubEstacion') {
                labelElement.hide();  // Ocultar label
            }
        inputElement.val(""); // Limpiar el campo cuando se oculta
    }
}

function convertirFechaGlobal(jsonDate) {
    if (!jsonDate || typeof jsonDate !== "string" || !jsonDate.match(/\/Date\((\d+)\)\//)) {
        return null;
    }

    // Extrae el timestamp en milisegundos
    const timestamp = parseInt(jsonDate.match(/\/Date\((\d+)\)\//)[1], 10);

    // Verifica si el timestamp es un número válido
    if (isNaN(timestamp)) {
        return null;
    }

    // Convierte a objeto Date
    const date = new Date(timestamp);

    // Formatea la fecha como DD/MM/YYYY
    const dia = ("0" + date.getDate()).slice(-2);
    const mes = ("0" + (date.getMonth() + 1)).slice(-2);
    const anio = date.getFullYear();

    return `${dia}/${mes}/${anio}`;
}

function setFechaPickerGlobal(selector, valor, formatoPlaceholder) {
    $(selector).attr("placeholder", formatoPlaceholder); // Establece el placeholder siempre

    if (!valor || valor.trim() === "") {
        $(selector).val(""); // Borra el valor si está vacío
    } else {
        $(selector).val(valor); // Asigna el valor si existe
    }
}


function asignarSiExisteGlobal(selectId, valor) {
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

function obtenerFechaActualMesAnio() {
    let fecha = new Date();
    let mes = ("0" + (fecha.getMonth() + 1)).slice(-2);
    let anio = fecha.getFullYear();
    return `${mes}/${anio}`;
}

function obtenerFechaActual() {
    let fecha = new Date();
    let dia = ("0" + fecha.getDate()).slice(-2);
    let mes = ("0" + (fecha.getMonth() + 1)).slice(-2);
    let anio = fecha.getFullYear();
    return `${dia}/${mes}/${anio}`;
}

function descargarFileCampania(nombre) {
    location.href = controlador + 'DescargarArchivoCampania?nombre=' + nombre;
}

function scrollTopModal(){
    const elements = document.getElementsByClassName('jquery-modal');
    for (let i = 0; i < elements.length; i++) {
      elements[i].scrollTop = 0;
    }
}

function contarCaracteres(event) {
    const textarea = event.target;
    const maxLength = textarea.maxLength;
    const caracteresRestantes = maxLength - textarea.value.length;
    const contador = textarea.nextElementSibling; // El siguiente elemento es el contador
    contador.textContent = `Caracteres restantes: ${caracteresRestantes}`;
}

document.addEventListener("DOMContentLoaded", function() {
    const textareas = document.querySelectorAll(".contar-caracteres");
    textareas.forEach(textarea => {
        textarea.addEventListener("input", contarCaracteres);
        const maxLength = textarea.maxLength;
        const contador = textarea.nextElementSibling;
        contador.textContent = `Caracteres restantes: ${maxLength}`;
    });
});

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

function setupDropdownToggle(dropdownId, inputId) {
    var dropdown = document.getElementById(dropdownId);
    var input = document.getElementById(inputId);

    function updateInputVisibility() {
        if (dropdown.value == '9999' || dropdown.value == 'otro') {
            input.style.visibility = 'visible';
        } else {
            input.style.visibility = 'hidden'; 
            input.value = '';
        }
    }
    updateInputVisibility();

    dropdown.addEventListener('change', updateInputVisibility);
}

function cerrarModalValidarReg(){
    if (cambiosRealizados) {
        popupGuardadoAutomatico();
        cambiosRealizados = false;
        return;
    }
    //document.querySelector('.cancel-button').setAttribute('rel', 'modal:close');
    if (typeof $.modal !== 'undefined') {
        $.modal.close();
      } else {
        const modal = document.querySelector('.modal');
        if (modal) {
          modal.style.display = 'none';
        }
      }
}


function formatearDatosGlobal(data, configuracionColumnas) {
    // Si no hay datos, devolverlos sin cambios
    const tieneDatos = data.some(row => row.some(value => value !== ""));
    if (!tieneDatos) {
        console.log("Los datos están vacíos, no se aplica formateo.");
        return data;
    }

    // Procesar solo si hay datos
    data.forEach((row, rowIndex) => {
        row.forEach((value, colIndex) => {
            const config = configuracionColumnas[colIndex];
            if (config && value !== "" && value !== null && value !== undefined) {
                let stringValue = String(value);
                if (config.tipo === "decimal" || config.tipo === "decimaltruncal" || config.tipo === "entero") {
                    stringValue = stringValue.replace(/,/g, ""); // Eliminar comas solo para decimales
                }

                
                let parsedValue = parseFloat(stringValue);

                if (config.tipo === "texto") {
                    // Recortar el texto si excede el límite
                    if (stringValue.length > config.largo) {
                        row[colIndex] = stringValue.substring(0, config.largo);
                    }
                } else if (!isNaN(parsedValue)) {
                    if (config.tipo === "decimal") {
                        // Redondear a la cantidad de decimales permitidos
                        row[colIndex] = parsedValue.toFixed(config.decimales);
                    } else if (config.tipo === "decimaltruncal") {
                        // Truncar a la cantidad de decimales permitidos
                        const factor = Math.pow(10, config.decimales);
                        row[colIndex] = (Math.floor(parsedValue * factor) / factor).toFixed(config.decimales);
                    } else if (config.tipo === "entero") {
                        // Convertir a entero
                        row[colIndex] = Math.round(parsedValue).toString();
                    } else if (config.tipo === "especial") {
                        // Reemplazar valores no permitidos con un valor por defecto
                        const valoresPermitidos = [17, 18, 19];
                        row[colIndex] = valoresPermitidos.includes(parsedValue) ? parsedValue.toString() : "";
                    }
                }
            }
        });
    });

    return data; // Retorna los datos con el formato aplicado
}

function obtenerRutaProyecto() {
    var anioServidor = new Date().getFullYear();
    var periodoCod = $("#periodoProyectoSelect").val();
    var periodoNombre = $("#periodoProyectoSelect option:selected").text();
    var tipoProyectoCod = $("#proyectoSelect").val();
    var tipoProyectoNombre = $("#proyectoSelect option:selected").text();
    var empresaCod = $("#empresaProyectoSelect").val();
    var empresaNombre = $("#empresaProyectoSelect option:selected").text();
    var cod_envio = $("#txtPoyCodi").val();
    var version = $("#txtversion").val() || "0";


    var cod_Subtipoproy = $("#subtipoProyectoSelect").val();
    var nom_Subtipoproy = $("#subtipoProyectoSelect option:selected").text();

    // Verificar si nom_Subtipoproy contiene "Seleccion" o "Proyecto" o si cod_Subtipoproy es ""
    var usarRutaCorta = !cod_Subtipoproy ||
        nom_Subtipoproy.toLowerCase().includes("seleccion") ||
        nom_Subtipoproy.toLowerCase().includes("proyecto");

    return usarRutaCorta
        ? `${anioServidor}\\${periodoCod}-${periodoNombre}\\${tipoProyectoCod}-${tipoProyectoNombre}\\${empresaCod}-${empresaNombre}\\${cod_envio}\\${version}`
        : `${anioServidor}\\${periodoCod}-${periodoNombre}\\${tipoProyectoCod}-${tipoProyectoNombre}\\${cod_Subtipoproy}-${nom_Subtipoproy}\\${empresaCod}-${empresaNombre}\\${cod_envio}\\${version}`;
}

