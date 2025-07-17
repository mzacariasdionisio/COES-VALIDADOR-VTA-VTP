using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PFR_ENTIDAD_DAT
    /// </summary>
    public interface IPfrEntidadDatRepository
    {
        void Save(PfrEntidadDatDTO entity);
        void Update(PfrEntidadDatDTO entity);
        void Delete(int pfrentcodi, int pfrcnpcodi, DateTime prfdatfechavig, int pfrdatdeleted);
        PfrEntidadDatDTO GetById(int pfrentcodi, int pfrcnpcodi, DateTime prfdatfechavig, int pfrdatdeleted);
        List<PfrEntidadDatDTO> List();
        List<PfrEntidadDatDTO> ListarPfrentidadVigente(DateTime fechaVigencia, string pfrentcodis, int pfrcatcodi);
        List<PfrEntidadDatDTO> GetByCriteria(int pfrentcodi, int pfrcnpcodi);
    }
}
