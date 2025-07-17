var controler = siteRoot + "Intervenciones/Registro/";

$(document).ready(function ($) {

    $('#Empresa').multipleSelect({
        placeholder: "-- Todos --",
        selectAll: true,
        filter: true,
        allSelected: onoffline,
        onClose: function (view) {
            setearFilterEmpresa();
        }
    });

    var idsEmpresas = $("#hfEmpresa").val();
    var arrEmp = idsEmpresas.split(',');
    $("#Empresa").multipleSelect('setSelects', arrEmp);

    $("#CrearProgramacion").click(function () {
        var idprogra = document.getElementById('tipoProgramacion').value;
        AbrirNuevaProgra(idprogra);
    });

    listarProgramas();

    $("#btnManualUsuario").click(function () {
        window.location = controler + 'DescargarManualUsuario';
    });
});

function listarProgramas() {
    var idTipoProgramacion = parseInt( document.getElementById('tipoProgramacion').value) || 0;

    $('#listado').html('');

    if (idTipoProgramacion > 0) {
        $.ajax({
            type: 'POST',
            url: controler + "ProgramacionesListado",
            datatype: "json",
            contentType: 'application/json',
            data: JSON.stringify({ idTipoProgramacion: idTipoProgramacion }),
            success: function (evt) {
                $('#listado').html(evt);

                if (idTipoProgramacion != 1 && idTipoProgramacion != 5)
                    $('#CrearProgramacion').css("visibility", "visible");
                else
                    $('#CrearProgramacion').css("visibility", "hidden");

                var excep_resultado = $("#hdResultado_PL").val();
                var excep_mensaje = $("#hdMensaje_PL").val();
                var excep_detalle = $("#hdDetalle_PL").val();

                if (excep_resultado != "-1") {
                    $('#tabla1').dataTable({
                        "sPaginationType": "full_numbers",
                        "destroy": "true",
                        "ordering": false,
                        "searching": false,
                        "iDisplayLength": 15,
                        "info": false,
                        "paging": false,
                        "scrollY": $('#listado').height() > 500 ? "560px" : "100%"
                    });
                } else {
                    $('#listado').html('');
                    alert(excep_mensaje);
                }
            },
            error: function () {
                alert("Lo sentimos, se ha producido un error");
            }

        });
    }
};

function setearFilterEmpresa() {
    var idEmpresa = $('#Empresa').val();
    idEmpresa = idEmpresa.join(",");

    $.ajax({
        type: 'POST',
        url: controler + 'SetearEmpresaFilter',
        data: { emprcodis: idEmpresa },
        success: function (e) {

        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function check(progCodi) {
    var idEmpresa = JSON.stringify($('#Empresa').val());

    if (idEmpresa != null && idEmpresa != "") {

        //Observación parametros en la URL - Modificado 15/05/2019
        var paramList = [
            { tipo: 'input', nombre: 'progCodi', value: progCodi },
        ];
        var form = CreateForm(controler + 'IntervencionesRegistro', paramList);
        document.body.appendChild(form);
        form.submit();
        return true;
        //:fin

        //window.location.href = controler + 'IntervencionesRegistro?progCodi=' + progCodi + '&tipoProgramacion=' + idTipoProgramacion + '&IdEmpresa=' + idEmpresa + '&flgSoloLectura=' + flgSoloLectura;
        //return true;
    } else {
        alert("Debes seleccionar una empresa");
        return false;
    }
}

function toDate(dateStr) {
    var parts = dateStr.split("/");
    return new Date(parts[2], parts[1] - 1, parts[0]);
}

function CreateForm(path, params, method = 'post') {
    var form = document.createElement('form');
    form.method = method;
    form.action = path;

    $.each(params, function (index, obj) {
        var hiddenInput = document.createElement(obj.tipo);
        hiddenInput.type = 'hidden';
        hiddenInput.name = obj.nombre;
        hiddenInput.value = obj.value;
        form.appendChild(hiddenInput);
    });
    return form;
}

//popput nuevo programa
function AbrirNuevaProgra(idprogra) {
    $.ajax({
        type: 'POST',
        url: controler + 'NuevaProgramacion',
        data: {
            idprogramacion: idprogra
        },
        success: function (result) {

            $('#tablaNewprograma').html(result);

            $('#listadoProgramas').html(result);

            setTimeout(function () {
                $('#popupProgramacion').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 250);
        },
        error: function (err) {
            alert('Ha ocurrido un error: ' + err);
        }
    });
}

//guardar Programación
function guardarProgramacion() {
    var entity = getObjetoProgramacionJson();
    if (confirm('¿Desea guardar el programa?')) {
        var msj = validarProgramacion(entity);
        if (msj == "") {
            $.ajax({
                type: 'POST',
                dataType: 'json',
                traditional: true,
                url: controler + "NuevaProgramacionGuardar",
                data: {
                    idprogramacion: entity.Evenclasecodi,
                    strfechaInicio: entity.Progrfechaini
                },
                cache: false,
                success: function (result) {
                    if (result.Resultado == '-1' && result.StrMensaje != "" || result.StrMensaje != null) {
                        alert(result.StrMensaje);
                    } else {
                        alert("Se guardó correctamente el registro");
                        $('#popupProgramacion').bPopup().close();
                        listarProgramas();
                    }
                },
                error: function (err) {
                    alert('Ha ocurrido un error.');
                }
            });
        } else {
            alert(msj);
        }
    }
}

//Json para nueva programación
function getObjetoProgramacionJson() {
    var obj = {};
    obj.Evenclasecodi = $("#idTipoProgra").val();

    if (obj.Evenclasecodi == 2) {
        obj.Progrfechaini = $("#InterfechainiD").val();
    } else if (obj.Evenclasecodi == 3) {
        obj.Progrfechaini = $('#cboSemanaIni').val();
    } else if (obj.Evenclasecodi == 4) {
        obj.Progrfechaini = $('#mes').val();
    }
    return obj;
}
//validar programación
function validarProgramacion(obj) {
    var msj = "";
    if (obj.Progrfechaini == null || obj.Progrfechaini == '') {
        msj += "Debe seleccionar una fecha inicio";
    }
    var fechaLimite = new Date();

    switch (obj.Evenclasecodi) {
        case "2": // diarios
            fechaLimite.setDate(fechaLimite.getDate() + 15);

            if (formatFecha(obj.Progrfechaini) > fechaLimite) {
                msj += "No se permite crear programas mayor a 15 días.";
            }

            break;
        case "3": // semanal
            fechaLimite.setDate(fechaLimite.getDate() + 42);

            if (formatFecha(obj.Progrfechaini) > fechaLimite) {
                msj += "No se permite crear programas mayor a 6 semanas.";
            }
            break;
        case "4": // mensual
            fechaLimite.setMonth(fechaLimite.getMonth() + 2);

            if (formatFechaMes(obj.Progrfechaini) > fechaLimite) {
                msj += "No se permite crear programas mayor a 2 meses.";
            }
            break;
    }

    return msj;
}

function formatFecha(sFecha) {   // DD/MM/AAAA
    if (sFecha == "") return false;

    var sDia = sFecha.substring(0, 2);
    var sMes = sFecha.substring(3, 5);
    var sAnio = sFecha.substring(6, 10);

    return new Date(sAnio, sMes - 1, sDia);
}

function formatFechaMes(date) { // MM AAAA
    var parts = date.split(" ");
    return new Date(parts[1], parts[0] - 1, 1);
}