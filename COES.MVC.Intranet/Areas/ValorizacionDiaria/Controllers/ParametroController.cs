using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Despacho;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.ValorizacionDiaria;
using COES.Servicios.Aplicacion.ValorizacionDiaria.Helper;
using COES.MVC.Intranet.Areas.ValorizacionDiaria.Models;
using log4net;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Globalization;

namespace COES.MVC.Intranet.Areas.ValorizacionDiaria.Controllers
{
    public class ParametroController : BaseController
    {
        //
        // GET: /Valorizacion/Parametro/
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(ParametroController));

        private DespachoAppServicio appDespacho = new DespachoAppServicio();
        private ContactoAppServicio appContacto = new ContactoAppServicio();
        private ValorizacionDiariaAppServicio appValorizacion = new ValorizacionDiariaAppServicio();

        public ActionResult Index()
        {
            var Fechadat = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            ParametroModel model = new ParametroModel
            {
                MargenReserva = appDespacho.ListarValoresHistoricosDespacho(ConstantesValorizacionDiaria.ConceptoCodiMargenReserva, ConstantesValorizacionDiaria.GrupoCodiParametro),
                CargoPorConsumo = appDespacho.ListarValoresHistoricosDespacho(ConstantesValorizacionDiaria.ConceptoCodiCargosConsumo, ConstantesValorizacionDiaria.GrupoCodiParametro),
                PorcentajePerdida = appDespacho.ListarValoresHistoricosDespacho(ConstantesValorizacionDiaria.ConceptoCodiPorcentajePerdida, ConstantesValorizacionDiaria.GrupoCodiParametro),
                CostoOportunidad = appDespacho.ListarValoresHistoricosDespacho(ConstantesValorizacionDiaria.ConceptoCodiCostoOportunidad, ConstantesValorizacionDiaria.GrupoCodiParametro),
                FactorRepartoLunes = appDespacho.ListarValoresHistoricosDespacho(ConstantesValorizacionDiaria.ConceptoCodiFactorRepartoLunes, ConstantesValorizacionDiaria.GrupoCodiParametro),
                FactorRepartoMartes = appDespacho.ListarValoresHistoricosDespacho(ConstantesValorizacionDiaria.ConceptoCodiFactorRepartoMartes, ConstantesValorizacionDiaria.GrupoCodiParametro),
                FactorRepartoMiercoles = appDespacho.ListarValoresHistoricosDespacho(ConstantesValorizacionDiaria.ConceptoCodiFactorRepartoMiercoles, ConstantesValorizacionDiaria.GrupoCodiParametro),
                FactorRepartoJueves = appDespacho.ListarValoresHistoricosDespacho(ConstantesValorizacionDiaria.ConceptoCodiFactorRepartoJueves, ConstantesValorizacionDiaria.GrupoCodiParametro),
                FactorRepartoViernes = appDespacho.ListarValoresHistoricosDespacho(ConstantesValorizacionDiaria.ConceptoCodiFactorRepartoViernes, ConstantesValorizacionDiaria.GrupoCodiParametro),
                FactorRepartoSabado = appDespacho.ListarValoresHistoricosDespacho(ConstantesValorizacionDiaria.ConceptoCodiFactorRepartoSabado, ConstantesValorizacionDiaria.GrupoCodiParametro),
                FactorRepartoDomingo = appDespacho.ListarValoresHistoricosDespacho(ConstantesValorizacionDiaria.ConceptoCodiFactorRepartoDomingo, ConstantesValorizacionDiaria.GrupoCodiParametro),
                CostoFueraBanda = appDespacho.ListarValoresHistoricosDespacho(ConstantesValorizacionDiaria.ConceptoCodiCostoFueraBanda, ConstantesValorizacionDiaria.GrupoCodiParametro),
                CostosOtrosEquipos = appDespacho.ListarValoresHistoricosDespacho(ConstantesValorizacionDiaria.ConceptoCodiCostoOtrosEquipos, ConstantesValorizacionDiaria.GrupoCodiParametro),
                FRECTotal = appDespacho.ListarValoresHistoricosDespacho(ConstantesValorizacionDiaria.ConceptoCodiFRECTotal, ConstantesValorizacionDiaria.GrupoCodiParametro),
                Empresas = (new ValorizacionDiariaAppServicio()).ObtenerEmpresasMME(),// appContacto.ObtenerEmpresas(3),
                HoraEjecucion = (new ValorizacionDiariaAppServicio()).ObtenerHoraEjecucion()

            };

            #region validaciones
            if (model.MargenReserva.Count() == 0)
            {
                ViewBag.MargenReserva = 0.00;
            }
            else
            {
                ViewBag.MargenReserva = model.MargenReserva[0].Formuladat;
            }

            if (model.FactorRepartoLunes.Count() == 0)
            {
                ViewBag.FactorRepartoLunes = 0.0000;
            }
            else
            {
                ViewBag.FactorRepartoLunes = model.FactorRepartoLunes[0].Formuladat;
            }

            if (model.FactorRepartoMartes.Count() == 0)
            {
                ViewBag.FactorRepartoMartes = 0.0000;
            }
            else
            {
                ViewBag.FactorRepartoMartes = model.FactorRepartoMartes[0].Formuladat;
            }

            if (model.FactorRepartoMiercoles.Count() == 0)
            {
                ViewBag.FactorRepartoMiercoles = 0.0000;
            }
            else
            {
                ViewBag.FactorRepartoMiercoles = model.FactorRepartoMiercoles[0].Formuladat;
            }

            if (model.FactorRepartoJueves.Count() == 0)
            {
                ViewBag.FactorRepartoJueves = 0.0000;
            }
            else
            {
                ViewBag.FactorRepartoJueves = model.FactorRepartoJueves[0].Formuladat;
            }

            if (model.FactorRepartoViernes.Count() == 0)
            {
                ViewBag.FactorRepartoViernes = 0.0000;
            }
            else
            {
                ViewBag.FactorRepartoViernes = model.FactorRepartoViernes[0].Formuladat;
            }

            if (model.FactorRepartoSabado.Count() == 0)
            {
                ViewBag.FactorRepartoSabado = 0.0000;
            }
            else
            {
                ViewBag.FactorRepartoSabado = model.FactorRepartoSabado[0].Formuladat;
            }

            if (model.FactorRepartoDomingo.Count() == 0)
            {
                ViewBag.FactorRepartoDomingo = 0.0000;
            }
            else
            {
                ViewBag.FactorRepartoDomingo = model.FactorRepartoDomingo[0].Formuladat;
            }

            if (model.PorcentajePerdida.Count() == 0)
            {
                ViewBag.PorcentajePerdida = 0;
            }
            else
            {
                ViewBag.PorcentajePerdida = model.PorcentajePerdida[0].Formuladat;
            }

            if (model.CostoOportunidad.Count() == 0)
            {
                ViewBag.CostoOportunidad = 0;
            }
            else
            {
                foreach (PrGrupodatDTO itemCostoOportunidad in model.CostoOportunidad)
                {
                    if(itemCostoOportunidad.Deleted == 0)
                    {
                        ViewBag.CostoOportunidad = itemCostoOportunidad.Formuladat;
                        break;
                    }
                }
            }

            if (model.CargoPorConsumo.Count() == 0)
            {
                ViewBag.CargoPorConsumo = 0;
            }
            else
            {
                ViewBag.CargoPorConsumo = model.CargoPorConsumo[0].Formuladat;
            }

            if (model.CostoFueraBanda.Count() == 0)
            {
                ViewBag.CostoFueraBanda = 0.00;
            }
            else
            {
                ViewBag.CostoFueraBanda = model.CostoFueraBanda[0].Formuladat;
            }

            if (model.CostosOtrosEquipos.Count() == 0)
            {
                ViewBag.CostosOtrosEquipos = 0.00;
            }
            else
            {
                ViewBag.CostosOtrosEquipos = model.CostosOtrosEquipos[0].Formuladat;
            }

            if (model.FRECTotal.Count() == 0)
            {
                ViewBag.FRECTotal = 0.00;
            }
            else
            {
                ViewBag.FRECTotal = model.FRECTotal[0].Formuladat;
            }
            #endregion
            //string anio = DateTime.Now.ToString("yyyy");
            //string mes = string.Empty;
            //switch ((DateTime.Now.ToString("MM")))
            //{
            //    case "01":
            //        mes = "Ene";
            //        break;
            //    case "02":
            //        mes = "Feb";
            //        break;
            //    case "03":
            //        mes = "Mar";
            //        break;
            //    case "04":
            //        mes = "Abr";
            //        break;
            //    case "05":
            //        mes = "May";
            //        break;
            //    case "06":
            //        mes = "Jun";
            //        break;
            //    case "07":
            //        mes = "Jul";
            //        break;
            //    case "08":
            //        mes = "Ago";
            //        break;
            //    case "09":
            //        mes = "Set";
            //        break;
            //    case "10":
            //        mes = "Oct";
            //        break;
            //    case "11":
            //        mes = "Nov";
            //        break;
            //    case "12":
            //        mes = "Dic";
            //        break;
            //}
            ViewBag.FechaFormatoMesAnio = DateTime.Now.ToString("MM yyyy");//mes + " " + anio;
            return View(model);
        }

