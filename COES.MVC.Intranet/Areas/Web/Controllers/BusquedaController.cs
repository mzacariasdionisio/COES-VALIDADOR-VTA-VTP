using COES.Dominio.DTO.Busqueda;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.SeguridadServicio;
using COES.Servicios.Aplicacion.Busqueda;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Web.Controllers
{
    public class BusquedaController : BaseController
    {
        /// <summary>
        /// Usuario que inicia sesion
        /// </summary>
        public string UserName
        {
            get
            {
                return (Session[DatosSesion.SesionUsuario] != null) ?
                    ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin : string.Empty;
            }
        }

        /// <summary>
        /// Instancia de la clase WbBusquedaServicio
        /// </summary>
        readonly WbBusquedaServicio servicioBusqueda = new WbBusquedaServicio();

        /// <summary>
        /// Instancia de la clase WbRecomendacionServicio
        /// </summary>
        readonly WbRecomendacionServicio servicioRecomendacion = new WbRecomendacionServicio();

        /// <summary>
        /// Instancia de la clase AzSearchService
        /// </summary>
        readonly AzSearchService servicioSearch = new AzSearchService();

        /// <summary>
        /// Instancia de la clase AzSearchService
        /// </summary>
        readonly AzOAIServicio servicioOpenAI = new AzOAIServicio();

        /// <summary>
        /// Vista principal de búsqueda
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!string.IsNullOrEmpty(UserName))
            {
                if (Session["ChatHistory"] == null)
                {
                    Session["ChatHistory"] = new List<ChatHistoryDTO> {
                        new ChatHistoryDTO {
                            Role = "assistant",
                            Content = "Bienvenido, ¿en qué puedo ayudarte?"
                        }
                    };
                }
                ViewBag.TiposDocumento = new SelectList(servicioSearch.ObtenerTiposDocumento().Result);

                return View();
            }
            else
            {
                return RedirectToLogin();
            }
        }

        /// <summary>
        ///  Permite limpiar el chat de una búsqueda conversacional
        /// </summary>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        public JsonResult LimpiarChat()
        {
            Session["ChatHistory"] = null;  // Eliminar los mensajes previos
            Session["ChatHistory"] = new List<ChatHistoryDTO> {
                new ChatHistoryDTO {
                    Role = "assistant",
                    Content = "Bienvenido, ¿en qué puedo ayudarte?"
                }
            };

            RespuestaDTO<string> rta = new RespuestaDTO<string>(true, "Limpieza ejecutada.");
            return new JsonResult { Data = rta };
        }

        /// <summary>
        /// Muestra el listado de resultados de la búsqueda
        /// </summary>
        /// <param name="busqueda"></param>
        /// <returns>Vista tabular de resultados</returns>
        [System.Web.Mvc.HttpPost]
        public async Task<PartialViewResult> Listar(BCDBusquedasDTO busqueda)
        {
            string texto = busqueda.Search_text;
            List<WbBusquedasDTO> resultados = await servicioSearch.BuscarDocumentosAsync(busqueda, UserName);

            var idSearchTask = Task.Run(async () =>
            {
                busqueda.Search_text = texto;
                if (!string.IsNullOrEmpty(UserName))
                {
                    busqueda.Search_user = UserName;
                }
                int idCreada = servicioBusqueda.AlmacenarBusqueda(busqueda);
                return idCreada;
            });

            int idSearch = await idSearchTask;

            await Task.Run(() => servicioBusqueda.GuardarTopResultados(resultados, idSearch, UserName));

            ViewBag.idBusqueda = idSearch;
            return PartialView(resultados);
        }

        /// <summary>
        ///  Permite recomendar un resultado de búsqueda
        /// </summary>
        /// <param name="data"></param>
        /// <param name="idBusqueda"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [Route("recomendar")]
        public async Task<JsonResult> Recomendar([FromBody] WbBusquedasDTO data, int idBusqueda)
        {
            RespuestaDTO<string> rta = servicioRecomendacion.SaveRecomendacion(data, idBusqueda, UserName);
            return new JsonResult { Data = rta };
        }

        /// <summary>
        ///  Permite guardar la apertura de un documento
        /// </summary>
        /// <param name="registro"></param>
        /// <param name="idBusqueda"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [Route("GuardarDocumentoAbierto")]
        public async Task<JsonResult> GuardarDocumentoAbierto([FromBody] WbBusquedasDTO registro, int idBusqueda)
        {
            RespuestaDTO<string> rta = await servicioRecomendacion.GuardarDocumentoAbierto(registro, idBusqueda, UserName);
            return new JsonResult { Data = rta };
        }

        /// <summary>
        ///  Permite actualizar las palabras clave de un documento
        /// </summary>
        /// <param name="id"></param>
        /// <param name="palabrasClave"></param>
        /// <returns>Booleano que indica si guardó o no la actualización</returns>
        [System.Web.Mvc.HttpPost]
        public async Task<JsonResult> ActualizarPalabrasClave(string id, string palabrasClave)
        {
            RespuestaDTO<string> rta = await servicioSearch.ActualizarPalabrasClaveAsync(id, palabrasClave);
            if (rta.Success)
            {
                servicioSearch.RestablecerIndexadorTabla();
                rta.Message += "Restableciendo índice... ";
            }
            return new JsonResult { Data = rta };
        }

        /// <summary>
        ///  Permite calificar la utilidad global de los resultados encontrados
        /// </summary>
        /// <param name="idBusqueda"></param>
        /// <param name="calificacion"></param>
        /// <returns>Objeto que indica si guardó o no la utilidad</returns>
        [System.Web.Mvc.HttpPost]
        public async Task<JsonResult> Calificar([FromBody] string idBusqueda, [FromBody] string calificacion)
        {
            RespuestaDTO<string> rta = await servicioBusqueda.CalificarBusqueda(idBusqueda, calificacion);
            return new JsonResult { Data = rta };
        }

        /// <summary>
        ///  Permite establecer la relación entre los resultados encontrados
        /// </summary>
        /// <param name="idBusqueda"></param>
        /// <param name="seleccion"></param>
        /// <returns>Objeto que indica si guardó o no la relación</returns>
        [System.Web.Mvc.HttpPost]
        public async Task<JsonResult> GuardarSeleccionRelacionados([FromBody] string idBusqueda, [FromBody] bool seleccion)
        {
            RespuestaDTO<string> rta = await servicioBusqueda.GuardarSeleccionRelacionados(idBusqueda, seleccion);
            return new JsonResult { Data = rta };
        }

        /// <summary>
        ///  Permite sugerir texto
        /// </summary>
        /// <param name="highlights"></param>
        /// <param name="fuzzy"></param>
        /// <param name="term"></param>
        /// <param name="searchField"></param>
        /// <returns>Lista de sugerencias</returns>
        public async Task<JsonResult> Sugerir(bool highlights, bool fuzzy, string term, string searchField)
        {
            var sugerencias = await servicioSearch.Sugerir(highlights, fuzzy, term, searchField);
            return new JsonResult { Data = sugerencias, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        /// <summary>
        /// Vista principal de logs de búsqueda
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Historico()
        {
            if (!string.IsNullOrEmpty(UserName))
            {
                return View();
            }
            else
            {
                return RedirectToLogin();
            }
        }

        /// <summary>
        /// Muestra el listado de resultados del histórico de búsquedas 
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <returns>Vista tabular de resultados</returns>
        [System.Web.Mvc.HttpPost]
        public async Task<PartialViewResult> ListadoHistorico(DateTime start_date, DateTime end_date)
        {
            List<BCDBusquedasDTO> resultados = await servicioBusqueda.ObtenerBusquedas(start_date, end_date);
            return PartialView(resultados);
        }

        /// <summary>
        /// Búsqueda conversacional
        /// </summary>
        /// <param name="busqueda"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> BusquedaConversacional([FromBody] string busqueda)
        {
            if (!string.IsNullOrEmpty(UserName))
            {
                List<ChatHistoryDTO> historial = new List<ChatHistoryDTO>
                {
                    new ChatHistoryDTO { Role = "user", Content = busqueda }
                };

                RespuestaChatDTO chatResponse = await servicioOpenAI.BusquedaConversacional(historial, UserName, busqueda);
                historial.Add(new ChatHistoryDTO { Role = "assistant", Content = chatResponse.Contenido });
                Session["ChatHistory"] = historial;

                return Json(new
                {
                    response = chatResponse.Contenido,
                    references = chatResponse.Referencias
                });
            }
            else
            {
                return Json("Debe autenticarse antes de utilizar este servicio");
            }
        }

        /// <summary>
        ///  Permite recomendar un resultado de búsqueda
        /// </summary>
        /// <param name="data"></param>
        /// <param name="idBusqueda"></param>
        /// <returns>Booleano que indica si guardó o no la relación</returns>
        [System.Web.Mvc.HttpPost]
        public async Task<JsonResult> Relacionar([FromBody] WbBusquedasDTO data, int idBusqueda)
        {
            RespuestaDTO<string> rta = await servicioRecomendacion.SaveRelacion(data, idBusqueda, UserName);
            return new JsonResult { Data = rta };
        }
    }
}
