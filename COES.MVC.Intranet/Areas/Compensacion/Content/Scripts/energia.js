var controlador = siteRoot + "compensacion/general/";

$( /**
   * Llamadas iniciales
   * @returns {} 
   */
   $( document ).ready(function() {
       $("select").change(function () { pintarBusqueda() }).change();
       $('#btnObtener').click(function () {
           obtenerData();
       });

       $('#btnExportar').click(function () {
           exportar();
       });
   }));

function exportar() {
    $.ajax({
        type: 'POST',
        url: controlador + 'exportar',
        data: {
            pecacodi: $("#pecacodi").val(),
            tipo: "EN"
        },
        dataType: 'json',
        success: function (result) {
            if (result != -1) {
                document.location.href = controlador + 'descargar?formato=' + 1 + '&file=' + result
            }
            else {
                mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
        }
    });
}

var pintarBusqueda =
    /**
    * Pinta el listado de periodos según el año seleccionado
    * @returns {} 
    */
    function () {


        $.ajax({
            type: "POST",
            url: controlador + "listarEnergia",
            data: {
                pecacodi: $("#pecacodi").val()
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

                $.contextMenu({
                    selector: ".menu-contextual",
                    callback: function(key) {
                        var id = $(this).attr("id");

                        if (key === "editar") {
                            editarRegistro(id);
                        }
                        if (key === "visualizar") {
                            //visualizarRegistro(id);
                        }
                    },
                    items: {
                        "editar": { name: "Editar", icon: "editar" },
                        "visualizar": { name: "Visualizar", icon: "visualizar" }
                    }
                });
            },
            error: function() {
                mostrarMensaje("mensaje", $mensajeErrorGeneral, $tipoMensajeError, $modoMensajeCuadro);
            }
        });
    };

function obtenerData() {
    $.ajax({
        type: 'POST',
        url: controlador + 'obtenerDataEnergia',
        data: {
            pecacodi: $("#pecacodi").val()
        },
        dataType: 'json',
        success: function (result) {
            $("select").change();
            //obtenerData
            //window.location.href = controlador + "Energia?pecacodi=" + $("#pecacodi").val();
        },
        error: function () {
            mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
        }
    });
}
