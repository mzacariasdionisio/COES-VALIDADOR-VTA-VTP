using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EQ_GRUPO_LINEA
    /// </summary>
    public interface IEqGrupoLineaRepository
    {
        int Save(EqGrupoLineaDTO entity);
        void Update(EqGrupoLineaDTO entity);
        void Delete(int grulincodi);
        EqGrupoLineaDTO GetById(int grulincodi);
        List<EqGrupoLineaDTO> List();
        List<EqGrupoLineaDTO> GetByCriteria(string tipo);
    }
}
