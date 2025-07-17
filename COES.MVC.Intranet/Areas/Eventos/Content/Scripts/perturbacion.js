var controlador = siteRoot + 'eventos/'

$(function () {
    $('.perlnk a').click(function (e) {
        $('.peritem').css('display', 'none');
        if (this.title != 'all') {
            $(this.title).css('display', 'block');
        }
        else {
            $('.peritem').css('display', 'block');
        }        
    });

    cargarDato();

    $('#btnGrabar').click(function () {
        grabarReporte();
    });

    $('#btnCancelar').click(function () {
        var id = $('#hfIdEvento').val();
        location.href = controlador + "evento/detalle?id=" + id;
    });

    $('#btnExportar').click(function () {
        
        $('#cntExportar').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown'
        });
    });

    $('#btnEliminar').click(function () {
        eliminar();
    });

    $('#btnExportarPDF').click(function () {
        exportar('PDF');
    });

    $('#btnExportarWORD').click(function () {
        exportar('WORD');
    });

    cargarBusqueda();
});

function cargarDato()
{
    if ($("#hfCausaEvento").length) {
        $('#cbCausaEvento').val($('#hfCausaEvento').val());
    }
    if ($('#hfIndicador').val() == "S")
    {
        $('#mensaje').css("display", "block");
        $('#mensaje').addClass("action-exito");
        $('#mensaje').text("Los datos fueron grabados correctamente...!");
    }
}

