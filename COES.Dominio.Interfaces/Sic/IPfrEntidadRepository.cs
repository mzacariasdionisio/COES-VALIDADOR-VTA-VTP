using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PFR_ENTIDAD
    /// </summary>
    public interface IPfrEntidadRepository
    {
        int Save(PfrEntidadDTO entity);
        void Update(PfrEntidadDTO entity);
        void Delete(int pfrentcodi);
        PfrEntidadDTO GetById(int pfrentcodi);
        List<PfrEntidadDTO> List();
        List<PfrEntidadDTO> GetByCriteria(int pfrcatcodi, string pfrentcodi, int pfrentestado);
    }
}
