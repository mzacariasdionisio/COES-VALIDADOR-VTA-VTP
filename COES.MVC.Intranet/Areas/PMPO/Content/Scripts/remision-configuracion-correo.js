var controlador = siteRoot + 'PMPO/ConfiguracionParametros/';

var ANCHO_REPORTE = 1000;

$(function () {

    $('#btnModificarConfiguracion').click(function () {
        guardarConf();
    });

    consultarListado();
});

function consultarListado() {
    ANCHO_REPORTE = $("#mainLayout").width() > 1100 ? $("#mainLayout").width() - 30 : 1100;

    $.ajax({
        type: 'POST',
        url: controlador + "ListaConfiguracionCorreo",
        dataType: 'json',
        data: {
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {

                $('#reporte').html(dibujarTablaListado(evt.ListaConfig));

                $('#tablaListado').dataTable({
                    "scrollY": 330,
                    "scrollX": true,
                    "sDom": 't',
                    "ordering": false,
                    "bPaginate": false,
                    "iDisplayLength": -1
                });
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function editarConf(id, formato, asunto) {
    $("#idParamEdit").val(id);
    $("#txtFormato").html(formato);
    $("#txtAsuntoEdit").html(asunto);

    setTimeout(function () {
        $('#popupEditar').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    });
}

function guardarConf() {
    var confpmcodi = parseInt($("#idParamEdit").val())|| 0;
    var asunto = $("#txtAsuntoEdit").val();

    $.ajax({
        type: 'POST',
        url: controlador + "GuardarConfCorreo",
        dataType: 'json',
        data: {
            confpmcodi: confpmcodi,
            asunto: asunto,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $('#popupEditar').bPopup().close();
                consultarListado();
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function dibujarTablaListado(lista) {

    var cadena = '';
    cadena += `
    <div style='clear:both; height:5px'></div>
    <table id='tablaListado' border='1' class='pretty tabla-icono' cellspacing='0'>
        <thead>
            <tr>
                <th>Editar</th>
                <th>Tipo de Parámetro</th>
                <th>Parámetro</th>
                <th>Valor</th>
                <th>Fecha creación</th>
                <th>Usuario de creación</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        cadena += `
            <tr>
                <td style='text-align: center;' onclick="editarConf(${ lista[key].Confpmcodi}, '${lista[key].ParametroDesc}', '${lista[key].Confpmvalor}');" style='cursor:pointer'>
                    <a>
                        <img src="${siteRoot}Content/Images/btn-edit.png" />
                    </a>
                </td>

                <td style='text-align: center;height: 30px;'>${lista[key].TipoParametroDesc}</td>
                <td style='text-align: center;'>${lista[key].ParametroDesc}</td>
                <td style='text-align: center;'>${lista[key].Confpmvalor}</td>
                <td style='text-align: center;'>${lista[key].UltimaModificacionUsuarioDesc}</td>
                <td style='text-align: center;'>${lista[key].UltimaModificacionFechaDesc}</td>               
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}