var rutaImagenes = siteRoot + "Content/Images/";
var controller = siteRoot + "CPPA/DescargaArchivos/";
var dtListado
var popNuevaCentral

$(document).ready(function () {

    $('#cboEmpresa').multipleSelect({
        single: true,
        filter: true
    });

    $('#cbAnio').change(function () {
        cargarAjustes($(this).val());
    });

    $('#cbAjuste').change(function () {
        cargarRevisiones($('#cbAnio').val(), $(this).val());
    })

    $('#btnRetornar').on('click', function () {
        $("#popDetalle").bPopup().close();
    });

    $('#btnConsultar').on('click', function () {
        listaArchivos();
    });

    cargaParametros();
});

function cargaParametros() {
    const anio = $('#Anio').val() || null;
    const ajuste = $('#Ajuste').val();
    const revision = $('#Revision').val();
    const emprcodi = $('#Emprcodi').val();

    if (anio !== null) {
        document.getElementById('cbAnio').value = anio;
        cargarAjustes(anio);
        if (ajuste !== null) {
            cargarRevisiones(anio, ajuste);
            document.getElementById('cbAjuste').value = ajuste;
            document.getElementById('cbRevision').value = revision;
            $('#cboEmpresa').multipleSelect('setSelects', [emprcodi]);
            listaArchivos();
        }
    }
}

function cargarAjustes(year) {

    if (year) {
        $('#cbAjuste').prop('disabled', false).empty().append('<option value="">Seleccione</option>');
        $('#cbRevision').prop('disabled', true).empty().append('<option value="">Seleccione</option>');

        listRevision
            .filter(x => x.Cpaapanio === parseInt(year))
            .map(x => x.Cpaapajuste)
            .filter((value, index, self) => self.indexOf(value) === index) // Eliminar duplicados
            .forEach((cpaapajuste) => { $('#cbAjuste').append('<option value="' + cpaapajuste + '">' + cpaapajuste + '</option>'); });
    }
    else {
        $('#cbAjuste').prop('disabled', true).empty().append('<option value="">Seleccione</option>');
        $('#cbRevision').prop('disabled', true).empty().append('<option value="">Seleccione</option>');
    }
}

function cargarRevisiones(year, fit) {
    if (year && fit) {
        $('#cbRevision').prop('disabled', false).empty().append('<option value="">Seleccione</option>');

        listRevision
            .filter(x => x.Cpaapanio === parseInt(year) && x.Cpaapajuste === fit)
            .forEach((revision) => {
                let estado = revision.Cparestado == 'A' ? '' : ' [' + revision.Cparestado + ']';
                $('#cbRevision').append('<option value="' + revision.Cparcodi + '">' + revision.Cparrevision + estado + '</option>');
            });
    }
    else {
        $('#cbRevision').prop('disabled', true).empty().append('<option value="">Seleccione</option>');
    }
}

