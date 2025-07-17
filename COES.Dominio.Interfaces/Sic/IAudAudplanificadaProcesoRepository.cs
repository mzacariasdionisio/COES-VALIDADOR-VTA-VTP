using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla AUD_AUDPLANIFICADA_PROCESO
    /// </summary>
    public interface IAudAudplanificadaProcesoRepository
    {
        int Save(AudAudplanificadaprocesoDTO entity);
        void Update(AudAudplanificadaprocesoDTO entity);
        void Delete(AudAudplanificadaprocesoDTO audPlanificadaProceso);
        void DeleteByAudPlanificada(AudAudplanificadaprocesoDTO audPlanificadaProceso);
        AudAudplanificadaprocesoDTO GetById(int audpcodi, int proccodi);
        List<AudAudplanificadaprocesoDTO> List(int audpcodi);
        List<AudAudplanificadaprocesoDTO> GetByCriteria(int audpcodi, string areacodi);
    }
}
