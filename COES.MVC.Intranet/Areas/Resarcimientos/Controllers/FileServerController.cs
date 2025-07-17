using COES.Framework.Base.Tools;
using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.Resarcimientos.Models;
using COES.MVC.Intranet.SeguridadServicio;
using COES.Servicios.Aplicacion.ResarcimientoNTCSE;
using COES.Servicios.Aplicacion.Helper;
using COES.MVC.Intranet.Areas.FileManager.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Configuration;
using System;
using System.IO;
using System.Collections;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.ResarcimientoNTCSE.Helper;



namespace COES.MVC.Intranet.Areas.Resarcimientos.Controllers
{
    public class FileServerController : BaseController
    {
        /// <summary>
        /// Instanciamiento de Servicios de Aplicacion
        /// </summary>
        ResarcimientoNTCSEAppServicio ntcse = new ResarcimientoNTCSEAppServicio();

        /// <summary>
        /// Carga de la pagina
        /// </summary>
        /// <returns></returns>
        public ViewResult Default()
        {
            ViewBag.Ruta = base.PathFiles;            
            return View();
        }

        #region SubirExcel


        /// <summary>
        /// Opción que permite subir la plantilla que utilizarán los usuarios de Extranet al filseserver.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]        
        public JsonResult SubirExcel(FormCollection collection)
        {
            try
            {
                HttpPostedFileBase fileData = Request.Files[0];
                if (FileServer.VerificarExistenciaFile(base.PathFiles, ConstantesResarcimiento.PlantillaRC, string.Empty))
                    FileServer.RenameBlob(base.PathFiles, ConstantesResarcimiento.PlantillaRC, DateTime.Now.ToString("dd-MM-yyyy hhmm") + ".xlsm", string.Empty);
                bool flag = FileServer.UploadFromStream(fileData.InputStream, base.PathFiles, ConstantesResarcimiento.PlantillaRC, string.Empty);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        #endregion


    }

}