function addRow(contenedor)
{  
    $.ajax({
        type: 'POST',
        url: controlador + 'evento/obtenernroorden',
        dataType: 'json',
        data: { tipo:contenedor},
        cache: false,
        success: function (resultado) {

            var rowNumber = resultado;
            var tdlabel = $('#' + contenedor + '> tbody > tr').length + 1;

            if (contenedor == "RP_SECUENCIA") {
                $('#' + contenedor + '> tbody').append('<tr>' +
                    '<td><input type="text" id="areatime' + contenedor + rowNumber + '"/><span style="display:none" id="txttime' + contenedor + rowNumber + '"></span></td>' +
                    '<td><textarea id="areadesc' + contenedor + rowNumber + '"/><span style="display:none" id="txtdesc' + contenedor + rowNumber + '"></span></td>'+
                    '<td>' +
                    '<div id="add' + contenedor + rowNumber + '">' +
                    '<div class="btn-ok" onclick="agregarItem(' + rowNumber + ',\'' + contenedor + '\')" ></div>' +
                    '<div class="btn-cancel" onclick="cancelItem(' + rowNumber + ',\'' + contenedor + '\')" ></div>' +
                    '</div>' +
                    '<div id="confirm' + contenedor + rowNumber + '" style="display:none">' +
                    '<div class="btn-ok" onclick="confirmEdit(' + rowNumber + ',\'' + contenedor + '\')" ></div>' +
                    '<div class="btn-cancel" onclick="cancelEdit(' + rowNumber + ',\'' + contenedor + '\')" ></div>' +
                    '</div>' +
                    '<div id="accion' + contenedor + rowNumber + '" style="display:none">' +
                    '<div class="btn-edit" onclick="allowEditItem(' + rowNumber + ',\'' + contenedor + '\')" ></div>' +
                    '<div class="btn-delete" onclick="deleteItem(' + rowNumber + ',\'' + contenedor + '\')" ></div>' +
                    '</div>' +
                    '</td></tr>');
            }
            else if (contenedor == "RP_ACTUACION") {
                $('#' + contenedor + '> tbody').append('<tr>' +
                    '<td><span id="txtSubEstacion' + contenedor + rowNumber + '"></span></td>' +
                    '<td><span  id="txtEquipo' + contenedor + rowNumber + '"></span><input type="hidden" id="hfCodEquipo' + contenedor + rowNumber + '" /><div class="btn-open" id="btnEquipo' + contenedor + rowNumber + '" onclick="openPopup(' + rowNumber + ',\'' + contenedor + '\',\'Equipo\');" ></div></td>' +
                    '<td><input id="areaSenal' + contenedor + rowNumber + '"/><span style="display:none" id="txtSenal' + contenedor + rowNumber + '"></span></td>' +
                    '<td><span  id="txtInterruptor' + contenedor + rowNumber + '"></span><input type="hidden" id="hfCodInterruptor' + contenedor + rowNumber + '" /><div class="btn-open"  id="btnInterruptor' + contenedor + rowNumber + '" onclick="openPopup(' + rowNumber + ',\'' + contenedor + '\',\'Interruptor\');" ></div></td>' +
                    '<td><input id="areaAc' + contenedor + rowNumber + '"/><span style="display:none" id="txtAc' + contenedor + rowNumber + '"></span></td>' +
                    '<td>' +
                    '<div id="add' + contenedor + rowNumber + '">' +
                    '<div class="btn-ok" onclick="agregarItem(' + rowNumber + ',\'' + contenedor + '\')" ></div>' +
                    '<div class="btn-cancel" onclick="cancelItem(' + rowNumber + ',\'' + contenedor + '\')" ></div>' +
                    '</div>' +
                    '<div id="confirm' + contenedor + rowNumber + '" style="display:none">' +
                    '<div class="btn-ok" onclick="confirmEdit(' + rowNumber + ',\'' + contenedor + '\')" ></div>' +
                    '<div class="btn-cancel" onclick="cancelEdit(' + rowNumber + ',\'' + contenedor + '\')" ></div>' +
                    '</div>' +
                    '<div id="accion' + contenedor + rowNumber + '" style="display:none">' +
                    '<div class="btn-edit" onclick="allowEditItem(' + rowNumber + ',\'' + contenedor + '\')" ></div>' +
                    '<div class="btn-delete" onclick="deleteItem(' + rowNumber + ',\'' + contenedor + '\')" ></div>' +
                    '</div>' +
                    '</td></tr>');
            }
            else{

                $('#' + contenedor + '> tbody').append('<tr><td>' + tdlabel + '</td><td><textarea id="area' + contenedor +
                    rowNumber + '"/><span style="display:none" id="txt' + contenedor + rowNumber + '"></span></td><td>' +
                    '<div id="add' + contenedor + rowNumber + '">' +
                    '<div class="btn-ok" onclick="agregarItem(' + rowNumber + ',\'' + contenedor + '\')" ></div>' +
                    '<div class="btn-cancel" onclick="cancelItem(' + rowNumber + ',\'' + contenedor + '\')" ></div>' +
                    '</div>' +
                    '<div id="confirm' + contenedor + rowNumber + '" style="display:none">' +
                    '<div class="btn-ok" onclick="confirmEdit(' + rowNumber + ',\'' + contenedor + '\')" ></div>' +
                    '<div class="btn-cancel" onclick="cancelEdit(' + rowNumber + ',\'' + contenedor + '\')" ></div>' +
                    '</div>' +
                    '<div id="accion' + contenedor + rowNumber + '" style="display:none">' +
                    '<div class="btn-edit" onclick="allowEditItem(' + rowNumber + ',\'' + contenedor + '\')" ></div>' +
                    '<div class="btn-delete" onclick="deleteItem(' + rowNumber + ',\'' + contenedor + '\')" ></div>' +
                    '</div>' +
                    '</td></tr>');
            }

            $('#btn' + contenedor).css('display', 'none');
        },
        error: function () {
            $('#mensaje').css("display", "block");
            $('#mensaje').addClass("action-error");
            $('#mensaje').text("Ha ocurrido un error");
        }
    });   
}

function validarEntrada(indice, tipo)
{
    var elemento = "";
    var hora = "";
    var senial = "";
    var ac = "";
    var equipo = "";
    var interruptor = "";
    var flag = true;
    var mensaje = "<ul>";
    
    if (tipo == "RP_SECUENCIA") {
        hora = $('#areatime' + tipo + indice).val();
        elemento = $('#areadesc' + tipo + indice).val();

        if (hora == "") {
            flag = false;
            mensaje = mensaje + "<li>Ingrese la hora</li>";
        }
        else
        {
            if (!validateTime(hora))
            {
                flag = false;
                mensaje = mensaje + "<li>Ingrese una hora válida.</li>";
            }
        }
        if (elemento == "")
        {
            flag = false;
            mensaje = mensaje + "<li>Ingrese el comentario</li>";
        }

    }
    else if (tipo == "RP_ACTUACION") {
        equipo = $('#hfCodEquipo' + tipo + indice).val();
        interruptor = $('#hfCodInterruptor' + tipo + indice).val();
        senial = $('#areaSenal' + tipo + indice).val();
        ac = $('#areaAc' + tipo + indice).val();

        if (equipo == "")
        {
            flag = false;
            mensaje = mensaje + "<li>Seleccione el equipo</li>";
        }
        if (interruptor == "")
        {
            flag = false;
            mensaje = mensaje + "<li>Seleccione el interruptor</li>";
        }
        if (senial == "")
        {
            flag = false;
            mensaje = mensaje + "<li>Ingrese la señal</li>";
        }
        if (ac == "")
        {
            flag = false;
            mensaje = mensaje + "<li>Especifique A/C</li>";
        }
    }
    else {
        elemento = $('#area' + tipo + indice).val();
        if (elemento == "") {
            flag = false;
            mensaje = mensaje + "<li>Ingrese el comentario</li>";
        }
    }

    if (flag == false) {
        return mensaje;
    }
    return "";
}

