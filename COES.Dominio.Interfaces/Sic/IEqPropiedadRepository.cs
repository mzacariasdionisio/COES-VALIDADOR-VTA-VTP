using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EQ_PROPIEDAD
    /// </summary>
    public interface IEqPropiedadRepository
    {
        int Save(EqPropiedadDTO entity, IDbConnection connection, IDbTransaction transaction);
        void Update(EqPropiedadDTO entity, IDbConnection connection, IDbTransaction transaction);
        void Delete(int propcodi);
        void Delete_UpdateAuditoria(int propcodi, string username);
        EqPropiedadDTO GetById(int propcodi);
        List<EqPropiedadDTO> List();
        List<EqPropiedadDTO> GetByCriteria(int famcodi, string nombre, int estado);

        #region MigracionSGOCOES-GrupoB
        List<EqPropiedadDTO> ListByFamcodi(int famcodi);
        #endregion

        #region Ficha Tecnica 2023
        List<EqPropiedadDTO> ListByIds(string propcodis);
        #endregion
    }
}
