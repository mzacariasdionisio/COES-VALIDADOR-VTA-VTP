using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.IEOD
{
    public class DesconexionEquiposAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(DesconexionEquiposAppServicio));

        #region Métodos Tabla SI_EMPRESA
        /// <summary>
        /// Devuelve lista de empresa por tipo de empresa
        /// </summary>
        /// <param name="idFormato"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> GetListaCriteria(string strTipoempresa)
        {
            return FactorySic.GetSiEmpresaRepository().GetByCriteria(strTipoempresa);
        }

        #endregion

        #region METODOS TABLA EQ_EQUIPO

        /// <summary>
        /// Genera los tipos de equipo para la empresa seleccionada
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> GetListaTiposEquiposXEmpresa(int idEmpresa)
        {
            return FactorySic.GetEqEquipoRepository().ListadoXEmpresa(idEmpresa);
        }

        /// <summary>
        /// Lista de Equipos según el tipo de equipo seleccionado
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="famCodi"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> GetListaEquiposXFamilia(int idEmpresa, int famCodi)
        {
            return FactorySic.GetEqEquipoRepository().GetByEmprFam(idEmpresa, famCodi);
        }

        /// <summary>
        /// devuelve detalle de equipo seleccionado
        /// </summary>
        /// <param name="idEquipo"></param>
        /// <returns></returns>
        public EqEquipoDTO GetEquipoxId(int idEquipo)
        {
            return FactorySic.GetEqEquipoRepository().GetById(idEquipo);
        }

        /// <summary>
        /// Devuelve la lista de equipos filtrados por empresa y por tipo de central de generacion
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="iCodFamilias"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> ListarCentralesXEmpresaGener(string idEmpresa, string codFamilias)
        {
            return FactorySic.GetEqEquipoRepository().ListarCentralesXEmpresaXFamiliaGEN(idEmpresa, codFamilias);
        }
        #endregion

        #region METODOS TABLA EVE_IEODCUADRO

        public void UpdateDesconexion(EveIeodcuadroDTO entity)
        {
            FactorySic.GetEveIeodcuadroRepository().Update(entity);
        }

        public int SaveDesconexion(EveIeodcuadroDTO entity)
        {
            return FactorySic.GetEveIeodcuadroRepository().Save(entity);
        }

        public List<EveIeodcuadroDTO> GetListarIeodCuadroxEmpresa(DateTime fechaInicio, DateTime FechaFin, int subCausaCodi, int emprcodi)
        {
            return FactorySic.GetEveIeodcuadroRepository().ListarEveIeodCuadroxEmpresa(fechaInicio, FechaFin, subCausaCodi, emprcodi);
        }

        public EveIeodcuadroDTO GetByIdRestriccion(int iccodi)
        {
            return FactorySic.GetEveIeodcuadroRepository().GetById(iccodi);
        }

        public void DeleteLogicoDesconexion(int iccodi)
        {
            FactorySic.GetEveIeodcuadroRepository().BorradoLogico(iccodi);
        }

        public List<EveIeodcuadroDTO> GetListarDesconexionesxCodigos(string pkCodis)
        {
            return FactorySic.GetEveIeodcuadroRepository().GetCriteriaxPKCodis(pkCodis);
        }

        #endregion

        #region Métodos Tabla ME_ENVIO

        /// <summary>
        /// Inserta un registro de la tabla ME_ENVIO
        /// </summary>
        public int SaveMeEnvio(MeEnvioDTO entity)
        {
            try
            {
                return FactorySic.GetMeEnvioRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ME_ENVIO
        /// </summary>
        public void UpdateMeEnvio(MeEnvioDTO entity)
        {
            try
            {
                FactorySic.GetMeEnvioRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Actualiza un registro de la tabla ME_ENVIO
        /// </summary>
        public void UpdateMeEnvio1(MeEnvioDTO entity)
        {
            try
            {
                FactorySic.GetMeEnvioRepository().Update1(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ME_ENVIO
        /// </summary>
        public MeEnvioDTO GetByIdMeEnvio(int idEnvio)
        {
            return FactorySic.GetMeEnvioRepository().GetById(idEnvio);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla MeEnvio
        /// </summary>
        public List<MeEnvioDTO> GetByCriteriaMeEnvios(int idEmpresa, int idFormato, DateTime fecha)
        {
            return FactorySic.GetMeEnvioRepository().GetByCriteria(idEmpresa, idFormato, fecha);
        }


        /// <summary>
        /// Lista de Envios por paginado
        /// </summary>
        /// <param name="idsEmpresa"></param>
        /// <param name="idsLectura"></param>
        /// <param name="idsFormato"></param>
        /// <param name="idsEstado"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="nroPaginas"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<MeEnvioDTO> GetListaMultipleMeEnvios(string idsEmpresa, string idsLectura, string idsFormato, string idsEstado, DateTime fechaIni, DateTime fechaFin, int nroPaginas, int pageSize)
        {
            if (string.IsNullOrEmpty(idsEmpresa)) idsEmpresa = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsLectura)) idsLectura = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsFormato)) idsFormato = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsEstado)) idsEstado = ConstantesAppServicio.ParametroDefecto;
            return FactorySic.GetMeEnvioRepository().GetListaMultiple(idsEmpresa, idsLectura, idsFormato, idsEstado, fechaIni, fechaFin, nroPaginas, pageSize);
        }

        /// <summary>
        /// Lista de envios para consulta excel si paginado
        /// </summary>
        /// <param name="idsEmpresa"></param>
        /// <param name="idsLectura"></param>
        /// <param name="idsFormato"></param>
        /// <param name="idsEstado"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<MeEnvioDTO> GetListaMultipleMeEnviosXLS(string idsEmpresa, string idsLectura, string idsFormato, string idsEstado, DateTime fechaIni, DateTime fechaFin)
        {
            if (string.IsNullOrEmpty(idsEmpresa)) idsEmpresa = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsLectura)) idsLectura = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsFormato)) idsFormato = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsEstado)) idsEstado = ConstantesAppServicio.ParametroDefecto;
            return FactorySic.GetMeEnvioRepository().GetListaMultipleXLS(idsEmpresa, idsLectura, idsFormato, idsEstado, fechaIni, fechaFin);
        }

        /// <summary>
        /// Devuelve el total de registros para listado de envios por paginado
        /// </summary>
        /// <param name="idsEmpresa"></param>
        /// <param name="idsLectura"></param>
        /// <param name="idsFormato"></param>
        /// <param name="idsEstado"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public int TotalListaMultipleMeEnvios(string idsEmpresa, string idsLectura, string idsFormato, string idsEstado, DateTime fechaIni, DateTime fechaFin)
        {
            if (string.IsNullOrEmpty(idsEmpresa)) idsEmpresa = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsLectura)) idsLectura = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsFormato)) idsFormato = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsEstado)) idsEstado = ConstantesAppServicio.ParametroDefecto;
            return FactorySic.GetMeEnvioRepository().TotalListaMultiple(idsEmpresa, idsLectura, idsFormato, idsEstado, fechaIni, fechaFin);
        }

        /// <summary>
        /// Obtiene el maximo id del envio de un formato de todos los periodos
        /// </summary>
        /// <param name="idFormato"></param>
        /// <returns></returns>
        public int ObtenerIdMaxEnvioFormato(int idFormato, int idEmpresa)
        {
            return FactorySic.GetMeEnvioRepository().GetMaxIdEnvioFormato(idFormato, idEmpresa);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla MeEnvio por rango de fechas
        /// </summary>
        public List<MeEnvioDTO> GetByCriteriaRangoFecha(int idEmpresa, int idFormato, DateTime fechaini, DateTime fechafin)
        {
            return FactorySic.GetMeEnvioRepository().GetByCriteriaRangoFecha(idEmpresa, idFormato, fechaini, fechafin);
        }

        /// <summary>
        /// Obtiene el maximo id del envio de un formato de todos los periodos
        /// </summary>
        /// <param name="idFormato"></param>
        /// <returns></returns>
        public int ObtenerIdMaxEnvioFormatoPeriodo(int idFormato, int idEmpresa, DateTime periodo)
        {
            return FactorySic.GetMeEnvioRepository().GetByMaxEnvioFormatoPeriodo(idFormato, idEmpresa, periodo);
        }


        #endregion

        #region Métodos Tabla ME_ENVIODET

        /// <summary>
        /// Inserta un registro de la tabla ME_ENVIODET
        /// </summary>
        public void SaveMeEnviodet(MeEnviodetDTO entity)
        {
            try
            {
                FactorySic.GetMeEnviodetRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ME_ENVIODET
        /// </summary>
        public void UpdateMeEnviodet(MeEnviodetDTO entity)
        {
            try
            {
                FactorySic.GetMeEnviodetRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla ME_ENVIODET
        /// </summary>
        public void DeleteMeEnviodet(int enviocodi, int fdatpkcodi)
        {
            try
            {
                FactorySic.GetMeEnviodetRepository().Delete(enviocodi, fdatpkcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ME_ENVIODET
        /// </summary>
        public MeEnviodetDTO GetByIdMeEnviodet(int enviocodi)
        {
            return FactorySic.GetMeEnviodetRepository().GetById(enviocodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_ENVIODET
        /// </summary>
        public List<MeEnviodetDTO> ListMeEnviodets()
        {
            return FactorySic.GetMeEnviodetRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla MeEnviodet
        /// </summary>
        public List<MeEnviodetDTO> GetByCriteriaMeEnviodets(int enviocodi)
        {
            return FactorySic.GetMeEnviodetRepository().GetByCriteria(enviocodi);
        }

        #endregion


    }
}
