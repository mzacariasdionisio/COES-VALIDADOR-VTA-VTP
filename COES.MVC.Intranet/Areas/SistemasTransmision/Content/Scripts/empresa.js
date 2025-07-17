var Empresa = "Empresa/";
var controlador = siteRoot + "SistemasTransmision/" + Empresa;

var Generador = "Generador/"
var controladorGen = siteRoot + "SistemasTransmision/" + Generador;

var Transmisor = "Transmisor/"
var controladorTrans = siteRoot + "SistemasTransmision/" + Transmisor;

var Barra = "Barra/";
var controladorBarr = siteRoot + "SistemasTransmision/" + Barra;

$(document).ready(function () {
    buscarTransmisor();
    buscarGenerador();
    buscarBarra();
    
    $('#btnNuevoTransmisor').click(function () {
        nuevoTransmisor();
    });

    $('#btnNuevoGenerador').click(function () {
        nuevoGenerador();
    });

    $('#btnExportarGenerador').click(function () {
        exportarGenerador(1);
    });

    $('#btnNuevoRecabarra').click(function () {
        nuevoRecabarra();
    });

    $('#btnConsultar').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        
    });

    $('#btnRefrescar').click(function () {
        refrescar();
    });

    $('#tab-container').easytabs({
        animate: false
    });
});

buscarTransmisor = function () {
    $.ajax({
        type: 'POST',
        url: controladorTrans + "List",
        data: { strecacodi: $('#strecacodi').val() },
        success: function (evt) {
            $('#listadoTransmisor').html(evt);
            addDeleteEventTran();
            //viewEvent();
            oTable = $('#tabla').dataTable({
                "sPaginationType": "full_numbers",
                "destroy": "true"
            });
            $('#btnNuevo').show();
            mostrarExito('Puede consultar y modificar la información');
        },
        error: function () {
            mostrarError("Lo sentimos, se ha producido un error");
        }
    });
};

buscarGenerador = function () {
    $.ajax({
        type: 'POST',
        url: controladorGen + "List",
        data: {strecacodi: $('#strecacodi').val() },
        success: function (evt) {
            $('#listadoGenerador').html(evt);
            addDeleteEventGen();
            //viewEvent();
            oTable = $('#tabla').dataTable({
                "sPaginationType": "full_numbers",
                "destroy": "true"
            });
            $('#btnNuevo').show();
            mostrarExito('Puede consultar y modificar la información');
        },
        error: function () {
            mostrarError("Lo sentimos, se ha producido un error");
        }
    });
};

buscarBarra = function () {
    $.ajax({
        type: 'POST',
        url: controladorBarr + "List",
        data: { strecacodi: $('#strecacodi').val() },
        success: function (evt) {
            $('#listadoBarra').html(evt);
            addDeleteEvent();
            oTable = $('#tabla').dataTable({
                "sPaginationType": "full_numbers",
                "destroy": "true"
            });
            $('#btnNuevo').show();
            mostrarExito('Puede consultar y modificar la información');
        },
        error: function () {
            mostrarError("Lo sentimos, se ha producido un error");
        }
    });
};

nuevoTransmisor = function () {
    var Stpercodi = document.getElementById('stpercodi').value;
    var Strecacodi = document.getElementById('strecacodi').value;
    window.location.href = controladorTrans + "new?stpercodi=" + Stpercodi + "&strecacodi=" + Strecacodi;
}

nuevoGenerador = function () {
    var Stpercodi = document.getElementById('stpercodi').value;
    var Strecacodi = document.getElementById('strecacodi').value;
    window.location.href = controladorGen + "new?stpercodi=" + Stpercodi + "&strecacodi=" + Strecacodi;
}

