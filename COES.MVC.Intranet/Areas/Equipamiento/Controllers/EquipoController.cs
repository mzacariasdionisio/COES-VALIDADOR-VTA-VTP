using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Equipamiento.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Equipamiento.Helper;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Titularidad;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Equipamiento.Controllers
{
    public class EquipoController : BaseController
    {
        //
        // GET: /Equipamiento/Equipo/
        GeneralAppServicio appGeneral = new GeneralAppServicio();
        EquipamientoAppServicio appEquipamiento = new EquipamientoAppServicio();
        TitularidadAppServicio servTTIE = new TitularidadAppServicio();
        DespachoAppServicio servDespacho = new DespachoAppServicio();

        #region Declaración de variables de Sesión

        private int IdOpcionEnvioFormato = ConstantesFichaTecnica.IdoptionAdminFicha;

        readonly List<EstadoModel> lsEstadosEquipo = new List<EstadoModel>();
        readonly List<DatoComboBox> listadoSINO = new List<DatoComboBox>();

        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().Name);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        private readonly List<int> _listCentrales = new List<int>();
        private readonly List<int> _listGeneradores = new List<int>();
        public EquipoController()
        {
            lsEstadosEquipo.Add(new EstadoModel { EstadoCodigo = "A", EstadoDescripcion = "Activo" });
            lsEstadosEquipo.Add(new EstadoModel { EstadoCodigo = "B", EstadoDescripcion = "Baja" });
            lsEstadosEquipo.Add(new EstadoModel { EstadoCodigo = "F", EstadoDescripcion = "Fuera de COES" });
            lsEstadosEquipo.Add(new EstadoModel { EstadoCodigo = "P", EstadoDescripcion = "Proyecto" });
            lsEstadosEquipo.Add(new EstadoModel { EstadoCodigo = "X", EstadoDescripcion = "Anulado" });

            listadoSINO.Add(new DatoComboBox { Descripcion = "SI", Valor = "S" });
            listadoSINO.Add(new DatoComboBox { Descripcion = "NO", Valor = "N" });

            //no usar el Constructor para llamar a consultas de BD, cada vez que se ejecuta algún JsonResult se utiliza este constructor haciendo lento el uso de este módulo

            _listCentrales.Add(4);
            _listCentrales.Add(5);
            _listCentrales.Add(37);
            _listCentrales.Add(39);

            _listGeneradores.Add(2);
            _listGeneradores.Add(3);
            _listGeneradores.Add(36);
            _listGeneradores.Add(38);
            log4net.Config.XmlConfigurator.Configure();
        }

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

        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            var modelo = new IndexEquipoModel();
            modelo.ListaTipoEmpresa =
                appGeneral.ListarTiposEmpresa().Where(t => t.Tipoemprcodi > 0).OrderBy(t => t.Tipoemprdesc).ToList();
            modelo.ListaTipoEquipo =
                appEquipamiento.ListEqFamilias().Where(t => t.Famcodi > 0).OrderBy(t => t.Famnomb).ToList();
            modelo.ListaEstados = lsEstadosEquipo;
            modelo.iEmpresa = 0;
            modelo.iTipoEmpresa = 0;
            modelo.iTipoEquipo = 0;
            modelo.sEstadoCodi = "A";
            modelo.ListaEmpresa = new List<SiEmpresaDTO>();

            modelo.TienePermiso = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);// verificar rol
            modelo.TienePermisoAdminFT = base.VerificarAccesoAccionXOpcion(Acciones.Grabar, base.UserName, this.IdOpcionEnvioFormato);

            bool AccesoNuevo = modelo.TienePermiso;
            bool AccesoEditar = modelo.TienePermiso;

            modelo.EnableNuevo = AccesoNuevo ? "" : "disabled=disabled";
            modelo.EnableEditar = AccesoEditar ? "" : "disableClick";

            modelo.AccesoNuevo = AccesoNuevo;
            modelo.AccesoEditar = AccesoEditar;

            return View(modelo);
        }

        #region Listado, Registro / Edición de equipos

        //[HttpPost]
        public JsonResult CargarEmpresas(int idTipoEmpresa)
        {
            var entitys = new List<SiEmpresaDTO>();
            entitys = this.appGeneral.ListadoComboEmpresasPorTipo(idTipoEmpresa).Where(t => t.Emprestado.Trim() != "E").ToList(); ;            
            var list = new SelectList(entitys, "Emprcodi", "Emprnomb");
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public PartialViewResult ListadoEquipos(IndexEquipoModel model)
        {
            int iEmpresa = model.iEmpresa;
            int iFamilia = model.iTipoEquipo;
            int iTipoEmpresa = model.iTipoEmpresa;
            int iEquipo = string.IsNullOrEmpty(model.CodigoEquipo) ? -2 : Convert.ToInt32(model.CodigoEquipo.Trim());
            string sEstado = model.sEstadoCodi;
            string sNombre = model.NombreEquipo;
            var lsResultado = appEquipamiento.ListaEquipamientoPaginado(iEmpresa, iFamilia, iTipoEmpresa, iEquipo,
                sEstado, sNombre, model.NroPagina, Constantes.PageSizeEvento);
            foreach (var oEquipo in lsResultado)
            {
                oEquipo.EstadoDesc = EquipamientoHelper.EstadoDescripcion(oEquipo.Equiestado);
                oEquipo.Osigrupocodi = EquipamientoHelper.EstiloEstado(oEquipo.Equiestado);

            }
            model.ListadoEquipamiento = lsResultado;
            model.TienePermisoAdminFT = base.VerificarAccesoAccionXOpcion(Acciones.Grabar, base.UserName, this.IdOpcionEnvioFormato);
            return PartialView(model);
        }

        [HttpPost]
        public PartialViewResult PaginadoEquipos(IndexEquipoModel model)
        {
            int iEmpresa = model.iEmpresa;
            int iFamilia = model.iTipoEquipo;
            int iTipoEmpresa = model.iTipoEmpresa;
            int iEquipo = string.IsNullOrEmpty(model.CodigoEquipo) ? -2 : Convert.ToInt32(model.CodigoEquipo.Trim());
            string sEstado = model.sEstadoCodi;
            string sNombre = model.NombreEquipo;
            model.IndicadorPagina = false;
            int nroRegistros = appEquipamiento.TotalEquipamiento(iEmpresa, iFamilia, iTipoEmpresa, iEquipo, sEstado,
                sNombre);

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSizeEvento;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }
            return PartialView(model);
        }

        /// <summary>
        /// Opción "Ver"
        /// </summary>
        /// <param name="iEquipo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult DetalleEquipo(int iEquipo)
        {
            var oEquipo = appEquipamiento.GetByIdEqEquipo(iEquipo);
            string strPadre = "";
            if (oEquipo.Equipadre.HasValue)
            {
                var oEquiPadre = appEquipamiento.GetByIdEqEquipo(oEquipo.Equipadre.Value);
                if (oEquiPadre != null)
                    strPadre = string.IsNullOrEmpty(oEquiPadre.Equinomb) ? string.Empty : oEquiPadre.Equinomb;
            }

            var modelo = new DetalleEquipoModel
            {
                Equicodi = oEquipo.Equicodi,
                AREANOMB = string.IsNullOrEmpty(oEquipo.AREANOMB) ? string.Empty : oEquipo.AREANOMB,
                EMPRNOMB = string.IsNullOrEmpty(oEquipo.EMPRNOMB) ? string.Empty : oEquipo.EMPRNOMB,
                Ecodigo = string.IsNullOrEmpty(oEquipo.Ecodigo) ? string.Empty : oEquipo.Ecodigo,
                EquiManiobraDesc = EquipamientoHelper.SiNoDescripcion(oEquipo.EquiManiobra),
                Equiabrev = string.IsNullOrEmpty(oEquipo.Equiabrev) ? string.Empty : oEquipo.Equiabrev,
                Equiabrev2 = string.IsNullOrEmpty(oEquipo.Equiabrev2) ? string.Empty : oEquipo.Equiabrev2,
                EquiestadoDesc = EquipamientoHelper.EstadoDescripcion(oEquipo.Equiestado),
                Equifechfinopcom =
                    oEquipo.Equifechfinopcom.HasValue
                        ? oEquipo.Equifechfinopcom.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
                        : "",
                Equifechiniopcom =
                    oEquipo.Equifechiniopcom.HasValue
                        ? oEquipo.Equifechiniopcom.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
                        : "",
                Equinomb = string.IsNullOrEmpty(oEquipo.Equinomb) ? string.Empty : oEquipo.Equinomb,
                Equipot = oEquipo.Equipot,
                Equitension = oEquipo.Equitension,
                Famnomb = string.IsNullOrEmpty(oEquipo.Famnomb) ? string.Empty : oEquipo.Famnomb,
                Lastdate = oEquipo.Lastdate,
                Lastuser = string.IsNullOrEmpty(oEquipo.Lastuser) ? string.Empty : oEquipo.Lastuser,
                Osinergcodi = string.IsNullOrEmpty(oEquipo.Osinergcodi) ? string.Empty : oEquipo.Osinergcodi,
                EquipadreDesc = strPadre
            };

            return PartialView(modelo);
        }

        /// <summary>
        /// Opción "Nuevo"
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult NuevoEquipo()
        {
            var listaEmpresas = this.appGeneral.ListadoComboEmpresasPorTipo(-2).Where(t => t.Emprestado.Trim() != "E").ToList();
            var listaFamilias = appEquipamiento.ListEqFamilias().Where(t => t.Famcodi > 0).OrderBy(t => t.Famnomb).ToList();
            var listaUbicaciones = appEquipamiento.ListaTodasAreasActivas();
            var listaGrupoDespacho = servDespacho.ListarPrGrupoDespacho();

            var modelo = new DetalleEquipoModel
            {
                ListaEmpresa = listaEmpresas.Where(x => x.Emprestado.Trim() == "A").ToList(),
                ListaEstados = lsEstadosEquipo,
                ListaProcManiobras = listadoSINO,
                ListaTipoEquipo = listaFamilias,
                ListaUbicaciones = listaUbicaciones,
                ListaEquipos =
                    appEquipamiento.ListEqEquipos().Where(e => e.Equiestado == "A" || e.Equiestado == "P").ToList(),
                ListaOperadores = listaEmpresas,
                ListaGrupo = listaGrupoDespacho
            };
            return PartialView(modelo);
        }

        [HttpPost]
        public JsonResult CargarAreas(int iFamilia)
        {
            var oFamilia = appEquipamiento.GetByIdEqFamilia(iFamilia);
            var iTipoArea = oFamilia.Tareacodi;
            var entitys = this.appEquipamiento.ListaTodasAreasActivasPorTipoArea(iTipoArea.Value);
            var list = new SelectList(entitys, "Areacodi", "Areanomb");
            return Json(list);
        }

        [HttpPost]
        public JsonResult CargarEquiposPadres(int iEmpresa, int iArea)
        {
            List<EqEquipoDTO> lsPadres = new List<EqEquipoDTO>();
            lsPadres = appEquipamiento.ObtenerEquipoPorAreaEmpresa(iEmpresa, iArea).ToList();
            var list = new SelectList(lsPadres, "EQUICODI", "Valor");
            return Json(list);
        }
        [HttpPost]
        public JsonResult CargarEquiposPadresHidro()
        {
            List<EqEquipoDTO> lsPadres = new List<EqEquipoDTO>();
            lsPadres = appEquipamiento.ObtenerEquipoPadresHidrologicosCuenca().ToList();
            var list = new SelectList(lsPadres, "EQUICODI", "EQUINOMB");
            return Json(list);
        }
        [HttpPost]
        public JsonResult CargarEquiposPadresEdit(int iEmpresa, int iArea)
        {
            List<EqEquipoDTO> lsPadres = new List<EqEquipoDTO>();
            lsPadres = appEquipamiento.ObtenerEquipoPorAreaEmpresaTodos(iEmpresa, iArea).ToList();
            var list = new SelectList(lsPadres, "EQUICODI", "EQUINOMB");
            return Json(list);
        }
        [HttpPost]
        public JsonResult CargarEquiposPadresHidroEdit()
        {
            List<EqEquipoDTO> lsPadres = new List<EqEquipoDTO>();
            lsPadres = appEquipamiento.ObtenerEquipoPadresHidrologicosCuencaTodos().ToList();
            var list = new SelectList(lsPadres, "EQUICODI", "EQUINOMB");
            return Json(list);
        }

        [HttpPost]
        public JsonResult CargarListadoGrupoDespacho()
        {
            var entitys = servDespacho.ListarPrGrupoDespacho();
            var list = new SelectList(entitys, "Grupocodi", "Gruponomb");
            return Json(list);
        }


        [HttpPost]
        public JsonResult GuardarEquipo(DetalleEquipoModel oEquipo)
        {
            try
            {
                this.ValidarSesionJsonResult();

                int grupocodi = -1;
                if (_listCentrales.Contains(oEquipo.Famcodi.Value) || _listGeneradores.Contains(oEquipo.Famcodi.Value))
                    grupocodi = oEquipo.Grupocodi ?? -1;

                bool esCentral = _listCentrales.Contains(oEquipo.Famcodi.Value);
                var oEquipoNuevo = new EqEquipoDTO
                {
                    Famcodi = oEquipo.Famcodi,
                    Lastdate = DateTime.Today,
                    Lastuser = base.UserName,
                    Areacodi = oEquipo.Areacodi,
                    Ecodigo = oEquipo.Ecodigo,
                    Elecodi = oEquipo.Elecodi,
                    Emprcodi = oEquipo.Emprcodi,
                    EquiManiobra = oEquipo.EquiManiobra,
                    Equiabrev = oEquipo.Equiabrev,
                    Equiabrev2 = oEquipo.Equiabrev2,
                    Equiestado = oEquipo.Equiestado,
                    Equifechfinopcom =
                        string.IsNullOrEmpty(oEquipo.Equifechfinopcom)
                            ? (DateTime?)null
                            : DateTime.ParseExact(oEquipo.Equifechfinopcom, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Equifechiniopcom =
                        string.IsNullOrEmpty(oEquipo.Equifechiniopcom)
                            ? (DateTime?)null
                            : DateTime.ParseExact(oEquipo.Equifechiniopcom, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Equinomb = oEquipo.Equinomb,
                    Equipadre = oEquipo.Equipadre,
                    Equipot = oEquipo.Equipot,
                    Osinergcodi = oEquipo.Osinergcodi,
                    Equitension = oEquipo.Equitension,
                    Operadoremprcodi = oEquipo.Operadoremprcodi,
                    Grupocodi = grupocodi
                };
                if (esCentral)
                {
                    if (oEquipo.Equipadre == null)
                    {
                        oEquipoNuevo.Equipadre = 0;
                    }
                    else
                    {
                        oEquipoNuevo.Equipadre = oEquipo.Equipadre;
                    }
                }
                else
                {
                    oEquipoNuevo.Equipadre = oEquipo.Equipadre;
                }

                //1. Crea el nuevo equipo asignado un código
                var equicodinuevo = appEquipamiento.SaveEqEquipo(oEquipoNuevo);
                oEquipoNuevo.Equicodi = equicodinuevo;

                //2. Almacena cada campo general como histórico del equipo
                appEquipamiento.RegistrarHistoricoCreacionEquipo(oEquipoNuevo, base.UserName);

                //3. Crea la relación de inicio del equipo con la empresa para el TTIE.
                servTTIE.SaveSiHisempeqDataInicial(oEquipoNuevo.Emprcodi ?? 0, equicodinuevo, oEquipo.Equiestado, base.UserName);

                return Json(1);
            }
            catch (Exception ex)
            {
                Log.Error("GuardarEquipo", ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Opción "Editar"
        /// </summary>
        /// <param name="iEquipo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult EditarEquipo(int iEquipo)
        {
            ValidarSesionUsuario();
            var oEquipo = appEquipamiento.GetByIdEqEquipo(iEquipo);
            var oFamilia = appEquipamiento.GetByIdEqFamilia(oEquipo.Famcodi.Value);

            var listaEmpresas = this.appGeneral.ListadoComboEmpresasPorTipo(-2).Where(t => t.Emprestado.Trim() != "E").ToList();
            var listaFamilias = appEquipamiento.ListEqFamilias().Where(t => t.Famcodi > 0).OrderBy(t => t.Famnomb).ToList();
            var listaGrupoDespacho = servDespacho.ListarPrGrupoDespacho();

            var modelo = new DetalleEquipoModel
            {
                ListaEmpresa = listaEmpresas,
                ListaEstados = lsEstadosEquipo,
                ListaProcManiobras = listadoSINO,
                ListaTipoEquipo = listaFamilias,
                ListaUbicaciones = this.appEquipamiento.ListaTodasAreasActivasPorTipoArea(oFamilia.Tareacodi.Value),
                ListaEquipos = appEquipamiento.ListarEquiposxFiltro(oEquipo.Emprcodi.Value, "AF", 0, 0, "", -99),
                ListaOperadores = listaEmpresas,
                ListaGrupo = listaGrupoDespacho,
                // ListEqEquipos().Where(e => e.Equiestado == "A" || e.Equiestado == "P").ToList(),
                Famcodi = oEquipo.Famcodi,
                Areacodi = oEquipo.Areacodi,
                Ecodigo = oEquipo.Ecodigo,
                Elecodi = oEquipo.Elecodi,
                Emprcodi = oEquipo.Emprcodi,
                EquiManiobra = oEquipo.EquiManiobra,
                Equiabrev = oEquipo.Equiabrev,
                Equiabrev2 = oEquipo.Equiabrev2,
                Equiestado = oEquipo.Equiestado,
                Equifechfinopcom =
                    oEquipo.Equifechfinopcom.HasValue
                        ? oEquipo.Equifechfinopcom.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
                        : "",
                Equifechiniopcom =
                    oEquipo.Equifechiniopcom.HasValue
                        ? oEquipo.Equifechiniopcom.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
                        : "",
                Equinomb = oEquipo.Equinomb,
                Equipadre = oEquipo.Equipadre,
                Equipot = oEquipo.Equipot,
                Osinergcodi = oEquipo.Osinergcodi,
                OsinergcodiGen = oEquipo.OsinergcodiGen,            // ticket-6068
                Equitension = oEquipo.Equitension,
                Equicodi = oEquipo.Equicodi,
                EMPRNOMB = oEquipo.EMPRNOMB,
                Famnomb = oEquipo.Famnomb,
                Operadoremprcodi = oEquipo.Operadoremprcodi,
                Grupocodi = oEquipo.Grupocodi
            };
            return PartialView(modelo);
        }

        [HttpPost]
        public JsonResult ActualizarEquipo(DetalleEquipoModel oEquipo)
        {
            try
            {
                this.ValidarSesionJsonResult();

                bool esCentral = _listCentrales.Contains(oEquipo.Famcodi.Value);

                var oEquipoNuevo = appEquipamiento.GetByIdEqEquipo(oEquipo.Equicodi);

                oEquipoNuevo.Famcodi = oEquipo.Famcodi;
                oEquipoNuevo.UsuarioUpdate = base.UserName;
                oEquipoNuevo.Areacodi = oEquipo.Areacodi ?? -1;
                oEquipoNuevo.Ecodigo = oEquipo.Ecodigo;
                //oEquipoNuevo.Elecodi = oEquipo.Elecodi;
                oEquipoNuevo.Emprcodi = oEquipo.Emprcodi;
                oEquipoNuevo.EquiManiobra = oEquipo.EquiManiobra;
                oEquipoNuevo.Equiabrev = oEquipo.Equiabrev;
                oEquipoNuevo.Equiabrev2 = oEquipo.Equiabrev2;
                oEquipoNuevo.Equiestado = oEquipo.Equiestado;
                //oEquipoNuevo.Equifechfinopcom = string.IsNullOrEmpty(oEquipo.Equifechfinopcom) ? (DateTime?) null : DateTime.ParseExact(oEquipo.Equifechfinopcom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //oEquipoNuevo.Equifechiniopcom = string.IsNullOrEmpty(oEquipo.Equifechiniopcom) ? (DateTime?) null : DateTime.ParseExact(oEquipo.Equifechiniopcom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                oEquipoNuevo.Equinomb = oEquipo.Equinomb;
                if (esCentral)
                {
                    if (oEquipo.Equipadre == null)
                    {
                        oEquipoNuevo.Equipadre = 0;
                    }
                    else
                    {
                        oEquipoNuevo.Equipadre = oEquipo.Equipadre;
                    }
                }
                else
                {
                    oEquipoNuevo.Equipadre = oEquipo.Equipadre;
                }
                //oEquipoNuevo.Equipadre = oEquipo.Equipadre;
                oEquipoNuevo.Equipot = oEquipo.Equipot;
                oEquipoNuevo.Osinergcodi = oEquipo.Osinergcodi;
                oEquipoNuevo.OsinergcodiGen = oEquipo.OsinergcodiGen;                       // ticket-6068
                oEquipoNuevo.Operadoremprcodi = oEquipo.Operadoremprcodi;
                oEquipoNuevo.Equitension = oEquipo.Equitension;

                int grupocodi = oEquipoNuevo.Grupocodi ?? -1;
                if (_listCentrales.Contains(oEquipo.Famcodi.Value) || _listGeneradores.Contains(oEquipo.Famcodi.Value))
                    grupocodi = oEquipo.Grupocodi ?? -1;
                oEquipoNuevo.Grupocodi = grupocodi;

                EqEquipoDTO oEquipoOriginal = appEquipamiento.GetByIdEqEquipo(oEquipo.Equicodi);

                //1. Actualiza los datos del equipo
                appEquipamiento.UpdateEqEquipo(oEquipoNuevo);

                //2. Almacena cada cambio del campo general como histórico del equipo 
                appEquipamiento.RegistrarHistoricoEdicionEquipo(oEquipoOriginal, oEquipoNuevo, base.UserName);

                return Json(1);
            }
            catch (Exception ex)
            {
                Log.Error("ActualizarEquipo", ex);
                return Json(-1);
            }
        }

        public JsonResult ValidarCambioEstado(int equicodi, string equiestado)
        {
            int resultado = -1;
            EqEquipoDTO equipo = appEquipamiento.GetByIdEqEquipo(equicodi);

            if (equiestado.Trim() == equipo.Equiestado.Trim())
            {
                resultado = 1;
            }
            else
            {
                if (equiestado.Trim() == "B" || equiestado.Trim() == "X")
                {
                    var equipos = appEquipamiento.ListarEquiposPorPadre(equicodi);
                    if (equipos != null)
                    {
                        int equiposActivos = equipos.Where(t => t.Equiestado == "A").Count();
                        if (equiposActivos > 0)
                        {
                            resultado = -1;
                        }
                        else
                        {
                            resultado = 1;
                        }
                    }
                }
                else
                {
                    resultado = 1;
                }

            }
            return Json(new { success = resultado });
        }


        /// <summary>
        /// Permite mostrar el mapa de coordenadas
        /// </summary>
        /// <param name="idEquipo"></param>
        /// <returns></returns>        
        public ViewResult Mapa(int idEquipo, string emprnomb, string famnomb, string equinomb)
        {
            ValidarSesionUsuario();
            string coordenadaX = string.Empty;
            string coordenadaY = string.Empty;
            this.appEquipamiento.ObtenerCoordenadasEquipo(idEquipo, out coordenadaX, out coordenadaY);

            MapaModel model = new MapaModel();
            model.Emprnomb = emprnomb;
            model.Equicodi = idEquipo;
            model.Tipoequipo = famnomb;
            model.Equinomb = equinomb;
            model.CoordenadaX = coordenadaX;
            model.CoordenadaY = coordenadaY;

            return View(model);
        }

        /// <summary>
        /// Permite grabar los coordenadas de un equipo
        /// </summary>
        /// <param name="idEquipo"></param>
        /// <param name="coordenadaX"></param>
        /// <param name="coordenadaY"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarCoordenada(int idEquipo, string coordenadaX, string coordenadaY)
        {
            try
            {
                this.appEquipamiento.GrabarCoordenadas(idEquipo, coordenadaX, coordenadaY, base.UserName);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        [HttpPost]
        public JsonResult ExportarListaEquipamiento(IndexEquipoModel model)
        {
            int result = 1;
            try
            {
                int iEmpresa = model.iEmpresa;
                int iFamilia = model.iTipoEquipo;
                int iTipoEmpresa = model.iTipoEmpresa;
                int iEquipo = string.IsNullOrEmpty(model.CodigoEquipo) ? -2 : Convert.ToInt32(model.CodigoEquipo.Trim());
                string sEstado = model.sEstadoCodi;
                string sNombre = model.NombreEquipo;
                var lsResultado = appEquipamiento.ListaEquipamientoPaginado(iEmpresa, iFamilia, iTipoEmpresa, iEquipo,
                    sEstado, sNombre, 1, Int32.MaxValue);
                foreach (var oEquipo in lsResultado)
                {
                    oEquipo.EstadoDesc = EquipamientoHelper.EstadoDescripcion(oEquipo.Equiestado);
                    oEquipo.Osigrupocodi = EquipamientoHelper.EstiloEstado(oEquipo.Equiestado);
                }

                string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteEquipamiento].ToString();
                string nombreReporteEquipos = NombreArchivo.ReporteEquipos;
                string plantilla = Constantes.PlantillaListadoEquipos;

                ExcelDocumentEquipamiento.GenerarListadoEquipos(ruta, nombreReporteEquipos, plantilla, lsResultado);
            }
            catch (Exception ex)
            {
                Log.Error("ExportarListaEquipamiento", ex);
                result = -1;
            }
            return Json(result);
        }

        [HttpGet]
        public virtual ActionResult DescargarListaEquipos()
        {
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteEquipamiento] + NombreArchivo.ReporteEquipos;
            return File(fullPath, Constantes.AppExcel, NombreArchivo.ReporteEquipos);
        }

        /// <summary>
        /// Reporte web - Auditoría de los cambios de los campos de un equipo. 
        /// </summary>
        /// <param name="equicodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarPropiedadAuditoria(int equicodi)
        {
            var model = new EquipamientoModel();

            try
            {
                model.EquipoSeleccionado = appEquipamiento.GetByIdEqEquipo(equicodi);
                model.ListaPropiedad = appEquipamiento.ListarPropiedadAuditoriaVigente(equicodi);
                model.Resultado = "1"; //1 es éxito
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1"; //-1 es error caso contrario éxito
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Reporte Excel - Auditoría de los cambios de los campos de un equipo. 
        /// </summary>
        /// <param name="equicodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarExcelPropiedadAuditoria(int equicodi)
        {
            var model = new EquipamientoModel();

            try
            {
                var equipoSeleccionado = appEquipamiento.GetByIdEqEquipo(equicodi);
                var listaPropiedad = appEquipamiento.ListarPropiedadAuditoriaVigente(equicodi);

                string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteEquipamiento].ToString();
                string fileName = NombreArchivo.ReporteAuditoriaCambio;
                ExcelDocumentEquipamiento.GenerarReporteAuditoriaCambio(equipoSeleccionado, listaPropiedad, ruta, fileName);

                model.Resultado = "1"; //1 es éxito
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1"; //-1 es error caso contrario éxito
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        [HttpGet]
        public virtual ActionResult DescargarExcelPropiedadAuditoria()
        {
            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteEquipamiento].ToString();
            string fileName = NombreArchivo.ReporteAuditoriaCambio;
            return DescargarArchivoTemporalYEliminarlo(ruta, fileName);
        }

        #endregion

        #region Copia completado de Propiedades de equipo

        public ViewResult IndexCopia()
        {
            ValidarSesionUsuario();
            var modelo = new IndexEquipoModel();
            modelo.ListaTipoEmpresa =
                appGeneral.ListarTiposEmpresa().Where(t => t.Tipoemprcodi > 0).OrderBy(t => t.Tipoemprdesc).ToList();
            modelo.ListaTipoEquipo =
                appEquipamiento.ListEqFamilias().Where(t => t.Famcodi > 0).OrderBy(t => t.Famnomb).ToList();
            modelo.ListaEstados = lsEstadosEquipo;
            modelo.iEmpresa = 0;
            modelo.iTipoEmpresa = 0;
            modelo.iTipoEquipo = 0;
            modelo.sEstadoCodi = "A";
            modelo.ListaEmpresa = new List<SiEmpresaDTO>();
            bool AccesoNuevo = Tools.VerificarAcceso(Session[DatosSesion.SesionIdOpcion], base.UserName,
                Acciones.Nuevo);
            bool AccesoEditar = Tools.VerificarAcceso(Session[DatosSesion.SesionIdOpcion], base.UserName,
                Acciones.Editar);
            modelo.EnableNuevo = AccesoNuevo ? "" : "disabled=disabled";
            modelo.EnableEditar = AccesoEditar ? "" : "disableClick";
            return View(modelo);
        }

        public PartialViewResult ListadoEquiposCopia(IndexEquipoModel model)
        {
            int iEmpresa = model.iEmpresa;
            int iFamilia = model.iTipoEquipo;
            int iTipoEmpresa = model.iTipoEmpresa;
            int iEquipo = string.IsNullOrEmpty(model.CodigoEquipo) ? -2 : Convert.ToInt32(model.CodigoEquipo.Trim());
            string sEstado = model.sEstadoCodi;

            var lsResultado = appEquipamiento.ListaEquipamientoPaginado(iEmpresa, iFamilia, iTipoEmpresa, iEquipo,
                sEstado, string.Empty, 1, int.MaxValue);
            foreach (var oEquipo in lsResultado)
            {
                oEquipo.EstadoDesc = EquipamientoHelper.EstadoDescripcion(oEquipo.Equiestado);
                oEquipo.Osigrupocodi = EquipamientoHelper.EstiloEstado(oEquipo.Equiestado);
            }
            model.ListadoEquipamiento = lsResultado;
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult CopiarPropiedades(int equipoOrigen, int equipoDestino)
        {
            ValidarSesionUsuario();
            int result = 1;
            try
            {
                appEquipamiento.CopiarPropiedades(equipoOrigen, equipoDestino, base.UserName);
            }
            catch (Exception ex)
            {
                Log.Error("CopiarPropiedades", ex);
                result = -1;
            }
            return Json(result);
        }

        #endregion

        #region Excel web de edición de Valores de Propiedades del Equipo

        /// <summary>
        /// Excel web para editar las propiedades del Equipo
        /// </summary>
        /// <param name="iEquipo"></param>
        /// <returns></returns>
        public ActionResult EquipoPropiedadesSheet(int iEquipo)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            var oEquipo = appEquipamiento.GetByIdEqEquipo(iEquipo);
            bool permiso = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            
            var modelo = new EquipoPropiedadesModel
            {
                Areanomb = string.IsNullOrEmpty(oEquipo.AREANOMB) ? "" : oEquipo.AREANOMB.Trim(),
                Emprnomb = string.IsNullOrEmpty(oEquipo.EMPRNOMB) ? "" : oEquipo.EMPRNOMB.Trim(),
                Equiabrev = string.IsNullOrEmpty(oEquipo.Equiabrev) ? "" : oEquipo.Equiabrev.Trim(),
                Equicodi = oEquipo.Equicodi,
                Equinomb = string.IsNullOrEmpty(oEquipo.Equinomb) ? "" : oEquipo.Equinomb.Trim(),
                Famnomb = string.IsNullOrEmpty(oEquipo.Famnomb) ? "" : oEquipo.Famnomb.Trim(),
                PropiedadNombre = "",
                EnableNuevo = permiso ? "" : "disabled=disabled",
                EnableEditar = permiso ? "" : "disableClick",
                AccesoNuevo = permiso,
                AccesoEditar = permiso,
                Fecha = DateTime.Now.Date.ToString(ConstantesAppServicio.FormatoFecha)
            };

            return View(modelo);
        }

        [HttpPost]
        public JsonResult PropiedadesHoja(string fecha, int iEquipo, string filtroFicha, bool habilitarEdicion)
        {
            EquipamientoModel model = new EquipamientoModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaConsulta = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                appEquipamiento.GetGridExcelWebPropiedadesEquipo(fechaConsulta, iEquipo, filtroFicha, habilitarEdicion, out HandsonModel handson, out List<EqPropequiDTO> listado, out List<int> listaDespuesFicha8);
                model.Handson = handson;
                model.ListaPropiedad = listado;
                model.ListaFlagVigenciaCorrecta = listaDespuesFicha8;
            }
            catch (Exception ex)
            {
                model.Resultado = "-1";
                model.Mensaje = ex.Message;

                model.Detalle = ex.StackTrace;
                Log.Error("PropiedadesHoja", ex);
            }

            return Json(model);
        }

        [HttpPost]
        public PartialViewResult HistoricoPropiedadEquipo(int iEquipo, int iPropiedad)
        {
            var oEquipo = appEquipamiento.GetByIdEqEquipo(iEquipo);
            var oPropiedad = appEquipamiento.GetByIdEqPropiedad(iPropiedad);
            var lsREsultado = appEquipamiento.ListarValoresHistoricosPropiedadPorEquipo(iEquipo, iPropiedad.ToString());
            foreach (var oHistorico in lsREsultado)
            {
                oHistorico.Valor = EquipamientoHelper.ConvertirEnLink(oHistorico.Valor);
            }
            var modelo = new HistoricoPropiedadModel
            {
                Equicodi = iEquipo,
                Propcodi = iPropiedad,
                Equinomb = oEquipo.Equinomb,
                PropNomb = oPropiedad.Propnomb,
                ListaValoresHistoricos = lsREsultado,
                MostrarColAdicional = !appEquipamiento.ListaPropiedadAuditoria().Contains(iPropiedad),
            };
            return PartialView(modelo);
        }

        [HttpPost]
        public JsonResult GrabarDatosPropiedades(string fecha, List<EqPropequiDTO> listaPropequi, int iEquipo, string filtroFicha, string orden)
        {
            EquipamientoModel model = new EquipamientoModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                DateTime fechaConsulta = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                appEquipamiento.ActualizarListaPropiedades(listaPropequi, fechaConsulta, iEquipo, filtroFicha, orden, base.UserName); 
            }
            catch (Exception ex)
            {
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                Log.Error("GrabarDatosPropiedades", ex);
            }

            return Json(model);
        }

        #endregion

        #region Carga masiva de nuevos equipos

        public ActionResult EquiposImportacion()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (base.IdOpcion == null) return base.RedirectToHomeDefault();

            IndexEquipoModel model = new IndexEquipoModel();

            return View(model);
        }

        /// <summary>
        /// ExportarEquipos
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarEquipos()
        {
            IndexEquipoModel model = new IndexEquipoModel();

            try
            {
                base.ValidarSesionJsonResult();

                string fileName = ConstantesEquipamientoAppServicio.NombrePlantillaExcelNuevosEquipos;
                string pathOrigen = ConstantesFichaTecnica.FolderRaizFichaTecnica + ConstantesFichaTecnica.Plantilla;
                string pathDestino = AppDomain.CurrentDomain.BaseDirectory + ConstantesEquipamientoAppServicio.RutaReportes;

                FileServer.CopiarFileAlterFinalOrigen(pathOrigen, pathDestino, fileName, null);

                appEquipamiento.GenerarExcelPlantilla(pathDestino, fileName);

                model.Resultado = "1";
                model.NombreArchivo = fileName;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Permite descargar el archivo Excel
        /// </summary>
        /// <param name="file">Nombre del archivo</param>
        /// <returns>Archivo</returns>
        public virtual ActionResult AbrirArchivo(string file)
        {
            return DescargarArchivoTemporalYEliminarlo(AppDomain.CurrentDomain.BaseDirectory + ConstantesEquipamientoAppServicio.RutaReportes, file);
        }

        /// <summary>
        /// UploadEquipo
        /// </summary>
        /// <param name="sFecha"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadEquipo(string sFecha)
        {
            try
            {
                base.ValidarSesionUsuario();
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesEquipamientoAppServicio.RutaReportes;

                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string sNombreArchivo = sFecha + "_" + file.FileName;

                    if (FileServer.VerificarExistenciaFile(null, sNombreArchivo, path))
                    {
                        //FileServer.DeleteBlob(sNombreArchivo, path + ConstantesEquipamientoAppServicio.Reportes);
                    }
                    file.SaveAs(path + sNombreArchivo);
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
        /// MostrarArchivoImportacion
        /// </summary>
        /// <param name="sFecha"></param>
        /// <param name="sFileName"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MostrarArchivoImportacion(string sFecha, string sFileName)
        {
            base.ValidarSesionUsuario();

            IndexEquipoModel model = new IndexEquipoModel();

            string fileName = sFecha + "_" + sFileName;
            model.ListaDocumentos = FileServer.ListarArhivos(null, AppDomain.CurrentDomain.BaseDirectory + ConstantesEquipamientoAppServicio.RutaReportes);
            var documento = new FileData();

            foreach (var item in model.ListaDocumentos)
            {
                if (String.Equals(item.FileName, fileName))
                {
                    model.Documento = new FileData();
                    model.Documento = item;
                    break;
                }
            }

            return Json(model);
        }

        /// <summary>
        /// ImportarEquiposExcel
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ImportarEquiposExcel(string fileName)
        {
            IndexEquipoModel model = new IndexEquipoModel();
            model.ListaEquiposCorrectos = new List<EqEquipoDTO>();
            model.ListaEquiposErrores = new List<EqEquipoDTO>();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                // Ruta de los archivos EXCEL leidos
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesEquipamientoAppServicio.RutaReportes;

                // Validar datos de Excel y realiza la importacion de los registros de este archivo           
                appEquipamiento.ValidarEquiposAImportarXLSX(path, fileName, base.UserName,
                                                       out List<EqEquipoDTO> lstRegEquiposCorrectos,
                                                       out List<EqEquipoDTO> lstRegEquiposErroneos,
                                                       out List<EqEquipoDTO> listaNuevo);

                model.ListaEquiposCorrectos = lstRegEquiposCorrectos;
                model.ListaEquiposErrores = lstRegEquiposErroneos;

                //validación si existen errores
                if (lstRegEquiposErroneos.Any())
                {
                    string filenameCSV = appEquipamiento.GenerarArchivoEquiposErroneosCSV(path, lstRegEquiposErroneos);
                    model.FileName = filenameCSV;

                    throw new Exception("¡No se guardó la información! Existen datos o registros que no permiten cargar el archivo completo.");
                }

                //validación si no tiene datos
                if (lstRegEquiposCorrectos.Count() == 0)
                {
                    throw new Exception("Por favor ingrese un documento con registros nuevos y/o actualizados.");
                }

                //Ejecución de la grabación de conceptos de un archivo Excel
                appEquipamiento.GuardarDatosMasivosEquipos(listaNuevo, base.UserName);

                model.StrMensaje = "¡La Información se grabó correctamente!";
            }
            catch (Exception ex)
            {
                model.StrMensaje = ex.Message;
                model.Resultado = "-1";
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }

            var json = Json(model);
            json.MaxJsonLength = Int32.MaxValue;

            return json;
        }

        /// <summary>
        /// AbrirArchivoCSV
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public virtual ActionResult AbrirArchivoCSV(string file)
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesEquipamientoAppServicio.RutaReportes + file;

            string app = ConstantesEquipamientoAppServicio.AppCSV;

            // lo guarda el CSV en la carpeta de descarga
            return File(path, app, file);
        }

        /// <summary>
        /// EliminarArchivosImportacionNuevo
        /// </summary>
        /// <param name="nombreArchivo"></param>
        /// <returns></returns>
        [HttpPost]
        public int EliminarArchivosImportacionNuevo(string nombreArchivo)
        {
            base.ValidarSesionUsuario();

            string nombreFile = string.Empty;

            IndexEquipoModel modelArchivos = new IndexEquipoModel();
            modelArchivos.ListaDocumentos = FileServer.ListarArhivos(null, AppDomain.CurrentDomain.BaseDirectory + ConstantesEquipamientoAppServicio.RutaReportes);
            foreach (var item in modelArchivos.ListaDocumentos)
            {
                string subString = item.FileName;
                if (subString == nombreArchivo)
                {
                    nombreFile = item.FileName;
                    break;
                }
            }

            if (FileServer.VerificarExistenciaFile(null, nombreFile, AppDomain.CurrentDomain.BaseDirectory + ConstantesEquipamientoAppServicio.RutaReportes))
            {
                FileServer.DeleteBlob(nombreFile, AppDomain.CurrentDomain.BaseDirectory + ConstantesEquipamientoAppServicio.RutaReportes);
            }

            return -1;
        }

        #endregion

        #region Relación Equipo y Proyecto EO

        /// <summary>
        /// Devuelve la informacion de relaciones de proyectos y empresas
        /// </summary>
        /// <param name="equicodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerDatosAsignacion(int equicodi)
        {
            IndexEquipoModel model = new IndexEquipoModel();

            try
            {
                string usuario = base.UserName;
                //usuario = "JPEREZ2";
                base.ValidarSesionJsonResult();

                model.Equipo = appEquipamiento.GetByIdEqEquipo(equicodi);
                model.ListaProyectosEquipo = appEquipamiento.ObtenerListadoProyectosPorEquipo(equicodi);
                model.ListadoEmpresasCopropietarias = appEquipamiento.ObtenerListadoEmpresasCopropietarias(equicodi.ToString());

                FichaTecnicaAppServicio servFT = new FichaTecnicaAppServicio();
                model.ListaEmpresas = servFT.ListarEmpresasActivas();
                
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Devuelve todos los proyectos activos
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerProyectosExistentes()
        {
            FTProyectoModel model = new FTProyectoModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                model.ListadoProyectos = appEquipamiento.ListarProyectosExistentes();
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
        /// Obtiene los datos del proyecto seleccionado
        /// </summary>
        /// <param name="ftprycodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerDatosProySel(int ftprycodi)
        {
            FTProyectoModel model = new FTProyectoModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                model.Proyecto = appEquipamiento.ObtenerDatosProyecto(ftprycodi);
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
        /// Devuelve los datos de una relacion Equipo-Proyecto
        /// </summary>
        /// <param name="ftreqpcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerDatosPEAuditoria(int ftreqpcodi)
        {
            FTProyectoModel model = new FTProyectoModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                model.RelEquipoProyecto = appEquipamiento.ObtenerDatosRelEquipoProyecto(ftreqpcodi);
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
        /// Devuelve los datos de una relacion Empresas Lineas Transmision
        /// </summary>
        /// <param name="ftreqpcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerDatosAuditoriaEmpCo(int ftreqecodi)
        {
            FTProyectoModel model = new FTProyectoModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                model.RelLTEmpresa = appEquipamiento.ObtenerDatosRelEmpresaLT(ftreqecodi);
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
        /// Obtiene los datos de la empresa seleccionado
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerDatosEmpresaSel(int emprcodi)
        {
            FTProyectoModel model = new FTProyectoModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                model.Empresa = appEquipamiento.ObtenerDatosEmpresa(emprcodi);
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
        /// Guarda los datos de la asignacion
        /// </summary>
        /// <param name="equicodi"></param>
        /// <param name="strCambiosPE"></param>
        /// <param name="strCambiosELT"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarInfoAsignacion(int equicodi, string strCambiosPE, string strCambiosELT)
        {
            FTProyectoModel model = new FTProyectoModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                appEquipamiento.GuardarDatosAsignacionFT(equicodi, strCambiosPE, strCambiosELT, base.UserName);
                appEquipamiento.ActualizarCambiosEnAsignacionDeProyectos(strCambiosPE, base.UserName);
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

        public ActionResult ImportacionRelEquipoProyecto()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (base.IdOpcion == null) return base.RedirectToHomeDefault();

            IndexEquipoModel model = new IndexEquipoModel();

            return View(model);
        }

        [HttpPost]
        public JsonResult ImportarRelEquipoProyectoExcel(string fileName)
        {
            IndexEquipoModel model = new IndexEquipoModel();
            model.ListaReleqpryCorrectos = new List<FtExtReleqpryDTO>();
            model.ListaReleqpryErrores = new List<FtExtReleqpryDTO>();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                // Ruta de los archivos EXCEL leidos
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesEquipamientoAppServicio.RutaReportes;

                // Validar datos de Excel y realiza la importacion de los registros de este archivo           
                appEquipamiento.ValidarRelEquipoProyectoAImportarXLSX(path, fileName, base.UserName,
                                                       out List<FtExtReleqpryDTO> lstRegEquiposCorrectos,
                                                       out List<FtExtReleqpryDTO> lstRegEquiposErroneos,
                                                       out List<FtExtReleqpryDTO> listaNuevo);

                model.ListaReleqpryCorrectos = lstRegEquiposCorrectos;
                model.ListaReleqpryErrores = lstRegEquiposErroneos;

                //validación si existen errores
                if (lstRegEquiposErroneos.Any())
                {
                    string filenameCSV = appEquipamiento.GenerarArchivoRelEquipoProyectoErroneosCSV(path, lstRegEquiposErroneos);
                    model.FileName = filenameCSV;

                    throw new Exception("¡No se guardó la información! Existen datos o registros que no permiten cargar el archivo completo.");
                }

                //validación si no tiene datos
                if (lstRegEquiposCorrectos.Count() == 0)
                {
                    throw new Exception("Por favor ingrese un documento con registros nuevos y/o actualizados.");
                }

                //Ejecución de la grabación de conceptos de un archivo Excel
                appEquipamiento.GuardarDatosMasivosRelEquipoProyecto(listaNuevo, base.UserName);

                model.StrMensaje = "¡La Información se grabó correctamente!";
            }
            catch (Exception ex)
            {
                model.StrMensaje = ex.Message;
                model.Resultado = "-1";
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }

            var json = Json(model);
            json.MaxJsonLength = Int32.MaxValue;

            return json;
        }

        [HttpPost]
        public JsonResult ExportarPlantillaRelEquipoProyecto()
        {
            IndexEquipoModel model = new IndexEquipoModel();

            try
            {
                base.ValidarSesionJsonResult();

                string fileName = ConstantesEquipamientoAppServicio.NombrePlantillaExcelRelPryEq;
                string pathOrigen = ConstantesFichaTecnica.FolderRaizFichaTecnica + ConstantesFichaTecnica.Plantilla;
                string pathDestino = AppDomain.CurrentDomain.BaseDirectory + ConstantesEquipamientoAppServicio.RutaReportes;

                FileServer.CopiarFileAlterFinalOrigen(pathOrigen, pathDestino, fileName, null);

                appEquipamiento.GenerarExcelPlantillaRelEquipoProyecto(pathDestino, fileName);

                model.Resultado = "1";
                model.NombreArchivo = fileName;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        #endregion
    }
}
