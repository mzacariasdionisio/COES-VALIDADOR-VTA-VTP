using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla MMM_BANDTOL
    /// </summary>
    public interface IMmmBandtolRepository
    {
        int Save(MmmBandtolDTO entity);
        void Update(MmmBandtolDTO entity);
        void Delete(int mmmtolcodi);
        MmmBandtolDTO GetById(int mmmtolcodi);
        MmmBandtolDTO GetByIndicadorYPeriodo(int immecodi, DateTime fechaPeriodo);
        List<MmmBandtolDTO> List();
        List<MmmBandtolDTO> GetByCriteria();
    }
}
