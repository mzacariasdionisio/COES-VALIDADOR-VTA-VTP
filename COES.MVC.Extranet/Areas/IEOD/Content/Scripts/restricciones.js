var controlador = siteRoot + 'IEOD/RestriccionesOperativas/';
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
    idFuenteDatos = idFuenteDatosRestriccionesOperativas;
    idFormato = 0;
    tipoFormato = 0;
    dibujarPanelIeod(0, 9, -1);
    $('#cbEmpresa').change(function () {
        dibujarPanelIeod(tipoFormato, 9, -1);
        limpiarBarra();
        hideMensaje();
        hideMensajeEvento();
    });


    $('#btnEnviarDatos').click(function () {
        if (evtHot.EnabledRestriccion) {
            grabarEnvio();
        }
        else {
            alert("No se puede enviar información, solo diponible para  visualización");
        }
    });

    $('#btnConsultar').click(function () {
        generaListado();
    });

    $('#btnVerEnvios').click(function () {
        popUpListaEnvios();
    });

    $('#btnAgregarRestriccion').click(function () {
        ichorini = $('#txtFecha').val();
        popUpAgregarRestriccion(0, -1, ichorini, "", "", "");
    });

    $('#btnCopiarRestriccion').click(function () {
        hideMensajeEvento();
        generaRestriccionesAuto();
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
        $('#barraRestriccionesOperativas').css("display", "table");
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
                $('#listado').html(dibujarTablaRestricciones(evtHot));
                $('#listado').css("display", "block");
            }
            else {
                alert("Error:" + evt.descripcion);
            }

            dibujarPanelIeod(tipoFormato, 9, -1);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function esconderOpcionBarra() {
    $('#btnEnviarDatos').css("display", "none");
    $('#btnAgregarRestriccion').css("display", "none");
}

function mostrarOpcionBarra() {
    $('#btnEnviarDatos').css("display", "block");
    $('#btnAgregarRestriccion').css("display", "block");
}

function dibujarTablaRestricciones(evt) {
    var tipoCentral = evt.IdTipoCentral;
    var strHtml = "<table  class='pretty tabla-icono' cellspacing='0' width='100%' id='tabla'>";
    strHtml += "<thead>";
    strHtml += "<tr><th>Empresa</th><th>Tipo Equipo</th><th>Equipo</th><th>Inicio</th><th>Fin</th><th>Usuario</th><th>Modificacion</th><th></th><th></th><th></tr>";
    strHtml += "</thead>";
    strHtml += "<tbody>";
    strHtml += generaViewListaRestricciones(evt);
    strHtml += "</tbody></table>";
    return strHtml;
}

function generaViewListaRestricciones(evt) {
    var cadenaHtml = "";
    estiloTabla = "style='text-align: left;padding-left:10px;";
    estiloTabla2 = "";
    if (!evt.EnabledRestriccion) {
        estiloTabla += ";background:#999999 !important;";
        estiloTabla2 = "style='background:#999999 !important;'";
    }
    estiloTabla += "'";
    for (var i = 0; i < evt.ListaRestricciones.length; i++) {
        if (evt.ListaRestricciones[i].opCrud != -1) { // no mostramos los elementos que han sido eliminados que vienen de BD y siguen en memoria
            var inicio = evt.ListaRestricciones[i].Ichorini;
            var fin = evt.ListaRestricciones[i].Ichorfin;
            var lasdate = evt.ListaRestricciones[i].Lastdate;
            cadenaHtml += "<tr>";
            cadenaHtml += "<td " + estiloTabla + ">" + evt.ListaRestricciones[i].Emprnomb + "</td>";
            cadenaHtml += "<td " + estiloTabla + ">" + evt.ListaRestricciones[i].Famabrev + "</td>";
            cadenaHtml += "<td " + estiloTabla + ">" + evt.ListaRestricciones[i].Equinomb + "</td>";
            cadenaHtml += "<td " + estiloTabla + ">" + inicio + "</td>";
            cadenaHtml += "<td " + estiloTabla + ">" + fin + "</td>";
            cadenaHtml += "<td " + estiloTabla + ">" + evt.ListaRestricciones[i].Lastuser + "</td>";
            cadenaHtml += "<td " + estiloTabla + ">" + lasdate + "</td>";

            if (evt.EnabledRestriccion) {
                cadenaHtml += "<td style='cursor:pointer;width:30px;'>";
                cadenaHtml += "<img onclick='verAdjunto(" + evt.ListaRestricciones[i].Iccodi + ");' src='" + siteRoot + "Content/Images/file.png' title='Ver Archivo' alt='Ver Archivo'/></td>";
                cadenaHtml += "<td  style='cursor:pointer;width:30px;'>";
                cadenaHtml += "<img onclick='editarRestriccion(" + evt.ListaRestricciones[i].Iccodi + ");' src='" + siteRoot + "Content/Images/btn-edit.png' title='Editar Restricción' alt='Editar Restriccion'/></td>";
                cadenaHtml += "<td  style='cursor:pointer;width:30px;'>";
                cadenaHtml += "<img onclick='eliminarRestriccion(" + evt.ListaRestricciones[i].Iccodi + ");'src='" + siteRoot + "Content/Images/Trash.png' title='Eliminar Resticción' alt='Eliminar Restricción'/></td>";
            }
            else {
                cadenaHtml += "<td " + estiloTabla2 + ">";
                cadenaHtml += "<img onclick='verAdjunto(" + evt.ListaRestricciones[i].Iccodi + ");' src='" + siteRoot + "Content/Images/file.png' title='Ver Archivo' alt='Ver Archivo'/></td>";
                cadenaHtml += "<td  " + estiloTabla2 + ">";
                cadenaHtml += "<img  src='" + siteRoot + "Content/Images/btn-edit.png' title='Editar Restricción' alt='Editar Restriccion'/></td>";
                cadenaHtml += "<td  " + estiloTabla2 + ">";
                cadenaHtml += "<img src='" + siteRoot + "Content/Images/Trash.png' title='Eliminar Resticción' alt='Eliminar Restricción'/></td>";
            }

            cadenaHtml += "</tr>";
        }
    }

    return cadenaHtml;
}

function actualizaListadoRestricciones() {
    $('#listado').css("display", "block");
    $('#listado').css("width", $('#mainLayout').width() + "px");
    $('#listado').html(dibujarTablaRestricciones(evtHot));
    //$('#tabla').dataTable({
    //    "scrollY": 430,
    //    "scrollX": true,
    //    "ordering": false,
    //    "sDom": 't',
    //    "iDisplayLength": 50
    //});
}

function popUpAgregarRestriccion(iccodi, equicodi, icfechaini, icfechafin, icdescrip1, Icnombarchfisico) {
    var idEmpresa = $('#cbEmpresa').val();
    icfini = icfechaini.split(' ')[0];
    ichorini = icfechaini.split(' ')[1];
    ichorfin = icfechafin.split(' ')[1];
    icffin = icfechafin.split(' ')[0];
    $.ajax({
        type: 'POST',
        url: controlador + 'ViewRestriccionOperativa',
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
            archEnvio: Icnombarchfisico
        },
        success: function (result) {
            $('#idRestricciones').html(result);
            setTimeout(function () {
                $('#newRestricciones').bPopup({
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

function editarRestriccion(iccodi) {
    var equicodi = -1;
    var ichorini = "";
    var ichorfin = "";
    var icdescrip1 = "";
    var icnombarchenvio = "";
    for (var i = 0; i < evtHot.ListaRestricciones.length; i++) {
        if (evtHot.ListaRestricciones[i].Iccodi == iccodi) {// encontrado                             
            equicodi = evtHot.ListaRestricciones[i].Equicodi;
            ichorini = evtHot.ListaRestricciones[i].Ichorini;
            ichorfin = evtHot.ListaRestricciones[i].Ichorfin;
            icdescrip1 = evtHot.ListaRestricciones[i].Icdescrip1;
            icnombarchenvio = evtHot.ListaRestricciones[i].Icnombarchenvio;
            evtHot.ListaRestricciones[i].IcnombarchfisicoAnt = evtHot.ListaRestricciones[i].Icnombarchfisico;
        }
    }

    popUpAgregarRestriccion(iccodi, equicodi, ichorini, ichorfin, icdescrip1, icnombarchenvio);
}

function eliminarRestriccion(iccodi) {
    if (confirm("¿Desea eliminar Restriccion...?")) {
        for (var i = 0; i < evtHot.ListaRestricciones.length; i++) {
            if (evtHot.ListaRestricciones[i].Iccodi == iccodi) {// encontrado                             
                if (iccodi > 0) { // registro viene de BD
                    evtHot.ListaRestricciones[i].opCrud = -1 // eliminado logico
                }
                else {
                    evtHot.ListaRestricciones.splice(i, 1); //eliminado fisico de la matriz auxiliar
                }
            }
        }
        actualizaListadoRestricciones(evtHot);
    }

}

function verAdjunto(id) {

    for (var i = 0; i < evtHot.ListaRestricciones.length; i++) {
        if (evtHot.ListaRestricciones[i].Iccodi == id) {
            Icnombarchfisico = evtHot.ListaRestricciones[i].Icnombarchfisico;
            cambioArch = evtHot.ListaRestricciones[i].CambioArchivo;
        }
    }
    if (cambioArch == null || cambioArch == undefined) { cambioArch = 0; }
    if (Icnombarchfisico == "" || Icnombarchfisico == null) {
        alert("No se encontro el archivo adjunto o no existe!");
    }
    else {
        window.location = controlador + 'DescargarArchivoRestriccion?archivo=' + Icnombarchfisico + '&icccodi=' + id + '&cambio=' + cambioArch;
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
        var matriz = obtenerMatrizEnviar(evtHot.ListaRestricciones);
        $.ajax({
            type: 'POST',
            //async: false,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            traditional: true,
            url: controlador + "RegistrarEnvioRestriciones",
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
            error: function (err) {
                mostrarError();
            }

        });
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
        cadena += "<tr onclick='mostrarEnvioRestricciones(" + lista[key].Enviocodi + ");' style='cursor:pointer'><td>" + lista[key].Enviocodi + "</td>";
        cadena += "<td>" + getFormattedDate(javaScriptDate) + "</td>";
        cadena += "<td>" + lista[key].Lastuser + "</td></tr>";
    }
    cadena += "</tbody></table>";
    return cadena;

}

function mostrarEnvioRestricciones(envio) {
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
    $('#barraRestriccionesOperativas').css("display", "none");
    $('#listado').css("display", "none");
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

function seteaMatriz(evt) {
    for (var i = 0; i < evt.ListaRestricciones.length; i++) {
        evt.ListaRestricciones[i].Ichorini = moment(evt.ListaRestricciones[i].Ichorini).format('DD/MM/YYYY HH:mm:ss');
        evt.ListaRestricciones[i].Ichorfin = moment(evt.ListaRestricciones[i].Ichorfin).format('DD/MM/YYYY HH:mm:ss');
        evt.ListaRestricciones[i].Lastdate = moment(evt.ListaRestricciones[i].Lastdate).format('DD/MM/YYYY HH:mm:ss');
    }
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

function generaRestriccionesAuto() {
    var idEmpresa = $('#cbEmpresa').val();
    var sFecha = $('#txtFecha').val()
    var idenvio = $("#hfIdEnvio").val();
    var idSubcausacodi = $("#hfIdSubcausacodi").val();
    var icccodi = ((evtHot.ListaRestricciones.length) + 1) * -1;
    var fechaactual = new Date();
    if (evtHot.ListaRestriccionesAnt.length > 0) {
        for (var i = 0; i < evtHot.ListaRestriccionesAnt.length; i++) {
            if (evtHot.ListaRestricciones.length > 0) { // verificamos si ya existen restricciones operativas
                var existeEquipo = false;
                for (var j = 0; j < evtHot.ListaRestricciones.length; j++) {
                    if (evtHot.ListaRestriccionesAnt[i].Equicodi == evtHot.ListaRestricciones[j].Equicodi) { // se generará restriccion operativa automática si no existe restriccion operativa para cada equipo
                        existeEquipo = true;
                    }
                }
                if (!existeEquipo) {
                    var horaFin = moment(evtHot.ListaRestriccionesAnt[i].Ichorfin).format('HH:mm');
                    if (horaFin == '23:59') {
                        var entity =
                        {
                            Iccodi: icccodi,
                            Equicodi: evtHot.ListaRestriccionesAnt[i].Equicodi,
                            Emprnomb: evtHot.ListaRestriccionesAnt[i].Emprnomb,
                            Famabrev: evtHot.ListaRestriccionesAnt[i].Famabrev,
                            Equinomb: evtHot.ListaRestriccionesAnt[i].Equinomb,
                            Ichorini: moment(convertStringToDate(sFecha, "00:00:00")).format('DD/MM/YYYY HH:mm:ss'),
                            Ichorfin: moment(convertStringToDate(sFecha, "23:59:59")).format('DD/MM/YYYY HH:mm:ss'),
                            Lastuser: evtHot.Lastuser,
                            Lastdate: moment(fechaactual).format('DD/MM/YYYY HH:mm:ss'),
                            Icdescrip1: "",
                            Subcausacodi: idSubcausacodi
                        };
                        evtHot.ListaRestricciones.push(entity);
                    }
                }
            }
            else {
                var horaFin = moment(evtHot.ListaRestriccionesAnt[i].Ichorfin).format('HH:mm');
                if (horaFin == '23:59') {
                    var entity =
                    {
                        Iccodi: icccodi,
                        Equicodi: evtHot.ListaRestriccionesAnt[i].Equicodi,
                        Emprnomb: evtHot.ListaRestriccionesAnt[i].Emprnomb,
                        Famabrev: evtHot.ListaRestriccionesAnt[i].Famabrev,
                        Equinomb: evtHot.ListaRestriccionesAnt[i].Equinomb,
                        Ichorini: moment(convertStringToDate(sFecha, "00:00:00")).format('DD/MM/YYYY HH:mm:ss'),
                        Ichorfin: moment(convertStringToDate(sFecha, "23:59:59")).format('DD/MM/YYYY HH:mm:ss'),
                        Lastuser: evtHot.Lastuser,
                        Lastdate: moment(fechaactual).format('DD/MM/YYYY HH:mm:ss'),
                        Icdescrip1: "",
                        Subcausacodi: idSubcausacodi
                    };
                    evtHot.ListaRestricciones.push(entity);
                }
            }
        }
    }
    actualizaListadoRestricciones();
}

