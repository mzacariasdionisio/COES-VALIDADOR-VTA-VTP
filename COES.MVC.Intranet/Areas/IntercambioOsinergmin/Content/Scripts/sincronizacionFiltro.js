$( /**
   *Llamadas iniciales 
   * @returns {} 
   */
    function () {
        $("#submit").click(filtroButtonClick);
        $('#btnExportar').click(function () {
            expotarExcel();
        });

        //- HDT Inicio 29/06/2017
        $('#btnExportarDatosOsi').click(function () {
            expotarExcelDatosOsi();
        });
        //- HDT Fin


        $('.rbTipo').click(function () {
            mostrarTipo();
        });
    });

var radio;
var entAsignar;
var codAsignar;

mostrarTipo = function () {

    var tipo = $('input[name=rbTipo]:checked').val();

    if (tipo == "P") {
        radio = "P";
    }

    if (tipo == "L") {
        radio = "L";
    }

}

var filtroButtonClick = function () {

    pintarBusqueda(1);
    pintarPaginado();
}

function expotarExcel() {

    var entidad = getEntidadSeleccionada();
    var filtroActual = getParametroFiltroActual();
    $.ajax({
        type: "POST",
        url: controlador + "exportar",
        data: {
            entidad: entidad,
            filtroActual: filtroActual
        },
        dataType: "json",
        success: function (result) {
            if (result !== -1) {
                document.location.href = controlador + "descargar?file=" + result;
            }
            else {
                mostrarMensaje("mensaje", $mensajeErrorGeneral, $tipoMensajeError, $modoMensajeCuadro);
            }
        },
        error: function () {
            mostrarMensaje("mensaje", $mensajeErrorGeneral, $tipoMensajeError, $modoMensajeCuadro);
        }
    });

}

//- HDT Inicio 29/06/2017
function expotarExcelDatosOsi() {

    var entidad = getEntidadSeleccionada();

    $.ajax({
        type: "POST",
        url: controlador + "exportarDatosOsi",
        data: {
            entidad: entidad
        },
        dataType: "json",
        success: function (result) {
            if (result !== -1) {
                document.location.href = controlador + "descargarMaestroOsinergmin?archivo=" + result;
            }
            else {
                mostrarMensaje("mensaje", $mensajeErrorGeneral, $tipoMensajeError, $modoMensajeCuadro);
            }
        },
        error: function () {
            mostrarMensaje("mensaje", $mensajeErrorGeneral, $tipoMensajeError, $modoMensajeCuadro);
        }
    });

}

var exportarButtonClick = function () {


};

var getArbol =
    /**
    * Obtiene la estructura del arbol y lo pinta
    * @returns {} 
    */
    function () {
        $.ajax({
            type: "POST",
            url: controlador + "getarbol",
            success: function (evt) {
                $("#arbol").html(evt);
            },
            error: function () {
                mostrarMensaje("mensaje", $mensajeErrorGeneral, $tipoMensajeError, $modoMensajeCuadro);
            }
        });
    };

var getFiltro =
    /**
    * Obtiene el html del filtro según la entidad seleccionada en el arbol
    * @returns {} 
    */
    function () {

        var entidad = getEntidadSeleccionada();
        var filtroActual = getParametroFiltroActual();

        $.ajax({
            type: "POST",
            url: controlador + "getfiltro",
            data: {
                entidad: entidad,
                filtroActual: filtroActual
            },
            success: function (evt) {
                $("#filtro").html(evt);
            },
            error: function () {
                mostrarMensaje("mensaje", $mensajeErrorGeneral, $tipoMensajeError, $modoMensajeCuadro);
            }
        });
    };

var getListado =
    /**
    * Obtiene el listado y pinta la grilla y su paginación
    * @returns {} 
    */
    function () {
        pintarPaginado();
        pintarBusqueda(1);
    };

var pintarPaginado =
    /**
    * Obtiene la paginación según la entidad y filtro
    * @returns {} 
    */
    function () {

        var entidad = getEntidadSeleccionada();
        var filtro = getParametroFiltroActual();

        $.ajax({
            type: "POST",
            url: controlador + "paginado",
            data: {
                entidad: entidad,
                filtro: filtro
            },
            success: function (evt) {
                $("#paginado").html(evt);
                mostrarPaginado();
            },
            error: function () {
                mostrarMensaje("mensaje", $mensajeErrorGeneral, $tipoMensajeError, $modoMensajeCuadro);
            }
        });
    };

