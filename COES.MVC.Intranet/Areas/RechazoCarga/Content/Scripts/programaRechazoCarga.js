var controlador = siteRoot + "rechazocarga/ProgramaRechazoCarga/";
var uploader;
var totCampoVacio = 0;
var totNoNumero = 0;
var totValorNegatico = 0;
var totFormatoHora = 0;

var listErrores = [];
var listDescripErrores = ["Campo vacío", "No es número", "Valor negativo", "Fecha Hora Fin anterior a Fecha Hora Inicio", "Hora no tiene formato correcto.", "Hora fin debe ser mayor a hora inicio."];

var validaInicial = true;
var hot;
var hotOptions;
var evtHot;

var hot2 = null;
var arregloDistrib = [];
var empresasSeleccionadasBuscador = [];

$(function () {
    $('#fechahoraInicio').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime",
        hourFormat: "24"
    });

    $('#fechahoraInicio').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#fechahoraInicio').val(date);
        }
    });
    
    $('#horaInicio').inputmask({
        mask: "h:s",
        placeholder: "hh:mm",
        alias: "datetime",
        hourFormat: "24"
    });

    $('#horaFin').inputmask({
        mask: "h:s",
        placeholder: "hh:mm",
        alias: "datetime",
        hourFormat: "24"
    });

    $('#fechaHoraInicioEjecucion').inputmask({
        mask: "1/2/y h:s",
        placeholder: "dd/mm/yyyy hh:mm",
        alias: "datetime",
        hourFormat: "24"
    });

    $('#fechaHoraInicioEjecucion').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#fechaHoraInicioEjecucion').val(date + " 00:00");
        }
    });

    $('#fechaHoraFinEjecucion').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#fechaHoraFinEjecucion').val(date + " 00:00");
        }
    });

    $('#fechaHoraFinEjecucion').inputmask({
        mask: "1/2/y h:s",
        placeholder: "dd/mm/yyyy hh:mm",
        alias: "datetime",
        hourFormat: "24"
    });

    $('#btnPopupEjecucion').click(function () {
        muestraRegistrarEjecucion();
    });

    $('#btnGrabarEjecucion').click(function () {
        grabarCuadroEjecucion();
    });

    $('#btnBuscarEmpresa').click(function () {
        muestraBuscarEmpresa();
    });

    $('#btnGrabar').click(function () {
        $('#tab-container').easytabs('select', "#tabClienteLibre");
        enviarDatos();
    });

    $('#btnMostrarErrores').click(function () {
        mostrarErrores(true);
    });

    $('#btnBuscar').click(function () {
        buscarEmpresa();
    });

    $('#btnAgregar').click(function () {
        agregarEmpresas();
    });

    $("#btnDescargarFormato").click(function () {
        descargarFormato();
    });

    $("#btnCerrar").click(function () {
        muestraGeneracionCuadroRechazoCarga();
    });

    //$("#btnEliminar").click(function () {
    //    eliminarCargasCreadas();
    //});

    $('#zona').change(function () {
        obtenerSubestacion();
        //obtenerSubestaciones();
    });

    $('input:radio[name=EsNuevoPrograma]').change(function () {
        esNuevoProgramaCambio();
    });

    $("#programa").change(function () {
        cargarDatosPrograma();
    });

    $("#btnSeleccionar").click(function () {
        seleccionarHojaCompleta();
    });

    $('#configuracion').change(function () {
        HabilitarOpcionCalcular();
    });

    $("#btnCalcular").click(function () {
        $('#tab-container').easytabs('select', "#tabClienteLibre");
        obtenerCalculoRechazoCarga();
    });    
   
    $('#btnReprogramar').click(function () {
        grabarReprogramacion();
    });

    $('#btnGrabarReprograma').click(function () {
       
        grabarReprogramacion();
    });       

    configurarAccionBotonEliminarSeleccionado();
    crearPupload();
    validarCargaInicialPrograma();
    cargarDatosCuadroPrograma();
    HabilitarOpcionCalcular();
    //obtenerSubestaciones();
    $("#SeccionSeleccionNuevoExistente").hide();
    HabilitarOpciones();
    $('#tab-container').easytabs({
        animate: false
    });

    $('#btnReporteExcel').click(function () {
        generarReporteExcel();
    });
    $('#btnReporteWord').click(function () {
        generarReporteWord();
    });

    consultarDistribuidores();

    $('#subestacion').multipleSelect({
        width: '210px',
        filter: true,
        onCheckAll: function () {
            $('#hdnTodasSubestaciones').val('1');
        },
        onUncheckAll: function () {
            $('#hdnTodasSubestaciones').val('0');
        }
    });

    $('#subestacion').multipleSelect('checkAll');
});

var muestraGeneracionCuadroRechazoCarga = function () {
    window.location.href = siteRoot + "rechazocarga/generacioncuadrosrechazocarga/Index";
}

function configurarAccionBotonEliminarSeleccionado() {
    var bototnEliminarSeleccionado = document.querySelector('#btnEliminarSeleccionado');
    bototnEliminarSeleccionado.addEventListener('click', function () {
        //debugger;
        var sel = hot.getSelected();
        if (sel != undefined) {
            var filaMinima = 2;
            var filaInicio = sel[0];
            if (filaMinima > filaInicio) { filaInicio = filaMinima; }
            var filaFin = sel[2];
            if (filaFin >= filaInicio) {
                var filaTemp = filaInicio;
                var idEliminado = '';
                var clientessEliminados = $("#hdnClientesEliminados").val();                
               
                while (filaTemp <= filaFin) {
                    idEliminado = idEliminado +  hot.getDataAtCell(filaInicio, 18) + ',';
                    hot.alter('remove_row', filaInicio, 1);
                    filaTemp++;
                }
                clientessEliminados = clientessEliminados + idEliminado;
                $("#hdnClientesEliminados").val(clientessEliminados);
               
                ocultarMensaje();
            } else {
                mostrarError("No hay filas de datos seleccionados");
            }
        }
    });

}

function eliminarCargasCreadas() {
    var codigoPrograma = $("#hdnCodigoPrograma").val();
    var codigoCuadroPrograma = $("#hdnCodigoCuadroPrograma").val();
    codigoCuadroPrograma = codigoCuadroPrograma == '' ? 0 : parseInt(codigoCuadroPrograma);

    if (confirm("¿Desea eliminar datos?. Se eliminará los datos permanentemente.")) {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: controlador + "EliminarProgramaRechazoCarga",
            data: {
                datos: JSON.stringify(hot.getData()),
                codigoPrograma: codigoPrograma,
                codigoCuadroPrograma: codigoCuadroPrograma
            },
            beforeSend: function () {
                mostrarAlerta("Enviando Información ..");
            },
            success: function (evt) {
                mostrarMensaje("Información eliminada correctamente.");
                var empresas = $("#empresasSeleccionadas").val();
                obtenerProgramaRechazoCarga(empresas, true);
            },
            error: function () {
                mostrarError("Ocurrió un error");
            }
        });
    }
}

function seleccionarHojaCompleta() {
    if (hot != undefined) {
        hot.selectCell(0, 0, hot.countRows() - 1, hot.countCols() - 1);
    }
}

function cargarDatosPrograma() {
    var codigoPrograma = $("#programa").val();
}

