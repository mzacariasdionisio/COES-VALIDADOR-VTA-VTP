using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Eventos.Helper;

namespace COES.Servicios.Aplicacion.EnviarCorreos
{
    /// <summary>
    /// Clases con métodos del módulo EnviarCorreos
    /// </summary>
    public class EnviarCorreosAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(EnviarCorreosAppServicio));

        #region Métodos Tabla EVE_MAILS

        /// <summary>
        /// Inserta un registro de la tabla EVE_MAILS
        /// </summary>
        public void SaveEveMails(EveMailsDTO entity)
        {
            try
            {
                FactorySic.GetEveMailsRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EVE_MAILS
        /// </summary>
        public void UpdateEveMails(EveMailsDTO entity)
        {
            try
            {
                FactorySic.GetEveMailsRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EVE_MAILS
        /// </summary>
        public void DeleteEveMails(int mailcodi)
        {
            try
            {
                FactorySic.GetEveMailsRepository().Delete(mailcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EVE_MAILS
        /// </summary>
        public EveMailsDTO GetByIdEveMails(int mailcodi)
        {
            return FactorySic.GetEveMailsRepository().GetById(mailcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EVE_MAILS
        /// </summary>
        public List<EveMailsDTO> ListEveMailss()
        {
            return FactorySic.GetEveMailsRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EveMails
        /// </summary>
        public List<EveMailsDTO> GetByCriteriaEveMailss()
        {
            return FactorySic.GetEveMailsRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite buscar las operaciones
        /// </summary>
        /// <param name="subCausacodi">Código de subcausa</param>
        /// <param name="fechaInicio">Fecha inicial</param>
        /// <param name="fechaFinal">Fecha final</param>
        /// <param name="nroPage">Número de página</param>
        /// <param name="pageSize">Tamaño de página</param>
        /// <returns></returns>
        public List<EveMailsDTO> BuscarOperaciones(int subCausacodi, DateTime fechaInicio, DateTime fechaFinal, int nroPage, int pageSize)
        {
            return FactorySic.GetEveMailsRepository().BuscarOperaciones(subCausacodi, fechaInicio, fechaFinal, nroPage, pageSize);
        }

        /// <summary>
        /// Permite buscar las operaciones
        /// </summary>
        /// <param name="subCausacodi">Código de subcausa</param>
        /// <param name="fechaInicio">Fecha inicial</param>
        /// <param name="fechaFinal">Fecha final</param>
        /// <param name="nroPage">Número de página</param>
        /// <param name="pageSize">Tamaño de página</param>
        /// <returns></returns>
        public List<EveMailsDTO> BuscarOperacionesDelTipoReProgramaPorFecha(string fecha)
        {
            return FactorySic.GetEveMailsRepository().BuscarOperacionesDelTipoReProgramaPorFecha(fecha);
        }

        /// <summary>
        /// Permite obtener el número de filas
        /// </summary>
        /// <param name="subCausacodi">Código de subcausa</param>
        /// <param name="fechaInicio">Fecha inicial</param>
        /// <param name="fechaFinal">Fecha final</param>
        /// <param name="nroPage">Número de página</param>
        /// <param name="pageSize">Tamaño de página</param>
        /// <returns></returns>
        public int ObtenerNroFilas(int subCausacodi, DateTime fechaInicio, DateTime fechaFinal, int nroPage, int pageSize)
        {
            return FactorySic.GetEveMailsRepository().ObtenerNroFilas(subCausacodi, fechaInicio, fechaFinal, nroPage, pageSize);
        }

        /// <summary>
        /// graba los datos
        /// </summary>
        public int Save(EveMailsDTO entity)
        {
            try
            {
                int id = 0;

                if (entity.Mailcodi == 0)
                {
                    //int idRegistro = (new GeneralAppServicio()).ObtenerNextIdTabla(ConstantesEvento.TablaEmails);
                    //entity.Mailcodi = idRegistro;
                    id = FactorySic.GetEveMailsRepository().Save(entity);
                }
                else
                {
                    FactorySic.GetEveMailsRepository().Update(entity);
                    id = entity.Mailcodi;
                }

                return id;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener datos para el envio de correos
        /// </summary>
        /// <param name="idTipoOperacion"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<EveMailsDTO> ExportarEnvioCorreos(int? idTipoOperacion, DateTime fechaInicio, DateTime fechaFin)
        {
            List<EveMailsDTO> list = new List<EveMailsDTO>();
            list = FactorySic.GetEveMailsRepository().ExportarEnvioCorreos(idTipoOperacion, fechaInicio, fechaFin);
            return list;
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EVE_SUBCAUSAEVENTO
        /// </summary>
        public EveSubcausaeventoDTO GetByIdSubCausaEvento(int subcausaCodi)
        {
            return FactorySic.GetEveSubcausaeventoRepository().GetById(subcausaCodi);
        }

        #endregion

    }
}
