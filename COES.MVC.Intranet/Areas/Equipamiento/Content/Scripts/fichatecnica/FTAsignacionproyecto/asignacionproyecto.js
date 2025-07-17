var controlador = siteRoot + 'Equipamiento/FTAsignacionProyecto/';

const ACTIVA = 1;
const BAJA = 0;

const NUEVO = 1;
const EDITAR = 2;
const VER = 3;

const ETAPA_CONEXION = 1;
const ETAPA_INTEGRACION = 2;
const ETAPA_OPERACION_COMERCIAL = 3;
const ETAPA_MODIFICACION = 4;

const IMG_EDITAR = '<img src="' + siteRoot + 'Content/Images/btn-edit.png" alt="Editar" title="Editar" width="19" height="19" style="">';
const IMG_VER = '<img src="' + siteRoot + 'Content/Images/btn-open.png" alt="Ver" title="Ver" width="19" height="19" style="">';
const IMG_ELIMINAR = '<img src="' + siteRoot + 'Content/Images/btn-cancel.png" alt="Eliminar" title="Eliminar" width="19" height="19" style="">';
const IMG_ACTIVAR = '<img src="' + siteRoot + 'Content/Images/btn-ok.png" alt="Activar" title="Activar" width="19" height="19" style="">';
const IMG_DETALLES = '<img src="' + siteRoot + 'Content/Images/btn-properties.png" alt="Detalles" title="Detalles" width="19" height="19" style="">';
const IMG_ADD = '<img src="' + siteRoot + 'Content/Images/btn-add.png" alt="Agregar" title="Agregar" width="19" height="19" style="">';

const COLOR_BAJA = "#FFDDDD";

const TIPO_EQUIPO = 1;
const CATEGORIA_GRUPO = 2;

//Listas para etapas C-I-O
var listaProyectosEnMemoria = [];
var listaCambiosGlobalEtapaCIO = [];
var listaCambiosDetallesEtapaCIO = [];

//Listas para etapa de Modificacion
var listaEquiposEnMemoria = [];
var listaElementosSegunFiltro = [];

var marcadoTodoCIO;


$(function () {
    $('#btnNuevo').click(function () {
        $("#nota_").css("display", "none");
        $("#nota_2").css("display", "none");
        $("#hdAccion").val(NUEVO);
        $("#hdIdRelEmpEtapa").val(0);
        $("#cbEmpresapop").multipleSelect('enable');
        habilitarSelect("cbEtapapop");
        //quito boton Agregar Proy
        $("#seccionBoton").html("");
        $("#btnGuardarProy").show();
        limpiarPopupNuevo();
        abrirPopup("popupProyecto", $("#hdAccion").val());
    });

    $('#cbEmpresa').multipleSelect({
        filter: true
    });

    $('#cbEmpresapop').multipleSelect({
        filter: true,
        single: true,
        onClose: function () {
        }
    });
    $('#cbEmpresa').multipleSelect('checkAll');

    $('#btnBuscar').click(function () {
        mostrarListadoAsigProyectos();
    });

    $('#btnGuardarProy').unbind();
    $('#btnGuardarProy').click(function () {
        guardarProyecto();
    });

    $('#btnCancelarProy').unbind();
    $('#btnCancelarProy').click(function () {
        accionesCerrarPopupProyecto();
    });


    $('#cbEmpresapop').change(function () {
        inicializarVariablesGlobales();
        mostrarMensajeEtapaEmpresa();
    });

    $('#cbEtapapop').change(function () {
        inicializarVariablesGlobales();
        mostrarMensajeEtapaEmpresa();

    });

    $('input[type=radio][name=origen]').change(function () {

        var valorRadio = parseInt(this.value) || 0;

        switch (valorRadio) {
            case TIPO_EQUIPO:
                $("#cbCategoriaGrupo").hide();
                $("#cbFamiliaEquipo").show();
                $("#cbFamiliaEquipo").val("-1");
                break;

            case CATEGORIA_GRUPO:
                $("#cbFamiliaEquipo").hide();
                $("#cbCategoriaGrupo").show();
                $("#cbCategoriaGrupo").val("-1");
                break;
        }

        mostrarElementosSegunFiltro();
    });

    $("#cbFamiliaEquipo").unbind();
    $("#cbFamiliaEquipo").change(function () {
        mostrarElementosSegunFiltro();
    });
    $("#cbCategoriaGrupo").unbind();
    $("#cbCategoriaGrupo").change(function () {
        mostrarElementosSegunFiltro();
    });

    $("#cbUbicacion").unbind();
    $("#cbUbicacion").change(function () {
        mostrarElementosSegunFiltro();
    });

    $('input[type=checkbox][name^="checkFL_"]').change(function () {
        setearModificacion(this);
    });


    mostrarListadoAsigProyectos();
});

function accionesCerrarPopupProyecto() {
    cerrarPopup('popupProyecto');

    listaProyectosEnMemoria = [];
    listaCambiosGlobalEtapaCIO = [];
    listaCambiosDetallesEtapaCIO = [];

    listaEquiposEnMemoria = [];
    listaElementosSegunFiltro = [];

    $("#hdIdRelEmpEtapa").val(0);
}

function limpiarPopupNuevo() {
    limpiarBarraMensaje("mensaje");
    limpiarBarraMensaje("mensaje_popupProyecto");

    inicializarVariablesGlobales();

    $("#tituloPopup").html("<span>Formulario de asignación de proyectos</span>");
    $("#cbEtapapop").val("0");
    $("#cbEmpresapop").multipleSelect("setSelects", [0]);
}

function inicializarVariablesGlobales() {

    $("#listadoProyectosElementosExtranetAsignacion").html("");

    mostrarMensaje('mensaje_popupProyecto', 'message', 'Seleccione empresa y etapa.');
    listaCambiosGlobalEtapaCIO = [];
    listaCambiosDetallesEtapaCIO = [];
    listaProyectosEnMemoria = [];
    listaEquiposEnMemoria = [];
    listaElementosSegunFiltro = [];
}

