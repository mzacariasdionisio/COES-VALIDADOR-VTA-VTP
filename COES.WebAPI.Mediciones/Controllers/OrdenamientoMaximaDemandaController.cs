using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using COES.WebAPI.Mediciones.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace COES.WebAPI.Mediciones.Controllers
{
    /// <summary>
    /// Servicio de consulta de Ordenamiento Máxima Demanda, publicada en portal COES http://www.coes.org.pe/Portal/portalinformacion/demanda?indicador=maxima >>
    /// Ranking de Demanda de Potencia >> Ordenamiento MD
    /// </summary>
    public class OrdenamientoMaximaDemandaController: ApiController
    {
        RankingConsolidadoAppServicio service = new RankingConsolidadoAppServicio();
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(DemandaController));


        /// <summary>
        /// Método que retorna el ordenamiento de Máxima Demanda (Generación 15 minutos)
        /// </summary>
        /// <param name="fecha">Mes año Ejm. 05 2019</param>        
        /// <param name="empresas">si desea de todas las empresas no escriba nada, en caso contrario escriba el codigo de la(s) empresas</param>
        /// <param name="tiposGeneracion">Eólica = 4, Hidroelectrica = 1, Termoeléctrica = 2 , Solar = 3, Todos= 0</param>
        /// <param name="central">Todos = 0 , COES =1 , Generación RER = 3, No COES = 10</param>
        /// <returns>
        /// {
        ///     MaximaDemanda,
        ///     Total,
        ///     ValorInternacional,
        ///     FechaHora
        /// }
        /// </returns>
        [ResponseType(typeof(List<OrdenamientoMD>))]
        public IHttpActionResult GetObtenerDemandaDiariaHFPHP(string fecha, string empresas, string tiposGeneracion,
            int central)
        {
            if (tiposGeneracion == "0") tiposGeneracion = null;
            
            DateTime fechaProceso = EPDate.GetFechaIniPeriodo(5, fecha, string.Empty, string.Empty, string.Empty);
            DateTime fechaIni = fechaProceso;
            DateTime fechaFin = fechaProceso.AddMonths(1).AddDays(-1);
            if (string.IsNullOrEmpty(empresas)) empresas = ConstantesMedicion.IdEmpresaTodos.ToString();
            if (string.IsNullOrEmpty(tiposGeneracion)) tiposGeneracion = ConstantesMedicion.IdTipoGeneracionTodos.ToString();

            bool esPortal = false; //User.Identity.Name.Length == 0;
            int estadoValidacion = esPortal ? ConstanteValidacion.EstadoValidado : ConstanteValidacion.EstadoTodos;

            DemandadiaDTO resultado = new DemandadiaDTO();
            List<DemandadiaDTO> listOrdenado = new List<DemandadiaDTO>();
            string tipoEmpresa = "";
            List<DemandadiaDTO> lista = this.service.ObtenerDemandaDiariaHFPHP(fechaIni, fechaFin, tipoEmpresa, empresas,
                tiposGeneracion, central, out resultado, out listOrdenado, estadoValidacion, fechaProceso).OrderBy(x => x.Medifecha).ToList();

            try
            {
                return Ok(
                (from campos in listOrdenado
                 select new
                 {
                     MaximaDemanda = campos.Valor,
                     Total = campos.ValorGeneracion,
                     ValorInternacional = campos.ValorInter,
                     FechaHora= campos.StrMediFecha
                 })
                );
            }
            catch (Exception ex)
            {
                log.Error("GetObtenerEmpresaPorTipo", ex);
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Ocurrió un error al consultar información", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
                throw new HttpResponseException(response);
            }
        }




    }
}