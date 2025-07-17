var controlador = siteRoot + 'eventos/informe/'

$(function () {
    $('#btnGrabarFile').click(function () {
        grabarFile();
    });

    $('#btnCancelarFile').click(function () {
        $('#popupFile').bPopup().close();
    });

    $('#btnExportarWord').click(function () {
        exportar('WORD');
    });

    $('#btnExportarPDF').click(function () {
        exportar('PDF');
    });

    $('#btnAceptarCopia').click(function () {
        copiar();
    });

    $('#btnCancelarCopia').click(function () {
        $('#popupCopiar').bPopup().close();
    });

    $('#btnAceptarFinalizar').click(function () {
        confirmarInforme();
    });

    $('#btnCancelarFinalizar').click(function () {
        $('#popupFinalizar').bPopup().close();
    });

    $('#btnListado').click(function () {
        document.location.href = siteRoot + 'eventos/evento/index';
    });    
});

openEvento = function () {
    $('#contenidoEvento').css('display', 'block');
    $('#contenidoInforme').css('display', 'none');
    setearEstilo("E");
}

openInforme = function (idInforme, indicador) {
    $('#contenidoEvento').css('display', 'none');
    $('#contenidoInforme').css('display', 'block');

    setearEstilo(indicador);

    $.ajax({
        type: "POST",
        url: controlador + "informe",
        data: {
            idEvento: $('#hfCodigoEvento').val(),
            idInforme: idInforme,
            indicador: indicador
        },
        success: function (evt) {
            $('#contenidoInforme').html(evt);
            cargarDocumentos(idInforme, $('#hfIndicadorEdicion').val());
        },
        error: function () {
            mostrarError();
        }
    });
}

openInterrupcion = function (idInforme){
    $('#divInterrupcion').css('display', 'block');
}

closeInterrupcion = function () {
    $('#divInterrupcion').css('display', 'none');
}

