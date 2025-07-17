using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Transferencias
{
    public interface ISolicitudCodigoRepository : IRepositoryBase
    {
        int Save(SolicitudCodigoDTO entity);
        void SaveSolicitudPeriodo(SolicitudCodigoDTO entity);
        void SaveSolicitudPeriodoVTP(SolicitudCodigoDTO entity);
        void Update(SolicitudCodigoDTO entity);
        void Delete(System.Int32 id);
        SolicitudCodigoDTO GetById(System.Int32 id);
        List<SolicitudCodigoDTO> List(string estado);
        List<SolicitudCodigoDTO> ListarCodigoRetiro(string nombreEmp, string tipousu, string tipocont, string bartran, string clinomb, DateTime? fechaIni, DateTime? fechaFin, string Solicodiretiobservacion, string estado);
        List<SolicitudCodigoDTO> ListarCodigoRetiroPaginado(string nombreEmp, string tipoUsu, string tipoCont, string barrTran, string cliNomb, DateTime? fechaIni, DateTime? fechaFin, string soliCodiRetiObservacion, string estado, int? pericodi, int NroPagina, int PageSize);

        List<SolicitudCodigoDTO> ListarExportacionCodigoRetiro(string nombreEmp, string tipoUsu, string tipoCont, string barrTran, string cliNomb, DateTime? fechaIni, DateTime? fechaFin, string soliCodiRetiObservacion, string estado, int? pericodi, int NroPagina, int PageSize);


        List<SolicitudCodigoDTO> GetByCriteria(string nombreEmp, string tipousu, string tipocont, string bartran, string clinomb, DateTime? fechaIni, DateTime? fechaFin, string Solicodiretiobservacion, string estado, string codretiro, int NroPagina, int PageSizeCodigoRetiro);
        SolicitudCodigoDTO GetByCodigoRetiCodigo(System.String sTRetCodigo);
        int ObtenerNroRegistros(string sEmprNomb, string sTipUsuNombre, string sTipConNombre, string sBarrBarraTransferencia, string sCliEmprNomb, DateTime? dCoReSoFechaInicio, DateTime? dCoReSoFechaFin, string Solicodiretiobservacion, string sCoReSotEstado, int? pericodi);
        SolicitudCodigoDTO GetCodigoRetiroByCodigo(string sCoReSoCodigo);

        int SolicitarBajar(SolicitudCodigoDTO entity);
        int UpdateObservacion(SolicitudCodigoDTO entity);
        SolicitudCodigoDTO CodigoRetiroVigenteByPeriodo(int iPericodi, string sCodigo);
        // ASSETEC 202001
        List<SolicitudCodigoDTO> ImportarPotenciasContratadas(int pericodi, int idEmpresa);
        void UpdateTipPotCodConsolidadoPeriodo(SolicitudCodigoDTO entity);
        void UpdateTipPotCodCodigoRetiro(SolicitudCodigoDTO entity);
    }
}
