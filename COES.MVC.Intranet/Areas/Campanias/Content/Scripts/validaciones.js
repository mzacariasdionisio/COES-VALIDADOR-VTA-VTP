
function getDaysInYear(year) {
    return ((year % 4 === 0 && year % 100 > 0) || year % 400 === 0) ? 366 : 365;
}

function calculateLoadFactor(energia, potenciaHP, potenciaHFP, year) {
    const diasAnio = getDaysInYear(year);
    const maxPotencia = Math.max(potenciaHP || 0, potenciaHFP || 0);

    if (maxPotencia === 0 || !energia) return 0;

    const factorCarga = (energia * 1000) / (24 * diasAnio * maxPotencia);
    return parseFloat(factorCarga.toFixed(4));
}
function loadFactorRenderer(instance, td, row, col, prop, value, cellProperties) {
    // Primero aplicamos el renderizado num�rico b�sico
    Handsontable.renderers.NumericRenderer.apply(this, arguments);

    // Aplicar el formato de 4 decimales
    if (typeof value === 'number') {
        td.innerHTML = value.toFixed(4);
    }

    const numValue = parseFloat(value);

    // Aplicar estilos directamente al elemento TD
    if (!isNaN(numValue)) {
        if (numValue < 0 || numValue > 1) {
            // Estilo para valores fuera de rango
            td.style.backgroundColor = '#ffebee'; // Fondo rojo claro
            td.style.color = '#d32f2f';          // Texto rojo oscuro
            td.style.fontWeight = 'bold';
        } else {
            // Estilo para valores dentro del rango
            td.style.backgroundColor = 'white';
            td.style.color = 'black';
            td.style.fontWeight = 'normal';
        }
    }
}
// Y la configuraci�n de la columna deber��a ser as��:
// En la configuraci�n de la tabla:
// En tu configuraci�n de tabla, modifica la definici�n de las columnas as��:
columns: [
    { type: 'numeric' }, // A�o
    { type: 'text' }, // Mes
    { type: 'numeric', format: '0.0000' }, // Energ��a
    { type: 'numeric', format: '0.0000' }, // HP
    {
        type: 'numeric',
        format: '0.0000',
        readOnly: true,
        renderer: function (instance, td, row, col, prop, value, cellProperties) {
            // Aplicar el renderizado num�rico b�sico
            Handsontable.renderers.NumericRenderer.apply(this, arguments);

            // Formatear a 4 decimales
            if (typeof value === 'number') {
                td.innerHTML = value.toFixed(4);
            }

            const numValue = parseFloat(value);
            if (!isNaN(numValue)) {
                if (numValue < 0 || numValue > 1) {
                    // Aplicar estilos directamente
                    td.style.backgroundColor = '#ffebee';
                    td.style.color = '#d32f2f';
                    td.style.fontWeight = 'bold';
                } else {
                    td.style.backgroundColor = 'white';
                    td.style.color = 'black';
                    td.style.fontWeight = 'normal';
                }
            }
        }
    }, // Factor de Carga
    { type: 'numeric', format: '0.0000' }, // Generaci�n
    { type: 'numeric', format: '0.0000' }, // HP
    { type: 'numeric', format: '0.0000' }  // HFP
]
// Tambi�n podemos agregar estos estilos CSS
const style = document.createElement('style');
style.textContent = `
    .factor-carga-cell {
        transition: color 0.3s ease;
    }
    .factor-carga-column.htNumeric {
        font-family: inherit !important;
    }
    .factor-carga-cell[style*="color: #ff0000"] {
        background-color: inherit !important;
    }
`;
document.head.appendChild(style);
function positiveNumberValidator(value, callback) {
    if (value === "" || (typeof value === 'number' && value >= 0 && value.toString().length === 4)) {
        callback(true);
    } else {
        console.error("Valor inválido: debe ser un número positivo de 4 dígitos.");
        mostrarNotificacion("Por favor, ingresa un valor de Periodo en años");
        callback(false);
    }
}

function positiveDecimalValidator(value, callback) {
    if (value === "" || (!isNaN(value) && parseFloat(value) >= 0)) {
        callback(true);
    } else {
        callback(false);
    }
}

function decimalRenderer(instance, td, row, col, prop, value, cellProperties) {
    if (typeof value === 'number') {
        value = value.toFixed(4);
    }
    Handsontable.renderers.TextRenderer.apply(this, arguments);
}

let isChangeValid = true;

function generalBeforeChange(hotInstance, changes, source) {
    if (source === "edit" || source === "paste" || source === "autofill") {
        isChangeValid = true;

        changes.forEach(function ([row, prop, oldValue, newValue]) {

            positiveDecimalValidator(newValue, function (isValid) {
                console.log('validar new value', newValue)
                if (!isValid) {
                    isChangeValid = false;
                    invalidValueFound = true;


                    console.error(`Valor inválido en la celda [${row}, ${prop}]: ${newValue}`);

                    hotInstance.setDataAtCell(row, prop, oldValue, 'edit');

                    mostrarNotificacion(`Por favor, ingresa un valor numerico valido en la celda [${row}, ${prop}]`);
                }
            });
        });
    }
}
function generalBeforeChange_2(hotInstance, changes, source) {
    if (source === "edit" || source === "paste" || source === "autofill") {
        isChangeValid = true;

        changes.forEach(function ([row, prop, oldValue, newValue]) {
            // Si la columna es Factor de Carga, no permitir edici�n directa
            if (prop === 4) {
                isChangeValid = false;
                mostrarNotificacion("El Factor de Carga se calcula automaticamente");
                hotInstance.setDataAtCell(row, prop, oldValue, 'edit');
                return;
            }

            positiveDecimalValidator(newValue, function (isValid) {
                if (!isValid) {
                    isChangeValid = false;
                    console.error(`Valor inválido en la celda [${row}, ${prop}]: ${newValue}`);
                    hotInstance.setDataAtCell(row, prop, oldValue, 'edit');
                    mostrarNotificacion(`Por favor, ingresa un valor numérico válido en la celda [${row}, ${prop}]`);
                }
            });
        });
    }
}

//function generalAfterChange_3(hotInstance, changes, configuracionColumnas) {
//    let isAdjusting = false;

//    if (!isAdjusting) {
//        isAdjusting = true; // Activar bandera para evitar bucles infinitos

//        changes.forEach(([row, col, oldValue, newValue]) => {
//            // Verificar si la columna tiene una configuraci�n espec��fica
//            const config = configuracionColumnas[col];
//            if (config) {
//                if (newValue !== null && newValue !== "") {
//                    const parsedValue = parseFloat(newValue);

