using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IWbNotificacionRepository
    {
        int Save(WbNotificacionDTO entity);
        void Update(WbNotificacionDTO entity);
        void Delete(int notiCodi);
        WbNotificacionDTO GetById(int notiCodi);
        List<WbNotificacionDTO> List();
        List<WbNotificacionDTO> GetByCriteria(string titulo, DateTime? fechaInicio, DateTime? fechaFin);
        void CambiarEstadoNotificacion(int notiCodi);

        List<WbTipoNotificacionDTO> ObtenerTipoNotificacionEventos();
    }
}
