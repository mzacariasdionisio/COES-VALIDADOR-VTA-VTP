using COES.Servicios.Aplicacion.Migraciones;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace COES.WebAPI.Migraciones
{

    public class MigracionesController: ApiController
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(MigracionesController));
        MigracionesAppServicio service = new MigracionesAppServicio();

        //public EventoController()
        //{
        //    log4net.Config.XmlConfigurator.Configure();
        //}


        /// <summary>
        /// Listar empresas Demanda Barra
        /// </summary>
        /// <param name="idTipoEmpresa"></param>
        /// <returns></returns>
        public IHttpActionResult GetListarEmpresasDemandaBarrra(int idTipoEmpresa)
        {
            try
            {
                return Ok(
              this.service.ListarEmpresasDemandaBarrra(idTipoEmpresa)
           );
            }
            catch (Exception ex)
            {
                log.Error("GetListarTipoEvento", ex);
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
                throw new HttpResponseException(response);

            }

        }



    }
}