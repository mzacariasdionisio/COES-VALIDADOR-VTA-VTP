$(function () {

    listErrores = [];
    colsToHide = [];
    $('#txtFecha').Zebra_DatePicker({
        onSelect: function () {
            mostrarExcelWeb();
        }
    });

    $('#txtMes').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            mostrarExcelWeb();
        }
    });

    $('#Anho').Zebra_DatePicker({
        format: 'Y',
        onSelect: function () {
            cargarSemanaAnho()
        }
    });

    $('#cbTipoFormato').change(function () {
        horizonte();
    });

    $('#btnConsultar').click(function () {
        mostrarExcelWeb();
    });

    $('#cbEmpresa').change(function () {
        mostrarExcelWeb();
    });

    $('#btnEnviarDatos').click(function () {
        if (evtHot.Handson.ReadOnly) {
            alert("No se puede enviar información, solo diponible para  visualización");
            return
        }
        else {
            enviarExcelWeb();
        }
    });

    $('#btnDescargarFormato').click(function () {
        if (validarSeleccionDatos()) {
            descargarFormato();
        }
        else {
            alert("Por favor seleccione la empresa correcta.");
        }
    });

    $('#btnMostrarErrores').click(function () {
        mostrarDetalleErrores();
    });

    $('#btnVerEnvios').click(function () {
        popUpListaEnvios();
    });

    $('#btnMostrarJustificaciones').click(function () {
        popUpListaJustificaciones();
    });

    $('#btnExpandirRestaurar').click(function () {
        expandirRestaurar();
    });

    $('#btnManttos').click(function () {
        mostrarManttos();
    });

    $('#btnLeyenda').click(function () {
        mostrarDetalleLeyenda();
    });

    $('#btnEventos').click(function () {
        mostrarEventos();
    });

    $('#btnGrafico').click(function () {
        mostrarGrafico();
    });

    limpiarBarra();
    crearPupload();
    mostrarExcelWeb();
});

//Muestra la barra de herramemntas para administrar los datos de Despacho diario
function mostrarExcelWeb() {
    limpiarBarra();
    if ($("#txtFecha").val() != "" || $("#hfSemana").val() != "" || $("#hfMes").val()) {
        $("#hfIdEnvio").val(0);
        mostrarFormulario(consulta);
    }
    else {
        alert("Error!.Ingresar fecha correcta");
    }
}

function mostrarElementosEnvio() {
    $('#mensajeEvento').css("display", "none");
    $('#filtro_grilla').css("display", "inline-block");
    $('#barra').css("display", "table");
}
function limpiarBarra() {
    $('#barra').css("display", "none");
    $('#filtro_grilla').css("display", "none");
    $('#detalleFormato').html("");
}

function mostrarEnvioExcelWeb(envio) {
    $('#enviosanteriores').bPopup().close();
    $("#hfIdEnvio").val(envio);
    mostrarFormulario(envioAnterior);
}

function expandirRestaurar() {
    if ($('#hfExpandirContraer').val() == "E") {
        expandirExcel();
        //calculateSize2(1);
        $('#hfExpandirContraer').val("C");
        $('#spanExpandirContraer').text('Restaurar');

        var img = $('#imgExpandirContraer').attr('src');
        var newImg = img.replace('expandir.png', 'contraer.png');
        $('#imgExpandirContraer').attr('src', newImg);

    }
    else {
        restaurarExcel();
        //calculateSize2(2);
        $('#hfExpandirContraer').val("E");
        $('#spanExpandirContraer').text('Expandir');

        var img = $('#imgExpandirContraer').attr('src');
        var newImg = img.replace('contraer.png', 'expandir.png');
        $('#imgExpandirContraer').attr('src', newImg);

    }

    updateDimensionHandson(hot);
}

function expandirExcel() {
    $('#mainLayout').addClass("divexcel");
    hot.render();
}

function restaurarExcel() {

    $('#tophead').css("display", "none");
    $('#detExcel').css("display", "block");
    $('#mainLayout').removeClass("divexcel");
    $('#itemExpandir').css("display", "block");
    $('#itemRestaurar').css("display", "none");
}

function descargarFormato() {
    $.ajax({
        type: 'POST',
        url: controlador + 'generarformato',
        async: true,
        contentType: 'application/json',
        data: JSON.stringify({
            data: hot.getData(),
            idEmpresa: $('#hfEmpresa').val(),
            dia: getFecha(),
            fecha: getFecha(),
            semana: getSemana(),
            mes: getMes(),
            idFormato: getIdFormato(),
            idEnvio : $("#hfIdEnvio").val()
        }),
        beforeSend: function () {
            mostrarExito("Descargando información ...");
        },
        success: function (result) {
            if (result.length > 0 && result != '-1') {
                window.location.href = controlador + 'descargarformato?archivo=' + result;
                $("#mensajeEvento").hide();
            }
            else {
                alert("Error en descargar el archivo");
            }
        },
        error: function (result) {
            alert('ha ocurrido un error al descargar el archivo excel. ' + result.status + ' - ' + result.statusText + '.');
        }
    });
}

