var ancho = 900;
var oTable2;

$(function () {

    ancho = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 30 : 900;

    $('#btnNuevaNota').bind('click', function (e) {
        initModalNota();

        openPopupCrear();
    });

    cargarValoresIniciales();
});

function mostrarReporteByFiltros() {
    cargarNota();
}

function cargarValoresIniciales() {
    cargarNota();
}

function initModalNota() {
    $("#tableContent").hide();
}

function openPopupCrear() {

    $('#popupDetalle').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown',
        transitionClose: 'slideUp',
        modalClose: false
    });
}

function guardarNota() {
    var periodoNota = getFechaInicio();

    var nota = {
        Sinotacodi: $("#txtSinotacodi").val(),
        Sinotadesc: $("#txtSinotadesc").val(),
        SinotaperiodoDesc: periodoNota,
        Mrepcodi: $("#hdReporcodi").val()
    };

    var msj = validarNota(nota);

    if (msj == "") {
        $.ajax({
            type: "POST",
            url: controlador + "GuardarNota",
            data: nota
        }).done(function (result) {
            if (result.Resultado != "-1") {
                limpiarFormularioNota();
                cargarNota();
                alert("La observación ha sido registrada correctamente");
            } else {
                alert("Ha ocurrido un error");
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            alert("Ha ocurrido un error");
        });
    } else {
        alert(msj);
    }
}

function validarNota(nota) {
    var msj = '';

    if (nota.Sinotadesc == "" || nota.Sinotadesc == null) {
        msj += "Ingrese descripción de nota.\n";
    } else {
        if (nota.Sinotadesc.length > 500)
            msj += "Máximo 500 caracteres.\n";
    }
    return msj;
}

function eliminarNota(Sinotacodi) {
    $.ajax({
        type: "POST",
        url: controlador + "DeleteNota",
        data: { Sinotacodi: Sinotacodi }
    }).done(function (result) {
        cargarNota();
        alert("La observación ha sido eliminada correctamente");
    }).fail(function (jqXHR, textStatus, errorThrown) {
        alert("Ha ocurrido un error");
    });
}

function cargarNota() {
    $("#tableContent").show("slow");
    $("#listaNota").html('');

    var repcodi = $("#hdReporcodi").val();
    $.ajax({
        type: "POST",
        async: false,
        url: controlador + "CargarListaNota",
        data: {
            periodo: getFechaInicio(),
            mrepcodi: repcodi
        },
        success: function (result) {
            var html = `
        <table class="pretty tabla-icono" id="tablaNota" width="100%">
                        <thead>
                            <tr>
                                <th width="10%">Orden</th>
                                <th width="50%">Descripción</th>
                                <th width="10%">Usuario</th>
                                <th width="10%">Última actualización</th>
                                <th width="10%">Acción</th>
                            </tr>
                        </thead>
                        <tbody></tbody>
                    </table>

        `;
            $("#listaNota").html(html);

            $("#tablaNota tbody").html('');
            for (var i = 0; i < result.length; i++) {
                var reg = result[i];
                var strHtml = '';
                strHtml += ('<tr id=' + reg.Sinotacodi + '>');
                strHtml += ('<td>' + reg.Sinotaorden + '</td>');
                strHtml += ('<td>' + reg.Sinotadesc + '</td>');
                strHtml += ('<td>' + reg.UltimaModificacionUsuarioDesc + '</td>');
                strHtml += ('<td>' + reg.UltimaModificacionFechaDesc + '</td>');
                strHtml += ('<td><input type="button" onclick="eliminarNota(' + reg.Sinotacodi + ');" value=Eliminar /></td>');
                strHtml += ('</tr>');
                $("#tablaNota tbody").append(strHtml);
            }

            oTable2 = $('#tablaNota').dataTable({
                "bJQueryUI": true,
                "scrollY": 650,
                "scrollX": true,
                "sDom": 't',
                "ordering": true,
                "iDisplayLength": 400,
                "sPaginationType": "full_numbers"
            });
            var strPeriodo = getFechaInicio().replace('/', 'a');
            strPeriodo = strPeriodo.replace('/', 'a');
            //oTable2.rowReordering({ sURL: controlador + "/UpdateListaNota?periodo=" + strPeriodo + "&mrepcodi=" + repcodi });
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function limpiarFormularioNota() {
    $("#txtSinotadesc").val("");
}