using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_VERIFICACION
    /// </summary>
    public interface IMeVerificacionRepository
    {
        int Save(MeVerificacionDTO entity);
        void Update(MeVerificacionDTO entity);
        void Delete(int verifcodi);
        MeVerificacionDTO GetById(int verifcodi);
        List<MeVerificacionDTO> List();
        List<MeVerificacionDTO> GetByCriteria();
    }
}
