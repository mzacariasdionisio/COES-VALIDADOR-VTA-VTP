var controlador = siteRoot + 'Equipamiento/FTFormatoExtranet/';

const NUEVO = 1;
const EDITAR = 2;
const VER = 3;
const IMG_EDITAR = '<img src="' + siteRoot + 'Content/Images/btn-edit.png" alt="Editar Evento" title="Editar Evento" width="19" height="19" style="">';
const IMG_VER = '<img src="' + siteRoot + 'Content/Images/btn-open.png" alt="Ver Evento" title="Ver Evento"width="19" height="19" style="">';
const IMG_ELIMINAR = '<img src="' + siteRoot + 'Content/Images/btn-cancel.png" alt="Eliminar Evento" title="Eliminar Evento"width="19" height="19" style="">';

const IMG_CAMP_VERDE = '<img src="' + siteRoot + 'Content/Images/alerta_IDCC.png" width="19" height="19" style="">';
const IMG_CAMP_AMARILLA = '<img src="' + siteRoot + 'Content/Images/alerta_scada.png" width="19" height="19" style="">';
const IMG_CAMP_ROJA = '<img src="' + siteRoot + 'Content/Images/alerta_ems.png" width="19" height="19" style="">';

const COLOR_BAJA = "#b8c0c5";


var posFilaNueva = -1;
var Arraynumerales = [];
var ArryNumeralEliminados = [];

$(function () {
    $('#fechaVigencia').Zebra_DatePicker({
        format: "d/m/Y",               
    });
    
    $('#btnExportar').click(function () {
        exportarEventos();
    });

    $('#btnNuevo').click(function () {
        $("#hdAccion").val(NUEVO);        
        limpiarPopupNuevo();
        abrirPopup("popupEvento", $("#hdAccion").val());
    });

    
    $('#btnGrabarForm').click(function () {
        guardarEvento();
    });

    mostrarListadoEventos();

    
});

