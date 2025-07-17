//Declarando las variables
let controlador = siteRoot + 'CeldaAcoplamiento/';
let hot;
let hotOptions;
let evtHot;

$(function () {

    $("#popUpEditar").addClass("general-popup");
    $("#popUpIncluir").addClass("general-popup");
    $("#popupExcluirEquipo").addClass("general-popup");

    $("#btnIncluir").click(function () {
        incluir();
    });

    $('#btnConsultar').click(function () {
        buscarCeldaAcoplamiento();
    });

    inputSoloNumeros('fCodigoId');
    $("#btnDescargarPlantilla").click(function () {
        descargarPlantilla();
    });

    $('#btnImportarDatos').click(function () {
        muestraPopupImportarDatos();
    });

    $("#btnSiguiente").click(function () {
        leerExcelSubido();
    });
    $("#btnCerrar").click(function () {
        $("#popupCargaMasiva").bPopup().close();
        buscarCeldaAcoplamiento();
    });

    crearPupload();

    inputSoloNumerosDecimales([
        'fNivelTension'
    ]);

    $("#btnExportar").click(function () {
        exportarDatos();
    });

    let permisoEdicion = $("#hdnNuevo").val();
    if (permisoEdicion == '1') {
        document.getElementById('btnIncluir').style.display = 'unset';
    }

    let permisoExportar = $("#hdnExportar").val();
    if (permisoExportar == '1') {
        document.getElementById('btnExportar').style.display = 'unset';
    }

    let permisoImportar = $("#hdnImportar").val();
    if (permisoImportar == '1') {
        document.getElementById('btnDescargarPlantilla').style.display = 'unset';
        document.getElementById('btnImportarDatos').style.display = 'unset';
    }

    $('#fUbicacion').multipleSelect({
        width: '100%',
        filter: true,
        single: true
    });

    $('#fEmpresa').multipleSelect({
        width: '100%',
        filter: true,
        single: true
    });

    $('#fUbicacion').multipleSelect('setSelects', ['0']);
    $('#fEmpresa').multipleSelect('setSelects', ['0']);
    
    consultar();

});
function buscarCeldaAcoplamiento() {
    consultar();
}

$.fn.dataTable.ext.type.order['date-dd-mm-yyyy-pre'] = function (date) {

    if (!date) return 0;

    let parts = date.trim().split(' ');
    let dateParts = parts[0].split('/');
    let timeParts = parts[1] ? parts[1].split(':') : ['00', '00'];

    let hour = parseInt(timeParts[0], 10);
    let minute = parseInt(timeParts[1], 10);
    console.log(new Date(dateParts[2], dateParts[1] - 1, dateParts[0], hour, minute).getTime())
    return new Date(dateParts[2], dateParts[1] - 1, dateParts[0], hour, minute).getTime();

};

let consultar = function () {

    let codigoId = $("#fCodigoId").val();
    let codigo = $("#fCodigo").val();
    let ubicacion = $("#fUbicacion").val();
    let empresa = $("#fEmpresa").val();
    let area = $("#fArea").val();
    let estado = $("#fEstado").val();   
    let incluirCalcular = 1;
    let tension = $("#fNivelTension").val();

    setTimeout(function () {

        $.ajax({
            type: 'POST',
            url: controlador + 'ListaCeldaAcoplamiento?codigoId=' + codigoId + "&codigo=" + codigo + "&ubicacion=" + ubicacion + "&empresa=" + empresa + "&area=" + area + "&estado=" + estado + "&incluirCalcular=" + incluirCalcular + "&tension=" + tension,
            success: function (evt) {
                $('#lista').html(evt);
                $('#tablaListado').dataTable({
                    "iDisplayLength": 25,
                    columnDefs: [
                        { type: 'date-dd-mm-yyyy', targets: 10 }
                    ],
                    language: {
                        info: 'Mostrando página _PAGE_ de _PAGES_',
                        infoEmpty: 'No hay registros disponibles',
                        infoFiltered: '(filtrado de _MAX_ registros totales)',
                        lengthMenu: 'Mostrar _MENU_ registros por página',
                        zeroRecords: 'No se encontró nada'
                    },
                    order: [[4, 'asc'], [1, 'asc']]
                });

                $('.dataTables_filter input').attr('maxLength', 50);
            },
            error: function () {
                mostrarError();
            }
        });
    }, 100);
};

function incluir() {
    window.location.href = controlador + "BuscarCeldaAcoplamiento";
};