function cargarDatosCuadroPrograma() {
    var codigoCuadroPrograma = $("#hdnCodigoCuadroPrograma").val();
    if (codigoCuadroPrograma > 0) {
        obtenerModelo('', true);
    }
}

function validarCargaInicialPrograma() {
    var esNuevo = $("#hdnEsNuevoPrograma").val();
    var codigoPrograma = $("#hdnCodigoPrograma").val();
    var codigoCuadroPrograma = $("#hdnCodigoCuadroPrograma").val();

    if (esNuevo == "0") {
        $("#ProgramaExistente").attr('checked', true);
        $("#ProgramaNuevo").attr('checked', false);
        $("#programa").val(codigoPrograma);
    };

    if (esNuevo == "1") {
        $("#ProgramaExistente").attr('checked', false);
        $("#ProgramaNuevo").attr('checked', true);
    };

    esNuevoProgramaCambio();
}

function esNuevoProgramaCambio() {
    var esNuevo = $("input:radio[name=EsNuevoPrograma]:checked").val();
    if (esNuevo == "0") {
        $("#btnEliminar").show();
        $("#SeccionProgramaExistente").show();
        $("#SeccionProgramaNuevo").hide();
    }

    if (esNuevo == "1") {
        $("#btnEliminar").hide();
        $("#SeccionProgramaExistente").show();
        $("#SeccionProgramaNuevo").hide();
    }
}

function crearPupload() {
    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectExcel',
        url: siteRoot + 'RechazoCarga/ProgramaRechazoCarga/Subir',
        flash_swf_url: 'Scripts/Moxie.swf',
        silverlight_xap_url: 'Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '5mb',
            mime_types: [
                { title: "Archivos Excel .xlsx", extensions: "xlsx,xls" }
            ]
        },
        init: {
            FilesAdded: function (up, files) {
                if (uploader.files.length == 2) {
                    uploader.removeFile(uploader.files[0]);
                }
                uploader.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarAlerta("Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            UploadComplete: function (up, file) {
                mostrarAlerta("Subida completada <strong>Por favor espere</strong>");
                leerExcelSubido();
                limpiarError();
            },
            Error: function (up, err) {
                mostrarError(err.code + "-" + err.message);
            }
        }
    });
    uploader.init();
}

function leerExcelSubido() {
    var bloqueHorario = $('input:radio[name=tipoEnergia]:checked').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'LeerExcelSubido',
        dataType: 'json',
        async: false,
        data: {
            bloqueHorario: bloqueHorario
        },
        success: function (respuesta) {
            if (respuesta.Exito == false) {
                mostrarError(respuesta.Mensaje);
            } else {
                if (typeof hot != 'undefined') {
                    hot.destroy();
                }
                crearHandsonTable(respuesta.Datos, true);
                evtHot = respuesta.Datos;
                mostrarExito("Archivo importado");
            }
        },
        error: function () {
            mostrarError("Ha ocurrido un error");
        }
    });
}

function limpiarError() {
    var totCampoVacio = 0;
    var totNoNumero = 0;
    var totValorNegatico = 0;
    var listErrores = [];
}

function descargarFormato() {
    mostrarErrores(false);
    var codigoCuadroPrograma = $("#hdnCodigoCuadroPrograma").val();
    //var subestacion = $("#subestacion").val();
    var demandaMinima = $("#demandaMinima").val();
    var nombreEmpresa = $("#nombreEmpresa").val();
    var bloqueHorario = $('input:radio[name=tipoEnergia]:checked').val();

    if (hot == undefined) {
        return false;
    }

    setTimeout(function () {
        if (listErrores.length > 0) {
            activarPopupErrores();
        } else {
            $.ajax({
                type: 'POST',
                url: controlador + 'GenerarFormato',
                dataType: 'json',
                data: {
                    bloqueHorario: bloqueHorario,
                    datos: JSON.stringify(hot.getData()),
                    codigoCuadroPrograma: codigoCuadroPrograma,
                    esConsulta: false
                },
                success: function (result) {
                    if (result == "1") {
                        window.location = controlador + 'DescargarFormato';
                    }
                    else {
                        alert(result);
                    }
                },
                error: function () {
                    alert("Error");
                }
            });
        }
    }, 100);

}

var table;
function buscarEmpresa() {
    //valida si la fecha de inicio tiene datos
    var fechahoraInicio = $("#fechahoraInicio").val();
    if (fechahoraInicio.trim() == '') {
        mostrarAlerta('Debe ingresar fecha y hora inicio');
        return false;
    }
    var partesAnioHoraMin = fechahoraInicio.split(" ");
    var fecha = partesAnioHoraMin[0];

    var lista = obtenerListaEmpresasGrilla();
    var zona = $('#zona').val();    
    //var estacion = $('#subestacion').val();
    var estacion = $('#subestacion').multipleSelect('getSelects');
    //alert(estacion);
    if (estacion == "[object Object]") {
        estacion = "";
    }
    $('#hdnSubestacion').val(estacion);
    //if (empresa == "") empresa = "-1";
    var medicion = $('#demandaMinima').val();
    var empresa = $('#nombreEmpresa').val();
    var bloqueHorario = $('input:radio[name=tipoEnergia]:checked').val();
    var periodoAntiguedad = $('#hdnPeriodosAntiguedad').val();

    var tipoBusqueda = $('input:radio[name=tipoBusqueda]:checked').val();    

    var subEstacionesSeleccionadas = 0;
    var totalSubestaciones = $('#hdnTotalSubestaciones').val();

    if ($('#hdnSubestacion').val() != '') {
        subEstacionesSeleccionadas = $('#hdnSubestacion').val().split(',').length;

        if (subEstacionesSeleccionadas == totalSubestaciones) {
            $('#hdnTodasSubestaciones').val('1');
        } else if (subEstacionesSeleccionadas > 1000) {
            alert('Debe seleccionar una cantidad menor a 1000 de subestaciones');
            return;
        } else {
            $('#hdnTodasSubestaciones').val('0');
        }

    }

    $.ajax({
        type: 'POST',
        datatype: 'json',
        url: controlador + "ListarEmpresas",
        data: {
            fechaInicio: fecha,
            bloqueHorario: bloqueHorario,
            zona: zona,
            estacion: $('#hdnSubestacion').val(),
            medicion: medicion,
            empresa: empresa,
            empresaequipoExcluir: lista,
            periodoAntiguedad: periodoAntiguedad,
            tipoBusqueda: tipoBusqueda,
            marcaTodasSubestaciones: $('#hdnTodasSubestaciones').val()
        },
        success: function (result) {
            $('#listadoEmpresas').css("width", "1000px");
            $('#listadoEmpresas').html(result);
            table = $('#tablaListaEmpresas').dataTable({
                "filter": false,
                "ordering": true,
                "searching": false,
                "paging": false,
                "scrollY": 250,
                "scrollX": false,
                "bDestroy": true,
                "columnDefs": [
                     {
                         "targets": 0,
                         'searchable': false,
                         'orderable': false,
                         'render': function (data, type, full, meta) {
                             return '<input type="checkbox" name="id[]" value="' + $('<div/>').text(data).html() + '">';
                         }
                    },
                    {
                        render: function (data, type, full, meta) {
                            return "<div class='text-wrap width-200'>" + data + "</div>";
                        },
                        targets: [1,2,4]
                    }

                ],
                "select": {
                    "style": "multi"
                },
                "order": [[1, "asc"]]
            });
            $('#SeleccionarTodo').on('change', function () {
                var rows, checked;
                rows = $('#tablaListaEmpresas').find('tbody tr');
                checked = $(this).prop('checked');
                $.each(rows, function () {
                    var checkbox = $($(this).find('td').eq(0)).find('input').prop('checked', checked);
                });
            });


        },
        error: function () {
            alert("Error al cargar la Lista de Empresas.");
        }
    });
}
function agregarEmpresas() {
    //debugger;
    empresasSeleccionadasBuscador = [];
    
    //var lista = obtenerListaEmpresasGrilla();
    var lista = '';
    $("input:checked", table.fnGetNodes()).each(function () {
        lista = lista + $(this).val() + ';';
        
    });

    if (lista == '') {
        alert('Debe seleccionar al menos una empresa.');
        return false;
    }    

    //Luego de seleccionar se debe de buscar por la empresa(s) seleccionadas y cerrar popup
    $("#popupSeleccionarEmpresas").bPopup().close();
    obtenerProgramaRechazoCarga(lista, false);
}
function obtenerListaEmpresasGrilla() {
    var lista = "";
    if (hot == undefined) return lista;
    var datos = hot.getData();
    var filas = datos.length;
    if (filas <= 2) return lista;
    for (i = 2; i < filas; i++) {
        lista = lista + datos[i][18] + "/" + datos[i][19] + ",";
    }
    return lista;
}
function muestraBuscarEmpresa() {
    $('#zona').val('0');    
    $('#nombreEmpresa').val('');
    $("#listadoEmpresas").html("");
    $('#hdnTodasSubestaciones').val('1');
    //$('#subestacion').multipleSelect('checkAll');

    obtenerSubestacion();

    $("#popupSeleccionarEmpresas").bPopup({
        autoClose: false
    });
}

