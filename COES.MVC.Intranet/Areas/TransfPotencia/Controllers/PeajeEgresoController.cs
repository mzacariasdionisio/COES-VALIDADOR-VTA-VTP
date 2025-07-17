using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.TransfPotencia.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.Transferencias;
using COES.Servicios.Aplicacion.TransfPotencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.TransfPotencia.Controllers
{
    public class PeajeEgresoController : BaseController
    {

        PeriodoAppServicio servicioPeriodo = new PeriodoAppServicio();
        TransfPotenciaAppServicio servicioTransfPotencia = new TransfPotenciaAppServicio();
        // GET: /TransfPotencia/PeajeEgreso/

        public ActionResult Index(int pericodi = 0, int recpotcodi = 0)
        {
            base.ValidarSesionUsuario();
            PeajeEgresoModel model = new PeajeEgresoModel();

            model.ListaPeriodos = this.servicioPeriodo.ListPeriodo();
            if (model.ListaPeriodos.Count > 0 && pericodi == 0)
            { pericodi = model.ListaPeriodos[0].PeriCodi; }
            if (pericodi > 0)
            {
                model.ListaRecalculoPotencia = this.servicioTransfPotencia.ListByPericodiVtpRecalculoPotencia(pericodi); //Ordenado en descendente
                if (model.ListaRecalculoPotencia.Count > 0 && recpotcodi == 0)
                { recpotcodi = (int)model.ListaRecalculoPotencia[0].Recpotcodi; }
            }

            if (pericodi > 0 && recpotcodi > 0)
            {
                model.EntidadRecalculoPotencia = this.servicioTransfPotencia.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);
            }
            else
            {
                model.EntidadRecalculoPotencia = new VtpRecalculoPotenciaDTO();
            }

            model.Pericodi = pericodi;
            model.Recpotcodi = recpotcodi;

            return View(model);
        }

        [HttpPost]
        public PartialViewResult Listado(int pericodi, int recpotcodi)
        {
            PeajeEgresoModel model = new PeajeEgresoModel();
            List<VtpPeajeEgresoDTO> list = servicioTransfPotencia.ObtenerReporteEnvioPorEmpresa(pericodi, recpotcodi);
            model.ListaPeajeEgreso = list;
            return PartialView(model);
        }
    }
}
