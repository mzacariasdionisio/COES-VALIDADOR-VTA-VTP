using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_CONFIGFORMATENVIO
    /// </summary>
    public interface IMeConfigformatenvioRepository
    {
        int Save(MeConfigformatenvioDTO entity);
        void Delete();
        MeConfigformatenvioDTO GetById(int idCfgenv);
        List<MeConfigformatenvioDTO> List();
        List<MeConfigformatenvioDTO> GetByCriteria(int idEmpresa,int idFormato);
    }
}
