using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;

namespace COES.Servicios.Aplicacion.InformeOsinerming
{
    public class ReporteMensualAppServicio
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ReporteMensualAppServicio));

        #region Métodos Tabla PSU_RPFHID

        /// <summary>
        /// Inserta un registro de la tabla PSU_RPFHID
        /// </summary>
        public void SavePsuRpfhid(PsuRpfhidDTO entity, string indicador)
        {
            try
            {
                if (indicador == ConstantesAppServicio.SI)
                {
                    FactorySic.GetPsuRpfhidRepository().Save(entity);
                }
                else 
                {
                    DateTime dato = entity.Rpfhidfecha;
                    decimal a = entity.Potindhidra;
                    decimal b = entity.Eneindhidra;
                    decimal c = entity.Rpfenetotal;
                    decimal d = entity.Rpfpotmedia;
                    FactorySic.GetPsuRpfhidRepository().Update(entity);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PSU_RPFHID
        /// </summary>
        public void UpdatePsuRpfhid(PsuRpfhidDTO entity)
        {
            try
            {
                FactorySic.GetPsuRpfhidRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PSU_RPFHID
        /// </summary>
        public void DeletePsuRpfhid(DateTime rpfhidfecha)
        {
            try
            {
                FactorySic.GetPsuRpfhidRepository().Delete(rpfhidfecha);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PSU_RPFHID
        /// </summary>
        public PsuRpfhidDTO GetByIdPsuRpfhid(DateTime rpfhidfecha)
        {
            return FactorySic.GetPsuRpfhidRepository().GetById(rpfhidfecha);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PSU_RPFHID
        /// </summary>
        public List<PsuRpfhidDTO> ListPsuRpfhids()
        {
            return FactorySic.GetPsuRpfhidRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PsuRpfhid
        /// </summary>
        public List<PsuRpfhidDTO> GetByCriteriaPsuRpfhids(DateTime fechaInicio, DateTime fechaFin)
        {
            return FactorySic.GetPsuRpfhidRepository().GetByCriteria(fechaInicio, fechaFin);
        }

        #endregion
    }
}
