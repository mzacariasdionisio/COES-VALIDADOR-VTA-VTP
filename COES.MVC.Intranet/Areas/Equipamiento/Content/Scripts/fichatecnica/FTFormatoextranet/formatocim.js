var controlador = siteRoot + 'equipamiento/FTFormatoExtranet/';

const IMG_FORMATO = '<img src="' + siteRoot + 'Content/Images/btn-properties.png" alt="Formato Extranet" title="Formato Extranet" width="19" height="19" style="">';
const IMG_CAMP_VERDE = '<img src="' + siteRoot + 'Content/Images/alerta_IDCC.png" width="19" height="19" alt="En Vigencia" title="En Vigencia" style="position: absolute; left: 12px; top:3px;">';
const IMG_CAMP_AMARILLA = '<img src="' + siteRoot + 'Content/Images/alerta_scada.png" width="19" height="19" alt="En Proyecto" title="En Proyecto"style="position: absolute; left: 12px; top:3px;">';
const IMG_CAMP_ROJA = '<img src="' + siteRoot + 'Content/Images/alerta_ems.png" width="19" height="19" alt="En Baja" title="En Baja"style="position: absolute; left: 12px; top:3px;">';

const VIGENTE = "V";
const BAJA = "B";
const PROYECTO = "P";

$(function () {
    $('#tab-container').easytabs({
        animate: false
    });
    $('#tab-container').easytabs('select', '#tabLista');

    //limpio mensaje cada vez q cambio de pestaña
    $('#tab-container').bind('easytabs:midTransition', function () {
        limpiarBarraMensaje("mensaje");
    });    

    $("#btnManualUsuario").click(function () {
        window.location = controlador + 'DescargarManualUsuario';
    });
                
    mostrarListadoFTVisiblesExt();   

});

