/**
* Llamadas iniciales
* @returns {} 
*/
$(document).ready(function () {
    $('#tabla').dataTable({
        //"paging": false,
        "pageLength": 25
    });
});

function abrirPopUp() {
    $('#popupPeriodo').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
}

function cerrarPopUp() {
    $('#popupPeriodo').bPopup().close();
}

function abrirPopUpNuevo() {
    $("#cbBarra").val("-1");

    abrirPopUp();
}

function obtenerCgndpe(pmCgndCodi) {

    var url = $("#getCgndpe").val();

    //$.ajax({
    //    type: 'Post',
    //    url: url,
    //    dataType: 'json',
    //    data: {
    //        PmCgndCodi: pmCgndCodi
    //    },
    //    cache: false,
    //    //async: false,
    //    success: function (result) {
    //        cargarDatos(result);
    //    },
    //    error: function () {
    //    },
    //    complete: function () {
    //        abrirPopUp();
    //    }
    //});

    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            cargarDatos(this.response);
            abrirPopUp();
        }
    };

    xhttp.open("Get", url + "/?PmCgndCodi=" + pmCgndCodi, false);
    xhttp.setRequestHeader("Content-type", "text/json; charset=utf-8");
    xhttp.send();
}

function cargarDatos(resultados) {
    var result = JSON.parse(resultados);
    $("#txtCodigo").val(result.CodCentral);
    $("#txtNombre").val(result.NombCentral);

    $("#cbBarra").val(result.CodBarra);
    $("#txtTipo").val(result.PmCgndTipoPlanta);
    $("#txtNUnidades").val(result.PmCgndNroUnidades);
    $("#txtPotInstalada").val(result.PmCgndPotInstalada);
    $("#txtFactorOpe").val(result.PmCgndFactorOpe);
    $("#txtProbFalla").val(result.PmCgndProbFalla);
    $("#txtSFal").val(result.PmCgndCorteOFalla);
    $("#txtPmCindCodi").val(result.PmCgndCodi);

}

function updateCgndpe() {
    //var codigo = $("#txtCodigo").val(result.CodCentral);
    //var nombreCentral = $("#txtNombre").val(result.NombCentral);

    var pmCindCodi = $("#txtPmCindCodi").val();
    var codiBarra = $("#cbBarra").val();
    var tipo = $("#txtTipo").val();
    var nUnidades = $("#txtNUnidades").val();
    var potInstalada = $("#txtPotInstalada").val();
    var factorOpe = $("#txtFactorOpe").val();
    var probFalla = $("#txtProbFalla").val();
    var sFal = $("#txtSFal").val();

    var url = $("#updateCgndpe").val();

    $.ajax({
        type: 'Post',
        url: url,
        dataType: 'json',
        data: {
            PmCgndCodi: pmCindCodi,
            GrupoCodi: codiBarra,
            PmCgndTipoPlanta: tipo,
            PmCgndNroUnidades: nUnidades,
            PmCgndPotInstalada: potInstalada,
            PmCgndFactorOpe: factorOpe,
            PmCgndProbFalla: probFalla,
            PmCgndCorteOFalla: sFal
        },
        cache: false,
        //async: false,
        success: function (result) {
            window.location.href = $("#reload").val();
        },
        error: function () {
        },
        complete: function () {
        }
    });
}

function ExportarExcel() {

    var url = $("#exportarExcel").val();
    //var periodo = $("#hddPeriodo").val();

    $.ajax({
        type: 'POST',
        url: url,
        dataType: 'json',
        data: {},
        success: function (result) {

            if (result != -1) {
                document.location.href = siteRoot + 'PMPO/GeneracionArchivosDAT/descargar?formato=' + 1 + '&file=' + result
                mostrarExitoOperacion("Exportación realizada.");
            }
            else {
                mostrarError('Opcion Exportar: Ha ocurrido un error');
            }

        },
        error: function () {
            mostrarMensaje('Ha ocurrido un error');
        }
    });
}


function GenerarRepGrupoRelaso() {

    var idDef = '4,5';

    $.ajax({
        type: 'POST',
        url: siteRoot + 'PMPO/GeneracionArchivosDAT/GenerarRepGrupoRelaso',
        dataType: 'json',
        data: {
            strGrrdefcodi: idDef
        },
        success: function (result) {

            if (result != -1) {                
                document.location.href = siteRoot + 'PMPO/GeneracionArchivosDAT/descargar?formato=' + 1 + '&file=' + result
                mostrarExitoOperacion("Exportación realizada.");
            }
            else {
                mostrarError('Opcion Exportar: Ha ocurrido un error');
            }

        },
        error: function () {
            mostrarMensaje('Ha ocurrido un error');
        }
    });
}