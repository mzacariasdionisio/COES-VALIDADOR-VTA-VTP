using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CB_REPORTE_DETALLE
    /// </summary>
    public interface ICbReporteDetalleRepository
    {
        int getIdDisponible();
        void Save(CbReporteDetalleDTO entity, IDbConnection conn, IDbTransaction tran);
        int Save(CbReporteDetalleDTO entity);
        void Update(CbReporteDetalleDTO entity);
        void Delete(int cbrepdcodi);
        CbReporteDetalleDTO GetById(int cbrepdcodi);
        List<CbReporteDetalleDTO> List();
        List<CbReporteDetalleDTO> GetByCriteria();
        List<CbReporteDetalleDTO> GetByIdCentral(int cbrcencodi);
        
    }
}
