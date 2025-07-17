var controlador = siteRoot + 'Siosein/TablasPrie/';

$(function () {        
   $('#btnAgregar').click(function () {
       popUpTablaPrie(-1);
    });

    cargarValoresIniciales();

});

function cargarValoresIniciales() {
    generaListado();    
}

function generaListado() {
    mostrarListadoTablasPrie();
}

function mostrarListadoTablasPrie() {
    $.ajax({
        type: 'POST',
        url: controlador + "lista",
        data: {},
        success: function (evt) {
            //$('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            table = $('#tabla').dataTable({
                "scrollY": 600,
                //"scrollX": true,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 70
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function popUpTablaPrie(idTabla) {
    $.ajax({
        type: 'POST',        
        url: controlador + 'ViewTablaPrie',
        dataType: "json",
        data: {
            idTabla: idTabla            
        },
        success: function (result) {
            document.getElementById("mantenimientoTabla").innerHTML = result;
            setTimeout(function () {
                $('#popupTabla').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    modalClose: false
                });

                // #region CAMBIO SIOSEI 18-09-19     
                $('#cbArea').val(codi);
                var codi = parseInt($('#hfAreaCodi').val());
                $('#cbArea').val(codi);

                $('#txtPlazoEntrega').Zebra_DatePicker({
                    //direction: -1
                });
                $('#campos').hide();
                // #endregion

            }, 50);
        },
        error: function (result) {
            alert('ha ocurrido un error al generar vista');
        }
    });

}
///UTL/////
function offshowView() {
    $('#enviosHorasOperacion').bPopup().close();
}

// funcion que muestra el detalle de una tabla con para edicion
function mostrarDetalle(idTabla) {

    popUpTablaPrie(idTabla)
}