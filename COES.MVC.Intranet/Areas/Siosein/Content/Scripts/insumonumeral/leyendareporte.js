var siteRootExtranet = "https://www.coes.org.pe/Extranet/";
var siteRootPortal = "https://www.coes.org.pe/";

$(function () {
    mostrarLeyendaReporte();
});


function mostrarLeyendaReporte() {
    var idnumeral = parseInt($("#hdIdNumeral").val()) || 0;
    var tiporeporte = parseInt($("#hdTipoReporte").val()) || 0;

    var htmlTexto = "";


    switch (tiporeporte) {
        case 1:////Informe Mensual
            htmlTexto = getTextoLeyendaInfMensual(idnumeral);
            break;
        case 2://Informe Anual
            htmlTexto = getTextoLeyendaInfAnual(idnumeral);
            break;        
    }
    $("#leyendaFuenteDatos").html(htmlTexto);

    $("#leyendaFuenteDatos").show();
}

function abrirVentanaAplicativo(urlAplicativo, flag) {
    var url = "";
    switch (flag) {
        case 0://	ENLACE INTRANET
            url = siteRoot + urlAplicativo;
            break;
        case 1://	ENLACE EXTRANET
            url = siteRootExtranet + urlAplicativo;
            break;
        case 2://	ENLACE PORTAL
            url =  siteRootPortal + urlAplicativo;
            break;

    }    
    window.open(url, '_blank').focus();
}

// #region Leyenda Formato txt

