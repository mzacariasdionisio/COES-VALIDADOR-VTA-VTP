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
    public interface IDemandaServicio
    {
        [OperationContract]
        List<MeMedicion48DTO> ObtenerDatosDemanda(DateTime fechaInicio, DateTime fechaFin, int lectoCodi);

        [OperationContract]
        Dominio.DTO.Sic.SupDemandaGrafico ObtenerDatosGrafico(int tipo);
    }
}
