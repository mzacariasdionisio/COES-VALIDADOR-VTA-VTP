using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RI_SOLICITUDDETALLE
    /// </summary>
    public interface IRiSolicituddetalleRepository
    {
        int Save(RiSolicituddetalleDTO entity);
        void Update(RiSolicituddetalleDTO entity);
        void Delete(int sdetcodi);
        RiSolicituddetalleDTO GetById(int sdetcodi);
        List<RiSolicituddetalleDTO> List();
        List<RiSolicituddetalleDTO> GetByCriteria();
        List<RiSolicituddetalleDTO> ListBySolicodi(int solicodi);
    }
}
