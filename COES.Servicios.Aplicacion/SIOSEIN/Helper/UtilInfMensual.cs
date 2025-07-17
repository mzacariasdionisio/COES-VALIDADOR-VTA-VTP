using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Siosein2;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace COES.Servicios.Aplicacion.SIOSEIN
{
    public static class UtilInfMensual
    {
        /// <summary>
        /// Metodo que retorna fechas actuales y anteriores apartir de un rango de fechas para el Informe Mensual
        /// </summary>
        /// <param name="fechaActualInicial"></param>
        /// <param name="fechaActualFinal"></param>
        /// <returns></returns>
        public static FechasPR5 ObtenerFechasInformesMensual(DateTime fechaActualInicial)
        {
            DateTime fechaActualFinal = fechaActualInicial.AddMonths(1).AddDays(-1);

            FechasPR5 objFecha = new FechasPR5();
            objFecha.TipoReporte = ConstantesSioSein.ReptipcodiMensual;
            objFecha.FechaInicial = fechaActualInicial;
            objFecha.FechaFinal = fechaActualFinal;
            objFecha.EsReporteXMes = true;

            int anioActual = fechaActualInicial.Year;
            fechaActualInicial = fechaActualInicial.Date;
            fechaActualFinal = fechaActualFinal.Date;

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // AÑO ACTUAL
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////            

            //fechas esta semana
            DateTime fechaIniSemanaFinalAnioActual = fechaActualInicial;

            //1 de enero de año actual
            DateTime fecha01EneroAnioActual = new DateTime(anioActual, 1, 1);
            DateTime fecha31DiciembreAnioActual = new DateTime(anioActual, 12, 31);

            //primer día de la primera semana del año actual
            DateTime fechaAnioActSem01 = EPDate.f_fechainiciosemana(anioActual, 1);

            //primer dia y último día del mes actual
            int mesActual = fechaIniSemanaFinalAnioActual.Month;
            DateTime fecha01MesAnioActual = new DateTime(anioActual, mesActual, 1);
            DateTime fechaFinMesAnioActual = fecha01MesAnioActual.AddMonths(1).AddMilliseconds(-1);

            DateTime MesAct_AnioAct_FechaIni = fecha01MesAnioActual;
            DateTime MesAct_AnioAct_FechaFin = fechaFinMesAnioActual;
            DateTime AnioAct_MesAct_Final = fechaActualFinal;

            //pasamos valores al objeto FechasPR5
            objFecha.AnioAct = new PR5DatoAnio();
            objFecha.AnioAct.RangoAct_FechaIni = MesAct_AnioAct_FechaIni;
            objFecha.AnioAct.RangoAct_FechaFin = MesAct_AnioAct_FechaFin;

            //primer dia y último día del mes Anterior anio actual
            DateTime fecha01_Mes1Ant_AnioAct_ = new DateTime(anioActual, mesActual, 1).AddMonths(-1);
            //DateTime fechaFin_Mes1Ant_AnioAct_ = fecha01_Mes1Ant_AnioAct_.AddMonths(1).AddDays(-1);
            DateTime fechaFin_Mes1Ant_AnioAct_ = fecha01_Mes1Ant_AnioAct_.AddMonths(1).AddMilliseconds(-1);
            DateTime Mes1Ant_AnioAct_FechaIni_ = fecha01_Mes1Ant_AnioAct_;
            DateTime Mes1Ant_AnioAct_FechaFin_ = fechaFin_Mes1Ant_AnioAct_;

            objFecha.AnioAct.Rango1Ant_FechaIni = Mes1Ant_AnioAct_FechaIni_;
            objFecha.AnioAct.Rango1Ant_FechaFin = Mes1Ant_AnioAct_FechaFin_;

            //primer dia y último día del mes 2Anterior anio actual
            DateTime fecha01_Mes2Ant_AnioAct_ = new DateTime(anioActual, mesActual, 1).AddMonths(-2);
            //DateTime fechaFin_Mes2Ant_AnioAct_ = fecha01_Mes2Ant_AnioAct_.AddMonths(1).AddDays(-1);
            DateTime fechaFin_Mes2Ant_AnioAct_ = fecha01_Mes2Ant_AnioAct_.AddMonths(1).AddMilliseconds(-1);
            DateTime Mes2Ant_AnioAct_FechaIni_ = fecha01_Mes2Ant_AnioAct_;
            DateTime Mes2Ant_AnioAct_FechaFin_ = fechaFin_Mes2Ant_AnioAct_;

            objFecha.AnioAct.Rango2Ant_FechaIni = Mes2Ant_AnioAct_FechaIni_;
            objFecha.AnioAct.Rango2Ant_FechaFin = Mes2Ant_AnioAct_FechaFin_;

            objFecha.AnioAct.Fecha_01Enero = fecha01EneroAnioActual;
            objFecha.AnioAct.Fecha_Inicial = fechaActualInicial;
            objFecha.AnioAct.Fecha_Final = AnioAct_MesAct_Final;
            objFecha.AnioAct.Fecha_31Diciembre = fecha31DiciembreAnioActual;

            objFecha.AnioAct.Sem01_FechaIni = fechaAnioActSem01;  //no usado ahasta 2.1

            objFecha.AnioAct.MesAct_FechaIni = fecha01MesAnioActual;
            objFecha.AnioAct.MesAct_FechaFin = fechaFinMesAnioActual;

            objFecha.AnioAct.NumMes = fecha01MesAnioActual.Month;
            objFecha.AnioAct.NumAnio = anioActual;
            objFecha.AnioAct.RangoAct_Num = EPDate.f_NombreMes(fecha01MesAnioActual.Month);
            objFecha.AnioAct.RangoAct_NumYAnio = EPDate.f_NombreMes(fecha01MesAnioActual.Month) + " " + anioActual;

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////            
            #region 1 AÑO ATRAS
            int anio1Anterior = anioActual - 1;

            //1 de enero de año actual
            DateTime fecha01EneroAnio1Anterior = new DateTime(anio1Anterior, 1, 1);
            DateTime fecha31DiciembreAnio1Anterior = new DateTime(anio1Anterior, 12, 31);

            //primer día de la primera semana del año actual
            DateTime fechaAnioAntSem01 = EPDate.f_fechainiciosemana(anio1Anterior, 1);

            //primer dia y último día del mes actual 
            DateTime fecha01MesAnio1Ant = new DateTime(anio1Anterior, mesActual, 1);
            DateTime fechaFinMesAnio1Ant = fecha01MesAnio1Ant.AddMonths(1).AddMilliseconds(-1);

            DateTime MesAct_Anio1Ant_FechaIni = fecha01MesAnio1Ant;
            DateTime MesAct_Anio1Ant_FechaFin = fechaFinMesAnio1Ant;
            DateTime Anio1Ant_MesAct_Final = fechaFinMesAnio1Ant;

            //pasamos valores al objeto FechasPR5
            objFecha.Anio1Ant = new PR5DatoAnio();
            objFecha.Anio1Ant.RangoAct_FechaIni = MesAct_Anio1Ant_FechaIni;
            objFecha.Anio1Ant.RangoAct_FechaFin = MesAct_Anio1Ant_FechaFin;

            objFecha.Anio1Ant.Fecha_01Enero = fecha01EneroAnio1Anterior;
            objFecha.Anio1Ant.Fecha_Inicial = fecha01MesAnio1Ant;
            objFecha.Anio1Ant.Fecha_Final = Anio1Ant_MesAct_Final;
            objFecha.Anio1Ant.Fecha_31Diciembre = fecha31DiciembreAnio1Anterior;

            objFecha.Anio1Ant.Sem01_FechaIni = fechaAnioAntSem01;  //No interviene hasta 2.1

            objFecha.Anio1Ant.NumMes = MesAct_Anio1Ant_FechaIni.Month;
            objFecha.Anio1Ant.NumAnio = anio1Anterior;
            objFecha.Anio1Ant.RangoAct_Num = EPDate.f_NombreMes(MesAct_Anio1Ant_FechaIni.Month);
            objFecha.Anio1Ant.RangoAct_NumYAnio = EPDate.f_NombreMes(MesAct_Anio1Ant_FechaIni.Month) + " " + anio1Anterior;

            objFecha.Anio1Ant.Ini_Data = objFecha.Anio1Ant.Sem01_FechaIni < objFecha.Anio1Ant.Fecha_01Enero ? objFecha.Anio1Ant.Sem01_FechaIni : objFecha.Anio1Ant.Fecha_01Enero;

            #endregion

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////            
            #region 2 AÑOS ATRAS
            int anio2Anterior = anio1Anterior - 1;

            //1 de enero de año actual
            DateTime fecha01EneroAnio2Anterior = new DateTime(anio2Anterior, 1, 1);
            DateTime fecha31DiciembreAnio2Anterior = new DateTime(anio2Anterior, 12, 31);

            //primer día de la primera semana del año actual
            DateTime fechaAnio2AntSem01 = EPDate.f_fechainiciosemana(anio2Anterior, 1);


            //primer dia y último día del mes Anterior anio actual
            DateTime fecha01_Mes1Ant_AnioAct = new DateTime(anio2Anterior, mesActual, 1);
            DateTime fechaFin_Mes1Ant_AnioAct = fecha01_Mes1Ant_AnioAct.AddMonths(1).AddMilliseconds(-1);

            DateTime Mes1Ant_AnioAct_FechaIni = fecha01_Mes1Ant_AnioAct;
            DateTime Mes1Ant_AnioAct_FechaFin = fechaFin_Mes1Ant_AnioAct;

            //primer dia y último día del mes Actual hace dos años
            DateTime fecha01MesAnio2Ant = new DateTime(anio2Anterior, mesActual, 1);
            DateTime fechaFinMesAnio2Ant = fecha01MesAnio2Ant.AddMonths(1).AddMilliseconds(-1);

            DateTime Anio2Ant_MesAct_Final = fechaFinMesAnio2Ant;

            //pasamos valores al objeto FechasPR5
            objFecha.Anio2Ant = new PR5DatoAnio();
            objFecha.Anio2Ant.RangoAct_FechaIni = Mes1Ant_AnioAct_FechaIni;
            objFecha.Anio2Ant.RangoAct_FechaFin = Mes1Ant_AnioAct_FechaFin;

            objFecha.Anio2Ant.Fecha_01Enero = fecha01EneroAnio2Anterior;
            objFecha.Anio2Ant.Fecha_Final = Anio2Ant_MesAct_Final;
            objFecha.Anio2Ant.Fecha_31Diciembre = fecha31DiciembreAnio2Anterior;

            objFecha.Anio2Ant.Sem01_FechaIni = fechaAnio2AntSem01; // no usado hasta 2.1

            objFecha.Anio2Ant.NumMes = Mes1Ant_AnioAct_FechaIni.Month;
            objFecha.Anio2Ant.NumAnio = anio2Anterior;
            objFecha.Anio2Ant.RangoAct_Num = EPDate.f_NombreMes(Mes1Ant_AnioAct_FechaIni.Month);
            objFecha.Anio2Ant.RangoAct_NumYAnio = EPDate.f_NombreMes(Mes1Ant_AnioAct_FechaIni.Month) + " " + anio1Anterior;
            
            #endregion

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////            
            #region 3 AÑOS ATRAS
            int anio3Anterior = anio2Anterior - 1;

            //1 de enero de año actual
            DateTime fecha01EneroAnio3Anterior = new DateTime(anio3Anterior, 1, 1);
            DateTime fecha31DiciembreAnio3Anterior = new DateTime(anio3Anterior, 12, 31);

            //primer día de la primera semana del año actual
            DateTime fechaAnio3AntSem01 = EPDate.f_fechainiciosemana(anio3Anterior, 1);

            //primer dia y último día del mes 2Anterior anio actual
            DateTime fecha01_Mes2Ant_AnioAct = new DateTime(anio3Anterior, mesActual, 1);
            DateTime fechaFin_Mes2Ant_AnioAct = fecha01_Mes2Ant_AnioAct.AddMonths(1).AddMilliseconds(-1);

            DateTime Mes2Ant_AnioAct_FechaIni = fecha01_Mes2Ant_AnioAct;
            DateTime Mes2Ant_AnioAct_FechaFin = fechaFin_Mes2Ant_AnioAct;

            //primer dia y último día del mes Actual hace dos años
            DateTime fecha01MesAnio3Ant = new DateTime(anio3Anterior, mesActual, 1);
            DateTime fechaFinMesAnio3Ant = fecha01MesAnio3Ant.AddMonths(1).AddMilliseconds(-1);

            DateTime Anio3Ant_MesAct_Final = fechaFinMesAnio3Ant;

            //pasamos valores al objeto FechasPR5
            objFecha.Anio3Ant = new PR5DatoAnio();
            objFecha.Anio3Ant.RangoAct_FechaIni = Mes2Ant_AnioAct_FechaIni;
            objFecha.Anio3Ant.RangoAct_FechaFin = Mes2Ant_AnioAct_FechaFin;
            objFecha.Anio3Ant.Fecha_01Enero = fecha01EneroAnio3Anterior;
            objFecha.Anio3Ant.Fecha_Final = Anio3Ant_MesAct_Final;
            objFecha.Anio3Ant.Fecha_31Diciembre = fecha31DiciembreAnio3Anterior;

            objFecha.Anio3Ant.Sem01_FechaIni = fechaAnio3AntSem01; // no usado hasta 2.1

            objFecha.Anio3Ant.NumAnio = anio3Anterior;
            #endregion

            objFecha.ListaFechaBisiesto = UtilAnexoAPR5.ListarFechasBisiestoEnRango(objFecha.Anio3Ant.RangoAct_FechaIni.AddYears(-2), objFecha.AnioAct.RangoAct_FechaFin.AddYears(2));

            return objFecha;
        }

        /// <summary>
        /// Metodo que retorna fechas actuales y anteriores apartir de un rango de fechas para el Informe Anual
        /// </summary>
        /// <param name="fecIniAnioActual"></param>
        /// <returns></returns>
        private static FechasPR5 ObtenerFechasInformesAnual(DateTime fecIniAnioActual)
        {
            fecIniAnioActual = fecIniAnioActual.AddMonths(11); //el parametro es enero para obtener el 1 de diciembre
            DateTime fecFinAnioActual = fecIniAnioActual.AddMonths(1).AddDays(-1); //31 de diciembre

            FechasPR5 objFecha = new FechasPR5();
            objFecha.TipoReporte = ConstantesSioSein.ReptipcodiAnual;
            objFecha.EsReporteXMes = true;
            objFecha.EsReporteAnual = true;
            objFecha.FechaInicial = fecIniAnioActual;
            objFecha.FechaFinal = fecFinAnioActual;
            objFecha.AnioAct = new PR5DatoAnio();
            objFecha.Anio1Ant = new PR5DatoAnio();
            objFecha.Anio2Ant = new PR5DatoAnio();
            objFecha.Anio3Ant = new PR5DatoAnio();


            fecIniAnioActual = fecIniAnioActual.Date;
            fecFinAnioActual = fecFinAnioActual.Date;

            int anioActual = fecIniAnioActual.Year;

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // AÑO ACTUAL
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //fechas esta semana
            DateTime fechaIniSemanaFinalAnioActual = fecIniAnioActual;
            DateTime fechaFinSemanaFinalAnioActual = fecFinAnioActual;

            //1 de enero de año actual
            DateTime fecha01EneroAnioActual = new DateTime(anioActual, 1, 1);
            DateTime fecha31DiciembreAnioActual = new DateTime(anioActual, 12, 31);

            //primer día de la primera semana del año actual
            DateTime fechaAnioActSem01 = EPDate.f_fechainiciosemana(anioActual, 1);

            //primer dia y último día del mes actual
            int mesActual = fechaIniSemanaFinalAnioActual.Month;
            DateTime fecha01MesAnioActual = new DateTime(anioActual, mesActual, 1);
            //DateTime fechaFinMesAnioActual = fecha01MesAnioActual.AddMonths(1).AddDays(-1);
            DateTime fechaFinMesAnioActual = fecha01MesAnioActual.AddMonths(1).AddMilliseconds(-1);

            //Máxima demanda en el periodo mes
            //tomar la fecha valida(2 de cada mes)
            DateTime fechaFinAnio = fechaFinSemanaFinalAnioActual;
            DateTime fechaComp1 = fechaFinAnio.AddDays(-fechaFinAnio.Day + 1);
            DateTime fechaComp2 = fechaIniSemanaFinalAnioActual.AddDays(-fechaIniSemanaFinalAnioActual.Day + 1);
            if ((fechaComp1 == fechaComp2 && fechaFinAnio <= fechaComp2.AddDays(2))
                || (fechaComp1 <= fechaFinAnio))
            {
                fechaFinAnio = fechaComp1.AddDays(-1);
                //fechaFinAnio = fechaComp1.AddMilliseconds(-1);
            }
            else { fechaFinAnio = fechaComp1.AddMonths(1).AddDays(-1); }
            //else { fechaFinAnio = fechaComp1.AddMonths(1).AddMilliseconds(-1); }

            ////Máximo Numero de semana que tiene data para el año actual
            int numeroMaximoSemanaDataActual = 1;
            Tuple<int, int> anioSemFinTmp = EPDate.f_numerosemana_y_anho(fechaFinSemanaFinalAnioActual);

            DateTime fechaIniTmp = EPDate.f_fechainiciosemana(anioSemFinTmp.Item2, anioSemFinTmp.Item1);
            DateTime fechaFinTmp = fechaIniTmp.AddDays(6);
            if (fechaFinTmp == fechaFinSemanaFinalAnioActual)
            { numeroMaximoSemanaDataActual = anioSemFinTmp.Item1; }
            else
            {
                anioSemFinTmp = EPDate.f_numerosemana_y_anho(fechaFinSemanaFinalAnioActual.AddDays(-7));
                numeroMaximoSemanaDataActual = anioSemFinTmp.Item1;
            }

            DateTime MesAct_AnioAct_FechaIni = fecha01MesAnioActual;
            DateTime MesAct_AnioAct_FechaFin = fechaFinMesAnioActual;
            DateTime AnioAct_MesAct_Final = fecFinAnioActual;

            //pasamos valores al objeto FechasPR5
            objFecha.AnioAct = new PR5DatoAnio();
            objFecha.AnioAct.RangoAct_FechaIni = MesAct_AnioAct_FechaIni;
            objFecha.AnioAct.RangoAct_FechaFin = MesAct_AnioAct_FechaFin;


            //primer dia y último día del mes Anterior anio actual
            DateTime fecha01_Mes1Ant_AnioAct_ = new DateTime(anioActual, mesActual, 1).AddMonths(-1);
            //DateTime fechaFin_Mes1Ant_AnioAct_ = fecha01_Mes1Ant_AnioAct_.AddMonths(1).AddDays(-1);
            DateTime fechaFin_Mes1Ant_AnioAct_ = fecha01_Mes1Ant_AnioAct_.AddMonths(1).AddMilliseconds(-1);
            DateTime Mes1Ant_AnioAct_FechaIni_ = fecha01_Mes1Ant_AnioAct_;
            DateTime Mes1Ant_AnioAct_FechaFin_ = fechaFin_Mes1Ant_AnioAct_;

            objFecha.AnioAct.Rango1Ant_FechaIni = Mes1Ant_AnioAct_FechaIni_;
            objFecha.AnioAct.Rango1Ant_FechaFin = Mes1Ant_AnioAct_FechaFin_;

            //primer dia y último día del mes 2Anterior anio actual
            DateTime fecha01_Mes2Ant_AnioAct_ = new DateTime(anioActual, mesActual, 1).AddMonths(-2);
            //DateTime fechaFin_Mes2Ant_AnioAct_ = fecha01_Mes2Ant_AnioAct_.AddMonths(1).AddDays(-1);
            DateTime fechaFin_Mes2Ant_AnioAct_ = fecha01_Mes2Ant_AnioAct_.AddMonths(1).AddMilliseconds(-1);
            DateTime Mes2Ant_AnioAct_FechaIni_ = fecha01_Mes2Ant_AnioAct_;
            DateTime Mes2Ant_AnioAct_FechaFin_ = fechaFin_Mes2Ant_AnioAct_;

            objFecha.AnioAct.Rango2Ant_FechaIni = Mes2Ant_AnioAct_FechaIni_;
            objFecha.AnioAct.Rango2Ant_FechaFin = Mes2Ant_AnioAct_FechaFin_;

            objFecha.AnioAct.Fecha_01Enero = fecha01EneroAnioActual;
            objFecha.AnioAct.Fecha_Inicial = fecIniAnioActual;
            objFecha.AnioAct.Fecha_Final = AnioAct_MesAct_Final;
            objFecha.AnioAct.Fecha_31Diciembre = fecha31DiciembreAnioActual;

            objFecha.AnioAct.Sem01_FechaIni = fechaAnioActSem01;  //no usado ahasta 2.1

            objFecha.AnioAct.MesAct_FechaIni = fecha01MesAnioActual;
            objFecha.AnioAct.MesAct_FechaFin = fechaFinMesAnioActual;

            objFecha.AnioAct.Max_Num_Sem = numeroMaximoSemanaDataActual;

            objFecha.AnioAct.NumAnio = anioActual;
            objFecha.AnioAct.RangoAct_Num = EPDate.f_NombreMes(fechaFinMesAnioActual.Month);
            objFecha.AnioAct.RangoAct_NumYAnio = EPDate.f_NombreMes(fechaFinMesAnioActual.Month) + " " + anioActual;

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////            
            #region 1 AÑO ATRAS
            int anio1Anterior = anioActual - 1;

            //1 de enero de año actual
            DateTime fecha01EneroAnio1Anterior = new DateTime(anio1Anterior, 1, 1);
            DateTime fecha31DiciembreAnio1Anterior = new DateTime(anio1Anterior, 12, 31);

            //primer día de la primera semana del año actual
            DateTime fechaAnioAntSem01 = EPDate.f_fechainiciosemana(anio1Anterior, 1);

            //primer dia y último día del mes actual 
            DateTime fecha01MesAnio1Ant = new DateTime(anio1Anterior, mesActual, 1);
            //DateTime fechaFinMesAnio1Ant = fecha01MesAnio1Ant.AddMonths(1).AddDays(-1);
            DateTime fechaFinMesAnio1Ant = fecha01MesAnio1Ant.AddMonths(1).AddMilliseconds(-1);


            DateTime MesAct_Anio1Ant_FechaIni = fecha01MesAnio1Ant;
            DateTime MesAct_Anio1Ant_FechaFin = fechaFinMesAnio1Ant;
            DateTime Anio1Ant_MesAct_Final = fechaFinMesAnio1Ant;

            //pasamos valores al objeto FechasPR5
            objFecha.Anio1Ant = new PR5DatoAnio();
            objFecha.Anio1Ant.RangoAct_FechaIni = MesAct_Anio1Ant_FechaIni;
            objFecha.Anio1Ant.RangoAct_FechaFin = MesAct_Anio1Ant_FechaFin;

            objFecha.Anio1Ant.Fecha_01Enero = fecha01EneroAnio1Anterior;
            objFecha.Anio1Ant.Fecha_Inicial = fecha01MesAnio1Ant;
            objFecha.Anio1Ant.Fecha_Final = Anio1Ant_MesAct_Final;
            objFecha.Anio1Ant.Fecha_31Diciembre = fecha31DiciembreAnio1Anterior;

            objFecha.Anio1Ant.Sem01_FechaIni = fechaAnioAntSem01;  //No interviene hasta 2.1

            objFecha.Anio1Ant.NumAnio = anio1Anterior;
            objFecha.Anio1Ant.RangoAct_Num = EPDate.f_NombreMes(MesAct_Anio1Ant_FechaFin.Month);
            objFecha.Anio1Ant.RangoAct_NumYAnio = EPDate.f_NombreMes(MesAct_Anio1Ant_FechaFin.Month) + " " + anio1Anterior;

            objFecha.Anio1Ant.Ini_Data = objFecha.Anio1Ant.Sem01_FechaIni < objFecha.Anio1Ant.Fecha_01Enero ? objFecha.Anio1Ant.Sem01_FechaIni : objFecha.Anio1Ant.Fecha_01Enero;

            #endregion

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////            
            #region 2 AÑOS ATRAS
            int anio2Anterior = anio1Anterior - 1;

            //1 de enero de año actual
            DateTime fecha01EneroAnio2Anterior = new DateTime(anio2Anterior, 1, 1);
            DateTime fecha31DiciembreAnio2Anterior = new DateTime(anio2Anterior, 12, 31);

            //primer día de la primera semana del año actual
            DateTime fechaAnio2AntSem01 = EPDate.f_fechainiciosemana(anio2Anterior, 1);


            //primer dia y último día del mes Anterior anio actual
            DateTime fecha01_Mes1Ant_AnioAct = new DateTime(anioActual, mesActual, 1).AddMonths(-1);
            //DateTime fechaFin_Mes1Ant_AnioAct = fecha01_Mes1Ant_AnioAct.AddMonths(1).AddDays(-1);
            DateTime fechaFin_Mes1Ant_AnioAct = fecha01_Mes1Ant_AnioAct.AddMonths(1).AddMilliseconds(-1);

            DateTime Mes1Ant_AnioAct_FechaIni = fecha01_Mes1Ant_AnioAct;
            DateTime Mes1Ant_AnioAct_FechaFin = fechaFin_Mes1Ant_AnioAct;

            //primer dia y último día del mes Actual hace dos años
            DateTime fecha01MesAnio2Ant = new DateTime(anio2Anterior, mesActual, 1);
            //DateTime fechaFinMesAnio2Ant = fecha01MesAnio2Ant.AddMonths(1).AddDays(-1);
            DateTime fechaFinMesAnio2Ant = fecha01MesAnio2Ant.AddMonths(1).AddMilliseconds(-1);

            DateTime Anio2Ant_MesAct_Final = fechaFinMesAnio2Ant;

            //pasamos valores al objeto FechasPR5
            objFecha.Anio2Ant = new PR5DatoAnio();
            objFecha.Anio2Ant.RangoAct_FechaIni = Mes1Ant_AnioAct_FechaIni;
            objFecha.Anio2Ant.RangoAct_FechaFin = Mes1Ant_AnioAct_FechaFin;

            objFecha.Anio2Ant.Fecha_01Enero = fecha01EneroAnio2Anterior;
            objFecha.Anio2Ant.Fecha_Final = Anio2Ant_MesAct_Final;
            objFecha.Anio2Ant.Fecha_31Diciembre = fecha31DiciembreAnio2Anterior;

            objFecha.Anio2Ant.Sem01_FechaIni = fechaAnio2AntSem01; // no usado hasta 2.1

            objFecha.Anio2Ant.NumAnio = anio2Anterior;
            #endregion

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////            
            #region 3 AÑOS ATRAS
            int anio3Anterior = anio2Anterior - 1;

            //1 de enero de año actual
            DateTime fecha01EneroAnio3Anterior = new DateTime(anio3Anterior, 1, 1);
            DateTime fecha31DiciembreAnio3Anterior = new DateTime(anio3Anterior, 12, 31);

            //primer día de la primera semana del año actual
            DateTime fechaAnio3AntSem01 = EPDate.f_fechainiciosemana(anio3Anterior, 1);

            //primer dia y último día del mes 2Anterior anio actual
            DateTime fecha01_Mes2Ant_AnioAct = new DateTime(anioActual, mesActual, 1).AddMonths(-2);
            //DateTime fechaFin_Mes2Ant_AnioAct = fecha01_Mes2Ant_AnioAct.AddMonths(1).AddDays(-1);
            DateTime fechaFin_Mes2Ant_AnioAct = fecha01_Mes2Ant_AnioAct.AddMonths(1).AddMilliseconds(-1);

            DateTime Mes2Ant_AnioAct_FechaIni = fecha01_Mes2Ant_AnioAct;
            DateTime Mes2Ant_AnioAct_FechaFin = fechaFin_Mes2Ant_AnioAct;

            //primer dia y último día del mes Actual hace dos años
            DateTime fecha01MesAnio3Ant = new DateTime(anio3Anterior, mesActual, 1);
            //DateTime fechaFinMesAnio3Ant = fecha01MesAnio3Ant.AddMonths(1).AddDays(-1);
            DateTime fechaFinMesAnio3Ant = fecha01MesAnio3Ant.AddMonths(1).AddMilliseconds(-1);

            DateTime Anio3Ant_MesAct_Final = fechaFinMesAnio3Ant;

            //pasamos valores al objeto FechasPR5
            objFecha.Anio3Ant = new PR5DatoAnio();
            objFecha.Anio3Ant.RangoAct_FechaIni = Mes2Ant_AnioAct_FechaIni;
            objFecha.Anio3Ant.RangoAct_FechaFin = Mes2Ant_AnioAct_FechaFin;
            objFecha.Anio3Ant.Fecha_01Enero = fecha01EneroAnio3Anterior;
            objFecha.Anio3Ant.Fecha_Final = Anio3Ant_MesAct_Final;
            objFecha.Anio3Ant.Fecha_31Diciembre = fecha31DiciembreAnio3Anterior;

            objFecha.Anio3Ant.Sem01_FechaIni = fechaAnio3AntSem01; // no usado hasta 2.1

            objFecha.Anio3Ant.NumAnio = anio3Anterior;
            #endregion

            objFecha.ListaFechaBisiesto = UtilAnexoAPR5.ListarFechasBisiestoEnRango(objFecha.Anio3Ant.RangoAct_FechaIni.AddYears(-2), objFecha.AnioAct.RangoAct_FechaFin.AddYears(2));

            return objFecha;
        }

        public static FechasPR5 ObtenerFechasInformesAnualUltimoMes(DateTime fecIniAnioActual)
        {
            return ObtenerFechasInformesAnual(fecIniAnioActual);
        }

        public static FechasPR5 ObtenerFechasInformesAnual12Meses(DateTime fecIniAnioActual)
        {
            var objFecha = ObtenerFechasInformesAnual(fecIniAnioActual);
            objFecha.AnioAct.RangoAct_FechaIni = objFecha.AnioAct.Fecha_01Enero;
            objFecha.AnioAct.RangoAct_FechaFin = objFecha.AnioAct.Fecha_31Diciembre;

            objFecha.Anio1Ant.RangoAct_FechaIni = objFecha.Anio1Ant.Fecha_01Enero;
            objFecha.Anio1Ant.RangoAct_FechaFin = objFecha.Anio1Ant.Fecha_31Diciembre;

            objFecha.Anio2Ant.RangoAct_FechaIni = objFecha.Anio2Ant.Fecha_01Enero;
            objFecha.Anio2Ant.RangoAct_FechaFin = objFecha.Anio2Ant.Fecha_31Diciembre;

            objFecha.Anio3Ant.RangoAct_FechaIni = objFecha.Anio3Ant.Fecha_01Enero;
            objFecha.Anio3Ant.RangoAct_FechaFin = objFecha.Anio3Ant.Fecha_31Diciembre;

            return objFecha;
        }

        #region Informe Mensual

        public static string GetNombreArchivoInformeMensual(DateTime fechaInicio, int version)
        {
            string nombreMes = EPDate.f_NombreMes(fechaInicio.Month);
            var nombreArchivo = "";
            nombreArchivo = string.Format("Inf_{0}_{1}_v{2}_SGI{3}", nombreMes, fechaInicio.Year, version, ConstantesAppServicio.ExtensionExcel); //Inf_{nombreMes}_{año}_v{#version}_SGI
            return nombreArchivo;
        }

        public static string GetNombreArchivoInformeMensualIndividual(DateTime fechaInicio, int version)
        {
            string nombreMes = EPDate.f_NombreMes(fechaInicio.Month);
            var nombreArchivo = string.Format("Inf_{0}_{1}_", nombreMes, fechaInicio.Year) + "{0}" + string.Format("_v{0}_SGI{1}", version, ConstantesAppServicio.ExtensionExcel); //Inf_{nombreMes}_{año}_v{#version}_SGI
            return nombreArchivo;
        }

        #region Resumen e Interconexion

        public static void GenerarHojaResumenProduccionGeneracion(ExcelPackage xlPackage, string nameWS, DateTime fechaIni, DateTime fechaFin, DateTime fechaMD,
                                List<AbiProdgeneracionDTO> listaDetalleProduccion, bool incluirMD)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            int colIniTabla = 1;
            int rowIniTabla = 6;

            string font = "Calibri";
            string colorCeldaFijo = "#0070C0";
            string colorCeldaOsi = "#00B050";
            string colorTextoFijo = "#ffffff";

            string colorCeldaCuerpo = "#FFFFFF";
            string colorTextoCuerpo = "#000000";

            string colorLinea = "#000000";

            #region  Filtros y Cabecera

            int colEmpresa = colIniTabla;
            int colCentral = colEmpresa + 1;
            int colEquipo = colCentral + 1;
            int colTipo = colEquipo + 1;
            int colEnergiaH = colTipo + 1;
            int colEnergiaT = colEnergiaH + 1;
            int colEnergiaE = colEnergiaT + 1;
            int colEnergiaS = colEnergiaE + 1;
            int colMD = colEnergiaS + 1;

            int colUltimo = incluirMD ? colMD : colEnergiaS;

            int rowTitulo = 1;
            ws.Cells[rowTitulo, colCentral].Value = "Tabla Resumen";
            int rowFecha = rowTitulo + 2;
            ws.Cells[rowFecha, colCentral].Value = "INICIO";
            ws.Cells[rowFecha, colEquipo].Value = "FIN";
            ws.Cells[rowFecha + 1, colEmpresa].Value = "Rango de consulta";
            ws.Cells[rowFecha + 1, colCentral].Value = fechaIni.ToString(ConstantesAppServicio.FormatoFecha);
            ws.Cells[rowFecha + 1, colEquipo].Value = fechaFin.ToString(ConstantesAppServicio.FormatoFecha);

            if (incluirMD)
            {
                ws.Cells[rowFecha + 1, colEnergiaH].Value = "Máxima Demanda";
                ws.Cells[rowFecha + 1, colEnergiaT].Value = fechaMD.ToString(ConstantesAppServicio.FormatoFechaFull);
            }

            UtilExcel.SetFormatoCelda(ws, rowTitulo, colCentral, rowTitulo, colCentral, "Centro", "Izquierda", "#000000", "#FFFFFF", font, 18, true, true);
            UtilExcel.SetFormatoCelda(ws, rowFecha, colCentral, rowFecha, colEquipo, "Centro", "Centro", "#000000", "#FFFFFF", font, 12, true, true);

            int rowEmpresa = rowIniTabla + 1;
            ws.Cells[rowEmpresa, colEmpresa].Value = "EMPRESA";
            ws.Cells[rowEmpresa, colCentral].Value = "CENTRAL";
            ws.Cells[rowEmpresa, colEquipo].Value = "UNIDAD";
            ws.Cells[rowEmpresa, colTipo].Value = "TIPO COMBUSTIBLE";
            ws.Cells[rowEmpresa, colEnergiaH].Value = "E. Hidráulica \n MWh";
            ws.Cells[rowEmpresa, colEnergiaT].Value = "E. Térmica \n MWh";
            ws.Cells[rowEmpresa, colEnergiaE].Value = "E. Eolica \n MWh";
            ws.Cells[rowEmpresa, colEnergiaS].Value = "E. Solar \n MWh";
            if (incluirMD) ws.Cells[rowEmpresa, colMD].Value = "MW";

            UtilExcel.SetFormatoCelda(ws, rowEmpresa, colEmpresa, rowEmpresa, colUltimo, "Centro", "Centro", colorTextoFijo, colorCeldaFijo, font, 12, true, true);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowEmpresa, colEmpresa, rowEmpresa, colUltimo, colorLinea, true);

            //ws.Row(rowEmpresa).Height = 36;
            ws.Column(1).Width = 3;
            ws.Column(colEmpresa).Width = 40;
            ws.Column(colCentral).Width = 30;
            ws.Column(colEquipo).Width = 40;
            ws.Column(colTipo).Width = 40;
            ws.Column(colEnergiaH).Width = 20;
            ws.Column(colEnergiaT).Width = 20;
            ws.Column(colEnergiaE).Width = 20;
            ws.Column(colEnergiaS).Width = 20;
            ws.Column(colMD).Width = 20;

            #endregion

            #region Cuerpo

            int rowData = rowEmpresa;

            for (int i = 0; i < listaDetalleProduccion.Count; i++)
            {
                var reg = listaDetalleProduccion[i];

                rowData++;

                ws.Cells[rowData, colEmpresa].Value = reg.Emprnomb;
                ws.Cells[rowData, colCentral].Value = reg.Central;
                ws.Cells[rowData, colEquipo].Value = reg.Equinomb;
                ws.Cells[rowData, colTipo].Value = reg.Fenergnomb;
                ws.Cells[rowData, colEnergiaH].Value = reg.EnergiaH;
                ws.Cells[rowData, colEnergiaT].Value = reg.EnergiaT;
                ws.Cells[rowData, colEnergiaE].Value = reg.EnergiaE;
                ws.Cells[rowData, colEnergiaS].Value = reg.EnergiaS;
                if (incluirMD) ws.Cells[rowData, colMD].Value = reg.PotenciaMD;

                UtilExcel.SetFormatoCelda(ws, rowData, colEmpresa, rowData, colUltimo, "Centro", "Izquierda", colorTextoCuerpo, colorCeldaCuerpo, font, 12, false);
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowData, colEmpresa, rowData, colUltimo, "Centro");

                UtilExcel.BorderCeldasLineaDelgada(ws, rowData, colEmpresa, rowData, colUltimo, colorLinea, true);
            }

            #endregion

            //No mostrar lineas
            ws.View.ShowGridLines = false;

            //filter
            ws.Cells[rowEmpresa, colEmpresa, rowEmpresa, colUltimo].AutoFilter = true;

            ws.View.ZoomScale = 90;

            //excel con Font Arial
            var allCells = ws.Cells[1, 1, ws.Dimension.End.Row, ws.Dimension.End.Column];
            var cellFont = allCells.Style.Font;
            cellFont.Name = font;
        }

        public static void GenerarHojaDetalleInterconexion(ExcelPackage xlPackage, string nameWS, DateTime fechaIni, DateTime fechaFin, DateTime fechaMD,
                                List<InfSGIFilaResumenInterc> listaDetalleInterconexion, bool incluirMD)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            int colIniTabla = 1;
            int rowIniTabla = 6;

            string font = "Calibri";
            string colorCeldaFijo = "#0070C0";
            string colorTextoFijo = "#ffffff";

            string colorCeldaCuerpo = "#FFFFFF";
            string colorTextoCuerpo = "#000000";

            string colorLinea = "#000000";

            #region  Filtros y Cabecera

            int colFecha = colIniTabla;
            int colPto1 = colFecha + 1;
            int colPto2 = colPto1 + 1;
            int colPto3 = colPto2 + 1;
            int colPto4 = colPto3 + 1;

            int rowTitulo = 1;
            ws.Cells[rowTitulo, colPto1].Value = "Consulta Interconexión Perú-Ecuador";
            int rowFecha = rowTitulo + 2;
            ws.Cells[rowFecha, colPto1].Value = "INICIO";
            ws.Cells[rowFecha, colPto2].Value = "FIN";
            ws.Cells[rowFecha + 1, colFecha].Value = "Rango de consulta";
            ws.Cells[rowFecha + 1, colPto1].Value = fechaIni.ToString(ConstantesAppServicio.FormatoFecha);
            ws.Cells[rowFecha + 1, colPto2].Value = fechaFin.ToString(ConstantesAppServicio.FormatoFecha);

            if (incluirMD)
            {
                ws.Cells[rowFecha, colPto4].Value = "Máxima Demanda";
                ws.Cells[rowFecha + 1, colPto4].Value = fechaMD.ToString(ConstantesAppServicio.FormatoFechaFull);
            }

            UtilExcel.SetFormatoCelda(ws, rowTitulo, colPto1, rowTitulo, colPto1, "Centro", "Izquierda", "#000000", "#FFFFFF", font, 18, true);
            UtilExcel.SetFormatoCelda(ws, rowFecha, colPto1, rowFecha, colPto2, "Centro", "Centro", "#000000", "#FFFFFF", font, 12, true, true);

            int rowExp = rowIniTabla + 1;
            int rowLinea = rowExp + 1;
            int rowMedida = rowLinea + 1;
            ws.Cells[rowExp, colFecha].Value = "FECHA HORA";

            ws.Cells[rowExp, colPto1].Value = "EXPORTACIÓN";
            ws.Cells[rowExp, colPto2].Value = "IMPORTACIÓN";
            ws.Cells[rowExp, colPto3].Value = "EXPORTACIÓN";
            ws.Cells[rowExp, colPto4].Value = "IMPORTACIÓN";

            ws.Cells[rowLinea, colPto1].Value = "L-2280\n(ZORRITOS)";
            ws.Cells[rowLinea, colPto2].Value = "L-2280\n(ZORRITOS)";
            ws.Cells[rowLinea, colPto3].Value = "L-2280\n(ZORRITOS)";
            ws.Cells[rowLinea, colPto4].Value = "L-2280\n(ZORRITOS)";

            ws.Cells[rowMedida, colPto1].Value = "MWh";
            ws.Cells[rowMedida, colPto2].Value = "MWh";
            ws.Cells[rowMedida, colPto3].Value = "MVarh";
            ws.Cells[rowMedida, colPto4].Value = "MVarh";

            UtilExcel.CeldasExcelAgrupar(ws, rowExp, colFecha, rowMedida, colFecha);
            UtilExcel.SetFormatoCelda(ws, rowExp, colFecha, rowMedida, colPto4, "Centro", "Centro", colorTextoFijo, colorCeldaFijo, font, 12, true, true);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowExp, colFecha, rowMedida, colPto4, colorLinea, true, true);

            //ws.Row(rowEmpresa).Height = 36;
            ws.Column(1).Width = 3;
            ws.Column(colFecha).Width = 40;
            ws.Column(colPto1).Width = 25;
            ws.Column(colPto2).Width = 25;
            ws.Column(colPto3).Width = 25;
            ws.Column(colPto4).Width = 25;

            #endregion

            #region Cuerpo

            int rowData = rowMedida;

            for (int i = 0; i < listaDetalleInterconexion.Count; i++)
            {
                var reg = listaDetalleInterconexion[i];

                rowData++;

                ws.Cells[rowData, colFecha].Value = reg.FechaHoraDesc;
                ws.Cells[rowData, colPto1].Value = reg.EnergiaExp;
                ws.Cells[rowData, colPto2].Value = reg.EnergiaImp;
                ws.Cells[rowData, colPto3].Value = reg.ReactivaExp;
                ws.Cells[rowData, colPto4].Value = reg.ReactivaImp;

                string colorFila = incluirMD && reg.TieneMD ? "#93c4ff" : colorCeldaCuerpo;
                UtilExcel.SetFormatoCelda(ws, rowData, colFecha, rowData, colPto4, "Centro", "Izquierda", colorTextoCuerpo, colorFila, font, 12, false);
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowData, colFecha, rowData, colPto4, "Centro");

                UtilExcel.BorderCeldasLineaDelgada(ws, rowData, colFecha, rowData, colPto4, colorLinea, true);
            }

            #endregion

            //No mostrar lineas
            ws.View.ShowGridLines = false;

            ws.View.ZoomScale = 90;

            //excel con Font Arial
            var allCells = ws.Cells[1, 1, ws.Dimension.End.Row, ws.Dimension.End.Column];
            var cellFont = allCells.Style.Font;
            cellFont.Name = font;
        }

        #endregion

        #region Resúmen Relevante

        /// <summary>
        /// Genera los parrafos del resumen relevante mensual
        /// </summary>
        /// <param name="objFecha"></param>
        /// <param name="listaParticipacionRecursosEnergeticos"></param>
        /// <param name="listaMDTipoRecursoEnergeticoData"></param>
        /// <returns></returns>
        public static ReporteResumenRelevante GetTextoResumenRelevanteInformeMensual(FechasPR5 objFecha, List<ResultadoTotalGeneracion> listaParticipacionRREETexto,
                                                List<ResultadoTotalGeneracion> listaPotGenData)
        {
            NumberFormatInfo nfi = UtilAnexoAPR5.GenerarNumberFormatInfo1();
            NumberFormatInfo nfi2 = UtilAnexoAPR5.GenerarNumberFormatInfo2();
            NumberFormatInfo nfi3 = UtilAnexoAPR5.GenerarNumberFormatInfo3();

            var listaValoryParti = listaParticipacionRREETexto.Where(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct).ToList();
            var listaVar = listaParticipacionRREETexto.Where(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_Total_Var).ToList();

            //primer parrafo
            string energiaMesAct = UtilAnexoAPR5.ImprimirValorTotalHtml(listaPotGenData.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct).Meditotal, nfi2);

            decimal? valorDiferencialMesActual = listaPotGenData.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct_Resta).Meditotal;
            string textoDiferencialMesActual = UtilAnexoAPR5.ImprimirValorTotalHtml(Math.Abs(valorDiferencialMesActual.GetValueOrDefault(0)), nfi2);

            decimal? variacionMesAct = listaPotGenData.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct_Var).Meditotal;
            string textoVariacionMesAct = variacionMesAct > 0 ? "un incremento" : "una disminución";
            string varenergiaMesAct = UtilAnexoAPR5.ImprimirVariacionHtml(Math.Abs(variacionMesAct.GetValueOrDefault(0)), nfi2);

            string mesActual = EPDate.f_NombreMes(objFecha.AnioAct.RangoAct_FechaIni.Month).ToLower();
            var anioActual = objFecha.AnioAct.RangoAct_FechaIni.Year;
            var anioPasado = anioActual - 1;

            //segundo parrafo
            string energiaHidro = UtilAnexoAPR5.ImprimirValorTotalHtml(listaValoryParti.Find(x => x.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiAgua).TotalProducido, nfi2);

            decimal? variacionHidro = listaVar.Find(x => x.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiAgua).Meditotal;
            string textoVariacionHidro = variacionHidro > 0 ? "mayor" : "menor";
            string varEnergiaHidro = UtilAnexoAPR5.ImprimirVariacionHtml(Math.Abs(variacionHidro.GetValueOrDefault(0)), nfi2);

            //tercer parrafo
            string energiaCentralTermo = UtilAnexoAPR5.ImprimirValorTotalHtml(listaValoryParti.Find(x => x.Fenergcodi == ConstantesSiosein2.FenergcodiTermoelectrico).TotalProducido, nfi2);

            decimal? variacionCentralTermo = listaVar.Find(x => x.Fenergcodi == ConstantesSiosein2.FenergcodiTermoelectrico).Meditotal;
            string textoVariacionCentralTermo = variacionCentralTermo > 0 ? "mayor" : "menor";
            string varEnergiaCentralTermo = UtilAnexoAPR5.ImprimirVariacionHtml(Math.Abs(variacionCentralTermo.GetValueOrDefault(0)), nfi2);

            string parEnergiaCamisea = UtilAnexoAPR5.ImprimirVariacionHtml(listaValoryParti.Find(x => x.Fenergcodi == ConstantesSiosein2.FenergcodiGasCamisea).Meditotal, nfi2);
            string parEnergiaAgMalacas = UtilAnexoAPR5.ImprimirVariacionHtml(listaValoryParti.Find(x => x.Fenergcodi == ConstantesSiosein2.FenergcodiGasNoCamisea).Meditotal, nfi2);
            string parEnergiaDiesel = UtilAnexoAPR5.ImprimirVariacionHtml(listaValoryParti.Find(x => x.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiDiesel).Meditotal, nfi2);
            string parEnergiaResidual = UtilAnexoAPR5.ImprimirVariacionHtml(listaValoryParti.Find(x => x.Fenergcodi == ConstantesSiosein2.FenergcodiRelevanteResidual).Meditotal, nfi2);
            string parEnergiaCarbon = UtilAnexoAPR5.ImprimirVariacionHtml(listaValoryParti.Find(x => x.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiCarbon).Meditotal, nfi2);
            string parEnergiaBiogas = UtilAnexoAPR5.ImprimirVariacionHtml(listaValoryParti.Find(x => x.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiBiogas).Meditotal, nfi2);
            string parEnergiaBagazo = UtilAnexoAPR5.ImprimirVariacionHtml(listaValoryParti.Find(x => x.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiBagazo).Meditotal, nfi2);

            //cuarto parrafo
            string energiaEolico = UtilAnexoAPR5.ImprimirValorTotalHtml(listaValoryParti.Find(x => x.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiEolica).TotalProducido, nfi2);
            string energiaSolar = UtilAnexoAPR5.ImprimirValorTotalHtml(listaValoryParti.Find(x => x.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiSolar).TotalProducido, nfi2);

            string parEnergiaEolico = UtilAnexoAPR5.ImprimirVariacionHtml(listaValoryParti.Find(x => x.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiEolica).Meditotal, nfi2);
            string parEnergiaSolar = UtilAnexoAPR5.ImprimirVariacionHtml(listaValoryParti.Find(x => x.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiSolar).Meditotal, nfi2);

            #region Cuerpo Resumen
            ReporteResumenRelevante obj = new ReporteResumenRelevante();

            obj.TituloReporte = "INFORME DE LA OPERACIÓN MENSUAL";

            obj.ListaFechaTituloReporte = new List<string>();
            obj.ListaFechaTituloReporte.Add(string.Format("{0} {1}", EPDate.f_NombreMes(objFecha.AnioAct.RangoAct_FechaIni.Month).ToUpper(), objFecha.AnioAct.RangoAct_FechaIni.Year));//NOVIEMBRE 2018          

            obj.Subtitulo = "1. RESUMEN";
            obj.TituloParrafo1 = string.Format("1.1. Producción de energía eléctrica en {0} {1} en comparación al mismo mes del año anterior", EPDate.f_NombreMes(objFecha.AnioAct.RangoAct_FechaIni.Month).ToLower(), objFecha.AnioAct.RangoAct_FechaIni.Year);

            obj.ListaItemParrafo1 = new List<string>();
            obj.ListaItemParrafo1.Add(string.Format(@"El total de la producción de energía eléctrica de la empresas generadoras integrantes del COES en el mes de {0} {1} fue de {2} GWh, lo que representa {3} de {4} GWh ({5}) en comparación con el año {6}.            "
                , mesActual  //
                , anioActual //
                , energiaMesAct //
                , textoVariacionMesAct //
                , textoDiferencialMesActual
                , varenergiaMesAct //
                , anioPasado //
                ));

            obj.ListaItemParrafo1.Add(string.Format(@"La producción de electricidad con centrales hidroeléctricas durante el mes de {0} {1} fue de {2} GWh ({3} {4} al registrado durante {0} del año {5}).            "
                , mesActual  //
                , anioActual //
                , energiaHidro //
                , varEnergiaHidro //
                , textoVariacionHidro //
                , anioPasado //
                ));

            obj.ListaItemParrafo1.Add(string.Format(@"La producción de electricidad con centrales termoeléctricas durante el mes de {0} {1} fue de {2} GWh ({3} {4} al registrado durante {0} del año {5})."
                            + "La participación del  gas natural de Camisea fue de {6}, mientras que las del gas que proviene de los yacimientos de Aguaytía y Malacas fue del {7}, la producción con diesel, residual, carbón, biogás y bagazo tuvieron una intervención del {8}, {9}, {10}, {11}, {12} respectivamente."
                , mesActual  //
                , anioActual //
                , energiaCentralTermo //
                , textoVariacionCentralTermo //
                , varEnergiaCentralTermo //
                , anioPasado
                , parEnergiaCamisea
                , parEnergiaAgMalacas
                , parEnergiaDiesel
                , parEnergiaResidual
                , parEnergiaCarbon
                , parEnergiaBiogas
                , parEnergiaBagazo
                ));

            obj.ListaItemParrafo1.Add(string.Format(@"La producción de energía eléctrica con centrales eólicas fue de {0} GWh y con centrales solares fue de {1} GWh, los cuales tuvieron una participación de {2} y {3} respectivamente."
                , energiaEolico //  
                , energiaSolar //
                , parEnergiaEolico
                , parEnergiaSolar
                ));

            #endregion

            return obj;
        }

        /// <summary>
        /// Devuelve la vista de la tabla con el resumen temporal
        /// </summary>
        /// <param name="objTexto"></param>
        /// <returns></returns>
        public static string ReporteResumenMensualHtml(ReporteResumenRelevante objTexto)
        {

            //
            string plantillaHtml = @"
                        <!-Titulo-->
                        <h1 style='text-align: center;margin-top: 0px;'>{0}</h1>

                        <div style='    margin-top: 30px;    font-weight: bold;    text-align: center;    margin-bottom: 30px;'>
                        {1}
                        </div>

                        <h2 style='text-align: left; margin-bottom: 60px;'>{2}</h2>

                        <!--Parrafo-->
                        <div>
                        <h3>{3}</h3>
                        </div>

                        <div>                        
                        <h3>{4}</h3>
                        </div>
                        
                        <!--Lista de parrafo1-->
                        <ul>
                            {5}
                        </ul>
            ";

            string strItemsParrafo = string.Join("</li><li style='padding-bottom: 15px;padding-top: 15px;'>", objTexto.ListaItemParrafo1);
            strItemsParrafo = "<li>" + strItemsParrafo + "</li>";

            StringBuilder strHtml = new StringBuilder();
            strHtml.AppendFormat(plantillaHtml, objTexto.TituloReporte, objTexto.ListaFechaTituloReporte[0], objTexto.Subtitulo
                , objTexto.TituloParrafo1, objTexto.Parrafo1, strItemsParrafo);

            return strHtml.ToString();
        }

        /// <summary>
        /// Genera el texto del reporte Resumen Relevante
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="tipoVistaReporte"></param>
        /// <param name="objTexto"></param>
        /// <param name="filaFinTexto"></param>
        public static void GenerarExcelTextoResumenRelevanteMensual(ExcelWorksheet ws, FechasPR5 objFecha, ReporteResumenRelevante objTexto, out int filaFinTexto)
        {
            string tipoVistaReporte = objFecha.TipoVistaReporte;

            filaFinTexto = 7;
            string tituloReporte = "";
            string periodoReporte = "";
            string subtituloReporte = "";
            string tituloParrafo1 = "";
            string parrafoItem1Reporte = "";
            string parrafoItem2Reporte = "";
            string parrafoItem3Reporte = "";
            string parrafoItem4Reporte = "";

            if (objTexto != null)
            {
                tituloReporte = objTexto.TituloReporte;
                periodoReporte = objTexto.ListaFechaTituloReporte[0];
                subtituloReporte = objTexto.Subtitulo;
                tituloParrafo1 = objTexto.TituloParrafo1;
                parrafoItem1Reporte = objTexto.ListaItemParrafo1[0];
                parrafoItem2Reporte = objTexto.ListaItemParrafo1[1];
                parrafoItem3Reporte = objTexto.ListaItemParrafo1[2];
                parrafoItem4Reporte = objTexto.ListaItemParrafo1[3];

                int filaIniTitulo = 5;
                int coluIniTitulo = 3;

                ws.Cells[filaIniTitulo, coluIniTitulo].Value = tituloReporte;
                ws.Cells[filaIniTitulo + 2, coluIniTitulo].Value = periodoReporte;
                ws.Cells[filaIniTitulo + 6, coluIniTitulo].Value = subtituloReporte;
                ws.Cells[filaIniTitulo + 7, coluIniTitulo].Value = tituloParrafo1;
                ws.Cells[filaIniTitulo + 12, coluIniTitulo + 1].Value = parrafoItem1Reporte;
                ws.Cells[filaIniTitulo + 14, coluIniTitulo + 1].Value = parrafoItem2Reporte;
                ws.Cells[filaIniTitulo + 16, coluIniTitulo + 1].Value = parrafoItem3Reporte;
                ws.Cells[filaIniTitulo + 18, coluIniTitulo + 1].Value = parrafoItem4Reporte;

                filaFinTexto = 24;
            }
            else
            {
                UtilExcel.BorrarCeldasExcel(ws, 6, 3, 25, 20);
            }
        }

        #endregion

        #region 1.3. Potencia Instalada en el SEIN

        /// <summary>
        /// Genera la vista del reporte potencia instalada en el sein 
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="tablaData"></param>
        /// <returns></returns>
        public static string GenerarListadoPotenciaInstaladaSeinHtml(TablaReporte tablaData)
        {
            var dataCab = tablaData.Cabecera.CabeceraData;
            var registros = tablaData.ListaRegistros;

            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = UtilAnexoAPR5.GenerarNumberFormatInfo3();

            strHtml.Append("<div id='listado_reporte' style='width: 420px;'>");
            strHtml.Append("<table class='pretty tabla-icono' id=''>");

            #region cabecera

            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.AppendFormat("<th style='width: 160px;' >{0}</th>", dataCab[0, 0]);
            strHtml.AppendFormat("<th style='width: 200px;' >{0}</th>", dataCab[0, 1]);
            strHtml.AppendFormat("<th style='width: 200px;' >{0}</th>", dataCab[0, 2]);
            strHtml.AppendFormat("<th style='width: 200px;' >{0}</th>", dataCab[0, 3]);
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            #endregion

            #region cuerpo
            strHtml.Append("<tbody>");
            foreach (var reg in registros)
            {
                strHtml.Append("<tr>");
                var styleCelda = reg.EsFilaResumen ? "background-color: #95B3D7;" : "";
                strHtml.AppendFormat("<td style='{1}'>{0}</td>", reg.Nombre, styleCelda);

                int col = 0;
                foreach (decimal? valor in reg.ListaData)
                {
                    if (col != 2)
                        strHtml.AppendFormat("<td class='alignValorCenter' style='{1}'>{0}</td>", UtilAnexoAPR5.ImprimirValorTotalOcultar0Html(valor, nfi), styleCelda);
                    else
                        strHtml.AppendFormat("<td class='alignValorCenter' style='{1}'>{0}</td>", UtilAnexoAPR5.ImprimirVariacionHtml(valor, nfi), styleCelda);
                    col++;
                }

                strHtml.Append("</tr>");
            }
            strHtml.Append("</tbody>");

            #endregion

            strHtml.Append("</table>");

            strHtml.Append("</div>");

            foreach (var descripcion in tablaData.Leyenda.ListaDescripcion)
            {
                strHtml.AppendFormat("<div style='margin-top: 15px;'>{0}</div>", descripcion);
                strHtml.Append("</div>");
            }

            return strHtml.ToString();
        }

        /// <summary>
        /// Genera la data que será usada en el grafico del reporte "potencia instalada en el sein"
        /// </summary>
        /// <param name="tipoReporte"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="lstDataActual"></param>
        /// <param name="lstDataAnterior"></param>
        /// <returns></returns>
        public static GraficoWeb GenerarGWebPotenciaInstaladaSeinHtml(TablaReporte tablaData)
        {
            var dataCab = tablaData.Cabecera.CabeceraData;
            var registros = tablaData.ListaRegistros.Where(x => !x.EsFilaResumen).ToList();

            List<string> lstTipoGeneracion = registros.Select(x => x.Nombre).ToList();

            List<string> listaMeses = new List<string>();
            listaMeses.Add(dataCab[0, 2]);
            listaMeses.Add(dataCab[0, 1]);

            var graficoWeb = new GraficoWeb
            {
                Subtitle = tablaData.Leyenda.ListaDescripcion[0],
                Type = "column",
                TitleText = " ",
                XAxisCategories = listaMeses,
                YaxixTitle = "MW",
                TooltipValueSuffix = " MW",
                YaxixLabelsFormat = "",
                SerieData = new DatosSerie[lstTipoGeneracion.Count()]
            };

            string color = "";
            int indexS = 0;
            foreach (var tipoGen in lstTipoGeneracion)
            {
                if (indexS == 0) color = "#3B79C3";
                if (indexS == 1) color = "#C83C39";
                if (indexS == 2) color = "#35A135";
                if (indexS == 3) color = "#FFC000";

                graficoWeb.SerieData[indexS] = new DatosSerie { Name = tipoGen, Data = new decimal?[listaMeses.Count()], Color = color };

                int colData = 0;
                int colMes = 1;
                foreach (var mesDesc in listaMeses)
                {
                    graficoWeb.SerieData[indexS].Data[colData] = registros[indexS].ListaData[colMes];

                    colMes--;
                    colData++;
                }

                //para poder ordenarlo en el grafico excel
                graficoWeb.SerieData[indexS].Z = indexS;

                indexS++;
            }

            graficoWeb.SerieData = graficoWeb.SerieData.Reverse().ToArray();

            return graficoWeb;
        }

        /// <summary>
        /// Genera la data que será usada en la tabla del reporte "potencia instalada en el sein"
        /// </summary>
        /// <param name="tipoReporte"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="lstDataActual"></param>
        /// <param name="lstDataAnterior"></param>
        /// <returns></returns>
        public static TablaReporte ObtenerDataTablaPotenciaInstaladaSEIN(FechasPR5 objFecha, List<SiTipogeneracionDTO> listaTgeneracion, List<ResultadoTotalGeneracion> listaTgeneracionData)
        {
            TablaReporte tabla = new TablaReporte();

            #region Cabecera

            CabeceraReporte cabRepo = new CabeceraReporte();

            string[,] matrizCabecera = new string[1, 4];
            matrizCabecera[0, 0] = "POTENCIA INSTALADA (MW)";
            matrizCabecera[0, 1] = objFecha.EsReporteAnual ? objFecha.AnioAct.NumAnio.ToString() : objFecha.AnioAct.RangoAct_NumYAnio.ToString();
            matrizCabecera[0, 2] = objFecha.EsReporteAnual ? objFecha.Anio1Ant.NumAnio.ToString() : objFecha.Anio1Ant.RangoAct_NumYAnio.ToString();
            matrizCabecera[0, 3] = "Variación (%)";

            cabRepo.CabeceraData = matrizCabecera;

            tabla.Cabecera = cabRepo;

            #endregion

            #region Cuerpo

            List<RegistroReporte> registros = new List<RegistroReporte>();
            foreach (var regFila in listaTgeneracion)
            {
                var listaDataXFila = listaTgeneracionData.Where(x => x.Tgenercodi == regFila.Tgenercodi).ToList();

                RegistroReporte registro = new RegistroReporte();
                List<decimal?> datos = new List<decimal?>();

                ResultadoTotalGeneracion regMDAnio0G = listaDataXFila.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct);
                ResultadoTotalGeneracion regMDAnio1G = listaDataXFila.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnio1Ant_SemAct);
                ResultadoTotalGeneracion regMDVarAnio0G = listaDataXFila.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct_Var);

                datos.Add(regMDAnio0G.Meditotal);
                datos.Add(regMDAnio1G.Meditotal);
                datos.Add(regMDVarAnio0G.Meditotal);

                registro.Nombre = regFila.Tgenernomb;
                registro.ListaData = datos;
                registro.EsFilaResumen = regFila.Tgenercodi == 0;

                registros.Add(registro);
            }

            #endregion

            tabla.ListaRegistros = registros;

            // Adicionamos los pie de pagina
            var textoPie = "";
            PieReporte pie = new PieReporte();
            List<string> lstPie = new List<string>();

            if (objFecha.EsReporteAnual)
                textoPie = string.Format(NotasPieWebInformeMensual.Cuadro1_Reporte_1p3, "", (objFecha.AnioAct.RangoAct_FechaIni.Year - 1), objFecha.AnioAct.RangoAct_FechaIni.Year);
            else
                textoPie = string.Format(NotasPieWebInformeMensual.Cuadro1_Reporte_1p3, EPDate.f_NombreMes(objFecha.AnioAct.RangoAct_FechaIni.Month).ToLower(), (objFecha.AnioAct.RangoAct_FechaIni.Year - 1), objFecha.AnioAct.RangoAct_FechaIni.Year);

            lstPie.Add(textoPie);

            pie.ListaDescripcion = lstPie;
            tabla.Leyenda = pie;

            return tabla;
        }

        /// <summary>
        /// Genera los graficos excel del reporte "potencia instalada en el sein"
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="objFecha"></param>
        /// <param name="graficoOp"></param>
        /// <param name="ultFilaT"></param>
        /// <param name="ultFilaGrafico"></param>
        public static void GenerarCharExcelGraficosPotenciaInst(ExcelWorksheet ws, FechasPR5 objFecha, GraficoWeb graficoOp, int ultFilaT, out int ultFilaGrafico)
        {
            string tipoVistaReporte = objFecha.TipoVistaReporte;
            int tipoDoc = objFecha.TipoReporte;

            if (tipoVistaReporte == ConstantesPR5ReportesServicio.TipoVistaIndividual)
            {
                var miChart1 = ws.Drawings["graficoIngresoOpSein"] as ExcelChart;

                if (miChart1 != null)
                    miChart1.SetSize(0, 0);
            }

            int filaIniTabla = 60;
            int coluIniTabla = 27; //AA

            int ultimaFilaData = 0;
            int ultimaColuData = 0;
            ultFilaGrafico = 0;

            GraficoWeb miGrafico = graficoOp;
            var miChart = ws.Drawings["graficoPotenciaIns"] as ExcelChart;
            DatosSerie[] seriesG = miGrafico.SerieData;
            int numDatos = seriesG.Length;
            DatosSerie[] miSerieOrdenada = new DatosSerie[numDatos];

            foreach (var serie in seriesG)
            {
                numDatos--;
                miSerieOrdenada[numDatos] = serie;
            }

            miGrafico.SerieData = miSerieOrdenada;

            if (miGrafico.SerieData.Count() > 0)
            {

                //creamos la tabla a usar
                int colu1 = 0;
                foreach (var recursos in miGrafico.XAxisCategories)
                {
                    ws.Cells[filaIniTabla + 1, coluIniTabla + 1 + colu1].Value = recursos;
                    colu1++;
                }
                int numR = 0;
                int filaX = 0;
                foreach (var item in miGrafico.SerieData)
                {
                    ws.Cells[filaIniTabla + 2 + filaX, coluIniTabla].Value = item.Name;

                    numR = item.Data.Count();
                    int coluX = 0;
                    foreach (var valor in item.Data)
                    {
                        ws.Cells[filaIniTabla + 2 + filaX, coluIniTabla + 1 + coluX].Value = valor != 0 ? valor : null;
                        coluX++;
                    }

                    filaX++;
                }

                //setear valores
                miChart.SetPosition(ultFilaT + 5, 0, 2, 0);
                ultFilaGrafico = ultFilaT + 5 + 17;
                miChart.Title.Text = miGrafico.TitleText;

                ultimaFilaData = filaIniTabla + 2 + filaX - 1;
                ultimaColuData = coluIniTabla + 1 + numR - 1;


                //ingresamos rangos a las series
                for (int serie = 0; serie < filaX; serie++)
                {
                    miChart.Series[serie].Header = (string)ws.Cells[filaIniTabla + 2 + serie, coluIniTabla, filaIniTabla + 2 + serie, coluIniTabla].Value;
                    miChart.Series[serie].Series = ExcelRange.GetAddress(filaIniTabla + 2 + serie, coluIniTabla + 1, filaIniTabla + 2 + serie, ultimaColuData);
                    miChart.Series[serie].XSeries = ExcelRange.GetAddress(filaIniTabla + 1, coluIniTabla + 1, filaIniTabla + 1, ultimaColuData);
                }

                #region Texto_Anotacion_3
                string texto = "";

                texto = UtilAnexoAPR5.EscogerAnotacion("1.3", 2, tipoVistaReporte, tipoDoc);

                UtilExcel.FormatoNotaNegrita(ws, ultFilaGrafico + 1, 3, texto + miGrafico.Subtitle);

                #endregion

                //ocultamos textos
                UtilExcel.CeldasExcelColorTexto(ws, filaIniTabla, coluIniTabla, ultimaFilaData, ultimaColuData, "#ffffff");
                UtilExcel.CeldasExcelColorFondo(ws, filaIniTabla, coluIniTabla, ultimaFilaData, ultimaColuData, "#ffffff");
            }
            else
            {
                miChart.SetSize(0, 0);
                UtilExcel.BorrarCeldasExcel(ws, filaIniTabla, coluIniTabla, filaIniTabla + 6, coluIniTabla + 8);
            }
        }

        /// <summary>
        /// Genera la tabla excel del reporte "potencia instalada en el sein"
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="objFecha"></param>
        /// <param name="tablaData"></param>
        /// <param name="RowIni"></param>
        /// <param name="ultFila"></param>
        public static void GenerarCharExcelListadoPotenciaInst(ExcelWorksheet ws, FechasPR5 objFecha, TablaReporte tablaData, int RowIni, out int ultFila)
        {
            string tipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaGrupal;
            int tipoDoc = objFecha.TipoReporte;

            int filaIniCabe = 8 + RowIni;
            int coluIniCabe = 3;

            int filaIniData = filaIniCabe + 1;
            int coluIniData = coluIniCabe;

            int ultimaFila = 0;
            int ultimaColu = 0;

            #region Encabezado_Reporte
            UtilAnexoAPR5.IngresarEncabezadoGeneral(ws, objFecha);
            #endregion


            if (tipoVistaReporte == ConstantesPR5ReportesServicio.TipoVistaGrupal)
            {
                #region Titulo y subtitulo
                ws.Cells[filaIniCabe - 2, 3].Value = UtilSemanalPR5.EscogerTitulosSubtitulos("2", 3, tipoVistaReporte, tipoDoc, tablaData.ListaItem);  //subtitulo
                UtilAnexoAPR5.FormatoSubtituloExcel(ws, filaIniCabe - 2, 3);

                #endregion
            }

            var dataCab = tablaData.Cabecera.CabeceraData;
            var registros = tablaData.ListaRegistros;


            #region Cabecera
            ws.Cells[filaIniCabe, coluIniCabe].Value = dataCab[0, 0];
            ws.Cells[filaIniCabe, coluIniCabe + 1].Value = dataCab[0, 1];
            ws.Cells[filaIniCabe, coluIniCabe + 2].Value = dataCab[0, 2];
            ws.Cells[filaIniCabe, coluIniCabe + 3].Value = dataCab[0, 3];

            #region Formato Cabecera
            ws.Row(filaIniCabe).Height = 40;
            ws.Column(coluIniCabe).Width = 60;
            ws.Column(coluIniCabe + 1).Width = 23;
            ws.Column(coluIniCabe + 2).Width = 23;
            ws.Column(coluIniCabe + 3).Width = 23;
            ws.Column(coluIniCabe + 4).Width = 23;
            ws.Column(coluIniCabe + 5).Width = 23;

            ultimaColu = coluIniData + 3;

            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniCabe, coluIniCabe, filaIniCabe, ultimaColu, ConstantesPR5ReportesServicio.TipoLetraCabecera, ConstantesPR5ReportesServicio.TamLetraCabecera);
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniCabe, coluIniCabe, filaIniCabe, ultimaColu, "Centro");
            UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaIniCabe, coluIniCabe, filaIniCabe, ultimaColu, "Centro");
            UtilExcel.CeldasExcelEnNegrita(ws, filaIniCabe, coluIniCabe, filaIniCabe, ultimaColu);
            UtilExcel.CeldasExcelWrapText(ws, filaIniCabe, coluIniCabe, filaIniCabe, ultimaColu);
            UtilExcel.CeldasExcelColorFondo(ws, filaIniCabe, coluIniCabe, filaIniCabe, ultimaColu, ConstantesPR5ReportesServicio.ColorFondoCabInformeEjecutivoSem);
            UtilExcel.CeldasExcelColorTexto(ws, filaIniCabe, coluIniCabe, filaIniCabe, ultimaColu, "#FFFFFF");
            UtilExcel.BorderCeldas2(ws, filaIniCabe, coluIniCabe, filaIniCabe, ultimaColu);

            #endregion
            #endregion

            #region cuerpo

            if (registros.Any())
            {

                //***************************      CUERPO DE LA TABLA         ***********************************//     
                int filaX = 0;
                foreach (var reg in registros)
                {
                    ws.Cells[filaIniData + filaX, coluIniData].Value = reg.Nombre;

                    int col = 0;
                    foreach (decimal? valor in reg.ListaData)
                    {
                        if (valor != 0)
                        {
                            if (col != 2)

                                ws.Cells[filaIniData + filaX, coluIniData + 1 + col].Value = valor;
                            else
                                ws.Cells[filaIniData + filaX, coluIniData + 1 + col].Value = valor / 100.0m;
                        }

                        string strFormat = col != 2 ? ConstantesPR5ReportesServicio.FormatoNumero3Digito : ConstantesPR5ReportesServicio.FormatoNumero3DigitoPorcentaje;
                        ws.Cells[filaIniData + filaX, coluIniData + 1 + col].Style.Numberformat.Format = strFormat;

                        col++;
                    }

                    filaX++;
                }

                ultimaFila = filaIniData + filaX - 1;

                #region Formato Cuerpo
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniData, coluIniData, ultimaFila, ultimaColu, ConstantesPR5ReportesServicio.TipoLetraCuerpo, ConstantesPR5ReportesServicio.TamLetraCuerpo);
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniData, coluIniData, ultimaFila, coluIniData, "Izquierdo");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniData, coluIniData + 1, ultimaFila, ultimaColu, "Centro");
                UtilExcel.CeldasExcelWrapText(ws, filaIniData, coluIniData, ultimaFila, ultimaColu);
                UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaIniData, coluIniData, ultimaFila, ultimaColu, "Centro");
                UtilExcel.CeldasExcelEnNegrita(ws, ultimaFila, coluIniData, ultimaFila, ultimaColu);

                UtilExcel.BorderCeldas2(ws, filaIniData, coluIniData, ultimaFila, ultimaColu);
                #endregion

                ultFila = ultimaFila;

                #region Texto_Anotacion_3
                string texto = UtilAnexoAPR5.EscogerAnotacion("1.3", 1, tipoVistaReporte, tipoDoc);
                UtilExcel.FormatoNotaNegrita(ws, ultimaFila + 1, coluIniData, texto + tablaData.Leyenda.ListaDescripcion[0]);

                #endregion


            }
            else
            {
                ultFila = filaIniCabe + 1;
            }

            #endregion
        }

        #endregion

        #region 2.2 Producción por tipo de Recurso Energético

        #endregion

        #region 8.1 PRODUCCION DE ELECTRICIDAD MENSUAL POR EMPRESA

        public static List<GenericoDTO> ListarFilaCuadro8_1Gen()
        {
            var listaCuadro = new List<GenericoDTO> {
                                    new GenericoDTO(){ Entero1 = ConstantesSiosein2.FilaCuadroTotalGeneracion, String1 = "TOTAL MWh" },
                                }.ToList();

            return listaCuadro;
        }

        public static ResultadoTotalGeneracion ResumenProduccionObtenerDataMWhTotal(List<MeMedicion96DTO> listaData96, int tipoResultadoFecha, DateTime fechaProceso, DateTime fechaIniConsulta, DateTime fechaFinConsulta,
            int tgenercodi, int emprcodi, int equipadre, int tipoSemanaRelProd = 0)
        {
            ResultadoTotalGeneracion m = new ResultadoTotalGeneracion();
            m.Medifecha = fechaProceso;
            m.TipoResultadoFecha = tipoResultadoFecha;
            m.Meditotal = 0;

            m.Tgenercodi = tgenercodi;
            m.Emprcodi = emprcodi;
            m.Equipadre = equipadre;
            m.TipoSemanaRelProd = tipoSemanaRelProd;

            if (listaData96.Count > 0)
            {
                decimal total = 0;
                foreach (var aux in listaData96)
                {
                    total += aux.Meditotal.GetValueOrDefault(0);
                }

                m.Meditotal = total / 4.0m;
            }

            m.FiltroCeldaDato = new FiltroCeldaDato()
            {
                FechaIni = fechaIniConsulta,
                FechaFin = fechaFinConsulta,
            };

            return m;
        }

        public static TablaReporte ObtenerDataTablaResumenProduccionMensual(FechasPR5 objFecha, List<SiEmpresaDTO> listaEmpresa, List<EqEquipoDTO> listaCentral,
                List<ResultadoTotalGeneracion> listaTgen, List<ResultadoTotalGeneracion> listaEnergEjec,
                List<ResultadoTotalGeneracion> listaTotalTgen, List<ResultadoTotalGeneracion> listaTotalEnergEjec, List<ResultadoTotalGeneracion> listaTIEC3)
        {
            TablaReporte tabla = new TablaReporte();
            tabla.ReptiCodiTabla = objFecha.TipoReporte;
            CabeceraReporte cabRepo = new CabeceraReporte();

            string[,] matrizCabecera = new string[4, 7];

            matrizCabecera[0, 0] = "EMPRESA";
            matrizCabecera[0, 1] = "CENTRAL";
            matrizCabecera[0, 2] = string.Format("ENERGÍA PRODUCIDA {0}", objFecha.AnioAct.RangoAct_NumYAnio.ToUpper());
            matrizCabecera[0, 6] = "ANUAL";

            matrizCabecera[1, 2] = "GENERACIÓN";
            matrizCabecera[1, 5] = "TOTAL";
            matrizCabecera[1, 6] = "ACUMULADO";

            matrizCabecera[2, 2] = "HIDROELÉCTRICA";
            matrizCabecera[2, 3] = "TERMOELÉCTRICA";
            matrizCabecera[2, 4] = "RER(***)";
            matrizCabecera[2, 5] = objFecha.AnioAct.RangoAct_Num.ToUpper();
            matrizCabecera[2, 6] = objFecha.AnioAct.NumAnio.ToString();

            matrizCabecera[3, 2] = "MWh";
            matrizCabecera[3, 3] = "MWh";
            matrizCabecera[3, 4] = "MWh";
            matrizCabecera[3, 5] = "MWh";
            matrizCabecera[3, 6] = "MWh";

            cabRepo.CabeceraData = matrizCabecera;
            tabla.Cabecera = cabRepo;

            #region cuerpo
            List<RegistroReporte> registros = new List<RegistroReporte>();

            var listaFila = new List<EqEquipoDTO>();
            foreach (var regEmp in listaEmpresa)
            {
                var listaEqXEmp = listaCentral.Where(x => x.Emprcodi == regEmp.Emprcodi).ToList();
                int i = 0;
                foreach (var regGr in listaEqXEmp)
                {
                    string empFila = i == 0 ? regEmp.Emprnomb : "";
                    listaFila.Add(new EqEquipoDTO() { Emprcodi = 0, Equipadre = regGr.Equipadre, Emprnomb = empFila, Central = regGr.Central });
                    i++;
                }
                listaFila.Add(new EqEquipoDTO() { Emprcodi = regEmp.Emprcodi, Equipadre = 0, Emprnomb = string.Format("Total {0}", regEmp.Emprnomb), Central = "" });
            }

            //Por tipo de Generación
            foreach (var regFila in listaFila)
            {
                RegistroReporte registro = new RegistroReporte();
                List<decimal?> datos = new List<decimal?>();

                var listaXTgen = listaTgen.Where(x => x.Emprcodi == regFila.Emprcodi && x.Equipadre == regFila.Equipadre).ToList();

                ResultadoTotalGeneracion regTgenHidro = listaXTgen.Find(x => x.Tgenercodi == ConstantesPR5ReportesServicio.TgenercodiHidro);
                ResultadoTotalGeneracion regTgenTermo = listaXTgen.Find(x => x.Tgenercodi == ConstantesPR5ReportesServicio.TgenercodiTermo);
                ResultadoTotalGeneracion regTgenRER = listaXTgen.Find(x => x.Tgenercodi == ConstantesPR5ReportesServicio.TgenercodiRER);

                ResultadoTotalGeneracion regEnergEjecAnio0 = listaEnergEjec.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct && x.Emprcodi == regFila.Emprcodi && x.Equipadre == regFila.Equipadre);
                ResultadoTotalGeneracion regEnergAcumAnio0 = listaEnergEjec.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_Acum && x.Emprcodi == regFila.Emprcodi && x.Equipadre == regFila.Equipadre);

                datos.Add(regTgenHidro.Meditotal);
                datos.Add(regTgenTermo.Meditotal);
                datos.Add(regTgenRER.Meditotal);

                datos.Add(regEnergEjecAnio0.Meditotal);
                datos.Add(regEnergAcumAnio0.Meditotal);

                registro.Nombre = regFila.Emprnomb;
                registro.Nombre2 = regFila.Central;
                registro.ListaData = datos;

                registros.Add(registro);
            }

            //fila totales y TIE
            foreach (var regTotal in ListarFilaCuadro8_1Gen())
            {
                RegistroReporte registro = new RegistroReporte();
                List<decimal?> datos = new List<decimal?>();

                var listaXTgen = listaTotalTgen.Where(x => x.TipoSemanaRelProd == regTotal.Entero1).ToList();

                ResultadoTotalGeneracion regTgenHidro = listaXTgen.Find(x => x.Tgenercodi == ConstantesPR5ReportesServicio.TgenercodiHidro);
                ResultadoTotalGeneracion regTgenTermo = listaXTgen.Find(x => x.Tgenercodi == ConstantesPR5ReportesServicio.TgenercodiTermo);
                ResultadoTotalGeneracion regTgenRER = listaXTgen.Find(x => x.Tgenercodi == ConstantesPR5ReportesServicio.TgenercodiRER);

                ResultadoTotalGeneracion regEnergEjecAnio0 = listaTotalEnergEjec.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct && x.TipoSemanaRelProd == regTotal.Entero1.Value);
                ResultadoTotalGeneracion regEnergAcumAnio0 = listaTotalEnergEjec.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_Acum && x.TipoSemanaRelProd == regTotal.Entero1.Value);

                datos.Add(regTgenHidro.Meditotal);
                datos.Add(regTgenTermo.Meditotal);
                datos.Add(regTgenRER.Meditotal);

                datos.Add(regEnergEjecAnio0.Meditotal);
                datos.Add(regEnergAcumAnio0.Meditotal);

                registro.EsFilaResumen = true;
                registro.Nombre = regTotal.String1;
                registro.ListaData = datos;

                registros.Add(registro);
            }

            //Agregar 2 filas de Interconexion
            foreach (var regTotal in ListarFilaCuadro8_2TIE())
            {
                RegistroReporte registro = new RegistroReporte();
                List<decimal?> datos = new List<decimal?>();

                ResultadoTotalGeneracion regEnergEjecAnio0 = listaTIEC3.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct && x.TipoSemanaRelProd == regTotal.Entero1.Value);
                ResultadoTotalGeneracion regEnergAcumAnio0 = listaTIEC3.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_Acum && x.TipoSemanaRelProd == regTotal.Entero1.Value);

                datos.Add(null);
                datos.Add(null);
                datos.Add(null);

                datos.Add(regEnergEjecAnio0.Meditotal);
                datos.Add(regEnergAcumAnio0.Meditotal);

                registro.EsFilaResumen = true;
                registro.Nombre = regTotal.String1;
                registro.ListaData = datos;

                registros.Add(registro);
            }

            #endregion

            tabla.ListaRegistros = registros;

            return tabla;
        }

        public static string ListarResumenProduccionMensualHtml(TablaReporte tablaData)
        {
            var dataCab = tablaData.Cabecera.CabeceraData;
            var registros = tablaData.ListaRegistros;

            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi2 = UtilAnexoAPR5.GenerarNumberFormatInfo2();
            var tamTabla = 1170;

            strHtml.Append("<div id='listado_reporte' style='height: auto; width: " + tamTabla + "px;'>");

            strHtml.Append("<table id='reporte' class='pretty tabla-icono' style='width: " + (tamTabla - 30) + "px;'>");

            #region cabecera

            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.AppendFormat("<th rowspan='4' style='width: 285px;'>{0}</th>", dataCab[0, 0]);
            strHtml.AppendFormat("<th rowspan='4' style='width: 285px;'>{0}</th>", dataCab[0, 1]);
            strHtml.AppendFormat("<th colspan='4'>{0}</th>", dataCab[0, 2]);
            strHtml.AppendFormat("<th>{0}</th>", dataCab[0, 6].Replace("\n", "<br>"));
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.AppendFormat("<th colspan='3'>{0}</th>", dataCab[1, 2]);
            strHtml.AppendFormat("<th style='width:100px;'>{0}</th>", dataCab[1, 5]);
            strHtml.AppendFormat("<th style='width:100px;'>{0}</th>", dataCab[1, 6]);
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.AppendFormat("<th style=''>{0}</th>", dataCab[2, 2]);
            strHtml.AppendFormat("<th style=''>{0}</th>", dataCab[2, 3]);
            strHtml.AppendFormat("<th style=''>{0}</th>", dataCab[2, 4]);
            strHtml.AppendFormat("<th style=''>{0}</th>", dataCab[2, 5]);
            strHtml.AppendFormat("<th style=''>{0}</th>", dataCab[2, 6]);
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.AppendFormat("<th style=''>{0}</th>", dataCab[3, 2]);
            strHtml.AppendFormat("<th style=''>{0}</th>", dataCab[3, 3]);
            strHtml.AppendFormat("<th style=''>{0}</th>", dataCab[3, 4]);
            strHtml.AppendFormat("<th style=''>{0}</th>", dataCab[3, 5]);
            strHtml.AppendFormat("<th style=''>{0}</th>", dataCab[3, 6]);
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            #endregion

            #region cuerpo
            strHtml.Append("<tbody>");
            int f = 0;
            foreach (var reg in registros)
            {
                if (!reg.EsFilaResumen)
                {
                    string styleTotalEmp = !string.IsNullOrEmpty(reg.Nombre) && string.IsNullOrEmpty(reg.Nombre2) ? "background-color: #DDEBF7;" : "";

                    strHtml.Append("<tr>");
                    strHtml.AppendFormat("<td style='padding-left: 5px;text-align: left;font-weight:bold;{1}'>{0}</td>", reg.Nombre, styleTotalEmp);
                    strHtml.AppendFormat("<td style='padding-left: 5px;text-align: left;{1}'>{0}</td>", reg.Nombre2, styleTotalEmp);

                    int c = 0;
                    foreach (decimal? col in reg.ListaData)
                    {
                        strHtml.AppendFormat("<td class='alignValorRight' style='{1}'>{0}</td>", UtilAnexoAPR5.ImprimirValorTotalOcultar0Html(col.HasValue ? col : null, nfi2), styleTotalEmp);

                        c++;
                    }
                }
                strHtml.Append("</tr>");
                f++;
            }
            strHtml.Append("</tbody>");

            strHtml.Append("<thead>");
            foreach (var reg in registros)
            {
                if (reg.EsFilaResumen)
                {
                    strHtml.Append("<tr>");
                    strHtml.AppendFormat("<th colspan='2' style='padding-left: 5px;text-align: left;font-weight:bold;'>{0}</t>", reg.Nombre);

                    int c = 0;
                    foreach (decimal? col in reg.ListaData)
                    {
                        strHtml.AppendFormat("<th class='alignValorRight' >{0}</th>", UtilAnexoAPR5.ImprimirValorTotalOcultar0Html(col.HasValue ? col : null, nfi2));

                        c++;
                    }

                    strHtml.Append("</tr>");
                }
            }
            strHtml.Append("</thead>");
            strHtml.Append("</table>");

            #endregion

            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Resumen Produccion
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="objFecha"></param>
        /// <param name="tablaData"></param>
        /// <param name="ultimaFilaTabla"></param>
        public static void GenerarCharExcelResumenProduccionMensual(ExcelWorksheet ws, FechasPR5 objFecha, TablaReporte tablaData, int filaIniEmpresa, out int ultimaFilaTabla)
        {
            string tipoVistaReporte = objFecha.TipoVistaReporte;
            int tipoDoc = objFecha.TipoReporte;

            var dataCab = tablaData.Cabecera.CabeceraData;
            var registrosDetalle = tablaData.ListaRegistros.Where(x => !x.EsFilaResumen).ToList();
            var registrosTotal = tablaData.ListaRegistros.Where(x => x.EsFilaResumen).ToList();

            int coluIniEmpresa = 1;
            ultimaFilaTabla = 0;

            //iterar por cada subcuadro
            if (registrosDetalle.Any())
            {
                int filaIniData = filaIniEmpresa + 4;
                int coluIniData = coluIniEmpresa;

                int ultimaFila = 0;
                int ultimaColu = 0;

                #region cabecera
                ws.Cells[filaIniEmpresa, coluIniEmpresa + 0].Value = dataCab[0, 0];
                ws.Cells[filaIniEmpresa, coluIniEmpresa + 1].Value = dataCab[0, 1];
                ws.Cells[filaIniEmpresa, coluIniEmpresa + 2].Value = dataCab[0, 2];
                ws.Cells[filaIniEmpresa, coluIniEmpresa + 6].Value = dataCab[0, 6];

                ws.Cells[filaIniEmpresa + 1, coluIniEmpresa + 2].Value = dataCab[1, 2];
                ws.Cells[filaIniEmpresa + 1, coluIniEmpresa + 5].Value = dataCab[1, 5];
                ws.Cells[filaIniEmpresa + 1, coluIniEmpresa + 6].Value = dataCab[1, 6];

                ws.Cells[filaIniEmpresa + 2, coluIniEmpresa + 2].Value = dataCab[2, 2];
                ws.Cells[filaIniEmpresa + 2, coluIniEmpresa + 3].Value = dataCab[2, 3];
                ws.Cells[filaIniEmpresa + 2, coluIniEmpresa + 4].Value = dataCab[2, 4];
                ws.Cells[filaIniEmpresa + 2, coluIniEmpresa + 5].Value = dataCab[2, 5];
                ws.Cells[filaIniEmpresa + 2, coluIniEmpresa + 6].Value = dataCab[2, 6];

                ws.Cells[filaIniEmpresa + 3, coluIniEmpresa + 2].Value = dataCab[3, 2];
                ws.Cells[filaIniEmpresa + 3, coluIniEmpresa + 3].Value = dataCab[3, 3];
                ws.Cells[filaIniEmpresa + 3, coluIniEmpresa + 4].Value = dataCab[3, 4];
                ws.Cells[filaIniEmpresa + 3, coluIniEmpresa + 5].Value = dataCab[3, 5];
                ws.Cells[filaIniEmpresa + 3, coluIniEmpresa + 6].Value = dataCab[3, 6];

                #endregion

                ultimaColu = coluIniEmpresa + 6;

                #region cuerpo

                ultimaFila = filaIniData + registrosDetalle.Count() - 1;
                ultimaFilaTabla = ultimaFila;

                #region Formato Cuerpo
                //UtilExcel.CeldasExcelWrapText(ws, filaIniData, coluIniData, ultimaFila, coluIniData);
                UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaIniData, coluIniData + 1, ultimaFila, ultimaColu, "Centro");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniData, coluIniData + 0, ultimaFila, coluIniData + 1, "Izquierda");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniData, coluIniData + 2, ultimaFila, ultimaColu, "Derecha");
                UtilExcel.CeldasExcelEnNegrita(ws, filaIniData, coluIniData + 0, ultimaFila, coluIniData + 0);
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniData, coluIniData, ultimaFila, ultimaColu, ConstantesPR5ReportesServicio.TipoLetraCuerpo, ConstantesPR5ReportesServicio.TamLetraCuerpo3);

                #endregion

                int filaX = 0;
                foreach (var reg in registrosDetalle)
                {
                    int colX = 0;

                    ws.Cells[filaIniData + filaX, coluIniData + colX].Value = reg.Nombre;
                    colX++;
                    if (!string.IsNullOrEmpty(reg.Nombre2))
                        ws.Cells[filaIniData + filaX, coluIniData + colX].Value = reg.Nombre2;
                    colX++;

                    if (!string.IsNullOrEmpty(reg.Nombre))
                    {
                        UtilExcel.CeldasExcelEnNegrita(ws, filaIniData + filaX, coluIniData + 0, filaIniData + filaX, coluIniData + 0);
                    }

                    if (!string.IsNullOrEmpty(reg.Nombre) && string.IsNullOrEmpty(reg.Nombre2))
                    {
                        string colorTotalEmp = "#DDEBF7";
                        UtilExcel.CeldasExcelColorFondo(ws, filaIniData + filaX, coluIniData + 0, filaIniData + filaX, ultimaColu, colorTotalEmp);
                        UtilExcel.CeldasExcelColorFondoYBorderSoloUnLado(ws, filaIniData + filaX, coluIniData + 0, filaIniData + filaX, ultimaColu, "#0000D0", "Abajo");
                    }

                    foreach (decimal? numValor in reg.ListaData)
                    {
                        string strFormat = ConstantesPR5ReportesServicio.FormatoNumero2Digito;

                        if (numValor != null)
                        {
                            ws.Cells[filaIniData + filaX, coluIniData + colX].Value = numValor;
                        }
                        ws.Cells[filaIniData + filaX, coluIniData + colX].Style.Numberformat.Format = strFormat;

                        colX++;
                    }

                    filaX++;
                }
                #endregion
            }

            //Filas resumen
            if (registrosTotal.Any())
            {
                int filaRes1 = filaIniEmpresa + 4 + registrosDetalle.Count();
                int filaResFin = filaRes1 + 2;
                int filaX = filaRes1;
                int coluIniData = coluIniEmpresa;
                int ultimaColu = coluIniEmpresa + 6;

                ultimaFilaTabla = filaResFin;

                foreach (var reg in registrosTotal)
                {
                    int colX = 0;

                    ws.Cells[filaX, coluIniData + colX].Value = reg.Nombre;
                    colX += 2; ;

                    foreach (decimal? numValor in reg.ListaData)
                    {
                        string strFormat = ConstantesPR5ReportesServicio.FormatoNumero2Digito;

                        if (numValor != null && numValor != 0)
                        {
                            ws.Cells[filaX, coluIniData + colX].Value = numValor;
                        }
                        ws.Cells[filaX, coluIniData + colX].Style.Numberformat.Format = strFormat;

                        colX++;
                    }
                    filaX++;
                }

                #region Formato Cuerpo
                UtilExcel.CeldasExcelWrapText(ws, filaRes1, coluIniData, filaResFin, ultimaColu);
                UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaRes1, coluIniData, filaResFin, ultimaColu, "Centro");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaRes1, coluIniData, filaResFin, coluIniData + 1, "Izquierda");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaRes1, coluIniData + 2, filaResFin, ultimaColu, "Derecha");
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaRes1, coluIniData, filaResFin, ultimaColu, ConstantesPR5ReportesServicio.TipoLetraCuerpo, ConstantesPR5ReportesServicio.TamLetraCuerpo3);

                UtilExcel.CeldasExcelColorTexto(ws, filaRes1, coluIniData, filaResFin, ultimaColu, "#FFFFFF");
                UtilExcel.CeldasExcelColorFondo(ws, filaRes1, coluIniData, filaResFin, ultimaColu, "#0077A5");

                UtilExcel.BorderCeldas2(ws, filaRes1, coluIniData, filaResFin, ultimaColu);

                UtilExcel.CeldasExcelEnNegrita(ws, filaRes1, coluIniData, filaResFin, ultimaColu);

                #endregion
            }
        }

        #endregion

        #region 8.2 MÁXIMA POTENCIA COINCIDENTE MENSUAL

        public static List<GenericoDTO> ListarFilaCuadro8_2Gen()
        {
            var listaCuadro = new List<GenericoDTO> {
                                    new GenericoDTO(){ Entero1 = ConstantesSiosein2.FilaCuadroTotalGeneracion, String1 = "TOTAL MÁXIMA POTENCIA COINCIDENTE" },
                                }.ToList();

            return listaCuadro;
        }

        public static List<GenericoDTO> ListarFilaCuadro8_2TIE()
        {
            var listaCuadro = new List<GenericoDTO> {
                                    new GenericoDTO(){ Entero1 = ConstantesSiosein2.FilaCuadroImportacion, String1 = "IMPORTACIÓN " },
                                    new GenericoDTO(){ Entero1 = ConstantesSiosein2.FilaCuadroExportacion, String1 = "EXPORTACIÓN" },
                                }.ToList();

            return listaCuadro;
        }

        public static List<GenericoDTO> ListarFilaCuadro8_2Sein()
        {
            var listaCuadro = new List<GenericoDTO> {
                                    new GenericoDTO(){ Entero1 = ConstantesSiosein2.FilaCuadroTotalSein, String1 = "TOTAL (CONSIDERANDO LA IMPORTACIÓN)" },
                                }.ToList();

            return listaCuadro;
        }

        public static TablaReporte ObtenerDataTablaMaximaDemandaMensual(FechasPR5 objFecha, List<MaximaDemandaDTO> listaMDCoincidenteDataDesc,
                        List<SiEmpresaDTO> listaEmpresa, List<EqEquipoDTO> listaCentral,
                        List<ResultadoTotalGeneracion> listaMDXCentral, List<ResultadoTotalGeneracion> listaMDXEmpresa,
                        List<ResultadoTotalGeneracion> listaTIEMD, List<ResultadoTotalGeneracion> listaMDTotal)
        {
            var regMDAct = listaMDCoincidenteDataDesc.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct);
            var regMDAnt = listaMDCoincidenteDataDesc.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnio1Ant_SemAct);
            var regMDActAcum = listaMDCoincidenteDataDesc.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_Acum);

            TablaReporte tabla = new TablaReporte();
            tabla.ReptiCodiTabla = objFecha.TipoReporte;
            CabeceraReporte cabRepo = new CabeceraReporte();

            string[,] matrizCabecera = new string[4, 6];

            matrizCabecera[0, 0] = "Empresa";
            matrizCabecera[0, 1] = "Central";
            matrizCabecera[0, 2] = "Máxima potencia coincidente (MW)";

            matrizCabecera[1, 2] = string.Format("{0}-{1}", EPDate.f_NombreMesCorto(objFecha.AnioAct.NumMes), objFecha.AnioAct.NumAnio);
            matrizCabecera[1, 3] = string.Format("{0}-{1}", EPDate.f_NombreMesCorto(objFecha.Anio1Ant.NumMes), objFecha.Anio1Ant.NumAnio);
            matrizCabecera[1, 4] = objFecha.AnioAct.NumAnio.ToString();
            matrizCabecera[1, 5] = string.Format("{0} / {1}", objFecha.AnioAct.NumAnio, objFecha.Anio1Ant.NumAnio);

            matrizCabecera[2, 2] = regMDAct.FechaOnlyDia;
            matrizCabecera[2, 3] = regMDAnt.FechaOnlyDia;
            matrizCabecera[2, 4] = regMDActAcum.FechaOnlyDia;
            matrizCabecera[2, 5] = "Variación";

            matrizCabecera[3, 2] = regMDAct.FechaOnlyHora;
            matrizCabecera[3, 3] = regMDAnt.FechaOnlyHora;
            matrizCabecera[3, 4] = regMDActAcum.FechaOnlyHora;
            matrizCabecera[3, 5] = "%";

            cabRepo.CabeceraData = matrizCabecera;
            tabla.Cabecera = cabRepo;

            #region cuerpo
            List<RegistroReporte> registros = new List<RegistroReporte>();

            var listaFila = new List<EqEquipoDTO>();
            foreach (var regEmp in listaEmpresa)
            {
                var listaEqXEmp = listaCentral.Where(x => x.Emprcodi == regEmp.Emprcodi).ToList();
                int i = 0;
                foreach (var regGr in listaEqXEmp)
                {
                    string empFila = i == 0 ? regEmp.Emprnomb : "";
                    listaFila.Add(new EqEquipoDTO() { Emprcodi = 0, Equipadre = regGr.Equipadre, Emprnomb = empFila, Central = regGr.Central });
                    i++;
                }
                listaFila.Add(new EqEquipoDTO() { Emprcodi = regEmp.Emprcodi, Equipadre = 0, Emprnomb = string.Format("Total {0}", regEmp.Emprnomb), Central = "" });
            }

            //filas de centrales y empresas
            foreach (var regFila in listaFila)
            {
                RegistroReporte registro = new RegistroReporte();
                List<decimal?> datos = new List<decimal?>();

                var listaDataXFila = regFila.Equipadre > 0 ? listaMDXCentral.Where(x => x.Equipadre == regFila.Equipadre).ToList() : listaMDXEmpresa.Where(x => x.Emprcodi == regFila.Emprcodi).ToList();

                ResultadoTotalGeneracion regMDAnio0G = listaDataXFila.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct);
                ResultadoTotalGeneracion regMDAnio1G = listaDataXFila.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnio1Ant_SemAct);
                ResultadoTotalGeneracion regMDAnio0AcumG = listaDataXFila.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_Acum);
                ResultadoTotalGeneracion regMDVarAnio0G = listaDataXFila.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct_Var);

                datos.Add(regMDAnio0G.Meditotal);
                datos.Add(regMDAnio1G.Meditotal);
                datos.Add(regMDAnio0AcumG.Meditotal);
                datos.Add(regMDVarAnio0G.Meditotal);

                registro.Nombre = regFila.Emprnomb;
                registro.Nombre2 = regFila.Central;
                registro.ListaData = datos;

                registros.Add(registro);
            }

            //fila totales y TIE
            List<GenericoDTO> listaFilaUltimo = new List<GenericoDTO>();
            listaFilaUltimo.AddRange(ListarFilaCuadro8_2Gen());
            listaFilaUltimo.AddRange(ListarFilaCuadro8_2TIE());
            listaFilaUltimo.AddRange(ListarFilaCuadro8_2Sein());
            foreach (var regFila in listaFilaUltimo)
            {
                RegistroReporte registro = new RegistroReporte();
                List<decimal?> datos = new List<decimal?>();

                var listaDataXFila = new List<ResultadoTotalGeneracion>();
                if (regFila.Entero1 == ConstantesSiosein2.FilaCuadroTotalGeneracion) listaDataXFila = listaMDTotal.Where(x => x.TipoSemanaRelProd == regFila.Entero1.Value).ToList();
                if (regFila.Entero1 == ConstantesSiosein2.FilaCuadroImportacion) listaDataXFila = listaTIEMD.Where(x => x.TipoSemanaRelProd == regFila.Entero1.Value).ToList();
                if (regFila.Entero1 == ConstantesSiosein2.FilaCuadroExportacion) listaDataXFila = listaTIEMD.Where(x => x.TipoSemanaRelProd == regFila.Entero1.Value).ToList();
                if (regFila.Entero1 == ConstantesSiosein2.FilaCuadroTotalSein) listaDataXFila = listaMDTotal.Where(x => x.TipoSemanaRelProd == regFila.Entero1.Value).ToList();

                ResultadoTotalGeneracion regMDAnio0G = listaDataXFila.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct);
                ResultadoTotalGeneracion regMDAnio1G = listaDataXFila.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnio1Ant_SemAct);
                ResultadoTotalGeneracion regMDAnio0AcumG = listaDataXFila.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_Acum);
                ResultadoTotalGeneracion regMDVarAnio0G = listaDataXFila.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct_Var);

                datos.Add(regMDAnio0G.Meditotal);
                datos.Add(regMDAnio1G.Meditotal);
                datos.Add(regMDAnio0AcumG.Meditotal);
                datos.Add(regMDVarAnio0G.Meditotal);

                registro.Nombre = regFila.String1;
                registro.EsFilaResumen = true;

                registro.ListaData = datos;

                registros.Add(registro);
            }

            #endregion

            tabla.ListaRegistros = registros;

            return tabla;
        }

        public static string ListarResumenMaximaDemandaMensualHtml(TablaReporte tablaData)
        {
            var dataCab = tablaData.Cabecera.CabeceraData;
            var registros = tablaData.ListaRegistros;

            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi2 = UtilAnexoAPR5.GenerarNumberFormatInfo2();
            var tamTabla = 1170;

            strHtml.Append("<div id='listado_reporte' style='height: auto; width: " + tamTabla + "px;'>");

            strHtml.Append("<table id='reporte' class='pretty tabla-icono' style='width: " + (tamTabla - 30) + "px;'>");

            #region cabecera

            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.AppendFormat("<th rowspan='4' style='width: 285px;'>{0}</th>", dataCab[0, 0]);
            strHtml.AppendFormat("<th rowspan='4' style='width: 285px;'>{0}</th>", dataCab[0, 1]);
            strHtml.AppendFormat("<th colspan='4'>{0}</th>", dataCab[0, 2]);
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.AppendFormat("<th style='width:100px;'>{0}</th>", dataCab[1, 2]);
            strHtml.AppendFormat("<th style='width:100px;'>{0}</th>", dataCab[1, 3]);
            strHtml.AppendFormat("<th style='width:100px;'>{0}</th>", dataCab[1, 4]);
            strHtml.AppendFormat("<th style='width:100px;'>{0}</th>", dataCab[1, 5]);
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.AppendFormat("<th style='width:100px;'>{0}</th>", dataCab[2, 2]);
            strHtml.AppendFormat("<th style='width:100px;'>{0}</th>", dataCab[2, 3]);
            strHtml.AppendFormat("<th style='width:100px;'>{0}</th>", dataCab[2, 4]);
            strHtml.AppendFormat("<th style='width:100px;'>{0}</th>", dataCab[2, 5]);
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.AppendFormat("<th style='width:100px;'>{0}</th>", dataCab[3, 2]);
            strHtml.AppendFormat("<th style='width:100px;'>{0}</th>", dataCab[3, 3]);
            strHtml.AppendFormat("<th style='width:100px;'>{0}</th>", dataCab[3, 4]);
            strHtml.AppendFormat("<th style='width:100px;'>{0}</th>", dataCab[3, 5]);
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            #endregion

            #region cuerpo
            strHtml.Append("<tbody>");
            int f = 0;
            foreach (var reg in registros)
            {
                if (!reg.EsFilaResumen)
                {
                    string styleTotalEmp = !string.IsNullOrEmpty(reg.Nombre) && string.IsNullOrEmpty(reg.Nombre2) ? "background-color: #DDEBF7;" : "";

                    strHtml.Append("<tr>");
                    strHtml.AppendFormat("<td style='padding-left: 5px;text-align: left;font-weight:bold;{1}'>{0}</td>", reg.Nombre, styleTotalEmp);
                    strHtml.AppendFormat("<td style='padding-left: 5px;text-align: left;{1}'>{0}</td>", reg.Nombre2, styleTotalEmp);

                    int c = 2;
                    foreach (decimal? col in reg.ListaData)
                    {
                        if (c == 5) //con signo  de %
                            strHtml.AppendFormat("<td class='alignValorRight' style='{1}'>{0}</td>", UtilAnexoAPR5.ImprimirVariacionHtml(col.HasValue ? col : null, nfi2), styleTotalEmp);
                        else
                            strHtml.AppendFormat("<td class='alignValorRight' style='{1}'>{0}</td>", UtilAnexoAPR5.ImprimirValorTotalOcultar0Html(col.HasValue ? col : null, nfi2), styleTotalEmp);

                        c++;
                    }
                }
                strHtml.Append("</tr>");
                f++;
            }
            strHtml.Append("</tbody>");

            strHtml.Append("<thead>");
            foreach (var reg in registros)
            {
                if (reg.EsFilaResumen)
                {
                    strHtml.Append("<tr>");
                    strHtml.AppendFormat("<th colspan='2' style='padding-left: 5px;text-align: left;font-weight:bold;'>{0}</t>", reg.Nombre);

                    int c = 2;
                    foreach (decimal? col in reg.ListaData)
                    {
                        if (c == 5) //con signo  de %
                            strHtml.AppendFormat("<th class='alignValorRight' style=''>{0}</th>", UtilAnexoAPR5.ImprimirVariacionHtml(col.HasValue ? col : null, nfi2));
                        else
                            strHtml.AppendFormat("<th class='alignValorRight' style=''>{0}</th>", UtilAnexoAPR5.ImprimirValorTotalOcultar0Html(col.HasValue ? col : null, nfi2));

                        c++;
                    }

                    strHtml.Append("</tr>");
                }
            }
            strHtml.Append("</thead>");
            strHtml.Append("</table>");

            #endregion

            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        public static void GenerarCharExcelResumenMaximaDemandaMensual(ExcelWorksheet ws, FechasPR5 objFecha, TablaReporte tablaData, int filaIniEmpresa, out int ultimaFilaTabla)
        {
            string tipoVistaReporte = objFecha.TipoVistaReporte;
            int tipoDoc = objFecha.TipoReporte;

            var dataCab = tablaData.Cabecera.CabeceraData;
            var registrosDetalle = tablaData.ListaRegistros.Where(x => !x.EsFilaResumen).ToList();
            var registrosTotal = tablaData.ListaRegistros.Where(x => x.EsFilaResumen).ToList();

            int coluIniEmpresa = 1;
            ultimaFilaTabla = 0;

            //iterar por cada subcuadro
            if (registrosDetalle.Any())
            {
                int filaIniData = filaIniEmpresa + 4;
                int coluIniData = coluIniEmpresa;

                int ultimaFila = 0;
                int ultimaColu = 0;

                #region cabecera
                ws.Cells[filaIniEmpresa, coluIniEmpresa + 0].Value = dataCab[0, 0];
                ws.Cells[filaIniEmpresa, coluIniEmpresa + 1].Value = dataCab[0, 1];
                ws.Cells[filaIniEmpresa, coluIniEmpresa + 2].Value = dataCab[0, 2];

                ws.Cells[filaIniEmpresa + 1, coluIniEmpresa + 2].Value = dataCab[1, 2];
                ws.Cells[filaIniEmpresa + 1, coluIniEmpresa + 3].Value = dataCab[1, 3];
                ws.Cells[filaIniEmpresa + 1, coluIniEmpresa + 4].Value = dataCab[1, 4];
                ws.Cells[filaIniEmpresa + 1, coluIniEmpresa + 5].Value = dataCab[1, 5];

                ws.Cells[filaIniEmpresa + 2, coluIniEmpresa + 2].Value = dataCab[2, 2];
                ws.Cells[filaIniEmpresa + 2, coluIniEmpresa + 3].Value = dataCab[2, 3];
                ws.Cells[filaIniEmpresa + 2, coluIniEmpresa + 4].Value = dataCab[2, 4];
                ws.Cells[filaIniEmpresa + 2, coluIniEmpresa + 5].Value = dataCab[2, 5];

                ws.Cells[filaIniEmpresa + 3, coluIniEmpresa + 2].Value = dataCab[3, 2];
                ws.Cells[filaIniEmpresa + 3, coluIniEmpresa + 3].Value = dataCab[3, 3];
                ws.Cells[filaIniEmpresa + 3, coluIniEmpresa + 4].Value = dataCab[3, 4];
                ws.Cells[filaIniEmpresa + 3, coluIniEmpresa + 5].Value = dataCab[3, 5];

                #endregion

                ultimaColu = coluIniEmpresa + 5;

                #region cuerpo

                ultimaFila = filaIniData + registrosDetalle.Count() - 1;
                ultimaFilaTabla = ultimaFila;

                #region Formato Cuerpo
                //UtilExcel.CeldasExcelWrapText(ws, filaIniData, coluIniData, ultimaFila, coluIniData);
                UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaIniData, coluIniData + 1, ultimaFila, ultimaColu, "Centro");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniData, coluIniData + 0, ultimaFila, coluIniData + 1, "Izquierda");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniData, coluIniData + 2, ultimaFila, ultimaColu, "Derecha");
                UtilExcel.CeldasExcelEnNegrita(ws, filaIniData, coluIniData + 0, ultimaFila, coluIniData + 0);
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniData, coluIniData, ultimaFila, ultimaColu, ConstantesPR5ReportesServicio.TipoLetraCuerpo, ConstantesPR5ReportesServicio.TamLetraCuerpo3);

                #endregion

                int filaX = 0;
                foreach (var reg in registrosDetalle)
                {
                    int colX = 0;

                    ws.Cells[filaIniData + filaX, coluIniData + colX].Value = reg.Nombre;
                    colX++;
                    if (!string.IsNullOrEmpty(reg.Nombre2))
                        ws.Cells[filaIniData + filaX, coluIniData + colX].Value = reg.Nombre2;
                    colX++;

                    if (!string.IsNullOrEmpty(reg.Nombre))
                    {
                        UtilExcel.CeldasExcelEnNegrita(ws, filaIniData + filaX, coluIniData + 0, filaIniData + filaX, coluIniData + 0);
                    }

                    if (!string.IsNullOrEmpty(reg.Nombre) && string.IsNullOrEmpty(reg.Nombre2))
                    {
                        string colorTotalEmp = "#DDEBF7";
                        UtilExcel.CeldasExcelColorFondo(ws, filaIniData + filaX, coluIniData + 0, filaIniData + filaX, ultimaColu, colorTotalEmp);
                        UtilExcel.CeldasExcelColorFondoYBorderSoloUnLado(ws, filaIniData + filaX, coluIniData + 0, filaIniData + filaX, ultimaColu, "#0000D0", "Abajo");
                    }

                    foreach (decimal? numValor in reg.ListaData)
                    {
                        string strFormat = ConstantesPR5ReportesServicio.FormatoNumero2Digito;

                        if (numValor != null)
                        {
                            ws.Cells[filaIniData + filaX, coluIniData + colX].Value = numValor;
                        }
                        ws.Cells[filaIniData + filaX, coluIniData + colX].Style.Numberformat.Format = strFormat;

                        colX++;
                    }

                    filaX++;
                }
                #endregion
            }

            //Filas resumen
            if (registrosTotal.Any())
            {
                int filaRes1 = filaIniEmpresa + 4 + registrosDetalle.Count();
                int filaResFin = filaRes1 + 3;
                int filaX = filaRes1;
                int coluIniData = coluIniEmpresa;
                int ultimaColu = coluIniEmpresa + 5;

                ultimaFilaTabla = filaResFin;

                foreach (var reg in registrosTotal)
                {
                    int colX = 0;

                    ws.Cells[filaX, coluIniData + colX].Value = reg.Nombre;
                    colX += 2; ;

                    foreach (decimal? numValor in reg.ListaData)
                    {
                        string strFormat = ConstantesPR5ReportesServicio.FormatoNumero2Digito;

                        if (numValor != null && numValor != 0)
                        {
                            ws.Cells[filaX, coluIniData + colX].Value = numValor;
                        }
                        ws.Cells[filaX, coluIniData + colX].Style.Numberformat.Format = strFormat;

                        colX++;
                    }
                    filaX++;
                }

                #region Formato Cuerpo
                UtilExcel.CeldasExcelWrapText(ws, filaRes1, coluIniData, filaResFin, ultimaColu);
                UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaRes1, coluIniData, filaResFin, ultimaColu, "Centro");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaRes1, coluIniData, filaResFin, coluIniData + 1, "Izquierda");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaRes1, coluIniData + 2, filaResFin, ultimaColu, "Derecha");
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaRes1, coluIniData, filaResFin, ultimaColu, ConstantesPR5ReportesServicio.TipoLetraCuerpo, ConstantesPR5ReportesServicio.TamLetraCuerpo3);

                UtilExcel.CeldasExcelColorTexto(ws, filaRes1, coluIniData, filaResFin, ultimaColu, "#FFFFFF");
                UtilExcel.CeldasExcelColorFondo(ws, filaRes1, coluIniData, filaResFin, ultimaColu, "#0077A5");

                UtilExcel.BorderCeldas2(ws, filaRes1, coluIniData, filaResFin, ultimaColu);

                UtilExcel.CeldasExcelEnNegrita(ws, filaRes1, coluIniData, filaResFin, ultimaColu);

                #endregion
            }
        }

        #endregion

        #region 8.2 MÁXIMA DEMANDA MENSUAL (HP y Fuera HP)

        public static void GenerarCharExcelResumenMaximaDemandaMensualHPyFHP(ExcelWorksheet ws, FechasPR5 objFecha, MeMedicion96DTO objMDSein,
                                List<MeMedicion96DTO> listaMDXDiaHP96, List<MeMedicion96DTO> listaMDXDiaFHP96)
        {
            int coluIniData = 1;
            int filaIniData = 12;
            int ultimaColu = coluIniData + 10;
            int ultimaFila = filaIniData + objFecha.AnioAct.RangoAct_FechaFin.Day - 1;

            ws.Cells[5, 3].Value = objMDSein.Meditotal;
            ws.Cells[6, 3].Value = objMDSein.Medifecha.Value.ToString(ConstantesAppServicio.FormatoFecha);
            ws.Cells[7, 3].Value = objMDSein.FechaMD.ToString(ConstantesAppServicio.FormatoHora);

            #region Formato Cuerpo

            UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaIniData, coluIniData + 1, ultimaFila, ultimaColu, "Centro");
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniData, coluIniData, ultimaFila, coluIniData + 1, "Centro");
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniData, coluIniData + 6, ultimaFila, coluIniData + 6, "Centro");
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniData, coluIniData + 2, ultimaFila, coluIniData + 5, "Derecha");
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniData, coluIniData + 7, ultimaFila, coluIniData + 10, "Derecha");
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniData, coluIniData, ultimaFila, ultimaColu, ConstantesPR5ReportesServicio.TipoLetraCuerpo, 7);
            UtilExcel.BorderCeldasLineaDelgada(ws, filaIniData, coluIniData, ultimaFila, ultimaColu, "#000000", true, true);

            string strFormat = ConstantesPR5ReportesServicio.FormatoNumero3Digito;
            ws.Cells[filaIniData, coluIniData + 2, ultimaFila, coluIniData + 5].Style.Numberformat.Format = strFormat;
            ws.Cells[filaIniData, coluIniData + 7, ultimaFila, coluIniData + 10].Style.Numberformat.Format = strFormat;
            ws.Cells[5, 3].Style.Numberformat.Format = strFormat;

            #endregion

            int filaX = 0;
            for (var day = objFecha.AnioAct.RangoAct_FechaIni; day <= objFecha.AnioAct.RangoAct_FechaFin; day = day.AddDays(1))
            {
                var objM96HP = listaMDXDiaHP96.Find(x => x.Medifecha == day);
                var objM96FHP = listaMDXDiaFHP96.Find(x => x.Medifecha == day);

                ws.Cells[filaIniData + filaX, coluIniData + 0].Value = day.ToString(ConstantesAppServicio.FormatoFecha);

                if (objM96FHP != null)
                {
                    ws.Cells[filaIniData + filaX, coluIniData + 1].Value = objM96FHP.FechaMD.ToString(ConstantesAppServicio.FormatoHora);
                    ws.Cells[filaIniData + filaX, coluIniData + 2].Value = objM96FHP.PotenciaActiva;
                    ws.Cells[filaIniData + filaX, coluIniData + 3].Value = objM96FHP.Imp;
                    ws.Cells[filaIniData + filaX, coluIniData + 4].Value = objM96FHP.Exp;
                    ws.Cells[filaIniData + filaX, coluIniData + 5].Value = objM96FHP.Meditotal;

                    if (objM96FHP.TieneMD)
                    {
                        UtilExcel.CeldasExcelColorFondo(ws, filaIniData + filaX, coluIniData + 1, filaIniData + filaX, coluIniData + 5, "#C4E8FF");
                    }
                }

                if (objM96HP != null)
                {
                    ws.Cells[filaIniData + filaX, coluIniData + 6].Value = objM96HP.FechaMD.ToString(ConstantesAppServicio.FormatoHora);
                    ws.Cells[filaIniData + filaX, coluIniData + 7].Value = objM96HP.PotenciaActiva;
                    ws.Cells[filaIniData + filaX, coluIniData + 8].Value = objM96HP.Imp;
                    ws.Cells[filaIniData + filaX, coluIniData + 9].Value = objM96HP.Exp;
                    ws.Cells[filaIniData + filaX, coluIniData + 10].Value = objM96HP.Meditotal;

                    if (objM96HP.TieneMD)
                    {
                        UtilExcel.CeldasExcelColorFondo(ws, filaIniData + filaX, coluIniData + 6, filaIniData + filaX, coluIniData + 10, "#C4E8FF");
                    }
                }

                filaX++;
            }

            ws.Column(1).Width = 9;
            ws.Column(2).Width = 6; //hora
            ws.Column(3).Width = 8;
            ws.Column(4).Width = 12;
            ws.Column(5).Width = 12;
            ws.Column(6).Width = 9;
            ws.Column(7).Width = 6; //hora
            ws.Column(8).Width = 8;
            ws.Column(9).Width = 12;
            ws.Column(10).Width = 12;
            ws.Column(11).Width = 9;
        }

        #endregion

        #endregion

        #region Informe Anual

        public static string GetNombreArchivoInformeAnual(DateTime fechaInicio, int version)
        {
            string nombreMes = EPDate.f_NombreMes(fechaInicio.Month);
            var nombreArchivo = "";
            nombreArchivo = string.Format("Inf_Anual {0}_v{1}_SGI{2}", fechaInicio.Year, version, ConstantesAppServicio.ExtensionExcel); //Inf_Anual {YYYY}_v{NroVersion}_SGI
            return nombreArchivo;
        }

        #region Resúmen Relevante

        /// <summary>
        /// Genera los parrafos del resumen relevante mensual
        /// </summary>
        /// <param name="objFecha"></param>
        /// <param name="listaParticipacionRecursosEnergeticos"></param>
        /// <param name="listaMDTipoRecursoEnergeticoData"></param>
        /// <returns></returns>
        public static ReporteResumenRelevante GetTextoResumenRelevanteInformeAnual(FechasPR5 objFecha, List<ResultadoTotalGeneracion> listaParticipacionRREETexto,
                                                List<ResultadoTotalGeneracion> listaPotGenData)
        {
            NumberFormatInfo nfi = UtilAnexoAPR5.GenerarNumberFormatInfo1();
            NumberFormatInfo nfi2 = UtilAnexoAPR5.GenerarNumberFormatInfo2();
            NumberFormatInfo nfi3 = UtilAnexoAPR5.GenerarNumberFormatInfo3();

            var listaValoryParti = listaParticipacionRREETexto.Where(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct).ToList();
            var listaVar = listaParticipacionRREETexto.Where(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_Total_Var).ToList();

            //primer parrafo
            string energiaMesAct = UtilAnexoAPR5.ImprimirValorTotalHtml(listaPotGenData.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct).Meditotal, nfi2);

            decimal? valorDiferencialMesActual = listaPotGenData.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct_Resta).Meditotal;
            string textoDiferencialMesActual = UtilAnexoAPR5.ImprimirValorTotalHtml(Math.Abs(valorDiferencialMesActual.GetValueOrDefault(0)), nfi2);

            decimal? variacionMesAct = listaPotGenData.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct_Var).Meditotal;
            string textoVariacionMesAct = variacionMesAct > 0 ? "un incremento" : "una disminución";
            string varenergiaMesAct = UtilAnexoAPR5.ImprimirVariacionHtml(Math.Abs(variacionMesAct.GetValueOrDefault(0)), nfi2);

            var anioActual = objFecha.AnioAct.RangoAct_FechaIni.Year;
            var anioPasado = anioActual - 1;

            //segundo parrafo
            string energiaHidro = UtilAnexoAPR5.ImprimirValorTotalHtml(listaValoryParti.Find(x => x.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiAgua).TotalProducido, nfi2);

            decimal? variacionHidro = listaVar.Find(x => x.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiAgua).Meditotal;
            string textoVariacionHidro = variacionHidro > 0 ? "mayor" : "menor";
            string varEnergiaHidro = UtilAnexoAPR5.ImprimirVariacionHtml(Math.Abs(variacionHidro.GetValueOrDefault(0)), nfi2);

            //tercer parrafo
            string energiaCentralTermo = UtilAnexoAPR5.ImprimirValorTotalHtml(listaValoryParti.Find(x => x.Fenergcodi == ConstantesSiosein2.FenergcodiTermoelectrico).TotalProducido, nfi2);

            decimal? variacionCentralTermo = listaVar.Find(x => x.Fenergcodi == ConstantesSiosein2.FenergcodiTermoelectrico).Meditotal;
            string textoVariacionCentralTermo = variacionCentralTermo > 0 ? "mayor" : "menor";
            string varEnergiaCentralTermo = UtilAnexoAPR5.ImprimirVariacionHtml(Math.Abs(variacionCentralTermo.GetValueOrDefault(0)), nfi2);

            string parEnergiaCamisea = UtilAnexoAPR5.ImprimirVariacionHtml(listaValoryParti.Find(x => x.Fenergcodi == ConstantesSiosein2.FenergcodiGasCamisea).Meditotal, nfi2);
            string parEnergiaAgMalacas = UtilAnexoAPR5.ImprimirVariacionHtml(listaValoryParti.Find(x => x.Fenergcodi == ConstantesSiosein2.FenergcodiGasNoCamisea).Meditotal, nfi2);
            string parEnergiaDiesel = UtilAnexoAPR5.ImprimirVariacionHtml(listaValoryParti.Find(x => x.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiDiesel).Meditotal, nfi2);
            string parEnergiaResidual = UtilAnexoAPR5.ImprimirVariacionHtml(listaValoryParti.Find(x => x.Fenergcodi == ConstantesSiosein2.FenergcodiRelevanteResidual).Meditotal, nfi2);
            string parEnergiaCarbon = UtilAnexoAPR5.ImprimirVariacionHtml(listaValoryParti.Find(x => x.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiCarbon).Meditotal, nfi2);
            string parEnergiaBiogas = UtilAnexoAPR5.ImprimirVariacionHtml(listaValoryParti.Find(x => x.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiBiogas).Meditotal, nfi2);
            string parEnergiaBagazo = UtilAnexoAPR5.ImprimirVariacionHtml(listaValoryParti.Find(x => x.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiBagazo).Meditotal, nfi2);

            //cuarto parrafo
            string energiaEolico = UtilAnexoAPR5.ImprimirValorTotalHtml(listaValoryParti.Find(x => x.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiEolica).TotalProducido, nfi2);
            string energiaSolar = UtilAnexoAPR5.ImprimirValorTotalHtml(listaValoryParti.Find(x => x.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiSolar).TotalProducido, nfi2);

            string parEnergiaEolico = UtilAnexoAPR5.ImprimirVariacionHtml(listaValoryParti.Find(x => x.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiEolica).Meditotal, nfi2);
            string parEnergiaSolar = UtilAnexoAPR5.ImprimirVariacionHtml(listaValoryParti.Find(x => x.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiSolar).Meditotal, nfi2);

            #region Cuerpo Resumen
            ReporteResumenRelevante obj = new ReporteResumenRelevante();

            obj.TituloReporte = "INFORME DE LA OPERACIÓN ANUAL";

            obj.ListaFechaTituloReporte = new List<string>();
            obj.ListaFechaTituloReporte.Add(string.Format("{0}", objFecha.AnioAct.RangoAct_FechaIni.Year));//2022

            obj.Subtitulo = "1. RESUMEN";
            obj.TituloParrafo1 = string.Format("1.1 PRODUCCIÓN DE ENERGÍA EN EL {0}", objFecha.AnioAct.RangoAct_FechaIni.Year);

            obj.ListaItemParrafo1 = new List<string>();
            obj.ListaItemParrafo1.Add(string.Format(@"El total de la producción de energía eléctrica de la empresas generadoras integrantes del COES en el {0} fue de {1} GWh, lo que representa {2} de {3} GWh ({4}) en comparación con el año {5}."
                , anioActual //
                , energiaMesAct //
                , textoVariacionMesAct //
                , textoDiferencialMesActual
                , varenergiaMesAct //
                , anioPasado //
                ));

            obj.ListaItemParrafo1.Add(string.Format(@"La producción de electricidad con centrales hidroeléctricas durante el {0} fue de {1} GWh ({2} {3} al registrado durante el año {4})."
                , anioActual //
                , energiaHidro //
                , varEnergiaHidro //
                , textoVariacionHidro //
                , anioPasado //
                ));

            obj.ListaItemParrafo1.Add(string.Format(@"La producción de electricidad con centrales termoeléctricas durante el {0} fue de {1} GWh ({2} {3} al registrado durante el año {4})."
                            + "La participación del  gas natural de Camisea fue de {5}, mientras que las del gas que proviene de los yacimientos de Aguaytía y Malacas fue del {6}, la producción con diesel, residual, carbón, biogás y bagazo tuvieron una intervención del {7}, {8}, {9}, {10}, {11} respectivamente."
                , anioActual //
                , energiaCentralTermo //
                , textoVariacionCentralTermo //
                , varEnergiaCentralTermo //
                , anioPasado
                , parEnergiaCamisea
                , parEnergiaAgMalacas
                , parEnergiaDiesel
                , parEnergiaResidual
                , parEnergiaCarbon
                , parEnergiaBiogas
                , parEnergiaBagazo
                ));

            obj.ListaItemParrafo1.Add(string.Format(@"La producción de energía eléctrica con centrales eólicas fue de {0} GWh y con centrales solares fue de {1} GWh, los cuales tuvieron una participación de {2} y {3} respectivamente."
                , energiaEolico //  
                , energiaSolar //
                , parEnergiaEolico
                , parEnergiaSolar
                ));

            #endregion

            return obj;
        }

        #endregion

        #region 6.1 EVOLUCIÓN MENSUAL DE LOS COSTOS MARGINALES PROMEDIO PONDERA DEL SEIN BARRA STA ROSA

        /// <summary>
        /// Devuelve la data para generar el listado del reporte Costos Marginales promedios  Barra Sta Rosa 
        /// </summary>
        /// <param name="lstCMPromStaRosa"></param>
        /// <returns></returns>
        public static TablaReporte ObtenerDataTablaCostosMarginalesStaRosaAnual(List<CostosMarginalesStaRosa> lstCMPromStaRosa)
        {
            TablaReporte tabla = new TablaReporte();
            CabeceraReporte cabRepo = new CabeceraReporte();
            var anios = lstCMPromStaRosa.Select(x => x.Anio).OrderByDescending(x => x).ToList();

            string[,] matrizCabecera = new string[1, 4];

            matrizCabecera[0, 0] = "Mes";
            matrizCabecera[0, 1] = anios.First().ToString();
            matrizCabecera[0, 2] = anios.Last().ToString();
            matrizCabecera[0, 3] = "Var. (%)";


            cabRepo.CabeceraData = matrizCabecera;

            tabla.Cabecera = cabRepo;

            #region cuerpo
            List<RegistroReporte> registros = new List<RegistroReporte>();

            for (int mes = 1; mes <= 12; mes++)
            {
                var data = lstCMPromStaRosa.Where(x => x.numMes == mes).OrderByDescending(x => x.Anio).ToList();
                var dataAnioAct = data.First();
                var dataAnioAnt = data.Last();

                RegistroReporte registro = new RegistroReporte();
                List<decimal?> datos = new List<decimal?>();

                registro.Nombre = dataAnioAct.nombMes;

                datos.Add(dataAnioAct.CostoPromedio);
                datos.Add(dataAnioAnt.CostoPromedio);
                datos.Add(dataAnioAnt.CostoPromedio != null || dataAnioAnt.CostoPromedio != 0 ? (dataAnioAct.CostoPromedio / dataAnioAnt.CostoPromedio - 1) * 100 : null);
                registro.ListaData = datos;

                registros.Add(registro);
            }

            tabla.ListaRegistros = registros;

            #endregion

            return tabla;
        }

        #endregion

        #region 8.1 Intercambios Internacionales de energía y potencia

        public static TablaReporte ObtenerDataTablaInterconexionInternacionalAnual(FechasPR5 objFecha,
                List<ResultadoTotalGeneracion> listaEnergia, List<ResultadoTotalGeneracion> listaMWmax, List<ResultadoTotalGeneracion> listaEnergiaTotal,
                List<ResultadoTotalGeneracion> listaMWmaxTotal)
        {
            TablaReporte tabla = new TablaReporte();
            CabeceraReporte cabRepo = new CabeceraReporte();

            string[,] matrizCabecera = new string[3, 9];

            matrizCabecera[0, 0] = "Mes";
            matrizCabecera[0, 1] = objFecha.Anio1Ant.NumAnio.ToString();
            matrizCabecera[0, 5] = objFecha.AnioAct.NumAnio.ToString();

            matrizCabecera[1, 1] = "EXPORTACIÓN";
            matrizCabecera[1, 3] = "IMPORTACIÓN";
            matrizCabecera[1, 5] = "EXPORTACIÓN";
            matrizCabecera[1, 7] = "IMPORTACIÓN";

            matrizCabecera[2, 1] = "ENERGÍA  (GWh)";
            matrizCabecera[2, 2] = "MÁXIMA POTENCIA (MW)";
            matrizCabecera[2, 3] = "ENERGÍA  (GWh)";
            matrizCabecera[2, 4] = "MÁXIMA POTENCIA (MW)";
            matrizCabecera[2, 5] = "ENERGÍA  (GWh)";
            matrizCabecera[2, 6] = "MÁXIMA POTENCIA (MW)";
            matrizCabecera[2, 7] = "ENERGÍA  (GWh)";
            matrizCabecera[2, 8] = "MÁXIMA POTENCIA (MW)";

            cabRepo.CabeceraData = matrizCabecera;

            tabla.Cabecera = cabRepo;

            #region cuerpo
            List<RegistroReporte> registros = new List<RegistroReporte>();

            for (int mes = 1; mes <= 12; mes++)
            {
                DateTime fechaMes1Ant = objFecha.Anio1Ant.Fecha_01Enero.AddMonths(mes - 1);
                DateTime fechaMesAct = objFecha.AnioAct.Fecha_01Enero.AddMonths(mes - 1);

                RegistroReporte registro = new RegistroReporte();
                List<decimal?> datos = ListarRegistroXFilaIntercambioInt(fechaMes1Ant, fechaMesAct, listaEnergia, listaMWmax);

                registro.Nombre = EPDate.f_NombreMes(mes);

                registro.ListaData = datos;

                registros.Add(registro);
            }

            tabla.ListaRegistros = registros;

            RegistroReporte registro0 = new RegistroReporte();
            List<decimal?> datos0 = ListarRegistroXFilaIntercambioInt(objFecha.Anio1Ant.Fecha_01Enero, objFecha.AnioAct.Fecha_01Enero, listaEnergiaTotal, listaMWmaxTotal);

            registro0.Nombre = "TOTAL";
            registro0.ListaData = datos0;
            registro0.EsFilaResumen = true;

            registros.Add(registro0);
            #endregion

            return tabla;
        }

        private static List<decimal?> ListarRegistroXFilaIntercambioInt(DateTime fecha1Ant, DateTime fechaAct, List<ResultadoTotalGeneracion> listaEnergia, List<ResultadoTotalGeneracion> listaMWmax)
        {
            decimal? energExp1 = listaEnergia.Find(x => x.Medifecha == fecha1Ant && x.TipoSemanaRelProd == ConstantesPR5ReportesServicio.TipoSemanaRelExp).Meditotal;
            decimal? energExp0 = listaEnergia.Find(x => x.Medifecha == fechaAct && x.TipoSemanaRelProd == ConstantesPR5ReportesServicio.TipoSemanaRelExp).Meditotal;
            decimal? energImp1 = listaEnergia.Find(x => x.Medifecha == fecha1Ant && x.TipoSemanaRelProd == ConstantesPR5ReportesServicio.TipoSemanaRelImp).Meditotal;
            decimal? energImp0 = listaEnergia.Find(x => x.Medifecha == fechaAct && x.TipoSemanaRelProd == ConstantesPR5ReportesServicio.TipoSemanaRelImp).Meditotal;

            decimal? mwExp1 = listaMWmax.Find(x => x.Medifecha == fecha1Ant && x.TipoSemanaRelProd == ConstantesPR5ReportesServicio.TipoSemanaRelExp).Meditotal;
            decimal? mwExp0 = listaMWmax.Find(x => x.Medifecha == fechaAct && x.TipoSemanaRelProd == ConstantesPR5ReportesServicio.TipoSemanaRelExp).Meditotal;
            decimal? mwImp1 = listaMWmax.Find(x => x.Medifecha == fecha1Ant && x.TipoSemanaRelProd == ConstantesPR5ReportesServicio.TipoSemanaRelImp).Meditotal;
            decimal? mwImp0 = listaMWmax.Find(x => x.Medifecha == fechaAct && x.TipoSemanaRelProd == ConstantesPR5ReportesServicio.TipoSemanaRelImp).Meditotal;

            List<decimal?> datos = new List<decimal?>();
            datos.Add(energExp1);
            datos.Add(mwExp1);
            datos.Add(energImp1);
            datos.Add(mwImp1);

            datos.Add(energExp0);
            datos.Add(mwExp0);
            datos.Add(energImp0);
            datos.Add(mwImp0);

            return datos;
        }

        public static TablaReporte ObtenerDataTablaVariacionInterconexionInternacionalAnual(FechasPR5 objFecha,
                List<ResultadoTotalGeneracion> listaEnergiaVar, List<ResultadoTotalGeneracion> listaMWmaxVar)
        {
            TablaReporte tabla = new TablaReporte();
            CabeceraReporte cabRepo = new CabeceraReporte();

            string[,] matrizCabecera = new string[2, 5];

            matrizCabecera[0, 0] = string.Format("Variación {0}/{1} (%)", objFecha.AnioAct.NumAnio, objFecha.Anio1Ant.NumAnio);
            matrizCabecera[0, 1] = "EXPORTACIÓN";
            matrizCabecera[0, 3] = "IMPORTACIÓN";

            matrizCabecera[1, 1] = "ENERGÍA";
            matrizCabecera[1, 2] = "MÁXIMA POTENCIA";
            matrizCabecera[1, 3] = "ENERGÍA";
            matrizCabecera[1, 4] = "MÁXIMA POTENCIA";

            cabRepo.CabeceraData = matrizCabecera;

            tabla.Cabecera = cabRepo;

            #region cuerpo
            List<RegistroReporte> registros = new List<RegistroReporte>();

            RegistroReporte registro0 = new RegistroReporte();
            List<decimal?> datos0 = new List<decimal?>();

            decimal? energExp = listaEnergiaVar.Find(x => x.TipoSemanaRelProd == ConstantesPR5ReportesServicio.TipoSemanaRelExp).Meditotal;
            decimal? mwExp = listaMWmaxVar.Find(x => x.TipoSemanaRelProd == ConstantesPR5ReportesServicio.TipoSemanaRelExp).Meditotal;
            decimal? energImp = listaEnergiaVar.Find(x => x.TipoSemanaRelProd == ConstantesPR5ReportesServicio.TipoSemanaRelImp).Meditotal;
            decimal? mwImp = listaMWmaxVar.Find(x => x.TipoSemanaRelProd == ConstantesPR5ReportesServicio.TipoSemanaRelImp).Meditotal;

            datos0.Add(energExp);
            datos0.Add(mwExp);
            datos0.Add(energImp);
            datos0.Add(mwImp);

            registro0.Nombre = "TOTAL";
            registro0.ListaData = datos0;
            registro0.EsFilaResumen = true;

            registros.Add(registro0);
            tabla.ListaRegistros = registros;

            #endregion

            return tabla;
        }

        public static string ListarTablaInterconexionInternacionalAnualHTML(FechasPR5 objFecha, TablaReporte tablaData)
        {
            var dataCab = tablaData.Cabecera.CabeceraData;
            var registros = tablaData.ListaRegistros;

            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = UtilAnexoAPR5.GenerarNumberFormatInfo2();

            strHtml.Append("<div id='listado_reporte' style='margin:0px auto; width: 1120px;'>");
            strHtml.Append("<table class='pretty tabla-icono' id=''>");

            #region cabecera

            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.AppendFormat("<th style='width: 160px;' rowspan='3'>{0}</th>", dataCab[0, 0]);
            strHtml.AppendFormat("<th style='width: 200px;' colspan='4'>{0}</th>", dataCab[0, 1]);
            strHtml.AppendFormat("<th style='width: 200px;' colspan='4'>{0}</th>", dataCab[0, 5]);
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.AppendFormat("<th style='width: 160px;' colspan='2'>{0}</th>", dataCab[1, 1]);
            strHtml.AppendFormat("<th style='width: 160px;' colspan='2'>{0}</th>", dataCab[1, 3]);
            strHtml.AppendFormat("<th style='width: 160px;' colspan='2'>{0}</th>", dataCab[1, 5]);
            strHtml.AppendFormat("<th style='width: 160px;' colspan='2'>{0}</th>", dataCab[1, 7]);
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.AppendFormat("<th style='width: 80px;'>{0}</th>", dataCab[2, 1]);
            strHtml.AppendFormat("<th style='width: 80px;'>{0}</th>", dataCab[2, 2]);
            strHtml.AppendFormat("<th style='width: 80px;'>{0}</th>", dataCab[2, 3]);
            strHtml.AppendFormat("<th style='width: 80px;'>{0}</th>", dataCab[2, 4]);
            strHtml.AppendFormat("<th style='width: 80px;'>{0}</th>", dataCab[2, 5]);
            strHtml.AppendFormat("<th style='width: 80px;'>{0}</th>", dataCab[2, 6]);
            strHtml.AppendFormat("<th style='width: 80px;'>{0}</th>", dataCab[2, 7]);
            strHtml.AppendFormat("<th style='width: 80px;'>{0}</th>", dataCab[2, 8]);
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            #endregion

            #region cuerpo
            strHtml.Append("<tbody>");
            foreach (var reg in registros)
            {
                strHtml.Append("<tr>");
                var styleCelda = reg.EsFilaResumen ? "background-color: #95B3D7;" : "";
                strHtml.AppendFormat("<td style='{1}'>{0}</td>", reg.Nombre, styleCelda);

                foreach (decimal? col in reg.ListaData)
                {
                    strHtml.AppendFormat("<td class='alignValorCenter' style='{1}'>{0}</td>", UtilAnexoAPR5.ImprimirValorTotalOcultar0Html(col, nfi), styleCelda);
                }

                strHtml.Append("</tr>");
            }
            strHtml.Append("</tbody>");

            #endregion

            strHtml.Append("</table>");

            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        public static string ListarTablaVariacionInterconexionInternacionalAnualHTML(FechasPR5 objFecha, TablaReporte tablaData)
        {
            var dataCab = tablaData.Cabecera.CabeceraData;
            var registros = tablaData.ListaRegistros;

            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = UtilAnexoAPR5.GenerarNumberFormatInfo1();

            strHtml.Append("<div id='listado_reporte' style='margin:0px auto; width: 1120px;'>");
            strHtml.Append("<table class='pretty tabla-icono' id=''>");
            #region cabecera

            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.AppendFormat("<th style='width: 160px;' rowspan='3'>{0}</th>", dataCab[0, 0]);
            strHtml.AppendFormat("<th style='width: 200px;' colspan='2'>{0}</th>", dataCab[0, 1]);
            strHtml.AppendFormat("<th style='width: 200px;' colspan='2'>{0}</th>", dataCab[0, 3]);
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.AppendFormat("<th style='width: 80px;'>{0}</th>", dataCab[1, 1]);
            strHtml.AppendFormat("<th style='width: 80px;'>{0}</th>", dataCab[1, 2]);
            strHtml.AppendFormat("<th style='width: 80px;'>{0}</th>", dataCab[1, 3]);
            strHtml.AppendFormat("<th style='width: 80px;'>{0}</th>", dataCab[1, 4]);
            strHtml.Append("</tr>");

            foreach (var reg in registros)
            {
                strHtml.Append("<tr>");

                foreach (decimal? col in reg.ListaData)
                {
                    strHtml.AppendFormat("<th class='alignValorCenter'>{0}</th>", UtilAnexoAPR5.ImprimirVariacionHtml(col, nfi));
                }

                strHtml.Append("</tr>");
            }
            strHtml.Append("</thead>");

            #endregion

            strHtml.Append("</table>");

            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        public static GraficoWeb GetGraficoInterconexionInternacionalAnual(FechasPR5 objFecha, TablaReporte tablaData)
        {
            var dataCab = tablaData.Cabecera.CabeceraData;
            var registros = tablaData.ListaRegistros.Where(x => !x.EsFilaResumen).ToList();
            int anio0 = objFecha.AnioAct.NumAnio;
            int anio1 = objFecha.Anio1Ant.NumAnio;

            GraficoWeb grafico = new GraficoWeb();
            grafico.SeriesData = new decimal?[8][];
            grafico.XAxisTitle = "Mes";
            // titulo el reporte               
            grafico.TitleText = @"EVOLUCIÓN MENSUAL DE LOS INTERCAMBIOS INTERNACIONALES  (PERÚ - ECUADOR)";
            grafico.YaxixTitle = "Energía (GWh)";

            grafico.XAxisCategories = new List<string>();
            grafico.SeriesName = new List<string>();
            grafico.SeriesType = new List<string>();
            grafico.SeriesYAxis = new List<int>();

            // Obtener Lista de intervalos categoria del grafico   
            grafico.XAxisCategories.AddRange(registros.Select(x => x.Nombre).ToList());

            // Obtener lista de valores para las series del grafico
            grafico.Series = new List<RegistroSerie>();
            for (var i = 0; i < 8; i++)
            {
                grafico.Series.Add(new RegistroSerie());
                switch (i)
                {
                    case 0:
                        grafico.Series[i].Name = "ENERGÍA EXPORTADA (GWh) " + anio1;
                        grafico.Series[i].Type = "column";
                        grafico.Series[i].Color = "#26829A";
                        grafico.Series[i].YAxis = 0;
                        grafico.Series[i].YAxisTitle = "GWh";
                        break;
                    case 1:
                        grafico.Series[i].Name = "MÁXIMA POTENCIA EXPORTADA (MW) " + anio1;
                        grafico.Series[i].Type = "spline";
                        grafico.Series[i].Color = "#6E548D";
                        grafico.Series[i].YAxis = 1;
                        grafico.Series[i].YAxisTitle = "MW";
                        break;
                    case 2:
                        grafico.Series[i].Name = "ENERGÍA IMPORTADA (GWh) " + anio1;
                        grafico.Series[i].Type = "column";
                        grafico.Series[i].Color = "#E46C0A";
                        grafico.Series[i].YAxis = 0;
                        grafico.Series[i].YAxisTitle = "GWh";
                        break;
                    case 3:
                        grafico.Series[i].Name = "MÁXIMA POTENCIA IMPORTADA (MW) " + anio1;
                        grafico.Series[i].Type = "spline";
                        grafico.Series[i].Color = "#8EA5CB";
                        grafico.Series[i].YAxis = 1;
                        grafico.Series[i].YAxisTitle = "MW";
                        break;

                    case 4:
                        grafico.Series[i].Name = "ENERGÍA EXPORTADA (GWh) " + anio0;
                        grafico.Series[i].Type = "column";
                        grafico.Series[i].Color = "#27548A";
                        grafico.Series[i].YAxis = 0;
                        grafico.Series[i].YAxisTitle = "GWh";
                        break;
                    case 5:
                        grafico.Series[i].Name = "MÁXIMA POTENCIA EXPORTADA (MW) " + anio0;
                        grafico.Series[i].Type = "spline";
                        grafico.Series[i].Color = "#FF0000";
                        grafico.Series[i].YAxis = 1;
                        grafico.Series[i].YAxisTitle = "MW";
                        break;
                    case 6:
                        grafico.Series[i].Name = "ENERGÍA IMPORTADA (GWh) " + anio0;
                        grafico.Series[i].Type = "column";
                        grafico.Series[i].Color = "#769535";
                        grafico.Series[i].YAxis = 0;
                        grafico.Series[i].YAxisTitle = "GWh";
                        break;
                    case 7:
                        grafico.Series[i].Name = "MÁXIMA POTENCIA IMPORTADA (MW) " + anio0;
                        grafico.Series[i].Type = "spline";
                        grafico.Series[i].Color = "#DEA900";
                        grafico.Series[i].YAxis = 1;
                        grafico.Series[i].YAxisTitle = "MW";
                        break;
                }
                grafico.SeriesData[i] = new decimal?[12];
            }

            //llenar data
            for (var i = 0; i < 8; i++)
            {
                for (var j = 1; j <= 12; j++)
                {
                    decimal? valor = registros[j - 1].ListaData[i];
                    if (valor != 0)
                        grafico.SeriesData[i][j - 1] = valor;
                }
            }

            return grafico;
        }

        public static void GenerarCharExcelIntercambioInternacionalAnual(ExcelWorksheet ws, FechasPR5 objFecha, TablaReporte dataTablaDetalle, TablaReporte dataTablaVar)
        {
            var dataCabTabla = dataTablaDetalle.Cabecera.CabeceraData;
            var registrosDetalleTabla = dataTablaDetalle.ListaRegistros;

            int coluIniData = 2;
            int filaIniData = 6;

            //cabecera tabla
            ws.Cells[3, 3].Value = dataCabTabla[0, 1];
            ws.Cells[3, 7].Value = dataCabTabla[0, 5];

            //tabla
            int filaX = 0;
            foreach (var reg in registrosDetalleTabla)
            {
                ws.Cells[filaIniData + filaX, coluIniData + 0].Value = reg.Nombre;

                int colX = 1;
                foreach (decimal? valor in reg.ListaData)
                {
                    if (valor != 0)
                        ws.Cells[filaIniData + filaX, coluIniData + colX].Value = valor;
                    colX++;
                }

                filaX++;
            }

            //variacion
            var dataCabVar = dataTablaVar.Cabecera.CabeceraData;
            var registrosDetalleVar = dataTablaVar.ListaRegistros;

            ws.Cells[20, 2].Value = dataCabVar[0, 0];

            int colX2 = 1;
            foreach (decimal? valor in registrosDetalleVar[0].ListaData)
            {
                if (valor != null)
                    ws.Cells[22, coluIniData + colX2].Value = valor.GetValueOrDefault(0) / 100.0m;
                ws.Cells[22, coluIniData + colX2].Style.Numberformat.Format = ConstantesPR5ReportesServicio.FormatoNumero1DigitoPorcentaje;
                colX2 += 2;
            }
        }

        #endregion

        #region 9.1 Producción de Electricidad Anual por Empresa y tipo de Generación en el Sein

        public static TablaReporte ObtenerDataTablaResumenProduccionAnual(FechasPR5 objFecha, List<SiEmpresaDTO> listaEmpresa, List<EqEquipoDTO> listaCentral,
                List<ResultadoTotalGeneracion> listaTgen, List<ResultadoTotalGeneracion> listaEnergEjec,
                List<ResultadoTotalGeneracion> listaTotalTgen, List<ResultadoTotalGeneracion> listaTotalEnergEjec, List<ResultadoTotalGeneracion> listaTIEC3)
        {
            TablaReporte tabla = new TablaReporte();
            tabla.ReptiCodiTabla = objFecha.TipoReporte;
            CabeceraReporte cabRepo = new CabeceraReporte();

            string[,] matrizCabecera = new string[4, 7];

            matrizCabecera[0, 0] = "EMPRESA";
            matrizCabecera[0, 1] = "CENTRAL";
            matrizCabecera[0, 2] = string.Format("ENERGÍA PRODUCIDA EN EL {0}", objFecha.AnioAct.NumAnio);

            matrizCabecera[1, 2] = "GENERACIÓN";
            matrizCabecera[1, 5] = "TOTAL";

            matrizCabecera[2, 2] = "HIDROELÉCTRICA";
            matrizCabecera[2, 3] = "TERMOELÉCTRICA";
            matrizCabecera[2, 4] = "RER(***)";
            matrizCabecera[2, 5] = objFecha.AnioAct.RangoAct_Num.ToUpper();

            matrizCabecera[3, 2] = "MWh";
            matrizCabecera[3, 3] = "MWh";
            matrizCabecera[3, 4] = "MWh";
            matrizCabecera[3, 5] = "MWh";

            cabRepo.CabeceraData = matrizCabecera;
            tabla.Cabecera = cabRepo;

            #region cuerpo
            List<RegistroReporte> registros = new List<RegistroReporte>();

            var listaFila = new List<EqEquipoDTO>();
            foreach (var regEmp in listaEmpresa)
            {
                var listaEqXEmp = listaCentral.Where(x => x.Emprcodi == regEmp.Emprcodi).ToList();
                int i = 0;
                foreach (var regGr in listaEqXEmp)
                {
                    string empFila = i == 0 ? regEmp.Emprnomb : "";
                    listaFila.Add(new EqEquipoDTO() { Emprcodi = 0, Equipadre = regGr.Equipadre, Emprnomb = empFila, Central = regGr.Central });
                    i++;
                }
                listaFila.Add(new EqEquipoDTO() { Emprcodi = regEmp.Emprcodi, Equipadre = 0, Emprnomb = string.Format("Total {0}", regEmp.Emprnomb), Central = "" });
            }

            //Por tipo de Generación
            foreach (var regFila in listaFila)
            {
                RegistroReporte registro = new RegistroReporte();
                List<decimal?> datos = new List<decimal?>();

                var listaXTgen = listaTgen.Where(x => x.Emprcodi == regFila.Emprcodi && x.Equipadre == regFila.Equipadre).ToList();

                ResultadoTotalGeneracion regTgenHidro = listaXTgen.Find(x => x.Tgenercodi == ConstantesPR5ReportesServicio.TgenercodiHidro);
                ResultadoTotalGeneracion regTgenTermo = listaXTgen.Find(x => x.Tgenercodi == ConstantesPR5ReportesServicio.TgenercodiTermo);
                ResultadoTotalGeneracion regTgenRER = listaXTgen.Find(x => x.Tgenercodi == ConstantesPR5ReportesServicio.TgenercodiRER);

                ResultadoTotalGeneracion regEnergAcumAnio0 = listaEnergEjec.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_Acum && x.Emprcodi == regFila.Emprcodi && x.Equipadre == regFila.Equipadre);

                datos.Add(regTgenHidro.Meditotal);
                datos.Add(regTgenTermo.Meditotal);
                datos.Add(regTgenRER.Meditotal);

                datos.Add(regEnergAcumAnio0.Meditotal);

                registro.Nombre = regFila.Emprnomb;
                registro.Nombre2 = regFila.Central;
                registro.ListaData = datos;

                registros.Add(registro);
            }

            //Tipo TOTAL sin interconexion
            foreach (var regTotal in ListarFilaCuadro8_2Gen())
            {
                RegistroReporte registro = new RegistroReporte();
                List<decimal?> datos = new List<decimal?>();

                var listaXTgen = listaTotalTgen.Where(x => x.TipoSemanaRelProd == regTotal.Entero1.Value).ToList();

                ResultadoTotalGeneracion regTgenHidro = listaXTgen.Find(x => x.Tgenercodi == ConstantesPR5ReportesServicio.TgenercodiHidro);
                ResultadoTotalGeneracion regTgenTermo = listaXTgen.Find(x => x.Tgenercodi == ConstantesPR5ReportesServicio.TgenercodiTermo);
                ResultadoTotalGeneracion regTgenRER = listaXTgen.Find(x => x.Tgenercodi == ConstantesPR5ReportesServicio.TgenercodiRER);

                ResultadoTotalGeneracion regEnergAcumAnio0 = listaTotalEnergEjec.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_Acum && x.TipoSemanaRelProd == regTotal.Entero1.Value);

                datos.Add(regTgenHidro.Meditotal);
                datos.Add(regTgenTermo.Meditotal);
                datos.Add(regTgenRER.Meditotal);

                datos.Add(regEnergAcumAnio0.Meditotal);

                registro.EsFilaResumen = true;
                registro.Nombre = regTotal.String1;
                registro.ListaData = datos;

                registros.Add(registro);
            }

            //Agregar 2 filas de Interconexion
            foreach (var regTotal in ListarFilaCuadro8_2TIE())
            {
                RegistroReporte registro = new RegistroReporte();
                List<decimal?> datos = new List<decimal?>();

                ResultadoTotalGeneracion regEnergAcumAnio0 = listaTIEC3.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_Acum && x.TipoSemanaRelProd == regTotal.Entero1.Value);

                datos.Add(null);
                datos.Add(null);
                datos.Add(null);

                datos.Add(regEnergAcumAnio0.Meditotal);

                registro.EsFilaResumen = true;
                registro.Nombre = regTotal.String1;
                registro.ListaData = datos;

                registros.Add(registro);
            }

            #endregion

            tabla.ListaRegistros = registros;

            return tabla;
        }

        public static string ListarResumenProduccionAnualHtml(TablaReporte tablaData)
        {
            var dataCab = tablaData.Cabecera.CabeceraData;
            var registros = tablaData.ListaRegistros;

            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi2 = UtilAnexoAPR5.GenerarNumberFormatInfo2();
            var tamTabla = 1170;

            strHtml.Append("<div id='listado_reporte' style='height: auto; width: " + tamTabla + "px;'>");

            strHtml.Append("<table id='reporte' class='pretty tabla-icono' style='width: " + (tamTabla - 30) + "px;'>");

            #region cabecera

            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.AppendFormat("<th rowspan='4' style='width: 285px;'>{0}</th>", dataCab[0, 0]);
            strHtml.AppendFormat("<th rowspan='4' style='width: 285px;'>{0}</th>", dataCab[0, 1]);
            strHtml.AppendFormat("<th colspan='4'>{0}</th>", dataCab[0, 2]);
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.AppendFormat("<th colspan='3'>{0}</th>", dataCab[1, 2]);
            strHtml.AppendFormat("<th style='width:100px;'>{0}</th>", dataCab[1, 5]);
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.AppendFormat("<th style=''>{0}</th>", dataCab[2, 2]);
            strHtml.AppendFormat("<th style=''>{0}</th>", dataCab[2, 3]);
            strHtml.AppendFormat("<th style=''>{0}</th>", dataCab[2, 4]);
            strHtml.AppendFormat("<th style=''>{0}</th>", dataCab[2, 5]);
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.AppendFormat("<th style=''>{0}</th>", dataCab[3, 2]);
            strHtml.AppendFormat("<th style=''>{0}</th>", dataCab[3, 3]);
            strHtml.AppendFormat("<th style=''>{0}</th>", dataCab[3, 4]);
            strHtml.AppendFormat("<th style=''>{0}</th>", dataCab[3, 5]);
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            #endregion

            #region cuerpo
            strHtml.Append("<tbody>");
            int f = 0;
            foreach (var reg in registros)
            {
                if (!reg.EsFilaResumen)
                {
                    string styleTotalEmp = !string.IsNullOrEmpty(reg.Nombre) && string.IsNullOrEmpty(reg.Nombre2) ? "background-color: #DDEBF7;" : "";

                    strHtml.Append("<tr>");
                    strHtml.AppendFormat("<td style='padding-left: 5px;text-align: left;font-weight:bold;{1}'>{0}</td>", reg.Nombre, styleTotalEmp);
                    strHtml.AppendFormat("<td style='padding-left: 5px;text-align: left;{1}'>{0}</td>", reg.Nombre2, styleTotalEmp);

                    int c = 0;
                    foreach (decimal? col in reg.ListaData)
                    {
                        strHtml.AppendFormat("<td class='alignValorRight' style='{1}'>{0}</td>", UtilAnexoAPR5.ImprimirValorTotalOcultar0Html(col.HasValue ? col : null, nfi2), styleTotalEmp);

                        c++;
                    }
                }
                strHtml.Append("</tr>");
                f++;
            }
            strHtml.Append("</tbody>");

            strHtml.Append("<thead>");
            foreach (var reg in registros)
            {
                if (reg.EsFilaResumen)
                {
                    strHtml.Append("<tr>");
                    strHtml.AppendFormat("<th colspan='2' style='padding-left: 5px;text-align: left;font-weight:bold;'>{0}</t>", reg.Nombre);

                    int c = 0;
                    foreach (decimal? col in reg.ListaData)
                    {
                        strHtml.AppendFormat("<th class='alignValorRight' >{0}</th>", UtilAnexoAPR5.ImprimirValorTotalOcultar0Html(col.HasValue ? col : null, nfi2));

                        c++;
                    }

                    strHtml.Append("</tr>");
                }
            }
            strHtml.Append("</thead>");
            strHtml.Append("</table>");

            #endregion

            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Resumen Produccion
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="objFecha"></param>
        /// <param name="tablaData"></param>
        /// <param name="ultimaFilaTabla"></param>
        public static void GenerarCharExcelResumenProduccionAnual(ExcelWorksheet ws, FechasPR5 objFecha, TablaReporte tablaData, int filaIniEmpresa, out int ultimaFilaTabla)
        {
            string tipoVistaReporte = objFecha.TipoVistaReporte;
            int tipoDoc = objFecha.TipoReporte;

            var dataCab = tablaData.Cabecera.CabeceraData;
            var registrosDetalle = tablaData.ListaRegistros.Where(x => !x.EsFilaResumen).ToList();
            var registrosTotal = tablaData.ListaRegistros.Where(x => x.EsFilaResumen).ToList();

            int coluIniEmpresa = 2;
            ultimaFilaTabla = 0;

            //iterar por cada subcuadro
            if (registrosDetalle.Any())
            {
                int filaIniData = filaIniEmpresa + 4;
                int coluIniData = coluIniEmpresa;

                int ultimaFila = 0;
                int ultimaColu = 0;

                #region cabecera
                ws.Cells[filaIniEmpresa, coluIniEmpresa + 0].Value = dataCab[0, 0];
                ws.Cells[filaIniEmpresa, coluIniEmpresa + 1].Value = dataCab[0, 1];
                ws.Cells[filaIniEmpresa, coluIniEmpresa + 2].Value = dataCab[0, 2];
                ws.Cells[filaIniEmpresa, coluIniEmpresa + 6].Value = dataCab[0, 6];

                ws.Cells[filaIniEmpresa + 1, coluIniEmpresa + 2].Value = dataCab[1, 2];
                ws.Cells[filaIniEmpresa + 1, coluIniEmpresa + 5].Value = dataCab[1, 5];
                ws.Cells[filaIniEmpresa + 1, coluIniEmpresa + 6].Value = dataCab[1, 6];

                ws.Cells[filaIniEmpresa + 2, coluIniEmpresa + 2].Value = dataCab[2, 2];
                ws.Cells[filaIniEmpresa + 2, coluIniEmpresa + 3].Value = dataCab[2, 3];
                ws.Cells[filaIniEmpresa + 2, coluIniEmpresa + 4].Value = dataCab[2, 4];
                ws.Cells[filaIniEmpresa + 2, coluIniEmpresa + 5].Value = dataCab[2, 5];
                ws.Cells[filaIniEmpresa + 2, coluIniEmpresa + 6].Value = dataCab[2, 6];

                ws.Cells[filaIniEmpresa + 3, coluIniEmpresa + 2].Value = dataCab[3, 2];
                ws.Cells[filaIniEmpresa + 3, coluIniEmpresa + 3].Value = dataCab[3, 3];
                ws.Cells[filaIniEmpresa + 3, coluIniEmpresa + 4].Value = dataCab[3, 4];
                ws.Cells[filaIniEmpresa + 3, coluIniEmpresa + 5].Value = dataCab[3, 5];
                ws.Cells[filaIniEmpresa + 3, coluIniEmpresa + 6].Value = dataCab[3, 6];

                #endregion

                ultimaColu = coluIniEmpresa + 5;

                #region cuerpo

                ultimaFila = filaIniData + registrosDetalle.Count() - 1;
                ultimaFilaTabla = ultimaFila;

                //border de la tabla
                UtilExcel.CeldasExcelColorFondo(ws, filaIniData, coluIniData, ultimaFila, ultimaColu, "#FFFFFF");
                UtilExcel.CeldasExcelColorFondoYBorderSoloUnLado(ws, filaIniData, coluIniData, ultimaFila, coluIniData, "#0000D0", "Izquierda");
                UtilExcel.CeldasExcelColorFondoYBorderSoloUnLado(ws, filaIniData, ultimaColu, ultimaFila, ultimaColu, "#0000D0", "Derecha");

                #region Formato Cuerpo
                //UtilExcel.CeldasExcelWrapText(ws, filaIniData, coluIniData, ultimaFila, coluIniData);
                UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaIniData, coluIniData + 1, ultimaFila, ultimaColu, "Centro");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniData, coluIniData + 0, ultimaFila, coluIniData + 1, "Izquierda");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniData, coluIniData + 2, ultimaFila, ultimaColu, "Derecha");
                UtilExcel.CeldasExcelEnNegrita(ws, filaIniData, coluIniData + 0, ultimaFila, coluIniData + 0);
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniData, coluIniData, ultimaFila, ultimaColu, ConstantesPR5ReportesServicio.TipoLetraCuerpo, ConstantesPR5ReportesServicio.TamLetraCuerpo3);

                #endregion

                int filaX = 0;
                foreach (var reg in registrosDetalle)
                {
                    int colX = 0;

                    ws.Cells[filaIniData + filaX, coluIniData + colX].Value = reg.Nombre;
                    colX++;
                    if (!string.IsNullOrEmpty(reg.Nombre2))
                        ws.Cells[filaIniData + filaX, coluIniData + colX].Value = reg.Nombre2;
                    colX++;

                    if (!string.IsNullOrEmpty(reg.Nombre))
                    {
                        UtilExcel.CeldasExcelEnNegrita(ws, filaIniData + filaX, coluIniData + 0, filaIniData + filaX, coluIniData + 0);
                    }

                    if (!string.IsNullOrEmpty(reg.Nombre) && string.IsNullOrEmpty(reg.Nombre2))
                    {
                        string colorTotalEmp = "#DDEBF7";
                        UtilExcel.CeldasExcelColorFondo(ws, filaIniData + filaX, coluIniData + 0, filaIniData + filaX, ultimaColu, colorTotalEmp);
                        UtilExcel.CeldasExcelColorFondoYBorderSoloUnLado(ws, filaIniData + filaX, coluIniData + 0, filaIniData + filaX, ultimaColu, "#0000D0", "Abajo");
                    }

                    foreach (decimal? numValor in reg.ListaData)
                    {
                        string strFormat = ConstantesPR5ReportesServicio.FormatoNumero2Digito;

                        if (numValor != null)
                        {
                            ws.Cells[filaIniData + filaX, coluIniData + colX].Value = numValor;
                        }
                        ws.Cells[filaIniData + filaX, coluIniData + colX].Style.Numberformat.Format = strFormat;

                        colX++;
                    }

                    filaX++;
                }
                #endregion

            }

            //Filas resumen
            if (registrosTotal.Any())
            {
                int filaRes1 = filaIniEmpresa + 4 + registrosDetalle.Count();
                int filaResFin = filaRes1 + 2;
                int filaX = filaRes1;
                int coluIniData = coluIniEmpresa;
                int ultimaColu = coluIniEmpresa + 5;

                ultimaFilaTabla = filaResFin;

                foreach (var reg in registrosTotal)
                {
                    int colX = 0;

                    ws.Cells[filaX, coluIniData + colX].Value = reg.Nombre;
                    colX += 2; ;

                    foreach (decimal? numValor in reg.ListaData)
                    {
                        string strFormat = ConstantesPR5ReportesServicio.FormatoNumero2Digito;

                        if (numValor != null && numValor != 0)
                        {
                            ws.Cells[filaX, coluIniData + colX].Value = numValor;
                        }
                        ws.Cells[filaX, coluIniData + colX].Style.Numberformat.Format = strFormat;

                        colX++;
                    }
                    filaX++;
                }

                #region Formato Cuerpo
                UtilExcel.CeldasExcelWrapText(ws, filaRes1, coluIniData, filaResFin, ultimaColu);
                UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaRes1, coluIniData, filaResFin, ultimaColu, "Centro");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaRes1, coluIniData, filaResFin, coluIniData + 1, "Izquierda");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaRes1, coluIniData + 2, filaResFin, ultimaColu, "Derecha");
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaRes1, coluIniData, filaResFin, ultimaColu, ConstantesPR5ReportesServicio.TipoLetraCuerpo, ConstantesPR5ReportesServicio.TamLetraCuerpo3);

                UtilExcel.CeldasExcelColorTexto(ws, filaRes1, coluIniData, filaResFin, ultimaColu, "#FFFFFF");
                UtilExcel.CeldasExcelColorFondo(ws, filaRes1, coluIniData, filaResFin, ultimaColu, "#0077A5");

                UtilExcel.BorderCeldas2(ws, filaRes1, coluIniData, filaResFin, ultimaColu);

                UtilExcel.CeldasExcelEnNegrita(ws, filaRes1, coluIniData, filaResFin, ultimaColu);

                #endregion
            }
        }

        #endregion

        #region 9.2 Máxima Potencia Coincidente Anual

        public static TablaReporte ObtenerDataTablaMaximaDemandaAnual(FechasPR5 objFecha, List<MaximaDemandaDTO> listaMDCoincidenteDataDesc,
                        List<SiEmpresaDTO> listaEmpresa, List<EqEquipoDTO> listaCentral,
                        List<ResultadoTotalGeneracion> listaMDXCentral, List<ResultadoTotalGeneracion> listaMDXEmpresa,
                        List<ResultadoTotalGeneracion> listaTIEMD, List<ResultadoTotalGeneracion> listaMDTotal)
        {
            var regMDAct = listaMDCoincidenteDataDesc.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct);
            var regMDAnt = listaMDCoincidenteDataDesc.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnio1Ant_SemAct);

            TablaReporte tabla = new TablaReporte();
            tabla.ReptiCodiTabla = objFecha.TipoReporte;
            CabeceraReporte cabRepo = new CabeceraReporte();

            string[,] matrizCabecera = new string[4, 5];

            matrizCabecera[0, 0] = "Empresa";
            matrizCabecera[0, 1] = "Central";
            matrizCabecera[0, 2] = "Máxima potencia coincidente (MW)";

            matrizCabecera[1, 2] = objFecha.AnioAct.NumAnio.ToString();
            matrizCabecera[1, 3] = objFecha.Anio1Ant.NumAnio.ToString();
            matrizCabecera[1, 4] = string.Format("{0} / {1}", objFecha.AnioAct.NumAnio, objFecha.Anio1Ant.NumAnio);

            matrizCabecera[2, 2] = regMDAct.FechaOnlyDia;
            matrizCabecera[2, 3] = regMDAnt.FechaOnlyDia;
            matrizCabecera[2, 4] = "Variación";

            matrizCabecera[3, 2] = regMDAct.FechaOnlyHora;
            matrizCabecera[3, 3] = regMDAnt.FechaOnlyHora;
            matrizCabecera[3, 4] = "%";

            cabRepo.CabeceraData = matrizCabecera;
            tabla.Cabecera = cabRepo;

            #region cuerpo
            List<RegistroReporte> registros = new List<RegistroReporte>();

            var listaFila = new List<EqEquipoDTO>();
            foreach (var regEmp in listaEmpresa)
            {
                var listaEqXEmp = listaCentral.Where(x => x.Emprcodi == regEmp.Emprcodi).ToList();
                int i = 0;
                foreach (var regGr in listaEqXEmp)
                {
                    string empFila = i == 0 ? regEmp.Emprnomb : "";
                    listaFila.Add(new EqEquipoDTO() { Emprcodi = 0, Equipadre = regGr.Equipadre, Emprnomb = empFila, Central = regGr.Central });
                    i++;
                }
                listaFila.Add(new EqEquipoDTO() { Emprcodi = regEmp.Emprcodi, Equipadre = 0, Emprnomb = string.Format("Total {0}", regEmp.Emprnomb), Central = "" });
            }

            //filas de centrales y empresas
            foreach (var regFila in listaFila)
            {
                RegistroReporte registro = new RegistroReporte();
                List<decimal?> datos = new List<decimal?>();

                var listaDataXFila = regFila.Equipadre > 0 ? listaMDXCentral.Where(x => x.Equipadre == regFila.Equipadre).ToList() : listaMDXEmpresa.Where(x => x.Emprcodi == regFila.Emprcodi).ToList();

                ResultadoTotalGeneracion regMDAnio0G = listaDataXFila.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct);
                ResultadoTotalGeneracion regMDAnio1G = listaDataXFila.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnio1Ant_SemAct);
                ResultadoTotalGeneracion regMDVarAnio0G = listaDataXFila.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct_Var);

                datos.Add(regMDAnio0G.Meditotal);
                datos.Add(regMDAnio1G.Meditotal);
                datos.Add(regMDVarAnio0G.Meditotal);

                registro.Nombre = regFila.Emprnomb;
                registro.Nombre2 = regFila.Central;
                registro.ListaData = datos;

                registros.Add(registro);
            }

            //fila totales y TIE
            List<GenericoDTO> listaFilaUltimo = new List<GenericoDTO>();
            listaFilaUltimo.AddRange(ListarFilaCuadro8_2Gen());
            listaFilaUltimo.AddRange(ListarFilaCuadro8_2TIE());
            listaFilaUltimo.AddRange(ListarFilaCuadro8_2Sein());
            foreach (var regFila in listaFilaUltimo)
            {
                RegistroReporte registro = new RegistroReporte();
                List<decimal?> datos = new List<decimal?>();

                var listaDataXFila = new List<ResultadoTotalGeneracion>();
                if (regFila.Entero1 == ConstantesSiosein2.FilaCuadroTotalGeneracion) listaDataXFila = listaMDTotal.Where(x => x.TipoSemanaRelProd == regFila.Entero1.Value).ToList();
                if (regFila.Entero1 == ConstantesSiosein2.FilaCuadroImportacion) listaDataXFila = listaTIEMD.Where(x => x.TipoSemanaRelProd == regFila.Entero1.Value).ToList();
                if (regFila.Entero1 == ConstantesSiosein2.FilaCuadroExportacion) listaDataXFila = listaTIEMD.Where(x => x.TipoSemanaRelProd == regFila.Entero1.Value).ToList();
                if (regFila.Entero1 == ConstantesSiosein2.FilaCuadroTotalSein) listaDataXFila = listaMDTotal.Where(x => x.TipoSemanaRelProd == regFila.Entero1.Value).ToList();

                ResultadoTotalGeneracion regMDAnio0G = listaDataXFila.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct);
                ResultadoTotalGeneracion regMDAnio1G = listaDataXFila.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnio1Ant_SemAct);
                ResultadoTotalGeneracion regMDVarAnio0G = listaDataXFila.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct_Var);

                datos.Add(regMDAnio0G.Meditotal);
                datos.Add(regMDAnio1G.Meditotal);
                datos.Add(regMDVarAnio0G.Meditotal);

                registro.Nombre = regFila.String1;
                registro.EsFilaResumen = true;

                registro.ListaData = datos;

                registros.Add(registro);
            }

            #endregion

            tabla.ListaRegistros = registros;

            return tabla;
        }

        public static string ListarResumenMaximaDemandaAnualHtml(TablaReporte tablaData)
        {
            var dataCab = tablaData.Cabecera.CabeceraData;
            var registros = tablaData.ListaRegistros;

            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi2 = UtilAnexoAPR5.GenerarNumberFormatInfo2();
            var tamTabla = 1170;

            strHtml.Append("<div id='listado_reporte' style='height: auto; width: " + tamTabla + "px;'>");

            strHtml.Append("<table id='reporte' class='pretty tabla-icono' style='width: " + (tamTabla - 30) + "px;'>");

            #region cabecera

            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.AppendFormat("<th rowspan='4' style='width: 285px;'>{0}</th>", dataCab[0, 0]);
            strHtml.AppendFormat("<th rowspan='4' style='width: 285px;'>{0}</th>", dataCab[0, 1]);
            strHtml.AppendFormat("<th colspan='4'>{0}</th>", dataCab[0, 2]);
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.AppendFormat("<th style='width:100px;'>{0}</th>", dataCab[1, 2]);
            strHtml.AppendFormat("<th style='width:100px;'>{0}</th>", dataCab[1, 3]);
            strHtml.AppendFormat("<th style='width:100px;'>{0}</th>", dataCab[1, 4]);
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.AppendFormat("<th style='width:100px;'>{0}</th>", dataCab[2, 2]);
            strHtml.AppendFormat("<th style='width:100px;'>{0}</th>", dataCab[2, 3]);
            strHtml.AppendFormat("<th style='width:100px;'>{0}</th>", dataCab[2, 4]);
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.AppendFormat("<th style='width:100px;'>{0}</th>", dataCab[3, 2]);
            strHtml.AppendFormat("<th style='width:100px;'>{0}</th>", dataCab[3, 3]);
            strHtml.AppendFormat("<th style='width:100px;'>{0}</th>", dataCab[3, 4]);
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            #endregion

            #region cuerpo
            strHtml.Append("<tbody>");
            int f = 0;
            foreach (var reg in registros)
            {
                if (!reg.EsFilaResumen)
                {
                    string styleTotalEmp = !string.IsNullOrEmpty(reg.Nombre) && string.IsNullOrEmpty(reg.Nombre2) ? "background-color: #DDEBF7;" : "";

                    strHtml.Append("<tr>");
                    strHtml.AppendFormat("<td style='padding-left: 5px;text-align: left;font-weight:bold;{1}'>{0}</td>", reg.Nombre, styleTotalEmp);
                    strHtml.AppendFormat("<td style='padding-left: 5px;text-align: left;{1}'>{0}</td>", reg.Nombre2, styleTotalEmp);

                    int c = 2;
                    foreach (decimal? col in reg.ListaData)
                    {
                        if (c == 4) //con signo  de %
                            strHtml.AppendFormat("<td class='alignValorRight' style='{1}'>{0}</td>", UtilAnexoAPR5.ImprimirVariacionHtml(col.HasValue ? col : null, nfi2), styleTotalEmp);
                        else
                            strHtml.AppendFormat("<td class='alignValorRight' style='{1}'>{0}</td>", UtilAnexoAPR5.ImprimirValorTotalOcultar0Html(col.HasValue ? col : null, nfi2), styleTotalEmp);

                        c++;
                    }
                }
                strHtml.Append("</tr>");
                f++;
            }
            strHtml.Append("</tbody>");

            strHtml.Append("<thead>");
            foreach (var reg in registros)
            {
                if (reg.EsFilaResumen)
                {
                    strHtml.Append("<tr>");
                    strHtml.AppendFormat("<th colspan='2' style='padding-left: 5px;text-align: left;font-weight:bold;'>{0}</t>", reg.Nombre);

                    int c = 2;
                    foreach (decimal? col in reg.ListaData)
                    {
                        if (c == 5) //con signo  de %
                            strHtml.AppendFormat("<th class='alignValorRight' style=''>{0}</th>", UtilAnexoAPR5.ImprimirVariacionHtml(col.HasValue ? col : null, nfi2));
                        else
                            strHtml.AppendFormat("<th class='alignValorRight' style=''>{0}</th>", UtilAnexoAPR5.ImprimirValorTotalOcultar0Html(col.HasValue ? col : null, nfi2));

                        c++;
                    }

                    strHtml.Append("</tr>");
                }
            }
            strHtml.Append("</thead>");
            strHtml.Append("</table>");

            #endregion

            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        public static void GenerarCharExcelResumenMaximaDemandaAnual(ExcelWorksheet ws, FechasPR5 objFecha, TablaReporte tablaData, int filaIniEmpresa, out int ultimaFilaTabla)
        {
            string tipoVistaReporte = objFecha.TipoVistaReporte;
            int tipoDoc = objFecha.TipoReporte;

            var dataCab = tablaData.Cabecera.CabeceraData;
            var registrosDetalle = tablaData.ListaRegistros.Where(x => !x.EsFilaResumen).ToList();
            var registrosTotal = tablaData.ListaRegistros.Where(x => x.EsFilaResumen).ToList();

            int coluIniEmpresa = 2;
            ultimaFilaTabla = 0;

            //iterar por cada subcuadro
            if (registrosDetalle.Any())
            {
                int filaIniData = filaIniEmpresa + 4;
                int coluIniData = coluIniEmpresa;

                int ultimaFila = 0;
                int ultimaColu = 0;

                #region cabecera
                ws.Cells[filaIniEmpresa, coluIniEmpresa + 0].Value = dataCab[0, 0];
                ws.Cells[filaIniEmpresa, coluIniEmpresa + 1].Value = dataCab[0, 1];
                ws.Cells[filaIniEmpresa, coluIniEmpresa + 2].Value = dataCab[0, 2];

                ws.Cells[filaIniEmpresa + 1, coluIniEmpresa + 2].Value = dataCab[1, 2];
                ws.Cells[filaIniEmpresa + 1, coluIniEmpresa + 3].Value = dataCab[1, 3];
                ws.Cells[filaIniEmpresa + 1, coluIniEmpresa + 4].Value = dataCab[1, 4];

                ws.Cells[filaIniEmpresa + 2, coluIniEmpresa + 2].Value = dataCab[2, 2];
                ws.Cells[filaIniEmpresa + 2, coluIniEmpresa + 3].Value = dataCab[2, 3];
                ws.Cells[filaIniEmpresa + 2, coluIniEmpresa + 4].Value = dataCab[2, 4];

                ws.Cells[filaIniEmpresa + 3, coluIniEmpresa + 2].Value = dataCab[3, 2];
                ws.Cells[filaIniEmpresa + 3, coluIniEmpresa + 3].Value = dataCab[3, 3];
                ws.Cells[filaIniEmpresa + 3, coluIniEmpresa + 4].Value = dataCab[3, 4];

                #endregion

                ultimaColu = coluIniEmpresa + 4;

                #region cuerpo

                ultimaFila = filaIniData + registrosDetalle.Count() - 1;
                ultimaFilaTabla = ultimaFila;

                //border de la tabla
                UtilExcel.CeldasExcelColorFondo(ws, filaIniData, coluIniData, ultimaFila, ultimaColu, "#FFFFFF");
                UtilExcel.CeldasExcelColorFondoYBorderSoloUnLado(ws, filaIniData, coluIniData, ultimaFila, coluIniData, "#0000D0", "Izquierda");
                UtilExcel.CeldasExcelColorFondoYBorderSoloUnLado(ws, filaIniData, ultimaColu, ultimaFila, ultimaColu, "#0000D0", "Derecha");

                #region Formato Cuerpo
                //UtilExcel.CeldasExcelWrapText(ws, filaIniData, coluIniData, ultimaFila, coluIniData);
                UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaIniData, coluIniData + 1, ultimaFila, ultimaColu, "Centro");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniData, coluIniData + 0, ultimaFila, coluIniData + 1, "Izquierda");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniData, coluIniData + 2, ultimaFila, ultimaColu, "Derecha");
                UtilExcel.CeldasExcelEnNegrita(ws, filaIniData, coluIniData + 0, ultimaFila, coluIniData + 0);
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniData, coluIniData, ultimaFila, ultimaColu, ConstantesPR5ReportesServicio.TipoLetraCuerpo, ConstantesPR5ReportesServicio.TamLetraCuerpo3);

                #endregion

                int filaX = 0;
                foreach (var reg in registrosDetalle)
                {
                    int colX = 0;

                    ws.Cells[filaIniData + filaX, coluIniData + colX].Value = reg.Nombre;
                    colX++;
                    if (!string.IsNullOrEmpty(reg.Nombre2))
                        ws.Cells[filaIniData + filaX, coluIniData + colX].Value = reg.Nombre2;
                    colX++;

                    if (!string.IsNullOrEmpty(reg.Nombre))
                    {
                        UtilExcel.CeldasExcelEnNegrita(ws, filaIniData + filaX, coluIniData + 0, filaIniData + filaX, coluIniData + 0);
                    }

                    if (!string.IsNullOrEmpty(reg.Nombre) && string.IsNullOrEmpty(reg.Nombre2))
                    {
                        string colorTotalEmp = "#DDEBF7";
                        UtilExcel.CeldasExcelColorFondo(ws, filaIniData + filaX, coluIniData + 0, filaIniData + filaX, ultimaColu, colorTotalEmp);
                        UtilExcel.CeldasExcelColorFondoYBorderSoloUnLado(ws, filaIniData + filaX, coluIniData + 0, filaIniData + filaX, ultimaColu, "#0000D0", "Abajo");
                    }

                    foreach (decimal? numValor in reg.ListaData)
                    {
                        string strFormat = ConstantesPR5ReportesServicio.FormatoNumero2Digito;

                        if (numValor != null)
                        {
                            ws.Cells[filaIniData + filaX, coluIniData + colX].Value = numValor;
                        }
                        ws.Cells[filaIniData + filaX, coluIniData + colX].Style.Numberformat.Format = strFormat;

                        colX++;
                    }

                    filaX++;
                }
                #endregion
            }

            //Filas resumen
            if (registrosTotal.Any())
            {
                int filaRes1 = filaIniEmpresa + 4 + registrosDetalle.Count();
                int filaResFin = filaRes1 + 3;
                int filaX = filaRes1;
                int coluIniData = coluIniEmpresa;
                int ultimaColu = coluIniEmpresa + 4;

                ultimaFilaTabla = filaResFin;

                foreach (var reg in registrosTotal)
                {
                    int colX = 0;

                    ws.Cells[filaX, coluIniData + colX].Value = reg.Nombre;
                    colX += 2; ;

                    foreach (decimal? numValor in reg.ListaData)
                    {
                        string strFormat = ConstantesPR5ReportesServicio.FormatoNumero2Digito;

                        if (numValor != null && numValor != 0)
                        {
                            ws.Cells[filaX, coluIniData + colX].Value = numValor;
                        }
                        ws.Cells[filaX, coluIniData + colX].Style.Numberformat.Format = strFormat;

                        colX++;
                    }
                    filaX++;
                }

                #region Formato Cuerpo
                UtilExcel.CeldasExcelWrapText(ws, filaRes1, coluIniData, filaResFin, ultimaColu);
                UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaRes1, coluIniData, filaResFin, ultimaColu, "Centro");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaRes1, coluIniData, filaResFin, coluIniData + 1, "Izquierda");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaRes1, coluIniData + 2, filaResFin, ultimaColu, "Derecha");
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaRes1, coluIniData, filaResFin, ultimaColu, ConstantesPR5ReportesServicio.TipoLetraCuerpo, ConstantesPR5ReportesServicio.TamLetraCuerpo3);

                UtilExcel.CeldasExcelColorTexto(ws, filaRes1, coluIniData, filaResFin, ultimaColu, "#FFFFFF");
                UtilExcel.CeldasExcelColorFondo(ws, filaRes1, coluIniData, filaResFin, ultimaColu, "#0077A5");

                UtilExcel.BorderCeldas2(ws, filaRes1, coluIniData, filaResFin, ultimaColu);

                UtilExcel.CeldasExcelEnNegrita(ws, filaRes1, coluIniData, filaResFin, ultimaColu);

                #endregion
            }
        }

        #endregion

        #endregion

        #region Util

        /// <summary>
        /// ConvertirMwhaMw
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="medicion"></param>
        /// <returns></returns>
        public static decimal ConvertirMwhaMw(decimal valor, ConstantesSiosein2.TipoMedicion medicion)
        {
            decimal resultado;
            switch (medicion)
            {
                case ConstantesSiosein2.TipoMedicion.Medicion24:
                    resultado = (decimal)valor;
                    break;
                case ConstantesSiosein2.TipoMedicion.Medicion48:
                    resultado = (decimal)valor * 2;
                    break;
                case ConstantesSiosein2.TipoMedicion.Medicion96:
                    resultado = (decimal)valor * 4;
                    break;

                default:
                    throw new ArgumentOutOfRangeException("medicion", medicion, null);
            }

            return resultado;
        }

        /// <summary>
        /// ConvertirMwaMwh
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="medicion"></param>
        /// <returns></returns>
        public static decimal? ConvertirMwaMwh(decimal? valor, ConstantesSiosein2.TipoMedicion medicion)
        {
            if (valor == null) return null;
            if (valor == 0M) return 0M;

            decimal resultado;
            resultado = ConvertirMwaMwh(valor.Value, medicion);

            return resultado;
        }

        /// <summary>
        /// ConvertirMwaMwh
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="medicion"></param>
        /// <returns></returns>
        public static decimal ConvertirMwaMwh(decimal valor, ConstantesSiosein2.TipoMedicion medicion)
        {
            decimal resultado;
            switch (medicion)
            {
                case ConstantesSiosein2.TipoMedicion.Medicion24:
                    resultado = (decimal)valor;
                    break;
                case ConstantesSiosein2.TipoMedicion.Medicion48:
                    resultado = (decimal)valor / 2;
                    break;
                case ConstantesSiosein2.TipoMedicion.Medicion96:
                    resultado = (decimal)valor / 4;
                    break;

                default:
                    throw new ArgumentOutOfRangeException("medicion", medicion, null);
            }

            return resultado;
        }

        public static decimal? ConvertirMWhaGWh(decimal? valor)
        {
            return (valor.HasValue) ? valor / 1000 : valor;
        }

        /// <summary>
        /// Calcula la variación porcentual de dos valores
        /// </summary>
        /// <param name="primerValor">Primer valor</param>
        /// <param name="segundoValor">Segundo valor</param>
        /// <returns></returns>
        public static decimal VariacionPorcentual(decimal primerValor, decimal segundoValor)
        {
            if (segundoValor == 0) return 0;
            return ((primerValor - segundoValor) / segundoValor) * 100M;
        }

        /// <summary>
        /// Calcula la variación porcentual de dos valores
        /// </summary>
        /// <param name="primerValor">Primer valor</param>
        /// <param name="segundoValor">Segundo valor</param>
        /// <returns></returns>
        public static decimal? VariacionPorcentual(decimal? primerValor, decimal? segundoValor)
        {
            if (!primerValor.HasValue || !segundoValor.HasValue) return null;
            return VariacionPorcentual(primerValor.Value, segundoValor.Value);
        }

        /// <summary>
        /// Calcula la variación porcentual de dos valores
        /// </summary>
        /// <param name="primerValor">Primer valor</param>
        /// <param name="segundoValor">Segundo valor</param>
        /// <returns></returns>
        public static decimal? VariacionPorcentual(decimal? primerValor, decimal segundoValor)
        {
            if (!primerValor.HasValue) return null;
            return VariacionPorcentual(primerValor.Value, segundoValor);
        }

        #endregion

    }

}
