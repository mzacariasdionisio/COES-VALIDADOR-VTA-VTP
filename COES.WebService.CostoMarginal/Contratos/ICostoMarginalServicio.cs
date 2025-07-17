using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.CortoPlazo.Helper;

namespace COES.WebService.CostoMarginal.Contratos
{
    [ServiceContract(Namespace = "www.coes.org.pe")]
    public interface ICostoMarginalServicio
    {
        [OperationContract]  
        List<CostoMarginalDTO> ListarCostosMarginales(int anio, int mes);

        [OperationContract]
        void EjecutarCostosMarginalesAlterno(DateTime fecha, int indicadorPSSE, bool reproceso, bool indicadorNCP, bool flagWeb, string rutaNCP,
          bool flagMD, int idEscenario, string usuario, string tipoEstimador, int tipo, int version);

        [OperationContract]
        void EjecutarCostosMarginales(DateTime fecha, int indicadorPSSE, bool reproceso, bool indicadorNCP, bool flagWeb);

        [OperationContract]
        void ValidacionProcesoCostosMarginales(DateTime fecha);

        [OperationContract]
        COES.Servicios.Aplicacion.CortoPlazo.Helper.ResultadoValidacion ObtenerAlertasCostosMarginales(DateTime fechaProceso);

        [OperationContract]
        int EjecutarReprocesoMasivo(DateTime fechaInicio, DateTime fechaFin, List<string> horas, bool flagMD, string usuario, string tipoEstimador, int version);

        [OperationContract]
        int EjecutarReprocesoMasivoModificado(string[][] datos, string usuario, int vesion);

        [OperationContract]
        int EjecutarReprocesoTIE(string[][] datos, string usuario, int barra, DateTime fechaProceso, int version);

        [OperationContract]
        int EjecutarReprocesoVA(string horas, string usuario, DateTime fechaProceso, int version);

    }
}
