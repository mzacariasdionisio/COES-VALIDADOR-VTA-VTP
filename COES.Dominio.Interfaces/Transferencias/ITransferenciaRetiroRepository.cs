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
    /// Interface de acceso a datos de la tabla TRN_TRANS_RETIRO_DETALLE
    /// </summary>
    public interface ITransferenciaRetiroRepository
    {
        int Save(TransferenciaRetiroDTO entity);
        void Update(TransferenciaRetiroDTO entity);
        void Delete(int pericodi, int version, string sCodigo);
        void DeleteListaTransferenciaRetiro(int iPeriCodi, int iVersion);
        void DeleteListaTransferenciaRetiroEmpresa(int iPeriCodi, int iVersion, int iEmprCodi);
        TransferenciaRetiroDTO GetById(System.Int32 id);
        List<TransferenciaRetiroDTO> List(int emprcodi, int pericodi, int version, int barracodi);
        List<TransferenciaRetiroDTO> ListByPeriodoVersion(int iPericodi, int iVersion);
        List<TransferenciaRetiroDTO> ListByPeriodoVersionEmpresa(int iPericodi, int iVersion, int iEmprCodi);
        TransferenciaRetiroDTO GetTransferenciaRetiroByCodigo(int iEmprCodi, int iPeriCodi, int iTEntVersion, string sCodigo);
        void BulkInsert(List<TrnTransRetiroBullk> entitys);
        int GetCodigoGenerado();

        int GetCodigoGeneradoDesc();
        void CopiarRetiros(int iPeriCodi, int iVersionOld, int iVersionNew, int iTranRetiCodi, int iTranRetiDetaCodi, IDbConnection conn, DbTransaction tran);
        TransferenciaRetiroDTO GetTransferenciaRetiroByCodigoEnvio(int iEmprCodi, int iPeriCodi, int iTEntVersion, int trnenvcodi, string sCodigo);
        void UpdateTransferenciaRetiroEstadoINA(int pericodi, int recacodi, List<string> listaRetiros, int genemprcodi, string suser);
        List<TransferenciaRetiroDTO> GetRetiroBy(int pericodi, int version, int genemprcodi);
    }

}
