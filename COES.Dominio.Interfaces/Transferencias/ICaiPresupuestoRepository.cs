using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CAI_PRESUPUESTO
    /// </summary>
    public interface ICaiPresupuestoRepository
    {
        int Save(CaiPresupuestoDTO entity);
        void Update(CaiPresupuestoDTO entity);
        void Delete(int caiprscodi);
        CaiPresupuestoDTO GetById(int caiprscodi);
        List<CaiPresupuestoDTO> List();
        List<CaiPresupuestoDTO> GetByCriteria();
    }
}
