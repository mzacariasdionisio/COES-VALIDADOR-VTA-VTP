var controlador = siteRoot + 'Equipamiento/FTAsignacionAreas/';
var OPCION_VER = 1;
var OPCION_EDITAR = 2;
var OPCION_NUEVO = 3;

const IMG_FORMATO = '<img src="' + siteRoot + 'Content/Images/btn-properties.png" alt="Ver Envío" width="19" height="19" style="">';
const IMG_DETALLE_ASIG = '<img src="' + siteRoot + 'Content/Images/btn-properties.png" alt="Ver Envío" width="19" height="19" title="Asignar áreas a requisitos" style="">';
const IMG_CAMP_VERDE = '<img src="' + siteRoot + 'Content/Images/alerta_IDCC.png" width="19" height="19" style="">';
const IMG_CAMP_AMARILLA = '<img src="' + siteRoot + 'Content/Images/alerta_scada.png" width="19" height="19" style="">';
const IMG_CAMP_ROJA = '<img src="' + siteRoot + 'Content/Images/alerta_ems.png" width="19" height="19" style="">';
const IMG_ELIMINAR = '<img src="' + siteRoot + 'Content/Images/btn-cancel.png" alt="Eliminar Evento" width="19" height="19" style="">';

var posFilaNueva = -1;
var Arraynumerales = [];
var ArryNumeralEliminados = [];

$(function () {
    $('#tab-container').easytabs({
        animate: false
    });

    //limpiar mensaje
    $('#tab-container').bind('easytabs:midTransition', function () {
        limpiarBarraMensaje("mensaje");
    });

    $(".tabOC").on("click", function () {
        listarOperacionConmercial();
    });

    $(".tabMO").on("click", function () {
        asignacionAreaBajaMO();
    });

    $('#btnGrabarForm').click(function () {
        guardarEvento();
    });

    $('#btnCancelarForm').click(function () {
        cerrarPopup("popupEvento");
    });

    listarFichaTecnica();
});

