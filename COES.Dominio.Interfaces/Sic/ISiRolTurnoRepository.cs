using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_ROL_TURNO
    /// </summary>
    public interface ISiRolTurnoRepository
    {
        void Save(SiRolTurnoDTO entity);
        void Update(SiRolTurnoDTO entity);
        void Delete(DateTime roltfecha, int actcodi, DateTime lastdate, int percodi);
        SiRolTurnoDTO GetById(DateTime roltfecha, int actcodi, DateTime lastdate, int percodi);
        List<SiRolTurnoDTO> List();
        List<SiRolTurnoDTO> GetByCriteria();
        List<SiRolTurnoDTO> ListaRols(DateTime fecIni, DateTime fecFin, string percodi);
        void SaveSiRolTurnoMasivo(List<SiRolTurnoDTO> Rols);
        void DeleteSiRolTurnoMasivo(DateTime fecIni, DateTime fecFin, string percodi);
        List<SiRolTurnoDTO> ListaMovimientos(DateTime fecIni, DateTime fecFin);
    }
}
