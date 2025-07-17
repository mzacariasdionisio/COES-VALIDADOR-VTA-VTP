using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;

namespace COES.Servicios.Aplicacion.Demo
{
    /// <summary>
    /// Clases con métodos del módulo Demo
    /// </summary>
    public class DemoAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(DemoAppServicio));

        #region Métodos Tabla SI_PRUEBA

        /// <summary>
        /// Inserta un registro de la tabla SI_PRUEBA
        /// </summary>
        public void SaveSiPrueba(SiPruebaDTO entity)
        {
            try
            {
                //FactorySic.GetSiPruebaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SI_PRUEBA
        /// </summary>
        public void UpdateSiPrueba(SiPruebaDTO entity)
        {
            try
            {
                FactorySic.GetSiPruebaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SI_PRUEBA
        /// </summary>
        public void DeleteSiPrueba(string pruebacodi)
        {
            try
            {
                FactorySic.GetSiPruebaRepository().Delete(pruebacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SI_PRUEBA
        /// </summary>
        public SiPruebaDTO GetByIdSiPrueba(string pruebacodi)
        {
            return FactorySic.GetSiPruebaRepository().GetById(pruebacodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SI_PRUEBA
        /// </summary>
        public List<SiPruebaDTO> ListSiPruebas()
        {
            return FactorySic.GetSiPruebaRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SiPrueba
        /// </summary>
        public List<SiPruebaDTO> GetByCriteriaSiPruebas()
        {
            return FactorySic.GetSiPruebaRepository().GetByCriteria();
        }

        /// <summary>
        /// Busca por el nombre
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public List<SiPruebaDTO> BuscarPorNombre(string nombre)
        {            
            return FactorySic.GetSiPruebaRepository().BuscarPorNombre(nombre);
        }

        #endregion

    }
}
