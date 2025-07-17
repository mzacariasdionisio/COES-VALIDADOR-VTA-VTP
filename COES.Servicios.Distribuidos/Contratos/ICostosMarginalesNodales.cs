using COES.Dominio.DTO.Sic;
using COES.Servicios.Distribuidos.Models;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace COES.Servicios.Distribuidos.Contratos
{
    /// <summary>
    /// Interface con los contratos de los servicios
    /// </summary>
    [ServiceContract(Namespace = "www.coes.org.pe")]
    public interface ICostosMarginalesNodales
    {

        [OperationContract]
        CostoMarginalNodalModel ObtenerRegistros(int correlativo);

        [OperationContract]
        CostoMarginalNodalModel ObetenerListadoEjecuciones(string fecha);

        [OperationContract]
        List<CpTopologiaDTO> ObtenerEscenariosPorDia(string fecha);
    }
}