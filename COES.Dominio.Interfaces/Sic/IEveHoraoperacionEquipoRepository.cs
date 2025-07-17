using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EVE_HORAOPERACION_EQUIPO
    /// </summary>
    public interface IEveHoraoperacionEquipoRepository
    {
        int Save(EveHoraoperacionEquipoDTO entity);
        void Update(EveHoraoperacionEquipoDTO entity);
        void Delete(int hopequcodi);
        EveHoraoperacionEquipoDTO GetById(int hopequcodi);
        List<EveHoraoperacionEquipoDTO> List();
        List<EveHoraoperacionEquipoDTO> GetByCriteria();
        List<EveHoraoperacionEquipoDTO> ObtenerEquiposInvolucrados(string lsthopcodis);
        
    }
}
