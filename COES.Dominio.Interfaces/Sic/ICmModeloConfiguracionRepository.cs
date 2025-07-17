using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CM_MODELO_CONFIGURACION
    /// </summary>
    public interface ICmModeloConfiguracionRepository
    {
        int Save(CmModeloConfiguracionDTO entity);
        void Update(CmModeloConfiguracionDTO entity);
        void Delete(int modconcodi);
        CmModeloConfiguracionDTO GetById(int modconcodi);
        List<CmModeloConfiguracionDTO> List();
        List<CmModeloConfiguracionDTO> GetByCriteria(int modembcodi);
    }
}
