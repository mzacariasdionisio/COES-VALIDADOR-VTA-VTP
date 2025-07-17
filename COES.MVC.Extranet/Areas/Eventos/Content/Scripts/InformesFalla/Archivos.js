var controlador = siteRoot + 'eventos/InformesFalla/';
var inputFile = $("#seleccionArchivos");
var listaDeArchivos;
var listabtnEliminar;
var archivosParaSubir = [];
let listaEliminarHtml;

$(function () {
    $("#btnEnviar").click(function () {
        Grabar();
    });
    $("#btnCancelar").click(function () {
        cerrarPopUp();
    });   
    ObtenerListado();
    
});

function actualizarListaDeArchivos() {
    listaDeArchivos = $("#listaDeArchivos");
    listabtnEliminar = $("#listabtnEliminar");
    $("#tablaArchivos").val('');
    let listaHtml = archivosParaSubir.map(function (item, index) {
        return `<ul style="padding: 2px;margin: 0;">${item.name}</ul>`;
    });
    
    listaEliminarHtml = archivosParaSubir.map(function (item, index) {
        return `<ul style="padding: 2px;margin: 0;">
                <a href="JavaScript:Eliminar(${index})" style="background-color:transparent; border-color:transparent;"><img src="../../../Content/Images/btn-cancel.png" style="width:12px;" /></a>
</ul>`;
    });
    listaDeArchivos.html(listaHtml);
    listabtnEliminar.html(listaEliminarHtml);
}

inputFile.on('change', function (e) {
    let files = e.target.files;
    if (files.length == 0) return;
  
    files = Array.from(files);
    archivosParaSubir = files;
    $('#mensajeEvento').css("display", "none");

    var incorrecto = 0;

    for (var i = 0; i < archivosParaSubir.length; i++) {
        var fileSize = archivosParaSubir[i].size;
        var ext = archivosParaSubir[i].name.split('.').pop();
        var nam = archivosParaSubir[i].name.split('.').shift();
        // Convertimos en minúscula porque la extensión del archivo puede estar en mayúscula
        ext = ext.toLowerCase();
        if (ext == 'pdf' || ext == 'doc' || ext == 'docx' || ext == 'xls' || ext == 'xlsx' || ext == 'csv' || ext == 'xlsm' || ext == 'zip' || ext == 'rar') {
            if (fileSize > 25000000) {
                mostrarAlerta('El tamaño del archivo es superior al permitido')
                incorrecto++;
                this.value = '';
                archivosParaSubir = '';
            }
            if (expresionRegular(nam) == 1) {
                mostrarAlerta('No puedes ingresar caracteres especiales');
                incorrecto++;
                this.value = '';
                archivosParaSubir = '';
            }
                
        }
        else {
            mostrarAlerta('El archivo no tiene la extensión adecuada. Debe agregarlo como .zip o .rar');
            incorrecto++;
            this.value = ''; // reset del valor
            archivosParaSubir = '';
        }
    }
    if (incorrecto == 0) {
        actualizarListaDeArchivos();
        $(this).val('');
    }

});


function Eliminar(index) {
    archivosParaSubir.splice(index, 1);
    actualizarListaDeArchivos();
}

function Grabar() {
    var Emprcodi = $("#hdEmprcodi").val();
    var IdEvento = $("#hdIdEvento").val();
    var TipoInforme = $("#hdTipoInforme").val();
    var PlazoEnvio = $("#hdPlazoEnvio").val();
    var formData = new FormData();
    
    formData.append("Emprcodi", Emprcodi);
    formData.append("IdEvento", IdEvento);
    formData.append("TipoInforme", TipoInforme); 
    formData.append("PlazoEnvio", PlazoEnvio);
    if (archivosParaSubir.length > 0) {
        for (i = 0; i < archivosParaSubir.length; i++) {
            formData.append("file", archivosParaSubir[i]);
        }
        $.ajax({
            url: controlador + "GrabarInformes",
            data: formData,
            type: 'POST',
            processData: false,
            contentType: false,
            success: function (data) {
                if (data.result > 1) {
                    if(data.rpta > 0)
                        mostrarExito("Los datos se guadaron correctamente. Existen archivos no migrados de Sco a Sev por tener un nombre mayor a 80 caracteres.");
                    else
                        mostrarExito("Los datos se guadaron correctamente");
                    /*alert('Se cargo correctamente');*/
                    ObtenerListado();
                    this.value = '';
                    archivosParaSubir = '';
                    listaDeArchivos.html('');
                    listabtnEliminar.html('');

                }
                if (data.result == -2)
                {
                    mostrarAlerta(data.responseText)
                    //alert(data.responseText);
                }
                
            }
        });
    }
    else
        mostrarAlerta('Debe seleccionar por lo menos un archivo.');
}

function ObtenerListado() {
    var IdEvento = $("#hdIdEvento").val();
    var TipoInforme = $("#hdTipoInforme").val();
    
    $("#listadoInformes").html('');

    $.ajax({
        type: 'POST',
        url: controlador + "ListadoInformesEnviados",
        data: {
            IdEvento: IdEvento,
            TipoInforme: TipoInforme
        },
        success: function (eData) {
            $('#listadoInformes').html(eData);
            
        },
        error: function (err) {
            mostrarError("Ha ocurrido un error");
        }
    });
}

const formato = /[`!@#$%^&*+={}':"\\|,<>\/?~\n/]/;
const expresionRegular = (texto) => {
    if (formato.test(texto)) {
        return 1;
    }
    else
        return 0;
}

function descargarInforme(appName,env_evencodi, eveinfcodi, nombreArchivo, anio, semestre, diames, afiversion, emprCodi) {

    var IdEvento = $("#hdIdEvento").val();
    var TipoInforme = $("#hdTipoInforme").val();

    var url = `${appName}/Eventos/InformesFalla/DescargarArchivoSco` +
        '?idEvento=' + IdEvento +
        '&env_evencodi=' + env_evencodi +
        '&eveinfcodi=' + eveinfcodi +
        '&nombreArchivo=' + encodeURIComponent(nombreArchivo) +
        '&anio=' + anio +
        '&semestre=' + semestre +
        '&diames=' + diames +
        '&TipoInforme=' + TipoInforme +
        '&emprCodi=' + emprCodi;

    window.location.href = url;
}

function descargarZipTodos(appName) {
    const idEvento = $("#hdIdEvento").val();
    const tipoInforme = $("#hdTipoInforme").val();

    const url = `${appName}/Eventos/InformesFalla/DescargarZipInformes?idEvento=${idEvento}&tipoInforme=${tipoInforme}`;
    window.location.href = url;
}
