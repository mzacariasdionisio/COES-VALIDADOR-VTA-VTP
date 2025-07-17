var controlador = siteRoot + 'Equipamiento/FichaTecnica/';
var OPCION_VER = 1;
var OPCION_EDITAR = 2;
var OPCION_NUEVO = 3;

var IMG_VER = `<img src="${siteRoot}Content/Images/btn-open.png" title="Ver Ficha Maestra"/>`;
var IMG_EDITAR = `<img src="${siteRoot}Content/Images/btn-edit.png" title="Editar Ficha Maestra"/>`;
var IMG_ELIMINAR = `<img src="${siteRoot}Content/Images/Trash.png" title="Eliminar Ficha Maestra"/>`;

$(function () {
    $('#btnConsultar').on('click', function () {
        listarFichaMaestra();
    });

    $('#btnAgregar').on('click', function () {
        nuevaFichaMaestra();
    });

    listarFichaMaestra();
});

///////////////////////////
/// Consulta
///////////////////////////
function listarFichaMaestra() {
    $.ajax({
        type: 'POST',
        url: controlador + 'FichaMaestraLista',
        data: {
        },
        success: function (result) {
            if (result.Resultado != "-1") {
                $('#listado').html(result);

                var html = _dibujarTablaListado(result);
                $('#listado').html(html);

                $('#tabla').dataTable({
                    "sDom": 'ft',
                    "ordering": false,
                    "iDisplayLength": 50
                });

                viewEvent();
            } else {
                alert(result.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function _dibujarTablaListado(model) {
    var lista = model.ListaFichaMaestra;

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="tabla" cellspacing="0" width="100%" >
        <thead>
            <tr>
                <th style="width: 100px">Acciones</th>
                <th style="width: 50px">Código</th>
                <th style="width: 240px">Nombre</th>
                <th style="width: 100px">Oficial</th>
                <th style="width: 100px">Ambiente</th>
                <th style="width: 120px">Usuario modificación</th>
                <th style="width: 120px">Fecha Modificación</th>
                <th style="width: 120px">Usuario registro</th>
                <th style="width: 120px">Fecha registro</th>
                <th style="width: 70px">Estado</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];

        var claseFila = "";
        if (item.Ftecestado == "B") { claseFila = "clase_baja"; }
        if (item.Ftecestado == "X") { claseFila = "clase_eliminado"; }
        if (item.Ftecestado == "A" && item.Ftecprincipal == 1) { claseFila = "clase_principal"; }

        var tdOpciones = _tdAcciones(model, item);

        cadena += `

            <tr id="fila_${item.Concepcodi}" class="${claseFila}">
                ${tdOpciones}
                <td style="text-align:center;">${item.Fteccodi}</td>
                <td>${item.Ftecnombre}</td>
                <td>${item.FtecprincipalDesc}</td>
                <td>${item.FtecambienteDesc}</td>
                <td>${item.Ftecusumodificacion}</td>
                <td>${item.FtecfecmodificacionDesc}</td>
                <td>${item.Ftecusucreacion}</td>
                <td>${item.FtecfeccreacionDesc}</td>
                <td>${item.FtecestadoDesc}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}

function _tdAcciones(Model, item) {
    var html = '';

    html += `<td>`;
    html += `   
                <a href="#" id="view, ${item.Fteccodi}" class="viewVer">
                    ${IMG_VER}
                </a>
                <a href="#" id="view, ${item.Fteccodi}" class="viewEdicion">
                    ${IMG_EDITAR}
                </a>
                <a href="#" id="view, ${item.Fteccodi}" class="viewEliminar">
                    ${IMG_ELIMINAR}
                </a>
        `;

    return html;
}

function viewEvent() {
    $('.viewVer').click(function (event) {
        event.preventDefault();
        idfm = $(this).attr("id").split(",")[1];
        verFichaMaestra(idfm);
    });
    $('.viewEdicion').click(function (event) {
        event.preventDefault();
        idfm = $(this).attr("id").split(",")[1];
        editarFichaMaestra(idfm);
    });
    $('.viewEliminar').click(function (event) {
        event.preventDefault();
        idfm = $(this).attr("id").split(",")[1];
        eliminarFichaMaestra(idfm);
    });
};

///////////////////////////
/// Edición
///////////////////////////

function eliminarFichaMaestra(id) {
    if (confirm('¿Desea eliminar la Ficha Maestra?')) {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            traditional: true,
            url: controlador + 'FichaMaestraEliminar',
            data: {
                id: id
            },
            cache: false,
            success: function (result) {
                if (result.Resultado == '-1') {
                    alert('Ha ocurrido un error:' + result.Mensaje);
                } else {
                    alert("Se eliminó correctamente la Ficha Maestra");
                    listarFichaMaestra();
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error.');
            }
        });
    }
}

function editarFichaMaestra(id) {
    formularioFichaMaestra(id, OPCION_EDITAR);
}

function verFichaMaestra(id) {
    formularioFichaMaestra(id, OPCION_VER);
}

function nuevaFichaMaestra() {
    formularioFichaMaestra(0, OPCION_NUEVO);
}

function formularioFichaMaestra(id, opcion) {
    $.ajax({
        type: 'POST',
        url: controlador + "FichaMaestraFormulario",
        data: {
            id: id
        },
        success: function (evt) {
            $('#editarFichaMaestra').html(evt);
            $('#mensaje').css("display", "none");

            inicializarFichaMaestra(opcion);

            setTimeout(function () {
                $('#popupCrear').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);

        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function inicializarFichaMaestra(opcion) {
    var checkPrin = $("#hfPrincipal").val();
    $('#chkPrincipal').prop('checked', checkPrin == "1");
    $("#txtNombre").val($("#hfNombre").val());
    var boolMostrarEstado = !$("#chkPrincipal").prop("checked");

    switch (opcion) {
        case OPCION_VER:
            $("#cbEstado").val($("#hfEstado").val());
            $("#cbAmbiente").val($("#hfAmbiente").val());

            $("#tdCancelar2").show();
            $(".tbfiltroFT").hide();
            $("#lbTeamB").parent().show();
            $(".tr_estado").show();
            $(".tr_ambiente").show();
            break;
        case OPCION_EDITAR:
            $("#cbEstado").val($("#hfEstado").val());
            $("#cbAmbiente").val($("#hfAmbiente").val());

            $("#tdGuardar").show();
            $("#tdCancelar").show();
            $("#txtNombre").removeAttr('disabled');
            $("#chkPrincipal").removeAttr('disabled');
            $("#lbTeamA").removeAttr('disabled');
            $("#lbTeamB").removeAttr('disabled');
            $("#cbEstado").removeAttr('disabled');
            //$("#cbAmbiente").removeAttr('disabled');
            $(".tbfiltroFT").show();
            $(".tr_ambiente").show();
            if (boolMostrarEstado) {
                $(".tr_estado").show();
                $("#cbAmbiente").removeAttr('disabled');
            }

            break;
        case OPCION_NUEVO:
            $("#tdGuardar").show();
            $("#tdCancelar").show();
            $("#txtNombre").removeAttr('disabled');
            $("#chkPrincipal").removeAttr('disabled');
            $("#lbTeamA").removeAttr('disabled');
            $("#lbTeamB").removeAttr('disabled');
            $("#cbAmbiente").removeAttr('disabled');
            $(".tbfiltroFT").show();
            $(".tr_estado").show();
            $(".tr_ambiente").show();
            break;
    }

    if (checkPrin == "1")
        $(".tr_estado").hide();

    $('#chkPrincipal').change(function () {
        if (this.checked) {
            $(".tr_estado").hide();
        } else {
            if (boolMostrarEstado)
                $(".tr_estado").show();
        }
    });

    //
    $("#btnMoveRight").unbind();
    $("#btnMoveRight").on("click", function (e) {
        var selectedOptions = $('#lbTeamA > option:selected');
        if (selectedOptions.length == 0) {
            alert("Seleccionar al menos una Ficha Técnica y mover");
            return false;
        }

        $('#lbTeamA > option:selected').appendTo('#lbTeamB');
        e.preventDefault();
    });

    $("#btnMoveAllRight").unbind();
    $("#btnMoveAllRight").on("click", function (e) {
        $('#lbTeamA > option').appendTo('#lbTeamB');
        e.preventDefault();
    });

    $("#btnMoveLeft").unbind();
    $("#btnMoveLeft").on("click", function (e) {
        var selectedOptions = $('#lbTeamB > option:selected');
        if (selectedOptions.length == 0) {
            alert("Seleccionar al menos un numero y mover");
            return false;
        }

        $('#lbTeamB > option:selected').appendTo('#lbTeamA');
        e.preventDefault();
    });

    $("#btnMoveAllLeft").unbind();
    $("#btnMoveAllLeft").on("click", function (e) {
        $('#lbTeamB > option').appendTo('#lbTeamA');
        e.preventDefault();
    });

    //
    $("#btnGuardar").unbind();
    $('#btnGuardar').click(function () {
        guardarFichaMaestra();
    });
    $("#btnCancelar").unbind();
    $('#btnCancelar').click(function () {
        cerrarPopup();
    });
    $("#btnCancelar2").unbind();
    $('#btnCancelar2').click(function () {
        cerrarPopup();
    });
}

function guardarFichaMaestra() {
    var entity = getObjetoJson();

    var msj = validarJson(entity);

    if (msj == "") {

        if (confirm('¿Desea guardar la Ficha Maestra?')) {
            limpiarMensaje();
            $.ajax({
                type: 'POST',
                dataType: 'json',
                traditional: true,
                url: controlador + 'FichaMaestraGuardar',
                data: {
                    id: entity.id,
                    nombre: entity.nombre,
                    checkPrincipal: entity.check,
                    listaSelec: entity.listaSelect,
                    estado: entity.estado,
                    ambiente: entity.ambiente
                },
                cache: false,
                success: function (result) {
                    if (result.Resultado == '-1') {
                        alert(result.Mensaje);
                    } else {
                        alert("Se guardó correctamente la Ficha Maestra");
                        cerrarPopup();
                        listarFichaMaestra();
                    }
                },
                error: function (err) {
                    alert('Ha ocurrido un error.');
                }
            });
        }
    } else {
        mostrarAlerta(msj);
    }
}

function getObjetoJson() {
    var listaFT = [];
    $("#lbTeamB option").each(function () {
        listaFT.push($(this).val());
    });

    var obj = {};
    obj.id = parseInt($("#hfIdMaestra").val()) || 0;
    obj.listaSelect = listaFT;
    obj.nombre = $("#txtNombre").val();
    obj.check = $("#chkPrincipal").prop("checked") ? 1 : 0;
    obj.estado = $("#cbEstado").val();
    obj.ambiente = $("#cbAmbiente").val();

    return obj;
}

function validarJson(obj) {
    var validacion = "<ul>";
    var flag = true;

    if (obj.nombre == null || obj.nombre == "") {
        validacion += "<li>Debe ingresar nombre.</li>";
        flag = false;
    }
    if (obj.listaSelect.length == 0) {
        validacion += "<li>Debe seleccionar al menos una Ficha Técnica.</li>";
        flag = false;
    }

    if (obj.ambiente == 0) {
        validacion += "<li>Debe seleccionar el ambiente.</li>";
        flag = false;
    }

    validacion = validacion + "</ul>";

    if (flag == true) validacion = "";

    return validacion;
}

mostrarAlerta = function (alerta) {
    $('#mensaje2').html(alerta);
    $('#mensaje2').css("display", "block");
}

limpiarMensaje = function () {
    $('#mensaje2').html("");
    $('#mensaje2').css("display", "none");
}

function cerrarPopup() {
    $('#popupCrear').bPopup().close();
}
