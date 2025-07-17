using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla AUD_PROGRAMAAUDITORIA
    /// </summary>
    public interface IAudProgramaauditoriaRepository
    {
        int Save(AudProgramaauditoriaDTO entity);
        void Update(AudProgramaauditoriaDTO entity);
        void Delete(AudProgramaauditoriaDTO entity);
        AudProgramaauditoriaDTO GetById(int progacodi);
        List<AudProgramaauditoriaDTO> List();
        List<AudProgramaauditoriaDTO> GetByCriteria(int audicodi);
        List<AudProgramaauditoriaDTO> GetByCriteriaEjecucion(AudProgramaauditoriaDTO programa);
    }
}
