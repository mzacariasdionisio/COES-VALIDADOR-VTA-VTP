using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CP_NODO_DETALLE
    /// </summary>
    public interface ICpNodoDetalleRepository
    {        
        void Update(CpNodoDetalleDTO entity);
        void Delete(int cpndetcodi);
        CpNodoDetalleDTO GetById(int cpndetcodi);
        List<CpNodoDetalleDTO> List();
        List<CpNodoDetalleDTO> GetByCriteria();
        int Save(CpNodoDetalleDTO nodo, IDbConnection connection, DbTransaction transaction);
        List<CpNodoDetalleDTO> ListaPorNodo(int codigoNodo);
        List<CpNodoDetalleDTO> ListaPorArbol(int codigoArbol);
    }
}
