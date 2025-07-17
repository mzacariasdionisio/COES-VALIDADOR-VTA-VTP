using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Globalization;
using COES.Servicios.Aplicacion.DPODemanda;
using COES.Servicios.Aplicacion.DPODemanda.Helper;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.DemandaPO.Models;
using COES.MVC.Intranet.Helper;
using System.IO;
using System.Net.Mime;
using COES.Servicios.Aplicacion.PronosticoDemanda.Helper;
using COES.Framework.Base.Tools;

namespace COES.MVC.Intranet.Areas.DemandaPO.Controllers
{
    public class DatosSirpitController : Controller
    {
        DemandaPOAppServicio servicio = new DemandaPOAppServicio();

        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult Subestacion()
        {
            DatosSirpitModel model = new DatosSirpitModel()
            {
                TipoMensaje = "info",
                Mensaje = "Consulta de datos maestros para las Subestaciones",
                cboSubestaciones = this.servicio.ListaSubestacionesDPO().OrderBy(x => x.Dposubnombre).ToList()
            };
            return PartialView(model);
        }

        /// <summary>
        /// Permite listar las subestaciones para la grilla
        /// </summary>
        /// <param name="subestacion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarSubestaciones(int subestacion)
        {
            List<DpoSubestacionDTO> subestaciones = this.servicio.ListaSubestacionesDPO()
                                                                 .OrderBy(x => x.Dposubnombre).ToList();
            subestaciones = (subestacion == 0) ? subestaciones : 
                                                   subestaciones.Where(x => x.Dposubcodi == subestacion).ToList();
            DatosSirpitModel model = new DatosSirpitModel()
            {
                ListaSubestaciones = subestaciones
            };
            return Json(model);
        }

        public PartialViewResult Transformador()
        {
            DatosSirpitModel model = new DatosSirpitModel()
            {
                TipoMensaje = "info",
                Mensaje = "Consulta de datos maestros para las Transformadores",
                cboTransformadores = this.servicio.ListaTransformadoresDPO().OrderBy(x => x.Dpotnfcodiexcel).ToList()
            };
            return PartialView(model);
        }

        /// <summary>
        /// Permite listar los tranformadores para la grilla
        /// </summary>
        /// <param name="subestacion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarTransformadores(int transformador)
        {
            List<DpoTransformadorDTO> transformadores = this.servicio.ListaTransformadoresDPO()
                                                                     .OrderBy(x => x.Dpotnfcodiexcel).ToList();
            transformadores = (transformador == 0) ? transformadores :
                                                       transformadores.Where(x => x.Dpotnfcodi == transformador)
                                                       .ToList();
            DatosSirpitModel model = new DatosSirpitModel()
            {
                ListaTransformadores = transformadores
            };
            return Json(model);
        }

        public PartialViewResult Barra()
        {
            DatosSirpitModel model = new DatosSirpitModel()
            {
                TipoMensaje = "info",
                Mensaje = "Consulta de datos maestros para las Barras",
                cboBarras = this.servicio.ListaBarrasDPO().OrderBy(x => x.Dpobarnombre).ToList()
            };
            return PartialView(model);
        }

        /// <summary>
        /// Permite listar las barras para la grilla
        /// </summary>
        /// <param name="barra"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarBarras(int barra)
        {
            List<DpoBarraDTO> barras = this.servicio.ListaBarrasDPO().OrderBy(x => x.Dpobarnombre).ToList();
            barras = (barra == 0) ? barras : barras.Where(x => x.Dpobarcodi == barra).ToList();
            DatosSirpitModel model = new DatosSirpitModel()
            {
                ListaBarras = barras
            };
            return Json(model);
        }

        public PartialViewResult Data()
        {
            DatosSirpitModel model = new DatosSirpitModel()
            {
                TipoMensaje = "info",
                Mensaje = "Consulta de datos de cada uno de los maestros",
                cboSubestaciones = this.servicio.ListaSubestacionesDPO().OrderBy(x => x.Dposubnombre).ToList(),
                cboTransformadores = this.servicio.ListaTransformadoresDPO().OrderBy(x => x.Dpotnfcodiexcel).ToList(),
                cboBarras = this.servicio.ListaBarrasDPO().OrderBy(x => x.Dpobarnombre).ToList()
            };
            return PartialView(model);
        }

