using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.RegistroIntegrantes
{
    /// <summary>
    /// Clases con métodos del módulo Generado
    /// </summary>
    public class HistoricoAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(HistoricoAppServicio));

        #region Métodos Tabla RI_HISTORICO

        /// <summary>
        /// Inserta un registro de la tabla RI_HISTORICO
        /// </summary>
        public void SaveRiHistorico(RiHistoricoDTO entity)
        {
            try
            {
                if (entity.Hisricodi == 0)
                {
                    entity.Hisrifeccreacion = DateTime.Now;
                    entity.Hisriusumodificacion = entity.Hisriusucreacion;
                    entity.Hisrifecmodificacion = DateTime.Now;
                    FactorySic.GetRiHistoricoRepository().Save(entity);
                }
                else
                {
                    entity.Hisriusumodificacion = entity.Hisriusucreacion;
                    entity.Hisrifecmodificacion = DateTime.Now;
                    FactorySic.GetRiHistoricoRepository().Update(entity);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }              

        /// <summary>
        /// Elimina un registro de la tabla RI_HISTORICO
        /// </summary>
        public void DeleteRiHistorico(int hisricodi)
        {
            try
            {
                FactorySic.GetRiHistoricoRepository().Delete(hisricodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla RI_HISTORICO
        /// </summary>
        public RiHistoricoDTO GetByIdRiHistorico(int hisricodi)
        {
            return FactorySic.GetRiHistoricoRepository().GetById(hisricodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla RI_HISTORICO
        /// </summary>
        public List<RiHistoricoDTO> ListRiHistoricos()
        {
            return FactorySic.GetRiHistoricoRepository().List();
        }
        public List<RiHistoricoDTO> ListAnios()
        {
            return FactorySic.GetRiHistoricoRepository().ListAnios();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla RiHistorico
        /// </summary>
        public List<RiHistoricoDTO> GetByCriteriaRiHistoricos(int anio, string tipo)
        {
            return FactorySic.GetRiHistoricoRepository().GetByCriteria(anio, tipo);
        }

        #endregion
    }
}
