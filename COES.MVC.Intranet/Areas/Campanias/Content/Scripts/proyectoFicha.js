var controlador = siteRoot + 'campanias/plantransmision/';
var controladorFichas = siteRoot + 'campanias/fichasproyecto/';

$(function () {
    ObtenerListadoProyectoFicha();
    $('#btnAgregar').on('click', function () {
        agregarP();
    });
    $('#btnEnviar').on('click', function () {
        enviar();
    });
    cargarEmpresa();
    cargarPeriodo();
});

function ObtenerListadoProyectoFicha() {
    $("#listadoProyectoFicha").html('');

    $.ajax({
        type: 'POST',
        url: controlador + 'ListadoProyectoFicha',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(''),
        success: function (eData) {
            $('#listadoProyectoFicha').css("width", $('.form-main').width() + "px");
            $('#listadoProyectoFicha').html(eData);
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

function ObtenerContenidoFicha(tipoProy) {
    $("#containerFicha").html('');

    $.ajax({
        type: 'POST',
        url: controladorFichas + tipoProy,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(''),
        success: function (eData) {
            $('#containerFicha').html(eData);
            $('#tab-container-ficha').easytabs({
                animate: false,
            });
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

agregar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "Proyecto",
        success: function (evt) {
            $('#popup').html(evt);
            $('#tab-container').easytabs();
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
agregarP = function (id, fuente, tipo) {

    $.ajax({
        type: 'POST',
        url: controlador + 'proyecto',
        data: {
            id: 'id'
        },
        global: false,
        success: function (evt) {
            $('#contenidoProyecto').html(evt);

            $('#popupProyecto').bPopup({
                autoClose: false,
            });

            $('#btnCancelar').on("click", function () {
                $('#popupEdicion').bPopup().close();
            });
            ObtenerContenidoFicha('G7_Lineas');
            $('#cbSubTipoProyecto').change(function () {
                $('#tab1').html($("#cbSubTipoProyecto").val());
                ObtenerContenidoFicha($("#cbSubTipoProyecto").val())
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
    $.ajax({
        type: 'POST',
        url: controlador + 'ListarEmpresas',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(''),
        success: function (eData) {
            cargarListaEmpresas(eData);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
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