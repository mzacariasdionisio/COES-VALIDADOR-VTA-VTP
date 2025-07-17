using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RTU_CONFIGURACION_GRUPO
    /// </summary>
    public interface IRtuConfiguracionGrupoRepository
    {
        int Save(RtuConfiguracionGrupoDTO entity);
        void Update(RtuConfiguracionGrupoDTO entity);
        void Delete(int rtugrucodi);
        RtuConfiguracionGrupoDTO GetById(int rtugrucodi);
        List<RtuConfiguracionGrupoDTO> List();
        List<RtuConfiguracionGrupoDTO> GetByCriteria();
    }
}
