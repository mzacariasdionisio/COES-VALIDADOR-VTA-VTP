using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace COES.Servicios.Distribuidos.Contratos
{
    /// <summary>
    /// Interface con los contratos de los servicios
    /// </summary>
    [ServiceContract(Namespace = "www.coes.org.pe")]
    public interface IProcesoServicio
    {
        [OperationContract]
        void EjecutarProcesoTarea();

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
        List<COES.Dominio.DTO.Sic.EveInformefallaDTO> ObtenerAlertaInformeFallas();

        [OperationContract]
        int EjecutarReprocesoMasivo(DateTime fechaInicio, DateTime fechaFin, List<string> horas, bool flagMD, string usuario, string tipoEstimador, int version);

        [OperationContract]
        int EjecutarPotenciaRemunerable(int pfpericodi, int pfrecacodi, int indrecacodiant, int recpotcodi, string usuario);

        [OperationContract]
        int EjecutarReprocesoMasivoModificado(string[][] datos, string usuario, int vesion);

        [OperationContract]
        int EjecutarReprocesoTIE(string[][] datos, string usuario, int barra, DateTime fechaProceso, int version);


        [OperationContract]
        int EjecutarReprocesoVA(string horas, string usuario, DateTime fechaProceso, int version);
    }
}