using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_MENSAJEENVIO
    /// </summary>
    public interface IMeMensajeenvioRepository
    {
        void Update(MeMensajeenvioDTO entity);
        void Delete();
        MeMensajeenvioDTO GetById();
        List<MeMensajeenvioDTO> List();
        List<MeMensajeenvioDTO> GetByCriteria();
    }
}