//                    // Validar si el valor es num�rico
//                    if (isNaN(parsedValue)) {
//                        mostrarNotificacion(
//                            `Por favor, ingresa un valor num�rico v�lido en la celda [${row + 1}, ${col + 1}]`
//                        );
//                        hotInstance.setDataAtCell(row, col, oldValue); // Restaurar el valor anterior
//                        return;
//                    }

//                    // Validar si se permite negativo
//                    if (!config.permitirNegativo && parsedValue < 0) {
//                        mostrarNotificacion(
//                            `Los valores negativos no est�n permitidos en la celda [${row + 1}, ${col + 1}]`
//                        );
//                        hotInstance.setDataAtCell(row, col, oldValue); // Restaurar el valor anterior
//                        return;
//                    }

//                    // Formatear el valor seg�n el tipo y decimales
//                    let formattedValue;

//                    if (config.tipo === "decimal") {
//                        formattedValue = parsedValue.toFixed(config.decimales || 2);
//                    } else if (config.tipo === "entero") {
//                        formattedValue = Math.round(parsedValue).toString();
//                    }

//                    if (newValue !== formattedValue) {
//                        hotInstance.setDataAtCell(row, col, formattedValue); // Ajustar al formato especificado
//                    }
//                }
//            }
//        });

//        isAdjusting = false; // Desactivar bandera
//    }
//}

function generalBeforeChange_3(hotInstance, changes, configuracionColumnas) {

    let isValid = true;

    changes.forEach(([row, col, oldValue, newValue]) => {
        const config = configuracionColumnas[col]; // Obt�n la configuraci�n basada en el ��ndice de la columna
        if (config) {
            if (newValue !== null && newValue !== "") {
                let parsedValue = parseFloat(newValue);

                // Validar si el valor es num�rico
                if (isNaN(parsedValue)) {

                    mostrarNotificacion(
                        `Valor inválido en fila ${row + 1}, columna ${col + 1}. Ingrese un número válido.`
                    );
                    isValid = false;
                    return; // Detener validaci�n para esta celda
                }

                // Validar si se permite negativo
                if (!config.permitirNegativo && parsedValue < 0) {
                    mostrarNotificacion(
                        `Los valores negativos no están permitidos en fila ${row + 1}, columna ${col + 1}.`
                    );
                    isValid = false;
                    return;
                }

                // Validar tipo "a�o"
                if (config.tipo === "anio") {
                    if (!/^\d{4}$/.test(newValue)) {
                        mostrarNotificacion(
                            `El valor en fila ${row + 1}, columna ${col + 1} debe ser un año válido (número de 4 dígitos).`
                        );
                        isValid = false;
                        return;
                    }
                }

                // Validar y formatear n�mero de decimales
                if (config.tipo === "decimal") {
                    const decimalLength = newValue.split(".")[1]?.length || 0;
                    if (decimalLength > config.decimales) {
                        mostrarNotificacion(
                            `El valor en fila ${row + 1}, columna ${col + 1} debe tener un máximo de ${config.decimales} decimales.`
                        );
                        isValid = false;
                        return;
                    }

                    // Formatear autom�ticamente a los decimales configurados
                    parsedValue = parsedValue.toFixed(config.decimales);
                    changes.forEach(change => {
                        const [changeRow, changeCol] = change;
                        if (changeRow === row && changeCol === col) {
                            change[3] = parsedValue; // Actualizar el valor formateado
                        }
                    });
                }

                // Validar tipo entero
                if (config.tipo === "entero") {
                    // Si el valor tiene decimales, formatearlo como un entero
                    if (!Number.isInteger(parsedValue)) {
                        mostrarNotificacion(
                            `El valor en fila ${row + 1}, columna ${col + 1} debe ser un número entero. Se ajustó automaticamente.`
                        );
                        parsedValue = Math.round(parsedValue);
                    }

                    // Actualizar el valor para reflejarlo sin decimales
                    changes.forEach(change => {
                        const [changeRow, changeCol] = change;
                        if (changeRow === row && changeCol === col) {
                            change[3] = parsedValue.toString(); // Eliminar cualquier formato decimal
                        }
                    });
                }
            }
        }
    });

    return isValid; // Retorna true si todos los cambios son v�lidos
}


/*Validacion para tablas before,after */
function generalBeforeChange_4(hotInstance, changes, configuracionColumnas) {
    changes.forEach(([row, col, oldValue, newValue]) => {
        const config = configuracionColumnas[col]; // Configuraci�n basada en la columna

        if (config) {
            const cellMeta = hotInstance.getCellMeta(row, col);
            let stringValue = newValue;
                 if (newValue !== null && newValue !== "") {
                if (config.tipo === "decimal" || config.tipo === "decimaltruncal") {
                    stringValue = stringValue.replace(/,/g, ""); // Eliminar comas solo para decimales
                }
       
                let parsedValue = parseFloat(stringValue);

                // Validar si el valor es num�rico
                if (config.tipo === "texto") {
                    // Validar la longitud del texto
                    if (newValue.length > config.largo) {
                        cellMeta.valid = false;
                        cellMeta.style = { backgroundColor: "#ffcccc" }; // Fondo rojo claro
                        mostrarNotificacion(`Error: La longitud máxima permitida es ${config.largo} caracteres en la fila ${row + 1}, columna ${col + 1}.`);
                    } else {
                        cellMeta.valid = true;
                        cellMeta.style = { backgroundColor: "" }; // Quitar fondo rojo
                    }
                } else if (isNaN(parsedValue)) {
                    cellMeta.valid = false;
                    cellMeta.style = { backgroundColor: "#ffcccc" }; // Fondo rojo claro
                } else if (!config.permitirNegativo && parsedValue < 0) {
                    cellMeta.valid = false;
                    cellMeta.style = { backgroundColor: "#ffcccc" }; // Fondo rojo claro
                } else if (config.tipo === "especial") {
                    // Permitir solo valores 17, 18, 19
                    const valoresPermitidos = [17, 18, 19];
                    if (!valoresPermitidos.includes(parsedValue)) {
                        cellMeta.valid = false;
                        cellMeta.style = { backgroundColor: "#ffcccc" }; // Fondo rojo claro
                    } else {
                        cellMeta.valid = true;
                        cellMeta.style = { backgroundColor: "" }; // Quitar fondo rojo
                    }
                } else if (config.tipo === "decimal") {
                    const decimales = config.decimales !== undefined ? config.decimales : 2; // Decimales predeterminados
                    newValue = parsedValue.toFixed(decimales); // Formatear el valor

                    // Actualizar el cambio con el valor formateado
                    changes.forEach(change => {
                        if (change[0] === row && change[1] === col) {
                            change[3] = newValue;
                        }
                    });

                    cellMeta.valid = true;
                    cellMeta.style = { backgroundColor: "" }; // Quitar fondo rojo
                } else if (config.tipo === "decimaltruncal") {
                    const truncDecimales = config.decimales !== undefined ? config.decimales : 4; // Decimales predeterminados
                    const factor = Math.pow(10, truncDecimales);

                    // Truncar si tiene m�s de 'truncDecimales' decimales
                    parsedValue = Math.trunc(parsedValue * factor) / factor;

                    // Completar con ceros si tiene menos de 'truncDecimales' decimales
                    newValue = parsedValue.toFixed(truncDecimales);

                    changes.forEach(change => {
                        if (change[0] === row && change[1] === col) {
                            change[3] = newValue;
                        }
                    });

                    cellMeta.valid = true;
                    cellMeta.style = { backgroundColor: "" }; // Quitar fondo rojo
                } else if (config.tipo === "entero") {
                    if (!Number.isInteger(parsedValue)) {
                        cellMeta.valid = false;
                        cellMeta.style = { backgroundColor: "#ffcccc" }; // Fondo rojo claro
                    } else {
                        cellMeta.valid = true;
                        cellMeta.style = { backgroundColor: "" }; // Quitar fondo rojo
                    }
                } else {
                    // Valor v�lido, limpiar errores previos
                    cellMeta.valid = true;
                    cellMeta.style = { backgroundColor: "" }; // Quitar fondo rojo
                }
            } else {
                // Si el nuevo valor est� vac�o, se considera v�lido
                cellMeta.valid = true;
                cellMeta.style = { backgroundColor: "" };
            }
        }
    });

    hotInstance.render(); // Refrescar la tabla para aplicar estilos
}

