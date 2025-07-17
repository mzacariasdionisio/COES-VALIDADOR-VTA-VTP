using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EVE_HO_EQUIPOREL
    /// </summary>
    public interface IEveHoEquiporelRepository
    {
        int Save(EveHoEquiporelDTO entity);
        void Update(EveHoEquiporelDTO entity);
        void Delete(int hoequicodi);
        void DeleteByHopcodi(int hopcodi);
        EveHoEquiporelDTO GetById();
        List<EveHoEquiporelDTO> List();
        List<EveHoEquiporelDTO> GetByCriteria(DateTime fechaIni, DateTime fechaFin);
        List<EveHoEquiporelDTO> ListaByHopcodi(int hopcodi);
    }
}
