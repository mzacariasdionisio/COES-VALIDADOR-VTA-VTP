var controler = siteRoot + "Intervenciones/Registro/";
var controlador = siteRoot + "Intervenciones/Registro/";


function formularioHistoria(objParam) {
    $.ajax({
        type: 'POST',
        url: controlador + "FormularioIndexHistoria",
        dataType: 'json',
        success: function (evt) {
            if (evt.Resultado != "-1") {

                //Inicializar popup
                $("#popupFormHistoria").html(generarPopupHtmlHistoriaEquipo(objParam));
                inicializarFormularioHistoria(evt);
                cargarEventosHistoriaJs();

                setTimeout(function () {
                    $('#popupFormHistoria').bPopup({
                        easing: 'easeOutBack',
                        speed: 450,
                        transition: 'slideDown',
                        modalClose: false
                    });
                }, 50);

                //mostrarHistoria();

            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

//Generar html
function generarPopupHtmlHistoriaEquipo(objParam) {

    var html = `<span class="button b-close"><span>X</span></span>
        <div class="popup-title">
            <span>Reporte histórico</span>

            <div class="content-botonera">
                <input type="button" id="btnExportarHistoria" value="Exportar" style='float: right;'/>
                <input type="button" id="btnBuscarHistoria" value="Buscar" style='float: right;'/>
            </div>
        </div>

        <div>
        <input type="hidden" id="TipoProgrcodiHistoria" value="${objParam.tipoProgramacion}" />
        <input type="hidden" id="EquicodiHistoria" value="${objParam.equicodi}" />
        </div>

        <div id="mensaje2" class="action-alert" style="margin:0; margin-bottom:10px" hidden>Por Favor complete los datos solicitados</div>

        <div class="ast" style="height: 600px; overflow-y: auto">

             <table id="tablaListadoHistoria" class="content-tabla-search" style="width:auto">

                 <tr>
                    <td colspan="2">
                        Fecha Inicio:
                        <div style="width:110px;display: inline;margin-right: 21px;" class="editor-label">
                            <input type="text" id="idFechaIniHistoria" value="" style="width: 100px" class="txtFecha" />
                        </div>

                        Fecha Fin:
                        <div style="width:110px;display: inline;" class="editor-label">
                            <input type="text" id="idFechaFinHistoria" value="" style="width: 100px" class="txtFecha" />
                        </div>
                    </td>
                 </tr>

                 <tr>
                     <td>Tipo de Programación:</td>
                     <td>
                         <select style="background-color:white;width: 250px;" id="cboTipoProgramacionHistoria" name="Entidad.Evenclasecodi">
                         </select>
                     </td>

                        <td>Disponibilidad:</td>
                        <td>
                            <select style="width:165px; background-color:white;" id="cboInterDispo">
                                <option value="0">TODO</option>
                                <option value="F">FS (Fuera de Servicio)</option>
                                <option value="E">ES (En Servicio)</option>
                            </select>
                        </td>
                 </tr>
             </table>

            <div class="content-hijo" id="mainLayoutEdit" style="padding-top:8px; padding-left: 0px; padding-right: 0px;">

                <div>
                    <div id="listadoHistoria">

                    </div>
                </div>
            </div>
        </div>
    `;

    return html;
}

//inicializa vista formulario
function inicializarFormularioHistoria(model) {
    document.getElementById('idFechaIniHistoria').value = model.Progrfechaini;
    document.getElementById('idFechaFinHistoria').value = model.Progrfechafin;

    $('#cboTipoProgramacionHistoria').empty();
    var optionTipoProgr = '<option value="" >----- Seleccione una clase ----- </option>';
    $.each(model.ListaTiposProgramacion, function (k, v) {
        optionTipoProgr += '<option value =' + v.Evenclasecodi + '>' + v.Evenclasedesc + '</option>';
    })
    $('#cboTipoProgramacionHistoria').append(optionTipoProgr);

}

function cargarEventosHistoriaJs() {
    $('#idFechaIniHistoria').unbind();
    $('#idFechaIniHistoria').Zebra_DatePicker({
        readonly_element: false
    });
    $('#idFechaFinHistoria').unbind();
    $('#idFechaFinHistoria').Zebra_DatePicker({
        readonly_element: false
    });

    $("#btnBuscarHistoria").unbind();
    $("#btnBuscarHistoria").click(function () {
        mostrarHistoria();
    });

    $("#btnExportarHistoria").unbind();
    $("#btnExportarHistoria").click(function () {
        exportarHistoria();
    });

    //Cambia Tipo Intervención
    //$("#cboTipoEventoPopup").unbind();
    //$('#cboTipoEventoPopup').on("change", function () {
    //    if (OPCION_ACTUAL == OPCION_EDITAR) {
    //        if (INTERVENCION_WEB.Tipoevencodi != $("#cboTipoEvento").val()) {
    //            $("#codseguimiento").val("");
    //        }
    //        else {
    //            $("#codseguimiento").val(INTERVENCION_WEB.Intercodsegempr);
    //        }
    //    }
    //});
}

//Obtener Intervención
function mostrarHistoria() {

    limpiarMensaje();
    var objData = getObjetoHistoriaFiltro();
    var msj = validarConsultaHistoria(objData);

    if (msj == "") {
        $.ajax({
            type: 'POST',
            url: controlador + "MostrarHistoriaEquipo",
            dataType: 'json',
            data: objData,
            success: function (evt) {
                if (evt.Resultado != "-1") {

                    var html = dibujarListadoHistoria(evt);
                    $('#listadoHistoria').html(html);

                } else {
                    alert("Ha ocurrido un error: " + evt.Mensaje);
                }
            },
            error: function (err) {
                alert("Error al cargar Intervención");
            }
        });
    } else {
        mostrarAlerta(msj);
    }
}


function dibujarListadoHistoria(model) {
    var lista = model.ListaFilaWeb;

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="TablaHistoriaEquipo" style="overflow: auto; height:auto; white-space: nowrap">
        <thead>
            <tr>
                <th style="text-align:center;">Horizonte</th>
                <th style="text-align:center;">Fecha inicio</th>
                <th style="text-align:center;">Fechafin</th>
                <th style="text-align:center;">Empresa</th>
                <th style="text-align:center;">Ubicacion</th>
                <th style="text-align:center;">Equipo</th>
                <th style="text-align:center;">Descripción</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (var key in lista) {
        var item = lista[key];
        var sStyle = "";

        cadena += `

            <tr id="fila_${item.Equicodi}">
                <td style="text-align:left; ${sStyle}">${item.Evenclasedesc}</td>
                <td style="text-align:left; ${sStyle}">${item.InterfechainiDesc}</td>
                <td style="text-align:left; ${sStyle}">${item.InterfechafinDesc}</td>
                <td style="text-align:left; ${sStyle}">${item.EmprNomb}</td>
                <td style="text-align:left; ${sStyle}">${item.AreaNomb}</td>
                <td style="text-align:left; ${sStyle}">${item.Equiabrev}</td>
                <td style="text-align:left; ${sStyle}">${item.Interdescrip}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}

function exportarHistoria() {
    var objData = getObjetoHistoriaFiltro();
    var msj = validarConsultaHistoria(objData);
    limpiarMensaje();

    if (msj == "") {
        $.ajax({
            type: 'POST',
            url: controler + 'ExportarHistoriaEquipo',
            data: objData,
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    var paramList = [
                        { tipo: 'input', nombre: 'file', value: evt.NombreArchivo }
                    ];
                    var form = CreateForm(controler + 'AbrirArchivo', paramList);
                    document.body.appendChild(form);
                    form.submit();
                }
                else {
                    alert('Ha ocurrido un error');
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error');
            }
        });
    } else {
        mostrarAlerta(msj);
    }
}

function getObjetoHistoriaFiltro() {

    var fechaIniHistoria = $('#idFechaIniHistoria').val();
    var fechaFinHistoria = $('#idFechaFinHistoria').val();

    var tipoProgramacion = parseInt($('#cboTipoProgramacionHistoria').val());
    var interIndispo = $('#cboInterDispo').val();
    var equicodiHistoria = $('#EquicodiHistoria').val() || "";
    var obj = {
        TipoProgramacion: tipoProgramacion,
        InterIndispo: interIndispo,
        Equicodi: equicodiHistoria,
        InterFechaIni: fechaIniHistoria,
        InterFechaFin: fechaFinHistoria,
    };

    return obj;
}

function validarConsultaHistoria(objFiltro) {
    var validacion = "<ul>";
    var flag = true;

    objFiltro.TipoProgramacion = parseInt(objFiltro.TipoProgramacion) || 0;
    if (objFiltro.TipoProgramacion <= 0) {
        validacion = validacion + "<li>Programación: campo requerido.</li>";//Campo Requerido
        flag = false;
    }

    // Valida filtros de fecha cuando falta seleccionar cualquiera de ellas
    if (objFiltro.InterFechaIni == "") {
        listaMsj.push("<li>Fecha Inicio: campo requerido.</li>");//Campo Requerido
        flag = false;
    }
    if (objFiltro.InterFechaFin == "") {
        validacion = validacion + "<li>Fecha Fin: campo requerido.</li>";//Campo Requerido
        flag = false;
    }

    // Valida consistencia del rango de fechas
    var interfechaini = objFiltro.InterFechaIni;
    var interfechafin = objFiltro.InterFechaFin;
    if (interfechaini != "" && interfechafin != "") {
        if (CompararFechas2(interfechaini, interfechafin) == false) {
            validacion = validacion + "<li>Fecha Inicio: debe ser menor que la fecha Fin.</li>";
            flag = false;
        }
    }

    validacion = validacion + "</ul>";

    if (flag == true) validacion = "";

    return validacion;
}

mostrarAlerta = function (alerta) {
    $('#mensaje2').html(alerta);
    $('#mensaje2').css("display", "block");
}

limpiarMensaje = function () {
    $('#mensaje2').html("");
    $('#mensaje2').css("display", "none");
}

function listarProgramas() { //CRUZADAS
    var idTipoPrograma = document.getElementById('tipoProgramacion').value;

    $.ajax({
        type: 'POST',
        url: controler + "ListarProgramas",
        datatype: "json",
        contentType: 'application/json',
        data: JSON.stringify({ idTipoPrograma: idTipoPrograma }),
        success: function (evt) {
            $('#listado').html(evt);
        },
        error: function () {
            alert("Lo sentimos, se ha producido un error");
        }
    });
}

function CompararFechas2(fecha1, fecha2) {

    //Split de las fechas recibidas para separarlas
    var x = fecha1.split('/');
    var z = fecha2.split('/');

    //Cambiamos el orden al formato americano, de esto dd/mm/yyyy a esto mm/dd/yyyy
    fecha1 = x[1] + '/' + x[0] + '/' + x[2];
    fecha2 = z[1] + '/' + z[0] + '/' + z[2];

    //Comparamos las fechas
    if (Date.parse(fecha1) > Date.parse(fecha2)) {
        return false;
    } else {
        return true;
    }
}