function muestraRegistrarEjecucion() {   
    $("#popupRegistrarEjecucion").bPopup({
        autoClose: false
    });
}
function muestraPopupReprogramar() {
    $("#popupReprogramar").bPopup({
        autoClose: false
    });
}
function obtenerProgramaRechazoCarga(listaEmpresas, esConsulta) {
    

    obtenerModelo(listaEmpresas, esConsulta);
}
function obtenerModelo(listaEmpresas, esConsulta) {
    //debugger;
    var dataInicial = null;
    if (typeof hot != 'undefined') {
        dataInicial = JSON.stringify(hot.getData());
        hot.destroy();
    }
    var codigoCuadroPrograma = $("#hdnCodigoCuadroPrograma").val();
    var bloqueHorario = $('input:radio[name=tipoEnergia]:checked').val();

    var fechahoraInicio = $("#fechahoraInicio").val();   
    var partesAnioHoraMin = fechahoraInicio.split(" ");
    var fecha = partesAnioHoraMin[0];
    var periodoAntiguedad = $("#hdnPeriodosAntiguedad").val();
    var tipoBusqueda = $('input:radio[name=tipoBusqueda]:checked').val();

    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerFormatoModelProgramaRechazoCarga",
        dataType: 'json',
        data: {
            datosIniciales: dataInicial,
            fechaInicio: fecha,
            bloqueHorario: bloqueHorario,            
            empresasSeleccionadas: listaEmpresas,
            codigoCuadroPrograma: codigoCuadroPrograma,
            esConsulta: esConsulta,
            periodoAntiguedad: periodoAntiguedad,
            tipoBusqueda: tipoBusqueda
        },
        success: function (modelo) {            
            crearHandsonTable(modelo, false);
            evtHot = modelo;
            $("#empresasSeleccionadas").val(listaEmpresas);
        },
        error: function () {
            alert("Error al cargar Excel Web");
        }
    });
}
function mostrarErrores(activar) {
    totCampoVacio = 0;
    totNoNumero = 0;
    totValorNegatico = 0;
    totFormatoHora = 0;
    validaInicial = true;
    listErrores.splice(0, listErrores.length);
    validarCuadroPrograma();
    validarRegistros(evtHot);
    if (activar) {
        activarPopupErrores();
    }
}

function activarPopupErrores() {
    setTimeout(function () {
        $('#idTerrores').html(dibujarTablaError());

        $('#validaciones').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
        $('#tablaError').dataTable({
            "scrollY": 330,
            "scrollX": true,
            "sDom": 't',
            "ordering": false,
            "bPaginate": false,
            "iDisplayLength": -1
        });

    }, 200);
}

function validarRegistros(data) {
    for (var row = 0; row < data.Handson.ListaExcelData.length; row++) {
        for (var col = 0; col < data.Handson.ListaExcelData[0].length; col++) {
            if ((row >= data.FilasCabecera) && (col <= data.ColumnasCabecera) && (!data.Handson.ReadOnly)) {
                if (!data.Handson.ListaFilaReadOnly[row]) {
                    var value = data.Handson.ListaExcelData[row][col];
                    var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();

                    if (col == 0) {
                        if (value == null || value == "") {
                            agregarError(celda, value, 0);
                        }
                    }
                    if (col == 13 || col == 14 || col == 16 || col == 17) {
                        if (value != null && value != "") {
                            
                            if (!validarFormatoHora(value)) {                                
                                agregarError(celda, value, 4);
                            }

                            if (col == 14 || col == 17) {
                                var fechaInicio = '01/01/2010';
                                var horaInicio = data.Handson.ListaExcelData[row][col - 1];
                                var horaFin = value;
                                var fechahoraInicio = fechaInicio + ' ' + horaInicio;
                                var fechahoraFin = fechaInicio + ' ' + horaFin;
                                var fechahoraInicioFormateada = formatearFecha(fechahoraInicio);
                                var fechahoraFinFormateada = formatearFecha(fechahoraFin);
                                if (fechahoraFinFormateada < fechahoraInicioFormateada) {
                                    agregarError(celda, value, 5);
                                }
                            }
                        }
                        continue;
                    }
                    if (col >= 6) {

                        if (col == 12 || col == 15) {
                            if (value != null && value != "") {
                                if (!validarDecimal(value)) {
                                    agregarError(celda, value, 1);
                                } else {
                                    if (Number(value) < 0) {
                                        agregarError(celda, value, 2);
                                    }
                                }
                            }
                            continue;
                        }

                        if (!validarDecimal(value)) {
                            agregarError(celda, value, 1);
                        } else {
                            if (Number(value) < 0) {
                                agregarError(celda, value, 2);
                            }
                        }
                    }
                }
            }
        }
    }
}

function validarFormatoHora(horaComparar) {
    const timeReg = /^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$/;

    if (!horaComparar.match(timeReg)) {        
        return false;
    } else {        
        return true;
    }
}

function validarCuadroPrograma() {

    var fechaInicio = $("#fechahoraInicio").val();
    var horaInicio = $("#horaInicio").val();
    var horaFin = $("#horaFin").val();
    var fechahoraInicio = fechaInicio + ' ' + horaInicio;
    var fechahoraFin = fechaInicio + ' ' + horaFin;
    var fechahoraInicioFormateada = formatearFecha(fechahoraInicio);
    var fechahoraFinFormateada = formatearFecha(fechahoraFin);
    if (fechahoraFinFormateada < fechahoraInicioFormateada) {
        var regError = {
            Celda: "",
            Valor: fechahoraFin,
            Tipo: 3
        };
        listErrores.push(regError);
    }
};

