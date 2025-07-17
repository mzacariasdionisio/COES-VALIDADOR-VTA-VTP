//Declarando las variables 
let controlador = siteRoot + 'UbicacionCOES/';
let hot;
let hotOptions;
let evtHot;
let txtConfirmacionGlobal;

$(function () {

    $('#cbTipoArea').val($("#idTipoArea").val());
    buscarArea();

    $("#popUpEditar").addClass("general-popup");

    $('#btnBuscar').click(function () {
        buscarArea();
    });

});

function buscarArea() {
    consultar();
}

let consultar = function () {

    let programaExistente = "";
    $("#chkProgramaExistente").click(function () { 
        if ($(this).is(':checked')) {
            programaExistente = "S";
        } else {
            programaExistente = "N";
        }
    });

    if ($("#chkProgramaExistente").is(':checked')) {
        programaExistente = "S";
    } else {
        programaExistente = "N";
    }
    
    setTimeout(function () {
        $.ajax({
            type: 'POST',
            url: controlador + 'ListaUbicaciones',
            data: {
                sCodigoTipoArea: $('#Tipo').val(),
                sNombre: $('#nombreArea').val(),
                sProgramaExistente: programaExistente
            },
            success: function (evt) {
                $('#listaUbicaciones').html(evt);
                $('#tablaListado').dataTable({
                    "iDisplayLength": 25,
                    language: {
                        info: 'Mostrando página _PAGE_ de _PAGES_',
                        infoEmpty: 'No hay registros disponibles',
                        infoFiltered: '(filtrado de _MAX_ registros totales)',
                        lengthMenu: 'Mostrar _MENU_ registros por página',
                        zeroRecords: 'No se encontró nada'
                    }, order: [[0, 'desc'], [1, 'asc'], [4, 'asc']]
                });

                $('.dataTables_filter input').attr('maxLength', 50);
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }, 100);
};

let guardar = function () {
    if (validar()) {
        let codigo = $('#Areacodi').val();
        let codigoEpr = $('#Epareacodi').val();
        let textoConfirmación = this.txtConfirmacionGlobal;

        let nombre = "";
        if ($('#nombreProteccion').val() != null && $('#nombreProteccion').val() != "") {
            nombre = $('#nombreProteccion').val().trim();
        }

        if (confirm(textoConfirmación)) {

            let datos = {
                iZona: $('#Zona').val(),
                iCodigo: codigo,
                iPrCodigo: codigoEpr,
                sFlag: 1,
                sNombre: nombre
            };
            
            $.ajax({
                type: 'POST',
                url: controlador + 'GuardarUbicacion',
                dataType: 'json',
                data: datos,
                cache: false,
                success: function (resultado) {
                    if (resultado == 1) {
                        $('#popUpEditar').bPopup().close();
                        buscarArea();
                    } else
                        mostrarError();
                },
                error: function () {
                    mostrarError();
                }
            });
        }
    } else {
        alert("Ingrese el nombre para protecciones");
    }
};

let validar = function () {
    if ($("#chkPresente").is(':checked')) {
        return $('#nombreProteccion').val() != "" && $('#nombreProteccion').val().trim().length > 0;
    } else {
        return true;
    }
}

let eliminar = function (e) {;
    let textoConfirmación = '¿Está seguro de eliminar la ubicación del módulo de Protecciones?'

    if (confirm(textoConfirmación)) {

        let datos = {
            iPrCodigo: e
        };
        
        $.ajax({
            type: 'POST',
            url: controlador + 'EliminarUbicacion',
            dataType: 'json',
            data: datos,
            cache: false,
            success: function (resultado) {
                switch (resultado.Estado) {
                    case 1:
                        buscarArea();
                        break;
                    case 0:
                        alert(resultado.Mensaje)
                        break;
                    case -1:
                        mostrarError();
                        break;
                }                
            },
            error: function () {
                mostrarError();
            }
        });
    }
};

function editarUbicacion(e, epr) {

    this.txtConfirmacionGlobal = "¿Está seguro de editar la ubicación del módulo de Protecciones?";

    $("#span_pop_title").text("Editar ubicación del módulo de Protecciones");

    $.ajax({
        type: 'POST',
        url: controlador + "Editar?id=" + e + "&idEpr=" + epr,
        success: function (evt) {
            $('#editarArea').html(evt);
            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popUpEditar').bPopup({
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
function agregarUbicacion(e) {

    this.txtConfirmacionGlobal = "¿Está seguro de agregar la ubicación al módulo de Protecciones?";

    $("#span_pop_title").text("Agregar ubicación al módulo de Protecciones");

    $.ajax({
        type: 'POST',
        url: controlador + "Editar?id=" + e + "&idEpr=" + 0,
        success: function (evt) {
            $('#editarArea').html(evt);
            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popUpEditar').bPopup({
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

let mostrarError = function () {
    $('#mensaje').removeClass("action-exito");
    $('#mensaje').addClass("action-error");
    $('#mensaje').text("Ha ocurrido un error");
    $('#mensaje').css("display", "block");
};


