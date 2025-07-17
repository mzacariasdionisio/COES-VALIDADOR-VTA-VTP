using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.GMM
{
    public class DetIncumplimientoAppServicio
    {

        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(IncumplimientoAppServicio));

        /// <summary>
        /// Inserta un registro de la tabla ...
        /// </summary>
        public void Save(GmmDetIncumplimientoDTO entity)
        {
            try
            {
                int _empgcodi = FactorySic.GetGmmDetIncumplimientoRepository().Save(entity);

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Elimina un registro de la tabla ...
        /// </summary>
        public void Delete(int dinccodi)
        {
            try
            {
                FactorySic.GetGmmDetIncumplimientoRepository().Delete(dinccodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public List<GmmDetIncumplimientoDTO> ListarArchivos(int dinccodi)
        {
            return FactorySic.GetGmmDetIncumplimientoRepository().ListarArchivos(dinccodi);
        }


        public List<GmmTipInformeDTO> ListarTipoInforme()
        {
            return FactorySic.GetGmmDetIncumplimientoRepository().ListarTipoInforme();
        }
    }
}
