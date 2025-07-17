using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla DAI_TABLACODIGO_DETALLE
    /// </summary>
    public interface IDaiTablacodigoDetalleRepository
    {
        int Save(DaiTablacodigoDetalleDTO entity);
        void Update(DaiTablacodigoDetalleDTO entity);
        void Delete(int tabdcodi);
        DaiTablacodigoDetalleDTO GetById(int tabdcodi);
        List<DaiTablacodigoDetalleDTO> List();
        List<DaiTablacodigoDetalleDTO> GetByCriteria();
    }
}
