using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;


namespace COES.Servicios.Aplicacion.RegistroIntegrantes
{
    public class TipoAppServicio : AppServicioBase
    {

        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(EmpresaAppServicio));

        #region Métodos Tabla SI_TIPO_COMPORTAMIENTO

        /// <summary>
        /// Permite listar los registros de una empresa de la tabla SI_TIPO_COMPORTAMIENTO
        /// </summary>
        public List<SiTipoComportamientoDTO> GetByEmpresaSiTipo(int emprcodi)
        {
            return FactorySic.GetSiTipoComportamientoRepository().ListByEmprcodi(emprcodi);
            
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SI_TIPO_COMPORTAMIENTO
        /// </summary>
        public SiTipoComportamientoDTO GetByIdSiTipo(int Tipocodi)
        {
            return FactorySic.GetSiTipoComportamientoRepository().GetById(Tipocodi);
        }

        /// <summary>
        /// Actualiza un registro de la tabla SI_TIPO_COMPORTAMIENTO
        /// </summary>
        public void UpdateSiTipo(SiTipoComportamientoDTO entity)
        {
            try
            {
                FactorySic.GetSiTipoComportamientoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SI_TIPO_COMPORTAMIENTO
        /// </summary>
        public void DeleteSiTipo(int Tipocodi)
        {
            try
            {
                FactorySic.GetSiTipoComportamientoRepository().Delete(Tipocodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SI_TIPO_COMPORTAMIENTO
        /// </summary>
        public int InsertSiTipo(SiTipoComportamientoDTO entity)
        {
            try
            {
                return FactorySic.GetSiTipoComportamientoRepository().Save(entity);                
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

          #endregion
    }
}
