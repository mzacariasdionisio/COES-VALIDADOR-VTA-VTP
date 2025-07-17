using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CAI_SDDP_PARAMETRO
    /// </summary>
    public interface ICaiSddpParametroRepository
    {
        int Save(CaiSddpParametroDTO entity);
        void Update(CaiSddpParametroDTO entity);
        void Delete();
        CaiSddpParametroDTO GetById(int caiajcodi);
        List<CaiSddpParametroDTO> List(int Sddppmcodi);
        List<CaiSddpParametroDTO> GetByCriteria();
    }
}
