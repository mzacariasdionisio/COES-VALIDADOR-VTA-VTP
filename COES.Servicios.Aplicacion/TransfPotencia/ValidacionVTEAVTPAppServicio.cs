
using COES.Dominio.DTO.ValidacionVTEAVTP;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.Helper;
using DocumentFormat.OpenXml.ExtendedProperties;
using log4net;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.TransfPotencia.Helper
{
    /// <summary>
    /// Clases Validación Vtea - Vtp
    /// </summary>
    public class ValidacionVteavtpAppServicio
    {

        readonly string urlSmeBD;
        readonly string urlSmeFunction;
        const string HttpMethodTrnperiodo = "trnperiodo";
        const string HttpMethodVtpVersions = "vtp_versions";
        const string HttpMethodVteaVersions = "vtea_versions";
        const string HttpMethodVtpValidacion = "vtp_validation";
        const string HttpMethodVtp = "vtp";
        const string HttpMethodVtpVtea = "vtp_vtea";

        const string HttpMethodVteaValidation = "vtea_validation";
        const string HttpMethodVtea = "vtea";


        const string HttpMethodVteaHistRol = "vtea_hist_rol"; //
        const string HttpMethodVteaDcUnit = "vtea_dc_unit";//
        const string HttpMethodVteaDetail = "vtea_detail";//

        /// <summary>
        /// Constructor Validación Vtea - Vtp
        /// </summary>
        public ValidacionVteavtpAppServicio(){
            urlSmeBD = ConfigurationManager.AppSettings["SmeApiRestBD"];
            urlSmeFunction = ConfigurationManager.AppSettings["SmeApiRestFunction"];
        }


        private static readonly ILog Logger = LogManager.GetLogger(typeof(ValidacionVteavtpAppServicio));

        /// <summary>
        /// Obtiene datos del servicio Trnperiodo
        /// </summary>
        public async Task<TrnPeriodoDTO> ObtenerSmeTrnPeriodo()
        {
            TrnPeriodoDTO trnPeriodoDTO = new TrnPeriodoDTO();
            try
            {
                string urlMetodo = string.Format("{0}/{1}", urlSmeBD, HttpMethodTrnperiodo);
                var response =await HttpServiceHelper.SendAsync(HttpMethod.Get, urlMetodo);
                trnPeriodoDTO = JsonConvert.DeserializeObject<TrnPeriodoDTO>(response);
                return trnPeriodoDTO;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                trnPeriodoDTO.Resultado = -1;
                trnPeriodoDTO.Mensaje = ex.Message.ToString();
                return trnPeriodoDTO;
            }
        }

        /// <summary>
        /// Obtiene datos del servicio Vtp Versions
        /// </summary>
        public async Task<VtpVersionDTO> ObtenerSmeVtpVersions(string perinombre, string recpotnombre)
        {
            VtpVersionDTO vtpVersionDTO = new VtpVersionDTO();  
            try
            {
                string urlMetodo = $"{urlSmeBD}/{HttpMethodVtpVersions}?perinombre={perinombre}&recpotnombre={recpotnombre}" ;

                string json = await HttpServiceHelper.SendAsync(HttpMethod.Get, urlMetodo);
                vtpVersionDTO = JsonConvert.DeserializeObject<VtpVersionDTO>(json);
                return vtpVersionDTO;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                vtpVersionDTO.Resultado = -1;
                vtpVersionDTO.Mensaje = ex.Message.ToString();
                return vtpVersionDTO;
            }
        }

        /// <summary>
        /// Obtiene datos del servicio Vtea Versions
        /// </summary>
        public async Task<VteaVersionDTO> ObtenerSmeVteaVersions(string perinombre, string recpotnombre)
        {
            VteaVersionDTO vteaVersionDTO = new VteaVersionDTO();
            try
            {
                string urlMetodo = $"{urlSmeBD}/{HttpMethodVteaVersions}?perinombre={perinombre}&recpotnombre={recpotnombre}";
                string json = await HttpServiceHelper.SendAsync(HttpMethod.Get, urlMetodo);
                vteaVersionDTO = JsonConvert.DeserializeObject<VteaVersionDTO>(json);


                return vteaVersionDTO;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                vteaVersionDTO.Resultado = -1;
                vteaVersionDTO.Mensaje = ex.Message.ToString();
                return vteaVersionDTO;
            }
        }

        /// <summary>
        /// Obtiene datos del servicio Vtp Validar
        /// </summary>
        public async Task<VtpValidacionDTO> FuncionVtpValidar(string perinombre, string recpotnombre)
        {
            VtpValidacionDTO vtpValidacionDTO = new VtpValidacionDTO();
            try
            {
                string urlMetodo = $"{urlSmeFunction}/{HttpMethodVtpValidacion}?perinombre={perinombre}&recpotnombre={recpotnombre}";
                string json = await HttpServiceHelper.SendAsync(HttpMethod.Get, urlMetodo);
                return JsonConvert.DeserializeObject<VtpValidacionDTO>(json);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                vtpValidacionDTO.Resultado = -1;
                vtpValidacionDTO.Mensaje = ex.Message.ToString();
                return vtpValidacionDTO;
            }
        }

        /// <summary>
        /// Obtiene datos del servicio FuncionVtea
        /// </summary>
        public async Task<VteaDTO> FuncionVtea(string perinombre, string recanombre)
        {
            VteaDTO vteaDTO = new VteaDTO();    
            try
            {
                string urlMetodo = $"{urlSmeFunction}/{HttpMethodVtea}/?perinombre={perinombre}&recanombre={recanombre}";

                string json = await HttpServiceHelper.SendAsync(HttpMethod.Get, urlMetodo);

                return JsonConvert.DeserializeObject<VteaDTO>(json);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                vteaDTO.Resultado = -1;
                vteaDTO.Mensaje = ex.Message.ToString();
                return vteaDTO;

            }
        }


        /// <summary>
        /// Obtiene datos del servicio FuncionVteaValidador
        /// </summary>
        public async Task<VteaValidadorDTO> FuncionVteaValidador(string perinombre, string recpotnombre)
        {
            VteaValidadorDTO vteaValidadorDTO = new VteaValidadorDTO(); 
            try
            {
                string urlMetodo = $"{urlSmeFunction}/{HttpMethodVteaValidation}?perinombre={perinombre}&recanombre={recpotnombre}";
                string json = await HttpServiceHelper.SendAsync(HttpMethod.Get, urlMetodo);
                return JsonConvert.DeserializeObject<VteaValidadorDTO>(json);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                vteaValidadorDTO.Resultado = -1;
                vteaValidadorDTO.Mensaje =ex.Message.ToString();
                return vteaValidadorDTO;

            }
        }

        /// <summary>
        /// Obtiene datos del servicio funcion/vtp_vtp
        /// </summary>
        public async Task<VtpDTO> FuncionVtp(string perinombre, string recpotnombre)
        {
            VtpDTO vtpDTO = new VtpDTO();
            try
            {
                string urlMetodo = $"{urlSmeFunction}/{HttpMethodVtp}?perinombre={perinombre}&recpotnombre={recpotnombre}";
                string json = await HttpServiceHelper.SendAsync(HttpMethod.Get, urlMetodo);
                return JsonConvert.DeserializeObject<VtpDTO>(json);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                vtpDTO.Resultado = -1;
                vtpDTO.Mensaje = ex.Message.ToString();
                return vtpDTO;
            }
        }

        /// <summary>
        /// Obtiene datos del servicio funcion/vtp_vtea
        /// </summary>
        public async Task<VtpVteaDTO> FuncionVtpVtea(string perinombre, string recanombre, string recpotnombre)
        {
            VtpVteaDTO vtpVteaDTO = new VtpVteaDTO();
            try
            {
                string urlMetodo = $"{urlSmeFunction}/{HttpMethodVtpVtea}/?perinombre={perinombre}&recpotnombre={recpotnombre}&recanombre={recanombre}";

                string json = await HttpServiceHelper.SendAsync(HttpMethod.Get,urlMetodo);
                return JsonConvert.DeserializeObject<VtpVteaDTO>(json);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                vtpVteaDTO.Resultado = -1;
                vtpVteaDTO.Mensaje = ex.Message.ToString();
                return vtpVteaDTO;
            }
        }

        /// <summary>
        /// Obtiene datos del servicio funcion/vtea_hist_rol
        /// </summary>
        public async Task<VteaHistRolDTO> FuncionVteaHistRol(string emprnom, string perinombre)
        {
            VteaHistRolDTO vteaHistRolDTO = new VteaHistRolDTO();
            try
            {
                string urlMetodo = $"{urlSmeFunction}/{HttpMethodVteaHistRol}/?emprnom={emprnom}&perinombre={perinombre}";

                string json = await HttpServiceHelper.SendAsync(HttpMethod.Get, urlMetodo);

                return JsonConvert.DeserializeObject<VteaHistRolDTO>(json);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                vteaHistRolDTO.Resultado = -1;
                vteaHistRolDTO.Mensaje = ex.Message.ToString();
                return vteaHistRolDTO;
            }
        }

        /// <summary>
        /// Obtiene datos del servicio sme/vtea_dc_unit
        /// </summary>
        public async Task<VteaDcUnitDTO> SmeVteaDcUnit(string bus, string client, string code, string company, string pericodi, string tentversion)
        {
            VteaDcUnitDTO vteaDcUnitDTO = new VteaDcUnitDTO();
            try
            {
                string urlMetodo = $"{urlSmeBD}/{HttpMethodVteaDcUnit}/?bus={bus}&client={client}&code={code}&company={company}&pericodi={pericodi}&tentversion={tentversion}";

                string json = await HttpServiceHelper.SendAsync(HttpMethod.Get, urlMetodo);

                return JsonConvert.DeserializeObject<VteaDcUnitDTO>(json);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                vteaDcUnitDTO.Resultado = -1;
                vteaDcUnitDTO.Mensaje = ex.Message.ToString();
                return vteaDcUnitDTO;
            }
        }

        /// <summary>
        /// Obtiene datos del servicio function/vtea_details
        /// </summary>
        public async Task<VteaDetailDTO> FuntionVteaDetail(string bus, string client, string code, string company,string day, string perinombre, string recanombre)
        {
            VteaDetailDTO vteaDetailDTO = new VteaDetailDTO();
            try
            {
                string urlMetodo = $"{urlSmeFunction}/{HttpMethodVteaDetail}/?bus={bus}&client={client}&code={code}&company={company}&day={day}&perinombre={perinombre}&recanombre={recanombre}";
                
                string json = await HttpServiceHelper.SendAsync(HttpMethod.Get, urlMetodo);

                return JsonConvert.DeserializeObject<VteaDetailDTO>(json);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                vteaDetailDTO.Resultado = -1;
                vteaDetailDTO.Mensaje = ex.Message.ToString();
                return vteaDetailDTO;
            }
        }


    }
}