procesarInterrupcion = function ()
{
    var idInforme = $('#hfCodigoInforme').val();
    var tipoInforme = $('#hfTipoInforme').val();
    $.ajax({
        type: 'POST',
        url: controlador + 'importarinterrupcion',
        dataType: 'json',
        global: false,
        data: {
            idInforme: idInforme
        },
        success: function (result) {
            if (result == 1){
                openInforme(idInforme, tipoInforme);                
            }
            else if (result == -1) {
                mostrarError();
            }
            else {
                var html = '<div class="action-alert"><ul>';
                for (var error in result) {
                    html = html + '<li>' + result[error] + '</li>';
                }
                html = html + '</ul></div>';

                $('#fileInfoInterrupcion').html(html);
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

grabarItemReporte = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'grabarelemento',
        dataType: 'json',
        global:false,
        data: $('#formElemento').serialize(),
        success: function (result) {
            if (result.Indicador > 0) {
                pintarElemento(result.Indicador, result.Entidad);
            }
            else {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

grabarTexto = function (id, idItemInforme)
{
    var comentario = $('#' + id).val();

    $.ajax({
        type: 'POST',
        url: controlador + 'actualizartexto',
        dataType: 'json',
        data: {
            idItemInforme: idItemInforme,           
            comentario: comentario
        },
        success: function (result) {
            if (result == -1){
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

obtenerListaEquipo = function (familia){
    $('#cbEquipo').get(0).options.length = 0;
    $.ajax({
        type: 'POST',
        url: controlador + 'obtenerequipos',
        dataType: 'json',
        data: { indicador: familia },
        cache: false,
        global: false,
        success: function (aData) {
            $('#cbEquipo').get(0).options.length = 0;
            $('#cbEquipo').get(0).options[0] = new Option("-SELECCIONE-", "");
            $.each(aData, function (i, item) {
                $('#cbEquipo').get(0).options[$('#cbEquipo').get(0).options.length] = new Option(item.TAREAABREV + ' '+ item.AREANOMB  + ' ' + item.Equiabrev, item.Equicodi);
            });
        },
        error: function () {
            mostrarError();
        }
    });
}

openElemento = function (idInforme, nroItem, nroSubItem, idItemInforme) {    
    $.ajax({
        type: "POST",
        url: controlador + "elemento",
        data: {
            idInforme : idInforme, 
            itemNumber: nroItem, 
            subItemNumber : nroSubItem, 
            idItemInforme: idItemInforme
        },
        success: function (evt) {
            $('#contenidoElemento').html(evt);

            setTimeout(function () {
                $('#popupElemento').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
        },
        error: function () {
            mostrarError();
        }
    });
}

deleteElemento = function (elemento, idItemInforme) {
    if (confirm('¿Está seguro de quitar este elemento?')) {       
        $.ajax({
            type: 'POST',
            url: controlador + 'eliminarelemento',
            dataType: 'json',
            data: {idItemInforme: idItemInforme},
            success: function (result) {
                if (result == 1) {
                    $('#' + elemento).remove();                    
                }
                else {
                    mostrarError();
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
}

cargarPrevio = function () {
    limpiarFormulario();
    var itemInforme = "";
    var subItemInforme = "";
    var valor = $('#cbItemInforme').val();
    if (valor == "5-1") {
        itemInforme = "5";
        subItemInforme = "1";
    }
    else if (valor == "5-2") {
        itemInforme = "5";
        subItemInforme = "2";
    }
    else if (valor == "5-3") {
        itemInforme = "5";
        subItemInforme = "3";
    }
    else {
        itemInforme = valor;
        subItemInforme = "0";
    }

    $('#hfItemInforme').val(itemInforme);
    $('#hfSubItemInforme').val(subItemInforme);

    cargarFormulario(1);
}

cargarFormulario = function (indicador) {
    $('.tr-elemento').css('display', 'none');
    var item = $('#hfItemInforme').val();

    if (item == "5") {
        var subItem = $('#hfSubItemInforme').val();
        $('#trUnidad').css('display', 'block');
        $('#trPotenciaActiva').css('display', 'block');
        $('#trPotenciaReactiva').css('display', 'block');
        $('#trObservacion').css('display', 'block');
        $('#spanObservacion').text('Observación');

        if (subItem == "1") {
            $('#spanUnidad').text('Unidad');
            if (indicador == 1) {
                cargarEquipamiento('U');
            }
        }
        if (subItem == "2") {
            $('#trSubestacionDe').css('display', 'block');
            $('#trSubestacionhasta').css('display', 'block');
            $('#spanUnidad').text('Línea de Transmisión');
            if (indicador == 1) {
                cargarEquipamiento('L');
            }
        }
        if (subItem == "3") {
            $('#trNivelTension').css('display', 'block');
            $('#spanUnidad').text('Transformador');
            if (indicador == 1) {
                cargarEquipamiento('T');
            }
        }
    }
    if (item == "6") {
        $('#trHora').css('display', 'block');
        $('#trObservacion').css('display', 'block');
        $('#spanObservacion').text('Descripción del evento');
    }
    if (item == "7") {
        $('#trUnidad').css('display', 'block');
        $('#trSenalizacion').css('display', 'block');
        $('#trInterruptor').css('display', 'block');
        $('#trAC').css('display', 'block');
        $('#spanUnidad').text('Equipo');
        if (indicador == 1) {
            cargarEquipamiento('U');
        }
    }
    if (item == "8") {
        $('#trUnidad').css('display', 'block');
        $('#trInterruptor').css('display', 'block');
        $('#trContadorAntes').css('display', 'block');
        $('#trContadorDespues').css('display', 'block');
        $('#spanUnidad').text('Celda');
        if (indicador == 1) {
            cargarEquipamiento('C');
        }
    }

    if (item == "10") {
        $('#trSuministro').css('display', 'block');
        $('#trPotenciaMW').css('display', 'block');
        $('#trHoraInicial').css('display', 'block');
        $('#trHoraFinal').css('display', 'block');
        $('#trProteccion').css('display', 'block');
    }
    if (item == "11") {
        $('#trObservacion').css('display', 'block');
        $('#spanObservacion').text('Conclusión');
    }
    if (item == "12") {
        $('#trObservacion').css('display', 'block');
        $('#spanObservacion').text('Acciones ejecutadas');
    }
    if (item == "13") {
        $('#trObservacion').css('display', 'block');
        $('#spanObservacion').text('Observación / Recomendación');
    }
}

grabarElemento = function () {
    var mensaje = validarElemento();
    if (mensaje == "") {
        grabarItemReporte();
        $('#mensajeElemento').removeClass();
        $('#mensajeElemento').addClass("action-exito");
        $('#mensajeElemento').html("Se agregó el elemento. Puede agregar otro.");

        limpiarFormulario();
        cargarFormulario(0);
    }
    else {
        $('#mensajeElemento').removeClass();
        $('#mensajeElemento').addClass("action-alert");
        $('#mensajeElemento').html(mensaje);
    }
}

cancelarElemento = function () {
    $('#popupElemento').bPopup().close();
}

cargarEquipamiento = function (item) {
    obtenerListaEquipo(item);
}

validarElemento = function () {
    var mensaje = "";

    var item = $('#hfItemInforme').val();
    var subItem = $('#hfSubItemInforme').val();

    if (item == "5") {
        if ($('#cbEquipo').val() == "") {
            mensaje = mensaje + "<li>Selecione una unidad.</li>";
        }      

        if (subItem == "2") {
            if ($('#txtSubestacionDe').val() == "") {
                mensaje = mensaje + "<li>Ingrese subestación desde.</li>";
            }
            if ($('#txtSubestacionHasta').val() == "") {
                mensaje = mensaje + "<li>Ingrese subestación hasta.</li>"
            }
        }
        if (subItem == "3") {
            if ($('#txtNivelTension').val() == "") {
                mensaje = mensaje + "<li>Ingrese nivel de tensión.</li>";
            }
        }
    }
    if (item == "6") {
        if ($('#txtHora').val() == "") {
            mensaje = mensaje + "<li>Ingrese la hora.</li>";
        }
        else {
            if (!$('#txtHora').inputmask("isComplete")) {
                mensaje = mensaje + "<li>Ingrese una hora correcta.</li>";
            }
        }
        if ($('#txtObservacion').val() == "") {
            mensaje = mensaje + "<li>Ingrese la descripción del evento.</li>";
        }
    }
    if (item == "7") {
        if ($('#cbEquipo').val() == "") {
            mensaje = mensaje + "<li>Selecione una unidad.</li>";
        }
        if ($('#txtSenializacion').val() == "") {
            mensaje = mensaje + "<li>Ingrese señalización.</li>";
        }
        if ($('#cbInterruptor').val() == "") {
            mensaje = mensaje + "<li>Seleccione un interruptor.</li>";
        }
        if ($('#cbAC').val() == "") {
            mensaje = mensaje + "<li>Seleccione A/C.</li>";
        }
    }
    if (item == "8") {
        if ($('#cbEquipo').val() == "") {
            mensaje = mensaje + "<li>Seleccione celda.</li>";
        }
        if ($('#cbInterruptor').val() == "") {
            mensaje = mensaje + "<li>Seleccione interruptor</li>";
        }
        if ($('#txtRA').val() == "" && $('#txtSA').val() == "" && $('#txtTA')) {
            mensaje = mensaje + "<li>Ingrese contadores inicial.</li>";
        }
        if ($('#txtRD').val() == "" && $('#txtSD').val() == "" && $('#txtTD')) {
            mensaje = mensaje + "<li>Ingrese contadores después.</li>";
        }
    }
    if (item == "10") {

        if ($('#txtSuministro').val() == "") {
            mensaje = mensaje + "<li>Ingrese el suministro.</li>";
        }
        if ($('#txtPotenciaMW').val() == "") {
            mensaje = mensaje + "<li>Ingrese la potencia (MW).</li>";
        }
        if ($('#txtHoraInicial').val() == "") {
            mensaje = mensaje + "<li>Ingrese la hora inicial de la interrupción.</li>";
        }
        else if (!$('#txtHoraInicial').inputmask("isComplete")) {
            mensaje = mensaje + "<li>Ingrese una hora correcta.</li>";
        }

        if ($('#txtHoraFinal').val() == "") {
            mensaje = mensaje + "<li>Ingrese la hora final de la interrupción.</li>";
        }
        else if (!$('#txtHoraFinal').inputmask("isComplete")) {
            mensaje = mensaje + "<li>Ingrese una hora correcta.</li>";
        }
        if ($('#txtProteccion').val() == "") {
            mensaje = mensaje + "<li>Ingrese la hora final de la interrupción.</li>";
        }
    }
    if (item == "11") {
        if ($('#txtObservacion').val() == "") {
            mensaje = mensaje + "<li>Ingrese la conclusión.</li>";
        }
    }
    if (item == "12") {
        if ($('#txtObservacion').val() == "") {
            mensaje = mensaje + "<li>Ingrese la acción ejecutada.</li>";
        }
    }
    if (item == "13") {
        if ($('#txtObservacion').val() == "") {
            mensaje = mensaje + "<li>Ingrese la observación / recomendación.</li>";
        }
    }

    var resultado = "";
    if (mensaje != "") {
        resultado = resultado + "<ul>";
        resultado = resultado + mensaje;
        resultado = resultado + "</ul>";
    }

    return resultado;
}

limpiarFormulario = function () {
    $('#cbEquipo').val("");
    $('#txtSubestacionDe').val("");
    $('#txtSubestacionHasta').val("");
    $('#txtPotActiva').val("");
    $('#txtPotReactiva').val("");
    $('#txtNivelTension').val("");
    $('#txtHora').val("");
    $('#txtObservacion').val("");
    $('#txtSenializacion').val("");  
    $('#cbInterruptor').val("");
    $('#cbAC').val("");
    $('#txtRA').val("");
    $('#txtSA').val("");
    $('#txtTA').val("");
    $('#txtRD').val("");
    $('#txtSD').val("");
    $('#txtTD').val("");
    $('#txtSuministro').val("");
    $('#txtPotenciaMW').val("");
    $('#txtHoraInicial').val("");
    $('#txtHoraFinal').val("");
    $('#txtProteccion').val("");
}

pintarElemento = function (indicador, model) {
    var id = '#' + model.Eveninfcodi + '_' + model.Itemnumber + '_' + model.Subitemnumber;
    var indice = $(id + ' >tbody >tr').length + 1;
    var html = '';
    var itemInforme = model.Itemnumber;
    var subItemInforme = model.Subitemnumber;
    var idtr = model.Eveninfcodi + '_' + model.Itemnumber + '_' + model.Subitemnumber + '_' + model.Infitemcodi;

    if (model.Desobservacion == null) model.Desobservacion = " ";
    if (model.Potactiva == null) model.Potactiva = " ";
    if (model.Potreactiva == null) model.Potreactiva = " ";

    if (itemInforme == 5) {
        if (subItemInforme == 1) {
            if (indicador == 1) {
                html =
                '<tr id = "' + idtr + '">' +
                '    <td>' + indice + '</td>' +
                '    <td>' + model.Areanomb + '</td>' +
                '    <td>' + model.Equinomb + '</td>' +
                '    <td>' + model.Potactiva + '</td>' +
                '    <td>' + model.Potreactiva + '</td>' +
                '    <td>' + model.Desobservacion + '</td>' +
                '    <td>' +
                '        <a href="JavaScript:openElemento(' + model.Eveninfcodi + ' , 5, 1, ' + model.Infitemcodi + ');" class="action-informe"><img src="' + siteRoot + 'Content/Images/Pen.png" title="Editar" /></a>' +
                '        <a href="JavaScript:deleteElemento(\'' + idtr + '\', ' + model.Infitemcodi + ');" class="action-informe"><img src="' + siteRoot + 'Content/Images/Trash.png" title="Eliminar" /></a>' +
                '    </td>' +
                '</tr>';
            }
            else {
                $('#' + idtr).find('td').eq(1).text(model.Areanomb);
                $('#' + idtr).find('td').eq(2).text(model.Equinomb);
                $('#' + idtr).find('td').eq(3).text(model.Potactiva);
                $('#' + idtr).find('td').eq(4).text(model.Potreactiva);
                $('#' + idtr).find('td').eq(5).text(model.Desobservacion);
            }
        }
        if (subItemInforme == 2) {
            if (indicador == 1) {
                html =
                '<tr id = "' + idtr + '">' +
                '    <td>' + indice + '</td>' +
                '    <td>' + model.Equinomb + ' - ' + model.Equicodi + '</td>' +
                '    <td>' + model.Subestacionde + '</td>' +
                '    <td>' + model.Subestacionhasta + '</td>' +
                '    <td>' + model.Potactiva + '</td>' +
                '    <td>' + model.Potreactiva + '</td>' +
                '    <td>' + model.Desobservacion + '</td>' +
                '    <td>' +
                '        <a href="JavaScript:openElemento(' + model.Eveninfcodi + ' , 5, 2, ' + model.Infitemcodi + ');" class="action-informe"><img src="' + siteRoot + 'Content/Images/Pen.png" title="Editar" /></a>' +
                '        <a href="JavaScript:deleteElemento(\'' + idtr + '\', ' + model.Infitemcodi + ');" class="action-informe"><img src="' + siteRoot + 'Content/Images/Trash.png" title="Eliminar" /></a>' +
                '    </td>' +
                '</tr>';
            }
            else {
                $('#' + idtr).find('td').eq(1).text(model.Equinomb + ' - ' + model.Equicodi);
                $('#' + idtr).find('td').eq(2).text(model.Subestacionde);
                $('#' + idtr).find('td').eq(3).text(model.Subestacionhasta);
                $('#' + idtr).find('td').eq(4).text(model.Potactiva);
                $('#' + idtr).find('td').eq(5).text(model.Potreactiva);
                $('#' + idtr).find('td').eq(6).text(model.Desobservacion);
            }
        }
        if (subItemInforme == 3) {
            if (indicador == 1) {
                html =
                '<tr id = "' + idtr + '">' +
                '    <td>' + indice + '</td>' +
                '    <td>' + model.Areanomb + '</td>' +
                '    <td>' + model.Equinomb + ' ' + model.Equicodi + '</td>' +
                '    <td>' + model.Potactiva + '</td>' +
                '    <td>' + model.Potreactiva + '</td>' +
                '    <td>' + model.Niveltension + '</td>' +
                '    <td>' + model.Desobservacion + '</td>' +
                '    <td>' +
                '        <a href="JavaScript:openElemento(' + model.Eveninfcodi + ' , 5, 3, ' + model.Infitemcodi + ');" class="action-informe"><img src="' + siteRoot + 'Content/Images/Pen.png" title="Editar" /></a>' +
                '        <a href="JavaScript:deleteElemento(\'' + idtr + '\', ' + model.Infitemcodi + ');" class="action-informe"><img src="' + siteRoot + 'Content/Images/Trash.png" title="Eliminar" /></a>' +
                '    </td>' +
                '</tr>';
            }
            else {
                $('#' + idtr).find('td').eq(1).text(model.Areanomb);
                $('#' + idtr).find('td').eq(2).text(model.Equinomb + ' - ' + model.Equicodi);
                $('#' + idtr).find('td').eq(3).text(model.Potactiva);
                $('#' + idtr).find('td').eq(4).text(model.Potreactiva);
                $('#' + idtr).find('td').eq(5).text(model.Niveltension);
                $('#' + idtr).find('td').eq(6).text(model.Desobservacion);
            }
        }
    }
    if (itemInforme == 6) {
        if (indicador == 1) {
            html =
            '<tr id = "' + idtr + '">' +
            '    <td>' + model.Itemhora + '</td>' +
            '    <td>' + model.Desobservacion + '</td>' +
            '    <td>' +
            '        <a href="JavaScript:openElemento(' + model.Eveninfcodi + ' , 6, 0, ' + model.Infitemcodi + ');" class="action-informe"><img src="' + siteRoot + 'Content/Images/Pen.png" title="Editar" /></a>' +
            '        <a href="JavaScript:deleteElemento(\'' + idtr + '\', ' + model.Infitemcodi + ');" class="action-informe"><img src="' + siteRoot + 'Content/Images/Trash.png" title="Eliminar" /></a>' +
            '    </td>' +
            '</tr>';
        }
        else {
            $('#' + idtr).find('td').eq(0).text(model.Itemhora);
            $('#' + idtr).find('td').eq(1).text(model.Desobservacion);
        }
    }
    if (itemInforme == 7) {
        if (indicador == 1) {
            html =
            '<tr id = "' + idtr + '">' +
            '     <td>' + model.Areanomb + '</td>' +
            '     <td>' + model.Equinomb + '</td>' +
            '     <td>' + model.Senializacion + '</td>' +
            '     <td>' + model.Internomb + '</td>' +
            '     <td>' + model.Ac + '</td>' +
            '    <td>' +
            '        <a href="JavaScript:openElemento(' + model.Eveninfcodi + ' , 7, 0, ' + model.Infitemcodi + ');" class="action-informe"><img src="' + siteRoot + 'Content/Images/Pen.png" title="Editar" /></a>' +
            '        <a href="JavaScript:deleteElemento(\'' + idtr + '\', ' + model.Infitemcodi + ');" class="action-informe"><img src="' + siteRoot + 'Content/Images/Trash.png" title="Eliminar" /></a>' +
            '    </td>' +
            '</tr>';
        }
        else {
            $('#' + idtr).find('td').eq(0).text(model.Areanomb);
            $('#' + idtr).find('td').eq(1).text(model.Equinomb);
            $('#' + idtr).find('td').eq(2).text(model.Senializacion);
            $('#' + idtr).find('td').eq(3).text(model.Internomb);
            $('#' + idtr).find('td').eq(4).text(model.Ac);
        }
    }
    if (itemInforme == 8) {
        if (indicador == 1) {
            html =
            '<tr id = "' + idtr + '">' +
            '    <td>' + model.Areanomb + '</td>' +
            '    <td>' + model.Equinomb + '</td>' +
            '    <td>' + model.Internomb + '</td>' +
            '    <td>' + model.Ra + '</td>' +
            '    <td>' + model.Sa + '</td>' +
            '    <td>' + model.Ta + '</td>' +
            '    <td>' + model.Rd + '</td>' +
            '    <td>' + model.Sd + '</td>' +
            '    <td>' + model.Td + '</td>' +
            '    <td>' +
            '        <a href="JavaScript:openElemento(' + model.Eveninfcodi + ' , 8, 0, ' + model.Infitemcodi + ');" class="action-informe"><img src="' + siteRoot + 'Content/Images/Pen.png" title="Editar" /></a>' +
            '        <a href="JavaScript:deleteElemento(\'' + idtr + '\', ' + model.Infitemcodi + ');" class="action-informe"><img src="' + siteRoot + 'Content/Images/Trash.png" title="Eliminar" /></a>' +
            '    </td>' +
            '</tr>';
        }
        else {
            $('#' + idtr).find('td').eq(0).text(model.Areanomb);
            $('#' + idtr).find('td').eq(1).text(model.Equinomb);
            $('#' + idtr).find('td').eq(2).text(model.Internomb);
            $('#' + idtr).find('td').eq(3).text(model.Ra);
            $('#' + idtr).find('td').eq(4).text(model.Sa);
            $('#' + idtr).find('td').eq(5).text(model.Ta);
            $('#' + idtr).find('td').eq(6).text(model.Rd);
            $('#' + idtr).find('td').eq(7).text(model.Sd);
            $('#' + idtr).find('td').eq(8).text(model.Td);
        }
    }
    if (itemInforme == 10) {
        if (indicador == 1) {
            html =
            '<tr id = "' + idtr + '">' +
            '    <td>' + indice + '</td>' +
            '    <td>' + model.Sumininistro + '</td>' +
            '    <td>' + model.Potenciamw + '</td>' +
            '    <td>' + model.DesIntInicio + '</td>' +
            '    <td>' + model.DesIntFin + '</td>' +
            '    <td>' + model.Duracion + '</td>' +
            '    <td>' + model.Proteccion + '</td>' +
            '    <td>' +
            '        <a href="JavaScript:openElemento(' + model.Eveninfcodi + ' , 10, 0, ' + model.Infitemcodi + ');" class="action-informe"><img src="' + siteRoot + 'Content/Images/Pen.png" title="Editar" /></a>' +
            '        <a href="JavaScript:deleteElemento(\'' + idtr + '\', ' + model.Infitemcodi + ');" class="action-informe"><img src="' + siteRoot + 'Content/Images/Trash.png" title="Eliminar" /></a>' +
            '    </td>' +
            '</tr>';
        }
        else {
            $('#' + idtr).find('td').eq(1).text(model.Sumininistro);
            $('#' + idtr).find('td').eq(2).text(model.Potenciamw);
            $('#' + idtr).find('td').eq(3).text(model.DesIntInicio);
            $('#' + idtr).find('td').eq(4).text(model.DesIntFin);
            $('#' + idtr).find('td').eq(5).text(model.Duracion);
            $('#' + idtr).find('td').eq(6).text(model.Proteccion);

        }
    }
    if (itemInforme == 11 || itemInforme == 12 || itemInforme == 13) {
        if (indicador == 1) {
            html =
            '<tr id = "' + idtr + '">' +
            '    <td>' + indice + '</td>' +
            '    <td>' + model.Desobservacion + '</td>' +
            '    <td>' +
            '        <a href="JavaScript:openElemento(' + model.Eveninfcodi + ' , ' + itemInforme + ' , 0, ' + model.Infitemcodi + ');" class="action-informe"><img src="' + siteRoot + 'Content/Images/Pen.png" title="Editar" /></a>' +
            '        <a href="JavaScript:deleteElemento(\'' + idtr + '\', ' + model.Infitemcodi + ');" class="action-informe"><img src="' + siteRoot + 'Content/Images/Trash.png" title="Eliminar" /></a>' +
            '    </td>' +
            '</tr>';
        }
        else {
            $('#' + idtr).find('td').eq(1).text(model.Desobservacion);
        }
    }
    if (indicador == 1) {
        $(id + '> tbody').append(html);
    }
}

cargarDocumentos = function (idInforme, indicador) {
    $.ajax({
        type: "POST",
        url: controlador + "anexo",
        data: {
            idInforme: idInforme,
            indicador: indicador
        },
        success: function (evt) {
            $('#contenedorFile').html(evt);            
        },
        error: function () {
            mostrarError();
        }
    });
}

deleteFile = function (idFile) {
    if (confirm('¿Está seguro de quitar este elemento?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'eliminarfile',
            dataType: 'json',
            data: { idFile: idFile},
            success: function (result) {
                if (result == 1) {
                    cargarDocumentos($('#hfCodigoInforme').val(), $('#hfIndicadorEdicion').val());
                }
                else {
                    mostrarError();
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
}

editarFile = function (idFile, descripcion) {
    $('#txtDescripcionFile').val(descripcion);
    $('#hfIdFileReporte').val(idFile);
       
    $('#popupFile').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    }); 
}

grabarFile = function () {
    $.ajax({
        type: 'POST',
        global: false,        
        url: controlador + 'grabarfile',
        dataType: 'json',
        data: {
            idFile: $('#hfIdFileReporte').val(),
            descripcion: $('#txtDescripcionFile').val()
        },
        success: function (result) {
            if (result == 1) {
                cargarDocumentos($('#hfCodigoInforme').val(), $('#hfIndicadorEdicion').val());
                $('#popupFile').bPopup().close();
            }
            else {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

openExportar = function () {
    $('#popupExportar').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
}

exportar = function (tipo) 
{
    var idEvento = $('#hfCodigoEvento').val();
    var idInforme = $('#hfCodigoInforme').val();
    var idEmpresa = $('#hfCodigoEmpresa').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'exportar',
        global: false,
        dataType: 'json',
        data: {
            idEvento: idEvento,
            idInforme: idInforme,
            idEmpresa: idEmpresa,
            tipo: tipo
        },
        cache: false,
        success: function (resultado) {
            if (resultado == 1) {
                window.location = controlador + "descargar?tipo=" + tipo;
            }
            else {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

openCopia = function () {
    var tipo = $('#hfTipoInforme').val();
    $('#cbOrigenFinal').css('display', 'none');
    $('#cbOrigenComplementario').css('display', 'none');

    if (tipo == "F") {
        $('#cbOrigenFinal').css('display', 'block');
    }
    if (tipo == "C"){
        $('#cbOrigenComplementario').css('display', 'block');
    }

    $('#popupCopiar').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
}

copiar = function () {
    var tipo = $('#hfTipoInforme').val();
    var idDestino = $('#hfCodigoInforme').val();
    var idOrigen = "";
    var indicador = $("input:radio[name='OpcionCopia']:checked").val();
    
    if (tipo == "F") {
        idOrigen = $('#CodigoInformePreliminar').val();
    }
    if (tipo == "C") {
        if ($('#cbOrigenComplementario').val() == "P") {
            idOrigen = $('#CodigoInformePreliminar').val();
        }
        if ($('#cbOrigenComplementario').val() == "F") {
            idOrigen = $('#CodigoInformeFinal').val();
        }
    }

    if (idOrigen != "" && idDestino != "") {
        if (confirm('¿Está seguro de realizar esta acción?')) {
            $.ajax({
                type: 'POST',
                url: controlador + 'copiar',
                global: false,
                dataType: 'json',
                data: {
                    idOrigen: idOrigen,
                    idDestino: idDestino,
                    indicador: indicador
                },
                cache: false,
                success: function (resultado) {
                    if (resultado == 1) {
                        openInforme(idDestino, tipo);
                        $('#popupCopia').bPopup().close();
                    }                    
                    else {
                        mostrarError();
                    }
                },
                error: function () {
                    mostrarError();
                }
            });
        }
    }
}

finalizar = function () {
    var tipo = $('#hfTipoInforme').val();
    var idEvento = $('#hfCodigoEvento').val();
    
    $.ajax({
        type: 'POST',
        url: controlador + 'validarfinalizar',
        global: false,
        dataType: 'json',
        data: {
            idEvento: idEvento,
            indicador: tipo
        },
        cache: false,
        success: function (resultado) {
            if (resultado == 1) {
                $('#divMensajeFinalizar').css('display', 'none');
            }
            if (resultado == 2) {
                $('#divMensajeFinalizar').css('display', 'block');
            }

            setTimeout(function () {
                $('#popupFinalizar').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);            
        },
        error: function () {
            mostrarError();
        }
    });
}

confirmarInforme = function () {
    var tipo = $('#hfTipoInforme').val();
    var idInforme = $('#hfCodigoInforme').val();
    $.ajax({
        type: 'POST',
        url: controlador + 'finalizar',
        global: false,
        dataType: 'json',
        data: {
            idEvento: $('#hfCodigoEvento').val(),
            tipo: tipo,
            idInforme: idInforme
        },
        cache: false,
        success: function (resultado) {
            if (resultado == 1) {
                openInforme(idInforme, tipo);
                $('#popupFinalizar').bPopup().close();
            }
            else if (resultado == 2) {
                mostrarAlerta("Debe agregar datos o adjuntos al informe para poder finalizar");
            }
            else {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

setearEstilo = function (indicador) {
    $('#enlaceInformeP').removeClass();
    $('#enlaceInformeF').removeClass();
    $('#enlaceInformeC').removeClass();
    $('#enlaceInformeA').removeClass();
    $('#enlaceInformeI').removeClass();
    $('#enlaceEvento').removeClass();

    if (indicador == 'P') {
        $('#enlaceEvento').addClass('opcion-informe');
        $('#enlaceInformeF').addClass('opcion-informe');
        $('#enlaceInformeC').addClass('opcion-informe');
        $('#enlaceInformeA').addClass('opcion-informe');
        $('#enlaceInformeI').addClass('opcion-informe');
        $('#enlaceInformeP').addClass('opcion-informe-active');
    }
    if (indicador == 'F') {
        $('#enlaceEvento').addClass('opcion-informe');
        $('#enlaceInformeP').addClass('opcion-informe');
        $('#enlaceInformeC').addClass('opcion-informe');
        $('#enlaceInformeA').addClass('opcion-informe');
        $('#enlaceInformeI').addClass('opcion-informe');
        $('#enlaceInformeF').addClass('opcion-informe-active');
    }
    if (indicador == 'C') {
        $('#enlaceEvento').addClass('opcion-informe');
        $('#enlaceInformeP').addClass('opcion-informe');
        $('#enlaceInformeF').addClass('opcion-informe');
        $('#enlaceInformeA').addClass('opcion-informe');
        $('#enlaceInformeI').addClass('opcion-informe');
        $('#enlaceInformeC').addClass('opcion-informe-active');
    }
    if (indicador == 'A') {
        $('#enlaceEvento').addClass('opcion-informe');
        $('#enlaceInformeP').addClass('opcion-informe');
        $('#enlaceInformeF').addClass('opcion-informe');
        $('#enlaceInformeC').addClass('opcion-informe');
        $('#enlaceInformeI').addClass('opcion-informe');
        $('#enlaceInformeA').addClass('opcion-informe-active');
    }
    if (indicador == 'E') {
        $('#enlaceEvento').addClass('opcion-informe-active');
        $('#enlaceInformeP').addClass('opcion-informe');
        $('#enlaceInformeF').addClass('opcion-informe');
        $('#enlaceInformeC').addClass('opcion-informe');
        $('#enlaceInformeA').addClass('opcion-informe');
        $('#enlaceInformeI').addClass('opcion-informe');
    }
    if (indicador == "I") {
        $('#enlaceEvento').addClass('opcion-informe');
        $('#enlaceInformeP').addClass('opcion-informe');
        $('#enlaceInformeF').addClass('opcion-informe');
        $('#enlaceInformeC').addClass('opcion-informe');
        $('#enlaceInformeI').addClass('opcion-informe-active');
        $('#enlaceInformeA').addClass('opcion-informe');
    }
}

cargarLogo = function (idEmpresa, extension)
{
    var d = new Date();    
    var file = 'http://www.coes.org.pe/appfileserver/documentos/Logos/LOGOEMPRESA_' + idEmpresa + "." + extension + "?time=" + d.getSeconds();
    $("#imgLogoEmpresa").attr("src", file);
}

mostrarError = function () {
    alert("Ha ocurrido un error.");
}

mostrarAlerta = function (mensaje) {
    alert(mensaje);
}

validarNumero = function (item, evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;

    if (charCode == 46) {
        var regex = new RegExp(/\./g)
        var count = $(item).val().match(regex).length;
        if (count > 1) {
            return false;
        }
    }

    if (charCode == 45) {
        var regex = new RegExp(/\-/g)
        var count = $(item).val().match(regex).length;
        if (count > 0) {
            return false;
        }
    }

    if (charCode > 31 && charCode != 45 && (charCode < 48 || charCode > 57)) {
        return false;
    }

    return true;
}

validarEntero = function (item, evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;

    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }

    return true;
}