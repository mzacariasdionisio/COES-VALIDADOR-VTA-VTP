using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Hidrologia.Helper;
using COES.MVC.Intranet.Areas.Hidrologia.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Hidrologia;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Servicios.Aplicacion.DemandaMaxima;
using log4net;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using COES.Servicios.Aplicacion.Despacho;
using COES.Servicios.Aplicacion.Transferencias;
using COES.Servicios.Aplicacion.Titularidad;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.General;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.Equipamiento;

namespace COES.MVC.Intranet.Areas.Hidrologia.Controllers
{
    public class PtoDespachoController : BaseController
    {
        private GeneralAppServicio appGeneral = new GeneralAppServicio();

        GrupoDespachoAppServicio servicio = new GrupoDespachoAppServicio();
        DespachoAppServicio servicioDesp = new DespachoAppServicio();

        //private List<EstadoModel> lsEstados = new List<EstadoModel>();
        //private DespachoAppServicio appDespacho = new DespachoAppServicio();

        //public PtoDespachoController()
        //{
        //    lsEstados.Add(new EstadoModel { EstadoCodigo = "S", EstadoDescripcion = "Activo" });
        //    lsEstados.Add(new EstadoModel { EstadoCodigo = "N", EstadoDescripcion = "Inactivo" });
        //}
        public ActionResult Index()
        {
            PtoDespachoModel model = new PtoDespachoModel();
            model.ListaEmpresas = this.servicio.ObtenerListaEmpresas();
            return View(model);
        }

        [HttpPost]
        public PartialViewResult Lista(string empresas, int nroPagina)
        {
            PtoDespachoModel model = new PtoDespachoModel();
            model.ListaGrupo = this.servicio.ListGruposDespachoPorCentral(empresas, nroPagina, Constantes.PageSize);
           
            return PartialView(model);
        }

        [HttpPost]
        public PartialViewResult Paginado(string empresas)
        {
            PtoDespachoModel model = new PtoDespachoModel();
            model.IndicadorPagina = false;
            int nroRegistros= servicio.ListGruposDespachoPorCentralXFiltro(empresas);

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


        public PartialViewResult Editar()
        {
            string codigo = "0";
            if (Request["id"] != null)
                codigo = Request["id"];
            int id = int.Parse(codigo);

            PtoDespachoModel model = new PtoDespachoModel();

            if(id > 0)
            {
                model.PrGrupo = servicio.GetByIdPrGrupo(id);
            }
            
            if (model.PrGrupo == null)
            {
                model.PrGrupo = new PrGrupoDTO();
                model.PrGrupo.Emprcodi = -1;
                model.PrGrupo.Catecodi = -1;
                model.PrGrupo.Grupopadre = -1;
                model.PrGrupo.Fenergcodi = -1;
                model.PrGrupo.Tipogrupocodi = -1;
                model.PrGrupo.Gruponomb = string.Empty;
                model.PrGrupo.Grupoabrev = string.Empty;
            }               

            model.ListarEmpresas = servicio.ListarEmpresas().Where(x => x.EMPRCODI > 0).ToList();
            model.ListarFEnergia = servicio.ListFuente();
            model.ListarTipoGrupo = servicio.ListaTipoGrupo();
            model.ListaGrupo = null;
            return PartialView(model);
        }


        public JsonResult cargasCentralByEmpCat(int empresa, int categoria)
        {
            if (categoria == 3) categoria = 4;
            if (categoria == 5) categoria = 6;
            if (categoria == 15) categoria = 16;
            if (categoria == 17) categoria = 18;
            List<PrGrupoDTO> centrales = this.servicio.ListGruposDespachoPorCentral(empresa.ToString(), 1, 200).Where(x => x.Catecodi == categoria).ToList();
            SelectList list = new SelectList(centrales, "grupocodi", "gruponomb");
            return Json(list);
        }

        [HttpPost]
        public JsonResult grabarPtoDespacho(int empresa, int codigrupo, string descripcion, string abrev, string grupotipo, int categoria, int grupopadre, string grupoactivo,
            string osicodi, int tipogrupo, int tipofuente, string tipogenerrer, string grupointegrante, string estado)
        {
            this.ValidarSesionJsonResult();

            
            PrGrupoDTO grupo = new PrGrupoDTO();
            int resultado = 1;
            int grupocodi = codigrupo;
            int accion = Constantes.AccionEditar;

            if (grupocodi == 0) accion = Constantes.AccionNuevo;
            else  grupo = servicio.GetByIdPrGrupo(grupocodi);

            grupo.Grupocodi = grupocodi;
            grupo.Lastuser = User.Identity.Name;
            grupo.Lastdate = DateTime.Now;
            grupo.Emprcodi = empresa;
            grupo.Gruponomb = descripcion;
            grupo.Grupoabrev = abrev;
            grupo.Grupotipo = grupotipo;
            grupo.Catecodi = categoria;
            grupo.Grupopadre = grupopadre;
            grupo.Grupoactivo = grupoactivo;
            grupo.Osinergcodi = osicodi;
            grupo.Osicodi = osicodi;
            grupo.Tipogrupocodi = tipogrupo;
            grupo.Fenergcodi = tipofuente;
            grupo.TipoGenerRer = tipogenerrer;
            grupo.Grupointegrante = grupointegrante;
            grupo.GrupoEstado = estado;
            grupo.Grupotipoc = (grupopadre > 0 ? 1 : 0);
            grupo.Barracodi = -1;
            grupo.Barracodi2 = -1;

            grupo.Grupovmax = null;
            grupo.Grupovmin = null;
            grupo.Grupoorden = null;

            grupo.Grupocodi2 = null;
            grupo.Grupotension = null;

            grupo.Grupotipocogen = "N";

            try
            {
                if (accion == Constantes.AccionNuevo)
                {
                    grupo.Grupousucreacion = grupo.Lastuser;
                    grupo.Grupofeccreacion = grupo.Lastdate;
                    grupo.Grupousumodificacion = grupo.Lastuser;
                    grupo.Grupofecmodificacion = grupo.Lastdate;
                    grupo.Grupocodi = servicioDesp.SavePrGrupo(grupo);
                    Session["NvoPtoDespacho"] = grupo.Grupocodi;
                    resultado = 1;
                }
                else
                {
                    grupo.Grupousumodificacion = grupo.Lastuser;
                    grupo.Grupofecmodificacion = grupo.Lastdate;
                    servicioDesp.UpdatePrGrupo(grupo);
                    resultado = 1;
                }
            }
            catch (Exception ex)
            {
                resultado = 0;
            }

            return Json(resultado);
            
        }

        /// <summary>
        /// Permite eliminar un punto de medición
        /// </summary>
        /// <param name="ptoDespacho"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeletePuntoDespacho(int ptoDespacho)
        {
            try
            {
                this.ValidarSesionJsonResult();

                this.servicioDesp.DeletePrGrupo(ptoDespacho, User.Identity.Name);
                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }

    }
}
