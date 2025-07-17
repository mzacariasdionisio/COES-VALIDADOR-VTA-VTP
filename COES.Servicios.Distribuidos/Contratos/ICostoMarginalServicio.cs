using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Transferencias;

namespace COES.Servicios.Distribuidos.Contratos
{
    [ServiceContract(Namespace = "www.coes.org.pe")]
    public interface ICostoMarginalServicio
    {
        [OperationContract]  
        List<CostoMarginalDTO> ListarCostosMarginales(int anio, int mes);
    }
}
