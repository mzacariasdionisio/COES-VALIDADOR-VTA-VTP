/**
* Llamadas iniciales
* @returns {} 
*/
$(document).ready(function () {
    $('#tabla').dataTable({
        //"paging": false
        "pageLength": 25
    });

    $('#txtFecha').Zebra_DatePicker({
        format: 'd/m/Y'
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

function obtenerMgnd(pmMgndCodi) {

    var url = $("#getMgndpe").val();

    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            cargarDatos(this.response);
            abrirPopUp();
        }
    };

    xhttp.open("GET", url + "/?PmMgndCodi=" + pmMgndCodi, false);
    xhttp.setRequestHeader("Content-type", "text/json; charset=utf-8");
    xhttp.send();
}

function cargarDatos(resultados) {
    var result = JSON.parse(resultados);
    $("#txtCodigo").val(result.CodCentral);
    $("#txtNombre").val(result.NombCentral);

    $("#txtFecha").val(result.PmMgndFechaTexto);
    $("#cbBarra").val(result.CodBarra);
    $("#txtTipo").val(result.PmMgndTipoPlanta);
    $("#txtNUnidades").val(result.PmMgndNroUnidades);
    $("#txtPotInstalada").val(result.PmMgndPotInstalada);
    $("#txtFactorOpe").val(result.PmMgndFactorOpe);
    $("#txtProbFalla").val(result.PmMgndProbFalla);
    $("#txtSFal").val(result.PmMgndCorteOFalla);

    $("#txtPmMgndCodi").val(result.PmMgndCodi);

}

function updateMgndpe() {
    
    //var nombreCentral = $("#txtNombre").val(result.NombCentral);
    
    var pmMgndCodi = $("#txtPmMgndCodi").val();
    var pmMgndFecha = $("#txtFecha").val();
    var codiBarra = $("#cbBarra").val();
    var tipo = $("#txtTipo").val();
    var nUnidades = $("#txtNUnidades").val();
    var potInstalada = $("#txtPotInstalada").val();
    var factorOpe = $("#txtFactorOpe").val();
    var probFalla = $("#txtProbFalla").val();
    var codigoCentral = $("#txtCodigo").val();
    var sFal = $("#txtSFal").val();
    
    var url = $("#updateMgndpe").val();

    $.ajax({
        type: 'Post',
        url: url,
        dataType: 'json',
        data: {
            PmMgndCodi: pmMgndCodi,
            PmMgndFechastr: pmMgndFecha,
            GrupoCodi: codigoCentral,
            CodBarra: codiBarra,
            PmMgndTipoPlanta: tipo,
            PmMgndNroUnidades: nUnidades,
            PmMgndPotInstalada: potInstalada,
            PmMgndFactorOpe: factorOpe,
            PmMgndProbFalla: probFalla,
            PmMgndCorteOFalla: sFal
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