using COES.Dominio.DTO.Scada;
using COES.Servicios.Distribuidos.Servicios;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Distribuidos.Contratos
{
    [ServiceContract(Namespace = "www.coes.org.pe")]
    public interface IDatosHistoricoSP7Servicio
    {
        [OperationContract]
        List<DatosSP7DTO> ObtenerDatosSP7(DateTime FechaInicio, DateTime FechaFin, string Path, string TipoTabla);

        [OperationContract]
        List<DatosSP7DTO> ObtenerDatosHistoricos(DateTime FechaInicio, DateTime FechaFin, string Canalcodi, string TipoTabla);

        [OperationContract]
        List<DatosSP7DTO> ObtenerDatosHistoricosDatoMinuto(DateTime FechaInicio, DateTime FechaFin, string Canalcodi, string TipoTabla);

        [OperationContract]
        List<DatosSP7DTO> ObtenerDatosHistoricosEvaluacionRPF(DateTime FechaInicio, DateTime FechaFin, string Canalcodi, string TipoTabla);

        [OperationContract]
        List<DatosSP7DTO> ObtenerDatosHistoricosInformeSemanal(DateTime FechaInicio, DateTime FechaFin, string Canalcodi, string TipoTabla);

        [OperationContract]
        List<DatosSP7DTO> ObtenerDatosHistoricosPath(DateTime FechaInicio, DateTime FechaFin, string Path, string TipoTabla);

        [OperationContract]
        List<DatosSP7DTO> ObtenerDatosHistoricosPathMedicion(DateTime FechaInicio, DateTime FechaFin, string Path, string TipoTabla);

        [OperationContract]
        List<DatosSP7DTO> ObtenerDatosHistoricosPathFrecuencia(DateTime FechaInicio, DateTime FechaFin, string Path, string TipoTabla);

        [OperationContract]
        List<DatosSP7DTO> ObtenerDatosHistoricos15Min(DateTime FechaInicio, DateTime FechaFin, bool IncluirDigital);
    }
}