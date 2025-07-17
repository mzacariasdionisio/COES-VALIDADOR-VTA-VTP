var controlador = siteRoot + "intercambioOsinergmin/importaciones/";

$( /**
   * Llamadas iniciales
   * @returns {} 
   */
    function() {
        //Cuando el año cambie, actualizamos el listado
        $("select").change(function () { busqueda() }).change();

        $("#crearSubmit").click(function () {
            window.location.href = controlador + "Create";
        });
    });

var busqueda =
    /**
    * Pinta el listado de periodos según el año seleccionado
    * @returns {} 
    */
    function () {

        if ($("#Anio").val() === null) return;
        $.ajax({
            type: "POST",
            url: controlador + "listarPeriodos",
            data: {
                anio: $("#Anio").val()
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

var editarRegistro = function (id) {
    window.location.href = controlador + "Edit?periodo=" + id;
};

function procesarEnvioCoes(periodoRegistro, tablasEmpresasProcesar) {

    if (tablasEmpresasProcesar > 0) {
        alert('Existe información incompleta en el periodo. Revisar.');
        return;
    }

    if ($("#Anio").val() === null) return;
    $.ajax({
        type: "POST",
        url: controlador + "ProcesarEnvioCoes",
        data: {
            periodo: periodoRegistro
        },
        success: function (evt) {

            if (evt == 'OK') {
                alert('Se realizó el envio de datos a Medidores satisfactoriamente.');
                busqueda();
            } else {
                alert(evt);
                //mostrarError(evt);
                //mostrarMensaje("Ha ocurrido un error en Envio Datos. Consulte con el Administrador.", $mensajeErrorGeneral, $tipoMensajeError, $modoMensajeCuadro);
            }
            
           
        },
        error: function () {
            mostrarMensaje("mensaje", $mensajeErrorGeneral, $tipoMensajeError, $modoMensajeCuadro);
        }
    });
}

function mostrarError(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-error');
}

function procesarAperturaCierre(pSicliCodi, accion) {
    $.ajax({
        type: "POST",
        url: controlador + "abrirCerrarPeriodo",
        data: {
            pSicliCodi: pSicliCodi,
            accion: accion
        },
        dataType: "json",
        success: function (result) {
            alert(result.mensaje);
            location.reload();
        },
        error: function (xhr) {
            mostrarError("Ocurrio un problema >> readyState: " + xhr.readyState + " | status: " + xhr.status + " | responseText: " + xhr.responseText);
        }
    });
}

function abrirPeriodo(pSicliCodi, accion) {
    procesarAperturaCierre(pSicliCodi, accion);
}


function cerrarPeriodo(pSicliCodi, accion, tablasEmpresasProcesar) {

    if (tablasEmpresasProcesar > 0) {
        alert('Existe información incompleta en el periodo. Revisar.');
        return;
    }
    procesarAperturaCierre(pSicliCodi, accion);
}