function openPopup(indice, tipo, caso)
{
    $('#hfIndicadorPopup').val(caso);
    $('#hfOrdenEquipo').val(indice);

    $('#busquedaEquipo').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
}

function agregarItem(indice, tipo)
{
    var elemento = "";
    var hora = "";
    var senial = "";
    var ac = "";
    var equipo = "";
    var interruptor = "";
    
    if (tipo == "RP_SECUENCIA")
    {
        hora = $('#areatime' + tipo + indice).val();
        elemento = $('#areadesc' + tipo + indice).val();
    }
    else if (tipo == "RP_ACTUACION")
    {
        equipo = $('#hfCodEquipo' + tipo + indice).val();
        interruptor = $('#hfCodInterruptor' + tipo + indice).val();
        senial = $('#areaSenal' + tipo + indice).val();
        ac = $('#areaAc' + tipo + indice).val();
    }
    else
    {
        elemento = $('#area' + tipo + indice).val();
    }

    var validacion = validarEntrada(indice, tipo);
   
    if (validacion == "") {      

        $.ajax({
            type: 'POST',
            url: controlador + 'evento/additemperturbacion',
            dataType: 'json',
            data: {
                nroOrden: indice, tipo: tipo, descripcion: elemento, hora : hora, equipo : equipo, interruptor: interruptor, senial : senial, ac : ac
            },
            cache: false,
            success: function (resultado) {
                if (resultado == 1) {

                    if (tipo == "RP_SECUENCIA") {

                        $('#txttime' + tipo + indice).html(hora);
                        $('#txttime' + tipo + indice).css('display', 'block');
                        $('#areatime' + tipo + indice).css('display', 'none');
                        $('#txtdesc' + tipo + indice).html(elemento);
                        $('#txtdesc' + tipo + indice).css('display', 'block');
                        $('#areadesc' + tipo + indice).css('display', 'none');
                        $('#add' + tipo + indice).css('display', 'none');
                        $('#accion' + tipo + indice).css('display', 'block');
                        $('#confirm' + tipo + indice).css('display', 'none');
                    }
                    else if (tipo == "RP_ACTUACION") {                    

                        $('#txtSenal' + tipo + indice).html(senial);
                        $('#txtSenal' + tipo + indice).css('display', 'block');
                        $('#areaSenal' + tipo + indice).css('display', 'none');
                        $('#txtAc' + tipo + indice).html(ac);
                        $('#txtAc' + tipo + indice).css('display', 'block');
                        $('#areaAc' + tipo + indice).css('display', 'none');
                        $('#btnEquipo' + tipo + indice).css('display', 'none');
                        $('#btnInterruptor' + tipo + indice).css('display', 'none');
                        $('#add' + tipo + indice).css('display', 'none');
                        $('#accion' + tipo + indice).css('display', 'block');
                        $('#confirm' + tipo + indice).css('display', 'none');
                    }
                    else {

                        $('#txt' + tipo + indice).html(elemento);
                        $('#txt' + tipo + indice).css('display', 'block');
                        $('#area' + tipo + indice).css('display', 'none');
                        $('#add' + tipo + indice).css('display', 'none');
                        $('#accion' + tipo + indice).css('display', 'block');
                        $('#confirm' + tipo + indice).css('display', 'none');
                    }

                    $('#btn' + tipo).css('display', 'block');
                }
                else {
                    $('#mensaje').css("display", "block");
                    $('#mensaje').addClass("action-error");
                    $('#mensaje').text("Ha ocurrido un error");
                }
            },
            error: function () {
                $('#mensaje').css("display", "block");
                $('#mensaje').addClass("action-error");
                $('#mensaje').text("Ha ocurrido un error");
            }
        });
    }
    else
    {
        $('#mensaje').css("display", "block");
        $('#mensaje').addClass("action-alert");
        $('#mensaje').html(validacion);
    }
}

