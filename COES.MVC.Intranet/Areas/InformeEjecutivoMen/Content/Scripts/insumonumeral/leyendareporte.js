var siteRootExtranet = "https://www.coes.org.pe/Extranet/";
var siteRootPortal = "https://www.coes.org.pe/";

$(function () {
    mostrarLeyendaReporte();
});


function mostrarLeyendaReporte() {
    var idnumeral = parseInt($("#hdIdNumeral").val()) || 0;
    //var tiporeporte = parseInt($("#hdTipoReporte").val()) || 0;

    var htmlTexto = "";

    htmlTexto = getTextoLeyendaInfEjecMensual(idnumeral);
    //switch (tiporeporte) {
    //    case 1:////Informe Mensual
    //        htmlTexto = getTextoLeyendaInfMensual(idnumeral);
    //        break;
    //    case 2://Informe Anual
    //        htmlTexto = getTextoLeyendaInfAnual(idnumeral);
    //        break;        
    //}
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

function getTextoLeyendaInfEjecMensual(id_numeral) {
    var htmlAplicativo = '';
    var urlAplicativo = '';
    var flag = 0; // 0: intranet, 1: extranet, 2: Portal

    switch (id_numeral) {
        case 219: //1.1. Produccion por empresa generadora
            htmlAplicativo = 'Medidores de Generación';
            break;
        case 220://1.2. Producción total de centrales de generación eléctrica con exportación a ecuador
            htmlAplicativo = 'Medidores de Generación';
            break;
        case 221://1.3. Participación por empresas en la producción total de energía del mes
            htmlAplicativo = 'Medidores de Generación';
            break;
        case 222://1.4. Evolución del crecimiento mensual de la máxima potencia coincidente sin exportación a ecuador
            htmlAplicativo = 'Medidores de Generación';
            break;
        case 223://1.5. Comparación de la cobertura de la máxima demanda por tipo de generación
            htmlAplicativo = 'Medidores de Generación';
            break;
        case 224://1.6. Despacho en el día de máxima potencia coincidente
            htmlAplicativo = 'Medidores de Generación';
            break;
        case 225://1.7. Cobertura de la máxima potencia coincidente por tipo de tecnología
            htmlAplicativo = 'Medidores de Generación';
            break;
        case 226://1.8. Utilización de los recursos energéticos
            htmlAplicativo = 'Medidores de Generación';
            break;
        case 227: //1.9. Participación de la utilización de los recursos energéticos en la producción de energía eléctrica
            htmlAplicativo = 'Medidores de Generación';
            break;
        case 229: //2.1 DEMANDA DE ENERGÍA ZONA NORTE 
            htmlAplicativo = 'Valorización de Transferencias de Energía Activa';
            break;
        case 230: //2.2 DEMANDA DE ENERGÍA ZONA CENTRO
            htmlAplicativo = 'Valorización de Transferencias de Energía Activa';
            break;
        case 231: //2.3 DEMANDA DE ENERGÍA ZONA SUR
            htmlAplicativo = 'Valorización de Transferencias de Energía Activa';
            break;
        case 233: //3.1. VOLÚMEN UTIL DE LOS EMBALSES Y LAGUNAS (Millones de m3)
            htmlAplicativo = 'Hidrología';
            break;
        case 234://3.2 PROMEDIO MENSUAL DE LOS CAUDALES (m3/s)
            htmlAplicativo = 'Hidrología';
            break;
        case 346://3.3
            htmlAplicativo = 'Hidrología';
            break;
        case 347://3.4
            htmlAplicativo = 'Hidrología';
            break;
        case 236: //4. INTERCONEXIONES
            htmlAplicativo = 'Interconexiones';
            break;
        case 238: //5. HORAS CONGESTION EN LOS PRINCIPALES EQUIPOS DE TRANSMISIÓN
            htmlAplicativo = 'Operaciones Varias';
            break;
        case 240: //6.1. EVOLUCIÓN DEL COSTO MARGINAL EN BARRA DE REFERENCIA
            htmlAplicativo = 'Costo Marginal VS Tarifa en Barra';
            break;
        case 241: //6.2. COSTOS MARGINALES DE LOS PRINCIPALES MODOS DE OPERACIÓN
            htmlAplicativo = 'Costos Marginales Revisados – Costos Variables';
            break;
        case 242: //6.3.  COSTOS MARGINALES EN LAS PRINCIPALES BARRAS DEL SEIN (US$/MWh)
            htmlAplicativo = 'Valorización de Transferencias de Energía Activa';
            break;
        case 244: //7.1.  Mantenimientos Ejecutados
            htmlAplicativo = 'Intervenciones';
            break;
        case 246: //8.1. TRANSFERENCIAS DE ENERGIA ACTIVA
            htmlAplicativo = 'Valorización de Transferencias de Energía Activa';
            break;
        case 247: //8.2. TRANSFERENCIAS DE POTENCIA
            htmlAplicativo = 'Valorización de Transferencias de Potencia';
            break;
        case 248: //8.3.  Valorizacion de las transferencias de Potencia (Soles)
            htmlAplicativo = 'Valorización de Transferencias de Potencia';
            break;
        case 249: //8.4.  Potencia Firme por Empresas (MW)
            htmlAplicativo = 'Potencia Firme Remunerable';
            break;
        case 251: //9.1 Compensacion a Transmisoras por Peaje de conexion y transmision, Sistema principal y Sistema Garantizado de Transmision
            htmlAplicativo = 'Valorización de Transferencias de Potencia y Peajes';
            break;       
        case 252: //9.2 Porcentaje de Compensacion a Transmisoras por Peaje de conexion y transmision
            htmlAplicativo = 'Valorización de Transferencias de Potencia y Peajes';
            break;
        case 253: //9.3 Compensacion a Transmisoras por Ingreso tarifario del Sistema principal y Sistema Garantizado de Transmision
            htmlAplicativo = 'Valorización de Transferencias de Potencia y Peajes';
            break;
        case 254: // 9.4 Porcentaje de Compensacion por Ingreso y tarifario
            htmlAplicativo = 'Valorización de Transferencias de Potencia y Peajes';
            break;
        case 256: //10. Eventos y Fallas que ocacionaron interrupcion
            htmlAplicativo = 'Eventos';
            break;
        case 258: //11. Fallas por tipo de equipo y causa segun clasificacion
            htmlAplicativo = 'Eventos';
            break;
        case 269: //11.2. Energía interrumpida (MWh) por fallas en las diferentes zonas del sistema eléctrico.
            htmlAplicativo = 'Eventos';
            break;
        case 271: //12.1. EVOLUCIÓN DE INTEGRANTES DEL COES
            htmlAplicativo = 'Medidores de Generación';
            break;
        case 272: //12.2. INGRESO DE EMPRESAS INTEGRANTES AL COES
            htmlAplicativo = 'Medidores de Generación';
            break;
        case 273: //12.3. RETIRO DE EMPRESAS INTEGRANTES DEL COES
            htmlAplicativo = 'Medidores de Generación';
            break;
        case 274: //12.4.CAMBIO DE DENOMINACIÓN Y FUSIÓN DE EMPRESAS INTEGRANTES DEL COES
            htmlAplicativo = 'Medidores de Generación';
            break;
    }

    switch (id_numeral) {
        case 219: //1.1. Produccion por empresa generadora
            urlAplicativo = 'mediciones/reportemedidores/index/';
            break;
        case 220://1.2. Producción total de centrales de generación eléctrica con exportación a ecuador
            urlAplicativo = 'mediciones/reportemedidores/index/';
            break;
        case 221://1.3. Participación por empresas en la producción total de energía del mes
            urlAplicativo = 'mediciones/reportemedidores/index/';
            break;
        case 222://1.4. Evolución del crecimiento mensual de la máxima potencia coincidente sin exportación a ecuador
            urlAplicativo = 'mediciones/reportemedidores/index/';
            break;
        case 223://1.5. Comparación de la cobertura de la máxima demanda por tipo de generación
            urlAplicativo = 'mediciones/reportemedidores/index/';
            break;
        case 224://1.6. Despacho en el día de máxima potencia coincidente
            urlAplicativo = 'mediciones/reportemedidores/index/';
            break;
        case 225://1.7. Cobertura de la máxima potencia coincidente por tipo de tecnología
            urlAplicativo = 'mediciones/reportemedidores/index/';
            break;
        case 226://1.8. Utilización de los recursos energéticos
            urlAplicativo = 'mediciones/reportemedidores/index/';
            break;
        case 227: //1.9. Participación de la utilización de los recursos energéticos en la producción de energía eléctrica
            urlAplicativo = 'mediciones/reportemedidores/index/';
            break;
        case 229: //2.1 DEMANDA DE ENERGÍA ZONA NORTE 
            urlAplicativo = 'transferencias/valortransferencia/index';
            break;
        case 230: //2.2 DEMANDA DE ENERGÍA ZONA CENTRO
            urlAplicativo = 'transferencias/valortransferencia/index';
            break;
        case 231: //2.3 DEMANDA DE ENERGÍA ZONA SUR
            urlAplicativo = 'transferencias/valortransferencia/index';
            break;
        case 233: //3.1. VOLÚMEN UTIL DE LOS EMBALSES Y LAGUNAS (Millones de m3)
            urlAplicativo = 'Hidrologia/Reporte/Index/';
            break;
        case 234://3.2 PROMEDIO MENSUAL DE LOS CAUDALES (m3/s)
            urlAplicativo = 'Hidrologia/Reporte/Index/';
            break;
        case 346://3.3 EVOLUCION DE LOS VOLUMENES
            urlAplicativo = 'Hidrologia/Reporte/Index/';
            break;
        case 347://3.4 EVOLUCION DE LOS CAUDALES
            urlAplicativo = 'Hidrologia/Reporte/Index/';
            break;
        case 236: //4. INTERCONEXIONES
            urlAplicativo = 'ieod/cargadatos/interconexion/';
            break;
        case 238: //5. HORAS CONGESTION EN LOS PRINCIPALES EQUIPOS DE TRANSMISIÓN
            urlAplicativo = 'eventos/operacionesvarias/index/';
            break;
        case 240: //6.1. EVOLUCIÓN DEL COSTO MARGINAL EN BARRA DE REFERENCIA
            urlAplicativo = 'web/cmvstarifa/index';
            break;
        case 241: //6.2. COSTOS MARGINALES DE LOS PRINCIPALES MODOS DE OPERACIÓN
            urlAplicativo = 'siosein/Numerales/ViewCostoMarginalCP/';
            break;
        case 242: //6.3.  COSTOS MARGINALES EN LAS PRINCIPALES BARRAS DEL SEIN (US$/MWh)
            urlAplicativo = 'transferencias/factorperdida/index';
            break;
        case 244: //7.1.  Mantenimientos Ejecutados
            urlAplicativo = 'Intervenciones/Registro/Programaciones/';
            break;
        case 246: //8.1. TRANSFERENCIAS DE ENERGIA ACTIVA
            urlAplicativo = 'transferencias/valortransferencia/index#paso4';
            break;
        case 247: //8.2. TRANSFERENCIAS DE POTENCIA
            urlAplicativo = 'transfpotencia/valortransfpc/index';
            break;
        case 248: //8.3.  Valorizacion de las transferencias de Potencia (Soles)
            urlAplicativo = 'transfpotencia/valortransfpc/index';
            break;
        case 249: //8.4.  Potencia Firme por Empresas (MW)
            urlAplicativo = 'potenciafirmeremunerable/general/index';
            break;
        case 251: //9.1 Compensacion a Transmisoras por Peaje de conexion y transmision, Sistema principal y Sistema Garantizado de Transmision
            urlAplicativo = 'transfpotencia/valortransfpc/index';
            break;
        case 252: //9.2 Porcentaje de Compensacion a Transmisoras por Peaje de conexion y transmision
            urlAplicativo = 'transfpotencia/valortransfpc/index';
            break;
        case 253: //9.3 Compensacion a Transmisoras por Ingreso tarifario del Sistema principal y Sistema Garantizado de Transmision
            urlAplicativo = 'transfpotencia/valortransfpc/index';
            break;
        case 254: // 9.4 Porcentaje de Compensacion por Ingreso y tarifario
            urlAplicativo = 'transfpotencia/valortransfpc/index';
            break;
        case 256: //10. Eventos y Fallas que ocacionaron interrupcion
            urlAplicativo = 'eventos/evento/index/';
            break;
        case 258: //11. Fallas por tipo de equipo y causa segun clasificacion
            urlAplicativo = 'eventos/evento/index/';
            break;
        case 269: //11.2. Energía interrumpida (MWh) por fallas en las diferentes zonas del sistema eléctrico.
            urlAplicativo = 'eventos/evento/index/';
            break;
        case 271: //12.1. EVOLUCIÓN DE INTEGRANTES DEL COES
            urlAplicativo = 'mediciones/reportemedidores/index/';
            break;
        case 272: //12.2. INGRESO DE EMPRESAS INTEGRANTES AL COES
            urlAplicativo = 'mediciones/reportemedidores/index/';
            break;
        case 273: //12.3. RETIRO DE EMPRESAS INTEGRANTES DEL COES
            urlAplicativo = 'mediciones/reportemedidores/index/';
            break;
        case 274: //12.4.CAMBIO DE DENOMINACIÓN Y FUSIÓN DE EMPRESAS INTEGRANTES DEL COES
            urlAplicativo = 'mediciones/reportemedidores/index/';
            break;
    }
    
    var htmlTexto = '';
    htmlTexto += `
        Los datos son obtenidos del aplicativo <b><span style="color: blue;">${htmlAplicativo}<span></b>. <input type="button" value="Ir a aplicativo" onclick="abrirVentanaAplicativo('${urlAplicativo}', ${flag});">
    `;    
    return htmlTexto;
}



//#endregion
