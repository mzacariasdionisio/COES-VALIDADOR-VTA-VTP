// scrips relacionados => "globales.js"
var controlador = siteRoot + 'IEOD/EquiposSEIN/';
var ruta = siteRoot + 'IEOD/EquiposSEIN/';

var listFormatCodi = [];
var listFormatPeriodo = [];

$(function () {
    $('#cbEmpresa').multipleSelect({
        width: '200px',
        filter: true
    });
    $('#cbFamilia').multipleSelect({
        width: '200px',
        filter: true
    });

    $('#idFecha').Zebra_DatePicker({
    });
    $('#idFechaF').Zebra_DatePicker({
    });
    $('#btnBuscar').click(function () {
        mostrarListado();
    });
    

    valoresIniciales();
    mostrarListado();/**/
    ocultarColumnas();/**/

    $(window).resize(function () {
        $('#listadoSEIN').css("width", $('#mainLayout').width() + "px");
    });
    $('#MessagesClose').hide();
});

function valoresIniciales() {
    
    $('#cbEmpresa').multipleSelect('checkAll');
    $('#cbFamilia').multipleSelect('checkAll');
   

}

function mostrarListado() {



    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    var Familia = $('#cbFamilia').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    if (Familia == "[object Object]") formato = "-1";
    if (Familia == "") Familia = "-1";
    $('#hfEmpresa').val(empresa);
    $('#hfFamilia').val(Familia);
    $.ajax({
        type: 'POST',
        url: controlador + "ListaVal",
        data: {
            sEmpresa: $('#hfEmpresa').val(), fecha: $('#idFecha').val(),fechaFin: $('#idFechaF').val(), sFamilia: $('#hfFamilia').val(),
            nroPagina: '1', orden: '1'
        },

        success: function (evt) {
            $('#listadoSEIN').css("width", $('#mainLayout').width() + "px");
            $('#listadoSEIN').html(evt);
            $('#tablaSEIN').dataTable({
                "scrollY": 430,
                "scrollX": true,
                "sDom": 't',
                "ordering": true,
                "iDisplayLength": 50
            });
        },
        error: function () {
            mensajeOperacion("Ha ocurrido un error");
        }
    });

   /* $('#MessagesClose').hide();*/
}

function aprobar(val)
{
    mensajeOperacion("\u00BF"+"Esta seguro de aprobar la solicitud?", null
            ,{
                showCancel: true,
                onOk: function () {

                    var indice = 0;
                    var ind = 1;
                    var encontrado = 0;
                    $('#tabla tbody tr').each(function(index){
       
                        if (encontrado == 0) { 
            
                            $(this).children("td").each(function (index2) {
                                if (index2 == 1) {

                                    var dato = $(this).text();
                                    if (dato == val) {
                                        encontrado = 1;
                                        indice = ind;
                                    }
                                }
            
                            });
                        }
                        ind = ind + 1;
                    });
                    var tableReg = document.getElementById('tabla');
                    var cellsOfRow = "";
                    cellsOfRow = tableReg.rows[indice].getElementsByTagName('td');

    
                    /* alert(ruta + 'EquiposSEIN/AprobarProceso');*/

                    $.ajax({

                        type: 'POST',
                        url: ruta + 'AprobarProceso',
                        dataType: 'json',
                        data: {
                            codigo: cellsOfRow[1].innerHTML, idempresa: cellsOfRow[0].innerHTML, idfamilia: cellsOfRow[3].innerHTML, idequipo: cellsOfRow[4].innerHTML,
                            idmotivo: cellsOfRow[2].innerHTML, ifecha: cellsOfRow[9].innerHTML, idUbicacion: 0, estado: 2
                        },
                        cache: false,
                        success: function (resultado) {
                            if (resultado == 1) {
                                mostrarListado();
                                mensajeOperacion("Registro Aprobado", 1);
                                /*
                                document.location.href = controlador + "ValidarSolicitudes";*/
                            }
                            else {
                                mensajeOperacion("Error al grabar equipo");
                            }

                        },
                        error: function () {
                            mensajeOperacion("Error al grabar equipo");
                        }

                    });

    
                },
                onCancel: function () {

                }
            });

};

function rechazar(val) {
    mensajeOperacion("\u00BF"+"Esta seguro de rechazar la solicitud?", null
            ,{
                showCancel: true,
                onOk: function () {


    var indice = 0;
    var ind = 1;
    var encontrado = 0;
    $('#tabla tbody tr').each(function (index) {

        if (encontrado == 0) {

            $(this).children("td").each(function (index2) {
                if (index2 == 1) {

                    var dato = $(this).text();
                    if (dato == val) {
                        encontrado = 1;
                        indice = ind;
                    }
                }

            });
        }
        ind = ind + 1;
    });
    var tableReg = document.getElementById('tabla');
    var cellsOfRow = "";
    cellsOfRow = tableReg.rows[indice].getElementsByTagName('td');




    $.ajax({

        type: 'POST',
        url: ruta + 'AprobarProceso',
        dataType: 'json',
        data: {
            codigo: cellsOfRow[1].innerHTML, idempresa: cellsOfRow[0].innerHTML, idfamilia: cellsOfRow[3].innerHTML, idequipo: cellsOfRow[4].innerHTML,
            idmotivo: cellsOfRow[2].innerHTML, ifecha: cellsOfRow[9].innerHTML, idUbicacion: 0, estado: 3
        },
        cache: false,
        success: function (resultado) {
            if (resultado == 1) {
                mostrarListado();
                mensajeOperacion("Registro rechazado", 1);
                /*document.location.href = controlador + "ValidarSolicitudes";*/
            }
            else {
                mensajeOperacion("Error al grabar equipo");
            }

        },
        error: function () {
            mensajeOperacion("Error al grabar equipo");
        }

    });
                },
                onCancel: function () {

                }
            });


};

function ocultarColumnas() {

    var tabla = $('#tabla').dataTable();

    tabla.fnSetColumnVis(1, false);

 /*   var table = $('#tablaSEIN').dataTable();

    var bVis9 = oTable.fnSettings().aoColumns[1].bVisible;
    oTable.fnSetColumnVis(1, bVis9 ? false : true);*/

    /*table.column(1).visible(false);*/
    /*$('#tablaSEIN').dataTable().destroy();*/
   /* $('#tablaSEIN').dataTable(
         {
             "columnDefs": [
                 {
                     "targets": [1],
                     "visible": false,
                     "searchable": false
                 },
                 {
                     "targets": [2],
                     "visible": false,
                     "searchable": false
                 }
             ]
         }

        );*/
        


  /*  var bVis10 = oTable.fnSettings().aoColumns[10].bVisible;
    oTable.fnSetColumnVis(10, bVis10 ? false : true);

    var bVis11 = oTable.fnSettings().aoColumns[11].bVisible;
    oTable.fnSetColumnVis(11, bVis11 ? false : true);*/
}
