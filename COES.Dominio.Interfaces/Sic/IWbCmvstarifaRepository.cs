using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla WB_CMVSTARIFA
    /// </summary>
    public interface IWbCmvstarifaRepository
    {
        int Save(WbCmvstarifaDTO entity);
        void Update(WbCmvstarifaDTO entity);
        void Delete(int cmtarcodi);
        WbCmvstarifaDTO GetById(int cmtarcodi);
        List<WbCmvstarifaDTO> List();
        List<WbCmvstarifaDTO> GetByCriteria(DateTime fechaInicio, DateTime fechaFin);
    }
}
