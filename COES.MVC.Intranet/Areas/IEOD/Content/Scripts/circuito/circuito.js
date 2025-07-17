var controlador = siteRoot + "IEOD/Circuito/";
var APP_MOSTRAR_MODAL_EQUIPO_YA_CARGADO = 0;
var TIPO_POPUP_EQUIPO = 0;
var POPUP_EQUIPO = 0;
var POPUP_EQUIPO_DET = 1;
var POPUP_EQUIPO_CIRC = 2;
var TIPO_CIRCUITO_DET_EQUIPO = 100;
var TIPO_CIRCUITO_DET_CIRC = 101;
var APP_OPCION = -1;
var OPCION_COPIAR = 1;
var OPCION_EDITAR = 2;
var OPCION_NUEVO = 3;
var OPCION_ELIMINAR = 4;
var OPCION_VER = 5;
var ancho = 1100;
var NUEVOCIRCUITO = false;

$(document).ready(function ($) {
    $('#tab-container').easytabs({
        animate: false
    });

    $('#cbFiltroEmpresa').multipleSelect({
        width: '250px',
        filter: true,
        onClose: function (view) {
        }
    });
    $('#cbFiltroEmpresa').multipleSelect('checkAll');

    ancho = $("#mainLayout").width() > 1100 ? $("#mainLayout").width() - 30 : 1100;

    $("#btnNuevo").click(function () {
        nuevoCircuito();
    });

    $('#check_estado_todos').click(function () {
        var todos = -1;
        var soloActivos = 1;
        if ($(this).is(':checked')) {
            FiltrarFechasVigencia($("#datepickerMax").val(), todos);
        } else {
            FiltrarFechasVigencia($("#datepickerMax").val(), soloActivos);
        }
    });

    var fechaHoy = ObtenerFechaParaDatepicker(new Date());
    $('#datepickerMax').Zebra_DatePicker({
        
        onSelect: function () {
            var todos = -1;
            var soloActivos = 1;

            var estado = soloActivos;
            if ($("#check_estado_todos").is(':checked')) {
                estado = todos;
            }
            FiltrarFechasVigencia($(this).val(), estado);
        }
    }).val(fechaHoy);

    $("#btnConsultar").click(function () {
        mostrarListado();
    });
    mostrarListado();

});

/////////////////////////////////////////////////////////////////////////////////////////////
/// Reporte
/////////////////////////////////////////////////////////////////////////////////////////////
function mostrarListado() {
    $('#listado1').hide();
    var idEmpresa = $('#cbFiltroEmpresa').multipleSelect('getSelects');
    if (idEmpresa == "[object Object]") idTipoOperacion = "-1";
    if (idEmpresa == "") idTipoOperacion = "-1";
    $('#hfEmpresa').val(idEmpresa);

    idEmpresa = $('#hfEmpresa').val();

    $.ajax({
        type: 'POST',
        url: controlador + "Lista",
        data: {
            listaEmpresa: idEmpresa,
        },
        success: function (evt) {
            $('#listado1').show();
            $('#resultado').html(evt);

            var anchoReporte = $('#reporte').width();
            $("#resultado").css("width", (anchoReporte > ancho ? ancho : anchoReporte) + "px");

            $('#reporte').dataTable({
                "sPaginationType": "full_numbers",
                "destroy": "true",
                "bInfo": true,
                "ordering": false,
                "iDisplayLength": 25,
                "language": {
                    "emptyTable": "¡No existen circuitos registrados!"
                }
            });
        },
        error: function (err) {
            alert("Ha ocurrido un error en listado");
        }
    });
}

///////////////////////////
/// Formulario
///////////////////////////

function guardarCircuito() {
    var entity = getObjetoJson();
    if (confirm('¿Desea guardar la Dependencia de Equipos?')) {
        var msj = validarCircuito(entity);

        if (msj == "") {
            var obj = JSON.stringify(entity);

            $.ajax({
                type: 'POST',
                dataType: 'json',
                traditional: true,
                url: controlador + 'CircuitoGuardar',
                data: {
                    strJsonData: obj
                },
                cache: false,
                success: function (result) {
                    if (result.Resultado == '-1') {
                        alert('Ha ocurrido un error:' + result.Mensaje);
                    } else {
                        if (entity.Circodi != 0) {
                            alert("Se editó correctamente el circuito");
                        } else {
                            alert("Se guardó correctamente el circuito");
                        }
                        nuevoCircuito();
                        $("#formularioCircuito").hide();

                        $('#tab-container').easytabs('select', '#vistaListado');
                        mostrarListado();
                    }
                },
                error: function (err) {
                    alert('Ha ocurrido un error.');
                }
            });
        } else {
            alert(msj);
        }
    }
}

