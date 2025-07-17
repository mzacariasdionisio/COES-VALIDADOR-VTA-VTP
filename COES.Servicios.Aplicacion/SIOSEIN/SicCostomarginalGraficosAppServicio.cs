using COES.Base.Core;
using COES.Base.Tools;
using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.Framework.Base.Core;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.Despacho;
using COES.Servicios.Aplicacion.Despacho.Helper;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.General.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Hidrologia;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Indisponibilidades;
using COES.Servicios.Aplicacion.IntercambioOsinergmin;
using COES.Servicios.Aplicacion.IntercambioOsinergmin.Helper;
using COES.Servicios.Aplicacion.Interconexiones;
using COES.Servicios.Aplicacion.Interconexiones.Helper;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using COES.Servicios.Aplicacion.Migraciones;
using COES.Servicios.Aplicacion.Siosein2;
using COES.Servicios.Aplicacion.Transferencias;
using COES.Servicios.Aplicacion.TransfPotencia;
using log4net;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace COES.Servicios.Aplicacion.SIOSEIN
{
    public partial class SicCostomarginalGraficosAppServicio : AppServicioBase
    {

        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SicCostomarginalGraficosAppServicio));


        /// <summary>
        /// 
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        /// 
        public ResultadoDTO<List<BarraDTO>> ListarBarraConFechaCMG(CostoMarginalDTO parametro)
        {
            FactoryTransferencia.GetPeriodoRepository().GetById(parametro.PeriCodi);
            ResultadoDTO<List<BarraDTO>> resultado = new ResultadoDTO<List<BarraDTO>>();
            resultado.Data = FactorySic.GetSicCostomarginalGraficosRepository().ListarBarrasPorCMG(parametro);

            return resultado;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        public ResultadoDTO<List<BarraDTO>> ListarBarrasPorCMGDiarioMensual(CostoMarginalDTO parametro)
        {
            FactoryTransferencia.GetPeriodoRepository().GetById(parametro.PeriCodi);
            ResultadoDTO<List<BarraDTO>> resultado = new ResultadoDTO<List<BarraDTO>>();
            if (parametro.tipoPromedio == "D")
                resultado.Data = FactorySic.GetSicCostomarginalGraficosRepository().ListarBarrasPorCMGDiario(parametro);
            else if (parametro.tipoPromedio == "M")
                resultado.Data = FactorySic.GetSicCostomarginalGraficosRepository().ListarBarrasPorCMGMensual(parametro);

            return resultado;
        }

        /// <summary>
        /// Lista los codigos marginales por barra
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        public ResultadoDTO<List<CostoMarginalGraficosDTO>> ListarCostoMarginalTotalPorBarra(CostoMarginalDTO parametro)
        {
            int columnaInicio = 0;
            int columnaFin = 0;
            string columName = "";
            DataTable tabla = new DataTable();
            //FactoryTransferencia.GetPeriodoRepository().GetById(parametro.PeriCodi);
            ResultadoDTO<List<CostoMarginalGraficosDTO>> resultado = new ResultadoDTO<List<CostoMarginalGraficosDTO>>();
            resultado.Data = new List<CostoMarginalGraficosDTO>();

            List<BarraDTO> paramBarras = new List<BarraDTO>();
            List<CostoMarginalGraficoValoresDTO> paramCMG = new List<CostoMarginalGraficoValoresDTO>();
            paramBarras = FactorySic.GetSicCostomarginalGraficosRepository().ListarBarrasPorArray(parametro);
            //
            PeriodoDTO objPeriodo = new PeriodoAppServicio().GetByIdPeriodo(parametro.PeriCodi);
            if (parametro.TipoCostoMarginal == "CMGTO")
            {
                columnaInicio = 8;
                columnaFin = 104;
                columName = "COSMARDIA";
                tabla = FactorySic.GetSicCostomarginalGraficosRepository().ListarCostoMarginalTotalPorBarras_NEW(parametro);
                //paramCMG = FactorySic.GetSicCostomarginalGraficosRepository().ListarCostoMarginalTotalPorBarras(parametro);

            }
            else if (parametro.TipoCostoMarginal == "CMGCN")
            {
                columnaInicio = 9;
                columnaFin = 57;
                columName = "CONGENEDIA";
                tabla = FactorySic.GetSicCostomarginalGraficosRepository().ListarCostoMarginalCongestionPorBarras_NEW(1, parametro);
            }
            else if (parametro.TipoCostoMarginal == "CMGEN")
            {
                columnaInicio = 9;
                columnaFin = 57;
                columName = "CONGENEDIA";
                tabla = FactorySic.GetSicCostomarginalGraficosRepository().ListarCostoMarginalCongestionPorBarras_NEW(2, parametro);
            }
            //DataTable tbl = FactorySic.GetSicCostomarginalGraficosRepository().ListarBarrasPorCMG2(parametro);

            #region Convertir Grafica
            for (int i = 0; i < tabla.Rows.Count; i++)
            {
                DataRow rows = tabla.Rows[i];
                int intervalo = 0;
                int intervalorOriginal = 0;
                int barrCodi = Convert.ToInt32(rows["barrcodi"].ToString());
                int cosMardia = Convert.ToInt32(rows[columName].ToString());
                //Obtener intervalos
                for (int c = 0; c < tabla.Columns.Count; c++)
                {
                    if (c >= columnaInicio && c < columnaFin)
                    {
                        if (parametro.TipoCostoMarginal == "CMGTO")
                            intervalo++;
                        else
                            intervalo = 1;



                        if ((intervalo % 2) != 0)
                        {
                            intervalorOriginal++;
                            string dia = ("0" + cosMardia.ToString()); ;
                            dia = dia.Substring(dia.Length - 2);
                            string mes = ("0" + objPeriodo.MesCodi.ToString()).PadRight(2);
                            mes = mes.Substring(mes.Length - 2);
                            string fecha = string.Format("{0}/{1}/{2}", dia, mes, objPeriodo.AnioCodi);
                            CostoMarginalGraficoValoresDTO entidad = new CostoMarginalGraficoValoresDTO();
                            entidad.BarrCodi = barrCodi;
                            entidad.FechaColumna = string.Format("{0}_{1}", fecha, intervalorOriginal);
                            if (parametro.TipoCostoMarginal == "CMGTO")
                                entidad.CMGRTotal = rows[c] == DBNull.Value ? null : (decimal?)Convert.ToDecimal(rows[c].ToString());
                            else if (parametro.TipoCostoMarginal == "CMGCN")
                                entidad.CMGRCongestion = rows[c] == DBNull.Value ? null : (decimal?)Convert.ToDecimal(rows[c].ToString());
                            else if (parametro.TipoCostoMarginal == "CMGEN")
                                entidad.CMGREnergia = rows[c] == DBNull.Value ? null : (decimal?)Convert.ToDecimal(rows[c].ToString());

                            paramCMG.Add(entidad);
                        }


                    }
                }
            }
            #endregion Convertir Grafica

            #region rango de fechas
            Dictionary<string, DateTime> fechaRangos = new Dictionary<string, DateTime>();
            DateTime inicio = new DateTime(objPeriodo.AnioCodi, objPeriodo.MesCodi, parametro.DiaInicio);
            DateTime fin = new DateTime(objPeriodo.AnioCodi, objPeriodo.MesCodi, parametro.DiaFin);
            double diferencia = (inicio.Date - fin.Date).TotalDays;
            List<DateTime> allDates = new List<DateTime>();
            for (DateTime date = inicio; date <= fin; date = date.AddDays(1))
                allDates.Add(date);
            foreach (var item in allDates)
            {
                TimeSpan tiempo = new TimeSpan(00, 30, 0);
                for (int i = 1; i <= 48; i++)
                {
                    DateTime fechaHora = item.Add(tiempo);
                    fechaRangos.Add(string.Format("{0}_{1}", item.ToString("dd/MM/yyyy"), i), fechaHora);
                    if (i == 47)
                        tiempo = (fechaHora.AddMinutes(29).TimeOfDay);
                    else
                        tiempo = (fechaHora.AddMinutes(30).TimeOfDay);

                }
            }
            #endregion rango de fechas

            if (paramBarras.Count > 0)
            {
                resultado.Data = new List<CostoMarginalGraficosDTO>();

                foreach (var itemBar in paramBarras)
                {
                    CostoMarginalGraficosDTO entidad = new CostoMarginalGraficosDTO();
                    entidad.BarrNombre = itemBar.BarrNombre;
                    #region Intervalos fechas
                    List<CostoMarginalGraficoValoresDTO> resultCosto = paramCMG.Where(x => x.BarrCodi == itemBar.BarrCodi).ToList();
                    foreach (var item in resultCosto)
                    {
                        item.FechaIntervalo = fechaRangos[item.FechaColumna];
                        item.FechaColumna = fechaRangos[item.FechaColumna].ToString();
                    }
                    entidad.CostosMarginales = resultCosto;
                    #endregion Intervalos fechas
                    resultado.Data.Add(entidad);
                }
            }


            return resultado;
        }


        #region Reportes desviacion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        public ResultadoDTO<List<CostoMarginalGraficosDTO>> ListarCostoMarginalDesviacion(CostoMarginalDesviacionDTO parametro)
        {
            DataTable tbl = new DataTable();
            ResultadoDTO<List<CostoMarginalGraficosDTO>> resultado = new ResultadoDTO<List<CostoMarginalGraficosDTO>>();
            resultado.Data = new List<CostoMarginalGraficosDTO>();
            if (parametro.TipoCostoMarginal == "CMGTO")
                tbl = FactorySic.GetSicCostomarginalGraficosRepository().ListarCostoMarginalDesviacion(parametro);
            else if (parametro.TipoCostoMarginal == "CMGCN")
            {
                parametro.TipCosto = 1;
                tbl = FactorySic.GetSicCostomarginalGraficosRepository().ListarCostoMarginalCongestionDesviacion(parametro);
            }
            else if (parametro.TipoCostoMarginal == "CMGEN")
            {
                parametro.TipCosto = 2;
                tbl = FactorySic.GetSicCostomarginalGraficosRepository().ListarCostoMarginalCongestionDesviacion(parametro);
            }
            #region Genera Grafica
            int columIndex = -1;
            int intervaloIndice = 0;
            DateTime intervalo = new DateTime(DateTime.Now.Year, 1, 1);
            TimeSpan tiempo = new TimeSpan(00, 00, 0);
            foreach (DataColumn item in tbl.Columns)
            {
                columIndex++;
                if (columIndex > 0)
                {
                    intervaloIndice++;
                    if (intervaloIndice == 48)
                        intervalo = intervalo.Add(new TimeSpan(00, 29, 0));
                    else
                        intervalo = intervalo.Add(new TimeSpan(00, 30, 0));

                    CostoMarginalGraficosDTO entidad = new CostoMarginalGraficosDTO();
                    entidad.Hora = intervalo.ToShortTimeString();
                    entidad.FechaIntervalo = intervalo;
                    if (tbl.Rows.Count >= 1)
                        entidad.CostoMarginal1 = Convert.ToDecimal(tbl.Rows[0][columIndex] == DBNull.Value ? "0" : tbl.Rows[0][columIndex].ToString());
                    if (tbl.Rows.Count >= 2)
                        entidad.CostoMarginal2 = Convert.ToDecimal(tbl.Rows[1][columIndex] == DBNull.Value ? "0" : tbl.Rows[1][columIndex].ToString());
                    decimal desviacion = 0;
                    if (entidad.CostoMarginal1 > 0 && entidad.CostoMarginal2 > 0)
                        desviacion = ((entidad.CostoMarginal2- entidad.CostoMarginal1  ) / entidad.CostoMarginal1) * 100;

                    //desviacion = ((entidad.CostoMarginal2 - entidad.CostoMarginal1) / entidad.CostoMarginal2) * 100;
                    entidad.Desviacion = decimal.Parse(desviacion.ToString("0.00"));
                    resultado.Data.Add(entidad);
                }
            }

            #endregion Genera Grafica

            return resultado;
        }

        #endregion Reportes desviacion


        #region Reportes promedio marginales
        /// <summary>
        ///  Reporte para promedio marginales
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        public ResultadoDTO<List<CostoMarginalGraficosDTO>> ListarPromedioMarginal(CostoMarginalDTO parametro)
        {
            DataTable tabla = new DataTable();
            int columnaInicio = 0;
            int columnaFin = 0;
            string columName = "";

            //FactoryTransferencia.GetPeriodoRepository().GetById(parametro.PeriCodi);
            ResultadoDTO<List<CostoMarginalGraficosDTO>> resultado = new ResultadoDTO<List<CostoMarginalGraficosDTO>>();
            resultado.Data = new List<CostoMarginalGraficosDTO>();

            List<BarraDTO> paramBarras = new List<BarraDTO>();
            List<CostoMarginalGraficoValoresDTO> paramCMG = new List<CostoMarginalGraficoValoresDTO>();

            paramBarras = FactorySic.GetSicCostomarginalGraficosRepository().ListarBarrasPorArray(parametro);
            //

            if (parametro.TipoCostoMarginal == "CMGTO" && parametro.tipoPromedio == "M")
                paramCMG = FactorySic.GetSicCostomarginalGraficosRepository().ListarPromedioMarginalTotalMensual(parametro);
            else if (parametro.TipoCostoMarginal == "CMGTO" && parametro.tipoPromedio == "D")
                paramCMG = FactorySic.GetSicCostomarginalGraficosRepository().ListarPromedioMarginalTotalDiario(parametro);
            else if (parametro.TipoCostoMarginal == "CMGCN" && parametro.tipoPromedio == "M")
            {
                parametro.tipoCongene = 1;
                paramCMG = FactorySic.GetSicCostomarginalGraficosRepository().ListarPromedioMarginalCongeneMensual(parametro);
            }
            else if (parametro.TipoCostoMarginal == "CMGCN" && parametro.tipoPromedio == "D")
            {
                parametro.tipoCongene = 1;
                paramCMG = FactorySic.GetSicCostomarginalGraficosRepository().ListarPromedioMarginalCongeneDiario(parametro);
            }
            else if (parametro.TipoCostoMarginal == "CMGEN" && parametro.tipoPromedio == "M")
            {
                parametro.tipoCongene = 2;
                paramCMG = FactorySic.GetSicCostomarginalGraficosRepository().ListarPromedioMarginalCongeneMensual(parametro);

            }
            else if (parametro.TipoCostoMarginal == "CMGEN" && parametro.tipoPromedio == "D")
            {
                parametro.tipoCongene = 2;
                paramCMG = FactorySic.GetSicCostomarginalGraficosRepository().ListarPromedioMarginalCongeneDiario(parametro);
            }



            if (paramBarras.Count > 0)
            {
                resultado.Data = new List<CostoMarginalGraficosDTO>();

                foreach (var itemBar in paramBarras)
                {
                    CostoMarginalGraficosDTO entidad = new CostoMarginalGraficosDTO();
                    entidad.BarrNombre = itemBar.BarrNombre;
                    #region Intervalos fechas
                    List<CostoMarginalGraficoValoresDTO> resultCosto = paramCMG.Where(x => x.BarrCodi == itemBar.BarrCodi).ToList();
                    entidad.CostosMarginales = resultCosto;
                    #endregion Intervalos fechas
                    resultado.Data.Add(entidad);
                }
            }

            return resultado;
        }

        #endregion Reportes promedio marginales

    }
}
