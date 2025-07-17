//Declarando las variables 
let controlador = siteRoot + 'HistorialCambioSE/';
let hot;
let hotOptions;
let evtHot;

$(function () {
    $("#container").addClass("general-popup");

    consultar();

    $("#popUpEditar").addClass("general-popup");

    $('#btnBuscar').click(function () {
        consultar();
    });

    $('#btnNuevo').click( function(){
        editar(0, 'NUEVO');
    })

    $('#btnExportar').click(function () {
        exportarListado();
    });

});



function validarGuardar() {
    let campos = [];

    if ($('#areacodi').val() == '') {
        campos.push('Subestación');
    }

    if ($('#epproycodi').val() == '') {
        campos.push('Proyecto');
    }

    if ($('#epsubemotivo').val() == '' || $('#epsubemotivo').val().trim().length == 0) {
        campos.push('Motivo');
    }

    if ($('#epsubefecha').val() == '') {
        campos.push('Fecha');
    }

    if ($("#hdnCargado").val() == 0) {
        campos.push('Memoria de cálculo');
    }

    if ($('#idCelda').val() == '') {
        campos.push('Celda');
    } 

    if (campos.length > 0) {
        
        mensajeValidador(campos);
        return false;
    }

    return true;
}

function mensajeValidador(campos) {
    let texto = "Los campos \n";
    for (let campo of campos) {
        texto = texto + campo + "(*) \n";
    }
    alert(texto + "son requeridos");
}

let loadValidacionFile = function (mensaje) {
    alert(mensaje);
}

let guardar = function () {
    if (validarGuardar()) {

        
        let epsubecodi = $('#epsubecodi').val();
        let epproycodi = $('#epproycodi').val();
        let areacodi = $('#areacodi').val();
        let epsubefecha = $('#epsubefecha').val();
        let epsubemotivo = $('#epsubemotivo').val().trim();
        let epsubememoriacalculo = $('#hdnEpsubememoriacalculo').val();
        let accion = $('#accion').val();

        let textoConfirmación = '¿Está seguro de agregar Memoria de Cálculo Subestación?'
        if (accion =="EDITAR") {
            textoConfirmación = '¿Está seguro de editar Memoria de Cálculo Subestación?'
        }

        if (confirm(textoConfirmación)) {

            let datos = {
                areacodi: areacodi,
                epproycodi: epproycodi,
                epsubefecha: epsubefecha,
                epsubemotivo: epsubemotivo,
                epsubecodi: epsubecodi,
                epsubememoriacalculo: epsubememoriacalculo,
                accion: accion
            };

            $.ajax({
                type: 'POST',
                url: controlador + 'Guardar',
                dataType: 'json',
                data: datos,
                cache: false,
                success: function (resultado) {
                    if (resultado == 1) {
                        $('#popUpEditar').bPopup().close();
                        consultar();
                    } else if (resultado == 2) {
                        alert("Subestación y fecha duplicado, ingrese otro.");
                    } else {
                        mostrarError();
                    }
                },
                error: function () {
                    mostrarError();
                }
            });
        }
    }
};

let mensajeExito = function () {
    $('#popupFormatoEnviadoOk').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
}

let mostrarError = function () {
    $('#mensaje').removeClass("action-exito");
    $('#mensaje').addClass("action-error");
    $('#mensaje').text("Ha ocurrido un error");
    $('#mensaje').css("display", "block");
};

$.fn.dataTable.ext.type.order['date-dd-mm-yyyy-pre'] = function (date) {
    let parts = date.split('/');
    return new Date(parts[2], parts[1] - 1, parts[0]).getTime();
};

let consultar = function () {
    let areacodi = $("#idSubestacion").val() == '' ? 0 : $("#idSubestacion").val();
    let zonacodi = $("#idZona").val() == '' ? 0 : $("#idZona").val();
    setTimeout(function () {
        $.ajax({
            type: 'POST',
            url: controlador + 'ListaHistorialCambioSE',
            data: {
                areacodi: areacodi,
                zonacodi: zonacodi
            },
            success: function (evt) {
                $('#lista').html(evt);
                $('#tablaListado').dataTable({
                    "iDisplayLength": 25,
                    columnDefs: [
                        { type: 'date-dd-mm-yyyy', targets: 4 } 
                    ],
                    language: {
                        info: 'Mostrando página _PAGE_ de _PAGES_',
                        infoEmpty: 'No hay registros disponibles',
                        infoFiltered: '(filtrado de _MAX_ registros totales)',
                        lengthMenu: 'Mostrar _MENU_ registros por página',
                        zeroRecords: 'No se encontró nada'
                    },
                    order:[[4,"desc"],[1,"asc"]]
                });

                $('.dataTables_filter input').attr('maxLength', 50);
            },
            error: function () {
                mostrarMensaje('Ha ocurrido un error.');
            }
        });
    }, 100);
};

