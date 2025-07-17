using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PR_CONCEPTO
    /// </summary>
    public interface IPrConceptoRepository
    {
        int Save(PrConceptoDTO entity, IDbConnection connection, IDbTransaction transaction);
        void Update(PrConceptoDTO entity, IDbConnection connection, IDbTransaction transaction);
        void Delete(int concepcodi);
        PrConceptoDTO GetById(int concepcodi);
        List<PrConceptoDTO> List();
        List<PrConceptoDTO> GetByCriteria(string concepcodis);
        List<PrConceptoDTO> ListarConceptosxFiltro(int catecodi, string nombre, int estado);

        #region MigracionSGOCOES-GrupoB
        List<PrConceptoDTO> ListByCatecodi(string catecodi);
        #endregion
    }
}

