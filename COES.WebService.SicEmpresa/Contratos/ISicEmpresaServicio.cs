using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using COES.Dominio.DTO.Sic;

namespace COES.WebService.SicEmpresa.Contratos
{
    /// <summary>
    /// Servicio que concentra los métodos de consulta de empresas
    /// </summary>
    [ServiceContract(Namespace = "www.coes.org.pe")]
    public interface ISicEmpresaServicio
    {
        /// <summary>
        /// Operación que permite consultar los datos de empresa según el ruc
        /// </summary>
        /// <param name="ruc">RUC de empresa</param>
        /// <returns>Objeto empresa</returns>
        [OperationContract]
        //[FaultContract(typeof(FaultData))]
        SiEmpresaDTO ConsultaEmpresa(string ruc);

        /// <summary>
        /// Método que devuelve el listado de Empresas por formato
        /// </summary>
        /// <param name="formatcodi">Código de Formato</param>
        /// <returns></returns>
        [OperationContract]
        List<SiEmpresaDTO> GetListaEmpresaFormato(int formatcodi);

        /// <summary>
        /// Método que devuelve el listado de Empresas para Energía Primaria
        /// </summary>
        /// <param name="formatcodi">Código de Formato</param>
        /// <returns></returns>
        [OperationContract]
        List<SiEmpresaDTO> GetListaEmpresaFormatoEnergiaPrimaria(int formatcodi);
    }
}