function validarEnvio() {
    retorno = true;
    totalErrores = listErrores.length;
    getTotalErrores();
    //valida si existen errores
    if ((totalErrores) > 0) {
        mostrarError("Existen errores en las celdas, favor corregir y vuelva a envíar");
        mostrarDetalleErrores();
        return false;
    }
    if (validaDataCongelada) {
        if (validarDataCongelada() > 0) {
            mostrarDetalleCongelados();
            return false;
        }
    }
    return retorno;
}

function enviarExcelWeb() {
    if (confirm("¿Desea enviar información a COES?")) {
        if (validarEnvio()) {
            envioData();
        }
    }
}

function envioData() {
    var empresa = $('#cbEmpresa').val();
    var fecha = $('#txtFecha').val();
    $('#hfEmpresa').val(empresa);

    $.ajax({
        type: 'POST',
        dataType: 'json',
        //async: false,
        contentType: 'application/json',
        traditional: true,
        url: controlador + "GrabarExcelWeb",
        data: JSON.stringify({
            data: hot.getData(),
            idEmpresa: empresa,
            fecha: getFecha(),
            semana: getSemana(),
            mes: getMes(),
            listaJustificacion: listaCongelados,
            idFormato: getIdFormato()
        }),

        beforeSend: function () {
            mostrarExito("Enviando Información ..");
        },
        success: function (evt) {
            if (evt.Resultado == 1) {
                $("#hfIdEnvio").val(evt.IdEnvio);
                mostrarFormulario(envioDatos);
                mostrarExito("Los datos se enviaron correctamente");
            }
            else {
                mostrarError("Error al Grabar: " + evt.Mensaje);
            }
        },
        error: function () {
            mostrarError();
        }

    });
}

function crearPupload() {
    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectExcel',
        url: siteRoot + 'ieod/DemandaDiaria/Upload',
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
                showMensaje();
                mostrarAlerta("Por favor espere ...(<strong>" + file.percent + "%</strong>)");
                hideMensajeEvento();
            },
            UploadComplete: function (up, file) {
                hideMensaje();
                var retorno = leerFileUpExcel();

                switch (retorno) {
                    case 1:
                        limpiarError();
                        $("#hfIdEnvio").val(-1);//-1 indica que el handsontable mostrara datos del archivo excel                    
                        mostrarFormulario(importarDatos);
                        break;
                    case -1:
                        mostrarError("Error: Archivo no corresponde a la empresa.");
                        break;
                    case -2:
                        mostrarError("Error: Archivo no corresponde al formato.");
                        break;
                    default:
                        mostrarError("Error");
                }
            },
            Error: function (up, err) {
                mostrarError(err.code + "-" + err.message);
            }
        }
    });
    uploader.init();
}

function mostrarFormulario(accion) {
    $("#mensaje").hide();
    listErrores = [];
    colsToHide = [];
    var idEmpresa = $("#cbEmpresa").val();
    //var fecha = $("#txtFecha").val();
    var idEnvio = $("#hfIdEnvio").val();
    var idFormato = $("#cbTipoFormato").val();
    $('#hfEmpresa').val($('#cbEmpresa').val());
    $('#hfHorizonte').val($('#cbHorizonte').val());

    fecha = getFecha();
    semana = getSemana();
    mes = getMes();

    $.ajax({
        type: 'POST',
        url: controlador + "MostrarGrilla",
        dataType: 'json',
        data: {
            idEmpresa: idEmpresa,
            fecha: fecha,
            semana: semana,
            mes: mes,
            idEnvio: idEnvio,
            idFormato: idFormato
        },
        success: function (evt) {
            if (evt != -1) {
                if (typeof hot != 'undefined') {
                    hot.destroy();
                }
                crearGrillaExcel(evt);
                evtHot = evt;
                generaFiltroGrilla(evt);
                switch (accion) {
                    case envioDatos:
                        var mensaje = mostrarMensajeEnvio(idEnvio);
                        mostrarExito("Los datos se enviaron correctamente. " + mensaje);
                        hideMensaje();
                        break;
                    case envioAnterior:
                        var mensaje = mostrarMensajeEnvio(idEnvio);
                        mostrarExito(mensaje);
                        hideMensaje();
                        break;
                    case consulta:
                        var mensaje = mostrarMensajeEnvio();
                        mostrarMensaje("Por favor complete los datos. <strong>Plazo del Envio: </strong>" + mensaje);
                        mostrarElementosEnvio();
                        break;
                    case importarDatos:
                        mostrarExito("<strong>Los datos se cargaron correctamente, por favor presione el botón enviar para grabar.</strong>");
                        break;
                }
                if (evtHot.Handson.ReadOnly) {
                    $('#btnSelectExcel').css("display", "none");
                }
                else {
                    $('#btnSelectExcel').css("display", "block");
                }
                dibujarPanelIeod(tipoFormato, idPosFormato, -1);

                updateDimensionHandson(hot);
            }
            else {
                alert("La empresa no tiene puntos de medición para cargar.");
                //document.location.href = controlador + 'Index';
            }
        },
        error: function () {
            alert("Error al cargar Excel Web");
        }
    });
}

