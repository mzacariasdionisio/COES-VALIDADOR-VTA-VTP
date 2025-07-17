var controlador = siteRoot + 'IEOD/TopologiaElect/';
var ListaEquiposTopologia = [];
var ancho = 900; 

$(function () {
    $('#cbEmpresa').multipleSelect({
        width: '250px',
        filter: true,
        onClose: function (view) {
            cargarTipoEquipoPadre();
        }
    });

    $('#btnConsultar').click(function () {
        generaListado();

    });

    $('#btnNuevaTopologia').click(function () {
        popUpAgregarEquipo(-1, -1, -1);
    });

    ancho = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 30 : 900;

    cargarValoresIniciales();
});

function cargarValoresIniciales() {
    $('#cbEmpresa').multipleSelect('checkAll');
    cargarTipoEquipoPadre();
    generaListado();
}

function generaListado() {

    mostrarListadoEquiposTopologia();

}

function mostrarListadoEquiposTopologia() {
    hideMensaje();

    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    var tipoEquipopadre = $('#cbTipoEquipoPadre').multipleSelect('getSelects');
    if (tipoEquipopadre == null) {
        tipoEquipopadre = "-1";
    }

    $('#hfEmpresa').val(empresa);
    $('#hfTipoEquipPadre').val(tipoEquipopadre);


    $.ajax({
        type: 'POST',
        url: controlador + "lista",
        data: {
            empresas: $('#hfEmpresa').val(),
            tipoEquipopadre: $('#hfTipoEquipPadre').val()
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);

            var anchoReporte = $('#tabla').width();
            $("#listado").css("width", (anchoReporte > ancho ? ancho : anchoReporte) + "px");

            $('#tabla').dataTable({
                "sPaginationType": "full_numbers",
                "bInfo": true,
                "ordering": false,
                "iDisplayLength": 15,
            });
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarTipoEquipoPadre() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');

    $('#hfEmpresa').val(empresa);

    var sEmpresa = $('#cbEmpresa').val();
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarTipoEquiposPadresIndex',

        data: { empresas: $('#hfEmpresa').val() },

        success: function (aData) {
            $('#tipoequipospadres').html(aData);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

///funcion que genera la vista para el ingreso de equipos dependientes de una topología

function popUpAgregarEquipo(idempresa, idEquipo, tipoEquipo) {

    $.ajax({
        type: 'POST',
        url: controlador + 'ViewAgregarEquipos',
        data: {
            idEmpresa: idempresa,
            idEquipo: idEquipo,
            tipoEquipo: tipoEquipo
        },
        success: function (result) {
            $('#agregarEquipo').html(result);

            inicializarViewEquipos();

            setTimeout(function () {
                $('#popupEquipo').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    modalClose: false
                });
            }, 50);
        },
        error: function (result) {
            alert('ha ocurrido un error al generar vista');
        }
    });
}
///UTL/////
function offshowView() {
    $('#enviosHorasOperacion').bPopup().close();
}

// funcion que muestra el detalle de un equipo padre y su topologia para edicion
function mostrarDetalle(emprcodi, equicodi, famcodi) {

    popUpAgregarEquipo(emprcodi, equicodi, famcodi)
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Nuevo

function inicializarViewEquipos() {
    ListaEquiposTopologia = [];

    $('#cbEmpresa2').val($('#hfEmpresa2').val());

    $('#cbEmpresa2').change(function () {
        cargarTipoEquipoPadrePopup();
    });

    $('#cbEmpresaEquipo').change(function () {
        cargarTipoEquipoHijoPopup();
    });

    $('#btnAgregarEquipo').click(function () {
        agregarEquipoTopologia();
    });


    $('#btnCancelar').click(function () {
        cancelar();
    });

    $('#btnGrabar').click(function () {
        grabarListaequiposTopologia();
    });

    $('#viewFiltroEquipo').click(function () {

        validarMostrarFiltrosequipoHijo();

    });

    cargarListaEquiposTopologia();
    cargarTipoEquipoPadrePopup();
    cargarTipoEquipoHijoPopup();
    $('#filtrosEquipos').hide();
    $('#btnGrabar').hide();
};

function cargarTipoEquipoPadrePopup() {
    empresa = $('#cbEmpresa2').val();
    tipoEquipoSelect = $('#hfTipoEquipoPadrePopup').val();
    activaFiltro = $('#hfActivaFiltros').val()
    idEquipoPadre = $('#hfEquipadre').val();


    $.ajax({
        type: 'POST',
        url: controlador + 'CargarTipoEquipoPadrePopup',

        data: {
            empresa: empresa,
            idTipoEquipo: tipoEquipoSelect,
            activaFiltro: activaFiltro,
            idEquipoPadre: idEquipoPadre
        },

        success: function (aData) {
            $('#tipoequipoPadrePopup').html(aData);
            if ($('#hfEmpresa2').val() == -1) { // si es nuevo
                ListaEquiposTopologia.length = 0;
                dibujarTablaEquipos();
            }
            $('#filtrosEquipos').hide();
        },
        error: function (err) {
            alert("Ha ocurrido un error rn cargar tipo de equipo");
        }
    });
}

function cargarTipoEquipoHijoPopup() {
    empresa = $('#cbEmpresaEquipo').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarTipoEquipoHijoPopup',

        data: {
            empresa: empresa
        },

        success: function (aData) {
            $('#tipoequipoHijoPopup').html(aData);

        },
        error: function (err) {
            alert("Ha ocurrido un error en cargar tipo de equipo");
        }
    });

}

function cargarEquipoPadrePopup() {

    idTipoPadre = $('#cbTipoEquipoPadrePopup').val();
    actFiltro = $('#hfFiltro').val();
    if (idTipoPadre == null) {
        idTipoPadre = 0;
    }
    idEmpresa = $('#cbEmpresa2').val();
    idEquipoPadre = $('#hfIdEquipoPadre02').val()

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarEquiposPadresPopup',

        data: {
            idEmpresa: idEmpresa,
            idTipoPadre: idTipoPadre,
            idEquipoPadre: idEquipoPadre,
            actFiltro: actFiltro
        },

        success: function (aData) {
            $('#equipoPadrePopup').html(aData);
            $('#filtrosEquipos').hide();
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarEquipoHijoPopup() {
    idTipoHijo = $('#cbTipoEquipoHijo').val();
    if (idTipoHijo == null) {
        idTipoHijo = 0;
    }
    idEmpresa = $('#cbEmpresaEquipo').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarEquiposHijosPopup',

        data: {
            idEmpresa: idEmpresa,
            idTipoHijo: idTipoHijo
        },

        success: function (aData) {
            $('#equipoHijoPopup').html(aData);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

//funcion que inicializa la lista de los equipos de una topología
function cargarListaEquiposTopologia() {
    strEquipos = $('#hfEquiposTopologia').val();
    arrEquipos = strEquipos.split('#');
    arrEquipos.splice(arrEquipos.length - 1, 1);


    ///codigo para interpretar la cadena de texto e inicializar la lista
    for (i = 0; i < arrEquipos.length; i++) {
        arrEquipo = arrEquipos[i].split(',');
        var entityEquipo =
        {
            Empresatopologia: arrEquipo[0],
            Equipotopologia: arrEquipo[1],
            Equicodi1: arrEquipo[2],
            Tiporelcodi: 26,
            Equicodi2: arrEquipo[3],
            Equirelagrup: arrEquipo[4],
            EquirelfecmodificacionDesc: arrEquipo[5],
            Equirelusumodificacion: arrEquipo[6],
            Equirelexcep: arrEquipo[7],
            OpCrud: 0, // -1: eliminar, 0:lectura, 1:crear, 2:actualizar 

        };
        ListaEquiposTopologia.push(entityEquipo);
    }

    dibujarTablaEquipos();
}

function dibujarTablaEquipos() {
    strHtml = "";
    strHtml += "<table border='0' class='pretty tabla-icono' cellspacing='0' width='100%' id='tablatopologia'>";
    strHtml += "<thead><tr><th></th><th>Empresa</th><th>Equipo</th><th>Agupación</th><th>Tipo de Excepción</th><th>Usuario</th><th>Fecha modificación</th></tr></thead><tbody>";

    //ordenamos el arreglo por Agrupacion
    ListaEquiposTopologia.sort(function (a, b) {
        return (a.Equirelagrup - b.Equirelagrup)
    })

    if (ListaEquiposTopologia.length > 0) {
        for (i = 0; i < ListaEquiposTopologia.length; i++) {
            if (ListaEquiposTopologia[i].OpCrud != -1) { // imprimimos los equipos nuevos y los que no has sido eliminados que vienen de BD
                nombreTipoExcepcion = "Ninguno";
                strHtml += "<tr><td>";
                strHtml += "<a href='#'><img onclick='eliminaEquipo(" + ListaEquiposTopologia[i].Equicodi1 + "," + ListaEquiposTopologia[i].Equicodi2 + "," + ListaEquiposTopologia[i].Equirelagrup + ");' src='" + siteRoot + "Content/Images/ico-delete.gif' title='Eliminar Equipo' alt='eliminar'/></a>";
                strHtml += "</td>";
                strHtml += "<td>" + ListaEquiposTopologia[i].Empresatopologia + "</td>";
                strHtml += "<td>" + ListaEquiposTopologia[i].Equipotopologia + "</td>";
                strHtml += "<td>" + ListaEquiposTopologia[i].Equirelagrup + "</td>";

                if (ListaEquiposTopologia[i].Equirelexcep == 1) {
                    nombreTipoExcepcion = "Sistema Aislado";
                }
                strHtml += "<td>" + nombreTipoExcepcion + "</td>";
                strHtml += "<td>" + ListaEquiposTopologia[i].Equirelusumodificacion + "</td>";
                strHtml += "<td>" + ListaEquiposTopologia[i].EquirelfecmodificacionDesc + "</td>";

                strHtml += "</tr>";
            }
        }
    }
    else {
        strHtml += "<tr><td colspan='7'>No hay elementos...</td></tr>";
    }
    strHtml += "</tbody></table>";
    $('#listadotopologia').css("width", $('#mainLayout').width() / 2 + "px");
    $('#listadotopologia').html(strHtml);
}

function cancelar() {
    $('#popupEquipo').bPopup().close();
}

function agregarEquipoTopologia() {

    equipopadre = $('#cbEquipoPadrePopup').val();
    empresa = $('#cbEmpresa2').val();
    tipoequipo = $('#cbTipoEquipoPadre').val();
    tipoequipotopologia = $('#cbTipoEquipoHijo').val();

    //datos de equipo topologia
    empresaTopologia = $('#cbEmpresaEquipo').val();
    nombEmprtopologia = $("#cbEmpresaEquipo option:selected").html();
    equipoTopologia = $('#cbEquipoHijo').val();
    nombEquipoTopologia = $("#cbEquipoHijo option:selected").html();
    agrupacion = $('#cbAgrupacion').val();
    tipoexcepcion = $('#cbTipoExcepcion').val();


    if (empresaTopologia > -1) {
        if (tipoequipotopologia > 0) {
            if (equipoTopologia > 0) {

                entityEquipo =
                    {
                        Empresatopologia: nombEmprtopologia,
                        Equipotopologia: nombEquipoTopologia,
                        Equicodi1: equipopadre,
                        Tiporelcodi: 26,
                        Equicodi2: equipoTopologia,
                        Equirelagrup: agrupacion,
                        Equirelusumodificacion: "",
                        EquirelfecmodificacionDesc: "",
                        Equirelexcep: tipoexcepcion,
                        OpCrud: 1, // -1: eliminar, 0:lectura, 1:crear, 2:actualizar 

                    };
                ListaEquiposTopologia.push(entityEquipo);

                maxValorAgrupacion = getMaxValorAgrupacion();
                nelements = $('#cbAgrupacion option').size();
                if (nelements < maxValorAgrupacion) {
                    $('#cbAgrupacion').get(0).options[$('#cbAgrupacion').get(0).options.length] = new Option(maxValorAgrupacion.toString(), maxValorAgrupacion);
                }

                dibujarTablaEquipos();
                $('#btnGrabar').show();
                $('#filtrosEquipos').hide();

            }
            else {
                alert("Debe seleccionar Equipo..!");
            }
        }
        else {
            alert("debe seleccionar Tipo de equipo..!");
        }
    }
    else {
        alert("debe seleccionar Empresa..!");
    }
}

function grabarListaequiposTopologia() {

    if (ListaEquiposTopologia.length > 0) {
        var matriz = ListaEquiposTopologia;
        $.ajax({
            type: 'POST',
            //async: false,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            traditional: true,
            url: controlador + "grabar",
            data: JSON.stringify({
                data: matriz
            }),
            beforeSend: function () {
                //mostrarExito("Enviando Información ..");
            },
            success: function (evt) {
                if (evt.Resultado == 1) {
                    mostrarMensaje("Los datos se enviaron correctamente");
                    cancelar();
                }
                else {
                    mostrarError("Error al Grabar: " + evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarError("Ha ocurrido un error");
            }
        });
    }
    else {
        alert("No existen equipoos para guardar...!");
    }
}

function eliminaEquipo(equicodi1, equicodi2, equirelagrup) {
    if (confirm("¿Desea eliminar equipo?")) {

        for (i = 0; i < ListaEquiposTopologia.length; i++) {
            if (ListaEquiposTopologia[i].Equicodi1 == equicodi1 && ListaEquiposTopologia[i].Equicodi2 == equicodi2 && ListaEquiposTopologia[i].Equirelagrup == equirelagrup) {
                if (ListaEquiposTopologia[i].OpCrud == 0) { // si registro viene de base de datos y es de sololectura
                    ListaEquiposTopologia[i].OpCrud = -1
                }
                else {
                    ListaEquiposTopologia.splice(i, 1);
                }
            }
        }

        dibujarTablaEquipos();
        $('#btnGrabar').show();
    }
}

//genereal en máximo valor de la agrupacion para los equipos registrados
function getMaxValorAgrupacion() {
    maxval = 0;
    for (i = 0; i < ListaEquiposTopologia.length; i++) {
        if (maxval < ListaEquiposTopologia[i].Equirelagrup) {
            maxval = ListaEquiposTopologia[i].Equirelagrup;
        }
    }
    return ++maxval;

}

function validarMostrarFiltrosequipoHijo() {
    empresa = $('#cbEmpresa2').val();
    tipoequipoPadre = $('#cbTipoEquipoPadrePopup').val();
    equipoPadre = $('#cbEquipoPadrePopup').val();

    if (empresa > -1) {
        if (tipoequipoPadre > -1) {
            if (equipoPadre > -1) {
                $('#cbEmpresaEquipo').val(-1);
                cargarTipoEquipoHijoPopup();
                $('#cbAgrupacion').val(1);
                $('#cbTipoExcepcion').val(0);
                $('#filtrosEquipos').show();
            }
            else {
                alert("debe seleccionar equipo  !");
            }
        }
        else {
            alert("Debe seleccionar tipo de equipo  !");
        }
    }
    else {
        alert("Debe seleccionar empresa  !");
    }
}