function formatearFecha(cadena) {
    var partes = cadena.split("/");
    var partesAnioHoraMin = partes[2].split(" ");
    var partesHoraMin = partesAnioHoraMin[1].split(":");
    var dia = partes[0];
    var mes = parseInt(partes[1]) - 1;
    var anio = partesAnioHoraMin[0];
    var hora = partesHoraMin[0];
    var minuto = partesHoraMin[1];
    var fecha = new Date(anio, mes, dia, hora, minuto, 0, 0);
    return fecha;
};

function dibujarTablaError() {
    var totalErrores = listErrores.length;
    var valorLimite = 50;
    var totalErroresMostrar = totalErrores > valorLimite ? valorLimite : totalErrores;
    var cadena = "";
    if (totalErrores > valorLimite) {
        cadena += "<div>Se encontraron " + totalErrores + " errores</div>";
    }
    cadena += "<div style='clear:both; height:5px'></div> ";
    cadena += "<table id='tablaError' border='1' class='pretty tabla-adicional' cellspacing='0'>";
    cadena += "<thead><tr><th>Celda</th><th>Valor</th><th>Error</th></tr></thead>";
    cadena += "<tbody>";
    for (var i = 0 ; i < totalErroresMostrar ; i++) {
        cadena += "<tr><td>" + listErrores[i].Celda + "</td>";
        cadena += "<td>" + listErrores[i].Valor + "</td>";
        cadena += "<td>" + listDescripErrores[listErrores[i].Tipo] + "</td></tr>";
    }
    cadena += "</tbody></table>";
    return cadena;
}

function getExcelColumnName(pi_columnNumber) {
    var li_dividend = pi_columnNumber;
    var ls_columnName = "";
    var li_modulo;

    while (li_dividend > 0) {
        li_modulo = (li_dividend - 1) % 26;
        ls_columnName = String.fromCharCode(65 + li_modulo) + ls_columnName;
        li_dividend = Math.floor((li_dividend - li_modulo) / 26);
    }

    return ls_columnName;
}

validarDecimal = function (n) {
    if (n == "")
        return false;

    var count = 0;
    var strCheck = "0123456789.-E";
    var i;

    for (i in n) {
        if (strCheck.indexOf(n[i]) == -1)
            return false;
        else {
            if (n[i] == '.') {
                count = count + 1;
            }
        }
    }
    if (count > 1) return false;
    return true;
}

function agregarError(celda, valor, tipo) {
    if (validarError(celda)) {
        var regError = {
            Celda: celda,
            Valor: valor,
            Tipo: tipo
        };

        listErrores.push(regError);
        switch (tipo) {
            case 0:
                totCampoVacio++;
                break;
            case 1:
                totNoNumero++;
                break;
            case 2:
                totValorNegatico++;
                break;
            case 4:
                totFormatoHora++;
                break;
        }
    }
}

function validarError(celda) {
    for (var j in listErrores) {
        if (listErrores[j]['Celda'] == celda) {
            return false;
        }
    }
    return true;
}

function enviarDatos() {
    mostrarErrores(false);

    //mostrarErrores(false);    
    var codigoPrograma = $("#hdnCodigoPrograma").val();
    var codigoCuadroPrograma = $("#hdnCodigoCuadroPrograma").val();

    //datos programa
    var esNuevoPrograma = $("input:radio[name=EsNuevoPrograma]:checked").val();
    var horizonte = $("#horizonte").val();
    var programaAbrev = $("#codigo").val();
    var nombrePrograma = $("#nombrePrograma").val();

    //datos cuadro programa
    var energiaRacionar = $("#energiaRacionar").val();
    var demandaMinima = $("#demandaMinima").val();
    var motivo = $("#motivo").val();
    var fechahoraInicio = $("#fechahoraInicio").val();
    var horaInicio = $("#horaInicio").val();
    var horaFin = $("#horaFin").val();
    var bloqueHorario = $('input:radio[name=tipoEnergia]:checked').val();
    var ubicacion = $("#ubicacion").val();

    var programaSeleccionado = $("#programa").val();

    if (codigoPrograma == 0 && codigoCuadroPrograma > 0)
    {
        programaSeleccionado = 0;
    } else {
        if (programaSeleccionado == '0' || programaSeleccionado == '') {
            //mostrarAlerta('Debe seleccionar un programa');
            //return false;
        }
    }   

    codigoPrograma = programaSeleccionado;

    if (energiaRacionar.trim() == '') {
        mostrarAlerta('Debe ingresar energia a racionar');
        return false;
    }

    if (demandaMinima.trim() == '') {
        mostrarAlerta('Debe ingresar umbral');
        return false;
    }

    if (motivo.trim() == '') {
        mostrarAlerta('Debe ingresar motivo');
        return false;
    }

    if (ubicacion.trim() == '') {
        mostrarAlerta('Debe ingresar ubicación');
        return false;
    }

    if (fechahoraInicio.trim() == '') {
        mostrarAlerta('Debe ingresar fecha');
        return false;
    }
    if (horaInicio.trim() == '') {
        mostrarAlerta('Debe ingresar hora inicio');
        return false;
    }

    if (horaFin.trim() == '') {
        mostrarAlerta('Debe ingresar hora fin');
        return false;
    }

    var fechahoraFin = fechahoraInicio + ' ' + horaFin;
    fechahoraInicio = fechahoraInicio + ' ' + horaInicio;

    var fechahoraInicioFormateada = formatearFecha(fechahoraInicio);
    var fechahoraFinFormateada = formatearFecha(fechahoraFin);
    if (fechahoraFinFormateada < fechahoraInicioFormateada) {
        mostrarAlerta('Hora Fin debe ser mayor a Hora inicio');
        return false;
    }

    var datosDistrib = JSON.stringify(hot2.getData());
    var distribElim = $("#hdnDistribEliminados").val();
    var clienteElim = $("#hdnClientesEliminados").val();
    
    setTimeout(function () {
        if (listErrores.length > 0) {
            activarPopupErrores();
        } else {
            if (confirm("¿Desea enviar información a COES?")) {
                $.ajax({
                    type: 'POST',
                    dataType: 'json',
                    url: controlador + "GrabarProgramaRechazoCarga",
                    data: {
                        datos: JSON.stringify(hot.getData()),
                        datosDistribuidor: datosDistrib,
                        codigoPrograma: codigoPrograma,
                        codigoCuadroPrograma: codigoCuadroPrograma,
                        horizonte: horizonte,
                        programaAbrev: programaAbrev,
                        nombrePrograma: nombrePrograma,
                        energiaRacionar: energiaRacionar,
                        demandaMinima: demandaMinima,
                        bloqueHorario: bloqueHorario,
                        configuracion: $("#configuracion").val(),
                        motivo: motivo,
                        ubicacion: ubicacion,
                        fechahoraInicio: fechahoraInicio,
                        fechahoraFin: fechahoraFin,
                        flagEracmf: document.getElementById('incluirCargaEracmf').checked ? 1 : 0,
                        flagEracmt: document.getElementById('incluirCargaEracmt').checked ? 1 : 0,
                        flagUsuariosRegulados: document.getElementById('incluirUsuariosRegulados').checked ? 1 : 0,
                        distribEliminados: distribElim,
                        perfilUsuario: $("#hdnPerfil").val(),
                        clientesEliminados: clienteElim
                    },
                    beforeSend: function () {
                        mostrarAlerta("Enviando Información ..");
                    },
                    success: function (respuesta) {
                        if (respuesta.Exito) {
                            $("#hdnCodigoPrograma").val(respuesta.CodigoPrograma);
                            $("#hdnCodigoCuadroPrograma").val(respuesta.CodigoCuadroPrograma);
                            document.getElementById('incluirUsuariosRegulados').checked = respuesta.IncluyeUsuariosRegulados > 0 ? true : false;

                            mostrarMensaje("Información grabada correctamente.");
                            setTimeout(function () {
                                ocultarMensaje();
                                obtenerProgramaRechazoCarga('', true);
                                consultarDistribuidores();
                            }, 2000);

                            document.getElementById('hdnDistribEliminados').value = '';
                            document.getElementById('hdnClientesEliminados').value = '';
                            var perfil = $("#hdnPerfil").val();

                            if (respuesta.CodigoPrograma > 0 && perfil == '2') {
                                $("#programa").val(respuesta.CodigoPrograma);
                            }

                            //var empresas = $("#empresasSeleccionadas").val();
                        }
                    },
                    error: function () {
                        mostrarError("Ocurrió un error");
                    }
                });
            }
        }
    }, 100);
}

