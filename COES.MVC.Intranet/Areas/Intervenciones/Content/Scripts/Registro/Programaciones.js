var controler = siteRoot + "Intervenciones/Registro/";

var IMG_AMPLIAR_PLAZO = `<img src="${siteRoot}Content/Images/calendar.png" title="Ampliar Plazo" />`;
var IMG_VER_INFORMACION = `<img src="${siteRoot}Content/Images/btn-open.png" title="Ver informacion del registro" />`;

var IMG_APROBAR = `<img src="${siteRoot}Content/Images/btn-ok.png" title="Aprobar Plan" />`;
var IMG_APROBAR_REVERTIDO = `<img src="${siteRoot}Content/Images/btn-aprobar_revertido.png" title="Aprobar Registros Revertidos" />`;

var IMG_IPDO_EDITAR = `<img src="${siteRoot}Content/Images/btn-edit.png" title="Editar informe IPDO" />`;
var IMG_IPDO_VER = `<img src="${siteRoot}Content/Images/doc.png" title="Ver informe IPDO" />`;

var IMG_IPSO_EDITAR = `<img src="${siteRoot}Content/Images/btn-edit.png" title="Editar informe IPSO" />`;
var IMG_IPSO_VER = `<img src="${siteRoot}Content/Images/doc.png" title="Ver informe IPSO" />`;

var IMG_IPDI_EDITAR = `<img src="${siteRoot}Content/Images/btn-edit.png" title="Editar informe IPDI" />`;
var IMG_IPDI_VER = `<img src="${siteRoot}Content/Images/doc2.png" title="Ver informe IPDI" />`;

var IMG_IPSI_EDITAR = `<img src="${siteRoot}Content/Images/btn-edit.png" title="Editar informe IPSI" />`;
var IMG_IPSI_VER = `<img src="${siteRoot}Content/Images/doc2.png" title="Ver informe IPSI" />`;

var IMG_IPMI_EDITAR = `<img src="${siteRoot}Content/Images/btn-edit.png" title="Editar informe IPMI" />`;
var IMG_IPMI_VER = `<img src="${siteRoot}Content/Images/doc2.png" title="Ver informe IPMI" />`;

var IMG_INFORME_AGENTE = `<img src="${siteRoot}Content/Images/excel.png" title="Ver informe" />`;
var IMG_INFORME_OSINERGMIN = `<img src="${siteRoot}Content/Images/excel.png" title="Informe Osinergmin" />`;

var IMG_SUBIR_INF_OP = `<img src="${siteRoot}Content/Images/subir_azul.png" title="Cargar el informe de la Operación al Portal público" />`;
var IMG_SUBIR_INF_IN = `<img src="${siteRoot}Content/Images/subir_negro.png" title="Cargar el informe de la Intervenciones al Portal público" />`;

$(document).ready(function ($) {

    $('#FechaDesde').Zebra_DatePicker({
        format: 'd/m/Y',
        onSelect: function () {
            listarProgramas();
        }
    });

    $('#FechaHasta').Zebra_DatePicker({
        format: 'd/m/Y',
        onSelect: function () {
            listarProgramas();
        }
    });

    $('#AnhoFiltroIni').Zebra_DatePicker({
        format: 'Y',
        onSelect: function () {
            cargarSemanaAnhoIni($('#AnhoFiltroIni').val());
        }
    });

    $('#cboSemanaIni').change(function () {
        listarProgramas();
    });

    $('#AnhoFiltroFin').Zebra_DatePicker({
        format: 'Y',
        onSelect: function () {
            cargarSemanaAnhoFin($('#AnhoFiltroFin').val());
        }
    });

    $('#cboSemanaFin').change(function () {
        listarProgramas();
    });

    $('#tdmesini').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            listarProgramas();
        }
    });

    $('#tdmesfin').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            listarProgramas();
        }
    });

    $('.celda-item').css("display", "none");
    $('.filtroDiario').css("display", "none");
    $('.filtroSemanal').css("display", "none");
    $('.filtroMensual').css("display", "none");


    $('#tipoProgramacion').change(function () {
        filtroXHorizonte();//activa los filtros
        listarProgramas();
    });

    $('#Empresa').multipleSelect({
        placeholder: "-- Todos --",
        selectAll: false,
        allSelected: onoffline,
        filter: true
    });

    $('#idFechaAmp').Zebra_DatePicker({
        direction: -1
    });

    //
    $("#idAgregar").unbind();
    $("#idAgregar").click(function () {
        guardarAmpliacion();
    });
    $("#idCancelar").click(function () {
        $('#validaciones').bPopup().close();
    });

    $("#CrearProgramacion").click(function () {
        var idprogra = document.getElementById('tipoProgramacion').value;
        AbrirNuevaProgra(idprogra);
    });

    $("#btnManualUsuario").click(function () {
        window.location = controler + 'DescargarManualUsuario';
    });

    filtroXHorizonte();//activa los filtros
    listarProgramas();
});

