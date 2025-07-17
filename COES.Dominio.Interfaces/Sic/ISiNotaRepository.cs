using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_NOTA
    /// </summary>
    public interface ISiNotaRepository
    {
        int Save(SiNotaDTO entity);
        void Update(SiNotaDTO entity);
        void Delete(int sinotacodi);
        SiNotaDTO GetById(int sinotacodi);
        List<SiNotaDTO> List();
        List<SiNotaDTO> GetByCriteria(DateTime periodo, int mrepcodi, int verscodi);
        void UpdateOrden(SiNotaDTO entity);
        int GetMaxSinotaorden(DateTime periodo, int mrepcodi, int? verscodi);
    }
}
