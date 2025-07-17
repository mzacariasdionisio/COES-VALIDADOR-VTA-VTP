$(function () {


    $('#cbTipoEquipo').val($('#hfFamcodi').val());
    $('#cbTipoEquipo').change(function () {
        cargarEquipos();
    });
    $('#txtHoraIni').inputmask({
        mask: "h:s:s",
        placeholder: "hh:mm:ss",
        alias: "datetime",
        hourformat: 24
    });
    $('#txtHoraFin').inputmask({
        mask: "h:s:s",
        placeholder: "hh:mm:ss",
        alias: "datetime",
        hourformat: 24
    });

    var diasZebra = getDirectionZebra($('#hfecha').val());

    if (diasZebra == 0) {
        //dia de hoy
        $('#txtFechaFin').Zebra_DatePicker({
            direction: [true, 1]
        });
    } else {
        if (diasZebra + 1 == 0) {
            //dia ayer
            $('#txtFechaFin').Zebra_DatePicker({
                direction: [false, 1]
            });
        } else {
            //caso normal
            $('#txtFechaFin').Zebra_DatePicker({
                direction: [getDirectionZebra($('#hfecha').val()) + 1, 1]
            });
        }
    }

    $("#btnGrabar").click(function () {
        var errors = 0;
        var fecha = $('#hfecha').val();
        var fechaIni = convertStringToDate(fecha, $('#txtFechaIni').val());
        var fechaIFin = convertStringToDate(fecha, $('#txtFechaFin').val());

        if ($('#cbTipoEquipo').val() == 0) {
            errors += 1;
            alert("Debe ingresar Tipo de Equipo  !");
            $('#cbTipoEquipo').focus();
            return false;
        }
        if ($('#cbEquipos').val() == 0) {
            errors += 1;
            alert("Debe ingresar Equipo  !");
            $('#cbEquipos').focus();
            return false;
        }
        if ($('#txtFechaIni').val() != $('#txtFechaFin').val()) {
            if ($('#txtHoraFin').val() != "00:00:00") {
                errors += 1;
                alert("La hora final debe ser '00:00:00'  !");
                return false;
            }
        }

        var fIni = $('#txtFechaIni').val() + " " + $('#txtHoraIni').val();
        var fFin = $('#txtFechaFin').val() + " " + $('#txtHoraFin').val();
        if (moment(fIni).isAfter(fFin)) {
            errors += 1;
            alert("La Fecha Inicio debe ser menor que la Fecha FIn  !");
            return false;
        }
        if (errors == 0) {
            registarRestriccion();
        }
    });

    $("#btnCancelar").click(function () {
        $('#newRestricciones').bPopup().close();
    });
    cargarEquipos();
    crearPupload();
    $('#nombreArchivo').html($('#hfNameArchEnvio').val());
    //$("#btnSelectFile").click(function () {
    //    crearPupload();
    //});
});

