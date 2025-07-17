
var catTituloHabilitante = 64;
var catTecnologia = 31;
var catRecursoUsa = 30;
var catPerfil = 16;
var catPrefactibilidad = 17;
var catFactibilidad = 18;
var catEstudDefinitivo = 19;
var catEia = 20;
var seccionUbicacionH2B = 48;
$(function () {
    var ruta_interna = obtenerRutaProyecto();  
    $('#curvaTurbinaCon').hide();

    $('#txtFechaAdjudicacionTemporal').Zebra_DatePicker({
        format: 'd/m/Y',
        onSelect: function () {
            cambiosRealizados = true;
        }
    });

    $('#txtFechaTitulo').Zebra_DatePicker({
        format: 'd/m/Y',
        onSelect: function () {
            cambiosRealizados = true;
        }
    });
    $('#fechaInicioConstruccion').Zebra_DatePicker({
        format: 'm/Y',
        onSelect: function () {
            cambiosRealizados = true;
        }
    });
    $('#fechaOperacion').Zebra_DatePicker({
        format: 'm/Y',
        onSelect: function () {
            cambiosRealizados = true;
        }
    });
    cargarCatalogo(catTituloHabilitante, "#cmbTipoElectrolizador");
    cargarCatalogo(catTecnologia, "#cmbTecnologia");
    cargarCatalogo(catRecursoUsa, "#txtRecursoUsado");
    cargarCatalogo(catPerfil, "#cmbPerfil");
    cargarCatalogo(catPrefactibilidad, "#cmbPrefactibilidad");
    cargarCatalogo(catFactibilidad, "#cmbFactibilidad");
    cargarCatalogo(catEstudDefinitivo, "#cmbEstudioDefinitivo");
    cargarCatalogo(catEia, "#cmbEIA");
    cargarDepartamentosH2VB();
    crearUploaderGeneral('btnSubirUbicacionB', '#tablaUbicacionB', seccionUbicacionH2B, null, ruta_interna);

});