        [HttpPost]
        public JsonResult GuardarMargenReserva(string ValorMargenReserva)
        {
            try
            {
                guardarGrupoDat(ValorMargenReserva, ConstantesValorizacionDiaria.GrupoCodiParametro, ConstantesValorizacionDiaria.ConceptoCodiMargenReserva);

                return Json(1);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Json(-1);
            }
        }

        [HttpPost]
        public JsonResult GuardarCargoConsumo(string ValorCargoConsumo)
        {
            try
            {
                guardarGrupoDat(ValorCargoConsumo, ConstantesValorizacionDiaria.GrupoCodiParametro, ConstantesValorizacionDiaria.ConceptoCodiCargosConsumo);

                return Json(1);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Json(-1);
            }
        }

        [HttpPost]
        public JsonResult GuardarPorcentajePerdida(string ValorPorcentajePerdida)
        {
            try
            {
                guardarGrupoDat(ValorPorcentajePerdida, ConstantesValorizacionDiaria.GrupoCodiParametro, ConstantesValorizacionDiaria.ConceptoCodiPorcentajePerdida);

                return Json(1);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Json(-1);
            }
        }

        [HttpPost]
        public JsonResult GuardarFactorReparto(string ValorLunes, string ValorMartes, string ValorMiercoles, string ValorJueves, string ValorViernes, string ValorSabado, string ValorDomingo)
        {
            try
            {
                guardarGrupoDat(ValorMartes, ConstantesValorizacionDiaria.GrupoCodiParametro, ConstantesValorizacionDiaria.ConceptoCodiFactorRepartoMartes);
                guardarGrupoDat(ValorLunes, ConstantesValorizacionDiaria.GrupoCodiParametro, ConstantesValorizacionDiaria.ConceptoCodiFactorRepartoLunes);
                guardarGrupoDat(ValorMiercoles, ConstantesValorizacionDiaria.GrupoCodiParametro, ConstantesValorizacionDiaria.ConceptoCodiFactorRepartoMiercoles);
                guardarGrupoDat(ValorJueves, ConstantesValorizacionDiaria.GrupoCodiParametro, ConstantesValorizacionDiaria.ConceptoCodiFactorRepartoJueves);
                guardarGrupoDat(ValorViernes, ConstantesValorizacionDiaria.GrupoCodiParametro, ConstantesValorizacionDiaria.ConceptoCodiFactorRepartoViernes);
                guardarGrupoDat(ValorSabado, ConstantesValorizacionDiaria.GrupoCodiParametro, ConstantesValorizacionDiaria.ConceptoCodiFactorRepartoSabado);
                guardarGrupoDat(ValorDomingo, ConstantesValorizacionDiaria.GrupoCodiParametro, ConstantesValorizacionDiaria.ConceptoCodiFactorRepartoDomingo);

                return Json(1);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Json(-1);
            }

        }