function filtroGrupo(lista) {
    for (var i = 0; i < listaPtos.length; i++) {
        find = buscarGrupo(lista, listaPtos[i].Equicodi);
        if (find == -1) {
            grupo = {
                Centralcodi: listaPtos[i].Equipadre,
                Gruponomb: (listaPtos[i].Equipopadre != null ? listaPtos[i].Equipopadre + "_" : "") + listaPtos[i].Equinomb,
                Grupocodi: listaPtos[i].Equicodi
            }
            lista.push(grupo);
        }
    }
}

function buscarGrupo(lista, grupo) {

    for (var i = 0; i < lista.length; i++) {
        if (lista[i].Grupocodi == grupo) {
            return i;
        }
    }
    return -1;
}

function filtrarGrupoXCentral() {
    listaGrupoAux = [];
    listaGrupo = [];
    filtroGrupo(listaGrupoAux);
    central = $('#cbCentral2').multipleSelect('getSelects').toString();

    listacentral = central.split(",");
    total = listaGrupoAux.length;

    filtroGrupoHtml = "<select id='cbGrupo' multiple= 'multiple'>";
    for (var j = 0; j < listacentral.length; j++) {
        for (var i = 0; i < total; i++) {
            grupo = {
                Centralcodi: listaPtos[i].Equipadre,
                Gruponomb: listaPtos[i].Equipopadre + "_" + listaPtos[i].Equinomb,
                Grupocodi: listaPtos[i].Equicodi
            }
            if (listaGrupoAux[i].Centralcodi == listacentral[j]) {
                listaGrupo.push(grupo);
                filtroGrupoHtml += "<option value='" + listaGrupoAux[i].Grupocodi + "'> " + listaGrupoAux[i].Gruponomb + "</option>";
            }
        }
    }
    filtroGrupoHtml += "</select>";
    $('.div_grupo select').html(filtroGrupoHtml);
    $('#cbGrupo').multipleSelect('refresh');
    $('#cbGrupo').multipleSelect('checkAll');
}

function mostrarManttos() {
    var cadena = "<div style='clear:both; height:5px'></div> ";
    cadena += "<table id='tablaManttos' border='1' class='pretty tabla-adicional' cellspacing='0'>";
    cadena += "<thead><tr><th>Equipo Afectado</th><th>Equipo en Mantto</th><th>Tipo Equipo</th><th>Descripción</th><th>Fecha Inicio</th><th>Fecha Inicio</th><th>Estado</th></tr></thead>";
    cadena += "<tbody>";
    filtroGrupo(listaGrupo);
    totEquipos = listaGrupo.length;
    for (var j = 0; j < totEquipos; j++) {
        listamanto = findMantos(listaGrupo[j].Grupocodi);
        for (var k = 0; k < listamanto.length; k++) {
            indice = buscarManto(listamanto[k]);
            if (indice != -1) {
                cadena += dibujarManto(listaGrupo[j].Gruponomb, indice);
            }
        }
    }

    cadena += "</tbody></table>";
    $('#idMantenimiento').html(cadena);
    $('#mantenimientos').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown',
        modalClose: false
    });

    $('#tablaManttos').dataTable({
        "scrollY": 330,
        "scrollX": true,
        "sDom": 't',
        "ordering": false,
        "bPaginate": false,
        "iDisplayLength": -1
    });
}

