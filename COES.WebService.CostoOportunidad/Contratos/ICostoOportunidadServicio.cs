using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.WebService.CostoOportunidad.Contratos
{
    [ServiceContract(Namespace = "www.coes.org.pe")]
    public interface ICostoOportunidadServicio
    {
        [OperationContract]
        void ImportarTodoSeñalesSP7(int tipoImportacion, DateTime? FechaDiario, int copercodi, int covercodi, string usuario, string tipo, out bool hayEjecucionEnCurso);
        [OperationContract]
        int ProcesarCalculo(int idVersion, DateTime fechaInicio, DateTime fechaFin, string usuario, int option);
        [OperationContract]
        int EjecutarReproceso(int idVersion, int indicador, DateTime fecInicio, DateTime fecFin,
            int indicadorDatos, string usuario, int option, int importarSP7);
        [OperationContract]
        void ReprocesarCalculoTodos(DateTime fechaIni, DateTime fechaFin, string usuario);
        [OperationContract]
        void CalculoFactoresUtilizacion();
        [OperationContract]
        int EjecutarProcesoDiario(DateTime fecha, string tipo, string usuario);
    }
}
