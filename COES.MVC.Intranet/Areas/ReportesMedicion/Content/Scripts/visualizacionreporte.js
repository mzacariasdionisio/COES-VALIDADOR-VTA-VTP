var controlador = siteRoot + 'reportesMedicion/formatoReporte/';
var hfCheckEmpresa = 0;
var hfCheckTipoEquipo = 0;
var hfCheckEquipo = 0;
var hfCheckTipoMedida = 0;

$(function () {
    inicializarPagina();

    $('#FechaDesde').Zebra_DatePicker({

    });

    $('#FechaHasta').Zebra_DatePicker({

    });
    $('#btnConsultar').click(function () {
        buscarReporte();
    });

    $('#btnExportar').click(function () {

    });

    listarEmpresa();
});

function inicializarPagina() {
    hfCheckEmpresa = $('#hfCheckEmpresa').val();
    hfCheckEquipo = $('#hfCheckEquipo').val();
    hfCheckTipoEquipo = $('#hfCheckTipoEquipo').val();
    hfCheckTipoMedida = $('#hfCheckTipoMedida').val();

    if (hfCheckEmpresa == 1 || hfCheckEquipo == 1 || hfCheckTipoEquipo == 1 || hfCheckTipoMedida == 1) {
        $('#div_filtro').css("display", "table-row");
    } else {
        $('#div_filtro').css("display", "none");
    }


    if (hfCheckEmpresa == 1) {
        $('.filtro_empresa').css("display", "table-cell");
    } else {
        $('.filtro_empresa').css("display", "none");
    }


    if (hfCheckTipoEquipo == 1) {
        $('.filtro_tipo_equipo').css("display", "table-cell");
    } else {
        $('.filtro_tipo_equipo').css("display", "none");
    }


    if (hfCheckEquipo == 1) {
        $('.filtro_equipo').css("display", "table-cell");
    } else {
        $('.filtro_equipo').css("display", "none");
    }


    if (hfCheckTipoMedida == 1) {
        $('.filtro_tipo_medida').css("display", "table-cell");
    } else {
        $('.filtro_tipo_medida').css("display", "none");
    }

    $('#paginado').hide();
}

function buscarReporte() {
    $("#listado").html('');
    $("#paginado").html('');
    if (esFiltroValido()) {
        pintarPaginado();
        pintarBusqueda(1);
    }
}

