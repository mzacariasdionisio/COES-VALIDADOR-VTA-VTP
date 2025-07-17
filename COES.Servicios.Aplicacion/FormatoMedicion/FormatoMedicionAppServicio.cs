using COES.Base.Core;
using COES.Dominio.DTO.Scada;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.CortoPlazo;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Interconexiones.Helper;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using COES.Servicios.Aplicacion.StockCombustibles;
using log4net;
using Microsoft.PowerBI.Api.V2.Models;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
#region Mejoras RDO
using COES.Servicios.Aplicacion.YupanaContinuo.Helper;
using OfficeOpenXml.Drawing;
using System.Net.Mail;
using System.Configuration;
using System.Net;
using static COES.Dominio.DTO.Sic.EqEquipoDTO;
#endregion

namespace COES.Servicios.Aplicacion.FormatoMedicion
{
    public class FormatoMedicionAppServicio : AppServicioBase
    {
        EmpresaAppServicio servEmpresa = new EmpresaAppServicio();
        McpAppServicio servicioMCP = new McpAppServicio();
        GeneracionRERAppServicio servRer = new GeneracionRERAppServicio();
        private static readonly ILog Logger = LogManager.GetLogger(typeof(FormatoMedicionAppServicio));

        public FormatoMedicionAppServicio()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        #region Tabla MeMediocion96

        /// <summary>
        /// Borra registro de la tabla medicion 96
        /// </summary>
        /// <param name="idPtomedicion"></param>
        /// <param name="idTipoInfo"></param>
        /// <param name="fecha"></param>
        /// <param name="idLectura"></param>
        public void DeleteMeMedicion96(int idPtomedicion, int idTipoInfo, DateTime fecha, int idLectura)
        {
            try
            {
                FactorySic.GetMeMedicion96Repository().Delete(idPtomedicion, idTipoInfo, fecha, idLectura);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region Metodos Cabecera

        /// <summary>
        /// Devuelve lista de cabeceras de formato
        /// </summary>
        /// <returns></returns>
        public List<MeCabeceraDTO> GetListMeCabecera()
        {
            return FactorySic.GetCabeceraRepository().List();
        }

        #endregion

        #region Métodos Tabla SI_EMPRESA
        /// <summary>
        /// Permite listar las empresas
        /// </summary>
        /// <returns></returns>
        public List<EmpresaDTO> ListarEmpresas()
        {
            return FactorySic.ObtenerEventoDao().ListarEmpresas();
        }

        /// <summary>
        /// Permite obtener las empresas con puntos de medicion
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarEmpresasPuntosMedicion()
        {
            return FactorySic.GetSiEmpresaRepository().ObtenerEmpresasPuntoMedicion();
        }

        public List<SiEmpresaDTO> ObtenerEmpresasPorUsuario(string userlogin)
        {
            return FactorySic.GetSiEmpresaRepository().GetByUser(userlogin);
        }

        /// <summary>
        /// Permite obtener las empresas por formato
        /// </summary>
        /// <param name="idFormato"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ObtenerEmpresaPorFormato(int idFormato)
        {
            return FactorySic.GetMeHojaptomedRepository().ObtenerEmpresasPorFormato(idFormato);
        }

        /// <summary>
        /// Devuelve entidad empresadto buscado por id
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public SiEmpresaDTO GetByIdSiEmpresa(int idEmpresa)
        {
            return FactorySic.GetSiEmpresaRepository().GetById(idEmpresa);
        }

        /// <summary>
        /// Devuelve lista de empresa por id formato
        /// </summary>
        /// <param name="idFormato"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> GetListaEmpresaFormato(int idFormato)
        {
                return FactorySic.GetSiEmpresaRepository().ObtenerEmpresaFormato(idFormato);
        }

        /// <summary>
        /// Devuelve lista de empresa por id formato
        /// </summary>
        /// <param name="idFormato"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> GetListaEmpresaFormatoMultiple(string idsFormatos)
        {
            return FactorySic.GetSiEmpresaRepository().ObtenerEmpresaFormatoMultiple(idsFormatos);
        }

        /// <summary>
        /// Devuelve lista de empresa por id formato
        /// </summary>
        /// <param name="idFormato"></param>
        /// <param name="IdEmpresa"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> GetListaEmpresaFormatoEmpresa(int idFormato, int IdEmpresa)
        {
            if (IdEmpresa == 1)
            {
                return FactorySic.GetSiEmpresaRepository().ObtenerEmpresaFormato(idFormato);
            }
            else
            {
                return FactorySic.GetSiEmpresaRepository().ObtenerEmpresaFormatoxUsuario(idFormato, IdEmpresa);
            }
        }


        /// <summary>
        /// Obtener la lista de empresas sin sus centrales solares
        /// </summary>
        /// <param name="formatcodi"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> GetListaEmpresaFormatoEnergiaPrimaria(int formatcodi)
        {
            var formato = GetByIdMeFormato(formatcodi);
            var cabecera = GetListMeCabecera().Find(x => x.Cabcodi == formato.Cabcodi);
            var listaHojaPto = GetListaPtos(DateTime.Now.Date, 0, -1, formatcodi, cabecera.Cabquery);

            listaHojaPto = listaHojaPto.Where(x => x.Famcodi != ConstantesHorasOperacion.IdTipoSolar).ToList();

            List<SiEmpresaDTO> lista = listaHojaPto.GroupBy(x => new { x.Emprcodi, x.Emprabrev, x.Emprnomb })
                .Select(x => new SiEmpresaDTO() { Emprcodi = x.Key.Emprcodi, Emprnomb = x.Key.Emprnomb, Emprabrev = x.Key.Emprabrev }).OrderBy(x => x.Emprnomb).ToList();

            return lista;
        }

        public List<SiEmpresaDTO> ListarEmpresasPorTipo(int iTipoEmpresa)
        {
            return FactorySic.GetSiEmpresaRepository().ListadoComboEmpresasPorTipo(iTipoEmpresa);
        }

        /// <summary>
        /// Obtener ListaEmpresaByOriglectcodi
        /// </summary>
        /// <param name="origlectcodi"></param>
        /// <param name="famcodi"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ObtenerListaEmpresaByOriglectcodi(int origlectcodi)
        {
            return FactorySic.GetSiEmpresaRepository().ListarEmpresaPorOrigenPtoMedicion(origlectcodi).Where(x => x.Emprcodi > 0).ToList();
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
        /// Listar envios con formatos de energia primaria
        /// </summary>
        /// <param name="envios"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idFormato"></param>
        /// <param name="idFormatoNuevo"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<MeEnvioDTO> GetByCriteriaMeEnviosFormatoEnergPrimaria(List<MeEnvioDTO> envios, int idEmpresa, int idFormato, int idFormatoNuevo, DateTime fecha)
        {
            if (idFormatoNuevo > 0 && idFormato != idFormatoNuevo)
            {
                var listaEnviosNuevo = this.GetByCriteriaMeEnvios(idEmpresa, idFormatoNuevo, fecha);
                listaEnviosNuevo.AddRange(envios);
                return listaEnviosNuevo;
            }

            return envios;
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

        /// <summary>
        /// Listar todos los envios por formato y fecha periodo
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="formatCodi"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<MeEnvioDTO> GetReporteTotalEnvioCumplimiento(string empresas, int formatCodi, DateTime fechaInicio, DateTime fechaFin)
        {
            return FactorySic.GetMeEnvioRepository().ObtenerReporteCumplimiento(empresas, formatCodi, fechaInicio, fechaFin);
        }
        #endregion

        #region Métodos Tabla SH_CAUDAL

        /// <summary>
        /// Inserta un registro de la tabla SH_CAUDAL
        /// </summary>
        public int SaveSHCaudal(SHCaudalDTO entity)
        {
            try
            {
                return FactorySic.GetSHCaudalRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Inserta un registro de la tabla SH_CAUDAL
        /// </summary>
        public int UpdateSHCaudal(SHCaudalDTO entity)
        {
            try
            {
                return FactorySic.GetSHCaudalRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Inserta un registro de la tabla SH_CAUDAL_DETALLE
        /// </summary>
        public int SaveSHCaudalDetalle(SHCaudalDetalleDTO entity)
        {
            try
            {
                return FactorySic.GetSHCaudalRepository().SaveDetalle(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Listar registros de la tabla SH_CAUDAL
        /// </summary>
        /// <param name="entity">Entidad</param>
        /// <returns></returns>
        public List<SHCaudalDTO> GetListaSHCaudal(SHCaudalDTO entity)
        {            
            try
            {
                return FactorySic.GetSHCaudalRepository().List(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene el caudal activo de la tabla SH_CAUDAL
        /// </summary>
        /// <param name="entity">Entidad</param>
        /// <returns></returns>
        public SHCaudalDTO GetCaudalActivo(SHCaudalDTO entity)
        {
            try
            {
                return FactorySic.GetSHCaudalRepository().GetCaudalActivo(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene el detalle de caudal de la tabla SH_CAUDAL_DETALLE
        /// </summary>
        /// <param name="entity">Entidad</param>
        /// <returns></returns>
        public List<SHCaudalDetalleDTO> ListDetalle(SHCaudalDTO entity)
        {
            try
            {
                return FactorySic.GetSHCaudalRepository().ListDetalle(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }


        #endregion

        #region Métodos Tabla ME_ENVIODET

        /// <summary>
        /// Inserta un registro de la tabla SI_ENVIODET
        /// </summary>
        public void SaveMeEnviodet(MeEnviodetDTO entity)
        {
            FactorySic.GetMeEnviodetRepository().Save(entity);
        }

        #endregion

        #region Métodos Tabla ME_CAMBIOENVIO

        /// <summary>
        /// Inserta un registro de la tabla ME_CAMBIOENVIO
        /// </summary>
        public void SaveMeCambioenvio(MeCambioenvioDTO entity)
        {
            try
            {
                FactorySic.GetMeCambioenvioRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_CAMBIOENVIO
        /// </summary>
        public List<MeCambioenvioDTO> ListMeCambioenvios(int idPto, int idTipoInfo, int idFormato, DateTime fecha)
        {
            return FactorySic.GetMeCambioenvioRepository().List(idPto, idTipoInfo, idFormato, fecha);
        }

        /// <summary>
        /// Lista todos los cambios realizados en un envio
        /// </summary>
        /// <param name="idFormato"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idEnvio"></param>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public List<MeCambioenvioDTO> GetAllCambioEnvio(int idFormato, DateTime fechaInicio, DateTime fechaFin, int idEnvio, int idEmpresa)
        {
            return FactorySic.GetMeCambioenvioRepository().GetAllCambioEnvio(idFormato, fechaInicio, fechaFin, idEnvio, idEmpresa);
        }
        /// <summary>
        /// Permite realizar búsquedas en la tabla MeCambioenvio
        /// </summary>
        public List<MeCambioenvioDTO> GetByCriteriaMeCambioenvios(string idsEmpresa, DateTime fecha, int idFormato, int idEnvio)
        {
            return FactorySic.GetMeCambioenvioRepository().GetByCriteria(idsEmpresa, fecha, idFormato);
        }

        /// <summary>
        /// Graba una lista de cambios 
        /// </summary>
        /// <param name="entitys"></param>
        public void GrabarCambios(List<MeCambioenvioDTO> entitys)
        {
            foreach (var entity in entitys)
                SaveMeCambioenvio(entity);
        }

        /// <summary>
        /// Obtiene todos los datos iniciales de fechas mayores al periodo seleccionado
        /// </summary>
        /// <param name="idFormato"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="fechaPeriodo"></param>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public List<MeCambioenvioDTO> GetAllOrigenEnvio(int idFormato, DateTime fechaInicio, DateTime fechaFin, DateTime fechaPeriodo, int idEmpresa)
        {
            return FactorySic.GetMeCambioenvioRepository().GetAllOrigenEnvio(idFormato, fechaInicio, fechaFin, fechaPeriodo, idEmpresa);
        }

        #endregion

        #region Métodos Tabla ME_MEDICIONXINTERVALO

        /// <summary>
        /// Graba una lista de mediciones
        /// </summary>
        /// <param name="lista"></param>
        public void GrabarMedicionesXIntevalo(List<MeMedicionxintervaloDTO> lista)
        {
            try
            {
                foreach (var reg in lista)
                {
                    this.SaveMeMedicionxintervalo(reg);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Inserta un registro de la tabla ME_MEDICIONXINTERVALO
        /// </summary>
        public void SaveMeMedicionxintervalo(MeMedicionxintervaloDTO entity)
        {
            try
            {
                FactorySic.GetMeMedicionxintervaloRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ME_MEDICIONXINTERVALO
        /// </summary>
        public void UpdateMeMedicionxintervalo(MeMedicionxintervaloDTO entity)
        {
            try
            {
                FactorySic.GetMeMedicionxintervaloRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_MEDICIONXINTERVALO
        /// </summary>
        public List<MeMedicionxintervaloDTO> ListMeMedicionxintervalos()
        {
            return FactorySic.GetMeMedicionxintervaloRepository().List();
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ME_MEDICIONXINTERVALO
        /// </summary>
        public MeMedicionxintervaloDTO GetByIdMeMedicionxintervalo(int ptoMedicodi, DateTime fechaIni)
        {
            return FactorySic.GetMeMedicionxintervaloRepository().GetById(ptoMedicodi, fechaIni);
        }

        /// <summary>
        /// Permite listar busquedas por rango de fecha, por empresa y tipo de formato de la tabla ME_MEDICIONXINTERVALO
        /// </summary>
        /// <param name="idFormato"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<MeMedicionxintervaloDTO> GetEnvioMedicionXIntervalo(int idFormato, int idEmpresa, DateTime fechaInicio, DateTime fechaFin)
        {
            return FactorySic.GetMeMedicionxintervaloRepository().GetEnvioArchivo(idFormato, idEmpresa, fechaInicio, fechaFin);
        }

        /// <summary>
        /// Permite eliminar los valores previos cargados
        /// </summary>
        /// <param name="horizonte"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        public void EliminarValoresCargados1XInter(int idLectura, int idFormato, int idEmpresa, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                FactorySic.GetMeMedicionxintervaloRepository().DeleteEnvioArchivo(idLectura, fechaInicio, fechaFin, idFormato, idEmpresa);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }




        /// <summary>
        /// Graba los datos enviados en el formato
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="usuario"></param>
        /// <param name="idEnvio"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="formato"></param>
        public void GrabarMedicionesXIntevalo(List<MeMedicionxintervaloDTO> entitys, string usuario, int idEnvio, int idEmpresa, MeFormatoDTO formato)
        {
            try
            {
                //Traer Ultimos Valores
                var lista = GetEnvioMedicionXIntervalo(formato.Formatcodi, idEmpresa, formato.FechaProceso, formato.FechaProceso);
                if (lista.Count > 0) // Verificar si hay cambios en el envio
                {
                    var listaCambio = new List<MeCambioenvioDTO>();
                    var listaOrigen = new List<MeCambioenvioDTO>();
                    foreach (var reg in entitys)
                    {
                        var regAnt = lista.Find(x => x.Medintfechaini == reg.Medintfechaini && x.Ptomedicodi == reg.Ptomedicodi &&
                    x.Tipoinfocodi == reg.Tipoinfocodi && x.Lectcodi == reg.Lectcodi && x.Medestcodi == reg.Medestcodi);
                        List<string> filaValores = new List<string>();
                        List<string> filaValoresOrigen = new List<string>();
                        List<string> filaCambios = new List<string>();
                        if (regAnt != null)
                        {
                            decimal? valorOrigen = regAnt.Medinth1; // (decimal?)regAnt.GetType().GetProperty("H" + i.ToString()).GetValue(regAnt, null);
                            decimal? valorModificado = reg.Medinth1; //(decimal?)reg.GetType().GetProperty("H" + i.ToString()).GetValue(reg, null);
                            if (valorModificado != null)
                                filaValores.Add(valorModificado.ToString());
                            else
                                filaValores.Add("");
                            if (valorOrigen != null)
                                filaValoresOrigen.Add(valorOrigen.ToString());
                            else
                                filaValoresOrigen.Add("");
                            if (valorOrigen != valorModificado)//&& valorOrigen != null && valorModificado != null)
                            {
                                filaCambios.Add("1");
                            }
                        }
                        if (filaCambios.Count > 0)
                        {
                            MeCambioenvioDTO cambio = new MeCambioenvioDTO();
                            cambio.Cambenvdatos = String.Join(",", filaValores);
                            cambio.Cambenvcolvar = String.Join(",", filaCambios);
                            cambio.Cambenvfecha = (DateTime)reg.Medintfechaini;
                            cambio.Enviocodi = idEnvio;
                            cambio.Formatcodi = formato.Formatcodi;
                            cambio.Ptomedicodi = reg.Ptomedicodi;
                            cambio.Tipoinfocodi = reg.Tipoinfocodi;
                            cambio.Lastuser = usuario;
                            cambio.Lastdate = DateTime.Now;
                            listaCambio.Add(cambio);
                            /// Si no ha habido cambio se graba el registro original
                            if (ListMeCambioenvios(reg.Ptomedicodi, reg.Tipoinfocodi, formato.Formatcodi, (DateTime)reg.Medintfechaini).Count == 0)
                            {
                                var listAux = GetByCriteriaRangoFecha(idEmpresa, formato.Formatcodi, formato.FechaInicio, formato.FechaProceso);
                                for (var fech = formato.FechaInicio; fech <= formato.FechaProceso; fech = fech.AddMonths(1))
                                {
                                    var listaMes = listAux.Where(x => x.Enviofechaperiodo == fech).ToList();
                                    int idEnvioMes = 0;
                                    if (listaMes.Count > 0)
                                    {
                                        idEnvioMes = listaMes.Min(x => x.Enviocodi);
                                        MeCambioenvioDTO origen = new MeCambioenvioDTO();
                                        origen.Cambenvdatos = String.Join(",", filaValoresOrigen);
                                        origen.Cambenvcolvar = "";
                                        origen.Cambenvfecha = (DateTime)reg.Medintfechaini;
                                        origen.Enviocodi = idEnvioMes;
                                        origen.Formatcodi = formato.Formatcodi;
                                        origen.Ptomedicodi = reg.Ptomedicodi;
                                        origen.Tipoinfocodi = reg.Tipoinfocodi;
                                        origen.Lastuser = usuario;
                                        origen.Lastdate = DateTime.Now;
                                        listaOrigen.Add(origen);
                                    }
                                }
                            }
                        }
                    }
                    if (listaCambio.Count > 0)
                    {//Grabar Cambios
                        GrabarCambios(listaCambio);
                        GrabarCambios(listaOrigen);
                        //si es primer reenvio grabar valores origen

                    }
                }
                //Eliminar Valores Previos
                EliminarValoresCargados1XInter(formato.Lectcodi, formato.Formatcodi, idEmpresa, formato.FechaProceso, formato.FechaProceso.AddDays(1).AddSeconds(-1));
                GrabarMedicionesXIntevalo(entitys);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Lista de Mediciones de Descarga y Vertimiento de lagunas
        /// </summary>
        /// <param name="formatCodi"></param>
        /// <param name="idsEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<MeMedicionxintervaloDTO> ListaMedIntervaloDescargaVert(int formatCodi, string idsEmpresa, DateTime fechaInicio,
            DateTime fechaFin)
        {
            return FactorySic.GetMeMedicionxintervaloRepository().GetHidrologiaDescargaVert(formatCodi, idsEmpresa, fechaInicio, fechaFin);
        }

        /// <summary>
        /// Lista de Mediciones de Descarga y Vertimiento de lagunas
        /// </summary>
        /// <param name="formatCodi"></param>
        /// <param name="idsEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<MeMedicionxintervaloDTO> ListaMedIntervaloDescargaVertPag(int formatCodi, string idsEmpresa, DateTime fechaInicio,
            DateTime fechaFin, int nroPagina, int pageSize)
        {
            return FactorySic.GetMeMedicionxintervaloRepository().GetHidrologiaDescargaVertPag(formatCodi, idsEmpresa, fechaInicio, fechaFin, nroPagina, pageSize);
        }

        /// <summary>
        /// Permite obtener la cantidad de registros con fechas distintas para la paginacion del reporte de descargas y vertimiento
        /// </summary>
        /// <returns></returns>
        public int ObtenerNroFilasDescargVert(int idFormato, string idsEmpresa, DateTime fechaInicio, DateTime fechaFin)
        {
            int cant;

            var lista1 = ListaMedIntervaloDescargaVert(idFormato, idsEmpresa, fechaInicio, fechaFin);
            cant = lista1.Count();
            return cant;
        }

        #endregion



        #region Métodos Tabla ME_MEDICION24

        /// <summary>
        /// Permite realizar búsquedas en la tabla MeMedicion24
        /// </summary>
        public List<MeMedicion24DTO> GetByCriteriaMeMedicion24s()
        {
            return FactorySic.GetMeMedicion24Repository().GetByCriteria();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla MeMedicion24
        /// </summary>
        public List<MeMedicion24DTO> GetByCriteriaMeMedicion24(DateTime fechaInicio, DateTime fechaFin, int lectcodi, int tipoinfocodi, int ptomedicodi)
        {
            return FactorySic.GetMeMedicion24Repository().GetByCriteria(fechaInicio, fechaFin, lectcodi, tipoinfocodi, ptomedicodi.ToString());
        }

        /// <summary>
        /// Permite eliminar los valores previos cargados
        /// </summary>
        /// <param name="horizonte"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        public void EliminarValoresCargados24(int idLectura, int idFormato, int idEmpresa, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                FactorySic.GetMeMedicion24Repository().DeleteEnvioArchivo(idLectura, fechaInicio, fechaFin, idFormato, idEmpresa);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite grabar los datos cargados
        /// </summary>
        /// <param name="entitys"></param>

        public void GrabarValoresCargados24(List<MeMedicion24DTO> entitys, string usuario, int idEnvio, int idEmpresa, MeFormatoDTO formato)
        {
            try
            {
                //Traer Ultimos Valores
                var lista = GetDataFormato24(idEmpresa, formato, 0, 0);
                if (lista.Count > 0) // Verificar si hay cambios en el envio
                {
                    var listaCambio = new List<MeCambioenvioDTO>();
                    var listaOrigen = new List<MeCambioenvioDTO>();
                    foreach (var reg in entitys)
                    {
                        var regAnt = lista.Find(x => x.Medifecha == reg.Medifecha && x.Ptomedicodi == reg.Ptomedicodi &&
                            x.Lectcodi == reg.Lectcodi);
                        List<string> filaValores = new List<string>();
                        List<string> filaValoresOrigen = new List<string>();
                        List<string> filaCambios = new List<string>();
                        if (regAnt != null)
                        {
                            for (int i = 1; i <= 24; i++)
                            {
                                decimal? valorOrigen = (decimal?)regAnt.GetType().GetProperty("H" + i.ToString()).GetValue(regAnt, null);
                                decimal? valorModificado = (decimal?)reg.GetType().GetProperty("H" + i.ToString()).GetValue(reg, null);
                                if (valorModificado != null)
                                    filaValores.Add(valorModificado.ToString());
                                else
                                    filaValores.Add("");
                                if (valorOrigen != null)
                                    filaValoresOrigen.Add(valorOrigen.ToString());
                                else
                                    filaValoresOrigen.Add("");
                                if (valorOrigen != valorModificado)// && valorOrigen != null && valorModificado != null)
                                {
                                    filaCambios.Add(i.ToString());
                                }
                            }
                        }
                        if (filaCambios.Count > 0)
                        {
                            MeCambioenvioDTO cambio = new MeCambioenvioDTO();
                            cambio.Cambenvdatos = String.Join(",", filaValores);
                            cambio.Cambenvcolvar = String.Join(",", filaCambios);
                            cambio.Cambenvfecha = (DateTime)reg.Medifecha;
                            cambio.Enviocodi = idEnvio;
                            cambio.Formatcodi = formato.Formatcodi;
                            cambio.Ptomedicodi = reg.Ptomedicodi;
                            cambio.Tipoinfocodi = reg.Tipoinfocodi;
                            cambio.Lastuser = usuario;
                            cambio.Lastdate = DateTime.Now;
                            listaCambio.Add(cambio);
                            /// Si no ha habido cambio se graba el registro original
                            if (ListMeCambioenvios(reg.Ptomedicodi, reg.Tipoinfocodi, formato.Formatcodi, (DateTime)reg.Medifecha).Count == 0)
                            {
                                int idEnvioPrevio = 0;
                                var listAux = GetByCriteriaMeEnvios(idEmpresa, formato.Formatcodi, formato.FechaProceso);
                                if (listAux.Count > 0)
                                    idEnvioPrevio = listAux.Min(x => x.Enviocodi);
                                MeCambioenvioDTO origen = new MeCambioenvioDTO();
                                origen.Cambenvdatos = String.Join(",", filaValoresOrigen);
                                origen.Cambenvcolvar = "";
                                origen.Cambenvfecha = (DateTime)reg.Medifecha;
                                origen.Enviocodi = idEnvioPrevio;
                                origen.Formatcodi = formato.Formatcodi;
                                origen.Ptomedicodi = reg.Ptomedicodi;
                                origen.Tipoinfocodi = reg.Tipoinfocodi;
                                origen.Lastuser = usuario;
                                origen.Lastdate = DateTime.Now;
                                listaOrigen.Add(origen);
                            }
                        }
                    }
                    if (listaCambio.Count > 0)
                    {//Grabar Cambios
                        GrabarCambios(listaCambio);
                        GrabarCambios(listaOrigen);
                        //si es primer reenvio grabar valores origen

                    }
                }
                //Eliminar Valores Previos
                EliminarValoresCargados24((int)formato.Lectcodi, formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin.AddDays(1));
                foreach (MeMedicion24DTO entity in entitys)
                {
                    FactorySic.GetMeMedicion24Repository().Save(entity);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Métodos Tabla ME_MEDICION48
        /// <summary>
        /// Permite realizar búsquedas en la tabla MeMedicion96
        /// </summary>
        public List<MeMedicion48DTO> GetByCriteriaMeMedicion48(DateTime fechaInicio, DateTime fechaFin, int lectcodi, int tipoinfocodi, int ptomedicodi)
        {
            return FactorySic.GetMeMedicion48Repository().GetByCriteria(fechaInicio, fechaFin, lectcodi.ToString(), tipoinfocodi, ptomedicodi.ToString());
        }
        #endregion

        #region Métodos Tabla ME_MEDICION96
        /// <summary>
        /// Permite realizar búsquedas en la tabla MeMedicion96
        /// </summary>
        public List<MeMedicion96DTO> GetByCriteriaMeMedicion96(DateTime fechaInicio, DateTime fechaFin, int lectcodi, int tipoinfocodi, int ptomedicodi)
        {
            return FactorySic.GetMeMedicion96Repository().GetByCriteria(fechaInicio, fechaFin, lectcodi, tipoinfocodi, ptomedicodi);
        }

        #region Modificacion PR15 - 24/11/2017
        /// <summary>
        /// 
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="fechaPeriodo"></param>
        /// <returns></returns>
        public List<ConsolidadoEnvioDTO> GetConsolidadoEnvioByEmpresaAndFormato96(int emprcodi, int lectcodi, int tipoinfocodi, DateTime fechaIni, DateTime fechaFin)
        {
            return FactorySic.GetMeMedicion96Repository().ConsolidadoEnvioByEmpresaAndFormato(emprcodi, lectcodi, tipoinfocodi, fechaIni, fechaFin);
        }
        #endregion
        #endregion

        #region Métodos Tabla ME_HOJA

        /// <summary>
        /// Graba entidad me_hoja
        /// </summary>
        /// <param name="hoja"></param>
        public int SaveMeHoja(MeHojaDTO hoja)
        {
            try
            {
                return FactorySic.GetMeHojaRepository().Save(hoja);

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener una lista de la tabla ME_HOJA
        /// </summary>
        public List<MeHojaDTO> ListMeHoja()
        {
            return FactorySic.GetMeHojaRepository().List();
        }

        /// <summary>
        /// Permite obtener una lista de la tabla ME_HOJA
        /// </summary>
        public List<MeHojaDTO> GetByCriteriaMeHoja(int formatcodi)
        {
            return FactorySic.GetMeHojaRepository().GetByCriteria(formatcodi).OrderBy(x => x.Hojaorden).ThenBy(x => x.Hojacodi).ToList();
        }

        /// <summary>
        /// Permite obtener una lista de hojas padres de un formato
        /// </summary>
        /// <param name="formatcodi"></param>
        /// <returns></returns>
        public List<MeHojaDTO> ListHojaPadre(int formatcodi)
        {
            return FactorySic.GetMeHojaRepository().ListPadre(formatcodi).OrderBy(x => x.Hojaorden).ThenBy(x => x.Hojacodi).ToList();
        }

        #endregion

        #region Métodos Tabla ME_TIPOPUNTOMEDICION
        /// <summary>
        /// Permite obtener una lista de la tabla ME_TIPOPUNTOMEDICION
        /// </summary>
        public MeTipopuntomedicionDTO GetByIdMeTipopuntomedicion(int tipoptomedicodi)
        {
            return FactorySic.GetMeTipopuntomedicionRepository().GetById(tipoptomedicodi);
        }
        #endregion

        #region Métodos Tabla ME_PTORELACION
        public List<int> ListPuntosRPF(int? idEmpresa, int? idCentral, DateTime fechaPeriodo)
        {
            List<int> puntosRpf = new List<int>();
            if (idEmpresa == null) idEmpresa = -1;
            if (idCentral == null) idCentral = -1;

            List<MePtorelacionDTO> list = FactorySic.GetMePtorelacionRepository().ObtenerPuntosRelacion(idEmpresa, idCentral, fechaPeriodo);

            puntosRpf = list.Where(x => x.Origlectcodi == 1).Select(x => (int)x.Ptomedicodi).Distinct().ToList();

            return puntosRpf;
        }

        /// <summary>
        /// Listar puntos de medición segun los equipos generadores
        /// </summary>
        /// <param name="equicodis"></param>
        /// <param name="fechaPeriodo"></param>
        /// <returns></returns>
        public List<MePtorelacionDTO> ListPuntosRPFByEquipo(List<int> listaEquicodi, DateTime fechaPeriodo)
        {
            if (listaEquicodi.Any())
            {
                List<MePtorelacionDTO> list = FactorySic.GetMePtorelacionRepository().ObtenerPuntosRelacion(-1, -1, fechaPeriodo);

                return list.Where(x => x.Origlectcodi == 1 && listaEquicodi.Contains(x.Equicodi)).ToList();
            }

            return new List<MePtorelacionDTO>();
        }

        #endregion
        //fin agregado

        #region Métodos Tabla ME_HOJAPTOMED

        ///// <summary>
        /// Inserta un registro de la tabla ME_HOJAPTOMED
        /// </summary>
        public void SaveMeHojaptomed(MeHojaptomedDTO entity, int empresa)
        {
            try
            {
                FactorySic.GetMeHojaptomedRepository().Save(entity, empresa);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Graba un punto de medicion en el formato y devuelve el registro grabado
        /// </summary>
        /// <param name="entity"></param>
        public MeHojaptomedDTO GrabarHojaPtoMedicion(MeHojaptomedDTO entity, int empresa)
        {
            SaveMeHojaptomed(entity, empresa);
            return GetByIdMeHojaptomed(entity.Hojacodi, entity.Formatcodi, entity.Tipoinfocodi, entity.Ptomedicodi, entity.Hojaptosigno, entity.Tptomedicodi, entity.Hojacodi);
        }

        /// <summary>
        /// Actualiza un registro de la tabla ME_HOJAPTOMED
        /// </summary>
        public void UpdateMeHojaptomed(MeHojaptomedDTO entity)
        {
            try
            {
                FactorySic.GetMeHojaptomedRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualizar el orden de la lista de puntos de medición
        /// </summary>
        /// <param name="lista"></param>
        public void UpdateListaMeHojaptomedByOrder(List<MeHojaptomedDTO> lista)
        {
            List<MeHojaptomedDTO> listaToUpdate = new List<MeHojaptomedDTO>();

            lista = lista.OrderBy(x => x.Hojaptoorden).ThenBy(x => x.Emprnomb).ThenBy(x => x.Equipopadre).ThenBy(x => x.Equinomb).ThenBy(x => x.Tipoinfocodi).ThenBy(x => x.Tipoptomedinomb).ToList();

            int orden = 1;
            foreach (var reg in lista)
            {
                if (reg.Hojaptoorden != orden)
                {
                    reg.Hojaptoorden = orden;
                    listaToUpdate.Add(reg);
                }
                orden++;
            }

            //Actualizar puntos de medición desordenados
            foreach (var reg in listaToUpdate)
            {
                this.UpdateMeHojaptomed(reg);
            }

            lista = lista.OrderBy(x => x.Hojaptoorden).ToList();
        }
        
        /// <summary>
        /// Eliminar un registro de la tabla ME_HOJAPTOMED
        /// </summary>
        public void DeleteMeHojaptomedById(int id)
        {
            try
            {
                FactorySic.GetMeHojaptomedRepository().DeleteById(id);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla ME_HOJAPTOMED
        /// </summary>        
        public void DeleteMeHojaptomed(int idEmpresa, int formatcodi, int tipoInfo, int idOrden, int ptomedicodi, int tptomedi, int hoja)
        {
            try
            {
                if (hoja == -1)
                {
                    FactorySic.GetMeHojaptomedRepository().Delete(formatcodi, tipoInfo, idOrden, ptomedicodi, tptomedi);
                    var lista = GetByCriteriaMeHojaptomeds(idEmpresa, formatcodi, DateTime.Now.Date, DateTime.Now.Date).OrderBy(x => x.Hojaptoorden);
                    int i = 1;
                    foreach (var reg in lista)
                    {
                        reg.Hojaptoorden = i;
                        i++;
                        UpdateMeHojaptomed(reg);
                    }
                }
                else
                {
                    //Eliminamos los puntos del medicion de hojaptomed
                    FactorySic.GetMeHojaptomedRepository().DeleteHoja(formatcodi, tipoInfo, idOrden, ptomedicodi, tptomedi, hoja);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ME_HOJAPTOMED
        /// </summary>
        public MeHojaptomedDTO GetByIdMeHojaptomed(int hojanumero, int formatcodi, int tipoinfocodi, int ptomedicodi, int hojaptosigno, int tptomedicodi, int hojacodi)
        {
            if (hojacodi == -1)
            {
                return FactorySic.GetMeHojaptomedRepository().GetById(formatcodi, tipoinfocodi, ptomedicodi, hojaptosigno, tptomedicodi);
            }
            else
            {
                return FactorySic.GetMeHojaptomedRepository().GetByIdHoja(formatcodi, tipoinfocodi, ptomedicodi, hojaptosigno, tptomedicodi, hojacodi);
            }
        }

        /// <summary>
        /// Permite listar los puntos de medicion la tabla MeHojaptomed
        /// </summary>
        public List<MeHojaptomedDTO> GetByCriteriaMeHojaptomeds(int emprcodi, int formatcodi, DateTime fechaIni, DateTime fechaFin)
        {
            return FactorySic.GetMeHojaptomedRepository().GetByCriteria(emprcodi, formatcodi, fechaIni, fechaFin);
        }

        /// <summary>
        /// Permite listar los puntos de medicion la tabla MeHojaptomed por formato
        /// </summary>
        public List<MeHojaptomedDTO> GetListaHojaptomed(int emprcodi, int formatcodi, int hojacodi)
        {
            if (hojacodi == -1)
            {
                return FactorySic.GetMeHojaptomedRepository().ListarHojaPtoByFormatoAndEmpresa(emprcodi, formatcodi.ToString());
            }
            else
            {
                return FactorySic.GetMeHojaptomedRepository().ListarHojaPtoByFormatoAndEmpresaHoja(emprcodi, formatcodi, hojacodi);
            }
        }

        /// <summary>
        /// Permite listar los puntos de medicion la tabla MeHojaptomed
        /// </summary>
        public List<MeHojaptomedDTO> GetByCriteria2MeHojaptomeds(int emprcodi, int formatcodi, string query, DateTime fechaIni, DateTime fechaFin)
        {
            return FactorySic.GetMeHojaptomedRepository().GetByCriteria2(emprcodi, formatcodi, query, fechaIni, fechaFin);
        }

        /// <summary>
        /// Utilizado en Stock de combustibles
        /// </summary>
        /// <param name="listaData"></param>
        /// <param name="idCfgFormato"></param>
        /// <param name="hoja"></param>
        /// <returns></returns>
        public List<MeHojaptomedDTO> GetListaPtosByHoja(List<MeHojaptomedDTO> listaData, int idCfgFormato, int hoja)
        {
            var lista = new List<MeHojaptomedDTO>();
            if (idCfgFormato == 0)
            {
                lista = listaData.Where(x => x.Hojaptoactivo == 1).ToList();
            }
            else
            {
                var config = GetByIdMeConfigformatenvio(idCfgFormato);
                int posHoja = config.Cfgenvhojas.Split(',').Select(s => Convert.ToInt32(s)).ToList().FindIndex(x => x == hoja);

                if (posHoja != -1)
                {
                    lista = listaData;
                    string strCfgenvptos = config.Cfgenvptos.Split('|')[posHoja].Trim();
                    string strCfgenvtipoinf = config.Cfgenvtipoinf.Split('|')[posHoja].Trim();
                    string strCfgenvorden = config.Cfgenvorden.Split('|')[posHoja].Trim();

                    if (strCfgenvptos != string.Empty && strCfgenvtipoinf != string.Empty && strCfgenvorden != string.Empty)
                    {
                        var ptos = strCfgenvptos.Split(',').Select(s => Convert.ToInt32(s)).ToList();
                        var tipoinfos = strCfgenvtipoinf.Split(',').Select(s => Convert.ToInt32(s)).ToList();
                        var orden = strCfgenvorden.Split(',').Select(s => Convert.ToInt32(s)).ToList();

                        lista = lista.Where(x => ptos.Contains(x.Ptomedicodi)).ToList();
                        for (var i = 0; i < ptos.Count; i++)
                        {
                            var find = lista.Find(x => x.Ptomedicodi == ptos[i] && x.Tipoinfocodi == tipoinfos[i]);
                            if (find != null)
                            {
                                find.Hojaptoorden = orden[i];
                            }
                        }
                    }
                    lista = lista.OrderBy(x => x.Hojaptoorden).ToList();
                }
            }

            return lista;
        }

        /// <summary>
        /// Utilizado en Stock de combustibles
        /// </summary>
        /// <param name="listaData"></param>
        /// <param name="idCfgFormato"></param>
        /// <returns></returns>
        public List<MeHojaptomedDTO> GetListaPtosByFormato(List<MeHojaptomedDTO> listaData, int idCfgFormato)
        {
            var lista = new List<MeHojaptomedDTO>();
            if (idCfgFormato == 0)
            {
                lista = listaData.Where(x => x.Hojaptoactivo == 1).ToList();
            }
            else
            {
                var config = GetByIdMeConfigformatenvio(idCfgFormato);
                var ptos = config.Cfgenvptos.Split(',').Select(s => Convert.ToInt32(s)).ToList();
                var tipoinfos = config.Cfgenvtipoinf.Split(',').Select(s => Convert.ToInt32(s)).ToList();
                var orden = config.Cfgenvorden.Split(',').Select(s => Convert.ToInt32(s)).ToList();
                lista = listaData.Where(x => ptos.Contains(x.Ptomedicodi)).ToList();
                for (var i = 0; i < ptos.Count; i++)
                {
                    var find = lista.Find(x => x.Ptomedicodi == ptos[i] && x.Tipoinfocodi == tipoinfos[i]);
                    if (find != null)
                    {
                        find.Hojaptoorden = orden[i];
                    }
                }
                lista = lista.OrderBy(x => x.Hojaptoorden).ToList();
            }

            return lista;
        }

        /// <summary>
        /// Obtiene lista de ptos de medicion del formato de acuerdo al envio indicado, toma en cuenta
        /// los ptos de medicion que habian en el momento del envio
        /// </summary>
        /// <param name="idEnvio"></param>idEnvio == 0 para nuevo envio.
        /// <param name="emprcodi"></param>
        /// <param name="formatcodi"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<MeHojaptomedDTO> GetListaPtos(DateTime fechaPeriodo, int idCfgFormato, int emprcodi, int formatcodi, string query)
        {
            var lista = GetByCriteria2MeHojaptomeds(emprcodi, formatcodi, query, fechaPeriodo, fechaPeriodo);
            if (idCfgFormato == 0)
            {
                lista = lista.Where(x => x.Hojaptoactivo == 1).ToList();
            }
            else
            {
                var config = GetByIdMeConfigformatenvio(idCfgFormato);

                if (config.Cfgenvhojas.Contains(','))
                {
                    List<int> listaHojacodi = config.Cfgenvhojas.Split(',').Select(s => Convert.ToInt32(s)).ToList();
                    List<MeHojaptomedDTO> listTemp = new List<MeHojaptomedDTO>();

                    for (int posHoja = 0; posHoja < listaHojacodi.Count(); posHoja++)
                    {
                        int hojacodi = listaHojacodi[posHoja];
                        string strCfgenvptos = config.Cfgenvptos.Split('|')[posHoja].Trim();
                        string strCfgenvtipoinf = config.Cfgenvtipoinf.Split('|')[posHoja].Trim();
                        string strCfgenvtipopto = config.Cfgenvtipopto.Length > 0 ? config.Cfgenvtipopto.Split('|')[posHoja].Trim() : string.Empty;
                        string strCfgenvorden = config.Cfgenvorden.Split('|')[posHoja].Trim();

                        if (strCfgenvptos != string.Empty && strCfgenvtipoinf != string.Empty && strCfgenvorden != string.Empty)
                        {
                            List<int> ptos = strCfgenvptos.Split(',').Select(s => Convert.ToInt32(s)).ToList();
                            List<int> tipoinfos = strCfgenvtipoinf.Split(',').Select(s => Convert.ToInt32(s)).ToList();
                            List<int> tipoptos = strCfgenvtipopto.Length > 0 ? strCfgenvtipopto.Split(',').Select(s => Convert.ToInt32(s)).ToList() : new List<int>();
                            List<int> orden = strCfgenvorden.Split(',').Select(s => Convert.ToInt32(s)).ToList();

                            var listaXHoja = lista.Where(x => ptos.Contains(x.Ptomedicodi) && x.Hojacodi == hojacodi).ToList();
                            for (var i = 0; i < ptos.Count; i++)
                            {
                                if (tipoptos.Count == 0) //la hoja no tiene tptomedicodis
                                {
                                    var find = listaXHoja.Find(x => x.Ptomedicodi == ptos[i] && x.Tipoinfocodi == tipoinfos[i]);
                                    if (find != null)
                                    {
                                        find.Hojaptoorden = orden[i];
                                    }
                                }
                                else
                                {
                                    var find = listaXHoja.Find(x => x.Ptomedicodi == ptos[i] && x.Tipoinfocodi == tipoinfos[i] && x.Tptomedicodi == tipoptos[i]);
                                    if (find != null)
                                    {
                                        find.Hojaptoorden = orden[i];
                                        listTemp.Add(find);
                                    }
                                }
                            }
                        }
                    }

                    if (listTemp.Count > 0) //existen configuraciones que guardan tptomedicodi
                    {
                        lista = listTemp;
                    }

                }
                else
                {
                    List<int> ptos = config.Cfgenvptos.Replace("|", ",").Split(',').Select(s => Convert.ToInt32(s)).ToList();
                    List<int> tipoinfos = config.Cfgenvtipoinf.Replace("|", ",").Split(',').Select(s => Convert.ToInt32(s)).ToList();
                    List<int> orden = config.Cfgenvorden.Replace("|", ",").Split(',').Select(s => Convert.ToInt32(s)).ToList();
                    lista = lista.Where(x => ptos.Contains(x.Ptomedicodi)).ToList();
                    for (var i = 0; i < ptos.Count; i++)
                    {
                        var find = lista.Find(x => x.Ptomedicodi == ptos[i] && x.Tipoinfocodi == tipoinfos[i]);
                        if (find != null)
                        {
                            find.Hojaptoorden = orden[i];
                        }
                    }
                }

                lista = lista.OrderBy(x => x.Hojaptoorden).ToList();
            }

            //adecuación para Interconexiones
            if (ConstantesInterconexiones.IdFormatoInterconexion == formatcodi)
            {
                foreach (var pto in lista)
                {
                    if (pto.Ptomedibarranomb != null
                        && (ConstantesInterconexiones.IdTipoPtomedicodiExportacionMwh == pto.Tptomedicodi
                        || ConstantesInterconexiones.IdTipoPtomedicodiImportacionMwh == pto.Tptomedicodi
                        || ConstantesInterconexiones.IdTipoPtomedicodiExportacionMVarh == pto.Tptomedicodi
                    || ConstantesInterconexiones.IdTipoPtomedicodiImportacionMVarh == pto.Tptomedicodi))
                    {
                        pto.Ptomedibarranomb = pto.Tipoptomedinomb.Trim().Substring(0, 11);
                    }

                    if (pto.Ptomedibarranomb != null
                        && (ConstantesInterconexiones.IdTipoInfocodiKV == pto.Tipoinfocodi))
                    {
                        pto.Ptomedibarranomb = "Tensión";
                    }

                    if (pto.Ptomedibarranomb != null
                        && (ConstantesInterconexiones.IdTipoInfocodiA == pto.Tipoinfocodi))
                    {
                        pto.Ptomedibarranomb = "Amperios";
                    }
                }
            }

            return lista;
        }

        /// <summary>
        /// Listar puntos de medicion para Energia Primaria
        /// </summary>
        /// <param name="listaHojaPto"></param>
        /// <param name="idEnvio"></param>
        /// <param name="idCfgFormato"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idFormato"></param>
        /// <param name="idFormatoNuevo"></param>
        /// <returns></returns>
        public List<MeHojaptomedDTO> GetListaPtosFormatoEnergPrimaria(List<MeHojaptomedDTO> listaHojaPto, int idEnvio, int idCfgFormato, int idEmpresa, int idFormato, int idFormatoNuevo)
        {
            if (ConstantesHard.IdFormatoEnergiaPrimaria == idFormato)
            {
                if (idFormatoNuevo > 0)
                {
                    if (idEnvio <= 0 || listaHojaPto.Count == 0) //Cuando no envio antes de la Fecha de inicio del nuevo formato
                    {
                        var formatoNuevo = GetByIdMeFormato(ConstantesHard.IdFormatoEnergiaPrimariaSolar);
                        var cabeceraNuevo = GetListMeCabecera().Find(x => x.Cabcodi == formatoNuevo.Cabcodi);
                        listaHojaPto = GetListaPtos(DateTime.Now.Date, idCfgFormato, idEmpresa, formatoNuevo.Formatcodi, cabeceraNuevo.Cabquery);
                    }

                    listaHojaPto = listaHojaPto.Where(x => x.Famcodi == ConstantesHorasOperacion.IdTipoSolar).ToList();
                }
                else
                {
                    listaHojaPto = listaHojaPto.Where(x => x.Famcodi != ConstantesHorasOperacion.IdTipoSolar).ToList();
                }
            }

            return listaHojaPto;
        }

        /// <summary>
        /// Agregar columnas adicionales
        /// </summary>
        /// <param name="fechaProceso"></param>
        /// <param name="listaHojaPto"></param>
        /// <returns></returns>
        public List<MeHojaptomedDTO> ListaHojaptoCheck(DateTime fechaProceso, List<MeHojaptomedDTO> listaHojaPto)
        {
            if (fechaProceso == DateTime.Today)
            {
                List<MeHojaptomedDTO> listaHojaPtoTmp = new List<MeHojaptomedDTO>();
                foreach (var regPto in listaHojaPto)
                {
                    regPto.TieneCheckExtranet = regPto.Hptoindcheck == ConstantesAppServicio.SI;
                    if (regPto.Hptoindcheck == ConstantesAppServicio.SI)
                    {
                        var regPto2 = (MeHojaptomedDTO)regPto.Clone();
                        regPto2.Hptoindcheck = null;

                        listaHojaPtoTmp.Add(regPto2);
                    }
                    listaHojaPtoTmp.Add(regPto);
                }
                return listaHojaPtoTmp;
            }
            else
            {
                foreach (var regPto in listaHojaPto)
                {
                    regPto.Hptoindcheck = null;
                }

                return listaHojaPto;
            }
        }

        /// <summary>
        /// Permite obtener la hora por el ID
        /// </summary>
        /// <param name="hojacodi"></param>
        /// <returns></returns>
        public MeHojaDTO GetByIdMeHoja(int hojacodi)
        {
            return FactorySic.GetMeHojaRepository().GetById(hojacodi);
        }

        public List<MeHojaptomedDTO> ObtenerPtosXFormato(int formatcodi, int emprcodi)
        {
            return FactorySic.GetMeHojaptomedRepository().ObtenerPtosXFormato(formatcodi, emprcodi);
        }

        /// <summary>
        /// Obtiene lista de puntos por formato y empresa
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="formatcodi"></param>
        /// <returns></returns>
        public List<MeHojaptomedDTO> GetPuntosFormato(int emprcodi, int formatcodi)
        {
            return FactorySic.GetMeHojaptomedRepository().GetPuntosFormato(emprcodi, formatcodi);
        }

        #endregion

        #region Métodos Tabla ME_CONFIGFORMATENVIO

        /// <summary>
        /// Graba Configuracion de formato envio
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int GrabarConfigFormatEnvio(MeConfigformatenvioDTO entity)
        {
            int idCfgEnvio = 0;
            var lista = GetByCriteriaMeHojaptomeds((int)entity.Emprcodi, (int)entity.Formatcodi, entity.FechaInicio, entity.FechaInicio);
            if (ConstantesStockCombustibles.IdFormatoConsumo == entity.Formatcodi)
            {
                lista = lista.OrderBy(x => x.Equipadre).ThenByDescending(x => x.Tptomedicodi).ThenBy(x => x.Equinomb).ToList();
            }
            lista = lista.Where(x => x.Hojaptoactivo == 1).ToList();

            var listaHojacodi = lista.Select(x => x.Hojacodi).Distinct().ToList();

            if (lista.Count > 0)
            {
                string strCfgenvptos = string.Empty;
                string strCfgenvorden = string.Empty;
                string strCfgenvtipoinf = string.Empty;
                string strCfgenvtipopto = string.Empty;
                string strCfgenvhojas = string.Empty;

                if (listaHojacodi.Count > 0)
                {
                    List<string> listaCfgenvptos = new List<string>();
                    List<string> listaCfgenvorden = new List<string>();
                    List<string> listaCfgenvtipoinf = new List<string>();
                    List<string> listaCfgenvtipopto = new List<string>();
                    List<string> listaCfgenvhojas = new List<string>();

                    foreach (var hojacodi in listaHojacodi)
                    {
                        listaCfgenvptos.Add(string.Join(",", lista.Where(x => x.Hojacodi == hojacodi).Select(x => x.Ptomedicodi).ToList()));
                        listaCfgenvorden.Add(entity.Cfgenvorden = string.Join(",", lista.Where(x => x.Hojacodi == hojacodi).Select(x => x.Hojaptoorden).ToList()));
                        listaCfgenvtipoinf.Add(entity.Cfgenvtipoinf = string.Join(",", lista.Where(x => x.Hojacodi == hojacodi).Select(x => x.Tipoinfocodi).ToList()));
                        listaCfgenvtipopto.Add(entity.Cfgenvtipopto = string.Join(",", lista.Where(x => x.Hojacodi == hojacodi).Select(x => x.Tptomedicodi).ToList()));
                        listaCfgenvhojas.Add(hojacodi + string.Empty);
                    }

                    strCfgenvptos = string.Join("|", listaCfgenvptos);
                    strCfgenvorden = string.Join("|", listaCfgenvorden);
                    strCfgenvtipoinf = string.Join("|", listaCfgenvtipoinf);
                    strCfgenvtipopto = string.Join("|", listaCfgenvtipopto);
                    strCfgenvhojas = string.Join(",", listaCfgenvhojas);
                }
                else
                {
                    strCfgenvptos = string.Join(",", lista.Select(x => x.Ptomedicodi).ToList());
                    strCfgenvorden = string.Join(",", lista.Select(x => x.Hojaptoorden).ToList());
                    strCfgenvtipoinf = string.Join(",", lista.Select(x => x.Tipoinfocodi).ToList());
                    strCfgenvtipopto = string.Join(",", lista.Select(x => x.Tptomedicodi).ToList());
                }

                entity.Cfgenvptos = strCfgenvptos;
                entity.Cfgenvorden = strCfgenvorden;
                entity.Cfgenvtipoinf = strCfgenvtipoinf;
                entity.Cfgenvtipopto = strCfgenvtipopto;
                entity.Cfgenvhojas = strCfgenvhojas;

                idCfgEnvio = VerificaFormatoUpdate((int)entity.Emprcodi, (int)entity.Formatcodi, entity.Cfgenvptos, entity.Cfgenvorden, entity.Cfgenvtipoinf, entity.Cfgenvtipopto, entity.Cfgenvhojas);
                if (idCfgEnvio == 0)
                {
                    entity.Cfgenvfecha = DateTime.Now;
                    idCfgEnvio = SaveMeConfigformatenvio(entity);
                }
            }

            return idCfgEnvio;
        }

        /// <summary>
        /// Inserta un registro de la tabla ME_CONFIGFORMATENVIO
        /// </summary>
        public int SaveMeConfigformatenvio(MeConfigformatenvioDTO entity)
        {
            int idCfgEnvio = 0;
            try
            {
                idCfgEnvio = FactorySic.GetMeConfigformatenvioRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return idCfgEnvio;
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ME_CONFIGFORMATENVIO
        /// </summary>
        public MeConfigformatenvioDTO GetByIdMeConfigformatenvio(int idCfgenv)
        {
            return FactorySic.GetMeConfigformatenvioRepository().GetById(idCfgenv);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla MeConfigformatenvio
        /// </summary>
        public List<MeConfigformatenvioDTO> GetByCriteriaMeConfigformatenvios(int idEmpresa, int idFormato)
        {
            return FactorySic.GetMeConfigformatenvioRepository().GetByCriteria(idEmpresa, idFormato);
        }

        public int VerificaFormatoUpdate(int idEmpresa, int idFormato, string listaPtos, string listaOrden, string listaTipoinf, string listaTipopto, string listaHoja)
        {
            int idCfg = 0;
            var entity = GetByCriteriaMeConfigformatenvios(idEmpresa, idFormato).FirstOrDefault();
            if (entity != null)
            {
                string ptos = entity.Cfgenvptos == null ? string.Empty : entity.Cfgenvptos.Trim();
                string orden = entity.Cfgenvorden == null ? string.Empty : entity.Cfgenvorden.Trim();
                string tipoinf = entity.Cfgenvtipoinf == null ? string.Empty : entity.Cfgenvtipoinf.Trim();
                string tipopto = entity.Cfgenvtipopto == null ? string.Empty : entity.Cfgenvtipopto.Trim();
                string hojas = entity.Cfgenvhojas == null ? string.Empty : entity.Cfgenvhojas.Trim();

                if (listaPtos == ptos && listaOrden == orden && tipoinf == listaTipoinf && hojas == listaHoja && tipopto == listaTipopto)
                {
                    idCfg = entity.Cfgenvcodi;
                }
            }

            return idCfg;
        }

        #endregion

        #region Métodos Tabla ME_FORMATO

        /// <summary>
        /// Inserta un registro de la tabla ME_FORMATO
        /// </summary>
        public int SaveMeFormato(MeFormatoDTO entity)
        {
            int id = 0;
            try
            {
                id = FactorySic.GetMeFormatoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return id;
        }

        /// <summary>
        /// Actualiza un registro de la tabla ME_FORMATO
        /// </summary>
        public void UpdateMeFormato(MeFormatoDTO entity)
        {
            try
            {
                FactorySic.GetMeFormatoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ME_FORMATO
        /// </summary>
        public MeFormatoDTO GetByIdMeFormato(int formatcodi)
        {
            var formato = FactorySic.GetMeFormatoRepository().GetById(formatcodi);
            return formato;
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ME_FORMATO
        /// </summary>
        public MeFormatoDTO GetByIdMeFormatoDetalle(int formatcodi, int idEmpresa, DateTime fechaPeriodo)
        {
            var formato = FactorySic.GetMeFormatoRepository().GetById(formatcodi);
            formato.ListaHoja = FactorySic.GetMeHojaRepository().GetByCriteria(formatcodi).OrderBy(x => x.Hojacodi).ToList();
            foreach (var reg in formato.ListaHoja)
            {
                reg.Cabecera = GetListMeCabecera().Where(x => x.Cabcodi == reg.Cabcodi).FirstOrDefault();
                reg.ListaPtos = GetByCriteria2MeHojaptomeds(idEmpresa, formatcodi, reg.Cabecera.Cabquery, fechaPeriodo, fechaPeriodo).OrderBy(x => x.Equipadre).ThenByDescending(x => x.Tptomedicodi).ThenBy(x => x.Equinomb).ToList();
            }
            return formato;
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_FORMATO
        /// </summary>
        public List<MeFormatoDTO> ListMeFormatos()
        {
            return FactorySic.GetMeFormatoRepository().List();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_FORMATO
        /// </summary>
        public List<MeFormatoDTO> ListMeFormatosOrigen()
        {
            return FactorySic.GetMeFormatoRepository().ListarFormatoOrigen();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla MeFormato
        /// </summary>
        public List<MeFormatoDTO> GetByCriteriaMeFormatos(int areaUsuario, int formatcodiOrigen)
        {
            var lista = FactorySic.GetMeFormatoRepository().GetByCriteria(areaUsuario, formatcodiOrigen);

            foreach (var reg in lista)
                reg.LastdateDesc = reg.Lastdate != null ? reg.Lastdate.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : string.Empty;

            return lista;
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla MeFormato
        /// </summary>
        public List<MeFormatoDTO> GetByModuloLecturaMeFormatos(int idModulo, int idLectura, int idEmpresa)
        {
            return FactorySic.GetMeFormatoRepository().GetByModuloLectura(idModulo, idLectura, idEmpresa);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla MeFormato para multiples lecturas y empresas
        /// </summary>
        public List<MeFormatoDTO> GetByModuloLecturaMeFormatosMultiple(int idModulo, string lectura, string empresa)
        {
            return FactorySic.GetMeFormatoRepository().GetByModuloLecturaMultiple(idModulo, lectura, empresa);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla MeFormato
        /// </summary>
        public List<MeFormatoDTO> GetByModuloMeFormatos(int idModulo)
        {
            return FactorySic.GetMeFormatoRepository().GetByModulo(idModulo);
        }

        #endregion

        #region Métodos Tabla ME_AMPLIACIONFECHA

        /// <summary>
        /// Inserta un registro de la tabla ME_AMPLIACIONFECHA
        /// </summary>
        public void SaveMeAmpliacionfecha(MeAmpliacionfechaDTO entity)
        {
            try
            {
                FactorySic.GetMeAmpliacionfechaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ME_AMPLIACIONFECHA
        /// </summary>
        public void UpdateMeAmpliacionfecha(MeAmpliacionfechaDTO entity)
        {
            try
            {
                FactorySic.GetMeAmpliacionfechaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ME_AMPLIACIONFECHA
        /// </summary>
        public MeAmpliacionfechaDTO GetByIdMeAmpliacionfecha(DateTime fecha, int empresa, int formato)
        {
            return FactorySic.GetMeAmpliacionfechaRepository().GetById(fecha, empresa, formato);
        }

        /// <summary>
        /// devuelve lista de ampliacion de fechas para listado simple 
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="empresa"></param>
        /// <param name="formato"></param>
        /// <returns></returns>
        public List<MeAmpliacionfechaDTO> ObtenerListaMeAmpliacionfechas(DateTime fechaIni, DateTime fechaFin, int empresa, int formato)
        {

            return FactorySic.GetMeAmpliacionfechaRepository().GetListaAmpliacion(fechaIni, fechaFin, empresa, formato);
        }

        /// <summary>
        /// Devuelve lista de ampliacion de fechas para listado multiple
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="sEmpresa"></param>
        /// <param name="sFormato"></param>
        /// <returns></returns>
        public List<MeAmpliacionfechaDTO> ObtenerListaMultipleMeAmpliacionfechas(DateTime fechaIni, DateTime fechaFin, string sEmpresa, string sFormato)
        {
            if (string.IsNullOrEmpty(sEmpresa)) sEmpresa = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(sFormato)) sFormato = ConstantesAppServicio.ParametroDefecto;
            return FactorySic.GetMeAmpliacionfechaRepository().GetListaMultiple(fechaIni, fechaFin, sEmpresa, sFormato);
        }

        #endregion

        #region Métodos Tabla ME_VALIDACION

        /// <summary>
        /// Inserta un registro de la tabla ME_VALIDACION
        /// </summary>
        public void SaveMeValidacion(MeValidacionDTO entity)
        {
            try
            {
                FactorySic.GetMeValidacionRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ME_VALIDACION
        /// </summary>
        public void UpdateMeValidacion(MeValidacionDTO entity)
        {
            try
            {
                FactorySic.GetMeValidacionRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ME_VALIDACION
        /// </summary>
        public MeValidacionDTO GetByIdMeValidacion(int formatcodi, int emprcodi, DateTime validfechaperiodo)
        {
            return FactorySic.GetMeValidacionRepository().GetById(formatcodi, emprcodi, validfechaperiodo);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_VALIDACION
        /// </summary>
        public List<MeValidacionDTO> GetByCriteriaMeValidacions(int formato, int emprcodi)
        {
            return FactorySic.GetMeValidacionRepository().GetByCriteria(formato, emprcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_VALIDACION
        /// </summary>
        public List<MeValidacionDTO> ListMeValidacions(DateTime fecha, int formato)
        {
            List<MeValidacionDTO> l = FactorySic.GetMeValidacionRepository().List(fecha, formato);

            l = l.GroupBy(x => new { x.Emprcodi, x.Emprnomb, x.Validestado })
                .Select(x => new MeValidacionDTO() { Emprcodi = x.Key.Emprcodi, Emprnomb = x.Key.Emprnomb, Validestado = x.Key.Validestado }).ToList();

            return l;
        }

        /// <summary>
        /// Actualiza un registro de la tabla ME_VALIDACION
        /// </summary>
        public void ValidarEnvioEmpresas(DateTime fecha, int formatcodi, string usuario, string empresas, int estado)
        {
            try
            {
                FactorySic.GetMeValidacionRepository().ValidarEmpresa(fecha, formatcodi, usuario, empresas, estado);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        //inicio agregado
        /// <summary>
        /// Graba Validación de envio
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void GrabarMeValidacion(MeValidacionDTO validacion, MeFormatoDTO formato)
        {
            var findValicion = this.GetByIdMeValidacion(formato.Formatcodi, validacion.Emprcodi, formato.FechaProceso);
            if (findValicion != null)
            {
                this.UpdateMeValidacion(validacion);
            }
            else
            {
                this.SaveMeValidacion(validacion);
            }
        }

        #endregion

        #region Métodos Tabla ME_VERIFICACION
        /// <summary>
        /// Inserta un registro de la tabla ME_VERIFICACION
        /// </summary>
        public void SaveMeVerificacion(MeVerificacionDTO entity)
        {
            try
            {
                FactorySic.GetMeVerificacionRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ME_VERIFICACION
        /// </summary>
        public void UpdateMeVerificacion(MeVerificacionDTO entity)
        {
            try
            {
                FactorySic.GetMeVerificacionRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla ME_VERIFICACION
        /// </summary>
        public void DeleteMeVerificacion(int verifcodi)
        {
            try
            {
                FactorySic.GetMeVerificacionRepository().Delete(verifcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ME_VERIFICACION
        /// </summary>
        public MeVerificacionDTO GetByIdMeVerificacion(int verifcodi)
        {
            return FactorySic.GetMeVerificacionRepository().GetById(verifcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_VERIFICACION
        /// </summary>
        public List<MeVerificacionDTO> ListMeVerificacions()
        {
            return FactorySic.GetMeVerificacionRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla MeVerificacion
        /// </summary>
        public List<MeVerificacionDTO> GetByCriteriaMeVerificacions()
        {
            return FactorySic.GetMeVerificacionRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla ME_VERIFICACION_FORMATO

        /// <summary>
        /// Inserta un registro de la tabla ME_VERIFICACION_FORMATO
        /// </summary>
        public void SaveMeVerificacionFormato(MeVerificacionFormatoDTO entity)
        {
            try
            {
                FactorySic.GetMeVerificacionFormatoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ME_VERIFICACION_FORMATO
        /// </summary>
        public void UpdateMeVerificacionFormato(MeVerificacionFormatoDTO entity)
        {
            try
            {
                FactorySic.GetMeVerificacionFormatoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla ME_VERIFICACION_FORMATO
        /// </summary>
        public void DeleteMeVerificacionFormato(int formatcodi, int verifcodi)
        {
            try
            {
                FactorySic.GetMeVerificacionFormatoRepository().Delete(formatcodi, verifcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ME_VERIFICACION_FORMATO
        /// </summary>
        public MeVerificacionFormatoDTO GetByIdMeVerificacionFormato(int formatcodi, int verifcodi)
        {
            return FactorySic.GetMeVerificacionFormatoRepository().GetById(formatcodi, verifcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_VERIFICACION_FORMATO
        /// </summary>
        public List<MeVerificacionFormatoDTO> ListMeVerificacionFormatos()
        {
            return FactorySic.GetMeVerificacionFormatoRepository().List();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_VERIFICACION_FORMATO
        /// </summary>
        public List<MeVerificacionFormatoDTO> ListMeVerificacionFormatosByFormato(int formatcodi)
        {
            return FactorySic.GetMeVerificacionFormatoRepository().ListByFormato(formatcodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla MeVerificacionFormato
        /// </summary>
        public List<MeVerificacionFormatoDTO> GetByCriteriaMeVerificacionFormatos()
        {
            return FactorySic.GetMeVerificacionFormatoRepository().GetByCriteria();
        }

        #endregion

        #region Util
        /// <summary>
        /// Se obtiene los datos enviados de un formato de la BD.
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="formato"></param>
        /// <param name="idEnvio"></param>
        /// <returns></returns>
        public List<MeMedicion24DTO> GetDataFormato24(int idEmpresa, MeFormatoDTO formato, int idEnvio, int idUltimoEnvio)
        {

            List<MeMedicion24DTO> lista24 = FactorySic.GetMeMedicion24Repository().GetEnvioArchivo(formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin);
            if (idEnvio != 0)
            {
                var lista = GetAllCambioEnvio(formato.Formatcodi, formato.FechaInicio, formato.FechaFin, idEnvio, idEmpresa);
                if (lista.Count > 0)
                {
                    foreach (var reg in lista)
                    {
                        var find = lista24.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Tipoinfocodi == reg.Tipoinfocodi &&
                            x.Medifecha == reg.Cambenvfecha);
                        if (find != null)
                        {
                            var fila = reg.Cambenvdatos.Split(',');
                            for (var i = 0; i < 24; i++)
                            {
                                decimal dato;
                                decimal? numero = null;
                                if (decimal.TryParse(fila[i], out dato))
                                    numero = dato;
                                find.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(find, numero);
                            }
                        }
                    }
                }
            }
            else
            {
                if (formato.Formatsecundario != 0 && lista24.Count == 0)
                {
                    lista24 = FactorySic.GetMeMedicion24Repository().GetDataFormatoSecundario(formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin);
                }
            }
            return lista24;
        }

        /// <summary>
        /// Se obtiene los datos enviados de un formato de la BD.
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="formato"></param>
        /// <param name="idEnvio"></param>
        /// <returns></returns>
        public List<MeMedicion1DTO> GetDataFormato1(int idEmpresa, MeFormatoDTO formato, int idEnvio, int idUltimoEnvio)
        {
            List<MeMedicion1DTO> lista1 = null;

                lista1 = FactorySic.GetMeMedicion1Repository().GetEnvioArchivo(formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin);
            
            if (idEnvio != 0)
            {
                var lista = GetAllCambioEnvio(formato.Formatcodi, formato.FechaInicio, formato.FechaFin, idEnvio, idEmpresa);
                if (lista.Count > 0)
                {
                    foreach (var reg in lista)
                    {
                        var find = lista1.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Tipoinfocodi == reg.Tipoinfocodi &&
                            x.Tipoptomedicodi == reg.Tipoptomedicodi && x.Medifecha == reg.Cambenvfecha);
                        if (find != null)
                        {
                            var fila = reg.Cambenvdatos != null ? reg.Cambenvdatos.Split(',') : new string[1] { string.Empty };
                            for (var i = 0; i < 1; i++)
                            {
                                decimal dato;
                                decimal? numero = null;
                                if (decimal.TryParse(fila[i], out dato))
                                    numero = dato;
                                find.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(find, numero);
                            }
                        }
                    }
                }
            }
            else
            {
                if (formato.Formatsecundario != 0 && lista1.Count == 0)
                {
                    lista1 = FactorySic.GetMeMedicion1Repository().GetDataFormatoSecundario(formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin);
                }
            }
            return lista1;
        }

        /// <summary>
        /// Se obtiene los datos enviados de un formato de la BD de me_medicon48.
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="formato"></param>
        /// <param name="idEnvio"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> GetDataFormato48(int idEmpresa, MeFormatoDTO formato, int idEnvio, int idUltimoEnvio)
        {
            List<MeMedicion48DTO> lista48 = new List<MeMedicion48DTO>();
            if (!formato.FlagUtilizaHoja)
            {
                lista48 = FactorySic.GetMeMedicion48Repository().GetEnvioArchivo(formato.Formatcodi, idEmpresa.ToString(), formato.FechaInicio, formato.FechaFin);
            }
            else
            {
                if (ConstantesIEOD.IdFormatoDemandaDiaria == formato.Formatcodi)
                {
                    List<MeMedicion48DTO> lista48Tmp = FactorySic.GetMeMedicion48Repository().GetEnvioArchivo2(formato.Formatcodi, idEmpresa.ToString(), formato.FechaProceso.Date.AddDays(-1), formato.FechaProceso.Date.AddDays(1));
                    foreach (var hoja in formato.ListaHoja)
                    {
                        if (ConstantesIEOD.LectCodiDemandaDiariaProgramado == hoja.Lectcodi)
                        {
                            DateTime fecha = formato.FechaProceso.Date.AddDays(1);
                            lista48.AddRange(lista48Tmp.Where(x => x.Medifecha == fecha && x.Hojacodi == hoja.Hojacodi).ToList());
                        }
                        else
                        {
                            DateTime fecha = formato.FechaProceso.Date.AddDays(-1);
                            lista48.AddRange(lista48Tmp.Where(x => x.Medifecha == fecha && x.Hojacodi == hoja.Hojacodi).ToList());
                        }
                    }
                }
                else
                {
                    lista48 = FactorySic.GetMeMedicion48Repository().GetEnvioArchivo2(formato.Formatcodi, idEmpresa.ToString(), formato.FechaInicio, formato.FechaFin);
                }
            }

            if (idEnvio != 0)
            {
                List<MeCambioenvioDTO> lista = new List<MeCambioenvioDTO>();
                if (ConstantesIEOD.IdFormatoDemandaDiaria == formato.Formatcodi)
                {
                    List<MeCambioenvioDTO> listaTmp = this.GetAllCambioEnvio(formato.Formatcodi, formato.FechaProceso.Date.AddDays(-1), formato.FechaProceso.Date.AddDays(1), idEnvio, idEmpresa);

                    foreach (var hoja in formato.ListaHoja)
                    {
                        if (ConstantesIEOD.LectCodiDemandaDiariaProgramado == hoja.Lectcodi)
                        {
                            DateTime fecha = formato.FechaProceso.Date.AddDays(1);
                            lista.AddRange(listaTmp.Where(x => x.Cambenvfecha == fecha && x.Hojacodi == hoja.Hojacodi).ToList());
                        }
                        else
                        {
                            DateTime fecha = formato.FechaProceso.Date.AddDays(-1);
                            lista.AddRange(listaTmp.Where(x => x.Cambenvfecha == fecha && x.Hojacodi == hoja.Hojacodi).ToList());
                        }
                    }
                }
                else
                {
                    lista = this.GetAllCambioEnvio(formato.Formatcodi, formato.FechaInicio, formato.FechaFin, idEnvio, idEmpresa);
                    if (formato.IdFormatoNuevo > 0 && formato.Formatcodi != formato.IdFormatoNuevo)
                    {
                        lista = this.GetAllCambioEnvio(formato.IdFormatoNuevo, formato.FechaInicio, formato.FechaFin, idEnvio, idEmpresa);
                    }
                }

                if (lista.Count > 0)
                {
                    foreach (var reg in lista)
                    {
                        MeMedicion48DTO find = !formato.FlagUtilizaHoja
                            ? lista48.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Tipoinfocodi == reg.Tipoinfocodi && x.Medifecha == reg.Cambenvfecha)
                            : lista48.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Tipoinfocodi == reg.Tipoinfocodi && x.Medifecha == reg.Cambenvfecha && x.Hojacodi == reg.Hojacodi);
                        if (find != null)
                        {
                            var fila = reg.Cambenvdatos.Split(',');
                            for (var i = 0; i < 48; i++)
                            {
                                decimal dato;
                                decimal? numero = null;
                                if (decimal.TryParse(fila[i], out dato))
                                    numero = dato;
                                find.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(find, numero);
                            }
                        }
                    }
                }
            }

            return lista48;
        }

        /// <summary>
        /// Se obtiene los datos enviados de un formato de la BD de me_medicon48.
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="formato"></param>
        /// <param name="idEnvio"></param>
        /// <returns></returns>
        public List<MeMedicion96DTO> GetDataFormato96(List<MeMedicion96DTO> listaOld, int idEmpresa, MeFormatoDTO formato, int idEnvio, int idUltimoEnvio)
        {
            List<MeMedicion96DTO> lista96 = listaOld.Count > 0 ? listaOld : FactorySic.GetMeMedicion96Repository().GetEnvioArchivo(formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin);
            if (idEnvio != 0)
            {
                var lista = GetAllCambioEnvio(formato.Formatcodi, formato.FechaInicio, formato.FechaFin, idEnvio, idEmpresa);
                if (lista.Count > 0)
                {
                    foreach (var reg in lista)
                    {
                        var find = lista96.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Tipoinfocodi == reg.Tipoinfocodi &&
                             x.Tipoptomedicodi == reg.Tipoptomedicodi && x.Medifecha == reg.Cambenvfecha);
                        if (find != null)
                        {
                            var fila = reg.Cambenvdatos.Split(',');
                            for (var i = 0; i < 96; i++)
                            {
                                decimal dato;
                                decimal? numero = null;
                                if (decimal.TryParse(fila[i], out dato))
                                    numero = dato;
                                find.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(find, numero);
                            }
                        }
                    }
                }
            }
            else
            {
                if (formato.Formatsecundario != 0 && lista96.Count == 0)
                {
                    //lista48 = FactorySic.GetMeMedicion48Repository().GetDataFormatoSecundario(formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin);
                }
            }
            return lista96;
        }


        #endregion

        #region Métodos Tabla ME_ESTADOENVIO

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_ESTADOENVIO
        /// </summary>
        public List<MeEstadoenvioDTO> ListMeEstadoenvios()
        {
            return FactorySic.GetMeEstadoenvioRepository().List();
        }

        #endregion

        #region Carga de Datos

        //Métodos para EnvioController
        /// <summary>
        /// Verifica si un formato enviado esta en plazo o fuyera de plazo
        /// </summary>
        /// <param name="formato"></param>
        /// <returns></returns>
        public bool ValidarPlazoController(MeFormatoDTO formato)
        {
            bool resultado = false;
            DateTime fechaActual = DateTime.Now;
            if ((fechaActual >= formato.FechaPlazoIni) && (fechaActual <= formato.FechaPlazo))
            {
                resultado = true;
            }
            return resultado;
        }

        /// <summary>
        /// Valida la fecha
        /// </summary>
        /// <param name="formato"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="horaini"></param>
        /// <param name="horafin"></param>
        /// <returns></returns>
        public bool ValidarFecha(MeFormatoDTO formato, int idEmpresa, out int horaini, out int horafin)
        {
            bool resultado = false;
            DateTime fechaActual = DateTime.Now;
            horaini = 0;
            horafin = 0;
            if ((fechaActual >= formato.FechaPlazoIni) && (fechaActual <= formato.FechaPlazo))
            {
                resultado = true;
            }
            else
            {
                var regfechaPlazo = this.GetByIdMeAmpliacionfecha(formato.FechaProceso, idEmpresa, formato.Formatcodi);
                if (regfechaPlazo != null) // si existe registro de ampliacion
                {

                    if ((fechaActual >= formato.FechaPlazoIni) && (fechaActual <= regfechaPlazo.Amplifechaplazo))
                    {
                        resultado = true;
                    }
                }
            }
            if ((formato.Formatdiaplazo == 0) && (resultado)) //Formato Tiempo Real
            {
                int hora = fechaActual.Hour;
                if (((hora - 1) % 3) == 0)
                {
                    horaini = hora - 1 - 1 * 3;
                    horafin = hora - 1;
                }
                else
                {
                    horafin = -1;//indica que formato tiempo real no tiene filas habilitadas
                    resultado = false;
                }
            }
            return true;
            //return resultado;
        }

        /// <summary>
        /// Obtiene el valor del formato seg{un su configuración
        /// </summary>
        /// <param name="idHorizonte"></param>
        /// <param name="formatoReal"></param>
        /// <returns></returns>
        public int ObtenerIdFormatoPadre(int formatoReal)
        {
            FormatoMedicionAppServicio logic = new FormatoMedicionAppServicio();

            var formato = logic.GetByIdMeFormato(formatoReal);
            int idFormato = formato.Formatdependeconfigptos != null ? formato.Formatdependeconfigptos.Value : formato.Formatcodi;

            return idFormato;
        }

        public int ObtenerIdFormato(int idHorizonte, int formato, ref int formatoReal)
        {
            var formatDiario = 0;
            var formatSemanal = 0;
            int IdFormato = 0;
            FormatoMedicionAppServicio logic = new FormatoMedicionAppServicio();
            switch (formato)
            {
                case ConstantesDemandaCP.FormatoDiarioCodi:
                    formatDiario = ConstantesDemandaCP.FormatoDiarioCodi;
                    formatSemanal = ConstantesDemandaCP.FormatoSemanalCodi;
                    break;
                case ConstantesHidrologiaCD.FormatoDiarioCodi:
                    formatDiario = ConstantesHidrologiaCD.FormatoDiarioCodi;
                    formatSemanal = ConstantesHidrologiaCD.FormatoSemanalCodi;
                    break;
                case ConstantesMedicionesCD.FormatoDiarioCodi:
                    formatDiario = ConstantesMedicionesCD.FormatoDiarioCodi;
                    formatSemanal = ConstantesMedicionesCD.FormatoSemanalCodi;
                    break;
                default:
                    break;
            }
            switch (idHorizonte)
            {
                case 0:
                    IdFormato = formatDiario;
                    formatoReal = formatDiario;
                    break;
                case 1:
                    var Formato = logic.GetByIdMeFormato(formatSemanal);
                    IdFormato = Formato.Formatdependeconfigptos != null ? (int)Formato.Formatdependeconfigptos : formatDiario;
                    formatoReal = formatSemanal;
                    break;
            }
            return IdFormato;
        }

        public void GenerarFileExcel(FormatoModel model, string ruta, int formatoCodi)
        {
            string nombrePlantilla = string.Empty;
            switch (formatoCodi)
            {
                case ConstantesDemandaCP.FormatoDiarioCodi:
                    nombrePlantilla = NombreArchivoDemandaCP.PlantillaDemandaCP;
                    break;
                case ConstantesHidrologiaCD.FormatoDiarioCodi:
                    nombrePlantilla = NombreArchivoHidrologiaCD.PlantillaHidrologiaCD;
                    break;
                case ConstantesHidrologiaCD.FormatoVolumenDIarioCodi:
                    nombrePlantilla = NombreArchivoHidrologiaCD.PlantillaHidrologiaCD;
                    break;
                default:
                    break;
            }
            //string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteDemandaBarras].ToString();
            string fileTemplate = nombrePlantilla;
            FileInfo template = new FileInfo(ruta + fileTemplate);
            //FileInfo newFile = new FileInfo(ruta + NombreArchivo.FormatoProgDiario);
            FileInfo newFile = new FileInfo(ruta + model.Formato.Formatnombre + ".xlsx");
            if (newFile.Exists)
            {
                newFile.Delete();
                //newFile = new FileInfo(ruta + NombreArchivo.FormatoProgDiario);
                newFile = new FileInfo(ruta + model.Formato.Formatnombre + ".xlsx");
            }
            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[ConstantesFormat.HojaFormatoExcel];
                //Escribe  Nombre Area
                ws.Cells[ParamFormato.RowArea, 1].Value = model.Formato.Areaname;
                ws.Cells[ParamFormato.RowTitulo, 1].Value = "";
                ws.Cells[ParamFormato.RowTitulo, 1].Value = model.Formato.Formatnombre;
                //Imprimimos Codigo Empresa y Codigo Formato
                ws.Cells[1, ParamFormato.ColEmpresa].Value = model.IdEmpresa.ToString();
                ws.Cells[1, ParamFormato.ColFormato].Value = model.Formato.Formatcodi.ToString();

                ///Descripcion del Formato
                /// Nombre de la Empresa
                int row = 10;
                int column = 2;
                ws.Cells[row, column].Value = model.Empresa;
                ws.Cells[row + 1, column].Value = model.Anho.ToString();
                switch (model.Formato.Formatperiodo)
                {
                    case ParametrosFormato.PeriodoDiario:
                        ws.Cells[row + 2, column - 1].Value = "Mes";
                        ws.Cells[row + 2, column].Value = model.Mes;
                        ws.Cells[row + 3, column - 1].Value = "Día";
                        ws.Cells[row + 3, column].Value = model.Dia.ToString();
                        row = row + 3;
                        if (model.ListaHojaPto[0].Famcodi == 43)
                        {
                            ws.Cells[row + 4, column - 1].Value = "Caudal";
                            ws.Cells[row + 4, column].Value = "m3/s";
                            row++;
                        }
                        break;
                    case ParametrosFormato.PeriodoSemanal:
                        ws.Cells[row + 2, column - 1].Value = "Semana";
                        var semanaLength = model.Semana.Length;
                        ws.Cells[row + 2, column].Value = model.Semana.Substring(4, semanaLength - 4);
                        row = row + 2;
                        if (model.ListaHojaPto[0].Famcodi == 43)
                        {
                            ws.Cells[row + 3, column - 1].Value = "Caudal";
                            ws.Cells[row + 3, column].Value = "m3/s";
                            row++;
                        }
                        break;
                    case 3:
                    case 5:
                        ws.Cells[row + 2, column - 1].Value = "Mes";
                        ws.Cells[row + 2, column].Value = model.Mes;
                        row += 2;
                        if (model.ListaHojaPto[0].Famcodi == 43)
                        {
                            ws.Cells[row + 3, column - 1].Value = "Caudal";
                            ws.Cells[row + 3, column].Value = "m3/s";
                            row++;
                        }
                        break;

                }

                ///Imprimimos cabecera de puntos de medicion
                row = ParamFormato.RowDatos;
                int totColumnas = model.ListaHojaPto.Count;
                int columnIni = ParamFormato.ColDatos;

                if (formatoCodi == ConstantesHidrologiaCD.FormatoVolumenDIarioCodi)
                {
                    totColumnas = 3;//columnas min. max, inicial
                    for (var i = 0; i < model.Handson.ListaExcelData.Count(); i++)
                    {
                        for (var j = 0; j <= totColumnas; j++)
                        {
                            decimal valor = 0;
                            bool canConvert = decimal.TryParse(model.Handson.ListaExcelData[i][j], out valor);
                            if (canConvert)
                                ws.Cells[row + i, j + 1].Value = valor;
                            else
                                ws.Cells[row + i, j + 1].Value = model.Handson.ListaExcelData[i][j];
                            ws.Cells[row + i, j + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            if (i < model.Formato.Formatrows && j >= model.Formato.Formatcols)
                            {
                                ws.Cells[row + i, j + 1].Style.Font.Color.SetColor(System.Drawing.Color.White);
                                ws.Cells[row + i, j + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                                ws.Cells[row + i, j + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                ws.Cells[row + i, j + 1].Style.WrapText = true;
                            }
                        }
                    }

                    /////////////////Formato a Celdas Head ///////////////////
                    using (var range = ws.Cells[row, 2, row + model.Formato.Formatrows - 1, totColumnas + 1])
                    {
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(Color.SteelBlue);
                    }
                    ////////////// Formato de Celdas Valores
                    using (var range = ws.Cells[row + model.Formato.Formatrows, 2, row + model.Handson.ListaExcelData.Count() + 1, totColumnas + 1])
                    {
                        range.Style.Numberformat.Format = @"0.000";
                    }
                }
                else
                {
                    for (var i = 0; i <= model.ListaHojaPto.Count; i++)
                    {
                        for (var j = 0; j < model.Formato.Formathorizonte * model.Formato.RowPorDia + model.Formato.Formatrows; j++)
                        {
                            decimal valor = 0;
                            bool canConvert = decimal.TryParse(model.Handson.ListaExcelData[j][i], out valor);
                            if (canConvert)
                                ws.Cells[row + j, i + 1].Value = valor;
                            else
                                ws.Cells[row + j, i + 1].Value = model.Handson.ListaExcelData[j][i];
                            ws.Cells[row + j, i + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            if (j < model.Formato.Formatrows && i >= model.Formato.Formatcols)
                            {
                                ws.Cells[row + j, i + 1].Style.Font.Color.SetColor(System.Drawing.Color.White);
                                ws.Cells[row + j, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                                ws.Cells[row + j, i + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                ws.Cells[row + j, i + 1].Style.WrapText = true;
                            }
                        }
                    }

                    /////////////////Formato a Celdas Head ///////////////////
                    using (var range = ws.Cells[row, 2, row + model.Formato.Formatrows - 1, model.ListaHojaPto.Count + 1])
                    {
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(Color.SteelBlue);
                    }
                    ////////////// Formato de Celdas Valores
                    using (var range = ws.Cells[row + model.Formato.Formatrows, 2, row + model.Formato.Formathorizonte * model.Formato.RowPorDia + model.Formato.Formatrows - 1, model.ListaHojaPto.Count + 1])
                    {
                        range.Style.Numberformat.Format = @"0.000";
                    }

                }
                /////////////////////// Celdas Merge /////////////////////

                foreach (var reg in model.Handson.ListaMerge)
                {
                    int fili = row + reg.row;
                    int filf = row + reg.row + reg.rowspan - 1;
                    int coli = reg.col + 1;
                    int colf = reg.col + reg.colspan - 1 + 1;
                    ws.Cells[fili, coli, filf, colf].Merge = true;
                }

                xlPackage.Save();
            }

        }

        public void GenerarFileExcelSH(FormatoModel model, MePtomedicionDTO ptomedicionDTO, string ruta)
        {
            string nombrePlantilla = string.Empty;
            nombrePlantilla = NombreArchivoHidrologiaCD.PlantillaSeriesHidrologicasCD;
            string fileTemplate = nombrePlantilla;
            FileInfo template = new FileInfo(ruta + fileTemplate);
            FileInfo newFile = new FileInfo(ruta + model.Formato.Formatnombre + ".xlsx");
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + model.Formato.Formatnombre + ".xlsx");
            }
            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[ConstantesFormat.HojaFormatoExcel];
                //Escribe  Nombre Area
                ws.Cells[ParamFormatoSH.RowTitulo, 4].Value = "";
                ws.Cells[ParamFormatoSH.RowTitulo, 4].Value = ptomedicionDTO.Tipoptomedinomb + " DEL " + ptomedicionDTO.Ptomedidesc;
                ws.Cells[ParamFormatoSH.RowCuenca, 2].Value = ptomedicionDTO.EquiPadrenomb;
                ws.Cells[ParamFormatoSH.RowEmpresa, 2].Value = ptomedicionDTO.Emprnomb;
                ws.Cells[ParamFormatoSH.RowCuenca, 10].Value = ptomedicionDTO.Equinomb;
                if (ptomedicionDTO.Tipoptomedinomb == "CAUDAL NATURAL")
                {
                    ws.Cells[ParamFormatoSH.RowEmbalseRio, 1].Value = "RIO";
                }
                else
                {
                    ws.Cells[ParamFormatoSH.RowEmbalseRio, 1].Value = "EMBALSE";
                    ws.Cells[ParamFormatoSH.RowCuenca, 8].Value = "";
                    ws.Cells[ParamFormatoSH.RowCuenca, 10].Value = "";
                }
                ws.Cells[ParamFormatoSH.RowEmbalseRio, 2].Value = ptomedicionDTO.Ptomedidesc;
                ws.Cells[ParamFormatoSH.RowPuntoMedicion, 2].Value = ptomedicionDTO.Ptomedibarranomb;                
                ws.Cells[ParamFormatoSH.RowEmpresa, 10].Value = ptomedicionDTO.CoordenadaX;
                ws.Cells[ParamFormatoSH.RowEmbalseRio, 10].Value = ptomedicionDTO.CoordenadaY;
                ws.Cells[ParamFormatoSH.RowPuntoMedicion, 10].Value = ptomedicionDTO.Altitud;

                var fontTablaCabecera = ws.Cells[1, 1, 9, 13].Style.Font;
                fontTablaCabecera.Size = 11;
                fontTablaCabecera.Name = "Calibri";

                //int row = 10;
                //int column = 2;

                int anioInicio = ConstantesHidrologiaCD.AnioInicioSH;
                int anioActual = DateTime.Now.Year;
                int rowInicio = ParamFormatoSH.RowAnio;

                int numInicioMatriz = 1;
                for (int anio = anioInicio; anio < anioActual; anio++)
                {
                    ws.Cells[rowInicio, 1].Value = Convert.ToInt16(anio.ToString());
                    ws.Cells[rowInicio, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[rowInicio, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    var border = ws.Cells[rowInicio, 1, rowInicio, 13].Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    var fontTabla = ws.Cells[rowInicio, 1, rowInicio, 13].Style.Font;
                    fontTabla.Size = 11;
                    fontTabla.Name = "Calibri";

                    if (!string.IsNullOrEmpty((model.Handson.ListaExcelData[numInicioMatriz][1]))) {
                        ws.Cells[rowInicio, 2].Value = Convert.ToDecimal(model.Handson.ListaExcelData[numInicioMatriz][1].ToString());
                    } else
                    {
                        ws.Cells[rowInicio, 2].Value = string.Empty;
                    }
                    if (!string.IsNullOrEmpty((model.Handson.ListaExcelData[numInicioMatriz][2]))) 
                    {
                        ws.Cells[rowInicio, 3].Value = Convert.ToDecimal(model.Handson.ListaExcelData[numInicioMatriz][2].ToString());
                    }
                    else
                    {
                        ws.Cells[rowInicio, 3].Value = string.Empty;
                    }
                    if (!string.IsNullOrEmpty((model.Handson.ListaExcelData[numInicioMatriz][3])))
                    {
                        ws.Cells[rowInicio, 4].Value = Convert.ToDecimal(model.Handson.ListaExcelData[numInicioMatriz][3].ToString());
                    }
                    else
                    {
                        ws.Cells[rowInicio, 4].Value = string.Empty;
                    }
                    if (!string.IsNullOrEmpty((model.Handson.ListaExcelData[numInicioMatriz][4])))
                    {
                        ws.Cells[rowInicio, 5].Value = Convert.ToDecimal(model.Handson.ListaExcelData[numInicioMatriz][4].ToString());
                    }
                    else
                    {
                        ws.Cells[rowInicio, 5].Value = string.Empty;
                    }
                    if (!string.IsNullOrEmpty((model.Handson.ListaExcelData[numInicioMatriz][5])))
                    {
                        ws.Cells[rowInicio, 6].Value = Convert.ToDecimal(model.Handson.ListaExcelData[numInicioMatriz][5].ToString());
                    }
                    else
                    {
                        ws.Cells[rowInicio, 6].Value = string.Empty;
                    }
                    if (!string.IsNullOrEmpty((model.Handson.ListaExcelData[numInicioMatriz][6])))
                    {
                        ws.Cells[rowInicio, 7].Value = Convert.ToDecimal(model.Handson.ListaExcelData[numInicioMatriz][6].ToString());
                    }
                    else
                    {
                        ws.Cells[rowInicio, 7].Value = string.Empty;
                    }
                    if (!string.IsNullOrEmpty((model.Handson.ListaExcelData[numInicioMatriz][7])))
                    {
                        ws.Cells[rowInicio, 8].Value = Convert.ToDecimal(model.Handson.ListaExcelData[numInicioMatriz][7].ToString());
                    }
                    else
                    {
                        ws.Cells[rowInicio, 8].Value = string.Empty;
                    }
                    if (!string.IsNullOrEmpty((model.Handson.ListaExcelData[numInicioMatriz][8])))
                    {
                        ws.Cells[rowInicio, 9].Value = Convert.ToDecimal(model.Handson.ListaExcelData[numInicioMatriz][8].ToString());
                    }
                    else
                    {
                        ws.Cells[rowInicio, 9].Value = string.Empty;
                    }
                    if (!string.IsNullOrEmpty((model.Handson.ListaExcelData[numInicioMatriz][9])))
                    {
                        ws.Cells[rowInicio, 10].Value = Convert.ToDecimal(model.Handson.ListaExcelData[numInicioMatriz][9].ToString());
                    }
                    else
                    {
                        ws.Cells[rowInicio, 10].Value = string.Empty;
                    }
                    if (!string.IsNullOrEmpty((model.Handson.ListaExcelData[numInicioMatriz][10])))
                    {
                        ws.Cells[rowInicio, 11].Value = Convert.ToDecimal(model.Handson.ListaExcelData[numInicioMatriz][10].ToString());
                    }
                    else
                    {
                        ws.Cells[rowInicio, 11].Value = string.Empty;
                    }
                    if (!string.IsNullOrEmpty((model.Handson.ListaExcelData[numInicioMatriz][11])))
                    {
                        ws.Cells[rowInicio, 12].Value = Convert.ToDecimal(model.Handson.ListaExcelData[numInicioMatriz][11].ToString());
                    }
                    else
                    {
                        ws.Cells[rowInicio, 12].Value = string.Empty;
                    }
                    if (!string.IsNullOrEmpty((model.Handson.ListaExcelData[numInicioMatriz][12])))
                    {
                        ws.Cells[rowInicio, 13].Value = Convert.ToDecimal(model.Handson.ListaExcelData[numInicioMatriz][12].ToString());
                    }
                    else
                    {
                        ws.Cells[rowInicio, 13].Value = string.Empty;
                    }
                    rowInicio++;
                    numInicioMatriz++;
                }
                ////////////// Formato de Celdas Valores
                using (var range = ws.Cells[ParamFormatoSH.RowAnio, 2, ParamFormatoSH.RowAnio + model.Handson.ListaExcelData.Count() + 1, 13])
                {
                    range.Style.Numberformat.Format = @"0.000";
                }


                //ws.Cells[row, column].Value = model.Empresa;
                //ws.Cells[row + 1, column].Value = model.Anho.ToString();
                /*switch (model.Formato.Formatperiodo)
                {
                    case ParametrosFormato.PeriodoDiario:
                        ws.Cells[row + 2, column - 1].Value = "Mes";
                        ws.Cells[row + 2, column].Value = model.Mes;
                        ws.Cells[row + 3, column - 1].Value = "Día";
                        ws.Cells[row + 3, column].Value = model.Dia.ToString();
                        row = row + 3;
                        if (model.ListaHojaPto[0].Famcodi == 43)
                        {
                            ws.Cells[row + 4, column - 1].Value = "Caudal";
                            ws.Cells[row + 4, column].Value = "m3/s";
                            row++;
                        }
                        break;
                    case ParametrosFormato.PeriodoSemanal:
                        ws.Cells[row + 2, column - 1].Value = "Semana";
                        var semanaLength = model.Semana.Length;
                        ws.Cells[row + 2, column].Value = model.Semana.Substring(4, semanaLength - 4);
                        row = row + 2;
                        if (model.ListaHojaPto[0].Famcodi == 43)
                        {
                            ws.Cells[row + 3, column - 1].Value = "Caudal";
                            ws.Cells[row + 3, column].Value = "m3/s";
                            row++;
                        }
                        break;
                    case 3:
                    case 5:
                        ws.Cells[row + 2, column - 1].Value = "Mes";
                        ws.Cells[row + 2, column].Value = model.Mes;
                        row += 2;
                        if (model.ListaHojaPto[0].Famcodi == 43)
                        {
                            ws.Cells[row + 3, column - 1].Value = "Caudal";
                            ws.Cells[row + 3, column].Value = "m3/s";
                            row++;
                        }
                        break;

                }*/

                ///Imprimimos cabecera de puntos de medicion
                //row = ParamFormatoSH.RowDatos;
                //int totColumnas = model.ListaHojaPto.Count;
                //int columnIni = ParamFormatoSH.ColDatos;

                /*if (formatoCodi == ConstantesHidrologiaCD.FormatoVolumenDIarioCodi)
                {
                    totColumnas = 3;//columnas min. max, inicial
                    for (var i = 0; i < model.Handson.ListaExcelData.Count(); i++)
                    {
                        for (var j = 0; j <= totColumnas; j++)
                        {
                            decimal valor = 0;
                            bool canConvert = decimal.TryParse(model.Handson.ListaExcelData[i][j], out valor);
                            if (canConvert)
                                ws.Cells[row + i, j + 1].Value = valor;
                            else
                                ws.Cells[row + i, j + 1].Value = model.Handson.ListaExcelData[i][j];
                            ws.Cells[row + i, j + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            if (i < model.Formato.Formatrows && j >= model.Formato.Formatcols)
                            {
                                ws.Cells[row + i, j + 1].Style.Font.Color.SetColor(System.Drawing.Color.White);
                                ws.Cells[row + i, j + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                                ws.Cells[row + i, j + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                ws.Cells[row + i, j + 1].Style.WrapText = true;
                            }
                        }
                    }

                    /////////////////Formato a Celdas Head ///////////////////
                    using (var range = ws.Cells[row, 2, row + model.Formato.Formatrows - 1, totColumnas + 1])
                    {
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(Color.SteelBlue);
                    }
                    ////////////// Formato de Celdas Valores
                    using (var range = ws.Cells[row + model.Formato.Formatrows, 2, row + model.Handson.ListaExcelData.Count() + 1, totColumnas + 1])
                    {
                        range.Style.Numberformat.Format = @"0.000";
                    }
                }
                else
                {
                    for (var i = 0; i <= model.ListaHojaPto.Count; i++)
                    {
                        for (var j = 0; j < model.Formato.Formathorizonte * model.Formato.RowPorDia + model.Formato.Formatrows; j++)
                        {
                            decimal valor = 0;
                            bool canConvert = decimal.TryParse(model.Handson.ListaExcelData[j][i], out valor);
                            if (canConvert)
                                ws.Cells[row + j, i + 1].Value = valor;
                            else
                                ws.Cells[row + j, i + 1].Value = model.Handson.ListaExcelData[j][i];
                            ws.Cells[row + j, i + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            if (j < model.Formato.Formatrows && i >= model.Formato.Formatcols)
                            {
                                ws.Cells[row + j, i + 1].Style.Font.Color.SetColor(System.Drawing.Color.White);
                                ws.Cells[row + j, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                                ws.Cells[row + j, i + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                ws.Cells[row + j, i + 1].Style.WrapText = true;
                            }
                        }
                    }

                    /////////////////Formato a Celdas Head ///////////////////
                    using (var range = ws.Cells[row, 2, row + model.Formato.Formatrows - 1, model.ListaHojaPto.Count + 1])
                    {
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(Color.SteelBlue);
                    }
                    ////////////// Formato de Celdas Valores
                    using (var range = ws.Cells[row + model.Formato.Formatrows, 2, row + model.Formato.Formathorizonte * model.Formato.RowPorDia + model.Formato.Formatrows - 1, model.ListaHojaPto.Count + 1])
                    {
                        range.Style.Numberformat.Format = @"0.000";
                    }

                }*/







                xlPackage.Save();
            }

        }

        //generar file excel para mediciones RER
        public void GenerarFileExcelRer(FormatoModel model, string ruta)
        {
            string fileTemplate = NombreArchivoMedicionesCD.PlantillaMedicionesCD;
            FileInfo template = new FileInfo(ruta + fileTemplate);
            FileInfo newFile = new FileInfo(ruta + model.Formato.Formatnombre + ".xlsx");
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + model.Formato.Formatnombre + ".xlsx");
            }
            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[ConstantesFormat.HojaFormatoExcel];
                //Escribe nombre de encabezado
                ws.Cells[2, ParamMedicionesExcell.ColEmpresa].Value = model.Empresa;
                ws.Cells[ParamMedicionesExcell.RowFormato, 2].Value = model.Formato.Formatnombre;

                //Imprimimos Codigo Empresa y Codigo Formato
                ws.Cells[1, ParamFormato.ColEmpresa].Value = model.IdEmpresa.ToString();
                ws.Cells[1, ParamFormato.ColFormato].Value = model.Formato.Formatcodi.ToString();

                ///Descripcion del Formato
                /// Nombre de la Empresa
                int row = 2;
                int column = 2;
                switch (model.Formato.Formatperiodo)
                {
                    case ParametrosFormato.PeriodoDiario:
                        ws.Cells[row + 2, column].Value = model.Handson.ListaExcelData[4][0];
                        ws.Cells[row + 2, column].Value = model.Formato.FechaInicio.ToString(ConstantesMedicion.FormatoFecha);
                        row = row + 3;
                        break;
                    case ParametrosFormato.PeriodoSemanal:
                        var ultimaFila = model.Formato.Formathorizonte * model.Formato.RowPorDia + model.Formato.Formatrows;
                        ws.Cells[row + 2, column - 1].Value = "Semana";
                        var semanaLength = model.Semana.Length;
                        ws.Cells[row + 2, column].Value = model.Semana.Substring(4, semanaLength - 4);
                        ws.Cells[row + 3, column - 1].Value = "Fecha Desde";
                        ws.Cells[row + 3, column - 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[row + 3, column - 1].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#CCFFFF"));
                        ws.Cells[row + 3, column - 1].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                        ws.Cells[row + 3, column].Value = model.Formato.FechaInicio.ToString(ConstantesMedicion.FormatoFecha);
                        ws.Cells[row + 4, column - 1].Value = "Fecha Hasta";
                        ws.Cells[row + 4, column - 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[row + 4, column - 1].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#CCFFFF"));
                        ws.Cells[row + 4, column - 1].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                        ws.Cells[row + 4, column].Value = model.Formato.FechaFin.ToString(ConstantesMedicion.FormatoFecha);
                        row = row + 4;
                        break;
                }

                ///Imprimimos cabecera de puntos de medicion
                row = ParamMedicionesExcell.RowDatos;
                int totColumnas = model.ListaHojaPto.Count;
                int columnIni = ParamMedicionesExcell.ColDatos;

                for (var i = 0; i <= model.ListaHojaPto.Count; i++)
                {
                    for (var j = 0; j < model.Formato.Formathorizonte * model.Formato.RowPorDia + model.Formato.Formatrows; j++)
                    {
                        decimal valor = 0;
                        bool canConvert = decimal.TryParse(model.Handson.ListaExcelData[j][i], out valor);
                        if (canConvert)
                            ws.Cells[row + j, i + 1].Value = valor;
                        else
                            ws.Cells[row + j, i + 1].Value = model.Handson.ListaExcelData[j][i];
                        ws.Cells[row + j, i + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        if (j < model.Formato.Formatrows && i >= model.Formato.Formatcols)
                        {
                            //ws.Cells[row + j, i + 1].Style.Font.Color.SetColor(System.Drawing.Color.White);
                            ws.Cells[row + j, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                            ws.Cells[row + j, i + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            ws.Cells[row + j, i + 1].Style.WrapText = true;
                        }
                    }
                }
                /////////////////Formato a Celdas Head ///////////////////
                using (var range = ws.Cells[row, 2, row + model.Formato.Formatrows - 1, model.ListaHojaPto.Count + 1])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#99CCFF"));
                }
                ////////////// Formato de Celdas Valores
                using (var range = ws.Cells[row + model.Formato.Formatrows, 2, row + model.Formato.Formathorizonte * model.Formato.RowPorDia + model.Formato.Formatrows - 1, model.ListaHojaPto.Count + 1])
                {
                    range.Style.Numberformat.Format = @"0.000";
                }

                /////////////////////// Celdas Merge /////////////////////

                foreach (var reg in model.Handson.ListaMerge)
                {
                    int fili = row + reg.row;
                    int filf = row + reg.row + reg.rowspan - 1;
                    int coli = reg.col + 1;
                    int colf = reg.col + reg.colspan - 1 + 1;
                    ws.Cells[fili, coli, filf, colf].Merge = true;
                }

                xlPackage.Save();
            }

        }

        public int VerificarIdsFormato(string file, int idEmpresa, int idFormato)
        {
            int retorno = 1;
            int idEmpresaArchivo;
            int idFormatoEmpresa;
            FileInfo fileInfo = new FileInfo(file);
            using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];
                var valorEmp = ws.Cells[1, ParamFormato.ColEmpresa].Value.ToString();
                if (!int.TryParse(valorEmp, NumberStyles.Any, CultureInfo.InvariantCulture, out idEmpresaArchivo))
                    idEmpresa = 0;
                var valorFormato = ws.Cells[1, ParamFormato.ColFormato].Value.ToString();
                if (!int.TryParse(valorFormato, NumberStyles.Any, CultureInfo.InvariantCulture, out idFormatoEmpresa))
                    idFormatoEmpresa = 0;
                if (idEmpresaArchivo != idEmpresa)
                {
                    retorno = -1;
                }
                if (idFormatoEmpresa != idFormato)
                {
                    retorno = -2;
                }
            }
            return retorno;
        }

        public void GetSizeFormato2(MeFormatoDTO formato)
        {
            switch (formato.Formatperiodo)
            {
                case ParametrosFormato.PeriodoDiario:
                    formato.FechaInicio = formato.FechaProceso;
                    formato.FechaFin = formato.FechaProceso.AddDays(formato.Formathorizonte);
                    formato.RowPorDia = ParametrosFormato.ResolucionDia / (int)formato.Formatresolucion;
                    if (formato.Formatdiaplazo == 0) //Informacion en Tiempo Real
                    {
                        formato.FechaPlazoIni = formato.FechaProceso;
                        formato.FechaPlazo = formato.FechaProceso.AddDays(1).AddMinutes(formato.Formatminplazo);
                    }
                    else
                    {
                        if (formato.Lecttipo == ParametrosLectura.Ejecutado) //Ejecutado
                        {
                            formato.FechaPlazoIni = formato.FechaProceso.AddDays(formato.Formatdiaplazo);
                            formato.FechaPlazo = formato.FechaProceso.AddDays(formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
                        }
                        else
                        {
                            formato.FechaPlazoIni = formato.FechaProceso.AddDays(-1);
                            formato.FechaPlazo = formato.FechaProceso.AddDays(-1).AddMinutes(formato.Formatminplazo);
                        }
                    }
                    break;
                case ParametrosFormato.PeriodoSemanal:
                    formato.FechaInicio = formato.FechaProceso;
                    formato.FechaFin = formato.FechaProceso.AddDays(formato.Formathorizonte);
                    if (formato.Lecttipo == ParametrosLectura.Ejecutado) //Ejecutado
                    {
                        formato.FechaPlazoIni = formato.FechaProceso.AddDays(7);
                        formato.FechaPlazo = formato.FechaProceso.AddDays(7 + formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
                    }
                    else
                    {
                        formato.FechaPlazoIni = formato.FechaProceso.AddDays(-7);
                        formato.FechaPlazo = formato.FechaProceso.AddDays(-7 + formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
                    }
                    switch (formato.Formatresolucion)
                    {
                        case ParametrosFormato.ResolucionCuartoHora:
                        case ParametrosFormato.ResolucionMediaHora:
                        case ParametrosFormato.ResolucionHora:
                            formato.RowPorDia = ParametrosFormato.ResolucionDia / (int)formato.Formatresolucion;
                            break;
                        case ParametrosFormato.ResolucionDia:
                            formato.RowPorDia = 1;
                            break;
                        case ParametrosFormato.ResolucionSemana:
                            formato.RowPorDia = 1;
                            break;
                    }
                    break;
                case ParametrosFormato.PeriodoMensualSemana: //Semanal Mediano Plazo
                    formato.RowPorDia = 1;

                    if (formato.Lecttipo == ParametrosLectura.Ejecutado) //Ejecutado
                    {
                        formato.FechaPlazoIni = formato.FechaProceso.AddMonths(1);
                        formato.FechaPlazo = formato.FechaProceso.AddMonths(1).AddDays(formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
                        //fecha inicio es primera semana del año
                        //fecha fin es ultima semana antes del mes seleccionado
                        //si se selecciona enero se toma todo el año anterior
                        if (formato.FechaProceso.Month == 1)
                        {
                            formato.FechaInicio = EPDate.f_fechainiciosemana(formato.FechaProceso.Year - 1, 1);
                            formato.FechaFin = EPDate.f_fechainiciosemana(formato.FechaProceso.Year - 1, EPDate.TotalSemanasEnAnho(formato.FechaProceso.Year - 1, 6));
                            formato.Formathorizonte = EPDate.TotalSemanasEnAnho(formato.FechaProceso.Year - 1, 6);
                        }
                        else
                        {
                            formato.FechaInicio = EPDate.f_fechainiciosemana(formato.FechaProceso.Year, 1);
                            formato.FechaFin = EPDate.f_fechainiciosemana(formato.FechaProceso.AddDays(-7));
                            formato.Formathorizonte = EPDate.f_numerosemana(formato.FechaFin);
                        }
                    }
                    else //Programado
                    {
                        formato.FechaPlazoIni = formato.FechaProceso.AddMonths(-1);
                        formato.FechaPlazo = formato.FechaProceso.AddMonths(-1).AddDays(formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
                        //fecha inicio es la primera semana del mes seleccionado
                        //fecha fin es la ultima semana del año
                        formato.FechaInicio = EPDate.f_fechainiciosemana(formato.FechaProceso.AddDays(7));
                        formato.FechaFin = EPDate.f_fechainiciosemana(formato.FechaProceso.AddYears(1));
                        formato.Formathorizonte = EPDate.f_numerosemana(formato.FechaFin) +
                            EPDate.TotalSemanasEnAnho(formato.FechaProceso.Year - 1, 6) -
                            EPDate.f_numerosemana(formato.FechaInicio) + 1;
                    }
                    break;
                case ParametrosFormato.PeriodoMensual:
                    switch (formato.Formatresolucion)
                    {
                        case ParametrosFormato.ResolucionCuartoHora:
                        case ParametrosFormato.ResolucionMediaHora:
                        case ParametrosFormato.ResolucionHora:
                            formato.FechaInicio = formato.FechaProceso;
                            formato.FechaFin = formato.FechaProceso.AddMonths(1).AddDays(-1);
                            //formato.FechaPlazo = formato.FechaFin.AddDays(formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
                            formato.FechaPlazo = formato.FechaProceso;
                            formato.FechaPlazo = formato.FechaProceso.AddDays(formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
                            formato.Formathorizonte = ((TimeSpan)(formato.FechaFin - formato.FechaInicio)).Days + 1;
                            formato.RowPorDia = ParametrosFormato.ResolucionDia / (int)formato.Formatresolucion;
                            break;
                        case ParametrosFormato.ResolucionDia:
                            formato.FechaInicio = formato.FechaProceso;
                            formato.FechaFin = formato.FechaProceso.AddMonths(1).AddDays(-1);
                            //formato.FechaPlazo = formato.FechaFin.AddDays(formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
                            formato.FechaPlazo = formato.FechaProceso;
                            formato.FechaPlazo = formato.FechaProceso.AddDays(formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
                            formato.Formathorizonte = ((TimeSpan)(formato.FechaFin - formato.FechaInicio)).Days;
                            formato.RowPorDia = 1;
                            break;
                        case ParametrosFormato.ResolucionMes:
                            formato.RowPorDia = 1;
                            if (formato.Lecttipo == ParametrosLectura.Ejecutado)
                            {
                                formato.Formathorizonte = formato.FechaProceso.Month;
                                formato.FechaFin = formato.FechaProceso;
                                formato.FechaInicio = new DateTime(formato.FechaProceso.Year, 1, 1);
                                //formato.FechaPlazo = formato.FechaFin.AddDays(formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
                                formato.FechaPlazo = formato.FechaProceso.AddMonths(1);
                                formato.FechaPlazo = formato.FechaProceso.AddMonths(1).AddDays(formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
                            }
                            else // Programado
                            {
                                formato.FechaInicio = formato.FechaProceso;
                                formato.FechaFin = formato.FechaProceso.AddMonths(12);
                                formato.Formathorizonte = 12;
                                //formato.FechaPlazo = formato.FechaInicio.AddDays(formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
                                formato.FechaPlazo = formato.FechaProceso;
                                formato.FechaPlazo = formato.FechaProceso.AddDays(formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
                            }
                            break;
                    }
                    break;
            }
        }

        /// <summary>
        /// Inicializa las dimensiones de la matriz de valores del objeto excel web
        /// </summary>
        /// <param name="rowsHead"></param>
        /// <param name="nFil"></param>
        /// <param name="colsHead"></param>
        /// <param name="nCol"></param>
        /// <returns></returns>
        public string[][] InicializaMatrizExcel(int rowsHead, int nFil, int colsHead, int nCol)
        {
            string[][] matriz = new string[nFil + rowsHead][];
            for (int i = 0; i < nFil; i++)
            {

                matriz[i + rowsHead] = new string[nCol + colsHead];
                for (int j = 0; j < nCol; j++)
                {
                    matriz[i + rowsHead][j + colsHead] = string.Empty;
                }
            }
            return matriz;
        }

        public void CargarCabeceraMatrizSH(HandsonModel modelHandson)
        {
            modelHandson.ListaExcelData[0] = new string[13];
            modelHandson.ListaExcelData[0][0] = "AÑO";
            modelHandson.ListaExcelData[0][1] = "ENE";
            modelHandson.ListaExcelData[0][2] = "FEB";
            modelHandson.ListaExcelData[0][3] = "MAR";
            modelHandson.ListaExcelData[0][4] = "ABR";
            modelHandson.ListaExcelData[0][5] = "MAY";
            modelHandson.ListaExcelData[0][6] = "JUN";
            modelHandson.ListaExcelData[0][7] = "JUL";
            modelHandson.ListaExcelData[0][8] = "AGO";
            modelHandson.ListaExcelData[0][9] = "SET";
            modelHandson.ListaExcelData[0][10] = "OCT";
            modelHandson.ListaExcelData[0][11] = "NOV";
            modelHandson.ListaExcelData[0][12] = "DIC";
        }

        public string[][] CargarCabeceraMatrizSHUploadExcel()
        {
            //string[] matriz = new string[0];
            string[][] matriz = new string[65][];
            matriz[0] = new string[13];
            matriz[0][0] = "AÑO";
            matriz[0][1] = "ENE";
            matriz[0][2] = "FEB";
            matriz[0][3] = "MAR";
            matriz[0][4] = "ABR";
            matriz[0][5] = "MAY";
            matriz[0][6] = "JUN";
            matriz[0][7] = "JUL";
            matriz[0][8] = "AGO";
            matriz[0][9] = "SET";
            matriz[0][10] = "OCT";
            matriz[0][11] = "NOV";
            matriz[0][12] = "DIC";

            int anioInicio = ConstantesHidrologiaCD.AnioInicioSH;
            int anioActual = DateTime.Now.Year;
            int rowInicio = ParamFormatoSH.RowAnio;
            int iniMatriz = 1;
            for (int anio = anioInicio; anio < anioActual; anio++)
            {
                matriz[iniMatriz] = new string[13];
                matriz[iniMatriz][0] = anio.ToString();
                matriz[iniMatriz][1] = "";
                matriz[iniMatriz][2] = "";
                matriz[iniMatriz][3] = "";
                matriz[iniMatriz][4] = "";
                matriz[iniMatriz][5] = "";
                matriz[iniMatriz][6] = "";
                matriz[iniMatriz][7] = "";
                matriz[iniMatriz][8] = "";
                matriz[iniMatriz][9] = "";
                matriz[iniMatriz][10] = "";
                matriz[iniMatriz][11] = "";
                matriz[iniMatriz][12] = "";
                iniMatriz++;
            }


            return matriz;
        }


        public string[][] InicializaMatrizExcelVolumenInicial(int rowsHead, int nFil, int colsHead, int nCol)
        {
            var countPtos = nCol / 3;

            string[][] matriz = new string[countPtos + rowsHead][];
            for (int i = 0; i < countPtos; i++)
            {
                matriz[i + rowsHead] = new string[3 + colsHead];
                for (int j = 0; j < 4; j++)
                {
                    matriz[i + rowsHead][j] = string.Empty;
                }
            }
            return matriz;
        }

        /// <summary>
        /// Lee archivo excel cargado y llena matriz de datos para visualizacion web
        /// </summary>
        /// <param name="file"></param>
        /// <param name="rowsHead"></param>
        /// <param name="nFil"></param>
        /// <param name="colsHead"></param>
        /// <param name="nCol"></param>
        /// <returns></returns>
        public Boolean LeerExcelFile(string[][] matriz, string file, int rowsHead, int nFil, int colsHead, int nCol, int formatocodi = 0)
        {
            var filaExecelData = ParamFormato.RowDatos;//
            switch (formatocodi)
            {
                case ConstantesMedicionesCD.FormatoDiarioCodi:
                    filaExecelData = ParamMedicionesExcell.FilaExcelData;
                    break;
                case ConstantesDemandaCP.FormatoDiarioCodi:
                    filaExecelData = ParamFormato.RowDatos;
                    break;
                default:
                    break;
            }

            Boolean retorno = false;
            FileInfo fileInfo = new FileInfo(file);

            using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];
                /// Verificar Formato
                for (int i = 0; i < nFil; i++)
                {
                    for (int j = 0; j < nCol; j++)
                    {
                        string valor = (ws.Cells[i + rowsHead + filaExecelData, j + colsHead + 1].Value != null) ?
                            ws.Cells[i + rowsHead + filaExecelData, j + colsHead + 1].Value.ToString() : string.Empty;
                        matriz[i + rowsHead][j + colsHead] = valor;
                    }
                }
            }
            return retorno;
        }

        /// <summary>
        /// Lee archivo excel cargado y llena matriz de datos para visualizacion web de SH
        /// </summary>
        /// <param name="file"></param>
        /// <param name="rowsHead"></param>
        /// <param name="nFil"></param>
        /// <param name="colsHead"></param>
        /// <param name="nCol"></param>
        /// <returns></returns>
        public Boolean LeerExcelFileSH(string[][] matriz, string file, int rowsHead, int nFil, int colsHead, int nCol, int formatocodi = 0)
        {
            var filaExecelData = ParamFormato.RowDatosSH;
            Boolean retorno = false;
            FileInfo fileInfo = new FileInfo(file);

            using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];
                /// Verificar Formato
                for (int i = 0; i < nFil; i++)
                {
                    for (int j = 0; j < nCol; j++)
                    {
                        string valor = (ws.Cells[i + rowsHead + filaExecelData, j + colsHead + 1].Value != null) ?
                            ws.Cells[i + rowsHead + filaExecelData, j + colsHead + 1].Value.ToString() : string.Empty;
                        matriz[i + rowsHead][j + colsHead] = valor;
                    }
                }
            }
            return retorno;
        }

        /// <summary>
        /// Lee archivo excel cargado de volumen inicial y llena la matriz para su visualización 
        /// </summary>
        /// <param name="matriz"></param>
        /// <param name="file"></param>
        /// <param name="rowsHead"></param>
        /// <param name="nFil"></param>
        /// <param name="colsHead"></param>
        /// <param name="nCol"></param>
        /// <param name="formatocodi"></param>
        /// <returns></returns>
        public Boolean LeerExcelFileVI(string[][] matriz, string file, int rowsHead, int nFil, int colsHead, int nCol, int formatocodi = 0)
        {
            var filaExecelData = ParamFormato.RowDatos;
            var totalColumn = 3;
            var nfilas = nCol / 3;

            Boolean retorno = false;
            FileInfo fileInfo = new FileInfo(file);
            using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];
                /// Verificar Formato
                for (int i = 0; i < nfilas; i++)
                {
                    for (int j = 0; j < totalColumn; j++)
                    {
                        string valor = (ws.Cells[i + rowsHead + filaExecelData, j + colsHead + 1].Value != null) ?
                            ws.Cells[i + rowsHead + filaExecelData, j + colsHead + 1].Value.ToString() : string.Empty;
                        matriz[i + rowsHead][j + colsHead] = valor;
                    }
                }
            }
            return retorno;
        }

        public void BorrarArchivo(String archivo)
        {
            if (System.IO.File.Exists(@archivo))
            {
                try
                {
                    System.IO.File.Delete(@archivo);
                }
                catch (System.IO.IOException e)
                {
                    return;
                }
            }
        }

        //validar checkblanco
        public void ValidarCheckBlancoExcelWeb48(List<MeMedicion48DTO> puntos, int checkBlanco)
        {
            string stValor = string.Empty;
            decimal valor = decimal.MinValue;

            foreach (var listaXPto in puntos.GroupBy(x => new { x.Ptomedicodi, x.Medifecha }))
            {
                foreach (var pto in listaXPto)
                {
                    for (var i = 1; i <= 48; i++)
                    {
                        var propiedad = pto.GetType().GetProperty("H" + i.ToString()).GetValue(pto);
                        stValor = propiedad != null ? propiedad.ToString() : "";
                        if (COES.Base.Tools.Util.EsNumero(stValor))
                        {
                            valor = decimal.Parse(stValor);
                            pto.GetType().GetProperty("H" + i.ToString()).SetValue(pto, valor);
                        }
                        else
                        {
                            if (checkBlanco == 0)
                                pto.GetType().GetProperty("H" + i.ToString()).SetValue(pto, null);
                            else
                                pto.GetType().GetProperty("H" + i.ToString()).SetValue(pto, 0);
                        }

                    }

                }
            }
        }

        /// <summary>
        /// Carga la data recibida del excel web a una lista de Medidion48DTO
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ptos"></param>
        /// <param name="idLectura"></param>
        /// <param name="colHead"></param>
        /// <param name="nCol"></param>
        /// <param name="filaHead"></param>
        /// <param name="nFil"></param>
        /// <param name="checkBlanco"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> LeerExcelWeb48(List<string> data, List<MeHojaptomedDTO> ptos, int idLectura, int colHead, int nCol, int filaHead, int nFil, int checkBlanco)
        {
            var matriz = GetMatrizExcel(data, colHead, nCol, filaHead, nFil);
            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>();
            MeMedicion48DTO reg = new MeMedicion48DTO();
            string stValor = string.Empty;
            decimal valor = decimal.MinValue;
            DateTime fecha = DateTime.MinValue;
            for (var i = 1; i < nCol; i++)
            {
                for (var j = 0; j < nFil; j++)
                {
                    //verificar inicio de dia
                    if ((j % 48) == 0)
                    {
                        if (j != 0)
                            lista.Add(reg);
                        reg = new MeMedicion48DTO();
                        reg.Ptomedicodi = ptos[i - 1].Ptomedicodi;
                        reg.Lectcodi = idLectura;
                        reg.Meditotal = 0;
                        reg.Tipoinfocodi = (int)ptos[i - 1].Tipoinfocodi;
                        reg.Emprcodi = ptos[i - 1].Emprcodi;
                        fecha = DateTime.ParseExact(matriz[j][0], ConstantesFormat.FormatoFechaHora, CultureInfo.InvariantCulture);
                        reg.Medifecha = new DateTime(fecha.Year, fecha.Month, fecha.Day);
                        stValor = matriz[j][i];
                        if (COES.Base.Tools.Util.EsNumero(stValor))
                        {
                            valor = decimal.Parse(stValor);
                            reg.H1 = valor;
                        }
                        else
                        {
                            if (checkBlanco == 0)
                                reg.H1 = null;
                            else
                                reg.H1 = 0;
                        }
                    }
                    else
                    {
                        int indice = j % 48 + 1;
                        stValor = matriz[j][i];
                        if (COES.Base.Tools.Util.EsNumero(stValor))
                        {
                            valor = decimal.Parse(stValor);
                            reg.GetType().GetProperty("H" + indice.ToString()).SetValue(reg, valor);
                        }
                        else
                        {
                            if (checkBlanco == 0)
                                reg.GetType().GetProperty("H" + indice.ToString()).SetValue(reg, null);
                            else
                                reg.GetType().GetProperty("H" + indice.ToString()).SetValue(reg, 0);
                        }
                    }
                }
                lista.Add(reg);
            }
            return lista;
        }

        /// <summary>
        /// Convierte una lista de mediciones en una Matriz Excel Web
        /// </summary>
        /// <param name="data"></param>
        /// <param name="colHead"></param>
        /// <param name="nCol"></param>
        /// <param name="filHead"></param>
        /// <param name="nFil"></param>
        /// <returns></returns>
        private string[][] GetMatrizExcel(List<string> data, int colHead, int nCol, int filHead, int nFil)
        {
            var inicio = (nCol + colHead - 1) * filHead;
            int col = 0;
            int fila = 0;
            string[][] arreglo = new string[nFil][];
            arreglo[0] = new string[nCol];
            for (int i = inicio; i < data.Count(); i++)
            {
                string valor = data[i];
                if (col == nCol)
                {
                    fila++;
                    col = 0;
                    arreglo[fila] = new string[nCol];
                }
                arreglo[fila][col] = valor;
                col++;
            }
            return arreglo;
        }

        //validar checkblanco Medicion1
        public void ValidarCheckBlancoExcelWeb1(List<MeMedicion1DTO> puntos, int checkBlanco)
        {
            string stValor = string.Empty;
            decimal valor = decimal.MinValue;

            foreach (var listaXPto in puntos.GroupBy(x => new { x.Ptomedicodi, x.Medifecha, x.Tipoptomedicodi }))
            {
                foreach (var pto in listaXPto)
                {
                    for (var i = 1; i <= 1; i++)
                    {
                        var propiedad = pto.GetType().GetProperty("H" + i.ToString()).GetValue(pto);
                        stValor = propiedad != null ? propiedad.ToString() : "";
                        if (COES.Base.Tools.Util.EsNumero(stValor))
                        {
                            valor = decimal.Parse(stValor);
                            pto.GetType().GetProperty("H" + i.ToString()).SetValue(pto, valor);
                        }
                        else
                        {
                            if (checkBlanco == 0)
                                pto.GetType().GetProperty("H" + i.ToString()).SetValue(pto, null);
                            else
                                pto.GetType().GetProperty("H" + i.ToString()).SetValue(pto, 0);
                        }

                    }

                }
            }
        }

        /// <summary>
        /// Carga la data recibida del excel web a una lista de Medidion24DTO
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ptos"></param>
        /// <param name="idLectura"></param>
        /// <param name="colHead"></param>
        /// <param name="nCol"></param>
        /// <param name="filaHead"></param>
        /// <param name="nFil"></param>
        /// <param name="checkBlanco"></param>
        /// <returns></returns>
        public List<MeMedicion24DTO> LeerExcelWeb24(List<string> data, List<MeHojaptomedDTO> ptos, int idLectura, int colHead, int nCol, int filaHead, int nFil)
        {
            var matriz = GetMatrizExcel(data, colHead, nCol, filaHead, nFil);
            List<MeMedicion24DTO> lista = new List<MeMedicion24DTO>();
            MeMedicion24DTO reg = new MeMedicion24DTO();
            string stValor = string.Empty;
            decimal valor = decimal.MinValue;
            DateTime fecha = DateTime.MinValue;
            for (var i = 1; i < nCol; i++)
            {
                for (var j = 0; j < nFil; j++)
                {
                    //verificar inicio de dia
                    if ((j % 24) == 0)
                    {
                        if (j != 0)
                            lista.Add(reg);
                        reg = new MeMedicion24DTO();
                        reg.Ptomedicodi = ptos[i - 1].Ptomedicodi;
                        reg.Lectcodi = idLectura;
                        reg.Tipoinfocodi = (int)ptos[i - 1].Tipoinfocodi;
                        reg.Emprcodi = ptos[i - 1].Emprcodi;
                        fecha = DateTime.ParseExact(matriz[j][0], ConstantesFormat.FormatoFechaHora, CultureInfo.InvariantCulture);
                        reg.Medifecha = new DateTime(fecha.Year, fecha.Month, fecha.Day);
                        stValor = matriz[j][i];
                        if (COES.Base.Tools.Util.EsNumero(stValor))
                        {
                            valor = decimal.Parse(stValor);
                            reg.H1 = valor;
                        }
                        else
                            reg.H1 = null;
                    }
                    else
                    {
                        stValor = matriz[j][i];
                        int indice = j % 24 + 1;
                        if (COES.Base.Tools.Util.EsNumero(stValor))
                        {
                            valor = decimal.Parse(stValor);

                            reg.GetType().GetProperty("H" + indice.ToString()).SetValue(reg, valor);
                        }
                        else
                            reg.GetType().GetProperty("H" + indice.ToString()).SetValue(reg, null);

                    }
                }
                lista.Add(reg);
            }
            return lista;
        }

        /// <summary>
        /// Genera el Formato en Excel HTML
        /// </summary>
        /// <param name="model"></param>
        /// <param name="idEnvio"></param>
        /// <param name="enPlazo"></param>
        /// <returns></returns>
        public string GenerarFormatoHtml(FormatoModel model, int idEnvio, Boolean enPlazo)
        {
            StringBuilder strHtml = new StringBuilder();

            strHtml.Append("");
            strHtml.Append("<div id='tab-2' class='ui-tabs-panel ui-widget-content ui-corner-bottom tab-panel js-tab-contents' name='grid'>");

            strHtml.Append("    <nav class='tool-menu tool-menu--grid'>");


            if (idEnvio <= 0)
            {
                strHtml.Append("        <ul class='tool-menu__btn-list'>");
                strHtml.Append("            <li><a class='link--tool js-download-grid' href='#' original-title='ctrl/⌘-S'><i class='ploticon-download'></i><p class='tool-menu__icon-label'>Formato</p></a></li>");
                if (enPlazo)
                {
                    strHtml.Append("            <li><a id='btnSelectExcel3' class='link--tool js-add-grid' href='javascript:;' original-title='ctrl/⌘-S'><i class='ploticon-copy'></i><p class='tool-menu__icon-label'>Agregar</p></a></li>");
                    strHtml.Append("            <li><a class='link--tool js-save-grid' href='#' original-title='ctrl/⌘-S'><i class='ploticon-save'></i><p class='tool-menu__icon-label'>Grabar</p></a></li>");
                }
                strHtml.Append("            <li><a class='link--tool js-export-grid' href='#' original-title='ctrl/⌘-S'><i class='ploticon-copy'></i><p class='tool-menu__icon-label'>Exportar</p></a></li>");
                if (model.ListaEnvios.Count > 0)
                    strHtml.Append("            <li><a id='" + model.ListaEnvios[model.ListaEnvios.Count - 1].Enviocodi.ToString() + "'class='link--tool js-reenvio-grid' href='#' original-title='ctrl/⌘-S'><i class='ploticon-copy'></i><p class='tool-menu__icon-label'>Envíos</p></a></li>");
                strHtml.Append("            <li><a class='link--tool js-exit-grid' href='#' original-title='ctrl/⌘-S'><i class='ploticon-copy'></i><p class='tool-menu__icon-label'>Salir</p></a></li>");
                strHtml.Append("        </ul>");

                strHtml.Append("        <ul class='tool-menu__btn-list'>");
                strHtml.Append("            <li><a class='link--tool js-nonumero-grid' href='#' original-title='ctrl/⌘-S'><i class='ploticon-copy'></i><p id='idNonum' class='tool-menu__icon-label'>0</p></a></li>");
                strHtml.Append("        </ul>");
            }
            else
            {
                strHtml.Append("        <ul class='tool-menu__btn-list'>");
                foreach (var reg in model.ListaEnvios)
                {
                    strHtml.Append("            <li><a id='" + reg.Enviocodi.ToString() + "' class='link--tool js-reenvio-grid' href='#' original-title='ctrl/⌘-S'><i class='ploticon-copy'></i><p class='tool-menu__icon-label'>" + reg.Enviocodi + "</p></a></li>");
                }
                strHtml.Append("            <li><a id='0' class='link--tool js-reenvio-grid' href='#' original-title='ctrl/⌘-S'><i class='ploticon-copy'></i><p class='tool-menu__icon-label'>Envíar</p></a></li>");
                strHtml.Append("            <li><a class='link--tool js-exit-grid' href='#' original-title='ctrl/⌘-S'><i class='ploticon-copy'></i><p class='tool-menu__icon-label'>Salir</p></a></li>");
                strHtml.Append("        </ul>");
            }

            strHtml.Append("    </nav>");


            strHtml.Append("<div style='clear:both; height:20px'></div>");
            strHtml.Append("<table class='table-form-vertical'>");
            strHtml.Append("  <tr><td >" + model.Formato.Areaname + " </td>");
            strHtml.Append("  <td>" + model.Formato.Formatnombre + " </td>");
            strHtml.Append("  <td>Empresa:</td><td>" + model.Empresa + "</td>");
            strHtml.Append("  <td>Año:</td><td>" + model.Anho + "</td>");
            switch (model.Formato.Formatperiodo)
            {
                case ParametrosFormato.PeriodoDiario:
                    strHtml.Append("  <td>Mes:</td><td>" + model.Mes + "</td>");
                    strHtml.Append("  <td>Día:</td><td>" + model.Dia + "</td>");
                    break;
                case ParametrosFormato.PeriodoSemanal:
                    strHtml.Append("  <td>Semana:</td><td>" + model.Semana + "</td>");
                    break;
                case ParametrosFormato.PeriodoMensual:
                case ParametrosFormato.PeriodoMensualSemana:
                    strHtml.Append("  <td>Mes:</td><td>" + model.Mes + "</td><tr>");
                    break;
            }
            strHtml.Append("</table></div>");


            return strHtml.ToString();
        }

        /// <summary>
        /// Inicializa lista de filas readonly para la matriz excel web
        /// </summary>
        /// <param name="filHead"></param>
        /// <param name="filData"></param>
        /// <returns></returns>
        public List<bool> InicializaListaFilaReadOnly(int filHead, int filData, bool plazo, int horaini, int horafin)
        {
            List<bool> lista = new List<bool>();
            for (int i = 0; i < filHead; i++)
            {
                lista.Add(true);
            }
            for (int i = 0; i < filData; i++)
            {
                if (plazo)
                {
                    if (horafin == 0)
                        lista.Add(false);
                    else
                    {
                        if ((i >= horaini) && (i < horafin))
                        {
                            lista.Add(false);
                        }
                        else
                            lista.Add(true);
                    }
                }
                else
                    lista.Add(true);
            }

            return lista;
        }

        /// <summary>
        /// Inicializa lista de filas readonly para la matriz excel web Series Hidrologicas
        /// </summary>
        /// <param name="filHead"></param>
        /// <param name="filData"></param>
        /// <returns></returns>
        public List<bool> InicializaListaFilaReadOnlySH(int filHead, int filData)
        {
            List<bool> lista = new List<bool>();
            for (int i = 0; i < filHead; i++)
            {
                lista.Add(false);
            }
            for (int i = 0; i < filData; i++)
            {
                lista.Add(false);
            }

            return lista;
        }

        /// <summary>
        /// Obtiene matriz de string que son solo los valores de las celdas del excel web ingresando como parametro una lista de mediciones.
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="listaCambios"></param>
        /// <param name="formato"></param>
        public void ObtieneMatrizWebExcel(FormatoModel model, List<object> lista, List<MeCambioenvioDTO> listaCambios, int idEnvio)
        {
            if (idEnvio > 0)
            {
                foreach (var reg in listaCambios)
                {
                    if (reg.Cambenvcolvar != null)
                    {
                        var cambios = reg.Cambenvcolvar.Split(',');
                        for (var i = 0; i < cambios.Count(); i++)
                        {
                            TimeSpan ts = reg.Cambenvfecha - model.Formato.FechaInicio;
                            var horizon = ts.Days;
                            var col = model.ListaHojaPto.Find(x => x.Ptomedicodi == reg.Ptomedicodi).Hojaptoorden + model.ColumnasCabecera - 1;
                            var ColumReal = model.ListaHojaPto.FindIndex(x => x.Ptomedicodi == reg.Ptomedicodi) + 1;
                            col = ColumReal;
                            var row = model.FilasCabecera +
                                ObtieneRowChange((int)model.Formato.Formatperiodo, (int)model.Formato.Formatresolucion, int.Parse(cambios[i]),
                                model.Formato.RowPorDia, reg.Cambenvfecha, model.Formato.FechaInicio);
                            //int.Parse(cambios[i]) + model.Formato.RowPorDia * horizon;
                            model.ListaCambios.Add(new CeldaCambios()
                            {
                                Row = row,
                                Col = col
                            });
                        }
                    }
                }
            }
            for (int k = 0; k < model.ListaHojaPto.Count; k++)
            {
                for (int z = 0; z < model.Formato.Formathorizonte; z++) //Horizonte se comporta como uno cuando resolucion es mas que un dia
                {
                    DateTime fechaFind = GetNextFilaHorizonte((int)model.Formato.Formatperiodo, (int)model.Formato.Formatresolucion, z, model.Formato.FechaInicio);
                    var reg = lista.Find(i => (int)i.GetType().GetProperty("Ptomedicodi").GetValue(i, null) == model.ListaHojaPto[k].Ptomedicodi
                                && (DateTime)i.GetType().GetProperty("Medifecha").GetValue(i, null) == fechaFind);

                    for (int j = 1; j <= model.Formato.RowPorDia; j++) // nBlock se comporta como horizonte cuando resolucion es mas de un dia
                    {
                        if (k == 0)
                        {
                            int jIni = 0;
                            if (model.Formato.Formatresolucion >= ParametrosFormato.ResolucionDia)
                                jIni = j - 1;
                            else
                                jIni = j;

                            model.Handson.ListaExcelData[z * model.Formato.RowPorDia + j + model.FilasCabecera - 1][model.ColumnasCabecera - 1] =
                               // ((model.FechaInicio.AddMinutes(z * ParametrosFormato.ResolucionDia + jIni * (int)model.Formato.Formatresolucion))).ToString(Constantes.FormatoFechaHora);
                               ObtenerCeldaFecha((int)model.Formato.Formatperiodo, (int)model.Formato.Formatresolucion, model.Formato.Lecttipo, z, jIni, model.Formato.FechaInicio);
                        }
                        if (reg != null)
                        {
                            decimal? valor = (decimal?)reg.GetType().GetProperty("H" + j).GetValue(reg, null);
                            if (valor != null)
                            {
                                model.Handson.ListaExcelData[z * model.Formato.RowPorDia + j + model.FilasCabecera - 1][k + model.ColumnasCabecera] = valor.ToString();
                            }
                        }
                    }
                }
            }

            //}
        }

        /// <summary>
        /// Obtiene matriz de string que son solo los valores de las celdas del excel web ingresando como parametro una lista de mediciones1.
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="listaCambios"></param>
        /// <param name="formato"></param>
        public void ObtenerListaExcelDataM1(FormatoModel model, List<object> lista, List<MeCambioenvioDTO> listaCambios, int idEnvio)
        {
            if (idEnvio > 0)
            {
                foreach (var reg in listaCambios)
                {
                    if (reg.Cambenvcolvar != null)
                    {
                        var cambios = reg.Cambenvcolvar.Split(',');
                        for (var i = 0; i < cambios.Count(); i++)
                        {
                            TimeSpan ts = reg.Cambenvfecha - model.Formato.FechaInicio;
                            var horizon = ts.Days;
                            var col = model.ListaHojaPto.Find(x => x.Ptomedicodi == reg.Ptomedicodi).Hojaptoorden + model.ColumnasCabecera - 1;
                            var ColumReal = model.ListaHojaPto.FindIndex(x => x.Ptomedicodi == reg.Ptomedicodi && x.Tptomedicodi == reg.Tipoptomedicodi) + 1;
                            var columValor = ColumReal % 3;
                            col = columValor == 0 ? 3 : columValor;
                            var row = (ColumReal - 1) / 3 + 2;
                            model.ListaCambios.Add(new CeldaCambios()
                            {
                                Row = row,
                                Col = col
                            });
                        }
                    }
                }
            }


            DateTime fechaFind = GetNextFilaHorizonte((int)model.Formato.Formatperiodo, (int)model.Formato.Formatresolucion, 0, model.Formato.FechaInicio);
            var nuev = 0;

            //listaptos
            foreach (var listaAgrupNew in model.ListaHojaPto.OrderBy(x => x.Hojaptoorden).ToList().GroupBy(x => x.Ptomedicodi))
            {
                var varcolum = 1;
                foreach (var listAgrup in listaAgrupNew)
                {
                    var reg = lista.Find(i => (int)i.GetType().GetProperty("Ptomedicodi").GetValue(i, null) == listAgrup.Ptomedicodi
                                && (DateTime)i.GetType().GetProperty("Medifecha").GetValue(i, null) == fechaFind
                                && (int)i.GetType().GetProperty("Tipoptomedicodi").GetValue(i, null) == listAgrup.Tptomedicodi);

                    if (reg != null)
                    {
                        decimal? valor = (decimal?)reg.GetType().GetProperty("H" + 1).GetValue(reg, null);
                        if (valor != null)
                        {
                            model.Handson.ListaExcelData[nuev + model.FilasCabecera][varcolum] = valor.ToString();
                        }
                    }
                    varcolum++;
                }
                nuev++;
            }
        }

        public int ObtieneRowChange(int periodo, int resolucion, int indiceBloque, int rowPorDia, DateTime fechaCambio, DateTime fechaInicio)
        {
            int row = 0;
            switch (periodo)
            {
                case ParametrosFormato.PeriodoMensual:
                    switch (resolucion)
                    {
                        case ParametrosFormato.ResolucionMes:
                            row = ((fechaCambio.Year - fechaInicio.Year) * 12) + fechaCambio.Month - fechaInicio.Month;
                            break;
                        default:
                            row = indiceBloque + rowPorDia * (fechaCambio - fechaInicio).Days - 1;
                            break;
                    }
                    break;
                default:
                    row = indiceBloque + rowPorDia * (fechaCambio - fechaInicio).Days - 1;
                    break;
            }

            return row;
        }

        /// <summary>
        /// Obtiene el nombre de la celda fechaa mostrarse en los formatos excel.
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="resolucion"></param>
        /// <param name="horizonte"></param>
        /// <param name="indice"></param>
        /// <param name="fechaInicio"></param>
        /// <returns></returns>
        public string ObtenerCeldaFecha(int periodo, int resolucion, int tipoLectura, int horizonte, int indice, DateTime fechaInicio)
        {
            string resultado = string.Empty;
            switch (periodo)
            {
                case ParametrosFormato.PeriodoMensual:
                    switch (resolucion)
                    {
                        case ParametrosFormato.ResolucionMes:
                            if (tipoLectura == ParametrosLectura.Ejecutado)
                                resultado = fechaInicio.Year.ToString() + " " + COES.Base.Tools.Util.ObtenerNombreMesAbrev(horizonte + 1);
                            else
                            {
                                resultado = fechaInicio.AddMonths(horizonte).Year.ToString() + " " + COES.Base.Tools.Util.ObtenerNombreMesAbrev(fechaInicio.AddMonths(horizonte).Month);
                            }
                            break;
                        default:
                            resultado = fechaInicio.AddMinutes(horizonte * ParametrosFormato.ResolucionDia + resolucion * indice).ToString(ConstantesFormat.FormatoFechaHora);
                            break;
                    }
                    break;
                case ParametrosFormato.PeriodoMensualSemana:
                    int semana = COES.Base.Tools.Util.ObtenerNroSemanasxAnho(fechaInicio, 6) + horizonte;
                    int semanaMax = COES.Base.Tools.Util.TotalSemanasEnAnho(fechaInicio.Year, 6);
                    semana = (semana > semanaMax) ? semana - semanaMax : semana;
                    string stSemana = (semana > 9) ? semana.ToString() : "0" + semana.ToString();
                    if (tipoLectura == ParametrosLectura.Ejecutado)
                    {

                        resultado = fechaInicio.AddDays(horizonte * 7).Year.ToString() + " Sem:" + stSemana;
                    }
                    else
                    {
                        resultado = fechaInicio.AddDays(horizonte * 7).Year.ToString() + " Sem:" + stSemana;
                    }
                    break;
                default:
                    resultado = fechaInicio.AddMinutes(horizonte * ParametrosFormato.ResolucionDia + resolucion * indice).ToString(ConstantesFormat.FormatoFechaHora);
                    break;
            }

            return resultado;
        }
        /// <summary>
        /// Devuelve la fecha del siguiente bloque
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="resolucion"></param>
        /// <param name="horizonte"></param>
        /// <param name="fechaInicio"></param>
        /// <returns></returns>
        public DateTime GetNextFilaHorizonte(int periodo, int resolucion, int horizonte, DateTime fechaInicio)
        {
            DateTime resultado = DateTime.MinValue;
            switch (periodo)
            {
                case ParametrosFormato.PeriodoMensual:
                    switch (resolucion)
                    {
                        case ParametrosFormato.ResolucionMes:
                            resultado = fechaInicio.AddMonths(horizonte);
                            break;

                        default:
                            resultado = fechaInicio.AddDays(horizonte);
                            break;
                    }
                    break;
                case ParametrosFormato.PeriodoMensualSemana:
                    resultado = fechaInicio.AddDays(horizonte * 7);
                    break;
                default:
                    resultado = fechaInicio.AddDays(horizonte);
                    break;
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene la lista de datos de generación Rer
        /// </summary>
        /// <param name="model"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="lista"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ObtenerDatosRer(int lectcodi, DateTime fechaInicio, DateTime fechaFin, int idEmpresa)
        {
            //>>>>>>>>>>>>>>>>>>>>>>>>>>Obtener datos de Generación Rer
            var horizonte = lectcodi == ConstantesMedicionesCD.FormatoSemanalCodi ? 1 : 0;

            DateTime fechaInicial = fechaInicio;
            DateTime fechaFinal = horizonte == 0 ? fechaInicial : fechaFin;

            List<MeMedicion48DTO> listaGeneracionRer = servRer.ConsultaDatosRer(idEmpresa, horizonte, fechaInicial, fechaFinal);

            return listaGeneracionRer;
        }

        #endregion

        #region Pronostico Demanda

        public List<object> GetDataPronosticoDemanda(int formatcodi, DateTime fechaini, DateTime fechafin)
        {
            List<Object> listaGenerica = new List<Object>();
            var lista = Factory.FactorySic.GetPrnMediciongrpRepository().GetDataFormatoPronosticoDemanda(formatcodi, fechaini, fechafin);
            foreach (var reg in lista)
            {
                listaGenerica.Add(reg);
            }
            return listaGenerica;
        }


        #endregion

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_LECTURA
        /// </summary>
        public List<MeLecturaDTO> ListMeLecturas()
        {
            return FactorySic.GetMeLecturaRepository().List();
        }

        /// <summary>
        /// Convierte Lista Object a medicion96
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        public static List<MeMedicion96DTO> Convert96DTO(List<Object> lista)
        {
            List<MeMedicion96DTO> listaFinal = new List<MeMedicion96DTO>();

            foreach (var entity in lista)
            {
                MeMedicion96DTO reg = new MeMedicion96DTO();
                reg.Lectcodi = (int)entity.GetType().GetProperty("Lectcodi").GetValue(entity, null);
                reg.Medifecha = (DateTime)entity.GetType().GetProperty("Medifecha").GetValue(entity, null);
                reg.Ptomedicodi = (int)entity.GetType().GetProperty("Ptomedicodi").GetValue(entity, null);
                reg.Tipoinfocodi = (int)entity.GetType().GetProperty("Tipoinfocodi").GetValue(entity, null);
                reg.Tipoptomedicodi = (int)entity.GetType().GetProperty("Tipoptomedicodi").GetValue(entity, null);
                for (int i = 1; i <= 96; i++)
                {
                    decimal? valor = (decimal?)entity.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i).ToString()).GetValue(entity, null);
                    reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i).ToString()).SetValue(reg, valor);
                }
                listaFinal.Add(reg);
            }
            return listaFinal;
        }

        public List<MeMedicion24DTO> GetDataAnt(int Formatcodi, int idEmpresa, DateTime FechaInicio, DateTime FechaFin)
        {
            return FactorySic.GetMeMedicion24Repository().GetEnvioArchivo(Formatcodi, idEmpresa, FechaInicio, FechaFin);
        }


        /// <summary>
        /// Validar si existe similitud de datos con el proceso anterior
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="formatoAnterior"></param>
        /// <returns></returns>
        public bool ValidarDataPeriodoAnterior48(List<MeMedicion48DTO> entitys, int idEmpresa, MeFormatoDTO formatoAnterior)
        {
            try
            {
                decimal? sumaDataAnterior = 0;
                decimal? sumaDataActual = 0;
                var listaDataAnterior = Convert48DTO(GetDataFormato(idEmpresa, formatoAnterior, 0, 0));
                if (listaDataAnterior.Count > 0) // Verificar si hay cambios en el envio
                {
                    foreach (var regAnt in listaDataAnterior)
                    {
                        for (int i = 1; i <= 48; i++)
                        {
                            decimal? valorOrigen = (decimal?)regAnt.GetType().GetProperty("H" + i.ToString()).GetValue(regAnt, null);
                            sumaDataAnterior += (valorOrigen == null ? 0 : valorOrigen);
                        }
                    }

                    foreach (var regAactual in entitys)
                    {
                        for (int i = 1; i <= 48; i++)
                        {
                            decimal? valorOrigen = (decimal?)regAactual.GetType().GetProperty("H" + i.ToString()).GetValue(regAactual, null);
                            sumaDataActual += (valorOrigen == null ? 0 : valorOrigen);
                        }
                    }


                    return sumaDataActual == sumaDataAnterior;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public bool ValidarDataPeriodoAnterior96(List<MeMedicion96DTO> entitys, int idEmpresa, MeFormatoDTO formatoAnterior)
        {
            try
            {
                decimal? sumaDataAnterior = 0;
                decimal? sumaDataActual = 0;
                var listaDataAnterior = Convert96DTO(GetDataFormato(idEmpresa, formatoAnterior, 0, 0));
                if (listaDataAnterior.Count > 0) // Verificar si hay cambios en el envio
                {
                    foreach (var regAnt in listaDataAnterior)
                    {
                        for (int i = 1; i <= 96; i++)
                        {
                            decimal? valorOrigen = (decimal?)regAnt.GetType().GetProperty("H" + i.ToString()).GetValue(regAnt, null);
                            sumaDataAnterior += (valorOrigen == null ? 0 : valorOrigen);
                        }
                    }

                    foreach (var regAactual in entitys)
                    {
                        for (int i = 1; i <= 96; i++)
                        {
                            decimal? valorOrigen = (decimal?)regAactual.GetType().GetProperty("H" + i.ToString()).GetValue(regAactual, null);
                            sumaDataActual += (valorOrigen == null ? 0 : valorOrigen);
                        }
                    }


                    return sumaDataActual == sumaDataAnterior;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void GrabarJustificacionCongelados(List<MeJustificacionDTO> listaJustificacion, int idEnvio, string usuario)
        {
            try
            {
                if (listaJustificacion != null)
                {
                    for (int i = 0; i < listaJustificacion.Count; i++)
                    {
                        MeJustificacionDTO mj = listaJustificacion[i];
                        mj.Enviocodi = idEnvio;
                        mj.Justfeccreacion = DateTime.Now;
                        mj.Justusucreacion = usuario;
                    }
                    //TODO Eliminar valores?
                    foreach (MeJustificacionDTO entity in listaJustificacion)
                    {
                        FactorySic.GetMeJustificacionRepository().Save(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite grabar los datos cargados
        /// </summary>
        /// <param name="entitys"></param>
        public void GrabarValoresCargados96(List<MeMedicion96DTO> entitys, string usuario, int idEnvio, int idEmpresa, MeFormatoDTO formato)
        {
            try
            {
                //determinar la lista de tptomedicodi
                var listaTptomedicodi = entitys.GroupBy(x => x.Tipoptomedicodi).Select(x => x.Key).ToList();

                int count = 0;
                //Traer Ultimos Valores
                var lista = Convert96DTO(GetDataFormato(idEmpresa, formato, 0, 0));
                if (lista.Count > 0) // Verificar si hay cambios en el envio
                {
                    var listaCambio = new List<MeCambioenvioDTO>();
                    var listaOrigen = new List<MeCambioenvioDTO>();


                    foreach (var reg in entitys)
                    {
                        reg.Tipoptomedicodi = reg.Tipoptomedicodi != 0 ? reg.Tipoptomedicodi : -1;

                        var regAnt = lista.Find(x => x.Medifecha == reg.Medifecha && x.Ptomedicodi == reg.Ptomedicodi &&
                            x.Lectcodi == reg.Lectcodi && x.Tipoinfocodi == reg.Tipoinfocodi && x.Tipoptomedicodi == reg.Tipoptomedicodi);


                        if (regAnt != null)
                        {
                            List<string> filaValores = new List<string>();
                            List<string> filaValoresOrigen = new List<string>();
                            List<string> filaCambios = new List<string>();
                            for (int i = 1; i <= 96; i++)
                            {
                                decimal? valorOrigen = (decimal?)regAnt.GetType().GetProperty("H" + i.ToString()).GetValue(regAnt, null);
                                decimal? valorModificado = (decimal?)reg.GetType().GetProperty("H" + i.ToString()).GetValue(reg, null);
                                if (valorModificado != null)
                                    filaValores.Add(valorModificado.ToString());
                                else
                                    filaValores.Add("");
                                if (valorOrigen != null)
                                    filaValoresOrigen.Add(valorOrigen.ToString());
                                else
                                    filaValoresOrigen.Add("");
                                if (valorOrigen != valorModificado)//&& valorOrigen != null && valorModificado != null)
                                {
                                    if (count <= 100)
                                    {
                                        filaCambios.Add(i.ToString());
                                        count++;
                                    }
                                }
                            }


                            if (filaCambios.Count > 0)
                            {
                                MeCambioenvioDTO cambio = new MeCambioenvioDTO();
                                cambio.Cambenvdatos = String.Join(",", filaValores);
                                cambio.Cambenvcolvar = String.Join(",", filaCambios);
                                cambio.Cambenvfecha = (DateTime)reg.Medifecha;
                                cambio.Enviocodi = idEnvio;
                                //cambio.Formatcodi = formato.Formatcodi;
                                cambio.Ptomedicodi = reg.Ptomedicodi;
                                cambio.Tipoinfocodi = reg.Tipoinfocodi;
                                cambio.Tipoptomedicodi = reg.Tipoptomedicodi;
                                cambio.Lastuser = usuario;
                                cambio.Lastdate = DateTime.Now;
                                listaCambio.Add(cambio);
                                /// Si no ha habido cambio se graba el registro original
                                if (ListMeCambioenvios(reg.Ptomedicodi, reg.Tipoinfocodi, formato.Formatcodi, (DateTime)reg.Medifecha).Count == 0)
                                {
                                    int idEnvioPrevio = 0;
                                    var listAux = GetByCriteriaMeEnvios(idEmpresa, formato.Formatcodi, formato.FechaInicio);
                                    if (listAux.Count > 0)
                                        idEnvioPrevio = listAux.Min(x => x.Enviocodi);
                                    MeCambioenvioDTO origen = new MeCambioenvioDTO();
                                    origen.Cambenvdatos = String.Join(",", filaValoresOrigen);
                                    origen.Cambenvcolvar = "";
                                    origen.Cambenvfecha = (DateTime)reg.Medifecha;
                                    origen.Enviocodi = idEnvioPrevio;
                                    //origen.Formatcodi = formato.Formatcodi;
                                    origen.Ptomedicodi = reg.Ptomedicodi;
                                    origen.Tipoinfocodi = reg.Tipoinfocodi;
                                    origen.Tipoptomedicodi = reg.Tipoptomedicodi;
                                    origen.Lastuser = usuario;
                                    origen.Lastdate = DateTime.Now;
                                    listaOrigen.Add(origen);
                                }
                            }
                        }
                    }
                    if (listaCambio.Count > 0)
                    {//Grabar Cambios
                        GrabarCambios(listaCambio);
                        GrabarCambios(listaOrigen);
                        //si es primer reenvio grabar valores origen

                    }
                }

                //Eliminar Valores Previos 
                //Condicional Formato de pruebas unidad
                if (formato.Formatcodi == ConstantesFormatoMedicion.IdFormatoPruebasAleatorias)
                {  //Formato de pruebas unidad

                    foreach (MeMedicion96DTO ent in entitys)//foreach (var tptomedicodi in listaTptomedicodi)
                    {
                        DeleteMeMedicion96(ent.Ptomedicodi, ConstantesFormatoMedicion.IdTipoinfocodiMw, (DateTime)ent.Medifecha, ConstantesFormatoMedicion.IdLectcodiPruebasAleatorias);
                        //var tptomedi = tptomedicodi != 0 ? tptomedicodi : -1;
                        //EliminarValoresCargados96(tptomedi, (int)formato.Lectcodi, formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin);
                    }
                    //Fin Formato de pruebas unidad
                }
                else
                {
                    foreach (var tptomedicodi in listaTptomedicodi)
                    {
                        var tptomedi = tptomedicodi != 0 ? tptomedicodi : -1;
                        EliminarValoresCargados96(tptomedi, (int)formato.Lectcodi, formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin);
                    }
                }


                if (formato.Formatcodi == ConstantesInterconexiones.IdFormatoInterconexion)
                {
                    //eliminar valores antiguos
                    List<DateTime> listaFechaAntiguo = entitys.Select(x => x.Medifecha.Value).Distinct().OrderBy(x => x).ToList();
                    foreach (var f in listaFechaAntiguo)
                    {
                        DeleteMeMedicion96(ConstantesFormatoMedicion.IdExportacionL2280MWh, ConstantesFormatoMedicion.IdMWh, f, ConstantesFormatoMedicion.IdlecturaInterconexion);
                        DeleteMeMedicion96(ConstantesFormatoMedicion.IdImportacionL2280MWh, ConstantesFormatoMedicion.IdMWh, f, ConstantesFormatoMedicion.IdlecturaInterconexion);
                        DeleteMeMedicion96(ConstantesFormatoMedicion.IdExportacionL2280MVARr, ConstantesFormatoMedicion.IdMVARh, f, ConstantesFormatoMedicion.IdlecturaInterconexion);
                        DeleteMeMedicion96(ConstantesFormatoMedicion.IdImportacionL2280MVARr, ConstantesFormatoMedicion.IdMVARh, f, ConstantesFormatoMedicion.IdlecturaInterconexion);
                        DeleteMeMedicion96(ConstantesFormatoMedicion.IdPtoMedicionL2280, ConstantesInterconexiones.IdTipoInfocodiKV, f, ConstantesFormatoMedicion.IdlecturaInterconexion);
                        DeleteMeMedicion96(ConstantesFormatoMedicion.IdPtoMedicionL2280, ConstantesInterconexiones.IdTipoInfocodiA, f, ConstantesFormatoMedicion.IdlecturaInterconexion);
                    }
                }

                foreach (MeMedicion96DTO entity in entitys)
                {
                    entity.Tipoptomedicodi = entity.Tipoptomedicodi != 0 ? entity.Tipoptomedicodi : -1;
                    entity.Meditotal = entity.Meditotal == null ? 0 : entity.Meditotal;
                    FactorySic.GetMeMedicion96Repository().Save(entity);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Se obtiene los datos enviados de un formato de la BD.
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="formato"></param>
        /// <param name="idEnvio"></param>
        /// <returns></returns>
        public List<Object> GetDataFormato(int idEmpresa, MeFormatoDTO formato, int idEnvio, int idUltimoEnvio)
        {
            List<Object> listaGenerica = new List<Object>();

            //asignar codigo de formato temporalmente
            var formatoValidate = formato.Formatdependeconfigptos != null ? (int)formato.Formatdependeconfigptos : formato.Formatcodi;
            var formatcodiInicio = formato.Formatcodi;
            formato.Formatcodi = formatoValidate;

            switch (formato.Formatresolucion)
            {
                case ParametrosFormato.ResolucionMes:
                case ParametrosFormato.ResolucionDia:
                case ParametrosFormato.ResolucionSemana:
                    DateTime fechaMaxEnvio = DateTime.MinValue;
                    int idMaxEnvio = ObtenerIdMaxEnvioFormato(formato.Formatcodi, idEmpresa);
                    var regMax = GetByIdMeEnvio(idMaxEnvio);
                    if (regMax != null)
                    {
                        fechaMaxEnvio = (DateTime)regMax.Enviofechaperiodo;
                    }
                    List<MeMedicion1DTO> lista1 = new List<MeMedicion1DTO>();
                    List<MeCambioenvioDTO> cambio = new List<MeCambioenvioDTO>();
                    if (idEnvio > 0)
                    {
                        lista1 = FactorySic.GetMeMedicion1Repository().GetEnvioArchivo(formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin, formato.Lectcodi);
                        cambio = GetAllCambioEnvio(formato.Formatcodi, formato.FechaInicio, formato.FechaFin, idEnvio, idEmpresa);
                    }
                    else if (idEnvio == 0)
                    {
                        if (formato.FechaProceso < fechaMaxEnvio)
                        {
                            if (idUltimoEnvio > 0)
                            {
                                lista1 = FactorySic.GetMeMedicion1Repository().GetEnvioArchivo(formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin, formato.Lectcodi);
                                cambio = GetAllCambioEnvio(formato.Formatcodi, formato.FechaInicio, formato.FechaFin, idUltimoEnvio, idEmpresa);
                            }
                        }
                        else
                        {
                            lista1 = FactorySic.GetMeMedicion1Repository().GetEnvioArchivo(formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin, formato.Lectcodi);
                            if (formato.Formatsecundario != 0)
                            {
                                var listaAux = FactorySic.GetMeMedicion1Repository().GetDataFormatoSecundario(formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin);
                                for (var i = formato.FechaInicio; i < formato.FechaFin; i = i.AddDays(1))
                                {
                                    var find = lista1.Find(x => x.Medifecha == i);
                                    if (find == null)
                                    {
                                        lista1 = lista1.Union(listaAux.Where(x => x.Medifecha == i)).ToList();
                                    }
                                }
                            }
                        }
                    }

                    if (idEnvio >= 0 && (lista1.Count > 0))
                    {
                        if (cambio.Count > 0)
                        {
                            foreach (var reg in cambio)
                            {
                                if (formato.Formatresolucion == ParametrosFormato.ResolucionDia)
                                {
                                    var findM1 = lista1.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Tipoinfocodi == reg.Tipoinfocodi && x.Medifecha == reg.Cambenvfecha && x.Tipoptomedicodi == reg.Tipoptomedicodi);
                                    if (findM1 != null)
                                    {
                                        decimal dato;
                                        decimal? numero = null;
                                        if (decimal.TryParse(reg.Cambenvdatos, out dato))
                                            numero = dato;
                                        findM1.H1 = numero;
                                    }
                                }
                                else
                                {
                                    var find = lista1.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Tipoinfocodi == reg.Tipoinfocodi && x.Medifecha == reg.Cambenvfecha);
                                    if (find != null)
                                    {
                                        decimal dato;
                                        decimal? numero = null;
                                        if (decimal.TryParse(reg.Cambenvdatos, out dato))
                                            numero = dato;
                                        find.H1 = numero;
                                    }
                                }
                            }
                        }
                    }
                    foreach (var reg in lista1)
                    {
                        listaGenerica.Add(reg);
                    }
                    break;

                case ParametrosFormato.ResolucionHora:
                    List<MeMedicion24DTO> lista24 = FactorySic.GetMeMedicion24Repository().GetEnvioArchivo(formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin);
                    if (idEnvio != 0)
                    {
                        var lista = GetAllCambioEnvio(formato.Formatcodi, formato.FechaInicio, formato.FechaFin, idEnvio, idEmpresa);
                        if (lista.Count > 0)
                        {
                            foreach (var reg in lista)
                            {
                                var find = lista24.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Tipoinfocodi == reg.Tipoinfocodi &&
                                    x.Medifecha == reg.Cambenvfecha);
                                if (find != null)
                                {
                                    var fila = reg.Cambenvdatos.Split(',');
                                    for (var i = 0; i < 24; i++)
                                    {
                                        decimal dato;
                                        decimal? numero = null;
                                        if (decimal.TryParse(fila[i], out dato))
                                            numero = dato;
                                        find.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(find, numero);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (formato.Formatsecundario != 0 && lista24.Count == 0)
                        {
                            lista24 = FactorySic.GetMeMedicion24Repository().GetDataFormatoSecundario(formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin);
                        }
                    }
                    foreach (var reg in lista24)
                    {
                        listaGenerica.Add(reg);
                    }
                    break;
                case ParametrosFormato.ResolucionMediaHora:
                    List<MeMedicion48DTO> lista48 = new List<MeMedicion48DTO>();
                    if (!formato.FlagUtilizaHoja)
                    {
                        lista48 = FactorySic.GetMeMedicion48Repository().GetEnvioArchivo(formato.Formatcodi, idEmpresa.ToString(), formato.FechaInicio, formato.FechaFin, formato.Lectcodi);
                    }
                    else
                    {
                        if (ConstantesIEOD.IdFormatoDemandaDiaria == formato.Formatcodi)
                        {
                            List<MeMedicion48DTO> lista48Tmp = FactorySic.GetMeMedicion48Repository().GetEnvioArchivo2(formato.Formatcodi, idEmpresa.ToString(), formato.FechaProceso.Date.AddDays(-1), formato.FechaProceso.Date.AddDays(1));
                            foreach (var hoja in formato.ListaHoja)
                            {
                                if (ConstantesIEOD.LectCodiDemandaDiariaProgramado == hoja.Lectcodi)
                                {
                                    DateTime fecha = formato.FechaProceso.Date.AddDays(1);
                                    lista48.AddRange(lista48Tmp.Where(x => x.Medifecha == fecha && x.Hojacodi == hoja.Hojacodi).ToList());
                                }
                                else
                                {
                                    DateTime fecha = formato.FechaProceso.Date.AddDays(-1);
                                    lista48.AddRange(lista48Tmp.Where(x => x.Medifecha == fecha && x.Hojacodi == hoja.Hojacodi).ToList());
                                }
                            }
                        }
                        else
                        {
                            lista48 = FactorySic.GetMeMedicion48Repository().GetEnvioArchivo2(formato.Formatcodi, idEmpresa.ToString(), formato.FechaInicio, formato.FechaFin);
                        }
                    }

                    if (idEnvio != 0)
                    {
                        List<MeCambioenvioDTO> lista = new List<MeCambioenvioDTO>();
                        if (ConstantesIEOD.IdFormatoDemandaDiaria == formato.Formatcodi)
                        {
                            List<MeCambioenvioDTO> listaTmp = this.GetAllCambioEnvio(formato.Formatcodi, formato.FechaProceso.Date.AddDays(-1), formato.FechaProceso.Date.AddDays(1), idEnvio, idEmpresa);

                            foreach (var hoja in formato.ListaHoja)
                            {
                                if (ConstantesIEOD.LectCodiDemandaDiariaProgramado == hoja.Lectcodi)
                                {
                                    DateTime fecha = formato.FechaProceso.Date.AddDays(1);
                                    lista.AddRange(listaTmp.Where(x => x.Cambenvfecha == fecha && x.Hojacodi == hoja.Hojacodi).ToList());
                                }
                                else
                                {
                                    DateTime fecha = formato.FechaProceso.Date.AddDays(-1);
                                    lista.AddRange(listaTmp.Where(x => x.Cambenvfecha == fecha && x.Hojacodi == hoja.Hojacodi).ToList());
                                }
                            }
                        }
                        else
                        {
                            lista = this.GetAllCambioEnvio(formato.Formatcodi, formato.FechaInicio, formato.FechaFin, idEnvio, idEmpresa);
                        }

                        if (lista.Count > 0)
                        {
                            foreach (var reg in lista)
                            {
                                MeMedicion48DTO find = !formato.FlagUtilizaHoja
                                ? lista48.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Tipoinfocodi == reg.Tipoinfocodi && x.Medifecha == reg.Cambenvfecha)
                                : lista48.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Tipoinfocodi == reg.Tipoinfocodi && x.Medifecha == reg.Cambenvfecha && x.Hojacodi == reg.Hojacodi);
                                if (find != null)
                                {
                                    var fila = reg.Cambenvdatos.Split(',');
                                    for (var i = 0; i < 48; i++)
                                    {
                                        decimal dato;
                                        decimal? numero = null;
                                        if (decimal.TryParse(fila[i], out dato))
                                            numero = dato;
                                        find.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(find, numero);
                                    }

                                }
                            }
                        }
                    }

                    foreach (var reg in lista48)
                    {
                        listaGenerica.Add(reg);
                    }
                    break;

                case ParametrosFormato.ResolucionCuartoHora:
                    List<MeMedicion96DTO> lista96 = FactorySic.GetMeMedicion96Repository().GetEnvioArchivo(formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin);
                    if (idEnvio != 0)
                    {
                        var lista = GetAllCambioEnvio(formato.Formatcodi, formato.FechaInicio, formato.FechaFin, idEnvio, idEmpresa);
                        if (lista.Count > 0)
                        {
                            foreach (var reg in lista)
                            {
                                var find = lista96.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Tipoinfocodi == reg.Tipoinfocodi &&
                                    x.Medifecha == reg.Cambenvfecha);
                                if (find != null)
                                {
                                    var fila = reg.Cambenvdatos.Split(',');
                                    for (var i = 0; i < 96; i++)
                                    {
                                        decimal dato;
                                        decimal? numero = null;
                                        if (decimal.TryParse(fila[i], out dato))
                                            numero = dato;
                                        find.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(find, numero);
                                    }

                                }
                            }
                        }
                    }

                    foreach (var reg in lista96)
                    {
                        listaGenerica.Add(reg);
                    }
                    break;
            }

            formato.Formatcodi = formatcodiInicio;
            return listaGenerica;
        }

        /// <summary>
        /// Se obtiene los datos enviados de un formato de la BD.
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="formato"></param>
        /// <param name="idEnvio"></param>
        /// <returns></returns>
        public List<Object> GetDataFormatoReporte(int idEmpresa, MeFormatoDTO formato, int ptomedicodi, int idEnvio, List<MeCambioenvioDTO> cambios, List<MeMedicion1DTO> listado1, List<MeMedicion24DTO> listado24, List<MeMedicion48DTO> listado48, List<MeMedicion96DTO> listado96)
        {
            List<Object> listaGenerica = new List<Object>();
            switch (formato.Formatresolucion)
            {
                case ParametrosFormato.ResolucionMes:
                case ParametrosFormato.ResolucionDia:
                case ParametrosFormato.ResolucionSemana:
                    List<MeMedicion1DTO> lista1 = new List<MeMedicion1DTO>();
                    List<MeCambioenvioDTO> cambio = new List<MeCambioenvioDTO>();
                    if (idEnvio > 0)
                    {
                        // lista1 =   listado1.Where(x=>x.codigo)
                        lista1 = listado1.Where(x => x.Medifecha >= formato.FechaInicio.Date && x.Medifecha <= formato.FechaFin && x.Ptomedicodi == ptomedicodi).ToList();
                        //   FactorySic.GetMeMedicion1Repository().GetEnvioArchivo(formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin);
                        cambio = cambios;
                        //  cambio = GetAllCambioEnvio(formato.Formatcodi, formato.FechaInicio, formato.FechaFin, idEnvio, idEmpresa);
                    }

                    if (idEnvio >= 0 && (lista1.Count > 0))
                    {
                        if (cambio.Count > 0)
                        {
                            foreach (var reg in cambio)
                            {
                                var find = lista1.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Tipoinfocodi == reg.Tipoinfocodi &&
                                    x.Medifecha == reg.Cambenvfecha);
                                if (find != null)
                                {
                                    decimal dato;
                                    decimal? numero = null;
                                    if (decimal.TryParse(reg.Cambenvdatos, out dato))
                                        numero = dato;
                                    find.H1 = numero;
                                }
                            }
                        }
                    }
                    foreach (var reg in lista1)
                    {
                        listaGenerica.Add(reg);
                    }
                    break;

                case ParametrosFormato.ResolucionHora:
                    //List<MeMedicion24DTO> lista24 = FactorySic.GetMeMedicion24Repository().GetEnvioArchivo(formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin);
                    List<MeMedicion24DTO> lista24 = listado24.Where(x => x.Medifecha >= formato.FechaInicio.Date && x.Medifecha <= formato.FechaFin && x.Ptomedicodi == ptomedicodi).ToList();
                    if (idEnvio != 0)
                    {
                        //  var lista = GetAllCambioEnvio(formato.Formatcodi, formato.FechaInicio, formato.FechaFin, idEnvio, idEmpresa);
                        var lista = cambios;
                        if (lista.Count > 0)
                        {
                            foreach (var reg in lista)
                            {
                                var find = lista24.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Tipoinfocodi == reg.Tipoinfocodi &&
                                    x.Medifecha == reg.Cambenvfecha);
                                if (find != null)
                                {
                                    var fila = reg.Cambenvdatos.Split(',');
                                    for (var i = 0; i < 24; i++)
                                    {
                                        decimal dato;
                                        decimal? numero = null;
                                        if (decimal.TryParse(fila[i], out dato))
                                            numero = dato;
                                        find.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(find, numero);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (formato.Formatsecundario != 0 && lista24.Count == 0)
                        {
                            lista24 = listado24.Where(x => x.Medifecha >= formato.FechaInicio.Date && x.Medifecha <= formato.FechaFin && x.Ptomedicodi == ptomedicodi).ToList();
                            // lista24 = FactorySic.GetMeMedicion24Repository().GetDataFormatoSecundario(formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin);
                        }
                    }
                    foreach (var reg in lista24)
                    {
                        listaGenerica.Add(reg);
                    }
                    break;
                case ParametrosFormato.ResolucionMediaHora:
                    List<MeMedicion48DTO> lista48 = new List<MeMedicion48DTO>();
                    if (!formato.FlagUtilizaHoja)
                    {
                        //lista48 = FactorySic.GetMeMedicion48Repository().GetEnvioArchivo(formato.Formatcodi, idEmpresa.ToString(), formato.FechaInicio, formato.FechaFin);
                        lista48 = listado48.Where(x => x.Medifecha >= formato.FechaInicio.Date && x.Medifecha <= formato.FechaFin && x.Ptomedicodi == ptomedicodi).ToList();
                    }
                    else
                    {
                        lista48 = FactorySic.GetMeMedicion48Repository().GetEnvioArchivo2(formato.Formatcodi, idEmpresa.ToString(), formato.FechaInicio, formato.FechaFin);
                    }

                    if (idEnvio != 0)
                    {
                        List<MeCambioenvioDTO> lista = new List<MeCambioenvioDTO>();
                        //lista = this.GetAllCambioEnvio(formato.Formatcodi, formato.FechaInicio, formato.FechaFin, idEnvio, idEmpresa);
                        lista = cambios;

                        if (lista.Count > 0)
                        {
                            foreach (var reg in lista)
                            {
                                MeMedicion48DTO find = !formato.FlagUtilizaHoja
                                ? lista48.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Tipoinfocodi == reg.Tipoinfocodi && x.Medifecha == reg.Cambenvfecha)
                                : lista48.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Tipoinfocodi == reg.Tipoinfocodi && x.Medifecha == reg.Cambenvfecha && x.Hojacodi == reg.Hojacodi);
                                if (find != null)
                                {
                                    var fila = reg.Cambenvdatos.Split(',');
                                    for (var i = 0; i < 48; i++)
                                    {
                                        decimal dato;
                                        decimal? numero = null;
                                        if (decimal.TryParse(fila[i], out dato))
                                            numero = dato;
                                        find.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(find, numero);
                                    }

                                }
                            }
                        }
                    }

                    foreach (var reg in lista48)
                    {
                        listaGenerica.Add(reg);
                    }
                    break;

                case ParametrosFormato.ResolucionCuartoHora:
                    List<MeMedicion96DTO> lista96 = listado96;
                    lista96 = listado96.Where(x => x.Medifecha >= formato.FechaInicio.Date && x.Medifecha <= formato.FechaFin && x.Ptomedicodi == ptomedicodi).ToList();
                    if (idEnvio != 0)
                    {
                        //var lista = GetAllCambioEnvio(formato.Formatcodi, formato.FechaInicio, formato.FechaFin, idEnvio, idEmpresa);
                        var lista = cambios;
                        if (lista.Count > 0)
                        {
                            foreach (var reg in lista)
                            {
                                var find = lista96.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Tipoinfocodi == reg.Tipoinfocodi &&
                                    x.Medifecha == reg.Cambenvfecha);
                                if (find != null)
                                {
                                    var fila = reg.Cambenvdatos.Split(',');
                                    for (var i = 0; i < 96; i++)
                                    {
                                        decimal dato;
                                        decimal? numero = null;
                                        if (decimal.TryParse(fila[i], out dato))
                                            numero = dato;
                                        find.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(find, numero);
                                    }

                                }
                            }
                        }
                    }

                    foreach (var reg in lista96)
                    {
                        listaGenerica.Add(reg);
                    }
                    break;
            }
            return listaGenerica;
        }

        //inicio agregado
        /// <summary>
        /// Permite eliminar los valores previos cargados
        /// </summary>
        /// <param name="idTptomedicodi"></param>
        /// <param name="idLectura"></param>
        /// <param name="idFormato"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        public void EliminarValoresCargados96(int idTptomedicodi, int idLectura, int idFormato, int idEmpresa, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                FactorySic.GetMeMedicion96Repository().DeleteEnvioArchivo2(idTptomedicodi, idLectura, fechaInicio, fechaFin, idFormato, idEmpresa);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        //fin agregado

        /// <summary>
        /// Permite eliminar los valores previos cargados
        /// </summary>
        /// <param name="horizonte"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        public void EliminarValoresCargados96(int idLectura, int idFormato, int idEmpresa, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                FactorySic.GetMeMedicion96Repository().DeleteEnvioArchivo(idLectura, fechaInicio, fechaFin, idFormato, idEmpresa);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public void EliminarValoresCargados96(int idLectura, int idFormato, int idEmpresa, DateTime fechaInicio, DateTime fechaFin, DateTime fechaTiee)
        {
            try
            {
                FactorySic.GetMeMedicion96Repository().DeleteEnvioArchivo(idLectura, fechaInicio, fechaFin, idFormato, idEmpresa, fechaTiee);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite eliminar los valores previos cargados
        /// </summary>
        /// <param name="horizonte"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        public void EliminarValoresCargados48(int idLectura, int idFormato, int idEmpresa, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                FactorySic.GetMeMedicion48Repository().DeleteEnvioArchivo(idLectura, fechaInicio, fechaFin, idFormato, idEmpresa);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite eliminar los valores previos cargados
        /// </summary>
        /// <param name="idTptomedicodi"></param>
        /// <param name="idLectura"></param>
        /// <param name="idFormato"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        public void EliminarValoresCargados1(int idTptomedicodi, int idLectura, int idFormato, int idEmpresa, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                FactorySic.GetMeMedicion1Repository().DeleteEnvioArchivo2(idTptomedicodi, idLectura, fechaInicio, fechaFin, idFormato, idEmpresa);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Convierte Lista Object a medicion48
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        public static List<MeMedicion48DTO> Convert48DTO(List<Object> lista)
        {
            List<MeMedicion48DTO> listaFinal = new List<MeMedicion48DTO>();

            foreach (var entity in lista)
            {
                MeMedicion48DTO reg = new MeMedicion48DTO();
                reg.Lectcodi = (int)entity.GetType().GetProperty("Lectcodi").GetValue(entity, null);
                reg.Medifecha = (DateTime)entity.GetType().GetProperty("Medifecha").GetValue(entity, null);
                reg.Ptomedicodi = (int)entity.GetType().GetProperty("Ptomedicodi").GetValue(entity, null);
                reg.Tipoinfocodi = (int)entity.GetType().GetProperty("Tipoinfocodi").GetValue(entity, null);
                reg.Hojacodi = (int)entity.GetType().GetProperty("Hojacodi").GetValue(entity, null);
                for (int i = 1; i <= 48; i++)
                {
                    decimal? valor = (decimal?)entity.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i).ToString()).GetValue(entity, null);
                    reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i).ToString()).SetValue(reg, valor);
                }
                listaFinal.Add(reg);
            }
            return listaFinal;
        }

        /// <summary>
        /// Permite grabar los datos cargados
        /// </summary>
        /// <param name="entitys"></param>
        public void GrabarValoresCargados48(List<MeMedicion48DTO> entitys, string usuario, int idEnvio, int idEmpresa, MeFormatoDTO formato)
        {
            try
            {
                //calcular Meditotal MeMedicion48DTO
                foreach (var val in entitys)
                {
                    decimal? sumTotal = 0;
                    for (int j = 1; j <= 48; j++)
                    {
                        decimal? valor = (decimal?)val.GetType().GetProperty("H" + j.ToString()).GetValue(val, null);
                        if (valor != null)
                            sumTotal += valor;
                        else
                            sumTotal += 0;
                    }
                    val.Meditotal = sumTotal;
                }

                //Traer Ultimos Valores
                var lista = Convert48DTO(GetDataFormato(idEmpresa, formato, 0, 0));

                //asignar codigo de formato temporalmente (solo debe aplicar para formatos usados en MCP Yupana)
                List<int> listaFormatcodiMcpYupana = new List<int>() { ConstantesDemandaCP.FormatoDiarioCodi, ConstantesDemandaCP. FormatoSemanalCodi
                                                            , ConstantesHidrologiaCD.FormatoDiarioCodi , ConstantesHidrologiaCD.FormatoReprogramaCodi, ConstantesHidrologiaCD.FormatoSemanalCodi
                                                            ,ConstantesHidrologiaCD.FormatoVolumenDIarioCodi, ConstantesHidrologiaCD.FormatoVolumenReprogramaCodi , ConstantesHidrologiaCD.FormatoVolumenSemanalCodi
                                                            , 112, 113, 114};

                var formatoValidate = formato.Formatdependeconfigptos != null && listaFormatcodiMcpYupana.Contains(formato.Formatcodi) ? (int)formato.Formatdependeconfigptos : formato.Formatcodi;
                var formatcodiInicio = formato.Formatcodi;
                formato.Formatcodi = formatoValidate;

                if (lista.Count > 0) // Verificar si hay cambios en el envio
                {
                    var listaCambio = new List<MeCambioenvioDTO>();
                    var listaOrigen = new List<MeCambioenvioDTO>();
                    foreach (var reg in entitys)
                    {
                        MeMedicion48DTO regAnt = null;
                        if (!formato.FlagUtilizaHoja)
                        {
                            regAnt = lista.Find(x => x.Medifecha == reg.Medifecha && x.Ptomedicodi == reg.Ptomedicodi && x.Lectcodi == reg.Lectcodi && x.Tipoinfocodi == reg.Tipoinfocodi);
                        }
                        else
                        {
                            regAnt = lista.Find(x => x.Medifecha == reg.Medifecha && x.Ptomedicodi == reg.Ptomedicodi
                                && x.Lectcodi == reg.Lectcodi && x.Tipoinfocodi == reg.Tipoinfocodi && x.Hojacodi == reg.Hojacodi);
                        }
                        List<string> filaValores = new List<string>();
                        List<string> filaValoresOrigen = new List<string>();
                        List<string> filaCambios = new List<string>();
                        if (regAnt != null)
                        {
                            for (int i = 1; i <= 48; i++)
                            {
                                decimal? valorOrigen = (decimal?)regAnt.GetType().GetProperty("H" + i.ToString()).GetValue(regAnt, null);
                                decimal? valorModificado = (decimal?)reg.GetType().GetProperty("H" + i.ToString()).GetValue(reg, null);
                                if (valorModificado != null)
                                    filaValores.Add(valorModificado.ToString());
                                else
                                    filaValores.Add("");
                                if (valorOrigen != null)
                                    filaValoresOrigen.Add(valorOrigen.ToString());
                                else
                                    filaValoresOrigen.Add("");
                                if (valorOrigen != valorModificado)// && valorOrigen != null && valorModificado != null)
                                {
                                    filaCambios.Add(i.ToString());
                                }
                            }
                        }
                        if (filaCambios.Count > 0)
                        {
                            MeCambioenvioDTO cambio = new MeCambioenvioDTO();
                            cambio.Cambenvdatos = String.Join(",", filaValores);
                            cambio.Cambenvcolvar = String.Join(",", filaCambios);
                            cambio.Cambenvfecha = (DateTime)reg.Medifecha;
                            cambio.Enviocodi = idEnvio;
                            cambio.Formatcodi = formato.Formatcodi;
                            cambio.Ptomedicodi = reg.Ptomedicodi;
                            cambio.Tipoinfocodi = reg.Tipoinfocodi;
                            cambio.Hojacodi = reg.Hojacodi;
                            cambio.Lastuser = usuario;
                            cambio.Lastdate = DateTime.Now;
                            listaCambio.Add(cambio);
                            /// Si no ha habido cambio se graba el registro original
                            if (ListMeCambioenvios(reg.Ptomedicodi, reg.Tipoinfocodi, formato.Formatcodi, (DateTime)reg.Medifecha).Count == 0)
                            {
                                List<MeEnvioDTO> listAux = new List<MeEnvioDTO>();
                                if (ConstantesIEOD.IdFormatoDemandaDiaria == formato.Formatcodi)
                                {
                                    if (ConstantesIEOD.LectCodiDemandaDiariaProgramado == reg.Lectcodi)
                                    {
                                        DateTime fecha = formato.FechaProceso.Date.AddDays(1);
                                        listAux = GetByCriteriaMeEnvios(idEmpresa, formato.Formatcodi, fecha);
                                    }
                                    else
                                    {
                                        DateTime fecha = formato.FechaProceso.Date.AddDays(-1);
                                        listAux = GetByCriteriaMeEnvios(idEmpresa, formato.Formatcodi, fecha);
                                    }
                                }
                                else
                                {
                                    listAux = GetByCriteriaMeEnvios(idEmpresa, formato.Formatcodi, formato.FechaProceso);
                                    listAux = GetByCriteriaMeEnviosFormatoEnergPrimaria(listAux, idEmpresa, formato.Formatcodi, formato.IdFormatoNuevo, formato.FechaProceso);
                                }

                                if (listAux.Count > 0)
                                {
                                    int idEnvioPrevio = listAux.Min(x => x.Enviocodi);
                                    MeCambioenvioDTO origen = new MeCambioenvioDTO();
                                    origen.Cambenvdatos = String.Join(",", filaValoresOrigen);
                                    origen.Cambenvcolvar = "";
                                    origen.Cambenvfecha = (DateTime)reg.Medifecha;
                                    origen.Enviocodi = idEnvioPrevio;
                                    origen.Formatcodi = formato.Formatcodi;
                                    origen.Ptomedicodi = reg.Ptomedicodi;
                                    origen.Tipoinfocodi = reg.Tipoinfocodi;
                                    cambio.Hojacodi = reg.Hojacodi;
                                    origen.Lastuser = usuario;
                                    origen.Lastdate = DateTime.Now;
                                    listaOrigen.Add(origen);
                                }
                            }
                        }
                    }
                    if (listaCambio.Count > 0)
                    {//Grabar Cambios
                        GrabarCambios(listaCambio);
                        GrabarCambios(listaOrigen);
                        //si es primer reenvio grabar valores origen

                    }
                }
                //Eliminar Valores Previos
                if (!formato.FlagUtilizaHoja)
                {
                    EliminarValoresCargados48((int)formato.Lectcodi, formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin);
                }
                else
                {
                    if (ConstantesIEOD.IdFormatoDemandaDiaria == formato.Formatcodi)
                    {
                        foreach (var hoja in formato.ListaHoja)
                        {
                            if (ConstantesIEOD.LectCodiDemandaDiariaProgramado == hoja.Lectcodi)
                            {
                                DateTime fecha = formato.FechaProceso.Date.AddDays(1);
                                EliminarValoresCargados48(hoja.Lectcodi.Value, formato.Formatcodi, idEmpresa, fecha, fecha);
                            }
                            else
                            {
                                DateTime fecha = formato.FechaProceso.Date.AddDays(-1);
                                EliminarValoresCargados48(hoja.Lectcodi.Value, formato.Formatcodi, idEmpresa, fecha, fecha);
                            }
                        }
                    }
                    else
                    {
                        foreach (var hoja in formato.ListaHoja)
                        {
                            EliminarValoresCargados48(hoja.Lectcodi.Value, formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin);
                        }
                    }
                }

                foreach (MeMedicion48DTO entity in entitys)
                {
                    if (entity.Hojacodi == ConstantesIEOD.EjecHojaCodiMVAR)
                    {
                        entity.Tipoinfocodi = ConstantesIEOD.EjecTipoInfoMVAR;
                    }
                    if (entity.Hojacodi == ConstantesIEOD.ProgHojaCodiMVAR)
                    {
                        entity.Tipoinfocodi = ConstantesIEOD.ProgTipoInfoMVAR;
                    }
                    //ASSETEC 20200519  ID de la solicitud : 12345
                    //FactorySic.GetMeMedicion48Repository().Save(entity);
                    FactorySic.GetMeMedicion48Repository().SaveMeMedicion48Id(entity);
                }

                formato.Formatcodi = formatcodiInicio;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Graba Valores Cargados en  Hoja Web Excel y verifica si hay repeditos
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="usuario"></param>
        /// <param name="idEnvio"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="formato"></param>
        public void GrabarValoresCargados1(List<MeMedicion1DTO> entitys, string usuario, int idEnvio, int idEmpresa, MeFormatoDTO formato, int lectcodi)
        {
            try
            {
                //determinar la lista de tptomedicodi
                var listaTptomedicodi = entitys.GroupBy(x => x.Tipoptomedicodi).Select(x => x.Key).ToList();

                //asignar codigo de formato temporalmente
                var formatoValidate = formato.Formatdependeconfigptos != null ? (int)formato.Formatdependeconfigptos : formato.Formatcodi;
                var formatcodiInicio = formato.Formatcodi;
                formato.Formatcodi = formatoValidate;

                //Traer Ultimos Valores
                var lista = Convert1DTO(GetDataFormato(idEmpresa, formato, 0, 0));
                if (lista.Count > 0) // Verificar si hay cambios en el envio
                {
                    var listaCambio = new List<MeCambioenvioDTO>();
                    var listaOrigen = new List<MeCambioenvioDTO>();
                    foreach (var reg in entitys)
                    {
                        reg.Tipoptomedicodi = reg.Tipoptomedicodi != 0 ? reg.Tipoptomedicodi : -1;

                        var regAnt = lista.Find(x => x.Medifecha == reg.Medifecha && x.Ptomedicodi == reg.Ptomedicodi &&
                            x.Lectcodi == reg.Lectcodi && x.Tipoinfocodi == reg.Tipoinfocodi && x.Tipoptomedicodi == reg.Tipoptomedicodi);
                        List<string> filaValores = new List<string>();
                        List<string> filaValoresOrigen = new List<string>();
                        List<string> filaCambios = new List<string>();
                        if (regAnt != null)
                        {
                            decimal? valorOrigen = regAnt.H1; // (decimal?)regAnt.GetType().GetProperty("H" + i.ToString()).GetValue(regAnt, null);
                            decimal? valorModificado = reg.H1; //(decimal?)reg.GetType().GetProperty("H" + i.ToString()).GetValue(reg, null);
                            if (valorModificado != null)
                                filaValores.Add(valorModificado.ToString());
                            else
                                filaValores.Add("");
                            if (valorOrigen != null)
                                filaValoresOrigen.Add(valorOrigen.ToString());
                            else
                                filaValoresOrigen.Add("");
                            if (valorOrigen != valorModificado)//&& valorOrigen != null && valorModificado != null)
                            {
                                filaCambios.Add("1");
                            }
                        }
                        if (filaCambios.Count > 0)
                        {
                            MeCambioenvioDTO cambio = new MeCambioenvioDTO();
                            cambio.Cambenvdatos = String.Join(",", filaValores);
                            cambio.Cambenvcolvar = String.Join(",", filaCambios);
                            cambio.Cambenvfecha = (DateTime)reg.Medifecha;
                            cambio.Enviocodi = idEnvio;
                            cambio.Formatcodi = formato.Formatcodi;
                            cambio.Ptomedicodi = reg.Ptomedicodi;
                            cambio.Tipoinfocodi = reg.Tipoinfocodi;
                            cambio.Tipoptomedicodi = reg.Tipoptomedicodi;
                            cambio.Lastuser = usuario;
                            cambio.Lastdate = DateTime.Now;
                            listaCambio.Add(cambio);
                            /// Si no ha habido cambio se graba el registro original
                            if (ListMeCambioenvios(reg.Ptomedicodi, reg.Tipoinfocodi, formato.Formatcodi, (DateTime)reg.Medifecha).Where(x => x.Tipoptomedicodi == reg.Tipoptomedicodi).ToList().Count == 0)
                            {
                                int idEnvioPrevio = 0;
                                var listAux = GetByCriteriaMeEnvios(idEmpresa, formato.Formatcodi, formato.FechaInicio);
                                if (listAux.Count > 0)
                                    idEnvioPrevio = listAux.Min(x => x.Enviocodi);
                                MeCambioenvioDTO origen = new MeCambioenvioDTO();
                                origen.Cambenvdatos = String.Join(",", filaValoresOrigen);
                                origen.Cambenvcolvar = "";
                                origen.Cambenvfecha = (DateTime)reg.Medifecha;
                                origen.Enviocodi = idEnvioPrevio;
                                //origen.Formatcodi = formato.Formatcodi;
                                origen.Ptomedicodi = reg.Ptomedicodi;
                                origen.Tipoinfocodi = reg.Tipoinfocodi;
                                origen.Tipoptomedicodi = reg.Tipoptomedicodi;
                                origen.Lastuser = usuario;
                                origen.Lastdate = DateTime.Now;
                                if (idEnvioPrevio > 0)
                                    listaOrigen.Add(origen);
                            }
                        }
                    }
                    if (listaCambio.Count > 0)
                    {//Grabar Cambios
                        GrabarCambios(listaCambio);
                        GrabarCambios(listaOrigen);
                        //si es primer reenvio grabar valores origen

                    }
                }

                    //Eliminar Valores Previos
                    foreach (var tptomedicodi in listaTptomedicodi)
                    {
                        var tptomedi = tptomedicodi != 0 ? tptomedicodi : -1;
                        EliminarValoresCargados1(tptomedi, lectcodi, formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin);
                    }

                foreach (MeMedicion1DTO entity in entitys)
                {
                    entity.Tipoptomedicodi = entity.Tipoptomedicodi != 0 ? entity.Tipoptomedicodi : -1;
                    FactorySic.GetMeMedicion1Repository().Save(entity);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Convierte Lista Object a medicion 1
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        public static List<MeMedicion1DTO> Convert1DTO(List<Object> lista)
        {
            List<MeMedicion1DTO> listaFinal = new List<MeMedicion1DTO>();
            foreach (var entity in lista)
            {
                MeMedicion1DTO reg = new MeMedicion1DTO();
                reg.Lectcodi = (int)entity.GetType().GetProperty("Lectcodi").GetValue(entity, null);
                reg.Medifecha = (DateTime)entity.GetType().GetProperty("Medifecha").GetValue(entity, null);
                reg.Ptomedicodi = (int)entity.GetType().GetProperty("Ptomedicodi").GetValue(entity, null);
                reg.Tipoinfocodi = (int)entity.GetType().GetProperty("Tipoinfocodi").GetValue(entity, null);
                reg.Tipoptomedicodi = (int)entity.GetType().GetProperty("Tipoptomedicodi").GetValue(entity, null);
                reg.H1 = (decimal?)entity.GetType().GetProperty("H1").GetValue(entity, null);
                listaFinal.Add(reg);
            }
            return listaFinal;
        }

        /// <summary>
        /// Permite eliminar los valores previos cargados
        /// </summary>
        /// <param name="horizonte"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        public void EliminarValoresCargados1(int idLectura, int idFormato, int idEmpresa, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                FactorySic.GetMeMedicion1Repository().DeleteEnvioArchivo(idLectura, fechaInicio, fechaFin, idFormato, idEmpresa);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #region Configuración de Plazo de Envio de los Formatos de la Extranet

        /// <summary>
        /// Configuración de Plazo de Envio de los Formatos
        /// </summary>
        /// <param name="formato"></param>
        public static void GetSizeFormato(MeFormatoDTO formato)
        {
            switch (formato.Formatperiodo)
            {
                case ParametrosFormato.PeriodoDiario:
                    formato.RowPorDia = ParametrosFormato.ResolucionDia / (int)formato.Formatresolucion;
                    if (formato.Lecttipo == ParametrosFormato.Programado)
                    {
                        formato.FechaInicio = formato.FechaProceso.AddDays(1);
                        formato.FechaFin = formato.FechaProceso.AddDays(formato.Formathorizonte);
                    }
                    else
                    {
                        //Ejecutado o Informacion en Tiempo Real
                        formato.FechaInicio = formato.FechaProceso;
                        formato.FechaFin = formato.FechaProceso.AddDays(formato.Formathorizonte - 1);

                        //Verificar plazo/fuera de plazo especial para los formatos de hidrología Tr
                        if (EsFormatoHidrologiaTr(formato.Areacode ?? 0, formato.Formatcodi)) {
                            if (formato.FechaInicio == DateTime.Today && formato.Enviobloquehora > 0)
                            {
                                int bloque = formato.Enviobloquehora.Value;

                                formato.Formatdiaplazo = 0;
                                formato.Formatdiafinplazo = 0;

                                //18:00. En plazo desde 15:00 a 19:00. Fuera de plazo: 19:00 - Fecha y hora actual
                                formato.Formatminplazo =  (bloque - 3) * 60;
                                formato.Formatminfinplazo = (bloque + 1) * 60;
                                //formato.Formatminfinfueraplazo = ;

                            }
                        }
                    }
                    break;
                case ParametrosFormato.PeriodoSemanal:
                    formato.FechaInicio = formato.FechaProceso;
                    formato.FechaFin = formato.FechaProceso.AddDays(formato.Formathorizonte - 1);

                    switch (formato.Formatresolucion)
                    {
                        case ParametrosFormato.ResolucionCuartoHora:
                        case ParametrosFormato.ResolucionMediaHora:
                        case ParametrosFormato.ResolucionHora:
                            formato.RowPorDia = ParametrosFormato.ResolucionDia / (int)formato.Formatresolucion;
                            break;
                        case ParametrosFormato.ResolucionDia:
                            formato.RowPorDia = 1;
                            break;
                        case ParametrosFormato.ResolucionSemana:
                            formato.RowPorDia = 1;
                            break;
                    }
                    break;
                case ParametrosFormato.PeriodoMensualSemana: //Semanal Mediano Plazo
                    formato.RowPorDia = 1;

                    if (formato.Lecttipo == ParametrosFormato.Ejecutado) //Ejecutado
                    {
                        //fecha inicio es primera semana del año
                        //fecha fin es ultima semana antes del mes seleccionado
                        //si se selecciona enero se toma todo el año anterior
                        if (formato.FechaProceso.Month == 1)
                        {
                            formato.FechaInicio = EPDate.f_fechainiciosemana(formato.FechaProceso.Year - 1, 1);
                            formato.FechaFin = EPDate.f_fechainiciosemana(formato.FechaProceso.Year - 1, EPDate.TotalSemanasEnAnho(formato.FechaProceso.Year - 1, 6));
                            formato.Formathorizonte = EPDate.TotalSemanasEnAnho(formato.FechaProceso.Year - 1, 6);
                        }
                        else
                        {
                            formato.FechaInicio = EPDate.f_fechainiciosemana(formato.FechaProceso.Year, 1);
                            formato.FechaFin = EPDate.f_fechainiciosemana(formato.FechaProceso.AddDays(-4));
                            formato.Formathorizonte = EPDate.f_numerosemana(formato.FechaFin);
                        }
                    }
                    else //Programado
                    {
                        //fecha inicio es la primera semana del mes seleccionado
                        //fecha fin es la ultima semana del año
                        formato.FechaInicio = EPDate.f_fechainiciosemana(formato.FechaProceso.AddDays(3));//EPDate.f_fechainiciosemana(formato.FechaProceso.AddDays(7));
                        formato.FechaFin = formato.FechaInicio.AddDays(56 * 7);//EPDate.f_fechainiciosemana(formato.FechaProceso.AddDays(56 * 7));
                        formato.Formathorizonte = 56; //EPDate.f_numerosemana(formato.FechaFin) +
                    }
                    break;
                case ParametrosFormato.PeriodoMensual:
                    switch (formato.Formatresolucion)
                    {
                        case ParametrosFormato.ResolucionCuartoHora:
                        case ParametrosFormato.ResolucionMediaHora:
                        case ParametrosFormato.ResolucionHora:
                            formato.FechaInicio = formato.FechaProceso;
                            formato.FechaFin = formato.FechaProceso.AddMonths(1).AddDays(-1);
                            formato.Formathorizonte = ((TimeSpan)(formato.FechaFin - formato.FechaInicio)).Days + 1;
                            formato.RowPorDia = ParametrosFormato.ResolucionDia / (int)formato.Formatresolucion;
                            break;
                        case ParametrosFormato.ResolucionDia:
                            formato.FechaInicio = formato.FechaProceso;
                            formato.FechaFin = formato.FechaProceso.AddMonths(1).AddDays(-1);
                            formato.Formathorizonte = ((TimeSpan)(formato.FechaFin - formato.FechaInicio)).Days;
                            formato.RowPorDia = 1;
                            break;
                        case ParametrosFormato.ResolucionMes:
                            formato.RowPorDia = 1;
                            if (formato.Lecttipo == ParametrosLectura.Ejecutado)
                            {
                                if (formato.FechaProceso.Month == 1)
                                {
                                    formato.Formathorizonte = 12;
                                    formato.FechaInicio = new DateTime(formato.FechaProceso.Year - 1, 1, 1);
                                }
                                else
                                {
                                    formato.Formathorizonte = formato.FechaProceso.Month - 1;
                                    formato.FechaInicio = new DateTime(formato.FechaProceso.Year, 1, 1);
                                }

                                formato.FechaFin = formato.FechaProceso;
                            }
                            else // Programado
                            {
                                if (formato.Formathorizonte == 90)
                                {
                                    // Carga de Demanda Coincidente GMME-LHBN
                                    formato.FechaInicio = formato.FechaProceso;
                                    formato.FechaFin = formato.FechaProceso.AddMonths(formato.Formathorizonte);
                                    formato.Formathorizonte = 3;
                                }
                                else {
                                    
                                    formato.FechaInicio = formato.FechaProceso;
                                    formato.FechaFin = formato.FechaProceso.AddMonths(12);
                                    formato.Formathorizonte = 12;
                                }
                            }
                            break;
                    }

                    break;
            }

            formato.FechaPlazoIni = formato.FechaProceso.AddMonths(formato.Formatmesplazo).AddDays(formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
            formato.FechaPlazo = formato.FechaProceso.AddMonths(formato.Formatmesfinplazo).AddDays(formato.Formatdiafinplazo).AddMinutes(formato.Formatminfinplazo);
            formato.FechaPlazoFuera = formato.FechaProceso.AddMonths(formato.Formatmesfinfueraplazo).AddDays(formato.Formatdiafinfueraplazo).AddMinutes(formato.Formatminfinfueraplazo);
        }

        /// <summary>
        /// Verifica si un formato enviado esta en plazo o fuera de plazo
        /// </summary>
        /// <param name="formato"></param>
        /// <returns></returns>
        public bool ValidarPlazo(MeFormatoDTO formato)
        {
            bool resultado = false;
            DateTime fechaActual = DateTime.Now;

            //Validación de vigencia de empresa
            if (!this.EsEmpresaVigente(formato.Emprcodi, fechaActual))
            {
                return false;
            }

            DateTime fechaEnvio = formato.IdEnvio > 0 && formato.FechaEnvio != null ? formato.FechaEnvio.Value : fechaActual;

            if ((fechaEnvio >= formato.FechaPlazoIni) && (fechaEnvio <= formato.FechaPlazo))
            {
                resultado = true;
            }

            return resultado;
        }

        /// <summary>
        /// Verifica si un Envio de Información esta en plazo, fuera de plazo, deshabilitado
        /// </summary>
        /// <param name="plazo"></param>
        /// <returns></returns>
        public string EnvioValidarPlazo(MeFormatoDTO formato, int idEmpresa)
        {
            string resultado = ConstantesEnvio.ENVIO_PLAZO_DESHABILITADO;

            DateTime fechaValidacion = DateTime.Now;

            //Validación de vigencia de empresa
            if (!this.EsEmpresaVigente(formato.Emprcodi, fechaValidacion))
            {
                return ConstantesEnvio.ENVIO_PLAZO_DESHABILITADO;
            }

            if (formato.FechaPlazoIni <= fechaValidacion && fechaValidacion <= formato.FechaPlazoFuera)
            {
                return fechaValidacion <= formato.FechaPlazo ? ConstantesEnvio.ENVIO_EN_PLAZO : ConstantesEnvio.ENVIO_FUERA_PLAZO;
            }
            else
            {
                //buscar en ampliación
                MeAmpliacionfechaDTO regfechaPlazo = this.GetByIdMeAmpliacionfecha(formato.FechaProceso, idEmpresa, formato.Formatcodi);
                if (regfechaPlazo != null)
                {
                    if ((fechaValidacion >= formato.FechaPlazoIni) && (fechaValidacion <= regfechaPlazo.Amplifechaplazo))
                    {
                        return ConstantesEnvio.ENVIO_FUERA_PLAZO;
                    }
                }
            }

            return resultado;
        }

        /// <summary>
        /// Verifica si un Envio de Información esta en plazo, fuera de plazo, deshabilitado
        /// </summary>
        /// <param name="plazo"></param>
        /// <returns></returns>
        public string ObtenerMensajePlazo(MeFormatoDTO formato, int idEmpresa)
        {
            string resultado = string.Empty;

            DateTime fechaValidacion = DateTime.Now;

            //Validación de vigencia de empresa
            if (!this.EsEmpresaVigente(formato.Emprcodi, fechaValidacion))
            {
                return string.Empty;
            }

            if (formato.FechaPlazoIni <= fechaValidacion && fechaValidacion <= formato.FechaPlazoFuera)
            {
                string hora1 = DateTime.Now.Date == formato.FechaPlazo.Date ? formato.FechaPlazo.ToString(ConstantesAppServicio.FormatoHora) : formato.FechaPlazo.ToString(ConstantesAppServicio.FormatoFechaFull);
                string hora2 = DateTime.Now.Date == formato.FechaPlazoFuera.Date ? formato.FechaPlazoFuera.ToString(ConstantesAppServicio.FormatoHora) : formato.FechaPlazoFuera.ToString(ConstantesAppServicio.FormatoFechaFull);

                if (fechaValidacion > formato.FechaPlazo)
                    return " tolerancia hasta " + hora2;
                return "permitido hasta " + hora1 + " con tolerancia hasta " + hora2;
            }
            else
            {
                //buscar en ampliación
                MeAmpliacionfechaDTO regfechaPlazo = this.GetByIdMeAmpliacionfecha(formato.FechaProceso, idEmpresa, formato.Formatcodi);
                if (regfechaPlazo != null)
                {
                    if ((fechaValidacion >= formato.FechaPlazoIni) && (fechaValidacion <= regfechaPlazo.Amplifechaplazo))
                    {
                        string hora1 = DateTime.Now.Date == regfechaPlazo.Amplifechaplazo.Date ? regfechaPlazo.Amplifechaplazo.ToString(ConstantesAppServicio.FormatoHora) : regfechaPlazo.Amplifechaplazo.ToString(ConstantesAppServicio.FormatoFechaFull);
                        return "ampliado hasta "+ hora1;
                    }
                }
            }

            return resultado;
        }

        /// <summary>
        /// Verificar si el formato es de Tiempo real
        /// </summary>
        /// <param name="areacode"></param>
        /// <param name="formatcodi"></param>
        /// <returns></returns>
        public static bool EsFormatoHidrologiaTr(int areacode, int formatcodi)
        {
            if (areacode == 3 && (formatcodi != 41 && formatcodi != 42))
                return true;

            return false;
        }

        #endregion

        /// <summary>
        /// Permite obtener un registro de la tabla ME_LECTURA
        /// </summary>
        public MeLecturaDTO GetByIdMeLectura(int lectcodi)
        {
            return FactorySic.GetMeLecturaRepository().GetById(lectcodi);
        }

        public List<FwAreaDTO> ListAreaXFormato(int idOrigen)
        {
            try
            {
                return FactorySic.GetFwAreaRepository().ListAreaXFormato(idOrigen);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_ORIGENLECTURA
        /// </summary>
        public List<MeOrigenlecturaDTO> ListMeOrigenlecturas()
        {
            return FactorySic.GetMeOrigenlecturaRepository().List();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla FW_AREA
        /// </summary>
        public List<FwAreaDTO> ListFwAreas()
        {
            return FactorySic.GetFwAreaRepository().List();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_MODULO
        /// </summary>
        public List<MeModuloDTO> ListMeModulos()
        {
            return FactorySic.GetMeModuloRepository().List();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SI_TIPOINFORMACION
        /// </summary>
        public List<SiTipoinformacionDTO> ListSiTipoinformacions()
        {
            return FactorySic.GetSiTipoinformacionRepository().List();
        }

        /// <summary>
        /// Devuelve lista de familia
        /// </summary>
        /// <returns></returns>
        public List<EqFamiliaDTO> ListarFamilia()
        {
            return FactorySic.GetEqFamiliaRepository().List();
        }


        public List<TipoSerie> ListarTipoSerie()
        {
            return FactorySic.GetEqFamiliaRepository().ListTipoSerie();

        }

        //
        public List<TipoPuntoMedicion> ListarTipoPuntoMedicion()
        {
            return FactorySic.GetEqFamiliaRepository().ListTipoPuntoMedicion();

        }

        public List<MePtomedicionDTO> ListarPuntoMedicionPorEmpresa(int CodEmpresa, int CodTipoSerie, int CodTipoPuntoMedicion)
        {
            return FactorySic.GetEqFamiliaRepository().ListPuntoMedicionPorEmpresa(CodEmpresa, CodTipoSerie, CodTipoPuntoMedicion);

        }

        public MePtomedicionDTO GetPtoMedicionById(int CodTipoPuntoMedicion)
        {
            return FactorySic.GetEqFamiliaRepository().GetPtoMedicionById(CodTipoPuntoMedicion);

        }

        


        public List<MePtomedicionDTO> ListarPuntoMedicionPorCuenca(int cuenca, int tptomedicodi)
        {
            return FactorySic.GetEqFamiliaRepository().ListPuntoMedicionPorCuenca(cuenca, tptomedicodi);

        }
        public List<MePtomedicionDTO> ListarPuntoMedicionPorCuencaNaturalEvaporado(int cuenca)
        {
            return FactorySic.GetEqFamiliaRepository().ListarPuntoMedicionPorCuencaNaturalEvaporado(cuenca);

        }
       
        public List<MePtomedicionDTO> ListarPuntoMedicionPorCuencaNaturalEvaporadoPorTipoPuntoMedicion(int cuenca, int tipopuntomedicion)
        {
            return FactorySic.GetEqFamiliaRepository().ListarPuntoMedicionPorCuencaNaturalEvaporadoPorTipoPuntoMedicion(cuenca, tipopuntomedicion);

        }
        public List<MePtomedicionDTO> ListarPtoMedicionCuenca(int cuenca)
        {
            return FactorySic.GetEqFamiliaRepository().ListarPtoMedicionCuenca(cuenca);

        }

        public List<MePtomedicionDTO> ListarPtoMedicionCuencaPorTipoPtoMedicion(int cuenca, int tipopuntomedicion)
        {
            return FactorySic.GetEqFamiliaRepository().ListarPtoMedicionCuencaTipoPuntoMedicion(cuenca, tipopuntomedicion);

        }
        public List<TablaVertical> ListaTablaVertical(string ptomedicodi, int tptomedicodi, int tiposeriecodi, int anioinicio, int aniofin)
        {
            return FactorySic.GetEqFamiliaRepository().ListaTablaVertical(ptomedicodi, tptomedicodi, tiposeriecodi, anioinicio, aniofin);

        }

        public List<GraficoSeries> ObtenerGraficoAnual(int tiposeriecodi, int tptomedicodi, int ptomedicodi, int anioinicio, int aniofin)
        {
            return FactorySic.GetEqFamiliaRepository().ObtenerGraficoAnual(tiposeriecodi, tptomedicodi, ptomedicodi, anioinicio, aniofin);

        }
        public List<GraficoSeries> ObtenerCaudalPuntosCalculados(int tiposeriecodi, int tptomedicodi, int ptomedicodi, int anioinicio, int aniofin,  string tipoReporte, string anios)
        {
            return FactorySic.GetEqFamiliaRepository().ObtenerCaudalPuntosCalculados(tiposeriecodi, tptomedicodi, ptomedicodi, anioinicio, aniofin, tipoReporte, anios);

        }
        public List<GraficoSeries> ObtenerGraficoMensual(int tiposeriecodi, int tptomedicodi, int ptomedicodi, int anioinicio)
        {
            return FactorySic.GetEqFamiliaRepository().ObtenerGraficoMensual(tiposeriecodi, tptomedicodi, ptomedicodi, anioinicio);

        }
        public List<GraficoSeries> ObtenerGraficoComparativaVolumen(int tiposeriecodi, int tptomedicodi, int ptomedicodi, string anios)
        {
            return FactorySic.GetEqFamiliaRepository().ObtenerGraficoComparativaVolumen(tiposeriecodi, tptomedicodi, ptomedicodi, anios);

        }
        public List<GraficoSeries> ObtenerGraficoComparativaNaturalEvaporada(int tiposeriecodi, int ptomedicodi, string anioinicio)
        {
            return FactorySic.GetEqFamiliaRepository().ObtenerGraficoComparativaNaturalEvaporada(tiposeriecodi, ptomedicodi, anioinicio);

        }
        public List<GraficoSeries> ObtenerGraficoComparativaLineaTendencia(int tiposeriecodi, int tptomedicodi, string ptomedicodi, int anioinicio, int aniofin)
        {
            return FactorySic.GetEqFamiliaRepository().ObtenerGraficoComparativaLineaTendencia(tiposeriecodi, tptomedicodi, ptomedicodi, anioinicio, aniofin);

        }
        

        public List<GraficoSeries> ObtenerGraficoTotal(int tiposeriecodi, int tptomedicodi, int ptomedicodi, int aniofin)
        {
            return FactorySic.GetEqFamiliaRepository().ObtenerGraficoTotal(tiposeriecodi, tptomedicodi, ptomedicodi, aniofin);

        }
        public List<GraficoSeries> ObtenerGraficoTotalNaturalEvaporada(int tiposeriecodi, int ptomedicodi, int aniofin)
        {
            return FactorySic.GetEqFamiliaRepository().ObtenerGraficoTotalNaturalEvaporada(tiposeriecodi, ptomedicodi, aniofin);

        }
        public List<GraficoSeries> ObtenerGraficoTotalLineaTendencia(int tiposeriecodi, string ptomedicodi, int aniofin, int tptomedicodi)
        {
            return FactorySic.GetEqFamiliaRepository().ObtenerGraficoTotalLineaTendencia(tiposeriecodi, ptomedicodi, aniofin, tptomedicodi);

        }
        public List<GraficoSeries> ObtenerGraficoEstadisticasAnuales(int tiposeriecodi, int tptomedicodi, int ptomedicodi, int anioinicio, int mesinicio, int aniofin, int mesfin)
        {
            return FactorySic.GetEqFamiliaRepository().ObtenerGraficoEstadisticasAnuales(tiposeriecodi, tptomedicodi, ptomedicodi, anioinicio, mesinicio, aniofin, mesfin);

        }



        /// <summary>
        /// Lista Familia By Origen Lectura y Empresa
        /// </summary>
        /// <param name="origlectcodi"></param>
        /// <param name="famcodi"></param>
        /// <returns></returns>
        public List<EqFamiliaDTO> ObtenerFamiliaPorOrigenLecturaEquipo(int origlectcodi, int emprcodi)
        {
            return FactorySic.GetEqFamiliaRepository().ListarFamiliaPorOrigenLecturaEquipo(origlectcodi, emprcodi);
        }

        /// <summary>
        /// Devuelve lista de familia xEmpresa
        /// </summary>
        /// <returns></returns>
        public List<EqFamiliaDTO> ListarFamiliaXEmp(int idEmpresa)
        {
            return FactorySic.GetEqFamiliaRepository().ListarFamiliaXEmp(idEmpresa).Where(x => x.Famcodi > 0).ToList();
        }

        /// <summary>
        /// Permite listar las categorias de grupos
        /// </summary>
        /// <returns></returns>
        public List<PrCategoriaDTO> ListarTipoGrupoPorOrigenLecturaYEmpresa(int origlectcodi, int emprcodi)
        {
            return FactorySic.GetPrCategoriaRepository().ListByOriglectcodiYEmprcodi(origlectcodi, emprcodi);
        }

        /// <summary>
        /// Permite listar las categorias de grupos
        /// </summary>
        /// <returns></returns>
        public List<PrCategoriaDTO> ListarTipoGrupo()
        {
            return FactorySic.GetPrCategoriaRepository().List();
        }

        /// <summary>
        /// Devuelve lista de equipo por empresa y familia
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="famcodi"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> GetByCriteriaEqequipo(int emprcodi, int famcodi)
        {
            return FactorySic.GetEqEquipoRepository().GetByEmprFam(emprcodi, famcodi);
        }

        /// <summary>
        /// Permite obtener los equipos por familia
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="famcodi"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> ObtenerEquiposPorFamilia(int emprcodi, int famcodi)
        {
            List<EqEquipoDTO> list = FactorySic.GetEqEquipoRepository().ObtenerEquipoPorFamilia(emprcodi, famcodi);

            if (list.Count > 0)
            {
                int max = list.Select(x => x.AREANOMB.Length).Max();

                foreach (EqEquipoDTO item in list)
                {
                    int count = max - item.AREANOMB.Length;
                    string espacio = string.Empty;
                    for (int i = 0; i <= count; i++)
                    {
                        espacio = espacio + "-";
                    }


                    item.Equinomb = item.AREANOMB + espacio + " " + item.Equinomb;
                }

                return list.OrderBy(x => x.Equinomb).ToList();
            }

            return new List<EqEquipoDTO>();

        }

        public List<MePtomedicionDTO> GetByIdEquipoMePtomedicion(int codigo, int opcion)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            try
            {
                if (opcion == 1)
                {
                    entitys = FactorySic.GetMePtomedicionRepository().GetByIdEquipo(codigo);
                }
                else
                {
                    entitys = FactorySic.GetMePtomedicionRepository().GetByIdGrupo(codigo);
                }
            }
            catch (Exception ex)
            {
                var result = ex.Message;
            }
            return entitys;
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla  
        /// </summary>
        public List<MeTipopuntomedicionDTO> ListMeTipopuntomedicions(string origlectcodi)
        {
            if (string.IsNullOrEmpty(origlectcodi)) origlectcodi = ConstantesAppServicio.ParametroDefecto;
            return FactorySic.GetMeTipopuntomedicionRepository().List(origlectcodi);
        }



        /// <summary>
        /// Lista pto de medicion duplicados
        /// </summary>
        /// <param name="equipo"></param>
        /// <param name="origen"></param>
        /// <param name="tipopto"></param>
        /// <returns></returns>
        public List<MePtomedicionDTO> ListarPtoMedicionDuplicados(int equipo, int origen, int tipopto)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            try
            {
                entitys = FactorySic.GetMePtomedicionRepository().ListarPtoDuplicado(equipo, origen, tipopto);
            }
            catch (Exception ex)
            {
                var result = ex.Message;
            }
            return entitys;
        }

        /// <summary>
        /// Lista pto de medicion duplicados por nombrepto y empresa
        /// </summary>
        /// <param name="nombrepto"></param>
        /// <param name="empresacodi"></param>
        /// <returns></returns>
        public List<MePtomedicionDTO> ListarPtoMedicionDuplicadoNombreEmpresa(string nombrepto, int empresacodi)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            try
            {
                entitys = FactorySic.GetMePtomedicionRepository().ListarPtoDuplicadoNombreEmpresa(nombrepto, empresacodi);
            }
            catch (Exception ex)
            {
                var result = ex.Message;
            }
            return entitys;
        }

        

        /// <summary>
        /// Lista pto de medicion duplicados
        /// </summary>
        /// <param name="equipo"></param>
        /// <param name="origen"></param>
        /// <param name="tipopto"></param>
        /// <returns></returns>
        public List<MePtomedicionDTO> ListarPtoMedicionDuplicadosGrupo(int grupo, int origen, int tipopto)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            try
            {
                entitys = FactorySic.GetMePtomedicionRepository().ListarPtoDuplicadoGrupo(grupo, origen, tipopto);
            }
            catch (Exception ex)
            {
                var result = ex.Message;
            }
            return entitys;
        }

        /// <summary>
        /// Busca pto de medicion por id
        /// </summary>
        /// <param name="ptomedicodi"></param>
        /// <returns></returns>
        public MePtomedicionDTO GetByIdMePtomedicion(int ptomedicodi)
        {
            return FactorySic.GetMePtomedicionRepository().GetById(ptomedicodi);
        }

        /// <summary>
        /// Devuelve lista de equipo por id equipo
        /// </summary>
        /// <param name="equicodi"></param>
        /// <returns></returns>
        public EqEquipoDTO GetByIdEqequipo(int equicodi)
        {
            EqEquipoDTO equipo = new EqEquipoDTO();
            equipo = FactorySic.GetEqEquipoRepository().GetById(equicodi);
            return equipo;
        }

        /// <summary>
        /// Graba entidad pto de medicio
        /// </summary>
        /// <param name="ptoMedicion"></param>
        public int SavePtoMedicion(MePtomedicionDTO ptoMedicion)
        {
            try
            {
                return FactorySic.GetMePtomedicionRepository().Save(ptoMedicion);

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// Actualiza pto de medicion
        /// </summary>
        /// <param name="entity"></param>
        public void UpdatePtoMedicion(MePtomedicionDTO entity)
        {
            try
            {
                FactorySic.GetMePtomedicionRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// Permite eliminar un punto de medicion
        /// </summary>
        /// <param name="idPtoMedicion"></param>
        /// <returns></returns>
        public int EliminarPuntoMedicion(int idPtoMedicion, string username)
        {
            try
            {
                int result = -1;
                int suma = FactorySic.GetMePtomedicionRepository().VerificarRelaciones(idPtoMedicion);

                if (suma == 0)
                {
                    FactorySic.GetMePtomedicionRepository().Delete(idPtoMedicion);
                    FactorySic.GetMePtomedicionRepository().Delete_UpdateAuditoria(idPtoMedicion, username);
                    result = 1;
                }
                else
                {
                    result = 2;
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Enví correo cuando se crea o elimina un punto de medición para un usuario libre
        /// </summary>
        /// <param name="siEmpresaDTO"></param>
        /// <param name="mePtomedicionDTO"></param>
        /// <param name="accion"></param>
        public void enviarCorreoUsuarioLibrePorCreacionEliminacion(COES.Dominio.DTO.Sic.SiEmpresaDTO siEmpresaDTO, MePtomedicionDTO mePtomedicionDTO, string accion = "")
        {
            try
            {
                MailMessage correo = new MailMessage();
                string remitente = ConfigurationManager.AppSettings["MailFrom"];
                correo.From = new MailAddress(remitente);
                correo.To.Add(ConfigurationManager.AppSettings["EmpresaUsuarioLibreCorreoNotificacionRegistroEliminacionPuntoMedicion"]);
                //correo.Bcc.Add("webapp@coes.org.pe");
                correo.Subject = accion == "NUEVO" ? "USUARIO LIBRE: REGISTRO DE PUNTO DE MEDICIÓN" : "USUARIO LIBRE: ELIMINACIÓN DE PUNTO DE MEDICIÓN";
                correo.Body = accion == "NUEVO" ? "USUARIO LIBRE: SE HA REGISTRADO PARA LA EMPRESA " + siEmpresaDTO.Emprrazsocial + "(" + siEmpresaDTO.Emprruc + ") EL PUNTO DE MEDICIÓN " + mePtomedicionDTO.Ptomedielenomb + "(" + mePtomedicionDTO.Ptomedicodi + ")." : "USUARIO LIBRE: SE HA ELIMINADO PARA LA EMPRESA " + siEmpresaDTO.Emprrazsocial + "(" + siEmpresaDTO.Emprruc + ") EL PUNTO DE MEDICIÓN " + mePtomedicionDTO.Ptomedidesc + "(" + mePtomedicionDTO.Ptomedicodi + ").";
                correo.IsBodyHtml = true;
                correo.Priority = MailPriority.Normal;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = ConfigurationManager.AppSettings["EmailServer"];

                smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["UserNameSMTP"],
                    ConfigurationManager.AppSettings["PasswordSMTP"]);

                smtp.Send(correo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener las substaciones de los puntos de medicion
        /// </summary>
        /// <returns></returns>
        public List<EqAreaDTO> ObtenerAreasPuntosMedicion()
        {
            return FactorySic.GetMePtomedicionRepository().ObtenerAreasFiltro();
        }


        /// <summary>
        /// Devuelve lista de punto de medicion
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="idsOriglectura"></param>
        /// <param name="idsTipoptomedicion"></param>
        /// <returns></returns>
        public List<MePtomedicionDTO> ListarPtoMedicion(string empresas, string idsOriglectura, string idsTipoptomedicion)
        {
            if (string.IsNullOrEmpty(empresas)) empresas = ConstantesAppServicio.ParametroNulo;
            if (string.IsNullOrEmpty(idsOriglectura)) idsOriglectura = ConstantesAppServicio.ParametroNulo;
            if (string.IsNullOrEmpty(idsTipoptomedicion)) idsTipoptomedicion = ConstantesAppServicio.ParametroDefecto;
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            try
            {
                entitys = FactorySic.GetMePtomedicionRepository().GetByCriteria(empresas, idsOriglectura, idsTipoptomedicion);
            }
            catch (Exception ex)
            {
                var result = ex.Message;
            }
            return entitys;
        }

        /// <summary>
        ///  Permite generar lista de medicion por intervalos desde lista de cambioenvio
        /// </summary>
        /// <param name="enviocodi"></param>
        /// <returns></returns>
        public List<MeMedicionxintervaloDTO> GetEnvioCambioMedicionXIntervalo(int enviocodi)
        {
            List<MeMedicionxintervaloDTO> listaDatos = new List<MeMedicionxintervaloDTO>();
            MeMedicionxintervaloDTO registro = null;
            var lista = FactorySic.GetMeCambioenvioRepository().GetById(enviocodi);
            foreach (var reg in lista)
            {
                registro = new MeMedicionxintervaloDTO();

                var campos = reg.Cambenvdatos.Split('#');
                if (campos.Length > 0)
                {
                    decimal valor;
                    if (decimal.TryParse(campos[0], out valor))
                    {
                        registro.Medinth1 = valor;
                        registro.Medintfechaini = reg.Cambenvfecha;
                        registro.Medintfechafin = (DateTime)reg.Lastdate;//ADD
                        registro.Ptomedicodi = reg.Ptomedicodi;
                        listaDatos.Add(registro);
                    }
                }
                if (campos.Length > 1)
                    registro.Medintdescrip = campos[1];
                int estado = 0;
                if (campos.Length > 2)
                {
                    int.TryParse(campos[2], out estado);
                }
                registro.Medestcodi = estado;
            }
            return listaDatos;
        }
        /// <summary>
        /// permite contar la cantidad de puntos de medicion
        /// </summary>       
        /// <summary>
        /// permite contar la cantidad de puntos de medicion
        /// </summary>       
        public int GetTotalPtomedicion(string empresas, string idsOriglectura, string idsTipoptomedicion, string idsFamilia,
          string ubicacion, string categoria, int tipoPunto, int? cliente, int? barra, int codigo)
        {
            if (string.IsNullOrEmpty(empresas)) empresas = ConstantesAppServicio.ParametroNulo;
            if (string.IsNullOrEmpty(idsOriglectura)) idsOriglectura = ConstantesAppServicio.ParametroNulo;
            if (string.IsNullOrEmpty(idsTipoptomedicion)) idsTipoptomedicion = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsFamilia)) idsFamilia = ConstantesAppServicio.ParametroNulo;
            if (string.IsNullOrEmpty(ubicacion)) ubicacion = ConstantesAppServicio.ParametroNulo;
            if (string.IsNullOrEmpty(categoria)) categoria = ConstantesAppServicio.ParametroNulo;
            if (cliente == null) cliente = int.Parse(ConstantesAppServicio.ParametroNulo);
            if (barra == null) barra = int.Parse(ConstantesAppServicio.ParametroNulo);

            if (tipoPunto == 3)
            {
                idsOriglectura = ConstantesFormatoMedicion.IdOrigenLecturaTransferencias.ToString();
            }

            return FactorySic.GetMePtomedicionRepository().ObtenerTotalPtomedicion(empresas, idsOriglectura, idsTipoptomedicion, idsFamilia,
                ubicacion, categoria, tipoPunto, codigo, cliente, barra);
        }

        /// <summary>
        /// Devuelve lista de puntos de medicion 
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="idsOriglectura"></param>
        /// <param name="idsTipoptomedicion"></param>
        /// <param name="nroPaginas"></param>
        /// <param name="pageSize"></param>
        /// <param name="idsFamilia"></param>
        /// <returns></returns>
        public List<MePtomedicionDTO> ListarDetallePtoMedicion(string empresas, string idsOriglectura, string idsTipoptomedicion,
           int nroPaginas, int pageSize, string idsFamilia, string ubicacion, string categoria, int tipoPunto, int codigo,
           int? cliente, int? barra, string campo, string orden)
        {
            if (string.IsNullOrEmpty(empresas)) empresas = ConstantesAppServicio.ParametroNulo;
            if (string.IsNullOrEmpty(idsOriglectura)) idsOriglectura = ConstantesAppServicio.ParametroNulo;
            if (string.IsNullOrEmpty(idsTipoptomedicion)) idsTipoptomedicion = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsFamilia)) idsFamilia = ConstantesAppServicio.ParametroNulo;
            if (string.IsNullOrEmpty(ubicacion)) ubicacion = ConstantesAppServicio.ParametroNulo;
            if (string.IsNullOrEmpty(categoria)) categoria = ConstantesAppServicio.ParametroNulo;
            if (cliente == null) cliente = int.Parse(ConstantesAppServicio.ParametroNulo);
            if (barra == null) barra = int.Parse(ConstantesAppServicio.ParametroNulo);

            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            try
            {
                if (tipoPunto == 3)
                {
                    idsOriglectura = ConstantesFormatoMedicion.IdOrigenLecturaTransferencias.ToString();
                }

                entitys = FactorySic.GetMePtomedicionRepository().ListarDetallePtoMedicionFiltro(empresas, idsOriglectura, idsTipoptomedicion,
                    nroPaginas, pageSize, idsFamilia, ubicacion, categoria, tipoPunto, codigo, cliente, barra, campo, orden);

                foreach (var regPto in entitys)
                {
                    if (regPto.Origlectcodi == 21 && regPto.Areacodi > 0)
                    {
                        regPto.AreaOperativa = regPto.Areanomb != null ? regPto.Areanomb.Trim() : ""; 
                    }

                    if (regPto.Origlectcodi == 22)//PMPO
                    {
                        regPto.Ptomedielenomb = (regPto.Ptomedielenomb ?? "") + " - " + (regPto.Ptomedidesc ?? "");
                    }
                }
            }
            catch (Exception ex)
            {
                var result = ex.Message;
            }
            return entitys;
        }

        /// <summary>
        /// Listar las Centrales por formato y empresa
        /// </summary>
        /// <param name="listaPto"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> ListCentralByEmpresaAndFormato(List<MeHojaptomedDTO> listaPto)
        {
            var listaEq = listaPto.GroupBy(x => new { x.Equipadre, x.Equipopadre, x.Famcodi }).Select(
                    grp => new EqEquipoDTO { Equicodi = grp.Key.Equipadre, Equinomb = grp.Key.Equipopadre, Famcodi = grp.Key.Famcodi }).ToList();

            return listaEq;
        }

        #region Métodos Tabla ME_SCADA_SP7
        /// <summary>
        /// Se obtiene los datos enviados de un formato de la BD de me_medicon48.
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="formato"></param>
        /// <param name="idEnvio"></param>
        /// <returns></returns>
        public List<MeScadaSp7DTO> GetListaDataFormatoScada(int idFormato, int idEmpresa, DateTime Fechaini, DateTime FechaFin)
        {
            return FactoryScada.GetMeScadaSp7Repository().GetDatosScadaAFormato(idFormato, idEmpresa, Fechaini, FechaFin, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto);

        }

        //inicio agregado
        public List<MeScadaSp7DTO> GetByCriteriaFormatoScada(DateTime fechainicio, DateTime fechafin, int formatcodi, int tipoinfocodi, int ptomedicodi)
        {
            return FactoryScada.GetMeScadaSp7Repository().GetByCriteria(fechainicio, fechafin, formatcodi, tipoinfocodi, ptomedicodi);
        }
        //fin agregado

        #endregion

        #region Metodos tabla EVE_MANTTO



        /// <summary>
        /// Obtiene Lista de Mantos para cruce con hoja excel web
        /// </summary>
        /// <param name="listaMantenimientoAux"></param>
        /// <param name="listaTop"></param>
        /// <param name="ListaBloqueManto"></param>
        /// <param name="listaEquipos"></param>
        /// <param name="dfecha"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ListManttoEquipoFecha2(ref List<EveManttoDTO> listaMantenimientoAux, ref List<EqEquirelDTO> listaTop, List<ManttoBloque> ListaBloqueManto, List<int> listaEquipos, DateTime dfecha)
        {
            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>();
            MeMedicion48DTO registro;
            List<MeMedicion48DTO> matrizMantenimento;
            List<EveManttoDTO> mantos;
            string equiposManto = "";
            if (listaEquipos.Count == 0)
            {
                return lista;
            }
            for (int k = 0; k < listaEquipos.Count; k++)
            {
                var equipoCol = listaEquipos[k];
                var listaAgrup = GetListEquiposAgrup(listaEquipos[k]); // Lista de grupos de equipos que estan conectados al equipo principal
                listaTop = listaTop.Union(listaAgrup).ToList();
                string idsEquipo = String.Join(",", listaAgrup.Select(x => x.Equicodi2).Distinct().ToList());
                string equipoManto = (idsEquipo == "") ? listaEquipos[k].ToString() : idsEquipo + "," + listaEquipos[k].ToString();
                if (equiposManto != "")
                    equiposManto += "," + equipoManto;
                else
                    equiposManto += equipoManto;
            }
            listaMantenimientoAux = servicioMCP.ListManttoEquipoFecha(equiposManto, 1, dfecha);
            for (int k = 0; k < listaEquipos.Count; k++)
            {
                registro = new MeMedicion48DTO();
                registro.Lectcodi = 0;
                registro.Equicodi = listaEquipos[k];
                //List<EqEquirelDTO> equiposAgrup = new List<EqEquirelDTO>();
                var equiposAgrup = GetListEquiposAgrup(listaEquipos[k]); // Lista de grupos de equipos que estan conectados al equipo principal
                //string idsEquipo = String.Join(",", equiposAgrup.Select(x => x.Equicodi2).Distinct().ToList());
                //string equipoManto = (idsEquipo == "") ? listaEquipos[k].ToString() : idsEquipo + "," + listaEquipos[k].ToString();
                mantos = listaMantenimientoAux.Where(x => x.Equicodi == listaEquipos[k]).ToList();
                foreach (var obj2 in mantos) // buscamos todos los mantenimientos para el equipo asociado
                {
                    var horainicio = ((DateTime)obj2.Evenini).Hour;
                    var horafin = ((DateTime)obj2.Evenfin).Hour;
                    var mininicio = ((DateTime)obj2.Evenini).Minute;
                    var minfin = ((DateTime)obj2.Evenfin).Minute;

                    int salto = mininicio > 30 ? 2 : 1;
                    var posIni = 2 * horainicio + salto;
                    //if (posIni == 0)
                    //    posIni++;
                    salto = minfin > 30 ? 1 : 0;
                    var posFin = 2 * horafin + salto;
                    if (horafin == 0)
                    {
                        horafin = 23;
                        minfin = 59;
                        posFin = 48;
                    }
                    for (int indice = posIni; indice <= posFin; indice++)
                    {
                        registro.GetType().GetProperty("H" + (indice).ToString()).SetValue(registro, (decimal?)1); // 1: hora encendida
                        ManttoBloque mantoBloque = new ManttoBloque();
                        mantoBloque.ListaManto = new List<int>();
                        //mantoBloque.ListaManto.Add(listaEquipos[k]);
                        mantoBloque.ListaManto.Add(obj2.Manttocodi);
                        mantoBloque.Bloque = indice;
                        mantoBloque.Equicodi = listaEquipos[k];
                        ListaBloqueManto.Add(mantoBloque);
                    }
                }

                matrizMantenimento = new List<MeMedicion48DTO>(); // Matriz de horas de mantenimeinto de todos los equipos conectados al equipo principal
                MeMedicion48DTO entity;
                // buscamos mantenimiento de los equipos asociados
                foreach (var obj in equiposAgrup)
                {
                    entity = new MeMedicion48DTO();
                    entity.Equicodi = obj.Equicodi2;
                    entity.Lectcodi = obj.Equirelagrup; // almacenamos el codigo del grupo
                    mantos = listaMantenimientoAux.Where(x => x.Equicodi == obj.Equicodi2).ToList();
                    foreach (var obj2 in mantos) // buscamos todos los mantenimientos para el equipo asociado
                    {
                        var horainicio = ((DateTime)obj2.Evenini).Hour;
                        var horafin = ((DateTime)obj2.Evenfin).Hour;
                        var mininicio = ((DateTime)obj2.Evenini).Minute;
                        var minfin = ((DateTime)obj2.Evenfin).Minute;
                        int salto = mininicio > 30 ? 2 : 1;
                        var posIni = 2 * horainicio + salto;
                        salto = minfin > 30 ? 1 : 0;
                        var posFin = 2 * horafin + salto;
                        if (horafin == 0)
                        {
                            horafin = 23;
                            minfin = 59;
                            posFin = 48;
                        }
                        for (int indice = posIni; indice <= posFin; indice++)
                        {
                            entity.GetType().GetProperty("H" + (indice).ToString()).SetValue(entity, (decimal?)obj2.Manttocodi); // 1: hora encendida
                        }
                    }
                    matrizMantenimento.Add(entity);
                }
                //********
                //listaMantenimiento = VerificamosMatrizMantenimiento(MatrizMantenimento); // funcion que verifica si un tipo de grupo esta completo en mantenimiento
                //modificaMatrices2(ref matrizEstado, ref matrizTipoEstado, listaMantenimiento, model.ListaHojaPto, model.FilasCabecera);
                for (int i = 1; i <= 48; i++)
                {
                    decimal? valor = (decimal?)registro.GetType().GetProperty("H" + i.ToString()).GetValue(registro, null);
                    if (valor == null)
                    {
                        var listGrupo = equiposAgrup.Select(x => x.Equirelagrup).Distinct().ToList();
                        foreach (var r in listGrupo)
                        {
                            var grupo = matrizMantenimento.Where(x => x.Lectcodi == r);
                            List<int> listamanto = new List<int>();
                            bool enManto = true;
                            foreach (var f in grupo)
                            {
                                decimal? valor2 = (decimal?)f.GetType().GetProperty("H" + i.ToString()).GetValue(f, null);
                                if (valor2 == null)
                                {
                                    enManto = false;
                                    break;
                                }
                                else
                                {
                                    listamanto.Add((int)valor2);
                                }
                            }
                            if (enManto)
                            {
                                registro.GetType().GetProperty("H" + i.ToString()).SetValue(registro, (decimal?)1);
                                ManttoBloque mantoBloque = new ManttoBloque();
                                mantoBloque.ListaManto = listamanto;
                                mantoBloque.Bloque = i;
                                mantoBloque.Equicodi = listaEquipos[k];
                                ListaBloqueManto.Add(mantoBloque);
                            }
                        }
                    }
                }
                lista.Add(registro);
            }

            return lista;
        }

        public List<MeMedicion48DTO> ListEventoEquipoFecha(ref List<EveEventoDTO> listaEventoAux, List<int> listaEquipos, DateTime dfecha)
        {
            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>();
            MeMedicion48DTO registro;
            List<EveEventoDTO> evento;
            if (listaEquipos.Count == 0)
            {
                return lista;
            }
            string idsEquipo = String.Join(",", listaEquipos);
            int evenclasecodi = 1;
            listaEventoAux = FactorySic.GetEveEventoRepository().ObtenerEventoEquipo(idsEquipo, dfecha, dfecha.AddDays(1), evenclasecodi).ToList();
            listaEventoAux = listaEventoAux.Where(x => x.Eveninterrup == ConstantesAppServicio.SI && x.Evenpreliminar == ConstantesAppServicio.NO).ToList();
            //borrar
            if (listaEventoAux.Count > 0)
                listaEventoAux[0].Evenfin = ((DateTime)listaEventoAux[0].Evenfin).AddMinutes(120);
            // fin borrar
            for (int k = 0; k < listaEquipos.Count; k++)
            {
                registro = new MeMedicion48DTO();
                registro.Lectcodi = 0;
                registro.Equicodi = listaEquipos[k];
                evento = listaEventoAux.Where(x => x.Equicodi == listaEquipos[k]).ToList();
                foreach (var obj2 in evento) // buscamos todos los eventos para el equipo asociado
                {
                    var horainicio = ((DateTime)obj2.Evenini).Hour;
                    var horafin = ((DateTime)obj2.Evenfin).Hour;
                    var mininicio = ((DateTime)obj2.Evenini).Minute;
                    var minfin = ((DateTime)obj2.Evenfin).Minute;

                    int salto = mininicio > 30 ? 2 : 1;
                    var posIni = 2 * horainicio + salto;
                    salto = minfin > 30 ? 1 : 0;
                    var posFin = 2 * horafin + salto;
                    if (horafin == 0)
                    {
                        horafin = 23;
                        minfin = 59;
                        posFin = 48;
                    }
                    for (int indice = posIni; indice <= posFin; indice++)
                    {
                        registro.GetType().GetProperty("H" + (indice).ToString()).SetValue(registro, (decimal?)1); // 1: hora encendida
                    }
                }
                lista.Add(registro);
            }
            return lista;
        }

        /// <summary>
        /// Obtener la disponibilidad de los equipos
        /// </summary>
        /// <param name="listaMantenimientoAux"></param>
        /// <param name="listaTop"></param>
        /// <param name="ListaBloqueManto"></param>
        /// <param name="listaEquipos"></param>
        /// <param name="dfecha"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ListManttoEquipoFechaDisponibilidad(ref List<EveManttoDTO> listaMantenimientoAux, ref List<EqEquirelDTO> listaTop, List<ManttoBloque> ListaBloqueManto, List<int> listaEquipos, DateTime dfecha)
        {
            List<EqCircuitoDTO> listaCircuitosBD = servicioMCP.ListEqCircuitos();
            List<EqCircuitoDetDTO> listaCircuitosDetBD = servicioMCP.ListEqCircuitoDets();
            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>();
            MeMedicion48DTO registro;
            List<MeMedicion48DTO> matrizMantenimento;
            List<EveManttoDTO> mantos;
            string equiposManto = "";
            List<int> listaEquicodis = new List<int>();
            if (listaEquipos.Count == 0)
            {
                return lista;
            }
            for (int k = 0; k < listaEquipos.Count; k++)
            {
                var equipoCol = listaEquipos[k];

                var listaAgrup = servicioMCP.ObtenerEquiposEnCircuito(equipoCol, dfecha, listaCircuitosBD, listaCircuitosDetBD);

                listaEquicodis.AddRange(listaAgrup);

            }
            listaEquicodis = listaEquicodis.Distinct().ToList();
            equiposManto = string.Join(",", listaEquicodis);
            int evenClaseCodi = 1;
            listaMantenimientoAux = servicioMCP.ListManttoEquipoFecha(equiposManto, evenClaseCodi, dfecha);

            for (int k = 0; k < listaEquipos.Count; k++)
            {
                int idDelEquipo = listaEquipos[k];

                registro = new MeMedicion48DTO();
                registro.Lectcodi = 0;
                registro.Equicodi = idDelEquipo;

                var equiposAgrup = servicioMCP.ObtenerEquiposEnCircuito(idDelEquipo, dfecha, listaCircuitosBD, listaCircuitosDetBD);

                mantos = listaMantenimientoAux.Where(x => x.Equicodi == idDelEquipo).ToList();
                foreach (var obj2 in mantos) // buscamos todos los mantenimientos para el equipo asociado
                {
                    int diaini = ((DateTime)obj2.Evenini).Day;
                    int diafin = ((DateTime)obj2.Evenfin).Day;
                    var horainicio = ((DateTime)obj2.Evenini).Hour;
                    var horafin = ((DateTime)obj2.Evenfin).Hour;
                    var mininicio = ((DateTime)obj2.Evenini).Minute;
                    var minfin = ((DateTime)obj2.Evenfin).Minute;

                    int posDia = diaini < diafin ? 1 : 0;
                    int salto = mininicio > 30 ? 2 : 1;
                    var posIni = 2 * horainicio + salto;
                    //if (posIni == 0)
                    //    posIni++;
                    salto = minfin > 30 ? 1 : 0;
                    var posFin = 2 * horafin + salto;
                    if (horafin == 0)
                    {
                        if(posDia == 1)
                        {
                            horafin = 23;
                            minfin = 59;
                            posFin = 48;
                        }
                        else posIni = 0;
                    }
                    for (int indice = posIni; indice <= posFin; indice++)
                    {
                        if(indice > 0)
                        {
                            registro.GetType().GetProperty("H" + (indice).ToString()).SetValue(registro, (decimal?)1); // 1: hora encendida
                            ManttoBloque mantoBloque = new ManttoBloque();
                            mantoBloque.ListaManto = new List<int>();
                            mantoBloque.ListaManto.Add(obj2.Manttocodi);
                            mantoBloque.Bloque = indice;
                            mantoBloque.Equicodi = listaEquipos[k];
                            ListaBloqueManto.Add(mantoBloque);
                        }
                    }
                }

                matrizMantenimento = new List<MeMedicion48DTO>(); // Matriz de horas de mantenimeinto de todos los equipos conectados al equipo principal
                matrizMantenimento = servicioMCP.ObtenerMantenimientosEquiposDependientes(equiposAgrup, listaMantenimientoAux);

                MeMedicion48DTO VectorDisponibilidad = new MeMedicion48DTO();

                // hallamos su disponibilidad  -- * null (sin mantenimiento)  /   * 1 (hay mantenimiento)
                List<int> listaMantenimientosDentro = new List<int>();
                VectorDisponibilidad = servicioMCP.ObtenerDisponibilidadPorEquipo(idDelEquipo, matrizMantenimento, dfecha, listaCircuitosBD, listaCircuitosDetBD, ref listaMantenimientosDentro);

                for (int i = 1; i <= 48; i++)
                {
                    decimal? valor = (decimal?)registro.GetType().GetProperty("H" + i.ToString()).GetValue(registro, null);
                    if (valor == null)
                    {
                        decimal? valorObtenido = (decimal?)VectorDisponibilidad.GetType().GetProperty("H" + i.ToString()).GetValue(VectorDisponibilidad, null);
                        registro.GetType().GetProperty("H" + i.ToString()).SetValue(registro, valorObtenido);

                        //Agregamos  los mentenimientos de los equipos dependientes
                        if (valorObtenido == 1)
                        {

                            ManttoBloque mantoBloque = new ManttoBloque();
                            mantoBloque.ListaManto = new List<int>();
                            mantoBloque.ListaManto = listaMantenimientosDentro;
                            mantoBloque.Bloque = i;
                            mantoBloque.Equicodi = listaEquipos[k];
                            ListaBloqueManto.Add(mantoBloque);
                        }
                    }
                }

                lista.Add(registro);
            }

            return lista;
        }

        #endregion

        #region metodos tabla Eq_Equirel

        public List<EqEquirelDTO> GetListEquiposAgrup(int equicodi)
        {
            return FactorySic.GetEqEquirelRepository().GetByCriteria(equicodi, "26");
        }
        #endregion

        #region Métodos Tabla ME_JUSTIFICACION

        /// <summary>
        /// Inserta un registro de la tabla ME_JUSTIFICACION
        /// </summary>
        public void SaveMeJustificacion(MeJustificacionDTO entity)
        {
            try
            {
                FactorySic.GetMeJustificacionRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ME_JUSTIFICACION
        /// </summary>
        public void UpdateMeJustificacion(MeJustificacionDTO entity)
        {
            try
            {
                FactorySic.GetMeJustificacionRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla ME_JUSTIFICACION
        /// </summary>
        public void DeleteMeJustificacion()
        {
            try
            {
                FactorySic.GetMeJustificacionRepository().Delete();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ME_JUSTIFICACION - ASSETEC 201909: se agrego el id
        /// </summary>
        public MeJustificacionDTO GetByIdMeJustificacion(int justcodi)
        {
            return FactorySic.GetMeJustificacionRepository().GetById(justcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_JUSTIFICACION
        /// </summary>
        public List<MeJustificacionDTO> ListMeJustificacions()
        {
            return FactorySic.GetMeJustificacionRepository().List();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_JUSTIFICACION
        /// </summary>
        public List<MeJustificacionDTO> ListMeJustificacionsByEnvio(int idEnvio)
        {
            return FactorySic.GetMeJustificacionRepository().ListByIdEnvio(idEnvio);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla MeJustificacion
        /// </summary>
        public List<MeJustificacionDTO> GetByCriteriaMeJustificacions()
        {
            return FactorySic.GetMeJustificacionRepository().GetByCriteria();
        }

        public List<EveSubcausaeventoDTO> GetListaJustificacion()
        {
            var lista = FactorySic.GetEveSubcausaeventoRepository().List();
            var listajust = lista.Where(x => x.Causaevencodi == ConstantesFormatoMedicion.IdCausaJustificacion).ToList();
            listajust.Add(new EveSubcausaeventoDTO() { Subcausacodi = -1, Subcausadesc = "Otro" });
            return listajust;
        }

        #endregion

        #region Demanda Diaria

        /// <summary>
        /// Listar todos los formatos de demanda diaria
        /// </summary>
        /// <returns></returns>
        public List<MeFormatoDTO> ListarFormatosDemandaDiaria()
        {
            var listaFormato = this.ListMeFormatos();
            var listaFiltroFormato = new List<MeFormatoDTO>();
            listaFiltroFormato.Add(listaFormato.Where(x => x.Formatcodi == ConstantesIEOD.IdFormatoDemandaDiaria).FirstOrDefault());
            listaFiltroFormato.Add(listaFormato.Where(x => x.Formatcodi == ConstantesIEOD.IdFormatoDemandaSemanal).FirstOrDefault());
            listaFiltroFormato.Add(listaFormato.Where(x => x.Formatcodi == ConstantesIEOD.IdFormatoDemandaMensual).FirstOrDefault());
            listaFiltroFormato.Add(listaFormato.Where(x => x.Formatcodi == ConstantesIEOD.IdFormatoDemandaRDOPrevista).FirstOrDefault());

            return listaFiltroFormato;
        }

        #endregion

        #region Fuente de Energía Primaria de las Unidades RER

        /// <summary>
        /// Listar todos los formatos de Energia Primaria
        /// </summary>
        /// <returns></returns>
        public List<MeFormatoDTO> ListarFormatosEnergiaPrimaria()
        {
            var listaFormato = this.ListMeFormatos();
            var listaFiltroFormato = new List<MeFormatoDTO>();
            listaFiltroFormato.Add(listaFormato.Find(x => x.Formatcodi == ConstantesHard.IdFormatoEnergiaPrimariaEolicoTermico));
            listaFiltroFormato.Add(listaFormato.Find(x => x.Formatcodi == ConstantesHard.IdFormatoEnergiaPrimariaSolar));

            return listaFiltroFormato;
        }

        /// <summary>
        /// Listar hojas de Energia Primaria
        /// </summary>
        /// <returns></returns>
        public List<MeHojaDTO> ListarHojaEnergiaPrimaria()
        {
            List<MeHojaDTO> lista = new List<MeHojaDTO>();

            foreach (var reg in this.ListarFormatosEnergiaPrimaria())
            {
                lista.AddRange(this.GetByCriteriaMeHoja(reg.Formatcodi));
            }

            return lista;
        }

        #endregion

        #region Generación RER y Calor Útil Proyectado

        /// <summary>
        /// Listar todos los formatos de GENERACIÓN RER
        /// </summary>
        /// <returns></returns>
        public List<MeFormatoDTO> ListarFormatosGeneracionRER()
        {
            var listaFormato = this.ListMeFormatos();
            var listaFiltroFormato = new List<MeFormatoDTO>();
            listaFiltroFormato.Add(listaFormato.Where(x => x.Formatcodi == ConstantesFormatoMedicion.IdFormatoGeneracionRERDiario).FirstOrDefault());
            listaFiltroFormato.Add(listaFormato.Where(x => x.Formatcodi == ConstantesFormatoMedicion.IdFormatoGeneracionRERSemanal).FirstOrDefault());

            return listaFiltroFormato;
        }

        /// <summary>
        /// Metodo disenio de la tabla cabecera y detalle Reporte Generación RER
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public string ReporteGeneracionRERProgHtml(int tipoPresentacion, int lectcodi, DateTime fechaIni, DateTime fechaFin, string idEmpresa, string tipoCentral, string tipoCogeneracion, string tipoReporte)
        {
            this.ListarGeneracionRERProg(tipoPresentacion, lectcodi, fechaIni, fechaFin, idEmpresa, tipoCentral, tipoCogeneracion, tipoReporte
                , out MeFormatoDTO formato, out List<MeMedicion48DTO> data, out List<MePtomedicionDTO> listaPto);

            NumberFormatInfo nfi = UtilAnexoAPR5.GenerarNumberFormatInfo3();
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 1;
            nfi.NumberDecimalSeparator = ",";

            if (listaPto.Count == 0)
                return string.Empty;

            StringBuilder strHtml = new StringBuilder();

            //***************************************************** CABECERA DE LA TABLA  ***************************************************************************************************

            int padding = 20;
            int anchoTotal = (110 + padding) + (listaPto.Count * (100 + padding));


            strHtml.Append("<div class='freeze_table' id='resultado' style='height: auto;'>");
            strHtml.AppendFormat("<table id='reporte' class='pretty tabla-icono' style='width: {0}px;' >", anchoTotal);

            strHtml.Append("<thead>");
            #region cabecera

            string centralOld = listaPto[0].Central;
            string color = ConstantesFormatoMedicion.ColorColumnaCentralParWeb;
            for (var i = 1; i <= listaPto.Count(); i++)
            {
                string centralActual = listaPto[i - 1].Central;
                if (centralActual != centralOld)
                {
                    centralOld = centralActual;
                    color = color == ConstantesFormatoMedicion.ColorColumnaCentralParWeb ? ConstantesFormatoMedicion.ColorColumnaCentralImparWeb : ConstantesFormatoMedicion.ColorColumnaCentralParWeb;
                }
                listaPto[i - 1].ColorEstado = "white-space: normal;word-break: break-word;background: " + color + ";color: #000000; width: " + 100 + "px;";
            }

            //Punto de medición
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width:110px; white-space: normal'>CÓDIGO</th>");
            foreach (var item in listaPto)
            {
                strHtml.AppendFormat("<th class='grupo_rer' style='white-space: normal;{1}'>{0}</th>", item.Ptomedicodi, item.ColorEstado);
            }
            strHtml.Append("</tr>");

            //Empresas
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width:110px; style='white-space: normal''>EMPRESA</th>");
            foreach (var item in listaPto)
            {
                strHtml.AppendFormat("<th class='empresa_rer' style='{1}'>{0}</th>", item.Emprnomb, item.ColorEstado);
            }
            strHtml.Append("</tr>");

            //Centrales
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width:110px; style='white-space: normal''>CENTRAL</th>");
            foreach (var item in listaPto)
            {
                strHtml.AppendFormat("<th class='grupo_rer' style='{1}'>{0}</th>", item.Central, item.ColorEstado);
            }
            strHtml.Append("</tr>");

            //Equipo
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width:110px; style='white-space: normal''>UNIDAD</th>");
            foreach (var item in listaPto)
            {
                strHtml.AppendFormat("<th class='grupo_rer' style='{1}'>{0}</th>", item.Ptomedibarranomb, item.ColorEstado);
            }
            strHtml.Append("</tr>");

            //Tipoinfocodi
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width:110px; style='white-space: normal''>HORA</th>");
            foreach (var item in listaPto)
            {
                strHtml.AppendFormat("<th class='grupo_rer' style='{0}'>MW</th>", item.ColorEstado);
            }
            strHtml.Append("</tr>");

            #endregion

            strHtml.Append("</thead>");

            /// ****************************************  CUERPO DE LA TABLA ******************************************************         
            strHtml.Append("<tbody>");

            #region cuerpo

            // Día - Hora
            for (var day = fechaIni.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
            {
                //HORA
                DateTime horas = day.AddMinutes(30);

                for (int h = 1; h <= 48; h++)
                {
                    strHtml.Append("<tr>");
                    strHtml.AppendFormat("<td class='tdbody_reporte'>{0:dd/MM/yyyy HH:mm}</td>", horas);

                    foreach (var pto in listaPto)
                    {
                        MeMedicion48DTO m48 = data.Find(x => x.Ptomedicodi == pto.Ptomedicodi && x.Medifecha == day);
                        decimal? valor = m48 != null ? (decimal?)m48.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m48, null) : null;

                        strHtml.AppendFormat("<td>{0}</td>", valor != null ? valor.Value.ToString("N", nfi) : string.Empty);
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
        /// Exportación del archivo excel Rer proyectado
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="nombreArchivo"></param>
        /// <param name="tipoPresentacion"></param>
        /// <param name="lectcodi"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="tipoCentral"></param>
        /// <param name="tipoCogeneracion"></param>
        /// <param name="tipoReporte"></param>
        public void GenerarExcelGeneracionRERProg(string ruta, string nombreArchivo, int tipoPresentacion, int lectcodi, DateTime fechaIni, DateTime fechaFin, string idEmpresa, string tipoCentral, string tipoCogeneracion, string tipoReporte)
        {
            FileInfo newFile = new FileInfo(ruta + nombreArchivo);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + nombreArchivo);
            }

            this.ListarGeneracionRERProg(tipoPresentacion, lectcodi, fechaIni, fechaFin, idEmpresa, tipoCentral, tipoCogeneracion, tipoReporte
                , out MeFormatoDTO formato, out List<MeMedicion48DTO> data, out List<MePtomedicionDTO> listaPto);

            using (ExcelPackage xlPackage = new ExcelPackage())
            {
                this.GenerarHojaGeneracionRERProg(xlPackage, tipoPresentacion, "REPORTE", lectcodi, formato.Formatnombre, fechaIni, fechaFin, data, listaPto);

                xlPackage.SaveAs(newFile);
            }
        }

        /// <summary>
        /// Genera hoja excel
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        private void GenerarHojaGeneracionRERProg(ExcelPackage xlPackage, int tipoPresentacion, string nombre, int lectcodi, string nombreFormato, DateTime fechaIni, DateTime fechaFin, List<MeMedicion48DTO> data, List<MePtomedicionDTO> listaPto)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nombre);

            int rowIni = 2;
            int rowFin = rowIni;
            int column = 1;

            ws.Row(1).Height = 0;

            #region Filtros

            int colIniProp = column;
            int colIniValor = colIniProp + 1;
            int rowIniEmpresa = rowIni;
            int rowIniFormato = rowIniEmpresa + 1;
            int rowIniFecha = rowIniFormato + 1;
            int rowIniSemana = rowIniFormato + 1;
            int rowIniFechaDesde = rowIniSemana + 1;
            int rowIniFechaHasta = rowIniFechaDesde + 1;

            ws.Cells[rowIniEmpresa, colIniProp].Value = "EMPRESA";
            ws.Cells[rowIniFormato, colIniProp].Value = "FORMATO";

            ws.Cells[rowIniEmpresa, colIniValor].Value = "(TODOS)";
            ws.Cells[rowIniFormato, colIniValor].Value = nombreFormato;

            int formatrows = tipoPresentacion == ConstantesFormatoMedicion.TipoPresentacionGrupoDespacho ? 4 : 5;
            int formathorizonte = 1;
            int rowPorDia = 48;
            switch (lectcodi)
            {
                case ConstantesFormatoMedicion.LectGeneracionRERSemanal:
                    formathorizonte = 7;

                    ws.Cells[rowIniSemana, colIniProp].Value = "SEMANA";
                    ws.Cells[rowIniFechaDesde, colIniProp].Value = "FECHA DESDE";
                    ws.Cells[rowIniFechaHasta, colIniProp].Value = "FECHA HASTA";

                    var tupla = EPDate.f_numerosemana_y_anho(fechaIni);

                    ws.Cells[rowIniSemana, colIniValor].Value = tupla.Item1;
                    ws.Cells[rowIniFechaDesde, colIniValor].Value = fechaIni.ToString(ConstantesAppServicio.FormatoFecha);
                    ws.Cells[rowIniFechaHasta, colIniValor].Value = fechaFin.ToString(ConstantesAppServicio.FormatoFecha);
                    rowFin = rowIniFechaHasta;
                    break;
                case ConstantesFormatoMedicion.LectGeneracionRERDiario:
                    ws.Cells[rowIniFecha, colIniProp].Value = "FECHA";
                    ws.Cells[rowIniFecha, colIniValor].Value = fechaIni.ToString(ConstantesAppServicio.FormatoFecha);
                    rowFin = rowIniFecha;
                    break;
            }

            //primera columna
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIni, colIniProp, rowFin, colIniProp, "Centro");
            UtilExcel.CeldasExcelColorTexto(ws, rowIni, colIniProp, rowFin, colIniProp, "#333399");
            UtilExcel.CeldasExcelColorFondoYBorder(ws, rowIni, colIniProp, rowFin, colIniProp, ColorTranslator.FromHtml(ConstantesFormatoMedicion.ColorColumnaHora), Color.Black);
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIni, colIniProp, rowFin, colIniProp, "Arial", 10);

            //segunda columna
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIni, colIniValor, rowFin, colIniValor, "Izquierda");
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIni, colIniValor, rowFin, colIniValor, "Arial", 8);
            UtilExcel.CeldasExcelEnNegrita(ws, rowIni, colIniValor, rowFin, colIniValor);

            #endregion

            ///Imprimimos cabecera de puntos de medicion
            int rowIniData = 9;
            int colIniData = column;
            int totColumnas = listaPto.Count;

            //nota
            ws.Cells[rowIniData - 1, colIniData].Value = "No modificar la estructura del documento";
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniData - 1, colIniData, rowIniData - 1, colIniData, "Arial", 8);
            ws.Cells[rowIniData - 1, colIniData].Style.Font.Italic = true;

            //ocultar filas
            var listaFilasOcultas = new List<int>();
            foreach (var posFila in listaFilasOcultas)
            {
                ws.Row(rowIniData + posFila).Height = 0;
            }

            //Cabecera
            for (var i = 0; i <= totColumnas; i++)
            {
                ws.Column(i + colIniData).Width = i == 0 ? 20 : 22;

                if (tipoPresentacion == ConstantesFormatoMedicion.TipoPresentacionGrupoDespacho)
                {
                    if (i == 0)
                    {
                        ws.Cells[rowIniData + 0, colIniData + i].Value = "COD DESPACHO";
                        ws.Cells[rowIniData + 1, colIniData + i].Value = "EMPRESA";
                        ws.Cells[rowIniData + 2, colIniData + i].Value = "GRUPO";
                        ws.Cells[rowIniData + 3, colIniData + i].Value = "HORA";
                    }
                    else
                    {
                        var pto = listaPto[i - 1];

                        ws.Cells[rowIniData + 0, colIniData + i].Value = pto.Ptomedicodi;
                        ws.Cells[rowIniData + 1, colIniData + i].Value = pto.Emprnomb;
                        ws.Cells[rowIniData + 2, colIniData + i].Value = pto.Ptomedibarranomb;
                        ws.Cells[rowIniData + 3, colIniData + i].Value = "MW";
                    }
                }
                else
                {
                    if (i == 0)
                    {
                        ws.Cells[rowIniData + 0, colIniData + i].Value = tipoPresentacion != ConstantesFormatoMedicion.TipoPresentacionCentral ? "CÓDIGO" : "COD DESPACHO";
                        ws.Cells[rowIniData + 1, colIniData + i].Value = "EMPRESA";
                        ws.Cells[rowIniData + 2, colIniData + i].Value = "CENTRAL";
                        ws.Cells[rowIniData + 3, colIniData + i].Value = "UNIDAD";
                        ws.Cells[rowIniData + 4, colIniData + i].Value = "HORA";
                    }
                    else
                    {
                        var pto = listaPto[i - 1];

                        ws.Cells[rowIniData + 0, colIniData + i].Value = pto.Ptomedicodi;
                        ws.Cells[rowIniData + 1, colIniData + i].Value = pto.Emprnomb;
                        ws.Cells[rowIniData + 2, colIniData + i].Value = pto.Central;
                        ws.Cells[rowIniData + 3, colIniData + i].Value = pto.Ptomedibarranomb;
                        ws.Cells[rowIniData + 4, colIniData + i].Value = "MW";
                    }

                }
            }

            /////////////////Formato a Celdas Head ///////////////////
            int rowIniPto = rowIniData;
            int rowFinPto = rowIniPto + formatrows - 1;
            int colHora = colIniData;

            int colIniPto = colHora + 1;
            int colFinPto = colIniPto + listaPto.Count - 1;
            int rowIniNumero = rowIniPto + formatrows;
            int rowFinNumero = rowIniPto + formathorizonte * rowPorDia + formatrows - 1;

            int numDia = 0;
            var j = 0; 
            for (var day = fechaIni.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
            {
                numDia++;

                //HORA
                DateTime horas = day.AddMinutes(30);

                for (int h = 1; h <= 48; h++)
                {
                    int i = 0;
                    ws.Cells[rowIniNumero + j, colIniData + i].Value = horas.ToString(ConstantesAppServicio.FormatoFechaFull);

                    for (i = 1; i <= totColumnas; i++)
                    {
                        var pto = listaPto[i - 1];

                        MeMedicion48DTO regpotActiva = data.Find(x => x.Ptomedicodi == pto.Ptomedicodi && x.Medifecha == day 
                                                                && x.Central == pto.Central && x.Ptomedinomb == pto.Ptomedibarranomb);
                        decimal? valor = regpotActiva != null ? (decimal?)regpotActiva.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regpotActiva, null) : null;
                        ws.Cells[rowIniNumero + j, colIniData + i].Value = valor;

                    }
                    j++;
                    horas = horas.AddMinutes(30);
                }
            }

            //Formato para toda la tabla (cabecera y cuerpo)
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniPto, colHora, rowFinNumero, colFinPto, "Centro");
            UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniPto, colHora, rowFinNumero, colFinPto, "Centro");

            //formato para las filas de cabecera (columna hora)
            UtilExcel.CeldasExcelColorTexto(ws, rowIniPto, colHora, rowFinPto, colHora, "#333399");
            UtilExcel.CeldasExcelColorFondoYBorder(ws, rowIniPto, colHora, rowFinPto, colHora, ColorTranslator.FromHtml(ConstantesFormatoMedicion.ColorColumnaHora), Color.Black);
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniPto, colHora, rowFinPto, colHora, "Arial", 10);

            if (listaPto.Count > 0)
            {
                //Formato para las filas de cabecera (columnas de puntos de medición)
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniPto, colIniPto, rowFinPto, colFinPto, "Centro");
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniPto, colIniPto, rowFinPto, colFinPto, "Arial", 10);
                UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniPto, colIniPto, rowFinPto, colFinPto, "Centro");

                string centralOld = tipoPresentacion == ConstantesFormatoMedicion.TipoPresentacionGrupoDespacho? listaPto[0].Ptomedibarranomb : listaPto[0].Central ;
                string color = ConstantesFormatoMedicion.ColorColumnaCentralPar;
                for (var i = 1; i <= totColumnas; i++)
                {
                    string centralActual = tipoPresentacion == ConstantesFormatoMedicion.TipoPresentacionGrupoDespacho ? listaPto[i-1].Ptomedibarranomb : listaPto[i-1].Central;
                    if (centralActual != centralOld)
                    {
                        centralOld = centralActual;
                        color = color == ConstantesFormatoMedicion.ColorColumnaCentralPar ? ConstantesFormatoMedicion.ColorColumnaCentralImpar : ConstantesFormatoMedicion.ColorColumnaCentralPar;
                    }
                    UtilExcel.CeldasExcelColorFondoYBorder(ws, rowIniPto, i + colIniData, rowFinPto, i + colIniData, ColorTranslator.FromHtml(color), Color.Black);
                }

                UtilExcel.CeldasExcelEnNegrita(ws, rowIniPto, colIniPto, rowFinPto, colFinPto);
                UtilExcel.CeldasExcelWrapText(ws, rowIniPto, colIniPto, rowFinPto, colFinPto);

                //Formato para las celdas numericas
                ws.Cells[rowIniNumero, colIniPto, rowFinNumero, colFinPto].Style.Numberformat.Format = @"0.00";
            }

            //border punteado 
            UtilExcel.BorderCeldasPunteado(ws, rowIniNumero, colHora, rowFinNumero, colFinPto);
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniNumero, colHora, rowFinNumero, colFinPto, "Arial", 8);

            //mostrar lineas horas
            for (int c = colHora; c <= colFinPto; c++)
            {
                int totalXRango = 48;
                for (int f = rowIniNumero; f < rowFinNumero; f += totalXRango)
                {
                    ws.Cells[f, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[f, c].Style.Fill.BackgroundColor.SetColor(Color.White);
                    ws.Cells[f, c].Style.Border.Top.Style = ExcelBorderStyle.Thin;

                    ws.Cells[f + totalXRango - 1, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[f + totalXRango - 1, c].Style.Fill.BackgroundColor.SetColor(Color.White);
                    ws.Cells[f + totalXRango - 1, c].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    ws.Cells[f, c, f + totalXRango - 1, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[f, c, f + totalXRango - 1, c].Style.Fill.BackgroundColor.SetColor(Color.White);
                    ws.Cells[f, c, f + totalXRango - 1, c].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    ws.Cells[f, c, f + totalXRango - 1, c].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                }
            }

            ws.View.FreezePanes(rowFinPto + 1, colHora + 1);
            ws.View.ZoomScale = 80;
            //ws.View.ShowGridLines = false;
        }

        /// <summary>
        /// Obtener información RER y Calor util
        /// </summary>
        /// <param name="tipoPresentacion"></param>
        /// <param name="lectcodi"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="tipoCentral"></param>
        /// <param name="tipoCogeneracion"></param>
        /// <param name="tipoReporte"></param>
        /// <param name="formato"></param>
        /// <param name="listaData"></param>
        /// <param name="listaPto"></param>
        public void ListarGeneracionRERProg(int tipoPresentacion, int lectcodi, DateTime fechaIni, DateTime fechaFin, string idEmpresa, string tipoCentral, string tipoCogeneracion, string tipoReporte
            , out MeFormatoDTO formato, out List<MeMedicion48DTO> listaData, out List<MePtomedicionDTO> listaPto)
        {
            idEmpresa = !string.IsNullOrEmpty(idEmpresa) ? idEmpresa.Trim() : ConstantesAppServicio.ParametroDefecto;

            //formato
            if (lectcodi == ConstantesFormatoMedicion.LectGeneracionRERDiario)
                formato = FactorySic.GetMeFormatoRepository().GetById(ConstantesFormatoMedicion.IdFormatoGeneracionRERDiario);
            else
                formato = FactorySic.GetMeFormatoRepository().GetById(ConstantesFormatoMedicion.IdFormatoGeneracionRERSemanal);

            //Coes, no Coes
            List<PrGrupodatDTO> listaOperacionCoes = FactorySic.GetPrGrupodatRepository().ListarHistoricoValores(ConstantesPR5ReportesServicio.PropGrupoOperacionCoes.ToString(), -1)
                                                        .Where(x => x.Deleted == 0).OrderByDescending(x => x.Fechadat).ToList();
            //Puntos de medición activos (para completar con puntos que no cargaron data)
            var listaPtoxFormatoTmp = FactorySic.GetMeHojaptomedRepository().ListarHojaPtoByFormatoAndEmpresa(-1, ConstantesFormatoMedicion.IdFormatoGeneracionRERDiario.ToString())
                                        .Where(x => x.Hojaptoactivo == 1).ToList();
            var listaPtoCalorUtil = listaPtoxFormatoTmp.Where(x => x.Tptomedicodi == ConstantesMedicion.IdTipoPtomedicodiCalorUtilGeneracion || x.Tptomedicodi == ConstantesMedicion.IdTipoPtomedicodiCalorUtilRecibidoProceso).Select(x => x.Ptomedicodi).ToList();

            List<MePtomedicionDTO> listaPtoxFormato = UtilAnexoAPR5.ListarPtoMedicionFromHojapto(listaPtoxFormatoTmp);
            if (ConstantesAppServicio.ParametroDefecto != idEmpresa)
            {
                List<int> listaEmprcodi = idEmpresa.Split(',').Select(Int32.Parse).ToList();
                listaPtoxFormato = listaPtoxFormato.Where(x => listaEmprcodi.Contains(x.Emprcodi.Value)).ToList();
            }

            //Grupos
            List<PrGrupoDTO> listaGrupo = FactorySic.GetPrGrupoRepository().List();
            //Puntos
            List<MePtomedicionDTO> listaPtoDespacho = FactorySic.GetMePtomedicionRepository().List(ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.OriglectcodiDespachomediahora.ToString());

            foreach (var regPto in listaPtoxFormato)
            {
                if (regPto.Ptomedicodi == 47491)
                { }
                var regGrupo = listaGrupo.Find(x => x.Grupocodi == regPto.Grupocodi);
                regPto.Grupointegrante = regGrupo != null && regGrupo.Grupointegrante != null ? regGrupo.Grupointegrante : ConstantesAppServicio.NO;
                regPto.Grupotipocogen = regGrupo != null && regGrupo.Grupotipocogen != null ? regGrupo.Grupotipocogen : ConstantesAppServicio.NO;
                regPto.Tipoptomedicodi = !listaPtoCalorUtil.Contains(regPto.Ptomedicodi) ? ConstantesMedicion.IdTipoPtomedicodiMedidaElectrica : regPto.Tipoptomedicodi;
                regPto.Grupointegrante = ReporteMedidoresAppServicio.SetValorCentralIntegrante(regPto.Grupocodi, fechaIni, regPto.Grupointegrante, listaOperacionCoes);

                //
                if (tipoPresentacion == ConstantesFormatoMedicion.TipoPresentacionGrupoDespacho)
                {
                    var ptoDespacho = listaPtoDespacho.Find(x => (x.Grupocodi == regPto.Grupocodi && regPto.Grupocodi > 0) || (x.Equicodi == regPto.Equicodi && regPto.Equicodi > 0) || (x.Equicodi == regPto.Equipadre && regPto.Equipadre > 0));
                    if (ptoDespacho != null && !listaPtoCalorUtil.Contains(regPto.Ptomedicodi))
                    {
                        regPto.Central = "GRUPO";
                        regPto.Ptomedicodi = ptoDespacho.Ptomedicodi;
                        regPto.Ptomedielenomb = ptoDespacho.Ptomedielenomb;
                    }
                }

                if (tipoPresentacion == ConstantesFormatoMedicion.TipoPresentacionCentral)
                {
                    var ptoDespacho = listaPtoDespacho.Find(x => (x.Grupocodi == regPto.Grupocodi && regPto.Grupocodi > 0) || (x.Equicodi == regPto.Equicodi && regPto.Equicodi > 0) || (x.Equicodi == regPto.Equipadre && regPto.Equipadre > 0));
                    if (ptoDespacho != null && !listaPtoCalorUtil.Contains(regPto.Ptomedicodi))
                    {
                        regPto.Ptomedicodi = ptoDespacho.Ptomedicodi;
                        regPto.Ptomedielenomb = "CENTRAL";
                    }
                }

                regPto.Central = (regPto.Central ?? "").Trim().ToUpper();
                regPto.Ptomedielenomb = (regPto.Ptomedielenomb ?? "").Trim().ToUpper();
            }

            //Medicion
            List<MeMedicion48DTO> listaM48Rango = FactorySic.GetMeMedicion48Repository().GetConsolidadoMaximaDemanda48SinGrupoIntegrante(
            ConstantesMedicion.IdTipogrupoTodos, ConstantesMedicion.IdTipoGeneracionTodos.ToString(), fechaIni, fechaFin, idEmpresa
                , lectcodi, ConstantesMedicion.IdTipoInfoPotenciaActiva, ConstantesMedicion.IdTptomedicodiTodos);

            foreach (var reg48 in listaM48Rango)
            {
                if (reg48.Ptomedicodi == 23236)
                { }
                reg48.Grupointegrante = ReporteMedidoresAppServicio.SetValorCentralIntegrante(reg48.Grupocodi, reg48.Medifecha, reg48.Grupointegrante, listaOperacionCoes);

                var pto = listaPtoxFormato.Find(x => x.Ptomedicodi == reg48.Ptomedicodi);
                reg48.Tipoptomedicodi = pto == null ? ConstantesMedicion.IdTipoPtomedicodiMedidaElectrica : pto.Tipoptomedicodi;

                //
                if (tipoPresentacion == ConstantesFormatoMedicion.TipoPresentacionGrupoDespacho)
                {
                    var ptoDespacho = listaPtoDespacho.Find(x => (x.Grupocodi == reg48.Grupocodi && reg48.Grupocodi>0) || (x.Equicodi == reg48.Equicodi&& reg48.Equicodi>0) || (x.Equicodi == reg48.Equipadre&& reg48.Equipadre>0));
                    if (ptoDespacho != null && !listaPtoCalorUtil.Contains(reg48.Ptomedicodi))
                    {
                        reg48.Central = "GRUPO";
                        reg48.Ptomedicodi = ptoDespacho.Ptomedicodi;
                        reg48.Ptomedinomb = ptoDespacho.Ptomedielenomb;
                    }
                }
                if (tipoPresentacion == ConstantesFormatoMedicion.TipoPresentacionCentral)
                {
                    var ptoDespacho = listaPtoDespacho.Find(x => (x.Grupocodi == reg48.Grupocodi && reg48.Grupocodi > 0) || (x.Equicodi == reg48.Equicodi && reg48.Equicodi > 0) || (x.Equicodi == reg48.Equipadre && reg48.Equipadre > 0));
                    if (ptoDespacho != null && !listaPtoCalorUtil.Contains(reg48.Ptomedicodi))
                    {
                        reg48.Ptomedicodi = ptoDespacho.Ptomedicodi;
                        reg48.Ptomedinomb = "CENTRAL";
                    }
                }

                reg48.Central = (reg48.Central ?? "").Trim().ToUpper();
                reg48.Ptomedinomb = (reg48.Ptomedinomb ?? "").Trim().ToUpper();
            }
            if (tipoPresentacion == ConstantesFormatoMedicion.TipoPresentacionGrupoDespacho || tipoPresentacion == ConstantesFormatoMedicion.TipoPresentacionCentral)
                listaM48Rango = ConvertirM48RerToM48Despacho(listaM48Rango, listaPtoCalorUtil);

            //Combo Integrante
            if (tipoCentral == ConstantesMedicion.IdTipogrupoCOES.ToString())
            {
                listaM48Rango = listaM48Rango.Where(x => x.Grupointegrante == ConstantesAppServicio.SI).ToList();
                listaPtoxFormato = listaPtoxFormato.Where(x => x.Grupointegrante == ConstantesAppServicio.SI).ToList();
            }
            if (tipoCentral == ConstantesMedicion.IdTipogrupoNoIntegrante.ToString())
            {
                listaM48Rango = listaM48Rango.Where(x => x.Grupointegrante != ConstantesAppServicio.SI).ToList();
                listaPtoxFormato = listaPtoxFormato.Where(x => x.Grupointegrante != ConstantesAppServicio.SI).ToList();
            }

            //Combo cogeneracion
            if (tipoCogeneracion == "1")
            {
                listaM48Rango = listaM48Rango.Where(x => x.Grupotipocogen == ConstantesAppServicio.SI).ToList();
                listaPtoxFormato = listaPtoxFormato.Where(x => x.Grupotipocogen == ConstantesAppServicio.SI).ToList();
            }
            if (tipoCogeneracion == "2")
            {
                listaM48Rango = listaM48Rango.Where(x => x.Grupotipocogen != ConstantesAppServicio.SI).ToList();
                listaPtoxFormato = listaPtoxFormato.Where(x => x.Grupotipocogen != ConstantesAppServicio.SI).ToList();
            }

            //Tipo reporte
            if (tipoReporte == "1")
            {
                listaM48Rango = listaM48Rango.Where(x => x.Tipoptomedicodi == ConstantesMedicion.IdTipoPtomedicodiMedidaElectrica).ToList();
                listaPtoxFormato = listaPtoxFormato.Where(x => x.Tipoptomedicodi == ConstantesMedicion.IdTipoPtomedicodiMedidaElectrica).ToList();
            }
            if (tipoReporte == "2")
            {
                listaM48Rango = listaM48Rango.Where(x => x.Tipoptomedicodi != ConstantesMedicion.IdTipoPtomedicodiMedidaElectrica).ToList();
                listaPtoxFormato = listaPtoxFormato.Where(x => x.Tipoptomedicodi != ConstantesMedicion.IdTipoPtomedicodiMedidaElectrica).ToList();
            }

            //puntos de medición            
            var listaPtoxM48 = UtilAnexoAPR5.ListarPtoMedicionFromM48(listaM48Rango);
            listaPtoxM48.AddRange(listaPtoxFormato);
            listaPto = listaPtoxM48.GroupBy(x => new { x.Ptomedicodi, x.Central, x.Ptomedielenomb })
                                    .Select(x => new MePtomedicionDTO()
                                    {
                                        Ptomedicodi = x.Key.Ptomedicodi,
                                        Emprcodi = x.First().Emprcodi,
                                        Emprnomb = (x.First().Emprnomb ?? "").Trim(),
                                        Equipadre = x.First().Equipadre,
                                        Central = x.Key.Central,
                                        Ptomedibarranomb = x.First().Ptomedielenomb,
                                        Tipoptomedicodi = x.First().Tipoptomedicodi
                                    })
                                    .OrderBy(x => x.Emprnomb).ThenBy(x => x.Tipoptomedicodi).ThenBy(x => x.Central).ThenBy(x => x.Ptomedibarranomb)
                                    .ToList();

            //Output
            listaData = listaM48Rango;
        }

        private List<MeMedicion48DTO> ConvertirM48RerToM48Despacho(List<MeMedicion48DTO> data, List<int> listaPtcodicalorUtil)
        {
            List<MeMedicion48DTO> listaCalorUtil = data.Where(x => listaPtcodicalorUtil.Contains(x.Ptomedicodi)).ToList();

            List<MeMedicion48DTO> listaMW = data.Where(x => !listaPtcodicalorUtil.Contains(x.Ptomedicodi)).ToList();

            listaMW = (from t in listaMW
                       group t by new { t.Medifecha, t.Ptomedicodi, t.Central, t.Ptomedinomb}
                                      into destino
                       select new MeMedicion48DTO()
                       {
                           Ptomedicodi = destino.Key.Ptomedicodi,
                           Medifecha = destino.Key.Medifecha,
                           Emprcodi = destino.First().Emprcodi,
                           Emprnomb = (destino.First().Emprnomb ?? "").Trim(),
                           Equipadre = destino.First().Equipadre,
                           Central = destino.Key.Central,
                           Ptomedinomb = destino.Key.Ptomedinomb,
                           Tipoptomedicodi = destino.First().Tipoptomedicodi,
                           Grupointegrante = destino.First().Grupointegrante,
                           Grupotipocogen = destino.First().Grupotipocogen,
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

            listaMW.AddRange(listaCalorUtil);

            return listaMW;
        }

        /// <summary>
        /// listar empresas RER
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarEmpresaGeneracionRER()
        {
            var listaGrupoRer = FactorySic.GetPrGrupoRepository().ListarAllGrupoRER(DateTime.Today);
            var listaGrupoCogen = FactorySic.GetPrGrupoRepository().ListarAllGrupoCoGeneracion(DateTime.Today);
            listaGrupoRer.AddRange(listaGrupoCogen);

            List<SiEmpresaDTO> empresas = FactorySic.GetSiEmpresaRepository().ObtenerEmpresaFormato(ConstantesFormatoMedicion.IdFormatoGeneracionRERDiario);
            empresas.AddRange(listaGrupoRer.Select(x => new SiEmpresaDTO() { Emprcodi = x.Emprcodi.Value, Emprnomb = x.Emprnomb }).ToList());

            //lista de empresas rer, cogeneracion y otras adicionales que pertenezcan al formato
            List<SiEmpresaDTO> l = empresas.GroupBy(x => x.Emprcodi)
                                        .Select(x => new SiEmpresaDTO() { Emprcodi = x.Key, Emprnomb = x.First().Emprnomb })
                                        .OrderBy(x => x.Emprnomb).ToList();

            return l;
        }

        /// <summary>
        /// Generar reporte de cumplimiento
        /// </summary>
        /// <param name="idFormato"></param>
        /// <param name="fechaIni"></param>
        /// <returns></returns>
        public List<MeEnvioDTO> GenerarReporteCumplimiento(int idFormato, DateTime fechaIni)
        {
            List<SiEmpresaDTO> empresas = FactorySic.GetSiEmpresaRepository().ObtenerEmpresaFormato(idFormato);

            List<MeEnvioDTO> listaEnvio = FactorySic.GetMeEnvioRepository().ObtenerReporteCumplimiento(ConstantesAppServicio.ParametroDefecto, idFormato, fechaIni, fechaIni);
            foreach (var reg in listaEnvio)
            {
                reg.Enviofecha2 = reg.Enviofecha.Value.ToString(ConstantesAppServicio.FormatoFechaFull);
                reg.EnvioplazoDesc = reg.Envioplazo == "P" ? "Dentro del plazo" : "Fuera del plazo";
            }
            var listaEmprcodiSiEnvio = listaEnvio.Select(x => x.Emprcodi).ToList();

            List<SiEmpresaDTO> listaEmpresaNoEnvio = empresas.Where(x => !listaEmprcodiSiEnvio.Contains(x.Emprcodi)).ToList();
            listaEmpresaNoEnvio = listaEmpresaNoEnvio.Where(x => x.Emprestado != ConstantesAppServicio.Baja).ToList();

            foreach (var reg in listaEmpresaNoEnvio)
            {
                listaEnvio.Add(new MeEnvioDTO() { Emprcodi = reg.Emprcodi, Emprnomb = reg.Emprnomb });
            }

            listaEnvio = listaEnvio.OrderBy(x => x.Emprnomb).ToList();

            return listaEnvio;
        }

        /// <summary>
        /// Copiar datos de EXT_LOGENVIO a ME_ENVIO
        /// </summary>
        public void EjecutarProcesoCopiarExtLogEnvioAMeEnvio()
        {
            List<MeEnvioDTO> listaEnvio = new List<MeEnvioDTO>();

            //historicos
            List<ExtLogenvioDTO> listaEnvDiario = FactorySic.GetExtLogenvioRepository().GetByCriteria(ConstantesFormatoMedicion.LectGeneracionRERDiario);
            List<ExtLogenvioDTO> listaEnvSemanal = FactorySic.GetExtLogenvioRepository().GetByCriteria(ConstantesFormatoMedicion.LectGeneracionRERSemanal);

            foreach (var reg in listaEnvDiario)
            {
                DateTime fechaPeriodo = reg.Feccarga.Value.Date;

                listaEnvio.Add(new MeEnvioDTO()
                {
                    Enviofechaperiodo = fechaPeriodo,
                    Enviofecha = reg.Lastdate,
                    Estenvcodi = 3,
                    Envioplazo = reg.Estenvcodi == 1 ? "P" : "F",
                    Lastuser = reg.Lastuser,
                    Emprcodi = reg.Emprcodi,
                    Formatcodi = ConstantesFormatoMedicion.IdFormatoGeneracionRERDiario,
                    Enviofechaini = fechaPeriodo,
                    Enviofechafin = fechaPeriodo
                });
            }

            foreach (var reg in listaEnvSemanal)
            {
                if (reg.NroAnio <= 0) reg.NroAnio = 2014;
                DateTime fechaPeriodo = EPDate.f_fechainiciosemana(reg.NroAnio, reg.Nrosemana.Value);

                listaEnvio.Add(new MeEnvioDTO()
                {
                    Enviofechaperiodo = fechaPeriodo,
                    Enviofecha = reg.Lastdate,
                    Estenvcodi = 3,
                    Envioplazo = reg.Estenvcodi == 1 ? "P" : "F",
                    Lastuser = reg.Lastuser,
                    Emprcodi = reg.Emprcodi,
                    Formatcodi = ConstantesFormatoMedicion.IdFormatoGeneracionRERSemanal,
                    Enviofechaini = fechaPeriodo,
                    Enviofechafin = fechaPeriodo.AddDays(6)
                });
            }

            listaEnvio = listaEnvio.OrderBy(x => x.Enviofechaperiodo).ThenBy(x => x.Enviofecha).ToList();

            //Obtener los envios historicos EXT_LOGENVIO que ya estan en la nueva tabla de ME_ENVIO
            List<MeEnvioDTO> lista1 = FactorySic.GetMeEnvioRepository().GetByCriteriaRango(-1, ConstantesFormatoMedicion.IdFormatoGeneracionRERDiario, new DateTime(2000, 1, 1), new DateTime(2021, 1, 1));
            List<MeEnvioDTO> lista2 = FactorySic.GetMeEnvioRepository().GetByCriteriaRango(-1, ConstantesFormatoMedicion.IdFormatoGeneracionRERSemanal, new DateTime(2000, 1, 1), new DateTime(2021, 1, 1));
            lista1.AddRange(lista2);
            lista1 = lista1.OrderBy(x => x.Enviofechaperiodo).ThenBy(x => x.Enviofecha).ToList();

            List<MeEnvioDTO> listaEnvioFinal = new List<MeEnvioDTO>();
            //verificar existentes
            foreach (var reg in listaEnvio)
            {
                if (lista1.Find(x => x.Formatcodi == reg.Formatcodi && x.Enviofechaperiodo == reg.Enviofechaperiodo) == null)
                {
                    listaEnvioFinal.Add(reg);
                }
            }

            //Guardar en BD 
            foreach (var reg in listaEnvioFinal)
            {
                FactorySic.GetMeEnvioRepository().Save(reg);
            }
        }

        #endregion

        #region Ampliación de Plazos

        public List<MeFormatoDTO> CargarFormatosAmpliacion(int app)
        {
            var lista = FactorySic.GetMeFormatoRepository().GetByCriteria(0, 0);

            List<MeFormatoDTO> listaFormatos = new List<MeFormatoDTO>();

            if (app == 0)
            {
                listaFormatos.Add(this.GetByIdMeFormato(ConstantesHard.IdFormatoFlujoTrans));
                listaFormatos.Add(this.GetByIdMeFormato(ConstantesHard.IdFormatoDespacho));
                listaFormatos.Add(this.GetByIdMeFormato(ConstantesStockCombustibles.IdFormatoConsumo));
                listaFormatos.Add(this.GetByIdMeFormato(ConstantesStockCombustibles.IdFormatoPGas));
                listaFormatos.Add(this.GetByIdMeFormato(ConstantesStockCombustibles.IdFormatoDisponibilidadGas));
                listaFormatos.Add(this.GetByIdMeFormato(ConstantesStockCombustibles.IdFormatoQuemaGas));
            }
            if (app>0)
            {
                listaFormatos = this.ListarFormatoByFiltroApp(lista, app, 1);
            }

            return listaFormatos;
        }

        /// <summary>
        /// Listar los formatos segun los aplicativos
        /// </summary>
        /// <param name="listaFormato"></param>
        /// <param name="app"></param>
        /// <param name="tipoAccion"></param>
        /// <returns></returns>
        public List<MeFormatoDTO> ListarFormatoByFiltroApp(List<MeFormatoDTO> listaFormato, int app, int tipoAccion)
        {
            if (app > 0)
            {
                List<int> listaFiltro = ListarFormatcodiByApp(app, tipoAccion);
                listaFormato = listaFormato.Where(x => listaFiltro.Contains(x.Formatcodi)).ToList();
            }

            return listaFormato;
        }

        /// <summary>
        /// Listar codigos de formato para cada aplicativo
        /// </summary>
        /// <param name="app"></param>
        /// <param name="tipoAccion"></param>
        /// <returns></returns>
        private List<int> ListarFormatcodiByApp(int app, int tipoAccion)
        {
            List<int> listaFiltro = new List<int>();
            if (app > 0)
            {
                string strFormatos = string.Empty;
                if (ConstantesFormatoMedicion.AplicativoProgRER == app) strFormatos = ConstantesFormatoMedicion.AplicativoProgRERListaFormato;
                if (ConstantesFormatoMedicion.AplicativoStock == app) strFormatos = ConstantesFormatoMedicion.AplicativoStockListaFormato;
                if (ConstantesFormatoMedicion.AplicativoPotenciaFirme == app) strFormatos = ConstantesFormatoMedicion.AplicativoPotenciaFirmeListaFormato;
                if (ConstantesFormatoMedicion.AplicativoIndisponibilidades == app) strFormatos = ConstantesFormatoMedicion.AplicativoIndisponibilidadesListaFormato;
                if (ConstantesFormatoMedicion.AplicativoCompHidraulico == app) strFormatos = ConstantesFormatoMedicion.AplicativoCompHidraulicoListaFormato;
                if (ConstantesFormatoMedicion.AplicativoProgRER == app && tipoAccion == 1) strFormatos = ConstantesFormatoMedicion.AplicativoProgRERListaFormatoAmpl;
                if (ConstantesFormatoMedicion.AplicativoPMPO == app) strFormatos = ConstantesFormatoMedicion.AplicativoPMPOListaFormato;

                if (!string.IsNullOrEmpty(strFormatos))
                    listaFiltro = strFormatos.Split(',').Select(x => int.Parse(x)).ToList();
            }

            return listaFiltro;
        }

        #endregion

        #region SIOSEIN
        public List<MePtomedicionDTO> ListPtoMedicionMeLectura(int origlectcodi, int lectcodi, int tipoinfocodi)
        {
            return FactorySic.GetMePtomedicionRepository().ListPtoMedicionMeLectura(origlectcodi, lectcodi, tipoinfocodi);
        }

        /// <summary>
        /// Permite listar los puntos de medicion la tabla MeHojaptomed
        /// </summary>
        public List<MeHojaptomedDTO> GetByCriteria3MeHojaptomeds(int emprcodi, int formatcodi, int cuenca, string query)
        {
            return FactorySic.GetMeHojaptomedRepository().GetByCriteria3(emprcodi, formatcodi, cuenca, query);
        }

        #endregion

        #region Modificación Tipo punto de medición

        /// <summary>
        /// Listado de Tipos de punto de medición filtrado po familia y tipo de información
        /// </summary>
        /// <param name="famCodi">Código de familia</param>
        /// <param name="tipoInfoCodi">Código de tipo de información</param>
        /// <returns></returns>
        public List<MeTipopuntomedicionDTO> ListarTiposPuntoMedicion(int famCodi, int tipoInfoCodi)
        {
            return FactorySic.GetMeTipopuntomedicionRepository().ListarTiposPuntoMedicion(famCodi, tipoInfoCodi);
        }
        #endregion

        #region Transferencia de Equipos

        /// <summary>
        /// Indica si esta vigente una empresa (segun las transferencias de equipos)
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="fechaConsulta"></param>
        /// <returns></returns>
        public bool EsEmpresaVigente(int emprcodi, DateTime fechaConsulta)
        {
            return this.servEmpresa.EsEmpresaVigente(emprcodi, fechaConsulta);
        }

        #endregion

        #region Mejoras IEOD
        /// <summary>
        /// Graba Datos de Despacho antiguo con infiormacion de extranet de Despacho
        /// </summary>
        /// <param name="data"></param>
        /// <param name="lptos"></param>
        /// <param name="emprcodi"></param>
        public void GrabarDatosDespachoExtranet(List<MeMedicion48DTO> data, List<MeHojaptomedDTO> lptos, int emprcodi)
        {
            List<MeMedicion48DTO> listaData = new List<MeMedicion48DTO>();

            var lPtoDespacho = FactorySic.GetMePtomedicionRepository().ListarPtomedicionDespachoAntiguo(emprcodi);
            /// Mw
            var dataMw = data.Where(x => x.Tipoinfocodi == 1).ToList();
            listaData = ObtenerDataDespacho(lPtoDespacho, dataMw, lptos, emprcodi);
            foreach (MeMedicion48DTO entity in listaData)
            {
                //Borrar Data
                FactorySic.GetMeMedicion48Repository().Delete(entity.Lectcodi, entity.Medifecha, entity.Tipoinfocodi, entity.Ptomedicodi);
                //Grabar Data
                FactorySic.GetMeMedicion48Repository().Save(entity);
            }
            var dataMvar = data.Where(x => x.Tipoinfocodi == 2).ToList();
            listaData = ObtenerDataDespacho(lPtoDespacho, dataMvar, lptos, emprcodi);
            foreach (MeMedicion48DTO entity in listaData)
            {
                //Borrar Data
                FactorySic.GetMeMedicion48Repository().Delete(entity.Lectcodi, entity.Medifecha, entity.Tipoinfocodi, entity.Ptomedicodi);
                //Grabar Data
                FactorySic.GetMeMedicion48Repository().Save(entity);
            }

        }
        /// <summary>
        /// Obtiene datos de Despacho de Centrales a partir de extranet de Despacho por grupos
        /// </summary>
        /// <param name="lPtoDespacho"></param>
        /// <param name="data"></param>
        /// <param name="lptos"></param>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ObtenerDataDespacho(List<MePtomedicionDTO> lPtoDespacho, List<MeMedicion48DTO> data, List<MeHojaptomedDTO> lptos, int emprcodi)
        {
            List<MeMedicion48DTO> listaData = new List<MeMedicion48DTO>();
            foreach (var reg in data)
            {
                var findpto = lptos.Find(x => x.Ptomedicodi == reg.Ptomedicodi);
                if (findpto != null)
                {
                    var findequipo = lPtoDespacho.Find(x => x.Equicodi == findpto.Equicodi);
                    if (findequipo != null)
                    {
                        reg.Ptomedicodi = findequipo.Ptomedicodi;
                        listaData.Add(reg);
                    }
                    else /// Buscar equipo Padre
                    {
                        var findequipadre = lPtoDespacho.Find(x => x.Equicodi == findpto.Equipadre);
                        if (findequipadre != null)
                        {
                            var dataPadre = listaData.Find(x => x.Ptomedicodi == findequipadre.Ptomedicodi);
                            if (dataPadre == null)
                            {
                                reg.Ptomedicodi = findequipadre.Ptomedicodi;
                                listaData.Add(reg);
                            }
                            else
                            {
                                //Sumar data
                                SumarDespacho(dataPadre, reg);
                            }
                        }
                    }
                }
            }
            return listaData;
        }
        /// <summary>
        /// Suma los Hi de dos registros
        /// </summary>
        /// <param name="reg"></param>
        /// <param name="regAnt"></param>
        public void SumarDespacho(MeMedicion48DTO registro, MeMedicion48DTO regAnt)
        {
            decimal? valor1, valor2;

            for (var i = 1; i <= 48; i++)
            {
                valor1 = (decimal?)registro.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i).ToString()).GetValue(registro, null);
                valor2 = (decimal?)regAnt.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i).ToString()).GetValue(regAnt, null);
                if (valor1 != null)
                {
                    if (valor2 != null)
                    {
                        valor1 = valor1 + valor2;
                    }
                }
                else
                {
                    if (valor2 != null)
                    {
                        valor1 = valor2;
                    }
                }
                registro.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i).ToString()).SetValue(registro, valor1);

            }

        }

        #endregion

        #region FIT - Aplicativo VTD

        /// <summary>
        /// ListarPtoMedicionDuplicadosTransferencia
        /// </summary>
        /// <param name="clientecodi"></param>
        /// <param name="barracodi"></param>
        /// <param name="origen"></param>
        /// <param name="tipoto"></param>
        /// <returns></returns>
        public List<MePtomedicionDTO> ListarPtoMedicionDuplicadosTransferencia(int clientecodi, int barracodi, int origen, int tipoto)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            try
            {
                entitys = FactorySic.GetMePtomedicionRepository().ListarPtoDuplicadoTransferencia(clientecodi, barracodi, origen, tipoto);
            }
            catch (Exception ex)
            {
                var result = ex.Message;
            }
            return entitys;
        }

        public List<MePtomedicionDTO> GetByIdClienteBarraMePtomedicion(int? idEmpresa, int? cliente, int? barra)
        {
            if (cliente == null) cliente = -1;
            if (barra == null) barra = -1;
            return FactorySic.GetMePtomedicionRepository().GetByIdClienteBarraMePtomedicion(idEmpresa, cliente, barra, ConstantesFormatoMedicion.IdOrigenLecturaTransferencias);
        }

        #endregion

        #region Medidores de Generación PR15
        /// <summary>
        /// Método que retorna el listado de tipos de punto de medición según el tipo de información
        /// </summary>
        /// <param name="tipoinfocodi">Código de Tipo de información</param>
        /// <returns>Listado de tipos de punto de medicón</returns>
        public List<MeTipopuntomedicionDTO> ListarTiposPuntoMedicionPorTipoInformacion(int tipoinfocodi)
        {
            return FactorySic.GetMeTipopuntomedicionRepository().ListarTiposPuntoMedicionPorTipoInformacion(tipoinfocodi);
        }
        public DateTime GetFechaProcesoAnterior(int? periodo, DateTime fechaProcesoActual)
        {
            DateTime fechaAnterior = fechaProcesoActual;
            switch (periodo)
            {
                case ParametrosFormato.PeriodoDiario:
                    fechaAnterior = fechaProcesoActual.AddDays(-1);
                    break;
                case ParametrosFormato.PeriodoSemanal:
                    fechaAnterior = fechaProcesoActual.AddDays(-7);
                    break;
                case ParametrosFormato.PeriodoMensualSemana:
                    break;
                case ParametrosFormato.PeriodoMensual:
                    fechaAnterior = fechaProcesoActual.AddMonths(-1);
                    break;
            }

            return fechaAnterior;
        }
        #endregion

        #region Mejoras SIOSEIN2

        public int EjecutarCopiaConfiguracion(int formatcodi, int tipoCopia, string usuario)
        {
            List<MeHojaDTO> listaHoja = FactorySic.GetMeHojaRepository().List();

            //Verificar si el formato tiene un ME_HOJA, esto es necesario para el ME_HOJAPTOMED
            List<MeHojaDTO> listaHojaxFmt = listaHoja.Where(x => x.Formatcodi == formatcodi).ToList();

            List<MeHojaptomedDTO> listaHpto = FactorySic.GetMeHojaptomedRepository().ListByFormatcodi(formatcodi.ToString());

            if (!listaHojaxFmt.Any())
                return 2; //si el formato no tiene hoja es un ERROR

            if (listaHpto.Find(x => x.Hojacodi <= 0) != null)
                return 3; //existe registros de ME_HOJAPTOMED que no tienen HOJACODI correcto

            var hoja1 = listaHpto.Find(x => x.Hojacodi == 1);
            if (hoja1 != null && hoja1.Formatcodi != ConstantesStockCombustibles.IdFormatoPGas)
                return 4; //se asigno por defecto registros de ME_HOJAPTOMED un hojacodi 1 pero este solo puede estar asignado al formato PRESION DE GAS Y TEMPERATURA

            DateTime fechaActualizacion = DateTime.Now;

            //tipo de copia especial
            if (formatcodi == ConstantesIEOD.IdFormatoDemandaDiaria) //tipoCopia == 2
                this.EjecutarCopiaConfiguracionCaso2DemandaDiaria(usuario, fechaActualizacion);

            if (tipoCopia == 1) //en el mismo formato se tiene MW y Mvar
                this.EjecutarCopiaConfiguracionCaso1(formatcodi, usuario, fechaActualizacion);

            if (tipoCopia == 3) //copiar el formato a su destino
                this.EjecutarCopiaConfiguracionCaso3(formatcodi, usuario, fechaActualizacion, true);

            return 1;
        }

        private void EjecutarCopiaConfiguracionCaso1(int formatcodi, string usuario, DateTime fechaActualizacion)
        {
            List<MeHojaDTO> listaHoja = FactorySic.GetMeHojaRepository().List();

            //Verificar si el formato tiene un ME_HOJA, esto es necesario para el ME_HOJAPTOMED
            List<MeHojaDTO> listaHojaxFmt = listaHoja.Where(x => x.Formatcodi == formatcodi).ToList();

            List<MeFormatoDTO> listaFormato = this.ListMeFormatos();

            List<MeHojaptomedDTO> listaHptoNuevo = new List<MeHojaptomedDTO>();
            List<MeHojaptomedDTO> listaHptoUpdate = new List<MeHojaptomedDTO>();

            //Obtener configuracion de puntos
            List<MeHojaptomedDTO> listaHpto = FactorySic.GetMeHojaptomedRepository().ListByFormatcodi(formatcodi.ToString());

            //1. MW MVar
            if (listaHojaxFmt.Count() == 1) //formato con una sola hoja
            {
                MeHojaDTO hoja = listaHoja.Find(x => x.Formatcodi == formatcodi);
                List<int> listaPtomedicodi = listaHpto.Where(x => x.Tipoinfocodi == ConstantesTipoInformacion.TipoinfoMW || x.Tipoinfocodi == ConstantesTipoInformacion.TipoinfoMVar).Select(x => x.Ptomedicodi).Distinct().ToList();

                List<MeHojaptomedDTO> listaHptoTienePto = listaHpto.Where(x => listaPtomedicodi.Contains(x.Ptomedicodi)).ToList();
                List<MeHojaptomedDTO> listaHptoNOTienePto = listaHpto.Where(x => !listaPtomedicodi.Contains(x.Ptomedicodi)).ToList();

                foreach (var ptomedicodi in listaPtomedicodi)
                {
                    List<MeHojaptomedDTO> lxPto = listaHptoTienePto.Where(x => x.Ptomedicodi == ptomedicodi).OrderBy(x => x.Tipoinfocodi).ToList(); //un punto puede tener MW, Mvar, kv, etc
                    var regPtodefault = lxPto.First();
                    int estadoPto = regPtodefault.Hojaptoactivo ?? 0;

                    var regMW = lxPto.Find(x => x.Tipoinfocodi == ConstantesTipoInformacion.TipoinfoMW);
                    var regMVar = lxPto.Find(x => x.Tipoinfocodi == ConstantesTipoInformacion.TipoinfoMVar);

                    if (regMW == null)
                    {
                        listaHptoNuevo.Add(new MeHojaptomedDTO()
                        {
                            Ptomedicodi = ptomedicodi,
                            Tipoinfocodi = ConstantesTipoInformacion.TipoinfoMW,
                            Formatcodi = formatcodi,
                            Hojaptosigno = regPtodefault.Hojaptosigno,
                            Hojaptoliminf = 0,
                            Hojaptolimsup = 10000,
                            Hojaptoactivo = regPtodefault.Hojaptoactivo,
                            Hojaptoorden = regPtodefault.Hojaptoorden,
                            Lastuser = usuario,
                            Lastdate = fechaActualizacion,
                            Hojacodi = hoja.Hojacodi,
                            Tptomedicodi = -1,
                            Hptoobservacion = ""
                        });
                    }
                    else
                    {
                        if (regMW.Hojaptoactivo != estadoPto)
                        {
                            regMW.Hojaptoactivo = estadoPto;
                            regMW.Lastuser = usuario;
                            regMW.Lastdate = fechaActualizacion;
                            listaHptoUpdate.Add(regMW);
                        }
                    }

                    if (regMVar == null)
                    {
                        listaHptoNuevo.Add(new MeHojaptomedDTO()
                        {
                            Ptomedicodi = ptomedicodi,
                            Tipoinfocodi = ConstantesTipoInformacion.TipoinfoMVar,
                            Formatcodi = formatcodi,
                            Hojaptosigno = regPtodefault.Hojaptosigno,
                            Hojaptoliminf = -1000,
                            Hojaptolimsup = 1000,
                            Hojaptoactivo = regPtodefault.Hojaptoactivo,
                            Hojaptoorden = regPtodefault.Hojaptoorden,
                            Lastuser = usuario,
                            Lastdate = fechaActualizacion,
                            Hojacodi = hoja.Hojacodi,
                            Tptomedicodi = -1,
                            Hptoobservacion = ""
                        });
                    }
                    else
                    {
                        if (regMVar.Hojaptoactivo != estadoPto)
                        {
                            regMVar.Hojaptoactivo = estadoPto;
                            regMVar.Lastuser = usuario;
                            regMVar.Lastdate = fechaActualizacion;
                            listaHptoUpdate.Add(regMVar);
                        }
                    }
                }

                //ejecutar insert / update
                foreach (var reg in listaHptoNuevo)
                {
                    reg.Lastuser = usuario;
                    reg.Lastdate = fechaActualizacion;
                    this.SaveMeHojaptomed(reg, 0);
                }
                foreach (var reg in listaHptoUpdate)
                {
                    reg.Lastuser = usuario;
                    reg.Lastdate = fechaActualizacion;
                    this.UpdateMeHojaptomed(reg);
                }
            }

            //2. Copiar a formatos dependientes
            this.EjecutarCopiaConfiguracionCaso3(formatcodi, usuario, fechaActualizacion, false);
        }

        private void EjecutarCopiaConfiguracionCaso2DemandaDiaria(string usuario, DateTime fechaActualizacion)
        {
            int formatcodi = ConstantesIEOD.IdFormatoDemandaDiaria;
            List<MeHojaDTO> listaHoja = FactorySic.GetMeHojaRepository().List();
            List<MeFormatoDTO> listaFormato = this.ListMeFormatos();

            MeHojaDTO hojaPrincipal = listaHoja.Find(x => x.Formatcodi == formatcodi && x.Hojaorden == 1);
            List<int> listaFormatcodiDependiente = listaFormato.Where(x => x.Formatdependeconfigptos == formatcodi).Select(x => x.Formatcodi).Distinct().ToList();

            //hojas dependientes
            List<MeHojaDTO> listaHojaSec = listaHoja.Where(x => x.Formatcodi == formatcodi && x.Hojaorden != 1).ToList();
            listaHojaSec.AddRange(listaHoja.Where(x => listaFormatcodiDependiente.Contains(x.Formatcodi.Value)).ToList());

            //Obtener configuracion de puntos
            List<MeHojaptomedDTO> listaHpto = FactorySic.GetMeHojaptomedRepository().ListByFormatcodi(formatcodi.ToString());

            //puntos de la hoja principal
            List<MeHojaptomedDTO> paramLista1 = listaHpto.Where(x => x.Hojacodi == hojaPrincipal.Hojacodi).ToList();

            foreach (var regHojaSec in listaHojaSec)
            {
                List<MeHojaptomedDTO> listaHptoNuevo = new List<MeHojaptomedDTO>();
                List<MeHojaptomedDTO> listaHptoUpdate = new List<MeHojaptomedDTO>();
                List<MeHojaptomedDTO> listaHptoDelete = new List<MeHojaptomedDTO>();

                int tipoinfocodi2 = regHojaSec.Hojanombre.ToUpper().Contains("MW") ? ConstantesTipoInformacion.TipoinfoMW : ConstantesTipoInformacion.TipoinfoMVar;
                List<MeHojaptomedDTO> paramLista2 = FactorySic.GetMeHojaptomedRepository().ListByFormatcodi(regHojaSec.Formatcodi.Value.ToString()).Where(x => x.Hojacodi == regHojaSec.Hojacodi).ToList();

                this.CopiarPtosOrigMWAPtosMvar(ConstantesTipoInformacion.TipoinfoMW, paramLista1
                                        , regHojaSec.Formatcodi.Value, regHojaSec.Hojacodi, tipoinfocodi2, paramLista2
                                        , out listaHptoNuevo, out listaHptoUpdate, out listaHptoDelete);

                //ejecutar insert / update
                foreach (var reg in listaHptoNuevo)
                {
                    reg.Lastdate = fechaActualizacion;
                    reg.Lastuser = usuario;
                    this.SaveMeHojaptomed(reg, 0);
                }
                foreach (var reg in listaHptoUpdate)
                {
                    reg.Lastdate = fechaActualizacion;
                    reg.Lastuser = usuario;
                    this.UpdateMeHojaptomed(reg);
                }
                foreach (var reg in listaHptoDelete)
                {
                    this.DeleteMeHojaptomedById(reg.Hojaptomedcodi);
                }
            }
        }

        private void EjecutarCopiaConfiguracionCaso3(int formatcodi, string usuario, DateTime fechaActualizacion, bool esObligatorioTenerDependientes)
        {
            List<MeFormatoDTO> listaFormato = this.ListMeFormatos();
            List<MeHojaDTO> listaHoja = FactorySic.GetMeHojaRepository().List();

            List<MeHojaptomedDTO> listaHptoOrigen = FactorySic.GetMeHojaptomedRepository().ListByFormatcodi(formatcodi.ToString());

            List<int> listaFormatcodiDependiente = listaFormato.Where(x => x.Formatdependeconfigptos == formatcodi).Select(x => x.Formatcodi).Distinct().ToList();
            List<MeHojaDTO> listaHojaSec = listaHoja.Where(x => listaFormatcodiDependiente.Contains(x.Formatcodi.Value)).ToList();

            if (esObligatorioTenerDependientes)
                if (!listaHojaSec.Any())
                    throw new Exception("El formato origen no tiene formatos dependientes. (Falta configuración en la tabla ME_HOJA)");

            foreach (var regHojaSec in listaHojaSec)
            {
                List<MeHojaptomedDTO> listaHptoNuevo = new List<MeHojaptomedDTO>();
                List<MeHojaptomedDTO> listaHptoUpdate = new List<MeHojaptomedDTO>();
                List<MeHojaptomedDTO> listaHptoDelete = new List<MeHojaptomedDTO>();

                List<MeHojaptomedDTO> listaHptoDependiente = FactorySic.GetMeHojaptomedRepository().ListByFormatcodi(regHojaSec.Formatcodi.Value.ToString()).Where(x => x.Hojacodi == regHojaSec.Hojacodi).ToList();
                this.CopiarPtosOrigAPtosDependientes(regHojaSec.Formatcodi.Value, regHojaSec.Hojacodi, listaHptoOrigen, listaHptoDependiente, out listaHptoNuevo, out listaHptoUpdate, out listaHptoDelete);

                //ejecutar insert / update
                foreach (var reg in listaHptoNuevo)
                {
                    reg.Lastuser = usuario;
                    reg.Lastdate = fechaActualizacion;
                    this.SaveMeHojaptomed(reg, 0);
                }
                foreach (var reg in listaHptoUpdate)
                {
                    reg.Lastuser = usuario;
                    reg.Lastdate = fechaActualizacion;
                    this.UpdateMeHojaptomed(reg);
                }
                foreach (var reg in listaHptoDelete)
                {
                    this.DeleteMeHojaptomedById(reg.Hojaptomedcodi);
                }
            }
        }

        private void CopiarPtosOrigMWAPtosMvar(int tipoinfo1, List<MeHojaptomedDTO> paramLista1, int formatcodi2, int hojacodi2, int tipoinfo2, List<MeHojaptomedDTO> paramLista2
            , out List<MeHojaptomedDTO> listaHptoNuevo, out List<MeHojaptomedDTO> listaHptoUpdate, out List<MeHojaptomedDTO> listaHptoDelete)
        {
            listaHptoNuevo = new List<MeHojaptomedDTO>();
            listaHptoUpdate = new List<MeHojaptomedDTO>();
            listaHptoDelete = new List<MeHojaptomedDTO>();

            //Adecuar la lista origen
            List<MeHojaptomedDTO> lista1 = paramLista1.Where(x => x.Tipoinfocodi == tipoinfo1).ToList();
            List<MeHojaptomedDTO> lista1AEliminar = paramLista1.Where(x => x.Tipoinfocodi != tipoinfo1).ToList();

            foreach (var reg in lista1AEliminar)
                reg.Hojaptoactivo = -1;
            listaHptoDelete.AddRange(lista1AEliminar);

            //Adecuar la lista destino
            List<MeHojaptomedDTO> lista2 = paramLista2.Where(x => x.Tipoinfocodi == tipoinfo2).ToList();
            List<MeHojaptomedDTO> lista2AEliminar = paramLista2.Where(x => x.Tipoinfocodi != tipoinfo2).ToList();

            foreach (var reg in lista2AEliminar)
                reg.Hojaptoactivo = -1;
            listaHptoDelete.AddRange(lista2AEliminar);

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
            List<int> listaPtomedicodi = lista1.Select(x => x.Ptomedicodi).Distinct().ToList();

            List<MeHojaptomedDTO> lista2HptoTienePto = lista2.Where(x => listaPtomedicodi.Contains(x.Ptomedicodi)).ToList();
            List<MeHojaptomedDTO> lista2HptoNOTienePto = lista2.Where(x => !listaPtomedicodi.Contains(x.Ptomedicodi)).ToList();

            //Eliminar logicamente los puntos de medicion que no estan en el formato origen
            foreach (var reg in lista2HptoNOTienePto)
                reg.Hojaptoactivo = -1;
            listaHptoDelete.AddRange(lista2HptoNOTienePto);


            foreach (var ptomedicodi in listaPtomedicodi)
            {
                if (ptomedicodi == 47243) 
                { }

                List<MeHojaptomedDTO> lxPtoTipoOrig = lista1.Where(x => x.Ptomedicodi == ptomedicodi).ToList();
                List<MeHojaptomedDTO> lxPtoTipoEnDepen = lista2.Where(x => x.Ptomedicodi == ptomedicodi).ToList();

                var regMW = lxPtoTipoOrig.FirstOrDefault();
                var regSec = lxPtoTipoEnDepen.FirstOrDefault();

                if (regSec == null)
                {
                    listaHptoNuevo.Add(new MeHojaptomedDTO()
                    {
                        Ptomedicodi = ptomedicodi,
                        Tipoinfocodi = tipoinfo2,
                        Formatcodi = formatcodi2,
                        Hojaptosigno = regMW.Hojaptosigno,
                        Hojaptoliminf = ConstantesTipoInformacion.TipoinfoMVar == tipoinfo2 ? -1000 : 0,
                        Hojaptolimsup = ConstantesTipoInformacion.TipoinfoMVar == tipoinfo2 ? 1000 : 10000,
                        Hojaptoactivo = regMW.Hojaptoactivo,
                        Hojaptoorden = regMW.Hojaptoorden,
                        Hojacodi = hojacodi2,
                        Tptomedicodi = ConstantesTipoInformacion.TipoinfoMVar == tipoinfo2 ? - 1 : 15,
                        Hptoobservacion = ""
                    });
                }
                else
                {
                    if (regSec.Hojaptoactivo != regMW.Hojaptoactivo)
                    {
                        regSec.Hojaptoactivo = regMW.Hojaptoactivo;
                        listaHptoUpdate.Add(regSec);
                    }

                    if (regSec.Hojaptoorden != regMW.Hojaptoorden)
                    {
                        regSec.Hojaptoorden = regMW.Hojaptoorden;
                        listaHptoUpdate.Add(regSec);
                    }
                }

                //Eliminar logicamente los puntos de medicion duplicados
                List<MeHojaptomedDTO> listaHptoDuplicados = lxPtoTipoEnDepen.Where(x => x.Hojaptomedcodi != regSec.Hojaptomedcodi).ToList();
                foreach (var reg in listaHptoDuplicados)
                    reg.Hojaptoactivo = -1;
                listaHptoDelete.AddRange(listaHptoDuplicados);
            }
        }

        private void CopiarPtosOrigAPtosDependientes(int formatcodiDependiente, int hojacodiDependiente
        , List<MeHojaptomedDTO> listaHptoOrigen, List<MeHojaptomedDTO> listaHptoDependiente
        , out List<MeHojaptomedDTO> listaHptoNuevo, out List<MeHojaptomedDTO> listaHptoUpdate, out List<MeHojaptomedDTO> listaHptoDelete)
        {
            listaHptoNuevo = new List<MeHojaptomedDTO>();
            listaHptoUpdate = new List<MeHojaptomedDTO>();
            listaHptoDelete = new List<MeHojaptomedDTO>();

            List<int> listaPtomedicodi = listaHptoOrigen.Select(x => x.Ptomedicodi).Distinct().ToList();

            List<MeHojaptomedDTO> listaHptoTienePto = listaHptoDependiente.Where(x => listaPtomedicodi.Contains(x.Ptomedicodi)).ToList();
            List<MeHojaptomedDTO> listaHptoNOTienePto = listaHptoDependiente.Where(x => !listaPtomedicodi.Contains(x.Ptomedicodi)).ToList();

            //Eliminar logicamente los puntos de medicion que no estan en el formato origen
            foreach (var reg in listaHptoNOTienePto)
                reg.Hojaptoactivo = -1;
            listaHptoUpdate.AddRange(listaHptoNOTienePto);

            foreach (var ptomedicodi in listaPtomedicodi)
            {
                List<MeHojaptomedDTO> lxPtoOrig = listaHptoOrigen.Where(x => x.Ptomedicodi == ptomedicodi).ToList(); //un punto puede tener MW, Mvar, kv, etc
                List<MeHojaptomedDTO> lxPtoDepen = listaHptoDependiente.Where(x => x.Ptomedicodi == ptomedicodi).ToList(); //un punto puede tener MW, Mvar, kv, etc

                List<int> listaTipoinfocodiXPto = lxPtoOrig.Select(x => x.Tipoinfocodi).Distinct().ToList();

                foreach (var tipoinfocodi in listaTipoinfocodiXPto)
                {
                    List<MeHojaptomedDTO> lxPtoTipoOrig = lxPtoOrig.Where(x => x.Tipoinfocodi == tipoinfocodi).ToList();
                    List<MeHojaptomedDTO> lxPtoTipoEnDepen = lxPtoDepen.Where(x => x.Tipoinfocodi == tipoinfocodi).ToList();

                    List<int> listaTptomedicodiXPto = lxPtoTipoOrig.Select(x => x.Tptomedicodi).Distinct().ToList();
                    foreach (var tptomedicodi in listaTptomedicodiXPto)
                    {
                        List<MeHojaptomedDTO> lxTptoOrig = lxPtoTipoOrig.Where(x => x.Tptomedicodi == tptomedicodi).ToList();
                        List<MeHojaptomedDTO> lxTptoEnDepen = lxPtoTipoEnDepen.Where(x => x.Tptomedicodi == tptomedicodi).ToList();

                        MeHojaptomedDTO regTptoOrig = lxTptoOrig.FirstOrDefault();
                        MeHojaptomedDTO regTptoEnDepen = lxTptoEnDepen.FirstOrDefault();

                        if (regTptoEnDepen != null)
                        {
                            var existeCambio = false;
                            decimal? modHojaptoliminf = regTptoEnDepen.Hojaptoliminf;
                            decimal? modHojaptolimsup = regTptoEnDepen.Hojaptolimsup;
                            int? modHojaptoactivo = regTptoEnDepen.Hojaptoactivo;
                            int modHojaptoorden = regTptoEnDepen.Hojaptoorden;
                            string modHptoobservacion = regTptoEnDepen.Hptoobservacion;

                            if (regTptoEnDepen.Hojaptoliminf != regTptoOrig.Hojaptoliminf)
                            {
                                modHojaptoliminf = regTptoOrig.Hojaptoliminf;
                                existeCambio = true;
                            }
                            if (regTptoEnDepen.Hojaptolimsup != regTptoOrig.Hojaptolimsup)
                            {
                                modHojaptolimsup = regTptoOrig.Hojaptolimsup;
                                existeCambio = true;
                            }
                            if (regTptoEnDepen.Hojaptoactivo != regTptoOrig.Hojaptoactivo)
                            {
                                modHojaptoactivo = regTptoOrig.Hojaptoactivo;
                                existeCambio = true;
                            }
                            if (regTptoEnDepen.Hojaptoorden != regTptoOrig.Hojaptoorden)
                            {
                                modHojaptoorden = regTptoOrig.Hojaptoorden;
                                existeCambio = true;
                            }
                            if (regTptoEnDepen.Hptoobservacion != regTptoOrig.Hptoobservacion)
                            {
                                modHptoobservacion = regTptoOrig.Hptoobservacion;
                                existeCambio = true;
                            }

                            if (existeCambio)
                            {
                                regTptoEnDepen.Hojaptoliminf = modHojaptoliminf;
                                regTptoEnDepen.Hojaptolimsup = modHojaptolimsup;
                                regTptoEnDepen.Hojaptoactivo = modHojaptoactivo;
                                regTptoEnDepen.Hojaptoorden = modHojaptoorden;
                                regTptoEnDepen.Hptoobservacion = modHptoobservacion;
                                listaHptoUpdate.Add(regTptoEnDepen);
                            }

                            //Eliminar logicamente los puntos de medicion duplicados
                            List<MeHojaptomedDTO> listaHptoDuplicados = lxTptoEnDepen.Where(x => x.Hojaptomedcodi != regTptoEnDepen.Hojaptomedcodi).ToList();
                            foreach (var reg in listaHptoDuplicados)
                                reg.Hojaptoactivo = -1;
                            listaHptoDelete.AddRange(listaHptoDuplicados);

                        }
                        else
                        {
                            listaHptoNuevo.Add(new MeHojaptomedDTO()
                            {
                                Ptomedicodi = ptomedicodi,
                                Tipoinfocodi = regTptoOrig.Tipoinfocodi,
                                Formatcodi = formatcodiDependiente,
                                Hojaptosigno = regTptoOrig.Hojaptosigno,
                                Hojaptoliminf = regTptoOrig.Hojaptoliminf,
                                Hojaptolimsup = regTptoOrig.Hojaptolimsup,
                                Hojaptoactivo = regTptoOrig.Hojaptoactivo,
                                Hojaptoorden = regTptoOrig.Hojaptoorden,
                                Hojacodi = hojacodiDependiente,
                                Tptomedicodi = regTptoOrig.Tptomedicodi,
                                Hptoobservacion = regTptoOrig.Hptoobservacion
                            });
                        }

                    }

                    //puntos del formato dependiente que ya no estan en el formato origen
                    List<MeHojaptomedDTO> listaHptoNOTieneTpto = lxPtoTipoEnDepen.Where(x => !listaTptomedicodiXPto.Contains(x.Tptomedicodi)).ToList();
                    //Eliminar logicamente los puntos de medicion cuyos tipoinfocodi y tpto no estan en el formato origen
                    foreach (var reg in listaHptoNOTieneTpto)
                        reg.Hojaptoactivo = -1;
                    listaHptoDelete.AddRange(listaHptoNOTieneTpto);
                }

                //puntos del formato dependiente que ya no estan en el formato origen
                List<MeHojaptomedDTO> listaHptoNOTieneTipoinfocodi = lxPtoDepen.Where(x => !listaTipoinfocodiXPto.Contains(x.Tipoinfocodi)).ToList();
                //Eliminar logicamente los puntos de medicion cuyos tipoinfocodi no estan en el formato origen
                foreach (var reg in listaHptoNOTieneTipoinfocodi)
                    reg.Hojaptoactivo = -1;
                listaHptoDelete.AddRange(listaHptoNOTieneTipoinfocodi);
            }
        }

        #endregion

        #region Mejoras RDO
        /// <summary>
        /// Lista formato de despacho generación
        /// </summary>
        public List<MeFormatoDTO> ListarFormatosDespachoGeneracion()
        {
            var listaFormato = this.ListMeFormatos();
            var listaFiltroFormato = new List<MeFormatoDTO>();
            listaFiltroFormato.Add(listaFormato.Where(x => x.Formatcodi == ConstantesFormatoMedicion.IdFormatoGeneracionDespachoDiario).FirstOrDefault());

            return listaFiltroFormato;
        }

        /// <summary>
        /// Inserta un registro de la tabla ME_ENVIO_HORARIO
        /// </summary>
        public void SaveHorarioMeEnvio(int idEnvio, int horario)
        {
            try
            {
                FactorySic.GetMeEnvioRepository().SaveHorario(idEnvio, horario);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla MeEnvio
        /// </summary>
        public List<MeEnvioDTO> GetByCriteriaCaudalVolumenMeEnvios(int idFormato, DateTime fecha)
        {
            return FactorySic.GetMeEnvioRepository().GetByCriteriaCaudalVolumen(idFormato, fecha);
        }

        /// <summary>
        /// Se obtiene los datos enviados de un formato de la BD.
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="formato"></param>
        /// <param name="idEnvio"></param>
        /// <returns></returns>
        public List<Object> GetDataFormatoEjecutados(int idEmpresa, MeFormatoDTO formato, int idEnvio, int idUltimoEnvio, string horario)
        {
            List<Object> listaGenerica = new List<Object>();

            //asignar codigo de formato temporalmente
            var formatoValidate = formato.Formatdependeconfigptos != null ? (int)formato.Formatdependeconfigptos : formato.Formatcodi;
            var formatcodiInicio = formato.Formatcodi;
            formato.Formatcodi = formatoValidate;

            switch (formato.Formatresolucion)
            {
                case ParametrosFormato.ResolucionHora:
                    List<MeMedicion24DTO> lista24 = FactorySic.GetMeMedicion24Repository().GetEnvioArchivoEjecutados(formato.Formatcodi, idEmpresa, formato.FechaInicio.AddDays(-1), formato.FechaInicio, horario);
                    if (idEnvio != 0)
                    {
                        var lista = GetAllCambioEnvio(formato.Formatcodi, formato.FechaInicio, formato.FechaFin, idEnvio, idEmpresa);
                        if (lista.Count > 0)
                        {
                            foreach (var reg in lista)
                            {
                                var find = lista24.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Tipoinfocodi == reg.Tipoinfocodi &&
                                    x.Medifecha == reg.Cambenvfecha);
                                if (find != null)
                                {
                                    var fila = reg.Cambenvdatos.Split(',');
                                    for (var i = 0; i < 24; i++)
                                    {
                                        decimal dato;
                                        decimal? numero = null;
                                        if (decimal.TryParse(fila[i], out dato))
                                            numero = dato;
                                        find.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(find, numero);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (formato.Formatsecundario != 0 && lista24.Count == 0)
                        {
                            lista24 = FactorySic.GetMeMedicion24Repository().GetDataFormatoSecundario(formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin);
                        }
                    }
                    foreach (var reg in lista24)
                    {
                        listaGenerica.Add(reg);
                    }
                    break;
                case ParametrosFormato.ResolucionMediaHora:
                    List<MeMedicion48DTO> lista48 = new List<MeMedicion48DTO>();
                    if (!formato.FlagUtilizaHoja)
                    {
                        lista48 = FactorySic.GetMeMedicion48Repository().GetEnvioArchivoEjecutados(formato.Formatcodi, idEmpresa.ToString(), formato.FechaInicio.AddDays(-1), formato.FechaInicio, formato.Lectcodi, horario);
                    }
                    else
                    {
                        lista48 = FactorySic.GetMeMedicion48Repository().GetEnvioArchivo2(formato.Formatcodi, idEmpresa.ToString(), formato.FechaInicio.AddDays(-1), formato.FechaInicio);
                    }

                    foreach (var reg in lista48)
                    {
                        listaGenerica.Add(reg);
                    }
                    break;

                case ParametrosFormato.ResolucionCuartoHora:
                    List<MeMedicion96DTO> lista96 = FactorySic.GetMeMedicion96Repository().GetEnvioArchivo(formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin);
                    if (idEnvio != 0)
                    {
                        var lista = GetAllCambioEnvio(formato.Formatcodi, formato.FechaInicio, formato.FechaFin, idEnvio, idEmpresa);
                        if (lista.Count > 0)
                        {
                            foreach (var reg in lista)
                            {
                                var find = lista96.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Tipoinfocodi == reg.Tipoinfocodi &&
                                    x.Medifecha == reg.Cambenvfecha);
                                if (find != null)
                                {
                                    var fila = reg.Cambenvdatos.Split(',');
                                    for (var i = 0; i < 96; i++)
                                    {
                                        decimal dato;
                                        decimal? numero = null;
                                        if (decimal.TryParse(fila[i], out dato))
                                            numero = dato;
                                        find.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(find, numero);
                                    }

                                }
                            }
                        }
                    }

                    foreach (var reg in lista96)
                    {
                        listaGenerica.Add(reg);
                    }
                    break;
            }

            formato.Formatcodi = formatcodiInicio;
            return listaGenerica;
        }

        /// <summary>
        /// Permite grabar los datos ejecutados
        /// </summary>
        /// <param name="entitys"></param>
        public void GrabarValoresEjecutados24(List<MeMedicion24DTO> entitys, string usuario, int idEnvio, int idEmpresa, MeFormatoDTO formato)
        {
            try
            {
                foreach (MeMedicion24DTO entity in entitys)
                {
                    FactorySic.GetMeMedicion24Repository().SaveEjecutados(entity, idEnvio, usuario, idEmpresa);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite grabar los datos cargados
        /// </summary>
        /// <param name="entitys"></param>
        public void GrabarValoresEjecutados48(List<MeMedicion48DTO> entitys, string usuario, int idEnvio, int idEmpresa, MeFormatoDTO formato)
        {
            try
            {
                foreach (MeMedicion48DTO entity in entitys)
                {
                    FactorySic.GetMeMedicion48Repository().SaveMeMedicion48Ejecutados(entity, idEnvio, usuario, idEmpresa);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        private string[][] GetMatrizExcelRDO(List<string> data, int colHead, int nCol, int filHead, int nFil)
        {
            var inicio = (nCol + colHead - 1) * filHead + nCol;
            int col = 0;
            int fila = 0;
            string[][] arreglo = new string[nFil][];
            arreglo[0] = new string[nCol];
            for (int i = inicio; i < data.Count(); i++)
            {
                string valor = data[i];
                if (col == nCol)
                {
                    fila++;
                    col = 0;
                    arreglo[fila] = new string[nCol];
                }
                arreglo[fila][col] = valor;
                col++;
            }
            return arreglo;
        }

        /// <summary>
        /// Carga la data recibida del excel web a una lista de Medidion24DTO
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ptos"></param>
        /// <param name="idLectura"></param>
        /// <param name="colHead"></param>
        /// <param name="nCol"></param>
        /// <param name="filaHead"></param>
        /// <param name="nFil"></param>
        /// <returns></returns>
        public List<MeMedicion24DTO> LeerExcelWebEjecutados24(List<string> data, List<MeHojaptomedDTO> ptos, int idLectura, int colHead, int nCol, int filaHead, int nFil)
        {
            var matriz = GetMatrizExcelRDO(data, colHead, nCol, filaHead, nFil);
            List<MeMedicion24DTO> lista = new List<MeMedicion24DTO>();
            MeMedicion24DTO reg = new MeMedicion24DTO();
            string stValor = string.Empty;
            decimal valor = decimal.MinValue;
            DateTime fecha = DateTime.MinValue;
            for (var i = 1; i < nCol; i++)
            {
                for (var j = 0; j < nFil; j++)
                {
                    //verificar inicio de dia
                    if ((j % 24) == 0)
                    {
                        if (j != 0)
                            lista.Add(reg);
                        reg = new MeMedicion24DTO();
                        reg.Ptomedicodi = ptos[i - 1].Ptomedicodi;
                        reg.Lectcodi = idLectura;
                        reg.Tipoinfocodi = (int)ptos[i - 1].Tipoinfocodi;
                        reg.Emprcodi = ptos[i - 1].Emprcodi;
                        fecha = DateTime.ParseExact(matriz[j][0], ConstantesFormat.FormatoFechaHora, CultureInfo.InvariantCulture);
                        reg.Medifecha = new DateTime(fecha.Year, fecha.Month, fecha.Day);
                        stValor = matriz[j][i];
                        if (COES.Base.Tools.Util.EsNumero(stValor))
                        {
                            valor = decimal.Parse(stValor);
                            reg.H1 = valor;
                        }
                        else
                            reg.H1 = null;
                    }
                    else
                    {
                        stValor = matriz[j][i];
                        int indice = j % 24 + 1;
                        if (COES.Base.Tools.Util.EsNumero(stValor))
                        {
                            valor = decimal.Parse(stValor);

                            reg.GetType().GetProperty("H" + indice.ToString()).SetValue(reg, valor);
                        }
                        else
                            reg.GetType().GetProperty("H" + indice.ToString()).SetValue(reg, null);

                    }
                }
                lista.Add(reg);
            }
            return lista;
        }

        /// <summary>
        /// Permite grabar los datos cargados
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="usuario"></param>
        /// <param name="idEnvio"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="formato"></param>
        public void GrabarValoresCargados24_Intranet(List<MeMedicion24DTO> entitys, string usuario, int idEnvio, int idEmpresa, MeFormatoDTO formato)
        {
            try
            {
                List<MeMedicion24DTO> listaExtranet24 = new List<MeMedicion24DTO>();
                List<MeMedicion24DTO> listaIntranet24 = new List<MeMedicion24DTO>();

                foreach (var regxEnv in entitys)
                {
                    listaExtranet24 = FactorySic.GetMeMedicion24Repository().GetEnvioArchivoEjecutados(formato.Formatcodi, idEmpresa, formato.FechaInicio.AddDays(-1), formato.FechaInicio, "0");
                    listaIntranet24 = FactorySic.GetMeMedicion24Repository().GetEnvioMeMedicion24Intranet(formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaInicio);

                    var findExt = listaExtranet24.Find(x => x.Ptomedicodi == regxEnv.Ptomedicodi && x.Lectcodi == regxEnv.Lectcodi && x.Emprcodi == regxEnv.Emprcodi);
                    var findInt = listaIntranet24.Find(x => x.Ptomedicodi == regxEnv.Ptomedicodi && x.Lectcodi == regxEnv.Lectcodi && x.Emprcodi == regxEnv.Emprcodi);

                    if (findExt == null)
                    {
                        findExt = new MeMedicion24DTO();
                        findExt.Ptomedicodi = regxEnv.Ptomedicodi;
                        findExt.Lectcodi = regxEnv.Lectcodi;
                        findExt.Emprcodi = regxEnv.Emprcodi;
                        findExt.Tipoinfocodi = regxEnv.Tipoinfocodi;
                    }

                    if (findExt != null)
                    {
                        for (var x = 0; x < 24; x++)
                        {
                            var valInt = regxEnv.GetType().GetProperty("H" + (x + 1)).GetValue(regxEnv, null);
                            var valExt = findExt.GetType().GetProperty("H" + (x + 1)).GetValue(findExt, null);

                            if (valInt != null && Convert.ToDecimal(valExt) != Convert.ToDecimal(valInt) && valInt.ToString() != "")
                            {
                                findExt.GetType().GetProperty("H" + (x + 1).ToString()).SetValue(findExt, Convert.ToDecimal(valInt));
                            }
                            else if (findInt != null)
                            {
                                var valIntOld = findInt.GetType().GetProperty("H" + (x + 1)).GetValue(findInt, null);
                                if (valIntOld != null && valIntOld.ToString() != "")
                                    findExt.GetType().GetProperty("H" + (x + 1).ToString()).SetValue(findExt, Convert.ToDecimal(valInt));
                                else
                                    findExt.GetType().GetProperty("H" + (x + 1).ToString()).SetValue(findExt, null);
                            }
                            else
                                findExt.GetType().GetProperty("H" + (x + 1).ToString()).SetValue(findExt, null);
                        }
                    }
                    findExt.Medifecha = formato.FechaProceso;
                    findExt.Lastdate = formato.Lastdate;
                    findExt.Lastuser = usuario;
                    FactorySic.GetMeMedicion24Repository().SaveIntranet(findExt, idEnvio);

                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene matriz de string que son solo los valores de las celdas del excel web ingresando como parametro una lista de mediciones.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="lista"></param>
        /// <param name="listaCambios"></param>
        /// <param name="idEnvio"></param>
        /// <param name="listaLimites"></param>
        public void ObtieneMatrizWebExcel24Ejecutados(FormatoModel model, List<MeMedicion24DTO> lista, List<MeCambioenvioDTO> listaCambios, int idEnvio, List<RDOLimiteHidrologiaCaudal> listaLimites)
        {
            decimal? _LimInf = 0;
            decimal? _LimMax = 0;
            try
            {
                if (idEnvio > 0)
                {
                    foreach (var reg in listaCambios)
                    {
                        if (reg.Cambenvcolvar != null)
                        {
                            var cambios = reg.Cambenvcolvar.Split(',');
                            for (var i = 0; i < cambios.Count(); i++)
                            {
                                TimeSpan ts = reg.Cambenvfecha - model.Formato.FechaInicio;
                                var horizon = ts.Days;
                                var col = model.ListaHojaPto.Find(x => x.Ptomedicodi == reg.Ptomedicodi).Hojaptoorden + model.ColumnasCabecera - 1;
                                var ColumReal = model.ListaHojaPto.FindIndex(x => x.Ptomedicodi == reg.Ptomedicodi) + 1;
                                col = ColumReal;
                                var row = model.FilasCabecera +
                                    ObtieneRowChange((int)model.Formato.Formatperiodo, (int)model.Formato.Formatresolucion, int.Parse(cambios[i]),
                                    model.Formato.RowPorDia, reg.Cambenvfecha, model.Formato.FechaInicio);
                                //int.Parse(cambios[i]) + model.Formato.RowPorDia * horizon;
                                model.ListaCambios.Add(new CeldaCambios()
                                {
                                    Row = row,
                                    Col = col
                                });
                            }
                        }
                    }
                }
                for (int k = 0; k < model.ListaHojaPto.Count; k++)
                {
                    for (int z = 0; z < model.Formato.Formathorizonte; z++) //Horizonte se comporta como uno cuando resolucion es mas que un dia
                    {
                        DateTime fechaFind = GetNextFilaHorizonte((int)model.Formato.Formatperiodo, (int)model.Formato.Formatresolucion, z, model.Formato.FechaInicio);
                        var reg = lista.Find(i => (int)i.GetType().GetProperty("Ptomedicodi").GetValue(i, null) == model.ListaHojaPto[k].Ptomedicodi
                                    && (DateTime)i.GetType().GetProperty("Medifecha").GetValue(i, null) == fechaFind && (int)i.GetType().GetProperty("Tipoinfocodi").GetValue(i, null) == model.ListaHojaPto[k].Tipoinfocodi);

                        if(reg == null)
                        {
                            DateTime fechaFindx = fechaFind.AddDays(-1);
                            reg = lista.Find(i => (int)i.GetType().GetProperty("Ptomedicodi").GetValue(i, null) == model.ListaHojaPto[k].Ptomedicodi
                                && (DateTime)i.GetType().GetProperty("Medifecha").GetValue(i, null) == fechaFindx && (int)i.GetType().GetProperty("Tipoinfocodi").GetValue(i, null) == model.ListaHojaPto[k].Tipoinfocodi);
                        }
                            
                        for (int j = 1; j <= model.Formato.RowPorDia; j++) // nBlock se comporta como horizonte cuando resolucion es mas de un dia
                        {
                            if (k == 0)
                            {
                                int jIni = 0;
                                if (model.Formato.Formatresolucion >= ParametrosFormato.ResolucionDia)
                                    jIni = j - 1;
                                else
                                    jIni = j - 1;

                                model.Handson.ListaExcelData[z * model.Formato.RowPorDia + j + model.FilasCabecera - 1][model.ColumnasCabecera - 1] =
                                   // ((model.FechaInicio.AddMinutes(z * ParametrosFormato.ResolucionDia + jIni * (int)model.Formato.Formatresolucion))).ToString(Constantes.FormatoFechaHora);
                                   ObtenerCeldaFecha((int)model.Formato.Formatperiodo, (int)model.Formato.Formatresolucion, model.Formato.Lecttipo, z, jIni, model.Formato.FechaInicio);
                            }
                            if (reg != null)
                            {
                                decimal? valor = (decimal?)reg.GetType().GetProperty("H" + j).GetValue(reg, null);
                                string _estado = (string)reg.GetType().GetProperty("E" + j).GetValue(reg, null);
                                if (valor != null)
                                {
                                    model.Handson.ListaExcelData[z * model.Formato.RowPorDia + j + model.FilasCabecera - 1][k + model.ColumnasCabecera] = valor.ToString();
                                    model.Handson.ListaExcelDataEjecutados[z * model.Formato.RowPorDia + j + model.FilasCabecera - 1][k + model.ColumnasCabecera] = _estado;

                                }

                                var limit = listaLimites.Where(x => x.Ptomedicodi == Convert.ToInt32(reg.GetType().GetProperty("Ptomedicodi").GetValue(reg, null))).SingleOrDefault();
                                if (limit != null)
                                {
                                    _LimInf = (decimal)limit.ArrayMinimo[j - 1];
                                    _LimMax = (decimal)limit.ArrayMaximo[j - 1];
                                }

                                model.Handson.ListaLimitesMinYupana[z * model.Formato.RowPorDia + j + model.FilasCabecera - 1][k + model.ColumnasCabecera] = _LimInf.ToString();
                                model.Handson.ListaLimitesMaxYupana[z * model.Formato.RowPorDia + j + model.FilasCabecera - 1][k + model.ColumnasCabecera] = _LimMax.ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }


        }

        /// <summary>
        /// Obtiene la lista de datos de generación RerNC
        /// </summary>
        /// <param name="model"></param>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public List<Object> ObtenerDatosRerNC(FormatoModel model, int idEmpresa)
        {
            //>>>>>>>>>>>>>>>>>>>>>>>>>>Obtener datos de Generación Rer
            List<Object> listaGeneracionRer = new List<Object>();
            DateTime fechaInicial = DateTime.Now;
            DateTime fechaFinal = DateTime.Now;
            var horizonte = model.Formato.Lectcodi == ConstantesMedicionesCD.FormatoSemanalCodi ? 1 : 0;
            fechaInicial = model.Formato.FechaInicio;
            fechaFinal = horizonte == 0 ? fechaInicial : model.Formato.FechaFin;

            listaGeneracionRer = servRer.ConsultaDatosRerNC(idEmpresa, horizonte, fechaInicial, fechaFinal);

            return listaGeneracionRer;
        }

        /// <summary>
        /// Obtiene matriz de string que son solo los valores de las celdas del excel web ingresando como parametro una lista de mediciones.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="lista"></param>
        /// <param name="listaCambios"></param>
        /// <param name="idEnvio"></param>
        /// <param name="listaLimites"></param>
        public void ObtieneMatrizWebExcel48Ejecutados(FormatoModel model, List<object> lista, List<MeCambioenvioDTO> listaCambios, int idEnvio, List<RDOLimiteRer> listaLimites)
        {
            if (idEnvio > 0)
            {
                foreach (var reg in listaCambios)
                {
                    if (reg.Cambenvcolvar != null)
                    {
                        var cambios = reg.Cambenvcolvar.Split(',');
                        for (var i = 0; i < cambios.Count(); i++)
                        {
                            TimeSpan ts = reg.Cambenvfecha - model.Formato.FechaInicio;
                            var horizon = ts.Days;
                            var col = model.ListaHojaPto.Find(x => x.Ptomedicodi == reg.Ptomedicodi).Hojaptoorden + model.ColumnasCabecera - 1;
                            var ColumReal = model.ListaHojaPto.FindIndex(x => x.Ptomedicodi == reg.Ptomedicodi) + 1;
                            col = ColumReal;
                            var row = model.FilasCabecera +
                                ObtieneRowChange((int)model.Formato.Formatperiodo, (int)model.Formato.Formatresolucion, int.Parse(cambios[i]),
                                model.Formato.RowPorDia, reg.Cambenvfecha, model.Formato.FechaInicio);
                            //int.Parse(cambios[i]) + model.Formato.RowPorDia * horizon;
                            model.ListaCambios.Add(new CeldaCambios()
                            {
                                Row = row,
                                Col = col
                            });
                        }
                    }
                }
            }
            for (int k = 0; k < model.ListaHojaPto.Count; k++)
            {
                for (int z = 0; z < model.Formato.Formathorizonte; z++) //Horizonte se comporta como uno cuando resolucion es mas que un dia
                {
                    DateTime fechaFind = GetNextFilaHorizonte((int)model.Formato.Formatperiodo, (int)model.Formato.Formatresolucion, z, model.Formato.FechaInicio);
                    var reg = lista.Find(i => (int)i.GetType().GetProperty("Ptomedicodi").GetValue(i, null) == model.ListaHojaPto[k].Ptomedicodi
                                && (DateTime)i.GetType().GetProperty("Medifecha").GetValue(i, null) == fechaFind);

                    for (int j = 1; j <= model.Formato.RowPorDia; j++) // nBlock se comporta como horizonte cuando resolucion es mas de un dia
                    {
                        if (k == 0)
                        {
                            int jIni = 0;
                            if (model.Formato.Formatresolucion >= ParametrosFormato.ResolucionDia)
                                jIni = j - 1;
                            else
                                jIni = j;

                            model.Handson.ListaExcelData[z * model.Formato.RowPorDia + j + model.FilasCabecera - 1][model.ColumnasCabecera - 1] =
                               // ((model.FechaInicio.AddMinutes(z * ParametrosFormato.ResolucionDia + jIni * (int)model.Formato.Formatresolucion))).ToString(Constantes.FormatoFechaHora);
                               ObtenerCeldaFecha((int)model.Formato.Formatperiodo, (int)model.Formato.Formatresolucion, model.Formato.Lecttipo, z, jIni, model.Formato.FechaInicio);
                        }
                        if (reg != null)
                        {
                            decimal? valor = (decimal?)reg.GetType().GetProperty("H" + j).GetValue(reg, null);
                            string _estado = reg.GetType().GetProperty("E" + j).GetValue(reg, null).ToString();
                            if (valor != null)
                            {
                                model.Handson.ListaExcelData[z * model.Formato.RowPorDia + j + model.FilasCabecera - 1][k + model.ColumnasCabecera] = valor.ToString();
                                model.Handson.ListaExcelDataEjecutados[z * model.Formato.RowPorDia + j + model.FilasCabecera - 1][k + model.ColumnasCabecera] = _estado;

                            }

                            var limit = listaLimites.Where(x => x.Ptomedicodi == Convert.ToInt32(reg.GetType().GetProperty("Ptomedicodi").GetValue(reg, null))).SingleOrDefault();
                            if (limit != null)
                            {
                                var _LimInf = limit.GetType().GetProperty("Pmin").GetValue(limit, null);
                                var _LimMax = limit.GetType().GetProperty("Pmax").GetValue(limit, null);
                                model.Handson.ListaLimitesMinYupana[z * model.Formato.RowPorDia + j + model.FilasCabecera - 1][k + model.ColumnasCabecera] = _LimInf == null ? "0" : _LimInf.ToString();
                                model.Handson.ListaLimitesMaxYupana[z * model.Formato.RowPorDia + j + model.FilasCabecera - 1][k + model.ColumnasCabecera] = _LimMax == null ? "0" : _LimMax.ToString();
                            }



                        }
                    }
                }
            }

            //}
        }
        /// <summary>
        /// Genera Excel de Generación de Despacho.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ruta"></param>
        public void GenerarFileExcelGeneracionDespacho(FormatoModel model, string ruta)
        {
            string fileTemplate = ConstantesGeneracionDespachoRDO.PlantillaGeneracionDespachoRDO;
            FileInfo template = new FileInfo(ruta + fileTemplate);
            FileInfo newFile = new FileInfo(ruta + model.Formato.Formatnombre + ".xlsx");
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + model.Formato.Formatnombre + ".xlsx");
            }
            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[ConstantesFormat.HojaFormatoExcel];
                //Escribe nombre de encabezado
                ws.Cells[2, ParamMedicionesExcell.ColEmpresa].Value = model.Empresa;
                ws.Cells[ParamMedicionesExcell.RowFormato, 2].Value = model.Formato.Formatnombre;

                //Imprimimos Codigo Empresa y Codigo Formato
                ws.Cells[1, ParamFormato.ColEmpresa].Value = model.IdEmpresa.ToString();
                ws.Cells[1, ParamFormato.ColFormato].Value = model.Formato.Formatcodi.ToString();

                ///Descripcion del Formato
                /// Nombre de la Empresa
                int row = 2;
                int column = 2;
                switch (model.Formato.Formatperiodo)
                {
                    case ParametrosFormato.PeriodoDiario:
                        ws.Cells[row + 2, column].Value = model.Handson.ListaExcelData[4][0];
                        ws.Cells[row + 2, column].Value = model.Formato.FechaInicio.ToString(ConstantesMedicion.FormatoFecha);
                        row = row + 3;
                        break;
                    case ParametrosFormato.PeriodoSemanal:
                        var ultimaFila = model.Formato.Formathorizonte * model.Formato.RowPorDia + model.Formato.Formatrows;
                        ws.Cells[row + 2, column - 1].Value = "Semana";
                        var semanaLength = model.Semana.Length;
                        ws.Cells[row + 2, column].Value = model.Semana.Substring(4, semanaLength - 4);
                        ws.Cells[row + 3, column - 1].Value = "Fecha Desde";
                        ws.Cells[row + 3, column - 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[row + 3, column - 1].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#CCFFFF"));
                        ws.Cells[row + 3, column - 1].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                        ws.Cells[row + 3, column].Value = model.Formato.FechaInicio.ToString(ConstantesMedicion.FormatoFecha);
                        ws.Cells[row + 4, column - 1].Value = "Fecha Hasta";
                        ws.Cells[row + 4, column - 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[row + 4, column - 1].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#CCFFFF"));
                        ws.Cells[row + 4, column - 1].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                        ws.Cells[row + 4, column].Value = model.Formato.FechaFin.ToString(ConstantesMedicion.FormatoFecha);
                        row = row + 4;
                        break;
                }

                ///Imprimimos cabecera de puntos de medicion
                row = ParamMedicionesExcell.RowDatos;
                int totColumnas = model.ListaHojaPto.Count;
                int columnIni = ParamMedicionesExcell.ColDatos;

                for (var i = 0; i <= model.ListaHojaPto.Count; i++)
                {
                    for (var j = 0; j < model.Formato.Formathorizonte * model.Formato.RowPorDia + model.Formato.Formatrows; j++)
                    {
                        decimal valor = 0;
                        bool canConvert = decimal.TryParse(model.Handson.ListaExcelData[j][i], out valor);
                        if (canConvert)
                            ws.Cells[row + j, i + 1].Value = valor;
                        else
                            ws.Cells[row + j, i + 1].Value = model.Handson.ListaExcelData[j][i];
                        ws.Cells[row + j, i + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        if (j < model.Formato.Formatrows && i >= model.Formato.Formatcols)
                        {
                            //ws.Cells[row + j, i + 1].Style.Font.Color.SetColor(System.Drawing.Color.White);
                            ws.Cells[row + j, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                            ws.Cells[row + j, i + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            ws.Cells[row + j, i + 1].Style.WrapText = true;
                        }
                    }
                }
                /////////////////Formato a Celdas Head ///////////////////
                using (var range = ws.Cells[row, 2, row + model.Formato.Formatrows - 1, model.ListaHojaPto.Count + 1])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#99CCFF"));
                }
                ////////////// Formato de Celdas Valores
                using (var range = ws.Cells[row + model.Formato.Formatrows, 2, row + model.Formato.Formathorizonte * model.Formato.RowPorDia + model.Formato.Formatrows - 1, model.ListaHojaPto.Count + 1])
                {
                    range.Style.Numberformat.Format = @"0.000";
                }

                /////////////////////// Celdas Merge /////////////////////

                foreach (var reg in model.Handson.ListaMerge)
                {
                    int fili = row + reg.row;
                    int filf = row + reg.row + reg.rowspan - 1;
                    int coli = reg.col + 1;
                    int colf = reg.col + reg.colspan - 1 + 1;
                    ws.Cells[fili, coli, filf, colf].Merge = true;
                }

                xlPackage.Save();
            }

        }

        /// <summary>
        /// Permite grabar los datos cargados
        /// </summary>
        /// <param name="entitys"></param>
        public void GrabarValoresCargados48_Intranet(List<MeMedicion48DTO> entitys, string usuario, int idEnvio, int idEmpresa, MeFormatoDTO formato)
        {
            try
            {
                foreach (MeMedicion48DTO entity in entitys)
                {
                    FactorySic.GetMeMedicion48Repository().SaveMeMedicion48Intranet(entity, idEnvio);
                }

                //formato.Formatcodi = formatcodiInicio;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        // <summary>
        /// Permite grabar los datos cargados
        /// </summary>
        /// <param name="entitys"></param>
        public void SaveMeMedicionxintervaloRDO(MeMedicionxintervaloDTO entity)
        {
            try
            {
                FactorySic.GetMeMedicionxintervaloRepository().SaveRDO(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Se obtiene los datos enviados de un formato de la BD.
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="formato"></param>
        /// <param name="idEnvio"></param>
        /// <param name="idUltimoEnvio"></param>
        /// <returns></returns>
        public List<MeMedicion24DTO> GetDataMeMedicionIntranet(int idEmpresa, MeFormatoDTO formato, int idEnvio, int idUltimoEnvio)
        {
            List<MeMedicion24DTO> listaGenerica = new List<MeMedicion24DTO>();

            List<MeMedicion24DTO> listaExtranet24 = FactorySic.GetMeMedicion24Repository().GetEnvioArchivoEjecutados(formato.Formatcodi, idEmpresa, formato.FechaInicio.AddDays(-1), formato.FechaInicio, "0");
            List<MeMedicion24DTO> listaIntranet24 = FactorySic.GetMeMedicion24Repository().GetEnvioMeMedicion24Intranet(formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaInicio);

            if (listaIntranet24.Count > 0)
            {
                foreach (var reg in listaIntranet24)
                {
                    var findExt = listaExtranet24.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Lectcodi == reg.Lectcodi && x.Medifecha == reg.Medifecha && x.Emprcodi == reg.Emprcodi);
                    if (findExt == null)
                        findExt = listaExtranet24.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Lectcodi == reg.Lectcodi && x.Medifecha == reg.Medifecha.AddDays(-1) && x.Emprcodi == reg.Emprcodi);

                    if (findExt == null)
                    {
                        findExt = new MeMedicion24DTO();
                        findExt.Ptomedicodi = reg.Ptomedicodi;
                        findExt.Lectcodi = reg.Lectcodi;
                        findExt.Emprcodi = reg.Emprcodi;
                        findExt.Tipoinfocodi = reg.Tipoinfocodi;
                        findExt.Medifecha = formato.FechaProceso;

                        listaExtranet24.Add(findExt);
                    }

                    var findInt = listaIntranet24.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Lectcodi == reg.Lectcodi && x.Medifecha == reg.Medifecha && x.Emprcodi == reg.Emprcodi);
                    if (findExt != null)
                    {
                        for (var x = 0; x < 24; x++)
                        {
                            var valInt = findInt.GetType().GetProperty("H" + (x + 1)).GetValue(findInt, null);
                            var valExt = findExt.GetType().GetProperty("H" + (x + 1)).GetValue(findExt, null);

                            if (valInt != null && Convert.ToDecimal(valExt) != Convert.ToDecimal(valInt) && valInt.ToString() != "")
                            {
                                findExt.GetType().GetProperty("H" + (x + 1).ToString()).SetValue(findExt, Convert.ToDecimal(valInt));
                            }
                        }
                    }
                }
            }
            foreach (var reg in listaExtranet24)
            {
                listaGenerica.Add(reg);
            }

            return listaGenerica;
        }
        /// <summary>
        /// Se obtiene los datos enviados de un formato de la BD.
        /// </summary>
        /// <param name="idFormato"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="horario"></param>
        /// <returns></returns>
        public List<MeMedicionxintervaloDTO> GetEnvioMedicionXIntervaloRDO(int idFormato, int idEmpresa, DateTime fechaInicio, DateTime fechaFin, string horario)
        {
            return FactorySic.GetMeMedicionxintervaloRepository().GetEnvioArchivoRDO(idFormato, idEmpresa, fechaInicio, fechaFin, horario);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla MeEnvio por horario
        /// </summary>
        public List<MeEnvioDTO> GetByCriteriaMeEnviosxHorario(int idEmpresa, int idFormato, DateTime fecha, int horario)
        {
            return FactorySic.GetMeEnvioRepository().GetByCriteriaxHorario(idEmpresa, idFormato, fecha, horario);
        }

        /// <summary>
        /// Se obtiene los datos enviados de un formato de la BD de me_medicon48.
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="formato"></param>
        /// <param name="idEnvio"></param>
        /// <param name="idUltimoEnvio"></param>
        /// <param name="horario"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> GetDataFormato48RDO(int idEmpresa, MeFormatoDTO formato, int idEnvio, int idUltimoEnvio, string horario)
        {
            List<MeMedicion48DTO> lista48 = new List<MeMedicion48DTO>();

            lista48 = FactorySic.GetMeMedicion48Repository().GetEnvioArchivoEjecutados(formato.Formatcodi, idEmpresa.ToString(), formato.FechaInicio, formato.FechaFin,formato.Lectcodi, horario);

            if (idEnvio != 0)
            {
                List<MeCambioenvioDTO> lista = new List<MeCambioenvioDTO>();
                if (ConstantesIEOD.IdFormatoDemandaDiaria == formato.Formatcodi)
                {
                    List<MeCambioenvioDTO> listaTmp = this.GetAllCambioEnvio(formato.Formatcodi, formato.FechaProceso.Date.AddDays(-1), formato.FechaProceso.Date.AddDays(1), idEnvio, idEmpresa);

                    foreach (var hoja in formato.ListaHoja)
                    {
                        if (ConstantesIEOD.LectCodiDemandaDiariaProgramado == hoja.Lectcodi)
                        {
                            DateTime fecha = formato.FechaProceso.Date.AddDays(1);
                            lista.AddRange(listaTmp.Where(x => x.Cambenvfecha == fecha && x.Hojacodi == hoja.Hojacodi).ToList());
                        }
                        else
                        {
                            DateTime fecha = formato.FechaProceso.Date.AddDays(-1);
                            lista.AddRange(listaTmp.Where(x => x.Cambenvfecha == fecha && x.Hojacodi == hoja.Hojacodi).ToList());
                        }
                    }
                }
                else
                {
                    lista = this.GetAllCambioEnvio(formato.Formatcodi, formato.FechaInicio, formato.FechaFin, idEnvio, idEmpresa);
                    if (formato.IdFormatoNuevo > 0 && formato.Formatcodi != formato.IdFormatoNuevo)
                    {
                        lista = this.GetAllCambioEnvio(formato.IdFormatoNuevo, formato.FechaInicio, formato.FechaFin, idEnvio, idEmpresa);
                    }
                }

                if (lista.Count > 0)
                {
                    foreach (var reg in lista)
                    {
                        MeMedicion48DTO find = !formato.FlagUtilizaHoja
                            ? lista48.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Tipoinfocodi == reg.Tipoinfocodi && x.Medifecha == reg.Cambenvfecha)
                            : lista48.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Tipoinfocodi == reg.Tipoinfocodi && x.Medifecha == reg.Cambenvfecha && x.Hojacodi == reg.Hojacodi);
                        if (find != null)
                        {
                            var fila = reg.Cambenvdatos.Split(',');
                            for (var i = 0; i < 48; i++)
                            {
                                decimal dato;
                                decimal? numero = null;
                                if (decimal.TryParse(fila[i], out dato))
                                    numero = dato;
                                find.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(find, numero);
                            }
                        }
                    }
                }
            }

            return lista48;
        }
        /// <summary>
        /// Configuración de Plazo de Envio de los Formatos
        /// </summary>
        /// <param name="formato"></param>
        public static void GetSizeFormatoRDO(MeFormatoDTO formato)
        {
            switch (formato.Formatperiodo)
            {
                case ParametrosFormato.PeriodoDiario:
                    formato.RowPorDia = ParametrosFormato.ResolucionDia / (int)formato.Formatresolucion;
                    if (formato.Lecttipo == ParametrosFormato.Programado)
                    {
                        formato.FechaInicio = formato.FechaProceso.AddDays(1);
                        formato.FechaFin = formato.FechaProceso.AddDays(formato.Formathorizonte);
                    }
                    else
                    {
                        //Ejecutado o Informacion en Tiempo Real
                        formato.FechaInicio = formato.FechaProceso;
                        formato.FechaFin = formato.FechaProceso.AddDays(formato.Formathorizonte - 1);
                    }
                    break;
                case ParametrosFormato.PeriodoSemanal:
                    formato.FechaInicio = formato.FechaProceso;
                    formato.FechaFin = formato.FechaProceso.AddDays(formato.Formathorizonte - 1);

                    switch (formato.Formatresolucion)
                    {
                        case ParametrosFormato.ResolucionCuartoHora:
                        case ParametrosFormato.ResolucionMediaHora:
                        case ParametrosFormato.ResolucionHora:
                            formato.RowPorDia = ParametrosFormato.ResolucionDia / (int)formato.Formatresolucion;
                            break;
                        case ParametrosFormato.ResolucionDia:
                            formato.RowPorDia = 1;
                            break;
                        case ParametrosFormato.ResolucionSemana:
                            formato.RowPorDia = 1;
                            break;
                    }
                    break;
                case ParametrosFormato.PeriodoMensualSemana: //Semanal Mediano Plazo
                    formato.RowPorDia = 1;

                    if (formato.Lecttipo == ParametrosFormato.Ejecutado) //Ejecutado
                    {
                        //fecha inicio es primera semana del año
                        //fecha fin es ultima semana antes del mes seleccionado
                        //si se selecciona enero se toma todo el año anterior
                        if (formato.FechaProceso.Month == 1)
                        {
                            formato.FechaInicio = EPDate.f_fechainiciosemana(formato.FechaProceso.Year - 1, 1);
                            formato.FechaFin = EPDate.f_fechainiciosemana(formato.FechaProceso.Year - 1, EPDate.TotalSemanasEnAnho(formato.FechaProceso.Year - 1, 6));
                            formato.Formathorizonte = EPDate.TotalSemanasEnAnho(formato.FechaProceso.Year - 1, 6);
                        }
                        else
                        {
                            formato.FechaInicio = EPDate.f_fechainiciosemana(formato.FechaProceso.Year, 1);
                            formato.FechaFin = EPDate.f_fechainiciosemana(formato.FechaProceso.AddDays(-4));
                            formato.Formathorizonte = EPDate.f_numerosemana(formato.FechaFin);
                        }
                    }
                    else //Programado
                    {
                        //fecha inicio es la primera semana del mes seleccionado
                        //fecha fin es la ultima semana del año
                        formato.FechaInicio = EPDate.f_fechainiciosemana(formato.FechaProceso.AddDays(3));//EPDate.f_fechainiciosemana(formato.FechaProceso.AddDays(7));
                        formato.FechaFin = formato.FechaInicio.AddDays(56 * 7);//EPDate.f_fechainiciosemana(formato.FechaProceso.AddDays(56 * 7));
                        formato.Formathorizonte = 56; //EPDate.f_numerosemana(formato.FechaFin) +
                    }
                    break;
                case ParametrosFormato.PeriodoMensual:
                    switch (formato.Formatresolucion)
                    {
                        case ParametrosFormato.ResolucionCuartoHora:
                        case ParametrosFormato.ResolucionMediaHora:
                        case ParametrosFormato.ResolucionHora:
                            formato.FechaInicio = formato.FechaProceso;
                            formato.FechaFin = formato.FechaProceso.AddMonths(1).AddDays(-1);
                            formato.Formathorizonte = ((TimeSpan)(formato.FechaFin - formato.FechaInicio)).Days + 1;
                            formato.RowPorDia = ParametrosFormato.ResolucionDia / (int)formato.Formatresolucion;
                            break;
                        case ParametrosFormato.ResolucionDia:
                            formato.FechaInicio = formato.FechaProceso;
                            formato.FechaFin = formato.FechaProceso.AddMonths(1).AddDays(-1);
                            formato.Formathorizonte = ((TimeSpan)(formato.FechaFin - formato.FechaInicio)).Days;
                            formato.RowPorDia = 1;
                            break;
                        case ParametrosFormato.ResolucionMes:
                            formato.RowPorDia = 1;
                            if (formato.Lecttipo == ParametrosLectura.Ejecutado)
                            {
                                if (formato.FechaProceso.Month == 1)
                                {
                                    formato.Formathorizonte = 12;
                                    formato.FechaInicio = new DateTime(formato.FechaProceso.Year - 1, 1, 1);
                                }
                                else
                                {
                                    formato.Formathorizonte = formato.FechaProceso.Month - 1;
                                    formato.FechaInicio = new DateTime(formato.FechaProceso.Year, 1, 1);
                                }

                                formato.FechaFin = formato.FechaProceso;
                            }
                            else // Programado
                            {
                                if (formato.Formathorizonte == 90)
                                {
                                    // Carga de Demanda Coincidente GMME-LHBN
                                    formato.FechaInicio = formato.FechaProceso;
                                    formato.FechaFin = formato.FechaProceso.AddMonths(formato.Formathorizonte);
                                    formato.Formathorizonte = 3;
                                }
                                else
                                {

                                    formato.FechaInicio = formato.FechaProceso;
                                    formato.FechaFin = formato.FechaProceso.AddMonths(12);
                                    formato.Formathorizonte = 12;
                                }
                            }
                            break;
                    }

                    break;
            }

            //formato.FechaPlazoIni = formato.FechaProceso.AddMonths(formato.Formatmesplazo).AddDays(formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
            formato.FechaPlazoIni = formato.FechaProceso.AddMonths(formato.Formatmesplazo).AddDays(formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
            //formato.FechaPlazo = formato.FechaProceso.AddMonths(formato.Formatmesfinplazo).AddDays(formato.Formatdiafinplazo).AddMinutes(formato.Formatminfinplazo);
            formato.FechaPlazo = DateTime.Now.AddHours(1);
            formato.FechaPlazoFuera = formato.FechaProceso.AddMonths(formato.Formatmesfinfueraplazo).AddDays(formato.Formatdiafinfueraplazo).AddMinutes(formato.Formatminfinfueraplazo);
        }
        #endregion

        #region Addin Carga de datos RER

        /// <summary>
        /// Obtener datos de medicion48 para servicio web
        /// </summary>
        /// <param name="fecIni"></param>
        /// <param name="fecFin"></param>
        /// <param name="fuente"></param>
        /// <param name="ptomedicodi"></param>
        /// <returns></returns>
        public List<Medicion48> GetListaObtenerMedicion48RERWS(DateTime fecIni, DateTime fecFin, int fuente, string ptomedicodi)
        {
            if (string.IsNullOrEmpty(ptomedicodi) || ptomedicodi == "-1") return new List<Medicion48>();

            //información de extranet
            int lectcodi = fuente == 1 ? 61 : 93;
            List<MeMedicion48DTO> listaM48ExtranetBD = FactorySic.GetMeMedicion48Repository().GetByCriteria(fecIni, fecFin, lectcodi.ToString(), 1, "-1");

            //obtener los equipos
            List<EqEquipoDTO> lEquipo = FactorySic.GetEqEquipoRepository().List().Where(x => x.Equiestado != "X").ToList();

            //obtener los puntos de medición
            List<MePtomedicionDTO> listaPtoAddin = FactorySic.GetMePtomedicionRepository().List(ptomedicodi, "-1");

            List<MePtomedicionDTO> listaPtoExtranet = new List<MePtomedicionDTO>();
            string ptomedicodisExtranet = string.Join(",", listaM48ExtranetBD.Select(x => x.Ptomedicodi).Distinct().ToList());
            if (!string.IsNullOrEmpty(ptomedicodisExtranet)) listaPtoExtranet = FactorySic.GetMePtomedicionRepository().List(ptomedicodisExtranet, "-1").Where(x => x.Tipoptomedicodi != 50).ToList();

            //famcodi centrales
            List<int> lfamcodiCentral = new List<int>() { ConstantesHorasOperacion.IdTipoEolica, ConstantesHorasOperacion.IdTipoSolar, ConstantesHorasOperacion.IdTipoHidraulica, ConstantesHorasOperacion.IdTipoTermica };

            //por cada pto del addin obtener los datos de extranet
            List<Medicion48> lfinal = new List<Medicion48>();
            foreach (var regPto in listaPtoAddin)
            {
                Medicion48 m48Final = new Medicion48();
                m48Final.Ptomedicodi = regPto.Ptomedicodi;
                m48Final.Tipoinfocodi = 1;
                m48Final.Lectcodi = regPto.Lectcodi;
                m48Final.MedifechaStr = regPto.Medifecha.ToString(ConstantesAppServicio.FormatoFecha);

                bool esCentral = lfamcodiCentral.Contains(regPto.Famcodi);

                var listaPtoX = new List<MePtomedicionDTO>();
                var listaEquicodiXDespacho = lEquipo.Where(x => x.Grupocodi == regPto.Grupocodi || x.Equicodi == regPto.Equicodi).Select(x => x.Equicodi).ToList();
                listaPtoX = listaPtoExtranet.Where(x => listaEquicodiXDespacho.Contains(x.Equicodi ?? 0)).ToList();

                var lptomedicodi = listaPtoX.Select(x => x.Ptomedicodi).ToList();
                var listaM48xPto = listaM48ExtranetBD.Where(x => lptomedicodi.Contains(x.Ptomedicodi));

                for (int i = 1; i <= 48; i++)
                {
                    decimal valor = 0;
                    foreach (var m48 in listaM48xPto)
                    {
                        valor += ((decimal?)m48.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(m48, null)).GetValueOrDefault(0);
                    }

                    m48Final.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).SetValue(m48Final, valor);
                }

                lfinal.Add(m48Final);
            }

            return lfinal;
        }

        /// <summary>
        /// Carga de datos de RER Reprograma
        /// </summary>
        /// <param name="fechaReprograma"></param>
        /// <param name="lista48WS"></param>
        /// <param name="fuente"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public int CargarDespachoRERAddin(DateTime fechaReprograma, List<MeMedicion48DTO> lista48WS, int fuente, string usuario)
        {
            int idFormato = fuente == 1 ? 112 : 113;

            MeFormatoDTO formato = GetByIdMeFormato(idFormato);
            formato.FechaProceso = fechaReprograma;
            GetSizeFormato2(formato);

            /////////////////////////////////////////////////////////////////////////////////////////
            List<MeMedicion48DTO> lista48 = Listar48FromAddin(idFormato, formato.Lectcodi, fechaReprograma, lista48WS);
            DateTime fechaActualizacion = DateTime.Now;

            /////////////////////////////////////////////////////////////////////////////////////////
            MeConfigformatenvioDTO config = new MeConfigformatenvioDTO();
            config.Formatcodi = idFormato;
            config.Emprcodi = -1;
            config.FechaInicio = formato.FechaProceso;
            int idConfig = GrabarConfigFormatEnvio(config);

            Boolean enPlazo = ValidarPlazo(formato);
            MeEnvioDTO envio = new MeEnvioDTO();
            envio.Archcodi = 0;
            envio.Emprcodi = -1;
            envio.Enviofecha = fechaActualizacion;
            envio.Enviofechaperiodo = formato.FechaProceso;
            envio.Enviofechaini = formato.FechaProceso;
            envio.Enviofechafin = formato.FechaProceso;
            envio.Envioplazo = (enPlazo) ? "P" : "F";
            envio.Estenvcodi = ParametrosEnvio.EnvioEnviado;
            envio.Lastdate = fechaActualizacion;
            envio.Lastuser = usuario;
            envio.Userlogin = usuario;
            envio.Formatcodi = idFormato;
            envio.Fdatcodi = 0;
            envio.Cfgenvcodi = idConfig;
            envio.Estenvcodi = 3;
            int idEnvio = SaveMeEnvio(envio);

            GrabarValoresCargados48(lista48, usuario, idEnvio, -1, formato);

            return 1;
        }

        private List<MeMedicion48DTO> Listar48FromAddin(int formatcodi, int lectcodi, DateTime fechaProceso, List<MeMedicion48DTO> lista48Input)
        {
            var lista48Final = new List<MeMedicion48DTO>();

            //Formato que se usa en Yupana
            var listaPtoXFormato = this.GetByCriteriaMeHojaptomeds(-1, 112, fechaProceso, fechaProceso);

            var listaEnvios = GetByCriteriaMeEnvios(-1, formatcodi, fechaProceso);

            List<MeMedicion48DTO> lista48BD = new List<MeMedicion48DTO>();
            if (!listaEnvios.Any())
            {
                lista48BD = ObtenerDatosRer(lectcodi, fechaProceso, fechaProceso, -1);
            }
            else
            {
                //lista48BD = FactorySic.GetMeMedicion48Repository().GetEnvioArchivo2(formato.Formatcodi, "-1", formato.FechaInicio, formato.FechaFin);
                lista48BD = FactorySic.GetMeMedicion48Repository().GetByCriteria(fechaProceso, fechaProceso, lectcodi.ToString(), 1, "-1");
            }

            //puntos de medición
            List<int> lptomedicodiInput = lista48Input.Select(x => x.Ptomedicodi).Distinct().ToList();
            List<MePtomedicionDTO> listaPtoAddin = new List<MePtomedicionDTO>();
            if (lptomedicodiInput.Any()) listaPtoAddin = FactorySic.GetMePtomedicionRepository().List(string.Join(",", lptomedicodiInput), "-1");

            List<int> lptomedicodiFmt = listaPtoXFormato.Select(x => x.Ptomedicodi).Distinct().ToList();
            List<MePtomedicionDTO> listaPtoFmt = new List<MePtomedicionDTO>();
            if (lptomedicodiFmt.Any()) listaPtoFmt = FactorySic.GetMePtomedicionRepository().List(string.Join(",", lptomedicodiFmt), "-1");

            //obtener los equipos
            List<EqEquipoDTO> lEquipo = FactorySic.GetEqEquipoRepository().List().Where(x => x.Equiestado != "X").ToList();

            //iterar por grupo de despacho
            List<int> lPtomedicodiUsado = new List<int>();
            foreach (var regPto in listaPtoAddin)
            {
                var listaEquicodiXDespacho = lEquipo.Where(x => x.Grupocodi == regPto.Grupocodi || x.Equicodi == regPto.Equicodi).Select(x => x.Equicodi).ToList();
                var listaPtoX = listaPtoFmt.Where(x => listaEquicodiXDespacho.Contains(x.Equicodi ?? 0)).ToList();
                lPtomedicodiUsado.AddRange(listaPtoX.Select(x => x.Ptomedicodi).ToList());

                var lista48xCentral = lista48Input.Where(x => x.Ptomedicodi == regPto.Ptomedicodi).ToList();

                if (listaPtoX.Any())
                {
                    if (listaPtoX.Count == 1)
                    {
                        //una central en formato y una central en addin
                        lista48xCentral.ForEach(x => x.Ptomedicodi = listaPtoX.First().Ptomedicodi);
                        lista48Final.AddRange(lista48xCentral);
                    }
                    else
                    {
                        int totalXAddin = listaPtoX.Count;

                        foreach (var ptoFmt in listaPtoX)
                        {
                            var m48Fmt = new MeMedicion48DTO();
                            m48Fmt.Ptomedicodi = ptoFmt.Ptomedicodi;
                            for (int i = 1; i <= 48; i++)
                            {
                                decimal valorH = GetValueHFromLista(i, lista48xCentral) / totalXAddin;
                                m48Fmt.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).SetValue(m48Fmt, valorH);
                            }

                            lista48Final.Add(m48Fmt);
                        }
                    }
                }
            }

            var lista48NoAddin = lista48BD.Where(x => !lPtomedicodiUsado.Contains(x.Ptomedicodi)).ToList();
            lista48Final.AddRange(lista48NoAddin);

            //Obtener Lectocodi
            lista48Final.ForEach(x => x.Medifecha = fechaProceso);
            lista48Final.ForEach(x => x.Lectcodi = lectcodi);
            lista48Final.ForEach(x => x.Tipoinfocodi = 1);

            return lista48Final;
        }

        private decimal GetValueHFromLista(int h, List<MeMedicion48DTO> lista48xCentral)
        {
            decimal valorTotal = 0;

            foreach (var m48 in lista48xCentral)
            {
                decimal valor = ((decimal?)m48.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m48, null)).GetValueOrDefault(0);
                valorTotal += valor;
            }

            return valorTotal;
        }

        #endregion

        #region CmgCP-PR07

        /// <summary>
        /// Información actualizada de RDO con la data de Hidrología Extranet
        /// </summary>
        /// <param name="fechaHora"></param>
        /// <param name="listaDataRdo"></param>
        /// <param name="listaHojaPto"></param>
        /// <returns></returns>
        public List<MeMedicion24DTO> ListarRdoCaudalActualizadoXExtranet(DateTime fechaHora, List<MeMedicion24DTO> listaDataRdo, List<MeHojaptomedDTO> listaHojaPto, out List<int> listaPosCheck)
        {
            //posicion en el excel web
            listaPosCheck = new List<int>();

            //1. Obtener datos de caudal TR
            string idsEmpresa = ConstantesAppServicio.ParametroDefecto;
            string idsCuenca = ConstantesAppServicio.ParametroDefecto;
            string idsFamilia = ConstantesAppServicio.ParametroDefecto;
            string idsTptoMedicion = "1,2,3,4,5,10,14,16,24,17,18,19,8,7,9,11,12,13,84";
            List<MeMedicion24DTO> lista24Hidro = FactorySic.GetMeMedicion24Repository().GetHidrologia(66, 16, idsEmpresa, idsCuenca, idsFamilia, fechaHora.Date, fechaHora.Date, idsTptoMedicion);
            List<MeMedicion24DTO> lista24HidroCaudal = lista24Hidro.Where(x => x.Tipoinfocodi == ConstantesAppServicio.TipoinfocodiM3s).ToList();

            //2. Actualizar
            int correlativo = 0;
            foreach (var regHpto in listaHojaPto)
            {
                MeMedicion24DTO regRdo = listaDataRdo.Find(x => x.Ptomedicodi == regHpto.Ptomedicodi);
                if (regRdo == null)
                {
                    regRdo = new MeMedicion24DTO();
                    regRdo.Ptomedicodi = regHpto.Ptomedicodi;
                    regRdo.Medifecha = fechaHora.Date;
                    regRdo.Tipoinfocodi = regHpto.Tipoinfocodi;

                    listaDataRdo.Add(regRdo);
                }

                //obtener data hidrologia (00:00 a 23:00)
                var regHoy = lista24HidroCaudal.Find(x => x.Ptomedicodi == regHpto.Ptomedicodi);
                if (regHoy != null)
                {
                    listaPosCheck.Add(correlativo);

                    for (int h = 1; h <= 24; h++)
                    {
                        decimal? valorH = (decimal?)regHoy.GetType().GetProperty("H" + (h)).GetValue(regHoy, null);
                        regRdo.GetType().GetProperty("H" + h).SetValue(regRdo, valorH);
                    }
                }

                correlativo++;
            }

            listaPosCheck = listaPosCheck.Distinct().ToList();

            return listaDataRdo;
        }

        /// <summary>
        /// Reporte de cumplimiento de formatos de hidrología de Tr
        /// </summary>
        /// <param name="fechaPeriodo"></param>
        /// <param name="idFormato"></param>
        /// <returns></returns>
        public List<RdoCumplimiento> ListarRptCumplimientoExtranetHidrologiaTr(DateTime fechaPeriodo, int idFormato)
        {
            List<RdoCumplimiento> listaRptCumpl = new List<RdoCumplimiento>();

            MeFormatoDTO listaFmt = FactorySic.GetMeFormatoRepository().GetById(idFormato);

            //obtener la configuracion activa (al dia de hoy)
            var listaHptoXFmt = FactorySic.GetMeHojaptomedRepository().ListByFormatcodi(idFormato.ToString()).Where(x => x.Hojaptoactivo == 1 && x.Hptoindcheck == "S").ToList();

            //obtener los envios del periodo seleccionado
            var listaEnvioXFmt = FactorySic.GetMeEnvioRepository().ObtenerReporteCumplimientoXBloqueHorario(ConstantesAppServicio.ParametroDefecto, idFormato.ToString(), fechaPeriodo, fechaPeriodo)
                                        .Where(x => x.Enviobloquehora > 0).ToList();

            //obtener relacion empresa-formato
            List<MeFormatoEmpresaDTO> listaRelEmpXFmt = new List<MeFormatoEmpresaDTO>();
            listaRelEmpXFmt.AddRange(listaHptoXFmt.Select(x => new MeFormatoEmpresaDTO() { Formatcodi = x.Formatcodi, Emprcodi = x.Emprcodi, Emprnomb = x.Emprnomb }).ToList());
            listaRelEmpXFmt.AddRange(listaEnvioXFmt.Select(x => new MeFormatoEmpresaDTO() { Formatcodi = x.Formatcodi.Value, Emprcodi = x.Emprcodi.Value, Emprnomb = x.Emprnomb }).ToList());
            listaRelEmpXFmt = listaRelEmpXFmt.GroupBy(x => new { x.Formatcodi, x.Emprcodi }).Select(x => x.First()).ToList();

            //formateo de resultado
            foreach (var regConfig in listaRelEmpXFmt)
            {
                RdoCumplimiento rpt = new RdoCumplimiento();
                rpt.NombreEmpresa = regConfig.Emprnomb;
                rpt.Emprcodi = regConfig.Emprcodi;
                rpt.Formatcodi = regConfig.Formatcodi;

                //Reporte de envio
                List<MeEnvioDTO> listaEnvExtranet = listaEnvioXFmt.Where(x => x.Emprcodi == regConfig.Emprcodi).ToList();

                for (int bloque = 3; bloque <= 24; bloque += 3)
                {
                    string plazoDesc = GetTextoCumplimientoXBloque(bloque, listaEnvExtranet);

                    rpt.GetType().GetProperty("Hora" + bloque).SetValue(rpt, plazoDesc);
                }

                listaRptCumpl.Add(rpt);
            }

            return listaRptCumpl;
        }

        private string GetTextoCumplimientoXBloque(int bloque, List<MeEnvioDTO> listaEnvExtranet)
        {
            var reg = listaEnvExtranet.Find(x => x.Enviobloquehora == bloque);
            if (reg != null)
            {
                if (reg.Envioplazo == "P") return "Envió en plazo";
                else return "Envió fuera de plazo";
            }

            return "No envió";
        }

        /// <summary>
        /// Lista de cumplimiento de envios de hidrologia,
        /// </summary>
        /// <param name="sEmpresas"></param>
        /// <param name="fInicio"></param>
        /// <param name="fFin"></param>
        /// <param name="idFormato"></param>
        /// <param name="nombreFormato"></param>
        /// <param name="idPeriodo"></param>
        /// <param name="rutaArchivo"></param>
        /// <param name="rutaLogo"></param>
        public void GeneraExcelCumplimientoExtranetHidrologiaTr(List<RdoCumplimiento> listadoRdo, string nombreFormato, string rutaArchivo)
        {
            FileInfo newFile = new FileInfo(rutaArchivo);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaArchivo);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                ws = xlPackage.Workbook.Worksheets.Add("Cumplimiento");
                ws = xlPackage.Workbook.Worksheets["Cumplimiento"];
                var fontTabla = ws.Cells[3, 2].Style.Font;
                fontTabla.Size = 14;
                fontTabla.Bold = true;
                ws.Cells[3, 2].Value = "REPORTE CUMPLIMIENTO:";
                ws.Cells[3, 3].Value = nombreFormato;

                int fila = 6;
                int col = 2;
                int col3 = col + 1;
                int col6 = col3 + 1;
                int col9 = col6 + 1;
                int col12 = col9 + 1;
                int col15 = col12 + 1;
                int col18 = col15 + 1;
                int col21 = col18 + 1;
                int col24 = col21 + 1;

                ws.Cells[5, col].Value = "Empresa/Bloque Horario";
                ws.Cells[5, col3].Value = "03:00";
                ws.Cells[5, col6].Value = "06:00";
                ws.Cells[5, col9].Value = "09:00";
                ws.Cells[5, col12].Value = "12:00";
                ws.Cells[5, col15].Value = "15:00";
                ws.Cells[5, col18].Value = "18:00";
                ws.Cells[5, col21].Value = "21:00";
                ws.Cells[5, col24].Value = "24:00";
                int cont = 8;

                string colorFondo = string.Empty;
                foreach (var obj in listadoRdo)
                {
                    ws.Cells[fila, col].Value = obj.NombreEmpresa;
                    ws.Cells[fila, col3].Value = obj.Hora3;
                    ws.Cells[fila, col6].Value = obj.Hora6;
                    ws.Cells[fila, col9].Value = obj.Hora9;
                    ws.Cells[fila, col12].Value = obj.Hora12;
                    ws.Cells[fila, col15].Value = obj.Hora15;
                    ws.Cells[fila, col18].Value = obj.Hora18;
                    ws.Cells[fila, col21].Value = obj.Hora21;
                    ws.Cells[fila, col24].Value = obj.Hora24;

                    UtilExcel.SetFormatoCelda(ws, fila, col3, fila, col24, "Centro", "Centro", "#000000", "#FFFFFF", "Calibri", 11, false);
                    UtilExcel.AllBorders(ws.Cells[fila, col3, fila, col24], ExcelBorderStyle.Thin, Color.Black);

                    fila++;
                }

                // borde de region de datos
                var borderReg = ws.Cells[5, 2, fila - 1, cont + 2].Style.Border;
                borderReg.Bottom.Style = borderReg.Top.Style = borderReg.Left.Style = borderReg.Right.Style = ExcelBorderStyle.Thin;
                //fila 5
                using (ExcelRange r = ws.Cells[5, 2, 5, cont + 2])
                {
                    r.Style.Font.Color.SetColor(Color.White);
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(70, 130, 210));
                }
                //datos
                using (ExcelRange r = ws.Cells[6, 2, fila - 1, 2])
                {
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(173, 216, 230));
                }

                ws.Column(col).AutoFit();
                ws.Column(col3).Width = 20;
                ws.Column(col6).Width = 20;
                ws.Column(col9).Width = 20;
                ws.Column(col12).Width = 20;
                ws.Column(col15).Width = 20;
                ws.Column(col18).Width = 20;
                ws.Column(col21).Width = 20;
                ws.Column(col24).Width = 20;
                xlPackage.Save();
            }
        }


        /// <summary>
        /// Lista de cumplimiento de envios de hidrologia,
        /// </summary>
        /// <param name="sEmpresas"></param>
        /// <param name="fInicio"></param>
        /// <param name="fFin"></param>
        /// <param name="idFormato"></param>
        /// <param name="nombreFormato"></param>
        /// <param name="idPeriodo"></param>
        /// <param name="rutaArchivo"></param>
        /// <param name="rutaLogo"></param>
        public void GeneraExcelCumplimientoExtranetFallas(List<MeEnvioDTO> listadoRdo, string nombreFormato, string rutaArchivo)
        {
            FileInfo newFile = new FileInfo(rutaArchivo);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaArchivo);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                ws = xlPackage.Workbook.Worksheets.Add("Cumplimiento");
                ws = xlPackage.Workbook.Worksheets["Cumplimiento"];
                var fontTabla = ws.Cells[3, 2].Style.Font;
                fontTabla.Size = 14;
                fontTabla.Bold = true;
                ws.Cells[3, 2].Value = "REPORTE CUMPLIMIENTO:";
                ws.Cells[3, 3].Value = nombreFormato;

                int fila = 7;
                int col = 2;
                //int col2 = 2;
                //int col3 = 2;
                int colInicial = 2;
                //int col3 = col + 1;
                //int col6 = col3 + 1;
                //int col9 = col6 + 1;
                //int col12 = col9 + 1;
                //int col15 = col12 + 1;
                //int col18 = col15 + 1;
                //int col21 = col18 + 1;
                //int col24 = col21 + 1;



                List<MeEnvioDTO> listaUltimosEnvios = new List<MeEnvioDTO>();
                var codigosEventos = listadoRdo.Select(y => new { y.Evencodi, y.Emprcodi }).Distinct().ToList();

                foreach (var item in codigosEventos)
                {
                    MeEnvioDTO envio = new MeEnvioDTO();
                    MeEnvioDTO ultimoEnvio = (listadoRdo.Where(x => x.Evencodi == item.Evencodi && x.Emprcodi == item.Emprcodi).OrderBy(c => c.Enviocodi).FirstOrDefault());


                    listaUltimosEnvios.Add(ultimoEnvio);
                }



                ws.Cells[5, col].Value = "EMPRESA";
                var listaHeader = listaUltimosEnvios.Select(y => new { y.Evenini, y.Evenasunto }).Distinct().ToList().OrderBy(c => c.Evenini);
                int cont = 0;

                foreach (var item in listaHeader)
                {
                    ws.Cells[5, col +1 ].Value = item.Evenini.Value.ToString("dd/MM/yyyy HH:mm:ss") ;
                    col++;
                    cont++;
                
                    ws.Column(col).Width = 20;
                }
                col = colInicial;
                ws.Cells[6, col].Value = "DESCRIPCION";
                foreach (var item in listaHeader)
                {
                    ws.Cells[6, col + 1].Value = item.Evenasunto;
                    col++;
                }

                var listaEmpresa = listaUltimosEnvios.Select(y => new { y.Emprcodi, y.Emprnomb }).Distinct().ToList().OrderBy(c => c.Emprnomb);

                string colorFondo = string.Empty;
                foreach (var item in listaEmpresa)
                {
                    col = colInicial;

                    ws.Cells[fila, col].Value = item.Emprnomb;
                    UtilExcel.SetFormatoCelda(ws, fila, col, fila, col, "Centro", "Left", "#000000", "#FFFFFF", "Calibri", 11, false);

                    var enviosHeader = listaUltimosEnvios.Select(y => new { y.Evenini }).Distinct().ToList().OrderBy(c => c.Evenini);
                    foreach (var itemEnvioHeader in enviosHeader)
                    {
                        var enviosEmpresa = (listaUltimosEnvios.Where(x => x.Emprcodi == item.Emprcodi).OrderBy(c => c.Evenini)).ToList();
                        string valor = "";
                        foreach (var itemEnviosEmpresa in enviosEmpresa)
                        {
                     
                            if (itemEnvioHeader.Evenini == itemEnviosEmpresa.Evenini)
                            {
                                valor = itemEnviosEmpresa.Envioplazo;


                            }
                        
                        }
                        ws.Cells[fila, col + 1].Value = valor;
                        col++;

                    }

                    UtilExcel.SetFormatoCelda(ws, fila, colInicial +1, fila, col, "Centro", "Centro", "#000000", "#FFFFFF", "Calibri", 11, false);
                    UtilExcel.AllBorders(ws.Cells[fila, colInicial, fila, col], ExcelBorderStyle.Thin, Color.Black);

                    fila++;
                }

                // borde de region de datos
                var borderReg = ws.Cells[5, 2, fila - 1, cont + 2].Style.Border;
                borderReg.Bottom.Style = borderReg.Top.Style = borderReg.Left.Style = borderReg.Right.Style = ExcelBorderStyle.Thin;
                //fila 5
                using (ExcelRange r = ws.Cells[5, 2, 5, cont + 2])
                {
                    r.Style.Font.Color.SetColor(Color.White);
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(70, 130, 210));
                }
                using (ExcelRange r = ws.Cells[6, 2, 6, cont + 2])
                {
                    r.Style.Font.Color.SetColor(Color.White);
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(70, 130, 210));

                }
                xlPackage.Save();
            }
        }
        #endregion

        #region Mejoras RDO-II
        //public List<MeEnvioDTO> GetByCriteriaMeEnviosUltimoEjecutado(int idEmpresa, int idFormato, DateTime fecha, int horario)
        //{
        //    return FactorySic.GetMeEnvioRepository().GetByCriteriaMeEnviosUltimoEjecutado(idEmpresa, idFormato, fecha, horario);
        //}
        /// <summary>
        /// Se obtiene los datos enviados de un formato de la BD de me_medicon48.
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="formato"></param>
        /// <param name="idEnvio"></param>
        /// <param name="idUltimoEnvio"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> GetDataFormato48UltimoEjecutado(int idEmpresa, MeFormatoDTO formato, int idEnvio, int idUltimoEnvio)
        {
            List<MeMedicion48DTO> lista48 = new List<MeMedicion48DTO>();
            lista48 = FactorySic.GetMeMedicion48Repository().GetEnvioArchivoUltimoEjecutado(formato.Formatcodi, idEmpresa.ToString(), formato.FechaInicio, formato.FechaFin, formato.Lectcodi, idEnvio);
                        
            return lista48;
        }
        #endregion

        #region PrimasRER.2023
        public List<MeMedicionxintervaloDTO> ListarBarrasPMPO(string fechaInicio, string fechaFin)
        {
            return FactorySic.GetMeMedicionxintervaloRepository().ListarBarrasPMPO(fechaInicio, fechaFin);
        }
        public List<MeMedicionxintervaloDTO> ListarCentralesPMPO(int emprcodi)
        {
            return FactorySic.GetMeMedicionxintervaloRepository().ListarCentralesPMPO(emprcodi);
        }
        #endregion
    }

}
