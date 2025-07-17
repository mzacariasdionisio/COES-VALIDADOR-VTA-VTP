using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Despacho.Helper;
using COES.MVC.Intranet.Areas.Despacho.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Despacho;
using COES.Servicios.Aplicacion.Despacho.Helper;
using COES.Servicios.Aplicacion.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Despacho.Controllers
{
    public class GrupoDespachoController : BaseController
    {
        /// <summary>
        /// Instancia de la clase servicio
        /// </summary>
        GrupoDespachoAppServicio servicio = new GrupoDespachoAppServicio();

        /// <summary>
        /// Permite mostrar la página inicial
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();

            GrupoDespachoModel model = new GrupoDespachoModel();
            model.ListaEmpresas = this.servicio.ObtenerListaEmpresas();
            model.Fecha = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.IndicadorConfigCMgN = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            model.Editar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);

            return View(model);
        }

        /// <summary>
        /// Permite otener las centrales por tipo de central y empresas
        /// </summary>
        /// <param name="tipoCentral"></param>
        /// <param name="empresas"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Central(string tipoCentral, string empresas)
        {
            GrupoDespachoModel model = new GrupoDespachoModel();
            List<PrGrupoDTO> list = this.servicio.ObtenerCentrales(tipoCentral, empresas);
            model.ListaCentral = list;

            return PartialView(model);
        }

        /// <summary>
        /// Permite visualizar el arbol
        /// </summary>
        /// <param name="mensajes"></param>
        /// <returns></returns>
        public PartialViewResult Arbol(string empresas, string tipoCentral, string central)
        {
            List<PrGrupoDTO> list = this.servicio.ObtenerArbolGrupos(empresas, tipoCentral, central);
            ViewBag.ArbolGrupo = GrupoDespachoHelper.ObtenerArbolGrupo(list);

            return PartialView();
        }

        /// <summary>
        /// Permite obtener los datos para las curvas de consumo
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="grupo"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CurvaConsumo(string empresas, string grupo, string fecha, string tipoCentral, string central)
        {
            try
            {
                DateTime fechaDatos = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                List<CurvaConsumo> list = this.servicio.ObtenerPametrosCurva(empresas, grupo, fechaDatos, tipoCentral, central);

                return Json(list);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Actualiza el dato de curva de despacho
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActualizarCurva(int id, int tipo)
        {
            try
            {
                this.servicio.ActualizarCurvaCMgN(id, tipo.ToString(), base.UserName);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        #region Curva consumo

        /// <summary>
        /// Permite obtener los datos para las curvas de consumo
        /// </summary>        
        /// <param name="grupocodi"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CurvaConsumoPorGrupo(string grupocodi, string fecha)
        {
            try
            {
                DateTime fechaDatos = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                List<CurvaConsumo> list = this.servicio.ObtenerPametrosCurvaPorGrupoCodi(grupocodi, fechaDatos);

                return Json(list);
            }
            catch
            {
                return Json(-1);
            }
        }
        /// <summary>
        /// Permite exportar al formato NCP
        /// </summary>
        /// <param name="fecha">Fecha consulta</param>           
        /// <returns></returns>
        public JsonResult GenerarArchivoTextoReporteNCP(string fecha)
        {
            int indicador = 1;
            try
            {
                DateTime fechaDatos = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                List<CurvaConsumo> list = this.servicio.ObtenerPametrosCurvaPorFecha(fechaDatos).OrderBy(x=>x.GrupocodiNCP).ToList();

                string codigo = "";
                string nSeg = "";
                string ger1 = "";
                string cons1 = "";
                string ger2 = "";
                string cons2 = "";
                string ger3 = "";
                string cons3 = "";
                string ger4 = "";
                string cons4 = "";

                int puntos = 0;

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesDespacho.FolderReporte;
                using (System.IO.StreamWriter file = FileServer.OpenWriterFile(ConstantesDespacho.NombreReporteEnviosTexto, ruta))
                {
                    file.WriteLine("!Cod #Seg ....GER1.... ...CONS1.... ....GER2.... ...CONS2.... ....GER3.... ...CONS3.... ....GER4.... ...CONS4....");

                    foreach (var item in list)
                    {
                        codigo = item.GrupocodiNCP.ToString();

                        ger1 = "";
                        cons1 = "";
                        ger2 = "";
                        cons2 = "";
                        ger3 = "";
                        cons3 = "";
                        ger4 = "";
                        cons4 = "";

                        puntos = 0;

                        foreach (var punto in item.ListaSerie.SerieConsumo)
                        {
                            puntos += 1;
                            switch (puntos)
                            {
                                case 1:
                                    {
                                        ger1 = punto.PuntoX.ToString();
                                        cons1 = punto.PuntoY.ToString();
                                    }
                                    break;
                                case 2:
                                    {
                                        ger2 = punto.PuntoX.ToString();
                                        cons2 = punto.PuntoY.ToString();
                                    }
                                    break;
                                case 3:
                                    {
                                        ger3 = punto.PuntoX.ToString();
                                        cons3 = punto.PuntoY.ToString();
                                    }
                                    break;
                                case 4:
                                    {
                                        ger4 = punto.PuntoX.ToString();
                                        cons4 = punto.PuntoY.ToString();
                                    }
                                    break;
                            }
                        }

                        nSeg = puntos.ToString();

                        if (puntos > 0)
                        {

                            file.WriteLine(codigo.PadLeft(4) +
                                   nSeg.PadLeft(5) +
                                   ger1.PadLeft(13) +
                                   cons1.PadLeft(13) +
                                   ger2.PadLeft(13) +
                                   cons2.PadLeft(13) +
                                   ger3.PadLeft(13) +
                                   cons3.PadLeft(13) +
                                   ger4.PadLeft(13) +
                                   cons4.PadLeft(13));
                        }
                    }

                }
                indicador = 1;
            }
            catch
            {
                indicador = -1;
            }
            return Json(indicador);
        }

        /// <summary>
        /// Permite exportar a texto reporte NCP
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult ExportarTexto()
        {
            string nombreArchivo = string.Empty;
            nombreArchivo = ConstantesDespacho.NombreReporteEnviosTexto;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesDespacho.FolderReporte;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, "text/plain", nombreArchivo);
        }
        

        public JsonResult SaveGrupodat(string iGrupoCodi, string curvcodi, string sValor, string sNcp, string sFechaDat)
        {
            int indicador = 1;
            try
            {

                if (curvcodi == "0")
                {
                    PrGrupodatDTO oDatoParametro = new PrGrupodatDTO();
                    var usuario = User.Identity.Name;

                    oDatoParametro.Deleted = 0;
                    oDatoParametro.Concepcodi = 243;
                    oDatoParametro.Fechaact = DateTime.Now;
                    oDatoParametro.Fechadat = DateTime.ParseExact(sFechaDat, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    oDatoParametro.Formuladat = sValor;
                    oDatoParametro.Grupocodi = int.Parse(iGrupoCodi);
                    oDatoParametro.Lastuser = usuario + " " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();

                    var rpta = this.servicio.SaveGrupodat(oDatoParametro);


                    if (rpta != "")
                        indicador = 2;
                    else
                    {
                        //servicio.ActualizarCodigoNCP(Convert.ToInt32(sNcp), int.Parse(iGrupoCodi));
                        //indicador = 1;
                    }
                }
                else
                {

                    List<String> grupos = iGrupoCodi.Split(ConstantesAppServicio.CaracterComa).ToList();
                    List<String> valores = sValor.Split(ConstantesAppServicio.CaracterComa).ToList();

                    for (int i = 0; i < valores.Count; i++)
                    {

                        PrGrupodatDTO oDatoParametro = new PrGrupodatDTO();
                        var usuario = User.Identity.Name;

                        oDatoParametro.Deleted = 0;
                        oDatoParametro.Concepcodi = 243;
                        oDatoParametro.Fechaact = DateTime.Now;
                        oDatoParametro.Fechadat = DateTime.ParseExact(sFechaDat, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                        oDatoParametro.Formuladat = valores[i];
                        oDatoParametro.Grupocodi = int.Parse(grupos[i]);
                        oDatoParametro.Lastuser = usuario + " " +  DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();

                        var rpta = this.servicio.SaveGrupodat(oDatoParametro);


                        if (rpta != "")
                            indicador = 2;
                        else
                        {
                            //servicio.ActualizarCodigoNCP(Convert.ToInt32(sNcp), int.Parse(grupos[i]));
                            //indicador = 1;
                        }
                    }
                }
            }
            catch
            {
                indicador = -1;
            }

            return Json(indicador);
        }

        public JsonResult obtenerNCP(int id)
        {
            int indicador = 0;
            try
            {
                indicador = this.servicio.ObtenerCodigoNCP(id);
            }
            catch
            {
                indicador = 0;
            }

            return Json(indicador);
        }

        /// <summary>
        /// Permite grabar el identificador NCP del grupo de despacho
        /// </summary>
        /// <param name="id"></param>
        /// <param name="codigoncp"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarIdentificadorNcp(int id, string codigoncp)
        {
            try
            {
                servicio.ActualizarCodigoNCP(Convert.ToInt32(codigoncp), id);                
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        [HttpPost]
        public JsonResult CurvaConsumoGrupoId(string empresas, string grupo, string fecha, string tipoCentral, string central, string grupoId)
        {
            try
            {
                //se obtiene la curva de consumo que tiene por grupoId
                DateTime fechaDatos = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                List<CurvaConsumo> list = this.servicio.ObtenerPametrosCurva(empresas, grupo, fechaDatos, tipoCentral, central);
                List<CurvaConsumo> subList = list.Where(x => x.Grupocodi == int.Parse(grupoId)).ToList();
                string fechaDato = this.servicio.ObtenerFechaEdicion(int.Parse(grupoId));
                
                CurvaConsumo obj = subList.FirstOrDefault();
                bool esSimple = false;
               
                
                if (obj.Curvcodi == 0 )
                {
                    esSimple = true;
                }else 
                {
                    PrGrupodatDTO objPrg = this.servicio.BuscaIDCurvaPrincipal(obj.Curvcodi);
                    if (obj.Curvcodi != 0 && obj.Grupocodi != objPrg.Curvgrupocodiprincipal)
                    {
                        esSimple = true;
                    }else
                    {
                        esSimple = false;
                    }
                }

                if (esSimple)
                {
                    List<CurvaConsumo> subListJuntos = new List<CurvaConsumo>();
                    subListJuntos.Add(obj);

                    return Json(new { datos = subListJuntos, fechaDatos = fechaDato });
                }
                else
                {
                    List<CurvaConsumo> subListJuntos = list.Where(x => x.Curvcodi == obj.Curvcodi).ToList();
                    //return Json(subListJuntos);
                    return Json(new { datos = subListJuntos, fechaDatos = fechaDato });
                }
            }
            catch
            {
                return Json(-1);
            }
        }
       
        #endregion 
    }
}
