using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;

namespace COES.Servicios.Aplicacion.Eventos
{
    /// <summary>
    /// Clases con métodos del módulo Sorteo
    /// </summary>
    public class SorteoAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SorteoAppServicio));

        #region Métodos Tabla PR_LOGSORTEO

        /*
        /// <summary>
        /// Inserta un registro de la tabla PR_LOGSORTEO
        /// </summary>
        public void SavePrLogsorteo(PrLogsorteoDTO entity)
        {
            try
            {
                FactorySic.GetPrLogsorteoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        */

        /// <summary>
        /// Actualiza un registro de la tabla PR_LOGSORTEO
        /// </summary>
        public void UpdatePrLogsorteo(PrLogsorteoDTO entity)
        {
            try
            {
                FactorySic.GetPrLogsorteoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PR_LOGSORTEO
        /// </summary>
        public void DeletePrLogsorteo(DateTime logfecha)
        {
            try
            {
                FactorySic.GetPrLogsorteoRepository().Delete(logfecha);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PR_LOGSORTEO
        /// </summary>
        public PrLogsorteoDTO GetByIdPrLogsorteo(DateTime logfecha)
        {
            return FactorySic.GetPrLogsorteoRepository().GetById(logfecha);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PR_LOGSORTEO
        /// </summary>
        public List<PrLogsorteoDTO> ListPrLogsorteos()
        {
            return FactorySic.GetPrLogsorteoRepository().List();
        }

        /// <summary>
        /// Permite obtener el conteo de registros por tipo y fecha.
        /// </summary>
        /// <param name="Tipo">Tipo de log</param>
        /// <param name="Fecha">Fecha de sorteo</param>
        /// <returns>Conteo por tipo</returns>
        public int ConteoCorreoTipo(string Tipo, DateTime Fecha)
        {
            return FactorySic.GetPrLogsorteoRepository().ConteoCorreoTipo(Tipo, Fecha);
        }


        /// <summary>
        /// Permite obtener el conteo de balota negra por fecha.
        /// </summary>        
        /// <param name="Fecha">Fecha de sorteo</param>
        /// <returns>Conteo de Balota Negra</returns>
        public int ConteoBalotaNegra(DateTime Fecha)
        {
            return FactorySic.GetPrLogsorteoRepository().ConteoBalotaNegra(Fecha);
        }

        /// <summary>
        /// Obtiene el nombre del equipo sorteado para pruebas
        /// </summary>
        /// <param name="Fecha">Fecha de sorteo</param>
        /// <returns>Nombre del equipo sorteado para pruebas</returns>
        public string EquipoPrueba(DateTime Fecha)
        {
            return FactorySic.GetPrLogsorteoRepository().EquipoPrueba(Fecha);
        }

        /// <summary>
        /// Obtiene el código del equipo sorteado para pruebas
        /// </summary>
        /// <param name="Fecha">Fecha de sorteo</param>
        /// <returns>Código del equipo sorteado para pruebas</returns>
        public int EquicodiPrueba(DateTime Fecha)
        {
            return FactorySic.GetPrLogsorteoRepository().EquicodiPrueba(Fecha);
        }

        #endregion

    }
}
