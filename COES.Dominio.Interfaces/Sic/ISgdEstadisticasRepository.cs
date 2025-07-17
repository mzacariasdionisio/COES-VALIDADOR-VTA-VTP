using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    ///
    /// </summary>
    public interface ISgdEstadisticasRepository
    {
        int Save(SgdEstadisticasDTO entity);
        void Update(SgdEstadisticasDTO entity);
        void Delete();
        SgdEstadisticasDTO GetById(int idEnvio);

        void UpdateCodiref(int sgdecodi, int fljcodiref);
        void UpdateNumext(int fljcodiresp, int fljcodi);

        List<SgdEstadisticasDTO> List(SgdEstadisticasDTO filterData);
        List<SgdEstadisticasDTO> GetByCriteria();
        List<SgdEstadisticasDTO> ListCodigosRef(DateTime fechaInicio, DateTime fechaFin);

    }
}
