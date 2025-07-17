using System;
using System.Collections.Generic;
using System.ServiceModel;
using COES.Dominio.DTO.Sic;

namespace COES.Servicios.Distribuidos.Contratos
{
    /// <summary>
    /// Interface con los contratos de los servicios
    /// </summary>
    [ServiceContract(Namespace = "www.coes.org.pe")]
    public interface IGeneralServicio
    {
        //[OperationContract]
        //List<EmpresaDTO> BuscarEmpresas(string nombre);

        //[OperationContract]
        //EmpresaDTO GetByIdEmpresa(int idEmpresa);


        [OperationContract]        
        List<SiPruebaDTO> BuscarPorNombre(string nombre);
        [OperationContract]     
        List<DocDiaEspDTO> ListDocDiaEsps();
        [OperationContract]     
        bool EsFeriado(DateTime adt_fecha);

        [OperationContract]
        CpFuentegamsDTO GetByIdCpFuentesgams(int indgsm);

        [OperationContract]
        int EnviarNotificacionTramiteVirtual(int idExpediente, int tipo);
    }
}
