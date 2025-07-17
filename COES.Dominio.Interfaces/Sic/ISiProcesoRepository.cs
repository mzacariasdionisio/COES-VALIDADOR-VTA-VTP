using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_PROCESO
    /// </summary>
    public interface ISiProcesoRepository
    {
        int Save(SiProcesoDTO entity);        
        void ExecIndicadores(DateTime fechaProceso, int GPS);
        void Update(SiProcesoDTO entity);
        void Delete(int prcscodi);
        SiProcesoDTO GetById(int prcscodi);
        List<SiProcesoDTO> List();
        List<SiProcesoDTO> GetByCriteria();
        SiProcesoDTO ObtenerParametros(int idProceso);
        void ActualizarEstado(int idProceso, string estado);
    }
}