function generalAfterChange_global(hotInstance, changes, configGlobal) {
    changes.forEach(([row, col, oldValue, newValue]) => {
        const config = configGlobal[col]; // Configuraci�n basada en la columna
        const cellMeta = hotInstance.getCellMeta(row, col);

        // Si la celda est� vac�a, se considera v�lida
        if (newValue === null || newValue === "") {
            cellMeta.valid = true;
            cellMeta.style = { backgroundColor: "" }; // Quitar color de fondo
            cambiosRealizados = true;
        }
    });

    hotInstance.render(); // Refrescar la tabla para aplicar estilos
}


function validarTablaAntesDeGuardar(hotInstance, opcional = '') {
    const data = hotInstance.getData();
    let totalErrores = 0;
    let celdasConErrores = [];

    for (let row = 0; row < data.length; row++) {
        for (let col = 0; col < data[row].length; col++) {
            const cellMeta = hotInstance.getCellMeta(row, col);
            if (cellMeta && cellMeta.valid === false) {
                totalErrores++;
                celdasConErrores.push(`Fila ${row + 1}, Columna ${col + 1}`);
                // Resaltar celda con error
                hotInstance.setCellMeta(row, col, "style", { backgroundColor: "#ffcccc" });
            }
        }
    }
    if (totalErrores > 0) {
        mostrarMensaje('mensajeFicha', 'error', 'Error al guardar. Corrija los errores. Si cambia de pestaña, los datos se perderán.', true);
        const mensajeErrores = `
Total de errores: ${totalErrores}${opcional ? ` (${opcional})` : ''}.

Mostrando las primeras 10:
${celdasConErrores.slice(0, 10).join("\n")}`;
        mostrarNotificacion(mensajeErrores); // Mostrar mensaje con los detalles de los errores
        return false; // No permitir guardar
    }

    hotInstance.render(); // Actualizar la tabla para aplicar estilos

    console.log("Validación exitosa. No se encontraron errores.");
    return true; // Permitir guardar si no hay errores
}




/*Validacion para tablas before,after */

//function validarYFormatearDatosGlobal({
//    data,
//    configuracionColumnas,
//    mostrarNotificacion,
//    hotInstance,
//    filasIgnorar = [],
//    columnasIgnorar = []
//}) {
//    if (!data || !Array.isArray(data)) {
//        console.error("Los datos proporcionados no son v�lidos.");
//        return [];
//    }

//    const tieneDatos = data.some(row => row.some(value => value !== ""));
//    let celdasConErrores = [];
//    let totalErrores = 0;

//    if (!tieneDatos) {
//        console.log("Los datos est�n vac��os, no se aplica validaci�n ni formateo.");
//        return data;
//    }

//    data.forEach((row, rowIndex) => {
//        if (filasIgnorar.includes(rowIndex)) return;

//        row.forEach((value, colIndex) => {
//            if (columnasIgnorar.includes(colIndex)) return;

//            const config = configuracionColumnas[colIndex];
//            if (config && value !== "" && value !== null && value !== undefined) {
//                let parsedValue = parseFloat(value);
//                let esValido = true;

//                if (isNaN(parsedValue)) {
//                    esValido = false;
//                } else if (!config.permitirNegativo && parsedValue < 0) {
//                    esValido = false;
//                } else if (config.tipo === "decimal") {
//                    parsedValue = parsedValue.toFixed(config.decimales);
//                    row[colIndex] = parsedValue;
//                } else if (config.tipo === "entero" && !Number.isInteger(parsedValue)) {
//                    esValido = false;
//                } else if (config.tipo === "especial") {
//                    const valoresPermitidos = config.valoresPermitidos || [];
//                    if (!valoresPermitidos.includes(parsedValue)) {
//                        esValido = false;
//                    }
//                }

//                if (!esValido) {
//                    totalErrores++;
//                    celdasConErrores.push(`Fila ${rowIndex + 1}, Columna ${colIndex + 1}, Valor: ${value}`);
//                    if (hotInstance) {
//                        hotInstance.setCellMeta(rowIndex, colIndex, "valid", false);
//                        hotInstance.setCellMeta(rowIndex, colIndex, "className", "cell-error"); // Clase CSS para errores
//                    }
//                } else if (hotInstance) {
//                    hotInstance.setCellMeta(rowIndex, colIndex, "valid", true);
//                    hotInstance.setCellMeta(rowIndex, colIndex, "className", ""); // Limpiar clase previa
//                }
//            }
//        });
//    });

//    if (hotInstance) {
//        hotInstance.render(); // Refrescar tabla para aplicar estilos
//    }

