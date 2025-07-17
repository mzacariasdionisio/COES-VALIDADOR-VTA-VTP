var controlador = siteRoot + 'Ensayo/';

var OPCION_VER = 1;
var numCheckedMO = 0;
$(function () {
    $('#FechaDesde').Zebra_DatePicker({
    });
    $('#FechaHasta').Zebra_DatePicker({
    });

    $('#cbEmpresa').multipleSelect({
        width: '250px',
        filter: true,
        onClose: function (view) {
            cargarCentrales();
        }
    });
    $('#cbCentral').multipleSelect({
        width: '250px',
        filter: true
    });

    $('#cbCentral').multipleSelect('checkAll');
    $('#cbEstado').multipleSelect({
        width: '250px',
        filter: true
    });

    $("#btnAgregarNuevoEnsayo").click(function () {
        popUpAgreNewEnsayo();
    });

    $('#btnExcel').click(function () {
        exportarExcel(0);
    });

    $('#btnBuscar').click(function () {
        buscarEnsayo(1);
    });

    $(window).resize(function () {
        $('#listado').css("width", $('#mainLayout').width() + "px");
    });
    $('#btnNuevo').click(function () {
        formatoNuevoEnsayo();
    });

    buscarEnsayo(1); // muestra el listado de todos los ensayos de todas las empresas GEN
    valoresIniciales();
});

function exportarExcel(nroPagina)  ///para exportar arhivos excel 
{
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    var estado = $('#cbEstado').multipleSelect('getSelects');
    var central = $('#cbCentral').multipleSelect('getSelects');
    var finicio = $('#FechaDesde').val();
    var ffin = $('#FechaHasta').val();
    if (empresa == "[object Object]") empresa = "-1";
    $('#hfEmpresa').val(empresa);
    $('#hfEstado').val("-1");
    $('#hfCentral').val(central);
    $.ajax({
        type: 'POST',
        url: controlador + 'genera/GenerarArchivoReporteXls',
        data: {
            empresas: $('#hfEmpresa').val(), estados: $('#hfEstado').val(),
            centrales: $('#hfCentral').val(), finicios: finicio,
            ffins: ffin
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controlador + "genera/ExportarReporte";
            }
            if (result == -1) {
                alert("Ha ocurrido un error al generar reporte excel");
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error al generar arhivo excel");
        }
    });
}

function valoresIniciales() {
    $('#cbEmpresa').multipleSelect('checkAll');
    $('#cbCentral').multipleSelect('checkAll');
    $('#cbEstado').multipleSelect('checkAll');
}

