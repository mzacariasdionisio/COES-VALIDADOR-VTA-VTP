var controlador = siteRoot + 'valorizaciondiaria/';
$(function () {
    $('#cbEmpresa').multipleSelect({
        width: '222px',
        filter: true
    });
    $('#cbCuenca').multipleSelect({
        width: '222px',
        filter: true
    });
    $('#cbPtoMedida').multipleSelect({
        width: '222px',
        filter: true
    });
    $('#FechaDesde').Zebra_DatePicker({
        
    });

    $('#FechaHasta').Zebra_DatePicker({
       
    });
    $('#btnBuscar').click(function () {
        buscarDatos();
    });
    buscarDatos();
});

function buscarDatos() {
 //   pintarPaginado();
    mostrarListado(1);
}

function mostrarListado(nroPagina) {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    
    if (empresa == "[object Object]") empresa = "-1";
    $('#hfEmpresa').val(empresa);
    var tipoInformacion = $('#cbTipoInformacion').val();
    $.ajax({
        type: 'POST',
        url: controlador + "reporte/lista",
        data: {
            idsEmpresa: $('#hfEmpresa').val(), idTipoInformacion: tipoInformacion, fechaInicial: $('#FechaDesde').val(), fechaFinal: $('#FechaHasta').val(),
            nroPagina: nroPagina
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": true,
                "sDom": 't',
                "iDisplayLength": 50
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}