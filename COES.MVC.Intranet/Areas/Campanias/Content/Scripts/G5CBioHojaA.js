var catNomSubEsacionSein = 15;
var catPerfil = 16;
var catPrefactibilidad = 17;
var catFactibilidad = 18;
var catEstudDefinitivo = 19;
var catEia = 20;
var catCombustible = 34;
seccionUbicacionBio = 31;
seccionUniBio = 32;
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
    //cargarCatalogo(catPropietario, "#Propietario");
    //cargarCatalogo(catConcesionTemporal, "#ConTemporal");
    //cargarCatalogo(catConcesionActual, "#TipoConActual");
    //cargarCatalogoSubestacion("#NomSubEstacion");
    //cargarCatalogo(catCombustible, "#TipoNomComb");
    //cargarCatalogo(catPerfil, "#Perfil");
    //cargarCatalogo(catPrefactibilidad, "#Prefactibilidad");
    //cargarCatalogo(catFactibilidad, "#Factibilidad");
    //cargarCatalogo(catEstudDefinitivo, "#EstDefinitivo");
    //cargarCatalogo(catEia, "#Eia");

    //cargarCatalogo(catPodCalInf, "#CombPoderCalorInf");
    //cargarCatalogo(catPodCalSup, "#CombPoderCalorSup");
    //cargarCatalogo(catCComb, "#CombCostoCombustible");
    //cargarCatalogo(catCTratComb, "#CombCostTratamiento");
    //cargarCatalogo(catCTranspComb, "#CombCostTransporte");
    //cargarCatalogo(catCVarNComb, "#CombCostoVariableNoComb");
    //cargarCatalogo(catCInvIni, "#CombCostoInversion");
    //cargarCatalogo(catRendPl, "#CombRendPlanta");
    //cargarCatalogo(catConsEspCon, "#CombConsEspec");
    $('#btnGrabarBioFichaA').on('click', function () {
        grabarFichaA();
    });
    //$('#FecAdjudicacionTemp').val(obtenerFechaActual());
    //$('#FecAdjudicacionAct').val(obtenerFechaActual());
    //$('#FecInicioConst').val(obtenerFechaActualMesAnio());
    //$('#FecOperacionComer').val(obtenerFechaActualMesAnio());

    $('#FecAdjudicacionTemp').Zebra_DatePicker({
        format: 'd/m/Y',
        onSelect: function () {
            cambiosRealizados = true; // Marcar que hubo un cambio
        }
    });

    $('#FecAdjudicacionAct').Zebra_DatePicker({
        format: 'd/m/Y',
        onSelect: function () {
            cambiosRealizados = true; // Marcar que hubo un cambio
        }
    });
    $('#FecInicioConst').Zebra_DatePicker({
        format: 'm/Y',
        onSelect: function () {
            cambiosRealizados = true; // Marcar que hubo un cambio
        }
    });
    $('#FecOperacionComer').Zebra_DatePicker({
        format: 'm/Y',
        onSelect: function () {
            cambiosRealizados = true; // Marcar que hubo un cambio
        }
    });
    cargarDepartamentos();
    var ruta_interna = obtenerRutaProyecto(); 
    
    crearUploaderGeneral('btnSubirUbicacionBio', '#tablaUbicacionBio', seccionUbicacionBio, null, ruta_interna);
    crearUploaderGeneral('btnSubirUniBio', '#tablaUnidadAerBio', seccionUniBio, null, ruta_interna);
  
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
    var selectDepartamentos = $('#departamento');
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
    var selectProvincias = $('#provincia');
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
    var selectDistritos = $('#Distrito');
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

