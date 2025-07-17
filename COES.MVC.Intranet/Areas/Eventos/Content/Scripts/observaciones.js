
iniLista = function () {

    $('#dtInicio').Zebra_DatePicker({
    });

    $('#dtFin').Zebra_DatePicker({
    });

    $("#btnConsultar").click(function () {
        Consultar();
    });
}

Consultar = function () {

    var cboEmpresaRecomendada = $("#cboEmpresaObservacion").val();
    var dtInicio = $("#dtInicio").val();
    var dtFin = $("#dtFin").val();

    if (dtInicio == "" || dtFin == "") {
        alert('Por favor seleccione un rango de fechas');
        return;
    }

    //var dtInicioArr = dtInicio.split('-');
    //var dtFinArr = dtFin.split('-');

    //dtInicio = dtInicioArr[2] + "/" + dtInicioArr[1] + "/" + dtInicioArr[0];
    //dtFin = dtFinArr[2] + "/" + dtFinArr[1] + "/" + dtFinArr[0];

    var obj = {
        EMPRCODI: cboEmpresaRecomendada,
        FechaInicio: dtInicio,
        FechaFin: dtFin
    }

    /**
     
                        <th>EMPRESA</th>
                        <th>EVENTO</th>
                        <th>OBSERVACION</th>
                        <th>LASTUSER</th>
                        <th>LASTDATE</th>
     */

    var controlador = siteRoot + 'Eventos/Observaciones/';

    $.ajax({
        type: 'POST',
        url: controlador + "ConsultarObservacion",
        data: obj,
        success: function (json) {
            
            if (json != "") {
                $("#tbodyLista").empty();
                var njson = json.length;
                var html = "";
                for (var i = 0; i < njson; i++) {
                    html += "<tr>";
                    html += "<td>" + json[i].EMPRNOMB + "</td>";
                    html += "<td>" + json[i].EVENTO + "</td>";
                    html += "<td>" + json[i].AFOOBSERVAC + "</td>";
                    html += "<td>" + json[i].LASTUSER + "</td>";
                    html += "<td>" + json[i].LASTDATEstr + "</td>";
                    html += "</tr>";
                }
                html = html.replace(/null/g, '-');
                $("#tbodyLista").append(html);
            } else {
                alert("No result");
                $("#tbodyLista").empty();
            }
        },
        error: function () {
            //mostrarError();
        }
    });

}


