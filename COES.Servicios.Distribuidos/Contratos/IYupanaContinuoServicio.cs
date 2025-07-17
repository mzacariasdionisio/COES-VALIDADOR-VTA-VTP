using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace COES.Servicios.Distribuidos.Contratos
{
    /// <summary>
    /// Interface con los contratos de los servicios
    /// </summary>
    [ServiceContract(Namespace = "www.coes.org.pe")]
    public interface IYupanaContinuoServicio
    {
        [OperationContract]
        void EjecutarYupanaContinuoAutomatico(string fecha, int hora);

        [OperationContract]
        void EjecutarYupanaContinuoManual(string fecha, int hora, string usuario);

        [OperationContract]
        void VerificarEstadoYupanaContinuo();

        [OperationContract]
        void TerminarEjecucionGams();

    }
}