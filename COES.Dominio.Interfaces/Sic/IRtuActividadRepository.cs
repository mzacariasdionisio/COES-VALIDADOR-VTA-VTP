using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RTU_ACTIVIDAD
    /// </summary>
    public interface IRtuActividadRepository
    {
        int Save(RtuActividadDTO entity);
        void Update(RtuActividadDTO entity);
        void Delete(int rtuactcodi, string username);
        RtuActividadDTO GetById(int rtuactcodi);
        List<RtuActividadDTO> List();
        List<RtuActividadDTO> GetByCriteria();
        List<RtuActividadDTO> ObtenerTipoResponsables();
        List<RtuActividadDTO> ObtenerActividadesPorTipoInforme(int tipoReporte);
    }
}