//    if (totalErrores > 0) {
//        const mensajeErrores = `
//Total de errores: ${totalErrores}.<br>
//Mostrando las primeras 10:<br>
//${celdasConErrores.slice(0, 10).join("<br>")}`;
//        mostrarNotificacion(mensajeErrores);
//    }

//    return data;
//}


function validarYFormatearDatos2(data, configuracionColumnas, mostrarNotificacion, hotInstance) {
    const tieneDatos = data.some(row => row.some(value => value !== ""));
    let celdasConErrores = [];
    let totalErrores = 0;

    // Si no tiene datos, retornar la data sin procesar
    if (!tieneDatos) {
        console.log("Los datos están vacíos, no se aplica validación ni formateo.");
        return data;
    }
    // Procesar solo si hay datos v�lidos
    data.forEach((row, rowIndex) => {
        row.forEach((value, colIndex) => {
            const config = configuracionColumnas[colIndex];
            if (config && value !== "" && value !== null && value !== undefined) {
                let prev = String(value);
                if (config.tipo === "decimal" || config.tipo === "decimaltruncal" || config.tipo === "entero") {
                    prev = prev.replace(/,/g, ""); // Eliminar comas solo para decimales
                }
                let parsedValue = parseFloat(prev);
                const stringValue = value.toString(); // Convertir a cadena para validaciones de formato

                // Inicializar como v�lido por defecto
                let esValido = true;

                if (config.tipo === "texto") {
                    // Validar la longitud del texto
                    if (stringValue.length > config.largo) {
                        esValido = false;
                        totalErrores++;
                        if (celdasConErrores.length < 10) {
                            celdasConErrores.push(
                                `Fila ${rowIndex + 1}, Columna ${colIndex + 1}, (Longitud excede ${config.largo} caracteres)`
                            );
                        }
                    }
                } else if (isNaN(parsedValue)) {
                    // Valor no num�rico
                    esValido = false;
                    totalErrores++;
                    if (celdasConErrores.length < 10) {
                        celdasConErrores.push(`Fila ${rowIndex + 1}, Columna ${colIndex + 1}`);
                    }
                } else if (!config.permitirNegativo && parsedValue < 0) {
                    // Valor negativo no permitido
                    esValido = false;
                    totalErrores++;
                    if (celdasConErrores.length < 10) {
                        celdasConErrores.push(`Fila ${rowIndex + 1}, Columna ${colIndex + 1}`);
                    }
                } else if (config.tipo === "decimal") {
                    // Formatear a la cantidad de decimales configurada
                    parsedValue = parsedValue.toFixed(config.decimales);
                    row[colIndex] = parsedValue; // Actualizar el valor formateado en la tabla
                } else if (config.tipo === "decimaltruncal") {
                    // Formatear a la cantidad de decimales configurada
                    const factor = Math.pow(10, config.decimales);
                    parsedValue = Math.floor(parsedValue * factor) / factor;
                    row[colIndex] = parsedValue; // Actualizar el valor formateado en la tabla
                } else if (config.tipo === "entero") {
                    // Validar si es entero
                    if (!Number.isInteger(parsedValue)) {
                        esValido = false; // Marcar como inv�lido
                        totalErrores++;
                        if (celdasConErrores.length < 10) {
                            celdasConErrores.push(`Fila ${rowIndex + 1}, Columna ${colIndex + 1}`);
                        }
                    } else {
                        row[colIndex] = Math.round(parsedValue).toString(); // Formatear a entero
                    }
                } else if (config.tipo === "especial") {
                    // Validar valores especiales permitidos (17, 18, 19)
                    const valoresPermitidos = [17, 18, 19];
                    if (!valoresPermitidos.includes(parsedValue)) {
                        esValido = false; // Marcar como inv�lido
                        totalErrores++;
                        if (celdasConErrores.length < 10) {
                            celdasConErrores.push(`Fila ${rowIndex + 1}, Columna ${colIndex + 1}`);
                        }
                    }
                }

                // Marcar la celda como v�lida o inv�lida
                if (hotInstance) {
                    hotInstance.setCellMeta(rowIndex, colIndex, 'valid', esValido);
                    hotInstance.setCellMeta(rowIndex, colIndex, 'style', {
                        backgroundColor: esValido ? '' : '#ffcccc', // Rojo claro para celdas inv�lidas
                    });
                }
            }
        });
    });

    // Mostrar notificaci�n acumulada si hay errores
    if (totalErrores > 0) {
        const mensajeErrores = `
Total de errores: ${totalErrores}.\n
Mostrando las primeras 10:\n
${celdasConErrores.join("\n")}`;
        mostrarNotificacion(mensajeErrores);
    }

    if (hotInstance) {
        hotInstance.render(); // Refrescar la tabla para aplicar los estilos
    }

    return data; // Retorna los datos con los cambios realizados
}

function validarYFormatearDatos(data, configuracionColumnas, mostrarNotificacion) {
    // Verificar si la data tiene al menos un valor no vac��o
    const tieneDatos = data.some(row => row.some(value => value !== ""));

    // Si no tiene datos, retornar la data sin procesar
    if (!tieneDatos) {
        console.log("Los datos están vacíos, no se aplica validación ni formateo.");
        return data;
    }

    // Procesar solo si hay datos v�lidos
    data.forEach((row, rowIndex) => {
        row.forEach((value, colIndex) => {
            const config = configuracionColumnas[colIndex];
            if (config) {
                if (value !== "" && value !== null && value !== undefined) {

                    const parsedValue = parseFloat(value);

                    // Si el valor no es num�rico o es NaN, reemplazar por vac��o
                    if (isNaN(parsedValue)) {

                        mostrarNotificacion(`Valor no válido detectado en la fila ${rowIndex + 1}, columna ${colIndex + 1}. Se ha eliminado.`);
                        row[colIndex] = ""; // Limpiar valores no num�ricos
                    } else {
                        // Validar si se permiten negativos
                        if (!config.permitirNegativo && parsedValue < 0) {
                            mostrarNotificacion(`Valor negativo no permitido en la fila ${rowIndex + 1}, columna ${colIndex + 1}. Se ha eliminado.`);
                            row[colIndex] = ""; // Limpiar valores negativos no permitidos
                        } else if (config.tipo === "decimal") {
                            // Formatear a la cantidad de decimales especificada
                            row[colIndex] = parsedValue.toFixed(config.decimales || 2);
                        } else if (config.tipo === "entero") {
                            // Redondear a entero
                            row[colIndex] = Math.round(parsedValue).toString();
                        }
                    }

                }
            }
        });
    });

    return data; // Retorna los datos validados y formateados
}



