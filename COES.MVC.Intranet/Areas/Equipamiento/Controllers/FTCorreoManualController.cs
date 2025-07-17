using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Equipamiento.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.SeguridadServicio;
using COES.Servicios.Aplicacion.Correo;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Equipamiento.Helper;
using COES.Servicios.Aplicacion.Eventos.Helper;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Equipamiento.Controllers
{
    public class FTCorreoManualController : BaseController
    {
        readonly FichaTecnicaAppServicio servicioFT = new FichaTecnicaAppServicio();
        readonly EquipamientoAppServicio appEquipamiento = new EquipamientoAppServicio();

        #region Declaración de variables

        readonly SeguridadServicioClient seguridad = new SeguridadServicioClient();
        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(FTCorreoManualController));
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

        /// <summary>
        /// Nombre del archivo
        /// </summary>
        public String RelacionCorreoEmpresa
        {
            get
            {
                return (Session[ConstantesFichaTecnica.SesionRelacionEmpresaCorreo] != null) ?
                    Session[ConstantesFichaTecnica.SesionRelacionEmpresaCorreo].ToString() : null;
            }
            set { Session[ConstantesFichaTecnica.SesionRelacionEmpresaCorreo] = value; }
        }

        #endregion


        #region Funciones
        /// <summary>
        /// Pantalla principal
        /// </summary>
        /// <param name="carpeta"></param>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            FTCorreoManualModel model = new FTCorreoManualModel();

            model.ListaEmpresas = servicioFT.ListarEmpresasExtranetFT();
            
            DateTime hoy = DateTime.Now;
            model.FechaInicio = hoy.AddMonths(-1).ToString(ConstantesAppServicio.FormatoFecha);
            model.FechaFin = hoy.ToString(ConstantesAppServicio.FormatoFecha);
            model.TienePermisoAdmin = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            model.ListadoProyectos = new List<FtExtProyectoDTO>();

            return View(model);
        }

        /// <summary>
        /// Lista los correos enviados a los agentes
        /// </summary>
        /// <param name="rangoIni"></param>
        /// <param name="rangoFin"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarCorreosEnviados(string idsEmpresa, string rangoIni, string rangoFin)
        {
            FTCorreoManualModel model = new FTCorreoManualModel();

            try
            {
                DateTime fechaInicio = DateTime.ParseExact(rangoIni, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFin = DateTime.ParseExact(rangoFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                model.TienePermisoAdmin = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                model.ListadoCorreosEnviados = servicioFT.ListarCorreosEnviados(idsEmpresa, fechaInicio, fechaFin);
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }


        /// <summary>
        /// Obtener datos de la plantilla del correo manual
        /// </summary>
        /// <param name="corrcodi"></param>
        /// <param name="idsEmpresas"></param>
        /// <param name="accion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerDatosDelCorreo(int corrcodi, string idsEmpresas, int accion)
        {
            FTCorreoManualModel model = new FTCorreoManualModel();

            try
            {
                List<int> emprcodis = new List<int>();
                base.ValidarSesionJsonResult();
                model.TienePermisoAdmin = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                bool hayCorreosEmpresas = true;

                SiCorreoDTO correo;
                if (corrcodi != 0)//Ver detalles de un correo
                {
                    correo = (new CorreoAppServicio()).GetByIdSiCorreo(corrcodi);
                    List<SiCorreoArchivoDTO> lstArchivos = servicioFT.ObtenerArchivosAdjuntados(corrcodi);

                    correo.Archivos = lstArchivos.Any() ? string.Join("/", lstArchivos.Select(x => x.Earchnombreoriginal).OrderBy(x => x).ToList()) : "";
                    model.ListadoProyectos = new List<FtExtProyectoDTO>();
                }
                else // nuevo correo
                {
                    correo = new SiCorreoDTO();

                    //Obtenemos los correos de las empresas seleccionadas
                    string lstCorreosAgentes = "";

                    if(idsEmpresas != "")
                    {
                        lstCorreosAgentes = ObtenerStrcorreosAgentePorEmpresa(idsEmpresas);
                        if (lstCorreosAgentes == "")
                        {
                            hayCorreosEmpresas = false;
                            //throw new Exception("No se encontró correos de agentes para la(s) empresa(s) seleccionada(s).");
                        }

                        List<string> lstStrIdEmpresas = idsEmpresas.Split(',').ToList();
                        foreach (var strId in lstStrIdEmpresas)
                        {
                            emprcodis.Add(Convert.ToInt32(strId));
                        }
                    }

                    //Agregamos firma en el pie de pagina
                    StringBuilder strHtml = new StringBuilder();
                    strHtml.Append("<p>&nbsp;</p>");
                    strHtml.Append("<div style='color: #222222; font-family: Arial, Helvetica, sans-serif; font-size: small; padding-left: 0px;'>&nbsp;</div>");
                    strHtml.Append("<div style='color: #222222; font-family: Arial, Helvetica, sans-serif; font-size: small; padding-left: 0px;'>&nbsp;</div>");
                    strHtml.Append("<div style='color: #222222; font-family: Arial, Helvetica, sans-serif; font-size: small; padding-left: 0px;'>&nbsp;</div>");
                    strHtml.Append("<div style='color: #222222; font-family: Arial, Helvetica, sans-serif; font-size: small; padding-left: 0px;'>&nbsp;</div>");
                    strHtml.Append("<div style='color: #222222; font-family: Arial, Helvetica, sans-serif; font-size: small; padding-left: 0px;'>&nbsp;</div>");
                    strHtml.Append("<div style='color: #222222; font-family: Arial, Helvetica, sans-serif; font-size: small; padding-left: 0px;'>");
                    strHtml.Append("<p class='MsoNormal' style='color: #000000; font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px;'><img class='CToWUd' src='https://www.coes.org.pe/Portal/Content/Images/logomail.jpg' alt='Logo Coes' width='127' height='66' /></p>");
                    strHtml.Append("<p class='MsoNormal' style='color: #000000; font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px;'><strong><span style='font-size: 8.0pt; color: #0077a5;'>D:</span></strong><span style='font-size: 8.0pt;'>&nbsp;Av. Los Conquistadores N&deg; 1144, San Isidro, Lima - Per&uacute;<u></u><u></u></span></p>");
                    strHtml.Append("<p class='MsoNormal' style='color: #000000; font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px;'><strong><span style='font-size: 8.0pt; color: #0077a5;'>T:</span></strong>&nbsp;<span style='font-size: 8.0pt;'>+51 611 8585 - Anexo: 657 / 593<u></u><u></u></span></p>");
                    strHtml.Append("<p class='MsoNormal' style='color: #000000; font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px;'><strong><span style='font-size: 8.0pt; color: #0077a5;'>W:</span></strong>&nbsp;<a href='http://www.coes.org.pe' target='_blank'><span style='font-size: 8.0pt; color: #0563c1;'>www.coes.org.pe</span></a></p>");
                    strHtml.Append("</div>");

                    
                    correo.Corrto = lstCorreosAgentes;
                    correo.Corrcontenido = strHtml.ToString();
                    model.ListadoProyectos = appEquipamiento.ListarProyectosExistentes().Where(x => emprcodis.Contains(x.Emprcodi)).OrderBy(x=>x.Ftprynombre).ToList();
                }
                

                model.Correo = correo;

                if(hayCorreosEmpresas)
                    model.Resultado = "1";
                else
                    model.Resultado = "0";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Obtiene los datos de los equipos para el proyecto seleccionado
        /// </summary>
        /// <param name="idProyecto"></param>
        /// <param name="idsEmpresas"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerEquipoPorProyecto(int idProyecto, string idsEmpresas)
        {
            FTCorreoManualModel model = new FTCorreoManualModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                List<FTRelacionEGP> listaTotal = new List<FTRelacionEGP>();

                List<string> lstStrIdEmpresas = idsEmpresas.Split(',').ToList();
                List<int> lstEmpresas = new List<int>();
                foreach (var strId in lstStrIdEmpresas)
                {
                    bool esEntero = int.TryParse(strId, out int numero);
                    if (esEntero)
                        lstEmpresas.Add(numero);
                }
                foreach (var emprcodi in lstEmpresas)
                {
                    List<FTRelacionEGP> listaTemp = servicioFT.ObtenerMOYEquiposRelacionadosAlProyecto(ConstantesFichaTecnica.PorDefecto, idProyecto, emprcodi, 1);

                    listaTotal.AddRange(listaTemp);
                }
                
                var listaAgrupada = listaTotal.GroupBy(x => new { x.Codigo, x.Equicodi, x.Grupocodi, x.EquipoNomb });
                List<FTRelacionEGP> listaFinal = new List<FTRelacionEGP>();
                foreach (var item in listaAgrupada)
                {
                    FTRelacionEGP obj = new FTRelacionEGP();
                    obj.Codigo = item.Key.Codigo;
                    obj.Equicodi = item.Key.Equicodi;
                    obj.Grupocodi = item.Key.Grupocodi;
                    obj.EquipoNomb = item.Key.EquipoNomb;
                    listaFinal.Add(obj);
                }

                model.ListadoRelacionEGP = listaFinal.OrderBy(x => x.EquipoNomb).ToList();

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Obtener Datos de equipos seleccionados
        /// </summary>
        /// <param name="strIdsSeleccionados"></param>
        /// <param name="idProyecto"></param>
        /// <param name="idsEmpresas"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerDatosEquiposSeleccionados(string strIdsSeleccionados, int idProyecto, string idsEmpresas)
        {
            FTCorreoManualModel model = new FTCorreoManualModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                List<FTRelacionEGP> listaTotal = new List<FTRelacionEGP>();
                List<FTRelacionEGP> listaFinal = new List<FTRelacionEGP>();

                List<string> lstStrIdEmpresas = idsEmpresas.Split(',').ToList();
                List<int> lstEmpresas = new List<int>();
                foreach (var strId in lstStrIdEmpresas)
                {
                    bool esEntero = int.TryParse(strId, out int numero);
                    if (esEntero)
                        lstEmpresas.Add(numero);
                }

                foreach (var emprcodi in lstEmpresas)
                {
                    List<FTRelacionEGP> listaTemp = servicioFT.ObtenerMOYEquiposRelacionadosAlProyecto(ConstantesFichaTecnica.PorDefecto, idProyecto, emprcodi, 1);
                    listaTotal.AddRange(listaTemp);
                }

                //filtrar los seleccionados
                List<string> lstSeleccionados = strIdsSeleccionados.Split(',').ToList();
                foreach (var grupoSel in lstSeleccionados)
                {
                    string[] arrSeleccionados = grupoSel.Split('_').ToArray();
                    int codigo = Convert.ToInt32(arrSeleccionados[0]);
                    string esEquicodi = arrSeleccionados[1];
                    string esGrupocodi = arrSeleccionados[2];

                    List<FTRelacionEGP> listaTemp = new List<FTRelacionEGP>();
                    if (esEquicodi == "S")
                        listaTemp = listaTotal.Where(x => x.Codigo == arrSeleccionados[0] && x.Equicodi == codigo).ToList();

                    if (esGrupocodi == "S")
                        listaTemp = listaTotal.Where(x => x.Codigo == arrSeleccionados[0] && x.Grupocodi == codigo).ToList();

                    listaFinal.AddRange(listaTemp);
                }

                model.ListadoRelacionEGP = listaFinal.OrderBy(x => x.EquipoNomb).ToList();

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }
        
        /// <summary>
        /// correos agentes en string por empresas
        /// </summary>
        /// <param name="idsEmpresas"></param>
        /// <returns></returns>
        private string ObtenerStrcorreosAgentePorEmpresa(string idsEmpresas)
        {
            List<string> lstStrIdEmpresas = idsEmpresas.Split(',').ToList();
            List<int> lstEmpresas = new List<int>();
            foreach (var strId in lstStrIdEmpresas)
            {
                bool esEntero = int.TryParse(strId, out int numero);
                if (esEntero)
                    lstEmpresas.Add(numero);
            }
            List<string> listaCorreo = new List<string>();
            List<string> listaRelaciones = new List<string>();
            foreach (var emprcodi in lstEmpresas)
            {
                List<string> listaCorreoTemp = ObtenerCorreosGeneradorModuloFT(emprcodi);
                if (listaCorreoTemp.Any())
                {
                    listaCorreoTemp = listaCorreoTemp.OrderBy(x => x).ToList();
                    listaCorreo.AddRange(listaCorreoTemp);


                    string correo = listaCorreoTemp.First();

                    string[] words = correo.Split('@');
                    string dominio = words.Length > 1 ? words[1].Trim() : "";

                    if(dominio != "")
                    {
                        string relacion = emprcodi + "#" + dominio;
                        listaRelaciones.Add(relacion);
                    }
                }
                    
            }

            this.RelacionCorreoEmpresa = string.Join("/", listaRelaciones);


            return string.Join(";", listaCorreo);
        }

        /// <summary>
        /// Consultar correos por empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        private List<string> ObtenerCorreosGeneradorModuloFT(int idEmpresa)
        {
            List<string> listaCorreo = new List<string>();

            //modulos extranet
            var listaModuloExtr = seguridad.ListarModulos().Where(x => (x.RolName.StartsWith("Usuario Extranet") || x.RolName.StartsWith("Extranet")) && x.ModEstado.Equals(ConstantesAppServicio.Activo)).OrderBy(x => x.ModNombre).ToList();

            //considerar solo a los usuarios activos de la empresa
            var listaUsuarios = seguridad.ListarUsuariosPorEmpresa(idEmpresa).Where(x => x.UserState == ConstantesAppServicio.Activo).ToList();
            foreach (var regUsuario in listaUsuarios)
            {
                var listaModuloXUsu = seguridad.ObtenerModulosPorUsuarioSelecion(regUsuario.UserCode).ToList();

                //modulos que tiene el usuario en extranet
                var listaModuloXUsuExt = listaModuloXUsu.Where(x => listaModuloExtr.Any(y => y.ModCodi == x.ModCodi)).ToList();

                var regFT = listaModuloXUsuExt.Find(x => x.ModCodi == ConstantesFichaTecnica.ModcodiFichaTecnicaExtranet); //Cambiar a 44 cuando se pase a CERTIFICACION
                if (regFT != null && regFT.Selected > 0) //si tiene check opción activa
                {
                    listaCorreo.Add((regUsuario.UserEmail ?? "").ToLower().Trim());
                }
            }

            return listaCorreo;
        }

        /// <summary>
        /// Envia correos al agente
        /// </summary>
        /// <param name="correo"></param>
        /// <param name="idsEmpresas"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EnviarCorreo(SiCorreoDTO correo, string idsEmpresas)
        {
            FTCorreoManualModel model = new FTCorreoManualModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                //Direccion temporal donde se suben los archivos
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesEnviarCorreos.RutaCorreo;

                //Archivos
                List<string> listFiles = new List<string>();
                string files = correo.Archivos;
                if (!string.IsNullOrEmpty(files))
                    listFiles = files.Split('/').ToList();

                //Obtengo id empresasa
                List<string> lstStrIdEmpresas = idsEmpresas.Split(',').ToList();
                List<int> lstEmpresas = new List<int>();
                foreach (var strId in lstStrIdEmpresas)
                {
                    bool esEntero = int.TryParse(strId, out int numero);
                    if (esEntero)
                        lstEmpresas.Add(numero);
                }
                lstEmpresas = lstEmpresas.Distinct().ToList();

                //Obtengo los correos por empresa
                Dictionary<int, string> relacionesD = new Dictionary<int, string>();
                if (this.RelacionCorreoEmpresa != "")
                {
                    List<string> lstRelaciones = this.RelacionCorreoEmpresa.Split('/').ToList();
                    
                    foreach (var relacion in lstRelaciones)
                    {
                        List<string> lstvalores = relacion.Split('#').ToList();
                        int emprcodi = Convert.ToInt32(lstvalores.First());
                        string dominio = lstvalores.Last().Trim();
                        relacionesD.Add(emprcodi, dominio);
                    }
                }

                string correosTotales = correo.Corrto;
                List<string> lstCorreosEmpresasTotales = correosTotales.Split(';').ToList();

                //Obtengo correos que no pertenezcan al grupo de empresas seleccionadas
                List<string> lstCorreosEmpresasSeleccionadas = new List<string>();
                foreach (int emprcodi in lstEmpresas)
                {
                    if (relacionesD.ContainsKey(emprcodi))
                    {
                        //filtro correos por empresa
                        string dominio1 = relacionesD[emprcodi];

                        if (dominio1 != "")
                        {
                            List<string> lstDestinatarios = correosTotales.Split(';').ToList();
                            List<string> lstDestinatariosSoloEmpresa = lstDestinatarios.Where(x => x.Contains(dominio1)).ToList();

                            if (lstDestinatariosSoloEmpresa.Any())
                            {
                                lstCorreosEmpresasSeleccionadas.AddRange(lstDestinatariosSoloEmpresa);                                
                            }

                        }
                    }                    
                }

                //Envio correos
                List<string> lstCorreosQueNoPertenecenAlGrupoEmpresasSeleccionadas = (List<string>)lstCorreosEmpresasTotales.Except(lstCorreosEmpresasSeleccionadas).ToList();                
                foreach (int emprcodi in lstEmpresas)
                {
                    if (relacionesD.ContainsKey(emprcodi))
                    {
                        //filtro correos por empresa
                        string dominio = relacionesD[emprcodi];

                        if (dominio != "")
                        {
                            List<string> lstDestinatarios = correosTotales.Split(';').ToList();
                            List<string> lstDestinatariosSoloEmpresa = lstDestinatarios.Where(x => x.Contains(dominio)).ToList();

                            if (lstDestinatariosSoloEmpresa.Any())
                            {
                                string correosFinales = string.Join(";", lstDestinatariosSoloEmpresa);
                                correo.Corrto = correosFinales;
                                correo.Emprcodi = emprcodi;
                                servicioFT.EnviarMensajeAgentes(correo, base.UserName, path, listFiles);
                            }

                        }
                    }
                    else
                    {
                        if (lstCorreosQueNoPertenecenAlGrupoEmpresasSeleccionadas.Any())
                        {
                            string correosFinalesSE = string.Join(";", lstCorreosQueNoPertenecenAlGrupoEmpresasSeleccionadas);
                            correo.Corrto = correosFinalesSE;
                            correo.Emprcodi = emprcodi;
                            servicioFT.EnviarMensajeAgentes(correo, base.UserName, path, listFiles);
                        }
                    }
                    
                }

                
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }


        /// <summary>
        /// Carga archivos adjuntados
        /// </summary>
        /// <param name="chunks"></param>
        /// <param name="chunk"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upload()
        {
            try
            {
                base.ValidarSesionUsuario();

                if (Request.Files.Count == 1)
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesEnviarCorreos.RutaCorreo;
                    var file = Request.Files[0];
                    string fileName = path + file.FileName;
                    //this.NombreFile = fileName;

                    if (FileServer.VerificarExistenciaFile(null, file.FileName, path))
                    {
                        FileServer.DeleteBlob(file.FileName, path);
                    }

                    file.SaveAs(fileName);

                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Descarga los archivos adjuntados
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="corrcodi"></param>
        /// <returns></returns>
        public virtual ActionResult DescargarArchivoEnvio(string fileName, int corrcodi)
        {
            base.ValidarSesionUsuario();

            byte[] buffer = servicioFT.GetBufferArchivoAdjuntoCorreo(corrcodi, fileName);
            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        #endregion
    }
}