function getTextoLeyendaInfMensual(id_numeral) {
    var htmlAplicativo = '';
    var urlAplicativo = '';
    var flag = 0; // 0: intranet, 1: extranet, 2: Portal

    switch (id_numeral) {
        case 305: //1. Reporte Resumen relevante
            htmlAplicativo = 'Medidores de Generación';
            break;
        case 306: //1.1. Ingreso en Operación Comercial al SEIN
            htmlAplicativo = 'Equipamiento';
            break;
        case 307://1.2. Retiro de Operación Comercial del SEIN
            htmlAplicativo = 'Equipamiento';
            break;
        case 308://1.3. Potencia Instalada en el SEIN
            htmlAplicativo = 'Equipamiento';
            break;
        case 309://2.1. Producción por tipo de Generación
            htmlAplicativo = 'Medidores de Generación';
            break;
        case 310://2.2. Producción por tipo de Recurso Energético
            htmlAplicativo = 'Medidores de Generación';
            break;
        case 311://2.3. Por Producción RER
            htmlAplicativo = 'Medidores de Generación';
            break;
        case 312://2.4. Factor de planta de las centrales RER
            htmlAplicativo = 'Medidores de Generación';
            break;
        case 313: //2.5. Participación de la producción por empresas Integrantes
            htmlAplicativo = 'Medidores de Generación';
            break;
        case 314: //3.1. MÁXIMA DEMANDA COINCIDENTE DE POTENCIA POR TIPO DE GENERACIÓN (MW)
            htmlAplicativo = 'IEOD - Anexo A - Numeral 4';
            break;
        case 315: //3.2. PARTICIPACIÓN DE LAS EMPRESAS INTEGRANTES EN LA MÁXIMA DEMANDA COINCIDENTE (MW)
            hhtmlAplicativo = 'Medidores de Generación';
            break;
        case 316: //4.1. Volumen útil de los embalses y lagunas (Mm3)
            htmlAplicativo = 'Hidrología';
            break;
        case 317://4.2. Evolucion de volumenes de embalses y lagunas (Mm3)
            htmlAplicativo = 'Hidrología';
            break;
        case 318://4.3. Promedio mensual de los caudales (m3/s)
            htmlAplicativo = 'Hidrología';
            break;            
        case 319://4.4.Evolucion mensual de los caudales(m3 / s)
            htmlAplicativo = 'Hidrología';
            break;
        case 320://5.1.  COSTOS MARGINALES EN LAS PRINCIPALES BARRAS DEL SEIN (US$/MWh)
            htmlAplicativo = 'Costos Marginales Revisados';
            break;
        case 322: //6. HORAS DE CONGESTIÓN EN LAS PRINCIPALES EQUIPOS DE TRANSMISIÓN DEL SEIN (Horas)
            htmlAplicativo = 'Operaciones Varias';
            break;
        case 323: //7. EVENTOS Y FALLAS QUE OCASIONARON INTERRUPCIÓN Y DISMINUCIÓN DE SUMINISTRO ELÉCTRICO
            htmlAplicativo = 'Eventos';
            break;
        case 349: //8.1 Producción de Electricidad Mensual por Empresa y Tipo de Generación en el Sein
            htmlAplicativo = 'Medidores de Generación';
            break;
        case 350: //8.2 Máxima Potencia Coincidente Mensual
            htmlAplicativo = 'Medidores de Generación';
            break;
        case 351: //8.3 Listado De Eventos y Fallas que ocasionaron interrupción y disminución de Suministro Eléctrico
            htmlAplicativo = 'Eventos';
            break;        
    }

    switch (id_numeral) {
        case 305: //1. Reporte Resumen relevante
            urlAplicativo = 'Mediciones/MedidoresGeneracion/index/';
            break;
        case 306: //1.1. Ingreso en Operación Comercial al SEIN
            urlAplicativo = 'Equipamiento/Equipo/Index/';
            break;
        case 307://1.2. Retiro de Operación Comercial del SEIN
            urlAplicativo = 'Equipamiento/Equipo/Index/';
            break;
        case 308://1.3. Potencia Instalada en el SEIN
            urlAplicativo = 'Equipamiento/Equipo/Index/';
            break;
        case 309://2.1. Producción por tipo de Generación
            urlAplicativo = 'Mediciones/MedidoresGeneracion/index/';
            break;
        case 310://2.2. Producción por tipo de Recurso Energético
            urlAplicativo = 'Mediciones/MedidoresGeneracion/index/';
            break;
        case 311://2.3. Por Producción RER
            urlAplicativo = 'Mediciones/MedidoresGeneracion/index/';
            break;
        case 312://2.4. Factor de planta de las centrales RER
            urlAplicativo = 'Mediciones/MedidoresGeneracion/index/';
            break;
        case 313: //2.5. Participación de la producción por empresas Integrantes
            urlAplicativo = 'Mediciones/MedidoresGeneracion/index/';
            break;
        case 314: //3.1. MÁXIMA DEMANDA COINCIDENTE DE POTENCIA POR TIPO DE GENERACIÓN (MW)
            urlAplicativo = 'IEOD/AnexoA/MenuAnexoA/';
            break;
        case 315: //3.2. PARTICIPACIÓN DE LAS EMPRESAS INTEGRANTES EN LA MÁXIMA DEMANDA COINCIDENTE (MW)
            urlAplicativo = 'Mediciones/MedidoresGeneracion/index/';
            break;
        case 316: //4.1. Volumen útil de los embalses y lagunas (Mm3)
            urlAplicativo = 'Hidrologia/Reporte/Index/';
            break;
        case 317://4.2. Evolucion de volumenes de embalses y lagunas (Mm3)
            urlAplicativo = 'Hidrologia/Reporte/Index/';
            break;
        case 318://4.3. Promedio mensual de los caudales (m3/s)
            urlAplicativo = 'Hidrologia/Reporte/Index/';
            break;
        case 319://4.4. Evolucion mensual de los caudales (m3/s)
            urlAplicativo = 'Hidrologia/Reporte/Index/';
            break;
        case 320://5.1.  COSTOS MARGINALES EN LAS PRINCIPALES BARRAS DEL SEIN (US$/MWh)
            urlAplicativo = 'siosein/Numerales/ViewCostoMarginalCP/';
            break;
        case 322: //6. HORAS DE CONGESTIÓN EN LAS PRINCIPALES EQUIPOS DE TRANSMISIÓN DEL SEIN (Horas) //6.1 Por Área Operativa
            urlAplicativo = 'Eventos/OperacionesVarias/Index';
            break;
        case 323: //7. EVENTOS Y FALLAS QUE OCASIONARON INTERRUPCIÓN Y DISMINUCIÓN DE SUMINISTRO ELÉCTRICO
            urlAplicativo = 'eventos/evento/index/';
            break;
        case 349: //8.1 Producción de Electricidad Mensual por Empresa y Tipo de Generación en el Sein
            urlAplicativo = 'Mediciones/MedidoresGeneracion/index/';            
            break;
        case 350: //8.2 Máxima Potencia Coincidente Mensual
            urlAplicativo = 'Mediciones/MedidoresGeneracion/index/';            
            break;
        case 351: //8.3 Listado De Eventos y Fallas que ocasionaron interrupción y disminución de Suministro Eléctrico
            urlAplicativo = 'eventos/evento/index/';
            break;        
    }
    
    var htmlTexto = '';
    htmlTexto += `
        Los datos son obtenidos del aplicativo <b><span style="color: blue;">${htmlAplicativo}<span></b>. <input type="button" value="Ir a aplicativo" onclick="abrirVentanaAplicativo('${urlAplicativo}', ${flag});">
    `;    
    return htmlTexto;
}

