var controlador = siteRoot + "compensacion/general/";

$( /**
   * Llamadas iniciales
   * @returns {} 
   */
   $( document ).ready(function() {
        pintarBusqueda();

        $('#btnCancelar').click(function () {
            cancelar();
        });

        $('#btnGrabar').click(function () {
            grabarInforme();
        });


   }));

var pintarBusqueda =
    /**
    * Pinta el listado de periodos según el año seleccionado
    * @returns {} 
    */
    function () {


        $.ajax({
            type: "POST",
            url: controlador + "listarMesValorizacion",
            data: {

            },
            success: function(evt) {
                //$("#listado").html(evt);
                //$("#tabla").dataTable({
                //    "scrollY": 314,
                //    "scrollX": false,
                //    "sDom": "t",
                //    "ordering": false,
                //    "bDestroy": true,
                //    "bPaginate": false,
                //    "iDisplayLength": 50
                //});

                $('#listado').html(evt);
                $('#tabla').dataTable({
                    "ordering": false,
                    "paging": false,
                    "bDestroy": true
                });

            },
            error: function() {
                mostrarMensaje("mensaje", $mensajeErrorGeneral, $tipoMensajeError, $modoMensajeCuadro);
            }
        });
    };

var editarRegistro = function (id) {
    window.location.href = controlador + "EditMesValorizacion?periodo=" + id;
}

function editarInforme(pericodi, perinombre, informe) {
    
    $("#pericodi").val(pericodi);
    $("#periodo").val(perinombre);
    $("#informe").val(informe);

    $("#popupEdicion").bPopup({
        autoClose: false
    });
}
function cancelar() {
    $('#popupEdicion').bPopup().close();
}

function grabarInforme() {

   
    if ($('#informe').val() == "" || $('#informe').val() == null) {
        alert("Debe ingresar el nombre del Informe.");
        return false;
    }

    var pericodi = $('#pericodi').val();

    if (confirm("¿Desea guardar la información?")) {

        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: controlador + "GuardarInforme",
            data: {
                pericodi: pericodi,
                nombreInforme: $('#informe').val()
            },
           
            success: function (evt) {
                if (evt.success) {
               
                    alert("Registro actualizado correctamente.");
                    $('#popupEdicion').bPopup().close();
                    $("#pericodi").val(0);
                    pintarBusqueda();
                    //llenarListadoProcesos();
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }


}

