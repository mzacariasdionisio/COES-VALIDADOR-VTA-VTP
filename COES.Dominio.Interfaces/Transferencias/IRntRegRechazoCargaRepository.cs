using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

using System.Data; //STS
using System.Data.Common; //STS


namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RNT_REG_RECHAZO_CARGA
    /// </summary>
    public interface IRntRegRechazoCargaRepository
    {
        int GetMaxId();
        int Save(RntRegRechazoCargaDTO entity, int codEnvio, IDbConnection conn, DbTransaction tran, int corrId);
        int Update(RntRegRechazoCargaDTO entity);
        int UpdateRC(int empresaGeneradora, int periodoCodi, IDbConnection conn, DbTransaction tran);
        int UpdateNRCF(RntRegRechazoCargaDTO entity);
        int UpdateEF(RntRegRechazoCargaDTO entity);
        void Delete(int regrechazocargacodi);
        RntRegRechazoCargaDTO GetById(int regrechazocargacodi);
        List<RntRegRechazoCargaDTO> List(int? EmpresaGeneradora, int Periodo, int Cliente, int PEntrega);
        List<RntRegRechazoCargaDTO> ListPaginado(int? EmpresaGeneradora, int Periodo, int Cliente, int PEntrega, int NroPaginado, int PageSize);
        List<RntRegRechazoCargaDTO> ListReporte(int? EmpresaGeneradora, int Periodo, int Cliente, int PEntrega);
        List<RntRegRechazoCargaDTO> ListReporteGrilla(int codigoEnvio);
        List<RntRegRechazoCargaDTO> ListReportePaginado(int? EmpresaGeneradora, int Periodo, int Cliente, int PEntrega, int NroPaginado, int PageSize);
        List<RntRegRechazoCargaDTO> ListAuditoriaRechazoCarga(int Audittablacodi, int Tauditcodi);
        List<RntRegRechazoCargaDTO> GetByCriteria(string user, int? EmpresaGeneradora, int Periodo, int Cliente, int PEntrega, int Ntension, DateTime desde);
        List<RntRegRechazoCargaDTO> GetByCriteriaPaginado(string user, int? EmpresaGeneradora, int Periodo, int Cliente, int PEntrega, int Ntension, DateTime desde, int NroPaginado, int PageSize);
        List<RntRegRechazoCargaDTO> ListAllRechazoCarga();
        List<RntRegRechazoCargaDTO> ListAllClienteRC();
        List<RntRegRechazoCargaDTO> ListChangeClienteRC(int idCliente);
    }
}