function cargarDatosHojaB() {
    console.log("Editar");
    if (modoModel == "editar" || modoModel == "consultar") {
        var idProyecto = $("#txtPoyCodi").val();
        $.ajax({
            type: 'POST',
            url: controladorFichas + 'GetCuestionarioH2B',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: idProyecto }),
            
            success: function (response) {
                console.log(response);
                var hojaacues = response.responseResult;

                //$('#txtFechaAdjudicacionTemporal').val(obtenerFechaActual());
                //$('#txtFechaTitulo').val(obtenerFechaActual());
                //$('#fechaInicioConstruccion').val(obtenerFechaActualMesAnio());
                //$('#fechaOperacion').val(obtenerFechaActualMesAnio());


                if (hojaacues.Proycodi == 0) {
                    //hojaacues.FechaConcesionTemporal = obtenerFechaActual();
                    //hojaacues.FechaTituloHabilitante = obtenerFechaActual();
                    //hojaacues.FechaInicioConstruccion = obtenerFechaActualMesAnio();
                    //hojaacues.FechaOperacionComercial = obtenerFechaActualMesAnio();
                } else {
                    hojaacues.FechaConcesionTemporal = convertirFechaGlobal(hojaacues.FechaConcesionTemporal);
                    hojaacues.FechaTituloHabilitante = convertirFechaGlobal(hojaacues.FechaTituloHabilitante);

                }

                setCuestionarioH2VB(hojaacues);
                cambiosRealizados = false;
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function setCuestionarioH2VB(param) {
    if (modoModel == "consultar") {
        desactivarCamposFormulario('H2VB');
        $('#btnGrabarH2VB').hide();
    }
    $("#txtNombreUnidad").val(param.NombreUnidad);
    console.log("distrito B es " + param.Distrito);

    cargarUbicacionH2VB(param.Distrito);
    $("#txtPropietario").val(param.Propietario);
    $("#txtSocioOperadorB").val(param.SocioOperador);
    $("#txtSocioInversionistaB").val(param.SocioInversionista);
    $(`#rbIncluidoPlan${param.IncluidoPlanTrans}`).prop("checked", true);
    //$("#rbIncluidoPlan").val(param.IncluidoPlanTrans);
    //$("#rbConcesionTemporal").val(param.ConcesionTemporal);
    $(`#rbConcesionTemporal${param.ConcesionTemporal}`).prop("checked", true);
    $("#cmbTipoElectrolizador").val(param.TipoElectrolizador);
    $("#txtFechaAdjudicacionTemporal").val(param.FechaConcesionTemporal);
    $("#txtFechaTitulo").val(param.FechaTituloHabilitante);
    $("#cmbPerfil").val(param.Perfil);
    $("#cmbPrefactibilidad").val(param.Prefactibilidad);
    $("#cmbFactibilidad").val(param.Factibilidad);
    $("#cmbEstudioDefinitivo").val(param.EstudioDefinitivo);
    $("#cmbEIA").val(param.EIA);
    $("#fechaInicioConstruccion").val(param.FechaInicioConstruccion);
    $("#txtPeriodoConstruccion").val(param.PeriodoConstruccion);
    $("#fechaOperacion").val(param.FechaOperacionComercial);
    $("#txtPotenciaInstalada").val(param.PotenciaInstalada);
    $("#txtRecursoUsado").val(param.RecursoUsado);
    $("#cmbTecnologia").val(param.Tecnologia);
    $("#txtOtroTecnologia").val(param.OtroTecnologia);
    $("#txtBarraConexion").val(param.BarraConexion);
    $("#txtNivelTensionB").val(param.NivelTension);
    $("#txtComentariosB").val(param.Comentarios);
    setupDropdownToggle('cmbTecnologia', 'txtOtroTecnologia');
    cargarArchivosRegistrados(seccionUbicacionH2B, '#tablaUbicacionB');

}

function getCuestionarioH2VB() {
    var param = {};
    param.NombreUnidad = $("#txtNombreUnidad").val();
    console.log("NombreUnidad" + param.NombreUnidad);
    param.Distrito = $("#distritosSelectB").val();
    console.log("Distrito" + param.Distrito);
    param.Propietario = $("#txtPropietario").val();
    console.log("Propietario" + param.Propietario);
    param.SocioOperador = $("#txtSocioOperadorB").val();
    console.log("SocioOperador" + param.SocioOperador);
    param.SocioInversionista = $("#txtSocioInversionistaB").val();
    console.log("SocioInversionista" + param.SocioInversionista);
    param.IncluidoPlanTrans = $('input[name="rbIncluidoPlan"]:checked').val();
    //param.IncluidoPlanTrans = $("#rbIncluidoPlan").val();
    param.ConcesionTemporal = $('input[name="rbIncluidoPlan"]:checked').val();
    //param.ConcesionTemporal = $("#rbConcesionTemporal").val();
    param.TipoElectrolizador = $("#cmbTipoElectrolizador").val();
    param.FechaConcesionTemporal = $("#txtFechaAdjudicacionTemporal").val();
    param.FechaTituloHabilitante = $("#txtFechaTitulo").val();
    param.Perfil = $("#cmbPerfil").val();
    param.Prefactibilidad = $("#cmbPrefactibilidad").val();
    param.Factibilidad = $("#cmbFactibilidad").val();
    param.EstudioDefinitivo = $("#cmbEstudioDefinitivo").val();
    param.EIA = $("#cmbEIA").val();
    param.FechaInicioConstruccion = $("#fechaInicioConstruccion").val();
    param.PeriodoConstruccion = $("#txtPeriodoConstruccion").val();
    param.FechaOperacionComercial = $("#fechaOperacion").val();
    param.PotenciaInstalada = $("#txtPotenciaInstalada").val();
    param.RecursoUsado = $("#txtRecursoUsado").val();
    param.Tecnologia = $("#cmbTecnologia").val();
    param.OtroTecnologia = $("#txtOtroTecnologia").val();
    param.BarraConexion = $("#txtBarraConexion").val();
    param.NivelTension = $("#txtNivelTensionB").val();
    console.log("NivelTension" + param.NivelTension);
    param.Comentarios = $("#txtComentariosB").val();
    console.log("Comentarios" + param.Comarios);
    console.log("Comentarios" + param);
    return param;
}

function grabarCuestionarioH2VB() {
    var param = {};
    param.TransmisionProyectoDTO = getCabeceraProyecto();
    var idProyecto = $("#txtPoyCodi").val();
    if (idProyecto != "0") {
        param.ProyCodi = idProyecto;
        
        param.CuestionarioH2VBDTO = getCuestionarioH2VB();
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarCuestionarioH2VB',
            data: param,
            
            success: function (result) {
                if (result) {
                    mostrarMensaje('mensajeFicha', 'exito', 'Los datos se grabaron correctamente.');
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
}

//function convertirFecha(fechaJSON) {

//    let timestamp = parseInt(fechaJSON.match(/\d+/)[0]);

//    let fecha = new Date(timestamp);

//    let dia = fecha.getDate().toString().padStart(2, '0');
//    let mes = (fecha.getMonth() + 1).toString().padStart(2, '0');
//    let anio = fecha.getFullYear();

//    let fechaFormateada = `${dia}-${mes}-${anio}`;
//    return fechaFormateada;
//}

function cargarDepartamentosH2VB() {
    $.ajax({
        type: 'POST',
        url: controladorFichas + 'ListarDepartamentos',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(''),
        success: function (eData) {
            console.log(eData);
            cargarListaDepartamentoH2VB(eData);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}
function cargarListaDepartamentoH2VB(departamentos) {
    var selectDepartamentos = $('#departamentosSelectB');
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
        cargarProvinciaH2VB(idSeleccionado);
    });

}
function cargarProvinciaH2VB(id, callback) {
    $.ajax({
        type: 'POST',
        url: controladorFichas + 'ListarProvincias',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: id }),
        success: function (eData) {
            console.log(eData);
            cargarListaProvinciaH2VB(eData);
            if (typeof callback === 'function') {
                callback();
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}
function cargarListaProvinciaH2VB(provincias) {
    var selectProvincias = $('#provinciasSelectB');
    selectProvincias.empty();
    var optionDefault = $('<option>', {
        value: "",
        text: "Seleccione una provincia"
    });
    selectProvincias.append(optionDefault);
    cargarListaDistritosH2VB([]);
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
        cargarDistritoH2VB(idSeleccionado);
    });

}
function cargarDistritoH2VB(id, callback) {
    $.ajax({
        type: 'POST',
        url: controladorFichas + 'ListarDistritos',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: id }),
        success: function (eData) {
            console.log(eData);
            cargarListaDistritosH2VB(eData);
            if (typeof callback === 'function') {
                callback();
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}
function cargarListaDistritosH2VB(distritos) {
    var selectDistritos = $('#distritosSelectB');
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
function cargarUbicacionH2VB(idDistrito) {
    $.ajax({
        type: 'POST',
        url: controladorFichas + 'UbicacionByDistro',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: idDistrito }),
        success: function (ubicacion) {
            $('#departamentosSelectB').val(ubicacion.DepartamentoId);

            cargarProvinciaH2VB(ubicacion.DepartamentoId, function () {
                $('#provinciasSelectB').val(ubicacion.ProvinciaId);

                cargarDistritoH2VB(ubicacion.ProvinciaId, function () {
                    $('#distritosSelectB').val(ubicacion.DistritoId);

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