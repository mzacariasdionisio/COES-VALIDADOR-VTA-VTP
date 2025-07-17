using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.Servicios.Distribuidos.Contratos
{
    [ServiceContract(Namespace = "www.coes.org.pe")]
    public interface IGeneracionRERServicio
    {
        [OperationContract]
        List<CodigoCargaRER> ObtenerCodigosDeCarga(string userLogin);

        [OperationContract]
        int CargarDatos(int horizonte, DateTime fecha, int anio, int nroSemana, List<Medicion48> valores, string userLogin);
    }
}
