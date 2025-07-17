var controlador = siteRoot + 'Equipamiento/FTProyecto/';

const NUEVO = 1;
const EDITAR = 2;
const VER = 3;
const IMG_EDITAR = '<img src="' + siteRoot + 'Content/Images/btn-edit.png" alt="Editar Proyecto" title="Editar Proyecto" width="19" height="19" style="">';
const IMG_VER = '<img src="' + siteRoot + 'Content/Images/btn-open.png" alt="Ver Proyecto" title="Ver Proyecto" width="19" height="19" style="">';
const IMG_ELIMINAR = '<img src="' + siteRoot + 'Content/Images/btn-cancel.png" alt="Dar de Baja Proyecto" title="Dar de Baja Proyecto"width="19" height="19" style="">';
const IMG_ACTIVAR = '<img src="' + siteRoot + 'Content/Images/btn-ok.png" alt="Activar Proyecto" title="Activar Proyecto"width="19" height="19" style="">';

const COLOR_BAJA = "#FFDDDD";

$(function () {
    $('#txtDesde').Zebra_DatePicker({
        format: "d/m/Y",
        pair: $('#txtHasta'),
        direction: false,
    });

    $('#txtHasta').Zebra_DatePicker({
        format: "d/m/Y",
        pair: $('#txtDesde'),
        direction: true,
    });

    $('#cbEmpresa').multipleSelect({
        width: '450px',
        filter: true,
        single: true,
        onClose: function () {
        }
    });
    $("#cbEmpresa").multipleSelect("setSelects", [-1]);

    $('#btnBuscar').click(function () {
        mostrarListadoProyectos();
    });

    $('#btnNuevo').click(function () {
        $("#hdAccion").val(NUEVO);
        limpiarPopupNuevo();
        abrirPopup("popupProyecto");       
    });
 
    $('#btnBuscarEstEO').click(function () {
        limpiarBarraMensaje("mensaje_popupProyecto");
        var siNoSel = document.querySelector('input[name="PNEstudioEo"]:checked');
        if (siNoSel != null) {
            if (siNoSel.value == "S") { //solo si se selecciona SI
                limpiarPopupBusquedaConEO();
                abrirPopup("popupBusquedaEO");
                cargarEmpresasSegunTipoConEO();
            }
        }
    });

    $('#cbPBEOTipoEmpresa').change(function () {
        cargarEmpresasSegunTipoConEO();
    });

    $('#btnBuscarEmpresa').click(function () {
        limpiarBarraMensaje("mensaje_popupProyecto");
        var siNoSel = document.querySelector('input[name="PNEstudioEo"]:checked');
        if (siNoSel != null) {
            if (siNoSel.value == "N") { //solo si se selecciona NO
                limpiarPopupBusquedaSinEO();
                abrirPopup("popupBusquedaEmp");
                cargarEmpresasSegunTipoSinEO();
            }
        }
    });

    $('#cbPBEmpTipoEmpresa').change(function () {
        cargarEmpresasSegunTipoSinEO();
    });

    $('input[type=radio][name=PNEstudioEo]').change(function () {
        if (this.value == 'S') {
            $("#btnBuscarEstEO").css("display", "block");
            $("#btnBuscarEmpresa").css("display", "none"); 
            $("#campoCodigo").html("Código Estudio (EO):");
            $("#hdConEstudioEo").val("S");
            
        }
        else if (this.value == 'N') {
            $("#btnBuscarEstEO").css("display", "none");
            $("#btnBuscarEmpresa").css("display", "block");
            $("#campoCodigo").html("Código Proyecto:");
            $("#hdConEstudioEo").val("N");
        }
        $("#txtPNEmpresa").val("");
        $("#txtPNCodigo").val("");
        $("#txtPNNombProy").val("");
        $("#txtPNNombProyEx").val("");

        deshabilitarTextField("txtPNEmpresa");
        deshabilitarTextField("txtPNCodigo");
        deshabilitarTextField("txtPNNombProy");
        deshabilitarTextField("txtPNNombProyEx");
    });

    $('#GuardarPy').click(function () {
        guardarProyecto();
    });

    $('#btnExportar').click(function () {
        exportarProyectos();
    });


    ////////////////////////////

    $('#GuardarPE').click(function () {
        guardarPtoEntrega(NUEVO, null);
    });
    
    mostrarListadoProyectos();
});