let isChangeVal = true;

function generalAfterChange(hotInstance, changes, source) {
    if ((source === "edit" || source === "paste" || source === "autofill") && changes) {

        changes.forEach(function ([row, prop, oldValue, newValue]) {
            var numericValue = parseFloat(newValue);
            if (!isNaN(numericValue) && numericValue >= 0) {

                var formattedValue = numericValue.toFixed(4);

                hotInstance.setDataAtCell(row, prop, parseFloat(formattedValue), 'set');
            } else {
                console.log("New value is not a number:", newValue);
            }
        });
    } else {
        console.log("Source is not 'edit' or isChangeValid is false");
    }
}

let isProcessing = false; // Bandera para evitar bucles infinitos

function generalAfterChange_2(hotInstance, changes, source) {
    if ((source === "edit" || source === "paste" || source === "autofill") && changes) {
        if (isProcessing) return; // Evita recursiones si ya estamos procesando
        isProcessing = true; // Activa la bandera

        changes.forEach(function ([row, prop, oldValue, newValue]) {
            const numericValue = parseFloat(newValue);

            // Validaci�n de valores num�ricos o vac�os
            if (!isNaN(numericValue) || newValue === "" || newValue === null) {
                if (!isNaN(numericValue)) {
                    const formattedValue = numericValue.toFixed(4);
                    if (formattedValue !== newValue) {
                        hotInstance.setDataAtCell(row, prop, formattedValue, 'format'); // Actualiza con formato
                    }
                }
                // Limpia errores si la celda est� vac�a o contiene un valor v�lido
                hotInstance.setCellMeta(row, prop, 'valid', true);
                hotInstance.setCellMeta(row, prop, 'className', ''); // Limpia estilos de error
            } else {
                // Marca como inv�lido si el valor no es num�rico ni vac�o
                hotInstance.setCellMeta(row, prop, 'valid', false);
                hotInstance.setCellMeta(row, prop, 'className', 'cell-invalid'); // Resalta como inv�lido
            }

            // C�lculo de Factor de Carga para columnas espec�ficas
            if ([1, 2, 3].includes(prop)) {
                const rowData = hotInstance.getDataAtRow(row);
                const year = parseInt(rowData[0]) || 0;
                const energia = parseFloat(rowData[1]) || 0;
                const potenciaHP = parseFloat(rowData[2]) || 0;
                const potenciaHFP = parseFloat(rowData[3]) || 0;

                const factorCarga = calculateLoadFactor(energia, potenciaHP, potenciaHFP, year);

                // Solo actualiza si el valor calculado difiere del actual
                const existingFactor = parseFloat(hotInstance.getDataAtCell(row, 4));
                if (existingFactor?.toFixed(4) !== factorCarga.toFixed(4)) {
                    hotInstance.setDataAtCell(row, 4, factorCarga.toFixed(4), 'auto');
                }

                // Validar el rango del Factor de Carga
                if (factorCarga < 0 || factorCarga > 1) {
                    hotInstance.setCellMeta(row, 4, 'valid', false);
                    hotInstance.setCellMeta(row, 4, 'className', 'cell-invalid');
                    mostrarNotificacion(
                        `El Factor de Carga (${factorCarga.toFixed(4)}) está fuera del rango permitido (0-1).`
                    );
                } else {
                    hotInstance.setCellMeta(row, 4, 'valid', true);
                    hotInstance.setCellMeta(row, 4, 'className', '');
                }
            }
        });

        isProcessing = false; // Desactiva la bandera
    } else {
        // Validar todas las celdas de la columna Factor de Carga
        const data = hotInstance.getData();
        data.forEach((row, index) => {
            const factorCarga = parseFloat(row[4]);
            if (factorCarga < 0 || factorCarga > 1) {
                hotInstance.setCellMeta(index, 4, 'valid', false);
                hotInstance.setCellMeta(index, 4, 'className', 'cell-invalid');
            } else {
                hotInstance.setCellMeta(index, 4, 'valid', true);
                hotInstance.setCellMeta(index, 4, 'className', '');
            }
        });
    }

    hotInstance.render(); // Refresca la tabla para aplicar los cambios
}


function afterChangeValidateEmpty(hotInstance, changes, source) {
    if (!["edit", "paste", "autofill"].includes(source) || !changes) {
        return; // Solo procesar si el cambio proviene de edici�n, pegado o autofill
    }

    changes.forEach(([row, col, oldValue, newValue]) => {
        const cellMeta = hotInstance.getCellMeta(row, col);

        // Si la celda queda vac�a, marcarla como v�lida y limpiar estilos
        if (newValue === "" || newValue === null) {
            cellMeta.valid = true;
            hotInstance.setCellMeta(row, col, "className", ""); // Limpiar clases previas
            hotInstance.setCellMeta(row, col, "style", { backgroundColor: "" }); // Restaurar fondo
            cambiosRealizados = true;
        }
    });

    hotInstance.render(); // Refrescar la tabla para aplicar los cambios visuales
}

let mostNotIsShow = false;
function mostrarNotificacion(mensaje, color = 'rgba(255, 0, 0, 0.8)') {
    if (mostNotIsShow) return;
    mostNotIsShow = true;
    const notificacion = document.createElement('span');
    notificacion.className = 'notificacion';
    notificacion.textContent = mensaje;

    notificacion.style.position = 'fixed';
    notificacion.style.top = '50%';
    notificacion.style.left = '50%';
    notificacion.style.transform = 'translate(-50%, -50%)';
    notificacion.style.backgroundColor = color; // Usar el color proporcionado o el predeterminado
    notificacion.style.color = '#fff';
    notificacion.style.fontWeight = 'bold';
    notificacion.style.padding = '15px 20px';
    notificacion.style.borderRadius = '6px';
    notificacion.style.zIndex = '99999';
    notificacion.style.boxShadow = '0px 4px 8px rgba(0, 0, 0, 0.1)';
    notificacion.style.fontFamily = 'Arial, sans-serif';
    notificacion.style.fontSize = '11px';
    notificacion.style.textAlign = 'justify';
    notificacion.style.maxWidth = '10%';

    document.body.appendChild(notificacion);

    setTimeout(function () {
        document.body.removeChild(notificacion);
        mostNotIsShow = false;
    }, 3000);
}

var archivosCargadosPorTabla = {};

