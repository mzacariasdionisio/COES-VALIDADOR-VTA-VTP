//Declarando las variables 
let controlador = siteRoot + 'EquipoCOES/';
let hot;
let hotOptions;
let evtHot;
let txtConfirmacionGlobal;

$(function () {

    consultar();

    $("#popUpEditar").addClass("general-popup");

    $('#btnBuscar').click(function () {
        buscarEquipo();
    });

    $('#btnExportar').click(function () {
        exportarListado();
    });

});

function buscarEquipo() {
    consultar();
}

let consultar = function () {

    let ubicacion = $('#Ubicacion').val();
    let tipo = $('#Tipo').val();
    let area = $('#nombreArea').val();

    if (ubicacion == 0) {
        ubicacion = "";
    } 

    if (tipo == 0) {
        tipo = "";
    }

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
            url: controlador + 'ListaEquipo?idUbicacion=' + ubicacion + "&idTipoEquipo=" + tipo + "&nombreEquipo=" + area + "&sProgramaExistente=" + programaExistente,
            success: function (evt) {
                $('#lista').html(evt);
                $('#tablaListado').dataTable({
                    "iDisplayLength": 25,
                    language: {
                        info: 'Mostrando página _PAGE_ de _PAGES_',
                        infoEmpty: 'No hay registros disponibles',
                        infoFiltered: '(filtrado de _MAX_ registros totales)',
                        lengthMenu: 'Mostrar _MENU_ registros por página',
                        zeroRecords: 'No se encontró nada'
                    },
                    order: [[0, "desc"], [4,"asc"],[5,"asc"]]
                });

                $('.dataTables_filter input').attr('maxLength', 50);
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }, 100);
};


let guardarEquipo = function () {

    if (validar()) {
        let codigo = $('#Id').val();
        let codigoEpr = $('#IdEpe').val();
    
        let textoConfirmación = this.txtConfirmacionGlobal;

        let nombre = "";
        if ($('#nombreProteccion').val() != null && $('#nombreProteccion').val() != "") {
            nombre = $('#nombreProteccion').val().trim();
        }

        if (confirm(textoConfirmación)) {

            let datos = {
                iCodigo: codigo,
                iPrCodigo: codigoEpr,
                sFlag: 1,
                sNombre: nombre
            };
        
            $.ajax({
                type: 'POST',
                url: controlador + 'GuardarEquipo',
                dataType: 'json',
                data: datos,
                cache: false,
                success: function (resultado) {
                    if (resultado == 1) {
                        $('#popUpEditar').bPopup().close();
                        buscarEquipo();
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
    if ($("#chkEquipo").is(':checked')) {
        return $('#nombreProteccion').val().trim().length > 0;
    } else {
        return true;
    }
}

let eliminarEquipo = function (e) {
    
    let textoConfirmación = '¿Está seguro de quitar del módulo de Protecciones?';

    if (confirm(textoConfirmación)) {

        let datos = {
            iPrCodigo: e
        };
        
        $.ajax({
            type: 'POST',
            url: controlador + 'EliminarEquipo',
            dataType: 'json',
            data: datos,
            cache: false,
            success: function (resultado) {

                switch (resultado.Estado) {
                    case 1:
                        buscarEquipo();
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

function editarEquipo(e, epe) {

    this.txtConfirmacionGlobal = "¿Está seguro de editar nombre del Equipo?";

    $("#span_pop_title").text("Editar nombre del Equipo");

    $.ajax({
        type: 'POST',
        url: controlador + "EditarEquipo?id=" + e + "&idEpe=" + epe,
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
function agregarEquipo(e) {

    this.txtConfirmacionGlobal = "¿Está seguro de agregar al módulo de Protecciones?";

    $("#span_pop_title").text("Agregar al módulo de Protecciones");

    $.ajax({
        type: 'POST',
        url: controlador + "EditarEquipo?id=" + e + "&idEpe=" + 0,
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

function exportarListado() {

    $.ajax({
        type: 'POST',
        url: controlador + 'ExportarEquipoCOES',
        data: {
            iArea: $("#Ubicacion").val(),
            iFamilia: $("#Tipo").val(),
            sNombre: $("#nombreArea").val()
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
