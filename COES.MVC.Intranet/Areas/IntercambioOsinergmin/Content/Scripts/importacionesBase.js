
var importar =
    /**
     * 
     * @returns {} 
    */
    function () {
        $.ajax({
            type: "POST",
            url: controlador + "Importar",
            data: {
                periodo: $("#PeriodoImportacionModel_Periodo").val()
            },
            dataType: "json",
            success: function(result) {
                if (result === 1) {
                    mostrarMensaje("mensaje", "Se realizó la importación correctamente.", $tipoMensajeExito, $modoMensajeCuadro);
                } else {
                    mostrarMensaje("mensaje", $mensajeErrorGeneral, $tipoMensajeError, $modoMensajeCuadro);
                }
            },
            error: function() {
                mostrarMensaje("mensaje", $mensajeErrorGeneral, $tipoMensajeError, $modoMensajeCuadro);
            }
        });
    }

var pintarBusqueda =
    /**
    * Pinta el listado de entidades con los datos pertinentes al periodo seleccionado
    * @returns {} 
    */
    function () {
        if ($("#PeriodoImportacionModel_Periodo").val() === null) return;
        $.ajax({
            type: "POST",
            url: controlador + "listarEntidades",
            data: {
                periodo: $("#PeriodoImportacionModel_Periodo").val()
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
                if ($("#importar").length) {
                    $("#importar").click(importar);
                }
            },
            error: function () {
                mostrarMensaje("mensaje", $mensajeErrorGeneral, $tipoMensajeError, $modoMensajeCuadro);
            }
        });
    };