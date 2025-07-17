//Declarando las variables 
let controlador = siteRoot + 'Transversal/';
let hot;
let hotOptions;
let evtHot;

$(function () {

    consultar();

    $("#btnRegresar").click(function () {
        retornar();
    });

});

function buscarEquipo() {
    consultar();
}

let consultar = function () {
    const codigoId = $("#hCodigoId").val();
    setTimeout(function () {
        $.ajax({
            type: 'POST',
            url: controlador + 'ListaHistorialCambio?codigoId=' + codigoId,
            success: function (evt) {
                $('#lista').html(evt);
                $('#tablaListadoActualizaciones').dataTable({
                    "iDisplayLength": 25,
                    language: {
                        info: 'Mostrando página _PAGE_ de _PAGES_',
                        infoEmpty: 'No hay registros disponibles',
                        infoFiltered: '(filtrado de _MAX_ registros totales)',
                        lengthMenu: 'Mostrar _MENU_ registros por página',
                        zeroRecords: 'No se encontró nada'
                    },
                    searching: false,
                    info: false
                });

                $('#tablaListadoPropiedadesActualizadas').dataTable({
                    "iDisplayLength": 25,
                    language: {
                        info: 'Mostrando página _PAGE_ de _PAGES_',
                        infoEmpty: 'No hay registros disponibles',
                        infoFiltered: '(filtrado de _MAX_ registros totales)',
                        lengthMenu: 'Mostrar _MENU_ registros por página',
                        zeroRecords: 'No se encontró nada'
                    },
                    searching: false,
                    info: false
                });

                $('.dataTables_filter input').attr('maxLength', 50);
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }, 100);
};

function mostrarListadoPropiedadActualizada(proyectoid) {
    const codigoid = $("#hCodigoId").val();

    $.ajax({
        type: 'POST',
        url: controlador + "ListaPropiedadesActualizadas",
        data: {
            codigoId: codigoid,
            proyectoId: proyectoid
        },
        success: function (evt) {

            let htmlLPA = dibujarTablaListadoPropiedadActualizada(evt.ListaPropiedadActualizada);
            $("#listadoPropiedadesActualizadas").html(htmlLPA);

            $('#tablaListadoPropiedadesActualizadas').dataTable({
                "iDisplayLength": 25,
                language: {
                    info: 'Mostrando página _PAGE_ de _PAGES_',
                    infoEmpty: 'No hay registros disponibles',
                    infoFiltered: '(filtrado de _MAX_ registros totales)',
                    lengthMenu: 'Mostrar _MENU_ registros por página',
                    zeroRecords: 'No se encontró nada'
                },
                searching: false,
                info: false
            });

        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function dibujarTablaListadoPropiedadActualizada(listaPropiedadActualizada) {

    let cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono display compact" id="tablaListadoPropiedadesActualizadas">
       <thead>
           <tr>
               <th style="width:30%">Propiedad</th>
               <th style="width:20%">Valor</th>
               <th style="width:20%">Fecha Vigencia</th>
               <th style="width:30%">Comentario</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (let key in listaPropiedadActualizada) {
        let item = listaPropiedadActualizada[key];

        let comentario = "";
        if (item.Propequicomentario != null) {
            comentario = item.Propequicomentario;
        }

        cadena += `
            <tr>
                <td style="text-align: left;">${item.Propnomb}</td>
                <td style="text-align: left;">${item.Valor}</td>
                <td style="text-align: left;">${item.FechaVigencia}</td>
                <td style="text-align: left;">` + comentario + `</td>
               
           </tr >           
        `;
    }

    cadena += `                   
        </tbody >
    </table >
    `;

    return cadena;
}

function retornar() {
    let ret = siteRoot + $("#hParthReturn").val();

    window.location.href = ret;
};

let mostrarError = function () {
    $('#mensaje').removeClass("action-exito");
    $('#mensaje').addClass("action-error");
    $('#mensaje').text("Ha ocurrido un error");
    $('#mensaje').css("display", "block");
};