//funcion que calcula el ancho disponible para la tabla reporte
function getHeightTablaListado() {
    var altoDisponible = $(window).height()
        - 36 //titulo del iframe
        - 15 //padding-top mainLayout
        - 70 //body Filtros
        - $(".dataTables_filter").height()
        - $(".dataTables_scrollHead").height()
        - 30 //nota abajo de tabla
        - 15 //padding-boton mainLayout
        - 230 //para que no haya scroll horizontal al ingresar por primera vez al menu Intervenciones 
        - 40 //adicional iframe
        ;

    return altoDisponible > 0 ? altoDisponible : 100;
}

function listarProgramas() {

    var idTipoProgramacion = parseInt(document.getElementById('tipoProgramacion').value) || 0;

    //obtener valores desde - hasta
    var desde = "";
    var hasta = "";
    switch (idTipoProgramacion) {
        case 1: case 2: // ejecutados y diarios
            desde = $('#FechaDesde').val();
            hasta = $('#FechaHasta').val();
            break;
        case 3: // semanal
            desde = $('#cboSemanaIni').val();
            hasta = $('#cboSemanaFin').val();
            break;
        case 4: // mensual
            desde = $('#tdmesini').val();
            hasta = $('#tdmesfin').val();
            break;
    }

    var msj = validarConsulta(idTipoProgramacion, desde, hasta);
    if (msj == "") {
        $('#listado').html('');
        if (idTipoProgramacion > 0) {
            $.ajax({
                type: 'POST',
                url: controler + "ProgramacionesListado",
                datatype: "json",
                contentType: 'application/json',
                data: JSON.stringify({ idTipoProgramacion: idTipoProgramacion, fdesde: desde, fhasta: hasta }),
                success: function (evt) {
                    if (evt.Resultado != "-1") {
                        $("#listado").hide();
                        $('#listado').css("width", $('#mainLayout').width() + "px");

                        var htmlTabla = _dibujarTablaListado(evt);
                        $('#listado').html(htmlTabla);
                        var nuevoTamanioTabla = getHeightTablaListado();
                        $("#listado").show();

                        if (idTipoProgramacion != 1)
                            $('#CrearProgramacion').css("visibility", "visible");
                        else
                            $('#CrearProgramacion').css("visibility", "hidden");

                        $('#listado').show();
                        $('#tabla1').dataTable({
                            "sPaginationType": "full_numbers",
                            "destroy": "true",
                            "ordering": false,
                            "searching": true,
                            "iDisplayLength": 15,
                            "info": false,
                            "paging": false,
                            "scrollY": $('#listado').height() > 200 ? nuevoTamanioTabla + "px" : "100%"
                        });
                    } else {
                        $('#listado').html('');
                        alert(excep_mensaje);
                    }
                },
                error: function (err) {
                    alert("Lo sentimos, se ha producido un error");
                }
            });
        }
    } else {
        alert(msj);
    }

};

