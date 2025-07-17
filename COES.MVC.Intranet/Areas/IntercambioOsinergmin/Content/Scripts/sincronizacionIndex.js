var controlador = siteRoot + "IntercambioOsinergmin/maestros/";
var codigoEntidadPorDefecto = 1;   // 0 = Código del enum de entidad empresa
var radio;
$( /**
   *Llamadas iniciales 
   * @returns {} 
   */
    function() {
        getArbol();
        getFiltro();
        getListado();
        $("#submit").click(filtroButtonClick); //on("click", filtroButtonClick());
        //$("#exportarExcel").click(exportarButtonClick); //on("click", exportarButtonClick());

        $('#btnExportar').click(function () {
            expotarExcel();
        });

        $('.rbTipo').click(function () {
            mostrarTipo();
        });

    });

mostrarTipo = function () {

    var tipo = $('input[name=rbTipo]:checked').val();

    if (tipo == "P") {
        radio = "P";
    }

    if (tipo == "L") {
        radio = "L";
    }

    alert("OKKAAA");

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


var exportarButtonClick = function () {

    
};

var getArbol =
    /**
    * Obtiene la estructura del arbol y lo pinta
    * @returns {} 
    */
    function() {
        $.ajax({
            type: "POST",
            url: controlador + "getarbol",
            success: function(evt) {
                $("#arbol").html(evt);
            },
            error: function() {
                mostrarMensaje("mensaje", $mensajeErrorGeneral, $tipoMensajeError, $modoMensajeCuadro);
            }
        });
    };

var getFiltro =
    /**
    * Obtiene el html del filtro según la entidad seleccionada en el arbol
    * @returns {} 
    */
    function() {

        var entidad = getEntidadSeleccionada();
        var filtroActual = getParametroFiltroActual();

        $.ajax({
            type: "POST",
            url: controlador + "getfiltro",
            data: {
                entidad: entidad,
                filtroActual: filtroActual
            },
            success: function(evt) {
                $("#filtro").html(evt);
            },
            error: function() {
                mostrarMensaje("mensaje", $mensajeErrorGeneral, $tipoMensajeError, $modoMensajeCuadro);
            }
        });
    };

var getListado =
    /**
    * Obtiene el listado y pinta la grilla y su paginación
    * @returns {} 
    */
    function() {
        pintarPaginado();
        pintarBusqueda(1);
    };

var pintarPaginado =
    /**
    * Obtiene la paginación según la entidad y filtro
    * @returns {} 
    */
    function() {

        var entidad = getEntidadSeleccionada();
        var filtro = getParametroFiltroActual();

        $.ajax({
            type: "POST",
            url: controlador + "paginado",
            data: {
                entidad: entidad,
                filtro: filtro
            },
            success: function(evt) {
                $("#paginado").html(evt);
                mostrarPaginado();
            },
            error: function() {
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
    function(nroPagina) {

        var entidad = getEntidadSeleccionada();
        var filtro = getParametroFiltroActual();

        var entidadCombo = "";

        //alert($("#entidadLab").val());

        //if ($("#entidadLab").val() = "Entidad: Resultado")
        //{
        //    entidadCombo = $("#entCodi").val();
        //}


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
            success: function(evt) {
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
            error: function() {
                mostrarMensaje("mensaje", $mensajeErrorGeneral, $tipoMensajeError, $modoMensajeCuadro);
            }
        });
    };

var getEntidadSeleccionada =
    /**
    * Obtiene el valor del campo codigo entidad, o un valor por defecto
    * @returns {} : Valor del campo
    */
    function() {
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
    function() {
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



var editarRegistro = function (id) {

    var entidad = getEntidadSeleccionada();

    $.ajax({
        type: "POST",
        data: {
            id: id,
            codigoEntidad: entidad
        },
        url: controlador + "Registro",
        success: function (evt) {
            $("#contenidoEdicion").html(evt);
            setTimeout(function () {
                $("#popupEdicion").bPopup({
                    autoClose: false
                });
            }, 50);
            if ($("#asignar").length) {
                $("#asignar").click(asignarRegistro);
            }
            if ($("#cancelar").length) {
                $("#cancelar").click(function () {
                    $("#popupEdicion").bPopup().close();
                });
            }
        },
        error: function () {
            mostrarMensaje("mensaje", $mensajeErrorGeneral, $tipoMensajeError, $modoMensajeCuadro);
        }
    });
};

var asignarRegistro = function () {

    var form = $("#asignarForm");
    var entidad = getEntidadSeleccionada();

    $.ajax({
        type: "POST",
        data: //form.serialize(),
            {
                __RequestVerificationToken: $('input[name="__RequestVerificationToken"]', form).val(),
                Codigo: $('input[name="__RequestVerificationToken"]', form).val(),
                Descripcion: $('input[name="Descripcion"]', form).val(),
                CodigoOsinergmin: $('input[name="CodigoOsinergmin"]', form).val(),
                EntidadDescripcion: $('input[name="EntidadDescripcion"]', form).val(),
                EntidadCodigo: $('input[name="EntidadCodigo"]', form).val(),
                entidadSeleccionada: entidad
            },
        url: controlador + "Asignar",
        success: function (evt) {
            if (evt === 1) {
                mostrarMensaje("mensaje", $mensajeExitoGeneral, $tipoMensajeError, $modoMensajeCuadro);
                $("#popupEdicion").bPopup().close();
                getListado();
            } else {
                mostrarMensaje("mensajeRegistro", $mensajeErrorGeneral, $tipoMensajeError, $modoMensajeCuadro);
            }
        },
        error: function () {
            mostrarMensaje("mensajeRegistro", $mensajeErrorGeneral, $tipoMensajeError, $modoMensajeCuadro);
        }
    });
}