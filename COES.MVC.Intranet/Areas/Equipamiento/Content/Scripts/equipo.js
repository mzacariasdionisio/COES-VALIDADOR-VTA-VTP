var controlador = siteRoot + 'Equipamiento/equipo/';
const COLOR_BAJA = "#FFDDDD";
const IMG_VER = '<img src="' + siteRoot + 'Content/Images/btn-open.png" alt="Ver Envío" width="19" height="19" style="">';
const IMG_ELIMINAR = '<img src="' + siteRoot + 'Content/Images/btn-cancel.png" alt="Ver Envío" width="19" height="19" style="">';
const IMG_ACTIVAR = '<img src="' + siteRoot + 'Content/Images/btn-ok.png" alt="Activar Envío" width="19" height="19" style="">';
const IMG_ADD = '<img src="' + siteRoot + 'Content/Images/btn-add.png" alt="Agregar Empresa Copropietaria" width="19" height="19" style="">';
const IMG_HISTORIAL_AUDITORIA_CAMBIO = '<img src="' + siteRoot + 'Content/Images/Visualizar.png" width="19" height="19" style="">';

var listaProyectosEquipoMemoria = [];
var listaEmpresaLineaMemoria = [];

$(function () {
    $('#cbTipoEmpresa').val(-2);
    $('#cbEmpresa').val(-2);
    $('#cbTipoEquipo').val(-2);
    $('#cbEstado').val(' ');
    //cargarEmpresas();
    //buscarEquipos();

    $("#btnBuscar").persisteBusqueda("#frmBusquedaEquipo", buscarEquipos,
        [
            { principal: "cbTipoEmpresa", dependiente: "cbEmpresa", accion: cargarEmpresas }
        ]);

    $('#btnBuscar').click(function () {
        buscarEquipos();
    });
    $('#btnNuevo').click(function () {
        NuevoEquipo();
    });
    $('#cbTipoEmpresa').change(function () {
        cargarEmpresas();
        return false;
    });
    $('#btnExportar').click(function () {
        exportarListadoEquipos();
    });
    $(window).resize(function () {
        $('#listado').css("width", $('#mainLayout').width() + "px");
    });
    $("#popupDetalleEquipo").addClass("general-popup");
    $("#popupNuevoEquipo").addClass("general-popup");
    $("#popupEditEquipo").addClass("general-popup");

    $('#btnDescargar').click(function () {
        plantillaEquipos();
    });
    $('#btnImportar').click(function () {
        window.location.href = controlador + 'EquiposImportacion';
    });

    //Relación de Equipos y proyectos
    $('#btnDescargarAsigP').click(function () {
        plantillaRelEquipoProyecto();
    });
    $('#btnImportarAsigP').click(function () {
        window.location.href = controlador + 'ImportacionRelEquipoProyecto';
    });
});

