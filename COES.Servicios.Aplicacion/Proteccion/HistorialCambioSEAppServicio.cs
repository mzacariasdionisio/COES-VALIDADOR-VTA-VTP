using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using log4net;
using System.Collections.Generic;


namespace COES.Servicios.Aplicacion.Equipamiento
{
    /// <summary>
    /// Clases con métodos del módulo Equipamiento
    /// </summary>
    public class HistorialCambioSEAppServicio : AppServicioBase
    {

        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ProyectoActualizacionAppServicio));


        public int Save(EprSubestacionDTO entity)
        {
            return FactorySic.GetEprSubestacionRepository().Save(entity);
        }

        public void Update(EprSubestacionDTO entity)
        {
            FactorySic.GetEprSubestacionRepository().Update(entity);
        }

        public void Delete_UpdateAuditoria(EprSubestacionDTO entity)
        {
            FactorySic.GetEprSubestacionRepository().Delete_UpdateAuditoria(entity);
        }

        public EprSubestacionDTO GetById(int epsubecodi)
        {
            return FactorySic.GetEprSubestacionRepository().GetById(epsubecodi);
        }

        public List<EprSubestacionDTO> List(int areacodi, int zonacodi)
        {
            return FactorySic.GetEprSubestacionRepository().List(areacodi, zonacodi);
        }

     

    }
}
