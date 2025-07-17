using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VTD_VALORIZACION
    /// </summary>
    public interface IVtdValorizacionRepository
    {
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);
        int Save(VtdValorizacionDTO entity);
        int Save(VtdValorizacionDTO entity, IDbConnection conn, DbTransaction tran);
        bool Update(VtdValorizacionDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(VtdValorizacionDTO entity);
        void UpdateEstado(VtdValorizacionDTO entity, IDbConnection conn, DbTransaction tran, String emprcodi);
        void Delete(int Valocodi);
        VtdValorizacionDTO GetById(int Valocodi);
        List<VtdValorizacionDTO> List();
        List<VtdValorizacionDTO> GetByCriteria();
        VtdValorizacionDTO ObtenerPrecioPotencia(int pericodi);

        List<SiEmpresaDTO> ObtenerEmpresas();
    }
}
