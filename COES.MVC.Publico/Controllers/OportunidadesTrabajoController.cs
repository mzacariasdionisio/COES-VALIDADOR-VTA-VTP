using COES.MVC.Publico.Helper;
using COES.MVC.Publico.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Base.Tools;
using System.Configuration;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Migraciones;
using System.IO;

namespace COES.MVC.Publico.Controllers
{
    public class OportunidadesTrabajoController : Controller
    {

        MigracionesAppServicio servicio = new MigracionesAppServicio();

        /// <summary>
        /// Carga inicial de la pagina
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {

            return View();
        }

        /// <summary>
        /// Carga el archivo cv a enviar
        /// </summary>
        /// <param name="nombreArchivo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upload(string nombreArchivo)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga;

                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string fileName = path + nombreArchivo;
                    file.SaveAs(fileName);
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Envia el email a la cuenta de rrhh
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult Enviar(OportunidadModel model)
        {
            try
            {
                string mensaje = this.ObtenerNotificacionAceptada(model);

                string path = AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga;
                string email = ConfigurationManager.AppSettings[Constantes.EmailOportunidades];

                List<string> listEmails = new List<string> { email };
                List<string> listCc = new List<string> { Constantes.EmailWebApp };
                List<string> listFile = new List<string> { path + model.nombrearchivo };

                mensaje = mensaje.Replace("[", "{");
                mensaje = mensaje.Replace("]", "}");

                Util.SendEmail(listEmails, (model.ConvocatoriaDesc != null ? model.ConvocatoriaDesc : "S/C") + " - " + Constantes.SubjectOportunidades, mensaje, Constantes.EmailWebApp, listCc, listFile);

                string mensajeRespuesta = this.ObtenerNotificacionRemitemte(model);

                Util.SendEmail(model.Correo, Constantes.SubjectOportunidadesRespuesta, mensajeRespuesta);

                if (System.IO.File.Exists(path + model.nombrearchivo)) System.IO.File.Delete(path + model.nombrearchivo);

                return Json(1);
            }
            catch (Exception ex)
            {
                
                return Json(-1);
            }
        }


        /// <summary>
        /// Arma el mensaje html para enviar al remitente
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private string ObtenerNotificacionRemitemte(OportunidadModel model)
        {
            string mensaje = @"
                <html>
                <head>      
                <style type='text/css'>
                <!--
                body
                [
                       font-family:Arial, Helvetica, sans-serif;
                       font-size:12px;
                       top:0;
                       left:0;
                       background-color:#ffffff;      
                ]               
                .celda
                [
                       color:#4171A0;
                       font-size:11px;
                       font-family:Arial, Helvetica, sans-serif;
                       font-weight:bold;
                       line-height:25px;
                       padding-left:20px;
                ]                
                -->
                </style>
                </head>
 
                <body>
 
                    <table width='605'>
                        <tr>
                            <td class='celda'>Estimado: {0}</td>
                        </tr>               
                        <tr>
                            <td><br /><br />Su envío ha sido exitoso. Gracias por visitar el portal del COES.</td>
                        </tr>
                    </table>
                </body>
                </html>
             ";

            return String.Format(mensaje, model.NombresCompletos + " " + model.Apellidos);
        }

        /// <summary>
        /// Arma el mensaje html a enviar
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private string ObtenerNotificacionAceptada(OportunidadModel model)
        {
            string mensaje = @"
                <html>
                <head>      
                <style type='text/css'>
                <!--
                body
                [
                       font-family:Arial, Helvetica, sans-serif;
                       font-size:12px;
                       top:0;
                       left:0;
                       background-color:#ffffff;      
                ]
                .celda
                [
                       color:#4171A0;
                       font-size:11px;
                       font-family:Arial, Helvetica, sans-serif;
                       font-weight:bold;
                       line-height:25px;
                       padding-left:20px;
                ]              
                -->
                </style>
                </head>
 
                <body>
 
                    <table width='605'>  
                        <tr>
                            <td>Convocatoria:</td>
                            <td class='celda'>{8}</td>
                        </tr>
                        <tr>
                            <td>Numero de Identificacion:</td>
                            <td class='celda'>{0}</td>
                        </tr>
                        <tr>
                            <td>Nombres Completos:</td>
                            <td class='celda'>{1}</td>
                        </tr>
                        <tr>
                            <td>Apellidos:</td>
                            <td class='celda'>{2}</td>
                        </tr>
                        <tr>
                            <td>Ciudad:</td>
                            <td class='celda'>{3}</td>
                        </tr>
                        <tr>
                            <td>Correo:</td>
                            <td class='celda'>{4}</td>
                        </tr>
                        <tr>
                            <td>Correo Alterno: </td>
                            <td class='celda'>{5}</td>
                        </tr>     
                        <tr>
                            <td>Telefono: </td>
                            <td class='celda'>{6}</td>
                        </tr>             
                        <tr>
                            <td>Descripcion: </td>
                            <td class='celda'>{7}</td>
                        </tr>    
                    </table>
                </body>
                </html>
             ";

            return String.Format(mensaje, model.NumeroDeIdentificacion, model.NombresCompletos, model.Apellidos,
                model.ciudad, model.Correo, model.Correoalterno, model.Telefonocontacto, model.descripcion, model.ConvocatoriaDesc);
        }

        public JsonResult CargarConvocatoria()
        {
            OportunidadModel model = new OportunidadModel();
            List<WbConvocatoriasDTO> lista = new List<WbConvocatoriasDTO>();
            lista = servicio.GetListaConvocatorias().Where(x => x.Convfechaini <= DateTime.Now.Date).OrderByDescending(x => x.Convnomb).ToList();
            return Json(lista.ToList());
        }
    }
}
