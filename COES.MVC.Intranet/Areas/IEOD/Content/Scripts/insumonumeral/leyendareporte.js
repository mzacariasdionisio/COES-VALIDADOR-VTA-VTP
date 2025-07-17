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
        case 1://1	ANEXO A
            htmlTexto = getTextoLeyendaAnexoA(idnumeral);
            break;
        case 2://2	INFORME SEMANAL
            htmlTexto = getTextoLeyendaSemanal(idnumeral);
            break;
        case 3://3	EJECUTIVO SEMANAL
            htmlTexto = getTextoLeyendaEjecutivoSem(idnumeral);
            break;
    }
    $("#leyenda_prie").html(htmlTexto);

    $("#leyenda_prie").show();
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

function getTextoLeyendaAnexoA(id_numeral) {
    var htmlAplicativo = '';
    var urlAplicativo = '';
    var flag = 0; // 0: intranet, 1: extranet, 2: Portal

    switch (id_numeral) {
        case 76: //1. Reporte de Eventos: fallas, interrupciones, restricciones y otros de carácter operativo
            htmlAplicativo = 'Eventos';
            break;
        case 77://2 Reporte de las principales restricciones operativas y mantenimiento de las Unidades de Generación y de los equipos del Sistema de Transmisión
            htmlAplicativo = 'Intervenciones';
            break;
        case 78://3. Reporte de ingreso a operación comercial de unidades o centrales de generación, así como de la conexión e integración al SEIN de instalaciones de transmisión
            htmlAplicativo = 'Equipos';
            break;
        case 4://4. Despacho registrado cada 30 minutos de las Unidades de Generación de los Integrantes del COES, asimismo, se incluye las Unidades de Generación con potencia superior a 5 MW conectadas al SEIN de empresas no Integrantes del COES (MW, MVAr)
            htmlAplicativo = 'CDispatch';
            break;
        case 5://5. Reporte de la demanda por áreas (MW)
            htmlAplicativo = 'CDispatch';
            break;
        case 6://6. Reporte de demanda de Grandes Usuarios (MW)
            htmlAplicativo = 'Reporte Demanda en Barras';
            break;
        case 7://7. Recursos energéticos y diagrama de duración de demanda del SEINL
            htmlAplicativo = 'CDispatch';
            break;
        case 184://8. Evolución de la producción de energía diaria.
            htmlAplicativo = 'CDispatch';
            break;
        case 8: //9. Máxima generación instantánea del SEIN (MW)
            htmlAplicativo = 'CDispatch';
            break;
        case 10: //10. Horas de orden de arranque y parada, así como las horas de ingreso y salida de las Unidades de Generación del SEIN
            htmlAplicativo = 'Horas de Operación';
            break;
        case 11: //11. Hora de inicio y fin de las Indisponibilidades de las Unidades de Generación del SEIN y su respectivo motivo
            htmlAplicativo = 'Intervenciones';
            break;
        case 12: //12. Reserva Fría del sistema
            htmlAplicativo = 'CDispatch';
            break;
        case 13: //13. Caudales en los principales afluentes a las Centrales Hidroeléctricas
            htmlAplicativo = 'Hidrología';
            break;
        case 14://14. Volúmenes horarios y caudales horarios de descarga de los embalses asociados a las Centrales Hidroeléctricas
            htmlAplicativo = 'Hidrología';
            break;
        case 18://15. Vertimientos en los embalses y/o presas en período y volumen
            htmlAplicativo = 'Hidrología';
            break;
        case 171://16. Volúmenes o cantidad de combustible almacenado a las 24:00 h de las Centrales Térmicas
            htmlAplicativo = 'Stock de combustible';
            break;
        case 175: //17. Volúmenes o cantidad diaria de combustible consumido (asociado a la generación) por cada Unidad de Generación termoeléctrica
            htmlAplicativo = 'Consumo de combustible';
            break;
        case 176: //18. Volúmenes diarios de gas natural consumido (asociado a la generación) y presión horaria del gas natural al ingreso (en el lado de alta presión)
            htmlAplicativo = 'Presión de Gas y Temperatura';
            break;
        case 177: //19. Reporte cada 30 minutos de la fuente de energía primaria de las unidades RER solar, geotérmica y biomasa. En caso de las Centrales Eólicas, la velocidad del viento registrada cada 30 minutos
            htmlAplicativo = 'Fuente de Energía primaria';
            break;
        case 179: //20. En caso sea una Central de Cogeneración Calificada, deberá remitir información sobre la producción del Calor Útil de sus Unidades de Generación o el Calor Útil recibido del proceso industrial asociado, en MW
            htmlAplicativo = 'Calor Útil';
            break;
        case 15: //21. Registro cada 30 minutos del flujo (MW y MVAr) por las líneas de transmisión y transformadores de potencia definidos por el COES
            htmlAplicativo = 'Flujo de potencia activa / reactiva';
            break;
        case 20: //23. Registro cada 30 minutos de la tensión de las Barras del SEIN definidas por el COES
            htmlAplicativo = 'Tensión de barras';
            break;
        case 21: //24. Reporte de sobrecarga de equipos mayores a 100 kV. De presentarse sobrecarga en equipos menores a 100 kV hasta los 60 kV, que ocasione acciones correctivas en la Operación en Tiempo Real, se incluirá dicha sobrecarga en el reporte respectivo
            htmlAplicativo = 'Operaciones Varias';
            break;
        case 172: //25. Reporte de líneas desconectadas por Regulación de Tensión
            htmlAplicativo = 'Operaciones Varias';
            break;
        case 173: //26. Reporte de Sistemas Aislados Temporales
            htmlAplicativo = 'Operaciones Varias';
            break;
        case 16: //27. Reporte de las variaciones sostenidas y súbitas de frecuencia en el SEIN
            htmlAplicativo = 'Frecuencia';
            break;
        case 180: //28. Reporte de Sistemas Aislados Temporales y sus variaciones sostenidas y súbitas de frecuencia
            htmlAplicativo = 'Frecuencia';
            break;       
        case 340: //29. Reporte de las interrupciones de suministro, Esquema Rechazo Automático de Carga (ERAC) y Rechazo Manual de Carga (RMC), así como Racionamiento, conforme a lo establecido en el Procedimiento Técnico del COES N° 16 “Racionamiento por déficit de oferta” (PR-16) o el que lo sustituya.
            htmlAplicativo = '';
            break;
        case 17: //30. Desviaciones de la demanda respecto a su pronóstico
            htmlAplicativo = 'Cdispatch';
            break;
        case 174: //31. Desviaciones de la producción de las Unidades de Generación
            htmlAplicativo = 'Cdispatch';
            break;
        case 3: //32. Costos Marginales de Corto Plazo cada 30 minutos en las Barras del SEIN
            htmlAplicativo = 'Costos marginales revisados';
            break;
        case 178: //33. Costo total de operación ejecutada
            htmlAplicativo = 'Cdispatch';
            break;
        case 181: //34. Calificación de la operación de las Unidades de Generación
            htmlAplicativo = 'Horas de Operación';
            break;
        case 182: //35. Registro de las congestiones del Sistema de Transmisión
            htmlAplicativo = 'Operaciones Varias';
            break;
        case 183: //36. Registro de asignación de la RRPF y RRSF
            htmlAplicativo = 'Unidad de Regulación secundaria';
            break;
        case 2: //37. Registro de los flujos (MW y MVAr) cada 30 minutos de los enlaces internacionales
            htmlAplicativo = 'Flujo de potencia activa y reactiva';
            break;
    }

    switch (id_numeral) {
        case 76: //1. Reporte de Eventos: fallas, interrupciones, restricciones y otros de carácter operativo
            urlAplicativo = 'eventos/evento/index/';
            break;
        case 77://2 Reporte de las principales restricciones operativas y mantenimiento de las Unidades de Generación y de los equipos del Sistema de Transmisión
            urlAplicativo = 'eventos/operacionesvarias/index/';
            break;
        case 78://3. Reporte de ingreso a operación comercial de unidades o centrales de generación, así como de la conexión e integración al SEIN de instalaciones de transmisión
            urlAplicativo = 'Equipamiento/Equipo/Index/';
            break;
        case 4://4. Despacho registrado cada 30 minutos de las Unidades de Generación de los Integrantes del COES, asimismo, se incluye las Unidades de Generación con potencia superior a 5 MW conectadas al SEIN de empresas no Integrantes del COES (MW, MVAr)
            urlAplicativo = 'Migraciones/Despacho/Index/';
            break;
        case 5://5. Reporte de la demanda por áreas (MW)
            urlAplicativo = 'Migraciones/Despacho/Index/';
            break;
        case 6://6. Reporte de demanda de Grandes Usuarios (MW)
            urlAplicativo = 'Migraciones/Reporte/DemandaBarras/';
            break;
        case 7://7. Recursos energéticos y diagrama de duración de demanda del SEINL
            urlAplicativo = 'Migraciones/Despacho/Index/';
            break;
        case 184://8. Evolución de la producción de energía diaria.
            urlAplicativo = 'Migraciones/Despacho/Index/';
            break;
        case 8: //9. Máxima generación instantánea del SEIN (MW)
            urlAplicativo = 'Migraciones/Despacho/Index/';
            break;
        case 10: //10. Horas de orden de arranque y parada, así como las horas de ingreso y salida de las Unidades de Generación del SEIN
            urlAplicativo = 'ieod/horasoperacion/reporte/';
            break;
        case 11: //11. Hora de inicio y fin de las Indisponibilidades de las Unidades de Generación del SEIN y su respectivo motivo
            urlAplicativo = 'Intervenciones/Registro/Programaciones/';
            break;
        case 12: //12. Reserva Fría del sistema
            urlAplicativo = 'Migraciones/Despacho/Index/';
            break;
        case 13: //13. Caudales en los principales afluentes a las Centrales Hidroeléctricas
            urlAplicativo = 'Hidrologia/Reporte/Index/';
            break;
        case 14://14. Volúmenes horarios y caudales horarios de descarga de los embalses asociados a las Centrales Hidroeléctricas
            urlAplicativo = 'Hidrologia/Reporte/Index/';
            break;
        case 18://15. Vertimientos en los embalses y/o presas en período y volumen
            urlAplicativo = 'Hidrologia/Reporte/DescargaLagVert/';
            break;
        case 171://16. Volúmenes o cantidad de combustible almacenado a las 24:00 h de las Centrales Térmicas
            urlAplicativo = 'StockCombustibles/Reportes/Stock/';
            break;
        case 175: //17. Volúmenes o cantidad diaria de combustible consumido (asociado a la generación) por cada Unidad de Generación termoeléctrica
            urlAplicativo = 'StockCombustibles/Reportes/Consumo/';
            break;
        case 176: //18. Volúmenes diarios de gas natural consumido (asociado a la generación) y presión horaria del gas natural al ingreso (en el lado de alta presión)
            urlAplicativo = 'StockCombustibles/Reportes/Consumo/';
            break;
        case 177: //19. Reporte cada 30 minutos de la fuente de energía primaria de las unidades RER solar, geotérmica y biomasa. En caso de las Centrales Eólicas, la velocidad del viento registrada cada 30 minutos
            urlAplicativo = 'ieod/EnergiaPrimaria/index/';
            flag = 1;
            break;
        case 179: //20. En caso sea una Central de Cogeneración Calificada, deberá remitir información sobre la producción del Calor Útil de sus Unidades de Generación o el Calor Útil recibido del proceso industrial asociado, en MW
            urlAplicativo = 'ieod/CalorUtil/index/';
            flag = 1;
            break;
        case 15: //21. Registro cada 30 minutos del flujo (MW y MVAr) por las líneas de transmisión y transformadores de potencia definidos por el COES
            urlAplicativo = 'ieod/cargadatos/flujos'; // nuevo enlace
            break;
        case 20: //23. Registro cada 30 minutos de la tensión de las Barras del SEIN definidas por el COES
            urlAplicativo = 'ieod/cargadatos/flujos'; //NUEVO ENLACE
            break;
        case 21: //24. Reporte de sobrecarga de equipos mayores a 100 kV. De presentarse sobrecarga en equipos menores a 100 kV hasta los 60 kV, que ocasione acciones correctivas en la Operación en Tiempo Real, se incluirá dicha sobrecarga en el reporte respectivo
            urlAplicativo = 'eventos/operacionesvarias/index/';
            break;
        case 172: //25. Reporte de líneas desconectadas por Regulación de Tensión
            urlAplicativo = 'eventos/operacionesvarias/index/';
            break;
        case 173: //26. Reporte de Sistemas Aislados Temporales
            urlAplicativo = 'eventos/operacionesvarias/index/';
            break;
        case 16: //27. Reporte de las variaciones sostenidas y súbitas de frecuencia en el SEIN
            urlAplicativo = 'serviciorpf/frecuencia/index/';            
            break;
        case 180: //28. Reporte de Sistemas Aislados Temporales y sus variaciones sostenidas y súbitas de frecuencia
            urlAplicativo = 'serviciorpf/frecuencia/index/';            
            break;
        case 340: //29. Reporte de las interrupciones de suministro, Esquema Rechazo Automático de Carga (ERAC) y Rechazo Manual de Carga (RMC), así como Racionamiento, conforme a lo establecido en el Procedimiento Técnico del COES N° 16 “Racionamiento por déficit de oferta” (PR-16) o el que lo sustituya.
            urlAplicativo = ''; ///NO EXISTE
            break;
        case 17: //30. Desviaciones de la demanda respecto a su pronóstico
            urlAplicativo = 'Migraciones/Despacho/Index/';
            break;
        case 174: //31. Desviaciones de la producción de las Unidades de Generación
            urlAplicativo = 'Migraciones/Despacho/Index/';
            break;
        case 3: //32. Costos Marginales de Corto Plazo cada 30 minutos en las Barras del SEIN
            urlAplicativo = 'siosein/Numerales/ViewCostoMarginalCP/';
            break;
        case 178: //33. Costo total de operación ejecutada
            urlAplicativo = 'Migraciones/Despacho/Index/';
            break;
        case 181: //34. Calificación de la operación de las Unidades de Generación
            urlAplicativo = 'ieod/horasoperacion/reporte/';
            break;
        case 182: //35. Registro de las congestiones del Sistema de Transmisión
            urlAplicativo = 'eventos/operacionesvarias/index/';
            break;
        case 183: //36. Registro de asignación de la RRPF y RRSF
            urlAplicativo = 'eventos/rsf/index/';
            break;
        case 2: //37. Registro de los flujos (MW y MVAr) cada 30 minutos de los enlaces internacionales
            urlAplicativo = 'ieod/cargadatos/flujos'; //NUEVO ENLACE
            break;
    }
    
    var htmlTexto = '';
    htmlTexto += `
        Los datos son obtenidos del aplicativo <b><span style="color: blue;">${htmlAplicativo}<span></b>. <input type="button" value="Ir a aplicativo" onclick="abrirVentanaAplicativo('${urlAplicativo}', ${flag});">
    `;    
    return htmlTexto;
}