function consultarEquipo(codigoId) {
    window.location.href = siteRoot + "Evaluacion/Transversal/IndexConsultarEquipo?codigoId=" + codigoId + "&pathReturn=Evaluacion-CeldaAcoplamiento-Index";
};

function historialCambio(codigoId) {
    window.location.href = siteRoot + "Evaluacion/Transversal/IndexHistorialCambio?codigoId=" + codigoId + "&pathReturn=Evaluacion-CeldaAcoplamiento-Index";
};

let mostrarError = function () {
    $('#mensaje').removeClass("action-exito");
    $('#mensaje').addClass("action-error");
    $('#mensaje').text("Ha ocurrido un error");
    $('#mensaje').css("display", "block");
};

function incluirProyecto(e) {

    $.ajax({
        type: 'POST',
        url: controlador + "Incluir?id=" + e,
        success: function (evt) {

            $('#incluir').html(evt);
            setTimeout(function () {
                $('#popUpIncluir').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
            $("#btnCerrarIncluir").click(function () {
                $("#popUpIncluir").bPopup().close();
            });
            $("#btnSeleccionar").click(function () {
                let idProyecto = $("#idProyecto").val();
                if (idProyecto == '') {
                    alert("Seleccione un proyecto")
                    return;
                }
                window.location.href = controlador + "IncluirModificar?idCeldaAcoplamiento=" + e + "&idProyecto=" + idProyecto + "&idEquipo=" + e + "&accion=Modificar";

            });

        },
        error: function () {
            mostrarError();
        }
    });
};

function muestraPopupImportarDatos() {
    $("#txtNombreArchivo").val('');
    $("#idTerrores").html("");

    document.getElementById('btnSiguiente').style.display = '';
    document.getElementById('btnCerrar').style.display = 'none';

    document.getElementById("tblCarga").style.display = "";
    document.getElementById("idTerrores").style.display = "none";
    document.getElementById("idExito").style.display = "none";

    document.getElementById("tituloInicio").innerText = 'Importar Datos | Paso 1 / 2';

    $("#popupCargaMasiva").bPopup({
        autoClose: false
    });
};

function descargarPlantilla() {

    $.ajax({
        type: 'POST',
        url: controlador + 'ExportarPlantilla',
        dataType: 'json',
        success: function (evt) {
            if (evt.Resultado != "-1") {
                window.location = controlador + "AbrirArchivo?file=" + evt.NombreArchivo;
            }
            else {
                alert(evt.StrMensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error de exportación');
        }
    });
};

function crearPupload() {

    let fecha = new Date();
    let formatofecha = formatDate(fecha);
    let uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnCargarArchivo',
        multipart_params: {
            "fecha": formatofecha
        },
        container: document.getElementById('container'),
        url: siteRoot + 'evaluacion/linea/upload',
        flash_swf_url: '~/Content/Scripts/Moxie.swf',
        silverlight_xap_url: '~/Content/Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '10mb',
            mime_types: [
                { title: "Archivos Excel .xlsx", extensions: "xlsx,xls" }
            ]
        },

        init: {
            PostInit: function () {
            },
            FilesAdded: function (up, files) {
                if (uploader.files.length == 2) {
                    uploader.removeFile(uploader.files[0]);
                }
                up.refresh();
                up.start();
            },
            UploadComplete: function (up, files) {

                plupload.each(files, function (file) {
                    let ext = file.name.split(".");
                    document.getElementById('txtNombreArchivo').value = ext[0] + '_' + formatofecha + '.' + ext[1];
                });

                document.getElementById('btnSiguiente').disabled = false;
            },
            Error: function (up, err) {
                alert("El archivo a importar no tiene el formato requerido [.xlsx]");
            }
        }
    });

    uploader.init();
};

function leerExcelSubido() {

    if ($('#txtNombreArchivo').val() == '') {
        alert('Debe seleccionar un archivo');
        return;
    }

    $.ajax({
        type: 'POST',
        url: controlador + 'LeerExcelSubido',
        dataType: 'json',
        async: false,
        success: function (respuesta) {
            document.getElementById('btnSiguiente').style.display = 'none';
            document.getElementById('btnCerrar').style.display = '';

            document.getElementById("tituloInicio").innerText = 'Importar Datos | Paso 2 / 2';

            if (!respuesta.Exito) {


                if (respuesta.RegistrosObservados > 0) {

                    document.getElementById("tblCarga").style.display = "none";
                    document.getElementById("idTerrores").style.display = "";
                    setTimeout(function () {
                        $('#idTerrores').html(dibujarTablaError(respuesta.ListaErroresCeldasAcoplamiento));

                        $('#tablaError').dataTable({
                            "scrollY": 250,
                            "scrollX": false,
                            "sDom": 't',
                            "ordering": false,
                            "bPaginate": false,
                            "iDisplayLength": -1
                        });

                    }, 200);
                } else if (respuesta.RegistrosProcesados == 0) {
                    document.getElementById("tblCarga").style.display = "none";
                    document.getElementById("idTerrores").style.display = "none";
                    document.getElementById("idExito").style.display = "";

                    document.getElementById("mensajeExito").innerText = 'No hubo registros a procesar en el archivo. Revisar Plantilla.';

                    if (respuesta.Mensaje != '') {
                        document.getElementById("mensajeExito").innerText = respuesta.Mensaje;
                    }
                }

            } else {
                document.getElementById("tblCarga").style.display = "none";
                document.getElementById("idTerrores").style.display = "none";
                document.getElementById("idExito").style.display = "";

                document.getElementById("mensajeExito").innerText = 'Archivo procesado, se han importado ' + respuesta.RegistrosProcesados + ' registro(s)';

            }
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
};


function formatDate(date) {
    return date.getFullYear().toString() +
        String(date.getMonth() + 1).padStart(2, '0') +
        String(date.getDate()).padStart(2, '0') +
        String(date.getHours()).padStart(2, '0') +
        String(date.getMinutes()).padStart(2, '0') +
        String(date.getSeconds()).padStart(2, '0');
}


function dibujarTablaError(listErrores) {
    let totalErrores = listErrores.length;
    let cadena = "";
    cadena += "<div style='text-align: center;'><b>Lo sentimos, se encontraron " + totalErrores + " inconsistencia(s) que impiden que la importación se complete. </b></div> <br />";
    cadena += "<div style='text-align: center;'>Recuerde que todos los datos deben estar correctos para que la importación termine con éxito. </div>";
    cadena += "<div style='clear:both; height:5px'></div> ";
    cadena += "<table id='tablaError' border='1' class='pretty tabla-adicional' cellspacing='0'>";
    cadena += "<thead><tr><th>Fila Excel</th><th>Inconsistencia</th></tr></thead>";
    cadena += "<tbody>";
    for (let i = 0; i < totalErrores; i++) {
        cadena += "<tr><td>" + listErrores[i].Item + "</td>";
        cadena += "<td style='word-wrap: break-word; white-space: normal;'>" + listErrores[i].Error + "</td></tr>";
    }
    cadena += "</tbody></table>";
    return cadena;
};

let exportarDatos = function () {

    let fCodigoId = $("#fCodigoId").val();
    let fCodigo = $("#fCodigo").val();

    let idSubestacion = obtenerValorMultiselect('fUbicacion');
    let idEmpresa = obtenerValorMultiselect('fEmpresa');

    let idArea = $("#fArea").val();
    let estado = $("#fEstado").val();
    let nivelTension = $("#fNivelTension").val();
    setTimeout(function () {
        $.ajax({
            type: 'POST',
            url: controlador + 'ExportarDatos',
            data: {
                equicodi: fCodigoId,
                codigo: fCodigo,
                emprcodi: idEmpresa,
                equiestado: estado,
                idsubestacion: idSubestacion,
                idarea: idArea,
                tension: nivelTension
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    window.location = controlador + "AbrirArchivo?file=" + evt.NombreArchivo;
                }
                else {
                    alert(evt.StrMensaje);
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }, 100);
};


function confirmarExcluir(id) {
    setTimeout(function () {
        $('#popupExcluirEquipo').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown'
        });
    }, 50);

    $("#IdEquipo").val(id);

    $('#btnExcluir').click(function () {
        const idEquipo = $("#IdEquipo").val();
        excluirEquipo(idEquipo);
    });

    $('#btnCerrarExcluir').click(function () {
        $("#popupExcluirEquipo").bPopup().close();
    });
};

function excluirEquipo(id) {

    $.ajax({
        type: 'POST',
        url: controlador + "ExcluirEquipo",
        data: {
            id: id
        },
        success: function (resultado) {
            $("#popupExcluirEquipo").bPopup().close();
            alert("La Celda de Acoplamiento fue excluida correctamente.");
            consultar();

        },
        error: function () {
            mostrarError();
        }
    });
};

//Permite obtener el valor de los multiselect por ID
function obtenerValorMultiselect(id) {
    let array = $('#' + id).val();
    let valor;

    if (array != null && array != "") {
        valor = array[0].toString();
    } else {
        valor = 0;
    }

    return valor;
}