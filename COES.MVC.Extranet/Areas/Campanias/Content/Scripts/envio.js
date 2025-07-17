var controlador = siteRoot + 'campanias/plantransmision/';
var controladorFichas = siteRoot + 'campanias/fichasproyecto/';
var idEliminacion = 0;
var planTransEncontrado = null;
$(function () {
    console.log("Entra1");
    $('#btnAgregar').on('click', function (event) {
        event.preventDefault();
        agregarP();
    });
    $('#btnEnviar').on('click', function () {
        enviar();
    });

    $('#btnAtrasP').click(function () {
        window.location.href = controlador;
    });
    
   
});

function cargarDatos() {
    return Promise.all([cargarEmpresa()])
        .then(cargarPeriodo);
}

function cargarEmpresaProyecto() {
    return new Promise((resolve, reject) => {
        $.ajax({
            type: 'POST',
            url: controlador + 'ListarEmpresas',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(''),
            success: function (eData) {
                console.log(eData);
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


function ObtenerListadoProyecto(codigoPlan, modo) {
    $("#listadoProyecto").html('');
    console.log("entro a la tramsision");
    $.ajax({
        type: 'POST',
        url: controlador + 'ListadoProyecto',
        data: {
            id: codigoPlan,
            modo: modo
        },
        success: function (eData) {
            $('#listadoProyecto').css("width", $('.form-main').width() + "px");
            $('#listadoProyecto').html(eData);
            $('#tablaProyecto').dataTable({
                bJQueryUI: true,
                "scrollY": 440,
                "scrollX": false,
                "sDom": 'ft',
                "ordering": true,
                "order": [[0, 'desc']],
                "iDisplayLength": -1
            });
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

agregarP = function () {
    var emp = $("#empresaSelect").val();
    var plan = $("#periodoSelect").val();
    $.ajax({
        type: 'POST',
        url: controlador + 'proyecto',
        data: {
            modo: "agregar",
            id: 0,
            emp,
            plan,
        },
        
        success: function (evt) {
            $('#contenidoProyecto').html(evt);

            $("#modal1").modal({
                escapeClose: false,
                clickClose: false,
                showClose: true
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

editarProyecto = function (id) {

    $.ajax({
        type: 'POST',
        url: controlador + 'proyecto',
        data: {
            id: id,
            modo: "editar"
        },
        
        success: function (evt) {
            $('#contenidoProyecto').html(evt);

            $("#modal1").modal({
                escapeClose: false,
                clickClose: false,
                showClose: true
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

consultarProyecto = function (id) {

    $.ajax({
        type: 'POST',
        url: controlador + 'proyecto',
        data: {
            id: id,
            modo: "consultar"
        },
        
        success: function (evt) {
            $('#contenidoProyecto').html(evt);

            $("#modal1").modal({
                escapeClose: false,
                clickClose: false,
                showClose: true
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

enviar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "Proyecto",
        success: function (evt) {
            $('#popup').html(evt);
            setTimeout(function () {
                $('#popup').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
        },
        error: function () {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

function cargarDepartamentos(){
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

function buscarPlantransmision(codPlanTr) {
    $.ajax({
        type: 'POST',
        url: controlador + 'BuscarPlanTransmision',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: codPlanTr }),
        success: function (eData) {
            if (eData != null && eData.length > 0 ) {
                var planT = eData[0];
                planTransEncontrado = planT;
                var periodoCodi = String(planT.Pericodi);
                $("#empresaSelect").val(planT.Codempresa).prop("disabled", true);
                $("#periodoSelect").val(periodoCodi).prop("disabled", true);
                $("#txtversion").val(planT.Planversion);
                console.log(planT);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function clonarPlantransmision(codPlanTr) {
    if(planTransEncontrado == null){
        $.ajax({
            type: 'POST',
            url: controlador + 'ClonarPlanTransmision',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: codPlanTr }),
            success: function (eData) {
                console.log("Plan transmision");
                console.log(eData);
                var idN = codPlanTr;
                if (eData.dataPlant != null) {
                    var planT = eData.dataPlant;
                    planTransEncontrado = planT;
                    idN = planT.Plancodi;
                    var periodoCodi = String(planT.Pericodi);
                    $("#empresaSelect").val(planT.Codempresa).prop("disabled", true);
                    $("#periodoSelect").val(periodoCodi).prop("disabled", true);
                    $("#txtCodPlanTransmision").val(idN);
                    console.log('idN',idN);
                }
                ObtenerListadoProyecto(idN,"false")

                let url = new URL(window.location.href);
                let idValue = url.searchParams.get("idN");
                if (idValue !== null) {
                    url.searchParams.delete("idN"); 
                    url.searchParams.set("id", idN)
                    window.history.replaceState(null, "", url);
                }
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    }
}


function cargarListaDepartamento(departamentos) {
    var selectDepartamentos = $('#departamentosSelect');
    $.each(departamentos, function (index, departamento) {
        // Crear la opción
        var option = $('<option>', {
            value: departamento.Id,
            text: departamento.Nombre
        });

        // Agregar la opción al select
        selectDepartamentos.append(option);
    });

    selectDepartamentos.change(function () {
        // Obtener el id del departamento seleccionado
        var idSeleccionado = $(this).val();
        // Llamar a la función con el id del departamento
        console.log(idSeleccionado);
        cargarProvincia(idSeleccionado);
    });

}

function cargarProvincia(id) {
    $.ajax({
        type: 'POST',
        url: controladorFichas + 'ListarProvincias',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: id }),
        success: function (eData) {
            console.log(eData);
            cargarListaProvincia(eData);
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
        // Crear la opción
        var option = $('<option>', {
            value: provincia.Id,
            text: provincia.Nombre
        });

        // Agregar la opción al select
        selectProvincias.append(option);
    });

    selectProvincias.change(function () {
        // Obtener el id del departamento seleccionado
        var idSeleccionado = $(this).val();
        // Llamar a la función con el id del departamento
        cargarDistrito(idSeleccionado);
    });

}

function cargarDistrito(id) {
    $.ajax({
        type: 'POST',
        url: controladorFichas + 'ListarDistritos',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: id }),
        success: function (eData) {
            console.log(eData);
            cargarListaDistritos(eData);
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
        // Crear la opción
        var option = $('<option>', {
            value: distrito.Id,
            text: distrito.Nombre
        });

        // Agregar la opción al select
        selectDistritos.append(option);
    });

}


function cargarEmpresa() {
    return new Promise((resolve, reject) => {
        $.ajax({
            type: 'POST',
            url: controlador + 'ListarEmpresas',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(''),
            success: function (eData) {
                console.log(eData);
                cargarListaEmpresas(eData);
                resolve();
            },
            error: function (err) {
                alert("Ha ocurrido un error");
                reject(err);
            }
        });
    });
}

function cargarListaEmpresas(empresas) {
    var selectEmpresa = $('#empresaSelect');
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

function cargarPeriodo() {
    return new Promise((resolve, reject) => {
        $.ajax({
            type: 'POST',
            url: controlador + 'ListarPeriodos',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(''),
            success: function (eData) {
                console.log(eData);
                cargarListaPeriodo(eData);
                resolve();
            },
            error: function (err) {
                alert("Ha ocurrido un error");
                reject(err);
            }
        });

    });
}

function cargarListaPeriodo(periodos) {
    var selectPeriodo = $('#periodoSelect');
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

function cargarEliminarProyecto() {
    var codi = $("#txtCodPlanTransmision").val();
    $.ajax({
        type: 'POST',
        url: controlador + 'EliminarProyecto',
        data: {
            id: idEliminacion,
            idPlan: codi
        },
        dataType: 'json',
        success: function (response) {
            console.log(idEliminacion);
            console.log(response);
            idEliminacion = 0;
            if (response.result == 1) {
                mostrarMensaje('mensaje', 'exito', 'El Proyecto ha sido eliminado correctamente.');
                ObtenerListadoProyecto(codi,"false");
            } else {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        },
        error: function () {
            console.log(idEliminacion);
            idEliminacion = 0;
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });


    $(`#popupEliminarProyecto`).bPopup().close();

}


function popupEliminarProy(id) {
    idEliminacion = id
    $('#popupEliminarProyecto').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
}

function popupEnviarProyectos(){
    $('#popupEnviarProyecto').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
}

function enviarProyectos() {

    var param = {};
    var codPlanTransmision = $("#txtCodPlanTransmision").val();
    var comentarios = $("#txtComentarioCorreo").val();
    param.CodPlanTransmision = codPlanTransmision;
    param.Comentarios = comentarios;
    $.ajax({
        type: 'POST',
        url: controlador + 'EnviarCorreoNotificacion',
        data: param,
        
        success: function (result) {
            if(result == 1){
                popupClose('popupEnviarProyecto')
                document.location.href = controlador;
            } else {
                mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error.');
        }
    });
}
function popupClose(id) {
    $(`#${id}`).bPopup().close();
}
function scrollToTop() {
    const modalContent = document.getElementById('contenidoProyecto');
    modalContent.scrollTo({ top: 0, behavior: 'smooth' });
}

function ObtenerListadoProyectoAbsolver(codigoPlan) {
    $("#listadoProyectoAbsolver").html('');
    console.log("entro a la tramsision");
    $.ajax({
        type: 'POST',
        url: controlador + 'ListadoProyectoAbsolver',
        data: {
            id: codigoPlan,
        },
        success: function (eData) {
            console.log('data', eData);
            $('#listadoProyectoAbsolver').css("width", $('.form-main').width() + "px");
            $('#listadoProyectoAbsolver').html(eData);
            $('#tablaProyecto').dataTable({
                bJQueryUI: true,
                "scrollY": 440,
                "scrollX": false,
                "sDom": 'ft',
                "ordering": true,
                "order": [[0, 'desc']],
                "iDisplayLength": -1
            });
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

absolverProyecto = function (id) {

    $.ajax({
        type: 'POST',
        url: controlador + 'AbsolverProyecto',
        data: {
            id: id
        },
        
        success: function (evt) {
            $('#contenidoProyecto').html(evt);

            $("#modal1").modal({
                escapeClose: false,
                clickClose: false,
                showClose: true
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

function popupEnviarRespuestas(){
    $('#popupEnviarRespuesta').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
}

function enviarRespuestas() {

    var param = {};
    var codPlanTransmision = $("#txtCodPlanTransmision").val();
    $.ajax({
        type: 'POST',
        url: controlador + 'ClonarPlanTransmision',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: codPlanTransmision }),
        success: function (eData) {
            if (eData.dataPlant != null) {
                var planT = eData.dataPlant;
                planTransEncontrado = planT;
                var idN = planT.Plancodi;
                param.CodPlanTransmision = codPlanTransmision;
                param.CodPlanTransmisionN = idN;
                $.ajax({
                    type: 'POST',
                    url: controlador + 'EnviarCorreoNotificacionAbsolucion',
                    data: param,
                    
                    success: function (result) {
                        if(result == 1){
                            popupClose('popupEnviarRespuesta')
                            window.location.href = controlador +"envio?id=" + idN + "&consult=true";
                        } else {
                            mostrarMensaje('mensajeFicha', 'error', 'Error no se pudo enviar la absolución de la observación.');
                        }
                    },
                    error: function () {
                        mostrarMensaje('mensajeFicha', 'error', 'Error no se pudo enviar la absolución de la observación.');
                    }
                });
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}