        [HttpPost]
        public JsonResult GuardarCostoOportunidad(string ValorCostoOportunidad)
        {
            try
            {
                guardarGrupoDat(ValorCostoOportunidad, ConstantesValorizacionDiaria.GrupoCodiParametro, ConstantesValorizacionDiaria.ConceptoCodiCostoOportunidad);
                return Json(1);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Json(-1);
            }
        }

        [HttpPost]
        public JsonResult GuardarAporteAd(string CostoFueraBanda, string CostoOtrosEquipos, string FRECTotal)
        {
            try
            {
                guardarGrupoDat(CostoFueraBanda, ConstantesValorizacionDiaria.GrupoCodiParametro, ConstantesValorizacionDiaria.ConceptoCodiCostoFueraBanda);
                guardarGrupoDat(CostoOtrosEquipos, ConstantesValorizacionDiaria.GrupoCodiParametro, ConstantesValorizacionDiaria.ConceptoCodiCostoOtrosEquipos);
                guardarGrupoDat(FRECTotal, ConstantesValorizacionDiaria.GrupoCodiParametro, ConstantesValorizacionDiaria.ConceptoCodiFRECTotal);
                return Json(1);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Json(-1);
            }
        }

        [HttpPost]
        public JsonResult GuardarHoraEjecucion(string horaejecucion)
        {
            string[] separadas;
            separadas = horaejecucion.Split(':');
            int hora = Convert.ToInt32(separadas[0]);
            int minutos = Convert.ToInt32(separadas[1]);
            try
            {
                SiProcesoDTO proceso = new SiProcesoDTO
                {
                    Prschorainicio = hora,
                    Prscminutoinicio = minutos
                };

                appValorizacion.UpdateHoraEjecucion(proceso);
                return Json(1);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Json(-1);
            }
        }