function cargarCentrales() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');

    if (empresa == "[object Object]") empresa = "-1";
    $('#hfEmpresa').val(empresa);
    $.ajax({
        type: 'POST',
        url: controlador + 'genera/CargarCentrales',

        data: { idEmpresa: $('#hfEmpresa').val() },

        success: function (aData) {

            $('#centrales').html(aData);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarCentral2() {

    //Limpiando los campos al escoger nueva Empresa
    $('#cbCentral2').empty();
    $('#cbCentral2').append('<option value="' + $("#hfCentral").val() + '">--SELECCIONE--</option>');
    $('#cbModoOp2').empty();
    $('#cbModoOp2').append('<option value="' + $("#hfCentral").val() + '">--SELECCIONE--</option>');


    var idEmpresa = $('#cbEmpresa2').val();
    $.ajax({
        type: 'POST',
        url: controlador + 'genera/CargarCentral2',
        dataType: 'json',
        async: true,
        data: {
            idEmpresa: idEmpresa
        },
        success: function (evt) {
            $('#cbCentral2').empty();
            $('#cbCentral2').append('<option value="-1">--SELECCIONE--</option>');
            var listaCentral = evt.ListaEquipo;
            for (var i = 0; i < listaCentral.length; i++) {
                $('#cbCentral2').append('<option value=' + listaCentral[i].Equicodi + '>' + listaCentral[i].Equinomb + '</option>');
            }
            generarListaMO(1, evt.ListaModosOperacion); //lo coloco para que limpie la lista de MO ya que ListaModosOperacion = null
            mostrarUnidadesGeneradoras(1, -9999, -9999); //lo coloco para que limpie la lista de Unidades ya que ListaEspecial = null
        },
        error: function (err) {
            alert("Ha ocurrido un error al cargar centrales");

        }
    });
}

function cargarListaModo_Grupo() {
    var idCentral = $('#cbCentral2').val();
    var idEmpresa = $("#cbEmpresa2").val() != undefined ? $("#cbEmpresa2").val() : $('#cbEmpresa').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'genera/CargarListaGrupoModo',
        dataType: 'json',
        async: true,
        data: {
            idCentral: idCentral,
            idEmpresa: idEmpresa
        },

        success: function (aData) {
            $('#cbModoOp2').empty();
            $('#cbModoOp2').append('<option value="' + $("#hfCentral").val() + '">--SELECCIONE--</option>');
            //Pra mostrarlo por combobox
            var listaMO = aData.ListaModosOperacion;
            for (var i = 0; i < listaMO.length; i++) {
                $('#cbModoOp2').append('<option value=' + listaMO[i].Grupocodi + '>' + listaMO[i].Gruponomb + '</option>');
            }
            generarListaMO(1, aData.ListaModosOperacion, aData.ListaEspecial);
        },
        error: function (err) {
            alert("Ha ocurrido un error al cargar Modos Operacion");
        }
    });
}

function generarListaMO(opcion, ListaMO, ListaMOEspeciales) {
    $("#tabla_area").html("");
    var strHtml = '';

    if (ListaMO == null || ListaMO.length == 0) {
        ListaMO = [];
        $("#tabla_area").hide();
    } else {
        $("#tabla_area").show();
    }

    strHtml += '<tbody>';
    var m = 0;
    for (var i = 0; i < ListaMO.length; i++) {

        if (opcion == 2) {
        } else {
            strHtml += '<tr>';
            strHtml += '<td class="">';
            strHtml += '<input type="checkbox" name="check_mo_cnp"  onClick="if(this.checked == true){muestraUnidades(' + ListaMO[i].Grupocodi + ');} else{ocultaUnidades(' + ListaMO[i].Grupocodi + ');}"  id="check_mo' + ListaMO[i].Grupocodi + '" value="' + ListaMO[i].Grupocodi + '" />';
            strHtml += '<input type="hidden" id="fila_mo' + ListaMO[i].Grupocodi + '" value="' + ListaMO[i].Grupocodi + '" />';
            strHtml += '</td>';
            strHtml += '<td class="">' + ListaMO[i].Grupoabrev + '</td>'; // lo que se pinta  
            strHtml += '<td class=""><td>';
            strHtml += '</tr>';
            strHtml += '<tr>';
            strHtml += '<td class=""><td>';
            strHtml += '<td id="tabla_unidades' + ListaMO[i].Grupocodi + '">';
            strHtml += '</td>';
            //Si el MO es especial creara la lista de las unidades generadoras            
            for (var k = 0; k < ListaMOEspeciales.length; k++) {
                if (ListaMO[i].Grupocodi = ListaMOEspeciales[k].Grupocodi) {  //si es especial                                           
                    mostrarUnidadesGeneradoras(ListaMOEspeciales[k].Grupocodi, ListaMOEspeciales[k].Emprcodi, ListaMOEspeciales[k].Equipadre);
                }
            }
            strHtml += '</tr>';

        }
    }
    strHtml += '</tbody>';



    $("#tabla_area").html(strHtml);

    $('input[name=check_area_cnp]:checkbox').unbind();
    $('input[name=check_area_cnp]:checkbox').change(function () {
    });
}

function ocultaUnidades(num) {
    $("#tabla_unidades" + num).hide();
    numCheckedMO--;
}

function muestraUnidades(num) {
    $("#tabla_unidades" + num).show();
    numCheckedMO++;
    arrayListadoUnidades(1, num);
}
function verificarCierre(num) {
    arrayListadoUnidades(2, num);
}
function verificarSiHayUnidadesSeleccionadas() {
    return numCheckedMO;
}

function arrayListadoUnidades(op, ipadre) {
    $.ajax({
        type: 'POST',
        url: controlador + 'genera/CargarArrayUnidadesGeneradorasEspeciales',
        data: {
            padre: ipadre
        },
        success: function (evt) {
            var vectorUnidades = evt.listitaUnidades;
            var tam = vectorUnidades.length;
            if (vectorUnidades.length > 0) {
                if (op == 1) { //para mostrar las unidades al seleccionar un MO especial
                    for (i = 0; i < vectorUnidades.length; i++) {

                        $("#check_unidad" + vectorUnidades[i]).prop('checked', true);
                    }
                }
                if (op == 2) { //para ocular las unidades si deseleccionamos todas
                    var numDesactivados = 0;
                    for (i = 0; i < vectorUnidades.length; i++) {
                        if (!$("#check_unidad" + vectorUnidades[i]).is(':checked')) {
                            numDesactivados = numDesactivados + 1;
                        }
                    }
                    if (numDesactivados == tam) {
                        $("#tabla_unidades" + ipadre).hide();
                        $("#check_mo" + ipadre).prop('checked', false);
                        numCheckedMO--;
                    }
                }
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error al marcado");
        }
    });
}

function mostrarUnidadesGeneradoras(padre, idEmpresa, idCentral) {
    var strHtml = '';
    $.ajax({
        type: 'POST',
        url: controlador + 'genera/CargarUnidadesGeneradorasEspeciales',
        dataType: 'json',
        async: true,
        data: {
            padre: padre,
            idEmpresa: idEmpresa,
            idCentral: idCentral
        },

        success: function (aData) {  //entrega solo unidades especiales
            var listaUnidades = aData.ListaEquipo;
            $("#tabla_unidades" + padre).html("");
            $("#tabla_unidades" + padre).hide(); //ocultamos todas las tablas de las unidades, se visualizaraá deacuerdo al check del MO  
            strHtml += '<tbody>';
            strHtml += '<tr>';
            strHtml += '<td colspan="2" class="" style="vertical-align: top; text-align: center">Unidades<td>';
            strHtml += '</tr>';

            for (var i = 0; i < listaUnidades.length; i++) {
                strHtml += '<tr>';
                strHtml += '<td class="">';
                strHtml += '<input type="checkbox" name="check_unidad_cnp" onClick="if(this.checked == false){verificarCierre(' + padre + ');}" id="check_unidad' + listaUnidades[i].Equicodi + '" value="' + listaUnidades[i].Equicodi + '" />';
                strHtml += '<input type="hidden" id="fila_unidad' + listaUnidades[i].Equicodi + '" value="' + listaUnidades[i].Equicodi + '" />';
                strHtml += '</td>';
                strHtml += '<td class="">' + listaUnidades[i].Equinomb + '</td>'; // lo que se pinta
                strHtml += '</tr>';
            }
            strHtml += '</tbody>';

            $("#tabla_unidades" + padre).html(strHtml);
            $('input[name=check_unidad_cnp]:checkbox').unbind();
        },
        error: function (err) {
            alert("Ha ocurrido un error al cargar las unidades ");
        }
    });

}

function buscarEnsayo(idEstado) {
    $('#hfEstado').val(idEstado);
    pintarPaginado()
    mostrarListado(1);
}

function pintarPaginado() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    var estado = "1";
    var central = $('#cbCentral').multipleSelect('getSelects');
    var finicio = $('#FechaDesde').val();
    var ffin = $('#FechaHasta').val();

    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    if (central == "[object Object]") central = "-1";
    $('#hfEmpresa').val(empresa);
    //$('#hfEstado').val(estado);
    $('#hfCentral').val(central);
    $.ajax({
        type: 'POST',
        url: controlador + "genera/paginado",
        data: {
            //empresas: $('#hfEmpresa').val(), estados: $('#hfEstado').val(),
            empresas: $('#hfEmpresa').val(), estados: "1",
            centrales: $('#hfCentral').val(), finicios: finicio,
            ffins: ffin
        },
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function (err) {
            alert("Ha ocurrido un error paginado");
        }
    });
}

function pintarBusqueda(nroPagina) {
    mostrarListado(nroPagina);
}

function mostrarListado(nroPagina) {
    $('#hfNroPagina').val(nroPagina);
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    var central = $('#cbCentral').multipleSelect('getSelects');
    var finicio = $('#FechaDesde').val();
    var ffin = $('#FechaHasta').val();
    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    if (central == "[object Object]") central = "-1";
    if (central == "") central = "-1";
    $('#hfEmpresa').val(empresa);
    $('#hfCentral').val(central);
    $.ajax({
        type: 'POST',
        url: controlador + "genera/ListaEnsayo",
        data: {
            empresas: $('#hfEmpresa').val(), centrales: $('#hfCentral').val(), nroPaginas: nroPagina,
            estado: parseInt($('#hfEstado').val()) || 0,
            finicios: finicio, ffins: ffin
        },
        success: function (evt) {

            $('#listado').css("width", $('#mainLayout').width() + "px");
            $('#listado').html(evt);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function formatoNuevoEnsayo() {
    window.location.href = controlador + "genera/Nuevo?id=0";
}

function mostrarDetalleEnsayo(id) {
    location.href = controlador + "genera/Nuevo?id=" + id;
}

function mostrarEnsayoFormato(id) {
    // id: "Código del ensayo que pasa como parametro"
    location.href = controlador + "genera/EnvioFormato?id=" + id;
}

function mostrarEnsayoUnidades(id) {
    // id: "Código del ensayo que pasa como parametro"
    location.href = controlador + "genera/Nuevo?id=" + id;
}

function mostrarPopUpMO(id) {
    // id: "Código del ensayo que pasa como parametro"
    popUpAgreNewMO(id);
}

function editarFormatoEnsayo(id) {
    location.href = controlador + "genera/EditarFormato?id=" + id;
}


function limpiarNumUnidades() {
    numCheckedMO = 0;
}