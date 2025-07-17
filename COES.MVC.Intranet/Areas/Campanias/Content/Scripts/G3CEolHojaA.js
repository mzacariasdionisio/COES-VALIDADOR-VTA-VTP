
var catPropietario = 2;
var catConcesionTemporal = 3;
var catConcesionActual = 4;
var catSerieVelocidad = 9;
var catEstudioGeol = 10;
var catEstudioTopo = 11;
var catTipTurbina = 12;
var catTipParqueEol = 13;
var catTipGenerador = 14;
var catNomSubEsacionSein = 15;
var catPerfil = 16;
var catPrefactibilidad = 17;
var catFactibilidad = 18;
var catEstudDefinitivo = 19;
var catEia = 20;
var catBacterias = 24;

var tablaEolHoja = null;
var datosGrafico = null;

seccionUbicacionEOL = 12;
seccionUniEol = 13
seccionInfBasiEol = 15;

var departamentos = [];
var provincias = [];
var distritos = [];



$(function () {
  
    $('#cmbNomSubEsacionSein').change(mostrarCampoOtro);

    $('#curvaTurbinaCon').hide();
    //$('#txtFechaAdjudicacionTemporal').val(obtenerFechaActual());
    //$('#txtFechaAdjudicacionActual').val(obtenerFechaActual());
    //$('#txtFechaInicioContruccion').val(obtenerFechaActualMesAnio());
    //$('#txtFechaOperacionComercial').val(obtenerFechaActualMesAnio());

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
    $('#txtFechaInicioContruccion').Zebra_DatePicker({
        format: 'm/Y',
        onSelect: function () {
            cambiosRealizados = true; // Marcar que hubo un cambio
        }
    });
    $('#txtFechaOperacionComercial').Zebra_DatePicker({
        format: 'm/Y',
        onSelect: function () {
            cambiosRealizados = true; // Marcar que hubo un cambio
        }
    });

   
    cargarDepartamentos();
    $('#btnGrabarEolicaHojaA').on('click', function () {
        console.log("ejecucionGrabar")
        grabarEolHojaA();
    });
    $('#btnGrabarEolicaHojaAGrafico').on('click', function () {
        $('#dataFichaAEo').hide();
        $('#curvaTurbinaCon').show();
    });
    crearTablaTurbina();
    console.log("ejecucionGrabar");
    var ruta_interna = obtenerRutaProyecto();
    crearUploaderGeneral('btnSubirUbicacionEOL', '#tablaUbicacionEol', seccionUbicacionEOL, null, ruta_interna);
    crearUploaderGeneral('btnSubirInfBasEol', '#tablaInfBasEOL', seccionInfBasiEol, null, ruta_interna);
    crearUploaderGeneral('btnSubirUniEol', '#tablaUnidadAer', seccionUniEol, null, ruta_interna);

    //crearUploadUbicacionEolA();
    //crearUploadUnidadEolA();
    //crearUploadInfBasicaA();



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


function getEolHojaA() {
    var param = {};
    param.Centralnombre = $("#txtCentralnombre").val();
    param.Distrito = $("#cmbDistrito").val();
    param.Propietario = $("#cmbPropietario").val();
    param.OtroPropietario = $("#txtOtroPropietario").val();
    param.Sociooperador = $("#txtOperador").val();
    param.Socioinversionista = $("#txtInversionista").val();
    param.ConcesionTemporal= $("#cmbConsesionTem").val();
    param.Fechaconcesiontemporal = $("#txtFechaAdjudicacionTemporal").val();
    param.Tipoconcesionactual = $("#cmbtipoConsesionActual").val();
    param.Fechaconcesionactual = $("#txtFechaAdjudicacionActual").val();
    param.Nombreestacionmet = $("#txtNombreestacionmet").val();
    param.Numestacionmet = $("#txtNumestacionmet").val();
    param.SerieVelViento = $("#cmbSerieVelViento").val();
    param.Periododisanio = $("#txtPeriododisanio").val();
    param.EstudioGeologico = $("#cmbEstudioGeologico").val();
    param.Perfodiamantinas = $("#txtPerfodiamantinas").val();
    param.Numcalicatas = $("#txtNumcalicatas").val();
    param.Estudiotopografico = $("#cmbEstudioTopo").val();
    param.Levantamientotopografico = $("#txtLevantamientoTop").val();
    param.Potenciainstalada = $("#txtPotenciainstalada").val();
    param.Velvientoinstalada = $("#txtVelvientoinstalada").val();
    param.Horpotnominal = $("#txtHorpotnominal").val();
    param.Veldesconexion = $("#txtVeldesconexion").val();
    param.Velconexion = $("#txtVelconexion").val();
    param.Tipocontrcentral = $("#txtTipoControlCentral").val();
    param.Rangovelturbina = $("#txtRangoVelocidad").val();
    param.Tipoturbina = $("#cmbTipTurbina").val();
    param.Energiaanual = $("#txtEnergiaanualC").val();
    param.TipoParqEolico = $("#cmbTipParqueEol").val();
    param.Tipotecgenerador = $("#cmbTipGenerador").val();
    param.Numpalturbina = $("#txtTipoTurbina").val();
    param.Diarotor = $("#txtDiarotor").val();
    param.Longpala = $("#txtLongpala").val();
    param.Alturatorre = $("#txtAlturatorre").val();
    param.Potnomgenerador = $("#txtPotenciaNom").val();
    param.Numunidades = $("#txtNumeroUnidAero").val();
    param.Numpolos = $("#txtNumeroPolos").val();
    param.Tensiongeneracion = $("#txtTensionGeneracion").val();
    param.Bess = $("#cmbBacterias").val();
    param.Energiamaxbat = $("#txtEnergiamaxbat").val();
    param.Potenciamaxbat = $("#txtPotenciamaxbat").val();
    param.Eficargabat = $("#txtEficargabat").val();
    param.Efidescargabat = $("#txtEfidescargabat").val();
    param.TiempoMaxRegulacion = $("#txtTiempoMax").val();
    param.RampaCargDescarg = $("#txtRampaCargDes").val();
    param.Tensionkv = $("#txtTensionkv").val();
    param.Longitudkm = $("#txtLongitudkm").val();
    param.Numternas = $("#txtNTemasTC").val();
    param.Nombresubestacion = $("#cmbNomSubEsacionSein").val();
    param.NombreSubOtro = $("#txtNombreSubOtro").val();
    param.Perfil = $("#cmbPerfil").val();
    param.Prefactibilidad = $("#cmbPrefactibilidad").val();
    param.Factibilidad = $("#cmbFactibilidad").val();
    param.Estudiodefinitivo = $("#cmbEstudDefinitivo").val();
    param.Eia = $("#cmbEia").val();
    param.Fechainicioconstruccion = $("#txtFechaInicioContruccion").val();
    param.Periodoconstruccion = $("#txtFechaPeriodoContruccion").val();
    param.Fechaoperacioncomercial = $("#txtFechaOperacionComercial").val();
    param.Comentarios = $("#txtComentarios").val();
    console.log(param);
    return param;
}

function formatearFecha(fecha) {
    if (fecha.startsWith('/Date(')) {
        // Extraer el timestamp de "/Date(...)"
        const timestamp = parseInt(fecha.match(/-?\d+/)[0], 10);
        const date = new Date(timestamp);
        const day = String(date.getUTCDate()).padStart(2, '0');
        const month = String(date.getUTCMonth() + 1).padStart(2, '0'); // Los meses van de 0 a 11
        const year = date.getUTCFullYear();
        return `${day}-${month}-${year}`;
    } else if (/^\d{2}-\d{4}$/.test(fecha)) {
        // Manejar fechas como "mm-yyyy"
        const [month, year] = fecha.split('-');
        return `01-${month}-${year}`; // Asumimos el primer día del mes
    } else {
        return fecha; // Devuelve como está si no cumple ningún formato
    }
}
function cargarUbicacionBioA(idDistrito) {
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
$(document).ready(function () {
    // toggleInputGlobal("cmbPropietario", "otro", "0"); // Aplicar la lógica al iniciar

    // Agregar evento para detectar cambios manuales en el select
    $("#cmbPropietario").on("change", function () {
        toggleInputGlobal("cmbPropietario", "txtOtroPropietario", "0");
    });
});

function setEolHojaA(param) {
    $("#txtCentralnombre").val(param.CentralNombre);
    cargarUbicacionBioA(param.Distrito);
    $("#cmbPropietario").val(param.Propietario);
    $("#txtOtroPropietario").val(param.OtroPropietario);
    toggleInputGlobal("cmbPropietario", "txtOtroPropietario", "0");
    $("#txtOperador").val(param.SocioOperador);
    $("#txtInversionista").val(param.SocioInversionista);
    $("#cmbConsesionTem").val(param.ConcesionTemporal);
    $("#cmbtipoConsesionActual").val(param.TipoConcesionActual);
    setFechaPickerGlobal("#txtFechaAdjudicacionTemporal", param.FechaConcesionTemporal, "dd/mm/aaaa");
    setFechaPickerGlobal("#txtFechaAdjudicacionActual", param.FechaConcesionActual, "dd/mm/aaaa");

    $("#txtNombreestacionmet").val(param.NombreEstacionMet);
    $("#txtNumestacionmet").val(param.NumEstacionMet);
    $("#cmbSerieVelViento").val(param.SerieVelViento);
    $("#txtPeriododisanio").val(param.PeriodoDisAnio);
    $("#cmbEstudioGeologico").val(param.EstudioGeologico);
    $("#txtPerfodiamantinas").val(param.PerfoDiamantinas);
    $("#txtNumcalicatas").val(param.NumCalicatas);
    $("#cmbEstudioTopo").val(param.EstudioTopografico);
    $("#txtLevantamientoTop").val(param.LevantamientoTopografico);
    $("#txtPotenciainstalada").val(param.PotenciaInstalada);
    $("#txtVelvientoinstalada").val(param.VelVientoInstalada);
    $("#txtHorpotnominal").val(param.HorPotNominal);
    $("#txtVeldesconexion").val(param.VelDesconexion);
    $("#txtVelconexion").val(param.VelConexion);
    $("#txtTipoControlCentral").val(param.TipoContrCentral);
    $("#txtRangoVelocidad").val(param.RangoVelTurbina);
    $("#cmbTipTurbina").val(param.TipoTurbina);
    $("#txtEnergiaanualC").val(param.EnergiaAnual);
    $("#cmbTipParqueEol").val(param.TipoParqEolico);
    $("#cmbTipGenerador").val(param.TipoTecGenerador);
    $("#txtTipoTurbina").val(param.NumPalTurbina);
    $("#txtDiarotor").val(param.DiaRotor);
    $("#txtLongpala").val(param.LongPala);
    $("#txtAlturatorre").val(param.AlturaTorre);
    $("#txtPotenciaNom").val(param.PotNomGenerador);
    $("#txtNumeroUnidAero").val(param.NumUnidades);
    $("#txtNumeroPolos").val(param.NumPolos);
    $("#txtTensionGeneracion").val(param.TensionGeneracion);
    $("#cmbBacterias").val(param.Bess);
    $("#txtEnergiamaxbat").val(param.EnergiaMaxBat);
    $("#txtPotenciamaxbat").val(param.PotenciaMaxBat);
    $("#txtEficargabat").val(param.EfiCargaBat);
    $("#txtEfidescargabat").val(param.EfiDescargaBat);
    $("#txtTiempoMax").val(param.TiempoMaxRegulacion);
    $("#txtRampaCargDes").val(param.RampaCargDescarg);
    $("#txtTensionkv").val(param.TensionKv);
    $("#txtLongitudkm").val(param.LongitudKm);
    $("#txtNTemasTC").val(param.NumTernas);
    
    $("#cmbPerfil").val(param.Perfil);
    $("#cmbPrefactibilidad").val(param.Prefactibilidad);
    $("#cmbFactibilidad").val(param.Factibilidad);
    $("#cmbEstudDefinitivo").val(param.EstudioDefinitivo);
    $("#cmbEia").val(param.Eia);
    $("#txtFechaInicioContruccion").val(param.FechaInicioConstruccion);
    $("#txtFechaPeriodoContruccion").val(param.PeriodoConstruccion);
    $("#txtFechaOperacionComercial").val(param.FechaOperacionComercial);
    $("#txtComentarios").val(param.Comentarios);
    if (modoModel == "consultar") {
        desactivarCamposFormulario('FichaA');
        $("#btnGrabarEolicaHojaA").hide();
    }
    if (param.NombreSubOtro == "" || param.NombreSubOtro == null || param.NombreSubOtro == undefined || param.NombreSubestacion !="otro") {

        $("#cmbNomSubEsacionSein").val(param.NombreSubestacion);
        $('.otroOcultarg3').hide();
    } else {
        $("#cmbNomSubEsacionSein").val("otro");
        $('.otroOcultarg3').show();
        $("#txtNombreSubOtro").val(param.NombreSubOtro);
    }

}


async function cargarCatalogosYAsignarValoresAG3(param) {
    console.log("Verificando y cargando catálogos...");
   
    let promesas = [
       
        esperarCatalogo(catPropietario, "#cmbPropietario"),
        esperarCatalogo(catConcesionTemporal, "#cmbConsesionTem"),
        esperarCatalogo(catConcesionActual, "#cmbtipoConsesionActual"),
        esperarCatalogo(catSerieVelocidad, "#cmbSerieVelViento"),
        esperarCatalogo(catEstudioGeol, "#cmbEstudioGeologico"),
        esperarCatalogo(catEstudioTopo, "#cmbEstudioTopo"),
        esperarCatalogo(catTipTurbina, "#cmbTipTurbina"),
        esperarCatalogo(catTipParqueEol, "#cmbTipParqueEol"),
        esperarCatalogo(catTipGenerador, "#cmbTipGenerador"),
        esperarCatalogoSubestacion("#cmbNomSubEsacionSein", false),
        esperarCatalogo(catPerfil, "#cmbPerfil"),
        esperarCatalogo(catPrefactibilidad, "#cmbPrefactibilidad"),
        esperarCatalogo(catFactibilidad, "#cmbFactibilidad"),
        esperarCatalogo(catEstudDefinitivo, "#cmbEstudDefinitivo"),
        esperarCatalogo(catEia, "#cmbEia"),
        esperarCatalogo(catBacterias, "#cmbBacterias")


    ];

    // Asegurar que todas las promesas de carga se completen antes de continuar
    await Promise.all(promesas);

    console.log("Todos los catálogos han sido cargados. Asignando valores...");



    asignarSiExisteGlobal("#cmbPropietario", param.Propietario);
    asignarSiExisteGlobal("#cmbConsesionTem", param.Concesiontemporal);
    asignarSiExisteGlobal("#cmbtipoConsesionActual", param.Tipoconcesionactual);
    asignarSiExisteGlobal("#cmbSerieVelViento", param.SerieVelViento);
    asignarSiExisteGlobal("#cmbEstudioGeologico", param.EstudioGeologico);
    asignarSiExisteGlobal("#cmbEstudioTopo", param.EstudioTopografico);
    asignarSiExisteGlobal("#cmbTipTurbina", param.TipoTurbina);
    asignarSiExisteGlobal("#cmbTipParqueEol", param.TipoParqEolico);
    asignarSiExisteGlobal("#cmbTipGenerador", param.TipoTecGenerador);
    asignarSiExisteGlobal("#cmbNomSubEsacionSein", param.Nombresubestacion);
    asignarSiExisteGlobal("#cmbPerfil", param.Perfil);
    asignarSiExisteGlobal("#cmbPrefactibilidad", param.Prefactibilidad);
    asignarSiExisteGlobal("#cmbFactibilidad", param.Factibilidad);
    asignarSiExisteGlobal("#cmbEstudDefinitivo", param.Estudiodefinitivo);
    asignarSiExisteGlobal("#cmbEia", param.Eia);
    asignarSiExisteGlobal("#cmbBacterias", param.Bess);


    console.log("Valores asignados correctamente.");
}








async function cargarDatosA() {
    limpiarCombos('#cmbPropietario, #cmbConsesionTem, #cmbtipoConsesionActual, #cmbSerieVelViento, #cmbEstudioGeologico, #cmbEstudioTopo, #cmbTipTurbina, #cmbTipParqueEol, #cmbTipGenerador, #cmbNomSubEsacionSein, #cmbPerfil, #cmbPrefactibilidad, #cmbFactibilidad, #cmbEstudDefinitivo, #cmbEia, #cmbBacterias');

    // Asegurarse de que todos los catálogos estén listos antes de continuar
    await cargarCatalogosYAsignarValoresAG3({});

    console.log("Editar");
    if (modoModel === "editar" || modoModel === "consultar") {
        var idProyecto = $("#txtPoyCodi").val();
        $.ajax({
            type: 'POST',
            url: controladorFichas + 'GetEolHojaA',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: idProyecto }),
            success: function (response) {
                console.log(response);
                var hojaAData = response.responseResult;

                // Convertir distritos de texto a número si es necesario
                if (hojaAData && hojaAData.RegHojaEolADetDTOs) {
                    hojaAData.RegHojaEolADetDTOs = hojaAData.RegHojaEolADetDTOs.map(item => ({
                        ...item,
                        distrito: parseFloat(item.distrito) || 0 // Convertir 'distrito' a número
                    }));
                }

                if (hojaAData.Proycodi == 0) {
                    //hojaAData.FechaConcesionTemporal = obtenerFechaActual();
                    //hojaAData.FechaConcesionActual = obtenerFechaActual();
                    //hojaAData.Fechainicioconstruccion = obtenerFechaActualMesAnio();
                    //hojaAData.Fechaoperacioncomercial = obtenerFechaActualMesAnio();
                } else {
                    hojaAData.FechaConcesionTemporal = convertirFechaGlobal(hojaAData.FechaConcesionTemporal);
                    hojaAData.FechaConcesionActual = convertirFechaGlobal(hojaAData.FechaConcesionActual);

                }

                // Usar la función setDatosTablaTurbina para ordenar y cargar los datos
                setDatosTablaTurbina(hojaAData.RegHojaEolADetDTOs);


                setEolHojaA(hojaAData);
                cargarArchivosRegistrados(seccionUbicacionEOL, '#tablaUbicacionEol');
                cargarArchivosRegistrados(seccionUniEol, '#tablaUnidadAer');
                cambiosRealizados = false;
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
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


function grabarEolHojaA() {
   // cargarLoading();
    var param = {};
    param.TransmisionProyectoDTO = getCabeceraProyecto();
    var idProyecto = $("#txtPoyCodi").val();
    if (idProyecto != "0") {
        param.ProyCodi = idProyecto;
        param.RegHojaEolADTO = getEolHojaA();
        param.RegHojaEolADetDTOs = getDatosTablaTurbina();
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarEolHojaA',
            data: param,
            
            success: function (result) {
            //    stopLoading();
                if (result==1) {
                    mostrarMensaje('mensajeFicha', 'exito', 'Los datos se grabaron correctamente.');
                    cambiosRealizados = false;
                }
                else {
                    mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
             //   stopLoading();
                mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error.');
            }
        });
    }

}

function crearTablaTurbina() {
    var data = [];

    // Si no hay datos previos, inicializar con filas vacías
    if (!datosGrafico || datosGrafico.length === 0) {
        for (var i = 0; i < 100; i++) {
            data.push(["", ""]);
        }
    } else {
        data = datosGrafico.map(item => [item.Speed, item.Acciona]);
    }

    var grilla = document.getElementById('tableTurbina');

    if (typeof tablaEolHoja !== 'undefined' && tablaEolHoja !== null) {
        tablaEolHoja.destroy();
    }

    tablaEolHoja = new Handsontable(grilla, {
        data: data,
        rowHeaders: true,
        colHeaders: ['Speed', 'Potencia'],
        colWidths: 170,
        manualColumnResize: true,
        contextMenu: true
    });

    tablaEolHoja.render();
}


// function getDatosTablaTurbina() {
//     var datosArray = tablaEolHoja.getData();
//     var datosObjetos = datosArray.map(function (row) {
//         return {
//             Speed: row[0] || "", // Default a vacío si está indefinido
//             Acciona: row[1] || "" // Default a vacío si está indefinido
//         };
//     });
//     return datosObjetos;
// }
function setDatosTablaTurbina(nuevosDatos) {
    if (!nuevosDatos) {
        nuevosDatos = [];
    }

    // Ordenar los datos por el campo `CentralADetCodi`
    nuevosDatos.sort(function (a, b) {
        return a.CentralADetCodi - b.CentralADetCodi;
    });

    // Asegurar que haya al menos 100 filas, completando las vacías si es necesario
    var maxRows = 100;
    for (var i = nuevosDatos.length; i < maxRows; i++) {
        nuevosDatos.push({ Speed: "", Acciona: "" });
    }

    // Convertir los datos a formato de matriz para Handsontable
    var formattedData = nuevosDatos.map(function (item) {
        return [item.Speed, item.Acciona];
    });

    // Cargar los datos en Handsontable
    if (typeof tablaEolHoja !== 'undefined' && tablaEolHoja !== null) {
        tablaEolHoja.loadData(formattedData);
        tablaEolHoja.render();
    } else {
        console.error("La tabla Handsontable no está inicializada.");
    }
}
function generarGrafica() {
    var tableData = tablaEolHoja.getData();
    datosGrafico = getDatosTablaTurbina();
    var categories = [];
    var data = [];

    for (var i = 0; i < tableData.length; i++) {
        if (tableData[i][0] !== "" && tableData[i][1] !== "") {
            categories.push(parseFloat(tableData[i][0]).toFixed(2)); // Redondear a 2 decimales
            data.push(parseFloat(tableData[i][1]));
        }
    }

    // Calcular el valor máximo del eje Y, añadiéndole un 10%
    var maxValue = Math.max(...data) || 0; // Evitar errores si no hay datos
    var yAxisMax = maxValue + (maxValue * 0.1);

    // Configuración dinámica basada en el número de registros
    var labelOptions = categories.length > 50 ? {
        rotation: -45, // Rotar las etiquetas
        step: Math.ceil(categories.length / 50), // Espaciar etiquetas dinámicamente
        formatter: function () {
            return this.value; // Mostrar el valor sin cambios
        }
    } : {
        rotation: 0, // No rotar para menos de 50 registros
        step: 1,
        formatter: function () {
            return this.value; // Mostrar el valor sin cambios
        }
    };

    // Ajustar dinámicamente el ancho del gráfico según la cantidad de datos
    var chartWidth = categories.length > 10 ? Math.max(1000, categories.length * 20) : 600; // Ancho mínimo de 600 para gráficos pequeños

    Highcharts.chart("containerGraficaEO", {
        chart: {
            type: 'line',
            width: chartWidth, // Ancho dinámico del gráfico
            height: 400,
        },
        title: {
            text: "Potencia",
        },
        xAxis: {
            categories: categories,
            labels: labelOptions,
            title: {
                text: null,
            },
        },
        yAxis: {
            min: 0,
            max: yAxisMax, // Usar el valor dinámico calculado
            title: {
                text: null,
            },
        },
        series: [
            {
                name: "Potencia",
                data: data,
                marker: {
                    enabled: false,
                },
            },
        ],
        credits: {
            enabled: false,
        },
    });
}

function getDatosTablaTurbina() {
    var datosArray = tablaEolHoja.getData();
    var datosObjetos = datosArray.map(function (row) {
        return {
            Speed: row[0],               // speed
            Acciona: row[1]   // Potencia
        };
    }).filter(function (obj) {
        // Validar que algun campos tengan datos antes de guardarlos
        return obj.Speed !== "" || obj.Acciona !== "";
    });
    console.log(datosObjetos);
    return datosObjetos;
}


function cerrarTablaGrafico() {
    datosGrafico = getDatosTablaTurbina();
    $('#dataFichaAEo').show();
    $('#curvaTurbinaCon').hide();
}

function crearUploadUbicacionEolA() {
    uploaderUbicacionEolA = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSubirUbicacionEOL',
        url: controladorFichas + 'UploadArchivoGeneral',
        flash_swf_url: 'Scripts/Moxie.swf',
        silverlight_xap_url: 'Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '5mb',
            mime_types: [
        
                { title: "Archivos comprimidos", extensions: "zip,rar,kmz" }
            ]
        },
        init: {
            FilesAdded: function (up, files) {
                if (uploaderUbicacionEolA.files.length == 2) {
                    uploaderUbicacionEolA.removeFile(uploaderUbicacionEolA.files[0]);
                }
                uploaderUbicacionEolA.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarMensaje('mensajeFicha', 'alert', "Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            FileUploaded: function (up, file, response) {
                var json = JSON.parse(response.response);
                if (json.indicador == 1) {
                    up.removeFile(file);
                    procesarArchivoEolA(json.fileNameNotPath, json.nombreReal, json.extension, '#tablaUbicacionEol', seccionUbicacionEOL)
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
    uploaderUbicacionEolA.init();
}

function crearUploadInfBasicaA() {
    uploaderInfBasEolA = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSubirInfBasEol',
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
                if (uploaderInfBasEolA.files.length == 2) {
                    uploaderInfBasEolA.removeFile(uploaderInfBasEolA.files[0]);
                }
                uploaderInfBasEolA.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarMensaje('mensajeFicha', 'alert', "Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            FileUploaded: function (up, file, response) {
                var json = JSON.parse(response.response);
                if (json.indicador == 1) {
                    up.removeFile(file);
                    procesarArchivoEolA(json.fileNameNotPath, json.nombreReal, json.extension, '#tablaInfBasEOL', seccionInfBasiEol)
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
    uploaderInfBasEolA.init();
}


function crearUploadUnidadEolA() {
    uploaderUnidadEolA = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSubirUniEol',
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
                if (uploaderUnidadEolA.files.length == 2) {
                    uploaderUnidadEolA.removeFile(uploaderUnidadEolA.files[0]);
                }
                uploaderUnidadEolA.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarMensaje('mensajeFicha', 'alert', "Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            FileUploaded: function (up, file, response) {
                var json = JSON.parse(response.response);
                if (json.indicador == 1) {
                    up.removeFile(file);
                    procesarArchivoEolA(json.fileNameNotPath, json.nombreReal, json.extension, '#tablaUnidadAer', seccionUniEol)
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
    uploaderUnidadEolA.init();
}


function procesarArchivoEolA(fileNameNotPath, nombreReal, tipo, tabla, seccionCodigo) {
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
                agregarFilaTablaEOL(nombreReal, result.id, tabla);
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


function agregarFilaTablaEOL(nombre, id, tabla) {
    // Obtener el cuerpo de la tabla
    var nuevaFila = '<tr id="fila' + id + '">' +
        '<td>' + nombre + '</td>' +
        '<td> </td>' +
        '</tr>';
    var tabla = $(tabla);
    tabla.append(nuevaFila);

}

function exportarTablaExcelTurbinas() {
    var headers = [
        ["Speed", "Potencia"]
    ];

    // Obtener los datos de la tabla Handsontable (tablaEolHoja)
    var datosArray = tablaEolHoja.getData();
    // Concatenar los encabezados con los datos
    var data = headers.concat(datosArray);

    // Crear una hoja de trabajo (worksheet) a partir de los datos
    var ws = XLSX.utils.aoa_to_sheet(data);
    aplicarBordes(ws);
    // Crear un nuevo libro de trabajo (workbook) y agregar la hoja
    var wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "CC.Eol-A");

    // Exportar el libro de trabajo a un archivo Excel
    XLSX.writeFile(wb, "G3A_01-CurvaTurbina.xlsx");
}
function importarExcelTurbinas() {
    var fileInput = document.createElement("input");
    fileInput.type = "file";
    fileInput.accept = ".xlsx, .xls";
    fileInput.onchange = function (e) {
        var file = e.target.files[0];
        var reader = new FileReader();

        reader.onload = function (e) {
            var data = new Uint8Array(e.target.result);
            var workbook = XLSX.read(data, { type: "array" });

            // Obtener la primera hoja de trabajo (sheet)
            var firstSheet = workbook.Sheets[workbook.SheetNames[0]];

            // Convertir la hoja a formato JSON
            var jsonData = XLSX.utils.sheet_to_json(firstSheet, { header: 1 });

            // Llamar a la función para actualizar la tabla con los datos importados
            updateTableFromExcelTurbinas(jsonData);
            cambiosRealizados = true;
        };

        reader.readAsArrayBuffer(file);
    };

    fileInput.click();
}
function mostrarCampoOtro() {
    var seleccionado = $('#cmbNomSubEsacionSein').val();
    if (seleccionado == 'otro') {

        $('.otroOcultarg3').show(); // Mostrar todos los elementos con la clase 'otroOcultarg3'
    } else {
        $('.otroOcultarg3').hide(); // Ocultar todos los elementos con la clase 'otroOcultarg3'
        
    }
}


function updateTableFromExcelTurbinas(jsonData) {
    // Eliminar el encabezado (primera fila) de los datos
    var data = jsonData.slice(1);

    // Limitar los datos a las primeras dos columnas y convertir valores no numéricos a ""
    var filteredData = data.map(row => {
        return row.slice(0, 2).map(cell => {
            return !isNaN(parseFloat(cell)) && isFinite(cell) ? cell : ""; // Reemplazar valores no numéricos con ""
        });
    });

    // Completar con filas vacías si hay menos de 100 filas
    while (filteredData.length < 100) {
        filteredData.push(["", ""]); // Añadir filas con dos columnas vacías
    }

    // Actualizar los datos de la tabla Handsontable (tablaEolHoja) con los datos importados
    if (typeof tablaEolHoja !== 'undefined' && tablaEolHoja !== null) {
        tablaEolHoja.loadData(filteredData);
        tablaEolHoja.render();
    } else {
        console.error("La tabla Handsontable no está inicializada.");
    }
}




