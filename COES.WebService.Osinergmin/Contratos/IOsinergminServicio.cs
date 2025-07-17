using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.WebService.Osinergmin.Contratos
{
    [ServiceContract(Namespace = "www.coes.org.pe")]
    public interface IOsinergminServicio
    {
        //[OperationContract]
        //List<CombustibleDTO> ListCombustibles();
        [OperationContract]
        List<EqEquipoDTO> ListadoCentralesOsinergmin();
        [OperationContract]
        List<GrupoGeneracionDTO> ListarGeneradoresDespachoOsinergmin();
        [OperationContract]
        List<SiFuenteenergiaDTO> ListarFuenteenergias();
        [OperationContract]
        List<PrGrupoDTO> ListaModosOperacionActivos();
    }
}

