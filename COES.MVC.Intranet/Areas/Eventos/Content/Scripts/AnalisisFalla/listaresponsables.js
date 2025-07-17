

iniLista = function () {

    $("#btnConsultarResponsables").click(function () {
        ConsultarResponsables();
    });

    $("#btnAgregarResponsables").click(function () {
        cargarResponsable();
        
    });

    $("#btnGrabarNuevoResponsable").click(function () {
        GrabarNuevoResponsable();
    });

    $("#btnGrabarEditarResponsable").click(function () {
        GrabarEditarResponsable();
    });


    $("#fileFirmaEditar").hide();

    ConsultarResponsables();
}

PopupAgregarResponsable = function () {

    $('#cboResponsables').val(0);
    $('#cboEstado').val('');
    $('#fileFirma').val('');

    $('#divAgregarResponsable').bPopup({
        follow: [true, true],
        position: ['auto', 'auto'],
        positionStyle: 'fixed',
    });
}

PopupEditarResponsable = function (json) {
    //let ubicacionArchivo = json.NombreArchivoFirma.replace("\\","\");
    console.log(json);
    $("#nombreArchivo").show();
    $("#fileFirmaEditar").hide();
    $("#btnLimpiarArchivo").show();
    $('#fileFirmaEditar').val('');

    $("#nombreResponsable").prop("disabled",true)
    $("#nombreResponsable").val(json.NombreCompleto);
    $("#hfCodigoResponsableEditar").val(json.CodigoResponsable);
    $("#cboEstadoEditar").val(json.Estado);
    //$("#fileFirmaEditar").val(json.NombreArchivoFirma);

    $("#nombreArchivo").prop("disabled", true);
    //$("#nombreArchivo").css("visibility", "hidden");
    $("#nombreArchivo").val(json.NombreArchivoFirma);
    /*$("#fileFirmaEditar").css("visibility", "hidden");*/



    $('#divEditarResponsable').bPopup({
        follow: [true, true],
        position: ['auto', 'auto'],
        positionStyle: 'fixed',
    });
}

GrabarNuevoResponsable = function () {

    let file = $("#fileFirma")[0].files[0];

    if ($("#cboResponsables").val()==0) {
        alert('Seleccione un responsable');
        return;
    }

    if ($("#cboEstado").val() == '') {
        alert('Seleccione un estado');
        return;
    }

    if (file != null) {
        let controlador = siteRoot + 'Eventos/AnalisisFallas/';
        let codigoResponsable = 0;
        let codigoDirector = parseInt($("#cboResponsables").val(), 10);
        let estado = $("#cboEstado").val();
        let nombreCompleto = $("#cboResponsables  option:selected").text();

        let frm = new FormData();
        frm.append("CodigoResponsable", codigoResponsable);
        frm.append("CodigoDirector", codigoDirector);
        frm.append("Estado", estado);
        frm.append("NombreCompleto", nombreCompleto);
        frm.append("NombreArchivoFirma", "");
        frm.append("file", file);

        $.ajax({
            url: controlador + "GrabarNuevoResponsable",
            data: frm,
            type: 'POST',
            processData: false,
            contentType: false,
            success: function (result) {
                if (result) {
                    alert('Registrado correctamente');
                    ConsultarResponsables();
                    $('#btnCancelar').click();
                }
            },
            error: function (errorThrown) {
                alert("Error: " + errorThrown);
            }  
        });
    } else {
        alert('Seleccione un archivo de firma');
    }
}

ConsultarResponsables = function () {

    let chkEstado = $("input[name='rbValor']:checked").val();
    let txtBuscar = $('#txtBuscarResponsable').val();
    let controlador = siteRoot + 'Eventos/AnalisisFallas/';

    var objData = {
        Estado: chkEstado,
        NombreApellidos: txtBuscar
    }

    $.ajax({
        type: 'POST',
        url: controlador + "ConsultarResponsables",
        data: objData,
        success: function (json) {
            if (json != "") {
                $("#tbodyListaResponsables").empty();
                var njson = json.length;
                var html = "";
                for (var i = 0; i < njson; i++) {
                    html += "<tr>";
                    html += "<td Style='display: none'>" + json[i].CodigoResponsable + "</td>";
                    html += "<td Style='display: none'>" + json[i].CodigoDirector + "</td>";
                    html += "<td>" + json[i].NombreCompleto + "</td>";
                    html += "<td>" + json[i].NombreArchivoFirma + "</td>";
                    html += "<td>" + json[i].Estado + "</td>";
                    html += "<td><a href= 'JavaScript:EditarResponsable(" + json[i].CodigoResponsable + ")' id='btnEditar'><img src='" + siteRoot + "Content/Images/btn-edit.png' style='margin-top: 5px;' /></a>";
                    html += "<a href= 'JavaScript:EliminarResponsable(" + json[i].CodigoResponsable + ")' id='btnEliminar'><img src='" + siteRoot + "Content/Images/btn-cancel.png' style='margin-top: 5px;' /></a></td>"
                    html += "</tr>";
                }
                html = html.replace(/null/g, '-');
                $("#tbodyListaResponsables").append(html);

            } else {
                //alert("No existe resultados para la consulta");
                $("#tbodyListaResponsables").empty();
            }
        },
        error: function () {
            //mostrarError();
        }
    });
}

