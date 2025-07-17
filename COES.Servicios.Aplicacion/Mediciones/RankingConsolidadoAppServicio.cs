using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using System.Linq;
using log4net;
using System.Text;
using System.IO;
using OfficeOpenXml;
using System.Net;
using OfficeOpenXml.Drawing;
using System.Globalization;
using OfficeOpenXml.Style;
using System.Drawing;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.General.Helper;
using COES.Servicios.Aplicacion.IEOD;

namespace COES.Servicios.Aplicacion.Mediciones
{
    public class RankingConsolidadoAppServicio : AppServicioBase
    {
        ReporteMedidoresAppServicio servReporte = new ReporteMedidoresAppServicio();
        ParametroAppServicio servParametro = new ParametroAppServicio();
        ExcelPackage xlPackage = null;

        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ConsultaMedidoresAppServicio));

        /// <summary>
        /// Lista los tipos de generación
        /// </summary>
        /// <returns></returns>
        public List<SiTipogeneracionDTO> ListaTipoGeneracion()
        {
            return FactorySic.GetSiTipogeneracionRepository().GetByCriteria().Where(x => x.Tgenercodi != -1 && x.Tgenercodi != 0 && x.Tgenercodi != 5).ToList();
        }

        /// <summary>
        /// Permite listar los tipos de empresas
        /// </summary>
        /// <returns></returns>
        public List<SiTipoempresaDTO> ListaTipoEmpresas()
        {
            return FactorySic.GetSiTipoempresaRepository().List();
        }

        /// <summary>
        /// Lista las fuentes de energía
        /// </summary>
        /// <returns></returns>
        public List<SiFuenteenergiaDTO> ListaFuenteEnergia()
        {
            return FactorySic.GetSiFuenteenergiaRepository().GetByCriteria().Where(x => x.Fenergcodi != -1 && x.Fenergcodi != 0).ToList();
        }

        /// <summary>
        /// Permite obtener las empresa por tipo
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ObteneEmpresasPorTipo(string tiposEmpresa)
        {
            if (string.IsNullOrEmpty(tiposEmpresa)) tiposEmpresa = ConstantesAppServicio.ParametroDefecto;
            return (new IEODAppServicio()).ListarEmpresasTienenCentralGenxTipoEmpresa(tiposEmpresa);
        }

        /// <summary>
        /// Muestra el reporte de máxima demanda HFP y HP
        /// </summary>
        /// <param name="fechaini"></param>
        /// <param name="fechafin"></param>
        /// <param name="tiposEmpresa"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="tipoGeneracion"></param>
        /// <param name="tipoCentral"></param>
        /// <param name="total"></param>
        /// <param name="listOrdenado"></param>
        /// <param name="estadoValidacion"></param>
        /// <param name="fechaPeriodo"></param>
        /// <returns></returns>
        public List<DemandadiaDTO> ObtenerDemandaDiariaHFPHP(DateTime fechaini, DateTime fechafin, string tiposEmpresa, string idEmpresa,
            string tipoGeneracion, int tipoCentral, out DemandadiaDTO total, out List<DemandadiaDTO> listOrdenado, int estadoValidacion, DateTime fechaPeriodo)
        {
            List<DemandadiaDTO> entitys = new List<DemandadiaDTO>();
            List<DemandadiaDTO> ordenamiento = new List<DemandadiaDTO>();

            //Parametros para calculo de MD
            List<SiParametroValorDTO> listaRangoNormaHP = servParametro.ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroRangoPeriodoHP);
            List<SiParametroValorDTO> listaBloqueHorario = servParametro.ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroHPPotenciaActiva);

            //Data Generación
            List<MeMedicion96DTO> listaDemanda = this.servReporte.ListaDataMDGeneracion(fechaini, fechafin, tipoCentral, tipoGeneracion, idEmpresa, estadoValidacion);

            //Data Interconexion
            List<MeMedicion96DTO> listaInterconexion = this.servReporte.ListaDataMDInterconexion96(fechaini, fechafin);

            //Data Total
            List<MeMedicion96DTO> listaMedicion = this.servReporte.ListaDataMDTotalSEIN(listaDemanda, listaInterconexion);

            decimal valorH = 0;
            decimal valorInter = 0, valorGen = 0;

            decimal valorMD = 0;
            DateTime fechaMD = fechaPeriodo;
            int horaMD = 1;

            for (DateTime f = fechaini.Date; f <= fechafin.Date; f = f.AddDays(1))
            {
                MeMedicion96DTO dmdTotalDia = listaMedicion.FirstOrDefault(x => x.Medifecha.Value.Date == f);
                MeMedicion96DTO dmdGenerDia = listaDemanda.FirstOrDefault(x => x.Medifecha.Value.Date == f);
                MeMedicion96DTO dmdInterDia = listaInterconexion.FirstOrDefault(x => x.Medifecha.Value.Date == f);

                if (dmdTotalDia != null)
                {
                    for (var j = 1; j <= 96; j++)
                    {
                        valorH = ((decimal?)dmdTotalDia.GetType().GetProperty("H" + j.ToString()).GetValue(dmdTotalDia, null)).GetValueOrDefault(0);
                        valorGen = dmdGenerDia != null ? ((decimal?)dmdGenerDia.GetType().GetProperty("H" + j.ToString()).GetValue(dmdGenerDia, null)).GetValueOrDefault(0) : 0;
                        valorInter = dmdInterDia != null ? ((decimal?)dmdInterDia.GetType().GetProperty("H" + j.ToString()).GetValue(dmdInterDia, null)).GetValueOrDefault(0) : 0;

                        //Valor de la Demanda cada 15min
                        DemandadiaDTO itemOrdenamiento = new DemandadiaDTO();
                        itemOrdenamiento.Valor = valorH;
                        itemOrdenamiento.ValorInter = valorInter;
                        itemOrdenamiento.StrMediFecha = (f.Date.AddMinutes((j) * 15)).ToString(ConstantesAppServicio.FormatoFechaFull);
                        itemOrdenamiento.ValorGeneracion = valorGen;
                        ordenamiento.Add(itemOrdenamiento);
                    }

                    MedidoresHelper.ObtenerValorHXPeriodoDemandaM96(ConstantesRepMaxDemanda.TipoMDNormativa, f.Date, new List<MeMedicion96DTO>() { dmdTotalDia }, listaRangoNormaHP, listaBloqueHorario,
                                                                    out decimal valorMDDia, out int hMD, out DateTime fechaHoraMD, out _);
                    if (valorMDDia > valorMD)
                    {
                        valorMD = valorMDDia;
                        fechaMD = fechaHoraMD.Date;
                        horaMD = hMD;
                    }

                    MedidoresHelper.ObtenerValorHXPeriodoDemandaM96(ConstantesRepMaxDemanda.TipoHoraPunta, f.Date, new List<MeMedicion96DTO>() { dmdTotalDia }, null, listaBloqueHorario,
                                                                    out decimal valorHP, out int hHP, out DateTime fechaHoraHP, out _);

                    MedidoresHelper.ObtenerValorHXPeriodoDemandaM96(ConstantesRepMaxDemanda.TipoFueraHoraPunta, f.Date, new List<MeMedicion96DTO>() { dmdTotalDia }, null, listaBloqueHorario,
                                                                    out decimal valorHFP, out int hHFP, out DateTime fechaHoraHFP, out _);

                    //Obtener HP  y HFP por día
                    DemandadiaDTO entity = new DemandadiaDTO();
                    entity.ValorHFP = valorHFP;
                    entity.ValorHP = valorHP;
                    entity.Medifecha = f.Date;
                    entity.MedifechaHFP = f.Date.AddMinutes(hHFP * 15).ToString(ConstantesAppServicio.FormatoHora);
                    entity.MedifechaHP = f.Date.AddMinutes(hHP * 15).ToString(ConstantesAppServicio.FormatoHora);
                    entity.ValorHPInter = dmdInterDia != null ? ((decimal?)dmdInterDia.GetType().GetProperty("H" + hHP.ToString()).GetValue(dmdInterDia, null)).GetValueOrDefault(0) / 4 : 0;
                    entity.ValorHFPInter = dmdInterDia != null ? ((decimal?)dmdInterDia.GetType().GetProperty("H" + hHFP.ToString()).GetValue(dmdInterDia, null)).GetValueOrDefault(0) / 4 : 0;

                    entitys.Add(entity);
                }
            }

            DemandadiaDTO resultado = new DemandadiaDTO();
            resultado.ValorMD = valorMD;
            resultado.HoraMD = fechaMD.AddMinutes(horaMD * 15).ToString(ConstantesAppServicio.FormatoHora);
            resultado.FechaMD = fechaMD.ToString(ConstantesAppServicio.FormatoFecha);
            resultado.IndexHoraMD = horaMD - 1;
            total = resultado;
            listOrdenado = ordenamiento.OrderByDescending(x => x.Valor).ToList();

            return entitys;
        }

        /// <summary>
        /// Permite obtener los datos para el gráfico de evolución 
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFin"></param>
        /// <param name="tiposEmpresa"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposGeneracion"></param>
        /// <param name="central"></param>
        /// <returns></returns>
        public List<MeMedicion96DTO> ObtenerDatosEvolucion(DateTime fechaInicial, DateTime fechaFin, string tiposEmpresa, string empresas,
            string tiposGeneracion, int central)
        {
            if (tiposEmpresa != ConstantesAppServicio.ParametroDefecto && empresas == ConstantesAppServicio.ParametroDefecto)
            {
                List<int> idsEmpresas = this.ObteneEmpresasPorTipo(tiposEmpresa).Select(x => x.Emprcodi).ToList();
                empresas = string.Join<int>(ConstantesAppServicio.CaracterComa.ToString(), idsEmpresas);
            }

            if (string.IsNullOrEmpty(empresas))
                return new List<MeMedicion96DTO>();

            List<MeMedicion96DTO> list = FactorySic.GetMeMedicion96Repository().ObtenerMaximaDemandaPorRecursoEnergetico(
                fechaInicial, fechaFin, empresas, tiposGeneracion, central);

            return list;
        }

        public List<MeMedicion96DTO> ObtenerDatosEvolucionRER(DateTime fechaInicial, DateTime fechaFin, string tiposEmpresa, string empresas,
            string tiposGeneracion, int central)
        {
            if (tiposEmpresa != ConstantesAppServicio.ParametroDefecto && empresas == ConstantesAppServicio.ParametroDefecto)
            {
                List<int> idsEmpresas = this.ObteneEmpresasPorTipo(tiposEmpresa).Select(x => x.Emprcodi).ToList();
                empresas = string.Join<int>(ConstantesAppServicio.CaracterComa.ToString(), idsEmpresas);
            }

            if (string.IsNullOrEmpty(empresas))
                return new List<MeMedicion96DTO>();

            List<MeMedicion96DTO> list = FactorySic.GetMeMedicion96Repository().ObtenerMaximaDemandaPorRecursoEnergeticoRER(
                fechaInicial, fechaFin, empresas, tiposGeneracion, central);

            return list;
        }

        /// <summary>
        /// Permite obtener los registros de máximos y minimos de la demanda
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFin"></param>
        /// <param name="tiposEmpresas"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposGeneracion"></param>
        /// <param name="central"></param>
        /// <returns></returns>
        public List<MeMedicion96DTO> ObtenerDatosMaximaMinimaAcumuladada(DateTime fechaInicial, DateTime fechaFin, string tiposEmpresa,
            string empresas, string tiposGeneracion, int central)
        {
            if (tiposEmpresa != ConstantesAppServicio.ParametroDefecto && empresas == ConstantesAppServicio.ParametroDefecto)
            {
                List<int> idsEmpresas = this.ObteneEmpresasPorTipo(tiposEmpresa).Select(x => x.Emprcodi).ToList();
                empresas = string.Join<int>(ConstantesAppServicio.CaracterComa.ToString(), idsEmpresas);
            }

            if (string.IsNullOrEmpty(empresas))
                return new List<MeMedicion96DTO>();

            int anio = fechaFin.Year;
            DateTime fechaIni = new DateTime(anio, 1, 1);
            DateTime? fechaMaximo = FactorySic.GetMeMedicion96Repository().ObtenerFechaMaximaDemanda(fechaIni, fechaFin,
                empresas, tiposGeneracion, central);
            DateTime? fechaMinimo = FactorySic.GetMeMedicion96Repository().ObtenerFechaMinimaDemanda(fechaIni, fechaFin,
                empresas, tiposGeneracion, central);

            List<MeMedicion96DTO> resultado = new List<MeMedicion96DTO>();

            if (fechaMaximo != null)
            {
                /*List<MeMedicion96DTO> listMaximo = FactorySic.GetMeMedicion96Repository().ListarTotalH((DateTime)fechaMaximo, (DateTime)fechaMaximo, empresas,
                    tiposGeneracion, central);*/
                List<MeMedicion96DTO> listaDemanda = this.servReporte.ListaDataMDGeneracion(fechaMaximo.Value, fechaMaximo.Value, central, tiposGeneracion, empresas, ConstanteValidacion.EstadoTodos);
                List<MeMedicion96DTO> listaInterconexion = this.servReporte.ListaDataMDInterconexion96(fechaMaximo.Value, fechaMaximo.Value);
                List<MeMedicion96DTO> listMaximo = this.servReporte.ListaDataMDTotalSEIN(listaDemanda, listaInterconexion);

                if (listMaximo.Count > 0)
                {
                    MeMedicion96DTO maximo = listMaximo[0];
                    maximo.Indicador = ConstantesMedicion.IndicadorMaximo;
                    resultado.Add(maximo);
                }
            }
            if (fechaMinimo != null)
            {
                /*List<MeMedicion96DTO> listMinimo = FactorySic.GetMeMedicion96Repository().ListarTotalH((DateTime)fechaMinimo, (DateTime)fechaMinimo, empresas,
                    tiposGeneracion, central);*/
                List<MeMedicion96DTO> listaDemanda = this.servReporte.ListaDataMDGeneracion(fechaMinimo.Value, fechaMinimo.Value, central, tiposGeneracion, empresas, ConstanteValidacion.EstadoTodos);
                List<MeMedicion96DTO> listaInterconexion = this.servReporte.ListaDataMDInterconexion96(fechaMinimo.Value, fechaMinimo.Value);
                List<MeMedicion96DTO> listMinimo = this.servReporte.ListaDataMDTotalSEIN(listaDemanda, listaInterconexion);

                if (listMinimo.Count > 0)
                {
                    MeMedicion96DTO minimo = listMinimo[0];
                    minimo.Indicador = ConstantesMedicion.IndicadorMinimo;
                    resultado.Add(minimo);
                }
            }

            return resultado;
        }

        /// <summary>
        /// Permite obtener las empresas a validar
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<MdValidacionDTO> ObtenerValidacionMes(DateTime fecha)
        {
            return FactorySic.GetMdValidacionRepository().GetByCriteria(fecha);
        }

    }
}
