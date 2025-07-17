using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CO_TIPOINFORMACION
    /// </summary>
    public interface ICoTipoinformacionRepository
    {
        int Save(CoTipoinformacionDTO entity);
        void Update(CoTipoinformacionDTO entity);
        void Delete(int cotinfcodi);
        CoTipoinformacionDTO GetById(int cotinfcodi);
        List<CoTipoinformacionDTO> List();
        List<CoTipoinformacionDTO> GetByCriteria();
    }
}
