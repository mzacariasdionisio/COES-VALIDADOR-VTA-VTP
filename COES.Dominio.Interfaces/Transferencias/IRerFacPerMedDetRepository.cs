using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RER_FAC_PER_MED_DET
    /// </summary>
    public interface IRerFacPerMedDetRepository
    {
        int Save(RerFacPerMedDetDTO entity);
        void Update(RerFacPerMedDetDTO entity);
        void Delete(int rerFpdCodi);
        RerFacPerMedDetDTO GetById(int rerFpdCodi);
        List<RerFacPerMedDetDTO> List();
        List<RerFacPerMedDetDTO> ListByFPM(int id);
        List<RerFacPerMedDetDTO> GetByRangeDate(DateTime dtInicio, DateTime dtFin);
    }
}
