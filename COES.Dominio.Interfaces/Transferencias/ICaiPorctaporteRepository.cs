using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CAI_PORCTAPORTE
    /// </summary>
    public interface ICaiPorctaporteRepository
    {
        int Save(CaiPorctaporteDTO entity);
        void Update(CaiPorctaporteDTO entity);
        void Delete(int caiajcodi);
        CaiPorctaporteDTO GetById(int caipacodi);
        List<CaiPorctaporteDTO> List();
        List<CaiPorctaporteDTO> GetByCriteria();
        List<CaiPorctaporteDTO> ByEmpresaImporte(int caiajcodi);
    }
}
