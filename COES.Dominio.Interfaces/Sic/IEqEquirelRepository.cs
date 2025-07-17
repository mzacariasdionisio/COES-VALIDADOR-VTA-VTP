using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EQ_EQUIREL
    /// </summary>
    public interface IEqEquirelRepository
    {
        void Save(EqEquirelDTO entity);
        void Update(EqEquirelDTO entity);
        void Delete(int equicodi1, int tiporelcodi, int equicodi2);
        void Delete_UpdateAuditoria(int equicodi1, int tiporelcodi, int equicodi2, string user);
        EqEquirelDTO GetById(int equicodi1, int tiporelcodi, int equicodi2);
        List<EqEquirelDTO> List();
        List<EqEquirelDTO> GetByCriteria(int equicodi, string tiporelcodi);
        List<EqEquirelDTO> GetByCriteriaTopologia(int equicodi1, int tipoRelTopologia);

        #region INTERVENCIONES
        List<EqEquirelDTO> ListarXRelacionesXIds(string idsEquipos);
        List<EqEquirelDTO> ListarXRelacionesBarraXIds(string idsEquipos);
        #endregion
    }

}
