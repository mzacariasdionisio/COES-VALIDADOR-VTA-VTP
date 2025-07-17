using COES.Dominio.DTO.Campania;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Campanias.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.Campanias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Http;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Areas.Campanias.Helper;
using COES.Servicios.Aplicacion.CalculoResarcimientos;
using COES.Servicios.Aplicacion.PMPO.Helper;
using COES.Framework.Base.Tools;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using System.IO.Compression;

namespace COES.MVC.Intranet.Areas.Campanias.Controllers
{
    public class PlanTransmisionController : BaseController
    {
        CampaniasAppService campaniasAppService = new CampaniasAppService();
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            CampaniasModel model = new CampaniasModel();
            return View(model);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Listado(string empresa, string estado, string periodo, string vigente, string fueraplazo, string estadoExcl)
        {
            CampaniasModel model = new CampaniasModel();
            List<int> idsEmpresas;
            if (empresa.Equals(""))
            {
                idsEmpresas = this.seguridad
                .ObtenerEmpresasPorUsuario(User.Identity.Name)
                .Select(x => (int)x.EMPRCODI)
                .ToList();
            }
            else
            {
                idsEmpresas = new List<int> { int.Parse(empresa) };
            }
            string empresas = string.Join<int>(ConstantesCampania.CaracterComa, idsEmpresas);
            model.ListaPlanTransmicion = campaniasAppService.GetPlanTransmisionByEstado(empresas, estado, periodo, vigente, fueraplazo, estadoExcl);
            return View(model);
        }

        public ActionResult Envio(int? id)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            CampaniasModel model = new CampaniasModel();
            if (id != null)
            { 
                model.Plancodi = id.Value;
            }
            return View(model);
        }

        public ActionResult RevisarEnvio(int? id)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            CampaniasModel model = new CampaniasModel();
            if (id != null)
            {
                model.Plancodi = id.Value;
            }
            return View(model);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult ListadoProyecto(int id)
        {
            List<TransmisionProyectoDTO> listaProyecto = campaniasAppService.GetTransmisionProyecto(id);
            ProyectoModel model = new ProyectoModel();
            model.listaProyecto = listaProyecto;
            return View(model);
        }
        
