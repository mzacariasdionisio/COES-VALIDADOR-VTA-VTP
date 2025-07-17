var controlador = siteRoot + 'IND/Configuracion/';
var ancho = 1100;

$(function () {
    $('#cntMenu').css("display", "none");

    $('#cbFamilia').change(function () {
        cargarFiltroEmpresa();
    });

    $('#cbAnio').change(function () {
        listadoPeriodo();
    });

    $('#cbPeriodo').change(function () {
        cargarFiltroEmpresa();
    });

    //registro
    $('#btnNuevo').bind('click', function (e) {
        inicializarPopup();
    });
    $('#btnGuardarForm').bind('click', function (e) {
        guardarEquivalencia();
    });
    $('#btnLimpiar').bind('click', function (e) {
        limpiarForm();
    });

    //editar
    $('#btnEditForm').bind('click', function (e) {
        guardarEdicionEquivalencia();
    });


    mostrarListado();
});

///////////////////////////////////////////////////////////////////
/// listado

function mostrarListado() {
    $('#listado').html('');
    ancho = $("#mainLayout").width() > 1100 ? $("#mainLayout").width() - 30 : 1100;

    $.ajax({
        type: "POST",
        async: false,
        url: controlador + "CargarListadoEquivalencia",
        data: {
        },
    }).done(function (evt) {
        if (evt.Resultado != "-1") {
            $('#listado').html(evt.Resultado);

            $('#miTabla').dataTable({
                "sPaginationType": "full_numbers",
                "destroy": "true",
                "ordering": false,
                "searching": true,
                "iDisplayLength": 15,
                "info": false,
                "paging": false,
                "scrollX": true,
                "scrollY": "400px"
            });
        } else {
            alert("Ha ocurrido un error: " + evt.Mensaje);
        }

    }).fail(function (jqXHR, textStatus, errorThrown) {
        alert("Ha ocurrido un error");
    });
}

