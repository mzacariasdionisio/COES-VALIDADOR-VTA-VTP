
var controlador = siteRoot + 'Equipamiento/FTReporte/';

var IMG_DESCARGAR_EXCEL = '<img src="' + siteRoot + 'Content/Images/ExportExcel.png" alt="Descargar Detalle" title="Descargar Detalle" width="19" height="19" style="">';

$(function () {
    $('#tab-container').easytabs({
        animate: false
    });
    $('#tab-container').easytabs('select', '#tabAdminFT');

    //////////////////// ADMIN //////////////////////////////
    $('#cbEmpresaAdmin').multipleSelect({
        width: '250px',
        filter: true,
    });
    $('#cbEmpresaAdmin').multipleSelect('checkAll');

    $('#FechaDesdeAdmin').Zebra_DatePicker({
        format: "d/m/Y",
        pair: $('#FechaHastaAdmin'),
        direction: false,
    });

    $('#FechaHastaAdmin').Zebra_DatePicker({
        format: "d/m/Y",
        pair: $('#FechaDesdeAdmin'),
        direction: true,
    });

    $('#btnBuscarAdmin').click(function () {
        buscarEnvioCumplimientoAdmin();
    });

    $('#btnExpotarAdmin').click(function () {
        exportarReporteAdminFT();
    });  
    
    buscarEnvioCumplimientoAdmin(); // muestra el listado de todos los envios de todas las empresas el filtro ingresado

    //////////////////// AREAS //////////////////////////////
    $('#cbEmpresaAreas').multipleSelect({
        width: '200px',
        filter: true,
    });
    $('#cbEmpresaAreas').multipleSelect('checkAll');

    $('#FechaDesdeAreas').Zebra_DatePicker({
        format: "d/m/Y",
        pair: $('#FechaHastaAreas'),
        direction: false,
    });

    $('#FechaHastaAreas').Zebra_DatePicker({
        format: "d/m/Y",
        pair: $('#FechaDesdeAreas'),
        direction: true,
    });

    $('#btnBuscarAreas').click(function () {
        buscarEnvioCumplimientoAreas();
    });

    $('#btnExpotarAreas').click(function () {
        exportarReporteAreas();
    });


    buscarEnvioCumplimientoAreas(); // muestra el listado de todos los envios de todas las empresas el filtro ingresado
});


////////////////////////////////////////////////
////////  Reporte Histórico Admin FT  //////////
////////////////////////////////////////////////
function buscarEnvioCumplimientoAdmin() {
    mostrarListadoCumplimientoAdminFT();
}

