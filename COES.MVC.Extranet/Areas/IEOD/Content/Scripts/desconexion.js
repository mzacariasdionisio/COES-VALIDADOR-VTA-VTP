var controlador = siteRoot + 'IEOD/DesconexionEquipos/';
var evtHot;

var APP_OPCION = -1;
var OPCION_COPIAR = 1;
var OPCION_EDITAR = 2;
var OPCION_NUEVO = 3;
var OPCION_ELIMINAR = 4;
var OPCION_VER = 5;
var OPCION_ENVIO_DATOS = 6;

$(function () {

    $('#txtFecha').Zebra_DatePicker({
    });
    idFuenteDatos = idFuenteDatosDesconexionEquipo;
    idFormato = 0;
    tipoFormato = 1;
    dibujarPanelIeod(tipoFormato, 3, -1);

    $('#cbEmpresa').change(function () {
        limpiarBarra();
        dibujarPanelIeod(tipoFormato, 3, -1);
        hideMensaje();
        hideMensajeEvento();
    });

    $('#btnConsultar').click(function () {
        generaListado();
    });

    $('#btnVerEnvios').click(function () {
        popUpListaEnvios();
    });

    $('#btnAgregarDesconexion').click(function () {
        ichorini = $('#txtFecha').val();
        popUpAgregarDesconexion(0, -1, ichorini, "", "", "");
    });

    $('#btnEnviarDatos').click(function () {
        if (evtHot.EnabledDesconexion) {
            grabarEnvio();
        }
        else {
            alert("No se puede enviar información, solo diponible para  visualización");
        }
    });

    $('#btnCopiarDesconexion').click(function () {
        hideMensajeEvento();
        generaDesconexionAuto();
    });
    cargarValoresIniciales();
});

function cargarValoresIniciales() {
    limpiarBarra();
}

function generaListado() {
    $('#mensaje').css("display", "none");
    if ($("#txtFecha").val() != "") {
        hideMensaje();
        hideMensajeEvento();
        $('#barraDesconexionesEquipos').css("display", "table");
        $("#hfIdEnvio").val(0);
        mostrarListado();
    }
    else {
        alert("Error!.Ingresar fecha correcta");
    }
}

