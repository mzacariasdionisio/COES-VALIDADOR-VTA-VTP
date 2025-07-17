using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RI_ETAPAREVISION
    /// </summary>
    public interface IRiEtaparevisionRepository
    {
        int Save(RiEtaparevisionDTO entity);
        void Update(RiEtaparevisionDTO entity);
        void Delete(int etrvcodi);
        RiEtaparevisionDTO GetById(int etrvcodi);
        List<RiEtaparevisionDTO> List();
        List<RiEtaparevisionDTO> GetByCriteria();
    }
}