function mostrarListadoEventos() {    
   $.ajax({
      type: 'POST',
      url: controlador + "listarEventos",
      data: {         
         },
      success: function (evt) {
          if (evt.Resultado != "-1") {
              var htmlList = dibujarTablaListadoEventos(evt.ListadoFtExtEventos, evt.TienePermisoAdmin);
              $("#listado").html(htmlList);
              $('#tablaEventos').dataTable({
                 "scrollY": 400,
                 "scrollX": false,
                        "sDom": 'ft',
                        "ordering": false,
                        "iDisplayLength": -1
                    });                   
                } else {
                    mostrarMensaje('mensaje', 'error', evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });    
}

function dibujarTablaListadoEventos(listaEventos, esEditable){
    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="tablaEventos" >
       <thead>
           <tr style="height: 22px;">
               <th style='width:90px;'>Acciones</th>               
               <th>Código</th>
               <th>Nombre</th>
                <th>Estado Actual </br>Extranet</th>
               <th>Fecha de Vigencia </br>Extranet</th>
               <th>Usuario Creación</th>
               <th>Fecha de Creación</th>
                <th>Usuario Modificación</th>
               <th>Fecha de Modificación</th>
            </tr>
        </thead>
        <tbody>
    `;


    for (key in listaEventos) {
        var item = listaEventos[key];
        var Campana = "";
        var botonEliminar = '';
        var botonEditar = '';
        var colorFila = "";

        if (item.EstadoActual == "Eliminado") {
            Campana = "";
            colorFila = COLOR_BAJA;
        }

        if (item.EstadoActual == "Baja")
            Campana = IMG_CAMP_ROJA;
        if (item.EstadoActual == "Vigente" && esEditable) {
            Campana = IMG_CAMP_VERDE;
            botonEditar += `
                <a href="JavaScript:editarEvento(${item.Ftevcodi},${EDITAR});">${IMG_EDITAR}</a>
            `;            
        }            
       
        if (item.EstadoActual == "En Proyecto" && esEditable) {
            Campana = IMG_CAMP_AMARILLA;
            botonEditar += `
                <a href="JavaScript:editarEvento(${item.Ftevcodi},${EDITAR});">${IMG_EDITAR}</a>
            `;
            botonEliminar += `
                <a href="JavaScript:eliminarEvento(${item.Ftevcodi});">${IMG_ELIMINAR}</a>
            `;

        }

        cadena += `       
            <tr>
                <td style='width:90px; background: ${colorFila}'>
                    <a href="JavaScript:mostrarEvento(${item.Ftevcodi},${VER} );">${IMG_VER}</a>
                    ${botonEditar}
                    ${botonEliminar}             
                </td>
                <td style="background: ${colorFila}">${item.Ftevcodi}</td>
                <td style="background: ${colorFila}">${item.Ftevnombre}</td>
                <td style="padding: 5px; text-align: left; background: ${colorFila}">${Campana} ${item.EstadoActual}</td>
                <td style="background: ${colorFila}">${item.FtevfecvigenciaextDesc}</td>
                <td style="background: ${colorFila}">${item.Ftevusucreacion}</td>
                <td style="background: ${colorFila}">${item.FtevfeccreacionDesc}</td>
                <td style="background: ${colorFila}">${item.Ftevusumodificacion}</td>
                <td style="background: ${colorFila}">${item.FtevfecmodificacionDesc}</td>
           </tr >           
        `;
    }

    cadena += `                   
        </tbody >
    </table >
    `;

    return cadena;


}

function addRequisitoInicioOpeCom() {        
    //document.getElementById('table_requisitos').insertRow(-1).innerHTML += `
    var htmlTr = `
    <tr id="tr_agrup_conf" class="tr_table_central_agrup">
        <input type="hidden" id="hdTotalSeleccionados_${posFilaNueva}" value="0"/>
        <td class="td_agrup" style="padding: 4px;">
            <input type="text" value="" style="background-color: white;width: 33px;" id="txtItem_${posFilaNueva}">
        </td>
        <td class="td_agrup" style="padding: 4px;">
            <textarea style="background-color: white;width: 650px;height: 72px;"  id="txtDescrip_${posFilaNueva}"></textarea>
        </td>
        <td class="td_agrup" style="padding: 4px; text-align: left;">
            <input type="checkbox" name="chkBox_${posFilaNueva}" id="chkHidro_${posFilaNueva}" value="1">Hidroeléctrica<br>
            <input type="checkbox" name="chkBox_${posFilaNueva}" id="chkTermo_${posFilaNueva}" value="2">Termoeléctrica<br>
            <input type="checkbox" name="chkBox_${posFilaNueva}" id="chkEo_${posFilaNueva}" value="3">Eólica<br>
            <input type="checkbox" name="chkBox_${posFilaNueva}" id="chkSol_${posFilaNueva}" value="4">Solar<br>
        </td>                            
        <td class="td_agrup" style="padding: 4px;">
            <a title="Eliminar registro"onclick="JavaScript:eliminarRegistroNuevo(${posFilaNueva});" class="borrar">  
                ${IMG_ELIMINAR}
            </a>
        </td>
    </tr>
    `;

    $("#idtbodyRequisitos").append(htmlTr);

    Arraynumerales.push(posFilaNueva);
    posFilaNueva += -1;

    //Toda la columna cambia (al escoger casilla cabecera)
    $('input[type=checkbox][name^="chkBox_"]').unbind();
    $('input[type=checkbox][name^="chkBox_"]').change(function () {
        var nota = "";
        var nameCheck = $(this).attr('name') + '';
        var array = nameCheck.split('_');
        var cod = array[1];
        var valorCheck = $(this).prop('checked');
        
        var numSel = parseInt($("#hdTotalSeleccionados_" + cod).val());
        if (valorCheck)
            numSel++;
        else
            numSel--;
        $("#hdTotalSeleccionados_" + cod).val(numSel);

        var numSelFinal = parseInt($("#hdTotalSeleccionados_" + cod).val());
        var inss = 0;
    });
}

function addRequisitoInicioOpeComVerEdit(ListaRequisitos, accion) {

    for (key in ListaRequisitos) {
        var item = ListaRequisitos[key];

        var chkHidro = "";
        var chkTermo = "";
        var chkEo = "";
        var chkSol = "";
        var numSelecc = 0;

        if (item.Fevrqflaghidro == "S") {
            chkHidro = " checked ";
            numSelecc++;
        }
        if (item.Fevrqflagtermo == "S") {
            chkTermo = " checked ";
            numSelecc++;
        }
        if (item.Fevrqflageolico == "S") {
            chkEo = " checked ";
            numSelecc++;
        }
        if (item.Fevrqflagsolar == "S") {
            chkSol = " checked ";
            numSelecc++;
        }

        var tdEliminar = `
            <a title="Eliminar registro"onclick="JavaScript:eliminarRegistroEdit(${item.Fevrqcodi}, ${accion});" class="borrar" id="btneliminar_${item.Fevrqcodi}">  
                ${IMG_ELIMINAR}
            </a>
        `;
        if (accion == VER) {
            tdEliminar = '';
        }

        var htmlTr = `
            <tr id="tr_agrup_conf" class="tr_table_central_agrup">
                <input type="hidden" id="hdTotalSeleccionados_${item.Fevrqcodi}" value="${numSelecc}"/>
                <td class="td_agrup" style="padding: 4px;">
                  <input type="text" value="${item.Fevrqliteral}" style="background-color: white;width: 33px;" id="txtItem_${item.Fevrqcodi}">
                </td>
                <td class="td_agrup" style="padding: 4px;">
                  <textarea style="background-color: white;width: 650px;height: 72px;" id="txtDescrip_${item.Fevrqcodi}" value="${item.Fevrqdesc}">  </textarea>
                </td>
                <td class="td_agrup" style="padding: 4px; text-align: left;">
                   <input type="checkbox" name="chkBox_${item.Fevrqcodi}"  id="chkHidro_${item.Fevrqcodi}" value="1" ${chkHidro}>Hidroeléctrica<br>
                   <input type="checkbox" name="chkBox_${item.Fevrqcodi}"  id="chkTermo_${item.Fevrqcodi}" value="2" ${chkTermo}>Termoeléctrica<br>
                   <input type="checkbox" name="chkBox_${item.Fevrqcodi}"  id="chkEo_${item.Fevrqcodi}" value="3" ${chkEo}>Eólica<br>
                   <input type="checkbox" name="chkBox_${item.Fevrqcodi}"  id="chkSol_${item.Fevrqcodi}" value="4" ${chkSol}>Solar<br>
                </td>
                <td class="td_agrup" style="padding: 4px;">
                   ${tdEliminar}
                </td>
            </tr>
        `;

        $("#idtbodyRequisitos").append(htmlTr);
        $("#txtDescrip_" + item.Fevrqcodi).val(item.Fevrqdesc);

        if (accion == VER) {
        
            //deshabilitamos los elementos
            let caja = document.getElementById("txtDescrip_" + item.Fevrqcodi);
            caja.disabled = true
            caja = document.getElementById("txtItem_" + item.Fevrqcodi);
            caja.disabled = true
            caja = document.getElementById("chkHidro_" + item.Fevrqcodi);
            caja.disabled = true
            caja = document.getElementById("chkTermo_" + item.Fevrqcodi);
            caja.disabled = true
            caja = document.getElementById("chkEo_" + item.Fevrqcodi);
            caja.disabled = true
            caja = document.getElementById("chkSol_" + item.Fevrqcodi);
            caja.disabled = true
        }

        Arraynumerales.push(item.Fevrqcodi);
    }

    //Toda la columna cambia (al escoger casilla cabecera)
    $('input[type=checkbox][name^="chkBox_"]').unbind();
    $('input[type=checkbox][name^="chkBox_"]').change(function () {
        var nota = "";
        var nameCheck = $(this).attr('name') + '';
        var array = nameCheck.split('_');
        var cod = array[1];

        var valorCheck = $(this).prop('checked');
        
        var numSel = parseInt($("#hdTotalSeleccionados_" + cod).val());
        if (valorCheck)
            numSel++;
        else
            numSel--;
        $("#hdTotalSeleccionados_" + cod).val(numSel);

        var numSelFinal = parseInt($("#hdTotalSeleccionados_" + cod).val());
        var inss = 0;
    });
}

function eliminarRegistroNuevo(pos) { 
    $(document).on('click', '.borrar', function (event) {
        event.preventDefault();
        $(this).closest('tr').remove();
    });    
    var aux = Arraynumerales.filter((e) => e != pos);
    Arraynumerales = aux;
}

function eliminarRegistroEdit(pos, accion) { 

    if (confirm('¿Desea eliminar el registro?')) {
        $(document).on('click', '.borrar', function (event) {
            event.preventDefault();
            $(this).closest('tr').remove();
        });
        var aux = Arraynumerales.filter((e) => e != pos);
        Arraynumerales = aux;
        ArryNumeralEliminados.push(pos);
    }
}


function mostrarEvento(idEvento, accion) {
    limpiarPopupNuevo();
    $.ajax({
        type: 'POST',
        url: controlador + "DetallarEvento",
        data: { ftevcodi: idEvento },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                prepararDetallesPopup(accion);
                setearInfoEvento(evt, accion);
                addRequisitoInicioOpeComVerEdit(evt.ListaDetalleFTExtEvento, accion)
                abrirPopup("popupEvento", accion);            

            } else {
                mostrarMensaje('mensaje', 'error', evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function editarEvento(idEvento, accion) {    
    mostrarEvento(idEvento, accion);
}

function prepararDetallesPopup(accion) {
    
    if (accion == VER) {
        deshabilitarTextField("txtFtevnombre");
        deshabilitarTextField("fechaVigencia");
        deshabilitarBotonGuardar("btnguardar");        
    } else {
        if (accion == EDITAR) {           
            habilitarTextField("txtFtevnombre");
            habilitarTextField("fechaVigencia");
            habilitarBotonGuardar("btnguardar");
            $("#tituloPopup").html("<span>[Editar] Requisitos para la obtención de la Conformidad de inicio de Operación Comercial</span>");
        }
    }
}

function setearInfoEvento(evt, accion) {

    $("#txtFtevnombre").val(evt.FTExtEvento.Ftevnombre);
    $("#fechaVigencia").val(evt.FTExtEvento.FtevfecvigenciaextDesc);
    $("#hdAccion").val(accion);
    $("#hdIdEvento").val(evt.FTExtEvento.Ftevcodi);
}

function eliminarEvento(id) {
    limpiarBarraMensaje("mensaje");
    if (confirm('¿Desea eliminar el registro?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'EliminarEvento',
            data: {
                ftevcodi: id
            },
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado == "1") {
                    mostrarListadoEventos();
                    mostrarMensaje('mensaje', 'exito', 'El registro se eliminó correctamente.');
                }
                else {
                    mostrarMensaje('mensaje', 'error', 'Error: ' + evt.Mensaje);
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }

}

function guardarEvento() {
    var accion = $("#hdAccion").val();
    if (accion == EDITAR || accion == NUEVO) {
        limpiarBarraMensaje("mensaje_popupEvento");

        var filtro = datosRequisitos(accion);
        var msg = validarDatosEvento(filtro);

        if (msg == "") {
            var matriz = JSON.stringify(filtro.ListaRequisitos);
            $.ajax({
                type: 'POST',
                url: controlador + "GuardarEvento",
                data: {
                    ftevnombre: filtro.nombreevento,
                    sfechavigencia: filtro.fechavigencia,
                    listaRequisitos: matriz,
                    accion: filtro.Accion,
                    ftevcodi: filtro.IdEvento
                },

                success: function (evt) {
                    if (evt.Resultado != "-1") {

                        mostrarListadoEventos();
                        mostrarMensaje('mensaje', 'exito', "La operacion se realizó satisfactoriamente.");
                        cerrarPopup("popupEvento");                        
                        posFilaNueva = -1;                                                

                    } else {
                        mostrarMensaje('mensaje_popupEvento', 'error', evt.Mensaje);
                    }
                },
                error: function (err) {
                    mostrarMensaje('mensaje_popupEvento', 'error', 'Ha ocurrido un error al guardar.');
                }
            });
        } else {
            mostrarMensaje('mensaje_popupEvento', 'alert', msg);
        }
    } else
        mostrarMensaje('mensaje_popupEvento', 'alert', "No tiene permiso para realizar cambios.");   
}

function datosRequisitos(accion) {
    var filtro = {};
    filtro.nombreevento = $("#txtFtevnombre").val();
    filtro.fechavigencia = $("#fechaVigencia").val();    
    filtro.ListaRequisitos = ObtenerListaRequisitos(accion);
    filtro.Accion = $("#hdAccion").val();
    filtro.IdEvento = parseInt($("#hdIdEvento").val()) || -3;
    return filtro;
}


function ObtenerListaRequisitos(accion) {
    ///leer la tabla y guardarlos en un arreglo
    var ListaobjRequisitos = [];
    for (e in Arraynumerales) {
       

        var txtItem = $('#txtItem_' + Arraynumerales[e]).val();
        var txtDescrip = $('#txtDescrip_' + Arraynumerales[e]).val();

        var idEvento = $("#hdIdEvento").val();
        if (idEvento === "") { idEvento = 0; }
        var nfevrqcodi = Arraynumerales[e];

        var chkHidro = "N";
        var chkTermmo = "N";
        var chkEo = "N";
        var chkSol = "N";

        
        var namecheck = 'chkHidro_' + Arraynumerales[e];

        if ($('#' + namecheck).is(':checked')) {
            chkHidro = "S";
        }
        namecheck = 'chkTermo_' + Arraynumerales[e];
        if ($('#' + namecheck).is(':checked')) {
            chkTermmo = "S";
        }
        namecheck = '#chkEo_' + Arraynumerales[e];
        if ($(namecheck).is(':checked')) {
            chkEo = "S";
        }
        namecheck = 'chkSol_' + Arraynumerales[e];

        if ($('#' + namecheck).is(':checked')) {
            chkSol = "S";
        }

        var flag = 1; // 1 nuevo, 2 editar 3 eliminar
        if (accion != NUEVO) {
            if (Arraynumerales[e] < 0) flag = 1;
            else flag = 2;
        }

        let requisito = {
            "ftevcodi": idEvento,
            "fevrqcodi": nfevrqcodi,
            "fevrqliteral": txtItem,
            "fevrqdesc": txtDescrip,
            "fevrqflaghidro": chkHidro,
            "fevrqflagtermo": chkTermmo,
            "fevrqflagsolar": chkSol,
            "fevrqflageolico": chkEo,
            "fevrqestado": "A",
            "Nuevo": flag

        }
     
        ListaobjRequisitos.push(requisito);        
    }
    if (accion == EDITAR) {
        for (aux in ArryNumeralEliminados) {        
            let requisito2 = {
                "ftevcodi": 0,
                "fevrqcodi": ArryNumeralEliminados[aux],
                "fevrqliteral": "",
                "fevrqdesc": "",
                "fevrqflaghidro": "",
                "fevrqflagtermo": "",
                "fevrqflagsolar": "",
                "fevrqflageolico": "",
                "fevrqestado": "",
                "Nuevo": 3
            }            
            ListaobjRequisitos.push(requisito2);
        }
    }
    return ListaobjRequisitos;
}


function exportarEventos() {
    limpiarBarraMensaje("mensaje");

    
        $.ajax({
            type: 'POST',
            url: controlador + 'exportarEventos',
            data: {                
            },
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    window.location = controlador + "Exportar?file_name=" + evt.Resultado;
                } else {
                    mostrarMensaje('mensaje', 'error', 'Error: ' + evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    
}


///UTIL////

function cerrarPopup(id) {
    $("#" + id).bPopup().close()
}

function abrirPopup(contentPopup, accion) {

    if (accion == NUEVO || accion == EDITAR) {
        habilitarTextField("txtFtevnombre");
        habilitarTextField("fechaVigencia");
        habilitarBotonGuardar("btnguardar");
    }  

    setTimeout(function () {
        $("#" + contentPopup).bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
}
function validarDatosEvento(datos) { 

    var msj = "";
   
    if (datos.nombreevento.trim() == "") {
        msj += "<p>Debe ingresar 'Nombre del evento'.</p>";
    } else {
        if (datos.nombreevento.trim().length > 200) {
            msj += "<p>El campo 'Nombre del evento' no debe exceder los 200 caracteres.</p>";
        }
    }

    if (datos.fechavigencia.trim() == "") {
        msj += "<p>Debe seleccionar una Fecha de vigencia Extranet.</p>";
    }

    if (Arraynumerales.length == 0) {
        msj += "<p>Debe agregar al menos un requisito.</p>";
    }
    else {
        var j = 1;
        var listaFilasSinItem = [];
        var listaFilasConExcesoCaracteresItem = [];
        var listaFilasSinDescripcion = [];
        var listaFilasConExcesoCaracteresDescripcion = [];

        for (let i = 0; i < datos.ListaRequisitos.length; i++) {
            if (datos.ListaRequisitos[i].Nuevo != 3) // si fueron eliminados no se considera
            {

                j = i + 1;
                if (datos.ListaRequisitos[i].fevrqliteral.trim() == "") {
                    //msj += "<p>Debe ingresar valor de 'Ítem' en la fila “" + j + "”.</p>";
                    listaFilasSinItem.push(j);
                }
                else {
                    var nI = datos.ListaRequisitos[i].fevrqliteral.trim().length;
                    if (nI > 10) {
                        //msj += "<p>El valor de “Ítem” en la fila “" + j + "” no debe exceder los 10 caracteres.</p>";
                        listaFilasConExcesoCaracteresItem.push(j);
                    }
                }
                if (datos.ListaRequisitos[i].fevrqdesc.trim() == "") {
                    //msj += "<p>Debe ingresar valor de “Descripción” en la fila “" + j + "”.</p>";
                    listaFilasSinDescripcion.push(j);
                }
                else {
                    var nD = datos.ListaRequisitos[i].fevrqdesc.trim().length;
                    if (nD > 1000) {
                        //msj += "El valor de “Descripción” en la fila  “" + j + "” no debe exceder los 1000 caracteres.</p>";
                        listaFilasConExcesoCaracteresDescripcion.push(j);
                    }
                }

            }
        }

        if (listaFilasSinItem.length > 0) {
            msj += "<p>Debe ingresar valor de 'Ítem' en la(s) fila(s): " + listaFilasSinItem.join(', ') + ".</p>";            
        }

        if (listaFilasConExcesoCaracteresItem.length > 0) {
            msj += "<p>El valor de 'Ítem' no debe exceder los 10 caracteres en la(s) fila(s): " + listaFilasConExcesoCaracteresItem.join(', ') + ".</p>";
        }

        if (listaFilasSinDescripcion.length > 0) {
            msj += "<p>Debe ingresar valor de 'Descripción' en la(s) fila(s): " + listaFilasSinDescripcion.join(', ') + ".</p>";
        }

        if (listaFilasConExcesoCaracteresDescripcion.length > 0) {
            msj += "<p>El valor de 'Descripción' no debe exceder los 1000 caracteres en la(s) fila(s): " + listaFilasConExcesoCaracteresDescripcion.join(', ') + ".</p>";
        }
    }

    var hayRequisitosSinArchivo = false;
    for (key in Arraynumerales) {
        var num = Arraynumerales[key];

        var numSel = parseInt($("#hdTotalSeleccionados_" + num).val());

        if (numSel == 0) {
            hayRequisitosSinArchivo = true;
        }        
    }

    if (hayRequisitosSinArchivo) {
        msj += "<p>Hay requisitos donde no se ha seleccionado una opción en 'Agregar Archivo'. Se requiere seleccionar mínimo una opción en cada requisito. </p>";
    }

    return msj;

}

function mostrarMensaje(id, tipo, mensaje) {
    $("#" + id).css("display", "block");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);

}

function limpiarBarraMensaje(id) {
    $('#' + id).css("display", "none");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-message');
    $('#' + id).html('');
}

function limpiarPopupNuevo() {
    limpiarBarraMensaje("mensaje_popupEvento");
    $("#txtFtevnombre").val("");
    $("#fechaVigencia").val("");
    $("#tituloPopup").html("<span>Requisitos para la obtención de la Conformidad de inicio de Operación Comercial</span>");    
    posFilaNueva = -1;
    limpiartablaRequisitos();
    
    
}

function habilitarTextField(id) {
    document.getElementById(id).disabled = false;
    $("#" + id).css("background", "white");
}

function deshabilitarTextField(id) {
    document.getElementById(id).disabled = true;
    $("#" + id).css("background", "#F2F4F3");
}

function deshabilitarBotonGuardar(id) {
    $("."+id).hide();
}

function habilitarBotonGuardar(id) {
    $("." + id).show();
}

function limpiartablaRequisitos() {
    for (e in Arraynumerales) {
        $(`#tr_agrup_conf`).remove();
    }
    Arraynumerales = [];
    ArryNumeralEliminados = [];
    
}

