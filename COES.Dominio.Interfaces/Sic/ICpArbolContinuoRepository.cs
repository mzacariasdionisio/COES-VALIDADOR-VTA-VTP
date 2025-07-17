using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CP_ARBOL_CONTINUO
    /// </summary>
    public interface ICpArbolContinuoRepository
    {
        int Save(CpArbolContinuoDTO regArbol, IDbConnection connection, DbTransaction transaction);
        void Update(CpArbolContinuoDTO entity);
        void Delete(int cparbcodi);
        CpArbolContinuoDTO GetById(int cparbcodi);
        List<CpArbolContinuoDTO> List();
        List<CpArbolContinuoDTO> GetByCriteria(int topcodi);
        CpArbolContinuoDTO GetUltimoArbol();
    }
}