function pintarPaginado() {
    var reporcodi = getReporte();

    $.ajax({
        type: 'POST',
        url: controlador + "PaginadoVisualizacion",
        data: {
            reporcodi: reporcodi,
            fechaInicial: $('#FechaDesde').val(),
            fechaFinal: $('#FechaHasta').val(),
        },
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function pintarBusqueda(pagina) {
    inicializarPagina();
    var reporte = getReporte();
    var empresas = getEmpresa();
    var tipoEquipo = getTipoEquipo();
    var equipo = getEquipo();
    var tipoMedida = getTipoMedida();

    $.ajax({
        type: 'POST',
        url: controlador + "ListaVisualizacion",
        data: {
            reporcodi: reporte,
            empresas: empresas,
            tipoEquipo: tipoEquipo,
            equipo: equipo,
            tipoMedida: tipoMedida,
            fechaInicial: $('#FechaDesde').val(),
            fechaFinal: $('#FechaHasta').val(),
            nroPagina: pagina
        },
        success: function (evt) {
            $('#listado').html("");
            $('#paginado').hide();

            if (evt.StrResultado != "-1") {
                $('#listado').css("width", $('#mainLayout').width() + "px");
                $('#listado').html(evt.StrResultado);

                $('#tabla').dataTable({
                    //bJQueryUI: true,
                    "bAutoWidth": false,
                    "bSort": false,
                    "scrollY": "auto",
                    "scrollX": true,
                    "sDom": 't',
                    "iDisplayLength": 24
                });
                $('#paginado').show();
            } else {
                $('#listado').html('Ha ocurrido un error en la generación del reporte.');
                console.log(evt.StrError);
            }
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

//////////////////////////////////
/// Util
//////////////////////////////////

function listarEmpresa() {
    var reporcodi = getReporte();

    $.ajax({
        type: 'POST',
        url: controlador + "CargarListaEmpresa",
        data: {
            reporcodi: reporcodi
        },
        global: false,
        success: function (evt) {
            if (hfCheckEmpresa == 1) {
                $('.filtro_empresa').css("display", "table-cell");
            } else {
                $('.filtro_empresa').css("display", "none");
            }

            $('#cbEmpresa').empty();
            var listaEmpresa = evt.ListaEmpresa;
            if (listaEmpresa.length == 0) {
                $('#cbEmpresa').append('<option value="0">No existen registros...</option>');
            } else {
                for (var i = 0; i < listaEmpresa.length; i++) {
                    $('#cbEmpresa').append('<option value=' + listaEmpresa[i].Emprcodi + '>' + listaEmpresa[i].Emprnomb + '</option>');
                }
            }

            $('#cbEmpresa').multipleSelect({
                width: '150px',
                filter: true,
                onClose: function (view) {
                    listarTipoEquipo();
                }
            });

            $('#cbEmpresa').multipleSelect('checkAll');
            listarTipoEquipo();
        },
        error: function () {
            alert("Error al mostrar lista de empresas");
        }
    });
};

function listarTipoEquipo() {
    var reporcodi = getReporte();
    var emprcodi = getEmpresa();

    $.ajax({
        type: 'POST',
        url: controlador + "CargarListaTipoEquipo",
        data: {
            reporcodi: reporcodi,
            emprcodi: emprcodi
        },
        global: false,
        success: function (evt) {
            if (hfCheckTipoEquipo == 1) {
                $('.filtro_tipo_equipo').css("display", "table-cell");
            } else {
                $('.filtro_tipo_equipo').css("display", "none");
            }

            $('#cbTipoEquipo').empty();
            var listaTipoEquipo = evt.ListaFamilia;
            if (listaTipoEquipo.length == 0) {
                $('#cbTipoEquipo').append('<option value="0">No existen registros...</option>');
            } else {
                for (var i = 0; i < listaTipoEquipo.length; i++) {
                    $('#cbTipoEquipo').append('<option value=' + listaTipoEquipo[i].Famcodi + '>' + listaTipoEquipo[i].Famnomb + '</option>');
                }
            }

            $('#cbTipoEquipo').multipleSelect({
                width: '150px',
                filter: true,
                onClose: function (view) {
                    listarEquipo();
                }
            });

            $('#cbTipoEquipo').multipleSelect('checkAll');
            listarEquipo();
        },
        error: function () {
            alert("Error al mostrar lista de tipo de equipos");
        }
    });
}

function listarEquipo() {
    var reporcodi = getReporte();
    var emprcodi = getEmpresa();
    var famcodi = getTipoEquipo();

    $.ajax({
        type: 'POST',
        url: controlador + "CargarListaEquipo",
        data: {
            reporcodi: reporcodi,
            emprcodi: emprcodi,
            famcodi: famcodi
        },
        global: false,
        success: function (evt) {
            if (hfCheckEquipo == 1) {
                $('.filtro_equipo').css("display", "table-cell");
            } else {
                $('.filtro_equipo').css("display", "none");
            }

            $('#cbEquipo').empty();
            var listaEquipo = evt.ListaEquipo;
            if (listaEquipo.length == 0) {
                $('#cbEquipo').append('<option value="0">No existen registros...</option>');
            } else {
                for (var i = 0; i < listaEquipo.length; i++) {
                    $('#cbEquipo').append('<option value=' + listaEquipo[i].Equicodi + '>' + listaEquipo[i].Equinomb + '</option>');
                }
            }

            $('#cbEquipo').multipleSelect({
                width: '150px',
                filter: true,
                onClose: function (view) {
                    listarTipoInformacion();
                }
            });

            $('#cbEquipo').multipleSelect('checkAll');
            listarTipoInformacion();
        },
        error: function () {
            alert("Error al mostrar lista de equipos");
        }
    });
}

function listarTipoInformacion() {
    var reporcodi = getReporte();
    var emprcodi = getEmpresa();
    var famcodi = getTipoEquipo();
    var equicodi = getEquipo();

    $.ajax({
        type: 'POST',
        url: controlador + "CargarListaTipoInformacion",
        data: {
            reporcodi: reporcodi,
            emprcodi: emprcodi,
            famcodi: famcodi,
            equicodi: equicodi
        },
        global: false,
        success: function (evt) {
            $('#cbTipoInformacion').empty();
            var listaMedidas = evt.ListaMedidas;
            if (listaMedidas.length == 0) {
                $('#cbTipoInformacion').append('<option value="0">No existen registros...</option>');
            } else {
                for (var i = 0; i < listaMedidas.length; i++) {
                    $('#cbTipoInformacion').append('<option value=' + listaMedidas[i].Tipoinfocodi + '>' + listaMedidas[i].Tipoinfoabrev + '</option>');
                }
            }

            $('#cbTipoInformacion').multipleSelect({
                width: '100px',
                filter: true,
                onClose: function (view) {
                    buscarReporte();
                }
            });

            $('#cbTipoInformacion').multipleSelect('checkAll');

            if (hfCheckTipoMedida == 1) {
                $('.filtro_tipo_medida').css("display", "table-cell");
            } else {
                $('.filtro_tipo_medida').css("display", "none");
            }
            buscarReporte();
        },
        error: function () {
            alert("Error al mostrar lista de tipo de información");
        }
    });
}

///////////////////////

function getReporte() {
    return $('#hfReporte').val();
}
function getEmpresa() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    $('#hfEmpresa').val(empresa);
    var idEmpresa = $('#hfEmpresa').val();

    return idEmpresa;
}
function getTipoEquipo() {
    var obj = $('#cbTipoEquipo').multipleSelect('getSelects');
    if (obj == "[object Object]") obj = "-1";
    $('#hfTipoEquipo').val(obj);
    var idobj = $('#hfTipoEquipo').val();

    return idobj;
}
function getEquipo() {
    var obj = $('#cbEquipo').multipleSelect('getSelects');
    if (obj == "[object Object]") obj = "-1";
    $('#hfTipoEquipo').val(obj);
    var idobj = $('#hfTipoEquipo').val();

    return idobj;
}
function getTipoMedida() {
    var obj = $('#cbTipoInformacion').multipleSelect('getSelects');
    if (obj == "[object Object]") obj = "-1";
    $('#hfTipoMedida').val(obj);
    var idobj = $('#hfTipoMedida').val();

    return idobj;
}

function esFiltroValido() {
    var empresas = getEmpresa();
    var tipoEquipo = getTipoEquipo();
    var equipo = getEquipo();
    var tipoMedida = getTipoMedida();

    if (hfCheckEmpresa == 1) {
        if (empresas == "") {
            return false;
        }
    }
    if (hfCheckTipoEquipo == 1) {
        if (tipoEquipo == "") {
            return false;
        }
    }
    if (hfCheckEquipo == 1) {
        if (equipo == "") {
            return false;
        }
    }
    if (hfCheckTipoMedida == 1) {
        if (tipoMedida == "") {
            return false;
        }
    }

    return true;
}