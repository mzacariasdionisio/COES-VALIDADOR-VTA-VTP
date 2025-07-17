using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla MMM_VERSION
    /// </summary>
    public interface IMmmVersionRepository
    {
        int Save(MmmVersionDTO entity);
        void Update(MmmVersionDTO entity);
        void Delete(int vermmcodi);
        MmmVersionDTO GetById(int vermmcodi);
        List<MmmVersionDTO> List();
        List<MmmVersionDTO> GetByCriteria();
        void UpdatePorcentaje(MmmVersionDTO entity);
        void UpdateEstadoVersion(MmmVersionDTO entity);

    }
}
