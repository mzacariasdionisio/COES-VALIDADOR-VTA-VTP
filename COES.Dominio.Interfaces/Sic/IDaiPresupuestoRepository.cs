using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla DAI_PRESUPUESTO
    /// </summary>
    public interface IDaiPresupuestoRepository
    {
        int Save(DaiPresupuestoDTO entity);
        void Update(DaiPresupuestoDTO entity);
        void Delete(DaiPresupuestoDTO presupuesto);
        DaiPresupuestoDTO GetById(int prescodi);
        List<DaiPresupuestoDTO> List();
        List<DaiPresupuestoDTO> GetByCriteria();
    }
}
