var hot = null;
var feriados = [];
var finessemana = [];
var modalidad = [];
var titulosGrupo = [];

function cargarGrilla(result) {
    feriados = result.Feriados;
    finessemana = result.FinesSemana;
    modalidad = result.ModalidadTrabajo;
    titulosGrupo = result.TitulosGrupo;
    var container = document.getElementById('listado');

    var hotOptions = {
        data: result.Data,
        height: 740,
        fillHandle: true,
        columnSorting: false,
        className: "htCenter",
        colWidths: result.AnchoColumnas,
        fixedRowsTop: 2,
        fixedColumnsLeft: 2,
        cells: function (row, col, prop) {

            var cellProperties = {};

            if (row == 0 || row == 1) {
                cellProperties.renderer = tituloRenderer;
                cellProperties.readOnly = true;
            }
            else {
                if (result.TitulosGrupo.includes(row)) {
                    cellProperties.renderer = colorSeperador;
                    cellProperties.readOnly = true;
                }
                else {
                    if ((col == 0 || col == 1)) {
                        cellProperties.renderer = ColorResponsable;
                        cellProperties.readOnly = true;
                    }
                    else {
                        cellProperties.renderer = customDropdownRenderer;
                        cellProperties.editor = "chosen";
                        cellProperties.chosenOptions = {};
                        cellProperties.chosenOptions.multiple = true;
                        cellProperties.chosenOptions.data = result.ListaActividad;
                    }
                }
            }

            return cellProperties;
        }
    };

    if (hot != null) hot.destroy();

    hot = new Handsontable(container, hotOptions);
    hot.render();
    hot.updateSettings({
        contextMenu: {
            callback: function (key, options) {
                var filaIni = hot.getSelected()[0];
                var columnaIni = hot.getSelected()[1];
                var filaFin = hot.getSelected()[2];
                var columnaFin = hot.getSelected()[3];

                for (var row = filaIni; row <= filaFin; row++) {
                    for (var column = columnaIni; column <= columnaFin; column++) {
                        modalidad[row][column] = key;
                    }
                }
                hot.render();

            },
            items: {
                "R": {
                    name: function () {
                        return " <div class= 'icon-derecho'><div class='rojo'></div><div>Marcar como remoto</div></div> ";
                    },
                    disabled: function () {
                        return (hot.getSelected()[0] <= 1 || hot.getSelected()[1] < 2);
                    }
                },
                "P": {
                    name: function () {
                        return " <div class= 'icon-derecho'><div class='negro'></div><div>Marcar como presencial</div></div> ";
                    },
                    disabled: function () {
                        return (hot.getSelected()[0] <= 1 || hot.getSelected()[1] < 2);
                    }
                }
            }
        }
    });
}

var tituloRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.backgroundColor = '#3D8AB8';
    td.style.fontWeight = 'bold';
    td.style.fontSize = '11px';
    td.style.color = '#FFFFFF';
    td.style.verticalAlign = 'middle';
};

var ColorResponsable = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.backgroundColor = '#CFE4FA';
    td.style.fontWeight = 'bold';
    td.style.fontSize = '11px';
    td.style.verticalAlign = 'middle';
};

var colorSeperador = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.backgroundColor = '#000000';
    td.style.fontSize = '11px';
    td.style.color = 'white';
};

function customDropdownRenderer(instance, td, row, col, prop, value, cellProperties) {
    var selectedId;
    var optionsList = cellProperties.chosenOptions.data;

    if (typeof optionsList === "undefined" || typeof optionsList.length === "undefined" || !optionsList.length) {
        Handsontable.TextCell.renderer(instance, td, row, col, prop, value, cellProperties);
        return td;
    }

    var values = (value + "").split(",");
    value = [];
    for (var index = 0; index < optionsList.length; index++) {

        if (values.indexOf(optionsList[index].id + "") > -1) {
            selectedId = optionsList[index].id;
            value.push(optionsList[index].label);
        }
    }
    value = value.join(", ");
    Handsontable.TextCell.renderer(instance, td, row, col, prop, value, cellProperties);

    if (finessemana[col] == 1) {
        td.style.backgroundColor = 'yellow';
    }
    if (feriados[col] == 1) {
        td.style.backgroundColor = 'green';
    }
    td.style.color = 'red';
    if (modalidad[row][col] == "P") {
        td.style.color = 'black';
    }

    return td;
}

function validarExistencia() {
    var data = hot.getData();

    var flagDatos = false;
    for (var row = 2; row < data.length; row++) {
        if (!titulosGrupo.includes(row)) {
            for (var column = 2; column < data[0].length; column++) {
                if (data[row][column] != undefined && data[row][column] != "") {
                    flagDatos = true;
                }
            }
        }
    }

    return flagDatos;
}

function getData() {
    return hot.getData();
}

function getModalidad() {
    return modalidad;
}