///////////////////////////
/// Consulta Fichas técnica
///////////////////////////
function listarFichaTecnica() {
    $('#tab-container').easytabs('select', '#vistaListado');
    //limpiarBarraMensaje("mensaje");

    $.ajax({
        type: 'POST',
        url: controlador + "FichaTecnicaLista",
        dataType: 'json',
        data: {
        },
        success: function (result) {
            if (result.Resultado != "-1") {
                $('#listado').html('');
                var html = _dibujarTablaListado(result);
                $('#listado').html(html);
                $('#TablaFichaTecnica').dataTable({
                    "destroy": "true",
                    "scrollY": 430,
                    "scrollX": true,
                    "sDom": 'ft',
                    "ordering": false,
                    "paging": false,
                    "searching": true,
                    "iDisplayLength": -1
                });

            } else {
                mostrarMensaje('mensaje', 'error', result.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function _dibujarTablaListado(model) {
    var lista = model.ListaFichaTecnica;

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="TablaFichaTecnica" cellspacing="0" width="100%" >
        <thead>
            <tr>
                <th colspan="3"></th>
                <th colspan="3">Ficha Técnica</th>
                <th colspan="3" class="ficha_padre">Ficha Técnica Padre</th>
                <th colspan="2" class="ficha_padre">Última modificación</th>
                <th></th>
            </tr>
            <tr>
                <th style="width: 120px">Acciones</th>
                <th style="width: 50px">Código</th>
                <th style="width: 50px">Oficial</th>
                <th style="width: 150px">Origen</th>
                <th style="width: 240px">Tipo</th>
                <th style="width: 400px">Nombre</th>
                <th style="width: 150px" class="ficha_padre">Origen</th>
                <th style="width: 240px" class="ficha_padre">Tipo</th>
                <th style="width: 330px" class="ficha_padre">Nombre</th>
                <th style="width: 120px" class="ficha_padre">Usuario</th>
                <th style="width: 120px" class="ficha_padre">Fecha</th>
                <th style="width: 120px">Estado</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (var key in lista) {
        var item = lista[key];

        var claseFila = "";
        if (item.Ftecprincipal == 1) { claseFila = "clase_principal"; }
        if (item.Fteqestado == "B" || item.Fteqestado == "X") { claseFila = "clase_eliminado"; }
        //var sStyle = item.EstiloEstado;

        cadena += `

            <tr id="fila_${item.Concepcodi}" class="${claseFila}">
                <td style='width:90px;'>
                    <a href="JavaScript:formularioFichaTecnica(${item.Fteqcodi}, '${item.OrigenTipoDesc}', '${item.Ftecprincipal}');">${IMG_FORMATO}</a>
                </td>
                <td style="text-align:center;">${item.Fteqcodi}</td>
                <td style="text-align:center;">${item.FtecprincipalDesc}</td>
                <td>${item.OrigenDesc}</td>
                <td class="ficha_tipo">${item.OrigenTipoDesc}</td>
                <td class="ficha_nombre">${item.Fteqnombre}</td>
                <td class="ficha_padre">${item.OrigenPadreDesc}</td>
                <td class="ficha_padre">${item.OrigenPadreTipoDesc}</td>
                <td class="ficha_padre">${item.Fteqnombrepadre}</td>
                <td class="ficha_padre">${item.Ftequsumodificacionasig}</td>
                <td class="ficha_padre">${item.FteqfecmodificacionasigDesc}</td>
                <td>${item.FteqestadoDesc}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}


// detalle
function formularioFichaTecnica(fteqcodi, tipo, principal) {
    $('#tab-container').easytabs('select', '#vistaDetalle');

    limpiarBarraMensaje("mensaje");

    TREE_DATA = null;
    $.ajax({
        type: 'POST',
        url: controlador + "FichaTecnicaFormulario",
        dataType: 'json',
        data: {
            id: fteqcodi
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                var htmlDGDetalle = dibujarDatosGeneralesDetalle(evt);
                $("#div_detalle").html(htmlDGDetalle);
                $("#btnGuardarD").hide();

                $("#hdTipoFichaSeleccionado").val(tipo);
                $("#hdIdFichaSeleccionado").val(fteqcodi);
                $("#hdOficial").val(principal);

                $('#btnHabilitar').unbind();
                $('#btnHabilitar').click(function () {
                    habilitar();
                });

                //Deshabilito boton PRECARGAR para los que son Vigentes
                if ($("#hdOficial").val() == 1)
                    $("#btnPrecargarD").css("display", "none");

                $('#btnGuardarD').unbind();
                $('#btnGuardarD').on("click", function () {
                    guardarFormatoExtranet();
                });

                $('#btnCancelarD').unbind();
                $('#btnCancelarD').on("click", function () {
                    regresarAListado();
                });

                $('#btnPrecargarD').unbind();
                $('#btnPrecargarD').on("click", function () {
                    if ($("#hdOficial").val() != 1)
                        precargarInfo();
                });

                //pintar datos
                FICHA_GLOBAL = evt;
                OPCION_ACTUAL = 1;

                $("#hfJsonTree").val(evt.TreeJson);

                $("#treeContainer").empty();
                var htmlDGDetalle = dibujarTree();
                $("#treeContainer").html(htmlDGDetalle);

                cargarMenuTree();
                mostrarArbol();

            } else {
                mostrarMensaje('mensaje', 'error', evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function dibujarDatosGeneralesDetalle(data) {
    var cadena = '';
    cadena += `
                    <div class="search-content">
                        <table class="content-tabla-search" style="width:100%;">
                            <tr>

                                <td style="">
                                    <input type="button" id="btnHabilitar" value="Habilitar Edición" style="float: left;"/>
                                </td>

                                <td style="float: right;">
                                    <input type="button" id="btnPrecargarD" value="Precargar" />
                                    <input type="button" id="btnGuardarD" value="Guardar" />
                                    <input type="button" id="btnCancelarD" value="Cancelar" />
                                </td>

                            </tr>
                        </table>
                    </div>

                    <div id="tab-container2" class='tab-container'>
                        <div class='panel-container'>

                            <div id="tabFormato">
                                
                                <div class="table-list" id="detalle_ficha_tecnica"></div>                                

                            </div>
                        </div>
                    </div>
    `;


    return cadena;
}


function dibujarTree() {

    var cadena = '';
    cadena += `
               <table id="treeFT" style="display: none;">
                    <colgroup>
                        <col style="width:50px">
                        <col style="width:350px">
                        <col style="width:50px">
                        <col style="width:150px">
                        <col style="width:50px">
                        <col style="width:250px">
                        <col style="width:50px">
                        <col style="width:50px">
                        <col style="width:50px">
                        <col style="width: 110px;">
                        <col style="width: 110px;">
                    </colgroup>
                    <thead>
                        <tr>
                            <th>#</th>
                            <th></th>
                            <th>Orientación</th>
                            <th>Tipo</th>
                            <th>Código</th>
                            <th>Nombre</th>
                            <th>Unidad</th>
                            <th>Tipo</th>
                            <th>Nota</th>
                            <th>Usuario</th>
                            <th>Fecha</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td></td>
                            <td></td>
                            <td style="text-align: center"></td>
                            <td></td>
                            <td style="text-align: center"></td>
                            <td></td>
                            <td style="text-align: center"></td>
                            <td style="text-align: center"></td>
                            <td style="text-align: center"></td>
                            <td style="text-align: center"></td>
                            <td style="text-align: center"></td>
                        </tr>
                    </tbody>
                </table>
    `;


    return cadena;
}

function mostrarArbol() {
    iniciarVistaPrevia();
}

function agregarColumnasFila() {
    var filasTabla = document.getElementById("reporte").rows;

    var numF = 1;
    for (key in filasTabla) {

        var fila = filasTabla[key];
        var idFila = fila.id;

        var numNuevasCol = 1;

        if (numF < 3) { //NUAVAS COLUMNAS PARA LAS 2 PRIMERAS FILAS

            if (numF == 1) {
                var tagFila1 = document.getElementById("1raF");

                //agrego primera fila
                var nuevaCol1 = document.createElement('td');
                var cadena1 = '';

                //cadena1 = '';

                nuevaCol1.innerHTML = cadena1;
                nuevaCol1.colSpan = 10;
                nuevaCol1.classList.add("campo_titulo_tab");
                tagFila1.appendChild(nuevaCol1);
            }

            if (numF == 2) {
                //agrego segunda fila
                var tagFila2 = document.getElementById("2daF");

                var nuevaCol2 = document.createElement('td');
                var idNuevaCol2 = document.getElementsByTagName("td").length
                nuevaCol2.setAttribute('id', idNuevaCol2);
                nuevaCol2.classList.add("campo_cab_add_celdas");
                var cadena2 = 'ÁREAS COES';

                nuevaCol2.innerHTML = cadena2;
                tagFila2.appendChild(nuevaCol2);
            }

        } else { //filas con DATA DE LA TABLA
            if (idFila != null) {
                const myArray = idFila.split("_");
                let numeral = myArray[0];
                let miFtitcodi = myArray[1];
                let miPropcodi = myArray[2];
                let miConcepcodi = myArray[3];
                let esArchivo = myArray[4];

                var tagFila = document.getElementById(idFila);

                for (var i = 1; i <= numNuevasCol; i++) {

                    //Agrego
                    var nuevaCol = document.createElement('td');

                    var cadena = '';

                    if ((miPropcodi != "N" || miConcepcodi != "N")) {
                        if (i < 2) {

                            cadena = `
                                    <select  class="areascorreo" style="background-color:white" id="cbAreas_${miFtitcodi}" disabled="disabled" multiple="multiple">
                                `;

                            for (var j = 0; j < FICHA_GLOBAL.ListaAreas.length; j++) {

                                var reg = FICHA_GLOBAL.ListaAreas[j];
                                cadena += `
                                    <option value="${reg.Faremcodi}">${reg.Faremnombre}</option>
                                `;
                            }

                            cadena += `</select>`;
                        }
                    }

                    nuevaCol.innerHTML = cadena;
                    tagFila.appendChild(nuevaCol);

                }


            }
        }
        numF++;
    }

    $(".areascorreo").multipleSelect({
        selectAll: true, 
        filter: true,
        placeholder: "SELECCIONE",
    });

    setearValoresAreas();
}

function guardarFormatoExtranet() {
    limpiarBarraMensaje("mensaje");

    var filtro = datosTablaGuardar();
    var msg = validarDatosTablaGuardar(filtro);

    if (msg == "") {
        var dataFormato = obtenerDatosFormatoAGuardar();
        var miFteqcodi = parseInt($("#hdIdFichaSeleccionado").val());

        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: controlador + "GrabarDatosFormato",
            contentType: "application/json",
            data: JSON.stringify({
                relacion: dataFormato,
                fteqcodi: miFteqcodi
            }),
            beforeSend: function () {
                //mostrarExito("Enviando Información ..");
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {

                    //limpio la pestaña detalle
                    $("#div_detalle").html('');
                    $('#tab-container').easytabs('select', '#vistaListado');
                    mostrarMensaje('mensaje', 'exito', "La información se guardó de forma correcta.");
                    listarFichaTecnica();
                } else {
                    mostrarMensaje('mensaje', 'alert', "Ha ocurrido un error. " + evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje', 'error', "Ha ocurrido un error.");
            }
        });
    } else {
        mostrarMensaje('mensaje', 'alert', msg);
    }
}

function obtenerDatosFormatoAGuardar() {
    var arrayInfoFilas = [];

    var filasTabla = document.getElementById("reporte").rows;

    var numF = 1;
    for (var key in filasTabla) {

        var fila = filasTabla[key];
        var idFila = fila.id;

        if (numF < 3) { //NUEVAS COLUMNAS PARA LAS 2 PRIMERAS FILAS

        } else {
            if (idFila != null) {
                const myArray = idFila.split("_");
                let numeral = myArray[0];
                let miFtitcodi = myArray[1];
                let miPropcodi = myArray[2];

                var tagFila = document.getElementById(idFila);
                var lstSelects = tagFila.getElementsByTagName("select"); //Recorro todos los select dentro de la fila

                //Si existe combos empiezo a guardarlos en una lista 
                if (lstSelects.length > 0) {

                    var nombF = tagFila.dataset.nombfila;
                    //var valCmb1 = lstSelects[0].value;
                    var areaCodis = $("#cbAreas_" + miFtitcodi).multipleSelect('getSelects').join(',');
                    //var areaCodis = $('#Empresa').multipleSelect('getSelects').join(',');

                    let lstArea = areaCodis.split(',');
                    for (var key1 in lstArea) {
                        var item = lstArea[key1];

                        //creo objeto
                        var reg = {
                            Friacodi: 0,
                            //Ftitcodi: parseInt(miFtitcodi),
                            Ftitcodi: parseInt(miFtitcodi),
                            Faremcodi: parseInt(item),
                            NombrePropiedad: nombF
                            //Concepcodi: miConcepcodi,
                        }

                        arrayInfoFilas.push(reg);
                    }
                }

                var sd = 0;
            }
        }
        numF++;
    }

    //let regFormato = {
    //    "Fteqcodi": miFteqcodi,
    //    "Ftetcodi": miFtetcodi,
    //    "ListaDataFila": arrayInfoFilas
    //}


    return arrayInfoFilas;
}

function datosTablaGuardar() {
    var filtro = {};

    var arrayInfoFilas = [];
    var tabla = document.getElementById("reporte");
    var hayTabla = false;

    if (tabla != null) {
        hayTabla = true;
        var filasTabla = tabla.rows;

        //Recorremos todas las filas de la tabla
        for (key in filasTabla) {
            var fila = filasTabla[key];
            var idFila = fila.id;

            if (idFila != null) {
                const myArray = idFila.split("_");
                let numeral = myArray[0];
                let miFtitcodi = myArray[1];

                //Entramos a cada fila <tr>
                var tagFila = document.getElementById(idFila);

                //Busco los input dentro de la fila (como solo tengo uno por fila, sera mas simple)
                var lstText = tagFila.getElementsByTagName("select");

                if (lstText.length > 0) {

                    var areaCodis = $("#cbAreas_" + miFtitcodi).multipleSelect('getSelects').join(',');

                    let lstArea = areaCodis.split(',');

                    if (lstArea.length == 1) {

                        if (lstArea[0] == "") {
                            let reg = {
                                "parametro": numeral
                            }

                            arrayInfoFilas.push(reg);
                        }
                    }
                }
            }
        }
    }

    filtro.existeTabla = hayTabla;
    filtro.listaTxt = arrayInfoFilas;

    return filtro;
}

function validarDatosTablaGuardar(datos) {

    var msj = "";

    var lista = datos.listaTxt;

    if (datos.existeTabla) {

        if (lista.length > 0) {
            msj += "En Los siguientes parámetros se debe escoger como mínimo 1 área de revisión:"
            for (key in lista) {
                var fila = lista[key];
                msj += "<p>En el numeral: " + fila.parametro + "</p>";
            }
        }
    } else {
        msj += "<p>No existe datos a guardar.</p>";
    }

    return msj;

}

function setearValoresAreas() {
    //var lstDataFilas = formatoExtranet.ListaItems;
    var lstDataFilas = FICHA_GLOBAL.ListaAllItems.filter(x => x.Ftittipoitem == 0).sort((m, n) => m.Ftitorden - n.Ftitorden);

    //var lstDataFilas = formatoExtranet.ListaDataFila;
    for (var key in lstDataFilas) {
        var item = lstDataFilas[key];

        var miFtitcodi = item.Ftitcodi;

        var lstIdsAreas = [];
        var lstAreasXItem = item.ListaAreasXItem;

        if (lstAreasXItem != null) {
            for (var key2 in lstAreasXItem) {
                var item2 = lstAreasXItem[key2];
                lstIdsAreas.push(item2.Faremcodi);
            }
            $("#cbAreas_" + miFtitcodi).multipleSelect('setSelects', lstIdsAreas);
        }
        else {
            //$("#cbAreas_" + miFtitcodi).multipleSelect('checkAll'); 
        }
    }
}

function regresarAListado() {
    $('#tab-container').easytabs('select', '#vistaListado');

    //limpio la pestaña DETALLE y FORMATO
    $("#detalle_ficha_tecnica").html('');
    $("#div_detalle").html('');
}

function habilitar() {
    $("#btnGuardarD").show();
    $("button.ms-choice").removeClass('disabled');
}

function precargarInfo() {
    limpiarBarraMensaje("mensaje");

    if (confirm('¿Desea precargar la configuración de la ficha técnica oficial?')) {

        //var filtro = datosPrecargar();
        var msg = "";

        if (msg == "") {

            //dataFormato = obtenerDatosFormatoAGuardar();
            var miFteqcodi = parseInt($("#hdIdFichaSeleccionado").val());

            $.ajax({
                type: 'POST',
                dataType: 'json',
                url: controlador + "ObtenerDataDelVigente",
                contentType: "application/json",
                data: JSON.stringify({
                    fteqcodi: miFteqcodi
                }),
                beforeSend: function () {
                    //mostrarExito("Enviando Información ..");
                },
                success: function (evt) {
                    if (evt.Resultado != "-1") {
                        setearValoresEnTabla(evt.ListaAllItems);
                    } else {
                        mostrarMensaje('mensaje', 'alert', "Ha ocurrido un error. " + evt.Mensaje);
                    }
                },
                error: function (err) {
                    mostrarMensaje('mensaje', 'error', "Ha ocurrido un error.");
                }
            });
        }
        else {
            mostrarMensaje('mensaje', 'alert', msg);
        }
    }
}

function setearValoresEnTabla(listaItems) {
    var lstDataFilas = listaItems.filter(x => x.Ftittipoitem == 0).sort((m, n) => m.Ftitorden - n.Ftitorden);

    for (var key in lstDataFilas) {
        var item = lstDataFilas[key];

        var miFtitcodi = item.Ftitcodi;

        var lstIdsAreas = [];
        var lstAreasXItem = item.ListaAreasXItem;

        if (lstAreasXItem != null) {
            for (var key2 in lstAreasXItem) {
                var item2 = lstAreasXItem[key2];
                lstIdsAreas.push(item2.Faremcodi);
            }
            $("#cbAreas_" + miFtitcodi).multipleSelect('setSelects', lstIdsAreas);
        }
        else
            $("#cbAreas_" + miFtitcodi).multipleSelect('checkAll');
    }
}


///////////////////////////
/// Consulta operación comercial
///////////////////////////
function listarOperacionConmercial() {
    $('#tab-container-config').easytabs('select', '#vistaOC');
    //limpiarBarraMensaje("mensaje");

    $.ajax({
        type: 'POST',
        url: controlador + "listarEventos",
        dataType: 'json',
        data: {
        },
        success: function (result) {
            if (result.Resultado != "-1") {
                $('#listadoOC').html('');
                var html = _dibujarListaOC(result);
                $('#listadoOC').html(html);
                $('#tablaEventos').dataTable({
                    "scrollY": 400,
                    "scrollX": false,
                    "sDom": 'ft',
                    "ordering": false,
                    "iDisplayLength": -1
                });

            } else {
                mostrarMensaje('mensaje', 'error', result.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function _dibujarListaOC(model) {
    var lista = model.ListadoFtExtEventos;

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
               <th class="ficha_padre">Usuario Modificación</th>
               <th class="ficha_padre">Fecha de Modificación</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];
        var Campana = "";
        var claseFila = "";

        if (item.EstadoActual == "Eliminado")
            claseFila = "clase_eliminado";
        if (item.EstadoActual == "Baja")
            Campana = IMG_CAMP_ROJA;
        if (item.EstadoActual == "Vigente")
            Campana = IMG_CAMP_VERDE;
        if (item.EstadoActual == "En Proyecto")
            Campana = IMG_CAMP_AMARILLA;

        cadena += `       
            <tr class="${claseFila}">
                <td style='width:90px;'>
                    <a href="JavaScript:mostrarEvento(${item.Ftevcodi});">${IMG_DETALLE_ASIG}</a>
                </td>
                <td>${item.Ftevcodi}</td>
                <td>${item.Ftevnombre}</td>
                <td style="padding: 5px; text-align: left;">${Campana} ${item.EstadoActual}</td>
                <td>${item.FtevfecvigenciaextDesc}</td>
                <td>${item.Ftevusucreacion}</td>
                <td>${item.FtevfeccreacionDesc}</td>
                <td class="ficha_padre">${item.Ftevusumodificacionasig}</td>
                <td class="ficha_padre">${item.FtevfecmodificacionasigDesc}</td>
           </tr >           
        `;

    }
    cadena += `                   
        </tbody >
    </table >
    `;

    return cadena;
}

function mostrarEvento(idEvento) {
    limpiarPopupNuevo();
    $.ajax({
        type: 'POST',
        url: controlador + "DetallarEvento",
        data: { ftevcodi: idEvento },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                //prepararDetallesPopup();
                //setearInfoEvento(evt);
                $("#hdIdEvento").val(evt.FTExtEvento.Ftevcodi);
                addRequisitoInicioOC(evt)
                abrirPopup("popupEvento");

            } else {
                mostrarMensaje('mensaje', 'error', evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function limpiarPopupNuevo() {
    limpiarBarraMensaje("mensaje_popupEvento");
    posFilaNueva = -1;

    for (e in Arraynumerales) {
        $(`#tr_agrup_conf`).remove();
    }
    Arraynumerales = [];
    ArryNumeralEliminados = [];
}

function abrirPopup(contentPopup) {

    $("#tituloPopup").html("<span>Asignación revisión Áreas COES conformidad de inicio de operación comercial</span>");

    setTimeout(function () {
        $("#" + contentPopup).bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
}

function addRequisitoInicioOC(evt) {

    var ListaRequisitos = evt.ListaDetalleFTExtEvento;

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
            <a title="Eliminar registro"onclick="JavaScript:eliminarRegistroEdit(${item.Fevrqcodi});" class="borrar" id="btneliminar_${item.Fevrqcodi}">  
                ${IMG_ELIMINAR}
            </a>
        `;
        //if (accion == VER) {
        //    tdEliminar = '';
        //}
        //var tdEliminar = '';

        var htmlTr = `
            <tr id="tr_agrup_conf" class="tr_table_central_agrup">
                <input type="hidden" id="hdTotalSeleccionados_${item.Fevrqcodi}" value="${numSelecc}"/>
                <td class="td_agrup" style="padding: 4px;">
                  <input type="text" value="${item.Fevrqliteral}" style="background-color: white;width: 33px;" id="txtItem_${item.Fevrqcodi}">
                </td>
                <td class="td_agrup" style="padding: 4px;">
                  <textarea style="background-color: white;width: 550px;height: 72px;" id="txtDescrip_${item.Fevrqcodi}" value="${item.Fevrqdesc}">  </textarea>
                </td>
                <td class="td_agrup" style="padding: 4px; text-align: left; width:150px">
                   <input type="checkbox" name="chkBox_${item.Fevrqcodi}"  id="chkHidro_${item.Fevrqcodi}" value="1" ${chkHidro}>Hidroeléctrica<br>
                   <input type="checkbox" name="chkBox_${item.Fevrqcodi}"  id="chkTermo_${item.Fevrqcodi}" value="2" ${chkTermo}>Termoeléctrica<br>
                   <input type="checkbox" name="chkBox_${item.Fevrqcodi}"  id="chkEo_${item.Fevrqcodi}" value="3" ${chkEo}>Eólica<br>
                   <input type="checkbox" name="chkBox_${item.Fevrqcodi}"  id="chkSol_${item.Fevrqcodi}" value="4" ${chkSol}>Solar<br>
                </td>
                <td class="td_agrup" style="padding: 4px;">`;

        htmlTr += inicializaDesplegables(evt.ListaAreas, "cbHidro_" + item.Fevrqcodi);
        htmlTr += inicializaDesplegables(evt.ListaAreas, "cbTermo_" + item.Fevrqcodi);
        htmlTr += inicializaDesplegables(evt.ListaAreas, "cbkEo_" + item.Fevrqcodi);
        htmlTr += inicializaDesplegables(evt.ListaAreas, "cbSol_" + item.Fevrqcodi);

        htmlTr += `</td></tr>`;

        $("#idtbodyRequisitos").append(htmlTr);
        $("#txtDescrip_" + item.Fevrqcodi).val(item.Fevrqdesc);

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

        Arraynumerales.push(item.Fevrqcodi);
    }


    $(".areasRequisito").multipleSelect({
        selectAll: true,
        filter: true,
        placeholder: "SELECCIONE",
    });

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

    setearValoresAreasRequisitos(ListaRequisitos);
}

function inicializaDesplegables(listaAreas, idCbo) {

    var cadena = `
              <select  class="areasRequisito" style="background-color:white; width: 150px;" id="${idCbo}" multiple="multiple">
             `;

    for (var j = 0; j < listaAreas.length; j++) {

        var reg = listaAreas[j];
        cadena += `
                   <option value="${reg.Faremcodi}">${reg.Faremnombre}</option>
                  `;
    }

    cadena += `</select>`;
    return cadena;
}

function setearValoresAreasRequisitos(listaRequisitos) {
    //var lstDataFilas = formatoExtranet.ListaItems;
    //var lstDataFilas = listaRequisitos.filter(x => x.Ftittipoitem == 0).sort((m, n) => m.Ftitorden - n.Ftitorden);

    //var lstDataFilas = formatoExtranet.ListaDataFila;
    for (key in listaRequisitos) {
        var item = listaRequisitos[key];

        var miFevrqcodi = item.Fevrqcodi;

        var lstIdsAreasHidro = [];
        var lstIdsAreasTermo = [];
        var lstIdsAreasEolico = [];
        var lstIdsAreasSolar = [];
        var lstAreasXReq = item.ListaAreasXRequisito;

        if (lstAreasXReq != null) {

            lstIdsAreasHidro = item.ListaAreasXRequisitoHidro;
            lstIdsAreasTermo = item.ListaAreasXRequisitoTermo;
            lstIdsAreasEolico = item.ListaAreasXRequisitoEolico;
            lstIdsAreasSolar = item.ListaAreasXRequisitoSolar;

            $("#cbHidro_" + miFevrqcodi).multipleSelect('setSelects', lstIdsAreasHidro);
            $("#cbTermo_" + miFevrqcodi).multipleSelect('setSelects', lstIdsAreasTermo);
            $("#cbkEo_" + miFevrqcodi).multipleSelect('setSelects', lstIdsAreasEolico);
            $("#cbSol_" + miFevrqcodi).multipleSelect('setSelects', lstIdsAreasSolar);
        }
        else {
            //$("#cbHidro_" + miFevrqcodi).multipleSelect('checkAll');
            //$("#cbTermo_" + miFevrqcodi).multipleSelect('checkAll');
            //$("#cbkEo_" + miFevrqcodi).multipleSelect('checkAll');
            //$("#cbSol_" + miFevrqcodi).multipleSelect('checkAll');
        }
    }
}

function guardarEvento() {

    //limpiarBarraMensaje("mensaje_popupEvento")
    var filtro = ObtenerListaRequisitos();
    var msg = validarDatosAreaXReq(filtro);
    //var msg = "";

    if (msg == "") {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: controlador + "GuardarRelaAreaReq",
            contentType: "application/json",
            data: JSON.stringify({
                relacion: filtro
            }),
            success: function (evt) {
                if (evt.Resultado != "-1") {

                    listarOperacionConmercial();
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
}

function ObtenerListaRequisitos() {
    ///leer la tabla y guardarlos en un arreglo
    var ListaobjRelAreaReq = [];
    var ListaobjRequisitos = [];
    for (e in Arraynumerales) {

        var txtItem = $('#txtItem_' + Arraynumerales[e]).val();

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


        var idEvento = $("#hdIdEvento").val();
        if (idEvento === "") { idEvento = 0; }
        var nfevrqcodi = Arraynumerales[e];

        var flagHidro = "N";
        var flagTermmo = "N";
        var flagEo = "N";
        var flagSol = "N";

        var lstIdsHIdro = $("#cbHidro_" + Arraynumerales[e]).multipleSelect('getSelects').join(',');
        var lstIdsTermo = $("#cbTermo_" + Arraynumerales[e]).multipleSelect('getSelects').join(',');
        var lstIdsEolico = $("#cbkEo_" + Arraynumerales[e]).multipleSelect('getSelects').join(',');
        var lstIdsSolar = $("#cbSol_" + Arraynumerales[e]).multipleSelect('getSelects').join(',');

        if (lstIdsHIdro.length > 0) {
            flagHidro = "S";
        }
        if (lstIdsTermo.length > 0) {
            flagTermmo = "S";
        }
        if (lstIdsEolico.length > 0) {
            flagEo = "S";
        }
        if (lstIdsSolar.length > 0) {
            flagSol = "S";
        }

        let relacionAreaReq = {
            Fevrqcodi: nfevrqcodi,
            //Faremcodi: ,
            Frraflaghidro: flagHidro,
            Frraflagtermo: flagTermmo,
            Frraflagsolar: flagSol,
            Frraflageolico: flagEo,
            Strflaghidro: lstIdsHIdro,
            Strflagtermo: lstIdsTermo,
            Strflageolico: lstIdsEolico,
            Strflagsolar: lstIdsSolar,
            Chkhidro: chkHidro,
            Chktermo: chkTermmo,
            Chksolar: chkSol,
            Chkolico: chkEo,
            RequisitoItem: txtItem
        }

        ListaobjRelAreaReq.push(relacionAreaReq);
        //ListaobjRequisitos.push(requisito);
    }
    return ListaobjRelAreaReq;
}

function validarDatosAreaXReq(listareq) {

    var msj = "";

    if (listareq.length > 0) {
        var msj2 = "En Los siguientes ítems de requisitos se debe escoger como mínimo 1 área de revisión en cada archivo marcado:"
        for (key in listareq) {

            var fila = listareq[key];
            if (fila.Chkhidro == "S" && fila.Strflaghidro.length == 0) {
                msj += "<p>Requisito: " + fila.RequisitoItem + "</p>";
            }
            if (fila.Chktermo == "S" && fila.Strflagtermo.length == 0) {
                msj += "<p>Requisito: " + fila.RequisitoItem + "</p>";
            }
            if (fila.Chksolar == "S" && fila.Strflagsolar.length == 0) {
                msj += "<p>Requisito: " + fila.RequisitoItem + "</p>";
            }
            if (fila.Chkolico == "S" && fila.Strflageolico.length == 0) {
                msj += "<p>Requisito: " + fila.RequisitoItem + "</p>";
            }
        }
    }

    if (msj != "")
        msj = msj2 + msj;

    return msj;

}

function cerrarPopup(id) {
    $("#" + id).bPopup().close()
}

///////////////////////////
/// Dar de baja MO
///////////////////////////
function asignacionAreaBajaMO() {
    $('#tab-container').easytabs('select', '#vistaBajaMO');
    //limpiarBarraMensaje("mensaje");

    $.ajax({
        type: 'POST',
        url: controlador + "ListarAreasBajaMO",
        dataType: 'json',
        data: {
        },
        success: function (result) {
            if (result.Resultado != "-1") {
                $('#div_BajaMO').html('');
                var html = _dibujarBajaMO(result);
                $('#div_BajaMO').html(html);

                $('#btnGuardarBajaMO').click(function () {
                    guardarRelacionBajaMO();
                });

                $("#cbBajaMO").multipleSelect({
                    selectAll: true,
                    filter: true,
                    placeholder: "SELECCIONE",
                });

                setearValoresAreasBajaMO(result);

            } else {
                mostrarMensaje('mensaje', 'error', result.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function _dibujarBajaMO(model) {
    var cadena = '';
    cadena += `</br></br>
    <table style="width:auto">
          <tr>
              <td class="tbform-label" style="vertical-align: middle">Seleccione áreas de revisión:</td>
              <td>`;
    cadena += `
                <select style="background-color:white" id="cbBajaMO" multiple="multiple">
              `;


    for (var j = 0; j < model.ListaAreas.length; j++) {

        var reg = model.ListaAreas[j];
        cadena += `
                    <option value="${reg.Faremcodi}">${reg.Faremnombre}</option>
                  `;
    }

    cadena += `</select> </td>`;

    cadena += ` <td> <input id="btnGuardarBajaMO" type="button" value="Guardar"> </td>
                </tr > </table >`;


    cadena += `</br></br>
                <table style="width:auto">
                <tr style="height: 20px;">
                    <td class="tbform-label">Usuario Modificación:</td>
                    <td>
                        <div style="" id="usuarioMFT">${model.RequisitoBajaMO.UsuarioModificacion}</div>
                    </td>
                </tr>
                <tr style="height: 20px;">
                    <td class="tbform-label">Fecha Modificación:</td>
                    <td>
                        <div style="" id="fechaMFT">${model.RequisitoBajaMO.FechaModificacion}</div>
                    </td>
                </tr>
    </table >
    `;

    return cadena;
}

function setearValoresAreasBajaMO(result) {

    var lstIdsAreasBajaMO = [];
    var lstAreasXItem = result.RequisitoBajaMO.ListaAreasXRequisito;

    if (lstAreasXItem != null && lstAreasXItem.length > 0) {
        for (key2 in lstAreasXItem) {
            var item2 = lstAreasXItem[key2];
            lstIdsAreasBajaMO.push(item2.Faremcodi);
        }
        $("#cbBajaMO").multipleSelect('setSelects', lstIdsAreasBajaMO);
    }
    else {
        //$("#cbBajaMO").multipleSelect('checkAll');
    }
}

function guardarRelacionBajaMO() {

    var msj = validarRelacionBajaMO();

    if (msj == "") {
        var areaCodigos = $("#cbBajaMO").multipleSelect('getSelects').join(',');

        $.ajax({
            type: 'POST',
            url: controlador + "GuardarRelAreaBajaMO",
            dataType: 'json',
            data: {
                areas: areaCodigos
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    asignacionAreaBajaMO();
                    mostrarMensaje('mensaje', 'exito', "La información se guardó de forma correcta.");
                } else {
                    mostrarMensaje('mensaje', 'alert', "Ha ocurrido un error. " + evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje', 'error', "Ha ocurrido un error.");
            }
        });
    }
    else {
        mostrarMensaje('mensaje', 'alert', msj);
    }
}

function validarRelacionBajaMO() {
    var msj = "";

    if ($("#cbBajaMO").val() == null || $("#cbBajaMO").val() == "") {
        msj += "Se debe escoger como mínimo 1 área de revisión." + "\n";
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
