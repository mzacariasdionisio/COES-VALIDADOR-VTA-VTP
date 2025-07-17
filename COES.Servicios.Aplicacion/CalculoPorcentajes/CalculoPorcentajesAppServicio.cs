using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Servicios.Aplicacion.CalculoPorcentajes.Helper;
using System.Data;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using System.Globalization;
using System.Configuration;
using COES.Servicios.Aplicacion.FormatoMedicion;

namespace COES.Servicios.Aplicacion.CalculoPorcentajes
{
    /// <summary>
    /// Clases con métodos del módulo CalculoPorcentajes
    /// </summary>
    public class CalculoPorcentajesAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(CalculoPorcentajesAppServicio));

        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        ReporteMedidoresAppServicio servicioReporteMedidores = new ReporteMedidoresAppServicio();
        ConsultaMedidoresAppServicio servicioConsultaMedidores = new ConsultaMedidoresAppServicio();
        FormatoMedicionAppServicio logic = new FormatoMedicionAppServicio();

        #region Métodos Tabla CONSULTA A SGOCOES
        /// <summary>
        /// Permite obtener la lista completa de los puntos de medición
        /// </summary>
        /// <returns></returns>
        public List<MePtomedicionDTO> ListMePtomedicion(string ptomedicodi, string origlectcodi)
        {
            return FactorySic.GetMePtomedicionRepository().List(ptomedicodi, origlectcodi);
        }

        /// <summary>
        /// Permite obtener una entidad PtoMedicion a partir del emprcodi y ptomedidesc
        /// </summary>
        /// <param name="Emprcodi">Identificador de la Empresa</param>
        /// <param name="Ptomedidesc">Nombre del punto de medición</param>
        public MePtomedicionDTO GetMePtomedicionByNombre(int Emprcodi, string Ptomedidesc)
        {
            return FactoryTransferencia.GetCaiAjusteempresaRepository().GetMePtomedicionByNombre(Emprcodi, Ptomedidesc);
        }
        
        #endregion
    
        #region Métodos Tabla CAI_AJUSTE

        /// <summary>
        /// Inserta un registro de la tabla CAI_AJUSTE
        /// </summary>
        public int SaveCaiAjuste(CaiAjusteDTO entity)
        {
            int id = 0;
            try
            {
                id = FactoryTransferencia.GetCaiAjusteRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return id;
        }

        /// <summary>
        /// Actualiza un registro de la tabla CAI_AJUSTE
        /// </summary>
        public void UpdateCaiAjuste(CaiAjusteDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCaiAjusteRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CAI_AJUSTE
        /// </summary>
        /// <param name="caiajcodi">Identificador de la Tabla CAI_AJUSTE</param>
        public void DeleteCaiAjuste(int caiajcodi)
        {
            try
            {
                FactoryTransferencia.GetCaiAjusteRepository().Delete(caiajcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CAI_AJUSTE
        /// </summary>
        /// <param name="caiajcodi">Identificador de la Tabla CAI_AJUSTE</param>
        public CaiAjusteDTO GetByIdCaiAjuste(int caiajcodi)
        {
            return FactoryTransferencia.GetCaiAjusteRepository().GetById(caiajcodi);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CAI_AJUSTE a partir del actual ajuste
        /// </summary>
        /// <param name="caiajcodi">Identificador de la Tabla CAI_AJUSTE</param>
        public CaiAjusteDTO GetByIdCaiAjusteAnterior(int caiajcodi)
        {
            return FactoryTransferencia.GetCaiAjusteRepository().GetByIdAnterior(caiajcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CAI_AJUSTE
        /// </summary>
        /// <param name="caiprscodi">Identificador de la Tabla CAI_PRESUPUESTO</param>
        public List<CaiAjusteDTO> ListCaiAjustes(int caiprscodi)
        {
            return FactoryTransferencia.GetCaiAjusteRepository().List(caiprscodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CaiAjuste
        /// </summary>
        public List<CaiAjusteDTO> GetByCriteriaCaiAjustes()
        {
            return FactoryTransferencia.GetCaiAjusteRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite listar los registros de la tabla CAI_AJUSTE por codigo del presupuesto
        /// </summary>
        /// <param name="caiprscodi">Identificador de la Tabla CAI_PRESUPUESTO</param>
        public List<CaiAjusteDTO> ListByCaiPrscodi(int caiprscodi)
        {
            return FactoryTransferencia.GetCaiAjusteRepository().ListByCaiPrscodi(caiprscodi);
        }

        #endregion

        #region Métodos Tabla CAI_AJUSTECMARGINAL

        /// <summary>
        /// Inserta un registro de la tabla CAI_AJUSTECMARGINAL
        /// </summary>
        public void SaveCaiAjustecmarginal(CaiAjustecmarginalDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCaiAjustecmarginalRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CAI_AJUSTECMARGINAL
        /// </summary>
        public void UpdateCaiAjustecmarginal(CaiAjustecmarginalDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCaiAjustecmarginalRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CAI_AJUSTECMARGINAL
        /// </summary>
        /// <param name="caiajcodi">Identificador de la tabla Ajuste</param>
        public void DeleteCaiAjustecmarginal(int caiajcodi)
        {
            try
            {
                FactoryTransferencia.GetCaiAjustecmarginalRepository().Delete(caiajcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CAI_AJUSTECMARGINAL
        /// </summary>
        /// <param name="caajcmcodi">Identificador de la Tabla CAI_AJUSTECMARGINAL</param>
        public CaiAjustecmarginalDTO GetByIdCaiAjustecmarginal(int caajcmcodi)
        {
            return FactoryTransferencia.GetCaiAjustecmarginalRepository().GetById(caajcmcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CAI_AJUSTECMARGINAL
        /// </summary>
        /// <param name="caiajcodi">Identificador de la Tabla CAI_AJUSTE</param>
        public List<CaiAjustecmarginalDTO> ListCaiAjustecmarginals(int caiajcodi)
        {
            return FactoryTransferencia.GetCaiAjustecmarginalRepository().List(caiajcodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CaiAjustecmarginal
        /// </summary>
        public List<CaiAjustecmarginalDTO> GetByCriteriaCaiAjustecmarginals()
        {
            return FactoryTransferencia.GetCaiAjustecmarginalRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla CAI_AJUSTEEMPRESA

        /// <summary>
        /// Inserta un registro de la tabla CAI_AJUSTEEMPRESA
        /// </summary>
        public void SaveCaiAjusteempresa(CaiAjusteempresaDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCaiAjusteempresaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CAI_AJUSTEEMPRESA
        /// </summary>
        public void UpdateCaiAjusteempresa(CaiAjusteempresaDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCaiAjusteempresaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CAI_AJUSTEEMPRESA
        /// </summary>
        /// <param name="caiajecodi">Identificador de la Tabla CAI_AJUSTEEMPRESA</param>
        public void DeleteCaiAjusteempresa(int caiajecodi)
        {
            try
            {
                FactoryTransferencia.GetCaiAjusteempresaRepository().Delete(caiajecodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CAI_AJUSTEEMPRESA
        /// </summary>
        /// <param name="caiajecodi">Identificador de la Tabla CAI_AJUSTEEMPRESA</param>
        public CaiAjusteempresaDTO GetByIdCaiAjusteempresa(int caiajecodi)
        {
            return FactoryTransferencia.GetCaiAjusteempresaRepository().GetById(caiajecodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CAI_AJUSTEEMPRESA
        /// </summary>
        public List<CaiAjusteempresaDTO> ListCaiAjusteempresas()
        {
            return FactoryTransferencia.GetCaiAjusteempresaRepository().List();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CAI_AJUSTEEMPRESA por Ajuste
        /// </summary>
        /// <param name="caiajcodi">Identificador de la tabla CAI_AJUSTE</param>
        public List<CaiAjusteempresaDTO> ListCaiAjusteempresasByAjuste(int caiajcodi, string caiajetipoinfo)
        {
            return FactoryTransferencia.GetCaiAjusteempresaRepository().ListAjuste(caiajcodi, caiajetipoinfo);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CAI_AJUSTEEMPRESA Por ajuste y empresa
        /// </summary>
        /// <param name="caiajcodi">Identificador de la tabla CAI_AJUSTE</param>
        /// <param name="emprcodi">Identificador de la tabla SI_EMPRESA</param>
        public List<CaiAjusteempresaDTO> ListCaiAjusteempresasByAjusteEmpresa(int caiajcodi, int emprcodi)
        {
            return FactoryTransferencia.GetCaiAjusteempresaRepository().ListAjusteEmpresa(caiajcodi, emprcodi);
        }

        /// <summary>
        /// Permite listar todos las empress de la tabla CAI_AJUSTEEMPRESA Por ajuste y tipoempresa
        /// </summary>
        /// <param name="caiajcodi">Identificador de la tabla CAI_AJUSTE</param>
        /// <param name="tipoemprcodi">Identificador de la tabla SI_TIPOEMPRESA</param>
        public List<CaiAjusteempresaDTO> ListCaiAjusteempresasByAjusteTipoEmpresa(int caiajcodi, int tipoemprcodi)
        {
            return FactoryTransferencia.GetCaiAjusteempresaRepository().ListEmpresaByAjusteTipoEmpresa(caiajcodi, tipoemprcodi);
        }

        /// <summary>
        /// Permite listar todas las Empresas de la tabla CAI_AJUSTEEMPRESA por Ajuste
        /// </summary>
        /// <param name="caiajcodi">Identificador de la tabla CAI_AJUSTE</param>
        public List<CaiAjusteempresaDTO> ListCaiAjusteempresasListEmpresasByAjuste(int caiajcodi)
        {
            return FactoryTransferencia.GetCaiAjusteempresaRepository().ListEmpresasByAjuste(caiajcodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CAI_AJUSTEEMPRESA
        /// </summary>
        public List<CaiAjusteempresaDTO> GetByCriteriaCaiAjusteempresas()
        {
            return FactoryTransferencia.GetCaiAjusteempresaRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite generar el archivo de exportación de la tabla CaiAjusteEmpresa
        /// </summary>
        /// <param name="caiajcodi">Código de la Versión del Ajuste</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarFormatoCaiAjusteEmpresa(int caiajcodi, string caiajetipoinfo, int formato, string pathFile, string pathLogo)
        {
            string fileName = string.Empty;
            CaiAjusteDTO EntidadAjuste = this.GetByIdCaiAjuste(caiajcodi);
            CaiPresupuestoDTO EntidadPresupuesto = this.GetByIdCaiPresupuesto(EntidadAjuste.Caiprscodi);
            List<CaiAjusteempresaDTO> ListaAjusteEmpresa = this.ListCaiAjusteempresasByAjuste(caiajcodi, caiajetipoinfo);

            if (formato == 1)
            {
                fileName = "ReporteAjusteEmpresas.xlsx";
                ExcelDocument.GenerarFormatoCaiAjusteEmpresas(pathFile + fileName, EntidadPresupuesto, EntidadAjuste, ListaAjusteEmpresa, caiajetipoinfo);
            }

            return fileName;
        }

        /// <summary>
        /// Permite listar las fechas ejecutadas (reportadas y pendientes) de una empresa registrado en la tabla CAI_AJUSTEEMPRESA
        /// </summary>
        /// <param name="caiajetipoinfo">E:Energia / T:Transmision</param>
        /// <param name="caiajcodi">Identificador de la tabla CAI_AJUSTE</param>
        /// <param name="emprcodi">Identificador de la tabla SI_EMPRESA</param>
        public List<CaiAjusteempresaDTO> ObtenerListaPeriodoEjecutado(string caiajetipoinfo, int caiajcodi, int emprcodi)
        {
            return FactoryTransferencia.GetCaiAjusteempresaRepository().ObtenerListaPeriodoEjecutado(caiajetipoinfo, caiajcodi, emprcodi);
        }

        /// <summary>
        /// Permite listar las fechas de proyección de una empresa registrado en la tabla CAI_AJUSTEEMPRESA
        /// </summary>
        /// <param name="caiajetipoinfo">E:Energia / T:Transmision</param>
        /// <param name="caiajcodi">Identificador de la tabla CAI_AJUSTE</param>
        /// <param name="emprcodi">Identificador de la tabla SI_EMPRESA</param>
        public List<CaiAjusteempresaDTO> ObtenerListaPeriodoProyectado(string caiajetipoinfo, int caiajcodi, int emprcodi)
        {
            return FactoryTransferencia.GetCaiAjusteempresaRepository().ObtenerListaPeriodoProyectado(caiajetipoinfo, caiajcodi, emprcodi);
        }

        /// <summary>
        /// Permite listar todas las Empresas con su tipo de la tabla CAI_AJUSTEEMPRESA por Ajuste
        /// </summary>
        /// <param name="caiajcodi">Identificador de la tabla CAI_AJUSTE</param>
        public List<CaiAjusteempresaDTO> ListCaiAjusteempresasTipoEmpr(int caiajcodi)
        {
            return FactoryTransferencia.GetCaiAjusteempresaRepository().ListCaiAjusteempresasTipoEmpresa(caiajcodi);
        }

        /// <summary>
        /// Permite listar todas las Empresas y sus puntos de Generación activos en el intervalo de fechas
        /// </summary>
        /// <param name="sFechaInicio">Fecha de Inicio de Ejecución. Formato: yyyy-MM-dd</param>
        /// <param name="sFechaFin">Fecha de Final de Ejecución. Formato: yyyy-MM-dd</param>
        public List<CaiAjusteempresaDTO> ListCaiAjusteempresasXPtoGeneracion(string sFechaInicio, string sFechaFin)
        {
            //Parametros de consulta
            string tiposEmpresa = "3"; //Generadores en SI_EMPRESA
            string tiposGeneracion = "4,1,3,2";
            List<int> idsEmpresas = this.servicioConsultaMedidores.ObteneEmpresasPorTipo(tiposEmpresa).Select(x => x.Emprcodi).ToList();
            string empresas = string.Join<int>(ConstantesAppServicio.CaracterComa.ToString(), idsEmpresas);
            int lectcodi = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["IdLecturaMedidorGeneracion"]);
            int iIdFamiliaSSAA = ConstantesMedicion.IdFamiliaSSAA;
            int iIdTipogrupoNoIntegrante = ConstantesMedicion.IdTipogrupoNoIntegrante;
            int iIdTipoInfoPotenciaActiva = ConstantesMedicion.IdTipoInfoPotenciaActiva;
            int iTptoMedicodiTodos = ConstantesMedidores.TptoMedicodiTodos;
            
            return FactoryTransferencia.GetCaiAjusteempresaRepository().ListEmpresasXPtoGeneracion(sFechaInicio, sFechaFin, tiposGeneracion, empresas,
                iIdFamiliaSSAA, iIdTipogrupoNoIntegrante, lectcodi, iIdTipoInfoPotenciaActiva, iTptoMedicodiTodos);
        }

        /// <summary>
        /// Permite listar todas las Empresas y sus puntos de medición activos de los usuarios libres en el intervalo de fechas
        /// </summary>
        /// <param name="sFechaInicio">Fecha de Inicio de Ejecución. Formato: yyyy-MM-dd</param>
        /// <param name="sFechaFin">Fecha de Final de Ejecución. Formato: yyyy-MM-dd</param>
        public List<CaiAjusteempresaDTO> ListCaiAjusteempresasXPtoUL(string sFechaInicio, string sFechaFin)
        {
            //Parametros de consulta
            int iFormatcodi = Int32.Parse(ConfigurationManager.AppSettings["IdFormatoPR16"]);
            string lectCodiPR16 = ConfigurationManager.AppSettings["IdLecturaPR16"];
            string lectCodiAlpha = ConfigurationManager.AppSettings["IdLecturaAlphaPR16"];
            int iTipoEmprcodi = 4; //Usuarios libres en SI_EMPRESA

            return FactoryTransferencia.GetCaiAjusteempresaRepository().ListEmpresasXPtoUL(sFechaInicio, sFechaFin, iFormatcodi, iTipoEmprcodi, lectCodiPR16, lectCodiAlpha);
        }

        /// <summary>
        /// Permite listar todas las Empresas y sus puntos de distribución registrados con FORMATCODI
        /// </summary>
        public List<CaiAjusteempresaDTO> ListCaiAjusteempresasXPtoDist()
        {
            return FactoryTransferencia.GetCaiAjusteempresaRepository().ListEmpresasXPtoDist();
        }

        /// <summary>
        /// Permite listar todas las Empresas y sus puntos de transmision registrados con FORMATCODI
        /// </summary>
        public List<CaiAjusteempresaDTO> ListCaiAjusteempresasXPtoTrans()
        {
            return FactoryTransferencia.GetCaiAjusteempresaRepository().ListEmpresasXPtoTrans();
        }

        /// <summary>
        /// Permite obtener la lista completa de los puntos de medición
        /// </summary>
        /// <returns></returns>
        public List<MePtomedicionDTO> ListCaiAjusteempresasPtomed(int origlectcodi)
        {
            return FactoryTransferencia.GetCaiAjusteempresaRepository().ListPtomed(origlectcodi);
        }
        #endregion

        #region Métodos Tabla CAI_EQUISDDPBARR

        /// <summary>
        /// Inserta un registro de la tabla CAI_EQUISDDPBARR
        /// </summary>
        public void SaveCaiEquisddpbarr(CaiEquisddpbarrDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCaiEquisddpbarrRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CAI_EQUISDDPBARR
        /// </summary>
        public void UpdateCaiEquisddpbarr(CaiEquisddpbarrDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCaiEquisddpbarrRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CAI_EQUISDDPBARR
        /// </summary>
        /// <param name="casddbcodi">Identificador de la tabla CAI_EQUISDDPBARR</param>
        public void DeleteCaiEquisddpbarr(int casddbcodi)
        {
            try
            {
                FactoryTransferencia.GetCaiEquisddpbarrRepository().Delete(casddbcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CAI_EQUISDDPBARR
        /// </summary>
        /// <param name="casddbcodi">Identificador de la tabla CAI_EQUISDDPBARR</param>
        public CaiEquisddpbarrDTO GetByIdCaiEquisddpbarr(int casddbcodi)
        {
            return FactoryTransferencia.GetCaiEquisddpbarrRepository().GetById(casddbcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CAI_EQUISDDPBARR
        /// </summary>
        public List<CaiEquisddpbarrDTO> ListCaiEquisddpbarrs()
        {
            return FactoryTransferencia.GetCaiEquisddpbarrRepository().List();
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CAI_EQUISDDPBARR
        /// </summary>
        /// <param name="casddbcodi">Identificador de la tabla CAI_EQUISDDPBARR</param>
        public CaiEquisddpbarrDTO GetByIdCaiEquisddpbarr2(int casddbcodi)
        {
            return FactoryTransferencia.GetCaiEquisddpbarrRepository().GetByIdCaiEquisddpbarr(casddbcodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CaiEquisddpbarr
        /// </summary>
        public List<CaiEquisddpbarrDTO> GetByCriteriaCaiEquisddpbarrs()
        {
            return FactoryTransferencia.GetCaiEquisddpbarrRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CAI_EQUISDDPBARR
        /// </summary>
        public List<CaiEquisddpbarrDTO> ListCaiEquisddpbarrs2()
        {
            return FactoryTransferencia.GetCaiEquisddpbarrRepository().ListCaiEquisddpbarr();
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CAI_EQUISDDPBARR
        /// </summary>
        /// <param name="sddpgmnombre">Nombre de la UNidad/Barra del SDDP de la tabla CAI_SDDP_GENMARG</param>
        public CaiEquisddpbarrDTO GetByNombreBarraSddp(string sddpgmnombre)
        {
            return FactoryTransferencia.GetCaiEquisddpbarrRepository().GetByNombreBarraSddp(sddpgmnombre);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CaiEquisddpbarr
        /// </summary>
        public List<CaiEquisddpbarrDTO> GetByCriteriaCaiEquiunidbarrsNoIns()
        {
            return FactoryTransferencia.GetCaiEquisddpbarrRepository().GetByCriteriaCaiEquiunidbarrsNoIns();
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CAI_EQUIUNIDBARR
        /// </summary>
        /// <param name="barrcodi">Identificador de la tabla TRN_BARRA</param>
        public CaiEquiunidbarrDTO GetByIdBarrcodi(int barrcodi)
        {
            return FactoryTransferencia.GetCaiEquiunidbarrRepository().GetByIdBarrcodi(barrcodi);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CAI_EQUISDDPBARR
        /// </summary>
        /// <param name="barrcodi">Identificador de la tabla TRN_BARRA</param>
        /// <param name="sddpgmnombre">Nombre de la UNidad/Barra del SDDP de la tabla CAI_SDDP_GENMARG</param>
        public CaiEquisddpbarrDTO GetByBarraNombreSddp(int barrcodi, string sddpgmnombre)
        {
            return FactoryTransferencia.GetCaiEquisddpbarrRepository().GetByBarraNombreSddp(barrcodi, sddpgmnombre);
        }
        #endregion

        #region Métodos Tabla CAI_EQUISDDPUNI

        /// <summary>
        /// Inserta un registro de la tabla CAI_EQUISDDPUNI
        /// </summary>
        public void SaveCaiEquisddpuni(CaiEquisddpuniDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCaiEquisddpuniRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CAI_EQUISDDPUNI
        /// </summary>
        public void UpdateCaiEquisddpuni(CaiEquisddpuniDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCaiEquisddpuniRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CAI_EQUISDDPUNI
        /// </summary>
        ///<param name="casdducodi">Identificador de la Tabla CAI_EQUISDDPUNI</param>
        public void DeleteCaiEquisddpuni(int casdducodi)
        {
            try
            {
                FactoryTransferencia.GetCaiEquisddpuniRepository().Delete(casdducodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CAI_EQUISDDPUNI
        /// </summary>
        ///<param name="casdducodi">Identificador de la Tabla CAI_EQUISDDPUNI</param>
        public CaiEquisddpuniDTO GetByIdCaiEquisddpuni(int casdducodi)
        {
            return FactoryTransferencia.GetCaiEquisddpuniRepository().GetById(casdducodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CAI_EQUISDDPUNI
        /// </summary>
        public List<CaiEquisddpuniDTO> ListCaiEquisddpuni()
        {
            return FactoryTransferencia.GetCaiEquisddpuniRepository().List();
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CAI_EQUISDDPUNI
        /// </summary>
        ///<param name="casdducodi">Identificador de la Tabla CAI_EQUISDDPUNI</param>
        public CaiEquisddpuniDTO GetByIdCaiEquisddpuni2(int casdducodi)
        {
            return FactoryTransferencia.GetCaiEquisddpuniRepository().GetByIdCaiEquisddpuni(casdducodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CAI_EQUISDDPUNI
        /// </summary>
        public List<CaiEquisddpuniDTO> GetByCriteriaCaiEquisddpuni()
        {
            return FactoryTransferencia.GetCaiEquisddpuniRepository().GetByCriteria();
        }


        /// <summary>
        /// Permite listar todas las centrales + unidades de la tabla eq_equipo
        /// </summary>
        /// <returns>Lista CentralGeneracionDTO</returns>
        public List<CentralGeneracionDTO> ListUnidad()
        {
            return FactoryTransferencia.GetCaiEquisddpuniRepository().Unidad();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CAI_EQUISDDPUNI
        /// </summary>
        public List<CaiEquisddpuniDTO> ListCaiEquisddpuni2()
        {
            return FactoryTransferencia.GetCaiEquisddpuniRepository().ListCaiEquisddpuni();
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CAI_EQUISDDPUNI
        /// </summary>
        ///<param name="sddpgmnombre">Nombre de la unidad de Generación en el SDDP</param>
        public CaiEquisddpuniDTO GetByNombreEquipoSddp(string sddpgmnombre)
        {
            return FactoryTransferencia.GetCaiEquisddpuniRepository().GetByNombreEquipoSddp(sddpgmnombre);
        }

        #endregion

        #region Métodos Tabla CAI_EQUIUNIDBARR

        /// <summary>
        /// Inserta un registro de la tabla CAI_EQUIUNIDBARR
        /// </summary>
        public void SaveCaiEquiunidbarr(CaiEquiunidbarrDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCaiEquiunidbarrRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CAI_EQUIUNIDBARR
        /// </summary>
        public void UpdateCaiEquiunidbarr(CaiEquiunidbarrDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCaiEquiunidbarrRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CAI_EQUIUNIDBARR
        /// </summary>
        /// <param name="caiunbcodi">Identificador de la tabla CAI_EQUIUNIDBARR</param>
        public void DeleteCaiEquiunidbarr(int caiunbcodi)
        {
            try
            {
                FactoryTransferencia.GetCaiEquiunidbarrRepository().Delete(caiunbcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CAI_EQUIUNIDBARR
        /// </summary>
        /// <param name="caiunbcodi">Identificador de la tabla CAI_EQUIUNIDBARR</param>
        public CaiEquiunidbarrDTO GetByIdCaiEquiunidbarr(int caiunbcodi)
        {
            return FactoryTransferencia.GetCaiEquiunidbarrRepository().GetById(caiunbcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CAI_EQUIUNIDBARR
        /// </summary>
        public List<CaiEquiunidbarrDTO> ListCaiEquiunidbarrs()
        {
            return FactoryTransferencia.GetCaiEquiunidbarrRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CaiEquiunidbarr
        /// </summary>
        public List<CaiEquiunidbarrDTO> GetByCriteriaCaiEquiunidbarrs()
        {
            return FactoryTransferencia.GetCaiEquiunidbarrRepository().GetByCriteria();
        }

        public CaiEquiunidbarrDTO GetByIdEquicodiUNI(int equicodiuni)
        {
            return FactoryTransferencia.GetCaiEquiunidbarrRepository().GetByEquicodiUNI(equicodiuni);
        }

        #endregion

        #region Métodos Tabla CAI_GENERDEMAN

        /// <summary>
        /// Inserta un registro de la tabla CAI_GENERDEMAN
        /// </summary>
        public void SaveCaiGenerdeman(CaiGenerdemanDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCaiGenerdemanRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Inserta multiples registros en la tabla CAI_GENERDEMAN a través de un Insert As Select
        /// </summary>
        /// <param name="cagdcmcodi">Identificador de la tabla CAI_GENERDEMAN</param>
        /// <param name="caiajcodi">Identificador de la Tabla CAI_AJUSTE</param>
        /// <param name="cagdcmfuentedat">Fuentes de Datos: EG: Ejecutados Generación / PG: Proyectados Generación / DE: Demanda ejecutada de los Usuarios Libres / DD: Demanda ejecutada / proyectada de los Distribuidores y Usuarios Libres / CM: Costos Marginales de Corto Plazo proyectados</param>
        /// <param name="cagdcmcalidadinfo">Calidad de la información: C:COES, A:Agente, M:Mejor información</param>
        /// <param name="T">Tipo de dato: E:Ejecutado, P:Proyectado</param>
        /// <param name="user">Usuario que procesa el registro masivo</param>
        /// <param name="Formatcodi">Identificador de la tabla ME_FORMATO</param>
        /// <param name="FechaInicio">Fecha de inicio de los registros del select</param>
        /// <param name="FechaFin">Fecha final de los registros del select</param>
        /// <param name="TipoEmprcodi">Identificador de la tabla tipo de empresa</param>
        /// <param name="lectCodiPR16"></param>
        /// <param name="lectCodiAlpha"></param>
        public void SaveCaiGenerdemanAsSelectUsuariosLibres(Int32 cagdcmcodi, int caiajcodi, string cagdcmfuentedat, string cagdcmcalidadinfo, string T, string user, int Formatcodi, string FechaInicio, string FechaFin, int TipoEmprcodi, string lectCodiPR16, string lectCodiAlpha)
        {
            try
            {
                FactoryTransferencia.GetCaiGenerdemanRepository().SaveAsSelectUsuariosLibres(cagdcmcodi, caiajcodi, cagdcmfuentedat, cagdcmcalidadinfo, T, user, Formatcodi, FechaInicio, FechaFin, TipoEmprcodi, lectCodiPR16, lectCodiAlpha);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Inserta multiples registros en la tabla CAI_GENERDEMAN a través de un Insert As Select de Me_Medicion96
        /// </summary>
        /// <param name="cagdcmcodi">Identificador de la tabla CAI_GENERDEMAN</param>
        /// <param name="caiajcodi">Identificador de la Tabla CAI_AJUSTE</param>
        /// <param name="cagdcmfuentedat">Fuentes de Datos: EG: Ejecutados Generación / PG: Proyectados Generación / DE: Demanda ejecutada de los Usuarios Libres / DD: Demanda ejecutada / proyectada de los Distribuidores y Usuarios Libres / CM: Costos Marginales de Corto Plazo proyectados</param>
        /// <param name="cagdcmcalidadinfo">Calidad de la información: C:COES, A:Agente, M:Mejor información</param>
        /// <param name="tipodato">Tipo de dato: E:Ejecutado, P:Proyectado</param>
        /// <param name="user">Usuario que procesa el registro masivo</param>
        /// <param name="Formatcodi">Identificador de la tabla ME_FORMATO</param>
        /// <param name="Lectcodi">Identificador de la tabla me_lectura</param>
        /// <param name="FechaInicio">Fecha de inicio de los registros del select</param>
        /// <param name="FechaFin">Fecha final de los registros del select</param>
        public void SaveCaiGenerdemanAsSelectMeMedicion96(int cagdcmcodi, int caiajcodi, string cagdcmfuentedat, string cagdcmcalidadinfo, string tipodato, string user, int Formatcodi, int Lectcodi, string FechaInicio, string FechaFin)
        {
            try
            {
                FactoryTransferencia.GetCaiGenerdemanRepository().SaveCaiGenerdemanAsSelectMeMedicion96(cagdcmcodi, caiajcodi, cagdcmfuentedat, cagdcmcalidadinfo, tipodato, user, Formatcodi, Lectcodi, FechaInicio, FechaFin);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CAI_GENERDEMAN
        /// </summary>
        public void UpdateCaiGenerdeman(CaiGenerdemanDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCaiGenerdemanRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CAI_GENERDEMAN
        /// </summary>
        /// <param name="caiajcodi">Identificador de la Tabla CAI_AJUSTE</param>
        /// <param name="cagdcmfuentedat">Fuentes de Datos: EG: Ejecutados Generación / PG: Proyectados Generación / DE: Demanda ejecutada de los Usuarios Libres / DD: Demanda ejecutada / proyectada de los Distribuidores y Usuarios Libres / CM: Costos Marginales de Corto Plazo proyectados</param>
        public void DeleteCaiGenerdeman(int caiajcodi, string cagdcmfuentedat)
        {
            try
            {
                FactoryTransferencia.GetCaiGenerdemanRepository().Delete(caiajcodi, cagdcmfuentedat);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CAI_GENERDEMAN
        /// </summary>
        /// <param name="cagdcmcodi">Identificador de la tabla CAI_GENERDEMAN</param>
        public CaiGenerdemanDTO GetByIdCaiGenerdeman(int cagdcmcodi)
        {
            return FactoryTransferencia.GetCaiGenerdemanRepository().GetById(cagdcmcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CAI_GENERDEMAN
        /// </summary>
        public List<CaiGenerdemanDTO> ListCaiGenerdemans()
        {
            return FactoryTransferencia.GetCaiGenerdemanRepository().List();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CAI_GENERDEMAN entrelazado con la tabla TRN_COSTO_MARGINAL
        /// </summary>
        /// <param name="caiajcodi">Identificador de la Tabla CAI_AJUSTE</param>
        /// <param name="emprcodi">Identificador de la Tabla SI_EMPRESA</param>
        /// <param name="ptomedicodi">Identificador de la Tabla ME_PTOMEDICION</param>
        /// <param name="caajcmmes">Año y Mes en que se realizara la consuta, formato: YYYYMM</param>
        /// <param name="pericodi">Identificador de la Tabla TRN_PERIODO</param>
        /// <param name="recacodi">Identificador de la Tabla TRN_RECALCULO</param>
        public List<CaiGenerdemanDTO> ListCaiGenerdemanBarrasMes(int caiajcodi, int emprcodi, int ptomedicodi, int caajcmmes, int pericodi, int recacodi)
        {
            return FactoryTransferencia.GetCaiGenerdemanRepository().ListGenDemBarrMes(caiajcodi, emprcodi, ptomedicodi, caajcmmes, pericodi, recacodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CAI_GENERDEMAN para Proyectado de Generación y Costo Marginal
        /// </summary>
        /// <param name="caiajcodi">Identificador de la Tabla CAI_AJUSTE</param>
        /// <param name="emprcodi">Identificador de la Tabla SI_EMPRESA</param>
        /// <param name="ptomedicodi">Identificador de la Tabla ME_PTOMEDICION</param>
        /// <param name="caajcmmes">Año y Mes en que se realizara la consuta, formato: YYYYMM</param>
        public List<CaiGenerdemanDTO> ListCaiGenerdemanProyectadoMes(int caiajcodi, int emprcodi, int ptomedicodi, int caajcmmes)
        {
            return FactoryTransferencia.GetCaiGenerdemanRepository().ListGenDemProyMes(caiajcodi, emprcodi, ptomedicodi, caajcmmes);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CAI_GENERDEMAN
        /// </summary>
        public List<CaiGenerdemanDTO> GetByCriteriaCaiGenerdemans()
        {
            return FactoryTransferencia.GetCaiGenerdemanRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CAI_GENERDEMAN
        /// </summary>
        /// <param name="sFechaInicio">Fecha inicio</param>
        /// <param name="sFechaFin">Fecha fin</param>
        public List<CaiGenerdemanDTO> GetByCriteriaCaiGenerdemansUsuariosLibresSGOCOES(string sFechaInicio, string sFechaFin, int iFormatcodi, int iTipoEmprcodi, string lectCodiPR16, string lectCodiAlpha)
        {
            return FactoryTransferencia.GetCaiGenerdemanRepository().GetByUsuarioLibresSGOCOES(sFechaInicio, sFechaFin, iFormatcodi, iTipoEmprcodi, lectCodiPR16, lectCodiAlpha);
        }

        /// <summary>
        /// Permite obtener el siguiente identificador de tabla CaiGenerdeman
        /// </summary>
        public Int32 GetCodigoGeneradoCaiGenerdeman()
        {
            return FactoryTransferencia.GetCaiGenerdemanRepository().GetCodigoGenerado();
        }

        /// <summary>
        /// Inserta de forma masiva una lista de CaiGenerdemanDTO
        /// </summary>
        /// <param name="entitys">CaiGenerdemanDTO</param>    
        public void BulkInsertCaiGenerdeman(List<CaiGenerdemanDTO> entitys)
        {
            try
            {
                FactoryTransferencia.GetCaiGenerdemanRepository().BulkInsert(entitys);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite realizar copiar la info en la tabla CaiGenerdeman y CaiGenerdemandia
        /// </summary>
        /// <param name="caiajcodi">Identificador de la tabla CAI_AJUSTE</param>    
        /// <param name="empresas">nombre de empresa</param>    
        /// <param name="central">codigo de central</param>    
        /// <param name="tiposGeneracion">Tipo de Generación</param>    
        /// <param name="fecInicio">Fecha Inicio</param>    
        /// <param name="fecFin">Fecha Fin</param>    
        /// <param name="sUser">Nombre de Usuario</param>    
        public int CopiarSGOCOESCaiGenerdemans(int caiajcodi, string empresas, int central, string tiposGeneracion, DateTime fecInicio, DateTime fecFin, string sUser)
        {
            int iNumReg = 0;
            try
            {
                string sCagdcmFuenteDatos = "EG"; //Ejecutados Generación
                int lectcodi = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["IdLecturaMedidorGeneracion"]);
                string fuentes = this.servicioConsultaMedidores.GetFuenteSSAA(tiposGeneracion);
                List<MeMedicion96DTO> listActiva = new List<MeMedicion96DTO>();

                listActiva = FactorySic.GetMeMedicion96Repository().ObtenerExportacionConsultaMedidores(fecInicio, fecFin, central, tiposGeneracion, empresas,
                    ConstantesMedicion.IdFamiliaSSAA, ConstantesMedicion.GrupoNoIntegrante, lectcodi, ConstantesMedicion.IdTipoInfoPotenciaActiva,
                    ConstantesMedicion.TipoGenerrer, ConstantesMedidores.TptoMedicodiTodos);
                //INSERTANDO CON BULKINSERT
                List<CaiGenerdemanDTO> listGDBullk = new List<CaiGenerdemanDTO>();
                int iContadorGD = 0;
                Int32 iCagdcmcodi = this.GetCodigoGeneradoCaiGenerdeman();
                foreach (MeMedicion96DTO dtoME96 in listActiva)
                {
                    CaiGenerdemanDTO dtoGD = new CaiGenerdemanDTO();
                    dtoGD.Cagdcmcodi = iCagdcmcodi;
                    dtoGD.Caiajcodi = caiajcodi;
                    dtoGD.Cagdcmfuentedat = sCagdcmFuenteDatos;
                    dtoGD.Ptomedicodi = dtoME96.Ptomedicodi;
                    dtoGD.Emprcodi = dtoME96.Emprcodi;
                    dtoGD.Equicodicen = dtoME96.Equipadre;
                    dtoGD.Equicodiuni = dtoME96.Equicodi;
                    dtoGD.Cagdcmcalidadinfo = "C"; //migrado del sgocoes
                    dtoGD.Cagdcmdia = dtoME96.Medifecha;
                    dtoGD.Cagdcmtotaldia = (decimal)dtoME96.Meditotal;
                    dtoGD.Cagdcmusucreacion = sUser;
                    dtoGD.Cagdcmfeccreacion = DateTime.Now;
                    dtoGD.H1 = (decimal)dtoME96.H1;
                    dtoGD.H2 = (decimal)dtoME96.H2;
                    dtoGD.H3 = (decimal)dtoME96.H3;
                    dtoGD.H4 = (decimal)dtoME96.H4;
                    dtoGD.H5 = (decimal)dtoME96.H5;
                    dtoGD.H6 = (decimal)dtoME96.H6;
                    dtoGD.H7 = (decimal)dtoME96.H7;
                    dtoGD.H8 = (decimal)dtoME96.H8;
                    dtoGD.H9 = (decimal)dtoME96.H9;
                    dtoGD.H10 = (decimal)dtoME96.H10;
                    dtoGD.H11 = (decimal)dtoME96.H11;
                    dtoGD.H12 = (decimal)dtoME96.H12;
                    dtoGD.H13 = (decimal)dtoME96.H13;
                    dtoGD.H14 = (decimal)dtoME96.H14;
                    dtoGD.H15 = (decimal)dtoME96.H15;
                    dtoGD.H16 = (decimal)dtoME96.H16;
                    dtoGD.H17 = (decimal)dtoME96.H17;
                    dtoGD.H18 = (decimal)dtoME96.H18;
                    dtoGD.H19 = (decimal)dtoME96.H19;
                    dtoGD.H20 = (decimal)dtoME96.H20;
                    dtoGD.H21 = (decimal)dtoME96.H21;
                    dtoGD.H22 = (decimal)dtoME96.H22;
                    dtoGD.H23 = (decimal)dtoME96.H23;
                    dtoGD.H24 = (decimal)dtoME96.H24;
                    dtoGD.H25 = (decimal)dtoME96.H25;
                    dtoGD.H26 = (decimal)dtoME96.H26;
                    dtoGD.H27 = (decimal)dtoME96.H27;
                    dtoGD.H28 = (decimal)dtoME96.H28;
                    dtoGD.H29 = (decimal)dtoME96.H29;
                    dtoGD.H30 = (decimal)dtoME96.H30;
                    dtoGD.H31 = (decimal)dtoME96.H31;
                    dtoGD.H32 = (decimal)dtoME96.H32;
                    dtoGD.H33 = (decimal)dtoME96.H33;
                    dtoGD.H34 = (decimal)dtoME96.H34;
                    dtoGD.H35 = (decimal)dtoME96.H35;
                    dtoGD.H36 = (decimal)dtoME96.H36;
                    dtoGD.H37 = (decimal)dtoME96.H37;
                    dtoGD.H38 = (decimal)dtoME96.H38;
                    dtoGD.H39 = (decimal)dtoME96.H39;
                    dtoGD.H40 = (decimal)dtoME96.H40;
                    dtoGD.H41 = (decimal)dtoME96.H41;
                    dtoGD.H42 = (decimal)dtoME96.H42;
                    dtoGD.H43 = (decimal)dtoME96.H43;
                    dtoGD.H44 = (decimal)dtoME96.H44;
                    dtoGD.H45 = (decimal)dtoME96.H45;
                    dtoGD.H46 = (decimal)dtoME96.H46;
                    dtoGD.H47 = (decimal)dtoME96.H47;
                    dtoGD.H48 = (decimal)dtoME96.H48;
                    dtoGD.H49 = (decimal)dtoME96.H49;
                    dtoGD.H50 = (decimal)dtoME96.H50;
                    dtoGD.H51 = (decimal)dtoME96.H51;
                    dtoGD.H52 = (decimal)dtoME96.H52;
                    dtoGD.H53 = (decimal)dtoME96.H53;
                    dtoGD.H54 = (decimal)dtoME96.H54;
                    dtoGD.H55 = (decimal)dtoME96.H55;
                    dtoGD.H56 = (decimal)dtoME96.H56;
                    dtoGD.H57 = (decimal)dtoME96.H57;
                    dtoGD.H58 = (decimal)dtoME96.H58;
                    dtoGD.H59 = (decimal)dtoME96.H59;
                    dtoGD.H60 = (decimal)dtoME96.H60;
                    dtoGD.H61 = (decimal)dtoME96.H61;
                    dtoGD.H62 = (decimal)dtoME96.H62;
                    dtoGD.H63 = (decimal)dtoME96.H63;
                    dtoGD.H64 = (decimal)dtoME96.H64;
                    dtoGD.H65 = (decimal)dtoME96.H65;
                    dtoGD.H66 = (decimal)dtoME96.H66;
                    dtoGD.H67 = (decimal)dtoME96.H67;
                    dtoGD.H68 = (decimal)dtoME96.H68;
                    dtoGD.H69 = (decimal)dtoME96.H69;
                    dtoGD.H70 = (decimal)dtoME96.H70;
                    dtoGD.H71 = (decimal)dtoME96.H71;
                    dtoGD.H72 = (decimal)dtoME96.H72;
                    dtoGD.H73 = (decimal)dtoME96.H73;
                    dtoGD.H74 = (decimal)dtoME96.H74;
                    dtoGD.H75 = (decimal)dtoME96.H75;
                    dtoGD.H76 = (decimal)dtoME96.H76;
                    dtoGD.H77 = (decimal)dtoME96.H77;
                    dtoGD.H78 = (decimal)dtoME96.H78;
                    dtoGD.H79 = (decimal)dtoME96.H79;
                    dtoGD.H80 = (decimal)dtoME96.H80;
                    dtoGD.H81 = (decimal)dtoME96.H81;
                    dtoGD.H82 = (decimal)dtoME96.H82;
                    dtoGD.H83 = (decimal)dtoME96.H83;
                    dtoGD.H84 = (decimal)dtoME96.H84;
                    dtoGD.H85 = (decimal)dtoME96.H85;
                    dtoGD.H86 = (decimal)dtoME96.H86;
                    dtoGD.H87 = (decimal)dtoME96.H87;
                    dtoGD.H88 = (decimal)dtoME96.H88;
                    dtoGD.H89 = (decimal)dtoME96.H89;
                    dtoGD.H90 = (decimal)dtoME96.H90;
                    dtoGD.H91 = (decimal)dtoME96.H91;
                    dtoGD.H92 = (decimal)dtoME96.H92;
                    dtoGD.H93 = (decimal)dtoME96.H93;
                    dtoGD.H94 = (decimal)dtoME96.H94;
                    dtoGD.H95 = (decimal)dtoME96.H95;
                    dtoGD.H96 = (decimal)dtoME96.H96;
                    dtoGD.T1 = "E";
                    dtoGD.T2 = "E";
                    dtoGD.T3 = "E";
                    dtoGD.T4 = "E";
                    dtoGD.T5 = "E";
                    dtoGD.T6 = "E";
                    dtoGD.T7 = "E";
                    dtoGD.T8 = "E";
                    dtoGD.T9 = "E";
                    dtoGD.T10 = "E";
                    dtoGD.T11 = "E";
                    dtoGD.T12 = "E";
                    dtoGD.T13 = "E";
                    dtoGD.T14 = "E";
                    dtoGD.T15 = "E";
                    dtoGD.T16 = "E";
                    dtoGD.T17 = "E";
                    dtoGD.T18 = "E";
                    dtoGD.T19 = "E";
                    dtoGD.T20 = "E";
                    dtoGD.T21 = "E";
                    dtoGD.T22 = "E";
                    dtoGD.T23 = "E";
                    dtoGD.T24 = "E";
                    dtoGD.T25 = "E";
                    dtoGD.T26 = "E";
                    dtoGD.T27 = "E";
                    dtoGD.T28 = "E";
                    dtoGD.T29 = "E";
                    dtoGD.T30 = "E";
                    dtoGD.T31 = "E";
                    dtoGD.T32 = "E";
                    dtoGD.T33 = "E";
                    dtoGD.T34 = "E";
                    dtoGD.T35 = "E";
                    dtoGD.T36 = "E";
                    dtoGD.T37 = "E";
                    dtoGD.T38 = "E";
                    dtoGD.T39 = "E";
                    dtoGD.T40 = "E";
                    dtoGD.T41 = "E";
                    dtoGD.T42 = "E";
                    dtoGD.T43 = "E";
                    dtoGD.T44 = "E";
                    dtoGD.T45 = "E";
                    dtoGD.T46 = "E";
                    dtoGD.T47 = "E";
                    dtoGD.T48 = "E";
                    dtoGD.T49 = "E";
                    dtoGD.T50 = "E";
                    dtoGD.T51 = "E";
                    dtoGD.T52 = "E";
                    dtoGD.T53 = "E";
                    dtoGD.T54 = "E";
                    dtoGD.T55 = "E";
                    dtoGD.T56 = "E";
                    dtoGD.T57 = "E";
                    dtoGD.T58 = "E";
                    dtoGD.T59 = "E";
                    dtoGD.T60 = "E";
                    dtoGD.T61 = "E";
                    dtoGD.T62 = "E";
                    dtoGD.T63 = "E";
                    dtoGD.T64 = "E";
                    dtoGD.T65 = "E";
                    dtoGD.T66 = "E";
                    dtoGD.T67 = "E";
                    dtoGD.T68 = "E";
                    dtoGD.T69 = "E";
                    dtoGD.T70 = "E";
                    dtoGD.T71 = "E";
                    dtoGD.T72 = "E";
                    dtoGD.T73 = "E";
                    dtoGD.T74 = "E";
                    dtoGD.T75 = "E";
                    dtoGD.T76 = "E";
                    dtoGD.T77 = "E";
                    dtoGD.T78 = "E";
                    dtoGD.T79 = "E";
                    dtoGD.T80 = "E";
                    dtoGD.T81 = "E";
                    dtoGD.T82 = "E";
                    dtoGD.T83 = "E";
                    dtoGD.T84 = "E";
                    dtoGD.T85 = "E";
                    dtoGD.T86 = "E";
                    dtoGD.T87 = "E";
                    dtoGD.T88 = "E";
                    dtoGD.T89 = "E";
                    dtoGD.T90 = "E";
                    dtoGD.T91 = "E";
                    dtoGD.T92 = "E";
                    dtoGD.T93 = "E";
                    dtoGD.T94 = "E";
                    dtoGD.T95 = "E";
                    dtoGD.T96 = "E";
                    listGDBullk.Add(dtoGD);
                    iContadorGD++;
                    if (iContadorGD == 50000)
                    {
                        this.BulkInsertCaiGenerdeman(listGDBullk);
                        listGDBullk = new List<CaiGenerdemanDTO>();
                        iNumReg += iContadorGD;
                        iContadorGD = 0;
                    }
                    iCagdcmcodi++;
                }
                if (iContadorGD > 0)
                {
                    //Quedo un faltante que insertar
                    this.BulkInsertCaiGenerdeman(listGDBullk);
                    iNumReg += iContadorGD;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return iNumReg;
        }

        #endregion

        #region Métodos Tabla CAI_IMPGENERACION

        /// <summary>
        /// Inserta un registro de la tabla CAI_IMPGENERACION
        /// </summary>
        public void SaveCaiImpgeneracion(CaiImpgeneracionDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCaiImpgeneracionRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CAI_IMPGENERACION
        /// </summary>
        public void UpdateCaiImpgeneracion(CaiImpgeneracionDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCaiImpgeneracionRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CAI_IMPGENERACION
        /// </summary>
        /// <param name="Caiajcodi">Identificador de la Tabla CAI_AJUSTE</param>
        public void DeleteCaiImpgeneracion(int caiajcodi)
        {
            try
            {
                FactoryTransferencia.GetCaiImpgeneracionRepository().Delete(caiajcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CAI_IMPGENERACION
        /// </summary>
        /// <param name="caimpgcodi">Identificador de la tabla CAI_IMPGENERACION</param>
        public CaiImpgeneracionDTO GetByIdCaiImpgeneracion(int caimpgcodi)
        {
            return FactoryTransferencia.GetCaiImpgeneracionRepository().GetById(caimpgcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CAI_IMPGENERACION
        /// </summary>
        public List<CaiImpgeneracionDTO> ListCaiImpgeneracions()
        {
            return FactoryTransferencia.GetCaiImpgeneracionRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CaiImpgeneracion
        /// </summary>
        public List<CaiImpgeneracionDTO> GetByCriteriaCaiImpgeneracions()
        {
            return FactoryTransferencia.GetCaiImpgeneracionRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla CAI_INGTRANSMISION

        /// <summary>
        /// Inserta un registro de la tabla CAI_INGTRANSMISION
        /// </summary>
        public void SaveCaiIngtransmision(CaiIngtransmisionDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCaiIngtransmisionRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Inserta multiples registros en la tabla CAI_INGTRANMISION a través de un Insert As Select de Me_Medicion1
        /// </summary>
        /// <param name="caitrcodi">Identificador de la tabla CAI_INGTRANMISION</param>
        /// <param name="caiajcodi">Identificador de la Tabla CAI_AJUSTE</param>
        /// <param name="tipodato">Tipo de dato: E:Ejecutado, P:Proyectado</param>
        /// <param name="user">Usuario que procesa el registro masivo</param>
        /// <param name="Formatcodi">Identificador de la tabla ME_FORMATO</param>
        /// <param name="Lectcodi">Identificador de la tabla me_lectura</param>
        /// <param name="FechaInicio">Fecha de inicio de los registros del select</param>
        /// <param name="FechaFin">Fecha final de los registros del select</param>
        public void SaveCaiIngtransmisionAsSelectMeMedicion1(int caitrcodi, int caiajcodi, string caitrcalidadinfo, string tipodato, string user, int Formatcodi, int Lectcodi, string FechaInicio, string FechaFin)
        {
            try
            {
                FactoryTransferencia.GetCaiIngtransmisionRepository().SaveCaiIngtransmisionAsSelectMeMedicion1(caitrcodi, caiajcodi, caitrcalidadinfo, tipodato, user, Formatcodi, Lectcodi, FechaInicio, FechaFin);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        

        /// <summary>
        /// Actualiza un registro de la tabla CAI_INGTRANSMISION
        /// </summary>
        public void UpdateCaiIngtransmision(CaiIngtransmisionDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCaiIngtransmisionRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CAI_AJUSTE
        /// </summary>
        /// <param name="caiajcodi">Identificador de la tabla CAI_AJUSTE</param>
        public void DeleteCaiIngtransmision(int caiajcodi)
        {
            try
            {
                FactoryTransferencia.GetCaiIngtransmisionRepository().Delete(caiajcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CAI_INGTRANSMISION
        /// </summary>
        /// <param name="caitrcodi">Identificador de la tabla CAI_INGTRANSMISION</param>
        public CaiIngtransmisionDTO GetByIdCaiIngtransmision(int caitrcodi)
        {
            return FactoryTransferencia.GetCaiIngtransmisionRepository().GetById(caitrcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CAI_INGTRANSMISION
        /// </summary>
        public List<CaiIngtransmisionDTO> ListCaiIngtransmisions()
        {
            return FactoryTransferencia.GetCaiIngtransmisionRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CaiIngtransmision
        /// </summary>
        public List<CaiIngtransmisionDTO> GetByCriteriaCaiIngtransmisions()
        {
            return FactoryTransferencia.GetCaiIngtransmisionRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite obtener el siguiente identificador de tabla CaiIngtransmision
        /// </summary>
        public Int32 GetCodigoGeneradoCaiIngtransmision()
        {
            return FactoryTransferencia.GetCaiIngtransmisionRepository().GetCodigoGenerado();
        }

        #endregion

        #region Métodos Tabla CAI_MAXDEMANDA

        /// <summary>
        /// Inserta un registro de la tabla CAI_MAXDEMANDA
        /// </summary>
        public void SaveCaiMaxdemanda(CaiMaxdemandaDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCaiMaxdemandaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CAI_MAXDEMANDA
        /// </summary>
        public void UpdateCaiMaxdemanda(CaiMaxdemandaDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCaiMaxdemandaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CAI_MAXDEMANDA
        /// </summary>
        /// <param name="caimdecodi">Identificador de la tabla CAI_MAXDEMANDA</param>
        public void DeleteCaiMaxdemanda(int caimdecodi)
        {
            try
            {
                FactoryTransferencia.GetCaiMaxdemandaRepository().Delete(caimdecodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina todos los registro Ejecutados de la tabla CAI_MAXDEMANDA pertenenecientes a una versión de ajuste
        /// </summary>
        /// <param name="caiajcodi">Identificador de la tabla CAI_AJUSTE</param>
        public void DeleteCaiMaxdemandaEjecutada(int caiajcodi)
        {
            try
            {
                FactoryTransferencia.GetCaiMaxdemandaRepository().DeleteEjecutada(caiajcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CAI_MAXDEMANDA
        /// </summary>
        /// <param name="caimdecodi">Identificador de la tabla CAI_MAXDEMANDA</param>
        public CaiMaxdemandaDTO GetByIdCaiMaxdemanda(int caimdecodi)
        {
            return FactoryTransferencia.GetCaiMaxdemandaRepository().GetById(caimdecodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CAI_MAXDEMANDA
        /// </summary>
        /// <param name="caiajcodi">Identificador de la tabla CAI_AJUSTE</param>
        public List<CaiMaxdemandaDTO> ListCaiMaxdemandas(int caiajcodi)
        {
            return FactoryTransferencia.GetCaiMaxdemandaRepository().List(caiajcodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CaiMaxdemanda
        /// </summary>
        public List<CaiMaxdemandaDTO> GetByCriteriaCaiMaxdemandas()
        {
            return FactoryTransferencia.GetCaiMaxdemandaRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CAI_MAXDEMANDA a través de YYYYMMM
        /// </summary>
        /// <param name="Caiajcodi">Identificador de la Tabla CAI_AJUSTE</param>
        /// <param name="CaimdeAnioMes">Formato YYYYMM de Maxima Demanda</param>    
        public CaiMaxdemandaDTO GetCaiMaxdemandaByCaimdeAnioMes(int Caiajcodi, int CaimdeAnioMes)
        {
            return FactoryTransferencia.GetCaiMaxdemandaRepository().GetByCaimdeAnioMes(Caiajcodi, CaimdeAnioMes);
        }

        /// <summary>
        /// Permite realizar la copia de la data del SGOCOES a la tabla CaiMaxdemanda
        /// </summary>
        /// <param name="caiajcodi">Identificador de la Tabla CAI_AJUSTE</param>
        /// <param name="fecInicio">Fecha Inicio</param>
        /// <param name="fecFin">Fecha Fin</param>
        /// <param name="sUser">Nombre de Usuario</param>
        public void CopiarSGOCOESCaiMaximaDemanda(int caiajcodi, DateTime fecInicio, DateTime fecFin, string sUser)
        {
            int iEstadoValidacion = 0;
            string tiposEmpresa = "1,2,4,5,3";
            int central = 1;
            string tiposGeneracion = "4,1,3,2";
            string fuentesEnergia = "1,6,7,5,3,9,2,4,10,11,8";
            bool hayCruceHOP = false;
            List<int> idsEmpresas = this.servicioReporteMedidores.ObteneEmpresasPorTipo(tiposEmpresa).Select(x => x.Emprcodi).ToList();
            string empresas = string.Join<int>(ConstantesAppServicio.CaracterComa.ToString(), idsEmpresas);

            List<MeMedicion96DTO> list = this.servicioReporteMedidores.ListaDataMDGeneracionConsolidado(fecInicio, fecFin, central, tiposGeneracion, empresas, iEstadoValidacion, fuentesEnergia, hayCruceHOP);

            //Data Generación
            List<MeMedicion96DTO> listaDemandaGen = this.servicioReporteMedidores.ListaDataMDGeneracionFromConsolidado(fecInicio, fecFin, list);

            //Data Interconexion
            List<MeMedicion96DTO> listaInterconexion = this.servicioReporteMedidores.ListaDataMDInterconexion96(fecInicio, fecFin);

            //Data Total
            List<MeMedicion96DTO> listDemanda = this.servicioReporteMedidores.ListaDataMDTotalSEIN(listaDemandaGen, listaInterconexion);

            MedicionReporteDTO umbrales = this.servicioReporteMedidores.ObtenerParametros(fecInicio, listDemanda, listaInterconexion);

            if (umbrales != null)
            {
                CaiMaxdemandaDTO dtoMaxDemanda = new CaiMaxdemandaDTO();
                dtoMaxDemanda.Caiajcodi = caiajcodi;
                dtoMaxDemanda.Caimdeaniomes = umbrales.FechaMaximaDemanda.Year * 100 + umbrales.FechaMaximaDemanda.Month;
                dtoMaxDemanda.Caimdefechor = umbrales.MaximaDemandaHora;
                dtoMaxDemanda.Caimdetipoinfo = "E";
                dtoMaxDemanda.Caimdemaxdemanda = umbrales.MaximaDemanda;
                dtoMaxDemanda.Caimdeusucreacion = sUser;
                dtoMaxDemanda.Caimdefeccreacion = DateTime.Now;
                dtoMaxDemanda.Caimdeusumodificacion = sUser;
                dtoMaxDemanda.Caimdefecmodificacion = DateTime.Now;
                this.SaveCaiMaxdemanda(dtoMaxDemanda);
            }

        }
        #endregion

        #region Métodos Tabla CAI_PORCTAPORTE

        /// <summary>
        /// Inserta un registro de la tabla CAI_PORCTAPORTE
        /// </summary>
        public void SaveCaiPorctaporte(CaiPorctaporteDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCaiPorctaporteRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CAI_PORCTAPORTE
        /// </summary>
        public void UpdateCaiPorctaporte(CaiPorctaporteDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCaiPorctaporteRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CAI_PORCTAPORTE
        /// </summary>
        /// <param name="caiajcodi">Identificador de la tabla CAI_AJUSTE</param>
        public void DeleteCaiPorctaporte(int caiajcodi)
        {
            try
            {
                FactoryTransferencia.GetCaiPorctaporteRepository().Delete(caiajcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CAI_PORCTAPORTE
        /// </summary>
        /// <param name="caipacodi">Identificador de la tabla CAI_PORCTAPORTE</param>
        public CaiPorctaporteDTO GetByIdCaiPorctaporte(int caipacodi)
        {
            return FactoryTransferencia.GetCaiPorctaporteRepository().GetById(caipacodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CAI_PORCTAPORTE
        /// </summary>
        public List<CaiPorctaporteDTO> ListCaiPorctaportes()
        {
            return FactoryTransferencia.GetCaiPorctaporteRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CaiPorctaporte
        /// </summary>
        public List<CaiPorctaporteDTO> GetByCriteriaCaiPorctaportes()
        {
            return FactoryTransferencia.GetCaiPorctaporteRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite obtener la lista de empresas con su respectivo Importe (Energía + Potencia) acumulado en el año presupuestal
        /// </summary>
        /// <param name="caiajcodi">Identificador de la tabla CAI_AJUSTE</param>
        public List<CaiPorctaporteDTO> ListCaiPorctaportesByEmpresaImporte(int caiajcodi)
        {
            return FactoryTransferencia.GetCaiPorctaporteRepository().ByEmpresaImporte(caiajcodi);
        }

        

        #endregion

        #region Métodos Tabla CAI_PRESUPUESTO

        /// <summary>
        /// Inserta un registro de la tabla CAI_PRESUPUESTO
        /// </summary>
        public void SaveCaiPresupuesto(CaiPresupuestoDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCaiPresupuestoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CAI_PRESUPUESTO
        /// </summary>
        public void UpdateCaiPresupuesto(CaiPresupuestoDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCaiPresupuestoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CAI_PRESUPUESTO
        /// </summary>
        /// <param name="caiprscodi">Identificador de la tabla CAI_PRESUPUESTO</param>
        public void DeleteCaiPresupuesto(int caiprscodi)
        {
            try
            {
                FactoryTransferencia.GetCaiPresupuestoRepository().Delete(caiprscodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CAI_PRESUPUESTO
        /// </summary>
        /// <param name="caiprscodi">Identificador de la tabla CAI_PRESUPUESTO</param>
        public CaiPresupuestoDTO GetByIdCaiPresupuesto(int caiprscodi)
        {
            return FactoryTransferencia.GetCaiPresupuestoRepository().GetById(caiprscodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CAI_PRESUPUESTO
        /// </summary>
        public List<CaiPresupuestoDTO> ListCaiPresupuestos()
        {
            return FactoryTransferencia.GetCaiPresupuestoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CaiPresupuesto
        /// </summary>
        public List<CaiPresupuestoDTO> GetByCriteriaCaiPresupuestos()
        {
            return FactoryTransferencia.GetCaiPresupuestoRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla CAI_SDDP_DURACION

        /// <summary>
        /// Inserta un registro de la tabla CAI_SDDP_DURACION
        /// </summary>
        public void SaveCaiSddpDuracion(CaiSddpDuracionDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCaiSddpDuracionRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CAI_SDDP_DURACION
        /// </summary>
        public void UpdateCaiSddpDuracion(CaiSddpDuracionDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCaiSddpDuracionRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CAI_SDDP_DURACION
        /// </summary>
        public void DeleteCaiSddpDuracion()
        {
            try
            {
                FactoryTransferencia.GetCaiSddpDuracionRepository().Delete();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CAI_SDDP_DURACION
        /// </summary>
        /// <param name="sddpducodi">Identificador de la tabla CAI_SDDP_DURACION</param>
        public CaiSddpDuracionDTO GetByIdCaiSddpDuracion(int sddpducodi)
        {
            return FactoryTransferencia.GetCaiSddpDuracionRepository().GetById(sddpducodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CAI_SDDP_DURACION
        /// </summary>
        public List<CaiSddpDuracionDTO> ListCaiSddpDuracions()
        {
            return FactoryTransferencia.GetCaiSddpDuracionRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CaiSddpDuracion
        /// </summary>
        public List<CaiSddpDuracionDTO> GetByCriteriaCaiSddpDuracions()
        {
            return FactoryTransferencia.GetCaiSddpDuracionRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CAI_SDDP_DURACION
        /// </summary>
        /// <param name="sddpduetapa">Etapa</param>
        public List<CaiSddpDuracionDTO> ListCaiSddpDuracionPorEtapa(int sddpduetapa)
        {
            return FactoryTransferencia.GetCaiSddpDuracionRepository().ListByEtapa(sddpduetapa);
        }
        #endregion

        #region Métodos Tabla CAI_SDDP_GENMARG

        /// <summary>
        /// Inserta un registro de la tabla CAI_SDDP_GENMARG
        /// </summary>
        public void SaveCaiSddpGenmarg(CaiSddpGenmargDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCaiSddpGenmargRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CAI_SDDP_GENMARG
        /// </summary>
        public void UpdateCaiSddpGenmarg(CaiSddpGenmargDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCaiSddpGenmargRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CAI_SDDP_GENMARG
        /// </summary>
        /// <param name="tipo">Tipo TER:Termica, HID:Didraulica, GND: Renovable, CMG:Costo Marginal</param>
        public void DeleteCaiSddpGenmarg(string tipo)
        {
            try
            {
                FactoryTransferencia.GetCaiSddpGenmargRepository().Delete(tipo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CAI_SDDP_GENMARG
        /// </summary>
        /// <param name="sddpgmcodi">Identificador de la Tabla CAI_SDDP_GENMARG</param>
        public CaiSddpGenmargDTO GetByIdCaiSddpGenmarg(int sddpgmcodi)
        {
            return FactoryTransferencia.GetCaiSddpGenmargRepository().GetById(sddpgmcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CAI_SDDP_GENMARG
        /// </summary>
        public List<CaiSddpGenmargDTO> ListCaiSddpGenmargs()
        {
            return FactoryTransferencia.GetCaiSddpGenmargRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CaiSddpGenmarg
        /// </summary>
        /// <param name="sddpgmtipo">Tipo TER:Termica, HID:Didraulica, GND: Renovable, CMG:Costo Marginal</param>
        public List<CaiSddpGenmargDTO> GetByCriteriaCaiSddpGenmargs(string sddpgmtipo)
        {
            return FactoryTransferencia.GetCaiSddpGenmargRepository().GetByCriteria(sddpgmtipo);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CaiSddpGenmarg
        /// </summary>
        /// <param name="sddpgmtipo">Tipo TER:Termica, HID:Didraulica, GND: Renovable, CMG:Costo Marginal</param>
        public List<CaiSddpGenmargDTO> GetByCriteriaCaiSddpGenmargsBarrNoIns(string sddpgmtipo)
        {
            return FactoryTransferencia.GetCaiSddpGenmargRepository().GetByCriteriaCaiSddpGenmargsBarrNoIns(sddpgmtipo);
        }

        /// <summary>
        /// Inserta de forma masiva una lista de CaiSddpGenmargDTO
        /// </summary>
        /// <param name="entity">Identificador CAI_SDDP_GENMARG</param>    
        public void BulkInsertValorSddpGenmarg(List<CaiSddpGenmargDTO> entitys)
        {
            try
            {
                FactoryTransferencia.GetCaiSddpGenmargRepository().BulkInsert(entitys);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Identificador siguiente de Base de datos
        /// </summary>
        /// <returns>Identificador CAI_SDDP_GENMARG</returns> 
        public int GetCodigoGeneradoCaiSddpGen()
        {
            try
            {
                int id = FactoryTransferencia.GetCaiSddpGenmargRepository().GetCodigoGenerado();
                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener suma segun criterios para energia TER, HID, GND
        /// </summary>
        /// <param name="sddpgmtipo">Tipo TER:Termica, HID:Didraulica, GND: Renovable, CMG:Costo Marginal</param>    
        /// <param name="sddpgmetapa">Etapa</param>    
        /// <param name="sddpgmbloque">Bloque</param>    
        /// <param name="sddpgmnombre">Nombre de la UNidad/Barra del SDDP</param>     
        /// <returns>Identificador CAI_SDDP_GENMARG</returns> 
        public decimal GetSumSddpGenmargByEtapa(string sddpgmtipo, int sddpgmetapa, int sddpgmbloque, string sddpgmnombre)
        {
            try
            {
                decimal suma = FactoryTransferencia.GetCaiSddpGenmargRepository().GetSumSddpGenmargByEtapa(sddpgmtipo, sddpgmetapa, sddpgmbloque, sddpgmnombre);
                return suma;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener suma segun criterios
        /// </summary>
        /// <returns>Identificador CAI_SDDP_GENMARG</returns> 
        /// <param name="sddpgmetapa">Tipo TER:Termica, HID:Didraulica, GND: Renovable, CMG:Costo Marginal</param>    
        /// <param name="sddpgmnombre">Nombre de la UNidad/Barra del SDDP</param>    
        public decimal GetSumSddpGenmargByEtapaB1(int sddpgmetapa, string sddpgmnombre)
        {
            try
            {
                decimal suma = FactoryTransferencia.GetCaiSddpGenmargRepository().GetSumSddpGenmargByEtapaB1(sddpgmetapa, sddpgmnombre);
                return suma;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener suma segun criterios
        /// </summary>
        /// <param name="sddpgmetapa">Tipo TER:Termica, HID:Didraulica, GND: Renovable, CMG:Costo Marginal</param>    
        /// <param name="sddpgmnombre">Nombre de la UNidad/Barra del SDDP</param>
        /// <returns>Identificador CAI_SDDP_GENMARG</returns> 
        public decimal GetSumSddpGenmargByEtapaB2(int sddpgmetapa, string sddpgmnombre)
        {
            try
            {
                decimal suma = FactoryTransferencia.GetCaiSddpGenmargRepository().GetSumSddpGenmargByEtapaB2(sddpgmetapa, sddpgmnombre);
                return suma;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener suma segun criterios
        /// </summary>
        /// <param name="sddpgmetapa">Tipo TER:Termica, HID:Didraulica, GND: Renovable, CMG:Costo Marginal</param>    
        /// <param name="sddpgmnombre">Nombre de la UNidad/Barra del SDDP</param>
        /// <returns>Identificador CAI_SDDP_GENMARG</returns> 
        public decimal GetSumSddpGenmargByEtapaB3(int sddpgmetapa, string sddpgmnombre)
        {
            try
            {
                decimal suma = FactoryTransferencia.GetCaiSddpGenmargRepository().GetSumSddpGenmargByEtapaB3(sddpgmetapa, sddpgmnombre);
                return suma;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener suma segun criterios
        /// </summary>
        /// <param name="sddpgmetapa">Tipo TER:Termica, HID:Didraulica, GND: Renovable, CMG:Costo Marginal</param>    
        /// <param name="sddpgmnombre">Nombre de la UNidad/Barra del SDDP</param>
        /// <returns>Identificador CAI_SDDP_GENMARG</returns> 
        public decimal GetSumSddpGenmargByEtapaB4(int sddpgmetapa, string sddpgmnombre)
        {
            try
            {
                decimal suma = FactoryTransferencia.GetCaiSddpGenmargRepository().GetSumSddpGenmargByEtapaB4(sddpgmetapa, sddpgmnombre);
                return suma;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener suma segun criterios
        /// </summary>
        /// <param name="sddpgmetapa">Tipo TER:Termica, HID:Didraulica, GND: Renovable, CMG:Costo Marginal</param>    
        /// <param name="sddpgmnombre">Nombre de la UNidad/Barra del SDDP</param>
        /// <returns>Identificador CAI_SDDP_GENMARG</returns> 
        public decimal GetSumSddpGenmargByEtapaB5(int sddpgmetapa, string sddpgmnombre)
        {
            try
            {
                decimal suma = FactoryTransferencia.GetCaiSddpGenmargRepository().GetSumSddpGenmargByEtapaB5(sddpgmetapa, sddpgmnombre);
                return suma;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region Métodos Tabla CAI_SDDP_PARAMDIA

        /// <summary>
        /// Inserta un registro de la tabla CAI_SDDP_PARAMDIA
        /// </summary>
        public void SaveCaiSddpParamdia(CaiSddpParamdiaDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCaiSddpParamdiaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CAI_SDDP_PARAMDIA
        /// </summary>
        public void UpdateCaiSddpParamdia(CaiSddpParamdiaDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCaiSddpParamdiaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CAI_SDDP_PARAMDIA
        /// </summary>
        public void DeleteCaiSddpParamdia()
        {
            try
            {
                FactoryTransferencia.GetCaiSddpParamdiaRepository().Delete();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CAI_SDDP_PARAMDIA
        /// </summary>
        /// <param name="caiajcodi">Identificador de la Tabla CAI_AJUSTE</param>
        public CaiSddpParamdiaDTO GetByIdCaiSddpParamdia(int caiajcodi)
        {
            return FactoryTransferencia.GetCaiSddpParamdiaRepository().GetById(caiajcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CAI_SDDP_PARAMDIA
        /// </summary>
        /// <param name="Sddppicodi">Identificador de la Tabla CAI_SDDP_PARAMINT</param>
        public List<CaiSddpParamdiaDTO> ListCaiSddpParamdia(int Sddppicodi)
        {
            return FactoryTransferencia.GetCaiSddpParamdiaRepository().List(Sddppicodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CAI_SDDP_PARAMDIA
        /// </summary>
        /// <param name="Sddppicodi">Identificador de la Tabla CAI_AJUSTE</param>
        public List<CaiSddpParamdiaDTO> GetByCriteriaCaiSddpParamdia(int caiajcodi)
        {
            return FactoryTransferencia.GetCaiSddpParamdiaRepository().GetByCriteria(caiajcodi);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CAI_SDDP_PARAMDIA
        /// </summary>
        /// <param name="Sddppicodi">Día del año Presupuesto</param>
        public CaiSddpParamdiaDTO GetByDiaCaiSddpParamdia(DateTime sddppddia)
        {
            return FactoryTransferencia.GetCaiSddpParamdiaRepository().GetByDiaCaiSddpParamdia(sddppddia);
        }

        #endregion

        #region Métodos Tabla CAI_SDDP_PARAMETRO

        /// <summary>
        /// Inserta un registro de la tabla CAI_SDDP_PARAMETRO
        /// </summary>
        public void SaveCaiSddpParametro(CaiSddpParametroDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCaiSddpParametroRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CAI_SDDP_PARAMETRO
        /// </summary>
        public void UpdateCaiSddpParametro(CaiSddpParametroDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCaiSddpParametroRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CAI_SDDP_PARAMETRO
        /// </summary>
        public void DeleteCaiSddpParametro()
        {
            try
            {
                FactoryTransferencia.GetCaiSddpParametroRepository().Delete();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CAI_SDDP_PARAMETRO
        /// </summary>
        /// <param name="caiajcodi">Identificador de la tabla CAI_AJUSTE</param>
        public CaiSddpParametroDTO GetByIdCaiSddpParametro(int caiajcodi)
        {
            return FactoryTransferencia.GetCaiSddpParametroRepository().GetById(caiajcodi);
        }

        

        /// <summary>
        /// Permite listar todos los registros de la tabla CAI_SDDP_PARAMETRO
        /// </summary>
        /// <param name="Sddppmcodi">Identificador de la tabla CAI_SDDP_PARAMETRO</param>
        public List<CaiSddpParametroDTO> ListCaiSddpParametro(int Sddppmcodi)
        {
            return FactoryTransferencia.GetCaiSddpParametroRepository().List(Sddppmcodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CAI_SDDP_PARAMETRO
        /// </summary>
        public List<CaiSddpParametroDTO> GetByCriteriaCaiSddpParametro()
        {
            return FactoryTransferencia.GetCaiSddpParametroRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla CAI_SDDP_PARAMINT

        /// <summary>
        /// Inserta un registro de la tabla CAI_SDDP_PARAMINT
        /// </summary>
        public void SaveCaiSddpParamint(CaiSddpParamintDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCaiSddpParamintRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CAI_SDDP_PARAMINT
        /// </summary>
        public void UpdateCaiSddpParamint(CaiSddpParamintDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCaiSddpParamintRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CAI_SDDP_PARAMINT
        /// </summary>
        public void DeleteCaiSddpParamint()
        {
            try
            {
                FactoryTransferencia.GetCaiSddpParamintRepository().Delete();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CAI_SDDP_PARAMINT
        /// </summary>
        /// <param name="Sddppicodi">Identificador de la tabla CAI_SDDP_PARAMINT</param>
        public CaiSddpParamintDTO GetByIdCaiSddpParamint(int Sddppicodi)
        {
            return FactoryTransferencia.GetCaiSddpParamintRepository().GetById(Sddppicodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CAI_SDDP_PARAMINT
        /// </summary>
        /// <param name="Sddppicodi">Identificador de la tabla CAI_SDDP_PARAMINT</param>
        public List<CaiSddpParamintDTO> ListCaiSddpParamint(int Sddppicodi)
        {
            return FactoryTransferencia.GetCaiSddpParamintRepository().List(Sddppicodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CAI_SDDP_PARAMINT
        /// </summary>
        /// <param name="caiajcodi">Identificador de la tabla CAI_AJUSTE</param>
        public List<CaiSddpParamintDTO> GetByCriteriaCaiSddpParamint(int caiajcodi)
        {
            return FactoryTransferencia.GetCaiSddpParamintRepository().GetByCriteria(caiajcodi);
        }

        #endregion

        #region Métodos Tabla CAI_SDDP_PARAMSEM

        /// <summary>
        /// Inserta un registro de la tabla CAI_SDDP_PARAMSEM
        /// </summary>
        public void SaveCaiSddpParamsem(CaiSddpParamsemDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCaiSddpParamsemRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CAI_SDDP_PARAMSEM
        /// </summary>
        public void DeleteCaiSddpParamsem()
        {
            try
            {
                FactoryTransferencia.GetCaiSddpParamsemRepository().Delete();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CAI_SDDP_PARAMSEM
        /// </summary>
        /// <param name="caiajcodi">Identificador de la tabla CAI_AJUSTE</param>
        public List<CaiSddpParamsemDTO> GetByCriteriaCaiSddpParamsem(int caiajcodi)
        {
            return FactoryTransferencia.GetCaiSddpParamsemRepository().GetByCriteria(caiajcodi);
        }

        #endregion

        #region Métodos Genelares

        /// <summary>
        /// Almacena un archivo en excel en un data set
        /// </summary>
        /// <param name="RutaArchivo"></param>
        /// <param name="Hoja"></param>
        public DataSet GeneraDataset(string RutaArchivo, int hoja)
        {
            return UtilCalculoPorcentajes.GeneraDataset(RutaArchivo, hoja);
        }

        /// <summary>
        /// Permite analizar si un dato es numérico
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public Boolean AnalizarNumerico(string valor)
        {
            Boolean bresult = false;
            decimal number3;
            bresult = decimal.TryParse(valor, out number3);

            return bresult;
        }

        /// <summary>
        /// Procedimiento que se encarga de ejecutar el procedimiento del calculo de porcentajes
        /// </summary>
        /// <param name="caiprscodi">Identificador de la tabla de Presupuesto</param>
        /// <param name="caiajcodi">Identificador de la tabla Ajuste</param>
        /// <param name="suser">Usuario conectado</param>
        public string ProcesarCalculo(int caiprscodi, int caiajcodi, string suser)
        {
            string sResultado = "1";
            try
            {
                //Limpiamos Todos los calculos anteriores de la Version en Acción
                string sBorrar = this.EliminarCalculo(caiajcodi);
                if (!sBorrar.Equals("1"))
                {
                    sResultado = "Lo sentimos, No se pudo eliminar el proceso de cálculo: " + sBorrar;
                    return sResultado;
                }
                //INICIALIZAMOS ALGUNAS VARIABLES GENERALES
                //Traemos la entidad de la versión de recalculo
                CaiPresupuestoDTO EntidadPresupuesto = this.GetByIdCaiPresupuesto(caiprscodi);
                CaiAjusteDTO EntidadAjuste = this.GetByIdCaiAjuste(caiajcodi);
                List<CaiAjusteempresaDTO> ListaAjusteEmpresa = this.ListCaiAjusteempresasByAjuste(caiajcodi, "E");

                //Fecha de Inicio de Ejecución
                string sMesInicioEjecutado = EntidadPresupuesto.Caiprsmesinicio.ToString();
                if (sMesInicioEjecutado.Length == 1) sMesInicioEjecutado = "0" + sMesInicioEjecutado;
                var sFechaInicioEjecutado = "01/" + sMesInicioEjecutado + "/" + EntidadPresupuesto.Caiprsanio;
                DateTime dFecInicioEjecutado = DateTime.ParseExact(sFechaInicioEjecutado, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //Fecha de Inicio de Proyectado
                string sMesInicioProyectado = EntidadAjuste.Caiajmes.ToString();
                if (sMesInicioProyectado.Length == 1) sMesInicioProyectado = "0" + sMesInicioProyectado;
                var sFechaInicioProyectado = "01/" + sMesInicioProyectado + "/" + EntidadAjuste.Caiajanio;
                DateTime dFecInicioProyectado = DateTime.ParseExact(sFechaInicioProyectado, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //Fecha Final
                DateTime dFecFinal = dFecInicioProyectado.AddMonths(EntidadPresupuesto.Caiprsnromeses + 1);

                #region Cálculo de montos por generación7distribución/usuarioslibres Ejecutados y proyectados
                //CALCULAMOS LOS EJECUTADO
                string sFuente = "";
                //Traemos la lista de Meses del periodo Ejecutado
                List<CaiAjustecmarginalDTO> ListaAjusteCM = this.ListCaiAjustecmarginals(caiajcodi);
                foreach (CaiAjustecmarginalDTO dtoAjusteCM in ListaAjusteCM)
                {
                    //Para cada mes: PERICODI, RECACODI, CAAJCMMES
                    //Traemos la Fecha, Hora de la demanda maxima del mes:
                    CaiMaxdemandaDTO dtoMaxDemanda = this.GetCaiMaxdemandaByCaimdeAnioMes(caiajcodi, dtoAjusteCM.Caajcmmes);
                    if (dtoMaxDemanda == null)
                    {
                        sResultado = "Lo sentimos, No esta presente la máxima demanda ejecutada para " + dtoAjusteCM.Caajcmmes;
                        return sResultado;
                    }
                    //Aqui esta la fecha y hora, formato: MM/DD/YYYY HH:MM:SS
                    int iMinuto = dtoMaxDemanda.Caimdefechor.Value.Minute;
                    int iHora = dtoMaxDemanda.Caimdefechor.Value.Hour;
                    int IntervaloInicio = (iHora * 4) + 1; //De hora en hora: 0->H1 1->H5
                    foreach (CaiAjusteempresaDTO dtoAjusteEmpresa in ListaAjusteEmpresa)
                    {
                        decimal dCaimpgTotEnergia = 0;
                        decimal dCaimpgImpEnergia = 0;
                        decimal dCaimpgTotPotencia = 0;
                        decimal dCaimpgImpPotencia = 0;

                        #region Recorremos la lista para cada dia de un mes, la Generación/Distribución/UsuarioLibre Ejecutada y CM en intervalos de 15 minutos;
                        List<CaiGenerdemanDTO> ListaGeneracionEjecutada = this.ListCaiGenerdemanBarrasMes(caiajcodi, dtoAjusteEmpresa.Emprcodi, dtoAjusteEmpresa.Ptomedicodi, dtoAjusteCM.Caajcmmes, dtoAjusteCM.Pericodi, dtoAjusteCM.Recacodi);
                        foreach (CaiGenerdemanDTO dtoGeneracionEjecutada in ListaGeneracionEjecutada)
                        {
                            dCaimpgTotEnergia += dtoGeneracionEjecutada.Cagdcmtotaldia;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H1 * dtoGeneracionEjecutada.CM1 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H2 * dtoGeneracionEjecutada.CM2 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H3 * dtoGeneracionEjecutada.CM3 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H4 * dtoGeneracionEjecutada.CM4 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H5 * dtoGeneracionEjecutada.CM5 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H6 * dtoGeneracionEjecutada.CM6 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H7 * dtoGeneracionEjecutada.CM7 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H8 * dtoGeneracionEjecutada.CM8 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H9 * dtoGeneracionEjecutada.CM9 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H10 * dtoGeneracionEjecutada.CM10 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H11 * dtoGeneracionEjecutada.CM11 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H12 * dtoGeneracionEjecutada.CM12 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H13 * dtoGeneracionEjecutada.CM13 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H14 * dtoGeneracionEjecutada.CM14 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H15 * dtoGeneracionEjecutada.CM15 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H16 * dtoGeneracionEjecutada.CM16 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H17 * dtoGeneracionEjecutada.CM17 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H18 * dtoGeneracionEjecutada.CM18 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H19 * dtoGeneracionEjecutada.CM19 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H20 * dtoGeneracionEjecutada.CM20 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H21 * dtoGeneracionEjecutada.CM21 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H22 * dtoGeneracionEjecutada.CM22 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H23 * dtoGeneracionEjecutada.CM23 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H24 * dtoGeneracionEjecutada.CM24 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H25 * dtoGeneracionEjecutada.CM25 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H26 * dtoGeneracionEjecutada.CM26 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H27 * dtoGeneracionEjecutada.CM27 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H28 * dtoGeneracionEjecutada.CM28 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H29 * dtoGeneracionEjecutada.CM29 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H30 * dtoGeneracionEjecutada.CM30 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H31 * dtoGeneracionEjecutada.CM31 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H32 * dtoGeneracionEjecutada.CM32 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H33 * dtoGeneracionEjecutada.CM33 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H34 * dtoGeneracionEjecutada.CM34 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H35 * dtoGeneracionEjecutada.CM35 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H36 * dtoGeneracionEjecutada.CM36 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H37 * dtoGeneracionEjecutada.CM37 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H38 * dtoGeneracionEjecutada.CM38 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H39 * dtoGeneracionEjecutada.CM39 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H40 * dtoGeneracionEjecutada.CM40 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H41 * dtoGeneracionEjecutada.CM41 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H42 * dtoGeneracionEjecutada.CM42 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H43 * dtoGeneracionEjecutada.CM43 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H44 * dtoGeneracionEjecutada.CM44 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H45 * dtoGeneracionEjecutada.CM45 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H46 * dtoGeneracionEjecutada.CM46 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H47 * dtoGeneracionEjecutada.CM47 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H48 * dtoGeneracionEjecutada.CM48 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H49 * dtoGeneracionEjecutada.CM49 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H50 * dtoGeneracionEjecutada.CM50 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H51 * dtoGeneracionEjecutada.CM51 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H52 * dtoGeneracionEjecutada.CM52 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H53 * dtoGeneracionEjecutada.CM53 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H54 * dtoGeneracionEjecutada.CM54 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H55 * dtoGeneracionEjecutada.CM55 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H56 * dtoGeneracionEjecutada.CM56 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H57 * dtoGeneracionEjecutada.CM57 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H58 * dtoGeneracionEjecutada.CM58 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H59 * dtoGeneracionEjecutada.CM59 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H60 * dtoGeneracionEjecutada.CM60 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H61 * dtoGeneracionEjecutada.CM61 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H62 * dtoGeneracionEjecutada.CM62 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H63 * dtoGeneracionEjecutada.CM63 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H64 * dtoGeneracionEjecutada.CM64 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H65 * dtoGeneracionEjecutada.CM65 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H66 * dtoGeneracionEjecutada.CM66 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H67 * dtoGeneracionEjecutada.CM67 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H68 * dtoGeneracionEjecutada.CM68 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H69 * dtoGeneracionEjecutada.CM69 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H70 * dtoGeneracionEjecutada.CM70 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H71 * dtoGeneracionEjecutada.CM71 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H72 * dtoGeneracionEjecutada.CM72 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H73 * dtoGeneracionEjecutada.CM73 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H74 * dtoGeneracionEjecutada.CM74 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H75 * dtoGeneracionEjecutada.CM75 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H76 * dtoGeneracionEjecutada.CM76 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H77 * dtoGeneracionEjecutada.CM77 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H78 * dtoGeneracionEjecutada.CM78 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H79 * dtoGeneracionEjecutada.CM79 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H80 * dtoGeneracionEjecutada.CM80 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H81 * dtoGeneracionEjecutada.CM81 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H82 * dtoGeneracionEjecutada.CM82 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H83 * dtoGeneracionEjecutada.CM83 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H84 * dtoGeneracionEjecutada.CM84 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H85 * dtoGeneracionEjecutada.CM85 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H86 * dtoGeneracionEjecutada.CM86 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H87 * dtoGeneracionEjecutada.CM87 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H88 * dtoGeneracionEjecutada.CM88 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H89 * dtoGeneracionEjecutada.CM89 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H90 * dtoGeneracionEjecutada.CM90 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H91 * dtoGeneracionEjecutada.CM91 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H92 * dtoGeneracionEjecutada.CM92 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H93 * dtoGeneracionEjecutada.CM93 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H94 * dtoGeneracionEjecutada.CM94 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H95 * dtoGeneracionEjecutada.CM95 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H96 * dtoGeneracionEjecutada.CM96 * 1000;
                            //Seleccionamos el dia de Máxima demanda
                            if ((dtoGeneracionEjecutada.Cagdcmdia.Value <= dtoMaxDemanda.Caimdefechor.Value) && (dtoGeneracionEjecutada.Cagdcmdia.Value.AddDays(1) > dtoMaxDemanda.Caimdefechor.Value))
                            {
                                sFuente = dtoGeneracionEjecutada.Cagdcmfuentedat;
                                //Es el dia de Maxima Demanda: H1, H5, H9, ... H93
                                decimal dH1 = 0;
                                decimal dH1Aux = 0;
                                //decimal dCM1 = 0;
                                for (int i = IntervaloInicio; i < IntervaloInicio + 4; i++ )
                                {
                                    dH1Aux = (decimal)dtoGeneracionEjecutada.GetType().GetProperty("H" + i).GetValue(dtoGeneracionEjecutada, null);
                                    if (dH1 < dH1Aux)
                                        dH1 = dH1Aux;
                                    //dCM1 = (decimal)dtoGeneracionEjecutada.GetType().GetProperty("CM" + i).GetValue(dtoGeneracionEjecutada, null);
                                
                                }
                                dCaimpgTotPotencia = dH1;
                                dCaimpgImpPotencia = dCaimpgTotPotencia * dtoMaxDemanda.Caimdemaxdemanda;
                            }
                        }
                        #endregion

                        CaiImpgeneracionDTO dtoImpgeneracion = new CaiImpgeneracionDTO();
                        dtoImpgeneracion.Caiajcodi = caiajcodi;
                        dtoImpgeneracion.Caimpgfuentedat = sFuente;
                        dtoImpgeneracion.Emprcodi = dtoAjusteEmpresa.Emprcodi;
                        dtoImpgeneracion.Ptomedicodi = dtoAjusteEmpresa.Ptomedicodi;
                        dtoImpgeneracion.Caimpgmes = dtoAjusteCM.Caajcmmes;
                        dtoImpgeneracion.Caimpgtotenergia = dCaimpgTotEnergia;
                        dtoImpgeneracion.Caimpgimpenergia = dCaimpgImpEnergia;
                        dtoImpgeneracion.Caimpgtotpotencia = dCaimpgTotPotencia;
                        dtoImpgeneracion.Caimpgimppotencia = dCaimpgImpPotencia;
                        dtoImpgeneracion.Caimpgusucreacion = suser;
                        this.SaveCaiImpgeneracion(dtoImpgeneracion);
                    }
                }
                //CALCULAMOS LO PROYECTADO
                sFuente = "";
                //Traemos la lista de Meses del periodo Proyectado
                DateTime dFecProyectado = dFecInicioProyectado;
                while (dFecProyectado < dFecFinal)
                {
                    //Extraemos año mes YYYYMM
                    int iAnioMes = dFecProyectado.Year * 100 + dFecProyectado.Month;
                    //Traemos la Fecha, Hora de la demanda maxima del mes:
                    CaiMaxdemandaDTO dtoMaxDemanda = this.GetCaiMaxdemandaByCaimdeAnioMes(caiajcodi, iAnioMes);
                    if (dtoMaxDemanda == null)
                    {
                        sResultado = "Lo sentimos, No esta presente la máxima demanda proyectada para " + iAnioMes;
                        return sResultado;
                    }
                    dFecProyectado = dFecProyectado.AddMonths(1);
                    //Aqui esta la fecha y hora, formato: MM/DD/YYYY HH:MM:SS
                    int iMinuto = dtoMaxDemanda.Caimdefechor.Value.Minute;
                    int iHora = dtoMaxDemanda.Caimdefechor.Value.Hour;
                    int IntervaloInicio = (iHora * 4) + 1; //De hora en hora: 0->H1 1->H5
                    foreach (CaiAjusteempresaDTO dtoAjusteEmpresa in ListaAjusteEmpresa)
                    {
                        decimal dCaimpgTotEnergia = 0;
                        decimal dCaimpgImpEnergia = 0;
                        decimal dCaimpgTotPotencia = 0;
                        decimal dCaimpgImpPotencia = 0;

                        #region Recorremos la lista para cada dia de un mes, de la Generación / Distribución / Usuario Libre Proyectada asociado a su correspondiente CM Proyectado en intervalos de 15 minutos;
                        List<CaiGenerdemanDTO> ListaGeneracionEjecutada = this.ListCaiGenerdemanProyectadoMes(caiajcodi, dtoAjusteEmpresa.Emprcodi, dtoAjusteEmpresa.Ptomedicodi, iAnioMes);
                        foreach (CaiGenerdemanDTO dtoGeneracionEjecutada in ListaGeneracionEjecutada)
                        {
                            dCaimpgTotEnergia += dtoGeneracionEjecutada.Cagdcmtotaldia;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H1 * dtoGeneracionEjecutada.CM1 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H2 * dtoGeneracionEjecutada.CM2 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H3 * dtoGeneracionEjecutada.CM3 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H4 * dtoGeneracionEjecutada.CM4 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H5 * dtoGeneracionEjecutada.CM5 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H6 * dtoGeneracionEjecutada.CM6 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H7 * dtoGeneracionEjecutada.CM7 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H8 * dtoGeneracionEjecutada.CM8 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H9 * dtoGeneracionEjecutada.CM9 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H10 * dtoGeneracionEjecutada.CM10 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H11 * dtoGeneracionEjecutada.CM11 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H12 * dtoGeneracionEjecutada.CM12 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H13 * dtoGeneracionEjecutada.CM13 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H14 * dtoGeneracionEjecutada.CM14 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H15 * dtoGeneracionEjecutada.CM15 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H16 * dtoGeneracionEjecutada.CM16 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H17 * dtoGeneracionEjecutada.CM17 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H18 * dtoGeneracionEjecutada.CM18 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H19 * dtoGeneracionEjecutada.CM19 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H20 * dtoGeneracionEjecutada.CM20 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H21 * dtoGeneracionEjecutada.CM21 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H22 * dtoGeneracionEjecutada.CM22 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H23 * dtoGeneracionEjecutada.CM23 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H24 * dtoGeneracionEjecutada.CM24 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H25 * dtoGeneracionEjecutada.CM25 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H26 * dtoGeneracionEjecutada.CM26 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H27 * dtoGeneracionEjecutada.CM27 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H28 * dtoGeneracionEjecutada.CM28 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H29 * dtoGeneracionEjecutada.CM29 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H30 * dtoGeneracionEjecutada.CM30 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H31 * dtoGeneracionEjecutada.CM31 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H32 * dtoGeneracionEjecutada.CM32 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H33 * dtoGeneracionEjecutada.CM33 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H34 * dtoGeneracionEjecutada.CM34 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H35 * dtoGeneracionEjecutada.CM35 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H36 * dtoGeneracionEjecutada.CM36 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H37 * dtoGeneracionEjecutada.CM37 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H38 * dtoGeneracionEjecutada.CM38 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H39 * dtoGeneracionEjecutada.CM39 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H40 * dtoGeneracionEjecutada.CM40 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H41 * dtoGeneracionEjecutada.CM41 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H42 * dtoGeneracionEjecutada.CM42 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H43 * dtoGeneracionEjecutada.CM43 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H44 * dtoGeneracionEjecutada.CM44 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H45 * dtoGeneracionEjecutada.CM45 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H46 * dtoGeneracionEjecutada.CM46 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H47 * dtoGeneracionEjecutada.CM47 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H48 * dtoGeneracionEjecutada.CM48 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H49 * dtoGeneracionEjecutada.CM49 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H50 * dtoGeneracionEjecutada.CM50 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H51 * dtoGeneracionEjecutada.CM51 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H52 * dtoGeneracionEjecutada.CM52 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H53 * dtoGeneracionEjecutada.CM53 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H54 * dtoGeneracionEjecutada.CM54 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H55 * dtoGeneracionEjecutada.CM55 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H56 * dtoGeneracionEjecutada.CM56 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H57 * dtoGeneracionEjecutada.CM57 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H58 * dtoGeneracionEjecutada.CM58 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H59 * dtoGeneracionEjecutada.CM59 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H60 * dtoGeneracionEjecutada.CM60 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H61 * dtoGeneracionEjecutada.CM61 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H62 * dtoGeneracionEjecutada.CM62 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H63 * dtoGeneracionEjecutada.CM63 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H64 * dtoGeneracionEjecutada.CM64 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H65 * dtoGeneracionEjecutada.CM65 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H66 * dtoGeneracionEjecutada.CM66 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H67 * dtoGeneracionEjecutada.CM67 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H68 * dtoGeneracionEjecutada.CM68 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H69 * dtoGeneracionEjecutada.CM69 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H70 * dtoGeneracionEjecutada.CM70 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H71 * dtoGeneracionEjecutada.CM71 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H72 * dtoGeneracionEjecutada.CM72 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H73 * dtoGeneracionEjecutada.CM73 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H74 * dtoGeneracionEjecutada.CM74 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H75 * dtoGeneracionEjecutada.CM75 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H76 * dtoGeneracionEjecutada.CM76 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H77 * dtoGeneracionEjecutada.CM77 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H78 * dtoGeneracionEjecutada.CM78 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H79 * dtoGeneracionEjecutada.CM79 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H80 * dtoGeneracionEjecutada.CM80 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H81 * dtoGeneracionEjecutada.CM81 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H82 * dtoGeneracionEjecutada.CM82 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H83 * dtoGeneracionEjecutada.CM83 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H84 * dtoGeneracionEjecutada.CM84 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H85 * dtoGeneracionEjecutada.CM85 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H86 * dtoGeneracionEjecutada.CM86 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H87 * dtoGeneracionEjecutada.CM87 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H88 * dtoGeneracionEjecutada.CM88 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H89 * dtoGeneracionEjecutada.CM89 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H90 * dtoGeneracionEjecutada.CM90 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H91 * dtoGeneracionEjecutada.CM91 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H92 * dtoGeneracionEjecutada.CM92 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H93 * dtoGeneracionEjecutada.CM93 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H94 * dtoGeneracionEjecutada.CM94 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H95 * dtoGeneracionEjecutada.CM95 * 1000;
                            dCaimpgImpEnergia += dtoGeneracionEjecutada.H96 * dtoGeneracionEjecutada.CM96 * 1000;
                            //Seleccionamos el dia de Máxima demanda
                            if ((dtoGeneracionEjecutada.Cagdcmdia.Value <= dtoMaxDemanda.Caimdefechor.Value) && (dtoGeneracionEjecutada.Cagdcmdia.Value.AddDays(1) > dtoMaxDemanda.Caimdefechor.Value))
                            {
                                sFuente = dtoGeneracionEjecutada.Cagdcmfuentedat;
                                //Es el dia de Maxima Demanda: H1, H5, H9, ... H93
                                decimal dH1 = 0;
                                decimal dH1Aux = 0;
                                //decimal dCM1 = 0;
                                for (int i = IntervaloInicio; i < IntervaloInicio + 4; i++)
                                {
                                    dH1Aux = (decimal)dtoGeneracionEjecutada.GetType().GetProperty("H" + i).GetValue(dtoGeneracionEjecutada, null);
                                    if (dH1 < dH1Aux)
                                        dH1 = dH1Aux;
                                    //dCM1 = (decimal)dtoGeneracionEjecutada.GetType().GetProperty("CM" + i).GetValue(dtoGeneracionEjecutada, null);

                                }
                                dCaimpgTotPotencia = dH1;
                                dCaimpgImpPotencia = dCaimpgTotPotencia * dtoMaxDemanda.Caimdemaxdemanda;
                            }
                        }
                        #endregion

                        CaiImpgeneracionDTO dtoImpgeneracion = new CaiImpgeneracionDTO();
                        dtoImpgeneracion.Caiajcodi = caiajcodi;
                        dtoImpgeneracion.Caimpgfuentedat = sFuente;
                        dtoImpgeneracion.Emprcodi = dtoAjusteEmpresa.Emprcodi;
                        dtoImpgeneracion.Ptomedicodi = dtoAjusteEmpresa.Ptomedicodi;
                        dtoImpgeneracion.Caimpgmes = iAnioMes;
                        dtoImpgeneracion.Caimpgtotenergia = dCaimpgTotEnergia;
                        dtoImpgeneracion.Caimpgimpenergia = dCaimpgImpEnergia;
                        dtoImpgeneracion.Caimpgtotpotencia = dCaimpgTotPotencia;
                        dtoImpgeneracion.Caimpgimppotencia = dCaimpgImpPotencia;
                        dtoImpgeneracion.Caimpgusucreacion = suser;
                        this.SaveCaiImpgeneracion(dtoImpgeneracion);
                    }
                }
                #endregion

                //El Cálculo de montos por ingresos por transmisión, ya esta registrado en la tabla CAI_INGTRANSMISION

                #region Cálculo del porcentaje del aporte de cada empresa
                //Traemos los totales por empresa para el ajustes: Gi, Di, Ti y Li 
                List<CaiPorctaporteDTO> ListaEmpresaImporte = this.ListCaiPorctaportesByEmpresaImporte(caiajcodi);
                //Calculamos el total de los importes de Gi, Di, Ti y Li 
                decimal dTotalCaipaImpAporte = 0;
                foreach (CaiPorctaporteDTO dtoEmpresaImporte in ListaEmpresaImporte)
                {
                    dTotalCaipaImpAporte += dtoEmpresaImporte.Caipaimpaporte;
                }
                if (dTotalCaipaImpAporte == 0)
                    dTotalCaipaImpAporte = 1;
                //Calculando su porcentaje para todas las empresas
                foreach (CaiPorctaporteDTO dtoEmpresaImporte in ListaEmpresaImporte)
                {
                    dtoEmpresaImporte.Caiajcodi = caiajcodi;
                    dtoEmpresaImporte.Caipapctaporte = (dtoEmpresaImporte.Caipaimpaporte / dTotalCaipaImpAporte) * 100;
                    dtoEmpresaImporte.Caipausucreacion = suser;
                    this.SaveCaiPorctaporte(dtoEmpresaImporte);
                }
                #endregion
            }
            catch (Exception e)
            {
                sResultado = e.StackTrace;
                sResultado = e.Message; //"-1";
            }
            return sResultado;
        }

        /// <summary>
        /// Procedimiento que se encarga de eliminar el calculo de porcentajes
        /// </summary>
        /// <param name="caiajcodi">Identificador de la versión del Ajuste</param>
        public string EliminarCalculo(int caiajcodi)
        {
            try
            {
                //Elimina información de la tabla CAI_PORCTAPORTE
                this.DeleteCaiPorctaporte(caiajcodi);

                //Elimina información de la tabla CAI_IMPGENERACION
                this.DeleteCaiImpgeneracion(caiajcodi);

                return "1";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        /// <summary>
        /// Permite listar los puntos de medicion la tabla MeHojaptomed
        /// </summary>
        public List<MeHojaptomedDTO> GetByCriteriaMeHojaptomeds(int emprcodi, int formatcodi)
        {
            return FactoryTransferencia.GetCaiAjusteempresaRepository().GetByCriteriaMeHojaptomeds(emprcodi, formatcodi);
        }

        /// <summary>
        /// Graba Configuracion de formato envio
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int GrabarConfigFormatEnvio(MeConfigformatenvioDTO entity)
        {
            int idCfgEnvio = 0;
            var lista = this.GetByCriteriaMeHojaptomeds((int)entity.Emprcodi, (int)entity.Formatcodi);
            if (lista.Count > 0)
            {
                entity.Cfgenvptos = string.Join(",", lista.Select(x => x.Ptomedicodi).ToList());
                entity.Cfgenvorden = string.Join(",", lista.Select(x => x.Hojaptoorden).ToList());
                entity.Cfgenvtipoinf = string.Join(",", lista.Select(x => x.Tipoinfocodi).ToList());
                //MANUEL: 20180904
                //TUVE QUE AGREGAR EL ULTIMO PARAMETRO entity.Cfgenvhojas PARA QUE COPILARA... INVESTIGAR QUE PARAMETRO ESPERA.
                idCfgEnvio = this.logic.VerificaFormatoUpdate((int)entity.Emprcodi, (int)entity.Formatcodi, entity.Cfgenvptos, entity.Cfgenvorden, entity.Cfgenvtipoinf, entity.Cfgenvtipopto, entity.Cfgenvhojas);
                if (idCfgEnvio == 0)
                {
                    entity.Cfgenvfecha = DateTime.Now;
                    idCfgEnvio = this.logic.SaveMeConfigformatenvio(entity);
                }
            }
            return idCfgEnvio;
        }
        #endregion

        #region Reportes del Calculo
        /// <summary>
        /// Permite generar el archivo de exportación de la Información "Energia" 
        /// </summary>
        /// <param name="caiprscodi">Código del presupuesto</param>
        /// <param name="caiajcodi">Código de la version del ajuste</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarReporteEnergia(int caiprscodi, int caiajcodi, int formato, string pathFile, string pathLogo)
        {
            string fileName = string.Empty;
            //Traemos la entidad de la versión del presupuesto
            CaiPresupuestoDTO EntidadPresupuesto = this.GetByIdCaiPresupuesto(caiprscodi);

            //Traemos la entidad de la versión del ajuste
            CaiAjusteDTO EntidadAjuste = this.GetByIdCaiAjuste(caiajcodi);

            //Obtengo el codigo de
            List<CaiAjusteempresaDTO> ListaEmpresas = this.ListCaiAjusteempresasTipoEmpr(caiajcodi);

            if (formato == 1)
            {
                fileName = "ReporteEnergia.xlsx";
                ExcelDocument.GenerarReporteEnergia(pathFile + fileName, EntidadPresupuesto, EntidadAjuste, ListaEmpresas);
            }
            return fileName;
        }

        /// <summary>
        /// Permite generar el archivo de exportación de la Información "Energia" 
        /// </summary>
        /// <param name="caiprscodi">Código del presupuesto</param>
        /// <param name="caiajcodi">Código de la version del ajuste</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarReportePotencia(int caiprscodi, int caiajcodi, int formato, string pathFile, string pathLogo)
        {
            string fileName = string.Empty;
            //Traemos la entidad de la versión del presupuesto
            CaiPresupuestoDTO EntidadPresupuesto = this.GetByIdCaiPresupuesto(caiprscodi);

            //Traemos la entidad de la versión del ajuste
            CaiAjusteDTO EntidadAjuste = this.GetByIdCaiAjuste(caiajcodi);

            //Obtengo el codigo de
            List<CaiAjusteempresaDTO> ListaEmpresas = this.ListCaiAjusteempresasListEmpresasByAjuste(caiajcodi);

            if (formato == 1)
            {
                fileName = "ReportePotencia.xlsx";
                ExcelDocument.GenerarReportePotencia(pathFile + fileName, EntidadPresupuesto, EntidadAjuste, ListaEmpresas);
            }
            return fileName;
        }

        /// <summary>
        /// Permite generar el archivo de exportación de la Información "Transmision" 
        /// </summary>
        /// <param name="caiprscodi">Código del presupuesto</param>
        /// <param name="caiajcodi">Código de la version del ajuste</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarReporteTransmision(int caiprscodi, int caiajcodi, int formato, string pathFile, string pathLogo)
        {
            string fileName = string.Empty;
            //Traemos la entidad de la versión del presupuesto
            CaiPresupuestoDTO EntidadPresupuesto = this.GetByIdCaiPresupuesto(caiprscodi);

            //Traemos la entidad de la versión del ajuste
            CaiAjusteDTO EntidadAjuste = this.GetByIdCaiAjuste(caiajcodi);

            //Obtengo el codigo de
            List<CaiAjusteempresaDTO> ListaEmpresas = this.ListCaiAjusteempresasListEmpresasByAjuste(caiajcodi);

            if (formato == 1)
            {
                fileName = "ReporteTransmisión.xlsx";
                ExcelDocument.GenerarReporteTransmision(pathFile + fileName, EntidadPresupuesto, EntidadAjuste, ListaEmpresas);
            }
            return fileName;
        }

        /// <summary>
        /// Permite generar el archivo de exportación de la Información "Porcentajes" 
        /// </summary>
        /// <param name="caiprscodi">Código del presupuesto</param>
        /// <param name="caiajcodi">Código de la version del ajuste</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarReportePorcentajes(int caiprscodi, int caiajcodi, int formato, string pathFile, string pathLogo)
        {
            string fileName = string.Empty;
            //Traemos la entidad de la versión del presupuesto
            CaiPresupuestoDTO EntidadPresupuesto = this.GetByIdCaiPresupuesto(caiprscodi);

            //Traemos la entidad de la versión del ajuste
            CaiAjusteDTO EntidadAjuste = this.GetByIdCaiAjuste(caiajcodi);

            //Obtengo el codigo de
            List<CaiAjusteempresaDTO> ListaEmpresas = this.ListCaiAjusteempresasTipoEmpr(caiajcodi);

            if (formato == 1)
            {
                fileName = "ReporteTransmisión.xlsx";
                ExcelDocument.GenerarReportePorcentajes(pathFile + fileName, EntidadPresupuesto, EntidadAjuste, ListaEmpresas);
            }
            return fileName;
        }

        #endregion

        
    }
}