function getTextoLeyendaInfAnual(id_numeral) {
    //alert(id_numeral);
    var htmlAplicativo = '';
    var urlAplicativo = '';    
    var flag = 0; // 0: intranet, 1: extranet, 2: Portal
    switch (id_numeral) {
        case 324: //1.1 Producción de energía
            htmlAplicativo = 'Medidores de Generación';
            break;

        case 325: //2.1 Ingreso en Operación Comercial
            htmlAplicativo = 'Equipamiento';
            break;

        case 326://2.2. Retiro de Operación Comercial
            htmlAplicativo = 'Equipamiento';
            break;

        case 345://2.3 Potencia instalada en el SEIN
            htmlAplicativo = 'Medidores de Generación';
            break;

        case 328://3.1. Producción por tipo de generación
            htmlAplicativo = 'Medidores de Generación';
            break;

        case 330://3.3. Producción por tipo de Recurso Energético
            htmlAplicativo = 'Medidores de Generación';
            break;

        case 331://3.4. Por Producción RER
            htmlAplicativo = 'Medidores de Generación';
            break;
        case 332://3.5. Participación de la producción por empresas Integrantes
            htmlAplicativo = 'Medidores de Generación';
            break;
        case 344://3.6. Participación de la producción por empresas Integrantes
            htmlAplicativo = 'Medidores de Generación';
            break;
        case 333://4.1. MÁXIMA POTENCIA COINCIDENTE POR TIPO DE GENERACIÓN (MW)
            htmlAplicativo = 'Medidores de Generación';
            break;

        case 334://4.2. PARTICIPACIÓN DE LAS EMPRESAS INTEGRANTES EN LA MÁXIMA POTENCIA COINCIDENTE
            htmlAplicativo = 'Medidores de Generación';
            break;
        case 335://5.1. EVOLUCIÓN DE LOS VOLÚMENES ALMACENADOS
            htmlAplicativo = 'Hidrología';
            break;
        case 336://5.2. EVOLUCIÓN PROMEDIO MENSUAL DE CAUDALES (m3/s)
            htmlAplicativo = 'Hidrología';
            break;
       
        case 337: //6.1 EVOLUCIÓN MENSUAL DE LOS COSTOS MARGINALES PROMEDIO PONDERA DEL SEIN BARRA STA ROSA
            htmlAplicativo = 'Costos Marginales Revisados';
            break;

        case 338: //7.1 POR ÁREA OPERATIVA
            htmlAplicativo = 'Operaciones Varias';
            break;

        case 339: //8.1 Intercambios Internacionales de energía y potencia
            htmlAplicativo = 'Intercambios Internacionales';
            break;

        case 352: //9.1 PRODUCCIÓN DE ELECTRICIDAD ANUAL POR EMPRESA Y TIPO DE GENERACIÓN EN EL SEIN
            htmlAplicativo = 'Medidores de Generación';
            break;
        case 353: //9.2 MÁXIMA POTENCIA COINCIDENTE ANUAL
            htmlAplicativo = 'Medidores de Generación';
            break;
        
    }

    switch (id_numeral) {
        case 324: //1.1 Producción de energía
            urlAplicativo = 'Mediciones/MedidoresGeneracion/index/';
            break;

        case 325: //2.1 Ingreso en Operación Comercial
            urlAplicativo = 'Equipamiento/Equipo/Index/';
            break;

        case 326://2.2. Retiro de Operación Comercial
            urlAplicativo = 'Equipamiento/Equipo/Index/';
            break;

        case 330: //3.3. Producción por tipo de Recurso Energético
            urlAplicativo = 'Mediciones/MedidoresGeneracion/index/';
            break;
            
        case 331://3.4. Por Producción RER
            urlAplicativo = 'Mediciones/MedidoresGeneracion/index/';
            break;

        case 345://2.3 Potencia instalada en el SEIN
            urlAplicativo = 'Mediciones/MedidoresGeneracion/index/';
            break;

        case 328://3.1. Producción por tipo de generación
            urlAplicativo = 'Mediciones/MedidoresGeneracion/index/';
            break;

        case 330://3.3. Producción por tipo de Recurso Energético
            urlAplicativo = 'Mediciones/MedidoresGeneracion/index/';
            break;

        case 331://3.4. Por Producción RER
            urlAplicativo = 'Mediciones/MedidoresGeneracion/index/';
            break;
        case 332://3.5. Participación de la producción por empresas Integrantes
            urlAplicativo = 'Mediciones/MedidoresGeneracion/index/';
            break;
        case 344://3.6. Participación de la producción por empresas Integrantes
            urlAplicativo = 'Mediciones/MedidoresGeneracion/index/';
            break;
        case 333://4.1. MÁXIMA POTENCIA COINCIDENTE POR TIPO DE GENERACIÓN (MW)
            urlAplicativo = 'Mediciones/MedidoresGeneracion/index/';
            break;

        case 334://4.2. PARTICIPACIÓN DE LAS EMPRESAS INTEGRANTES EN LA MÁXIMA POTENCIA COINCIDENTE
            urlAplicativo = 'Mediciones/MedidoresGeneracion/index/';
            break;
        case 335://5.1. EVOLUCIÓN DE LOS VOLÚMENES ALMACENADOS
            urlAplicativo = 'Hidrologia/Reporte/Index/';
            break;
        case 336://5.2. EVOLUCIÓN PROMEDIO MENSUAL DE CAUDALES (m3/s)
            urlAplicativo = 'Hidrologia/Reporte/Index/';
            break;

        case 337: //6.1 EVOLUCIÓN MENSUAL DE LOS COSTOS MARGINALES PROMEDIO PONDERA DEL SEIN BARRA STA ROSA
            urlAplicativo = 'siosein/Numerales/ViewCostoMarginalCP/';
            break;

        case 338: //7.1 POR ÁREA OPERATIVA
            urlAplicativo = 'eventos/operacionesvarias/index/';
            break;

        case 339: //8.1 Intercambios Internacionales de energía y potencia
            urlAplicativo = 'interconexiones/reportes/ReporteInterElect/';
            break;

        case 352: //9.1 PRODUCCIÓN DE ELECTRICIDAD ANUAL POR EMPRESA Y TIPO DE GENERACIÓN EN EL SEIN
            urlAplicativo = 'Mediciones/MedidoresGeneracion/index/';
            break;
        case 353: //9.2 MÁXIMA POTENCIA COINCIDENTE ANUAL
            urlAplicativo = 'Mediciones/MedidoresGeneracion/index/';
            break;
    }
 

    var htmlTexto = '';
    htmlTexto += `
        Los datos son obtenidos del aplicativo <b><span style="color: blue;">${htmlAplicativo}<span></b>. <input type="button" value="Ir a aplicativo" onclick="abrirVentanaAplicativo('${urlAplicativo}', ${flag});">
    `;
    

    return htmlTexto;
}

//#endregion
