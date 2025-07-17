using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RTU_ROLTURNO_DETALLE
    /// </summary>
    public interface IRtuRolturnoDetalleRepository
    {
        int Save(RtuRolturnoDetalleDTO entity);
        void Update(RtuRolturnoDetalleDTO entity);
        void Delete(int rtudetcodi);
        RtuRolturnoDetalleDTO GetById(int rtudetcodi);
        List<RtuRolturnoDetalleDTO> List();
        List<RtuRolturnoDetalleDTO> GetByCriteria();
    }
}