let eliminar = function (e) {

    let textoConfirmación = '¿Está seguro de eliminar la Memoria de Cálculo Subestación?';

    if (confirm(textoConfirmación)) {

        let datos = {
            epsubecodi: e
        };

        $.ajax({
            type: 'POST',
            url: controlador + 'Eliminar',
            dataType: 'json',
            data: datos,
            cache: false,
            success: function (resultado) {
                if (resultado == 1) {
                    consultar();
                } else
                    mostrarError();
            },
            error: function () {
                mostrarError();
            }
        });
    }
};

function descargarArchivo(nomArchivo, epsubecodi) {
    window.location = controlador + 'DescargarArchivo?fileName=' + nomArchivo + '&epsubecodi=' + epsubecodi;

}

function mostrarMensaje(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-message');
}

function soloConsulta() {

    desactivarControles(['areacodi',
        'epproycodi',
        'epsubemotivo',
        'epsubefecha',
        'epsubememoriacalculo'
    ]);

    $("#btnGrabar").hide();
    $("#btnUpload").hide();
    $("#btnCargar").hide();
}

function desactivarControles(arrayControles) {
    for (let control of arrayControles) {
        $('#' + control).prop('disabled', true);
    }
}

function editar(e, accion) {
    $.ajax({
        type: 'POST',
        url: controlador + "Editar?id=" + e,
        data: {
            id: e,
            accion:accion
        },
        success: function (evt) {
            $('#editarArea').html(evt);
            setTimeout(function () {
                $('#popUpEditar').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    width: 1500
                });

                let today = new Date();
                let yyyy = today.getFullYear();
                let mm = today.getMonth() + 1;
                let dd = day_of_the_month(today);
                let maxFecha = dd + "/" + mm + "/" + yyyy;

                $('#epsubefecha').Zebra_DatePicker({
                    show_clear_date: 0,
                    direction: [false, maxFecha]
                });

                switch (accion) {
                    case "EDITAR":
                        $("#tituloEditar").html("Actualización de Memoria de Cálculo Subestación");
                        if ($("#epsubememoriacalculo").val() == "") {
                            $("#hdnCargado").val(0);
                        } else {
                            $("#hdnCargado").val(1);
                        }
                        
                        break;
                    case "NUEVO":
                        $("#tituloEditar").html("Creación de Memoria de Cálculo Subestación");
                        $("#hdnCargado").val(0);
                        break;
                    case "CONSULTAR":
                        $("#tituloEditar").html("Consulta de Memoria de Cálculo Subestación");
                        soloConsulta();
                        break;
                }
            }, 50);
       
        },
        error: function () {
            mostrarError();
        }
    });
};

function exportarListado() {
    let areacodi = $("#idSubestacion").val() == '' ? 0 : $("#idSubestacion").val();
    let zonacodi = $("#idZona").val() == '' ? 0 : $("#idZona").val();

    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarArchivoZip',
        data: {
        areacodi: areacodi,
        zonacodi: zonacodi
        }, 
        dataType: 'json',
        success: function (evt) {

            if (evt.Resultado != "-1") {
                window.location = controlador + "ExportarArchivoZip?file_name=" + evt.Resultado + "&ruta="+ evt.Ruta;
            } else {
                mostrarMensaje('Error: ' + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('Ha ocurrido un error.');
        }
    });
}

let convertirCadenaFecha = function (dateString) {
    let d = dateString.split("/");
    let dat = new Date(d[2] + '/' + d[1] + '/' + d[0]);
    return dat;
};

function day_of_the_month(d) {
    return (d.getDate() < 10 ? '0' : '') + d.getDate();
}

function validacionHistorialCambiosSE() {

    let msj = "";

    if ($('#areacodi').val() <= 0) {
        msj = "Seleccione Subestación\n";
    }

    if ($('#epproycodi').val() <= 0) {
        msj = msj + "Seleccione Proyecto\n";
    }

    if ($("#epsubemotivo").val() == "" || $('#epsubemotivo').val().trim().length == 0) {
        msj = msj + "Ingrese Motivo\n";
    }

    if ($("#epsubefecha").val() == "") {
        msj = msj + "Ingrese Fecha\n";
    }

    if ($("#epsubememoriacalculo").val() == "" || $('#epsubememoriacalculo').val().trim().length == 0) {
        msj = msj + "Ingrese Memoria de Cálculo\n";
    }

    if (msj != null && msj != "") {
        alert(msj);
        return false;
    } else {
        return true;
    }

}