function cargarFiltroEmpresa() {
    $("#txtCentral").val('');
    $("#txtUnidad").val('');

    $('#cboEmpresa').unbind();
    $("#cboEmpresa").empty();
    $('#cboEmpresa').get(0).options[$('#cboEmpresa').get(0).options.length] = new Option("--SELECCIONE--", 0);

    $('#cboCentral').unbind();
    $("#cboCentral").empty();
    $('#cboCentral').get(0).options[$('#cboCentral').get(0).options.length] = new Option("--SELECCIONE--", 0);

    $('#cboUnidad').unbind();
    $("#cboUnidad").empty();
    $('#cboUnidad').get(0).options[$('#cboUnidad').get(0).options.length] = new Option("--SELECCIONE--", 0);

    var obj = getObjJson();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarFiltros',
        dataType: 'json',
        data: {
            famcodi: obj.Famcodi,
            ipericodi: obj.Ipericodi,
            emprcodi: obj.Emprcodi,
            equipadre: obj.Equipadre
        },
        cache: false,
        success: function (data) {
            if (data.Resultado != "-1") {
                if (data.ListaEmpresa.length > 0) {
                    $.each(data.ListaEmpresa, function (i, item) {
                        $('#cboEmpresa').get(0).options[$('#cboEmpresa').get(0).options.length] = new Option(item.Emprnomb, item.Emprcodi);
                    });
                }

                $('#cboEmpresa').unbind();
                $('#cboEmpresa').change(function () {
                    cargarFiltroCentral();
                });
            } else {
                alert('Ha ocurrido un error: ' + data.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function cargarFiltroCentral() {
    $("#txtCentral").val('');
    $("#txtUnidad").val('');

    $('#cboCentral').unbind();
    $("#cboCentral").empty();
    $('#cboCentral').get(0).options[$('#cboCentral').get(0).options.length] = new Option("--SELECCIONE--", 0);

    $('#cboUnidad').unbind();
    $("#cboUnidad").empty();
    $('#cboUnidad').get(0).options[$('#cboUnidad').get(0).options.length] = new Option("--SELECCIONE--", 0);

    var obj = getObjJson();

    if (obj.Emprcodi > 0) {
        $.ajax({
            type: 'POST',
            url: controlador + 'CargarFiltros',
            dataType: 'json',
            data: {
                famcodi: obj.Famcodi,
                ipericodi: obj.Ipericodi,
                emprcodi: obj.Emprcodi,
                equipadre: obj.Equipadre
            },
            cache: false,
            success: function (data) {
                if (data.Resultado != "-1") {
                    if (data.ListaCentral.length > 0) {
                        $.each(data.ListaCentral, function (i, item) {
                            $('#cboCentral').get(0).options[$('#cboCentral').get(0).options.length] = new Option(item.Central, item.Equipadre);
                        });
                    }

                    $('#cboCentral').unbind();
                    $('#cboCentral').change(function () {
                        var central = parseInt($("#cboCentral").val()) || 0;
                        if (central > 0) $("#txtCentral").val($("#cboCentral option:selected").text());
                        else $("#txtCentral").val('');

                        cargarFiltroUnidad();
                    });
                } else {
                    alert('Ha ocurrido un error: ' + data.Mensaje);
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error.');
            }
        });
    }
}

function cargarFiltroUnidad() {
    $("#txtUnidad").val('');

    $('#cboUnidad').unbind();
    $("#cboUnidad").empty();
    $('#cboUnidad').get(0).options[$('#cboUnidad').get(0).options.length] = new Option("--SELECCIONE--", 0);

    var obj = getObjJson();

    if (obj.Emprcodi > 0 && obj.Equipadre > 0) {
        $.ajax({
            type: 'POST',
            url: controlador + 'CargarFiltros',
            dataType: 'json',
            data: {
                famcodi: obj.Famcodi,
                ipericodi: obj.Ipericodi,
                emprcodi: obj.Emprcodi,
                equipadre: obj.Equipadre
            },
            cache: false,
            success: function (data) {
                if (data.Resultado != "-1") {
                    if (data.ListaEquipoFiltro.length > 0) {
                        $.each(data.ListaEquipoFiltro, function (i, item) {
                            $('#cboUnidad').get(0).options[$('#cboUnidad').get(0).options.length] = new Option(item.UnidadnombPR25, item.Equicodi + "#" + item.Grupocodi);
                        });

                        $('#cboUnidad').unbind();
                        $('#cboUnidad').change(function () {
                            if (obj.Famcodi == 4) {
                                var codigoeq = parseInt($("#cboUnidad").val()) || 0;
                                if (codigoeq > 0) $("#txtUnidad").val($("#cboUnidad option:selected").text());
                                else $("#txtUnidad").val('');
                            } else {
                                $("#txtUnidad").val('');
                            }
                        });
                    }

                } else {
                    alert('Ha ocurrido un error: ' + data.Mensaje);
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error.');
            }
        });
    }
}

function listadoPeriodo() {

    var anio = parseInt($("#cbAnio").val()) || 0;

    $("#cbPeriodo").empty();

    $.ajax({
        type: 'POST',
        url: controlador + "PeriodoListado",
        data: {
            anio: anio,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                if (evt.ListaPeriodo.length > 0) {
                    $.each(evt.ListaPeriodo, function (i, item) {
                        $('#cbPeriodo').get(0).options[$('#cbPeriodo').get(0).options.length] = new Option(item.Iperinombre, item.Ipericodi);
                    });
                } else {
                    $('#cbPeriodo').get(0).options[0] = new Option("--", "0");
                }

                mostrarListado();
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

///////////////////////////////////////////////////////////////////
/// Formulario Gaseoducto

function inicializarPopup() {
    $('#cboEmpresa').unbind();
    $('#cboEmpresa').change(function () {
        cargarFiltroCentral();
    });

    $('#popupEquivalencia').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown',
        transitionClose: 'slideUp',
        modalClose: false
    });
}

function limpiarForm() {
    $("#txtCentral").val('');
    $("#txtUnidad").val('');

    $("#cboUnidad").unbind();
    $("#cboCentral").unbind();

    $("#cboCentral").empty();
    $('#cboCentral').get(0).options[$('#cboCentral').get(0).options.length] = new Option("--SELECCIONE--", 0);

    $("#cboUnidad").empty();
    $('#cboUnidad').get(0).options[$('#cboUnidad').get(0).options.length] = new Option("--SELECCIONE--", 0);

    $("#cboEmpresa").val(0);
}

function guardarEquivalencia() {
    var msj = validarForm();
    var obj = getObjJson();

    if (msj == '') {
        $.ajax({
            type: "POST",
            contentType: 'application/json; charset=utf-8',
            traditional: true,
            url: controlador + "GuardarEquivalencia",
            data: JSON.stringify({
                inRegistro: obj
            }),
            cache: false,
            success: function (data) {
                if (data.Resultado != "-1") {
                    alert('El registro se guardó correctamente');
                    //$('#popupEquivalencia').bPopup().close();
                    $('#cboUnidad').val(0);
                    $("#txtUnidad").val('');

                    mostrarListado();

                    //limpiarForm();
                } else {
                    alert('Ha ocurrido un error: ' + data.Mensaje);
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error.');
            }
        });
    } else {
        alert(msj);
    }
}

function editarEquiv(unicodi, central, uniunidadnomb, uninombcentral, uninombunidad) {
    $("#hfUnicodi").val(unicodi);
    $("#txtCentralEditCoes").val(central);
    $("#txtUnidadEditCoes").val(uniunidadnomb);
    $("#txtCentralEdit").val(uninombcentral);
    $("#txtUnidadEdit").val(uninombunidad);

    $('#popupEditar').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown',
        transitionClose: 'slideUp',
        modalClose: false
    });
}

function guardarEdicionEquivalencia() {
    var model = {
        Iunicodi: $("#hfUnicodi").val(),
        Iuninombcentral: $("#txtCentralEdit").val(),
        Iuninombunidad: $("#txtUnidadEdit").val(),
    };

    var mensaje = '';
    if (model.Iuninombcentral == null || model.Iuninombcentral == '') {
        mensaje = mensaje + "<li>Ingrese nombre de la central.</li>";
    }
    if (model.Iuninombunidad == null || model.Iuninombunidad == '') {
        mensaje = mensaje + "<li>Ingrese nombre de la unidad.</li>";
    }

    if (mensaje == '') {
        $.ajax({
            type: "POST",
            contentType: 'application/json; charset=utf-8',
            traditional: true,
            url: controlador + "EditarEquivalencia",
            data: JSON.stringify({
                inRegistro: model
            }),
            cache: false,
            success: function (data) {
                if (data.Resultado != "-1") {
                    alert('La edición se guardó correctamente.');
                    $('#popupEditar').bPopup().close();

                    mostrarListado();

                } else {
                    alert('Ha ocurrido un error: ' + data.Mensaje);
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error.');
            }
        });
    } else {
        alert(mensaje);
    }
}

function eliminarEquiv(id) {

    $.ajax({
        type: "POST",
        url: controlador + "EliminarEquivalencia",
        data: {
            id: id
        },
        cache: false,
        success: function (data) {
            if (data.Resultado != "-1") {
                alert('La eliminación se guardó correctamente.');
                $('#popupEditar').bPopup().close();

                mostrarListado();

            } else {
                alert('Ha ocurrido un error: ' + data.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function validarForm() {
    var mensaje = '';

    var model = getObjJson();

    if (model.Emprcodi <= 0) {
        mensaje = mensaje + "<li>Seleccione empresa.</li>";
    }
    if (model.Equipadre <= 0) {
        mensaje = mensaje + "<li>Seleccione central.</li>";
    }
    if (model.Equicodi == null) {
        mensaje = mensaje + "<li>Seleccione unidad.</li>";
    }
    if (model.Iuninombcentral == null || model.Iuninombcentral == '') {
        mensaje = mensaje + "<li>Ingrese nombre de la central.</li>";
    }
    if (model.Iuninombunidad == null || model.Iuninombunidad == '') {
        mensaje = mensaje + "<li>Ingrese nombre de la unidad.</li>";
    }

    return mensaje;
}

function getObjJson() {
    var obj = {};

    obj.Ipericodi = parseInt($("#cbPeriodo").val()) || 0;
    obj.Famcodi = parseInt($("#cbFamilia").val()) || 0;
    obj.Emprcodi = parseInt($("#cboEmpresa").val()) || 0;
    obj.Equipadre = parseInt($("#cboCentral").val()) || 0;
    obj.Equicodi = null;
    obj.Grupocodi = null;
    obj.Iuninombcentral = $("#txtCentral").val().trim();
    obj.Iuninombunidad = $("#txtUnidad").val().trim();

    obj.Iuniunidadnomb = $("#cboUnidad option:selected").text();

    var equipo = $('#cboUnidad').val();
    if (equipo != '') {
        var listaCodigo = equipo.split('#');
        obj.Equicodi = listaCodigo[0];
        obj.Grupocodi = listaCodigo[1];
    }

    return obj;
}