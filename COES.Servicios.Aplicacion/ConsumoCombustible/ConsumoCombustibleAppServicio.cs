using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.Equipamiento.Helper;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Indisponibilidades;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using COES.Servicios.Aplicacion.Migraciones.Helper;
using COES.Servicios.Aplicacion.StockCombustibles;
using log4net;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace COES.Servicios.Aplicacion.ConsumoCombustible
{
    public class ConsumoCombustibleAppServicio : AppServicioBase
    {
        INDAppServicio indServ = new INDAppServicio();
        HorasOperacionAppServicio servHO = new HorasOperacionAppServicio();
        EjecutadoAppServicio servEjec = new EjecutadoAppServicio();

        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ConsumoCombustibleAppServicio));

        #region Métodos Tabla PR_GRUPOEQ

        /// <summary>
        /// Permite realizar búsquedas en la tabla PrGrupoeq
        /// </summary>
        public List<PrGrupoeqDTO> GetByCriteriaPrGrupoeqs(int grupocodi, int equipadre)
        {
            return FactorySic.GetPrGrupoeqRepository().GetByCriteria(grupocodi, equipadre).Where(x => x.Geqactivo == 1).ToList();
        }

        #endregion

        #region Métodos Tabla CCC_REPORTE

        /// <summary>
        /// Inserta un registro de la tabla CCC_REPORTE
        /// </summary>
        public int SaveCccReporte(CccReporteDTO entity, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                return FactorySic.GetCccReporteRepository().Save(entity, conn, tran);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CCC_REPORTE
        /// </summary>
        public void UpdateCccReporte(CccReporteDTO entity)
        {
            try
            {
                FactorySic.GetCccReporteRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CCC_REPORTE
        /// </summary>
        public void DeleteCccReporte(int cccrptcodi)
        {
            try
            {
                FactorySic.GetCccReporteRepository().Delete(cccrptcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CCC_REPORTE
        /// </summary>
        public CccReporteDTO GetByIdCccReporte(int cccrptcodi)
        {
            return FactorySic.GetCccReporteRepository().GetById(cccrptcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CCC_REPORTE
        /// </summary>
        public List<CccReporteDTO> ListCccReportes()
        {
            return FactorySic.GetCccReporteRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CccReporte
        /// </summary>
        public List<CccReporteDTO> GetByCriteriaCccReportes(string cccvercodi)
        {
            return FactorySic.GetCccReporteRepository().GetByCriteria(cccvercodi);
        }

        #endregion

        #region Métodos Tabla CCC_VERSION

        /// <summary>
        /// Inserta un registro de la tabla CCC_VERSION
        /// </summary>
        public int SaveCccVersion(CccVersionDTO entity, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                return FactorySic.GetCccVersionRepository().Save(entity, conn, tran);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CCC_VERSION
        /// </summary>
        public void UpdateCccVersion(CccVersionDTO entity)
        {
            try
            {
                FactorySic.GetCccVersionRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CCC_VERSION
        /// </summary>
        public void DeleteCccVersion(int cccvercodi)
        {
            try
            {
                FactorySic.GetCccVersionRepository().Delete(cccvercodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CCC_VERSION
        /// </summary>
        public CccVersionDTO GetByIdCccVersion(int cccvercodi)
        {
            var reg = FactorySic.GetCccVersionRepository().GetById(cccvercodi);
            FormatearCCCVersion(reg);

            return reg;
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CCC_VERSION
        /// </summary>
        public List<CccVersionDTO> ListCccVersions()
        {
            return FactorySic.GetCccVersionRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CccVersion
        /// </summary>
        public List<CccVersionDTO> GetByCriteriaCccVersions(DateTime fecha, DateTime fechaFin, string horizonte)
        {
            var lista = FactorySic.GetCccVersionRepository().GetByCriteria(fecha, fechaFin, horizonte).OrderBy(x => x.Cccvernumero).ToList();

            foreach (var reg in lista)
            {
                FormatearCCCVersion(reg);
            }

            return lista;
        }

        private void FormatearCCCVersion(CccVersionDTO reg)
        {
            reg.CccverfeccreacionDesc = reg.Cccverfeccreacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull);
            reg.CccverfecmodificacionDesc = reg.Cccverfecmodificacion != null ? reg.Cccverfecmodificacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull) : string.Empty;

            if (reg.Cccverhorizonte == ConstantesConsumoCombustible.HorizonteMensual)
                reg.CccverfechaDesc = reg.Cccverfecha.ToString(ConstantesAppServicio.FormatoMes);
            else
                reg.CccverfechaDesc = reg.Cccverfecha.ToString(ConstantesAppServicio.FormatoFecha);

            reg.Cccverobs = !string.IsNullOrEmpty(reg.Cccverobs) ? reg.Cccverobs.Trim() : string.Empty;
            reg.ListaObs = reg.Cccverobs.Split(new string[] { ConstantesConsumoCombustible.SeparadorObs }, StringSplitOptions.None).ToList();
            reg.ListaObs = reg.ListaObs.Where(x => x != "").ToList();
        }

        #endregion

        #region Métodos Tabla CCC_VCOM

        /// <summary>
        /// Inserta un registro de la tabla CCC_VCOM
        /// </summary>
        public int SaveCccVcom(CccVcomDTO entity, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                return FactorySic.GetCccVcomRepository().Save(entity, conn, tran);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CCC_VCOM
        /// </summary>
        public void UpdateCccVcom(CccVcomDTO entity)
        {
            try
            {
                FactorySic.GetCccVcomRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CCC_VCOM
        /// </summary>
        public void DeleteCccVcom(int vcomcodi)
        {
            try
            {
                FactorySic.GetCccVcomRepository().Delete(vcomcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CCC_VCOM
        /// </summary>
        public CccVcomDTO GetByIdCccVcom(int vcomcodi)
        {
            return FactorySic.GetCccVcomRepository().GetById(vcomcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CCC_VCOM
        /// </summary>
        public List<CccVcomDTO> ListCccVcoms()
        {
            return FactorySic.GetCccVcomRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CccVcom
        /// </summary>
        public List<CccVcomDTO> GetByCriteriaCccVcoms(int cccvercodi)
        {
            return FactorySic.GetCccVcomRepository().GetByCriteria(cccvercodi);
        }

        /// <summary>
        /// Formatear los reportes
        /// </summary>
        /// <param name="reg"></param>
        /// <param name="listaLeyenda"></param>
        private void FormatearCCCVCOM(CccVcomDTO reg)
        {
            NumberFormatInfo nfi4 = new CultureInfo("en-US", false).NumberFormat;
            nfi4.NumberGroupSeparator = " ";
            nfi4.NumberDecimalDigits = 4;
            nfi4.NumberDecimalSeparator = ",";

            NumberFormatInfo nfi2 = new CultureInfo("en-US", false).NumberFormat;
            nfi2.NumberGroupSeparator = " ";
            nfi2.NumberDecimalDigits = 2;
            nfi2.NumberDecimalSeparator = ",";

            reg.VcomvalorDesc = reg.Vcomvalor.GetValueOrDefault(0) != 0 ? reg.Vcomvalor.Value.ToString("N", nfi4) : string.Empty;
        }

        #endregion

        #region Métodos Tabla REP_VCOM

        /// <summary>
        /// Inserta un registro de la tabla REP_VCOM
        /// </summary>
        public void SaveRepVcom(RepVcomDTO entity)
        {
            try
            {
                FactorySic.GetRepVcomRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla REP_VCOM
        /// </summary>
        public void UpdateRepVcom(RepVcomDTO entity)
        {
            try
            {
                FactorySic.GetRepVcomRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla REP_VCOM
        /// </summary>
        public void DeleteRepVcom(int periodo)
        {
            try
            {
                FactorySic.GetRepVcomRepository().Delete(periodo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla REP_VCOM
        /// </summary>
        public RepVcomDTO GetByIdRepVcom(int periodo, string codigomodooperacion, string codigotipocombustible)
        {
            return FactorySic.GetRepVcomRepository().GetById(periodo, codigomodooperacion, codigotipocombustible);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla REP_VCOM
        /// </summary>
        public List<RepVcomDTO> ListRepVcoms()
        {
            return FactorySic.GetRepVcomRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla RepVcom
        /// </summary>
        public List<RepVcomDTO> GetByCriteriaRepVcoms()
        {
            return FactorySic.GetRepVcomRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla SI_FACTORCONVERSION

        /// <summary>
        /// Inserta un registro de la tabla SI_FACTORCONVERSION
        /// </summary>
        public void SaveSiFactorconversion(SiFactorconversionDTO entity)
        {
            try
            {
                FactorySic.GetSiFactorconversionRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SI_FACTORCONVERSION
        /// </summary>
        public void UpdateSiFactorconversion(SiFactorconversionDTO entity)
        {
            try
            {
                FactorySic.GetSiFactorconversionRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SI_FACTORCONVERSION
        /// </summary>
        public void DeleteSiFactorconversion(int tconvcodi)
        {
            try
            {
                FactorySic.GetSiFactorconversionRepository().Delete(tconvcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SI_FACTORCONVERSION
        /// </summary>
        public SiFactorconversionDTO GetByIdSiFactorconversion(int tconvcodi)
        {
            return FactorySic.GetSiFactorconversionRepository().GetById(tconvcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SI_FACTORCONVERSION
        /// </summary>
        public List<SiFactorconversionDTO> ListSiFactorconversions()
        {
            return FactorySic.GetSiFactorconversionRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SiFactorconversion
        /// </summary>
        public List<SiFactorconversionDTO> GetByCriteriaSiFactorconversions()
        {
            return FactorySic.GetSiFactorconversionRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla SI_FUENTEENERGIA

        /// <summary>
        /// Actualiza un registro de la tabla SI_FUENTEENERGIA
        /// </summary>
        public void UpdateSiFuenteenergia(SiFuenteenergiaDTO entity)
        {
            try
            {
                FactorySic.GetSiFuenteenergiaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SI_FUENTEENERGIA
        /// </summary>
        public SiFuenteenergiaDTO GetByIdSiFuenteenergia(int fenergcodi)
        {
            return FactorySic.GetSiFuenteenergiaRepository().GetById(fenergcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SI_FUENTEENERGIA
        /// </summary>
        public List<SiFuenteenergiaDTO> ListSiFuenteenergias()
        {
            return FactorySic.GetSiFuenteenergiaRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SiFuenteenergia
        /// </summary>
        public List<SiFuenteenergiaDTO> GetByCriteriaSiFuenteenergias()
        {
            return FactorySic.GetSiFuenteenergiaRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla SI_TIPOINFORMACION

        /// <summary>
        /// Permite listar todos los registros de la tabla SI_TIPOINFORMACION
        /// </summary>
        public List<SiTipoinformacionDTO> ListSiTipoinformacions()
        {
            return FactorySic.GetSiTipoinformacionRepository().List().Where(x => x.Tipoinfocodi > 0).OrderBy(x => x.Tipoinfoabrev).ToList();
        }

        #endregion

        #region ALGORITMO CCC: Cálculo de Consumo de Combustible

        /// <summary>
        /// Funcion que obtiene los datos a guardar y también para validar en extranet
        /// </summary>
        /// <param name="fechaPeriodo"></param>
        /// <param name="emprcodi">por defecto -1: todas las empresas</param>
        /// <returns></returns>
        public List<CccReporteDTO> ListarReporteCCC(DateTime fechaPeriodo, string emprcodis, string equipadres, out ReporteConsumoCombustible objDatosReporte)
        {
            //mensajes
            List<ResultadoValidacionAplicativo> listaMsj = new List<ResultadoValidacionAplicativo>();

            #region Insumos

            emprcodis = !string.IsNullOrEmpty(emprcodis) ? emprcodis : ConstantesAppServicio.ParametroDefecto;
            equipadres = !string.IsNullOrEmpty(equipadres) ? equipadres : ConstantesAppServicio.ParametroDefecto;

            /// Lista de unidades (incluye con combustible secundario), grupos y modos
            indServ.ListarUnidadTermicoCCC(fechaPeriodo, fechaPeriodo, emprcodis, equipadres, out List<EqEquipoDTO> listaUnidad
                                            , out List<EqEquipoDTO> listaEquiposTermicos
                                            , out List<PrGrupoDTO> listaGrupoModo, out List<PrGrupoDTO> listaAllGrupo
                                            , out List<ResultadoValidacionAplicativo> listaMsj3);
            listaMsj.AddRange(listaMsj3);

            //Tvs
            List<int> listaEquicodiTv = listaUnidad.Select(x => x.EquicodiTVCicloComb).Where(x => x > 0).Distinct().ToList();

            EqEquipoDTO eqCTVentGas = listaUnidad.Find(x => x.Equicodi == 290 && x.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiGas);

            //solo ciclo simple o especiales aparecen en el reporte, quitar la central gas Ventanilla pero mantener su ciclo simple
            listaUnidad = listaUnidad.Where(x => !x.TieneCicloComb && !(x.Equicodi == 290 && x.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiGas)).ToList();

            List<EqEquipoDTO> listaEqGen = listaEquiposTermicos.Where(x => x.Famcodi == ConstantesHorasOperacion.IdGeneradorTemoelectrico).ToList();

            // Horas de Operacion
            List<EveHoraoperacionDTO> listaHO = ListarHo(fechaPeriodo, emprcodis, equipadres, listaGrupoModo);

            // Despacho Extranet 
            List<MeMedicion48DTO> lista48xGen = Listar48DespachoExtranet(fechaPeriodo, emprcodis, equipadres, listaEqGen, out List<ResultadoValidacionAplicativo> listaMsj6);
            listaMsj.AddRange(listaMsj6);

            // Consumo de Combustible Real
            List<MeMedicionxintervaloDTO> listaConsumoEjec = ListarMxInt(fechaPeriodo, emprcodis);

            // Curva de Consumo de Combustible vs Potencia
            List<ConsumoHorarioCombustible> listaCurva = ListarCurvaConsumoCombPotencia(fechaPeriodo, listaGrupoModo, out List<ResultadoValidacionAplicativo> listaMsj5);
            listaMsj.AddRange(listaMsj5);

            //Tipo de medida
            List<SiFuenteenergiaDTO> listaFenerg = GetByCriteriaSiFuenteenergias();

            //Actualizar Valores de Consumo alternativo a medida COES
            ActualizarMedidaCombustibleArranque(listaUnidad);

            //Setear valor de rendimiento bagazo
            ActualizarRendimientoBagazo(listaUnidad, fechaPeriodo, out List<ResultadoValidacionAplicativo> listaMsj20);
            listaMsj.AddRange(listaMsj20);

            //Obtener versión y detalle del día anterior
            DateTime fechaPeriodoAyer = fechaPeriodo.AddDays(-1);
            List<CccVersionDTO> listaVersionAyer = GetByCriteriaCccVersions(fechaPeriodoAyer, fechaPeriodoAyer, ConstantesConsumoCombustible.HorizonteDiario);
            CccVersionDTO objVersionAyer = listaVersionAyer.LastOrDefault();

            List<CccReporteDTO> listaRepdetAyer = new List<CccReporteDTO>();
            if (objVersionAyer != null) listaRepdetAyer = GetByCriteriaCccReportes(objVersionAyer.Cccvercodi.ToString());

            #endregion

            #region Calculo

            //horas de operación por cada modo
            //List<DetalleHoraOperacionModo> listaDetalleHO = ListarDetalleHO(listaHO, listaGrupoModo);

            //horas de operación
            List<EveHoraoperacionDTO> listaHoCS = ListarHOCCtoCicloSimple(listaHO, listaUnidad);
            List<DetalleHoraOperacionModo> listaDetalleHOCs = ListarDetalleHO(listaHoCS, listaGrupoModo);


            List<CeldaMWxModo> listaCelda = new List<CeldaMWxModo>();

            //obtener el detalle por unidad - Parte 1
            var lista48Clone1 = ClonarM48(lista48xGen);

            List<CeldaMWxModo> listaCelda1 = new List<CeldaMWxModo>();
            List<ResultadoValidacionAplicativo> listaMjs2 = new List<ResultadoValidacionAplicativo>();

            //0- Obtener las horas de operacion cortadas cada 30 min
            List<EveHoraoperacionDTO> listaHOModoCS = listaHoCS.Where(x => x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoModo).ToList();
            List<EveHoraoperacionDTO> listaHOUnidadCS = listaHoCS.Where(x => x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoUnidad && !listaEquicodiTv.Contains(x.Equicodi.Value)).ToList();
            servHO.ListarHO30minConsumoCombustibleDivididoX(listaHOUnidadCS, fechaPeriodo, out List<EveHoraoperacionDTO> listaHoConCruceCS, out List<EveHoraoperacionDTO> listaHoSinCruceCS);

            var listaHOUnidadConCruce = new List<EveHoraoperacionDTO>();
            listaHOUnidadConCruce.AddRange(listaHOModoCS);
            listaHOUnidadConCruce.AddRange(listaHoConCruceCS);

            var listaHOUnidadSinCruce = new List<EveHoraoperacionDTO>();
            listaHOUnidadSinCruce.AddRange(listaHOModoCS);
            listaHOUnidadSinCruce.AddRange(listaHoSinCruceCS);

            //1- Calcular celdas 30 min con horas de operación (datos instantaneos)
            ListaCeldaReporte48(fechaPeriodo, listaUnidad, ref lista48Clone1, listaHOUnidadConCruce, listaGrupoModo, listaCurva, listaDetalleHOCs, listaEquicodiTv
                                                               , ref listaCelda1, ref listaMjs2);
            //2- Validar celdas 30 min con horas de operación que no terminan en multiplo de 30min (última media hora)
            ValidarHoraSinDespachoMenor30min(fechaPeriodo, listaUnidad, lista48Clone1, listaHOUnidadSinCruce, listaGrupoModo, listaEquicodiTv
                                                                , ref listaCelda1, ref listaMjs2);

            listaCelda.AddRange(listaCelda1);
            listaMsj.AddRange(listaMjs2);

            /////////////////////////////////////////////////////////////////////////////
            //Procesar por unidad
            List<CccReporteDTO> listaRpt = new List<CccReporteDTO>();

            //quitar las unidades de ciclo simple de Ventanilla y luego agregar la central para guardar en la bd
            listaUnidad = listaUnidad.Where(x => !(x.Equipadre == 290 && x.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiGas)).ToList();
            if (eqCTVentGas != null)
            {
                listaUnidad.Add(eqCTVentGas);
            }

            foreach (var regUnidad in listaUnidad)
            {
                CccReporteDTO reg = new CccReporteDTO();
                reg.Emprcodi = regUnidad.Emprcodi.Value;
                reg.Emprnomb = regUnidad.Emprnomb;
                reg.Equipadre = regUnidad.Equipadre.Value;
                reg.Central = regUnidad.Central;
                reg.Equicodi = regUnidad.Equicodi;
                reg.Equinomb = regUnidad.Equiabrev;
                reg.Grupocodi = regUnidad.Grupocodi;
                reg.Fenergcodi = regUnidad.Fenergcodi;
                reg.Fenergnomb = regUnidad.Fenergnomb;
                reg.Cccrptflagtienecurva = 1;

                if (reg.Equipadre == 290)
                { }
                if (reg.Equicodi == 35)
                { }

                //Consumo Real por unidad
                reg.Cccrptvalorreal = GetConsumoRealXUnidad(regUnidad.Fenergcodi, regUnidad, listaConsumoEjec);

                #region Consumo teórico y gráfico

                //obtener los modos de operación ciclo simple o especial de la unidad y la potencia asociada
                List<CeldaMWxModo> listaCeldaXUnidad = new List<CeldaMWxModo>();

                if (regUnidad.Equicodi == 290 && regUnidad.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiGas)
                {
                    //CT Ventanilla es un caso especial. El medidor lo tiene a nivel de la central y no por unidad (TG3,TG4)
                    listaCeldaXUnidad = listaCelda.Where(x => x.UnidadTermico.Fenergcodi == regUnidad.Fenergcodi &&
                                                              x.UnidadTermico.Equipadre == regUnidad.Equicodi).ToList();
                }
                else
                {
                    listaCeldaXUnidad = listaCelda.Where(x => x.UnidadTermico.Fenergcodi == regUnidad.Fenergcodi &&
                                                              x.UnidadTermico.Equicodi == regUnidad.Equicodi && x.UnidadTermico.Grupocodi == regUnidad.Grupocodi).ToList();
                }

                //consumo horario por generador
                AsignarConsumoHorarioXUnidad(regUnidad, ref listaCeldaXUnidad, out decimal valorTeorico);
                reg.Cccrptvalorteorico = valorTeorico;

                reg.Mogrupocodi = AsignarModoOperacionRepresentativo(reg.Equicodi, reg.Fenergcodi, listaHO, listaGrupoModo);

                //verificar si tiene alerta de Horas de operación
                reg.TieneAlertaHo = listaCeldaXUnidad.Where(x => x.TieneAlertaHo).Count() > 0;

                #endregion

                listaRpt.Add(reg);

                //Agregar filas de combustible alternativo para el arranque, esto tiene valor TEORICO que ya está en BD
                if (regUnidad.FenergcodiCombAlt > 0)
                {
                    CccReporteDTO regAlt = new CccReporteDTO();
                    regAlt.Emprcodi = regUnidad.Emprcodi.Value;
                    regAlt.Equipadre = regUnidad.Equipadre.Value;
                    regAlt.Equicodi = regUnidad.Equicodi;
                    regAlt.Fenergcodi = regUnidad.FenergcodiCombAlt;
                    regAlt.Cccrptflagtienecurva = ConstantesConsumoCombustible.FlagCombustibleArranque;

                    regAlt.Cccrptvalorreal = GetConsumoRealXUnidad(regUnidad.FenergcodiCombAlt, regUnidad, listaConsumoEjec);

                    if (regUnidad.ConsumoCombAlt > 0)//tiene parametrizacion en la bd
                    {
                        GetDatosHOCombAltArranque(regUnidad.Grupocodi ?? 0, regAlt.Equipadre, regUnidad.Equinomb, fechaPeriodo, listaDetalleHOCs
                                                , out int numArranques, out List<ResultadoValidacionAplicativo> listaMsj2);

                        regAlt.Cccrptvalorteorico = numArranques * regUnidad.ConsumoCombAlt;
                    }

                    listaRpt.Add(regAlt);
                }
            }

            //ajustes de redondeo y presentación
            foreach (var reg in listaRpt)
            {
                SiFuenteenergiaDTO regF = listaFenerg.Find(x => x.Fenergcodi == reg.Fenergcodi);
                reg.Tipoinfocodi = regF.Tinfcoes;

                if (reg.Cccrptvalorreal.GetValueOrDefault(0) <= 0) reg.Cccrptvalorreal = null;
                if (reg.Cccrptvalorteorico.GetValueOrDefault(0) <= 0) reg.Cccrptvalorteorico = null;


                //variacion
                if (reg.Cccrptvalorteorico.GetValueOrDefault(0) != 0)
                {
                    reg.Cccrptvariacion = (reg.Cccrptvalorreal.GetValueOrDefault(0) - reg.Cccrptvalorteorico) / reg.Cccrptvalorteorico;
                    reg.Cccrptvariacion = Math.Round(reg.Cccrptvariacion.Value, 5);

                    if (reg.Cccrptvariacion < -1.0m) reg.Cccrptvariacion = -1.0m;
                    if (reg.Cccrptvariacion > 1.0m) reg.Cccrptvariacion = 1.0m;
                }

                //redondeo
                if (reg.Cccrptvalorteorico.GetValueOrDefault(0) != 0)
                    reg.Cccrptvalorteorico = Math.Round(reg.Cccrptvalorteorico.Value, 5);
                if (reg.Cccrptvalorreal.GetValueOrDefault(0) != 0)
                    reg.Cccrptvalorreal = Math.Round(reg.Cccrptvalorreal.Value, 5);
                if (reg.Cccrptvariacion.GetValueOrDefault(0) != 0)
                    reg.Cccrptvariacion = Math.Round(reg.Cccrptvariacion.Value, 5);

                //Validar con el consumo de ayer
                var objRegUnidadAyer = listaRepdetAyer.Find(x => x.Equicodi == reg.Equicodi && x.Fenergcodi == reg.Fenergcodi);
                if (objRegUnidadAyer != null)
                {
                    if (reg.Cccrptvalorreal > 0 && reg.Cccrptvalorreal == objRegUnidadAyer.Cccrptvalorreal)
                    {
                        listaMsj.Add(new ResultadoValidacionAplicativo()
                        {
                            Equipadre = reg.Equipadre,
                            Equicodi = reg.Equicodi,
                            Fenergcodi = reg.Fenergcodi,
                            Descripcion = string.Format("El generador {0} tiene el mismo consumo real que el día de ayer.", reg.Equinomb)
                        });
                    }
                }
            }

            //agregar mensajes de combustible alternativo
            listaMsj.AddRange(ListarMensajeValidacionConsumoAlternativoArranque(fechaPeriodo, listaRpt, listaDetalleHOCs));

            //formatear mensajes y quitar duplicados
            listaMsj = listaMsj.GroupBy(x => new { x.Equipadre, x.Descripcion }).Select(x => new ResultadoValidacionAplicativo()
            {
                Equipadre = x.Key.Equipadre,
                Descripcion = x.Key.Descripcion,
                Central = x.First().Central,
                Emprnomb = (x.First().Emprnomb ?? "").Trim()
            }).OrderBy(x => x.Emprnomb).ThenBy(x => x.Central).ThenBy(x => x.Descripcion).ToList();

            foreach (var reg in listaMsj)
            {
                var regCentral = listaEquiposTermicos.Find(x => x.Equicodi == reg.Equipadre);
                if (regCentral != null)
                {
                    reg.Central = regCentral.Equinomb;
                    reg.Emprnomb = regCentral.Emprnomb;
                }
            }

            #endregion

            objDatosReporte = new ReporteConsumoCombustible();
            objDatosReporte.ListaUnidad = listaUnidad;
            objDatosReporte.ListaEquiposTermicos = listaEquiposTermicos;
            objDatosReporte.Lista48xGen = lista48xGen;
            objDatosReporte.ListaCurva = listaCurva;
            objDatosReporte.ListaMsj = listaMsj;
            objDatosReporte.ListaDetalleHO = listaDetalleHOCs;
            objDatosReporte.ListaCeldaDetalle = listaCelda1;
            objDatosReporte.ListaCeldaDetalle2 = new List<CeldaMWxModo>();

            return listaRpt;
        }

        /// <summary>
        /// INSUMO: Extranet Consumo de Combustible
        /// </summary>
        /// <param name="fechaPeriodo"></param>
        /// <param name="emprcodis"></param>
        /// <returns></returns>
        private List<MeMedicionxintervaloDTO> ListarMxInt(DateTime fechaPeriodo, string emprcodis)
        {
            List<MeMedicionxintervaloDTO> listaConsumoEjec = FactorySic.GetMeMedicionxintervaloRepository().GetListaMedxintervConsumo(ConstantesStockCombustibles.LectCodiConsumo, ConstantesStockCombustibles.Origlectcodi, emprcodis
                                                       , fechaPeriodo, fechaPeriodo, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto);

            return listaConsumoEjec;
        }

        /// <summary>
        /// INSUMO: Horas de Operación
        /// </summary>
        /// <param name="fechaPeriodo"></param>
        /// <param name="emprcodis"></param>
        /// <param name="equipadres"></param>
        /// <returns></returns>
        private List<EveHoraoperacionDTO> ListarHo(DateTime fechaPeriodo, string emprcodis, string equipadres, List<PrGrupoDTO> listaModo)
        {
            string sParamEmpresa = emprcodis != "-1" ? emprcodis : ConstantesHorasOperacion.ParamEmpresaTodos;
            string sParamCentral = equipadres != "-1" ? equipadres : ConstantesHorasOperacion.ParamCentralTodos;
            List<EveHoraoperacionDTO> listaHOP = servHO.ListarHorasOperacionByCriteria(fechaPeriodo, fechaPeriodo.AddDays(1), sParamEmpresa, sParamCentral, ConstantesHorasOperacion.TipoListadoTodo);
            listaHOP = servHO.CompletarListaHoraOperacionTermo(listaHOP);

            foreach (var reg in listaHOP)
                servHO.FormatearDescripcionesHop(reg);

            //quitar ho_unidad de Huaycoloro que se encuentran duplicados para el dia 09/10/2020
            List<int> lHopcodiError = new List<int>() { 436138, 436140 };
            listaHOP = listaHOP.Where(x => !lHopcodiError.Contains(x.Hopcodi)).ToList();

            //actualizar fuente de energia para las horas de operación que tengan RESIDUAL
            var listaHopResidual = listaHOP.Where(x => x.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiResidual).ToList();

            List<EveHoraoperacionDTO> listaHOModoR = listaHopResidual.Where(x => x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoModo).ToList();
            List<EveHoraoperacionDTO> listaHOUnidadR = listaHopResidual.Where(x => x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoUnidad).ToList();

            foreach (var regHo in listaHOModoR)
            {
                var listaHOdet = listaHOUnidadR.Where(x => x.Hopcodipadre == regHo.Hopcodi).ToList();

                PrGrupoDTO regModo = listaModo.Find(x => x.Grupocodi == regHo.Grupocodi);
                if (regModo != null && regHo.Fenergcodi != regModo.Fenergcodi)
                {
                    regHo.Fenergcodi = regModo.Fenergcodi.Value;

                    foreach (var regDet in listaHOdet)
                    {
                        regDet.Fenergcodi = regModo.Fenergcodi.Value; ;
                    }
                }
            }

            return listaHOP;
        }

        /// <summary>
        /// INSUMO: Despacho por Unidades de generación Extranet
        /// </summary>
        /// <param name="fechaPeriodo"></param>
        /// <param name="emprcodis"></param>
        /// <param name="listaEqGen"></param>
        /// <returns></returns>
        private List<MeMedicion48DTO> Listar48DespachoExtranet(DateTime fechaPeriodo, string emprcodis, string equipadres, List<EqEquipoDTO> listaEqGen, out List<ResultadoValidacionAplicativo> listaMsj)
        {
            equipadres = !string.IsNullOrEmpty(equipadres) ? equipadres : "-1";
            listaMsj = new List<ResultadoValidacionAplicativo>();

            List<MeMedicion48DTO> lista48xGen = servEjec.ListaDataMDGeneracionConsolidado48(fechaPeriodo, fechaPeriodo, ConstantesMedicion.IdTipogrupoCOES
                                                      , ConstantesMedicion.IdTipoGeneracionTermoelectrica.ToString(), emprcodis, ConstanteValidacion.EstadoTodos, ConstantesMedicion.IdTipoRecursoTodos.ToString()
                                                      , false, ConstantesPR5ReportesServicio.LectDespachoEjecutado);

            foreach (var reg in lista48xGen)
            {
                reg.Equinomb = !string.IsNullOrWhiteSpace(reg.Equinomb) ? reg.Equinomb.Trim() : "";
            }

            //incluir registros para equipos que no tienen data
            foreach (var reg in listaEqGen)
            {
                if (lista48xGen.Find(x => x.Equicodi == reg.Equicodi) == null)
                {
                    MeMedicion48DTO reg48 = new MeMedicion48DTO();
                    reg48.Emprcodi = reg.Emprcodi ?? 0;
                    reg48.Emprnomb = reg.Emprnomb;
                    reg48.Equipadre = reg.Equipadre ?? 0;
                    reg48.Central = (reg.Central ?? "").Trim();
                    reg48.Equicodi = reg.Equicodi;
                    reg48.Equinomb = reg.Equinomb;
                    reg48.Grupocodi = reg.Grupocodi ?? 0;
                    reg48.Gruponomb = reg.Gruponomb;
                    reg48.Medifecha = fechaPeriodo;

                    lista48xGen.Add(reg48);

                    listaMsj.Add(new ResultadoValidacionAplicativo()
                    {
                        Equipadre = reg48.Equipadre,
                        Descripcion = string.Format("El generador {0} no tiene información de Despacho cargado en la Extranet.", reg48.Equinomb)
                    });
                }
            }

            //filtrar m48 de las centrales
            if (equipadres != "-1")
            {
                List<int> listaEquipadre = equipadres.Split(',').Select(x => int.Parse(x)).ToList();
                lista48xGen = lista48xGen.Where(x => listaEquipadre.Contains(x.Equipadre)).ToList();
            }

            return lista48xGen;
        }

        /// <summary>
        /// INSUMO: Curva de consumo de Combustible vs Potencia
        /// </summary>
        /// <param name="fechaPeriodo"></param>
        /// <param name="listaGrupoModo"></param>
        /// <returns></returns>
        private List<ConsumoHorarioCombustible> ListarCurvaConsumoCombPotencia(DateTime fechaPeriodo, List<PrGrupoDTO> listaGrupoModo, out List<ResultadoValidacionAplicativo> listaMsj)
        {
            listaMsj = new List<ResultadoValidacionAplicativo>();

            /// Curvas de Consumo de los modos de operación
            List<PrGrupodatDTO> listaGrupodat = INDAppServicio.ListarPrGrupodatHistoricoDecimalValido(ConstantesIndisponibilidades.ConcepcodisCurvaCombModo);

            //Curva de Consumo de combustible de las Unidades Especiales
            List<PrGrupoEquipoValDTO> listaEquipoval = INDAppServicio.ListarPrGrupoEquipoValHistoricoDecimalValido(ConstantesIndisponibilidades.ConcepcodisCurvaCombModo);

            List<SiFuenteenergiaDTO> listaFenerg = GetByCriteriaSiFuenteenergias();
            List<SiFactorconversionDTO> listaFactor = GetByCriteriaSiFactorconversions();

            //
            List<ConsumoHorarioCombustible> listaCurva = new List<ConsumoHorarioCombustible>();

            foreach (var regModo in listaGrupoModo)
            {
                if (regModo.Grupocodi == 312)
                { }

                SiFuenteenergiaDTO regFenerg = listaFenerg.Find(x => x.Fenergcodi == regModo.Fenergcodi);
                string tipoUnidad = regFenerg.Tinfcoesabrev;

                int tinfOrigenParametros = -1;
                int tinfDestinoCoes = regFenerg.Tinfcoes;

                switch (regModo.Fenergcodi)
                {
                    case ConstantesPR5ReportesServicio.FenergcodiGas:
                    case ConstantesPR5ReportesServicio.FenergcodiBiogas:
                        tinfOrigenParametros = ConstantesAppServicio.Tipoinfocodim3;//en bd usan m3
                        //factorConversion = 1;
                        break;
                    case ConstantesPR5ReportesServicio.FenergcodiDiesel:
                    case ConstantesPR5ReportesServicio.FenergcodiR500:
                    case ConstantesPR5ReportesServicio.FenergcodiR6:
                    case ConstantesPR5ReportesServicio.FenergcodiResidual:
                        tinfOrigenParametros = ConstantesAppServicio.TipoinfocodiGalones;
                        //factorConversion = 1 / 264.172m; //en bd usan galones
                        break;

                    case ConstantesPR5ReportesServicio.FenergcodiBagazo:
                    case ConstantesPR5ReportesServicio.FenergcodiCarbon:
                        tinfOrigenParametros = ConstantesAppServicio.TipoinfocodiKg;
                        //factorConversion = 1.0m / 1000; //en bd usan kilos
                        break;
                }

                decimal factorConversion = 1.0m;
                if (tinfOrigenParametros != tinfDestinoCoes)
                {
                    SiFactorconversionDTO regF = listaFactor.Find(x => x.Tinforigen == tinfOrigenParametros && x.Tinfdestino == tinfDestinoCoes);
                    if (regF != null)
                        factorConversion = regF.Tconvfactor;
                }

                if (regModo.TieneModoEspecial && !regModo.TieneModoCicloCombinado)
                {
                    for (int i = 0; i < regModo.ListaEquicodi.Count; i++)
                    {
                        int equicodi = regModo.ListaEquicodi[i];

                        ConsumoHorarioCombustible cesp = INDAppServicio.ObtenerCurvaConsumo(fechaPeriodo, equicodi, regModo.Grupocodi, true, factorConversion
                                                                        , listaGrupodat, listaEquipoval);
                        cesp.FechaDia = fechaPeriodo;
                        cesp.Emprcodi = regModo.Emprcodi ?? 0;
                        cesp.Emprnomb = regModo.Emprnomb;
                        cesp.Grupocodi = regModo.Grupocodi;
                        cesp.Gruponomb = regModo.Gruponomb + " (" + regModo.ListaEquiabrev[i] + ")";
                        cesp.Equipadre = regModo.Equipadre;
                        cesp.Fenergcodi = regModo.Fenergcodi ?? 0;
                        cesp.UnidadMedida = tipoUnidad;
                        cesp.EsUnidadEspecial = true;

                        listaCurva.Add(cesp);

                        if (cesp.PendienteM01 <= 0)
                        {
                            listaMsj.Add(new ResultadoValidacionAplicativo()
                            {
                                Equipadre = cesp.Equipadre,
                                Central = regModo.Central,
                                Emprnomb = regModo.Emprnomb,
                                Descripcion = string.Format("La unidad {0} no tiene Curva Potencia vs Consumo.", cesp.Gruponomb)
                            });
                        }

                    }
                }
                else
                {
                    ConsumoHorarioCombustible c = INDAppServicio.ObtenerCurvaConsumo(fechaPeriodo, regModo.Equicodi, regModo.Grupocodi, false, factorConversion
                                                                    , listaGrupodat, listaEquipoval);
                    c.FechaDia = fechaPeriodo;
                    c.Emprcodi = regModo.Emprcodi ?? 0;
                    c.Emprnomb = regModo.Emprnomb;
                    c.Grupocodi = regModo.Grupocodi;
                    c.Gruponomb = regModo.Gruponomb;
                    c.Equicodi = regModo.Equicodi;
                    c.Equinomb = regModo.Equinomb;
                    c.Equipadre = regModo.Equipadre;
                    c.Fenergcodi = regModo.Fenergcodi ?? 0;
                    c.UnidadMedida = tipoUnidad;

                    listaCurva.Add(c);

                    if (c.PendienteM01 <= 0)
                    {
                        listaMsj.Add(new ResultadoValidacionAplicativo()
                        {
                            Equipadre = c.Equipadre,
                            Central = regModo.Central,
                            Emprnomb = regModo.Emprnomb,
                            Descripcion = string.Format("La unidad {0} no tiene Curva Potencia vs Consumo.", c.Gruponomb)
                        });
                    }

                }
            }

            return listaCurva;
        }

        /// <summary>
        /// Convertir unidades de los parametros a unidad COES del combustible de arranque
        /// </summary>
        /// <param name="listaUnidad"></param>
        private void ActualizarMedidaCombustibleArranque(List<EqEquipoDTO> listaUnidad)
        {
            List<SiFuenteenergiaDTO> listaFenerg = GetByCriteriaSiFuenteenergias();
            List<SiFactorconversionDTO> listaFactor = GetByCriteriaSiFactorconversions();

            foreach (var regUnidad in listaUnidad)
            {
                if (regUnidad.FenergcodiCombAlt > 0)
                {
                    SiFuenteenergiaDTO regFenerg = listaFenerg.Find(x => x.Fenergcodi == regUnidad.FenergcodiCombAlt);

                    int tinfOrigenParametros = ConstantesAppServicio.TipoinfocodiGalones;
                    int tinfDestinoCoes = regFenerg.Tinfcoes;
                    decimal factorConversion = 1.0m;

                    if (tinfOrigenParametros != tinfDestinoCoes)
                    {
                        SiFactorconversionDTO regF = listaFactor.Find(x => x.Tinforigen == tinfOrigenParametros && x.Tinfdestino == tinfDestinoCoes);
                        if (regF != null)
                            factorConversion = regF.Tconvfactor;
                    }

                    regUnidad.ConsumoCombAlt = regUnidad.ConsumoCombAlt.GetValueOrDefault(0) * factorConversion;
                }
            }
        }

        private void ActualizarRendimientoBagazo(List<EqEquipoDTO> listaUnidad, DateTime fechaFin, out List<ResultadoValidacionAplicativo> listaMsj)
        {
            listaMsj = new List<ResultadoValidacionAplicativo>();

            //Rendimiento Consumo IEOD Bagazo
            List<EqPropequiDTO> listaRendBagazoEq = INDAppServicio.ListarEqPropequiHistoricoDecimalValido(ConstantesConsumoCombustible.PropiedadRendBagazo.ToString());

            foreach (var reg in listaUnidad)
            {
                if (ConstantesPR5ReportesServicio.FenergcodiBagazo == reg.Fenergcodi)
                {
                    //Evaluar RER
                    INDAppServicio.GetValorDecimalFromListaEqPropequi(fechaFin, reg.Equicodi, listaRendBagazoEq, out decimal? rendBagazo, out DateTime? fechaVigRenB, out string comentarioRendB);

                    if (reg.Famcodi == ConstantesHorasOperacion.IdGeneradorTemoelectrico)
                    {
                        if (rendBagazo > 0)
                        {
                            reg.RendBagazo = rendBagazo;
                        }
                        else
                        {
                            listaMsj.Add(new ResultadoValidacionAplicativo() { Equipadre = reg.Equipadre.Value, TipoFuenteDatoDesc = "Unidad de Generación", Descripcion = "El generador " + reg.Equinomb + " no tiene Rendimiento de consumo IEOD." });
                        }
                    }
                }
            }
        }

        /// <summary>
        /// CÁLCULO: Obtener el consumo REAL por Unidad de generación
        /// </summary>
        /// <param name="fenergcodi"></param>
        /// <param name="regUnidad"></param>
        /// <param name="listaConsumoEjec"></param>
        /// <returns></returns>
        private decimal GetConsumoRealXUnidad(int fenergcodi, EqEquipoDTO regUnidad, List<MeMedicionxintervaloDTO> listaConsumoEjec)
        {
            List<MeMedicionxintervaloDTO> listaXInt = new List<MeMedicionxintervaloDTO>();

            if (regUnidad.Famcodi == ConstantesHorasOperacion.IdTipoTermica || regUnidad.EsUnaUnidadXCentral)
            {
                listaXInt = listaConsumoEjec.Where(x => regUnidad.ListaEquicodi.Contains(x.Equicodi) || x.Equicodi == regUnidad.Equipadre).ToList();
            }
            else
            {
                listaXInt = listaConsumoEjec.Where(x => x.Equicodi == regUnidad.Equicodi).ToList();
            }

            //Fuente de Energia del Grupo => 15: NAFTA & GAS REFINERIA
            if (fenergcodi == ConstantesPR5ReportesServicio.FenergcodiNR)
            {
                return listaXInt.Where(x => x.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiRef || x.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiNaf).Sum(x => x.Medinth1 ?? 0);
            }

            return listaXInt.Where(x => x.Fenergcodi == fenergcodi).Sum(x => x.Medinth1 ?? 0);
        }

        /// <summary>
        /// CÁLCULO: Obtener los datos por Unidad de generación (modos de operación, curva, generadores, horas de operación)
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="listaUnidadTermo"></param>
        /// <param name="lista48Gen"></param>
        /// <param name="listaHO"></param>
        /// <param name="listaGrupoModo"></param>
        /// <param name="listaCurva"></param>
        /// <param name="listaDetHo"></param>
        /// <param name="listaMsj"></param>
        /// <returns></returns>
        private void ListaCeldaReporte48(DateTime f, List<EqEquipoDTO> listaUnidadTermo
                                                        , ref List<MeMedicion48DTO> lista48Gen, List<EveHoraoperacionDTO> listaHO30min, List<PrGrupoDTO> listaGrupoModo
                                                        , List<ConsumoHorarioCombustible> listaCurva, List<DetalleHoraOperacionModo> listaDetHo, List<int> listaEquicodiTv
                                                        , ref List<CeldaMWxModo> listaCelda, ref List<ResultadoValidacionAplicativo> listaMsj)
        {
            //Calificacione
            List<EveSubcausaeventoDTO> listaSubcausa = servHO.ListarTipoOperacionHO();

            //Horas de operacion
            List<EveHoraoperacionDTO> listaHOModo = listaHO30min.Where(x => x.Hophorini.Value.Date == f && x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoModo).ToList();
            List<EveHoraoperacionDTO> listaHOUnidad = listaHO30min.Where(x => x.Hophorini.Value.Date == f && x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoUnidad
                                                                            && !listaEquicodiTv.Contains(x.Equicodi.Value)).ToList();

            //Generadores sin considerar TV
            lista48Gen = lista48Gen.Where(x => !listaEquicodiTv.Contains(x.Equicodi)).ToList();

            //Recorrer horas de operación a nivel de modo
            foreach (var regHo in listaHOModo)
            {
                if (regHo.Hopcodi == 697650)
                { }

                PrGrupoDTO regModo = listaGrupoModo.Find(x => x.Grupocodi == regHo.Grupocodi);
                var listaHoXModo = listaHOUnidad.Where(x => x.Hopcodipadre == regHo.Hopcodi).ToList();

                if (regModo != null)
                {
                    //Recorrer horas de operación a nivel de unidad
                    foreach (var regHoUni in listaHoXModo)
                    {
                        MeMedicion48DTO reg48Gen = lista48Gen.Find(x => x.Equicodi == regHoUni.Equicodi);
                        EqEquipoDTO regEqUnidad = null;
                        if (!regModo.TieneModoEspecial || regModo.TieneModoCicloCombinado)
                        {
                            regEqUnidad = listaUnidadTermo.Find(x => x.Fenergcodi == regModo.Fenergcodi && regModo.ListaEquicodi.All(y => x.ListaEquicodi.Contains(y)));
                        }
                        else
                        {
                            regEqUnidad = listaUnidadTermo.Find(x => x.Fenergcodi == regModo.Fenergcodi && x.Grupocodi == regModo.Grupocodi && x.Equicodi == regHoUni.Equicodi);
                        }

                        if (regEqUnidad != null && reg48Gen != null)
                        {
                            //Determinar los grupos por cada media hora
                            for (int h = regHoUni.HIni48; h <= regHoUni.HFin48; h++)
                            {
                                if (h >= 1 && h <= 48)
                                {
                                    if (regHoUni.Equicodi == 12778 && h == 3)
                                    { }

                                    DateTime fi = f.Date.AddMinutes(h * 30);

                                    //Valor de potencia
                                    decimal? valorH = (decimal?)reg48Gen.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg48Gen, null);
                                    if (valorH > 0)
                                    {
                                    }

                                    //Total de minutos
                                    int minutoH = regHoUni.TotalMinuto;
                                    if (minutoH > 0)
                                    {
                                        //Obtener MW. Posteriormete se calcula el consumo por unidad
                                        AsignarDatosX30minToUnidadCicloSimple(h, valorH ?? 0, minutoH, f, regEqUnidad, regModo, regHo.Grupocodi.Value, regHo.Subcausacodi.Value,
                                                            listaCurva, listaDetHo, listaSubcausa, ref listaCelda);

                                        //Si existe datos de despacho entonces ya no considerarlo para el siguiente modo de operación
                                        if (valorH.GetValueOrDefault(0) > 0)
                                        {
                                            reg48Gen.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).SetValue(reg48Gen, 0.0m);
                                        }
                                        else
                                        {
                                            //existe horas de operación pero no existe despacho
                                            var regCelda = listaCelda.Find(x => x.UnidadTermico.Equicodi == regEqUnidad.Equicodi && x.UnidadTermico.Grupocodi == regEqUnidad.Grupocodi
                                                                            && x.Grupocodimodo == regHo.Grupocodi.Value);
                                            if (regCelda.ListaCalifHo[h - 1] != 0 && regCelda.ListaMW[h - 1].GetValueOrDefault(0) == 0)
                                            {
                                                listaMsj.Add(new ResultadoValidacionAplicativo()
                                                {
                                                    Equipadre = reg48Gen.Equipadre,
                                                    Descripcion = string.Format("La unidad de generación {0} no tiene datos de despacho para la media hora {1}."
                                                                                                                , (regHoUni.Equiabrev ?? "").Trim(), fi.ToString(ConstantesAppServicio.FormatoFechaFull))
                                                });
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (regEqUnidad == null)
                                listaMsj.Add(new ResultadoValidacionAplicativo() { Equipadre = regModo.Equipadre, Descripcion = string.Format("El modo de operación {0} no está asociado a ninguna unidad.", regModo.Gruponomb) });

                            if (reg48Gen == null)
                                listaMsj.Add(new ResultadoValidacionAplicativo() { Equipadre = regModo.Equipadre, Descripcion = string.Format("La unidad {0} tiene registros en Horas de Operación pero no registros en Despacho.", (regHoUni.Equiabrev ?? "").Trim()) });
                        }
                    }
                }
            }

            //buscar si existe MW sin horas de operación
            foreach (var reg48Gen in lista48Gen)
            {
                for (int h = 1; h <= 48; h++)
                {
                    decimal? valorH = (decimal?)reg48Gen.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg48Gen, null);
                    DateTime fi = f.Date.AddMinutes(h * 30);

                    if (valorH > 0)
                    {
                        listaMsj.Add(new ResultadoValidacionAplicativo()
                        {
                            Equipadre = reg48Gen.Equipadre,
                            Descripcion = string.Format("La unidad de generación {0} no tiene horas de operación para la media hora {1}."
                                                                                                , (reg48Gen.Equiabrev ?? "").Trim(), fi.ToString(ConstantesAppServicio.FormatoFechaFull))
                        });
                    }
                }
            }

            //si no existe horas de operación para la unidad, agregar detalle sin datos de Potencia o consumo (solo para visualización)
            foreach (var regUnidad in listaUnidadTermo)
            {
                //obtener los modos de operación de la unidad y la potencia asociada
                List<CeldaMWxModo> listaCeldaXUnidad = listaCelda.Where(x => x.UnidadTermico.Fenergcodi == regUnidad.Fenergcodi &&
                                                                    x.UnidadTermico.Equicodi == regUnidad.Equicodi && x.UnidadTermico.Grupocodi == regUnidad.Grupocodi).ToList();

                if (!listaCeldaXUnidad.Any())
                {
                    PrGrupoDTO regModo = listaGrupoModo.Find(x => x.Grupocodi == regUnidad.Grupocodi);
                    if (regModo != null)
                    {
                        AsignarDatosX30minToUnidadCicloSimple(1, 0, 0, f, regUnidad, regModo, regUnidad.Grupocodi.Value, 0, listaCurva, listaDetHo, listaSubcausa, ref listaCelda);
                    }
                }
            }

            //Asociar grafico web
            foreach (var celda in listaCelda)
            {
                celda.Grafico = GenerarGraficoXUnidad(f, celda.UnidadTermico, celda);

                //verificar si tiene alerta de Horas de operación
                var listaSubc = ConstantesConsumoCombustible.ListaCalificacionAlertaHO;

                bool tieneCalifXh = celda.ListaCalifHo.Where(x => listaSubc.Contains(x)).Count() > 0;
                bool tieneDetalleHo = celda.DetalleHO != null ? celda.DetalleHO.ListaHO.Select(x => x.Subcausacodi).Where(x => listaSubc.Contains(x ?? 0)).Count() > 0 : false;

                celda.TieneAlertaHo = tieneCalifXh || tieneDetalleHo;
            }
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
        /// <param name="listaCurva"></param>
        /// <param name="listaDetHo"></param>
        /// <param name="listaCelda"></param>
        private void AsignarDatosX30minToUnidadCicloSimple(int h, decimal valorH, int minutoH, DateTime fechaPeriodo, EqEquipoDTO regUnidad, PrGrupoDTO regModo, int grupocodimodo, int subcausacodi
                                                , List<ConsumoHorarioCombustible> listaCurva, List<DetalleHoraOperacionModo> listaDetHo, List<EveSubcausaeventoDTO> listaSubcausa
                                                , ref List<CeldaMWxModo> listaCelda)
        {
            var regCelda = listaCelda.Find(x => x.UnidadTermico.Equicodi == regUnidad.Equicodi && x.UnidadTermico.Grupocodi == regUnidad.Grupocodi && x.Grupocodimodo == grupocodimodo);
            var regSubcausa = listaSubcausa.Find(x => x.Subcausacodi == subcausacodi);

            if (regCelda == null)
            {
                regCelda = new CeldaMWxModo()
                {
                    Fecha = fechaPeriodo,
                    Grupocodimodo = grupocodimodo,
                    UnidadTermico = regUnidad,
                    Equipadre = regUnidad.Equipadre.Value
                };
                regCelda.Modo = regModo;

                //asignar modo y detalle de horas de operación
                if (!regModo.TieneModoEspecial || regModo.TieneModoCicloCombinado)
                {
                    regCelda.Curva = listaCurva.Find(x => x.Grupocodi == regCelda.Grupocodimodo);
                    regCelda.DetalleHO = listaDetHo.Find(x => x.Grupocodi == regCelda.Grupocodimodo);
                }
                else
                {
                    regCelda.Curva = listaCurva.Find(x => x.Grupocodi == regUnidad.Grupocodi && x.Equicodi == regUnidad.Equicodi);
                    regCelda.DetalleHO = listaDetHo.Find(x => x.Grupocodi == regUnidad.Grupocodi && x.Equicodi == regUnidad.Equicodi);
                }

                //setear valor
                listaCelda.Add(regCelda);
            }

            //setear minutos de la celda
            regCelda.ListaMinuto[h - 1] = regCelda.ListaMinuto[h - 1] + minutoH;
            if (regCelda.ListaMinuto[h - 1] > 30)
            { }
            //setear valores por cada H 30min
            regCelda.ListaMW[h - 1] = (regCelda.ListaMW[h - 1] ?? 0) + valorH;
            if (regCelda.ListaCalifHo[h - 1] == 0)
            {
                regCelda.ListaCalifHo[h - 1] = subcausacodi;
                regCelda.ListaSubcausadesc[h - 1] = regSubcausa != null ? regSubcausa.Subcausadesc : "";
            }
            SetValoresGraficoHorasOperacion(regCelda, h - 1);

            //si es central de reserva fria, calcular la cantidad de generadores segun el mw
            SetValoresMensajeReservaFria(regCelda, h, valorH, regUnidad);
        }

        /// <summary>
        /// Asignar valores de Columnas horas de operacion
        /// </summary>
        /// <param name="regCelda"></param>
        /// <param name="h"></param>
        private void SetValoresGraficoHorasOperacion(CeldaMWxModo regCelda, int h)
        {

            if (regCelda.ListaCalifHo[h] != 0)
            {
                decimal valorHxCalif = regCelda.UnidadTermico.Pe.GetValueOrDefault(0); //por defecto POR POTENCIA O ENERGIA
                string descCalif = "";

                switch (regCelda.ListaCalifHo[h])
                {
                    case ConstantesSubcausaEvento.SubcausaAMinimaCarga:
                    case ConstantesSubcausaEvento.SubcausaPorRestricOpTemporal:
                        valorHxCalif = regCelda.UnidadTermico.Pmin.GetValueOrDefault(0);
                        break;
                    case ConstantesSubcausaEvento.SubcausaPorRsf:
                        valorHxCalif = regCelda.UnidadTermico.Rsf.GetValueOrDefault(0);
                        break;
                    case ConstantesSubcausaEvento.SubcausaPorPruebas:
                        valorHxCalif = 0;
                        descCalif = "POR PRUEBAS";
                        break;
                    case ConstantesSubcausaEvento.SubcausaPorSeguridad:
                        valorHxCalif = 0;
                        descCalif = "POR SEGURIDAD";
                        break;
                    case ConstantesSubcausaEvento.SubcausaPorTension:
                        valorHxCalif = 0;
                        descCalif = "POR TENSIÓN";
                        break;
                }

                regCelda.ListaCalifHoDesc[h] = descCalif;
                regCelda.ListaMWHo[h] = valorHxCalif;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="regCelda"></param>
        /// <param name="h"></param>
        /// <param name="valorH"></param>
        /// <param name="regUnidad"></param>
        private void SetValoresMensajeReservaFria(CeldaMWxModo regCelda, int h, decimal valorH, EqEquipoDTO regUnidad)
        {
            if (regUnidad.Gruporeservafria == 1 && regUnidad.NumeroGen > 0)
            {
                decimal pe = regUnidad.Pe.GetValueOrDefault(0);
                if (valorH > 0 && pe > 0)
                {
                    decimal peXGen = pe / regUnidad.NumeroGen;

                    decimal valNumGenXH = Math.Ceiling(valorH / peXGen);

                    int numGenXH = Convert.ToInt32(valNumGenXH);
                    if (numGenXH <= 0) numGenXH = 1;
                    if (numGenXH > regUnidad.NumeroGen) numGenXH = regUnidad.NumeroGen;

                    regCelda.ListaNumGen[h - 1] = numGenXH;
                    regCelda.ListaMensaje[h - 1] = string.Format("Número de generadores: {0}\nPe por generador: {1}", numGenXH, Math.Round(peXGen, 4));
                }
            }
        }

        private void ValidarHoraSinDespachoMenor30min(DateTime f, List<EqEquipoDTO> listaUnidadTermo, List<MeMedicion48DTO> lista48Gen, List<EveHoraoperacionDTO> listaHO30min,
                                                    List<PrGrupoDTO> listaGrupoModo, List<int> listaEquicodiTv,
                                                    ref List<CeldaMWxModo> listaCelda, ref List<ResultadoValidacionAplicativo> listaMsj)
        {
            //Horas de operacion
            List<EveHoraoperacionDTO> listaHOModo = listaHO30min.Where(x => x.Hophorini.Value.Date == f && x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoModo).ToList();
            List<EveHoraoperacionDTO> listaHOUnidad = listaHO30min.Where(x => x.Hophorini.Value.Date == f && x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoUnidad
                                                                            && !listaEquicodiTv.Contains(x.Equicodi.Value)).ToList();

            //Generadores sin considerar TV
            lista48Gen = lista48Gen.Where(x => !listaEquicodiTv.Contains(x.Equicodi)).ToList();

            //Recorrer horas de operación a nivel de modo
            foreach (var regHo in listaHOModo)
            {
                PrGrupoDTO regModo = listaGrupoModo.Find(x => x.Grupocodi == regHo.Grupocodi);
                var listaHoXModo = listaHOUnidad.Where(x => x.Hopcodipadre == regHo.Hopcodi).ToList();

                if (regModo != null)
                {
                    //Recorrer horas de operación a nivel de unidad
                    foreach (var regHoUni in listaHoXModo)
                    {
                        MeMedicion48DTO reg48Gen = lista48Gen.Find(x => x.Equicodi == regHoUni.Equicodi);
                        EqEquipoDTO regEqUnidad = null;
                        if (!regModo.TieneModoEspecial || regModo.TieneModoCicloCombinado)
                        {
                            regEqUnidad = listaUnidadTermo.Find(x => x.Fenergcodi == regModo.Fenergcodi && regModo.ListaEquicodi.All(y => x.ListaEquicodi.Contains(y)));
                        }
                        else
                        {
                            regEqUnidad = listaUnidadTermo.Find(x => x.Fenergcodi == regModo.Fenergcodi && x.Grupocodi == regModo.Grupocodi && x.Equicodi == regHoUni.Equicodi);
                        }

                        if (regEqUnidad != null && reg48Gen != null)
                        {
                            //Determinar los grupos por cada media hora
                            for (int h = regHoUni.HIni48; h <= regHoUni.HFin48; h++)
                            {
                                if (h >= 1 && h <= 48)
                                {
                                    int minutoH = regHoUni.TotalMinuto;

                                    //si la hora de operación original dura menos a 30min
                                    if (minutoH < 30)
                                    {
                                        var regCelda = listaCelda.Find(x => x.UnidadTermico.Equicodi == regEqUnidad.Equicodi && x.UnidadTermico.Grupocodi == regEqUnidad.Grupocodi && x.Grupocodimodo == regHo.Grupocodi);

                                        //setear minutos de la celda
                                        regCelda.ListaMinuto[h - 1] = regCelda.ListaMinuto[h - 1] + minutoH;

                                        //cuando no haya despacho ejecutado de las unidades de generación termoeléctricas debido a horas de operaciones menores a media hora.
                                        var valorPotenciaCelda = (regCelda.ListaMW[h - 1] ?? 0);
                                        if (valorPotenciaCelda <= 0)
                                        {
                                            if (regHoUni.Hophorini > regHoUni.HoraIni48) //la hora ini original debe estar dentro de la media hora
                                            {
                                                listaMsj.Add(new ResultadoValidacionAplicativo()
                                                {
                                                    Equipadre = reg48Gen.Equipadre,
                                                    Descripcion = string.Format("El modo de operación {0} ({1}) operó desde las {2} a {3}.", (regHo.Gruponomb ?? "").Trim(), (regHoUni.Equiabrev ?? "").Trim(),
                                                                    regHoUni.Hophorini.Value.ToString(ConstantesAppServicio.FormatoFechaFull), regHoUni.Hophorfin.Value.ToString(ConstantesAppServicio.FormatoFechaFull))
                                                });
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// obtener valor de consumo por unidad
        /// </summary>
        /// <param name="regUnidad"></param>
        /// <param name="listaCeldaXUnidad"></param>
        /// <param name="valorTeorico"></param>
        private void AsignarConsumoHorarioXUnidad(EqEquipoDTO regUnidad, ref List<CeldaMWxModo> listaCeldaXUnidad, out decimal valorTeorico)
        {
            valorTeorico = 0;

            //calcular el consumo por media hora
            for (int h = 1; h <= 48; h++)
            {
                if (h == 1)
                { }

                foreach (var regModoxUnidad in listaCeldaXUnidad)
                {
                    //total de minutos en esa media hora
                    int totalMin = regModoxUnidad.ListaMinuto[h - 1];
                    if (totalMin > 30) totalMin = 30;

                    //Obtener potencia del modo
                    decimal valorMW = regModoxUnidad.ListaMW[h - 1].GetValueOrDefault(0);

                    //periodos de operación menores a 30 minuto utilizar potencia minima
                    if (totalMin > 0 && totalMin < 30)
                    {
                        valorMW = regModoxUnidad.UnidadTermico.Pmin.GetValueOrDefault(0);
                        regModoxUnidad.ListaMW[h - 1] = valorMW;
                    }

                    //obtener consumo
                    decimal valorConsumoH = 0;
                    decimal valorConsumo30min = 0;

                    if (ConstantesPR5ReportesServicio.FenergcodiBagazo == regUnidad.Fenergcodi)
                    {
                        //convertir consumo horario a 30 minutos
                        if (regUnidad.RendBagazo > 0 && valorMW > 0 && totalMin > 0)
                        {
                            valorConsumoH = valorMW / regUnidad.RendBagazo.Value;
                            valorConsumo30min = valorConsumoH / (60.0m / totalMin);

                            regModoxUnidad.ListaConsumo[h - 1] = valorConsumo30min;
                        }
                    }
                    else
                    {
                        ConsumoHorarioCombustible regCurva = regModoxUnidad.Curva;
                        if (regCurva != null && valorMW > 0 && totalMin > 0)
                        {
                            valorConsumoH = 0;

                            //caso Reserva fria de Pto Maldonado y Pucallpa
                            if (regUnidad.Gruporeservafria == 1 && regUnidad.NumeroGen > 0)
                            {
                                int numGenxH = regModoxUnidad.ListaNumGen[h - 1];
                                if (numGenxH > 0)
                                {
                                    decimal factorRf = numGenxH / (regUnidad.NumeroGen * 1.0m);
                                    decimal bRf = regCurva.CoeficienteIndependiente * factorRf;

                                    valorConsumoH = (valorMW * decimal.Round(regCurva.PendienteM01, 4) + decimal.Round(bRf, 4));
                                }
                            }
                            else
                            {
                                //caso general
                                valorConsumoH = (valorMW * decimal.Round(regCurva.PendienteM01, 4) + decimal.Round(regCurva.CoeficienteIndependiente, 4));
                            }

                            //convertir consumo horario a 30 minutos
                            valorConsumo30min = valorConsumoH / (60.0m / totalMin);

                            regModoxUnidad.ListaConsumo[h - 1] = valorConsumo30min;
                        }
                    }

                    valorTeorico += valorConsumo30min;
                }
            }
        }

        /// <summary>
        /// Generar datos para grafico web
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="regUnidad"></param>
        /// <param name="regCeldaXUnidad"></param>
        /// <returns></returns>
        private GraficoWeb GenerarGraficoXUnidad(DateTime fecha, EqEquipoDTO regUnidad, CeldaMWxModo regCeldaXUnidad)
        {
            if (regUnidad.Equipadre == 12815)
            { }

            if (regUnidad.Equicodi == 13604 && regUnidad.Fenergcodi == 3)
            { }

            //obtener lista Nota:
            var listaSubc = ConstantesConsumoCombustible.ListaCalificacionAlertaHO;
            List<EveHoraoperacionDTO> listaHoConNota = new List<EveHoraoperacionDTO>();
            if (regCeldaXUnidad.DetalleHO != null && regCeldaXUnidad.DetalleHO.ListaHO != null)
            {
                var listaHoxModo = regCeldaXUnidad.DetalleHO.ListaHO.Where(x => listaSubc.Contains(x.Subcausacodi ?? 0)).ToList();
                listaHoConNota.AddRange(listaHoxModo);
            }

            List<string> listaNota = new List<string>();
            foreach (var reg in listaHoConNota)
                listaNota.Add(GetDescripcionHo(reg));

            GraficoWeb grafico = ObtenerGraficoXUnidad(fecha, regUnidad.Equiabrev + " - " + regUnidad.Fenergnomb, listaNota, regCeldaXUnidad.ListaMW, regCeldaXUnidad.ListaMWHo, regCeldaXUnidad.ListaSubcausadesc);

            return grafico;
        }

        /// <summary>
        /// Generar objeto grafico web
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="unidadnomb"></param>
        /// <param name="listaNota"></param>
        /// <param name="listaValorDesp"></param>
        /// <param name="listaValorHo"></param>
        /// <returns></returns>
        private GraficoWeb ObtenerGraficoXUnidad(DateTime fecha, string unidadnomb, List<string> listaNota, decimal?[] listaValorDesp, decimal?[] listaValorHo, string[] listaSubcausadesc)
        {
            GraficoWeb grafico = new GraficoWeb();
            grafico.TitleText = unidadnomb;
            grafico.ListaNota = listaNota;

            grafico.Series = new List<RegistroSerie>();
            grafico.SeriesType = new List<string>();
            grafico.SeriesName = new List<string>();
            grafico.YAxixTitle = new List<string>();

            grafico.SerieDataS = new DatosSerie[2][];
            grafico.Series.Add(new RegistroSerie());
            grafico.Series.Add(new RegistroSerie());

            grafico.YAxixTitle.Add("MW");
            grafico.XAxisTitle = "Dia:Horas";

            grafico.Series[0].Name = "Despacho";
            grafico.Series[0].Type = "line";
            grafico.Series[0].Color = "#3498DB";
            grafico.Series[0].YAxisTitle = "MW";

            grafico.Series[1].Name = "Horas de Operación";
            grafico.Series[1].Type = "line";
            grafico.Series[1].Color = "#DC143C";
            grafico.Series[1].YAxisTitle = "MW";


            int numDia = 1;

            grafico.SerieDataS[0] = new DatosSerie[48 * numDia];
            grafico.SerieDataS[1] = new DatosSerie[48 * numDia];

            // titulo el reporte
            grafico.XAxisCategories = new List<string>();
            grafico.SeriesType = new List<string>();
            grafico.SeriesYAxis = new List<int>();
            grafico.SeriesYAxis.Add(0);

            int indiceDia = 0;
            for (var day = fecha.Date; day.Date <= fecha.Date; day = day.AddDays(1))
            {
                for (var j = 1; j <= 48; j++)
                {
                    decimal? valor1 = listaValorDesp[j - 1];
                    valor1 = valor1 != null ? Math.Round(valor1.Value, 2) : 0;

                    decimal? valor2 = listaValorHo[j - 1];
                    valor2 = valor2 != null ? Math.Round(valor2.Value, 2) : 0;

                    grafico.XAxisCategories.Add(day.AddMinutes(j * 30).ToString(ConstantesAppServicio.FormatoFechaHora));
                    var serieDespacho = new DatosSerie();
                    serieDespacho.X = day.AddMinutes(j * 30);
                    serieDespacho.Y = valor1;

                    var serieHo = new DatosSerie();
                    serieHo.X = day.AddMinutes(j * 30);
                    serieHo.Y = valor2;
                    serieHo.Type = (listaSubcausadesc[j - 1] ?? "").Trim();

                    grafico.SerieDataS[0][indiceDia * 48 + (j - 1)] = serieDespacho;
                    grafico.SerieDataS[1][indiceDia * 48 + (j - 1)] = serieHo;
                }

                indiceDia++;
            }

            return grafico;
        }

        /// <summary>
        /// Clonar lista48
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        private List<MeMedicion48DTO> ClonarM48(List<MeMedicion48DTO> lista)
        {
            List<MeMedicion48DTO> l = new List<MeMedicion48DTO>();

            foreach (var reg in lista)
            {
                var regClone = new MeMedicion48DTO()
                {
                    Medifecha = reg.Medifecha,
                    Emprcodi = reg.Emprcodi,
                    Central = (reg.Central ?? "").Trim(),
                    Equipadre = reg.Equipadre,
                    Equicodi = reg.Equicodi,
                    Equiabrev = reg.Equiabrev,
                    Grupocodi = reg.Grupocodi
                };
                for (int h = 1; h <= 48; h++)
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
        /// buscar modo de operación representativo para la unidad por dia
        /// </summary>
        /// <param name="equicodi"></param>
        /// <param name="fenergcodi"></param>
        /// <param name="listaHOP"></param>
        /// <param name="listaModo"></param>
        /// <returns></returns>
        private int? AsignarModoOperacionRepresentativo(int equicodi, int fenergcodi, List<EveHoraoperacionDTO> listaHOP, List<PrGrupoDTO> listaModo)
        {
            List<EveHoraoperacionDTO> listaHoModo = listaHOP.Where(x => x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoModo).OrderBy(x => x.Hophorini).ToList();
            List<EveHoraoperacionDTO> listaHoUnidad = listaHOP.Where(x => x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoUnidad).OrderBy(x => x.Hophorini).ToList();

            var listaHoxEqF = listaHoUnidad.Where(x => x.Fenergcodi == fenergcodi && x.Equicodi == equicodi).ToList();
            if (equicodi == 290 && fenergcodi == ConstantesPR5ReportesServicio.FenergcodiGas)
                listaHoxEqF = listaHoUnidad.Where(x => x.Fenergcodi == fenergcodi && x.Equipadre == equicodi).ToList();

            if (listaHoxEqF.Any())
            {
                List<int> listaHopcodi = listaHoxEqF.Select(x => x.Hopcodipadre.Value).ToList();

                List<EveHoraoperacionDTO> listaHmodoXEqF = listaHoModo.Where(x => listaHopcodi.Contains(x.Hopcodi))
                                                            .OrderByDescending(x => (x.Hophorfin.Value - x.Hophorini.Value).TotalMinutes).ToList();

                //modo de operación con mayor duracion
                if (listaHmodoXEqF.Any())
                {
                    int grupocodiRepresentativo = listaHmodoXEqF.FirstOrDefault().Grupocodi.Value;

                    return grupocodiRepresentativo;
                }
            }

            return null;
        }

        /// <summary>
        /// Convertir horas de operacion de modos de operacion (combinado y otros) a modos de ciclo simple
        /// </summary>
        /// <param name="listaHO"></param>
        /// <param name="listaUnidad"></param>
        /// <returns></returns>
        private List<EveHoraoperacionDTO> ListarHOCCtoCicloSimple(List<EveHoraoperacionDTO> listaHO, List<EqEquipoDTO> listaUnidad)
        {
            List<EveHoraoperacionDTO> listaHoNuevo = new List<EveHoraoperacionDTO>();

            List<EveHoraoperacionDTO> listaHoModo = listaHO.Where(x => x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoModo).OrderBy(x => x.Hophorini).ToList();
            List<EveHoraoperacionDTO> listaHoUnidad = listaHO.Where(x => x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoUnidad).OrderBy(x => x.Hophorini).ToList();

            //se asume que todas las unidades son de ciclo simple
            List<EqEquipoDTO> listaCS = listaUnidad; //.Where(x => x.TieneCalculoCS).ToList();

            int hopcodiCont = listaHO.Any() ? listaHO.Max(x => x.Hopcodi) + 1 : 1;

            List<int> listaEquipadre = listaCS.Select(x => x.Equipadre.Value).Distinct().ToList();
            foreach (var equipadre in listaEquipadre)
            {
                //lista de horas de operación a nivel de modo
                List<EveHoraoperacionDTO> listaHoModoXCentral = listaHoModo.Where(x => x.Equipadre == equipadre).OrderBy(x => x.Hophorini).ToList();
                foreach (var hoModo in listaHoModoXCentral)
                {
                    if (hoModo.Hopcodi == 697589)
                    { }

                    //detalle por unidades
                    var listaHoUni = listaHoUnidad.Where(x => x.Hopcodipadre == hoModo.Hopcodi).ToList();
                    foreach (var hoUni in listaHoUni)
                    {
                        var regCS = listaCS.Find(x => x.Equicodi == hoUni.Equicodi && x.Fenergcodi == hoModo.Fenergcodi);
                        if (regCS != null)
                        {
                            //cambiar el grupo al del modo de operación principal del combustible
                            EveHoraoperacionDTO reg1 = (EveHoraoperacionDTO)hoModo.Clone();
                            reg1.Grupocodi = regCS.Grupocodi;
                            reg1.Hopcodi = hopcodiCont;

                            hopcodiCont++;
                            EveHoraoperacionDTO reg2 = (EveHoraoperacionDTO)hoUni.Clone();
                            reg2.Grupocodi = regCS.Grupocodi;
                            //reg2.Hopcodi = hopcodiCont;
                            reg2.Hopcodipadre = reg1.Hopcodi;

                            listaHoNuevo.Add(reg1);
                            listaHoNuevo.Add(reg2);
                        }
                    }
                }
            }

            return listaHoNuevo;
        }

        /// <summary>
        /// Agrupar las horas de operación por modo de operación
        /// </summary>
        /// <param name="listaHOP"></param>
        /// <param name="listaGrupoModo"></param>
        /// <returns></returns>
        private List<DetalleHoraOperacionModo> ListarDetalleHO(List<EveHoraoperacionDTO> listaHOP, List<PrGrupoDTO> listaGrupoModo)
        {
            List<EveHoraoperacionDTO> listaHoModo = listaHOP.Where(x => x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoModo).OrderBy(x => x.Hophorini).ToList();
            List<EveHoraoperacionDTO> listaHoUnidad = listaHOP.Where(x => x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoUnidad).OrderBy(x => x.Hophorini).ToList();

            //
            List<DetalleHoraOperacionModo> lista = new List<DetalleHoraOperacionModo>();

            foreach (var reg in listaGrupoModo)
            {
                var sublistaHo = listaHoModo.Where(x => x.Grupocodi == reg.Grupocodi).OrderBy(x => x.Hophorini).ToList();
                if (sublistaHo.Any())
                {
                    List<string> listaHOdesc = sublistaHo.Select(x => GetDescripcionHo(x)).ToList();

                    lista.Add(new DetalleHoraOperacionModo()
                    {
                        Gruponomb = reg.Gruponomb,
                        Grupocodi = reg.Grupocodi,
                        ListaHO = sublistaHo,
                        ListaHODesc = listaHOdesc,
                        Comentario = string.Join("\n\n", listaHOdesc)
                    });
                }

                if (reg.TieneModoEspecial)
                {
                    for (int i = 0; i < reg.ListaEquicodi.Count; i++)
                    {
                        int equicodi = reg.ListaEquicodi[i];
                        List<string> listaHOEqdesc = new List<string>();

                        var sublistaHoEq = listaHoUnidad.Where(x => x.Equicodi == equicodi).OrderBy(x => x.Hophorini).ToList();
                        foreach (var ho in sublistaHoEq)
                        {
                            var hoPadre = listaHoModo.Find(x => x.Hopcodi == ho.Hopcodipadre);
                            ho.Subcausadesc = hoPadre != null ? hoPadre.Subcausadesc : "";
                            ho.Gruponomb = hoPadre != null ? hoPadre.Gruponomb : "";
                            listaHOEqdesc.Add(GetDescripcionHo(ho));
                        }

                        lista.Add(new DetalleHoraOperacionModo()
                        {
                            Gruponomb = reg.Gruponomb + " (" + reg.ListaEquiabrev[i] + ")",
                            Grupocodi = reg.Grupocodi,
                            Equicodi = equicodi,
                            ListaHO = sublistaHoEq,
                            ListaHODesc = listaHOEqdesc,
                            Comentario = string.Join("\n\n", listaHOEqdesc)
                        });
                    }
                }
            }

            return lista;
        }

        /// <summary>
        /// Obtener descripción de la hora de operación (aparece como comentario en excel)
        /// </summary>
        /// <param name="ho"></param>
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

        private List<ResultadoValidacionAplicativo> ListarMensajeValidacionConsumoAlternativoArranque(DateTime fechaPeriodo, List<CccReporteDTO> listaRpt, List<DetalleHoraOperacionModo> listaDetalleHOCs)
        {
            List<ResultadoValidacionAplicativo> listaMsj = new List<ResultadoValidacionAplicativo>();

            //si no existe horas de operación (entra en PARALELO el día siguiente) pero consumo combustible, asumir que tiene un arranque
            //if (numArranques == 0 && regAlt.Cccrptvalorreal > 0)
            //{
            //    numArranques = 1;
            //}

            var listaUniAlt = listaRpt.Where(x => x.Cccrptflagtienecurva == ConstantesConsumoCombustible.FlagCombustibleArranque).ToList();

            foreach (var regAlt in listaUniAlt)
            {
                var regUnidad = listaRpt.Find(x => x.Equicodi == regAlt.Equicodi && x.Cccrptflagtienecurva != ConstantesConsumoCombustible.FlagCombustibleArranque);

                if (regUnidad != null)
                {
                    GetDatosHOCombAltArranque(regUnidad.Grupocodi ?? 0, regAlt.Equipadre, regUnidad.Equinomb, fechaPeriodo, listaDetalleHOCs
                                            , out int numArranques, out List<ResultadoValidacionAplicativo> listaMsj2);
                    listaMsj.AddRange(listaMsj2);

                    //alternativo
                    if (regAlt.Cccrptvalorreal > 0 && numArranques == 0)
                    {
                        listaMsj.Add(new ResultadoValidacionAplicativo()
                        {
                            Equipadre = regUnidad.Equipadre,
                            Equicodi = regUnidad.Equicodi,
                            Descripcion = string.Format("El generador {0} no tiene órdenes de arranque para calcular el consumo teórico.", regUnidad.Equinomb)
                        });
                    }

                    //el principal tiene info y el alternativo no tiene data
                    if (regUnidad.Cccrptvalorreal > 0 && regAlt.Cccrptvalorreal.GetValueOrDefault(0) <= 0 && listaMsj2.Any())
                    {
                        listaMsj.Add(new ResultadoValidacionAplicativo()
                        {
                            Equipadre = regUnidad.Equipadre,
                            Equicodi = regUnidad.Equicodi,
                            Fenergcodi = regAlt.Fenergcodi,
                            Descripcion = string.Format("El generador {0} no tiene consumo real para el combustible de arranque.", regUnidad.Equinomb)
                        });
                    }
                }
            }

            return listaMsj;
        }

        private void GetDatosHOCombAltArranque(int grupocodi, int equipadre, string equinomb, DateTime fechaPeriodo, List<DetalleHoraOperacionModo> listaDetalleHOCs
                                            , out int numArranques, out List<ResultadoValidacionAplicativo> listaMsj)
        {
            listaMsj = new List<ResultadoValidacionAplicativo>();

            var regDetHo = listaDetalleHOCs.Find(x => x.Grupocodi == grupocodi);
            numArranques = 0;

            if (regDetHo != null && regDetHo.ListaHO != null && regDetHo.ListaHO.Any())
            {
                var listaHoValido = regDetHo.ListaHO.OrderBy(x => x.Hophorini).ToList();

                for (int i = 0; i < listaHoValido.Count; i++)
                {
                    var regHoActual = listaHoValido[i];
                    var regAnterior = i != 0 ? listaHoValido[i - 1] : null;
                    bool tieneAnterior = i != 0;
                    if (regAnterior != null && regHoActual != null)
                    {
                        var totalMin = (regHoActual.Hophorini.Value - regAnterior.Hophorfin.Value).TotalMinutes;
                        tieneAnterior = totalMin <= 1;
                    }

                    if (!tieneAnterior && regHoActual.Hophorini.Value > fechaPeriodo)
                    {
                        numArranques += (regHoActual.Hophorordarranq != null ? 1 : 0);

                        if (regHoActual.Hophorordarranq == null)
                        {
                            listaMsj.Add(new ResultadoValidacionAplicativo()
                            {
                                Equipadre = equipadre,
                                Descripcion = string.Format("El generador {0} no tiene orden de arranque para la hora {1}.", equinomb, regHoActual.Hophorini.Value.ToString(ConstantesAppServicio.FormatoFechaFull2))
                            });
                        }
                    }
                }
            }


        }

        #endregion

        #region ALGORITMO CCC: Reporte Diario Cálculo Consumo, Reporte de Desviación de Consumo de Combustible (Web, Gráfico y Excel)

        /// <summary>
        /// Genera tabla Html listado versión por fecha
        /// </summary>
        /// <param name="fechaPeriodo"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GenerarTablaHtmlVersionDiario(DateTime fechaPeriodo, string url)
        {
            List<CccVersionDTO> listaVersion = GetByCriteriaCccVersions(fechaPeriodo, fechaPeriodo, ConstantesConsumoCombustible.HorizonteDiario);

            StringBuilder str = new StringBuilder();
            str.Append("<table class='pretty tabla-adicional' border='0' cellspacing='0' width='100%' id='tabla_version'>");

            str.Append("<thead>");
            #region cabecera
            str.Append("<tr>");
            str.Append("<th style='width: 100px'>Opciones</th>");
            str.Append("<th style=''>N° Versión</th>");
            str.Append("<th style=''>Fecha periodo</th>");
            //str.Append("<th style=''>Observación</th>");
            str.Append("<th style=''>Usuario creación</th>");
            str.Append("<th style=''>Fecha creación</th>");
            str.Append("</tr>");
            #endregion
            str.Append("</thead>");

            str.Append("<tbody>");
            #region cuerpo

            foreach (var reg in listaVersion)
            {
                str.Append("<tr>");

                str.Append("<td>");
                str.AppendFormat("<a class='' href='JavaScript:verReporte({0});' style='margin-right: 4px;'><img style='margin-top: 4px; margin-bottom: 4px;' src='{1}Content/Images/btn-open.png' title='Ver reporte' /></a>", reg.Cccvercodi, url);
                str.AppendFormat("<a class='' href='JavaScript:exportarReporte({0});' style='margin-right: 4px;'><img style='margin-top: 4px; margin-bottom: 4px;margin-left: 15px; margin-right: 15px;' src='{1}Content/Images/ExportExcel.png' title='Exportar reporte' /></a>", reg.Cccvercodi, url);
                str.AppendFormat("<a class='' href='JavaScript:verGrafico({0});' style='margin-right: 4px;'><img style='margin-top: 4px; margin-bottom: 4px;' src='{1}Content/Images/ContextMenu/grafico.png' title='Gráficos' /></a>", reg.Cccvercodi, url);
                str.Append("</td>");

                str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.Cccvernumero);
                str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.CccverfechaDesc);
                //str.AppendFormat("<td class='' style='text-align: left'>{0}</td>", reg.Cccverobs.Replace(ConstantesConsumoCombustible.SeparadorObs, "<br/>"));
                str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.Cccverusucreacion);
                str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.CccverfeccreacionDesc);

                str.Append("</tr>");
            }

            #endregion
            str.Append("</tbody>");

            str.Append("</table>");

            return str.ToString();
        }

        #region Cálculo

        /// <summary>
        /// Procesar Cálculo de consumo de combustible
        /// </summary>
        /// <param name="fechaPeriodo"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public int ProcesarCalculoConsumoCombustible(DateTime fechaPeriodo, string usuario, bool esEjecucionMensual)
        {
            DateTime fechaRegistro = DateTime.Now;

            CccVersionDTO regVersion = new CccVersionDTO()
            {
                Cccverhorizonte = ConstantesConsumoCombustible.HorizonteDiario,
                Cccverfecha = fechaPeriodo.Date,
                Cccvernumero = GetNumeroVersionActual(fechaPeriodo, ConstantesConsumoCombustible.HorizonteDiario),
                Cccverestado = ConstantesConsumoCombustible.EstadoGenerado,
                Cccverfeccreacion = fechaRegistro,
                Cccverusucreacion = usuario,
            };

            List<CccReporteDTO> listaRpt = ListarReporteCCC(fechaPeriodo, "-1", "-1", out ReporteConsumoCombustible objDatosReporte);

            List<string> listaObs = new List<string>();
            if (esEjecucionMensual) listaObs.Add("Generado en Reporte VCOM");
            listaObs.AddRange(objDatosReporte.ListaMsj.Select(x => x.Descripcion));

            string obs = string.Join(ConstantesConsumoCombustible.SeparadorObs, listaObs);
            regVersion.Cccverobs = obs.Length > 1500 ? obs.Substring(0, 1500) : obs;

            bool tieneCambio = ExisteCambioInformacion(fechaPeriodo, listaRpt);
            if (tieneCambio)
                return GuardarReporteCCCTransaccional(regVersion, listaRpt);

            return 0;
        }

        /// <summary>
        /// Guardado transaccional
        /// </summary>
        /// <param name="regVersion"></param>
        /// <param name="listaRpt"></param>
        /// <returns></returns>
        private int GuardarReporteCCCTransaccional(CccVersionDTO regVersion, List<CccReporteDTO> listaRpt)
        {

            DbTransaction tran = null;
            int ivercodi = 0;
            try
            {
                IDbConnection conn = FactorySic.GetSiMigracionRepository().BeginConnection();
                tran = FactorySic.GetSiMigracionRepository().StartTransaction(conn);

                ivercodi = this.SaveCccVersion(regVersion, conn, tran);

                int maxIdRptcodi = FactorySic.GetCccReporteRepository().GetMaxId();
                foreach (var regRpt in listaRpt)
                {
                    //guardar bd
                    regRpt.Cccvercodi = ivercodi;
                    regRpt.Cccrptcodi = maxIdRptcodi;
                    Logger.Info("GRUPO " + regRpt.Grupocodi);
                    Logger.Info("TIPO INFO " + regRpt.Tipoinfocodi);
                    Console.WriteLine("GRUPO " + regRpt.Grupocodi);
                    Console.WriteLine("TIPO INFO " + regRpt.Tipoinfocodi);
                    this.SaveCccReporte(regRpt, conn, tran);

                    maxIdRptcodi++;
                }

                tran.Commit();
            }
            catch (Exception ex)
            {
                ivercodi = 0;
                if (tran != null)
                    tran.Rollback();
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }


            return ivercodi;
        }

        /// <summary>
        /// número de version actual
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        private int GetNumeroVersionActual(DateTime fecha, string horizonte)
        {
            //obtener el numero de version
            List<CccVersionDTO> listaReporteBD = this.GetByCriteriaCccVersions(fecha, fecha, horizonte);
            var rptnumversion = listaReporteBD.Any() ? listaReporteBD.Max(x => x.Cccvernumero) + 1 : 1;

            return rptnumversion;
        }

        /// <summary>
        /// Verificar si existe cambio en el detalle del CCC
        /// </summary>
        /// <param name="fechaPeriodo"></param>
        /// <param name="listaRptMemoria"></param>
        /// <returns></returns>
        private bool ExisteCambioInformacion(DateTime fechaPeriodo, List<CccReporteDTO> listaRptMemoria)
        {
            List<CccVersionDTO> listaVersBD = this.GetByCriteriaCccVersions(fechaPeriodo, fechaPeriodo, ConstantesConsumoCombustible.HorizonteDiario);
            var vercodi = listaVersBD.Any() ? listaVersBD.Max(x => x.Cccvercodi) : -1;

            if (vercodi > 0)
            {
                List<CccReporteDTO> listaRptBD = GetByCriteriaCccReportes(vercodi.ToString());

                //comparar bd con memory
                foreach (var reg1 in listaRptBD)
                {
                    var reg2 = listaRptMemoria.Find(x => x.Equicodi == reg1.Equicodi 
                                                        && x.Grupocodi == reg1.Grupocodi 
                                                        && x.Equipadre == reg1.Equipadre
                                                         && x.Emprcodi == reg1.Emprcodi
                                                        && x.Fenergcodi == reg1.Fenergcodi && x.Grupocodi == reg1.Grupocodi 
                                                        && x.Mogrupocodi == reg1.Mogrupocodi && x.Tipoinfocodi == reg1.Tipoinfocodi);
                    if (reg2 != null)
                    {
                        if (reg2.Cccrptvalorreal != reg1.Cccrptvalorreal)
                            return true;

                        if (reg2.Cccrptvalorteorico != reg1.Cccrptvalorteorico)
                            return true;

                        if (reg2.Cccrptvariacion != reg1.Cccrptvariacion)
                            return true;
                    }
                    else
                    {
                        return true;
                    }
                }


                //comparar memory con bd
                foreach (var reg1 in listaRptMemoria)
                {
                    var reg2 = listaRptBD.Find(x => x.Equicodi == reg1.Equicodi && x.Grupocodi == reg1.Grupocodi && x.Equipadre == reg1.Equipadre
                                                        && x.Fenergcodi == reg1.Fenergcodi && x.Grupocodi == reg1.Grupocodi && x.Mogrupocodi == reg1.Mogrupocodi && x.Tipoinfocodi == reg1.Tipoinfocodi);
                    if (reg2 != null)
                    {
                        if (reg2.Cccrptvalorreal != reg1.Cccrptvalorreal)
                            return true;

                        if (reg2.Cccrptvalorteorico != reg1.Cccrptvalorteorico)
                            return true;

                        if (reg2.Cccrptvariacion != reg1.Cccrptvariacion)
                            return true;
                    }
                    else
                    {
                        return true;
                    }
                }

                return false;
            }
            else
            {
                return true;//no existe bd
            }
        }

        #endregion

        #region Información almacenada en BD

        /// <summary>
        /// Obtener el reporte con los filtros
        /// </summary>
        /// <param name="vercodi"></param>
        /// <param name="empresa"></param>
        /// <param name="central"></param>
        /// <returns></returns>
        public List<CccReporteDTO> ListarReporte(int vercodi, string empresa, string central)
        {
            CccVersionDTO regVersion = GetByIdCccVersion(vercodi);

            this.ListaDataXVersionReporte(vercodi, empresa, central
                                            , out List<CccReporteDTO> listaReptotOut
                                            , out List<SiEmpresaDTO> listaEmpresa
                                            , out List<EqEquipoDTO> listaCentral
                                            , out List<EqEquipoDTO> listaEquipo);

            List<LeyendaCCC> listaLeyenda = ListarLeyenda(regVersion.Cccverfecha);

            //Formatear reporte

            foreach (var reg in listaReptotOut)
            {
                FormatearCCCReporte(reg, listaLeyenda);
            }

            return listaReptotOut;
        }

        /// <summary>
        /// Formatear los reportes
        /// </summary>
        /// <param name="reg"></param>
        /// <param name="listaLeyenda"></param>
        private void FormatearCCCReporte(CccReporteDTO reg, List<LeyendaCCC> listaLeyenda)
        {
            NumberFormatInfo nfi4 = new CultureInfo("en-US", false).NumberFormat;
            nfi4.NumberGroupSeparator = " ";
            nfi4.NumberDecimalDigits = 4;
            nfi4.NumberDecimalSeparator = ",";

            NumberFormatInfo nfi2 = new CultureInfo("en-US", false).NumberFormat;
            nfi2.NumberGroupSeparator = " ";
            nfi2.NumberDecimalDigits = 2;
            nfi2.NumberDecimalSeparator = ",";

            reg.CccrptvalorrealDesc = reg.Cccrptvalorreal != null ? reg.Cccrptvalorreal.Value.ToString("N", nfi4) : string.Empty;
            reg.CccrptvalorteoricoDesc = reg.Cccrptvalorteorico != null ? reg.Cccrptvalorteorico.Value.ToString("N", nfi4) : string.Empty;
            reg.CccrptvariacionDesc = reg.Cccrptvariacion != null ? (reg.Cccrptvariacion.Value * 100).ToString("N", nfi2) + "%" : string.Empty;

            //falta real
            if (reg.Cccrptvalorreal.GetValueOrDefault(0) <= 0 && reg.Cccrptvalorteorico.GetValueOrDefault(0) > 0)
                reg.PintarCeldaFaltaValorReal = true;

            //falta teorico
            if (reg.Cccrptvalorteorico.GetValueOrDefault(0) <= 0 && reg.Cccrptvalorreal.GetValueOrDefault(0) > 0)
                reg.PintarCeldaFaltaValorTeorico = true;

            decimal variacion = reg.Cccrptvariacion.GetValueOrDefault(0);
            var regLeyenda = listaLeyenda.Find(x => x.Min <= variacion && variacion <= x.Max);
            reg.ColorFondo = regLeyenda != null ? regLeyenda.Color : "#FFFFFF";
            reg.TieneTransgresion = regLeyenda != null ? regLeyenda.TieneTransgresion : false;
        }

        /// <summary>
        /// listar leyenda
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <returns></returns>
        public List<LeyendaCCC> ListarLeyenda(DateTime fechaConsulta)
        {
            decimal param = 0.10m;

            List<LeyendaCCC> lista = new List<LeyendaCCC>();

            lista.Add(new LeyendaCCC() { Min = -1, Max = param * -1, Color = "#F07896", TieneTransgresion = true }); //Desviación entre -100% y -10.00%
            lista.Add(new LeyendaCCC() { Min = param * -1, Max = param * -1 + 0.02m, Color = "#FFF2CC", TieneTransgresion = false }); //Desviación entre -10.00% y -8.00%
            lista.Add(new LeyendaCCC() { Min = param * -1 + 0.02m, Max = param - 0.02m, Color = "#E2EFDA", TieneTransgresion = false }); //Desviación entre -8.00% y 8.00%
            lista.Add(new LeyendaCCC() { Min = param - 0.02m, Max = param, Color = "#FFF2CC", TieneTransgresion = false }); //Desviación entre 8.00% y 10.00%
            lista.Add(new LeyendaCCC() { Min = param, Max = 1, Color = "#F07896", TieneTransgresion = true }); //Desviación entre 10.00% y 100%

            foreach (var reg in lista)
            {
                reg.Descripcion = string.Format("Desviación entre {0}% y {1}%", reg.Min * 100, reg.Max * 100);
            }

            return lista.OrderBy(x => x.Min).ThenBy(x => x.Max).ToList();
        }

        /// <summary>
        /// obtener los filtros
        /// </summary>
        /// <param name="ivercodi"></param>
        /// <param name="empresa"></param>
        /// <param name="central"></param>
        /// <param name="listaReptotOut"></param>
        /// <param name="listaEmpresa"></param>
        /// <param name="listaCentral"></param>
        /// <param name="listaEquipo"></param>
        public void ListaDataXVersionReporte(int ivercodi, string empresa, string central
                                                , out List<CccReporteDTO> listaReptotOut
                                                , out List<SiEmpresaDTO> listaEmpresa
                                                , out List<EqEquipoDTO> listaCentral
                                                , out List<EqEquipoDTO> listaEquipo)
        {
            CccVersionDTO regVersion = GetByIdCccVersion(ivercodi);
            List<CccReporteDTO> listaRepdet = GetByCriteriaCccReportes(ivercodi.ToString());

            empresa = !string.IsNullOrEmpty(empresa) ? empresa : ConstantesAppServicio.ParametroDefecto;
            central = !string.IsNullOrEmpty(central) ? central : ConstantesAppServicio.ParametroDefecto;

            if (ConstantesAppServicio.ParametroDefecto != empresa)
            {
                int[] emprcodis = empresa.Split(',').Select(x => int.Parse(x)).ToArray();
                listaRepdet = listaRepdet.Where(x => emprcodis.Contains(x.Emprcodi)).ToList();
            }

            if (ConstantesAppServicio.ParametroDefecto != central)
            {
                int[] equipadres = central.Split(',').Select(x => int.Parse(x)).ToArray();
                listaRepdet = listaRepdet.Where(x => equipadres.Contains(x.Equipadre)).ToList();
            }

            listaEmpresa = listaRepdet.GroupBy(x => x.Emprcodi).Select(x => new SiEmpresaDTO() { Emprcodi = x.Key, Emprnomb = x.First().Emprnomb }).OrderBy(x => x.Emprnomb).ToList();
            listaCentral = listaRepdet.GroupBy(x => x.Equipadre).Select(x => new EqEquipoDTO() { Equipadre = x.Key, Central = x.First().Central, Emprcodi = x.First().Emprcodi }).OrderBy(x => x.Central).ToList();
            listaEquipo = listaRepdet.GroupBy(x => x.Equicodi).Select(x => new EqEquipoDTO() { Equicodi = x.Key, Equinomb = x.First().Equinomb, Equipadre = x.First().Equipadre, Emprcodi = x.First().Emprcodi }).OrderBy(x => x.Equinomb).ToList();

            listaReptotOut = listaRepdet;
        }

        #endregion

        #region Web y gráfico

        /// <summary>
        /// Genera tabla Html listado versión por fecha
        /// </summary>
        /// <param name="fechaPeriodo"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GenerarTablaHtmlListadoReporte(int vercodi, string empresa, string central)
        {
            List<CccReporteDTO> listaRept = ListarReporte(vercodi, empresa, central);

            return GenerarTablaHtmlListadoRptDiario(listaRept);
        }

        /// <summary>
        /// Tabla del reporte. utilizado para web y correo
        /// </summary>
        /// <param name="listaRept"></param>
        /// <returns></returns>
        public string GenerarTablaHtmlListadoRptDiario(List<CccReporteDTO> listaRept)
        {

            int numFilasObs = listaRept.Where(x => x.PintarCeldaFaltaValorReal || x.PintarCeldaFaltaValorTeorico).Count();
            int numFilaHo = listaRept.Where(x => x.TieneAlertaHo).Count();

            StringBuilder str = new StringBuilder();

            if (numFilasObs > 0)
                str.AppendFormat("<div><span style='padding-left: 50px;background-color: {1};padding-top: 5px;padding-bottom: 5px; margin-right: 10px;'></span> Existen {0} unidade(s) sin información de Consumo Real o Consumo teórico.</div>", numFilasObs, ConstantesConsumoCombustible.ColorFaltaValorConsumo);

            if (numFilaHo > 0)
                str.AppendFormat("<div><span style='padding-left: 50px;background-color: {1};padding-top: 5px;padding-bottom: 5px; margin-right: 10px;'></span> Existen {0} unidade(s) con hora(s) de operación con calificación de POR PRUEBAS, POR TENSIÓN o POR SEGURIDAD.</div>", numFilasObs, ConstantesConsumoCombustible.ColorAlertaHO);

            str.Append("<table class='pretty tabla-adicional' border='0' cellspacing='0' width='100%' id='tabla_reporte'>");

            str.Append("<thead>");
            #region cabecera
            str.Append("<tr>");
            str.Append("<th style=''>Empresa</th>");
            str.Append("<th style=''>Central</th>");
            str.Append("<th style=''>Unidad de generación</th>");
            str.Append("<th style=''>Tipo de combustible</th>");
            str.Append("<th style=''>Consumo REAL</th>");
            str.Append("<th style=''>Consumo TEÓRICO</th>");
            str.Append("<th style=''>Variación</th>");
            str.Append("</tr>");
            #endregion
            str.Append("</thead>");

            str.Append("<tbody>");
            #region cuerpo

            foreach (var reg in listaRept)
            {
                str.Append("<tr>");

                string colorReal = "";
                string colorTeorico = "";
                string descReal = string.Empty;
                string descTeorico = string.Empty;

                if (reg.TieneAlertaHo)
                {
                    colorTeorico = ConstantesConsumoCombustible.ColorAlertaHO;
                }

                if (reg.PintarCeldaFaltaValorReal)
                {
                    colorReal = ConstantesConsumoCombustible.ColorFaltaValorConsumo;
                }
                if (reg.PintarCeldaFaltaValorTeorico)
                {
                    colorTeorico = ConstantesConsumoCombustible.ColorFaltaValorConsumo;
                }

                string scolorReal = string.Format("background-color: {0} !important;", colorReal);
                string scolorTeorico = string.Format("background-color: {0} !important;", colorTeorico);

                str.AppendFormat("<td class='emprcodi_{1}' style='text-align: center;height: 20px;'>{0}</td>", reg.Emprnomb, reg.Emprcodi);
                str.AppendFormat("<td class='equipadre_{1}' style='text-align: center'>{0}</td>", reg.Central, reg.Equipadre);
                str.AppendFormat("<td class='equicodi_{1}' style='text-align: center'>{0}</td>", reg.Equinomb, reg.Equicodi);
                str.AppendFormat("<td class='fenergcodi_{1}' style='text-align: center'>{0}</td>", reg.Fenergnomb, reg.Fenergcodi);
                str.AppendFormat("<td class='' style='text-align: center; {1}'>{0}</td>", reg.CccrptvalorrealDesc, scolorReal);
                str.AppendFormat("<td class='' style='text-align: center; {1}'>{0}</td>", reg.CccrptvalorteoricoDesc, scolorTeorico);
                str.AppendFormat("<td class='' style='text-align: center; background-color: {1}'>{0}</td>", reg.CccrptvariacionDesc, reg.ColorFondo);

                str.Append("</tr>");
            }

            #endregion
            str.Append("</tbody>");

            str.Append("</table>");

            return str.ToString();
        }

        /// <summary>
        /// Gráfico
        /// </summary>
        /// <param name="vercodi"></param>
        /// <param name="empresa"></param>
        /// <param name="central"></param>
        /// <returns></returns>
        public List<GraficoWeb> ListarGraficoXUnidad(int vercodi, int empresa, int central)
        {
            CccVersionDTO regVersion = GetByIdCccVersion(vercodi);

            List<CccReporteDTO> listaRept = ListarReporte(vercodi, empresa.ToString(), central.ToString());
            List<LeyendaCCC> listaLeyenda = ListarLeyenda(regVersion.Cccverfecha);

            List<EqEquipoDTO> listaCentral = listaRept.GroupBy(x => x.Equipadre)
                                                    .Select(x => new EqEquipoDTO()
                                                    {
                                                        Equipadre = x.Key,
                                                        Central = x.First().Central ?? "",
                                                        Emprnomb = x.First().Emprnomb ?? "",
                                                        Emprabrev = x.First().Emprabrev ?? ""
                                                    })
                                                    .OrderBy(x => x.Emprnomb).ThenBy(x => x.Central).ToList();

            List<CccReporteDTO> listaCalculo = ListarReporteCCC(regVersion.Cccverfecha, empresa.ToString(), central.ToString(), out ReporteConsumoCombustible objRpt);

            return objRpt.ListaCeldaDetalle.Select(x => x.Grafico).ToList();
        }

        #endregion

        #region Exportación

        /// <summary>
        /// Generar archivo excel
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="vercodi"></param>
        /// <param name="empresa"></param>
        /// <param name="central"></param>
        /// <param name="nameFile"></param>
        public void GenerarRptExcel(string ruta, int vercodi, string empresa, string central, out string nameFile)
        {
            CccVersionDTO regVersion = GetByIdCccVersion(vercodi);

            List<CccReporteDTO> listaRept = ListarReporte(vercodi, empresa, central);
            List<LeyendaCCC> listaLeyenda = ListarLeyenda(regVersion.Cccverfecha);

            List<EqEquipoDTO> listaCentral = listaRept.GroupBy(x => x.Equipadre)
                                                    .Select(x => new EqEquipoDTO()
                                                    {
                                                        Equipadre = x.Key,
                                                        Central = x.First().Central ?? "",
                                                        Emprnomb = x.First().Emprnomb ?? "",
                                                        Emprabrev = x.First().Emprabrev ?? ""
                                                    })
                                                    .OrderBy(x => x.Emprnomb).ThenBy(x => x.Central).ToList();

            List<CccReporteDTO> listaCalculo = ListarReporteCCC(regVersion.Cccverfecha, empresa, central, out ReporteConsumoCombustible objRpt);

            foreach (var reg in listaRept)
            {
                var regCalculo = listaCalculo.Find(x => x.Equicodi == reg.Equicodi && x.Fenergcodi == reg.Fenergcodi);
                if (regCalculo != null)
                    reg.TieneAlertaHo = regCalculo.TieneAlertaHo; //esta info no está en bd

                var listaMsj = objRpt.ListaMsj.Where(x => x.Equipadre == reg.Equipadre);
                reg.TieneAlertaFaltaDataCoes = listaMsj.Any();

                //mensajes consumo alternativo
                var listaMsjxUnidad = listaMsj.Where(x => x.Equicodi == reg.Equicodi && x.Fenergcodi == reg.Fenergcodi);
                if (listaMsjxUnidad.Any())
                {
                    //falta real
                    if (reg.Cccrptvalorreal.GetValueOrDefault(0) <= 0)
                        reg.PintarCeldaFaltaValorReal = true;
                }
            }

            //Nombre de archivo
            nameFile = string.Format("CálculoConsumoDeCombustible_{0}_revis_{1}.xlsx", regVersion.Cccverfecha.ToString(ConstantesAppServicio.FormatoFechaYMD), regVersion.Cccvernumero);

            string rutaFile = ruta + nameFile;

            FileInfo newFile = new FileInfo(rutaFile);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                GenerarHojaExcel(xlPackage, "Resultados", 6, 2, regVersion.Cccverfecha, listaRept, listaLeyenda, objRpt.ListaMsj);
                xlPackage.Save();

                foreach (var regCentral in listaCentral)
                {
                    if (regCentral.Equipadre == 290)
                    { }

                    List<CccReporteDTO> listaReptXCentral = listaRept.Where(x => x.Equipadre == regCentral.Equipadre).ToList();

                    string nameWs = regCentral.Emprabrev.Trim() + "_" + (regCentral.Central.Replace("C.T.", "")).Trim();
                    if (nameWs.Length > 30) nameWs = nameWs.Substring(0, 29);

                    GenerarHojaExcelDetalle(xlPackage, nameWs, 1, 2, regVersion.Cccverfecha, listaReptXCentral, listaLeyenda, regCentral.Equipadre.Value, objRpt);
                    xlPackage.Save();
                }
            }
        }

        /// <summary>
        /// Generar hoja principal
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="nameWS"></param>
        /// <param name="rowIniTabla"></param>
        /// <param name="colIniTabla"></param>
        /// <param name="listaRept"></param>
        /// <param name="listaLeyenda"></param>
        /// <param name="listaMjs"></param>
        private void GenerarHojaExcel(ExcelPackage xlPackage, string nameWS, int rowIniTabla, int colIniTabla, DateTime fecha
                                                            , List<CccReporteDTO> listaRept, List<LeyendaCCC> listaLeyenda, List<ResultadoValidacionAplicativo> listaMjs)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            GenerarTablaResultadoExcel(true, ws, rowIniTabla, colIniTabla, fecha, listaRept, listaLeyenda, listaMjs);
        }

        /// <summary>
        /// Generar la tabla. Reutilizado para la pestaña principal y para las centrales
        /// </summary>
        /// <param name="incluirCaracteristica"></param>
        /// <param name="ws"></param>
        /// <param name="rowIniTabla"></param>
        /// <param name="colIniTabla"></param>
        /// <param name="listaRept"></param>
        /// <param name="listaLeyenda"></param>
        /// <param name="listaMjs"></param>
        private void GenerarTablaResultadoExcel(bool incluirCaracteristica, ExcelWorksheet ws, int rowIniTabla, int colIniTabla, DateTime fecha
                                            , List<CccReporteDTO> listaRept, List<LeyendaCCC> listaLeyenda, List<ResultadoValidacionAplicativo> listaMjs)
        {
            string font = "Calibri";
            string colorCeldaFijo = "#0070C0";
            string colorTextoFijo = "#ffffff";

            string colorCeldaCuerpo = "#FFFFFF";
            string colorTextoCuerpo = "#000000";

            string colorLinea = "#000000";

            #region  Filtros y Cabecera

            int colEmpresa = colIniTabla;
            int colCentral = colEmpresa + 1;
            int colUnidad = colCentral + 1;
            int colTipo = colUnidad + 1;
            int colReal = colTipo + 1;
            int colTeorico = colReal + 1;
            int colVariacion = colTeorico + 1;

            int rowTitulo = rowIniTabla;
            ws.Cells[rowTitulo, colEmpresa].Value = string.Format("Resultados {0}", fecha.ToString(ConstantesAppServicio.FormatoFecha));
            UtilExcel.CeldasExcelAgrupar(ws, rowTitulo, colEmpresa, rowTitulo, colVariacion);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowTitulo, colEmpresa, rowTitulo, colVariacion, colorLinea);
            UtilExcel.SetFormatoCelda(ws, rowTitulo, colEmpresa, rowTitulo, colVariacion, "Centro", "Izquierda", colorTextoFijo, colorCeldaFijo, font, 12, true, true);

            int rowEmpresa = rowTitulo + 1;
            ws.Cells[rowEmpresa, colEmpresa].Value = "EMPRESA";
            ws.Cells[rowEmpresa, colCentral].Value = "CENTRAL";
            ws.Cells[rowEmpresa, colUnidad].Value = "UNIDAD";
            ws.Cells[rowEmpresa, colTipo].Value = "COMBUSTIBLE";
            ws.Cells[rowEmpresa, colReal].Value = "CONSUMO REAL";
            ws.Cells[rowEmpresa, colTeorico].Value = "CONSUMO TEÓRICO";
            ws.Cells[rowEmpresa, colVariacion].Value = "VARIACIÓN";
            UtilExcel.SetFormatoCelda(ws, rowEmpresa, colEmpresa, rowEmpresa, colVariacion, "Centro", "Centro", colorTextoFijo, colorCeldaFijo, font, 12, true);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowEmpresa, colEmpresa, rowEmpresa, colEmpresa, colorLinea);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowEmpresa, colCentral, rowEmpresa, colCentral, colorLinea);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowEmpresa, colUnidad, rowEmpresa, colUnidad, colorLinea);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowEmpresa, colTipo, rowEmpresa, colTipo, colorLinea);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowEmpresa, colReal, rowEmpresa, colReal, colorLinea);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowEmpresa, colTeorico, rowEmpresa, colTeorico, colorLinea);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowEmpresa, colVariacion, rowEmpresa, colVariacion, colorLinea);

            //ws.Row(rowEmpresa).Height = 36;
            ws.Column(1).Width = 3;
            ws.Column(colEmpresa).Width = 30;
            ws.Column(colCentral).Width = 20;
            ws.Column(colUnidad).Width = 20;
            ws.Column(colTipo).Width = 15;
            ws.Column(colReal).Width = 20;
            ws.Column(colTeorico).Width = 20;
            ws.Column(colVariacion).Width = 12;

            #endregion

            #region Leyenda

            int rowLeyenda = 1;
            int colLeyenda = colVariacion + 2;

            ws.Cells[rowLeyenda, colLeyenda].Value = "Leyenda";
            rowLeyenda++;

            foreach (var reg in listaLeyenda)
            {
                UtilExcel.SetFormatoCelda(ws, rowLeyenda, colLeyenda, rowLeyenda, colLeyenda, "Centro", "Derecha", colorTextoCuerpo, reg.Color, font, 12, false);
                UtilExcel.BorderCeldasLineaDelgada(ws, rowLeyenda, colLeyenda, rowLeyenda, colLeyenda, colorLinea);

                ws.Cells[rowLeyenda, colLeyenda + 1].Value = reg.Descripcion;

                rowLeyenda++;
            }

            #endregion

            #region Nota

            bool tieneCombArraAlt = listaRept.Count(x => x.Cccrptflagtienecurva == ConstantesConsumoCombustible.FlagCombustibleArranque) > 0;

            if (!incluirCaracteristica && tieneCombArraAlt)
            {
                int rowNota = 1;
                int rowFinNota = rowNota + 2;
                int colNota = colLeyenda + 5;

                ws.Cells[rowNota, colNota].Value = "Nota";

                //UtilExcel.SetFormatoCelda(ws, rowNota, colNota, rowFinNota, colNota, "Centro", "Izquierda", colorTextoCuerpo, colorTextoFijo, font, 12, false);
                //UtilExcel.BorderCeldasLineaDelgada(ws, rowNota, colNota, rowFinNota, colNota, colorLinea);

                ws.Cells[rowNota + 1, colNota].Value = "Si la unidad de generación tiene combustible alternativo para el arranque, según la parametrización de base de datos del COES, el valor de CONSUMO TEORICO se calculará de la siguiente forma:";
                ws.Cells[rowNota + 2, colNota].Value = " - CONSUMO TEORICO = (Número de ordenes de arranque) * (valor del combustible de arranque).";
            }

            #endregion

            #region Cuerpo

            int rowData = rowEmpresa;

            for (int i = 0; i < listaRept.Count; i++)
            {
                var reg = listaRept[i];

                rowData++;

                ws.Cells[rowData, colEmpresa].Value = reg.Emprnomb;
                ws.Cells[rowData, colCentral].Value = reg.Central;
                ws.Cells[rowData, colUnidad].Value = reg.Equinomb;
                ws.Cells[rowData, colTipo].Value = reg.Fenergnomb;
                UtilExcel.SetFormatoCelda(ws, rowData, colEmpresa, rowData, colTipo, "Centro", "Izquierda", colorTextoCuerpo, colorCeldaCuerpo, font, 12, false);

                string colorCentral = colorCeldaCuerpo;
                string descObsCentral = string.Empty;
                if (reg.TieneAlertaFaltaDataCoes)
                {
                    colorCentral = ConstantesConsumoCombustible.ColorFaltaValorDatosCOES;
                    descObsCentral = "No existe alguna información de Parámetros de modos de operación, Curva Potencia vs Consumo, Horas de Operación o Despacho Extranet.";
                }
                UtilExcel.SetFormatoCelda(ws, rowData, colCentral, rowData, colCentral, "Centro", "Izquierda", colorTextoCuerpo, colorCentral, font, 12, false);
                UtilExcel.AgregarComentarioExcel(ws, rowData, colCentral, descObsCentral);

                string colorReal = colorCeldaCuerpo;
                string colorTeorico = colorCeldaCuerpo;
                string descReal = string.Empty;
                string descTeorico = string.Empty;

                if (reg.TieneAlertaHo)
                {
                    colorTeorico = ConstantesConsumoCombustible.ColorAlertaHO;
                    descTeorico = "La unidad tiene hora(s) de operación con calificación POR PRUEBAS, POR TENSIÓN o POR SEGURIDAD." + "\n";
                }

                if (reg.PintarCeldaFaltaValorReal)
                {
                    colorReal = ConstantesConsumoCombustible.ColorFaltaValorConsumo;
                    descReal = "No tiene información de Consumo Real." + "\n";
                }
                if (reg.PintarCeldaFaltaValorTeorico)
                {
                    colorTeorico = ConstantesConsumoCombustible.ColorFaltaValorConsumo;
                    descTeorico = "No tiene información de Consumo Teórico." + "\n";
                }

                ws.Cells[rowData, colReal].Value = reg.Cccrptvalorreal;
                UtilExcel.SetFormatoCelda(ws, rowData, colReal, rowData, colReal, "Centro", "Derecha", colorTextoCuerpo, colorReal, font, 10, false);
                UtilExcel.AgregarComentarioExcel(ws, rowData, colReal, descReal);

                ws.Cells[rowData, colTeorico].Value = reg.Cccrptvalorteorico;
                UtilExcel.SetFormatoCelda(ws, rowData, colTeorico, rowData, colTeorico, "Centro", "Derecha", colorTextoCuerpo, colorTeorico, font, 10, false);
                UtilExcel.AgregarComentarioExcel(ws, rowData, colTeorico, descTeorico);

                UtilExcel.CeldasExcelIndentar(ws, rowData, colReal, rowData, colTeorico, 1);
                UtilExcel.CeldasExcelFormatoNumero(ws, rowData, colReal, rowData, colTeorico, 1);

                ws.Cells[rowData, colVariacion].Value = reg.Cccrptvariacion;
                UtilExcel.SetFormatoCelda(ws, rowData, colVariacion, rowData, colVariacion, "Centro", "Derecha", colorTextoCuerpo, reg.ColorFondo, font, 10, false);
                UtilExcel.CeldasExcelIndentar(ws, rowData, colVariacion, rowData, colVariacion, 1);
                UtilExcel.CeldasExcelFormatoPorcentaje(ws, rowData, colVariacion, rowData, colVariacion, 2);

                UtilExcel.BorderCeldasLineaDelgada(ws, rowData, colEmpresa, rowData, colEmpresa, colorLinea);
                UtilExcel.BorderCeldasLineaDelgada(ws, rowData, colCentral, rowData, colCentral, colorLinea);
                UtilExcel.BorderCeldasLineaDelgada(ws, rowData, colUnidad, rowData, colUnidad, colorLinea);
                UtilExcel.BorderCeldasLineaDelgada(ws, rowData, colTipo, rowData, colTipo, colorLinea);
                UtilExcel.BorderCeldasLineaDelgada(ws, rowData, colReal, rowData, colReal, colorLinea);
                UtilExcel.BorderCeldasLineaDelgada(ws, rowData, colTeorico, rowData, colTeorico, colorLinea);
                UtilExcel.BorderCeldasLineaDelgada(ws, rowData, colVariacion, rowData, colVariacion, colorLinea);
            }

            #endregion

            if (listaMjs.Any())
            {
                int rowIniMsj = rowData + 3;
                ws.Cells[rowIniMsj, colEmpresa].Value = "Observaciones:";
                UtilExcel.SetFormatoCelda(ws, rowIniMsj, colEmpresa, rowIniMsj, colEmpresa, "Centro", "Izquierda", "#FF0000", colorCeldaCuerpo, font, 16, true);

                foreach (var desc in listaMjs)
                {
                    rowIniMsj++;
                    ws.Cells[rowIniMsj, colEmpresa].Value = string.Format("[{0}] [{1}] {2}", desc.Emprnomb, desc.Central, desc.Descripcion);
                }
            }

            if (incluirCaracteristica)
            {
                //No mostrar lineas
                ws.View.ShowGridLines = false;

                //filter
                ws.Cells[rowEmpresa, colEmpresa, rowEmpresa, colCentral].AutoFilter = true;

                ws.View.FreezePanes(rowEmpresa + 1, colUnidad + 1);

                ws.View.ZoomScale = 90;

                //excel con Font Arial
                var allCells = ws.Cells[1, 1, ws.Dimension.End.Row, ws.Dimension.End.Column];
                var cellFont = allCells.Style.Font;
                cellFont.Name = font;
            }
        }

        /// <summary>
        /// Generar detalle por central
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="nameWS"></param>
        /// <param name="rowIniTabla"></param>
        /// <param name="colIniTabla"></param>
        /// <param name="listaRept"></param>
        /// <param name="listaLeyenda"></param>
        /// <param name="equipadre"></param>
        /// <param name="objRpt"></param>
        private void GenerarHojaExcelDetalle(ExcelPackage xlPackage, string nameWS, int rowIniTabla, int colIniTabla, DateTime fecha
                                                            , List<CccReporteDTO> listaRept, List<LeyendaCCC> listaLeyenda, int equipadre, ReporteConsumoCombustible objRpt)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            string colorTab = GetColorTabReporte(listaRept);
            if (!string.IsNullOrEmpty(colorTab)) ws.TabColor = ColorTranslator.FromHtml(colorTab);

            List<ResultadoValidacionAplicativo> listaMsjXCentral = objRpt.ListaMsj.Where(x => x.Equipadre == equipadre).ToList();

            GenerarTablaResultadoExcel(false, ws, rowIniTabla, colIniTabla + 1, fecha, listaRept, listaLeyenda, listaMsjXCentral);

            List<CeldaMWxModo> listaModoUnidadXCentral = objRpt.ListaCeldaDetalle.Where(x => x.Equipadre == equipadre).OrderBy(x => x.Curva.Gruponomb).ToList();

            List<EqEquipoDTO> listaGenXCentral = objRpt.ListaEquiposTermicos.Where(x => x.Equipadre == equipadre && x.Famcodi == ConstantesHorasOperacion.IdGeneradorTemoelectrico).OrderBy(x => x.Equiabrev).ToList();
            List<MeMedicion48DTO> lista48MWxGen = objRpt.Lista48xGen.Where(x => x.Equipadre == equipadre).ToList();

            rowIniTabla += listaRept.Count + (listaMsjXCentral.Count > 0 ? listaMsjXCentral.Count + 3 : 0) + 3;

            string font = "Calibri";
            string colorCeldaGen = "#00B050";
            string colorCeldaCs = "#0070C0";
            string colorCeldaModo = "#F4B084";
            string colorTextoFijo = "#ffffff";

            string colorLinea = "#000000";

            #region  Filtros y Cabecera

            int colHora = colIniTabla;
            int colGen = colHora + 1;
            int colCons = listaGenXCentral.Any() ? colGen + listaGenXCentral.Count : colGen;
            int colHo = listaModoUnidadXCentral.Any() ? colCons + listaModoUnidadXCentral.Count * 2 : colCons;
            int colFin = listaModoUnidadXCentral.Any() ? colHo + listaModoUnidadXCentral.Count - 1 : colHo;

            int rowIniTitulo = rowIniTabla;
            int row2Titulo = rowIniTitulo + 1;
            int rowFinTitulo = row2Titulo + 1;

            ws.Cells[rowIniTitulo, colHora].Value = "HORA";
            UtilExcel.CeldasExcelAgrupar(ws, rowIniTitulo, colHora, rowFinTitulo, colHora);
            UtilExcel.SetFormatoCelda(ws, rowIniTitulo, colHora, rowFinTitulo, colHora, "Centro", "Centro", colorTextoFijo, colorCeldaCs, font, 12, true);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowIniTitulo, colHora, rowFinTitulo, colHora, colorLinea);

            //primera fila
            if (listaGenXCentral.Any())
            {
                int tamanio = listaGenXCentral.Count;

                ws.Cells[rowIniTitulo, colGen].Value = "Potencia Activa";
                UtilExcel.CeldasExcelAgrupar(ws, rowIniTitulo, colGen, rowIniTitulo, colGen + tamanio - 1);
                UtilExcel.SetFormatoCelda(ws, rowIniTitulo, colGen, rowIniTitulo, colGen + tamanio - 1, "Centro", "Centro", colorTextoFijo, colorCeldaGen, font, 12, true, false);
                UtilExcel.BorderCeldasLineaDelgada(ws, rowIniTitulo, colGen, rowIniTitulo, colGen + tamanio - 1, colorLinea);
            }
            if (listaModoUnidadXCentral.Any())
            {
                int tamanio = listaModoUnidadXCentral.Count * 2;

                ws.Cells[rowIniTitulo, colCons].Value = "Consumo Teórico por Combustible";
                UtilExcel.CeldasExcelAgrupar(ws, rowIniTitulo, colCons, rowIniTitulo, colCons + tamanio - 1);
                UtilExcel.SetFormatoCelda(ws, rowIniTitulo, colCons, rowIniTitulo, colCons + tamanio - 1, "Centro", "Centro", colorTextoFijo, colorCeldaCs, font, 12, true, false);
                UtilExcel.BorderCeldasLineaDelgada(ws, rowIniTitulo, colCons, rowIniTitulo, colCons + tamanio - 1, colorLinea);
            }
            if (listaModoUnidadXCentral.Any())
            {
                int tamanio = listaModoUnidadXCentral.Count;
                ws.Cells[rowIniTitulo, colHo].Value = "Horas Operación por Combustible";
                UtilExcel.SetFormatoCelda(ws, rowIniTitulo, colHo, rowIniTitulo, colHo + tamanio - 1, "Centro", "Centro", colorTextoFijo, colorCeldaModo, font, 12, true, false);
                UtilExcel.CeldasExcelAgrupar(ws, rowIniTitulo, colHo, rowIniTitulo, colHo + tamanio - 1);
                UtilExcel.BorderCeldasLineaDelgada(ws, rowIniTitulo, colHo, rowIniTitulo, colHo + tamanio - 1, colorLinea);
            }

            //segunda y tercera fila
            for (int i = 0; i < listaGenXCentral.Count; i++)
            {
                var reg = listaGenXCentral[i];
                ws.Cells[row2Titulo, colGen + i].Value = reg.Equiabrev;
                ws.Cells[rowFinTitulo, colGen + i].Value = "MW";

                UtilExcel.SetFormatoCelda(ws, row2Titulo, colGen + i, rowFinTitulo, colGen + i, "Centro", "Centro", colorTextoFijo, colorCeldaGen, font, 12, true, true);
                UtilExcel.BorderCeldasLineaDelgada(ws, row2Titulo, colGen + i, row2Titulo, colGen + i, colorLinea);
                UtilExcel.BorderCeldasLineaDelgada(ws, rowFinTitulo, colGen + i, rowFinTitulo, colGen + i, colorLinea);

                ws.Column(colGen + i).Width = 10;
            }
            for (int i = 0; i < listaModoUnidadXCentral.Count; i++)
            {
                var regUnidad = listaModoUnidadXCentral[i].UnidadTermico;
                var regCurva = listaModoUnidadXCentral[i].Curva;

                ws.Cells[row2Titulo, colCons + i * 2].Value = regUnidad.UnidadnombCCC;
                UtilExcel.CeldasExcelAgrupar(ws, row2Titulo, colCons + i * 2, row2Titulo, colCons + i * 2 + 1);

                ws.Cells[rowFinTitulo, colCons + i * 2].Value = "MW";
                ws.Cells[rowFinTitulo, colCons + i * 2 + 1].Value = regCurva != null ? regCurva.UnidadMedida : "Consumo";

                UtilExcel.SetFormatoCelda(ws, row2Titulo, colCons + i * 2, rowFinTitulo, colCons + i * 2 + 1, "Centro", "Centro", colorTextoFijo, colorCeldaCs, font, 12, true, true);
                UtilExcel.BorderCeldasLineaDelgada(ws, row2Titulo, colCons + i * 2, row2Titulo, colCons + i * 2, colorLinea);
                UtilExcel.BorderCeldasLineaDelgada(ws, rowFinTitulo, colCons + i * 2 + 1, rowFinTitulo, colCons + i * 2 + 1, colorLinea);

                ws.Column(colCons + i * 2).Width = 10;
                ws.Column(colCons + i * 2 + 1).Width = 12;
            }
            for (int i = 0; i < listaModoUnidadXCentral.Count; i++)
            {
                var regUnidad = listaModoUnidadXCentral[i].UnidadTermico;
                var regDetalle = listaModoUnidadXCentral[i].DetalleHO;

                ws.Cells[row2Titulo, colHo + i].Value = regUnidad.UnidadnombCCC;
                ws.Cells[rowFinTitulo, colHo + i].Value = "MW";

                UtilExcel.SetFormatoCelda(ws, row2Titulo, colHo + i, rowFinTitulo, colHo + i, "Centro", "Centro", colorTextoFijo, colorCeldaModo, font, 12, true, true);
                UtilExcel.BorderCeldasLineaDelgada(ws, row2Titulo, colHo + i, row2Titulo, colHo + i, colorLinea);
                UtilExcel.BorderCeldasLineaDelgada(ws, rowFinTitulo, colHo + i, rowFinTitulo, colHo + i, colorLinea);

                UtilExcel.AgregarComentarioExcel2(ws, row2Titulo, colHo + i, row2Titulo - 5, regDetalle != null ? regDetalle.Comentario : "");

                ws.Column(colHo + i).Width = 10;
            }

            //ws.Row(rowEmpresa).Height = 36;
            ws.Column(1).Width = 3;
            ws.Column(colHora).Width = 7;
            //ws.Row(rowIniTitulo).Height = 35;

            ws.Column(colHora + 5).Width = 13;
            ws.Column(colHora + 6).Width = 13;

            #endregion

            #region Cuerpo

            int numDecimalMw = 1;
            int numDecimalCons = 1;

            int rowData = rowFinTitulo + 1;
            int rowIniData = rowData;

            //mostrar lineas horas
            int numDia = 1;
            int numRowsXCuadro = 8;
            for (int c = colHora; c <= colFin; c++)
            {
                for (int f = rowIniData; f < rowIniData + numDia * 48; f += numRowsXCuadro)
                {
                    ws.Cells[f, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[f, c].Style.Fill.BackgroundColor.SetColor(Color.White);
                    ws.Cells[f, c].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ws.Cells[f, c].Style.Border.Top.Color.SetColor(Color.Blue);

                    ws.Cells[f + numRowsXCuadro - 1, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[f + numRowsXCuadro - 1, c].Style.Fill.BackgroundColor.SetColor(Color.White);
                    ws.Cells[f + numRowsXCuadro - 1, c].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    ws.Cells[f + numRowsXCuadro - 1, c].Style.Border.Bottom.Color.SetColor(Color.Blue);

                    ws.Cells[f, c, f + numRowsXCuadro - 1, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[f, c, f + numRowsXCuadro - 1, c].Style.Fill.BackgroundColor.SetColor(Color.White);
                    ws.Cells[f, c, f + numRowsXCuadro - 1, c].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    ws.Cells[f, c, f + numRowsXCuadro - 1, c].Style.Border.Left.Color.SetColor(Color.Blue);
                    ws.Cells[f, c, f + numRowsXCuadro - 1, c].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    ws.Cells[f, c, f + numRowsXCuadro - 1, c].Style.Border.Right.Color.SetColor(Color.Blue);
                }
            }

            for (int h = 1; h <= 48; h++)
            {
                DateTime fechaHora = DateTime.Today.AddMinutes(h * 30);

                ws.Cells[rowData, colHora].Value = fechaHora.ToString(ConstantesAppServicio.FormatoHora);
                UtilExcel.SetFormatoCelda(ws, rowData, colHora, rowData, colHora, "Centro", "Centro", "#000000", "#FFFFFF", font, 12, true);

                //
                for (int i = 0; i < listaGenXCentral.Count; i++)
                {
                    var regGen = listaGenXCentral[i];
                    var m48Gen = lista48MWxGen.Find(x => x.Equicodi == regGen.Equicodi);

                    decimal? valor = m48Gen != null ? (decimal?)m48Gen.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m48Gen, null) : null;

                    //
                    if (valor.GetValueOrDefault(0) != 0)
                        ws.Cells[rowData, colGen + i].Value = valor;
                    UtilExcel.CeldasExcelFormatoNumero(ws, rowData, colGen + i, rowData, colGen + i, numDecimalMw);
                    UtilExcel.CeldasExcelIndentar(ws, rowData, colGen + i, rowData, colGen + i, 1);
                }

                //Consumo por ciclo simple
                for (int i = 0; i < listaModoUnidadXCentral.Count; i++)
                {
                    var regModo = listaModoUnidadXCentral[i].Modo;
                    var l48MwModo = listaModoUnidadXCentral[i].ListaMW;
                    var l48ConsModo = listaModoUnidadXCentral[i].ListaConsumo;

                    decimal? valorMw = l48MwModo[h - 1];
                    decimal? valorCons = l48ConsModo[h - 1];
                    string mensaje = listaModoUnidadXCentral[i].ListaMensaje[h - 1];

                    //                    
                    if (valorMw.GetValueOrDefault(0) != 0)
                        ws.Cells[rowData, colCons + i * 2].Value = valorMw;

                    if (valorCons.GetValueOrDefault(0) != 0)
                        ws.Cells[rowData, colCons + i * 2 + 1].Value = valorCons;

                    //comentario reserva fria 
                    UtilExcel.AgregarComentarioExcel(ws, rowData, colCons + i * 2, mensaje);

                    UtilExcel.CeldasExcelFormatoNumero(ws, rowData, colCons + i * 2, rowData, colCons + i * 2, numDecimalMw);
                    UtilExcel.CeldasExcelFormatoNumero(ws, rowData, colCons + i * 2 + 1, rowData, colCons + i * 2 + 1, numDecimalCons);
                    UtilExcel.CeldasExcelIndentar(ws, rowData, colCons + i * 2, rowData, colCons + i * 2 + 1, 1);
                }

                //Horas de operación
                for (int i = 0; i < listaModoUnidadXCentral.Count; i++)
                {
                    var regModo = listaModoUnidadXCentral[i].Modo;
                    var l48MwHo = listaModoUnidadXCentral[i].ListaMWHo;
                    var l48Calif = listaModoUnidadXCentral[i].ListaCalifHo;
                    var l48CalifDesc = listaModoUnidadXCentral[i].ListaCalifHoDesc;

                    decimal? valorMwHo = l48MwHo[h - 1];
                    int calif = l48Calif[h - 1];
                    string califDesc = l48CalifDesc[h - 1];

                    //
                    if (valorMwHo.GetValueOrDefault(0) != 0)
                        ws.Cells[rowData, colHo + i].Value = valorMwHo.GetValueOrDefault(0);

                    if (calif == ConstantesSubcausaEvento.SubcausaPorRsf)
                        UtilExcel.SetFormatoCelda(ws, rowData, colHo + i, rowData, colHo + i, "Centro", "Derecha", ConstantesConsumoCombustible.ColorTextoPorRsf, "#FFFFFF", font, 11, false);
                    if (calif == ConstantesSubcausaEvento.SubcausaAMinimaCarga)
                        UtilExcel.SetFormatoCelda(ws, rowData, colHo + i, rowData, colHo + i, "Centro", "Derecha", ConstantesConsumoCombustible.ColorTextoMinimaCarga, "#FFFFFF", font, 11, false);

                    if (!string.IsNullOrEmpty(califDesc))
                    {
                        UtilExcel.SetFormatoCelda(ws, rowData, colHo + i, rowData, colHo + i, "Centro", "Derecha", "#000000", ConstantesConsumoCombustible.ColorAlertaHO, font, 8, false);
                        ws.Cells[rowData, colHo + i].Value = califDesc;
                    }

                    UtilExcel.CeldasExcelFormatoNumero(ws, rowData, colHo + i, rowData, colHo + i, numDecimalMw);
                    UtilExcel.CeldasExcelIndentar(ws, rowData, colHo + i, rowData, colHo + i, 1);
                }

                rowData++;
            }

            #endregion

            #region Gráfico y Curvas

            int rowGraf = rowIniData + 2;

            int colGraf = colFin + 1;
            int colCurva = colGraf + 13;

            for (int i = 0; i < listaModoUnidadXCentral.Count; i++)
            {
                var regUnidad = listaModoUnidadXCentral[i].UnidadTermico;
                var regCurva = listaModoUnidadXCentral[i].Curva;
                var regDetalle = listaModoUnidadXCentral[i].DetalleHO;

                if (regUnidad.Equicodi == 13 && regUnidad.Fenergcodi == 2)
                { }

                bool tieneAlerta = listaModoUnidadXCentral[i].TieneAlertaHo;

                AgregarGraficoExcel(ws, i, "graf_" + i, rowGraf - 1, colGraf, rowIniData, colHora, colCons + i * 2, colHo + i, regUnidad.UnidadnombCCC, tieneAlerta, regDetalle?.Comentario);

                AgregarTablaCurva(ws, regUnidad, regCurva, rowGraf, colCurva, colorLinea, font);

                rowGraf += 18;
            }

            #endregion

            //No mostrar lineas
            ws.View.ShowGridLines = false;

            //filter
            //ws.Cells[rowEmpresa, colEmpresa, rowEmpresa, colCentral].AutoFilter = true;

            if (listaMsjXCentral.Count <= 7)
                ws.View.FreezePanes(rowFinTitulo + 1, colHora + 1);

            ws.View.ZoomScale = 80;

            //excel con Font Arial
            var allCells = ws.Cells[1, 1, ws.Dimension.End.Row, ws.Dimension.End.Column];
            var cellFont = allCells.Style.Font;
            cellFont.Name = font;

        }

        /// <summary>
        /// Agregar grafico en excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="nameGraf"></param>
        /// <param name="rowIniGr"></param>
        /// <param name="colIniGr"></param>
        /// <param name="rowIniData"></param>
        /// <param name="colHora"></param>
        /// <param name="colDesp"></param>
        /// <param name="colHo"></param>
        /// <param name="titulo"></param>
        /// <param name="tieneAlertaHo"></param>
        private void AgregarGraficoExcel(ExcelWorksheet ws, int i, string nameGraf, int rowIniGr, int colIniGr, int rowIniData, int colHora, int colDesp, int colHo, string titulo, bool tieneAlertaHo, string comentarioHo)
        {
            var lineaChart = ws.Drawings.AddChart(nameGraf, eChartType.Line) as ExcelLineChart;
            lineaChart.SetPosition(rowIniGr, 0, colIniGr, 0);
            lineaChart.SetSize(700, 330);

            lineaChart.Title.Text = titulo;
            if (tieneAlertaHo)
            {
                lineaChart.Fill.Color = Color.Yellow;

                lineaChart.Title.Text = titulo + "\n Verificar horas de operación";
                lineaChart.Title.Font.Color = Color.Red;
                lineaChart.Title.Font.Bold = true;
                lineaChart.Title.Font.Size = 24;
            }

            lineaChart.DataLabel.ShowLeaderLines = true;
            lineaChart.YAxis.Title.Text = "MW";
            lineaChart.YAxis.MinValue = 0.0;
            lineaChart.Legend.Position = eLegendPosition.Bottom;
            //lineaChart.XAxis.Orientation = eAxisOrientation.MaxMin;

            var rangoDesp = ws.Cells[rowIniData, colDesp, rowIniData + 47, colDesp];
            var rangoHo = ws.Cells[rowIniData, colHo, rowIniData + 47, colHo];
            var rangoHora = ws.Cells[rowIniData, colHora, rowIniData + 47, colHora];

            var serieDesp = (ExcelChartSerie)lineaChart.Series.Add(rangoDesp, rangoHora);
            serieDesp.Header = "Despacho";

            var serieHo = (ExcelChartSerie)lineaChart.Series.Add(rangoHo, rangoHora);
            serieHo.Header = "Horas de Operación";

            //agregar celda para ver detalle de horas de operacion
            if (!string.IsNullOrEmpty(comentarioHo))
            {
                int rowVerDetalle = rowIniGr + (i <= 2 ? 17 : 18);
                ws.Cells[rowVerDetalle, colIniGr + 1].Value = "Serie Horas de operación: los valores corresponden a la calificación.";
                UtilExcel.AgregarComentarioExcel(ws, rowVerDetalle, colIniGr + 1, comentarioHo);
                //UtilExcel.AgregarComentarioExcel(ws, rowVerDetalle, colIniGr + 2, comentarioHo);
            }
        }

        /// <summary>
        /// Agregar tabla de consumo vs potencia, datos de RSF, potencia firme
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="regUnidad"></param>
        /// <param name="regC"></param>
        /// <param name="rowIniCurva"></param>
        /// <param name="colIniCh"></param>
        /// <param name="colorLinea"></param>
        /// <param name="font"></param>
        private void AgregarTablaCurva(ExcelWorksheet ws, EqEquipoDTO regUnidad, ConsumoHorarioCombustible regC, int rowIniCurva, int colIniCh, string colorLinea, string font)
        {
            //Datos de la curva
            int rowNombCurva = rowIniCurva;

            ws.Cells[rowNombCurva, colIniCh].Value = !regC.EsUnidadEspecial ? "Modo:" : "Unidad:";
            ws.Cells[rowNombCurva, colIniCh + 1].Value = string.Format("[{0}] {1}", regC.Grupocodi, regC.Gruponomb);

            rowNombCurva++;
            ws.Cells[rowNombCurva, colIniCh].Value = "Pe (MW):";
            ws.Cells[rowNombCurva, colIniCh + 1].Value = regC.PotEfectiva;
            ws.Cells[rowNombCurva, colIniCh + 2].Value = " a partir de ";
            ws.Cells[rowNombCurva, colIniCh + 3].Value = regC.FechaDescPotEfectiva;

            rowNombCurva++;
            ws.Cells[rowNombCurva, colIniCh].Value = "Pmin (MW):";
            ws.Cells[rowNombCurva, colIniCh + 1].Value = regC.PotMinima;
            ws.Cells[rowNombCurva, colIniCh + 2].Value = " a partir de ";
            ws.Cells[rowNombCurva, colIniCh + 3].Value = regC.FechaDescPotMinima;
            UtilExcel.SetFormatoCelda(ws, rowNombCurva, colIniCh + 1, rowNombCurva, colIniCh + 1, "Centro", "Derecha", ConstantesConsumoCombustible.ColorTextoMinimaCarga, "#FFFFFF", font, 12, false);

            if (regUnidad.FenergcodiCombAlt > 0 && regUnidad.ConsumoCombAlt > 0) //diesel
            {
                rowNombCurva++;
                ws.Cells[rowNombCurva, colIniCh].Value = string.Format("Comb arr ({0}):", "m3");
                ws.Cells[rowNombCurva, colIniCh + 1].Value = regUnidad.ConsumoCombAlt;
                ws.Cells[rowNombCurva, colIniCh + 2].Value = " a partir de ";
                ws.Cells[rowNombCurva, colIniCh + 3].Value = regUnidad.FechaVigenciaCombAlt.Value.ToString(ConstantesAppServicio.FormatoFecha);
            }

            if (regUnidad.Rsf > 0)
            {
                rowNombCurva++;
                ws.Cells[rowNombCurva, colIniCh].Value = "RSF (MW):";
                ws.Cells[rowNombCurva, colIniCh + 1].Value = regUnidad.Rsf;
                ws.Cells[rowNombCurva, colIniCh + 2].Value = " a partir de ";
                ws.Cells[rowNombCurva, colIniCh + 3].Value = regUnidad.FechaVigenciaRsf.Value.ToString(ConstantesAppServicio.FormatoFecha);
                UtilExcel.SetFormatoCelda(ws, rowNombCurva, colIniCh + 1, rowNombCurva, colIniCh + 1, "Centro", "Derecha", ConstantesConsumoCombustible.ColorTextoPorRsf, "#FFFFFF", font, 12, false);
            }

            if (regUnidad.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiBagazo)
            {
                rowNombCurva++;
                ws.Cells[rowNombCurva, colIniCh].Value = "Rend (MWh/Tn):";
                ws.Cells[rowNombCurva, colIniCh + 1].Value = regUnidad.RendBagazo;
                //ws.Cells[rowNombCurva, colIniCh + 2].Value = " a partir de ";
                //ws.Cells[rowNombCurva, colIniCh + 3].Value = regUnidad.fecha;
            }

            rowNombCurva++;

            rowNombCurva++;
            ws.Cells[rowNombCurva, colIniCh].Value = "y = m*x + b";
            ws.Cells[rowNombCurva, colIniCh + 1].Value = "Pendiente (m): ";
            ws.Cells[rowNombCurva, colIniCh + 2].Value = regC.PendienteM01;
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowNombCurva, colIniCh + 1, rowNombCurva, colIniCh + 1, "Derecha");
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowNombCurva, colIniCh + 2, rowNombCurva, colIniCh + 2, "Derecha");

            rowNombCurva++;
            ws.Cells[rowNombCurva, colIniCh + 1].Value = "Coeficiente independiente(b): ";
            ws.Cells[rowNombCurva, colIniCh + 2].Value = regC.CoeficienteIndependiente;
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowNombCurva, colIniCh + 1, rowNombCurva, colIniCh + 1, "Derecha");
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowNombCurva, colIniCh + 2, rowNombCurva, colIniCh + 2, "Derecha");

            rowNombCurva++;
            ws.Cells[rowNombCurva, colIniCh + 1].Value = "Coeficiente correlación: ";
            ws.Cells[rowNombCurva, colIniCh + 2].Value = regC.CoeficienteCorrelacion;
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowNombCurva, colIniCh + 1, rowNombCurva, colIniCh + 1, "Derecha");
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowNombCurva, colIniCh + 2, rowNombCurva, colIniCh + 2, "Derecha");

            rowNombCurva++;
            rowNombCurva++;
            int rowIniTablaC = rowNombCurva;
            ws.Cells[rowNombCurva, colIniCh].Value = "MW";
            ws.Cells[rowNombCurva, colIniCh + 1].Value = "A partir de";
            ws.Cells[rowNombCurva, colIniCh + 2].Value = regC.UnidadMedida + " / h";
            ws.Cells[rowNombCurva, colIniCh + 3].Value = "A partir de";

            ws.Column(colIniCh).Width = 16;
            ws.Column(colIniCh + 1).Width = 16;
            ws.Column(colIniCh + 2).Width = 13;
            ws.Column(colIniCh + 3).Width = 13;

            rowNombCurva++;
            ws.Cells[rowNombCurva, colIniCh].Value = regC.PotEfectiva;
            ws.Cells[rowNombCurva, colIniCh + 1].Value = regC.FechaDescPotEfectiva;
            ws.Cells[rowNombCurva, colIniCh + 2].Value = regC.ConsumoPotEfectiva;
            ws.Cells[rowNombCurva, colIniCh + 3].Value = regC.FechaDescConsumoPotEfectiva;

            rowNombCurva++;
            ws.Cells[rowNombCurva, colIniCh].Value = regC.PotParcial1;
            ws.Cells[rowNombCurva, colIniCh + 1].Value = regC.FechaDescPotParcial1;
            ws.Cells[rowNombCurva, colIniCh + 2].Value = regC.ConsumoPotParcial1;
            ws.Cells[rowNombCurva, colIniCh + 3].Value = regC.FechaDescConsumoPotParcial1;

            rowNombCurva++;
            ws.Cells[rowNombCurva, colIniCh].Value = regC.PotParcial2;
            ws.Cells[rowNombCurva, colIniCh + 1].Value = regC.FechaDescPotParcial2;
            ws.Cells[rowNombCurva, colIniCh + 2].Value = regC.ConsumoPotParcial2;
            ws.Cells[rowNombCurva, colIniCh + 3].Value = regC.FechaDescConsumoPotParcial2;

            rowNombCurva++;
            ws.Cells[rowNombCurva, colIniCh].Value = regC.PotParcial3;
            ws.Cells[rowNombCurva, colIniCh + 1].Value = regC.FechaDescPotParcial3;
            ws.Cells[rowNombCurva, colIniCh + 2].Value = regC.ConsumoPotParcial3;
            ws.Cells[rowNombCurva, colIniCh + 3].Value = regC.FechaDescConsumoPotParcial3;

            rowNombCurva++;
            ws.Cells[rowNombCurva, colIniCh].Value = regC.PotParcial4;
            ws.Cells[rowNombCurva, colIniCh + 1].Value = regC.FechaDescPotParcial4;
            ws.Cells[rowNombCurva, colIniCh + 2].Value = regC.ConsumoPotParcial4;
            ws.Cells[rowNombCurva, colIniCh + 3].Value = regC.FechaDescConsumoPotParcial4;

            UtilExcel.CeldasExcelFormatoNumero(ws, rowIniTablaC + 1, colIniCh + 1, rowNombCurva, colIniCh + 1, 2);
            UtilExcel.CeldasExcelFormatoNumero(ws, rowIniTablaC + 1, colIniCh + 3, rowNombCurva, colIniCh + 3, 2);

            //
            UtilExcel.SetFormatoCelda(ws, rowIniTablaC, colIniCh, rowIniTablaC, colIniCh + 1, "Centro", "Centro", "#000000", "#FFFFFF", font, 12, true);
            UtilExcel.SetFormatoCelda(ws, rowIniTablaC, colIniCh + 2, rowIniTablaC, colIniCh + 3, "Centro", "Centro", "#000000", "#FFFFFF", font, 12, true);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowIniTablaC, colIniCh, rowIniTablaC, colIniCh, colorLinea);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowIniTablaC, colIniCh + 1, rowIniTablaC, colIniCh + 1, colorLinea);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowIniTablaC, colIniCh + 2, rowIniTablaC, colIniCh + 2, colorLinea);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowIniTablaC, colIniCh + 3, rowIniTablaC, colIniCh + 3, colorLinea);

            //
            UtilExcel.SetFormatoCelda(ws, rowIniTablaC + 1, colIniCh, rowNombCurva, colIniCh, "Centro", "Centro", "#000000", "#FFFFFF", font, 12, false);
            UtilExcel.SetFormatoCelda(ws, rowIniTablaC + 1, colIniCh + 1, rowNombCurva, colIniCh + 1, "Centro", "Centro", "#000000", "#FFFFFF", font, 12, false);
            UtilExcel.SetFormatoCelda(ws, rowIniTablaC + 1, colIniCh + 2, rowNombCurva, colIniCh + 2, "Centro", "Centro", "#000000", "#FFFFFF", font, 12, false);
            UtilExcel.SetFormatoCelda(ws, rowIniTablaC + 1, colIniCh + 3, rowNombCurva, colIniCh + 3, "Centro", "Centro", "#000000", "#FFFFFF", font, 12, false);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowIniTablaC + 1, colIniCh, rowNombCurva, colIniCh, colorLinea);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowIniTablaC + 1, colIniCh + 1, rowNombCurva, colIniCh + 1, colorLinea);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowIniTablaC + 1, colIniCh + 2, rowNombCurva, colIniCh + 2, colorLinea);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowIniTablaC + 1, colIniCh + 3, rowNombCurva, colIniCh + 3, colorLinea);
        }

        /// <summary>
        /// Color para alerta de tab
        /// </summary>
        /// <param name="listaRptXCentral"></param>
        /// <returns></returns>
        private string GetColorTabReporte(List<CccReporteDTO> listaRptXCentral)
        {
            string colorTab = string.Empty;

            bool tieneAlertaFaltaData = listaRptXCentral.Find(x => x.TieneAlertaFaltaDataCoes) != null;
            bool tieneAlertaHo = listaRptXCentral.Find(x => x.TieneAlertaHo) != null;
            bool tieneFaltaConsumo = listaRptXCentral.Find(x => x.PintarCeldaFaltaValorReal || x.PintarCeldaFaltaValorTeorico) != null;
            bool tieneTransgresion = listaRptXCentral.Find(x => x.TieneTransgresion) != null;

            if (tieneAlertaFaltaData) colorTab = ConstantesConsumoCombustible.ColorFaltaValorDatosCOES;
            if (tieneAlertaHo) colorTab = ConstantesConsumoCombustible.ColorAlertaHO;
            if (tieneFaltaConsumo) colorTab = ConstantesConsumoCombustible.ColorFaltaValorConsumo;
            if (tieneTransgresion) colorTab = ConstantesConsumoCombustible.ColorTieneTransgresion;

            return colorTab;
        }

        #endregion

        #endregion

        #region Envio notificación de Transgresión 

        /// <summary>
        /// obtener las unidades que tienes transgresión
        /// </summary>
        /// <param name="fechaPeriodo"></param>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        public List<CccReporteDTO> ListarTransgresionXEmpresa(DateTime fechaPeriodo, int emprcodi)
        {
            List<LeyendaCCC> listaLeyenda = ListarLeyenda(fechaPeriodo);
            List<CccReporteDTO> listaCalculo = ListarReporteCCC(fechaPeriodo, emprcodi.ToString(), "-1", out ReporteConsumoCombustible objDatosReporte);

            foreach (var reg in listaCalculo)
            {
                FormatearCCCReporte(reg, listaLeyenda);
            }

            return listaCalculo.Where(x => x.Fenergcodi != ConstantesPR5ReportesServicio.FenergcodiBagazo && x.TieneTransgresion).ToList();
        }

        #endregion

        #region Tipo de combustible

        /// <summary>
        /// Listado Html de tipo de combustible
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GenerarTablaHtmlListadoTipoCombustible(string url)
        {
            List<SiFuenteenergiaDTO> lista = GetByCriteriaSiFuenteenergias().OrderBy(x => x.Osinergcodi).ToList();

            StringBuilder str = new StringBuilder();
            str.Append("<table class='pretty tabla-adicional' border='0' cellspacing='0' width='100%' id='tabla_combustible'>");

            str.Append("<thead>");
            #region cabecera
            str.Append("<tr>");
            str.Append("<th style='width: 30px'>Opciones</th>");
            str.Append("<th style=''>Código</th>");
            str.Append("<th style=''>Combustible</th>");
            str.Append("<th style=''></th>");
            str.Append("<th style=''>Código Osinergmin</th>");
            str.Append("<th style=''>Unidad COES</th>");
            str.Append("<th style=''>Unidad Osinergmin</th>");
            str.Append("</tr>");
            #endregion
            str.Append("</thead>");

            str.Append("<tbody>");
            #region cuerpo

            foreach (var reg in lista)
            {
                str.Append("<tr>");

                str.Append("<td>");
                str.AppendFormat("<a class='' href='JavaScript:editarCombustible({0});' style='margin-right: 4px;'><img style='margin-top: 4px; margin-bottom: 4px;' src='{1}Content/Images/btn-edit.png' title='Editar registro' /></a>", reg.Fenergcodi, url);
                str.Append("</td>");

                str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.Fenergcodi);
                str.AppendFormat("<td class='' style='text-align: center;'>{0}</td>", reg.Fenergnomb);
                str.AppendFormat("<td class='' style='text-align: center;background-color: {0};'></td>", reg.Fenergcolor);
                str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.Osinergcodi);
                str.AppendFormat("<td class='' style='text-align: center'>{1} ( {0} )</td>", reg.Tinfcoesabrev, reg.Tinfcoesdesc);
                str.AppendFormat("<td class='' style='text-align: center'>{1} ( {0} )</td>", reg.Tinfosiabrev, reg.Tinfosidesc);

                str.Append("</tr>");
            }

            #endregion
            str.Append("</tbody>");

            str.Append("</table>");

            return str.ToString();
        }

        /// <summary>
        /// Actualizar tipo de combustible
        /// </summary>
        /// <param name="fenergcodi"></param>
        /// <param name="osinergcodi"></param>
        /// <param name="tinfcoes"></param>
        /// <param name="tinfosi"></param>
        public void ActualizarSiFuenteEnergia(int fenergcodi, string osinergcodi, int tinfcoes, int tinfosi)
        {
            SiFuenteenergiaDTO reg = GetByIdSiFuenteenergia(fenergcodi);

            reg.Osinergcodi = osinergcodi;
            reg.Tinfcoes = tinfcoes;
            reg.Tinfosi = tinfosi;

            UpdateSiFuenteenergia(reg);
        }

        #endregion

        #region Factor de Conversión

        /// <summary>
        /// lista de factor
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GenerarTablaHtmlListadoFactorConversion(string url)
        {
            List<SiFactorconversionDTO> lista = GetByCriteriaSiFactorconversions();

            StringBuilder str = new StringBuilder();
            str.Append("<table class='pretty tabla-adicional' border='0' cellspacing='0' width='100%' id='tabla_factor'>");

            str.Append("<thead>");
            #region cabecera
            str.Append("<tr>");
            str.Append("<th style='width: 30px'>Opciones</th>");
            str.Append("<th style=''>Unidad Origen</th>");
            str.Append("<th style=''>Factor de conversión</th>");
            str.Append("<th style=''>Unidad Destino</th>");
            str.Append("</tr>");
            #endregion
            str.Append("</thead>");

            str.Append("<tbody>");
            #region cuerpo

            foreach (var reg in lista)
            {
                str.Append("<tr>");

                str.Append("<td>");
                str.AppendFormat("<a class='' href='JavaScript:editarFactor({0});' style='margin-right: 4px;'><img style='margin-top: 4px; margin-bottom: 4px;' src='{1}Content/Images/btn-edit.png' title='Editar registro' /></a>", reg.Tconvcodi, url);
                str.Append("</td>");

                str.AppendFormat("<td class='' style='text-align: center'>{1} ( {0} )</td>", reg.Tinforigenabrev, reg.Tinforigendesc);
                str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.Tconvfactor);
                str.AppendFormat("<td class='' style='text-align: center'>{1} ( {0} )</td>", reg.Tinfdestinoabrev, reg.Tinfdestinodesc);

                str.Append("</tr>");
            }

            #endregion
            str.Append("</tbody>");

            str.Append("</table>");

            return str.ToString();
        }

        /// <summary>
        /// actualizar factor
        /// </summary>
        /// <param name="tconvcodi"></param>
        /// <param name="tconvfactor"></param>
        public void ActualizarFactorConversion(int tconvcodi, decimal tconvfactor)
        {
            SiFactorconversionDTO reg = GetByIdSiFactorconversion(tconvcodi);

            reg.Tconvfactor = tconvfactor;

            UpdateSiFactorconversion(reg);
        }

        /// <summary>
        /// guardar factor
        /// </summary>
        /// <param name="reg"></param>
        public void GuardarFactorConversion(SiFactorconversionDTO reg)
        {
            var lista = ListSiFactorconversions();

            if (lista.Find(x => x.Tinfdestino == reg.Tinfdestino && x.Tinforigen == reg.Tinforigen) != null)
            {
                throw new ArgumentException("Ya existe el factor de conversión.");
            }

            SaveSiFactorconversion(reg);
        }

        #endregion

        #region ALGORITMO VCOM: Cálculo de Reporte Mensual VCOM

        /// <summary>
        /// Funcion que obtiene los datos a guardar y también para validar en extranet
        /// </summary>
        /// <param name="fechaPeriodo"></param>
        /// <param name="emprcodi">por defecto -1: todas las empresas</param>
        /// <returns></returns>
        public List<CccVcomDTO> ListarDataCalculoReporteVCOM(int verscodi, DateTime fechaPeriodo, string emprcodis, string equipadres, out ReporteVCOM reporteVcom)
        {
            //mensajes
            List<ResultadoValidacionAplicativo> listaMsj = new List<ResultadoValidacionAplicativo>();

            #region Insumos

            DateTime fechaIni = fechaPeriodo.Date;
            DateTime fechaFin = fechaPeriodo.Date.AddMonths(1).AddDays(-1);

            //Detalle Diario
            List<CccVersionDTO> listaVersionDiaria = new List<CccVersionDTO>();

            ListarDetalleDiarioCCC(fechaIni, fechaFin, verscodi
                                        , out listaVersionDiaria, out List<CccReporteDTO> listaRepdet, out List<ResultadoValidacionAplicativo> listaMjs);
            listaMsj.AddRange(listaMjs);

            //fuente de energia
            List<SiFuenteenergiaDTO> listaFenerg = GetByCriteriaSiFuenteenergias();

            //Factor de conversion
            List<SiFactorconversionDTO> listaFactor = GetByCriteriaSiFactorconversions();

            //obtener los modos de operacion del mes
            indServ.ListarUnidadTermicoCCC(fechaIni, fechaFin, emprcodis, equipadres, out List<EqEquipoDTO> listaUnidad
                                            , out List<EqEquipoDTO> listaEquiposTermicos
                                            , out List<PrGrupoDTO> listaGrupoModo, out List<PrGrupoDTO> listaAllGrupo
                                            , out List<ResultadoValidacionAplicativo> listaMsj3);

            List<PrGrupodatDTO> listaHist = INDAppServicio.ListarPrGrupodatHistoricoVigente(ConstantesMigraciones.ConcepcodisCodigoOsinergmin, null, false);

            #endregion

            #region Calculo

            List<CccVcomDTO> listaUnidadVCOM = new List<CccVcomDTO>();

            /////////////////////////////////////////////////////////////////////////////
            ///Totalizar por generador
            List<CccReporteDTO> listaTot = listaRepdet.GroupBy(x => new { x.Equicodi, x.Fenergcodi, x.Tipoinfocodi })
                                            .Select(x => new CccReporteDTO()
                                            {
                                                Equicodi = x.Key.Equicodi,
                                                Equinomb = x.First().Equinomb,
                                                Fenergcodi = x.Key.Fenergcodi,
                                                Fenergnomb = x.First().Fenergnomb,
                                                Tipoinfocodi = x.Key.Tipoinfocodi,
                                                Tipoinfoabrev = x.First().Tipoinfoabrev,
                                                Emprcodi = x.Last().Emprcodi,
                                                Emprnomb = x.Last().Emprnomb,
                                                Equipadre = x.First().Equipadre,
                                                Central = x.First().Central,
                                                Cccrptvalorreal = x.Sum(y => y.Cccrptvalorreal ?? 0)
                                            })
                                            .OrderBy(x => x.Emprnomb).ThenBy(x => x.Central).ToList();

            //obtener detalle por equipo y modo de operación
            List<CccReporteDTO> listaTotXEqXModo = listaRepdet.GroupBy(x => new { x.Equicodi, x.Mogrupocodi, x.Fenergcodi, x.Tipoinfocodi })
                                            .Select(x => new CccReporteDTO()
                                            {
                                                Equicodi = x.Key.Equicodi,
                                                Grupocodi = x.Key.Mogrupocodi,
                                                Equinomb = x.First().Equinomb,
                                                Fenergcodi = x.Key.Fenergcodi,
                                                Fenergnomb = x.First().Fenergnomb,
                                                Tipoinfocodi = x.Key.Tipoinfocodi,
                                                Tipoinfoabrev = x.First().Tipoinfoabrev,
                                                Emprcodi = x.Last().Emprcodi,
                                                Emprnomb = x.Last().Emprnomb,
                                                Equipadre = x.First().Equipadre,
                                                Central = x.First().Central,
                                                Cccrptvalorreal = x.Sum(y => y.Cccrptvalorreal ?? 0)
                                            })
                                            .OrderBy(x => x.Emprnomb).ThenBy(x => x.Central).ToList();

            //centrales
            List<EqEquipoDTO> listaCentralXFenerg = listaTot.Where(x => x.Cccrptvalorreal > 0)
                                                            .GroupBy(x => new { x.Equipadre, x.Fenergcodi })
                                                                .Select(x => new EqEquipoDTO()
                                                                {
                                                                    Equipadre = x.Key.Equipadre,
                                                                    Central = x.First().Central,
                                                                    Fenergcodi = x.Key.Fenergcodi,
                                                                    Fenergnomb = x.First().Fenergnomb
                                                                }).ToList();
            foreach (var central in listaCentralXFenerg)
            {
                if (central.Equipadre == 13601)
                { }

                List<PrGrupoDTO> listaModoXCentral = listaGrupoModo.Where(x => x.Equipadre == central.Equipadre && x.Fenergcodi == central.Fenergcodi).ToList();
                List<EqEquipoDTO> listaUnidadXCentralOpcomercial = listaUnidad.Where(x => x.Equipadre == central.Equipadre && x.Fenergcodi == central.Fenergcodi).OrderByDescending(x => x.Pe).ToList();
                List<CccReporteDTO> listaEqDet = listaTotXEqXModo.Where(x => x.Equipadre == central.Equipadre && x.Fenergcodi == central.Fenergcodi).ToList();

                List<int> listaEquicodi = listaEqDet.Select(x => x.Equicodi).Distinct().ToList();
                //caso especial CT Ventanilla gas
                if (central.Equipadre == 290 && central.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiGas)
                {
                    var listaEqVent = listaEquiposTermicos.Where(x => x.Equipadre == central.Equipadre && x.Famcodi == ConstantesHorasOperacion.IdGeneradorTemoelectrico).Select(x => x.Equicodi).ToList();
                    var lId = listaUnidadXCentralOpcomercial.Where(x => x.EquicodiTVCicloComb > 0).Select(x => x.EquicodiTVCicloComb).Distinct().ToList();
                    if (lId.Any())
                        listaEqVent = listaEqVent.Where(x => !lId.Contains(x)).ToList();
                    listaEquicodi = listaEqVent;
                }
                List<int> listaMogrupocodiXCentral = listaRepdet.Where(x => x.Equipadre == central.Equipadre && x.Fenergcodi == central.Fenergcodi && x.Mogrupocodi > 0)
                                                                .Select(x => x.Mogrupocodi.Value).Distinct().ToList();

                List<PrGrupoDTO> listaModoHop = listaModoXCentral.Where(x => listaMogrupocodiXCentral.Contains(x.Grupocodi))
                                                                .OrderByDescending(x => x.Potencia).ToList();

                //agregar unidades VCOM
                List<CccVcomDTO> listaVCOMxCentral = new List<CccVcomDTO>();

                //actualizar modos de operacion sobrantes
                ListarModosVCOMXHo(central.Equipadre.Value, central.Fenergcodi, ref listaVCOMxCentral, ref listaEquicodi, listaModoHop, ref listaEqDet);

                if (listaEquicodi.Any() || listaEqDet.Any())
                {
                    //si no existe hora de operación en el mes, escoger el primer modo de operación con mayor potencia efectiva
                    ListarModosVCOMDefault(central.Equipadre.Value, central.Fenergcodi, ref listaVCOMxCentral, ref listaEquicodi, listaModoXCentral, listaUnidadXCentralOpcomercial, ref listaEqDet);
                }

                //preparan mensaje
                listaEqDet = listaEqDet.Where(x => x.Cccrptvalorreal > 0).ToList();
                if (listaEqDet.Any())
                    listaMsj.Add(new ResultadoValidacionAplicativo()
                    {
                        Descripcion = string.Format("[{0}][{1}] Las unidades {2} tienen consumo de combustible pero no tiene relacionado modos de operación."
                                                                        , central.Fenergnomb, central.Central, string.Join(",", listaEqDet.Select(x => x.Equinomb)))
                    });

                listaUnidadVCOM.AddRange(listaVCOMxCentral);
            }


            /////////////////////////////////////////////////////////////////////////////
            //Procesar por unidad
            foreach (var regUnidad in listaUnidadVCOM)
            {
                if (regUnidad.Equipadre == 13601)
                { }

                SiFuenteenergiaDTO regFe = listaFenerg.Find(x => x.Fenergcodi == regUnidad.Fenergcodi);

                SiFactorconversionDTO regFactor = listaFactor.Find(x => x.Tinforigen == regUnidad.Tinfcoes && x.Tinfdestino == regFe.Tinfosi);
                regUnidad.Tinfosi = regFe.Tinfosi;

                decimal factor = 1;
                if (regFactor != null) factor = regFactor.Tconvfactor;

                decimal valorConsumoMedidaOsi = regUnidad.ValorMedidaCoes.GetValueOrDefault(0) * factor;

                regUnidad.Vcomvalor = valorConsumoMedidaOsi;

                //codigos osinergmin
                regUnidad.Vcomcodigotcomb = regFe.Osinergcodi;

                INDAppServicio.GetValorStringFromListaGrupoDat(fechaFin, regUnidad.Grupocodi, listaHist, out string osignergcodiModo, out DateTime? fechaVigencia);
                regUnidad.Vcomcodigomop = osignergcodiModo;
            }

            //ajustes de redondeo y presentación
            foreach (var reg in listaUnidadVCOM)
            {
                reg.Vcomvalor = Math.Round(reg.Vcomvalor.GetValueOrDefault(0), 4);
            }

            #endregion

            reporteVcom = new ReporteVCOM();
            reporteVcom.ListaUnidad = listaTot;
            reporteVcom.ListaDetalleDiario = listaRepdet;
            reporteVcom.ListaMsj = listaMsj;
            reporteVcom.FechaIni = fechaIni;
            reporteVcom.FechaFin = fechaFin;
            reporteVcom.ListaVersionDiaria = listaVersionDiaria;

            return listaUnidadVCOM;
        }

        /// <summary>
        /// INSUMO: Reporte de CCC diario
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="verscodi"></param>
        /// <param name="listaVersionValido"></param>
        /// <param name="listaRepdet"></param>
        /// <param name="listaMjs"></param>
        private void ListarDetalleDiarioCCC(DateTime fechaIni, DateTime fechaFin, int verscodi
                                                            , out List<CccVersionDTO> listaVersionValido, out List<CccReporteDTO> listaRepdet, out List<ResultadoValidacionAplicativo> listaMjs)
        {
            listaMjs = new List<ResultadoValidacionAplicativo>();
            listaVersionValido = new List<CccVersionDTO>();

            List<int> listarRptcodiInput = new List<int>();
            if (verscodi > 0)
            {
                CccVersionDTO regVersionBD = GetByIdCccVersion(verscodi);
                listarRptcodiInput = !string.IsNullOrEmpty(regVersionBD.Cccverrptcodis) ? regVersionBD.Cccverrptcodis.Split(',').Select(x => Convert.ToInt32(x)).ToList() : new List<int>();
            }

            //bd
            List<CccVersionDTO> listaVersionXMes = GetByCriteriaCccVersions(fechaIni, fechaFin, ConstantesConsumoCombustible.HorizonteDiario);
            if (verscodi > 0)
                listaVersionXMes = listaVersionXMes.Where(x => listarRptcodiInput.Contains(x.Cccvercodi)).ToList();

            for (var day = fechaIni; day <= fechaFin; day = day.AddDays(1))
            {
                var regVersionXDia = listaVersionXMes.Where(x => x.Cccverfecha == day).OrderByDescending(x => x.Cccvernumero).FirstOrDefault();
                if (regVersionXDia != null)
                    listaVersionValido.Add(regVersionXDia);
                else
                {
                    listaMjs.Add(new ResultadoValidacionAplicativo() { Descripcion = string.Format("No existe versión de Cálculo Diario {0}.", day.ToString(ConstantesAppServicio.FormatoFecha)) });
                }
            }

            //Obtener detalle diario
            listaRepdet = new List<CccReporteDTO>();
            if (listaVersionValido.Any())
            {
                string strListarVerscodi = string.Join(",", listaVersionValido.Select(x => x.Cccvercodi));
                listaRepdet = GetByCriteriaCccReportes(strListarVerscodi).OrderBy(x => x.Cccverfecha).ToList();
                listaRepdet = listaRepdet.Where(x => x.Cccrptflagtienecurva != ConstantesConsumoCombustible.FlagCombustibleArranque).ToList();
            }

        }

        /// <summary>
        /// Obtener modos de operación que tienen horas de operación
        /// </summary>
        /// <param name="equipadre"></param>
        /// <param name="fenergcodi"></param>
        /// <param name="listaEquicodiAll"></param>
        /// <param name="listaModoxHo"></param>
        /// <param name="listaEqDet"></param>
        /// <returns></returns>
        private List<CccVcomDTO> ListarModosVCOMXHo(int equipadre, int fenergcodi, ref List<CccVcomDTO> listaVCOMxCentral, ref List<int> listaEquicodiAll, List<PrGrupoDTO> listaModoxHo, ref List<CccReporteDTO> listaEqDet)
        {
            List<CccVcomDTO> l = new List<CccVcomDTO>();

            listaModoxHo = listaModoxHo.OrderByDescending(x => x.Potencia).ToList();

            var listaEquicodi = listaEquicodiAll;

            foreach (var regModo in listaModoxHo)
            {
                if (listaEquicodi.Any())
                {
                    AgregarUnidadVCOM(equipadre, fenergcodi, regModo, true, ref listaVCOMxCentral, ref listaEquicodi, ref listaEqDet);

                    //if (regModo.TieneModoCicloCombinado || regModo.TieneModoEspecial)
                    //{
                    //    if (regModo.ListaEquicodi.Count() > 1 && listaEquicodi.All(x => regModo.ListaEquicodi.Contains(x)))
                    //    {
                    //        AgregarUnidadVCOM(equipadre, fenergcodi, regModo, true, ref listaVCOMxCentral, ref listaEquicodi, ref listaEqDet);
                    //    }
                    //}

                    //if (regModo.ListaEquicodi.Count() == 1 && regModo.TieneModoCicloSimple)
                    //{
                    //    if (listaEquicodi.Any(x => regModo.ListaEquicodi.Contains(x)))
                    //    {
                    //        AgregarUnidadVCOM(equipadre, fenergcodi, regModo, true, ref listaVCOMxCentral, ref listaEquicodi, ref listaEqDet);
                    //    }
                    //}
                }
            }

            listaEquicodiAll = listaEquicodi;

            return l;
        }

        /// <summary>
        /// Obtener modos de operación que NO tienen horas de operación
        /// </summary>
        /// <param name="equipadre"></param>
        /// <param name="fenergcodi"></param>
        /// <param name="listaEquicodiAll"></param>
        /// <param name="listaModoxFenerg"></param>
        /// <param name="listaUnidad"></param>
        /// <param name="listaEqDet"></param>
        /// <returns></returns>
        private List<CccVcomDTO> ListarModosVCOMDefault(int equipadre, int fenergcodi, ref List<CccVcomDTO> listaVCOMxCentral, ref List<int> listaEquicodiAll, List<PrGrupoDTO> listaModoxFenerg, List<EqEquipoDTO> listaUnidad, ref List<CccReporteDTO> listaEqDet)
        {
            List<CccVcomDTO> l = new List<CccVcomDTO>();

            listaModoxFenerg = listaModoxFenerg.OrderByDescending(x => x.Potencia).ToList();

            var listaEquicodi = listaEquicodiAll;

            foreach (var reg in listaUnidad)
            {
                if (listaEquicodi.Any())
                {
                    var regModo = listaModoxFenerg.Find(x => x.Grupocodi == reg.Grupocodi);

                    AgregarUnidadVCOM(equipadre, fenergcodi, regModo, false, ref listaVCOMxCentral, ref listaEquicodi, ref listaEqDet);

                    //if (regModo.TieneModoCicloCombinado || regModo.TieneModoEspecial)
                    //{
                    //    if (regModo.ListaEquicodi.Count() > 1 && listaEquicodi.All(x => regModo.ListaEquicodi.Contains(x)))
                    //    {
                    //        AgregarUnidadVCOM(equipadre, fenergcodi, regModo, false, ref listaVCOMxCentral, ref listaEquicodi, ref listaEqDet);
                    //    }
                    //}

                    //if (regModo.ListaEquicodi.Count() == 1 && regModo.TieneModoCicloSimple)
                    //{
                    //    if (listaEquicodi.Any(x => regModo.ListaEquicodi.Contains(x)))
                    //    {
                    //        AgregarUnidadVCOM(equipadre, fenergcodi, regModo, false, ref listaVCOMxCentral, ref listaEquicodi, ref listaEqDet);
                    //    }
                    //}
                }
            }

            listaEquicodiAll = listaEquicodi;

            return l;
        }

        /// <summary>
        /// crear objeto
        /// </summary>
        /// <param name="equipadre"></param>
        /// <param name="fenergcodi"></param>
        /// <param name="regModo"></param>
        /// <param name="filtrarDetXModo"></param>
        /// <param name="listaEquicodi"></param>
        /// <param name="listaEqDet"></param>
        /// <param name="regVcom"></param>
        private void AgregarUnidadVCOM(int equipadre, int fenergcodi, PrGrupoDTO regModo, bool filtrarDetXModo, ref List<CccVcomDTO> listaVCOMxCentral, ref List<int> listaEquicodi, ref List<CccReporteDTO> listaEqDet)
        {
            List<CccReporteDTO> listaDetxModo = new List<CccReporteDTO>();

            //obtener el detalle diario de las unidades del modo
            if (equipadre == 290 && fenergcodi == ConstantesPR5ReportesServicio.FenergcodiGas)
            {
                listaDetxModo = listaEqDet.ToList();
                filtrarDetXModo = false;
            }
            else
            {
                listaDetxModo = listaEqDet.Where(x => regModo.ListaEquicodi.Contains(x.Equicodi)).ToList();
            }

            //detalle diario con las unidades con el modo representativo del dia
            if (filtrarDetXModo)
            {
                listaDetxModo = listaDetxModo.Where(x => x.Grupocodi == regModo.Grupocodi).ToList();
            }

            //obtener el total por modo o por central (Ventanilla)
            decimal valorModo = listaDetxModo.Sum(x => x.Cccrptvalorreal ?? 0);

            if (listaDetxModo.Any() && valorModo > 0)
            {
                CccVcomDTO regVcom = listaVCOMxCentral.Find(x => x.Grupocodi == regModo.Grupocodi);
                if (regVcom == null)
                {
                    regVcom = new CccVcomDTO();
                    regVcom.Emprcodi = regModo.Emprcodi.Value;
                    regVcom.Equipadre = regModo.Equipadre;
                    regVcom.Grupocodi = regModo.Grupocodi;
                    regVcom.Tinfcoes = listaEqDet.First().Tipoinfocodi;
                    regVcom.Fenergcodi = regModo.Fenergcodi.Value;

                    listaVCOMxCentral.Add(regVcom);
                }

                regVcom.ValorMedidaCoes = regVcom.ValorMedidaCoes.GetValueOrDefault(0) + valorModo;

                //quitar uno o más equipos
                if (!filtrarDetXModo)
                    listaEquicodi = listaEquicodi.Where(x => !regModo.ListaEquicodi.Contains(x)).ToList();

                //quitar de la lista el consumo sumado
                foreach (var regDet in listaDetxModo)
                    listaEqDet.Remove(regDet);
            }
        }

        #endregion

        #region ALGORITMO VCOM: Reporte VCOM (Web y Excel)

        /// <summary>
        /// Genera tabla Html listado versión por fecha
        /// </summary>
        /// <param name="fechaPeriodo"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GenerarTablaHtmlListadoVersionVCOM(DateTime fechaPeriodo, string url, bool tienePermiso)
        {
            List<CccVersionDTO> listaVersion = GetByCriteriaCccVersions(fechaPeriodo, fechaPeriodo, ConstantesConsumoCombustible.HorizonteMensual);

            StringBuilder str = new StringBuilder();
            str.Append("<table class='pretty tabla-adicional' border='0' cellspacing='0' width='100%' id='tabla_version'>");

            str.Append("<thead>");
            #region cabecera
            str.Append("<tr>");
            str.Append("<th style='width: 50px'>Opciones</th>");
            str.Append("<th style=''>N° Versión</th>");
            str.Append("<th style=''>Fecha periodo</th>");
            //str.Append("<th style=''>Observación</th>");
            str.Append("<th style=''>Usuario creación</th>");
            str.Append("<th style=''>Fecha creación</th>");
            if (tienePermiso)
                str.Append("<th style='background-color: #6FC90F; width: 30px'>Guardar REP_VCOM</th>");
            str.Append("<th style='background-color: #6FC90F;'>Usuario</th>");
            str.Append("<th style='background-color: #6FC90F;'>Fecha</th>");
            str.Append("</tr>");
            #endregion
            str.Append("</thead>");

            str.Append("<tbody>");
            #region cuerpo

            foreach (var reg in listaVersion)
            {
                str.Append("<tr>");

                str.Append("<td>");
                str.AppendFormat("<a class='' href='JavaScript:verReporte({0});' style='margin-right: 4px;'><img style='margin-top: 4px; margin-bottom: 4px;' src='{1}Content/Images/btn-open.png' title='Ver reporte' /></a>", reg.Cccvercodi, url);
                str.AppendFormat("<a class='' href='JavaScript:exportarReporte({0});' style='margin-right: 4px;'><img style='margin-top: 4px; margin-bottom: 4px;' src='{1}Content/Images/ExportExcel.png' title='Exportar reporte' /></a>", reg.Cccvercodi, url);
                str.Append("</td>");

                str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.Cccvernumero);
                str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.CccverfechaDesc);
                //str.AppendFormat("<td class='' style='text-align: left'>{0}</td>", reg.Cccverobs.Replace(ConstantesConsumoCombustible.SeparadorObs, "<br/>"));
                str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.Cccverusucreacion);
                str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.CccverfeccreacionDesc);

                if (tienePermiso)
                {
                    str.Append("<td>");
                    if (reg.Cccverestado != ConstantesConsumoCombustible.EstadoValidado)
                        str.AppendFormat("<a class='' href='JavaScript:guardarRepVcom({0});' style='margin-right: 4px;'><img style='margin-top: 4px; margin-bottom: 4px; margin-left: 40px;' src='{1}Content/Images/prnsave.png' title='Guardar en tabla REP_VCOM' /></a>", reg.Cccvercodi, url);
                    str.Append("</td>");
                }
                str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.Cccverusumodificacion);
                str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.CccverfecmodificacionDesc);

                str.Append("</tr>");
            }

            #endregion
            str.Append("</tbody>");

            str.Append("</table>");

            return str.ToString();
        }

        #region Cálculo

        /// <summary>
        /// Ejecutar proceso
        /// </summary>
        /// <param name="fechaPeriodo"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public int ProcesarCalculoVCOM(DateTime fechaPeriodo, string usuario)
        {
            //generar detalle por dia
            EjecutarReporteDiarioXPeriodo(fechaPeriodo, usuario);

            //
            DateTime fechaRegistro = DateTime.Now;

            CccVersionDTO regVersion = new CccVersionDTO()
            {
                Cccverhorizonte = ConstantesConsumoCombustible.HorizonteMensual,
                Cccverfecha = fechaPeriodo.Date,
                Cccvernumero = GetNumeroVersionActual(fechaPeriodo, ConstantesConsumoCombustible.HorizonteMensual),
                Cccverestado = ConstantesConsumoCombustible.EstadoGenerado,
                Cccverfeccreacion = fechaRegistro,
                Cccverusucreacion = usuario,
            };

            List<CccVcomDTO> listaRpt = ListarDataCalculoReporteVCOM(-1, fechaPeriodo, "-1", "-1", out ReporteVCOM reporteVcom);
            regVersion.Cccverrptcodis = string.Join(",", reporteVcom.ListaVersionDiaria.Select(x => x.Cccvercodi));

            List<string> listaObs = new List<string>();
            listaObs.AddRange(reporteVcom.ListaMsj.Select(x => x.Descripcion));

            string obs = string.Join(ConstantesConsumoCombustible.SeparadorObs, listaObs);
            regVersion.Cccverobs = obs.Length > 1500 ? obs.Substring(0, 1500) : obs;

            bool tieneCambioVersion = ExisteCambioVersionVCOM(regVersion);

            bool tieneCambio = ExisteCambioInformacionVCOM(fechaPeriodo, listaRpt);
            if (tieneCambio || tieneCambioVersion)
                return GuardarReporteVCOMTransaccional(regVersion, listaRpt);

            return 0;
        }

        /// <summary>
        /// Guardado transaccional
        /// </summary>
        /// <param name="regVersion"></param>
        /// <param name="listaRpt"></param>
        /// <returns></returns>
        private int GuardarReporteVCOMTransaccional(CccVersionDTO regVersion, List<CccVcomDTO> listaRpt)
        {

            DbTransaction tran = null;
            int ivercodi = 0;
            try
            {
                IDbConnection conn = FactorySic.GetSiMigracionRepository().BeginConnection();
                tran = FactorySic.GetSiMigracionRepository().StartTransaction(conn);

                ivercodi = this.SaveCccVersion(regVersion, conn, tran);

                int maxId = FactorySic.GetCccVcomRepository().GetMaxId();
                foreach (var regRpt in listaRpt)
                {
                    //guardar bd
                    regRpt.Cccvercodi = ivercodi;
                    regRpt.Vcomcodi = maxId;
                    this.SaveCccVcom(regRpt, conn, tran);

                    maxId++;
                }

                tran.Commit();
            }
            catch (Exception ex)
            {
                ivercodi = 0;
                if (tran != null)
                    tran.Rollback();
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }


            return ivercodi;
        }

        /// <summary>
        /// existe cambio de cabecera
        /// </summary>
        /// <param name="regMemoria"></param>
        /// <returns></returns>
        private bool ExisteCambioVersionVCOM(CccVersionDTO regMemoria)
        {
            var listaReporteBD = this.GetByCriteriaCccVersions(regMemoria.Cccverfecha, regMemoria.Cccverfecha, regMemoria.Cccverhorizonte);
            CccVersionDTO regBD = listaReporteBD.OrderByDescending(x => x.Cccvernumero).FirstOrDefault();

            if (regBD != null)
            {
                var lRptcodisMemoria = !string.IsNullOrEmpty(regMemoria.Cccverrptcodis) ? regMemoria.Cccverrptcodis.Split(',').Select(x => Convert.ToInt32(x)).ToList() : new List<int>();
                var lRptcodisBD = !string.IsNullOrEmpty(regBD.Cccverrptcodis) ? regBD.Cccverrptcodis.Split(',').Select(x => Convert.ToInt32(x)).ToList() : new List<int>();

                foreach (var rptcodi in lRptcodisMemoria)
                {
                    if (!lRptcodisBD.Contains(rptcodi))
                        return true;
                }

                foreach (var rptcodi in lRptcodisBD)
                {
                    if (!lRptcodisMemoria.Contains(rptcodi))
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// existe cambio de detalle
        /// </summary>
        /// <param name="fechaPeriodo"></param>
        /// <param name="listaRptMemoria"></param>
        /// <returns></returns>
        private bool ExisteCambioInformacionVCOM(DateTime fechaPeriodo, List<CccVcomDTO> listaRptMemoria)
        {
            List<CccVersionDTO> listaVersBD = this.GetByCriteriaCccVersions(fechaPeriodo, fechaPeriodo, ConstantesConsumoCombustible.HorizonteMensual);
            var vercodi = listaVersBD.Any() ? listaVersBD.Max(x => x.Cccvercodi) : -1;

            if (vercodi > 0)
            {
                List<CccVcomDTO> listaRptBD = GetByCriteriaCccVcoms(vercodi);

                //comparar bd con memory
                foreach (var reg1 in listaRptBD)
                {
                    var reg2 = listaRptMemoria.Find(x => x.Grupocodi == reg1.Grupocodi && x.Equipadre == reg1.Equipadre
                                                        && x.Fenergcodi == reg1.Fenergcodi && x.Tinfcoes == reg1.Tinfcoes && x.Tinfosi == reg1.Tinfosi);
                    if (reg2 != null)
                    {
                        if (reg2.Vcomvalor != reg1.Vcomvalor)
                            return true;

                        if (reg2.Vcomcodigomop != reg1.Vcomcodigomop)
                            return true;

                        if (reg2.Vcomcodigotcomb != reg1.Vcomcodigotcomb)
                            return true;
                    }
                    else
                    {
                        return true;
                    }
                }


                //comparar memory con bd
                foreach (var reg1 in listaRptMemoria)
                {
                    var reg2 = listaRptBD.Find(x => x.Grupocodi == reg1.Grupocodi && x.Equipadre == reg1.Equipadre
                                                        && x.Fenergcodi == reg1.Fenergcodi && x.Tinfcoes == reg1.Tinfcoes && x.Tinfosi == reg1.Tinfosi);
                    if (reg2 != null)
                    {
                        if (reg2.Vcomvalor != reg1.Vcomvalor)
                            return true;

                        if (reg2.Vcomcodigomop != reg1.Vcomcodigomop)
                            return true;

                        if (reg2.Vcomcodigotcomb != reg1.Vcomcodigotcomb)
                            return true;
                    }
                    else
                    {
                        return true;
                    }
                }

                return false;
            }
            else
            {
                return true;//no existe bd
            }
        }

        #endregion

        #region Información almacenada en BD

        /// <summary>
        /// Obtener el reporte con los filtros
        /// </summary>
        /// <param name="vercodi"></param>
        /// <param name="empresa"></param>
        /// <param name="central"></param>
        /// <returns></returns>
        public List<CccVcomDTO> ListarReporteVCOMxFiltro(int vercodi, string empresa, string central, out CccVersionDTO regVersion)
        {
            regVersion = GetByIdCccVersion(vercodi);

            this.ListaDataVCOMXVersionReporte(vercodi, empresa, central
                                            , out List<CccVcomDTO> listaReptotOut
                                            , out List<SiEmpresaDTO> listaEmpresa
                                            , out List<EqEquipoDTO> listaCentral
                                            , out List<EqEquipoDTO> listaEquipo);

            //Formatear reporte
            foreach (var reg in listaReptotOut)
            {
                FormatearCCCVCOM(reg);
            }

            return listaReptotOut;
        }

        /// <summary>
        /// obtener los filtros
        /// </summary>
        /// <param name="ivercodi"></param>
        /// <param name="empresa"></param>
        /// <param name="central"></param>
        /// <param name="listaReptotOut"></param>
        /// <param name="listaEmpresa"></param>
        /// <param name="listaCentral"></param>
        /// <param name="listaEquipo"></param>
        public void ListaDataVCOMXVersionReporte(int ivercodi, string empresa, string central
                                                , out List<CccVcomDTO> listaReptotOut
                                                , out List<SiEmpresaDTO> listaEmpresa
                                                , out List<EqEquipoDTO> listaCentral
                                                , out List<EqEquipoDTO> listaEquipo)
        {
            CccVersionDTO regVersion = GetByIdCccVersion(ivercodi);
            List<CccVcomDTO> listaRepdet = GetByCriteriaCccVcoms(ivercodi);

            empresa = !string.IsNullOrEmpty(empresa) ? empresa : ConstantesAppServicio.ParametroDefecto;
            central = !string.IsNullOrEmpty(central) ? central : ConstantesAppServicio.ParametroDefecto;

            if (ConstantesAppServicio.ParametroDefecto != empresa)
            {
                int[] emprcodis = empresa.Split(',').Select(x => int.Parse(x)).ToArray();
                listaRepdet = listaRepdet.Where(x => emprcodis.Contains(x.Emprcodi)).ToList();
            }

            if (ConstantesAppServicio.ParametroDefecto != central)
            {
                int[] equipadres = central.Split(',').Select(x => int.Parse(x)).ToArray();
                listaRepdet = listaRepdet.Where(x => equipadres.Contains(x.Equipadre)).ToList();
            }

            listaEmpresa = listaRepdet.GroupBy(x => x.Emprcodi).Select(x => new SiEmpresaDTO() { Emprcodi = x.Key, Emprnomb = x.First().Emprnomb }).OrderBy(x => x.Emprnomb).ToList();
            listaCentral = listaRepdet.GroupBy(x => x.Equipadre).Select(x => new EqEquipoDTO() { Equipadre = x.Key, Central = x.First().Central, Emprcodi = x.First().Emprcodi }).OrderBy(x => x.Central).ToList();
            listaEquipo = listaRepdet.GroupBy(x => x.Equicodi).Select(x => new EqEquipoDTO() { Equicodi = x.Key, Equinomb = x.First().Equinomb, Equipadre = x.First().Equipadre, Emprcodi = x.First().Emprcodi }).OrderBy(x => x.Equinomb).ToList();

            listaReptotOut = listaRepdet.OrderBy(x => x.Emprnomb).ThenBy(x => x.Central).ThenBy(x => x.Fenergnomb).ThenBy(x => x.Gruponomb).ToList();
        }

        /// <summary>
        /// Listar modos de operación
        /// </summary>
        /// <returns></returns>
        public List<PrGrupoDTO> ListarModoOperacionSEIN()
        {
            //Obtener todos los modos de operación
            List<PrGrupoDTO> listaGrupo = GetByCriteriaPrGrupoeqs(-1, -1).GroupBy(x => x.Grupocodi).Select(x => new PrGrupoDTO()
            {
                Grupocodi = x.Key,
                Gruponomb = x.First().Gruponomb,
                Grupoabrev = x.First().Grupoabrev,
                Equipadre = x.First().Equipadre,
                Emprcodi = x.First().Emprcodi,
                Emprnomb = x.First().Emprnomb,
                GrupoEstado = x.First().Grupoestado,
                Osinergcodi = x.First().Osinergcodi,
                Fenergcodi = x.First().Fenergcodi,
                Fenergnomb = x.First().Fenergnomb
            }).OrderBy(x => x.GrupoEstado).ThenBy(x => x.Gruponomb).ToList();

            foreach (var reg in listaGrupo)
            {
                reg.Emprnomb = reg.Emprnomb != null ? reg.Emprnomb.Trim() : string.Empty;
                reg.Catenomb = reg.Catenomb != null ? reg.Catenomb.Trim() : string.Empty;
                reg.Gruponomb = reg.Gruponomb != null ? reg.Gruponomb.Trim() : string.Empty;
                reg.Grupoabrev = reg.Grupoabrev != null ? reg.Grupoabrev.Trim() : string.Empty;
                reg.Areanomb = reg.Areanomb != null && !reg.Areanomb.Trim().Contains("(NO-DEF)") ? reg.Areanomb.Trim() : string.Empty;
                reg.Areadesc = reg.Areadesc != null && !reg.Areadesc.Trim().Contains("(NO-DEF)") ? reg.Areadesc.Trim() : string.Empty;

                reg.GrupoactivoDesc = reg.Grupoactivo != null ? Util.SiNoDescripcion(reg.Grupoactivo) : string.Empty;
                reg.GrupoEstadoDesc = EquipamientoHelper.EstadoDescripcion(reg.GrupoEstado);
            }

            //Filtrar solo los validos (no eliminados)
            List<string> listaEstado = new List<string>() { "A", "B", "F", "P" };
            listaGrupo = listaGrupo.Where(x => listaEstado.Contains(x.GrupoEstado)).ToList();

            return listaGrupo;
        }

        /// <summary>
        /// Generar una nueva versión con modificaciones manuales
        /// </summary>
        /// <param name="vercodiAnterior"></param>
        /// <param name="listaCambio"></param>
        /// <param name="usuario"></param>
        public void GuardarVersionConCambio(int vercodiAnterior, List<VCOMCambio> listaCambio, string usuario)
        {
            //Insumos
            CccVersionDTO regVersionBD = GetByIdCccVersion(vercodiAnterior);
            List<CccVcomDTO> listaRepdetBD = GetByCriteriaCccVcoms(vercodiAnterior);
            List<PrGrupoDTO> listaModoOp = ListarModoOperacionSEIN();
            List<SiFuenteenergiaDTO> listaFuenteEnergia = GetByCriteriaSiFuenteenergias();

            //Nueva versión
            CccVersionDTO regVersion = new CccVersionDTO
            {
                Cccvercodi = 0,
                Cccverfecha = regVersionBD.Cccverfecha,
                Cccverhorizonte = ConstantesConsumoCombustible.HorizonteMensual,
                Cccvernumero = GetNumeroVersionActual(regVersionBD.Cccverfecha, ConstantesConsumoCombustible.HorizonteMensual),
                Cccverestado = ConstantesConsumoCombustible.EstadoGenerado,
                Cccverfeccreacion = DateTime.Now,
                Cccverusucreacion = usuario,
                Cccverrptcodis = regVersionBD.Cccverrptcodis
            };

            //Cambios
            List<int> listaVcomcodiEliminado = listaCambio.Where(x => x.TipoCambio == ConstantesConsumoCombustible.TipoCambioEliminar).Select(x => x.Vcomcodi).Distinct().ToList();
            List<int> listaVcomcodiAlterado = listaCambio.Select(x => x.Vcomcodi).Distinct().ToList();

            List<CccVcomDTO> listaRepdet = new List<CccVcomDTO>();
            //Obtener los registros sin cambios
            listaRepdet.AddRange(listaRepdetBD.Where(x => !listaVcomcodiAlterado.Contains(x.Vcomcodi)).ToList());

            //Nuevos o modificados
            foreach (var item in listaCambio.Where(x => !listaVcomcodiEliminado.Contains(x.Vcomcodi)))
            {
                PrGrupoDTO regModo = listaModoOp.Find(x => x.Grupocodi == item.Grupocodi);
                SiFuenteenergiaDTO regFenerg = listaFuenteEnergia.Find(x => x.Fenergcodi == item.FenergcodiFinal);
                decimal consumo = item.ConsumoFinal ?? 0;

                if (regModo != null && regFenerg != null)
                {
                    listaRepdet.Add(new CccVcomDTO()
                    {
                        Emprcodi = regModo.Emprcodi.Value,
                        Equipadre = regModo.Equipadre,
                        Equicodi = 0,
                        Grupocodi = regModo.Grupocodi,
                        Fenergcodi = regFenerg.Fenergcodi,
                        Tinfcoes = regFenerg.Tinfcoes,
                        Tinfosi = regFenerg.Tinfosi,
                        Vcomvalor = consumo,
                        Vcomcodigomop = regModo.Osinergcodi,
                        Vcomcodigotcomb = regFenerg.Osinergcodi
                    });
                }
            }

            //Validar duplicados
            List<string> listaDupl = new List<string>();
            foreach (var item in listaRepdet.GroupBy(x => new { x.Grupocodi, x.Fenergcodi }))
            {
                if (item.Count() > 1)
                {
                    listaDupl.Add(string.Format("Modo de operación '{0}' con combustible '{1}'", listaModoOp.FirstOrDefault(x => x.Grupocodi == item.Key.Grupocodi).Gruponomb
                                            , listaFuenteEnergia.FirstOrDefault(x => x.Fenergcodi == item.Key.Fenergcodi).Fenergnomb));
                }
            }
            if (listaDupl.Any())
            {
                throw new ArgumentException(string.Format("Los siguientes registros tienen duplicados: {0}", string.Join(", ", listaDupl)));
            }

            //Guardar en BD
            int vercodiNuevo = GuardarReporteVCOMTransaccional(regVersion, listaRepdet);

            //Generar log
            string nombreArchivoLog = GetNombreArchivoLogCambio(vercodiNuevo);
            string carpetaTemporal = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
            GenerarRptExcelLogCambioVCOM(carpetaTemporal, regVersionBD.Cccvernumero, regVersion.Cccvernumero, regVersion.Cccverfecha, listaCambio, nombreArchivoLog);

            //Mover a FileServer
            MoverArchivoVCOMFileServer(carpetaTemporal, nombreArchivoLog);
        }

        #endregion

        #region Exportación

        /// <summary>
        /// Generar archivo excel
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="vercodi"></param>
        /// <param name="empresa"></param>
        /// <param name="central"></param>
        /// <param name="nameFile"></param>
        public void GenerarRptExcelVCOM(string ruta, int vercodi, string empresa, string central, out string nameFile)
        {
            List<CccVcomDTO> listaRept = ListarReporteVCOMxFiltro(vercodi, empresa, central, out CccVersionDTO regVersion);

            ListarDataCalculoReporteVCOM(vercodi, regVersion.Cccverfecha, empresa, central, out ReporteVCOM objRpt);

            //Nombre de archivo
            nameFile = string.Format("ReporteVCOM_{0}_revis_{1}.xlsx", regVersion.Cccverfecha.ToString(ConstantesAppServicio.FormatoMes), regVersion.Cccvernumero);

            string rutaFile = ruta + nameFile;

            FileInfo newFile = new FileInfo(rutaFile);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                GenerarHojaExcelVCOM(xlPackage, "CUADRO", 2, 2, regVersion.Cccverfecha, listaRept, objRpt.ListaMsj);
                xlPackage.Save();

                GenerarTablaConsumoDiario(xlPackage, "CONSUMO", 2, 2, objRpt.FechaIni, objRpt.FechaFin, objRpt.ListaUnidad, objRpt.ListaDetalleDiario, objRpt.ListaVersionDiaria, objRpt.ListaMsj);
                xlPackage.Save();
            }
        }

        /// <summary>
        /// Generar hoja principal
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="nameWS"></param>
        /// <param name="rowIniTabla"></param>
        /// <param name="colIniTabla"></param>
        /// <param name="listaRept"></param>
        /// <param name="listaLeyenda"></param>
        /// <param name="listaMjs"></param>
        private void GenerarHojaExcelVCOM(ExcelPackage xlPackage, string nameWS, int rowIniTabla, int colIniTabla, DateTime fecha
                                                            , List<CccVcomDTO> listaRept, List<ResultadoValidacionAplicativo> listaMjs)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

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
            int colModo = colCentral + 1;
            int colTipo = colModo + 1;
            int colMedidaCoes = colTipo + 1;
            int colConsumoOsi = colMedidaCoes + 1;
            int colMedidaOsi = colConsumoOsi + 1;
            int colCodigoModo = colMedidaOsi + 1;
            int colCodigoTipo = colCodigoModo + 1;

            int rowTitulo = rowIniTabla;
            ws.Cells[rowTitulo, colEmpresa].Value = string.Format("Reporte VCOM - {0}", fecha.ToString(ConstantesAppServicio.FormatoMes));
            UtilExcel.CeldasExcelAgrupar(ws, rowTitulo, colEmpresa, rowTitulo, colCodigoTipo);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowTitulo, colEmpresa, rowTitulo, colCodigoTipo, colorLinea);
            UtilExcel.SetFormatoCelda(ws, rowTitulo, colEmpresa, rowTitulo, colCodigoTipo, "Centro", "Centro", colorTextoFijo, colorCeldaFijo, font, 12, true, true);

            int rowEmpresa = rowTitulo + 1;
            ws.Cells[rowEmpresa, colEmpresa].Value = "EMPRESA";
            ws.Cells[rowEmpresa, colCentral].Value = "CENTRAL";
            ws.Cells[rowEmpresa, colModo].Value = "Modo de Operación";
            ws.Cells[rowEmpresa, colTipo].Value = "COMBUSTIBLE";
            ws.Cells[rowEmpresa, colMedidaCoes].Value = "MEDIDA COES";
            ws.Cells[rowEmpresa, colConsumoOsi].Value = "CONSUMO OSINERGMIN";
            ws.Cells[rowEmpresa, colMedidaOsi].Value = "Medida Osinergmin";
            ws.Cells[rowEmpresa, colCodigoModo].Value = "Código Modo";
            ws.Cells[rowEmpresa, colCodigoTipo].Value = "Código Combustible";

            UtilExcel.SetFormatoCelda(ws, rowEmpresa, colEmpresa, rowEmpresa, colCodigoTipo, "Centro", "Centro", colorTextoFijo, colorCeldaFijo, font, 12, true, true);
            UtilExcel.SetFormatoCelda(ws, rowEmpresa, colConsumoOsi, rowEmpresa, colMedidaOsi, "Centro", "Centro", colorTextoFijo, colorCeldaOsi, font, 12, true, true);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowEmpresa, colEmpresa, rowEmpresa, colCodigoTipo, colorLinea, true);

            //ws.Row(rowEmpresa).Height = 36;
            ws.Column(1).Width = 3;
            ws.Column(colEmpresa).Width = 40;
            ws.Column(colCentral).Width = 30;
            ws.Column(colModo).Width = 40;
            ws.Column(colTipo).Width = 15;
            ws.Column(colMedidaCoes).Width = 15;
            ws.Column(colConsumoOsi).Width = 20;
            ws.Column(colMedidaOsi).Width = 15;
            ws.Column(colCodigoModo).Width = 15;
            ws.Column(colCodigoTipo).Width = 15;

            #endregion

            #region Cuerpo

            int rowData = rowEmpresa;
            int rowIniRangoEmpresa = rowData + 1;
            string empresaActual, empresaSiguiente;

            for (int i = 0; i < listaRept.Count; i++)
            {
                var reg = listaRept[i];

                rowData++;
                empresaActual = reg.Emprnomb;
                empresaSiguiente = i + 1 < listaRept.Count ? listaRept[i + 1].Emprnomb : string.Empty;

                ws.Cells[rowData, colEmpresa].Value = reg.Emprnomb;
                ws.Cells[rowData, colCentral].Value = reg.Central;
                ws.Cells[rowData, colModo].Value = reg.Gruponomb;
                ws.Cells[rowData, colTipo].Value = reg.Fenergnomb;
                ws.Cells[rowData, colMedidaCoes].Value = reg.Tinfcoesabrev;
                ws.Cells[rowData, colConsumoOsi].Value = reg.Vcomvalor;
                ws.Cells[rowData, colMedidaOsi].Value = reg.Tinfosiabrev;
                ws.Cells[rowData, colCodigoModo].Value = reg.Vcomcodigomop;
                ws.Cells[rowData, colCodigoTipo].Value = reg.Vcomcodigotcomb;

                UtilExcel.SetFormatoCelda(ws, rowData, colEmpresa, rowData, colCodigoTipo, "Centro", "Izquierda", colorTextoCuerpo, colorCeldaCuerpo, font, 12, false);
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowData, colModo, rowData, colMedidaCoes, "Centro");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowData, colMedidaOsi, rowData, colCodigoTipo, "Centro");

                ws.Cells[rowData, colConsumoOsi].Value = reg.Vcomvalor;
                UtilExcel.SetFormatoCelda(ws, rowData, colConsumoOsi, rowData, colConsumoOsi, "Centro", "Derecha", colorTextoCuerpo, colorCeldaCuerpo, font, 12, false);

                UtilExcel.CeldasExcelIndentar(ws, rowData, colConsumoOsi, rowData, colConsumoOsi, 1);
                UtilExcel.CeldasExcelFormatoNumero(ws, rowData, colConsumoOsi, rowData, colConsumoOsi, 4);

                UtilExcel.BorderCeldasLineaDelgada(ws, rowData, colEmpresa, rowData, colCodigoTipo, colorLinea, true);

                //border central
                if (empresaActual != empresaSiguiente)
                {
                    UtilExcel.BorderCeldasLineaGruesa(ws, rowIniRangoEmpresa, colEmpresa, rowData, colCodigoTipo, colorLinea);
                    rowIniRangoEmpresa = rowData + 1;
                }
            }

            #endregion

            if (listaMjs.Any())
            {
                int rowIniMsj = rowData + 3;
                ws.Cells[rowIniMsj, colEmpresa].Value = "Observaciones:";
                UtilExcel.SetFormatoCelda(ws, rowIniMsj, colEmpresa, rowIniMsj, colEmpresa, "Centro", "Izquierda", "#FF0000", colorCeldaCuerpo, font, 12, true);

                List<string> listaDescripcion = listaMjs.Select(x => x.Descripcion).Distinct().ToList();

                foreach (var desc in listaDescripcion)
                {
                    rowIniMsj++;
                    ws.Cells[rowIniMsj, colEmpresa].Value = desc;
                }
            }

            //No mostrar lineas
            ws.View.ShowGridLines = false;

            //filter
            ws.Cells[rowEmpresa, colEmpresa, rowEmpresa, colCentral].AutoFilter = true;

            ws.View.FreezePanes(rowEmpresa + 1, colModo + 1);

            ws.View.ZoomScale = 90;

            //excel con Font Arial
            var allCells = ws.Cells[1, 1, ws.Dimension.End.Row, ws.Dimension.End.Column];
            var cellFont = allCells.Style.Font;
            cellFont.Name = font;
        }

        /// <summary>
        /// Generar la tabla. Reutilizado para la pestaña principal y para las centrales
        /// </summary>
        /// <param name="incluirCaracteristica"></param>
        /// <param name="ws"></param>
        /// <param name="rowIniTabla"></param>
        /// <param name="colIniTabla"></param>
        /// <param name="listaRept"></param>
        /// <param name="listaLeyenda"></param>
        /// <param name="listaMjs"></param>
        private void GenerarTablaConsumoDiario(ExcelPackage xlPackage, string nameWS, int rowIniTabla, int colIniTabla, DateTime fechaIni, DateTime fechaFin
                                            , List<CccReporteDTO> listaUnidad, List<CccReporteDTO> listaDetalle, List<CccVersionDTO> listaVersion, List<ResultadoValidacionAplicativo> listaMjs)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            string font = "Calibri";
            string colorCeldaFijo = "#0070C0";
            string colorTextoFijo = "#ffffff";

            string colorCeldaCuerpo = "#FFFFFF";
            string colorTextoCuerpo = "#000000";

            string colorLinea = "#000000";
            int numDias = (fechaFin - fechaIni).Days + 1;

            #region  Filtros y Cabecera

            int colEmpresa = colIniTabla;
            int colCentral = colEmpresa + 1;
            int colUnidad = colCentral + 1;
            int colTipo = colUnidad + 1;
            int colMedida = colTipo + 1;
            int colReal = colMedida + 1;

            int rowTitulo = rowIniTabla;
            ws.Cells[rowTitulo, colEmpresa].Value = string.Format("Consumo de combustible - {0}", fechaIni.ToString(ConstantesAppServicio.FormatoMes));
            UtilExcel.CeldasExcelAgrupar(ws, rowTitulo, colEmpresa, rowTitulo, colReal);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowTitulo, colEmpresa, rowTitulo, colReal, colorLinea);
            UtilExcel.SetFormatoCelda(ws, rowTitulo, colEmpresa, rowTitulo, colReal, "Centro", "Centro", colorTextoFijo, colorCeldaFijo, font, 12, true, true);

            int rowEmpresa = rowTitulo + 1;
            ws.Cells[rowEmpresa, colEmpresa].Value = "EMPRESA";
            ws.Cells[rowEmpresa, colCentral].Value = "CENTRAL";
            ws.Cells[rowEmpresa, colUnidad].Value = "UNIDAD";
            ws.Cells[rowEmpresa, colTipo].Value = "COMBUSTIBLE";
            ws.Cells[rowEmpresa, colMedida].Value = "MEDIDA COES";
            ws.Cells[rowEmpresa, colReal].Value = "CONSUMO TOTAL";
            ws.Column(1).Width = 3;
            ws.Column(colEmpresa).Width = 30;
            ws.Column(colCentral).Width = 20;
            ws.Column(colUnidad).Width = 20;
            ws.Column(colTipo).Width = 15;
            ws.Column(colMedida).Width = 15;
            ws.Column(colReal).Width = 20;

            int colIniFecha = colReal + 1;
            int colTmp = colIniFecha;
            for (var day = fechaIni; day <= fechaFin; day = day.AddDays(1))
            {
                ws.Cells[rowTitulo, colTmp].Value = day;
                ws.Cells[rowTitulo, colTmp].Style.Numberformat.Format = "dd/mm";
                ws.Column(colTmp).Width = 15;

                var regVersionXDia = listaVersion.Find(x => x.Cccverfecha == day);
                if (regVersionXDia != null)
                {
                    ws.Cells[rowEmpresa, colTmp].Value = "N° " + regVersionXDia.Cccvernumero;
                    string descVersion = string.Format("Fecha de creación: {0}", regVersionXDia.CccverfeccreacionDesc) + "\n";
                    descVersion += string.Format("Usuario de creación: {0}", regVersionXDia.Cccverusucreacion) + "\n";
                    if (!string.IsNullOrEmpty(regVersionXDia.Cccverobs))
                        descVersion += string.Format("Observación: {0}", string.Join("\n", regVersionXDia.ListaObs));
                    UtilExcel.AgregarComentarioExcel(ws, rowEmpresa, colTmp, descVersion);
                }

                colTmp++;
            }

            UtilExcel.SetFormatoCelda(ws, rowTitulo, colEmpresa, rowEmpresa, colReal + numDias, "Centro", "Centro", colorTextoFijo, colorCeldaFijo, font, 12, true);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowTitulo, colEmpresa, rowTitulo, colReal + numDias, colorLinea, true);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowEmpresa, colEmpresa, rowEmpresa, colReal + numDias, colorLinea, true);

            //ws.Row(rowEmpresa).Height = 36;

            #endregion

            #region Cuerpo

            int rowData = rowEmpresa;
            int rowIniRangoCentral = rowData + 1;
            string centralActual, centralSiguiente;

            for (int i = 0; i < listaUnidad.Count; i++)
            {
                var reg = listaUnidad[i];

                rowData++;
                centralActual = reg.Central;
                centralSiguiente = i + 1 < listaUnidad.Count ? listaUnidad[i + 1].Central : string.Empty;

                ws.Cells[rowData, colEmpresa].Value = reg.Emprnomb;
                ws.Cells[rowData, colCentral].Value = reg.Central;
                ws.Cells[rowData, colUnidad].Value = reg.Equinomb;
                ws.Cells[rowData, colTipo].Value = reg.Fenergnomb;
                ws.Cells[rowData, colMedida].Value = reg.Tipoinfoabrev;
                UtilExcel.SetFormatoCelda(ws, rowData, colEmpresa, rowData, colUnidad, "Centro", "Izquierda", colorTextoCuerpo, colorCeldaCuerpo, font, 12, false);
                UtilExcel.SetFormatoCelda(ws, rowData, colTipo, rowData, colMedida, "Centro", "Centro", colorTextoCuerpo, colorCeldaCuerpo, font, 12, false);

                ws.Cells[rowData, colReal].Value = reg.Cccrptvalorreal;

                UtilExcel.SetFormatoCelda(ws, rowData, colReal, rowData, colReal + numDias, "Centro", "Derecha", colorTextoCuerpo, colorCeldaCuerpo, font, 12, false);
                UtilExcel.BorderCeldasLineaDelgada(ws, rowData, colEmpresa, rowData, colReal + numDias, colorLinea, true);

                //
                List<CccReporteDTO> listaDetXEq = listaDetalle.Where(x => x.Fenergcodi == reg.Fenergcodi && x.Equicodi == reg.Equicodi).ToList();

                colTmp = colIniFecha;
                for (var day = fechaIni; day <= fechaFin; day = day.AddDays(1))
                {
                    var regDetXDia = listaDetXEq.Find(x => x.Cccverfecha == day);
                    if (regDetXDia != null)
                    {
                        ws.Cells[rowData, colTmp].Value = regDetXDia.Cccrptvalorreal;
                        UtilExcel.AgregarComentarioExcel(ws, rowData, colTmp, regDetXDia.Mogruponomb);
                    }

                    colTmp++;
                }

                UtilExcel.CeldasExcelIndentar(ws, rowData, colReal, rowData, colReal + numDias, 1);
                UtilExcel.CeldasExcelFormatoNumero(ws, rowData, colReal, rowData, colReal + numDias, 2);

                //border central
                if (centralActual != centralSiguiente)
                {
                    UtilExcel.BorderCeldasLineaGruesa(ws, rowIniRangoCentral, colEmpresa, rowData, colReal, colorLinea);
                    UtilExcel.BorderCeldasLineaGruesa(ws, rowIniRangoCentral, colReal + 1, rowData, colReal + numDias, colorLinea);
                    rowIniRangoCentral = rowData + 1;
                }
            }

            #endregion

            if (listaMjs.Any())
            {
                int rowIniMsj = rowData + 3;
                ws.Cells[rowIniMsj, colEmpresa].Value = "Observaciones:";
                UtilExcel.SetFormatoCelda(ws, rowIniMsj, colEmpresa, rowIniMsj, colEmpresa, "Centro", "Izquierda", "#FF0000", colorCeldaCuerpo, font, 12, true);

                List<string> listaDescripcion = listaMjs.Select(x => x.Descripcion).Distinct().ToList();

                foreach (var desc in listaDescripcion)
                {
                    rowIniMsj++;
                    ws.Cells[rowIniMsj, colEmpresa].Value = desc;
                }
            }

            //No mostrar lineas
            ws.View.ShowGridLines = false;

            //filter
            ws.Cells[rowEmpresa, colEmpresa, rowEmpresa, colCentral].AutoFilter = true;

            ws.View.FreezePanes(rowEmpresa + 1, colReal + 1);

            ws.View.ZoomScale = 90;

            //excel con Font Arial
            var allCells = ws.Cells[1, 1, ws.Dimension.End.Row, ws.Dimension.End.Column];
            var cellFont = allCells.Style.Font;
            cellFont.Name = font;
        }

        /// <summary>
        /// Nombre de archivo log
        /// </summary>
        /// <param name="verscodi"></param>
        /// <returns></returns>
        public string GetNombreArchivoLogCambio(int verscodi)
        {
            return string.Format("LogCambioVCOM_revis_{0}.xlsx", verscodi);
        }

        /// <summary>
        /// Generar archivo log de cambios
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="vercodiAnterior"></param>
        /// <param name="vercodiNuevo"></param>
        /// <param name="listaCambio"></param>
        /// <param name="nameFile"></param>
        public void GenerarRptExcelLogCambioVCOM(string ruta, int nroVersionAnterior, int nroVersionFinal, DateTime fechaPeriodo, List<VCOMCambio> listaCambio, string nameFile)
        {
            string rutaFile = ruta + nameFile;

            FileInfo newFile = new FileInfo(rutaFile);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }

            //crear archivo
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                GenerarHojaExcelLogCambioVCOM(xlPackage, "Cambios", nroVersionAnterior, nroVersionFinal, fechaPeriodo, listaCambio);
                xlPackage.Save();
            }
        }

        private void GenerarHojaExcelLogCambioVCOM(ExcelPackage xlPackage, string nameWS, int nroVersionAnterior, int nroVersionFinal, DateTime fechaPeriodo, List<VCOMCambio> listaCambio)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            string font = "Calibri";
            string colorCeldaFijo = "#0070C0";
            string colorTextoFijo = "#ffffff";

            string colorCeldaCuerpo = "#FFFFFF";
            string colorTextoCuerpo = "#000000";

            string colorLinea = "#000000";

            #region  Filtros y Cabecera
            int rowIniTabla = 5;
            int colIniTabla = 2;

            int colNCambio = colIniTabla;
            int colTipoCambio = colNCambio + 1;
            int colModo = colTipoCambio + 1;
            int colFenergAnt = colModo + 1;
            int colFenergFinal = colFenergAnt + 1;
            int colConsumoAnt = colFenergFinal + 1;
            int colConsumoFinal = colConsumoAnt + 1;

            int rowTitulo = rowIniTabla;
            ws.Cells[2, colNCambio].Value = "Versión anterior:";
            ws.Cells[3, colNCambio].Value = "Versión final:";
            ws.Cells[2, colNCambio + 1].Value = "n° " + nroVersionAnterior;
            ws.Cells[3, colNCambio + 1].Value = "n° " + nroVersionFinal;
            ws.Cells[rowTitulo, colNCambio].Value = string.Format("Listado de cambios - {0}", fechaPeriodo.ToString(ConstantesAppServicio.FormatoMes));
            UtilExcel.CeldasExcelAgrupar(ws, rowTitulo, colNCambio, rowTitulo, colConsumoFinal);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowTitulo, colNCambio, rowTitulo, colConsumoFinal, colorLinea);
            UtilExcel.SetFormatoCelda(ws, rowTitulo, colNCambio, rowTitulo, colConsumoFinal, "Centro", "Centro", colorTextoFijo, colorCeldaFijo, font, 12, true, true);

            int rowEmpresa = rowTitulo + 1;
            ws.Cells[rowEmpresa, colNCambio].Value = "N° Cambio";
            ws.Cells[rowEmpresa, colTipoCambio].Value = "Tipo de Cambio";
            ws.Cells[rowEmpresa, colModo].Value = "Modo de Operación";
            ws.Cells[rowEmpresa, colFenergAnt].Value = "Combustible Anterior";
            ws.Cells[rowEmpresa, colFenergFinal].Value = "Combustible Final";
            ws.Cells[rowEmpresa, colConsumoAnt].Value = "Consumo OSINERGMIN Anterior";
            ws.Cells[rowEmpresa, colConsumoFinal].Value = "Consumo OSINERGMIN Final";

            UtilExcel.SetFormatoCelda(ws, rowEmpresa, colNCambio, rowEmpresa, colConsumoFinal, "Centro", "Centro", colorTextoFijo, colorCeldaFijo, font, 12, true, true);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowEmpresa, colNCambio, rowEmpresa, colConsumoFinal, colorLinea, true);

            //ws.Row(rowEmpresa).Height = 36;
            ws.Column(1).Width = 3;
            ws.Column(colNCambio).Width = 18;
            ws.Column(colTipoCambio).Width = 18;
            ws.Column(colModo).Width = 50;
            ws.Column(colFenergAnt).Width = 15;
            ws.Column(colFenergFinal).Width = 15;
            ws.Column(colConsumoAnt).Width = 20;
            ws.Column(colConsumoFinal).Width = 20;

            #endregion

            #region Cuerpo

            int rowData = rowEmpresa;

            for (int i = 0; i < listaCambio.Count; i++)
            {
                var reg = listaCambio[i];

                rowData++;

                ws.Cells[rowData, colNCambio].Value = reg.NroCambio;
                ws.Cells[rowData, colTipoCambio].Value = reg.TipoCambioDesc;
                ws.Cells[rowData, colModo].Value = reg.Gruponomb;
                ws.Cells[rowData, colFenergAnt].Value = reg.FenergnombAnterior;
                ws.Cells[rowData, colFenergFinal].Value = reg.FenergnombFinal;
                ws.Cells[rowData, colConsumoAnt].Value = reg.ConsumoAnterior;
                ws.Cells[rowData, colConsumoFinal].Value = reg.ConsumoFinal;

                UtilExcel.SetFormatoCelda(ws, rowData, colNCambio, rowData, colConsumoFinal, "Centro", "Izquierda", colorTextoCuerpo, colorCeldaCuerpo, font, 12, false);
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowData, colNCambio, rowData, colConsumoFinal, "Centro");

                UtilExcel.BorderCeldasLineaDelgada(ws, rowData, colNCambio, rowData, colConsumoFinal, colorLinea, true);
            }

            #endregion

            //No mostrar lineas
            ws.View.ShowGridLines = false;

            //excel con Font Arial
            var allCells = ws.Cells[1, 1, ws.Dimension.End.Row, ws.Dimension.End.Column];
            var cellFont = allCells.Style.Font;
            cellFont.Name = font;
        }

        private void MoverArchivoVCOMFileServer(string carpetaTemporal, string filename)
        {
            //mover a fileserver
            string pathAlternativo = GetPathPrincipal();

            int resCopia = FileServer.CopiarFileAlterFinal(carpetaTemporal, GetCarpetaVCOM(), filename, pathAlternativo);
            if (resCopia == -1)
                throw new ArgumentException("No se pudo guardar el Log en el FileServer.");

            //eliminar de carpeta temporal
            System.IO.File.Delete(carpetaTemporal + filename);
        }

        #endregion

        #endregion

        #region Ejecutar CCC de todo el mes

        /// <summary>
        /// Ejecutar el proceso diario en una sola acción
        /// </summary>
        /// <param name="fechaPeriodo"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public string EjecutarReporteDiarioXPeriodo(DateTime fechaPeriodo, string usuario)
        {
            string obs = "";
            //Validar insumos
            DateTime fechaIni = fechaPeriodo.Date;
            DateTime fechaFin = fechaPeriodo.Date.AddMonths(1).AddDays(-1);

            for (var day = fechaIni; day <= fechaFin; day = day.AddDays(1))
            {
                var codigo = ProcesarCalculoConsumoCombustible(day, usuario, true);
                if (codigo != 0)
                    obs += string.Format("Se generó una nueva versión para el día {0}. \n", day.ToString(ConstantesAppServicio.FormatoFecha));
            }

            return obs;
        }

        #endregion

        #region Guardar en tabla REP_VCOM

        /// <summary>
        /// Guardar en tabla REP_VCOM
        /// </summary>
        /// <param name="verscodi"></param>
        /// <param name="usuario"></param>
        public void EjecutarGuardarEnRepVcom(int verscodi, string usuario)
        {
            List<CccVcomDTO> listaRept = ListarReporteVCOMxFiltro(verscodi, "-1", "-1", out CccVersionDTO regVersion);
            int periodo = Convert.ToInt32(regVersion.Cccverfecha.ToString(ConstantesAppServicio.FormatoAnioMes));

            List<RepVcomDTO> listaRepVcom = new List<RepVcomDTO>();
            foreach (var reg in listaRept)
            {
                var regRepVcom = new RepVcomDTO();
                regRepVcom.Periodo = periodo;
                regRepVcom.Codigomodooperacion = reg.Vcomcodigomop;
                regRepVcom.Codigotipocombustible = reg.Vcomcodigotcomb;
                regRepVcom.Valor = reg.Vcomvalor;

                listaRepVcom.Add(regRepVcom);
            }

            //elimnar la info de bd
            DeleteRepVcom(periodo);

            //guardar en bd
            foreach (var reg in listaRepVcom)
            {
                SaveRepVcom(reg);
            }


            regVersion.Cccverusumodificacion = usuario;
            regVersion.Cccverfecmodificacion = DateTime.Now;
            regVersion.Cccverestado = ConstantesConsumoCombustible.EstadoValidado;
            UpdateCccVersion(regVersion);
        }


        #endregion


        #region Manejo de Archivos

        /// <summary>
        /// Permite obtener la carpeta principal de PR05
        /// </summary>
        /// <returns></returns>
        public string GetPathPrincipal()
        {
            //- Definimos la carpeta raiz (termina con /)
            string pathRaiz = FileServer.GetDirectory();
            return pathRaiz;
        }

        public string GetCarpetaVCOM()
        {
            //- Seteamos la carpeta correspondiente al dia
            string pathSubcarpeta = ConstantesConsumoCombustible.FolderRaizVCOM;
            return pathSubcarpeta;
        }

        public byte[] GetBufferArchivoEnvioFinal(string fileName)
        {
            string pathAlternativo = GetPathPrincipal();

            try
            {
                string pathDestino = GetCarpetaVCOM();

                if (FileServer.VerificarExistenciaFile(pathDestino, fileName, pathAlternativo))
                {
                    return FileServer.DownloadToArrayByte(pathDestino + fileName, pathAlternativo);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new ArgumentException("No se pudo descargar el archivo del servidor.", ex);
            }

            return null;
        }

        #endregion

    }

    public class VCOMCambio
    {
        public int Vcomcodi { get; set; }
        public string TipoCambioDesc { get; set; }
        public int TipoCambio { get; set; }
        public int NroCambio { get; set; }

        public int Grupocodi { get; set; }
        public string Gruponomb { get; set; }
        public int FenergcodiAnterior { get; set; }
        public int FenergcodiFinal { get; set; }
        public string FenergnombAnterior { get; set; }
        public string FenergnombFinal { get; set; }
        public decimal? ConsumoAnterior { get; set; }
        public decimal? ConsumoFinal { get; set; }
    }
}
