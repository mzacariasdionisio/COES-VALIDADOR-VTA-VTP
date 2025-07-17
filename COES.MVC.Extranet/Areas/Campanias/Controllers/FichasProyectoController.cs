using COES.Dominio.DTO.Campania;
using COES.Dominio.DTO.Sic;
using COES.MVC.Extranet.Controllers;
using COES.Servicios.Aplicacion.Campanias;
using System;
using System.Web.Mvc;
using log4net;
using COES.MVC.Extranet.Areas.Campanias.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using log4net.Repository.Hierarchy;
using COES.MVC.Extranet.Helper;
using COES.Framework.Base.Tools;
using System.IO;


namespace COES.MVC.Extranet.Areas.Campanias.Controllers
{
    public class FichasProyectoController : BaseController
    {
        CampaniasAppService campaniasAppService = new CampaniasAppService();
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(FichasProyectoController));

        public FichasProyectoController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        [System.Web.Mvc.HttpPost]
        public PartialViewResult G1_CentralHidroelectrica()
        {
            CampaniasModel model = new CampaniasModel();
            try
            {
            }
            catch (Exception ex)
            {
            }
            return PartialView(model);
        }
        [System.Web.Mvc.HttpPost]
        public PartialViewResult G2_CentralTermoelectrica()
        {
            try
            {
            }
            catch (Exception ex)
            {
            }
            return PartialView();
        }
        [System.Web.Mvc.HttpPost]
        public PartialViewResult G3_CentralEolica()
        {
            try
            {
            }
            catch (Exception ex)
            {
            }
            return PartialView();
        }
        [System.Web.Mvc.HttpPost]
        public PartialViewResult G4_CentralSolar()
        {
            try
            {
            }
            catch (Exception ex)
            {
            }
            return PartialView();
        }
        [System.Web.Mvc.HttpPost]
        public PartialViewResult G5_CentralBiomasa()
        {
            try
            {
            }
            catch (Exception ex)
            {
            }
            return PartialView();
        }
        [System.Web.Mvc.HttpPost]
        public PartialViewResult G6_Subestaciones()
        {
            try
            {
            }
            catch (Exception ex)
            {
            }
            return PartialView();
        }
        [System.Web.Mvc.HttpPost]
        public PartialViewResult G7_Lineas()
        {
            try
            {
            }
            catch (Exception ex)
            {
            }
            return PartialView();
        }
        [System.Web.Mvc.HttpPost]
        public PartialViewResult T1_Lineas()
        {
            try
            {
            }
            catch (Exception ex)
            {
            }
            return PartialView();
        }
        [System.Web.Mvc.HttpPost]
        public PartialViewResult T2_Subestaciones()
        {
            try
            {
            }
            catch (Exception ex)
            {
            }
            return PartialView();
        }
        [System.Web.Mvc.HttpPost]
        public PartialViewResult T3_Cronograma()
        {
            try
            {
            }
            catch (Exception ex)
            {
            }
            return PartialView();
        }
        [System.Web.Mvc.HttpPost]
        public PartialViewResult ITC_SistemaElectricoParametros()
        {
            try
            {
            }
            catch (Exception ex)
            {
            }
            return PartialView();
        }
        [System.Web.Mvc.HttpPost]
        public PartialViewResult ITC_DemandaEDE()
        {
            try
            {
            }
            catch (Exception ex)
            {
            }
            return PartialView();
        }
        [System.Web.Mvc.HttpPost]
        public PartialViewResult D1_DemandaGrandesProyectos()
        {
            try
            {
            }
            catch (Exception ex)
            {
            }
            return PartialView();
        }
        [System.Web.Mvc.HttpPost]
        public PartialViewResult CuestionarioGeneracionDistribuida()
        {
            try
            {
            }
            catch (Exception ex)
            {
            }
            return PartialView();
        }
        [System.Web.Mvc.HttpPost]
        public PartialViewResult Cuestionario_H2V()
        {
            try
            {
            }
            catch (Exception ex)
            {
            }
            return PartialView();
        }


