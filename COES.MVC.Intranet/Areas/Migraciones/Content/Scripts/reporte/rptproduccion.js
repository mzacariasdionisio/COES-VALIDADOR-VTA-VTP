var controlador = siteRoot + 'Migraciones/Reporte/';

const PROG_SEMANAL = "3";
const PROG_DIARIO = "4";
const REPROG_DIARIO = "5";
const EJECUTADO = "6";
const NUM_DECIMALES = 3;
const NOMBTABLA_C = "tablaReportePHC";
const NOMBTABLA_NC = "tablaReportePHNC";

$(function () {
    //Fechas
    $('#cbInformacion').val(EJECUTADO);

    $('#txtFechaIni').Zebra_DatePicker({
        format: "d/m/Y",
        pair: $('#txtFechaFin'),
        direction: false,
        onSelect: function (date) { },
        onChange: function (view, elements) { },
    });

    $('#txtFechaFin').Zebra_DatePicker({
        format: "d/m/Y",
        pair: $('#txtFechaIni'),
        direction: [true, "01/01/2050"],
       
    });

    $('#cbInformacion').change(function () {
        resetearCampoFechas();        
    });

    $('#btnConsultar').click(function () {
        mostrarListadoRepProduccion();
    });

    $('#btnExportar').click(function () {
        exportarRepProduccion();
    });

    mostrarListadoRepProduccion();
    
});

function resetearCampoFechas() {
    let opcionEscogida = $('#cbInformacion').val();
    let fecIni = "";
    let fecFin = "";

    switch (opcionEscogida) {
        case PROG_SEMANAL:
            fecIni = $('#hdFecIniSemana').val(); 
            fecFin = $('#hdFecFinSemana').val(); 
            break;
        case PROG_DIARIO:
            fecIni = obtenerFecha(1);
            fecFin = obtenerFecha(1);
            break;
        case REPROG_DIARIO:
            fecIni = obtenerFecha(0);
            fecFin = obtenerFecha(0);
            break;
        case EJECUTADO:
            fecIni = obtenerFecha(-1);
            fecFin = obtenerFecha(-1);
            break;        
    }

    $('#txtFechaIni').val(fecIni);
    $('#txtFechaFin').val(fecFin);

    $('#txtFechaIni').Zebra_DatePicker({
        format: "d/m/Y",
        pair: $('#txtFechaFin'),
        direction: false,
        onSelect: function (date) {
            $('#txtFechaFin').Zebra_DatePicker({
                format: "d/m/Y",
                pair: $('#txtFechaIni'),
                direction: [$('#txtFechaIni').val(), "01/01/2050"]
            });
        },
        
    });

    $('#txtFechaFin').Zebra_DatePicker({
        format: "d/m/Y",
        pair: $('#txtFechaIni'),
        direction: [$('#txtFechaIni').val(), "01/01/2050"]
    });

}