function deleteItem(indice, tipo)
{
    if (confirm('¿Está seguro de quitar este elemento?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'evento/deleteitemperturbacion',
            dataType: 'json',
            data: {
                nroOrden: indice, tipo: tipo,
            },
            cache: false,
            success: function (resultado) {
                if (resultado == 1) {
                    $('#accion' + tipo + indice).closest('tr').remove();
                    if (tipo != "RP_ACTUACION" && tipo != "RP_SECUENCIA") {
                        var id = 1;
                        $('#' + tipo + '> tbody tr').each(function () {
                            $(this).find("td").eq(0).text(id);
                            id++;
                        });
                    }
                }
                else {
                    $('#mensaje').css("display", "block");
                    $('#mensaje').addClass("action-error");
                    $('#mensaje').text("Ha ocurrido un error");
                }
            },
            error: function () {
                $('#mensaje').css("display", "block");
                $('#mensaje').addClass("action-error");
                $('#mensaje').text("Ha ocurrido un error");
            }
        });
    }
}

function cancelItem(indice, tipo)
{
    $('#add' + tipo + indice).closest('tr').remove();
    $('#btn' + tipo).css('display', 'block');

    $('#mensaje').removeClass("action-alert");
    $('#mensaje').css("display", "block");
    $('#mensaje').addClass("action-message");
    $('#mensaje').html("Ingrese los datos");
}

function allowEditItem(indice, tipo)
{
    if (tipo != "RP_ACTUACION" && tipo != "RP_SECUENCIA") {
        $('#area' + tipo + indice).val($('#txt' + tipo + indice).text());
        $('#accion' + tipo + indice).css('display', 'none');
        $('#confirm' + tipo + indice).css('display', 'block');
        $('#txt' + tipo + indice).css('display', 'none');
        $('#area' + tipo + indice).css('display', 'block');
    }
    else if (tipo == "RP_SECUENCIA")
    {
        $('#areatime' + tipo + indice).val($('#txttime' + tipo + indice).text());
        $('#areadesc' + tipo + indice).val($('#txtdesc' + tipo + indice).text());
        $('#txttime' + tipo + indice).css('display', 'none');
        $('#txtdesc' + tipo + indice).css('display', 'none');
        $('#areatime' + tipo + indice).css('display', 'block');
        $('#areadesc' + tipo + indice).css('display', 'block');

        $('#accion' + tipo + indice).css('display', 'none');
        $('#confirm' + tipo + indice).css('display', 'block');
    }
    else if (tipo == "RP_ACTUACION")
    {
        $('#areaSenal' + tipo + indice).val($('#txtSenal' + tipo + indice).text());
        $('#areaAc' + tipo + indice).val($('#txtAc' + tipo + indice).text());       
        $('#areaAc' + tipo + indice).css('display', 'block');
        $('#areaSenal' + tipo + indice).css('display', 'block');
        $('#txtSenal' + tipo + indice).css('display', 'none');
        $('#txtAc' + tipo + indice).css('display', 'none');
        $('#btnEquipo' + tipo + indice).css('display', 'block');
        $('#btnInterruptor' + tipo + indice).css('display', 'block');
        $('#accion' + tipo + indice).css('display', 'none');
        $('#confirm' + tipo + indice).css('display', 'block');
    }
}

