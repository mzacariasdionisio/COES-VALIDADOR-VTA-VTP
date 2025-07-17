using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla MP_CATEGORIA
    /// </summary>
    public interface IMpCategoriaRepository
    {
        int Save(MpCategoriaDTO entity);
        void Update(MpCategoriaDTO entity);
        void Delete(int mcatcodi);
        MpCategoriaDTO GetById(int mcatcodi);
        List<MpCategoriaDTO> List();
        List<MpCategoriaDTO> GetByCriteria();
    }
}