function mostrarListado(opcion) {

    var idEmpresa = $('#cbEmpresa').val();
    var sFecha = $('#txtFecha').val()
    var idenvio = $("#hfIdEnvio").val();
    $.ajax({
        type: 'POST',
        url: controlador + "lista",
        data: {
            idEmpresa: idEmpresa,
            sFecha: sFecha,
            idEnvio: idenvio
        },
        success: function (evt) {
            if (evt.error == undefined) {
                evtHot = evt;

                switch (opcion) {
                    case OPCION_ENVIO_DATOS:
                        var mensaje = mostrarMensajeEnvioFuenteDatos(evt);
                        mostrarExito("Los datos se enviaron correctamente. " + mensaje);
                        break;
                    default:
                        var mensaje = mostrarMensajeEnvioFuenteDatos(evt);
                        mostrarMensaje(mensaje);
                        break;
                }

                if (idenvio > 0 || evtHot.PlazoEnvio.TipoPlazo == "D") {
                    esconderOpcionBarra();
                }
                else {
                    mostrarOpcionBarra();
                }

                seteaMatriz(evtHot);
                $('#listado').css("width", $('#mainLayout').width() + "px");
                $('#listado').html(dibujarTablaDesconexiones(evtHot));
                $('#listado').css("display", "block");
            }
            else {
                alert("Error:" + evt.descripcion);
            }

            dibujarPanelIeod(tipoFormato, 3, -1);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function esconderOpcionBarra() {
    $('#btnEnviarDatos').css("display", "none");
    $('#btnAgregarDesconexion').css("display", "none");
}

function mostrarOpcionBarra() {
    $('#btnEnviarDatos').css("display", "block");
    $('#btnAgregarDesconexion').css("display", "block");
}

function dibujarTablaDesconexiones(evt) {

    var strHtml = "<table  class='pretty tabla-icono' cellspacing='0' width='100%' id='tabla'>";
    strHtml += "<thead>";
    strHtml += "<tr><th>Empresa</th><th>Equipo</th><th>Tipo Equipo</th><th>Inicio</th><th>Fin</th><th>Usuario</th><th>Modificacion</th><th></th><th><th></th></th></tr>";
    strHtml += "</thead>";
    strHtml += "<tbody>";
    strHtml += generaViewListaDesconexiones(evt);
    strHtml += "</tbody></table>";
    return strHtml;
}

function generaViewListaDesconexiones(evt) {
    var cadenaHtml = "";
    estiloTabla = "style='text-align: left;padding-left:10px;";
    estiloTabla2 = "";
    if (!evt.EnabledDesconexion) {
        estiloTabla += ";background:#999999 !important;";
        estiloTabla2 = "style='background:#999999 !important;'";
    }
    estiloTabla += "'";
    for (var i = 0; i < evt.ListaDesconexiones.length; i++) {
        if (evt.ListaDesconexiones[i].opCrud != -1) { // no mostramos los elementos que han sido eliminados que vienen de BD y siguen en memoria
            var inicio = evt.ListaDesconexiones[i].Ichorini;
            var fin = evt.ListaDesconexiones[i].Ichorfin;
            var lasdate = evt.ListaDesconexiones[i].Lastdate;
            cadenaHtml += "<tr>";
            cadenaHtml += "<td " + estiloTabla + ">" + evt.ListaDesconexiones[i].Emprnomb + "</td>";
            cadenaHtml += "<td " + estiloTabla + ">" + evt.ListaDesconexiones[i].Equinomb + "</td>";
            cadenaHtml += "<td " + estiloTabla + ">" + evt.ListaDesconexiones[i].Famabrev + "</td>";
            cadenaHtml += "<td " + estiloTabla + ">" + inicio + "</td>";
            cadenaHtml += "<td " + estiloTabla + ">" + fin + "</td>";
            cadenaHtml += "<td " + estiloTabla + ">" + evt.ListaDesconexiones[i].Lastuser + "</td>";
            cadenaHtml += "<td " + estiloTabla + ">" + lasdate + "</td>";

            if (evt.EnabledDesconexion) {
                cadenaHtml += "<td style='cursor:pointer;width:30px;'>";
                cadenaHtml += "<img onclick='verAdjunto(" + evt.ListaDesconexiones[i].Iccodi + ");' src='" + siteRoot + "Content/Images/file.png' title='Ver Archivo' alt='Ver Archivo'/></td>";
                cadenaHtml += "<td  style='cursor:pointer;width:30px;'>";
                cadenaHtml += "<img onclick='editarDesconexion(" + evt.ListaDesconexiones[i].Iccodi + ");' src='" + siteRoot + "Content/Images/btn-edit.png' title='Editar Desconexion' alt='Editar Desconexion'/></td>";
                cadenaHtml += "<td  style='cursor:pointer;width:30px;'>";
                cadenaHtml += "<img onclick='eliminarDesconexion(" + evt.ListaDesconexiones[i].Iccodi + ");' src='" + siteRoot + "Content/Images/btn-cancel.png' title='Eliminar Desconexion' alt='Eliminar Desconexion'/></td>";
            }
            else {
                cadenaHtml += "<td " + estiloTabla2 + ">";
                cadenaHtml += "<img onclick='verAdjunto(" + evt.ListaDesconexiones[i].Iccodi + ");' src='" + siteRoot + "Content/Images/file.png' title='Ver Archivo' alt='Ver Archivo'/></td>";
                cadenaHtml += "<td " + estiloTabla2 + "><img src='" + siteRoot + "Content/Images/btn-edit.png' title='Editar Desconexion' alt='Editar Desconexion'/></td>";
                cadenaHtml += "<td " + estiloTabla2 + ">";
                cadenaHtml += "<img src='" + siteRoot + "Content/Images/btn-cancel.png' title='Eliminar Desconexion' alt='Eliminar Desconexion'/></td>";
            }

            cadenaHtml += "</tr>";
        }
    }
    return cadenaHtml;
}

function actualizaListadoDesconexiones(evt) {
    $('#listado').css("width", $('#mainLayout').width() + "px");
    $('#listado').html(dibujarTablaDesconexiones(evtHot));
    //$('#tabla').dataTable({
    //    "scrollY": 430,
    //    "scrollX": true,
    //    "ordering": false,
    //    "sDom": 't',
    //    "iDisplayLength": 50
    //});
}

function popUpAgregarDesconexion(iccodi, equicodi, icfechaini, icfechafin, icdescrip1, icnombarchfisico) {
    var idEmpresa = $('#cbEmpresa').val();
    icfini = icfechaini.split(' ')[0];
    ichorini = icfechaini.split(' ')[1];
    ichorfin = icfechafin.split(' ')[1];
    icffin = icfechafin.split(' ')[0];
    $.ajax({
        type: 'POST',
        url: controlador + 'ViewDesconexion',
        async: false,
        data: {
            idEmpresa: idEmpresa,
            fechaIni: icfini,
            fechaFin: icffin,
            iccodi: iccodi,
            equicodi: equicodi,
            ichorini: ichorini,
            ichorfin: ichorfin,
            icdescrip1: icdescrip1,
            archEnvio: icnombarchfisico
        },
        success: function (result) {
            $('#idDesconexion').html(result);
            setTimeout(function () {
                $('#newDesconexion').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    modalClose: false
                });
            }, 50);
        },
        error: function (result) {
            alert('ha ocurrido un error al generar vista');
        }
    });
}

function popUpListaEnvios() {
    $('#idEnviosAnteriores').html(dibujarTablaEnvios(evtHot.ListaEnvios));
    setTimeout(function () {
        $('#enviosanteriores').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
        $('#tablalenvio').dataTable({
            "scrollY": 330,
            "scrollX": true,
            "sDom": 't',
            "ordering": false,
            "bPaginate": false,
            "iDisplayLength": -1
        });
    }, 50);
}
function editarDesconexion(iccodi) {
    var equicodi = -1;
    var ichorini = "";
    var ichorfin = "";
    var icdescrip1 = "";
    var icnombarchenvio = "";
    //if (iccodi < 0) { //registro no esta en BD.
    for (var i = 0; i < evtHot.ListaDesconexiones.length; i++) {
        if (evtHot.ListaDesconexiones[i].Iccodi == iccodi) {// encontrado      
            equicodi = evtHot.ListaDesconexiones[i].Equicodi;
            ichorini = evtHot.ListaDesconexiones[i].Ichorini;
            ichorfin = evtHot.ListaDesconexiones[i].Ichorfin;
            icdescrip1 = evtHot.ListaDesconexiones[i].Icdescrip1;
            icnombarchenvio = evtHot.ListaDesconexiones[i].Icnombarchenvio;
            evtHot.ListaDesconexiones[i].IcnombarchfisicoAnt = evtHot.ListaDesconexiones[i].Icnombarchfisico;
        }
    }
    // }
    popUpAgregarDesconexion(iccodi, equicodi, ichorini, ichorfin, icdescrip1, icnombarchenvio);
}

function eliminarDesconexion(iccodi) {

    if (confirm("¿Desea eliminar Desconexion...?")) {
        if (evtHot.ListaDesconexiones.length > 0) {
            for (var i = 0; i < evtHot.ListaDesconexiones.length; i++) {
                if (evtHot.ListaDesconexiones[i].Iccodi == iccodi) {// encontrado
                    if (iccodi > 0) { // registro viene de BD
                        evtHot.ListaDesconexiones[i].opCrud = -1 // eliminado logico
                    }
                    else {
                        evtHot.ListaDesconexiones.splice(i, 1); //eliminado fisico de la matriz auxiliar
                    }
                }
            }
        }
        actualizaListadoDesconexiones(evtHot);
    }

}

function verAdjunto(id) {

    for (var i = 0; i < evtHot.ListaDesconexiones.length; i++) {
        if (evtHot.ListaDesconexiones[i].Iccodi == id) {
            Icnombarchfisico = evtHot.ListaDesconexiones[i].Icnombarchfisico;
            cambioArch = evtHot.ListaDesconexiones[i].CambioArchivo;
        }
    }
    if (cambioArch == null || cambioArch == undefined) { cambioArch = 0; }
    if (Icnombarchfisico == "" || Icnombarchfisico == null) {
        alert("No se encontro el archivo adjunto o no existe!");
    }
    else {
        window.location = controlador + 'DescargarArchivoDesconexion?archivo=' + Icnombarchfisico + '&icccodi=' + id + '&cambio=' + cambioArch;
    }

}

function obtenerMatrizEnviar(lista) {
    var listaData = []
    for (var i = 0; i < lista.length; i++) {
        var fechaIni = obtenerDate(lista[i].Ichorini);
        var fechaFin = obtenerDate(lista[i].Ichorfin);
        var fechaLastdate = obtenerDate(lista[i].Lastdate);

        var entity = JSON.parse(JSON.stringify(lista[i]));
        entity.Ichorini = fechaIni;
        entity.Ichorfin = fechaFin;
        entity.Lastdate = fechaLastdate;

        listaData.push(entity);
    }

    return listaData;
}

function obtenerDate(formato) {
    //moment(convertStringToDate(sFecha, "00:00:00")).format('DD/MM/YYYY HH:mm:ss')
    var fecha = formato.substring(0, 10);
    var hora = formato.substring(11, formato.length);
    var fechaDate = convertStringToDate(fecha, hora);
    return fechaDate;
}

function grabarEnvio() {
    var enviocodi = $('#hcodenvio').val();
    var idEmpresa = $('#cbEmpresa').val();
    var fecha = $('#txtFecha').val();

    if (confirm("¿Desea enviar información a COES?")) {
        hideMensaje();

        var matriz = obtenerMatrizEnviar(evtHot.ListaDesconexiones);
        $.ajax({
            type: 'POST',
            //async: false,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            traditional: true,
            url: controlador + "RegistrarEnvioDesconexiones",
            data: JSON.stringify({
                data: matriz,
                idEmpresa: idEmpresa,
                fecha: fecha
            }),
            beforeSend: function () {
                mostrarExito("Enviando Información ..");
            },
            success: function (evt) {
                if (evt.Resultado == 1) {
                    $("#hfIdEnvio").val(evt.IdEnvio);
                    mostrarListado(OPCION_ENVIO_DATOS);
                }
                else {
                    mostrarError("Error al Grabar");
                }
            },
            error: function () {
                mostrarError();
            }

        });
        dibujarPanelIeod(1, 3, -1);
    }
}

/// Muestra los envios anteriores
function dibujarTablaEnvios(lista) {

    var cadena = "<div style='clear:both; height:5px'></div> ";
    cadena += "<table id='tablalenvio' border='1' class='pretty tabla-adicional' cellspacing='0'>";
    cadena += "<thead><tr><th>Id Envío</th><th>Fecha Hora</th><th>Usuario</th></tr></thead>";
    cadena += "<tbody>";

    for (key in lista) {
        var javaScriptDate = new Date(parseInt(lista[key].Enviofecha.substr(6)));
        cadena += "<tr onclick='mostrarEnvioDesconexiones(" + lista[key].Enviocodi + ");' style='cursor:pointer'><td>" + lista[key].Enviocodi + "</td>";
        cadena += "<td>" + getFormattedDate(javaScriptDate) + "</td>";
        cadena += "<td>" + lista[key].Lastuser + "</td></tr>";
    }
    cadena += "</tbody></table>";
    return cadena;

}

function mostrarEnvioDesconexiones(envio) {
    hideMensaje();
    hideMensajeEvento();
    $('#enviosanteriores').bPopup().close();
    $("#hfIdEnvio").val(envio);
    mostrarListado();
    //actualizaListadoRestricciones();
    //var mensaje = mostrarMensajeEnvio();
    //mostrarExito(mensaje);
}

///UTIL/////

function limpiarBarra() {
    $('#barraDesconexionesEquipos').css("display", "none");
    $('#listado').css("display", "none");
}

function seteaMatriz(evt) {
    for (var i = 0; i < evt.ListaDesconexiones.length; i++) {
        evt.ListaDesconexiones[i].Ichorini = moment(evt.ListaDesconexiones[i].Ichorini).format('DD/MM/YYYY HH:mm:ss');
        evt.ListaDesconexiones[i].Ichorfin = moment(evt.ListaDesconexiones[i].Ichorfin).format('DD/MM/YYYY HH:mm:ss');
        evt.ListaDesconexiones[i].Lastdate = moment(evt.ListaDesconexiones[i].Lastdate).format('DD/MM/YYYY HH:mm:ss');
    }
}

//convierte 2 cadenas de texto fecha(dd/mm/yyyy) y horas(hh:mm:ss) a tipo Date
function convertStringToDate(fecha, horas) {
    var partsFecha = fecha.split('/');
    if (horas == "") {
        return "";
    }
    var partsHoras = horas.split(':');
    //new Date(yyyy, mm-1, dd, hh, mm, ss);
    return new Date(partsFecha[2], partsFecha[1] - 1, partsFecha[0], partsHoras[0], partsHoras[1], partsHoras[2]);
}


function getFormattedDate(date) {
    if (date instanceof Date) {
        var year = date.getFullYear();
        var month = (1 + date.getMonth()).toString();
        month = month.length > 1 ? month : '0' + month;
        var day = date.getDate().toString();
        day = day.length > 1 ? day : '0' + day;
        var hours = date.getHours();
        var minutes = date.getMinutes();
        var ampm = hours >= 12 ? 'pm' : 'am';
        hours = hours % 12;
        hours = hours ? hours : 12; // the hour '0' should be '12'
        hours = hours < 10 ? '0' + hours : hours;
        minutes = minutes < 10 ? '0' + minutes : minutes;
        var strTime = hours + ':' + minutes + ' ' + ampm;

        return year + '/' + month + '/' + day + " " + strTime;
    }
    else {
        return "No es fecha";
    }
}

function generaDesconexionAuto() {

    var idEmpresa = $('#cbEmpresa').val();
    var sFecha = $('#txtFecha').val()
    var idenvio = $("#hfIdEnvio").val();
    var idSubcausacodi = $("#hfIdSubcausacodi").val();
    var icccodi = ((evtHot.ListaDesconexiones.length) + 1) * -1;
    var fechaactual = new Date();
    if (evtHot.ListaDesconexionesAnt.length > 0) {
        for (var i = 0; i < evtHot.ListaDesconexionesAnt.length; i++) {
            if (evtHot.ListaDesconexiones.length > 0) { // verificamos si ya existen restricciones operativas
                var existeEquipo = false;
                for (var j = 0; j < evtHot.ListaDesconexiones.length; j++) {
                    if (evtHot.ListaDesconexionesAnt[i].Equicodi == evtHot.ListaDesconexiones[j].Equicodi) { // se generará restriccion operativa automática si no existe restriccion operativa para cada equipo
                        existeEquipo = true;
                    }
                }
                if (!existeEquipo) {
                    var horaFin = moment(evtHot.ListaDesconexionesAnt[i].Ichorfin).format('HH:mm:ss');
                    if (horaFin == '00:00:00') {
                        var entity =
                        {
                            Iccodi: icccodi,
                            Equicodi: evtHot.ListaDesconexionesAnt[i].Equicodi,
                            Emprnomb: evtHot.ListaDesconexionesAnt[i].Emprnomb,
                            Famabrev: evtHot.ListaDesconexionesAnt[i].Famabrev,
                            Equinomb: evtHot.ListaDesconexionesAnt[i].Equinomb,
                            Ichorini: moment(convertStringToDate(sFecha, "00:00:00")).format('DD/MM/YYYY HH:mm:ss'),
                            Ichorfin: moment(convertStringToDate(sFecha, "00:00:00")).format('DD/MM/YYYY HH:mm:ss'),
                            Lastuser: evtHot.Lastuser,
                            Lastdate: moment(fechaactual).format('DD/MM/YYYY HH:mm:ss'),
                            Icdescrip1: "",
                            Subcausacodi: idSubcausacodi
                        };
                        evtHot.ListaDesconexiones.push(entity);
                    }
                }
            }
            else {
                var horaFin = moment(evtHot.ListaDesconexionesAnt[i].Ichorfin).format('HH:mm:ss');
                if (horaFin == '00:00:00') {
                    var entity =
                    {
                        Iccodi: icccodi,
                        Equicodi: evtHot.ListaDesconexionesAnt[i].Equicodi,
                        Emprnomb: evtHot.ListaDesconexionesAnt[i].Emprnomb,
                        Famabrev: evtHot.ListaDesconexionesAnt[i].Famabrev,
                        Equinomb: evtHot.ListaDesconexionesAnt[i].Equinomb,
                        Ichorini: moment(convertStringToDate(sFecha, "00:00:00")).format('DD/MM/YYYY HH:mm:ss'),
                        Ichorfin: moment(convertStringToDate(sFecha, "00:00:00")).format('DD/MM/YYYY HH:mm:ss'),
                        Lastuser: evtHot.Lastuser,
                        Lastdate: moment(fechaactual).format('DD/MM/YYYY HH:mm:ss'),
                        Icdescrip1: "",
                        Subcausacodi: idSubcausacodi
                    };
                    evtHot.ListaDesconexiones.push(entity);
                }
            }
        }
    }
    actualizaListadoDesconexiones();
}
