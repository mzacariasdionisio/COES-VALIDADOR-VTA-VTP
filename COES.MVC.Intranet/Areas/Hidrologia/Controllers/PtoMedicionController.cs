using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Hidrologia.Helper;
using COES.MVC.Intranet.Areas.Hidrologia.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.DemandaMaxima;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Titularidad;
using COES.Servicios.Aplicacion.Transferencias;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using COES.Servicios.Aplicacion.DemandaMaxima;
using log4net;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using COES.Servicios.Aplicacion.Despacho;
using COES.Servicios.Aplicacion.Transferencias;
using COES.Servicios.Aplicacion.Titularidad;
using static COES.Dominio.DTO.Sic.EqEquipoDTO;


namespace COES.MVC.Intranet.Areas.Hidrologia.Controllers
{
    public class PtoMedicionController : BaseController
    {
        /// <summary>
        /// Codigo de propiedad de area operativa
        /// </summary>
        public int IdPropiedadArea = 1064;
        public int TipoDemandaBarra = 6;
       
        public int IdFormato = Int32.Parse(ConfigurationManager.AppSettings["IdFormatoPR16"]);
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(PtoMedicionController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        // GET: /Hidrologia/PtoMedicion/
        public FormatoMedicionAppServicio servicio;
        public DemandaMaximaAppServicio servicioDM;
        TitularidadAppServicio servTTIE = new TitularidadAppServicio();
        COES.Servicios.Aplicacion.General.EmpresaAppServicio empresaServicio = new COES.Servicios.Aplicacion.General.EmpresaAppServicio();

        public PtoMedicionController()
        {
            servicio = new FormatoMedicionAppServicio();
            servicioDM = new DemandaMaximaAppServicio();
            log4net.Config.XmlConfigurator.Configure();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("PtoMedicionController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("PtoMedicionController", ex);
                throw;
            }
        }

        /// <summary>
        /// Index de inicio de controller PtoMedicion
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int? codigo, int? tipoFuente)
        {
            BusquedaPtoMedicionModel model = new BusquedaPtoMedicionModel();
            model.ListaTipoPuntoMedicion = servicio.ListMeTipopuntomedicions("-1");
            model.ListaOrigenLectura = servicio.ListMeOrigenlecturas().Where(x => x.Origlectcodi != 0).OrderBy(x => x.Origlectnombre).ToList();
            //model.ListaTipoInformacion = servicio.ListSiTipoinformacions();
            model.ListaFamilia = servicio.ListarFamilia().Where(x => x.Famcodi != 0 && x.Famcodi != -1).OrderBy(x=>x.Famnomb).ToList();
            model.ListaEmpresas = servicio.ListarEmpresasPuntosMedicion();
            model.ListaAreas = servicio.ObtenerAreasPuntosMedicion();
            model.ListaTipoGrupo = (new DespachoAppServicio()).ListarTipoGrupo();
            bool flagEditar = Tools.VerificarAcceso(base.IdOpcion, base.UserName, Acciones.Editar);
            model.OpcionEspecial = base.VerificarAccesoAccion(Acciones.Adicional, base.UserName);
            model.OpcionEditar = flagEditar;

            var listaFuente = new List<GenericoDTO>();
            listaFuente.Add(new GenericoDTO() {Entero1 = 1,String1 = "EQUIPOS" });
            listaFuente.Add(new GenericoDTO() { Entero1 = 2, String1 = "GRUPOS" });
            listaFuente.Add(new GenericoDTO() { Entero1 = 3, String1 = "TRANSFERENCIA" });
            listaFuente.Add(new GenericoDTO() { Entero1 = 4, String1 = "NO DEFINIDO" });
            model.ListaFuente = listaFuente;

            model.FiltroPto = codigo != null ? codigo.Value.ToString() : "";
            model.TipoFuente = tipoFuente.GetValueOrDefault(0);
            #region FIT-Aplicativo VTD
            model.ListBarra = (new BarraAppServicio()).ListBarras();
            #endregion

            return View(model);
        }