var container;
function crearHandsonTable(evtHot, validar) {
    function celdaNumerica(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.textAlign = 'right';
    }
    function horaRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.textAlign = 'right';
    }
    function tituloAzul(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.color = 'white';
        td.style.background = 'darkblue';
    }

    function tituloVerdeClaro(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.color = 'white';
        td.style.background = 'lightgreen';
    }

    function tituloVerdeOscuro(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.color = 'white';
        td.style.background = 'green';
    }

    function errorRowRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.color = 'black';
        td.style.background = '#FF4C42';
    }

    function cambiosCellRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.textAlign = 'right';
        td.style.background = '#FFFFD7';
        $(td).html(formatFloat(parseFloat(value), 3, '.', ','));
    }

    function negativeValueRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        if (parseInt(value, 10) < 0) {
            td.style.color = 'orange';
            td.style.fontStyle = 'italic';
        }
    }

    function limitesCellRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.textAlign = 'right';
        if (Number(value) && value != "") {
            if (Number(value) < evtHot.ListaHojaPto[col - 1].Hojaptoliminf) {
                td.style.background = 'orange';
                $(td).html(formatFloat(Number(value), 3, '.', ','));
                var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                agregarError(celda, value, 3);

            }
            else {
                if (Number(value) > evtHot.ListaHojaPto[col - 1].Hojaptolimsup) {
                    td.style.background = 'yellow';
                    $(td).html(formatFloat(Number(value), 3, '.', ','));
                    var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                    agregarError(celda, value, 4);
                }
                else {
                    td.style.background = 'white';
                    $(td).html(formatFloat(Number(value), 3, '.', ','));
                }
            }
        }
        else {
            if (value == "0")
                $(td).html("0.000");
            else if (value != "") {
                if (!Number(value)) {
                    td.style.background = 'red';
                    var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                    agregarError(celda, value, 2);
                }
            }
        }

    }

    function readOnlyValueRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.textAlign = 'left';
        //td.style.color = 'DimGray';
        //td.style.background = 'Gainsboro';
        td.style.color = 'black';
        td.style.background = 'white';
        if (parseFloat(value)){
            td.style.textAlign = 'right';
            //$(td).html(formatFloat(parseFloat(value), 3, '.', ','));
        }
        else {
            if (value == "0" || value == "0.00")
            {
                $(td).html("0.00");
                td.style.textAlign = 'right';
                //$(td).html("0.000");
            }
                
        }
    }

    function formatFloat(valor, numeroDecimales, separadorDecimalOriginal, separadorDecimalNuevo) {
        var redondeado = parseInt(Math.round(valor * Math.pow(10, numeroDecimales))) / Math.pow(10, numeroDecimales);
        var texto = redondeado.toString();
        var formateadoDecimal = texto.replace(separadorDecimalOriginal, separadorDecimalNuevo);
        return formateadoDecimal;
    }

    function calculateSize() {
        var offset;
        offset = Handsontable.Dom.offset(container);

        if (offset.top == 222) {
            availableHeight = $(window).height() - offset.top - 90;
        }
        else {
            availableHeight = $(window).height() - offset.top - 20;
        }
        //if (offset.left > 0)
        //    availableWidth = $(window).width() - 2 * offset.left; //$("#divGeneral").width() - 50; //
        if (offset.top > 0) {
            availableHeight = 500; //availableHeight < HEIGHT_MINIMO ? HEIGHT_MINIMO : availableHeight;
            //container.style.height = availableHeight + 'px';
        }
        container.style.height = availableHeight + 'px';
        hot.render();
    }

    container = document.getElementById('detalleFormato');
    Handsontable.renderers.registerRenderer('negativeValueRenderer', negativeValueRenderer);
    Handsontable.Dom.addEvent(window, 'resize', calculateSize);
    Handsontable.Dom.addEvent(container, 'click', function () {
        validaInicial = false;
    });

    hotOptions = {
        data: evtHot.Handson.ListaExcelData,
        height: 500,
        maxRows: evtHot.Handson.ListaExcelData.length,
        maxCols: evtHot.Handson.ListaExcelData[0].length,
        colHeaders: true,
        rowHeaders: true,
        fillHandle: true,
        columnSorting: false,
        className: "htCenter",
        readOnly: evtHot.Handson.ReadOnly,
        fixedRowsTop: evtHot.FilasCabecera,
        //fixedColumnsLeft: evtHot.ColumnasFijas,
        mergeCells: evtHot.Handson.ListaMerge,
        colWidths: evtHot.Handson.ListaColWidth,
        outsideClickDeselects: false,
        stretchH: 'all',
        contextMenu: false,
        afterLoadData: function () {
            this.render();
        },
        hiddenColumns: {
            columns: [ 18, 19, 20, 21],
            indicators: false
        },        
        beforeChange: function (changes, source) {
            for (var i = changes.length - 1; i >= 0; i--) {
                //var valorOld = changes[i][2];
                //var valorNew = changes[i][3];
                //var liminf = evtHot.ListaHojaPto[changes[i][1] - 1].Hojaptoliminf;
                //var limsup = evtHot.ListaHojaPto[changes[i][1] - 1].Hojaptolimsup;
                //var tipoOld = getTipoError(valorOld, liminf, limsup);//isNaN(changes[i][2]) ? true : false;
                //var tipoNew = getTipoError(valorNew, liminf, limsup); //isNaN(changes[i][3]) ? true : false;
                //var celda = getExcelColumnName(parseInt(changes[i][1]) + 1) + (parseInt(changes[i][0]) + 1).toString();
                //if (tipoOld > 1) {
                //    eliminarError(celda, tipoOld);
                //    if (tipoNew > 1) {
                //        agregarError(celda, changes[i][3], tipoNew);
                //    }
                //}
                //else {
                //    if (tipoNew > 1) {
                //        agregarError(celda, changes[i][3], tipoNew);
                //    }
                //}
            }
        },
        columns: [
            {},
            {},
            {},
            {},
            {},
            {},
            {},
            {},
            {},
            {},
            {},
            {},
            {},
            {
                type: 'time',
                timeFormat: 'HH:mm',
                correctFormat: true
            },
            {
                type: 'time',
                timeFormat: 'HH:mm',
                correctFormat: true
            },
            {},
            {
                type: 'time',
                timeFormat: 'HH:mm',
                correctFormat: true
            },
            {
                type: 'time',
                timeFormat: 'HH:mm',
                correctFormat: true
            },
            {},
            {},
            {},
            {}
        ],

        cells: function (row, col, prop) {
            var cellProperties = {};
            var formato = "";
            var render;
            var readOnly = true;
            var consulta = $("#hdnFlagConsulta").val();
            var edicionDemanda = $("#hdnFlagEditarDemanda").val();

            var tipo;
            if (row == 0) {
                if (col == 8 || col == 12 || col == 15) {
                    render = tituloAzul
                }

                readOnly = true;
            }
            if (row == 1) {
                render = tituloAzul;
                readOnly = true;
            }
            if (row > 1) {
                readOnly = col <= 7;                               

                if (col <= 6) {
                    render = readOnlyValueRenderer;                    

                } else if (col == 13 || col == 14 || col == 16 || col == 17)
                {
                    render = horaRenderer;
                }
                else {
                    if (col == 7 && edicionDemanda > 0) {
                        readOnly = false;
                    }
                    render = celdaNumerica;
                }
            }
            if (consulta == "1") {
                readOnly = true;
            }
            cellProperties = {
                renderer: render,
                format: formato,
                type: tipo,
                readOnly: readOnly
            }

            return cellProperties;
        }
    };

    hot = new Handsontable(container, hotOptions);
    //hot.render();
    calculateSize();

    if (validar == true) {
        validarRegistros(evtHot);
    }
}