exportarGenerador = function (formato) {
    var Stpercodi = document.getElementById('stpercodi').value;
    var Strecacodi = document.getElementById('strecacodi').value;
    $.ajax({
        type: 'POST',
        url: controladorGen + 'ExportarGenerador',
        data: { stpercodi: Stpercodi, strecacodi: Strecacodi, formato: formato },
        dataType: 'json',
        success: function (result) {
            if (result != -1) {
                window.location = controladorGen + 'abrirarchivo?formato=' + formato + '&file=' + result;
                mostrarExito("Felicidades, el archivo se descargo correctamente...!");
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

nuevoRecabarra = function () {
    var Stpercodi = document.getElementById('stpercodi').value;
    var Strecacodi = document.getElementById('strecacodi').value;
    window.location.href = controladorBarr + "new?stpercodi=" + Stpercodi + "&strecacodi=" + Strecacodi;
}

addDeleteEventTran = function () {
    $(".deleteET").click(function (e) {
        e.preventDefault();
        if (confirm("¿Desea eliminar la información seleccionada?")) {
            id = $(this).attr("id").split("_")[1];
            $.ajax({
                type: "post",
                dataType: "text",
                url: controladorTrans + "DeleteET/" + id,
                data: addAntiForgeryToken({ id: id }),
                success: function (resultado) {
                    if (resultado == "true")
                        $("#filaET_" + id).remove();
                    else
                        alert("No se ha logrado eliminar el registro");
                }
            });
        }
    });
};

addDeleteEventGen = function () {
    $(".deleteEG").click(function (e) {
        e.preventDefault();
        if (confirm("¿Desea eliminar la información seleccionada?")) {
            id = $(this).attr("id").split("_")[1];
            $.ajax({
                type: "post",
                dataType: "text",
                url: controladorGen + "DeleteEG/" + id,
                data: addAntiForgeryToken({ id: id }),
                success: function (resultado) {
                    if (resultado == "true")
                        $("#filaEG_" + id).remove();
                    else
                        alert("No se ha logrado eliminar el registro");
                }
            });
        }
    });
};

addDeleteEvent = function () {
    $(".deleteBAR").click(function (e) {
        e.preventDefault();
        if (confirm("¿Desea eliminar la información seleccionada?")) {
            id = $(this).attr("id").split("_")[1];
            $.ajax({
                type: "post",
                dataType: "text",
                url: controladorBarr + "DeleteBAR/" + id,
                data: addAntiForgeryToken({ id: id }),
                success: function (resultado) {
                    if (resultado == "true")
                        $("#filaBAR_" + id).remove();
                    else
                        alert("No se ha logrado eliminar el registro");
                }
            });
        }
    });
};

Recargar = function () {
    var tab1 = document.getElementById('paso1');
    var tab2 = document.getElementById('paso2');
    var tab3 = document.getElementById('paso3');
    var tabSelected;
    if (tab1.className) {
        tabSelected = "paso1";
    }
    else if (tab2.className) {
        tabSelected = "paso2";

    } else if (tab3.className) {

        tabSelected = "paso3";
    }
    var cmbStPercodi = document.getElementById('stpercodi');
    var cmbStRecacodi = document.getElementById('strecacodi');
    window.location.href = controlador + "index?stpercodi=" + cmbStPercodi.value + "&strecacodi=" + cmbStRecacodi.value + "#" +tabSelected;
}

RecargarTitular = function () {
    var cmbStPercodi = document.getElementById('pericodi');
    var cmbStRecacodi = document.getElementById('recacodi');
    var cmbEmprecodi = document.getElementById('emprcodi');
    window.location.href = controladorTrans + "New?stpercodi=" + cmbStPercodi.value + "&strecacodi=" + cmbStRecacodi.value + "&emprcodi=" + cmbEmprecodi.value;
}

RecargarTitular2 = function () {
    var cmbStPercodi = document.getElementById('pericodi');
    var cmbStRecacodi = document.getElementById('recacodi');
    var cmbEmprecodi = document.getElementById('emprcodi');
    window.location.href = controladorGen + "New?stpercodi=" + cmbStPercodi.value + "&strecacodi=" + cmbStRecacodi.value + "&emprcodi=" + cmbEmprecodi.value;
}

RecargarActualizarTrans = function () {
    var cmbStTranscodi = document.getElementById('stranscodi');
    var cmbEmprecodi = document.getElementById('emprcodi');
    window.location.href = controladorTrans + "Edit?id=" + cmbStTranscodi.value + "&emprecodi=" + cmbEmprecodi.value;
}

ReturnTab2 = function () {
    var stpercodi = document.getElementById('pericodi');
    var strecacodi = document.getElementById('recacodi');
    window.location.href = controlador + "Index?stpercodi=" + stpercodi.value + "&strecacodi=" + strecacodi.value + "#paso2";
}

ReturnTab3 = function () {
    var stpercodi = document.getElementById('pericodi');
    var strecacodi = document.getElementById('recacodi');
    window.location.href = controlador + "Index?stpercodi=" + stpercodi.value + "&strecacodi=" + strecacodi.value + "#paso3";
}

refrescar = function () {
    window.location.href = controlador + "index";    
}