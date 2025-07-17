using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CM_REGIONSEGURIDAD_DETALLE
    /// </summary>
    public interface ICmRegionseguridadDetalleRepository
    {
        int Save(CmRegionseguridadDetalleDTO entity);
        void Update(CmRegionseguridadDetalleDTO entity);
        void Delete(int idRegion, int idEquipo);
        CmRegionseguridadDetalleDTO GetById(int regdetcodi);
        List<CmRegionseguridadDetalleDTO> List(int idRegion, int idEquipo);
        List<CmRegionseguridadDetalleDTO> GetByCriteria(int idRegion);
        List<CmRegionseguridadDetalleDTO> ObtenerEquipos(int tipo);
        List<CmRegionseguridadDetalleDTO> ObtenerEquiposCentral();
        List<CmRegionseguridadDetalleDTO> ObtenerModoOperacion();
        List<CmRegionseguridadDetalleDTO> ObtenerEquiposLinea(int tipo);
    }
}
