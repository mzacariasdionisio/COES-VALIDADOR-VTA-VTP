using COES.Dominio.DTO.Sic;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CP_NODO_CONTINUO
    /// </summary>
    public interface ICpNodoContinuoRepository
    {        
        void Update(CpNodoContinuoDTO entity);
        void Delete(int cpnodocodi);
        CpNodoContinuoDTO GetById(int cpnodocodi);
        List<CpNodoContinuoDTO> List();
        List<CpNodoContinuoDTO> ListaPorArbol(int arbolcodi);
        List<CpNodoContinuoDTO> GetByCriteria();
        int Save(CpNodoContinuoDTO nodo, IDbConnection connection, DbTransaction transaction);
        CpNodoContinuoDTO GetByNumero(int cparbcodi, int cpnodonumero);
    }
}