function mostrarMensaje(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-message');
}

function mostrarError(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-error');
}

function ocultarMensaje() {
    $('#mensaje').removeClass();
    $('#mensaje').html("");
}
function soloNumeros(e) {
    var key = window.Event ? e.which : e.keyCode
    return (key <= 13 || (key >= 48 && key <= 57) || key == 46);
}
function mostrarAlerta(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-alert');
}

function mostrarExito(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-exito');
}

function obtenerSubestacion() {
    $("#subestacion").empty();
    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerSubEstacion',
        dataType: 'json',
        data: {
            codigoZona: $("#zona").val()
        },
        success: function (cuadros) {
            $('#subestacion').empty().multipleSelect('refresh');
            //$.each(cuadros, function (i, cuadro) {
            //    $("#subestacion").append('<option value="' + cuadro.Value + '">' +
            //         cuadro.Text + '</option>');
            //});
            var cbPrueba = document.getElementById("subestacion");
            for (i = 0; i < cuadros.length; i += 1) {

                var fila = cuadros[i];
                var option = document.createElement('option');
                option.value = fila.Value;
                option.text = fila.Text;
                cbPrueba.add(option);
            };

            $('#subestacion').multipleSelect('refresh');
            $('#subestacion').multipleSelect('checkAll');
        },
        error: function (ex) {
            mostrarError('Consultar cuadros por programa: Ha ocurrido un error');
        }
    });
};

function obtenerSubestaciones() {
    //$("#subestacion").empty();
    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerSubEstaciones',
        
        data: {
            codigoZona: $("#zona").val()
        },
        success: function (aData) {
            $('#subestaciones').html(aData);
        },
        error: function (ex) {
            mostrarError('Consultar cuadros por programa: Ha ocurrido un error');
        }
    });
};

function HabilitarOpcionCalcular() {
    var configuracion = $("#configuracion").val();
    var configuracionRadial = $("#hdnConfiguracionRadial").val();

    if (configuracion == configuracionRadial) {
        //alert('1');
        //$("#btnCalcular").prop('disabled', false);
        document.getElementById('btnCalcular').style.display = 'inline';
    } else {
        //alert('2');
        //$("#btnCalcular").prop('disabled', true);
        document.getElementById('btnCalcular').style.display = 'none';
    }
    var edicionDemanda = $("#hdnFlagEditarDemanda").val();

    if (edicionDemanda == 0) {
        document.getElementById('btnCalcular').style.display = 'none';
    }
};
function obtenerCalculoRechazoCarga() {
    mostrarErrores(false);
    var codigoCuadroPrograma = $("#hdnCodigoCuadroPrograma").val();

    var energiaRacionar = $("#energiaRacionar").val();
    var nombreEmpresa = $("#nombreEmpresa").val();
    var bloqueHorario = $('input:radio[name=tipoEnergia]:checked').val();

    if (hot == undefined) {
        return false;
    }

    setTimeout(function () {
        if (listErrores.length > 0) {
            activarPopupErrores();
        } else {
            $.ajax({
                type: 'POST',
                url: controlador + 'ObtenerCalculoRechazoCarga',
                dataType: 'json',
                data: {
                    datos: JSON.stringify(hot.getData()),
                    datosDistribuidor: JSON.stringify(hot2.getData()),
                    magnitudRechazoCarga: energiaRacionar,
                    incluyeEracmf: document.getElementById('incluirCargaEracmf').checked ? 1 : 0,
                    incluyeEracmt: document.getElementById('incluirCargaEracmt').checked ? 1 : 0,
                    bloqueHorario: bloqueHorario,
                    umbral: 0
                },
                success: function (result) {
                    crearHandsonTable(result.ModeloUsuariosLibres, false);
                    evtHot = result.ModeloUsuariosLibres;
                    if (result.MensajeDistribucion != '') {
                        alert(result.MensajeDistribucion);
                    } else if (result.CargarDistribuidores){
                        crearHandsonTableDistrib(result);
                    }

                    if (result.DistribuidoresEliminados != '') {
                        var distribuidoresEliminados = $("#hdnDistribEliminados").val();
                        distribuidoresEliminados = distribuidoresEliminados + result.DistribuidoresEliminados + ',';
                        $("#hdnDistribEliminados").val(distribuidoresEliminados);
                        
                    }
                    //$("#empresasSeleccionadas").val(listaEmpresas);
                },
                error: function () {
                    alert("Error al calcular rechazo carga");
                }
            });
        }
    }, 100);

}

