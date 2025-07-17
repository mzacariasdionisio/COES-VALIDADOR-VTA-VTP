using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;

namespace COES.Servicios.Aplicacion.Mediciones
{
    /// <summary>
    /// Clases con métodos del módulo Lectura
    /// </summary>
    public class LecturaAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(LecturaAppServicio));

        #region Métodos Tabla F_LECTURA

        /// <summary>
        /// Inserta un registro de la tabla F_LECTURA
        /// </summary>
        //public void SaveFLectura(FLecturaDTO entity)
        //{
        //    try
        //    {
        //        FactorySic.GetFLecturaRepository().Save(entity);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ConstantesAppServicio.LogError, ex);
        //        throw new Exception(ex.Message, ex);
        //    }
        //}

        /// <summary>
        /// Actualiza un registro de la tabla F_LECTURA
        /// </summary>
        public void UpdateFLectura(FLecturaDTO entity)
        {
            try
            {
                FactorySic.GetFLecturaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla F_LECTURA
        /// </summary>
        public void DeleteFLectura()
        {
            try
            {
                FactorySic.GetFLecturaRepository().Delete();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla F_LECTURA
        /// </summary>
        public FLecturaDTO GetByIdFLectura(DateTime FechaHora, Int32 GpsCodi)
        {
            return FactorySic.GetFLecturaRepository().GetById(FechaHora, GpsCodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla F_LECTURA
        /// </summary>
        public List<FLecturaDTO> ListFLecturas()
        {
            return FactorySic.GetFLecturaRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla FLectura
        /// </summary>
        public List<FLecturaDTO> GetByCriteriaFLecturas(Int32 GpsCodi, DateTime FechaInicio, DateTime FechaFin)
        {
            return FactorySic.GetFLecturaRepository().GetByCriteria(GpsCodi, FechaInicio, FechaFin);
        }

        public DataTable GetFechaDesvNumPorGpsFecha(int GpsCodi, DateTime FechaInicio, DateTime FechaFin)
        {
            return FactorySic.GetFLecturaRepository().GetFechaDesvNumPorGpsFecha(GpsCodi, FechaInicio, FechaFin);
        }

        public DataTable GetByCriteriaDatatable(Int32 GpsCodi, DateTime FechaInicio, DateTime FechaFin)
        {
            return FactorySic.GetFLecturaRepository().GetByCriteriaDatatable(GpsCodi, FechaInicio, FechaFin);
        }

        public DataTable GetByCriteriaDatatable2(Int32 GpsCodi, DateTime FechaInicio, DateTime FechaFin)
        {
            return FactorySic.GetFLecturaRepository().GetByCriteriaDatatable2(GpsCodi, FechaInicio, FechaFin);
        }

        public int ContarRegistrosRepetidos(Int32 GpsCodi, DateTime FechaInicio, DateTime FechaFin)
        {
            return FactorySic.GetFLecturaRepository().ContarRegistrosRepetidos(GpsCodi, FechaInicio, FechaFin);
        }

        #endregion

    }
}
