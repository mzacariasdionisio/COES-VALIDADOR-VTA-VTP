using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla AUD_TABLACODIGO_DETALLE
    /// </summary>
    public interface IAudTablacodigoDetalleRepository
    {
        int Save(AudTablacodigoDetalleDTO entity);
        void Update(AudTablacodigoDetalleDTO entity);
        void Delete(int tabcdcodi);
        AudTablacodigoDetalleDTO GetById(int tabcdcodi);
        AudTablacodigoDetalleDTO GetByDescripcion(string descripcion);
        List<AudTablacodigoDetalleDTO> List();
        List<AudTablacodigoDetalleDTO> GetByCriteria(int tabccodi);

    }
}
