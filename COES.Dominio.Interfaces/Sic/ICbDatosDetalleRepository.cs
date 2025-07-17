using COES.Dominio.DTO.Sic;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CB_DATOS_DETALLE
    /// </summary>
    public interface ICbDatosDetalleRepository
    {
        int GetMaxId();
        int Save(CbDatosDetalleDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(CbDatosDetalleDTO entity);
        void Delete(int cbdetcodi);
        CbDatosDetalleDTO GetById(int cbdetcodi);
        List<CbDatosDetalleDTO> List();
        List<CbDatosDetalleDTO> GetByCriteria(int cbvercodi);
    }
}
