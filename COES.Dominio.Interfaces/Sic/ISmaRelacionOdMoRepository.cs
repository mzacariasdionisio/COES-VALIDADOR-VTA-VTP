using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

using System.Data; //STS
using System.Data.Common; //STS

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SMA_RELACION_OD_MO
    /// </summary>
    public interface ISmaRelacionOdMoRepository
    {
        int GetMaxId();
        int Save(int oferdecodi, SmaRelacionOdMoDTO entity, IDbConnection conn, DbTransaction tran, int corrMO);
        void Update(SmaRelacionOdMoDTO entity);
        void Delete(int odmocodi);
        SmaRelacionOdMoDTO GetById(int odmocodi);
        List<SmaRelacionOdMoDTO> List();
        List<SmaRelacionOdMoDTO> GetByCriteria();
    }
}