function mostrarListadoFTVisiblesExt() {
    limpiarBarraMensaje("mensaje");

    $.ajax({
        type: 'POST',
        url: controlador + "listarFTVisibleExt",
        data: {},
        success: function (evt) {
            if (evt.Resultado != "-1") {
                var htmlTabla = dibujarTablaFTVE(evt.ListaFichaTecnica);
                $("#div_listado").html(htmlTabla);

                $('#TablaFichaTecnica').dataTable({
                    "scrollY": 480,
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

function dibujarTablaFTVE(lista) {


    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="TablaFichaTecnica" cellspacing="0" width="100%" >
        <thead>
            <tr>
                <th colspan="3"></th>
                <th colspan="3">Ficha Técnica</th>
                <th colspan="2" class="ficha_padre">Extranet</th>
                <th colspan="2">Última modificación</th>
                <th></th>
            </tr>
            <tr>
                <th style="width: 120px">Acciones</th>
                <th style="width: 50px">Código</th>
                <th style="width: 50px">Oficial</th>
                <th style="width: 150px">Origen</th>
                <th style="width: 240px">Tipo</th>
                <th style="width: 400px">Nombre</th>
                <th style="width: 150px" class="ficha_padre">Estado Actual Extranet</th>
                <th style="width: 240px" class="ficha_padre">Fecha de vigencia Extranet</th>
                <th style="width: 120px">Usuario</th>
                <th style="width: 120px">Fecha</th>
                <th style="width: 120px">Estado</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];

        var claseFila = "";

        var Campana = "";

        if (item.EstadoActualExtranet == VIGENTE)
            Campana = IMG_CAMP_VERDE;
        if (item.EstadoActualExtranet == BAJA)
            Campana = IMG_CAMP_ROJA;
        if (item.EstadoActualExtranet == PROYECTO) {
            Campana = IMG_CAMP_AMARILLA;
        }

        cadena += `

            <tr id="fila_${item.Concepcodi}" class="${claseFila}">
                <td style='width:90px;'>
                    <a href="JavaScript:abrirFormatoExtranet(${item.Fteqcodi}, '${item.EstadoActualExtranet}', '${item.OrigenTipoDesc}');">${IMG_FORMATO}</a>
                </td>
                <td style="text-align:center;">${item.Fteqcodi}</td>
                <td style="text-align:center;">${item.FtecprincipalDesc}</td>
                <td>${item.OrigenDesc}</td>
                <td>${item.OrigenTipoDesc}</td>
                <td>${item.Fteqnombre}</td>
                <td style="position: relative;">${Campana} ${item.EstadoActualExtranetDesc}</td>
                <td>${item.FechaVigenciaExt}</td>
                <td>${item.UltimaModificacionUsuarioDesc}</td>
                <td>${item.UltimaModificacionFechaDesc}</td>
                <td>${item.FteqestadoDesc}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}

function abrirFormatoExtranet(fteqcodi, estadoFT, tipo) {
    $('#tab-container').easytabs('select', '#tabDetalle');

    limpiarBarraMensaje("mensaje");

    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerCamposGeneralesDetalles",
        data: {
            fteqcodi: fteqcodi
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                var htmlDGDetalle = dibujarDatosGeneralesDetalle(evt);
                $("#div_detalle").html(htmlDGDetalle);
                $("#hdIdFichaSeleccionado").val(fteqcodi);
                $("#hdFamcodiSeleccionado").val(evt.Famcodi);
                $("#hdCatecodiSeleccionado").val(evt.Catecodi);
                
                $("#hdEstadoFichaSeleccionado").val(estadoFT);
                $("#hdTipoFichaSeleccionado").val(tipo);
                

                //Deshabilito boton PRECARGAR para los que son Vigentes
                if ($("#hdEstadoFichaSeleccionado").val() == "V")
                    $("#btnPrecargarD").css("display", "none");

                $('#btnHabilitarD').unbind();
                $('#btnHabilitarD').click(function () {
                    habilitar();
                });

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
                    if ($("#hdEstadoFichaSeleccionado").val() != "V")
                        precargarInfo();
                });

                $('#btnPrevisualizarD').unbind();
                $('#btnPrevisualizarD').click(function () {
                    previsualizarExtranet();
                });

                $('#cbEtapa').change(function () {
                    limpiarBarraMensaje("mensaje");

                    //limpio la pestaña FORMATO
                    $("#detalle_ficha_tecnica").html('');
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



function dibujarDatosGeneralesDetalle(data) {

    var fechaV = data.Fecha;
    var lista = data.ListaEtapas;
    var cadena = '';
    cadena += `
                    <div>
                        <table style="width: auto; height: 26px;">
                            <tr>
                                <td class="label_campos_filtro" style="font-size: 13px;">Fecha de vigencia Extranet:</td>
                                <td id="txtFecVigenciaD" style="font-weight: bold; font-size: 14px; padding-left: 8px;">${fechaV}</td>

                            </tr>
                        </table>
                    </div>
                    <div class="search-content">
                        <table class="content-tabla-search" style="width:100%;">
                            <tr>
                                <td class="label_campos_filtro" style="width: 40px;">Etapa:</td>
                                <td style="width: 270px;">
                                    <select id="cbEtapa" style="width: 250px;">
                                        <option value="-1">-- SELECCIONE --</option>
    `;

    for (key in lista) {
        var item = lista[key];

        cadena += `                                        
                                            <option value="${item.Ftetcodi}">${item.Ftetnombre}</option>
        `;
    }
    cadena += `
                                    </select>
                                </td>

                                <td style="">
                                    <input type="button" id="btnHabilitarD" value="Habilitar Edición" style="float: left;"/>
                                </td>
                                <td style="float: right;">
                                    <input type="button" id="btnPrecargarD" value="Precargar" />
                                    <input type="button" id="btnPrevisualizarD" value="Previsualizar" />
                                    <input type="button" id="btnGuardarD" value="Guardar" />
                                    <input type="button" id="btnCancelarD" value="Cancelar" />
                                </td>

                            </tr>
                        </table>
                    </div>

                    <div id="tab-container2" class='tab-container'>
                        <ul class='etabs'>
                            <li class='tab'><a href="#tabFormato">Formato</a></li>
                            <li class='tab'><a href="#tabVistaPrevia">Vista Previa</a></li>
                        </ul>
                        <div class='panel-container'>

                            <div id="tabFormato">
                                
                                <div class="table-list" id="detalle_ficha_tecnica"></div>                                

                            </div>

                            <div id="tabVistaPrevia">

                                <div id="div_VistaPrev" class="content-tabla">


                                </div>
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


function habilitar() {
    limpiarBarraMensaje("mensaje");

    var filtro = datosHabilitar();
    var msg = validarFiltroHabilitar(filtro);
   
    if (msg == "") {

        $('#editarFichaTecnica').html('');

        TREE_DATA = null;

        var idFTSel = $("#hdIdFichaSeleccionado").val();
        $.ajax({
            type: 'POST',
            url: controlador + "ObtenerDatosFT",
            data: {
                id: idFTSel
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    FICHA_GLOBAL = evt;
                    OPCION_ACTUAL = 1; //PRUEBA

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
    } else {
        mostrarMensaje('mensaje', 'alert', msg);

        //limpio la pestaña FORMATO
        $("#detalle_ficha_tecnica").html('');
    
    }
}

function datosHabilitar() {
    var filtro = {};

    filtro.etapa = $('#cbEtapa').val();

    return filtro;
}

function validarFiltroHabilitar(datos) {

    var msj = "";

    if (datos.etapa == -1) {
        msj += "<p>Debe escoger una etapa correcta.</p>";
    }

    return msj;

}

function mostrarArbol() {
    iniciarVistaPrevia();
    
}

function agregarColumnasFila(){ 
    var filasTabla = document.getElementById("reporte").rows;

    var nombEtapa = "";
    var miFtetcodi = parseInt($("#cbEtapa").val()) || 0;

    switch (miFtetcodi) {
        case 1: nombEtapa = "CONEXIÓN"; break;
        case 2: nombEtapa = "INTEGRACIÓN"; break;
        case 3: nombEtapa = "OPERACIÓN COMERCIAL"; break;
        case 4: nombEtapa = "MODIFICACIÓN DE FICHA TÉCNICA"; break;        
    }

    var numF = 1;
    for (key in filasTabla) {

        var fila = filasTabla[key];
        var idFila = fila.id;

        var numNuevasCol = 9;

        if (numF < 3) { //NUAVAS COLUMNAS PARA LAS 2 PRIMERAS FILAS

            if (numF == 1) {
                var tagFila1 = document.getElementById("1raF");

                //agrego primera fila
                var nuevaCol1 = document.createElement('td');                
                var cadena1 = 'Configuración Ficha Técnica para Extranet (' + nombEtapa + ')';
                nuevaCol1.innerHTML = cadena1;
                nuevaCol1.colSpan = 10;
                nuevaCol1.classList.add("campo_titulo_tab");
                tagFila1.appendChild(nuevaCol1);
            }

            if (numF == 2) {
                //agrego segunda fila
                var tagFila2 = document.getElementById("2daF");

                for (var i = 1; i <= numNuevasCol; i++) {
                    var nuevaCol2 = document.createElement('td');
                    var idNuevaCol2 = document.getElementsByTagName("td").length
                    nuevaCol2.setAttribute('id', idNuevaCol2);
                    nuevaCol2.classList.add("campo_cab_add_celdas");                    
                    var cadena2 = '';
                    switch (i) {
                        case 1:
                            cadena2 = 'Contiene Comentario';
                            break;
                        case 2:
                            cadena2 = 'Opción Confidencial (valor o detalle)';
                            break;
                        case 3:
                            cadena2 = 'Parámetro Bloqueado Edición';
                            break;
                        case 4:
                            cadena2 = 'Adjuntar Sustento';
                            break;
                        case 5:
                            cadena2 = 'Opción Confidencial (Sustento)';
                            break;
                        case 6:
                            cadena2 = 'Instructivo de Llenado';
                            break;
                        case 7:
                            cadena2 = 'Envío Obligatorio de valor';
                            break;
                        case 8:
                            cadena2 = 'Envío Obligatorio de sustento';
                            break;
                        case 9:
                            cadena2 = 'Detalle de instructivo de llenado';
                            break;
                    }

                    nuevaCol2.innerHTML = cadena2;
                    tagFila2.appendChild(nuevaCol2);
                }
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
                    //SI por DEFECTO Para etapas: Conexión (1), Integración (2) y Modificación (4) en las columnas Instructivo de llenado (6)
                    var seleccionado = "";
                    if (miFtetcodi != 3) { //para etapas diferente a Operacion comercial
                        if ( i == 6) {
                            seleccionado = "selected";
                        }
                    }

                    //Agrego
                    var nuevaCol = document.createElement('td');
                    
                    var cadena = '';

                    if ((miPropcodi != "N" || miConcepcodi != "N")) {
                        if (i < 9) {
                            cadena = `
                                   <select id="cmb_${i}_${miFtitcodi}" style="width: auto;">
                                    <option value="N">No</option>
                                    <option value="S" ${seleccionado}>Si</option>
                                  </select>
                                `;
                        } else {
                            cadena = `<input type="text" id="txt_${i}_${miFtitcodi}" name="${numeral}" style="width: 120px;" value="" />`;
                        }
                    }

                    nuevaCol.innerHTML = cadena;
                    tagFila.appendChild(nuevaCol);
                }
                

            }
        }
        numF++;
    }

    setearValoresConfiguracionFE();
}

function guardarFormatoExtranet() {
    limpiarBarraMensaje("mensaje");

    var filtro = datosTablaGuardar();
    var msg = validarDatosTablaGuardar(filtro);

    if (msg == "") {

        dataFormato = obtenerDatosFormatoAGuardar(); 
        var miFteqcodi = parseInt($("#hdIdFichaSeleccionado").val());

        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: controlador + "GrabarDatosFormato",
            contentType: "application/json",
            data: JSON.stringify({
                formato: dataFormato,
                fteqcodi: miFteqcodi
            }),
            beforeSend: function () {
                //mostrarExito("Enviando Información ..");
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    mostrarMensaje('mensaje', 'exito', "La información se guardó de forma correcta.");
                } else {
                    mostrarMensaje('mensaje', 'alert', "Ha ocurrido un error. " + evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje', 'error', "Ha ocurrido un error.");
            }
        });
    }   else {
            mostrarMensaje('mensaje', 'alert', msg);
    }
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

                //Entramos a cada fila <tr>
                var tagFila = document.getElementById(idFila);

                //Busco los input dentro de la fila (como solo tengo uno por fila, sera mas simple)
                var lstText = tagFila.getElementsByTagName("input");

                if (lstText.length > 0) {
                    var valCampoTxt = lstText[0].value;
                    var txtName = lstText[0].name;

                    //creo objeto y listo
                    let reg = {
                        "Numeral": txtName,
                        "Valor": valCampoTxt
                    }

                    arrayInfoFilas.push(reg);
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

            for (key in lista) {
                var fila = lista[key];

                if (fila.Valor.length > 1000) {
                    msj += "<p>En el numeral: '" + fila.Numeral + "', el campo 'Detalle de instructivo de llenado' sobrepasa la cantidad máxima de caracteres (1000).</p>";
                }
            }
        }
    } else {
        msj += "<p>No existe datos a guardar.</p>";
    }

    return msj;

}


function obtenerDatosFormatoAGuardar() {
    var arrayInfoFilas = [];

    var filasTabla = document.getElementById("reporte").rows;

    var numF = 1;
    for (key in filasTabla) {

        var fila = filasTabla[key];
        var idFila = fila.id;

        if (numF < 3) { //NUEVAS COLUMNAS PARA LAS 2 PRIMERAS FILAS

        } else {
            if (idFila != null) {
                const myArray = idFila.split("_");
                let numeral = myArray[0];
                let miFtitcodi = myArray[1];
                let miPropcodi = myArray[2];
                let miConcepcodi = myArray[3];
                let esArchivo = myArray[4];

                var tagFila = document.getElementById(idFila);              
                var lstSelects = tagFila.getElementsByTagName("select"); //Recorro todos los select dentro de la fila

                //Si existe combos empiezo a guardarlos en una lista 
                if (lstSelects.length > 0) {
                    
                    var nombF = tagFila.dataset.nombfila;
                    

                    var valCmb1 = lstSelects[0].value;
                    var valCmb2 = lstSelects[1].value;
                    var valCmb3 = lstSelects[2].value;
                    var valCmb4 = lstSelects[3].value;
                    var valCmb5 = lstSelects[4].value;
                    var valCmb6 = lstSelects[5].value;
                    var valCmb7 = lstSelects[6].value;
                    var valCmb8 = lstSelects[7].value;

                    var lstText = tagFila.getElementsByTagName("input");
                    var valTxt = lstText[0].value;

                    //creo objeto y listo
                    let reg = {
                        Fitcfgcodi: 0,
                        Ftitcodi: parseInt(miFtitcodi),
                        Fitcfgflagcomentario: valCmb1,
                        Fitcfgflagvalorconf: valCmb2,
                        Fitcfgflagbloqedicion: valCmb3,
                        Fitcfgflagsustento: valCmb4,
                        Fitcfgflagsustentoconf: valCmb5,
                        Fitcfgflaginstructivo: valCmb6,
                        Fitcfgflagvalorobligatorio: valCmb7,
                        Fitcfgflagsustentoobligatorio: valCmb8,
                        Fitcfginstructivo: valTxt,
                        NombreFila: nombF,
                        Concepcodi: miConcepcodi,
                        Propcodi: miPropcodi                        
                    }                    

                    arrayInfoFilas.push(reg);
                }

                var sd = 0;
            }
        }
        numF++;
    }

    var miFteqcodi = parseInt($("#hdIdFichaSeleccionado").val());
    var miFtetcodi = parseInt($("#cbEtapa").val());

    let regFormato = {
        "Fteqcodi": miFteqcodi,
        "Ftetcodi": miFtetcodi,
        "ListaDataFila": arrayInfoFilas
    }


    return regFormato;
}

function setearValoresConfiguracionFE() {
    var miFteqcodi = parseInt($("#hdIdFichaSeleccionado").val());
    var miFtetcodi = parseInt($("#cbEtapa").val());

    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerValoresConfiguracionFormatoExtranet",
        data: {
            fteqcodi: miFteqcodi,
            ftetcodi: miFtetcodi
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {

                if (evt.ExisteInfoGuardada == 1)
                    setearValoresEnTablaFT(evt.FormatoExtranet);

            } else {
                mostrarMensaje('mensaje', 'error', evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function setearValoresEnTablaFT(formatoExtranet) {
    var lstDataFilas = formatoExtranet.ListaDataFila;
    for (key in lstDataFilas) {
        var item = lstDataFilas[key];

        var miFtitcodi = item.Ftitcodi;

        $("#cmb_1_" + miFtitcodi).val(item.Fitcfgflagcomentario);
        $("#cmb_2_" + miFtitcodi).val(item.Fitcfgflagvalorconf);
        $("#cmb_3_" + miFtitcodi).val(item.Fitcfgflagbloqedicion);
        $("#cmb_4_" + miFtitcodi).val(item.Fitcfgflagsustento);
        $("#cmb_5_" + miFtitcodi).val(item.Fitcfgflagsustentoconf);
        $("#cmb_6_" + miFtitcodi).val(item.Fitcfgflaginstructivo);
        $("#cmb_7_" + miFtitcodi).val(item.Fitcfgflagvalorobligatorio);
        $("#cmb_8_" + miFtitcodi).val(item.Fitcfgflagsustentoobligatorio);

        var inst = item.Fitcfginstructivo != null ? item.Fitcfginstructivo : "";
        $("#txt_9_" + miFtitcodi).val(inst);
    }
}

function regresarAListado() {
    $('#tab-container').easytabs('select', '#tabLista');

    //limpio la pestaña DETALLE y FORMATO
    $("#detalle_ficha_tecnica").html('');
    $("#div_detalle").html('');
}

function precargarInfo() {
    limpiarBarraMensaje("mensaje");

    if (confirm('¿Desea precargar la configuración de la ficha técnica vigente mostrada en la extranet?')) {

        var filtro = datosPrecargar();
        var msg = validarFiltroPrecargar(filtro);

        if (msg == "") {

            dataFormato = obtenerDatosFormatoAGuardar();
            var miFteqcodi = parseInt($("#hdIdFichaSeleccionado").val());
            var miFtetcodi = parseInt($("#cbEtapa").val());
            var miFamcodi = parseInt($("#hdFamcodiSeleccionado").val());
            var miCatecodi = parseInt($("#hdCatecodiSeleccionado").val());                        

            $.ajax({
                type: 'POST',
                dataType: 'json',
                url: controlador + "ObtenerDataDelVigente",
                contentType: "application/json",
                data: JSON.stringify({
                    fteqcodi: miFteqcodi,
                    ftetcodi: miFtetcodi,
                    famcodi: miFamcodi,
                    catecodi: miCatecodi,
                    formato: dataFormato,
                }),
                beforeSend: function () {
                    //mostrarExito("Enviando Información ..");
                },
                success: function (evt) {
                    if (evt.Resultado != "-1") {
                        if (evt.ExisteInfoGuardada == 1)
                            setearValoresEnTablaFT(evt.FormatoExtranet);
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

var COL_ADICIONAL = 2;
var OPCION_GLOBAL_EDITAR = true;
var mostrarColumnaValorConfidencial = false;
var mostrarColumnaInstructivoLlenado = false;
var mostrarColumnaSustento = false;

/**
 * Previsualizar
 * @returns
 */
function previsualizarExtranet() {
    $('#tab-container2').easytabs('select', '#tabVistaPrevia');

    var dataFormatoCfg = obtenerDatosFormatoAGuardar(); 

    var modeloFicha = {
        ListaTreeItems: FICHA_GLOBAL.ListaItems, //.ListaTreeItems
        ListaAllItems: FICHA_GLOBAL.ListaAllItems,
        ListaTreeData: FICHA_GLOBAL.ListaTreeData,
        ListaNota: [],
        ListaItemConfig: dataFormatoCfg.ListaDataFila
    };

    _determinarColumnasAdicionales(modeloFicha.ListaItemConfig);

    //Inicializar vista previa
    $("#div_VistaPrev").css("width", anchoPortal + "px");
    $("#div_VistaPrev").html(_extranet_generarHtmlReporteDetalleFichaTecnica(modeloFicha));
}

function _determinarColumnasAdicionales(lstConfiguraciones) {
    COL_ADICIONAL = 0;
    mostrarColumnaValorConfidencial = false;
    mostrarColumnaInstructivoLlenado = false;
    mostrarColumnaSustento = false;

    //Seteo si debe mostrar Columna VALOR CONFIDENCIAL
    var conFlagValConf = lstConfiguraciones.find(x => x.Fitcfgflagvalorconf === "S");
    if (conFlagValConf != null) {
        mostrarColumnaValorConfidencial = true;
        COL_ADICIONAL++;
    }

    //Seteo si debe mostrar Columna INSTRUCTIVO DE LLENADO
    var conInstructivoLlenado = lstConfiguraciones.find(x => x.Fitcfgflaginstructivo === "S");
    if (conInstructivoLlenado != null) {
        mostrarColumnaInstructivoLlenado = true;
        COL_ADICIONAL++;
    }

    //Seteo si debe mostrar Columna SUSTENTO
    var conSustento = lstConfiguraciones.find(x => x.Fitcfgflagsustento === "S");
    if (conSustento != null) {
        mostrarColumnaSustento = true;
        COL_ADICIONAL++;
    }
}

function datosPrecargar() {
    var filtro = {};
    
    var tabla = document.getElementById("reporte");
    var hayTabla = false;

    if (tabla != null) {
        hayTabla = true;
    }

    filtro.existeTabla = hayTabla;
    filtro.etapa = $('#cbEtapa').val();

    return filtro;
}

function validarFiltroPrecargar(datos) {

    var msj = "";

    if (datos.existeTabla) {
        if (datos.etapa == -1) {
            msj += "<p>Debe escoger una etapa correcta.</p>";
        }
    } else {
        msj += "<p>Debe habilitar edición para poder precargar la información.</p>";
    }

    return msj;

}

function cerrarPopup(id) {
    $("#" + id).bPopup().close()
}

function abrirPopup(contentPopup) {
    setTimeout(function () {
        $("#" + contentPopup).bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
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