function _dibujarTablaListado(model) {
    var lista = model.ListaProgramaciones;

    var thPrograma = '';
    if (model.IdTipoProgramacion != 1) {
        thPrograma = `
                <th>Aprobar</th>
                <th>Estado</th>
                <th>Informe Op.</th>
                <th>Informe In.</th>
                <th>Reporte</th>
                <th>Portal Web</th>
        `;
    }

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="tabla1">
        <thead>
            <tr>
                <th>Programación</th>
                <th>Ampliar Plazo</th>
                <th>Registro</th>
                ${thPrograma}
            </tr>
        </thead>
        <tbody>
    `;

    for (var key in lista) {
        var item = lista[key];

        var tdAprobar = '';
        var tdInformeOp = '';
        var tdInformeIn = '';
        var tdExcel = '';
        var tdPortal = '';

        if (model.IdTipoProgramacion != 1) {
            if (item.EstadoIntranetDesc != "Aprobado") {
                tdAprobar = `    
                        <td>
                            <a href="#" onclick="aprobar(${item.Progrcodi}, ${model.IdTipoProgramacion})" class="checkIntervencion">
                                ${IMG_APROBAR}
                            </a>
                        </td>

                        <td style="text-align:left">${item.EstadoIntranetDesc}</td>
                          `;
            }
            else {

                if (item.EsPlanRevertido && item.TieneRegistrosxReversion && (item.Progresaprobadorev == null || item.Progresaprobadorev == 0)) {
                    tdAprobar = `    
                            <td>
                                <a href="#" onclick="aprobarReversion(${item.Progrcodi}, ${model.IdTipoProgramacion})" class="checkIntervencion">
                                    ${IMG_APROBAR_REVERTIDO}
                                </a>
                            </td>

                            <td style="text-align:left; color:red"><b>${item.EstadoIntranetDesc} (Atención Urgente)</b></td>
                              `;
                }
                else {
                    tdAprobar = `    
                            <td></td>
                            <td style="text-align:left">${item.EstadoIntranetDesc}</td>
                              `;
                }
            }

            //
            if (item.TotalRegistro > 0) {

                if (model.IdTipoProgramacion == 2) {
                    tdInformeOp = `  
                                <a href="#" onclick="imprimirInformeEditar(${item.Progrcodi}, 1)" class="checkIntervencion">
                                    ${IMG_IPDO_EDITAR}
                                </a>
                                <a href="#" onclick="imprimirInforme(${item.Progrcodi}, ${model.IdTipoProgramacion},1)" class="checkIntervencion">
                                    ${IMG_IPDO_VER}
                                </a>
                                   `;
                }
                else {
                    if (model.IdTipoProgramacion == 3) {
                        tdInformeOp = `  
                                    <a href="#" onclick="imprimirInformeEditar(${item.Progrcodi}, 3)" class="checkIntervencion">
                                        ${IMG_IPSO_EDITAR}
                                    </a>
                                    <a href="#" onclick="imprimirInforme(${item.Progrcodi}, ${model.IdTipoProgramacion},1)" class="checkIntervencion">
                                        ${IMG_IPSO_VER}
                                    </a>
                                       `;
                    }
                }

                if (model.IdTipoProgramacion == 2) {
                    tdInformeIn = `  
                                <a href="#" onclick="imprimirInformeEditar(${item.Progrcodi}, 2)" class="checkIntervencion">
                                    ${IMG_IPDI_EDITAR}
                                </a>
                                <a href="#" onclick="imprimirInforme(${item.Progrcodi}, ${model.IdTipoProgramacion}, 2)" class="checkIntervencion">
                                    ${IMG_IPDI_VER}
                                </a>
                                  `;
                }
                else {
                    if (model.IdTipoProgramacion == 3) {
                        tdInformeIn = `  
                                    <a href="#" onclick="imprimirInformeEditar(${item.Progrcodi}, 4)" class="checkIntervencion">
                                        ${IMG_IPSI_EDITAR}
                                    </a>
                                    <a href="#" onclick="imprimirInforme(${item.Progrcodi}, ${model.IdTipoProgramacion}, 2)" class="checkIntervencion">
                                        ${IMG_IPSI_VER}
                                    </a>
                                      `;
                    }
                    else {
                        if (model.IdTipoProgramacion == 4) {
                            tdInformeIn = `  
                            <a href="#" onclick="imprimirInformeEditar(${item.Progrcodi}, 5)" class="checkIntervencion">
                                ${IMG_IPMI_EDITAR}
                            </a>
                              `;
                        }

                        tdInformeIn += `  
                        <a href="#" onclick="imprimirInforme(${item.Progrcodi}, ${model.IdTipoProgramacion}, 0)" class="checkIntervencion">
                            ${IMG_IPMI_VER}
                        </a>
                          `;
                    }
                }

                //
                tdExcel = ` 
                            <a href="#" onclick="imprimirReporte(${item.Progrcodi})" class="checkIntervencion">
                                ${IMG_INFORME_AGENTE}
                            </a>
                            <a href="#" onclick="imprimirReporteOsinergmin(${item.Progrcodi})" class="checkIntervencion">
                                ${IMG_INFORME_OSINERGMIN}
                            </a>
                            `;

                //

                if (model.IdTipoProgramacion == 2 || model.IdTipoProgramacion == 3) {
                    tdPortal += ` 
                            <a href="#" onclick="subirInformeOperacionAPortal(${item.Progrcodi})" class="checkIntervencion">
                                ${IMG_SUBIR_INF_OP}
                            </a>
                            `;
                }
                if (model.IdTipoProgramacion == 2 || model.IdTipoProgramacion == 3 || model.IdTipoProgramacion == 4 || model.IdTipoProgramacion == 5) {
                    tdPortal += ` 
                            <a href="#" onclick="subirInformeIntervencionAPortal(${item.Progrcodi})" class="checkIntervencion">
                                ${IMG_SUBIR_INF_IN}
                            </a>
                            `;
                }
            }
        }

        cadena += `            
            <tr id="fila_${item.Progrcodi}">
                <td style="text-align:left" id="prueba">${item.ProgrnombYPlazo}</td>

                <td>
                    <a href="#" onclick="ampliacion(${item.Progrcodi},${model.IdTipoProgramacion})" class="checkAmpliacion">
                        ${IMG_AMPLIAR_PLAZO}
                    </a>
                </td>

                <td>
                    <a href="#" onclick="check(${item.Progrcodi}, ${model.IdTipoProgramacion}, ${item.Progrsololectura})" class="checkIntervencion">
                        ${IMG_VER_INFORMACION}
                    </a>
                </td>
        `;

        if (model.IdTipoProgramacion != 1) {
            cadena += `
                ${tdAprobar}
                <td>${tdInformeOp}</td>
                <td>${tdInformeIn}</td>
                <td>${tdExcel}</td>
                <td>${tdPortal}</td>
            </tr>
        `;
        }

        cadena += `    
            </tr>
        `;

    }
    cadena += "</tbody></table>";

    return cadena;
}

function validarConsulta(idTipoProgramacion, desde, hasta) {
    var msj = "";

    switch (idTipoProgramacion) {
        case 1: case 2: // ejecutados y diarios

            if (formatFecha(desde) > formatFecha(hasta)) {
                msj += "Fecha Desde debe ser menor a la fecha Hasta";
            }

            break;
        case 3: // semanal
            if (formatFecha(desde) > formatFecha(hasta)) {
                msj += "Fecha Desde debe ser menor a la fecha Hasta";
            }
            break;
        case 4: // mensual
            if (formatFechaMes(desde) > formatFechaMes(hasta)) {
                msj += "Fecha Desde debe ser menor a la fecha Hasta";
            }
            break;
    }

    return msj;
}

function formatFecha(sFecha) {   // DD/MM/AAAA
    if (sFecha == "") return false;

    var sDia = sFecha.substring(0, 2);
    var sMes = sFecha.substring(3, 5);
    var sAnio = sFecha.substring(6, 10);

    return new Date(sAnio, sMes - 1, sDia);
}

function formatFechaMes(date) { // MM AAAA
    var parts = date.split(" ");
    return new Date(parts[1], parts[0] - 1, 1);
}

function cargarSemanaAnhoIni(anho) {
    $.ajax({
        type: 'POST',
        url: controler + 'CargarSemana',
        async: false,
        data: {
            idAnho: anho,
            tipoSem: 'Ini'
        },
        success: function (aData) {
            $("#divSemanaIni").html(aData);
            listarProgramas();

            $('#cboSemanaIni').unbind();
            $('#cboSemanaIni').change(function () {
                listarProgramas();
            });
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}
function cargarSemanaAnhoFin(anho) {
    $.ajax({
        type: 'POST',
        url: controler + 'CargarSemana',
        async: false,
        data: {
            idAnho: anho,
            tipoSem: 'Fin'
        },
        success: function (aData) {
            $("#divSemanaFin").html(aData);
            listarProgramas();

            $('#cboSemanaFin').unbind();
            $('#cboSemanaFin').change(function () {
                listarProgramas();
            });
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

filtroXHorizonte = function () {

    var tipoProgramacion = parseInt(document.getElementById('tipoProgramacion').value) || 0;

    $(".filtroSello").hide();

    switch (tipoProgramacion) {
        case 1:// ejecutados
            $('.celda-item').css("display", "table-cell");
            $('.filtroDiario').css("display", "table-cell");
            $('.filtroSemanal').css("display", "none");
            $('.filtroMensual').css("display", "none");

            //valor inicial
            $('#FechaDesde').val($('#hdfechaini').val());
            $('#FechaHasta').val($('#hdEjecutadofin').val());

            break;
        case 2: // diarios
            $('.celda-item').css("display", "table-cell");
            $('.filtroDiario').css("display", "table-cell");
            $('.filtroSemanal').css("display", "none");
            $('.filtroMensual').css("display", "none");

            //valor inicial
            $('#FechaDesde').val($('#hdfechaini').val());
            $('#FechaHasta').val($('#hdDiariofin').val());
            break;
        case 3: // semanal
            $('.celda-item').css("display", "table-cell");
            $('.filtroSemanal').css("display", "table-cell");
            $('.filtroDiario').css("display", "none");
            $('.filtroMensual').css("display", "none");
            $(".filtroSello").css("display", "table-cell");

            break;
        case 4: // mensual
            $('.celda-item').css("display", "table-cell");
            $('.filtroMensual').css("display", "table-cell");
            $('.filtroDiario').css("display", "none");
            $('.filtroSemanal').css("display", "none");

            break;
        default:
            $('.celda-item').css("display", "none");
            $('.filtroDiario').css("display", "none");
            $('.filtroSemanal').css("display", "none");
            $('.filtroMensual').css("display", "none");
    }
};

function check(progCodi) {
    var idEmpresa = JSON.stringify($('#Empresa').val());

    if (idEmpresa != null && idEmpresa != "") {
        progCodi = parseInt(progCodi) || 0;
        window.location.href = controler + 'IntervencionesRegistro?progCodi=' + progCodi;

        return true;
        //:fin
    } else {
        alert("Debes seleccionar una empresa");
        return false;
    }
}

function aprobar(progrCodi, idTipoProgramacion) {
    if (confirm("¿Desea aprobar el plan seleccionado?")) {
        $.ajax({
            type: 'POST',
            url: controler + 'AprobarPlan',
            data: { progrCodi: progrCodi, idTipoProgramacion: idTipoProgramacion },
            success: function (evt) {
                if (evt.Resultado != '-1') {
                    if (evt.Resultado == "1") {
                        listarProgramas();
                        document.getElementById('mensaje').innerHTML = "<div class='action-exito mensajes'>¡Se han aprobado las intervenciones correctamente!</div>";
                        setTimeout(function () { $(".mensajes").fadeOut(800).fadeIn(800).fadeOut(500).fadeIn(500).fadeOut(300); }, 3000);
                    }
                    else {
                        alert("No existen registros para aprobar.");
                    }
                } else {
                    alert(evt.Mensaje);
                }
            },
            error: function (err) {
                alert("Error inesperado");
            }
        });
    }
}

function imprimirInforme(progrCodi, idTipoProgramacion, tipo) {
    var checkSello = tieneCheckSello();
    $.ajax({
        type: 'POST',
        url: controler + 'GenerarWordInforme',
        data: {
            progrCodi: progrCodi,
            idTipoProgramacion: idTipoProgramacion,
            tipo: tipo,
            sTieneSelloSemanal: checkSello
        },
        dataType: 'json',
        success: function (result) {
            if (result != -1) {
                //Modificación 15/05/2019
                var paramList = [
                    { tipo: 'input', nombre: 'file', value: result }
                ];
                var form = CreateForm(controler + 'AbrirArchivo', paramList);
                document.body.appendChild(form);
                form.submit();
                return true;
            }
            else {
                alert("Ha ocurrido un error");
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function imprimirInformeEditar(progcodi, tipo) {
    document.location.href = siteRoot + "intervenciones/parametro/DetalleInformeListado?id=" + tipo + "&progcodi=" + progcodi;
}

function imprimirReporte(progrCodi) {
    $.ajax({
        type: 'POST',
        url: controler + 'GenerarExcelReporteIntervencionesProcesadas',
        data: { progrCodi: progrCodi },
        dataType: 'json',
        success: function (result) {
            if (result != -1) {
                //Modificación 15/05/2019
                var paramList = [
                    { tipo: 'input', nombre: 'file', value: result }
                ];
                var form = CreateForm(controler + 'AbrirArchivo', paramList);
                document.body.appendChild(form);
                form.submit();
                return true;
            }
            else {
                alert("No existen registros a exportar");
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function toDate(dateStr) {
    var parts = dateStr.split("/");
    return new Date(parts[2], parts[1] - 1, parts[0]);
}

function CreateForm(path, params, method = 'post') {
    var form = document.createElement('form');
    form.method = method;
    form.action = path;

    $.each(params, function (index, obj) {
        var hiddenInput = document.createElement(obj.tipo);
        hiddenInput.type = 'hidden';
        hiddenInput.name = obj.nombre;
        hiddenInput.value = obj.value;
        form.appendChild(hiddenInput);
    });
    return form;
}

//ampliación
function ampliacion(item1, item2) {
    listarAmpliaciones(item1, item2);
}


function listarAmpliaciones(item1, item2) {
    var idprogra = parseInt(item1) || -1;
    $.ajax({
        type: 'POST',
        url: controler + 'ListarHistorico',
        data: {
            idprogramacion: idprogra,
            tipoProgra: item2
        },
        success: function (result) {

            $('#tablaListadoCabecera').html(result);

            $('#listadoGrupoDat').html(result);

            $('#tablaListadoGrupodat').dataTable({
                "destroy": "true",
                "sPaginationType": "full_numbers",
                "ordering": false,
                "searching": false,
                "paging": false,
                "info": false,
                "aoColumnDefs": [{
                    'bSortable': false,
                }],
                "order": [[2, "asc"]],
                "iDisplayLength": 15
            });

            setTimeout(function () {
                $('#popupAmpliacion').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 250);
        },
        error: function (err) {
            alert('Ha ocurrido un error: ' + err);
        }
    });
}

//guardar ampliación
function guardarAmpliacion() {
    var entity = getObjetoAmpliacionJson();
    if (confirm('¿Desea guardar el registro?')) {
        var msj = validarAmpliacion(entity);
        if (msj == "") {
            $.ajax({
                type: 'POST',
                dataType: 'json',
                traditional: true,
                url: controler + "ProgrAmpliacionGuardar",
                data: {
                    idprogramacion: entity.progracodi,
                    strfechaInicio: entity.fechlimite,
                    strfechaFin: entity.fechAmpliacion,
                    hora: entity.valorHora,
                    minuto: entity.valorMinuto,
                    desc: entity.descripcion
                },
                cache: false,
                success: function (result) {
                    if (result.Resultado == '-1') {
                        alert('Ha ocurrido un error:' + result.StrMensaje);
                    } else {
                        alert("Se guardó correctamente el registro");

                        codigooo = entity.progracodi;
                        listarProgramas();

                        listarAmpliaciones(entity.progracodi, document.getElementById('tipoProgramacion').value);
                    }
                },
                error: function (err) {
                    alert('Ha ocurrido un error.');
                }
            });
        } else {
            alert(msj);
        }
    }
}

function validarAmpliacion(obj) {
    var msj = "";

    if (obj.fechAmpliacion == null || obj.fechAmpliacion == '') {
        msj += " \n Debe seleccionar una fecha";
    }
    if (obj.descripcion.length < 0) {
        msj += " \n Debe ingresar una Descripción";
    }

    //HORA
    if (obj.valorHora == null || obj.valorHora == '') {
        msj += " \n Debe ingresar una hora.";//Campo Requerido
    }
    else {
        if (validarNumero(obj.valorHora)) {
            var horainicio = parseInt(obj.valorHora);
            if (horainicio > 24) {
                msj += " \n Hora: Ingrese un valor menor o igual que 24.";
            }
        }
        else {
            msj += " \n Hora: Debe ser un número.";
        }
    }

    //MINUTO
    if (obj.valorMinuto == null || obj.valorMinuto == '') {
        msj += " \n Debe ingresar minutos.";//Campo Requerido
    }
    else {
        if (validarNumero(obj.valorMinuto)) {
            var minutoini = parseInt(obj.valorMinuto);
            if (minutoini > 59) {
                msj += " \n Minuto: Ingrese un valor menor o igual que 59.";
            }
        }
        else {
            msj += " \n Minuto: Debe ser un número.";
        }
    }

    return msj;
}

function validarNumero(texto) {
    return /^-?[\d.]+(?:e-?\d+)?$/.test(texto);
};

function getObjetoAmpliacionJson() {
    var obj = {};
    obj.fechAmpliacion = $("#idFechaAmp").val();
    //obj.valorHora = $("#cbHora").val();
    obj.valorHora = $("#amplihora").val();
    obj.valorMinuto = $("#ampliminuto").val();
    obj.fechlimite = $("#idFechaLimite").val();
    obj.progracodi = $("#progrcodi").val();
    obj.descripcion = $("#descripcion").val();

    return obj;
}

//popput nuevo programa
function AbrirNuevaProgra(idprogra) {
    $.ajax({
        type: 'POST',
        url: controler + 'NuevaProgramacion',
        data: {
            idprogramacion: idprogra
        },
        success: function (result) {

            $('#tablaNewprograma').html(result);

            $('#listadoProgramas').html(result);

            setTimeout(function () {
                $('#popupProgramacion').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 250);
        },
        error: function (err) {
            alert('Ha ocurrido un error: ' + err);
        }
    });
}

//guardar Programación
function guardarProgramacion() {
    var entity = getObjetoProgramacionJson();
    if (confirm('¿Desea guardar el programa?')) {
        var msj = validarProgramacion(entity);
        if (msj == "") {
            $.ajax({
                type: 'POST',
                dataType: 'json',
                traditional: true,
                url: controler + "NuevaProgramacionGuardar",
                data: {
                    idprogramacion: entity.Evenclasecodi,
                    strfechaInicio: entity.Progrfechaini
                },
                cache: false,
                success: function (result) {
                    if (result.Resultado == '-1' && result.StrMensaje != "" || result.StrMensaje != null) {
                        alert(result.StrMensaje);
                    } else {
                        alert("Se guardó correctamente el registro");
                        $('#popupProgramacion').bPopup().close();
                        listarProgramas();
                    }
                },
                error: function (err) {
                    alert('Ha ocurrido un error.');
                }
            });
        } else {
            alert(msj);
        }
    }
}

//Json para nueva programación
function getObjetoProgramacionJson() {
    var obj = {};
    obj.Evenclasecodi = $("#idTipoProgra").val();

    if (obj.Evenclasecodi == 2) {
        obj.Progrfechaini = $("#InterfechainiD").val();
    } else if (obj.Evenclasecodi == 3) {
        obj.Progrfechaini = $('#cboSemanaIni').val();
    } else if (obj.Evenclasecodi == 4) {
        obj.Progrfechaini = $('#mes').val();
    } else if (obj.Evenclasecodi == 5) {
        var periodo = document.getElementById('cboPeriodo').value;
        var anio2 = $('#AnhoIni2').val();
        obj.Progrfechaini = periodo == 1 ? "01/01/" + anio2 : "01/07/" + anio2;
    }
    return obj;
}
//validar programación
function validarProgramacion(obj) {
    var msj = "";
    if (obj.Progrfechaini == null || obj.Progrfechaini == '') {
        msj += "Debe seleccionar una fecha inicio";
    }

    if (obj.Evenclasecodi == 5 && document.getElementById('cboPeriodo').value == 0) {
        msj += "Debe seleccionar un periodo del año";
    }
    return msj;
}

//cargar semanas año
function cargarSemanaAnho(tipoSem) {
    $.ajax({
        type: 'POST',
        url: controler + 'CargarSemana',
        data: {
            idAnho: getAnio(tipoSem), tipoSem: tipoSem
        },
        success: function (aData) {
            $('#Semana' + tipoSem).html(aData);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function getAnio(tipoSem) {
    var anio = $('#Anho' + tipoSem).val();
    return parseInt(anio);
}

function aprobarReversion(progrCodi, idTipoProgramacion) {
    if (confirm("¿Desea aprobar los registros revertidos?")) {
        $.ajax({
            type: 'POST',
            url: controler + 'AprobarReversion',
            data: { progrCodi: progrCodi, idTipoProgramacion: idTipoProgramacion },
            success: function (evt) {
                if (evt.Resultado != '-1') {
                    if (evt.Resultado == "1") {
                        listarProgramas();
                        document.getElementById('mensaje').innerHTML = "<div class='action-exito mensajes'>¡Se han aprobado los registros revertidos de las intervenciones correctamente!</div>";
                        setTimeout(function () { $(".mensajes").fadeOut(800).fadeIn(800).fadeOut(500).fadeIn(500).fadeOut(300); }, 3000);
                    }
                    else {
                        alert("No existen registros para aprobar.");
                    }
                } else {
                    alert(evt.Mensaje);
                }
            },
            error: function (err) {
                alert("Error inesperado");
            }
        });
    }
}

function imprimirReporteOsinergmin(progrCodi) {
    var tipoProCodi = parseInt(document.getElementById('tipoProgramacion').value) || 0;

    $.ajax({
        type: 'POST',
        url: controler + 'GenerarExcelIntervencionesOsinergmin',
        data: {
            progrCodi: progrCodi,
            tipoProCodi: tipoProCodi
        },
        dataType: 'json',
        success: function (result) {
            if (result == -1) {
                alert("No se encuentra datos a exportar!")
            }
            else if (result != -1) {
                document.location.href = controler + 'AbrirArchivo?file=' + result;
            }
            else {
                alert("Ha ocurrido un error");
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

//Subir informes zipeados a portal
async function subirInformeOperacionAPortal(progrcodi) {
    await verificarFileInformeAPortal(progrcodi, 1);

    if (TIPO_VERIFICAR > 0) {
        var mensaje = TIPO_VERIFICAR == 1 ? "¿Desea cargar Informe de Operación al Portal Web?" : "Ya existe Informe de Operación en el Portal Web ¿Desea reemplazarlo?";
        subirInformeAPortal(progrcodi, 1, mensaje);
    }
}

async function subirInformeIntervencionAPortal(progrcodi) {
    await verificarFileInformeAPortal(progrcodi, 2);

    if (TIPO_VERIFICAR > 0) {
        var mensaje = TIPO_VERIFICAR == 1 ? "¿Desea cargar Informe de Intervención al Portal Web?" : "Ya existe Informe de Intervención en el Portal Web ¿Desea reemplazarlo?";
        subirInformeAPortal(progrcodi, 2, mensaje);
    }
}

async function subirInformeAPortal(progrcodi, tipoInforme, mensaje) {
    var checkSello = tieneCheckSello();

    if (confirm(mensaje)) {
        $.ajax({
            type: 'POST',
            url: controler + 'SubirInformeAPortal',
            data: {
                progrcodi: progrcodi,
                tipoInforme: tipoInforme,
                sTieneSelloSemanal: checkSello
            },
            dataType: 'json',
            success: function (model) {
                if (model.Resultado != -1) {
                    alert("La acción se ha realizado correctamente.");
                }
                else {
                    alert("Ha ocurrido un error");
                }
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    }
}

var TIPO_VERIFICAR = 0;
async function verificarFileInformeAPortal(progrcodi, tipoInforme) {
    TIPO_VERIFICAR = 0;
    return $.ajax({
        type: 'POST',
        url: controler + 'VerificarFileInformeAPortal',
        data: {
            progrcodi: progrcodi,
            tipoInforme: tipoInforme
        },
        dataType: 'json',
        success: function (model) {
            if (model.Resultado != -1) {
                TIPO_VERIFICAR = model.TieneArchivos ? 1 : 2;
            }
            else {
                alert("Ha ocurrido un error");
            }
        },
        error: function (err) {
            EXISTE_INFORME_PORTAL = true;
        }
    });
}

//sello en informe word
function tieneCheckSello() {
    var check = $('#chkSello').is(":checked");
    return check ? "S" : "N"; 
}