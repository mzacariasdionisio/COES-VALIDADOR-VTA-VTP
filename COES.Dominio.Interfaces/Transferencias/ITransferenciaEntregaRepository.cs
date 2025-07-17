using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla TRN_TRANS_ENTREGA
    /// </summary>
    public interface ITransferenciaEntregaRepository
    {
        int Save(TransferenciaEntregaDTO entity);
        void Update(TransferenciaEntregaDTO entity);
        void Delete(int pericodi, int version, string sCodigo);
        void DeleteListaTransferenciaEntrega(int pericodi, int version);
        TransferenciaEntregaDTO GetById(System.Int32 id);
        List<TransferenciaEntregaDTO> List(int emprcodi, int pericodi, int version, int barracodi);
        List<TransferenciaEntregaDTO> ListByPeriodoVersion(int iPericodi, int iVersion);
        TransferenciaEntregaDTO GetTransferenciaEntregaByCodigo(int iEmprCodi, int iPeriCodi, int iTEntVersion, string sCodigo);
        void BulkInsert(List<TrnTransEntregaBullk> entitys);
        int GetCodigoGenerado();
        int GetCodigoGeneradoDec();
        void CopiarEntregas(int iPeriCodi, int iVersionOld, int iVersionNew, int iTransEntrCodi, int iTransEntrDetCodi, IDbConnection conn, DbTransaction tran);
        TransferenciaEntregaDTO GetTransferenciaEntregaByCodigoEnvio(int iEmprCodi, int iPeriCodi, int iTEntVersion, int trnenvcodi, string sCodigo);
        void UpdateTransferenciaEntregaEstadoINA(int pericodi, int recacodi, List<string> listaEntregas, int emprcodi, string suser);
    }
}
