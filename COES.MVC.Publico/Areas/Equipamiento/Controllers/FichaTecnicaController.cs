using COES.Dominio.DTO.Sic;
using COES.MVC.Publico.Areas.Equipamiento.Models;
using COES.MVC.Publico.Helper;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Equipamiento.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Publico.Areas.Equipamiento.Controllers
{
    public class FichaTecnicaController : Controller
    {
        FichaTecnicaAppServicio servFictec = new FichaTecnicaAppServicio();

        #region Declaracion de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(FichaTecnicaController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        /// <summary>
        /// Excepciones ocurridas en el controlador
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error(NameController, objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal(NameController, ex);
                throw;
            }
        }

        #endregion

        #region Index

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            FichaTecnicaModel model = new FichaTecnicaModel();
            model.ListaFichaTecnicaSelec = new List<FtFictecXTipoEquipoDTO>();

            try
            {
                model.FichaMaestra = this.servFictec.GetFichaMaestraPrincipal(ConstantesFichaTecnica.FichaMaestraPortal);
                model.ListaFichaTecnicaSelec = this.servFictec.ListarFichaTecnicaByMaestra(model.FichaMaestra.Fteccodi);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
            }

            return View(model);
        }

        /// <summary>
        /// Listar Empresas por Ficha Tecnica
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListaEmpresaXFichaTecnica(int idFT)
        {
            FichaTecnicaModel model = new FichaTecnicaModel();
            try
            {
                var fichaTecnica = servFictec.GetFichaTecnica(idFT);
                model.Origen = fichaTecnica.Origen;
                model.TipoElementoId = model.Origen == ConstantesFichaTecnica.OrigenTipoEquipo ? fichaTecnica.Famcodi.Value : -2;
                model.ListaEmpresa = new List<SiEmpresaDTO>();
                List<SiEmpresaDTO> Lista = this.servFictec.ListarEmpresaByFichaTecnica(idFT);
                foreach (var item in Lista)
                {
                    SiEmpresaDTO Empresa = this.servFictec.GetByIdSiEmpresa(item.Emprcodi);
                    if (Empresa.Emprestado == "A")
                    {
                        model.ListaEmpresa.Add(Empresa);
                    }
                }
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
            }

            return Json(model);
        }

        /// <summary>
        /// Lista de Elementos
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ElementosListado(int idFicha, int iEmpresa)
        {
            FichaTecnicaModel model = new FichaTecnicaModel();
            try
            {
                var fichaMaestra = this.servFictec.GetFichaMaestraPrincipal(ConstantesFichaTecnica.FichaMaestraPortal);
                var fichaTecnica = this.servFictec.ListarAllFichaTecnicaByMaestra(fichaMaestra.Fteccodi).Find(x => x.Fteqcodi == idFicha);

                //ficha seleccionada es de la maestra
                if (fichaTecnica != null)
                {
                    model.ListaElemento = this.servFictec.ListarElementoFichaTecnica(idFicha, iEmpresa, "A");
                    model.IdFicha = idFicha;

                    //quitar los equipos o grupos ocultos
                    model.ListaElemento = model.ListaElemento.Where(x => x.Ftveroculto != "S").ToList();

                    model.Resultado = "1";
                }
                else
                {
                    throw new ArgumentException("Debe seleccionar una ficha técnica.");
                }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                //model.Detalle = ex.StackTrace;
            }

            var JsonResult = Json(model);
            JsonResult.MaxJsonLength = int.MaxValue;

            return JsonResult;
        }

        #endregion

        #region Detalle

        /// <summary>
        /// Detalle de una Ficha Tecnica
        /// </summary>
        /// <param name="tipo">equipo o grupo</param>
        /// <param name="idTipo">codigo famcodi o catecodi</param>
        /// <param name="idElemento">codigo de equipo o grupo</param>
        /// <returns></returns>
        public ActionResult IndexDetalle(int idFicha, int idElemento)
        {
            FichaTecnicaModel model = new FichaTecnicaModel();

            model.Elemento = new ElementoFichaTecnica();
            model.Elemento.Existe = ConstantesFichaTecnica.FichaMaestraNoPrincipal;

            var listaEmpresa = new List<SiEmpresaDTO>();

            try
            {
                var fichaMaestra = this.servFictec.GetFichaMaestraPrincipal(ConstantesFichaTecnica.FichaMaestraPortal);
                var fichaTecnica = this.servFictec.ListarAllFichaTecnicaByMaestra(fichaMaestra.Fteccodi).Find(x => x.Fteqcodi == idFicha);

                //ficha seleccionada es de la maestra
                if (fichaTecnica != null)
                {
                    model.Elemento = this.servFictec.GetElementoDetalleExistente(idFicha, idElemento);
                    model.TipoElemento = model.Elemento.Tipo;
                    model.TipoElementoId = model.Elemento.TipoId;
                    model.Codigo = idElemento;

                    if (model.TipoElementoId == 1)// tipo subestación
                    {
                        //obtener empresas
                        listaEmpresa = servFictec.ListaEmpresasXFichaSubestacion(model.Elemento.Fteqcodi, model.Elemento.Areacodi.Value, true);
                    }

                    model.IdFicha = idFicha;
                }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Mensaje = ex.ToString();
            }

            model.ListaEmpresa = new List<SiEmpresaDTO>();
            if (listaEmpresa.Count != 1)
                model.ListaEmpresa.Add(new SiEmpresaDTO() { Emprcodi = -2, Emprnomb = "--TODOS--" });
            model.ListaEmpresa.AddRange(listaEmpresa);

            return View(model);
        }

        /// <summary>
        /// GenerarDetalleFichaTecnica
        /// </summary>
        /// <param name="idFM"></param>
        /// <param name="tipo"></param>
        /// <param name="idTipo"></param>
        /// <param name="idElemento"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarDetalleFichaTecnica(int idFicha, int idElemento)
        {
            FichaTecnicaModel model = new FichaTecnicaModel();
            //idElemento = 1;
            try
            {
                List<FtFictecItemDTO> listaTreeItems, listaAllItems;
                List<TreeItemFichaTecnica> listaItemsJson;
                List<FtFictecNotaDTO> listaNota;
                List<FtFictecXTipoEquipoDTO> listaHijo;
                FtFictecXTipoEquipoDTO fichaTecnica;
                List<EqEquipoDTO> listaEquipo;
                List<PrGrupoDTO> listaGrupo;

                bool flagSoloEqActivo = true;
                this.servFictec.ReporteDetalleFichaTecnica(idFicha, idElemento, ConstantesFichaTecnica.PORTAL, flagSoloEqActivo, DateTime.Today,
                    out listaAllItems, out listaTreeItems, out listaItemsJson, out listaNota, out listaHijo, out fichaTecnica,
                    out listaEquipo, out listaGrupo, out bool flagExisteComentario);

                model.ListaAllItems = listaAllItems;
                model.ListaTreeItems = listaTreeItems;
                model.ListaItemsJson = listaItemsJson;

                model.FlagExisteComentario = flagExisteComentario ? 1 : 0;
                model.ListaNota = listaNota;

                model.ListaHijo = listaHijo;
                model.FichaTecnica = fichaTecnica;
                model.ListaEquipo = listaEquipo;
                model.ListaGrupo = listaGrupo;

                model.IdFicha = idFicha;
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
            }

            return Json(model);
        }

        /// <summary>
        /// Exportación de Excel del Detalle
        /// </summary>
        /// <param name="sFechaIni"></param>
        /// <param name="sFechaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarFichaTecnica(int idFicha, int idElemento, int opcionComentario)
        {
            string[] datos = new string[2];
            try
            {
                var fichaMaestra = this.servFictec.GetFichaMaestraPrincipal(ConstantesFichaTecnica.FichaMaestraPortal);
                var fichaTecnica = this.servFictec.ListarAllFichaTecnicaByMaestra(fichaMaestra.Fteccodi).Find(x => x.Fteqcodi == idFicha);
                //ficha seleccionada es de la maestra
                if (fichaTecnica != null)
                {

                    bool incluirColumnaComentario = opcionComentario == 1;
                    string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
                    bool flagSoloEqActivo = true;
                    this.servFictec.ReporteDetalleFichaTecnicaExcel(pathLogo, idFicha, idElemento, ConstantesFichaTecnica.PORTAL, flagSoloEqActivo, incluirColumnaComentario, false, DateTime.Now, false,
                                                                out string ruta, out string nombre);

                    datos[0] = ruta;
                    datos[1] = nombre;

                    var jsonResult = Json(datos);
                    return jsonResult;
                }
                else
                {
                    throw new ArgumentException("Debe seleccionar una ficha técnica.");
                }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(new { Error = -1, Descripcion = ex.Message });
            }
        }

        /// <summary>
        /// Permite descargar el archivo 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarExcel()
        {
            string strArchivoTemporal = Request["archivo"];
            string strArchivoNombre = Request["nombre"];
            byte[] buffer = null;

            if (System.IO.File.Exists(strArchivoTemporal))
            {
                buffer = System.IO.File.ReadAllBytes(strArchivoTemporal);
                System.IO.File.Delete(strArchivoTemporal);
            }

            string strNombreArchivo = string.Format("{0}.xlsx", strArchivoNombre);

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, strNombreArchivo);
        }

        #endregion
    }
}