function grabarReprogramacion() {
    mostrarErrores(false);

    var codigoReprograma = '';    

    var codigoCuadroPrograma = $("#hdnCodigoCuadroPrograma").val();
    var codigoPrograma = $("#hdnCodigoPrograma").val();

    var fechahoraInicio = $("#fechahoraInicio").val();
    if (fechahoraInicio.trim() == '') {
        mostrarAlerta('Debe ingresar fecha y hora inicio');
        return false;
    }

    setTimeout(function () {
        if (listErrores.length > 0) {
            activarPopupErrores();
        } else {
            if (confirm("¿Esta seguro de Reprogramar el cuadro?")) {
                $.ajax({
                    type: 'POST',
                    dataType: 'json',
                    url: controlador + "ReprogramarCuadroRechazoCarga",
                    data: {

                        codigoCuadroPrograma: codigoCuadroPrograma,
                        codigoPrograma: codigoPrograma,
                        codigoReprograma: codigoReprograma,
                        fechaInicio: fechahoraInicio
                    },
                    beforeSend: function () {
                        mostrarAlerta("Enviando Información ..");
                    },
                    success: function (respuesta) {
                        if (respuesta.Exito) {
                            var mensaje = 'Reprograma registrado correctamente.\n';
                            mensaje = mensaje + 'Vuelva a ingresar al cuadro.';
                            alert(mensaje);
                            
                            muestraGeneracionCuadroRechazoCarga();
                        } else {
                            if (respuesta.Mensaje == "-1") {
                                alert('El código de referencia ya existe. Ingrese otro.');
                            }
                        }
                    },
                    error: function () {
                        mostrarError("Ocurrió un error");
                    }
                });
            }
        }
    }, 100);
}
function grabarCuadroEjecucion() {
    mostrarErrores(false);

    //var datos = JSON.stringify(hot.getData())
    //if (datos.split("[").length <= 4) {
    //    alert("El cuadro debe tener al menos un cliente");
    //    return;
    //}

    //mostrarErrores(false);    
    
    var codigoCuadroPrograma = $("#hdnCodigoCuadroPrograma").val();   

    //datos cuadro programa
   
    var fechahoraInicio = $("#fechaHoraInicioEjecucion").val();
    var fechahoraFin = $("#fechaHoraFinEjecucion").val();       

    if (fechahoraInicio.trim() == '') {
        alert('Debe ingresar fecha y hora inicio');
        return false;
    }

    if (fechahoraFin.trim() == '') {
        alert('Debe ingresar fecha y hora fin');
        return false;
    }

    var fechahoraInicioFormateada = formatearFecha(fechahoraInicio);
    var fechahoraFinFormateada = formatearFecha(fechahoraFin);
    if (fechahoraFinFormateada < fechahoraInicioFormateada) {
        alert('Fecha Hora Fin debe ser mayor a Fecha Hora inicio');
        return false;
    }

    setTimeout(function () {
        if (listErrores.length > 0) {
            activarPopupErrores();
        } else {
            if (confirm("¿Desea enviar información a COES?")) {
                $.ajax({
                    type: 'POST',
                    dataType: 'json',
                    url: controlador + "GrabarEjecucionCuadroRechazoCarga",
                    data: {
                       
                        codigoCuadroPrograma: codigoCuadroPrograma, 
                        fechahoraInicio: fechahoraInicio,
                        fechahoraFin: fechahoraFin
                        
                    },
                    beforeSend: function () {
                        mostrarAlerta("Enviando Información ..");
                    },
                    success: function (respuesta) {
                        if (respuesta.Exito) {                           
                            alert("Información grabada correctamente.");
                            setTimeout(function () {
                                //ocultarMensaje();
                                muestraGeneracionCuadroRechazoCarga();
                            }, 1000);
                            //var empresas = $("#empresasSeleccionadas").val();
                        }
                    },
                    error: function () {
                        mostrarError("Ocurrió un error");
                    }
                });
            }
        }
    }, 100);
}

function HabilitarOpciones() {

    var consulta = $("#hdnFlagConsulta").val();

    if (consulta > 0) {
        document.getElementById("programa").disabled = true;
        document.getElementById("energiaRacionar").disabled = true;
        document.getElementById("demandaMinima").disabled = true;
        $('input[name=tipoEnergia]').attr("disabled", true);
        document.getElementById("configuracion").disabled = true;
        document.getElementById("motivo").disabled = true;
        document.getElementById("fechahoraInicio").disabled = true;
        document.getElementById("horaInicio").disabled = true;
        document.getElementById("horaFin").disabled = true;
        document.getElementById("ubicacion").disabled = true;
        $('input[type=checkbox]').attr("disabled", true);

        document.getElementById("btnCalcular").style.display = 'none';
        document.getElementById("btnGrabar").style.display = 'none';
        //document.getElementById("btnPopupEjecucion").style.display = 'none';

        document.getElementById("btnBuscarEmpresa").style.display = 'none';
        document.getElementById("btnEliminarSeleccionado").style.display = 'none';
        document.getElementById("btnSelectExcel").style.display = 'none';
        document.getElementById("btnMostrarErrores").style.display = 'none';

    } else {
        var programa = $("#hdnCodigoPrograma").val();
        var cuadroPrograma = $("#hdnCodigoCuadroPrograma").val();

        if (programa == 0 && cuadroPrograma > 0) {
            //document.getElementById("programa").disabled = true;
        }

        var edicionDemanda = $("#hdnFlagEditarDemanda").val();

        if (edicionDemanda == 0) {
            document.getElementById("btnBuscarEmpresa").style.display = 'none';
            document.getElementById("btnEliminarSeleccionado").style.display = 'none';
            document.getElementById("btnSelectExcel").style.display = 'none';
        }

        var perfil = $("#hdnPerfil").val();
        if (perfil == '2') {
            document.getElementById("programa").disabled = true;
        }
    }

   

    if ($("#hdnVerReportes").val() == 1) {
        document.getElementById("trReportes").style.display = '';
    }
}

function generarReporteExcel() {
   
    var codigoCuadroPrograma = $("#hdnCodigoCuadroPrograma").val();    
    var evento = $("#hdnTieneEvento").val();    

    if (codigoCuadroPrograma == '0' || codigoCuadroPrograma == '' || codigoCuadroPrograma == null) {
        return false;
    }

    if (evento == '' || evento == null) {

        alert('El cuadro no tiene evento CTAF asignado.');
        return;
    }
    
    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarReporte',
        dataType: 'json',
        data: {
            codigoCuadroPrograma: codigoCuadroPrograma,
            reporteId: $("#cbReportes").val(),
            eventoCTAF: evento
        },
        success: function (result) {
            if (result != "-1") {
                window.location = controlador + 'DescargarReporte?file=' + result;
            }
            else {
                alert("Ocurrio un error al generar el reporte.");
            }
        },
        error: function () {
            alert("Error");
        }
    });
}


consultarDistribuidores = function () {

    var codigoCuadroPrograma = $("#hdnCodigoCuadroPrograma").val();

    if (hot2 != null) {
        hot2.destroy();
    }
    //var container = document.getElementById('contenedor');
    cargarGrilla(codigoCuadroPrograma);

};

var dropdownDistribRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    var selectedId;

    for (var index = 0; index < arregloDistrib.length; index++) {
        if (parseInt(value) === arregloDistrib[index].id) {
            selectedId = arregloDistrib[index].id;
            value = arregloDistrib[index].text;
        }
    }

    $(td).addClass("estilocombo");
    Handsontable.TextCell.renderer.apply(this, arguments);
    $('#selectedId').text(selectedId);
};