function mostrarListadoAsigProyectos() {
    limpiarBarraMensaje("mensaje");
    var filtro = datosFiltro();
    var msg = validarDatosFiltroBusqueda(filtro);

    if (msg == "") {
        $.ajax({
            type: 'POST',
            url: controlador + "listarProyectosAsig",
            data: {
                idempresa: filtro.empresa,
                idetapa: filtro.etapa
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    var htmlList = dibujarTablaListadoAsigProyectos(evt.ListadoProyectosAsig);
                    $("#listado").html(htmlList);
                    $('#tablaProyectosAsig').dataTable({
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
    else {
        mostrarMensaje('mensaje', 'error', msg);
    }

}

function dibujarTablaListadoAsigProyectos(ListadoProyectosAsig) {
    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="tablaProyectosAsig" >
       <thead>
           <tr style="height: 22px;">
               <th style='width:90px;'>Acciones</th>               
               <th>Empresa</th>
               <th>Etapa</th>
               <th>Estado</th>
               <th>Usuario creación</th>
               <th>Fecha creación</th>
               <th>Usuario modificación</th>
               <th>Fecha modificación</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in ListadoProyectosAsig) {
        var item = ListadoProyectosAsig[key];
        var botonEliminar = '';
        var botonEditar = '';
        var botonActivar = '';
        var colorFila = "";
        var estado = "Activo";

        if (item.Fetempestado == "A") { //Activo
            botonEditar += `
                <a href="JavaScript:mostrarProyecto(${item.Fetempcodi},${EDITAR});">${IMG_EDITAR}</a>
            `;
            botonEliminar += `
                <a href="JavaScript:cambiaestadoProyecto(${item.Fetempcodi},${BAJA});">${IMG_ELIMINAR}</a>
            `;
        }
        else { // de baja
            colorFila = COLOR_BAJA;
            estado = "Baja";
            botonActivar += `
                <a href="JavaScript:cambiaestadoProyecto(${item.Fetempcodi},${ACTIVA});">${IMG_ACTIVAR}</a>
            `;
        }

        cadena += `
            <tr>
                <td style='width:90px; background: ${colorFila}'>
                    <a href="JavaScript:mostrarProyecto(${item.Fetempcodi},${VER});">${IMG_VER}</a>
                    ${botonEditar}
                    ${botonEliminar}
                    ${botonActivar}
        `;

        cadena += `                    
                </td>
                <td style="background: ${colorFila}">${item.Emprnomb}</td>
                <td style="background: ${colorFila}">${item.Ftetnombre}</td>
                <td style="background: ${colorFila}">${estado}</td>
                <td style="background: ${colorFila}">${item.Fetempusucreacion}</td>
                <td style="background: ${colorFila}">${item.StrFetempfeccreacion}</td>
                <td style="background: ${colorFila}">${item.Fetempusumodificacion}</td>
                <td style="background: ${colorFila}">${item.StrFetempfecmodificacion}</td>                
           </tr >           
        `;
    }

    cadena += `                   
        </tbody >
    </table >
    `;

    return cadena;
}

function cambiaestadoProyecto(id, accion) {
    var mensajecpnfirmación = '¿Desea eliminar el registro?';
    var mensajesalida = 'Eliminación exitosa!.';
    if (accion == ACTIVA) {
        mensajecpnfirmación = '¿Desea activar el registro?';
        mensajesalida = 'Se actualizó el registro!.';
    }
    limpiarBarraMensaje("mensaje");
    if (confirm(mensajecpnfirmación)) {

        $.ajax({
            type: 'POST',
            url: controlador + 'CambiarestadoProyecto',
            data: {
                fetempcodi: id,
                opt: accion
            },
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado == "1") {
                    mostrarListadoAsigProyectos();
                    mostrarMensaje('mensaje', 'exito', mensajesalida);
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


function mostrarProyecto(id, accion) {

    $("#hdAccion").val(accion);

    if (accion == EDITAR) {
        $("#nota_").css("display", "block");
        $("#nota_2").css("display", "block");
    }

    else {
        $("#nota_").css("display", "none");
        $("#nota_2").css("display", "none");
    }



    limpiarPopupNuevo();
    $.ajax({
        type: 'POST',
        url: controlador + "DetallarProyecto",
        data: { fetempcodi: id },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                prepararDetallesPopup(accion);
                setearInfoProyecto(evt, accion);
                abrirPopup("popupProyecto", accion);

                //Almaceno info de cambios de los check (CASO CUANDO AGREAGO NUEVOS EQUIPOS)
                $('input[type=checkbox][name^="checkFL_"]').change(function () {
                    setearModificacion(this);
                });

                $('input[type=checkbox][name^="checkCentral_"]').change(function () {
                    setearModifCheckCentral(this);
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

function prepararDetallesPopup(accion) {

    $("#cbEmpresapop").multipleSelect('disable');
    deshabilitarSelect("cbEtapapop");

    if (accion == VER) {
        $("#btnGuardarProy").hide();
        $("#tituloPopup").html("<span>[Detalles] Formulario de asignación de proyectos</span>");

    } else {
        if (accion == EDITAR) {
            $("#btnGuardarProy").show();
            $("#tituloPopup").html("<span>[Editar] Formulario de asignación de proyectos</span>");
        }
    }
}

function setearInfoProyecto(evt, accion) {

    var idEtapa = evt.Proyecto.Ftetcodi;
    $("#cbEtapapop").val(idEtapa);
    $("#cbEmpresapop").multipleSelect("setSelects", [evt.Proyecto.Emprcodi]);

    $("#hdIdRelEmpEtapa").val(evt.Proyecto.Fetempcodi);

    if (idEtapa != ETAPA_MODIFICACION) {
        llenarArrayDeProyectos(evt.Proyecto.ListaProyectos);
        mostrarProyectosExtranet(listaProyectosEnMemoria);
    } else {
        llenarArrayDeElementos(evt.ListadoRelacionEGP);
        mostrarListadoEquiposO(listaEquiposEnMemoria);
    }
}

function llenarArrayDeProyectos(listaProys) {
    listaProyectosEnMemoria = [];

    for (key in listaProys) {
        var objProyecto = listaProys[key];

        let proy = {
            "feeprycodi": objProyecto.Feeprycodi,

            "ftprycodi": objProyecto.Ftprycodi,
            "nombreEmpresa": objProyecto.NombreEmpresa,
            "codigoProy": objProyecto.CodigoProy,
            "nombProyExt": objProyecto.NombProyExt
        }

        listaProyectosEnMemoria.push(proy);
    }
}

function llenarArrayDeElementos(listado) {

    listaEquiposEnMemoria = [];

    for (key in listado) {
        var objRelacionEGP = listado[key];
        var idE = objRelacionEGP.Codigo;


        let proy = {
            "Feeeqcodi": objRelacionEGP.Feeeqcodi,
            "Codigo": objRelacionEGP.Codigo,  //string
            "Emprcodi": objRelacionEGP.Emprcodi, //string
            "EmpresaCoNomb": objRelacionEGP.EmpresaCoNomb, //string
            "EmpresaNomb": objRelacionEGP.EmpresaNomb, //string
            "Equicodi": objRelacionEGP.Equicodi, //entero o nulo
            "Grupocodi": objRelacionEGP.Grupocodi, //entero o nulo
            "Tipo": objRelacionEGP.Tipo, //string
            "Ubicacion": objRelacionEGP.Ubicacion, //string
            "EstadoDesc": objRelacionEGP.EstadoDesc, //string
            "EquipoNomb": objRelacionEGP.EquipoNomb, //string
            "EstadoReg": "M",  // A: Agregado, E:Eliminado, M:Mismo
            //"Emprcodi": objRelacionEGP.Emprcodi,
            "TieneCambios": objRelacionEGP.TieneCambios,
            "FlagEquipoOtroEtapa": objRelacionEGP.FlagEquipoOtroEtapa,
            "FlagCentralCOES": objRelacionEGP.FlagCentralCOES,
            "Famcodi": objRelacionEGP.Famcodi,
            "EditoFlag": ""
        }

        listaEquiposEnMemoria.push(proy);


    }
}

function guardarProyecto() {

    var accion = parseInt($("#hdAccion").val());
    if (accion == EDITAR || accion == NUEVO) {
        limpiarBarraMensaje("mensaje_popupProyecto");

        var filtro = datosRequisitos();
        var msg = validarDatosProyecto(filtro);

        if (msg == "") {
            var miLstProy = FormatearListaProyectos(); //datos tabla proyectos
            var miLstCambiosCIO = FormatearListaCambiosCIO(); //datos cambios etapas CIO
            var miLstElementos = FormatearListaElementos(); //datos etapa Modificacion

            $.ajax({

                type: 'POST',
                dataType: 'json',
                url: controlador + "GuardarProyecto",
                contentType: "application/json",
                data: JSON.stringify({
                    accion: accion,
                    fetempcodi: filtro.idRelEmpEtapa,
                    emprcodi: filtro.idempresa,
                    idetapa: filtro.etapaid,
                    lstProyectos: miLstProy,
                    lstCambiosCIO: miLstCambiosCIO,
                    lstElementos: miLstElementos
                }),
                beforeSend: function () {
                    //mostrarExito("Enviando Información ..");
                },

                success: function (evt) {
                    if (evt.Resultado != "-1") {
                        mostrarListadoAsigProyectos();
                        mostrarMensaje('mensaje', 'exito', "La operación se realizó satisfactoriamente.");
                        accionesCerrarPopupProyecto();

                    } else {
                        mostrarMensaje('mensaje_popupProyecto', 'error', evt.Mensaje);
                    }
                },
                error: function (err) {
                    mostrarMensaje('mensaje_popupProyecto', 'error', 'Ha ocurrido un error al guardar.');
                }
            });
        } else {
            mostrarMensaje('mensaje_popupProyecto', 'alert', msg);
        }
    } else
        mostrarMensaje('mensaje_popupProyecto', 'alert', "No tiene permiso para realizar cambios.");
}

function datosRequisitos() {
    var filtro = {};
    filtro.idempresa = parseInt($('#cbEmpresapop').val()) || 0;
    filtro.etapaid = $("#cbEtapapop").val();
    filtro.ListaProyectos = listaProyectosEnMemoria;
    filtro.ListaEquipos = listaEquiposEnMemoria;
    filtro.idRelEmpEtapa = parseInt($("#hdIdRelEmpEtapa").val()) || 0;

    return filtro;
}


function validarDatosProyecto(filtro) {

    var msj = "";

    //validaciones generales
    if (filtro.idempresa == 0) {
        msj += "<p>Debe seleccionar una Empresa.</p>";
    }

    if (filtro.etapaid == 0) {
        msj += "<p>Debe seleccionar una Etapa.</p>";
    }

    //validaciones especificas por accion
    var accion = $("#hdAccion").val();

    if (accion == NUEVO) {
        if (filtro.etapaid == ETAPA_CONEXION || filtro.etapaid == ETAPA_INTEGRACION || filtro.etapaid == ETAPA_OPERACION_COMERCIAL) {
            if (filtro.ListaProyectos.length == 0) {
                msj += "<p>Debe seleccionar al menos un 'Proyecto Extranet'.</p>";
            }
        } else {
            if (filtro.etapaid == ETAPA_MODIFICACION) {
                if (filtro.ListaEquipos.length == 0) {
                    msj += "<p>Debe ingresar al menos un 'Equipo'.</p>";
                }
            }
        }
    } else {
        if (accion == EDITAR) {
            //Se permite listado vacio de equipos y proyectos
        }
    }

    return msj;
}



function datosFiltro() {
    var filtro = {};
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    $('#hfEmpresa').val(empresa);


    filtro.empresa = $('#hfEmpresa').val()
    filtro.etapa = $("#cbEtapa").val();
    return filtro;
}

function validarDatosFiltroBusqueda(datos) {

    var msj = "";

    if (datos.empresa == "") {
        msj += "<p>Debe seleccionar una empresa.</p>";
    }

    return msj;

}


function mostrarMensajeEtapaEmpresa() {
    var idEmpresa = $("#cbEmpresapop").val();
    var idEtapa = $("#cbEtapapop").val();
    var accion = parseInt($("#hdAccion").val());

    var mensaje = "";

    if (idEmpresa == "0") {
        if (idEtapa == "0") {
            mensaje = "Seleccione empresa y etapa.";
        } else {
            mensaje = "Seleccione empresa.";
        }
    } else {
        if (idEtapa == "0") {
            mensaje = "Seleccione etapa.";
        }
    }

    if (mensaje == "") {
        limpiarBarraMensaje("mensaje_popupProyecto");

        var etapaEnNuevo = parseInt(idEtapa);

        var htmlList = "";

        if (accion == NUEVO || accion == EDITAR) {
            if (etapaEnNuevo == 4) {
                htmlList = dibujarAgregarEquipo();
            } else {
                htmlList = dibujarAgregarProyecto();
            }
        }
        $("#seccionBoton").html(htmlList);

    }
    else {
        mostrarMensaje('mensaje_popupProyecto', 'message', mensaje);

        $("#seccionBoton").html("");
    }

}

function dibujarAgregarProyecto() {

    var cadena = '';
    cadena += `
            <table id="" style="padding-left: 190px;">
                <tr style="height: 25px;">
                    <td class="tbform-label" style="width: 130px;">Agregar Proyecto:</td>
                    <td>
                        <a title="Agregar proyecto" href="JavaScript:agregarProyectoExtranet();">  ${IMG_ADD} </a>

                    </td>
                </tr>
            </table>
    `;

    return cadena;
}

function dibujarAgregarEquipo() {

    var cadena = '';
    cadena += `
            <table id="" style=" padding-left: 190px;">
                <tr style="height: 25px;">
                    <td class="tbform-label" style="width: 130px;">Agregar Equipo:</td>
                    <td>
                        <a title="Agregar Equipo" href="JavaScript:agregarEquipo();">  ${IMG_ADD} </a>

                    </td>
                </tr>
            </table>
    `;

    return cadena;
}


function agregarProyectoExtranet() {
    limpiarBarraMensaje("mensaje_popupProyecto");
    limpiarBarraMensaje("mensaje_popupBusquedaPy");

    $.ajax({
        type: 'POST',
        url: controlador + 'obtenerProyectosExistentes',
        data: {
        },
        dataType: 'json',
        success: function (evt) {
            if (evt.Resultado == "1") {
                var htmlPy = dibujarTablaListadoPyE(evt.ListadoProyectos);
                $("#listadoProyectosAsignacion").html(htmlPy);

                //verifico si marco check global
                if (marcadoTodoCIO)
                    document.getElementById("checkTodo_Py").checked = true;

                $('#tablaPyE').dataTable({
                    "scrollY": 300,
                    "scrollX": false,
                    "sDom": 'ft',
                    "ordering": false,
                    "iDisplayLength": -1
                });

                abrirPopup("popupBusquedaPy");

                $('#btnAgregarPy').unbind();
                $('#btnAgregarPy').click(function () {
                    agregarProyecto();
                });

                //Toda la columna cambia (al escoger casilla cabecera)
                $('input[type=checkbox][name^="checkTodo_Py"]').unbind();
                $('input[type=checkbox][name^="checkTodo_Py"]').change(function () {
                    var valorCheck = $(this).prop('checked');
                    $("input[type=checkbox][id^=checkPy_]").each(function () {
                        $("#" + this.id).prop("checked", valorCheck);
                    });
                });

                //Verifico si el check cabecera debe pintarse o no al editar cualquier check hijo
                $('input[type=checkbox][name^="checkPy_"]').change(function () {
                    verificarCheckGrupalPy();
                });
            }
            else {
                mostrarMensaje('mensaje_popupProyecto', 'error', 'Error: ' + evt.Mensaje);
            }
        },
        error: function () {
            mostrarMensaje('mensaje_popupProyecto', 'error', 'Se ha producido un error.');
        }
    });
}

function verificarCheckGrupalPy() {
    //Empresas Interrupcion Suministro con check
    var val1 = 0;
    $("input[type=checkbox][id^=checkPy_]").each(function () {
        var IsCheckedIS = $("#" + this.id)[0].checked;
        if (IsCheckedIS) {

        } else {
            val1 = val1 + 1;
        }
    });

    var v = true;
    if (val1 > 0)
        v = false;

    $("#checkTodo_Py").prop("checked", v);
}

function dibujarTablaListadoPyE(lista) {

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="tablaPyE" >
       <thead>
           <tr style="height: 22px;">
               <th style='width:40px;'>
                    <input type="checkbox" class="chbx" name="checkTodo_Py" id="checkTodo_Py">
               </th>
               
               <th style='width:140px;'>Código</th>
               
               <th style='width:520px;'>Nombre del Proyecto (Extranet)</th>
            </tr>
        </thead>
        <tbody>
    `;

    var totalMarcados = 0;
    for (key in lista) {
        var item = lista[key];

        //Si el proyecto esta el el listado de la tabla, debe mostrar su check seleccionado
        const registro = listaProyectosEnMemoria.find(x => x.ftprycodi === item.Ftprycodi);
        var flagCheck = "";
        if (registro != null) {
            flagCheck = "checked";
            totalMarcados++;
        }

        cadena += `
            <tr>
                <td style='width:40px;'>                    
                    <input type="checkbox" class="chbx" name="checkPy_${item.Ftprycodi}" id="checkPy_${item.Ftprycodi}" value="${item.Ftprycodi}" ${flagCheck}>
                </td>
                
                <td style='width:140px;'>${item.Ftpryeocodigo}</td>
                
                <td style='width:520px;'>${item.Ftprynombre}</td>
               
           </tr >           
        `;
    }

    cadena += `                   
        </tbody >
    </table >
    `;
    if (lista.length == totalMarcados)
        marcadoTodoCIO = true;
    else
        marcadoTodoCIO = false;

    return cadena;
}


function agregarProyecto() {
    limpiarBarraMensaje("mensaje_popupBusquedaPy");

    var filtro = datosConfirmar();
    var msg = validarDatosConfirmar(filtro);

    if (msg == "") {
        var strIdsProyectos = filtro.seleccionados.join(",");
        $.ajax({
            type: 'POST',
            url: controlador + "obtenerDatosProysSel",
            data: {
                strIdsProyectos: strIdsProyectos
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    agregarProyectoAArrayProyectos(evt.ListadoProyectos);
                    mostrarProyectosExtranet(listaProyectosEnMemoria);

                    cerrarPopup('popupBusquedaPy');


                } else {
                    mostrarMensaje('mensaje_popupBusquedaPy', 'error', evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje_popupBusquedaPy', 'error', 'Ha ocurrido un error al agregar proyecto.');
            }
        });
    } else {
        mostrarMensaje('mensaje_popupBusquedaPy', 'alert', msg);
    }
}

function agregarProyectoAArrayProyectos(listadoProyectos) {

    for (key in listadoProyectos) {
        var objProyecto = listadoProyectos[key];

        //Si el proyecto ya esta en la lista memoria no lo agrego, solo los que no estan
        var flagYaEstaEnListaEnMemoria = evaluarExistenciaEnMemoriaCIO(objProyecto);

        if (!flagYaEstaEnListaEnMemoria) {
            let proy = {
                "feeprycodi": -1,

                "ftprycodi": objProyecto.Ftprycodi,
                "nombreEmpresa": objProyecto.Emprnomb,
                "codigoProy": objProyecto.Ftpryeocodigo,
                "nombProyExt": objProyecto.Ftprynombre,
            }

            listaProyectosEnMemoria.push(proy);
        }
    }
}

function evaluarExistenciaEnMemoriaCIO(objFtExtProyecto) {
    var salida;
    var idProy = objFtExtProyecto.Ftprycodi;
    const registro = listaProyectosEnMemoria.find(x => x.ftprycodi === idProy);

    if (registro != null) {
        salida = true;
    } else {
        salida = false;
    }

    return salida;
}

function datosConfirmar() {
    var filtro = {};

    let valoresCheck = [];

    $("input[type=checkbox][name^='checkPy_']:checked").each(function () {
        valoresCheck.push(this.value);
    });
    filtro.seleccionados = valoresCheck;

    return filtro;
}

function validarDatosConfirmar(datos) {

    var msj = "";

    if (datos.seleccionados.length == 0) {
        msj += "<p>Debe seleccionar mínimamente un Proyecto.</p>";
    }

    return msj;
}


function mostrarProyectosExtranet(arrayProyectos) {
    $("#listadoProyectosElementosExtranetAsignacion").html("");

    var htmlTabla = dibujarTablaListadoProyectos(arrayProyectos);
    $("#listadoProyectosElementosExtranetAsignacion").html(htmlTabla);

    var tamAlturaTablaPyE = 200;

    //primero generar datatable (esperar algunos milisegundos para que el div.html() se incruste totalmente en el body)
    setTimeout(function () {
        $('#tablaProyectos').dataTable({
            "scrollY": tamAlturaTablaPyE,
            "scrollX": false,
            "sDom": 't',
            "ordering": false,
            "iDisplayLength": -1
        });
    }, 150);
}


function dibujarTablaListadoProyectos(arrayProyectos) {

    var accion = parseInt($("#hdAccion").val());

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="tablaProyectos" style="width: 1200px;" >
       <thead>
           <tr style="height: 22px;">                             
               <th style='width:100px;'>Código</th>               
               <th style='width:480px;'>Nombre del Proyecto (Extranet)</th>
               <th style='width:60px;'>Detalle Equipo(s)</th>
    `;
    if (accion != VER) {
        cadena += `
                <th style='width:60px;'>Eliminar</th>
        `;
    }

    cadena += `
            </tr>
        </thead>
        <tbody>
    `;

    for (key in arrayProyectos) {
        var item = arrayProyectos[key];


        cadena += `
            <tr>                                
                <td style="">${item.codigoProy}</td>                
                <td style="">${item.nombProyExt}</td>
                <td style=''>
                    <a href="JavaScript:verDetallesEquipos(${item.feeprycodi},${item.ftprycodi});">${IMG_DETALLES}</a>
                </td>
        `;
        if (accion != VER) {
            cadena += `
                <td style=''>                    
                    <a href="JavaScript:eliminarProyecto(${item.feeprycodi},${item.ftprycodi});">${IMG_ELIMINAR}</a>
                </td>
            `;
        }

        cadena += `
           </tr >           
        `;
    }

    cadena += `                   
        </tbody >
    </table >
    `;

    return cadena;
}

function eliminarProyecto(idReg, idProy) {
    limpiarBarraMensaje("mensaje_popupProyecto");
    if (confirm('¿Desea eliminar el registro?')) {

        const registro = listaProyectosEnMemoria.find(x => x.feeprycodi === idReg && x.ftprycodi === idProy);

        if (registro != null) {

            const index = listaProyectosEnMemoria.map(x => x.ftprycodi).indexOf(idProy);
            const filaEliminada = listaProyectosEnMemoria.splice(index, 1);

            mostrarMensaje('mensaje_popupProyecto', 'exito', 'Registro eliminado satisfactoriamente.');
            mostrarProyectosExtranet(listaProyectosEnMemoria);

            //tambien quitamos los cambios pertenecientes al proyecto del arrayGlobal de cambios
            var lista = [];
            for (key in listaCambiosGlobalEtapaCIO) {
                var item = listaCambiosGlobalEtapaCIO[key];
                lista.push(item);
            }

            for (key in lista) {
                var item = lista[key];
                let miFtprycodi = item.ftprycodi;
                let miEmprcodi = item.emprcodi;
                let miEquicodi = item.equicodi;
                let miGrupocodi = item.grupocodi;

                if (miFtprycodi == idProy.toString()) {
                    var pos = listaCambiosGlobalEtapaCIO.findIndex(function (x) { return x.ftprycodi === miFtprycodi && x.emprcodi === miEmprcodi && x.equicodi === miEquicodi && x.grupocodi === miGrupocodi; });
                    const filaEliminada = listaCambiosGlobalEtapaCIO.splice(pos, 1);
                }
            }

        } else {
            mostrarMensaje('mensaje_popupProyecto', 'error', 'Ha ocurrido un error al eliminar registro.');
        }

    }
}

function verDetallesEquipos(idReg, idProy) {
    limpiarBarraMensaje("mensaje_popupProyecto");
    listaCambiosDetallesEtapaCIO = [];
    $("#btnActualizarDet").css("display", "none");

    var filtro = datosDetalleEqMO();
    var msg = validarFiltroPrecargar(filtro);

    if (msg == "") {
        $.ajax({
            type: 'POST',
            url: controlador + "ObtenerMOYEquiposAsociadosAlPy",
            data: {
                feeprycodi: idReg,
                ftprycodi: idProy,
                emprcodi: filtro.idEmpresa,
                idEtapa: filtro.idEtapa
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {

                    var htmlTabla = dibujarTablaDetallesRelEquipoMOPy(evt.ListadoRelacionEGP, idReg, idProy, filtro.idEmpresa, filtro.idEtapa);
                    $("#listadoDetalleEqMoDelProyecto").html(htmlTabla);

                    //primero generar datatable (esperar algunos milisegundos para que el div.html() se incruste totalmente en el body)
                    setTimeout(function () {
                        $('#TablaDetalleRelMoEPy').dataTable({
                            "destroy": "true",
                            "scrollY": 350,
                            "sDom": 'ft',
                            "ordering": false,
                            "iDisplayLength": -1
                        });
                    }, 150);

                    //luego de renderizado la tabla abrir popup
                    abrirPopup("popupRelEqMOConProyectos");

                    //Verifico si se debe mostrar el boton ACTUALIZAR DETALLE
                    $('input[type=checkbox][name^="chkFlagCIO_"]').change(function () {
                        verificarCambiosChecksEtapaCIO(this);
                    });

                    $('#btnActualizarDet').unbind();
                    $('#btnActualizarDet').click(function () {
                        var accion = parseInt($("#hdAccion").val());
                        if (accion != VER) {
                            guardarEnArrayGlobal();
                            cerrarPopup("popupRelEqMOConProyectos");
                            listaCambiosDetallesEtapaCIO = [];
                        } else {
                            mostrarMensaje('mensaje_popupRelEqMOProyecto', 'alert', "No tiene permiso para realizar cambios.");
                        }
                    });

                    $('#btnCancelarActDetCIO').unbind();
                    $('#btnCancelarActDetCIO').click(function () {
                        cerrarPopup('popupRelEqMOConProyectos');
                        listaCambiosDetallesEtapaCIO = [];
                    });

                } else {
                    mostrarMensaje('mensaje_popupProyecto', 'error', evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje_popupProyecto', 'error', 'Ha ocurrido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaje_popupProyecto', 'alert', msg);
    }
}

function datosDetalleEqMO() {
    var filtro = {};

    filtro.idEmpresa = parseInt($("#cbEmpresapop").val()) || 0;
    filtro.idEtapa = parseInt($("#cbEtapapop").val()) || 0;

    return filtro;
}

function validarFiltroPrecargar(datos) {

    var msj = "";

    if (datos.idEmpresa <= 0) {
        msj += "<p>Debe escoger una empresa.</p>";
    }

    return msj;

}


function dibujarTablaDetallesRelEquipoMOPy(lista, feeprycodi, ftprycodi, emprcodi, idEtapa) {

    var accion = parseInt($("#hdAccion").val());

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="TablaDetalleRelMoEPy" cellspacing="0" style="width: 1200px" >
        <thead>            
            <tr>
                <th style="width: 80px">Código</th>
                <th style="width: 250px">Empresa <br>Titular</th>
                <th style="width: 250px">Empresa <br>Copropietaria</th>
                <th style="width: 120px">Tipo de <br>equipo / categoría</th>
                <th style="width: 150px">Nombre</th>
                <th style="width: 200px">Ubicación</th>
                <th style="width: 100px">Estado equipo</th>
                <th style="width: 80px">Equipo <br>otra etapa</th>
                <th style="width: 80px">Auditoría</th>
            </tr>
        </thead>
        <tbody>
    `;

    var fila = 1;
    for (key in lista) {
        var item = lista[key];

        var deshabilitarCheck = "";
        if (accion == VER)
            deshabilitarCheck = "disabled";


        var miEquicodi = item.Equicodi != null ? item.Equicodi.toString() : "Null";
        var miGrupocodi = item.Grupocodi != null ? item.Grupocodi.toString() : "Null";
        var miFtprycodi = item.Ftprycodi.toString();
        var miEmprcodi = item.Emprcodi.toString();

        var flagValCheck = "";
        //Verifico en el arrayGlobal si el check para el registro ha sufrido cambios
        const existeRegistro = listaCambiosGlobalEtapaCIO.find(x => x.equicodi === miEquicodi && x.grupocodi === miGrupocodi && x.ftprycodi === miFtprycodi && x.emprcodi === miEmprcodi);
        //Si existe en el arrayGlobal de cambios, el valor del check lo toma de ese cambio
        if (existeRegistro != null) {
            flagValCheck = existeRegistro.flagequipootroetapa;

        } else { // si no esta en la lista de cambios toma el valor de la BD
            flagValCheck = item.FlagEquipoOtroEtapa;
        }

        var flagChecked = "";
        if (flagValCheck == "S")
            flagChecked = "checked";

        cadena += `

           <tr>
                <td style="">${item.Codigo}</td>
                <td style="">${item.EmpresaNomb}</td>
                <td style="">${item.EmpresaCoNomb}</td>
                <td style="">${item.Tipo}</td>
                <td style="">${item.EquipoNomb}</td>
                <td style="">${item.Ubicacion}</td>
                <td style="">${item.EstadoDesc}</td>
                <td style=""><input ${deshabilitarCheck} type="checkbox" id="chkFlagCIO_${fila}_${flagValCheck}_${item.Ftprycodi}_${item.Emprcodi}_${miEquicodi}_${miGrupocodi}" name="chkFlagCIO_${fila}_${flagValCheck}_${item.Ftprycodi}_${item.Emprcodi}_${miEquicodi}_${miGrupocodi}" value="${item.FlagEquipoOtroEtapa}" ${flagChecked}> </td> 
        `;
        if (item.TieneCambios == 1) {
            cadena += `
                <td style=""><a href="JavaScript:mostrarAuditoriaCheckCIO(${feeprycodi}, ${ftprycodi}, ${emprcodi}, ${item.Equicodi}, ${item.Grupocodi},  ${idEtapa});">${IMG_VER}</a></td> 
            `;
        } else {
            cadena += `
                <td style=""></td>
            `;
        }
        cadena += `
           </tr>           
        `;
        fila++;
    }
    cadena += "</tbody></table>";

    return cadena;
}


function verificarCambiosChecksEtapaCIO(miObj) {

    var valorCheck = $(miObj).prop('checked');
    let miValChkFin = valorCheck ? "S" : "N";
    var idCheck = miObj.id;

    if (idCheck != null) {
        const myArray = idCheck.split("_");
        let miFila = myArray[1];
        let miValChkIni = myArray[2];
        let miFtprycodi = myArray[3];
        let miEmprcodi = myArray[4];
        let miEquicodi = myArray[5];
        let miGrupocodi = myArray[6];

        //verifico si coincide el checked con el valIni
        if (miValChkFin == miValChkIni) {
            //quito del listado de cambios
            const existeRegistro = listaCambiosDetallesEtapaCIO.find(x => x.equicodi === miEquicodi && x.grupocodi === miGrupocodi && x.ftprycodi === miFtprycodi && x.emprcodi === miEmprcodi);

            if (existeRegistro != null) {
                //Elimino del array
                const index = listaCambiosDetallesEtapaCIO.map(x => x.posIni).indexOf(existeRegistro.posIni);
                const filaEliminada = listaCambiosDetallesEtapaCIO.splice(index, 1);
            }
        }
        else {
            //agrego al array de cambios en cualquier posicion
            let registro = {
                "posIni": miFila,
                "flagequipootroetapa": miValChkFin,
                "ftprycodi": miFtprycodi,
                "emprcodi": miEmprcodi,
                "equicodi": miEquicodi,
                "grupocodi": miGrupocodi,
            }
            listaCambiosDetallesEtapaCIO.push(registro);
        }
    }

    //Una vez calculado el array de cambios, verifico si se muestra o no el boton ACTUALIZAR DETALLES
    if (listaCambiosDetallesEtapaCIO.length > 0) {
        $("#btnActualizarDet").css("display", "table-row");
    } else {
        $("#btnActualizarDet").css("display", "none");
    }

    //si es edicion deshabilito
    var accion = parseInt($("#hdAccion").val());
    if (accion == VER)
        $("#btnActualizarDet").css("display", "none");

}

function guardarEnArrayGlobal() {
    var listaCambios = listaCambiosDetallesEtapaCIO;

    for (key in listaCambios) {
        var regCambio = listaCambios[key];

        let miFtprycodi = regCambio.ftprycodi;
        let miEmprcodi = regCambio.emprcodi;
        let miEquicodi = regCambio.equicodi;
        let miGrupocodi = regCambio.grupocodi;

        //reviso si el registro existe en el arrayGlobal
        const registro = listaCambiosGlobalEtapaCIO.find(x => x.ftprycodi === miFtprycodi && x.emprcodi === miEmprcodi && x.equicodi === miEquicodi && x.grupocodi === miGrupocodi);

        //si ya esta en el arrayGlobal, lo quito (xq agegar un cambio en una lista de cambios es lo mismo que quitarlo dado que el cambio puede tomar 2 unicos valores: "S" o "N" )
        if (registro != null) {
            var pos = listaCambiosGlobalEtapaCIO.findIndex(function (x) { return x.ftprycodi === miFtprycodi && x.emprcodi === miEmprcodi && x.equicodi === miEquicodi && x.grupocodi === miGrupocodi; });
            const filaEliminada = listaCambiosGlobalEtapaCIO.splice(pos, 1);
        } else { //si no esta en el arrayGlobal, lo agrego
            listaCambiosGlobalEtapaCIO.push(regCambio);
        }
    }
}

function FormatearListaProyectos() {
    var lstSalida = [];
    var lista = listaProyectosEnMemoria;

    for (key in lista) {
        var item = lista[key];

        var miFeeprycodi = item.feeprycodi;
        var miFtprycodi = item.ftprycodi;
        var miNombreEmpresa = item.nombreEmpresa;
        var miCodigoProy = item.codigoProy;
        var miNombProyExt = item.nombProyExt;

        var val1 = miFeeprycodi != null ? (isNaN(miFeeprycodi) ? null : parseInt(miFeeprycodi)) : null;
        var val2 = miFtprycodi != null ? (isNaN(miFtprycodi) ? null : parseInt(miFtprycodi)) : null;
        var val3 = miNombreEmpresa != null ? (miNombreEmpresa != undefined ? miNombreEmpresa : "") : "";
        var val4 = miCodigoProy != null ? (miCodigoProy != undefined ? miCodigoProy : "") : "";
        var val5 = miNombProyExt != null ? (miNombProyExt != undefined ? miNombProyExt : "") : "";

        let objFtExtEtempdetpry = {
            "Feeprycodi": val1,
            "Ftprycodi": val2,
            "NombreEmpresa": val3,
            "CodigoProy": val4,
            "NombProyExt": val5
        }

        lstSalida.push(objFtExtEtempdetpry);
    }

    return lstSalida;
}

function FormatearListaCambiosCIO() {
    var lstSalida = [];
    var lista = listaCambiosGlobalEtapaCIO;

    for (key in lista) {
        var item = lista[key];

        var miFtprycodi = item.ftprycodi;
        var miEmprcodi = item.emprcodi;
        var miEquicodi = item.equicodi;
        var miGrupocodi = item.grupocodi;
        var miFlag = item.flagequipootroetapa;

        var val1 = parseInt(miFtprycodi);
        var val2 = parseInt(miEmprcodi);
        var val3 = miEquicodi != null ? (isNaN(miEquicodi) ? null : parseInt(miEquicodi)) : null;
        var val4 = miGrupocodi != null ? (isNaN(miGrupocodi) ? null : parseInt(miGrupocodi)) : null;
        var val5 = miFlag != null ? miFlag : "";

        let objRelacionEGP = {
            "Ftprycodi": val1,
            "Emprcodi": val2,
            "Equicodi": val3,
            "Grupocodi": val4,
            "FlagEquipoOtroEtapa": val5
        }

        lstSalida.push(objRelacionEGP);
    }

    return lstSalida;
}

function FormatearListaElementos() {
    var lstSalida = [];
    var lista = listaEquiposEnMemoria;

    for (key in lista) {
        var item = lista[key];

        var miCodigo = item.Codigo;
        var miEmprcodi = item.Emprcodi;
        var miEmpresaCoNomb = item.EmpresaCoNomb;
        var miEmpresaNomb = item.EmpresaNomb;
        var miEquicodi = item.Equicodi;
        var miEquipoNomb = item.EquipoNomb;
        var miEstadoDesc = item.EstadoDesc;
        var miEstadoReg = item.EstadoReg;
        var miFeeeqcodi = item.Feeeqcodi;
        var miFlagEquipoOtroEtapa = item.FlagEquipoOtroEtapa;
        var miGrupocodi = item.Grupocodi;
        var miTieneCambios = item.TieneCambios;
        var miTipo = item.Tipo;
        var miUbicacion = item.Ubicacion;
        var miEditoFlag = item.EditoFlag;
        var miFlagCentralCOES = item.FlagCentralCOES;

        var val1 = miCodigo != null ? (miCodigo != undefined ? miCodigo : "") : "";
        var val2 = miEmprcodi != null ? (isNaN(miEmprcodi) ? null : parseInt(miEmprcodi)) : null;
        var val3 = miEmpresaCoNomb != null ? (miEmpresaCoNomb != undefined ? miEmpresaCoNomb : "") : "";
        var val4 = miEmpresaNomb != null ? (miEmpresaNomb != undefined ? miEmpresaNomb : "") : "";
        var val5 = miEquicodi != null ? (isNaN(miEquicodi) ? null : parseInt(miEquicodi)) : null;
        var val6 = miEquipoNomb != null ? (miEquipoNomb != undefined ? miEquipoNomb : "") : "";
        var val7 = miEstadoDesc != null ? (miEstadoDesc != undefined ? miEstadoDesc : "") : "";
        var val8 = miEstadoReg != null ? (miEstadoReg != undefined ? miEstadoReg : "") : "";
        var val9 = miFeeeqcodi != null ? (isNaN(miFeeeqcodi) ? null : parseInt(miFeeeqcodi)) : null;
        var val10 = miFlagEquipoOtroEtapa != null ? (miFlagEquipoOtroEtapa != undefined ? miFlagEquipoOtroEtapa : "") : "";
        var val11 = miGrupocodi != null ? (isNaN(miGrupocodi) ? null : parseInt(miGrupocodi)) : null;
        var val12 = miTieneCambios != null ? (isNaN(miTieneCambios) ? null : parseInt(miTieneCambios)) : null;
        var val13 = miTipo != null ? (miTipo != undefined ? miTipo : "") : "";
        var val14 = miUbicacion != null ? (miUbicacion != undefined ? miUbicacion : "") : "";
        var val15 = miEditoFlag != null ? (miEditoFlag != undefined ? miEditoFlag : "") : "";
        var val16 = miFlagCentralCOES != null ? (miFlagCentralCOES != undefined ? miFlagCentralCOES : "") : "";

        let objRelacionEGP = {
            "Codigo": val1,
            "Emprcodi": val2,
            "EmpresaCoNomb": val3,
            "EmpresaNomb": val4,
            "Equicodi": val5,
            "EquipoNomb": val6,
            "EstadoDesc": val7,
            "EstadoReg": val8,
            "Feeeqcodi": val9,
            "FlagEquipoOtroEtapa": val10,
            "Grupocodi": val11,
            "TieneCambios": val12,
            "Tipo": val13,
            "Ubicacion": val14,
            "EditoFlag": val15,
            "FlagCentralCOES": val16,
        }

        lstSalida.push(objRelacionEGP);
    }

    return lstSalida;
}

function mostrarAuditoriaCheckCIO(feeprycodi, ftprycodi, emprcodi, equicodi, grupocodi, idEtapa) {

    limpiarBarraMensaje("mensaje_popupRelEqMOProyecto");
    limpiarDatosPopupAuditoria();

    var msg = "";

    if (msg == "") {

        $.ajax({
            type: 'POST',
            url: controlador + "obtenerDatosAuditoriaCIO",
            data: {
                feeprycodi: feeprycodi,
                ftprycodi: ftprycodi,
                emprcodi: emprcodi,
                equicodi: equicodi,
                grupocodi: grupocodi,
                idEtapa: idEtapa
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {

                    mostrarDatosPEAuditoriaCIO(evt.DetalleCIO);

                } else {
                    mostrarMensaje('mensaje_popupRelEqMOProyecto', 'error', evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje_popupRelEqMOProyecto', 'error', 'Ha ocurrido un error al mostrar auditoria.');
            }
        });
    } else {
        mostrarMensaje('mensaje_popupRelEqMOProyecto', 'alert', msg);
    }
}

function limpiarDatosPopupAuditoria() {
    $("#usuCreacionVal").html("");
    $("#fecCreacionVal").html("");
    $("#usuModificacionVal").html("");
    $("#fecModificacionVal").html("");
}

function mostrarDatosPEAuditoriaCIO(obj) {
    $("#usuCreacionVal").html(obj.Feepequsucreacion);
    $("#fecCreacionVal").html(obj.FechaCreacionDesc);
    $("#usuModificacionVal").html(obj.Feepequsumodificacion);
    $("#fecModificacionVal").html(obj.FechaModificacionDesc);

    abrirPopup('popupAuditoria');
}



function agregarEquipo() {
    limpiarBarraMensaje("mensaje_popupProyecto");
    limpiarBarraMensaje("mensaje_popupBusquedaEq");

    $("#origen_1").prop("checked", true);
    $("#cbFamiliaEquipo").val("-1");

    $("#cbCategoriaGrupo").hide();
    $("#cbFamiliaEquipo").show();

    $("#cbUbicacion").val("-1");

    mostrarElementosSegunFiltro();

}



function mostrarElementosSegunFiltro() {
    listaElementosSegunFiltro = [];

    var tipo = document.querySelector('input[name="origen"]:checked').value;

    var idEmpresa = parseInt($('#cbEmpresapop').val());
    var idUbicacion = parseInt($('#cbUbicacion').val());
    var idElemento;
    if (tipo == TIPO_EQUIPO)
        idElemento = $("#cbFamiliaEquipo").val();
    else {
        if (tipo == CATEGORIA_GRUPO)
            idElemento = $("#cbCategoriaGrupo").val();
    }

    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerListadoElementos',
        data: {
            tipo: tipo,
            idElemento: idElemento,
            emprcodi: idEmpresa,
            idUbicacion: idUbicacion
        },
        dataType: 'json',
        success: function (evt) {
            if (evt.Resultado == "1") {
                listaElementosSegunFiltro = evt.ListadoRelacionEGP;

                var htmlPy = dibujarTablaBusquedaRelacionElementos(evt.ListadoRelacionEGP);
                $("#listadoBusquedaElementosO").html(htmlPy);

                //primero generar datatable (esperar algunos milisegundos para que el div.html() se incruste totalmente en el body)
                setTimeout(function () {
                    $('#TablaDetalleRelO').dataTable({
                        "scrollY": 300,
                        "scrollX": false,
                        "sDom": 'ft',
                        "ordering": false,
                        "iDisplayLength": -1
                    });
                }, 150);

                //luego de renderizado la tabla abrir popup
                abrirPopup("popupBusquedaEq");

                $('#btnAgregarEq').unbind();
                $('#btnAgregarEq').click(function () {
                    agregarElemento(tipo, idElemento, idEmpresa);
                });

                $('#btnCancelarEq').unbind();
                $('#btnCancelarEq').click(function () {
                    cerrarPopup('popupBusquedaEq');
                    listaElementosSegunFiltro = [];
                });

                //Toda la columna cambia (al escoger casilla cabecera)
                $('input[type=checkbox][name^="checkTodo_E"]').unbind();
                $('input[type=checkbox][name^="checkTodo_E"]').change(function () {
                    var valorCheck = $(this).prop('checked');
                    $("input[type=checkbox][id^=checkE_]").each(function () {
                        $("#" + this.id).prop("checked", valorCheck);
                    });
                });

                //Verifico si el check cabecera debe pintarse o no al editar cualquier check hijo
                $('input[type=checkbox][name^="checkE_"]').change(function () {
                    verificarCheckGrupalE();
                });

            }
            else {
                mostrarMensaje('mensaje_popupProyecto', 'error', 'Error: ' + evt.Mensaje);
            }
        },
        error: function () {
            mostrarMensaje('mensaje_popupProyecto', 'error', 'Se ha producido un error.');
        }
    });
}

function dibujarTablaBusquedaRelacionElementos(lista) {

    //Verifico si el check global esta marcado o no
    var totalMarcados = 0;
    var marcaCheckGlobal;
    for (key in lista) {
        var item = lista[key];

        const registro = listaEquiposEnMemoria.find(x => x.Codigo === item.Codigo);
        if (registro != null) {
            totalMarcados++;
        }
    }

    var flagCheckTodo = "";
    if (lista.length == totalMarcados) {
        marcaCheckGlobal = true;
        flagCheckTodo = "checked";
    }
    else {
        marcaCheckGlobal = false;
    }

    //armo tabla
    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="TablaDetalleRelO" cellspacing="0"   style="width: 1200px" >
        <thead>            
            <tr style="height: 30px;">
                <th style='width:40px; white-space: inherit; '>
                    <input type="checkbox" class="chbx" name="checkTodo_E" id="checkTodo_E" ${flagCheckTodo}>
                </th>
                <th style=" width:  80px">Código</th>
                <th style=" width: 250px">Empresa</th>
                <th style=" width: 250px">Empresa <br>Copropietaria</th>
                <th style=" width: 120px">Tipo de <br>equipo / categoría</th>
                <th style=" width: 150px">Nombre</th>
                <th style=" width: 200px">Ubicación</th>
                <th style=" width:  100px">Estado <br>equipo</th>
                
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];

        //Si el proyecto esta el el listado de la tabla, debe mostrar su check seleccionado
        //const registro = listaEquiposEnMemoria.find(x => x.ftprycodi === item.Ftprycodi);
        const registro = listaEquiposEnMemoria.find(x => x.Codigo === item.Codigo);
        var flagCheck = "";
        if (registro != null) {
            flagCheck = "checked";
        }

        var miEquicodi = item.Equicodi != null ? item.Equicodi.toString() : "Null";
        var miGrupocodi = item.Grupocodi != null ? item.Grupocodi.toString() : "Null";

        cadena += `

           <tr >
                <td style='white-space: inherit; '>
                    <input type="checkbox" class="chbx" name="checkE_${miEquicodi}_${miGrupocodi}" id="checkE_${miEquicodi}_${miGrupocodi}" value="${item.Codigo}" ${flagCheck}>
                </td>                
                <td style=" white-space: inherit;">${item.Codigo}</td>
                <td style=" white-space: inherit;">${item.EmpresaNomb}</td>
                <td style=" white-space: inherit;">${item.EmpresaCoNomb}</td>
                <td style=" white-space: inherit;">${item.Tipo}</td>
                <td style=" white-space: inherit;">${item.EquipoNomb}</td>
                <td style=" white-space: inherit;">${item.Ubicacion}</td>
                <td style=" white-space: inherit;">${item.EstadoDesc}</td>
               
        `;

        cadena += `
           </tr >           
        `;

    }
    cadena += "</tbody></table>";

    return cadena;
}

function verificarCheckGrupalE() {
    //Empresas Interrupcion Suministro con check
    var val1 = 0;
    $("input[type=checkbox][id^=checkE_]").each(function () {
        var IsCheckedIS = $("#" + this.id)[0].checked;
        if (IsCheckedIS) {

        } else {
            val1 = val1 + 1;
        }
    });

    var v = true;
    if (val1 > 0)
        v = false;

    if (!v)
        $("#checkTodo_E").removeAttr('checked');
    else
        $("#checkTodo_E").prop("checked", v);
}

function agregarElemento(tipo, idElemento, emprcodi) {
    limpiarBarraMensaje("mensaje_popupBusquedaEq");

    var filtro = datosElementoAgregar();
    var msg = validarDatosElementoAgregar(filtro);

    if (msg == "") {
        var strIdsSeleccionados = filtro.seleccionados.join(",");
        var idEtapa = parseInt($("#cbEtapapop").val());
        $.ajax({
            type: 'POST',
            url: controlador + "obtenerDatosElementosSel",
            data: {
                strIdsSeleccionados: strIdsSeleccionados,
                tipo: tipo,
                idElemento: idElemento,
                emprcodi: emprcodi,
                idEtapa: idEtapa
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {

                    agregarEquipoAArrayEquipos(evt.ListadoRelacionEGP);
                    mostrarListadoEquiposO(listaEquiposEnMemoria);

                    cerrarPopup('popupBusquedaEq');
                    listaElementosSegunFiltro = [];

                    //Almaceno info de cambios de los check (CASO CUANDO AGREAGO NUEVOS EQUIPOS)
                    $('input[type=checkbox][name^="checkFL_"]').change(function () {
                        setearModificacion(this);
                    });

                    $('input[type=checkbox][name^="checkCentral_"]').change(function () {
                        setearModifCheckCentral(this);
                    });

                } else {
                    mostrarMensaje('mensaje_popupBusquedaEq', 'error', evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje_popupBusquedaEq', 'error', 'Ha ocurrido un error al agregar equipo/grupo.');
            }
        });
    } else {
        mostrarMensaje('mensaje_popupBusquedaEq', 'alert', msg);
    }
}

function setearModificacion(miObj) {
    var valorCheck = $(miObj).prop('checked');
    let miValChkFin = valorCheck ? "S" : "N";
    var idCheck = miObj.id;

    const arraySel = idCheck.split("_");
    var id = arraySel[1];
    if (id != null) {

        const registro = listaEquiposEnMemoria.find(x => x.Codigo === id);

        var flagIni = registro.FlagEquipoOtroEtapa;
        var editoFIni = registro.EditoFlag;

        var listado = listaEquiposEnMemoria;
        for (key in listado) {
            var obje = listado[key];

            //Edito algunos de sus valores
            if (obje.Codigo == id) {
                if (obje.EditoFlag == "S") {
                    obje.EditoFlag = "N";
                    obje.FlagEquipoOtroEtapa = flagIni == "S" ? "N" : (flagIni == "N" ? "S" : "");
                } else {
                    obje.EditoFlag = "S";
                    obje.FlagEquipoOtroEtapa = flagIni == "S" ? "N" : (flagIni == "N" ? "S" : "");
                }
            }
        }
    }
}

function setearModifCheckCentral(miObj) {
    var valorCheck = $(miObj).prop('checked');
    let miValChkFin = valorCheck ? "S" : "N";
    var idCheck = miObj.id;

    const arraySel = idCheck.split("_");
    var id = arraySel[1];
    if (id != null) {

        const registro = listaEquiposEnMemoria.find(x => x.Codigo === id);
        var flagIni = registro.FlagCentralCOES;

        var listado = listaEquiposEnMemoria;
        for (key in listado) {
            var obje = listado[key];

            //Edito algunos de sus valores
            if (obje.Codigo == id) {
                obje.FlagCentralCOES = flagIni == "S" ? "N" : (flagIni == "N" ? "S" : miValChkFin);
            }
        }
    }
}

function agregarEquipoAArrayEquipos(listado) {


    for (key in listado) {
        var objRelacionEGP = listado[key];
        var idE = objRelacionEGP.Codigo;

        //Si el proyecto ya esta en la lista memoria no lo agrego, solo los que no estan
        var flagYaEstaEnListaEnMemoria = evaluarExistenciaEnMemoriaO(objRelacionEGP);

        if (!flagYaEstaEnListaEnMemoria) {
            let proy = {
                "Feeeqcodi": -1,
                "Codigo": objRelacionEGP.Codigo,  //string
                "Emprcodi": objRelacionEGP.Emprcodi, //string
                "EmpresaCoNomb": objRelacionEGP.EmpresaCoNomb, //string
                "EmpresaNomb": objRelacionEGP.EmpresaNomb, //string
                "Equicodi": objRelacionEGP.Equicodi, //entero o nulo
                "Grupocodi": objRelacionEGP.Grupocodi, //entero o nulo
                "Tipo": objRelacionEGP.Tipo, //string
                "Ubicacion": objRelacionEGP.Ubicacion, //string
                "EstadoDesc": objRelacionEGP.EstadoDesc, //string
                "EquipoNomb": objRelacionEGP.EquipoNomb, //string
                "EstadoReg": "A",  // A: Agregado, E:Eliminado, M:Mismo
                //"Emprcodi": objRelacionEGP.Emprcodi,
                "TieneCambios": objRelacionEGP.TieneCambios,
                "FlagEquipoOtroEtapa": objRelacionEGP.FlagEquipoOtroEtapa,
                "FlagCentralCOES": objRelacionEGP.FlagCentralCOES,
                "Famcodi": objRelacionEGP.Famcodi,
                "EditoFlag": ""
            }

            listaEquiposEnMemoria.push(proy);
        }

    }
}


function evaluarExistenciaEnMemoriaO(objRelacionEGP) {
    var salida;
    var idE = objRelacionEGP.Codigo;
    const registro = listaEquiposEnMemoria.find(x => x.Codigo === idE);

    if (registro != null) {
        salida = true;
    } else {
        salida = false;
    }

    return salida;
}

function datosElementoAgregar() {
    var filtro = {};

    let valoresCheck = [];

    $("input[type=checkbox][name^='checkE_']:checked").each(function () {
        valoresCheck.push(this.value);
    });
    filtro.seleccionados = valoresCheck;

    return filtro;
}

function validarDatosElementoAgregar(datos) {

    var msj = "";

    if (datos.seleccionados.length == 0) {
        msj += "<p>Debe ingresar al menos un 'Equipo'.</p>";
    }

    return msj;
}


function mostrarListadoEquiposO(arrayElementos) {
    $("#listadoProyectosElementosExtranetAsignacion").html("");

    var htmlTabla = dibujarTablaListadoElementos(arrayElementos);
    $("#listadoProyectosElementosExtranetAsignacion").html(htmlTabla);

    var tamAlturaTablaPyE = 200;

    //primero generar datatable (esperar algunos milisegundos para que el div.html() se incruste totalmente en el body)
    setTimeout(function () {
        $('#tablaElementosO').dataTable({
            "scrollY": tamAlturaTablaPyE,
            "scrollX": false,
            "sDom": 't',
            "ordering": false,
            "iDisplayLength": -1
        });
    }, 150);
}


function dibujarTablaListadoElementos(arrayElementos) {

    var accion = parseInt($("#hdAccion").val());

    var lista = arrayElementos;
    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="tablaElementosO" cellspacing="0"  >
        <thead>            
            <tr style="height: 30px;">
                
                <th style="width: 80px">Código</th>
                <th style="width: 250px">Empresa</th>
                <th style="width: 250px">Empresa <br>Copropietaria</th>
                <th style="width: 120px">Tipo de <br>equipo / categoría</th>
                <th style="width: 150px">Nombre</th>
                <th style="width: 200px">Ubicación</th>
                <th style="width: 80px">Estado <br>equipo</th>
                <th style="width: 80px">Central <br>COES</th>
                <th style="width: 80px">Equipo <br>otra etapa</th>
                
    `;
    if (accion != VER) {
        cadena += `
                <th style='width:80px;'>Eliminar</th>
        `;
    }

    cadena += `
                <th style='width:80px;'>Auditoria</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];

        var flagValCheck = item.FlagEquipoOtroEtapa;
        var flagCentralValCheck = item.FlagCentralCOES;
        var flagCentralChecked = "";
        if (flagCentralValCheck == "S")
            flagCentralChecked = "checked";

        var flagChecked = "";
        if (flagValCheck == "S")
            flagChecked = "checked";

        var deshabilitarCheck = "";
        if (accion == VER)
            deshabilitarCheck = "disabled";

        var htmlCheckCentralCoes = "";
        if (item.Famcodi != null && (item.Famcodi == 39 || item.Famcodi == 4 || item.Famcodi == 37 || item.Famcodi == 5)) {
            htmlCheckCentralCoes = `
                                <input ${deshabilitarCheck} type="checkbox" class="chbx" name="checkCentral_${item.Codigo}" id="checkCentral_${item.Codigo}" value="${item.Codigo}" ${flagCentralChecked}>
                            `;
        }

        cadena += `
           <tr >                               
                <td style="">${item.Codigo}</td>
                <td style="">${item.EmpresaNomb}</td>
                <td style="">${item.EmpresaCoNomb}</td>
                <td style="">${item.Tipo}</td>
                <td style="">${item.EquipoNomb}</td>
                <td style="">${item.Ubicacion}</td>
                <td style="">${item.EstadoDesc}</td>
                <td style=''>
                    ${htmlCheckCentralCoes}
                </td>
                <td style=''>
                    <input ${deshabilitarCheck} type="checkbox" class="chbx" name="checkFL_${item.Codigo}" id="checkFL_${item.Codigo}" value="${item.Codigo}" ${flagChecked}>
                </td>
        `;
        if (accion != VER) {
            cadena += `
                <td style=''>
                    <a href="JavaScript:eliminarElemento(${item.Codigo});">${IMG_ELIMINAR}</a>
                </td>
        `;
        }
        if (item.TieneCambios == 1) {
            cadena += `
                <td style=''>
                    <a href="JavaScript:mostrarAuditoriaCheckO(${item.Feeeqcodi});">${IMG_VER}</a>
                </td>            
            `;
        }
        else {
            cadena += `
                <td></td>
            `;
        }
        cadena += `
           </tr >           
        `;

    }
    cadena += "</tbody></table>";


    return cadena;

}

function eliminarElemento(codigo) {

    var strCodigo = codigo.toString();
    limpiarBarraMensaje("mensaje_popupProyecto");
    if (confirm('¿Desea eliminar el registro?')) {

        const registro = listaEquiposEnMemoria.find(x => x.Codigo === strCodigo);

        if (registro != null) {

            const index = listaEquiposEnMemoria.map(x => x.Codigo).indexOf(strCodigo);
            const filaEliminada = listaEquiposEnMemoria.splice(index, 1);

            mostrarMensaje('mensaje_popupProyecto', 'exito', 'Registro eliminado satisfactoriamente.');
            mostrarListadoEquiposO(listaEquiposEnMemoria);

        } else {
            mostrarMensaje('mensaje_popupProyecto', 'error', 'Ha ocurrido un error al eliminar equipo/grupo.');
        }

    }
}

function mostrarAuditoriaCheckO(feeeqcodi) {

    limpiarBarraMensaje("mensaje_popupProyecto");
    limpiarDatosPopupAuditoria();

    var msg = "";

    if (msg == "") {

        $.ajax({
            type: 'POST',
            url: controlador + "obtenerDatosAuditoriaO",
            data: {
                feeeqcodi: feeeqcodi
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {

                    mostrarDatosPEAuditoriaO(evt.DetalleO);

                } else {
                    mostrarMensaje('mensaje_popupProyecto', 'error', evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje_popupProyecto', 'error', 'Ha ocurrido un error al mostrar auditoria.');
            }
        });
    } else {
        mostrarMensaje('mensaje_popupProyecto', 'alert', msg);
    }
}

function mostrarDatosPEAuditoriaO(obj) {
    $("#usuCreacionVal").html(obj.Feeequsucreacion);
    $("#fecCreacionVal").html(obj.FechaCreacionDesc);
    $("#usuModificacionVal").html(obj.Feeequsumodificacion);
    $("#fecModificacionVal").html(obj.FechaModificacionDesc);

    abrirPopup('popupAuditoria');
}

///UTIL////
function abrirPopup(contentPopup, accion) {

    setTimeout(function () {
        $("#" + contentPopup).bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
}


function cerrarPopup(id) {
    $("#" + id).bPopup().close()
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


function deshabilitarSelect(id) {
    document.getElementById(id).disabled = true;
    $("#" + id).css("background", "#F2F4F3");
}

function habilitarSelect(id) {
    document.getElementById(id).disabled = false;
    $("#" + id).css("background", "white");
}

