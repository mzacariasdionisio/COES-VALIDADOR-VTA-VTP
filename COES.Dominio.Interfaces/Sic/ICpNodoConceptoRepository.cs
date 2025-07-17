using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CP_NODO_CONCEPTO
    /// </summary>
    public interface ICpNodoConceptoRepository
    {
        int Save(CpNodoConceptoDTO entity);
        void Update(CpNodoConceptoDTO entity);
        void Delete(int cpnconcodi);
        CpNodoConceptoDTO GetById(int cpnconcodi);
        List<CpNodoConceptoDTO> List();
        List<CpNodoConceptoDTO> GetByCriteria();
    }
}