        [System.Web.Mvc.HttpPost]
        public JsonResult ListarDepartamentos()
        {
            List<DepartamentoDTO> departamentoDTOs = new List<DepartamentoDTO>();
            departamentoDTOs = campaniasAppService.GetDepartamentos();
            return Json(departamentoDTOs);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult ListarProvincias(string id)
        {
            List<ProvinciaDTO> provinciaDTOs = new List<ProvinciaDTO>();
            provinciaDTOs = campaniasAppService.GetProvincias(id);
            return Json(provinciaDTOs);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult ListarDistritos(string id)
        {
            List<DistritoDTO> distritoDTOs = new List<DistritoDTO>();
            distritoDTOs = campaniasAppService.GetDistrito(id);
            return Json(distritoDTOs);
        }
        [System.Web.Mvc.HttpPost]
        public JsonResult UbicacionByDistro(string id)
        {
            UbicacionDTO distritoDTOs = new UbicacionDTO();
            
            distritoDTOs = campaniasAppService.GetUbicacionByDistrito(id);
            Console.WriteLine("UbicacionByDistro");
            Console.WriteLine(distritoDTOs);
            return Json(distritoDTOs);
        }


        [System.Web.Mvc.HttpPost]
        public JsonResult ListarEmpresas()
        {
            //List<SiEmpresaDTO> ListaEmpresaUsuario = this.seguridad.ObtenerEmpresasPorUsuario(User.Identity.Name).
            // Select(x => new SiEmpresaDTO
            // {
            //     Emprcodi = x.EMPRCODI,
            //     Emprnomb = x.EMPRNOMB
            // }).ToList();
            List<SiEmpresaDTO> ListaEmpresaUsuario = new List<SiEmpresaDTO>();
            return Json(ListaEmpresaUsuario);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult ListarPeriodos()
        {
            List<PeriodoDTO> ListaPeriodos = campaniasAppService.ListPeriodos();
            return Json(ListaPeriodos);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult ListarCatalogo(int id)
        {
            List<DataCatalogoDTO> dataCatalogoDTOs = new List<DataCatalogoDTO>();
            dataCatalogoDTOs = campaniasAppService.ListParametria(id);
            return Json(dataCatalogoDTOs);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult Grabar(ProyectoModel proyectoModel)
        {
            int result = 0;
            ProyectoModel proyectoDTO = new ProyectoModel();

            return Json(proyectoDTO);
        }


        [System.Web.Mvc.HttpPost]
        public JsonResult ListarProyectos()
        {
            List<TipoProyectoDTO> ListaProyectos = campaniasAppService.ListTipoProyecto();
            return Json(ListaProyectos);
        }

        public ActionResult UploadArchivoUbicacion(int? id)
        {
            try
            {
                string extension = string.Empty;
                string fileName = string.Empty;
                string fileNameNotPath = string.Empty;
                string nombreReal = string.Empty;
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string path = campaniasAppService.ObtenerPathArchivosCampianas();
                    nombreReal = file.FileName;
                    int indexOf = nombreReal.LastIndexOf('.');
                    extension = nombreReal.Substring(indexOf + 1, nombreReal.Length - indexOf - 1);
                    Guid myuuid = Guid.NewGuid();
                    string myuuidAsString = myuuid.ToString();
                    //if (id != null)
                    //{
                    fileNameNotPath = myuuidAsString + "." + extension;
                    fileName = myuuidAsString + "." + extension;
                    //}
                    //else
                    //{
                    //    fileNameNotPath = string.Format(ConstantesCampania.TemporalUbicacion, extension);
                    //    fileName = string.Format(ConstantesCampania.TemporalUbicacion, extension);
                    //}
                    fileName = path + fileName;
                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }

                    file.SaveAs(fileName);

                    //if (FileServer.VerificarExistenciaFile(ConstantesCampania.FolderCampanias, fileName,
                    //    ConstantesCampania.RutaBaseCampania))
                    //{
                    //    FileServer.DeleteBlob(ConstantesCampania.FolderCampanias + fileName,
                    //        ConstantesCampania.RutaBaseCampania);
                    //}

                    //FileServer.UploadFromStream(file.InputStream, ConstantesCampania.FolderCampanias, fileName,
                    //    ConstantesCampania.RutaBaseCampania);

                }
                return Json(new { success = true, indicador = 1, extension = extension, fileNameNotPath = fileNameNotPath, nombreReal = nombreReal }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }


        }

        public ActionResult UploadArchivoInfBasica(int? id)
        {
            try
            {
                string extension = string.Empty;
                string fileName = string.Empty;
                string fileNameNotPath = string.Empty;
                string nombreReal = string.Empty;
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string path = campaniasAppService.ObtenerPathArchivosCampianas();
                    nombreReal = file.FileName;
                    int indexOf = nombreReal.LastIndexOf('.');
                    extension = nombreReal.Substring(indexOf + 1, nombreReal.Length - indexOf - 1);
                    Guid myuuid = Guid.NewGuid();
                    string myuuidAsString = myuuid.ToString();
                    //if (id != null)
                    //{
                    fileNameNotPath = myuuidAsString + "." + extension;
                    fileName = myuuidAsString + "." + extension;
                    //}
                    //else
                    //{
                    //    fileNameNotPath = string.Format(ConstantesCampania.TemporalUbicacion, extension);
                    //    fileName = string.Format(ConstantesCampania.TemporalUbicacion, extension);
                    //}
                    fileName = path + fileName;
                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }

                    file.SaveAs(fileName);

                    //if (FileServer.VerificarExistenciaFile(ConstantesCampania.FolderCampanias, fileName,
                    //    ConstantesCampania.RutaBaseCampania))
                    //{
                    //    FileServer.DeleteBlob(ConstantesCampania.FolderCampanias + fileName,
                    //        ConstantesCampania.RutaBaseCampania);
                    //}

                    //FileServer.UploadFromStream(file.InputStream, ConstantesCampania.FolderCampanias, fileName,
                    //    ConstantesCampania.RutaBaseCampania);

                }
                return Json(new { success = true, indicador = 1, extension = extension, fileNameNotPath = fileNameNotPath, nombreReal = nombreReal }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }


        }

        public ActionResult UploadArchivoUnTurbo(int? id)
        {
            try
            {
                string extension = string.Empty;
                string fileName = string.Empty;
                string fileNameNotPath = string.Empty;
                string nombreReal = string.Empty;
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string path = campaniasAppService.ObtenerPathArchivosCampianas();
                    nombreReal = file.FileName;
                    int indexOf = nombreReal.LastIndexOf('.');
                    extension = nombreReal.Substring(indexOf + 1, nombreReal.Length - indexOf - 1);
                    Guid myuuid = Guid.NewGuid();
                    string myuuidAsString = myuuid.ToString();
                    //if (id != null)
                    //{
                    fileNameNotPath = myuuidAsString + "." + extension;
                    fileName = myuuidAsString + "." + extension;
                    //}
                    //else
                    //{
                    //    fileNameNotPath = string.Format(ConstantesCampania.TemporalUbicacion, extension);
                    //    fileName = string.Format(ConstantesCampania.TemporalUbicacion, extension);
                    //}
                    fileName = path + fileName;
                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }

                    file.SaveAs(fileName);

                    //if (FileServer.VerificarExistenciaFile(ConstantesCampania.FolderCampanias, fileName,
                    //    ConstantesCampania.RutaBaseCampania))
                    //{
                    //    FileServer.DeleteBlob(ConstantesCampania.FolderCampanias + fileName,
                    //        ConstantesCampania.RutaBaseCampania);
                    //}

                    //FileServer.UploadFromStream(file.InputStream, ConstantesCampania.FolderCampanias, fileName,
                    //    ConstantesCampania.RutaBaseCampania);

                }
                return Json(new { success = true, indicador = 1, extension = extension, fileNameNotPath = fileNameNotPath, nombreReal = nombreReal }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UploadArchivoGeneral(int? id, string ordena_ruta = "")
        {
            try
            {
                string extension = string.Empty;
                string fileName = string.Empty;
                string fileNameNotPath = string.Empty;
                string nombreReal = string.Empty;

                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string path = campaniasAppService.ObtenerPathArchivosCampianas() + ordena_ruta;

                    // Asegurar que la carpeta existe antes de guardar el archivo
                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }

                    nombreReal = file.FileName;
                    int indexOf = nombreReal.LastIndexOf('.');
                    extension = nombreReal.Substring(indexOf + 1);

                    Guid myuuid = Guid.NewGuid();
                    fileNameNotPath = myuuid.ToString() + "." + extension;
                    fileName = Path.Combine(path, fileNameNotPath); // Construcción segura de la ruta

                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }

                    file.SaveAs(fileName);

                }
                return Json(new { success = true, indicador = 1, extension, fileNameNotPath, nombreReal }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UploadArchivoCuenca(int? id)
        {
            try
            {
                string extension = string.Empty;
                string fileName = string.Empty;
                string fileNameNotPath = string.Empty;
                string nombreReal = string.Empty;
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string path = campaniasAppService.ObtenerPathArchivosCampianas();
                    nombreReal = file.FileName;
                    int indexOf = nombreReal.LastIndexOf('.');
                    extension = nombreReal.Substring(indexOf + 1, nombreReal.Length - indexOf - 1);
                    Guid myuuid = Guid.NewGuid();
                    string myuuidAsString = myuuid.ToString();
                    //if (id != null)
                    //{
                    fileNameNotPath = myuuidAsString + "." + extension;
                    fileName = myuuidAsString + "." + extension;
                    //}
                    //else
                    //{
                    //    fileNameNotPath = string.Format(ConstantesCampania.TemporalUbicacion, extension);
                    //    fileName = string.Format(ConstantesCampania.TemporalUbicacion, extension);
                    //}
                    fileName = path + fileName;
                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }

                    file.SaveAs(fileName);

                    //if (FileServer.VerificarExistenciaFile(ConstantesCampania.FolderCampanias, fileName,
                    //    ConstantesCampania.RutaBaseCampania))
                    //{
                    //    FileServer.DeleteBlob(ConstantesCampania.FolderCampanias + fileName,
                    //        ConstantesCampania.RutaBaseCampania);
                    //}

                    //FileServer.UploadFromStream(file.InputStream, ConstantesCampania.FolderCampanias, fileName,
                    //    ConstantesCampania.RutaBaseCampania);

                }
                return Json(new { success = true, indicador = 1, extension = extension, fileNameNotPath = fileNameNotPath, nombreReal = nombreReal }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UploadArchivoEquipamiento(int? id)
        {
            try
            {
                string extension = string.Empty;
                string fileName = string.Empty;
                string fileNameNotPath = string.Empty;
                string nombreReal = string.Empty;
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string path = campaniasAppService.ObtenerPathArchivosCampianas();
                    nombreReal = file.FileName;
                    int indexOf = nombreReal.LastIndexOf('.');
                    extension = nombreReal.Substring(indexOf + 1, nombreReal.Length - indexOf - 1);
                    Guid myuuid = Guid.NewGuid();
                    string myuuidAsString = myuuid.ToString();
                    //if (id != null)
                    //{
                    fileNameNotPath = myuuidAsString + "." + extension;
                    fileName = myuuidAsString + "." + extension;
                    //}
                    //else
                    //{
                    //    fileNameNotPath = string.Format(ConstantesCampania.TemporalUbicacion, extension);
                    //    fileName = string.Format(ConstantesCampania.TemporalUbicacion, extension);
                    //}
                    fileName = path + fileName;
                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }

                    file.SaveAs(fileName);

                    //if (FileServer.VerificarExistenciaFile(ConstantesCampania.FolderCampanias, fileName,
                    //    ConstantesCampania.RutaBaseCampania))
                    //{
                    //    FileServer.DeleteBlob(ConstantesCampania.FolderCampanias + fileName,
                    //        ConstantesCampania.RutaBaseCampania);
                    //}

                    //FileServer.UploadFromStream(file.InputStream, ConstantesCampania.FolderCampanias, fileName,
                    //    ConstantesCampania.RutaBaseCampania);

                }
                return Json(new { success = true, indicador = 1, extension = extension, fileNameNotPath = fileNameNotPath, nombreReal = nombreReal }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }


        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarRegistroArchivo(ArchivoInfoDTO archivoInfoDTO)
        {
            int responseResult = 0;
            archivoInfoDTO.ArchFechaSubida = DateTime.Now;
            archivoInfoDTO.ArchCodi = campaniasAppService.GetLastArchivoInfoId();
            archivoInfoDTO.IndDel = Constantes.IndDel;
            archivoInfoDTO.UsuarioCreacion = User.Identity.Name;

            if (campaniasAppService.SaveArchivoInfo(archivoInfoDTO))
            {
                responseResult = 1;
                return Json(new { success = true, responseResult = responseResult, id = archivoInfoDTO.ArchCodi, descripcion = archivoInfoDTO.Descripcion }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                responseResult = 0;
                return Json(new { success = false, responseResult = responseResult, id = 0 }, JsonRequestBehavior.AllowGet);
            }


        }

         [System.Web.Mvc.HttpPost]
        public JsonResult ValidarArchivo(ArchivoInfoDTO archivoInfoDTO)
        {
            archivoInfoDTO.IndDel = Constantes.IndDel;
           
            List<ArchivoInfoDTO> Lista1DTO = campaniasAppService.GetArchivoInfoProyCodiNom(archivoInfoDTO.ProyCodi, archivoInfoDTO.SeccCodi, archivoInfoDTO.ArchNombre);
            return Json(new { success = true, responseResult = Lista1DTO }, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult ListarArchivosFichas(ProyectoModel proyectoModel)
        {
            List<ArchivoInfoDTO> Lista1DTO = campaniasAppService.GetArchivoInfoProyCodi(proyectoModel.ProyCodi, proyectoModel.Secccodi);
            return Json(new { success = true, responseResult = Lista1DTO }, JsonRequestBehavior.AllowGet);

        }


        [System.Web.Mvc.HttpPost]
        public JsonResult EliminarFile(int id)
        {
            int result = 0;
            string usuario = User.Identity.Name;
            if (campaniasAppService.DeleteArchivoInfoById(id, usuario))
            {
                result = 1;
            }
            else
            {
                result = 0;
            }
            return Json(result);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult EliminarCuenca(int id)
        {
            int result = 0;
            string usuario = User.Identity.Name;
            if (campaniasAppService.DeleteRegHojaDById2(id, usuario))
            {
                result = 1;
            }
            else
            {
                result = 0;
            }
            return Json(result);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetChHojaA(int id)
        {
            RegHojaADTO regHojaADTO = campaniasAppService.GetRegHojaAById(id);
            return Json(new { success = true, responseResult = regHojaADTO }, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetChHojaB(int id)
        {
            RegHojaBDTO regHojaBDTO = campaniasAppService.GetRegHojaBById(id);
            return Json(new { success = true, responseResult = regHojaBDTO }, JsonRequestBehavior.AllowGet);
        }
        [System.Web.Mvc.HttpPost]
        public JsonResult GetChHojaC(int id)
        {
            RegHojaCDTO regHojaCDTO = campaniasAppService.GetRegHojaCById(id);
            List<DetRegHojaCDTO> ListaDTO = campaniasAppService.GetDetRegHojaCFichaCCodi(regHojaCDTO.Fichaccodi);
            regHojaCDTO.DetRegHojaCs = ListaDTO;
            return Json(new { success = true, responseResult = regHojaCDTO }, JsonRequestBehavior.AllowGet);
        }

         [System.Web.Mvc.HttpPost]
        public JsonResult GetChHojaD(int id)
        {
            // Obtiene la lista de registros
            List<RegHojaDDTO> regHojaDDTO = campaniasAppService.GetRegHojaDById(id);

            // Itera sobre cada elemento en la lista
            foreach (var item in regHojaDDTO)
            {
                // Obtiene los detalles para el elemento actual usando su Fichaccodi
                item.ListDetRegHojaD = campaniasAppService.GetDetRegHojaDFichaCCodi(item.Hojadcodi);
            }

            // Devuelve la respuesta en formato JSON
            return Json(new { success = true, responseResult = regHojaDDTO }, JsonRequestBehavior.AllowGet);
        }


        [System.Web.Mvc.HttpPost]
        public JsonResult GetRegHojaCCTTA(int id)
        {
            RegHojaCCTTADTO regHojaCCTTADTO = campaniasAppService.GetRegHojaCCTTAById(id);
            return Json(new { success = true, responseResult = regHojaCCTTADTO }, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetRegHojaCCTTB(int id)
        {
            RegHojaCCTTBDTO regHojaCCTTBDTO = campaniasAppService.GetRegHojaCCTTBById(id);
            return Json(new { success = true, responseResult = regHojaCCTTBDTO }, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetRegHojaCCTTC(int id)
        {
            RegHojaCCTTCDTO regHojaCCTTCDTO = campaniasAppService.GetRegHojaCCTTCById(id);
            List<Det1RegHojaCCTTCDTO> Lista1DTO = campaniasAppService.GetDet1RegHojaCCTTCCentralCodi(regHojaCCTTCDTO.Centralcodi);
            List<Det2RegHojaCCTTCDTO> Lista2DTO = campaniasAppService.GetDet2RegHojaCCTTCCentralCodi(regHojaCCTTCDTO.Centralcodi);
            regHojaCCTTCDTO.Det1RegHojaCCTTCDTO = Lista1DTO;
            regHojaCCTTCDTO.Det2RegHojaCCTTCDTO = Lista2DTO;
            return Json(new { success = true, responseResult = regHojaCCTTCDTO }, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetSubEstacionesA(int id)
        {
            SubestFicha1DTO regHojaa = campaniasAppService.GetSubestFicha1ById(id);
            List<SubestFicha1Det1DTO> Lista1DTO = campaniasAppService.GetSubestFicha1Det1ById(regHojaa.SubestFicha1Codi);
            List<SubestFicha1Det2DTO> Lista2DTO = campaniasAppService.GetSubestFicha1Det2ById(regHojaa.SubestFicha1Codi);
            List<SubestFicha1Det3DTO> Lista3DTO = campaniasAppService.GetSubestFicha1Det3ById(regHojaa.SubestFicha1Codi);
            regHojaa.Lista1DTOs = Lista1DTO;
            regHojaa.Lista2DTOs = Lista2DTO;
            regHojaa.Lista3DTOs = Lista3DTO;
            return Json(new { success = true, responseResult = regHojaa }, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetT2SubEstacionesA(int id)
        {
            T2SubestFicha1DTO regHojaa = campaniasAppService.GetT2SubestFicha1ById(id);
            List<T2SubestFicha1Det1DTO> Lista1DTO = campaniasAppService.GetT2SubestFicha1Det1ById(regHojaa.SubestFicha1Codi);
            List<T2SubestFicha1Det2DTO> Lista2DTO = campaniasAppService.GetT2SubestFicha1Det2ById(regHojaa.SubestFicha1Codi);
            List<T2SubestFicha1Det3DTO> Lista3DTO = campaniasAppService.GetT2SubestFicha1Det3ById(regHojaa.SubestFicha1Codi);
            regHojaa.Lista1DTOs = Lista1DTO;
            regHojaa.Lista2DTOs = Lista2DTO;
            regHojaa.Lista3DTOs = Lista3DTO;
            return Json(new { success = true, responseResult = regHojaa }, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetCuestionarioH2A(int id)
        {
            CuestionarioH2VADTO regHojaa = campaniasAppService.GetCuestionarioH2VAById(id);
            List<CuestionarioH2VADet1DTO> Lista1DTO = campaniasAppService.GetCuestionarioH2VADet1ById(regHojaa.H2vaCodi);
            List<CuestionarioH2VADet2DTO> Lista2DTO = campaniasAppService.GetCuestionarioH2VADet2ById(regHojaa.H2vaCodi);
            regHojaa.ListCH2VADet1DTOs = Lista1DTO;
            regHojaa.ListCH2VADet2DTOs = Lista2DTO;
            return Json(new { success = true, responseResult = regHojaa }, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetCuestionarioH2B(int id)
        {
            CuestionarioH2VBDTO regHojaa = campaniasAppService.GetCuestionarioH2VBById(id);
            return Json(new { success = true, responseResult = regHojaa }, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetCuestionarioH2C(int id)
        {
            List<CuestionarioH2VCDTO> regHojaC = campaniasAppService.GetCuestionarioH2VCById(id);
            return Json(new { success = true, responseResult = regHojaC }, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetCuestionarioH2E(int id)
        {
            List<CuestionarioH2VEDTO> regHojaE = campaniasAppService.GetCuestionarioH2VEById(id);
            return Json(new { success = true, responseResult = regHojaE }, JsonRequestBehavior.AllowGet);
        }


     


        [System.Web.Mvc.HttpPost]
        public JsonResult GetEolHojaA(int id)
        {
            RegHojaEolADTO regHojaEolADTO = campaniasAppService.GetRegHojaEolAById(id);
            List<RegHojaEolADetDTO> regHojaEolADetDTOs = campaniasAppService.GetRegHojaEolADetCodi(regHojaEolADTO.CentralACodi);
            regHojaEolADTO.RegHojaEolADetDTOs = regHojaEolADetDTOs;
            return Json(new { success = true, responseResult = regHojaEolADTO }, JsonRequestBehavior.AllowGet);
        }


        [System.Web.Mvc.HttpPost]
        public JsonResult GetEolHojaB(int id)
        {
            RegHojaEolBDTO regHojaEolBDTO = campaniasAppService.GetRegHojaEolBById(id);
            return Json(new { success = true, responseResult = regHojaEolBDTO }, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetEolHojaC(int id)
        {
            RegHojaEolCDTO regHojaEolCDTO = campaniasAppService.GetRegHojaEolCById(id);
            //List<RegHojaEolADetDTO> listaDTO = campaniasAppService.GetRegHojaEolADetCodi(regHojaEolCDTO.CentralCCodi);
            regHojaEolCDTO.DetRegHojaEolCDTO = campaniasAppService.GetDetRegHojaEolCCodi(regHojaEolCDTO.CentralCCodi);
            return Json(new { success = true, responseResult = regHojaEolCDTO }, JsonRequestBehavior.AllowGet);
        }


        [System.Web.Mvc.HttpPost]
        public JsonResult GetSolHojaA(int id)
        {
            SolHojaADTO solHojaADTO = campaniasAppService.GetSolHojaAById(id);
            return Json(new { success = true, responseResult = solHojaADTO }, JsonRequestBehavior.AllowGet);
        }


        [System.Web.Mvc.HttpPost]
        public JsonResult GetSolHojaB(int id)
        {
            SolHojaBDTO solHojaBDTO = campaniasAppService.GetSolHojaBById(id);
            return Json(new { success = true, responseResult = solHojaBDTO }, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetSolHojaC(int id)
        {
            SolHojaCDTO solHojaCDTO = campaniasAppService.GetSolHojaCById(id);
            solHojaCDTO.ListaDetSolHojaCDTO = campaniasAppService.GetDetSolHojaCCodi(solHojaCDTO.Solhojaccodi);
            return Json(new { success = true, responseResult = solHojaCDTO }, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetBioHojaA(int id)
        {
            BioHojaADTO bioHojaADTO = campaniasAppService.GetBioHojaAById(id);
            return Json(new { success = true, responseResult = bioHojaADTO }, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetCCGDA(int id)
        {
            CCGDADTO ccgdaDTO = campaniasAppService.GetCcgdaById(id);
            return Json(new { success = true, responseResult = ccgdaDTO }, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetCCGDB(int id)
        {
            List<CCGDBDTO> ccgdbDTO = campaniasAppService.GetCcgdbById(id);
            return Json(new { success = true, responseResult = ccgdbDTO }, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetCCGDC(int id)
        {
            List<CCGDCOptDTO> cCGDCOptDTOs = campaniasAppService.GetCcgdcOptById(id);
            List<CCGDCPesDTO> cCGDCPesDTOs = campaniasAppService.GetCcgdcPesById(id);
            return Json(new { success = true, responseResult1 = cCGDCOptDTOs, responseResult2 = cCGDCPesDTOs }, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetCCGDD(int id)
        {
            List<CCGDDDTO> regHojaE = campaniasAppService.GetCcgddById(id);
            return Json(new { success = true, responseResult = regHojaE }, JsonRequestBehavior.AllowGet);
        }


        [System.Web.Mvc.HttpPost]
        public JsonResult GetCCGDE(int id)
        {
            CCGDEDTO ccgdaDTO = campaniasAppService.GetCcgdeById(id);
            return Json(new { success = true, responseResult = ccgdaDTO }, JsonRequestBehavior.AllowGet);
        }


        [System.Web.Mvc.HttpPost]
        public JsonResult GetCCGDF(int id)
        {
            List<CCGDFDTO> ccgdfDTO = campaniasAppService.GetCcgdfById(id);
            return Json(new { success = true, responseResult = ccgdfDTO }, JsonRequestBehavior.AllowGet);
        }



        [System.Web.Mvc.HttpPost]
        public JsonResult GetBioHojaB(int id)
        {
            BioHojaBDTO bioHojaBDTO = campaniasAppService.GetBioHojaBById(id);
            return Json(new { success = true, responseResult = bioHojaBDTO }, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetBioHojaC(int id)
        {
            BioHojaCDTO bioHojaCDTO = campaniasAppService.GetBioHojaCById(id);
            bioHojaCDTO.ListaDetBioHojaCDTO = campaniasAppService.GetDetBioHojaCCodi(bioHojaCDTO.Biohojaccodi);
            return Json(new { success = true, responseResult = bioHojaCDTO }, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetItcdf104(int id)
        {
            List<Itcdf104DTO> itcdf104DTO = campaniasAppService.GetItcdf104ById(id);
            return Json(new { success = true, responseResult = itcdf104DTO }, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetItcdf108(int id)
        {
            List<Itcdf108DTO> itcdf108DTO = campaniasAppService.GetItcdf108ById(id);
            return Json(new { success = true, responseResult = itcdf108DTO }, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetItcdf110(int id)
        {
            List<Itcdf110DTO> itcdf110DTOs = campaniasAppService.GetItcdf110ById(id);
            for (int i = 0; i < itcdf110DTOs.Count; i++)
            {
                itcdf110DTOs[i].ListItcdf110Det = campaniasAppService.GetItcdf110DetCodi(itcdf110DTOs[i].Itcdf110Codi);
            }
            return Json(new { success = true, responseResult = itcdf110DTOs }, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetItcdf116(int id)
        {
            List<Itcdf116DTO> itcdf116DTOs = campaniasAppService.GetItcdf116ById(id);
            for (int i = 0; i < itcdf116DTOs.Count; i++)
            {
                itcdf116DTOs[i].ListItcdf116Det = campaniasAppService.GetItcdf116DetCodi(itcdf116DTOs[i].Itcdf116Codi);
            }
            return Json(new { success = true, responseResult = itcdf116DTOs }, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetItcdf121(int id)
        {
            List<Itcdf121DTO> itcdf121DTOs = campaniasAppService.GetItcdf121ById(id);
            for (int i = 0; i < itcdf121DTOs.Count; i++)
            {
                itcdf121DTOs[i].ListItcdf121Det = campaniasAppService.GetItcdf121DetCodi(itcdf121DTOs[i].Itcdf121Codi);
            }
            return Json(new { success = true, responseResult = itcdf121DTOs }, JsonRequestBehavior.AllowGet);
        }


        [System.Web.Mvc.HttpPost]
        public JsonResult GetItcdf123(int id)
        {
            List<Itcdf123DTO> itcdf123DTOs = campaniasAppService.GetItcdf123ById(id);
            return Json(new { success = true, responseResult = itcdf123DTOs }, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetItcdf012(int id)
        {
            List<Itcdfp012DTO> itcdfp012DTOs = campaniasAppService.GetItcdfp012ById(id);
            return Json(new { success = true, responseResult = itcdfp012DTOs }, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetItcdf013(int id)
        {
            List<Itcdfp013DTO> Itcdfp013DTOs = campaniasAppService.GetItcdfp013ById(id);
            for (int i = 0; i < Itcdfp013DTOs.Count; i++)
            {
                Itcdfp013DTOs[i].ListItcdf013Det = campaniasAppService.GetItcdfp013DetCodi(Itcdfp013DTOs[i].Itcdfp013Codi);
            }
            return Json(new { success = true, responseResult = Itcdfp013DTOs }, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetItcPrm1(int id)
        {
            List<ItcPrm1Dto> itcPrm1Dtos = campaniasAppService.GetItcprm1ById(id);
            return Json(new { success = true, responseResult = itcPrm1Dtos }, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetItcPrm2(int id)
        {
            List<ItcPrm2Dto> itcPrm2Dtos = campaniasAppService.GetItcprm2ById(id);
            return Json(new { success = true, responseResult = itcPrm2Dtos }, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetItcRed1(int id)
        {
            List<ItcRed1Dto> itcRed1Dto = campaniasAppService.GetItcred1ById(id);
            return Json(new { success = true, responseResult = itcRed1Dto }, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetItcRed2(int id)
        {
            List<ItcRed2Dto> itcRed2Dto = campaniasAppService.GetItcred2ById(id);
            return Json(new { success = true, responseResult = itcRed2Dto }, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetItcRed3(int id)
        {
            List<ItcRed3Dto> itcRed3Dto = campaniasAppService.GetItcred3ById(id);
            return Json(new { success = true, responseResult = itcRed3Dto }, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetItcRed4(int id)
        {
            List<ItcRed4Dto> itcRed4Dto = campaniasAppService.GetItcred4ById(id);
            return Json(new { success = true, responseResult = itcRed4Dto }, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetItcRed5(int id)
        {
            List<ItcRed5Dto> itcRed5Dto = campaniasAppService.GetItcred5ById(id);
            return Json(new { success = true, responseResult = itcRed5Dto }, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetCH2VF(int id)
        {
            CuestionarioH2VFDTO CH2VFDTO = campaniasAppService.GetCuestionarioH2VFById(id);
            return Json(new { success = true, responseResult = CH2VFDTO }, JsonRequestBehavior.AllowGet);
        }


        [System.Web.Mvc.HttpPost]
        public JsonResult GetCH2VG(int id)
        {
            List<CuestionarioH2VGDTO> CH2VGDTO = campaniasAppService.GetCuestionarioH2VGById(id);
            return Json(new { success = true, responseResult = CH2VGDTO }, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetCroFicha1(int id)
        {
            CroFicha1DTO croFicha1DTO = campaniasAppService.GetCroFicha1ById(id);
            croFicha1DTO.ListaCroFicha1DetDTO = campaniasAppService.GetCroFicha1DetCodi(croFicha1DTO.CroFicha1codi);
            return Json(new { success = true, responseResult = croFicha1DTO }, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetLFichaA(int id)
        {
            LineasFichaADTO lineasFichaADTO = campaniasAppService.GetLineasFichaAById(id);
            lineasFichaADTO.LineasFichaADet1DTO = campaniasAppService.GetLineasFichaADet1Codi(lineasFichaADTO.LinFichaACodi);
            lineasFichaADTO.LineasFichaADet2DTO = campaniasAppService.GetLineasFichaADet2Codi(lineasFichaADTO.LinFichaACodi);
            return Json(new { success = true, responseResult = lineasFichaADTO }, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetT1LFichaA(int id)
        {
            T1LinFichaADTO lineasFichaADTO = campaniasAppService.GetLineasT1FichaAById(id);
            lineasFichaADTO.LineasFichaADet1DTO = campaniasAppService.GetT1LineasFichaADet1Codi(lineasFichaADTO.LinFichaACodi);
            lineasFichaADTO.LineasFichaADet2DTO = campaniasAppService.GetT1LineasFichaADet2Codi(lineasFichaADTO.LinFichaACodi);
            return Json(new { success = true, responseResult = lineasFichaADTO }, JsonRequestBehavior.AllowGet);
        }


        [System.Web.Mvc.HttpPost]
        public JsonResult GetLFichaB(int id)
        {
            LineasFichaBDTO lineasFichaBDTO = campaniasAppService.GetLineasFichaBById(id);
            lineasFichaBDTO.LineasFichaBDetDTO = campaniasAppService.GetLineasFichaBDetCodi(lineasFichaBDTO.FichaBCodi);
            return Json(new { success = true, responseResult = lineasFichaBDTO }, JsonRequestBehavior.AllowGet);
        }


        [System.Web.Mvc.HttpPost]
        public JsonResult GetDFormatoA(int id)
        {
            FormatoD1ADTO formatoD1ADTO = campaniasAppService.GetFormatoD1AById(id);
            formatoD1ADTO.ListaFormatoDet1A = campaniasAppService.GetFormatoD1ADET1ByProyCodi(formatoD1ADTO.FormatoD1ACodi);
            formatoD1ADTO.ListaFormatoDet2A = campaniasAppService.GetFormatoD1ADET2ByProyCodi(formatoD1ADTO.FormatoD1ACodi);
            formatoD1ADTO.ListaFormatoDet3A = campaniasAppService.GetFormatoD1ADET3ByProyCodi(formatoD1ADTO.FormatoD1ACodi);
            formatoD1ADTO.ListaFormatoDet4A = campaniasAppService.GetFormatoD1ADET4ByProyCodi(formatoD1ADTO.FormatoD1ACodi);
            formatoD1ADTO.ListaFormatoDet5A = campaniasAppService.GetFormatoD1ADET5ByProyCodi(formatoD1ADTO.FormatoD1ACodi);
            return Json(new { success = true, responseResult = formatoD1ADTO }, JsonRequestBehavior.AllowGet);
        }


        [System.Web.Mvc.HttpPost]
        public JsonResult GetDFormatoB(int id)
        {
            FormatoD1BDTO formatoD1BDTO = campaniasAppService.GetFormatoD1BById(id);
            formatoD1BDTO.ListaFormatoDet1B = campaniasAppService.GetFormatoD1BDetByCodi(formatoD1BDTO.FormatoD1BCodi);
            return Json(new { success = true, responseResult = formatoD1BDTO }, JsonRequestBehavior.AllowGet);
        }



        [System.Web.Mvc.HttpPost]
        public JsonResult GetDFormatoC(int id)
        {
            FormatoD1CDTO formatoD1CDTO = campaniasAppService.GetFormatoD1CById(id);
            formatoD1CDTO.ListaFormatoDe1CDet = campaniasAppService.GetFormatoD1CDetCCodi(formatoD1CDTO.FormatoD1CCodi);
            return Json(new { success = true, responseResult = formatoD1CDTO }, JsonRequestBehavior.AllowGet);
        }


        [System.Web.Mvc.HttpPost]
        public JsonResult GetDFormatoD(int id)
        {
            List<FormatoD1DDTO> regDFormatoD = campaniasAppService.GetFormatoD1DById(id);
            return Json(new { success = true, responseResult = regDFormatoD }, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetItcdf011(int id, int offset, int pageSize)
        {
            Itcdfp011DTO Itcdfp011DTOs = campaniasAppService.GetItcdfp011ById(id);
            Itcdfp011DTOs.ListItcdf011Det = campaniasAppService.GetItcdfp011DetByIdPag(Itcdfp011DTOs.Itcdfp011Codi, offset, pageSize);
            return Json(new { success = true, responseResult = Itcdfp011DTOs }, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetItcdf011Det(int id, int offset, int pageSize)
        {
            List<Itcdfp011DetDTO> ListItcdf011Det = campaniasAppService.GetItcdfp011DetByIdPag(id, offset, pageSize);
            return Json(new { success = true, responseResult = ListItcdf011Det }, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetITCFE01(int id)
        {
            ITCFE01DTO result = campaniasAppService.GetRegITCFE01ById(id);
            return Json(new { success = true, responseResult = result }, JsonRequestBehavior.AllowGet);
        }

    }

}
