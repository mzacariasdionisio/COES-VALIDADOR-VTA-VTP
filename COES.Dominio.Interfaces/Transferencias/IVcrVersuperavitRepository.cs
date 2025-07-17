using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VCR_VERSUPERAVIT
    /// </summary>
    public interface IVcrVersuperavitRepository
    {
        int Save(VcrVersuperavitDTO entity);
        void Update(VcrVersuperavitDTO entity);
        void Delete(int vcrvsacodi);
        VcrVersuperavitDTO GetById(int vcrvsacodi);
        List<VcrVersuperavitDTO> List();
        List<VcrVersuperavitDTO> GetByCriteria(int vcrdsrcodi);
        List<VcrVersuperavitDTO> ListDia(int vcrdsrcodi, int grupocodi, DateTime vcrvsafecha);

        //Agregado el 29-04-2019
        List<VcrVersuperavitDTO> ListDiaURS(DateTime vcrvsafecha);
    }
}