var pintarBusqueda =
    /**
    * Obtiene el listado según la página actual, la entidad actual y el filtro
    * el resultado lo pinta en una grilla
    * @param {} nroPagina : Número de página
    * @returns {} 
    */
    function (nroPagina) {

        var entidad = getEntidadSeleccionada();
        var filtro = getParametroFiltroActual();

        var entidadCombo = "";

        //alert($("#hfEnt").val());

        if ($("#hfEnt").val() == "Entidad: Resultado")
        {
            entidadCombo = $("#entCodi").val();
        }

        $.ajax({
            type: "POST",
            url: controlador + "listregistrosentidad",
            data: {
                entidad: entidad,
                filtro: filtro,
                nroPagina: nroPagina,
                combo: entidadCombo,
                radio: radio
            },
            success: function (evt) {
                $("#listado").html(evt);
                $("#tabla").dataTable({
                    "scrollY": 314,
                    "scrollX": false,
                    "sDom": "t",
                    "ordering": false,
                    "bDestroy": true,
                    "bPaginate": false,
                    "iDisplayLength": 50
                });
            },
            error: function () {
                mostrarMensaje("mensaje", $mensajeErrorGeneral, $tipoMensajeError, $modoMensajeCuadro);
            }
        });
    };

var getEntidadSeleccionada =
    /**
    * Obtiene el valor del campo codigo entidad, o un valor por defecto
    * @returns {} : Valor del campo
    */
    function () {
        var entidad;
        if ($("#CodigoEntidad").length) {
            entidad = $("#CodigoEntidad").val();
        } else {
            entidad = codigoEntidadPorDefecto;
        }
        return entidad;
    };

var getParametroFiltroActual =
    /**
    * Obtiene el valor del campo NombreEntidad o EntidadEnum, según el filtro activo
    * @returns {} : Valor del campo
    */
    function () {
        var filtroActual;
        if ($("#NombreEntidad").length) {
            filtroActual = $("#NombreEntidad").val() !== null ? $("#NombreEntidad").val() : "";
        } else {
            if ($("#EntidadEnum").length) {
                if ($("#EntidadEnum").val() !== null) {
                    filtroActual = $("#EntidadEnum").val();
                } else {
                    filtroActual = codigoEntidadPorDefecto.toString();
                }
            } else {
                filtroActual = "";
            }
        }
        return filtroActual;
    };

var asignar = function (ent, cod, desc, selected) {

    entAsignar = ent;
    codAsignar = cod;

    $('#entidades').html('');
    $("#lblCodigo").val(cod);
    $("#lblDescripcion").val(desc);
    $("#lblEntidad").text(ent);

    if (ent != "Barra") {
        document.getElementById('textBarra').style.display = 'none';
        document.getElementById('entidades').style.display = '';
        $.ajax({
            type: 'POST',
            url: controlador + 'ObtenerListaEntidad',
            data: {
                entidad: ent
            },
            async: true,
            dataType: 'json',
            success: function (data) {
                var jsonData = JSON.parse(data);

                var $select = $('#entidades');
                $(jsonData).each(function (index, o) {
                    var $option = $("<option/>").attr("value", o.id).text(o.name);
                    $select.append($option);
                });

                $("#entidades").val(selected);

                setTimeout(function () {
                    $('#popupEdicion').bPopup({
                        autoClose: false
                    });
                }, 50);
            },
            error: function () {
                mostrarMensaje('mensaje', 'Ha ocurrido un error al obtener la lista de Entidades.', $tipoMensajeError, $modoMensajeCuadro);
            }
        });
    }
    else {
        document.getElementById('entidades').style.display = 'none';
        document.getElementById('textBarra').style.display = '';

        $("#textBarra").val(selected);

        setTimeout(function () {
            $('#popupEdicion').bPopup({
                autoClose: false
            });
        }, 50);
    }

}

$('#btnGrabarEditor').click(function () {

    var codigo = "";
    document.getElementById('mensaje').style.display = '';
    if (entAsignar != "Barra") {
        codigo = $("#entidades").val();
    } else {
        codigo = $("#textBarra").val();
    }

    $.ajax({
        type: "POST",
        url: controlador + "GrabarHomologacion",
        data: {
            entidad: entAsignar,
            codigoCoes: codAsignar,
            codigoOsinergmin: codigo
        },
        dataType: "json",
        success: function (result) {
            if (result == 1) {
                mostrarExito("Registro guardado con éxito.");
                $('#popupEdicion').bPopup().close();
                pintarBusqueda(1);
                pintarPaginado();
            }
            else {
                mostrarError("Ocurrio un error al momento de guardar la información.");
            }
        },
        error: function () {
            mostrarMensaje("mensaje", $mensajeErrorGeneral, $tipoMensajeError, $modoMensajeCuadro);
        }
    });

    $('#popupEdicion').bPopup().close();

});

function quitarAsignacion(ent, codigo, codigoOsinergmin) {
    document.getElementById('mensaje').style.display = '';
    if (confirm("¿Desea quitar la homologación?")) {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: controlador + "quitarAsignacion",
            data: {
                entidad: ent,
                codigo: codigo,
                codigoOsinergmin: codigoOsinergmin
            },
            success: function (evt) {
                mostrarExito("Homologación desecha correctamente.");
                pintarBusqueda(1);
                pintarPaginado();
            },
            error: function () {
                mostrarError();
            }
        });
    }

}

function mostrarError(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-error');
}

function mostrarExito(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-exito');
}