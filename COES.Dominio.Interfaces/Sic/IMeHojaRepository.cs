using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_HOJA
    /// </summary>
    public interface IMeHojaRepository
    {
        int Save(MeHojaDTO entity);
        void Update(MeHojaDTO entity);
        void Delete(int id);
        MeHojaDTO GetById(int id);
        List<MeHojaDTO> List();
        List<MeHojaDTO> GetByCriteria(int formatcodi);
        List<MeHojaDTO> ListPadre(int formatCodi);
    }
}