function confirmEdit(indice, tipo)
{
    var elemento = "";
    var hora = "";
    var senial = "";
    var ac = "";
    var equipo = "";
    var interruptor = "";

    if (tipo == "RP_SECUENCIA")
    {
        hora = $('#areatime' + tipo + indice).val();
        elemento = $('#areadesc' + tipo + indice).val();
    }
    else if (tipo == "RP_ACTUACION")
    {
        equipo = $('#hfCodEquipo' + tipo + indice).val();
        interruptor = $('#hfCodInterruptor' + tipo + indice).val();
        senial = $('#areaSenal' + tipo + indice).val();
        ac = $('#areaAc' + tipo + indice).val();
    }
    else
    {
        elemento = $('#area' + tipo + indice).val();
    }

    var validacion = validarEntrada(indice, tipo);

    if (validacion == "") {

        $.ajax({
            type: 'POST',
            url: controlador + 'evento/edititemperturbacion',
            dataType: 'json',
            data: {
                nroOrden: indice, tipo: tipo, descripcion: elemento, hora: hora, equipo: equipo, interruptor: interruptor, senial: senial, ac: ac
            },
            cache: false,
            success: function (resultado) {
                if (resultado == 1) {

                    if (tipo == "RP_SECUENCIA") {
                        $('#txttime' + tipo + indice).html(hora);
                        $('#txttime' + tipo + indice).css('display', 'block');
                        $('#areatime' + tipo + indice).css('display', 'none');
                        $('#txtdesc' + tipo + indice).html(elemento);
                        $('#txtdesc' + tipo + indice).css('display', 'block');
                        $('#areadesc' + tipo + indice).css('display', 'none');

                        $('#add' + tipo + indice).css('display', 'none');
                        $('#accion' + tipo + indice).css('display', 'block');
                        $('#confirm' + tipo + indice).css('display', 'none');
                    }
                    else if (tipo == "RP_ACTUACION") {

                        $('#txtSenal' + tipo + indice).html(senial);
                        $('#txtSenal' + tipo + indice).css('display', 'block');
                        $('#areaSenal' + tipo + indice).css('display', 'none');
                        $('#txtAc' + tipo + indice).html(ac);
                        $('#txtAc' + tipo + indice).css('display', 'block');
                        $('#areaAc' + tipo + indice).css('display', 'none');
                        $('#btnEquipo' + tipo + indice).css('display', 'none');
                        $('#btnInterruptor' + tipo + indice).css('display', 'none');
                        $('#add' + tipo + indice).css('display', 'none');
                        $('#accion' + tipo + indice).css('display', 'block');
                        $('#confirm' + tipo + indice).css('display', 'none');
                    }
                    else {

                        $('#txt' + tipo + indice).html(elemento);
                        $('#txt' + tipo + indice).css('display', 'block');
                        $('#area' + tipo + indice).css('display', 'none');
                        $('#accion' + tipo + indice).css('display', 'block');
                        $('#confirm' + tipo + indice).css('display', 'none');
                    }

                    $('#btn' + tipo).css('display', 'block');
                }
                else {
                    $('#mensaje').css("display", "block");
                    $('#mensaje').addClass("action-error");
                    $('#mensaje').text("Ha ocurrido un error");
                }
            },
            error: function () {
                $('#mensaje').css("display", "block");
                $('#mensaje').addClass("action-error");
                $('#mensaje').text("Ha ocurrido un error");
            }
        });       
    }
    else
    {
        $('#mensaje').css("display", "block");
        $('#mensaje').addClass("action-alert");
        $('#mensaje').html("Ingrese la descripción.");
    }
}

function cancelEdit(indice, tipo)
{
    if (tipo != "RP_ACTUACION" && tipo != "RP_SECUENCIA") {

        $('#accion' + tipo + indice).css('display', 'block');
        $('#confirm' + tipo + indice).css('display', 'none');
        $('#txt' + tipo + indice).css('display', 'block');
        $('#area' + tipo + indice).css('display', 'none');
    }
    else if (tipo == "RP_SECUENCIA")
    {
        $('#accion' + tipo + indice).css('display', 'block');
        $('#confirm' + tipo + indice).css('display', 'none');
        $('#txttime' + tipo + indice).css('display', 'block');
        $('#txtdesc' + tipo + indice).css('display', 'block');
        $('#areatime' + tipo + indice).css('display', 'none');
        $('#areadesc' + tipo + indice).css('display', 'none');
    }
    else if (tipo == "RP_ACTUACION") {
        $('#accion' + tipo + indice).css('display', 'block');
        $('#confirm' + tipo + indice).css('display', 'none');
        $('#txtSenal' + tipo + indice).css('display', 'block');
        $('#txtAc' + tipo + indice).css('display', 'block');
        $('#areaSenal' + tipo + indice).css('display', 'none');
        $('#areaAc' + tipo + indice).css('display', 'none');
        $('#btnEquipo' + tipo + indice).css('display', 'none');
        $('#btnInterruptor' + tipo + indice).css('display', 'none');
    }

    $('#btn' + tipo).css('display', 'block');
    $('#mensaje').css("display", "block");
    $('#mensaje').addClass("action-message");
    $('#mensaje').html("Ingrese los datos");
}

