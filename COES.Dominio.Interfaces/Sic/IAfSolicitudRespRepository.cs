using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla AF_SOLICITUD_RESP
    /// </summary>
    public interface IAfSolicitudRespRepository
    {
        int Save(AfSolicitudRespDTO entity);
        int Update(AfSolicitudRespDTO entity);
        void Delete(int sorespcodi);
        AfSolicitudRespDTO GetById(int sorespcodi);
        List<AfSolicitudRespDTO> List();
        List<AfSolicitudRespDTO> GetByCriteria();

        //Lista solicitudes por filtro
        List<AfSolicitudRespDTO> ListarSolicitudesxFiltro(AfSolicitudRespDTO solicitud);
    }
}