function crearUploaderGeneral(
    buttonId,
    tableSelector,
    sectionName,
    descripcion,
    ruta_interna = null
 
) {

    var filters;
    var ordena_ruta = ruta_interna;

    if (buttonId.startsWith('btnSubirUbicacion')) {
        filters = {
            max_file_size: '1024mb',
            mime_types: [
                { title: "Documentos", extensions: "kml,kmz" }
            ]
        };
    }
    else if (buttonId.startsWith('btnPDFoDWG')) {
        filters = {
            max_file_size: '1024mb',
            mime_types: [
                { title: "Documentos", extensions: "PDF,DWG" }
            ]
        };
    } else if (buttonId.startsWith('btnPDFoXLS')) {
        filters = {
            max_file_size: '1024mb',
            mime_types: [
                { title: "Documentos", extensions: "PFD,ZIP" }
            ]
        };
    }
    else {
        filters = {
            max_file_size: '1024mb',
            mime_types: [
                { title: "Imagenes", extensions: "jpg,jpeg,gif,png" },
                { title: "Archivos comprimidos", extensions: "zip,rar" },
                { title: "Documentos", extensions: "pdf,doc,docx,xls,xlsx,ppt,pptx,csv,msg,dwg" }
            ]
        };
    }

    if (!archivosCargadosPorTabla[tableSelector]) {
        archivosCargadosPorTabla[tableSelector] = [];
    }
    var uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: buttonId,
        url: controladorFichas + 'UploadArchivoGeneral',
        flash_swf_url: 'Scripts/Moxie.swf',
        silverlight_xap_url: 'Scripts/Moxie.xap',
        multi_selection: false,
        filters: filters,
        multipart_params: { ordena_ruta: ordena_ruta },
        init: {
            FilesAdded: function (up, files) {
                if (descripcion) {
                    var desc = $("#txtDescripcionAdicional").val();
                    if (!desc) {
                        mostrarNotificacion("Por favor complete el campo de descripción antes de subir el archivo.");
                        up.removeFile(files[0]);
                        up.refresh();
                        return;
                    }
                }

                var fileName = files[0].name;
                var fileExtension = fileName.split('.').pop();

                // if (archivosCargadosPorTabla[tableSelector].some(function (archivo) {
                //     return archivo.nombre === fileName && archivo.extension === fileExtension;
                // })) {
                //     mostrarNotificacion("El archivo '" + fileName + "' ya fue subido anteriormente en esta tabla.");
                //     up.removeFile(files[0]);
                //     up.refresh();
                //     return;
                // }

                param = {};
                param.SeccCodi = sectionName;
                param.ProyCodi = $("#txtPoyCodi").val();
                param.ArchNombre = fileName;
                param.ARCHUBICACION = ordena_ruta;


                $.ajax({
                    type: 'POST',
                    url: controladorFichas + 'ValidarArchivo',
                    data: param,
                    success: function (result) {
                        if (result.responseResult.length > 0) {
                            mostrarNotificacion("El archivo '" + fileName + "' ya fue subido anteriormente en esta tabla.");
                            up.removeFile(files[0]);
                            up.refresh();
                            return;
                        } else {
                            if (files.length > 1) {
                                mostrarNotificacion("Solo puedes subir un archivo a la vez.");
                                up.splice(1, files.length - 1);
                            }
                            up.start();
                            up.refresh();
                        }
                    },
                    error: function () {
                        mostrarNotificacion("El archivo '" + fileName + "' ya fue subido anteriormente en esta tabla.");
                    }
                });
            },
            UploadProgress: function (up, file) {
                $("#barraProgreso").css("width", file.percent + "%").text(file.percent + "%");
                mostrarMensaje('mensajeFicha', 'alert', "Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            FileUploaded: function (up, file, response) {
                var json = JSON.parse(response.response);
                if (json.indicador == 1) {
                    up.removeFile(file);

                    archivosCargadosPorTabla[tableSelector].push({
                        nombre: file.name,
                        extension: file.name.split('.').pop()
                    });
                    if (descripcion) {
                        procesarArchivoGeneralAdi(json.fileNameNotPath, json.nombreReal, json.extension, tableSelector, sectionName, ordena_ruta);
                    } else {

                        procesarArchivoGeneral(json.fileNameNotPath, json.nombreReal, json.extension, tableSelector, sectionName, ordena_ruta);
                    }

                    mostrarMensaje('mensajeFicha', 'exito', "Archivo subido correctamente.");
                } else {
                    mostrarMensaje('mensajeFicha', 'error', "Error al procesar el archivo.");
                }
            },
            UploadComplete: function (up, file) {
                mostrarMensaje('mensajeFicha', 'alert', "Subida completada. <strong>Por favor espere...</strong>");
            },
            Error: function (up, err) {
                switch (err.code) {
                    case plupload.FILE_SIZE_ERROR:
                        mostrarNotificacion("El archivo excede el tamaño máximo permitido de " + filters.max_file_size);
                        break;
                    case plupload.FILE_EXTENSION_ERROR:
                        mostrarNotificacion("El tipo de archivo no es compatible. Solo se permiten: " + filters.mime_types.map(m => m.extensions).join(', '));
                        break;
                    default:
                        mostrarNotificacion("Error" + err.code + ": " + err.message);
                }
            }
        }
    });

    uploader.init();
}

