using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_TIPOMIGRAOPERACION
    /// </summary>
    public interface ISiTipomigraOperacionRepository
    {
        int Save(SiTipomigraoperacionDTO entity);
        void Update(SiTipomigraoperacionDTO entity);
        void Delete(int tmopercodi);
        SiTipomigraoperacionDTO GetById(int tmopercodi);
        List<SiTipomigraoperacionDTO> List();
        List<SiTipomigraoperacionDTO> GetByCriteria();
    }
}
