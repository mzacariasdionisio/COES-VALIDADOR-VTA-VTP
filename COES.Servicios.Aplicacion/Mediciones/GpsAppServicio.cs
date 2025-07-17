using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Mediciones
{
    public class GpsAppServicio
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(GpsAppServicio));

        #region Métodos Tabla ME_GPS

        /// <summary>
        /// Inserta un registro de la tabla ME_GPS
        /// </summary>
        public void SaveMeGps(MeGpsDTO entity)
        {
            try
            {
                FactorySic.GetMeGpsRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ME_GPS
        /// </summary>
        public void UpdateMeGps(MeGpsDTO entity)
        {
            try
            {
                FactorySic.GetMeGpsRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla ME_GPS
        /// </summary>
        public void DeleteMeGps(int gpscodi)
        {
            try
            {
                FactorySic.GetMeGpsRepository().Delete(gpscodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ME_GPS
        /// </summary>
        public MeGpsDTO GetByIdMeGps(int gpscodi)
        {
            return FactorySic.GetMeGpsRepository().GetById(gpscodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_GPS
        /// </summary>
        public List<MeGpsDTO> ListMeGpss()
        {
            return FactorySic.GetMeGpsRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla MeGps
        /// </summary>
        public List<MeGpsDTO> GetByCriteriaMeGpss(int? empresa, string nombre, string oficial)
        {
            return FactorySic.GetMeGpsRepository().GetByCriteria(empresa, nombre, oficial);
        }

        #endregion

        /// <summary>
        /// Lista las empresas
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarEmpresas()
        {
            return FactorySic.GetSiEmpresaRepository().ListGeneral();
        }
    }
}
