using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.WebService.Compensacion.Contratos
{
    [ServiceContract(Namespace = "www.coes.org.pe")]
    public interface ICompensacionServicio
    {
        [OperationContract]
        List<ModoOperacionParametrosDTO> ListarModosOperacionParametros(int iGrupoCodi);
        [OperationContract]
        List<ModoOperacionCostosDTO> ListarModosOperacionCostos();
    }
}
