using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RTU_ROLTURNO_ACTIVIDAD
    /// </summary>
    public interface IRtuRolturnoActividadRepository
    {
        int Save(RtuRolturnoActividadDTO entity);
        void Update(RtuRolturnoActividadDTO entity);
        void Delete(int rturaccodi);
        RtuRolturnoActividadDTO GetById(int rturaccodi);
        List<RtuRolturnoActividadDTO> List();
        List<RtuRolturnoActividadDTO> GetByCriteria();
    }
}
