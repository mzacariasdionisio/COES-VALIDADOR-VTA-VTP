using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PMO_SDDP_CODIGO
    /// </summary>
    public interface IPmoSddpCodigoRepository
    {
        int Save(PmoSddpCodigoDTO entity);
        void Update(PmoSddpCodigoDTO entity);
        void Delete(int sddpcodi);
        PmoSddpCodigoDTO GetById(int sddpcodi);
        PmoSddpCodigoDTO GetByNumYTipo(int num, int tsddpcodi);
        List<PmoSddpCodigoDTO> List();
        List<PmoSddpCodigoDTO> GetByCriteria(string tsddpcodi);
    }
}
