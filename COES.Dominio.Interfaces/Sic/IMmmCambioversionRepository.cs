using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla MMM_CAMBIOVERSION
    /// </summary>
    public interface IMmmCambioversionRepository
    {
        int Save(MmmCambioversionDTO entity);
        void Update(MmmCambioversionDTO entity);
        MmmCambioversionDTO GetById(int camvercodi);
        List<MmmCambioversionDTO> List();
        List<MmmCambioversionDTO> ListByPeriodo(DateTime fechaPeriodo);
        List<MmmCambioversionDTO> GetByCriteria(int vermmcodi);
    }
}