function getTextoLeyendaSemanal(id_numeral) {
    var htmlAplicativo = '';
    var urlAplicativo = '';    
    var flag = 0; // 0: intranet, 1: extranet, 2: Portal
    switch (id_numeral) {
        case 108: //Resumen Relevante
            htmlAplicativo = 'Anexo A (numeral 4)';
            break;

        case 343: //Resumen de Producción
            htmlAplicativo = 'Anexo A (numeral 4)'; ///NUEVO
            break;

        case 44://1.1 Ingreso en Operación Comercial al SEIN
            htmlAplicativo = 'Equipamiento';
            break;

        case 45://1.2 Retiro de Operación Comercial
            htmlAplicativo = 'Equipamiento';
            break;

        case 46://2.1 Producción por tipo de Generación
            htmlAplicativo = 'Anexo A (numeral 4)';
            break;

        case 47://2.2 Producción por tipo de Recurso Energético
            htmlAplicativo = 'Anexo A (numeral 4)';
            break;

        case 48://2.3 Producción por Recursos Energéticos Renovables
            htmlAplicativo = 'Anexo A (numeral 4)';
            break;
        case 49://2.4 Factor de planta de las centrales RER
            htmlAplicativo = 'Anexo A (numeral 4)';
            break;
        case 50://2.5 Participación de la producción por empresas Integrantes
            htmlAplicativo = 'Anexo A (numeral 4)';
            break;

        case 51://3.1 Máxima demanda Por tipo de generación
            htmlAplicativo = 'Anexo A (numeral 4)';
            break;
        case 52://3.2 Participación por Empresas Integrantes
            htmlAplicativo = 'Anexo A (numeral 4)';
            break;
        case 53://3.3 Evolución de la demanda por áreas operativas
            htmlAplicativo = 'Anexo A (numeral 4)';
            break;
       
        case 54: //4.1 Demanda de Grandes Usuarios en el día de máxima demanda semanal (MW)
            htmlAplicativo = 'Reporte Demanda en Barras';
            break;

        case 55: //4.2 Diagrama de Carga por rangos de potencia en Grandes Usuarios (MW)
            htmlAplicativo = 'Reporte Demanda en Barras';
            break;

        case 56: //4.3 Demanda de energía por área operativa de los Principales Grandes Usuarios (GWh)
            htmlAplicativo = 'Reporte Demanda en Barras';
            break;

        case 57: //5.1 Volumen útil de los embalses y lagunas (Mm3)
            htmlAplicativo = 'Hidrología';
            break;

        case 58: //5.2 Evolución de volúmenes de embalses y lagunas
            htmlAplicativo = 'Hidrología';
            break;

        case 59: //5.3 Promedio semanal de los caudales (m3/s)
            htmlAplicativo = 'Hidrología';
            break;

        case 60: //5.4 Evolución de los caudales
            htmlAplicativo = 'Hidrología';
            break;

        case 61: //6.1 Por tipo de combustible
            htmlAplicativo = 'Consumo de Combustible de Centrales Termoeléctricas';
            break;

        case 62: //7.1 COSTO DE OPERACIÓN EJECUTADO ACUMULADO SEMANAL DEL SEIN (Millones de S/.)
            htmlAplicativo = 'Anexo A (numeral 33)';
            break;

        case 63: //8.1 Evolución de los Costos Marginales Nodales Promedio semanal del SEIN (S/./MWh)
            htmlAplicativo = 'Costos Marginales Revisados';
            break;

        case 64: //8.2. Evolución  de los Costos Marginales Nodales Promedio semanal por área operativa (US$/MWh)
            htmlAplicativo = 'Costos Marginales Revisados';
            break;

        case 65: //9.1 Tensión en barras de la red en 500kV           
            htmlAplicativo = 'Scada SP7 cada 15 minutos';
            break;       

        case 66: //9.2 Tensión en barras de la red en 220kV            
            htmlAplicativo = 'Scada SP7 cada 15 minutos';
            break;

        case 67: //9.3 Tensión en barras de la red en 138kV
            htmlAplicativo = 'Scada SP7 cada 15 minutos';
            break;

        case 68: //10.1 Flujo máximo de Interconexión en los Enlaces Centro - Norte y Centro - Sur
            htmlAplicativo = 'Interconexiones';
            break;

        case 69: //11.1 Por Área Operativa
            htmlAplicativo = 'Operaciones varias';
            break;

        case 71: //12.1 Intercambios de electricidad de energía y potencia
            htmlAplicativo = 'Flujos de potencia activa cada 30 minutos';
            break;

        case 72: //13.1 Fallas por tipo de equipo y causa según clasificación CIER
            htmlAplicativo = 'Eventos';
            break;

        case 341: //13.1 Fallas por tipo de equipo y causa según clasificación CIER
            htmlAplicativo = 'Eventos';
            break;
    }

    switch (id_numeral) {
        case 108: //Resumen Relevante
            urlAplicativo = 'IEOD/AnexoA/MenuAnexoA/';
            break;

        case 343: //Resumen de Producción
            urlAplicativo = 'IEOD/AnexoA/MenuAnexoA/'; 
            break;
            
        case 44://1.1 Ingreso en Operación Comercial al SEIN
            urlAplicativo = 'Equipamiento/Equipo/Index/'; 
            break;

        case 45://1.2 Retiro de Operación Comercial
            urlAplicativo = 'Equipamiento/Equipo/Index/';
            break;

        case 46://2.1 Producción por tipo de Generación
            urlAplicativo = 'IEOD/AnexoA/MenuAnexoA/'; 
            break;

        case 47://2.2 Producción por tipo de Recurso Energético
            urlAplicativo = 'IEOD/AnexoA/MenuAnexoA/';
            break;

        case 48://2.3 Producción por Recursos Energéticos Renovables
            urlAplicativo = 'IEOD/AnexoA/MenuAnexoA/';
            break;
        case 49://2.4 Factor de planta de las centrales RER
            urlAplicativo = 'IEOD/AnexoA/MenuAnexoA/';
            break;
        case 50://2.5 Participación de la producción por empresas Integrantes
            urlAplicativo = 'IEOD/AnexoA/MenuAnexoA/';
            break;

        case 51://3.1 Máxima demanda Por tipo de generación
            urlAplicativo = 'IEOD/AnexoA/MenuAnexoA/';
            break;
        case 52://3.2 Participación por Empresas Integrantes
            urlAplicativo = 'IEOD/AnexoA/MenuAnexoA/';
            break;
        case 53://3.3 Evolución de la demanda por áreas operativas
            urlAplicativo = 'IEOD/AnexoA/MenuAnexoA/';
            break;

        case 54: //4.1 Demanda de Grandes Usuarios en el día de máxima demanda semanal (MW)
            urlAplicativo = 'Migraciones/Reporte/DemandaBarras/';
            break;

        case 55: //4.2 Diagrama de Carga por rangos de potencia en Grandes Usuarios (MW)
            urlAplicativo = 'Migraciones/Reporte/DemandaBarras/';
            break;

        case 56: //4.3 Demanda de energía por área operativa de los Principales Grandes Usuarios (GWh)
            urlAplicativo = 'Migraciones/Reporte/DemandaBarras/';
            break;

        case 57: //5.1 Volumen útil de los embalses y lagunas (Mm3)
            urlAplicativo = 'Hidrologia/Reporte/Index/';
            break;

        case 58: //5.2 Evolución de volúmenes de embalses y lagunas
            urlAplicativo = 'Hidrologia/Reporte/Index/';
            break;

        case 59: //5.3 Promedio semanal de los caudales (m3/s)
            urlAplicativo = 'Hidrologia/Reporte/Index/';
            break;

        case 60: //5.4 Evolución de los caudales
            urlAplicativo = 'Hidrologia/Reporte/Index/';
            break;

        case 61: //6.1 Por tipo de combustible
            urlAplicativo = 'StockCombustibles/Reportes/Consumo/';
            break;

        case 62: //7.1 COSTO DE OPERACIÓN EJECUTADO ACUMULADO SEMANAL DEL SEIN (Millones de S/.)
            urlAplicativo = 'IEOD/AnexoA/MenuAnexoA/';
            break;

        case 63: //8.1 Evolución de los Costos Marginales Nodales Promedio semanal del SEIN (S/./MWh)
            urlAplicativo = 'siosein/Numerales/CostoMarginalMensual/';
            break;

        case 64: //8.2. Evolución  de los Costos Marginales Nodales Promedio semanal por área operativa (US$/MWh)
            urlAplicativo = 'siosein/Numerales/CostoMarginalMensual/';
            break;

        case 65: //9.1 Tensión en barras de la red en 500kV           
            urlAplicativo = 'tiemporeal/scadasp7/index/';
            break;

        case 66: //9.2 Tensión en barras de la red en 220kV            
            urlAplicativo = 'tiemporeal/scadasp7/index/';
            break;

        case 67: //9.3 Tensión en barras de la red en 138kV
            urlAplicativo = 'tiemporeal/scadasp7/index/';
            break;

        case 68: //10.1 Flujo máximo de Interconexión en los Enlaces Centro - Norte y Centro - Sur
            urlAplicativo = 'ieod/cargadatos/interconexion'; 
            break;

        case 69: //11.1 Por Área Operativa
            urlAplicativo = 'Eventos/OperacionesVarias/Index';
            break;

        case 71: //12.1 Intercambios de electricidad de energía y potencia
            urlAplicativo = 'ieod/cargadatos/flujos'; // nuevo enlace....
            break;

        case 72: //13.1 Fallas por tipo de equipo y causa según clasificación CIER
            urlAplicativo = 'eventos/evento/index/';
            break;

        case 341: //13.1 Fallas por tipo de equipo y causa según clasificación CIER
            urlAplicativo = 'eventos/evento/index/';
            break;
    }
 

    var htmlTexto = '';
    htmlTexto += `
        Los datos son obtenidos del aplicativo <b><span style="color: blue;">${htmlAplicativo}<span></b>. <input type="button" value="Ir a aplicativo" onclick="abrirVentanaAplicativo('${urlAplicativo}', ${flag});">
    `;
    

    return htmlTexto;
}

