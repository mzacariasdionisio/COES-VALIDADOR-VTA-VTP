using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RTU_CONFIGURACION_PERSONA
    /// </summary>
    public interface IRtuConfiguracionPersonaRepository
    {
        int Save(RtuConfiguracionPersonaDTO entity);
        void Update(RtuConfiguracionPersonaDTO entity);
        void Delete(int rtupercodi);
        RtuConfiguracionPersonaDTO GetById(int rtupercodi);
        List<RtuConfiguracionPersonaDTO> List();
        List<RtuConfiguracionPersonaDTO> GetByCriteria();
    }
}
