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
    public interface IHidrologiaServicio
    {

        [OperationContract]
        List<MeMedicion24> ObtenerData(string idsEmpresa, string fecha, string fechaFinal, string idsTipoPtoMed);
        [OperationContract] 
        List<SiEmpresaDTO> listadoEmpresas();
        [OperationContract]
        List<SiTipoinformacionDTO> listadoUnidades();
    }
}