function plantillaEquipos() {

    $.ajax({
        type: 'POST',
        url: controlador + 'ExportarEquipos',
        dataType: 'json',
        success: function (evt) {
            if (evt.Resultado != "-1") {
                window.location = controlador + "AbrirArchivo?file=" + evt.NombreArchivo;
            }
            else {
                alert(evt.StrMensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error de exportación');
        }
    });
}

mostrarExitoOperacion = function () {
    $('#mensaje').removeClass("action-error");
    $('#mensaje').addClass("action-exito");
    $('#mensaje').text("La operación se realizó con exito...!");
    $('#mensaje').css("display", "block");
};

mostrarError = function () {
    $('#mensaje').removeClass("action-exito");
    $('#mensaje').addClass("action-error");
    $('#mensaje').text("Ha ocurrido un error");
    $('#mensaje').css("display", "block");
};
cargarEmpresas = function () {
    return $.ajax({
        type: 'GET',
        url: controlador + '/CargarEmpresas',
        dataType: 'json',
        data: { idTipoEmpresa: $('#cbTipoEmpresa').val() },
        cache: false,
        success: function (aData) {
            $('#cbEmpresa').get(0).options.length = 0;
            $('#cbEmpresa').get(0).options[0] = new Option("TODOS", "-2");
            $.each(aData, function (i, item) {
                $('#cbEmpresa').get(0).options[$('#cbEmpresa').get(0).options.length] = new Option(item.Text, item.Value);
            });
        },
        error: function () {
            mostrarError();
        }
    });
};
buscarEquipos = function () {
    $('#mensaje').css("display", "none");
    pintarPaginado();
    mostrarListado(1);
};
pintarPaginado = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "PaginadoEquipos",
        data: $('#frmBusquedaEquipo').serialize(),
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            mostrarError();
        }
    });
};
mostrarListado = function (nroPagina) {
    $('#hfNroPagina').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controlador + "ListadoEquipos",
        data: $('#frmBusquedaEquipo').serialize(),
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 50
            });
        },
        error: function () {
            mostrarError();
        }
    });
};
pintarBusqueda = function (nroPagina) {
    mostrarListado(nroPagina);
};
ocultarMensaje = function () {
    $('#mensaje').css("display", "none");
};
mostrarDetalleEquipo = function (equicodi) {
    $.ajax({
        type: 'POST',
        url: controlador + "DetalleEquipo",
        data: {
            iEquipo: equicodi
        },
        success: function (evt) {
            $('#detalleEquipo').html(evt);
            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popupDetalleEquipo').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);

        },
        error: function () {
            mostrarError();
        }
    });
};
validarEquiposDependientesActivos = function (equicodi, equiestado) {
    var resultado = "";

    $.ajax({
        type: 'POST',
        url: controlador + "ValidarCambioEstado",
        dataType: 'json',
        async: false,
        data: {
            equicodi: equicodi,
            equiestado: equiestado
        },
        success: function (res) {
            resultado = res.success;
        }
    });

    if (resultado == 1)
        return true;
    else
        return false;

};
NuevoEquipo = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "NuevoEquipo",
        success: function (evt) {
            $('#nuevoEquipo').html(evt);
            $('#mensaje').css("display", "none");
            $("#cbEstadoNuevoEquipo").val("P"); //Por defecto Proyecto
            setTimeout(function () {
                $('#popupNuevoEquipo').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);

        },
        error: function () {
            mostrarError();
        }
    });
};
guardarEquipo = function () {
    var serializedData = $('#frmNewEquipo').serializeArray();
    // Convertir la cadena serializada en un objeto
    var formData = {};
    $.each(serializedData, function (i, field) {
        formData[field.name] = field.value;
    });

    if (formData['Famcodi'] == 42 || formData['Famcodi'] == 19) {
        formData['Equipadre'] = formData['Equipadre2']
        if (formData['Equipadre'] == 0) {
            alert('El dato “Es componente” es obligatorio.')
            return;
        }
    }
    var newSerializedData = $.param(formData);

    if (confirm('¿Está seguro que desea guardar el equipo?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'GuardarEquipo',
            dataType: 'json',
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            data: newSerializedData,
            cache: false,
            success: function (resultado) {
                if (resultado == 1) {
                    alert("Registro exitoso");
                    mostrarExitoOperacion();
                    $('#popupNuevoEquipo').bPopup().close();
                    buscarEquipos();
                } else {
                    alert("Hubo un error en la solicitud");
                    mostrarError();
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
};
editarEquipo = function (equicodi) {
    $.ajax({
        type: 'POST',
        url: controlador + "EditarEquipo",
        data: {
            iEquipo: equicodi
        },
        success: function (evt) {
            $('#editarEquipo').html(evt);
            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popupEditEquipo').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);

        },
        error: function () {
            mostrarError();
        }
    });
};
actualizarEquipo = function () {
    var equipo = $('#hdnEquipoCodigo').val();
    var estado = $('#cbEstadoEditEquipo').val();
    var serializedData = $('#frmEditEquipo').serializeArray();
    // Convertir la cadena serializada en un objeto
    var formData = {};
    $.each(serializedData, function (i, field) {
        formData[field.name] = field.value;
    });

    if (formData['Famcodi'] == 42 || formData['Famcodi'] == 19) {
        formData['Equipadre'] = formData['Equipadre2']
        if (formData['Equipadre'] == 0) {
            alert('El dato “Es componente” es obligatorio.')
            return;
        }
    }
    var newSerializedData = $.param(formData);

    if (validarEquiposDependientesActivos(equipo, estado)) {

        if (confirm('¿Está seguro que desea actualizar el equipo?')) {
            $.ajax({
                type: 'POST',
                url: controlador + 'ActualizarEquipo',
                dataType: 'json',
                contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                data: newSerializedData,
                cache: false,
                success: function (resultado) {
                    if (resultado == 1) {
                        alert("Modificación exitoso");

                        mostrarExitoOperacion();
                        $('#popupEditEquipo').bPopup().close();
                        buscarEquipos();
                    } else {
                        alert("Hubo un error en la solicitud");
                        mostrarError();
                    }

                },
                error: function () {
                    mostrarError();
                }
            });
        }
    }
    else {
        alert('Antes de actualizar el estado de este equipo, se debe actualizar el estado a los equipos dependientes de este.')
        return false;
    }

};
propiedadesEquipo = function (equicodi) {
    location.href = controlador + "EquipoPropiedadesSheet?iEquipo=" + equicodi;
};

editarCoordenada = function (equicodi, emprnomb, famnomb, equinomb) {

    $('#popupMapa').bPopup({
        content: 'iframe',
        contentContainer: '#contenidoMapa',
        loadUrl: controlador + 'mapa?idEquipo=' + equicodi + "&emprnomb=" + emprnomb + "&famnomb=" + famnomb + "&equinomb=" + equinomb
    });
};
exportarListadoEquipos = function () {
    $('#mensaje').css("display", "none");
    $.ajax({
        type: 'POST',
        url: controlador + "ExportarListaEquipamiento",
        data: $('#frmBusquedaEquipo').serialize(),
        dataType: 'json',
        cache: false,
        success: function (resultado) {
            if (resultado == 1) {
                location.href = controlador + "DescargarListaEquipos";
            } else {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });
};

///////////////////////////  Relación de Equipo y Proyecto EO //////////////////////////////////

function plantillaRelEquipoProyecto() {

    $.ajax({
        type: 'POST',
        url: controlador + 'ExportarPlantillaRelEquipoProyecto',
        dataType: 'json',
        success: function (evt) {
            if (evt.Resultado != "-1") {
                window.location = controlador + "AbrirArchivo?file=" + evt.NombreArchivo;
            }
            else {
                alert(evt.StrMensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error de exportación');
        }
    });
}




function asignarEquipoExtranetFT(idEquipo) {
    limpiarBarraMensaje("mensaje");
    limpiarBarraMensaje("mensaje_popupAsignarEquiposFT");

    $("#seccionEmpCop").html("");

    $("#hdEquipo").val(idEquipo);

    $.ajax({
        type: 'POST',
        url: controlador + 'obtenerDatosAsignacion',
        data: {
            equicodi: idEquipo
        },
        dataType: 'json',
        success: function (evt) {
            if (evt.Resultado == "1") {
                //Verifico si es o no linea de transmision
                $("#hdEsLT").val(0);
                if (evt.Equipo.Famcodi == 8)
                    $("#hdEsLT").val(1); //Si es LT

                //Muestro los datos del equipo
                completarDatosEquipo(evt.Equipo);

                //muestro lista de proyectos
                var arrayEquiposProyectos = obtenerArrayDeProyectos(evt.ListaProyectosEquipo);
                mostrarProyectosExtranet(arrayEquiposProyectos);

                $('#btnConfirmarPy').unbind();
                $('#btnConfirmarPy').click(function () {
                    confirmarProyecto();
                });

                $('#btnGuardarAsignacion').unbind();
                $('#btnGuardarAsignacion').click(function () {
                    guardarDatosAsignacion();
                });

                //Si es linea de transmision muestro dicha seccion
                if ($("#hdEsLT").val() == 1) {
                    var htmlSeccionEC = dibujarBaseSeccionEmpresasCopropietarias(evt.ListaEmpresas);
                    $("#seccionEmpCop").html(htmlSeccionEC);

                    $('#cbEmpresaP').multipleSelect({
                        width: '450px',
                        filter: true,
                        single: true,
                        onClose: function () {
                        }
                    });
                    $("#cbEmpresaP").multipleSelect("setSelects", [-1]);

                    //muestro lista de empresas
                    var arrayEmpresasLT = obtenerArrayDeEmpresas(evt.ListadoEmpresasCopropietarias);
                    mostrarEmpresasCopropietarias(arrayEmpresasLT);
                }

                abrirPopup("popupAsignarEquiposFT");
            }
            else {
                mostrarMensaje('mensaje', 'error', 'Error: ' + evt.StrMensaje);
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });

}

function completarDatosEquipo(objEquipo) {
    $("#codigoVal").html(objEquipo.Equicodi);
    $("#nombreVal").html(objEquipo.Equinomb);
    $("#tipoEVal").html(objEquipo.Famnomb);
    $("#empresaVal").html(objEquipo.Emprnomb);
    $("#ubicacionVal").html(objEquipo.Areanomb);

}

function obtenerArrayDeProyectos(listaProyectos) {
    var array = [];

    for (key in listaProyectos) {
        var item = listaProyectos[key];

        var estadoDesc = "";
        if (item.Ftreqpestado == 0)
            estadoDesc = "Baja";

        if (item.Ftreqpestado == 1)
            estadoDesc = "Activo";

        let proy = {
            "idEquipoProyecto": item.Ftreqpcodi,
            "codigoProyecto": item.Ftprycodi,
            "nombreEmpresa": item.Emprnomb,
            "codigo": item.Ftpryeocodigo,
            "nombProy": item.Ftpryeonombre,
            "nombProyExt": item.Ftprynombre,
            "estado": item.Ftreqpestado,
            "estadoDesc": estadoDesc,
            "esAgregado": 0,
            "esEditado": 0
        }

        array.push(proy);
    }
    listaProyectosEquipoMemoria = array;

    return array;
}

function mostrarProyectosExtranet(arrayProyectos) {
    $("#listadoProyectosExtranet").html("");

    var htmlTabla = dibujarTablaListadoProyectos(arrayProyectos);
    $("#listadoProyectosExtranet").html(htmlTabla);

    var esLT = $("#hdEsLT").val();
    var tamAlturaTablaPyE = 0;

    if (esLT == 0)
        tamAlturaTablaPyE = 350;
    if (esLT == 1)
        tamAlturaTablaPyE = 140;

    $('#tablaProyectos').dataTable({
        "scrollY": tamAlturaTablaPyE,
        "scrollX": false,
        "sDom": 't',
        "ordering": false,
        "iDisplayLength": -1
    });
}


function dibujarTablaListadoProyectos(arrayProyectos) {

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="tablaProyectos"  >
       <thead>
           <tr style="height: 22px;">
               <th style='width:60px;'>Acción</th>
               <th style='width:180px;'>Empresa</th>
               <th style='width:100px;'>Código Estudio (EO)</th>
               <th style='width:180px;'>Nombre del Proyecto</th>
               <th style='width:180px;'>Nombre del Proyecto (Extranet)</th>
               <th style='width:50px;'>Estado</th>
               <th style='width:60px;'>Auditoria</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in arrayProyectos) {
        var item = arrayProyectos[key];
        var colorFila = "";

        if (item.estado == 0)
            colorFila = COLOR_BAJA;

        cadena += `
            <tr>
                <td style='width:60px; background: ${colorFila}'>                   
        `;

        if (item.estado == 0) {
            cadena += `
                    <a href="JavaScript:activarProyecto(${item.idEquipoProyecto},${item.codigoProyecto});">${IMG_ACTIVAR}</a>
            `;
        } else {
            if (item.estado == 1) {
                cadena += `                    
                    <a href="JavaScript:eliminarProyecto(${item.idEquipoProyecto},${item.codigoProyecto});">${IMG_ELIMINAR}</a>
                `;
            }
        }
        cadena += `                    
                </td>
                <td style="background: ${colorFila}; width:180px; ">${item.nombreEmpresa}</td>
                <td style="background: ${colorFila}; width:100px; ">${item.codigo}</td>
                <td style="background: ${colorFila}; width:180px; ">${item.nombProy}</td>
                <td style="background: ${colorFila}; width:180px; ">${item.nombProyExt}</td>
                <td style="background: ${colorFila}; width:50px; ">${item.estadoDesc}</td>
        `;
        if (item.esAgregado == 0) {
            cadena += `
                <td style="background: ${colorFila}; width:60px; "><a href="JavaScript:mostrarAuditoriaPE(${item.idEquipoProyecto});">${IMG_VER}</a></td>
            `;
        } else {
            cadena += `
                <td style="background: ${colorFila}"></td>
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

function agregarProyectoExtranet() {
    limpiarBarraMensaje("mensaje_popupAsignarEquiposFT");
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
                $("#listadoProyectos").html(htmlPy);

                $('#tablaPyE').dataTable({
                    "scrollY": 250,
                    "scrollX": false,
                    "sDom": 'ft',
                    "ordering": false,
                    "iDisplayLength": -1
                });

                abrirPopup("popupBusquedaPy");
            }
            else {
                mostrarMensaje('mensaje_popupAsignarEquiposFT', 'error', 'Error: ' + evt.Mensaje);
            }
        },
        error: function () {
            mostrarMensaje('mensaje_popupAsignarEquiposFT', 'error', 'Se ha producido un error.');
        }
    });
}


function dibujarTablaListadoPyE(lista) {

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="tablaPyE" >
       <thead>
           <tr style="height: 22px;">
               <th style='width:40px;'>Sel.</th>
               <th style='width:200px;'>Empresa</th>
               <th style='width:130px;'>Código Estudio (EO)</th>
               <th style='width:260px;'>Nombre del Proyecto</th>
               <th style='width:260px;'>Nombre del Proyecto (Extranet)</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];

        cadena += `
            <tr>
                <td style='width:40px;'>        
                    <input type="radio" id="rdEstudio_${item.Ftprycodi}" name="rdPy" value="${item.Ftprycodi}">
                </td>
                <td style='width:200px;'>${item.Emprnomb}</td>
                <td style='width:130px;'>${item.Ftpryeocodigo}</td>
                <td style='width:260px;'>${item.Ftpryeonombre}</td>
                <td style='width:260px;'>${item.Ftprynombre}</td>
               
           </tr >           
        `;
    }

    cadena += `                   
        </tbody >
    </table >
    `;

    return cadena;
}

function confirmarProyecto() {
    limpiarBarraMensaje("mensaje_popupBusquedaPy");

    var filtro = datosConfirmar();
    var msg = validarDatosConfirmar(filtro);

    if (msg == "") {
        var idProyecto = parseInt(filtro.seleccionado.value) || 0;
        $.ajax({
            type: 'POST',
            url: controlador + "obtenerDatosProySel",
            data: {
                ftprycodi: idProyecto
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    agregarProyectoAArrayProyectos(evt.Proyecto);
                    mostrarProyectosExtranet(listaProyectosEquipoMemoria);

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

function agregarProyectoAArrayProyectos(objProyecto) {
    let proy = {
        "idEquipoProyecto": -1,

        "codigoProyecto": objProyecto.Ftprycodi,
        "nombreEmpresa": objProyecto.Emprnomb,
        "codigo": objProyecto.Ftpryeocodigo,
        "nombProy": objProyecto.Ftpryeonombre,
        "nombProyExt": objProyecto.Ftprynombre,
        "estado": 1,
        "estadoDesc": "Activo",
        "esAgregado": 1,
        "esEditado": 0
    }

    listaProyectosEquipoMemoria.unshift(proy);

}

function datosConfirmar() {
    var filtro = {};

    var radioSel = document.querySelector('input[name="rdPy"]:checked');

    filtro.seleccionado = radioSel;

    return filtro;
}

function validarDatosConfirmar(datos) {

    var msj = "";


    if (datos.seleccionado == null) {
        msj += "<p>Debe seleccionar un Proyecto.</p>";
    } else {
        var idProyecto = parseInt(datos.seleccionado.value) || 0;

        //busco repitencias en los activos
        let lstIguales = listaProyectosEquipoMemoria.filter(x => x.codigoProyecto === idProyecto && x.estado === 1);

        if (lstIguales.length > 0) {
            msj += "<p>El proyecto seleccionado ya se encuentra en la tabla. No se permite duplicados.</p>";

        }
    }

    return msj;
}

function eliminarProyecto(idReg, idProy) {
    limpiarBarraMensaje("mensaje_popupAsignarEquiposFT");
    if (confirm('¿Desea eliminar el registro?')) {

        const registro = listaProyectosEquipoMemoria.find(x => x.idEquipoProyecto === idReg && x.codigoProyecto === idProy);

        if (registro != null) {
            registro.estado = 0;
            registro.estadoDesc = "Baja";
            registro.esEditado = 1;

            mostrarMensaje('mensaje_popupAsignarEquiposFT', 'exito', 'Registro dado de baja satisfactoriamente.');
            mostrarProyectosExtranet(listaProyectosEquipoMemoria);
        } else {
            mostrarMensaje('mensaje_popupAsignarEquiposFT', 'error', 'Ha ocurrido un error al eliminar registro.');
        }

    }
}


function activarProyecto(idReg, idProy) {
    limpiarBarraMensaje("mensaje_popupAsignarEquiposFT");
    if (confirm('¿Desea activar el registro?')) {

        const registro = listaProyectosEquipoMemoria.find(x => x.idEquipoProyecto === idReg && x.codigoProyecto === idProy);

        //valido que no hay otro registro activo con el mismo codigoProyecto
        const regActivoConMismoProyecto = listaProyectosEquipoMemoria.find(x => x.estado === 1 && x.codigoProyecto === idProy);

        if (regActivoConMismoProyecto != null) {
            mostrarMensaje('mensaje_popupAsignarEquiposFT', 'error', 'Existe otro registro activo con el mismo Proyecto.');
        } else {
            if (registro != null) {
                registro.estado = 1;
                registro.estadoDesc = "Activo";
                registro.esEditado = 1;
                mostrarMensaje('mensaje_popupAsignarEquiposFT', 'exito', 'Registro activado satisfactoriamente.');
                mostrarProyectosExtranet(listaProyectosEquipoMemoria);
            } else {
                mostrarMensaje('mensaje_popupAsignarEquiposFT', 'error', 'Ha ocurrido un error al activar registro.');
            }
        }
    }
}

function mostrarAuditoriaPE(ftreqpcodi) {
    limpiarBarraMensaje("mensaje_popupAsignarEquiposFT");
    limpiarDatosPopupAuditoria();

    var filtro = datosAuditoria(ftreqpcodi);
    var msg = validarDatosAuditoria(filtro);

    if (msg == "") {

        var idReg = parseInt(ftreqpcodi) || 0;

        $.ajax({
            type: 'POST',
            url: controlador + "obtenerDatosPEAuditoria",
            data: {
                ftreqpcodi: idReg
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {

                    mostrarDatosPEAuditoria(evt.RelEquipoProyecto);

                } else {
                    mostrarMensaje('mensaje_popupAsignarEquiposFT', 'error', evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje_popupAsignarEquiposFT', 'error', 'Ha ocurrido un error al mostrar estudios EO.');
            }
        });
    } else {
        mostrarMensaje('mensaje_popupAsignarEquiposFT', 'alert', msg);
    }
}

function datosAuditoria(ftreqpcodi) {
    var filtro = {};

    filtro.id = ftreqpcodi;

    return filtro;
}

function validarDatosAuditoria(datos) {

    var msj = "";


    if (datos.id == -1) {
        msj += "<p>Debe seleccionar un registro ya guardado en base de datos.</p>";
    }

    return msj;
}

function limpiarDatosPopupAuditoria() {
    $("#usuCreacionVal").html("");
    $("#fecCreacionVal").html("");
    $("#usuModificacionVal").html("");
    $("#fecModificacionVal").html("");
}

function mostrarDatosPEAuditoria(objReleqpry) {
    $("#usuCreacionVal").html(objReleqpry.Ftreqpusucreacion);
    $("#fecCreacionVal").html(objReleqpry.FechaCreacionDesc);
    $("#usuModificacionVal").html(objReleqpry.Ftreqpusumodificacion);
    $("#fecModificacionVal").html(objReleqpry.FechaModificacionDesc);

    abrirPopup('popupAuditoria');
}


function dibujarBaseSeccionEmpresasCopropietarias(lista) {
    var cadena = '';
    cadena += `
            <div>
                <table id='table_b' style="margin-top: 40px;">
                    <thead>
                        <tr>
                            <th class='th1'>Empresa Copropietaria (Caso seccionamiento LT)</th>
                            <th class='th2'>

                            </th>
                        </tr>
                    </thead>
                </table>
                <table class="content-tabla-search" style="width:auto">
                    <tr>
                        <td class="label_filtro" style="padding-right: 5px; padding-left: 130px;">Empresa:</td>
                        <td>
                            <select id="cbEmpresaP" style="width: 450px;">
                                <option value="-3">-- SELECCIONE --</option>
    `;
    for (key in lista) {
        var item = lista[key];

        cadena += `
                                <option value="${item.Emprcodi}">${item.Emprnomb}</option>                      
        `;
    }
    cadena += `                                
                            </select>
                        </td>

                        <td class="">
                            <a title="Agregar empresa" href="JavaScript:agregarEmpresaCP();"> ${IMG_ADD}</a>
                        </td>

                    </tr>

                </table>
            </div>
            <div id="listadoEmpresasCopropietarias" style="margin-left: 110px;">
            </div>
    `;

    return cadena;
}



function obtenerArrayDeEmpresas(listaRelEmpresaLT) {
    var array = [];

    for (key in listaRelEmpresaLT) {
        var item = listaRelEmpresaLT[key];

        var estadoDesc = "";
        if (item.Ftreqeestado == 0)
            estadoDesc = "Baja";

        if (item.Ftreqeestado == 1)
            estadoDesc = "Activo";

        let regEmpLt = {
            "idRelEmpresaLT": item.Ftreqecodi,
            "idEmpresa": item.Emprcodi,
            "nombreEmpresa": item.Emprnomb,
            "estado": item.Ftreqeestado,
            "estadoDesc": estadoDesc,
            "esAgregado": 0,
            "esEditado": 0
        }

        array.push(regEmpLt);
    }
    listaEmpresaLineaMemoria = array;

    return array;
}

function mostrarEmpresasCopropietarias(arrayEmpresas) {
    $("#listadoEmpresasCopropietarias").html("");

    var htmlTabla = dibujarTablaListadoEmpresasCop(arrayEmpresas);
    $("#listadoEmpresasCopropietarias").html(htmlTabla);

    var tamAlturaTablaEC = 140;

    $('#tablaEmpresasCo').dataTable({
        "scrollY": tamAlturaTablaEC,
        "scrollX": false,
        "sDom": 't',
        "ordering": false,
        "iDisplayLength": -1
    });
}


function dibujarTablaListadoEmpresasCop(arrayEmpresas) {

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="tablaEmpresasCo"  >
       <thead>
           <tr style="height: 22px;">
               <th style='width:60px;'>Acción</th>
               <th style='width:360px;'>Empresa</th>               
               <th style='width:110px;'>Estado</th>
               <th style='width:70px;'>Auditoria</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in arrayEmpresas) {
        var item = arrayEmpresas[key];
        var colorFila = "";

        if (item.estado == 0)
            colorFila = COLOR_BAJA;

        cadena += `
            <tr>
                <td style='width:60px; background: ${colorFila}'>                   
        `;

        if (item.estado == 0) {
            cadena += `
                    <a href="JavaScript:activarEmpresaCo(${item.idRelEmpresaLT},${item.idEmpresa});">${IMG_ACTIVAR}</a>
            `;
        } else {
            if (item.estado == 1) {
                cadena += `                    
                    <a href="JavaScript:eliminarEmpresaCo(${item.idRelEmpresaLT},${item.idEmpresa});">${IMG_ELIMINAR}</a>
                `;
            }
        }
        cadena += `                    
                </td>
                <td style="background: ${colorFila}; width:360px; ">${item.nombreEmpresa}</td>                
                <td style="background: ${colorFila}; width:110px; ">${item.estadoDesc}</td>
        `;
        if (item.esAgregado == 0) {
            cadena += `
                <td style="background: ${colorFila}; width:70px; "><a href="JavaScript:mostrarAuditoriaEmpresaCo(${item.idRelEmpresaLT});">${IMG_VER}</a></td>
            `;
        } else {
            cadena += `
                <td style="background: ${colorFila}"></td>
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


function eliminarEmpresaCo(idReg, idEmpresa) {
    limpiarBarraMensaje("mensaje_popupAsignarEquiposFT");
    if (confirm('¿Desea eliminar el registro?')) {

        const registro = listaEmpresaLineaMemoria.find(x => x.idRelEmpresaLT === idReg && x.idEmpresa === idEmpresa);

        if (registro != null) {
            registro.estado = 0;
            registro.estadoDesc = "Baja";
            registro.esEditado = 1;

            mostrarMensaje('mensaje_popupAsignarEquiposFT', 'exito', 'Registro dado de baja satisfactoriamente.');
            mostrarEmpresasCopropietarias(listaEmpresaLineaMemoria);
        } else {
            mostrarMensaje('mensaje_popupAsignarEquiposFT', 'error', 'Ha ocurrido un error al eliminar registro.');
        }

    }
}


function activarEmpresaCo(idReg, idEmpresa) {
    limpiarBarraMensaje("mensaje_popupAsignarEquiposFT");
    if (confirm('¿Desea activar el registro?')) {

        const registro = listaEmpresaLineaMemoria.find(x => x.idRelEmpresaLT === idReg && x.idEmpresa === idEmpresa);

        //valido que no hay otro registro activo con el mismo emprcodi
        const regActivoConMismoEmpresa = listaEmpresaLineaMemoria.find(x => x.estado === 1 && x.idEmpresa === idEmpresa);

        if (regActivoConMismoEmpresa != null) {
            mostrarMensaje('mensaje_popupAsignarEquiposFT', 'error', 'Existe otro registro activo con la misma empresa.');
        } else {
            if (registro != null) {
                registro.estado = 1;
                registro.estadoDesc = "Activo";
                registro.esEditado = 1;
                mostrarMensaje('mensaje_popupAsignarEquiposFT', 'exito', 'Registro activado satisfactoriamente.');
                mostrarEmpresasCopropietarias(listaEmpresaLineaMemoria);
            } else {
                mostrarMensaje('mensaje_popupAsignarEquiposFT', 'error', 'Ha ocurrido un error al activar registro.');
            }
        }
    }
}

function mostrarAuditoriaEmpresaCo(ftreqecodi) {
    limpiarBarraMensaje("mensaje_popupAsignarEquiposFT");
    limpiarDatosPopupAuditoria();

    var filtro = datosAuditoriaEmpCo(ftreqecodi);
    var msg = validarDatosAuditoriaEmpCo(filtro);

    if (msg == "") {

        var idReg = parseInt(ftreqecodi) || 0;

        $.ajax({
            type: 'POST',
            url: controlador + "obtenerDatosAuditoriaEmpCo",
            data: {
                ftreqecodi: idReg
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {

                    mostrarDatosAuditoriaELT(evt.RelLTEmpresa);

                } else {
                    mostrarMensaje('mensaje_popupAsignarEquiposFT', 'error', evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje_popupAsignarEquiposFT', 'error', 'Ha ocurrido un error al mostrar estudios EO.');
            }
        });
    } else {
        mostrarMensaje('mensaje_popupAsignarEquiposFT', 'alert', msg);
    }
}

function datosAuditoriaEmpCo(ftreqecodi) {
    var filtro = {};

    filtro.id = ftreqecodi;

    return filtro;
}

function validarDatosAuditoriaEmpCo(datos) {

    var msj = "";


    if (datos.id == -1) {
        msj += "<p>Debe seleccionar un registro ya guardado en base de datos.</p>";
    }

    return msj;
}

function mostrarDatosAuditoriaELT(objReleqemplt) {
    $("#usuCreacionVal").html(objReleqemplt.Ftreqeusucreacion);
    $("#fecCreacionVal").html(objReleqemplt.FechaCreacionDesc);
    $("#usuModificacionVal").html(objReleqemplt.Ftreqeusumodificacion);
    $("#fecModificacionVal").html(objReleqemplt.FechaModificacionDesc);

    abrirPopup('popupAuditoria');
}


function agregarEmpresaCP() {
    limpiarBarraMensaje("mensaje_popupAsignarEquiposFT");

    var filtro = datosAddEmpresa();
    var msg = validarDatosAddEmpresa(filtro);

    if (msg == "") {

        var idEmpresa = parseInt(filtro.elegido);
        $.ajax({
            type: 'POST',
            url: controlador + "obtenerDatosEmpresaSel",
            data: {
                emprcodi: idEmpresa
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    agregarEmpresaAArrayEmpresasCo(evt.Empresa);
                    mostrarEmpresasCopropietarias(listaEmpresaLineaMemoria);

                } else {
                    mostrarMensaje('mensaje_popupAsignarEquiposFT', 'error', evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje_popupAsignarEquiposFT', 'error', 'Ha ocurrido un error al agregar empresa.');
            }
        });
    } else {
        mostrarMensaje('mensaje_popupAsignarEquiposFT', 'alert', msg);
    }
}

function agregarEmpresaAArrayEmpresasCo(objEmpresa) {
    let regEmpLt = {
        "idRelEmpresaLT": -1,
        "idEmpresa": objEmpresa.Emprcodi,
        "nombreEmpresa": objEmpresa.Emprnomb,
        "estado": 1,
        "estadoDesc": "Activo",
        "esAgregado": 1,
        "esEditado": 0
    }

    listaEmpresaLineaMemoria.unshift(regEmpLt);

}

function datosAddEmpresa() {
    var filtro = {};

    filtro.elegido = $("#cbEmpresaP").val();

    return filtro;
}

function validarDatosAddEmpresa(datos) {

    var msj = "";

    if (datos.elegido != null) {
        if (isNaN(datos.elegido)) {
            msj += "<p>Debe seleccionar una empresa correcta.</p>";
        } else {
            if (datos.elegido == "-3") {
                msj += "<p>Debe seleccionar una empresa.</p>";
            } else {
                var idEmpresa = parseInt(datos.elegido);

                //busco repitencias en los activos
                let lstIguales = listaEmpresaLineaMemoria.filter(x => x.idEmpresa === idEmpresa && x.estado === 1);

                if (lstIguales.length > 0) {
                    msj += "<p>La empresa seleccionada ya se encuentra en la tabla. No se permite duplicado.</p>";
                }
            }
        }
    } else {
        msj += "<p>Debe seleccionar una empresa correcta.</p>";
    }

    return msj;
}


function guardarDatosAsignacion() {
    limpiarBarraMensaje("mensaje_popupAsignarEquiposFT");
    var idEquipo = $("#hdEquipo").val();

    var filtro = datosAsignacion();
    var msg = validarDatosAsignacion(filtro);

    if (msg == "") {

        $.ajax({
            type: 'POST',
            url: controlador + "guardarInfoAsignacion",
            data: {
                equicodi: idEquipo,
                strCambiosPE: filtro.strCambiosProyExt,
                strCambiosELT: filtro.strCambiosEmpresasCo,
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    mostrarMensaje('mensaje', 'exito', "La información se guardó correctamente.");
                    cerrarPopup('popupAsignarEquiposFT');

                } else {
                    mostrarMensaje('mensaje_popupAsignarEquiposFT', 'error', evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje_popupAsignarEquiposFT', 'error', 'Ha ocurrido un error al mostrar estudios EO.');
            }
        });
    } else {
        mostrarMensaje('mensaje_popupAsignarEquiposFT', 'alert', msg);
    }
}


function datosAsignacion() {
    var filtro = {};

    //Obtengo listado de cambios en proyectos extranet
    let arrayCambiosEP = listaProyectosEquipoMemoria.filter(x => x.esAgregado === 1 || x.esEditado === 1);

    //Obtengo listado de cambios en empresas copropietarias
    let arrayCambiosELT = listaEmpresaLineaMemoria.filter(x => x.esAgregado === 1 || x.esEditado === 1);

    filtro.arrCambiosProyectosExtranet = arrayCambiosEP;
    filtro.strCambiosProyExt = obtenerCadenaCambiosProyExt(arrayCambiosEP);

    filtro.arrCambiosEmprCoprop = arrayCambiosELT;
    filtro.strCambiosEmpresasCo = obtenerCadenaCambiosEmpCo(arrayCambiosELT);

    return filtro;
}

function validarDatosAsignacion(datos) {

    var msj = "";
    var numCambios = datos.arrCambiosProyectosExtranet.length + datos.arrCambiosEmprCoprop.length;

    if (numCambios == 0) {
        msj += "<p>No se detectó cambios, ingrese o edite información.</p>";
    }

    return msj;

}

function obtenerCadenaCambiosProyExt(arrayCambios) {
    var salida = "";

    var lstCambios = [];
    for (key in arrayCambios) {
        var item = arrayCambios[key];
        var reg = item.idEquipoProyecto + "$$" + item.codigoProyecto + "$$" + item.estado + "$$" + item.esAgregado + "$$" + item.esEditado;
        lstCambios.push(reg);
    }

    salida = lstCambios.join("??");
    return salida;
}



function obtenerCadenaCambiosEmpCo(arrayCambios) {
    var salida = "";

    var lstCambios = [];
    for (key in arrayCambios) {
        var item = arrayCambios[key];
        var reg = item.idRelEmpresaLT + "$$" + item.idEmpresa + "$$" + item.estado + "$$" + item.esAgregado + "$$" + item.esEditado;
        lstCambios.push(reg);
    }

    salida = lstCambios.join("??");
    return salida;
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
////////////////////////////////////////////////////////////////////////////////////////
// Auditoria Cambios

function mostrarPopupAuditoriaCambio(equicodi) {
    $.ajax({
        type: 'POST',
        url: controlador + "ListarPropiedadAuditoria",
        data: {
            equicodi: equicodi
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $("#listadoAuditoriaCambio").html("");

                var htmlTabla = dibujarTablaListadoAuditoriaCambio(evt.EquipoSeleccionado, evt.ListaPropiedad);
                $("#listadoAuditoriaCambio").html(htmlTabla);

                abrirPopup("popupAuditoriaCambio");

                setTimeout(function () {
                    $('#tablaAuditoriaCambio').dataTable({
                        "scrollY": false,
                        "scrollX": false,
                        "sDom": 't',
                        "ordering": false,
                        "iDisplayLength": -1
                    });
                }, 50);

            } else {
                mostrarMensaje('mensaje_popupAuditoriaCambio', 'error', evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje_popupAuditoriaCambio', 'error', 'Ha ocurrido un error.');
        }
    });
}

function dibujarTablaListadoAuditoriaCambio(obj, lista) {

    var cadena = '';
    cadena += `

    <input type='hidden' id='hfEquicodiAuditoriaCambio' value='${obj.Equicodi}' />
    
    <table class="content-tabla-search" style="width:auto">
        <tr>
            <td class="tbform-label">Empresa:</td>
            <td>
                ${obj.Emprnomb}
            </td>
        </tr>
        <tr>
            <td class="tbform-label">Código:</td>
            <td>
                ${obj.Equicodi}
            </td>
        </tr>
        <tr>
            <td class="tbform-label">Equipo:</td>
            <td>
                ${obj.Equinomb}
            </td>
        </tr>
        <tr>
            <td class="tbform-label">Ubicación:</td>
            <td>
                ${obj.Areanomb}
            </td>
        </tr>
        <tr>
            <td class="tbform-label">Tipo de Equipo:</td>
            <td>
                ${obj.Famnomb}
            </td>
        </tr>
    </table>
    <div style="clear:both; height:15px"></div>

    <table border="0" class="pretty tabla-icono" id="tablaAuditoriaCambio"  width="100%" >
       <thead>
           <tr style="height: 22px;">
               <th style='width:200px;'>Parámetro</th>
               <th style='width:180px;'>Valor</th>
               <th style='width:100px;'>Usuario <br/>Última modificación</th>
               <th style='width:100px;'>Fecha <br/> Última modificación</th>
               <th style='width:60px;'>Acción</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];

        cadena += `   
           <tr>   
                <td style="width:200px; text-align: left; padding-left: 5px;">${item.Propnomb}</td>
                <td style="width:180px; ">${item.Valor}</td>
                <td style="width:100px; ">${item.UltimaModificacionUsuarioDesc}</td>
                <td style="width:100px; ">${item.UltimaModificacionFechaDesc}</td>
                <td style="width:60px; " title='Ver historial'><a href="JavaScript:mostrarHistorialAuditoriaCambio(${item.Equicodi}, ${item.Propcodi});">${IMG_HISTORIAL_AUDITORIA_CAMBIO}</a></td>
           </tr>   
        `;
    }

    cadena += `                   
        </tbody >
    </table >
    `;

    return cadena;
}

function mostrarHistorialAuditoriaCambio(equicodi, propcodi) {
    $.ajax({
        type: 'POST',
        url: controlador + "HistoricoPropiedadEquipo",
        data: {
            iEquipo: equicodi,
            iPropiedad: propcodi
        },
        success: function (evt) {
            $('#historicoPropiedadAuditoriaCambio').html(evt);
            setTimeout(function () {
                $('#popupHistoricoAuditoriaCambio').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);

        },
        error: function (err) {
            mostrarError('Ha ocurrido un error.');
        }
    });
};

function exportarExcelPropiedadAuditoria() {
    var equicodi = parseInt($("#hfEquicodiAuditoriaCambio").val()) || 0;

    $.ajax({
        type: 'POST',
        url: controlador + "ExportarExcelPropiedadAuditoria",
        data: {
            equicodi: equicodi,
        },
        dataType: 'json',
        cache: false,
        success: function (model) {
            if (model.Resultado == 1) {
                location.href = controlador + "DescargarExcelPropiedadAuditoria";
            } else {
                mostrarMensaje('mensaje_popupAuditoriaCambio', 'error', model.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje_popupAuditoriaCambio', 'error', "Ha ocurrido un error");
        }
    });
};