function mostrarListadoProyectos() {
    limpiarBarraMensaje("mensaje");

    var filtro = datosFiltro();
    var msg = validarDatosFiltroBusqueda(filtro);

    if (msg == "") {
        $.ajax({
            type: 'POST',
            url: controlador + "listarProyectos",
            data: {
                empresa: filtro.empresa,
                rangoIni: filtro.rangoIni,
                rangoFin: filtro.rangoFin
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    var htmlPy = dibujarTablaListadoProyectos(evt.ListadoProyectos);
                    $("#listado").html(htmlPy);

                    $('#tablaProyectos').dataTable({
                        "scrollY": 480,
                        "scrollX": false,
                        "sDom": 'ft',
                        "ordering": false,
                        "iDisplayLength": -1
                    });

                    //Guardo los datos para la exportacion
                    $("#hdEmpresaEscogida").val($("#cbEmpresa").val());
                    $("#hdRangoIniEscogido").val($("#txtDesde").val());
                    $("#hdRangoFinEscogido").val($("#txtHasta").val());

                } else {
                    mostrarMensaje('mensaje', 'error', evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    } else {
        mostrarMensaje('mensaje', 'error', msg);
    }
}

function datosFiltro() {
    var filtro = {}; 
    
    var finicio = $('#txtDesde').val();
    var ffin = $('#txtHasta').val();
   
    filtro.empresa = $('#cbEmpresa').val();    
    filtro.rangoIni = finicio;
    filtro.rangoFin = ffin;

    return filtro;
}

function validarDatosFiltroBusqueda(datos) {

    var msj = "";

    if (datos.empresa == "") {
        msj += "<p>Debe escoger una empresa correcta.</p>";
    }

    if (datos.rangoIni == "") {
        if (datos.rangoFin == "") {
            msj += "<p>Debe escoger un rango inicial y final correctos.</p>";
        } else {
            msj += "<p>Debe escoger una fecha inicial correcta.</p>";
        }
    } else {
        if (datos.rangoFin == "") {
            msj += "<p>Debe escoger una fecha final correcta.</p>";
        }
        else {
            if (convertirFecha(datos.rangoIni) > convertirFecha(datos.rangoFin)) {
                msj += "<p>Debe escoger un rango correcto, la fecha final debe ser posterior o igual a la fecha inicial.</p>";
            }
        }
    }

    return msj;

}

function convertirFecha(fecha) {
    var arrayFecha = fecha.split('/');
    var dia = arrayFecha[0];
    var mes = arrayFecha[1];
    var anio = arrayFecha[2];

    var salida = anio + mes + dia;
    return salida;
}

function dibujarTablaListadoProyectos(listaProyectos) {

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="tablaProyectos" >
       <thead>
           <tr style="height: 22px;">
               <th style='width:90px;'>Acciones</th>
               <th>Empresa</th>
               <th>Código Estudio (EO)</th>
               <th>Nombre del Proyecto</th>
                <th>Nombre del Proyecto (Extranet)</th>
               <th>Estado</th>
               <th>Usuario Creación</th>
               <th>Fecha de Creación</th>
                <th>Usuario Modificación</th>
               <th>Fecha de Modificación</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in listaProyectos) {
        var item = listaProyectos[key];
        var colorFila = "";

        if (item.Ftpryestado == "B")
            colorFila = COLOR_BAJA;

        cadena += `
            <tr>
                <td style='width:90px; background: ${colorFila}'>
                    <a href="JavaScript:mostrarProyecto(${item.Ftprycodi},${VER} );">${IMG_VER}</a>
        `;
        if (item.Ftpryestado == "B") {
            cadena += `
                    <a href="JavaScript:activarProyecto(${item.Ftprycodi});">${IMG_ACTIVAR}</a>
            `;
        } else {
            cadena += `
                    <a href="JavaScript:mostrarProyecto(${item.Ftprycodi},${EDITAR});">${IMG_EDITAR}</a>
                    <a href="JavaScript:eliminarProyecto(${item.Ftprycodi});">${IMG_ELIMINAR}</a>
            `;
        }
        cadena += `                    
                </td>
                <td style="background: ${colorFila}">${item.Emprnomb}</td>
                <td style="background: ${colorFila}">${item.Ftpryeocodigo}</td>
                <td style="background: ${colorFila}">${item.Ftpryeonombre}</td>
                <td style="background: ${colorFila}">${item.Ftprynombre}</td>
                <td style="background: ${colorFila}">${item.FtpryestadoDesc}</td>
                <td style="background: ${colorFila}">${item.Ftpryusucreacion}</td>
                <td style="background: ${colorFila}">${item.FechaCreaciónDesc}</td>
                <td style="background: ${colorFila}">${item.Ftpryusumodificacion}</td>
                <td style="background: ${colorFila}">${item.FechaModificacionDesc}</td>
           </tr >           
        `;
    }

    cadena += `                   
        </tbody >
    </table >
    `;

    return cadena;
}

function limpiarPopupNuevo() {
    limpiarBarraMensaje("mensaje_popupProyecto");

    $("#rdPNSi").prop("checked", true);
    $("#rdPNNo").prop("checked", false);
    $("#txtPNEmpresa").val("");
    $("#txtPNCodigo").val("");
    $("#txtPNNombProy").val("");
    $("#txtPNNombProyEx").val("");

    $("#hdEsteocodiUsado").val("");

    deshabilitarTextField("txtPNEmpresa");
    deshabilitarTextField("txtPNCodigo");
    deshabilitarTextField("txtPNNombProy");
    deshabilitarTextField("txtPNNombProyEx");

    $("#campoCodigo").html("Código Estudio (EO):");

    $("#bloqueRadio").css("display", "contents");
    $("#bloqueBotones").css("display", "block");
    $("#tituloPopup").html("<span>Nuevo Proyecto</span>");
}

function limpiarPopupBusquedaConEO() {
    limpiarBarraMensaje("mensaje_popupBusquedaEO");

    $("#cbPBEOTipoEmpresa").val("-2");
    $("#cbPBEOEmpresa").val("-5");
    
}

function limpiarPopupBusquedaSinEO() {
    limpiarBarraMensaje("mensaje_popupBusquedaEmp");

    $("#cbPBEmpTipoEmpresa").val("-2");
    $("#cbPBEmpEmpresa").val("-5");

}

function cargarEmpresasSegunTipoConEO() {
    limpiarBarraMensaje("mensaje_popupBusquedaEO");
    $("#listadoEstudio").html("");

    var valSel = $("#cbPBEOTipoEmpresa").val();
    var tipo;

    if (isNaN(valSel)) {
        tipo = -3;
    } else {
        tipo = parseInt($("#cbPBEOTipoEmpresa").val())
    }
      
    
    $("#cbPBEOEmpresa").empty(); 
    

    if (tipo == -3) {
        mostrarMensaje('mensaje_popupBusquedaEO', 'alert', "Seleccionar un tipo de empresa correcto.");
    } else {

        $.ajax({
            type: 'POST',
            url: controlador + 'CargarEmpresasXTipo',
            data: {
                tipoEmpresa: tipo
            },

            success: function (evt) {
                if (evt.Resultado != "-1") {
                    var numEmpresas = evt.ListaEmpresas.length;
                    if (numEmpresas > 0) {
                       
                        //Lleno una variable con todos los datos de las empresas   
                        $('#cbPBEOEmpresa').get(0).options[0] = new Option("--  Seleccione Empresa  --", "-5");
                        $.each(evt.ListaEmpresas, function (i, item) { //item es string                           
                            $('#cbPBEOEmpresa').get(0).options[$('#cbPBEOEmpresa').get(0).options.length] = new Option(item.Emprnomb, item.Emprcodi); // listaIdentificadores es List<string>, si es objeto usar asi item.Pfrpernombre
                        });

                        $('#cbPBEOEmpresa').multipleSelect({
                            //width: '450px',
                            filter: true,
                            single: true,
                            onClose: function () {
                            }
                        });
                        $("#cbPBEOEmpresa").multipleSelect("setSelects", [-5]);

                        
                        $('#cbPBEOEmpresa').change(function () {
                            mostrarListadoEstudioEO();
                        });

                        $('#btnConfirmarEO').unbind();
                        $('#btnConfirmarEO').click(function () {
                            confirmarEstudioEO();
                        });
                        
                    } else {
                        $('#cbPBEOEmpresa').get(0).options[0] = new Option("--  Seleccione Empresa  --", "-5");
                    }
                } else {
                    mostrarMensaje('mensaje_popupBusquedaEO', 'error', "Ha ocurrido un error: " + evt.Mensaje);
                   
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje_popupBusquedaEO', 'error', 'Se ha producido un error al cargar el listado de empresas para el tipo escogido.');

            }
        });
    }
}

function cargarEmpresasSegunTipoSinEO() {
    limpiarBarraMensaje("mensaje_popupBusquedaEmp");
    

    var valSel = $("#cbPBEmpTipoEmpresa").val();
    var tipo;

    if (isNaN(valSel)) {
        tipo = -3;
    } else {
        tipo = parseInt($("#cbPBEmpTipoEmpresa").val())
    }


    $("#cbPBEmpEmpresa").empty();


    if (tipo == -3) {
        mostrarMensaje('mensaje_popupBusquedaEmp', 'alert', "Seleccionar un tipo de empresa correcto.");
    } else {

        $.ajax({
            type: 'POST',
            url: controlador + 'CargarEmpresasXTipo',
            data: {
                tipoEmpresa: tipo
            },

            success: function (evt) {
                if (evt.Resultado != "-1") {
                    var numEmpresas = evt.ListaEmpresas.length;
                    if (numEmpresas > 0) {


                        //Lleno una variable con todos los datos de las empresas     
                        $('#cbPBEmpEmpresa').get(0).options[0] = new Option("--  Seleccione Empresa  --", "-5");
                        $.each(evt.ListaEmpresas, function (i, item) { //item es string                           
                            $('#cbPBEmpEmpresa').get(0).options[$('#cbPBEmpEmpresa').get(0).options.length] = new Option(item.Emprnomb, item.Emprcodi); // listaIdentificadores es List<string>, si es objeto usar asi item.Pfrpernombre
                        });

                        $('#cbPBEmpEmpresa').multipleSelect({
                            //width: '450px',
                            filter: true,
                            single: true,
                            onClose: function () {
                            }
                        });
                        $("#cbPBEmpEmpresa").multipleSelect("setSelects", [-5]);                        

                        $('#btnSeleccionarEmp').unbind();
                        $('#btnSeleccionarEmp').click(function () {
                            confirmarEmpresa();
                        });

                    } else {
                        $('#cbPBEmpEmpresa').get(0).options[0] = new Option("--  Seleccione Empresa  --", "-5");
                    }
                } else {
                    mostrarMensaje('mensaje_popupBusquedaEmp', 'error', "Ha ocurrido un error: " + evt.Mensaje);

                }
            },
            error: function (err) {
                mostrarMensaje('mensaje_popupBusquedaEmp', 'error', 'Se ha producido un error al cargar el listado de empresas para el tipo escogido.');

            }
        });
    }
}

function mostrarListadoEstudioEO() {
    limpiarBarraMensaje("mensaje_popupBusquedaEO");
    
    var empresa = $('#cbPBEOEmpresa').val() ;
    var val = isNaN(empresa);
    if (!val) {
        $.ajax({
            type: 'POST',
            url: controlador + "listarEstudiosEo",
            data: {
                idEmpresa: empresa
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    var htmlLEO = dibujarTablaListadoEstudio(evt.ListadoEstudiosEo);
                    $("#listadoEstudio").html(htmlLEO);

                    $('#tablaEstudio').dataTable({
                        "scrollY": 150,
                        "scrollX": false,
                        "sDom": 'ft',
                        "ordering": false,
                        "iDisplayLength": -1
                    });

                } else {
                    mostrarMensaje('mensaje_popupBusquedaEO', 'error', evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje_popupBusquedaEO', 'error', 'Ha ocurrido un error al mostrar estudios EO.');
            }
        });
    } 
}

function dibujarTablaListadoEstudio(listaEstudio) {

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="tablaEstudio" >
       <thead>
           <tr style="height: 22px;">
               <th style='width:90px;'>Sel.</th>
               <th>Código Estudio</th>
               <th>Nombre Estudio</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in listaEstudio) {
        var item = listaEstudio[key];

        cadena += `
            <tr>
                <td style='width:90px;'>        
                    <input type="radio" id="rdEstudio_${item.Esteocodi}" name="rdEstudioEO" value="${item.Esteocodi}">
                </td>
                <td>${item.Esteocodiusu}</td>
                <td>${item.Esteonomb}</td>
               
           </tr >           
        `;
    }

    cadena += `                   
        </tbody >
    </table >
    `;

    return cadena;
}

function confirmarEstudioEO() {
    limpiarBarraMensaje("mensaje_popupBusquedaEO");
    var filtro = datosConfirmar();
    var msg = validarDatosConfirmar(filtro);
    
    if (msg == "") {
        var idEstudioEO = parseInt(filtro.seleccionado.value) || 0;
        $.ajax({
            type: 'POST',
            url: controlador + "obtenerDatoEstudioEO",
            data: {
                esteocodi: idEstudioEO
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    limpiarDatosProyectoNuevo();
                    completarDatosEstudioEnFormulario(evt.EstudioEO);
                    habilitarTextField("txtPNNombProyEx");
                    
                    cerrarPopup('popupBusquedaEO');
                    

                } else {
                    mostrarMensaje('mensaje_popupBusquedaEO', 'error', evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje_popupBusquedaEO', 'error', 'Ha ocurrido un error al mostrar estudios EO.');
            }
        });
    } else {
        mostrarMensaje('mensaje_popupBusquedaEO', 'alert', msg);
    }
}

function confirmarEmpresa() {
    limpiarBarraMensaje("mensaje_popupBusquedaEmp");
    var filtro = datosEmpConfirmar();
    var msg = validarDatosEmpConfirmar(filtro);

    if (msg == "") {
        var idEmpresa = parseInt(filtro.empresa) || 0;
        $.ajax({
            type: 'POST',
            url: controlador + "obtenerDatoEmpresa",
            data: {
                emprcodi: idEmpresa
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    limpiarDatosProyectoNuevo();
                    completarDatosEmpresaEnFormulario(evt.Empresa);
                    habilitarTextField("txtPNNombProy");
                    habilitarTextField("txtPNNombProyEx");
                    cerrarPopup('popupBusquedaEmp');
                    

                } else {
                    mostrarMensaje('mensaje_popupBusquedaEmp', 'error', evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje_popupBusquedaEmp', 'error', 'Ha ocurrido un error al mostrar empresas.');
            }
        });
    } else {
        mostrarMensaje('mensaje_popupBusquedaEmp', 'alert', msg);
    }
}

function limpiarDatosProyectoNuevo() {
    $("#txtPNEmpresa").val("");
    $("#txtPNCodigo").val("");
    $("#txtPNNombProy").val("");
    $("#txtPNNombProyEx").val("");
    
    $("#hdEsteocodiUsado").val("");
}

function habilitarTextField(id) {
    document.getElementById(id).disabled = false;
    $("#" + id).css("background", "white");
}

function deshabilitarTextField(id) {
    document.getElementById(id).disabled = true;
    $("#" + id).css("background", "#F2F4F3");
}

function datosConfirmar() {
    var filtro = {};

    var radioSel = document.querySelector('input[name="rdEstudioEO"]:checked');

    filtro.seleccionado = radioSel;
    filtro.empresa = $("#cbPBEOEmpresa").val();
    
    return filtro;
}

function validarDatosConfirmar(datos) {

    var msj = "";

    if (datos.empresa == -5) {
        msj += "<p>Debe seleccionar una empresa.</p>";
    } else {
        if (datos.seleccionado == null) {
            msj += "<p>Debe seleccionar un Código de Estudio EO.</p>";
        }
    }

    return msj;

}

function datosEmpConfirmar() {
    var filtro = {};

    var idEmpr = parseInt($("#cbPBEmpEmpresa").val());
    filtro.empresa = idEmpr;

    return filtro;
}

function validarDatosEmpConfirmar(datos) {

    var msj = "";

    if (datos.empresa == -5) {
        msj += "<p>Debe seleccionar una empresa.</p>";
    }

    return msj;

}

function completarDatosEstudioEnFormulario(objEstudioEO) {
    $("#txtPNEmpresa").val(objEstudioEO.Emprnomb);
    $("#txtPNCodigo").val(objEstudioEO.Esteocodiusu);
    $("#txtPNNombProy").val(objEstudioEO.Esteonomb);
    $("#hdIdEmpresa").val(objEstudioEO.Emprcoditp);
    $("#hdEsteocodiUsado").val(objEstudioEO.Esteocodi);
    $("#hdConEstudioEo").val("S");
    
}

function completarDatosEmpresaEnFormulario(objEmpresa) {
    $("#txtPNEmpresa").val(objEmpresa.Emprnomb);
    $("#txtPNCodigo").val(objEmpresa.Codigo);
    $("#hdIdEmpresa").val(objEmpresa.Emprcodi);
    $("#hdConEstudioEo").val("N");
}

function guardarProyecto() {
    var accion = $("#hdAccion").val();
    if (accion == EDITAR || accion == NUEVO) {
        limpiarBarraMensaje("mensaje_popupProyecto");

        var filtro = datosProyecto();
        var msg = validarDatosProyecto(filtro);

        if (msg == "") {

            $.ajax({
                type: 'POST',
                url: controlador + "guardarProyecto",
                data: {
                    empresaNomb: filtro.empresa,
                    codigo: filtro.codigo,
                    proyNomb: filtro.nombProyecto,
                    proyExtNomb: filtro.nombProyectoExt,
                    esteocodi: filtro.idCodigoEo,
                    empresaId: filtro.idEmpresa,
                    conEstudio: filtro.ConEstudioEo,
                    accion: filtro.Accion,
                    ftprycodi: filtro.IdProyecto
                },

                success: function (evt) {
                    if (evt.Resultado != "-1") {

                        mostrarListadoProyectos();
                        mostrarMensaje('mensaje', 'exito', "La operacion se realizó satisfactoriamente.");
                        cerrarPopup("popupProyecto");

                    } else {
                        mostrarMensaje('mensaje_popupProyecto', 'error', evt.Mensaje);
                    }
                },
                error: function (err) {
                    mostrarMensaje('mensaje_popupProyecto', 'error', 'Ha ocurrido un error al mostrar empresas.');
                }
            });
        } else {
            mostrarMensaje('mensaje_popupProyecto', 'alert', msg);
        }
    } else
        mostrarMensaje('mensaje_popupProyecto', 'alert', "No tiene permiso para realizar cambios.");
}

function datosProyecto() {
    var filtro = {};
   
    filtro.empresa = $("#txtPNEmpresa").val();
    filtro.codigo = $("#txtPNCodigo").val();
    filtro.nombProyecto = $("#txtPNNombProy").val();
    filtro.nombProyectoExt = $("#txtPNNombProyEx").val();

    filtro.idCodigoEo = parseInt($("#hdEsteocodiUsado").val()) || -3;
    filtro.idEmpresa = parseInt($("#hdIdEmpresa").val()) || -3;
    filtro.ConEstudioEo =  $("#hdConEstudioEo").val();
    filtro.Accion = $("#hdAccion").val();
    filtro.IdProyecto = parseInt($("#hdIdProyecto").val()) || -3;

    return filtro;
}

function validarDatosProyecto(datos) {

    var msj = "";

    if (datos.ConEstudioEo.trim() == "N") {   //Sin codigo EO
        if (datos.empresa.trim() == "") {
            msj += "<p>Debe seleccionar una empresa.</p>";
        }

        if (datos.codigo.trim() == "") {
            msj += "<p>No hay dato en el campo 'Código Proyecto'.</p>";
        }

        if (datos.nombProyecto.trim() == "") {
            msj += "<p>Debe ingresar 'Nombre proyecto'.</p>";
        } else {
            if (datos.nombProyecto.trim().length > 200) {
                msj += "<p>El campo 'Nombre Proyecto' no debe exceder los 200 caracteres.</p>";
            }
        }

        if (datos.nombProyectoExt.trim() == "") {
            msj += "<p>Debe ingresar 'Nombre proyecto (Extranet)'.</p>";
        } else {
            if (datos.nombProyectoExt.trim().length > 200) {
                msj += "<p>El campo 'Nombre Proyecto (Extranet)' no debe exceder los 200 caracteres.</p>";
            }
        }
    } else {
        if (datos.ConEstudioEo.trim() == "S") {   //Con codigo EO
            if (datos.empresa.trim() == "") {
                msj += "<p>Debe seleccionar una empresa.</p>";
            }

            if (datos.codigo.trim() == "") {
                msj += "<p>Debe seleccionar 'Código Estudio (EO)'.</p>";
            }

            if (datos.nombProyecto.trim() == "") {
                msj += "<p>No hay dato en el campo 'Nombre Proyecto'.</p>";
            } 

            if (datos.nombProyectoExt.trim() == "") {
                msj += "<p>Debe ingresar 'Nombre proyecto (Extranet)'.</p>";
            } else {
                if (datos.nombProyectoExt.trim().length > 200) {
                    msj += "<p>El campo 'Nombre Proyecto (Extranet)' no debe exceder los 200 caracteres.</p>";
                }
            }
        }
    }

    return msj;

}

function exportarProyectos() {
    limpiarBarraMensaje("mensaje");

    var filtro = datosExportarFiltro();
    var msg = validarDatosFiltroBusqueda(filtro);
    if (msg == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'exportarProyectos',
            data: {
                empresa: filtro.empresa,
                rangoIni: filtro.rangoIni,
                rangoFin: filtro.rangoFin
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
    } else {
        mostrarMensaje('mensaje', 'error', msg);
    }
}

function datosExportarFiltro() {
    var filtro = {};

    filtro.empresa = $('#hdEmpresaEscogida').val();
    filtro.rangoIni = $('#hdRangoIniEscogido').val();
    filtro.rangoFin = $('#hdRangoFinEscogido').val();

    return filtro;
}

function mostrarProyecto(idProyecto, accion) {
    limpiarBarraMensaje("mensaje"); 
    limpiarBarraMensaje("mensaje_popupProyecto"); 
    $.ajax({
        type: 'POST',
        url: controlador + "detallarProyecto",
        data: { ftprycodi: idProyecto },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                prepararDetallesPopup(evt.Proyecto, accion);
                setearInfoProyecto(evt.Proyecto, accion);

                abrirPopup("popupProyecto");

            } else {
                mostrarMensaje('mensaje', 'error', evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function prepararDetallesPopup(objProyecto, accion) {
    var campo = "Código Proyecto:";
    if (objProyecto.Esteocodi != null && objProyecto.Esteocodi != "") {
        campo = " Código Estudio (EO):";
    }

    $("#bloqueRadio").css("display", "none");
    $("#campoCodigo").html(campo);
    
    if (accion == VER) {
        $("#tituloPopup").html("<span>Detalle del Proyecto</span>");
        deshabilitarTextField("txtPNEmpresa");
        deshabilitarTextField("txtPNCodigo");
        deshabilitarTextField("txtPNNombProy");
        deshabilitarTextField("txtPNNombProyEx");
        $("#bloqueBotones").css("display", "none");
        
    } else {
        if (accion == EDITAR) {
            $("#tituloPopup").html("<span>Edición del Proyecto</span>");
            deshabilitarTextField("txtPNEmpresa");
            deshabilitarTextField("txtPNCodigo");

            if (objProyecto.Esteocodi != null && objProyecto.Esteocodi != "")
                deshabilitarTextField("txtPNNombProy");
            else
                habilitarTextField("txtPNNombProy");

            habilitarTextField("txtPNNombProyEx");
            $("#bloqueBotones").css("display", "block");
        }
    }
}

function setearInfoProyecto(objProyecto,accion) {
    

    $("#txtPNEmpresa").val(objProyecto.Emprnomb);
    $("#txtPNCodigo").val(objProyecto.Ftpryeocodigo);
    $("#txtPNNombProy").val(objProyecto.Ftpryeonombre);
    $("#txtPNNombProyEx").val(objProyecto.Ftprynombre);

    var esteocodi = "";
    var conEstudio = "N";
    if (objProyecto.Esteocodi != null) {
        esteocodi = objProyecto.Esteocodi;
        conEstudio = "S";
    }
    
    $("#hdEsteocodiUsado").val(esteocodi);
    $("#hdIdEmpresa").val(objProyecto.Emprcodi);
    $("#hdConEstudioEo").val(conEstudio);
    $("#hdAccion").val(accion);
    $("#hdIdProyecto").val(objProyecto.Ftprycodi);
}

function eliminarProyecto(ftprycodi) {
    limpiarBarraMensaje("mensaje");
    if (confirm('¿Desea eliminar el registro?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'EliminarProyecto',
            data: {
                ftprycodi: ftprycodi
            },
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado == "1") {
                    mostrarListadoProyectos();
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

function activarProyecto(ftprycodi) {
    limpiarBarraMensaje("mensaje");
    if (confirm('¿Desea activar el registro?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'activarProyecto',
            data: {
                ftprycodi: ftprycodi
            },
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado == "1") {
                    mostrarListadoProyectos();
                    mostrarMensaje('mensaje', 'exito', 'Se actualizó el registro correctamente.');
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