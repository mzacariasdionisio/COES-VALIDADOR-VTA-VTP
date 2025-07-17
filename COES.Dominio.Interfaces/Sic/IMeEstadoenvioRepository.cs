using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_ESTADOENVIO
    /// </summary>
    public interface IMeEstadoenvioRepository
    {
        void Update(MeEstadoenvioDTO entity);
        void Delete();
        MeEstadoenvioDTO GetById();
        List<MeEstadoenvioDTO> List();
        List<MeEstadoenvioDTO> GetByCriteria();
    }
}