        /// <summary>
        /// Devuelve vista parcial para mostrar paginado
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="tipoOrigenLectura"></param>
        /// <param name="tipoPtomedicodi"></param>
        /// <param name="idsFamilia"></param>
        /// <param name="nroPagina"></param>
        /// <returns></returns>
        [HttpPost]        
        public PartialViewResult Paginado(string empresas, string tipoOrigenLectura, string tipoPtomedicodi, string idsFamilia, string ubicacion,
            string categoria, int tipoPunto, int? codigo, int? cliente, int? barra,  int nroPagina)
        {
            if (codigo == null) codigo = -1;
            PtoMedicionModel model = new PtoMedicionModel();
            model.IndicadorPagina = false;

            if (tipoPunto == 1 && codigo > 0) idsFamilia = "-1";
            if (tipoPunto == 2 && codigo > 0) categoria = "-1";

            int nroRegistros = servicio.GetTotalPtomedicion(empresas, tipoOrigenLectura, tipoPtomedicodi, idsFamilia, ubicacion, categoria, tipoPunto,
                cliente, barra, (int)codigo);
            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSize;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView(model);
        }

        
        /// <summary>
        /// Devuelve vista parcial para mostrar listado de  ptos de medición
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="tipoOrigenLectura"></param>
        /// <param name="tipoPtomedicodi"></param>
        /// <param name="tipoEquipo"></param>
        /// <param name="nroPagina"></param>
        /// <param name="idsFamilia"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(string empresas, string tipoOrigenLectura, string tipoPtomedicodi, string tipoEquipo, int nroPagina,
            string idsFamilia, string ubicacion, string categoria, int tipoPunto, int? codigo, int? cliente, int? barra, string campo, string orden)
        {
            if (codigo == null) codigo = -1;
            PtoMedicionModel model = new PtoMedicionModel();

            if (tipoPunto == 1 && codigo > 0) idsFamilia = "-1";
            if (tipoPunto == 2 && codigo > 0) categoria = "-1";

            model.ListaPtoMedicion = servicio.ListarDetallePtoMedicion(empresas, tipoOrigenLectura, tipoPtomedicodi, nroPagina, Constantes.PageSize, idsFamilia,
                ubicacion, categoria, tipoPunto, (int)codigo, cliente, barra, campo, orden);
            bool flagEditar = Tools.VerificarAcceso(base.IdOpcion, base.UserName, Acciones.Editar);
            model.OpcionEditar = flagEditar;
            model.OpcionEspecial = base.VerificarAccesoAccion(Acciones.Adicional, base.UserName);
            model.IndicadorFuente = tipoPunto;
            return PartialView(model);
        }

        
        /// <summary>
        /// Permite exportar el resultado
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="tipoOrigenLectura"></param>
        /// <param name="tipoPtomedicodi"></param>
        /// <param name="tipoEquipo"></param>
        /// <param name="idsFamilia"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Exportar(string empresas, string tipoOrigenLectura, string tipoPtomedicodi, string tipoEquipo, string idsFamilia,
            string ubicacion, string categoria, int tipoPunto, int? codigo, int? cliente, int? barra)
        {
            try
            {
                if (codigo == null) codigo = -1;
                string path = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionHidrologia;
                string file = ConstantesHidrologia.ExportacionPuntosMedicion;
                int nroRegistros = servicio.GetTotalPtomedicion(empresas, tipoOrigenLectura, tipoPtomedicodi, idsFamilia, ubicacion,
                    categoria, tipoPunto, cliente, barra, (int)codigo);
                List<MePtomedicionDTO> list = servicio.ListarDetallePtoMedicion(empresas, tipoOrigenLectura, tipoPtomedicodi, 1, nroRegistros,
                    idsFamilia, ubicacion, categoria, tipoPunto, (int)codigo, cliente, barra, ConstantesHidrologia.CampoOrdenDefecto, ConstantesHidrologia.OrdenDefecto);

                ExcelDocument.ExportarPuntosMedicion(list, path, file, tipoOrigenLectura, tipoPunto);

                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }



        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Descargar()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionHidrologia + ConstantesHidrologia.ExportacionPuntosMedicion;
            return File(fullPath, Constantes.AppExcel, ConstantesHidrologia.ExportacionPuntosMedicion);
        }

        /// <summary>
        /// Agregar Punto de Medicion en BD
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Grabar(int empresa, int? equipocodi, int lectura, int ptomedicion, int orden, string barranomb, string elenomb,
            string osicodi, short tipopto, string descripcion, int? codRef, int tipoFuente, int? grupo, string estado,
            int? clientcodi, int? barracodi, string PuntoConexion, string areaOp, int? TipoSerie, int? familia)
        {
            int resultado = 0;
            this.ValidarSesionJsonResult();
            MePtomedicionDTO punto = null;
            bool ptoExiste = ptomedicion == 0 ? false : true;

            if (tipoFuente == 1)
            {
                punto = new MePtomedicionDTO();
                resultado = 1;
                int ptomedicodi = ptomedicion;
                int accion = Constantes.AccionEditar;
                if (ptomedicodi == 0)
                {
                    accion = Constantes.AccionNuevo;
                    var buscar = servicio.ListarPtoMedicionDuplicados((int)equipocodi, lectura, tipopto);

                    if ((familia==19) || (familia == 42)) {
                        var duplicado = servicio.ListarPtoMedicionDuplicadoNombreEmpresa(barranomb, (int)empresa);
                        if (duplicado.Count > 0)
                        {
                            // le colocamos el valor de 3 cuando existe mas de 1 registro con el mismo nombre y en la misma empresa
                            resultado = 3;
                            return Json(resultado);

                        }
                    }
                    
                    if (buscar.Count > 0)
                        //return Json(-1); //Existe Punto
                        resultado = 2;
                }
                else
                {
                    punto = servicio.GetByIdMePtomedicion(ptomedicodi);
                }

                punto.Ptomedicodi = ptomedicodi;
                punto.Lastuser = User.Identity.Name;
                punto.Lastdate = DateTime.Now;
                punto.Emprcodi = empresa;
                punto.Equicodi = equipocodi;
                punto.Origlectcodi = lectura;
                punto.Orden = orden;
                punto.Ptomedibarranomb = barranomb;
                punto.Ptomedielenomb = elenomb;
                punto.Ptomedidesc = descripcion;
                punto.Osicodi = osicodi;
                punto.Tipoptomedicodi = tipopto;
                punto.Ptomediestado = estado;
                punto.Grupocodi = grupo;
                punto.Clientecodi = clientcodi;
                punto.PuntoConexion = PuntoConexion;
                punto.TipoSerie = TipoSerie;
                punto.Areacodi = areaOp != "" ? int.Parse(areaOp) : -1;

                try
                {
                    if (accion == Constantes.AccionNuevo)
                    {
                        int grupocodi = -1;
                        var equipo = servicio.GetByIdEqequipo((int)equipocodi);
                        if (equipo != null)
                            if (equipo.Grupocodi != null)
                                grupocodi = (int)equipo.Grupocodi;
                        punto.Grupocodi = grupocodi;

                        if (codRef == null)
                            punto.Codref = PuntoMedicion.CodRef;
                        else
                            punto.Codref = codRef;

                        punto.Tipoinfocodi = PuntoMedicion.TipoInfoCodi;
                        //punto.Ptomediestado = PuntoMedicion.EstadoActivo;
                        punto.Ptomedicodi = servicio.SavePtoMedicion(punto);
                        Session["NvoPtoMedicion"] = punto.Ptomedicodi;
                    }
                    else
                    {
                        if (codRef == null)
                            punto.Codref = PuntoMedicion.CodRef;
                        else
                            punto.Codref = codRef;
                        servicio.UpdatePtoMedicion(punto);
                    }

                    this.servTTIE.SaveSiHisempptoDataInicial(punto.Emprcodi ?? 0, punto.Ptomedicodi, punto.Ptomediestado, User.Identity.Name);
                }
                catch (Exception ex)
                {
                    resultado = 0;
                }
            }
            else if(tipoFuente == 2)
            {
               // tipopto = 15; //es necesario grabar tipoptomedicodi  para puntos de medicion en PMPO

                punto = new MePtomedicionDTO();
                resultado = 1;
                int ptomedicodi = ptomedicion;
                int accion = Constantes.AccionEditar;
                if (ptomedicodi == 0)
                {
                    accion = Constantes.AccionNuevo;
                    var buscar = servicio.ListarPtoMedicionDuplicadosGrupo((int)grupo, lectura, tipopto);
                    if (buscar.Count > 0)                        
                        resultado = 2;
                }
                else
                {
                    punto = servicio.GetByIdMePtomedicion(ptomedicodi);
                }

                punto.Ptomedicodi = ptomedicodi;
                punto.Lastuser = User.Identity.Name;
                punto.Lastdate = DateTime.Now;
                punto.Emprcodi = empresa;
                punto.Grupocodi = grupo;
                punto.Equicodi = -1;
                punto.Origlectcodi = lectura;
                punto.Orden = orden;
                punto.Ptomedibarranomb = barranomb;
                punto.Ptomedielenomb = elenomb;
                punto.Ptomedidesc = descripcion;
                punto.Osicodi = osicodi;
                punto.Tipoptomedicodi = tipopto;
                punto.Ptomediestado = estado;
                punto.Clientecodi = clientcodi;
                punto.Areacodi = areaOp != "" ? int.Parse(areaOp) : -1;
                try
                {
                    if (accion == Constantes.AccionNuevo)
                    {
                        punto.Tipoinfocodi = PuntoMedicion.TipoInfoCodi;
                        //punto.Ptomediestado = PuntoMedicion.EstadoActivo;
                        punto.Ptomedicodi = servicio.SavePtoMedicion(punto);
                        Session["NvoPtoMedicion"] = punto.Ptomedicodi;
                    }
                    else
                    {                       
                        servicio.UpdatePtoMedicion(punto);
                    }

                    this.servTTIE.SaveSiHisempptoDataInicial(punto.Emprcodi ?? 0, punto.Ptomedicodi, punto.Ptomediestado, User.Identity.Name);
                }
                catch(Exception ex)
                {
                    resultado = 0;
                }
            }
            #region FIT - Aplicativo VTD
            else if (tipoFuente == 3)
            {
                punto = new MePtomedicionDTO();
                resultado = 1;
                int ptomedicodi = ptomedicion;
                int accion = Constantes.AccionEditar;
                if (ptomedicodi == 0)
                {
                    accion = Constantes.AccionNuevo;
                    var buscar = servicio.ListarPtoMedicionDuplicadosTransferencia((int)clientcodi, (int)barracodi, lectura, tipopto);
                    if (buscar.Count > 0)
                        resultado = 2;
                }
                else
                {
                    punto = servicio.GetByIdMePtomedicion(ptomedicodi);
                }

                punto.Ptomedicodi = ptomedicodi;
                punto.Lastuser = User.Identity.Name;
                punto.Lastdate = DateTime.Now;
                punto.Emprcodi = empresa;
                punto.Grupocodi = -1;
                punto.Equicodi = -1;
                punto.Clientecodi = clientcodi;
                punto.Barrcodi = barracodi;
                punto.Origlectcodi = lectura;
                punto.Orden = orden;
                punto.Ptomedibarranomb = barranomb;
                punto.Ptomedielenomb = elenomb;
                punto.Ptomedidesc = descripcion;
                punto.Osicodi = osicodi;
                punto.Tipoptomedicodi = tipopto;
                punto.Ptomediestado = estado;
                punto.Areacodi = areaOp != "" ? int.Parse(areaOp) : -1;
                try
                {
                    if (accion == Constantes.AccionNuevo)
                    {
                        punto.Tipoinfocodi = PuntoMedicion.TipoInfoCodi;
                        //punto.Ptomediestado = PuntoMedicion.EstadoActivo;
                        punto.Ptomedicodi = servicio.SavePtoMedicion(punto);
                        Session["NvoPtoMedicion"] = punto.Ptomedicodi;
                    }
                    else
                    {
                        servicio.UpdatePtoMedicion(punto);
                    }

                    this.servTTIE.SaveSiHisempptoDataInicial(punto.Emprcodi ?? 0, punto.Ptomedicodi, punto.Ptomediestado, User.Identity.Name);
                }
                catch
                {
                    resultado = 0;
                }

                return Json(resultado);
            }
            else
            {
                resultado = -1;
            }
            #endregion

            try
            {
                if (ptoExiste == false)
                {
                    COES.Dominio.DTO.Sic.SiEmpresaDTO empresaDTO = empresaServicio.ObtenerEmpresa(empresa);

                    if(empresaDTO.Tipoemprcodi == int.Parse(ConfigurationManager.AppSettings["EmpresaUsuarioLibreCodigo"]))
                    {
                        servicio.enviarCorreoUsuarioLibrePorCreacionEliminacion(empresaDTO, punto, "NUEVO");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("PtoMedicionController.Grabar", ex);

            }

            return Json(resultado);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="suministrador"></param>
        /// <param name="equipocodi"></param>
        /// <param name="area"></param>
        /// <param name="ptomedicion"></param>
        /// <param name="tension"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarDatosPtoSuministrador(int suministrador, int ptomedicion)
        {
            int resultado = -1;

            try
            {
                if (ptomedicion == 0)//NuevoPuntoMedicion
                {
                    if (Session["NvoPtoMedicion"] != null)
                    {
                        int IdPuntoMedicion = Convert.ToInt32(Session["NvoPtoMedicion"]);
                        //Se crea un registro con la relación PtoMedicion - Suministrador
                        MePtosuministradorDTO obj = new MePtosuministradorDTO();
                        obj.Emprcodi = suministrador;
                        obj.Ptosufechainicio = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                        obj.Ptosuusucreacion = User.Identity.Name;
                        obj.Ptosufeccreacion = DateTime.Now;
                        obj.Ptomedicodi = IdPuntoMedicion;
                        this.servicioDM.SaveMePtosuministro(obj);
                        resultado = 1;
                    }
                    else
                    {
                        resultado = -1;
                    }

                }
                else//Editar Punto de medición
                {
                    //PUNTO SUMINISTRADOR
                    var ptoMedicion = servicio.GetByIdMePtomedicion(ptomedicion);
                    var IdSuministrador = -1;

                    //var ListaSuministradores = this.servicioDM.ListaEditorPtoSuministro(DateTime.Today.ToString(Constantes.FormatoFecha), ptoMedicion.Emprcodi.Value, this.IdFormato);
                    //foreach (var oEmpSum in ListaSuministradores)
                    //{
                    //    if (oEmpSum.Ptomedicodi == ptoMedicion.Ptomedicodi)
                    //    {
                    //        var oAux = servicioDM.GetbyidPtoSuministrador(oEmpSum.Ptosucodi);
                    //        IdSuministrador = oAux.Emprcodi;
                    //    }
                    //}
                    var oSuministrador = this.servicioDM.ObtenerSuministradorVigente(ptomedicion);
                    if (oSuministrador != null)
                    {
                        IdSuministrador = oSuministrador.Emprcodi;
                    }
                    if (suministrador != IdSuministrador)//En caso Cod Suministrador sea diferente al almacenado, se crea nuevo registro.
                    {
                        MePtosuministradorDTO obj = new MePtosuministradorDTO();
                        obj.Emprcodi = suministrador;
                        obj.Ptosufechainicio = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                        obj.Ptosuusucreacion = User.Identity.Name;
                        obj.Ptosufeccreacion = DateTime.Now;
                        obj.Ptomedicodi = ptomedicion;
                        this.servicioDM.SaveMePtosuministro(obj);
                        resultado = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("PtoMedicionController.GrabarDatosPtoSuministrador", ex);
                resultado = -1;
            }
            
            

            return Json(resultado);
        }
        /// <summary>
        /// Método que permite actualizar la tension y ubicación de un suministro
        /// </summary>
        /// <param name="equipocodi">Código suministro</param>
        /// <param name="area">Código ubicación</param>
        /// <param name="tension">Valor Tensión</param>
        /// <returns></returns>
        public JsonResult ActualizarEquipoSuministro(int equipocodi, int area,string tension)
        {
            int resultado = -1;
            try
            {
                var oEquipo = (new EquipamientoAppServicio()).GetByIdEqEquipo(equipocodi);
                bool bActualizarEquipo = false;

                if (oEquipo.Areacodi != area)//Verificamos que la ubicacion sea la misma
                {
                    oEquipo.Areacodi = area;
                    bActualizarEquipo = true;
                }
                if (oEquipo.Equitension.HasValue)//Verificacmos que ala Tensión sea la misma
                {
                    if (oEquipo.Equitension.Value != Convert.ToDecimal(tension))
                    {
                        oEquipo.Equitension = Convert.ToDecimal(tension);
                        bActualizarEquipo = true;
                    }
                }
                else
                {
                    if (tension.Trim() != "0")
                    {
                        oEquipo.Equitension = Convert.ToDecimal(tension);
                        bActualizarEquipo = true;
                    }
                }

                if (bActualizarEquipo)
                {
                    oEquipo.UsuarioUpdate = User.Identity.Name;
                    (new EquipamientoAppServicio()).UpdateEqEquipo(oEquipo);
                }
                resultado = 1;
            }
            catch (Exception ex)
            {
                log.Error("PtoMedicionController.ActualizarEquipoSuministro", ex);
                resultado = -1;
            }
            return Json(resultado);
        }

        /// <summary>
        /// Permite mostrar las fuentes para obtener las fórmulas
        /// </summary>
        /// <param name="indicador"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarBarras(int? empresa)
        {
            List<EqEquipoDTO> barras = this.servicio.ObtenerEquiposPorFamilia(empresa.GetValueOrDefault(0), 7);
            SelectList list = new SelectList(barras, EntidadPropiedad.Equicodi, EntidadPropiedad.Equinomb);
            return Json(list);
        }

        /// <summary>
        /// Permite eliminar un punto de medición
        /// </summary>
        /// <param name="ptoMedicion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeletePunto(int ptoMedicion)
        {
            try
            {
                this.ValidarSesionJsonResult();
                MePtomedicionDTO puntoMedicion = servicio.GetByIdMePtomedicion(ptoMedicion);

                this.servTTIE.DeleteSiHisempptoDataByPuntoMedicion(ptoMedicion, User.Identity.Name);

                int result = this.servicio.EliminarPuntoMedicion(ptoMedicion, User.Identity.Name);

                if (puntoMedicion.Emprcodi > 0)
                {
                    int emprCodi = (int)puntoMedicion.Emprcodi;
                    COES.Dominio.DTO.Sic.SiEmpresaDTO empresaDTO = empresaServicio.ObtenerEmpresa(emprCodi);

                    if (empresaDTO.Tipoemprcodi == int.Parse(ConfigurationManager.AppSettings["EmpresaUsuarioLibreCodigo"]))
                    {
                        servicio.enviarCorreoUsuarioLibrePorCreacionEliminacion(empresaDTO, puntoMedicion, "ELIMINAR");
                    }
                }

                return Json(result);
            }
            catch(Exception ex)
            {
                log.Error(NameController, ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Devuelve Vista Parcial para popup de edicón pto de medición
        /// </summary>
        /// <returns></returns>
        public PartialViewResult Editar()
        {
            string codigo = "0";
            if (Request["id"] != null)
                codigo = Request["id"];
            int id = int.Parse(codigo);

            PtoMedicionModel model = new PtoMedicionModel();
            model.Ptomedicion = servicio.GetByIdMePtomedicion(id);
            model.IndicadorFuente = 1;
            if (model.Ptomedicion == null)
            {
                model.Ptomedicion = new MePtomedicionDTO();
                model.Ptomedicion.Emprcodi = -1;
                model.Ptomedicion.Famcodi = 0;
                model.Ptomedicion.Equicodi = 0;
                model.Ptomedicion.Origlectcodi = 0;
                model.Ptomedicion.Ptomedicodi = 0;
                model.Ptomedicion.Grupocodi = -1;
                model.Ptomedicion.Orden = 0;
                model.Ptomedicion.Ptomedibarranomb = string.Empty;
                model.Ptomedicion.Ptomedielenomb = string.Empty;
                model.Ptomedicion.Osicodi = string.Empty;
                model.Ptomedicion.Emprcodref = -1;
                model.Ptomedicion.Codref = -1;
                model.IdCategoria = -1;
                model.Ptomedicion.Ptomediestado = Constantes.EstadoActivo;
                model.Ptomedicion.TipoSerie = -1;

                model.Ptomedicion.Areacodi = -1;
                model.Ptomedicion.StrAreacodi = "-1";
            }
            else
            {
                //INC 2024-003075//
                EqEquipoDTO eq = new EqEquipoDTO();
                eq = servicio.GetByIdEqequipo(model.Ptomedicion.Equicodi ?? 0);
                if (model.Ptomedicion.Emprcodi != eq.Emprcodi)
                {
                    model.Ptomedicion.Emprcodi = eq.Emprcodi;
                }

                if (model.Ptomedicion.Areacodi <= 0)
                {
                    model.Ptomedicion.Areacodi = -1;
                    model.Ptomedicion.StrAreacodi = "-1";
                }
                else
                {
                    model.Ptomedicion.StrAreacodi = model.Ptomedicion.Areacodi.ToString();
                }

                if (model.Ptomedicion.Equicodi != null && model.Ptomedicion.Equicodi != -1)
                {
                    if (model.Ptomedicion.Orden == null)
                    {
                        model.Ptomedicion.Orden = -1;
                    }

                    model.Ptomedicion.Emprcodref = -1;

                    if (model.Ptomedicion.Codref != null)
                    {
                        if (model.Ptomedicion.Codref > 0)
                        {
                            int codRef = (int)model.Ptomedicion.Codref;
                            EqEquipoDTO barra = (new COES.Servicios.Aplicacion.Equipamiento.EquipamientoAppServicio()).GetByIdEqEquipo(codRef);
                            if (barra != null)
                            {
                                model.Ptomedicion.Emprcodref = barra.Emprcodi;
                            }

                            EqEquipoDTO equipo = (new EquipamientoAppServicio()).GetByIdEqEquipo((int)model.Ptomedicion.Codref);
                            if(equipo != null)
                                model.TensionBarra = equipo.Equitension;
                        }
                    }

                    string areaOperativa = (new EquipamientoAppServicio()).GetValorPropiedad(this.IdPropiedadArea, (int)model.Ptomedicion.Equicodi);
                    model.AreaOperativaEquipo = areaOperativa;

                    if (model.Ptomedicion.Origlectcodi == 19)//
                    {
                        var oSuministro = (new EquipamientoAppServicio()).GetByIdEqEquipo(model.Ptomedicion.Equicodi.Value);
                        var oFamilia = (new EquipamientoAppServicio()).GetByIdEqFamilia(oSuministro.Famcodi.Value);
                        model.TensionSuministro = oSuministro.Equitension;
                        model.IdAreaCodiSuministro = oSuministro.Areacodi.Value;

                        var oSuministrador = this.servicioDM.ObtenerSuministradorVigente(model.Ptomedicion.Ptomedicodi);
                        if (oSuministrador == null)
                        {
                            model.IdSuministrador = 0;
                        }
                        else
                        {
                            model.IdSuministrador = oSuministrador.Emprcodi;
                        }
                    }
                }
                else if (model.Ptomedicion.Grupocodi != null && model.Ptomedicion.Grupocodi != -1)
                {
                    model.IndicadorFuente = 2;

                    PrGrupoDTO entity = (new DespachoAppServicio()).GetByIdPrGrupo((int)model.Ptomedicion.Grupocodi);
                    model.IdCategoria = (int)entity.Catecodi;

                }
                else if (model.Ptomedicion.Clientecodi != null && model.Ptomedicion.Barrcodi != null)
                {
                    model.IndicadorFuente = 3;
                }

            }

            if (model.Ptomedicion.Equicodi != null && model.Ptomedicion.Equicodi != -1)
            {
                if (model.Ptomedicion.Origlectcodi == 19)//
                {
                    var oSuministro = (new EquipamientoAppServicio()).GetByIdEqEquipo(model.Ptomedicion.Equicodi.Value);
                    var oFamilia = (new EquipamientoAppServicio()).GetByIdEqFamilia(oSuministro.Famcodi.Value);
                    model.TensionSuministro = oSuministro.Equitension;
                    model.ListaAreasSuministro = (new EquipamientoAppServicio()).ListaTodasAreasActivasPorTipoArea(oFamilia.Tareacodi.Value);
                }
            }

            model.ListaTipoSerie = servicio.ListarTipoSerie();
            model.ListaEmpresas = servicio.ListarEmpresas().Where(x => x.EMPRCODI > 0).ToList();
            model.ListaFamilia = servicio.ListarFamilia();
            model.ListaEquipo = model.Ptomedicion.Clientecodi > 0 && model.Ptomedicion.Origlectcodi == 32 ? this.servicio.ObtenerEquiposPorFamilia((int)model.Ptomedicion.Clientecodi, model.Ptomedicion.Famcodi) : this.servicio.ObtenerEquiposPorFamilia((int)model.Ptomedicion.Emprcodi, model.Ptomedicion.Famcodi);
            model.ListaTipoPuntoMedicion = servicio.ListMeTipopuntomedicions(model.Ptomedicion.Origlectcodi.ToString());
            model.ListaOrigenLectura = servicio.ListMeOrigenlecturas().OrderBy(x => x.Origlectnombre).ToList();

            model.ListaEmpresasSuministradoras = servicioDM.ListaEmpresasSuministrador();

            model.IdCategoriaPadre = -1;
            model.ListaCategoriaSuperior = new List<EqCategoriaDTO>();
            model.ListaCategoria = new List<EqCategoriaDTO>();
            model.ListaSubclasificacion = new List<EqCategoriaDetDTO>();
            model.ListaTipoGrupo = (new DespachoAppServicio()).ListarTipoGrupo();

            #region FIT-Aplicativo VTD
            model.ListBarra = (new BarraAppServicio()).ListBarras();
            #endregion

            return PartialView(model);
        }

        /// <summary>
        /// Permite devolver los grupos
        /// </summary>
        /// <param name="categoria"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerGrupos(int categoria, int? idEmpresa)
        {
            List<string> listaEstado = new List<string>() { "A", "P", "F"};
            List<PrGrupoDTO> list = (new DespachoAppServicio()).ObtenerGruposPorCategoria(categoria, ConstantesAppServicio.ParametroDefecto, -1, -1)
                                        .Where(x=> listaEstado.Contains(x.GrupoEstado)).ToList();
            if (idEmpresa > 0) list = list.Where(x => x.Emprcodi == idEmpresa.Value).ToList();

            var jsonResult = Json(list);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult ObtenerGrupos2(int? idEmpresa)
        {
            List<PrGrupoDTO> listT = (new DespachoAppServicio()).ObtenerGruposPorCategoria(3, Constantes.EstadoActivo, -1, -1);
            List<PrGrupoDTO> listH = (new DespachoAppServicio()).ObtenerGruposPorCategoria(5, Constantes.EstadoActivo, -1, -1);
            List<PrGrupoDTO> listE = (new DespachoAppServicio()).ObtenerGruposPorCategoria(17, Constantes.EstadoActivo, -1, -1);
            List<PrGrupoDTO> listS = (new DespachoAppServicio()).ObtenerGruposPorCategoria(15, Constantes.EstadoActivo, -1, -1);
            if (idEmpresa > 0) listT = listT.Where(x => x.Emprcodi == idEmpresa.Value).ToList();
            if (idEmpresa > 0) listH = listH.Where(x => x.Emprcodi == idEmpresa.Value).ToList();
            if (idEmpresa > 0) listE = listE.Where(x => x.Emprcodi == idEmpresa.Value).ToList();
            if (idEmpresa > 0) listS = listS.Where(x => x.Emprcodi == idEmpresa.Value).ToList();

            List<PrGrupoDTO> list = new List<PrGrupoDTO>();
            list.AddRange(listT);
            list.AddRange(listH);
            list.AddRange(listE);
            list.AddRange(listS);

            var jsonResult = Json(list);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// devuelve listado de equipo para filtro equipo
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idFamilia"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarEquipos(int? idEmpresa, int? idFamilia, int? idCtgdet)
        {
            SelectList list;
            if (idCtgdet == null || idCtgdet <= 0)
            {
                List<EqEquipoDTO> entitys  = this.servicio.ObtenerEquiposPorFamilia(idEmpresa.GetValueOrDefault(0), idFamilia.GetValueOrDefault(0));
                list = new SelectList(entitys, EntidadPropiedad.EquiCodi, EntidadPropiedad.EquiNomb);
            }
            else
            {
                List<EqCategoriaEquipoDTO> entitys = (new EquipamientoAppServicio()).ListaClasificacionByCategoriaAndEmpresa(idCtgdet.Value, idEmpresa.GetValueOrDefault(0));
                list = new SelectList(entitys, EntidadPropiedad.EquiCodi, EntidadPropiedad.EquiNomb);
            }

            return Json(list);
        }

        /// <summary>
        /// Devuelve listado para filtro de tipo de ptos
        /// </summary>
        /// <param name="tipoOrigenLectura"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarTipoPto(string tipoOrigenLectura)
        {
            List<MeTipopuntomedicionDTO> lista = new List<MeTipopuntomedicionDTO>();
            lista = servicio.ListMeTipopuntomedicions(tipoOrigenLectura);
            SelectList list = new SelectList(lista, EntidadPropiedad.Tipoptomedicodi, EntidadPropiedad.Tipoptomedinomb);
            return Json(list);
        }

        /// <summary>
        /// Devuelve listado para filtro de tipo de ptos que pertenecen a series hidrologicas
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarTipoPtoHidro()
        {
            List<TipoPuntoMedicion> lista = new List<TipoPuntoMedicion>();
            lista = servicio.ListarTipoPuntoMedicion();
            SelectList list = new SelectList(lista, EntidadPropiedad.Tipoptomedicodi, EntidadPropiedad.Tipoptomedinomb);
            return Json(list);
        }
        /// <summary>
        /// Devuelve Vista Parcial para tipo de pto de medición
        /// </summary>
        /// <param name="tipoOrigenLectura"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult TipoPtoMedicion(string tipoOrigenLectura)
        {
            BusquedaPtoMedicionModel model = new BusquedaPtoMedicionModel();
            List<MeTipopuntomedicionDTO> lista = new List<MeTipopuntomedicionDTO>();
            lista = servicio.ListMeTipopuntomedicions(tipoOrigenLectura);

            model.ListaTipoPuntoMedicion = lista;
            return PartialView(model);
        }

        
        /// <summary>
        /// Muestra la tensión de un equipo
        /// </summary>
        /// <param name="idEquipo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarTension(int idEquipo)
        {
            EqEquipoDTO entidad = (new EquipamientoAppServicio()).GetByIdEqEquipo(idEquipo);            
            return Json(entidad.Equitension);
        }

        /// <summary>
        /// Permite obtener el área operativa del equipo
        /// </summary>
        /// <param name="idEquipo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerAreaOperativa(int idEquipo)
        {
            string areaOperativa = (new EquipamientoAppServicio()).GetValorPropiedad(this.IdPropiedadArea, idEquipo);
            return Json(areaOperativa);
        }

        /// <summary>
        /// Permite actualizar el area operativa del equipo
        /// </summary>
        /// <param name="idEquipo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActualizarAreaOperativa(int idEquipo, string valor)
        {
            try
            {
                EqPropequiDTO propiedad = new EqPropequiDTO
                {
                    Equicodi = idEquipo,
                    Propcodi = this.IdPropiedadArea,
                    Valor = valor,
                    Propequiusucreacion = base.UserName,
                    Propequifeccreacion = DateTime.Now,
                    Fechapropequi = DateTime.Today,
                };

                (new EquipamientoAppServicio()).SaveEqPropequi(propiedad);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        [HttpPost]
        public JsonResult CargarAreas(int iFamilia)
        {
            var oFamilia = (new EquipamientoAppServicio()).GetByIdEqFamilia(iFamilia);
            var iTipoArea = oFamilia.Tareacodi;
            var entitys = (new EquipamientoAppServicio()).ListaTodasAreasActivasPorTipoArea(iTipoArea.Value).Select(t=>new {Areacodi=t.Areacodi,Areanomb=t.Areaabrev.Trim()+" - "+t.Areanomb.Trim()});
            var list = new SelectList(entitys, "Areacodi", "Areanomb");
            return Json(list);
        }
        /// <summary>
        /// Muestra la ubicación de un equipo
        /// </summary>
        /// <param name="idEquipo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarAreaEquipo(int idEquipo)
        {
            EqEquipoDTO entidad = (new EquipamientoAppServicio()).GetByIdEqEquipo(idEquipo);
            return Json(entidad.Areacodi);
        }

        //inicio agregado
        [HttpPost]
        public JsonResult ListaCategoria(int idFamilia, int ctgPadre, int idEmpresa)
        {
            try
            {
                List<EqCategoriaDTO> listaValida = new List<EqCategoriaDTO>();
                List<EqCategoriaDTO> listaCategoria = (new EquipamientoAppServicio()).ListCategoriaHijoByIdPadreAndEmpresa(idFamilia, ctgPadre, idEmpresa);
                listaCategoria = listaCategoria.Where(x => x.Ctgestado == Constantes.EstadoActivo).ToList();
                var codi = 0;
                foreach (var ctg in listaCategoria)
                {
                    if (codi == ctg.Ctgcodi)
                    {
                        ctg.Ctgnomb = ctg.Ctgnomb + " - Con subcategorías";
                        codi = 0;
                    }
                    else
                    {
                        codi = ctg.Ctgcodi;
                    }

                    if (ctg.TotalEquipo > 0)
                    {
                        listaValida.Add(ctg);
                    }
                }

                SelectList list = new SelectList(listaValida, "Ctgcodi", "Ctgnomb");
                return Json(list);
            }
            catch (Exception ex)
            {
                log.Error("CategoriaController", ex);
                return Json(-1);
            }
        }

        [HttpPost]
        public JsonResult ListaSubclasificacion(int idCtg, int idEmpresa)
        {
            try
            {
                List<EqCategoriaDetDTO> listaValida = new List<EqCategoriaDetDTO>();
                var listaSub = (new EquipamientoAppServicio()).ListEqCategoriaDetalleByCategoriaAndEmpresa(idCtg, idEmpresa);
                listaSub = listaSub.Where(x => x.Ctgdetestado == Constantes.EstadoActivo).ToList();
                foreach (var det in listaSub)
                {
                    if (det.TotalEquipo > 0)
                    {
                        listaValida.Add(det);
                    }
                }

                SelectList list = new SelectList(listaValida, "Ctgdetcodi", "Ctgdetnomb");
                return Json(list);
            }
            catch (Exception ex)
            {
                log.Error("CategoriaController", ex);
                return Json(-1);
            }
        }
        //fin agregado
    }
}
