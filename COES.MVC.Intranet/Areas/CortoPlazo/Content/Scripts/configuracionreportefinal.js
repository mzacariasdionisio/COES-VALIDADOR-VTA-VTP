var controlador = siteRoot + 'cortoplazo/reportefinal/';

$(function () {
    $('#tab-container').easytabs({
        animate: false
    });

    $('#txtFecha').Zebra_DatePicker({
      
    });

   
    $('#btnConsultar').on('click', function () {
        consultar();
    });

    $('.btn-agregar').on('click', function () {
        editar(0, $(this).attr('data-id'));
    });

    $('#btnPeriodo').on('click', function () {
        editarPeriodo(0);
    });

    $('#btnUmbral').on('click', function () {
        editarUmbral(0);
    });

    $('#btnEquipo').on('click', function () {
        editarEquipo(0);
    });

    consultar();
});

consultar = function () {
    if ($('#txtFecha').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'obtenerconfiguracion',
            data: {
                fecha: $('#txtFecha').val()
            },
            dataType: 'json',
            success: function (result) {
                pintarTablas(result);
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }
    else {      
        mostrarMensaje('mensaje', 'alert', 'Por favor seleccione una fecha.');
    }
};

pintarTablas = function (result) {

    //****** Tabla de barras desconocidas ******//
    var html = '<table class="pretty tabla-adicional" id="tablaBarraDesconocida">';
    html = html + ' <thead>';
    html = html + '     <tr>';
    html = html + '         <th>Acciones</th>';
    html = html + '         <th>Barra de Transferencia</th>';
    html = html + '         <th>Vigencia a partir de</th>';
    html = html + '         <th>Usuario Mod.</th>';
    html = html + '         <th>Fecha Mod.</th>'; 
    html = html + '     </tr>';
    html = html + ' </thead>';
    html = html + ' <tbody>';
  
    for (var i in result.ListaBarraDesconocida) {        
        html = html + '     <tr>';
        html = html + '         <td>';
        html = html + '             <a href="JavaScript:editar(' + result.ListaBarraDesconocida[i].Cmbarecodi + ', 0)"><img src="' + siteRoot + 'content/images/btn-edit.png" /></a>';
        html = html + '             <a href="JavaScript:eliminar(' + result.ListaBarraDesconocida[i].Cmbarecodi  + ')"><img src="' + siteRoot + 'content/images/btn-cancel.png" /></a>';
        html = html + '             <a href="JavaScript:verHistorico(' + result.ListaBarraDesconocida[i].Barrcodi  + ', 0)"><img src="' + siteRoot + 'content/images/btn-properties.png" /></a>';
        html = html + '         </td>';
        html = html + '         <td>' + result.ListaBarraDesconocida[i].Barrnombre + '</td>';
        html = html + '         <td>' + result.ListaBarraDesconocida[i].Vigencia + '</td>';
        html = html + '         <td>' + result.ListaBarraDesconocida[i].Cmbareusumodificacion + '</td>';
        html = html + '         <td>' + result.ListaBarraDesconocida[i].Modificacion + '</td>';
        html = html + '     </tr>';       
    }

    html = html + ' </tbody>';
    html = html + '</table>';

    $('#cntBarraDesconocida').html(html);

    $('#tablaBarraDesconocida').dataTable({
        "iDisplayLength": 100
    });

     //****** Tabla de barras EMS ******//

    html = '';
    html = '<table class="pretty tabla-adicional" id="tablaBarrasEMS">';
    html = html + ' <thead>';
    html = html + '     <tr>';
    html = html + '         <th style="width:80px">Acciones</th>';
    html = html + '         <th>Barra de Transferencia</th>';
    html = html + '         <th>Barra EMS</th>';
    html = html + '         <th>Tipo Relación</th>';
    html = html + '         <th>Vigencia a partir de</th>';
    html = html + '         <th>Reporte</th>';
    html = html + '         <th>Usuario Mod.</th>';
    html = html + '         <th>Fecha Mod.</th>';
    html = html + '     </tr>';
    html = html + ' </thead>';
    html = html + ' <tbody>';

    for (var i in result.ListaBarraEMS) {

        var txtReporte = "No";
        if (result.ListaBarraEMS[i].Cmbarereporte == "S") txtReporte = "Si";

        html = html + '     <tr>';
        html = html + '         <td style="width:80px">';
        html = html + '             <a href="JavaScript:editar(' + result.ListaBarraEMS[i].Cmbarecodi + ', 1)"><img src="' + siteRoot + 'content/images/btn-edit.png" /></a>';
        html = html + '             <a href="JavaScript:eliminar(' + result.ListaBarraEMS[i].Cmbarecodi + ')"><img src="' + siteRoot + 'content/images/btn-cancel.png" /></a>';
        html = html + '             <a href="JavaScript:verHistorico(' + result.ListaBarraEMS[i].Barrcodi + ', 1)"><img src="' + siteRoot + 'content/images/btn-properties.png" /></a>';
        html = html + '             <a href="JavaScript:agregarBarraEMS(' + result.ListaBarraEMS[i].Cmbarecodi + ', ' + result.ListaBarraEMS[i].Cnfbarcodi + ', 1)"><img src="' + siteRoot + 'content/images/btn-add.png" /></a>';
        html = html + '         </td>';
        html = html + '         <td>' + result.ListaBarraEMS[i].Barrnombre + '</td>';
        html = html + '         <td>' + result.ListaBarraEMS[i].Cnfbarnombre + '</td>';
        html = html + '         <td>' + result.ListaBarraEMS[i].TipoRelacion + '</td>';
        html = html + '         <td>' + result.ListaBarraEMS[i].Vigencia + '</td>';
        html = html + '         <td>' + txtReporte + '</td>';
        html = html + '         <td>' + result.ListaBarraEMS[i].Cmbareusumodificacion + '</td>';
        html = html + '         <td>' + result.ListaBarraEMS[i].Modificacion + '</td>';
        html = html + '     </tr>';
    }

    html = html + ' </tbody>';
    html = html + '</table>';

    $('#cntBarraEMS').html(html);

    $('#tablaBarrasEMS').dataTable({
        "iDisplayLength": 100
    });

    //******* Barras de Transferencias ******//
    html = '';
    html = '<table class="pretty tabla-adicional" id="tablaTransferencias">';
    html = html + ' <thead>';
    html = html + '     <tr>';
    html = html + '         <th>Acciones</th>';
    html = html + '         <th>Barra de Transferencia 01</th>';
    html = html + '         <th>Barra de Transferencia 02</th>';   
    html = html + '         <th>Vigencia a partir de</th>';
    html = html + '         <th>Usuario Mod.</th>';
    html = html + '         <th>Fecha Mod.</th>';
    html = html + '     </tr>';
    html = html + ' </thead>';
    html = html + ' <tbody>';

    for (var i in result.ListaBarraTransferencia) {
        html = html + '     <tr>';
        html = html + '         <td>';
        html = html + '             <a href="JavaScript:editar(' + result.ListaBarraTransferencia[i].Cmbarecodi + ', 2)"><img src="' + siteRoot + 'content/images/btn-edit.png" /></a>';
        html = html + '             <a href="JavaScript:eliminar(' + result.ListaBarraTransferencia[i].Cmbarecodi + ')"><img src="' + siteRoot + 'content/images/btn-cancel.png" /></a>';
        html = html + '             <a href="JavaScript:verHistorico(' + result.ListaBarraTransferencia[i].Barrcodi + ', 2)"><img src="' + siteRoot + 'content/images/btn-properties.png" /></a>';
        html = html + '         </td>';
        html = html + '         <td>' + result.ListaBarraTransferencia[i].Barrnombre + '</td>';
        html = html + '         <td>' + result.ListaBarraTransferencia[i].Barrnombre2 + '</td>';
        html = html + '         <td>' + result.ListaBarraTransferencia[i].Vigencia + '</td>';
        html = html + '         <td>' + result.ListaBarraTransferencia[i].Cmbareusumodificacion + '</td>';
        html = html + '         <td>' + result.ListaBarraTransferencia[i].Modificacion + '</td>';
        html = html + '     </tr>';
    }

    html = html + ' </tbody>';
    html = html + '</table>';

    $('#cntBarraTransferencia').html(html);

    $('#tablaTransferencias').dataTable({
        "iDisplayLength": 100
    });

    //**********Relación equipos y barras***********//
    html = '';
    html = '<table class="pretty tabla-adicional" id="tablaEquipos">';
    html = html + ' <thead>';
    html = html + '     <tr>';
    html = html + '         <th>Acciones</th>';
    html = html + '         <th>Equipo Congestión</th>';
    html = html + '         <th>Barra de Transferencia Adicional</th>';
    html = html + '         <th>Vigencia a partir de</th>';
    html = html + '         <th>Usuario Mod.</th>';
    html = html + '         <th>Fecha Mod.</th>';
    html = html + '     </tr>';
    html = html + ' </thead>';
    html = html + ' <tbody>';

    for (var i in result.ListaRelacionEquipo) {

        var count = 1;
        if (result.ListaRelacionEquipo[i].ListaDetalle.length > 0)
            count = result.ListaRelacionEquipo[i].ListaDetalle.length;

        html = html + '     <tr>';
        html = html + '         <td rowspan="' + count + '">';
        html = html + '             <a href="JavaScript:editarEquipo(' + result.ListaRelacionEquipo[i].Cmeqbacodi + ')"><img src="' + siteRoot + 'content/images/btn-edit.png" /></a>';
        html = html + '             <a href="JavaScript:eliminarEquipo(' + result.ListaRelacionEquipo[i].Cmeqbacodi + ')"><img src="' + siteRoot + 'content/images/btn-cancel.png" /></a>';
        html = html + '             <a href="JavaScript:verHistoricoEquipo(' + result.ListaRelacionEquipo[i].Configcodi + ')"><img src="' + siteRoot + 'content/images/btn-properties.png" /></a>';
        html = html + '         </td>';
        html = html + '         <td rowspan="' + count + '">' + result.ListaRelacionEquipo[i].Equinomb + '</td>';

        if (result.ListaRelacionEquipo[i].ListaDetalle.length > 0) {
            html = html + '         <td>' + result.ListaRelacionEquipo[i].ListaDetalle[0].Barrnombre + '</td>';
        }
        
        html = html + '         <td rowspan="' + count + '">' + result.ListaRelacionEquipo[i].Vigencia + '</td>';
        html = html + '         <td rowspan="' + count + '">' + result.ListaRelacionEquipo[i].Cmeqbausumodificacion + '</td>';
        html = html + '         <td rowspan="' + count + '">' + result.ListaRelacionEquipo[i].Modificacion + '</td>';
        html = html + '     </tr>';

        if (result.ListaRelacionEquipo[i].ListaDetalle.length > 1) {
            for (var j = 1; j < result.ListaRelacionEquipo[i].ListaDetalle.length; j++) {
                html = html + '     <tr>';
                html = html + '         <td>' + result.ListaRelacionEquipo[i].ListaDetalle[j].Barrnombre + '</td>';
                html = html + '     </tr>';
            }
        }
    }

    html = html + ' </tbody>';
    html = html + '</table>';

    $('#cntBarraEquipo').html(html);

 


    //**********Periodos***********//

    html = '';
    html = '<table class="pretty tabla-adicional" id="tablaPeriodo">';
    html = html + ' <thead>';
    html = html + '     <tr>';
    html = html + '         <th rowspan="2">Acciones</th>';
    html = html + '         <th colspan="3">Periodo</th>';
    html = html + '         <th rowspan="2">Vigencia a partir de</th>';
    html = html + '         <th rowspan="2">Usuario Mod.</th>';
    html = html + '         <th rowspan="2">Fecha Mod.</th>';
    html = html + '     </tr>';
    html = html + '     <tr>';
    html = html + '         <th>Base</th>';
    html = html + '         <th>Media</th>';
    html = html + '         <th>Punta</th>';
    html = html + '     </tr>';
    html = html + ' </thead>';
    html = html + ' <tbody>';

    for (var i in result.ListaPeriodo) {
        html = html + '     <tr>';
        html = html + '         <td>';
        html = html + '             <a href="JavaScript:editarPeriodo(' + result.ListaPeriodo[i].Cmpercodi + ')"><img src="' + siteRoot + 'content/images/btn-edit.png" /></a>';
        html = html + '             <a href="JavaScript:eliminarPeriodo(' + result.ListaPeriodo[i].Cmpercodi + ')"><img src="' + siteRoot + 'content/images/btn-cancel.png" /></a>';
        html = html + '             <a href="JavaScript:verHistoricoPeriodo()"><img src="' + siteRoot + 'content/images/btn-properties.png" /></a>';
        html = html + '         </td>';
        html = html + '         <td>' + result.ListaPeriodo[i].Cmperbase + '</td>';
        html = html + '         <td>' + result.ListaPeriodo[i].Cmpermedia + '</td>';
        html = html + '         <td>' + result.ListaPeriodo[i].Cmperpunta + '</td>';
        html = html + '         <td>' + result.ListaPeriodo[i].Vigencia + '</td>';
        html = html + '         <td>' + result.ListaPeriodo[i].Cmperusumodificacion + '</td>';
        html = html + '         <td>' + result.ListaPeriodo[i].Modificacion + '</td>';
        html = html + '     </tr>';
    }

    html = html + ' </tbody>';
    html = html + '</table>';

    $('#cntPeriodo').html(html);

    $('#tablaPeriodo').dataTable({
        "iDisplayLength": 100
    });

    //**********Umbrales***********//

    html = '';
    html = '<table class="pretty tabla-adicional" id="tablaUmbral">';
    html = html + ' <thead>';
    html = html + '     <tr>';
    html = html + '         <th rowspan="2">Acciones</th>';
    html = html + '         <th colspan="7">Umbrales</th>';
    html = html + '         <th rowspan="2">Vigencia a partir de</th>';
    html = html + '         <th rowspan="2">Usuario Mod.</th>';
    html = html + '         <th rowspan="2">Fecha Mod.</th>';
    html = html + '     </tr>';
    html = html + '     <tr>';
    html = html + '         <th>Max. Total</th>';
    html = html + '         <th>Min. Total</th>';
    html = html + '         <th>Max. Energía</th>';
    html = html + '         <th>Min. Energía</th>';
    html = html + '         <th>Max. Congestión</th>';
    html = html + '         <th>Min. Congestión</th>';
    html = html + '         <th>Diferencia</th>';
    html = html + '     </tr>';
    html = html + ' </thead>';
    html = html + ' <tbody>';

    for (var i in result.ListaUmbral) {
        html = html + '     <tr>';
        html = html + '         <td>';
        html = html + '             <a href="JavaScript:editarUmbral(' + result.ListaUmbral[i].Cmurcodi + ')"><img src="' + siteRoot + 'content/images/btn-edit.png" /></a>';
        html = html + '             <a href="JavaScript:eliminarUmbral(' + result.ListaUmbral[i].Cmurcodi + ')"><img src="' + siteRoot + 'content/images/btn-cancel.png" /></a>';
        html = html + '             <a href="JavaScript:verHistoricoUmbral()"><img src="' + siteRoot + 'content/images/btn-properties.png" /></a>';
        html = html + '         </td>';
        html = html + '         <td>' + result.ListaUmbral[i].Cmurmaxbarra + '</td>';
        html = html + '         <td>' + result.ListaUmbral[i].Cmurminbarra + '</td>';
        html = html + '         <td>' + result.ListaUmbral[i].Cmurmaxenergia + '</td>';
        html = html + '         <td>' + result.ListaUmbral[i].Cmurminenergia + '</td>';
        html = html + '         <td>' + result.ListaUmbral[i].Cmurmaxconges + '</td>';
        html = html + '         <td>' + result.ListaUmbral[i].Cmurminconges + '</td>';
        html = html + '         <td>' + result.ListaUmbral[i].Cmurdiferencia + '</td>';
        html = html + '         <td>' + result.ListaUmbral[i].Vigencia + '</td>';
        html = html + '         <td>' + result.ListaUmbral[i].Cmurusumodificacion + '</td>';
        html = html + '         <td>' + result.ListaUmbral[i].Modificacion + '</td>';
        html = html + '     </tr>';
    }

    html = html + ' </tbody>';
    html = html + '</table>';

    $('#cntUmbral').html(html);

    $('#tablaUmbral').dataTable({
        "iDisplayLength": 100
    });

};

editar = function (id, tipo) {
    $.ajax({
        type: 'POST',
        url: controlador + 'configuracionbarra',
        data: {
            idConfiguracion: id,
            tipo: tipo,
            fecha: $('#txtFecha').val()
        },
        success: function (evt) {
            $('#contenidoBarra').html(evt);

            setTimeout(function () {
                $('#popupBarra').bPopup({
                    autoClose: false
                });
            }, 50);

            $('#txtFechaVigencia').Zebra_DatePicker();            
            $('#txtFechaExpiracion').Zebra_DatePicker();

            $('#cbTipoRelacion').val($('#hfTipoRelacion').val());
            $('#cbBarraTransferencia').val($('#hfBarraTransferencia').val());
            $('#cbBarraDesconocida').val($('#hfBarraDesconocida').val());
            $('#cbBarraTransferencia2').val($('#hfBarraTransferencia2').val());
            $('#cbBarraEMS').val($('#hfBarraEMS').val());
            $('#cbBarraEMS2').val($('#hfBarraEMS2').val());
            $('#cbMostrarReporte').val($('#hfMostrarReporte').val());

            var tipoRegistro = $('#hfTipoRegistro').val();

            $('#trTipoRelacion').hide();
            $('#trBarraTransferencia').hide();
            $('#trBarraDesconocida').hide();
            $('#trBarraTransferencia2').hide();
            $('#trBarraEMS').hide();
            $('#trBarraEMS2').hide();
            $('#spanBarraEMS').text('Barra EMS ');
            $('#trMostrarReporte').hide();

            $('#cbTipoRelacion').on('change', function () {
                cargarRelacionBarra($('#cbTipoRelacion').val());
            });

            if (tipoRegistro == "0") {
                $('#trBarraTransferencia').show();
            }
            else if (tipoRegistro == "1") {
                $('#trTipoRelacion').show();
                $('#trBarraEMS').show();
                $('#trMostrarReporte').show();
                cargarRelacionBarra($('#hfTipoRelacion').val());

            }
            else if (tipoRegistro == "2") {
                $('#trBarraTransferencia').show();
                $('#trBarraTransferencia2').show();
            }
            else if (tipoRegistro == "3") {
                $('#trBarraTransferencia').show();
                $('#spanBarraEMS').text('Barra EMS (Conocida)');
                $('#trBarraEMS').show();
                $('#trBarraEMS2').show();
            }

            $('#btnGrabar').on('click', function () {
                grabarBarra();
            });

            $('#btnCancelar').on('click', function () {
                $('#popupBarra').bPopup().close();
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }

    });
};

verHistorico = function (idBarra, tipo) {

    $.ajax({
        type: 'POST',
        url: controlador + 'historicobarra',
        data: {
            idBarra: idBarra,
            tipoRegistro: tipo            
        },
        success: function (evt) {
            $('#contenidoHistoricoBarra').html(evt);

            setTimeout(function () {
                $('#popupHistoricoBarra').bPopup({
                    autoClose: false
                });
            }, 50);
            
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }

    });
};

agregarBarraEMS = function (id, idBarra) {
    $.ajax({
        type: 'POST',
        url: controlador + 'addbarraems',
        data: {
            idRelacion: id,
            idBarra: idBarra
        },
        success: function (evt) {
            $('#contenidoBarraEMS').html(evt);

            setTimeout(function () {
                $('#popupBarraEMS').bPopup({
                    autoClose: false
                });
            }, 50);
                       
            $('#btnAgregarBarraEMSAdicional').on('click', function () {
                mostrarMensaje('mensajeEditBarraEMSAdicional', 'message', 'Por favor ingrese los datos.');
                if ($('#cbBarraEMSAdicional').val() != "") {
                    agregarBarraEMSAdicional($('#cbBarraEMSAdicional').val(), $("#cbBarraEMSAdicional option:selected").text());
                }
                else {
                    mostrarMensaje('mensajeEditBarraEMSAdicional', 'alert', 'Debe seleccionar una barra');
                }
            });

            $('#btnGrabarBarraEMSAdicional').on('click', function () {
                grabarBarraEMSAdicional();
            });

            $('#btnCancelarBarraEMSAdicional').on('click', function () {
                $('#popupBarraEMS').bPopup().close();
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }

    });
}

agregarBarraEMSAdicional = function (barraCodi, barraNomb) {
    var count = 0;
    var flag = true;
    $('#tablaBarraEMSAdicional>tbody tr').each(function (i) {
        $punto = $(this).find('#hfBarraEMSItem');
        if ($punto.val() == barraCodi) {
            flag = false;
        }
    });

    if (flag) {

        if (barraCodi != $('#hfIdBarraEMSPrincipal').val()) {

            $('#tablaBarraEMSAdicional> tbody').append(
                '<tr>' +
                '   <td style="text-align:center">' +
                '       <input type="hidden" id="hfBarraEMSItem" value="' + barraCodi + '" /> ' +
                '       <img src="' + siteRoot + 'Content/Images/btn-cancel.png" alt="" onclick="$(this).parent().parent().remove();" style="cursor:pointer" />' +
                '   </td>' +
                '   <td>' + barraNomb + '</td>' +
                '</tr>'
            );
        }
        else {
            mostrarMensaje('mensajeEditBarraEMSAdicional', 'alert', 'No puede agregar nuevamente la barra EMS principal.');
        }
    }
    else {
        mostrarMensaje('mensajeEditBarraEMSAdicional', 'alert', 'La barra ya se encuentra agregada');
    }
};

grabarBarraEMSAdicional = function () {
    var validacion = validarBarraEMSAdicional();

    if (validacion == "") {

        var count = 0;
        var items = "";
        $('#tablaBarraEMSAdicional>tbody tr').each(function (i) {
            $punto = $(this).find('#hfBarraEMSItem');
            var constante = (count > 0) ? "," : "";
            items = items + constante + $punto.val();
            count++;
        });

        $('#hfListaBarrasEMSAdicional').val(items);

        if (confirm('¿Está seguro de realizar esta operación?')) {
            $.ajax({
                type: 'POST',
                url: controlador + 'grabarbarraemsadicional',
                data: $('#frmRegistroBarraEMSAdicional').serialize(),
                dataType: 'json',
                success: function (result) {
                    if (result.Resultado == 1) {
                        mostrarMensaje('mensaje', 'exito', result.Mensaje);
                        $('#popupBarraEMS').bPopup().close();
                        //consultar();
                    }                   
                    else  {
                        mostrarMensaje('mensajeEditBarraEMSAdicional', 'error', 'Ha ocurrido un error.');
                    }
                },
                error: function () {
                    mostrarMensaje('mensajeEditBarraEMSAdicional', 'error', 'Ha ocurrido un error.');
                }
            });
        }
    }
    else {
        mostrarMensaje('mensajeEditBarraEMSAdicional', 'alert', validacion);
    }
};

validarBarraEMSAdicional = function () {

    var html = "<ul>";
    var flag = true;
        
    var count = $('#tablaBarraEMSAdicional >tbody >tr').length;

    if (count == 0) {
        mensaje = mensaje + "<li>Seleccione barras EMS adicionales.</li>";
        flag = false;
    }

    html = html + "</ul>";

    flag = true;

    if (flag) {
        html = "";
    }

    return html;
};

eliminar = function (id) {
    if (confirm('¿Está seguro de realizar esta operación?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'eliminarbarra',
            data: {
                id: id
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensaje', 'exito', 'La operación se realizó correctamente.');                    
                    consultar();
                }               
                else if (result == -1) {
                    mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }
};

grabarBarra = function () {

    var validacion = validarBarra();

    if (validacion == "") {

        if (confirm('¿Está seguro de realizar esta operación?')) {
            $.ajax({
                type: 'POST',
                url: controlador + 'grabarbarra',
                data: $('#frmRegistro').serialize(),
                dataType: 'json',
                success: function (result) {
                    if (result.Resultado == 1) {
                        mostrarMensaje('mensaje', 'exito', result.Mensaje);
                        $('#popupBarra').bPopup().close();
                        consultar();
                    }
                    else if (result.Resultado == 2) {
                        mostrarMensaje('mensajeEdit', 'alert', result.Mensaje);
                    }
                    else if (result.Resultado == -1) {
                        mostrarMensaje('mensajeEdit', 'error', 'Ha ocurrido un error.');
                    }
                },
                error: function () {
                    mostrarMensaje('mensajeEdit', 'error', 'Ha ocurrido un error.');
                }

            });
        }
    }
    else {
        mostrarMensaje('mensajeEdit', 'alert', validacion);
    }
};

validarBarra = function () {
    var tipoRegistro = $('#hfTipoRegistro').val();
    var html = "<ul>";
    var flag = true;

    if ($('#txtFechaVigencia').val() == "") {
        html = html + "<li>Seleccione fecha de vigencia</li>";
        flag = false;
    }

    if (tipoRegistro == "0") {
        if ($('#cbBarraTransferencia').val() == "-1") {
            html = html + "<li>Seleccione barra de transferencia</li>";
            flag = false;
        }
    }
    else if (tipoRegistro == "1") {
        if ($('#cbTipoRelacion').val() == "") {
            html = html + "<li>Seleccione tipo de relación</li>";
            flag = false;
        }
        else {
            var tipoRelacion = $('#cbTipoRelacion').val();

            if (tipoRelacion == "0") {
                if ($('#cbBarraTransferencia').val() == "-1") {
                    html = html + "<li>Seleccione barra de transferencia</li>";
                    flag = false;
                }
                
            }
            else if (tipoRelacion == "1") {
                if ($('#cbBarraDesconocida').val() == "-1") {
                    html = html + "<li>Seleccione barra desconocida</li>";
                    flag = false;
                }
              
            }
            else if (tipoRelacion == "2") {
                if ($('#cbBarraTransferencia').val() == "-1") {
                    html = html + "<li>Seleccione barra adicional</li>";
                    flag = false;
                }
            }
            else if (tipoRelacion == "3") {
                if ($('#cbBarraTransferencia').val() == "-1") {
                    html = html + "<li>Seleccione barra de transferencia</li>";
                    flag = false;
                }

            }
        }

        if ($('#cbBarraEMS').val() == "-1") {
            html = html + "<li>Seleccione barra EMS</li>";
            flag = false;
        }

        if ($('#cbBarraEMS2').val() == "-1" && tipoRelacion == "3") {
            html = html + "<li>Seleccione barra EMS para el tipo desconocido</li>";
            flag = false;
        }

        if ($('#cbMostrarReporte').val() == "") {
            html = html + "<li>Debe indicar si la barra se mostrará en el reporte</li>";
            flag = false;
        }

    }
    else if (tipoRegistro == "2") {
        if ($('#cbBarraTransferencia').val() == "-1") {
            html = html + "<li>Seleccione barra de transferencia 1</li>";
            flag = false;
        }

        if ($('#cbBarraTransferencia2').val() == "-1") {
            html = html + "<li>Seleccione barra de transferencia 2</li>";
            flag = false;
        }

        if ($('#cbBarraTransferencia').val() != "-1" && $('#cbBarraTransferencia2').val() != "-1") {
            if ($('#cbBarraTransferencia').val() == $('#cbBarraTransferencia2').val() ) {
                html = html + "<li>La barra 1 debe ser distinta a la barra 2</li>";
                flag = false;
            }
        }
    }


    html = html + "</ul>";

    if (flag) {
        html = "";
    }

    return html;
};

cargarRelacionBarra = function (tipo) {

    $('#trBarraTransferencia').hide();
    $('#trBarraDesconocida').hide();
    $('#trBarraEMS2').hide();
    $('#spanBarraEMS').text('Barra EMS ');
   

    if (tipo == "0") {
        $('#trBarraTransferencia').show();      
    }
    else if (tipo == "1") {
        $('#trBarraDesconocida').show();       
    }
    else if (tipo == "2") {
        $('#trBarraTransferencia').show();      
    }
    else if (tipo == "3") {
        $('#spanBarraEMS').text('Barra EMS (Conocida)');
        $('#trBarraEMS').show();
        $('#trBarraEMS2').show();
        $('#trBarraTransferencia').show();
    }
};

editarPeriodo = function (id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'configuracionperiodo',
        data: {
            idConfiguracion: id
        },
        success: function (evt) {
            $('#contenidoPeriodo').html(evt);

            setTimeout(function () {
                $('#popupPeriodo').bPopup({
                    autoClose: false
                });
            }, 50);

            $('#txtFechaVigenciaPeriodo').Zebra_DatePicker();
            $('#txtFechaExpiracionPeriodo').Zebra_DatePicker();

            $('#btnGrabarPeriodo').on('click', function () {
                grabarPeriodo();
            });

            $('#btnCancelarPeriodo').on('click', function () {
                $('#popupPeriodo').bPopup().close();
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }

    });
};

verHistoricoPeriodo = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'historicoperiodo',      
        success: function (evt) {
            $('#contenidoHistoricoPeriodo').html(evt);

            setTimeout(function () {
                $('#popupHistoricoPeriodo').bPopup({
                    autoClose: false
                });
            }, 50);

        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
};

eliminarPeriodo = function (id) {
    if (confirm('¿Está seguro de realizar esta operación?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'eliminarperiodo',
            data: {
                id: id
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensaje', 'exito', 'La operación se realizó correctamente.');
                    consultar();
                }
                else if (result == -1) {
                    mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }
};

grabarPeriodo = function () {
    var validacion = validarPeriodo();

    if (validacion == "") {

        if (confirm('¿Está seguro de realizar esta operación?')) {
            $.ajax({
                type: 'POST',
                url: controlador + 'grabarperiodo',
                data: $('#frmRegistroPeriodo').serialize(),
                dataType: 'json',
                success: function (result) {
                    if (result.Resultado == 1) {
                        mostrarMensaje('mensaje', 'exito', result.Mensaje);
                        $('#popupPeriodo').bPopup().close();
                        consultar();
                    }
                    else if (result.Resultado == 2) {
                        mostrarMensaje('mensajeEditPeriodo', 'alert', result.Mensaje);
                    }
                    else if (result.Resultado == -1) {
                        mostrarMensaje('mensajeEditPeriodo', 'error', 'Ha ocurrido un error.');
                    }
                },
                error: function () {
                    mostrarMensaje('mensajeEditPeriodo', 'error', 'Ha ocurrido un error.');
                }

            });
        }
    }
    else {
        mostrarMensaje('mensajeEditPeriodo', 'alert', validacion);
    }
};

validarPeriodo = function () {
    
    var html = "<ul>";
    var flag = true;

    if ($('#txtFechaVigenciaPeriodo').val() == "") {
        html = html + "<li>Seleccione fecha de vigencia</li>";
        flag = false;
    }

    if (!validarHora($('#txtPeriodoBase').val())) {
        html = html + "<li>El rango de horas de base debe tener el formato hh:mm - hh:mm</li>";
        flag = false;
    }
    if (!validarHora($('#txtPeriodoMedia').val())) {
        html = html + "<li>El rango de horas de media debe tener el formato hh:mm - hh:mm</li>";
        flag = false;
    }
    if (!validarHora($('#txtPeriodoPunta').val())) {
        html = html + "<li>El rango de horas de punta debe tener el formato hh:mm - hh:mm</li>";
        flag = false;
    }
    
    html = html + "</ul>";

    if (flag) {
        html = "";
    }

    return html;
};

editarUmbral = function (id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'configuracionumbral',
        data: {
            idConfiguracion: id
        },
        success: function (evt) {
            $('#contenidoUmbral').html(evt);

            setTimeout(function () {
                $('#popupUmbral').bPopup({
                    autoClose: false
                });
            }, 50);

            $('#txtFechaVigenciaUmbral').Zebra_DatePicker();
            $('#txtFechaExpiracionUmbral').Zebra_DatePicker();

            $('#btnGrabarUmbral').on('click', function () {
                grabarUmbral();
            });

            $('#btnCancelarUmbral').on('click', function () {
                $('#popupUmbral').bPopup().close();
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }

    });
};

verHistoricoUmbral = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'historicoumbral',
        success: function (evt) {
            $('#contenidoHistoricoUmbral').html(evt);

            setTimeout(function () {
                $('#popupHistoricoUmbral').bPopup({
                    autoClose: false
                });
            }, 50);

        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
};

eliminarUmbral = function (id) {
    if (confirm('¿Está seguro de realizar esta operación?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'eliminarumbral',
            data: {
                id: id
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensaje', 'exito', 'La operación se realizó correctamente.');
                    consultar();
                }
                else if (result == -1) {
                    mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }
};

grabarUmbral = function () {
    var validacion = validarUmbral();

    if (validacion == "") {

        if (confirm('¿Está seguro de realizar esta operación?')) {
            $.ajax({
                type: 'POST',
                url: controlador + 'grabarumbral',
                data: $('#frmRegistroUmbral').serialize(),
                dataType: 'json',
                success: function (result) {
                    if (result.Resultado == 1) {
                        mostrarMensaje('mensaje', 'exito', result.Mensaje);
                        $('#popupUmbral').bPopup().close();
                        consultar();
                    }
                    else if (result.Resultado == 2) {
                        mostrarMensaje('mensajeEditUmbral', 'alert', result.Mensaje);
                    }
                    else if (result.Resultado == -1) {
                        mostrarMensaje('mensajeEditUmbral', 'error', 'Ha ocurrido un error.');
                    }
                },
                error: function () {
                    mostrarMensaje('mensajeEditUmbral', 'error', 'Ha ocurrido un error.');
                }
            });
        }
    }
    else {
        mostrarMensaje('mensajeEditUmbral', 'alert', validacion);
    }
};

validarUmbral = function () {

    var html = "<ul>";
    var flag = true;

    if ($('#txtFechaVigenciaUmbral').val() == "") {
        html = html + "<li>Seleccione fecha de vigencia</li>";
        flag = false;
    }

    if ($('#txtTotalMaximo').val() == "") {
        html = html + "<li>Ingrese umbral CMg Total Máximo</li>";
        flag = false;
    }
    else {
        if (!validarNumero($('#txtTotalMaximo').val())) {
            html = html + "<li>El umbral CMg Total Máximo no tiene el formato correcto</li>";
            flag = false;
        }
    }

    if ($('#txtTotalMinimo').val() == "") {
        html = html + "<li>Ingrese umbral CMg Total Mínimo</li>";
        flag = false;
    }
    else {
        if (!validarNumero($('#txtTotalMinimo').val())) {
            html = html + "<li>El umbral CMg Total Mínimo no tiene el formato correcto</li>";
            flag = false;
        }
    }


    if ($('#txtEnergiaMaximo').val() == "") {
        html = html + "<li>Ingrese umbral CMg Energía Máximo</li>";
        flag = false;
    }
    else {
        if (!validarNumero($('#txtEnergiaMaximo').val())) {
            html = html + "<li>El umbral CMg Energía Máximo no tiene el formato correcto</li>";
            flag = false;
        }
    }

    if ($('#txtEnergiaMinimo').val() == "") {
        html = html + "<li>Ingrese umbral CMg Energía Mínimo</li>";
        flag = false;
    }
    else {
        if (!validarNumero($('#txtEnergiaMinimo').val())) {
            html = html + "<li>El umbral CMg Energía Mínimo no tiene el formato correcto</li>";
            flag = false;
        }
    }

    if ($('#txtCongestionMaximo').val() == "") {
        html = html + "<li>Ingrese umbral CMg Congestión Máximo</li>";
        flag = false;
    }
    else {
        if (!validarNumero($('#txtCongestionMaximo').val())) {
            html = html + "<li>El umbral CMg Congestión Máximo no tiene el formato correcto</li>";
            flag = false;
        }
    }

    if ($('#txtCongestionMinimo').val() == "") {
        html = html + "<li>Ingrese umbral CMg Congestión Mínimo</li>";
        flag = false;
    }
    else {
        if (!validarNumero($('#txtCongestionMinimo').val())) {
            html = html + "<li>El umbral CMg Congestión Mínimo no tiene el formato correcto</li>";
            flag = false;
        }
    }

    html = html + "</ul>";

    if (flag) {
        html = "";
    }

    return html;
};

editarEquipo = function (id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'configuracionequipo',
        data: {
            idConfiguracion: id,
            fecha: $('#txtFecha').val()
        },
        success: function (evt) {
            $('#contenidoEquipo').html(evt);

            setTimeout(function () {
                $('#popupEquipo').bPopup({
                    autoClose: false
                });
            }, 50);

            $('#txtFechaVigenciaEquipo').Zebra_DatePicker();
            $('#txtFechaExpiracionEquipo').Zebra_DatePicker();

            $('#cbEquipo').val($('#hfEquipo').val());

            $('#btnAgregarBarra').on('click', function () {
                mostrarMensaje('mensajeEditEquipo', 'message', 'Por favor ingrese los datos.');
                if ($('#cbBarraAdicional').val() != "") {
                    agregarBarra($('#cbBarraAdicional').val(), $("#cbBarraAdicional option:selected").text());
                }
                else {
                    mostrarMensaje('mensajeEditEquipo', 'alert', 'Debe seleccionar una barra');
                }
            });

            $('#btnGrabarEquipo').on('click', function () {
                grabarEquipo();
            });

            $('#btnCancelarEquipo').on('click', function () {
                $('#popupEquipo').bPopup().close();
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
};

agregarBarra = function (barraCodi, barraNomb) {    
    var count = 0;
    var flag = true;
    $('#tablaEquipoAdicional>tbody tr').each(function (i) {
        $punto = $(this).find('#hfEquipoItem');
        if ($punto.val() == barraCodi) {
            flag = false;
        }
    });

    if (flag) {
        $('#tablaEquipoAdicional> tbody').append(
            '<tr>' +
            '   <td style="text-align:center">' +
            '       <input type="hidden" id="hfEquipoItem" value="' + barraCodi + '" /> ' +
            '       <img src="' + siteRoot + 'Content/Images/btn-cancel.png" alt="" onclick="$(this).parent().parent().remove();" style="cursor:pointer" />' +
            '   </td>' +
            '   <td>' + barraNomb+ '</td>' +
            '</tr>'
        );
    }
    else {
        mostrarMensaje('mensajeEditEquipo', 'alert', 'La barra ya se encuentra agregada');
    }
};

verHistoricoEquipo = function (id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'historicoequipo',
        data: {
            id: id
        },
        success: function (evt) {
            $('#contenidoHistoricoEquipo').html(evt);

            setTimeout(function () {
                $('#popupHistoricoEquipo').bPopup({
                    autoClose: false
                });
            }, 50);

        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
};

eliminarEquipo = function (id) {
    if (confirm('¿Está seguro de realizar esta operación?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'eliminarequipo',
            data: {
                id: id
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensaje', 'exito', 'La operación se realizó correctamente.');
                    consultar();
                }
                else if (result == -1) {
                    mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }
};

grabarEquipo = function () {
    var validacion = validarEquipo();

    if (validacion == "") {

        var count = 0;
        var items = "";
        $('#tablaEquipoAdicional>tbody tr').each(function (i) {
            $punto = $(this).find('#hfEquipoItem');
            var constante = (count > 0) ? "," : "";
            items = items + constante + $punto.val();
            count++;
        });

        $('#hfListaBarras').val(items);

        if (confirm('¿Está seguro de realizar esta operación?')) {
            $.ajax({
                type: 'POST',
                url: controlador + 'grabarequipo',
                data: $('#frmRegistroEquipo').serialize(),
                dataType: 'json',
                success: function (result) {
                    if (result.Resultado == 1) {
                        mostrarMensaje('mensaje', 'exito', result.Mensaje);
                        $('#popupEquipo').bPopup().close();
                        consultar();
                    }
                    else if (result.Resultado == 2) {
                        mostrarMensaje('mensajeEditEquipo', 'alert', result.Mensaje);
                    }
                    else if (result.Resultado == -1) {
                        mostrarMensaje('mensajeEditEquipo', 'error', 'Ha ocurrido un error.');
                    }
                },
                error: function () {
                    mostrarMensaje('mensajeEditEquipo', 'error', 'Ha ocurrido un error.');
                }
            });
        }
    }
    else {
        mostrarMensaje('mensajeEditEquipo', 'alert', validacion);
    }
};

validarEquipo = function () {

    var html = "<ul>";
    var flag = true;

    if ($('#txtFechaVigenciaEquipo').val() == "") {
        html = html + "<li>Seleccione fecha de vigencia</li>";
        flag = false;
    }

    var count = $('#tablaEquipoAdicional >tbody >tr').length;

    if (count == 0) {
        mensaje = mensaje + "<li>Seleccione barras adicionales.</li>";
        flag = false;
    }

    html = html + "</ul>";

    if (flag) {
        html = "";
    }

    return html;
};

validarHora = function (inputStr) {
   
    if (!inputStr || inputStr.length < 1) { return false; }

    var part = inputStr.split('-');

    if (part.length != 2) { return false;}

    var time = part[0].split(':');
    var flagIni = (time.length === 2
        && parseInt(time[0], 10) >= 0
        && parseInt(time[0], 10) <= 23
        && parseInt(time[1], 10) >= 0
        && parseInt(time[1], 10) <= 59);

    if (!flagIni) { return false; }

    time = part[1].split(':');
    flagIni = (time.length === 2
        && parseInt(time[0], 10) >= 0
        && parseInt(time[0], 10) <= 23
        && parseInt(time[1], 10) >= 0
        && parseInt(time[1], 10) <= 59);

    if (!flagIni) { return false; }

    return true;
}

function validarNumero(texto) {
    return /^-?[\d.]+(?:e-?\d+)?$/.test(texto);
};

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
};