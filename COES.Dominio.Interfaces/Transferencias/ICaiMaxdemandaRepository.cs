using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CAI_MAXDEMANDA
    /// </summary>
    public interface ICaiMaxdemandaRepository
    {
        int Save(CaiMaxdemandaDTO entity);
        void Update(CaiMaxdemandaDTO entity);
        void Delete(int caimdecodi);
        CaiMaxdemandaDTO GetById(int caimdecodi);
        List<CaiMaxdemandaDTO> List(int caiajcodi);
        List<CaiMaxdemandaDTO> GetByCriteria();
        void DeleteEjecutada(int caiajcodi);
        CaiMaxdemandaDTO GetByCaimdeAnioMes(int Caiajcodi, int CaimdeAnioMes);
    }
}