function cargarEquipos() {
    var idTipoEquipo = "0";
    if ($('#cbTipoEquipo').val() != null) {
        var tipoEquipo = ($('#cbTipoEquipo').val()).split('/');
        idTipoEquipo = tipoEquipo[0];
    }

    //$('#ubicacion').html("");
    //    mostrarOcultarCentral(0);
    var idEmpresa = $('#cbEmpresa').val();
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarEquipos',

        data: {
            idTipoEquipo: idTipoEquipo,
            idEmpresa: idEmpresa,
            idequipo: $('#hfEquiCodi').val(),
            actFiltro: $('#hfActFiltro').val()
        },

        success: function (aData) {
            $('#equipos').html(aData);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function getDirectionZebra(fechaForm) {
    var fechaActual = moment();
    fechaActual = moment(fechaActual, "DD-MM-YYYY");
    fechaForm = moment(fechaForm, "DD-MM-YYYY");
    var diff = fechaForm.diff(fechaActual, 'days');
    return diff;
}

function registarRestriccion() {
    var enviocodi = $('#hcodenvio').val();
    var iccodi = $('#hfIccodi').val();
    var fecha = $('#hfecha').val();
    var famcodi = ($('#cbTipoEquipo').val()).split('/');
    var famabrev = famcodi[1];
    var idSubcausacodi = $("#hIdSubcausacodi").val();

    var equicodi = $('#cbEquipos').val();
    //var horaIni = $('#txtFechaIni').val();
    //var horaFin = $('#txtFechaFin').val();
    var fechaIni = $('#txtFechaIni').val();//getFechaForm(fecha, $('#txtFechaIni').val());
    var fechaFin = $('#txtFechaFin').val();//getFechaForm(fecha, $('#txtFechaFin').val());;
    var horaIni = $('#txtHoraIni').val();//getHoraForm(fecha, $('#txtFechaIni').val());
    var horaFin = $('#txtHoraFin').val();//getHoraForm(fecha, $('#txtFechaFin').val());
    var descrip = $('#txtDescripcion').val();
    var Empnomb = $("#cbEmpresa option:selected").html();
    var strEquipo = ($("#cbEquipos option:selected").html()).split('-');
    var equinomb = strEquipo[0];
    var lasuser = $('#hfLastUser').val();
    var fechaactual = new Date();
    var archFisico = $('#hfNameArchFisico').val();
    var archEnvio = $('#hfNameArchEnvio').val();
    var cambioArch = $('#hfCambioArchivo').val(); // 1: si fue cambiado 0. si nofue cambiado
    var idEmpresa = parseInt($("#cbEmpresa").val());

    var flagCruces = verificaCruces(fecha, horaIni, horaFin, equicodi, iccodi);

    if (!flagCruces) { // si no existe cruces con otras restricciones para el mismo equipo
        if (iccodi == 0) // si es nuevo 
        {
            codiAuto = ((evtHot.ListaRestricciones.length) + 1) * -1;
            var entity =
            {
                Iccodi: codiAuto,
                Emprcodi: idEmpresa,
                Equicodi: equicodi,
                Emprnomb: Empnomb,
                Famabrev: famabrev,
                Equinomb: equinomb,
                Ichorini: moment(convertStringToDate(fechaIni, horaIni)).format('DD/MM/YYYY HH:mm:ss'),
                Ichorfin: moment(convertStringToDate(fechaFin, horaFin)).format('DD/MM/YYYY HH:mm:ss'),
                Lastuser: lasuser,
                Lastdate: moment(fechaactual).format('DD/MM/YYYY HH:mm:ss'),
                Icdescrip1: descrip,
                Subcausacodi: idSubcausacodi,
                Icnombarchfisico: archFisico,
                Icnombarchenvio: archEnvio
            };
            evtHot.ListaRestricciones.push(entity);
        }
        else // si es modificacion
        {
            for (var i = 0; i < evtHot.ListaRestricciones.length; i++) {
                if (evtHot.ListaRestricciones[i].Iccodi == iccodi) {// encontrado               
                    evtHot.ListaRestricciones[i].Ichorini = moment(convertStringToDate(fecha, horaIni)).format('DD/MM/YYYY HH:mm:ss'),
                        evtHot.ListaRestricciones[i].Ichorfin = moment(convertStringToDate(fecha, horaFin)).format('DD/MM/YYYY HH:mm:ss'),
                        evtHot.ListaRestricciones[i].Icdescrip1 = descrip,
                        evtHot.ListaRestricciones[i].Lastdate = moment(fechaactual).format('DD/MM/YYYY HH:mm:ss'),
                        evtHot.ListaRestricciones[i].Icnombarchfisico = archFisico,
                        evtHot.ListaRestricciones[i].Icnombarchenvio = archEnvio,
                        evtHot.ListaRestricciones[i].CambioArchivo = cambioArch, // si hubo cambio en archivo
                        evtHot.ListaRestricciones[i].opCrud = 2 /// -1: eliminar, 0:lectura, 1:crear, 2:actualizar 
                }
            }

        }
        actualizaListadoRestricciones(evtHot);
        $('#newRestricciones').bPopup().close();
    }
    else {
        alert("Existe cruces de horas con restricciones del mismo equipo!")
    }
}

function verificaCruces(fecha, horaIni, horaFin, equicodi, iccodi) {
    var horaIni = convertStringToDate(fecha, horaIni);
    var horaFin = convertStringToDate(fecha, horaFin);

    if (evtHot.ListaRestricciones.length > 0) {
        for (var i = 0; i < evtHot.ListaRestricciones.length; i++) {
            if (evtHot.ListaRestricciones[i].Equicodi == equicodi && evtHot.ListaRestricciones[i].Iccodi != iccodi) {

                var fechaFrom = evtHot.ListaRestricciones[i].Ichorini.split(' ');
                var fechaFrom2 = fechaFrom[0].split('/');
                var horaFrom = fechaFrom[1].split(':');

                var dateFrom = new Date(fechaFrom2[2], fechaFrom2[1] - 1, fechaFrom2[0], horaFrom[0], horaFrom[1], horaFrom[2]);

                var fechaTo = evtHot.ListaRestricciones[i].Ichorfin.split(' ');
                var fechaTo2 = fechaTo[0].split('/');
                var horaTo = fechaTo[1].split(':');
                var dateTo = new Date(fechaTo2[2], fechaTo2[1] - 1, fechaTo2[0], horaTo[0], horaTo[1], horaTo[2]);

                if (moment(horaIni).isBetween(dateFrom, dateTo))
                    return true;
                if (moment(horaFin).isBetween(dateFrom, dateTo))
                    return true;
                if (moment(dateFrom).isBetween(moment(horaIni), moment(horaFin)))
                    return true;
                if (moment(dateTo).isBetween(moment(horaIni), moment(horaFin)))
                    return true;
            }
        }
    }
    return false;
}

function crearPupload() {
    var nombre_fisico;
    var nombre_envio;
    var equicodi = $('#hfEquiCodi').val();
    var horaIni = $('#txtHoraIni').val();
    var horaIniR = horaIni.split(':')[0] + horaIni.split(':')[1] + horaIni.split(':')[2];
    var fenvio = $('#hfecha').val();
    var fenvioR = fenvio.split('/')[0] + fenvio.split('/')[1] + fenvio.split('/')[2];
    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectFile',
        container: document.getElementById('container'),
        multipart_params: {
            "idEquipo": equicodi,
            "horaIni": horaIniR,
            "fecha": fenvioR
        },
        url: siteRoot + 'IEOD/RestriccionesOperativas/upload',
        flash_swf_url: 'Scripts/Moxie.swf',
        silverlight_xap_url: 'Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '5mb',
            mime_types: [
                { title: "Archivos Excel .xlsx", extensions: "xlsx,xls" }
            ]
        },
        init: {
            FilesAdded: function (up, files) {
                if (uploader.files.length == 2) {
                    uploader.removeFile(uploader.files[0]);
                }
                nombre_envio = files[0].name;
                //if (files[0].name.length > 10) {
                //    nombre_envio = files[0].name.substring(0, 10) + " ...";
                //}
                var ext = files[0].name.split(".").pop();


                equicodi = $('#cbEquipos').val();
                horaIni = $('#txtHoraIni').val();
                horaIniR = horaIni.split(':')[0] + horaIni.split(':')[1] + horaIni.split(':')[2]
                fenvio = $('#hfecha').val();
                fenvioR = fenvio.split('/')[0] + fenvio.split('/')[1] + fenvio.split('/')[2];
                nombre_fisico = "ARCH_" + equicodi + "_" + fenvioR + horaIniR + "." + ext;
                uploader.bind('BeforeUpload', function (up, file) {
                    up.settings.multipart_params = {

                        "idEquipo": equicodi,
                        "horaIni": horaIniR,
                        "fecha": fenvioR

                    };
                });

                $('#hfNameArchFisico').val(nombre_fisico);
                $('#hfNameArchEnvio').val(nombre_envio);
                $('#hfCambioArchivo').val(1);
                uploader.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                showMensaje();
                mostrarAlerta("Por favor espere ...(<strong>" + file.percent + "%</strong>)");
                hideMensajeEvento();
            },
            UploadComplete: function (up, file) {
                hideMensaje();
                $('#nombreArchivo').html(nombre_envio);

            },
            Error: function (up, err) {
                mostrarError(err.code + "-" + err.message);
            }
        }

    });
    uploader.init();
}