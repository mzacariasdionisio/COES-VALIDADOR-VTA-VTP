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
    public interface IOperacionesVariasServicio
    {

        [OperationContract]
        List<EveIeodcuadro> ObtenerRegistros(int evenClase, int subCausacodi, string fechaIni, string fechaFin);

        [OperationContract]
        List<EveEvenclaseDTO> ObtenerHorizontes();

        [OperationContract]
        List<EveSubcausaeventoDTO> ObtenerTiposOperacion();
    }
}