function mostrarListadoCumplimientoAdminFT() {
    limpiarBarraMensaje("mensaje");

    var filtro = datosFiltroAdminFT();
    var msg = validarDatosFiltroEnvios(filtro);
    var idEmpresa = filtro.todos ? "-1" : $('#hfEmpresaAdmin').val()
    if (msg == "") {
        $.ajax({
            type: 'POST',
            url: controlador + "ListarReporteCumplimientoAdminFT",
            dataType: 'json',
            data: {
                empresas: idEmpresa,
                ftetcodi: filtro.etapa,
                idProyecto: parseInt(filtro.proyecto),
                finicios: filtro.finicio,
                ffins: filtro.ffin
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    
                    var htmlEnvios = dibujarTablaReporteCumplimientoAdminFT(evt.ListaReporteCumplimientoAdminFT);
                    $("#reporte").html(htmlEnvios);
                    var tamAnchoh = parseInt($('.header').width());
                    var tamAncho = parseInt($('#mainLayout').width());
                    var tamA = tamAnchoh - 240;
                    $('#listadoGeneralEnvios').css("width", tamA + "px");
                    $('#listadoGeneralEnvios').css("height", "430px"); 
                    $('#listadoGeneralEnvios').css("overflow", "auto"); 
                   

                } else {
                    mostrarMensaje('mensaje', 'error', evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    } else {
        mostrarMensaje('mensaje', 'error', msg);
    }
}

function datosFiltroAdminFT() {
    var filtro = {};

    var empresa = $('#cbEmpresaAdmin').multipleSelect('getSelects');
    var etapa = parseInt($('#cbEtapaAdmin').val()) || 0;
    var proyecto = parseInt($("#cbProyectoAdmin").val()) || 0;    
    var finicio = $('#FechaDesdeAdmin').val();
    var ffin = $('#FechaHastaAdmin').val();
    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    $('#hfEmpresaAdmin').val(empresa);

    //verifico si esta seleccionado TODOS
    var Todos = false;
    var lstSel = [];
    $("#CajaPrincipal input:checkbox[name=selectAllIdEmpresa]:checked").each(function () {
        var textoFiltrado = $('#CajaPrincipal .ms-search input').val();
        if (textoFiltrado == "")
            lstSel.push($(this));
    });
    if (lstSel.length > 0)
        Todos = true;


    filtro.empresa = empresa;
    filtro.etapa = etapa;
    filtro.proyecto = proyecto;
    filtro.finicio = finicio;
    filtro.ffin = ffin;
    filtro.todos = Todos;

    return filtro;
}

function validarDatosFiltroEnvios(datos) {
    var msj = "";

    if (datos.empresa.length == 0) {
        msj += "<p>Debe escoger una empresa correcta.</p>";
    }

    if (datos.etapa == 0) {
        msj += "<p>Debe escoger una etapa correcta.</p>";
    } else {
        if (datos.etapa < -1 || datos.etapa > 4) {
            msj += "<p>Debe escoger una etapa correcta.</p>";
        }
    }

    if (datos.finicio == "") {
        if (datos.ffin == "") {
            msj += "<p>Debe escoger un rango inicial y final correctos.</p>";
        } else {
            msj += "<p>Debe escoger un rango inicial correcto.</p>";
        }
    } else {
        if (datos.ffin == "") {
            msj += "<p>Debe escoger un rango final correcto.</p>";
        } else {
            if (convertirFecha(datos.finicio) > convertirFecha(datos.ffin)) {
                msj += "<p>Debe escoger un rango correcto, la fecha final debe ser posterior o igual a la fecha inicial.</p>";
            }

        }
    }

    return msj;
}

function convertirFecha(fecha) {
    var arrayFecha = fecha.split('/');
    var dia = arrayFecha[0];
    var mes = arrayFecha[1];
    var anio = arrayFecha[2];

    var salida = anio + mes + dia;
    return salida;
}


function dibujarTablaReporteCumplimientoAdminFT(lista) {

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="tablaEnviosAdminFT" style="">
       <thead>
           <tr>
               <th style="width:60px;white-space: inherit;">Empresa</th>
               <th style="width:50px;white-space: inherit;">Etapa</th>
               <th style="width:70px;white-space: inherit;">Nombre <br>Proyecto</th>
               <th style="width:70px;white-space: inherit;">Equipo(s) Proyecto</th>
               <th style="width:70px;white-space: inherit;">Nombre Equipo(s)</th>               
               <th style="width:40px;white-space: inherit;">Código<br>del Envío</th>
               <th style="width:60px;white-space: inherit;">Acciones</th>
               <th style="width:60px;white-space: inherit;">Usuario Registro</th>
               <th style="width:60px;white-space: inherit;">Correo Usuario Registro</th>
               <th style="width:60px;white-space: inherit;">Fecha Hora</th>
               <th style="width:60px;white-space: inherit;">Condición</th>
    `;

    cadena += `
            </tr>
        </thead>
        <tbody>
    `;

    var usuario = "";

    for (var key in lista) {
        var item = lista[key];

        var rowspan = item.NumAcciones;

        cadena += `
            <tr>
                <td style="width:60px;white-space: inherit;" rowspan="${rowspan}">${item.Empresa}</td>
                <td style="width:50px;white-space: inherit;" rowspan="${rowspan}">${item.Etapa}</td>
                <td style="width:70px;white-space: inherit;" rowspan="${rowspan}">${item.NombreProyecto}</td>
        `;

        if (item.EquiposProyectoUnico.trim() != "") {
            var arrayNombEq = item.NombreEquipos.split(", ");
            var numEq = arrayNombEq.length;
            cadena += `
                <td style="width:70px;white-space: inherit;" rowspan="${rowspan}">
                    <div id="equiPyU_${item.Ftenvcodi}"> ${item.EquiposProyectoUnico}
            `;
            if (numEq > 1) {
                cadena += `
                        <a title="Ver Envío" href="JavaScript:mostrarMasProyecto(${item.Ftenvcodi});" style="float: right;color: green;font-weight: bold;">Ver Más</a>
                `;
            }
            cadena += `
                    </div>
                    <div id="equiPyT_${item.Ftenvcodi}" style="display:none;"> ${item.EquiposProyecto}
            `;
            if (numEq > 1) {
                cadena += `
                        <a title="Ver Envío" href="JavaScript:mostrarMenosProyecto(${item.Ftenvcodi});" style="float: right;color: green;font-weight: bold;">Ver Menos</a>
                `;
            }
            cadena += `
                    </div>                    
                </td>
            `;
        } else {
            cadena += `
                <td style="width:70px;white-space: inherit;" rowspan="${rowspan}">${item.EquiposProyecto}</td>
            `;
        }

        if (item.NombreEquiposUnico.trim() != "") {
            var arrayNombEq2 = item.NombreEquipos.split(", ");
            var numEq2 = arrayNombEq2.length;
            cadena += `
                <td style="width:70px;white-space: inherit;" rowspan="${rowspan}">
                    <div id="nomEquiU_${item.Ftenvcodi}"> ${item.NombreEquiposUnico}
            `;
            if (numEq2 > 1) {
                cadena += `
                        <a title="Ver Envío" href="JavaScript:mostrarMasEquipo(${item.Ftenvcodi});" style="float: right;color: green;font-weight: bold;">Ver Más</a>
                `;
            }
            cadena += `
                    </div>
                    <div id="nomEquiT_${item.Ftenvcodi}" style="display:none;" rowspan="${rowspan}"> ${item.NombreEquipos}
            `;
            if (numEq2 > 1) {
                cadena += `
                        <a title="Ver Envío" href="JavaScript:mostrarMenosEquipo(${item.Ftenvcodi});" style="float: right;color: green;font-weight: bold;">Ver Menos</a>
                `;
            }
            cadena += `
                    </div>
                </td>
            `;
        } else {
            cadena += `
                <td style="width:70px;white-space: inherit;" rowspan="${rowspan}">${item.NombreEquipos}</td>
            `;
        }

        cadena += `
                <td style="width:40px;white-space: inherit;" rowspan="${rowspan}">${item.Ftenvcodi}</td>
        `;

        $.each(item.ListaAcciones, function (index, object) {

            var condicion = object.Condicion != null ? object.Condicion : "";

            cadena += `
                <td style="width:200px;white-space: inherit;" class="text_left">${object.Accion}</td>
                <td style="width:60px;white-space: inherit;">${object.UsuarioRegistroNomb}</td>
                <td style="width:60px;white-space: inherit;">${object.UsuarioRegistroCorreo}</td>
                <td style="width:60px;white-space: inherit;">${object.FechaHora}</td>
                <td style="width:60px;white-space: inherit;" class="text_left">${condicion}</td>`

            cadena += `</tr>`;
        });

    }


    cadena += `                   
        </tbody >
    </table >
    `;

    return cadena;
}

function mostrarMasProyecto(id) {
    $("#equiPyU_" + id).css("display", "none");
    $("#equiPyT_" + id).css("display", "block");
}

function mostrarMenosProyecto(id) {
    $("#equiPyU_" + id).css("display", "block");
    $("#equiPyT_" + id).css("display", "none");
}

function mostrarMasEquipo(id) {
    $("#nomEquiU_" + id).css("display", "none");
    $("#nomEquiT_" + id).css("display", "block");
}

function mostrarMenosEquipo(id) {
    $("#nomEquiU_" + id).css("display", "block");
    $("#nomEquiT_" + id).css("display", "none");
}


//exportarReporteAdminFT
function exportarReporteAdminFT() {
    limpiarBarraMensaje("mensaje");

    var filtro = datosFiltroAdminFT();
    var msg = validarDatosFiltroEnvios(filtro);
    var idEmpresa = filtro.todos ? "-1" : $('#hfEmpresaAdmin').val()

    if (msg == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'GenerarReporteCumplimientoAdminFT',
            data: {
                empresas: idEmpresa,
                ftetcodi: filtro.etapa, 
                idProyecto: parseInt(filtro.proyecto),
                finicios: filtro.finicio,
                ffins: filtro.ffin
            },
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    window.location = controlador + "ExportarCumplimiento?file_name=" + evt.Resultado;
                } else {
                    mostrarMensaje('mensaje', 'error', 'Error: ' + evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    } else {
        mostrarMensaje('mensaje', 'error', msg);
    }
}


////////////////////////////////////////////////
//////////  Reporte Histórico Areas  ///////////
////////////////////////////////////////////////
function buscarEnvioCumplimientoAreas() {
    mostrarListadoCumplimientoAreas();
}

function mostrarListadoCumplimientoAreas() {
    limpiarBarraMensaje("mensaje");

    var filtro = datosFiltroAreas();
    var msg = validarDatosFiltroEnviosAreas(filtro);
    var idEmpresa = filtro.todos ? "-1" : $('#hfEmpresaAreas').val();
    if (msg == "") {
        $.ajax({
            type: 'POST',
            url: controlador + "ListarReporteCumplimientoAreas",
            dataType: 'json',
            data: {
                empresas: idEmpresa,
                ftetcodi: filtro.etapa,
                idProyecto: parseInt(filtro.proyecto),
                idArea: parseInt(filtro.area),
                
                finicios: filtro.finicio,
                ffins: filtro.ffin
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {

                    var htmlEnvios = dibujarTablaReporteCumplimientoAreas(evt.ListaReporteCumplimientoRevAreas);
                    $("#reporteAreas").html(htmlEnvios);
                    var tamAnchoh = parseInt($('.header').width());
                    var tamAncho = parseInt($('#mainLayout').width());
                    var tamA = tamAnchoh - 240;
                    $('#listadoGeneralEnviosAreas').css("width", tamA + "px");
                    $('#listadoGeneralEnviosAreas').css("height", "430px");
                    $('#listadoGeneralEnviosAreas').css("overflow", "auto");


                } else {
                    mostrarMensaje('mensaje', 'error', evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    } else {
        mostrarMensaje('mensaje', 'error', msg);
    }
}

function datosFiltroAreas() {
    var filtro = {};

    var empresa = $('#cbEmpresaAreas').multipleSelect('getSelects');
    var etapa = parseInt($('#cbEtapaAreas').val()) || 0;
    var proyecto = parseInt($("#cbProyectoAreas").val()) || 0;
    var area = parseInt($("#cbAreaAreas").val()) || 0;
    
    var finicio = $('#FechaDesdeAreas').val();
    var ffin = $('#FechaHastaAreas').val();
    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    $('#hfEmpresaAreas').val(empresa);

    //verifico si esta seleccionado TODOS
    var Todos = false;
    var lstSel = [];
    $("#CajaPrincipalAreas input:checkbox[name=selectAllIdEmpresaAreas]:checked").each(function () {        
        var textoFiltrado = $('#CajaPrincipalAreas .ms-search input').val();
        if (textoFiltrado == "")
            lstSel.push($(this));
    });
    if (lstSel.length > 0)
        Todos = true;


    filtro.empresa = empresa;
    filtro.etapa = etapa;
    filtro.proyecto = proyecto;
    filtro.area = area;
    filtro.finicio = finicio;
    filtro.ffin = ffin;
    filtro.todos = Todos;

    return filtro;
}

function validarDatosFiltroEnviosAreas(datos) {
    var msj = "";

    if (datos.empresa.length == 0) {
        msj += "<p>Debe escoger una empresa correcta.</p>";
    }

    if (datos.etapa == 0) {
        msj += "<p>Debe escoger una etapa correcta.</p>";
    } else {
        if (datos.etapa < -1 || datos.etapa > 4) {
            msj += "<p>Debe escoger una etapa correcta.</p>";
        }
    }

    if (datos.finicio == "") {
        if (datos.ffin == "") {
            msj += "<p>Debe escoger un rango inicial y final correctos.</p>";
        } else {
            msj += "<p>Debe escoger un rango inicial correcto.</p>";
        }
    } else {
        if (datos.ffin == "") {
            msj += "<p>Debe escoger un rango final correcto.</p>";
        } else {
            if (convertirFecha(datos.finicio) > convertirFecha(datos.ffin)) {
                msj += "<p>Debe escoger un rango correcto, la fecha final debe ser posterior o igual a la fecha inicial.</p>";
            }

        }
    }

    return msj;
}


function dibujarTablaReporteCumplimientoAreas(lista) {

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="tablaEnviosAreas" style="">
       <thead>
           <tr>
               <th style="width:60px;white-space: inherit;">Empresa</th>
               <th style="width:50px;white-space: inherit;">Etapa</th>
               <th style="width:70px;white-space: inherit;">Nombre <br>Proyecto</th>
               <th style="width:70px;white-space: inherit;">Equipo(s) Proyecto</th>
               <th style="width:70px;white-space: inherit;">Nombre Equipo(s)</th>               
               <th style="width:40px;white-space: inherit;">Código<br>del Envío</th>
               <th style="width:60px;white-space: inherit;">Acciones</th>
               <th style="width:60px;white-space: inherit;">Área(s) COES <br>asignada(s)</th>
               <th style="width:60px;white-space: inherit;">Usuario Registro</th>
               <th style="width:60px;white-space: inherit;">Plazo límite de respuesta <br> (Área(s) COES)</th>
               <th style="width:60px;white-space: inherit;">Correo Usuario Registro</th>
               <th style="width:60px;white-space: inherit;">Fecha Hora</th>
               <th style="width:60px;white-space: inherit;">Condición</th>
    `;

    cadena += `
            </tr>
        </thead>
        <tbody>
    `;

    var usuario = "";

    for (var key in lista) {
        var item = lista[key];

        var rowspan = item.NumAcciones;

        cadena += `
            <tr>
                <td style="width:60px;white-space: inherit;" rowspan="${rowspan}">${item.Empresa}</td>
                <td style="width:50px;white-space: inherit;" rowspan="${rowspan}">${item.Etapa}</td>
                <td style="width:70px;white-space: inherit;" rowspan="${rowspan}">${item.NombreProyecto}</td>
        `;

        if (item.EquiposProyectoUnico.trim() != "") {
            var arrayNombEq = item.NombreEquipos.split(", ");
            var numEq = arrayNombEq.length;
            cadena += `
                <td style="width:70px;white-space: inherit;" rowspan="${rowspan}">
                    <div id="equiPyU_${item.Ftenvcodi}"> ${item.EquiposProyectoUnico}
            `;
            if (numEq > 1) {
                cadena += `
                        <a title="Ver Envío" href="JavaScript:mostrarMasProyecto(${item.Ftenvcodi});" style="float: right;color: green;font-weight: bold;">Ver Más</a>
                `;
            }
            cadena += `
                    </div>
                    <div id="equiPyT_${item.Ftenvcodi}" style="display:none;"> ${item.EquiposProyecto}
            `;
            if (numEq > 1) {
                cadena += `
                        <a title="Ver Envío" href="JavaScript:mostrarMenosProyecto(${item.Ftenvcodi});" style="float: right;color: green;font-weight: bold;">Ver Menos</a>
                `;
            }
            cadena += `
                    </div>                    
                </td>
            `;
        } else {
            cadena += `
                <td style="width:70px;white-space: inherit;" rowspan="${rowspan}">${item.EquiposProyecto}</td>
            `;
        }

        if (item.NombreEquiposUnico.trim() != "") {
            var arrayNombEq2 = item.NombreEquipos.split(", ");
            var numEq2 = arrayNombEq2.length;
            cadena += `
                <td style="width:70px;white-space: inherit;" rowspan="${rowspan}">
                    <div id="nomEquiU_${item.Ftenvcodi}"> ${item.NombreEquiposUnico}
            `;
            if (numEq2 > 1) {
                cadena += `
                        <a title="Ver Envío" href="JavaScript:mostrarMasEquipo(${item.Ftenvcodi});" style="float: right;color: green;font-weight: bold;">Ver Más</a>
                `;
            }
            cadena += `
                    </div>
                    <div id="nomEquiT_${item.Ftenvcodi}" style="display:none;" rowspan="${rowspan}"> ${item.NombreEquipos}
            `;
            if (numEq2 > 1) {
                cadena += `
                        <a title="Ver Envío" href="JavaScript:mostrarMenosEquipo(${item.Ftenvcodi});" style="float: right;color: green;font-weight: bold;">Ver Menos</a>
                `;
            }
            cadena += `
                    </div>
                </td>
            `;
        } else {
            cadena += `
                <td style="width:70px;white-space: inherit;" rowspan="${rowspan}">${item.NombreEquipos}</td>
            `;
        }

        cadena += `
                <td style="width:40px;white-space: inherit;" rowspan="${rowspan}">${item.Ftenvcodi}</td>
        `;

        $.each(item.ListaAcciones, function (index, object) {

            var condicion = object.Condicion != null ? object.Condicion : "";

            cadena += `
                <td style="width:200px;white-space: inherit;" class="text_left">${object.Accion}</td>
                <td style="width:60px;white-space: inherit;">${object.AreasCOESAsignadas}</td>
                <td style="width:60px;white-space: inherit;">${object.UsuarioRegistroNomb}</td>
                <td style="width:60px;white-space: inherit;">${object.PlazoLimiteRpta}</td>
                <td style="width:60px;white-space: inherit;">${object.UsuarioRegistroCorreo}</td>
                <td style="width:60px;white-space: inherit;">${object.FechaHora}</td>
                <td style="width:60px;white-space: inherit;">${condicion}</td>`

            cadena += `</tr>`;
        });

    }


    cadena += `                   
        </tbody >
    </table >
    `;

    return cadena;
}


function exportarReporteAreas() {
    limpiarBarraMensaje("mensaje");
      
    var filtro = datosFiltroAreas();
    var msg = validarDatosFiltroEnviosAreas(filtro);
    var idEmpresa = filtro.todos ? "-1" : $('#hfEmpresaAreas').val()

    if (msg == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'GenerarReporteCumplimientoAreas',
            data: {
                empresas: idEmpresa,
                ftetcodi: filtro.etapa,
                idProyecto: parseInt(filtro.proyecto),
                idArea: parseInt(filtro.area),
                finicios: filtro.finicio,
                ffins: filtro.ffin
            },
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    window.location = controlador + "ExportarCumplimiento?file_name=" + evt.Resultado;
                } else {
                    mostrarMensaje('mensaje', 'error', 'Error: ' + evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    } else {
        mostrarMensaje('mensaje', 'error', msg);
    }
}


////////////////////////////////////////////////
// Util
////////////////////////////////////////////////
function cerrarPopup(id) {
    $("#" + id).bPopup().close()
}

function abrirPopup(contentPopup) {
    setTimeout(function () {
        $("#" + contentPopup).bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
}


function mostrarMensaje(id, tipo, mensaje) {
    $("#" + id).css("display", "block");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);

}

function limpiarBarraMensaje(id) {
    $('#' + id).css("display", "none");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-message');
    $('#' + id).html('');
}