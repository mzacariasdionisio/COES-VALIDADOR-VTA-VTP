var controladorParametro = siteRoot + 'PMPO/ConfiguracionParametros/';

$(function () {
    $('#btnEjecutarEstructuraFS').on("click", function () {
        ejecutarNuevaEstructuraFS();
    });

    cargarHtmlSubcarpeta(); 
});

//Manejo de archivos
function verArchivo(subcarpeta) {
    $.ajax({
        type: 'POST',
        url: controladorParametro + 'ObtenerFolder',
        data: {
            subcarpeta: subcarpeta
        },
        dataType: 'json',
        success: function (model) {

            if (model.Resultado !== '-1') {
                $('#hfBaseDirectory').val("");
                $('#hfRelativeDirectory').val(model.PathSubcarpeta);

                browser();
                $('.bread-crumb').css("display", 'none');
                $('#resultado').hide();
                $('#folder').show();
            } else {
                alert(model.StrMensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function ejecutarNuevaEstructuraFS() {
    $.ajax({
        type: 'POST',
        url: controladorParametro + 'CopiarANuevaEstructuraFS',
        data: {
        },
        dataType: 'json',
        success: function (model) {
            if (model.Resultado !== '-1') {
                alert("Se copió los archivos a la nueva estructura.");

                window.location.href = controladorParametro + 'IndexFileServer';
            } else {
                alert(model.StrMensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function cargarHtmlSubcarpeta() {
    var listaSubcarpeta = $("#subcarpetasIntervenciones").val().split('|');
    $('#listado').html(dibujarTablaSubcarpeta(listaSubcarpeta));

    $('#tablaListado').dataTable({
        "scrollY": 530,
        "scrollX": true,
        "destroy": "true",
        "sDom": 't',
        "ordering": false,
        "iDisplayLength": -1,
        "stripeClasses": [],
    });
}

function dibujarTablaSubcarpeta(lista) {

    var cadena = '';
    cadena += `
    <div style='clear:both; height:5px'></div>
    <table id='tablaListado' border='1' class='pretty tabla-icono' cellspacing='0' style='width: 100%'>
        <thead>
            <tr>
                <th style='width: 50px'>Ver archivos</th>
                <th style='width: 150px'>Subcarpeta</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];

        cadena += `
            <tr>
                <td style='text-align: center;'>    
                    <a href="JavaScript:verArchivo('${item}');"><img src="${siteRoot}Content/Images/folder_open.png" title="Permite ver los archivos" /></a>
                </td>
                <td style='text-align: left;'>${item}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}