        [HttpPost]
        public JsonResult GuardarMontoporReactiva(int emprcodi, decimal caermonto, string date)
        {
            try
            {
                //cambios

                int mes = Int32.Parse(date.Substring(0, 2));
                int anho = Int32.Parse(date.Substring(3, 4));
                DateTime fechaInicio = new DateTime(anho, mes, 1);

                VtdCargoEneReacDTO entity = new VtdCargoEneReacDTO
                {
                    Emprcodi = emprcodi,
                    Caermonto = caermonto,
                    Caersucreacion = base.UserName,
                    Caerfeccreacion = DateTime.Now,
                    Caerfecha = fechaInicio, // DateTime.ParseExact(date, Constantes.FormatoFecha, CultureInfo.InvariantCulture),
                    Caerdeleted = 0,
                    Caerusumodificacion = base.UserName,
                    Caerfecmodificacion = DateTime.Now
                };

                appValorizacion.GuardarVTDMER(entity);
                return Json(1);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Json(-1);
            }
        }

        [HttpPost]
        public JsonResult EliminarMontoporReactiva(int emprcodi, string date)
        {
            DateTime fecha = DateTime.ParseExact(date, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            try
            {
                appValorizacion.DeleteVTDMER(emprcodi, fecha);
                return Json(1);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Json(-1);
            }
        }

        [HttpPost]
        public JsonResult MostrarMontodeEmpresaSeleccionada(int emprcodi)
        {
            decimal monto;
            DateTime fecha = DateTime.Now;
            try
            {
                monto = appValorizacion.ObtenerMontoEnergiaReactivaPorParticipante(emprcodi, fecha).Caermonto;
                return Json(monto);
            }
            catch (Exception)
            {
                return Json(-1);
            }
        }




        private bool guardarGrupoDat(string valor, int Grupocodi, int Concepcodi)
        {
            try
            {
                PrGrupodatDTO entity = new PrGrupodatDTO();
                entity.Fechadat = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1); // DateTime.Now;
                entity.Grupocodi = Grupocodi;
                entity.Concepcodi = Concepcodi;
                entity.Formuladat = valor;
                entity.Fechaact = DateTime.Now;
                entity.Lastuser = UserName;
                PrGrupodatDTO grupo = appDespacho.GetByIdPrGrupodat((DateTime)entity.Fechadat, entity.Concepcodi, entity.Grupocodi, 0);
                if (grupo == null)
                {
                    appDespacho.SavePrGrupodat(entity);
                }
                else { appDespacho.UpdatePrGrupodat(entity); }
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        #region cambios
        public PartialViewResult ListaEnergiaReactiva(string fecha)
        {
            //DateTime date = DateTime.ParseExact(fecha, "dd M yyyy", CultureInfo.InvariantCulture);

            int mes = Int32.Parse(fecha.Substring(0, 2));
            int anho = Int32.Parse(fecha.Substring(3, 4));
            DateTime date = new DateTime(anho, mes, 1);
            try
            {
                ConsultasModel model = new ConsultasModel
                {
                    EnergiaReactiva = appValorizacion.ListVTDMER(date)
                };
                return PartialView(model);
            }
            catch (Exception)
            {
                return PartialView();
            }
        }

        
        #endregion



    }
}
