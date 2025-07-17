using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_TIPOINFORMACION
    /// </summary>
    public interface ISiTipoinformacionRepository
    {
        void Save(SiTipoinformacionDTO entity);
        void Update(SiTipoinformacionDTO entity);
        void Delete(int tipoinfocodi);
        SiTipoinformacionDTO GetById(int tipoinfocodi);
        List<SiTipoinformacionDTO> List();
        List<SiTipoinformacionDTO> GetByCriteria(string tipoinfocodi);

    }
}
