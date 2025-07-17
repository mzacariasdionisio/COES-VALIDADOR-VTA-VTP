var controlador = siteRoot + "compensacion/general/";

$( /**
   * Llamadas iniciales
   * @returns {} 
   */
   $( document ).ready(function() {
       $("select").change(function () { pintarBusqueda() }).change();
   }));



var pintarBusqueda =
    /**
    * Pinta el listado de periodos según el año seleccionado
    * @returns {} 
    */
    function () {


        $.ajax({
            type: "POST",
            url: controlador + "listarCostosMarginales",
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
                    "iDisplayLength": 50000
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