        /// <summary>
        /// Permite listar las los datos de las tablas Maestras en la grilla
        /// </summary>
        /// <param name="barra"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarData(int subestacion, int transformador, int barra, string fechainicio, string fechafin)
        {
            DatosSirpitModel model = new DatosSirpitModel()
            {
                ListaDatos96 = this.servicio.ListAllBetweenDates(fechainicio, fechafin, subestacion, transformador, barra)
            };
            var JsonResult = Json(model);
            JsonResult.MaxJsonLength = Int32.MaxValue;
            return JsonResult;
        }

        public PartialViewResult CargaSirpit()
        {
            DatosSirpitModel model = new DatosSirpitModel()
            {
                TipoMensaje = "info",
                Mensaje = "Carga de datos SIRPIT",
            };
            return PartialView(model);
        }

        /// <summary>
        /// Importa el archivo con la información de las Maestras SIRPIT, SICLI
        /// </summary>
        /// <param name="archivo">Archivo importado que sera subido al servidor temporalmente</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MaestrasImportar(HttpPostedFileBase archivo)
        {
            object res = new object();
            string typeMsg = String.Empty;
            string dataMsg = String.Empty;
            List<string> detailMsg = new List<string>();//String.Empty;
            string regFecha = DateTime.Now.ToString(ConstantesDpo.FormatoFecha);

            //string ruta = @"D:\";
            string ruta = ConfigurationManager.AppSettings[ConstantesDpo.MedidorDemandaPO].ToString();
            string nombreArchivo = Path.GetFileName(archivo.FileName);
            string extension = Path.GetExtension(archivo.FileName);

            if (!(extension == ConstantesDpo.ExtensionXlsx || extension == ConstantesDpo.ExtensionTxt)) {
                typeMsg = "error";
                dataMsg = "El formato no es valido, solo se admite formato .xlsx(maestras) y .txt(datos)...";
                res = new { typeMsg, dataMsg, detailMsg };
                return Json(res);
            }

            if (!Directory.Exists(ruta))
            {
                typeMsg = "error";
                dataMsg = "La carpeta requerida para la importación no existe";
                res = new { typeMsg, dataMsg, detailMsg };
                return Json(res);
            }

            try
            {
                //Crea el archivo en el servidor                
                FileInfo nuevoArchivo = new FileInfo(ruta + nombreArchivo);
                if (nuevoArchivo.Exists) nuevoArchivo.Delete();
                archivo.SaveAs(ruta + nombreArchivo);

                //Procesa los datos del archivo excel
                if (extension == ConstantesDpo.ExtensionXlsx)
                {
                    res = this.servicio.MaestrasDPOProcesar(ruta + nombreArchivo, regFecha, User.Identity.Name);
                }
                else {
                    res = this.servicio.DatosDPO96(ruta + nombreArchivo, regFecha, User.Identity.Name);
                }
                

                //Elimina el archivo del servidor
                nuevoArchivo.Delete();
            }
            catch (Exception ex)
            {
                typeMsg = "error";
                dataMsg = ex.Message;
                res = new { typeMsg, dataMsg, detailMsg };
                return Json(res);
            }

            return Json(res);
        }

        [HttpPost]
        public ActionResult GetFiles(string folderPath)
        {
            if (FileServer.VerificarLaExistenciaDirectorio(folderPath))
            {
                string regFecha = DateTime.Now.ToString(ConstantesDpo.FormatoFecha);
                List<FileData> files = FileServer.ListarArhivos(string.Empty, folderPath);
                List<string> fileNames = new List<string>();

                foreach (FileData file in files)
                {
                    string fileName = file.FileName;
                    if (file.Extension != ".txt")
                        continue;
                    //Leemos cada archivo
                    //FileData File = FileServer.ObtenerArchivoEspecifico(folderPath, fileName);
                    object res = this.servicio.DatosDPO96FileServer(folderPath, fileName, regFecha, User.Identity.Name);
                    fileNames.Add(fileName + " - " + res.ToString());
                }

                return Json(fileNames);
            }
            else
            {
                return Json("-1");
            }
        }
    }
}