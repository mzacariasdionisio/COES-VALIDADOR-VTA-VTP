using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_RELACIONPTO
    /// </summary>
    public interface IMeRelacionptoRepository
    {
        int Save(MeRelacionptoDTO entity);
        void Update(MeRelacionptoDTO entity);
        void Delete(int relptocodi);
        MeRelacionptoDTO GetById(int relptocodi);
        List<MeRelacionptoDTO> List();
        List<MeRelacionptoDTO> GetByCriteria(string ptomedicodical, string ptomedicodi);
    }
}
