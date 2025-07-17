$(function () {

    $("#tblListMensaje").DataTable({
        filter: true,
        info: true,
        processing: true,
        scrollY: 500,
        scrollCollapse: true,
        paging: false,
        columns: [
            { orderable: false },
            null,
            null,
            null,
            null,
            null
        ]
    });

    $("#chbxMensajesAll").change(function () {
        $("input[name='chbxMensajes']").prop('checked', this.checked);
    });


});

