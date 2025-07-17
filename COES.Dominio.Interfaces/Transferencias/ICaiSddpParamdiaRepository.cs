using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;


namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CAI_SDDP_PARAMDIA
    /// </summary> 
    public interface ICaiSddpParamdiaRepository
    {
        int Save(CaiSddpParamdiaDTO entity);
        void Update(CaiSddpParamdiaDTO entity);
        void Delete();
        CaiSddpParamdiaDTO GetById(int caiajcodi);
        List<CaiSddpParamdiaDTO> List(int Sddppicodi);
        List<CaiSddpParamdiaDTO> GetByCriteria(int caiajcodi);
        CaiSddpParamdiaDTO GetByDiaCaiSddpParamdia(DateTime sddppddia);
    }
}

