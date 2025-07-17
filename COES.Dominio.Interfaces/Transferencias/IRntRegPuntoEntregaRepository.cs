using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

using System.Data; //STS
using System.Data.Common; //STS


namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RNT_REG_PUNTO_ENTREGA
    /// </summary>
    public interface IRntRegPuntoEntregaRepository
    {
        int GetMaxId();
        int Save(RntRegPuntoEntregaDTO entity, int codenvio, IDbConnection conn, DbTransaction tran, int corrId);
        int Update(RntRegPuntoEntregaDTO entity);
        int UpdatePE(int empresaGeneradora, int periodoCodi, IDbConnection conn, DbTransaction tran);
        void Delete(int regpuntoentcodi);
        RntRegPuntoEntregaDTO GetById(int regpuntoentcodi);
        List<RntRegPuntoEntregaDTO> List(int? EmpresaGeneradora, int Periodo, int Cliente, int PEntrega);
        List<RntRegPuntoEntregaDTO> ListPaginado(int? EmpresaGeneradora, int Periodo, int Cliente, int PEntrega, int NroPaginado, int PageSize);
        List<RntRegPuntoEntregaDTO> ListAuditoriaPuntoEntrega(int Audittablacodi, int Tauditcodi);
        List<RntRegPuntoEntregaDTO> ListReporte(int? EmpresaGeneradora, int Periodo, int Cliente, int PEntrega);
        List<RntRegPuntoEntregaDTO> ListReporteCarga(int? EmpresaGeneradora, int Periodo, int PuntoEntrega, int CodigoEnvio);
        List<RntRegPuntoEntregaDTO> ListReporteGrilla(int codEnvio);
        RntRegPuntoEntregaDTO ListBarras(string barrnombre);
        List<RntRegPuntoEntregaDTO> ListAllBarras();
        List<RntRegPuntoEntregaDTO> ListReportePaginado(int? EmpresaGeneradora, int Periodo, int Cliente, int PEntrega, int NroPaginado, int PageSize);
        List<RntRegPuntoEntregaDTO> GetByCriteria(string user, int? EmpresaGeneradora, int Periodo, int Cliente, int PEntrega, int Ntension, DateTime desde);
        List<RntRegPuntoEntregaDTO> GetByCriteriaPaginado(string user, int? EmpresaGeneradora, int Periodo, int Cliente, int PEntrega, int Ntension, DateTime desde, int NroPaginado, int PageSize);
        List<RntRegPuntoEntregaDTO> ListAllPuntoEntrega();
        List<RntRegPuntoEntregaDTO> ListAllClientePE();
        List<RntRegPuntoEntregaDTO> ListChangeClientePE(int idCliente);

    }
}
