/// Crea El objeto Handson en la pagina web
let container;
function crearHandsonTable(evt) {

    let matrizTipoColor = evt.MatrizExcelColores;

    function fechaRowRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.color = 'white';
        td.style.background = 'rgb(41, 128, 185)';
    }

    function firstRowRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontWeight = 'bold';
        td.style.fontSize = '12px';
        td.style.color = 'MidnightBlue';
        td.style.background = '#2980B9';
    }

    function firstRowRenderer2(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontWeight = 'bold';
        td.style.fontSize = '10px';
        td.style.color = 'White';
        td.style.background = '#2980B9';
    }

    function descripRowRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '10px';
        td.style.color = 'MidnightBlue';
        td.style.background = '#EAF7D9';
    }

    function descripRowRenderer2(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '10px';
        td.style.color = 'MidnightBlue';
        td.style.background = '#E8F6FF';
    }

    function cambiosCellRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.textAlign = 'right';      
        td.style.background = '#FFFFD7';
        $(td).html(formatFloat(parseFloat(value), 3, '.', ','));
    }

    function readOnlyValueRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.textAlign = 'right';
        td.style.color = 'DimGray';
        td.style.background = 'Gainsboro';
        if (parseFloat(value))
            $(td).html(formatFloat(parseFloat(value), 3, '.', ','));
        else if(value == "0"){
                $(td).html("0.000");
        }
    }
    
    container = document.getElementById('detalleFormato');
        let hotOptions = {
        data: evt.Handson.ListaExcelData,
        height: 1200,
        maxRows: evt.Handson.ListaExcelData.length,
        maxCols: evt.Handson.ListaExcelData[0].length,       
        colHeaders: true,
        rowHeaders: true,        
        fillHandle: true,
        columnSorting: false,        
        className: "htCenter",
        readOnly: evt.Handson.ReadOnly,
        fixedRowsTop: evt.FilasCabecera,
        fixedColumnsLeft: evt.ColumnasCabecera,
        mergeCells: evt.Handson.ListaMerge,
        colWidths: evt.Handson.ListaColWidth,
        afterLoadData: function () {
            this.render();
        },        

        cells: function (row, col, prop) {
            let cellProperties = {};
            let formato = "";
            let render;
            let readOnly=true;
            let tipo;
            if (col === 0) {               
                render = fechaRowRenderer;
            }
            else if (evt.Handson.ReadOnly) {
                    render = readOnlyValueRenderer;
                
            }

            if (row < evt.FilasCabecera) {
                if (row == 0) {
                    let indMerge = findIndiceMerge(col, evt.Handson.ListaMerge);
                    if ((indMerge % 2) == 0) {
                        render = firstRowRenderer;
                    }
                    else {
                        render = firstRowRenderer2;
                    }
                }
                else if ((row % 2) == 0) {
                        render = descripRowRenderer;
                }
                else {
                    render = descripRowRenderer2;
                }
                
            }

            // zona de datos
            if ((row >= evt.FilasCabecera) && (col >= evt.ColumnasCabecera) && (!evt.Handson.ReadOnly)) {
                render = readOnlyValueRenderer;
                formato = '0,0.000';
                tipo = 'numeric';
                // imprimimos color de celda
                
                if (matrizTipoColor != null) {
                    render = render_readonly(this, row, col, matrizTipoColor[row][col]);
                }
                

            }
            for (let i in evt.ListaCambios) {
                if ((row == evt.ListaCambios[i].Row) && (col == evt.ListaCambios[i].Col)) {                  
                    render = cambiosCellRenderer;
                    formato = '0,0.000';
                    tipo = 'numeric';
                }
            }

            cellProperties = {
                renderer: render,
                format: formato,
                type: tipo,
                readOnly: readOnly
            }

            return cellProperties;
        }
    };
        
    let hot = new Handsontable(container, hotOptions);
}


function render_readonly(ht, row, col, tipo) {
    let font_color = "DimGray";
    let fondo ="Silver";
    switch (tipo) {// solo lectura toda la matriz
        case 0: fondo = "Bisque"; break;
        case 1: fondo = "#E0F8F7"; break;
        case 2: fondo = "#F2F2F2"; break;
        case 3: fondo = "#E0F8F7"; break;
        case 4: fondo = "#F2F2F2"; break;
        case 5: fondo = "#E0F8F7"; break;
        case 6: fondo = "#F2F2F2"; break;
        case 7: fondo = "#E0F8F7"; break;
        case 8: fondo = "#F2F2F2"; break;
        case 9: fondo = "#E0F8F7"; break;
        case 10: fondo = "#F2F2F2"; break;
    }

    ht.instance.getDataAtCell(row, col);

    $(ht.instance.getCell(row, col)).css(
        {
            "color": font_color,
            "background-color": fondo,
            "vertical-align": "middle"
        })

}