        [System.Web.Http.HttpGet]
        public virtual ActionResult DescargarExcel()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + ConstantesCampanias.FolderFichas + ConstantesCampanias.FolderReporte+ FormatoArchivosExcelCampanias.NombrePlanTransmisionNew;
            return File(fullPath, Constantes.AppExcel, FormatoArchivosExcelCampanias.NombrePlanTransmisionNew);
        }

        [System.Web.Http.HttpPost]
        public JsonResult ExportarExcel(string empresa, string estado, string periodo, string vigente, string fueraplazo, string estadoExcl)
        {
            int result = 1;

            try
            {
                List<PlanTransmisionDTO> listaProyecto = new List<PlanTransmisionDTO>();
                listaProyecto = campaniasAppService.GetPlanTransmisionByEstado(empresa, estado, periodo, vigente, fueraplazo, estadoExcl);
                ExcelDocument.GenerarExcelPlanTransmision(listaProyecto);
            }
            catch (Exception ex)
            {
                result = -1;
            }
            return Json(result);
        }


        [System.Web.Mvc.HttpPost]
        public JsonResult DescargarProyecto(string idproyectos)
        {
            int indicador = 1;
            List<int> tipoProyecto = new List<int>();
            string identificadorUnico = "";
            try
            {
                List<DataSubestacionDTO> subestaciones = campaniasAppService.ListParamSubestacion();

                // Mostrar las subestaciones en la consola del servidor para depuración
                Console.WriteLine("Catálogo de subestaciones cargado:");
                foreach (var sub in subestaciones)
                {
                    Console.WriteLine($"ID: {sub.Equicodi}, Nombre: {sub.Equinomb}");
                }

                
                if (idproyectos.Length > 0)
                {
                    string[] arrayIdProyectos = idproyectos.Split(',');
                    if (arrayIdProyectos.Length > 0)
                    {
 
                        string ruta = AppDomain.CurrentDomain.BaseDirectory;
                        var nombreTemp = ConstantesCampanias.FolderTemp;
                        string path = Path.Combine(ruta, ConstantesCampanias.FolderFichas);

                        string fechaHora = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                        string uid = Guid.NewGuid().ToString().Substring(0, 8); // puedes usar completo si prefieres
                        identificadorUnico = $"uid_{fechaHora}_{uid}";

                        string pathFichasTemp = Path.Combine(path, nombreTemp, identificadorUnico);

                        string pathOrigen = campaniasAppService.ObtenerPathArchivosCampianas();

                        //borrar carpeta temporal cuando existan registros
                        FileServer.CreateFolder("", nombreTemp, path);
                        //if (FileServer.VerificarExistenciaDirectorio(nombreTemp + "/", path))
                        //{
                        //    FileServer.DeleteFolderAlter(nombreTemp, path);
                        //    FileServer.CreateFolder("", nombreTemp, path);
                        //}
                        foreach (string strProyecto in arrayIdProyectos)
                        {

                            int idProyecto = Convert.ToInt16(strProyecto);
                            string pathFichaTipo = "";
                            string pathFichaSubTipo = "";
                            string subtipo = "";
                            TransmisionProyectoDTO tranmsProyecto = campaniasAppService.GetTransmisionProyectoById(idProyecto);
                            tranmsProyecto.Periodo = campaniasAppService.GetPeriodoDTOById(tranmsProyecto.Pericodi);
                            // CATALOGO
                            List<DataCatalogoDTO> dataCatalogoDTOs = campaniasAppService.ListParametriaAll();
                            tranmsProyecto.DataCatalogoDTOs = dataCatalogoDTOs;

                            tipoProyecto.Add(tranmsProyecto.Tipocodi);
                            if (tranmsProyecto.Tipocodi == TipoProyecto.Generacion)
                            {
                                pathFichaTipo = Path.Combine(pathFichasTemp, ConstantesCampanias.FolderFichasGeneracion);
                                //if (!FileServer.VerificarExistenciaDirectorio(ConstantesCampanias.FolderFichasGeneracion, pathFichasTemp))
                                //    FileServer.CreateFolder("", ConstantesCampanias.FolderFichasGeneracion, pathFichasTemp);
                                // tranmsProyecto.Proynombre = tranmsProyecto.Proynombre + "-" +tranmsProyecto.Proycodi ;
                                if (tranmsProyecto.Tipoficodi == SubTipoProyecto.CentralHidroeléctrica) 
                                {
                                    pathFichaSubTipo = Path.Combine(pathFichaTipo, ConstantesCampanias.FolderFichasGeneracionCHidro);
                                    subtipo = ConstantesCampanias.FolderFichasGeneracionCHidro;
                                    //if (!FileServer.VerificarExistenciaDirectorio(ConstantesCampanias.FolderFichasGeneracionCHidro, pathFichaTipo)) FileServer.CreateFolder("", ConstantesCampanias.FolderFichasGeneracionCHidro, pathFichaTipo);
                                    if (!FileServer.VerificarExistenciaDirectorio(tranmsProyecto.Proynombre, pathFichaSubTipo)) FileServer.CreateFolder("", tranmsProyecto.Proynombre, pathFichaSubTipo);
                                    descargarProyectoTipoGeneracionCentralHidro(tranmsProyecto,identificadorUnico);
                                } 
                                else if (tranmsProyecto.Tipoficodi == SubTipoProyecto.CentralTermoeléctrica) 
                                {
                                    pathFichaSubTipo = pathFichaTipo + ConstantesCampanias.FolderFichasGeneracionCTermo;
                                    subtipo = ConstantesCampanias.FolderFichasGeneracionCTermo;
                                    //if (!FileServer.VerificarExistenciaDirectorio(ConstantesCampanias.FolderFichasGeneracionCTermo, pathFichaTipo)) FileServer.CreateFolder("", ConstantesCampanias.FolderFichasGeneracionCTermo, pathFichaTipo);
                                    if (!FileServer.VerificarExistenciaDirectorio(tranmsProyecto.Proynombre, pathFichaSubTipo)) FileServer.CreateFolder("", tranmsProyecto.Proynombre, pathFichaSubTipo);
                                    descargarProyectoTipoGeneracionCentralTermo(tranmsProyecto, subestaciones, identificadorUnico);
                                } 
                                else if (tranmsProyecto.Tipoficodi == SubTipoProyecto.CentralEólica) 
                                {
                                    pathFichaSubTipo = pathFichaTipo + ConstantesCampanias.FolderFichasGeneracionCEolica;
                                    subtipo = ConstantesCampanias.FolderFichasGeneracionCEolica;
                                    //if (!FileServer.VerificarExistenciaDirectorio(ConstantesCampanias.FolderFichasGeneracionCEolica, pathFichaTipo)) FileServer.CreateFolder("", ConstantesCampanias.FolderFichasGeneracionCEolica, pathFichaTipo);
                                    if (!FileServer.VerificarExistenciaDirectorio(tranmsProyecto.Proynombre, pathFichaSubTipo)) FileServer.CreateFolder("", tranmsProyecto.Proynombre, pathFichaSubTipo);
                                    descargarProyectoTipoGeneracionCentralEolica(tranmsProyecto, subestaciones, identificadorUnico);
                                } 
                                else if(tranmsProyecto.Tipoficodi == SubTipoProyecto.CentralSolar) 
                                {
                                    pathFichaSubTipo = pathFichaTipo + ConstantesCampanias.FolderFichasGeneracionCSolar;
                                    subtipo = ConstantesCampanias.FolderFichasGeneracionCSolar;
                                    if (!FileServer.VerificarExistenciaDirectorio(ConstantesCampanias.FolderFichasGeneracionCSolar, pathFichaTipo)) FileServer.CreateFolder("", ConstantesCampanias.FolderFichasGeneracionCSolar, pathFichaTipo);
                                    if (!FileServer.VerificarExistenciaDirectorio(tranmsProyecto.Proynombre, pathFichaSubTipo)) FileServer.CreateFolder("", tranmsProyecto.Proynombre, pathFichaSubTipo);
                                    descargarProyectoTipoGeneracionCentralSolar(tranmsProyecto, identificadorUnico);
                                }
                                else if (tranmsProyecto.Tipoficodi == SubTipoProyecto.CentralBiomasa)
                                {
                                    pathFichaSubTipo = pathFichaTipo + ConstantesCampanias.FolderFichasGeneracionCBiom;
                                    subtipo = ConstantesCampanias.FolderFichasGeneracionCBiom;
                                    if (!FileServer.VerificarExistenciaDirectorio(ConstantesCampanias.FolderFichasGeneracionCBiom, pathFichaTipo)) FileServer.CreateFolder("", ConstantesCampanias.FolderFichasGeneracionCBiom, pathFichaTipo);
                                    if (!FileServer.VerificarExistenciaDirectorio(tranmsProyecto.Proynombre, pathFichaSubTipo)) FileServer.CreateFolder("", tranmsProyecto.Proynombre, pathFichaSubTipo);
                                    descargarProyectoTipoGeneracionCentralBiom(tranmsProyecto, subestaciones, identificadorUnico);
                                }   
                               if(!string.IsNullOrEmpty(subtipo))
                                {
                                    if (!FileServer.VerificarExistenciaDirectorio(ConstantesCampanias.FolderFichasGeneracionSubestaciones, pathFichaSubTipo + tranmsProyecto.Proynombre)) FileServer.CreateFolder("", ConstantesCampanias.FolderFichasGeneracionSubestaciones, pathFichaSubTipo + tranmsProyecto.Proynombre + "\\");
                                    descargarProyectoTipoGeneracionSubestacion(tranmsProyecto, subtipo, identificadorUnico);
                                    if (!FileServer.VerificarExistenciaDirectorio(ConstantesCampanias.FolderFichasGeneracionLineas, pathFichaSubTipo + tranmsProyecto.Proynombre)) FileServer.CreateFolder("", ConstantesCampanias.FolderFichasGeneracionLineas, pathFichaSubTipo + tranmsProyecto.Proynombre + "\\");
                                    descargarProyectoTipoGeneracionLinea(tranmsProyecto, subtipo, identificadorUnico);
                               }
                            }
                            else if (tranmsProyecto.Tipocodi == TipoProyecto.Transmision)
                            {
                                pathFichaTipo = Path.Combine(pathFichasTemp, ConstantesCampanias.FolderFichasTransmision);
                                //if (!FileServer.VerificarExistenciaDirectorio(ConstantesCampanias.FolderFichasTransmision, pathFichasTemp)) FileServer.CreateFolder("", ConstantesCampanias.FolderFichasTransmision, pathFichasTemp);
                                if (!FileServer.VerificarExistenciaDirectorio(tranmsProyecto.Proynombre, pathFichaTipo)) FileServer.CreateFolder("", tranmsProyecto.Proynombre, pathFichaTipo);
                                descargarProyectoTipoTransmisionLinea(tranmsProyecto, identificadorUnico);
                                descargarProyectoTipoTransmisionSubestacion(tranmsProyecto, identificadorUnico);
                                descargarProyectoTipoTransmisionCronograma(tranmsProyecto, identificadorUnico);
                            }
                            else if (tranmsProyecto.Tipocodi == TipoProyecto.ITC)
                            {
                                pathFichaTipo = Path.Combine(pathFichasTemp, ConstantesCampanias.FolderFichasITC);
                                //if (!FileServer.VerificarExistenciaDirectorio(ConstantesCampanias.FolderFichasITC, pathFichasTemp)) FileServer.CreateFolder("", ConstantesCampanias.FolderFichasITC, pathFichasTemp);
                                if (!FileServer.VerificarExistenciaDirectorio(tranmsProyecto.Proynombre, pathFichaTipo)) FileServer.CreateFolder("", tranmsProyecto.Proynombre, pathFichaTipo);
                                descargarProyectoTipoITCSistemaElectrico(tranmsProyecto, identificadorUnico);
                                descargarProyectoTipoITCDemanda(tranmsProyecto, identificadorUnico);
                            }
                            else if (tranmsProyecto.Tipocodi == TipoProyecto.Demanda)
                            {
                                pathFichaTipo = Path.Combine(pathFichasTemp, ConstantesCampanias.FolderFichasDemanda);
                                //if (!FileServer.VerificarExistenciaDirectorio(ConstantesCampanias.FolderFichasDemanda, pathFichasTemp)) FileServer.CreateFolder("", ConstantesCampanias.FolderFichasDemanda, pathFichasTemp);
                                if (!FileServer.VerificarExistenciaDirectorio(tranmsProyecto.Proynombre, pathFichaTipo)) FileServer.CreateFolder("", tranmsProyecto.Proynombre, pathFichaTipo);
                                descargarProyectoTipoDemanda(tranmsProyecto, subestaciones, identificadorUnico);
                            }
                            else if (tranmsProyecto.Tipocodi == TipoProyecto.GeneracionDistribuida)
                            {
                                pathFichaTipo = Path.Combine(pathFichasTemp, ConstantesCampanias.FolderFichasGeneracionDistribuida);
                                //if (!FileServer.VerificarExistenciaDirectorio(ConstantesCampanias.FolderFichasGeneracionDistribuida, pathFichasTemp)) FileServer.CreateFolder("", ConstantesCampanias.FolderFichasGeneracionDistribuida, pathFichasTemp);
                                if (!FileServer.VerificarExistenciaDirectorio(tranmsProyecto.Proynombre, pathFichaTipo)) FileServer.CreateFolder("", tranmsProyecto.Proynombre, pathFichaTipo);
                                descargarProyectoTipoGeneracionDistribuida(tranmsProyecto, identificadorUnico);
                            }
                            else if (tranmsProyecto.Tipocodi == TipoProyecto.HidrogenoVerde)
                            {
                                pathFichaTipo = Path.Combine(pathFichasTemp, ConstantesCampanias.FolderFichasHidrogenoVerde);
                                //if (!FileServer.VerificarExistenciaDirectorio(ConstantesCampanias.FolderFichasHidrogenoVerde, pathFichasTemp)) FileServer.CreateFolder("", ConstantesCampanias.FolderFichasHidrogenoVerde, pathFichasTemp);
                                if (!FileServer.VerificarExistenciaDirectorio(tranmsProyecto.Proynombre, pathFichaTipo)) FileServer.CreateFolder("", tranmsProyecto.Proynombre, pathFichaTipo);
                                descargarProyectoTipoHidrogenoVerde(tranmsProyecto, identificadorUnico);
                            }
                           if (!string.IsNullOrEmpty(pathFichaTipo)) {
                                string pathFichaProyecto;
                                
                                if (tranmsProyecto.Tipocodi == TipoProyecto.Generacion) {
                                    // Usamos Path.Combine para construir la ruta de forma segura
                                    pathFichaProyecto = Path.Combine(pathFichaTipo, pathFichaSubTipo, tranmsProyecto.Proynombre);
                                } else {
                                    pathFichaProyecto = Path.Combine(pathFichaTipo, tranmsProyecto.Proynombre);
                                }
                                
                                // Validar que el nombre del proyecto no contiene caracteres inválidos
                                if (tranmsProyecto.Proynombre.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0) {
                                    throw new ArgumentException("El nombre del proyecto contiene caracteres inválidos.");
                                }
                                
                                // Verificar si la carpeta del proyecto existe, si no, crearla
                                if (!Directory.Exists(pathFichaProyecto)) {
                                    Directory.CreateDirectory(pathFichaProyecto);
                                }

                                // Llamada a la función de descarga de archivos
                                descargarArchivosFichas(idProyecto, (pathFichaProyecto+"\\"), pathOrigen, identificadorUnico);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                indicador = -1;
                throw ex;
            }
            return Json(new { tipos = tipoProyecto, uid = identificadorUnico });

        }

        public void descargarProyectoTipoGeneracionCentralHidro(TransmisionProyectoDTO proyecto,string identificadorUnico)
        {
            ProyectoModel model = new ProyectoModel();
            model.TransmisionProyectoDTO = proyecto;
            List<DataCatalogoDTO> ListCatalogo = proyecto.DataCatalogoDTOs;
            DataCatalogoDTO catalogo;
            // FICHA A
            RegHojaADTO regHojaADTO = campaniasAppService.GetRegHojaAById(proyecto.Proycodi);
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catPropietario && c.Valor == regHojaADTO.Propietario);
            if (catalogo != null)
            {
                regHojaADTO.Propietario = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catConcesionTemporal && c.Valor == regHojaADTO.Concesiontemporal);
            if (catalogo != null)
            {
                regHojaADTO.Concesiontemporal = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catConcesionActual && c.Valor == regHojaADTO.Tipoconcesionactual);
            if (catalogo != null)
            {
                regHojaADTO.Tipoconcesionactual = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catTipoPPL && c.Valor == regHojaADTO.Tuneltipo);
            if (catalogo != null)
            {
                regHojaADTO.Tuneltipo = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catSE && c.Valor == regHojaADTO.Tuberiatipo);
            if (catalogo != null)
            {
                regHojaADTO.Tuberiatipo = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catSE && c.Valor == regHojaADTO.Maquinatipo);
            if (catalogo != null)
            {
                regHojaADTO.Maquinatipo = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catPerfil && c.Valor == regHojaADTO.Perfil);
            if (catalogo != null)
            {
                regHojaADTO.Perfil = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catPreFactibilidad && c.Valor == regHojaADTO.Prefactibilidad);
            if (catalogo != null)
            {
                regHojaADTO.Prefactibilidad = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catFactibilidad && c.Valor == regHojaADTO.Factibilidad);
            if (catalogo != null)
            {
                regHojaADTO.Factibilidad = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catEstudDef && c.Valor == regHojaADTO.Estudiodefinitivo);
            if (catalogo != null)
            {
                regHojaADTO.Estudiodefinitivo = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catEia && c.Valor == regHojaADTO.Eia);
            if (catalogo != null)
            {
                regHojaADTO.Eia = catalogo.DescortaDatacat;
            }

            model.RegHojaADTO = regHojaADTO;
            // FICHA B
            RegHojaBDTO regHojaBDTO = campaniasAppService.GetRegHojaBById(proyecto.Proycodi);
            model.RegHojaBDTO = regHojaBDTO;
            // FICHA C
            RegHojaCDTO regHojaCDTO = campaniasAppService.GetRegHojaCById(proyecto.Proycodi);
            List<DetRegHojaCDTO> detRegHojaCDTO = campaniasAppService.GetDetRegHojaCFichaCCodi(regHojaCDTO.Fichaccodi);
            regHojaCDTO.DetRegHojaCs = detRegHojaCDTO;
            model.RegHojaCDTO = regHojaCDTO;
            // FICHA D
            List<RegHojaDDTO> regHojaDDTO = campaniasAppService.GetRegHojaDById(proyecto.Proycodi);
            if (regHojaDDTO != null)
            {
                foreach (var item in regHojaDDTO)
                {
                    item.ListDetRegHojaD = campaniasAppService.GetDetRegHojaDFichaCCodi(item.Hojadcodi);
                }
            }
                    model.ListRegHojaD = regHojaDDTO;
            // CATALOGO
            List<DataCatalogoDTO> dataCatalogoDTOs =  campaniasAppService.ListParametria(CategoriaRequisito.CentralHidroHojaC);
            model.DataCatalogoDTOs = dataCatalogoDTOs;
            //UBICACION
            if (regHojaADTO.Distrito != null && regHojaADTO.Distrito != "") {
                model.ubicacionDTO = campaniasAppService.GetUbicacionByDistrito(regHojaADTO.Distrito);
            }

            ExcelDocument.GenerarExcelFichaProyectoTipoGeneracionCentralHidro(model,identificadorUnico);
        }

        public void descargarProyectoTipoGeneracionCentralTermo(TransmisionProyectoDTO proyecto, List<DataSubestacionDTO> subestaciones, string identificadorUnico)
        {
            ProyectoModel model = new ProyectoModel();
            model.TransmisionProyectoDTO = proyecto;
            List<DataCatalogoDTO> ListCatalogo = proyecto.DataCatalogoDTOs;
            DataCatalogoDTO catalogo;
            // CCTTA
            RegHojaCCTTADTO regHojaCCTTADTO = campaniasAppService.GetRegHojaCCTTAById(proyecto.Proycodi);
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catPropietario && c.Valor == regHojaCCTTADTO.Propietario);
            if (catalogo != null)
            {
                regHojaCCTTADTO.Propietario = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catConcesionActual && c.Valor == regHojaCCTTADTO.Tipoconcesionactual);
            if (catalogo != null)
            {
                regHojaCCTTADTO.Tipoconcesionactual = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catCombustible && c.Valor == regHojaCCTTADTO.Combustibletipo);
            if (catalogo != null)
            {
                regHojaCCTTADTO.Combustibletipo = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catPerfil && c.Valor == regHojaCCTTADTO.Perfil);
            if (catalogo != null)
            {
                regHojaCCTTADTO.Perfil = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catPreFactibilidad && c.Valor == regHojaCCTTADTO.Prefactibilidad);
            if (catalogo != null)
            {
                regHojaCCTTADTO.Prefactibilidad = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catFactibilidad && c.Valor == regHojaCCTTADTO.Factibilidad);
            if (catalogo != null)
            {
                regHojaCCTTADTO.Factibilidad = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catEstudDef && c.Valor == regHojaCCTTADTO.Estudiodefinitivo);
            if (catalogo != null)
            {
                regHojaCCTTADTO.Estudiodefinitivo = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catEia && c.Valor == regHojaCCTTADTO.Eia);
            if (catalogo != null)
            {
                regHojaCCTTADTO.Eia = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catPodCalInf && c.Valor == regHojaCCTTADTO.Undpci);
            if (catalogo != null)
            {
                regHojaCCTTADTO.Undpci = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catPodCalSup && c.Valor == regHojaCCTTADTO.Undpcs);
            if (catalogo != null)
            {
                regHojaCCTTADTO.Undpcs = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catCComb && c.Valor == regHojaCCTTADTO.Undcomb);
            if (catalogo != null)
            {
                regHojaCCTTADTO.Undcomb = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catCTratComb && c.Valor == regHojaCCTTADTO.Undtrtcomb);
            if (catalogo != null)
            {
                regHojaCCTTADTO.Undtrtcomb = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catCTranspComb && c.Valor == regHojaCCTTADTO.Undtrnspcomb);
            if (catalogo != null)
            {
                regHojaCCTTADTO.Undtrnspcomb = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catCVarNComb && c.Valor == regHojaCCTTADTO.Undvarncmb);
            if (catalogo != null)
            {
                regHojaCCTTADTO.Undvarncmb = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catCInvIni && c.Valor == regHojaCCTTADTO.Undinvinic);
            if (catalogo != null)
            {
                regHojaCCTTADTO.Undinvinic = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catRendPl && c.Valor == regHojaCCTTADTO.Undrendcnd);
            if (catalogo != null)
            {
                regHojaCCTTADTO.Undrendcnd = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catConsEspCon && c.Valor == regHojaCCTTADTO.Undconscp);
            // Asignar el nombre de la subestación correspondiente
            if (!string.IsNullOrEmpty(regHojaCCTTADTO.Nombresubestacion))
            {
                var subestacion = subestaciones.FirstOrDefault(s => s.Equicodi.ToString() == regHojaCCTTADTO.Nombresubestacion);
                if (subestacion != null)
                {
                    regHojaCCTTADTO.Nombresubestacion = subestacion.Equinomb;
                }

            }
            if (catalogo != null)
            {
                regHojaCCTTADTO.Undconscp = catalogo.DescortaDatacat;
            }
            model.RegHojaCCTTADTO = regHojaCCTTADTO;
            // CCTTB
            RegHojaCCTTBDTO regHojaCCTTBDTO = campaniasAppService.GetRegHojaCCTTBById(proyecto.Proycodi);
            model.RegHojaCCTTBDTO = regHojaCCTTBDTO;
            // CCTTC
            RegHojaCCTTCDTO regHojaCCTTCDTO = campaniasAppService.GetRegHojaCCTTCById(proyecto.Proycodi);
            List<Det1RegHojaCCTTCDTO> det1RegHojaCCTTCDTO = campaniasAppService.GetDet1RegHojaCCTTCCentralCodi(regHojaCCTTCDTO.Centralcodi);
            regHojaCCTTCDTO.Det1RegHojaCCTTCDTO = det1RegHojaCCTTCDTO;
            List<Det2RegHojaCCTTCDTO> det2RegHojaCCTTCDTO = campaniasAppService.GetDet2RegHojaCCTTCCentralCodi(regHojaCCTTCDTO.Centralcodi);
            regHojaCCTTCDTO.Det2RegHojaCCTTCDTO = det2RegHojaCCTTCDTO;
            model.RegHojaCCTTCDTO = regHojaCCTTCDTO;
            // CATALOGO
            List<DataCatalogoDTO> dataCatalogoDTOs =  campaniasAppService.ListParametria(CategoriaRequisito.CentralTermoHojaC);
            model.DataCatalogoDTOs = dataCatalogoDTOs;
            //UBICACION
            if (regHojaCCTTADTO.Distrito != null && regHojaCCTTADTO.Distrito != "")
            {
                model.ubicacionDTO = campaniasAppService.GetUbicacionByDistrito(regHojaCCTTADTO.Distrito);
            }
            ExcelDocument.GenerarExcelFichaProyectoTipoGeneracionCentralTermo(model,identificadorUnico);
        }

        public void descargarProyectoTipoGeneracionCentralEolica(TransmisionProyectoDTO proyecto, List<DataSubestacionDTO> subestaciones, string identificadorUnico)
        {
            ProyectoModel model = new ProyectoModel();
            model.TransmisionProyectoDTO = proyecto;
            List<DataCatalogoDTO> ListCatalogo = proyecto.DataCatalogoDTOs;
            DataCatalogoDTO catalogo;
            // EOL-A
            RegHojaEolADTO regHojaEolADTO = campaniasAppService.GetRegHojaEolAById(proyecto.Proycodi);
            List<RegHojaEolADetDTO> regHojaEolADetDTOs = campaniasAppService.GetRegHojaEolADetCodi(regHojaEolADTO.CentralACodi);
            regHojaEolADTO.RegHojaEolADetDTOs = regHojaEolADetDTOs;
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catPropietario && c.Valor == regHojaEolADTO.Propietario);
            if (catalogo != null)
            {
                regHojaEolADTO.Propietario = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catConcesionTemporal && c.Valor == regHojaEolADTO.ConcesionTemporal);
            if (catalogo != null)
            {
                regHojaEolADTO.ConcesionTemporal = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catConcesionActual && c.Valor == regHojaEolADTO.TipoConcesionActual);
            if (catalogo != null)
            {
                regHojaEolADTO.TipoConcesionActual = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catSerieVelocidad && c.Valor == regHojaEolADTO.SerieVelViento);
            if (catalogo != null)
            {
                regHojaEolADTO.SerieVelViento = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catEstudioGeol && c.Valor == regHojaEolADTO.EstudioGeologico);
            if (catalogo != null)
            {
                regHojaEolADTO.EstudioGeologico = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catEstudioTopo && c.Valor == regHojaEolADTO.EstudioTopografico);
            if (catalogo != null)
            {
                regHojaEolADTO.EstudioTopografico = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catTipTurbina && c.Valor == regHojaEolADTO.TipoTurbina);
            if (catalogo != null)
            {
                regHojaEolADTO.TipoTurbina = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catTipParqueEol && c.Valor == regHojaEolADTO.TipoParqEolico);
            if (catalogo != null)
            {
                regHojaEolADTO.TipoParqEolico = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catTipGenerador && c.Valor == regHojaEolADTO.TipoTecGenerador);
            if (catalogo != null)
            {
                regHojaEolADTO.TipoTecGenerador = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catPerfil && c.Valor == regHojaEolADTO.Perfil);
            if (catalogo != null)
            {
                regHojaEolADTO.Perfil = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catPreFactibilidad && c.Valor == regHojaEolADTO.Prefactibilidad);
            if (catalogo != null)
            {
                regHojaEolADTO.Prefactibilidad = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catFactibilidad && c.Valor == regHojaEolADTO.Factibilidad);
            if (catalogo != null)
            {
                regHojaEolADTO.Factibilidad = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catEstudDef && c.Valor == regHojaEolADTO.EstudioDefinitivo);
            if (catalogo != null)
            {
                regHojaEolADTO.EstudioDefinitivo = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catEia && c.Valor == regHojaEolADTO.Eia);
            if (catalogo != null)
            {
                regHojaEolADTO.Eia = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catBacterias && c.Valor == regHojaEolADTO.Bess);
            if (catalogo != null)
            {
                regHojaEolADTO.Bess = catalogo.DescortaDatacat;
            }
            // Asignar el nombre de la subestación correspondiente
            if (!string.IsNullOrEmpty(regHojaEolADTO.NombreSubestacion))
            {
                var subestacion = subestaciones.FirstOrDefault(s => s.Equicodi.ToString() == regHojaEolADTO.NombreSubestacion);
                if (subestacion != null)
                {
                    regHojaEolADTO.NombreSubestacion = subestacion.Equinomb;
                }

            }
            model.RegHojaEolADTO = regHojaEolADTO;
            // EOL-B
            RegHojaEolBDTO regHojaEolBDTO = campaniasAppService.GetRegHojaEolBById(proyecto.Proycodi);
            model.RegHojaEolBDTO = regHojaEolBDTO;
            // EOL-C
            RegHojaEolCDTO regHojaEolCDTO = campaniasAppService.GetRegHojaEolCById(proyecto.Proycodi);
            List<DetRegHojaEolCDTO> listaDTO = campaniasAppService.GetDetRegHojaEolCCodi(regHojaEolCDTO.CentralCCodi);
            regHojaEolCDTO.DetRegHojaEolCDTO = listaDTO;
            model.RegHojaEolCDTO = regHojaEolCDTO;
            // CATALOGO
            List<DataCatalogoDTO> dataCatalogoDTOs =  campaniasAppService.ListParametria(CategoriaRequisito.CentralEolHojaC);
            model.DataCatalogoDTOs = dataCatalogoDTOs;
            //UBICACION
            if (regHojaEolADTO.Distrito != null && regHojaEolADTO.Distrito != "")
            {
                model.ubicacionDTO = campaniasAppService.GetUbicacionByDistrito(regHojaEolADTO.Distrito);
            }
            ExcelDocument.GenerarExcelFichaProyectoTipoGeneracionCentralEolica(model,identificadorUnico);
        }

        public void descargarProyectoTipoGeneracionCentralSolar(TransmisionProyectoDTO proyecto, string identificadorUnico)
        {
            ProyectoModel model = new ProyectoModel();
            model.TransmisionProyectoDTO = proyecto;
            List<DataCatalogoDTO> ListCatalogo = proyecto.DataCatalogoDTOs;
            DataCatalogoDTO catalogo;
            // SOL-A
            SolHojaADTO solHojaADTO = campaniasAppService.GetSolHojaAById(proyecto.Proycodi);
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catPropietario && c.Valor == solHojaADTO.Propietario);
            if (catalogo != null)
            {
                solHojaADTO.Propietario = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catConcesionTemporal && c.Valor == solHojaADTO.Concesiontemporal);
            if (catalogo != null)
            {
                solHojaADTO.Concesiontemporal = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catConcesionActual && c.Valor == solHojaADTO.Tipoconcesionact);
            if (catalogo != null)
            {
                solHojaADTO.Tipoconcesionact = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catPerfil && c.Valor == solHojaADTO.Perfil);
            if (catalogo != null)
            {
                solHojaADTO.Perfil = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catPreFactibilidad && c.Valor == solHojaADTO.Prefact);
            if (catalogo != null)
            {
                solHojaADTO.Prefact = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catFactibilidad && c.Valor == solHojaADTO.Factibilidad);
            if (catalogo != null)
            {
                solHojaADTO.Factibilidad = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catEstudDef && c.Valor == solHojaADTO.Estdefinitivo);
            if (catalogo != null)
            {
                solHojaADTO.Estdefinitivo = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catEia && c.Valor == solHojaADTO.Eia);
            if (catalogo != null)
            {
                solHojaADTO.Eia = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catSegSolar && c.Valor == solHojaADTO.Seguidorsol);
            if (catalogo != null)
            {
                solHojaADTO.Seguidorsol = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catRadSolar && c.Valor == solHojaADTO.Serieradiacion);
            if (catalogo != null)
            {
                solHojaADTO.Serieradiacion = catalogo.DescortaDatacat;
            }
            model.SolHojaADTO = solHojaADTO;
            // SOL-B
            SolHojaBDTO solHojaBDTO = campaniasAppService.GetSolHojaBById(proyecto.Proycodi);
            model.SolHojaBDTO = solHojaBDTO;
            // SOL-C
            SolHojaCDTO solHojaCDTO = campaniasAppService.GetSolHojaCById(proyecto.Proycodi);
            solHojaCDTO.ListaDetSolHojaCDTO = campaniasAppService.GetDetSolHojaCCodi(solHojaCDTO.Solhojaccodi);
            model.SolHojaCDTO = solHojaCDTO;
            // CATALOGO
            List<DataCatalogoDTO> dataCatalogoDTOs =  campaniasAppService.ListParametria(CategoriaRequisito.CentralSolarHojaC);
            model.DataCatalogoDTOs = dataCatalogoDTOs;
            //UBICACION
            if (solHojaADTO.Distrito != null && solHojaADTO.Distrito != "")
            {
                model.ubicacionDTO = campaniasAppService.GetUbicacionByDistrito(solHojaADTO.Distrito);
            }
            ExcelDocument.GenerarExcelFichaProyectoTipoGeneracionCentralSolar(model, identificadorUnico);
        }

        public void descargarProyectoTipoGeneracionCentralBiom(TransmisionProyectoDTO proyecto, List<DataSubestacionDTO> subestaciones, string identificadorUnico)
        {
            ProyectoModel model = new ProyectoModel();
            model.TransmisionProyectoDTO = proyecto;
            List<DataCatalogoDTO> ListCatalogo = proyecto.DataCatalogoDTOs;
            DataCatalogoDTO catalogo;
            // BIO-A
            BioHojaADTO bioHojaADTO = campaniasAppService.GetBioHojaAById(proyecto.Proycodi);
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catPropietario && c.Valor == bioHojaADTO.Propietario);
            if (catalogo != null)
            {
                bioHojaADTO.Propietario = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catConcesionTemporal && c.Valor == bioHojaADTO.ConTemporal);
            if (catalogo != null)
            {
                bioHojaADTO.ConTemporal = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catConcesionActual && c.Valor == bioHojaADTO.TipoConActual);
            if (catalogo != null)
            {
                bioHojaADTO.TipoConActual = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catPerfil && c.Valor == bioHojaADTO.Perfil);
            if (catalogo != null)
            {
                bioHojaADTO.Perfil = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catPreFactibilidad && c.Valor == bioHojaADTO.Prefactibilidad);
            if (catalogo != null)
            {
                bioHojaADTO.Prefactibilidad = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catFactibilidad && c.Valor == bioHojaADTO.Factibilidad);
            if (catalogo != null)
            {
                bioHojaADTO.Factibilidad = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catEstudDef && c.Valor == bioHojaADTO.EstDefinitivo);
            if (catalogo != null)
            {
                bioHojaADTO.EstDefinitivo  = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catEia && c.Valor == bioHojaADTO.Eia);
            if (catalogo != null)
            {
                bioHojaADTO.Eia = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catCombustible && c.Valor == bioHojaADTO.TipoNomComb);
            if (catalogo != null)
            {
                bioHojaADTO.TipoNomComb = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catPodCalInf && c.Valor == bioHojaADTO.CombPoderCalorInf);
            if (catalogo != null)
            {
                bioHojaADTO.CombPoderCalorInf = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catPodCalSup && c.Valor == bioHojaADTO.CombPoderCalorSup);
            if (catalogo != null)
            {
                bioHojaADTO.CombPoderCalorSup = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catCComb && c.Valor == bioHojaADTO.CombCostoCombustible);
            if (catalogo != null)
            {
                bioHojaADTO.CombCostoCombustible = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catCTratComb && c.Valor == bioHojaADTO.CombCostTratamiento);
            if (catalogo != null)
            {
                bioHojaADTO.CombCostTratamiento = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catCTranspComb && c.Valor == bioHojaADTO.CombCostTransporte);
            if (catalogo != null)
            {
                bioHojaADTO.CombCostTransporte = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catCVarNComb && c.Valor == bioHojaADTO.CombCostoVariableNoComb);
            if (catalogo != null)
            {
                bioHojaADTO.CombCostoVariableNoComb = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catCInvIni && c.Valor == bioHojaADTO.CombCostoInversion);
            if (catalogo != null)
            {
                bioHojaADTO.CombCostoInversion = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catRendPl && c.Valor == bioHojaADTO.CombRendPlanta);
            if (catalogo != null)
            {
                bioHojaADTO.CombRendPlanta = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catConsEspCon && c.Valor == bioHojaADTO.CombConsEspec);
            if (catalogo != null)
            {
                bioHojaADTO.CombConsEspec = catalogo.DescortaDatacat;
            }
            // Asignar el nombre de la subestación correspondiente
            if (!string.IsNullOrEmpty(bioHojaADTO.NomSubEstacion))
            {
                var subestacion = subestaciones.FirstOrDefault(s => s.Equicodi.ToString() == bioHojaADTO.NomSubEstacion);
                if (subestacion != null)
                {
                    bioHojaADTO.NomSubEstacion = subestacion.Equinomb;
                }

            }
            model.BioHojaADTO = bioHojaADTO;
            // BIO-B
            BioHojaBDTO bioHojaBDTO = campaniasAppService.GetBioHojaBById(proyecto.Proycodi);
            model.BioHojaBDTO = bioHojaBDTO;
            // BIO-C
            BioHojaCDTO bioHojaCDTO = campaniasAppService.GetBioHojaCById(proyecto.Proycodi);
            bioHojaCDTO.ListaDetBioHojaCDTO = campaniasAppService.GetDetBioHojaCCodi(bioHojaCDTO.Biohojaccodi);
            model.BioHojaCDTO = bioHojaCDTO;
            // CATALOGO
            List<DataCatalogoDTO> dataCatalogoDTOs =  campaniasAppService.ListParametria(CategoriaRequisito.CentralBioHojaC);
            model.DataCatalogoDTOs = dataCatalogoDTOs;
            //UBICACION
            if (bioHojaADTO.Distrito != null && bioHojaADTO.Distrito != "")
            {
                model.ubicacionDTO = campaniasAppService.GetUbicacionByDistrito(bioHojaADTO.Distrito);
            }
            
            ExcelDocument.GenerarExcelFichaProyectoTipoGeneracionCentralBiom(model, identificadorUnico);
        }

        public void descargarProyectoTipoGeneracionSubestacion(TransmisionProyectoDTO proyecto, string subtipo, string identificadorUnico)
        {
            ProyectoModel model = new ProyectoModel();
            model.TransmisionProyectoDTO = proyecto;
            // FICHA 1
            SubestFicha1DTO regHojaa = campaniasAppService.GetSubestFicha1ById(proyecto.Proycodi);
            List<SubestFicha1Det1DTO> Lista1DTO = campaniasAppService.GetSubestFicha1Det1ById(regHojaa.SubestFicha1Codi);
            List<SubestFicha1Det2DTO> Lista2DTO = campaniasAppService.GetSubestFicha1Det2ById(regHojaa.SubestFicha1Codi);
            List<SubestFicha1Det3DTO> Lista3DTO = campaniasAppService.GetSubestFicha1Det3ById(regHojaa.SubestFicha1Codi);
            regHojaa.Lista1DTOs = Lista1DTO;
            regHojaa.Lista2DTOs = Lista2DTO;
            regHojaa.Lista3DTOs = Lista3DTO;
            model.subestFicha1DTO = regHojaa;
            // CATALOGO
            List<DataCatalogoDTO> dataCatalogoDTOs = campaniasAppService.ListParametria(CategoriaRequisito.SubesTrasnformPot);
            model.DataCatalogoDTOs = dataCatalogoDTOs;
            List<DataCatalogoDTO> dataCatalogoDTOs2 = campaniasAppService.ListParametria(CategoriaRequisito.SubesTrasnformPotPrueba);
            model.DataCatalogoDTOs2 = dataCatalogoDTOs2;
            List<DataCatalogoDTO> dataCatalogoDTOs3 = campaniasAppService.ListParametria(CategoriaRequisito.SubesCompenReactPrueba);
            model.DataCatalogoDTOs3 = dataCatalogoDTOs3;

            ExcelDocument.GenerarExcelFichaProyectoTipoGeneracionSubestacion(model, subtipo, identificadorUnico);
        }

        public void descargarProyectoTipoGeneracionLinea(TransmisionProyectoDTO proyecto, string subtipo, string identificadorUnico)
        {
            ProyectoModel model = new ProyectoModel();
            model.TransmisionProyectoDTO = proyecto;
            // FICHA A
            LineasFichaADTO lineasFichaADTO = campaniasAppService.GetLineasFichaAById(proyecto.Proycodi);
            lineasFichaADTO.LineasFichaADet1DTO = campaniasAppService.GetLineasFichaADet1Codi(lineasFichaADTO.LinFichaACodi);
            lineasFichaADTO.LineasFichaADet2DTO = campaniasAppService.GetLineasFichaADet2Codi(lineasFichaADTO.LinFichaACodi);
            model.lineasFichaADTO = lineasFichaADTO;
            // FICHA B
            LineasFichaBDTO lineasFichaBDTO = campaniasAppService.GetLineasFichaBById(proyecto.Proycodi);
            lineasFichaBDTO.LineasFichaBDetDTO = campaniasAppService.GetLineasFichaBDetCodi(lineasFichaBDTO.FichaBCodi);
            model.LineasFichaBDTO = lineasFichaBDTO;

            ExcelDocument.GenerarExcelFichaProyectoTipoGeneracionLinea(model, subtipo, identificadorUnico);
        }


        public void descargarProyectoTipoITCSistemaElectrico(TransmisionProyectoDTO proyecto, string identificadorUnico)
        {
            ProyectoModel model = new ProyectoModel();
            model.TransmisionProyectoDTO = proyecto;
            // PRM-1
            List<ItcPrm1Dto> itcPrm1Dtos = campaniasAppService.GetItcprm1ById(proyecto.Proycodi);
            model.ListaItcPrm1DTO = itcPrm1Dtos;
            // PRM-2
            List<ItcPrm2Dto> itcPrm2Dtos = campaniasAppService.GetItcprm2ById(proyecto.Proycodi);
            model.ListaItcPrm2DTO = itcPrm2Dtos;
            // RED-1
            List<ItcRed1Dto> itcRed1Dto = campaniasAppService.GetItcred1ById(proyecto.Proycodi);
            model.ListaItcRed1DTO = itcRed1Dto;
            // RED-2
            List<ItcRed2Dto> itcRed2Dto = campaniasAppService.GetItcred2ById(proyecto.Proycodi);
            model.ListaItcRed2DTO = itcRed2Dto;
            // RED-3
            List<ItcRed3Dto> itcRed3Dto = campaniasAppService.GetItcred3ById(proyecto.Proycodi);
            model.ListaItcRed3DTO = itcRed3Dto;
            // RED-4
            List<ItcRed4Dto> itcRed4Dto = campaniasAppService.GetItcred4ById(proyecto.Proycodi);
            model.ListaItcRed4DTO = itcRed4Dto;
            // RED-5
            List<ItcRed5Dto> itcRed5Dto = campaniasAppService.GetItcred5ById(proyecto.Proycodi);
            model.ListaItcRed5DTO = itcRed5Dto;

            ExcelDocument.GenerarExcelFichaProyectoTipoITCSistemaElectrico(model, identificadorUnico);
        }
        public void descargarProyectoTipoITCDemanda(TransmisionProyectoDTO proyecto, string identificadorUnico)
        {
            ProyectoModel model = new ProyectoModel();
            model.TransmisionProyectoDTO = proyecto;
            // F-104
            List<Itcdf104DTO> itcdf104DTO = campaniasAppService.GetItcdf104ById(proyecto.Proycodi);
            model.Itcdf104DTOs = itcdf104DTO;
            // F-108
            List<Itcdf108DTO> Itcdf108DTOs = campaniasAppService.GetItcdf108ById(proyecto.Proycodi);
            model.Itcdf108DTOs = Itcdf108DTOs;
            // F-P01.1

            Itcdfp011DTO Itcdfp011DTOs = campaniasAppService.GetItcdfp011ById(proyecto.Proycodi);
            List<Itcdfp011DetDTO> detalles = new List<Itcdfp011DetDTO>();

            int page = 0;
            int pageSize = 5000;
            int totalRegistros = 0; // Variable para calcular el desplazamiento dinámico
            List<Itcdfp011DetDTO> tempResults;

            do
            {
                tempResults = campaniasAppService.GetItcdfp011DetByIdPag(Itcdfp011DTOs.Itcdfp011Codi, totalRegistros, pageSize);

                if (tempResults != null && tempResults.Count > 0)
                {
                    detalles.AddRange(tempResults);
                    totalRegistros += tempResults.Count; // Aumentar en función de la cantidad obtenida
                }

            } while (tempResults != null && tempResults.Count == pageSize); // Seguir hasta que no haya más registros

            Itcdfp011DTOs.ListItcdf011Det = detalles;
            model.Itcdfp011DTO = Itcdfp011DTOs;
            // F-P01.2
            List<Itcdfp012DTO> itcdfp012DTOs = campaniasAppService.GetItcdfp012ById(proyecto.Proycodi);
            model.Itcdfp012DTOs = itcdfp012DTOs;
            // F-P01.3
            List<Itcdfp013DTO> Itcdfp013DTOs = campaniasAppService.GetItcdfp013ById(proyecto.Proycodi);
            for (int i = 0; i < Itcdfp013DTOs.Count; i++)
            {
                Itcdfp013DTOs[i].ListItcdf013Det = campaniasAppService.GetItcdfp013DetCodi(Itcdfp013DTOs[i].Itcdfp013Codi);
            }
            model.Itcdfp013DTOs = Itcdfp013DTOs;
            // F-110
            List<Itcdf110DTO> itcdf110DTOs = campaniasAppService.GetItcdf110ById(proyecto.Proycodi);
            for (int i = 0; i < itcdf110DTOs.Count; i++)
            {
                itcdf110DTOs[i].ListItcdf110Det = campaniasAppService.GetItcdf110DetCodi(itcdf110DTOs[i].Itcdf110Codi);
            }
            model.Itcdf110DTOs = itcdf110DTOs;
            // F-116
            List<Itcdf116DTO> itcdf116DTOs = campaniasAppService.GetItcdf116ById(proyecto.Proycodi);
            for (int i = 0; i < itcdf116DTOs.Count; i++)
            {
                itcdf116DTOs[i].ListItcdf116Det = campaniasAppService.GetItcdf116DetCodi(itcdf116DTOs[i].Itcdf116Codi);
            }
             model.Itcdf116DTOs = itcdf116DTOs;
            // F-121
            List<Itcdf121DTO> itcdf121DTOs = campaniasAppService.GetItcdf121ById(proyecto.Proycodi);
            for (int i = 0; i < itcdf121DTOs.Count; i++)
            {
                itcdf121DTOs[i].ListItcdf121Det = campaniasAppService.GetItcdf121DetCodi(itcdf121DTOs[i].Itcdf121Codi);
            }
            model.Itcdf121DTOs = itcdf121DTOs;
            // F-123
            List<Itcdf123DTO> itcdf123DTOs = campaniasAppService.GetItcdf123ById(proyecto.Proycodi);
            model.Itcdf123DTOs = itcdf123DTOs;
            // F-E01
            
            ExcelDocument.GenerarExcelFichaProyectoTipoITCDemanda(model, identificadorUnico);
        }
        public void descargarProyectoTipoTransmisionLinea(TransmisionProyectoDTO proyecto, string identificadorUnico)
        {
            ProyectoModel model = new ProyectoModel();
            model.TransmisionProyectoDTO = proyecto;

            // FICHA 1
            T1LinFichaADTO lineasFichaADTO = campaniasAppService.GetLineasT1FichaAById(proyecto.Proycodi);
            lineasFichaADTO.LineasFichaADet1DTO = campaniasAppService.GetT1LineasFichaADet1Codi(lineasFichaADTO.LinFichaACodi);
            lineasFichaADTO.LineasFichaADet2DTO = campaniasAppService.GetT1LineasFichaADet2Codi(lineasFichaADTO.LinFichaACodi);
            model.T1lineasFichaADTO = lineasFichaADTO;

            ExcelDocument.GenerarExcelFichaProyectoTipoTransmisionLinea(model, identificadorUnico);
        }
        public void descargarProyectoTipoTransmisionSubestacion(TransmisionProyectoDTO proyecto, string identificadorUnico)
        {
            ProyectoModel model = new ProyectoModel();
            model.TransmisionProyectoDTO = proyecto;

            // FICHA 1
            T2SubestFicha1DTO regHojaa = campaniasAppService.GetT2SubestFicha1ById(proyecto.Proycodi);
            List<T2SubestFicha1Det1DTO> Lista1DTO = campaniasAppService.GetT2SubestFicha1Det1ById(regHojaa.SubestFicha1Codi);
            List<T2SubestFicha1Det2DTO> Lista2DTO = campaniasAppService.GetT2SubestFicha1Det2ById(regHojaa.SubestFicha1Codi);
            List<T2SubestFicha1Det3DTO> Lista3DTO = campaniasAppService.GetT2SubestFicha1Det3ById(regHojaa.SubestFicha1Codi);
            regHojaa.Lista1DTOs = Lista1DTO;
            regHojaa.Lista2DTOs = Lista2DTO;
            regHojaa.Lista3DTOs = Lista3DTO;
            model.t2SubestFicha1DTO = regHojaa;
            // CATALOGO
            List<DataCatalogoDTO> dataCatalogoDTOs = campaniasAppService.ListParametria(CategoriaRequisito.SubesTrasnformPot);
            model.DataCatalogoDTOs = dataCatalogoDTOs;
            List<DataCatalogoDTO> dataCatalogoDTOs2 = campaniasAppService.ListParametria(CategoriaRequisito.SubesTrasnformPotPrueba);
            model.DataCatalogoDTOs2 = dataCatalogoDTOs2;
            List<DataCatalogoDTO> dataCatalogoDTOs3 = campaniasAppService.ListParametria(CategoriaRequisito.SubesCompenReactPrueba);
            model.DataCatalogoDTOs3 = dataCatalogoDTOs3;

            ExcelDocument.GenerarExcelFichaProyectoTipoTransmisionSubestacion(model, identificadorUnico);
        }
        public void descargarProyectoTipoTransmisionCronograma(TransmisionProyectoDTO proyecto, string identificadorUnico)
        {
            ProyectoModel model = new ProyectoModel();
            model.TransmisionProyectoDTO = proyecto;

            // FICHA 1
            CroFicha1DTO croFicha1DTO = campaniasAppService.GetCroFicha1ById(proyecto.Proycodi);
            croFicha1DTO.ListaCroFicha1DetDTO = campaniasAppService.GetCroFicha1DetCodi(croFicha1DTO.CroFicha1codi);
            model.CroFicha1DTO = croFicha1DTO;
            // CATALOGO
            List<DataCatalogoDTO> dataCatalogoDTOs = campaniasAppService.ListParametria(CategoriaRequisito.TransmisionCrono);
            model.DataCatalogoDTOs = dataCatalogoDTOs;
            ExcelDocument.GenerarExcelFichaProyectoTipoTransmisionCronograma(model, identificadorUnico);
        }
        public void descargarProyectoTipoDemanda(TransmisionProyectoDTO proyecto, List<DataSubestacionDTO> subestaciones, string identificadorUnico)
        {
            ProyectoModel model = new ProyectoModel();
            model.TransmisionProyectoDTO = proyecto;
            List<DataCatalogoDTO> ListCatalogo = proyecto.DataCatalogoDTOs;
            DataCatalogoDTO catalogo;
            // FormatoD1-A
            FormatoD1ADTO formatoD1ADTO = campaniasAppService.GetFormatoD1AById(proyecto.Proycodi);
            formatoD1ADTO.ListaFormatoDet1A = campaniasAppService.GetFormatoD1ADET1ByProyCodi(formatoD1ADTO.FormatoD1ACodi);
            formatoD1ADTO.ListaFormatoDet2A = campaniasAppService.GetFormatoD1ADET2ByProyCodi(formatoD1ADTO.FormatoD1ACodi);
            formatoD1ADTO.ListaFormatoDet3A = campaniasAppService.GetFormatoD1ADET3ByProyCodi(formatoD1ADTO.FormatoD1ACodi);
            formatoD1ADTO.ListaFormatoDet4A = campaniasAppService.GetFormatoD1ADET4ByProyCodi(formatoD1ADTO.FormatoD1ACodi);
            formatoD1ADTO.ListaFormatoDet5A = campaniasAppService.GetFormatoD1ADET5ByProyCodi(formatoD1ADTO.FormatoD1ACodi);
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catTipoCarga && c.Valor == formatoD1ADTO.TipoCarga);
            if (catalogo != null)
            {
                formatoD1ADTO.TipoCarga = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catSubEmpElectro && c.Valor == formatoD1ADTO.EmpresaSuminicodi);
            if (catalogo != null)
            {
                formatoD1ADTO.EmpresaSuminicodi = catalogo.DescortaDatacat;
            }
            model.FormatoD1ADTO = formatoD1ADTO;
            // FormatoD1-B
            FormatoD1BDTO formatoD1BDTO = campaniasAppService.GetFormatoD1BById(proyecto.Proycodi);
            formatoD1BDTO.ListaFormatoDet1B = campaniasAppService.GetFormatoD1BDetByCodi(formatoD1BDTO.FormatoD1BCodi);
            model.FormatoD1BDTO = formatoD1BDTO;
            // FormatoD1-C
            FormatoD1CDTO formatoD1CDTO = campaniasAppService.GetFormatoD1CById(proyecto.Proycodi);
            formatoD1CDTO.ListaFormatoDe1CDet = campaniasAppService.GetFormatoD1CDetCCodi(formatoD1CDTO.FormatoD1CCodi);
            model.FormatoD1CDTO = formatoD1CDTO;
            // FormatoD1-D
            List<FormatoD1DDTO> regDFormatoD = campaniasAppService.GetFormatoD1DById(proyecto.Proycodi);
            model.listaFormatoDs = regDFormatoD;
            // CATALOGO
            List<DataCatalogoDTO> dataCatalogoDTOs = campaniasAppService.ListParametria(CategoriaRequisito.DemandaHojaD);
            model.DataCatalogoDTOs = dataCatalogoDTOs;
            List<DataCatalogoDTO> dataCatalogoDTOs2 = campaniasAppService.ListParametria(CategoriaRequisito.DemandaHojaA);
            model.DataCatalogoDTOs2 = dataCatalogoDTOs2;
            //UBICACION
            if (formatoD1ADTO.Distrito != null && formatoD1ADTO.Distrito != "")
            {
                model.ubicacionDTO = campaniasAppService.GetUbicacionByDistrito(formatoD1ADTO.Distrito);
            }
            // Asignar el nombre de la subestación correspondiente
            if (!string.IsNullOrEmpty(formatoD1ADTO.SubestacionCodi))
            {
                var subestacion = subestaciones.FirstOrDefault(s => s.Equicodi.ToString() == formatoD1ADTO.SubestacionCodi);
                if (subestacion != null)
                {
                    formatoD1ADTO.SubestacionCodi = subestacion.Equinomb;
                }
                
            }

            ExcelDocument.GenerarExcelFichaProyectoTipoDemanda(model, identificadorUnico);
        }
        public void descargarProyectoTipoGeneracionDistribuida(TransmisionProyectoDTO proyecto, string identificadorUnico)
        {
            ProyectoModel model = new ProyectoModel();
            model.TransmisionProyectoDTO = proyecto;
            List<DataCatalogoDTO> ListCatalogo = proyecto.DataCatalogoDTOs;
            DataCatalogoDTO catalogo;
            // CC.GD-A
            CCGDADTO ccgdaDTO = campaniasAppService.GetCcgdaById(proyecto.Proycodi);
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catObjProyecto && c.Valor == ccgdaDTO.ObjetivoProyecto);
            if (catalogo != null)
            {
                ccgdaDTO.ObjetivoProyecto = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catTituloHab && c.Valor == ccgdaDTO.TipoTecnologia);
            if (catalogo != null)
            {
                ccgdaDTO.TipoTecnologia = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catEstadoOperacion && c.Valor == ccgdaDTO.EstadoOperacion);
            if (catalogo != null)
            {
                ccgdaDTO.EstadoOperacion = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catPerfil && c.Valor == ccgdaDTO.Perfil);
            if (catalogo != null)
            {
                ccgdaDTO.Perfil = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catPreFactibilidad && c.Valor == ccgdaDTO.Prefactibilidad);
            if (catalogo != null)
            {
                ccgdaDTO.Prefactibilidad = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catFactibilidad && c.Valor == ccgdaDTO.Factibilidad);
            if (catalogo != null)
            {
                ccgdaDTO.Factibilidad = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catEstudDef && c.Valor == ccgdaDTO.EstDefinitivo);
            if (catalogo != null)
            {
                ccgdaDTO.EstDefinitivo = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catEia && c.Valor == ccgdaDTO.Eia);
            if (catalogo != null)
            {
                ccgdaDTO.Eia = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catRecursoUsado && c.Valor == ccgdaDTO.RecursoUsada);
            if (catalogo != null)
            {
                ccgdaDTO.RecursoUsada = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catTecnologia && c.Valor == ccgdaDTO.Tecnologia);
            if (catalogo != null)
            {
                ccgdaDTO.Tecnologia = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catEstadoOperacion && c.Valor == ccgdaDTO.EstadoOperacionGD);
            if (catalogo != null)
            {
                ccgdaDTO.EstadoOperacionGD = catalogo.DescortaDatacat;
            }
            model.CcgdaDTO = ccgdaDTO;
            // CC.GD-B
            List<CCGDBDTO> ccgdbDTO = campaniasAppService.GetCcgdbById(proyecto.Proycodi);
            model.Ccgdbdtos = ccgdbDTO;
            // CC.GD-C
            List<CCGDCOptDTO> cCGDCOptDTOs = campaniasAppService.GetCcgdcOptById(proyecto.Proycodi);
            List<CCGDCPesDTO> cCGDCPesDTOs = campaniasAppService.GetCcgdcPesById(proyecto.Proycodi);
            model.CCGDCOptDTOs = cCGDCOptDTOs;
            model.CCGDCPesDTOs = cCGDCPesDTOs;
            // CC.GD-D
            List<CCGDDDTO> regHojaE = campaniasAppService.GetCcgddById(proyecto.Proycodi);
            model.Ccgdddtos = regHojaE;
            // CC.GD-E
            CCGDEDTO ccgdeDTO = campaniasAppService.GetCcgdeById(proyecto.Proycodi);
            model.CcgdeDTO = ccgdeDTO;
            // CC.GD-F
            List<CCGDFDTO> ccgdfDTO = campaniasAppService.GetCcgdfById(proyecto.Proycodi);
            model.CcgdfDTOs = ccgdfDTO;
            // CATALOGO
            List<DataCatalogoDTO> dataCatalogoDTOs = campaniasAppService.ListParametria(CategoriaRequisito.CcGdHojaF);
            model.DataCatalogoDTOs = dataCatalogoDTOs;
            //UBICACION
            if (ccgdaDTO.DistritoCodi != null && ccgdaDTO.DistritoCodi != "")
            {
                model.ubicacionDTO = campaniasAppService.GetUbicacionByDistrito(ccgdaDTO.DistritoCodi);
            }
            if (ccgdaDTO.DistritoGDCodi != null && ccgdaDTO.DistritoGDCodi != "")
            {
                model.ubicacionDTO2 = campaniasAppService.GetUbicacionByDistrito(ccgdaDTO.DistritoGDCodi);
            }
            ExcelDocument.GenerarExcelFichaProyectoTipoGeneracionDistribuida(model, identificadorUnico);
        }

        public void descargarProyectoTipoHidrogenoVerde(TransmisionProyectoDTO proyecto, string identificadorUnico)
        {
            ProyectoModel model = new ProyectoModel();
            model.TransmisionProyectoDTO = proyecto;
            List<DataCatalogoDTO> ListCatalogo = proyecto.DataCatalogoDTOs;
            DataCatalogoDTO catalogo;
            // H2V-A
            CuestionarioH2VADTO regHojaa = campaniasAppService.GetCuestionarioH2VAById(proyecto.Proycodi);
            List<CuestionarioH2VADet1DTO> Lista1DTO = campaniasAppService.GetCuestionarioH2VADet1ById(regHojaa.H2vaCodi);
            List<CuestionarioH2VADet2DTO> Lista2DTO = campaniasAppService.GetCuestionarioH2VADet2ById(regHojaa.H2vaCodi);
            regHojaa.ListCH2VADet1DTOs = Lista1DTO;
            regHojaa.ListCH2VADet2DTOs = Lista2DTO;
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catElectrolizador && c.Valor == regHojaa.TipoElectrolizador);
            if (catalogo != null)
            {
                regHojaa.TipoElectrolizador = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catObjProyec && c.Valor == regHojaa.ObjetivoProyecto);
            if (catalogo != null)
            {
                regHojaa.ObjetivoProyecto = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catHidrogeno && c.Valor == regHojaa.UsoEsperadoHidro);
            if (catalogo != null)
            {
                regHojaa.UsoEsperadoHidro = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catTransporteH2 && c.Valor == regHojaa.MetodoTransH2);
            if (catalogo != null)
            {
                regHojaa.MetodoTransH2 = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catSuministro && c.Valor == regHojaa.TipoSuministro);
            if (catalogo != null)
            {
                regHojaa.TipoSuministro = catalogo.DescortaDatacat;
            }
            catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catEstadoOperacion && c.Valor == regHojaa.SituacionAct);
            if (catalogo != null)
            {
                regHojaa.SituacionAct = catalogo.DescortaDatacat;
            }
            model.CuestionarioH2VADTO = regHojaa;
            // H2V-B
            CuestionarioH2VBDTO regHojab = campaniasAppService.GetCuestionarioH2VBById(proyecto.Proycodi);
            model.CuestionarioH2VBDTO = regHojab;
            // H2V-C
            List<CuestionarioH2VCDTO> regHojaC = campaniasAppService.GetCuestionarioH2VCById(proyecto.Proycodi);
            model.Ch2vcDTOs = regHojaC;
            // H2V-E
            List<CuestionarioH2VEDTO> regHojaE = campaniasAppService.GetCuestionarioH2VEById(proyecto.Proycodi);
            model.Ch2veDTOs = regHojaE;
            // H2V-F
            CuestionarioH2VFDTO CH2VFDTO = campaniasAppService.GetCuestionarioH2VFById(proyecto.Proycodi);
            model.Ch2vfDTO = CH2VFDTO;
            // H2V-G
            List<CuestionarioH2VGDTO> CH2VGDTO = campaniasAppService.GetCuestionarioH2VGById(proyecto.Proycodi);
            model.Ch2vgDTOs = CH2VGDTO;
            // CATALOGO
            List<DataCatalogoDTO> dataCatalogoDTOs = campaniasAppService.ListParametria(CategoriaRequisito.H2VHojaG);
            model.DataCatalogoDTOs = dataCatalogoDTOs;
            List<DataCatalogoDTO> dataCatalogoDTOs2 = campaniasAppService.ListParametria(CategoriaRequisito.H2VHojaA);
            model.DataCatalogoDTOs2 = dataCatalogoDTOs2;
            //UBICACION
            if (regHojaa.Distrito != null && regHojaa.Distrito != "")
            {
                model.ubicacionDTO = campaniasAppService.GetUbicacionByDistrito(regHojaa.Distrito);
            }
            if (regHojab.Distrito != null && regHojab.Distrito != "")
            {
                model.ubicacionDTO2 = campaniasAppService.GetUbicacionByDistrito(regHojab.Distrito);
            }
            ExcelDocument.GenerarExcelFichaProyectoTipoHidrogenoVerde(model,proyecto.EmpresaNom,proyecto.Proynombre, identificadorUnico);
        }

        public void descargarArchivosFichas(int proycodi, string pathFichaTipo, string pathFichaOrigen, string identificadorUnico)
        {
            // Asegurarse de quitar la barra final
            pathFichaTipo = pathFichaTipo.TrimEnd('\\');

            // Obtener el nombre del último segmento del path
            string ultimoSegmento = Path.GetFileName(pathFichaTipo);
            string pathSinUltimoSegmento = Path.GetDirectoryName(pathFichaTipo);

            // Crear el nuevo path con el proycodi
            string nuevoUltimoSegmento = $"{ultimoSegmento}-{proycodi}";
            pathFichaTipo = Path.Combine(pathSinUltimoSegmento, nuevoUltimoSegmento) + "\\";

            List<ArchivoInfoDTO> listaArchivoDTO = campaniasAppService.GetArchivoInfoByProyCodi(proycodi);
            if (listaArchivoDTO.Count > 0) {
                if (!FileServer.VerificarExistenciaDirectorio(ConstantesCampanias.FolderArchivos, pathFichaTipo)) FileServer.CreateFolder("", ConstantesCampanias.FolderArchivos, pathFichaTipo);
                string pathArchivos = pathFichaTipo + ConstantesCampanias.FolderArchivos;
                foreach (var archivoDTO in listaArchivoDTO)
                {
                    string pathOrigen = Path.Combine(pathFichaOrigen + archivoDTO.ArchUbicacion + "\\", archivoDTO.ArchNombreGenerado);

                    // Obtener la extensión del archivo
                    string extension = Path.GetExtension(archivoDTO.ArchNombre);
                    string nombreSinExtension = Path.GetFileNameWithoutExtension(archivoDTO.ArchNombre);

                    // Construir el nombre con el SeccCodi al final
                    string nombreFinal = $"{nombreSinExtension}({archivoDTO.SeccCodi}){extension}";
                    string pathDestino = Path.Combine(pathArchivos, nombreFinal);

                    try
                    {
                        if (System.IO.File.Exists(pathOrigen))
                        {
                            if (!System.IO.File.Exists(pathDestino))
                            {
                                System.IO.File.Copy(pathOrigen, pathDestino);
                                Console.WriteLine(string.Format("Archivo copiado: {0} a {1}", archivoDTO.ArchNombreGenerado, pathDestino));
                            }
                            else
                            {
                                Console.WriteLine(string.Format("Ya existe: {0}", pathDestino));
                            }
                        }
                        else
                        {
                            Console.WriteLine(string.Format("No existe archivo en origen: {0}", pathOrigen));
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(string.Format("Error copiando {0}: {1}", archivoDTO.ArchNombreGenerado, ex.Message));
                    }
                }
            }   
        }

        /// <summary>
        /// Descarga el reporte excel del servidor
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult ExportarFichaExcel(int tipo)
        {
            try
            {
                string nombreArchivo = string.Empty;
                if (tipo == TipoProyecto.Generacion)
                {
                    nombreArchivo = FormatoArchivosExcelCampanias.NombreFichaExcelGeneracionSubestaciones;
                }
                else if (tipo == TipoProyecto.Transmision)
                {
                    //nombreArchivo = FormatoArchivosExcelCampanias.NombreFichaExcelITCDemanda;
                }
                else if (tipo == TipoProyecto.ITC)
                {
                    nombreArchivo = FormatoArchivosExcelCampanias.NombreFichaExcelITCDemanda;
                }
                else if (tipo == TipoProyecto.Demanda)
                {
                    //nombreArchivo = FormatoArchivosExcelCampanias.NombreFichaExcelITCDemanda;
                }
                else if (tipo == TipoProyecto.GeneracionDistribuida)
                {
                    nombreArchivo = FormatoArchivosExcelCampanias.NombreFichaExcelGeneracionDistribuida;
                }
                else if (tipo == TipoProyecto.HidrogenoVerde)
                {
                    nombreArchivo = FormatoArchivosExcelCampanias.NombreFichaExcelITCDemanda;
                }
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesCampanias.FolderFichas + ConstantesCampanias.FolderTemp;
                string fullPath = ruta + nombreArchivo;
                return File(fullPath, Constantes.AppExcel, nombreArchivo);
            }
            catch (Exception ex)
            {
                // Log the exception
                return new HttpStatusCodeResult(500, ex.Message);
            }
        }

        public virtual ActionResult GenerarArchivosZipReporte(string nameZip, string uid)
        {
            try
            {
                string ruta = AppDomain.CurrentDomain.BaseDirectory;
                string pathBase = Path.Combine(ruta, ConstantesCampanias.FolderFichas);
                string carpetaUID = Path.Combine(pathBase, ConstantesCampanias.FolderTemp, uid);
                string zipPath = Path.Combine(carpetaUID, nameZip);

                if (System.IO.File.Exists(zipPath)) System.IO.File.Delete(zipPath);

                using (var zipToOpen = new FileStream(zipPath, FileMode.Create))
                using (var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Create))
                {
                    var archivos = Directory.GetFiles(carpetaUID, "*", SearchOption.AllDirectories)
                        .Where(f => !f.EndsWith(".zip"))
                        .ToArray();

                    int baseLength = carpetaUID.EndsWith("\\") ? carpetaUID.Length : carpetaUID.Length + 1;

                    foreach (var archivo in archivos)
                    {
                        string relativePath = archivo.Substring(baseLength);
                        archive.CreateEntryFromFile(archivo, relativePath);
                    }
                }

                // Retornar el ZIP como stream con eliminación después de la descarga
                var stream = new FileStream(zipPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                return new FileStreamResult(new AutoDeleteZipAndFolderStream(stream, zipPath, carpetaUID), "application/octet-stream")
                {
                    FileDownloadName = nameZip
                };
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"ERROR: {ex.Message}", ex);
            }
        }
        public class AutoDeleteZipAndFolderStream : Stream
        {
            private readonly Stream _innerStream;
            private readonly string _zipPath;
            private readonly string _folderPath;

            public AutoDeleteZipAndFolderStream(Stream innerStream, string zipPath, string folderPath)
            {
                _innerStream = innerStream;
                _zipPath = zipPath;
                _folderPath = folderPath;
            }

            public override void Close()
            {
                base.Close();
                _innerStream.Close();

                try
                {
                    if (System.IO.File.Exists(_zipPath))
                        System.IO.File.Delete(_zipPath);

                    if (Directory.Exists(_folderPath))
                        Directory.Delete(_folderPath, true);
                }
                catch (Exception ex)
                {
                    // Log opcional
                }
            }

            #region Métodos delegados
            public override bool CanRead => _innerStream.CanRead;
            public override bool CanSeek => _innerStream.CanSeek;
            public override bool CanWrite => _innerStream.CanWrite;
            public override long Length => _innerStream.Length;
            public override long Position { get => _innerStream.Position; set => _innerStream.Position = value; }
            public override void Flush() => _innerStream.Flush();
            public override int Read(byte[] buffer, int offset, int count) => _innerStream.Read(buffer, offset, count);
            public override long Seek(long offset, SeekOrigin origin) => _innerStream.Seek(offset, origin);
            public override void SetLength(long value) => _innerStream.SetLength(value);
            public override void Write(byte[] buffer, int offset, int count) => _innerStream.Write(buffer, offset, count);
            #endregion
        }


        [System.Web.Mvc.HttpPost]
        public ActionResult ListadoProyectoFicha(int id)
        {
            List<TransmisionProyectoDTO> listaProyecto = campaniasAppService.GetTransmisionProyecto(id);
            ProyectoModel model = new ProyectoModel();
            model.listaProyecto = listaProyecto;
            return View(model);
        }

        public ActionResult ProyectoFicha()
        {
            CampaniasModel model = new CampaniasModel();
            return View(model);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Proyecto(int id, string modo)
        {
            CampaniasModel model = new CampaniasModel();
                TransmisionProyectoDTO proyecto = campaniasAppService.GetTransmisionProyectoById(id);
                List<int> hojasPeriodo = campaniasAppService.GetDetalleHojaByPericodi(proyecto.Pericodi, Constantes.IndDel);
                model.TransmisionProyecto = proyecto;
                model.ListaHojas = hojasPeriodo;
                model.Modo = modo;

            return View(model);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult ListarEmpresas()
        {
            List<SiEmpresaDTO> ListaEmpresaUsuario = this.seguridad.ObtenerEmpresasPorUsuario(User.Identity.Name).
             Select(x => new SiEmpresaDTO
             {
                 Emprcodi = x.EMPRCODI,
                 Emprnomb = x.EMPRNOMB
             }).ToList();
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
        public JsonResult BuscarPlanTransmision(int id)
        {
            List<PlanTransmisionDTO> dataPlant = campaniasAppService.GetPlanTransmisionByFilters(id);
            return Json(dataPlant);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult ListarProyectos()
        {
            List<TipoProyectoDTO> ListaProyectos = campaniasAppService.ListTipoProyecto();
            return Json(ListaProyectos);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult ActivarEnvio(int id)
        {
            int result = 0;
            
            if (campaniasAppService.DesactivatePlanById(id))
             {
                 if (campaniasAppService.ActivatePlanById(id))
                 {
                     result = 1;
                 }                
             }
             else
             {
                 result=0;
             } 
           
            return Json(result);
        }

         [System.Web.Mvc.HttpPost]
        public JsonResult AprobarEnvio(int id)
        {
            int result = 0;
            try
            {
                string planestado = "Aprobado";
                if (campaniasAppService.UpdatePlanEstadoById(id, planestado))
                {
                    if (campaniasAppService.DesactivatePlanById(id))
                    {
                        if (campaniasAppService.ActivatePlanById(id))
                        {
                            EnvioDto envioDto = new EnvioDto();
                            envioDto.PlanTransmision = campaniasAppService.GetPlanTransmisionById(id);
                            envioDto.Correos = envioDto.PlanTransmision.CorreoUsu;
                            int plantillaCorreo = 322;
                            campaniasAppService.EnviarCorreoNotificacionRevision(envioDto, plantillaCorreo);
                            result = 1;
                        }                
                    }             
                }
            }
            catch (Exception ex)
            {
                result = -1;
            }
            return Json(result);
        }

         [System.Web.Mvc.HttpPost]
        public JsonResult RechazarEnvio(ProyectoModel periodoModel)
        {
             int result = 0;
            try
            {
                string planestado = "Rechazado";
                if (campaniasAppService.UpdatePlanEstadoById(periodoModel.CodPlanTransmision, planestado))
                {
                    EnvioDto envioDto = new EnvioDto();
                    envioDto.PlanTransmision = campaniasAppService.GetPlanTransmisionById(periodoModel.CodPlanTransmision);
                    envioDto.Correos = envioDto.PlanTransmision.CorreoUsu;
                    envioDto.Comentarios = periodoModel.Comentarios;
                    int plantillaCorreo = 323;
                    campaniasAppService.EnviarCorreoNotificacionRevision(envioDto, plantillaCorreo);
                    result = 1;              
                }
            }
            catch (Exception ex)
            {
                result = -1;
            }
            return Json(result);
        }

        [System.Web.Mvc.HttpGet]
        public virtual ActionResult DescargarArchivoCampania(string nombre)
        {
            string path = campaniasAppService.ObtenerPathArchivosCampianas();
            ArchivoInfoDTO archivoInfoDTO = campaniasAppService.GetArchivoInfoNombreGenerado(nombre);
            string fileNameInicial = archivoInfoDTO.ArchNombreGenerado;
            string fileName = archivoInfoDTO.ArchNombre;
            string ubi_detalle = archivoInfoDTO.ArchUbicacion + "\\";
            Stream stream = campaniasAppService.DownloadToStream(path+ ubi_detalle + fileNameInicial);
            return File(stream, archivoInfoDTO.ArchTipo, fileName);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult ListarSubestacion()
        {
            List<DataSubestacionDTO> subestacionDTOs = new List<DataSubestacionDTO>();
            subestacionDTOs = campaniasAppService.ListParamSubestacion();
            return Json(subestacionDTOs);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult ListarCatalogoXDesc(string descortaCat)
        {
            List<CatalogoDTO> dataCatalogoDTOs = new List<CatalogoDTO>();
            dataCatalogoDTOs = campaniasAppService.GetCatalogoXdesc(descortaCat);
            return Json(dataCatalogoDTOs);
        }

        [System.Web.Mvc.HttpGet]
        public virtual ActionResult DescargarArchivoCampaniaObservacion(string nombre)
        {
            string path = campaniasAppService.ObtenerPathArchivosCampianas();
            ArchivoObsDTO archivoObsDTO = campaniasAppService.GetArchivoObsNombreArchivo(nombre);
            string fileNameInicial = archivoObsDTO.NombreArchGen;
            string fileName = archivoObsDTO.NombreArch;
            string ubi_detalle = string.IsNullOrEmpty(archivoObsDTO.RutaArch) ? "" : archivoObsDTO.RutaArch + "\\";
            Stream stream = campaniasAppService.DownloadToStream(path + ubi_detalle + fileNameInicial);
            return File(stream, archivoObsDTO.Tipo, fileName);
        }

    }
}
