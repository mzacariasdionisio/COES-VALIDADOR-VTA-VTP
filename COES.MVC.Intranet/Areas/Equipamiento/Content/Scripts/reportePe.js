var controlador = siteRoot + 'Equipamiento/ReportePotencia/';
$(function() {
    $('#FechaDesde').Zebra_DatePicker({
        format: 'm Y'
    });
    $('#FechaHasta').Zebra_DatePicker({
        format: 'm Y'
    });
});