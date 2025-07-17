
iniLista = function () {

    $('#dtInicio').Zebra_DatePicker({
    });

    $('#dtFin').Zebra_DatePicker({
    });

    $("#btnConsultar").click(function () {
        Consultar();
    });

    $('#txtFechaMedidaAdoptada').Zebra_DatePicker({});

    $("#btnSubirCartaRecomendacionCOES").click(function () {
        SubirCartaRecomendacionCOES()
    });

    $("#btnSubirCartadeRespuesta").click(function () {
        SubirCartadeRespuesta()
    });
}

Consultar = function () {

    var cboEmpresaRecomendada = $("#cboEmpresaRecomendada").val();
    var cboEstado = $("#cboEstado").val();
    var cboImportante = $("#cboImportante").val();
    var dtInicio = $("#dtInicio").val();
    var dtFin = $("#dtFin").val();

    //if (dtInicio == "" || dtFin == "") {
    //    alert('Por favor seleccione un rango de fechas');
    //    return;
    //}

    //var dtInicioArr = dtInicio.split('-');
    //var dtFinArr = dtFin.split('-');

    //dtInicio = dtInicioArr[2] + "/" + dtInicioArr[1] + "/" + dtInicioArr[0];
    //dtFin = dtFinArr[2] + "/" + dtFinArr[1] + "/" + dtFinArr[0];




    //string estado = (cbx_estado.Text == "PENDIENTE DE RESPUESTA") ? "R" : cbx_estado.Text.Substring(0, 1);

    if (cboEstado == "PENDIENTE DE RESPUESTA") {
        cboEstado = "R";
    } else if (cboEstado != "") {
        cboEstado = cboEstado.substring(0, 1);
    }

    //string importante = (this.cbImportante.SelectedIndex == 1) ? "S" : "N";

    if (cboImportante == "IMPORTANTE") {
        cboImportante = 'S';
    } else if (cboImportante == "NO IMPORTANTE") {
        cboImportante = 'N';
    }

    if (cboEmpresaRecomendada == "0") {
        cboEmpresaRecomendada = "";
    }


    var obj = {
        EMPRCODI: cboEmpresaRecomendada,
        ESTADO: cboEstado,
        IMPORTANTE: cboImportante,
        FechaInicio: dtInicio,
        FechaFin: dtFin
    }

    /**
     
 <th>CODIGO</th>
                        <th>EMPRESA</th>
                        <th>RECOMENDACION</th>
                        <th>ESTADO</th>
                        <th>RESPUESTA</th>
                        <th>OBSERVACION</th>
                        <th>ACCION FINAL</th>
                        <th>NRO REG. RESPUESTA</th>
                        <th>IMPORTANTE</th>

     */

    var controlador = siteRoot + 'Eventos/Recomendaciones/';

    $.ajax({
        type: 'POST',
        url: controlador + "ConsultarRecomendacion",
        data: obj,
        success: function (json) {

            if (json != "") {
                $("#tbodyLista").empty();
                var njson = json.length;
                var html = "";
                for (var i = 0; i < njson; i++) {
                    html += "<tr>";
                    html += "<td><a href='" + siteRoot + "Eventos/AnalisisFallas/Update/" + json[i].AFECODI + "' onclick='Editar(1)' id='btnEditar' target='_blank'><img src='" + siteRoot + "Content/Images/btn-edit.png' style='margin-top: 5px;' /></a> <a href='#' onclick='VerMedidasAdoptadas(" + json[i].AFRREC + "," + json[i].EMPRCODI + ")'><img src='" + siteRoot + "Content/Images/btn-properties.png' style='margin-top: 5px;'/></a></td>";
                    html += "<td>" + json[i].CODIGO + "</td>";
                    html += "<td>" + json[i].EMPRNOMB + "</td>";
                    html += "<td>" + json[i].RECOMENDACION + "</td>";
                    html += "<td>" + json[i].ESTADO + "</td>";
                    html += "<td>" + json[i].RESPUESTA + "</td>";
                    html += "<td>" + json[i].OBSERVACION + "</td>";
                    html += "<td>" + json[i].ACCIONFINAL + "</td>";
                    html += "<td>" + json[i].NROREGRESPUESTA + "</td>";
                    html += "<td>" + json[i].INDIMPORTANTE + "</td>";
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


VerMedidasAdoptadas = function (id, empresa) {

    var controlador = siteRoot + 'Eventos/Recomendaciones/';

    $.ajax({
        url: controlador + "ObtenerMedidasAdoptadas/" + id,
        type: 'POST',
        processData: false,
        contentType: false,
        success: function (json) {
            if (json != "") {

                $("#hfMedidaAdoptadaSelected").val(id);
                $("#hfMedidaAdoptadaEmpresaSelected").val(empresa);

                $("#txtMedidaAdoptadaAFECORR").val(json.AFECORR);
                $("#txtMedidaAdoptadaAFEANIO").val(json.AFEANIO);
                $("#txtMedidaAdoptadaEVENASUNTO").val(json.EVENASUNTO);
                $("#txtMedidaAdoptadaEMPRNOMB").val(json.EMPRNOMB);
                $("#txtMedidaAdoptadaAFRRECOMEND").val(json.AFRRECOMEND);

                if (json.INDIMPORTANTE == "S") {
                    document.getElementById("chkMedidaAdoptadaImportante").checked = true;
                }

                $("#txtMedidaAdoptadaNroRegistroRespuesta").val(json.NROREGRESPUESTA);
                $("#txtFechaMedidaAdoptada").val(json.AFRMEDADOPFECHAstr);
                $("#txtMedidaAdoptada").val(json.AFRMEDADOPMEDIDA);


                if (json.AFRPUBLICAFECHA != null) {
                    document.getElementById("chkCartaRecomendacion").checked = true;
                }

                if (json.AFRPUBLICAFECHA != null) {
                    $("#btnVerCartaRecomendacionCOES").show();
                } else {
                    $("#btnSubirCartaRecomendacionCOES").show();
                    $("#flCartaRecomendacionCOES").show();
                }


                if (json.LASTDATERPTA != null) {
                    $("#btnVerCartaRespuesta").show();
                    $("#btnEliminarVerCartaRespuesta").show();
                } else {
                    $("#btnSubirCartadeRespuesta").show();
                    $("#flCartaRespuesta").show();
                }

                var Cumplimiento = "";

                //switch (json.AFRMEDADOPNIVCUMP) {
                //    case "E":
                //        Cumplimiento = "EN PROCESO";
                //        break;
                //    case "P":
                //        Cumplimiento = "PARCIAL";
                //        break;
                //    case "C":
                //        Cumplimiento = "CUMPLIO";
                //        break;
                //    case "S":
                //        Cumplimiento = "SIN CUMPLIR";
                //        break;
                //    case "R":
                //        Cumplimiento = "PENDIENTE DE RESPUESTA";
                //        break;
                //}
                if (json.AFRMEDADOPNIVCUMP == null)
                    $('#cboNiveldeCumplimiento').val("R");
                else
                    $("#cboNiveldeCumplimiento").val(json.AFRMEDADOPNIVCUMP);


                $('#divMedidasAdoptadas').bPopup({
                    follow: [true, true],
                    position: ['auto', 'auto'],
                    positionStyle: 'fixed',
                });

            }
        }
    });
}

ActualizarRecomendacionMA = function () {

    var controlador = siteRoot + 'Eventos/AnalisisFallas/';

    var Importante = "N";
    var chk = document.getElementById("chkMedidaAdoptadaImportante").checked;
    if (chk) {
        Importante = "S";
    }

    var frm = new FormData();
    frm.append("AFRREC", $("#hfMedidaAdoptadaSelected").val());
    frm.append("NROREGRESPUESTA", $("#txtMedidaAdoptadaNroRegistroRespuesta").val());
    frm.append("INDIMPORTANTE", Importante);

    $.ajax({
        url: controlador + "ActualizarRecomendacionMA",
        data: frm,
        type: 'POST',
        processData: false,
        contentType: false,
        success: function (result) {
            if (result) {
                alert("Se actualizo correctamente");
            } else {
                alert("No se pudo actualizar");
            }
        }
    });

}

SubirCartaRecomendacionCOES = function () {

    var file = $("#flCartaRecomendacionCOES")[0].files[0];
    if (file != null) {

        var controlador = siteRoot + 'Eventos/AnalisisFallas/';
        var frm = new FormData();
        frm.append("AFRREC", $("#hfMedidaAdoptadaSelected").val());
        frm.append("ANIO", $("#txtMedidaAdoptadaAFEANIO").val());
        frm.append("file", file);

        $.ajax({
            url: controlador + "ActualizarCartaRecomendacionCOES",
            data: frm,
            type: 'POST',
            processData: false,
            contentType: false,
            success: function (result) {
                if (result) {

                    $("#btnSubirCartaRecomendacionCOES").show();
                    $("#btnVerCartaRecomendacionCOES").show();
                    $("#flCartaRecomendacionCOES").val("");


                    document.getElementById("chkCartaRecomendacion").checked = true;

                    alert('Se cargo correctamente');
                }
            }
        });


    } else {
        alert('Seleccione un archivo');
    }

}

SubirCartadeRespuesta = function () {

    var file = $("#flCartaRespuesta")[0].files[0];
    if (file != null) {

        var controlador = siteRoot + 'Eventos/AnalisisFallas/';
        var frm = new FormData();
        frm.append("AFRREC", $("#hfMedidaAdoptadaSelected").val());
        frm.append("ANIO", $("#txtMedidaAdoptadaAFEANIO").val());
        frm.append("AFRMEDADOPNIVCUMP", $("#cboNiveldeCumplimiento").val());
        frm.append("AFRMEDADOPMEDIDA", $("#txtMedidaAdoptada").val());
        frm.append("AFRMEDADOPFECHAU", $("#txtFechaMedidaAdoptada").val());
        frm.append("file", file);

        $.ajax({
            url: controlador + "ActualizarCartaRespuesta",
            data: frm,
            type: 'POST',
            processData: false,
            contentType: false,
            success: function (result) {
                if (result) {
                    $("#flCartaRespuesta").val("");

                    $("#btnVerCartaRespuesta").show();
                    $("#btnEliminarVerCartaRespuesta").show();


                    $("#btnSubirCartadeRespuesta").hide();
                    $("#flCartaRespuesta").hide();



                    alert('Se cargo correctamente');
                }
            }
        });


    } else {
        alert('Seleccione un archivo');
    }

}

EliminarCartaRespuesta = function () {


    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var frm = new FormData();
    frm.append("AFRREC", $("#hfMedidaAdoptadaSelected").val());
    frm.append("ANIO", $("#txtMedidaAdoptadaAFEANIO").val());

    $.ajax({
        url: controlador + "EliminarCartaRespuesta",
        data: frm,
        type: 'POST',
        processData: false,
        contentType: false,
        success: function (result) {
            if (result) {
                $("#flCartaRecomendacionCOES").val("");

                $("#btnVerCartaRespuesta").hide();
                $("#btnEliminarVerCartaRespuesta").hide();


                $("#btnSubirCartadeRespuesta").show();
                $("#flCartaRespuesta").show();

                alert('Se elimino correctamente');
            }
        }
    });




}

ActualizarRecomendacionMAG = function () {

    var controlador = siteRoot + 'Eventos/AnalisisFallas/';


    var frm = new FormData();
    frm.append("AFRREC", $("#hfMedidaAdoptadaSelected").val());
    frm.append("AFRMEDADOPNIVCUMP", $("#cboNiveldeCumplimiento").val());
    frm.append("AFRMEDADOPMEDIDA", $("#txtMedidaAdoptada").val());
    frm.append("AFRMEDADOPFECHAU", $("#txtFechaMedidaAdoptada").val());

    $.ajax({
        url: controlador + "ActualizarRecomendacionMAG",
        data: frm,
        type: 'POST',
        processData: false,
        contentType: false,
        success: function (result) {
            if (result) {
                alert("Se actualizo correctamente");
            } else {
                alert("No se pudo actualizar");
            }
        }
    });

}


VerCartaRecomendacionCOES = function () {
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var filename = "EV-REM-" + $("#txtMedidaAdoptadaAFEANIO").val() + "-" + $("#hfMedidaAdoptadaSelected").val() + ".pdf";
    window.open(controlador + 'verArchivo?filename=' + filename + '&anio=' + $("#txtMedidaAdoptadaAFEANIO").val() + '&id=' + $("#hfMedidaAdoptadaSelected").val(), "_blank", 'fullscreen=yes');
}

VerCartaRespuesta = function () {
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var filename = "EV-RER-" + $("#txtMedidaAdoptadaAFEANIO").val() + "-" + $("#hfMedidaAdoptadaSelected").val() + ".pdf";
    window.open(controlador + 'verArchivo?filename=' + filename + '&anio=' + $("#txtMedidaAdoptadaAFEANIO").val() + '&id=' + $("#hfMedidaAdoptadaSelected").val(), "_blank", 'fullscreen=yes');
}

function cerrar() { $('#divMedidasAdoptadas').bPopup().close() }