using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.Framework.Base.Core;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.CortoPlazo;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.General.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using log4net;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace COES.Servicios.Aplicacion.Monitoreo
{
    public class MonitoreoAppServicio : AppServicioBase
    {
        EjecutadoAppServicio servEjec = new EjecutadoAppServicio();
        ParametroAppServicio servParametro = new ParametroAppServicio();
        CortoPlazoAppServicio servCP = new CortoPlazoAppServicio();
        IEODAppServicio servIEOD = new IEODAppServicio();
        HorasOperacionAppServicio servHO = new HorasOperacionAppServicio();

        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(MonitoreoAppServicio));

        #region Métodos Tabla MMM_BANDTOL

        /// <summary>
        /// Inserta un registro de la tabla MMM_BANDTOL
        /// </summary>
        public void SaveMmmBandtol(MmmBandtolDTO entity)
        {
            try
            {
                FactorySic.GetMmmBandtolRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla MMM_BANDTOL
        /// </summary>
        public void UpdateMmmBandtol(MmmBandtolDTO entity)
        {
            try
            {
                FactorySic.GetMmmBandtolRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla MMM_BANDTOL
        /// </summary>
        public void DeleteMmmBandtol(int mmmtolcodi)
        {
            try
            {
                FactorySic.GetMmmBandtolRepository().Delete(mmmtolcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla MMM_BANDTOL
        /// </summary>
        public MmmBandtolDTO GetByIdMmmBandtol(int mmmtolcodi)
        {
            return FactorySic.GetMmmBandtolRepository().GetById(mmmtolcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla MMM_BANDTOL
        /// </summary>
        public List<MmmBandtolDTO> ListMmmBandtols()
        {
            return FactorySic.GetMmmBandtolRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla MmmBandtol
        /// </summary>
        public List<MmmBandtolDTO> GetByCriteriaMmmBandtols()
        {
            return FactorySic.GetMmmBandtolRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla MMM_JUSTIFICACION

        /// <summary>
        /// Inserta un registro de la tabla MMM_JUSTIFICACION
        /// </summary>
        public void SaveMmmJustificacion(MmmJustificacionDTO entity)
        {
            try
            {
                FactorySic.GetMmmJustificacionRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla MMM_JUSTIFICACION
        /// </summary>
        public void UpdateMmmJustificacion(MmmJustificacionDTO entity)
        {
            try
            {
                FactorySic.GetMmmJustificacionRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla MMM_JUSTIFICACION
        /// </summary>
        public void DeleteMmmJustificacion(int mjustcodi)
        {
            try
            {
                FactorySic.GetMmmJustificacionRepository().Delete(mjustcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla MMM_JUSTIFICACION
        /// </summary>
        public MmmJustificacionDTO GetByIdMmmJustificacion(int mjustcodi)
        {
            return FactorySic.GetMmmJustificacionRepository().GetById(mjustcodi);
        }

        #endregion

        #region Métodos Tabla MMM_INDICADOR

        /// <summary>
        /// Inserta un registro de la tabla MMM_INDICADOR
        /// </summary>
        public void SaveMmmIndicador(MmmIndicadorDTO entity)
        {
            try
            {
                FactorySic.GetMmmIndicadorRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla MMM_INDICADOR
        /// </summary>
        public void UpdateMmmIndicador(MmmIndicadorDTO entity)
        {
            try
            {
                FactorySic.GetMmmIndicadorRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla MMM_INDICADOR
        /// </summary>
        public void DeleteMmmIndicador(int immecodi)
        {
            try
            {
                FactorySic.GetMmmIndicadorRepository().Delete(immecodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla MMM_INDICADOR
        /// </summary>
        public MmmIndicadorDTO GetByIdMmmIndicador(int immecodi)
        {
            return FactorySic.GetMmmIndicadorRepository().GetById(immecodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla MMM_INDICADOR
        /// </summary>
        public List<MmmIndicadorDTO> ListMmmIndicadors()
        {
            return FactorySic.GetMmmIndicadorRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla MmmIndicador
        /// </summary>
        public List<MmmIndicadorDTO> GetByCriteriaMmmIndicadors()
        {
            return FactorySic.GetMmmIndicadorRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla MMM_VERSION

        /// <summary>
        /// Inserta un registro de la tabla MMM_VERSION
        /// </summary>
        public int SaveMmmVersion(MmmVersionDTO entity)
        {
            return FactorySic.GetMmmVersionRepository().Save(entity);
        }
        /// <summary>
        /// Actualizacion Masiva de versiones por periodo
        /// </summary>
        public void UpdateMmmVersionPeriodo(MmmVersionDTO entity)
        {
            if (entity.Vermmestado == ConstantesMonitoreo.EstadoAprobado)
            {
                List<MmmVersionDTO> lista = this.GetByCriteriaMmmVersions();
                this.UpdateMmmVersion(entity);
                var listVersion = lista.Where(x => x.Vermmfechaperiodo == entity.Vermmfechaperiodo && x.Vermmcodi != entity.Vermmcodi).ToList();
                //Actualiza  estado a publicado
                foreach (var recorrido in listVersion)
                {
                    MmmVersionDTO regVersion = new MmmVersionDTO();
                    regVersion.Vermmcodi = recorrido.Vermmcodi;
                    regVersion.Vermmfechaaprobacion = null;
                    regVersion.Vermmestado = ConstantesMonitoreo.EstadoNovigente;
                    this.UpdateMmmVersionEstado(regVersion);
                }
            }
            else
            {
                this.UpdateMmmVersion(entity);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla MMM_VERSION
        /// </summary>
        public void UpdateMmmVersion(MmmVersionDTO entity)
        {
            try
            {
                FactorySic.GetMmmVersionRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// Actualiza un registro de la tabla MMM_VERSION
        /// </summary>
        public void UpdateMmmVersionEstado(MmmVersionDTO entity)
        {
            try
            {
                FactorySic.GetMmmVersionRepository().UpdateEstadoVersion(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualizar porcentaje de generación
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateMmmVersionPorcentaje(MmmVersionDTO entity)
        {
            try
            {
                FactorySic.GetMmmVersionRepository().UpdatePorcentaje(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla MMM_VERSION
        /// </summary>
        public void DeleteMmmVersion(int vermmcodi)
        {
            try
            {
                FactorySic.GetMmmVersionRepository().Delete(vermmcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla MMM_VERSION
        /// </summary>
        public MmmVersionDTO GetByIdMmmVersion(int vermmcodi)
        {
            return FactorySic.GetMmmVersionRepository().GetById(vermmcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla MMM_VERSION
        /// </summary>
        public List<MmmVersionDTO> ListMmmVersions()
        {
            List<MmmVersionDTO> l = FactorySic.GetMmmVersionRepository().List();

            foreach (var reg in l)
            {
                reg.VermmfeccreacionDesc = reg.Vermmfeccreacion != null ? reg.Vermmfeccreacion.ToString(ConstantesAppServicio.FormatoFechaHora) : string.Empty;
                reg.VermmfechaaprobacionDesc = reg.Vermmfechaaprobacion != null ? reg.Vermmfechaaprobacion.Value.ToString(ConstantesAppServicio.FormatoFechaHora) : string.Empty;
                reg.Vermmmotivo = reg.Vermmmotivo != null ? reg.Vermmmotivo.Trim() : string.Empty;
                reg.Vermmusucreacion = reg.Vermmusucreacion != null ? reg.Vermmusucreacion.Trim() : string.Empty;
                reg.VermmestadoDesc = (reg.Vermmestado == 1) ? "Por Aprobar" : (reg.Vermmestado == 2) ? "Publicado" : "No vigente";
                reg.vermmPortalDesc = (reg.Vermmmotivoportal == 1) ? "Si" : "No";
                reg.Vermmporcentaje = reg.Vermmporcentaje;
                reg.Vermmmotivo = reg.Vermmmotivo;
            }
            return l;
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla MMM_VERSION
        /// </summary>
        public List<MmmVersionDTO> ListMmmVersionCombo(DateTime fecha)
        {
            List<MmmVersionDTO> l = FactorySic.GetMmmVersionRepository().List();
            List<MmmVersionDTO> lista = l.Where(x => x.Vermmfechaperiodo == fecha && x.Vermmporcentaje == 100).OrderByDescending(x => x.Vermmcodi).ToList();
            List<MmmVersionDTO> listaFinal = new List<MmmVersionDTO>();

            int recorrido = 1;
            foreach (var recorridoVersion in lista)
            {
                if (recorrido < lista.Count)
                {
                    listaFinal.Add(recorridoVersion);
                }

                recorrido = recorrido + 1;
            }

            return listaFinal;
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla MmmVersion
        /// </summary>
        public List<MmmVersionDTO> GetByCriteriaMmmVersions()
        {
            return FactorySic.GetMmmVersionRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla MMM_DATO

        /// <summary>
        /// Inserta un registro de la tabla MMM_FACTABLE
        /// </summary>
        /// <param name="entity"></param>
        public void SaveMmmDato(MmmDatoDTO entity)
        {
            try
            {
                FactorySic.GetMmmDatoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla MMM_FACTABLE
        /// </summary>
        public void UpdateMmmDato(MmmDatoDTO entity)
        {
            try
            {
                FactorySic.GetMmmDatoRepository().Update(entity);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla MMM_FACTABLE
        /// </summary>
        public void DeleteMmmDato(int Mmmdatocodi)
        {
            try
            {
                FactorySic.GetMmmDatoRepository().Delete(Mmmdatocodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla MMM_FACTABLE
        /// </summary>
        public MmmDatoDTO GetByIdMmmDato(int Mmmdatocodi)
        {
            return FactorySic.GetMmmDatoRepository().GetById(Mmmdatocodi);
        }

        #endregion

        #region Métodos Tabla MMM_CAMBIOVERSION

        /// <summary>
        /// Inserta un registro de la tabla MMM_CAMBIOVERSION
        /// </summary>
        public void SaveMmmCambioversion(MmmCambioversionDTO entity)
        {
            try
            {
                FactorySic.GetMmmCambioversionRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla MMM_CAMBIOVERSION
        /// </summary>
        public void UpdateMmmCambioversion(MmmCambioversionDTO entity)
        {
            try
            {
                FactorySic.GetMmmCambioversionRepository().Update(entity);
            }
            catch (Exception ex)
            {
                // Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla MMM_CAMBIOVERSION
        /// </summary>
        public List<MmmCambioversionDTO> ListMmmCambioversionsByPeriodo(DateTime fechaPeriodo)
        {
            return FactorySic.GetMmmCambioversionRepository().ListByPeriodo(fechaPeriodo);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla MmmCambioversion
        /// </summary>
        public List<MmmCambioversionDTO> GetByCriteriaMmmCambioversions(int vermmcodi)
        {
            return FactorySic.GetMmmCambioversionRepository().GetByCriteria(vermmcodi);
        }

        #endregion

        #region Métodos Tabla PR_GRUPOXCNFBAR

        /// <summary>
        /// Inserta un registro de la tabla PR_GRUPOXCNFBAR
        /// </summary>
        public void SavePrGrupoxcnfbar(PrGrupoxcnfbarDTO entity)
        {
            try
            {
                FactorySic.GetPrGrupoxcnfbarRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PR_GRUPOXCNFBAR
        /// </summary>
        public void UpdatePrGrupoxcnfbar(PrGrupoxcnfbarDTO entity)
        {
            try
            {
                FactorySic.GetPrGrupoxcnfbarRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PR_GRUPOXCNFBAR
        /// </summary>
        public void DeletePrGrupoxcnfbar(PrGrupoxcnfbarDTO entity)
        {
            try
            {
                FactorySic.GetPrGrupoxcnfbarRepository().Delete(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PR_GRUPOXCNFBAR
        /// </summary>
        public PrGrupoxcnfbarDTO GetByIdPrGrupoxcnfbar(int grcnfbcodi)
        {
            return FactorySic.GetPrGrupoxcnfbarRepository().GetById(grcnfbcodi);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PR_GRUPOXCNFBAR
        /// </summary>
        public PrGrupoxcnfbarDTO GetByGrupocodiPrGrupoxcnfbar(int grupocodi)
        {
            return FactorySic.GetPrGrupoxcnfbarRepository().GetByGrupocodi(grupocodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PR_GRUPOXCNFBAR
        /// </summary>
        public List<PrGrupoxcnfbarDTO> ListPrGrupoxcnfbars()
        {
            return FactorySic.GetPrGrupoxcnfbarRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PrGrupoxcnfbar
        /// </summary>
        public List<PrGrupoxcnfbarDTO> GetByCriteriaPrGrupoxcnfbars()
        {
            return FactorySic.GetPrGrupoxcnfbarRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla PR_GRUPO

        /// <summary>
        /// Obtener grupo
        /// </summary>
        /// <param name="iGrupo"></param>
        /// <returns></returns>
        public PrGrupoDTO ObjGrupoUnidad(int iGrupo)
        {
            return FactorySic.GetPrGrupoRepository().GetById(iGrupo);
        }

        #endregion

        #region Métodos Tabla TRN_BARRA

        /// <summary>
        /// Permite listar todas las barras generadoras de la tabla trn_barra
        /// </summary>
        /// <returns>Lista de BarraDTO</returns>
        public List<BarraDTO> ListarGrupoBarra()
        {
            return FactoryTransferencia.GetBarraRepository().ListarGrupoBarraEjec();
        }

        /// <summary>
        /// Lista de barra por empresa y fecha de consulta
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="fechaIni"></param>
        /// <returns></returns>
        public List<BarraDTO> ListarBarraPorEmpresa(string empresas, string fechaIni)
        {
            // DateTime fecha = new DateTime(Int32.Parse(fechaIni.Substring(3, 4)), Int32.Parse(fechaIni.Substring(0, 2)), 1);

            DateTime fecha = new DateTime(Int32.Parse(fechaIni.Substring(6, 4)), Int32.Parse(fechaIni.Substring(3, 2)), Int32.Parse(fechaIni.Substring(0, 2)));

            List<SiEmpresaDTO> listaEmpresa = this.ListarEmpresasMonitoreo(fecha, empresas);

            List<BarraDTO> listaBarra = new List<BarraDTO>();

            foreach (var empresa in listaEmpresa)
            {
                listaBarra.AddRange(ListarGrupoBarra().Where(x => x.Emprcodi == empresa.Emprcodi).Distinct().ToList());
            }
            return listaBarra;
        }

        #endregion

        #region Generación de Versión

        /// <summary>
        /// Creacion de nueva generacion proceso en segundo plano
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="usuario"></param>
        public void GenerarVersionIndicadoresMME(DateTime fechaInicio, DateTime fechaFin, MmmVersionDTO regVersion, string usuario)
        {
            // Lista de versiones
            DateTime fechaPeriodo = new DateTime(fechaInicio.Year, fechaInicio.Month, 1).Date;
            var cantidadVersion = this.ListMmmVersions().Where(x => x.Vermmporcentaje != -1 && x.Vermmfechaperiodo.Date == fechaPeriodo).ToList();
            //Declaraciones de listas
            var listaOrigen = new List<MmmCambioversionDTO>();
            var listaOrigenProgramado = new List<MmmCambioversionDTO>();
            var listaOrigenMarginalEjec = new List<MmmCambioversionDTO>();
            var listaOrigenMarginalProg = new List<MmmCambioversionDTO>();
            var listaOrigenCostoVariable = new List<MmmCambioversionDTO>();
            var listaOrigenEmpresa = new List<MmmCambioversionDTO>();
            var listafactableSave = new List<MmmDatoDTO>();
            var listaCambioVersion = new List<MmmCambioversionDTO>();
            List<MmmDatoDTO> listNuevos = new List<MmmDatoDTO>();
            List<MmmDatoDTO> listSaveTmp = new List<MmmDatoDTO>(); //Save grupocodi no existentes
            List<MmmDatoDTO> listUpdateTmp = new List<MmmDatoDTO>(); //Save grupocodi no existentes

            //Lista mmm_Factable
            List<MmmDatoDTO> listFacTableMes = FactorySic.GetMmmDatoRepository().ListPeriodo(fechaInicio, fechaFin.AddDays(1));
            int maxId = FactorySic.GetMmmDatoRepository().MaxSidFacTable();
            //Costos marginales
            List<CostoMarginalDTO> listCostoMarginalesEjec = this.ListarCostosMarginales(fechaInicio);
            List<CmCostomarginalprogDTO> listCostoMarginalProg = this.ListCostoMarginalProg(fechaInicio);
            //Despacho
            List<MeMedicion48DTO> listMedi48 = this.ListarIMedicion48(fechaInicio, fechaFin);
            //Costos Variables
            List<MeMedicion48DTO> listaModo30min; List<MeMedicion48DTO> listaCV30min;
            this.ListarCostoVariables(fechaInicio, fechaFin, listMedi48, out listaModo30min, out listaCV30min);

            #region Generación de Versión
            //Recorrido de fecha
            int diaRecorrido = 1;
            decimal porcentajeGlobal = 0;

            for (var day = fechaInicio; day.Date <= fechaFin.Date; day = day.AddDays(1))
            {
                diaRecorrido = day.Day;
                List<MmmDatoDTO> listFacTableByDia = listFacTableMes.Where(x => x.Mmmdatfecha >= day.AddMinutes(30) && x.Mmmdatfecha <= day.AddDays(1)).ToList();

                int diaTotalFact = fechaFin.Day;
                porcentajeGlobal = 50 * (diaRecorrido / (diaTotalFact + 0.0m));
                regVersion.Vermmporcentaje = porcentajeGlobal;
                this.UpdateMmmVersionPorcentaje(regVersion);

                //Lista de datos Mw  Costo Marginal
                var listMedi48Ejecutado = listMedi48.Where(x => x.Emprnomb != null && x.Medifecha == day && x.Lectcodi == ConstantesPR5ReportesServicio.LectDespachoEjecutadoHisto).ToList().OrderBy(x => x.Emprnomb).ToList();
                var listMedi48Programado = listMedi48.Where(x => x.Emprnomb != null && x.Medifecha == day && x.Lectcodi == ConstantesPR5ReportesServicio.LectDespachoProgramadoDiario).ToList().OrderBy(x => x.Emprnomb).ToList();
                var listaGrupoDia = listMedi48.Where(x => x.Medifecha == day).Select(x => new { x.Grupocodi, x.Grupopadre })
                    .GroupBy(x => new { x.Grupocodi, x.Grupopadre }).Select(x => new { Grupocodi = x.Key.Grupocodi, Grupopadre = x.Key.Grupopadre }).Where(x => x.Grupopadre > 0).OrderBy(x => x.Grupocodi).ToList();
                var listGrupocodimodo = listaModo30min.Where(x => x.Medifecha == day).ToList();
                var listCVByGrupoDespacho = listaCV30min.Where(x => x.Medifecha == day).ToList();

                // Recorrido de grupos 
                foreach (var regGrupo in listaGrupoDia)
                {
                    List<MmmDatoDTO> listFacTableByDiaAndGrupo = listFacTableByDia.Where(x => x.Grupocodi == regGrupo.Grupocodi).OrderBy(x => x.Mmmdathoraindex).ToList();
                    var obj48PotEjec = listMedi48Ejecutado.Find(x => x.Grupocodi == regGrupo.Grupocodi);
                    var obj48PotProg = listMedi48Programado.Find(x => x.Grupocodi == regGrupo.Grupocodi);
                    var obj48Modogrupocodi = listGrupocodimodo.Find(x => x.Grupocodi == regGrupo.Grupocodi);
                    var obj48CV = listCVByGrupoDespacho.Find(x => x.Grupocodi == regGrupo.Grupocodi);
                    var objCostoMar = listCostoMarginalesEjec.Find(x => x.GrupoCodi == regGrupo.Grupocodi && x.CosMarDia == diaRecorrido);

                    DateTime horas = day.AddMinutes(30);
                    for (int i = 1; i <= 48; i++)
                    {
                        var regFactable = listFacTableByDiaAndGrupo.Find(x => x.Mmmdathoraindex == i);
                        if (regFactable == null)
                        {
                            MmmDatoDTO regFac = new MmmDatoDTO();
                            decimal? valorOrigenProgramado = null;
                            decimal? valorOrigenEjectuado = null;
                            decimal? valorCostMarEjec = null;
                            decimal? valorCostMarProg = null;
                            decimal? valorCostVariable = null;
                            int? mogrupocodi = null;

                            if (obj48PotEjec != null)
                            {
                                valorOrigenEjectuado = (decimal?)obj48PotEjec.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(obj48PotEjec, null);
                            }

                            if (obj48PotProg != null)
                            {
                                valorOrigenProgramado = (decimal?)obj48PotProg.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(obj48PotProg, null);
                            }

                            if (objCostoMar != null)
                            {
                                valorCostMarEjec = (decimal?)objCostoMar.GetType().GetProperty(ConstantesAppServicio.CaracterCosMar + (i * 2)).GetValue(objCostoMar, null);
                                regFac.Barrcodi = objCostoMar.BarrCodi;
                            }

                            var listCostoMarProg = listCostoMarginalProg.Find(x => (x.Grupocodi == regGrupo.Grupocodi || (x.Grupocodi == regGrupo.Grupopadre && regGrupo.Grupopadre > 0)) && x.Cmarprfecha == horas);
                            if (listCostoMarProg != null)
                            {
                                valorCostMarProg = listCostoMarProg.Cmarprtotal;
                                regFac.Cnfbarcodi = listCostoMarProg.Cnfbarcodi;
                            }
                            if (obj48CV != null)
                            {
                                valorCostVariable = (decimal?)obj48CV.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(obj48CV, null);
                            }
                            if (obj48Modogrupocodi != null)
                            {
                                decimal? mogrupocodi48 = (decimal?)obj48Modogrupocodi.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(obj48Modogrupocodi, null);
                                if (mogrupocodi48 != null)
                                {
                                    mogrupocodi = Convert.ToInt32(mogrupocodi48.GetValueOrDefault(0));
                                }
                            }

                            regFac.Mmmdatcodi = maxId++;
                            regFac.Mmmdatfecha = horas;
                            regFac.Mmmdathoraindex = i;
                            regFac.Grupocodi = regGrupo.Grupocodi;
                            regFac.Mmmdatmwejec = valorOrigenEjectuado;
                            regFac.Mmmdatmwprog = valorOrigenProgramado;
                            regFac.Mmmdatcmgejec = valorCostMarEjec;
                            regFac.Mmmdatcmgprog = valorCostMarProg;
                            regFac.Mmmdatocvar = valorCostVariable;
                            regFac.Mogrupocodi = mogrupocodi;

                            if (regFac.Barrcodi == 0)
                            {
                                regFac.Barrcodi = null;
                            }
                            if (obj48PotEjec != null)
                            {
                                regFac.Emprcodi = obj48PotEjec.Emprcodi;
                            }
                            else if (obj48PotProg != null)
                            {
                                regFac.Emprcodi = obj48PotProg.Emprcodi;
                            }

                            listSaveTmp.Add(regFac);

                            horas = horas.AddMinutes(30);
                        }
                    }
                }
                // Si hubieran  nuevos grupos se insertar en la mm_factable
                if (listSaveTmp.Count >= 1)
                {
                    MmmDatoDTO reg = new MmmDatoDTO();
                    for (int i = 0; i < listSaveTmp.Count; i++)
                    {
                        reg = listSaveTmp[i];
                        FactorySic.GetMmmDatoRepository().Save(reg);
                    }
                }
                listNuevos.AddRange(listSaveTmp);
                listSaveTmp.Clear();
                diaRecorrido = diaRecorrido + 1;
            }
            listFacTableMes.AddRange(listNuevos);
            listFacTableMes = listFacTableMes.OrderBy(x => x.Mmmdatfecha).ThenBy(x => x.Grupocodi).ToList();

            #endregion

            int dia = 1;
            int diaTotal = fechaFin.Day;

            #region Verificación de Cambios entre versión

            regVersion.Vermmporcentaje = 50;
            this.UpdateMmmVersionPorcentaje(regVersion);

            //  hay mas de una version ingresa 
            DateTime fechaCambio = DateTime.Now;
            if (cantidadVersion.Count > 1)
            {
                List<MmmCambioversionDTO> listCambios = this.ListMmmCambioversionsByPeriodo(fechaPeriodo);

                //  Recorrido de todo el periodo si encuentra cambios
                for (var day = fechaInicio.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
                {
                    diaRecorrido = day.Day;
                    List<MmmDatoDTO> listFacTableByDia = listFacTableMes.Where(x => x.Mmmdatfecha >= day.AddMinutes(30) && x.Mmmdatfecha <= day.AddDays(1)).ToList();

                    //Actualizar porcentaje de Generacion
                    int diaTotalFact = fechaFin.Day;
                    porcentajeGlobal = 50 + (25 * (diaRecorrido / (diaTotalFact + 0.0m)));
                    regVersion.Vermmporcentaje = porcentajeGlobal;
                    this.UpdateMmmVersionPorcentaje(regVersion);

                    // Medicion 48 Ejecutado y programado
                    var list48Ejec = listMedi48.Where(x => x.Emprnomb != null && x.Medifecha == day && x.Lectcodi == ConstantesPR5ReportesServicio.LectDespachoEjecutadoHisto).OrderBy(x => x.Emprnomb).ToList();
                    var list48Prog = listMedi48.Where(x => x.Emprnomb != null && x.Medifecha == day && x.Lectcodi == ConstantesPR5ReportesServicio.LectDespachoProgramadoDiario).OrderBy(x => x.Emprnomb).ToList();
                    var listGrupocodimodo = listaModo30min.Where(x => x.Medifecha == day).ToList();
                    var listCVByGrupoDespacho = listaCV30min.Where(x => x.Medifecha == day).ToList();
                    var listCostoMarginalProgDia = listCostoMarginalProg.Where(x => x.Cmarprfecha >= day.AddMinutes(30) && x.Cmarprfecha <= day.AddDays(1)).ToList();
                    var listCostoMarDia = listCostoMarginalesEjec.Where(x => x.CosMarDia == diaRecorrido).ToList();

                    //MW EJECUTADO 
                    List<PrGrupoxcnfbarDTO> valor = this.ListPrGrupoxcnfbars();

                    var listaGrupoDiaNuevo = listMedi48.Where(x => x.Medifecha == day).Select(x => new { x.Grupocodi, x.Grupopadre }).ToList();
                    var listaGrupoDiaActual = listFacTableByDia.Select(x => new { x.Grupocodi, x.Grupopadre }).ToList();
                    listaGrupoDiaActual.AddRange(listaGrupoDiaNuevo);

                    var listaGrupoDia = listaGrupoDiaActual.GroupBy(x => new { x.Grupocodi, x.Grupopadre })
                        .Select(x => new { Grupocodi = x.Key.Grupocodi, Grupopadre = x.Key.Grupopadre }).Where(x => x.Grupopadre > 0).OrderBy(x => x.Grupocodi).ToList();

                    foreach (var regGrupo in listaGrupoDia)
                    {
                        if (regGrupo.Grupocodi == 417)
                        {
                        }

                        List<MmmDatoDTO> listFacTableByDiaAndGrupo = listFacTableByDia.Where(x => x.Grupocodi == regGrupo.Grupocodi).OrderBy(x => x.Mmmdathoraindex).ToList();

                        //Declariaciones de lista de datos de Monitoreo
                        decimal? valorNuevoMmmdatomwejec = null;
                        decimal? valorNuevoMmmdatomwprog = null;
                        decimal? valorNuevoCostMarEjec = null;
                        decimal? valorNuevoCostVariable = null;
                        decimal? valorNuevoCostMarProg = null;

                        var obj48Ejec = list48Ejec.Find(x => x.Grupocodi == regGrupo.Grupocodi);
                        var obj48Prog = list48Prog.Find(x => x.Grupocodi == regGrupo.Grupocodi);
                        var objCostoMar = listCostoMarginalesEjec.Find(x => x.GrupoCodi == regGrupo.Grupocodi && x.CosMarDia == dia);
                        var obj48Modogrupocodi = listGrupocodimodo.Find(x => x.Grupocodi == regGrupo.Grupocodi);
                        var obj48CV = listCVByGrupoDespacho.Find(x => x.Grupocodi == regGrupo.Grupocodi);
                        //Empresa
                        int valorNuevoEmprcodi = -1;
                        if (obj48Ejec != null)
                        {
                            valorNuevoEmprcodi = obj48Ejec.Emprcodi;
                        }
                        else if (obj48Prog != null)
                        {
                            valorNuevoEmprcodi = obj48Prog.Emprcodi;
                        }
                        //Barra programada
                        var barraProgNueva = listCostoMarginalProgDia.Find(x => (x.Grupocodi == regGrupo.Grupocodi || (x.Grupocodi == regGrupo.Grupopadre && regGrupo.Grupopadre > 0)));
                        int? valorNuevoCnfbarcodi = null;
                        if (barraProgNueva != null)
                        {
                            valorNuevoCnfbarcodi = barraProgNueva.Cnfbarcodi;
                        }
                        //Barra ejecutada
                        var barraNueva = listCostoMarDia.Find(x => x.GrupoCodi == regGrupo.Grupocodi);
                        int? valorNuevoBarrcodi = null;
                        if (barraNueva != null)
                        {
                            valorNuevoBarrcodi = barraNueva.BarrCodi;
                        }
                        //Modo de Operación
                        int? valorNuevoMogrupocodi = null;

                        DateTime horas = day.AddMinutes(30);
                        for (int i = 1; i <= 48; i++)
                        {
                            if (i == 28)
                            {
                            }
                            var regFactable = listFacTableByDiaAndGrupo.Find(x => x.Mmmdathoraindex == i);
                            valorNuevoMmmdatomwejec = null;
                            valorNuevoMmmdatomwprog = null;
                            valorNuevoCostMarEjec = null;
                            valorNuevoCostVariable = null;
                            valorNuevoCostMarProg = null;
                            valorNuevoMogrupocodi = null;

                            // Verifica cambio Empresa
                            // Obtiene Mw Ejecutado de Medicion 48
                            if (obj48Ejec != null)
                            {
                                valorNuevoMmmdatomwejec = (decimal?)obj48Ejec.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(obj48Ejec, null);
                            }
                            // Obtiene Mw Programado de Medicion 48
                            if (obj48Prog != null)
                            {
                                valorNuevoMmmdatomwprog = (decimal?)obj48Prog.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(obj48Prog, null);
                            }
                            // Obtiene Costo Marginal
                            if (objCostoMar != null)
                            {
                                valorNuevoCostMarEjec = (decimal?)objCostoMar.GetType().GetProperty(ConstantesAppServicio.CaracterCosMar + (i * 2)).GetValue(objCostoMar, null);
                            }
                            // Obtiene Costo Variable 
                            if (obj48CV != null)
                            {
                                valorNuevoCostVariable = (decimal?)obj48CV.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(obj48CV, null);
                            }
                            // Obtiene Costo Programado
                            var listCostoMarProg = listCostoMarginalProg.Find(x => (x.Grupocodi == regGrupo.Grupocodi || (x.Grupocodi == regGrupo.Grupopadre && regGrupo.Grupopadre > 0)) && x.Cmarprfecha == horas);
                            if (listCostoMarProg != null)
                            {
                                valorNuevoCostMarProg = listCostoMarProg.Cmarprtotal;
                            }
                            // Obtiene Modo de Operación
                            if (obj48Modogrupocodi != null)
                            {
                                decimal? valorMOH = (decimal?)obj48Modogrupocodi.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(obj48Modogrupocodi, null);
                                if (valorMOH.GetValueOrDefault(0) > 0)
                                {
                                    valorNuevoMogrupocodi = Convert.ToInt32(valorMOH.GetValueOrDefault(0));
                                }
                            }

                            if (regFactable != null)
                            {
                                int recorrido = 0;
                                MmmDatoDTO regFac = new MmmDatoDTO();
                                decimal? valorAntEjec = regFactable.Mmmdatmwejec;
                                decimal? valorAntProg = regFactable.Mmmdatmwprog;
                                decimal? valorAntCostMar = regFactable.Mmmdatcmgejec;
                                decimal? valorAntCosProg = regFactable.Mmmdatcmgprog;
                                decimal? valorAntCostV = regFactable.Mmmdatocvar;
                                int valorAntEmprcodi = regFactable.Emprcodi;
                                int? valorAntBarrcodi = regFactable.Barrcodi;
                                int? valorAntCnfbarcodi = regFactable.Cnfbarcodi;
                                int? valorAntMogrupocodi = regFactable.Mogrupocodi;


                                if (valorNuevoEmprcodi != regFactable.Emprcodi)
                                {
                                    foreach (var objVersion in cantidadVersion)
                                    {
                                        var resultado = listCambios.Find(x => x.Vermmcodi == objVersion.Vermmcodi && x.Camvertipo == ConstantesMonitoreo.CambioEmprcodi && x.Mmmdatcodi == regFactable.Mmmdatcodi);
                                        if (objVersion.Vermmcodi != regVersion.Vermmcodi)
                                        {
                                            if (resultado == null)
                                            {
                                                listaOrigen.Add(this.CreateCambioversion(objVersion.Vermmcodi, ConstantesMonitoreo.CambioEmprcodi, regFactable.Mmmdatcodi, regFactable.Emprcodi, fechaCambio, usuario));
                                            }
                                        }
                                    }
                                    recorrido = recorrido + 1;
                                }

                                // Verifica cambio Mw ejecutado 
                                if (valorNuevoMmmdatomwejec != valorAntEjec)
                                {
                                    foreach (var objVersion in cantidadVersion)
                                    {
                                        var resultado = listCambios.Find(x => x.Vermmcodi == objVersion.Vermmcodi && x.Camvertipo == ConstantesMonitoreo.CambioEmprcodi && x.Mmmdatcodi == regFactable.Mmmdatcodi);
                                        if (objVersion.Vermmcodi != regVersion.Vermmcodi)
                                        {
                                            if (resultado == null)
                                            {
                                                listaOrigen.Add(this.CreateCambioversion(objVersion.Vermmcodi, ConstantesMonitoreo.CambioMmmdatomwejec, regFactable.Mmmdatcodi, regFactable.Mmmdatmwejec, fechaCambio, usuario));
                                            }
                                        }
                                    }
                                    recorrido = recorrido + 1;
                                }
                                // Verifica cambio Mw Programado 
                                if (valorNuevoMmmdatomwprog != valorAntProg)
                                {
                                    foreach (var objVersion in cantidadVersion)
                                    {
                                        var resultado = listCambios.Find(x => x.Vermmcodi == objVersion.Vermmcodi && x.Camvertipo == ConstantesMonitoreo.CambioMmmdatomwprog && x.Mmmdatcodi == regFactable.Mmmdatcodi);
                                        if (objVersion.Vermmcodi != regVersion.Vermmcodi)
                                        {
                                            if (resultado == null)
                                            {
                                                listaOrigen.Add(this.CreateCambioversion(objVersion.Vermmcodi, ConstantesMonitoreo.CambioMmmdatomwprog, regFactable.Mmmdatcodi, regFactable.Mmmdatmwejec, fechaCambio, usuario));
                                            }
                                        }
                                    }
                                    recorrido = recorrido + 1;
                                }
                                // Verifica cambio Costo Marginal Ejecutado

                                if (valorNuevoCostMarEjec != valorAntCostMar)
                                {
                                    foreach (var objVersion in cantidadVersion)
                                    {
                                        var resultado = listCambios.Find(x => x.Vermmcodi == objVersion.Vermmcodi && x.Camvertipo == ConstantesMonitoreo.CambioMmmdatocmgejec && x.Mmmdatcodi == regFactable.Mmmdatcodi);
                                        if (objVersion.Vermmcodi != regVersion.Vermmcodi)
                                        {
                                            if (resultado == null)
                                            {
                                                listaOrigen.Add(this.CreateCambioversion(objVersion.Vermmcodi, ConstantesMonitoreo.CambioMmmdatocmgejec, regFactable.Mmmdatcodi, regFactable.Mmmdatcmgejec, fechaCambio, usuario));
                                            }
                                        }
                                    }
                                    recorrido = recorrido + 1;
                                }
                                // Verifica cambio Costo Marginado Programado 

                                if (valorNuevoCostMarProg != valorAntCosProg)
                                {
                                    foreach (var objVersion in cantidadVersion)
                                    {
                                        var resultado = listCambios.Find(x => x.Vermmcodi == objVersion.Vermmcodi && x.Camvertipo == ConstantesMonitoreo.CambioMmmdatocmgprog && x.Mmmdatcodi == regFactable.Mmmdatcodi);
                                        if (objVersion.Vermmcodi != regVersion.Vermmcodi)
                                        {
                                            if (resultado == null)
                                            {
                                                listaOrigen.Add(this.CreateCambioversion(objVersion.Vermmcodi, ConstantesMonitoreo.CambioMmmdatocmgprog, regFactable.Mmmdatcodi, regFactable.Mmmdatmwprog, fechaCambio, usuario));
                                            }
                                        }
                                    }
                                    recorrido = recorrido + 1;
                                }
                                // Verifica cambio Costo Variable 
                                if (valorNuevoCostVariable != valorAntCostV)
                                {
                                    foreach (var objVersion in cantidadVersion)
                                    {
                                        var resultado = listCambios.Find(x => x.Vermmcodi == objVersion.Vermmcodi && x.Camvertipo == ConstantesMonitoreo.CambioCvar && x.Mmmdatcodi == regFactable.Mmmdatcodi);
                                        if (objVersion.Vermmcodi != regVersion.Vermmcodi)
                                        {
                                            if (resultado == null)
                                            {
                                                listaOrigen.Add(this.CreateCambioversion(objVersion.Vermmcodi, ConstantesMonitoreo.CambioCvar, regFactable.Mmmdatcodi, regFactable.Mmmdatocvar, fechaCambio, usuario));
                                            }
                                        }
                                    }
                                    recorrido = recorrido + 1;
                                }

                                // Verifica cambio Modo de operación
                                if (valorNuevoMogrupocodi != valorAntMogrupocodi)
                                {
                                    foreach (var objVersion in cantidadVersion)
                                    {
                                        var resultado = listCambios.Find(x => x.Vermmcodi == objVersion.Vermmcodi && x.Camvertipo == ConstantesMonitoreo.CambioMogrupocodi && x.Mmmdatcodi == regFactable.Mmmdatcodi);
                                        if (objVersion.Vermmcodi != regVersion.Vermmcodi)
                                        {
                                            if (resultado == null)
                                            {
                                                listaOrigen.Add(this.CreateCambioversion(objVersion.Vermmcodi, ConstantesMonitoreo.CambioMogrupocodi, regFactable.Mmmdatcodi, regFactable.Mogrupocodi, fechaCambio, usuario));
                                            }
                                        }
                                    }
                                    recorrido = recorrido + 1;
                                }

                                // Verifica cambio Barra Ejecutada
                                if (valorNuevoBarrcodi != valorAntBarrcodi)
                                {
                                    foreach (var objVersion in cantidadVersion)
                                    {
                                        var resultado = listCambios.Find(x => x.Vermmcodi == objVersion.Vermmcodi && x.Camvertipo == ConstantesMonitoreo.CambioBarrcodi && x.Mmmdatcodi == regFactable.Mmmdatcodi);
                                        if (objVersion.Vermmcodi != regVersion.Vermmcodi)
                                        {
                                            if (resultado == null)
                                            {
                                                listaOrigen.Add(this.CreateCambioversion(objVersion.Vermmcodi, ConstantesMonitoreo.CambioBarrcodi, regFactable.Mmmdatcodi, regFactable.Barrcodi, fechaCambio, usuario));
                                            }
                                        }
                                    }
                                    recorrido = recorrido + 1;
                                }
                                // Verifica cambio Barra de Programación
                                if (valorNuevoCnfbarcodi != valorAntCnfbarcodi)
                                {
                                    foreach (var objVersion in cantidadVersion)
                                    {
                                        var resultado = listCambios.Find(x => x.Vermmcodi == objVersion.Vermmcodi && x.Camvertipo == ConstantesMonitoreo.CambioCnfbarcodi && x.Mmmdatcodi == regFactable.Mmmdatcodi);
                                        if (objVersion.Vermmcodi != regVersion.Vermmcodi)
                                        {
                                            if (resultado == null)
                                            {
                                                listaOrigen.Add(this.CreateCambioversion(objVersion.Vermmcodi, ConstantesMonitoreo.CambioCnfbarcodi, regFactable.Mmmdatcodi, regFactable.Cnfbarcodi, fechaCambio, usuario));
                                            }
                                        }
                                    }
                                    recorrido = recorrido + 1;
                                }
                                // Si existe algun cambio se agrega en la lista
                                if (recorrido >= 1)
                                {
                                    regFactable.Emprcodi = valorNuevoEmprcodi;
                                    regFactable.Barrcodi = valorNuevoBarrcodi;
                                    regFactable.Cnfbarcodi = valorNuevoCnfbarcodi;
                                    regFactable.Mogrupocodi = valorNuevoMogrupocodi;
                                    regFactable.Mmmdatmwejec = valorNuevoMmmdatomwejec;
                                    regFactable.Mmmdatmwprog = valorNuevoMmmdatomwprog;
                                    regFactable.Mmmdatcmgejec = valorNuevoCostMarEjec;
                                    regFactable.Mmmdatcmgprog = valorNuevoCostMarProg;
                                    regFactable.Mmmdatocvar = valorNuevoCostVariable;

                                    listUpdateTmp.Add(regFactable);
                                }
                            }
                            horas = horas.AddMinutes(30);
                        }
                    }

                    if (listaOrigen.Count >= 1)
                    {
                        foreach (var entity in listaOrigen)
                            this.SaveMmmCambioversion(entity);
                    }

                    // Actualiza en la factable
                    foreach (var entity in listUpdateTmp)
                        this.UpdateMmmDato(entity);

                    listaOrigen.Clear();
                    listUpdateTmp.Clear();
                    dia = dia + 1;
                }
            }
            #endregion
            //Generación de Excel
            regVersion.Vermmporcentaje = 75;
            this.UpdateMmmVersionPorcentaje(regVersion);
            this.GeneracionIndicadoresExcel(fechaInicio, fechaFin, listFacTableMes, regVersion, ConstantesMonitoreo.ReportesIndicadores);

            //Actualizar generación
            regVersion.Vermmporcentaje = 100;
            regVersion.Vermmfechageneracion = DateTime.Now;
            this.UpdateMmmVersionPorcentaje(regVersion);
        }

        /// <summary>
        /// Objeto de cambio entre versiones
        /// </summary>
        /// <param name="vermmcodi"></param>
        /// <param name="tipoCambio"></param>
        /// <param name="Mmmdatocodi"></param>
        /// <param name="valor"></param>
        /// <param name="fechaCambio"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public MmmCambioversionDTO CreateCambioversion(int vermmcodi, int tipoCambio, int Mmmdatocodi, decimal? valor, DateTime fechaCambio, string usuario)
        {
            MmmCambioversionDTO cambio = new MmmCambioversionDTO();
            cambio.Vermmcodi = vermmcodi;
            cambio.Camvertipo = tipoCambio;
            cambio.Camverfeccreacion = fechaCambio;
            cambio.Camverusucreacion = usuario;
            cambio.Camvervalor = valor;
            cambio.Mmmdatcodi = Mmmdatocodi;

            return cambio;
        }

        /// <summary>
        /// Lista Mw Ejecutados y Programados de medicion 48
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ListarIMedicion48(DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion48DTO> listaFinal = new List<MeMedicion48DTO>();

            // Obtiene Mw Ejecutados 
            List<MeMedicion48DTO> listaPotXDia = this.servEjec.ListaDataGeneracion48(fechaInicio, fechaFin, ConstantesMedicion.IdTipogrupoCOES
                , ConstantesMedicion.IdTipoGeneracionTodos.ToString(), ConstantesMedicion.IdEmpresaTodos.ToString(), ConstanteValidacion.EstadoTodos
                , ConstantesMedicion.IdTipoRecursoTodos.ToString(), false, ConstantesTipoInformacion.TipoinfoMW, ConstantesPR5ReportesServicio.LectDespachoEjecutadoHisto);

            listaPotXDia = listaPotXDia.Where(x => x.Tipogrupocodi != ConstantesMedicion.IdTipogrupoNoIntegrante).ToList();

            // Obtiene Mw Programados 
            List<MeMedicion48DTO> listaPotXDiaProgramado = this.servEjec.ListaDataGeneracion48(fechaInicio, fechaFin, ConstantesMedicion.IdTipogrupoCOES
               , ConstantesMedicion.IdTipoGeneracionTodos.ToString(), ConstantesMedicion.IdEmpresaTodos.ToString(), ConstanteValidacion.EstadoTodos
               , ConstantesMedicion.IdTipoRecursoTodos.ToString(), false, ConstantesTipoInformacion.TipoinfoMW, ConstantesPR5ReportesServicio.LectDespachoProgramadoDiario);
            listaPotXDiaProgramado = listaPotXDiaProgramado.Where(x => x.Tipogrupocodi != ConstantesMedicion.IdTipogrupoNoIntegrante).ToList();

            listaFinal.AddRange(listaPotXDia);
            listaFinal.AddRange(listaPotXDiaProgramado);
            return listaFinal;
        }

        /// <summary>
        /// Devuelve una lista de costo marginales ejecutados
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <returns></returns>
        public List<CostoMarginalDTO> ListarCostosMarginales(DateTime fechaInicio)
        {
            List<CostoMarginalDTO> listCostoMarinal = new List<CostoMarginalDTO>();

            int aniomes = Convert.ToInt32(fechaInicio.ToString(ConstantesBase.FormatoAnioMes));
            PeriodoDTO periodo = FactoryTransferencia.GetPeriodoRepository().GetByAnioMes(aniomes);
            if (periodo != null)
            {
                int versionRecalculo = FactoryTransferencia.GetRecalculoRepository().GetUltimaVersion(periodo.PeriCodi);
                listCostoMarinal = FactoryTransferencia.GetCostoMarginalRepository().ListCostoMarginalWithGrupo(periodo.PeriCodi, versionRecalculo);
            }

            return listCostoMarinal;
        }

        /// <summary>
        /// Listar Costos variables
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public void ListarCostoVariables(DateTime fechaIni, DateTime fechaFin, List<MeMedicion48DTO> listaM48
            , out List<MeMedicion48DTO> listaModo30minOut, out List<MeMedicion48DTO> listaCV30minOut)
        {
            List<MeMedicion48DTO> listaModo30min = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaCV30min = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaM48Ejecutado = listaM48.Where(x => x.Lectcodi == ConstantesPR5ReportesServicio.LectDespachoEjecutadoHisto).ToList();

            List<PrCvariablesDTO> ListCostoVariables = FactorySic.GetPrCvariablesRepository().ListCostoVariablesxRangoFecha(fechaIni.AddDays(-7), fechaFin);

            List<PrGrupoDTO> listaGrupoGeneracion = this.servHO.ListarAllGrupoGeneracion(fechaIni, "'S'");
            List<PrGrupoDTO> listaModoOpTotal = this.servHO.ListarModoOperacionXCentralYEmpresa(-2, -2);
            List<PrGrupoDTO> listaUnidadTermo = this.servHO.ListarAllUnidadTermoelectrica();

            List<EveHoraoperacionDTO> listaHOPTotal = this.servHO.ListarHorasOperacionByCriteria(fechaIni, fechaFin.AddDays(1), ConstantesHorasOperacion.ParamEmpresaTodos, ConstantesHorasOperacion.ParamCentralTodos, ConstantesHorasOperacion.TipoListadoTodo);
            listaHOPTotal = this.servHO.CompletarListaHoraOperacionTermo(listaHOPTotal);

            decimal? valorCV;
            for (DateTime f = fechaIni.Date; f <= fechaFin.Date; f = f.AddDays(1))
            {
                var listaMedicionFuente = listaM48Ejecutado.Where(x => x.Medifecha.Date == f).ToList();

                var listaHO30min = this.servHO.ListarHO30min(listaHOPTotal, f);
                List<EveHoraoperacionDTO> listaHOModo = listaHO30min.Where(x => x.Hophorini.Value.Date == f && x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoModo).ToList();
                List<EveHoraoperacionDTO> listaHOUnidad = listaHO30min.Where(x => x.Hophorini.Value.Date == f && x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoUnidad).ToList();

                foreach (var m48 in listaMedicionFuente)
                {
                    List<MeMedicion48DTO> listaMedicionHOPEquipo = new List<MeMedicion48DTO>();

                    if (ConstantesPR5ReportesServicio.TgenercodiTermo == m48.Tgenercodi && m48.Grupocodi == 417)
                    {
                        int aaaaa = 0;
                    }

                    var equipo = listaUnidadTermo.Find(x => x.Grupocodi == m48.Grupocodi);
                    int? equicodi = equipo != null ? equipo.Equicodi : -1;

                    MeMedicion48DTO grupocodi30min = new MeMedicion48DTO();
                    grupocodi30min.Medifecha = f;
                    grupocodi30min.Grupocodi = m48.Grupocodi;
                    grupocodi30min.Gruponomb = m48.Gruponomb;
                    grupocodi30min.Grupopadre = m48.Grupopadre;
                    grupocodi30min.Central = m48.Central;
                    grupocodi30min.Emprcodi = m48.Emprcodi;
                    grupocodi30min.Emprnomb = m48.Emprnomb;
                    grupocodi30min.TipoFormulaMonitoreo = ConstantesMonitoreo.TipoDataGrupocodiModo;

                    MeMedicion48DTO cv30min = new MeMedicion48DTO();
                    cv30min.Medifecha = f;
                    cv30min.Grupocodi = m48.Grupocodi;
                    cv30min.Gruponomb = m48.Gruponomb;
                    cv30min.Grupopadre = m48.Grupopadre;
                    cv30min.Central = m48.Central;
                    cv30min.Emprcodi = m48.Emprcodi;
                    cv30min.Emprnomb = m48.Emprnomb;
                    cv30min.TipoFormulaMonitoreo = ConstantesMonitoreo.TipoDataCV;

                    //Determinar los Modos de operación/Grupo Hidraulico por cada media hora y su correspondiente costo variable
                    for (int h = 1; h <= 48; h++)
                    {
                        DateTime fi = f.AddMinutes(h * 30);

                        PrGrupoDTO grupo = null;
                        int grupocodi;

                        if (ConstantesPR5ReportesServicio.TgenercodiTermo == m48.Tgenercodi)
                        {
                            EveHoraoperacionDTO hopunidad = listaHOUnidad.Find(x => x.Equicodi == equicodi && x.HoraIni48 <= fi && fi <= x.HoraFin48);
                            EveHoraoperacionDTO hop = hopunidad != null ? listaHOModo.Find(x => x.Hopcodi == hopunidad.Hopcodipadre.GetValueOrDefault(-2)) : null;
                            if (hop != null)
                            {
                                grupocodi = hop.Grupocodi.Value;
                                grupo = listaModoOpTotal.Find(x => x.Grupocodi == hop.Grupocodi);
                                grupocodi30min.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).SetValue(grupocodi30min, (decimal)hop.Grupocodi);

                                PrCvariablesDTO cvModo = this.GetCostoVariable(ListCostoVariables, grupocodi, f);
                                if (cvModo != null)
                                    valorCV = cvModo.Cvc.GetValueOrDefault(0) + cvModo.Cvnc.GetValueOrDefault(0);
                                else
                                    valorCV = null;

                                cv30min.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).SetValue(cv30min, valorCV);
                            }
                            else
                            {
                                if (ConstantesMedicion.IdTipogrupoNoIntegrante == m48.Tipogrupocodi) //caso C.T. Tablazo o no Integrantes
                                {
                                    grupocodi = m48.Grupocodi;
                                    grupo = listaGrupoGeneracion.Find(x => x.Grupocodi == m48.Grupocodi);
                                    grupocodi30min.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).SetValue(grupocodi30min, (decimal)m48.Grupocodi);
                                }
                            }
                        }
                        else
                        {
                            grupocodi = m48.Grupocodi;
                            grupo = listaGrupoGeneracion.Find(x => x.Grupocodi == m48.Grupocodi);
                            grupocodi30min.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).SetValue(grupocodi30min, (decimal)m48.Grupocodi);
                            cv30min.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).SetValue(cv30min, 0.0m);
                        }
                    }

                    listaModo30min.Add(grupocodi30min);
                    listaCV30min.Add(cv30min);
                }
            }

            listaModo30minOut = listaModo30min;
            listaCV30minOut = listaCV30min;
        }

        /// <summary>
        /// Obtener costo variable de un modo de operacion en un día determinado
        /// </summary>
        /// <param name="ListaData"></param>
        /// <param name="grupocodi"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        private PrCvariablesDTO GetCostoVariable(List<PrCvariablesDTO> ListaData, int grupocodi, DateTime fecha)
        {
            PrCvariablesDTO reg = ListaData.Where(x => x.Grupocodi == grupocodi && x.Repfecha.Date <= fecha.Date)
                .OrderByDescending(x => x.Repfecha).FirstOrDefault();

            return reg;
        }

        /// <summary>
        /// Lista de Costos marginales por Fecha
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<CmCostomarginalprogDTO> ListCostoMarginalProg(DateTime fecha)
        {
            List<CmCostomarginalprogDTO> lista = FactorySic.GetCmCostomarginalprogRepository().ListPeriodoCostoMarProg(fecha);

            foreach (var reg in lista)
            {
                if (reg.Cmarprfecha.Value.Hour == 23 && reg.Cmarprfecha.Value.Minute == 59)
                {
                    reg.Cmarprfecha = reg.Cmarprfecha.Value.AddMinutes(1);
                }
            }

            return lista;
        }

        /// <summary>
        /// Nombre y ruta del Archivo de Una version
        /// </summary>
        /// <param name="version"></param>
        /// <param name="nombreArchivo"></param>
        /// <param name="rutaArchivo"></param>
        public void GetNombreYRutaArchivoReporteGeneracion(MmmVersionDTO version, out string nombreArchivo, out string rutaArchivo)
        {
            int mes = version.Vermmfechaperiodo.Month;
            string anio = version.Vermmfechaperiodo.Year + string.Empty;
            anio = anio.Substring(2, 2);

            nombreArchivo = EPDate.f_NombreMesCorto(mes) + anio + "_Version" + version.Vermmnumero;
            rutaArchivo = ConfigurationManager.AppSettings[ConstantesMonitoreo.RutaExcelIndicadores]
                + ConstantesMonitoreo.RptExcelGeneralIndicadores + nombreArchivo + ConstantesMonitoreo.ExtensionExcel;
        }

        /// <summary>
        /// Respuesta de generacion, 1: No existe generación en curso, 0: Sí existe generación realizandose 
        /// </summary>
        /// <returns></returns>
        public int NoExisteGeneracionEnProceso()
        {
            int resultado = 0;
            List<MmmVersionDTO> listVersion = this.GetByCriteriaMmmVersions();

            if (listVersion.Count != 0)
            {
                MmmVersionDTO listVersion2 = this.GetByCriteriaMmmVersions().OrderByDescending(x => x.Vermmcodi).First();
                if (listVersion2.Vermmporcentaje != 100 && listVersion2.Vermmporcentaje != -1)
                {
                    resultado = 0;
                }
                else
                {
                    resultado = 1;
                }
            }
            else
            {
                resultado = 1;
            }
            return resultado;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public string ReporteListadoVersionHtml(DateTime fechaInicio, DateTime fechaFin, string url)
        {
            StringBuilder strHtml = new StringBuilder();
            List<MmmVersionDTO> data = this.ListMmmVersions();

            MmmVersionDTO version = new MmmVersionDTO();
            var listVersion = this.GetByCriteriaMmmVersions().OrderByDescending(x => x.Vermmcodi).FirstOrDefault();

            //Te devuelve  lista de generacion
            var listaGeneracion = data.Where(x => x.Vermmfechaperiodo >= fechaInicio && x.Vermmfechaperiodo <= fechaFin).OrderBy(x => x.Vermmcodi).ToList();

            int padding = 20;
            //Te da un formato  de tabla
            int anchoTotal = (100 + padding) + listaGeneracion.Count * (200 + padding);
            if (listVersion != null)
            {
                if (listVersion.Vermmporcentaje != 100 && listVersion.Vermmporcentaje != -1)
                {
                    int anio = listVersion.Vermmfechaperiodo.Year;
                    int mes = listVersion.Vermmfechaperiodo.Month;
                    strHtml.Append(" <div id='textoMensaje' style=' margin: 0; margin-bottom: 10px;'  class='action-alert'>");
                    strHtml.Append("Existe una generación del Reporte de Indicadores en curso, Periodo: <span style='font-weight: bold;'>" + EPDate.f_NombreMes(mes).ToUpper() + " " + anio + "</span>");
                    strHtml.Append("</div>");
                }
            }
            strHtml.Append("<br>");
            strHtml.Append("<div class='freeze_table' id='resultado1' style='height: auto;'>");
            strHtml.AppendFormat("<table id='reporte' class='pretty tabla-icono' style='table-layout: fixed; width: {0}px;' >", anchoTotal);
            //Se genera cabecera
            #region cabecera
            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px'> Versión </th>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px'> Fecha de Período </th>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 150px'> Fecha de Generación </th>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px'> Estado </th>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px'> Fecha de Aprobación </th>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px'> Motivo </th>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px'> Usuario </th>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px'> Motivo en Portal </th>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px'> Acción </th>");
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");
            #endregion
            strHtml.Append("<tbody>");
            #region cuerpo

            //Te lista las generacion dependiendo del periodo
            foreach (var valor in listaGeneracion)
            {
                bool estado = false;
                string msjError = valor.Vermmporcentaje == -1 ? (valor.Vermmmsjgeneracion != null ? valor.Vermmmsjgeneracion
                    .Replace(Environment.NewLine, "<br />").Replace("\r", "<br />").Replace("\n", "<br />") : string.Empty) : string.Empty;
                string msjError1 = msjError.Replace("<br />", "\\n\\n");
                string msjError2 = msjError.Replace("<br />", ". ");

                strHtml.AppendFormat("<tr title='{0}'>", msjError2);
                int id = valor.Vermmcodi;
                strHtml.Append(string.Format("<td>{0}</td>", valor.Vermmnumero));
                strHtml.Append(string.Format("<td>{0}</td>", valor.Vermmfechaperiodo.ToString(ConstantesAppServicio.FormatoMes)));
                string color; color = (valor.VermmestadoDesc == ConstantesMonitoreo.PorAprobar) ? "#FED866" : (valor.VermmestadoDesc == ConstantesMonitoreo.Publicado) ? "#A8D08E" : "#F60825";
                string colorLetra; colorLetra = (valor.VermmestadoDesc == ConstantesMonitoreo.PorAprobar) ? "#555051" : (valor.VermmestadoDesc == ConstantesMonitoreo.Publicado) ? "#555051" : "white";

                if (valor.Vermmporcentaje != 100 && valor.Vermmporcentaje != -1)
                {
                    strHtml.Append(string.Format("<td>{0}% <img src='" + url + "Content/Images/loading.gif' width='15'/></td>", valor.Vermmporcentaje));
                }
                else if (valor.Vermmporcentaje == -1)
                {
                    if (msjError.Length == 0)
                    {
                        strHtml.AppendFormat("<td><img src='{0}Content/Images/error.png' width='15'/></td>", url);
                    }
                    else
                    {
                        strHtml.AppendFormat("<td><img src='{0}Content/Images/error.png' onclick=\"javascript: alert('{1}');\" width='15'/></td>", url, msjError1);
                    }
                }
                else
                {
                    strHtml.Append(string.Format("<td>{0}</td>", valor.Vermmfechageneracion != null ? valor.Vermmfechageneracion.Value.ToString(ConstantesAppServicio.FormatoFechaFull) : string.Empty));
                    estado = true;
                }
                strHtml.Append(string.Format("<td style='background-color: " + color + "; color:" + colorLetra + ";' >{0}</td>", valor.VermmestadoDesc));
                strHtml.Append(string.Format("<td>{0}</td>", valor.VermmfechaaprobacionDesc));
                strHtml.Append(string.Format("<td>{0}</td>", valor.Vermmmotivo));
                strHtml.Append(string.Format("<td>{0}</td>", valor.Vermmusucreacion));
                strHtml.Append(string.Format("<td>{0}</td>", valor.vermmPortalDesc));

                string nombreExcel, file;
                this.GetNombreYRutaArchivoReporteGeneracion(valor, out nombreExcel, out file);
                if (estado)
                {
                    FileInfo newFile = new FileInfo(file);
                    if (newFile.Exists)
                    {
                        strHtml.Append(string.Format("<td>{0}</td>", " <a href='JavaScript:visualizarGeneracion(" + id + ");' title='Visualizar Reporte de Indicadores'><img src='" + url + "Content/Images/btn-open.png' alt='Editar versión' /></a> <a href='JavaScript:editarGeneracion(" + id + ");' title='Editar versión'><img src='" + url + "Content/Images/btn-edit.png' alt='Editar versión' /></a> <a href='JavaScript:DescargarExcelVersion(\"" + nombreExcel + "\");' title='Exportar Excel'><img src='" + url + "Content/Images/ExportExcel.png' alt='Exportar Excel' /></a>"));
                    }
                    else
                    {
                        strHtml.Append(string.Format("<td>{0}</td>", " <a href='JavaScript:visualizarGeneracion(" + id + ");' title='Visualizar Reporte de Indicadores'><img src='" + url + "Content/Images/btn-open.png' alt='Editar versión' /></a> <a href='JavaScript:editarGeneracion(" + id + ");' title='Editar versión'><img src='" + url + "Content/Images/btn-edit.png' alt='Editar versión' /></a>"));
                    }
                }
                else
                {
                    strHtml.Append(string.Format("<td></td>"));
                }
                strHtml.Append("</tr>");
            }
            #endregion
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        /// <summary>
        ///  Al iniciar el servidor de la Intranet buscará si una generación quedo pendiente de terminar
        /// </summary>
        public void VerificarUltimaGeneracionMonitoreoMME()
        {
            try
            {
                MmmVersionDTO ultimaVersion = this.GetByCriteriaMmmVersions().OrderByDescending(x => x.Vermmcodi).FirstOrDefault();

                if (ultimaVersion != null)
                {
                    MmmVersionDTO regVersion = new MmmVersionDTO();
                    int version = ultimaVersion.Vermmcodi;
                    if (ultimaVersion.Vermmporcentaje != 100 && ultimaVersion.Vermmporcentaje != -1)
                    {
                        regVersion.Vermmcodi = version;
                        regVersion.Vermmporcentaje = -1;
                        regVersion.Vermmmsjgeneracion = "Ocurrió un error cuando se realizaba el proceso. La generación de la versión empezó a las " + ultimaVersion.Vermmfeccreacion.ToString(ConstantesAppServicio.FormatoFechaFull2)
                            + ", se terminó cuando estaba al " + ultimaVersion.Vermmporcentaje + "% al detenerse el servidor.\nEl servidor inició nuevamente a las " + DateTime.Now.ToString(ConstantesAppServicio.FormatoFechaFull2);
                        regVersion.Vermmmsjgeneracion = regVersion.Vermmmsjgeneracion.Trim();
                        if (regVersion.Vermmmsjgeneracion.Length > 500)
                            regVersion.Vermmmsjgeneracion = regVersion.Vermmmsjgeneracion.Substring(0, 500);

                        this.UpdateMmmVersionPorcentaje(regVersion);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
            }
        }

        /// <summary>
        /// Obtener fecha maxima de consulta para los reporte de monitoreo
        /// </summary>
        /// <param name="strFechaIniProceso"></param>
        /// <param name="numDiaPicker"></param>
        public void GetFechaMaxGeneracionPermitida(out string strFechaIniProceso, out int numDiaPicker)
        {
            int dia = DateTime.Now.Day;
            //resta de mes a iniciar
            int mes;
            // Configura la cantidad de meses a mostrar dependiendo si es antes del 6 del mes acual o no 

            if (dia < 6)
            {
                numDiaPicker = ((dia - 1) * -1) - 60;
                mes = 2;
            }
            else
            {
                numDiaPicker = ((dia - 1) * -1) - 30;
                mes = 1;
            }
            strFechaIniProceso = new DateTime(DateTime.Now.Year, (DateTime.Now.Month), 1).AddMonths(-mes).ToString(ConstantesAppServicio.FormatoMes);
        }

        #endregion

        #region Reporte Indicadores

        /// <summary>
        /// Devuelve la lista de empresas para el monitoreo
        /// </summary>
        /// <param name="fechaPeriodo"></param>
        /// <param name="filtroEmpresas"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarEmpresasMonitoreo(DateTime fechaPeriodo, string filtroEmpresas)
        {
            //Lista de integrantes
            DateTime fechaIniPeriodo = new DateTime(fechaPeriodo.Year, fechaPeriodo.Month, 1);
            DateTime fechaFinPeriodo = fechaIniPeriodo.AddMonths(1).AddDays(-1);
            List<SiEmpresaDTO> listaIntegrantes = FactorySic.GetSiEmpresaRepository().ListarEmpresaIntegranteMonitoreoMME(fechaIniPeriodo, fechaFinPeriodo);
            //Lista de de empresas
            List<SiEmpresaDTO> empresas = listaIntegrantes;

            //Recorrido a la empresas  si no  se encuentra la razon social se remplaza por el nombre de empresa 
            foreach (var objEmpr in empresas)
            {
                if (objEmpr.Emprrazsocial != null)
                {
                    objEmpr.Emprnomb = objEmpr.Emprrazsocial;
                }
            }
            empresas = empresas.OrderBy(x => x.Emprnomb).ToList();

            //Obtener solo empresas segun filtro
            int[] empresaslistado = new int[filtroEmpresas.Length];
            if (filtroEmpresas != ConstantesAppServicio.ParametroDefecto)
            {
                empresaslistado = filtroEmpresas.Split(',').Select(x => int.Parse(x)).ToArray();
                empresas = empresas.Where(x => empresaslistado.Contains(x.Emprcodi)).ToList();
            }

            return empresas;
        }

        /// <summary>
        /// Obtener la lista de empresas segun el filtro del combo multiple
        /// </summary>
        /// <param name="listaIntegrantes"></param>
        /// <param name="filtroEmpresas"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarEmpresasMonitoreoByData(List<SiEmpresaDTO> listaIntegrantes, string filtroEmpresas)
        {

            //Lista de de empre
            //Recorrido a la empresas  sas
            List<SiEmpresaDTO> empresas = listaIntegrantes;
            //si no  se encuentra la razon social se remplaza por el nombre de empresa 
            foreach (var objEmpr in empresas)
            {
                if (objEmpr.Emprrazsocial != null)
                {
                    objEmpr.Emprnomb = objEmpr.Emprrazsocial;
                }
            }
            empresas = empresas.OrderBy(x => x.Emprnomb).ToList();

            //Obtener solo empresas segun filtro
            int[] empresaslistado = new int[filtroEmpresas.Length];
            if (filtroEmpresas != ConstantesAppServicio.ParametroDefecto)
            {
                empresaslistado = filtroEmpresas.Split(',').Select(x => int.Parse(x)).ToArray();
                empresas = empresas.Where(x => empresaslistado.Contains(x.Emprcodi)).ToList();
            }

            return empresas;
        }

        /// <summary>
        /// Retorna  Lista de MW Ejecutado Programado  Cm Ejecutado Cm Progrado y Costo Varaible
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="listEjecutado"></param>
        /// <param name="listProgramado"></param>
        /// <param name="listCostoMarginal"></param>
        /// <param name="listCostoMarginalProg"></param>
        /// <param name="listCostoVariable"></param>
        /// <param name="generacion"></param>
        /// <param name="indicador"></param>
        public void ConvertirMMMDatoToM48(DateTime fechaInicio, DateTime fechaFin, List<MmmDatoDTO> listFacTable, out List<MeMedicion48DTO> listEjecutado, out List<MeMedicion48DTO> listProgramado
            , out List<MeMedicion48DTO> listCostoMarginalEjec, out List<MeMedicion48DTO> listCostoMarginalProg
            , out List<MeMedicion48DTO> listModo30minByGrupo, out List<MeMedicion48DTO> listCostoVariableByGrupo
            , int vermmcodi, int indicador)
        {
            listEjecutado = new List<MeMedicion48DTO>();
            listProgramado = new List<MeMedicion48DTO>();
            listCostoMarginalEjec = new List<MeMedicion48DTO>();
            listCostoMarginalProg = new List<MeMedicion48DTO>();
            listModo30minByGrupo = new List<MeMedicion48DTO>();
            listCostoVariableByGrupo = new List<MeMedicion48DTO>();

            // Retorna la lista de cambio conrespecto a la generacion
            List<MmmCambioversionDTO> listaCambios = vermmcodi > 0 ? this.GetByCriteriaMmmCambioversions(vermmcodi) : new List<MmmCambioversionDTO>();
            DateTime diaIncremental;
            int dia = fechaInicio.Day;

            // recorrido de lista  
            for (DateTime day = fechaInicio.Date; day <= fechaFin.Date; day = day.AddDays(1))
            {
                //Lista la informacion  de mmm-dato  y las filtra por fecha
                List<MmmDatoDTO> listFactableByDia = listFacTable.Where(x => x.Mmmdatfecha >= day.AddMinutes(30) && x.Mmmdatfecha <= day.AddDays(1)).ToList();
                //Lista de grupocodi
                List<int> listaGrupoCodi = listFactableByDia.Select(x => x.Grupocodi).Distinct().OrderBy(x => x).ToList();

                #region Ejecutados y Programados, Costos Variables, Modos de Operacion, Empresa
                foreach (var grupocodi in listaGrupoCodi)
                {
                    if (grupocodi == 187)
                    {

                    }

                    var grupo = listFactableByDia.Find(x => x.Grupocodi == grupocodi);

                    if (grupo != null)
                    {
                        MeMedicion48DTO reg48FinalEjecutado = new MeMedicion48DTO();
                        reg48FinalEjecutado.Emprcodi = grupo.Emprcodi;
                        reg48FinalEjecutado.Grupocodi = grupo.Grupocodi;
                        reg48FinalEjecutado.Barrcodi = grupo.Barrcodi.GetValueOrDefault(0);
                        reg48FinalEjecutado.Catecodi = grupo.Catecodi;
                        reg48FinalEjecutado.Medifecha = day;
                        reg48FinalEjecutado.Grupopadre = grupo.Grupopadre;
                        reg48FinalEjecutado.TipoFormulaMonitoreo = ConstantesMonitoreo.TipoFormulaPotGrupoEjec;

                        MeMedicion48DTO reg48FinalProgramado = new MeMedicion48DTO();
                        reg48FinalProgramado.Emprcodi = grupo.Emprcodi;
                        reg48FinalProgramado.Grupocodi = grupo.Grupocodi;
                        reg48FinalProgramado.Barrcodi = grupo.Barrcodi.GetValueOrDefault(0);
                        reg48FinalProgramado.Catecodi = grupo.Catecodi;
                        reg48FinalProgramado.Medifecha = day;
                        reg48FinalProgramado.Grupopadre = grupo.Grupopadre;
                        reg48FinalProgramado.TipoFormulaMonitoreo = ConstantesMonitoreo.TipoFormulaPotGrupoProg;

                        MeMedicion48DTO reg48FinalModo = new MeMedicion48DTO();
                        reg48FinalModo.Emprcodi = grupo.Emprcodi;
                        reg48FinalModo.Grupocodi = grupo.Grupocodi;
                        reg48FinalModo.Barrcodi = grupo.Barrcodi.GetValueOrDefault(0);
                        reg48FinalModo.Catecodi = grupo.Catecodi;
                        reg48FinalModo.Medifecha = day;
                        reg48FinalModo.Grupopadre = grupo.Grupopadre;

                        MeMedicion48DTO reg48FinalCV = new MeMedicion48DTO();
                        reg48FinalCV.Emprcodi = grupo.Emprcodi;
                        reg48FinalCV.Grupocodi = grupo.Grupocodi;
                        reg48FinalCV.Barrcodi = grupo.Barrcodi.GetValueOrDefault(0);
                        reg48FinalCV.Catecodi = grupo.Catecodi;
                        reg48FinalCV.Medifecha = day;
                        reg48FinalCV.Grupopadre = grupo.Grupopadre;

                        for (int i = 1; i <= 48; i++)
                        {
                            if (i == 28)
                            {

                            }

                            diaIncremental = day.AddMinutes(i * 30);

                            var factValores = listFactableByDia.Find(x => x.Mmmdatfecha == diaIncremental && x.Grupocodi == grupo.Grupocodi);

                            if (factValores != null)
                            {
                                //Objeto de cambios version 
                                var mwCambios = listaCambios.Find(x => x.Mmmdatcodi == factValores.Mmmdatcodi && x.Camvertipo == ConstantesMonitoreo.CambioMmmdatomwejec);
                                var mwprogCambios = listaCambios.Find(x => x.Mmmdatcodi == factValores.Mmmdatcodi && x.Camvertipo == ConstantesMonitoreo.CambioMmmdatomwprog);
                                var emprcodiCambios = listaCambios.Find(x => x.Mmmdatcodi == factValores.Mmmdatcodi && x.Camvertipo == ConstantesMonitoreo.CambioEmprcodi);
                                var barraTransCambios = listaCambios.Find(x => x.Mmmdatcodi == factValores.Mmmdatcodi && x.Camvertipo == ConstantesMonitoreo.CambioBarrcodi);

                                decimal? mmmdatomwejecutado = factValores.Mmmdatmwejec;
                                decimal? mmmdatomwprogramado = factValores.Mmmdatmwprog;
                                reg48FinalEjecutado.Barrcodi = factValores.Barrcodi.GetValueOrDefault(-1);
                                reg48FinalProgramado.Barrcodi = factValores.Barrcodi.GetValueOrDefault(-1);
                                reg48FinalEjecutado.Emprcodi = factValores.Emprcodi;
                                reg48FinalProgramado.Emprcodi = factValores.Emprcodi;

                                if (emprcodiCambios != null && emprcodiCambios.Camvervalor != factValores.Emprcodi)
                                {
                                    reg48FinalEjecutado.Emprcodi = Convert.ToInt32(emprcodiCambios.Camvervalor);
                                    reg48FinalProgramado.Emprcodi = Convert.ToInt32(emprcodiCambios.Camvervalor);
                                    grupo.Emprcodi = Convert.ToInt32(emprcodiCambios.Camvervalor);
                                }

                                if (barraTransCambios != null && barraTransCambios.Camvervalor != factValores.Barrcodi)
                                {
                                    reg48FinalEjecutado.Barrcodi = Convert.ToInt32(barraTransCambios.Camvervalor);
                                    reg48FinalProgramado.Barrcodi = Convert.ToInt32(barraTransCambios.Camvervalor);
                                }

                                if (mwCambios != null && mwCambios.Camvervalor != factValores.Mmmdatmwejec)
                                {
                                    mmmdatomwejecutado = mwCambios.Camvervalor;
                                }

                                if (mwprogCambios != null && mwprogCambios.Camvervalor != factValores.Mmmdatmwprog)
                                {

                                    mmmdatomwprogramado = mwprogCambios.Camvervalor;
                                }

                                reg48FinalCV.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).SetValue(reg48FinalCV, factValores.Mmmdatocvar);
                                reg48FinalModo.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).SetValue(reg48FinalModo, Convert.ToDecimal(factValores.Mogrupocodi));
                                reg48FinalEjecutado.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).SetValue(reg48FinalEjecutado, mmmdatomwejecutado);
                                reg48FinalProgramado.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).SetValue(reg48FinalProgramado, mmmdatomwprogramado);
                            }
                        }

                        //Agrega la informacion en un la lista que sera reutilizada en los indicadores
                        listEjecutado.Add(reg48FinalEjecutado);
                        listProgramado.Add(reg48FinalProgramado);
                        listCostoVariableByGrupo.Add(reg48FinalCV);
                        listModo30minByGrupo.Add(reg48FinalModo);
                    }
                }

                #endregion

                #region Costos Marginales Ejecutados y Programados
                var listaBarraYEmpresa = listFactableByDia.Where(x => x.Barrcodi.GetValueOrDefault(0) > 0).Select(x => new { x.Barrcodi, x.Emprcodi })
                    .GroupBy(x => new { x.Emprcodi, x.Barrcodi }).Select(x => new MeMedicion48DTO() { Emprcodi = x.Key.Emprcodi, Barrcodi = x.Key.Barrcodi.GetValueOrDefault(0) }).ToList();

                if (ConstantesMonitoreo.CodigoILE == indicador || ConstantesMonitoreo.CodigoIMU == indicador || ConstantesMonitoreo.CodigoIRT == indicador)
                {
                    foreach (var barraEmp in listaBarraYEmpresa)
                    {
                        MmmDatoDTO grupo = listFactableByDia.Find(x => x.Barrcodi == barraEmp.Barrcodi && x.Emprcodi == barraEmp.Emprcodi);

                        MeMedicion48DTO regCostoMarginal = new MeMedicion48DTO();
                        regCostoMarginal.Medifecha = day;
                        regCostoMarginal.Emprcodi = grupo.Emprcodi;
                        regCostoMarginal.Barrcodi = grupo.Barrcodi.GetValueOrDefault(0);

                        var listaFactByBarra = listFactableByDia.Where(x => x.Barrcodi == barraEmp.Barrcodi).OrderByDescending(x => x.Grupocodi).ToList();

                        for (int i = 1; i <= 48; i++)
                        {
                            decimal? costoMg = null;
                            var objDato = listaFactByBarra.Find(x => x.Mmmdathoraindex == i);

                            if (objDato != null)
                            {
                                costoMg = objDato.Mmmdatcmgejec;
                                var cmEjectuado = listaCambios.Find(x => x.Mmmdatcodi == objDato.Mmmdatcodi && x.Camvertipo == ConstantesMonitoreo.CambioMmmdatocmgejec);

                                if (cmEjectuado != null && cmEjectuado.Camvervalor != objDato.Mmmdatcmgejec)
                                {
                                    costoMg = cmEjectuado.Camvervalor;
                                }
                            }

                            regCostoMarginal.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).SetValue(regCostoMarginal, costoMg);
                        }

                        listCostoMarginalEjec.Add(regCostoMarginal);
                    }
                }

                var listFactableByDiaprueba = listFactableByDia.Where(x => x.Grupocodi == 187).ToList();

                var listaBarraProgYEmpresa = listFactableByDia.Where(x => x.Cnfbarcodi.GetValueOrDefault(0) > 0).Select(x => new { x.Cnfbarcodi, x.Emprcodi })
                       .GroupBy(x => new { x.Emprcodi, x.Cnfbarcodi }).Select(x => new MeMedicion48DTO() { Emprcodi = x.Key.Emprcodi, Cnfbarcodi = x.Key.Cnfbarcodi.GetValueOrDefault(0) }).ToList();

                if (ConstantesMonitoreo.CodigoIRT == indicador)
                {
                    foreach (var barraEmp in listaBarraProgYEmpresa)
                    {
                        MmmDatoDTO grupo = listFactableByDia.Find(x => x.Cnfbarcodi == barraEmp.Cnfbarcodi && x.Emprcodi == barraEmp.Emprcodi);

                        MeMedicion48DTO regCostoMarginalProg = new MeMedicion48DTO();
                        regCostoMarginalProg.Medifecha = day;
                        regCostoMarginalProg.Emprcodi = grupo.Emprcodi;
                        regCostoMarginalProg.Cnfbarcodi = grupo.Cnfbarcodi.GetValueOrDefault(0);
                        regCostoMarginalProg.TipoFormulaMonitoreo = ConstantesMonitoreo.TipoFormulaCMProg;

                        var listaFactByBarra = listFactableByDia.Where(x => x.Cnfbarcodi == barraEmp.Cnfbarcodi).OrderByDescending(x => x.Grupocodi).ToList();

                        for (int i = 1; i <= 48; i++)
                        {
                            decimal? costoMg = null;
                            var objDato = listaFactByBarra.Find(x => x.Mmmdathoraindex == i);

                            if (objDato != null)
                            {
                                costoMg = objDato.Mmmdatcmgprog;
                                var cmgProgramado = listaCambios.Find(x => x.Mmmdatcodi == objDato.Mmmdatcodi && x.Camvertipo == ConstantesMonitoreo.CambioMmmdatocmgprog);

                                if (cmgProgramado != null && cmgProgramado.Camvervalor != objDato.Mmmdatcmgprog)
                                {
                                    costoMg = cmgProgramado.Camvervalor;
                                }
                            }

                            regCostoMarginalProg.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).SetValue(regCostoMarginalProg, costoMg);
                        }

                        listCostoMarginalProg.Add(regCostoMarginalProg);
                    }
                }

                #endregion
            }
        }

        /// <summary>
        /// Método que devuelve el primer indicador
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ListarIndicador(int vermmcodi, DateTime fechaInicio, DateTime fechaFin, int indicador, List<MmmDatoDTO> listFacTable, int tipoReporte, out List<SiEmpresaDTO> listaEmpresa)
        {
            //Declaraciones de Listas a utilizar en los indicadores
            List<MeMedicion48DTO> listaFinal = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaPotXDiaEjecutado;
            List<MeMedicion48DTO> listaPotXDiaProgramado;
            List<MeMedicion48DTO> listCostoMarginalEjec;
            List<MeMedicion48DTO> listCostoMarginalProg;
            List<MeMedicion48DTO> listCostoVariables;
            List<MeMedicion48DTO> listModo30minByGrupo;

            // Obtención de la data de la Factable
            this.ConvertirMMMDatoToM48(fechaInicio, fechaFin, listFacTable, out listaPotXDiaEjecutado, out listaPotXDiaProgramado
                , out listCostoMarginalEjec, out listCostoMarginalProg, out listModo30minByGrupo, out listCostoVariables, vermmcodi, indicador);

            List<int> listaEmprcodiByData = listFacTable.Select(x => x.Emprcodi).Distinct().ToList();


            //Lista de Empresas para ese periodo
            listaEmpresa = this.ListarEmpresasMonitoreo(fechaInicio, ConstantesAppServicio.ParametroDefecto);

            listaEmpresa = listaEmpresa.Where(x => listaEmprcodiByData.Contains(x.Emprcodi)).ToList();

            // Obtencion de  Mw Programado Ejecutado y Programado
            List<MeMedicion48DTO> listaPotXDiaXEmpresa = this.ListarPotenciaXEmpresaM48(fechaInicio, fechaFin, listaPotXDiaEjecutado, ConstantesMonitoreo.TipoFormulaMWEjec, listaEmpresa);
            List<MeMedicion48DTO> listaProgXDiaXEmpresa = this.ListarPotenciaXEmpresaM48(fechaInicio, fechaFin, listaPotXDiaProgramado, ConstantesMonitoreo.TipoFormulaMWProg, listaEmpresa);

            //Obtener total de la potencia del SEIN
            List<MeMedicion48DTO> listaTotalXDia = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaCuotaXDiaXEmpresa = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaHHIXDia = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaHHICuotaXDiaXEmpresa = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaIOPxDiaXEmpresa = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaRSDxDiaXEmpresa = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaMDXDia = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaCVxDiaXBarra = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaILExDiaXBarra = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaIMUxDiaXBarra = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaCongestion = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaEnlaceTrans = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaIRT = new List<MeMedicion48DTO>();

            #region Potencia Total del Sein de cada día
            for (DateTime day = fechaInicio.Date; day <= fechaFin.Date; day = day.AddDays(1))
            {
                MeMedicion48DTO regTotal = new MeMedicion48DTO();
                regTotal.TipoFormulaMonitoreo = ConstantesMonitoreo.TipoFormulaTotalPotencia;
                regTotal.Medifecha = day;

                var listaPotXDiaXEmpresaTemp = listaPotXDiaXEmpresa.Where(x => x.Medifecha == day).Distinct().ToList();
                foreach (var regtmp in listaPotXDiaXEmpresaTemp)
                {
                    for (int i = 1; i <= 48; i++)
                    {
                        decimal? valorNuevo = (decimal?)regtmp.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).GetValue(regtmp, null);
                        decimal? valorAcum = (decimal?)regTotal.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).GetValue(regTotal, null);
                        if (valorNuevo != null)
                        {
                            valorAcum = valorAcum.GetValueOrDefault(0) + valorNuevo;
                            regTotal.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).SetValue(regTotal, valorAcum);
                        }
                    }
                }
                listaTotalXDia.Add(regTotal);
            }
            #endregion

            if (ConstantesMonitoreo.CodigoS == indicador || ConstantesMonitoreo.CodigoHHI == indicador)
            {
                #region Cuota de Mercado

                foreach (var potEmp in listaPotXDiaXEmpresa)
                {
                    MeMedicion48DTO cuotaEmp = new MeMedicion48DTO();
                    cuotaEmp.Emprcodi = potEmp.Emprcodi;
                    cuotaEmp.TipoFormulaMonitoreo = ConstantesMonitoreo.TipoFormulaCuota;
                    cuotaEmp.Medifecha = potEmp.Medifecha;

                    MeMedicion48DTO regTotal = listaTotalXDia.Find(x => x.Medifecha == potEmp.Medifecha);

                    for (int i = 1; i <= 48; i++)
                    {
                        decimal? valorPotEmpXH = (decimal?)potEmp.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).GetValue(potEmp, null);
                        decimal? valorTotalXH = (decimal?)regTotal.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).GetValue(regTotal, null);
                        decimal? cuotaEmpXH = valorTotalXH != 0 ? (valorPotEmpXH / valorTotalXH) * 100 : 0;
                        cuotaEmp.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).SetValue(cuotaEmp, cuotaEmpXH);
                    }
                    listaCuotaXDiaXEmpresa.Add(cuotaEmp);
                }

                #endregion

                #region Porcentaje de Error Banda de Tolerancia

                if (ConstantesMonitoreo.ReportePorcentajeErrorBandaTolerancia == tipoReporte)
                {
                    MmmBandtolDTO bt = this.GetBandaToleranciaByIndicadorYPeriodo(ConstantesMonitoreo.CodigoS, fechaInicio);
                    if (bt != null)
                    {
                        List<MeMedicion48DTO> listaCuotaErrorBTXDiaXEmpresa = new List<MeMedicion48DTO>();
                        foreach (var regIndicador in listaCuotaXDiaXEmpresa)
                        {
                            MeMedicion48DTO errorBT = new MeMedicion48DTO();
                            errorBT.Emprcodi = regIndicador.Emprcodi;
                            errorBT.TipoFormulaMonitoreo = ConstantesMonitoreo.TipoFormulaCuotaErrorBT;
                            errorBT.Medifecha = regIndicador.Medifecha;

                            for (int i = 1; i <= 48; i++)
                            {
                                decimal? indicadorXH = (decimal?)regIndicador.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).GetValue(regIndicador, null);
                                if (indicadorXH.GetValueOrDefault(0) != 0)
                                {
                                    decimal? errorXH = (indicadorXH - bt.Mmmtolvalorreferencia / indicadorXH) * 100;
                                    errorBT.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).SetValue(errorBT, errorXH);
                                }
                            }
                            listaCuotaErrorBTXDiaXEmpresa.Add(errorBT);
                        }

                        listaFinal.AddRange(listaCuotaErrorBTXDiaXEmpresa);
                    }
                }

                #endregion
            }
            if (ConstantesMonitoreo.CodigoHHI == indicador)
            {
                #region HHI
                for (DateTime day = fechaInicio.Date; day <= fechaFin.Date; day = day.AddDays(1))
                {
                    MeMedicion48DTO regTotal = new MeMedicion48DTO();
                    regTotal.TipoFormulaMonitoreo = ConstantesMonitoreo.TipoFormulaHHI;
                    regTotal.Medifecha = day;

                    var listaCuotaXDia = listaCuotaXDiaXEmpresa.Where(x => x.Medifecha == day).ToList();
                    foreach (var cuotaEmp in listaCuotaXDia)
                    {
                        MeMedicion48DTO cuota2Emp = new MeMedicion48DTO();
                        cuota2Emp.Emprcodi = cuotaEmp.Emprcodi;
                        cuota2Emp.TipoFormulaMonitoreo = ConstantesMonitoreo.TipoFormulaHHICuotaMercado;
                        cuota2Emp.Medifecha = cuotaEmp.Medifecha;

                        for (int i = 1; i <= 48; i++)
                        {
                            decimal? valorAcum = (decimal?)regTotal.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).GetValue(regTotal, null);

                            decimal? valorCuotaEmpXH = (decimal?)cuotaEmp.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).GetValue(cuotaEmp, null);
                            decimal? cuotaHHIEmpXH = null;
                            if (valorCuotaEmpXH != null)
                            {
                                cuotaHHIEmpXH = valorCuotaEmpXH.Value * valorCuotaEmpXH.Value;
                            }
                            cuota2Emp.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).SetValue(cuota2Emp, cuotaHHIEmpXH);
                            valorAcum = valorAcum.GetValueOrDefault(0) + cuotaHHIEmpXH.GetValueOrDefault(0);
                            regTotal.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).SetValue(regTotal, valorAcum);
                        }
                        listaHHICuotaXDiaXEmpresa.Add(cuota2Emp);
                    }

                    listaHHIXDia.Add(regTotal);
                }
                #endregion

                #region Porcentaje de Error Banda de Tolerancia

                if (ConstantesMonitoreo.ReportePorcentajeErrorBandaTolerancia == tipoReporte)
                {
                    MmmBandtolDTO bt = this.GetBandaToleranciaByIndicadorYPeriodo(ConstantesMonitoreo.CodigoHHI, fechaInicio);
                    if (bt != null)
                    {
                        List<MeMedicion48DTO> listaHHIErrorBTXDia = new List<MeMedicion48DTO>();
                        foreach (var regIndicador in listaHHIXDia)
                        {
                            MeMedicion48DTO errorBT = new MeMedicion48DTO();
                            errorBT.TipoFormulaMonitoreo = ConstantesMonitoreo.TipoFormulaHHIErrorBT;
                            errorBT.Medifecha = regIndicador.Medifecha;

                            for (int i = 1; i <= 48; i++)
                            {
                                decimal? indicadorXH = (decimal?)regIndicador.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).GetValue(regIndicador, null);
                                if (indicadorXH.GetValueOrDefault(0) != 0)
                                {
                                    decimal? errorXH = (indicadorXH - bt.Mmmtolvalorreferencia / indicadorXH) * 100;
                                    errorBT.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).SetValue(errorBT, errorXH);
                                }
                            }
                            listaHHIErrorBTXDia.Add(errorBT);
                        }

                        listaFinal.AddRange(listaHHIErrorBTXDia);
                    }
                }

                #endregion
            }

            if (ConstantesMonitoreo.CodigoIOP == indicador || ConstantesMonitoreo.CodigoRSD == indicador)
            {
                #region Máxima Demanda Programada

                for (DateTime day = fechaInicio.Date; day <= fechaFin.Date; day = day.AddDays(1))
                {
                    MeMedicion48DTO regTotal = new MeMedicion48DTO();

                    var listaProgXDiaXEmpresaTemp = listaProgXDiaXEmpresa.Where(x => x.Medifecha == day).ToList();
                    foreach (var regtmp in listaProgXDiaXEmpresaTemp)
                    {
                        for (int i = 1; i <= 48; i++)
                        {
                            regTotal.TipoFormulaMonitoreo = ConstantesMonitoreo.TipoFormulaMaximaDemanda;
                            regTotal.Medifecha = day;

                            decimal? valorNuevo = (decimal?)regtmp.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).GetValue(regtmp, null);

                            decimal? valorAcum = (decimal?)regTotal.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).GetValue(regTotal, null);
                            if (valorNuevo != null)
                            {
                                valorAcum = valorAcum.GetValueOrDefault(0) + valorNuevo;
                                regTotal.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).SetValue(regTotal, valorAcum);
                            }
                        }
                    }

                    decimal? valorFinal = 0;
                    for (int i = 1; i <= 48; i++)
                    {
                        decimal? valor = (decimal?)regTotal.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(regTotal, null);
                        if (valor >= valorFinal)
                        {
                            valorFinal = valor;
                        }
                    }

                    MeMedicion48DTO regTotal2 = new MeMedicion48DTO();
                    for (int i = 1; i <= 48; i++)
                    {
                        regTotal2.TipoFormulaMonitoreo = ConstantesMonitoreo.TipoFormulaMaximaDemanda;
                        regTotal2.Medifecha = day;
                        regTotal2.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).SetValue(regTotal2, valorFinal);
                    }

                    listaMDXDia.Add(regTotal2);
                }

                #endregion
            }

            if (ConstantesMonitoreo.CodigoIOP == indicador)
            {
                #region Oferta Pivotal
                for (DateTime day = fechaInicio.Date; day <= fechaFin.Date; day = day.AddDays(1))
                {
                    MeMedicion48DTO total = listaTotalXDia.Find(x => x.Medifecha == day);
                    MeMedicion48DTO md = listaMDXDia.Find(x => x.Medifecha == day);

                    var listaPotXDia = listaPotXDiaXEmpresa.Where(x => x.Medifecha == day).ToList();

                    foreach (var potEmp in listaPotXDia)
                    {
                        MeMedicion48DTO iopEmp = new MeMedicion48DTO();
                        iopEmp.Emprcodi = potEmp.Emprcodi;
                        iopEmp.TipoFormulaMonitoreo = ConstantesMonitoreo.TipoFormulaIOP;
                        iopEmp.Medifecha = potEmp.Medifecha;

                        for (int i = 1; i <= 48; i++)
                        {
                            decimal? valorPE = (decimal?)potEmp.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).GetValue(potEmp, null);
                            decimal? valorTotalXH = (decimal?)total.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).GetValue(total, null);
                            decimal? valorMDXH = (decimal?)md.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).GetValue(md, null);

                            decimal? iop = valorPE.GetValueOrDefault(0) > (valorTotalXH.GetValueOrDefault(0) - valorMDXH.GetValueOrDefault(0))
                                ? ConstantesMonitoreo.ValorIOPEsPivotal : ConstantesMonitoreo.ValorIOPNoPivotal;
                            iopEmp.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).SetValue(iopEmp, iop);
                        }

                        listaIOPxDiaXEmpresa.Add(iopEmp);
                    }
                }

                listaFinal.AddRange(listaIOPxDiaXEmpresa);
                #endregion
            }

            if (ConstantesMonitoreo.CodigoRSD == indicador)
            {
                #region Oferta Residual
                for (DateTime day = fechaInicio.Date; day <= fechaFin.Date; day = day.AddDays(1))
                {
                    MeMedicion48DTO total = listaTotalXDia.Find(x => x.Medifecha == day);
                    MeMedicion48DTO md = listaMDXDia.Find(x => x.Medifecha == day);

                    var listaPotXDia = listaPotXDiaXEmpresa.Where(x => x.Medifecha == day).ToList();

                    foreach (var potEmp in listaPotXDia)
                    {
                        MeMedicion48DTO iorEmp = new MeMedicion48DTO();
                        iorEmp.Emprcodi = potEmp.Emprcodi;
                        iorEmp.TipoFormulaMonitoreo = ConstantesMonitoreo.TipoFormulaRSD;
                        iorEmp.Medifecha = day;

                        for (int i = 1; i <= 48; i++)
                        {
                            decimal? valorPE = (decimal?)potEmp.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).GetValue(potEmp, null);
                            decimal? valorTotalXH = (decimal?)total.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).GetValue(total, null);
                            decimal? valorMDXH = (decimal?)md.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).GetValue(md, null);

                            decimal ior = valorMDXH.GetValueOrDefault(0) != 0 ?
                                ((valorTotalXH.GetValueOrDefault(0) - valorPE.GetValueOrDefault(0)) / valorMDXH.GetValueOrDefault(0)) * 100 : 0;
                            iorEmp.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).SetValue(iorEmp, ior);
                        }

                        listaRSDxDiaXEmpresa.Add(iorEmp);
                    }
                }

                listaFinal.AddRange(listaRSDxDiaXEmpresa);
                #endregion

                #region Porcentaje de Error Banda de Tolerancia

                if (ConstantesMonitoreo.ReportePorcentajeErrorBandaTolerancia == tipoReporte)
                {
                    MmmBandtolDTO bt = this.GetBandaToleranciaByIndicadorYPeriodo(ConstantesMonitoreo.CodigoRSD, fechaInicio);
                    if (bt != null)
                    {
                        List<MeMedicion48DTO> listaRSDErrorBTxDiaXEmpresa = new List<MeMedicion48DTO>();
                        foreach (var regIndicador in listaRSDxDiaXEmpresa)
                        {
                            MeMedicion48DTO errorBT = new MeMedicion48DTO();
                            errorBT.Emprcodi = regIndicador.Emprcodi;
                            errorBT.TipoFormulaMonitoreo = ConstantesMonitoreo.TipoFormulaRSDErrorBT;
                            errorBT.Medifecha = regIndicador.Medifecha;

                            for (int i = 1; i <= 48; i++)
                            {
                                decimal? indicadorXH = (decimal?)regIndicador.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).GetValue(regIndicador, null);
                                if (indicadorXH.GetValueOrDefault(0) != 0)
                                {
                                    decimal? errorXH = (indicadorXH - bt.Mmmtolvalorreferencia / indicadorXH) * 100;
                                    errorBT.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).SetValue(errorBT, errorXH);
                                }
                            }
                            listaRSDErrorBTxDiaXEmpresa.Add(errorBT);
                        }

                        listaFinal.AddRange(listaRSDErrorBTxDiaXEmpresa);
                    }
                }

                #endregion
            }
            if (ConstantesMonitoreo.CodigoILE == indicador || ConstantesMonitoreo.CodigoIMU == indicador || ConstantesMonitoreo.CodigoIRT == indicador)
            {
                #region Costos Marginales Ejecutados

                List<BarraDTO> ListGrupoBarra = this.ListarGrupoBarra();
                listCostoMarginalEjec = this.ListarBarraTransferenciaM48(fechaInicio, fechaFin, listCostoMarginalEjec, ConstantesMonitoreo.TipoFormulaCMEjec, ListGrupoBarra);

                #endregion
            }

            if (ConstantesMonitoreo.CodigoILE == indicador || ConstantesMonitoreo.CodigoIMU == indicador)
            {
                #region Costo Variable X Barra de Transferencia
                for (DateTime day = fechaInicio.Date; day <= fechaFin.Date; day = day.AddDays(1))
                {
                    var listaPotXDia = listaPotXDiaEjecutado.Where(x => x.Medifecha == day).ToList();
                    var listaModoXDia = listModo30minByGrupo.Where(x => x.Medifecha == day).ToList();
                    var listCMgXDia = listCostoMarginalEjec.Where(x => x.Medifecha == day).ToList();
                    var listaCVXDia = listCostoVariables.Where(x => x.Medifecha == day).ToList();

                    var listaBarraYEmpresaXDia = listCostoMarginalEjec.Where(x => x.Barrcodi > 0).Select(x => new { x.Barrcodi, x.Emprcodi })
                            .GroupBy(x => new { x.Emprcodi, x.Barrcodi }).Select(x => new MeMedicion48DTO() { Emprcodi = x.Key.Emprcodi, Barrcodi = x.Key.Barrcodi }).ToList();

                    foreach (var regBarraEmp in listaBarraYEmpresaXDia)
                    {
                        if (regBarraEmp.Barrcodi == 334)
                        {
                        }

                        MeMedicion48DTO objCmg = listCMgXDia.Find(x => x.Barrcodi == regBarraEmp.Barrcodi && x.Emprcodi == regBarraEmp.Emprcodi);

                        MeMedicion48DTO cvBarra = new MeMedicion48DTO();
                        cvBarra.Emprcodi = regBarraEmp.Emprcodi;
                        cvBarra.Barrcodi = regBarraEmp.Barrcodi;
                        cvBarra.TipoFormulaMonitoreo = ConstantesMonitoreo.TipoFormulaCV;
                        cvBarra.Medifecha = day;

                        MeMedicion48DTO ileBarra = new MeMedicion48DTO();
                        ileBarra.Emprcodi = regBarraEmp.Emprcodi;
                        ileBarra.Barrcodi = regBarraEmp.Barrcodi;
                        ileBarra.TipoFormulaMonitoreo = ConstantesMonitoreo.TipoFormulaILE;
                        ileBarra.Medifecha = day;

                        MeMedicion48DTO imuBarra = new MeMedicion48DTO();
                        imuBarra.Emprcodi = regBarraEmp.Emprcodi;
                        imuBarra.Barrcodi = regBarraEmp.Barrcodi;
                        imuBarra.TipoFormulaMonitoreo = ConstantesMonitoreo.TipoFormulaIMU;
                        imuBarra.Medifecha = day;

                        var listaGrupocodi = listaPotXDia.Where(x => x.Barrcodi == regBarraEmp.Barrcodi && x.Emprcodi == regBarraEmp.Emprcodi).Select(x => x.Grupocodi).OrderBy(x => x).ToList();

                        for (int i = 1; i <= 48; i++)
                        {
                            //Calculo de Costo Variable x Barra
                            decimal? cvH = null;
                            int numMO = 0;
                            bool tienePotH = false;

                            foreach (var grupocodi in listaGrupocodi)
                            {
                                var potEjecGrupoXDia = listaPotXDia.Find(x => x.Grupocodi == grupocodi);
                                decimal? potEjec = null;
                                if (potEjecGrupoXDia != null)
                                {
                                    potEjec = (decimal?)potEjecGrupoXDia.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).GetValue(potEjecGrupoXDia, null);
                                }

                                if (potEjec.GetValueOrDefault(0) > 0)
                                {
                                    tienePotH = true;
                                    var regMO = listaModoXDia.Find(x => x.Grupocodi == grupocodi);
                                    var regCV = listaCVXDia.Find(x => x.Grupocodi == grupocodi);

                                    int grupocodiMO = 0;
                                    if (regMO != null)
                                    {
                                        grupocodiMO = Convert.ToInt32(((decimal?)regMO.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).GetValue(regMO, null)).GetValueOrDefault(0));
                                    }

                                    decimal? cvGrupo = null;
                                    if (regCV != null)
                                    {
                                        cvGrupo = (decimal?)regCV.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).GetValue(regCV, null);
                                    }

                                    if (grupocodiMO > 0)
                                    {
                                        cvH = cvH.GetValueOrDefault(0) + cvGrupo.GetValueOrDefault(0);
                                        numMO++;
                                    }
                                }
                            }

                            //Cv de la barra
                            if (tienePotH)
                            {
                                cvBarra.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).SetValue(cvBarra, 0.0m);
                            }

                            if (numMO > 0)
                            {
                                cvH = cvH.GetValueOrDefault(0) / numMO;
                                cvBarra.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).SetValue(cvBarra, cvH);
                                ileBarra.TieneIndicador = true;
                                imuBarra.TieneIndicador = true;
                            }

                            //Costo marginal Ejecutado
                            decimal? cmgH = null;
                            if (objCmg != null && tienePotH)
                            {
                                cmgH = (decimal?)objCmg.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).GetValue(objCmg, null);
                            }
                            else
                            {
                                objCmg.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).SetValue(objCmg, null);
                            }

                            //Calculo de ILE
                            if (objCmg != null && cmgH.GetValueOrDefault(0) != 0 && cvH != null && tienePotH)
                            {
                                decimal? ileH = (cmgH.GetValueOrDefault(0) - cvH.GetValueOrDefault(0)) / cmgH.GetValueOrDefault(0);
                                ileBarra.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).SetValue(ileBarra, ileH);
                            }

                            //Calculo de IMU
                            if (cvH != null && cvH.GetValueOrDefault(0) != 0 && cmgH != null && tienePotH)
                            {
                                decimal? imuH = (cmgH.GetValueOrDefault(0) - cvH.GetValueOrDefault(0)) / cvH.GetValueOrDefault(0);
                                imuBarra.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).SetValue(imuBarra, imuH);
                            }
                        }

                        listaCVxDiaXBarra.Add(cvBarra);
                        listaILExDiaXBarra.Add(ileBarra);
                        listaIMUxDiaXBarra.Add(imuBarra);
                    }
                }

                #endregion

                #region Porcentaje de Error Banda de Tolerancia

                if (ConstantesMonitoreo.ReportePorcentajeErrorBandaTolerancia == tipoReporte)
                {
                    MmmBandtolDTO bt = this.GetBandaToleranciaByIndicadorYPeriodo(ConstantesMonitoreo.CodigoILE, fechaInicio);
                    if (bt != null)
                    {
                        List<MeMedicion48DTO> listaILEErrorBTxDiaXBarra = new List<MeMedicion48DTO>();
                        foreach (var regIndicador in listaILExDiaXBarra)
                        {
                            MeMedicion48DTO errorBT = new MeMedicion48DTO();
                            errorBT.Emprcodi = regIndicador.Emprcodi;
                            errorBT.Barrcodi = regIndicador.Barrcodi;
                            errorBT.TipoFormulaMonitoreo = ConstantesMonitoreo.TipoFormulaILEErrorBT;
                            errorBT.Medifecha = regIndicador.Medifecha;

                            for (int i = 1; i <= 48; i++)
                            {
                                decimal? indicadorXH = (decimal?)regIndicador.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).GetValue(regIndicador, null);
                                if (indicadorXH.GetValueOrDefault(0) != 0)
                                {
                                    decimal? errorXH = (indicadorXH - bt.Mmmtolvalorreferencia / indicadorXH) * 100;
                                    errorBT.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).SetValue(errorBT, errorXH);
                                }
                            }
                            listaILEErrorBTxDiaXBarra.Add(errorBT);
                        }

                        listaFinal.AddRange(listaILEErrorBTxDiaXBarra);
                    }
                }

                #endregion

                #region Porcentaje de Error Banda de Tolerancia

                if (ConstantesMonitoreo.ReportePorcentajeErrorBandaTolerancia == tipoReporte)
                {
                    MmmBandtolDTO bt = this.GetBandaToleranciaByIndicadorYPeriodo(ConstantesMonitoreo.CodigoIMU, fechaInicio);
                    if (bt != null)
                    {
                        List<MeMedicion48DTO> listaIMUErrorBTxDiaXBarra = new List<MeMedicion48DTO>();
                        foreach (var regIndicador in listaIMUxDiaXBarra)
                        {
                            MeMedicion48DTO errorBT = new MeMedicion48DTO();
                            errorBT.Emprcodi = regIndicador.Emprcodi;
                            errorBT.Barrcodi = regIndicador.Barrcodi;
                            errorBT.TipoFormulaMonitoreo = ConstantesMonitoreo.TipoFormulaIMUErrorBT;
                            errorBT.Medifecha = regIndicador.Medifecha;

                            for (int i = 1; i <= 48; i++)
                            {
                                decimal? indicadorXH = (decimal?)regIndicador.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).GetValue(regIndicador, null);
                                if (indicadorXH.GetValueOrDefault(0) != 0)
                                {
                                    decimal? errorXH = (indicadorXH - bt.Mmmtolvalorreferencia / indicadorXH) * 100;
                                    errorBT.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).SetValue(errorBT, errorXH);
                                }
                            }
                            listaIMUErrorBTxDiaXBarra.Add(errorBT);
                        }

                        listaFinal.AddRange(listaIMUErrorBTxDiaXBarra);
                    }
                }

                #endregion
            }

            if (ConstantesMonitoreo.CodigoIRT == indicador)
            {
                listaFinal.AddRange(listaPotXDiaEjecutado);
                listaFinal.AddRange(listaPotXDiaProgramado);
                listaFinal.AddRange(listCostoMarginalProg);

                #region Congestiones
                DateTime fechaIni2 = new DateTime(fechaInicio.Year, fechaInicio.Month, 1);
                DateTime fechaFin2 = fechaIni2.AddMonths(1).AddDays(-1);
                List<EveCongesgdespachoDTO> listCongestiones = this.BuscarOperacionesCongestion(fechaIni2, fechaFin2, vermmcodi);

                List<EveCongesgdespachoDTO> listaCongXAreaXFecha = listCongestiones.GroupBy(x => new { x.Emprcodi, x.Emprnomb, x.Areanomb, x.Ichorini, x.Ichorfin })
                    .Select(x => new EveCongesgdespachoDTO() { Emprcodi = x.Key.Emprcodi, Emprnomb = x.Key.Emprnomb, Areanomb = x.Key.Areanomb, Ichorini = x.Key.Ichorini, Ichorfin = x.Key.Ichorfin }).ToList();

                foreach (var reg in listaCongXAreaXFecha)
                {
                    var listaEvento = listCongestiones.Where(x => x.Areanomb == reg.Areanomb && x.Ichorini == reg.Ichorini && x.Ichorfin == reg.Ichorfin).ToList();

                    List<string> equipos = listaEvento.Where(x => x.Equiabrev != null).Select(x => x.Equiabrev.Trim()).Distinct().ToList();
                    List<string> descripciones = listaEvento.Where(x => x.Icdescrip2 != null).Select(x => x.Icdescrip2.Trim()).Distinct().ToList();
                    List<string> descGrupo = listaEvento.Where(x => x.Gruponomb != null).Select(x => (x.Catecodi == 3? "C.T. " : "C.H. ") + x.Gruponomb.Trim()).Distinct().ToList();

                    MeMedicion48DTO cong = new MeMedicion48DTO();
                    cong.Hophorini = reg.Ichorini.Value;
                    cong.Hophorfin = reg.Ichorfin.Value;
                    cong.TipoFormulaMonitoreo = ConstantesMonitoreo.TipoFormulaCongestion;
                    cong.Areanomb = reg.Areanomb;
                    cong.Equinomb = string.Join(" y ", equipos);
                    cong.Descripcion = string.Join(". ", descripciones);
                    cong.Gruponomb = string.Join(". ", descGrupo);
                    cong.Emprcodi = reg.Emprcodi.Value;
                    cong.Emprnomb = reg.Emprnomb;

                    listaCongestion.Add(cong);
                }

                listaCongestion = listaCongestion.OrderBy(x => x.Hophorini).ToList();

                //Grupos despacho - congestion
                List<BarraDTO> ListGrupoBarra = this.ListarGrupoBarra();
                List<PrGrupoxcnfbarDTO> listGrupoBarraProg = this.ListarGrupoDespachoBarraProgramacion();

                var listaGrupo = listCongestiones.Where(x => x.Grupocodi > 0).GroupBy(x => new { x.Grupocodi, x.Grupopadre })
                    .Select(x => new { Grupocodi = x.Key.Grupocodi.Value, Grupopadre = x.Key.Grupopadre }).ToList();
                foreach (var grupo in listaGrupo)
                {
                    var conges = listCongestiones.Find(x => x.Grupocodi == grupo.Grupocodi);
                    var barra = ListGrupoBarra.Find(x => x.Grupocodi == grupo.Grupocodi || x.Grupopadre == grupo.Grupocodi);
                    var barraProg = listGrupoBarraProg.Find(x => x.Grupocodi == grupo.Grupocodi || x.Grupocodi == grupo.Grupopadre);
                    var listaCongesXGrupo = listCongestiones.Where(x => x.Grupocodi == grupo.Grupocodi).ToList();
                    List<string> equipos = listaCongesXGrupo.Where(x => x.Equiabrev != null).Select(x => x.Equiabrev.Trim()).Distinct().ToList();

                    MeMedicion48DTO reg = new MeMedicion48DTO();
                    reg.Grupocodi = grupo.Grupocodi;
                    reg.Emprcodi = conges.Emprcodi.Value;
                    reg.Emprnomb = conges.Emprnomb;
                    reg.TipoFormulaMonitoreo = ConstantesMonitoreo.TipoFormulaEnlaceTrans;
                    reg.Equinomb = string.Join(" y ", equipos);
                    reg.Gruponomb = conges.Gruponomb;

                    if (barra != null)
                    {
                        reg.Barrcodi = barra.BarrCodi;
                        reg.Barrnombre = barra.BarrNombre;
                    }

                    if (barraProg != null)
                    {
                        reg.Cnfbarcodi = barraProg.Cnfbarcodi;
                        reg.Cnfbarnombre = barraProg.Cnfbarnombre;
                    }

                    listaEnlaceTrans.Add(reg);

                    //Generar IRT
                    List<DateTime> listaFechaXGrupo = listaCongesXGrupo.Select(x => x.Ichorini.Value.Date).Distinct().OrderBy(x => x).ToList();

                    foreach (var day in listaFechaXGrupo)
                    {
                        MeMedicion48DTO objIRT = new MeMedicion48DTO();
                        objIRT.Medifecha = day;
                        objIRT.Grupocodi = grupo.Grupocodi;
                        objIRT.Emprcodi = conges.Emprcodi.Value;
                        objIRT.Emprnomb = conges.Emprnomb;
                        objIRT.TipoFormulaMonitoreo = ConstantesMonitoreo.TipoFormulaIRT;

                        var listaCongesXGrupoXDia = listaCongesXGrupo.Where(x => x.Ichorini.Value.Date == day).ToList();
                        foreach (var congGrupo in listaCongesXGrupoXDia)
                        {
                            DateTime dt1 = congGrupo.Ichorini.Value;
                            DateTime dt2 = congGrupo.Ichorfin.Value;

                            if (dt1 < day) dt1 = day;
                            if (dt2 > day.AddDays(1)) dt2 = day.AddDays(1);

                            int hi = (int)dt1.TimeOfDay.TotalMinutes / 30;
                            int hf = (dt2.TimeOfDay.TotalMinutes == 0) ? 48 : (int)dt2.TimeOfDay.TotalMinutes / 30;

                            hi = hi < 48 ? hi + 1 : hi;

                            if ((1 <= hi && hi <= 48) && hi <= hf && (1 <= hf && hf <= 48))
                            {
                                DateTime di = day.AddMinutes(30 * hi);
                                DateTime df = day.AddMinutes(30 * hf);

                                MeMedicion48DTO regpotEjec = this.GetPotenciaTotalXDiaByGrupoReporte7(day, listaPotXDiaEjecutado, grupo.Grupocodi);
                                MeMedicion48DTO regpotProg = this.GetPotenciaTotalXDiaByGrupoReporte7(day, listaPotXDiaProgramado, grupo.Grupocodi);
                                MeMedicion48DTO regCMgejec = listCostoMarginalEjec.Find(x => x.Medifecha == day && x.Barrcodi == reg.Barrcodi);
                                MeMedicion48DTO regCMgprog = listCostoMarginalProg.Find(x => x.Medifecha == day && x.Cnfbarcodi == reg.Cnfbarcodi);

                                for (int h = hi; h <= hf; h++)
                                {
                                    decimal? potEjec = null;
                                    if (regpotEjec != null)
                                    {
                                        potEjec = (decimal?)regpotEjec.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regpotEjec, null);
                                    }

                                    decimal? potProg = null;
                                    if (regpotProg != null)
                                    {
                                        potProg = (decimal?)regpotProg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regpotProg, null);
                                    }

                                    decimal? cmg = null;
                                    if (regCMgejec != null)
                                    {
                                        cmg = (decimal?)regCMgejec.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regCMgejec, null);
                                        if (cmg != null)
                                            cmg = cmg * 1000;
                                    }

                                    decimal? cmgProg = null;
                                    if (regCMgprog != null && potProg != null)
                                    {
                                        cmgProg = (decimal?)regCMgprog.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regCMgprog, null);
                                    }

                                    decimal? irt = 0;
                                    if (potProg.GetValueOrDefault(0) != 0 && cmgProg.GetValueOrDefault(0) != 0)
                                    {
                                        irt = (potEjec.GetValueOrDefault(0) * cmg.GetValueOrDefault(0)) / (potProg.GetValueOrDefault(0) * cmgProg.GetValueOrDefault(0));
                                    }
                                    objIRT.GetType().GetProperty(ConstantesAppServicio.CaracterH + (h)).SetValue(objIRT, irt);
                                }
                            }
                            else
                            {
                            }
                        }

                        listaIRT.Add(objIRT);
                    }
                }

                listaFinal.AddRange(listaCongestion);
                listaFinal.AddRange(listaEnlaceTrans);
                listaFinal.AddRange(listaIRT);

                #endregion

                #region Porcentaje de Error Banda de Tolerancia

                if (ConstantesMonitoreo.ReportePorcentajeErrorBandaTolerancia == tipoReporte)
                {
                    MmmBandtolDTO bt = this.GetBandaToleranciaByIndicadorYPeriodo(ConstantesMonitoreo.CodigoIRT, fechaInicio);
                    if (bt != null)
                    {
                        List<MeMedicion48DTO> listaIRTErrorBTXDia = new List<MeMedicion48DTO>();
                        foreach (var regIndicador in listaIRT)
                        {
                            MeMedicion48DTO errorBT = new MeMedicion48DTO();
                            errorBT.Grupocodi = regIndicador.Grupocodi;
                            errorBT.Emprcodi = regIndicador.Emprcodi;
                            errorBT.TipoFormulaMonitoreo = ConstantesMonitoreo.TipoFormulaIRTErrorBT;
                            errorBT.Medifecha = regIndicador.Medifecha;

                            for (int i = 1; i <= 48; i++)
                            {
                                decimal? indicadorXH = (decimal?)regIndicador.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).GetValue(regIndicador, null);
                                if (indicadorXH.GetValueOrDefault(0) != 0)
                                {
                                    decimal? errorXH = (indicadorXH - bt.Mmmtolvalorreferencia / indicadorXH) * 100;
                                    errorBT.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).SetValue(errorBT, errorXH);
                                }
                            }
                            listaIRTErrorBTXDia.Add(errorBT);
                        }

                        listaFinal.AddRange(listaIRTErrorBTXDia);
                    }
                }

                #endregion
            }

            listaFinal.AddRange(listaPotXDiaXEmpresa);
            listaFinal.AddRange(listaProgXDiaXEmpresa);
            listaFinal.AddRange(listaTotalXDia);

            listaFinal.AddRange(listaCuotaXDiaXEmpresa);

            listaFinal.AddRange(listaHHIXDia);
            listaFinal.AddRange(listaHHICuotaXDiaXEmpresa);

            listaFinal.AddRange(listaMDXDia);

            listaFinal.AddRange(listCostoMarginalEjec);
            listaFinal.AddRange(listaCVxDiaXBarra);
            listaFinal.AddRange(listaILExDiaXBarra);
            listaFinal.AddRange(listaIMUxDiaXBarra);


            return listaFinal;
        }

        /// <summary>
        /// Obtener total x grupo o padre
        /// </summary>
        /// <param name="day"></param>
        /// <param name="lista"></param>
        /// <param name="grupocodi"></param>
        /// <returns></returns>
        private MeMedicion48DTO GetPotenciaTotalXDiaByGrupoReporte7(DateTime day, List<MeMedicion48DTO> lista, int grupocodi)
        {
            var listaGrupo = lista.Where(x => x.Medifecha == day && (x.Grupocodi == grupocodi || x.Grupopadre == grupocodi)).ToList();
            MeMedicion48DTO regTotal = null;
            if (listaGrupo.Count > 0)
            {
                regTotal = new MeMedicion48DTO();
                regTotal.Medifecha = day;
                regTotal.Grupocodi = grupocodi;

                foreach (var regtmp in listaGrupo)
                {
                    for (int i = 1; i <= 48; i++)
                    {
                        decimal? valorNuevo = (decimal?)regtmp.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).GetValue(regtmp, null);
                        decimal? valorAcum = (decimal?)regTotal.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).GetValue(regTotal, null);
                        if (valorNuevo != null)
                        {
                            valorAcum = valorAcum.GetValueOrDefault(0) + valorNuevo;
                            regTotal.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).SetValue(regTotal, valorAcum);
                        }
                    }
                }
            }

            return regTotal;
        }

        /// <summary>
        /// Lista de congestiones Reporte 7
        /// </summary>
        /// <param name="listaCongXAreaXFecha"></param>
        /// <returns></returns>
        private List<MeMedicion48DTO> ListarCongXAreaYFechaReporte7(List<MeMedicion48DTO> listaCongXAreaXFecha)
        {
            var listaCong = listaCongXAreaXFecha.GroupBy(x => new { x.Areanomb, x.Hophorini, x.Hophorfin })
                    .Select(x => new MeMedicion48DTO() { Areanomb = x.Key.Areanomb, Hophorini = x.Key.Hophorini, Hophorfin = x.Key.Hophorfin }).ToList();

            foreach (var reg in listaCong)
            {
                var regTmp = listaCongXAreaXFecha.Find(x => x.Areanomb == reg.Areanomb && x.Hophorini == reg.Hophorini && x.Hophorfin == reg.Hophorfin);
                reg.Equinomb = regTmp.Equinomb;
                reg.Descripcion = regTmp.Descripcion;
                reg.Gruponomb = regTmp.Gruponomb;
            }

            return listaCong;
        }

        /// <summary>
        /// Listar barra de transferencia para todos los dias y empresas
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="listCostoMarginalEjec"></param>
        /// <param name="tipo"></param>
        /// <param name="listGrupoBarra"></param>
        /// <returns></returns>
        private List<MeMedicion48DTO> ListarBarraTransferenciaM48(DateTime fechaInicio, DateTime fechaFin, List<MeMedicion48DTO> listCostoMarginalEjec, int tipo, List<BarraDTO> listGrupoBarra)
        {
            List<MeMedicion48DTO> listaFinal = new List<MeMedicion48DTO>();

            var listaBarraYEmpresa = listGrupoBarra.Where(x => x.BarrCodi > 0).Select(x => new { x.BarrCodi, x.Emprcodi })
                    .GroupBy(x => new { x.Emprcodi, x.BarrCodi }).Select(x => new MeMedicion48DTO() { Emprcodi = x.Key.Emprcodi, Barrcodi = x.Key.BarrCodi }).ToList();

            var listaBarraYEmpresaMes = listCostoMarginalEjec.Where(x => x.Barrcodi > 0).Select(x => new { x.Barrcodi, x.Emprcodi })
                    .GroupBy(x => new { x.Emprcodi, x.Barrcodi }).Select(x => new MeMedicion48DTO() { Emprcodi = x.Key.Emprcodi, Barrcodi = x.Key.Barrcodi }).ToList();

            // Union de EmpreCodi en una sola Lista 
            listaBarraYEmpresa.AddRange(listaBarraYEmpresaMes);
            listaBarraYEmpresa = listaBarraYEmpresa.Where(x => x.Barrcodi > 0).GroupBy(x => new { x.Emprcodi, x.Barrcodi })
                .Select(x => new MeMedicion48DTO() { Emprcodi = x.Key.Emprcodi, Barrcodi = x.Key.Barrcodi }).OrderBy(x => x.Barrcodi).ToList();

            List<BarraDTO> listGrupoBarraAll = FactoryTransferencia.GetBarraRepository().List();

            //Recorrido de dias
            for (DateTime day = fechaInicio.Date; day <= fechaFin.Date; day = day.AddDays(1))
            {
                var listCostoMarginalEjecByDia = listCostoMarginalEjec.Where(x => x.Medifecha == day).ToList();
                foreach (var barraEmp in listaBarraYEmpresa)
                {
                    BarraDTO barra = listGrupoBarra.Find(x => x.BarrCodi == barraEmp.Barrcodi);
                    BarraDTO barraHist = listGrupoBarraAll.Find(x => x.BarrCodi == barraEmp.Barrcodi);
                    MeMedicion48DTO barraXDia = listCostoMarginalEjecByDia.Find(x => x.Emprcodi == barraEmp.Emprcodi && x.Barrcodi == barraEmp.Barrcodi);

                    if (barraEmp.Barrcodi == 334)
                    {
                    }

                    if (barraXDia != null)
                    {
                        if (barra != null)
                        {
                            barraXDia.Barrnombre = barra.BarrNombre;
                            barraXDia.Emprnomb = barra.Emprnomb;
                        }
                        else
                        {
                            if (barraHist != null)
                            {
                                barraXDia.Barrnombre = barraHist.BarrNombre;
                            }
                            else
                            {
                                //no existe barra en BD
                            }
                        }
                    }
                    else
                    {
                        barraXDia = new MeMedicion48DTO();
                        barraXDia.Medifecha = day;
                        barraXDia.Barrcodi = barraEmp.Barrcodi;
                        barraXDia.Emprcodi = barraEmp.Emprcodi;
                        if (barra != null)
                        {
                            barraXDia.Barrnombre = barra.BarrNombre;
                            barraXDia.Emprnomb = barra.Emprnomb;
                        }
                        else
                        {
                            if (barraHist != null)
                            {
                                barraXDia.Barrnombre = barraHist.BarrNombre;
                            }
                            else
                            {
                                //no existe barra en BD
                            }
                        }
                    }

                    barraXDia.TipoFormulaMonitoreo = tipo;

                    listaFinal.Add(barraXDia);
                }
            }

            return listaFinal;
        }

        /// <summary>
        /// Listar Potencia por Empresa cada 30Minutos
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="listaPotXDia"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        private List<MeMedicion48DTO> ListarPotenciaXEmpresaM48(DateTime fechaInicio, DateTime fechaFin, List<MeMedicion48DTO> listaPotXDia, int tipo, List<SiEmpresaDTO> listaEmpresa)
        {
            List<MeMedicion48DTO> listaFinal = new List<MeMedicion48DTO>();
            List<int> listaEmprcodi = listaPotXDia.Select(x => x.Emprcodi).Distinct().ToList();
            List<int> listaEmprcodiMes = listaEmpresa.Select(x => x.Emprcodi).Distinct().ToList();
            listaEmprcodi.AddRange(listaEmprcodiMes);
            listaEmprcodi = listaEmprcodi.Distinct().ToList();

            //Recorrido de dias
            for (DateTime day = fechaInicio.Date; day <= fechaFin.Date; day = day.AddDays(1))
            {
                foreach (int emprcodi in listaEmprcodi)
                {
                    //Lista de empresa por dia 
                    var listaXEmpresaYDia = listaPotXDia.Where(x => x.Medifecha == day && x.Emprcodi == emprcodi);
                    MeMedicion48DTO regTotal = new MeMedicion48DTO();
                    regTotal.Medifecha = day;
                    regTotal.Emprcodi = emprcodi;
                    regTotal.TipoFormulaMonitoreo = tipo;
                    decimal? acum = null;
                    decimal total = 0;

                    for (int i = 1; i <= 48; i++)
                    {
                        acum = null;
                        foreach (var regtmp in listaXEmpresaYDia)
                        {
                            decimal? valorNuevo = (decimal?)regtmp.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).GetValue(regtmp, null);
                            if (valorNuevo != null)
                            {
                                acum = acum.GetValueOrDefault(0) + valorNuevo;
                            }
                        }

                        regTotal.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).SetValue(regTotal, acum);
                        total = total + acum.GetValueOrDefault(0);
                    }

                    regTotal.Meditotal = total;

                    listaFinal.Add(regTotal);
                }
            }

            return listaFinal;
        }

        /// <summary>
        /// Buscar las congestiones registradas
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        public List<EveCongesgdespachoDTO> BuscarOperacionesCongestion(DateTime fechaInicio, DateTime fechaFinal, int vermmcodi)
        {
            MmmVersionDTO objVersion = this.GetByIdMmmVersion(vermmcodi);
            List<EveCongesgdespachoDTO> listaData = FactorySic.GetEveCongesgdespachoRepository().BuscarOperacionesCongestion(fechaInicio, fechaFinal);
            listaData = listaData.Where(x => x.Congdefeccreacion <= objVersion.Vermmfeccreacion).ToList();

            return listaData;
        }

        /// <summary>
        /// Obtener Tendencia HHI
        /// </summary>
        /// <param name="fechaPeriodo"></param>
        /// <returns></returns>
        public SiParametroValorDTO GetParametroTendenciaHHI(DateTime fechaPeriodo)
        {
            SiParametroValorDTO param = this.servParametro.GetParametroTendenciaHHI(fechaPeriodo);
            param.HHITendenciaUno = param.HHITendenciaUno * param.HHITendenciaUno * ConstantesMonitoreo.FactorHHI;
            param.HHITendenciaCero = param.HHITendenciaCero * param.HHITendenciaCero * ConstantesMonitoreo.FactorHHI;
            param.HHITendenciaUnoColor = ConstantesMonitoreo.ColorHHITendenciaUno;
            param.HHITendenciaCeroColor = ConstantesMonitoreo.ColorHHITendenciaCero;

            return param;
        }

        /// <summary>
        /// Obtener Parametro Oferta Pivotal
        /// </summary>
        /// <param name="fechaPeriodo"></param>
        /// <returns></returns>
        public SiParametroValorDTO GetParametroOfertaPivotal(DateTime fechaPeriodo)
        {
            SiParametroValorDTO param = new SiParametroValorDTO();
            param.IOPEsPivotal = ConstantesMonitoreo.ValorIOPEsPivotal;
            param.IOPNoPivotal = ConstantesMonitoreo.ValorIOPNoPivotal;
            param.IOPEsPivotalColor = ConstantesMonitoreo.ColorIOPEsPivotal;
            param.IOPNoPivotalColor = ConstantesMonitoreo.ColorIOPNoPivotal;

            return param;
        }

        /// <summary>
        /// Visualizar reporte Control de cambio Html segun el tipo de Indicador
        /// </summary>
        /// <param name="tipoIndicador"></param>
        /// <param name="empresas"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="vermmcodi"></param>
        /// <returns></returns>
        public List<string> ReporteIndicadorByTipoHtml(int tipoIndicador, string empresas, DateTime fechaInicio, DateTime fechaFin, int vermmcodi
            , int tipoReporte, string idBarra, string url, out List<MmmJustificacionDTO> listJustif)
        {
            List<string> listaResultado = new List<string>();
            listJustif = new List<MmmJustificacionDTO>();
            List<MmmJustificacionDTO> listJustifByIndicador = new List<MmmJustificacionDTO>();
            List<MmmDatoDTO> listFacTable = FactorySic.GetMmmDatoRepository().ListPeriodo(fechaInicio.AddMinutes(30), fechaFin.AddDays(1));

            switch (tipoReporte)
            {
                case ConstantesMonitoreo.ReportesIndicadores:
                    if (tipoIndicador == -1 || tipoIndicador == ConstantesMonitoreo.CodigoS)
                        //Reporte Indicador 1 
                        listaResultado.Add(this.ReporteIndicador1Html(empresas, fechaInicio, fechaFin, vermmcodi, listFacTable));
                    if (tipoIndicador == -1 || tipoIndicador == ConstantesMonitoreo.CodigoHHI)
                        //Reporte Indicador 2
                        listaResultado.Add(this.ReporteIndicador2Html(empresas, fechaInicio, fechaFin, vermmcodi, listFacTable));
                    if (tipoIndicador == -1 || tipoIndicador == ConstantesMonitoreo.CodigoIOP)
                        //Reporte Indicador 3
                        listaResultado.Add(this.ReporteIndicador3Html(empresas, fechaInicio, fechaFin, vermmcodi, listFacTable));
                    if (tipoIndicador == -1 || tipoIndicador == ConstantesMonitoreo.CodigoRSD)
                        //Reporte Indicador 4
                        listaResultado.Add(this.ReporteIndicador4Html(empresas, fechaInicio, fechaFin, vermmcodi, listFacTable));
                    if (tipoIndicador == -1 || tipoIndicador == ConstantesMonitoreo.CodigoILE)
                        //Reporte Indicador 5
                        listaResultado.Add(this.ReporteIndicador5Html(empresas, fechaInicio, fechaFin, vermmcodi, listFacTable));
                    if (tipoIndicador == -1 || tipoIndicador == ConstantesMonitoreo.CodigoIMU)
                        //Reporte Indicador 6
                        listaResultado.Add(this.ReporteIndicador6Html(empresas, fechaInicio, fechaFin, vermmcodi, listFacTable));
                    if (tipoIndicador == -1 || tipoIndicador == ConstantesMonitoreo.CodigoIRT)
                        //Reporte Indicador 7
                        listaResultado.Add(this.ReporteIndicador7Html(empresas, fechaInicio, fechaFin, vermmcodi, listFacTable));
                    break;
                case ConstantesMonitoreo.LogErroresMonitoreo:
                    if (tipoIndicador == -1 || tipoIndicador == ConstantesMonitoreo.CodigoS)
                        //Reporte Log Errores Indicador 1 
                        listaResultado.Add(this.LogErrorIndicador1Html(empresas, fechaInicio, fechaFin, vermmcodi, listFacTable));
                    if (tipoIndicador == -1 || tipoIndicador == ConstantesMonitoreo.CodigoHHI)
                        //Reporte Log Errores Indicador 2
                        listaResultado.Add(this.LogErrorIndicador2Html(empresas, fechaInicio, fechaFin, vermmcodi, listFacTable));
                    if (tipoIndicador == -1 || tipoIndicador == ConstantesMonitoreo.CodigoIOP)
                        //Reporte Log Errores Indicador 3
                        listaResultado.Add(this.LogErrorIndicador3Html(empresas, fechaInicio, fechaFin, vermmcodi, listFacTable));
                    if (tipoIndicador == -1 || tipoIndicador == ConstantesMonitoreo.CodigoRSD)
                        //Reporte Log Errores Indicador 4
                        listaResultado.Add(this.LogErrorIndicador4Html(empresas, fechaInicio, fechaFin, vermmcodi, listFacTable));
                    if (tipoIndicador == -1 || tipoIndicador == ConstantesMonitoreo.CodigoILE)
                        //Reporte Log Errores Indicador 5
                        listaResultado.Add(this.LogErrorIndicador5Html(empresas, fechaInicio, fechaFin, vermmcodi, listFacTable));
                    if (tipoIndicador == -1 || tipoIndicador == ConstantesMonitoreo.CodigoIMU)
                        //Reporte Log Errores Indicador 6
                        listaResultado.Add(this.LogErrorIndicador6Html(empresas, fechaInicio, fechaFin, vermmcodi, listFacTable));
                    if (tipoIndicador == -1 || tipoIndicador == ConstantesMonitoreo.CodigoIRT)
                        //Reporte Log Errores Indicador 7
                        listaResultado.Add(this.LogErrorIndicador7Html(empresas, fechaInicio, fechaFin, vermmcodi, listFacTable));
                    break;
                case ConstantesMonitoreo.ControlCambiosMonitoreo:
                    if (tipoIndicador == -1 || tipoIndicador == ConstantesMonitoreo.CodigoS)
                        //Reporte Control de Cambios Indicador 1 
                        listaResultado.Add(this.ControlCambiosIndicador1Html(empresas, fechaInicio, fechaFin, vermmcodi, listFacTable));
                    if (tipoIndicador == -1 || tipoIndicador == ConstantesMonitoreo.CodigoHHI)
                        //Reporte Control de Cambios Indicador 2
                        listaResultado.Add(this.ControlCambiosIndicador2Html(empresas, fechaInicio, fechaFin, vermmcodi, listFacTable));
                    if (tipoIndicador == -1 || tipoIndicador == ConstantesMonitoreo.CodigoIOP)
                        //Reporte Control de Cambios Indicador 3
                        listaResultado.Add(this.ControlCambiosIndicador3Html(empresas, fechaInicio, fechaFin, vermmcodi, listFacTable));
                    if (tipoIndicador == -1 || tipoIndicador == ConstantesMonitoreo.CodigoRSD)
                        //Reporte Control de Cambios Indicador 4
                        listaResultado.Add(this.ControlCambiosIndicador4Html(empresas, fechaInicio, fechaFin, vermmcodi, listFacTable));
                    if (tipoIndicador == -1 || tipoIndicador == ConstantesMonitoreo.CodigoILE)
                        //Reporte Control de Cambios Indicador 5
                        listaResultado.Add(this.ControlCambiosIndicador5Html(empresas, fechaInicio, fechaFin, vermmcodi, listFacTable, idBarra));
                    if (tipoIndicador == -1 || tipoIndicador == ConstantesMonitoreo.CodigoIMU)
                        //Reporte Control de Cambios Indicador 6
                        listaResultado.Add(this.ControlCambiosIndicador6Html(empresas, fechaInicio, fechaFin, vermmcodi, listFacTable, idBarra));
                    if (tipoIndicador == -1 || tipoIndicador == ConstantesMonitoreo.CodigoIRT)
                        //Reporte Control de Cambios Indicador 7
                        listaResultado.Add(this.ControlCambiosIndicador7Html(empresas, fechaInicio, fechaFin, vermmcodi, listFacTable, idBarra));
                    break;
                case ConstantesMonitoreo.ReportePorcentajeErrorBandaTolerancia:
                    if (tipoIndicador == -1 || tipoIndicador == ConstantesMonitoreo.CodigoS)
                    {
                        //Reporte Indicador 1 
                        listaResultado.Add(this.ReportePorcentajeErrorBT1Html(empresas, fechaInicio, fechaFin, vermmcodi, listFacTable, url, out listJustifByIndicador));
                        listJustif.AddRange(listJustifByIndicador);
                    }
                    if (tipoIndicador == -1 || tipoIndicador == ConstantesMonitoreo.CodigoHHI)
                    {
                        //Reporte Indicador 2
                        listaResultado.Add(this.ReportePorcentajeErrorBT2Html(empresas, fechaInicio, fechaFin, vermmcodi, listFacTable, url, out listJustifByIndicador));
                        listJustif.AddRange(listJustifByIndicador);
                    }
                    if (tipoIndicador == -1 || tipoIndicador == ConstantesMonitoreo.CodigoIOP)
                    {
                        //Reporte Indicador 3
                        listaResultado.Add(this.ReportePorcentajeErrorBT3Html(empresas, fechaInicio, fechaFin, vermmcodi, listFacTable, url, out listJustifByIndicador));
                        listJustif.AddRange(listJustifByIndicador);
                    }
                    if (tipoIndicador == -1 || tipoIndicador == ConstantesMonitoreo.CodigoRSD)
                    {
                        //Reporte Indicador 4
                        listaResultado.Add(this.ReportePorcentajeErrorBT4Html(empresas, fechaInicio, fechaFin, vermmcodi, listFacTable, url, out listJustifByIndicador));
                        listJustif.AddRange(listJustifByIndicador);
                    }
                    if (tipoIndicador == -1 || tipoIndicador == ConstantesMonitoreo.CodigoILE)
                    {
                        //Reporte Indicador 5
                        listaResultado.Add(this.ReportePorcentajeErrorBT5Html(empresas, fechaInicio, fechaFin, vermmcodi, listFacTable, url, out listJustifByIndicador));
                        listJustif.AddRange(listJustifByIndicador);
                    }
                    if (tipoIndicador == -1 || tipoIndicador == ConstantesMonitoreo.CodigoIMU)
                    {
                        //Reporte Indicador 6
                        listaResultado.Add(this.ReportePorcentajeErrorBT6Html(empresas, fechaInicio, fechaFin, vermmcodi, listFacTable, url, out listJustifByIndicador));
                        listJustif.AddRange(listJustifByIndicador);
                    }
                    if (tipoIndicador == -1 || tipoIndicador == ConstantesMonitoreo.CodigoIRT)
                    {
                        //Reporte Indicador 7
                        listaResultado.Add(this.ReportePorcentajeErrorBT7Html(empresas, fechaInicio, fechaFin, vermmcodi, listFacTable, url, out listJustifByIndicador));
                        listJustif.AddRange(listJustifByIndicador);
                    }
                    break;
            }
            return listaResultado;
        }

        /// <summary>
        /// Obtener Html del Indicador 1
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        /// 
        public string ReporteIndicador1Html(string empresas, DateTime fechaInicio, DateTime fechaFin, int vermmcodi, List<MmmDatoDTO> listFacTable)
        {
            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";

            List<SiEmpresaDTO> listaEmpresa;
            List<MeMedicion48DTO> data = this.ListarIndicador(vermmcodi, fechaInicio, fechaFin, ConstantesMonitoreo.CodigoS, listFacTable, ConstantesMonitoreo.ReportesIndicadores, out listaEmpresa);
            listaEmpresa = this.ListarEmpresasMonitoreoByData(listaEmpresa, empresas);

            if (listaEmpresa.Count == 0) return string.Empty;

            int padding = 20;
            int anchoTotal = (100 + padding) + listaEmpresa.Count * (200 + padding);

            strHtml.Append("<div class='freeze_table' id='resultado1' style='height: 800px;'>");
            strHtml.AppendFormat("<table id='reporte1' class='pretty tabla-icono' style='table-layout: fixed; width: {0}px;' >", anchoTotal);
            strHtml.Append("<thead>");
            #region cabecera
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width: 110px;' rowspan=2> MM/DD/hh:mm</th>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px' rowspan=2> PES </th>");
            for (int i = 1; i <= 2; i++)
            {
                foreach (var datos in listaEmpresa)
                {
                    if (datos.Emprcodi == 12708)
                    {

                    }
                    strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 100px' class='cuota_{1}'>{0}</th>", datos.Emprnomb, i);
                }
            }
            strHtml.Append("</tr>");
            strHtml.Append("<tr>");

            for (int i = 1; i <= 2; i++)
            {
                string valor = (i == 1) ? " PE (MW)" : " S (%)";
                foreach (var datos in listaEmpresa)
                {
                    strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 100px' class='cuota_{1}'>{0}</th>", valor, i);
                }
            }
            strHtml.Append("</tr>");
            #endregion
            strHtml.Append("</thead>");
            strHtml.Append("<tbody>");
            #region cuerpo
            // Día - Hora

            for (var day = fechaInicio.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
            {
                MeMedicion48DTO total = data.Find(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaTotalPotencia);

                //HORA
                DateTime horas = day.AddMinutes(30);

                for (int h = 1; h <= 48; h++)
                {
                    strHtml.Append("<tr>");
                    strHtml.Append(string.Format("<td class='tdbody_reporte'>{0:dd/MM/yyyy HH:mm}</td>", horas));

                    //Potencia total
                    decimal? potenciaTotal = null;
                    if (total != null)
                    {
                        potenciaTotal = (decimal?)total.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(total, null);
                    }
                    strHtml.Append(string.Format("<td>{0}</td>", potenciaTotal != null ? (potenciaTotal.Value).ToString("N", nfi) : string.Empty));

                    for (int i = 1; i <= 2; i++)
                    {
                        foreach (var empresa in listaEmpresa)
                        {
                            //Potencia / Cuota
                            MeMedicion48DTO reg = data.Find(x => x.Medifecha == day && x.Emprcodi == empresa.Emprcodi && x.TipoFormulaMonitoreo == i);
                            decimal? pot = null;
                            if (reg != null)
                            {
                                pot = (decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg, null);
                            }
                            strHtml.Append(string.Format("<td>{0}</td>", pot != null && pot.GetValueOrDefault(0) != 0 ? (pot.Value).ToString("N", nfi) : string.Empty));
                        }
                    }
                    strHtml.Append("</tr>");
                    horas = horas.AddMinutes(30);
                }
            }

            #endregion
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Html indicador 2
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public string ReporteIndicador2Html(string empresas, DateTime fechaInicio, DateTime fechaFin, int vermmcodi, List<MmmDatoDTO> listFacTable)
        {
            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";

            List<SiEmpresaDTO> listaEmpresa;
            List<MeMedicion48DTO> data = this.ListarIndicador(vermmcodi, fechaInicio, fechaFin, ConstantesMonitoreo.CodigoHHI, listFacTable, ConstantesMonitoreo.ReportesIndicadores, out listaEmpresa);
            listaEmpresa = this.ListarEmpresasMonitoreoByData(listaEmpresa, empresas);

            SiParametroValorDTO param = this.GetParametroTendenciaHHI(fechaInicio);
            decimal tendenciaUno = param.HHITendenciaUno.GetValueOrDefault(0);
            decimal tendenciaCero = param.HHITendenciaCero.GetValueOrDefault(0);
            string colorUno = param.HHITendenciaUnoColor;
            string colorCero = param.HHITendenciaCeroColor;

            if (listaEmpresa.Count == 0) return string.Empty;

            int padding = 20;
            int anchoTotal = (100 + padding) + listaEmpresa.Count * (200 + padding);

            strHtml.Append("<div class='freeze_table' id='resultado2' style='height: auto;'>");
            strHtml.AppendFormat("<table id='reporte2' class='pretty tabla-icono' style='table-layout: fixed; width: {0}px;' >", anchoTotal);
            strHtml.Append("<thead>");
            #region cabecera
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width: 110px;' rowspan=2>MM/DD/hh:mm</th>");
            foreach (var datos in listaEmpresa)
            {
                strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px'>" + datos.Emprnomb + "</th>");
            }
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px;' rowspan=2> HHI</th>");
            strHtml.Append("</tr>");
            strHtml.Append("<tr>");

            // Cabecera cuota de mercado al cuadrado
            foreach (var datos in listaEmpresa)
            {
                strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px'> S  <sup>2</sup> </th>");
            }
            strHtml.Append("</tr>");
            #endregion
            strHtml.Append("</thead>");
            strHtml.Append("<tbody>");
            #region cuerpo
            for (var day = fechaInicio.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
            {
                MeMedicion48DTO total = data.Find(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaHHI);

                DateTime horas = day.AddMinutes(30);
                for (int h = 1; h <= 48; h++)
                {
                    strHtml.Append("<tr>");
                    strHtml.Append(string.Format("<td class='tdbody_reporte'>{0:dd/MM/yyyy HH:mm}</td>", horas));

                    foreach (var empresa in listaEmpresa)
                    {
                        // Cuota para HHI
                        MeMedicion48DTO reg = data.Find(x => x.Medifecha == day && x.Emprcodi == empresa.Emprcodi && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaHHICuotaMercado);
                        decimal? cuotaHHI = null;
                        if (reg != null)
                        {
                            cuotaHHI = (decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg, null);
                        }
                        strHtml.Append(string.Format("<td>{0}</td>", cuotaHHI.GetValueOrDefault(0) != 0 ? cuotaHHI.Value.ToString("N", nfi) : string.Empty));
                    }

                    decimal totalHhi = 0;
                    if (total != null)
                    {
                        totalHhi = ((decimal?)total.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(total, null)).GetValueOrDefault(0);
                    }

                    if (totalHhi >= tendenciaUno)
                    {
                        strHtml.AppendFormat("<td  style='background-color: {1}; color:White;'>{0}</td>", totalHhi != 0 ? totalHhi.ToString("N", nfi) : string.Empty, colorUno);
                    }
                    else if (totalHhi <= tendenciaCero)
                    {
                        strHtml.AppendFormat("<td  style='background-color: {1}; color:White;'>{0}</td>", totalHhi != 0 ? totalHhi.ToString("N", nfi) : string.Empty, colorCero);
                    }
                    else
                    {
                        strHtml.AppendFormat("<td >{0}</td>", totalHhi != 0 ? totalHhi.ToString("N", nfi) : string.Empty);
                    }

                    strHtml.Append("</tr>");
                    horas = horas.AddMinutes(30);
                }
            }
            #endregion
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Html indicador 3
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public string ReporteIndicador3Html(string empresas, DateTime fechaInicio, DateTime fechaFin, int vermmcodi, List<MmmDatoDTO> listFacTable)
        {
            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";

            List<SiEmpresaDTO> listaEmpresa;
            List<MeMedicion48DTO> data = this.ListarIndicador(vermmcodi, fechaInicio, fechaFin, ConstantesMonitoreo.CodigoIOP, listFacTable, ConstantesMonitoreo.ReportesIndicadores, out listaEmpresa);
            listaEmpresa = this.ListarEmpresasMonitoreoByData(listaEmpresa, empresas);

            SiParametroValorDTO param = this.GetParametroOfertaPivotal(fechaInicio);
            string colorEsPivotal = param.IOPEsPivotalColor;
            string colorNoPivotal = param.IOPNoPivotalColor;

            if (listaEmpresa.Count == 0) return string.Empty;

            int padding = 20;
            int anchoTotal = (100 + padding) + listaEmpresa.Count * (200 + padding);

            strHtml.Append("<div class='freeze_table' id='resultado3' style='height: auto;'>");
            strHtml.AppendFormat("<table id='reporte3' class='pretty tabla-icono' style='table-layout: fixed; width: {0}px;' >", anchoTotal);
            strHtml.Append("<thead>");
            #region cabecera
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width: 110px;' rowspan=2>MM/DD/hh:mm</th>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px' rowspan=2 > MD  (MW)</th>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px' rowspan=2> PES (MW) </th>");
            for (int i = 1; i <= 2; i++)
            {
                foreach (var datos in listaEmpresa)
                {
                    strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px'>" + datos.Emprnomb + "</th>");
                }
            }

            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            for (int i = 1; i <= 2; i++)
            {
                foreach (var datos in listaEmpresa)
                {
                    string valor; valor = (i == 1) ? "PE (MW)" : "IOP";
                    strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px'> " + valor + " </th>");
                }
            }
            strHtml.Append("</tr>");
            #endregion
            strHtml.Append("</thead>");
            strHtml.Append("<tbody>");
            #region cuerpo
            for (var day = fechaInicio.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
            {
                MeMedicion48DTO total = data.Find(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaTotalPotencia);
                MeMedicion48DTO md = data.Find(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaMaximaDemanda);

                DateTime horas = day.AddMinutes(30);
                for (int h = 1; h <= 48; h++)
                {
                    strHtml.Append("<tr>");
                    strHtml.Append(string.Format("<td class='tdbody_reporte'>{0:dd/MM/yyyy HH:mm}</td>", horas));

                    //Maxima Demanda Programada
                    decimal? maximademanda = null;
                    if (md != null)
                    {
                        maximademanda = (decimal?)md.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(md, null);
                    }
                    strHtml.Append(string.Format("<td>{0}</td>", maximademanda != null ? (maximademanda.Value).ToString("N", nfi) : string.Empty));

                    //Potencia Total
                    decimal? potenciaTotal = null;
                    if (total != null)
                    {
                        potenciaTotal = (decimal?)total.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(total, null);
                    }
                    strHtml.Append(string.Format("<td>{0}</td>", potenciaTotal != null ? (potenciaTotal.Value).ToString("N", nfi) : string.Empty));

                    //Potencia x Empresa
                    foreach (var empresa in listaEmpresa)
                    {
                        MeMedicion48DTO reg = data.Find(x => x.Medifecha == day && x.Emprcodi == empresa.Emprcodi && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaMWEjec);
                        decimal? pot = null;
                        if (reg != null)
                        {
                            pot = (decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg, null);
                        }
                        strHtml.Append(string.Format("<td>{0}</td>", pot != null ? (pot.Value).ToString("N", nfi) : string.Empty));
                    }

                    //IOP x Empresa
                    foreach (var empresa in listaEmpresa)
                    {
                        MeMedicion48DTO reg = data.Find(x => x.Medifecha == day && x.Emprcodi == empresa.Emprcodi && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaIOP);
                        int iop = ConstantesMonitoreo.ValorIOPEsPivotal;

                        if (reg != null)
                        {
                            iop = Convert.ToInt32(((decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg, null)).GetValueOrDefault(0));
                        }

                        string color = ConstantesMonitoreo.ValorIOPEsPivotal == iop ? colorEsPivotal : colorNoPivotal;
                        strHtml.AppendFormat("<td style='background-color: {1}; color:White;'>{0}</td>", iop, color);
                    }
                    strHtml.Append("</tr>");
                    horas = horas.AddMinutes(30);
                }
            }
            #endregion
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        /// <summary>
        ///  Html indicador 4
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public string ReporteIndicador4Html(string empresas, DateTime fechaInicio, DateTime fechaFin, int vermmcodi, List<MmmDatoDTO> listFacTable)
        {
            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";

            List<SiEmpresaDTO> listaEmpresa;
            List<MeMedicion48DTO> data = this.ListarIndicador(vermmcodi, fechaInicio, fechaFin, ConstantesMonitoreo.CodigoRSD, listFacTable, ConstantesMonitoreo.ReportesIndicadores, out listaEmpresa);
            listaEmpresa = this.ListarEmpresasMonitoreoByData(listaEmpresa, empresas);

            if (listaEmpresa.Count == 0) return string.Empty;

            int padding = 20;
            int anchoTotal = (100 + padding) + listaEmpresa.Count * (200 + padding);

            strHtml.Append("<div class='freeze_table' id='resultado4' style='height: auto;'>");
            strHtml.AppendFormat("<table id='reporte4' class='pretty tabla-icono' style='table-layout: fixed; width: {0}px;' >", anchoTotal);
            strHtml.Append("<thead>");
            #region cabecera
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width: 110px;' rowspan=2>MM/DD/hh:mm</th>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px' rowspan=2 > MD  (MW)</th>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px' rowspan=2> PES (MW) </th>");
            for (int i = 1; i <= 2; i++)
            {
                foreach (var datos in listaEmpresa)
                {
                    strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 100px' class='residual_{1}'>{0}</th>", datos.Emprnomb, i);
                }
            }
            strHtml.Append("</tr>");
            strHtml.Append("<tr>");

            for (int i = 1; i <= 2; i++)
            {
                foreach (var datos in listaEmpresa)
                {
                    string valor; valor = (i == 1) ? " PE (MW)" : "RSD";
                    strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 100px' class='residual_{1}'>{0}</th>", valor, i);
                }
            }

            strHtml.Append("</tr>");
            #endregion
            strHtml.Append("</thead>");
            strHtml.Append("<tbody>");
            #region cuerpo
            for (var day = fechaInicio.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
            {
                MeMedicion48DTO total = data.Find(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaTotalPotencia);
                MeMedicion48DTO md = data.Find(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaMaximaDemanda);

                DateTime horas = day.AddMinutes(30);
                for (int h = 1; h <= 48; h++)
                {
                    strHtml.Append("<tr>");
                    strHtml.Append(string.Format("<td class='tdbody_reporte'>{0:dd/MM/yyyy HH:mm}</td>", horas));

                    //Maxima Demanda Programada
                    decimal? maximademanda = null;
                    if (md != null)
                    {
                        maximademanda = (decimal?)md.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(md, null);
                    }
                    strHtml.Append(string.Format("<td>{0}</td>", maximademanda != null ? (maximademanda.Value).ToString("N", nfi) : string.Empty));

                    //Potencia Total
                    decimal? potenciaTotal = null;
                    if (total != null)
                    {
                        potenciaTotal = (decimal?)total.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(total, null);
                    }
                    strHtml.Append(string.Format("<td>{0}</td>", potenciaTotal != null ? (potenciaTotal.Value).ToString("N", nfi) : string.Empty));

                    //Potencia x Empresa
                    foreach (var empresa in listaEmpresa)
                    {
                        MeMedicion48DTO reg = data.Find(x => x.Medifecha == day && x.Emprcodi == empresa.Emprcodi && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaMWEjec);
                        decimal? pot = null;
                        if (reg != null)
                        {
                            pot = (decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg, null);
                        }
                        strHtml.Append(string.Format("<td>{0}</td>", pot.GetValueOrDefault(0) != 0 ? (pot.Value).ToString("N", nfi) : string.Empty));
                    }

                    //IOR x Empresa
                    foreach (var empresa in listaEmpresa)
                    {
                        MeMedicion48DTO reg = data.Find(x => x.Medifecha == day && x.Emprcodi == empresa.Emprcodi && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaRSD);
                        decimal? ior = null;

                        if (reg != null)
                        {
                            ior = ((decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg, null)).GetValueOrDefault(0);
                        }

                        strHtml.AppendFormat("<td >{0}</td>", ior.GetValueOrDefault(0) != 0 ? ior.Value.ToString("N", nfi) : string.Empty);
                    }
                    strHtml.Append("</tr>");
                    horas = horas.AddMinutes(30);
                }
            }
            #endregion
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        /// <summary>
        ///  Html indicador 5
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public string ReporteIndicador5Html(string empresas, DateTime fechaInicio, DateTime fechaFin, int vermmcodi, List<MmmDatoDTO> listFacTable)
        {
            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";

            List<SiEmpresaDTO> listaEmpresa;
            List<MeMedicion48DTO> data = this.ListarIndicador(vermmcodi, fechaInicio, fechaFin, ConstantesMonitoreo.CodigoILE, listFacTable, ConstantesMonitoreo.ReportesIndicadores, out listaEmpresa);
            listaEmpresa = this.ListarEmpresasMonitoreoByData(listaEmpresa, empresas);

            List<MeMedicion48DTO> listaBarra = data.Where(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCMEjec)
                .Select(x => new { Emprcodi = x.Emprcodi, Barrcodi = x.Barrcodi, Barrnombre = x.Barrnombre })
                .GroupBy(x => new { x.Emprcodi, x.Barrcodi, x.Barrnombre })
                .Select(x => new MeMedicion48DTO() { Emprcodi = x.Key.Emprcodi, Barrcodi = x.Key.Barrcodi, Barrnombre = x.Key.Barrnombre })
                .OrderBy(x => x.Barrnombre).ToList();

            int totalBarra = listaBarra.Count();
            totalBarra = totalBarra + 1;

            if (listaEmpresa.Count == 0) return string.Empty;

            int padding = 20;
            int anchoCol = 100;
            int anchoTotal = (100 + padding) + totalBarra * 3 * (anchoCol + 50 + padding);

            strHtml.Append("<div class='freeze_table' id='resultado5' style='height: auto;'>");
            strHtml.Append("<div  id='resultado5' style='height: auto; width:auto;'>");
            strHtml.AppendFormat("<table id='reporte5' class='pretty tabla-icono' style='table-layout: fixed; width: {0}px;' >", anchoTotal);
            strHtml.Append("<thead>");
            #region cabecera
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width: 110px;' rowspan='3'> MM/DD/hh:mm </th>");

            //Empresas
            int contEmpresa = 1;
            foreach (var empresa in listaEmpresa)
            {
                int totalBarraXEmpr = listaBarra.Where(x => x.Emprcodi == empresa.Emprcodi).Count();
                totalBarraXEmpr = totalBarraXEmpr > 0 ? totalBarraXEmpr : 1;

                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal; width: {2}px;' colspan ='{1}' class='cmg_{3}'>{0}</th>"
                    , empresa.Emprnomb, 3 * totalBarraXEmpr, 3 * totalBarraXEmpr * anchoCol, contEmpresa % 2);
                contEmpresa++;
            }

            strHtml.Append("</tr>");
            strHtml.Append("<tr>");

            //Barra X Empresa
            contEmpresa = 1;
            foreach (var empresa in listaEmpresa)
            {
                var listaBarraXEmpr = listaBarra.Where(x => x.Emprcodi == empresa.Emprcodi).ToList();
                int totalBarraXEmpr = listaBarraXEmpr.Count();
                if (totalBarraXEmpr > 0)
                {
                    foreach (var barra in listaBarraXEmpr)
                    {
                        strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal; width: {2}px;'  colspan ='{1}' class='cmg_{3}'>{0}</th>"
                            , barra.Barrnombre, 3, anchoCol * 3, contEmpresa % 2);
                    }
                }
                else
                {
                    strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal; width: {2}px;'  colspan ='{1}' class='cmg_{3}'>{0}</th>"
                        , "", 3, anchoCol * 3, contEmpresa % 2);
                }
                contEmpresa++;
            }

            strHtml.Append("</tr>");
            strHtml.Append("<tr>");

            //Valores x Barra
            contEmpresa = 1;
            foreach (var empresa in listaEmpresa)
            {
                int totalBarraXEmpr = listaBarra.Where(x => x.Emprcodi == empresa.Emprcodi).Count();
                totalBarraXEmpr = totalBarraXEmpr > 0 ? totalBarraXEmpr : 1;
                for (int j = 0; j < totalBarraXEmpr; j++)
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        string valor; valor = (i == 1) ? " CMg <br> (k,m,t)" : (i == 2) ? " CV <br> (S/K wh)" : "ILE  <br>  (Índice de Lerner)";
                        strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal; width: {1}px;'  class='cmg_{2}'>{0}</th>"
                            , valor, anchoCol, contEmpresa % 2);
                    }
                }
                contEmpresa++;
            }

            strHtml.Append("</tr>");
            #endregion
            strHtml.Append("</thead>");
            #region cuerpo
            strHtml.Append("<tbody>");

            for (var day = fechaInicio.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
            {
                List<MeMedicion48DTO> dataCMEjec = data.Where(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCMEjec).OrderBy(x => x.Emprnomb).ThenBy(x => x.Barrnombre).ToList();
                List<MeMedicion48DTO> dataCV = data.Where(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCV).ToList();
                List<MeMedicion48DTO> dataILE = data.Where(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaILE).ToList();

                DateTime horas = day.AddMinutes(30);
                for (int h = 1; h <= 48; h++)
                {
                    strHtml.Append("<tr>");

                    strHtml.Append(string.Format("<td>{0:dd/MM/yyyy HH:mm}</td>", horas));

                    foreach (var empresa in listaEmpresa)
                    {
                        var listaBarraXEmpr = listaBarra.Where(x => x.Emprcodi == empresa.Emprcodi).ToList();
                        int totalBarraXEmpr = listaBarraXEmpr.Count();
                        if (totalBarraXEmpr > 0)
                        {
                            foreach (var barra in listaBarraXEmpr)
                            {
                                if (barra.Barrcodi == 334)
                                {
                                }

                                MeMedicion48DTO objcmg = dataCMEjec.Find(x => x.Medifecha == day && x.Barrcodi == barra.Barrcodi && x.Emprcodi == barra.Emprcodi);
                                MeMedicion48DTO objcv = dataCV.Find(x => x.Medifecha == day && x.Barrcodi == barra.Barrcodi && x.Emprcodi == barra.Emprcodi);
                                MeMedicion48DTO objile = dataILE.Find(x => x.Medifecha == day && x.Barrcodi == barra.Barrcodi && x.Emprcodi == barra.Emprcodi);

                                decimal? valorCMg = null;
                                decimal? valorCV = null;
                                decimal? valorIle = null;
                                bool tieneGeneracion = false;

                                if (objcmg != null)
                                {
                                    valorCMg = (decimal?)objcmg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(objcmg, null);
                                }

                                if (objcv != null)
                                {
                                    valorCV = (decimal?)objcv.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(objcv, null);
                                }

                                if (objile != null)
                                {
                                    valorIle = (decimal?)objile.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(objile, null);
                                    tieneGeneracion = objile.TieneIndicador;
                                }

                                strHtml.AppendFormat("<td>{0}</td>", valorCMg != null ? (valorCMg.Value).ToString("N", nfi) : string.Empty);
                                strHtml.AppendFormat("<td>{0}</td>", valorCV != null ? (valorCV.Value).ToString("N", nfi) : string.Empty);
                                strHtml.AppendFormat("<td>{0}</td>", !tieneGeneracion ? string.Empty : valorIle != null ? (valorIle.Value).ToString("N", nfi) : "(*)");
                            }
                        }
                        else
                        {
                            strHtml.Append("<td></td>");
                            strHtml.Append("<td></td>");
                            strHtml.Append("<td></td>");
                        }
                    }
                    strHtml.Append("</tr>");
                    horas = horas.AddMinutes(30);
                }
            }

            #endregion
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        /// <summary>
        ///  Html indicador 6
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public string ReporteIndicador6Html(string empresas, DateTime fechaInicio, DateTime fechaFin, int vermmcodi, List<MmmDatoDTO> listFacTable)
        {
            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";

            List<SiEmpresaDTO> listaEmpresa;
            List<MeMedicion48DTO> data = this.ListarIndicador(vermmcodi, fechaInicio, fechaFin, ConstantesMonitoreo.CodigoIMU, listFacTable, ConstantesMonitoreo.ReportesIndicadores, out listaEmpresa);
            listaEmpresa = this.ListarEmpresasMonitoreoByData(listaEmpresa, empresas);

            List<MeMedicion48DTO> listaBarra = data.Where(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCMEjec)
                .Select(x => new { Emprcodi = x.Emprcodi, Barrcodi = x.Barrcodi, Barrnombre = x.Barrnombre })
                .GroupBy(x => new { x.Emprcodi, x.Barrcodi, x.Barrnombre })
                .Select(x => new MeMedicion48DTO() { Emprcodi = x.Key.Emprcodi, Barrcodi = x.Key.Barrcodi, Barrnombre = x.Key.Barrnombre })
                .OrderBy(x => x.Barrnombre).ToList();

            int totalBarra = listaBarra.Count();
            totalBarra = totalBarra + 1;

            if (listaEmpresa.Count == 0) return string.Empty;

            int padding = 20;
            int anchoCol = 100;
            int anchoTotal = (100 + padding) + totalBarra * 3 * (anchoCol + 50 + padding);

            strHtml.Append("<div class='freeze_table' id='resultado6' style='height: auto;'>");
            strHtml.Append("<div  id='resultado6' style='height: auto; width:auto;'>");
            strHtml.AppendFormat("<table id='reporte6' class='pretty tabla-icono' style='table-layout: fixed; width: {0}px;' >", anchoTotal);
            strHtml.Append("<thead>");
            #region cabecera
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width: 110px;' rowspan='3'> MM/DD/hh:mm </th>");

            //Empresas
            int contEmpresa = 1;
            foreach (var empresa in listaEmpresa)
            {
                int totalBarraXEmpr = listaBarra.Where(x => x.Emprcodi == empresa.Emprcodi).Count();
                totalBarraXEmpr = totalBarraXEmpr > 0 ? totalBarraXEmpr : 1;

                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal; width: {2}px;' colspan ='{1}' class='cmg_{3}'>{0}</th>"
                    , empresa.Emprnomb, 3 * totalBarraXEmpr, 3 * totalBarraXEmpr * anchoCol, contEmpresa % 2);
                contEmpresa++;
            }

            strHtml.Append("</tr>");
            strHtml.Append("<tr>");

            //Barra X Empresa
            contEmpresa = 1;
            foreach (var empresa in listaEmpresa)
            {
                var listaBarraXEmpr = listaBarra.Where(x => x.Emprcodi == empresa.Emprcodi).ToList();
                int totalBarraXEmpr = listaBarraXEmpr.Count();
                if (totalBarraXEmpr > 0)
                {
                    foreach (var barra in listaBarraXEmpr)
                    {
                        strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal; width: {2}px;'  colspan ='{1}' class='cmg_{3}'>{0}</th>"
                            , barra.Barrnombre, 3, anchoCol * 3, contEmpresa % 2);
                    }
                }
                else
                {
                    strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal; width: {2}px;'  colspan ='{1}' class='cmg_{3}'>{0}</th>"
                        , "", 3, anchoCol * 3, contEmpresa % 2);
                }
                contEmpresa++;
            }

            strHtml.Append("</tr>");
            strHtml.Append("<tr>");

            //Valores x Barra
            contEmpresa = 1;
            foreach (var empresa in listaEmpresa)
            {
                int totalBarraXEmpr = listaBarra.Where(x => x.Emprcodi == empresa.Emprcodi).Count();
                totalBarraXEmpr = totalBarraXEmpr > 0 ? totalBarraXEmpr : 1;
                for (int j = 0; j < totalBarraXEmpr; j++)
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        string valor; valor = (i == 1) ? " CMg <br> (k,m,t)" : (i == 2) ? " CV <br> (S/K wh)" : "IMU  <br>  (Índice de Mark Up )";
                        strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal; width: {1}px;' class='cmg_{2}'>{0}</th>"
                            , valor, anchoCol, contEmpresa % 2);
                    }
                }
                contEmpresa++;
            }

            strHtml.Append("</tr>");
            #endregion
            strHtml.Append("</thead>");
            #region cuerpo
            strHtml.Append("<tbody>");

            for (var day = fechaInicio.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
            {
                List<MeMedicion48DTO> dataCMEjec = data.Where(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCMEjec).OrderBy(x => x.Emprnomb).ThenBy(x => x.Barrnombre).ToList();
                List<MeMedicion48DTO> dataCV = data.Where(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCV).ToList();
                List<MeMedicion48DTO> dataIMU = data.Where(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaIMU).ToList();

                DateTime horas = day.AddMinutes(30);
                for (int h = 1; h <= 48; h++)
                {
                    strHtml.Append("<tr>");

                    strHtml.Append(string.Format("<td>{0:dd/MM/yyyy HH:mm}</td>", horas));

                    foreach (var empresa in listaEmpresa)
                    {
                        var listaBarraXEmpr = listaBarra.Where(x => x.Emprcodi == empresa.Emprcodi).ToList();
                        int totalBarraXEmpr = listaBarraXEmpr.Count();
                        if (totalBarraXEmpr > 0)
                        {
                            foreach (var barra in listaBarraXEmpr)
                            {
                                if (barra.Barrcodi == 642)
                                {
                                }

                                MeMedicion48DTO objcmg = dataCMEjec.Find(x => x.Medifecha == day && x.Barrcodi == barra.Barrcodi && x.Emprcodi == barra.Emprcodi);
                                MeMedicion48DTO objcv = dataCV.Find(x => x.Medifecha == day && x.Barrcodi == barra.Barrcodi && x.Emprcodi == barra.Emprcodi);
                                MeMedicion48DTO objimu = dataIMU.Find(x => x.Medifecha == day && x.Barrcodi == barra.Barrcodi && x.Emprcodi == barra.Emprcodi);

                                decimal? valorCMg = null;
                                decimal? valorCV = null;
                                decimal? valorImu = null;
                                bool tieneGeneracion = false;

                                if (objcmg != null)
                                {
                                    valorCMg = (decimal?)objcmg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(objcmg, null);
                                }

                                if (objcv != null)
                                {
                                    valorCV = (decimal?)objcv.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(objcv, null);
                                }

                                if (objimu != null)
                                {
                                    valorImu = (decimal?)objimu.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(objimu, null);
                                    tieneGeneracion = objimu.TieneIndicador;
                                }

                                strHtml.Append(string.Format("<td>{0}</td>", valorCMg != null ? (valorCMg.Value).ToString("N", nfi) : string.Empty));
                                strHtml.Append(string.Format("<td>{0}</td>", valorCV != null ? (valorCV.Value).ToString("N", nfi) : string.Empty));
                                strHtml.Append(string.Format("<td>{0}</td>", !tieneGeneracion ? string.Empty : valorImu != null ? (valorImu.Value).ToString("N", nfi) : "(*)"));
                            }
                        }
                        else
                        {
                            strHtml.Append("<td></td>");
                            strHtml.Append("<td></td>");
                            strHtml.Append("<td></td>");
                        }
                    }
                    strHtml.Append("</tr>");
                    horas = horas.AddMinutes(30);
                }
            }

            #endregion
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Html indicador 7
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="fechaHist"></param>
        /// <param name="generacion"></param>
        /// <returns></returns>
        public string ReporteIndicador7Html(string empresas, DateTime fechaInicio, DateTime fechaFin, int vermmcodi, List<MmmDatoDTO> listFacTable)
        {
            StringBuilder strHtml = new StringBuilder();

            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";

            //Empresas
            List<SiEmpresaDTO> listaEmpresa;
            List<MeMedicion48DTO> data = this.ListarIndicador(vermmcodi, fechaInicio, fechaFin, ConstantesMonitoreo.CodigoIRT, listFacTable, ConstantesMonitoreo.ReportesIndicadores, out listaEmpresa);
            listaEmpresa = this.ListarEmpresasMonitoreo(fechaInicio, empresas);

            List<int> listaEmprcodi = listaEmpresa.Select(x => x.Emprcodi).ToList();

            //Congestiones
            List<MeMedicion48DTO> listaCongXAreaXFecha = data.Where(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCongestion
                && listaEmprcodi.Contains(x.Emprcodi)).ToList();
            listaCongXAreaXFecha = this.ListarCongXAreaYFechaReporte7(listaCongXAreaXFecha);

            //Grupos despacho
            List<MeMedicion48DTO> listaBarraEmpGrupo = data.Where(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaEnlaceTrans
                && listaEmprcodi.Contains(x.Emprcodi)).ToList();

            if (listaBarraEmpGrupo.Count == 0)
            {
                listaBarraEmpGrupo.Add(new MeMedicion48DTO() { Grupocodi = -1, Gruponomb = string.Empty, Grupopadre = -2, Central = string.Empty, Barrcodi = -1, Barrnombre = string.Empty });
            }

            #region Lista de Congestiones

            strHtml.Append("<div class='freeze_table' id='resultadocong7' style='height: auto;'>");
            strHtml.AppendFormat("<table id='reportecong7' class='pretty tabla-icono' style='table-layout: fixed; width: {0}px;' >", 800);
            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 80px'>" + "FECHA" + "</th>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 80px'>" + "INICIO" + "</th>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 80px'>" + "FINAL" + "</th>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px'>" + "UBICACIÓN" + "</th>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px'>" + "EQUIPO" + "</th>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 150px'>" + "OBSERVACIONES" + "</th>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 200px'>" + "GENERACIÓN CON UNA LOCALIZACIÓN ESPECÍFICA, QUE OBTENGAN PODER DE MERCADO" + "</th>");
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");

            strHtml.Append("<tbody>");

            foreach (var reg in listaCongXAreaXFecha)
            {
                strHtml.Append("<tr>");
                strHtml.AppendFormat("<td>{0}</td>", reg.Hophorini.ToString(ConstantesAppServicio.FormatoFecha));
                strHtml.AppendFormat("<td>{0}</td>", reg.Hophorini.ToString(ConstantesAppServicio.FormatoOnlyHora));
                strHtml.AppendFormat("<td>{0}</td>", reg.Hophorfin.ToString(ConstantesAppServicio.FormatoOnlyHora));
                strHtml.AppendFormat("<td>{0}</td>", reg.Areanomb != null ? (reg.Areanomb) : string.Empty);
                strHtml.AppendFormat("<td>{0}</td>", reg.Equinomb);
                strHtml.AppendFormat("<td>{0}</td>", reg.Descripcion);
                strHtml.AppendFormat("<td>{0}</td>", reg.Gruponomb);
                strHtml.Append("</tr>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            strHtml.Append("</div>");

            #endregion

            strHtml.Append("<br/>");
            strHtml.Append("<br/>");

            #region Reporte
            int padding = 20;
            int columnas = listaBarraEmpGrupo.Count * 5;
            int anchoTotalCong = (100 + padding) + (columnas) * (150 + padding);

            strHtml.Append("<div class='freeze_table' id='resultado7' style='height: auto;'>");

            strHtml.AppendFormat("<table id='reporte7' class='pretty tabla-icono' style='table-layout: fixed; width: {0}px;' >", anchoTotalCong);
            strHtml.Append("<thead>");
            #region Cabecera
            strHtml.Append("<tr>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px' rowspan=4>" + "MM/DD/hh:mm" + "</th>");

            //Enlace de transmision
            foreach (var grupo in listaBarraEmpGrupo)
            {
                strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 500px' colspan='4'>Enlace de transmisión</th>");
                strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px' rowspan='4'>" + "IRT" + "</th>");
            }

            strHtml.Append("</tr>");

            //Lineas, trafos
            strHtml.Append("<tr>");
            foreach (var grupo in listaBarraEmpGrupo)
            {
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 500px' colspan='4'>{0}</th>", grupo.Equinomb);
            }
            strHtml.Append("</tr>");

            //Grupo, Barra
            strHtml.Append("<tr>");

            foreach (var grupo in listaBarraEmpGrupo)
            {
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 100px'>{0}</th>", grupo.Gruponomb);
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 100px'>{0}</th>", grupo.Gruponomb);
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 100px'>{0}</th>", grupo.Barrnombre);
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 100px'>{0}</th>", grupo.Barrnombre);
            }

            strHtml.Append("</tr>");

            //Datos
            strHtml.Append("<tr>");
            foreach (var grupo in listaBarraEmpGrupo)
            {
                strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px'> PU (MW)</th>");
                strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px'> PP (MW)</th>");
                strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px'> CMg (S/ MWh)</th>");
                strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px'> CMgprog (S/ MWh)</th>");

            }
            strHtml.Append("</tr>");
            #endregion
            strHtml.Append("</thead>");
            strHtml.Append("<tbody>");
            #region Cuerpo
            for (var day = fechaInicio.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
            {
                List<MeMedicion48DTO> dataXDia = data.Where(x => x.Medifecha == day).ToList();

                DateTime horas = day.AddMinutes(30);

                for (int h = 1; h <= 48; h++)
                {
                    strHtml.Append("<tr>");
                    strHtml.Append(string.Format("<td class='tdbody_reporte'>{0:dd/MM/yyyy HH:mm}</td>", horas));

                    foreach (var grupo in listaBarraEmpGrupo)
                    {
                        MeMedicion48DTO regIRT = dataXDia.Find(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaIRT && x.Grupocodi == grupo.Grupocodi);
                        if (regIRT != null)
                        {
                            decimal? irt = (decimal?)regIRT.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regIRT, null);
                            if (irt != null)
                            {
                                //Potencia ejecutada
                                MeMedicion48DTO regpotEjec = this.GetPotenciaTotalXDiaByGrupoReporte7(day, dataXDia.Where(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaPotGrupoEjec).ToList(), grupo.Grupocodi);
                                decimal? potEjec = null;
                                if (regpotEjec != null)
                                {
                                    potEjec = (decimal?)regpotEjec.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regpotEjec, null);
                                }
                                strHtml.Append(string.Format("<td>{0}</td>", potEjec.GetValueOrDefault(0) != 0 ? (potEjec.Value).ToString("N", nfi) : string.Empty));

                                //Potencia programada
                                MeMedicion48DTO regpotProg = this.GetPotenciaTotalXDiaByGrupoReporte7(day, dataXDia.Where(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaPotGrupoProg).ToList(), grupo.Grupocodi);
                                decimal? potProg = null;
                                if (regpotProg != null)
                                {
                                    potProg = (decimal?)regpotProg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regpotProg, null);
                                }
                                strHtml.Append(string.Format("<td>{0}</td>", potProg.GetValueOrDefault(0) != 0 ? (potProg.Value).ToString("N", nfi) : string.Empty));

                                //Costos marg Ejecutados
                                MeMedicion48DTO regCMgejec = dataXDia.Find(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCMEjec && x.Barrcodi == grupo.Barrcodi && x.Emprcodi == grupo.Emprcodi);
                                decimal? cmg = null;
                                if (regCMgejec != null && potEjec.GetValueOrDefault(0) != 0)
                                {
                                    cmg = (decimal?)regCMgejec.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regCMgejec, null);
                                    if (cmg != null)
                                        cmg = cmg * 1000;
                                }
                                strHtml.Append(string.Format("<td>{0}</td>", cmg.GetValueOrDefault(0) != 0 ? (cmg.Value).ToString("N", nfi) : string.Empty));

                                //Costos marg Programados
                                MeMedicion48DTO regCMgprog = dataXDia.Find(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCMProg && x.Cnfbarcodi == grupo.Cnfbarcodi && x.Emprcodi == grupo.Emprcodi);
                                decimal? cmgProg = null;
                                if (regCMgprog != null && potProg.GetValueOrDefault(0) != 0)
                                {
                                    cmgProg = (decimal?)regCMgprog.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regCMgprog, null);
                                }
                                strHtml.Append(string.Format("<td>{0}</td>", cmgProg.GetValueOrDefault(0) != 0 ? (cmgProg.Value).ToString("N", nfi) : string.Empty));

                                strHtml.Append(string.Format("<td style='font-weight: bold;'>{0}</td>", irt.GetValueOrDefault(0) != 0 ? (irt.Value).ToString("N", nfi) : string.Empty));
                            }
                            else
                            {
                                strHtml.Append("<td></td>");
                                strHtml.Append("<td></td>");
                                strHtml.Append("<td></td>");
                                strHtml.Append("<td></td>");
                                strHtml.Append("<td></td>");
                            }
                        }
                        else
                        {
                            strHtml.Append("<td></td>");
                            strHtml.Append("<td></td>");
                            strHtml.Append("<td></td>");
                            strHtml.Append("<td></td>");
                            strHtml.Append("<td></td>");
                        }
                    }

                    strHtml.Append("</tr>");
                    horas = horas.AddMinutes(30);
                }
            }
            #endregion
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            strHtml.Append("</div>");
            #endregion

            return strHtml.ToString();
        }

        /// <summary>
        /// Generacion Masiva excel
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="rutaNombreArchivo"></param>
        /// <param name="url"></param>
        /// <param name="imm"></param>
        /// <param name="tituloIme"></param>
        /// <param name="columna"></param>
        /// <param name="indicador"></param>
        /// <param name="version"></param>
        public void GeneracionIndicadoresExcel(DateTime fechaIni, DateTime fechaFin, List<MmmDatoDTO> listFacTableMes, MmmVersionDTO version, int tipoReporte)
        {
            string nombreExcel, file;
            this.GetNombreYRutaArchivoReporteGeneracion(version, out nombreExcel, out file);

            FileInfo newFile = new FileInfo(file);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(file);
            }

            int colIniFreeze = 2;
            int colFinFreeze = colIniFreeze + 1;
            int rowFreeze = 5;
            int rowTitulo = 2;

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                // Reporte Indicador 1 

                colFinFreeze = colIniFreeze + 1;
                ExcelCabGeneral(ref ws, xlPackage, tipoReporte, ConstantesMonitoreo.AbrevCuotaMercado, ConstantesMonitoreo.RptExcelCuotaMercado, fechaIni, rowTitulo, colFinFreeze + 1);
                GenerarExcelReporte1(ConstantesAppServicio.ParametroDefecto, ws, fechaIni, fechaFin, rowFreeze, colIniFreeze, colFinFreeze, version.Vermmcodi, listFacTableMes);
                ExcelFooterGeneral(ref ws);
                this.ActualizarPorcentajeGeneracionVersionExcel(version, 1);

                // Reporte Indicador 2
                colFinFreeze = colIniFreeze;
                ExcelCabGeneral(ref ws, xlPackage, tipoReporte, ConstantesMonitoreo.AbrevIndHeHi, ConstantesMonitoreo.RptExcelIndHeHi, fechaIni, rowTitulo, colFinFreeze + 1);
                GenerarExcelReporte2(ConstantesAppServicio.ParametroDefecto, ws, fechaIni, fechaFin, rowFreeze, colIniFreeze, colFinFreeze, version.Vermmcodi, listFacTableMes);
                ExcelFooterGeneral(ref ws);
                this.ActualizarPorcentajeGeneracionVersionExcel(version, 2);

                //Reporte Indicador 3
                colFinFreeze = colIniFreeze + 2;
                ExcelCabGeneral(ref ws, xlPackage, tipoReporte, ConstantesMonitoreo.AbrevPivotal, ConstantesMonitoreo.RptExcelPivotal, fechaIni, rowTitulo, colFinFreeze + 1);
                GenerarExcelReporte3(ConstantesAppServicio.ParametroDefecto, ws, fechaIni, fechaFin, rowFreeze, colIniFreeze, colFinFreeze, version.Vermmcodi, listFacTableMes);
                ExcelFooterGeneral(ref ws);
                this.ActualizarPorcentajeGeneracionVersionExcel(version, 3);

                //Reporte Indicador 4
                colFinFreeze = colIniFreeze + 2;
                ExcelCabGeneral(ref ws, xlPackage, tipoReporte, ConstantesMonitoreo.AbrevResidual, ConstantesMonitoreo.RptExcelResidual, fechaIni, rowTitulo, colFinFreeze + 1);
                GenerarExcelReporte4(ConstantesAppServicio.ParametroDefecto, ws, fechaIni, fechaFin, rowFreeze, colIniFreeze, colFinFreeze, version.Vermmcodi, listFacTableMes);
                ExcelFooterGeneral(ref ws);
                this.ActualizarPorcentajeGeneracionVersionExcel(version, 4);

                //Reporte Indicador 5
                colFinFreeze = colIniFreeze;
                ExcelCabGeneral(ref ws, xlPackage, tipoReporte, ConstantesMonitoreo.AbrevLerner, ConstantesMonitoreo.RptExcelLerner, fechaIni, rowTitulo, colFinFreeze + 1);
                GenerarExcelReporte5(ConstantesAppServicio.ParametroDefecto, ws, fechaIni, fechaFin, rowFreeze, colIniFreeze, colFinFreeze, version.Vermmcodi, listFacTableMes);
                ExcelFooterGeneral(ref ws);
                this.ActualizarPorcentajeGeneracionVersionExcel(version, 5);

                //Reporte Indicador 6
                colFinFreeze = colIniFreeze;
                ExcelCabGeneral(ref ws, xlPackage, tipoReporte, ConstantesMonitoreo.AbrevImu, ConstantesMonitoreo.RptExcelImu, fechaIni, rowTitulo, colFinFreeze + 1);
                GenerarExcelReporte6(ConstantesAppServicio.ParametroDefecto, ws, fechaIni, fechaFin, rowFreeze, colIniFreeze, colFinFreeze, version.Vermmcodi, listFacTableMes);
                ExcelFooterGeneral(ref ws);
                this.ActualizarPorcentajeGeneracionVersionExcel(version, 6);

                //Reporte Indicador 7
                colFinFreeze = colIniFreeze;
                ExcelCabGeneral(ref ws, xlPackage, tipoReporte, ConstantesMonitoreo.AbrevRed, ConstantesMonitoreo.RptExcelRed, fechaIni, rowTitulo, colFinFreeze + 1);
                GenerarExcelReporte7(ConstantesAppServicio.ParametroDefecto, ws, fechaIni, fechaFin, rowFreeze, colIniFreeze, colFinFreeze, version.Vermmcodi, listFacTableMes);
                ExcelFooterGeneral(ref ws);
                this.ActualizarPorcentajeGeneracionVersionExcel(version, 7);

                xlPackage.Save();
            }
        }

        /// <summary>
        /// Actualizar Porcentaje Generacion Version Excel
        /// </summary>
        /// <param name="version"></param>
        /// <param name="numHoja"></param>
        private void ActualizarPorcentajeGeneracionVersionExcel(MmmVersionDTO version, int numHoja)
        {
            int diaTotalFact = 7;
            decimal porcentajeGlobal = 75 + (24 * (numHoja / (diaTotalFact + 0.0m)));
            version.Vermmporcentaje = porcentajeGlobal;
            this.UpdateMmmVersionPorcentaje(version);
        }

        /// <summary>
        /// Generar archivo Excel segun el tipo de Indicador
        /// </summary>
        /// <param name="tipoIndicador"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="vermmcodi"></param>
        /// <param name="ruta"></param>
        public void GenerarArchivoExcelByTipo(int tipoIndicador, string idEmpresa, DateTime fechaIni, DateTime fechaFin, int vermmcodi, int tipoReporte, out string nameFile)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesMonitoreo.Directorio;

            string hoja = string.Empty;
            string titulo = string.Empty;

            switch (tipoReporte)
            {
                case ConstantesMonitoreo.ReportesIndicadores:
                    break;
                case ConstantesMonitoreo.ReportePorcentajeErrorBandaTolerancia:
                    titulo = "Transgresión_";
                    break;
            }

            switch (tipoIndicador)
            {
                case ConstantesMonitoreo.CodigoS:
                    hoja = ConstantesMonitoreo.AbrevCuotaMercado;
                    titulo = ConstantesMonitoreo.RptExcelCuotaMercado;
                    break;
                case ConstantesMonitoreo.CodigoHHI:
                    hoja = ConstantesMonitoreo.AbrevIndHeHi;
                    titulo = ConstantesMonitoreo.RptExcelIndHeHi;
                    break;
                case ConstantesMonitoreo.CodigoIOP:
                    hoja = ConstantesMonitoreo.AbrevPivotal;
                    titulo = ConstantesMonitoreo.RptExcelPivotal;
                    break;
                case ConstantesMonitoreo.CodigoRSD:
                    hoja = ConstantesMonitoreo.AbrevResidual;
                    titulo = ConstantesMonitoreo.RptExcelResidual;
                    break;
                case ConstantesMonitoreo.CodigoILE:
                    hoja = ConstantesMonitoreo.AbrevLerner;
                    titulo = ConstantesMonitoreo.RptExcelLerner;
                    break;
                case ConstantesMonitoreo.CodigoIMU:
                    hoja = ConstantesMonitoreo.AbrevImu;
                    titulo = ConstantesMonitoreo.RptExcelImu;
                    break;
                case ConstantesMonitoreo.CodigoIRT:
                    hoja = ConstantesMonitoreo.AbrevRed;
                    titulo = ConstantesMonitoreo.RptExcelRed;
                    break;
            }

            nameFile = titulo + ConstantesMonitoreo.ExtensionExcel;

            this.GenerarArchivoExcel(idEmpresa, fechaIni, fechaFin, ruta + nameFile, hoja, titulo, tipoIndicador, vermmcodi, tipoReporte);
        }

        /// <summary>
        /// Generar archivo Excel Todos los indicadores
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="rutaNombreArchivo"></param>
        /// <param name="imm"></param>
        /// <param name="tituloIme"></param>
        /// <param name="indicador"></param>
        /// <param name="version"></param>
        public void GenerarArchivoExcel(string empresas, DateTime fechaIni, DateTime fechaFin, string rutaNombreArchivo, string imm, string tituloIme, int indicador, int version, int tipoReporte)
        {
            #region variables para graficos exp excel

            string xAxisTitle = string.Empty, yAxisTitle = string.Empty, titulo = string.Empty;
            #endregion
            FileInfo newFile = new FileInfo(rutaNombreArchivo);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaNombreArchivo);
            }

            int colIniFreeze = 2;
            int colFinFreeze = colIniFreeze + 1;
            int rowFreeze = 6;
            int rowTitulo = 2;

            List<MmmDatoDTO> listFacTable = FactorySic.GetMmmDatoRepository().ListPeriodo(fechaIni.AddMinutes(30), fechaFin.AddDays(1));

            switch (tipoReporte)
            {
                case ConstantesMonitoreo.ReportesIndicadores:
                    using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                    {
                        ExcelWorksheet ws = null;

                        switch (indicador)
                        {
                            case ConstantesMonitoreo.CodigoS:
                                colFinFreeze = colIniFreeze + 1;
                                ExcelCabGeneral(ref ws, xlPackage, tipoReporte, ConstantesMonitoreo.AbrevCuotaMercado, ConstantesMonitoreo.RptExcelCuotaMercado, fechaIni, rowTitulo, colFinFreeze + 1);
                                GenerarExcelReporte1(ConstantesAppServicio.ParametroDefecto, ws, fechaIni, fechaFin, rowFreeze, colIniFreeze, colFinFreeze, version, listFacTable);
                                ExcelFooterGeneral(ref ws);
                                break;
                            case ConstantesMonitoreo.CodigoHHI:
                                colFinFreeze = colIniFreeze;
                                ExcelCabGeneral(ref ws, xlPackage, tipoReporte, ConstantesMonitoreo.AbrevIndHeHi, ConstantesMonitoreo.RptExcelIndHeHi, fechaIni, rowTitulo, colFinFreeze + 1);
                                GenerarExcelReporte2(ConstantesAppServicio.ParametroDefecto, ws, fechaIni, fechaFin, rowFreeze, colIniFreeze, colFinFreeze, version, listFacTable);
                                ExcelFooterGeneral(ref ws);
                                break;
                            case ConstantesMonitoreo.CodigoIOP:
                                colFinFreeze = colIniFreeze + 2;
                                ExcelCabGeneral(ref ws, xlPackage, tipoReporte, ConstantesMonitoreo.AbrevPivotal, ConstantesMonitoreo.RptExcelPivotal, fechaIni, rowTitulo, colFinFreeze + 1);
                                GenerarExcelReporte3(ConstantesAppServicio.ParametroDefecto, ws, fechaIni, fechaFin, rowFreeze, colIniFreeze, colFinFreeze, version, listFacTable);
                                ExcelFooterGeneral(ref ws);
                                break;
                            case ConstantesMonitoreo.CodigoRSD:
                                colFinFreeze = colIniFreeze + 2;
                                ExcelCabGeneral(ref ws, xlPackage, tipoReporte, ConstantesMonitoreo.AbrevResidual, ConstantesMonitoreo.RptExcelResidual, fechaIni, rowTitulo, colFinFreeze + 1);
                                GenerarExcelReporte4(ConstantesAppServicio.ParametroDefecto, ws, fechaIni, fechaFin, rowFreeze, colIniFreeze, colFinFreeze, version, listFacTable);
                                ExcelFooterGeneral(ref ws);
                                break;
                            case ConstantesMonitoreo.CodigoILE:
                                colFinFreeze = colIniFreeze;
                                ExcelCabGeneral(ref ws, xlPackage, tipoReporte, ConstantesMonitoreo.AbrevLerner, ConstantesMonitoreo.RptExcelLerner, fechaIni, rowTitulo, colFinFreeze + 1);
                                GenerarExcelReporte5(ConstantesAppServicio.ParametroDefecto, ws, fechaIni, fechaFin, rowFreeze, colIniFreeze, colFinFreeze, version, listFacTable);
                                ExcelFooterGeneral(ref ws);
                                break;
                            case ConstantesMonitoreo.CodigoIMU:
                                colFinFreeze = colIniFreeze;
                                ExcelCabGeneral(ref ws, xlPackage, tipoReporte, ConstantesMonitoreo.AbrevImu, ConstantesMonitoreo.RptExcelImu, fechaIni, rowTitulo, colFinFreeze + 1);
                                GenerarExcelReporte6(ConstantesAppServicio.ParametroDefecto, ws, fechaIni, fechaFin, rowFreeze, colIniFreeze, colFinFreeze, version, listFacTable);
                                ExcelFooterGeneral(ref ws);
                                break;
                            case ConstantesMonitoreo.CodigoIRT:
                                colFinFreeze = colIniFreeze;
                                ExcelCabGeneral(ref ws, xlPackage, tipoReporte, ConstantesMonitoreo.AbrevRed, ConstantesMonitoreo.RptExcelRed, fechaIni, rowTitulo, colFinFreeze + 1);
                                GenerarExcelReporte7(ConstantesAppServicio.ParametroDefecto, ws, fechaIni, fechaFin, rowFreeze, colIniFreeze, colFinFreeze, version, listFacTable);
                                ExcelFooterGeneral(ref ws);
                                break;
                        }

                        xlPackage.Save();
                    }
                    break;
                case ConstantesMonitoreo.ReportePorcentajeErrorBandaTolerancia:
                    using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                    {
                        ExcelWorksheet ws = null;

                        switch (indicador)
                        {
                            case ConstantesMonitoreo.CodigoS:
                                colFinFreeze = colIniFreeze + 1;
                                ExcelCabGeneral(ref ws, xlPackage, tipoReporte, ConstantesMonitoreo.AbrevCuotaMercado, ConstantesMonitoreo.RptExcelCuotaMercado, fechaIni, rowTitulo, colFinFreeze + 1);
                                GenerarExcelReportePorcentajeErrorBT1(ConstantesAppServicio.ParametroDefecto, ws, fechaIni, fechaFin, rowFreeze, colIniFreeze, colFinFreeze, version, listFacTable);
                                ExcelFooterGeneral(ref ws);
                                break;
                            case ConstantesMonitoreo.CodigoHHI:
                                colFinFreeze = colIniFreeze;
                                ExcelCabGeneral(ref ws, xlPackage, tipoReporte, ConstantesMonitoreo.AbrevIndHeHi, ConstantesMonitoreo.RptExcelIndHeHi, fechaIni, rowTitulo, colFinFreeze + 1);
                                GenerarExcelReportePorcentajeErrorBT2(ConstantesAppServicio.ParametroDefecto, ws, fechaIni, fechaFin, rowFreeze, colIniFreeze, colFinFreeze, version, listFacTable);
                                ExcelFooterGeneral(ref ws);
                                break;
                            case ConstantesMonitoreo.CodigoIOP:
                                colFinFreeze = colIniFreeze + 2;
                                ExcelCabGeneral(ref ws, xlPackage, tipoReporte, ConstantesMonitoreo.AbrevPivotal, ConstantesMonitoreo.RptExcelPivotal, fechaIni, rowTitulo, colFinFreeze + 1);
                                GenerarExcelReportePorcentajeErrorBT3(ConstantesAppServicio.ParametroDefecto, ws, fechaIni, fechaFin, rowFreeze, colIniFreeze, colFinFreeze, version, listFacTable);
                                ExcelFooterGeneral(ref ws);
                                break;
                            case ConstantesMonitoreo.CodigoRSD:
                                colFinFreeze = colIniFreeze + 2;
                                ExcelCabGeneral(ref ws, xlPackage, tipoReporte, ConstantesMonitoreo.AbrevResidual, ConstantesMonitoreo.RptExcelResidual, fechaIni, rowTitulo, colFinFreeze + 1);
                                GenerarExcelReportePorcentajeErrorBT4(ConstantesAppServicio.ParametroDefecto, ws, fechaIni, fechaFin, rowFreeze, colIniFreeze, colFinFreeze, version, listFacTable);
                                ExcelFooterGeneral(ref ws);
                                break;
                            case ConstantesMonitoreo.CodigoILE:
                                colFinFreeze = colIniFreeze;
                                ExcelCabGeneral(ref ws, xlPackage, tipoReporte, ConstantesMonitoreo.AbrevLerner, ConstantesMonitoreo.RptExcelLerner, fechaIni, rowTitulo, colFinFreeze + 1);
                                GenerarExcelReportePorcentajeErrorBT5(ConstantesAppServicio.ParametroDefecto, ws, fechaIni, fechaFin, rowFreeze, colIniFreeze, colFinFreeze, version, listFacTable);
                                ExcelFooterGeneral(ref ws);
                                break;
                            case ConstantesMonitoreo.CodigoIMU:
                                colFinFreeze = colIniFreeze;
                                ExcelCabGeneral(ref ws, xlPackage, tipoReporte, ConstantesMonitoreo.AbrevImu, ConstantesMonitoreo.RptExcelImu, fechaIni, rowTitulo, colFinFreeze + 1);
                                GenerarExcelReportePorcentajeErrorBT6(ConstantesAppServicio.ParametroDefecto, ws, fechaIni, fechaFin, rowFreeze, colIniFreeze, colFinFreeze, version, listFacTable);
                                ExcelFooterGeneral(ref ws);
                                break;
                            case ConstantesMonitoreo.CodigoIRT:
                                colFinFreeze = colIniFreeze;
                                ExcelCabGeneral(ref ws, xlPackage, tipoReporte, ConstantesMonitoreo.AbrevRed, ConstantesMonitoreo.RptExcelRed, fechaIni, rowTitulo, colFinFreeze + 1);
                                GenerarExcelReportePorcentajeErrorBT7(ConstantesAppServicio.ParametroDefecto, ws, fechaIni, fechaFin, rowFreeze, colIniFreeze, colFinFreeze, version, listFacTable);
                                ExcelFooterGeneral(ref ws);
                                break;
                        }

                        xlPackage.Save();
                    }
                    break;
            }
        }

        /// <summary>
        /// Formato Cabecera de titulo de los indicadores
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="xlPackage"></param>
        /// <param name="nameWS"></param>
        /// <param name="titulo"></param>
        /// <param name="cadenaFecha"></param>
        /// <param name="fecha2"></param>
        /// <param name="columna"></param>
        private void ExcelCabGeneral(ref ExcelWorksheet ws, ExcelPackage xlPackage, int tipoReporte, string nameWS, string tituloIndicador, DateTime fechaPeriodo, int rowIni, int colIni)
        {
            ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];
            ws.TabColor = ColorTranslator.FromHtml("#4774C5");

            // mes año
            string cadenaFecha = fechaPeriodo.ToString(ConstantesAppServicio.FormatoMes);
            int sub = Int32.Parse(cadenaFecha.Substring(0, 2));
            string anio = cadenaFecha.Substring(2, 5);
            string mes = COES.Base.Tools.Util.ObtenerNombreMes(sub);

            //Titulo
            string tituloReporte = string.Empty;

            switch (tipoReporte)
            {
                case ConstantesMonitoreo.ReportesIndicadores:
                    tituloReporte = "REPORTE DE INDICADORES PARA EL MONITOREO DEL MERCADO MAYORISTA DE ELECTRICIDAD - " + mes + " " + anio + " (RCD OSINERGMIN N° 209-2017-OS/CD)";
                    break;
                case ConstantesMonitoreo.ReportePorcentajeErrorBandaTolerancia:
                    tituloReporte = "REPORTE DE TRANSGRESIÓN DE BANDA DE TOLERANCIA - " + mes + " " + anio + " (RCD OSINERGMIN N° 209-2017-OS/CD)";
                    break;
            }

            int rowTitulo = rowIni;
            ws.Cells[rowTitulo, colIni].Value = tituloReporte;
            ws.Cells[rowTitulo, colIni].Style.Font.Size = 72;
            ws.Cells[rowTitulo, colIni].Style.Font.Bold = true;
            ws.Cells[rowTitulo, colIni].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            //IMME
            int rowIndicador = rowTitulo + 2;
            int colFinTitulo = colIni + 8;
            ws.Cells[rowIndicador, colIni].Value = tituloIndicador;
            ws.Cells[rowIndicador, colIni].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            ws.Cells[rowIndicador, colIni, rowIndicador, colFinTitulo].Style.Font.Bold = true;
            ws.Cells[rowIndicador, colIni, rowIndicador, colFinTitulo].Style.Font.Size = 50;
            ws.Cells[rowIndicador, colIni, rowIndicador, colFinTitulo].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[rowIndicador, colIni, rowIndicador, colFinTitulo].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#4472C4"));
            ws.Cells[rowIndicador, colIni, rowIndicador, colFinTitulo].Style.Font.Color.SetColor(Color.White);

            ws.Row(rowTitulo).Height = 120;
            ws.Row(rowIndicador).Height = 85;

            //Logo
            HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
            ExcelPicture picture = ws.Drawings.AddPicture("Imagen", img);

            picture.From.Column = 0;
            picture.From.Row = 0;
            picture.From.ColumnOff = Pixel2MTU(80);
            picture.From.RowOff = Pixel2MTU(80);
            picture.SetSize(303, 167); //202,11
        }

        /// <summary>
        /// Deterina ancho en pixeles para el logo
        /// </summary>
        /// <param name="pixels"></param>
        /// <returns></returns>
        public int Pixel2MTU(int pixels)
        {
            //convert pixel to MTU
            int MTU_PER_PIXEL = 9525;
            int mtus = pixels * MTU_PER_PIXEL;
            return mtus;
        }

        /// <summary>
        /// Footer para todos los excels
        /// </summary>
        /// <param name="ws"></param>
        private void ExcelFooterGeneral(ref ExcelWorksheet ws)
        {
            //Font para toda la Hoja excel
            var allCells = ws.Cells[1, 1, ws.Dimension.End.Row, ws.Dimension.End.Column];
            var cellFont = allCells.Style.Font;
            cellFont.Name = "Arial";

            //No mostrar lineas
            ws.View.ShowGridLines = false;
        }

        /// <summary>
        /// Generación de Reporte 1
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="ws"></param>
        /// <param name="nombreSheet"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="vermmcodi"></param>
        public void GenerarExcelReporte1(string empresas, ExcelWorksheet ws, DateTime fechaIni, DateTime fechaFin, int rowIni, int colIni, int colFinFreeze, int vermmcodi, List<MmmDatoDTO> listFacTable)
        {
            List<SiEmpresaDTO> listaEmpresa;
            List<MeMedicion48DTO> dataTotal = this.ListarIndicador(vermmcodi, fechaIni, fechaFin, ConstantesMonitoreo.CodigoS, listFacTable, ConstantesMonitoreo.ReportesIndicadores, out listaEmpresa);
            listaEmpresa = this.ListarEmpresasMonitoreoByData(listaEmpresa, empresas);

            int row = rowIni + 5;
            int col = colIni;
            //
            if (listaEmpresa.Count > 0)
            {
                #region cabecera

                int rowIniNombreReporte = row;
                /// Fila Hora - Empresa - Total

                int colIniFecha = col;
                int rowIniFecha = row;
                int rowFinFecha = rowIniFecha + 2 - 1;
                ws.Cells[rowIniFecha, colIniFecha].Value = "MM/DD/hh:mm";
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Merge = true;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.WrapText = true;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Font.Bold = true;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Font.Size = 28;

                int colIniSein = colIniFecha + 1;
                int rowIniSein = rowIniFecha;
                int rowFinSein = rowFinFecha;
                ws.Cells[rowIniSein, colIniSein].Value = "PES";
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Merge = true;
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.WrapText = true;
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.Font.Bold = true;
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.Font.Size = 28;


                int rowIniEmp = row;
                int colIniEmp = colIniSein + 1;
                int colFinEmp = colIniEmp;

                int colIniNombreReporte = colIniEmp;

                for (int j = 1; j <= 2; j++)
                {
                    for (int i = 0; i < listaEmpresa.Count; i++)
                    {
                        //Empresa
                        var thEmp = listaEmpresa[i];

                        colFinEmp = colIniEmp;
                        ws.Cells[rowIniEmp, colIniEmp].Value = thEmp.Emprnomb.Trim();

                        ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.Font.Size = 24;
                        ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Merge = true;
                        ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.WrapText = true;

                        ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.TextRotation = 90;
                        colIniEmp = colFinEmp + 1;
                    }
                }

                int rowIniMw = row + 1;
                int colIniMw = colIniSein + 1;
                int colFinMw = colIniMw;

                for (int j = 1; j <= 2; j++)
                {
                    for (int i = 0; i < listaEmpresa.Count; i++)
                    {
                        //Empresa
                        var thEmp = listaEmpresa[i];

                        ws.Cells[rowIniMw, colIniMw].Value = j == 1 ? "PE (MW)" : "S (%)";

                        colFinMw = colIniMw;
                        ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.Font.Size = 28;
                        ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Merge = true;

                        ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.WrapText = true;
                        ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        colIniMw = colFinMw + 1;
                    }
                }

                int colFinNombreReporte = colFinEmp;

                var colorBorder = Color.White;
                var colorBorderHora = Color.Black;
                var classTipoEmpresa = "#4472C4";
                var classTipoPes = "#9BC2E6";
                var classTiporcen = "#9BC2E6";
                using (var range = ws.Cells[rowIniFecha, colIniSein, rowFinFecha, colIniSein])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Font.Bold = true;
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin, colorBorderHora);
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Color.SetColor(colorBorderHora);
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(colorBorderHora);
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Color.SetColor(colorBorderHora);
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Color.SetColor(colorBorderHora);
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(classTipoPes));
                    range.Style.Font.Color.SetColor(Color.Black);
                }

                using (var range = ws.Cells[rowIniNombreReporte, colIniNombreReporte, rowIniMw, (colFinNombreReporte / 2) + 2])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Font.Bold = true;
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin, colorBorder);
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Color.SetColor(colorBorder);
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(colorBorder);
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Color.SetColor(colorBorder);
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Color.SetColor(colorBorder);
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(classTipoEmpresa));
                    range.Style.Font.Color.SetColor(Color.White);
                }

                using (var range = ws.Cells[rowIniNombreReporte, (colFinNombreReporte / 2) + 3, rowIniMw, colFinNombreReporte])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Font.Bold = true;
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin, colorBorder);
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Color.SetColor(colorBorder);
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(colorBorder);
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Color.SetColor(colorBorder);
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Color.SetColor(colorBorder);
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(classTiporcen));
                    range.Style.Font.Color.SetColor(Color.Black);
                }

                #endregion

                int rowIniData = rowIniMw + 1;
                row = rowIniData;

                #region cuerpo

                int numDia = 0;

                int colData = colIniFecha;
                for (var day = fechaIni.Date; day.Date <= fechaFin; day = day.AddDays(1))
                {
                    numDia++;
                    DateTime horas = day.AddMinutes(30);

                    List<MeMedicion48DTO> data = dataTotal.Where(x => x.Medifecha == day).ToList();
                    MeMedicion48DTO total = data.Find(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaTotalPotencia);

                    for (int h = 1; h <= 48; h++)
                    {
                        //Fecha
                        ws.Cells[row, colIniFecha].Value = horas.ToString(ConstantesAppServicio.FormatoFechaHora);
                        ws.Cells[row, colIniFecha].Style.Font.Size = 28;
                        ws.Cells[row, colIniFecha].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        //PES
                        colData = colIniFecha;
                        colData++;

                        decimal? potenciaTotal = null;
                        if (total != null)
                        {
                            potenciaTotal = (decimal?)total.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(total, null);
                        }

                        ws.Cells[row, colData].Value = potenciaTotal.GetValueOrDefault(0);

                        //PE / S
                        colData++;
                        for (int i = 1; i <= 2; i++)
                        {
                            foreach (var empresa in listaEmpresa)
                            {
                                ws.Cells[row, colData].Style.WrapText = true;
                                ws.Cells[row, colData].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                MeMedicion48DTO reg = data.Find(x => x.Emprcodi == empresa.Emprcodi && x.TipoFormulaMonitoreo == i);
                                decimal? pot = null;
                                if (reg != null)
                                {
                                    pot = (decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg, null);
                                }

                                if (pot.GetValueOrDefault(0) == 0)
                                {
                                    ws.Cells[row, colData].Value = string.Empty;
                                }
                                else
                                {
                                    ws.Cells[row, colData].Value = pot;
                                }
                                colData++;
                            }
                        }

                        horas = horas.AddMinutes(30);
                        row++;
                    }
                }

                colData--;

                using (var range = ws.Cells[rowIniData, colIniFecha, (rowIniData + numDia * 48) - 1, colData])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.White);
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Style = ExcelBorderStyle.Dotted;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                }

                //Formato Data
                using (var range = ws.Cells[rowIniData, colIniFecha + 1, (rowIniData + numDia * 48) - 1, colData])
                {
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.Style.Font.Size = 28;
                    range.Style.Numberformat.Format = "#,##0.000";
                }

                for (int f = rowIniData + 48; f <= rowIniData + numDia * 48; f += 48)
                {
                    ws.Cells[f, colIniFecha, f, colData].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                }

                //Formato de Filas y columnas
                for (int columna = colIniSein; columna <= colData; columna++)
                    ws.Column(columna).Width = 40;

                ws.Column(colIniFecha).Width = 50;
                ws.Column(colIniSein).Width = 28;
                ws.Row(rowIniNombreReporte).Height = 60;
                ws.Row(rowIniEmp).Height = 410;
                ws.Row(rowIniMw).Height = 200;

                #endregion

                ws.View.FreezePanes(rowFinFecha + 1, colFinFreeze + 1);
            }
            ws.View.ZoomScale = 30;
        }

        /// <summary>
        /// Generación de Reporte Excel 2
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="ws"></param>
        /// <param name="nombreSheet"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="vermmcodi"></param>
        public void GenerarExcelReporte2(string empresas, ExcelWorksheet ws, DateTime fechaIni, DateTime fechaFin, int rowIni, int colIni, int colFinFreeze, int vermmcodi, List<MmmDatoDTO> listFacTable)
        {
            SiParametroValorDTO param = this.GetParametroTendenciaHHI(fechaIni);
            decimal tendenciaUno = param.HHITendenciaUno.GetValueOrDefault(0);
            decimal tendenciaCero = param.HHITendenciaCero.GetValueOrDefault(0);
            string colorUno = param.HHITendenciaUnoColor;
            string colorCero = param.HHITendenciaCeroColor;

            List<SiEmpresaDTO> listaEmpresa;
            List<MeMedicion48DTO> dataTotal = this.ListarIndicador(vermmcodi, fechaIni, fechaFin, ConstantesMonitoreo.CodigoHHI, listFacTable, ConstantesMonitoreo.ReportesIndicadores, out listaEmpresa);
            listaEmpresa = this.ListarEmpresasMonitoreoByData(listaEmpresa, empresas);

            int row = rowIni + 5;
            int col = colIni;
            //
            if (listaEmpresa.Count > 0)
            {
                #region cabecera
                int rowIniNombreReporte = row;
                /// Fila Hora - Empresa - Total

                int colIniFecha = col;
                int rowIniFecha = row;
                int rowFinFecha = rowIniFecha + 2 - 1;
                ws.Cells[rowIniFecha, colIniFecha].Value = "MM/DD/hh:mm";
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Merge = true;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.WrapText = true;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Font.Bold = true;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Font.Size = 28;

                int rowIniEmp = row;
                int colIniEmp = colIniFecha + 1;
                int colFinEmp = colIniEmp;

                int colIniNombreReporte = colIniEmp;

                for (int i = 0; i < listaEmpresa.Count; i++)
                {
                    //Empresa
                    var thEmp = listaEmpresa[i];
                    colFinEmp = colIniEmp;
                    ws.Cells[rowIniEmp, colIniEmp].Value = thEmp.Emprnomb.Trim();
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.Font.Size = 24;
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Merge = true;
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.WrapText = true;
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.TextRotation = 90;
                    colIniEmp = colFinEmp + 1;
                }

                colFinEmp = colIniEmp;
                ws.Cells[rowIniEmp, colIniEmp, rowIniEmp + 1, colFinEmp].Value = "HHI";
                ws.Cells[rowIniEmp, colIniEmp, rowIniEmp + 1, colFinEmp].Merge = true;
                ws.Cells[rowIniEmp, colIniEmp, rowIniEmp + 1, colFinEmp].Style.WrapText = true;
                ws.Cells[rowIniEmp, colIniEmp, rowIniEmp + 1, colFinEmp].Style.Font.Size = 28;
                ws.Cells[rowIniEmp, colIniEmp, rowIniEmp + 1, colFinEmp].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniEmp, colIniEmp, rowIniEmp + 1, colFinEmp].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniEmp, colIniEmp, rowIniEmp + 1, colFinEmp].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                colIniEmp = colFinEmp + 1;

                int rowIniPorc = row + 1;
                int colIniPorc = colIniFecha + 1;
                int colFinPorc = colIniPorc;

                for (int i = 0; i < listaEmpresa.Count; i++)
                {
                    colFinPorc = colIniPorc;

                    ws.Cells[rowIniPorc, colIniPorc].IsRichText = true;
                    ExcelRichTextCollection rtfCollection = ws.Cells[rowIniPorc, colIniPorc].RichText;
                    ExcelRichText ert = null;
                    ert = rtfCollection.Add("S");
                    ert.FontName = "Arial";
                    ert.Color = Color.White;
                    ert.Size = 28;
                    ert.Bold = true;

                    ert = rtfCollection.Add("2");
                    ert.VerticalAlign = ExcelVerticalAlignmentFont.Superscript;
                    ert.FontName = "Arial";
                    ert.Color = Color.White;
                    ert.Size = 28;
                    ert.Bold = true;

                    ws.Cells[rowIniPorc, colIniPorc].Style.Font.Size = 28;
                    ws.Cells[rowIniPorc, colIniPorc].Style.WrapText = true;
                    ws.Cells[rowIniPorc, colIniPorc].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniPorc, colIniPorc].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[rowIniPorc, colIniPorc].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    colIniPorc = colFinPorc + 1;
                }

                colFinPorc = colIniPorc;

                colIniPorc = colFinPorc + 1;

                //Nombre Reporte
                int colFinNombreReporte = colFinEmp;

                var colorBorder = Color.White;
                var classTipoEmpresa = "#4472C4";

                var colorBorderHora = Color.Black;

                using (var range = ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Font.Bold = true;
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin, colorBorderHora);
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Color.SetColor(colorBorderHora);
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(colorBorderHora);
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Color.SetColor(colorBorderHora);
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Color.SetColor(colorBorderHora);
                    range.Style.Fill.BackgroundColor.SetColor(Color.White);
                    range.Style.Font.Color.SetColor(Color.Black);
                }

                using (var range = ws.Cells[rowIniNombreReporte, colIniNombreReporte, rowIniPorc, colFinNombreReporte])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Font.Bold = true;
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin, colorBorder);
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Color.SetColor(colorBorder);
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(colorBorder);
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Color.SetColor(colorBorder);
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Color.SetColor(colorBorder);
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(classTipoEmpresa));
                    range.Style.Font.Color.SetColor(Color.White);
                }

                #endregion

                int rowIniData = rowIniPorc + 1;
                row = rowIniData;

                #region cuerpo

                int numDia = 0;

                int colData = colIniFecha;
                for (var day = fechaIni.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
                {
                    numDia++;
                    DateTime horas = day.AddMinutes(30);

                    List<MeMedicion48DTO> data = dataTotal.Where(x => x.Medifecha == day).ToList();
                    MeMedicion48DTO total = data.Find(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaHHI);

                    for (int h = 1; h <= 48; h++)
                    {
                        //Fecha
                        ws.Cells[row, colIniFecha].Value = horas.ToString(ConstantesAppServicio.FormatoFechaHora);
                        ws.Cells[row, colIniFecha].Style.Font.Size = 28;
                        ws.Cells[row, colIniFecha].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        colData = colIniFecha;

                        colData++;

                        foreach (var empresa in listaEmpresa)
                        {
                            // Cuota para HHI
                            MeMedicion48DTO reg = data.Find(x => x.Emprcodi == empresa.Emprcodi && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaHHICuotaMercado);
                            decimal? cuotaHHI = null;
                            if (reg != null)
                            {
                                cuotaHHI = (decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg, null);
                            }
                            //elemento al  cuadrado
                            ws.Cells[row, colData].Value = cuotaHHI;
                            colData++;
                        }

                        decimal totalHhi = 0;
                        if (total != null)
                        {
                            totalHhi = ((decimal?)total.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(total, null)).GetValueOrDefault(0);
                        }

                        if (totalHhi >= tendenciaUno)
                        {
                            ws.Cells[row, colData].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[row, colData].Style.Font.Color.SetColor(Color.White);
                            ws.Cells[row, colData].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(param.HHITendenciaUnoColor));
                            ws.Cells[row, colData].Value = totalHhi;
                        }
                        else if (totalHhi <= tendenciaCero)
                        {
                            ws.Cells[row, colData].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[row, colData].Style.Font.Color.SetColor(Color.White);
                            ws.Cells[row, colData].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(param.HHITendenciaCeroColor));
                            ws.Cells[row, colData].Value = totalHhi;
                        }
                        else if (totalHhi > 0)
                        {
                            ws.Cells[row, colData].Value = (totalHhi);
                        }
                        colData++;

                        horas = horas.AddMinutes(30);
                        row++;
                    }
                }

                colData--;

                using (var range = ws.Cells[rowIniData, colIniFecha, (rowIniData + numDia * 48) - 1, colData])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.White);
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Style = ExcelBorderStyle.Dotted;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                }

                using (var range = ws.Cells[rowIniData, colIniFecha + 1, (rowIniData + numDia * 48) - 1, colData])
                {
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.Style.Font.Size = 28;
                    range.Style.Numberformat.Format = "#,##0.000";
                }

                for (int f = rowIniData + 48; f <= rowIniData + numDia * 48; f += 48)
                {
                    ws.Cells[f, colIniFecha, f, colData].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                }

                //Formato de Filas y columnas
                for (int columna = colIniFecha + 1; columna <= colData; columna++)
                    ws.Column(columna).Width = 40;

                ws.Column(colIniFecha).Width = 50;
                ws.Row(rowIniNombreReporte).Height = 60;
                ws.Row(rowIniEmp).Height = 400;
                ws.Row(rowIniPorc).Height = 200;

                #endregion

                ws.View.FreezePanes(rowFinFecha + 1, colFinFreeze + 1);
            }
            ws.View.ZoomScale = 30;
        }

        /// <summary>
        /// Generación de Reporte Excel 3
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="ws"></param>
        /// <param name="nombreSheet"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="vermmcodi"></param>
        public void GenerarExcelReporte3(string empresas, ExcelWorksheet ws, DateTime fechaIni, DateTime fechaFin, int rowIni, int colIni, int colFinFreeze, int vermmcodi, List<MmmDatoDTO> listFacTable)
        {
            SiParametroValorDTO param = this.GetParametroOfertaPivotal(fechaIni);
            string colorEsPivotal = param.IOPEsPivotalColor;
            string colorNoPivotal = param.IOPNoPivotalColor;

            List<SiEmpresaDTO> listaEmpresa;
            List<MeMedicion48DTO> dataTotal = this.ListarIndicador(vermmcodi, fechaIni, fechaFin, ConstantesMonitoreo.CodigoIOP, listFacTable, ConstantesMonitoreo.ReportesIndicadores, out listaEmpresa);
            listaEmpresa = this.ListarEmpresasMonitoreoByData(listaEmpresa, empresas);

            int row = rowIni + 5;
            int col = colIni;
            //
            if (listaEmpresa.Count > 0)
            {
                #region cabecera

                int rowIniNombreReporte = row;
                /// Fila Hora - Empresa - Total

                int colIniFecha = col;
                int rowIniFecha = row;
                int rowFinFecha = rowIniFecha + 2 - 1;
                ws.Cells[rowIniFecha, colIniFecha].Value = "MM/DD/hh:mm";
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Merge = true;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.WrapText = true;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Font.Size = 28;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Fill.BackgroundColor.SetColor(Color.Black);
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Border.Top.Color.SetColor(Color.Black);

                int colIniMd = colIniFecha + 1;
                int rowIniMd = rowIniFecha;
                int rowFinMd = rowFinFecha;
                ws.Cells[rowIniMd, colIniMd].Value = "MD (MW)";
                ws.Cells[rowIniMd, colIniMd, rowFinMd, colIniMd].Merge = true;
                ws.Cells[rowIniMd, colIniMd, rowFinMd, colIniMd].Style.WrapText = true;
                ws.Cells[rowIniMd, colIniMd, rowFinMd, colIniMd].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniMd, colIniMd, rowFinMd, colIniMd].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniMd, colIniMd, rowFinMd, colIniMd].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[rowIniMd, colIniMd, rowFinMd, colIniMd].Style.Font.Size = 28;
                ws.Cells[rowIniMd, colIniMd, rowFinMd, colIniMd].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[rowIniMd, colIniMd, rowFinMd, colIniMd].Style.Fill.BackgroundColor.SetColor(Color.Black);
                ws.Cells[rowIniMd, colIniMd, rowFinMd, colIniMd].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[rowIniMd, colIniMd, rowFinMd, colIniMd].Style.Border.Top.Color.SetColor(Color.Black);

                int colIniSein = colIniMd + 1;
                int rowIniSein = rowIniFecha;
                int rowFinSein = rowFinFecha;
                ws.Cells[rowIniSein, colIniSein].Value = "PES (MW)";
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Merge = true;
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.WrapText = true;
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.Font.Size = 28;
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.Fill.BackgroundColor.SetColor(Color.Black);
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.Border.Top.Color.SetColor(Color.Black);

                int rowIniEmp = row;
                int colIniEmp = colIniSein + 1;
                int colFinEmp = colIniEmp;

                int colIniNombreReporte = colIniEmp;

                for (int j = 1; j <= 2; j++)
                {
                    for (int i = 0; i < listaEmpresa.Count; i++)
                    {
                        //Empresa
                        var thEmp = listaEmpresa[i];
                        colFinEmp = colIniEmp;
                        ws.Cells[rowIniEmp, colIniEmp].Value = thEmp.Emprnomb.Trim();
                        ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.Font.Size = 24;
                        ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Merge = true;
                        ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.WrapText = true;
                        ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.TextRotation = 90;
                        colIniEmp = colFinEmp + 1;
                    }
                }

                int rowIniMw = row + 1;
                int colIniMw = colIniFecha + 1;
                int colFinMw = colIniMw;

                ws.Cells[rowIniMw, colIniMw].Value = "MD (MW)";
                colIniMw++;

                ws.Cells[rowIniMw, colIniMw].Value = "PES (MW)";

                colIniMw++;
                for (int j = 1; j <= 2; j++)
                {
                    for (int i = 0; i < listaEmpresa.Count; i++)
                    {
                        ws.Cells[rowIniMw, colIniMw].Value = j == 1 ? "PE (MW)" : "IOP";

                        colFinMw = colIniMw;
                        ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.Font.Size = 28;
                        ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Merge = true;
                        ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.WrapText = true;
                        ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        colIniMw = colFinMw + 1;
                    }
                }

                //Nombre Reporte
                colIniNombreReporte = colIniNombreReporte - 3;
                int colFinNombreReporte = colFinEmp;

                var colorBorder = Color.White;
                var colorBorderHora = Color.Black;
                var classTipoEmpresa = "#4472C4";
                var classTipoSein = "#D9E1F2";
                var classHora = "#F9FBFB";
                var classTipoIop = "#D9E1F2";

                //Fecha
                using (var range = ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Font.Bold = true;
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin, colorBorderHora);
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Color.SetColor(colorBorderHora);
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(colorBorderHora);
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Color.SetColor(colorBorderHora);
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Color.SetColor(colorBorderHora);
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(classHora));
                    range.Style.Font.Color.SetColor(Color.Black);
                }

                //MD
                using (var range = ws.Cells[rowIniMd, colIniMd, rowFinMd, colIniSein])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Font.Bold = true;
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin, colorBorderHora);
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Color.SetColor(colorBorderHora);
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(colorBorderHora);
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Color.SetColor(colorBorderHora);
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Color.SetColor(colorBorderHora);
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(classTipoSein));
                    range.Style.Font.Color.SetColor(Color.Black);
                }

                //PE
                using (var range = ws.Cells[rowIniNombreReporte, colIniNombreReporte + 3, rowIniMw, (colFinNombreReporte / 2) + 2])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Font.Bold = true;
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin, colorBorder);
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Color.SetColor(colorBorder);
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(colorBorder);
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Color.SetColor(colorBorder);
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Color.SetColor(colorBorder);
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(classTipoEmpresa));
                    range.Style.Font.Color.SetColor(Color.White);
                }

                //IOP
                using (var range = ws.Cells[rowIniNombreReporte, (colFinNombreReporte / 2) + 3, rowIniMw, colFinNombreReporte])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Font.Bold = true;
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin, colorBorder);
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Color.SetColor(colorBorder);
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(colorBorder);
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Color.SetColor(colorBorder);
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Color.SetColor(colorBorder);
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(classTipoIop));
                    range.Style.Font.Color.SetColor(Color.Black);
                }

                #endregion

                int rowIniData = rowIniMw + 1;
                row = rowIniData;

                #region cuerpo

                int numDia = 0;

                int colData = colIniFecha;
                for (var day = fechaIni.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
                {
                    numDia++;

                    List<MeMedicion48DTO> data = dataTotal.Where(x => x.Medifecha == day).ToList();
                    MeMedicion48DTO total = data.Find(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaTotalPotencia);
                    MeMedicion48DTO md = data.Find(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaMaximaDemanda);

                    DateTime horas = day.AddMinutes(30);

                    for (int h = 1; h <= 48; h++)
                    {
                        //Fecha
                        ws.Cells[row, colIniFecha].Value = horas.ToString(ConstantesAppServicio.FormatoFechaHora);
                        ws.Cells[row, colIniFecha].Style.Font.Size = 28;
                        ws.Cells[row, colIniFecha].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[row, colIniFecha].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[row, colIniFecha].Style.Fill.BackgroundColor.SetColor(Color.White);
                        colData = colIniFecha + 1;

                        //Maxima Demanda Programada
                        decimal? maximademanda = null;
                        if (md != null)
                        {
                            maximademanda = (decimal?)md.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(md, null);
                        }
                        ws.Cells[row, colData].Value = maximademanda;
                        ws.Cells[row, colData].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[row, colData].Style.Fill.BackgroundColor.SetColor(Color.White);
                        colData++;

                        //Potencia Total
                        decimal? potenciaTotal = null;
                        if (total != null)
                        {
                            potenciaTotal = (decimal?)total.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(total, null);
                        }
                        ws.Cells[row, colData].Value = potenciaTotal;
                        ws.Cells[row, colData].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[row, colData].Style.Fill.BackgroundColor.SetColor(Color.White);
                        colData++;

                        //Potencia x Empresa
                        foreach (var empresa in listaEmpresa)
                        {
                            MeMedicion48DTO reg = data.Find(x => x.Medifecha == day && x.Emprcodi == empresa.Emprcodi && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaMWEjec);
                            decimal? pot = null;
                            if (reg != null)
                            {
                                pot = (decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg, null);
                            }

                            ws.Cells[row, colData].Value = pot;
                            ws.Cells[row, colData].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[row, colData].Style.Fill.BackgroundColor.SetColor(Color.White);
                            colData++;
                        }

                        //IOP x Empresa
                        foreach (var empresa in listaEmpresa)
                        {
                            MeMedicion48DTO reg = data.Find(x => x.Medifecha == day && x.Emprcodi == empresa.Emprcodi && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaIOP);
                            int iop = ConstantesMonitoreo.ValorIOPEsPivotal;

                            if (reg != null)
                            {
                                iop = Convert.ToInt32(((decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg, null)).GetValueOrDefault(0));
                            }

                            string color = ConstantesMonitoreo.ValorIOPEsPivotal == iop ? colorEsPivotal : colorNoPivotal;

                            ws.Cells[row, colData].Value = iop;
                            ws.Cells[row, colData].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[row, colData].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(color));
                            ws.Cells[row, colData].Style.Font.Color.SetColor(Color.White);

                            colData++;
                        }

                        horas = horas.AddMinutes(30);
                        row++;
                    }
                }

                colData--;

                using (var range = ws.Cells[rowIniData, colIniFecha, (rowIniData + numDia * 48) - 1, colData])
                {
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Style = ExcelBorderStyle.Dotted;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                }

                using (var range = ws.Cells[rowIniData, colIniFecha + 1, rowIniData + numDia * 48, colData])
                {
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.Style.Font.Size = 28;
                    range.Style.Numberformat.Format = "#,##0.000";
                }

                for (int f = rowIniData + 48; f <= rowIniData + numDia * 48; f += 48)
                {
                    ws.Cells[f, colIniFecha, f, colData].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                }

                //Formato de Filas y columnas
                for (int columna = colIniFecha + 1; columna <= colData; columna++)
                    ws.Column(columna).Width = 40;

                ws.Column(colIniFecha).Width = 50;

                ws.Row(rowIniEmp).Height = 400;
                ws.Row(rowIniMw).Height = 200;

                #endregion

                #region Leyenda
                int rowLeyenda = row + 3;
                int rowPivotal = row + 4;
                int rowNoPivotal = row + 5;

                ws.Cells[rowLeyenda, colIniFecha].Value = "Leyenda:";
                ws.Cells[rowLeyenda, colIniFecha].Style.Font.Size = 30;
                ws.Cells[rowLeyenda, colIniFecha].Style.Font.Bold = true;

                ws.Cells[rowPivotal, colIniMd].Value = " '" + ConstantesMonitoreo.ValorIOPEsPivotal + "' (Pivotal) ";
                ws.Cells[rowPivotal, colIniMd].Style.Font.Size = 30;
                ws.Cells[rowPivotal, colIniMd].Style.Font.Bold = true;
                ws.Cells[rowPivotal, colIniFecha].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[rowPivotal, colIniFecha].Style.Border.BorderAround(ExcelBorderStyle.Thin, colorBorder);
                ws.Cells[rowPivotal, colIniFecha].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[rowPivotal, colIniFecha].Style.Border.Top.Color.SetColor(colorBorder);
                ws.Cells[rowPivotal, colIniFecha].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                ws.Cells[rowPivotal, colIniFecha].Style.Border.Bottom.Color.SetColor(colorBorder);
                ws.Cells[rowPivotal, colIniFecha].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[rowPivotal, colIniFecha].Style.Border.Left.Color.SetColor(colorBorder);
                ws.Cells[rowPivotal, colIniFecha].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[rowPivotal, colIniFecha].Style.Border.Right.Color.SetColor(colorBorder);
                ws.Cells[rowPivotal, colIniFecha].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(colorEsPivotal));
                ws.Cells[rowPivotal, colIniFecha].Style.Font.Color.SetColor(Color.Black);

                ws.Cells[rowNoPivotal, colIniMd].Value = " '" + ConstantesMonitoreo.ValorIOPNoPivotal + "' (No Pivotal) ";
                ws.Cells[rowNoPivotal, colIniMd].Style.Font.Size = 30;
                ws.Cells[rowNoPivotal, colIniMd].Style.Font.Bold = true;
                ws.Cells[rowNoPivotal, colIniFecha].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[rowNoPivotal, colIniFecha].Style.Border.BorderAround(ExcelBorderStyle.Thin, colorBorder);
                ws.Cells[rowNoPivotal, colIniFecha].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[rowNoPivotal, colIniFecha].Style.Border.Top.Color.SetColor(colorBorder);
                ws.Cells[rowNoPivotal, colIniFecha].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                ws.Cells[rowNoPivotal, colIniFecha].Style.Border.Bottom.Color.SetColor(colorBorder);
                ws.Cells[rowNoPivotal, colIniFecha].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[rowNoPivotal, colIniFecha].Style.Border.Left.Color.SetColor(colorBorder);
                ws.Cells[rowNoPivotal, colIniFecha].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[rowNoPivotal, colIniFecha].Style.Border.Right.Color.SetColor(colorBorder);
                ws.Cells[rowNoPivotal, colIniFecha].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(colorNoPivotal));
                ws.Cells[rowNoPivotal, colIniFecha].Style.Font.Color.SetColor(Color.Black);

                #endregion

                ws.View.FreezePanes(rowFinFecha + 1, colFinFreeze + 1);
            }
            ws.View.ZoomScale = 30;
        }

        /// <summary>
        /// Generación de Reporte Excel 4
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="ws"></param>
        /// <param name="nombreSheet"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="vermmcodi"></param>
        public void GenerarExcelReporte4(string empresas, ExcelWorksheet ws, DateTime fechaIni, DateTime fechaFin, int rowIni, int colIni, int colFinFreeze, int vermmcodi, List<MmmDatoDTO> listFacTable)
        {
            List<SiEmpresaDTO> listaEmpresa;
            List<MeMedicion48DTO> dataTotal = this.ListarIndicador(vermmcodi, fechaIni, fechaFin, ConstantesMonitoreo.CodigoRSD, listFacTable, ConstantesMonitoreo.ReportesIndicadores, out listaEmpresa);
            listaEmpresa = this.ListarEmpresasMonitoreoByData(listaEmpresa, empresas);

            int row = rowIni + 5;
            int col = colIni;
            //
            if (listaEmpresa.Count > 0)
            {
                #region cabecera

                int rowIniNombreReporte = row;
                /// Fila Hora - Empresa - Total

                int colIniFecha = col;
                int rowIniFecha = row;
                int rowFinFecha = rowIniFecha + 2 - 1;
                ws.Cells[rowIniFecha, colIniFecha].Value = "MM/DD/hh:mm";
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Merge = true;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.WrapText = true;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Font.Size = 28;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Fill.BackgroundColor.SetColor(Color.Black);
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Border.Top.Color.SetColor(Color.Black);

                int colIniMd = colIniFecha + 1;
                int rowIniMd = rowIniFecha;
                int rowFinMd = rowFinFecha;
                ws.Cells[rowIniMd, colIniMd].Value = "MD (MW)";
                ws.Cells[rowIniMd, colIniMd, rowFinMd, colIniMd].Merge = true;
                ws.Cells[rowIniMd, colIniMd, rowFinMd, colIniMd].Style.WrapText = true;
                ws.Cells[rowIniMd, colIniMd, rowFinMd, colIniMd].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniMd, colIniMd, rowFinMd, colIniMd].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniMd, colIniMd, rowFinMd, colIniMd].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[rowIniMd, colIniMd, rowFinMd, colIniMd].Style.Font.Size = 28;
                ws.Cells[rowIniMd, colIniMd, rowFinMd, colIniMd].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[rowIniMd, colIniMd, rowFinMd, colIniMd].Style.Fill.BackgroundColor.SetColor(Color.Black);
                ws.Cells[rowIniMd, colIniMd, rowFinMd, colIniMd].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[rowIniMd, colIniMd, rowFinMd, colIniMd].Style.Border.Top.Color.SetColor(Color.Black);

                int colIniSein = colIniMd + 1;
                int rowIniSein = rowIniFecha;
                int rowFinSein = rowFinFecha;
                ws.Cells[rowIniSein, colIniSein].Value = "PES (MW)";
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Merge = true;
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.WrapText = true;
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.Font.Size = 28;
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.Fill.BackgroundColor.SetColor(Color.Black);
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.Border.Top.Color.SetColor(Color.Black);

                int rowIniEmp = row;
                int colIniEmp = colIniSein + 1;
                int colFinEmp = colIniEmp;

                int colIniNombreReporte = colIniEmp;

                for (int j = 1; j <= 2; j++)
                {
                    for (int i = 0; i < listaEmpresa.Count; i++)
                    {
                        //Empresa
                        var thEmp = listaEmpresa[i];
                        colFinEmp = colIniEmp;
                        ws.Cells[rowIniEmp, colIniEmp].Value = thEmp.Emprnomb.Trim();
                        ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.Font.Size = 24;
                        ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Merge = true;
                        ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.WrapText = true;
                        ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.TextRotation = 90;
                        colIniEmp = colFinEmp + 1;
                    }
                }

                int rowIniMw = row + 1;
                int colIniMw = colIniFecha + 1;
                int colFinMw = colIniMw;

                ws.Cells[rowIniMw, colIniMw].Value = "MD (MW)";
                colIniMw++;

                ws.Cells[rowIniMw, colIniMw].Value = "PES (MW)";

                colIniMw++;
                for (int j = 1; j <= 2; j++)
                {
                    for (int i = 0; i < listaEmpresa.Count; i++)
                    {
                        ws.Cells[rowIniMw, colIniMw].Value = j == 1 ? "PE (MW)" : "RSD";

                        colFinMw = colIniMw;
                        ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.Font.Size = 28;
                        ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Merge = true;
                        ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.WrapText = true;
                        ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        colIniMw = colFinMw + 1;
                    }
                }

                //Nombre Reporte
                colIniNombreReporte = colIniNombreReporte - 3;
                int colFinNombreReporte = colFinEmp;

                var colorBorder = Color.White;
                var colorBorderHora = Color.Black;
                var classTipoEmpresa = "#4472C4";
                var classTipoSein = "#D9E1F2";
                var classHora = "#F9FBFB";
                var classTipoRSD = "#D9E1F2";

                //Fecha
                using (var range = ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Font.Bold = true;
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin, colorBorderHora);
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Color.SetColor(colorBorderHora);
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(colorBorderHora);
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Color.SetColor(colorBorderHora);
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Color.SetColor(colorBorderHora);
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(classHora));
                    range.Style.Font.Color.SetColor(Color.Black);
                }

                //MD
                using (var range = ws.Cells[rowIniMd, colIniMd, rowFinMd, colIniSein])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Font.Bold = true;
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin, colorBorderHora);
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Color.SetColor(colorBorderHora);
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(colorBorderHora);
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Color.SetColor(colorBorderHora);
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Color.SetColor(colorBorderHora);
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(classTipoSein));
                    range.Style.Font.Color.SetColor(Color.Black);
                }

                //PE
                using (var range = ws.Cells[rowIniNombreReporte, colIniNombreReporte + 3, rowIniMw, (colFinNombreReporte / 2) + 2])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Font.Bold = true;
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin, colorBorder);
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Color.SetColor(colorBorder);
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(colorBorder);
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Color.SetColor(colorBorder);
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Color.SetColor(colorBorder);
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(classTipoEmpresa));
                    range.Style.Font.Color.SetColor(Color.White);
                }

                //RSD
                using (var range = ws.Cells[rowIniNombreReporte, (colFinNombreReporte / 2) + 3, rowIniMw, colFinNombreReporte])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Font.Bold = true;
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin, colorBorder);
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Color.SetColor(colorBorder);
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(colorBorder);
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Color.SetColor(colorBorder);
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Color.SetColor(colorBorder);
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(classTipoRSD));
                    range.Style.Font.Color.SetColor(Color.Black);
                }

                #endregion

                int rowIniData = rowIniMw + 1;
                row = rowIniData;

                #region cuerpo

                int numDia = 0;

                int colData = colIniFecha;
                for (var day = fechaIni.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
                {
                    numDia++;

                    List<MeMedicion48DTO> data = dataTotal.Where(x => x.Medifecha == day).ToList();
                    MeMedicion48DTO total = data.Find(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaTotalPotencia);
                    MeMedicion48DTO md = data.Find(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaMaximaDemanda);

                    DateTime horas = day.AddMinutes(30);

                    for (int h = 1; h <= 48; h++)
                    {
                        //Fecha
                        ws.Cells[row, colIniFecha].Value = horas.ToString(ConstantesAppServicio.FormatoFechaHora);
                        ws.Cells[row, colIniFecha].Style.Font.Size = 28;
                        ws.Cells[row, colIniFecha].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        colData = colIniFecha + 1;

                        //Maxima Demanda Programada
                        decimal? maximademanda = null;
                        if (md != null)
                        {
                            maximademanda = (decimal?)md.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(md, null);
                        }
                        ws.Cells[row, colData].Value = maximademanda;
                        colData++;

                        //Potencia Total
                        decimal? potenciaTotal = null;
                        if (total != null)
                        {
                            potenciaTotal = (decimal?)total.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(total, null);
                        }

                        ws.Cells[row, colData].Value = potenciaTotal;
                        colData++;

                        //Potencia x Empresa
                        foreach (var empresa in listaEmpresa)
                        {
                            MeMedicion48DTO reg = data.Find(x => x.Medifecha == day && x.Emprcodi == empresa.Emprcodi && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaMWEjec);
                            decimal? pot = null;
                            if (reg != null)
                            {
                                pot = (decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg, null);
                            }
                            if (pot.GetValueOrDefault(0) != 0)
                                ws.Cells[row, colData].Value = pot;
                            colData++;
                        }

                        //IOR x Empresa
                        foreach (var empresa in listaEmpresa)
                        {
                            MeMedicion48DTO reg = data.Find(x => x.Medifecha == day && x.Emprcodi == empresa.Emprcodi && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaRSD);
                            decimal? ior = null;

                            if (reg != null)
                            {
                                ior = ((decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg, null)).GetValueOrDefault(0);
                            }

                            if (ior.GetValueOrDefault(0) != 0)
                                ws.Cells[row, colData].Value = ior;

                            colData++;
                        }

                        horas = horas.AddMinutes(30);
                        row++;
                    }
                }

                colData--;

                using (var range = ws.Cells[rowIniData, colIniFecha, (rowIniData + numDia * 48) - 1, colData])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.White);
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Style = ExcelBorderStyle.Dotted;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                }

                using (var range = ws.Cells[rowIniData, colIniFecha + 1, rowIniData + numDia * 48, colData])
                {
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.Style.Font.Size = 28;
                    range.Style.Numberformat.Format = "#,##0.000";
                }

                for (int f = rowIniData + 48; f <= rowIniData + numDia * 48; f += 48)
                {
                    ws.Cells[f, colIniFecha, f, colData].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                }

                //Formato de Filas y columnas
                for (int columna = colIniFecha + 1; columna <= colData; columna++)
                    ws.Column(columna).Width = 40;

                ws.Column(colIniFecha).Width = 50;

                ws.Row(rowIniEmp).Height = 400;
                ws.Row(rowIniMw).Height = 200;

                #endregion

                ws.View.FreezePanes(rowFinFecha + 1, colFinFreeze + 1);
            }
            ws.View.ZoomScale = 30;
        }

        /// <summary>
        /// Generación de Reporte Excel 5
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="ws"></param>
        /// <param name="nombreSheet"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="vermmcodi"></param>
        public void GenerarExcelReporte5(string empresas, ExcelWorksheet ws, DateTime fechaIni, DateTime fechaFin, int rowIni, int colIni, int colFinFreeze, int vermmcodi, List<MmmDatoDTO> listFacTable)
        {
            List<SiEmpresaDTO> listaEmpresa;
            List<MeMedicion48DTO> data = this.ListarIndicador(vermmcodi, fechaIni, fechaFin, ConstantesMonitoreo.CodigoILE, listFacTable, ConstantesMonitoreo.ReportesIndicadores, out listaEmpresa);
            listaEmpresa = this.ListarEmpresasMonitoreoByData(listaEmpresa, empresas);

            List<MeMedicion48DTO> listaBarra = data.Where(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCMEjec)
                .Select(x => new { Emprcodi = x.Emprcodi, Barrcodi = x.Barrcodi, Barrnombre = x.Barrnombre })
                .GroupBy(x => new { x.Emprcodi, x.Barrcodi, x.Barrnombre })
                .Select(x => new MeMedicion48DTO() { Emprcodi = x.Key.Emprcodi, Barrcodi = x.Key.Barrcodi, Barrnombre = x.Key.Barrnombre })
                .OrderBy(x => x.Barrnombre).ToList();

            int row = rowIni + 5;
            int col = colIni;
            //
            if (listaEmpresa.Count > 0)
            {
                #region cabecera

                int rowIniNombreReporte = row;
                /// Fila Hora - Empresa - Total

                int colIniFecha = col;
                int rowIniFecha = row;
                int rowFinFecha = rowIniFecha + 3 - 1;
                ws.Cells[rowIniFecha, colIniFecha].Value = "MM/DD/hh:mm";
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Font.Size = 28;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Merge = true;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Font.Bold = true;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.WrapText = true;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                int rowIniEmp = row;
                int colIniEmp = colIniFecha + 1;
                int colFinEmp = colIniEmp - 1;

                int colIniNombreReporte = colIniEmp;
                Color colorEmpresaImpar = ColorTranslator.FromHtml(ConstantesMonitoreo.ColorCmgEmpresaImpar);
                Color colorEmpresaPar = ColorTranslator.FromHtml(ConstantesMonitoreo.ColorCmgEmpresaPar);

                //Empresa
                int contEmpresa = 1;
                foreach (var empresa in listaEmpresa)
                {
                    int totalBarraXEmpr = listaBarra.Where(x => x.Emprcodi == empresa.Emprcodi).Count();
                    totalBarraXEmpr = totalBarraXEmpr > 0 ? totalBarraXEmpr : 1;

                    colFinEmp = colFinEmp + (3 * totalBarraXEmpr);

                    ws.Cells[rowIniEmp, colIniEmp].Value = empresa.Emprnomb;
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.Font.Size = 28;
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Merge = true;
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.WrapText = true;
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.Fill.BackgroundColor.SetColor(contEmpresa % 2 == 0 ? colorEmpresaPar : colorEmpresaImpar);

                    colIniEmp = colFinEmp + 1;
                    contEmpresa++;
                }

                int rowIniBarra = rowIniEmp + 1;
                int colIniBarra = colIniFecha + 1;
                int colFinBarra = colIniBarra - 1;

                //Barra X Empresa
                contEmpresa = 1;
                foreach (var empresa in listaEmpresa)
                {
                    var listaBarraXEmpr = listaBarra.Where(x => x.Emprcodi == empresa.Emprcodi).ToList();
                    int totalBarraXEmpr = listaBarraXEmpr.Count();
                    if (totalBarraXEmpr > 0)
                    {
                        foreach (var barra in listaBarraXEmpr)
                        {
                            colFinBarra = colFinBarra + 3;
                            ws.Cells[rowIniBarra, colIniBarra].Value = barra.Barrnombre;
                            ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.Font.Size = 28;
                            ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Merge = true;
                            ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.WrapText = true;
                            ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.Fill.BackgroundColor.SetColor(contEmpresa % 2 == 0 ? colorEmpresaPar : colorEmpresaImpar);
                            colIniBarra = colFinBarra + 1;
                        }
                    }
                    else
                    {
                        colFinBarra = colFinBarra + 3;
                        ws.Cells[rowIniBarra, colIniBarra].Value = "";
                        ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.Font.Size = 28;
                        ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Merge = true;
                        ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.WrapText = true;
                        ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.Fill.BackgroundColor.SetColor(contEmpresa % 2 == 0 ? colorEmpresaPar : colorEmpresaImpar);
                        colIniBarra = colFinBarra + 1;
                    }
                    contEmpresa++;
                }

                int rowIniMw = rowIniBarra + 1;
                int colIniMw = colIniFecha + 1;
                int colFinMw = colIniMw;

                //Valores x Barra
                contEmpresa = 1;
                foreach (var empresa in listaEmpresa)
                {
                    int totalBarraXEmpr = listaBarra.Where(x => x.Emprcodi == empresa.Emprcodi).Count();
                    totalBarraXEmpr = totalBarraXEmpr > 0 ? totalBarraXEmpr : 1;
                    for (int j = 0; j < totalBarraXEmpr; j++)
                    {
                        for (int i = 1; i <= 3; i++)
                        {
                            string valor; valor = (i == 1) ? " CMg  (k,m,t)" : (i == 2) ? " CV  (S/K wh)" : "ILE   (Índice de Lerner)";
                            ws.Cells[rowIniMw, colIniMw].Value = valor;
                            colFinMw = colIniMw;
                            ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.Font.Size = 28;
                            ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Merge = true;
                            ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.WrapText = true;
                            ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.Fill.BackgroundColor.SetColor(contEmpresa % 2 == 0 ? colorEmpresaPar : colorEmpresaImpar);
                            colIniMw = colFinMw + 1;
                        }
                    }
                    contEmpresa++;
                }
                int colFinNombreReporte = colFinEmp;


                var colorBorder = Color.Black;
                using (var range = ws.Cells[rowIniNombreReporte, colIniNombreReporte, rowIniMw, colFinNombreReporte])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Font.Bold = true;
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin, colorBorder);
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Color.SetColor(colorBorder);
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(colorBorder);
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Color.SetColor(colorBorder);
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Color.SetColor(colorBorder);
                    range.Style.Font.Color.SetColor(Color.Black);
                }

                #endregion

                int rowIniData = rowIniMw + 1;
                row = rowIniData;

                #region cuerpo

                int numDia = 0;

                int colData = colIniFecha + 1;

                for (var day = fechaIni.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
                {
                    List<MeMedicion48DTO> dataCMEjec = data.Where(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCMEjec).OrderBy(x => x.Emprnomb).ThenBy(x => x.Barrnombre).ToList();
                    List<MeMedicion48DTO> dataCV = data.Where(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCV).ToList();
                    List<MeMedicion48DTO> dataILE = data.Where(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaILE).ToList();

                    numDia++;
                    DateTime horas = day.AddMinutes(30);

                    for (int h = 1; h <= 48; h++)
                    {
                        colData = colIniFecha;
                        ws.Cells[row, colData].Value = horas.ToString(ConstantesAppServicio.FormatoFechaHora);

                        foreach (var empresa in listaEmpresa)
                        {
                            var listaBarraXEmpr = listaBarra.Where(x => x.Emprcodi == empresa.Emprcodi).ToList();
                            int totalBarraXEmpr = listaBarraXEmpr.Count();
                            if (totalBarraXEmpr > 0)
                            {
                                foreach (var barra in listaBarraXEmpr)
                                {
                                    if (barra.Barrcodi == 642)
                                    {
                                    }

                                    MeMedicion48DTO objcmg = dataCMEjec.Find(x => x.Medifecha == day && x.Barrcodi == barra.Barrcodi && x.Emprcodi == barra.Emprcodi);
                                    MeMedicion48DTO objcv = dataCV.Find(x => x.Medifecha == day && x.Barrcodi == barra.Barrcodi && x.Emprcodi == barra.Emprcodi);
                                    MeMedicion48DTO objile = dataILE.Find(x => x.Medifecha == day && x.Barrcodi == barra.Barrcodi && x.Emprcodi == barra.Emprcodi);

                                    decimal? valorCMg = null;
                                    decimal? valorCV = null;
                                    decimal? valorIle = null;
                                    bool tieneGeneracion = false;

                                    if (objcmg != null)
                                    {
                                        valorCMg = (decimal?)objcmg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(objcmg, null);
                                    }
                                    colData = colData + 1;
                                    ws.Cells[row, colData].Value = valorCMg;

                                    if (objcv != null)
                                    {
                                        valorCV = (decimal?)objcv.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(objcv, null);
                                    }
                                    colData = colData + 1;
                                    ws.Cells[row, colData].Value = valorCV;

                                    colData = colData + 1;
                                    if (objile != null)
                                    {
                                        valorIle = (decimal?)objile.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(objile, null);
                                        tieneGeneracion = objile.TieneIndicador;
                                        if (tieneGeneracion)
                                        {
                                            if (valorIle != null)
                                            {
                                                ws.Cells[row, colData].Value = valorIle;
                                            }
                                            else { ws.Cells[row, colData].Value = "(*)"; }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                colData = colData + 1;
                                colData = colData + 1;
                                colData = colData + 1;
                            }
                        }

                        row = row + 1;


                        horas = horas.AddMinutes(30);
                    }
                }

                using (var range = ws.Cells[rowIniData, colIniFecha, (rowIniData + numDia * 48) - 1, colData])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.White);
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Style = ExcelBorderStyle.Dotted;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                }
                using (var range = ws.Cells[rowIniData, colIniFecha, (rowIniData + numDia * 48) - 1, colIniFecha])
                {
                    range.Style.Font.Size = 28;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                //Formato Data
                using (var range = ws.Cells[rowIniData, colIniFecha + 1, rowIniData + numDia * 48 - 1, colData])
                {
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.Style.Font.Size = 28;
                    range.Style.Numberformat.Format = "#,##0.00";
                }

                for (int f = rowIniData + 48; f <= rowIniData + numDia * 48; f += 48)
                {
                    ws.Cells[f, colIniFecha, f, colData].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                }

                for (int columna = colIniFecha + 1; columna <= colData; columna++)
                    ws.Column(columna).Width = 40;

                //Formato de Filas y columnas
                ws.Column(colIniFecha).Width = 60;
                ws.Row(rowIniEmp).Height = 120;
                ws.Row(rowIniBarra).Height = 100;
                ws.Row(rowIniMw).Height = 70;

                #endregion

                #region leyenda

                int rowIniLeyenda = row + 3;
                int rowIniIndeterminado = row + 5;

                ws.Cells[rowIniLeyenda, colIniFecha].Value = "Leyenda:";
                ws.Cells[rowIniLeyenda, colIniFecha].Style.Font.Size = 28;
                ws.Cells[rowIniLeyenda, colIniFecha].Style.Font.Bold = true;

                ws.Cells[rowIniIndeterminado, colIniFecha].Value = "(*)";
                ws.Cells[rowIniIndeterminado, colIniFecha].Style.Font.Size = 28;
                ws.Cells[rowIniIndeterminado, colIniFecha + 1].Value = " Indeterminado   (''/0'') ";
                ws.Cells[rowIniIndeterminado, colIniFecha + 1].Style.Font.Size = 28;

                #endregion

                ws.View.FreezePanes(rowFinFecha + 1, colFinFreeze + 1);
            }
            ws.View.ZoomScale = 30;
        }

        /// <summary>
        /// Generación de Reporte Excel 6
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="ws"></param>
        /// <param name="nombreSheet"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="vermmcodi"></param>
        public void GenerarExcelReporte6(string empresas, ExcelWorksheet ws, DateTime fechaIni, DateTime fechaFin, int rowIni, int colIni, int colFinFreeze, int vermmcodi, List<MmmDatoDTO> listFacTable)
        {
            List<SiEmpresaDTO> listaEmpresa;
            List<MeMedicion48DTO> data = this.ListarIndicador(vermmcodi, fechaIni, fechaFin, ConstantesMonitoreo.CodigoIMU, listFacTable, ConstantesMonitoreo.ReportesIndicadores, out listaEmpresa);
            listaEmpresa = this.ListarEmpresasMonitoreoByData(listaEmpresa, empresas);

            List<MeMedicion48DTO> listaBarra = data.Where(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCMEjec)
                .Select(x => new { Emprcodi = x.Emprcodi, Barrcodi = x.Barrcodi, Barrnombre = x.Barrnombre })
                .GroupBy(x => new { x.Emprcodi, x.Barrcodi, x.Barrnombre })
                .Select(x => new MeMedicion48DTO() { Emprcodi = x.Key.Emprcodi, Barrcodi = x.Key.Barrcodi, Barrnombre = x.Key.Barrnombre })
                .OrderBy(x => x.Barrnombre).ToList();

            int row = rowIni + 5;
            int col = colIni;
            //
            if (listaEmpresa.Count > 0)
            {
                #region cabecera

                int rowIniNombreReporte = row;
                /// Fila Hora - Empresa - Total

                int colIniFecha = col;
                int rowIniFecha = row;
                int rowFinFecha = rowIniFecha + 3 - 1;
                ws.Cells[rowIniFecha, colIniFecha].Value = "MM/DD/hh:mm";
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Font.Size = 28;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Merge = true;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Font.Bold = true;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.WrapText = true;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                int rowIniEmp = row;
                int colIniEmp = colIniFecha + 1;
                int colFinEmp = colIniEmp - 1;

                int colIniNombreReporte = colIniEmp;

                Color colorEmpresaImpar = ColorTranslator.FromHtml(ConstantesMonitoreo.ColorCmgEmpresaImpar);
                Color colorEmpresaPar = ColorTranslator.FromHtml(ConstantesMonitoreo.ColorCmgEmpresaPar);

                //Empresa
                int contEmpresa = 1;
                foreach (var empresa in listaEmpresa)
                {
                    int totalBarraXEmpr = listaBarra.Where(x => x.Emprcodi == empresa.Emprcodi).Count();
                    totalBarraXEmpr = totalBarraXEmpr > 0 ? totalBarraXEmpr : 1;

                    colFinEmp = colFinEmp + (3 * totalBarraXEmpr);

                    ws.Cells[rowIniEmp, colIniEmp].Value = empresa.Emprnomb;
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.Font.Size = 28;
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Merge = true;
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.WrapText = true;
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.Fill.BackgroundColor.SetColor(contEmpresa % 2 == 0 ? colorEmpresaPar : colorEmpresaImpar);

                    colIniEmp = colFinEmp + 1;
                    contEmpresa++;
                }

                int rowIniBarra = rowIniEmp + 1;
                int colIniBarra = colIniFecha + 1;
                int colFinBarra = colIniBarra - 1;

                //Barra X Empresa
                contEmpresa = 1;
                foreach (var empresa in listaEmpresa)
                {
                    var listaBarraXEmpr = listaBarra.Where(x => x.Emprcodi == empresa.Emprcodi).ToList();
                    int totalBarraXEmpr = listaBarraXEmpr.Count();
                    if (totalBarraXEmpr > 0)
                    {
                        foreach (var barra in listaBarraXEmpr)
                        {
                            colFinBarra = colFinBarra + 3;
                            ws.Cells[rowIniBarra, colIniBarra].Value = barra.Barrnombre;
                            ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.Font.Size = 28;
                            ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Merge = true;
                            ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.WrapText = true;
                            ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.Fill.BackgroundColor.SetColor(contEmpresa % 2 == 0 ? colorEmpresaPar : colorEmpresaImpar);
                            colIniBarra = colFinBarra + 1;
                        }
                    }
                    else
                    {
                        colFinBarra = colFinBarra + 3;
                        ws.Cells[rowIniBarra, colIniBarra].Value = "";
                        ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.Font.Size = 28;
                        ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Merge = true;
                        ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.WrapText = true;
                        ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.Fill.BackgroundColor.SetColor(contEmpresa % 2 == 0 ? colorEmpresaPar : colorEmpresaImpar);
                        colIniBarra = colFinBarra + 1;
                    }
                    contEmpresa++;
                }

                int rowIniMw = rowIniBarra + 1;
                int colIniMw = colIniFecha + 1;
                int colFinMw = colIniMw;

                //Valores x Barra
                contEmpresa = 1;
                foreach (var empresa in listaEmpresa)
                {
                    int totalBarraXEmpr = listaBarra.Where(x => x.Emprcodi == empresa.Emprcodi).Count();
                    totalBarraXEmpr = totalBarraXEmpr > 0 ? totalBarraXEmpr : 1;
                    for (int j = 0; j < totalBarraXEmpr; j++)
                    {
                        for (int i = 1; i <= 3; i++)
                        {
                            string valor; valor = (i == 1) ? " CMg  (k,m,t)" : (i == 2) ? " CV  (S/K wh)" : "IMU (Índice de Mark Up )";
                            ws.Cells[rowIniMw, colIniMw].Value = valor;
                            colFinMw = colIniMw;
                            ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.Font.Size = 28;
                            ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Merge = true;
                            ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.WrapText = true;
                            ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.Fill.BackgroundColor.SetColor(contEmpresa % 2 == 0 ? colorEmpresaPar : colorEmpresaImpar);
                            colIniMw = colFinMw + 1;
                        }
                    }
                    contEmpresa++;
                }
                int colFinNombreReporte = colFinEmp;


                var colorBorder = Color.Black;
                using (var range = ws.Cells[rowIniNombreReporte, colIniNombreReporte, rowIniMw, colFinNombreReporte])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Font.Bold = true;
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin, colorBorder);
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Color.SetColor(colorBorder);
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(colorBorder);
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Color.SetColor(colorBorder);
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Color.SetColor(colorBorder);
                    range.Style.Font.Color.SetColor(Color.Black);
                }

                #endregion

                int rowIniData = rowIniMw + 1;
                row = rowIniData;

                #region cuerpo

                int numDia = 0;

                int colData = colIniFecha + 1;

                for (var day = fechaIni.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
                {
                    List<MeMedicion48DTO> dataCMEjec = data.Where(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCMEjec).OrderBy(x => x.Emprnomb).ThenBy(x => x.Barrnombre).ToList();
                    List<MeMedicion48DTO> dataCV = data.Where(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCV).ToList();
                    List<MeMedicion48DTO> dataIMU = data.Where(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaIMU).ToList();

                    numDia++;
                    DateTime horas = day.AddMinutes(30);

                    for (int h = 1; h <= 48; h++)
                    {
                        colData = colIniFecha;
                        ws.Cells[row, colData].Value = horas.ToString(ConstantesAppServicio.FormatoFechaHora);
                        ws.Cells[row, colIniFecha].Style.Font.Size = 28;
                        ws.Cells[row, colIniFecha].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        foreach (var empresa in listaEmpresa)
                        {
                            var listaBarraXEmpr = listaBarra.Where(x => x.Emprcodi == empresa.Emprcodi).ToList();
                            int totalBarraXEmpr = listaBarraXEmpr.Count();
                            if (totalBarraXEmpr > 0)
                            {
                                foreach (var barra in listaBarraXEmpr)
                                {
                                    if (barra.Barrcodi == 642)
                                    {
                                    }

                                    MeMedicion48DTO objcmg = dataCMEjec.Find(x => x.Medifecha == day && x.Barrcodi == barra.Barrcodi && x.Emprcodi == barra.Emprcodi);
                                    MeMedicion48DTO objcv = dataCV.Find(x => x.Medifecha == day && x.Barrcodi == barra.Barrcodi && x.Emprcodi == barra.Emprcodi);
                                    MeMedicion48DTO objimu = dataIMU.Find(x => x.Medifecha == day && x.Barrcodi == barra.Barrcodi && x.Emprcodi == barra.Emprcodi);

                                    decimal? valorCMg = null;
                                    decimal? valorCV = null;
                                    decimal? valorImu = null;
                                    bool tieneGeneracion = false;

                                    if (objcmg != null)
                                    {
                                        valorCMg = (decimal?)objcmg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(objcmg, null);
                                    }
                                    colData = colData + 1;
                                    ws.Cells[row, colData].Value = valorCMg;

                                    if (objcv != null)
                                    {
                                        valorCV = (decimal?)objcv.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(objcv, null);
                                    }
                                    colData = colData + 1;
                                    ws.Cells[row, colData].Value = valorCV;

                                    colData = colData + 1;
                                    if (objimu != null)
                                    {
                                        valorImu = (decimal?)objimu.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(objimu, null);
                                        tieneGeneracion = objimu.TieneIndicador;
                                        if (tieneGeneracion)
                                        {
                                            if (valorImu != null)
                                            {
                                                ws.Cells[row, colData].Value = valorImu;
                                            }
                                            else { ws.Cells[row, colData].Value = "(*)"; }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                colData = colData + 1;
                                colData = colData + 1;
                                colData = colData + 1;
                            }
                        }

                        row = row + 1;

                        horas = horas.AddMinutes(30);
                    }
                }

                using (var range = ws.Cells[rowIniData, colIniFecha, (rowIniData + numDia * 48) - 1, colData])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.White);
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Style = ExcelBorderStyle.Dotted;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                }
                using (var range = ws.Cells[rowIniData, colIniFecha, (rowIniData + numDia * 48) - 1, colIniFecha])
                {
                    range.Style.Font.Size = 28;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                //Formato Data
                using (var range = ws.Cells[rowIniData, colIniFecha + 1, rowIniData + numDia * 48 - 1, colData])
                {
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.Style.Font.Size = 28;
                    range.Style.Numberformat.Format = "#,##0.00";
                }

                for (int f = rowIniData + 48; f <= rowIniData + numDia * 48; f += 48)
                {
                    ws.Cells[f, colIniFecha, f, colData].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                }

                for (int columna = colIniFecha + 1; columna <= colData; columna++)
                    ws.Column(columna).Width = 40;

                //Formato de Filas y columnas
                ws.Column(colIniFecha).Width = 60;
                ws.Row(rowIniEmp).Height = 120;
                ws.Row(rowIniBarra).Height = 100;
                ws.Row(rowIniMw).Height = 70;

                #endregion

                #region leyenda

                int rowIniLeyenda = row + 3;
                int rowIniIndeterminado = row + 5;

                ws.Cells[rowIniLeyenda, colIniFecha].Value = "Leyenda:";
                ws.Cells[rowIniLeyenda, colIniFecha].Style.Font.Size = 28;
                ws.Cells[rowIniLeyenda, colIniFecha].Style.Font.Bold = true;

                ws.Cells[rowIniIndeterminado, colIniFecha].Value = "(*)";
                ws.Cells[rowIniIndeterminado, colIniFecha].Style.Font.Size = 28;
                ws.Cells[rowIniIndeterminado, colIniFecha + 1].Value = " Indeterminado   (''/0'') ";
                ws.Cells[rowIniIndeterminado, colIniFecha + 1].Style.Font.Size = 28;

                #endregion

                ws.View.FreezePanes(rowFinFecha + 1, colFinFreeze + 1);
            }
            ws.View.ZoomScale = 30;
        }

        /// <summary>
        /// Generación de Reporte 1
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="ws"></param>
        /// <param name="nombreSheet"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="vermmcodi"></param>
        public void GenerarExcelReporte7(string empresas, ExcelWorksheet ws, DateTime fechaIni, DateTime fechaFin, int rowIni, int colIni, int colFinFreeze, int vermmcodi, List<MmmDatoDTO> listFacTable)
        {
            DateTime fechaPeriodo = new DateTime(fechaIni.Year, fechaIni.Month, 1).Date;
            DateTime fechaFinPeriodo = fechaPeriodo.AddMonths(1).AddDays(-1);

            List<SiEmpresaDTO> listaEmpresa;
            List<MeMedicion48DTO> dataTotal = this.ListarIndicador(vermmcodi, fechaIni, fechaFin, ConstantesMonitoreo.CodigoIRT, listFacTable, ConstantesMonitoreo.ReportesIndicadores, out listaEmpresa);
            listaEmpresa = this.ListarEmpresasMonitoreo(fechaIni, empresas);

            List<int> listaEmprcodi = listaEmpresa.Select(x => x.Emprcodi).ToList();

            //Congestiones
            List<MeMedicion48DTO> listaCongXAreaXFecha = dataTotal.Where(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCongestion
                && listaEmprcodi.Contains(x.Emprcodi)).ToList();
            listaCongXAreaXFecha = this.ListarCongXAreaYFechaReporte7(listaCongXAreaXFecha);

            //Grupos despacho
            List<MeMedicion48DTO> listaBarraEmpGrupo = dataTotal.Where(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaEnlaceTrans
                && listaEmprcodi.Contains(x.Emprcodi)).ToList();

            if (listaBarraEmpGrupo.Count == 0)
            {
                listaBarraEmpGrupo.Add(new MeMedicion48DTO() { Grupocodi = -1, Gruponomb = string.Empty, Grupopadre = -2, Central = string.Empty, Barrcodi = -1, Barrnombre = string.Empty });
            }

            int row = rowIni + 2;
            int col = colIni;

            #region Congestiones
            /// Fila Hora - Empresa - Total

            int colCongestionIni = col;
            int rowCongestion = row;

            //            CONGESTIÓN	
            //Desde:  01/08/2018 Hasta: 31/08/2018	

            ws.Cells[rowCongestion, colCongestionIni].Value = "CONGESTIÓN";
            ws.Cells[rowCongestion, colCongestionIni].Style.WrapText = true;
            ws.Cells[rowCongestion, colCongestionIni].Style.Font.Size = 36;
            ws.Cells[rowCongestion, colCongestionIni].Style.Font.Bold = true;

            ws.Cells[rowCongestion + 1, colCongestionIni].Value = "Desde: " + fechaPeriodo.ToString(ConstantesBase.FormatoFechaPE) + " Hasta: " + fechaFinPeriodo.ToString(ConstantesBase.FormatoFechaPE);
            ws.Cells[rowCongestion + 1, colCongestionIni].Style.Font.Size = 36;
            ws.Cells[rowCongestion + 1, colCongestionIni].Style.Font.Bold = true;

            #region cabecera
            string nombreCampo = string.Empty;

            int rowIniTblConges = rowCongestion + 3;
            for (int i = 1; i <= 7; i++)
            {
                if (i == 1)
                {
                    nombreCampo = "FECHA";
                }

                if (i == 2)
                {
                    nombreCampo = "INICIO";
                }

                if (i == 3)
                {
                    nombreCampo = "FINAL";
                }

                if (i == 4)
                {
                    nombreCampo = "UBICACIÓN";
                }

                if (i == 5)
                {
                    nombreCampo = "EQUIPO";
                }

                if (i == 6)
                {
                    nombreCampo = "OBSERVACIONES";
                }

                if (i == 7)
                {
                    nombreCampo = "GENERACIÓN CON UNA LOCALIZACIÓN ESPECÍFICA, QUE OBTENGAN PODER DE MERCADO";
                }

                ws.Cells[rowIniTblConges, colCongestionIni + i - 1].Value = nombreCampo;
                ws.Cells[rowIniTblConges, colCongestionIni + i - 1].Merge = true;
                ws.Cells[rowIniTblConges, colCongestionIni + i - 1].Style.WrapText = true;
                ws.Cells[rowIniTblConges, colCongestionIni + i - 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniTblConges, colCongestionIni + i - 1].Style.Font.Size = 36;
            }

            var colorBorder = Color.Black;
            var classTipoConges = "#2980B9";
            using (var range = ws.Cells[rowIniTblConges, colCongestionIni, rowIniTblConges, colCongestionIni + 7 - 1])
            {
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Font.Bold = true;
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin, colorBorder);
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Top.Color.SetColor(colorBorder);
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Color.SetColor(colorBorder);
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Color.SetColor(colorBorder);
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Color.SetColor(colorBorder);
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(classTipoConges));
                range.Style.Font.Color.SetColor(Color.White);
            }

            #endregion

            #region Cuerpo
            int colDataIni = colCongestionIni;
            int rowDataIni = rowIniTblConges + 1;
            int colDataFin = colDataIni + 7 - 1;
            string dato = string.Empty;
            foreach (var conges in listaCongXAreaXFecha)
            {
                for (int i = 1; i <= 7; i++)
                {
                    if (i == 1)
                    {
                        dato = conges.Hophorini.ToString(ConstantesAppServicio.FormatoFecha);
                    }

                    if (i == 2)
                    {
                        dato = conges.Hophorini.ToString(ConstantesAppServicio.FormatoOnlyHora);
                    }

                    if (i == 3)
                    {
                        dato = conges.Hophorfin.ToString(ConstantesAppServicio.FormatoOnlyHora);
                    }

                    if (i == 4)
                    {
                        dato = conges.Areanomb;
                    }

                    if (i == 5)
                    {
                        dato = conges.Equinomb;
                    }

                    if (i == 6)
                    {
                        dato = conges.Descripcion;
                    }
                    if (i == 7)
                    {
                        dato = conges.Gruponomb;
                    }

                    ws.Cells[rowDataIni, colCongestionIni + i - 1].Value = dato;
                    ws.Cells[rowDataIni, colCongestionIni + i - 1].Style.WrapText = true;
                    ws.Cells[rowDataIni, colCongestionIni + i - 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowDataIni, colCongestionIni + i - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[rowDataIni, colCongestionIni + i - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[rowDataIni, colCongestionIni + i - 1].Style.Font.Size = 36;
                    ws.Cells[rowDataIni, colCongestionIni + i - 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[rowDataIni, colCongestionIni + i - 1].Style.Font.Color.SetColor(ColorTranslator.FromHtml(classTipoConges));
                    ws.Cells[rowDataIni, colCongestionIni + i - 1].Style.Fill.BackgroundColor.SetColor(Color.White);
                }
                ws.Row(rowIniTblConges).Height = 90;
                rowDataIni = rowDataIni + 1;
            }

            for (int columna = colDataIni + 1; columna < colDataFin; columna++)
                ws.Column(columna).Width = 84;
            ws.Column(colDataFin).Width = 200;

            ws.Row(rowIniTblConges).Height = 100;

            #endregion

            #endregion

            row = rowDataIni + 5;
            //
            if (listaEmpresa.Count > 0)
            {
                #region cabecera

                /// Fila Hora - Empresa - Total

                int colIniFecha = col;
                int rowIniFecha = row;
                int rowFinFecha = rowIniFecha + 4 - 1;
                ws.Cells[rowIniFecha, colIniFecha].Value = "MM/DD/hh:mm";
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Merge = true;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.WrapText = true;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Font.Bold = true;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Font.Size = 28;

                int rowIniEnl = row;
                int colIniEnl = colIniFecha + 1;
                int colFinEnl = colIniEnl;
                int colIniIrt = colIniFecha;
                int rowFinIrt = rowIniEnl + 3;
                int rowIniNombres = rowIniEnl + 2;
                int rowIniLinea = rowIniEnl + 1;

                int colIniReporte = colIniEnl;

                Color colorEnlace = ColorTranslator.FromHtml("#9BC2E6");

                //Enlace de transmision
                foreach (var grupo in listaBarraEmpGrupo)
                {
                    colFinEnl = colIniEnl + 3;

                    //
                    ws.Cells[rowIniEnl, colIniEnl].Value = "Enlace de transmisión";
                    ws.Cells[rowIniEnl, colIniEnl, rowIniEnl, colFinEnl].Style.Font.Size = 24;
                    ws.Cells[rowIniEnl, colIniEnl, rowIniEnl, colFinEnl].Merge = true;
                    ws.Cells[rowIniEnl, colIniEnl, rowIniEnl, colFinEnl].Style.WrapText = true;
                    ws.Cells[rowIniEnl, colIniEnl, rowIniEnl, colFinEnl].Style.Font.Bold = true;
                    ws.Cells[rowIniEnl, colIniEnl, rowIniEnl, colFinEnl].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniEnl, colIniEnl, rowIniEnl, colFinEnl].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[rowIniEnl, colIniEnl, rowIniEnl, colFinEnl].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[rowIniEnl, colIniEnl, rowIniEnl, colFinEnl].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[rowIniEnl, colIniEnl, rowIniEnl, colFinEnl].Style.Font.Color.SetColor(Color.Black);
                    ws.Cells[rowIniEnl, colIniEnl, rowIniEnl, colFinEnl].Style.Fill.BackgroundColor.SetColor(colorEnlace);

                    //
                    ws.Cells[rowIniLinea, colIniEnl].Value = grupo.Equinomb;
                    ws.Cells[rowIniLinea, colIniEnl, rowIniLinea, colFinEnl].Style.Font.Size = 24;
                    ws.Cells[rowIniLinea, colIniEnl, rowIniLinea, colFinEnl].Merge = true;
                    ws.Cells[rowIniLinea, colIniEnl, rowIniLinea, colFinEnl].Style.WrapText = true;
                    ws.Cells[rowIniLinea, colIniEnl, rowIniLinea, colFinEnl].Style.Font.Bold = true;
                    ws.Cells[rowIniLinea, colIniEnl, rowIniLinea, colFinEnl].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniLinea, colIniEnl, rowIniLinea, colFinEnl].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[rowIniLinea, colIniEnl, rowIniLinea, colFinEnl].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    //       
                    ws.Cells[rowIniNombres, colIniEnl].Value = grupo.Gruponomb;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.Font.Size = 24;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Merge = true;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.WrapText = true;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.Font.Bold = true;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    ws.Cells[rowFinIrt, colIniEnl].Value = "PU (MW)";
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Font.Size = 24;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Merge = true;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.WrapText = true;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Font.Bold = true;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Font.Color.SetColor(Color.Black);
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Fill.BackgroundColor.SetColor(colorEnlace);

                    //
                    colIniEnl++;
                    ws.Cells[rowIniNombres, colIniEnl].Value = grupo.Gruponomb;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.Font.Size = 24;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Merge = true;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.WrapText = true;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.Font.Bold = true;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    ws.Cells[rowFinIrt, colIniEnl].Value = "PP (MW)";
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Font.Size = 24;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Merge = true;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.WrapText = true;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Font.Bold = true;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Font.Color.SetColor(Color.Black);
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Fill.BackgroundColor.SetColor(colorEnlace);

                    //
                    colIniEnl++;
                    ws.Cells[rowIniNombres, colIniEnl].Value = grupo.Barrnombre;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.Font.Size = 24;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Merge = true;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.WrapText = true;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.Font.Bold = true;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    ws.Cells[rowFinIrt, colIniEnl].Value = "CMg (S/ MWh)";
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Font.Size = 24;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Merge = true;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.WrapText = true;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Font.Bold = true;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Font.Color.SetColor(Color.Black);
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Fill.BackgroundColor.SetColor(colorEnlace);

                    //
                    colIniEnl++;
                    ws.Cells[rowIniNombres, colIniEnl].Value = grupo.Barrnombre;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.Font.Size = 24;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Merge = true;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.WrapText = true;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.Font.Bold = true;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    ws.Cells[rowFinIrt, colIniEnl].Value = "CMgprog (S/ MWh)";
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Font.Size = 24;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Merge = true;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.WrapText = true;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Font.Bold = true;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Font.Color.SetColor(Color.Black);
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Fill.BackgroundColor.SetColor(colorEnlace);

                    //
                    colIniIrt = colFinEnl + 1;
                    ws.Cells[rowIniEnl, colIniIrt].Value = "IRT";
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Style.Font.Size = 24;
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Merge = true;
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Style.WrapText = true;
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Style.Font.Bold = true;
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Style.Font.Color.SetColor(Color.Black);
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Style.Fill.BackgroundColor.SetColor(colorEnlace);

                    colIniEnl = colIniIrt + 1;
                }

                #endregion

                int rowIniData = rowFinIrt + 1;
                row = rowIniData;

                #region cuerpo

                int numDia = 0;

                int colData = colIniFecha;
                for (var day = fechaIni.Date; day.Date <= fechaFin; day = day.AddDays(1))
                {
                    numDia++;
                    DateTime horas = day.AddMinutes(30);

                    List<MeMedicion48DTO> dataXDia = dataTotal.Where(x => x.Medifecha == day).ToList();

                    for (int h = 1; h <= 48; h++)
                    {
                        //Fecha
                        ws.Cells[row, colIniFecha].Value = horas.ToString(ConstantesAppServicio.FormatoFechaHora);
                        ws.Cells[row, colIniFecha].Style.Font.Size = 28;
                        ws.Cells[row, colIniFecha].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        colData = colIniFecha;

                        foreach (var grupo in listaBarraEmpGrupo)
                        {
                            MeMedicion48DTO regIRT = dataXDia.Find(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaIRT && x.Grupocodi == grupo.Grupocodi);
                            if (regIRT != null)
                            {
                                decimal? irt = (decimal?)regIRT.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regIRT, null);
                                if (irt != null)
                                {
                                    //Potencia ejecutada
                                    MeMedicion48DTO regpotEjec = this.GetPotenciaTotalXDiaByGrupoReporte7(day, dataXDia.Where(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaPotGrupoEjec).ToList(), grupo.Grupocodi);
                                    decimal? potEjec = null;
                                    if (regpotEjec != null)
                                    {
                                        potEjec = (decimal?)regpotEjec.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regpotEjec, null);
                                    }
                                    colData++;
                                    if (potEjec.GetValueOrDefault(0) != 0)
                                        ws.Cells[row, colData].Value = potEjec;

                                    //Potencia programada
                                    MeMedicion48DTO regpotProg = this.GetPotenciaTotalXDiaByGrupoReporte7(day, dataXDia.Where(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaPotGrupoProg).ToList(), grupo.Grupocodi);

                                    decimal? potProg = null;
                                    if (regpotProg != null)
                                    {
                                        potProg = (decimal?)regpotProg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regpotProg, null);
                                    }
                                    colData++;
                                    if (potProg.GetValueOrDefault(0) != 0)
                                        ws.Cells[row, colData].Value = potProg;

                                    //Costos marg Ejecutados
                                    MeMedicion48DTO regCMgejec = dataXDia.Find(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCMEjec && x.Barrcodi == grupo.Barrcodi && x.Emprcodi == grupo.Emprcodi);
                                    decimal? cmg = null;
                                    if (regCMgejec != null && potEjec.GetValueOrDefault(0) != 0)
                                    {
                                        cmg = (decimal?)regCMgejec.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regCMgejec, null);
                                        if (cmg != null)
                                            cmg = cmg * 1000;
                                    }
                                    colData++;
                                    if (cmg.GetValueOrDefault(0) != 0)
                                        ws.Cells[row, colData].Value = cmg;

                                    //Costos marg Programados
                                    MeMedicion48DTO regCMgprog = dataXDia.Find(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCMProg && x.Cnfbarcodi == grupo.Cnfbarcodi && x.Emprcodi == grupo.Emprcodi);
                                    decimal? cmgProg = null;
                                    if (regCMgprog != null && potProg.GetValueOrDefault(0) != 0)
                                    {
                                        cmgProg = (decimal?)regCMgprog.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regCMgprog, null);
                                    }
                                    colData++;
                                    if (cmgProg.GetValueOrDefault(0) != 0)
                                        ws.Cells[row, colData].Value = cmgProg;

                                    colData++;
                                    if (irt.GetValueOrDefault(0) != 0)
                                        ws.Cells[row, colData].Value = irt;
                                }
                                else
                                {
                                    colData++;
                                    colData++;
                                    colData++;
                                    colData++;
                                    colData++;
                                }
                            }
                            else
                            {
                                colData++;
                                colData++;
                                colData++;
                                colData++;
                                colData++;
                            }
                        }
                        horas = horas.AddMinutes(30);
                        row++;
                    }
                }

                using (var range = ws.Cells[rowIniData, colIniFecha, (rowIniData + numDia * 48) - 1, colData])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.White);
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Style = ExcelBorderStyle.Dotted;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                }

                //Formato Data
                using (var range = ws.Cells[rowIniData, colIniFecha + 1, (rowIniData + numDia * 48) - 1, colData])
                {
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.Style.Font.Size = 28;
                    range.Style.Numberformat.Format = "#,##0.000";
                }

                for (int f = rowIniData + 48; f <= rowIniData + numDia * 48; f += 48)
                {
                    ws.Cells[f, colIniFecha, f, colData].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                }

                //Formato de Filas y columnas
                for (int columna = colIniReporte; columna <= colData; columna++)
                    ws.Column(columna).Width = 80;

                ws.Column(colIniFecha).Width = 50;
                ws.Row(rowIniFecha).Height = 70;
                ws.Row(rowIniLinea).Height = 70;
                ws.Row(rowIniNombres).Height = 70;
                ws.Row(rowFinIrt).Height = 70;

                #endregion

                ws.View.FreezePanes(rowFinFecha + 1, colFinFreeze + 1);
            }
            ws.View.ZoomScale = 30;
        }

        #endregion

        #region Dashboard

        /// <summary>
        /// Obtener fecha maxima de consulta para los reporte DashBoard
        /// </summary>
        /// <param name="strFechaIniProceso"></param>
        /// <param name="numDiaPicker"></param>
        public void GetFechaMaxGeneracionPermitidaDasBoard(out string strFechaIniProceso, out int numDiaPicker)
        {
            int dia = DateTime.Now.Day;
            //resta de mes a iniciar
            int mes;
            // Configura la cantidad de meses a mostrar dependiendo si es antes del 6 del mes acual o no 

            if (dia < 6)
            {
                numDiaPicker = ((dia - 1) * -1) - 30;
                mes = 2;
            }
            else
            {
                numDiaPicker = ((dia - 1) * -1);
                mes = 1;
            }
            strFechaIniProceso = new DateTime(DateTime.Now.Year, (DateTime.Now.Month), 1).AddMonths(-mes).ToString(ConstantesAppServicio.FormatoMes);
        }

        /// <summary>
        /// Lista de indicadores Dashborard
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ListarIndicadorDashBoard(DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion48DTO> listaFinal = new List<MeMedicion48DTO>();

            List<MeMedicion48DTO> listaPotXDia = this.servEjec.ListaDataGeneracion48(fechaInicio, fechaFin, ConstantesMedicion.IdTipogrupoCOES
                , ConstantesMedicion.IdTipoGeneracionTodos.ToString(), ConstantesMedicion.IdEmpresaTodos.ToString(), ConstanteValidacion.EstadoTodos
                , ConstantesMedicion.IdTipoRecursoTodos.ToString(), false, ConstantesTipoInformacion.TipoinfoMW, ConstantesPR5ReportesServicio.LectDespachoEjecutadoHisto);

            listaPotXDia = listaPotXDia.Where(x => x.Tipogrupocodi != ConstantesMedicion.IdTipogrupoNoIntegrante).ToList();


            List<MeMedicion48DTO> listaPotXDiaProgramado = this.servEjec.ListaDataGeneracion48(fechaInicio, fechaFin, ConstantesMedicion.IdTipogrupoCOES
               , ConstantesMedicion.IdTipoGeneracionTodos.ToString(), ConstantesMedicion.IdEmpresaTodos.ToString(), ConstanteValidacion.EstadoTodos
               , ConstantesMedicion.IdTipoRecursoTodos.ToString(), false, ConstantesTipoInformacion.TipoinfoMW, ConstantesPR5ReportesServicio.LectDespachoProgramadoDiario);






            //Obtener la data x dia y por empresa
            List<MeMedicion48DTO> listaPotXDiaXEmpresa = (from t in listaPotXDia
                                                          group t by new { t.Medifecha, t.Emprcodi, t.Emprnomb }

                                                              into destino
                                                          select new MeMedicion48DTO()
                                                          {
                                                              TipoFormulaMonitoreo = ConstantesMonitoreo.TipoFormulaMWEjec,
                                                              // Barrcodi=destino.Key.Barrcodi,
                                                              Medifecha = destino.Key.Medifecha,
                                                              Emprcodi = destino.Key.Emprcodi,
                                                              Emprnomb = destino.Key.Emprnomb,
                                                              //  Grupocodi=destino.Key.Grupocodi,
                                                              //Catecodi=destino.Key.Catecodi,
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
                                                              H48 = destino.Sum(t => t.H48)
                                                          }).ToList();

            listaFinal.AddRange(listaPotXDiaXEmpresa);
            return listaFinal;

        }

        /// <summary>
        /// Grafico periodo diario
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public GraficoWeb GraficoMedidorMonitoreoCuotaHHI(string empresas, DateTime fechaInicio, DateTime fechaFin, List<MeMedicion48DTO> data, int indicador)
        {
            decimal?[][] seriesData = new decimal?[1][];

            if (indicador == 1)
            {
                seriesData[0] = new decimal?[5];
                seriesData[0][0] = ConstantesMonitoreo.PosicionInicial;
                seriesData[0][1] = ConstantesMonitoreo.PosicionUnoCuotaMercado;
                seriesData[0][2] = ConstantesMonitoreo.PosicionDosCuotaMercado;
                seriesData[0][3] = ConstantesMonitoreo.PosicionTresCuotaMercado;
                seriesData[0][4] = ConstantesMonitoreo.PosicionCuatroCuotaMercado;
            }
            else
            {
                seriesData[0] = new decimal?[5];
                seriesData[0][0] = ConstantesMonitoreo.PosicionInicial;
                seriesData[0][1] = ConstantesMonitoreo.PosicionUnoHhi;
                seriesData[0][2] = ConstantesMonitoreo.PosicionDosHhi;
                seriesData[0][3] = ConstantesMonitoreo.PosicionTresHhi;
                seriesData[0][4] = ConstantesMonitoreo.PosicionCuatroHhi;
            }
            List<SiEmpresaDTO> listaEmpresa = this.ListarEmpresasMonitoreo(fechaInicio, empresas);
            // var objEmpresa = listaEmpresa.Select(x => new { x.Emprnomb }).ToList();

            string nombreEmpresa = string.Empty;

            //      nombreEmpresa = objEmpresa.Count == 1 ? listaEmpresa.GetType(0): string.Empty;

            if (listaEmpresa.Count == 1)
            {
                foreach (var empresa in listaEmpresa)
                {
                    nombreEmpresa = empresa.Emprnomb;
                }
            }

            string tituloDash = indicador == 1 ? ConstantesMonitoreo.TituloCuotaMercado + " " + nombreEmpresa : string.Empty;
            if (indicador == 1)
            {
                var graficoWeb = new GraficoWeb
                {
                    SeriesData = seriesData,
                    //Titulo cuoata de mercado
                    TitleText = tituloDash,
                    //Rango de porcentaje de Cuota de mercado
                    Series = new List<RegistroSerie>()
                {
                 new RegistroSerie{ To = ConstantesMonitoreo.PosicionInicial,From= ConstantesMonitoreo.PosicionUnoCuotaMercado,Color= "#B5C6C6",Porcentaje= 60 },
                 new RegistroSerie{ To = ConstantesMonitoreo.PosicionUnoCuotaMercado,From= ConstantesMonitoreo.PosicionDosCuotaMercado,Color= "#28E60A",Porcentaje= 60},
                 new RegistroSerie{ To = ConstantesMonitoreo.PosicionDosCuotaMercado,From= ConstantesMonitoreo.PosicionTresCuotaMercado,Color= "#F1F907",Porcentaje= 60 },
                 new RegistroSerie{ To = ConstantesMonitoreo.PosicionTresCuotaMercado,From= ConstantesMonitoreo.PosicionCuatroCuotaMercado,Color= "#FF0000",Porcentaje= 60 },
                }
                };
                GenerarDashBoard(empresas, fechaInicio, fechaFin, graficoWeb, data, indicador);
                return graficoWeb;
            }
            else
            {
                var graficoWeb = new GraficoWeb
                {
                    SeriesData = seriesData,
                    //Titulo cuoata de mercado
                    TitleText = tituloDash,
                    //Rango  de Cuota de HHI
                    Series = new List<RegistroSerie>()
                {
                 new RegistroSerie{ To = ConstantesMonitoreo.PosicionInicial,From= ConstantesMonitoreo.PosicionUnoHhi,Color= "#B5C6C6",Porcentaje= 60 },
                 new RegistroSerie{ To = ConstantesMonitoreo.PosicionUnoHhi,From= ConstantesMonitoreo.PosicionDosHhi,Color= "#28E60A",Porcentaje= 60},
                 new RegistroSerie{ To = ConstantesMonitoreo.PosicionDosHhi,From= ConstantesMonitoreo.PosicionTresHhi,Color= "#F1F907",Porcentaje= 60 },
                 new RegistroSerie{ To = ConstantesMonitoreo.PosicionTresHhi,From= ConstantesMonitoreo.PosicionCuatroHhi,Color= "#FF0000",Porcentaje= 60 },

                }
                };
                GenerarDashBoard(empresas, fechaInicio, fechaFin, graficoWeb, data, indicador);
                return graficoWeb;
            }
        }

        /// <summary>
        /// Grafico Periodo Barra
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public GraficoWeb GraficoGrupoDespacho(string empresas, DateTime fechaInicio, DateTime fechaFin, List<MeMedicion48DTO> data)
        {

            GraficoWeb Grafico = new GraficoWeb();
            Grafico.Series = new List<RegistroSerie>();
            Grafico.SeriesType = new List<string>();
            Grafico.SeriesName = new List<string>();
            Grafico.YAxixTitle = new List<string>();

            int[] empresaslistado = new int[empresas.Length];
            empresaslistado = empresas.Split(',').Select(x => int.Parse(x)).ToArray();
            // Obtiene data de indicador 1
            List<MmmDatoDTO> listFacTable = FactorySic.GetMmmDatoRepository().ListPeriodo(fechaInicio, fechaFin.AddDays(1));
            var listaGrupo = listFacTable.Select(x => new { x.Emprcodi, x.Grupocodi, x.Catecodi }).Where(x => empresaslistado.Contains(x.Emprcodi)).Distinct().ToList();

            Grafico.SerieDataS = new DatosSerie[listaGrupo.Count][];

            int termoCant = 0;
            int hidromoCant = 0;
            int otros = 0;

            foreach (var grupoRec in listaGrupo)
            {
                if (grupoRec.Catecodi == 2 || grupoRec.Catecodi == 3 || grupoRec.Catecodi == 4)
                {
                    termoCant++;
                }

                else if (grupoRec.Catecodi == 5 || grupoRec.Catecodi == 6 || grupoRec.Catecodi == 9)
                {
                    hidromoCant++;
                }
                else if (grupoRec.Catecodi != 0)
                {
                    otros++;

                }
            }

            for (int i = 1; i <= 2; i++)
            {
                if (i == 1 && termoCant > 0)
                {
                    RegistroSerie regSerie = new RegistroSerie();

                    regSerie.Name = "Termo";
                    regSerie.Type = "bar";
                    regSerie.Color = "orange";
                    List<DatosSerie> listadata = new List<DatosSerie>();


                    regSerie.TipoPto = termoCant;
                    regSerie.Data = listadata;
                    Grafico.Series.Add(regSerie);
                }
                else if (hidromoCant > 0)
                {
                    RegistroSerie regSerie = new RegistroSerie();

                    regSerie.Name = "Hidro";
                    regSerie.Type = "bar";
                    regSerie.Color = "blue";
                    List<DatosSerie> listadata = new List<DatosSerie>();

                    regSerie.TipoPto = hidromoCant;
                    regSerie.Data = listadata;
                    Grafico.Series.Add(regSerie);
                }
                else if (otros > 0)
                {
                    RegistroSerie regSerie = new RegistroSerie();

                    regSerie.Name = "Hidro";
                    regSerie.Type = "bar";
                    regSerie.Color = "green";
                    List<DatosSerie> listadata = new List<DatosSerie>();

                    regSerie.Acumulado = otros;
                    regSerie.Data = listadata;
                    Grafico.Series.Add(regSerie);
                }
            }


            Grafico.TitleText = "Total Grupos de Despacho por tipo de generación";
            if (listaGrupo.Count > 0)
            {
                Grafico.XAxisCategories = new List<string>();
                Grafico.SeriesType = new List<string>();
                Grafico.SeriesYAxis = new List<int>();
            }


            return Grafico;
        }

        /// <summary>
        /// Dashboard Promedio RSI INDICADOR 4
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public GraficoMedidorGeneracionMoni GraficoColumnaCuotaMercadoHHI(string empresa, DateTime fechaInicio, DateTime fechaFin, List<MeMedicion48DTO> data, int indicador)
        {
            int TipoIndicador = indicador == 1 ? ConstantesMonitoreo.TipoFormulaCuota : ConstantesMonitoreo.TipoFormulaHHICuotaMercado;
            string tituloIndicador = indicador == 1 ? ConstantesMonitoreo.TituloBarraCuota : ConstantesMonitoreo.TituloHHI;

            List<PuntoGraficoMedidorGeneracionMoni> listaGraf = new List<PuntoGraficoMedidorGeneracionMoni>();
            //variables
            List<SiEmpresaDTO> listaEmpresa = this.ListarEmpresasMonitoreo(fechaInicio, empresa);
            List<string> fechaLimites = new List<string>();
            //Generar data
            GraficoMedidorGeneracionMoni g = new GraficoMedidorGeneracionMoni();

            DateTime fechaNuevaInicio = new DateTime(fechaInicio.Year, fechaInicio.Month, 1);
            DateTime fechaNuevaFin = new DateTime(fechaInicio.Year, fechaInicio.Month, (fechaNuevaInicio.AddMonths(1).AddDays(-1).Day));


            TimeSpan dat = fechaNuevaFin - fechaNuevaInicio;
            int dias = dat.Days == 0 ? 1 : (fechaNuevaFin.Day);

            int cantidad = (dias) * 48;

            List<string> nombreEmpresa = new List<string>();
            int limite = 0;
            foreach (var empresas in listaEmpresa)
            {
                int reccoridoEmpresa = 0;
                PuntoGraficoMedidorGeneracionMoni gr = new PuntoGraficoMedidorGeneracionMoni();
                gr.ListaFuente1 = new decimal?[dias];
                for (var day = fechaNuevaInicio.Date; day.Date <= fechaNuevaFin.Date; day = day.AddDays(1))
                {
                    decimal? totalPromedio = 0;

                    DateTime horas = day.AddMinutes(30);
                    for (int h = 1; h <= 48; h++)
                    {
                        decimal? valor = null;
                        MeMedicion48DTO total = data.Find(x => x.Medifecha == day && x.TipoFormulaMonitoreo == TipoIndicador && x.Emprcodi == empresas.Emprcodi);

                        if (total != null)
                        {
                            valor = (decimal?)total.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(total, null);
                        }

                        totalPromedio = totalPromedio + valor.GetValueOrDefault(0);

                        horas = horas.AddMinutes(30);
                    }
                    gr.ListaFuente1[reccoridoEmpresa] = Decimal.Round((totalPromedio / (48)).Value, 2);
                    reccoridoEmpresa = reccoridoEmpresa + 1;

                    gr.Empresa = empresas.Emprnomb;
                    gr.Fecha = horas;
                    gr.FechaString = gr.Fecha.ToString(ConstantesAppServicio.FormatoFechaHora);


                    if (limite == 0)
                    {
                        fechaLimites.Add(day.ToString(ConstantesAppServicio.FormatoFecha));
                    }
                }
                limite = limite + 1;
                listaGraf.Add(gr);
            }

            g.NombreEmpresa = nombreEmpresa;
            g.ListaPunto = listaGraf;
            g.CategoriaFecha = fechaLimites;


            g.TituloGrafico = tituloIndicador + EPDate.f_NombreMes(fechaInicio.Month).ToUpper() + " " + fechaInicio.Year;

            string tituloY = indicador == ConstantesMonitoreo.CodigoS ? ConstantesMonitoreo.TituloCuotaMercado : ConstantesMonitoreo.TituloHHIY;

            g.TituloFuente1 = tituloY;
            return g;
        }

        /// <summary>
        /// Dashboard Promedio RSI INDICADOR 4
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public GraficoMedidorGeneracionMoni GenerarPromedioGraficoResidual(string empresa, DateTime fechaInicio, DateTime fechaFin, List<MeMedicion48DTO> data)
        {
            List<PuntoGraficoMedidorGeneracionMoni> listaGraf = new List<PuntoGraficoMedidorGeneracionMoni>();
            //variables
            List<SiEmpresaDTO> listaEmpresa = this.ListarEmpresasMonitoreo(fechaInicio, empresa);
            List<string> fechaLimites = new List<string>();
            //Generar data
            GraficoMedidorGeneracionMoni g = new GraficoMedidorGeneracionMoni();
            TimeSpan dat = fechaFin - fechaInicio;

            int dias = dat.Days == 0 ? 1 : dat.Days;

            int cantidad = (dias) * 48;

            List<string> nombreEmpresa = new List<string>();
            int limite = 0;
            PuntoGraficoMedidorGeneracionMoni gr = new PuntoGraficoMedidorGeneracionMoni();
            int recorridoEmpresa = 0;
            gr.ListaFuente1 = new decimal?[listaEmpresa.Count];
            foreach (var empresas in listaEmpresa)
            {
                decimal? PromedioRsi = 0;
                for (var day = fechaInicio.Date; day.Date < fechaFin.Date; day = day.AddDays(1))
                {
                    DateTime horas = day.AddMinutes(30);
                    for (int h = 1; h <= 48; h++)
                    {
                        MeMedicion48DTO total = data.Find(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaRSD && x.Emprcodi == empresas.Emprcodi);
                        decimal? TotalResidual = null;
                        if (total != null)
                        {
                            TotalResidual = (decimal?)total.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(total, null);
                        }
                        gr.Empresa = empresas.Emprnomb;
                        gr.Fecha = horas;
                        gr.FechaString = gr.Fecha.ToString(ConstantesAppServicio.FormatoFechaHora);

                        if (TotalResidual == null)
                        {
                            TotalResidual = 0;
                        }
                        PromedioRsi = PromedioRsi + TotalResidual;
                        horas = horas.AddMinutes(30);

                        if (limite == 0)
                        {
                            fechaLimites.Add(horas.ToString(ConstantesAppServicio.FormatoFechaHora));
                        }
                    }
                }
                gr.ListaFuente1[recorridoEmpresa] = Decimal.Round((PromedioRsi / (48 * dias)).Value, 2);
                recorridoEmpresa = recorridoEmpresa + 1;
                limite = limite + 1;
                nombreEmpresa.Add(empresas.Emprnomb);
            }
            listaGraf.Add(gr);
            g.NombreEmpresa = nombreEmpresa;
            g.ListaPunto = listaGraf;
            g.CategoriaFecha = fechaLimites;
            g.TituloGrafico = ConstantesMonitoreo.TituloPromedioResidual;

            return g;
        }

        /// <summary>
        /// Graficos HehI
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public GraficoMedidorGeneracionMoni GenerarGraficoHehi(string empresa, DateTime fechaInicio, DateTime fechaFin, List<MeMedicion48DTO> data)
        {
            List<PuntoGraficoMedidorGeneracionMoni> listaGraf = new List<PuntoGraficoMedidorGeneracionMoni>();
            //variables
            int tipoGrafico = 0;
            string nombreFuente1 = string.Empty, nombreFuente2 = string.Empty;
            string valorFuente1 = string.Empty, valorFuente2 = string.Empty;
            string tituloFuente1 = string.Empty, tituloFuente2 = string.Empty;
            string leyendaFuente1 = string.Empty, leyendaFuente2 = string.Empty;


            //Generar data
            for (var day = fechaInicio.Date; day.Date < fechaFin.Date; day = day.AddDays(1))
            {
                DateTime horas = day.AddMinutes(30);
                for (int h = 1; h <= 48; h++)
                {
                    MeMedicion48DTO total = data.Find(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaHHI);

                    decimal? TotalHhi = null;
                    if (total != null)
                    {
                        TotalHhi = (decimal?)total.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(total, null);
                    }
                    //fecha
                    PuntoGraficoMedidorGeneracionMoni gr = new PuntoGraficoMedidorGeneracionMoni();
                    // aqui es donde aGREGAR  

                    gr.Fecha = horas;
                    gr.FechaString = gr.Fecha.ToString(ConstantesAppServicio.FormatoFechaHora);
                    gr.ValorFuente1 = Decimal.Round(TotalHhi.Value, 3);
                    listaGraf.Add(gr);
                    horas = horas.AddMinutes(30);
                }
            }

            //crear objeto grafico
            GraficoMedidorGeneracionMoni g = new GraficoMedidorGeneracionMoni();
            g.ListaPunto = listaGraf;
            g.TituloGrafico = ConstantesMonitoreo.TituloEvoluacionMediaHHI + "-" + EPDate.f_NombreMes(fechaInicio.Month).ToUpper() + " " + fechaInicio.Year;
            g.LeyendaFuente1 = "HHI";
            g.Nombre = string.Format("{0}_{1:HHmmss}.xlsx", g.TituloGrafico, DateTime.Now);
            g.FechaInicio = fechaInicio.ToString(ConstantesAppServicio.FormatoFechaHora);
            g.FechaFin = fechaFin.ToString(ConstantesAppServicio.FormatoFechaHora);
            g.TipoGrafico = tipoGrafico;
            return g;
        }

        /// <summary>
        /// Graficos HehI
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public GraficoMedidorGeneracionMoni GenerarGraficoIndiceOfertaPivotalResidual(string empresa, DateTime fechaInicio, DateTime fechaFin, List<MeMedicion48DTO> data, int indicador)
        {
            int tipoIndicador = indicador == 3 ? ConstantesMonitoreo.TipoFormulaIOP : ConstantesMonitoreo.TipoFormulaRSD;
            string tituloIndicador = indicador == 3 ? ConstantesMonitoreo.TituloPivotal : ConstantesMonitoreo.TituloResidual;

            List<PuntoGraficoMedidorGeneracionMoni> listaGraf = new List<PuntoGraficoMedidorGeneracionMoni>();
            //variables
            List<SiEmpresaDTO> listaEmpresa = this.ListarEmpresasMonitoreo(fechaInicio, empresa);
            List<string> fechaLimites = new List<string>();
            //Generar data
            GraficoMedidorGeneracionMoni g = new GraficoMedidorGeneracionMoni();

            TimeSpan dat = fechaFin - fechaInicio;
            int dias = dat.Days == 0 ? 1 : (dat.Days);

            int cantidad = (dias) * 48;

            List<string> nombreEmpresa = new List<string>();
            int limite = 0;
            foreach (var empresas in listaEmpresa)
            {
                int reccoridoEmpresa = 0;

                PuntoGraficoMedidorGeneracionMoni gr = new PuntoGraficoMedidorGeneracionMoni();
                gr.ListaFuente1 = new decimal?[cantidad];
                for (var day = fechaInicio.Date; day.Date < fechaFin.Date; day = day.AddDays(1))
                {

                    DateTime horas = day.AddMinutes(30);
                    for (int h = 1; h <= 48; h++)
                    {
                        MeMedicion48DTO total = data.Find(x => x.Medifecha == day && x.TipoFormulaMonitoreo == tipoIndicador && x.Emprcodi == empresas.Emprcodi);
                        decimal? TotalResidual = null;
                        if (total != null)
                        {
                            TotalResidual = (decimal?)total.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(total, null);
                        }
                        gr.Empresa = empresas.Emprnomb;
                        gr.Fecha = horas;
                        gr.FechaString = gr.Fecha.ToString(ConstantesAppServicio.FormatoFechaHora);

                        if (TotalResidual == null)
                        {
                            gr.ListaFuente1[reccoridoEmpresa] = null;
                        }
                        else
                        {
                            gr.ListaFuente1[reccoridoEmpresa] = Decimal.Round(TotalResidual.Value, 2);
                        }



                        if (limite == 0)
                        {
                            fechaLimites.Add(horas.ToString(ConstantesAppServicio.FormatoFechaHora));
                        }

                        horas = horas.AddMinutes(30);
                        reccoridoEmpresa = reccoridoEmpresa + 1;
                    }
                    nombreEmpresa.Add(empresas.Emprnomb);
                }
                limite = limite + 1;
                listaGraf.Add(gr);
            }
            //   g.NombreEmpresa = nombreEmpresa;
            g.ListaPunto = listaGraf;
            g.CategoriaFecha = fechaLimites;
            g.TituloGrafico = tituloIndicador;
            return g;
        }

        /// <summary>
        /// DashBoard Precio Costo
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="data"></param>
        /// <param name="listadoBarra"></param>
        /// <returns></returns>
        public GraficoMedidorGeneracionMoni GenerarGraficoCurvasLernerCostoPrecio(string empresa, DateTime fechaInicio, DateTime fechaFin, List<MeMedicion48DTO> data, string listadoBarra, int indicador)
        {

            int TipoIndicador = indicador == 5 ? ConstantesMonitoreo.TipoFormulaILE : ConstantesMonitoreo.TipoFormulaIMU;
            string tituloIndicador = indicador == 5 ? ConstantesMonitoreo.TituloIndiceLener : ConstantesMonitoreo.TituloIndicePrecioCosto;

            List<PuntoGraficoMedidorGeneracionMoni> listaGraf = new List<PuntoGraficoMedidorGeneracionMoni>();
            //variables
            List<BarraDTO> listaBarra = ListarGrupoBarra();
            List<SiEmpresaDTO> listaEmpresa = this.ListarEmpresasMonitoreo(fechaInicio, empresa);
            int[] barralistado = new int[listadoBarra.Length];
            barralistado = listadoBarra.Split(',').Select(x => int.Parse(x)).ToArray();

            List<BarraDTO> listaBarraRecorrido = listaBarra.Where(x => barralistado.Contains(x.BarrCodi)).ToList();

            //Generar data
            List<string> nombreEmpresa = new List<string>();
            List<int> codigoEmpresa = new List<int>();
            List<int> codigoBarra = new List<int>();
            List<string> fechaLimites = new List<string>();
            List<DateTime> fechaLimites2 = new List<DateTime>();

            GraficoMedidorGeneracionMoni g = new GraficoMedidorGeneracionMoni();
            TimeSpan dat = fechaFin - fechaInicio;

            int dias = dat.Days == 0 ? 1 : (dat.Days);

            int cantidad = (dias) * 48;
            int limite = 0;
            foreach (var empresas in listaEmpresa)
            {
                // Recorrido de barras
                var listaBarr = listaBarraRecorrido.Select(x => new { x.BarrCodi, x.BarrNombre, x.Emprcodi }).Where(x => x.Emprcodi == empresas.Emprcodi).Distinct().ToList();
                foreach (var barra in listaBarr)
                {
                    int recorridoBarra = 0;
                    PuntoGraficoMedidorGeneracionMoni gr = new PuntoGraficoMedidorGeneracionMoni();
                    gr.ListaFuente1 = new decimal?[cantidad];

                    for (var day = fechaInicio.Date; day.Date < fechaFin.Date; day = day.AddDays(1))
                    {
                        int diaInicio = fechaInicio.Day;
                        int diaFin = fechaFin.Day;
                        DateTime horas = day.AddMinutes(30);
                        gr.Empresa = empresas.Emprnomb + " - " + barra.BarrNombre + tituloIndicador;
                        MeMedicion48DTO total = data.Find(x => x.Medifecha == day && x.TipoFormulaMonitoreo == TipoIndicador && x.Emprcodi == empresas.Emprcodi && x.Barrcodi == barra.BarrCodi);
                        for (int h = 1; h <= 48; h++)
                        {
                            decimal? TotalResidual = null;
                            if (total != null)
                            {
                                TotalResidual = (decimal?)total.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(total, null);
                            }

                            if (TotalResidual == null)
                            {
                                gr.ListaFuente1[recorridoBarra] = null;
                            }
                            else
                            {
                                gr.ListaFuente1[recorridoBarra] = Decimal.Round(TotalResidual.Value, 2);
                            }

                            recorridoBarra = recorridoBarra + 1;

                            if (limite == 0)
                            {
                                fechaLimites.Add(horas.ToString(ConstantesAppServicio.FormatoFechaHora));
                                horas = horas.AddMinutes(30);
                            }

                        }
                    }
                    listaGraf.Add(gr);
                    limite = limite + 1;
                }
            }
            g.CategoriaFecha = fechaLimites;
            g.ListaPunto = listaGraf;
            g.TituloGrafico = indicador == ConstantesMonitoreo.CodigoILE ? ConstantesMonitoreo.TituloLener : ConstantesMonitoreo.TituloPrecioCosto;
            return g;
        }

        /// <summary>
        /// Genera DashBoard
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="graficoWeb"></param>
        /// <param name="data"></param>
        private void GenerarDashBoard(string empresas, DateTime fechaInicio, DateTime fechaFin, GraficoWeb graficoWeb, List<MeMedicion48DTO> data, int indicador)
        {
            graficoWeb.YaxixMax = graficoWeb.SeriesData[0].Max(x => x.Value);
            graficoWeb.YaxixMin = graficoWeb.SeriesData[0].Min(x => x.Value);
            List<SiEmpresaDTO> listaEmpresa = this.ListarEmpresasMonitoreo(fechaInicio, empresas);
            // Devuelve 1 si es cuota de mercado y 2 si es HeHI
            int TipoIndicador = indicador == 1 ? ConstantesMonitoreo.TipoFormulaCuota : ConstantesMonitoreo.TipoFormulaHHICuotaMercado;
            // Obtiene  promedio total de la cuota de mercado
            decimal valorTotal = 0;
            // Obtiene data de indicador 1

            TimeSpan dat = fechaFin - fechaInicio;

            int dias = dat.Days == 0 ? 1 : dat.Days;

            int cantidad = (dias) * 48;

            for (DateTime day = fechaInicio.Date; day < fechaFin.Date; day = day.AddDays(1))
            {
                foreach (var empresa in listaEmpresa)
                {
                    for (int h = 1; h <= 48; h++)
                    {
                        decimal? cuotaMercado = 0;
                        MeMedicion48DTO reg = data.Find(x => x.Medifecha == day && x.Emprcodi == empresa.Emprcodi && x.TipoFormulaMonitoreo == TipoIndicador);

                        if (reg == null)
                        {
                            cuotaMercado = 0;
                        }
                        else
                        {
                            cuotaMercado = (decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg, null);
                        }
                        valorTotal = valorTotal + cuotaMercado.GetValueOrDefault(0);
                    }
                }
            }

            Decimal resultadoTotal = TipoIndicador == ConstantesMonitoreo.TipoFormulaCuota ? Decimal.Round(valorTotal / (48 * dias), 2) : Decimal.Round(valorTotal / (48 * dias), 2);
            DatosSerie[][] objTotalCuotaMercado = new DatosSerie[1][];
            objTotalCuotaMercado[0] = new DatosSerie[5];
            objTotalCuotaMercado[0][0] = new DatosSerie() { Color = "black", Z = resultadoTotal };
            graficoWeb.SerieDataS = objTotalCuotaMercado;
        }

        /// <summary>
        /// Valores Indicadores del DashBoard 
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public string ResultadoHtmlIndicador(string empresas, DateTime fechaInicio, DateTime fechaFin, List<MeMedicion48DTO> data, int indicador)
        {
            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";
            List<SiEmpresaDTO> listaEmpresa = this.ListarEmpresasMonitoreo(fechaInicio, empresas);

            decimal? resultado = 0;

            switch (indicador)
            {
                case ConstantesMonitoreo.CodigoS:
                    // Recorrido  de dias 
                    for (var day = fechaInicio.Date; day.Date < fechaFin.Date; day = day.AddDays(1))
                    {
                        if (listaEmpresa.Count >= 1)
                        {
                            decimal? pot = 0;
                            // Recorrido sumatoria total de MW Ejectuado por filtro de Empresas 
                            foreach (var empresa in listaEmpresa)
                            {

                                for (int h = 1; h <= 48; h++)
                                {
                                    MeMedicion48DTO reg = data.Find(x => x.Medifecha == day && x.Emprcodi == empresa.Emprcodi && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaMWEjec);

                                    if (reg != null)
                                    {
                                        pot = (decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg, null);

                                    }
                                    resultado = resultado + pot.GetValueOrDefault(0);
                                }
                            }
                        }
                    }
                    strHtml.Append("<div class='dashboard_graf_num_caja'>");
                    strHtml.Append("<div class='dashboard_graf_num_titulo'>Energía Acumulada (MWh)</div>");
                    strHtml.Append("<br>");
                    strHtml.Append("<br>");
                    strHtml.Append("<div class='dashboard_graf_num_valor'>" + (resultado.Value / 2).ToString("N", nfi) + " K </div>");
                    strHtml.Append(" </div>");
                    break;
                case ConstantesMonitoreo.CodigoHHI:
                    strHtml.Append("<div class='dashboard_graf_num_caja'>");
                    strHtml.Append("<div class='dashboard_graf_num_titulo'>Total Generadores Integrantes del COES</div>");
                    strHtml.Append("<br>");
                    strHtml.Append("<br>");
                    strHtml.Append("<div class='dashboard_graf_num_valor'>" + listaEmpresa.Count + "</div>");
                    strHtml.Append(" </div>");
                    break;
                case ConstantesMonitoreo.CodigoIOP:

                    int cantidad2 = 0;

                    //IOP x Empresa
                    foreach (var empresa in listaEmpresa)
                    {
                        int iop = ConstantesMonitoreo.ValorIOPEsPivotal;
                        int total = 0;
                        for (var day = fechaInicio.Date; day.Date < fechaInicio.AddDays(1).Date; day = day.AddDays(1))
                        {
                            MeMedicion48DTO reg = data.Find(x => x.Medifecha == day && x.Emprcodi == empresa.Emprcodi && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaIOP);
                            for (int h = 1; h <= 48; h++)
                            {

                                if (reg != null)
                                {
                                    iop = Convert.ToInt32(((decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg, null)).GetValueOrDefault(0));
                                }

                                total = total + iop;
                            }
                        }
                        cantidad2 = (total / 48) >= 1 ? cantidad2 + 1 : cantidad2 + 0;
                    }

                    strHtml.Append("<div class='dashboard_graf_num_caja'>");
                    strHtml.Append("<div class='dashboard_graf_num_titulo'>Total Generadores Integrantes del COES PIVOT </div>");
                    //Calcular Valor PIVOTAL 
                    strHtml.Append("<div class='dashboard_graf_num_valor'>" + cantidad2 + "</div>");
                    strHtml.Append(" <br>");
                    strHtml.Append(" <br>");
                    strHtml.Append(" </div>");
                    strHtml.Append(" <br>");
                    strHtml.Append("<div class='dashboard_graf_num_caja'>");
                    strHtml.Append("<div class='dashboard_graf_num_titulo'>Total Generadores Integrantes del COES</div>");
                    strHtml.Append("<br>");
                    strHtml.Append("<br>");
                    strHtml.Append("<div class='dashboard_graf_num_valor'>" + listaEmpresa.Count + "</div>");
                    break;
                case ConstantesMonitoreo.CodigoRSD:

                    int totalFinalMenorRsd = 0;
                    int totalFinalMayorRsd = 0;

                    foreach (var empresas2 in listaEmpresa)
                    {
                        int mayorRsd = 0;

                        for (var day = fechaInicio.Date; day.Date < fechaFin.Date; day = day.AddDays(1))
                        {
                            DateTime horas = day.AddMinutes(30);
                            for (int h = 1; h <= 48; h++)
                            {
                                MeMedicion48DTO total = data.Find(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaRSD && x.Emprcodi == empresas2.Emprcodi);
                                decimal? valorResidual = null;
                                if (total != null)
                                {
                                    valorResidual = (decimal?)total.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(total, null);
                                    mayorRsd = valorResidual > 100 ? mayorRsd + 1 : mayorRsd + 0;
                                }
                            }
                        }

                        totalFinalMayorRsd = mayorRsd > 0 ? totalFinalMayorRsd + 1 : totalFinalMayorRsd + 0;
                        totalFinalMenorRsd = mayorRsd < 1 ? totalFinalMenorRsd + 1 : totalFinalMenorRsd + 0;
                    }

                    strHtml.Append("<div class='dashboard_graf_num_caja'>");
                    strHtml.Append("<div class='dashboard_graf_num_titulo'>GENERADORES CON RSD > 100% </div>");
                    strHtml.Append("<br>");
                    strHtml.Append("<br>");
                    strHtml.Append("<div class='dashboard_graf_num_valor'>" + totalFinalMayorRsd + "</div>");
                    strHtml.Append(" </div>");
                    strHtml.Append(" <br>");
                    strHtml.Append("<div class='dashboard_graf_num_caja'>");
                    strHtml.Append("<div class='dashboard_graf_num_titulo'>GENERADORES CON RSD < 100%</div>");
                    strHtml.Append("<br>");
                    strHtml.Append("<br>");
                    strHtml.Append("<div class='dashboard_graf_num_valor'>" + totalFinalMenorRsd + "</div>");
                    strHtml.Append(" </div>");
                    break;
            }
            return strHtml.ToString();
        }

        /// <summary>
        /// Retorno valor filtro de fecha
        /// </summary>
        /// <param name="data"></param>
        /// <param name="empresas"></param>
        /// <param name="fechaDiaInicio"></param>
        /// <param name="fechaDiaFin"></param>
        /// <param name="fechaSemana"></param>
        /// <param name="fechaMes"></param>
        /// <param name="periodo"></param>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        public void DataDashBoardMw(out List<MeMedicion48DTO> data, int indicador, DateTime fechaDiaInicio, DateTime fechaDiaFin, string fechaSemana, string fechaMes, string periodo, out DateTime f1, out DateTime f2, out DateTime f3, out DateTime f4)
        {
            int IndicadorMMI = indicador == 1 ? ConstantesMonitoreo.CodigoS : indicador == 2 ? ConstantesMonitoreo.CodigoHHI : indicador == 3 ? ConstantesMonitoreo.CodigoIOP : indicador == 4 ? ConstantesMonitoreo.CodigoRSD : indicador == 5 ? ConstantesMonitoreo.CodigoILE : ConstantesMonitoreo.CodigoIMU;
            //Lista  la ultima version
            var listVersion = this.GetByCriteriaMmmVersions().Where(x => x.Vermmfechaperiodo.Date == fechaDiaInicio.Date).OrderByDescending(x => x.Vermmcodi).FirstOrDefault();
            int? version;
            version = listVersion != null ? version = listVersion.Vermmcodi : version = -1;
            //Declaracion de fechas para recorrido en el  DashBoard
            f1 = fechaDiaInicio;
            f2 = fechaDiaFin;
            f3 = fechaDiaInicio;
            f4 = fechaDiaFin;

            data = new List<MeMedicion48DTO>();

            List<SiEmpresaDTO> listaEmpresa;

            switch (periodo)
            {
                case ConstantesMonitoreo.PeriodoDia:
                    f2 = fechaDiaFin.AddDays(1);
                    if (indicador == 1 || indicador == 2)
                    {
                        DateTime fechaIniPeriodo = new DateTime(fechaDiaInicio.Year, fechaDiaInicio.Month, 1);
                        DateTime fechaFinPeriodo = fechaIniPeriodo.AddMonths(1).AddDays(-1);
                        List<MmmDatoDTO> listFacTable = FactorySic.GetMmmDatoRepository().ListPeriodo(fechaIniPeriodo.AddMinutes(30), fechaFinPeriodo.AddDays(1));
                        data = this.ListarIndicador(version.Value, fechaIniPeriodo, fechaFinPeriodo, IndicadorMMI, listFacTable, ConstantesMonitoreo.Dashboard, out listaEmpresa);
                    }
                    else
                    {
                        List<MmmDatoDTO> listFacTable = FactorySic.GetMmmDatoRepository().ListPeriodo(f1, f2);
                        data = this.ListarIndicador(version.Value, fechaDiaInicio, fechaDiaFin, IndicadorMMI, listFacTable, ConstantesMonitoreo.Dashboard, out listaEmpresa);
                    }
                    break;

                case ConstantesMonitoreo.PeriodoSemana:
                    f1 = GetFechaIniPeriodo(fechaSemana);
                    f2 = f1.AddDays(7);
                    if (indicador == 1 || indicador == 2)
                    {
                        f3 = GetFechaIniPeriodo(fechaSemana);
                        f4 = f1.AddDays(7);
                        f3 = new DateTime(f3.Year, f3.Month, 1);
                        f4 = f3.AddMonths(1).AddDays(-1);
                        List<MmmDatoDTO> listFacTable = FactorySic.GetMmmDatoRepository().ListPeriodo(f3, f4.AddDays(1));
                        data = this.ListarIndicador(version.Value, f3, f4, IndicadorMMI, listFacTable, ConstantesMonitoreo.Dashboard, out listaEmpresa);
                    }
                    else
                    {
                        List<MmmDatoDTO> listFacTable = FactorySic.GetMmmDatoRepository().ListPeriodo(f1, f2);
                        data = this.ListarIndicador(version.Value, f1, f2, IndicadorMMI, listFacTable, ConstantesMonitoreo.Dashboard, out listaEmpresa);
                    }
                    break;

                case ConstantesMonitoreo.PeriodoMes:
                    string fechan = fechaMes.ToString();
                    f1 = new DateTime(Int32.Parse(fechan.Substring(3, 4)), Int32.Parse(fechan.Substring(0, 2)), 1);
                    f2 = f1.AddMonths(1).AddDays(-1);
                    f3 = new DateTime(Int32.Parse(fechan.Substring(3, 4)), Int32.Parse(fechan.Substring(0, 2)), 1);
                    f4 = f1.AddMonths(1).AddDays(-1);
                    List<MmmDatoDTO> listFacTable2 = FactorySic.GetMmmDatoRepository().ListPeriodo(f1.AddMinutes(30), f2.AddDays(1));
                    data = this.ListarIndicador(version.Value, f1, f2, IndicadorMMI, listFacTable2, ConstantesMonitoreo.Dashboard, out listaEmpresa);
                    break;
            }
        }

        /// <summary>
        /// FeCHA pERIODO
        /// </summary>
        /// <param name="semana"></param>
        /// <returns></returns>
        public static DateTime GetFechaIniPeriodo(string semana)
        {
            DateTime fechaProceso = DateTime.MinValue;


            int anho = Int32.Parse(semana.Substring(0, 4));
            int sem = Int32.Parse(semana.Substring(4, semana.Length - 4));
            fechaProceso = EPDate.f_fechainiciosemana(anho, sem);
            return fechaProceso;
        }

        #endregion

        #region Log de Errores Monitores

        /// <summary>
        /// Obtener Html del log errores  Indicador 1
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        /// 
        public string LogErrorIndicador1Html(string empresas, DateTime fechaInicio, DateTime fechaFin, int vermmcodi, List<MmmDatoDTO> listFacTable)
        {
            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";

            List<SiEmpresaDTO> listaEmpresa;
            List<MeMedicion48DTO> data = this.ListarIndicador(vermmcodi, fechaInicio, fechaFin, ConstantesMonitoreo.CodigoS, listFacTable, ConstantesMonitoreo.LogErroresMonitoreo, out listaEmpresa);
            listaEmpresa = this.ListarEmpresasMonitoreoByData(listaEmpresa, empresas);

            if (listaEmpresa.Count == 0) return string.Empty;

            int padding = 20;
            int anchoTotal = (100 + padding) + listaEmpresa.Count * (200 + padding);

            strHtml.Append("<div class='freeze_table' id='resultado1' style='height: 800px;'>");
            strHtml.AppendFormat("<table id='reporte1' class='pretty tabla-icono' style='table-layout: fixed; width: {0}px;' >", anchoTotal);
            strHtml.Append("<thead>");
            #region cabecera
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width: 110px;' rowspan=2> MM/DD/hh:mm</th>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px' rowspan=2> PES </th>");
            for (int i = 1; i <= 2; i++)
            {
                foreach (var datos in listaEmpresa)
                {
                    strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 100px' class='cuota_{1}'>{0}</th>", datos.Emprnomb, i);
                }
            }
            strHtml.Append("</tr>");
            strHtml.Append("<tr>");

            for (int i = 1; i <= 2; i++)
            {
                string valor = (i == 1) ? " PE (MW)" : " S (%)";
                foreach (var datos in listaEmpresa)
                {
                    strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 100px' class='cuota_{1}'>{0}</th>", valor, i);
                }
            }
            strHtml.Append("</tr>");
            #endregion
            strHtml.Append("</thead>");
            strHtml.Append("<tbody>");
            #region cuerpo
            // Día - Hora

            for (var day = fechaInicio.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
            {
                MeMedicion48DTO total = data.Find(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaTotalPotencia);

                //HORA
                DateTime horas = day.AddMinutes(30);

                for (int h = 1; h <= 48; h++)
                {
                    strHtml.Append("<tr>");
                    strHtml.Append(string.Format("<td class='tdbody_reporte'>{0:dd/MM/yyyy HH:mm}</td>", horas));

                    //Potencia total
                    decimal? potenciaTotal = null;
                    if (total != null)
                    {
                        potenciaTotal = (decimal?)total.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(total, null);
                    }
                    strHtml.Append(string.Format("<td>{0}</td>", potenciaTotal != null ? (potenciaTotal.Value).ToString("N", nfi) : string.Empty));

                    for (int i = 1; i <= 2; i++)
                    {
                        foreach (var empresa in listaEmpresa)
                        {
                            int cantidad = 0;
                            //Potencia / Cuota
                            MeMedicion48DTO reg = data.Find(x => x.Medifecha == day && x.Emprcodi == empresa.Emprcodi && x.TipoFormulaMonitoreo == i);
                            decimal? pot = null;
                            if (reg != null)
                            {
                                pot = (decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg, null);

                                decimal? repetido = 0;

                                for (int c = 1; c <= 48; c++)
                                {
                                    repetido = (decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + c).GetValue(reg, null);
                                    cantidad = repetido == pot && repetido != null && repetido != 0 ? cantidad + 1 : cantidad + 0;

                                }
                            }
                            string color = cantidad == 48 ? ConstantesMonitoreo.ColorLogError : string.Empty;
                            strHtml.Append(string.Format("<td style='background-color: {1};'>{0}</td>", pot != null && pot.GetValueOrDefault(0) != 0 ? (pot.Value).ToString("N", nfi) : string.Empty, color));

                        }
                    }
                    strHtml.Append("</tr>");
                    horas = horas.AddMinutes(30);
                }
            }

            #endregion
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Html indicador 2
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public string LogErrorIndicador2Html(string empresas, DateTime fechaInicio, DateTime fechaFin, int vermmcodi, List<MmmDatoDTO> listFacTable)
        {
            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";

            List<SiEmpresaDTO> listaEmpresa;
            List<MeMedicion48DTO> data = this.ListarIndicador(vermmcodi, fechaInicio, fechaFin, ConstantesMonitoreo.CodigoHHI, listFacTable, ConstantesMonitoreo.LogErroresMonitoreo, out listaEmpresa);
            listaEmpresa = this.ListarEmpresasMonitoreoByData(listaEmpresa, empresas);

            SiParametroValorDTO param = this.GetParametroTendenciaHHI(fechaInicio);
            decimal tendenciaUno = param.HHITendenciaUno.GetValueOrDefault(0);
            decimal tendenciaCero = param.HHITendenciaCero.GetValueOrDefault(0);
            string colorUno = param.HHITendenciaUnoColor;
            string colorCero = param.HHITendenciaCeroColor;

            if (listaEmpresa.Count == 0) return string.Empty;

            int padding = 20;
            int anchoTotal = (100 + padding) + listaEmpresa.Count * (200 + padding);
            strHtml.Append("<div class='freeze_table' id='resultado2' style='height: auto;'>");
            strHtml.AppendFormat("<table id='reporte2' class='pretty tabla-icono' style='table-layout: fixed; width: {0}px;' >", anchoTotal);
            strHtml.Append("<thead>");
            #region cabecera
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width: 110px;' rowspan=2>MM/DD/hh:mm</th>");
            foreach (var datos in listaEmpresa)
            {
                strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px'>" + datos.Emprnomb + "</th>");
            }
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px;' rowspan=2> HHI</th>");
            strHtml.Append("</tr>");
            strHtml.Append("<tr>");

            // Cabecera cuota de mercado al cuadrado
            foreach (var datos in listaEmpresa)
            {
                strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px'> S  <sup>2</sup> </th>");
            }
            strHtml.Append("</tr>");
            #endregion
            strHtml.Append("</thead>");
            strHtml.Append("<tbody>");
            #region cuerpo
            for (var day = fechaInicio.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
            {
                MeMedicion48DTO total = data.Find(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaHHI);

                DateTime horas = day.AddMinutes(30);
                for (int h = 1; h <= 48; h++)
                {
                    strHtml.Append("<tr>");
                    strHtml.Append(string.Format("<td class='tdbody_reporte'>{0:dd/MM/yyyy HH:mm}</td>", horas));

                    foreach (var empresa in listaEmpresa)
                    {
                        int cantidad = 0;
                        // Cuota para HHI
                        MeMedicion48DTO reg = data.Find(x => x.Medifecha == day && x.Emprcodi == empresa.Emprcodi && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaHHICuotaMercado);
                        decimal? cuotaHHI = null;
                        if (reg != null)
                        {
                            cuotaHHI = (decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg, null);

                            decimal? repetido = 0;

                            for (int c = 1; c <= 48; c++)
                            {
                                repetido = (decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + c).GetValue(reg, null);
                                cantidad = repetido == cuotaHHI && repetido != null && repetido != 0 ? cantidad + 1 : cantidad + 0;
                            }
                        }
                        string color = cantidad == 48 ? ConstantesMonitoreo.ColorLogError : string.Empty;
                        strHtml.Append(string.Format("<td style='background-color: {1};'>{0}</td>", cuotaHHI != null && cuotaHHI.GetValueOrDefault(0) != 0 ? (cuotaHHI.Value).ToString("N", nfi) : string.Empty, color));
                    }

                    decimal totalHhi = 0;
                    if (total != null)
                    {
                        totalHhi = ((decimal?)total.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(total, null)).GetValueOrDefault(0);
                    }

                    if (totalHhi >= tendenciaUno)
                    {
                        strHtml.AppendFormat("<td  style='background-color: {1}; color:White;'>{0}</td>", totalHhi != 0 ? totalHhi.ToString("N", nfi) : string.Empty, colorUno);
                    }
                    else if (totalHhi <= tendenciaCero)
                    {
                        strHtml.AppendFormat("<td  style='background-color: {1}; color:White;'>{0}</td>", totalHhi != 0 ? totalHhi.ToString("N", nfi) : string.Empty, colorCero);
                    }
                    else
                    {
                        strHtml.AppendFormat("<td >{0}</td>", totalHhi != 0 ? totalHhi.ToString("N", nfi) : string.Empty);
                    }

                    strHtml.Append("</tr>");
                    horas = horas.AddMinutes(30);
                }
            }
            #endregion
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Html indicador 3
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public string LogErrorIndicador3Html(string empresas, DateTime fechaInicio, DateTime fechaFin, int vermmcodi, List<MmmDatoDTO> listFacTable)
        {
            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";

            List<SiEmpresaDTO> listaEmpresa;
            List<MeMedicion48DTO> data = this.ListarIndicador(vermmcodi, fechaInicio, fechaFin, ConstantesMonitoreo.CodigoIOP, listFacTable, ConstantesMonitoreo.LogErroresMonitoreo, out listaEmpresa);
            listaEmpresa = this.ListarEmpresasMonitoreoByData(listaEmpresa, empresas);

            SiParametroValorDTO param = this.GetParametroOfertaPivotal(fechaInicio);
            string colorEsPivotal = param.IOPEsPivotalColor;
            string colorNoPivotal = param.IOPNoPivotalColor;

            if (listaEmpresa.Count == 0) return string.Empty;

            int padding = 20;
            int anchoTotal = (100 + padding) + listaEmpresa.Count * (200 + padding);
            strHtml.Append("<div class='freeze_table' id='resultado3' style='height: auto;'>");
            strHtml.AppendFormat("<table id='reporte3' class='pretty tabla-icono' style='table-layout: fixed; width: {0}px;' >", anchoTotal);
            strHtml.Append("<thead>");
            #region cabecera
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width: 110px;' rowspan=2>MM/DD/hh:mm</th>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px' rowspan=2 > MD  (MW)</th>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px' rowspan=2> PES (MW) </th>");
            for (int i = 1; i <= 2; i++)
            {
                foreach (var datos in listaEmpresa)
                {
                    strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px'>" + datos.Emprnomb + "</th>");
                }
            }

            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            for (int i = 1; i <= 2; i++)
            {
                foreach (var datos in listaEmpresa)
                {
                    string valor; valor = (i == 1) ? "PE (MW)" : "IOP";
                    strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px'> " + valor + " </th>");
                }
            }
            strHtml.Append("</tr>");
            #endregion
            strHtml.Append("</thead>");
            strHtml.Append("<tbody>");
            #region cuerpo
            for (var day = fechaInicio.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
            {
                MeMedicion48DTO total = data.Find(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaTotalPotencia);
                MeMedicion48DTO md = data.Find(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaMaximaDemanda);

                DateTime horas = day.AddMinutes(30);
                for (int h = 1; h <= 48; h++)
                {
                    strHtml.Append("<tr>");
                    strHtml.Append(string.Format("<td class='tdbody_reporte'>{0:dd/MM/yyyy HH:mm}</td>", horas));

                    //Maxima Demanda Programada
                    decimal? maximademanda = null;
                    if (md != null)
                    {
                        maximademanda = (decimal?)md.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(md, null);
                    }
                    strHtml.Append(string.Format("<td>{0}</td>", maximademanda != null ? (maximademanda.Value).ToString("N", nfi) : string.Empty));

                    //Potencia Total
                    decimal? potenciaTotal = null;
                    if (total != null)
                    {
                        potenciaTotal = (decimal?)total.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(total, null);
                    }
                    strHtml.Append(string.Format("<td>{0}</td>", potenciaTotal != null ? (potenciaTotal.Value).ToString("N", nfi) : string.Empty));

                    //Potencia x Empresa
                    foreach (var empresa in listaEmpresa)
                    {
                        int cantidad = 0;
                        MeMedicion48DTO reg = data.Find(x => x.Medifecha == day && x.Emprcodi == empresa.Emprcodi && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaMWEjec);
                        decimal? pot = null;
                        if (reg != null)
                        {
                            pot = (decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg, null);

                            decimal? repetido = 0;
                            for (int c = 1; c <= 48; c++)
                            {
                                repetido = (decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + c).GetValue(reg, null);
                                cantidad = repetido == pot && repetido != null && repetido != 0 ? cantidad + 1 : cantidad + 0;
                            }

                        }
                        string color = cantidad == 48 ? ConstantesMonitoreo.ColorLogError : string.Empty;
                        strHtml.Append(string.Format("<td style='background-color: {1};'>{0}</td>", pot != null && pot.GetValueOrDefault(0) != 0 ? (pot.Value).ToString("N", nfi) : string.Empty, color));
                    }

                    //IOP x Empresa
                    foreach (var empresa in listaEmpresa)
                    {
                        MeMedicion48DTO reg = data.Find(x => x.Medifecha == day && x.Emprcodi == empresa.Emprcodi && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaIOP);
                        int iop = ConstantesMonitoreo.ValorIOPEsPivotal;

                        if (reg != null)
                        {
                            iop = Convert.ToInt32(((decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg, null)).GetValueOrDefault(0));
                        }

                        string color = ConstantesMonitoreo.ValorIOPEsPivotal == iop ? colorEsPivotal : colorNoPivotal;
                        strHtml.AppendFormat("<td style='background-color: {1}; color:White;'>{0}</td>", iop, color);
                    }
                    strHtml.Append("</tr>");
                    horas = horas.AddMinutes(30);
                }
            }
            #endregion
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Html indicador 3
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public string LogErrorIndicador4Html(string empresas, DateTime fechaInicio, DateTime fechaFin, int vermmcodi, List<MmmDatoDTO> listFacTable)
        {
            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";

            List<SiEmpresaDTO> listaEmpresa;
            List<MeMedicion48DTO> data = this.ListarIndicador(vermmcodi, fechaInicio, fechaFin, ConstantesMonitoreo.CodigoIOP, listFacTable, ConstantesMonitoreo.LogErroresMonitoreo, out listaEmpresa);
            listaEmpresa = this.ListarEmpresasMonitoreoByData(listaEmpresa, empresas);

            SiParametroValorDTO param = this.GetParametroOfertaPivotal(fechaInicio);
            string colorEsPivotal = param.IOPEsPivotalColor;
            string colorNoPivotal = param.IOPNoPivotalColor;

            if (listaEmpresa.Count == 0) return string.Empty;

            int padding = 20;
            int anchoTotal = (100 + padding) + listaEmpresa.Count * (200 + padding);
            strHtml.Append("<div class='freeze_table' id='resultado3' style='height: auto;'>");
            strHtml.AppendFormat("<table id='reporte3' class='pretty tabla-icono' style='table-layout: fixed; width: {0}px;' >", anchoTotal);
            strHtml.Append("<thead>");
            #region cabecera
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width: 110px;' rowspan=2>MM/DD/hh:mm</th>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px' rowspan=2 > MD  (MW)</th>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px' rowspan=2> PES (MW) </th>");
            for (int i = 1; i <= 2; i++)
            {
                foreach (var datos in listaEmpresa)
                {
                    strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px'>" + datos.Emprnomb + "</th>");
                }
            }

            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            for (int i = 1; i <= 2; i++)
            {
                foreach (var datos in listaEmpresa)
                {
                    string valor; valor = (i == 1) ? "PE (MW)" : "IOP";
                    strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px'> " + valor + " </th>");
                }
            }
            strHtml.Append("</tr>");
            #endregion
            strHtml.Append("</thead>");
            strHtml.Append("<tbody>");
            #region cuerpo
            for (var day = fechaInicio.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
            {
                MeMedicion48DTO total = data.Find(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaTotalPotencia);
                MeMedicion48DTO md = data.Find(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaMaximaDemanda);

                DateTime horas = day.AddMinutes(30);
                for (int h = 1; h <= 48; h++)
                {
                    strHtml.Append("<tr>");
                    strHtml.Append(string.Format("<td class='tdbody_reporte'>{0:dd/MM/yyyy HH:mm}</td>", horas));

                    //Maxima Demanda Programada
                    decimal? maximademanda = null;
                    if (md != null)
                    {
                        maximademanda = (decimal?)md.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(md, null);
                    }
                    strHtml.Append(string.Format("<td>{0}</td>", maximademanda != null ? (maximademanda.Value).ToString("N", nfi) : string.Empty));

                    //Potencia Total
                    decimal? potenciaTotal = null;
                    if (total != null)
                    {
                        potenciaTotal = (decimal?)total.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(total, null);

                    }
                    strHtml.Append(string.Format("<td>{0}</td>", potenciaTotal != null ? (potenciaTotal.Value).ToString("N", nfi) : string.Empty));

                    //Potencia x Empresa
                    foreach (var empresa in listaEmpresa)
                    {
                        int cantidad = 0;
                        MeMedicion48DTO reg = data.Find(x => x.Medifecha == day && x.Emprcodi == empresa.Emprcodi && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaMWEjec);
                        decimal? pot = null;
                        if (reg != null)
                        {
                            pot = (decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg, null);

                            decimal? repetido = 0;
                            for (int c = 1; c <= 48; c++)
                            {
                                repetido = (decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + c).GetValue(reg, null);
                                cantidad = repetido == pot && repetido != null && repetido != 0 ? cantidad + 1 : cantidad + 0;
                            }

                        }
                        string color = cantidad == 48 ? ConstantesMonitoreo.ColorLogError : string.Empty;
                        strHtml.Append(string.Format("<td style='background-color: {1};'>{0}</td>", pot != null && pot.GetValueOrDefault(0) != 0 ? (pot.Value).ToString("N", nfi) : string.Empty, color));

                    }

                    //IOP x Empresa
                    foreach (var empresa in listaEmpresa)
                    {
                        MeMedicion48DTO reg = data.Find(x => x.Medifecha == day && x.Emprcodi == empresa.Emprcodi && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaIOP);
                        int iop = ConstantesMonitoreo.ValorIOPEsPivotal;

                        if (reg != null)
                        {
                            iop = Convert.ToInt32(((decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg, null)).GetValueOrDefault(0));
                        }

                        string color = ConstantesMonitoreo.ValorIOPEsPivotal == iop ? colorEsPivotal : colorNoPivotal;
                        strHtml.AppendFormat("<td style='background-color: {1}; color:White;'>{0}</td>", iop, color);
                    }
                    strHtml.Append("</tr>");
                    horas = horas.AddMinutes(30);
                }
            }
            #endregion
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        /// <summary>
        ///  Html indicador 5
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public string LogErrorIndicador5Html(string empresas, DateTime fechaInicio, DateTime fechaFin, int vermmcodi, List<MmmDatoDTO> listFacTable)
        {
            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";

            List<SiEmpresaDTO> listaEmpresa;
            List<MeMedicion48DTO> data = this.ListarIndicador(vermmcodi, fechaInicio, fechaFin, ConstantesMonitoreo.CodigoILE, listFacTable, ConstantesMonitoreo.LogErroresMonitoreo, out listaEmpresa);
            listaEmpresa = this.ListarEmpresasMonitoreoByData(listaEmpresa, empresas);

            List<MeMedicion48DTO> listaBarra = data.Where(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCMEjec)
                .Select(x => new { Emprcodi = x.Emprcodi, Barrcodi = x.Barrcodi, Barrnombre = x.Barrnombre })
                .GroupBy(x => new { x.Emprcodi, x.Barrcodi, x.Barrnombre })
                .Select(x => new MeMedicion48DTO() { Emprcodi = x.Key.Emprcodi, Barrcodi = x.Key.Barrcodi, Barrnombre = x.Key.Barrnombre })
                .OrderBy(x => x.Barrnombre).ToList();

            int totalBarra = listaBarra.Count();
            totalBarra = totalBarra + 1;

            if (listaEmpresa.Count == 0) return string.Empty;

            int padding = 20;
            int anchoCol = 100;
            int anchoTotal = (100 + padding) + totalBarra * 3 * (anchoCol + 50 + padding);
            strHtml.Append("<div class='freeze_table' id='resultado5' style='height: auto;'>");
            strHtml.Append("<div  id='resultado5' style='height: auto; width:auto;'>");
            strHtml.AppendFormat("<table id='reporte5' class='pretty tabla-icono' style='table-layout: fixed; width: {0}px;' >", anchoTotal);
            strHtml.Append("<thead>");
            #region cabecera
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width: 110px;' rowspan='3'> MM/DD/hh:mm </th>");

            //Empresas
            int contEmpresa = 1;
            foreach (var empresa in listaEmpresa)
            {
                int totalBarraXEmpr = listaBarra.Where(x => x.Emprcodi == empresa.Emprcodi).Count();
                totalBarraXEmpr = totalBarraXEmpr > 0 ? totalBarraXEmpr : 1;

                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal; width: {2}px;' colspan ='{1}' class='cmg_{3}'>{0}</th>"
                    , empresa.Emprnomb, 3 * totalBarraXEmpr, 3 * totalBarraXEmpr * anchoCol, contEmpresa % 2);
                contEmpresa++;
            }

            strHtml.Append("</tr>");
            strHtml.Append("<tr>");

            //Barra X Empresa
            contEmpresa = 1;
            foreach (var empresa in listaEmpresa)
            {
                var listaBarraXEmpr = listaBarra.Where(x => x.Emprcodi == empresa.Emprcodi).ToList();
                int totalBarraXEmpr = listaBarraXEmpr.Count();
                if (totalBarraXEmpr > 0)
                {
                    foreach (var barra in listaBarraXEmpr)
                    {
                        strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal; width: {2}px;'  colspan ='{1}' class='cmg_{3}'>{0}</th>"
                            , barra.Barrnombre, 3, anchoCol * 3, contEmpresa % 2);
                    }
                }
                else
                {
                    strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal; width: {2}px;'  colspan ='{1}' class='cmg_{3}'>{0}</th>"
                        , "", 3, anchoCol * 3, contEmpresa % 2);
                }
                contEmpresa++;
            }

            strHtml.Append("</tr>");
            strHtml.Append("<tr>");

            //Valores x Barra
            contEmpresa = 1;
            foreach (var empresa in listaEmpresa)
            {
                int totalBarraXEmpr = listaBarra.Where(x => x.Emprcodi == empresa.Emprcodi).Count();
                totalBarraXEmpr = totalBarraXEmpr > 0 ? totalBarraXEmpr : 1;
                for (int j = 0; j < totalBarraXEmpr; j++)
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        string valor; valor = (i == 1) ? " CMg <br> (k,m,t)" : (i == 2) ? " CV <br> (S/K wh)" : "ILE  <br>  (Índice de Lerner)";
                        strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal; width: {1}px;'  class='cmg_{2}'>{0}</th>"
                            , valor, anchoCol, contEmpresa % 2);
                    }
                }
                contEmpresa++;
            }

            strHtml.Append("</tr>");
            #endregion
            strHtml.Append("</thead>");
            #region cuerpo
            strHtml.Append("<tbody>");

            for (var day = fechaInicio.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
            {
                List<MeMedicion48DTO> dataCMEjec = data.Where(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCMEjec).OrderBy(x => x.Emprnomb).ThenBy(x => x.Barrnombre).ToList();
                List<MeMedicion48DTO> dataCV = data.Where(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCV).ToList();
                List<MeMedicion48DTO> dataILE = data.Where(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaILE).ToList();

                DateTime horas = day.AddMinutes(30);
                for (int h = 1; h <= 48; h++)
                {
                    strHtml.Append("<tr>");

                    strHtml.Append(string.Format("<td>{0:dd/MM/yyyy HH:mm}</td>", horas));

                    foreach (var empresa in listaEmpresa)
                    {
                        var listaBarraXEmpr = listaBarra.Where(x => x.Emprcodi == empresa.Emprcodi).ToList();
                        int totalBarraXEmpr = listaBarraXEmpr.Count();
                        if (totalBarraXEmpr > 0)
                        {
                            foreach (var barra in listaBarraXEmpr)
                            {
                                int cantidadRepetidasMarginal = 0;

                                if (barra.Barrcodi == 334)
                                {
                                }

                                MeMedicion48DTO objcmg = dataCMEjec.Find(x => x.Medifecha == day && x.Barrcodi == barra.Barrcodi && x.Emprcodi == barra.Emprcodi);
                                MeMedicion48DTO objcv = dataCV.Find(x => x.Medifecha == day && x.Barrcodi == barra.Barrcodi && x.Emprcodi == barra.Emprcodi);
                                MeMedicion48DTO objile = dataILE.Find(x => x.Medifecha == day && x.Barrcodi == barra.Barrcodi && x.Emprcodi == barra.Emprcodi);

                                decimal? valorCMg = null;
                                decimal? valorCV = null;
                                decimal? valorIle = null;
                                bool tieneGeneracion = false;

                                if (objcmg != null)
                                {
                                    valorCMg = (decimal?)objcmg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(objcmg, null);


                                    decimal? repetido = 0;
                                    for (int c = 1; c <= 48; c++)
                                    {
                                        repetido = (decimal?)objcmg.GetType().GetProperty(ConstantesAppServicio.CaracterH + c).GetValue(objcmg, null);
                                        cantidadRepetidasMarginal = repetido == valorCMg && repetido != null && repetido != 0 ? cantidadRepetidasMarginal + 1 : cantidadRepetidasMarginal + 0;
                                    }

                                }
                                if (objcv != null)
                                {
                                    valorCV = (decimal?)objcv.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(objcv, null);
                                }

                                if (objile != null)
                                {
                                    valorIle = (decimal?)objile.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(objile, null);
                                    tieneGeneracion = objile.TieneIndicador;
                                }



                                //strHtml.AppendFormat("<td>{0}</td>", valorCMg != null ? (valorCMg.Value).ToString("N", nfi) : string.Empty);
                                //strHtml.AppendFormat("<td>{0}</td>", valorCV != null ? (valorCV.Value).ToString("N", nfi) : string.Empty);
                                //strHtml.AppendFormat("<td>{0}</td>", !tieneGeneracion ? string.Empty : valorIle != null ? (valorIle.Value).ToString("N", nfi) : "(*)");

                                string colorLogMargial = cantidadRepetidasMarginal == 48 ? ConstantesMonitoreo.ColorLogError : string.Empty;
                                strHtml.Append(string.Format("<td style='background-color: {1};'>{0}</td>", valorCMg != null ? (valorCMg.Value).ToString("N", nfi) : string.Empty, colorLogMargial));
                                strHtml.AppendFormat("<td>{0}</td>", valorCV != null ? (valorCV.Value).ToString("N", nfi) : string.Empty);
                                strHtml.AppendFormat("<td>{0}</td>", !tieneGeneracion ? string.Empty : valorIle != null ? (valorIle.Value).ToString("N", nfi) : "(*)");


                            }
                        }
                        else
                        {
                            strHtml.Append("<td></td>");
                            strHtml.Append("<td></td>");
                            strHtml.Append("<td></td>");
                        }
                    }
                    strHtml.Append("</tr>");
                    horas = horas.AddMinutes(30);
                }
            }

            #endregion
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        /// <summary>
        ///  Html indicador 6
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public string LogErrorIndicador6Html(string empresas, DateTime fechaInicio, DateTime fechaFin, int vermmcodi, List<MmmDatoDTO> listFacTable)
        {
            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";

            List<SiEmpresaDTO> listaEmpresa;
            List<MeMedicion48DTO> data = this.ListarIndicador(vermmcodi, fechaInicio, fechaFin, ConstantesMonitoreo.CodigoIMU, listFacTable, ConstantesMonitoreo.LogErroresMonitoreo, out listaEmpresa);
            listaEmpresa = this.ListarEmpresasMonitoreoByData(listaEmpresa, empresas);

            List<MeMedicion48DTO> listaBarra = data.Where(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCMEjec)
                .Select(x => new { Emprcodi = x.Emprcodi, Barrcodi = x.Barrcodi, Barrnombre = x.Barrnombre })
                .GroupBy(x => new { x.Emprcodi, x.Barrcodi, x.Barrnombre })
                .Select(x => new MeMedicion48DTO() { Emprcodi = x.Key.Emprcodi, Barrcodi = x.Key.Barrcodi, Barrnombre = x.Key.Barrnombre })
                .OrderBy(x => x.Barrnombre).ToList();

            int totalBarra = listaBarra.Count();
            totalBarra = totalBarra + 1;

            if (listaEmpresa.Count == 0) return string.Empty;

            int padding = 20;
            int anchoCol = 100;
            int anchoTotal = (100 + padding) + totalBarra * 3 * (anchoCol + 50 + padding);
            strHtml.Append("<div class='freeze_table' id='resultado6' style='height: auto;'>");
            strHtml.Append("<div  id='resultado6' style='height: auto; width:auto;'>");
            strHtml.AppendFormat("<table id='reporte6' class='pretty tabla-icono' style='table-layout: fixed; width: {0}px;' >", anchoTotal);
            strHtml.Append("<thead>");
            #region cabecera
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width: 110px;' rowspan='3'> MM/DD/hh:mm </th>");

            //Empresas
            int contEmpresa = 1;
            foreach (var empresa in listaEmpresa)
            {
                int totalBarraXEmpr = listaBarra.Where(x => x.Emprcodi == empresa.Emprcodi).Count();
                totalBarraXEmpr = totalBarraXEmpr > 0 ? totalBarraXEmpr : 1;

                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal; width: {2}px;' colspan ='{1}' class='cmg_{3}'>{0}</th>"
                    , empresa.Emprnomb, 3 * totalBarraXEmpr, 3 * totalBarraXEmpr * anchoCol, contEmpresa % 2);
                contEmpresa++;
            }

            strHtml.Append("</tr>");
            strHtml.Append("<tr>");

            //Barra X Empresa
            contEmpresa = 1;
            foreach (var empresa in listaEmpresa)
            {
                var listaBarraXEmpr = listaBarra.Where(x => x.Emprcodi == empresa.Emprcodi).ToList();
                int totalBarraXEmpr = listaBarraXEmpr.Count();
                if (totalBarraXEmpr > 0)
                {
                    foreach (var barra in listaBarraXEmpr)
                    {
                        strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal; width: {2}px;'  colspan ='{1}' class='cmg_{3}'>{0}</th>"
                            , barra.Barrnombre, 3, anchoCol * 3, contEmpresa % 2);
                    }
                }
                else
                {
                    strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal; width: {2}px;'  colspan ='{1}' class='cmg_{3}'>{0}</th>"
                        , "", 3, anchoCol * 3, contEmpresa % 2);
                }
                contEmpresa++;
            }

            strHtml.Append("</tr>");
            strHtml.Append("<tr>");

            //Valores x Barra
            contEmpresa = 1;
            foreach (var empresa in listaEmpresa)
            {
                int totalBarraXEmpr = listaBarra.Where(x => x.Emprcodi == empresa.Emprcodi).Count();
                totalBarraXEmpr = totalBarraXEmpr > 0 ? totalBarraXEmpr : 1;
                for (int j = 0; j < totalBarraXEmpr; j++)
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        string valor; valor = (i == 1) ? " CMg <br> (k,m,t)" : (i == 2) ? " CV <br> (S/K wh)" : "IMU  <br>  (Índice de Mark Up )";
                        strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal; width: {1}px;' class='cmg_{2}'>{0}</th>"
                            , valor, anchoCol, contEmpresa % 2);
                    }
                }
                contEmpresa++;
            }

            strHtml.Append("</tr>");
            #endregion
            strHtml.Append("</thead>");
            #region cuerpo
            strHtml.Append("<tbody>");

            for (var day = fechaInicio.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
            {
                List<MeMedicion48DTO> dataCMEjec = data.Where(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCMEjec).OrderBy(x => x.Emprnomb).ThenBy(x => x.Barrnombre).ToList();
                List<MeMedicion48DTO> dataCV = data.Where(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCV).ToList();
                List<MeMedicion48DTO> dataIMU = data.Where(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaIMU).ToList();

                DateTime horas = day.AddMinutes(30);
                for (int h = 1; h <= 48; h++)
                {
                    strHtml.Append("<tr>");

                    strHtml.Append(string.Format("<td>{0:dd/MM/yyyy HH:mm}</td>", horas));

                    foreach (var empresa in listaEmpresa)
                    {
                        var listaBarraXEmpr = listaBarra.Where(x => x.Emprcodi == empresa.Emprcodi).ToList();
                        int totalBarraXEmpr = listaBarraXEmpr.Count();
                        if (totalBarraXEmpr > 0)
                        {
                            foreach (var barra in listaBarraXEmpr)
                            {
                                int cantidadRepetidasMarginal = 0;

                                if (barra.Barrcodi == 642)
                                {
                                }

                                MeMedicion48DTO objcmg = dataCMEjec.Find(x => x.Medifecha == day && x.Barrcodi == barra.Barrcodi && x.Emprcodi == barra.Emprcodi);
                                MeMedicion48DTO objcv = dataCV.Find(x => x.Medifecha == day && x.Barrcodi == barra.Barrcodi && x.Emprcodi == barra.Emprcodi);
                                MeMedicion48DTO objimu = dataIMU.Find(x => x.Medifecha == day && x.Barrcodi == barra.Barrcodi && x.Emprcodi == barra.Emprcodi);

                                decimal? valorCMg = null;
                                decimal? valorCV = null;
                                decimal? valorImu = null;
                                bool tieneGeneracion = false;

                                if (objcmg != null)
                                {
                                    valorCMg = (decimal?)objcmg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(objcmg, null);

                                    decimal? repetido = 0;
                                    for (int c = 1; c <= 48; c++)
                                    {
                                        repetido = (decimal?)objcmg.GetType().GetProperty(ConstantesAppServicio.CaracterH + c).GetValue(objcmg, null);
                                        cantidadRepetidasMarginal = repetido == valorCMg && repetido != null && repetido != 0 ? cantidadRepetidasMarginal + 1 : cantidadRepetidasMarginal + 0;
                                    }
                                }

                                if (objcv != null)
                                {
                                    valorCV = (decimal?)objcv.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(objcv, null);
                                }

                                if (objimu != null)
                                {
                                    valorImu = (decimal?)objimu.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(objimu, null);
                                    tieneGeneracion = objimu.TieneIndicador;
                                }
                                //trHtml.Append(string.Format("<td>{0}</td>", valorCMg != null ? (valorCMg.Value).ToString("N", nfi) : string.Empty));
                                string colorLogMargial = cantidadRepetidasMarginal == 48 ? ConstantesMonitoreo.ColorLogError : string.Empty;
                                strHtml.Append(string.Format("<td style='background-color: {1};'>{0}</td>", valorCMg != null ? (valorCMg.Value).ToString("N", nfi) : string.Empty, colorLogMargial));

                                strHtml.Append(string.Format("<td>{0}</td>", valorCV != null ? (valorCV.Value).ToString("N", nfi) : string.Empty));
                                strHtml.Append(string.Format("<td>{0}</td>", !tieneGeneracion ? string.Empty : valorImu != null ? (valorImu.Value).ToString("N", nfi) : "(*)"));
                            }
                        }
                        else
                        {
                            strHtml.Append("<td></td>");
                            strHtml.Append("<td></td>");
                            strHtml.Append("<td></td>");
                        }
                    }
                    strHtml.Append("</tr>");
                    horas = horas.AddMinutes(30);
                }
            }

            #endregion
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Html indicador 7
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="fechaHist"></param>
        /// <param name="generacion"></param>
        /// <returns></returns>
        public string LogErrorIndicador7Html(string empresas, DateTime fechaInicio, DateTime fechaFin, int vermmcodi, List<MmmDatoDTO> listFacTable)
        {
            StringBuilder strHtml = new StringBuilder();

            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";

            List<SiEmpresaDTO> listaEmpresa;
            List<MeMedicion48DTO> data = this.ListarIndicador(vermmcodi, fechaInicio, fechaFin, ConstantesMonitoreo.CodigoIRT, listFacTable, ConstantesMonitoreo.LogErroresMonitoreo, out listaEmpresa);
            listaEmpresa = this.ListarEmpresasMonitoreoByData(listaEmpresa, empresas);

            List<int> listaEmprcodi = listaEmpresa.Select(x => x.Emprcodi).ToList();

            //Congestiones
            List<MeMedicion48DTO> listaCongXAreaXFecha = data.Where(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCongestion
                && listaEmprcodi.Contains(x.Emprcodi)).ToList();

            //Grupos despacho
            List<MeMedicion48DTO> listaBarraEmpGrupo = data.Where(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaEnlaceTrans
                && listaEmprcodi.Contains(x.Emprcodi)).ToList();

            if (listaBarraEmpGrupo.Count == 0)
            {
                listaBarraEmpGrupo.Add(new MeMedicion48DTO() { Grupocodi = -1, Gruponomb = string.Empty, Grupopadre = -2, Central = string.Empty, Barrcodi = -1, Barrnombre = string.Empty });
            }

            #region Lista de Congestiones

            strHtml.Append("<div class='freeze_table' id='resultadocong7' style='height: auto;'>");
            strHtml.AppendFormat("<table id='reportecong7' class='pretty tabla-icono' style='table-layout: fixed; width: {0}px;' >", 800);
            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 80px'>" + "FECHA" + "</th>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 80px'>" + "INICIO" + "</th>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 80px'>" + "FINAL" + "</th>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px'>" + "UBICACIÓN" + "</th>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px'>" + "EQUIPO" + "</th>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 150px'>" + "OBSERVACIONES" + "</th>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 200px'>" + "GENERACIÓN CON UNA LOCALIZACIÓN ESPECÍFICA, QUE OBTENGAN PODER DE MERCADO" + "</th>");
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");

            strHtml.Append("<tbody>");

            foreach (var reg in listaCongXAreaXFecha)
            {
                strHtml.Append("<tr>");
                strHtml.AppendFormat("<td>{0}</td>", reg.Hophorini.ToString(ConstantesAppServicio.FormatoFecha));
                strHtml.AppendFormat("<td>{0}</td>", reg.Hophorini.ToString(ConstantesAppServicio.FormatoOnlyHora));
                strHtml.AppendFormat("<td>{0}</td>", reg.Hophorfin.ToString(ConstantesAppServicio.FormatoOnlyHora));
                strHtml.AppendFormat("<td>{0}</td>", reg.Areanomb != null ? (reg.Areanomb) : string.Empty);
                strHtml.AppendFormat("<td>{0}</td>", reg.Equinomb);
                strHtml.AppendFormat("<td>{0}</td>", reg.Descripcion);
                strHtml.AppendFormat("<td>{0}</td>", reg.Gruponomb);
                strHtml.Append("</tr>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            strHtml.Append("</div>");

            #endregion

            strHtml.Append("<br/>");
            strHtml.Append("<br/>");

            #region Reporte
            int padding = 20;
            int columnas = listaBarraEmpGrupo.Count * 5;
            int anchoTotalCong = (100 + padding) + (columnas) * (150 + padding);

            strHtml.Append("<div class='freeze_table' id='resultado7' style='height: auto;'>");

            strHtml.AppendFormat("<table id='reporte7' class='pretty tabla-icono' style='table-layout: fixed; width: {0}px;' >", anchoTotalCong);
            strHtml.Append("<thead>");
            #region Cabecera
            strHtml.Append("<tr>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px' rowspan=4>" + "MM/DD/hh:mm" + "</th>");

            //Enlace de transmision
            foreach (var grupo in listaBarraEmpGrupo)
            {
                strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 500px' colspan='4'>Enlace de transmisión</th>");
                strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px' rowspan='4'>" + "IRT" + "</th>");
            }

            strHtml.Append("</tr>");

            //Lineas, trafos
            strHtml.Append("<tr>");
            foreach (var grupo in listaBarraEmpGrupo)
            {
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 500px' colspan='4'>{0}</th>", grupo.Equinomb);
            }
            strHtml.Append("</tr>");

            //Grupo, Barra
            strHtml.Append("<tr>");

            foreach (var grupo in listaBarraEmpGrupo)
            {
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 100px'>{0}</th>", grupo.Gruponomb);
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 100px'>{0}</th>", grupo.Gruponomb);
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 100px'>{0}</th>", grupo.Barrnombre);
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 100px'>{0}</th>", grupo.Barrnombre);
            }

            strHtml.Append("</tr>");

            //Datos
            strHtml.Append("<tr>");
            foreach (var grupo in listaBarraEmpGrupo)
            {
                strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px'> PU (MW)</th>");
                strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px'> PP (MW)</th>");
                strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px'> CMg (S/ MWh)</th>");
                strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px'> CMgprog (S/ MWh)</th>");

            }
            strHtml.Append("</tr>");
            #endregion
            strHtml.Append("</thead>");
            strHtml.Append("<tbody>");
            #region Cuerpo
            for (var day = fechaInicio.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
            {
                List<MeMedicion48DTO> dataXDia = data.Where(x => x.Medifecha == day).ToList();

                DateTime horas = day.AddMinutes(30);

                for (int h = 1; h <= 48; h++)
                {
                    strHtml.Append("<tr>");
                    strHtml.Append(string.Format("<td class='tdbody_reporte'>{0:dd/MM/yyyy HH:mm}</td>", horas));

                    foreach (var grupo in listaBarraEmpGrupo)
                    {
                        int cantidadLogMwEjec = 0;
                        int cantidadLogMwProg = 0;
                        int cantidadCosmarEjec = 0;
                        int cantidadCosmarProg = 0;

                        MeMedicion48DTO regIRT = dataXDia.Find(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaIRT && x.Grupocodi == grupo.Grupocodi);
                        if (regIRT != null)
                        {
                            decimal? irt = (decimal?)regIRT.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regIRT, null);
                            if (irt != null)
                            {
                                //Potencia ejecutada
                                MeMedicion48DTO regpotEjec = this.GetPotenciaTotalXDiaByGrupoReporte7(day, dataXDia.Where(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaPotGrupoEjec).ToList(), grupo.Grupocodi);
                                decimal? potEjec = null;
                                if (regpotEjec != null)
                                {
                                    potEjec = (decimal?)regpotEjec.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regpotEjec, null);

                                    decimal? repetido = 0;
                                    for (int c = 1; c <= 48; c++)
                                    {
                                        repetido = (decimal?)regpotEjec.GetType().GetProperty(ConstantesAppServicio.CaracterH + c).GetValue(regpotEjec, null);
                                        cantidadLogMwEjec = repetido == potEjec && repetido != null && repetido != 0 ? cantidadLogMwEjec + 1 : cantidadLogMwEjec + 0;
                                    }

                                }

                                string colorLogMargial = cantidadLogMwEjec == 48 ? ConstantesMonitoreo.ColorLogError : string.Empty;
                                strHtml.Append(string.Format("<td style='background-color: {1};'>{0}</td>", potEjec != null ? (potEjec.Value).ToString("N", nfi) : string.Empty, colorLogMargial));

                                //Potencia programada
                                MeMedicion48DTO regpotProg = this.GetPotenciaTotalXDiaByGrupoReporte7(day, dataXDia.Where(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaPotGrupoProg).ToList(), grupo.Grupocodi);
                                decimal? potProg = null;
                                if (regpotProg != null)
                                {
                                    potProg = (decimal?)regpotProg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regpotProg, null);

                                    decimal? repetido = 0;
                                    for (int c = 1; c <= 48; c++)
                                    {
                                        repetido = (decimal?)regpotProg.GetType().GetProperty(ConstantesAppServicio.CaracterH + c).GetValue(regpotProg, null);
                                        cantidadLogMwProg = repetido == potProg && repetido != null && repetido != 0 ? cantidadLogMwProg + 1 : cantidadLogMwProg + 0;
                                    }
                                }
                                string colorMwProg = cantidadLogMwProg == 48 ? ConstantesMonitoreo.ColorLogError : string.Empty;
                                strHtml.Append(string.Format("<td style='background-color: {1}; color:black;'>{0}</td>", potProg != null ? (potProg.Value).ToString("N", nfi) : string.Empty, colorMwProg));

                                //Costos marg Ejecutados
                                MeMedicion48DTO regCMgejec = dataXDia.Find(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCMEjec && x.Barrcodi == grupo.Barrcodi && x.Emprcodi == grupo.Emprcodi);
                                decimal? cmg = null;
                                if (regCMgejec != null && potEjec.GetValueOrDefault(0) != 0)
                                {
                                    cmg = (decimal?)regCMgejec.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regCMgejec, null);
                                    if (cmg != null)
                                        cmg = cmg * 1000;

                                    decimal? repetido = 0;
                                    for (int c = 1; c <= 48; c++)
                                    {
                                        repetido = (decimal?)regCMgejec.GetType().GetProperty(ConstantesAppServicio.CaracterH + c).GetValue(regCMgejec, null);
                                        cantidadCosmarEjec = repetido == potProg && repetido != null && repetido != 0 ? cantidadCosmarEjec + 1 : cantidadCosmarEjec + 0;
                                    }
                                }
                                strHtml.Append(string.Format("<td>{0}</td>", cmg.GetValueOrDefault(0) != 0 ? (cmg.Value).ToString("N", nfi) : string.Empty));

                                //Costos marg Programados
                                MeMedicion48DTO regCMgprog = dataXDia.Find(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCMProg && x.Cnfbarcodi == grupo.Cnfbarcodi && x.Emprcodi == grupo.Emprcodi);
                                decimal? cmgProg = null;
                                if (regCMgprog != null && potProg.GetValueOrDefault(0) != 0)
                                {
                                    cmgProg = (decimal?)regCMgprog.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regCMgprog, null);

                                    decimal? repetido = 0;
                                    for (int c = 1; c <= 48; c++)
                                    {
                                        repetido = (decimal?)regCMgprog.GetType().GetProperty(ConstantesAppServicio.CaracterH + c).GetValue(regCMgprog, null);
                                        cantidadCosmarProg = repetido == potProg && repetido != null && repetido != 0 ? cantidadCosmarProg + 1 : cantidadCosmarProg + 0;
                                    }
                                }

                                string colorLogCostoMargianlEjec = cantidadCosmarEjec == 48 ? ConstantesMonitoreo.ColorLogError : string.Empty;
                                strHtml.Append(string.Format("<td style='background-color: {1}; color:black;'>{0}</td>", cmgProg != null ? (cmgProg.Value).ToString("N", nfi) : string.Empty, colorLogCostoMargianlEjec));

                                string colorLogCostoMargianlProg = cantidadCosmarProg == 48 ? ConstantesMonitoreo.ColorLogError : string.Empty;
                                strHtml.Append(string.Format("<td style='background-color: {1}; color:black;'>{0}</td>", irt != null ? (irt.Value).ToString("N", nfi) : string.Empty, colorLogCostoMargianlProg));

                            }
                            else
                            {
                                strHtml.Append("<td></td>");
                                strHtml.Append("<td></td>");
                                strHtml.Append("<td></td>");
                                strHtml.Append("<td></td>");
                                strHtml.Append("<td></td>");
                            }
                        }
                        else
                        {
                            strHtml.Append("<td></td>");
                            strHtml.Append("<td></td>");
                            strHtml.Append("<td></td>");
                            strHtml.Append("<td></td>");
                            strHtml.Append("<td></td>");
                        }
                    }

                    strHtml.Append("</tr>");
                    horas = horas.AddMinutes(30);
                }
            }
            #endregion
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            strHtml.Append("</div>");
            #endregion

            return strHtml.ToString();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla MMM_VERSION corespondiente a Log de Errores
        /// </summary>
        public List<MmmVersionDTO> ListMmmVersionComboLogErrores(DateTime fecha)
        {
            List<MmmVersionDTO> l = FactorySic.GetMmmVersionRepository().List();
            List<MmmVersionDTO> lista = l.Where(x => x.Vermmfechaperiodo == fecha && x.Vermmporcentaje == 100).OrderByDescending(x => x.Vermmcodi).ToList();
            List<MmmVersionDTO> listaFinal = new List<MmmVersionDTO>();

            foreach (var recorridoVersion in lista)
            {
                listaFinal.Add(recorridoVersion);
            }

            return listaFinal;
        }

        #endregion

        #region Control de cambios Monitoreo

        /// <summary>
        /// Lista Control de cambiosa indicador 1 2 3 4 
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="vermmcodi"></param>
        /// <param name="listFacTable"></param>
        /// <returns></returns>
        public List<ReporteControlCambios> ListaControlCambios(string empresas, DateTime fechaInicio, DateTime fechaFin, int vermmcodi, List<MmmDatoDTO> listFacTable)
        {
            //Lista de empresas
            List<int> listaEmprcodiByData = listFacTable.Select(x => x.Emprcodi).Distinct().ToList();
            List<SiEmpresaDTO> listaEmpresa = this.ListarEmpresasMonitoreo(fechaInicio, empresas);
            listaEmpresa = listaEmpresa.Where(x => listaEmprcodiByData.Contains(x.Emprcodi)).ToList();

            List<MmmVersionDTO> listVersion = this.GetByCriteriaMmmVersions();
            var objVersionActual = listVersion.Find(x => x.Vermmcodi == vermmcodi && x.Vermmfechaperiodo.Date == fechaInicio);
            var listVersionPeriodo = listVersion.Where(x => x.Vermmfechaperiodo.Date == fechaInicio && x.Vermmporcentaje == 100).ToList();
            int version = 0;
            ///Lista medicion48 Version Actual
            version = this.VersionAnterior(objVersionActual.Vermmcodi, listVersionPeriodo);

            //Objeto version anterior
            var objVersionAnterior = listVersion.Find(x => x.Vermmcodi == version && x.Vermmfechaperiodo.Date == fechaInicio);
            //Lista cambio version Anterior
            List<MmmCambioversionDTO> listCambioVersionAnterior = GetByCriteriaMmmCambioversions(version);
            //Lista  de  cambios de version monitoreo
            List<MmmDatoDTO> listDatosFinales = ListarDatosCambios(listCambioVersionAnterior, listFacTable);

            List<ReporteControlCambios> listControlCambio = new List<ReporteControlCambios>();

            var listaData = listDatosFinales.Select(x => new { x.Mmmdatfecha, x.Mmmdathoraindex, x.Emprcodi }).Distinct().ToList();

            //Obtiene lista de fecha del periodo
            List<DateTime> listaFechas = listDatosFinales.Select(x => x.Mmmdatfecha.Date).Distinct().ToList();

            //Obtiene fecha Inicial cambio
            var fechaInicioCambio = listaFechas.OrderBy(x => x.Date).FirstOrDefault();
            //Obtiene fecha Fin cambio
            var fechaFinCambio = listaFechas.OrderByDescending(x => x.Date).FirstOrDefault();
            //Lista Data version  actual de los dias  del periodo con cambios
            List<MeMedicion48DTO> dataActual = new List<MeMedicion48DTO>();
            //Lista Data version Anterior de los dias  del periodo con cambios
            List<MeMedicion48DTO> dataAnterior = new List<MeMedicion48DTO>();

            //Lista Data version  actual de los dias  del periodo con cambios
            List<MeMedicion48DTO> dataActualMwProg = new List<MeMedicion48DTO>();
            //Lista Data version Anterior de los dias  del periodo con cambios
            List<MeMedicion48DTO> dataAnteriorMwProg = new List<MeMedicion48DTO>();

            List<SiEmpresaDTO> listaEmpresaAux;

            //Console.WriteLine("Solo la hora: {0}\n", DateTime.Now.ToString("h:mm:ss"));

            List<MeMedicion48DTO> listaDataActual = this.ListarIndicador(vermmcodi, fechaInicio.Date, fechaFin.Date, ConstantesMonitoreo.CodigoS, listFacTable, ConstantesMonitoreo.ControlCambiosMonitoreo, out listaEmpresaAux);
            List<MeMedicion48DTO> listaDataAnterior = this.ListarIndicador(version, fechaInicio.Date, fechaFin.Date, ConstantesMonitoreo.CodigoS, listFacTable, ConstantesMonitoreo.ControlCambiosMonitoreo, out listaEmpresaAux);
            List<MeMedicion48DTO> listaDataActualMwProg = this.ListarIndicador(vermmcodi, fechaInicio.Date, fechaFin.Date, ConstantesMonitoreo.TipoFormulaMWProg, listFacTable, ConstantesMonitoreo.ControlCambiosMonitoreo, out listaEmpresaAux);
            List<MeMedicion48DTO> listaDataAnteriorMwProg = this.ListarIndicador(version, fechaInicio.Date, fechaFin.Date, ConstantesMonitoreo.TipoFormulaMWProg, listFacTable, ConstantesMonitoreo.ControlCambiosMonitoreo, out listaEmpresaAux);

            if (listaFechas.Count != 0)
            {
                DateTime? fehcaInicio = fechaInicioCambio.Date;
                DateTime? fehcaFin = fechaFinCambio.Date;

                foreach (var recorridoFecha in listaFechas)
                {
                    dataActual.AddRange(listaDataActual.Where(x => x.Medifecha == recorridoFecha));
                    dataAnterior.AddRange(listaDataAnterior.Where(x => x.Medifecha == recorridoFecha));

                    dataActualMwProg.AddRange(listaDataActualMwProg.Where(x => x.Medifecha == recorridoFecha));
                    dataAnteriorMwProg.AddRange(listaDataAnteriorMwProg.Where(x => x.Medifecha == recorridoFecha));
                }
                //Mw Programado Versión Actual y Anterior 
                //Console.WriteLine("Solo la hora: {0}\n", DateTime.Now.ToString("h:mm:ss"));

                foreach (var recorridoV in listaData)
                {
                    ReporteControlCambios regControlCambio = new ReporteControlCambios();

                    MeMedicion48DTO regActual = dataActual.Find(x => x.Medifecha.Date == recorridoV.Mmmdatfecha.Date && x.Emprcodi == recorridoV.Emprcodi && x.TipoFormulaMonitoreo == ConstantesMonitoreo.CodigoS);

                    MeMedicion48DTO regAnterior = dataAnterior.Find(x => x.Medifecha.Date == recorridoV.Mmmdatfecha.Date && x.Emprcodi == recorridoV.Emprcodi && x.TipoFormulaMonitoreo == ConstantesMonitoreo.CodigoS);

                    MeMedicion48DTO regActualMwProg = dataActualMwProg.Find(x => x.Medifecha.Date == recorridoV.Mmmdatfecha.Date && x.Emprcodi == recorridoV.Emprcodi && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaMWProg);
                    MeMedicion48DTO regAnteriorMwProg = dataAnteriorMwProg.Find(x => x.Medifecha.Date == recorridoV.Mmmdatfecha.Date && x.Emprcodi == recorridoV.Emprcodi && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaMWProg);

                    decimal? MwEjecutadoActual = regActual != null ? (decimal?)regActual.GetType().GetProperty(ConstantesAppServicio.CaracterH + recorridoV.Mmmdathoraindex).GetValue(regActual, null) : null;
                    decimal? MwEjecutadoAnterior = regAnterior != null ? (decimal?)regAnterior.GetType().GetProperty(ConstantesAppServicio.CaracterH + recorridoV.Mmmdathoraindex).GetValue(regAnterior, null) : null;

                    decimal? MwProgActual = regActualMwProg != null ? (decimal?)regActualMwProg.GetType().GetProperty(ConstantesAppServicio.CaracterH + recorridoV.Mmmdathoraindex).GetValue(regActualMwProg, null) : null;
                    decimal? MwProgAnterior = regAnteriorMwProg != null ? (decimal?)regAnteriorMwProg.GetType().GetProperty(ConstantesAppServicio.CaracterH + recorridoV.Mmmdathoraindex).GetValue(regAnteriorMwProg, null) : null;

                    if (MwEjecutadoActual != MwEjecutadoAnterior || MwProgActual != MwProgAnterior)
                    {
                        var emprnomb = listaEmpresa.Find(x => x.Emprcodi == recorridoV.Emprcodi);

                        regControlCambio.Emprnomb = emprnomb.Emprnomb;
                        regControlCambio.Fecha = recorridoV.Mmmdatfecha;
                        regControlCambio.MWEjecActual = MwEjecutadoActual != MwEjecutadoAnterior ? MwEjecutadoActual : null;
                        regControlCambio.MWEjecAnt = MwEjecutadoActual != MwEjecutadoAnterior ? MwEjecutadoAnterior : null;
                        regControlCambio.MWProgActual = MwProgActual != MwProgAnterior ? MwProgActual : null;
                        regControlCambio.MWProgAnt = MwProgActual != MwProgAnterior ? MwProgAnterior : null;
                        regControlCambio.FechaEnvioAnt = objVersionAnterior.Vermmfeccreacion;
                        regControlCambio.FechaEnvioActual = objVersionActual.Vermmfeccreacion;
                        regControlCambio.UsuarioCreacionAnterior = objVersionAnterior.Vermmusucreacion;
                        regControlCambio.UsuarioCreacionActual = objVersionActual.Vermmusucreacion;

                        listControlCambio.Add(regControlCambio);
                    }
                }
            }
            var listControl = listControlCambio.OrderBy(x => x.Fecha).ToList();
            return listControl;

        }

        /// <summary>
        /// Lista Control de cambios Indicador 5 y 6
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="vermmcodi"></param>
        /// <param name="listFacTable"></param>
        /// <param name="barras"></param>
        /// <returns></returns>
        public List<ReporteControlCambios> ListaControlCambiosIndicadorLernerCosto(string empresas, DateTime fechaInicio, DateTime fechaFin, int vermmcodi, List<MmmDatoDTO> listFacTable, string barras)
        {
            //Lista de empresas
            List<int> listaEmprcodiByData = listFacTable.Select(x => x.Emprcodi).Distinct().ToList();
            List<SiEmpresaDTO> listaEmpresa = this.ListarEmpresasMonitoreo(fechaInicio, empresas);
            listaEmpresa = listaEmpresa.Where(x => listaEmprcodiByData.Contains(x.Emprcodi)).ToList();

            List<MmmVersionDTO> listVersion = this.GetByCriteriaMmmVersions();
            var objVersionActual = listVersion.Find(x => x.Vermmcodi == vermmcodi && x.Vermmfechaperiodo.Date == fechaInicio);
            var listVersionPeriodo = listVersion.Where(x => x.Vermmfechaperiodo.Date == fechaInicio && x.Vermmporcentaje == 100).ToList();
            int version = 0;
            int[] barralistado = new int[barras.Length];
            barralistado = barras.Split(',').Select(x => int.Parse(x)).ToArray();
            List<BarraDTO> listaBarra = ListarGrupoBarra();
            List<BarraDTO> listaBarraRecorrido = listaBarra.Where(x => barralistado.Contains(x.BarrCodi)).ToList();
            ///Lista medicion48 Version Actual
            version = this.VersionAnterior(objVersionActual.Vermmcodi, listVersionPeriodo);

            var objVersionAnterior = listVersion.Find(x => x.Vermmcodi == version && x.Vermmfechaperiodo.Date == fechaInicio);

            List<MmmCambioversionDTO> listCambioVersionAnterior = GetByCriteriaMmmCambioversions(version);

            List<MmmDatoDTO> listDatosFinales = ListarDatosCambios(listCambioVersionAnterior, listFacTable);

            var listaData = listDatosFinales.Select(x => new { x.Mmmdatfecha, x.Mmmdathoraindex, x.Emprcodi, x.Barrcodi }).Distinct().ToList();
            //  var listaData = listDatosFinales.Select(x => new { x.Mmmdatfecha, x.Mmmdathoraindex, x.Emprcodi, x.Barrcodi }).Where(x => x.Emprcodi == datos.Emprcodi).Distinct().ToList();
            //Obtiene lista de fecha del periodo
            var listaFechas = listDatosFinales.Select(x => new { x.Mmmdatfecha.Date }).Distinct().ToList();

            //Obtiene fecha Inicial cambio
            var fechaInicioCambio = listaFechas.OrderBy(x => x.Date).FirstOrDefault();
            //Obtiene fecha Fin cambio
            var fechaFinCambio = listaFechas.OrderByDescending(x => x.Date).FirstOrDefault();
            //Lista Data version  actual de los dias  del periodo con cambios
            List<MeMedicion48DTO> dataActual = new List<MeMedicion48DTO>();
            //Lista Data version Anterior de los dias  del periodo con cambios
            List<MeMedicion48DTO> dataAnterior = new List<MeMedicion48DTO>();

            //Lista Data version  actual de los dias  del periodo con cambios
            List<MeMedicion48DTO> dataActualMwProg = new List<MeMedicion48DTO>();
            //Lista Data version Anterior de los dias  del periodo con cambios
            List<MeMedicion48DTO> dataAnteriorMwProg = new List<MeMedicion48DTO>();

            List<SiEmpresaDTO> listaEmpresaAux;
            List<ReporteControlCambios> listControlCambio = new List<ReporteControlCambios>();
            //Console.WriteLine("Solo la hora: {0}\n", DateTime.Now.ToString("h:mm:ss"));
            if (listaFechas.Count != 0)
            {
                DateTime? fehcaInicio = fechaInicioCambio.Date;
                DateTime? fehcaFin = fechaFinCambio.Date;

                foreach (var recorridoFecha in listaFechas)
                {

                    dataActual.AddRange(this.ListarIndicador(vermmcodi, recorridoFecha.Date, recorridoFecha.Date, ConstantesMonitoreo.CodigoILE, listFacTable, ConstantesMonitoreo.ControlCambiosMonitoreo, out listaEmpresaAux));
                    dataAnterior.AddRange(this.ListarIndicador(version, recorridoFecha.Date, recorridoFecha.Date, ConstantesMonitoreo.CodigoILE, listFacTable, ConstantesMonitoreo.ControlCambiosMonitoreo, out listaEmpresaAux));
                }

                foreach (var recorridoV in listaData)
                {
                    ReporteControlCambios regControlCambio = new ReporteControlCambios();

                    DateTime fechaRecorrido = recorridoV.Mmmdatfecha.Date;

                    var objBarra = listaBarraRecorrido.Find(x => x.BarrCodi == recorridoV.Barrcodi);

                    var regActualCostoEject = dataActual.Find(x => x.Emprcodi == recorridoV.Emprcodi && x.Emprnomb != null && x.Barrcodi == recorridoV.Barrcodi && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCMEjec);
                    var regAnteriorCostoEject = dataAnterior.Find(x => x.Emprcodi == recorridoV.Emprcodi && x.Emprnomb != null && x.Barrcodi == recorridoV.Barrcodi && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCMEjec);
                    var regActualCv = dataActual.Find(x => x.Emprcodi == recorridoV.Emprcodi && x.Emprnomb != null && x.Barrcodi == recorridoV.Barrcodi && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCV);
                    var regAnteriorCv = dataAnterior.Find(x => x.Emprcodi == recorridoV.Emprcodi && x.Emprnomb != null && x.Barrcodi == recorridoV.Barrcodi && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCV);

                    decimal? cmgjecutadoAnterior = regAnteriorCostoEject != null ? (decimal?)regAnteriorCostoEject.GetType().GetProperty(ConstantesAppServicio.CaracterH + recorridoV.Mmmdathoraindex).GetValue(regAnteriorCostoEject, null) : null;
                    decimal? cmgjecutadoActual = regActualCostoEject != null ? (decimal?)regActualCostoEject.GetType().GetProperty(ConstantesAppServicio.CaracterH + recorridoV.Mmmdathoraindex).GetValue(regActualCostoEject, null) : null;

                    decimal? cvjecutadoAnterior = regAnteriorCv != null ? (decimal?)regAnteriorCv.GetType().GetProperty(ConstantesAppServicio.CaracterH + recorridoV.Mmmdathoraindex).GetValue(regAnteriorCv, null) : null;
                    decimal? cvjecutadoActual = regActualCv != null ? (decimal?)regActualCv.GetType().GetProperty(ConstantesAppServicio.CaracterH + recorridoV.Mmmdathoraindex).GetValue(regActualCv, null) : null;

                    if (cmgjecutadoActual != cmgjecutadoAnterior || cvjecutadoActual != cvjecutadoAnterior)
                    {
                        string nombreBarra = objBarra != null ? objBarra.BarrNombre : string.Empty;
                        var emprnomb = listaEmpresa.Find(x => x.Emprcodi == recorridoV.Emprcodi);
                        regControlCambio.Emprnomb = emprnomb.Emprnomb;
                        regControlCambio.Barrcodi = recorridoV.Barrcodi.Value;
                        regControlCambio.Barrnombre = nombreBarra;

                        regControlCambio.Fecha = recorridoV.Mmmdatfecha;
                        regControlCambio.CosmarActual = cmgjecutadoActual != cmgjecutadoAnterior ? cmgjecutadoActual : null;
                        regControlCambio.CosmarAnt = cmgjecutadoActual != cmgjecutadoAnterior ? cmgjecutadoAnterior : null;
                        regControlCambio.CvActual = cvjecutadoActual != cvjecutadoAnterior ? cvjecutadoActual : null;
                        regControlCambio.CvAnt = cvjecutadoActual != cvjecutadoAnterior ? cvjecutadoAnterior : null;

                        regControlCambio.FechaEnvioAnt = objVersionAnterior.Vermmfeccreacion;
                        regControlCambio.FechaEnvioActual = objVersionActual.Vermmfeccreacion;
                        regControlCambio.UsuarioCreacionAnterior = objVersionAnterior.Vermmusucreacion;
                        regControlCambio.UsuarioCreacionActual = objVersionActual.Vermmusucreacion;

                        listControlCambio.Add(regControlCambio);
                    }
                }

            }
            var listControl = listControlCambio.OrderBy(x => x.Fecha).ToList();
            return listControl;
        }

        /// <summary>
        /// Lista control de Cambio Congestiones
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="vermmcodi"></param>
        /// <param name="listFacTable"></param>
        /// <param name="barras"></param>
        /// <returns></returns>
        public List<ReporteControlCambios> ListaControlCambiosCongestion(string empresas, DateTime fechaInicio, DateTime fechaFin, int vermmcodi, List<MmmDatoDTO> listFacTable, string barras)
        {
            //Lista de empresas
            List<int> listaEmprcodiByData = listFacTable.Select(x => x.Emprcodi).Distinct().ToList();
            List<SiEmpresaDTO> listaEmpresa = this.ListarEmpresasMonitoreo(fechaInicio, empresas);
            listaEmpresa = listaEmpresa.Where(x => listaEmprcodiByData.Contains(x.Emprcodi)).ToList();

            List<MmmVersionDTO> listVersion = this.GetByCriteriaMmmVersions();
            var objVersionActual = listVersion.Find(x => x.Vermmcodi == vermmcodi && x.Vermmfechaperiodo.Date == fechaInicio);
            var listVersionPeriodo = listVersion.Where(x => x.Vermmfechaperiodo.Date == fechaInicio && x.Vermmporcentaje == 100).ToList();
            int version = 0;
            int[] barralistado = new int[barras.Length];
            barralistado = barras.Split(',').Select(x => int.Parse(x)).ToArray();
            List<BarraDTO> listaBarra = ListarGrupoBarra();
            List<BarraDTO> listaBarraRecorrido = listaBarra.Where(x => barralistado.Contains(x.BarrCodi)).ToList();
            ///Lista medicion48 Version Actual
            version = this.VersionAnterior(objVersionActual.Vermmcodi, listVersionPeriodo);
            var objVersionAnterior = listVersion.Find(x => x.Vermmcodi == version && x.Vermmfechaperiodo.Date == fechaInicio);
            List<MmmCambioversionDTO> listCambioVersionAnterior = GetByCriteriaMmmCambioversions(version);
            List<MmmDatoDTO> listDatosFinales = new List<MmmDatoDTO>();
            listDatosFinales = ListarDatosCambios(listCambioVersionAnterior, listFacTable);
            List<ReporteControlCambios> listControlCambio = new List<ReporteControlCambios>();

            foreach (var datos in listaEmpresa)
            {
                var listaData = listDatosFinales.Select(x => new { x.Mmmdatfecha, x.Mmmdathoraindex, x.Emprcodi, x.Barrcodi, x.Grupocodi }).Where(x => x.Emprcodi == datos.Emprcodi).Distinct().ToList();
                var listaFechas = listDatosFinales.Select(x => new { x.Mmmdatfecha.Date, x.Emprcodi }).Where(x => x.Emprcodi == datos.Emprcodi).Distinct().ToList();
                //Lista Data version  actual de los dias  del periodo con cambios
                List<MeMedicion48DTO> dataActual = new List<MeMedicion48DTO>();
                //Lista Data version Anterior de los dias  del periodo con cambios
                List<MeMedicion48DTO> dataAnterior = new List<MeMedicion48DTO>();
                //Lista Data version  actual de los dias  del periodo con cambios
                List<MeMedicion48DTO> dataActualMwProg = new List<MeMedicion48DTO>();
                //Lista Data version Anterior de los dias  del periodo con cambios
                List<MeMedicion48DTO> dataAnteriorMwProg = new List<MeMedicion48DTO>();

                List<SiEmpresaDTO> listaEmpresaAux;
                foreach (var registroData in listaFechas)
                {
                    dataActual.AddRange(this.ListarIndicador(vermmcodi, registroData.Date, registroData.Date, ConstantesMonitoreo.CodigoIRT, listFacTable, ConstantesMonitoreo.ControlCambiosMonitoreo, out listaEmpresaAux));
                    dataAnterior.AddRange(this.ListarIndicador(version, registroData.Date, registroData.Date, ConstantesMonitoreo.CodigoIRT, listFacTable, ConstantesMonitoreo.ControlCambiosMonitoreo, out listaEmpresaAux));
                }

                foreach (var recorridoV in listaData)
                {
                    MeMedicion48DTO regIRT = dataActual.Find(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaIRT && x.Grupocodi == recorridoV.Grupocodi);
                    if (regIRT != null)
                    {
                        ReporteControlCambios regControlCambio = new ReporteControlCambios();
                        DateTime fechaRecorrido = recorridoV.Mmmdatfecha.Date;
                        //Declaracion de Objetos Anterior y actual
                        var objBarra = listaBarraRecorrido.Find(x => x.BarrCodi == recorridoV.Barrcodi);
                        var regActualMwEject = dataActual.Find(x => x.Grupocodi == recorridoV.Grupocodi && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaPotGrupoEjec);
                        var regAnteriorMwEject = dataAnterior.Find(x => x.Grupocodi == recorridoV.Grupocodi && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaPotGrupoEjec);
                        var regActualMwProg = dataActual.Find(x => x.Emprcodi == datos.Emprcodi && x.Emprnomb != null && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaPotGrupoProg);
                        var regAnteriorMwProg = dataAnterior.Find(x => x.Emprcodi == datos.Emprcodi && x.Emprnomb != null && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaPotGrupoProg);
                        var regActualCostoEject = dataActual.Find(x => x.Emprcodi == datos.Emprcodi && x.Emprnomb != null && x.Barrcodi == recorridoV.Barrcodi && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCMEjec);
                        var regAnteriorCostoEject = dataAnterior.Find(x => x.Emprcodi == datos.Emprcodi && x.Emprnomb != null && x.Barrcodi == recorridoV.Barrcodi && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCMEjec);
                        var regActualCostoProg = dataActual.Find(x => x.Emprcodi == datos.Emprcodi && x.Emprnomb != null && x.Barrcodi == recorridoV.Barrcodi && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCMProg);
                        var regAnteriorCostoProg = dataAnterior.Find(x => x.Emprcodi == datos.Emprcodi && x.Emprnomb != null && x.Barrcodi == recorridoV.Barrcodi && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCMProg);

                        decimal? MwEjectuActual = regActualMwEject != null ? (decimal?)regActualMwEject.GetType().GetProperty(ConstantesAppServicio.CaracterH + recorridoV.Mmmdathoraindex).GetValue(regActualMwEject, null) : null;
                        decimal? MwEjecAnterior = regAnteriorMwEject != null ? (decimal?)regAnteriorMwEject.GetType().GetProperty(ConstantesAppServicio.CaracterH + recorridoV.Mmmdathoraindex).GetValue(regAnteriorMwEject, null) : null;
                        decimal? MwProgActual = regActualMwProg != null ? (decimal?)regActualMwProg.GetType().GetProperty(ConstantesAppServicio.CaracterH + recorridoV.Mmmdathoraindex).GetValue(regActualMwProg, null) : null;
                        decimal? MwProgAnterior = regAnteriorMwProg != null ? (decimal?)regAnteriorMwProg.GetType().GetProperty(ConstantesAppServicio.CaracterH + recorridoV.Mmmdathoraindex).GetValue(regAnteriorMwProg, null) : null;
                        decimal? cmgjecutadoActual = regActualCostoEject != null ? (decimal?)regActualCostoEject.GetType().GetProperty(ConstantesAppServicio.CaracterH + recorridoV.Mmmdathoraindex).GetValue(regActualCostoEject, null) : null;
                        decimal? cmgjecutadoAnterior = regAnteriorCostoEject != null ? (decimal?)regAnteriorCostoEject.GetType().GetProperty(ConstantesAppServicio.CaracterH + recorridoV.Mmmdathoraindex).GetValue(regAnteriorCostoEject, null) : null;
                        decimal? cmgProgActual = regActualCostoProg != null ? (decimal?)regActualCostoProg.GetType().GetProperty(ConstantesAppServicio.CaracterH + recorridoV.Mmmdathoraindex).GetValue(regActualCostoProg, null) : null;
                        decimal? cmgProgAnterior = regAnteriorCostoProg != null ? (decimal?)regAnteriorCostoProg.GetType().GetProperty(ConstantesAppServicio.CaracterH + recorridoV.Mmmdathoraindex).GetValue(regAnteriorCostoProg, null) : null;

                        //Si hay variones distintos se hace la comparacion
                        if (cmgjecutadoActual != cmgjecutadoAnterior || cmgProgActual != cmgProgAnterior || MwEjecAnterior != MwEjectuActual || MwProgActual != MwProgAnterior)
                        {
                            string nombreBarra = objBarra != null ? objBarra.BarrNombre : string.Empty;
                            regControlCambio.Emprnomb = datos.Emprnomb;
                            regControlCambio.Barrcodi = recorridoV.Barrcodi.GetValueOrDefault(0);
                            regControlCambio.Barrnombre = nombreBarra;
                            regControlCambio.Grupocodi = regIRT.Grupocodi;
                            regControlCambio.Gruponomb = regIRT.Gruponomb;
                            regControlCambio.Fecha = recorridoV.Mmmdatfecha;
                            regControlCambio.MWEjecAnt = MwEjectuActual != MwEjecAnterior ? MwEjecAnterior : null; ;
                            regControlCambio.MWEjecActual = MwEjectuActual != MwEjecAnterior ? MwEjectuActual : null;
                            regControlCambio.MWProgAnt = MwProgActual != MwProgAnterior ? MwProgAnterior : null;
                            regControlCambio.MWProgActual = MwProgActual != MwProgAnterior ? MwProgAnterior : null;
                            regControlCambio.CosmarActual = cmgjecutadoActual != cmgjecutadoAnterior ? cmgjecutadoActual : null;
                            regControlCambio.CosmarAnt = cmgjecutadoActual != cmgjecutadoAnterior ? cmgjecutadoAnterior : null; ;
                            regControlCambio.CosmarProgActual = cmgProgActual != cmgProgAnterior ? cmgProgActual : null;
                            regControlCambio.CosmarProgAnt = cmgProgActual != cmgProgAnterior ? cmgProgAnterior : null;
                            regControlCambio.FechaEnvioAnt = objVersionAnterior.Vermmfeccreacion;
                            regControlCambio.FechaEnvioActual = objVersionActual.Vermmfeccreacion;
                            regControlCambio.UsuarioCreacionAnterior = objVersionAnterior.Vermmusucreacion;
                            regControlCambio.UsuarioCreacionActual = objVersionActual.Vermmusucreacion;
                            listControlCambio.Add(regControlCambio);
                        }
                    }
                }
            }
            return listControlCambio;
        }

        /// <summary>
        /// Version Anterior  Seleccionada
        /// </summary>
        /// <param name="versionActual"></param>
        /// <param name="listVersionPeriodo"></param>
        /// <returns></returns>
        public int VersionAnterior(int versionActual, List<MmmVersionDTO> listVersionPeriodo)
        {
            int version = 0;
            foreach (var recorridoVersion in listVersionPeriodo)
            {
                if (recorridoVersion.Vermmcodi == versionActual)
                {
                    break;
                }
                version = recorridoVersion.Vermmcodi;
            }
            return version;
        }

        /// <summary>
        /// Lita Datos Seleccionados Version
        /// </summary>
        /// <param name="listCambioVersionAnterior"></param>
        /// <param name="listFacTable"></param>
        /// <returns></returns>
        public List<MmmDatoDTO> ListarDatosCambios(List<MmmCambioversionDTO> listCambioVersionAnterior, List<MmmDatoDTO> listFacTable)
        {
            List<MmmDatoDTO> listDatosFinales = new List<MmmDatoDTO>();

            List<DateTime> listaFechas = listCambioVersionAnterior.Select(x => x.Mmmdatfecha.Date).Distinct().OrderBy(x => x).ToList();

            foreach (var dia in listaFechas)
            {
                var listaFacXDia = listFacTable.Where(x => x.Mmmdatfecha >= dia.Date && x.Mmmdatfecha <= dia.Date.AddDays(1)).ToList();
                var listaCamXDia = listCambioVersionAnterior.Where(x => x.Mmmdatfecha >= dia.Date && x.Mmmdatfecha <= dia.Date.AddDays(1)).ToList();

                foreach (var rec in listaCamXDia)
                {
                    var listData = listaFacXDia.Where(x => x.Mmmdatcodi == rec.Mmmdatcodi);
                    listDatosFinales.AddRange(listData);
                }
            }

            return listDatosFinales;
        }

        /// <summary>
        /// Obtener Html del Control de cambios  Indicador 1
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        /// 
        public string ControlCambiosIndicador1Html(string empresas, DateTime fechaInicio, DateTime fechaFin, int vermmcodi, List<MmmDatoDTO> listFacTable)
        {
            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";

            //Lista de empresas
            int padding = 20;
            int anchoTotal = (100 + padding) + (8) * (200 + padding);

            strHtml.Append("<div class='freeze_table' id='resultado1' style='height: 800px;'>");
            strHtml.AppendFormat("<table id='reporte1' class='pretty tabla-icono' style='table-layout: fixed; width: {0}px;' >", anchoTotal);
            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width: 250px; word-wrap: break-word; white-space: normal;'> Empresa </th>");
            strHtml.Append("<th style='width: 100px; word-wrap: break-word; white-space: normal;'> Fecha </th>");
            strHtml.Append("<th style='width: 110px; word-wrap: break-word; white-space: normal;' class='envio_anterior'> Envío Anterior <br/> (MW Ejecutado) </th>");
            strHtml.Append("<th style='width: 110px; word-wrap: break-word; white-space: normal;'> Último  Envío <br/>  (MW Ejecutado)</th>");
            strHtml.Append("<th style='width: 100px; word-wrap: break-word; white-space: normal;' class='envio_anterior'> Fecha de Envío Anterior </th>");
            strHtml.Append("<th style='width: 100px; word-wrap: break-word; white-space: normal;'> Fecha de Envío Último </th>");
            strHtml.Append("<th style='width: 80px; word-wrap: break-word; white-space: normal;' class='envio_anterior'> Usuario de Envío Anterior </th>");
            strHtml.Append("<th style='width: 80px; word-wrap: break-word; white-space: normal;'> Usuario de Envío Último </th>");
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");
            strHtml.Append("<tbody>");

            //     ConstantesMonitoreo
            List<ReporteControlCambios> listCambios = this.ListaControlCambios(empresas, fechaInicio, fechaFin, vermmcodi, listFacTable);

            foreach (var datos in listCambios)
            {
                strHtml.Append("<tr>");
                strHtml.Append(string.Format("<td class='tdbody_reporte'>{0}</td>", datos.Emprnomb));
                strHtml.Append(string.Format("<td>{0}</td>", datos.Fecha.ToString(ConstantesBase.FormatFechaFull)));
                strHtml.Append(string.Format("<td>{0}</td>", datos.MWEjecAnt != null ? datos.MWEjecAnt.Value.ToString("N", nfi) : string.Empty));
                strHtml.Append(string.Format("<td>{0}</td>", datos.MWEjecActual != null ? datos.MWEjecActual.Value.ToString("N", nfi) : string.Empty));
                strHtml.Append(string.Format("<td>{0}</td>", datos.FechaEnvioAnt.ToString(ConstantesBase.FormatFechaFull)));
                strHtml.Append(string.Format("<td>{0}</td>", datos.FechaEnvioActual.ToString(ConstantesBase.FormatFechaFull)));
                strHtml.Append(string.Format("<td>{0}</td>", datos.UsuarioCreacionAnterior));
                strHtml.Append(string.Format("<td>{0}</td>", datos.UsuarioCreacionActual));
                strHtml.Append("</tr>");
            }
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Obtener Html del Control de cambios  Indicador 2
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        /// 
        public string ControlCambiosIndicador2Html(string empresas, DateTime fechaInicio, DateTime fechaFin, int vermmcodi, List<MmmDatoDTO> listFacTable)
        {
            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";
            //Lista de empresas

            int padding = 20;
            int anchoTotal = (100 + padding) + (8) * (200 + padding);

            strHtml.Append("<div class='freeze_table' id='resultado2' style='height: 800px;'>");
            strHtml.AppendFormat("<table id='reporte2' class='pretty tabla-icono' style='table-layout: fixed; width: {0}px;' >", anchoTotal);
            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width: 250px; word-wrap: break-word; white-space: normal;'> Empresa </th>");
            strHtml.Append("<th style='width: 100px; word-wrap: break-word; white-space: normal;'> Fecha </th>");
            strHtml.Append("<th style='width: 110px; word-wrap: break-word; white-space: normal;' class='envio_anterior'> Envío Anterior <br/> (MW Ejecutado) </th>");
            strHtml.Append("<th style='width: 110px; word-wrap: break-word; white-space: normal;'> Último  Envío <br/>  (MW Ejecutado)</th>");
            strHtml.Append("<th style='width: 100px; word-wrap: break-word; white-space: normal;' class='envio_anterior'> Fecha de Envío Anterior </th>");
            strHtml.Append("<th style='width: 100px; word-wrap: break-word; white-space: normal;'> Fecha de Envío Último </th>");
            strHtml.Append("<th style='width: 80px; word-wrap: break-word; white-space: normal;' class='envio_anterior'> Usuario de Envío Anterior </th>");
            strHtml.Append("<th style='width: 80px; word-wrap: break-word; white-space: normal;'> Usuario de Envío Último </th>");
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");
            strHtml.Append("<tbody>");

            //     ConstantesMonitoreo
            List<ReporteControlCambios> listCambios = this.ListaControlCambios(empresas, fechaInicio, fechaFin, vermmcodi, listFacTable);

            foreach (var datos in listCambios)
            {
                strHtml.Append("<tr>");
                strHtml.Append(string.Format("<td class='tdbody_reporte'>{0}</td>", datos.Emprnomb));
                strHtml.Append(string.Format("<td>{0}</td>", datos.Fecha.ToString(ConstantesBase.FormatFechaFull)));
                strHtml.Append(string.Format("<td>{0}</td>", datos.MWEjecAnt != null ? datos.MWEjecAnt.Value.ToString("N", nfi) : string.Empty));
                strHtml.Append(string.Format("<td>{0}</td>", datos.MWEjecActual != null ? datos.MWEjecActual.Value.ToString("N", nfi) : string.Empty));
                strHtml.Append(string.Format("<td>{0}</td>", datos.FechaEnvioAnt.ToString(ConstantesBase.FormatFechaFull)));
                strHtml.Append(string.Format("<td>{0}</td>", datos.FechaEnvioActual.ToString(ConstantesBase.FormatFechaFull)));
                strHtml.Append(string.Format("<td>{0}</td>", datos.UsuarioCreacionAnterior));
                strHtml.Append(string.Format("<td>{0}</td>", datos.UsuarioCreacionActual));
                strHtml.Append("</tr>");
            }
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Html Control de cambios indicador 2
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public string ControlCambiosIndicador3Html(string empresas, DateTime fechaInicio, DateTime fechaFin, int vermmcodi, List<MmmDatoDTO> listFacTable)
        {
            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";
            //Lista de empresas

            int padding = 20;
            int anchoTotal = (100 + padding) + (10) * (200 + padding);

            strHtml.Append("<div class='freeze_table' id='resultado3' style='height: 800px;'>");
            strHtml.AppendFormat("<table id='reporte3' class='pretty tabla-icono' style='table-layout: fixed; width: {0}px;' >", anchoTotal);
            strHtml.Append("<thead>");
            strHtml.Append("<tr>");

            strHtml.Append("<th style='width: 250px; word-wrap: break-word; white-space: normal;'> Empresa </th>");
            strHtml.Append("<th style='width: 100px; word-wrap: break-word; white-space: normal;'> Fecha </th>");
            strHtml.Append("<th style='width: 110px; word-wrap: break-word; white-space: normal;' class='envio_anterior'> Envío Anterior <br/> (MW Ejecutado) </th>");
            strHtml.Append("<th style='width: 110px; word-wrap: break-word; white-space: normal;'> Último  Envío <br/>  (MW Ejecutado)</th>");
            strHtml.Append("<th style='width: 110px; word-wrap: break-word; white-space: normal;' class='envio_anterior'> Envío Anterior <br/> (MW Programado) </th>");
            strHtml.Append("<th style='width: 110px; word-wrap: break-word; white-space: normal;'> Último  Envío <br/>  (MW Programado)</th>");
            strHtml.Append("<th style='width: 100px; word-wrap: break-word; white-space: normal;' class='envio_anterior'> Fecha de Envío Anterior </th>");
            strHtml.Append("<th style='width: 100px; word-wrap: break-word; white-space: normal;'> Fecha de Envío Último </th>");
            strHtml.Append("<th style='width: 80px; word-wrap: break-word; white-space: normal;' class='envio_anterior'> Usuario de Envío Anterior </th>");
            strHtml.Append("<th style='width: 80px; word-wrap: break-word; white-space: normal;'> Usuario de Envío Último </th>");

            strHtml.Append("</tr>");
            strHtml.Append("</thead>");
            strHtml.Append("<tbody>");

            //     ConstantesMonitoreo
            List<ReporteControlCambios> listCambios = this.ListaControlCambios(empresas, fechaInicio, fechaFin, vermmcodi, listFacTable);

            foreach (var datos in listCambios)
            {
                strHtml.Append("<tr>");
                strHtml.Append(string.Format("<td class='tdbody_reporte'>{0}</td>", datos.Emprnomb));
                strHtml.Append(string.Format("<td>{0}</td>", datos.Fecha.ToString(ConstantesBase.FormatFechaFull)));
                strHtml.Append(string.Format("<td>{0}</td>", datos.MWEjecAnt != null ? datos.MWEjecAnt.Value.ToString("N", nfi) : string.Empty));
                strHtml.Append(string.Format("<td>{0}</td>", datos.MWEjecActual != null ? datos.MWEjecActual.Value.ToString("N", nfi) : string.Empty));
                strHtml.Append(string.Format("<td>{0}</td>", datos.MWProgAnt != null ? datos.MWProgAnt.Value.ToString("N", nfi) : string.Empty));
                strHtml.Append(string.Format("<td>{0}</td>", datos.MWProgActual != null ? datos.MWProgActual.Value.ToString("N", nfi) : string.Empty));
                strHtml.Append(string.Format("<td>{0}</td>", datos.FechaEnvioAnt.ToString(ConstantesBase.FormatFechaFull)));
                strHtml.Append(string.Format("<td>{0}</td>", datos.FechaEnvioActual.ToString(ConstantesBase.FormatFechaFull)));
                strHtml.Append(string.Format("<td>{0}</td>", datos.UsuarioCreacionAnterior));
                strHtml.Append(string.Format("<td>{0}</td>", datos.UsuarioCreacionActual));
                strHtml.Append("</tr>");
            }
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        /// <summary>
        ///  Control de cambios Html indicador 4 
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public string ControlCambiosIndicador4Html(string empresas, DateTime fechaInicio, DateTime fechaFin, int vermmcodi, List<MmmDatoDTO> listFacTable)
        {
            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";
            //Lista de empresas

            int padding = 20;
            int anchoTotal = (100 + padding) + (10) * (200 + padding);

            strHtml.Append("<div class='freeze_table' id='resultado4' style='height: 800px;'>");
            strHtml.AppendFormat("<table id='reporte4' class='pretty tabla-icono' style='table-layout: fixed; width: {0}px;' >", anchoTotal);
            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width: 250px; word-wrap: break-word; white-space: normal;'> Empresa </th>");
            strHtml.Append("<th style='width: 100px; word-wrap: break-word; white-space: normal;'> Fecha </th>");
            strHtml.Append("<th style='width: 110px; word-wrap: break-word; white-space: normal;' class='envio_anterior'> Envío Anterior <br/> (MW Ejecutado) </th>");
            strHtml.Append("<th style='width: 110px; word-wrap: break-word; white-space: normal;'> Último  Envío <br/>  (MW Ejecutado)</th>");
            strHtml.Append("<th style='width: 110px; word-wrap: break-word; white-space: normal;' class='envio_anterior'> Envío Anterior <br/> (MW Programado) </th>");
            strHtml.Append("<th style='width: 110px; word-wrap: break-word; white-space: normal;'> Último  Envío <br/>  (MW Programado)</th>");
            strHtml.Append("<th style='width: 100px; word-wrap: break-word; white-space: normal;' class='envio_anterior'> Fecha de Envío Anterior </th>");
            strHtml.Append("<th style='width: 100px; word-wrap: break-word; white-space: normal;'> Fecha de Envío Último </th>");
            strHtml.Append("<th style='width: 80px; word-wrap: break-word; white-space: normal;' class='envio_anterior'> Usuario de Envío Anterior </th>");
            strHtml.Append("<th style='width: 80px; word-wrap: break-word; white-space: normal;'> Usuario de Envío Último </th>");
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");
            strHtml.Append("<tbody>");

            //     ConstantesMonitoreo
            List<ReporteControlCambios> listCambios = this.ListaControlCambios(empresas, fechaInicio, fechaFin, vermmcodi, listFacTable);


            foreach (var datos in listCambios)
            {
                strHtml.Append("<tr>");
                strHtml.Append(string.Format("<td class='tdbody_reporte'>{0}</td>", datos.Emprnomb));
                strHtml.Append(string.Format("<td>{0}</td>", datos.Fecha.ToString(ConstantesBase.FormatFechaFull)));
                strHtml.Append(string.Format("<td>{0}</td>", datos.MWEjecAnt != null ? datos.MWEjecAnt.Value.ToString("N", nfi) : string.Empty));
                strHtml.Append(string.Format("<td>{0}</td>", datos.MWEjecActual != null ? datos.MWEjecActual.Value.ToString("N", nfi) : string.Empty));
                strHtml.Append(string.Format("<td>{0}</td>", datos.MWProgAnt != null ? datos.MWProgAnt.Value.ToString("N", nfi) : string.Empty));
                strHtml.Append(string.Format("<td>{0}</td>", datos.MWProgActual != null ? datos.MWProgActual.Value.ToString("N", nfi) : string.Empty));
                strHtml.Append(string.Format("<td>{0}</td>", datos.FechaEnvioAnt.ToString(ConstantesBase.FormatFechaFull)));
                strHtml.Append(string.Format("<td>{0}</td>", datos.FechaEnvioActual.ToString(ConstantesBase.FormatFechaFull)));
                strHtml.Append(string.Format("<td>{0}</td>", datos.UsuarioCreacionAnterior));
                strHtml.Append(string.Format("<td>{0}</td>", datos.UsuarioCreacionActual));
                strHtml.Append("</tr>");
            }
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Html Control de cambios indicador 2
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public string ControlCambiosIndicador5Html(string empresas, DateTime fechaInicio, DateTime fechaFin, int vermmcodi, List<MmmDatoDTO> listFacTable, string barras)
        {
            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";

            int padding = 20;
            int anchoTotal = (100 + padding) + (11) * (200 + padding);

            strHtml.Append("<div class='freeze_table' id='resultado5' style='height: 800px;'>");
            strHtml.AppendFormat("<table id='reporte5' class='pretty tabla-icono' style='table-layout: fixed; width: {0}px;' >", anchoTotal);
            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width: 250px; word-wrap: break-word; white-space: normal;'> Empresa </th>");
            strHtml.Append("<th style='width: 150px; word-wrap: break-word; white-space: normal;'> Barra </th>");
            strHtml.Append("<th style='width: 100px; word-wrap: break-word; white-space: normal;'> Fecha </th>");

            strHtml.Append("<th style='width: 110px; word-wrap: break-word; white-space: normal;' class='envio_anterior'> Envío Anterior <br/> (Cmg Ejecutado) </th>");
            strHtml.Append("<th style='width: 110px; word-wrap: break-word; white-space: normal;'> Último  Envío <br/>  (Cmg Ejecutado)</th>");

            strHtml.Append("<th style='width: 110px; word-wrap: break-word; white-space: normal;' class='envio_anterior'> Envío Anterior <br/> (CV) </th>");
            strHtml.Append("<th style='width: 110px; word-wrap: break-word; white-space: normal;'> Último  Envío <br/>  (CV)</th>");

            strHtml.Append("<th style='width: 100px; word-wrap: break-word; white-space: normal;' class='envio_anterior'> Fecha de Envío Anterior </th>");
            strHtml.Append("<th style='width: 100px; word-wrap: break-word; white-space: normal;'> Fecha de Envío Último </th>");
            strHtml.Append("<th style='width: 80px; word-wrap: break-word; white-space: normal;' class='envio_anterior'> Usuario de Envío Anterior </th>");
            strHtml.Append("<th style='width: 80px; word-wrap: break-word; white-space: normal;'> Usuario de Envío Último </th>");
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");
            strHtml.Append("<tbody>");

            /////////////inicio

            List<ReporteControlCambios> listCambios = this.ListaControlCambiosIndicadorLernerCosto(empresas, fechaInicio, fechaFin, vermmcodi, listFacTable, barras);

            foreach (var datos in listCambios)
            {
                strHtml.Append("<tr>");
                strHtml.Append(string.Format("<td class='tdbody_reporte'>{0}</td>", datos.Emprnomb));
                strHtml.Append(string.Format("<td>{0}</td>", datos.Barrnombre));
                strHtml.Append(string.Format("<td>{0}</td>", datos.Fecha.ToString(ConstantesBase.FormatFechaFull)));
                strHtml.Append(string.Format("<td>{0}</td>", datos.CosmarAnt != null ? datos.CosmarAnt.Value.ToString("N", nfi) : string.Empty));
                strHtml.Append(string.Format("<td>{0}</td>", datos.CosmarActual != null ? datos.CosmarActual.Value.ToString("N", nfi) : string.Empty));
                strHtml.Append(string.Format("<td>{0}</td>", datos.CvAnt != null ? datos.CvAnt.Value.ToString("N", nfi) : string.Empty));
                strHtml.Append(string.Format("<td>{0}</td>", datos.CvActual != null ? datos.CvActual.Value.ToString("N", nfi) : string.Empty));
                strHtml.Append(string.Format("<td>{0}</td>", datos.FechaEnvioAnt.ToString(ConstantesBase.FormatFechaFull)));
                strHtml.Append(string.Format("<td>{0}</td>", datos.FechaEnvioActual.ToString(ConstantesBase.FormatFechaFull)));
                strHtml.Append(string.Format("<td>{0}</td>", datos.UsuarioCreacionAnterior));
                strHtml.Append(string.Format("<td>{0}</td>", datos.UsuarioCreacionActual));
                strHtml.Append("</tr>");
            }
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            strHtml.Append("</div>");

            return strHtml.ToString();


        }

        /// <summary>
        ///  Control de cambios Html indicador 6
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public string ControlCambiosIndicador6Html(string empresas, DateTime fechaInicio, DateTime fechaFin, int vermmcodi, List<MmmDatoDTO> listFacTable, string barras)
        {
            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";

            int padding = 20;
            int anchoTotal = (100 + padding) + (11) * (200 + padding);

            strHtml.Append("<div class='freeze_table' id='resultado6' style='height: 800px;'>");
            strHtml.AppendFormat("<table id='reporte6' class='pretty tabla-icono' style='table-layout: fixed; width: {0}px;' >", anchoTotal);
            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width: 250px; word-wrap: break-word; white-space: normal;'> Empresa </th>");
            strHtml.Append("<th style='width: 150px; word-wrap: break-word; white-space: normal;'> Barra </th>");
            strHtml.Append("<th style='width: 100px; word-wrap: break-word; white-space: normal;'> Fecha </th>");

            strHtml.Append("<th style='width: 110px; word-wrap: break-word; white-space: normal;' class='envio_anterior'> Envío Anterior <br/> (Cmg Ejecutado) </th>");
            strHtml.Append("<th style='width: 110px; word-wrap: break-word; white-space: normal;'> Último  Envío <br/>  (Cmg Ejecutado)</th>");

            strHtml.Append("<th style='width: 110px; word-wrap: break-word; white-space: normal;' class='envio_anterior'> Envío Anterior <br/> (CV) </th>");
            strHtml.Append("<th style='width: 110px; word-wrap: break-word; white-space: normal;'> Último  Envío <br/>  (CV)</th>");

            strHtml.Append("<th style='width: 100px; word-wrap: break-word; white-space: normal;' class='envio_anterior'> Fecha de Envío Anterior </th>");
            strHtml.Append("<th style='width: 100px; word-wrap: break-word; white-space: normal;'> Fecha de Envío Último </th>");
            strHtml.Append("<th style='width: 80px; word-wrap: break-word; white-space: normal;' class='envio_anterior'> Usuario de Envío Anterior </th>");
            strHtml.Append("<th style='width: 80px; word-wrap: break-word; white-space: normal;'> Usuario de Envío Último </th>");
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");
            strHtml.Append("<tbody>");

            /////////////inicio
            List<ReporteControlCambios> listCambios = this.ListaControlCambiosIndicadorLernerCosto(empresas, fechaInicio, fechaFin, vermmcodi, listFacTable, barras);

            foreach (var datos in listCambios)
            {
                strHtml.Append("<tr>");
                strHtml.Append(string.Format("<td class='tdbody_reporte'>{0}</td>", datos.Emprnomb));
                strHtml.Append(string.Format("<td>{0}</td>", datos.Barrnombre));
                strHtml.Append(string.Format("<td>{0}</td>", datos.Fecha.ToString(ConstantesBase.FormatFechaFull)));
                strHtml.Append(string.Format("<td>{0}</td>", datos.CosmarAnt != null ? datos.CosmarAnt.Value.ToString("N", nfi) : string.Empty));
                strHtml.Append(string.Format("<td>{0}</td>", datos.CosmarActual != null ? datos.CosmarActual.Value.ToString("N", nfi) : string.Empty));
                strHtml.Append(string.Format("<td>{0}</td>", datos.CvAnt != null ? datos.CvAnt.Value.ToString("N", nfi) : string.Empty));
                strHtml.Append(string.Format("<td>{0}</td>", datos.CvActual != null ? datos.CvActual.Value.ToString("N", nfi) : string.Empty));
                strHtml.Append(string.Format("<td>{0}</td>", datos.FechaEnvioAnt.ToString(ConstantesBase.FormatFechaFull)));
                strHtml.Append(string.Format("<td>{0}</td>", datos.FechaEnvioActual.ToString(ConstantesBase.FormatFechaFull)));
                strHtml.Append(string.Format("<td>{0}</td>", datos.UsuarioCreacionAnterior));
                strHtml.Append(string.Format("<td>{0}</td>", datos.UsuarioCreacionActual));
                strHtml.Append("</tr>");
            }
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Html indicador 7
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="fechaHist"></param>
        /// <param name="generacion"></param>
        /// <returns></returns>
        public string ControlCambiosIndicador7Html(string empresas, DateTime fechaInicio, DateTime fechaFin, int vermmcodi, List<MmmDatoDTO> listFacTable, string barras)
        {
            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";
            int padding = 20;
            int anchoTotal = (100 + padding) + (15) * (200 + padding);
            strHtml.Append("<div class='freeze_table' id='resultado7' style='height: 800px;'>");
            strHtml.AppendFormat("<table id='reporte7' class='pretty tabla-icono' style='table-layout: fixed; width: {0}px;' >", anchoTotal);
            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width: 250px; word-wrap: break-word; white-space: normal;'> Empresa </th>");
            strHtml.Append("<th style='width: 150px; word-wrap: break-word; white-space: normal;'> Grupo Despacho </th>");
            strHtml.Append("<th style='width: 150px; word-wrap: break-word; white-space: normal;'> Barra </th>");
            strHtml.Append("<th style='width: 100px; word-wrap: break-word; white-space: normal;'> Fecha </th>");
            strHtml.Append("<th style='width: 110px; word-wrap: break-word; white-space: normal;' class='envio_anterior'> Envío Anterior <br/> (MW Ejecutado) </th>");
            strHtml.Append("<th style='width: 110px; word-wrap: break-word; white-space: normal;'> Último  Envío <br/>  (MW Ejecutado)</th>");
            strHtml.Append("<th style='width: 110px; word-wrap: break-word; white-space: normal;' class='envio_anterior'> Envío Anterior <br/> (MW Programado) </th>");
            strHtml.Append("<th style='width: 110px; word-wrap: break-word; white-space: normal;'> Último  Envío <br/>  (MW Programado)</th>");
            strHtml.Append("<th style='width: 110px; word-wrap: break-word; white-space: normal;' class='envio_anterior'> Envío Anterior <br/> (Cmg Ejecutado) </th>");
            strHtml.Append("<th style='width: 110px; word-wrap: break-word; white-space: normal;'> Último  Envío <br/>  (Cmg Ejecutado)</th>");
            strHtml.Append("<th style='width: 110px; word-wrap: break-word; white-space: normal;' class='envio_anterior'> Envío Anterior <br/> (Cmg Programado) </th>");
            strHtml.Append("<th style='width: 110px; word-wrap: break-word; white-space: normal;'> Último  Envío <br/>  (Cmg Programado)</th>");
            strHtml.Append("<th style='width: 100px; word-wrap: break-word; white-space: normal;' class='envio_anterior'> Fecha de Envío Anterior </th>");
            strHtml.Append("<th style='width: 100px; word-wrap: break-word; white-space: normal;'> Fecha de Envío Último </th>");
            strHtml.Append("<th style='width: 80px; word-wrap: break-word; white-space: normal;' class='envio_anterior'> Usuario de Envío Anterior </th>");
            strHtml.Append("<th style='width: 80px; word-wrap: break-word; white-space: normal;'> Usuario de Envío Último </th>");
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");
            strHtml.Append("<tbody>");

            /////////////inicio
            List<ReporteControlCambios> listCambios = this.ListaControlCambiosCongestion(empresas, fechaInicio, fechaFin, vermmcodi, listFacTable, barras);

            foreach (var datos in listCambios)
            {
                strHtml.Append("<tr>");
                strHtml.Append(string.Format("<td class='tdbody_reporte'>{0}</td>", datos.Emprnomb));
                strHtml.Append(string.Format("<td>{0}</td>", datos.Gruponomb));
                strHtml.Append(string.Format("<td>{0}</td>", datos.Barrnombre));
                strHtml.Append(string.Format("<td>{0}</td>", datos.Fecha.ToString(ConstantesBase.FormatFechaFull)));
                strHtml.Append(string.Format("<td>{0}</td>", datos.MWEjecAnt != null ? datos.MWEjecAnt.Value.ToString("N", nfi) : string.Empty));
                strHtml.Append(string.Format("<td>{0}</td>", datos.MWEjecActual != null ? datos.MWEjecActual.Value.ToString("N", nfi) : string.Empty));
                strHtml.Append(string.Format("<td>{0}</td>", datos.MWProgAnt != null ? datos.MWProgAnt.Value.ToString("N", nfi) : string.Empty));
                strHtml.Append(string.Format("<td>{0}</td>", datos.MWProgActual != null ? datos.MWProgActual.Value.ToString("N", nfi) : string.Empty));
                strHtml.Append(string.Format("<td>{0}</td>", datos.CosmarAnt != null ? datos.CosmarAnt.Value.ToString("N", nfi) : string.Empty));
                strHtml.Append(string.Format("<td>{0}</td>", datos.CosmarActual != null ? datos.CosmarActual.Value.ToString("N", nfi) : string.Empty));
                strHtml.Append(string.Format("<td>{0}</td>", datos.CosmarProgAnt != null ? datos.CosmarProgAnt.Value.ToString("N", nfi) : string.Empty));
                strHtml.Append(string.Format("<td>{0}</td>", datos.CosmarProgActual != null ? datos.CosmarProgActual.Value.ToString("N", nfi) : string.Empty));
                strHtml.Append(string.Format("<td>{0}</td>", datos.FechaEnvioAnt.ToString(ConstantesBase.FormatFechaFull)));
                strHtml.Append(string.Format("<td>{0}</td>", datos.FechaEnvioActual.ToString(ConstantesBase.FormatFechaFull)));
                strHtml.Append(string.Format("<td>{0}</td>", datos.UsuarioCreacionAnterior));
                strHtml.Append(string.Format("<td>{0}</td>", datos.UsuarioCreacionActual));
                strHtml.Append("</tr>");
            }
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            strHtml.Append("</div>");

            return strHtml.ToString();

        }
        #endregion

        #region Parámetros

        /// <summary>
        /// Listar estado
        /// </summary>
        /// <returns></returns>
        public List<EstadoParametro> ListarEstadoParametro()
        {
            List<EstadoParametro> listaEstado = new List<EstadoParametro>();
            listaEstado.Add(new EstadoParametro { EstadoCodigo2 = "A", EstadoCodigo = "N", EstadoDescripcion = "Activo" });
            listaEstado.Add(new EstadoParametro { EstadoCodigo2 = "B", EstadoCodigo = "S", EstadoDescripcion = "Baja" });

            return listaEstado;
        }

        /// <summary>
        /// Listar historico de Parametro de la Tendencia de HHI
        /// </summary>
        /// <returns></returns>
        public List<ParametroTendenciaHHI> ListarParametroTendenciaHHI()
        {
            List<SiParametroValorDTO> listaParam = this.servParametro.ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroTendenciaHHI);

            List<SiParametroValorDTO> listaRango = listaParam.Where(x => x.Siparveliminado == ConstantesParametro.EstadoActivo || x.Siparveliminado == ConstantesParametro.EstadoBaja).ToList().OrderByDescending(x => x.Siparvfechainicial).ToList();
            List<ParametroTendenciaHHI> lista = this.servParametro.GetListaParametroTendenciaHHI(listaRango, this.ListarEstadoParametro());

            return lista;
        }

        #endregion

        #region Relación Grupo Despacho - Barra Programada

        /// <summary>
        /// Listar la relacion entre Centrales y barras de programacion
        /// </summary>
        /// <returns></returns>
        public List<PrGrupoxcnfbarDTO> ListarGrupoDespachoBarraProgramacion()
        {
            return this.ListPrGrupoxcnfbars().Where(x => x.Grcnfbestado == ConstantesMonitoreo.PrGrupoBarraprogEstadoActivo).ToList();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CmConfigbarra
        /// </summary>
        public List<CmConfigbarraDTO> ListarBarraProgramacion(bool flagTodos)
        {
            List<CmConfigbarraDTO> lista = this.servCP.GetByCriteriaCmConfigbarras(ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto);

            return flagTodos ? lista : lista.Where(x => !string.IsNullOrEmpty(x.Cnfbarnombncp)).ToList();
        }

        /// <summary>
        /// Listar grupo por categoria
        /// </summary>
        /// <param name="categoria"></param>
        /// <returns></returns>
        public List<PrGrupoDTO> ListarGrupoPorCategoria(string iCategoria)
        {
            List<PrGrupoDTO> listaGrupo = FactorySic.GetPrGrupoRepository().ListaPrGruposPaginado(-2, iCategoria, string.Empty, ConstantesAppServicio.ParametroDefecto, -1, -1, DateTime.Now, -1, -1).ToList();
            return listaGrupo;
        }

        /// <summary>
        /// Reporte Html de la Relacion de Grupo y Barra
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="iCategoria"></param>
        /// <returns></returns>
        public string ReporteRelacionGrupoBarraProgHtml(string empresas, string iCategoria, string url)
        {
            StringBuilder strHtml = new StringBuilder();

            int[] empresaslistado = empresas.Split(',').Select(x => int.Parse(x)).ToArray();

            //Lista de barras de Programación
            List<CmConfigbarraDTO> listBarras = this.ListarBarraProgramacion(true);

            //Lista de grupo despacho de tipo Central
            List<PrGrupoDTO> listaGrupo = this.ListarGrupoPorCategoria(iCategoria);
            listaGrupo = listaGrupo.Where(x => x.Emprcodi != null && empresaslistado.Contains(x.Emprcodi.Value)).OrderBy(x => x.Emprnomb).ThenBy(x => x.Gruponomb).ToList();

            //Relación
            List<PrGrupoxcnfbarDTO> listaRelacion = this.ListPrGrupoxcnfbars().Where(x => x.Grcnfbestado == ConstantesMonitoreo.PrGrupoBarraprogEstadoActivo).ToList();

            strHtml.Append("<div class='freeze_table' id='resultado' style='height: auto;'>");
            strHtml.Append("<table id='reporte' class='pretty tabla-icono' style='width: auto'>");

            #region cabecera
            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.Append("<th class='despacho' colspan=4 style='width: 650px;'>Grupo de Despacho </th>");
            strHtml.Append("<th class='barra_prog' colspan=3 style='width: 400px;'>Barra de Programación </th>");
            strHtml.Append("<th style='width: 100px; word-wrap: break-word; white-space: normal'></th>");
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.Append("<th class='despacho' style='width: 50px;' >Código</th>");
            strHtml.Append("<th class='despacho' style='width: 250px;'>Empresa</th>");
            strHtml.Append("<th class='despacho' style='width: 200px;'>Tipo Central</th>");
            strHtml.Append("<th class='despacho' style='width: 250px;'>Central de Generación</th>");

            strHtml.Append("<th class='barra_prog' style='width: 50px;' >Código</th>");
            strHtml.Append("<th class='barra_prog' style='width: 165px;'>Nombre NCP</th>");
            strHtml.Append("<th class='barra_prog' style='width: 165px;'>Nombre</th>");

            strHtml.Append("<th style='width: 100px; word-wrap: break-word; white-space: normal'>Acciones</th>");
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");
            #endregion

            #region cuerpo
            strHtml.Append("<tbody>");

            foreach (var reg in listaGrupo)
            {
                string claseFila = "";
                if (reg.GrupoEstado != ConstantesAppServicio.SI) { claseFila = "clase_eliminado"; }

                strHtml.AppendFormat("<tr class='{0}'>", claseFila);
                strHtml.Append(string.Format("<td>{0}</td>", reg.Grupocodi));
                strHtml.Append(string.Format("<td>{0}</td>", reg.Emprnomb.Trim()));
                strHtml.Append(string.Format("<td>{0}</td>", reg.Catenomb));
                strHtml.Append(string.Format("<td>{0}</td>", reg.Gruponomb != null ? (reg.Gruponomb).ToString() : string.Empty));

                PrGrupoxcnfbarDTO regRelacion = listaRelacion.Find(x => x.Grupocodi == reg.Grupocodi);
                int idBarra = regRelacion != null ? regRelacion.Cnfbarcodi : 0;
                CmConfigbarraDTO barra = listBarras.Find(x => x.Cnfbarcodi == idBarra);

                if (barra != null)
                {
                    strHtml.Append(string.Format("<td>{0}</td>", barra.Cnfbarcodi));
                    strHtml.Append(string.Format("<td>{0}</td>", barra.Cnfbarnombncp));
                    strHtml.Append(string.Format("<td>{0}</td>", barra.Cnfbarnombre));
                    strHtml.Append(string.Format("<td>{0}</td>", " <a href='JavaScript:eliminarRelacion(" + reg.Grupocodi + "," + regRelacion.Grcnfbcodi + ");' title='Eliminar relacion'><img src='" + url + "Content/Images/btn-Cancel.png' alt='Eliminar relacion' /></a>"));
                }
                else
                {
                    strHtml.Append(string.Format("<td>{0}</td>", string.Empty));
                    strHtml.Append(string.Format("<td>{0}</td>", string.Empty));
                    strHtml.Append(string.Format("<td>{0}</td>", string.Empty));
                    strHtml.Append(string.Format("<td>{0}</td>", " <a href='JavaScript:agregarRelacion(" + reg.Grupocodi + ");' title='Agregar  relacion'><img src='" + url + "Content/Images/btn-add.png' alt='Agregar relacion' /></a>"));
                }

                strHtml.Append("</tr>");
            }
            strHtml.Append("</tbody>");
            #endregion

            strHtml.Append("</table>");
            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Existe algun relacion del grupo en BD
        /// </summary>
        /// <param name="grupcodi"></param>
        /// <returns></returns>
        public bool ExisteRelacionGrupoBarraProg(int grupcodi)
        {
            List<PrGrupoxcnfbarDTO> listaRel = this.ListPrGrupoxcnfbars();
            List<PrGrupoxcnfbarDTO> listaRelByGrupo = listaRel.Where(x => x.Grupocodi == grupcodi).ToList();
            return listaRelByGrupo.Count >= 1;
        }

        #endregion

        #region Actualización de Banda de Tolerancia

        /// <summary>
        /// Permite obtener un registro de la tabla MMM_BANDTOL
        /// </summary>
        public bool ExisteByIndicadorYPeriodo(int immecodi, DateTime fechaPeriodo)
        {
            MmmBandtolDTO reg = FactorySic.GetMmmBandtolRepository().GetByIndicadorYPeriodo(immecodi, fechaPeriodo);

            return reg != null;
        }

        /// <summary>
        /// Obtener banda de tolerancia vigente
        /// </summary>
        /// <param name="immecodi"></param>
        /// <param name="fechaPeriodo"></param>
        /// <returns></returns>
        public MmmBandtolDTO GetBandaToleranciaByIndicadorYPeriodo(int immecodi, DateTime fechaPeriodo)
        {
            DateTime fechaIni = new DateTime(fechaPeriodo.Year, fechaPeriodo.Month, 1);
            DateTime fechaFin = new DateTime(fechaPeriodo.Year, fechaPeriodo.Month, 1).AddMonths(1).AddDays(-1);

            List<MmmBandtolDTO> lista = this.ListMmmBandtols().Where(x => x.Immecodi == immecodi && x.Mmmtolestado == ConstantesAppServicio.Activo)
               .OrderByDescending(x => x.Mmmtolfechavigencia).ToList();

            //completar rango
            if (lista.Count > 0)
            {
                for (int i = 0; i < lista.Count; i++)
                {
                    lista[i].MmmtolfechavigenciaIni = lista[i].Mmmtolfechavigencia;
                    lista[i].MmmtolfechavigenciaIni = new DateTime(lista[i].MmmtolfechavigenciaIni.Year, lista[i].MmmtolfechavigenciaIni.Month, 1);

                    if (i == 0)
                    {
                        lista[i].MmmtolfechavigenciaFin = new DateTime(fechaPeriodo.Year + 50, fechaPeriodo.Month, 1);
                    }
                    else
                    {
                        lista[i].MmmtolfechavigenciaFin = lista[i - 1].MmmtolfechavigenciaIni.AddDays(-1);
                    }
                    lista[i].MmmtolfechavigenciaFin = new DateTime(lista[i].MmmtolfechavigenciaFin.Year, lista[i].MmmtolfechavigenciaFin.Month, 1);
                    lista[i].MmmtolfechavigenciaFin = lista[i].MmmtolfechavigenciaFin.AddMonths(1).AddDays(-1);
                }
            }

            //buscar la configuración vigente
            foreach (var param in lista)
            {
                if (param.MmmtolfechavigenciaIni.Date <= fechaIni && fechaFin <= param.MmmtolfechavigenciaFin.Date)
                {
                    return param;
                }
            }

            return null;
        }

        /// <summary>
        /// Listar Historico
        /// </summary>
        /// <returns></returns>
        public List<MmmBandtolDTO> ListarBandaToleranciaHistorico()
        {
            List<MmmBandtolDTO> lista = new List<MmmBandtolDTO>();

            List<MmmBandtolDTO> listaData = this.GetByCriteriaMmmBandtols();

            foreach (var reg in listaData)
            {
                lista.Add(this.GetBandaToleranciaFromLista(reg, this.ListarEstadoParametro()));
            }

            return lista;
        }

        /// <summary>
        /// Formatear valores de un registro de Banda
        /// </summary>
        /// <param name="reg"></param>
        /// <param name="listaEstado"></param>
        /// <returns></returns>
        public MmmBandtolDTO GetBandaToleranciaFromLista(MmmBandtolDTO reg, List<EstadoParametro> listaEstado)
        {
            reg.MmmtolfechavigenciaDesc = reg.Mmmtolfechavigencia.ToString(ConstantesAppServicio.FormatoFecha);
            reg.Periodo = reg.Mmmtolfechavigencia.ToString(ConstantesAppServicio.FormatoMes);

            if (listaEstado != null)
            {
                reg.MmmtolestadoDesc = listaEstado.Find(x => x.EstadoCodigo2 == reg.Mmmtolestado).EstadoDescripcion;
            }
            switch (reg.Mmmtolestado)
            {
                case ConstantesAppServicio.Baja:
                    reg.ClaseFila = "fila_baja";
                    break;
                case ConstantesAppServicio.Activo:
                    reg.Editable = true;
                    break;
                default:
                    reg.Editable = false;
                    reg.ClaseFila = string.Empty;
                    break;
            }

            reg.MmmtolfeccreacionDesc = reg.Mmmtolfeccreacion != null ? reg.Mmmtolfeccreacion.Value.ToString(ConstantesBase.FormatoFechaFullBase) : string.Empty;
            reg.MmmtolfecmodificacionDesc = reg.Mmmtolfecmodificacion != null ? reg.Mmmtolfecmodificacion.Value.ToString(ConstantesBase.FormatoFechaFullBase) : string.Empty;

            return reg;
        }

        #endregion

        #region Reporte de % Errores en base a Bandas de Tolerancia

        /// <summary>
        /// Permite listar todos los registros de la tabla MMM_JUSTIFICACION segun los filtros
        /// </summary>
        public List<MmmJustificacionDTO> ListMmmJustificacionByFechaAndIndicador(int immecodi, DateTime fechaIni, DateTime fechaFin)
        {
            List<MmmJustificacionDTO> lista = FactorySic.GetMmmJustificacionRepository().ListByFechaAndIndicador(immecodi, fechaIni.AddMinutes(30), fechaFin.AddDays(1));

            foreach (var reg in lista)
            {
                reg.MjustfechaDesc = reg.Mjustfecha.ToString(ConstantesAppServicio.FormatoFechaFull);
            }

            return lista;
        }

        /// <summary>
        /// Obtener Html del Indicador 1
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        /// 
        public string ReportePorcentajeErrorBT1Html(string empresas, DateTime fechaInicio, DateTime fechaFin, int vermmcodi, List<MmmDatoDTO> listFacTable, string url, out List<MmmJustificacionDTO> dataJustif)
        {
            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";

            List<SiEmpresaDTO> listaEmpresa;
            List<MeMedicion48DTO> data = this.ListarIndicador(vermmcodi, fechaInicio, fechaFin, ConstantesMonitoreo.CodigoS, listFacTable, ConstantesMonitoreo.ReportePorcentajeErrorBandaTolerancia, out listaEmpresa);
            listaEmpresa = this.ListarEmpresasMonitoreoByData(listaEmpresa, empresas);

            dataJustif = this.ListMmmJustificacionByFechaAndIndicador(ConstantesMonitoreo.CodigoS, fechaInicio, fechaFin);
            MmmBandtolDTO bt = this.GetBandaToleranciaByIndicadorYPeriodo(ConstantesMonitoreo.CodigoS, fechaInicio);

            if (listaEmpresa.Count == 0) return string.Empty;

            int padding = 20;
            int anchoTotal = (100 + padding) + listaEmpresa.Count * (200 + padding);

            strHtml.Append("<div class='freeze_table' id='resultado1' style='height: 800px;'>");
            strHtml.AppendFormat("<table id='reporte1' class='pretty tabla-icono' style='table-layout: fixed; width: {0}px;' >", anchoTotal);
            strHtml.Append("<thead>");
            #region cabecera
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width: 110px;' rowspan=2> MM/DD/hh:mm</th>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px' rowspan=2> PES </th>");
            foreach (var datos in listaEmpresa)
            {
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 100px' class='cuota_{1}'>{0}</th>", datos.Emprnomb, 1);
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 100px' class='cuota_{1}'>{0}</th>", datos.Emprnomb, 2);
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 100px' class='cuota_{1}'>{0}</th>", datos.Emprnomb, 2);
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 100px' class='cuota_{1}'>{0}</th>", datos.Emprnomb, 2);
            }
            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            foreach (var datos in listaEmpresa)
            {
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 100px' class='cuota_{1}'>{0}</th>", "S (%)", 1);
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 100px' class='cuota_{1}'>{0}</th>", " Error (%)", 2);
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 100px' class='cuota_{1}'>{0}</th>", " Tolerancia", 2);
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 100px' class='cuota_{1}'>{0}</th>", "Justificación", 2);
            }
            strHtml.Append("</tr>");
            #endregion
            strHtml.Append("</thead>");
            strHtml.Append("<tbody>");
            #region cuerpo
            for (var day = fechaInicio.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
            {
                MeMedicion48DTO total = data.Find(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaTotalPotencia);

                //HORA
                DateTime horas = day.AddMinutes(30);

                for (int h = 1; h <= 48; h++)
                {
                    strHtml.Append("<tr>");
                    strHtml.Append(string.Format("<td class='tdbody_reporte'>{0:dd/MM/yyyy HH:mm}</td>", horas));

                    //Potencia total
                    decimal? potenciaTotal = null;
                    if (total != null)
                    {
                        potenciaTotal = (decimal?)total.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(total, null);
                    }
                    strHtml.Append(string.Format("<td>{0}</td>", potenciaTotal != null ? (potenciaTotal.Value).ToString("N", nfi) : string.Empty));

                    foreach (var empresa in listaEmpresa)
                    {
                        //Cuota
                        MeMedicion48DTO reg = data.Find(x => x.Medifecha == day && x.Emprcodi == empresa.Emprcodi && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCuota);
                        decimal? pot = null;
                        if (reg != null)
                        {
                            pot = (decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg, null);
                        }
                        strHtml.Append(string.Format("<td>{0}</td>", pot != null && pot.GetValueOrDefault(0) != 0 ? (pot.Value).ToString("N", nfi) : string.Empty));

                        //Error
                        MeMedicion48DTO regError = data.Find(x => x.Medifecha == day && x.Emprcodi == empresa.Emprcodi && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCuotaErrorBT);
                        decimal? error = null;
                        if (regError != null)
                        {
                            error = (decimal?)regError.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regError, null);
                        }
                        strHtml.Append(string.Format("<td>{0}</td>", error != null ? (error.Value).ToString("N", nfi) : string.Empty));

                        ////Tolerancia
                        bool mostrarJustificacion = false;
                        if (bt != null)
                        {
                            strHtml.Append(string.Format("<td>{0}</td>", error != null ? (bt.Mmmtolvalortolerancia).ToString() : string.Empty));
                            mostrarJustificacion = error != null && bt.Mmmtolvalortolerancia < error;
                        }
                        else
                        {
                            strHtml.Append(string.Format("<td>{0}</td>", string.Empty));
                        }

                        //Justificación
                        strHtml.AppendFormat("<td class='justif-{0}'>", ConstantesMonitoreo.CodigoS);
                        if (mostrarJustificacion)
                        {
                            MmmJustificacionDTO regJustif = dataJustif.Find(x => x.Mjustfecha == horas && x.Emprcodi == empresa.Emprcodi);

                            if (regJustif != null && regJustif.Mjustdescripcion != null)
                            {
                                strHtml.AppendFormat("<img class='edit' src='" + url + "Content/Images/Pen.png' alt='Editar justificación' onclick='javascript:editJustif(this)' width='15'/>");
                                strHtml.AppendFormat("<img class='add' style='display:none' src='" + url + "Content/Images/Plus.png' alt='Agregar justificación' onclick='javascript:addJustif(this)'  width='15'/>");
                            }
                            else
                            {
                                strHtml.AppendFormat("<img class='add' src='" + url + "Content/Images/Plus.png' alt='Agregar justificación' onclick='javascript:addJustif(this)' width='15'/>");
                                strHtml.AppendFormat("<img class='edit' style='display:none' src='" + url + "Content/Images/Pen.png' alt='Editar justificación' onclick='javascript:editJustif(this)'  width='15'/>");
                            }
                        }
                        strHtml.AppendFormat("<input type='hidden' name='hfFechaJustif' value='{0:dd/MM/yyyy HH:mm}'>", horas);
                        strHtml.AppendFormat("<input type='hidden' name='hfIndicador' value='{0}'>", ConstantesMonitoreo.CodigoS);
                        strHtml.AppendFormat("<input type='hidden' name='hfEmprcodi' value='{0}'>", empresa.Emprcodi);
                        strHtml.AppendFormat("</td>");
                    }

                    strHtml.Append("</tr>");
                    horas = horas.AddMinutes(30);
                }
            }

            #endregion
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Html indicador 2
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public string ReportePorcentajeErrorBT2Html(string empresas, DateTime fechaInicio, DateTime fechaFin, int vermmcodi, List<MmmDatoDTO> listFacTable, string url, out List<MmmJustificacionDTO> dataJustif)
        {
            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";

            List<SiEmpresaDTO> listaEmpresa;
            List<MeMedicion48DTO> data = this.ListarIndicador(vermmcodi, fechaInicio, fechaFin, ConstantesMonitoreo.CodigoHHI, listFacTable, ConstantesMonitoreo.ReportePorcentajeErrorBandaTolerancia, out listaEmpresa);
            listaEmpresa = this.ListarEmpresasMonitoreoByData(listaEmpresa, empresas);

            dataJustif = this.ListMmmJustificacionByFechaAndIndicador(ConstantesMonitoreo.CodigoS, fechaInicio, fechaFin);

            SiParametroValorDTO param = this.GetParametroTendenciaHHI(fechaInicio);
            decimal tendenciaUno = param.HHITendenciaUno.GetValueOrDefault(0);
            decimal tendenciaCero = param.HHITendenciaCero.GetValueOrDefault(0);
            string colorUno = param.HHITendenciaUnoColor;
            string colorCero = param.HHITendenciaCeroColor;

            if (listaEmpresa.Count == 0) return string.Empty;

            int padding = 20;
            int anchoTotal = (100 + padding) + listaEmpresa.Count * (200 + padding);

            strHtml.Append("<div class='freeze_table' id='resultado2' style='height: auto;'>");
            strHtml.AppendFormat("<table id='reporte2' class='pretty tabla-icono' style='table-layout: fixed; width: {0}px;' >", anchoTotal);
            strHtml.Append("<thead>");
            #region cabecera
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width: 110px;' rowspan=2>MM/DD/hh:mm</th>");
            foreach (var datos in listaEmpresa)
            {
                strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px'>" + datos.Emprnomb + "</th>");
            }
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px;' rowspan=2> HHI</th>");
            //strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px;' rowspan=2> Error (%)</th>");
            strHtml.Append("</tr>");
            strHtml.Append("<tr>");

            // Cabecera cuota de mercado al cuadrado
            foreach (var datos in listaEmpresa)
            {
                strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px'> S  <sup>2</sup> </th>");
            }
            strHtml.Append("</tr>");
            #endregion
            strHtml.Append("</thead>");
            strHtml.Append("<tbody>");
            #region cuerpo
            for (var day = fechaInicio.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
            {
                MeMedicion48DTO total = data.Find(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaHHI);

                DateTime horas = day.AddMinutes(30);
                for (int h = 1; h <= 48; h++)
                {
                    strHtml.Append("<tr>");
                    strHtml.Append(string.Format("<td class='tdbody_reporte'>{0:dd/MM/yyyy HH:mm}</td>", horas));

                    foreach (var empresa in listaEmpresa)
                    {
                        // Cuota para HHI
                        MeMedicion48DTO reg = data.Find(x => x.Medifecha == day && x.Emprcodi == empresa.Emprcodi && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaHHICuotaMercado);
                        decimal? cuotaHHI = null;
                        if (reg != null)
                        {
                            cuotaHHI = (decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg, null);
                        }
                        strHtml.Append(string.Format("<td>{0}</td>", cuotaHHI.GetValueOrDefault(0) != 0 ? cuotaHHI.Value.ToString("N", nfi) : string.Empty));
                    }

                    decimal totalHhi = 0;
                    if (total != null)
                    {
                        totalHhi = ((decimal?)total.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(total, null)).GetValueOrDefault(0);
                    }

                    if (totalHhi >= tendenciaUno)
                    {
                        strHtml.AppendFormat("<td  style='background-color: {1}; color:White;'>{0}</td>", totalHhi != 0 ? totalHhi.ToString("N", nfi) : string.Empty, colorUno);
                    }
                    else if (totalHhi <= tendenciaCero)
                    {
                        strHtml.AppendFormat("<td  style='background-color: {1}; color:White;'>{0}</td>", totalHhi != 0 ? totalHhi.ToString("N", nfi) : string.Empty, colorCero);
                    }
                    else
                    {
                        strHtml.AppendFormat("<td >{0}</td>", totalHhi != 0 ? totalHhi.ToString("N", nfi) : string.Empty);
                    }

                    ////Error
                    //MeMedicion48DTO regError = data.Find(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaHHIErrorBT);
                    //decimal? error = null;
                    //if (regError != null)
                    //{
                    //    error = (decimal?)regError.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regError, null);
                    //}
                    //strHtml.Append(string.Format("<td>{0}</td>", error != null && error.GetValueOrDefault(0) != 0 ? (error.Value).ToString("N", nfi) : string.Empty));

                    strHtml.Append("</tr>");
                    horas = horas.AddMinutes(30);
                }
            }
            #endregion
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Html indicador 3
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public string ReportePorcentajeErrorBT3Html(string empresas, DateTime fechaInicio, DateTime fechaFin, int vermmcodi, List<MmmDatoDTO> listFacTable, string url, out List<MmmJustificacionDTO> dataJustif)
        {
            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";

            List<SiEmpresaDTO> listaEmpresa;
            List<MeMedicion48DTO> data = this.ListarIndicador(vermmcodi, fechaInicio, fechaFin, ConstantesMonitoreo.CodigoIOP, listFacTable, ConstantesMonitoreo.ReportePorcentajeErrorBandaTolerancia, out listaEmpresa);
            listaEmpresa = this.ListarEmpresasMonitoreoByData(listaEmpresa, empresas);

            dataJustif = this.ListMmmJustificacionByFechaAndIndicador(ConstantesMonitoreo.CodigoS, fechaInicio, fechaFin);

            SiParametroValorDTO param = this.GetParametroOfertaPivotal(fechaInicio);
            string colorEsPivotal = param.IOPEsPivotalColor;
            string colorNoPivotal = param.IOPNoPivotalColor;

            if (listaEmpresa.Count == 0) return string.Empty;

            int padding = 20;
            int anchoTotal = (100 + padding) + listaEmpresa.Count * (200 + padding);

            strHtml.Append("<div class='freeze_table' id='resultado3' style='height: auto;'>");
            strHtml.AppendFormat("<table id='reporte3' class='pretty tabla-icono' style='table-layout: fixed; width: {0}px;' >", anchoTotal);
            strHtml.Append("<thead>");
            #region cabecera
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width: 110px;' rowspan=2>MM/DD/hh:mm</th>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px' rowspan=2 > MD  (MW)</th>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px' rowspan=2> PES (MW) </th>");
            for (int i = 1; i <= 2; i++)
            {
                foreach (var datos in listaEmpresa)
                {
                    strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px'>" + datos.Emprnomb + "</th>");
                }
            }

            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            for (int i = 1; i <= 2; i++)
            {
                foreach (var datos in listaEmpresa)
                {
                    string valor; valor = (i == 1) ? "PE (MW)" : "IOP";
                    strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px'> " + valor + " </th>");
                }
            }
            strHtml.Append("</tr>");
            #endregion
            strHtml.Append("</thead>");
            strHtml.Append("<tbody>");
            #region cuerpo
            for (var day = fechaInicio.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
            {
                MeMedicion48DTO total = data.Find(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaTotalPotencia);
                MeMedicion48DTO md = data.Find(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaMaximaDemanda);

                DateTime horas = day.AddMinutes(30);
                for (int h = 1; h <= 48; h++)
                {
                    strHtml.Append("<tr>");
                    strHtml.Append(string.Format("<td class='tdbody_reporte'>{0:dd/MM/yyyy HH:mm}</td>", horas));

                    //Maxima Demanda Programada
                    decimal? maximademanda = null;
                    if (md != null)
                    {
                        maximademanda = (decimal?)md.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(md, null);
                    }
                    strHtml.Append(string.Format("<td>{0}</td>", maximademanda != null ? (maximademanda.Value).ToString("N", nfi) : string.Empty));

                    //Potencia Total
                    decimal? potenciaTotal = null;
                    if (total != null)
                    {
                        potenciaTotal = (decimal?)total.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(total, null);
                    }
                    strHtml.Append(string.Format("<td>{0}</td>", potenciaTotal != null ? (potenciaTotal.Value).ToString("N", nfi) : string.Empty));

                    //Potencia x Empresa
                    foreach (var empresa in listaEmpresa)
                    {
                        MeMedicion48DTO reg = data.Find(x => x.Medifecha == day && x.Emprcodi == empresa.Emprcodi && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaMWEjec);
                        decimal? pot = null;
                        if (reg != null)
                        {
                            pot = (decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg, null);
                        }
                        strHtml.Append(string.Format("<td>{0}</td>", pot != null ? (pot.Value).ToString("N", nfi) : string.Empty));
                    }

                    //IOP x Empresa
                    foreach (var empresa in listaEmpresa)
                    {
                        MeMedicion48DTO reg = data.Find(x => x.Medifecha == day && x.Emprcodi == empresa.Emprcodi && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaIOP);
                        int iop = ConstantesMonitoreo.ValorIOPEsPivotal;

                        if (reg != null)
                        {
                            iop = Convert.ToInt32(((decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg, null)).GetValueOrDefault(0));
                        }

                        string color = ConstantesMonitoreo.ValorIOPEsPivotal == iop ? colorEsPivotal : colorNoPivotal;
                        strHtml.AppendFormat("<td style='background-color: {1}; color:White;'>{0}</td>", iop, color);
                    }
                    strHtml.Append("</tr>");
                    horas = horas.AddMinutes(30);
                }
            }
            #endregion
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        /// <summary>
        ///  Html indicador 4
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public string ReportePorcentajeErrorBT4Html(string empresas, DateTime fechaInicio, DateTime fechaFin, int vermmcodi, List<MmmDatoDTO> listFacTable, string url, out List<MmmJustificacionDTO> dataJustif)
        {
            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";

            List<SiEmpresaDTO> listaEmpresa;
            List<MeMedicion48DTO> data = this.ListarIndicador(vermmcodi, fechaInicio, fechaFin, ConstantesMonitoreo.CodigoRSD, listFacTable, ConstantesMonitoreo.ReportePorcentajeErrorBandaTolerancia, out listaEmpresa);
            listaEmpresa = this.ListarEmpresasMonitoreoByData(listaEmpresa, empresas);

            dataJustif = this.ListMmmJustificacionByFechaAndIndicador(ConstantesMonitoreo.CodigoRSD, fechaInicio, fechaFin);
            MmmBandtolDTO bt = this.GetBandaToleranciaByIndicadorYPeriodo(ConstantesMonitoreo.CodigoRSD, fechaInicio);

            if (listaEmpresa.Count == 0) return string.Empty;

            int padding = 20;
            int anchoTotal = (100 + padding) + listaEmpresa.Count * (200 + padding);

            strHtml.Append("<div class='freeze_table' id='resultado4' style='height: auto;'>");
            strHtml.AppendFormat("<table id='reporte4' class='pretty tabla-icono' style='table-layout: fixed; width: {0}px;' >", anchoTotal);
            strHtml.Append("<thead>");
            #region cabecera
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width: 110px;' rowspan=2>MM/DD/hh:mm</th>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px' rowspan=2 > MD  (MW)</th>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px' rowspan=2> PES (MW) </th>");
            foreach (var datos in listaEmpresa)
            {
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 100px' class='residual_{1}'>{0}</th>", datos.Emprnomb, 1);
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 100px' class='residual_{1}'>{0}</th>", datos.Emprnomb, 2);
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 100px' class='residual_{1}'>{0}</th>", datos.Emprnomb, 2);
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 100px' class='residual_{1}'>{0}</th>", datos.Emprnomb, 2);

            }
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            foreach (var datos in listaEmpresa)
            {
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 100px' class='residual_{1}'>{0}</th>", "RSD", 1);
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 100px' class='residual_{1}'>{0}</th>", "Error (%)", 2);
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 100px' class='residual_{1}'>{0}</th>", " Tolerancia", 2);
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 100px' class='residual_{1}'>{0}</th>", "Justificación", 2);
            }
            strHtml.Append("</tr>");
            #endregion
            strHtml.Append("</thead>");
            strHtml.Append("<tbody>");
            #region cuerpo
            for (var day = fechaInicio.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
            {
                MeMedicion48DTO total = data.Find(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaTotalPotencia);
                MeMedicion48DTO md = data.Find(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaMaximaDemanda);

                DateTime horas = day.AddMinutes(30);
                for (int h = 1; h <= 48; h++)
                {
                    strHtml.Append("<tr>");
                    strHtml.Append(string.Format("<td class='tdbody_reporte'>{0:dd/MM/yyyy HH:mm}</td>", horas));

                    //Maxima Demanda Programada
                    decimal? maximademanda = null;
                    if (md != null)
                    {
                        maximademanda = (decimal?)md.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(md, null);
                    }
                    strHtml.Append(string.Format("<td>{0}</td>", maximademanda != null ? (maximademanda.Value).ToString("N", nfi) : string.Empty));

                    //Potencia Total
                    decimal? potenciaTotal = null;
                    if (total != null)
                    {
                        potenciaTotal = (decimal?)total.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(total, null);
                    }
                    strHtml.Append(string.Format("<td>{0}</td>", potenciaTotal != null ? (potenciaTotal.Value).ToString("N", nfi) : string.Empty));

                    //RSD x Empresa
                    foreach (var empresa in listaEmpresa)
                    {
                        MeMedicion48DTO reg = data.Find(x => x.Medifecha == day && x.Emprcodi == empresa.Emprcodi && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaRSD);
                        decimal? ior = null;

                        if (reg != null)
                        {
                            ior = ((decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg, null)).GetValueOrDefault(0);
                        }

                        strHtml.AppendFormat("<td >{0}</td>", ior.GetValueOrDefault(0) != 0 ? ior.Value.ToString("N", nfi) : string.Empty);

                        //Error
                        MeMedicion48DTO regError = data.Find(x => x.Medifecha == day && x.Emprcodi == empresa.Emprcodi && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaRSDErrorBT);
                        decimal? error = null;
                        if (regError != null)
                        {
                            error = (decimal?)regError.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regError, null);
                        }
                        strHtml.Append(string.Format("<td>{0}</td>", error != null ? (error.Value).ToString("N", nfi) : string.Empty));

                        ////Tolerancia
                        bool mostrarJustificacion = false;
                        if (bt != null)
                        {
                            strHtml.Append(string.Format("<td>{0}</td>", error != null ? (bt.Mmmtolvalortolerancia).ToString() : string.Empty));
                            mostrarJustificacion = error != null && bt.Mmmtolvalortolerancia < error;
                        }
                        else
                        {
                            strHtml.Append(string.Format("<td>{0}</td>", string.Empty));
                        }

                        //Justificación
                        strHtml.AppendFormat("<td class='justif-{0}'>", ConstantesMonitoreo.CodigoRSD);
                        if (mostrarJustificacion)
                        {
                            MmmJustificacionDTO regJustif = dataJustif.Find(x => x.Mjustfecha == horas && x.Emprcodi == empresa.Emprcodi);

                            if (regJustif != null && regJustif.Mjustdescripcion != null)
                            {
                                strHtml.AppendFormat("<img class='edit' src='" + url + "Content/Images/Pen.png' alt='Editar justificación' onclick='javascript:editJustif(this)' width='15'/>");
                                strHtml.AppendFormat("<img class='add' style='display:none' src='" + url + "Content/Images/Plus.png' alt='Agregar justificación' onclick='javascript:addJustif(this)'  width='15'/>");
                            }
                            else
                            {
                                strHtml.AppendFormat("<img class='add' src='" + url + "Content/Images/Plus.png' alt='Agregar justificación' onclick='javascript:addJustif(this)' width='15'/>");
                                strHtml.AppendFormat("<img class='edit' style='display:none' src='" + url + "Content/Images/Pen.png' alt='Editar justificación' onclick='javascript:editJustif(this)'  width='15'/>");
                            }
                        }
                        strHtml.AppendFormat("<input type='hidden' name='hfFechaJustif' value='{0:dd/MM/yyyy HH:mm}'>", horas);
                        strHtml.AppendFormat("<input type='hidden' name='hfIndicador' value='{0}'>", ConstantesMonitoreo.CodigoRSD);
                        strHtml.AppendFormat("<input type='hidden' name='hfEmprcodi' value='{0}'>", empresa.Emprcodi);
                        strHtml.AppendFormat("</td>");
                        strHtml.AppendFormat("<input type='hidden' name='hfFechaJustif' value='{0:dd/MM/yyyy HH:mm}'>", horas);
                        strHtml.AppendFormat("<input type='hidden' name='hfIndicador' value='{0}'>", ConstantesMonitoreo.CodigoS);
                        strHtml.AppendFormat("<input type='hidden' name='hfEmprcodi' value='{0}'>", empresa.Emprcodi);
                        strHtml.AppendFormat("</td>");

                    }
                    strHtml.Append("</tr>");
                    horas = horas.AddMinutes(30);
                }
            }
            #endregion
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        /// <summary>
        ///  Html indicador 5
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public string ReportePorcentajeErrorBT5Html(string empresas, DateTime fechaInicio, DateTime fechaFin, int vermmcodi, List<MmmDatoDTO> listFacTable, string url, out List<MmmJustificacionDTO> dataJustif)
        {
            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";

            List<SiEmpresaDTO> listaEmpresa;
            List<MeMedicion48DTO> data = this.ListarIndicador(vermmcodi, fechaInicio, fechaFin, ConstantesMonitoreo.CodigoILE, listFacTable, ConstantesMonitoreo.ReportePorcentajeErrorBandaTolerancia, out listaEmpresa);
            listaEmpresa = this.ListarEmpresasMonitoreoByData(listaEmpresa, empresas);
            dataJustif = this.ListMmmJustificacionByFechaAndIndicador(ConstantesMonitoreo.CodigoILE, fechaInicio, fechaFin);
            MmmBandtolDTO bt = this.GetBandaToleranciaByIndicadorYPeriodo(ConstantesMonitoreo.CodigoILE, fechaInicio);

            List<MeMedicion48DTO> listaBarra = data.Where(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCMEjec)
                .Select(x => new { Emprcodi = x.Emprcodi, Barrcodi = x.Barrcodi, Barrnombre = x.Barrnombre })
                .GroupBy(x => new { x.Emprcodi, x.Barrcodi, x.Barrnombre })
                .Select(x => new MeMedicion48DTO() { Emprcodi = x.Key.Emprcodi, Barrcodi = x.Key.Barrcodi, Barrnombre = x.Key.Barrnombre })
                .OrderBy(x => x.Barrnombre).ToList();

            int totalBarra = listaBarra.Count();
            totalBarra = totalBarra + 1;

            if (listaEmpresa.Count == 0) return string.Empty;

            int padding = 20;
            int anchoCol = 100;
            int anchoTotal = (100 + padding) + totalBarra * 6 * (anchoCol + 50 + padding);

            strHtml.Append("<div class='freeze_table' id='resultado5' style='height: auto;'>");
            strHtml.Append("<div  id='resultado5' style='height: auto; width:auto;'>");
            strHtml.AppendFormat("<table id='reporte5' class='pretty tabla-icono' style='table-layout: fixed; width: {0}px;' >", anchoTotal);
            strHtml.Append("<thead>");
            #region cabecera
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width: 110px;' rowspan='3'> MM/DD/hh:mm </th>");

            //Empresas
            int contEmpresa = 1;
            foreach (var empresa in listaEmpresa)
            {
                int totalBarraXEmpr = listaBarra.Where(x => x.Emprcodi == empresa.Emprcodi).Count();
                totalBarraXEmpr = totalBarraXEmpr > 0 ? totalBarraXEmpr : 1;

                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal; width: {2}px;' colspan ='{1}' class='cmg_{3}'>{0}</th>"
                    , empresa.Emprnomb, 4 * totalBarraXEmpr, 2 * totalBarraXEmpr * anchoCol, contEmpresa % 2);
                contEmpresa++;
            }

            strHtml.Append("</tr>");
            strHtml.Append("<tr>");

            //Barra X Empresa
            contEmpresa = 1;
            foreach (var empresa in listaEmpresa)
            {
                var listaBarraXEmpr = listaBarra.Where(x => x.Emprcodi == empresa.Emprcodi).ToList();
                int totalBarraXEmpr = listaBarraXEmpr.Count();
                if (totalBarraXEmpr > 0)
                {
                    foreach (var barra in listaBarraXEmpr)
                    {
                        strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal; width: {2}px;'  colspan ='{1}' class='cmg_{3}'>{0}</th>"
                            , barra.Barrnombre, 4, anchoCol * 2, contEmpresa % 2);
                    }
                }
                else
                {
                    strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal; width: {2}px;'  colspan ='{1}' class='cmg_{3}'>{0}</th>"
                        , "", 4, anchoCol * 2, contEmpresa % 2);
                }
                contEmpresa++;
            }

            strHtml.Append("</tr>");
            strHtml.Append("<tr>");

            //Valores x Barra
            contEmpresa = 1;
            foreach (var empresa in listaEmpresa)
            {
                int totalBarraXEmpr = listaBarra.Where(x => x.Emprcodi == empresa.Emprcodi).Count();
                totalBarraXEmpr = totalBarraXEmpr > 0 ? totalBarraXEmpr : 1;
                for (int j = 0; j < totalBarraXEmpr; j++)
                {
                    for (int i = 1; i <= 4; i++)
                    {
                        string valor = (i == 1) ? "ILE  <br>  (Índice de Lerner)" : i == 2 ? "Error (%)" : i == 3 ? "Tolerancia" : "Justificación";
                        strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal; width: {1}px;'  class='cmg_{2}'>{0}</th>"
                            , valor, anchoCol, contEmpresa % 2);
                    }
                }
                contEmpresa++;
            }

            strHtml.Append("</tr>");
            #endregion
            strHtml.Append("</thead>");
            #region cuerpo
            strHtml.Append("<tbody>");
            for (var day = fechaInicio.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
            {
                List<MeMedicion48DTO> dataILE = data.Where(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaILE).ToList();
                List<MeMedicion48DTO> dataILEErrorBT = data.Where(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaILEErrorBT).ToList();

                DateTime horas = day.AddMinutes(30);
                for (int h = 1; h <= 48; h++)
                {
                    strHtml.Append("<tr>");

                    strHtml.Append(string.Format("<td>{0:dd/MM/yyyy HH:mm}</td>", horas));

                    foreach (var empresa in listaEmpresa)
                    {
                        var listaBarraXEmpr = listaBarra.Where(x => x.Emprcodi == empresa.Emprcodi).ToList();
                        int totalBarraXEmpr = listaBarraXEmpr.Count();
                        if (totalBarraXEmpr > 0)
                        {
                            foreach (var barra in listaBarraXEmpr)
                            {
                                MeMedicion48DTO objile = dataILE.Find(x => x.Medifecha == day && x.Barrcodi == barra.Barrcodi && x.Emprcodi == barra.Emprcodi);

                                decimal? valorIle = null;
                                bool tieneGeneracion = false;

                                if (objile != null)
                                {
                                    valorIle = (decimal?)objile.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(objile, null);
                                    tieneGeneracion = objile.TieneIndicador;
                                }

                                strHtml.AppendFormat("<td>{0}</td>", !tieneGeneracion ? string.Empty : valorIle != null ? (valorIle.Value).ToString("N", nfi) : "(*)");

                                //Error
                                MeMedicion48DTO regError = dataILEErrorBT.Find(x => x.Medifecha == day && x.Emprcodi == empresa.Emprcodi && x.Barrcodi == barra.Barrcodi);
                                decimal? error = null;
                                if (regError != null)
                                {
                                    error = (decimal?)regError.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regError, null);
                                }
                                strHtml.AppendFormat("<td>{0}</td>", !tieneGeneracion || error == null ? string.Empty : (error.Value).ToString("N", nfi));

                                ////Tolerancia
                                bool mostrarJustificacion = false;
                                if (bt != null)
                                {
                                    strHtml.Append(string.Format("<td>{0}</td>", error != null ? (bt.Mmmtolvalortolerancia).ToString() : string.Empty));
                                    mostrarJustificacion = error != null && bt.Mmmtolvalortolerancia < error;
                                }
                                else
                                {
                                    strHtml.Append(string.Format("<td>{0}</td>", string.Empty));
                                }

                                //Justificación
                                strHtml.AppendFormat("<td class='justif-{0}'>", ConstantesMonitoreo.CodigoILE);
                                if (mostrarJustificacion)
                                {
                                    MmmJustificacionDTO regJustif = dataJustif.Find(x => x.Mjustfecha == horas && x.Emprcodi == empresa.Emprcodi && x.Barrcodi == barra.Barrcodi);

                                    if (regJustif != null && regJustif.Mjustdescripcion != null)
                                    {
                                        strHtml.AppendFormat("<img class='edit' src='" + url + "Content/Images/Pen.png' alt='Editar justificación' onclick='javascript:editJustif(this)' width='15'/>");
                                        strHtml.AppendFormat("<img class='add' style='display:none' src='" + url + "Content/Images/Plus.png' alt='Agregar justificación' onclick='javascript:addJustif(this)'  width='15'/>");
                                    }
                                    else
                                    {
                                        strHtml.AppendFormat("<img class='add' src='" + url + "Content/Images/Plus.png' alt='Agregar justificación' onclick='javascript:addJustif(this)' width='15'/>");
                                        strHtml.AppendFormat("<img class='edit' style='display:none' src='" + url + "Content/Images/Pen.png' alt='Editar justificación' onclick='javascript:editJustif(this)'  width='15'/>");
                                    }
                                }
                                strHtml.AppendFormat("<input type='hidden' name='hfFechaJustif' value='{0:dd/MM/yyyy HH:mm}'>", horas);
                                strHtml.AppendFormat("<input type='hidden' name='hfIndicador' value='{0}'>", ConstantesMonitoreo.CodigoILE);
                                strHtml.AppendFormat("<input type='hidden' name='hfEmprcodi' value='{0}'>", empresa.Emprcodi);
                                strHtml.AppendFormat("<input type='hidden' name='hfBarrcodi' value='{0}'>", barra.Barrcodi);
                                strHtml.AppendFormat("</td>");
                            }
                        }
                        else
                        {
                            strHtml.Append("<td></td>");
                            strHtml.Append("<td></td>");
                            strHtml.Append("<td></td>");
                            strHtml.Append("<td></td>");
                        }
                    }
                    strHtml.Append("</tr>");
                    horas = horas.AddMinutes(30);
                }
            }

            #endregion
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        /// <summary>
        ///  Html indicador 6
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public string ReportePorcentajeErrorBT6Html(string empresas, DateTime fechaInicio, DateTime fechaFin, int vermmcodi, List<MmmDatoDTO> listFacTable, string url, out List<MmmJustificacionDTO> dataJustif)
        {
            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";

            List<SiEmpresaDTO> listaEmpresa;
            List<MeMedicion48DTO> data = this.ListarIndicador(vermmcodi, fechaInicio, fechaFin, ConstantesMonitoreo.CodigoIMU, listFacTable, ConstantesMonitoreo.ReportePorcentajeErrorBandaTolerancia, out listaEmpresa);
            listaEmpresa = this.ListarEmpresasMonitoreoByData(listaEmpresa, empresas);

            dataJustif = this.ListMmmJustificacionByFechaAndIndicador(ConstantesMonitoreo.CodigoIMU, fechaInicio, fechaFin);
            MmmBandtolDTO bt = this.GetBandaToleranciaByIndicadorYPeriodo(ConstantesMonitoreo.CodigoIMU, fechaInicio);

            List<MeMedicion48DTO> listaBarra = data.Where(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCMEjec)
                .Select(x => new { Emprcodi = x.Emprcodi, Barrcodi = x.Barrcodi, Barrnombre = x.Barrnombre })
                .GroupBy(x => new { x.Emprcodi, x.Barrcodi, x.Barrnombre })
                .Select(x => new MeMedicion48DTO() { Emprcodi = x.Key.Emprcodi, Barrcodi = x.Key.Barrcodi, Barrnombre = x.Key.Barrnombre })
                .OrderBy(x => x.Barrnombre).ToList();

            int totalBarra = listaBarra.Count();
            totalBarra = totalBarra + 1;

            if (listaEmpresa.Count == 0) return string.Empty;

            int padding = 20;
            int anchoCol = 100;
            int anchoTotal = (100 + padding) + totalBarra * 3 * (anchoCol + 50 + padding);

            strHtml.Append("<div class='freeze_table' id='resultado6' style='height: auto;'>");
            strHtml.Append("<div  id='resultado6' style='height: auto; width:auto;'>");
            strHtml.AppendFormat("<table id='reporte6' class='pretty tabla-icono' style='table-layout: fixed; width: {0}px;' >", anchoTotal);
            strHtml.Append("<thead>");
            #region cabecera
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width: 110px;' rowspan='3'> MM/DD/hh:mm </th>");

            //Empresas
            int contEmpresa = 1;
            foreach (var empresa in listaEmpresa)
            {
                int totalBarraXEmpr = listaBarra.Where(x => x.Emprcodi == empresa.Emprcodi).Count();
                totalBarraXEmpr = totalBarraXEmpr > 0 ? totalBarraXEmpr : 1;

                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal; width: {2}px;' colspan ='{1}' class='cmg_{3}'>{0}</th>"
                    , empresa.Emprnomb, 4 * totalBarraXEmpr, 2 * totalBarraXEmpr * anchoCol, contEmpresa % 2);
                contEmpresa++;
            }

            strHtml.Append("</tr>");
            strHtml.Append("<tr>");

            //Barra X Empresa
            contEmpresa = 1;
            foreach (var empresa in listaEmpresa)
            {
                var listaBarraXEmpr = listaBarra.Where(x => x.Emprcodi == empresa.Emprcodi).ToList();
                int totalBarraXEmpr = listaBarraXEmpr.Count();
                if (totalBarraXEmpr > 0)
                {
                    foreach (var barra in listaBarraXEmpr)
                    {
                        strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal; width: {2}px;'  colspan ='{1}' class='cmg_{3}'>{0}</th>"
                            , barra.Barrnombre, 4, anchoCol * 2, contEmpresa % 2);
                    }
                }
                else
                {
                    strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal; width: {2}px;'  colspan ='{1}' class='cmg_{3}'>{0}</th>"
                        , "", 4, anchoCol * 2, contEmpresa % 2);
                }
                contEmpresa++;
            }

            strHtml.Append("</tr>");
            strHtml.Append("<tr>");

            //Valores x Barra
            contEmpresa = 1;


            foreach (var empresa in listaEmpresa)
            {
                int totalBarraXEmpr = listaBarra.Where(x => x.Emprcodi == empresa.Emprcodi).Count();
                totalBarraXEmpr = totalBarraXEmpr > 0 ? totalBarraXEmpr : 1;
                for (int j = 0; j < totalBarraXEmpr; j++)
                {
                    for (int i = 1; i <= 4; i++)
                    {
                        string valor = (i == 1) ? "IMU  <br>  (Índice de Mark Up)" : i == 2 ? "Error (%)" : i == 3 ? "Tolerancia" : "Justificación";
                        strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal; width: {1}px;'  class='cmg_{2}'>{0}</th>"
                            , valor, anchoCol, contEmpresa % 2);
                    }
                }
                contEmpresa++;
            }



            strHtml.Append("</tr>");
            #endregion
            strHtml.Append("</thead>");
            #region cuerpo
            strHtml.Append("<tbody>");
            for (var day = fechaInicio.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
            {
                List<MeMedicion48DTO> dataIMU = data.Where(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaIMU).ToList();
                List<MeMedicion48DTO> dataIMUErrorBT = data.Where(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaIMUErrorBT).ToList();

                DateTime horas = day.AddMinutes(30);
                for (int h = 1; h <= 48; h++)
                {
                    strHtml.Append("<tr>");

                    strHtml.Append(string.Format("<td>{0:dd/MM/yyyy HH:mm}</td>", horas));

                    foreach (var empresa in listaEmpresa)
                    {
                        var listaBarraXEmpr = listaBarra.Where(x => x.Emprcodi == empresa.Emprcodi).ToList();
                        int totalBarraXEmpr = listaBarraXEmpr.Count();
                        if (totalBarraXEmpr > 0)
                        {
                            foreach (var barra in listaBarraXEmpr)
                            {
                                MeMedicion48DTO objimu = dataIMU.Find(x => x.Medifecha == day && x.Barrcodi == barra.Barrcodi && x.Emprcodi == barra.Emprcodi);

                                decimal? valorImu = null;
                                bool tieneGeneracion = false;

                                if (objimu != null)
                                {
                                    valorImu = (decimal?)objimu.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(objimu, null);
                                    tieneGeneracion = objimu.TieneIndicador;
                                }

                                strHtml.Append(string.Format("<td>{0}</td>", !tieneGeneracion ? string.Empty : valorImu != null ? (valorImu.Value).ToString("N", nfi) : "(*)"));

                                //Error
                                MeMedicion48DTO regError = dataIMUErrorBT.Find(x => x.Medifecha == day && x.Barrcodi == barra.Barrcodi && x.Emprcodi == empresa.Emprcodi);
                                decimal? error = null;
                                if (regError != null)
                                {
                                    error = (decimal?)regError.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regError, null);
                                }
                                strHtml.AppendFormat("<td>{0}</td>", !tieneGeneracion || error == null ? string.Empty : (error.Value).ToString("N", nfi));

                                ////Tolerancia
                                bool mostrarJustificacion = false;
                                if (bt != null)
                                {
                                    strHtml.Append(string.Format("<td>{0}</td>", error != null ? (bt.Mmmtolvalortolerancia).ToString() : string.Empty));
                                    mostrarJustificacion = error != null && bt.Mmmtolvalortolerancia < error;
                                }
                                else
                                {
                                    strHtml.Append(string.Format("<td>{0}</td>", string.Empty));
                                }

                                //Justificación
                                strHtml.AppendFormat("<td class='justif-{0}'>", ConstantesMonitoreo.CodigoIMU);
                                if (mostrarJustificacion)
                                {
                                    MmmJustificacionDTO regJustif = dataJustif.Find(x => x.Mjustfecha == horas && x.Emprcodi == empresa.Emprcodi);

                                    if (regJustif != null && regJustif.Mjustdescripcion != null)
                                    {
                                        strHtml.AppendFormat("<img class='edit' src='" + url + "Content/Images/Pen.png' alt='Editar justificación' onclick='javascript:editJustif(this)' width='15'/>");
                                        strHtml.AppendFormat("<img class='add' style='display:none' src='" + url + "Content/Images/Plus.png' alt='Agregar justificación' onclick='javascript:addJustif(this)'  width='15'/>");
                                    }
                                    else
                                    {
                                        strHtml.AppendFormat("<img class='add' src='" + url + "Content/Images/Plus.png' alt='Agregar justificación' onclick='javascript:addJustif(this)' width='15'/>");
                                        strHtml.AppendFormat("<img class='edit' style='display:none' src='" + url + "Content/Images/Pen.png' alt='Editar justificación' onclick='javascript:editJustif(this)'  width='15'/>");
                                    }
                                }
                                strHtml.AppendFormat("<input type='hidden' name='hfFechaJustif' value='{0:dd/MM/yyyy HH:mm}'>", horas);
                                strHtml.AppendFormat("<input type='hidden' name='hfIndicador' value='{0}'>", ConstantesMonitoreo.CodigoIMU);
                                strHtml.AppendFormat("<input type='hidden' name='hfEmprcodi' value='{0}'>", empresa.Emprcodi);
                                strHtml.AppendFormat("<input type='hidden' name='hfBarrcodi' value='{0}'>", barra.Barrcodi);
                                strHtml.AppendFormat("</td>");
                            }
                        }
                        else
                        {
                            strHtml.Append("<td></td>");
                            strHtml.Append("<td></td>");
                            strHtml.Append("<td></td>");
                            strHtml.Append("<td></td>");
                        }
                    }
                    strHtml.Append("</tr>");
                    horas = horas.AddMinutes(30);
                }
            }

            #endregion
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Html indicador 7
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="fechaHist"></param>
        /// <param name="generacion"></param>
        /// <returns></returns>
        public string ReportePorcentajeErrorBT7Html(string empresas, DateTime fechaInicio, DateTime fechaFin, int vermmcodi, List<MmmDatoDTO> listFacTable, string url, out List<MmmJustificacionDTO> dataJustif)
        {
            StringBuilder strHtml = new StringBuilder();

            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";

            List<SiEmpresaDTO> listaEmpresa;
            List<MeMedicion48DTO> data = this.ListarIndicador(vermmcodi, fechaInicio, fechaFin, ConstantesMonitoreo.CodigoIRT, listFacTable, ConstantesMonitoreo.ReportePorcentajeErrorBandaTolerancia, out listaEmpresa);
            listaEmpresa = this.ListarEmpresasMonitoreo(fechaInicio, empresas);

            dataJustif = this.ListMmmJustificacionByFechaAndIndicador(ConstantesMonitoreo.CodigoIRT, fechaInicio, fechaFin);
            MmmBandtolDTO bt = this.GetBandaToleranciaByIndicadorYPeriodo(ConstantesMonitoreo.CodigoIRT, fechaInicio);

            List<int> listaEmprcodi = listaEmpresa.Select(x => x.Emprcodi).ToList();

            //Congestiones
            List<MeMedicion48DTO> listaCongXAreaXFecha = data.Where(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCongestion
                && listaEmprcodi.Contains(x.Emprcodi)).ToList();
            listaCongXAreaXFecha = this.ListarCongXAreaYFechaReporte7(listaCongXAreaXFecha);

            //Grupos despacho
            List<MeMedicion48DTO> listaBarraEmpGrupo = data.Where(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaEnlaceTrans
                && listaEmprcodi.Contains(x.Emprcodi)).ToList();

            if (listaBarraEmpGrupo.Count == 0)
            {
                listaBarraEmpGrupo.Add(new MeMedicion48DTO() { Grupocodi = -1, Gruponomb = string.Empty, Grupopadre = -2, Central = string.Empty, Barrcodi = -1, Barrnombre = string.Empty });
            }

            #region Lista de Congestiones

            strHtml.Append("<div class='freeze_table' id='resultadocong7' style='height: auto;'>");
            strHtml.AppendFormat("<table id='reportecong7' class='pretty tabla-icono' style='table-layout: fixed; width: {0}px;' >", 800);
            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 80px'>" + "FECHA" + "</th>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 80px'>" + "INICIO" + "</th>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 80px'>" + "FINAL" + "</th>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px'>" + "UBICACIÓN" + "</th>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px'>" + "EQUIPO" + "</th>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 150px'>" + "OBSERVACIONES" + "</th>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 200px'>" + "GENERACIÓN CON UNA LOCALIZACIÓN ESPECÍFICA, QUE OBTENGAN PODER DE MERCADO" + "</th>");
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");

            strHtml.Append("<tbody>");

            foreach (var reg in listaCongXAreaXFecha)
            {
                strHtml.Append("<tr>");
                strHtml.AppendFormat("<td>{0}</td>", reg.Hophorini.ToString(ConstantesAppServicio.FormatoFecha));
                strHtml.AppendFormat("<td>{0}</td>", reg.Hophorini.ToString(ConstantesAppServicio.FormatoOnlyHora));
                strHtml.AppendFormat("<td>{0}</td>", reg.Hophorfin.ToString(ConstantesAppServicio.FormatoOnlyHora));
                strHtml.AppendFormat("<td>{0}</td>", reg.Areanomb != null ? (reg.Areanomb) : string.Empty);
                strHtml.AppendFormat("<td>{0}</td>", reg.Equinomb);
                strHtml.AppendFormat("<td>{0}</td>", reg.Descripcion);
                strHtml.AppendFormat("<td>{0}</td>", reg.Gruponomb);
                strHtml.Append("</tr>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            strHtml.Append("</div>");

            #endregion

            strHtml.Append("<br/>");
            strHtml.Append("<br/>");

            #region Reporte
            int padding = 20;
            int columnas = listaBarraEmpGrupo.Count * 5;
            int anchoTotalCong = (100 + padding) + (columnas) * (150 + padding);

            strHtml.Append("<div class='freeze_table' id='resultado7' style='height: auto;'>");

            strHtml.AppendFormat("<table id='reporte7' class='pretty tabla-icono' style='table-layout: fixed; width: {0}px;' >", anchoTotalCong);
            strHtml.Append("<thead>");
            #region Cabecera
            strHtml.Append("<tr>");
            strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px' rowspan=4>" + "MM/DD/hh:mm" + "</th>");

            //Enlace de transmision
            foreach (var grupo in listaBarraEmpGrupo)
            {
                strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 500px' colspan='4'>Enlace de transmisión</th>");
                strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px' rowspan='4'>" + "IRT" + "</th>");
                strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px' rowspan='4'>" + "Error (%)" + "</th>");
                strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px' rowspan='4'>" + "Tolerancia" + "</th>");
                strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px' rowspan='4'>" + "Justificación" + "</th>");
            }

            strHtml.Append("</tr>");

            //Lineas, trafos
            strHtml.Append("<tr>");
            foreach (var grupo in listaBarraEmpGrupo)
            {
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 500px' colspan='4'>{0}</th>", grupo.Equinomb);
            }
            strHtml.Append("</tr>");

            //Grupo, Barra
            strHtml.Append("<tr>");

            foreach (var grupo in listaBarraEmpGrupo)
            {
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 100px'>{0}</th>", grupo.Gruponomb);
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 100px'>{0}</th>", grupo.Gruponomb);
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 100px'>{0}</th>", grupo.Barrnombre);
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 100px'>{0}</th>", grupo.Barrnombre);
            }

            strHtml.Append("</tr>");

            //Datos
            strHtml.Append("<tr>");
            foreach (var grupo in listaBarraEmpGrupo)
            {
                strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px'> PU (MW)</th>");
                strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px'> PP (MW)</th>");
                strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px'> CMg (S/ MWh)</th>");
                strHtml.Append("<th style='word-wrap: break-word; white-space: normal;width: 100px'> CMgprog (S/ MWh)</th>");

            }
            strHtml.Append("</tr>");
            #endregion
            strHtml.Append("</thead>");
            strHtml.Append("<tbody>");
            #region Cuerpo
            for (var day = fechaInicio.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
            {
                List<MeMedicion48DTO> dataXDia = data.Where(x => x.Medifecha == day).ToList();

                DateTime horas = day.AddMinutes(30);

                for (int h = 1; h <= 48; h++)
                {
                    strHtml.Append("<tr>");
                    strHtml.Append(string.Format("<td class='tdbody_reporte'>{0:dd/MM/yyyy HH:mm}</td>", horas));

                    foreach (var grupo in listaBarraEmpGrupo)
                    {
                        MeMedicion48DTO regIRT = dataXDia.Find(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaIRT && x.Grupocodi == grupo.Grupocodi);
                        if (regIRT != null)
                        {
                            decimal? irt = (decimal?)regIRT.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regIRT, null);
                            if (irt != null)
                            {
                                //Potencia ejecutada
                                MeMedicion48DTO regpotEjec = dataXDia.Find(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaPotGrupoEjec && x.Grupocodi == grupo.Grupocodi);
                                decimal? potEjec = null;
                                if (regpotEjec != null)
                                {
                                    potEjec = (decimal?)regpotEjec.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regpotEjec, null);
                                }
                                strHtml.Append(string.Format("<td>{0}</td>", potEjec.GetValueOrDefault(0) != 0 ? (potEjec.Value).ToString("N", nfi) : string.Empty));

                                //Potencia programada
                                MeMedicion48DTO regpotProg = dataXDia.Find(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaPotGrupoProg && x.Grupocodi == grupo.Grupocodi);
                                decimal? potProg = null;
                                if (regpotProg != null)
                                {
                                    potProg = (decimal?)regpotProg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regpotProg, null);
                                }
                                strHtml.Append(string.Format("<td>{0}</td>", potProg.GetValueOrDefault(0) != 0 ? (potProg.Value).ToString("N", nfi) : string.Empty));

                                //Costos marg Ejecutados
                                MeMedicion48DTO regCMgejec = dataXDia.Find(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCMEjec && x.Barrcodi == grupo.Barrcodi && x.Emprcodi == grupo.Emprcodi);
                                decimal? cmg = null;
                                if (regCMgejec != null && potEjec.GetValueOrDefault(0) != 0)
                                {
                                    cmg = (decimal?)regCMgejec.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regCMgejec, null);
                                    if (cmg != null)
                                        cmg = cmg * 1000;
                                }
                                strHtml.Append(string.Format("<td>{0}</td>", cmg.GetValueOrDefault(0) != 0 ? (cmg.Value).ToString("N", nfi) : string.Empty));

                                //Costos marg Programados
                                MeMedicion48DTO regCMgprog = dataXDia.Find(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCMProg && x.Cnfbarcodi == grupo.Cnfbarcodi && x.Emprcodi == grupo.Emprcodi);
                                decimal? cmgProg = null;
                                if (regCMgprog != null && potProg.GetValueOrDefault(0) != 0)
                                {
                                    cmgProg = (decimal?)regCMgprog.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regCMgprog, null);
                                }
                                strHtml.Append(string.Format("<td>{0}</td>", cmgProg.GetValueOrDefault(0) != 0 ? (cmgProg.Value).ToString("N", nfi) : string.Empty));

                                //IRT
                                strHtml.Append(string.Format("<td>{0}</td>", irt.GetValueOrDefault(0) != 0 ? (irt.Value).ToString("N", nfi) : string.Empty));

                                //Error
                                MeMedicion48DTO regError = dataXDia.Find(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaIRTErrorBT && x.Grupocodi == grupo.Grupocodi);
                                decimal? error = null;
                                if (regError != null)
                                {
                                    error = (decimal?)regError.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regError, null);
                                }
                                strHtml.Append(string.Format("<td>{0}</td>", error != null ? (error.Value).ToString("N", nfi) : string.Empty));

                                ////Tolerancia
                                bool mostrarJustificacion = false;
                                if (bt != null)
                                {
                                    strHtml.Append(string.Format("<td>{0}</td>", error != null ? (bt.Mmmtolvalortolerancia).ToString() : string.Empty));
                                    mostrarJustificacion = error != null && bt.Mmmtolvalortolerancia < error;
                                }
                                else
                                {
                                    strHtml.Append(string.Format("<td>{0}</td>", string.Empty));
                                }

                                //Justificación
                                strHtml.AppendFormat("<td class='justif-{0}'>", ConstantesMonitoreo.CodigoIRT);
                                if (mostrarJustificacion)
                                {
                                    MmmJustificacionDTO regJustif = dataJustif.Find(x => x.Mjustfecha == horas && x.Grupocodi == grupo.Grupocodi);

                                    if (regJustif != null && regJustif.Mjustdescripcion != null)
                                    {
                                        strHtml.AppendFormat("<img class='edit' src='" + url + "Content/Images/Pen.png' alt='Editar justificación' onclick='javascript:editJustif(this)' width='15'/>");
                                        strHtml.AppendFormat("<img class='add' style='display:none' src='" + url + "Content/Images/Plus.png' alt='Agregar justificación' onclick='javascript:addJustif(this)'  width='15'/>");
                                    }
                                    else
                                    {
                                        strHtml.AppendFormat("<img class='add' src='" + url + "Content/Images/Plus.png' alt='Agregar justificación' onclick='javascript:addJustif(this)' width='15'/>");
                                        strHtml.AppendFormat("<img class='edit' style='display:none' src='" + url + "Content/Images/Pen.png' alt='Editar justificación' onclick='javascript:editJustif(this)'  width='15'/>");
                                    }
                                }
                                strHtml.AppendFormat("<input type='hidden' name='hfFechaJustif' value='{0:dd/MM/yyyy HH:mm}'>", horas);
                                strHtml.AppendFormat("<input type='hidden' name='hfIndicador' value='{0}'>", ConstantesMonitoreo.CodigoIRT);
                                strHtml.AppendFormat("<input type='hidden' name='hfEmprcodi' value='{0}'>", grupo.Emprcodi);
                                strHtml.AppendFormat("<input type='hidden' name='hfGrupocodi' value='{0}'>", grupo.Grupocodi);
                                strHtml.AppendFormat("</td>");
                            }
                            else
                            {
                                strHtml.Append("<td></td>");
                                strHtml.Append("<td></td>");
                                strHtml.Append("<td></td>");
                                strHtml.Append("<td></td>");
                                strHtml.Append("<td></td>");
                                strHtml.Append("<td></td>");
                                strHtml.Append("<td></td>");
                                strHtml.Append("<td></td>");
                            }
                        }
                        else
                        {
                            strHtml.Append("<td></td>");
                            strHtml.Append("<td></td>");
                            strHtml.Append("<td></td>");
                            strHtml.Append("<td></td>");
                            strHtml.Append("<td></td>");
                            strHtml.Append("<td></td>");
                            strHtml.Append("<td></td>");
                            strHtml.Append("<td></td>");
                        }
                    }

                    strHtml.Append("</tr>");
                    horas = horas.AddMinutes(30);
                }
            }
            #endregion
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            strHtml.Append("</div>");
            #endregion

            return strHtml.ToString();
        }

        /// <summary>
        /// Registrar/Actualizar justificaciones por Fecha e Indicador
        /// </summary>
        /// <param name="indicador"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="listaData"></param>
        /// <param name="usuario"></param>
        public void GuardarJustificacionByFechaIndicador(int indicador, DateTime fechaInicio, DateTime fechaFin, List<MmmJustificacionDTO> listaData, string usuario)
        {
            DateTime fechaActual = DateTime.Now;

            listaData = listaData.Where(x => x.Immecodi == indicador).ToList();
            List<MmmJustificacionDTO> dataBD = this.ListMmmJustificacionByFechaAndIndicador(indicador, fechaInicio, fechaFin);

            List<MmmJustificacionDTO> dataCreate = new List<MmmJustificacionDTO>();
            List<MmmJustificacionDTO> dataUpdate = new List<MmmJustificacionDTO>();

            foreach (var reg in listaData)
            {
                reg.Mjustfecha = DateTime.ParseExact(reg.MjustfechaDesc, ConstantesAppServicio.FormatoFechaFull, CultureInfo.InvariantCulture);
                var regExist = dataBD.Find(x => x.Immecodi == indicador && x.Mjustfecha == reg.Mjustfecha && x.Emprcodi == reg.Emprcodi);
                if (regExist != null)
                {
                    if (regExist.Mjustdescripcion != reg.Mjustdescripcion)
                    {
                        regExist.Mjustdescripcion = reg.Mjustdescripcion;
                        regExist.Mjustusumodificacion = usuario;
                        regExist.Mjustfecmodificacion = fechaActual;
                        dataUpdate.Add(regExist);
                    }
                }
                else
                {
                    reg.Mjustusucreacion = usuario;
                    reg.Mjustfeccreacion = fechaActual;
                    dataCreate.Add(reg);
                    dataBD.Add(reg);
                }
            }

            foreach (var reg in dataCreate)
                this.SaveMmmJustificacion(reg);

            foreach (var reg in dataUpdate)
                this.UpdateMmmJustificacion(reg);
        }

        /// <summary>
        /// Generación de Reporte 1
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="ws"></param>
        /// <param name="nombreSheet"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="vermmcodi"></param>
        public void GenerarExcelReportePorcentajeErrorBT1(string empresas, ExcelWorksheet ws, DateTime fechaIni, DateTime fechaFin, int rowIni, int colIni, int colFinFreeze, int vermmcodi, List<MmmDatoDTO> listFacTable)
        {
            List<SiEmpresaDTO> listaEmpresa;
            List<MeMedicion48DTO> dataTotal = this.ListarIndicador(vermmcodi, fechaIni, fechaFin, ConstantesMonitoreo.CodigoS, listFacTable, ConstantesMonitoreo.ReportePorcentajeErrorBandaTolerancia, out listaEmpresa);
            listaEmpresa = this.ListarEmpresasMonitoreoByData(listaEmpresa, empresas);

            List<MmmJustificacionDTO> dataJustif = this.ListMmmJustificacionByFechaAndIndicador(ConstantesMonitoreo.CodigoS, fechaIni, fechaFin);
            MmmBandtolDTO bt = this.GetBandaToleranciaByIndicadorYPeriodo(ConstantesMonitoreo.CodigoS, fechaIni);

            int row = rowIni + 5;
            int col = colIni;
            //
            if (listaEmpresa.Count > 0)
            {
                #region cabecera

                int rowIniNombreReporte = row;
                /// Fila Hora - Empresa - Total

                int colIniFecha = col;
                int rowIniFecha = row;
                int rowFinFecha = rowIniFecha + 2 - 1;
                ws.Cells[rowIniFecha, colIniFecha].Value = "MM/DD/hh:mm";
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Merge = true;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.WrapText = true;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Font.Bold = true;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Font.Size = 28;

                int colIniSein = colIniFecha + 1;
                int rowIniSein = rowIniFecha;
                int rowFinSein = rowFinFecha;
                ws.Cells[rowIniSein, colIniSein].Value = "PES";
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Merge = true;
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.WrapText = true;
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.Font.Bold = true;
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.Font.Size = 28;

                int rowIniEmp = row;
                int colIniEmp = colIniSein + 1;
                int colFinEmp = colIniEmp;

                int colIniNombreReporte = colIniEmp;

                var colorBorder = Color.White;
                var colorBorderHora = Color.Black;
                var classTipoEmpresa = "#4472C4";
                var classTipoPes = "#9BC2E6";
                var classTiporcen = "#9BC2E6";

                for (int i = 0; i < listaEmpresa.Count; i++)
                {
                    //Empresa
                    var thEmp = listaEmpresa[i];
                    for (int j = 1; j <= 4; j++)
                    {
                        colFinEmp = colIniEmp;
                        ws.Cells[rowIniEmp, colIniEmp].Value = thEmp.Emprnomb.Trim();

                        ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.Font.Size = 24;
                        ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Merge = true;
                        ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.WrapText = true;

                        ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.TextRotation = 90;

                        var range = ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp];
                        if (j == 1)
                        {
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Font.Bold = true;
                            range.Style.Border.BorderAround(ExcelBorderStyle.Thin, colorBorder);
                            range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Top.Color.SetColor(colorBorder);
                            range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Bottom.Color.SetColor(colorBorder);
                            range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Left.Color.SetColor(colorBorder);
                            range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Right.Color.SetColor(colorBorder);
                            range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(classTipoEmpresa));
                            range.Style.Font.Color.SetColor(Color.White);
                        }
                        else
                        {
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Font.Bold = true;
                            range.Style.Border.BorderAround(ExcelBorderStyle.Thin, colorBorder);
                            range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Top.Color.SetColor(colorBorder);
                            range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Bottom.Color.SetColor(colorBorder);
                            range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Left.Color.SetColor(colorBorder);
                            range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Right.Color.SetColor(colorBorder);
                            range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(classTiporcen));
                            range.Style.Font.Color.SetColor(Color.Black);
                        }
                        colIniEmp = colFinEmp + 1;
                    }
                }

                int rowIniMw = row + 1;
                int colIniMw = colIniSein + 1;
                int colFinMw = colIniMw;

                for (int i = 0; i < listaEmpresa.Count; i++)
                {
                    //Empresa
                    var thEmp = listaEmpresa[i];
                    for (int j = 1; j <= 4; j++)
                    {
                        ws.Cells[rowIniMw, colIniMw].Value = j == 1 ? "S (%)" : j == 2 ? "Error (%)" : j == 3 ? "Tolerancia" : "Justificación";

                        colFinMw = colIniMw;
                        ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.Font.Size = 28;
                        ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Merge = true;

                        ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.WrapText = true;
                        ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        var range = ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw];
                        if (j == 1)
                        {
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Font.Bold = true;
                            range.Style.Border.BorderAround(ExcelBorderStyle.Thin, colorBorder);
                            range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Top.Color.SetColor(colorBorder);
                            range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Bottom.Color.SetColor(colorBorder);
                            range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Left.Color.SetColor(colorBorder);
                            range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Right.Color.SetColor(colorBorder);
                            range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(classTipoEmpresa));
                            range.Style.Font.Color.SetColor(Color.White);
                        }
                        else
                        {
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Font.Bold = true;
                            range.Style.Border.BorderAround(ExcelBorderStyle.Thin, colorBorder);
                            range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Top.Color.SetColor(colorBorder);
                            range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Bottom.Color.SetColor(colorBorder);
                            range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Left.Color.SetColor(colorBorder);
                            range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Right.Color.SetColor(colorBorder);
                            range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(classTiporcen));
                            range.Style.Font.Color.SetColor(Color.Black);
                        }

                        colIniMw = colFinMw + 1;
                    }
                }

                int colFinNombreReporte = colFinEmp;

                using (var range = ws.Cells[rowIniFecha, colIniSein, rowFinFecha, colIniSein])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Font.Bold = true;
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin, colorBorderHora);
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Color.SetColor(colorBorderHora);
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(colorBorderHora);
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Color.SetColor(colorBorderHora);
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Color.SetColor(colorBorderHora);
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(classTipoPes));
                    range.Style.Font.Color.SetColor(Color.Black);
                }

                #endregion

                int rowIniData = rowIniMw + 1;
                row = rowIniData;

                #region cuerpo

                int numDia = 0;

                int colData = colIniFecha;
                for (var day = fechaIni.Date; day.Date <= fechaFin; day = day.AddDays(1))
                {
                    numDia++;
                    DateTime horas = day.AddMinutes(30);

                    List<MeMedicion48DTO> data = dataTotal.Where(x => x.Medifecha == day).ToList();
                    MeMedicion48DTO total = data.Find(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaTotalPotencia);

                    for (int h = 1; h <= 48; h++)
                    {
                        //Fecha
                        ws.Cells[row, colIniFecha].Value = horas.ToString(ConstantesAppServicio.FormatoFechaHora);
                        ws.Cells[row, colIniFecha].Style.Font.Size = 28;
                        ws.Cells[row, colIniFecha].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        //PES
                        colData = colIniFecha;
                        colData++;

                        decimal? potenciaTotal = null;
                        if (total != null)
                        {
                            potenciaTotal = (decimal?)total.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(total, null);
                        }

                        ws.Cells[row, colData].Value = potenciaTotal.GetValueOrDefault(0);

                        //PE / S
                        colData++;
                        foreach (var empresa in listaEmpresa)
                        {
                            if (empresa.Emprcodi == 11412)
                            {
                            }

                            ws.Cells[row, colData].Style.WrapText = true;
                            ws.Cells[row, colData].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            //Cuota
                            MeMedicion48DTO reg = data.Find(x => x.Emprcodi == empresa.Emprcodi && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCuota);
                            decimal? pot = null;
                            if (reg != null)
                            {
                                pot = (decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg, null);
                            }

                            if (pot.GetValueOrDefault(0) == 0)
                            {
                                ws.Cells[row, colData].Value = string.Empty;
                            }
                            else
                            {
                                ws.Cells[row, colData].Value = pot;
                            }
                            colData++;

                            //Error
                            MeMedicion48DTO regError = data.Find(x => x.Emprcodi == empresa.Emprcodi && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCuotaErrorBT);
                            decimal? error = null;
                            if (regError != null)
                            {
                                error = (decimal?)regError.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regError, null);
                            }
                            ws.Cells[row, colData].Value = error;
                            colData++;

                            //Tolerancia
                            bool mostrarJustificacion = false;
                            if (bt != null && error != null)
                            {
                                ws.Cells[row, colData].Value = bt.Mmmtolvalortolerancia;
                                mostrarJustificacion = error != null && bt.Mmmtolvalortolerancia < error;
                            }
                            colData++;

                            //Justificación
                            if (mostrarJustificacion)
                            {
                                MmmJustificacionDTO regJustif = dataJustif.Find(x => x.Mjustfecha == horas && x.Emprcodi == empresa.Emprcodi);
                                if (regJustif != null && regJustif.Mjustdescripcion != null)
                                {
                                    ws.Cells[row, colData].Value = regJustif.Mjustdescripcion;
                                }
                            }
                            colData++;
                        }

                        horas = horas.AddMinutes(30);
                        row++;
                    }
                }

                colData--;

                using (var range = ws.Cells[rowIniData, colIniFecha, (rowIniData + numDia * 48) - 1, colData])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.White);
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Style = ExcelBorderStyle.Dotted;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                }

                //Formato Data
                using (var range = ws.Cells[rowIniData, colIniFecha + 1, (rowIniData + numDia * 48) - 1, colData])
                {
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.Style.Font.Size = 28;
                    range.Style.Numberformat.Format = "#,##0.000";
                }

                for (int f = rowIniData + 48; f <= rowIniData + numDia * 48; f += 48)
                {
                    ws.Cells[f, colIniFecha, f, colData].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                }

                //Formato de Filas y columnas
                for (int columna = colIniSein; columna <= colData; columna++)
                    ws.Column(columna).Width = 40;

                ws.Column(colIniFecha).Width = 50;
                ws.Column(colIniSein).Width = 28;
                ws.Row(rowIniNombreReporte).Height = 60;
                ws.Row(rowIniEmp).Height = 410;
                ws.Row(rowIniMw).Height = 200;

                #endregion

                ws.View.FreezePanes(rowFinFecha + 1, colFinFreeze + 1);
            }
            ws.View.ZoomScale = 30;
        }

        /// <summary>
        /// Generación de Reporte Excel 2
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="ws"></param>
        /// <param name="nombreSheet"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="vermmcodi"></param>
        public void GenerarExcelReportePorcentajeErrorBT2(string empresas, ExcelWorksheet ws, DateTime fechaIni, DateTime fechaFin, int rowIni, int colIni, int colFinFreeze, int vermmcodi, List<MmmDatoDTO> listFacTable)
        {
            SiParametroValorDTO param = this.GetParametroTendenciaHHI(fechaIni);
            decimal tendenciaUno = param.HHITendenciaUno.GetValueOrDefault(0);
            decimal tendenciaCero = param.HHITendenciaCero.GetValueOrDefault(0);
            string colorUno = param.HHITendenciaUnoColor;
            string colorCero = param.HHITendenciaCeroColor;

            List<SiEmpresaDTO> listaEmpresa;
            List<MeMedicion48DTO> dataTotal = this.ListarIndicador(vermmcodi, fechaIni, fechaFin, ConstantesMonitoreo.CodigoHHI, listFacTable, ConstantesMonitoreo.ReportesIndicadores, out listaEmpresa);
            listaEmpresa = this.ListarEmpresasMonitoreoByData(listaEmpresa, empresas);

            int row = rowIni + 5;
            int col = colIni;
            //
            if (listaEmpresa.Count > 0)
            {
                #region cabecera
                int rowIniNombreReporte = row;
                /// Fila Hora - Empresa - Total

                int colIniFecha = col;
                int rowIniFecha = row;
                int rowFinFecha = rowIniFecha + 2 - 1;
                ws.Cells[rowIniFecha, colIniFecha].Value = "MM/DD/hh:mm";
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Merge = true;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.WrapText = true;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Font.Bold = true;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Font.Size = 28;

                int rowIniEmp = row;
                int colIniEmp = colIniFecha + 1;
                int colFinEmp = colIniEmp;

                int colIniNombreReporte = colIniEmp;

                for (int i = 0; i < listaEmpresa.Count; i++)
                {
                    //Empresa
                    var thEmp = listaEmpresa[i];
                    colFinEmp = colIniEmp;
                    ws.Cells[rowIniEmp, colIniEmp].Value = thEmp.Emprnomb.Trim();
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.Font.Size = 24;
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Merge = true;
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.WrapText = true;
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.TextRotation = 90;
                    colIniEmp = colFinEmp + 1;
                }

                colFinEmp = colIniEmp;
                ws.Cells[rowIniEmp, colIniEmp, rowIniEmp + 1, colFinEmp].Value = "HHI";
                ws.Cells[rowIniEmp, colIniEmp, rowIniEmp + 1, colFinEmp].Merge = true;
                ws.Cells[rowIniEmp, colIniEmp, rowIniEmp + 1, colFinEmp].Style.WrapText = true;
                ws.Cells[rowIniEmp, colIniEmp, rowIniEmp + 1, colFinEmp].Style.Font.Size = 28;
                ws.Cells[rowIniEmp, colIniEmp, rowIniEmp + 1, colFinEmp].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniEmp, colIniEmp, rowIniEmp + 1, colFinEmp].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniEmp, colIniEmp, rowIniEmp + 1, colFinEmp].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                colIniEmp = colFinEmp + 1;

                int rowIniPorc = row + 1;
                int colIniPorc = colIniFecha + 1;
                int colFinPorc = colIniPorc;

                for (int i = 0; i < listaEmpresa.Count; i++)
                {
                    colFinPorc = colIniPorc;

                    ws.Cells[rowIniPorc, colIniPorc].IsRichText = true;
                    ExcelRichTextCollection rtfCollection = ws.Cells[rowIniPorc, colIniPorc].RichText;
                    ExcelRichText ert = null;
                    ert = rtfCollection.Add("S");
                    ert.FontName = "Arial";
                    ert.Color = Color.White;
                    ert.Size = 28;
                    ert.Bold = true;

                    ert = rtfCollection.Add("2");
                    ert.VerticalAlign = ExcelVerticalAlignmentFont.Superscript;
                    ert.FontName = "Arial";
                    ert.Color = Color.White;
                    ert.Size = 28;
                    ert.Bold = true;

                    ws.Cells[rowIniPorc, colIniPorc].Style.Font.Size = 28;
                    ws.Cells[rowIniPorc, colIniPorc].Style.WrapText = true;
                    ws.Cells[rowIniPorc, colIniPorc].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniPorc, colIniPorc].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[rowIniPorc, colIniPorc].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    colIniPorc = colFinPorc + 1;
                }

                colFinPorc = colIniPorc;

                colIniPorc = colFinPorc + 1;

                //Nombre Reporte
                int colFinNombreReporte = colFinEmp;

                var colorBorder = Color.White;
                var classTipoEmpresa = "#4472C4";

                var colorBorderHora = Color.Black;

                using (var range = ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Font.Bold = true;
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin, colorBorderHora);
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Color.SetColor(colorBorderHora);
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(colorBorderHora);
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Color.SetColor(colorBorderHora);
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Color.SetColor(colorBorderHora);
                    range.Style.Fill.BackgroundColor.SetColor(Color.White);
                    range.Style.Font.Color.SetColor(Color.Black);
                }

                using (var range = ws.Cells[rowIniNombreReporte, colIniNombreReporte, rowIniPorc, colFinNombreReporte])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Font.Bold = true;
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin, colorBorder);
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Color.SetColor(colorBorder);
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(colorBorder);
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Color.SetColor(colorBorder);
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Color.SetColor(colorBorder);
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(classTipoEmpresa));
                    range.Style.Font.Color.SetColor(Color.White);
                }

                #endregion

                int rowIniData = rowIniPorc + 1;
                row = rowIniData;

                #region cuerpo

                int numDia = 0;

                int colData = colIniFecha;
                for (var day = fechaIni.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
                {
                    numDia++;
                    DateTime horas = day.AddMinutes(30);

                    List<MeMedicion48DTO> data = dataTotal.Where(x => x.Medifecha == day).ToList();
                    MeMedicion48DTO total = data.Find(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaHHI);

                    for (int h = 1; h <= 48; h++)
                    {
                        //Fecha
                        ws.Cells[row, colIniFecha].Value = horas.ToString(ConstantesAppServicio.FormatoFechaHora);
                        ws.Cells[row, colIniFecha].Style.Font.Size = 28;
                        ws.Cells[row, colIniFecha].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        colData = colIniFecha;

                        colData++;

                        foreach (var empresa in listaEmpresa)
                        {
                            // Cuota para HHI
                            MeMedicion48DTO reg = data.Find(x => x.Emprcodi == empresa.Emprcodi && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaHHICuotaMercado);
                            decimal? cuotaHHI = null;
                            if (reg != null)
                            {
                                cuotaHHI = (decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg, null);
                            }
                            //elemento al  cuadrado
                            ws.Cells[row, colData].Value = cuotaHHI;
                            colData++;
                        }

                        decimal totalHhi = 0;
                        if (total != null)
                        {
                            totalHhi = ((decimal?)total.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(total, null)).GetValueOrDefault(0);
                        }

                        if (totalHhi >= tendenciaUno)
                        {
                            ws.Cells[row, colData].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[row, colData].Style.Font.Color.SetColor(Color.White);
                            ws.Cells[row, colData].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(param.HHITendenciaUnoColor));
                            ws.Cells[row, colData].Value = totalHhi;
                        }
                        else if (totalHhi <= tendenciaCero)
                        {
                            ws.Cells[row, colData].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[row, colData].Style.Font.Color.SetColor(Color.White);
                            ws.Cells[row, colData].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(param.HHITendenciaCeroColor));
                            ws.Cells[row, colData].Value = totalHhi;
                        }
                        else if (totalHhi > 0)
                        {
                            ws.Cells[row, colData].Value = (totalHhi);
                        }
                        colData++;

                        horas = horas.AddMinutes(30);
                        row++;
                    }
                }

                colData--;

                using (var range = ws.Cells[rowIniData, colIniFecha, (rowIniData + numDia * 48) - 1, colData])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.White);
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Style = ExcelBorderStyle.Dotted;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                }

                using (var range = ws.Cells[rowIniData, colIniFecha + 1, (rowIniData + numDia * 48) - 1, colData])
                {
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.Style.Font.Size = 28;
                    range.Style.Numberformat.Format = "#,##0.000";
                }

                for (int f = rowIniData + 48; f <= rowIniData + numDia * 48; f += 48)
                {
                    ws.Cells[f, colIniFecha, f, colData].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                }

                //Formato de Filas y columnas
                for (int columna = colIniFecha + 1; columna <= colData; columna++)
                    ws.Column(columna).Width = 40;

                ws.Column(colIniFecha).Width = 50;
                ws.Row(rowIniNombreReporte).Height = 60;
                ws.Row(rowIniEmp).Height = 400;
                ws.Row(rowIniPorc).Height = 200;

                #endregion

                ws.View.FreezePanes(rowFinFecha + 1, colFinFreeze + 1);
            }
            ws.View.ZoomScale = 30;
        }

        /// <summary>
        /// Generación de Reporte Excel 3
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="ws"></param>
        /// <param name="nombreSheet"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="vermmcodi"></param>
        public void GenerarExcelReportePorcentajeErrorBT3(string empresas, ExcelWorksheet ws, DateTime fechaIni, DateTime fechaFin, int rowIni, int colIni, int colFinFreeze, int vermmcodi, List<MmmDatoDTO> listFacTable)
        {
            SiParametroValorDTO param = this.GetParametroOfertaPivotal(fechaIni);
            string colorEsPivotal = param.IOPEsPivotalColor;
            string colorNoPivotal = param.IOPNoPivotalColor;

            List<SiEmpresaDTO> listaEmpresa;
            List<MeMedicion48DTO> dataTotal = this.ListarIndicador(vermmcodi, fechaIni, fechaFin, ConstantesMonitoreo.CodigoIOP, listFacTable, ConstantesMonitoreo.ReportesIndicadores, out listaEmpresa);
            listaEmpresa = this.ListarEmpresasMonitoreoByData(listaEmpresa, empresas);

            int row = rowIni + 5;
            int col = colIni;
            //
            if (listaEmpresa.Count > 0)
            {
                #region cabecera

                int rowIniNombreReporte = row;
                /// Fila Hora - Empresa - Total

                int colIniFecha = col;
                int rowIniFecha = row;
                int rowFinFecha = rowIniFecha + 2 - 1;
                ws.Cells[rowIniFecha, colIniFecha].Value = "MM/DD/hh:mm";
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Merge = true;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.WrapText = true;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Font.Size = 28;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Fill.BackgroundColor.SetColor(Color.Black);
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Border.Top.Color.SetColor(Color.Black);

                int colIniMd = colIniFecha + 1;
                int rowIniMd = rowIniFecha;
                int rowFinMd = rowFinFecha;
                ws.Cells[rowIniMd, colIniMd].Value = "MD (MW)";
                ws.Cells[rowIniMd, colIniMd, rowFinMd, colIniMd].Merge = true;
                ws.Cells[rowIniMd, colIniMd, rowFinMd, colIniMd].Style.WrapText = true;
                ws.Cells[rowIniMd, colIniMd, rowFinMd, colIniMd].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniMd, colIniMd, rowFinMd, colIniMd].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniMd, colIniMd, rowFinMd, colIniMd].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[rowIniMd, colIniMd, rowFinMd, colIniMd].Style.Font.Size = 28;
                ws.Cells[rowIniMd, colIniMd, rowFinMd, colIniMd].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[rowIniMd, colIniMd, rowFinMd, colIniMd].Style.Fill.BackgroundColor.SetColor(Color.Black);
                ws.Cells[rowIniMd, colIniMd, rowFinMd, colIniMd].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[rowIniMd, colIniMd, rowFinMd, colIniMd].Style.Border.Top.Color.SetColor(Color.Black);

                int colIniSein = colIniMd + 1;
                int rowIniSein = rowIniFecha;
                int rowFinSein = rowFinFecha;
                ws.Cells[rowIniSein, colIniSein].Value = "PES (MW)";
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Merge = true;
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.WrapText = true;
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.Font.Size = 28;
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.Fill.BackgroundColor.SetColor(Color.Black);
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.Border.Top.Color.SetColor(Color.Black);

                int rowIniEmp = row;
                int colIniEmp = colIniSein + 1;
                int colFinEmp = colIniEmp;

                int colIniNombreReporte = colIniEmp;

                for (int j = 1; j <= 2; j++)
                {
                    for (int i = 0; i < listaEmpresa.Count; i++)
                    {
                        //Empresa
                        var thEmp = listaEmpresa[i];
                        colFinEmp = colIniEmp;
                        ws.Cells[rowIniEmp, colIniEmp].Value = thEmp.Emprnomb.Trim();
                        ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.Font.Size = 24;
                        ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Merge = true;
                        ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.WrapText = true;
                        ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.TextRotation = 90;
                        colIniEmp = colFinEmp + 1;
                    }
                }

                int rowIniMw = row + 1;
                int colIniMw = colIniFecha + 1;
                int colFinMw = colIniMw;

                ws.Cells[rowIniMw, colIniMw].Value = "MD (MW)";
                colIniMw++;

                ws.Cells[rowIniMw, colIniMw].Value = "PES (MW)";

                colIniMw++;
                for (int j = 1; j <= 2; j++)
                {
                    for (int i = 0; i < listaEmpresa.Count; i++)
                    {
                        ws.Cells[rowIniMw, colIniMw].Value = j == 1 ? "PE (MW)" : "IOP";

                        colFinMw = colIniMw;
                        ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.Font.Size = 28;
                        ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Merge = true;
                        ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.WrapText = true;
                        ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        colIniMw = colFinMw + 1;
                    }
                }

                //Nombre Reporte
                colIniNombreReporte = colIniNombreReporte - 3;
                int colFinNombreReporte = colFinEmp;

                var colorBorder = Color.White;
                var colorBorderHora = Color.Black;
                var classTipoEmpresa = "#4472C4";
                var classTipoSein = "#D9E1F2";
                var classHora = "#F9FBFB";
                var classTipoIop = "#D9E1F2";

                //Fecha
                using (var range = ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Font.Bold = true;
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin, colorBorderHora);
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Color.SetColor(colorBorderHora);
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(colorBorderHora);
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Color.SetColor(colorBorderHora);
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Color.SetColor(colorBorderHora);
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(classHora));
                    range.Style.Font.Color.SetColor(Color.Black);
                }

                //MD
                using (var range = ws.Cells[rowIniMd, colIniMd, rowFinMd, colIniSein])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Font.Bold = true;
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin, colorBorderHora);
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Color.SetColor(colorBorderHora);
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(colorBorderHora);
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Color.SetColor(colorBorderHora);
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Color.SetColor(colorBorderHora);
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(classTipoSein));
                    range.Style.Font.Color.SetColor(Color.Black);
                }

                //PE
                using (var range = ws.Cells[rowIniNombreReporte, colIniNombreReporte + 3, rowIniMw, (colFinNombreReporte / 2) + 2])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Font.Bold = true;
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin, colorBorder);
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Color.SetColor(colorBorder);
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(colorBorder);
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Color.SetColor(colorBorder);
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Color.SetColor(colorBorder);
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(classTipoEmpresa));
                    range.Style.Font.Color.SetColor(Color.White);
                }

                //IOP
                using (var range = ws.Cells[rowIniNombreReporte, (colFinNombreReporte / 2) + 3, rowIniMw, colFinNombreReporte])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Font.Bold = true;
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin, colorBorder);
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Color.SetColor(colorBorder);
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(colorBorder);
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Color.SetColor(colorBorder);
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Color.SetColor(colorBorder);
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(classTipoIop));
                    range.Style.Font.Color.SetColor(Color.Black);
                }

                #endregion

                int rowIniData = rowIniMw + 1;
                row = rowIniData;

                #region cuerpo

                int numDia = 0;

                int colData = colIniFecha;
                for (var day = fechaIni.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
                {
                    numDia++;

                    List<MeMedicion48DTO> data = dataTotal.Where(x => x.Medifecha == day).ToList();
                    MeMedicion48DTO total = data.Find(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaTotalPotencia);
                    MeMedicion48DTO md = data.Find(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaMaximaDemanda);

                    DateTime horas = day.AddMinutes(30);

                    for (int h = 1; h <= 48; h++)
                    {
                        //Fecha
                        ws.Cells[row, colIniFecha].Value = horas.ToString(ConstantesAppServicio.FormatoFechaHora);
                        ws.Cells[row, colIniFecha].Style.Font.Size = 28;
                        ws.Cells[row, colIniFecha].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[row, colIniFecha].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[row, colIniFecha].Style.Fill.BackgroundColor.SetColor(Color.White);
                        colData = colIniFecha + 1;

                        //Maxima Demanda Programada
                        decimal? maximademanda = null;
                        if (md != null)
                        {
                            maximademanda = (decimal?)md.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(md, null);
                        }
                        ws.Cells[row, colData].Value = maximademanda;
                        ws.Cells[row, colData].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[row, colData].Style.Fill.BackgroundColor.SetColor(Color.White);
                        colData++;

                        //Potencia Total
                        decimal? potenciaTotal = null;
                        if (total != null)
                        {
                            potenciaTotal = (decimal?)total.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(total, null);
                        }
                        ws.Cells[row, colData].Value = potenciaTotal;
                        ws.Cells[row, colData].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[row, colData].Style.Fill.BackgroundColor.SetColor(Color.White);
                        colData++;

                        //Potencia x Empresa
                        foreach (var empresa in listaEmpresa)
                        {
                            MeMedicion48DTO reg = data.Find(x => x.Medifecha == day && x.Emprcodi == empresa.Emprcodi && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaMWEjec);
                            decimal? pot = null;
                            if (reg != null)
                            {
                                pot = (decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg, null);
                            }

                            ws.Cells[row, colData].Value = pot;
                            ws.Cells[row, colData].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[row, colData].Style.Fill.BackgroundColor.SetColor(Color.White);
                            colData++;
                        }

                        //IOP x Empresa
                        foreach (var empresa in listaEmpresa)
                        {
                            MeMedicion48DTO reg = data.Find(x => x.Medifecha == day && x.Emprcodi == empresa.Emprcodi && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaIOP);
                            int iop = ConstantesMonitoreo.ValorIOPEsPivotal;

                            if (reg != null)
                            {
                                iop = Convert.ToInt32(((decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg, null)).GetValueOrDefault(0));
                            }

                            string color = ConstantesMonitoreo.ValorIOPEsPivotal == iop ? colorEsPivotal : colorNoPivotal;

                            ws.Cells[row, colData].Value = iop;
                            ws.Cells[row, colData].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[row, colData].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(color));
                            ws.Cells[row, colData].Style.Font.Color.SetColor(Color.White);

                            colData++;
                        }

                        horas = horas.AddMinutes(30);
                        row++;
                    }
                }

                colData--;

                using (var range = ws.Cells[rowIniData, colIniFecha, (rowIniData + numDia * 48) - 1, colData])
                {
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Style = ExcelBorderStyle.Dotted;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                }

                using (var range = ws.Cells[rowIniData, colIniFecha + 1, rowIniData + numDia * 48, colData])
                {
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.Style.Font.Size = 28;
                    range.Style.Numberformat.Format = "#,##0.000";
                }

                for (int f = rowIniData + 48; f <= rowIniData + numDia * 48; f += 48)
                {
                    ws.Cells[f, colIniFecha, f, colData].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                }

                //Formato de Filas y columnas
                for (int columna = colIniFecha + 1; columna <= colData; columna++)
                    ws.Column(columna).Width = 40;

                ws.Column(colIniFecha).Width = 50;

                ws.Row(rowIniEmp).Height = 400;
                ws.Row(rowIniMw).Height = 200;

                #endregion

                #region Leyenda
                int rowLeyenda = row + 3;
                int rowPivotal = row + 4;
                int rowNoPivotal = row + 5;

                ws.Cells[rowLeyenda, colIniFecha].Value = "Leyenda:";
                ws.Cells[rowLeyenda, colIniFecha].Style.Font.Size = 30;
                ws.Cells[rowLeyenda, colIniFecha].Style.Font.Bold = true;

                ws.Cells[rowPivotal, colIniMd].Value = " '" + ConstantesMonitoreo.ValorIOPEsPivotal + "' (Pivotal) ";
                ws.Cells[rowPivotal, colIniMd].Style.Font.Size = 30;
                ws.Cells[rowPivotal, colIniMd].Style.Font.Bold = true;
                ws.Cells[rowPivotal, colIniFecha].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[rowPivotal, colIniFecha].Style.Border.BorderAround(ExcelBorderStyle.Thin, colorBorder);
                ws.Cells[rowPivotal, colIniFecha].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[rowPivotal, colIniFecha].Style.Border.Top.Color.SetColor(colorBorder);
                ws.Cells[rowPivotal, colIniFecha].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                ws.Cells[rowPivotal, colIniFecha].Style.Border.Bottom.Color.SetColor(colorBorder);
                ws.Cells[rowPivotal, colIniFecha].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[rowPivotal, colIniFecha].Style.Border.Left.Color.SetColor(colorBorder);
                ws.Cells[rowPivotal, colIniFecha].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[rowPivotal, colIniFecha].Style.Border.Right.Color.SetColor(colorBorder);
                ws.Cells[rowPivotal, colIniFecha].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(colorEsPivotal));
                ws.Cells[rowPivotal, colIniFecha].Style.Font.Color.SetColor(Color.Black);

                ws.Cells[rowNoPivotal, colIniMd].Value = " '" + ConstantesMonitoreo.ValorIOPNoPivotal + "' (No Pivotal) ";
                ws.Cells[rowNoPivotal, colIniMd].Style.Font.Size = 30;
                ws.Cells[rowNoPivotal, colIniMd].Style.Font.Bold = true;
                ws.Cells[rowNoPivotal, colIniFecha].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[rowNoPivotal, colIniFecha].Style.Border.BorderAround(ExcelBorderStyle.Thin, colorBorder);
                ws.Cells[rowNoPivotal, colIniFecha].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[rowNoPivotal, colIniFecha].Style.Border.Top.Color.SetColor(colorBorder);
                ws.Cells[rowNoPivotal, colIniFecha].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                ws.Cells[rowNoPivotal, colIniFecha].Style.Border.Bottom.Color.SetColor(colorBorder);
                ws.Cells[rowNoPivotal, colIniFecha].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[rowNoPivotal, colIniFecha].Style.Border.Left.Color.SetColor(colorBorder);
                ws.Cells[rowNoPivotal, colIniFecha].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[rowNoPivotal, colIniFecha].Style.Border.Right.Color.SetColor(colorBorder);
                ws.Cells[rowNoPivotal, colIniFecha].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(colorNoPivotal));
                ws.Cells[rowNoPivotal, colIniFecha].Style.Font.Color.SetColor(Color.Black);

                #endregion

                ws.View.FreezePanes(rowFinFecha + 1, colFinFreeze + 1);
            }
            ws.View.ZoomScale = 30;
        }

        /// <summary>
        /// Generación de Reporte Excel 4
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="ws"></param>
        /// <param name="nombreSheet"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="vermmcodi"></param>
        public void GenerarExcelReportePorcentajeErrorBT4(string empresas, ExcelWorksheet ws, DateTime fechaIni, DateTime fechaFin, int rowIni, int colIni, int colFinFreeze, int vermmcodi, List<MmmDatoDTO> listFacTable)
        {
            List<SiEmpresaDTO> listaEmpresa;
            List<MeMedicion48DTO> dataTotal = this.ListarIndicador(vermmcodi, fechaIni, fechaFin, ConstantesMonitoreo.CodigoRSD, listFacTable, ConstantesMonitoreo.ReportePorcentajeErrorBandaTolerancia, out listaEmpresa);
            listaEmpresa = this.ListarEmpresasMonitoreoByData(listaEmpresa, empresas);

            List<MmmJustificacionDTO> dataJustif = this.ListMmmJustificacionByFechaAndIndicador(ConstantesMonitoreo.CodigoRSD, fechaIni, fechaFin);
            MmmBandtolDTO bt = this.GetBandaToleranciaByIndicadorYPeriodo(ConstantesMonitoreo.CodigoRSD, fechaIni);

            int row = rowIni + 5;
            int col = colIni;
            //
            if (listaEmpresa.Count > 0)
            {
                #region cabecera

                int rowIniNombreReporte = row;
                /// Fila Hora - Empresa - Total

                int colIniFecha = col;
                int rowIniFecha = row;
                int rowFinFecha = rowIniFecha + 2 - 1;
                ws.Cells[rowIniFecha, colIniFecha].Value = "MM/DD/hh:mm";
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Merge = true;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.WrapText = true;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Font.Size = 28;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Fill.BackgroundColor.SetColor(Color.Black);
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Border.Top.Color.SetColor(Color.Black);

                int colIniMd = colIniFecha + 1;
                int rowIniMd = rowIniFecha;
                int rowFinMd = rowFinFecha;
                ws.Cells[rowIniMd, colIniMd].Value = "MD (MW)";
                ws.Cells[rowIniMd, colIniMd, rowFinMd, colIniMd].Merge = true;
                ws.Cells[rowIniMd, colIniMd, rowFinMd, colIniMd].Style.WrapText = true;
                ws.Cells[rowIniMd, colIniMd, rowFinMd, colIniMd].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniMd, colIniMd, rowFinMd, colIniMd].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniMd, colIniMd, rowFinMd, colIniMd].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[rowIniMd, colIniMd, rowFinMd, colIniMd].Style.Font.Size = 28;
                ws.Cells[rowIniMd, colIniMd, rowFinMd, colIniMd].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[rowIniMd, colIniMd, rowFinMd, colIniMd].Style.Fill.BackgroundColor.SetColor(Color.Black);
                ws.Cells[rowIniMd, colIniMd, rowFinMd, colIniMd].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[rowIniMd, colIniMd, rowFinMd, colIniMd].Style.Border.Top.Color.SetColor(Color.Black);

                int colIniSein = colIniMd + 1;
                int rowIniSein = rowIniFecha;
                int rowFinSein = rowFinFecha;
                ws.Cells[rowIniSein, colIniSein].Value = "PES (MW)";
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Merge = true;
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.WrapText = true;
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.Font.Size = 28;
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.Fill.BackgroundColor.SetColor(Color.Black);
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[rowIniSein, colIniSein, rowFinSein, colIniSein].Style.Border.Top.Color.SetColor(Color.Black);

                int rowIniEmp = row;
                int colIniEmp = colIniSein + 1;
                int colFinEmp = colIniEmp;

                int colIniNombreReporte = colIniEmp;

                var colorBorder = Color.White;
                var colorBorderHora = Color.Black;
                var classTipoEmpresa = "#4472C4";
                var classTipoSein = "#D9E1F2";
                var classHora = "#F9FBFB";
                var classTipoRSD = "#D9E1F2";

                for (int i = 0; i < listaEmpresa.Count; i++)
                {
                    //Empresa
                    var thEmp = listaEmpresa[i];
                    for (int j = 1; j <= 4; j++)
                    {
                        colFinEmp = colIniEmp;
                        ws.Cells[rowIniEmp, colIniEmp].Value = thEmp.Emprnomb.Trim();

                        ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.Font.Size = 24;
                        ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Merge = true;
                        ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.WrapText = true;

                        ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.TextRotation = 90;

                        var range = ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp];
                        if (j == 1)
                        {
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Font.Bold = true;
                            range.Style.Border.BorderAround(ExcelBorderStyle.Thin, colorBorder);
                            range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Top.Color.SetColor(colorBorder);
                            range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Bottom.Color.SetColor(colorBorder);
                            range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Left.Color.SetColor(colorBorder);
                            range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Right.Color.SetColor(colorBorder);
                            range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(classTipoEmpresa));
                            range.Style.Font.Color.SetColor(Color.White);
                        }
                        else
                        {
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Font.Bold = true;
                            range.Style.Border.BorderAround(ExcelBorderStyle.Thin, colorBorder);
                            range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Top.Color.SetColor(colorBorder);
                            range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Bottom.Color.SetColor(colorBorder);
                            range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Left.Color.SetColor(colorBorder);
                            range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Right.Color.SetColor(colorBorder);
                            range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(classTipoRSD));
                            range.Style.Font.Color.SetColor(Color.Black);
                        }
                        colIniEmp = colFinEmp + 1;
                    }
                }

                int rowIniMw = row + 1;
                int colIniMw = colIniFecha + 1;
                int colFinMw = colIniMw;

                ws.Cells[rowIniMw, colIniMw].Value = "MD (MW)";
                colIniMw++;

                ws.Cells[rowIniMw, colIniMw].Value = "PES (MW)";

                colIniMw++;

                for (int i = 0; i < listaEmpresa.Count; i++)
                {
                    //Empresa
                    var thEmp = listaEmpresa[i];
                    for (int j = 1; j <= 4; j++)
                    {
                        ws.Cells[rowIniMw, colIniMw].Value = j == 1 ? "RSD" : j == 2 ? "Error (%)" : j == 3 ? "Tolerancia" : "Justificación";

                        colFinMw = colIniMw;
                        ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.Font.Size = 28;
                        ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Merge = true;

                        ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.WrapText = true;
                        ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        var range = ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw];
                        if (j == 1)
                        {
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Font.Bold = true;
                            range.Style.Border.BorderAround(ExcelBorderStyle.Thin, colorBorder);
                            range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Top.Color.SetColor(colorBorder);
                            range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Bottom.Color.SetColor(colorBorder);
                            range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Left.Color.SetColor(colorBorder);
                            range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Right.Color.SetColor(colorBorder);
                            range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(classTipoEmpresa));
                            range.Style.Font.Color.SetColor(Color.White);
                        }
                        else
                        {
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Font.Bold = true;
                            range.Style.Border.BorderAround(ExcelBorderStyle.Thin, colorBorder);
                            range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Top.Color.SetColor(colorBorder);
                            range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Bottom.Color.SetColor(colorBorder);
                            range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Left.Color.SetColor(colorBorder);
                            range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Right.Color.SetColor(colorBorder);
                            range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(classTipoRSD));
                            range.Style.Font.Color.SetColor(Color.Black);
                        }

                        colIniMw = colFinMw + 1;
                    }
                }

                //Nombre Reporte
                colIniNombreReporte = colIniNombreReporte - 3;
                int colFinNombreReporte = colFinEmp;

                //Fecha
                using (var range = ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Font.Bold = true;
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin, colorBorderHora);
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Color.SetColor(colorBorderHora);
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(colorBorderHora);
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Color.SetColor(colorBorderHora);
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Color.SetColor(colorBorderHora);
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(classHora));
                    range.Style.Font.Color.SetColor(Color.Black);
                }

                //MD
                using (var range = ws.Cells[rowIniMd, colIniMd, rowFinMd, colIniSein])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Font.Bold = true;
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin, colorBorderHora);
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Color.SetColor(colorBorderHora);
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(colorBorderHora);
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Color.SetColor(colorBorderHora);
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Color.SetColor(colorBorderHora);
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(classTipoSein));
                    range.Style.Font.Color.SetColor(Color.Black);
                }

                #endregion

                int rowIniData = rowIniMw + 1;
                row = rowIniData;

                #region cuerpo

                int numDia = 0;

                int colData = colIniFecha;
                for (var day = fechaIni.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
                {
                    numDia++;

                    List<MeMedicion48DTO> data = dataTotal.Where(x => x.Medifecha == day).ToList();
                    MeMedicion48DTO total = data.Find(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaTotalPotencia);
                    MeMedicion48DTO md = data.Find(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaMaximaDemanda);

                    DateTime horas = day.AddMinutes(30);

                    for (int h = 1; h <= 48; h++)
                    {
                        //Fecha
                        ws.Cells[row, colIniFecha].Value = horas.ToString(ConstantesAppServicio.FormatoFechaHora);
                        ws.Cells[row, colIniFecha].Style.Font.Size = 28;
                        ws.Cells[row, colIniFecha].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        colData = colIniFecha + 1;

                        //Maxima Demanda Programada
                        decimal? maximademanda = null;
                        if (md != null)
                        {
                            maximademanda = (decimal?)md.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(md, null);
                        }
                        ws.Cells[row, colData].Value = maximademanda;
                        colData++;

                        //Potencia Total
                        decimal? potenciaTotal = null;
                        if (total != null)
                        {
                            potenciaTotal = (decimal?)total.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(total, null);
                        }

                        ws.Cells[row, colData].Value = potenciaTotal;
                        colData++;

                        foreach (var empresa in listaEmpresa)
                        {
                            //RSD
                            MeMedicion48DTO reg = data.Find(x => x.Medifecha == day && x.Emprcodi == empresa.Emprcodi && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaRSD);
                            decimal? ior = null;

                            if (reg != null)
                            {
                                ior = ((decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg, null)).GetValueOrDefault(0);
                            }

                            if (ior.GetValueOrDefault(0) != 0)
                                ws.Cells[row, colData].Value = ior;

                            colData++;

                            //Error
                            MeMedicion48DTO regError = data.Find(x => x.Emprcodi == empresa.Emprcodi && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaRSDErrorBT);
                            decimal? error = null;
                            if (regError != null)
                            {
                                error = (decimal?)regError.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regError, null);
                            }
                            ws.Cells[row, colData].Value = error;
                            colData++;

                            //Tolerancia
                            bool mostrarJustificacion = false;
                            if (bt != null && error != null)
                            {
                                ws.Cells[row, colData].Value = bt.Mmmtolvalortolerancia;
                                mostrarJustificacion = error != null && bt.Mmmtolvalortolerancia < error;
                            }
                            colData++;

                            //Justificación
                            if (mostrarJustificacion)
                            {
                                MmmJustificacionDTO regJustif = dataJustif.Find(x => x.Mjustfecha == horas && x.Emprcodi == empresa.Emprcodi);
                                if (regJustif != null && regJustif.Mjustdescripcion != null)
                                {
                                    ws.Cells[row, colData].Value = regJustif.Mjustdescripcion;
                                }
                            }
                            colData++;
                        }

                        horas = horas.AddMinutes(30);
                        row++;
                    }
                }

                colData--;

                using (var range = ws.Cells[rowIniData, colIniFecha, (rowIniData + numDia * 48) - 1, colData])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.White);
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Style = ExcelBorderStyle.Dotted;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                }

                using (var range = ws.Cells[rowIniData, colIniFecha + 1, rowIniData + numDia * 48, colData])
                {
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.Style.Font.Size = 28;
                    range.Style.Numberformat.Format = "#,##0.000";
                }

                for (int f = rowIniData + 48; f <= rowIniData + numDia * 48; f += 48)
                {
                    ws.Cells[f, colIniFecha, f, colData].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                }

                //Formato de Filas y columnas
                for (int columna = colIniFecha + 1; columna <= colData; columna++)
                    ws.Column(columna).Width = 40;

                ws.Column(colIniFecha).Width = 50;

                ws.Row(rowIniEmp).Height = 400;
                ws.Row(rowIniMw).Height = 200;

                #endregion

                ws.View.FreezePanes(rowFinFecha + 1, colFinFreeze + 1);
            }
            ws.View.ZoomScale = 30;
        }

        /// <summary>
        /// Generación de Reporte Excel 5
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="ws"></param>
        /// <param name="nombreSheet"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="vermmcodi"></param>
        public void GenerarExcelReportePorcentajeErrorBT5(string empresas, ExcelWorksheet ws, DateTime fechaIni, DateTime fechaFin, int rowIni, int colIni, int colFinFreeze, int vermmcodi, List<MmmDatoDTO> listFacTable)
        {
            List<SiEmpresaDTO> listaEmpresa;
            List<MeMedicion48DTO> data = this.ListarIndicador(vermmcodi, fechaIni, fechaFin, ConstantesMonitoreo.CodigoILE, listFacTable, ConstantesMonitoreo.ReportePorcentajeErrorBandaTolerancia, out listaEmpresa);
            listaEmpresa = this.ListarEmpresasMonitoreoByData(listaEmpresa, empresas);

            List<MmmJustificacionDTO> dataJustif = this.ListMmmJustificacionByFechaAndIndicador(ConstantesMonitoreo.CodigoILE, fechaIni, fechaFin);
            MmmBandtolDTO bt = this.GetBandaToleranciaByIndicadorYPeriodo(ConstantesMonitoreo.CodigoILE, fechaIni);

            List<MeMedicion48DTO> listaBarra = data.Where(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCMEjec)
                .Select(x => new { Emprcodi = x.Emprcodi, Barrcodi = x.Barrcodi, Barrnombre = x.Barrnombre })
                .GroupBy(x => new { x.Emprcodi, x.Barrcodi, x.Barrnombre })
                .Select(x => new MeMedicion48DTO() { Emprcodi = x.Key.Emprcodi, Barrcodi = x.Key.Barrcodi, Barrnombre = x.Key.Barrnombre })
                .OrderBy(x => x.Barrnombre).ToList();

            int row = rowIni + 5;
            int col = colIni;
            //
            if (listaEmpresa.Count > 0)
            {
                #region cabecera

                int rowIniNombreReporte = row;
                /// Fila Hora - Empresa - Total

                int colIniFecha = col;
                int rowIniFecha = row;
                int rowFinFecha = rowIniFecha + 3 - 1;
                ws.Cells[rowIniFecha, colIniFecha].Value = "MM/DD/hh:mm";
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Font.Size = 28;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Merge = true;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Font.Bold = true;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.WrapText = true;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                int rowIniEmp = row;
                int colIniEmp = colIniFecha + 1;
                int colFinEmp = colIniEmp - 1;

                int colIniNombreReporte = colIniEmp;
                Color colorEmpresaImpar = ColorTranslator.FromHtml(ConstantesMonitoreo.ColorCmgEmpresaImpar);
                Color colorEmpresaPar = ColorTranslator.FromHtml(ConstantesMonitoreo.ColorCmgEmpresaPar);

                //Empresa
                int contEmpresa = 1;
                foreach (var empresa in listaEmpresa)
                {
                    int totalBarraXEmpr = listaBarra.Where(x => x.Emprcodi == empresa.Emprcodi).Count();
                    totalBarraXEmpr = totalBarraXEmpr > 0 ? totalBarraXEmpr : 1;

                    colFinEmp = colFinEmp + (4 * totalBarraXEmpr);

                    ws.Cells[rowIniEmp, colIniEmp].Value = empresa.Emprnomb;
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.Font.Size = 28;
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Merge = true;
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.WrapText = true;
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.Fill.BackgroundColor.SetColor(contEmpresa % 2 == 0 ? colorEmpresaPar : colorEmpresaImpar);

                    colIniEmp = colFinEmp + 1;
                    contEmpresa++;
                }

                int rowIniBarra = rowIniEmp + 1;
                int colIniBarra = colIniFecha + 1;
                int colFinBarra = colIniBarra - 1;

                //Barra X Empresa
                contEmpresa = 1;
                foreach (var empresa in listaEmpresa)
                {
                    var listaBarraXEmpr = listaBarra.Where(x => x.Emprcodi == empresa.Emprcodi).ToList();
                    int totalBarraXEmpr = listaBarraXEmpr.Count();
                    if (totalBarraXEmpr > 0)
                    {
                        foreach (var barra in listaBarraXEmpr)
                        {
                            colFinBarra = colFinBarra + 4;
                            ws.Cells[rowIniBarra, colIniBarra].Value = barra.Barrnombre;
                            ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.Font.Size = 28;
                            ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Merge = true;
                            ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.WrapText = true;
                            ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.Fill.BackgroundColor.SetColor(contEmpresa % 2 == 0 ? colorEmpresaPar : colorEmpresaImpar);
                            colIniBarra = colFinBarra + 1;
                        }
                    }
                    else
                    {
                        colFinBarra = colFinBarra + 4;
                        ws.Cells[rowIniBarra, colIniBarra].Value = "";
                        ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.Font.Size = 28;
                        ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Merge = true;
                        ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.WrapText = true;
                        ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.Fill.BackgroundColor.SetColor(contEmpresa % 2 == 0 ? colorEmpresaPar : colorEmpresaImpar);
                        colIniBarra = colFinBarra + 1;
                    }
                    contEmpresa++;
                }

                int rowIniMw = rowIniBarra + 1;
                int colIniMw = colIniFecha + 1;
                int colFinMw = colIniMw;

                //Valores x Barra
                contEmpresa = 1;
                foreach (var empresa in listaEmpresa)
                {
                    int totalBarraXEmpr = listaBarra.Where(x => x.Emprcodi == empresa.Emprcodi).Count();
                    totalBarraXEmpr = totalBarraXEmpr > 0 ? totalBarraXEmpr : 1;
                    for (int j = 0; j < totalBarraXEmpr; j++)
                    {
                        for (int i = 1; i <= 4; i++)
                        {
                            string valor; valor = (i == 1) ? "ILE   (Índice de Lerner)" : i == 2 ? "Error (%)" : i == 3 ? "Tolerancia" : "Justificación";
                            ws.Cells[rowIniMw, colIniMw].Value = valor;
                            colFinMw = colIniMw;
                            ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.Font.Size = 28;
                            ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Merge = true;
                            ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.WrapText = true;
                            ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.Fill.BackgroundColor.SetColor(contEmpresa % 2 == 0 ? colorEmpresaPar : colorEmpresaImpar);
                            colIniMw = colFinMw + 1;
                        }
                    }
                    contEmpresa++;
                }
                int colFinNombreReporte = colFinEmp;


                var colorBorder = Color.Black;
                using (var range = ws.Cells[rowIniNombreReporte, colIniNombreReporte, rowIniMw, colFinNombreReporte])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Font.Bold = true;
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin, colorBorder);
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Color.SetColor(colorBorder);
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(colorBorder);
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Color.SetColor(colorBorder);
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Color.SetColor(colorBorder);
                    range.Style.Font.Color.SetColor(Color.Black);
                }

                #endregion

                int rowIniData = rowIniMw + 1;
                row = rowIniData;

                #region cuerpo

                int numDia = 0;

                int colData = colIniFecha + 1;

                for (var day = fechaIni.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
                {
                    List<MeMedicion48DTO> dataILE = data.Where(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaILE).ToList();
                    List<MeMedicion48DTO> dataILEErrorBT = data.Where(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaILEErrorBT).ToList();

                    numDia++;
                    DateTime horas = day.AddMinutes(30);

                    for (int h = 1; h <= 48; h++)
                    {
                        colData = colIniFecha;
                        ws.Cells[row, colData].Value = horas.ToString(ConstantesAppServicio.FormatoFechaHora);

                        foreach (var empresa in listaEmpresa)
                        {
                            var listaBarraXEmpr = listaBarra.Where(x => x.Emprcodi == empresa.Emprcodi).ToList();
                            int totalBarraXEmpr = listaBarraXEmpr.Count();
                            if (totalBarraXEmpr > 0)
                            {
                                foreach (var barra in listaBarraXEmpr)
                                {
                                    MeMedicion48DTO objile = dataILE.Find(x => x.Medifecha == day && x.Barrcodi == barra.Barrcodi && x.Emprcodi == barra.Emprcodi);

                                    decimal? valorIle = null;
                                    bool tieneGeneracion = false;

                                    //ILE
                                    colData = colData + 1;
                                    if (objile != null)
                                    {
                                        valorIle = (decimal?)objile.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(objile, null);
                                        tieneGeneracion = objile.TieneIndicador;
                                        if (tieneGeneracion)
                                        {
                                            if (valorIle != null)
                                            {
                                                ws.Cells[row, colData].Value = valorIle;
                                            }
                                            else { ws.Cells[row, colData].Value = "(*)"; }
                                        }
                                    }

                                    //Error
                                    colData = colData + 1;
                                    MeMedicion48DTO regError = dataILEErrorBT.Find(x => x.Emprcodi == empresa.Emprcodi && x.Barrcodi == barra.Barrcodi);
                                    decimal? error = null;
                                    if (regError != null)
                                    {
                                        error = (decimal?)regError.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regError, null);
                                    }
                                    ws.Cells[row, colData].Value = error;

                                    //Tolerancia
                                    colData = colData + 1;
                                    bool mostrarJustificacion = false;
                                    if (bt != null && error != null)
                                    {
                                        ws.Cells[row, colData].Value = bt.Mmmtolvalortolerancia;
                                        mostrarJustificacion = error != null && bt.Mmmtolvalortolerancia < error;
                                    }

                                    //Justificación
                                    colData = colData + 1;
                                    if (mostrarJustificacion)
                                    {
                                        MmmJustificacionDTO regJustif = dataJustif.Find(x => x.Mjustfecha == horas && x.Emprcodi == empresa.Emprcodi && x.Barrcodi == barra.Barrcodi);
                                        if (regJustif != null && regJustif.Mjustdescripcion != null)
                                        {
                                            ws.Cells[row, colData].Value = regJustif.Mjustdescripcion;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                colData = colData + 1;
                                colData = colData + 1;
                                colData = colData + 1;
                                colData = colData + 1;
                            }
                        }

                        row = row + 1;


                        horas = horas.AddMinutes(30);
                    }
                }

                using (var range = ws.Cells[rowIniData, colIniFecha, (rowIniData + numDia * 48) - 1, colData])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.White);
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Style = ExcelBorderStyle.Dotted;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                }
                using (var range = ws.Cells[rowIniData, colIniFecha, (rowIniData + numDia * 48) - 1, colIniFecha])
                {
                    range.Style.Font.Size = 28;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                //Formato Data
                using (var range = ws.Cells[rowIniData, colIniFecha + 1, rowIniData + numDia * 48 - 1, colData])
                {
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.Style.Font.Size = 28;
                    range.Style.Numberformat.Format = "#,##0.00";
                }

                for (int f = rowIniData + 48; f <= rowIniData + numDia * 48; f += 48)
                {
                    ws.Cells[f, colIniFecha, f, colData].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                }

                for (int columna = colIniFecha + 1; columna <= colData; columna++)
                    ws.Column(columna).Width = 40;

                //Formato de Filas y columnas
                ws.Column(colIniFecha).Width = 60;
                ws.Row(rowIniEmp).Height = 120;
                ws.Row(rowIniBarra).Height = 100;
                ws.Row(rowIniMw).Height = 70;

                #endregion

                #region leyenda

                int rowIniLeyenda = row + 3;
                int rowIniIndeterminado = row + 5;

                ws.Cells[rowIniLeyenda, colIniFecha].Value = "Leyenda:";
                ws.Cells[rowIniLeyenda, colIniFecha].Style.Font.Size = 28;
                ws.Cells[rowIniLeyenda, colIniFecha].Style.Font.Bold = true;

                ws.Cells[rowIniIndeterminado, colIniFecha].Value = "(*)";
                ws.Cells[rowIniIndeterminado, colIniFecha].Style.Font.Size = 28;
                ws.Cells[rowIniIndeterminado, colIniFecha + 1].Value = " Indeterminado   (''/0'') ";
                ws.Cells[rowIniIndeterminado, colIniFecha + 1].Style.Font.Size = 28;

                #endregion

                ws.View.FreezePanes(rowFinFecha + 1, colFinFreeze + 1);
            }
            ws.View.ZoomScale = 30;
        }

        /// <summary>
        /// Generación de Reporte Excel 6
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="ws"></param>
        /// <param name="nombreSheet"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="vermmcodi"></param>
        public void GenerarExcelReportePorcentajeErrorBT6(string empresas, ExcelWorksheet ws, DateTime fechaIni, DateTime fechaFin, int rowIni, int colIni, int colFinFreeze, int vermmcodi, List<MmmDatoDTO> listFacTable)
        {
            List<SiEmpresaDTO> listaEmpresa;
            List<MeMedicion48DTO> data = this.ListarIndicador(vermmcodi, fechaIni, fechaFin, ConstantesMonitoreo.CodigoIMU, listFacTable, ConstantesMonitoreo.ReportePorcentajeErrorBandaTolerancia, out listaEmpresa);
            listaEmpresa = this.ListarEmpresasMonitoreoByData(listaEmpresa, empresas);

            List<MmmJustificacionDTO> dataJustif = this.ListMmmJustificacionByFechaAndIndicador(ConstantesMonitoreo.CodigoIMU, fechaIni, fechaFin);
            MmmBandtolDTO bt = this.GetBandaToleranciaByIndicadorYPeriodo(ConstantesMonitoreo.CodigoIMU, fechaIni);

            List<MeMedicion48DTO> listaBarra = data.Where(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCMEjec)
                .Select(x => new { Emprcodi = x.Emprcodi, Barrcodi = x.Barrcodi, Barrnombre = x.Barrnombre })
                .GroupBy(x => new { x.Emprcodi, x.Barrcodi, x.Barrnombre })
                .Select(x => new MeMedicion48DTO() { Emprcodi = x.Key.Emprcodi, Barrcodi = x.Key.Barrcodi, Barrnombre = x.Key.Barrnombre })
                .OrderBy(x => x.Barrnombre).ToList();

            int row = rowIni + 5;
            int col = colIni;
            //
            if (listaEmpresa.Count > 0)
            {
                #region cabecera

                int rowIniNombreReporte = row;
                /// Fila Hora - Empresa - Total

                int colIniFecha = col;
                int rowIniFecha = row;
                int rowFinFecha = rowIniFecha + 3 - 1;
                ws.Cells[rowIniFecha, colIniFecha].Value = "MM/DD/hh:mm";
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Font.Size = 28;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Merge = true;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Font.Bold = true;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.WrapText = true;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                int rowIniEmp = row;
                int colIniEmp = colIniFecha + 1;
                int colFinEmp = colIniEmp - 1;

                int colIniNombreReporte = colIniEmp;
                Color colorEmpresaImpar = ColorTranslator.FromHtml(ConstantesMonitoreo.ColorCmgEmpresaImpar);
                Color colorEmpresaPar = ColorTranslator.FromHtml(ConstantesMonitoreo.ColorCmgEmpresaPar);

                //Empresa
                int contEmpresa = 1;
                foreach (var empresa in listaEmpresa)
                {
                    int totalBarraXEmpr = listaBarra.Where(x => x.Emprcodi == empresa.Emprcodi).Count();
                    totalBarraXEmpr = totalBarraXEmpr > 0 ? totalBarraXEmpr : 1;

                    colFinEmp = colFinEmp + (4 * totalBarraXEmpr);

                    ws.Cells[rowIniEmp, colIniEmp].Value = empresa.Emprnomb;
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.Font.Size = 28;
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Merge = true;
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.WrapText = true;
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.Fill.BackgroundColor.SetColor(contEmpresa % 2 == 0 ? colorEmpresaPar : colorEmpresaImpar);

                    colIniEmp = colFinEmp + 1;
                    contEmpresa++;
                }

                int rowIniBarra = rowIniEmp + 1;
                int colIniBarra = colIniFecha + 1;
                int colFinBarra = colIniBarra - 1;

                //Barra X Empresa
                contEmpresa = 1;
                foreach (var empresa in listaEmpresa)
                {
                    var listaBarraXEmpr = listaBarra.Where(x => x.Emprcodi == empresa.Emprcodi).ToList();
                    int totalBarraXEmpr = listaBarraXEmpr.Count();
                    if (totalBarraXEmpr > 0)
                    {
                        foreach (var barra in listaBarraXEmpr)
                        {
                            colFinBarra = colFinBarra + 4;
                            ws.Cells[rowIniBarra, colIniBarra].Value = barra.Barrnombre;
                            ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.Font.Size = 28;
                            ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Merge = true;
                            ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.WrapText = true;
                            ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.Fill.BackgroundColor.SetColor(contEmpresa % 2 == 0 ? colorEmpresaPar : colorEmpresaImpar);
                            colIniBarra = colFinBarra + 1;
                        }
                    }
                    else
                    {
                        colFinBarra = colFinBarra + 4;
                        ws.Cells[rowIniBarra, colIniBarra].Value = "";
                        ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.Font.Size = 28;
                        ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Merge = true;
                        ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.WrapText = true;
                        ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[rowIniBarra, colIniBarra, rowIniBarra, colFinBarra].Style.Fill.BackgroundColor.SetColor(contEmpresa % 2 == 0 ? colorEmpresaPar : colorEmpresaImpar);
                        colIniBarra = colFinBarra + 1;
                    }
                    contEmpresa++;
                }

                int rowIniMw = rowIniBarra + 1;
                int colIniMw = colIniFecha + 1;
                int colFinMw = colIniMw;

                //Valores x Barra
                contEmpresa = 1;
                foreach (var empresa in listaEmpresa)
                {
                    int totalBarraXEmpr = listaBarra.Where(x => x.Emprcodi == empresa.Emprcodi).Count();
                    totalBarraXEmpr = totalBarraXEmpr > 0 ? totalBarraXEmpr : 1;
                    for (int j = 0; j < totalBarraXEmpr; j++)
                    {
                        for (int i = 1; i <= 4; i++)
                        {
                            string valor; valor = (i == 1) ? "IMU (Índice de Mark Up )" : i == 2 ? "Error (%)" : i == 3 ? "Tolerancia" : "Justificación";
                            ws.Cells[rowIniMw, colIniMw].Value = valor;
                            colFinMw = colIniMw;
                            ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.Font.Size = 28;
                            ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Merge = true;
                            ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.WrapText = true;
                            ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[rowIniMw, colIniMw, rowIniMw, colFinMw].Style.Fill.BackgroundColor.SetColor(contEmpresa % 2 == 0 ? colorEmpresaPar : colorEmpresaImpar);
                            colIniMw = colFinMw + 1;
                        }
                    }
                    contEmpresa++;
                }
                int colFinNombreReporte = colFinEmp;


                var colorBorder = Color.Black;
                using (var range = ws.Cells[rowIniNombreReporte, colIniNombreReporte, rowIniMw, colFinNombreReporte])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Font.Bold = true;
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin, colorBorder);
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Color.SetColor(colorBorder);
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(colorBorder);
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Color.SetColor(colorBorder);
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Color.SetColor(colorBorder);
                    range.Style.Font.Color.SetColor(Color.Black);
                }

                #endregion

                int rowIniData = rowIniMw + 1;
                row = rowIniData;

                #region cuerpo

                int numDia = 0;

                int colData = colIniFecha + 1;

                for (var day = fechaIni.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
                {
                    List<MeMedicion48DTO> dataIMU = data.Where(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaIMU).ToList();
                    List<MeMedicion48DTO> dataIMUErrorBT = data.Where(x => x.Medifecha == day && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaIMUErrorBT).ToList();

                    numDia++;
                    DateTime horas = day.AddMinutes(30);

                    for (int h = 1; h <= 48; h++)
                    {
                        colData = colIniFecha;
                        ws.Cells[row, colData].Value = horas.ToString(ConstantesAppServicio.FormatoFechaHora);
                        ws.Cells[row, colIniFecha].Style.Font.Size = 28;
                        ws.Cells[row, colIniFecha].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        foreach (var empresa in listaEmpresa)
                        {
                            var listaBarraXEmpr = listaBarra.Where(x => x.Emprcodi == empresa.Emprcodi).ToList();
                            int totalBarraXEmpr = listaBarraXEmpr.Count();
                            if (totalBarraXEmpr > 0)
                            {
                                foreach (var barra in listaBarraXEmpr)
                                {
                                    MeMedicion48DTO objimu = dataIMU.Find(x => x.Medifecha == day && x.Barrcodi == barra.Barrcodi && x.Emprcodi == barra.Emprcodi);

                                    decimal? valorImu = null;
                                    bool tieneGeneracion = false;

                                    //IMU
                                    colData = colData + 1;
                                    if (objimu != null)
                                    {
                                        valorImu = (decimal?)objimu.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(objimu, null);
                                        tieneGeneracion = objimu.TieneIndicador;
                                        if (tieneGeneracion)
                                        {
                                            if (valorImu != null)
                                            {
                                                ws.Cells[row, colData].Value = valorImu;
                                            }
                                            else { ws.Cells[row, colData].Value = "(*)"; }
                                        }
                                    }

                                    //Error
                                    colData = colData + 1;
                                    MeMedicion48DTO regError = dataIMUErrorBT.Find(x => x.Emprcodi == empresa.Emprcodi && x.Barrcodi == barra.Barrcodi);
                                    decimal? error = null;
                                    if (regError != null)
                                    {
                                        error = (decimal?)regError.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regError, null);
                                    }
                                    ws.Cells[row, colData].Value = error;

                                    //Tolerancia
                                    colData = colData + 1;
                                    bool mostrarJustificacion = false;
                                    if (bt != null && error != null)
                                    {
                                        ws.Cells[row, colData].Value = bt.Mmmtolvalortolerancia;
                                        mostrarJustificacion = error != null && bt.Mmmtolvalortolerancia < error;
                                    }

                                    //Justificación
                                    colData = colData + 1;
                                    if (mostrarJustificacion)
                                    {
                                        MmmJustificacionDTO regJustif = dataJustif.Find(x => x.Mjustfecha == horas && x.Emprcodi == empresa.Emprcodi && x.Barrcodi == barra.Barrcodi);
                                        if (regJustif != null && regJustif.Mjustdescripcion != null)
                                        {
                                            ws.Cells[row, colData].Value = regJustif.Mjustdescripcion;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                colData = colData + 1;
                                colData = colData + 1;
                                colData = colData + 1;
                            }
                        }

                        row = row + 1;

                        horas = horas.AddMinutes(30);
                    }
                }

                using (var range = ws.Cells[rowIniData, colIniFecha, (rowIniData + numDia * 48) - 1, colData])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.White);
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Style = ExcelBorderStyle.Dotted;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                }
                using (var range = ws.Cells[rowIniData, colIniFecha, (rowIniData + numDia * 48) - 1, colIniFecha])
                {
                    range.Style.Font.Size = 28;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                //Formato Data
                using (var range = ws.Cells[rowIniData, colIniFecha + 1, rowIniData + numDia * 48 - 1, colData])
                {
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.Style.Font.Size = 28;
                    range.Style.Numberformat.Format = "#,##0.00";
                }

                for (int f = rowIniData + 48; f <= rowIniData + numDia * 48; f += 48)
                {
                    ws.Cells[f, colIniFecha, f, colData].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                }

                for (int columna = colIniFecha + 1; columna <= colData; columna++)
                    ws.Column(columna).Width = 40;

                //Formato de Filas y columnas
                ws.Column(colIniFecha).Width = 60;
                ws.Row(rowIniEmp).Height = 120;
                ws.Row(rowIniBarra).Height = 100;
                ws.Row(rowIniMw).Height = 70;

                #endregion

                #region leyenda

                int rowIniLeyenda = row + 3;
                int rowIniIndeterminado = row + 5;

                ws.Cells[rowIniLeyenda, colIniFecha].Value = "Leyenda:";
                ws.Cells[rowIniLeyenda, colIniFecha].Style.Font.Size = 28;
                ws.Cells[rowIniLeyenda, colIniFecha].Style.Font.Bold = true;

                ws.Cells[rowIniIndeterminado, colIniFecha].Value = "(*)";
                ws.Cells[rowIniIndeterminado, colIniFecha].Style.Font.Size = 28;
                ws.Cells[rowIniIndeterminado, colIniFecha + 1].Value = " Indeterminado   (''/0'') ";
                ws.Cells[rowIniIndeterminado, colIniFecha + 1].Style.Font.Size = 28;

                #endregion

                ws.View.FreezePanes(rowFinFecha + 1, colFinFreeze + 1);
            }
            ws.View.ZoomScale = 30;
        }

        /// <summary>
        /// Generación de Reporte 1
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="ws"></param>
        /// <param name="nombreSheet"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="vermmcodi"></param>
        public void GenerarExcelReportePorcentajeErrorBT7(string empresas, ExcelWorksheet ws, DateTime fechaIni, DateTime fechaFin, int rowIni, int colIni, int colFinFreeze, int vermmcodi, List<MmmDatoDTO> listFacTable)
        {
            DateTime fechaPeriodo = new DateTime(fechaIni.Year, fechaIni.Month, 1).Date;
            DateTime fechaFinPeriodo = fechaPeriodo.AddMonths(1).AddDays(-1);

            List<SiEmpresaDTO> listaEmpresa;
            List<MeMedicion48DTO> dataTotal = this.ListarIndicador(vermmcodi, fechaIni, fechaFin, ConstantesMonitoreo.CodigoIRT, listFacTable, ConstantesMonitoreo.ReportePorcentajeErrorBandaTolerancia, out listaEmpresa);
            listaEmpresa = this.ListarEmpresasMonitoreo(fechaIni, empresas);

            List<MmmJustificacionDTO> dataJustif = this.ListMmmJustificacionByFechaAndIndicador(ConstantesMonitoreo.CodigoIRT, fechaIni, fechaFin);
            MmmBandtolDTO bt = this.GetBandaToleranciaByIndicadorYPeriodo(ConstantesMonitoreo.CodigoIRT, fechaIni);

            List<int> listaEmprcodi = listaEmpresa.Select(x => x.Emprcodi).ToList();

            //Congestiones
            List<MeMedicion48DTO> listaCongXAreaXFecha = dataTotal.Where(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCongestion
                && listaEmprcodi.Contains(x.Emprcodi)).ToList();
            listaCongXAreaXFecha = this.ListarCongXAreaYFechaReporte7(listaCongXAreaXFecha);

            //Grupos despacho
            List<MeMedicion48DTO> listaBarraEmpGrupo = dataTotal.Where(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaEnlaceTrans
                && listaEmprcodi.Contains(x.Emprcodi)).ToList();

            if (listaBarraEmpGrupo.Count == 0)
            {
                listaBarraEmpGrupo.Add(new MeMedicion48DTO() { Grupocodi = -1, Gruponomb = string.Empty, Grupopadre = -2, Central = string.Empty, Barrcodi = -1, Barrnombre = string.Empty });
            }

            int row = rowIni + 2;
            int col = colIni;

            #region Congestiones
            /// Fila Hora - Empresa - Total

            int colCongestionIni = col;
            int rowCongestion = row;

            //            CONGESTIÓN	
            //Desde:  01/08/2018 Hasta: 31/08/2018	

            ws.Cells[rowCongestion, colCongestionIni].Value = "CONGESTIÓN";
            ws.Cells[rowCongestion, colCongestionIni].Style.WrapText = true;
            ws.Cells[rowCongestion, colCongestionIni].Style.Font.Size = 36;
            ws.Cells[rowCongestion, colCongestionIni].Style.Font.Bold = true;

            ws.Cells[rowCongestion + 1, colCongestionIni].Value = "Desde: " + fechaPeriodo.ToString(ConstantesBase.FormatoFechaPE) + " Hasta: " + fechaFinPeriodo.ToString(ConstantesBase.FormatoFechaPE);
            ws.Cells[rowCongestion + 1, colCongestionIni].Style.Font.Size = 36;
            ws.Cells[rowCongestion + 1, colCongestionIni].Style.Font.Bold = true;

            #region cabecera
            string nombreCampo = string.Empty;

            int rowIniTblConges = rowCongestion + 3;
            for (int i = 1; i <= 7; i++)
            {
                if (i == 1)
                {
                    nombreCampo = "FECHA";
                }

                if (i == 2)
                {
                    nombreCampo = "INICIO";
                }

                if (i == 3)
                {
                    nombreCampo = "FINAL";
                }

                if (i == 4)
                {
                    nombreCampo = "UBICACIÓN";
                }

                if (i == 5)
                {
                    nombreCampo = "EQUIPO";
                }

                if (i == 6)
                {
                    nombreCampo = "OBSERVACIONES";
                }

                if (i == 7)
                {
                    nombreCampo = "GENERACIÓN CON UNA LOCALIZACIÓN ESPECÍFICA, QUE OBTENGAN PODER DE MERCADO";
                }

                ws.Cells[rowIniTblConges, colCongestionIni + i - 1].Value = nombreCampo;
                ws.Cells[rowIniTblConges, colCongestionIni + i - 1].Merge = true;
                ws.Cells[rowIniTblConges, colCongestionIni + i - 1].Style.WrapText = true;
                ws.Cells[rowIniTblConges, colCongestionIni + i - 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniTblConges, colCongestionIni + i - 1].Style.Font.Size = 36;
            }

            var colorBorder = Color.Black;
            var classTipoConges = "#2980B9";
            using (var range = ws.Cells[rowIniTblConges, colCongestionIni, rowIniTblConges, colCongestionIni + 7 - 1])
            {
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Font.Bold = true;
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin, colorBorder);
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Top.Color.SetColor(colorBorder);
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Color.SetColor(colorBorder);
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Color.SetColor(colorBorder);
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Color.SetColor(colorBorder);
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(classTipoConges));
                range.Style.Font.Color.SetColor(Color.White);
            }

            #endregion

            #region Cuerpo
            int colDataIni = colCongestionIni;
            int rowDataIni = rowIniTblConges + 1;
            int colDataFin = colDataIni + 7 - 1;
            string dato = string.Empty;
            foreach (var conges in listaCongXAreaXFecha)
            {
                for (int i = 1; i <= 7; i++)
                {
                    if (i == 1)
                    {
                        dato = conges.Hophorini.ToString(ConstantesAppServicio.FormatoFecha);
                    }

                    if (i == 2)
                    {
                        dato = conges.Hophorini.ToString(ConstantesAppServicio.FormatoOnlyHora);
                    }

                    if (i == 3)
                    {
                        dato = conges.Hophorfin.ToString(ConstantesAppServicio.FormatoOnlyHora);
                    }

                    if (i == 4)
                    {
                        dato = conges.Areanomb;
                    }

                    if (i == 5)
                    {
                        dato = conges.Equinomb;
                    }

                    if (i == 6)
                    {
                        dato = conges.Descripcion;
                    }
                    if (i == 7)
                    {
                        dato = conges.Gruponomb;
                    }

                    ws.Cells[rowDataIni, colCongestionIni + i - 1].Value = dato;
                    ws.Cells[rowDataIni, colCongestionIni + i - 1].Style.WrapText = true;
                    ws.Cells[rowDataIni, colCongestionIni + i - 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowDataIni, colCongestionIni + i - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[rowDataIni, colCongestionIni + i - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[rowDataIni, colCongestionIni + i - 1].Style.Font.Size = 36;
                    ws.Cells[rowDataIni, colCongestionIni + i - 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[rowDataIni, colCongestionIni + i - 1].Style.Font.Color.SetColor(ColorTranslator.FromHtml(classTipoConges));
                    ws.Cells[rowDataIni, colCongestionIni + i - 1].Style.Fill.BackgroundColor.SetColor(Color.White);
                }
                ws.Row(rowIniTblConges).Height = 90;
                rowDataIni = rowDataIni + 1;
            }

            for (int columna = colDataIni + 1; columna < colDataFin; columna++)
                ws.Column(columna).Width = 84;
            ws.Column(colDataFin).Width = 200;

            ws.Row(rowIniTblConges).Height = 100;

            #endregion

            #endregion

            row = rowDataIni + 5;
            //
            if (listaEmpresa.Count > 0)
            {
                #region cabecera

                /// Fila Hora - Empresa - Total

                int colIniFecha = col;
                int rowIniFecha = row;
                int rowFinFecha = rowIniFecha + 4 - 1;
                ws.Cells[rowIniFecha, colIniFecha].Value = "MM/DD/hh:mm";
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Merge = true;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.WrapText = true;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Font.Bold = true;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Font.Size = 28;

                int rowIniEnl = row;
                int colIniEnl = colIniFecha + 1;
                int colFinEnl = colIniEnl;
                int colIniIrt = colIniFecha;
                int rowFinIrt = rowIniEnl + 3;
                int rowIniNombres = rowIniEnl + 2;
                int rowIniLinea = rowIniEnl + 1;

                int colIniReporte = colIniEnl;

                Color colorEnlace = ColorTranslator.FromHtml("#9BC2E6");

                //Enlace de transmision
                foreach (var grupo in listaBarraEmpGrupo)
                {
                    colFinEnl = colIniEnl + 3;

                    //
                    ws.Cells[rowIniEnl, colIniEnl].Value = "Enlace de transmisión";
                    ws.Cells[rowIniEnl, colIniEnl, rowIniEnl, colFinEnl].Style.Font.Size = 24;
                    ws.Cells[rowIniEnl, colIniEnl, rowIniEnl, colFinEnl].Merge = true;
                    ws.Cells[rowIniEnl, colIniEnl, rowIniEnl, colFinEnl].Style.WrapText = true;
                    ws.Cells[rowIniEnl, colIniEnl, rowIniEnl, colFinEnl].Style.Font.Bold = true;
                    ws.Cells[rowIniEnl, colIniEnl, rowIniEnl, colFinEnl].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniEnl, colIniEnl, rowIniEnl, colFinEnl].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[rowIniEnl, colIniEnl, rowIniEnl, colFinEnl].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[rowIniEnl, colIniEnl, rowIniEnl, colFinEnl].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[rowIniEnl, colIniEnl, rowIniEnl, colFinEnl].Style.Font.Color.SetColor(Color.Black);
                    ws.Cells[rowIniEnl, colIniEnl, rowIniEnl, colFinEnl].Style.Fill.BackgroundColor.SetColor(colorEnlace);

                    //
                    ws.Cells[rowIniLinea, colIniEnl].Value = grupo.Equinomb;
                    ws.Cells[rowIniLinea, colIniEnl, rowIniLinea, colFinEnl].Style.Font.Size = 24;
                    ws.Cells[rowIniLinea, colIniEnl, rowIniLinea, colFinEnl].Merge = true;
                    ws.Cells[rowIniLinea, colIniEnl, rowIniLinea, colFinEnl].Style.WrapText = true;
                    ws.Cells[rowIniLinea, colIniEnl, rowIniLinea, colFinEnl].Style.Font.Bold = true;
                    ws.Cells[rowIniLinea, colIniEnl, rowIniLinea, colFinEnl].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniLinea, colIniEnl, rowIniLinea, colFinEnl].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[rowIniLinea, colIniEnl, rowIniLinea, colFinEnl].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    //       
                    ws.Cells[rowIniNombres, colIniEnl].Value = grupo.Gruponomb;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.Font.Size = 24;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Merge = true;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.WrapText = true;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.Font.Bold = true;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    ws.Cells[rowFinIrt, colIniEnl].Value = "PU (MW)";
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Font.Size = 24;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Merge = true;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.WrapText = true;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Font.Bold = true;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Font.Color.SetColor(Color.Black);
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Fill.BackgroundColor.SetColor(colorEnlace);

                    //
                    colIniEnl++;
                    ws.Cells[rowIniNombres, colIniEnl].Value = grupo.Gruponomb;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.Font.Size = 24;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Merge = true;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.WrapText = true;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.Font.Bold = true;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    ws.Cells[rowFinIrt, colIniEnl].Value = "PP (MW)";
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Font.Size = 24;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Merge = true;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.WrapText = true;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Font.Bold = true;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Font.Color.SetColor(Color.Black);
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Fill.BackgroundColor.SetColor(colorEnlace);

                    //
                    colIniEnl++;
                    ws.Cells[rowIniNombres, colIniEnl].Value = grupo.Barrnombre;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.Font.Size = 24;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Merge = true;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.WrapText = true;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.Font.Bold = true;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    ws.Cells[rowFinIrt, colIniEnl].Value = "CMg (S/ MWh)";
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Font.Size = 24;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Merge = true;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.WrapText = true;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Font.Bold = true;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Font.Color.SetColor(Color.Black);
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Fill.BackgroundColor.SetColor(colorEnlace);

                    //
                    colIniEnl++;
                    ws.Cells[rowIniNombres, colIniEnl].Value = grupo.Barrnombre;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.Font.Size = 24;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Merge = true;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.WrapText = true;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.Font.Bold = true;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[rowIniNombres, colIniEnl, rowIniNombres, colIniEnl].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    ws.Cells[rowFinIrt, colIniEnl].Value = "CMgprog (S/ MWh)";
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Font.Size = 24;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Merge = true;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.WrapText = true;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Font.Bold = true;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Font.Color.SetColor(Color.Black);
                    ws.Cells[rowFinIrt, colIniEnl, rowFinIrt, colIniEnl].Style.Fill.BackgroundColor.SetColor(colorEnlace);

                    //
                    colIniIrt = colFinEnl + 1;
                    ws.Cells[rowIniEnl, colIniIrt].Value = "IRT";
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Style.Font.Size = 24;
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Merge = true;
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Style.WrapText = true;
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Style.Font.Bold = true;
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Style.Font.Color.SetColor(Color.Black);
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Style.Fill.BackgroundColor.SetColor(colorEnlace);

                    //
                    colIniIrt = colIniIrt + 1;
                    ws.Cells[rowIniEnl, colIniIrt].Value = "Error (%)";
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Style.Font.Size = 24;
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Merge = true;
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Style.WrapText = true;
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Style.Font.Bold = true;
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Style.Font.Color.SetColor(Color.Black);
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Style.Fill.BackgroundColor.SetColor(colorEnlace);

                    //
                    colIniIrt = colIniIrt + 1;
                    ws.Cells[rowIniEnl, colIniIrt].Value = "Tolerancia";
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Style.Font.Size = 24;
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Merge = true;
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Style.WrapText = true;
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Style.Font.Bold = true;
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Style.Font.Color.SetColor(Color.Black);
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Style.Fill.BackgroundColor.SetColor(colorEnlace);

                    //
                    colIniIrt = colIniIrt + 1;
                    ws.Cells[rowIniEnl, colIniIrt].Value = "Justificación";
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Style.Font.Size = 24;
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Merge = true;
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Style.WrapText = true;
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Style.Font.Bold = true;
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Style.Font.Color.SetColor(Color.Black);
                    ws.Cells[rowIniEnl, colIniIrt, rowFinIrt, colIniIrt].Style.Fill.BackgroundColor.SetColor(colorEnlace);

                    colIniEnl = colIniIrt + 1;
                }

                #endregion

                int rowIniData = rowFinIrt + 1;
                row = rowIniData;

                #region cuerpo

                int numDia = 0;

                int colData = colIniFecha;
                for (var day = fechaIni.Date; day.Date <= fechaFin; day = day.AddDays(1))
                {
                    numDia++;
                    DateTime horas = day.AddMinutes(30);

                    List<MeMedicion48DTO> dataXDia = dataTotal.Where(x => x.Medifecha == day).ToList();

                    for (int h = 1; h <= 48; h++)
                    {
                        //Fecha
                        ws.Cells[row, colIniFecha].Value = horas.ToString(ConstantesAppServicio.FormatoFechaHora);
                        ws.Cells[row, colIniFecha].Style.Font.Size = 28;
                        ws.Cells[row, colIniFecha].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        colData = colIniFecha;

                        foreach (var grupo in listaBarraEmpGrupo)
                        {
                            MeMedicion48DTO regIRT = dataXDia.Find(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaIRT && x.Grupocodi == grupo.Grupocodi);
                            if (regIRT != null)
                            {
                                decimal? irt = (decimal?)regIRT.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regIRT, null);
                                if (irt != null)
                                {
                                    //Potencia ejecutada
                                    MeMedicion48DTO regpotEjec = this.GetPotenciaTotalXDiaByGrupoReporte7(day, dataXDia.Where(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaPotGrupoEjec).ToList(), grupo.Grupocodi);
                                    decimal? potEjec = null;
                                    if (regpotEjec != null)
                                    {
                                        potEjec = (decimal?)regpotEjec.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regpotEjec, null);
                                    }
                                    colData++;
                                    if (potEjec.GetValueOrDefault(0) != 0)
                                        ws.Cells[row, colData].Value = potEjec;

                                    //Potencia programada
                                    MeMedicion48DTO regpotProg = this.GetPotenciaTotalXDiaByGrupoReporte7(day, dataXDia.Where(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaPotGrupoProg).ToList(), grupo.Grupocodi);
                                    decimal? potProg = null;
                                    if (regpotProg != null)
                                    {
                                        potProg = (decimal?)regpotProg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regpotProg, null);
                                    }
                                    colData++;
                                    if (potProg.GetValueOrDefault(0) != 0)
                                        ws.Cells[row, colData].Value = potProg;

                                    //Costos marg Ejecutados
                                    MeMedicion48DTO regCMgejec = dataXDia.Find(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCMEjec && x.Barrcodi == grupo.Barrcodi && x.Emprcodi == grupo.Emprcodi);
                                    decimal? cmg = null;
                                    if (regCMgejec != null && potEjec.GetValueOrDefault(0) != 0)
                                    {
                                        cmg = (decimal?)regCMgejec.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regCMgejec, null);
                                        if (cmg != null)
                                            cmg = cmg * 1000;
                                    }
                                    colData++;
                                    if (cmg.GetValueOrDefault(0) != 0)
                                        ws.Cells[row, colData].Value = cmg;

                                    //Costos marg Programados
                                    MeMedicion48DTO regCMgprog = dataXDia.Find(x => x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaCMProg && x.Cnfbarcodi == grupo.Cnfbarcodi && x.Emprcodi == grupo.Emprcodi);
                                    decimal? cmgProg = null;
                                    if (regCMgprog != null && potProg.GetValueOrDefault(0) != 0)
                                    {
                                        cmgProg = (decimal?)regCMgprog.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regCMgprog, null);
                                    }
                                    colData++;
                                    if (cmgProg.GetValueOrDefault(0) != 0)
                                        ws.Cells[row, colData].Value = cmgProg;

                                    //IRT
                                    colData++;
                                    if (irt.GetValueOrDefault(0) != 0)
                                        ws.Cells[row, colData].Value = irt;

                                    //Error
                                    colData++;
                                    MeMedicion48DTO regError = dataXDia.Find(x => x.Grupocodi == grupo.Grupocodi && x.TipoFormulaMonitoreo == ConstantesMonitoreo.TipoFormulaIRTErrorBT);
                                    decimal? error = null;
                                    if (regError != null)
                                    {
                                        error = (decimal?)regError.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regError, null);
                                    }
                                    ws.Cells[row, colData].Value = error;

                                    //Tolerancia
                                    colData++;
                                    bool mostrarJustificacion = false;
                                    if (bt != null && error != null)
                                    {
                                        ws.Cells[row, colData].Value = bt.Mmmtolvalortolerancia;
                                        mostrarJustificacion = error != null && bt.Mmmtolvalortolerancia < error;
                                    }

                                    //Justificación
                                    colData++;
                                    if (mostrarJustificacion)
                                    {
                                        MmmJustificacionDTO regJustif = dataJustif.Find(x => x.Mjustfecha == horas && x.Grupocodi == grupo.Grupocodi);
                                        if (regJustif != null && regJustif.Mjustdescripcion != null)
                                        {
                                            ws.Cells[row, colData].Value = regJustif.Mjustdescripcion;
                                        }
                                    }
                                }
                                else
                                {
                                    colData++;
                                    colData++;
                                    colData++;
                                    colData++;
                                    colData++;
                                    colData++;
                                    colData++;
                                    colData++;
                                }
                            }
                            else
                            {
                                colData++;
                                colData++;
                                colData++;
                                colData++;
                                colData++;
                                colData++;
                                colData++;
                                colData++;
                            }
                        }
                        horas = horas.AddMinutes(30);
                        row++;
                    }
                }

                using (var range = ws.Cells[rowIniData, colIniFecha, (rowIniData + numDia * 48) - 1, colData])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.White);
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Style = ExcelBorderStyle.Dotted;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                }

                //Formato Data
                using (var range = ws.Cells[rowIniData, colIniFecha + 1, (rowIniData + numDia * 48) - 1, colData])
                {
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.Style.Font.Size = 28;
                    range.Style.Numberformat.Format = "#,##0.000";
                }

                for (int f = rowIniData + 48; f <= rowIniData + numDia * 48; f += 48)
                {
                    ws.Cells[f, colIniFecha, f, colData].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                }

                //Formato de Filas y columnas
                for (int columna = colIniReporte; columna <= colData; columna++)
                    ws.Column(columna).Width = 80;

                ws.Column(colIniFecha).Width = 50;
                ws.Row(rowIniFecha).Height = 70;
                ws.Row(rowIniLinea).Height = 70;
                ws.Row(rowIniNombres).Height = 70;
                ws.Row(rowFinIrt).Height = 70;

                #endregion

                ws.View.FreezePanes(rowFinFecha + 1, colFinFreeze + 1);
            }
            ws.View.ZoomScale = 30;
        }

        #endregion

    }
}