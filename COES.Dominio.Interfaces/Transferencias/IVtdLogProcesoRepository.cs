using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VTD_LOGPROCESO
    /// </summary>
    public interface IVtdLogProcesoRepository
    {
        int Save(VtdLogProcesoDTO entity);
        void Update(VtdLogProcesoDTO entity);
        void Delete(int logpcodi);
        VtdLogProcesoDTO GetById(int logpcodi);
        List<VtdLogProcesoDTO> List();
        List<VtdLogProcesoDTO> GetByCriteria();
        List<VtdLogProcesoDTO> GetListByDate(DateTime date);
        List<VtdLogProcesoDTO> GetListPageByDate(DateTime date, int nroPage, int pageSize);
    }
}
