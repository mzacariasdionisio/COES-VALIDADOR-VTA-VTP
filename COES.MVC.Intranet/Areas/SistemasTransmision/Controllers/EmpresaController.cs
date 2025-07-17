using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.SistemasTransmision.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.SistemasTransmision;
using COES.Servicios.Aplicacion.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.SistemasTransmision.Controllers
{
    public class EmpresaController : BaseController
    {
        //
        // GET: /SistemasTransmision/Empresa/

        EmpresaAppServicio servicioEmpresa = new EmpresaAppServicio();
        SistemasTransmisionAppServicio servicioSistemasTransmision = new SistemasTransmisionAppServicio();

        public ActionResult Index(int stpercodi = 0, int strecacodi = 0)
        {
            EmpresaModel model = new EmpresaModel();
            model.ListaPerido = this.servicioSistemasTransmision.ListStPeriodos();
            if (model.ListaPerido.Count > 0 && stpercodi == 0)
            { stpercodi = model.ListaPerido[0].Stpercodi; }
            if (stpercodi > 0)
            {
                model.ListaStRecalculo = this.servicioSistemasTransmision.ListByPericodiRecalculo(stpercodi);
                if (model.ListaStRecalculo.Count > 0 && strecacodi == 0)
                { strecacodi = (int)model.ListaStRecalculo[0].Strecacodi; }
            }

            if (stpercodi > 0 && strecacodi > 0)
            {
                model.EntidadStRecalculo = this.servicioSistemasTransmision.GetByIdStRecalculoView(stpercodi, strecacodi);
            }
            model.StPercodi = stpercodi;
            model.StRecacodi = strecacodi;
            model.bNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);

            return PartialView(model);
        }
        
    }
}