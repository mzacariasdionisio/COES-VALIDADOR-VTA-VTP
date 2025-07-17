var controlador = siteRoot + 'campanias/plantransmision/';
var controladorReporte = siteRoot + 'campanias/reportes/';
$(function () {
    $(".repProyectos").on("change", verificarHabilitacion);

    cargarDatos().then(() => {
        $('#btnGenerar').on('click', function () {
            if (validateForm()) {
                generarReporte();
            }
        });
    });
    // Uso de la función
    cargarCatalogo("99")
        .then(data => {
            console.log("Datos recibidos:", data);
        })
        .catch(error => {
            console.error("Error en la solicitud:", error);
        });
});

function verificarHabilitacion() {
    let algunProyectoSeleccionado = $(".repProyectos:checked").length > 0;

    let cronogramaCheckbox = $(".chkbox_class[value='13']");

    if (algunProyectoSeleccionado) {
        cronogramaCheckbox.prop("disabled", false);
    } else {
        cronogramaCheckbox.prop("disabled", true).prop("checked", false);
    }
}

function validateForm() {
    const periodo = $('#periodoSelect').val();
    const empresa = $('#empresaSelect').multipleSelect('getSelects');
    const estado = $('#cbEstado').val();
    const tipoReporte = $('#cbTipoReporte').val();
    const checkboxes = document.querySelectorAll('input[name="checkTipoProyecto"]:checked');

    if (!periodo) {
        mostrarMensaje('errorReportes', 'Por favor, seleccione un período en el campo Plan de transmisión.', 1, 2);
        return false;
    }
    if (empresa.length === 0) {
        mostrarMensaje('errorReportes', 'Por favor, seleccione al menos una empresa.', 1, 2);
        return false;
    }
    if (!estado) {
        mostrarMensaje('errorReportes', 'Por favor, seleccione un valor en el campo Estado.', 1, 2);
        return false;
    }
    if (!tipoReporte) {
        mostrarMensaje('errorReportes', 'Por favor, seleccione el tipo de reporte que desea generar.', 1, 2);
        return false;
    }

    if (checkboxes.length === 0) {
        mostrarMensaje('errorReportes', 'Se requiere seleccionar en el checkbox un tipo de reporte.', 1, 2);
        return false;
    }

    //document.getElementById('alertMessage').style.display = 'none';
    return true;
}

function cargarDatos() {
    return Promise.all([cargarPeriodo()])
        .then(cargarEmpresa);
}

generarReporte = function () {
    var idreporte_val = $('input:checkbox:checked.chkbox_class').map(function () {
        return this.value;
    }).get().join(",");
    var empresa = $('#empresaSelect').multipleSelect('getSelects');
    var periodo = $('#periodoSelect').val();
    var estado = $('#cbEstado').val();
    var tipoReporte = $('#cbTipoReporte').val();

    console.log('id reporte', idreporte_val);
    console.log('periodoSelect', periodo);
    console.log('estado', estado);
    console.log('tipoReporte', tipoReporte);
    $.ajax({
        type: 'POST',
        url: controladorReporte + 'DescargarReporte',
        data: {
            empresas: empresa.join(','),
            periodo: periodo,
            estado: estado,
            idreportes: idreporte_val,
            tipoReporte: tipoReporte
        },
        success: function (result) {
            if (result.tipoReporte.length > 0) {
                var cod = $("#periodoSelect option:selected").text();
                var name = `ReporteEnvios${cod}.zip`;
                window.location.href = controladorReporte + `GenerarZipReporte?nameZip=${name}&nameFolderTemp=${result.nombreFolderTemp}`;
            }
            else {
                alert("Ha ocurrido un error al generar ficha excel");
            }
            $("#errorReportes").html("").attr("class", "class-error")
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });

}

function cargarEmpresa() {
    return new Promise((resolve, reject) => {
        $.ajax({
            type: 'POST',
            url: controlador + 'ListarEmpresas',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(''),
            success: function (eData) {
                cargarListaEmpresas(eData);
                $('#empresaSelect').multipleSelect({
                    width: '178px',
                    filter: true
                });
                $('#empresaSelect').multipleSelect('checkAll');
                resolve();
            },
            error: function (err) {
                alert("Ha ocurrido un error");
                reject(err);
            }
        });
    });
}


function cargarListaEmpresas(empresas) {
    var selectEmpresa = $('#empresaSelect');
    $.each(empresas, function (index, empresa) {
        // Crear la opción
        if(empresa.Emprcodi != 0){
            var option = $('<option>', {
                value: empresa.Emprcodi,
                text: empresa.Emprnomb
            });
    
            // Agregar la opción al select
            selectEmpresa.append(option);
        }
    });
}

function cargarPeriodo() {
    return new Promise((resolve, reject) => {
    $.ajax({
        type: 'POST',
        url: controlador + 'ListarPeriodos',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(''),
        success: function (eData) {
            cargarListaPeriodo(eData);
            resolve(); 
        },
        error: function (err) {
            alert("Ha ocurrido un error");
            reject(err); 
        }
        });
    });
}

function cargarListaPeriodo(periodos) {
    var selectPeriodo = $('#periodoSelect');
    $.each(periodos, function (index, periodo) {
        // Crear la opción
        var option = $('<option>', {
            value: periodo.PeriCodigo,
            text: periodo.PeriNombre
        });

        // Agregar la opción al select
        selectPeriodo.append(option);
    });
}

function cargarCatalogo(descortaCat) {
    return new Promise((resolve, reject) => {
        $.ajax({
            type: 'POST',
            url: controlador + 'ListarCatalogoXDesc',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ descortaCat }), // Enviar el parámetro como objeto
            success: function (response) {
                resolve(response); // Resolver la promesa con los datos recibidos
            },
            error: function (err) {
                alert("Ha ocurrido un error al cargar el catálogo");
                reject(err); // Rechazar la promesa con el error recibido
            }
        });
    });
}


document.addEventListener("DOMContentLoaded", function () {

    function actualizarCheckboxes() {
        var select = document.getElementById("cbTipoReporte");
        var selectedValue = select.value;

        // Ocultar todos los checkboxes y desmarcarlos
        document.querySelectorAll(".chkbox_class").forEach(function (checkbox) {
            checkbox.checked = false;
            checkbox.parentElement.style.display = "none";
        });

        // Mostrar solo los que corresponden a la selección
        var mostrarClase = "";
        switch (selectedValue) {
            case "DeEmpresas":
                mostrarClase = "repEmpresas";
                break;
            case "DeProyectos":
                mostrarClase = "repProyectos";
                break;
            case "DePronosticoDemanda":
                mostrarClase = "repPronosticos";
                break;
            case "DePronosticos":
                mostrarClase = "repITC";
                break;
            default:
                return;
        }

        document.querySelectorAll("." + mostrarClase).forEach(function (checkbox) {
            console.log('checbox', checkbox)
            checkbox.parentElement.style.display = "block";
            if (selectedValue === "DeEmpresas") {
                checkbox.checked = true;
            }
            if (selectedValue === "DePronosticoDemanda") {
                checkbox.checked = true;
            }
        });
    }

    document.getElementById("cbTipoReporte").addEventListener("change", actualizarCheckboxes);

    // Seleccionar por defecto 'DePronosticos' y mostrar repITC
    //document.getElementById("cbTipoReporte").value = "DePronosticos";
    actualizarCheckboxes();
});


