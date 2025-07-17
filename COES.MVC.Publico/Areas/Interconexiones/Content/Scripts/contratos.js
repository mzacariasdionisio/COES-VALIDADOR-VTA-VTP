var controlador = siteRoot + 'interconexiones/'
$(function () {  
    $('#cbTipoOp').multipleSelect({
        width: '222px',
        filter: true
    });

    $('#FechaDesde').Zebra_DatePicker({

    });

    $('#FechaHasta').Zebra_DatePicker({

    });

    $('#btnBuscar').click(function () {
        //$('#listado').html("");
        //$('#paginado').html("");
        //var resultado = validarFiltros();
        //if (resultado == "") {
        //    buscarDatos();            
        //}
        //else {
        //    alert("Error:" + resultado);
        //}
        buscarDatos();
    });
    
    $('#btnNuevo').click(function () {
        formatoNuevoEnsayo()
    });

    cargarPrevio();
    buscarDatos();

    
    
});// end function

function cargarPrevio() {
    $('#cbTipoOp').multipleSelect('checkAll');    
}

function buscarDatos() {
    //pintarPaginado(1);   
    mostrarListado();
}

function mostrarListado() {
    var tipoOp = $('#cbTipoOp').multipleSelect('getSelects');

    if (tipoOp == "[object Object]") tipoOp = "-1";
    if (tipoOp == "") tipoOp = "-1";    
    
    $('#hfTipoOp').val(tipoOp);  

    $.ajax({
        type: 'POST',
        url: controlador + "reportes/ListaContratos",
        data: {            
            tipoOperacion: $('#hfTipoOp').val(),           
            fechaInicial: $('#FechaDesde').val(),
            fechaFinal: $('#FechaHasta').val()
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");
            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 50
            });
        },
        error: function () {
            alert("Ha ocurrido un error en lista");
        }
    });
}


function formatoNuevoEnsayo() {
    window.location.href = controlador + "reportes/NuevoContrato?id=0";
}

function mostrarDetalleContrato(id) {
    location.href = controlador + "reportes/NuevoContrato?id=" + id;
}