EliminarResponsable = function (codigoResponsable) {

    let controlador = siteRoot + 'Eventos/AnalisisFallas/';

    var objData = {
        CodigoResponsable: codigoResponsable
    }

    $.ajax({
        type: 'POST',
        url: controlador + "EliminarResponsable",
        data: objData,
        success: function (json) {
            if (json != "") {
                alert("Se eliminó al Responsable");
                ConsultarResponsables();
                
            } else {
                //alert("No existe resultados para la consulta");
            }
        },
        error: function () {
            //mostrarError();
        }
    });
}

EditarResponsable = function (codigoResponsable) {

    let controlador = siteRoot + 'Eventos/AnalisisFallas/';

    var objData = {
        CodigoResponsable: codigoResponsable
    }

    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerResponsable",
        data: objData,
        success: function (json) {
            if (json != "") {
                PopupEditarResponsable(json);
                
            } else {
                alert("No existen resultados para la consulta del responsable");
            }
        },
        error: function () {
            //mostrarError();
        }
    });
}

//ObtenerResponsable = function (codigoResponsable) {

//    let controlador = siteRoot + 'Eventos/AnalisisFallas/';

//    var objData = {
//        CodigoResponsable: codigoResponsable
//    }

//    $.ajax({
//        type: 'POST',
//        url: controlador + "ObtenerResponsable",
//        data: objData,
//        success: function (json) {
//            if (json != "") {
//                PopupEditarResponsable(json);
//            } else {
//                alert("No existe resultados para la consulta del responsable");
//            }
//        },
//        error: function () {
//            //mostrarError();
//        }
//    });
//}


LimpiarArchivo = function () {
    $("#nombreArchivo").hide();
    $("#nombreArchivo").val('');
    $("#fileFirmaEditar").show();
    $("#btnLimpiarArchivo").hide();
}



GrabarEditarResponsable = function () {
    $("#nombreArchivo").val()

    let file = $("#fileFirmaEditar")[0].files[0];


    if ($("#nombreArchivo").val() === '') {
        if (file === null) {
            alert('Seleccione un archivo de firma');
            return;
        }
    }

        let controlador = siteRoot + 'Eventos/AnalisisFallas/';
        let codigoResponsable = parseInt($("#hfCodigoResponsableEditar").val(), 10);
        //let codigoDirector = parseInt($("#cboResponsablesEditar").val(), 10);
        let estado = $("#cboEstadoEditar").val();
        let nombreCompleto = $("#cboResponsablesEditar  option:selected").text();
    let nombreArchivo = $("#nombreArchivo").val();
        let frm = new FormData();
        frm.append("CodigoResponsable", codigoResponsable);
        //frm.append("CodigoDirector", codigoDirector);
        frm.append("Estado", estado);
        frm.append("NombreCompleto", nombreCompleto);
    frm.append("NombreArchivoFirma", nombreArchivo);

        if (file != null) {
            frm.append("file", file);
        }

        $.ajax({
            url: controlador + "GrabarEditarResponsable",
            data: frm,
            type: 'POST',
            processData: false,
            contentType: false,
            success: function (result) {
                if (result) {
                    alert('Guardado correctamente');
                    ConsultarResponsables();                  
                    //$('#btnCancelar').click();
                    var bPopup = $('#divEditarResponsable').bPopup();
                    bPopup.close();
                }
            },
            error: function (errorThrown) {
                alert("Ocurrió un error" + errorThrown);
            }
        });
}

cargarResponsable = function () {
    debugger;
    let controlador = siteRoot + 'Eventos/AnalisisFallas/';
    $("[id*='cboResponsables']").empty();
    $.ajax({
        type: 'POST',
        url: controlador + "listaResponsables",
        dataType: 'json',
        cache: false,
        success: function (evt) {
            var _len = evt.length;
            for (i = 0; i < _len; i++) {
            post = evt[i];
                var select = document.getElementById("cboResponsables");
                select.options[select.options.length] = new Option(post.DirectorioNombre, post.DirectorioCodigo);
            }
            PopupAgregarResponsable();

        },
        error: function (errorThrown) {
            alert("Ocurrió un error" + errorThrown);
        }
    });

}