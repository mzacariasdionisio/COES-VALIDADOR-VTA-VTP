using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CM_VOLUMEN_CALCULO
    /// </summary>
    public interface ICmVolumenCalculoRepository
    {
        int Save(CmVolumenCalculoDTO entity);
        void Update(CmVolumenCalculoDTO entity);
        void Delete(int volcalcodi);
        CmVolumenCalculoDTO GetById(int volcalcodi);
        List<CmVolumenCalculoDTO> List();
        List<CmVolumenCalculoDTO> GetByCriteria(DateTime fechaPeriodo, int periodoH);
    }
}
