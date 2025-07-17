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

namespace COES.Servicios.Aplicacion.General
{
   public class MovilAppServicio : AppServicioBase
    {

        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(MovilAppServicio));

        #region Métodos Tabla WB_REGISTROIMEI

        /// <summary>
        /// Inserta un registro de la tabla WB_REGISTROIMEI
        /// </summary>
        public void SaveWbRegistroimei(WbRegistroimeiDTO entity)
        {
            try
            {
                FactorySic.GetWbRegistroimeiRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla WB_REGISTROIMEI
        /// </summary>
        public void UpdateWbRegistroimei(WbRegistroimeiDTO entity)
        {
            try
            {
                FactorySic.GetWbRegistroimeiRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla WB_REGISTROIMEI
        /// </summary>
        public void DeleteWbRegistroimei(int regimecodi)
        {
            try
            {
                FactorySic.GetWbRegistroimeiRepository().Delete(regimecodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla WB_REGISTROIMEI
        /// </summary>
        public WbRegistroimeiDTO GetByIdWbRegistroimei(int regimecodi)
        {
            return FactorySic.GetWbRegistroimeiRepository().GetById(regimecodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla WB_REGISTROIMEI
        /// </summary>
        public List<WbRegistroimeiDTO> ListWbRegistroimeis()
        {
            return FactorySic.GetWbRegistroimeiRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla WbRegistroimei
        /// </summary>
        public List<WbRegistroimeiDTO> GetByCriteriaWbRegistroimeis()
        {
            return FactorySic.GetWbRegistroimeiRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla WB_AYUDAAPP

        /// <summary>
        /// Inserta un registro de la tabla WB_AYUDAAPP
        /// </summary>
        public void SaveWbAyudaapp(WbAyudaappDTO entity)
        {
            try
            {
                FactorySic.GetWbAyudaappRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla WB_AYUDAAPP
        /// </summary>
        public void UpdateWbAyudaapp(WbAyudaappDTO entity)
        {
            try
            {
                FactorySic.GetWbAyudaappRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla WB_AYUDAAPP
        /// </summary>
        public void DeleteWbAyudaapp(int ayuappcodi)
        {
            try
            {
                FactorySic.GetWbAyudaappRepository().Delete(ayuappcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla WB_AYUDAAPP
        /// </summary>
        public WbAyudaappDTO GetByIdWbAyudaapp(int ayuappcodi)
        {
            return FactorySic.GetWbAyudaappRepository().GetById(ayuappcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla WB_AYUDAAPP
        /// </summary>
        public List<WbAyudaappDTO> ListWbAyudaapps()
        {
            return FactorySic.GetWbAyudaappRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla WbAyudaapp
        /// </summary>
        public List<WbAyudaappDTO> GetByCriteriaWbAyudaapps()
        {
            return FactorySic.GetWbAyudaappRepository().GetByCriteria();
        }

        #endregion

        public int ValidarCredenciales(string usuario, string clave)
        {
            if (usuario == "usuariozeit" && clave == "zeit2018")
            {
                return 1;
            }
            return 0;
        }

        #region Nuevos Metodos

        public List<WbTipoNotificacionDTO> ObtenerTipoNotificacionEventos()
        {
            return FactorySic.GetWbNotificacionRepository().ObtenerTipoNotificacionEventos();
        }

        #endregion

    }
}