function mostrarEventos() {
    var cadena = "<div style='clear:both; height:5px'></div> ";
    cadena += "<table id='tablaEventos' border='1' class='pretty tabla-adicional' cellspacing='0'>";
    cadena += "<thead><tr><th>Equipo en Evento</th><th>Tipo Equipo</th><th>Descripción</th><th>Fecha Inicio</th><th>Fecha Fin</th></tr></thead>";
    cadena += "<tbody>";

    for (var i = 0; i < eventos.length; i++) {
        tamano = (eventos[i].Evenasunto.length > 20) ? 20 : eventos[i].Evenasunto.length;
        descripcion = eventos[i].Evenasunto.substr(0, tamano);
        eveini = parseJsonDate(eventos[i].Evenini);
        fechaIni = eveini.getFullYear().toString() + "-" + ("0" + (eveini.getMonth() + 1).toString()).slice(-2) + "-" + ("0" + eveini.getDate().toString()).slice(-2);
        fechaIni += " " + ("0" + eveini.getHours().toString()).slice(-2) + ":" + ("0" + eveini.getMinutes().toString()).slice(-2) + ":" + ("0" + eveini.getSeconds().toString()).slice(-2);
        evefin = parseJsonDate(eventos[i].Evenfin);
        fechaFin = evefin.getFullYear().toString() + "-" + ("0" + (evefin.getMonth() + 1).toString()).slice(-2) + "-" + ("0" + evefin.getDate().toString()).slice(-2);
        fechaFin += " " + ("0" + evefin.getHours().toString()).slice(-2) + ":" + ("0" + evefin.getMinutes().toString()).slice(-2) + ":" + ("0" + evefin.getSeconds().toString()).slice(-2);
        cadena += "<tr><td>" + eventos[i].Equinomb + "</td><td>" + eventos[i].Famnomb + "</td>" +
            "<td><p title='" + eventos[i].Evenasunto + "'>" + descripcion + "</p></td>" + "<td>" + fechaIni + "</td><td>" + fechaFin + "</td></tr>";


    }

    cadena += "</tbody></table>";
    $('#idEvento').html(cadena);
    $('#eventos').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown',
        modalClose: false
    });

    $('#tablaEventos').dataTable({
        "scrollY": 330,
        "scrollX": true,
        "sDom": 't',
        "ordering": false,
        "bPaginate": false,
        "iDisplayLength": -1
    });
}

function findMantos(equicodi) {
    lista = [];
    for (var i = 0 ; i < listaBloqueMantos.length; i++) {

        if (listaBloqueMantos[i].Equicodi == equicodi) {

            for (var j = 0; j < listaBloqueMantos[i].ListaManto.length; j++) {
                indice = lista.indexOf(listaBloqueMantos[i].ListaManto[j]);
                if (indice == -1) {
                    lista.push(listaBloqueMantos[i].ListaManto[j]);
                }
            }
        }
    }
    return lista;
}

function buscarManto(manto) {
    for (var i = 0; i < manttos.length; i++) {
        if (manto == manttos[i].Manttocodi) {
            return i;
        }
    }
    return -1;
}

function dibujarManto(equipo, i) {
    tamano = (manttos[i].Evendescrip.length > 20) ? 20 : manttos[i].Evendescrip.length;
    descripcion = manttos[i].Evendescrip.substr(0, tamano);
    eveini = parseJsonDate(manttos[i].Evenini);
    fechaIni = eveini.getFullYear().toString() + "-" + ("0" + (eveini.getMonth() + 1).toString()).slice(-2) + "-" + ("0" + eveini.getDate().toString()).slice(-2);
    fechaIni += " " + ("0" + eveini.getHours().toString()).slice(-2) + ":" + ("0" + eveini.getMinutes().toString()).slice(-2) + ":" + ("0" + eveini.getSeconds().toString()).slice(-2);
    evefin = parseJsonDate(manttos[i].Evenfin);
    fechaFin = evefin.getFullYear().toString() + "-" + ("0" + (evefin.getMonth() + 1).toString()).slice(-2) + "-" + ("0" + evefin.getDate().toString()).slice(-2);
    fechaFin += " " + ("0" + evefin.getHours().toString()).slice(-2) + ":" + ("0" + evefin.getMinutes().toString()).slice(-2) + ":" + ("0" + evefin.getSeconds().toString()).slice(-2);
    cadena = "<tr><td>" + equipo + "</td><td>" + (manttos[i].Equinomb != null ? manttos[i].Equinomb : "") + "</td><td>" + (manttos[i].Famnomb != null ? manttos[i].Famnomb : "") + "</td>" +
        "<td><p title='" + manttos[i].Evendescrip + "'>" + descripcion + "</p></td>" + "<td>" + fechaIni + "</td><td>" + fechaFin + "</td><td style='text-align: center;'>" + manttos[i].Evenindispo + "</td></tr>";
    return cadena;
}