function getObjetoJson() {
    var obj = {};

    obj.Circodi = parseInt($("#hfCodigo").val()) || 0;
    obj.Circnomb = $("#nomCircuito").val()
    obj.Equicodi = parseInt($("#cboEquipo").val()) || 0;
    obj.ListaDetalleCircuito = listarDetalleCircuito();

    return obj;
}

function validarCircuito(obj) {
    var msj = "";
    if (obj.Circnomb == null || obj.Circnomb.trim() == "") {
        msj += "Debe ingresar nombre de circuito." + "\n";
    }

    if (obj.Equicodi <= 0) {
        msj += "Debe seleccionar un equipo." + "\n";
    }

    if (obj.ListaDetalleCircuito.length == 0) {
        msj += "No existe dependencias para el equipo seleccionado.";
    }

    return msj;
}

function listarDetalleCircuito() {
    return det_generarListaDetFromHtml(0);
}

function inicializarFormulario() {
    $("#formularioCircuito").show();

    $('#btnBuscarEquipoPopup').unbind();
    $('#btnBuscarEquipoPopup').click(function () {
        TIPO_POPUP_EQUIPO = POPUP_EQUIPO;
        visualizarBuscarEquipo();
    });

    $('#btnAgregarDetEquipo').unbind();
    $('#btnAgregarDetEquipo').click(function () {
        TIPO_POPUP_EQUIPO = POPUP_EQUIPO_DET;
        visualizarBuscarEquipo();
    });

    $("#nomEmpresa").val('');
    $("#nomUbicacion").val('');
    $("#cboEquipo").val('');
    $("#nomEquipo").val('');
    $("#cboEmpresa").val('');
    $("#famAbrev").val('');
    $("#nomCircuito").val('');
    $('#nomCircuito').removeAttr("disabled");
    $("#hfHoDetalleJson").val('');
    $("#hfCodigo").val(0);

    det_actualizarTablaDetalle();

    $("#btnAceptar2").unbind();
    $("#btnAceptar2").click(function () {
        guardarCircuito();
    });

    $('#tab-container').easytabs('select', '#vistaDetalle');
}

function nuevoCircuito() {
    APP_OPCION = OPCION_NUEVO;
    inicializarFormulario();

    $("#btnBuscarEquipoPopup").show();
    $("#btnAgregarDetEquipo").show();
    $("#btnAceptar2").show();
    $("#btnCancelar2").show();
}

function verCircuito(id) {
    APP_OPCION = OPCION_VER;
    cargarCircuito(id);
}

function editarCircuito(id) {
    APP_OPCION = OPCION_EDITAR;
    cargarCircuito(id);
}

