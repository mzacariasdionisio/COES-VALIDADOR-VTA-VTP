var ID_CENTRAL_DEFAULT = -2;
var TIPO_CENTRAL_HIDROELECTRICA = 4;
var TIPO_CENTRAL_TERMOELECTRICA = 5;
var TIPO_CENTRAL_SOLAR = 37;
var TIPO_CENTRAL_EOLICA = 39;

var APP_OPCION = -1;
var OPCION_COPIAR = 1;
var OPCION_EDITAR = 2;
var OPCION_NUEVO = 3;
var OPCION_ELIMINAR = 4;
var OPCION_VER = 5;

function popUpAgreNewEnsayo() {

    idEmpresa = $('#cbEmpresa').val();
    central = $('#cbCentral').val();

    $.ajax({
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        traditional: true,
        async: false,
        url: controlador + 'genera/ViewPopUpNuevoEnsayo',
        data: JSON.stringify({
            idEmpresa: parseInt(idEmpresa),
            idCentral: parseInt(central)
        }), success: function (result) {

            $("#miPopupRegistrarNuevoEnsayo .popup-title span").text("Registro de Nuevo Ensayo");

            $('#idRegistrarNuevoEnsayo').html(result);
            setTimeout(function () {
                $('#miPopupRegistrarNuevoEnsayo').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    modalClose: false
                });
            }, 50);
            //Cuando cambiamos de empresa debera cambiar las centrales
            $('#cbEmpresa2').change(function () {
                cargarCentral2();

            });

            //Cuando cambiamos de Central, cambiar los MO
            $('#cbCentral2').change(function () {
                cargarListaModo_Grupo();
            });

            $("#btnCancelar3").click(function () {
                $('#miPopupRegistrarNuevoEnsayo').bPopup().close();
            });



            // Graba los datos de un nuevo ensayo...
            $('#btnAceptar3').click(function () {
                var resultado = validarEnsayo(); //si contiene registros a guardar
                if (resultado == "") {

                    $('#miPopupRegistrarNuevoEnsayo').bPopup().close();
                    guardarEnsayo();
                    limpiarNumUnidades();

                }
                else {
                    alert("Hubo Error :" + resultado);
                }
            });
        },
        error: function (result) {
            alert('Ha ocurrido un error al generar vista POPUP');
        }
    });
}

///funcion que genera la vista para mostrar los Modos de Operación de los ensayos
/// <params>
//</params>
function popUpAgreNewMO(idEnsayo) {

    var idEnsayo;
    $.ajax({
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        traditional: true,
        async: false,
        url: controlador + 'genera/ViewPopUpModosOperacion',
        data: JSON.stringify({
            idEnsayo: parseInt(idEnsayo)


        }), success: function (result) {

            $("#popupModosOperacion .popup-title span").text("Modos de Operación");

            $('#idMostrarMO').html(result);
            setTimeout(function () {
                $('#popupModosOperacion').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    modalClose: false
                });
            }, 50);


        },
        error: function (result) {
            alert('Ha ocurrido un error al generar vista de los MO');
        }
    });

}

//para guardar el ensayo
function guardarEnsayo() {
    var idCentral = $('#cbCentral2').val();
    var idEmpresa = $("#cbEmpresa2").val();
    var dataListaMoOpYUnid = [];
    var listadoMOyUnid1 = obtenerMOconSusUnidades();
    var lista1 = JSON.stringify(listadoMOyUnid1)

    $.ajax({
        type: 'POST',
        url: controlador + "genera/GuardarEnsayoNuevo",
        dataType: 'json',
        data: {
            idEmpresa: idEmpresa,
            idCentral: idCentral,
            dataListaMoOpYUnid: lista1
        },
        success: function (evt) {
            if (evt.resultado == "2") {
                alert("Error : No se envió el email ")
            }
            setTimeout(function () {
                $('#popupEnsNuevoOK').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
            $("#btnPAceptarEnsayo").click(function () {
                $('#popupEnsNuevoOK').bPopup().close();
            });
            buscarEnsayo(1);

        },
        error: function () {
            alert("Ha ocurrido un error en guardar ensayo");
        }
    });
}

function obtenerMOconSusUnidades() {

    var listaGeneral = [];

    $("input[name=check_mo_cnp]:checked").each(function () {
        var objMOConSusUnidades = {};
        var listaUnidadesXMO = [];

        var codigoDelMO = $(this).val() || 0;
        objMOConSusUnidades.CodigoMO = parseInt(codigoDelMO);
        objMOConSusUnidades.ListaUnidades = [];
        $("#tabla_unidades" + codigoDelMO + " input[name=check_unidad_cnp]:checked").each(function () {
            var unidad = $(this).val() || 0;
            objMOConSusUnidades.ListaUnidades.push(unidad);
        });
        listaGeneral.push(objMOConSusUnidades);
    });

    return listaGeneral;
}

function validarEnsayo() {
    var resultado = "";
    var empresa = $('#cbEmpresa2').val();
    var central = $('#cbCentral2').val();
    var moseleccionados = verificarSiHayUnidadesSeleccionadas();

    if (empresa == 0) {
        resultado = " Seleccionar Empresa";
        return resultado;
    }
    if (central == -1) {
        resultado = " Seleccionar Central";
        return resultado;
    }

    if (moseleccionados == 0) {
        resultado = " Seleccionar Modo de Operación";
        return resultado;
    }


    return resultado;
}
