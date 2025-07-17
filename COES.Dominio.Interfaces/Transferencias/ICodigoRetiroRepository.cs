using COES.Base.Core;
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
    /// Interface de acceso a datos de la tabla TRN_CODIGO_RETIRO_SOLICITUD
    /// </summary>
    public interface ICodigoRetiroRepository : IRepositoryBase
    {

        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);

        int Save(CodigoRetiroDTO entity);
        void Update(CodigoRetiroDTO entity);
        void UpdateEstadoAprobacion(CodigoRetiroDTO entity);
        void UpdateEstadoRechazar(CodigoRetiroDTO entity);
        void UpdateBajaCodigoVTEA(CodigoRetiroDTO entity, IDbConnection conn, DbTransaction tran);
        void UpdateBajaCodigoVTEAVTP(int iCoReSoCodi, string sCoesUserName);
        void UpdateVariacion(CodigoRetiroDTO entity, IDbConnection conn, DbTransaction tran);
        void Delete(System.Int32 id);
        CodigoRetiroDTO GetById(System.Int32 id);
        List<CodigoRetiroDTO> List(string estado);
        List<CodigoRetiroDTO> ListarGestionCodigosVTEAVTP(int? genEmprCodi, int? cliCodi, int? tipoCont, int? tipoUsu, int? barrCodi, int? barrCodiSum, string coresoEstado, string coresoestapr, string coregeCodVteaVtp, DateTime? fechaIni, DateTime? fechaFin, int periCodi, int nroPagina, int pageSize);
        List<CodigoRetiroDTO> ListarGestionCodigosVTEAVTPAprobar(int? genEmprCodi, int? cliCodi, int? tipoCont, int? tipoUsu, int? barrCodi, int? barrCodiSum, string coresoEstado, string coresoestapr, string coregeCodVteaVtp, DateTime? fechaIni, DateTime? fechaFin, int periCodi, int nroPagina, int pageSize);
        List<CodigoRetiroDTO> ListarGestionCodigosExportarVTEAVTP(int? genEmprCodi, int? cliCodi, int? tipoCont, int? tipoUsu, int? barrCodi, int? barrCodiSum, string coresoEstado, string coregeCodVteaVtp, DateTime? fechaIni, DateTime? fechaFin, int periCodi, int nroPagina, int pageSize);
        List<CodigoRetiroDTO> ListarCodigoVTEAByEmprBarr(int? genemprcodi, int? cliemprcodi, int? barrcodi);

        List<CodigoRetiroDTO> GetByCriteriaExtranet(string nombreEmp, string tipousu, string tipocont, string bartran, string clinomb, DateTime? fechaIni, DateTime? fechaFin, string Solicodiretiobservacion, string estado);
        List<CodigoRetiroDTO> GetByCriteria(string nombreEmp, string tipousu, string tipocont, string bartran, string clinomb, DateTime? fechaIni, DateTime? fechaFin, string Solicodiretiobservacion, string estado, string codretiro, int NroPagina, int PageSizeCodigoRetiro);
        CodigoRetiroDTO GetByCodigoRetiCodigo(System.String sTRetCodigo);
        CodigoRetiroDTO GetByIdGestionCodigosVTEAVTP(System.Int32 id, Int32 pericodi);
        int ObtenerNroRegistros(string sEmprNomb, string sTipUsuNombre, string sTipConNombre, string sBarrBarraTransferencia, string sCliEmprNomb, DateTime? dCoReSoFechaInicio, DateTime? dCoReSoFechaFin, string Solicodiretiobservacion, string sCoReSotEstado, string codretiro);
        int ObtenerNroRegistrosGestionCodigosVTEAVTP(int periCodi, int? genEmprCodi, int? cliCodi, int? tipoCont, int? tipoUsu, int? barrCodi, int? barrCodiSum, string coresoEstado, string coregeCodVteaVtp, DateTime? fechaIni, DateTime? fechaFin);


        CodigoRetiroDTO GetCodigoRetiroByCodigo(string sCoReSoCodigo);
        CodigoRetiroDTO CodigoRetiroVigenteByPeriodo(int iPericodi, string sCodigo);
        // ASSETEC 202001
        List<CodigoRetiroDTO> ImportarPotenciasContratadas(int pericodi, int idEmpresa);
        bool ValidarExisteCodigoEnEnvios(string sTRetCodigo);
        List<CodigoRetiroDTO> ListCodRetirosByEmpresaYFecha(int genemprcodi, string coresofechainicio, string coresofechafin); // PrimasRER.2023
    }
}