function procesarArchivoGeneral(fileNameNotPath, nombreReal, tipo, tabla, seccionCodigo, ordena_ruta = null) {
 
    param = {};
    param.SeccCodi = seccionCodigo;
    param.ProyCodi = $("#txtPoyCodi").val();
    param.ArchNombre = nombreReal;
    param.ArchNombreGenerado = fileNameNotPath;
    param.ArchTipo = tipo;
    param.ARCHUBICACION = ordena_ruta;
    $.ajax({
        type: 'POST',
        url: controladorFichas + 'GrabarRegistroArchivo',
        data: param,
        success: function (result) {
            if (result.responseResult > 0) {
                mostrarMensaje('mensajeFicha', 'exito', 'Archivo registrado correctamente.');
                console.log("Id Archivo: Valida" + result.id)
                agregarFilaTablaGeneral(nombreReal, result.id, tabla, fileNameNotPath);
            }
            else {
                mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error.');
        }
    });
}

function agregarFilaTablaGeneral(nombre, id, tabla, archivoNombre) {
    var nuevaFila = '<tr id="fila' + id + '">' +
        '<td><a href="#" onclick="descargarFileCampania(\'' + archivoNombre + '\')">' + nombre + '</a></td>' +
        '<td></td>' +
        '</tr>';
    var tabla = $(tabla);
    tabla.append(nuevaFila);
    console.log("agrega archivo**")
}

function eliminarFile(id) {
    document.getElementById("contenidoPopup").innerHTML = '¿Estás seguro de realizar esta operación?';
    $('#popupProyectoGeneral').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
    $('#btnConfirmarPopup').off('click').on('click', function() {
        $.ajax({
            type: 'POST',
            url: controladorFichas + 'EliminarFile',
            data: {
                id: id,
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    $("#fila" + id).remove();
                    mostrarMensaje('mensajeFicha', 'exito', 'El archivo se eliminó correctamente.');
                }
                else {
                    mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error.');
            }
        });
        popupClose('popupProyectoGeneral');
    });
}

function aplicarFondoGris(ws, columnasCalculadas, hotInstance, headerRows = 3) {
    for (let r = headerRows; r < hotInstance.countRows() + headerRows; r++) {
        columnasCalculadas.forEach(columna => {
            let cellAddress = XLSX.utils.encode_cell({ r: r, c: columna });
            if (!ws[cellAddress]) return;
            if (!ws[cellAddress].s) ws[cellAddress].s = {};
            ws[cellAddress].s.fill = { fgColor: { rgb: "D3D3D3" } }; // Fondo gris
        });
    }
}


function exportExcelGInvEstimada(hot, headers, fileName) {
    var data = headers.concat(hot.getData());
    var ws = XLSX.utils.aoa_to_sheet(data);
    aplicarBordes(ws);
    var startRow = 1;
    var startCol = 1;

    for (var R = startRow; R < data.length; R++) {
        for (var C = startCol; C < data[R].length; C++) {
            var cellAddress = XLSX.utils.encode_cell({ r: R, c: C });
            var cell = ws[cellAddress];
            if (cell && typeof cell.v === 'number') {
                cell.z = '0.0000';
            }
        }
    }

    var wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "Datos");

    XLSX.writeFile(wb, fileName);
}

function importExcelGInvEstimada(hot, updateFunction) {
    const currentData = hot.getData();
    var fileInput = document.createElement("input");
    fileInput.type = "file";
    fileInput.accept = ".xlsx, .xls";
    fileInput.onchange = function (e) {
        var file = e.target.files[0];
        var reader = new FileReader();

        reader.onload = function (e) {
            var data = new Uint8Array(e.target.result);
            var workbook = XLSX.read(data, { type: "array" });
            var firstSheet = workbook.Sheets[workbook.SheetNames[0]];
            var jsonData = XLSX.utils.sheet_to_json(firstSheet, { header: 1 });
            const dataProcesada = jsonData.map((row, index) => {
                const firstColumn = currentData[index - 1]?.[0] || '';
                const value = parseFloat(row[1]);
                if (isNaN(value) || !Number.isFinite(value)) {
                    row[1] = "";
                }
                return [firstColumn, ...row.slice(1)];
            });
            updateFunction(dataProcesada);
        };

        reader.readAsArrayBuffer(file);
    };

    fileInput.click();
}

function exportExcelGSein(hot, fileName) {
    var headers = [
        [
            "",
            "Demanda",
            "Demanda",
            "Demanda",
            "Demanda",
            "Generación",
            "Generación",
            "Generación",
            "Demanda Neta = Demanda - Generación",
            "Demanda Neta = Demanda - Generación",
            "Demanda Neta = Demanda - Generación"
        ],
        [
            "",
            "",
            "POTENCIA (MW)",
            "POTENCIA (MW)",
            "",
            "",
            "POTENCIA (MW)",
            "POTENCIA (MW)",
            "",
            "POTENCIA (MW)",
            "POTENCIA (MW)"
        ],
        [
            "Año",
            "ENERGIA (GWh)",
            "HP",
            "HFP",
            "FACTOR DE CARGA (%)",
            "ENERGIA (GWh)",
            "HP",
            "HFP",
            "ENERGIA (GWh)",
            "HP",
            "HFP"
        ]
    ];

    var datosArray = [];
    for (let r = 0; r < hot.countRows(); r++) {
        let rowData = hot.getDataAtRow(r);
        let energiaDemanda = parseFloat(rowData[1]) || 0;
        let energiaGeneracion = parseFloat(rowData[5]) || 0;
        let demandaNeta = energiaDemanda - energiaGeneracion;
        rowData[8] = parseFloat(demandaNeta.toFixed(4));
        rowData[9] = rowData[9] !== null && rowData[9] !== undefined ? rowData[9] : 0;
        rowData[10] = rowData[10] !== null && rowData[10] !== undefined ? rowData[10] : 0;

        datosArray.push(rowData);
    }

    var exportData = headers.concat(datosArray);
    var ws = XLSX.utils.aoa_to_sheet(exportData);

    // Definir merges
    ws["!merges"] = [
        { s: { r: 0, c: 1 }, e: { r: 0, c: 4 } },
        { s: { r: 0, c: 5 }, e: { r: 0, c: 7 } },
        { s: { r: 0, c: 8 }, e: { r: 0, c: 10 } },
        { s: { r: 1, c: 2 }, e: { r: 1, c: 3 } },
        { s: { r: 1, c: 6 }, e: { r: 1, c: 7 } },
        { s: { r: 1, c: 9 }, e: { r: 1, c: 10 } }
    ];

    // Ajustar anchos de las columnas
    const columnWidths = [
        10, 20, 15, 15, 25, 20, 15, 15, 30, 15, 15
    ];
    ws["!cols"] = columnWidths.map(width => ({ wch: width }));

    // Aplicar estilos a las cabeceras
    const headerStyle = {
        fill: { fgColor: { rgb: "ADD8E6" } },
        font: { bold: true },
        alignment: { horizontal: "center", vertical: "center" },
        border: {
            top: { style: "thin", color: { rgb: "000000" } },
            bottom: { style: "thin", color: { rgb: "000000" } },
            left: { style: "thin", color: { rgb: "000000" } },
            right: { style: "thin", color: { rgb: "000000" } }
        }
    };

    for (let R = 0; R < headers.length; R++) {
        for (let C = 0; C < headers[R].length; C++) {
            const cellAddress = XLSX.utils.encode_cell({ r: R, c: C });
            if (!ws[cellAddress]) ws[cellAddress] = { v: headers[R][C] || "" }; // Crear celda si no existe
            ws[cellAddress].s = headerStyle;
        }
    }
    aplicarFondoGris(ws, [4,8], hot, headers.length);

    var wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "Datos");
    XLSX.writeFile(wb, fileName + ".xlsx");
}

