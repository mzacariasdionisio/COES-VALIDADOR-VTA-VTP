using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using COES.Servicios.Distribuidos.Contratos;
using COES.Dominio.DTO.Sic;
using log4net;
using COES.Servicios.Aplicacion.General;
using System.ServiceModel.Web;
using System.Net;
using COES.Servicios.Distribuidos.Resultados;
using COES.Servicios.Aplicacion.FormatoMedicion;

namespace COES.Servicios.Distribuidos.Servicios
{
    /// <summary>
    /// Servicio que concentra los métodos de consulta de empresas
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerSession)]
    public class SicEmpresaServicio : ISicEmpresaServicio
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(SicEmpresaServicio));
        /// <summary>
        /// Método Constructor
        /// </summary>
        public SicEmpresaServicio() {
            log4net.Config.XmlConfigurator.Configure();
        }
        /// <summary>
        /// Operación que permite consultar los datos de empresa según el ruc
        /// </summary>
        /// <param name="ruc">RUC de empresa</param>
        /// <returns>Objeto empresa</returns>
        public SiEmpresaDTO ConsultaEmpresa(string ruc)
        {
            SiEmpresaDTO empresaResult = new SiEmpresaDTO();
            //try
            //{
                Decimal dRuc = 0;
                if (ruc.Trim().Length != 11)//Ruc con mas o menos dígitos
                {
                    //FaultCode faulCode = new FaultCode("1");
                    //FaultReason faultReason = new FaultReason("RUC no tiene 11 dígitos.");
                    //throw new FaultException(faultReason, faulCode);

                    FaultData fault = new FaultData();
                    fault.FaulCode = "1";
                    fault.FaultString = "RUC no tiene 11 dígitos.";
                    FaultReason fr = new FaultReason("RUC no tiene 11 dígitos.");
                    FaultCode fc = new FaultCode("1");
                    throw new FaultException<FaultData>(fault, fr,fc);
                }
                if (!Decimal.TryParse(ruc, out dRuc))
                { // RUC no letras
                    //FaultCode faulCode = new FaultCode("2");
                    //FaultReason faultReason = new FaultReason("RUC tiene caracteres inválidos.");
                    FaultData fault = new FaultData();
                    fault.FaulCode = "2";
                    fault.FaultString = "RUC tiene caracteres inválidos.";
                    FaultReason fr = new FaultReason("RUC tiene caracteres inválidos.");
                    FaultCode fc = new FaultCode("2");
                    throw new FaultException<FaultData>(fault, fr,fc);
                }

                EmpresaAppServicio empresaServicio = new EmpresaAppServicio();
                var resultadoEmpresa = empresaServicio.BuscarEmpresas(string.Empty, ruc, -2, "-1", "A", 1, int.MaxValue);
                if (resultadoEmpresa != null && resultadoEmpresa.Count > 0)
                {
                    empresaResult = resultadoEmpresa.FirstOrDefault();
                }
                return empresaResult;
            //}
            //catch (Exception ex)
            //{
            //    //    log.Error("ConsultaEmpresa", ex);
            //    //    //OutgoingWebResponseContext response = WebOperationContext.Current.OutgoingResponse;
            //    //    //response.StatusCode = HttpStatusCode.InternalServerError;
            //    //    //response.StatusDescription = "Error en operación ConsultaEmpresa";
            //    return null;
            //}
        }

        /// <summary>
        /// Método que devuelve el listado de Empresas por formato
        /// </summary>
        /// <param name="formatcodi">Código de Formato</param>
        /// <returns></returns>
        List<SiEmpresaDTO> ISicEmpresaServicio.GetListaEmpresaFormato(int formatcodi)
        {
            List<SiEmpresaDTO> lsEmrpesas = new List<SiEmpresaDTO>();
            if (formatcodi < 1)
            {
                throw new FaultException(new FaultReason("El código de formato no es válido"), new FaultCode("1"));
            }

            try
            {
                FormatoMedicionAppServicio servFormato = new FormatoMedicionAppServicio();
                lsEmrpesas = servFormato.GetListaEmpresaFormato(formatcodi);
            }
            catch (Exception ex)
            {
                log.Error("GetListaEmpresaFormato", ex);
                throw new FaultException(new FaultReason("Ocurrió un error al consultar la lista de empresas."), new FaultCode("2"));
            }

            return lsEmrpesas;
        }

        /// <summary>
        /// Método que devuelve el listado de Empresas para Energía Primaria
        /// </summary>
        /// <param name="formatcodi">Código de Formato</param>
        /// <returns></returns>
        List<SiEmpresaDTO> ISicEmpresaServicio.GetListaEmpresaFormatoEnergiaPrimaria(int formatcodi)
        {
            List<SiEmpresaDTO> lsEmrpesas = new List<SiEmpresaDTO>();
            if (formatcodi < 1)
            {
                throw new FaultException(new FaultReason("El código de formato no es válido"), new FaultCode("1"));
            }

            try
            {
                FormatoMedicionAppServicio servFormato = new FormatoMedicionAppServicio();
                lsEmrpesas = servFormato.GetListaEmpresaFormatoEnergiaPrimaria(formatcodi);
            }
            catch (Exception ex)
            {
                log.Error("GetListaEmpresaFormatoEnergiaPrimaria", ex);
                throw new FaultException(new FaultReason("Ocurrió un error al consultar la lista de empresas."), new FaultCode("2"));
            }
            return lsEmrpesas;
        }
    }
}