function parseJsonDate(jsonDateString) {
    return new Date(parseInt(jsonDateString.replace('/Date(', '')));
}

function mostrarmensajes(enPlazo, idEnvio, fechaEnvio) {
    //Mensaje descriptivo del envio
    var envio = $("#hfIdEnvio").val();
    //hideMensaje();
    hideMensajeEvento();
    hideMsgFueraPlazo();
    if (envio > 0) {
        var plazo = (enPlazo) ? "en plazo" : "fuera de plazo";
        var mensaje = "Código de envío : " + idEnvio + "   , Fecha de envío: " + fechaEnvio + "   , Enviado en " + plazo;
        mostrarMensaje(mensaje);
    }
    else {
        if (!enPlazo) {
            mostrarMsgFueraPlazo();
        }
    }
}

function mostrarMensaje(mensaje) {
    $('#mensaje').removeClass("action-exito");
    $('#mensaje').removeClass("action-alert");
    $('#mensaje').addClass("action-message");
    $('#mensaje').html(mensaje);
    $('#mensaje').css("display", "block");
}

function mostrarError(alerta) {
    $('#mensajeEvento').removeClass("action-message");
    $('#mensajeEvento').removeClass("action-alert");
    $('#mensajeEvento').removeClass("action-exito");
    $('#mensajeEvento').addClass("action-error");
    $('#mensajeEvento').html(alerta);
    $('#mensajeEvento').css("display", "block");
}

function mostrarExito(mensaje) {
    $('#mensajeEvento').removeClass("action-error");
    $('#mensajeEvento').removeClass("action-alert");
    $('#mensajeEvento').removeClass("action-message");
    $('#mensajeEvento').addClass("action-exito");
    $('#mensajeEvento').html(mensaje);
    $('#mensajeEvento').css("display", "block");
}

function hideMensaje() {

    $('#mensaje').css("display", "none");
}

function showMensaje() {

    $('#mensaje').css("display", "block");
}

function hideMensajeEvento() {

    $('#mensajeEvento').css("display", "none");
}

function mostrarMsgFueraPlazo() {
    $('#mensajePlazo').html("Formato Fuera de Plazo");
    $('#mensajePlazo').css("display", "block");
}

function hideMsgFueraPlazo() {
    $('#mensajePlazo').css("display", "none");
}

function mostrarAlerta(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-alert');
}


function horizonte() {
    var opcion = buscarPeriodo($('#cbTipoFormato').val());
    switch (parseInt(opcion)) {
        case 1: //dia
            $('.cntFecha').css("display", "table-cell");
            $('.cntSemana').css("display", "none");
            $('.cntMes').css("display", "none");
            break;
        case 2: //semanal
            $('.cntFecha').css("display", "none");
            $('.cntSemana').css("display", "table-cell");
            $('.cntMes').css("display", "none");
            break;
            //mensual

            //break;
        case 3: case 5:
            $('.cntFecha').css("display", "none");
            $('.cntSemana').css("display", "none");
            $('.cntMes').css("display", "inline");
            $('.cntMes.divmes').css("display", "block");
            break;
    }

    $.ajax({
        type: 'POST',
        url: controlador + 'SetearFechasEnvio',
        dataType: 'json',
        data: {
            idFormato: $('#cbTipoFormato').val()
        },
        success: function (result) {
            $('#txtMes').val(result.mes);
            $('#txtFecha').val(result.fecha);
            $('#hfSemana').val(result.semana);
            $('#Anho').val(result.anho);
            switch (parseInt(opcion)) {
                case 1:
                case 3:
                case 5:
                    mostrarExcelWeb();
                    break;
                case 2:
                    cargarSemanaAnho();
                    break;
            }
        },
        error: function () {
            alert("Error");
        }
    });

}

function buscarPeriodo(valor) {
    var strFormatCodi = $('#hfFormatCodi').val();
    var strFormatPeriodo = $('#hfFormatPeriodo').val();

    listFormatCodi = strFormatCodi.split(',');
    listFormatPeriodo = strFormatPeriodo.split(',');

    for (var i = 0 ; i < listFormatCodi.length; i++)
        if (listFormatCodi[i] == valor)
            return listFormatPeriodo[i];
}

function cargarSemanaAnho() {
    var anho = $('#Anho').val();
    $('#hfAnho').val(anho);
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarSemanas',

        data: { idAnho: $('#hfAnho').val() },

        success: function (aData) {
            $('#divSemana').html(aData);
            $("#cbSemana").val($("#hfSemana").val());
            mostrarExcelWeb();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}