function mostrarListadoRepProduccion() {
    limpiarBarraMensaje("mensaje");

    var objData = getObjetoFiltro();
    var msj = validarConsulta(objData);

    if (msj == "") {

        $.ajax({
            type: 'POST',
            url: controlador + "listarRepProduccion",
            data: {
                tipo: objData.Tipo,
                fechaInicial: objData.FechaInicio,
                fechaFinal: objData.FechaFin
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    
                    let numL1 = evt.DatosRepGeneracionCoes.ListadoRegistros.length;
                    let numL2 = evt.DatosRepGeneracionNoCoes.ListadoRegistros.length;

                    var htmlReporteCoes = dibujarTablaReporte(evt.DatosRepGeneracionCoes, NOMBTABLA_C, "REPORTE DE EMPRESAS COES" );
                    $("#listadoProdHidrolCoes").html(htmlReporteCoes);
                    var htmlReporteNoCoes = dibujarTablaReporte(evt.DatosRepGeneracionNoCoes, NOMBTABLA_NC, "REPORTE DE EMPRESAS NO COES");
                    $("#listadoProdHidrolNoCoes").html(htmlReporteNoCoes);
                    
                    var tamAnchoh = parseInt($('.header').width());
                    var tamA = tamAnchoh - 240;

                    $("#listado1").removeAttr("style");
                    if (numL1 > 0) {
                        $('#listado1').css("width", tamA + "px");
                        $('#listado1').css("height", "250px");
                        $('#listado1').css("overflow", "auto");
                    } else {
                        $('#' + NOMBTABLA_C).dataTable({
                            "scrollCollapse": true, //colapsa si no supera los 200
                            "scrollY": 200,
                            "scrollX": false,
                            "sDom": 't',
                            "ordering": false,
                            "iDisplayLength": -1
                        });
                    }

                    $("#listado2").removeAttr("style");
                    if (numL2 > 0) {
                        $('#listado2').css("width", tamA + "px");
                        $('#listado2').css("height", "250px");
                        $('#listado2').css("overflow", "auto");
                    } else {

                        $('#' + NOMBTABLA_NC).dataTable({
                            "scrollCollapse": true, //colapsa si no supera los 200
                            "scrollY": 200,
                            "scrollX": false,
                            "sDom": 't',
                            "ordering": false,
                            "iDisplayLength": -1
                        });
                    }                    

                } else {
                    mostrarMensaje('mensaje', 'error', evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    } else {
        mostrarMensaje('mensaje', 'error', msj);
    }
}

function getObjetoFiltro() {

    var tipo = parseInt($('#cbInformacion').val()) || 0;
    var fechaInicio = $('#txtFechaIni').val();
    var fechaFin = $('#txtFechaFin').val();

    var obj = {
        Tipo: tipo,
        FechaInicio: fechaInicio,
        FechaFin: fechaFin
    };

    return obj;
}

function validarConsulta(objFiltro) {
    var listaMsj = [];

    // Valida consistencia del rango de fechas
    var fechaInicio = objFiltro.FechaInicio;
    var fechaFin = objFiltro.FechaFin;
    var tipo = objFiltro.Tipo;

    if (tipo < 3 || tipo > 6) {
        listaMsj.push("Debe escoger una opcion en el campo 'Información'.");
    }
    if (fechaInicio != "" && fechaFin != "") {
        if (CompararFechas(fechaInicio, fechaFin) == false) {
            listaMsj.push("La fecha de inicio no puede ser mayor que la fecha de fin.");
        }
    }

    var msj = listaMsj.join('\n');
    return msj;
}

function dibujarTablaReporte(datos, nombTabla, titulo) {

    var listado = datos.ListadoRegistros;
    var cadena = '';


    let idT = "";
    switch (nombTabla) {
        case NOMBTABLA_C: idT = "fecMDC"; break;
        case NOMBTABLA_NC: idT = "fecMDNC"; break;
    }

    $("#" + idT).html("");
    var primerLista = listado[0];
    if (primerLista != null && primerLista != undefined && nombTabla == NOMBTABLA_C) {
        let fechMD = primerLista.StrFechaMaxDemanda;
        if (fechMD != "") {
            $("#" + idT).html("* Máxima Demanda: <div style='float: inline-end;'> &nbsp;" + fechMD + " h </div>");
        }
    }
    cadena += `
    <table border="0" class="pretty tabla-icono" id="${nombTabla}" >
       <thead>
            <tr style="height: 25px; ">
               
               <th colspan="7" style=" font-size: 13px;" >${titulo}</th>
            </tr>
           <tr style="height: 22px;">
               <th>Empresa</th>
               <th>Código Punto GD</th>
               <th>Central/Unidad</th>
               <th>E. Hidráulica <br> MWh.</th>
               <th>E. Térmica <br> MWh.</th>
               <th>Total <br> MWh.</th>
               <th>Máx. Dem. (*)<br> MW.</th>                              
            </tr>
        </thead>
        <tbody>
    `;
    let idEmpresa = -1;
    var valorTotalSein = datos.StrTotalSein;
    var valorTotalMD = datos.StrTotalMD;

    for (var key in listado) {
        var item = listado[key];
        let emprcodi = item.Emprcodi;
        var numReg = item.NumReg;
        var valorEH = item.StrEHidro;
        var valorET = item.StrETermo;
        var valorT = item.StrETotal;
        var valorMD = item.StrMaxDemanda;
        

        if (idEmpresa != emprcodi) {
            cadena += `
                <tr>
                    <td rowspan="${numReg}">${item.Emprnomb}</td>                    
            `;
        } 

        cadena += `
                <td>${item.Ptomedicodi}</td>
                <td>${item.UnidadNomb}</td>
                <td style="text-align: end;">${valorEH}</td>
                <td style="text-align: end;">${valorET}</td>
                <td style="text-align: end;">${valorT}</td>
                <td style="text-align: end;">${valorMD}</td>
           </tr >
        `;
        idEmpresa = emprcodi;
    }

    cadena += `
        </tbody >
    `;

    cadena += `
        <tfoot>
            
           <tr style="height: 22px;">
               <td style="padding-top: 4px; text-align: center;">TOTAL</td>
               <td colspan="4"></td>
               <td style="padding-top: 4px; text-align: end;">${valorTotalSein}</td>
               <td style="padding-top: 4px; text-align: end;">${valorTotalMD}</td>
            </tr>
        </tfoot>
    </table >
    `;

    return cadena;
}

function exportarRepProduccion() { 
    limpiarBarraMensaje("mensaje");

    var objData = getObjetoFiltro();
    var msj = validarConsulta(objData);

    if (msj == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'GenerarArchivoRP',
            data: {
                tipo: objData.Tipo,
                fechaInicial: objData.FechaInicio,
                fechaFinal: objData.FechaFin
            },
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    window.location = controlador + "ExportarReporte?file_name=" + evt.Resultado;
                } else {
                    mostrarMensaje('mensaje', 'error', 'Error: ' + evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    } else {
        mostrarMensaje('mensaje', 'error', msj);
    }
}


/**********************************************/
/************ Funciones Generales  ************/


//Devuelve numero con 3 decimales, recortados si tiene mas de 3 decimales
function CompararFechas(fecha1, fecha2) {

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

function AddZero(num) {
    return (num >= 0 && num < 10) ? "0" + num : num + "";
}

function obtenerFecha(diasAgregarQuitar) { //devuelve strFecha en formato dd/mm/yyyy
    var hoy = new Date();
    let DIA_EN_MILISEGUNDOS = 0;

    if (diasAgregarQuitar != 0) {
        DIA_EN_MILISEGUNDOS = (diasAgregarQuitar * 24) * 60 * 60 * 1000;
    } 
    
    let fecha = new Date(hoy.getTime() + DIA_EN_MILISEGUNDOS);
    var strDateTime = [[AddZero(fecha.getDate()), AddZero(fecha.getMonth() + 1), fecha.getFullYear()].join("/")].join(" ");

    return strDateTime;
}

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
