using COES.Dominio.DTO.Sic;
using COES.MVC.Extranet.Areas.Eventos.Models;
using COES.MVC.Extranet.Helper;
using COES.Servicios.Aplicacion.Evento;
using COES.Servicios.Aplicacion.Eventos.Helper;
using COES.Servicios.Aplicacion.Informe;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.Eventos.Controllers
{
    public class EventoController : Controller
    {
        /// <summary>
        /// Instancia de la clase servicio de eventos
        /// </summary>
        EventosAppServicio servicio = new EventosAppServicio();


        /// <summary>
        /// Almacena las empresas del usario
        /// </summary>
        public List<int> ListaEmpresa
        {
            get
            {
                return (Session[DatosSesion.ListaEmpresa] != null) ?
                    (List<int>)Session[DatosSesion.ListaEmpresa] : new List<int>();
            }
            set { Session[DatosSesion.ListaEmpresa] = value; }
        }
       
        /// <summary>
        /// Pagina inicial del modulo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            EventoModel model = new EventoModel();
            model.FechaInicio = DateTime.Now.AddDays(-7).ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.ListaTipoEvento = this.servicio.ListarTipoEvento().Where(x => x.Tipoevencodi ==
                ConstantesEvento.TipoEventoEvento || x.Tipoevencodi ==
                ConstantesEvento.TipoEventoFalla).ToList();
            model.ListaEmpresa = (new SeguridadServicio.SeguridadServicioClient()).ObtenerEmpresasPorUsuario(User.Identity.Name).ToList();
            model.IndicadorEmpresa = -1;
            if (model.ListaEmpresa.Count > 0)
            {
                model.IndicadorEmpresa = 1;
                if (model.ListaEmpresa.Count == 1) {
                    model.IndicadorEmpresa = 2;
                    model.IdEmpresa = model.ListaEmpresa[0].EMPRCODI;
                }
            }

            List<int> idsEmpresas = model.ListaEmpresa.Select(x => (int)x.EMPRCODI).Distinct().ToList();
            this.ListaEmpresa = idsEmpresas;

            return View(model);
        }

        /// <summary>
        /// Permite mostrar el paginado de la consulta
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(string fechaInicio, string fechaFin, int? idTipoEvento)
        {
            PaginadoModel model = new PaginadoModel();
            model.IndicadorPagina = false;

            DateTime fecInicio = DateTime.Now;
            DateTime fecFin = DateTime.Now;

            if (!string.IsNullOrEmpty(fechaInicio))
            {
                fecInicio = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (!string.IsNullOrWhiteSpace(fechaFin))
            {
                fecFin = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            fecFin = fecFin.AddDays(1);
            if (idTipoEvento == null) idTipoEvento = 0;
            int nroRegistros = this.servicio.ObtenerNroRegistrosConsultaExtranet(fecInicio, fecFin, idTipoEvento);

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
        /// Permite pintar la lista de registros
        /// </summary>
        /// <returns></returns>
        public ActionResult Listado(string fechaInicio, string fechaFin, int? idTipoEvento, int nroPagina)
        {
            EventoModel model = new EventoModel();

            DateTime fecInicio = DateTime.Now;
            DateTime fecFin = DateTime.Now;

            if (!string.IsNullOrEmpty(fechaInicio))
            {
                fecInicio = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (!string.IsNullOrWhiteSpace(fechaFin))
            {
                fecFin = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            fecFin = fecFin.AddDays(1);
            if (idTipoEvento == null) idTipoEvento = 0;

            model.ListaEvento = this.servicio.ConsultaEventoExtranet(fecInicio, fecFin, idTipoEvento, nroPagina, 
                Constantes.PageSize);

            return View(model);
        }

        /// <summary>
        /// Permite mostrar los estados de los informes cargados
        /// </summary>
        /// <param name="idEvento"></param>
        /// <param name="indicador"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Informe(int idEvento, string indicador)
        {
            DatoInformeModel model = new DatoInformeModel();
            List<EventoInformeReporte> reporte = (new InformeAppServicio()).ObtenerInformeResumen(idEvento, this.ListaEmpresa, indicador);
            model.Reporte = reporte.Where(x => x.Emprcodi != -1 && x.Emprcodi != 0).ToList();
            model.ReporteSCO = reporte.Where(x => x.Emprcodi == -1).ToList();
            model.Indicador = indicador; 

            return PartialView(model);
        }
       
    }
}
