
$(function () {
    $("#tvGeneral").fancytree({
        autoScroll: true, // Automatically scroll nodes into visible area.
        clickFolderMode: 3, // 1:activate, 2:expand, 3:activate and expand, 4:activate (dblclick expands)
        debugLevel: 2, // 0:quiet, 1:normal, 2:debug
        quicksearch: true, // Navigate to next node by typing the first letters.
        selectMode: 1, // 1:single, 2:multi, 3:multi-hier
        source: window.treeData,
        click: function (e, data) {
            var key = data.node.key;
            if ((key !== "1000" && key !== "2000") && (key.toString() !== $("#CodigoEntidad").val().toString())) {
                $("#CodigoEntidad").val(key);
                $("#CodigoEntidad").trigger("change");
            }
        }
    });

    $("#tvGeneral").fancytree("getRootNode").visit(function (node) {
        node.setExpanded(true);
    });

    $("#CodigoEntidad").change(function () {
        if ($("#NombreEntidad").length) {
            $("#NombreEntidad").val("");
        }

        document.getElementById('mensaje').style.display = 'none';
        radio = "P";
        getFiltro();
        getListado();
    });
});