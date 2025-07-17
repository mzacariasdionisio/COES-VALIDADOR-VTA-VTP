using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PFR_TIPO
    /// </summary>
    public interface IPfrTipoRepository
    {
        int Save(PfrTipoDTO entity);
        void Update(PfrTipoDTO entity);
        void Delete(int pfrcatcodi);
        PfrTipoDTO GetById(int pfrcatcodi);
        List<PfrTipoDTO> List();
        List<PfrTipoDTO> GetByCriteria();
    }
}
