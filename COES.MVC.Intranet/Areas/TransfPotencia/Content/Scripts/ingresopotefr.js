var controlador = siteRoot + "transfpotencia/ingresopotefr/";

$(function () {
    $('#btnNuevo').hide();
    $('#btnCargarInfoPFR').hide();

    //$('#btnConsultar').click(function () {
    //    mostrarAlerta("Espere un momento, se esta procesando su solicitud");
    //    buscar();
    //});

    $('#recpotcodi').change(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        buscar();
    });

    if ($('#pericodi').val() > 0 && $('#recpotcodi').val() > 0)
    {
        buscar();
    }

    $("#btnCargarInfoPFR").click(function () {
        openPopup("popupCargaInfoPFR");
    });

    $("#btnProcesarIngresoPFR").click(function () {
        procesarCargaPFR();
    });

    $("#btnCancelarIngresoPFR").click(function () {
        $('#popupCargaInfoPFR').bPopup().close();
    });
});

buscar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "lista",
        data: { pericodi: $('#pericodi').val(), recpotcodi: $('#recpotcodi').val() },
        success: function (evt) {
            $('#listado').html(evt);
            addDeleteEvent();
            viewEvent();
            oTable = $('#tabla').dataTable({
                "sPaginationType": "full_numbers",
                "destroy": "true"
            });
            $('#btnNuevo').show();
            $('#btnCargarInfoPFR').show();

            if ($('#recalculoentidad').val() == "1") {
                document.getElementById("btnCargarInfoPFR").style.visibility = 'visible';
                document.getElementById("texto").style.visibility = 'visible';
                txt = document.getElementById("texto");
                txt.innerHTML = "FORMATO NUEVO: CENTRAL UNIDAD SEPARADO ";
            }
            else {
                document.getElementById("btnCargarInfoPFR").style.visibility = 'hidden';
                document.getElementById("texto").style.visibility = 'visible';
                txt = document.getElementById("texto");
                txt.innerHTML = "FORMATO ANTIGUO: CENTRAL UNIDAD UNIFICADO";
            }

            mostrarExito('Puede consultar y modificar la información');
        },
        error: function () {
            mostrarError("Lo sentimos, se ha producido un error");
        }
    });
};

//Descripcion
viewEvent = function () {
    $('.view').click(function () {
        iPefrcodi = $(this).attr("id").split("_")[1];
        abrirPopup(iPefrcodi);
    });
};

abrirPopup = function (ipefrcodi) {
    $.ajax({
        type: 'POST',
        url: controlador + "View",
        data: { ipefrcodi: ipefrcodi },
        success: function (evt) {
            $('#popup').html(evt);
            setTimeout(function () {
                $('#popup').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
        },
        error: function () {
            mostrarError("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

//Funciones de eliminado de registro
addDeleteEvent = function () {
    $(".delete").click(function (e) {
        e.preventDefault();
        if (confirm("¿Desea eliminar la información seleccionada?")) {
            ipefrcodi = $(this).attr("id").split("_")[1];
            $.ajax({
                type: "post",
                dataType: "text",
                url: controlador + "Delete",
                data: { pericodi: $('#pericodi').val(), recpotcodi: $('#recpotcodi').val(), ipefrcodi: ipefrcodi },
                success: function (resultado) {
                    if (resultado == "true") {
                        $("#fila_" + ipefrcodi).remove();
                        mostrarExito("El registro se ha eliminado correctamente");
                    }
                    else
                        mostrarError("No se ha logrado eliminar el registro");
                }
            });
        }
    });
};

Recargar = function () {
    var cmbPericodi = document.getElementById('pericodi');
    window.location.href = controlador + "index?pericodi=" + cmbPericodi.value;
}

function openPopup(contentPopup) {
    setTimeout(function () {
        $("#" + contentPopup).bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
}

function procesarCargaPFR() {
    var obj = {};
    obj = getObjetoJsonCabecera();

    if (confirm('¿Desea Procesar la carga de PFR?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'ProcesarCargaPFR',
            data: {
                pfrreccodi: obj.Pfrreccodi,
                pericodi: obj.pericodi,
                recpotcodi: obj.recpotcodi
            },
            success: function (result) {
                if (result.Resultado == '-1') {
                    alert('Ha ocurrido un error:' + result.Mensaje);
                } else {
                    if (result.Resultado == '1') {
                        $('#popupCargaInfoPFR').bPopup().close();
                        alert("La información de Potencia Firme Remunerable ha sido cargada exitosamente.");

                        buscar();
                    }
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error.');
            }
        });

    }
}

function getObjetoJsonCabecera() {
    var obj = {};

    obj.Pfrreccodi = parseInt($("#idrecapfr").val()) || 0;
    obj.pericodi = parseInt($("#pericodi").val()) || 0;
    obj.recpotcodi = parseInt($("#recpotcodi").val()) || 0;

    return obj;
}
