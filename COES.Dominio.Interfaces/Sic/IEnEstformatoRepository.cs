using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EN_ESTFORMATO
    /// </summary>
    public interface IEnEstformatoRepository
    {
        int Save(EnEstformatoDTO entity);
        void Update(EnEstformatoDTO entity);
        void Delete(int estfmtcodi);
        EnEstformatoDTO GetById(int estfmtcodi);
        List<EnEstformatoDTO> List();
        List<EnEstformatoDTO> ListFormatoXEstado(int ensayocodi, int iformatocodi);
        List<EnEstformatoDTO> GetByCriteria();
    }
}