function cargarCircuito(id) {
    $.ajax({
        type: 'POST',
        dataType: 'json',
        traditional: true,
        url: controlador + 'CircuitoEditar',
        data: {
            id: id
        },
        cache: false,
        success: function (result) {
            if (result.Resultado == '-1') {
                alert('Ha ocurrido un error: ' + result.Mensaje);
            } else {
                inicializarFormulario();

                $("#nomEmpresa").val(result.Circuito.Emprnomb);
                $("#cboEmpresa").val(result.Circuito.Emprcodi);
                $("#sEmprcodi").val("[" + result.Circuito.Emprcodi + "]");

                $("#famAbrev").val(result.Circuito.Famabrev);

                $("#nomEquipo").val(result.Circuito.Equinomb);
                $("#cboEquipo").val(result.Circuito.Equicodi);

                $("#nomUbicacion").val(result.Circuito.Areanomb);
                $("#cboUbicacion").val(result.Circuito.Areacodi);

                $("#nomCircuito").val(result.Circuito.Circnomb);
                $("#hfCodigo").val(result.Circuito.Circodi);


                //var esActualizado = false;
                var html = det_generarTablaDetalle(result.ListaDetalleCircuito, APP_OPCION);
                $("#tablaDetalleCircuito").html(html);
                $('.datepicker1').Zebra_DatePicker( );
                

                $(".detalle_circuito").show();

                if (APP_OPCION == OPCION_VER) {
                    $("#btnBuscarEquipoPopup").hide();
                    $("#btnAgregarDetEquipo").hide();
                    $("#btnAceptar2").hide();
                    $("#btnCancelar2").hide();
                    $("#nomCircuito").prop('disabled', 'disabled');
                } else {
                    $("#btnBuscarEquipoPopup").hide();
                    $("#btnAgregarDetEquipo").show();
                    $("#btnAceptar2").show();
                    $("#btnCancelar2").show();
                }
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function darbajaCircuito(id) {
    $.ajax({
        type: 'POST',
        dataType: 'json',
        traditional: true,
        url: controlador + 'CircuitoDarBaja',
        data: {
            id: id
        },
        cache: false,
        success: function (result) {
            if (result.Resultado == '-1') {
                alert('Ha ocurrido un error:' + result.Mensaje);
            } else {
                alert("Se dió de baja el registro correctamente");
                mostrarListado();
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

///////////////////////////////////////////////////////////////////////////////////////////////////
/// Detalle
///////////////////////////////////////////////////////////////////////////////////////////////////

function det_actualizarTablaDetalle() {
    if (APP_OPCION != OPCION_VER) {
        $("#btnAgregarDetEquipo").show();
        $("#btnAgregarDetCircuito").show();
    }

    var listaDetalle = [];

    $(".detalle_circuito").hide();
    var dataDet = $("#hfHoDetalleJson").val();
    //var esActualizado = true;
    if (dataDet != null && dataDet != "") {
        listaDetalle = JSON.parse(dataDet);

        var html = det_generarTablaDetalle(listaDetalle, APP_OPCION);
        $("#tablaDetalleCircuito").html(html);
        $('.datepicker1').Zebra_DatePicker();

        $(".detalle_circuito").show();
    } else {
        var html = det_generarTablaDetalle([], APP_OPCION);
        $("#tablaDetalleCircuito").html(html);
        $('.datepicker1').Zebra_DatePicker();
        $(".detalle_circuito").show();
    }
}

function ObtenerFechaParaDatepicker(fecha) {
    var strDateTime = [[AddZero(fecha.getDate()),
        AddZero(fecha.getMonth() + 1),
        fecha.getFullYear()
    ].join("/")
    ].join(" ");
    return strDateTime;
}

function AddZero(num) {
    return (num >= 0 && num < 10) ? "0" + num : num + "";
}

function det_obtener_ObjForm() {
    var pos_row = parseInt($('#desg_form_fila').val()) || 0;

    var idequidet = parseInt($('#circ_det_cboEquipo').val()) || 0;
    var idcircdet = parseInt($('#circ_det_cboCircuitoDet').val()) || 0;
    var tipoDet = TIPO_CIRCUITO_DET_EQUIPO;
    var nombEqui = $("#circ_det_nomEquipo").val();
    var nombCirc = $("#circ_det_nomCircuito").val();
    var nombEmp = $("#circ_det_nomEmpresa").val();
    if (idequidet == 0) {
        tipoDet = TIPO_CIRCUITO_DET_CIRC;
        idequidet = null;
        nombEqui = nombCirc;
    } else {
        idcircdet = null;
    }

    var now = new Date();
    var strDateTime = ObtenerFechaParaDatepicker(now);

    var objDet =
    {
        TipoDet: tipoDet,
        Equicodihijo: idequidet,
        Circodihijo: idcircdet,
        Nombre: nombEqui,
        Empresa: nombEmp,
        UltimaModificacionUsuarioDesc: '',
        UltimaModificacionFechaDesc: '',
        Circdtagrup : 1,
        Circdtcodi: 0,
        FechaVigencia: strDateTime,
        Circdtestado: 1,
        Fila: pos_row
    };

    return objDet;
}

function det_generarTablaDetalle(listaDetalle, tipoOpcion) {
    //Generar Tabla html
    var html = '';    

    //if (listaDetalle.length > 0) {
    html += `
        <table class='pretty tabla-icono tabla_ho_det' id='tabla_detalle_circuito' style='width: 970px; margin-top: 5px; margin-bottom: 5px;'>
            <thead>
                <tr>`;
                if (OPCION_VER != tipoOpcion) {
                    html += "<th style='width: 45px;'></th>";
                }
    html += `
                    <th>Fecha Vigencia</th>
                    <th>Tipo</th>
                    <th>Empresa</th>
                    <th>Nombre</th>
                    <th>Nivel</th>
                    <th>Usuario</th>
                    <th>Fecha modificación</th>
                    <th>Estado</th>
                    `;

           

    html += `
                </tr>
            </thead>
            <tbody>
        `;

    var pos_row = 1;
    var fila = 1;
    var habilitado = false;
    var numRegistros = listaDetalle.length;

    listaDetalle.forEach(function (objDet) {
        

        //habilita combo si nuevocircuito y si es el ultimo
        if (NUEVOCIRCUITO == true) {
            if (numRegistros == fila) {
                habilitado = true;
            }            
        }
        var idFila = 'tr_det_' + pos_row;
        var idRegistro = objDet.Circdtcodi;
        var idEqui = objDet.Equicodihijo;
        var idCirc = objDet.Circodihijo;
        var descTipo = det_NombreTipoDetalle(objDet.TipoDet);
        var codigoTipo = objDet.TipoDet;
        var nombreDet = objDet.Nombre;
        var empresaDet = objDet.Empresa;
        var usuarioDet = objDet.UltimaModificacionUsuarioDesc;
        var fechaDet = objDet.UltimaModificacionFechaDesc;
        var nivelAgrupo = det_nivelAgrupacion(objDet.Circdtagrup, tipoOpcion, pos_row, habilitado);
        var fecVigencia = objDet.FechaVigencia;
        var state = objDet.Circdtestado;
        var estado = "Activo";
        if (state == 0){
            estado = "Inactivo";
        }


        fila = fila + 1;

        
        html += `
                <tr id="${idFila}"`;
        if (state == 0) {
            html += `style="pointer-events:none;" class="fila_inhabilitada">`;
        } else {
            html += `>`;
        }
        
        if (OPCION_VER != tipoOpcion && state == 1) {
            html += `
                    <td style='text-align: center'>
                        
                        <a href='JavaScript:eliminar_En_Circuito(${idRegistro},${pos_row})'>
                            <img src='${siteRoot}Content/Images/Trash.png' title='Eliminar fila' />
                        </a>
                        <a href='JavaScript:editar_Niveles(${pos_row})'>
                            <img src='${siteRoot}Content/Images/btn-edit.png' title='Editar' />
                        </a>
                    </td>
                    `;
        } else {
            html += `<td></td> `;
        }   

        if (OPCION_VER != tipoOpcion) {
            html += `<td><input type="text" class="datepicker1" name="fila_det_fechaVig" style="width:100px;"  data - zdp_direction="1" value = '${fecVigencia}' ></td>`;
        } else {
            html += `<td>${fecVigencia}</td>`;
        }
        html += ` 
                    <td>${descTipo}</td>
                    <td>${empresaDet}</td>
                    <td>${nombreDet}
                        <input type="hidden" class='fila_desg' name="fila_det_pos" style="width:77px;" value="${pos_row}"  />
                        <input type="hidden" class='fila_desg' name="fila_det_tipo" style="width:77px;" value="${codigoTipo}"  />
                        <input type="hidden" class='fila_desg' name="fila_det_id" style="width:77px;" value="${idRegistro}"  />
                        <input type="hidden" class='fila_desg' name="fila_det_id_equi" style="width:77px;" value="${idEqui}"  />
                        <input type="hidden" class='fila_desg' name="fila_det_id_circ" style="width:77px;" value="${idCirc}"  />
                        <input type="hidden" class='fila_desg' name="fila_det_desc_tipo" style="width:77px;" value="${descTipo}"  />
                        <input type="hidden" class='fila_desg' name="fila_det_nombre" style="width:77px;" value="${nombreDet}"  />
                        <input type="hidden" class='fila_desg' name="fila_det_empresa" style="width:77px;" value="${empresaDet}"  />
                        <input type="hidden" class='fila_desg' name="fila_det_usuario" style="width:77px;" value="${usuarioDet}"  />
                        <input type="hidden" class='fila_desg' name="fila_det_fecha" style="width:77px;" value="${fechaDet}"  />
                        
                        
                        <input type="hidden" id="stado" class='fila_desg' name="fila_det_estado" style="width:77px;" value="${state}"  />
                    </td>
                    <td>${nivelAgrupo}</td>
                    <td>${usuarioDet}</td>
                    <td>${fechaDet}</td>
                    <td>${estado}</td>
                `;
        
        
        html += `
                </tr>
            `;

        pos_row++;
    });

    html += `
            </tbody>
        </table>
        `;

    return html;
}

function det_nivelAgrupacion(nivel, tipoOpcion, pos_row, esActualizado) {
    //var disab = OPCION_VER != tipoOpcion ? '' : 'disabled';
    var disab = 'disabled';
    if (esActualizado) {
        disab = 'enabled';
    }
    var html = `
        <select id="combo_${pos_row}"name="fila_det_agrup" ${disab} style="width:43px;">
    `;
    for (var i = 1; i <= 15; i++) {
        var selected = i == nivel ? "selected" : "";
        html += `<option value="${i}" ${selected}>${i}</option>`;
    }
    html += `
        </select>
    `;

    return html;
}

function editar_Niveles(pos_row) {
    document.getElementById("combo_" + pos_row).disabled = false;
}


function ActualizarCamposRegistroInactivo(posicion, circuitoDet) {
    var estadoX = "#tr_det_" + posicion + " " + 'input[name=fila_det_estado]';
    var fechaModificacionX = "#tr_det_" + posicion + " " + 'input[name=fila_det_fecha]';
    var usuarioX = "#tr_det_" + posicion + " " + 'input[name=fila_det_usuario]';
    $(estadoX).val(circuitoDet.Circdtestado);
    $(fechaModificacionX).val(circuitoDet.UltimaModificacionFechaDesc);
    $(usuarioX).val(circuitoDet.Circdtusumodificacion);
}

function eliminar_En_Circuito(circdtcodi, pos) {
    if (confirm('¿Desea eliminar el registro?')) {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            traditional: true,
            url: controlador + 'EliminarEnCircuito',
            data: {
                circdtcodi: circdtcodi
            },
            cache: false,
            success: function (result) {
                if (result.Resultado == '-1') {
                    alert('Ha ocurrido un error:' + result.Mensaje);
                } else {
                    alert("Se eliminó correctamente el circuito");
                    ActualizarCamposRegistroInactivo(pos, result.CircuitoDet);                  
                    var listaDet = det_generarListaDetFromHtml(-1);
                    $("#hfHoDetalleJson").val(JSON.stringify(listaDet));
                    det_actualizarTablaDetalle();
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error.');
            }
        });
    }
}



function FiltrarFechasVigencia(fechaLimite, estados) {    
    var listaTotal = det_generarListaDetFromHtml(-1);
    $("#hfHoDetalleJson").val(JSON.stringify(listaTotal));
    det_actualizarTablaDetalle();

    //Aplicando filtros
    listaTotal.forEach(function (registro) {
        pos = registro.Fila;

        //filtro por fechaLimite
        var resultado = registro.FechaVigencia.localeCompare(fechaLimite);
        //si fechaVigencia > fechaLimite, OCULTO FILA
        if (resultado > 0) {
            var id = "#tr_det_" + pos;
            $(id).hide();
        } 

        //filtro por Estados
        if (estados == 1) { //piden solo Activos
            if (registro.Circdtestado == 0) {
                var id = "#tr_det_" + pos;
                $(id).hide();
            }
        }
    });
   
}


function det_quitarFila(pos_row) {
    $('#tabla_detalle_circuito #tr_det_' + pos_row).remove();

    var listaDesg = det_generarListaDetFromHtml(pos_row);
    $("#hfHoDetalleJson").val(JSON.stringify(listaDesg));
}

function det_guardarDet() {
    var msj = '';
    var objDet = det_obtener_ObjForm();

    if (TIPO_POPUP_EQUIPO == POPUP_EQUIPO_DET && objDet.Equicodihijo == null) {
        msj += 'Debe seleccionar equipo.';
    }
    if (TIPO_POPUP_EQUIPO == POPUP_EQUIPO_CIRC && objDet.Circodihijo == null) {
        msj += 'Debe seleccionar circuito.';
    }

    if (msj.length > 0) {
        alert(msj)
        return false;
    }

    var listaDet = det_generarListaDetFromHtml(objDet.Fila);
    listaDet.push(objDet);

    //Validación
    $("#hfHoDetalleJson").val(JSON.stringify(listaDet));
    NUEVOCIRCUITO = true;
    det_actualizarTablaDetalle();
    NUEVOCIRCUITO = false;
    return true;
}

function det_generarListaDetFromHtml(pos_row_omitir) {
    var listaDet = [];
    pos_row_omitir = parseInt(pos_row_omitir) || 0;

    $('#tabla_detalle_circuito tbody').find('tr').each(function () {
        var idFila = " #" + $(this).get()[0].id;

        var id_pos_row = idFila + " " + 'input[name=fila_det_pos]';
        var id_tipo = idFila + " " + 'input[name=fila_det_tipo]';
        var id = idFila + " " + 'input[name=fila_det_id]';
        var id_equi = idFila + " " + 'input[name=fila_det_id_equi]';
        var id_circ = idFila + " " + 'input[name=fila_det_id_circ]';
        var nombEqui = idFila + " " + 'input[name=fila_det_nombre]';
        var desc_tipo = idFila + " " + 'input[name=fila_det_desc_tipo]';
        var empresaDet = idFila + " " + 'input[name=fila_det_empresa]';
        var usuarioDet = idFila + " " + 'input[name=fila_det_usuario]';
        var fechaDet = idFila + " " + 'input[name=fila_det_fecha]';
        var nivelAgrup = idFila + " " + 'select[name=fila_det_agrup]';
        var fechitaV = idFila + " " + 'input[name=fila_det_fechaVig]';
              
        var state = idFila + " " + 'input[name=fila_det_estado]';
        
        var pos_row = parseInt($(id_pos_row).val()) || 0;
        id_tipo = parseInt($(id_tipo).val()) || 0;
        id = parseInt($(id).val()) || 0;
        id_equi = parseInt($(id_equi).val()) || 0;
        id_circ = parseInt($(id_circ).val()) || 0;
        nombEqui = $(nombEqui).val();
        desc_tipo = $(desc_tipo).val();
        empresaDet = $(empresaDet).val();
        usuarioDet = $(usuarioDet).val();
        fechaDet = $(fechaDet).val();
        fechitaV = $(fechitaV).val();
        
        state = $(state).val();
        nivelAgrup = parseInt($(nivelAgrup).val()) || 0;

        var objDet =
        {
            TipoDet: id_tipo,
            Equicodihijo: id_equi,
            Circodihijo: id_circ,
            Circdtagrup: nivelAgrup, 
            Nombre: nombEqui,
            Empresa: empresaDet,
            UltimaModificacionUsuarioDesc: usuarioDet,
            UltimaModificacionFechaDesc: fechaDet,
            Circdtcodi: id,
            FechaVigencia: fechitaV,
            Circdtestado: state,
            Fila: pos_row
        };

        if (pos_row_omitir > 0 && pos_row == pos_row_omitir) { }
        else
            listaDet.push(objDet);
    });

    return listaDet;
}

function det_NombreTipoDetalle(tipo) {
    var tipoDet = '';
    if (tipo == TIPO_CIRCUITO_DET_EQUIPO)
        tipoDet = 'Equipo';
    if (tipo == TIPO_CIRCUITO_DET_CIRC)
        tipoDet = 'Circuito';

    return tipoDet;
}

///////////////////////////
/// Búsqueda Equipo
///////////////////////////
function visualizarBuscarEquipo() {

    if (APP_MOSTRAR_MODAL_EQUIPO_YA_CARGADO == 0) {
        cargarBusquedaEquipo(APP_MOSTRAR_MODAL_EQUIPO_YA_CARGADO);
    } else {
        openBusquedaEquipo();
    }

    APP_MOSTRAR_MODAL_EQUIPO_YA_CARGADO++;
}

function cargarBusquedaEquipo(flag) {
    $.ajax({
        type: "POST",
        url: controlador + "BusquedaEquipo",
        data: {
            filtroFamilia: -1
        },
        global: false,
        success: function (evt) {
            $('#busquedaEquipo').html(evt);
            openBusquedaEquipo();
        },
        error: function (req, status, error) {
            alert('Ha ocurrido un error.');
        }
    });
}

function openBusquedaEquipo() {
    if (TIPO_POPUP_EQUIPO == POPUP_EQUIPO_DET) {
        $(".columna_agregar.circuito").show();
    } else {
        $(".columna_agregar.circuito").hide();
    }

    $('#busquedaEquipo').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
    $('#txtFiltro').focus();
}

function seleccionarCircuitoDet(tipoSeleccion, circodi, nombreCircuito, equicodi, equinomb, Areanomb, Emprnomb, Famabrev, emprcodi, areacodi) {
    if (TIPO_POPUP_EQUIPO == POPUP_EQUIPO) {
        $("#cboEquipo").val('');
        $("#nomEquipo").val('');
        $("#cboEmpresa").val('');
        $("#nomEmpresa").val('');
        $("#nomUbicacion").val('');
        $("#famAbrev").val('');
        //$('#cboUbicacion').val();

        $("#nomEmpresa").val(Emprnomb);
        $("#cboEmpresa").val(emprcodi);
        $("#sEmprcodi").val("[" + emprcodi + "]");

        $("#famAbrev").val(Famabrev);

        $("#nomEquipo").val(equinomb);
        $("#cboEquipo").val(equicodi);

        $("#nomUbicacion").val(Areanomb);
        $("#cboUbicacion").val(areacodi);

        //validacion del nombre del circuito
        var textNombCircuito = $("#nomCircuito").val();
        if (textNombCircuito !== undefined && textNombCircuito != null && textNombCircuito.trim() != '') { } else {
            $("#nomCircuito").val(equinomb);
        }

        $('#busquedaEquipo').bPopup().close();
    } else {
        $("#circ_det_cboEquipo").val('');
        $("#circ_det_cboCircuitoDet").val('');
        $("#circ_det_nomUbicacion").val('');
        $("#circ_det_nomEquipo").val('');
        $("#circ_det_nomCircuito").val('');
        $("#circ_det_cboEmpresa").val('');
        $("#circ_det_nomEmpresa").val('');
        $("#circ_det_famAbrev").val('');

        if (tipoSeleccion == 2) {
            TIPO_POPUP_EQUIPO = POPUP_EQUIPO_DET;

            $("#circ_det_nomEmpresa").val(Emprnomb);
            $("#circ_det_cboEmpresa").val(emprcodi);
            $("#circ_det_sEmprcodi").val("[" + emprcodi + "]");
            $("#circ_det_famAbrev").val(Famabrev);
            $("#circ_det_nomEquipo").val(equinomb);
            $("#circ_det_cboEquipo").val(equicodi);
            $("#circ_det_nomUbicacion").val(Areanomb);
            $("#circ_det_cboUbicacion").val(areacodi);

            var res = det_guardarDet();
            if (res)
                $('#busquedaEquipo').bPopup().close();
        }

        if (tipoSeleccion == 3) {
            TIPO_POPUP_EQUIPO = POPUP_EQUIPO_CIRC;

            $("#circ_det_nomEmpresa").val(Emprnomb);
            $("#circ_det_cboEmpresa").val(emprcodi);
            $("#circ_det_sEmprcodi").val("[" + emprcodi + "]");
            $("#circ_det_famAbrev").val(Famabrev);
            $("#circ_det_nomEquipo").val(equinomb);
            $("#circ_det_nomCircuito").val(nombreCircuito);
            $("#circ_det_cboCircuitoDet").val(circodi);
            $("#circ_det_nomUbicacion").val(Areanomb);
            $("#circ_det_cboUbicacion").val(areacodi);

            var res = det_guardarDet();
            if (res)
                $('#busquedaEquipo').bPopup().close();
        }

    }
}

function mostrarError(err) {
    console.log(err);
}