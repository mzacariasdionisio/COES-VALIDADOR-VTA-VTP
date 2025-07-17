using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;


namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CAI_SDDP_PARAMINT
    /// </summary>
    /// 
    public interface ICaiSddpParamintRepository
    {
        int Save(CaiSddpParamintDTO entity);
        void Update(CaiSddpParamintDTO entity);
        void Delete();
        CaiSddpParamintDTO GetById(int Sddppicodi);
        List<CaiSddpParamintDTO> List(int Sddppicodi);
        List<CaiSddpParamintDTO> GetByCriteria(int caiajcodi);
    }
}