function cargarBusqueda()
{
    $.ajax({
        type: "POST",
        url: controlador + "busqueda/index",
        data: { },
        success: function (evt) {
            $('#busquedaEquipo').html(evt);

        },
        error: function (req, status, error) {
            alert("Ha ocurrido un error.");
        }
    });
}

function seleccionarEquipo(idEquipo, substacion, equipo, empresa, idEmpresa) {
      
    var caso = $('#hfIndicadorPopup').val();
    var id = $('#hfOrdenEquipo').val();   
    if (caso == "Equipo") {
        $('#txtSubEstacionRP_ACTUACION' + id).text(substacion);
        $('#txtEquipoRP_ACTUACION' + id).text(equipo);
        $('#hfCodEquipoRP_ACTUACION' + id).val(idEquipo);
    }
    if (caso == "Interruptor") {
        $('#txtInterruptorRP_ACTUACION' + id).text(equipo);
        $('#hfCodInterruptorRP_ACTUACION' + id).val(idEquipo);
    }

    $('#busquedaEquipo').bPopup().close();
}

function validateTime(inputStr)
{
    if (!inputStr || inputStr.length<1) {return false;}
    var time = inputStr.split(':');
    return (time.length === 2 
           && parseInt(time[0],10)>=0 
           && parseInt(time[0],10)<=23 
           && parseInt(time[1],10)>=0 
           && parseInt(time[1],10)<=59) || 
           (time.length === 3
           && parseInt(time[0],10)>=0 
           && parseInt(time[0],10)<=23 
           && parseInt(time[1],10)>=0 
           && parseInt(time[1],10)<=59
           && parseInt(time[2],10)>=0 
           && parseInt(time[2],10)<=59)
}

function grabarReporte()
{
    var id = $('#hfIdEvento').val();
    var causa = $('#cbCausaEvento').val();
    var descripcion = $('#eventoAsunto').val();
    var analisis = $('#eventoAnalisis').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'evento/grabarreporte',
        dataType: 'json',
        data: {
            idEvento: id, idCausa : causa, asunto:descripcion, analisis:analisis
        },
        cache: false,
        success: function (resultado) {
            if (resultado == 1) {      
                location.href = controlador + "evento/perturbacion?id=" + id + "&ind=S";            }
            else
            {
                $('#mensaje').css("display", "block");
                $('#mensaje').addClass("action-error");
                $('#mensaje').text("Ha ocurrido un error");
            }
        },
        error: function () {
            $('#mensaje').css("display", "block");
            $('#mensaje').addClass("action-error");
            $('#mensaje').text("Ha ocurrido un error");
        }
    });
}

function exportar(tipo)
{
    var id = $('#hfIdEvento').val();
    $.ajax({
        type: 'POST',
        url: controlador + 'evento/generararchivo',
        dataType: 'json',
        data: {
            idEvento: id, tipo : tipo
        },
        cache: false,
        success: function (resultado) {
            if (resultado == 1) {
                window.location = controlador + "evento/exportar?ind=" + tipo;
            }
            else {
                $('#mensaje').css("display", "block");
                $('#mensaje').addClass("action-error");
                $('#mensaje').text("Ha ocurrido un error");
            }
        },
        error: function () {
            $('#mensaje').css("display", "block");
            $('#mensaje').addClass("action-error");
            $('#mensaje').text("Ha ocurrido un error");
        }
    });
}

function eliminar()
{
    if (confirm('¿Está seguro de realizar esta operación?')) {
        var id = $('#hfIdEvento').val();
        $.ajax({
            type: 'POST',
            url: controlador + 'evento/eliminarreporte',
            dataType: 'json',
            data: {
                idEvento: id
            },
            cache: false,
            success: function (resultado) {
                if (resultado == 1) {
                    location.href = controlador + "evento/detalle?id=" + id;
                }
                else {
                    $('#mensaje').css("display", "block");
                    $('#mensaje').addClass("action-error");
                    $('#mensaje').text("Ha ocurrido un error");
                }
            },
            error: function () {
                $('#mensaje').css("display", "block");
                $('#mensaje').addClass("action-error");
                $('#mensaje').text("Ha ocurrido un error");
            }
        });
    }
}