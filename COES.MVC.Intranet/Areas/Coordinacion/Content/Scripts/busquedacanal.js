var controladorBusqueda = siteRoot + 'coordinacion/registroobservacion/'

$(function () {

    $("#tablaCanal .check").on('click', function () {

        var contador = $("#hfContador").val();
        var seleccion = $("#hfSeleccion").val();


        if (seleccion == "")
            contador = 0;

        if (this.checked) {      

            //Si no esta se agrega            
            if ($("#hfSeleccion").val().indexOf($(this).val()) == -1) {              
                var constante = (contador > 0) ? "," : "";
                $("#hfSeleccion").val(seleccion + constante + $(this).val());
                var resultado = parseInt(contador) + 1;
                $("#hfContador").val(parseInt(contador) + 1);       
                
            } 
            
        } else { //Si hay seleccion de otra pagina y y no esta chequeado buscarlo y sacarlo de la seleccion

            $("#hfSeleccion").val($("#hfSeleccion").val().replace($(this).val() + ",", "")); //con su coma
            $("#hfSeleccion").val($("#hfSeleccion").val().replace("," + $(this).val(), "")); //Si el el ultimo
            $("#hfSeleccion").val($("#hfSeleccion").val().replace($(this).val(), "")); //Si es el unico elemento
            $("#hfSeleccion").val($("#hfSeleccion").val().replace(",,", ",")); //si queda doble coma
        }
       
        console.log($("#hfSeleccion").val());
    });

    $("#btnBuscar").unbind();
    $('#btnBuscar').click(function () {

        $("#hfSeleccion").val("");

        buscarCanal();
    });

    $('#txtFiltro').focus();

    $('#searchContentCanal').keypress(function (e) {
        if (e.keyCode == '13') {
            $('#btnBuscar').click();
        }
    });

    $("#btnObtenerSenalesObservadasBusqueda").unbind();
    $('#btnObtenerSenalesObservadasBusqueda').click(function () {

        $("#hfSeleccion").val("");

        buscarSenalesObservadasBusqueda();
    });
       

});


    function buscarCanal() {
        pintarPaginado();
        pintarBusqueda(1);
    }

    function buscarSenalesObservadasBusqueda() {
        pintarPaginadoSenalesObservadasBusqueda();
        pintarBusquedaSenalesObservadasBusqueda(1);
    }



function pintarPaginado() {
    
    $.ajax({
        type: "POST",
        url: controladorBusqueda + "paginadocanal",
        data: {
            idEmpresa: $('#hfIdEmpresaBusqueda').val(),
            idZona: $('#cbZona').val(),
            idTipo: $('#cbTipo').val(),
            filtro: $('#txtFiltro').val()
        },
        global: false,
        success: function (evt) {
            $('#cntPaginado').html(evt);
            mostrarPaginado();            
        },
        error: function (req, status, error) {
            mostrarMensajePopup('messageCanal', 'error', 'Ha ocurrido un error.');
        }
    });
}

function pintarPaginadoSenalesObservadasBusqueda() {

    $.ajax({
        type: "POST",
        url: controladorBusqueda + "paginadocanalSenalesObservadasBusqueda",
        data: {
            idEmpresa: $('#hfIdEmpresaBusqueda').val()
        },
        global: false,
        success: function (evt) {
            $('#cntPaginado').html(evt);
            mostrarPaginado();            
        },
        error: function (req, status, error) {
            mostrarMensajePopup('messageCanal', 'error', 'Ha ocurrido un error.');
        }
    });
}

function pintarBusqueda(nroPagina) {

    $.ajax({
        type: "POST",
        url: controladorBusqueda + "listacanal",
        global: false,
        data: {
            idEmpresa: $('#hfIdEmpresaBusqueda').val(),
            idZona: $('#cbZona').val(),
            idTipo: $('#cbTipo').val(),
            filtro: $('#txtFiltro').val(),
            nroPagina: nroPagina
        },
        success: function (evt) {
            $('#cntCanal').html(evt);
            $('#messageCanal').hide();

            establecerseleccionados()
        },
        error: function (req, status, error) {
            mostrarMensajePopup('messageCanal', 'error', 'Ha ocurrido un error.');
        }
    });
}

function pintarBusquedaSenalesObservadasBusqueda(nroPagina) {

    $.ajax({
        type: "POST",
        url: controladorBusqueda + "listacanalSenalesObservadasBusqueda",
        global: false,
        data: {
            idEmpresa: $('#hfIdEmpresaBusqueda').val(),
            nroPagina: nroPagina
        },
        success: function (evt) {

            $('#cntCanal').html(evt);
            $('#messageCanal').hide();

            establecerseleccionados()
        },
        error: function (req, status, error) {

            mostrarMensajePopup('messageCanal', 'error', 'Ha ocurrido un error.');
        }
    });
}

mostrarMensajePopup = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
    $('#' + id).show();
}


function establecerseleccionados() {

    console.log("entro a funcion");

    $("#tablaCanal input").each(function () {
        
        if ($("#hfSeleccion").val().indexOf($(this).val()) > - 1) {

            $(this).attr('checked', true);
            console.log("entro a establecer check a: " + $(this).val());
        }

    });

        
}
