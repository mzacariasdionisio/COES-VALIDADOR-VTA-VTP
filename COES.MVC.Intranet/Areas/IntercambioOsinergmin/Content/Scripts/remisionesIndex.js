var controlador = siteRoot + "intercambioOsinergmin/remisiones/";

$( /**
   * Llamadas iniciales
   * @returns {} 
   */
   $( document ).ready(function() {
        
        //Cuando el año cambie, actualizamos el listado
       //$("#Anio").change(busqueda()); //on("change", pintarBusqueda);

       $("select").change(function () { busqueda() }).change();
       $("#crearSubmit").click(function () {
           window.location.href = controlador + "Create";
       });

   }));

function busqueda() {

    if ($("#Anio").val() === null) return;

    $.ajax({
        type: "POST",
        url: controlador + "listarPeriodos",
        data: {
            anio: $("#Anio").val()
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

            $.contextMenu({
                selector: ".menu-contextual",
                callback: function (key) {
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
        error: function () {
            mostrarMensaje("mensaje", $mensajeErrorGeneral, $tipoMensajeError, $modoMensajeCuadro);
        }
    });
}


var editarRegistro = function (id) {
    window.location.href = controlador + "Edit?periodo=" + id;
}

var crearRegistro = function (id) {
    window.location.href = controlador + "Create";
}