function getTextoLeyendaEjecutivoSem(id_numeral) {
    var htmlAplicativo = '';
    var urlAplicativo = '';    
    var flag = 0; // 0: intranet, 1: extranet, 2: Portal

    switch (id_numeral) {
        
        case 108: //Resumen Relevante
            htmlAplicativo = 'Anexo A (numeral 4)';
            break;

        case 22://1.1 Ingreso en Operación Comercial al SEIN
            htmlAplicativo = 'Equipamiento';
            break;

        case 23://1.2 Retiro de Operación Comercial
            htmlAplicativo = 'Equipamiento';
            break;
        
        case 24://2.1 Producción por tipo de Generación
            htmlAplicativo = 'Anexo A (numeral 4)';
            break;

        case 25://2.2 Producción por tipo de Recurso Energético
            htmlAplicativo = 'Anexo A (numeral 4)';
            break;
        case 26://2.3 Producción por Recursos Energéticos Renovables
            htmlAplicativo = 'Anexo A (numeral 4)';
            break;
        case 27://2.4 Factor de planta de las centrales RER
            htmlAplicativo = 'Anexo A (numeral 4)';
            break;
       
        case 28: //2.5 Participación de la producción por empresas Integrantes
            htmlAplicativo = 'Anexo A (numeral 4)';
            break;

        case 29://3.1 Máxima demanda Por tipo de generación
            htmlAplicativo = 'Anexo A (numeral 4)';
            break;
        case 30://3.2 Participación por Empresas Integrantes
            htmlAplicativo = 'Anexo A (numeral 4)';
            break;
        case 31://3.3 Evolución de la demanda por áreas operativas
            htmlAplicativo = 'Anexo A (numeral 4)”.';
            break;
       
        case 32: //4.1 Volumen útil de los embalses y lagunas (Mm3)
            htmlAplicativo = 'Hidrología';
            break;
       
        case 33: //4.2 Evolución de volúmenes de embalses y lagunas
            htmlAplicativo = 'Hidrología';
            break;

        case 34: //4.3 Promedio mensual de los caudales (m3/s)
            htmlAplicativo = 'Hidrología';
            break;

        case 35: //4.4 Evolución de los caudales
            htmlAplicativo = 'Hidrología';
            break;

        case 36: //5.1 Evolución de los Costos de operación acumulado semanal del SEIN (Millones de S/.)
            htmlAplicativo = 'Anexo A (numeral 33)';
            break;

        case 37: //6.1 Evolución de los Costos Marginales Promedio Nodales Diarios del SEIN (S/./MWh)
            htmlAplicativo = 'Costos Marginales Revisados';
            break;

        case 38: //7.1 Flujo máximo de Interconexión en los Enlaces Centro - Norte y Centro - Sur
            htmlAplicativo = 'Interconexiones';
            break;

        case 39: //8.1 Por Área Operativa
            htmlAplicativo = 'Operaciones varias';
            break;

        case 40: //9.1 Por tipo de combustible
            htmlAplicativo = 'Consumo de Combustible de Centrales Termoeléctricas';
            break;

        case 41: //10.1 Intercambios de electricidad de energía y potencia
            htmlAplicativo = 'Flujos de potencia activa cada 30 minutos';
            break;

        case 42: //11.1 Fallas por tipo de equipo y causa según clasificación CIER
            htmlAplicativo = 'Eventos';
            break;

        case 342: //11.1 Fallas por tipo de equipo y causa según clasificación CIER
            htmlAplicativo = 'Eventos';
            break;
    }

    switch (id_numeral) {

        case 108: //Resumen Relevante
            urlAplicativo = 'IEOD/AnexoA/MenuAnexoA/';
            break;

        case 22://1.1 Ingreso en Operación Comercial al SEIN
            urlAplicativo = 'Equipamiento/Equipo/Index/';
            break;

        case 23://1.2 Retiro de Operación Comercial
            urlAplicativo = 'Equipamiento/Equipo/Index/';
            break;

        case 24://2.1 Producción por tipo de Generación
            urlAplicativo = 'IEOD/AnexoA/MenuAnexoA/';
            break;

        case 25://2.2 Producción por tipo de Recurso Energético
            urlAplicativo = 'IEOD/AnexoA/MenuAnexoA/';
            break;
        case 26://2.3 Producción por Recursos Energéticos Renovables
            urlAplicativo = 'IEOD/AnexoA/MenuAnexoA/';
            break;
        case 27://2.4 Factor de planta de las centrales RER
            urlAplicativo = 'IEOD/AnexoA/MenuAnexoA/';
            break;

        case 28: //2.5 Participación de la producción por empresas Integrantes
            urlAplicativo = 'IEOD/AnexoA/MenuAnexoA/';
            break;

        case 29://3.1 Máxima demanda Por tipo de generación
            urlAplicativo = 'IEOD/AnexoA/MenuAnexoA/';
            break;
        case 30://3.2 Participación por Empresas Integrantes
            urlAplicativo = 'IEOD/AnexoA/MenuAnexoA/';
            break;
        case 31://3.3 Evolución de la demanda por áreas operativas
            urlAplicativo = 'IEOD/AnexoA/MenuAnexoA/';
            break;

        case 32: //4.1 Volumen útil de los embalses y lagunas (Mm3)
            urlAplicativo = 'Hidrologia/Reporte/Index/'; 
            break;

        case 33: //4.2 Evolución de volúmenes de embalses y lagunas
            urlAplicativo = 'Hidrologia/Reporte/Index/';
            break;

        case 34: //4.3 Promedio mensual de los caudales (m3/s)
            urlAplicativo = 'Hidrologia/Reporte/Index/';
            break;

        case 35: //4.4 Evolución de los caudales
            urlAplicativo = 'Hidrologia/Reporte/Index/';
            break;

        case 36: //5.1 Evolución de los Costos de operación acumulado semanal del SEIN (Millones de S/.)
            urlAplicativo = 'IEOD/AnexoA/MenuAnexoA/';
            break;

        case 37: //6.1 Evolución de los Costos Marginales Promedio Nodales Diarios del SEIN (S/./MWh)
            urlAplicativo = 'siosein/Numerales/CostoMarginalMensual/';
            break;

        case 38: //7.1 Flujo máximo de Interconexión en los Enlaces Centro - Norte y Centro - Sur
            urlAplicativo = 'ieod/cargadatos/interconexion'; 
            break;

        case 39: //8.1 Por Área Operativa
            urlAplicativo = 'eventos/operacionesvarias/index/';
            break;

        case 40: //9.1 Por tipo de combustible
            urlAplicativo = 'StockCombustibles/Reportes/Consumo/';
            break;

        case 41: //10.1 Intercambios de electricidad de energía y potencia
            urlAplicativo = 'ieod/cargadatos/flujos'; 
            break;

        case 42: //11.1 Fallas por tipo de equipo y causa según clasificación CIER
            urlAplicativo = 'eventos/evento/index/';
            break;

        case 342: //11.2 Detalle de eventos
            urlAplicativo = 'eventos/evento/index/';
            break;
    }
         
    var htmlTexto = '';
    htmlTexto += `
        Los datos son obtenidos del aplicativo <b><span style="color: blue;">${htmlAplicativo}<span></b>. <input type="button" value="Ir a aplicativo" onclick="abrirVentanaAplicativo('${urlAplicativo}', ${flag});">
    `;   

    return htmlTexto;
}

//#endregion
