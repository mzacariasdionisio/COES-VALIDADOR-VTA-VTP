using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_MEDIDOR
    /// </summary>
    public interface IMeMedidorRepository
    {
        int Save(MeMedidorDTO entity);
        void Update(MeMedidorDTO entity);
        void Delete(int medicodi);
        MeMedidorDTO GetById(int medicodi);
        List<MeMedidorDTO> List();
        List<MeMedidorDTO> GetByCriteria();
    }
}