function getBioHojaA() {
    var param = {};
    param.CentralNombre = $("#CentralNombre").val();
    param.Distrito = $("#Distrito").val();
    param.Propietario = $("#Propietario").val();
    param.Otro = $("#Otro").val();
    param.SocioOperador = $("#SocioOperador").val();
    param.SocioInversionista = $("#SocioInversionista").val();
    param.ConTemporal = $("#ConTemporal").val();
    param.FecAdjudicacionTemp =$("#FecAdjudicacionTemp").val();
    param.TipoConActual = $("#TipoConActual").val();
    param.FecAdjudicacionAct = $("#FecAdjudicacionAct").val();
    param.PotInstalada = $("#PotInstalada").val();
    param.TipoNomComb = $("#TipoNomComb").val();
    param.OtroComb = $("#OtroComb").val();
    param.PotMaxima = $("#PotMaxima").val();
    param.PoderCalorInf = $("#PoderCalorInf").val();
    param.CombPoderCalorInf = $("#CombPoderCalorInf").val();
    param.PotMinima = $("#PotMinima").val();
    param.PoderCalorSup = $("#PoderCalorSup").val();
    param.CombPoderCalorSup = $("#CombPoderCalorSup").val();
    param.CostCombustible = $("#CostCombustible").val();
    param.CombCostoCombustible = $("#CombCostoCombustible").val();
    param.CostTratamiento = $("#CostTratamiento").val();
    param.CombCostTratamiento = $("#CombCostTratamiento").val();
    param.CostTransporte = $("#CostTransporte").val();
    param.CombCostTransporte = $("#CombCostTransporte").val();
    param.CostoVariableNoComb = $("#CostoVariableNoComb").val();
    param.CombCostoVariableNoComb = $("#CombCostoVariableNoComb").val();
    param.CostInversion = $("#CostInversion").val();
    param.CombCostoInversion = $("#CombCostoInversion").val();
    param.RendPlanta = $("#RendPlanta").val();
    param.CombRendPlanta = $("#CombRendPlanta").val();
    param.ConsEspec = $("#ConsEspec").val();
    param.CombConsEspec = $("#CombConsEspec").val();
    param.TipoMotorTer = $("#TipoMotorTer").val();
    param.VelNomRotacion = $("#VelNomRotacion").val();
    param.PotEjeMotorTer = $("#PotEjeMotorTer").val();
    param.NumMotoresTer = $("#NumMotoresTer").val();
    param.PotNomGenerador = $("#PotNomGenerador").val();
    param.NumGeneradores = $("#NumGeneradores").val();
    param.TipoGenerador = $("#TipoGenerador").val();
    param.TenGeneracion = $("#TenGeneracion").val();
    param.Tension = $("#Tension").val();
    param.Longitud = $("#Longitud").val();
    param.NumTernas = $("#NumTernas").val();
    param.NomSubEstacion = $("#NomSubEstacion").val();
    param.OtroSubEstacion = $("#OtroSubEstacion").val();
    param.Perfil = $("#Perfil").val();
    param.Prefactibilidad = $("#Prefactibilidad").val();
    param.Factibilidad = $("#Factibilidad").val();
    param.EstDefinitivo = $("#EstDefinitivo").val();
    param.Eia = $("#Eia").val();
    param.FecInicioConst = $("#FecInicioConst").val();
    param.PeriodoConst = $("#PeriodoConst").val();
    param.FecOperacionComer = $("#FecOperacionComer").val();
    param.Comentarios = $("#Comentarios").val();
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

function procesarArchivoInfBas(fileNameNotPath, nombreReal, tipo) {
    param = {};
    param.SeccCodi = 2;
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

function procesarArchivoUbicacion(fileNameNotPath, nombreReal, tipo) {
    param = {};
    param.SeccCodi = 1;
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

function agregarFilaUbicacion(nombre, id) {
    // Obtener el cuerpo de la tabla
    var nuevaFila = '<tr id="fila' + id + '">' +
        '<td>' + nombre + '</td>' +
        '<td> <a href="#"><img src="' + siteRoot + 'Content/Images/eliminar.png" class="btnEliminar" onclick="eliminarFile(' + id + ')" title="Eliminar archivo"/></a></td>' +
        '</tr>';
    var tabla = $('#tablaUbicacion');
    tabla.append(nuevaFila);
}

function agregarFilaInfBas(nombre, id) {
    // Obtener el cuerpo de la tabla
    var nuevaFila = '<tr id="fila' + id + '">' +
        '<td>' + nombre + '</td>' +
        '<td> <a href="#"><img src="' + siteRoot + 'Content/Images/eliminar.png" class="btnEliminar" onclick="eliminarFile(' + id + ')" title="Eliminar archivo"/></a></td>' +
        '</tr>';
    var tabla = $('#tablaInfBasica');
    tabla.append(nuevaFila);
}

function grabarFichaA() {
    var param = {};
    param.TransmisionProyectoDTO = getCabeceraProyecto();
    var idProyecto = $("#txtPoyCodi").val();
    if (idProyecto != "0") {
        param.ProyCodi = idProyecto;
        param.BioHojaADTO = getBioHojaA();
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarBioHojaA',
            data: param,
            
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensajeFicha', 'exito', 'Los datos se grabaron correctamente.');
                } else {
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

// Ejecutar al cargar la página para el caso de "Propietario"
$(document).ready(function () {
   // toggleInputGlobal("Propietario", "Otro", "0"); // Aplicar la lógica al iniciar

    // Agregar evento para detectar cambios manuales en el select
    $("#Propietario").on("change", function () {
        toggleInputGlobal("Propietario", "Otro", "0");
    });
    $("#NomSubEstacion").on("change", function () {
        toggleInputGlobal("NomSubEstacion", "OtroSubEstacion", "0");
    });
});

function setBioHojaA(param) {
    $("#CentralNombre").val(param.CentralNombre);
    //$("#Distrito").val(param.Distrito);
    cargarUbicacionBioA(param.Distrito);
    $("#Propietario").val(param.Propietario);
    $("#Otro").val(param.Otro);
    toggleInputGlobal("Propietario", "Otro", "0");

    $("#SocioOperador").val(param.SocioOperador);
    $("#SocioInversionista").val(param.SocioInversionista);
    $("#ConTemporal").val(param.ConTemporal);
    setFechaPickerGlobal("#FecAdjudicacionTemp", param.FecAdjudicacionTemp, "dd/mm/aaaa");
    $("#TipoConActual").val(param.TipoConActual);
    setFechaPickerGlobal("#FecAdjudicacionAct", param.FecAdjudicacionAct, "dd/mm/aaaa");
    $("#PotInstalada").val(param.PotInstalada);
    $("#TipoNomComb").val(param.TipoNomComb);
    $("#OtroComb").val(param.OtroComb);
    $("#PotMaxima").val(param.PotMaxima);
    $("#PoderCalorInf").val(param.PoderCalorInf);
    $("#CombPoderCalorInf").val(param.CombPoderCalorInf);
    $("#PotMinima").val(param.PotMinima);
    $("#PoderCalorSup").val(param.PoderCalorSup);
    $("#CombPoderCalorSup").val(param.CombPoderCalorSup);
    $("#CostCombustible").val(param.CostCombustible);
    $("#CombCostoCombustible").val(param.CombCostoCombustible);
    $("#CostTratamiento").val(param.CostTratamiento);
    $("#CombCostTratamiento").val(param.CombCostTratamiento);
    $("#CostTransporte").val(param.CostTransporte);
    $("#CombCostTransporte").val(param.CombCostTransporte);
    $("#CostoVariableNoComb").val(param.CostoVariableNoComb);
    $("#CombCostoVariableNoComb").val(param.CombCostoVariableNoComb);
    $("#CostInversion").val(param.CostInversion);
    $("#CombCostoInversion").val(param.CombCostoInversion);
    $("#RendPlanta").val(param.RendPlanta);
    $("#CombRendPlanta").val(param.CombRendPlanta);
    $("#ConsEspec").val(param.ConsEspec);
    $("#CombConsEspec").val(param.CombConsEspec);
    $("#TipoMotorTer").val(param.TipoMotorTer);
    $("#VelNomRotacion").val(param.VelNomRotacion);
    $("#PotEjeMotorTer").val(param.PotEjeMotorTer);
    $("#NumMotoresTer").val(param.NumMotoresTer);
    $("#PotNomGenerador").val(param.PotNomGenerador);
    $("#NumGeneradores").val(param.NumGeneradores);
    $("#TipoGenerador").val(param.TipoGenerador);
    $("#TenGeneracion").val(param.TenGeneracion);
    $("#Tension").val(param.Tension);
    $("#Longitud").val(param.Longitud);
    $("#NumTernas").val(param.NumTernas);
    $("#NomSubEstacion").val(param.NomSubEstacion);
    $("#OtroSubEstacion").val(param.OtroSubEstacion);

    toggleInputGlobal("NomSubEstacion", "OtroSubEstacion", "0");
    $("#Perfil").val(param.Perfil);
    $("#Prefactibilidad").val(param.Prefactibilidad);
    $("#Factibilidad").val(param.Factibilidad);
    $("#EstDefinitivo").val(param.EstDefinitivo);
    $("#Eia").val(param.Eia);
    $("#FecInicioConst").val(param.FecInicioConst);
    $("#PeriodoConst").val(param.PeriodoConst);
    $("#FecOperacionComer").val(param.FecOperacionComer);
    $("#Comentarios").val(param.Comentarios);
    setupDropdownToggle('TipoNomComb', 'OtroComb');
    cargarArchivosRegistrados(seccionUbicacionBio, '#tablaUbicacionBio');
    cargarArchivosRegistrados(seccionUniBio, '#tablaUnidadAerBio');
    if (modoModel == "consultar") {
        desactivarCamposFormulario('BFichaA');
        $("#btnGrabarBioFichaA").hide();
    }
}

async function cargarDatosA() {
    limpiarCombos('#Propietario, #ConTemporal, #TipoConActual, #Perfil, #Prefactibilidad, #Factibilidad, #EstDefinitivo, #Eia, #TipoNomComb, #CombPoderCalorInf, #CombPoderCalorSup, #CombCostoCombustible, #CombCostTratamiento, #CombCostTransporte, #CombCostoVariableNoComb, #CombCostoInversion, #CombRendPlanta, #CombConsEspec');
    limpiarCombos2('#NomSubEstacion');
    
    console.log("Cargando catálogos...");

    // Primero, se cargan los catálogos y se asignan valores
    await cargarCatalogosYAsignarValoresAG5();

    console.log("Todos los catálogos han sido cargados. Procediendo a obtener los datos del proyecto...");

    if (modoModel === "editar" || modoModel === "consultar") {
        var idProyecto = $("#txtPoyCodi").val();

        try {
            const response = await $.ajax({
                type: 'POST',
                url: controladorFichas + 'GetBioHojaA',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ id: idProyecto }),
            });

            console.log("Respuesta del servidor:", response);

            var hojaAData = response.responseResult;

            if (hojaAData.Proycodi == 0) {
                //hojaAData.FecAdjudicacionTemp = obtenerFechaActual();
                //hojaAData.FecAdjudicacionAct = obtenerFechaActual();
                //hojaAData.FecInicioConst = obtenerFechaActualMesAnio();
                //hojaAData.FecOperacionComer = obtenerFechaActualMesAnio();
            } else {
                hojaAData.FecAdjudicacionTemp = convertirFechaGlobal(hojaAData.FecAdjudicacionTemp);
                hojaAData.FecAdjudicacionAct = convertirFechaGlobal(hojaAData.FecAdjudicacionAct);
            }

            // Llamar a setBioHojaA solo después de que todos los catálogos estén listos
            await setBioHojaA(hojaAData);
            cambiosRealizados = false;

        } catch (error) {
            console.error("Error en la carga de datos:", error);
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error al cargar los datos del proyecto.');
        }
    }
}

async function cargarCatalogosYAsignarValoresAG5() {
    console.log("Verificando y cargando catálogos...");

    let promesas = [
        esperarCatalogo(catPropietario, "#Propietario"),
        esperarCatalogo(catConcesionTemporal, "#ConTemporal"),
        esperarCatalogo(catConcesionActual, "#TipoConActual"),
        esperarCatalogo(catPerfil, "#Perfil"),
        esperarCatalogo(catPrefactibilidad, "#Prefactibilidad"),
        esperarCatalogo(catFactibilidad, "#Factibilidad"),
        esperarCatalogo(catEstudDefinitivo, "#EstDefinitivo"),
        esperarCatalogo(catEia, "#Eia"),
        esperarCatalogo(catCombustible, "#TipoNomComb"),
        esperarCatalogo(catPodCalInf, "#CombPoderCalorInf"),
        esperarCatalogo(catPodCalSup, "#CombPoderCalorSup"),
        esperarCatalogo(catCComb, "#CombCostoCombustible"),
        esperarCatalogo(catCTratComb, "#CombCostTratamiento"),
        esperarCatalogo(catCTranspComb, "#CombCostTransporte"),
        esperarCatalogo(catCVarNComb, "#CombCostoVariableNoComb"),
        esperarCatalogo(catCInvIni, "#CombCostoInversion"),
        esperarCatalogo(catRendPl, "#CombRendPlanta"),
        esperarCatalogo(catConsEspCon, "#CombConsEspec"),
        esperarCatalogoSubestacion("#NomSubEstacion", false) // Se añade el catálogo de subestaciones
    ];

    // Asegurar que todas las promesas de carga se completen antes de continuar
    await Promise.all(promesas);

    console.log("Todos los catálogos han sido cargados. Asignando valores...");
}

function cargarUbicacionBioA(idDistrito) {
    $.ajax({
        type: 'POST',
        url: controladorFichas + 'UbicacionByDistro',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: idDistrito }),
        success: function (ubicacion) {
            $('#departamento').val(ubicacion.DepartamentoId);

            cargarProvincia(ubicacion.DepartamentoId, function () {
                $('#provincia').val(ubicacion.ProvinciaId);

                cargarDistrito(ubicacion.ProvinciaId, function () {
                    $('#Distrito').val(ubicacion.DistritoId);

                });
            });
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