function importGSein(hot, updateFunction) {
    const currentData = hot.getData();
    var fileInput = document.createElement("input");
    fileInput.type = "file";
    fileInput.accept = ".xlsx, .xls";
    fileInput.onchange = function (e) {
        var file = e.target.files[0];
        var reader = new FileReader();

        reader.onload = function (e) {
            var data = new Uint8Array(e.target.result);
            var workbook = XLSX.read(data, { type: "array" });
            var firstSheet = workbook.Sheets[workbook.SheetNames[0]];
            var jsonD = XLSX.utils.sheet_to_json(firstSheet, { header: 1 });
            var jsonData = jsonD.map((row, index) => {
                if (index > 2) {
                    const firstColumn = currentData[index - 3]?.[0] || '';
                    return [firstColumn, ...row.slice(1)];
                } else {
                    return row;
                }
            });
            if (jsonData.length > 1) {
                let invalidFactorsFound = false;
                for (let i = 3; i < jsonData.length; i++) {
                    const rowData = jsonData[i];
                    if (rowData && rowData.length >= 4) {
                        for (let j = 1; j < rowData.length; j++) {
                            const value = parseFloat(rowData[j]);
                            if (isNaN(value) || !Number.isFinite(value)) {
                                rowData[j] = "";
                            }
                        }
                        const year = parseInt(rowData[0]);
                        const energia = parseFloat(rowData[1]);
                        const potenciaHP = parseFloat(rowData[2]);
                        const potenciaHFP = parseFloat(rowData[3]);

                        const factorCarga = calculateLoadFactor(energia, potenciaHP, potenciaHFP, year);
                        rowData[4] = factorCarga;
                        if (factorCarga < 0 || factorCarga > 1) {
                            invalidFactorsFound = true;
                        }
                    }
                }
                if (invalidFactorsFound) {
                    mostrarNotificacion("Se encontraron Factores de Carga fuera del rango permitido (0-1) en los datos importados. Por favor, revise los valores.");
                }
            }
            updateFunction(jsonData);
        };
        reader.readAsArrayBuffer(file);
    };

    fileInput.click();
}

function exportarTablaResumenSitProy(hotTable, nombreArchivo = "ResumenSitProy_Datos", nombreHoja = "ResumenSitProy") {

    var headers = [
        ["", "Estado Situacional"],
        ["Requisitos", "En Elaboración", "Presentado", "En Trámite (evaluación)", "Aprobado/ autorizado", "Firmado"]
    ];

    var data = hotTable.getData();
    var transformedData = data.map(row => row.slice(1).map(cell => (cell === true || cell === 'true') ? 1 : ((cell === false || cell === 'false') ? 0 : cell)));
    var dataWithHeaders = headers.concat(transformedData);
    var ws = XLSX.utils.aoa_to_sheet(dataWithHeaders);
    ws["!merges"] = [
        { s: { r: 0, c: 1 }, e: { r: 0, c: 5 } },
    ];
    var wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, nombreHoja);
    aplicarBordes(ws);
    XLSX.writeFile(wb, `${nombreArchivo}.xlsx`);
}

function importarTablaResumenSitProy(hotTable) {
    var fileInput = document.createElement("input");
    fileInput.type = "file";
    fileInput.accept = ".xlsx, .xls";

    fileInput.onchange = function (e) {
        var file = e.target.files[0];
        var reader = new FileReader();

        reader.onload = function (e) {
            var data = new Uint8Array(e.target.result);
            var workbook = XLSX.read(data, { type: "array" });
            var firstSheet = workbook.Sheets[workbook.SheetNames[0]];
            var jsonData = XLSX.utils.sheet_to_json(firstSheet, { header: 1 });
            updateTableFromExcelResumenSitProy(jsonData, hotTable);
            cambiosRealizados = true;
        };

        reader.readAsArrayBuffer(file);
    };
    fileInput.click();
}

function updateTableFromExcelResumenSitProy(jsonData, hotTable) {
    const currentData = hotTable.getData();
    const numColumns = currentData[0]?.length || 0;
    var headers = jsonData.slice(0, 2);
    var data = jsonData.slice(2);

    var reorganizedData = data.map((row, index) => {
        var firstColumn = currentData[index]?.[0] || '';
        var secondColumn = currentData[index]?.[1] || '';
        return [firstColumn, secondColumn, ...row.slice(1)];
    });
    reorganizedData = reorganizedData.map(row => {
        if (row.length < numColumns) {
            return [...row, ...Array(numColumns - row.length).fill(null)];
        } else if (row.length > numColumns) {
            return row.slice(0, numColumns);
        }
        return row;
    });

    var transformedData = reorganizedData.map(row => {
        return row.map((cell, cellIndex) => {
            if (cellIndex === 0 || cellIndex === 1) {
                return cell;
            }
            return cell === 1 ? true : (cell === 0 ? false : false);
        });
    });
    // Cargar los datos en la tabla Handsontable
    hotTable.loadData(transformedData);
}

function procesarArchivoGeneralAdi(fileNameNotPath, nombreReal, tipo, tabla, seccionCodigo, ordena_ruta) {
    param = {};
    param.SeccCodi = seccionCodigo;
    param.ProyCodi = $("#txtPoyCodi").val();
    param.ArchNombre = nombreReal;
    param.ArchNombreGenerado = fileNameNotPath;
    param.ArchTipo = tipo;
    param.Descripcion = $("#txtDescripcionAdicional").val();
    param.ARCHUBICACION = ordena_ruta;
    $.ajax({
        type: 'POST',
        url: controladorFichas + 'GrabarRegistroArchivo',
        data: param,
        success: function (result) {
            if (result.responseResult > 0) {
                mostrarMensaje('mensajeFicha', 'exito', 'Archivo registrado correctamente.');
                console.log("Id Archivo:" + result.id);
                agregarFilaTablaAdi(nombreReal, result.id, tabla, result.descripcion, fileNameNotPath);
            }
            else {
                mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error.');
        }
    });
}

function agregarFilaTablaAdi(nombre, id, tabla, descripcion, archivoNombre) {
    var nuevaFila = '<tr id="fila' + id + '">' +
        '<td>' + (descripcion !== null ? descripcion : "") + '</td>' +
        '<td><a href="#" onclick="descargarFileCampania(\'' + archivoNombre + '\')">' + nombre + '</a></td>' +
        '<td></td>' +
        '</tr>';
    var tabla = $(tabla);
    tabla.append(nuevaFila);
    $("#txtDescripcionAdicional").val("");
}


function FormatDisabledRenderer(instance, td, row) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.background = '#f7f7f7';
}
