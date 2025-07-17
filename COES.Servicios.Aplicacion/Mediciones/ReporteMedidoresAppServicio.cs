using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.General.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Indisponibilidades;
using COES.Servicios.Aplicacion.Interconexiones;
using COES.Servicios.Aplicacion.Interconexiones.Helper;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using COES.Servicios.Aplicacion.Titularidad;
using log4net;
using NPOI.SS.Formula.Atp;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;

namespace COES.Servicios.Aplicacion.Mediciones
{
    public class ReporteMedidoresAppServicio : AppServicioBase
    {
        TitularidadAppServicio servTitEmp = new TitularidadAppServicio();
        EquipamientoAppServicio servEquipamiento = new EquipamientoAppServicio();
        ParametroAppServicio servParametro = new ParametroAppServicio();
        ExcelPackage xlPackage = null;

        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ReporteMedidoresAppServicio));
        InterconexionesAppServicio serInterconexion = new InterconexionesAppServicio();

        /// <summary>
        /// Lista los tipos de generación
        /// </summary>
        /// <returns></returns>
        public List<SiTipogeneracionDTO> ListaTipoGeneracion()
        {
            return FactorySic.GetSiTipogeneracionRepository().GetByCriteria().Where(x => x.Tgenercodi != -1 && x.Tgenercodi != 0 && x.Tgenercodi != 5).ToList();
        }

        /// <summary>
        /// Lista las fuentes de energía
        /// </summary>
        /// <returns></returns>
        public List<SiFuenteenergiaDTO> ListaFuenteEnergia(int idTipoGeneracion)
        {
            return FactorySic.GetSiFuenteenergiaRepository().GetByCriteria().Where(x => x.Fenergcodi != -1
                && x.Fenergcodi != 0 && (x.Tgenercodi == idTipoGeneracion || idTipoGeneracion == 0)).ToList();
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
        /// Permite listar las empresas
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListaEmpresa()
        {
            return FactorySic.GetSiEmpresaRepository().ObtenerEmpresasSEIN();
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
        /// Lista de empresas segun el tipo de generación
        /// </summary>
        /// <param name="tgenercodi"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ObteneEmpresasPorTipogeneracion(int tgenercodi)
        {
            string famcodi = "1000";

            switch (tgenercodi)
            {
                case ConstantesMedicion.IdTipoGeneracionTodos:
                    famcodi = ConstantesHorasOperacion.CodFamilias;
                    break;
                case ConstantesMedicion.IdTipoGeneracionHidrolectrica:
                    famcodi = ConstantesMedidores.CentralHidraulica.ToString();
                    break;
                case ConstantesMedicion.IdTipoGeneracionTermoelectrica:
                    famcodi = ConstantesMedidores.CentralTermica.ToString();
                    break;
                case ConstantesMedicion.IdTipoGeneracionSolar:
                    famcodi = ConstantesMedidores.CentralSolar.ToString();
                    break;
                case ConstantesMedicion.IdTipoGeneracionEolica:
                    famcodi = ConstantesMedidores.CentralEolica.ToString();
                    break;
                case ConstantesMedicion.IdTipoGeneracionNuclear:
                    break;
            }

            return FactorySic.GetSiEmpresaRepository().ListarEmpresasxTipoEquipos(famcodi, ConstantesAppServicio.ParametroDefecto).OrderBy(x => x.Emprnomb).ToList();
        }

        #region Registros de Medidores

        /// <summary>
        /// Permite obtener el reporte consolidado de máxima demanda
        /// </summary>
        /// <param name="valoresMD"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="tiposEmpresa"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposGeneracion"></param>
        /// <param name="fuentesEnergia"></param>
        /// <param name="central"></param>
        /// <returns></returns>
        public List<MedicionReporteDTO> ObtenerReporteConsolidado(List<ItemReporteMedicionDTO> valoresMD, DateTime fechaInicial, DateTime fechaFinal, string tiposEmpresa, string empresas,
            string tiposGeneracion, string fuentesEnergia, int central)
        {
            #region Parametros

            if (!string.IsNullOrEmpty(tiposGeneracion))
            {
                if (!string.IsNullOrEmpty(fuentesEnergia))
                {
                    if (tiposGeneracion != ConstantesAppServicio.ParametroDefecto && fuentesEnergia != ConstantesAppServicio.ParametroDefecto)
                    {
                        List<SiFuenteenergiaDTO> listFuente = this.ListaFuenteEnergia(0);
                        List<int> tipos = (tiposGeneracion.Split(ConstantesAppServicio.CaracterComa)).Select(n => int.Parse(n)).ToList();
                        List<int> ids = listFuente.Where(x => tipos.Any(y => x.Tgenercodi == y)).Select(x => x.Fenergcodi).ToList();
                        List<int> fuentes = (fuentesEnergia.Split(ConstantesAppServicio.CaracterComa)).Select(n => int.Parse(n)).ToList();
                        List<int> permitidos = fuentes.Where(x => ids.Any(y => x == y)).ToList();
                        fuentesEnergia = string.Join<int>(ConstantesAppServicio.CaracterComa.ToString(), permitidos);

                        if (string.IsNullOrEmpty(fuentesEnergia)) fuentesEnergia = ConstantesAppServicio.ParametroNoExiste;
                    }
                    else
                    {
                        if (tiposGeneracion != ConstantesAppServicio.ParametroDefecto)
                        {
                            List<SiFuenteenergiaDTO> listFuente = this.ListaFuenteEnergia(0);
                            List<int> tipos = (tiposGeneracion.Split(ConstantesAppServicio.CaracterComa)).Select(n => int.Parse(n)).ToList();
                            List<int> ids = listFuente.Where(x => tipos.Any(y => x.Tgenercodi == y)).Select(x => x.Fenergcodi).ToList();
                            fuentesEnergia = string.Join<int>(ConstantesAppServicio.CaracterComa.ToString(), ids);

                            if (string.IsNullOrEmpty(fuentesEnergia)) fuentesEnergia = ConstantesAppServicio.ParametroNoExiste;
                        }

                    }
                }
            }

            if (tiposEmpresa != ConstantesAppServicio.ParametroDefecto && empresas == ConstantesAppServicio.ParametroDefecto)
            {
                List<int> idsEmpresas = this.ObteneEmpresasPorTipo(tiposEmpresa).Select(x => x.Emprcodi).ToList();
                empresas = string.Join<int>(ConstantesAppServicio.CaracterComa.ToString(), idsEmpresas);
            }

            #endregion

            List<MedicionReporteDTO> resultado = new List<MedicionReporteDTO>();

            List<MeMedicion96DTO> listCon = this.ListaDataMDGeneracionConsolidado(fechaInicial, fechaFinal, central, tiposGeneracion, empresas, ConstanteValidacion.EstadoTodos, fuentesEnergia, false);

            //Data Generación
            List<MeMedicion96DTO> listaDemandaGen = this.ListaDataMDGeneracionFromConsolidado(fechaInicial, fechaFinal, listCon);

            //Data Interconexion
            List<MeMedicion96DTO> listaInterconexion = this.ListaDataMDInterconexion96(fechaInicial, fechaFinal);

            //Data Total
            List<MeMedicion96DTO> listaGeneral = this.ListaDataMDTotalSEIN(listaDemandaGen, listaInterconexion);

            //Parametros para calculo de MD
            List<SiParametroValorDTO> listaRangoNormaHP = servParametro.ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroRangoPeriodoHP);
            List<SiParametroValorDTO> listaBloqueHorario = servParametro.ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroHPPotenciaActiva);

            var list = listCon.Where(x => x.Ptomedicodi != 5020).ToList();
            var listEmpresas = list.Select(x => new { x.Emprcodi, x.Emprnomb }).Distinct().OrderBy(x => x.Emprnomb).ToList();
            var listCentrales = list.Select(x => new { x.Emprcodi, x.Central }).Distinct().ToList();
            var listFechas = list.Select(x => new { ((DateTime)x.Medifecha).Year, ((DateTime)x.Medifecha).Month }).Distinct()
                                    .Select(x => new DateTime(x.Year, x.Month, 1)).OrderBy(x => x).ToList();

            var listEquipos = list.Select(x => new
            {
                x.Emprcodi,
                x.Central,
                x.Equicodi,
                x.Equinomb,
                x.Tgenernomb,
                x.Tgenercodi,
                x.Fenergcodi,
                x.Fenergnomb
            }).Distinct().ToList();

            foreach (var itemFecha in listFechas)
            {
                //md por dia
                List<MeMedicion96DTO> listValoresXMes = listaGeneral.Where(x => ((DateTime)x.Medifecha).Year == itemFecha.Year && ((DateTime)x.Medifecha).Month == itemFecha.Month).ToList();

                //Consolidado
                decimal max = 0;
                DateTime fechaMax = new DateTime(itemFecha.Year, itemFecha.Month, 1);
                int horaMax = 1;
                decimal interconexion = 0;
                foreach (MeMedicion96DTO item in listValoresXMes)
                {
                    MeMedicion96DTO dmdInterconexionDia = listaInterconexion.Where(x => x.Medifecha.Value.Date == item.Medifecha).FirstOrDefault() ?? new MeMedicion96DTO();

                    MedidoresHelper.ObtenerValorHXPeriodoDemandaM96(ConstantesRepMaxDemanda.TipoMDNormativa, item.Medifecha.Value, new List<MeMedicion96DTO>() { item }, listaRangoNormaHP, listaBloqueHorario,
                                                                    out decimal valorMDDia, out int hMD, out DateTime fechaHoraMD, out _);

                    if (valorMDDia > max)
                    {
                        max = valorMDDia;
                        fechaMax = item.Medifecha.Value;
                        horaMax = hMD;
                        interconexion = ((decimal?)dmdInterconexionDia.GetType().GetProperty("H" + hMD.ToString()).GetValue(dmdInterconexionDia, null)).GetValueOrDefault(0);
                    }
                }

                ItemReporteMedicionDTO objMes = new ItemReporteMedicionDTO();
                objMes.Anio = itemFecha.Year;
                objMes.Mes = itemFecha.Month;
                objMes.FechaMaximaDemanda = fechaMax;
                objMes.HoraMaximaDemanda = fechaMax.AddMinutes(15 * horaMax).ToString(ConstantesAppServicio.FormatoFecha);
                objMes.MaximaDemanda = max;
                objMes.Interconexion = interconexion;
                objMes.Indice = horaMax;

                valoresMD.Add(objMes);
            }

            foreach (var itemEmpresa in listEmpresas)
            {
                var subCentral = listCentrales.Where(x => x.Emprcodi == itemEmpresa.Emprcodi).ToList();

                foreach (var itemCentral in subCentral)
                {
                    var subEquipo = listEquipos.Where(x => x.Central == itemCentral.Central && x.Emprcodi == itemCentral.Emprcodi).ToList();

                    foreach (var itemEquipo in subEquipo)
                    {

                        foreach (var itemFecha in listFechas)
                        {
                            var datosMD = valoresMD.Where(x => x.Anio == itemFecha.Year && x.Mes == itemFecha.Month).
                                Select(x => new { x.FechaMaximaDemanda, x.Indice }).FirstOrDefault();
                            MeMedicion96DTO unidadMaxima = list.Where(x => x.Emprcodi == itemEmpresa.Emprcodi && x.Equicodi == itemEquipo.Equicodi && x.Medifecha ==
                                datosMD.FechaMaximaDemanda).FirstOrDefault();

                            decimal valor = 0;

                            if (unidadMaxima != null)
                            {
                                valor = ((decimal?)unidadMaxima.GetType().GetProperty(ConstantesAppServicio.CaracterH + datosMD.Indice).GetValue(unidadMaxima, null)).GetValueOrDefault(0);
                            }

                            MedicionReporteDTO resultadoEquipo = new MedicionReporteDTO();

                            resultadoEquipo.Emprnomb = itemEmpresa.Emprnomb;
                            resultadoEquipo.Central = itemCentral.Central;
                            resultadoEquipo.Unidad = itemEquipo.Equinomb;
                            resultadoEquipo.Fenergcodi = itemEquipo.Fenergcodi;
                            resultadoEquipo.Fenergnomb = itemEquipo.Fenergnomb;
                            resultadoEquipo.Tgenercodi = itemEquipo.Tgenercodi;
                            resultadoEquipo.Tgenernomb = itemEquipo.Tgenernomb;
                            resultadoEquipo.Anio = itemFecha.Year;
                            resultadoEquipo.Mes = itemFecha.Month;
                            resultadoEquipo.ValorMD = valor;
                            resultadoEquipo.EmprCodi = itemEmpresa.Emprcodi;
                            resultadoEquipo.EquiCodi = itemEquipo.Equicodi;
                            resultadoEquipo.FechaMaximaDemanda = datosMD.FechaMaximaDemanda;
                            resultadoEquipo.Indice = datosMD.Indice;

                            resultado.Add(resultadoEquipo);
                        }
                    }
                }
            }

            return resultado;
        }

        /// <summary>
        /// Permite generar el reporte resumen de medidores
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposGeneracion"></param>
        /// <param name="fuentesEnergia"></param>
        /// <param name="central"></param>
        /// <param name="indAdjudicado"></param>
        public void ObtenerReporteMedidores(DateTime fechaInicial, DateTime fechaFinal, string tiposEmpresa, string empresas,
                   string tiposGeneracion, string fuentesEnergia, int central, out List<MedicionReporteDTO> listCuadrosFE, out List<MedicionReporteDTO> listCuadrosTG, out List<MedicionReporteDTO> listCuadrosUnidades,
                   out MedicionReporteDTO datosReporte, out List<MedicionReporteDTO> reporteFE, out List<MeMedicion96DTO> reporteEmpresas, out List<MedicionReporteDTO> reporteTG,
                    out List<LogErrorHOPvsMedidores> listaValidacion)
        {
            if (tiposEmpresa != ConstantesAppServicio.ParametroDefecto && empresas == ConstantesAppServicio.ParametroDefecto)
            {
                List<int> idsEmpresas = this.ObteneEmpresasPorTipo(tiposEmpresa).Select(x => x.Emprcodi).ToList();
                empresas = string.Join<int>(ConstantesAppServicio.CaracterComa.ToString(), idsEmpresas);
            }

            List<MeMedicion96DTO> list = this.ListaDataMDGeneracionConsolidadoYValidacion(fechaInicial, fechaFinal, central, tiposGeneracion, empresas, ConstanteValidacion.EstadoTodos,
                                                    fuentesEnergia, true, ConstantesMedicion.IdTipoInfoPotenciaActiva, out listaValidacion);

            /*List<MeMedicion96DTO> listDemanda = FactorySic.GetMeMedicion96Repository().ObtenerDatosReporteMD(empresas,
                central, fuentesEnergia, fechaInicial, fechaFinal);*/

            //Data Generación
            List<MeMedicion96DTO> listaDemandaGen = this.ListaDataMDGeneracionFromConsolidado(fechaInicial, fechaFinal, list);

            //Data Interconexion
            List<MeMedicion96DTO> listaInterconexion = this.ListaDataMDInterconexion96(fechaInicial, fechaFinal);

            //Data Total
            List<MeMedicion96DTO> listDemanda = this.ListaDataMDTotalSEIN(listaDemandaGen, listaInterconexion);
            MedicionReporteDTO umbrales = this.ObtenerParametros(fechaInicial, listDemanda, listaInterconexion);

            datosReporte = umbrales;

            #region Armado de cuadros Fuente Energia

            listCuadrosFE = ObtenerListCuadroByParametro(ConstantesMedicion.TipoCuadroFuenteEnergia, list, umbrales);
            listCuadrosTG = ObtenerListCuadroByParametro(ConstantesMedicion.TipoCuadroTipoGeneracion, list, umbrales);
            listCuadrosUnidades = ObtenerListCuadroByParametro(ConstantesMedicion.TipoCuadroUnidades, list, umbrales);

            #endregion

            List<MeMedicion96DTO> listTipoRecurso = list.Where(x => x.Medifecha == umbrales.FechaMaximaDemanda).ToList();
            int indice = umbrales.HoraMaximaDemanda;

            List<MeMedicion96DTO> listFuenteGeneracion = new List<MeMedicion96DTO>();

            #region Fuente Generacion

            listFuenteGeneracion = (from t in listTipoRecurso
                                    group t by new { t.Fenergcodi, t.Fenergnomb }
                                        into destino
                                    select new MeMedicion96DTO()
                                    {
                                        Fenergcodi = destino.Key.Fenergcodi,
                                        Fenergnomb = destino.Key.Fenergnomb,
                                        H1 = destino.Sum(t => t.H1),
                                        H2 = destino.Sum(t => t.H2),
                                        H3 = destino.Sum(t => t.H3),
                                        H4 = destino.Sum(t => t.H4),
                                        H5 = destino.Sum(t => t.H5),
                                        H6 = destino.Sum(t => t.H6),
                                        H7 = destino.Sum(t => t.H7),
                                        H8 = destino.Sum(t => t.H8),
                                        H9 = destino.Sum(t => t.H9),
                                        H10 = destino.Sum(t => t.H10),

                                        H11 = destino.Sum(t => t.H11),
                                        H12 = destino.Sum(t => t.H12),
                                        H13 = destino.Sum(t => t.H13),
                                        H14 = destino.Sum(t => t.H14),
                                        H15 = destino.Sum(t => t.H15),
                                        H16 = destino.Sum(t => t.H16),
                                        H17 = destino.Sum(t => t.H17),
                                        H18 = destino.Sum(t => t.H18),
                                        H19 = destino.Sum(t => t.H19),
                                        H20 = destino.Sum(t => t.H20),

                                        H21 = destino.Sum(t => t.H21),
                                        H22 = destino.Sum(t => t.H22),
                                        H23 = destino.Sum(t => t.H23),
                                        H24 = destino.Sum(t => t.H24),
                                        H25 = destino.Sum(t => t.H25),
                                        H26 = destino.Sum(t => t.H26),
                                        H27 = destino.Sum(t => t.H27),
                                        H28 = destino.Sum(t => t.H28),
                                        H29 = destino.Sum(t => t.H29),
                                        H30 = destino.Sum(t => t.H30),

                                        H31 = destino.Sum(t => t.H31),
                                        H32 = destino.Sum(t => t.H32),
                                        H33 = destino.Sum(t => t.H33),
                                        H34 = destino.Sum(t => t.H34),
                                        H35 = destino.Sum(t => t.H35),
                                        H36 = destino.Sum(t => t.H36),
                                        H37 = destino.Sum(t => t.H37),
                                        H38 = destino.Sum(t => t.H38),
                                        H39 = destino.Sum(t => t.H39),
                                        H40 = destino.Sum(t => t.H40),

                                        H41 = destino.Sum(t => t.H41),
                                        H42 = destino.Sum(t => t.H42),
                                        H43 = destino.Sum(t => t.H43),
                                        H44 = destino.Sum(t => t.H44),
                                        H45 = destino.Sum(t => t.H45),
                                        H46 = destino.Sum(t => t.H46),
                                        H47 = destino.Sum(t => t.H47),
                                        H48 = destino.Sum(t => t.H48),
                                        H49 = destino.Sum(t => t.H49),
                                        H50 = destino.Sum(t => t.H50),


                                        H51 = destino.Sum(t => t.H51),
                                        H52 = destino.Sum(t => t.H52),
                                        H53 = destino.Sum(t => t.H53),
                                        H54 = destino.Sum(t => t.H54),
                                        H55 = destino.Sum(t => t.H55),
                                        H56 = destino.Sum(t => t.H56),
                                        H57 = destino.Sum(t => t.H57),
                                        H58 = destino.Sum(t => t.H58),
                                        H59 = destino.Sum(t => t.H59),
                                        H60 = destino.Sum(t => t.H60),

                                        H61 = destino.Sum(t => t.H61),
                                        H62 = destino.Sum(t => t.H62),
                                        H63 = destino.Sum(t => t.H63),
                                        H64 = destino.Sum(t => t.H64),
                                        H65 = destino.Sum(t => t.H65),
                                        H66 = destino.Sum(t => t.H66),
                                        H67 = destino.Sum(t => t.H67),
                                        H68 = destino.Sum(t => t.H68),
                                        H69 = destino.Sum(t => t.H69),
                                        H70 = destino.Sum(t => t.H70),

                                        H71 = destino.Sum(t => t.H71),
                                        H72 = destino.Sum(t => t.H72),
                                        H73 = destino.Sum(t => t.H73),
                                        H74 = destino.Sum(t => t.H74),
                                        H75 = destino.Sum(t => t.H75),
                                        H76 = destino.Sum(t => t.H76),
                                        H77 = destino.Sum(t => t.H77),
                                        H78 = destino.Sum(t => t.H78),
                                        H79 = destino.Sum(t => t.H79),
                                        H80 = destino.Sum(t => t.H80),

                                        H81 = destino.Sum(t => t.H81),
                                        H82 = destino.Sum(t => t.H82),
                                        H83 = destino.Sum(t => t.H83),
                                        H84 = destino.Sum(t => t.H84),
                                        H85 = destino.Sum(t => t.H85),
                                        H86 = destino.Sum(t => t.H86),
                                        H87 = destino.Sum(t => t.H87),
                                        H88 = destino.Sum(t => t.H88),
                                        H89 = destino.Sum(t => t.H89),
                                        H90 = destino.Sum(t => t.H90),

                                        H91 = destino.Sum(t => t.H91),
                                        H92 = destino.Sum(t => t.H92),
                                        H93 = destino.Sum(t => t.H93),
                                        H94 = destino.Sum(t => t.H94),
                                        H95 = destino.Sum(t => t.H95),
                                        H96 = destino.Sum(t => t.H96)

                                    }).ToList();

            #endregion

            List<MedicionReporteDTO> resultadoFE = new List<MedicionReporteDTO>();

            decimal totalMDFuenteEnergia = 0;
            decimal totalEnergiaFuenteEnergia = 0;

            foreach (MeMedicion96DTO item in listFuenteGeneracion)
            {
                MedicionReporteDTO itemFE = new MedicionReporteDTO();
                itemFE.Fenergnomb = item.Fenergnomb;
                itemFE.Fenergcodi = item.Fenergcodi;

                if (indice >= 1 && indice <= 96)
                {
                    object result = item.GetType().GetProperty(ConstantesAppServicio.CaracterH + indice).GetValue(item, null);
                    itemFE.MDFuenteEnergia = (result != null) ? Convert.ToDecimal(result) : 0;
                    totalMDFuenteEnergia = totalMDFuenteEnergia + itemFE.MDFuenteEnergia;
                }

                object totFuente = list.Where(x => x.Fenergcodi == item.Fenergcodi).Sum(x => x.Meditotal);

                if (totFuente != null)
                {
                    itemFE.EnergiaFuenteEnergia = Convert.ToDecimal(totFuente) / 4.0M;
                    totalEnergiaFuenteEnergia = totalEnergiaFuenteEnergia + itemFE.EnergiaFuenteEnergia;
                }

                resultadoFE.Add(itemFE);
            }

            List<SiFuenteenergiaDTO> listTotFuente = this.ListaFuenteEnergia(0);
            List<SiFuenteenergiaDTO> listTotFuenteFalta = listTotFuente.Where(x => !listFuenteGeneracion.Any(y => y.Fenergcodi == x.Fenergcodi)).ToList();

            foreach (SiFuenteenergiaDTO item in listTotFuenteFalta)
            {
                MedicionReporteDTO itemFE = new MedicionReporteDTO();
                itemFE.Fenergnomb = item.Fenergnomb;
                itemFE.Fenergcodi = item.Fenergcodi;
                itemFE.MDFuenteEnergia = 0;
                object totFuente = list.Where(x => x.Fenergcodi == item.Fenergcodi).Sum(x => x.Meditotal);
                if (totFuente != null)
                {
                    itemFE.EnergiaFuenteEnergia = Convert.ToDecimal(totFuente) / 4.0M;
                    totalEnergiaFuenteEnergia = totalEnergiaFuenteEnergia + itemFE.EnergiaFuenteEnergia;
                }
                resultadoFE.Add(itemFE);
            }

            resultadoFE = resultadoFE.OrderBy(x => x.Fenergnomb).ToList();

            MedicionReporteDTO finalFE = new MedicionReporteDTO();
            finalFE.EnergiaFuenteEnergia = totalEnergiaFuenteEnergia;
            finalFE.MDFuenteEnergia = totalMDFuenteEnergia;
            finalFE.IndicadorTotal = true;
            resultadoFE.Add(finalFE);

            reporteFE = resultadoFE;

            List<MeMedicion96DTO> listEmpresa = new List<MeMedicion96DTO>();
            listEmpresa = (from t in list
                           group t by new { t.Emprcodi, t.Emprnomb, t.Tgenercodi, t.Tgenernomb }
                               into destino
                           select new MeMedicion96DTO()
                           {
                               Emprcodi = destino.Key.Emprcodi,
                               Emprnomb = destino.Key.Emprnomb,
                               Tgenercodi = destino.Key.Tgenercodi,
                               Tgenernomb = destino.Key.Tgenernomb,
                               Meditotal = destino.Sum(t => t.Meditotal) / 4.0M
                           }).ToList();

            reporteEmpresas = listEmpresa;

            List<MeMedicion96DTO> listTipoGeneracion = new List<MeMedicion96DTO>();

            listTipoGeneracion = (from t in listTipoRecurso
                                  group t by new { t.Tgenercodi, t.Tgenernomb }
                                      into destino
                                  select new MeMedicion96DTO()
                                  {
                                      Tgenercodi = destino.Key.Tgenercodi,
                                      Tgenernomb = destino.Key.Tgenernomb,
                                      H1 = destino.Sum(t => t.H1),
                                      H2 = destino.Sum(t => t.H2),
                                      H3 = destino.Sum(t => t.H3),
                                      H4 = destino.Sum(t => t.H4),
                                      H5 = destino.Sum(t => t.H5),
                                      H6 = destino.Sum(t => t.H6),
                                      H7 = destino.Sum(t => t.H7),
                                      H8 = destino.Sum(t => t.H8),
                                      H9 = destino.Sum(t => t.H9),
                                      H10 = destino.Sum(t => t.H10),

                                      H11 = destino.Sum(t => t.H11),
                                      H12 = destino.Sum(t => t.H12),
                                      H13 = destino.Sum(t => t.H13),
                                      H14 = destino.Sum(t => t.H14),
                                      H15 = destino.Sum(t => t.H15),
                                      H16 = destino.Sum(t => t.H16),
                                      H17 = destino.Sum(t => t.H17),
                                      H18 = destino.Sum(t => t.H18),
                                      H19 = destino.Sum(t => t.H19),
                                      H20 = destino.Sum(t => t.H20),

                                      H21 = destino.Sum(t => t.H21),
                                      H22 = destino.Sum(t => t.H22),
                                      H23 = destino.Sum(t => t.H23),
                                      H24 = destino.Sum(t => t.H24),
                                      H25 = destino.Sum(t => t.H25),
                                      H26 = destino.Sum(t => t.H26),
                                      H27 = destino.Sum(t => t.H27),
                                      H28 = destino.Sum(t => t.H28),
                                      H29 = destino.Sum(t => t.H29),
                                      H30 = destino.Sum(t => t.H30),

                                      H31 = destino.Sum(t => t.H31),
                                      H32 = destino.Sum(t => t.H32),
                                      H33 = destino.Sum(t => t.H33),
                                      H34 = destino.Sum(t => t.H34),
                                      H35 = destino.Sum(t => t.H35),
                                      H36 = destino.Sum(t => t.H36),
                                      H37 = destino.Sum(t => t.H37),
                                      H38 = destino.Sum(t => t.H38),
                                      H39 = destino.Sum(t => t.H39),
                                      H40 = destino.Sum(t => t.H40),

                                      H41 = destino.Sum(t => t.H41),
                                      H42 = destino.Sum(t => t.H42),
                                      H43 = destino.Sum(t => t.H43),
                                      H44 = destino.Sum(t => t.H44),
                                      H45 = destino.Sum(t => t.H45),
                                      H46 = destino.Sum(t => t.H46),
                                      H47 = destino.Sum(t => t.H47),
                                      H48 = destino.Sum(t => t.H48),
                                      H49 = destino.Sum(t => t.H49),
                                      H50 = destino.Sum(t => t.H50),


                                      H51 = destino.Sum(t => t.H51),
                                      H52 = destino.Sum(t => t.H52),
                                      H53 = destino.Sum(t => t.H53),
                                      H54 = destino.Sum(t => t.H54),
                                      H55 = destino.Sum(t => t.H55),
                                      H56 = destino.Sum(t => t.H56),
                                      H57 = destino.Sum(t => t.H57),
                                      H58 = destino.Sum(t => t.H58),
                                      H59 = destino.Sum(t => t.H59),
                                      H60 = destino.Sum(t => t.H60),

                                      H61 = destino.Sum(t => t.H61),
                                      H62 = destino.Sum(t => t.H62),
                                      H63 = destino.Sum(t => t.H63),
                                      H64 = destino.Sum(t => t.H64),
                                      H65 = destino.Sum(t => t.H65),
                                      H66 = destino.Sum(t => t.H66),
                                      H67 = destino.Sum(t => t.H67),
                                      H68 = destino.Sum(t => t.H68),
                                      H69 = destino.Sum(t => t.H69),
                                      H70 = destino.Sum(t => t.H70),

                                      H71 = destino.Sum(t => t.H71),
                                      H72 = destino.Sum(t => t.H72),
                                      H73 = destino.Sum(t => t.H73),
                                      H74 = destino.Sum(t => t.H74),
                                      H75 = destino.Sum(t => t.H75),
                                      H76 = destino.Sum(t => t.H76),
                                      H77 = destino.Sum(t => t.H77),
                                      H78 = destino.Sum(t => t.H78),
                                      H79 = destino.Sum(t => t.H79),
                                      H80 = destino.Sum(t => t.H80),

                                      H81 = destino.Sum(t => t.H81),
                                      H82 = destino.Sum(t => t.H82),
                                      H83 = destino.Sum(t => t.H83),
                                      H84 = destino.Sum(t => t.H84),
                                      H85 = destino.Sum(t => t.H85),
                                      H86 = destino.Sum(t => t.H86),
                                      H87 = destino.Sum(t => t.H87),
                                      H88 = destino.Sum(t => t.H88),
                                      H89 = destino.Sum(t => t.H89),
                                      H90 = destino.Sum(t => t.H90),

                                      H91 = destino.Sum(t => t.H91),
                                      H92 = destino.Sum(t => t.H92),
                                      H93 = destino.Sum(t => t.H93),
                                      H94 = destino.Sum(t => t.H94),
                                      H95 = destino.Sum(t => t.H95),
                                      H96 = destino.Sum(t => t.H96)

                                  }).ToList();


            List<MedicionReporteDTO> resultadoTG = new List<MedicionReporteDTO>();

            foreach (MeMedicion96DTO item in listTipoGeneracion)
            {
                MedicionReporteDTO itemTG = new MedicionReporteDTO();
                itemTG.Tgenernomb = item.Tgenernomb;

                if (indice >= 1 && indice <= 96)
                {
                    object result = item.GetType().GetProperty(ConstantesAppServicio.CaracterH + indice).GetValue(item, null);
                    itemTG.MDFuenteEnergia = (result != null) ? Convert.ToDecimal(result) : 0;
                }

                resultadoTG.Add(itemTG);
            }


            reporteTG = resultadoTG;
        }

        /// <summary>
        /// ObtenerListCuadroByParametro
        /// </summary>
        /// <param name="parametro"></param>
        /// <param name="list"></param>
        /// <param name="umbrales"></param>
        /// <returns></returns>
        private List<MedicionReporteDTO> ObtenerListCuadroByParametro(int parametro, List<MeMedicion96DTO> list, MedicionReporteDTO umbrales)
        {
            var listEmpresas = list.Select(x => new { x.Emprcodi, x.Emprnomb }).Distinct().OrderBy(x => x.Emprnomb).ToList();
            var listCentrales = list.Select(x => new { x.Emprcodi, x.Central }).Distinct().ToList();

            var listEquipos = new List<EqEquipoDTO>();
            switch (parametro)
            {
                case ConstantesMedicion.TipoCuadroFuenteEnergia:
                    listEquipos = list.GroupBy(x => new { x.Emprcodi, x.Central, x.Equicodi, x.Equinomb, x.Tgenernomb, x.Tgenercodi, x.Fenergcodi, x.Fenergnomb }).
                        Select(x => new EqEquipoDTO() { Emprcodi = x.Key.Emprcodi, Central = x.Key.Central, Equicodi = x.Key.Equicodi, Equinomb = x.Key.Equinomb, Tgenernomb = x.Key.Tgenernomb, Tgenercodi = x.Key.Tgenercodi, Fenergcodi = x.Key.Fenergcodi, Fenergnomb = x.Key.Fenergnomb }).ToList();
                    break;
                case ConstantesMedicion.TipoCuadroTipoGeneracion:
                    listEquipos = list.GroupBy(x => new { x.Emprcodi, x.Central, x.Equicodi, x.Equinomb, x.Tgenernomb, x.Tgenercodi, })
                    .Select(x => new EqEquipoDTO() { Emprcodi = x.Key.Emprcodi, Central = x.Key.Central, Equicodi = x.Key.Equicodi, Equinomb = x.Key.Equinomb, Tgenernomb = x.Key.Tgenernomb, Tgenercodi = x.Key.Tgenercodi, }).ToList();
                    break;
                case ConstantesMedicion.TipoCuadroUnidades:
                    listEquipos = list.GroupBy(x => new { x.Emprcodi, x.Central, x.Equicodi, x.Equinomb, })
                        .Select(x => new EqEquipoDTO() { Emprcodi = x.Key.Emprcodi, Central = x.Key.Central, Equicodi = x.Key.Equicodi, Equinomb = x.Key.Equinomb, }).ToList();
                    break;
            }

            List<MedicionReporteDTO> resultado = new List<MedicionReporteDTO>();

            int i = 1;
            decimal sumTotal = 0;
            decimal maxTotal = 0;
            decimal minTotal = 0;

            decimal solarTotal = 0;
            decimal eolicaTotal = 0;
            decimal hidraulicaTotal = 0;
            decimal termicoTotal = 0;

            foreach (var itemEmpresa in listEmpresas)
            {
                var subCentral = listCentrales.Where(x => x.Emprcodi == itemEmpresa.Emprcodi).ToList();

                decimal sumEmpresa = 0;
                decimal maxEmpresa = 0;
                decimal minEmpresa = 0;

                decimal solarEmpresa = 0;
                decimal eolicaEmpresa = 0;
                decimal hidraulicaEmpresa = 0;
                decimal termicoEmpresa = 0;

                foreach (var itemCentral in subCentral)
                {
                    var subEquipo = listEquipos.Where(x => x.Central == itemCentral.Central && x.Emprcodi == itemCentral.Emprcodi)
                    .GroupBy(x => new { x.Equicodi, x.Equinomb }).Select(x => new MeMedicion96DTO { Equicodi = x.Key.Equicodi, Equinomb = x.Key.Equinomb }).ToList();

                    foreach (var itemEquipo in subEquipo)
                    {
                        var listaTipoFiltroXEquipo = new List<MeMedicion96DTO>();
                        switch (parametro)
                        {
                            case ConstantesMedicion.TipoCuadroFuenteEnergia:
                                listaTipoFiltroXEquipo = listEquipos.Where(x => x.Central == itemCentral.Central && x.Emprcodi == itemCentral.Emprcodi && x.Equicodi == itemEquipo.Equicodi)
                                .GroupBy(x => new { x.Equicodi, x.Equinomb, x.Tgenercodi, x.Tgenernomb, x.Fenergcodi, x.Fenergnomb }).Select(x => new MeMedicion96DTO() { Equicodi = x.Key.Equicodi, Equinomb = x.Key.Equinomb, Tgenercodi = x.Key.Tgenercodi, Tgenernomb = x.Key.Tgenernomb, Fenergcodi = x.Key.Fenergcodi, Fenergnomb = x.Key.Fenergnomb }).ToList();
                                break;
                            case ConstantesMedicion.TipoCuadroTipoGeneracion:
                                listaTipoFiltroXEquipo = listEquipos.Where(x => x.Central == itemCentral.Central && x.Emprcodi == itemCentral.Emprcodi && x.Equicodi == itemEquipo.Equicodi)
                                .GroupBy(x => new { x.Equicodi, x.Equinomb, x.Tgenercodi, x.Tgenernomb }).Select(x => new MeMedicion96DTO() { Equicodi = x.Key.Equicodi, Equinomb = x.Key.Equinomb, Tgenercodi = x.Key.Tgenercodi, Tgenernomb = x.Key.Tgenernomb }).ToList();
                                break;
                            case ConstantesMedicion.TipoCuadroUnidades:
                                listaTipoFiltroXEquipo = listEquipos.Where(x => x.Central == itemCentral.Central && x.Emprcodi == itemCentral.Emprcodi && x.Equicodi == itemEquipo.Equicodi)
                                .GroupBy(x => new { x.Equicodi, x.Equinomb }).Select(x => new MeMedicion96DTO() { Equicodi = x.Key.Equicodi, Equinomb = x.Key.Equinomb }).ToList();
                                break;
                        }
                        listaTipoFiltroXEquipo = listaTipoFiltroXEquipo.OrderBy(x => x.Fenergcodi).ToList();

                        foreach (var tipoRec in listaTipoFiltroXEquipo)
                        {
                            itemEquipo.Tgenercodi = tipoRec.Tgenercodi;
                            itemEquipo.Tgenernomb = tipoRec.Tgenernomb;
                            MedicionReporteDTO resultadoEquipo = new MedicionReporteDTO();

                            resultadoEquipo.NroItem = i;
                            resultadoEquipo.Emprnomb = itemEmpresa.Emprnomb;
                            resultadoEquipo.Central = itemCentral.Central;
                            resultadoEquipo.Unidad = itemEquipo.Equinomb;
                            resultadoEquipo.Fenergcodi = tipoRec.Fenergcodi;
                            resultadoEquipo.Fenergnomb = tipoRec.Fenergnomb;
                            resultadoEquipo.Tgenercodi = itemEquipo.Tgenercodi;
                            resultadoEquipo.Tgenernomb = itemEquipo.Tgenernomb;
                            resultadoEquipo.IndicadorTotal = false;
                            resultadoEquipo.IndicadorTotalGeneral = false;

                            List<MeMedicion96DTO> listaUnidadMaxima = new List<MeMedicion96DTO>();
                            List<MeMedicion96DTO> listaUnidadMinima = new List<MeMedicion96DTO>();

                            decimal energiaUnidad = 0;
                            decimal maximaUnidad = 0;
                            decimal minimaUnidad = 0;

                            object subTotal = null;

                            switch (parametro)
                            {
                                case ConstantesMedicion.TipoCuadroFuenteEnergia:
                                    subTotal = list.Where(x => x.Equicodi == itemEquipo.Equicodi && x.Emprcodi == itemEmpresa.Emprcodi && x.Fenergcodi == tipoRec.Fenergcodi).Sum(x => x.Meditotal.GetValueOrDefault(0));
                                    listaUnidadMaxima = list.Where(x => x.Equicodi == itemEquipo.Equicodi && x.Emprcodi == itemEmpresa.Emprcodi && x.Fenergcodi == tipoRec.Fenergcodi && x.Medifecha == umbrales.FechaMaximaDemanda).ToList();
                                    listaUnidadMinima = list.Where(x => x.Equicodi == itemEquipo.Equicodi && x.Emprcodi == itemEmpresa.Emprcodi && x.Fenergcodi == tipoRec.Fenergcodi && x.Medifecha == umbrales.FechaMinimaDemanda).ToList();
                                    break;
                                case ConstantesMedicion.TipoCuadroTipoGeneracion:
                                    subTotal = list.Where(x => x.Equicodi == itemEquipo.Equicodi && x.Emprcodi == itemEmpresa.Emprcodi && x.Tgenercodi == tipoRec.Tgenercodi).Sum(x => x.Meditotal.GetValueOrDefault(0));
                                    listaUnidadMaxima = list.Where(x => x.Equicodi == itemEquipo.Equicodi && x.Emprcodi == itemEmpresa.Emprcodi && x.Tgenercodi == tipoRec.Tgenercodi && x.Medifecha == umbrales.FechaMaximaDemanda).ToList();
                                    listaUnidadMinima = list.Where(x => x.Equicodi == itemEquipo.Equicodi && x.Emprcodi == itemEmpresa.Emprcodi && x.Tgenercodi == tipoRec.Tgenercodi && x.Medifecha == umbrales.FechaMinimaDemanda).ToList();
                                    break;
                                case ConstantesMedicion.TipoCuadroUnidades:
                                    subTotal = list.Where(x => x.Equicodi == itemEquipo.Equicodi && x.Emprcodi == itemEmpresa.Emprcodi).Sum(x => x.Meditotal.GetValueOrDefault(0));
                                    listaUnidadMaxima = list.Where(x => x.Equicodi == itemEquipo.Equicodi && x.Emprcodi == itemEmpresa.Emprcodi && x.Medifecha == umbrales.FechaMaximaDemanda).ToList();
                                    listaUnidadMinima = list.Where(x => x.Equicodi == itemEquipo.Equicodi && x.Emprcodi == itemEmpresa.Emprcodi && x.Medifecha == umbrales.FechaMinimaDemanda).ToList();
                                    break;
                            }

                            if (subTotal != null)
                            {
                                energiaUnidad = energiaUnidad + Convert.ToDecimal(subTotal);
                            }

                            foreach (var regMax in listaUnidadMaxima)
                            {
                                var maximaUnidadTmp = ((decimal?)regMax.GetType().GetProperty(ConstantesAppServicio.CaracterH + umbrales.HoraMaximaDemanda).GetValue(regMax, null)).GetValueOrDefault(0);
                                if (maximaUnidadTmp > maximaUnidad)
                                    maximaUnidad = maximaUnidadTmp;
                            }

                            foreach (var regMin in listaUnidadMinima)
                            {
                                var minimaUnidadTmp = ((decimal?)regMin.GetType().GetProperty(ConstantesAppServicio.CaracterH + umbrales.HoraMinimaDemanda).GetValue(regMin, null)).GetValueOrDefault(0);
                                if (minimaUnidadTmp > minimaUnidad)
                                    minimaUnidad = minimaUnidadTmp;
                            }

                            resultadoEquipo.Total = (energiaUnidad != 0) ? energiaUnidad / 4 : 0;
                            resultadoEquipo.MaximaDemanda = maximaUnidad;
                            resultadoEquipo.MinimaDemanda = minimaUnidad;

                            sumEmpresa = sumEmpresa + resultadoEquipo.Total;
                            maxEmpresa = maxEmpresa + resultadoEquipo.MaximaDemanda;
                            minEmpresa = minEmpresa + resultadoEquipo.MinimaDemanda;

                            if (itemEquipo.Tgenercodi == 1)
                            {
                                resultadoEquipo.Hidraulico = resultadoEquipo.Total;
                                hidraulicaEmpresa = hidraulicaEmpresa + resultadoEquipo.Total;
                            }
                            else if (itemEquipo.Tgenercodi == 2)
                            {
                                resultadoEquipo.Termico = resultadoEquipo.Total;
                                termicoEmpresa = termicoEmpresa + resultadoEquipo.Total;
                            }
                            else if (itemEquipo.Tgenercodi == 3)
                            {
                                resultadoEquipo.Solar = resultadoEquipo.Total;
                                solarEmpresa = solarEmpresa + resultadoEquipo.Total;
                            }
                            else if (itemEquipo.Tgenercodi == 4)
                            {
                                resultadoEquipo.Eolico = resultadoEquipo.Total;
                                eolicaEmpresa = eolicaEmpresa + resultadoEquipo.Total;
                            }

                            resultado.Add(resultadoEquipo);
                            i++;
                        }
                    }
                }

                sumTotal = sumTotal + sumEmpresa;
                maxTotal = maxTotal + maxEmpresa;
                minTotal = minTotal + minEmpresa;

                solarTotal = solarTotal + solarEmpresa;
                termicoTotal = termicoTotal + termicoEmpresa;
                hidraulicaTotal = hidraulicaTotal + hidraulicaEmpresa;
                eolicaTotal = eolicaTotal + eolicaEmpresa;


                MedicionReporteDTO resultadoEmpresa = new MedicionReporteDTO();
                resultadoEmpresa.IndicadorTotal = true;
                resultadoEmpresa.Emprnomb = itemEmpresa.Emprnomb;
                resultadoEmpresa.Total = sumEmpresa;
                resultadoEmpresa.MaximaDemanda = maxEmpresa;
                resultadoEmpresa.MinimaDemanda = minEmpresa;
                resultadoEmpresa.Solar = solarEmpresa;
                resultadoEmpresa.Eolico = eolicaEmpresa;
                resultadoEmpresa.Termico = termicoEmpresa;
                resultadoEmpresa.Hidraulico = hidraulicaEmpresa;
                resultado.Add(resultadoEmpresa);
            }

            resultado = resultado.OrderBy(x => x.Emprnomb).ThenBy(x => x.IndicadorTotal).ThenBy(x => x.Central).ThenBy(x => x.Unidad).ThenBy(x => x.Fenergnomb).ToList();

            MedicionReporteDTO resultadoTotal = new MedicionReporteDTO();
            resultadoTotal.IndicadorTotalGeneral = true;
            resultadoTotal.Total = sumTotal;
            resultadoTotal.MaximaDemanda = maxTotal;
            resultadoTotal.MinimaDemanda = minTotal;
            resultadoTotal.Solar = solarTotal;
            resultadoTotal.Hidraulico = hidraulicaTotal;
            resultadoTotal.Termico = termicoTotal;
            resultadoTotal.Eolico = eolicaTotal;
            resultado.Add(resultadoTotal);

            return resultado;
        }

        /// <summary>
        /// Permite obtener la máxima y nímima demanda
        /// </summary>
        /// <param name="list"></param>
        public MedicionReporteDTO ObtenerParametros(DateTime fechaPeriodo, List<MeMedicion96DTO> list, List<MeMedicion96DTO> listaInterconexion)
        {
            MedicionReporteDTO entity = new MedicionReporteDTO();

            decimal max = decimal.MinValue;
            decimal min = decimal.MaxValue;
            DateTime fechaMax = fechaPeriodo;
            int horaMax = 1;
            DateTime fechaMin = fechaPeriodo;
            int horaMin = 1;

            int index = 0;
            int count = 0;

            //Parametros para calculo de MD
            List<SiParametroValorDTO> listaRangoNormaHP = servParametro.ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroRangoPeriodoHP);
            List<SiParametroValorDTO> listaBloqueHorario = servParametro.ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroHPPotenciaActiva);

            foreach (MeMedicion96DTO item in list)
            {
                MedidoresHelper.ObtenerValorHXPeriodoDemandaM96(ConstantesRepMaxDemanda.TipoMDNormativa, item.Medifecha.Value, new List<MeMedicion96DTO>() { item }, listaRangoNormaHP, listaBloqueHorario,
                                                                out decimal valorMDDia, out int hMD, out DateTime fechaHoraMD, out _);

                MedidoresHelper.ObtenerValorHXPeriodoDemandaM96(ConstantesRepMaxDemanda.TipoMinimaTodoDia, item.Medifecha.Value, new List<MeMedicion96DTO>() { item }, null, null,
                                                                out decimal valorMinDia, out int hMin, out DateTime fechaHoraMin, out _);

                if (valorMDDia > max)
                {
                    max = valorMDDia;
                    fechaMax = item.Medifecha.Value;
                    horaMax = hMD;
                }

                if (valorMinDia < min)
                {
                    min = valorMinDia;
                    fechaMin = item.Medifecha.Value;
                    horaMin = hMin;
                }

                count++;
            }

            entity.MaximaDemanda = max;
            entity.FechaMaximaDemanda = fechaMax;
            entity.HoraMaximaDemanda = horaMax;
            entity.MinimaDemanda = min;
            entity.FechaMinimaDemanda = fechaMin;
            entity.HoraMinimaDemanda = horaMin;

            entity.MaximaDemandaHora = fechaMax.AddMinutes(horaMax * 15);
            entity.MinimaDemandaHora = fechaMin.AddMinutes(horaMin * 15);

            if (index < list.Count)
            {
                MeMedicion96DTO maxima = list[index];
                DateTime fechaMaximoValorDia = maxima.Medifecha.Value;

                MedidoresHelper.ObtenerValorHXPeriodoDemandaM96(ConstantesRepMaxDemanda.TipoPeriodoBloqueMaxima, fechaMaximoValorDia, new List<MeMedicion96DTO>() { maxima }, null, listaBloqueHorario,
                                                                out decimal valorBMax, out int hBMax, out DateTime fechaHoraBMax, out _);
                MedidoresHelper.ObtenerValorHXPeriodoDemandaM96(ConstantesRepMaxDemanda.TipoPeriodoBloqueMedia, fechaMaximoValorDia, new List<MeMedicion96DTO>() { maxima }, null, listaBloqueHorario,
                                                                out decimal valorBMedia, out int hBMedia, out DateTime fechaHoraBMedia, out _);
                MedidoresHelper.ObtenerValorHXPeriodoDemandaM96(ConstantesRepMaxDemanda.TipoPeriodoBloqueMinima, fechaMaximoValorDia, new List<MeMedicion96DTO>() { maxima }, null, listaBloqueHorario,
                                                                out decimal valorBMin, out int hBMin, out DateTime fechaHoravalorBMin, out _);

                MeMedicion96DTO dmdInterconexionDia = listaInterconexion.Where(x => x.Medifecha.Value.Date == fechaMax.Date).FirstOrDefault() ?? new MeMedicion96DTO();

                entity.BloqueMaximaDemanda = valorBMax;
                entity.BloqueMediaDemanda = valorBMedia;
                entity.BloqueMinimaDemanda = valorBMin;
                entity.BloqueMaximaHora = hBMax;
                entity.BloqueMediaHora = hBMedia;
                entity.BloqueMinimaHora = hBMin;
                entity.BloqueMaximaInterconexion = (dmdInterconexionDia != null) ? ((decimal?)dmdInterconexionDia.GetType().GetProperty("H" + hBMax.ToString()).GetValue(dmdInterconexionDia, null)).GetValueOrDefault(0) : 0M;
                entity.BloqueMediaInterconexion = (dmdInterconexionDia != null) ? ((decimal?)dmdInterconexionDia.GetType().GetProperty("H" + hBMedia.ToString()).GetValue(dmdInterconexionDia, null)).GetValueOrDefault(0) : 0M;
                entity.BloqueMinimaInterconexion = (dmdInterconexionDia != null) ? ((decimal?)dmdInterconexionDia.GetType().GetProperty("H" + hBMin.ToString()).GetValue(dmdInterconexionDia, null)).GetValueOrDefault(0) : 0M;

                entity.HoraBloqueMaxima = fechaMax.AddMinutes(15 * hBMax);
                entity.HoraBloqueMedia = fechaMax.AddMinutes(15 * hBMedia);
                entity.HoraBloqueMinima = fechaMax.AddMinutes(15 * hBMin);
            }

            return entity;
        }

        /// <summary>
        /// Permite generar el reporte en excel de los medidores de generación
        /// </summary>
        /// <param name="listCuadros"></param>
        /// <param name="umbrales"></param>
        /// <param name="listFuenteEnergia"></param>
        /// <param name="path"></param>
        /// <param name="file"></param>
        public void GenerarArchivoExcel(List<MedicionReporteDTO> listCuadrosFE, List<MedicionReporteDTO> listCuadrosTG, MedicionReporteDTO umbrales,
            List<MedicionReporteDTO> listFuenteEnergia, DateTime fechaConsulta, DateTime fechaInicio, DateTime fechaFin,
            string path, string file)
        {
            try
            {
                file = path + file;
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }

                using (xlPackage = new ExcelPackage(newFile))
                {
                    this.CrearHojaResumen(umbrales, listFuenteEnergia, fechaConsulta, fechaInicio, fechaFin);

                    this.CrearHojaRecursoEnergetico(listCuadrosFE);

                    this.CrearHoraTipoGeneracion(listCuadrosTG, umbrales);

                    xlPackage.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Crea la hoja con los datos a exportar
        /// </summary>
        /// <param name="hojaName"></param>
        /// <param name="list"></param>
        protected void CrearHojaResumen(MedicionReporteDTO umbrales, List<MedicionReporteDTO> listFuenteEnergia, DateTime fechaConsulta,
             DateTime fechaDesde, DateTime fechaHasta)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("RESUMEN");

            if (ws != null)
            {
                ws.Cells[5, 2].Value = "REPORTE DE PRODUCCIÓN DE ENERGÍA ELÉCTRICA Y POTENCIA EJECUTADA";

                ExcelRange rg = ws.Cells[5, 2, 5, 2];
                rg.Style.Font.Size = 13;
                rg.Style.Font.Bold = true;

                ws.Cells[7, 2].Value = "FECHA CONSULTA " + fechaConsulta.ToString("dd/MM/yyyy");
                rg = ws.Cells[7, 2, 7, 4];
                rg.Merge = true;
                rg.Style.Font.Size = 10;
                rg.Style.Font.Bold = true;
                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#EAF7D9"));
                rg.Style.Border.Left.Style = ExcelBorderStyle.Double;
                rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#BBDF8D"));
                rg.Style.Border.Right.Style = ExcelBorderStyle.Double;
                rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#BBDF8D"));
                rg.Style.Border.Top.Style = ExcelBorderStyle.Double;
                rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#BBDF8D"));
                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#BBDF8D"));

                ws.Cells[9, 2].Value = "PERIODO DE CONSULTA DEL " + fechaDesde.ToString("dd/MM/yyyy") + " AL " + fechaHasta.ToString("dd/MM/yyyy");
                rg = ws.Cells[9, 2, 9, 4];
                rg.Merge = true;
                rg.Style.Font.Size = 10;
                rg.Style.Font.Bold = true;
                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#EAF7D9"));
                rg.Style.Border.Left.Style = ExcelBorderStyle.Double;
                rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#BBDF8D"));
                rg.Style.Border.Right.Style = ExcelBorderStyle.Double;
                rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#BBDF8D"));
                rg.Style.Border.Top.Style = ExcelBorderStyle.Double;
                rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#BBDF8D"));
                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#BBDF8D"));


                ws.Cells[11, 2].Value = "Fecha del día de la máxima demanda: " + umbrales.FechaMaximaDemanda.ToString("dd/MM/yyyy");

                rg = ws.Cells[11, 2, 11, 6];
                rg.Merge = true;
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                rg.Style.Font.Color.SetColor(Color.White);
                rg.Style.Font.Size = 10;
                rg.Style.Font.Bold = true;

                ws.Cells[12, 2].Value = "Bloque horario";
                ws.Cells[12, 3].Value = "Hora";
                ws.Cells[12, 4].Value = "Exportación";
                ws.Cells[12, 5].Value = "Importación";
                ws.Cells[12, 6].Value = "MW";

                rg = ws.Cells[12, 2, 12, 4];
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#E8F6FF"));
                rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#245C86"));
                rg.Style.Font.Size = 10;
                rg.Style.Font.Bold = true;


                ws.Cells[13, 2].Value = "Máxima";
                ws.Cells[13, 3].Value = umbrales.HoraBloqueMaxima.ToString("HH:mm");
                ws.Cells[13, 4].Value = umbrales.BloqueMaximaInterconexion > 0 ? umbrales.BloqueMaximaInterconexion : 0;
                ws.Cells[13, 5].Value = umbrales.BloqueMaximaInterconexion > 0 ? 0 : -1 * umbrales.BloqueMaximaInterconexion;
                ws.Cells[13, 6].Value = umbrales.BloqueMaximaDemanda;

                ws.Cells[14, 2].Value = "Media";
                ws.Cells[14, 3].Value = umbrales.HoraBloqueMedia.ToString("HH:mm");
                ws.Cells[14, 4].Value = umbrales.BloqueMediaInterconexion > 0 ? umbrales.BloqueMediaInterconexion : 0;
                ws.Cells[14, 5].Value = umbrales.BloqueMediaInterconexion > 0 ? 0 : -1 * umbrales.BloqueMediaInterconexion;
                ws.Cells[14, 6].Value = umbrales.BloqueMediaDemanda;

                ws.Cells[15, 2].Value = "Mínima";
                ws.Cells[15, 3].Value = umbrales.HoraBloqueMinima.ToString("HH:mm");
                ws.Cells[15, 4].Value = umbrales.BloqueMinimaInterconexion > 0 ? umbrales.BloqueMinimaInterconexion : 0;
                ws.Cells[15, 5].Value = umbrales.BloqueMinimaInterconexion > 0 ? 0 : -1 * umbrales.BloqueMinimaInterconexion;
                ws.Cells[15, 6].Value = umbrales.BloqueMinimaDemanda;


                rg = ws.Cells[13, 4, 15, 6];
                rg.Style.Numberformat.Format = "#,##0.000";

                rg = ws.Cells[13, 2, 15, 6];
                rg.Style.Font.Size = 10;
                rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));


                rg = ws.Cells[11, 2, 15, 6];
                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                ws.Cells[17, 2].Value = "Tipo de recurso energético";
                rg = ws.Cells[17, 2, 19, 2];
                rg.Merge = true;
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                rg.Style.Font.Color.SetColor(Color.White);
                rg.Style.Font.Size = 10;
                rg.Style.Font.Bold = true;

                ws.Cells[17, 3].Value = "Energía (MWh)";
                rg = ws.Cells[17, 3, 19, 3];
                rg.Merge = true;
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                rg.Style.Font.Color.SetColor(Color.White);
                rg.Style.Font.Size = 10;
                rg.Style.Font.Bold = true;


                ws.Cells[17, 4].Value = "Máxima demanda (MW)";
                rg = ws.Cells[17, 4, 17, 4];
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                rg.Style.Font.Color.SetColor(Color.White);
                rg.Style.Font.Size = 10;
                rg.Style.Font.Bold = true;


                ws.Cells[18, 4].Value = "Dia: " + umbrales.FechaMaximaDemanda.ToString("dd/MM/yyyy");
                rg = ws.Cells[18, 4, 18, 4];
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#E8F6FF"));
                rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#245C86"));
                rg.Style.Font.Size = 10;
                rg.Style.Font.Bold = true;


                ws.Cells[19, 4].Value = umbrales.MaximaDemandaHora.ToString("HH:mm");
                rg = ws.Cells[19, 4, 19, 4];
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                rg.Style.Font.Color.SetColor(Color.White);
                rg.Style.Font.Size = 10;
                rg.Style.Font.Bold = true;


                int index = 20;
                foreach (MedicionReporteDTO item in listFuenteEnergia)
                {
                    if (!item.IndicadorTotal)
                    {
                        ws.Cells[index, 2].Value = item.Fenergnomb;
                        ws.Cells[index, 3].Value = item.EnergiaFuenteEnergia;
                        ws.Cells[index, 4].Value = item.MDFuenteEnergia;

                        rg = ws.Cells[index, 2, index, 4];
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));
                    }
                    else
                    {
                        ws.Cells[index, 2].Value = "TOTAL";
                        ws.Cells[index, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[index, 3].Value = item.EnergiaFuenteEnergia;
                        ws.Cells[index, 4].Value = item.MDFuenteEnergia;

                        rg = ws.Cells[index, 2, index, 4];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2E8DCD"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                    }

                    index++;
                }


                rg = ws.Cells[20, 3, index, 4];
                rg.Style.Numberformat.Format = "#,##0.000";


                rg = ws.Cells[17, 2, index - 1, 4];
                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));


                ws.Column(2).Width = 30;

                rg = ws.Cells[7, 3, index, 6];
                rg.AutoFitColumns();


                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                picture.From.Column = 1;
                picture.From.Row = 1;
                picture.To.Column = 2;
                picture.To.Row = 2;
                picture.SetSize(120, 60);

            }
        }

        /// <summary>
        /// Permite generar la hoja de consulta de reporte por recurso energético
        /// </summary>
        /// <param name="list"></param>
        protected void CrearHojaRecursoEnergetico(List<MedicionReporteDTO> list)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("RECURSO ENEGÉTICO");

            if (ws != null)
            {
                ws.Cells[5, 2].Value = "REPORTE DE PRODUCCIÓN DE ENERGÍA ELÉCTRICA Y POTENCIA EJECUTADA POR TIPO DE RECURSO ENERGÉTICO";

                ExcelRange rg = ws.Cells[5, 2, 5, 2];
                rg.Style.Font.Size = 13;
                rg.Style.Font.Bold = true;

                int row = 8;

                ws.Cells[row, 2].Value = "N°";
                ws.Cells[row, 3].Value = "Empresa";
                ws.Cells[row, 4].Value = "Central";
                ws.Cells[row, 5].Value = "Unidad";
                ws.Cells[row, 6].Value = "Tipo de Recurso Energético";
                ws.Cells[row, 7].Value = "Energía (MWh)";
                ws.Cells[row, 8].Value = "Máxima Demanda(MW)";
                ws.Cells[row, 9].Value = "Mínima Demanda(MW)";

                rg = ws.Cells[row, 2, row, 9];
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                rg.Style.Font.Color.SetColor(Color.White);
                rg.Style.Font.Size = 10;
                rg.Style.Font.Bold = true;

                row++;

                foreach (MedicionReporteDTO item in list)
                {
                    if (!item.IndicadorTotal && !item.IndicadorTotalGeneral)
                    {
                        ws.Cells[row, 2].Value = item.NroItem;
                        ws.Cells[row, 3].Value = item.Emprnomb;
                        ws.Cells[row, 4].Value = item.Central;
                        ws.Cells[row, 5].Value = item.Unidad;
                        ws.Cells[row, 6].Value = item.Fenergnomb;
                        ws.Cells[row, 7].Value = item.Total;
                        ws.Cells[row, 8].Value = item.MaximaDemanda;
                        ws.Cells[row, 9].Value = item.MinimaDemanda;

                        rg = ws.Cells[row, 2, row, 9];
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));
                    }
                    else
                    {
                        if (item.IndicadorTotal)
                        {
                            ws.Cells[row, 2].Value = "TOTAL: " + item.Emprnomb.Trim();
                            ws.Cells[row, 2, row, 6].Merge = true;
                            ws.Cells[row, 7].Value = item.Total;
                            ws.Cells[row, 8].Value = item.MaximaDemanda;
                            ws.Cells[row, 9].Value = item.MinimaDemanda;

                            rg = ws.Cells[row, 2, row, 9];
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#E8F6FF"));
                            rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#245C86"));
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Bold = true;

                            ws.Cells[row, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        }
                        if (item.IndicadorTotalGeneral)
                        {
                            ws.Cells[row, 2].Value = "TOTAL GENERAL";
                            ws.Cells[row, 2, row, 6].Merge = true;
                            ws.Cells[row, 7].Value = item.Total;
                            ws.Cells[row, 8].Value = item.MaximaDemanda;
                            ws.Cells[row, 9].Value = item.MinimaDemanda;

                            rg = ws.Cells[row, 2, row, 9];
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2E8DCD"));
                            rg.Style.Font.Color.SetColor(Color.White);
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Bold = true;

                            ws.Cells[row, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        }
                    }

                    row++;
                }


                rg = ws.Cells[9, 7, row, 9];
                rg.Style.Numberformat.Format = "#,##0.000";


                rg = ws.Cells[8, 2, row, 9];
                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                ws.Column(2).Width = 10;

                rg = ws.Cells[8, 3, row, 9];
                rg.AutoFitColumns();

                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                picture.From.Column = 1;
                picture.From.Row = 1;
                picture.To.Column = 2;
                picture.To.Row = 2;
                picture.SetSize(120, 60);

            }

        }

        /// <summary>
        /// Permite generar la hora del reporte por tipo de generación
        /// </summary>
        /// <param name="list"></param>
        /// <param name="umbral"></param>
        protected void CrearHoraTipoGeneracion(List<MedicionReporteDTO> list, MedicionReporteDTO umbral)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("TIPO DE GENERACIÓN");

            if (ws != null)
            {
                ws.Cells[5, 2].Value = "REPORTE DE PRODUCCIÓN DE ENERGÍA ELÉCTRICA Y POTENCIA EJECUTADA POR TIPO DE GENERACIÓN";

                ExcelRange rg = ws.Cells[5, 2, 5, 2];
                rg.Style.Font.Size = 13;
                rg.Style.Font.Bold = true;

                ws.Cells[6, 10].Value = "Máxima demanda: el " + umbral.FechaMaximaDemanda.ToString("dd/MM/yyyy") + "a las " + umbral.MaximaDemandaHora.ToString("HH:mm");
                ws.Cells[6, 10, 6, 12].Merge = true;
                ws.Cells[7, 10].Value = "Mínima demanda: el " + umbral.FechaMinimaDemanda.ToString("dd/MM/yyyy") + "a las " + umbral.MinimaDemandaHora.ToString("HH:mm");
                ws.Cells[7, 10, 7, 12].Merge = true;

                rg = ws.Cells[6, 10, 7, 12];
                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#EAF7D9"));
                rg.Style.Border.Left.Style = ExcelBorderStyle.Double;
                rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#BBDF8D"));
                rg.Style.Border.Right.Style = ExcelBorderStyle.Double;
                rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#BBDF8D"));
                rg.Style.Border.Top.Style = ExcelBorderStyle.Double;
                rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#BBDF8D"));
                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#BBDF8D"));

                int row = 9;

                ws.Cells[row, 2].Value = "N°";
                ws.Cells[row, 3].Value = "Empresa";
                ws.Cells[row, 4].Value = "Central";
                ws.Cells[row, 5].Value = "Unidad";
                ws.Cells[row, 6].Value = "Energía Hidroeléctrica (MWh)";
                ws.Cells[row, 7].Value = "Energía Termoeléctrica (MWh)";
                ws.Cells[row, 8].Value = "Energía Solar (MWh)";
                ws.Cells[row, 9].Value = "Energía Eólica (MWh)";
                ws.Cells[row, 10].Value = "Total Empresa (MWh)";
                ws.Cells[row, 11].Value = "Máxima Demanda (MW)";
                ws.Cells[row, 12].Value = "Mínima Demanda (MW)";

                rg = ws.Cells[row, 2, row, 12];
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                rg.Style.Font.Color.SetColor(Color.White);
                rg.Style.Font.Size = 10;
                rg.Style.Font.Bold = true;

                row++;

                foreach (MedicionReporteDTO item in list)
                {
                    if (!item.IndicadorTotal && !item.IndicadorTotalGeneral)
                    {
                        ws.Cells[row, 2].Value = item.NroItem;
                        ws.Cells[row, 3].Value = item.Emprnomb;
                        ws.Cells[row, 4].Value = item.Central;
                        ws.Cells[row, 5].Value = item.Unidad;
                        ws.Cells[row, 6].Value = item.Hidraulico;
                        ws.Cells[row, 7].Value = item.Termico;
                        ws.Cells[row, 8].Value = item.Solar;
                        ws.Cells[row, 9].Value = item.Eolico;
                        ws.Cells[row, 10].Value = item.Total;
                        ws.Cells[row, 11].Value = item.MaximaDemanda;
                        ws.Cells[row, 12].Value = item.MinimaDemanda;
                        rg = ws.Cells[row, 2, row, 12];
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));
                    }
                    else
                    {
                        if (item.IndicadorTotal)
                        {
                            ws.Cells[row, 2].Value = "TOTAL " + item.Emprnomb.Trim();
                            ws.Cells[row, 2, row, 5].Merge = true;

                            ws.Cells[row, 6].Value = item.Hidraulico;
                            ws.Cells[row, 7].Value = item.Termico;
                            ws.Cells[row, 8].Value = item.Solar;
                            ws.Cells[row, 9].Value = item.Eolico;
                            ws.Cells[row, 10].Value = item.Total;
                            ws.Cells[row, 11].Value = item.MaximaDemanda;
                            ws.Cells[row, 12].Value = item.MinimaDemanda;

                            rg = ws.Cells[row, 2, row, 12];
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#E8F6FF"));
                            rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#245C86"));
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Bold = true;

                            ws.Cells[row, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        }
                        if (item.IndicadorTotalGeneral)
                        {
                            ws.Cells[row, 2].Value = "TOTAL GENERAL";
                            ws.Cells[row, 2, row, 5].Merge = true;

                            ws.Cells[row, 6].Value = item.Hidraulico;
                            ws.Cells[row, 7].Value = item.Termico;
                            ws.Cells[row, 8].Value = item.Solar;
                            ws.Cells[row, 9].Value = item.Eolico;
                            ws.Cells[row, 10].Value = item.Total;
                            ws.Cells[row, 11].Value = item.MaximaDemanda;
                            ws.Cells[row, 12].Value = item.MinimaDemanda;

                            rg = ws.Cells[row, 2, row, 12];
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2E8DCD"));
                            rg.Style.Font.Color.SetColor(Color.White);
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Bold = true;

                            ws.Cells[row, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        }
                    }

                    row++;
                }

                rg = ws.Cells[10, 7, row, 12];
                rg.Style.Numberformat.Format = "#,##0.000";


                rg = ws.Cells[9, 2, row, 12];
                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));


                ws.Column(2).Width = 10;

                rg = ws.Cells[9, 3, row, 12];
                rg.AutoFitColumns();

                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                picture.From.Column = 1;
                picture.From.Row = 1;
                picture.To.Column = 2;
                picture.To.Row = 2;
                picture.SetSize(120, 60);
            }
        }

        #endregion

        #region Cálculo de la Máxima Demanda

        /// <summary>
        /// Data total del SEIN
        /// </summary>
        /// <param name="listaMDDataGeneracion"></param>
        /// <param name="listaMDDataInterconexion96"></param>
        /// <returns></returns>
        public List<MeMedicion96DTO> ListaDataMDTotalSEIN(List<MeMedicion96DTO> listaMDDataGeneracion, List<MeMedicion96DTO> listaMDDataInterconexion96)
        {
            List<MeMedicion96DTO> listaMedicion = new List<MeMedicion96DTO>();
            foreach (var reg in listaMDDataGeneracion)
            {
                MeMedicion96DTO regTot = new MeMedicion96DTO();
                regTot.Medifecha = reg.Medifecha;
                regTot.Tipoinfocodi = reg.Tipoinfocodi;
                regTot.Lectcodi = reg.Lectcodi;
                regTot.Tipoptomedicodi = reg.Tipoptomedicodi;
                regTot.Emprcodi = reg.Emprcodi;
                regTot.Emprnomb = reg.Emprnomb;
                regTot.Tgenercodi = reg.Tgenercodi;

                var exp = listaMDDataInterconexion96.ToList().FirstOrDefault(x => x.Medifecha == reg.Medifecha);

                if (exp != null)
                    for (int i = 1; i <= 96; i++)
                    {
                        decimal valorExp = ((decimal?)exp.GetType().GetProperty("H" + i.ToString()).GetValue(exp, null)).GetValueOrDefault(0);
                        decimal newval = ((decimal?)reg.GetType().GetProperty("H" + i.ToString()).GetValue(reg, null)).GetValueOrDefault(0) - valorExp;
                        regTot.GetType().GetProperty("H" + i.ToString()).SetValue(regTot, newval);
                    }

                listaMedicion.Add(regTot);
            }

            return listaMedicion;
        }

        /// <summary>
        /// Data de las unidades de generación
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="tipoCentral"></param>
        /// <param name="tipoGeneracion"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="estadoValidacion"></param>
        /// <param name="fechaPeriodo"></param>
        /// <returns></returns>
        public List<MeMedicion96DTO> ListaDataMDGeneracion(DateTime fechaIni, DateTime fechaFin, int tipoCentral, string tipoGeneracion, string idEmpresa, int estadoValidacion)
        {
            tipoGeneracion = tipoGeneracion == ConstantesMedicion.IdTipoGeneracionTodos.ToString() ? ConstantesMedicion.ListaGeneracionTodos : tipoGeneracion;
            string idEmpresaValida = this.ListaEmpresaValidadas(fechaFin, idEmpresa, estadoValidacion);

            int lectcodi = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["IdLecturaMedidorGeneracion"]);
            List<MeMedicion96DTO> listaDemanda = new List<MeMedicion96DTO>();
            if (idEmpresaValida != null && idEmpresaValida.Length > 0)
            {
                listaDemanda = FactorySic.GetMeMedicion96Repository().GetResumenMaximaDemanda(fechaIni, fechaFin, tipoCentral, tipoGeneracion.ToString(), idEmpresaValida.ToString()
                    , ConstantesMedicion.IdFamiliaSSAA, ConstantesMedicion.GrupoNoIntegrante, lectcodi, ConstantesMedicion.IdTipoInfoPotenciaActiva, ConstantesMedicion.TipoGenerrer);
            }

            return listaDemanda;
        }

        /// <summary>
        /// Data de Interconexiones Internacionales, medidor principal y secundario en MW
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<MeMedicion96DTO> ListaDataMDInterconexion96(DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion96DTO> lista = new List<MeMedicion96DTO>();
            List<MeMedicion96DTO> listaInterconexion = this.serInterconexion.ObtenerDataHistoricaInterconexionByMedidor(1, ConstantesInterconexiones.IdMedidorConsolidado, fechaInicio, fechaFin, true);

            for (DateTime day = fechaInicio.Date; day <= fechaFin.Date; day = day.AddDays(1))
            {
                var listaDay = listaInterconexion.Where(x => x.Medifecha.Value.Date == day).ToList();
                var listaTptomedicodi = listaDay.GroupBy(x => x.Tipoptomedicodi).Select(x => x.Key).ToList();

                var regDefault = listaDay.Where(x => x.Tipoptomedicodi == -1).FirstOrDefault();
                var regExpMwh = listaDay.Where(x => x.Tipoptomedicodi == ConstantesInterconexiones.IdTipoPtomedicodiExportacionMwh).FirstOrDefault();
                var regImpMwh = listaDay.Where(x => x.Tipoptomedicodi == ConstantesInterconexiones.IdTipoPtomedicodiImportacionMwh).FirstOrDefault();

                if (regDefault != null)
                {
                    lista.Add(regDefault);
                }
                else
                {
                    var reg = new MeMedicion96DTO();
                    reg.Tipoinfocodi = ConstantesFormatoMedicion.IdMWh;
                    reg.Lectcodi = ConstantesFormatoMedicion.IdlecturaInterconexion;
                    reg.Medifecha = day;

                    decimal total = 0M;
                    if (regExpMwh != null && regImpMwh != null)
                    {
                        for (int i = 1; i <= 96; i++)
                        {
                            var valorEpxMWh = (decimal?)regExpMwh.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(regExpMwh, null);
                            var valorImpMWh = (decimal?)regImpMwh.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(regImpMwh, null);
                            decimal valor = (decimal)((valorEpxMWh ?? 0) - (valorImpMWh ?? 0));
                            reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i).ToString()).SetValue(reg, valor);
                            total += valor;
                        }
                    }

                    reg.Meditotal = total;

                    lista.Add(reg);
                }
            }

            //Mwh a Mw 15m
            foreach (var reg in lista)
            {
                for (int i = 1; i <= 96; i++)
                {
                    decimal valorExp = ((decimal?)reg.GetType().GetProperty("H" + i.ToString()).GetValue(reg, null)).GetValueOrDefault(0);
                    reg.GetType().GetProperty("H" + i.ToString()).SetValue(reg, valorExp * 4);
                }
            }

            return lista;
        }

        public void ListaFlujo15minInterconexion96(DateTime fechaInicio, DateTime fechaFin, out List<MeMedicion96DTO> listaTotal,
                        out List<MeMedicion96DTO> listaTotalExp, out List<MeMedicion96DTO> listaTotalImp)
        {
            listaTotal = new List<MeMedicion96DTO>();
            listaTotalExp = new List<MeMedicion96DTO>();
            listaTotalImp = new List<MeMedicion96DTO>();

            List<MeMedicion96DTO> listaInterconexion = this.serInterconexion.ObtenerDataHistoricaInterconexionByMedidor(1, ConstantesInterconexiones.IdMedidorConsolidado, fechaInicio, fechaFin, true);

            for (DateTime day = fechaInicio.Date; day <= fechaFin.Date; day = day.AddDays(1))
            {
                var reg = new MeMedicion96DTO();
                reg.Tipoinfocodi = ConstantesTipoInformacion.TipoinfoMW;
                reg.Tipoptomedicodi = -1;
                reg.Medifecha = day;
                listaTotal.Add(reg);

                var reg2 = new MeMedicion96DTO();
                reg2.Tipoinfocodi = ConstantesTipoInformacion.TipoinfoMW;
                reg2.Tipoptomedicodi = ConstantesInterconexiones.IdTipoPtomedicodiExportacionMwh;
                reg2.Medifecha = day;
                listaTotalExp.Add(reg2);

                var reg3 = new MeMedicion96DTO();
                reg3.Tipoinfocodi = ConstantesTipoInformacion.TipoinfoMW;
                reg3.Tipoptomedicodi = ConstantesInterconexiones.IdTipoPtomedicodiImportacionMwh;
                reg3.Medifecha = day;
                listaTotalImp.Add(reg3);

                //REPARTIR
                var listaDay = listaInterconexion.Where(x => x.Medifecha == day).ToList();

                var regDefault = listaDay.Where(x => x.Tipoptomedicodi == -1).FirstOrDefault() ?? new MeMedicion96DTO();
                var regExpMw = listaDay.Where(x => x.Tipoptomedicodi == ConstantesInterconexiones.IdTipoPtomedicodiExportacionMwh).FirstOrDefault() ?? new MeMedicion96DTO();
                var regImpMw = listaDay.Where(x => x.Tipoptomedicodi == ConstantesInterconexiones.IdTipoPtomedicodiImportacionMwh).FirstOrDefault() ?? new MeMedicion96DTO();

                decimal total = 0M;
                decimal totalExp = 0M;
                decimal totalImp = 0M;
                for (int i = 1; i <= 96; i++)
                {
                    //Convertir MWh a MW  (convertir hora a media hora)
                    decimal valorEpxMW = ((decimal?)regExpMw.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(regExpMw, null) ?? 0) * 4;
                    decimal valorImpMW = ((decimal?)regImpMw.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(regImpMw, null) ?? 0) * 4;
                    decimal valor = valorEpxMW - valorImpMW;

                    reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i).ToString()).SetValue(reg, valor);
                    reg2.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i).ToString()).SetValue(reg2, valorEpxMW);
                    reg3.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i).ToString()).SetValue(reg3, valorImpMW);
                    total += valor;
                    totalExp += valorEpxMW;
                    totalImp += valorImpMW;
                }

                reg.Meditotal = total;
                reg2.Meditotal = totalExp;
                reg3.Meditotal = totalImp;
            }
        }

        /// <summary>
        /// Obtener la data de medidores  (potencia activa)
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="tipoCentral"></param>
        /// <param name="tipoGeneracion"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="estadoValidacion"></param>
        /// <param name="tipoRecurso"></param>
        /// <param name="hayCruceHOP"></param>
        /// <returns></returns>
        public List<MeMedicion96DTO> ListaDataMDGeneracionConsolidado(DateTime fechaIni, DateTime fechaFin, int tipoCentral, string tipoGeneracion, string idEmpresa
           , int estadoValidacion, string tipoRecurso, bool hayCruceHOP)
        {
            return ListaDataMDGeneracionConsolidadoYValidacion(fechaIni, fechaFin, tipoCentral, tipoGeneracion, idEmpresa,
                                            estadoValidacion, tipoRecurso, hayCruceHOP,
                                            ConstantesMedicion.IdTipoInfoPotenciaActiva, out List<LogErrorHOPvsMedidores> listaValidacion);
        }

        public List<MeMedicion96DTO> ListaDataMDGeneracionConsolidadoYValidacion(DateTime fechaIni, DateTime fechaFin, int tipoCentral, string tipoGeneracion, string idEmpresa
       , int estadoValidacion, string tipoRecurso, bool hayCruceHOP, int tipoinfocodi, out List<LogErrorHOPvsMedidores> listaValidacion)
        {
            tipoGeneracion = tipoGeneracion == ConstantesMedicion.IdTipoGeneracionTodos.ToString() ? ConstantesMedicion.ListaGeneracionTodos : tipoGeneracion;
            string idEmpresaValida = idEmpresaValida = this.ListaEmpresaValidadas(fechaFin, idEmpresa, estadoValidacion);

            int lectcodi = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["IdLecturaMedidorGeneracion"]);
            List<MeMedicion96DTO> listaMedicionFuenteTotal = new List<MeMedicion96DTO>();
            listaValidacion = new List<LogErrorHOPvsMedidores>();
            if (idEmpresaValida != null && idEmpresaValida.Length > 0)
            {
                List<MeMedicion96DTO> listaDemanda = new List<MeMedicion96DTO>();

                //Coes, no Coes
                List<PrGrupodatDTO> listaOperacionCoes = FactorySic.GetPrGrupodatRepository().ListarHistoricoValores(ConstantesPR5ReportesServicio.PropGrupoOperacionCoes.ToString(), -1)
                                                            .Where(x => x.Deleted == 0).OrderByDescending(x => x.Fechadat).ToList();

                //Medicion
                List<MeMedicion96DTO> listaM96Rango = FactorySic.GetMeMedicion96Repository().GetConsolidadoMaximaDemanda(tipoCentral, tipoGeneracion, fechaIni, fechaFin, idEmpresaValida
                    , lectcodi, tipoinfocodi);

                foreach (var reg96 in listaM96Rango)
                {
                    reg96.Grupointegrante = ReporteMedidoresAppServicio.SetValorCentralIntegrante(reg96.Grupocodi, reg96.Medifecha, reg96.Grupointegrante, listaOperacionCoes);
                }

                if (tipoCentral == ConstantesMedicion.IdTipogrupoCOES)
                    listaM96Rango = listaM96Rango.Where(x => x.Grupointegrante == ConstantesAppServicio.SI).ToList();
                if (tipoCentral == ConstantesMedicion.IdTipogrupoNoIntegrante)
                    listaM96Rango = listaM96Rango.Where(x => x.Grupointegrante != ConstantesAppServicio.SI).ToList();

                listaDemanda.AddRange(listaM96Rango);

                listaMedicionFuenteTotal = listaDemanda;
            }

            List<MeMedicion96DTO> listaTotal = listaMedicionFuenteTotal;

            //Cruce con Horas de Operación
            if (hayCruceHOP)
            {
                string strFechaCruceHO = System.Configuration.ConfigurationManager.AppSettings["FechaCruceHoraOperacion"];
                DateTime fechaCruceHO = DateTime.ParseExact(strFechaCruceHO, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                DateTime? fechaIniSinCruceHO = null;
                DateTime? fechaFinSinCruceHO = null;
                DateTime? fechaIniConCruceHO = null;
                DateTime? fechaFinConCruceHO = null;
                if (fechaIni <= fechaCruceHO && fechaCruceHO <= fechaFin)
                {
                    fechaIniSinCruceHO = fechaIni.Date;
                    fechaFinSinCruceHO = fechaCruceHO.AddDays(-1);
                    fechaIniConCruceHO = fechaCruceHO;
                    fechaFinConCruceHO = fechaFin.Date;
                }
                else
                {
                    if (fechaCruceHO <= fechaIni)
                    {
                        fechaIniConCruceHO = fechaIni.Date;
                        fechaFinConCruceHO = fechaFin.Date;
                    }
                    else
                    {
                        fechaIniSinCruceHO = fechaIni.Date;
                        fechaFinSinCruceHO = fechaFin.Date;
                    }
                }

                List<MeMedicion96DTO> listaMedicionSinCruce = new List<MeMedicion96DTO>();
                List<MeMedicion96DTO> listaMedicionConCruce = new List<MeMedicion96DTO>();

                if (fechaIniSinCruceHO != null && fechaFinSinCruceHO != null)
                {
                    listaMedicionSinCruce = listaMedicionFuenteTotal.Where(x => x.Medifecha >= fechaIniSinCruceHO && x.Medifecha <= fechaFinSinCruceHO).ToList();
                }
                if (fechaIniConCruceHO != null && fechaFinConCruceHO != null)
                {
                    listaMedicionConCruce = listaMedicionFuenteTotal.Where(x => x.Medifecha >= fechaIniConCruceHO && x.Medifecha <= fechaFinConCruceHO).ToList();
                    List<EveHoraoperacionDTO> listaHOPTotal = (new HorasOperacionAppServicio()).ListarHorasOperacionByCriteria(fechaIniConCruceHO.Value.AddDays(-1), fechaFinConCruceHO.Value.AddDays(2), ConstantesHorasOperacion.ParamEmpresaTodos, ConstantesHorasOperacion.ParamCentralTodos, ConstantesHorasOperacion.TipoListadoTodo);
                    listaMedicionConCruce = this.ListarData96CruceHorasOperacion(fechaIniConCruceHO.Value, fechaFinConCruceHO.Value, listaMedicionConCruce, listaHOPTotal, false, false,
                                                        out listaValidacion);
                }

                listaTotal = new List<MeMedicion96DTO>();
                listaTotal.AddRange(listaMedicionSinCruce);
                listaTotal.AddRange(listaMedicionConCruce);
            }

            //Filtrar Fuente de energía
            if (tipoRecurso != ConstantesMedicion.IdTipoRecursoTodos.ToString())
            {
                int[] result = tipoRecurso.Split(',').Select(x => int.Parse(x)).ToArray();
                listaTotal = listaTotal.Where(x => result.Contains(x.Fenergcodi)).ToList();
            }

            //Generar total
            decimal? valorH = null;
            decimal total = 0;
            List<decimal> listaH = new List<decimal>();

            foreach (var reg in listaTotal)
            {
                listaH = new List<decimal>();
                total = 0;
                for (int h = 1; h <= 96; h++)
                {
                    valorH = (decimal?)reg.GetType().GetProperty("H" + h).GetValue(reg, null);
                    if (valorH != null)
                    {
                        listaH.Add(valorH.Value);
                    }
                }

                if (listaH.Count > 0)
                {
                    total = listaH.Sum(x => x);
                }

                reg.Meditotal = total;
            }

            return listaTotal;
        }

        /// <summary>
        /// Establecer el valor de Grupointegrante para registros cada 15min
        /// </summary>
        /// <param name="reg96"></param>
        /// <param name="listaOperacionCoes"></param>
        public static string SetValorCentralIntegrante(int? grupocodi, DateTime? medifecha, string grupointegrante, List<PrGrupodatDTO> listaOperacionCoes)
        {
            var regDat = listaOperacionCoes.Find(x => x.Fechadat <= medifecha && x.Grupocodi == grupocodi);
            return regDat != null ? regDat.Formuladat : (grupointegrante != null ? grupointegrante : ConstantesAppServicio.NO);
        }

        /// <summary>
        /// Listar Data de Medidores de Generación con Cruce de Horas de Operación
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="listaMedicionFuenteTotal"></param>
        /// <param name="listaHOPTotal"></param>
        /// <param name="estaCompletadoHO"></param>
        /// <param name="soloIncluirSiTieneHOSiosein2"></param>
        /// <param name="listaValidacion"></param>
        /// <returns></returns>
        public List<MeMedicion96DTO> ListarData96CruceHorasOperacion(DateTime fechaIni, DateTime fechaFin,
                            List<MeMedicion96DTO> listaMedicionFuenteTotal, List<EveHoraoperacionDTO> listaHOPTotal,
                            bool estaCompletadoHO, bool soloIncluirSiTieneHOSiosein2, out List<LogErrorHOPvsMedidores> listaValidacion)
        {
            List<MeMedicion96DTO> listaMedicionHOP = new List<MeMedicion96DTO>(); //Lista de potencia clasificada por fuente de energia
            listaValidacion = new List<LogErrorHOPvsMedidores>(); //lista de validaciones

            //Data maestra
            var servHO = (new HorasOperacionAppServicio());
            List<SiFuenteenergiaDTO> listaFuenteEnergia = (new EjecutadoAppServicio()).ListarFuenteEnergia();
            List<SiTipogeneracionDTO> listaTipoGeneracion = this.ListaTipoGeneracion();
            List<PrGrupoDTO> listaGrupoGeneracion = servHO.ListarAllGrupoGeneracion(fechaIni, "'S', 'N'");
            List<PrGrupoDTO> listaModoOpTotal = servHO.ListarModoOperacionXCentralYEmpresa(-2, -2);
            List<int> listaFamcodiCentral = new List<int>() { ConstantesHorasOperacion.IdTipoEolica, ConstantesHorasOperacion.IdTipoSolar, ConstantesHorasOperacion.IdTipoTermica, ConstantesHorasOperacion.IdTipoHidraulica };
            List<int> listaFamcodiTermico = new List<int>() { ConstantesHorasOperacion.IdTipoTermica, ConstantesHorasOperacion.IdGeneradorTemoelectrico };

            //Si la hora de operación fue creado por el aplicativo de escritorio (<add key="FechaCruceHoraOperacion" value="01/08/2019" />) entonces agregarle registros a nivel de unidad
            if (!estaCompletadoHO)
                listaHOPTotal = servHO.CompletarListaHoraOperacionTermo(listaHOPTotal);
            List<EveHoraoperacionDTO> listaHO15min = servHO.ListarHO15minDivididoXh(listaHOPTotal); //agregar bloques adicionales (antes y despues de los bloques que sí tienen cruce)
            if (soloIncluirSiTieneHOSiosein2) listaHO15min = listaHO15min.Where(x => x.Es15MinTieneHo).ToList(); //para el siosein2 no considerar los bloques adicionales (antes y despues de los bloques que sí tienen cruce)

            //Recorrer por dia
            for (DateTime f = fechaIni.Date; f <= fechaFin.Date; f = f.AddDays(1))
            {
                var listaMedicionFuente = listaMedicionFuenteTotal.Where(x => x.Medifecha.Value.Date == f).ToList();

                List<EveHoraoperacionDTO> listaHOModo = listaHO15min.Where(x => x.FechaProceso == f && x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoModo).ToList();
                List<EveHoraoperacionDTO> listaHOUnidad = listaHO15min.Where(x => x.FechaProceso == f && x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoUnidad).ToList();

                //Recorrer por cada generador o central
                foreach (var m96 in listaMedicionFuente)
                {
                    bool esPuntoCentral = listaFamcodiCentral.Contains(m96.Famcodi);
                    bool esPuntoTermico = listaFamcodiTermico.Contains(m96.Famcodi);
                    int equipadre = m96.Equipadre;
                    int equicodi = m96.Equicodi;

                    //Listar horas de operación segun el punto de medición (a nivel de central o de generador)
                    List<EveHoraoperacionDTO> listaHOUnidadXEq = new List<EveHoraoperacionDTO>();
                    if (!esPuntoCentral) listaHOUnidadXEq = listaHOUnidad.Where(x => x.Equicodi == equicodi).ToList();
                    else listaHOUnidadXEq = listaHOUnidad.Where(x => x.Equicodi == equicodi || x.Equipadre == equipadre).ToList();

                    //Fuente de energia por defecto de la unidad (se utiliza cuando no hay potencia o no hay modo de operacion) 
                    SiFuenteenergiaDTO fenergDefaultDia = listaFuenteEnergia.Find(x => x.Fenergcodi == m96.Fenergcodi) ?? new SiFuenteenergiaDTO(); //por defecto del grupo despacho
                    if (listaHOUnidadXEq.Any())
                    {
                        List<int> listaFenergcodiXUnidad = listaHOUnidad.Select(x => x.Fenergcodi).Distinct().ToList();

                        //si solo operó con un combustible durante todo el dia utilizar ese (puede ser distinto al del grupo despacho)
                        if (listaFenergcodiXUnidad.Count == 1)
                        {
                            fenergDefaultDia = listaFuenteEnergia.Find(x => x.Fenergcodi == listaFenergcodiXUnidad[0]) ?? new SiFuenteenergiaDTO();
                        }
                    }

                    //Calcular por cada 1/4 de hora
                    List<MeMedicion96DTO> listaMedicionHOPEquipo = new List<MeMedicion96DTO>();
                    List<LogErrorHOPvsMedidores> listaValidacionEq = new List<LogErrorHOPvsMedidores>();
                    for (int h = 1; h <= 96; h++)
                    {
                        if (m96.Ptomedicodi == 434 && h == 55)
                        { }

                        DateTime fechaHoraH = f.AddMinutes(h * 15);

                        //Potencia de la Unidad
                        decimal? valorH = (decimal?)m96.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m96, null);

                        //filtrar las las horas de operación a nivel de unidades (EVE_HO_UNIDAD)
                        List<EveHoraoperacionDTO> listaHOUnidadXEqH96 = listaHOUnidadXEq.Where(x => x.HIni96 == h).ToList();

                        //determinar la potencia por cada fuente de energia en el cuarto de hora
                        GetFenergcodiYMWXCuartoHora(valorH, listaHOUnidadXEqH96, esPuntoTermico, fenergDefaultDia, listaFuenteEnergia, soloIncluirSiTieneHOSiosein2,
                                                out List<SiFuenteenergiaDTO> listaFenergMWxH, out List<LogErrorHOPvsMedidores> listaValidacionHXPto);

                        //repartir la potencia del cuarto de hora entre los recursos energeticos del punto de medición
                        SetFenergcodiYMWXCuartoHora(h, fechaHoraH, m96, listaFenergMWxH, ref listaMedicionHOPEquipo);

                        //Setear datos para mostrar en pantalla
                        SetDatosValidacioCruceHo(h, valorH, fechaHoraH, m96, ref listaValidacionHXPto);
                        listaValidacionEq.AddRange(listaValidacionHXPto);
                    }

                    //agregar a la lista global
                    listaMedicionHOP.AddRange(listaMedicionHOPEquipo);
                    listaValidacion.AddRange(listaValidacionEq);
                }
            }

            return listaMedicionHOP;
        }

        private void SetFenergcodiYMWXCuartoHora(int h, DateTime fi, MeMedicion96DTO objMe96, List<SiFuenteenergiaDTO> listaFenergMWxH,
                            ref List<MeMedicion96DTO> listaMedicionHOPEquipo)
        {
            //Generar registros pero diferenciados por tipo recurso
            foreach (var objFenerg in listaFenergMWxH)
            {
                MeMedicion96DTO obj96 = listaMedicionHOPEquipo.Find(x => x.Fenergcodi == objFenerg.Fenergcodi);
                if (obj96 == null)
                {
                    //generar objeto
                    obj96 = new MeMedicion96DTO();
                    obj96.Fenergcodi = objFenerg.Fenergcodi; //fuente de combustible
                    obj96.Fenergnomb = objFenerg.Fenergnomb;
                    obj96.Fenercolor = objFenerg.Fenergcolor;

                    obj96.Medifecha = objMe96.Medifecha;
                    obj96.Ptomedicodi = objMe96.Ptomedicodi;
                    obj96.Grupocomb = objMe96.Grupocomb;
                    obj96.Tgenercodi = objMe96.Tgenercodi;
                    obj96.Tgenernomb = objMe96.Tgenernomb;
                    obj96.Emprnomb = objMe96.Emprnomb;
                    obj96.Emprcodi = objMe96.Emprcodi;
                    obj96.Central = objMe96.Central;
                    obj96.Equipadre = objMe96.Equipadre;
                    obj96.Equinomb = objMe96.Equinomb;
                    obj96.Equicodi = objMe96.Equicodi;
                    obj96.Grupocodi = objMe96.Grupocodi;
                    obj96.Tipogrupocodi = objMe96.Tipogrupocodi;
                    obj96.Tipogenerrer = objMe96.Tipogenerrer;
                    obj96.Grupotipocogen = objMe96.Grupotipocogen;
                    obj96.Grupointegrante = objMe96.Grupointegrante;
                    obj96.Famcodi = objMe96.Famcodi;

                    obj96.Meditotal = 0;

                    listaMedicionHOPEquipo.Add(obj96);
                }

                obj96.GetType().GetProperty(ConstantesAppServicio.CaracterH + (h)).SetValue(obj96, objFenerg.Potencia);

                obj96.Meditotal += objFenerg.Potencia.GetValueOrDefault(0);
            }
        }

        private void GetFenergcodiYMWXCuartoHora(decimal? valorH, List<EveHoraoperacionDTO> listaHoUnidad, bool esUnidadTermica, SiFuenteenergiaDTO fenergDefaultDia,
                                                List<SiFuenteenergiaDTO> listaFuenteEnergiaBD, bool soloIncluirSiTieneHOSiosein2,
                                                out List<SiFuenteenergiaDTO> listaFenergMWxH, out List<LogErrorHOPvsMedidores> listaValidacionHXPto)
        {
            listaFenergMWxH = new List<SiFuenteenergiaDTO>(); //fenergcodi, duracion en minutos, potencia MW
            listaValidacionHXPto = new List<LogErrorHOPvsMedidores>(); //mensajes

            //Si es Unidad termoeléctrica y tiene potencia se puede determinar la fuente sino usar la fuente por defecto
            if (esUnidadTermica && valorH.GetValueOrDefault(0) != 0)
            {
                //0. Determinar las fuentes de energia candidatos
                var listaFenergH = new List<SiFuenteenergiaDTO>();
                if (listaHoUnidad.Any())
                {
                    //Determinar bloques de 15 minutos válidos
                    var listaHoUnidadValido = listaHoUnidad.Where(x => x.TotalMinuto > 0).ToList();
                    if (!listaHoUnidadValido.Any())
                    {
                        //si solo existe una fuente de energia o no existe bloques con duracion mayor a 0
                        listaHoUnidadValido.Add(listaHoUnidad.First());
                    }

                    listaFenergH = listaHoUnidadValido.GroupBy(x => x.Fenergcodi)
                                                .Select(x => new SiFuenteenergiaDTO()
                                                { Fenergcodi = x.Key, TotalMinuto = x.Max(z => z.TotalMinuto), Fenergnomb = x.First().Fenergnomb }).ToList();

                    //minutos en horas de operación
                    int totalMinutosHoDistinct = listaFenergH.Sum(x => x.TotalMinuto);

                    //cuando existe más de varias horas ficticias con combustibles distintos (no se puede determinar el combustible correcto y por ello se asume el default)
                    if (listaFenergH.Count() > 1 && totalMinutosHoDistinct == 0)
                    {
                        listaFenergH = new List<SiFuenteenergiaDTO>();
                    }
                }

                //1. Cuando hay horas de operación obtener la fuente de energia del modo de operación
                if (listaFenergH.Any())
                {
                    if (listaFenergH.Count() == 1)
                    {
                        //si solo hay una fuente de energia en el cuarto de hora entonces se asumen que usa todo el tiempo
                        listaFenergMWxH.Add(new SiFuenteenergiaDTO() { Fenergcodi = listaFenergH.First().Fenergcodi, TotalMinuto = listaFenergH.First().TotalMinuto, Potencia = valorH, Fenergnomb = listaFenergH.First().Fenergnomb });
                    }
                    else
                    {
                        int totalMinutosHoDistinct = listaFenergH.Sum(x => x.TotalMinuto);

                        //en caso de dualidad repartir proporcionalmente en el tiempo
                        foreach (var itemFenerg in listaFenergH)
                        {
                            decimal participacion = itemFenerg.TotalMinuto / (totalMinutosHoDistinct * 1.0m);
                            decimal valorMWpart = Math.Round(participacion * valorH.GetValueOrDefault(0), 5);//se redondea 5 decimales para que tenga igual cantidades como lo carga el agente
                            listaFenergMWxH.Add(new SiFuenteenergiaDTO() { Fenergcodi = itemFenerg.Fenergcodi, TotalMinuto = itemFenerg.TotalMinuto, Potencia = valorMWpart, Fenergnomb = itemFenerg.Fenergnomb });
                        }

                        listaValidacionHXPto.Add(new LogErrorHOPvsMedidores()
                        {
                            DescripcionError = string.Format("La unidad presenta más de una fuente de energía: {0}. ", string.Join(" ", listaFenergMWxH.Select(x => string.Format("[{0} - {1}min - {2}MW]", x.Fenergnomb, x.TotalMinuto, x.Potencia))))
                        });
                    }
                }
                else
                {
                    //2. No tiene hora de operación pero sí data en M96 se le colocará 0 si el flag es obligatorio tener HO (numeral de Siosein2)
                    if (soloIncluirSiTieneHOSiosein2) valorH = 0;

                    //si no hay hora de operación utilizar la fuente de energia del grupo despacho
                    listaFenergMWxH.Add(new SiFuenteenergiaDTO() { Fenergcodi = fenergDefaultDia.Fenergcodi, TotalMinuto = 15, Potencia = valorH, Fenergnomb = fenergDefaultDia.Fenergnomb });

                    listaValidacionHXPto.Add(new LogErrorHOPvsMedidores()
                    {
                        DescripcionError = string.Format("La unidad no presenta horas de operación. Se considera fuente de energía {0} del grupo despacho.",
                                                    fenergDefaultDia.Fenergnomb)
                    });
                }
            }
            else
            {
                //eolico, hidraulico, solar y no aplica
                listaFenergMWxH.Add(new SiFuenteenergiaDTO() { Fenergcodi = fenergDefaultDia.Fenergcodi, TotalMinuto = 15, Potencia = valorH });
            }

            //Setear datos de combustible
            foreach (var itemFe in listaFenergMWxH)
            {
                var objBD = listaFuenteEnergiaBD.Find(x => x.Fenergcodi == itemFe.Fenergcodi) ?? new SiFuenteenergiaDTO();
                itemFe.Fenergnomb = objBD.Fenergnomb;
                itemFe.Fenergcolor = objBD.Fenergcolor;
            }
        }

        private void SetDatosValidacioCruceHo(int h, decimal? valorH, DateTime fechaHoraH, MeMedicion96DTO m96, ref List<LogErrorHOPvsMedidores> listaValidacionHXPto)
        {
            foreach (var item in listaValidacionHXPto)
            {
                item.FilaIni = h;
                item.FechaH = fechaHoraH.ToString(ConstantesAppServicio.FormatoFechaHora);

                item.Emprcodi = m96.Emprcodi;
                item.Empresa = m96.Emprnomb;

                item.Central = m96.Central;

                item.Equicodi = m96.Equicodi;
                item.Equipo = m96.Equinomb;

                item.Ptomedicodi = m96.Ptomedicodi;
                item.GrupocodiDespacho = m96.Grupocodi;

                item.PotenciaH = valorH != null ? valorH.ToString() : "";
            }
        }

        /// <summary>
        /// Obtener la data sumarizada de la generación por día
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="listaMDDataGeneracion"></param>
        /// <returns></returns>
        public List<MeMedicion96DTO> ListaDataMDGeneracionFromConsolidado(DateTime fechaIni, DateTime fechaFin, List<MeMedicion96DTO> listaMDDataGeneracion)
        {
            List<MeMedicion96DTO> listaFinal = new List<MeMedicion96DTO>();

            if (listaMDDataGeneracion.Count > 0)
            {
                var regPrimer = listaMDDataGeneracion.First();

                for (DateTime f = fechaIni.Date; f <= fechaFin.Date; f = f.AddDays(1))
                {
                    var listaMedicionFuente = listaMDDataGeneracion.Where(x => x.Medifecha.Value.Date == f).ToList();

                    MeMedicion96DTO reg = new MeMedicion96DTO();
                    reg.Medifecha = f;
                    reg.Tipoinfocodi = regPrimer.Tipoinfocodi;
                    reg.Lectcodi = regPrimer.Lectcodi;

                    foreach (var regtmp in listaMedicionFuente)
                    {
                        for (int i = 1; i <= 96; i++)
                        {
                            decimal? valorNuevo = (decimal?)regtmp.GetType().GetProperty("H" + (i)).GetValue(regtmp, null);
                            decimal? valorAcum = (decimal?)reg.GetType().GetProperty("H" + (i)).GetValue(reg, null);
                            if (valorNuevo != null)
                            {
                                valorAcum = valorAcum.GetValueOrDefault(0) + valorNuevo;
                                reg.GetType().GetProperty("H" + (i)).SetValue(reg, valorAcum);
                            }
                        }
                    }

                    listaFinal.Add(reg);
                }

                decimal? valorH = null;
                decimal total = 0;
                List<decimal> listaH = new List<decimal>();

                foreach (var reg in listaFinal)
                {
                    listaH = new List<decimal>();
                    total = 0;
                    for (int h = 1; h <= 96; h++)
                    {
                        valorH = (decimal?)reg.GetType().GetProperty("H" + h).GetValue(reg, null);
                        if (valorH != null)
                        {
                            listaH.Add(valorH.Value);
                        }
                    }

                    if (listaH.Count > 0)
                    {
                        total = listaH.Sum(x => x);
                    }

                    reg.Meditotal = total;
                }
            }

            return listaFinal;
        }

        /// <summary>
        /// Obtener día y hora de la máxima demanda del sistema menos 15minutos
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public DateTime GetDiaPeriodoDemanda96(DateTime fechaInicio, DateTime fechaFin)
        {
            return GetDiaPeriodoDemanda96XFiltro(fechaInicio, fechaFin, ConstantesRepMaxDemanda.TipoMDNormativa, ConstantesMedicion.IdTipogrupoCOES, ConstantesMedicion.IdTipoGeneracionTodos, ConstantesMedicion.IdEmpresaTodos, ConstanteValidacion.EstadoTodos);
        }

        /// <summary>
        /// Obtener día y hora de la máxima demanda menos 15minutos
        /// Si la maxima hora es las H96, devolvería la fecha del dia siguiente por tanto se le resta 15min para que devuelva el mismo día,
        /// posteiormente para efectos de reporte se le suma 15min, pero para consulta de data se toma el Medifecha.Date
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="bloquePeriodo"></param>
        /// <param name="tipoCentral"></param>
        /// <param name="tipoGeneracion"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="estadoValidacion"></param>
        /// <returns></returns>
        public DateTime GetDiaPeriodoDemanda96XFiltro(DateTime fechaInicio, DateTime fechaFin, int bloquePeriodo, int tipoCentral, int tipoGeneracion, int idEmpresa, int estadoValidacion)
        {
            //Data Generación
            List<MeMedicion96DTO> listaDemanda = this.ListaDataMDGeneracion(fechaInicio, fechaFin, tipoCentral, tipoGeneracion.ToString(), idEmpresa.ToString(), estadoValidacion);

            //Data Interconexion
            List<MeMedicion96DTO> listaInterconexion = this.ListaDataMDInterconexion96(fechaInicio, fechaFin);

            //Data Total
            List<MeMedicion96DTO> listaMedicion = this.ListaDataMDTotalSEIN(listaDemanda, listaInterconexion);

            //Parametros para calculo de MD
            List<SiParametroValorDTO> listaRangoNormaHP = servParametro.ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroRangoPeriodoHP);
            List<SiParametroValorDTO> listaBloqueHorario = servParametro.ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroHPPotenciaActiva);

            //Cálculo
            GetDiaMaximaDemandaFromDataMD96(fechaInicio, fechaFin, bloquePeriodo, listaMedicion, listaRangoNormaHP, listaBloqueHorario,
                                        out DateTime fechaMD, out DateTime fechaDia, out int hMax);

            //Si la maxima hora es las H96, devolvería la fecha del dia siguiente por tanto se le resta 15min para que devuelva el mismo día,
            //posteiormente para efectos de reporte se le suma 15min, pero para consulta de data se toma el Medifecha.Date
            return fechaDia.AddMinutes((hMax - 1) * 15);
        }

        /// <summary>
        /// Obtener día de la máxima demanda segun data de entrada
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="bloquePeriodo"></param>
        /// <param name="listaMedicionTotal"></param>
        /// <param name="listaRangoNormaHP"></param>
        /// <param name="listaBloqueHorario"></param>
        /// <param name="fechaMD"></param>
        /// <param name="fechaDia"></param>
        /// <param name="hMax"></param>
        public void GetDiaMaximaDemandaFromDataMD96(DateTime fechaInicio, DateTime fechaFin, int bloquePeriodo,
            List<MeMedicion96DTO> listaMedicionTotal, List<SiParametroValorDTO> listaRangoNormaHP, List<SiParametroValorDTO> listaBloqueHorario,
            out DateTime fechaMD, out DateTime fechaDia, out int hMax)
        {
            List<MeMedicion96DTO> listaMedicion = listaMedicionTotal.Where(x => x.Medifecha >= fechaInicio.Date && x.Medifecha <= fechaFin.Date).ToList();

            decimal maximoValor = 0;
            decimal valorH = 0;
            DateTime maximoValorDia = fechaInicio.Date;
            int maximoValorColumna = 1;

            if (listaMedicion.Count() != 0)
            {
                for (var i = 0; i < listaMedicion.Count(); i++)
                {
                    MedidoresHelper.ObtenerValorHXPeriodoDemandaM96(bloquePeriodo, listaMedicion[i].Medifecha.Value, new List<MeMedicion96DTO>() { listaMedicion[i] }, listaRangoNormaHP, listaBloqueHorario,
                                                                out valorH, out int j, out DateTime fechaHoraOut, out _);

                    if (valorH > maximoValor)
                    {
                        maximoValor = valorH;
                        maximoValorDia = listaMedicion[i].Medifecha.Value;
                        maximoValorColumna = j;
                    }
                }
            }

            //salidas
            fechaDia = maximoValorDia;
            fechaMD = maximoValorDia.AddMinutes(maximoValorColumna * 15);
            hMax = maximoValorColumna;
        }

        /// <summary>
        /// Método que devuelve los bloques del dia de MD
        /// </summary>
        /// <param name="fechaMaximaDemanda"></param>
        /// <param name="tipoCentral"></param>
        /// <param name="tipoGeneracion"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="estadoValidacion"></param>
        /// <returns></returns>
        public List<MaximaDemandaDTO> ListarResumenDiaMaximaDemanda96(DateTime fechaMaximaDemanda, int tipoCentral, int tipoGeneracion, int idEmpresa, int estadoValidacion)
        {
            var lista = ListarBloqueResumenDiaMaximaDemanda96(fechaMaximaDemanda, tipoCentral, tipoGeneracion, idEmpresa, estadoValidacion);

            return lista.Where(x => x.CodigoHorario == ConstantesRepMaxDemanda.CodigoHorarioMaxima
                                || x.CodigoHorario == ConstantesRepMaxDemanda.CodigoHorarioMedia
                                || x.CodigoHorario == ConstantesRepMaxDemanda.CodigoHorarioMinima).ToList();
        }

        /// <summary>
        /// Método que devuelve los bloques del dia de MD, HP y HFP
        /// </summary>
        /// <param name="fechaMaximaDemanda"></param>
        /// <param name="tipoCentral"></param>
        /// <param name="tipoGeneracion"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="estadoValidacion"></param>
        /// <returns></returns>
        public List<MaximaDemandaDTO> ListarResumenDiaMaximaDemanda96HPyHFP(DateTime fechaMaximaDemanda, int tipoCentral, int tipoGeneracion, int idEmpresa, int estadoValidacion)
        {
            var lista = ListarBloqueResumenDiaMaximaDemanda96(fechaMaximaDemanda, tipoCentral, tipoGeneracion, idEmpresa, estadoValidacion);

            return lista.Where(x => x.CodigoHorario == ConstantesRepMaxDemanda.CodigoHorarioMD
                                || x.CodigoHorario == ConstantesRepMaxDemanda.CodigoHorarioHP
                                || x.CodigoHorario == ConstantesRepMaxDemanda.CodigoHorarioHFP).ToList();
        }

        /// <summary>
        /// Método que devuelve la máxima demanda para el Portal
        /// </summary>
        /// <param name="fechaMaximaDemanda"></param>
        /// <param name="tipoCentral"></param>
        /// <param name="tipoGeneracion"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="estadoValidacion"></param>
        /// <returns></returns>
        public MaximaDemandaDTO GetResumenDiaMaximaDemanda96(DateTime fechaMaximaDemanda, int tipoCentral, int tipoGeneracion, int idEmpresa, int estadoValidacion)
        {
            var lista = ListarBloqueResumenDiaMaximaDemanda96(fechaMaximaDemanda, tipoCentral, tipoGeneracion, idEmpresa, estadoValidacion);

            return lista.Find(x => x.CodigoHorario == ConstantesRepMaxDemanda.CodigoHorarioMD) ?? new MaximaDemandaDTO();
        }

        private List<MaximaDemandaDTO> ListarBloqueResumenDiaMaximaDemanda96(DateTime fechaMaximaDemanda, int tipoCentral, int tipoGeneracion, int idEmpresa, int estadoValidacion)
        {
            DateTime fechaPeriodo = new DateTime(fechaMaximaDemanda.Year, fechaMaximaDemanda.Month, 1);

            //Parametros para calculo de MD
            List<SiParametroValorDTO> listaRangoNormaHP = servParametro.ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroRangoPeriodoHP);
            List<SiParametroValorDTO> listaBloqueHorario = servParametro.ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroHPPotenciaActiva);

            //Data Generación
            List<MeMedicion96DTO> listaDemanda = this.ListaDataMDGeneracion(fechaMaximaDemanda, fechaMaximaDemanda, tipoCentral, tipoGeneracion.ToString(), idEmpresa.ToString(), estadoValidacion);

            //Data Interconexion
            List<MeMedicion96DTO> listaInterconexion = this.ListaDataMDInterconexion96(fechaMaximaDemanda, fechaMaximaDemanda);

            //Data Total
            List<MeMedicion96DTO> listaMedicion = this.ListaDataMDTotalSEIN(listaDemanda, listaInterconexion);

            MeMedicion96DTO dmdTotalDia = listaMedicion.FirstOrDefault();
            MeMedicion96DTO dmdInterconexionDia = listaInterconexion.FirstOrDefault();

            //Reporte

            MaximaDemandaDTO bloqueMaximaDemanda = new MaximaDemandaDTO();
            bloqueMaximaDemanda.CodigoHorario = ConstantesRepMaxDemanda.CodigoHorarioMaxima;
            bloqueMaximaDemanda.TipoHorario = "MÁXIMA";
            bloqueMaximaDemanda.FechaHora = fechaPeriodo;

            MaximaDemandaDTO bloqueMediaDemanda = new MaximaDemandaDTO();
            bloqueMediaDemanda.CodigoHorario = ConstantesRepMaxDemanda.CodigoHorarioMedia;
            bloqueMediaDemanda.TipoHorario = "MEDIA";
            bloqueMediaDemanda.FechaHora = fechaPeriodo;

            MaximaDemandaDTO bloqueMinimaDemanda = new MaximaDemandaDTO();
            bloqueMinimaDemanda.CodigoHorario = ConstantesRepMaxDemanda.CodigoHorarioMinima;
            bloqueMinimaDemanda.TipoHorario = "MÍNIMA";
            bloqueMinimaDemanda.FechaHora = fechaPeriodo;

            MaximaDemandaDTO bloqueMD = new MaximaDemandaDTO();
            bloqueMD.CodigoHorario = ConstantesRepMaxDemanda.CodigoHorarioMD;
            bloqueMD.TipoHorario = "";
            bloqueMD.FechaHora = fechaPeriodo;

            MaximaDemandaDTO bloqueHP = new MaximaDemandaDTO();
            bloqueHP.CodigoHorario = ConstantesRepMaxDemanda.CodigoHorarioHP;
            bloqueHP.TipoHorario = "HP";
            bloqueHP.FechaHora = fechaPeriodo;

            MaximaDemandaDTO bloqueHFP = new MaximaDemandaDTO();
            bloqueHFP.CodigoHorario = ConstantesRepMaxDemanda.CodigoHorarioHFP;
            bloqueHFP.TipoHorario = "HFP";
            bloqueHFP.FechaHora = fechaPeriodo;

            if (dmdTotalDia != null)
            {
                DateTime fechaMaximoValorDia = dmdTotalDia.Medifecha.Value;

                MedidoresHelper.ObtenerValorHXPeriodoDemandaM96(ConstantesRepMaxDemanda.TipoMDNormativa, fechaMaximoValorDia, new List<MeMedicion96DTO>() { dmdTotalDia }, listaRangoNormaHP, listaBloqueHorario,
                                                                out decimal valorMD, out int hMD, out DateTime fechaHoraMD, out int tipoCalculoMD);

                MedidoresHelper.ObtenerValorHXPeriodoDemandaM96(ConstantesRepMaxDemanda.TipoPeriodoBloqueMaxima, fechaMaximoValorDia, new List<MeMedicion96DTO>() { dmdTotalDia }, null, listaBloqueHorario,
                                                                out decimal valorBMax, out int hBMax, out DateTime fechaHoraBMax, out _);
                MedidoresHelper.ObtenerValorHXPeriodoDemandaM96(ConstantesRepMaxDemanda.TipoPeriodoBloqueMedia, fechaMaximoValorDia, new List<MeMedicion96DTO>() { dmdTotalDia }, null, listaBloqueHorario,
                                                                out decimal valorBMedia, out int hBMedia, out DateTime fechaHoraBMedia, out _);
                MedidoresHelper.ObtenerValorHXPeriodoDemandaM96(ConstantesRepMaxDemanda.TipoPeriodoBloqueMinima, fechaMaximoValorDia, new List<MeMedicion96DTO>() { dmdTotalDia }, null, listaBloqueHorario,
                                                                out decimal valorBMin, out int hBMin, out DateTime fechaHoraBMin, out _);

                MedidoresHelper.ObtenerValorHXPeriodoDemandaM96(ConstantesRepMaxDemanda.TipoHoraPunta, fechaMaximoValorDia, new List<MeMedicion96DTO>() { dmdTotalDia }, null, listaBloqueHorario,
                                                                out decimal valorHP, out int hHP, out DateTime fechaHoraHP, out _);

                MedidoresHelper.ObtenerValorHXPeriodoDemandaM96(ConstantesRepMaxDemanda.TipoFueraHoraPunta, fechaMaximoValorDia, new List<MeMedicion96DTO>() { dmdTotalDia }, null, listaBloqueHorario,
                                                                out decimal valorHFP, out int hHFP, out DateTime fechaHoraHFP, out _);

                //Calculo de Bloque Máxima demanda
                bloqueMaximaDemanda.FechaHora = fechaMaximoValorDia.AddMinutes(hBMax * 15);
                bloqueMaximaDemanda.Valor = valorBMax;
                bloqueMaximaDemanda.ValorInter = (dmdInterconexionDia != null) ? ((decimal?)dmdInterconexionDia.GetType().GetProperty("H" + hBMax.ToString()).GetValue(dmdInterconexionDia, null)).GetValueOrDefault(0) : 0M;
                bloqueMaximaDemanda.HDemanda = hBMax;

                //Calculo de Bloque Media demanda
                bloqueMediaDemanda.FechaHora = fechaMaximoValorDia.AddMinutes(hBMedia * 15);
                bloqueMediaDemanda.Valor = valorBMedia;
                bloqueMediaDemanda.ValorInter = (dmdInterconexionDia != null) ? ((decimal?)dmdInterconexionDia.GetType().GetProperty("H" + hBMedia.ToString()).GetValue(dmdInterconexionDia, null)).GetValueOrDefault(0) : 0M;
                bloqueMediaDemanda.HDemanda = hBMedia;

                //Calculo de Bloque minima demanda
                bloqueMinimaDemanda.FechaHora = fechaMaximoValorDia.AddMinutes(hBMin * 15);
                bloqueMinimaDemanda.Valor = valorBMin;
                bloqueMinimaDemanda.ValorInter = (dmdInterconexionDia != null) ? ((decimal?)dmdInterconexionDia.GetType().GetProperty("H" + hBMin.ToString()).GetValue(dmdInterconexionDia, null)).GetValueOrDefault(0) : 0M;
                bloqueMinimaDemanda.HDemanda = hBMin;

                //Cálculo de MD
                bloqueMD.FechaHora = fechaMaximoValorDia.AddMinutes(hMD * 15);
                bloqueMD.Valor = valorMD;
                bloqueMD.ValorInter = (dmdInterconexionDia != null) ? ((decimal?)dmdInterconexionDia.GetType().GetProperty("H" + hMD.ToString()).GetValue(dmdInterconexionDia, null)).GetValueOrDefault(0) : 0M;
                bloqueMD.HDemanda = hMD;
                bloqueMD.TieneCalculoMDenHP = ConstantesRepMaxDemanda.TipoHoraPunta == tipoCalculoMD;
                if (bloqueMD.TieneCalculoMDenHP)
                {
                    var listaParam15min = ParametroAppServicio.GetListaParametroHPPotenciaActiva(listaBloqueHorario, fechaMaximoValorDia.Date, ParametrosFormato.ResolucionCuartoHora).OrderByDescending(x => x.Fecha).ToList();
                    var paramHp = listaParam15min.Find(x => x.EsVigente) ?? new ParametroHPPotenciaActiva();
                    bloqueMD.HorarioHPDesc = string.Format("Se considera el periodo de Horas Punta desde las {0} h a {1} h.", paramHp.HoraMedia, paramHp.HoraMaxima);
                }

                //Cálculo de HP
                bloqueHP.FechaHora = fechaMaximoValorDia.AddMinutes(hHP * 15);
                bloqueHP.Valor = valorHP;
                bloqueHP.ValorInter = (dmdInterconexionDia != null) ? ((decimal?)dmdInterconexionDia.GetType().GetProperty("H" + hHP.ToString()).GetValue(dmdInterconexionDia, null)).GetValueOrDefault(0) : 0M;
                bloqueHP.HDemanda = hHP;

                //Cálculo de HP
                bloqueHFP.FechaHora = fechaMaximoValorDia.AddMinutes(hHFP * 15);
                bloqueHFP.Valor = valorHFP;
                bloqueHFP.ValorInter = (dmdInterconexionDia != null) ? ((decimal?)dmdInterconexionDia.GetType().GetProperty("H" + hHFP.ToString()).GetValue(dmdInterconexionDia, null)).GetValueOrDefault(0) : 0M;
                bloqueHFP.HDemanda = hHFP;
            }

            List<MaximaDemandaDTO> listaMaximaDemanda = new List<MaximaDemandaDTO>
            {
                bloqueMaximaDemanda,
                bloqueMediaDemanda,
                bloqueMinimaDemanda,
                bloqueMD,
                bloqueHP,
                bloqueHFP
            };

            foreach (var bloque in listaMaximaDemanda)
            {
                bloque.FechaHoraFull = bloque.FechaHora.ToString(ConstantesBase.FormatFechaFull);
                bloque.FechaOnlyDia = bloque.FechaHora.ToString("dd/MM/yyyy");
                bloque.FechaOnlyHora = bloque.FechaHora.ToString("HH:mm");
            }

            return listaMaximaDemanda;
        }

        public List<ConsolidadoEnvioDTO> GetConsolidadoMaximaDemanda96(DateTime fechaMaximaDemanda, int tipoCentral, int tipoGeneracion, int idEmpresa, int estadoValidacion)
        {
            List<ConsolidadoEnvioDTO> lista = new List<ConsolidadoEnvioDTO>();

            int cuartoHora = fechaMaximaDemanda.Hour * 4 + fechaMaximaDemanda.Minute / 15;
            cuartoHora = cuartoHora + 1;
            DateTime FechaSinMinutos = new DateTime(fechaMaximaDemanda.Year, fechaMaximaDemanda.Month, fechaMaximaDemanda.Day);

            var listaMedicion = this.ListaDataMDGeneracionConsolidado(fechaMaximaDemanda, fechaMaximaDemanda, tipoCentral, tipoGeneracion.ToString(), idEmpresa.ToString()
                , estadoValidacion, ConstantesMedicion.IdTipoRecursoTodos.ToString(), false);

            foreach (var medicion in listaMedicion)
            {
                ConsolidadoEnvioDTO reg = new ConsolidadoEnvioDTO();
                reg.Emprcodi = medicion.Emprcodi;
                reg.Empresa = medicion.Emprnomb;
                reg.IdCentral = ((short)medicion.Famcodi == 4 || (short)medicion.Famcodi == 5 || (short)medicion.Famcodi == 37 || (short)medicion.Famcodi == 39) ? (short)medicion.Equicodi : (short)medicion.Equipadre;
                //reg.Central = ((short)medicion.Famcodi == 4 || (short)medicion.Famcodi == 5 || (short)medicion.Famcodi == 37 || (short)medicion.Famcodi == 39) ? medicion.Equinomb : medicion.Central;
                reg.Central = medicion.Central;
                reg.GrupSSAA = medicion.Equinomb;
                reg.Equicodi = medicion.Equicodi;
                reg.Total = ((decimal?)medicion.GetType().GetProperty("H" + cuartoHora.ToString()).GetValue(medicion, null)).GetValueOrDefault(0);
                reg.TipoGeneracion = (short)medicion.Tgenercodi;
                lista.Add(reg);
            }

            lista = lista.OrderBy(x => x.Empresa).ThenBy(x => x.Central).ThenBy(x => x.GrupSSAA).ToList();

            return lista;
        }

        public List<ConsolidadoEnvioDTO> GetRecursoEnergeticoMaximaDemanda96(DateTime fechaMaximaDemanda, int tipoCentral, int tipoGeneracion, int idEmpresa, int estadoValidacion)
        {
            List<MeMedicion96DTO> listaMedicion = new List<MeMedicion96DTO>();
            List<ConsolidadoEnvioDTO> lista = new List<ConsolidadoEnvioDTO>();

            int cuartoHora = fechaMaximaDemanda.Hour * 4 + fechaMaximaDemanda.Minute / 15;
            cuartoHora = cuartoHora + 1;
            DateTime FechaSinMinutos = new DateTime(fechaMaximaDemanda.Year, fechaMaximaDemanda.Month, fechaMaximaDemanda.Day);

            listaMedicion = this.ListaDataMDGeneracionConsolidado(fechaMaximaDemanda, fechaMaximaDemanda, tipoCentral, tipoGeneracion.ToString(), idEmpresa.ToString()
                , estadoValidacion, ConstantesMedicion.IdTipoRecursoTodos.ToString(), true);

            var listaGrupoRecEnerg = listaMedicion
                .GroupBy(x => new
                {
                    x.Fenergcodi,
                    x.Fenergnomb,
                    x.Fenercolor
                })
                .Select(x => new ConsolidadoEnvioDTO
                {
                    Fenergcodi = x.Key.Fenergcodi,
                    Fenergnomb = x.Key.Fenergnomb,
                    Fenercolor = x.Key.Fenercolor
                }).ToList();

            foreach (var tipoGen in listaGrupoRecEnerg)
            {
                decimal total = 0;
                var listaTmpMed = listaMedicion.Where(x => x.Fenergcodi == tipoGen.Fenergcodi).ToList();
                foreach (var medicion in listaTmpMed)
                {
                    total += ((decimal?)medicion.GetType().GetProperty("H" + cuartoHora.ToString()).GetValue(medicion, null)).GetValueOrDefault(0);
                }


                ConsolidadoEnvioDTO reg = new ConsolidadoEnvioDTO();
                reg.Fenergcodi = tipoGen.Fenergcodi;
                reg.Fenergnomb = tipoGen.Fenergnomb;
                reg.Fenercolor = tipoGen.Fenercolor;
                reg.Total = total;

                lista.Add(reg);
            }

            //calcular total
            decimal totalGeneracion = lista.Sum(x => x.Total);

            //asignar porcentaje
            foreach (var med in lista)
            {
                med.Porcentaje = 0;
                if (totalGeneracion > 0)
                {
                    med.Porcentaje = med.Total / totalGeneracion * 100;
                }
            }

            lista = lista.OrderBy(x => x.Fenergnomb).ToList();

            return lista;
        }

        /// <summary>
        /// Retornar todos los cuartos de hora del día de máxima
        /// </summary>
        /// <param name="Fecha"></param>
        /// <param name="Bloque"></param>
        /// <param name="integrante"></param>
        /// <returns></returns>
        public List<MaximaDemandaDTO> GetResumenDetalleDiaMaximaDemanda96(DateTime fechaMaximaDemanda, int tipoCentral, int tipoGeneracion, int idEmpresa, int estadoValidacion, DateTime fechaPeriodo)
        {
            List<MaximaDemandaDTO> listaMaximaDemanda = new List<MaximaDemandaDTO>();
            decimal valorH = 0;

            ////////// Fin de Declaracion de Variables

            //Data Generación
            List<MeMedicion96DTO> listaDemanda = this.ListaDataMDGeneracion(fechaMaximaDemanda, fechaMaximaDemanda, tipoCentral, tipoGeneracion.ToString(), idEmpresa.ToString(), estadoValidacion);

            //Data Interconexion
            List<MeMedicion96DTO> listaInterconexion = this.ListaDataMDInterconexion96(fechaMaximaDemanda, fechaMaximaDemanda);

            //Data Total
            List<MeMedicion96DTO> listaMedicion = this.ListaDataMDTotalSEIN(listaDemanda, listaInterconexion);

            MeMedicion96DTO dmdTotalDia = listaMedicion.FirstOrDefault();
            MeMedicion96DTO dmdInterconexionDia = listaInterconexion.FirstOrDefault();

            if (dmdTotalDia != null)
            {

                for (var j = 1; j <= 96; j++)
                {
                    valorH = ((decimal?)dmdTotalDia.GetType().GetProperty("H" + j.ToString()).GetValue(dmdTotalDia, null)).GetValueOrDefault(0);

                    //Calculo de la Máxima demanda
                    MaximaDemandaDTO bloqueDemanda = new MaximaDemandaDTO();
                    bloqueDemanda.FechaHora = dmdTotalDia.Medifecha.Value.AddMinutes(j * 15);
                    bloqueDemanda.Valor = valorH;
                    bloqueDemanda.ValorInter = (dmdInterconexionDia != null) ? ((decimal?)dmdInterconexionDia.GetType().GetProperty("H" + j.ToString()).GetValue(dmdInterconexionDia, null)).GetValueOrDefault(0) : 0M;

                    listaMaximaDemanda.Add(bloqueDemanda);
                }
            }

            foreach (var bloque in listaMaximaDemanda)
            {
                bloque.FechaHoraFull = bloque.FechaHora.ToString(ConstantesBase.FormatFechaFull);
                bloque.FechaOnlyDia = bloque.FechaHora.ToString("dd/MM/yyyy");
                bloque.FechaOnlyHora = bloque.FechaHora.ToString("HH:mm");
            }

            return listaMaximaDemanda;

        }

        public List<WbResumenmddetalleDTO> GetResumenDetalleDiaMaximaDemanda(int rsmcodi)
        {
            List<WbResumenmddetalleDTO> listaMaximaDemanda = new List<WbResumenmddetalleDTO>();

            listaMaximaDemanda = FactorySic.GetWbResumenmddetalleRepository().GetByIdMd(rsmcodi);

            return listaMaximaDemanda;
        }

        /// <summary>
        /// ObtenerDatosReporteDiagramaCarga
        /// </summary>
        /// <param name="fechaProceso"></param>
        /// <returns></returns>
        public List<MaximaDemandaDTO> ObtenerDatosReporteDiagramaCarga(DateTime fechaProceso)
        {
            DateTime fechaIni = fechaProceso;
            DateTime fechaFin = fechaProceso.AddMonths(1).AddDays(-1);

            bool esPortal = false;
            int estadoValidacion = esPortal ? ConstanteValidacion.EstadoValidado : ConstanteValidacion.EstadoTodos;
            int tipoCentral = 1;
            int tipoGeneracion = 0;

            DateTime diaMaximaDemanda = this.GetDiaPeriodoDemanda96XFiltro(fechaIni, fechaFin, ConstantesRepMaxDemanda.TipoMDNormativa, tipoCentral, tipoGeneracion, ConstantesMedicion.IdEmpresaTodos, estadoValidacion);

            //Lista demanda por hora
            List<MaximaDemandaDTO> list = this.GetResumenDetalleDiaMaximaDemanda96(diaMaximaDemanda, tipoCentral, tipoGeneracion, ConstantesMedicion.IdEmpresaTodos, estadoValidacion, fechaProceso);

            return list;
        }


        public List<MeMedicion96DTO> GetConsolidadoExcesoPotenReact(int tipoCentral, int tipoGeneracion, string idEmpresa, string fechaIni, string fechaFin, int famcodiSSAA, int lectcodi, int tipoinfocodi, int tptomedicodi)
        {
            return FactorySic.GetMeMedicion96Repository().GetConsolidadoExcesoPotenReact(tipoCentral, tipoGeneracion, idEmpresa, fechaIni, fechaFin, ConstantesMedicion.IdFamiliaSSAA, lectcodi, tipoinfocodi, tptomedicodi);
        }

        public string GenerarFileExcelReporteMaximaDemanda(string nombreHoja, string titulo, string strMes, List<MaximaDemandaDTO> listaResumenDemanda, List<ConsolidadoEnvioDTO> listaConsolidadoDemanda, List<string> listaNormativa)
        {
            string fileExcel = string.Empty;

            using (ExcelPackage xlPackage = new ExcelPackage())
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nombreHoja);
                ws.View.ShowGridLines = false;

                int col = 2;
                int row = 1;

                #region inicio
                int rowTitulo = 2;
                ws.Cells[rowTitulo, col + 1].Value = titulo;
                ws.Cells[rowTitulo, col + 1].Style.Font.Bold = true;
                ws.Cells[rowTitulo + 1, col + 1].Value = strMes;

                row = 6;
                int rowPR = row;
                ws.Cells[row, col].Value = "CONFORME A LOS SIGUIENTES PROCEDIMIENTOS TÉCNICOS DEL COES:";
                ws.Cells[row, col].Style.Font.Bold = true;
                row++;
                ws.Cells[row, col].Value = "PR-N° 30: VALORIZACIÓN DE LAS TRANSFERENCIAS DE POTENCIA Y COMPENSACIONES AL SISTEMA PRINCIPAL Y SISTEMA GARANTIZADO DE TRANSMISIÓN (Vigente desde el 01 de junio de 2015)";
                ws.Cells[row, col].Style.Indent = 2;
                ws.Cells[row, col].Style.Font.Bold = true;
                row++;
                ws.Cells[row, col].Value = "PR-N° 43: INTERCAMBIOS INTERNACIONALES DE ELECTRICIDAD EN EL MARCO DE LA DECISIÓN 757 DE LA CAN";
                ws.Cells[row, col].Style.Indent = 2;
                ws.Cells[row, col].Style.Font.Bold = true;

                foreach (var reg in listaNormativa)
                {
                    row++;
                    ws.Cells[row, col].Value = reg;
                    ws.Cells[row, col].Style.Indent = 2;
                    ws.Cells[row, col].Style.Font.Bold = true;
                }
                #endregion

                row += 2;

                #region cabecera Resumen
                int rowIniBloqueHorario = row;
                int rowFinBloqueHorario = rowIniBloqueHorario + 1;
                int rowIniInterconexión = row;
                int rowMW = rowIniInterconexión + 1;

                int colIniBloqueHorario = col;
                int colIniFechaHora = colIniBloqueHorario + 1;
                int colIniInterconexión = colIniFechaHora + 1;
                int colFinInterconexión = colIniInterconexión + 1;
                int colIniPeruEcu = colIniFechaHora + 1;
                int colIniEcuPeru = colIniPeruEcu + 1;
                int colIniSein = colIniEcuPeru + 1;

                ws.Cells[rowIniBloqueHorario, colIniBloqueHorario].Value = "Bloque Horario";
                ws.Cells[rowIniBloqueHorario, colIniBloqueHorario, rowFinBloqueHorario, colIniBloqueHorario].Merge = true;

                ws.Cells[rowIniBloqueHorario, colIniFechaHora].Value = "Fecha Hora";
                ws.Cells[rowIniBloqueHorario, colIniFechaHora, rowFinBloqueHorario, colIniFechaHora].Merge = true;

                ws.Cells[rowIniInterconexión, colIniInterconexión].Value = "Interconexión";
                ws.Cells[rowIniInterconexión, colIniInterconexión, rowIniInterconexión, colFinInterconexión].Merge = true;

                ws.Cells[rowMW, colIniPeruEcu].Value = "PER-ECU Exportación \n MW";
                ws.Cells[rowMW, colIniPeruEcu].Style.WrapText = true;

                ws.Cells[rowMW, colIniEcuPeru].Value = "ECU-PER Importación \n MW";
                ws.Cells[rowMW, colIniEcuPeru].Style.WrapText = true;

                ws.Cells[rowIniBloqueHorario, colIniSein].Value = "SEIN \n MW";
                ws.Cells[rowIniBloqueHorario, colIniSein, rowFinBloqueHorario, colIniSein].Merge = true;
                ws.Cells[rowIniBloqueHorario, colIniSein, rowFinBloqueHorario, colIniSein].Style.WrapText = true;

                using (var range = ws.Cells[rowIniBloqueHorario, colIniBloqueHorario, rowFinBloqueHorario, colIniSein])
                {
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Color.SetColor(Color.Black);
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Color.SetColor(Color.Black);
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Color.SetColor(Color.Black);
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(Color.Black);

                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    range.Style.Font.Color.SetColor(Color.White);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                row = rowFinBloqueHorario;
                #endregion
                row += 1;
                #region cuerpo Resumen
                foreach (var res in listaResumenDemanda)
                {
                    var export = 0M;
                    var import = 0M;
                    if (res.ValorInter > 0)
                    {
                        export = res.ValorInter;
                    }
                    else
                    {
                        import = res.ValorInter * -1;
                    }

                    ws.Cells[row, colIniBloqueHorario].Value = res.TipoHorario;
                    ws.Cells[row, colIniBloqueHorario].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    ws.Cells[row, colIniFechaHora].Value = res.FechaHoraFull;
                    ws.Cells[row, colIniFechaHora].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[row, colIniFechaHora].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    ws.Cells[row, colIniPeruEcu].Value = export;
                    ws.Cells[row, colIniPeruEcu].Style.Numberformat.Format = "0.000";
                    ws.Cells[row, colIniPeruEcu].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    ws.Cells[row, colIniEcuPeru].Value = import;
                    ws.Cells[row, colIniEcuPeru].Style.Numberformat.Format = "0.000";
                    ws.Cells[row, colIniEcuPeru].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    ws.Cells[row, colIniSein].Value = res.Valor;
                    ws.Cells[row, colIniSein].Style.Numberformat.Format = "0.000";
                    ws.Cells[row, colIniSein].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    row++;
                }

                #endregion

                row += 2;

                #region cabecera Consolidado
                int rowIniEmpresa = row;

                int colIniEmpresa = col;
                int colIniCentral = colIniEmpresa + 1;
                int colIniUnidad = colIniCentral + 1;
                int colIniMW = colIniUnidad + 1;

                ws.Cells[rowIniEmpresa, colIniEmpresa].Value = "Empresa";
                ws.Cells[rowIniEmpresa, colIniCentral].Value = "Central";
                ws.Cells[rowIniEmpresa, colIniUnidad].Value = "Unidad";
                ws.Cells[rowIniEmpresa, colIniMW].Value = "MW";

                using (var range = ws.Cells[rowIniEmpresa, colIniBloqueHorario, rowIniEmpresa, colIniMW])
                {
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Color.SetColor(Color.Black);
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Color.SetColor(Color.Black);
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Color.SetColor(Color.Black);
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(Color.Black);

                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    range.Style.Font.Color.SetColor(Color.White);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
                row = rowIniEmpresa;
                #endregion
                row += 1;
                #region cuerpo Consolidado

                var data = listaConsolidadoDemanda;
                var listaEmpresa = data.Select(y => new { y.Emprcodi, y.Empresa }).Distinct().ToList().OrderBy(c => c.Empresa);
                var listaEmprcodi = listaEmpresa.Select(x => x.Emprcodi).ToList();
                foreach (var empcodi in listaEmprcodi)
                {
                    //empresa
                    string nomEmpresa = data.Where(x => x.Emprcodi == empcodi).FirstOrDefault().Empresa;
                    int cantMWPorEmpresa = data.Where(x => x.Emprcodi == empcodi).Select(y => y.Equicodi).Distinct().Count();

                    ws.Cells[row, colIniEmpresa].Value = nomEmpresa;
                    ws.Cells[row, colIniEmpresa, row + cantMWPorEmpresa - 1, colIniEmpresa].Merge = true;
                    ws.Cells[row, colIniEmpresa, row + cantMWPorEmpresa - 1, colIniEmpresa].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[row, colIniEmpresa, row + cantMWPorEmpresa - 1, colIniEmpresa].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    //central
                    var listaCentral = data.Where(x => x.Emprcodi == empcodi).Select(y => new { idCentral = y.IdCentral, y.Central }).Distinct().ToList().OrderBy(c => c.Central);
                    var listaCentralcodi = listaCentral.Select(x => x.idCentral).ToList();
                    for (int u = 0; u < listaCentralcodi.Count; u++)
                    {
                        if (u != 0)
                        {
                            row++;
                        }

                        var centralcodi = listaCentralcodi[u];
                        var nomCentral = data.Where(x => x.Emprcodi == empcodi && x.IdCentral == centralcodi).FirstOrDefault().Central;
                        int cantMWPorCentral = data.Where(x => x.Emprcodi == empcodi && x.IdCentral == centralcodi).Select(y => y.Equicodi).Distinct().Count();

                        ws.Cells[row, colIniCentral].Value = nomCentral;
                        ws.Cells[row, colIniCentral, row + cantMWPorCentral - 1, colIniCentral].Merge = true;
                        ws.Cells[row, colIniCentral, row + cantMWPorCentral - 1, colIniCentral].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[row, colIniCentral, row + cantMWPorCentral - 1, colIniCentral].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        //unidad
                        var listaUnidad = data.Where(x => x.Emprcodi == empcodi && x.IdCentral == centralcodi).Select(y => new { y.Equicodi, y.GrupSSAA }).Distinct().ToList().OrderBy(c => c.GrupSSAA); ;
                        var listaUnidadcodi = listaUnidad.Select(x => x.Equicodi).ToList();
                        for (int y = 0; y < listaUnidadcodi.Count; y++)
                        {
                            if (y != 0)
                            {
                                row++;
                            }
                            var unidadcodi = listaUnidadcodi[y];
                            var unidad = data.Where(x => x.Emprcodi == empcodi && x.IdCentral == centralcodi && x.Equicodi == unidadcodi).FirstOrDefault();
                            var nomUnidad = unidad.GrupSSAA;
                            var totalUnidad = unidad.Total;


                            ws.Cells[row, colIniUnidad].Value = nomUnidad;
                            ws.Cells[row, colIniUnidad].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[row, colIniMW].Value = totalUnidad;
                            ws.Cells[row, colIniMW].Style.Numberformat.Format = "0.000";
                            ws.Cells[row, colIniMW].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        }
                    }
                    row++;
                }
                #endregion

                //ancho de columnas
                ws.Column(1).Width = 3;
                ws.Column(2).Width = 30;
                ws.Column(3).Width = 25;
                ws.Column(4).Width = 25;
                ws.Column(5).Width = 25;
                ws.Column(6).Width = 25;

                ws.Row(rowMW).Height = 30;

                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                ExcelPicture picture = ws.Drawings.AddPicture("Imagen", img);
                picture.From.Column = 1;
                picture.From.Row = 1;


                fileExcel = System.IO.Path.GetTempFileName();
                xlPackage.SaveAs(new FileInfo(fileExcel));
            }

            return fileExcel;
        }

        public string GenerarFileExcelReporteDiagramaCargaMaximaDemanda(string strMes, string titulo, string leyenda, MaximaDemandaDTO maximaDemanda, List<MaximaDemandaDTO> listaResumenDetalleDemanda, List<string> listaNormativa)
        {
            string fileExcel = string.Empty;

            using (ExcelPackage xlPackage = new ExcelPackage())
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Diagrama de Carga");
                ws.View.ShowGridLines = false;

                int col = 2;
                int row = 1;

                #region inicio
                int rowTitulo = 5;
                ws.Cells[rowTitulo, col].Value = "DIAGRAMA DE CARGA DEL DÍA DE MÁXIMA DEMANDA";
                ws.Cells[rowTitulo, col].Style.Font.Bold = true;
                ws.Cells[rowTitulo + 1, col].Value = strMes;

                row = 9;
                int rowPR = row;
                ws.Cells[row, col].Value = "CONFORME A LOS SIGUIENTES PROCEDIMIENTOS TÉCNICOS DEL COES:";
                ws.Cells[row, col].Style.Font.Bold = true;
                row++;
                ws.Cells[row, col].Value = "PR-N° 30: VALORIZACIÓN DE LAS TRANSFERENCIAS DE POTENCIA Y COMPENSACIONES AL SISTEMA PRINCIPAL Y SISTEMA GARANTIZADO DE TRANSMISIÓN (Vigente desde el 01 de junio de 2015)";
                ws.Cells[row, col].Style.Indent = 2;
                ws.Cells[row, col].Style.Font.Bold = true;
                row++;
                ws.Cells[row, col].Value = "PR-N° 43: INTERCAMBIOS INTERNACIONALES DE ELECTRICIDAD EN EL MARCO DE LA DECISIÓN 757 DE LA CAN";
                ws.Cells[row, col].Style.Indent = 2;
                ws.Cells[row, col].Style.Font.Bold = true;

                foreach (var reg in listaNormativa)
                {
                    row++;
                    ws.Cells[row, col].Value = reg;
                    ws.Cells[row, col].Style.Indent = 2;
                    ws.Cells[row, col].Style.Font.Bold = true;
                }
                #endregion

                row += 2;

                #region filtros
                int rowIniBloqueHorario = row;
                int rowIniOnlyFecha = rowIniBloqueHorario + 1;
                int rowIniOnlyHora = rowIniOnlyFecha + 1;

                int colIniBloqueHorario = col;
                int colIniOnlyFecha = colIniBloqueHorario;
                int colIniOnlyHora = colIniOnlyFecha;

                ws.Cells[rowIniBloqueHorario, colIniBloqueHorario].Value = "Máxima Demanda";
                ws.Cells[rowIniBloqueHorario, colIniBloqueHorario + 1].Value = maximaDemanda.Valor;

                ws.Cells[rowIniOnlyFecha, colIniOnlyFecha].Value = "Fecha";
                ws.Cells[rowIniOnlyFecha, colIniOnlyFecha + 1].Value = maximaDemanda.FechaOnlyDia;

                ws.Cells[rowIniOnlyHora, colIniOnlyHora].Value = "Hora";
                ws.Cells[rowIniOnlyHora, colIniOnlyHora + 1].Value = maximaDemanda.FechaOnlyHora;

                using (var range = ws.Cells[rowIniBloqueHorario, colIniBloqueHorario, rowIniOnlyHora, colIniBloqueHorario + 1])
                {
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Color.SetColor(Color.Black);
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Color.SetColor(Color.Black);
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Color.SetColor(Color.Black);
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(Color.Black);

                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }
                using (var range = ws.Cells[rowIniBloqueHorario, colIniBloqueHorario, rowIniOnlyHora, colIniBloqueHorario])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    range.Style.Font.Color.SetColor(Color.White);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                row = rowIniOnlyHora;
                #endregion
                row += 2;

                #region cabecera detalle
                int rowIniHora = row;

                int colIniHora = col;
                int colIniDemanda = colIniHora + 1;

                ws.Cells[rowIniHora, colIniHora].Value = "Hora";
                ws.Cells[rowIniHora, colIniDemanda].Value = "Demanda";

                using (var range = ws.Cells[rowIniHora, colIniHora, rowIniHora, colIniDemanda])
                {
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Color.SetColor(Color.Black);
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Color.SetColor(Color.Black);
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Color.SetColor(Color.Black);
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(Color.Black);

                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    range.Style.Font.Color.SetColor(Color.White);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
                row = rowIniHora;
                #endregion
                row++;
                #region cuerpo Resumen
                int rowDetIniOnlyHora = row;
                foreach (var res in listaResumenDetalleDemanda)
                {
                    ws.Cells[row, colIniHora].Value = res.FechaOnlyHora;
                    ws.Cells[row, colIniHora].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[row, colIniHora].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    ws.Cells[row, colIniDemanda].Value = res.Valor;
                    ws.Cells[row, colIniDemanda].Style.Numberformat.Format = "0.000";
                    ws.Cells[row, colIniDemanda].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    row++;
                }
                int rowDetFinOnlyHora = row;
                #endregion

                #region chart
                var chart1 = ws.Drawings.AddChart("Chart1", eChartType.Area);
                chart1.SetPosition(rowDetIniOnlyHora, 0, colIniDemanda + 3, 0);
                chart1.SetSize(1200, 600);
                chart1.Title.Text = titulo;
                chart1.YAxis.Title.Text = leyenda;
                chart1.Legend.Position = eLegendPosition.Bottom;

                var ran0 = ws.Cells[rowDetIniOnlyHora, colIniHora, rowDetFinOnlyHora, colIniHora];
                var ran1 = ws.Cells[rowDetIniOnlyHora, colIniDemanda, rowDetFinOnlyHora, colIniDemanda];

                //Fuente1
                var serie = (ExcelChartSerie)chart1.Series.Add(ran1, ran0);
                serie.Header = leyenda;
                #endregion

                //ancho de columnas
                ws.Column(1).Width = 3;
                ws.Column(2).Width = 18;
                ws.Column(3).Width = 25;

                //logo
                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                ExcelPicture picture = ws.Drawings.AddPicture("Imagen", img);
                picture.From.Column = 1;
                picture.From.Row = 1;


                fileExcel = System.IO.Path.GetTempFileName();
                xlPackage.SaveAs(new FileInfo(fileExcel));
            }

            return fileExcel;
        }

        public string GenerarFileExcelReporteRecursoEnergetico(string strMes, string titulo, string leyenda, int bloqueHorario, List<MaximaDemandaDTO> listaResumenDemanda, List<ConsolidadoEnvioDTO> listaConsolidadoRecursoEnergetico, List<string> listaNormativa)
        {
            string fileExcel = string.Empty;

            using (ExcelPackage xlPackage = new ExcelPackage())
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Recurso Energético");
                ws.View.ShowGridLines = false;

                int col = 2;
                int row = 1;

                #region inicio
                int rowTitulo = 2;
                ws.Cells[rowTitulo, col + 1].Value = "REPORTE DE MÁXIMA DEMANDA";
                ws.Cells[rowTitulo, col + 1].Style.Font.Bold = true;
                ws.Cells[rowTitulo + 1, col + 1].Value = strMes;

                row = 6;
                int rowPR = row;
                ws.Cells[row, col].Value = "CONFORME A LOS SIGUIENTES PROCEDIMIENTOS TÉCNICOS DEL COES:";
                ws.Cells[row, col].Style.Font.Bold = true;
                row++;
                ws.Cells[row, col].Value = "PR-N° 30: VALORIZACIÓN DE LAS TRANSFERENCIAS DE POTENCIA Y COMPENSACIONES AL SISTEMA PRINCIPAL Y SISTEMA GARANTIZADO DE TRANSMISIÓN (Vigente desde el 01 de junio de 2015)";
                ws.Cells[row, col].Style.Indent = 2;
                ws.Cells[row, col].Style.Font.Bold = true;
                row++;
                ws.Cells[row, col].Value = "PR-N° 43: INTERCAMBIOS INTERNACIONALES DE ELECTRICIDAD EN EL MARCO DE LA DECISIÓN 757 DE LA CAN";
                ws.Cells[row, col].Style.Indent = 2;
                ws.Cells[row, col].Style.Font.Bold = true;

                foreach (var reg in listaNormativa)
                {
                    row++;
                    ws.Cells[row, col].Value = reg;
                    ws.Cells[row, col].Style.Indent = 2;
                    ws.Cells[row, col].Style.Font.Bold = true;
                }
                #endregion

                row += 2;

                #region cabecera Resumen
                int rowIniBloqueHorario = row;
                int rowFinBloqueHorario = rowIniBloqueHorario + 1;
                int rowIniInterconexión = row;
                int rowMW = rowIniInterconexión + 1;

                int colIniBloqueHorario = col;
                int colIniFechaHora = colIniBloqueHorario + 1;
                int colIniInterconexión = colIniFechaHora + 1;
                int colFinInterconexión = colIniInterconexión + 1;
                int colIniPeruEcu = colIniFechaHora + 1;
                int colIniEcuPeru = colIniPeruEcu + 1;
                int colIniSein = colIniEcuPeru + 1;

                ws.Cells[rowIniBloqueHorario, colIniBloqueHorario].Value = "Bloque Horario";
                ws.Cells[rowIniBloqueHorario, colIniBloqueHorario, rowFinBloqueHorario, colIniBloqueHorario].Merge = true;

                ws.Cells[rowIniBloqueHorario, colIniFechaHora].Value = "Fecha Hora";
                ws.Cells[rowIniBloqueHorario, colIniFechaHora, rowFinBloqueHorario, colIniFechaHora].Merge = true;

                ws.Cells[rowIniInterconexión, colIniInterconexión].Value = "Interconexión";
                ws.Cells[rowIniInterconexión, colIniInterconexión, rowIniInterconexión, colFinInterconexión].Merge = true;

                ws.Cells[rowMW, colIniPeruEcu].Value = "PER-ECU Exportación \n MW";
                ws.Cells[rowMW, colIniPeruEcu].Style.WrapText = true;

                ws.Cells[rowMW, colIniEcuPeru].Value = "ECU-PER Importación \n MW";
                ws.Cells[rowMW, colIniEcuPeru].Style.WrapText = true;

                ws.Cells[rowIniBloqueHorario, colIniSein].Value = "SEIN \n MW";
                ws.Cells[rowIniBloqueHorario, colIniSein, rowFinBloqueHorario, colIniSein].Merge = true;
                ws.Cells[rowIniBloqueHorario, colIniSein, rowFinBloqueHorario, colIniSein].Style.WrapText = true;

                using (var range = ws.Cells[rowIniBloqueHorario, colIniBloqueHorario, rowFinBloqueHorario, colIniSein])
                {
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Color.SetColor(Color.Black);
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Color.SetColor(Color.Black);
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Color.SetColor(Color.Black);
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(Color.Black);

                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    range.Style.Font.Color.SetColor(Color.White);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                row = rowFinBloqueHorario;
                #endregion
                row += 1;
                #region cuerpo Resumen

                decimal maxImport = 0M;
                decimal maxExport = 0M;
                var tot = 0M;
                foreach (var res in listaResumenDemanda)
                {
                    var export = 0M;
                    var import = 0M;
                    if (res.ValorInter > 0)
                    {
                        export = res.ValorInter;
                    }
                    else
                    {
                        import = res.ValorInter * -1;
                    }

                    if (tot < res.Valor)
                    {
                        tot = res.Valor;
                        maxExport = export;
                        maxImport = import;
                    }

                    ws.Cells[row, colIniBloqueHorario].Value = res.TipoHorario;
                    ws.Cells[row, colIniBloqueHorario].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    ws.Cells[row, colIniFechaHora].Value = res.FechaHoraFull;
                    ws.Cells[row, colIniFechaHora].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[row, colIniFechaHora].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    ws.Cells[row, colIniPeruEcu].Value = export;
                    ws.Cells[row, colIniPeruEcu].Style.Numberformat.Format = "0.000";
                    ws.Cells[row, colIniPeruEcu].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    ws.Cells[row, colIniEcuPeru].Value = import;
                    ws.Cells[row, colIniEcuPeru].Style.Numberformat.Format = "0.000";
                    ws.Cells[row, colIniEcuPeru].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    ws.Cells[row, colIniSein].Value = res.Valor;
                    ws.Cells[row, colIniSein].Style.Numberformat.Format = "0.000";
                    ws.Cells[row, colIniSein].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    row++;
                }

                if (bloqueHorario == 0)
                {
                    if (listaResumenDemanda[0].ValorInter > 0)
                    {
                        maxExport = listaResumenDemanda[0].ValorInter;
                    }
                    else
                    {
                        maxImport = listaResumenDemanda[0].ValorInter * -1;
                    }
                }

                #endregion

                row += 2;

                #region cabecera Tipo Generacion
                int rowIniTipoGeneracion = row;

                int colIniTipoGenNomb = col;
                int colIniTipoGenMW = colIniTipoGenNomb + 1;
                int colIniTipoGenParticipacion = colIniTipoGenMW + 1;

                ws.Cells[rowIniTipoGeneracion, colIniTipoGenNomb].Value = "Tipo";
                ws.Cells[rowIniTipoGeneracion, colIniTipoGenMW].Value = "MW";
                ws.Cells[rowIniTipoGeneracion, colIniTipoGenParticipacion].Value = "PARTICIPACIÓN %";

                using (var range = ws.Cells[rowIniTipoGeneracion, colIniTipoGenNomb, rowIniTipoGeneracion, colIniTipoGenParticipacion])
                {
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Color.SetColor(Color.Black);
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Color.SetColor(Color.Black);
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Color.SetColor(Color.Black);
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(Color.Black);

                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    range.Style.Font.Color.SetColor(Color.White);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                row = rowIniTipoGeneracion;
                #endregion

                row++;
                #region cuerpo Tipo Generacion
                int rowIniGraTipoGen = row;
                var total = 0.0M;
                var porcentaje = 0.0M;
                var porcExpor = 0.0M;
                var porcImpor = 0.0M;
                List<string> listaColor = new List<string>();
                foreach (var item in listaConsolidadoRecursoEnergetico)
                {
                    listaColor.Add(item.Fenercolor);

                    total += item.Total;
                    porcentaje += item.Porcentaje;

                    ws.Cells[row, colIniTipoGenNomb].Value = item.Fenergnomb;
                    ws.Cells[row, colIniTipoGenNomb].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    ws.Cells[row, colIniTipoGenMW].Value = item.Total;
                    ws.Cells[row, colIniTipoGenMW].Style.Numberformat.Format = "0.000";
                    ws.Cells[row, colIniTipoGenMW].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    ws.Cells[row, colIniTipoGenParticipacion].Value = item.Porcentaje;
                    ws.Cells[row, colIniTipoGenParticipacion].Style.Numberformat.Format = "0.000";
                    ws.Cells[row, colIniTipoGenParticipacion].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    row++;
                }

                var totSeinConInter = total + maxImport - maxExport;
                if (totSeinConInter > 0)
                {
                    porcExpor = maxExport / totSeinConInter * 100;
                    porcImpor = maxImport / totSeinConInter * 100;
                }

                int rowFinGraTipoGen = row - 1;
                //total Perú
                ws.Cells[row, colIniTipoGenNomb].Value = "TOTAL PERÚ";
                ws.Cells[row, colIniTipoGenNomb].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells[row, colIniTipoGenMW].Value = total;
                ws.Cells[row, colIniTipoGenMW].Style.Numberformat.Format = "0.000";
                ws.Cells[row, colIniTipoGenMW].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells[row, colIniTipoGenParticipacion].Value = porcentaje;
                ws.Cells[row, colIniTipoGenParticipacion].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[row, colIniTipoGenParticipacion].Style.Numberformat.Format = "0.000";

                using (var range = ws.Cells[row, colIniTipoGenNomb, row, colIniTipoGenParticipacion])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#DCE6F2"));
                }

                row++;

                //interconexion
                ws.Cells[row, colIniTipoGenNomb].Value = "EXPORTACIÓN (PER-ECU)";
                ws.Cells[row, colIniTipoGenNomb].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells[row, colIniTipoGenMW].Value = maxExport;
                ws.Cells[row, colIniTipoGenMW].Style.Numberformat.Format = "0.000";
                ws.Cells[row, colIniTipoGenMW].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells[row, colIniTipoGenParticipacion].Value = porcExpor;
                ws.Cells[row, colIniTipoGenParticipacion].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[row, colIniTipoGenParticipacion].Style.Numberformat.Format = "0.000";

                row++;

                ws.Cells[row, colIniTipoGenNomb].Value = "IMPORTACIÓN (ECU-PER)";
                ws.Cells[row, colIniTipoGenNomb].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells[row, colIniTipoGenMW].Value = maxImport;
                ws.Cells[row, colIniTipoGenMW].Style.Numberformat.Format = "0.000";
                ws.Cells[row, colIniTipoGenMW].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells[row, colIniTipoGenParticipacion].Value = porcImpor;
                ws.Cells[row, colIniTipoGenParticipacion].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[row, colIniTipoGenParticipacion].Style.Numberformat.Format = "0.000";

                row++;

                //TOTAL

                ws.Cells[row, colIniTipoGenNomb].Value = "TOTAL";
                ws.Cells[row, colIniTipoGenNomb].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells[row, colIniTipoGenMW].Value = totSeinConInter;
                ws.Cells[row, colIniTipoGenMW].Style.Numberformat.Format = "0.000";
                ws.Cells[row, colIniTipoGenMW].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells[row, colIniTipoGenParticipacion].Value = "--";
                ws.Cells[row, colIniTipoGenParticipacion].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[row, colIniTipoGenParticipacion].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                using (var range = ws.Cells[row, colIniTipoGenNomb, row, colIniTipoGenParticipacion])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    range.Style.Font.Color.SetColor(Color.White);
                }
                #endregion

                #region chart
                var chart1 = ws.Drawings.AddChart("Chart1", eChartType.Pie) as ExcelPieChart;
                chart1.SetPosition(rowIniTipoGeneracion, 0, colIniTipoGenParticipacion + 2, 0);
                chart1.SetSize(800, 400);
                chart1.Title.Text = titulo;
                chart1.Legend.Position = eLegendPosition.Bottom;

                var ran0 = ws.Cells[rowIniGraTipoGen, colIniTipoGenNomb, rowFinGraTipoGen, colIniTipoGenNomb];
                var ran1 = ws.Cells[rowIniGraTipoGen, colIniTipoGenParticipacion, rowFinGraTipoGen, colIniTipoGenParticipacion];

                //Fuente1
                var serie = (ExcelChartSerie)chart1.Series.Add(ran1, ran0);
                serie.Header = leyenda;

                var pieSeries = (ExcelPieChartSerie)serie;
                //pieSeries.DataLabel.ShowValue = true;
                pieSeries.DataLabel.ShowPercent = true;
                pieSeries.DataLabel.ShowLeaderLines = true;
                //pieSeries.DataLabel.Separator = ";  ";
                pieSeries.DataLabel.Position = eLabelPosition.BestFit;

                ExcelDocumentMedicion.SetPieChartPointColors(chart1, listaColor);
                ExcelDocumentMedicion.SetPieChartDataLabelPercent(chart1);
                #endregion

                //ancho de columnas
                ws.Column(1).Width = 3;
                ws.Column(2).Width = 30;
                ws.Column(3).Width = 25;
                ws.Column(4).Width = 25;
                ws.Column(5).Width = 25;
                ws.Column(6).Width = 25;

                ws.Row(rowMW).Height = 30;

                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                ExcelPicture picture = ws.Drawings.AddPicture("Imagen", img);
                picture.From.Column = 1;
                picture.From.Row = 1;

                fileExcel = System.IO.Path.GetTempFileName();
                xlPackage.SaveAs(new FileInfo(fileExcel));
            }

            return fileExcel;
        }

        public string GenerarFileExcelReporteExcesoPotenReact(string nombreHoja, string nombreHojaDet, string titulo, string tituloDet, string strMes, List<ConsolidadoEnvioDTO> listaConsolidadoDemanda, List<MeMedicion96DTO> listaDataEA, List<MeMedicion96DTO> listaDataERC, List<MeMedicion96DTO> listaDataERI)
        {
            string fileExcel = string.Empty;
            DateTime fechaProceso = EPDate.GetFechaIniPeriodo(5, strMes, "", "", "");
            DateTime fechaInicial = fechaProceso.Date;
            DateTime fechaFinal = fechaProceso.AddMonths(1).AddDays(-1).Date;

            if (listaConsolidadoDemanda.Count > 0)
            {
                using (ExcelPackage xlPackage = new ExcelPackage())
                {
                    #region Exceso
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nombreHoja);
                    ws.View.ShowGridLines = false;

                    int col = 2;
                    int row = 1;

                    int rowTitulo = row + 1;
                    ws.Cells[rowTitulo, col + 1].Value = titulo;
                    ws.Cells[rowTitulo, col + 1].Style.Font.Bold = true;
                    ws.Cells[rowTitulo + 1, col + 1].Value = strMes;

                    row = rowTitulo + 3;
                    int rowIni = row;
                    int nfil = listaConsolidadoDemanda.Count;
                    int ncol = 4;
                    int xFil = row;
                    using (ExcelRange r = ws.Cells[xFil, 2, xFil, 5])
                    {
                        r.Style.Font.Color.SetColor(Color.White);
                        r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                        r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        r.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    }

                    ws.Cells[xFil, 2].Value = "EMPRESA";
                    ws.Cells[xFil, 3].Value = "MODO OPERACIÓN";
                    ws.Cells[xFil, 4].Value = "INDUCTIVA";
                    ws.Cells[xFil, 5].Value = "CAPACITIVA";

                    ws.Column(1).Width = 5;
                    ws.Column(2).Width = 35;
                    ws.Column(3).Width = 35;
                    ws.Column(4).Width = 20;
                    ws.Column(5).Width = 20;

                    //****************************************
                    var i = 1;
                    foreach (var reg in listaConsolidadoDemanda)
                    {
                        ws.Cells[xFil + i, 2].Value = reg.Empresa;
                        ws.Cells[xFil + i, 3].Value = reg.GrupSSAA;
                        ws.Cells[xFil + i, 4].Value = reg.TotalInductiva;
                        ws.Cells[xFil + i, 5].Value = reg.TotalCapacitiva;

                        i++;
                    }
                    ////////////// Formato de Celdas Valores
                    var border = ws.Cells[rowIni, col, rowIni + nfil, col + ncol - 1].Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture("Imagen", img);
                    picture.From.Column = 1;
                    picture.From.Row = 1;
                    #endregion

                    #region Reactiva
                    ws = xlPackage.Workbook.Worksheets.Add(nombreHojaDet);
                    ws.View.ShowGridLines = false;

                    col = 2;
                    row = 1;

                    rowTitulo = row + 1;
                    ws.Cells[rowTitulo, col + 1].Value = tituloDet;
                    ws.Cells[rowTitulo, col + 1].Style.Font.Bold = true;
                    ws.Cells[rowTitulo + 1, col + 1].Value = strMes;

                    row = rowTitulo + 3;

                    int rowIniEmpresa = row;
                    int rowIniModo = rowIniEmpresa + 1;
                    int colIniEmpresa = col;
                    int colFinEmpresa = col;
                    int colIniModo = col;
                    int colFinModo = col;
                    int rowIniEnergia = rowIniModo + 1;
                    int colIniEnergia = col;

                    ws.Cells[rowIniEmpresa, colIniEmpresa].Value = "Empresa";
                    ws.Cells[rowIniModo, colIniModo].Value = "Modo Operación";
                    ws.Cells[rowIniEnergia, colIniEnergia].Value = "Fecha / Hora";
                    colIniEmpresa++;
                    colIniModo++;
                    colIniEnergia++;

                    //empresas
                    var listaEmpresa = listaConsolidadoDemanda.Select(y => new { y.Emprcodi, y.Empresa }).Distinct().ToList().OrderBy(c => c.Empresa).ToList();
                    for (int j = 0; j < listaEmpresa.Count; j++)
                    {
                        var emprcodi = listaEmpresa[j].Emprcodi;
                        var empresa = listaEmpresa[j].Empresa;

                        var listaModo = listaConsolidadoDemanda.Where(x => x.Emprcodi == emprcodi).ToList().Select(y => new { y.IdGrupo, y.GrupSSAA }).Distinct().ToList().OrderBy(c => c.GrupSSAA).ToList();

                        colFinEmpresa = colIniEmpresa + listaModo.Count * 3;
                        ws.Cells[rowIniEmpresa, colIniEmpresa].Value = empresa;
                        ws.Cells[rowIniEmpresa, colIniEmpresa, rowIniEmpresa, colFinEmpresa - 1].Merge = true;
                        colIniEmpresa = colFinEmpresa;

                        //modo
                        for (int g = 0; g < listaModo.Count; g++)
                        {
                            var modo = listaModo[g].GrupSSAA;
                            colFinModo = colIniModo + 3;
                            ws.Cells[rowIniModo, colIniModo].Value = modo;
                            ws.Cells[rowIniModo, colIniModo, rowIniModo, colFinModo - 1].Merge = true;

                            //energia
                            ws.Cells[rowIniEnergia, colIniModo].Value = "EA";
                            ws.Cells[rowIniEnergia, colIniModo + 1].Value = "ER. IND";
                            ws.Cells[rowIniEnergia, colIniModo + 2].Value = "ER. CAP";

                            colIniModo = colFinModo;
                        }
                    }

                    using (ExcelRange r = ws.Cells[rowIniEmpresa, col, rowIniEnergia, colFinEmpresa - 1])
                    {
                        r.Style.Font.Color.SetColor(Color.White);
                        r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                        r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        r.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    }

                    //data
                    row = rowIniEnergia + 1;
                    int rowIniData = row;
                    int colIniHora = col;
                    int colEnergia = colIniHora + 1;
                    for (DateTime f = fechaInicial.Date; f <= fechaFinal.Date; f = f.AddDays(1))
                    {
                        for (int h = 1; h <= 96; h++)
                        {
                            var hora = f.AddMinutes(15 * h).ToString(ConstantesBase.FormatFechaFull);
                            colIniHora = col;
                            ws.Cells[row, colIniHora].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[row, colIniHora].Value = hora;
                            colIniHora++;

                            for (int j = 0; j < listaEmpresa.Count; j++)
                            {
                                var emprcodi = listaEmpresa[j].Emprcodi;
                                var listaModo = listaConsolidadoDemanda.Where(x => x.Emprcodi == emprcodi).ToList().Select(y => new { y.IdGrupo, y.GrupSSAA }).Distinct().ToList().OrderBy(c => c.GrupSSAA).ToList();
                                for (int g = 0; g < listaModo.Count; g++)
                                {
                                    var ea = listaDataEA.Where(y => y.Hophorini.Date == f && y.Grupocodi == listaModo[g].IdGrupo).FirstOrDefault();
                                    var eri = listaDataERI.Where(y => y.Hophorini.Date == f && y.Grupocodi == listaModo[g].IdGrupo).FirstOrDefault();
                                    var erc = listaDataERC.Where(z => z.Hophorini.Date == f && z.Grupocodi == listaModo[g].IdGrupo).FirstOrDefault();
                                    decimal? valorea = null, valorerc = null, valoreri = null;
                                    if (ea != null)
                                    {
                                        valorea = (decimal?)ea.GetType().GetProperty("H" + h).GetValue(ea, null);
                                    }
                                    if (eri != null)
                                    {
                                        valoreri = (decimal?)eri.GetType().GetProperty("H" + h).GetValue(eri, null);
                                    }
                                    if (erc != null)
                                    {
                                        valorerc = (decimal?)erc.GetType().GetProperty("H" + h).GetValue(erc, null);
                                    }

                                    ws.Cells[row, colIniHora].Style.Numberformat.Format = @"0.00000";
                                    ws.Cells[row, colIniHora].Value = valorea;
                                    colIniHora++;
                                    ws.Cells[row, colIniHora].Style.Numberformat.Format = @"0.00000";
                                    ws.Cells[row, colIniHora].Value = valoreri;
                                    colIniHora++;
                                    ws.Cells[row, colIniHora].Style.Numberformat.Format = @"0.00000";
                                    ws.Cells[row, colIniHora].Value = valorerc;
                                    colIniHora++;
                                }
                            }
                            row++;
                        }
                    }

                    border = ws.Cells[rowIniEmpresa, col, row - 1, colFinEmpresa - 1].Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    ws.Column(1).Width = 5;
                    ws.Column(2).Width = 35;
                    for (int m = 3; m < colFinEmpresa; m++)
                    {
                        ws.Column(m).Width = 10;
                    }

                    HttpWebRequest request2 = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response2 = (HttpWebResponse)request2.GetResponse();
                    System.Drawing.Image img2 = System.Drawing.Image.FromStream(response2.GetResponseStream());
                    ExcelPicture picture2 = ws.Drawings.AddPicture("Imagen", img);
                    picture2.From.Column = 1;
                    picture2.From.Row = 1;
                    #endregion

                    fileExcel = System.IO.Path.GetTempFileName();
                    xlPackage.SaveAs(new FileInfo(fileExcel));
                }
            }

            return fileExcel;
        }

        public int GetPosicionH(int hora, int min)
        {
            int cuarto = 0;
            if ((0 <= min) && (min <= 15))
            {
                cuarto = 1;
            }
            if ((15 < min) && (min <= 30))
            {
                cuarto = 2;
            }
            if ((30 < min) && (min <= 45))
            {
                cuarto = 3;
            }
            if ((45 < min) && (min < 60))
            {
                cuarto = 4;
            }

            return hora * 4 + cuarto;
        }

        /// <summary>
        /// Funcion que devuelve lista de empresas validadas
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="estadoValidacion"></param>
        /// <returns></returns>
        private string ListaEmpresaValidadas(DateTime fechaIni, string idEmpresa, int estadoValidacion)
        {
            if (ConstanteValidacion.EstadoValidado == estadoValidacion)
            {
                DateTime fechaPeriodo = new DateTime(fechaIni.Year, fechaIni.Month, 1);

                List<int> listaEmprcodi = new List<int>(), listaEmprcodiFinal = new List<int>();
                if (idEmpresa == ConstantesMedicion.IdEmpresaTodos.ToString())
                {
                    List<MeValidacionDTO> listaValidacionData = FactorySic.GetMeValidacionRepository().ListarValidacionXFormatoYFecha(ConstantesMedidores.IdFormatoCargaCentralPotActiva.ToString(), fechaPeriodo);
                    listaEmprcodiFinal = listaValidacionData.Where(x => x.Validestado == ConstanteValidacion.EstadoValidado).Select(x => x.Emprcodi).ToList();
                }
                else
                {
                    listaEmprcodi = idEmpresa.Split(',').Select(Int32.Parse).ToList();
                    foreach (var emprcodi in listaEmprcodi)
                    {
                        MeValidacionDTO validacionData = FactorySic.GetMeValidacionRepository().GetById(ConstantesMedidores.IdFormatoCargaCentralPotActiva, emprcodi, fechaPeriodo);
                        if (validacionData != null && validacionData.Validestado == ConstanteValidacion.EstadoValidado)
                        {
                            listaEmprcodiFinal.Add(emprcodi);
                        }
                    }
                }

                return string.Join(",", listaEmprcodiFinal);
            }

            return idEmpresa;
        }

        /// <summary>
        /// Obtener la máxima demanda por mes
        /// </summary>
        /// <param name="mes"></param>
        /// <param name="esPortal"></param>
        /// <returns></returns>
        public MaximaDemandaDTO GetDiaMaximaDemanda96XMes(DateTime mes, bool esPortal)
        {
            DateTime fechaProceso = new DateTime(mes.Year, mes.Month, 1);
            DateTime fechaIni = fechaProceso;
            DateTime fechaFin = fechaProceso.AddMonths(1).AddDays(-1);

            int estadoValidacion = esPortal ? ConstanteValidacion.EstadoValidado : ConstanteValidacion.EstadoTodos;
            int tipoCentral = ConstantesMedicion.IdTipogrupoCOES; //COES 
            int tipoGeneracion = ConstantesMedicion.IdTipoGeneracionTodos; //TODOS

            DateTime diaMaximaDemanda = this.GetDiaPeriodoDemanda96XFiltro(fechaIni, fechaFin, ConstantesRepMaxDemanda.TipoMDNormativa, tipoCentral, tipoGeneracion, ConstantesMedicion.IdEmpresaTodos, estadoValidacion);
            return this.GetResumenDiaMaximaDemanda96(diaMaximaDemanda, tipoCentral, tipoGeneracion, ConstantesMedicion.IdEmpresaTodos, estadoValidacion);
        }

        #endregion

        #region SIOSEIN2    

        public void ListaReportePotenciaXTipoRecursoTotalByLectcodi96(DateTime fcdesde, DateTime fchasta, bool esFuenteEnergiaCategorizada, string idtiporecurso, string tipogeneracion, out List<MeMedicion96DTO> listaTotal, out List<MeMedicion96DTO> listaTotalRER)
        {

            List<MeMedicion96DTO> lista96Final = new List<MeMedicion96DTO>(), lista96FinalRER = new List<MeMedicion96DTO>();
            List<MeMedicion96DTO> lista96Tmp = new List<MeMedicion96DTO>(), lista96TmpRER = new List<MeMedicion96DTO>();
            List<MeMedicion96DTO> listaReporte = new List<MeMedicion96DTO>(), listaReporteRER = new List<MeMedicion96DTO>();

            //La data cruzada con Hop que determina que TipoFuenteEnergia tuvo para tal TipoGeneracion
            List<MeMedicion96DTO> lista96Data = this.ListaDataMDGeneracionConsolidado(fcdesde, fchasta, ConstantesMedicion.IdTipogrupoCOES
                , tipogeneracion, ConstantesMedicion.IdEmpresaTodos.ToString(), ConstanteValidacion.EstadoTodos, ConstantesMedicion.IdTipoRecursoTodos.ToString(), true);

            if (esFuenteEnergiaCategorizada)
            {
                List<int> listaTipoGeneracion = idtiporecurso.Split(',').Select(x => int.Parse(x)).ToList();
                #region Data de Fuente de Energia con Categorias
                //HIDRO
                if (listaTipoGeneracion.Contains(ConstantesPR5ReportesServicio.FenergcodiAgua) || idtiporecurso.Equals(ConstantesAppServicio.ParametroDefecto))
                {
                    List<EqCategoriaDetDTO> listaHidro = FactorySic.GetEqCategoriaDetalleRepository().ListByCategoriaAndEstado(ConstantesPR5ReportesServicio.CategoriaRecursoAgua, ConstantesAppServicio.Activo);
                    foreach (var reg in listaHidro)
                    {
                        lista96Tmp.AddRange(this.ListarCategoriaDetalleToMedicion96(reg.Ctgdetcodi, ConstantesPR5ReportesServicio.FenergcodiAgua, lista96Data));
                    }
                    listaTipoGeneracion.Remove(ConstantesPR5ReportesServicio.FenergcodiAgua);
                }

                //TERMO
                if (listaTipoGeneracion.Contains(ConstantesPR5ReportesServicio.FenergcodiGas) || idtiporecurso.Equals(ConstantesAppServicio.ParametroDefecto))
                {
                    List<EqCategoriaDetDTO> listaGas = FactorySic.GetEqCategoriaDetalleRepository().ListByCategoriaAndEstado(ConstantesPR5ReportesServicio.CategoriaRecursoGas, ConstantesAppServicio.Activo);
                    foreach (var reg in listaGas)
                    {
                        lista96Tmp.AddRange(this.ListarCategoriaDetalleToMedicion96(reg.Ctgdetcodi, ConstantesPR5ReportesServicio.FenergcodiGas, lista96Data));
                    }
                    listaTipoGeneracion.Remove(ConstantesPR5ReportesServicio.FenergcodiGas);
                }

                //SOLAR, EOLICA
                if (listaTipoGeneracion.Count > 1)
                {
                    lista96Tmp.AddRange(lista96Data.Where(x => listaTipoGeneracion.Contains(x.Fenergcodi)).ToList());
                }


                //Generar data para reporte

                lista96Final = lista96Tmp
                    .GroupBy(t => new { t.Medifecha, t.Fenergcodi, t.Ctgdetcodi })
                    .Select(x => ObtenerObjetoMedicion96Dto(x.ToList()))
                    .ToList();

                listaReporte = this.ListarTipoRecursoEnergetico96();
                int orden = 1;
                foreach (var reg in listaReporte)
                {
                    var m96 = lista96Final.Find(x => x.Fenergcodi == reg.Fenergcodi && x.Ctgdetcodi == reg.Ctgdetcodi);
                    if (m96 != null)
                    {
                        decimal? valor = null;
                        List<decimal> listaH = new List<decimal>();

                        for (int h = 1; h <= 96; h++)
                        {
                            valor = (decimal?)m96.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m96, null);
                            reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).SetValue(reg, valor);
                            if (valor != null)
                            {
                                listaH.Add(valor.Value);
                            }
                        }

                        reg.Meditotal = listaH.Sum(x => x);
                    }

                    reg.Orden = orden;
                    orden++;
                }

                #endregion
            }
            else
            {
                #region Data Recursos Energeticos
                //Todo los recursos energeticos
                lista96Final = lista96Data
                    .GroupBy(t => new { t.Medifecha, t.Fenergcodi })
                    .Select(x => ObtenerObjetoMedicion96Dto(x.ToList())).ToList();

                var listaFenerg = FactorySic.GetSiFuenteenergiaRepository().List();
                foreach (var regFE in listaFenerg)
                {
                    var lista96FE = lista96Final.Where(x => x.Fenergcodi == regFE.Fenergcodi);
                    foreach (var m96 in lista96FE)
                    {
                        MeMedicion96DTO reg = new MeMedicion96DTO();
                        reg.Fenergcodi = regFE.Fenergcodi;
                        reg.Fenergnomb = regFE.Fenergnomb;
                        reg.Fenercolor = regFE.Fenergcolor;
                        reg.Orden = regFE.Fenergorden;

                        decimal? valor;
                        var listaH = new List<decimal>();

                        for (int h = 1; h <= 96; h++)
                        {
                            valor = (decimal?)m96.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m96, null);
                            reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).SetValue(reg, valor);
                            if (valor != null)
                            {
                                listaH.Add(valor.Value);
                            }
                        }

                        reg.Medifecha = m96.Medifecha;
                        reg.Meditotal = listaH.Sum(x => x);

                        listaReporte.Add(reg);
                    }
                }

                #endregion

                #region Data Recursos Energeticos RER
                List<PrGrupoDTO> listaRER = this.ListarAllGrupoRER(fcdesde);
                List<int> listagrupocodi = listaRER.Select(x => x.Grupocodi).Distinct().ToList();
                List<int> listagrupopadre = listaRER.Select(x => x.Grupopadre.GetValueOrDefault(0)).Distinct().ToList();
                listagrupocodi.AddRange(listagrupopadre);

                lista96Tmp = lista96Data.Where(x => listagrupocodi.Contains(x.Grupocodi)).ToList();

                //Todo los recursos energeticos
                lista96FinalRER = lista96Tmp.GroupBy(t => new { t.Medifecha, t.Fenergcodi })
                    .Select(x => ObtenerObjetoMedicion96Dto(x.ToList())).ToList();

                foreach (var regFE in listaFenerg)
                {
                    var lista48FE = lista96FinalRER.Where(x => x.Fenergcodi == regFE.Fenergcodi);
                    foreach (var m96 in lista48FE)
                    {
                        MeMedicion96DTO reg = new MeMedicion96DTO();
                        reg.Fenergcodi = regFE.Fenergcodi;


                        decimal? valor = null;
                        List<decimal> listaH = new List<decimal>();

                        for (int h = 1; h <= 96; h++)
                        {
                            valor = (decimal?)m96.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m96, null);
                            reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).SetValue(reg, valor);
                            if (valor != null)
                            {
                                listaH.Add(valor.Value);
                            }
                        }

                        reg.Medifecha = m96.Medifecha;
                        reg.Meditotal = listaH.Sum(x => x);

                        listaReporteRER.Add(reg);
                    }
                }

                #endregion
            }

            listaTotal = listaReporte;
            listaTotalRER = listaReporteRER;

        }

        private MeMedicion96DTO ObtenerObjetoMedicion96Dto(IEnumerable<MeMedicion96DTO> listaMedicion)
        {
            var medicion96 = new MeMedicion96DTO();
            if (!listaMedicion.Any()) return medicion96;

            medicion96.Medifecha = listaMedicion.First().Medifecha;
            medicion96.Fenergcodi = listaMedicion.First().Fenergcodi;
            medicion96.Ctgdetcodi = listaMedicion.First().Ctgdetcodi;
            var listaH = new List<decimal>();
            for (int Hx = 1; Hx <= 96; Hx++)
            {
                var HxVal = listaMedicion.Sum(x => (decimal?)x.GetType().GetProperty(ConstantesAppServicio.CaracterH + Hx).GetValue(x, null));
                medicion96.GetType().GetProperty(ConstantesAppServicio.CaracterH + Hx).SetValue(medicion96, HxVal);

                if (HxVal != null)
                    listaH.Add(HxVal.Value);
            }
            medicion96.Meditotal = listaH.Sum();
            return medicion96;

        }

        /// <summary>
        /// Todo los tipos de rr.ee.
        /// </summary>
        /// <returns></returns>
        public List<MeMedicion96DTO> ListarTipoRecursoEnergetico96()
        {
            List<MeMedicion96DTO> lista = new List<MeMedicion96DTO>();

            //Data
            List<SiFuenteenergiaDTO> listaTipoRecurso = FactorySic.GetSiFuenteenergiaRepository().List().Where(x => x.Fenergcodi > 0)
                .OrderBy(x => x.Tgenercodi).ThenBy(x => x.Fenergcodi).ToList();

            List<EqCategoriaDetDTO> listaHidro = FactorySic.GetEqCategoriaDetalleRepository()
                .ListByCategoriaAndEstado(ConstantesPR5ReportesServicio.CategoriaRecursoAgua, ConstantesAppServicio.Activo)
                .OrderBy(x => x.Ctgdetnomb).ToList();

            List<EqCategoriaDetDTO> listaGas = FactorySic.GetEqCategoriaDetalleRepository()
                .ListByCategoriaAndEstado(ConstantesPR5ReportesServicio.CategoriaRecursoGas, ConstantesAppServicio.Activo)
                .OrderBy(x => x.TotalEquipo).ThenBy(x => x.Ctgdetnomb).ToList();

            ///Generar el orden para el reporte
            EqCategoriaDetDTO ctg;
            SiFuenteenergiaDTO fteEnerg;

            //Recurso Agua
            fteEnerg = listaTipoRecurso.Find(x => x.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiAgua);

            ctg = listaHidro.Find(x => x.Ctgdetcodi == ConstantesPR5ReportesServicio.SubCategoriaRecursoAguaPasada);
            if (ctg != null)
            {
                lista.Add(this.GenerarObj96FromCategoria(fteEnerg, ctg));
            }

            ctg = listaHidro.Find(x => x.Ctgdetcodi == ConstantesPR5ReportesServicio.SubCategoriaRecursoAguaRegulacion);
            if (ctg != null)
            {
                lista.Add(this.GenerarObj96FromCategoria(fteEnerg, ctg));
            }

            //Recurso Gas
            fteEnerg = listaTipoRecurso.Find(x => x.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiGas);

            ctg = listaGas.Find(x => x.Ctgdetcodi == ConstantesPR5ReportesServicio.SubCategoriaRecursoGasNatural);
            if (ctg != null)
            {
                lista.Add(this.GenerarObj96FromCategoria(fteEnerg, ctg));
            }

            ctg = listaGas.Find(x => x.Ctgdetcodi == ConstantesPR5ReportesServicio.SubCategoriaRecursoGasMalacas);
            if (ctg != null)
            {
                lista.Add(this.GenerarObj96FromCategoria(fteEnerg, ctg));
            }

            ctg = listaGas.Find(x => x.Ctgdetcodi == ConstantesPR5ReportesServicio.SubCategoriaRecursoGasAguaytia);
            if (ctg != null)
            {
                lista.Add(this.GenerarObj96FromCategoria(fteEnerg, ctg));
            }

            ctg = listaGas.Find(x => x.Ctgdetcodi == ConstantesPR5ReportesServicio.SubCategoriaRecursoGasLaIsla);
            if (ctg != null)
            {
                lista.Add(this.GenerarObj96FromCategoria(fteEnerg, ctg));
            }

            //Demás fuentes de energias
            fteEnerg = listaTipoRecurso.Find(x => x.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiCarbon);
            if (fteEnerg != null)
            {
                lista.Add(this.GenerarObj96FromFuenteEnergia(fteEnerg));
            }

            fteEnerg = listaTipoRecurso.Find(x => x.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiDiesel);
            if (fteEnerg != null)
            {
                lista.Add(this.GenerarObj96FromFuenteEnergia(fteEnerg));
            }

            fteEnerg = listaTipoRecurso.Find(x => x.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiR500);
            if (fteEnerg != null)
            {
                lista.Add(this.GenerarObj96FromFuenteEnergia(fteEnerg));
            }

            fteEnerg = listaTipoRecurso.Find(x => x.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiR6);
            if (fteEnerg != null)
            {
                lista.Add(this.GenerarObj96FromFuenteEnergia(fteEnerg));
            }

            fteEnerg = listaTipoRecurso.Find(x => x.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiBagazo);
            if (fteEnerg != null)
            {
                lista.Add(this.GenerarObj96FromFuenteEnergia(fteEnerg));
            }

            fteEnerg = listaTipoRecurso.Find(x => x.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiBiogas);
            if (fteEnerg != null)
            {
                lista.Add(this.GenerarObj96FromFuenteEnergia(fteEnerg));
            }

            fteEnerg = listaTipoRecurso.Find(x => x.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiSolar);
            if (fteEnerg != null)
            {
                lista.Add(this.GenerarObj96FromFuenteEnergia(fteEnerg));
            }

            fteEnerg = listaTipoRecurso.Find(x => x.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiEolica);
            if (fteEnerg != null)
            {
                lista.Add(this.GenerarObj96FromFuenteEnergia(fteEnerg));
            }

            return lista;
        }


        /// <summary>
        /// GenerarObj48FromCategoria
        /// </summary>
        /// <param name="fteEnerg"></param>
        /// <param name="ctg"></param>
        /// <returns></returns>
        private MeMedicion96DTO GenerarObj96FromCategoria(SiFuenteenergiaDTO fteEnerg, EqCategoriaDetDTO ctg)
        {
            MeMedicion96DTO m = new MeMedicion96DTO();
            m.Fenergcodi = fteEnerg.Fenergcodi;
            m.Fenergnomb = fteEnerg.Fenergnomb;
            m.Fenercolor = fteEnerg.Fenergcolor;

            m.Ctgdetcodi = ctg.Ctgdetcodi;
            m.Ctgdetnomb = ctg.Ctgdetnomb;

            return m;
        }

        /// <summary>
        /// GenerarObj48FromFuenteEnergia
        /// </summary>
        /// <param name="fteEnerg"></param>
        /// <returns></returns>
        private MeMedicion96DTO GenerarObj96FromFuenteEnergia(SiFuenteenergiaDTO fteEnerg)
        {
            MeMedicion96DTO m = new MeMedicion96DTO();
            m.Fenergcodi = fteEnerg.Fenergcodi;
            m.Fenergnomb = fteEnerg.Fenergnomb;
            m.Fenercolor = fteEnerg.Fenergcolor;
            m.Ctgdetnomb = fteEnerg.Fenergnomb;

            return m;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctgdetcodi"></param>
        /// <param name="fenergcodi"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<MeMedicion96DTO> ListarCategoriaDetalleToMedicion96(int ctgdetcodi, int fenergcodi, List<MeMedicion96DTO> data)
        {
            List<MeMedicion96DTO> listaData = new List<MeMedicion96DTO>();

            List<EqCategoriaEquipoDTO> listaEquipo = FactorySic.GetEqCategoriaEquipoRepository().ListaClasificacionByCategoriaDetalle(ctgdetcodi);
            List<int> listaEquicodi = listaEquipo.Select(x => x.Equicodi).Where(x => x > 0).Distinct().ToList();
            List<MeMedicion96DTO> dataTmp = data.Where(x => x.Fenergcodi == fenergcodi).ToList();

            foreach (var reg in dataTmp)
            {
                if (listaEquicodi.Contains(reg.Equicodi))
                {
                    reg.Ctgdetcodi = ctgdetcodi;
                    listaData.Add(reg);
                }
            }

            return listaData;
        }

        /// <summary>
        /// Listar todos los grupos de generacion RER
        /// </summary>
        /// <returns></returns>
        public List<PrGrupoDTO> ListarAllGrupoRER(DateTime fechaPeriodo)
        {
            var lista = FactorySic.GetPrGrupoRepository().ListarAllGrupoRER(fechaPeriodo);
            foreach (var grupo in lista)
            {
                grupo.Emprnomb = grupo.Emprnomb != null ? grupo.Emprnomb.Trim() : string.Empty;
            }
            return lista;
        }

        #endregion;

        #region Métodos Tabla ABI_ENERGIAXFUENTENERGIA

        /// <summary>
        /// Inserta un registro de la tabla ABI_ENERGIAXFUENTENERGIA
        /// </summary>
        public void SaveAbiEnergiaxfuentenergia(AbiEnergiaxfuentenergiaDTO entity)
        {
            try
            {
                FactorySic.GetAbiEnergiaxfuentenergiaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla ABI_ENERGIAXFUENTENERGIA
        /// </summary>
        public void DeleteAbiEnergiaxfuentenergia(int mdfecodi)
        {
            try
            {
                FactorySic.GetAbiEnergiaxfuentenergiaRepository().Delete(mdfecodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla ABI_ENERGIAXFUENTENERGIA
        /// </summary>
        public void DeleteAbiEnergiaxfuentenergiaByMes(DateTime fechaPeriodo)
        {
            try
            {
                FactorySic.GetAbiEnergiaxfuentenergiaRepository().DeleteByMes(fechaPeriodo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ABI_ENERGIAXFUENTENERGIA
        /// </summary>
        public AbiEnergiaxfuentenergiaDTO GetByIdAbiEnergiaxfuentenergia(int mdfecodi)
        {
            return FactorySic.GetAbiEnergiaxfuentenergiaRepository().GetById(mdfecodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ABI_ENERGIAXFUENTENERGIA
        /// </summary>
        public List<AbiEnergiaxfuentenergiaDTO> ListAbiEnergiaxfuentenergias()
        {
            return FactorySic.GetAbiEnergiaxfuentenergiaRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla AbiEnergiaxfuentenergia
        /// </summary>
        public List<AbiEnergiaxfuentenergiaDTO> GetByCriteriaAbiEnergiaxfuentenergias()
        {
            return FactorySic.GetAbiEnergiaxfuentenergiaRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla WB_RESUMENMDDETALLE

        /// <summary>
        /// Inserta un registro de la tabla WB_RESUMENMDDETALLE
        /// </summary>
        public void SaveWbResumenmddetalle(WbResumenmddetalleDTO entity)
        {
            try
            {
                FactorySic.GetWbResumenmddetalleRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla WB_RESUMENMDDETALLE
        /// </summary>
        public void DeleteWbResumenmddetalle(int resmddcodi)
        {
            try
            {
                FactorySic.GetWbResumenmddetalleRepository().Delete(resmddcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla WB_RESUMENMDDETALLE
        /// </summary>
        public void DeleteWbResumenmddetalleByMes(DateTime fechaPeriodo)
        {
            try
            {
                FactorySic.GetWbResumenmddetalleRepository().DeleteByMes(fechaPeriodo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla WB_RESUMENMDDETALLE
        /// </summary>
        public WbResumenmddetalleDTO GetByIdWbResumenmddetalle(int resmddcodi)
        {
            return FactorySic.GetWbResumenmddetalleRepository().GetById(resmddcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla WB_RESUMENMDDETALLE
        /// </summary>
        public List<WbResumenmddetalleDTO> ListWbResumenmddetalles()
        {
            return FactorySic.GetWbResumenmddetalleRepository().List();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla WB_RESUMENMDDETALLE
        /// </summary>
        public List<WbResumenmddetalleDTO> ListWbResumenmddetalle(int rsmcodi)
        {
            return FactorySic.GetWbResumenmddetalleRepository().GetByIdMd(rsmcodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla WbResumenmddetalle
        /// </summary>
        public List<WbResumenmddetalleDTO> GetByCriteriaWbResumenmddetalles()
        {
            return FactorySic.GetWbResumenmddetalleRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla WB_RESUMENMD

        /// <summary>
        /// Inserta un registro de la tabla WB_RESUMENMD
        /// </summary>
        public int SaveWbResumenmd(WbResumenmdDTO entity)
        {
            try
            {
                return FactorySic.GetWbResumenmdRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla WB_RESUMENMD
        /// </summary>
        public void DeleteWbResumenmd(int resmdcodi)
        {
            try
            {
                FactorySic.GetWbResumenmdRepository().Delete(resmdcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla WB_RESUMENMD
        /// </summary>
        public void DeleteWbResumenmdByMes(DateTime fechaPeriodo)
        {
            try
            {
                FactorySic.GetWbResumenmdRepository().DeleteByMes(fechaPeriodo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla WB_RESUMENMD
        /// </summary>
        public WbResumenmdDTO GetByIdWbResumenmd(int resmdcodi)
        {
            return FactorySic.GetWbResumenmdRepository().GetById(resmdcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla WB_RESUMENMD
        /// </summary>
        public List<WbResumenmdDTO> ListWbResumenmds()
        {
            return FactorySic.GetWbResumenmdRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla WbResumenmd
        /// </summary>
        public List<WbResumenmdDTO> GetWbResumenmdByCriteria(DateTime fechaInicio, DateTime fechaFin)
        {
            return FactorySic.GetWbResumenmdRepository().GetByCriteria(fechaInicio, fechaFin);
        }

        /// <summary>
        /// Retornar máxima demanda
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public WbResumenmdDTO GetWbResumenmdMaxDemanda(DateTime fechaInicio, DateTime fechaFin)
        {
            return FactorySic.GetWbResumenmdRepository().GetMaxMdvalor(fechaInicio, fechaInicio);
        }

        #endregion

        #region Métodos Tabla ABI_FACTORPLANTA

        /// <summary>
        /// Inserta un registro de la tabla ABI_FACTORPLANTA
        /// </summary>
        public void SaveAbiFactorplanta(AbiFactorplantaDTO entity)
        {
            try
            {
                FactorySic.GetAbiFactorplantaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ABI_FACTORPLANTA
        /// </summary>
        public void UpdateAbiFactorplanta(AbiFactorplantaDTO entity)
        {
            try
            {
                FactorySic.GetAbiFactorplantaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla ABI_FACTORPLANTA
        /// </summary>
        public void DeleteAbiFactorplanta(int fpcodi)
        {
            try
            {
                FactorySic.GetAbiFactorplantaRepository().Delete(fpcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina registros de la tabla ABI_FACTORPLANTA por mes
        /// </summary>
        public void DeleteAbiFactorplantaByMes(DateTime fechaPeriodo)
        {
            try
            {
                FactorySic.GetAbiFactorplantaRepository().DeleteByMes(fechaPeriodo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ABI_FACTORPLANTA
        /// </summary>
        public AbiFactorplantaDTO GetByIdAbiFactorplanta(int fpcodi)
        {
            return FactorySic.GetAbiFactorplantaRepository().GetById(fpcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ABI_FACTORPLANTA
        /// </summary>
        public List<AbiFactorplantaDTO> ListAbiFactorplantas()
        {
            return FactorySic.GetAbiFactorplantaRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla AbiFactorplanta
        /// </summary>
        public List<AbiFactorplantaDTO> GetByCriteriaAbiFactorplantas()
        {
            return FactorySic.GetAbiFactorplantaRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla ABI_PRODGENERACION

        /// <summary>
        /// Inserta un registro de la tabla ABI_PRODGENERACION
        /// </summary>
        public void SaveAbiProdgeneracion(AbiProdgeneracionDTO entity, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                FactorySic.GetAbiProdgeneracionRepository().Save(entity, conn, tran);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ABI_PRODGENERACION
        /// </summary>
        public void UpdateAbiProdgeneracion(AbiProdgeneracionDTO entity)
        {
            try
            {
                FactorySic.GetAbiProdgeneracionRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla ABI_PRODGENERACION
        /// </summary>
        public void DeleteAbiProdgeneracionByRango(DateTime fechaIni, DateTime fechaFin, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                FactorySic.GetAbiProdgeneracionRepository().DeleteByRango(fechaIni, fechaFin, conn, tran);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Métodos Tabla ABI_POTEFEC

        /// <summary>
        /// Inserta un registro de la tabla ABI_POTEFEC
        /// </summary>
        public void SaveAbiPotefec(AbiPotefecDTO entity)
        {
            try
            {
                FactorySic.GetAbiPotefecRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ABI_POTEFEC
        /// </summary>
        public void UpdateAbiPotefec(AbiPotefecDTO entity)
        {
            try
            {
                FactorySic.GetAbiPotefecRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla ABI_POTEFEC
        /// </summary>
        public void DeleteAbiPotefec(int pefeccodi)
        {
            try
            {
                FactorySic.GetAbiPotefecRepository().Delete(pefeccodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina registros de la tabla ABI_POTEFEC por mes
        /// </summary>
        public void DeleteAbiPotefecByMes(DateTime fechaPeriodo)
        {
            try
            {
                FactorySic.GetAbiPotefecRepository().DeleteByMes(fechaPeriodo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ABI_POTEFEC
        /// </summary>
        public AbiPotefecDTO GetByIdAbiPotefec(int pefeccodi)
        {
            return FactorySic.GetAbiPotefecRepository().GetById(pefeccodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ABI_POTEFEC
        /// </summary>
        public List<AbiPotefecDTO> ListAbiPotefecs()
        {
            return FactorySic.GetAbiPotefecRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla AbiPotefec
        /// </summary>
        public List<AbiPotefecDTO> GetByCriteriaAbiPotefecs()
        {
            return FactorySic.GetAbiPotefecRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla ABI_MEDIDORES_RESUMEN

        /// <summary>
        /// Inserta un registro de la tabla ABI_MEDIDORES_RESUMEN
        /// </summary>
        public void SaveAbiMedidoresResumen(AbiMedidoresResumenDTO entity, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                FactorySic.GetAbiMedidoresResumenRepository().Save(entity, conn, tran);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ABI_MEDIDORES_RESUMEN
        /// </summary>
        public void UpdateAbiMedidoresResumen(AbiMedidoresResumenDTO entity)
        {
            try
            {
                FactorySic.GetAbiMedidoresResumenRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla ABI_MEDIDORES_RESUMEN
        /// </summary>
        public void DeleteAbiMedidoresResumenByRango(DateTime fechaIni, DateTime fechaFin, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                FactorySic.GetAbiMedidoresResumenRepository().DeleteByRango(fechaIni, fechaFin, conn, tran);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Estructuras para almacenamiento de información para los aplicativos BI de Producción de energía y Máxima Demanda

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fechaPeriodoIni"></param>
        /// <param name="fechaPeriodoFin"></param>
        public void GuardarEstructurasMaximaDemanda(DateTime fechaPeriodoIni, DateTime fechaPeriodoFin, string usuarioActualizacion)
        {
            DateTime fechaActualizacion = DateTime.Now;
            int lectcodi = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["IdLecturaMedidorGeneracion"]);

            //Obtener data de la maxima demanda
            DateTime fechaInicial = new DateTime(fechaPeriodoIni.Year, fechaPeriodoIni.Month, 1).Date;
            DateTime fechaFinal = new DateTime(fechaPeriodoFin.Year, fechaPeriodoFin.Month, 1).AddMonths(1).AddDays(-1);

            //Data de Medicion96
            List<MeMedicion96DTO> listAllSinCruceHO = this.ListaDataMDGeneracionConsolidado(fechaInicial, fechaFinal, ConstantesMedicion.IdTipogrupoCOES, ConstantesMedicion.IdTipoGeneracionTodos.ToString(), ConstantesMedicion.IdEmpresaTodos.ToString(), ConstanteValidacion.EstadoTodos, ConstantesMedicion.IdTipoRecursoTodos.ToString(), false);
            List<MeMedicion96DTO> listAllConCruceHO = this.ListaDataMDGeneracionConsolidado(fechaInicial, fechaFinal, ConstantesMedicion.IdTipogrupoCOES, ConstantesMedicion.IdTipoGeneracionTodos.ToString(), ConstantesMedicion.IdEmpresaTodos.ToString(), ConstanteValidacion.EstadoTodos, ConstantesMedicion.IdTipoRecursoTodos.ToString(), false);

            //Data Generación
            List<MeMedicion96DTO> listaAllDemandaGenSinCruceHO = this.ListaDataMDGeneracionFromConsolidado(fechaInicial, fechaFinal, listAllSinCruceHO);
            List<MeMedicion96DTO> listaAllDemandaGenConCruceHO = this.ListaDataMDGeneracionFromConsolidado(fechaInicial, fechaFinal, listAllSinCruceHO);

            //Data Interconexion
            List<MeMedicion96DTO> listaAllInterconexion = this.ListaDataMDInterconexion96(fechaInicial, fechaFinal); //para maxima demanda
            List<MeMedicion96DTO> listaAllInterconexionHistorico = this.serInterconexion.ObtenerDataHistoricaInterconexionByMedidor(1, ConstantesInterconexiones.IdMedidorConsolidado, fechaInicial, fechaFinal, true);

            List<DateTime> listaMes = new List<DateTime>();
            for (var mes = fechaPeriodoIni.Date; mes.Date <= fechaPeriodoFin.Date; mes = mes.AddMonths(1))
                listaMes.Add(mes);

            foreach (var mes in listaMes)
            {
                DateTime fechaIniMes = mes;
                DateTime fechaFinMes = mes.AddMonths(1).AddDays(-1);

                List<MeMedicion96DTO> listConCruceHO = listAllConCruceHO.Where(x => x.Medifecha >= fechaIniMes && x.Medifecha <= fechaFinMes).ToList();
                List<MeMedicion96DTO> listaDemandaGenConCruceHO = listaAllDemandaGenConCruceHO.Where(x => x.Medifecha >= fechaIniMes && x.Medifecha <= fechaFinMes).ToList();
                List<MeMedicion96DTO> listaDemandaGenSinCruceHO = listaAllDemandaGenSinCruceHO.Where(x => x.Medifecha >= fechaIniMes && x.Medifecha <= fechaFinMes).ToList();
                List<MeMedicion96DTO> listaInterconexion = listaAllInterconexion.Where(x => x.Medifecha >= fechaIniMes && x.Medifecha <= fechaFinMes).ToList();
                List<MeMedicion96DTO> listaInterconexionHistorico = listaAllInterconexionHistorico.Where(x => x.Medifecha >= fechaIniMes && x.Medifecha <= fechaFinMes).ToList();
                List<MeMedicion96DTO> listDemanda = this.ListaDataMDTotalSEIN(listaDemandaGenSinCruceHO, listaInterconexion);

                this.DeleteWbResumenmddetalleByMes(fechaIniMes);
                this.DeleteWbResumenmdByMes(fechaIniMes);
                this.DeleteAbiEnergiaxfuentenergiaByMes(fechaIniMes);
                FactorySic.GetMeMedicion96Repository().Delete(ConstantesFormatoMedicion.IdExportacionMW, ConstantesAppServicio.TipoinfocodiMW, fechaIniMes, fechaFinMes, lectcodi);
                FactorySic.GetMeMedicion96Repository().Delete(ConstantesFormatoMedicion.IdImportacionMW, ConstantesAppServicio.TipoinfocodiMW, fechaIniMes, fechaFinMes, lectcodi);

                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                /// 1. Almacenamiento y obtención de los valores de las mediciones cada 15 minutos del día de máxima demanda por mes de las empresas integrantes del COES

                MedicionReporteDTO umbrales = this.ObtenerParametros(fechaIniMes, listDemanda, listaInterconexion);
                //Obtener día MD
                DateTime fechaMD = umbrales.FechaMaximaDemanda;

                //Detalle del día de la MD
                MeMedicion96DTO detalleMD = listDemanda.Find(x => x.Medifecha == fechaMD.Date);

                WbResumenmdDTO regResumenMD = new WbResumenmdDTO();
                regResumenMD.Resmdfecha = umbrales.MaximaDemandaHora;
                regResumenMD.Resmdmes = new DateTime(fechaMD.Year, fechaMD.Month, 1).Date;
                regResumenMD.Resmdvalor = umbrales.MaximaDemanda;
                regResumenMD.Lastuser = usuarioActualizacion;
                regResumenMD.Lastdate = fechaActualizacion;

                if (detalleMD != null)
                {
                    for (int i = 1; i <= 96; i++)
                    {
                        decimal valH = ((decimal?)detalleMD.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(detalleMD, null)).GetValueOrDefault(0);
                        regResumenMD.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).SetValue(regResumenMD, valH);
                    }
                }

                int codigoMD = this.SaveWbResumenmd(regResumenMD);

                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                /// 2. Almacenamiento y obtención de la producción de energía por tipo de recurso energético por día (Gas, Hidráulica, Carbón, Diesel, Residual, Eólico, Solar, Bagazo y Biogás)
                List<AbiEnergiaxfuentenergiaDTO> listaFE = (from t in listConCruceHO
                                                            group t by new { t.Medifecha, t.Fenergcodi, t.Fenergnomb }
                                                              into destino
                                                            select new AbiEnergiaxfuentenergiaDTO()
                                                            {
                                                                Mdfefecha = destino.Key.Medifecha.Value,
                                                                Mdfevalor = destino.Sum(t => t.Meditotal ?? 0),
                                                                Mdfemes = fechaIniMes,
                                                                Fenergcodi = destino.Key.Fenergcodi,
                                                                Mdfeusumodificacion = usuarioActualizacion,
                                                                Mdfefecmodificacion = fechaActualizacion
                                                            }).OrderBy(x => x.Mdfefecha).ThenBy(x => x.Fenergcodi).ToList();

                foreach (var regFE in listaFE)
                {
                    this.SaveAbiEnergiaxfuentenergia(regFE);
                }

                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////  
                /// 3. Almacenamiento y obtención por punto de medición por tipo de recurso energético en la fecha y hora de la máxima demanda mensual

                int hMD = umbrales.HoraMaximaDemanda;
                List<WbResumenmddetalleDTO> listaDetalleResumenMD = new List<WbResumenmddetalleDTO>();

                if (hMD > 0)
                {
                    List<MeMedicion96DTO> listM96XPtoYFenerg = listConCruceHO.Where(x => x.Medifecha.Value.Date == fechaMD.Date).ToList();

                    List<int> listaPtomedicodi = listM96XPtoYFenerg.Select(x => x.Ptomedicodi).Distinct().OrderBy(x => x).ToList();

                    foreach (var ptomedicodi in listaPtomedicodi)
                    {
                        List<MeMedicion96DTO> lXPto = listM96XPtoYFenerg.Where(x => x.Ptomedicodi == ptomedicodi).ToList();

                        MeMedicion96DTO pto = lXPto.First();
                        decimal valorHPto = 0;
                        foreach (var regPto in lXPto)
                        {
                            decimal valH = ((decimal?)regPto.GetType().GetProperty(ConstantesAppServicio.CaracterH + hMD).GetValue(regPto, null)).GetValueOrDefault(0);
                            if (valH > valorHPto)
                            {
                                valorHPto = valH;
                                pto = regPto;
                            }
                        }

                        WbResumenmddetalleDTO regDetalle = new WbResumenmddetalleDTO();
                        regDetalle.Resmdcodi = codigoMD;
                        regDetalle.Ptomedicodi = ptomedicodi;
                        regDetalle.Fenergcodi = pto.Fenergcodi;
                        regDetalle.Resmddfecha = umbrales.MaximaDemandaHora;
                        regDetalle.Resmddvalor = valorHPto;
                        regDetalle.Resmddmes = fechaIniMes;
                        regDetalle.Resmddtipogenerrer = pto.Tipogenerrer != null ? pto.Tipogenerrer : ConstantesAppServicio.NO;
                        regDetalle.Resmddusumodificacion = usuarioActualizacion;
                        regDetalle.Resmddfecmodificacion = fechaActualizacion;

                        listaDetalleResumenMD.Add(regDetalle);
                    }
                }

                foreach (var regDRMD in listaDetalleResumenMD)
                {
                    this.SaveWbResumenmddetalle(regDRMD);
                }

                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                /// 4. Almacenamiento y obtención por puntos de medición para los intercambios internacionales (Importación y Exportación) cada quince minutos de forma diaria

                List<MeMedicion96DTO> listaNewInter = new List<MeMedicion96DTO>();

                for (DateTime day = fechaIniMes.Date; day <= fechaFinMes.Date; day = day.AddDays(1))
                {
                    var listaDay = listaInterconexionHistorico.Where(x => x.Medifecha.Value.Date == day).ToList();
                    var listaTptomedicodi = listaDay.GroupBy(x => x.Tipoptomedicodi).Select(x => x.Key).ToList();

                    var regExpMwh = listaDay.Where(x => x.Tipoptomedicodi == ConstantesInterconexiones.IdTipoPtomedicodiExportacionMwh).FirstOrDefault();
                    var regImpMwh = listaDay.Where(x => x.Tipoptomedicodi == ConstantesInterconexiones.IdTipoPtomedicodiImportacionMwh).FirstOrDefault();

                    if (regExpMwh != null && regImpMwh != null)
                    {
                        MeMedicion96DTO regExp = new MeMedicion96DTO();
                        regExp.Ptomedicodi = ConstantesFormatoMedicion.IdExportacionMW;
                        regExp.Tipoinfocodi = ConstantesAppServicio.TipoinfocodiMW;
                        regExp.Lectcodi = ConstantesFormatoMedicion.IdlecturaInterconexion;
                        regExp.Medifecha = day;
                        regExp.Meditotal = 0.0m;

                        MeMedicion96DTO regImp = new MeMedicion96DTO();
                        regImp.Ptomedicodi = ConstantesFormatoMedicion.IdImportacionMW;
                        regImp.Tipoinfocodi = ConstantesAppServicio.TipoinfocodiMW;
                        regImp.Lectcodi = ConstantesFormatoMedicion.IdlecturaInterconexion;
                        regImp.Medifecha = day;
                        regImp.Meditotal = 0.0m;

                        for (int i = 1; i <= 96; i++)
                        {
                            decimal valorEpxMWh = ((decimal?)regExpMwh.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(regExpMwh, null)).GetValueOrDefault(0) * 4;
                            decimal valorImpMWh = ((decimal?)regImpMwh.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(regImpMwh, null)).GetValueOrDefault(0) * 4;

                            regExp.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i).ToString()).SetValue(regExp, valorEpxMWh);
                            regImp.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i).ToString()).SetValue(regImp, valorImpMWh);
                        }

                        listaNewInter.Add(regExp);
                        listaNewInter.Add(regImp);
                    }
                }

                List<decimal> listaH;
                decimal total, valorH;
                foreach (var reg in listaNewInter)
                {
                    listaH = new List<decimal>();
                    total = 0;
                    for (int h = 1; h <= 96; h++)
                    {
                        valorH = ((decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg, null)).GetValueOrDefault(0);
                        listaH.Add(valorH);
                    }

                    total = listaH.Sum(x => x);

                    reg.Meditotal = total;
                }

                foreach (var regInt in listaNewInter)
                {
                    FactorySic.GetMeMedicion96Repository().Save(regInt);
                }
            }
        }

        /// <summary>
        /// Guardar Estructuras Produccion Generacion
        /// </summary>
        /// <param name="fechaPeriodoIni"></param>
        /// <param name="fechaPeriodoFin"></param>
        /// <param name="usuarioActualizacion"></param>
        public void GuardarEstructurasProduccionGeneracionYResumen(DateTime fechaPeriodoIni, DateTime fechaPeriodoFin, string usuarioActualizacion)
        {
            DateTime fechaActualizacion = DateTime.Now;

            //Obtener fechas
            DateTime fechaInicial = new DateTime(fechaPeriodoIni.Year, fechaPeriodoIni.Month, 1).Date;
            DateTime fechaFinal = new DateTime(fechaPeriodoFin.Year, fechaPeriodoFin.Month, 1).AddMonths(1).AddDays(-1);

            //Data de Medicion96 (tiene cruce con horas de operación)
            List<MeMedicion96DTO> listaDemandaGenSEIN96 = this.ListaDataMDGeneracionConsolidado(fechaInicial, fechaFinal, ConstantesMedicion.IdTipogrupoTodos, ConstantesMedicion.IdTipoGeneracionTodos.ToString(), ConstantesMedicion.IdEmpresaTodos.ToString(), ConstanteValidacion.EstadoTodos, ConstantesMedicion.IdTipoRecursoTodos.ToString(), true);
            List<MeMedicion96DTO> listaDemandaGenCOES96 = listaDemandaGenSEIN96.Where(x => x.Grupointegrante == ConstantesAppServicio.SI).ToList();

            List<MeMedicion96DTO> listaDemandaGen96XDia = ListaDataMDGeneracionFromConsolidado(fechaInicial, fechaFinal, listaDemandaGenCOES96);

            //Data Interconexion
            ListaFlujo15minInterconexion96(fechaInicial, fechaFinal, out List<MeMedicion96DTO> listaInterconexion96,
                        out List<MeMedicion96DTO> listaTotalExp, out List<MeMedicion96DTO> listaTotalImp);

            //Data Total (incluyendo interconexion)
            List<MeMedicion96DTO> listaMedicionTotal96 = this.ListaDataMDTotalSEIN(listaDemandaGen96XDia, listaInterconexion96);

            //Parametros para calculo de MD
            List<SiParametroValorDTO> listaRangoNormaHP = servParametro.ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroRangoPeriodoHP);
            List<SiParametroValorDTO> listaBloqueHorario = servParametro.ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroHPPotenciaActiva);

            //Obtener detalle de la generación de las unidades por día
            List<AbiProdgeneracionDTO> listaProdgen = listaDemandaGenSEIN96.GroupBy(t => new
            {
                t.Medifecha,
                t.Fenergcodi,
                t.Tgenercodi,
                t.Equicodi,
                t.Equipadre,
                t.Emprcodi,
                t.Grupointegrante,
                t.Tipogenerrer
            })
                                                       .Select(destino => new AbiProdgeneracionDTO()
                                                       {
                                                           Pgenfecha = destino.Key.Medifecha.Value,
                                                           Pgenvalor = destino.Sum(t => t.Meditotal ?? 0),
                                                           Fenergcodi = destino.Key.Fenergcodi,
                                                           Tgenercodi = destino.Key.Tgenercodi,
                                                           Equicodi = destino.Key.Equicodi,
                                                           Equipadre = destino.Key.Equipadre,
                                                           Emprcodi = destino.Key.Emprcodi,
                                                           Grupocodi = destino.First().Grupocodi,
                                                           Pgenintegrante = destino.Key.Grupointegrante,
                                                           Pgentipogenerrer = destino.Key.Tipogenerrer,
                                                           Pgenusumodificacion = usuarioActualizacion,
                                                           Pgenfecmodificacion = fechaActualizacion
                                                       }).OrderBy(x => x.Pgenfecha).ThenBy(x => x.Equicodi).ThenBy(x => x.Fenergcodi).ToList();

            //resumen de datos de centrales integrantes COES
            List<AbiMedidoresResumenDTO> listaResumen = listaDemandaGenCOES96.GroupBy(x => x.Medifecha.Value)
                                                      .Select(x => new AbiMedidoresResumenDTO()
                                                      {
                                                          Mregenfecha = x.Key,
                                                          Mregentotalsein = x.Sum(y => y.Meditotal ?? 0)
                                                      }).ToList();
            //Objetos
            foreach (var regDia in listaResumen)
            {
                var listaGenXDia = listaDemandaGenCOES96.Where(x => x.Medifecha == regDia.Mregenfecha.Date).ToList();

                //Total de potencia en el día
                regDia.Mregentotalexp = listaTotalExp.Find(x => x.Medifecha == regDia.Mregenfecha.Date).Meditotal;
                regDia.Mregentotalimp = listaTotalImp.Find(x => x.Medifecha == regDia.Mregenfecha.Date).Meditotal;
                regDia.Mregentotalsein += (regDia.Mregentotalimp.GetValueOrDefault(0) - regDia.Mregentotalexp.GetValueOrDefault(0));

                //Máxima demanda del SEIN
                this.GetDiaMaximaDemandaFromDataMD96(regDia.Mregenfecha.Date, regDia.Mregenfecha.Date, ConstantesRepMaxDemanda.TipoMDNormativa, listaMedicionTotal96, listaRangoNormaHP, listaBloqueHorario,
                                                                    out DateTime fechaHoraMDSein, out DateTime fechaDia, out int hMax);

                regDia.Mregenmdhidro = GetValorH(fechaHoraMDSein, listaGenXDia.Where(x => x.Tgenercodi == ConstantesPR5ReportesServicio.TgenercodiHidro).ToList());
                regDia.Mregenmdtermo = GetValorH(fechaHoraMDSein, listaGenXDia.Where(x => x.Tgenercodi == ConstantesPR5ReportesServicio.TgenercodiTermo).ToList());
                regDia.Mregenmdeolico = GetValorH(fechaHoraMDSein, listaGenXDia.Where(x => x.Tgenercodi == ConstantesPR5ReportesServicio.TgenercodiEolica).ToList());
                regDia.Mregenmdsolar = GetValorH(fechaHoraMDSein, listaGenXDia.Where(x => x.Tgenercodi == ConstantesPR5ReportesServicio.TgenercodiSolar).ToList());

                regDia.Mregenmdhora = fechaHoraMDSein;
                regDia.Mregenmdsein = GetValorH(fechaHoraMDSein, listaMedicionTotal96);
                regDia.Mregenmdexp = GetValorH(fechaHoraMDSein, listaTotalExp);
                regDia.Mregenmdimp = GetValorH(fechaHoraMDSein, listaTotalImp);

                //Máxima Demanda en Hora Punta
                this.GetDiaMaximaDemandaFromDataMD96(regDia.Mregenfecha.Date, regDia.Mregenfecha.Date, ConstantesRepMaxDemanda.TipoHoraPunta, listaMedicionTotal96, null, listaBloqueHorario,
                                                                    out DateTime fechaHoraHP, out DateTime fechaDiaHP, out int hMaxHP);

                regDia.Mregenhphora = fechaHoraHP;
                regDia.Mregenhpsein = GetValorH(fechaHoraHP, listaMedicionTotal96);
                regDia.Mregenhpexp = GetValorH(fechaHoraHP, listaTotalExp);
                regDia.Mregenhpimp = GetValorH(fechaHoraHP, listaTotalImp);

                //Máxima Demanda en Fuera Hora Punta
                this.GetDiaMaximaDemandaFromDataMD96(regDia.Mregenfecha.Date, regDia.Mregenfecha.Date, ConstantesRepMaxDemanda.TipoFueraHoraPunta, listaMedicionTotal96, null, listaBloqueHorario,
                                                                    out DateTime fechaHoraFHP, out DateTime fechaDiaFHP, out int hMaxFHP);

                regDia.Mregenfhphora = fechaHoraFHP;
                regDia.Mregenfhpsein = GetValorH(fechaHoraFHP, listaMedicionTotal96);
                regDia.Mregenfhpexp = GetValorH(fechaHoraFHP, listaTotalExp);
                regDia.Mregenfhpimp = GetValorH(fechaHoraFHP, listaTotalImp);
            }

            //los datos redondeados a 5 decimales
            foreach (var reg in listaProdgen)
            {
                reg.Pgenvalor = Math.Round(reg.Pgenvalor, 5);
            }

            //Guardar en BD
            GuardarTablaAbiMedidoresTransaccional(fechaInicial, fechaFinal, listaResumen, listaProdgen);
        }

        public static decimal? GetValorH(DateTime fechaHora, List<MeMedicion96DTO> lista)
        {
            DateTime diaConsulta = fechaHora.Date;

            int cuartoHora = fechaHora.Hour * 4 + fechaHora.Minute / 15;
            if (cuartoHora == 0)
            {
                cuartoHora = 96; //las 00:00
                diaConsulta = diaConsulta.AddDays(-1);
            }

            var listaXDia = lista.Where(x => x.Medifecha == diaConsulta.Date);
            if (listaXDia.Any())
            {
                decimal valor = 0;
                foreach (var regDia in listaXDia)
                {
                    decimal valorD = ((decimal?)regDia.GetType().GetProperty(ConstantesAppServicio.CaracterH + cuartoHora.ToString()).GetValue(regDia, null)).GetValueOrDefault(0);
                    valor += valorD;
                }

                return valor;
            }

            return null;
        }

        private void GuardarTablaAbiMedidoresTransaccional(DateTime fechaIni, DateTime fechaFin, List<AbiMedidoresResumenDTO> listaResumen, List<AbiProdgeneracionDTO> listaDetGen)
        {
            IDbConnection conn = null;
            DbTransaction tran = null;

            try
            {
                conn = FactorySic.GetAbiMedidoresResumenRepository().BeginConnection();
                tran = FactorySic.GetAbiMedidoresResumenRepository().StartTransaction(conn);

                DeleteAbiMedidoresResumenByRango(fechaIni, fechaFin, conn, tran);

                DeleteAbiProdgeneracionByRango(fechaIni, fechaFin, conn, tran);

                int correlativoMregencodi = FactorySic.GetAbiMedidoresResumenRepository().GetMaxId();
                foreach (var reg in listaResumen)
                {
                    reg.Mregencodi = correlativoMregencodi;
                    SaveAbiMedidoresResumen(reg, conn, tran);
                    correlativoMregencodi++;
                }

                int correlativoPgencodi = FactorySic.GetAbiProdgeneracionRepository().GetMaxId();
                foreach (var reg in listaDetGen)
                {
                    reg.Pgencodi = correlativoPgencodi;
                    SaveAbiProdgeneracion(reg, conn, tran);
                    correlativoPgencodi++;
                }

                //guardar definitivamente
                tran.Commit();
            }
            catch (Exception ex)
            {
                if (tran != null)
                    tran.Rollback();
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new ArgumentException("Ocurrió un error al momento de guardar los datos.");
            }
            finally
            {
                if (conn != null)
                    if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        /// <summary>
        /// Guardar Estructuras Factor Planta
        /// </summary>
        /// <param name="fechaPeriodoIni"></param>
        /// <param name="fechaPeriodoFin"></param>
        /// <param name="usuarioActualizacion"></param>
        public void GuardarEstructurasFactorPlanta(DateTime fechaPeriodoIni, DateTime fechaPeriodoFin, string usuarioActualizacion)
        {
            DateTime fechaActualizacion = DateTime.Now;

            //Obtener fechas
            DateTime fechaIniAnio = new DateTime(fechaPeriodoIni.Year, 1, 1).Date;
            DateTime fechaInicial = new DateTime(fechaPeriodoIni.Year, fechaPeriodoIni.Month, 1).Date;
            DateTime fechaFinal = new DateTime(fechaPeriodoFin.Year, fechaPeriodoFin.Month, 1).AddMonths(1).AddDays(-1);

            TimeSpan tsacum = fechaFinal.Subtract(fechaIniAnio);
            int totalDiaAcum = (int)tsacum.TotalDays + 1;

            //Data de Medicion96
            List<MeMedicion96DTO> listAllSinCruceHO = this.ListaDataMDGeneracionConsolidado(fechaIniAnio, fechaFinal, ConstantesMedicion.IdTipogrupoTodos, ConstantesMedicion.IdTipoGeneracionTodos.ToString(), ConstantesMedicion.IdEmpresaTodos.ToString(), ConstanteValidacion.EstadoTodos, ConstantesMedicion.IdTipoRecursoTodos.ToString(), false);
            this.SetEquicodiActualToMedicion96(listAllSinCruceHO);

            //Coes, no Coes
            List<PrGrupodatDTO> listaOperacionCoes = FactorySic.GetPrGrupodatRepository().ListarHistoricoValores(ConstantesPR5ReportesServicio.PropGrupoOperacionCoes.ToString(), -1)
                                                        .Where(x => x.Deleted == 0).OrderByDescending(x => x.Fechadat).ToList();
            //meses
            List<DateTime> listaMes = new List<DateTime>();
            for (var mes = fechaPeriodoIni.Date; mes.Date <= fechaPeriodoFin.Date; mes = mes.AddMonths(1))
                listaMes.Add(mes);

            foreach (var mes in listaMes)
            {
                DateTime fechaIniMes = mes;
                DateTime fechaFinMes = mes.AddMonths(1).AddDays(-1);

                //Generación acumulada
                List<MeMedicion96DTO> listSinCruceHOxAcum = listAllSinCruceHO.Where(x => x.Medifecha >= fechaIniAnio && x.Medifecha <= fechaFinMes).ToList();

                List<AbiFactorplantaDTO> listaCentralXAcum = (from t in listSinCruceHOxAcum
                                                              group t by new { t.Tgenercodi, t.Equipadreactual }
                                                                  into destino
                                                              select new AbiFactorplantaDTO()
                                                              {
                                                                  Fpfechaperiodo = fechaIniMes,
                                                                  Fpenergia = destino.Sum(t => t.Meditotal ?? 0),
                                                                  Tgenercodi = destino.Key.Tgenercodi,
                                                                  Grupocodi = destino.Last().Grupocodi,
                                                                  Equicodiactual = destino.Key.Equipadreactual,
                                                                  Equicodi = destino.Last().Equipadre,
                                                                  Emprcodi = destino.Last().Emprcodi,
                                                                  Fptipogenerrer = destino.Last().Tipogenerrer,
                                                                  Fpusumodificacion = usuarioActualizacion,
                                                                  Fpfecmodificacion = fechaActualizacion
                                                              }).OrderBy(x => x.Fpfechaperiodo).ThenBy(x => x.Tgenercodi).ThenBy(x => x.Emprcodi).ThenBy(x => x.Equicodi).ToList();
                listaCentralXAcum = listaCentralXAcum.Where(x => x.Fpenergia > 0).ToList();

                var listaTmp = listaCentralXAcum.Where(x => x.Equicodiactual == 11571).ToList();

                //Potencia Efectiva de las centrales 
                List<EqEquipoDTO> listaEqPe = ListarEquipoCentralYPeXFecha(fechaFinMes);

                //Obtener potencia efectiva por central
                foreach (var reg in listaCentralXAcum)
                {
                    if (reg.Equicodi == 15785)
                    { }

                    var regDat = listaOperacionCoes.Find(x => x.Fechadat <= fechaFinMes && x.Grupocodi == reg.Grupocodi);
                    reg.Fpintegrante = regDat != null ? regDat.Formuladat : ConstantesAppServicio.SI;

                    var objCentralPe = listaEqPe.Find(x => x.Equipadre == reg.Equicodiactual);
                    if (objCentralPe != null) reg.Fppe = objCentralPe.Pe ?? 0;

                    reg.Fpenergia = reg.Fpenergia / 4; //MW a MWh
                    if (reg.Fppe > 0)
                    {
                        reg.Fpvalor = reg.Fpenergia / (reg.Fppe * totalDiaAcum * 24);
                        reg.Fpvalor = reg.Fpvalor * 100;
                    }
                }
                listaCentralXAcum = listaCentralXAcum.Where(x => x.Fpvalor > 0).ToList();

                //Generación mes
                TimeSpan tsmes = fechaFinMes.Subtract(fechaIniMes);
                int totalDiaMes = (int)tsmes.TotalDays + 1;

                List<MeMedicion96DTO> listSinCruceHOxMes = listAllSinCruceHO.Where(x => x.Medifecha >= fechaIniMes && x.Medifecha <= fechaFinMes).ToList();
                List<AbiFactorplantaDTO> listaCentralXMes = (from t in listSinCruceHOxMes
                                                             group t by new { t.Tgenercodi, t.Equipadreactual }
                                                                  into destino
                                                             select new AbiFactorplantaDTO()
                                                             {
                                                                 Fpfechaperiodo = fechaIniMes,
                                                                 Fpenergia = destino.Sum(t => t.Meditotal ?? 0),
                                                                 Tgenercodi = destino.Key.Tgenercodi,
                                                                 Grupocodi = destino.Last().Grupocodi,
                                                                 Equicodiactual = destino.Key.Equipadreactual,
                                                                 Equicodi = destino.Last().Equipadre,
                                                                 Emprcodi = destino.Last().Emprcodi,
                                                                 Fptipogenerrer = destino.Last().Tipogenerrer,
                                                             }).OrderBy(x => x.Fpfechaperiodo).ThenBy(x => x.Tgenercodi).ThenBy(x => x.Emprcodi).ThenBy(x => x.Equicodi).ToList();
                listaCentralXMes = listaCentralXMes.Where(x => x.Fpenergia > 0).ToList(); //

                foreach (var reg in listaCentralXMes)
                {
                    var objMes = listaCentralXMes.Find(x => x.Equicodi == reg.Equicodi && x.Fpintegrante == reg.Fpintegrante);

                    reg.FpenergiaMes = objMes != null ? objMes.Fpenergia / 4 : 0; //MW a MWh
                    if (reg.Fppe > 0)
                    {
                        reg.Fpvalormes = reg.FpenergiaMes / (reg.Fppe * totalDiaMes * 24);
                        reg.Fpvalormes = reg.Fpvalormes * 100;
                    }

                    var regDat = listaOperacionCoes.Find(x => x.Fechadat <= fechaFinMes && x.Grupocodi == reg.Grupocodi);
                    reg.Fpintegrante = regDat != null ? regDat.Formuladat : ConstantesAppServicio.SI;
                }

                this.DeleteAbiFactorplantaByMes(fechaIniMes);

                foreach (var reg in listaCentralXAcum)
                {
                    reg.Equicodi = reg.Equicodiactual;
                    reg.Fpvalor = Math.Round(reg.Fpvalor, 2);
                    reg.Fpvalormes = Math.Round(reg.Fpvalormes, 2);
                    reg.Fppe = Math.Round(reg.Fppe, 5);
                    reg.Fpenergia = Math.Round(reg.Fpenergia / 1000, 5);
                    reg.Fpintegrante = reg.Fpintegrante != null ? reg.Fpintegrante : ConstantesAppServicio.NO;
                    reg.Fptipogenerrer = reg.Fptipogenerrer != null ? reg.Fptipogenerrer : ConstantesAppServicio.NO;
                    this.SaveAbiFactorplanta(reg);
                }
            }
        }

        public List<EqEquipoDTO> ListarEquipoCentralYPeXFecha(DateTime fechaPeriodo)
        {
            var servIndisp = new INDAppServicio();

            //obtener la lista de unidades para ese escenario
            servIndisp.ListarEqCentralSolarOpComercial(fechaPeriodo, fechaPeriodo, out List<EqEquipoDTO> listaCentrales1, out List<EqEquipoDTO> istaAllEquipos1, out List<ResultadoValidacionAplicativo> istaMsj1);
            servIndisp.ListarEqCentralEolicaOpComercial(fechaPeriodo, fechaPeriodo, out List<EqEquipoDTO> listaCentrales2, out List<EqEquipoDTO> istaAllEquipos2, out List<ResultadoValidacionAplicativo> istaMsj2);
            servIndisp.ListarEqCentralHidraulicoOpComercial(fechaPeriodo, fechaPeriodo, out List<EqEquipoDTO> listaCentrales3, out List<EqEquipoDTO> istaEquiposHidro, out List<ResultadoValidacionAplicativo> istaMsj3);
            servIndisp.ListarUnidadTermicoOpComercial(ConstantesIndisponibilidades.AppPF, fechaPeriodo, fechaPeriodo, out List<EqEquipoDTO> listaUnidadesTermo, out List<EqEquipoDTO> istaEquiposTermicos, out List<ResultadoValidacionAplicativo> istaMsj4);

            List<EqEquipoDTO> listaPropEq = new List<EqEquipoDTO>();
            listaPropEq.AddRange(listaCentrales1);
            listaPropEq.AddRange(listaCentrales2);
            listaPropEq.AddRange(listaCentrales3);

            var listaEqCentralTermico = new List<EqEquipoDTO>();
            foreach (var item in listaUnidadesTermo.GroupBy(x => x.Equipadre))
            {
                var objEqCentral = item.First();
                objEqCentral.Pe = item.Sum(x => x.Pe);
                listaEqCentralTermico.Add(objEqCentral);
            }

            listaPropEq.AddRange(listaEqCentralTermico);

            return listaPropEq;
        }

        /// <summary>
        /// Obtener valor de potencia efectiva
        /// </summary>
        /// <param name="listaPE"></param>
        /// <param name="equicodi">Código de la central</param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public static decimal GetPotInstaladaOrEfectivaByEquipo(List<EqPropequiDTO> listaPot, int equicodi, DateTime fechaFin)
        {
            EqPropequiDTO regProp = listaPot.Where(x => x.Fechapropequi < fechaFin.Date.AddDays(1)).ToList().Find(x => x.Equicodi == equicodi);
            return regProp != null ? regProp.ValorDecimal.GetValueOrDefault(0) : 0;
        }

        /// <summary>
        /// Obtener valor de potencia efectiva
        /// </summary>
        /// <param name="listaPE"></param>
        /// <param name="emprcodi"></param>
        /// <param name="equicodi">Código de la central</param>
        /// <returns></returns>
        public static decimal GetPotEfectivaTermico(List<PrGrupodatDTO> listaPE, int emprcodi, int equicodi, out int? grupocodimodo)
        {
            List<PrGrupodatDTO> listaDataXEmp = listaPE.Where(x => x.Equipadre == equicodi || x.Equicodi == equicodi).ToList();
            if (listaDataXEmp.Select(x => x.Emprcodi).Distinct().Count() >= 2) //si existen más de dos empresas para el mismo grupo
            {
                listaDataXEmp = listaDataXEmp.Where(x => x.Emprcodi == emprcodi).ToList();
            }

            List<PrGrupodatDTO> listaData = listaDataXEmp.Where(x => x.Equipadre == equicodi).ToList();
            if (listaData.Count == 0)
                listaData = listaDataXEmp.Where(x => x.Equicodi == equicodi).ToList();

            grupocodimodo = null;
            if (listaData.Count > 0 && listaData.First().Famcodi == ConstantesHorasOperacion.IdTipoTermica)
                grupocodimodo = listaData.First().Grupocodimodo;
            return listaData.Sum(x => x.Valor);
        }

        /// <summary>
        /// Guardar Estructuras Potencia Instaladad y Potencia Efectiva
        /// </summary>
        /// <param name="fechaPeriodoIni"></param>
        /// <param name="fechaPeriodoFin"></param>
        /// <param name="usuarioActualizacion"></param>
        public void GuardarEstructurasPotEfectiva(DateTime fechaPeriodoIni, DateTime fechaPeriodoFin, string usuarioActualizacion)
        {
            DateTime fechaActualizacion = DateTime.Now;

            //Obtener fechas
            DateTime fechaIniAnio = new DateTime(fechaPeriodoIni.Year, 1, 1).Date;
            DateTime fechaInicial = new DateTime(fechaPeriodoIni.Year, fechaPeriodoIni.Month, 1).Date;
            DateTime fechaFinal = new DateTime(fechaPeriodoFin.Year, fechaPeriodoFin.Month, 1).AddMonths(1).AddDays(-1);

            //Lista de Centrales y unidades de generación
            List<EqEquipoDTO> listaEq = FactorySic.GetEqEquipoRepository().ListarEquiposPorFamilia(ConstantesHorasOperacion.CodFamilias);
            listaEq.AddRange(FactorySic.GetEqEquipoRepository().ListarEquiposPorFamilia(ConstantesHorasOperacion.CodFamiliasGeneradores));

            //Lista de equipos con categoria
            List<EqCategoriaEquipoDTO> listaEqCtgTecnologia = this.servEquipamiento.ListaClasificacionPaginado(-2, -2, -2, -2,
               ConstantesPR5ReportesServicio.CategoriaTecnolog, -3, string.Empty, -1, -1);
            List<EqCategoriaEquipoDTO> listaEqCtgGas = this.servEquipamiento.ListaClasificacionPaginado(-2, -2, -2, -2,
               ConstantesPR5ReportesServicio.CategoriaRecursoGas, -3, string.Empty, -1, -1);

            //Coes, no Coes
            List<PrGrupodatDTO> listaOperacionCoes = FactorySic.GetPrGrupodatRepository().ListarHistoricoValores(ConstantesPR5ReportesServicio.PropGrupoOperacionCoes.ToString(), -1)
                                                        .Where(x => x.Deleted == 0).OrderByDescending(x => x.Fechadat).ToList();

            //meses
            List<DateTime> listaMes = new List<DateTime>();
            for (var mes = fechaPeriodoIni.Date; mes.Date <= fechaPeriodoFin.Date; mes = mes.AddMonths(1))
                listaMes.Add(mes);

            foreach (var mes in listaMes)
            {
                List<EqEquipoDTO> listaEqFinal = new List<EqEquipoDTO>();
                DateTime fechaIniMes = mes;
                DateTime fechaFinMes = mes.AddMonths(1).AddDays(-1);

                //Lista de equipos en operacion comercial
                List<EqEquipoDTO> listaEqMes = ListarEquipoOpComercialYPeXFecha(fechaFinMes);

                List<EqEquipoDTO> listaEqMesGenHidro = listaEqMes.Where(x => x.Famcodi == ConstantesHorasOperacion.IdGeneradorHidroelectrico).ToList();
                List<EqEquipoDTO> listaEqMesCentralSolar = listaEqMes.Where(x => x.Famcodi == ConstantesHorasOperacion.IdTipoSolar).ToList();
                List<EqEquipoDTO> listaEqMesCentralEolica = listaEqMes.Where(x => x.Famcodi == ConstantesHorasOperacion.IdTipoEolica).ToList();
                List<EqEquipoDTO> listaEqMesTermo = listaEqMes.Where(x => x.Famcodi == ConstantesHorasOperacion.IdTipoTermica || x.Famcodi == ConstantesHorasOperacion.IdGeneradorTemoelectrico).ToList();

                List<AbiPotefecDTO> listaPotEfeGenXMes = listaEqMes.Select(x => new AbiPotefecDTO()
                {
                    Pefecfechames = fechaIniMes,
                    Tgenercodi = x.Tgenercodi,
                    Fenergcodi = x.Fenergcodi,
                    Emprcodi = x.Emprcodi.GetValueOrDefault(0),
                    Grupocodi = x.Grupocodi.GetValueOrDefault(0),
                    Famcodi = x.Famcodi.GetValueOrDefault(0),
                    Equicodi = x.Equicodi,
                    Equipadre = x.Equipadre.GetValueOrDefault(0),
                    Pefecvalorpe = x.Pe ?? 0,
                    Pefecvalorpinst = x.PotenciaInstalada ?? 0,
                    Pefectipogenerrer = x.Tipogenerrer,
                    Pefecfecmodificacion = fechaActualizacion,
                    Pefecusumodificacion = usuarioActualizacion
                }).ToList();
                listaPotEfeGenXMes = listaPotEfeGenXMes.OrderBy(x => x.Tgenercodi).ThenBy(x => x.Emprcodi).ThenBy(x => x.Equicodi).ToList();

                foreach (var reg in listaPotEfeGenXMes)
                {
                    if (reg.Famcodi == ConstantesHorasOperacion.IdTipoTermica || reg.Famcodi == ConstantesHorasOperacion.IdTipoHidraulica
                        || reg.Famcodi == ConstantesHorasOperacion.IdTipoEolica || reg.Famcodi == ConstantesHorasOperacion.IdTipoSolar)
                    {
                        reg.Equipadre = reg.Equicodi;
                    }

                    var regDat = listaOperacionCoes.Find(x => x.Fechadat <= fechaFinMes && x.Grupocodi == reg.Grupocodi);
                    reg.Pefecintegrante = regDat != null ? regDat.Formuladat : ConstantesAppServicio.SI;

                    var regCtg = listaEqCtgTecnologia.Find(x => x.Equicodi == reg.Equicodi);
                    if (regCtg != null)
                        reg.Ctgdetcodi = regCtg.Ctgdetcodi;

                    var regCtgGas = listaEqCtgGas.Find(x => x.Equicodi == reg.Equicodi);
                    if (regCtgGas != null)
                        reg.Ctgdetcodi2 = regCtgGas.Ctgdetcodi;
                }

                listaPotEfeGenXMes = listaPotEfeGenXMes.Where(x => x.Pefecvalorpe > 0).ToList();

                this.DeleteAbiPotefecByMes(fechaIniMes);

                foreach (var reg in listaPotEfeGenXMes)
                {
                    reg.Pefecintegrante = reg.Pefecintegrante != null ? reg.Pefecintegrante : ConstantesAppServicio.NO;
                    reg.Pefectipogenerrer = reg.Pefectipogenerrer != null ? reg.Pefectipogenerrer : ConstantesAppServicio.NO;
                    this.SaveAbiPotefec(reg);
                }
            }
        }

        public List<EqEquipoDTO> ListarEquipoOpComercialYPeXFecha(DateTime fechaPeriodo)
        {
            var servIndisp = new INDAppServicio();

            //obtener la lista de unidades para ese escenario
            servIndisp.ListarEqCentralSolarOpComercial(fechaPeriodo, fechaPeriodo, out List<EqEquipoDTO> listaCentrales1, out List<EqEquipoDTO> istaAllEquipos1, out List<ResultadoValidacionAplicativo> istaMsj1);
            servIndisp.ListarEqCentralEolicaOpComercial(fechaPeriodo, fechaPeriodo, out List<EqEquipoDTO> listaCentrales2, out List<EqEquipoDTO> istaAllEquipos2, out List<ResultadoValidacionAplicativo> istaMsj2);
            servIndisp.ListarEqCentralHidraulicoOpComercial(fechaPeriodo, fechaPeriodo, out List<EqEquipoDTO> listaCentrales3, out List<EqEquipoDTO> istaEquiposHidro, out List<ResultadoValidacionAplicativo> istaMsj3);
            servIndisp.ListarUnidadTermicoOpComercial(ConstantesIndisponibilidades.AppPF, fechaPeriodo, fechaPeriodo, out List<EqEquipoDTO> listaUnidadesTermo, out List<EqEquipoDTO> istaEquiposTermicos, out List<ResultadoValidacionAplicativo> istaMsj4);

            List<EqEquipoDTO> listaPropEq = new List<EqEquipoDTO>();
            listaPropEq.AddRange(listaCentrales1);
            listaPropEq.AddRange(listaCentrales2);
            listaPropEq.AddRange(istaEquiposHidro.Where(x => x.Famcodi == ConstantesHorasOperacion.IdGeneradorHidroelectrico).ToList());
            listaPropEq.AddRange(listaUnidadesTermo);

            return listaPropEq;
        }

        /// <summary>
        /// Agrega el último codigo de la central a sus anteriores migraciones
        /// </summary>
        /// <param name="listaData"></param>
        private void SetEquicodiActualToMedicion96(List<MeMedicion96DTO> listaData)
        {
            List<SiHisempeqDataDTO> listaHistEq = this.servTitEmp.ListSiHisempeqDatas("-1");

            foreach (var reg in listaData)
            {
                if (reg.Equipadre == 11571 || reg.Equipadre == 18825 || reg.Equipadre == 18863)
                { }
                var regHistEq = listaHistEq.Find(x => x.Equicodi == reg.Equipadre || x.Equicodiold == reg.Equipadre || x.Equicodiactual == reg.Equipadre);
                if (regHistEq != null)
                    reg.Equipadreactual = regHistEq.Equicodiactual;
                else
                    reg.Equipadreactual = reg.Equipadre;
            }
        }

        #endregion

        #region Validación de Medidores vs Horas de Operación 

        /// <summary>
        /// Cálculo de Validación
        /// </summary>
        /// <param name="fechaPeriodo"></param>
        /// <param name="emprcodi"></param>
        /// <param name="listaM96Extranet"></param>
        /// <returns></returns>
        public List<LogErrorHOPvsMedidores> ListarValidacionHopVsMedidores(DateTime fechaPeriodo, int emprcodi, List<MeMedicion96DTO> listaM96Extranet)
        {
            DateTime fechaIni = fechaPeriodo;
            DateTime fechaFin = fechaPeriodo.AddMonths(1).AddDays(-1);

            var servHO = new HorasOperacionAppServicio();
            var servInd = new INDAppServicio();

            //mensajes
            List<ResultadoValidacionAplicativo> listaMsj = new List<ResultadoValidacionAplicativo>();

            #region Insumos

            // Medidores de generación
            List<MeMedicion96DTO> listaM96 = listaM96Extranet ?? new List<MeMedicion96DTO>();

            // Puntos de medición (unidades)
            List<MePtomedicionDTO> listaPto = listaM96.GroupBy(x => x.Ptomedicodi)
                                            .Select(x => new MePtomedicionDTO()
                                            {
                                                Ptomedicodi = x.Key,
                                                Equicodi = x.First().Equicodi,
                                                Equinomb = x.First().Equinomb,
                                                Famcodi = (short)x.First().Famcodi,
                                                Equipadre = x.First().Equipadre,
                                                Central = x.First().Central
                                            }).ToList();

            // Listar todos los equipos y centrales que tienen operación comercial
            var listaEquiposTermicos = servInd.ListarEquipoOpComercial(fechaIni, fechaFin, ConstantesHorasOperacion.IdTipoTermica, out List<ResultadoValidacionAplicativo> listaMsjEq, false);
            List<EqEquipoDTO> listaEqGen = listaEquiposTermicos.Where(x => x.Famcodi == ConstantesHorasOperacion.IdGeneradorTemoelectrico).ToList();
            List<EqEquipoDTO> listaEqCentral = listaEquiposTermicos.Where(x => x.Famcodi == ConstantesHorasOperacion.IdTipoTermica).ToList();

            List<EqEquipoDTO> listaUnidad = listaEquiposTermicos.Where(x => listaPto.Select(y => y.Equicodi).Contains(x.Equicodi)).ToList();

            // Lista modos de operación
            List<PrGrupoDTO> listaGrupo = FactorySic.GetPrGrupoRepository().List();
            listaGrupo = listaGrupo.Where(x => x.GrupoEstado != ConstantesAppServicio.Anulado).ToList();
            foreach (var regUnidad in listaUnidad)
            {
                var regGrupo = listaGrupo.Find(x => x.Grupocodi == regUnidad.Grupocodi);
                if (regGrupo != null) regUnidad.Grupointegrante = (regGrupo.Grupointegrante ?? "N").Trim();
            }

            List<PrGrupoDTO> listaGrupoModo = listaGrupo.Where(x => x.Catecodi == 2).ToList();

            // Integrantes Coes, no Coes
            List<PrGrupodatDTO> listaOperacionCoes = FactorySic.GetPrGrupodatRepository().ListarHistoricoValores(ConstantesPR5ReportesServicio.PropGrupoOperacionCoes.ToString(), -1)
                                                        .Where(x => x.Deleted == 0).OrderByDescending(x => x.Fechadat).ToList();

            // Horas de Operacion
            List<EveHoraoperacionDTO> listaHOP = servHO.ListarHorasOperacionByCriteria(fechaPeriodo, fechaPeriodo.AddMonths(1),
                                                    emprcodi.ToString(), ConstantesHorasOperacion.ParamCentralTodos, ConstantesHorasOperacion.TipoListadoTodo);
            listaHOP = servHO.CompletarListaHoraOperacionTermo(listaHOP);

            List<EveSubcausaeventoDTO> listaSubcausa = servHO.ListarTipoOperacionHO();

            #endregion

            #region Calculo

            List<CeldaMW96xUnidad> listaCelda = new List<CeldaMW96xUnidad>();


            //obtener el detalle por unidad - Parte 1
            var lista96Clone1 = ClonarM96(listaM96);

            for (DateTime f = fechaIni.Date; f <= fechaFin.Date; f = f.AddDays(1))
            {
                //medidores x dia
                var listaM96XDia = lista96Clone1.Where(x => x.Medifecha == f).ToList();
                //horas de operación a h96
                var listaHO15minXDia = servHO.ListarHO15minExtranetMedidores(listaHOP.Where(x => x.Hophorini.Value.Date == f).ToList(), f);
                //Asignar horas de operación a unidad
                List<Detalle96HoraOperacionUnidad> listaDetalleHOXDia = ListarDetalleHO(listaHOP.Where(x => x.Hophorini.Value.Date == f).ToList(), listaUnidad, listaGrupoModo);

                List<CeldaMW96xUnidad> listaCelda1 = ListaCeldaReporte96(f, listaUnidad, listaM96XDia, listaHO15minXDia, listaGrupoModo, listaDetalleHOXDia, listaSubcausa, listaOperacionCoes
                                                                    , out List<ResultadoValidacionAplicativo> listaMjs2);

                listaCelda.AddRange(listaCelda1);
                listaMsj.AddRange(listaMjs2);
            }

            /////////////////////////////////////////////////////////////////////////////
            //Procesar por unidad
            List<LogErrorHOPvsMedidores> listaLogFila = new List<LogErrorHOPvsMedidores>();
            foreach (var regUnidad in listaUnidad)
            {
                int fila = 19;
                for (DateTime f = fechaIni.Date; f <= fechaFin.Date; f = f.AddDays(1))
                {
                    var regCelda = listaCelda.Find(x => x.Fecha == f && x.UnidadTermico.Equicodi == regUnidad.Equicodi);
                    if (regCelda != null)
                    {
                        for (int h = 1; h <= 96; h++)
                        {
                            string msjH = regCelda.ListaMensaje[h - 1];
                            if (!string.IsNullOrEmpty(msjH))
                            {
                                LogErrorHOPvsMedidores log = new LogErrorHOPvsMedidores()
                                {
                                    FilaIni = fila,
                                    FilaFin = fila,
                                    Linea = fila + "",
                                    FechaInicio = f.AddMinutes(h * 15).ToString(ConstantesAppServicio.FormatoFechaFull),
                                    FechaFin = f.AddMinutes(h * 15).ToString(ConstantesAppServicio.FormatoFechaFull),
                                    Empresa = (regCelda.UnidadTermico.Emprnomb ?? "").Trim(),
                                    Central = (regCelda.UnidadTermico.Central ?? "").Trim(),
                                    Grupo = (regCelda.UnidadTermico.Equiabrev ?? "").Trim(),
                                    ModoOperacion = (regCelda.ListaGruponomb[h - 1] ?? "").Trim(),
                                    DescripcionError = msjH
                                };

                                listaLogFila.Add(log);
                            }
                            fila++;
                        }
                    }
                    else
                    {
                        fila += 96;
                    }
                }
            }

            //Unificar lineas consecutivas
            List<LogErrorHOPvsMedidores> listaRpt = new List<LogErrorHOPvsMedidores>();
            foreach (var regAgrup in listaLogFila.GroupBy(x => new { x.ModoOperacion, x.DescripcionError }))
            {
                var listaXAgrup = regAgrup.ToList();

                //generar reporte HOP
                for (int i = 0; i < listaXAgrup.Count; i++)
                {
                    var regAct = listaXAgrup[i];

                    LogErrorHOPvsMedidores log = new LogErrorHOPvsMedidores()
                    {
                        FilaIni = regAct.FilaIni,
                        FilaFin = regAct.FilaFin,
                        Linea = regAct.Linea,
                        FechaInicio = regAct.FechaInicio,
                        FechaFin = regAct.FechaFin,
                        Empresa = regAct.Empresa,
                        Central = regAct.Central,
                        Grupo = regAct.Grupo,
                        ModoOperacion = regAgrup.Key.ModoOperacion,
                        DescripcionError = regAgrup.Key.DescripcionError
                    };

                    if (i + 1 < listaXAgrup.Count)//tiene siguiente
                    {
                        //buscar fin
                        bool fin = false;
                        for (int j = i + 1; j < listaXAgrup.Count && !fin; j++)
                        {
                            var regSig = listaXAgrup[j];

                            if (regAct.FilaFin + 1 == regSig.FilaIni)
                            {
                                log.FechaFin = regSig.FechaFin;
                                log.FilaFin = regSig.FilaIni;
                                log.Linea = (log.FilaIni != log.FilaFin ? log.FilaIni + "-" + log.FilaFin : log.FilaIni.ToString());
                                i++;
                            }
                            else
                            {
                                fin = true;
                            }

                            regAct = regSig;
                        }
                    }

                    listaRpt.Add(log);
                }
            }

            #endregion

            return listaRpt;
        }

        /// <summary>
        /// CÁLCULO: Obtener los datos por Unidad de generación
        /// </summary>
        /// <param name="f"></param>
        /// <param name="listaUnidadTermo"></param>
        /// <param name="lista96Gen"></param>
        /// <param name="listaHO15min"></param>
        /// <param name="listaGrupoModo"></param>
        /// <param name="listaDetHo"></param>
        /// <param name="listaSubcausa"></param>
        /// <param name="listaOperacionCoes"></param>
        /// <param name="listaMsj"></param>
        /// <returns></returns>
        private List<CeldaMW96xUnidad> ListaCeldaReporte96(DateTime f, List<EqEquipoDTO> listaUnidadTermo
                                                        , List<MeMedicion96DTO> lista96Gen, List<EveHoraoperacionDTO> listaHO15min, List<PrGrupoDTO> listaGrupoModo
                                                        , List<Detalle96HoraOperacionUnidad> listaDetHo, List<EveSubcausaeventoDTO> listaSubcausa, List<PrGrupodatDTO> listaOperacionCoes,
                                                        out List<ResultadoValidacionAplicativo> listaMsj)
        {
            List<CeldaMW96xUnidad> listaCelda = new List<CeldaMW96xUnidad>();
            listaMsj = new List<ResultadoValidacionAplicativo>();

            //Setear valores defecto
            foreach (var regUnidad in listaUnidadTermo)
            {
                //Es integrante
                bool esIntegrante = ReporteMedidoresAppServicio.SetValorCentralIntegrante(regUnidad.Grupocodi, f, regUnidad.Grupointegrante, listaOperacionCoes) == "S";

                if (esIntegrante)
                {
                    //obtener los modos de operación de la unidad y la potencia asociada
                    List<CeldaMW96xUnidad> listaCeldaXUnidad = listaCelda.Where(x => x.UnidadTermico.Equicodi == regUnidad.Equicodi).ToList();

                    if (!listaCeldaXUnidad.Any())
                    {
                        MeMedicion96DTO reg96Gen = lista96Gen.Find(x => x.Equicodi == regUnidad.Equicodi);

                        for (int h = 1; h <= 96; h++)
                        {
                            decimal? valorH = 0;
                            if (reg96Gen != null) valorH = (decimal?)reg96Gen.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg96Gen, null);

                            AsignarDatosX15minToUnidad(h, valorH ?? 0, f, regUnidad, new PrGrupoDTO(), 0, 0, new List<Detalle96HoraOperacionUnidad>(), listaSubcausa, ref listaCelda);
                        }
                    }
                }
            }

            //Setear flag hora de operación
            List<EveHoraoperacionDTO> listaHOModo = listaHO15min.Where(x => x.Hophorini.Value.Date == f && x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoModo).ToList();
            List<EveHoraoperacionDTO> listaHOUnidad = listaHO15min.Where(x => x.Hophorini.Value.Date == f && x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoUnidad).ToList();

            foreach (var regHo in listaHOModo)
            {
                if (regHo.Hopcodi == 435640)
                { }

                PrGrupoDTO regModo = listaGrupoModo.Find(x => x.Grupocodi == regHo.Grupocodi);
                var listaHoXModo = listaHOUnidad.Where(x => x.Hopcodipadre == regHo.Hopcodi).ToList();

                foreach (var regHoUni in listaHoXModo)
                {
                    //Fila Excel de Extranet
                    MeMedicion96DTO reg96Gen = lista96Gen.Find(x => x.Equicodi == regHoUni.Equicodi);
                    if (reg96Gen == null) reg96Gen = lista96Gen.Find(x => x.Equicodi == regHoUni.Equipadre);

                    //Unidad termica
                    EqEquipoDTO regEqUnidad = null;
                    if (reg96Gen != null) regEqUnidad = listaUnidadTermo.Find(x => x.Equicodi == reg96Gen.Equicodi);

                    //Es integrante
                    bool esIntegrante = false;
                    if (regEqUnidad != null) esIntegrante = ReporteMedidoresAppServicio.SetValorCentralIntegrante(regEqUnidad.Grupocodi, f, regEqUnidad.Grupointegrante, listaOperacionCoes) == "S";

                    //REalizar validación
                    if (regEqUnidad != null && reg96Gen != null && esIntegrante)
                    {
                        //Determinar los grupos por cada media hora
                        for (int h = regHoUni.HIni96; h <= regHoUni.HFin96; h++)
                        {
                            if (h >= 1 && h <= 96)
                            {
                                if (regHoUni.Equicodi == 20524 && h == 33)
                                { }

                                DateTime fi = f.Date.AddMinutes(h * 15);

                                decimal? valorH = (decimal?)reg96Gen.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg96Gen, null);

                                AsignarDatosX15minToUnidad(h, valorH ?? 0, f, regEqUnidad, regModo, regHo.Grupocodi.Value, regHo.Subcausacodi.Value, listaDetHo, listaSubcausa, ref listaCelda);
                                if (valorH.GetValueOrDefault(0) > 0)
                                    reg96Gen.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).SetValue(reg96Gen, 0.0m);
                                else
                                {
                                    var regCelda = listaCelda.Find(x => x.UnidadTermico.Equicodi == regEqUnidad.Equicodi);
                                    if (regCelda.ListaCalifHo[h - 1] != 0 && regCelda.ListaMW[h - 1].GetValueOrDefault(0) == 0)
                                    {
                                        listaMsj.Add(new ResultadoValidacionAplicativo()
                                        {
                                            Equipadre = reg96Gen.Equipadre,
                                            Descripcion = string.Format("La unidad de generación {0} no tiene datos de despacho para la media hora {1}."
                                                                    , (regHoUni.Equiabrev ?? "").Trim(), fi.ToString(ConstantesAppServicio.FormatoFechaFull))
                                        });
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (regEqUnidad == null)
                            listaMsj.Add(new ResultadoValidacionAplicativo() { Equipadre = regModo.Equipadre, Descripcion = string.Format("El modo de operación {0} no está asociado a ninguna unidad.", regModo.Gruponomb) });

                        if (reg96Gen == null)
                            listaMsj.Add(new ResultadoValidacionAplicativo() { Equipadre = regModo.Equipadre, Descripcion = string.Format("La unidad {0} tiene registros en Horas de Operación pero no registros en Despacho.", (regHoUni.Equiabrev ?? "").Trim()) });
                    }
                }
            }

            //Validación
            foreach (var regCelda in listaCelda)
            {
                //celda.TieneAlertaHo = tieneCalifXh || tieneDetalleHo;
                for (int h = 1; h <= 96; h++)
                {
                    var mwExtranetMedidor = regCelda.ListaMW[h - 1].GetValueOrDefault(0);
                    var mwHoraOperacion = regCelda.ListaMWHo[h - 1].GetValueOrDefault(0);

                    string msjCeldaH = "";
                    if (mwExtranetMedidor > 0 && mwHoraOperacion <= 0) msjCeldaH = "NO EXISTE HORAS DE OPERACIÓN EN PERIODOS INDICADOS.";
                    if (mwExtranetMedidor <= 0 && mwHoraOperacion > 0) msjCeldaH = "NO EXISTE GENERACIÓN EN PERIODOS INDICADOS, PERO SÍ HAY REGISTRO DE HORAS DE OPERACIÓN.";
                    regCelda.ListaMensaje[h - 1] = msjCeldaH;
                }
            }

            return listaCelda;
        }

        /// <summary>
        /// Actualizar datos de la Unidad de generación
        /// </summary>
        /// <param name="h"></param>
        /// <param name="valorH"></param>
        /// <param name="fechaPeriodo"></param>
        /// <param name="regUnidad"></param>
        /// <param name="regModo"></param>
        /// <param name="grupocodimodo"></param>
        /// <param name="subcausacodi"></param>
        /// <param name="listaDetHo"></param>
        /// <param name="listaSubcausa"></param>
        /// <param name="listaCelda"></param>
        private void AsignarDatosX15minToUnidad(int h, decimal valorH, DateTime fechaPeriodo, EqEquipoDTO regUnidad,
                                                PrGrupoDTO regModo, int grupocodimodo, int subcausacodi,
                                                 List<Detalle96HoraOperacionUnidad> listaDetHo,
                                                List<EveSubcausaeventoDTO> listaSubcausa, ref List<CeldaMW96xUnidad> listaCelda)
        {
            var regCelda = listaCelda.Find(x => x.Fecha == fechaPeriodo && x.UnidadTermico.Equicodi == regUnidad.Equicodi);
            var regSubcausa = listaSubcausa.Find(x => x.Subcausacodi == subcausacodi);

            if (regCelda == null)
            {
                regCelda = new CeldaMW96xUnidad()
                {
                    Fecha = fechaPeriodo,
                    UnidadTermico = regUnidad,
                    Equipadre = regUnidad.Equipadre.Value
                };

                regCelda.DetalleHO = listaDetHo.Find(x => x.Equicodi == regUnidad.Equicodi);

                //setear valor
                listaCelda.Add(regCelda);
            }

            //setear valores por cada H 30min
            if (valorH > 0) regCelda.ListaMW[h - 1] = valorH;
            if (regCelda.ListaCalifHo[h - 1] == 0)
            {
                regCelda.ListaGrupocodimodo[h - 1] = grupocodimodo;
                regCelda.ListaGruponomb[h - 1] = regModo.Gruponomb;
                regCelda.ListaCalifHo[h - 1] = subcausacodi;
                regCelda.ListaSubcausadesc[h - 1] = regSubcausa != null ? regSubcausa.Subcausadesc : "";
            }

            //asginar mwh x modo
            if (regCelda.ListaCalifHo[h - 1] != 0)
            {
                string descCalif = "";

                regCelda.ListaCalifHoDesc[h - 1] = descCalif;
                regCelda.ListaMWHo[h - 1] = 1.0m; //flag existe hora de operación
            }
        }

        /// <summary>
        /// Agrupar las horas de operación por modo de operación
        /// </summary>
        /// <param name="listaHOP"></param>
        /// <param name="listaUnidad"></param>
        /// <param name="listaGrupoModo"></param>
        /// <returns></returns>
        private List<Detalle96HoraOperacionUnidad> ListarDetalleHO(List<EveHoraoperacionDTO> listaHOP, List<EqEquipoDTO> listaUnidad, List<PrGrupoDTO> listaGrupoModo)
        {
            List<EveHoraoperacionDTO> listaHoModo = listaHOP.Where(x => x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoModo).OrderBy(x => x.Hophorini).ToList();
            List<EveHoraoperacionDTO> listaHoUnidad = listaHOP.Where(x => x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoUnidad).OrderBy(x => x.Hophorini).ToList();

            //
            List<Detalle96HoraOperacionUnidad> lista = new List<Detalle96HoraOperacionUnidad>();

            foreach (var reg in listaUnidad)
            {
                var sublistaHoCentral = listaHoModo.Where(x => x.Equipadre == reg.Equicodi).OrderBy(x => x.Hophorini).ToList();
                if (sublistaHoCentral.Any())
                {
                    List<string> listaHOdesc = sublistaHoCentral.Select(x => GetDescripcionHo(x)).ToList();

                    lista.Add(new Detalle96HoraOperacionUnidad()
                    {
                        Gruponomb = reg.Gruponomb,
                        Grupocodi = reg.Grupocodi ?? 0,
                        ListaHO = sublistaHoCentral,
                        ListaHODesc = listaHOdesc,
                        Comentario = string.Join("\n\n", listaHOdesc)
                    });
                }
                else
                {
                    var sublistaHoEq = listaHoUnidad.Where(x => x.Equicodi == reg.Equicodi).OrderBy(x => x.Hophorini).ToList();
                    List<string> listaHOEqdesc = new List<string>();
                    foreach (var ho in sublistaHoEq)
                    {
                        var hoPadre = listaHoModo.Find(x => x.Hopcodi == ho.Hopcodipadre);
                        ho.Subcausadesc = hoPadre != null ? hoPadre.Subcausadesc : "";
                        ho.Gruponomb = hoPadre != null ? hoPadre.Gruponomb : "";
                        listaHOEqdesc.Add(GetDescripcionHo(ho));
                    }

                    lista.Add(new Detalle96HoraOperacionUnidad()
                    {
                        Gruponomb = "", //reg.Gruponomb + " (" + reg.ListaEquiabrev[i] + ")",
                        Grupocodi = reg.Grupocodi ?? 0,
                        Equicodi = reg.Equicodi,
                        ListaHO = sublistaHoEq,
                        ListaHODesc = listaHOEqdesc,
                        Comentario = string.Join("\n\n", listaHOEqdesc)
                    });

                }
            }

            return lista;
        }

        /// <summary>
        /// ClonarM96
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        private List<MeMedicion96DTO> ClonarM96(List<MeMedicion96DTO> lista)
        {
            List<MeMedicion96DTO> l = new List<MeMedicion96DTO>();

            foreach (var reg in lista)
            {
                var regClone = new MeMedicion96DTO()
                {
                    Medifecha = reg.Medifecha,
                    Ptomedicodi = reg.Ptomedicodi,
                    Emprcodi = reg.Emprcodi,
                    Central = (reg.Central ?? "").Trim(),
                    Equipadre = reg.Equipadre,
                    Equicodi = reg.Equicodi,
                    Grupocodi = reg.Grupocodi
                };
                for (int h = 1; h <= 96; h++)
                {

                    decimal? valorH = (decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg, null);

                    if (valorH > 0)
                    {
                        regClone.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).SetValue(regClone, valorH);
                    }
                }

                l.Add(regClone);
            }

            return l;
        }

        /// <summary>
        /// Obtener descripción de la hora de operación (aparece como comentario en excel)
        /// </summary>
        /// <param name="reg"></param>
        /// <returns></returns>
        private string GetDescripcionHo(EveHoraoperacionDTO reg)
        {
            string separador = "  > ";
            string desc = (reg.Gruponomb ?? "").Trim() + separador + reg.Hophorini.Value.ToString(ConstantesAppServicio.FormatoHHmmss) + " - " + reg.Hophorfin.Value.ToString(ConstantesAppServicio.FormatoHHmmss);

            desc += string.Format("{0} {1}", separador, reg.Subcausadesc);

            //
            if (!string.IsNullOrEmpty(reg.HophorordarranqDesc))
                desc += string.Format("{0} {1}: {2}", separador, "O. Arranque", reg.HophorordarranqDesc);
            if (!string.IsNullOrEmpty(reg.HophorparadaDesc))
                desc += string.Format("{0} {1}: {2}", separador, "O. Parada", reg.HophorparadaDesc);

            //if (!string.IsNullOrEmpty(reg.Gruponomb))
            //    desc += string.Format(" {0} {1}: {2}", separador, "Modo de operación", reg.Gruponomb.Trim());

            //
            if (!string.IsNullOrEmpty(reg.HopensayopeDesc))
                desc += string.Format("{0} {1}: {2}", separador, "Ensayo de Potencia efectiva", reg.HopensayopeDesc);
            if (!string.IsNullOrEmpty(reg.HopsaisladoDesc))
                desc += string.Format("{0} {1}: {2}", separador, "Sistema aislado", reg.HopsaisladoDesc);

            //
            if (reg.Hopcausacodi > 0)
                desc += string.Format("{0} {1}: {2}", separador, " Motivo Operación Forzada", reg.HopcausacodiDesc);
            if (!string.IsNullOrEmpty(reg.HoplimtransDesc))
                desc += string.Format("{0} {1}", separador, reg.HoplimtransDesc);
            if (!string.IsNullOrEmpty(reg.HopcompordarrqDesc))
                desc += string.Format("{0} {1}: {2}", separador, "Compensar O. Arranque", "SÍ");
            if (!string.IsNullOrEmpty(reg.HopcompordpardDesc))
                desc += string.Format("{0} {1}: {2}", separador, "Compensar O. Parada", "SÍ");

            if (!string.IsNullOrEmpty(reg.Hopdesc))
                desc += string.Format("{0} {1}: {2}", separador, "Descripción", reg.Hopdesc);
            if (!string.IsNullOrEmpty(reg.Hopobs))
                desc += string.Format("{0} {1}: {2}", separador, "Observación del agente", reg.Hopobs);


            //if (!string.IsNullOrEmpty(reg.LastdateDesc))
            //    desc += string.Format("{0} {1} {2}", separador, "Registrado por ", reg.Lastuser + " " + reg.LastdateDesc);

            return desc;
        }

        /// <summary>
        /// GenerarExcelLogErroresHOP
        /// </summary>
        /// <param name="listaLog"></param>
        /// <param name="usuario"></param>
        /// <param name="fechaConsulta"></param>
        /// <param name="pathLogo"></param>
        /// <param name="strEmpresa"></param>
        /// <param name="strMes"></param>
        /// <param name="listaHop"></param>
        /// <returns></returns>
        public string GenerarExcelLogErroresHOP(List<LogErrorHOPvsMedidores> listaLog, string usuario, DateTime fechaConsulta, string pathLogo,
                                            string strEmpresa, string strMes, List<ReporteHoraoperacionDTO> listaHop)
        {
            string fileExcel = string.Empty;

            using (ExcelPackage xlPackage = new ExcelPackage())
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Log de Errores");

                //Logo
                UtilExcel.AddImageLocal(ws, 1, 0, pathLogo);

                //Armando tabla de identificador de usuario y fecha de modificación
                int row = 4;
                ws.Cells[row, 1].Value = "LOG DE ERRORES";
                UtilExcel.CeldasExcelAgrupar(ws, row, 1, row, 8);
                UtilExcel.SetFormatoCelda(ws, row, 1, row, 8, "Centro", "Centro", "#FFFFFF", "#227ABE", "Calibri", 10, true);

                ws.Cells[++row, 1].Value = "Usuario: " + usuario;
                ws.Cells[++row, 1].Value = "Fecha y Hora: " + fechaConsulta.ToString(ConstantesAppServicio.FormatoFechaFull2);
                ws.Cells[++row, 1].Value = "Para consultas enviar correo a: sgi@coes.org.pe";
                ws.Cells[row, 1].Style.Font.Bold = true;

                int rowCab = row + 2;
                ws.Cells[rowCab, 1].Value = "Línea";
                ws.Cells[rowCab, 2].Value = "Inicio";
                ws.Cells[rowCab, 3].Value = "Fin";
                ws.Cells[rowCab, 4].Value = "Empresa";
                ws.Cells[rowCab, 5].Value = "Central";
                ws.Cells[rowCab, 6].Value = "Grupo";
                ws.Cells[rowCab, 7].Value = "Modo de Operacion";
                ws.Cells[rowCab, 8].Value = "Descripción del Error";

                ExcelRange rg = ws.Cells[rowCab, 1, rowCab, 8];
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#227ABE"));
                rg.Style.Font.Color.SetColor(Color.White);
                rg.Style.Font.Size = 10;
                rg.Style.Font.Bold = true;

                ws.View.FreezePanes(rowCab + 1, 3 + 1);

                row = rowCab + 1;
                foreach (var item in listaLog)
                {
                    ws.Cells[row, 1].Value = item.Linea;
                    ws.Cells[row, 2].Value = item.FechaInicio;
                    ws.Cells[row, 3].Value = item.FechaFin;
                    ws.Cells[row, 4].Value = item.Empresa;
                    ws.Cells[row, 5].Value = item.Central;
                    ws.Cells[row, 6].Value = item.Grupo;
                    ws.Cells[row, 7].Value = item.ModoOperacion;
                    ws.Cells[row, 8].Value = item.DescripcionError;
                    UtilExcel.CeldasExcelAlinearHorizontalmente(ws, row, 1, row, 8, "Centro");

                    row++;
                }

                int rowFin = row > rowCab + 1 ? row - 1 : row;

                rg = ws.Cells[rowCab, 1, rowFin, 8];
                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                ws.Column(1).Width = 10; //Linea
                ws.Column(2).Width = 17; //FechaInicio
                ws.Column(3).Width = 17; //FechaFin
                ws.Column(4).Width = 30; //Empresa
                ws.Column(5).Width = 20; //Central
                ws.Column(6).Width = 20; //Grupo
                ws.Column(7).Width = 30; //ModoOperacion
                ws.Column(8).Width = 100; //DescripcionError

                ExcelWorksheet ws1 = xlPackage.Workbook.Worksheets.Add("HOP");
                HojaExcelReporteHOP(ws1, pathLogo, strEmpresa, strMes, listaHop);

                //generar archivo temporal
                fileExcel = System.IO.Path.GetTempFileName();
                xlPackage.SaveAs(new FileInfo(fileExcel));
            }

            return fileExcel;
        }

        /// <summary>
        /// GenerarExcelReporteHOP
        /// </summary>
        /// <param name="strEmpresa"></param>
        /// <param name="strMes"></param>
        /// <param name="listaData"></param>
        /// <param name="pathLogo"></param>
        /// <returns></returns>
        public string GenerarExcelReporteHOP(string strEmpresa, string strMes, List<ReporteHoraoperacionDTO> listaData, string pathLogo)
        {
            string fileExcel = string.Empty;

            using (ExcelPackage xlPackage = new ExcelPackage())
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Reporte HOP");
                HojaExcelReporteHOP(ws, pathLogo, strEmpresa, strMes, listaData);

                fileExcel = System.IO.Path.GetTempFileName();
                xlPackage.SaveAs(new FileInfo(fileExcel));
            }

            return fileExcel;
        }

        /// <summary>
        /// HojaExcelReporteHOP
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="pathLogo"></param>
        /// <param name="strEmpresa"></param>
        /// <param name="strMes"></param>
        /// <param name="listaData"></param>
        private void HojaExcelReporteHOP(ExcelWorksheet ws, string pathLogo, string strEmpresa, string strMes, List<ReporteHoraoperacionDTO> listaData)
        {
            //Logo
            UtilExcel.AddImageLocal(ws, 1, 0, pathLogo);

            int col = 2;
            int row = 1;

            #region filtro
            int rowTitulo = 2;
            ws.Cells[rowTitulo, col + 1].Value = "Reporte de Horas de Operación";
            ws.Cells[rowTitulo, col + 1].Style.Font.Bold = true;

            row = 5;

            int rowIniEmpresa = row;
            int rowIniMes = rowIniEmpresa + 1;
            int colIniEmpresa = col;
            int colIniMes = colIniEmpresa;

            ws.Cells[rowIniEmpresa, colIniEmpresa].Value = "Empresa";
            ws.Cells[rowIniEmpresa, colIniEmpresa + 1].Value = strEmpresa;

            ws.Cells[rowIniMes, colIniMes].Value = "Mes";
            ws.Cells[rowIniMes, colIniMes + 1].Value = strMes;

            using (var range = ws.Cells[rowIniEmpresa, colIniEmpresa, rowIniMes, colIniEmpresa])
            {
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Color.SetColor(Color.Black);
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Color.SetColor(Color.Black);
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Top.Color.SetColor(Color.Black);
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Color.SetColor(Color.Black);

                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                range.Style.Font.Color.SetColor(Color.White);
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }
            row = rowIniMes;
            #endregion
            row += 2;

            #region cabecera
            int rowIniCabecera = row;
            int colIniCentral = col;
            int colIniUnidad = colIniCentral + 1;
            int colIniModo = colIniUnidad + 1;
            int colIniFechaIni = colIniModo + 1;
            int colIniFechaFin = colIniFechaIni + 1;

            ws.Cells[rowIniCabecera, colIniCentral].Value = "Central";
            ws.Cells[rowIniCabecera, colIniUnidad].Value = "Unidad";
            ws.Cells[rowIniCabecera, colIniModo].Value = "Modo de Operación";
            ws.Cells[rowIniCabecera, colIniFechaIni].Value = "Fecha Inicio";
            ws.Cells[rowIniCabecera, colIniFechaFin].Value = "Fecha Fin";

            using (var range = ws.Cells[rowIniCabecera, colIniCentral, rowIniCabecera, colIniFechaFin])
            {
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Color.SetColor(Color.Black);
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Color.SetColor(Color.Black);
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Top.Color.SetColor(Color.Black);
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Color.SetColor(Color.Black);

                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                range.Style.Font.Color.SetColor(Color.White);
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }
            row = rowIniCabecera;
            #endregion

            row += 1;

            #region cuerpo
            var data = listaData;
            var listaCentral = data.Select(y => new { y.IdCentral, y.Central }).Distinct().ToList().OrderBy(c => c.Central);
            var listaCentralcodi = listaCentral.Select(x => x.IdCentral).ToList();
            foreach (var centralcodi in listaCentralcodi)
            {
                //central
                string nomCentral = data.Where(x => x.IdCentral == centralcodi).FirstOrDefault().Central;
                int cantUnidadPorCentral = data.Where(x => x.IdCentral == centralcodi).Count();

                ws.Cells[row, colIniCentral].Value = nomCentral;
                ws.Cells[row, colIniCentral, row + cantUnidadPorCentral - 1, colIniCentral].Merge = true;
                ws.Cells[row, colIniCentral, row + cantUnidadPorCentral - 1, colIniCentral].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[row, colIniCentral, row + cantUnidadPorCentral - 1, colIniCentral].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //Unidad
                var listaUnidad = data.Where(x => x.IdCentral == centralcodi).Select(y => new { y.IdUnidad, y.Unidad }).Distinct().ToList().OrderBy(c => c.Unidad);
                var listaUnidadcodi = listaUnidad.Select(x => x.IdUnidad).ToList();
                for (int u = 0; u < listaUnidadcodi.Count; u++)
                {
                    if (u != 0)
                    {
                        row++;
                    }

                    var unidadcodi = listaUnidadcodi[u];
                    var nomUnidad = data.Where(x => x.IdCentral == centralcodi && x.IdUnidad == unidadcodi).FirstOrDefault().Unidad;
                    int cantModoOpGrupoPorUnidad = data.Where(x => x.IdCentral == centralcodi && x.IdUnidad == unidadcodi).Count();

                    ws.Cells[row, colIniUnidad].Value = nomUnidad;
                    ws.Cells[row, colIniUnidad, row + cantModoOpGrupoPorUnidad - 1, colIniUnidad].Merge = true;
                    ws.Cells[row, colIniUnidad, row + cantModoOpGrupoPorUnidad - 1, colIniUnidad].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[row, colIniUnidad, row + cantModoOpGrupoPorUnidad - 1, colIniUnidad].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    //Modo de operación - grupo
                    var listaModo = data.Where(x => x.IdCentral == centralcodi && x.IdUnidad == unidadcodi).Select(y => new { y.IdModoOpGrupo, y.ModoOpGrupo }).Distinct().ToList().OrderBy(c => c.ModoOpGrupo); ;
                    var listaModocodi = listaModo.Select(x => x.IdModoOpGrupo).ToList();
                    for (int y = 0; y < listaModocodi.Count; y++)
                    {
                        if (y != 0)
                        {
                            row++;
                        }
                        var modocodi = listaModocodi[y];
                        var nomModoOp = data.Where(x => x.IdCentral == centralcodi && x.IdUnidad == unidadcodi && x.IdModoOpGrupo == modocodi).FirstOrDefault().ModoOpGrupo;
                        var listaHOP = data.Where(x => x.IdCentral == centralcodi && x.IdUnidad == unidadcodi && x.IdModoOpGrupo == modocodi).ToList();
                        int cantHOPPorModo = listaHOP.Count;

                        ws.Cells[row, colIniModo].Value = nomModoOp;
                        ws.Cells[row, colIniModo, row + cantHOPPorModo - 1, colIniModo].Merge = true;
                        ws.Cells[row, colIniModo, row + cantHOPPorModo - 1, colIniModo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[row, colIniModo, row + cantHOPPorModo - 1, colIniModo].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        for (int z = 0; z < cantHOPPorModo; z++)
                        {
                            if (z != 0)
                            {
                                row++;
                            }
                            var hop = listaHOP[z];

                            ws.Cells[row, colIniFechaIni].Value = hop.FechaIni;
                            ws.Cells[row, colIniFechaIni].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[row, colIniFechaFin].Value = hop.FechaFin;
                            ws.Cells[row, colIniFechaFin].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        }

                    }
                }
                row++;
            }
            #endregion

            //ancho de columnas
            ws.Column(1).Width = 3;
            ws.Column(colIniCentral).Width = 30;
            ws.Column(colIniUnidad).Width = 25;
            ws.Column(colIniModo).Width = 25;
            ws.Column(colIniFechaIni).Width = 25;
            ws.Column(colIniFechaFin).Width = 25;
        }

        #endregion
    }

    /// <summary>
    /// Clase que guarda los datos de cálculo
    /// </summary>
    public class CeldaMW96xUnidad
    {
        public DateTime Fecha { get; set; }
        public EqEquipoDTO UnidadTermico { get; set; }
        public int Equipadre { get; set; }
        public Detalle96HoraOperacionUnidad DetalleHO { get; set; }

        public decimal?[] ListaMW { get; set; }
        public decimal?[] ListaMWHo { get; set; }
        public string[] ListaMensaje { get; set; }

        public int[] ListaGrupocodimodo { get; set; }
        public string[] ListaGruponomb { get; set; }
        public int[] ListaCalifHo { get; set; }
        public string[] ListaCalifHoDesc { get; set; }
        public string[] ListaSubcausadesc { get; set; }

        public CeldaMW96xUnidad()
        {
            ListaMW = new decimal?[96];
            ListaMWHo = new decimal?[96];
            ListaMensaje = new string[96];

            ListaGrupocodimodo = new int[96];
            ListaGruponomb = new string[96];
            ListaCalifHo = new int[96];
            ListaCalifHoDesc = new string[96];
            ListaSubcausadesc = new string[96];

            DetalleHO = new Detalle96HoraOperacionUnidad();
        }
    }

    /// <summary>
    /// Clase que almacena las horas de operacion por cada unidad 
    /// </summary>
    public class Detalle96HoraOperacionUnidad
    {
        public int Equicodi { get; set; }
        public int Equipadre { get; set; }
        public int Grupocodi { get; set; }
        public string Gruponomb { get; set; }
        public List<EveHoraoperacionDTO> ListaHO { get; set; }
        public List<string> ListaHODesc { get; set; }
        public string Comentario { get; set; }

        public Detalle96HoraOperacionUnidad()
        {
            ListaHO = new List<EveHoraoperacionDTO>();
            ListaHODesc = new List<string>();
        }
    }

    /// <summary>
    /// Clase log de errores
    /// </summary>
    public class LogErrorHOPvsMedidores
    {
        public int FilaIni { get; set; }
        public int FilaFin { get; set; }
        public string Linea { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string Empresa { get; set; }
        public string Central { get; set; }
        public string Grupo { get; set; }
        public string Equipo { get; set; }
        public string ModoOperacion { get; set; }
        public string DescripcionError { get; set; }

        public string FechaH { get; set; }
        public string PotenciaH { get; set; }
        public int Ptomedicodi { get; set; }
        public int Emprcodi { get; set; }
        public int Equicodi { get; set; }
        public int GrupocodiDespacho { get; set; }
        public int GrupocodiModo { get; set; }
    }

}
