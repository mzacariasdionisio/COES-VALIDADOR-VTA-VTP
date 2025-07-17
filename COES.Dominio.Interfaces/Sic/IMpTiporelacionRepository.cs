using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla MP_TIPORELACION
    /// </summary>
    public interface IMpTiporelacionRepository
    {
        int Save(MpTiporelacionDTO entity);
        void Update(MpTiporelacionDTO entity);
        void Delete(int mtrelcodi);
        MpTiporelacionDTO GetById(int mtrelcodi);
        List<MpTiporelacionDTO> List();
        List<MpTiporelacionDTO> GetByCriteria();
    }
}