var eliminarDistribuidor = function (id) {

    var distribuidoresEliminados = $("#hdnDistribEliminados").val();
    distribuidoresEliminados = distribuidoresEliminados + id + ',';
    $("#hdnDistribEliminados").val(distribuidoresEliminados);
    
    //$.ajax({
    //    type: 'POST',
    //    url: controlador + 'EliminarDistribuidor',
    //    datatype: 'json',
    //    data: {
    //        rcprodcodi: id
    //    },
    //    success: function (result) {

    //    },
    //    error: function (jqXHR, textStatus, errorThrown) {
    //        mostrarError();
    //        console.log('Error:', jqXHR);
    //    }
    //});
};

cargarGrilla = function (codigoCuadroProg) {

    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerCuadroProgDistribuidores',
        datatype: 'json',
        data: {
            codigoCuadroPrograma: codigoCuadroProg
        },
        success: function (result) {
           
            var dataDistrib = result.ListSiEmpresa;

            arregloDistrib = [];

            for (var j in dataDistrib) {
                arregloDistrib.push({ id: dataDistrib[j].Emprcodi, text: dataDistrib[j].Emprrazsocial });
            }

            var container = document.getElementById('detalleDistribuidores');
            var soloLectura = document.getElementById('hdnFlagConsulta').value == 1 ? true : false;
            var edicionDemanda = $("#hdnFlagEditarDemanda").val();

            if (!soloLectura) {
                soloLectura = edicionDemanda > 0 ? false : true;
            }
            //datos del listado procesado
            var data = result.DatosMatriz;

            var hotOptions2 = {
                data: data,
                colHeaders: ['', 'Distribuidor', 'Subestación', 'Demanda', 'Carga a Rechazar'],
                columns: [
                    { type: 'numeric', readOnly: true },
                    {
                    },
                    {
                    },
                    {
                        type: 'numeric',
                        format: '0,0.00',
                    },
                    {
                        type: 'numeric',
                        format: '0,0.00',
                    }
                ],
                readOnly: soloLectura,
                rowHeaders: true,
                comments: true,
                height: 400,
                colWidths: [1, 600, 500, 150, 150],
                cells: function (row, col, prop) {

                    var cellProperties = {};

                    if (col == 1) {
                        cellProperties.editor = 'select2';
                        cellProperties.renderer = dropdownDistribRenderer;
                        cellProperties.width = "400px";
                        cellProperties.select2Options = {
                            data: arregloDistrib,
                            dropdownAutoWidth: true,
                            width: 'resolve',
                            allowClear: false,
                        };
                    }

                    return cellProperties;
                },
                afterLoadData: function () {
                    this.render();
                },
                beforeRemoveRow: function (index, amount) {
                    //console.log('beforeRemove: index: %d, amount: %d', index, amount);
                    //eliminando filas

                    var codigos = "";
                    for (var i = amount; i > 0; i--) {

                        var valorCelda = hot2.getDataAtCell(index + i - 1, 0);

                        if (valorCelda != "" && valorCelda != null) {
                            eliminarDistribuidor(valorCelda);
                            //console.log('id eliminado: ' + valorCelda);

                        }
                    }
                }
            };

            hot2 = new Handsontable(container, hotOptions2);

            if (!soloLectura)
            {
                hot2.updateSettings({
                    contextMenu: {
                        callback: function (key, options) {
                            if (key === 'about') {
                                setTimeout(function () {
                                    alert("This is a context menu with default and custom options mixed");
                                }, 100);
                            }
                        },
                        items: {
                            "row_below": {
                                name: function () {
                                    return " <div class= 'icon-agregar' >Agregar Distribuidor</div> ";
                                }
                            },
                            "remove_row": {
                                name: function () {
                                    return " <div class= 'icon-eliminar' >Eliminar Distribuidor</div> ";
                                }
                            }
                        }
                    }
                });
            }         


        },
        error: function (jqXHR, textStatus, errorThrown) {
            //mostrarError();
            console.log('Error:', jqXHR);
        }
    });

};

function crearHandsonTableDistrib(result) {
    if (typeof hot2 != 'undefined') {
        hot2.destroy();
    }
    var dataDistrib = result.ListSiEmpresa;

    arregloDistrib = [];

    for (var j in dataDistrib) {
        arregloDistrib.push({ id: dataDistrib[j].Emprcodi, text: dataDistrib[j].Emprrazsocial });
    }

    var container = document.getElementById('detalleDistribuidores');

    //datos del listado procesado
    var data = result.DatosMatriz;
    var soloLectura = document.getElementById('hdnFlagConsulta').value == 1 ? true : false;
    var edicionDemanda = $("#hdnFlagEditarDemanda").val();

    if (!soloLectura) {
        soloLectura = edicionDemanda > 0 ? false : true;
    }
    var hotOptions2 = {
        data: data,
        colHeaders: ['', 'Distribuidor', 'Subestación', 'Demanda', 'Carga a Rechazar'],
        columns: [
            { type: 'numeric', readOnly: true },
            {
            },
            {
            },
            {
                type: 'numeric',
                format: '0,0.00',
            },
            {
                type: 'numeric',
                format: '0,0.00',
            }
        ],
        readOnly: soloLectura,
        rowHeaders: true,
        comments: true,
        height: 400,
        colWidths: [1, 600, 500, 150, 150],
        cells: function (row, col, prop) {

            var cellProperties = {};

            if (col == 1) {
                cellProperties.editor = 'select2';
                cellProperties.renderer = dropdownDistribRenderer;
                cellProperties.width = "400px";
                cellProperties.select2Options = {
                    data: arregloDistrib,
                    dropdownAutoWidth: true,
                    width: 'resolve',
                    allowClear: false,
                };
            }

            return cellProperties;
        },
        afterLoadData: function () {
            this.render();
        },
        beforeRemoveRow: function (index, amount) {
            //console.log('beforeRemove: index: %d, amount: %d', index, amount);
            //eliminando filas

            var codigos = "";
            for (var i = amount; i > 0; i--) {

                var valorCelda = hot2.getDataAtCell(index + i - 1, 0);

                if (valorCelda != "" && valorCelda != null) {
                    eliminarDistribuidor(valorCelda);
                    //console.log('id eliminado: ' + valorCelda);

                }
            }
        }
    };

    hot2 = new Handsontable(container, hotOptions2);

    if (!soloLectura)
    {
        hot2.updateSettings({
            contextMenu: {
                callback: function (key, options) {
                    if (key === 'about') {
                        setTimeout(function () {
                            alert("This is a context menu with default and custom options mixed");
                        }, 100);
                    }
                },
                items: {
                    "row_below": {
                        name: function () {
                            return " <div class= 'icon-agregar' >Agregar Distribuidor</div> ";
                        }
                    },
                    "remove_row": {
                        name: function () {
                            return " <div class= 'icon-eliminar' >Eliminar Distribuidor</div> ";
                        }
                    }
                }
            }
        });
    }    

}

function generarReporteWord()
{
    var codigoCuadroPrograma = $("#hdnCodigoCuadroPrograma").val();
    var evento = $("#hdnTieneEvento").val();    

    if (codigoCuadroPrograma == '0' || codigoCuadroPrograma == '' || codigoCuadroPrograma == null) {
        return false;
    }

    if (evento == null || evento == '') {

        alert('El cuadro no tiene evento CTAF asignado.');
        return;
    }

    var id = $("#cbReportes").val();
    var url = controlador + 'generarReporteWord?reporteId=' + id + "&evento=" + evento;
    var link = document.createElement("a");
    link.href = url;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    delete link;
}