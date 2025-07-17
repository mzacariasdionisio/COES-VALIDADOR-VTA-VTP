using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.Transferencias.Models;
using COES.Servicios.Aplicacion.Transferencias;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Areas.Transferencias.Helper;
using COES.MVC.Intranet.Controllers;
using COES.Dominio.DTO.Sic;
using System.IO;
using COES.Framework.Base.Tools;

namespace COES.MVC.Intranet.Areas.Transferencias.Controllers
{
    public class ModeloEnvioController : BaseController
    {

        string pathModelo = @"\\coes.org.pe\archivosapp\webapp\";
        string folder = @"ModeloSTR\";

        ModeloEnvioAppServicio servicio = new ModeloEnvioAppServicio();
        
        // GET: Transferencias/ModeloEnvio
        public ActionResult Index()
        {
            ModeloEnvioModel model = new ModeloEnvioModel();
            model.ListaEmpresas = this.ObtenerEmpresasPorUsuario(base.UserName);
            model.ListaPeriodos = (new PeriodoAppServicio()).ListPeriodo();
            return View(model);
        }

        /// <summary>
        /// Obtener Empresas Por Usuario
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ObtenerEmpresasPorUsuario(string usuario)
        {
            List<SiEmpresaDTO> listaEmpresas = new List<SiEmpresaDTO>();

            //bool accesoEmpresa = base.VerificarAccesoAccion(Acciones.AccesoEmpresa, usuario);            

            //if (accesoEmpresa)
            //{
                listaEmpresas = (new SeguridadServicio.SeguridadServicioClient()).ListarEmpresas().ToList().Where(x => x.TIPOEMPRCODI == 3
           || x.EMPRCODI == 11772 || x.EMPRCODI == 13).OrderBy(x => x.EMPRNOMB)
                   .Select(x => new SiEmpresaDTO() { Emprcodi = x.EMPRCODI, Emprnomb = x.EMPRNOMB }).OrderBy(x => x.Emprnomb).ToList();
            //}
            //else
            //{
            //    listaEmpresas = base.ListaEmpresas                    
            //        .Select(x => new SiEmpresaDTO() { Emprcodi = x.EMPRCODI, Emprnomb = x.EMPRNOMB }).ToList();
            //}

            //if (!listaEmpresas.Any())
            //    listaEmpresas.Add(new SiEmpresaDTO() { Emprcodi = 0, Emprnomb = "No Existe" });

            return listaEmpresas;
        }

        [HttpPost]
        public JsonResult ObtenerVersion(int pericodi)
        {
            ModeloEnvioModel model = new ModeloEnvioModel();
            model.ListaRecalculo = (new RecalculoAppServicio()).ListRecalculos(pericodi);
            return Json(model);
        }

        [HttpPost]
        public PartialViewResult Listado(int empresa, int periodo, int version)
        {
            ModeloEnvioModel model = new ModeloEnvioModel();
            model.ListaEnvio = this.servicio.GetByCriteriaTrnModeloEnvios(empresa, periodo, version);
            return PartialView(model);
        }

        public ActionResult Upload(int chunks, int chunk, string name, int empresa, int periodo, int version)
        {
            try
            {
                string url = "PERIODO_" + periodo + "/" +  "VERSION_" + version +  "/" + "EMPRESA_" + empresa + "/";
               
                string extension = name.Split('.')[1];

                if (Request.Files.Count > 0)
                {
                    if (chunks > 1)
                    {
                        var file = Request.Files[0];
                        string path = AppDomain.CurrentDomain.BaseDirectory + @"Areas/Transferencias/TransferenciasModelo/";
                        using (var fs = new FileStream(Path.Combine(path, name), chunk == 0 ? FileMode.Create : FileMode.Append))
                        {
                            var buffer = new byte[file.InputStream.Length];
                            file.InputStream.Read(buffer, 0, buffer.Length);
                            fs.Write(buffer, 0, buffer.Length);
                        }

                        if (chunk == chunks - 1)
                        {
                         

                            TrnModeloEnvioDTO entity = new TrnModeloEnvioDTO();
                            entity.Emprcodi = empresa;
                            entity.Pericodi = periodo;
                            entity.Modenvversion = version;
                            entity.Modenvfecenvio = DateTime.Now;
                            entity.Modenvusuario = base.UserName;
                            entity.Modenvextension = extension;
                            entity.Modendfile = name;

                            int idEnvio = this.servicio.SaveTrnModeloEnvio(entity);
                            string nombreFile = "MODELO_" + idEnvio + "." + extension;                            

                            if (!FileServer.VerificarExistenciaDirectorio(folder + url, pathModelo))
                            {
                                FileServer.CreateFolder(folder, url, pathModelo);
                            }

                            if (FileServer.VerificarExistenciaFile(url, nombreFile, pathModelo))
                            {
                                FileServer.DeleteBlob(url + nombreFile, pathModelo);
                            }

                            FileServer.UploadFromFileDirectory(path + name, folder + url, nombreFile, pathModelo);

                           
                        }
                    }
                    else
                    {
                        TrnModeloEnvioDTO entity = new TrnModeloEnvioDTO();
                        entity.Emprcodi = empresa;
                        entity.Pericodi = periodo;
                        entity.Modenvversion = version;
                        entity.Modenvfecenvio = DateTime.Now;
                        entity.Modenvusuario = base.UserName;
                        entity.Modenvextension = extension;
                        entity.Modendfile = name;

                        int idEnvio = this.servicio.SaveTrnModeloEnvio(entity);
                        string nombreFile = "MODELO_" + idEnvio + "." + extension;

                        if (!FileServer.VerificarExistenciaDirectorio(folder + url, pathModelo))
                        {
                            FileServer.CreateFolder(folder, url, pathModelo);
                        }

                        if (FileServer.VerificarExistenciaFile(folder + url, nombreFile, pathModelo))
                        {
                            FileServer.DeleteBlob(folder + url + nombreFile, pathModelo);
                        }

                        var file = Request.Files[0];
                        FileServer.UploadFromStream(file.InputStream, folder + url, nombreFile, pathModelo);                      
                    }
                }

                return Json(new { success = true, indicador = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarArchivo(int id)
        {
            TrnModeloEnvioDTO entity = this.servicio.GetByIdTrnModeloEnvio(id);

            string url = folder + "PERIODO_" + entity.Pericodi + "/VERSION_" + entity.Modenvversion + "/EMPRESA_" + entity.Emprcodi + "/MODELO_" + id + "." + entity.Modenvextension;
            Stream result = FileServer.DownloadToStream(url, pathModelo);

            return File(result, entity.Modenvextension, entity.Modendfile);
        }

        public ActionResult Demo()
        {
            return View();
        }

        public ActionResult DemoCodigo()
        {
            return View();
        }

        public ActionResult DemoPorcentaje()
        {
            return View();
        }

        public ActionResult DemoDetalleCodigo()
        {
            return View();
        }

        public ActionResult DemoDetalleCodigo1()
        {
            return View();
        }

        public ActionResult DemoDetalleCodigo2()
        {
            return View();
        }


        public ActionResult DemoDetalleCodigo3()
        {
            return View();
        }

        public ActionResult DemoConsulta()
        {
            return View();
        }

        public ActionResult DemoModelo()
        {
            return View();
        }

        public ActionResult DemoReporteVTEA()
        {
            return View();
        }

        public ActionResult DemoReporteVTEA1()
        {
            return View();
        }

        public ActionResult DemoSaldo()
        {
            return View();
        }
    }
}