function listaArchivos() {

    const cparcodi = $('#cbRevision').val();
    const emprcodi = $('#cboEmpresa').val();

    if (!cparcodi || cparcodi.trim() === '' && !emprcodi || emprcodi.trim() === '') {
        SetMessage('#message', 'El Año presupuestal / Ajuste / Revisión / Empresa no han sido seleccionados', 'warning', true);
        if ($.fn.DataTable.isDataTable('#dtListado')) {
            $('#dtListado').DataTable().clear().draw();
        }
        return;
    }


    $.ajax({
        type: 'POST',
        url: controller + 'ListaDocumentosGrilla',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            cparcodi: cparcodi,
            emprcodi: emprcodi
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {

            if (result.ListaDocumentosGrilla && result.ListaDocumentosGrilla.length === 0) {
                if ($.fn.DataTable.isDataTable('#dtListado')) {
                    $('#dtListado').DataTable().clear().draw();
                }
                SetMessage('#message', 'No existen envíos de la empresa para los filtros seleccionados', 'warning', true);
                return;
            }

            dtListado = $('#dtListado').DataTable({
                data: result.ListaDocumentosGrilla,
                columns: [
                    {
                        title: 'Acciones',
                        data: null
                    },
                    { title: 'Codigo', data: 'Cpadoccodi', visible: false },
                    {
                        title: 'Fecha envío',
                        data: 'Cpadocfeccreacion',
                        visible: true,
                        render: function (data) {
                            if (data) {
                                return formatDateTime(data);
                            } else {
                                return data
                            }
                        }
                    },
                    { title: 'Usuario', data: 'Cpadocusucreacion', visible: true },
                    { title: 'Año ppto.', data: 'Cpaapanio', visible: true },
                    { title: 'Ajuste', data: 'Cpaapajuste', visible: true },
                    { title: 'idRevision', data: 'Cparcodi', visible: false },
                    { title: 'Revisión', data: 'Cparrevision', visible: true },
                    { title: 'Código envío', data: 'Cpadoccodenvio', visible: true },
                    { title: 'Emprcodi', data: 'Emprcodi', visible: false },
                    { title: 'Empresa', data: 'Emprnomb', visible: true }
                ],
                columnDefs: [
                    {
                      targets: [0], width: 70,
                      defaultContent: '<img class="clsVer" src="' + rutaImagenes + 'btn-open.png" style="cursor: pointer;" title="Ver"/> ' + '<img class="clsDescarga" src="' + rutaImagenes + 'btn-download.png" style="cursor: pointer;" title="Descargar"/>'
                    }
                ],
                createdRow: function (row, data, index) {

                },
                searching: true,
                bLengthChange: false,
                bSort: false,
                destroy: true,
                paging: false,
                info: false,
                scrollY: '500px',
            });
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

$(document).on('click', '#dtListado tr td .clsVer', function (e) {
    //$('#dtDetalle').html("");
    var row = $(this).closest('tr');
    var r = dtListado.row(row).data();

    listaDocumentosDetalle(r.Cpadoccodi, r.Cpadoccodenvio);
});

$(document).on('click', '#dtListado tr td .clsDescarga', function (e) {
    var row = $(this).closest('tr');
    var r = dtListado.row(row).data();

    descargaDocumentosZipeados(r.Cpadoccodi, r.Cpaapanio, r.Cpaapajuste, r.Cparrevision, r.Cpadoccodenvio, r.Emprnomb);
});

function descargaDocumentosZipeados(documento, anio, ajuste, revision, envio, empresa) {

    $.ajax({
        url: controller + 'DescargaDocumentosZipeados',
        type: 'GET',
        data: { documento: documento, anio: anio, ajuste: ajuste, revision: revision, envio: envio, empresa: empresa },
        success: function (response) {
            if (response.success) {
                var byteCharacters = atob(response.fileBytes);
                var byteArray = new Uint8Array(byteCharacters.length);

                for (var i = 0; i < byteCharacters.length; i++) {
                    byteArray[i] = byteCharacters.charCodeAt(i);
                }

                var blob = new Blob([byteArray], { type: 'application/zip' });
                var url = window.URL.createObjectURL(blob);
                var a = document.createElement('a');
                a.href = url;
                a.download = response.fileName;
                a.click();

                window.URL.revokeObjectURL(url);

                SetMessage('#message', 'El archivo se descargó correctamente.', 'success', true);
            } else {
                alert(response.message || 'Error al descargar el archivo.');
            }
        },
        error: function () {
            alert('Error al descargar los archivos.');
        }
    });
}

function listaDocumentosDetalle(cpadoccodi, cpadoccodenvio) {
    const anio = $('#cbAnio').val();
    const ajuste = $('#cbAjuste').val();
    const revision = $('#cbRevision').val();
    const revisionName = $('#cbRevision option:selected').text();
    const emprcodi = $('#cboEmpresa').val();
    const emprnomb = $('#cboEmpresa option:selected').text();

    var form = document.createElement("form");
    form.method = "POST";
    form.action = siteRoot + "CPPA/DescargaArchivos/ListaDetalleGrilla";

    var hiddenDocumento = document.createElement("input");
    hiddenDocumento.type = "hidden";
    hiddenDocumento.name = "documento";
    hiddenDocumento.value = cpadoccodi;
    form.appendChild(hiddenDocumento);

    var hiddenAnio = document.createElement("input");
    hiddenAnio.type = "hidden";
    hiddenAnio.name = "anio";
    hiddenAnio.value = anio;
    form.appendChild(hiddenAnio);

    var hiddenAjuste = document.createElement("input");
    hiddenAjuste.type = "hidden";
    hiddenAjuste.name = "ajuste";
    hiddenAjuste.value = ajuste;
    form.appendChild(hiddenAjuste);

    var hiddenRevision = document.createElement("input");
    hiddenRevision.type = "hidden";
    hiddenRevision.name = "revision";
    hiddenRevision.value = revision;
    form.appendChild(hiddenRevision);

    var hiddenRevisionName = document.createElement("input");
    hiddenRevisionName.type = "hidden";
    hiddenRevisionName.name = "revisionName";
    hiddenRevisionName.value = revisionName;
    form.appendChild(hiddenRevisionName);

    var hiddenEnvio = document.createElement("input");
    hiddenEnvio.type = "hidden";
    hiddenEnvio.name = "envio";
    hiddenEnvio.value = cpadoccodenvio;
    form.appendChild(hiddenEnvio);

    var hiddenEmprcodi = document.createElement("input");
    hiddenEmprcodi.type = "hidden";
    hiddenEmprcodi.name = "emprcodi";
    hiddenEmprcodi.value = emprcodi;
    form.appendChild(hiddenEmprcodi);

    var hiddenEmprnomb = document.createElement("input");
    hiddenEmprnomb.type = "hidden";
    hiddenEmprnomb.name = "emprnomb";
    hiddenEmprnomb.value = emprnomb;
    form.appendChild(hiddenEmprnomb);

    document.body.appendChild(form);
    form.submit();
}

function formatDate(dateString) {
    // Extrae el número de milisegundos
    const milliseconds = parseInt(dateString.replace(/\/Date\((\d+)\)\//, '$1'));
    const date = new Date(milliseconds);

    // Formato dd/mm/yyyy
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const year = date.getFullYear();

    return `${day}/${month}/${year}`;
}

function formatDateTime(dateString) {
    // Extrae el número de milisegundos
    const milliseconds = parseInt(dateString.replace(/\/Date\((\d+)\)\//, '$1'));
    const date = new Date(milliseconds);

    // Formato dd/mm/yyyy
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0'); // Meses son 0-indexados
    const year = date.getFullYear();

    // Formato hh:mm
    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');

    //return `${day}/${month}/${year}`;
    return `${day}/${month}/${year} ${hours}:${minutes}`;
}

function RefillDropDownListMultiple(element, data, data_id, data_name) {
    //Vacia el elemento
    element.empty();
    //Carga el elemento
    $.each(data, function (i, item) {
        var n_value = i, n_html = item;
        if (data_id) n_value = item[data_id];
        if (data_name) n_html = item[data_name];
        element.append($('<option></option>').val(n_value).html(n_html));
        if (i === 0) {
            primerElementoValor = n_value;
        }
    });

    element.multipleSelect('refresh');
    if (primerElementoValor !== null) {
        element.multipleSelect('setSelects', [primerElementoValor]);
    }
}

function SetMessage(container, msg, tpo, del) {
    var new_class = "msg-" + tpo;//Identifica la nueva clase css
    $(container).removeClass($(container).attr('class'));//Quita la clase css previa
    $(container).addClass(new_class);
    $(container).html(msg);//Carga el mensaje
    //$(container).show();

    //Focus to message
    $('html, body').animate({ scrollTop: 0 }, 5);

    //Mensaje con delay o no
    if (del) $(container).show(0).delay(5000).hide(0);//5 Segundos
    else $(container).show();
}