using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PR_GRUPO_EQUIPO_VAL
    /// </summary>
    public interface IPrGrupoEquipoValRepository
    {
        void Save(PrGrupoEquipoValDTO entity);
        void Update(PrGrupoEquipoValDTO entity);
        void Delete(int grupocodi, int concepcodi, int equicodi, DateTime greqvafechadat, int greqvadeleted);
        PrGrupoEquipoValDTO GetById(int grupocodi, int concepcodi, int equicodi, DateTime greqvafechadat, int greqvadeleted);
        List<PrGrupoEquipoValDTO> List();
        List<PrGrupoEquipoValDTO> GetByCriteria();
        List<PrGrupoEquipoValDTO> GetValorPropiedadDetalle(int idGrupo, int idConcepto);
        decimal GetValorPropiedadDetalleEquipo(int idEquipo, int idConcepto, DateTime fecha);

        List<PrGrupoEquipoValDTO> ListarHistoricoValores(string concepcodi, string idEquipo, string idGrupo);
        List<PrGrupoEquipoValDTO> ListarPrGrupoEquipoValVigente(DateTime fechaVigencia, string concepcodi, string idEquipo, string idGrupo);
    }
}
