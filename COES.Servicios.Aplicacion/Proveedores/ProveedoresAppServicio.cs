using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;

namespace COES.Servicios.Aplicacion.Proveedores
{
    /// <summary>
    /// Clases con métodos del módulo Proveedores
    /// </summary>
    public class ProveedoresAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ProveedoresAppServicio));

        #region Métodos Tabla WB_PROVEEDOR

        /// <summary>
        /// Inserta un registro de la tabla WB_PROVEEDOR
        /// </summary>
        public void SaveWbProveedor(WbProveedorDTO entity)
        {
            try
            {
                FactorySic.GetWbProveedorRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla WB_PROVEEDOR
        /// </summary>
        public void UpdateWbProveedor(WbProveedorDTO entity)
        {
            try
            {
                FactorySic.GetWbProveedorRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla WB_PROVEEDOR
        /// </summary>
        public void DeleteWbProveedor(int provcodi)
        {
            try
            {
                FactorySic.GetWbProveedorRepository().Delete(provcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla WB_PROVEEDOR
        /// </summary>
        public WbProveedorDTO GetByIdWbProveedor(int provcodi)
        {
            return FactorySic.GetWbProveedorRepository().GetById(provcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla WB_PROVEEDOR
        /// </summary>
        public List<WbProveedorDTO> ListWbProveedors()
        {
            return FactorySic.GetWbProveedorRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla WbProveedor
        /// </summary>
        public List<WbProveedorDTO> GetByCriteriaWbProveedors(string nombre, string tipo, DateTime? fechaD, DateTime? fechaH)
        {
            return FactorySic.GetWbProveedorRepository().GetByCriteria(nombre, tipo, fechaD, fechaH);
        }

        /// <summary>
        /// Permite obtener la lista de tipo de proveedores
        /// </summary>
        /// <returns></returns>
        public List<string> GetByCriteriaTipo() 
        {
            return FactorySic.GetWbProveedorRepository().GetByCriteriaTipo();
        }

        #endregion

    }
}
