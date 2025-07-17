using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CAI_EQUIUNIDBARR
    /// </summary>
    public interface ICaiEquiunidbarrRepository
    {
        int Save(CaiEquiunidbarrDTO entity);
        void Update(CaiEquiunidbarrDTO entity);
        void Delete(int caiunbcodi);
        CaiEquiunidbarrDTO GetById(int caiunbcodi);
        CaiEquiunidbarrDTO GetByIdBarrcodi(int barrcodi);
        List<CaiEquiunidbarrDTO> List();
        List<CaiEquiunidbarrDTO> GetByCriteria();
        CaiEquiunidbarrDTO GetByEquicodiUNI(int equicodiuni);
    }
}
