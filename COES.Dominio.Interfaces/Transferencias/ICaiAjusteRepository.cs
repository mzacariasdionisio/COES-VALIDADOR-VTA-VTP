using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CAI_AJUSTE
    /// </summary>
    public interface ICaiAjusteRepository
    {
        int Save(CaiAjusteDTO entity);
        void Update(CaiAjusteDTO entity);
        void Delete(int caiajcodi);
        CaiAjusteDTO GetById(int caiajcodi);
        List<CaiAjusteDTO> List(int caiprscodi);
        List<CaiAjusteDTO> GetByCriteria();
        List<CaiAjusteDTO> ListByCaiPrscodi(int caiprscodi);
        CaiAjusteDTO GetByIdAnterior(int caiajcodi);
    }
}
