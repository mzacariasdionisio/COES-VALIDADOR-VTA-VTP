using COES.MVC.Intranet.Helper;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Transferencias;
using COES.MVC.Intranet.Areas.Transferencias.Models;
using COES.MVC.Intranet.Areas.Transferencias.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;

namespace COES.MVC.Intranet.Areas.Transferencias.Controllers
{
    public class ComparativoEntregaRetiroController : Controller
    {
        TransferenciasAppServicio servicioTransferencias = new TransferenciasAppServicio();
        EmpresaAppServicio servicioEmpresa = new EmpresaAppServicio();
        PeriodoAppServicio servicioPeriodo = new PeriodoAppServicio();
        RecalculoAppServicio servicioRecalculo = new RecalculoAppServicio();
        BarraAppServicio servicioBarra = new BarraAppServicio();
        ValorTransferenciaAppServicio servicioVal = new ValorTransferenciaAppServicio();

        public ActionResult Index(int pericodi = 0, int pericodi2 = 0, int recacodi = 0, int recacodi2 = 0, int emprcodi = 0, int trnenvtipinf = 0)
        {
            ConsultaEnviosModel model = new ConsultaEnviosModel();
            model.ListaCodigos = new List<ValorTransferenciaDTO>();
            model.ListaPeriodos = this.servicioPeriodo.ListPeriodo();
            if (model.ListaPeriodos.Count > 0 && pericodi == 0)
            {
                pericodi = model.ListaPeriodos[0].PeriCodi;
            }
            if (pericodi > 0)
            {
                //model.EntidadPeriodo = this.servicioPeriodo.GetByIdPeriodo(pericodi);

                model.ListaRecalculos = this.servicioRecalculo.ListRecalculos(pericodi); //Ordenado en descendente
                if (model.ListaRecalculos.Count > 0 && recacodi == 0)
                {
                    recacodi = model.ListaRecalculos[0].RecaCodi;
                }
            }

            //a
            if (model.ListaPeriodos.Count > 0 && pericodi2 == 0)
            {
                pericodi2 = model.ListaPeriodos[0].PeriCodi;
            }
            if (pericodi2 > 0)
            {

                model.ListaRecalculos2 = this.servicioRecalculo.ListRecalculos(pericodi2); //Ordenado en descendente
                if (model.ListaRecalculos2.Count > 0 && recacodi2 == 0)
                {
                    recacodi2 = model.ListaRecalculos[0].RecaCodi;
                }

            }

            var Periodo1 = pericodi;
            var Periodo2 = pericodi2;

            if (Periodo1 > Periodo2)
                Periodo2 = Periodo1;

            if (pericodi2 < pericodi)
                Periodo1 = pericodi2;


            model.ListaCodigos = new ValorTransferenciaAppServicio().ListarCodigos(0);
            model.ListaEmpresas = new EmpresaAppServicio().ListaInterValorTrans();

            model.ListaTipoInfo = ListTipoInfo();
            //model.ListaBarras = this.servicioBarra.ListBarras();

            model.pericodi = pericodi;
            model.recacodi = recacodi;
            model.pericodi2 = pericodi2;
            model.recacodi2 = recacodi2;
            model.emprcodi = emprcodi;
            model.trnenvtipinf = trnenvtipinf;

            return View(model);
        }

        /// Permite listar los tipos de información
        public List<TipoInformacionDTO> ListTipoInfo()
        {
            List<TipoInformacionDTO> Lista = new List<TipoInformacionDTO>();
            TipoInformacionDTO dtoTipoInfo = new TipoInformacionDTO();
            dtoTipoInfo.TipoInfoCodi = 1;
            dtoTipoInfo.TipoInfoCodigo = "ER";
            dtoTipoInfo.TipoInfoNombre = "Entregas y Retiros en MWH";
            Lista.Add(dtoTipoInfo);
            dtoTipoInfo = new TipoInformacionDTO();
            dtoTipoInfo.TipoInfoCodi = 2;
            dtoTipoInfo.TipoInfoCodigo = "IB";
            dtoTipoInfo.TipoInfoNombre = "Entregas y Retiros Valorizados";
            Lista.Add(dtoTipoInfo);

            return Lista;
        }

        /// <summary>
        /// Prepara una vista para ingresar un nuevo registro
        /// </summary>
        /// <returns></returns>
        public ActionResult ListaCodigo(int trnenvtipinf, int pericodi, int version, int? empcodi, int? cliemprcodi, int? barrcodi, string flagEntrReti, string fechaInicio, string fechaFin)
        {
            empcodi = empcodi == 0 ? null : empcodi;
            ValorTransferenciaModel modelo = new ValorTransferenciaModel();
            DateTime? dtfi = null;
            if (string.IsNullOrEmpty(fechaInicio))
            {
                dtfi = null;
            }
            else
            {
                dtfi = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture); //Convert.ToDateTime(fechaInicio);
            }

            DateTime? dtff = null;
            if (string.IsNullOrEmpty(fechaFin))
            {

                dtff = null;
            }
            else
            {
                dtff = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture); //Convert.ToDateTime(fechaFin);
            }
            barrcodi = barrcodi == 0 ? null : barrcodi;
            if (trnenvtipinf == 1)
                modelo.ListaValorTransferencia = this.servicioVal.ListarCodigosValorizados(pericodi, version, empcodi, cliemprcodi, barrcodi, flagEntrReti, dtfi, dtff);
            else if (trnenvtipinf == 2)
                modelo.ListaValorTransferencia = this.servicioVal.ListarCodigosValorizadosTransferencia(pericodi, version, empcodi, barrcodi, flagEntrReti, dtfi, dtff);

            return PartialView(modelo);
        }


        /// <summary>
        /// Prepara una vista para ingresar un nuevo registro
        /// </summary>
        /// <returns></returns>
        public ActionResult MostrarGrafico(int trnenvtipinf, int pericodi, int version, int? empcodi, string codigos, int tipo, DateTime FechaInicio, DateTime FechaFin)
        {
            ValorTransferenciaModel modelo = new ValorTransferenciaModel();
            //modelo.ListaValorTransferencia = this.servicioVal.ListarCodigosValorizados(pericodi, version, empcodi, cliemprcodi, barrcodi, flagEntrReti);

            if (tipo == 0)
            {
                var dataER = this.servicioVal.ListarCodigosValorizadosGrafica(trnenvtipinf,pericodi, version, empcodi, codigos, FechaInicio, FechaFin);
                var json = new JsonResult();
                json.Data = new { dataER = dataER };
                json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                json.MaxJsonLength = int.MaxValue;
                return json;
            }
            else
            {
                var dataER = this.servicioVal.ListarCodigosValorizadosGraficaTotal(trnenvtipinf,pericodi, version, empcodi, codigos, FechaInicio, FechaFin);
                return Json(new { dataER = dataER }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult ListaComparativo(int trnenvtipinf, int pericodi, int version, int pericodi2, int version2, int?
            empcodi, int? cliemprcodi, int? barrcodi, string flagEntrReti, string dias, string codigos)
        {
            ValorTransferenciaModel modelo = new ValorTransferenciaModel();
            modelo.ListaComparativo = this.servicioVal.ListarComparativo(pericodi, version, pericodi2, version2, empcodi, cliemprcodi, barrcodi,
                                                                                flagEntrReti, dias, codigos);
            return PartialView(modelo);
        }

        /// <summary>
        /// Prepara una vista para ingresar un nuevo registro
        /// </summary>
        /// <returns></returns>
        public ActionResult MostrarComparativos(int trnenvtipinf, int pericodi, int version, int pericodi2, int version2, int?
            empcodi, int? cliemprcodi, int? barrcodi, string flagEntrReti, string dias, string codigos)
        {
            var data = this.servicioVal.ListarComparativo(pericodi, version, pericodi2, version2, empcodi, cliemprcodi, barrcodi,
                                                                                flagEntrReti, dias, codigos);


            return Json(data);
        }




        #region CUP08
        [HttpPost]
        public ActionResult ListaInterCoReSoCliPorEmpresa(int emprcodi)
        {
            return Json(new SelectList(new EmpresaAppServicio().ListaRetiroCliente(emprcodi), "EMPRCODI", "EMPRNOMBRE"));
        }

        [HttpPost]
        public ActionResult ListarTodasLasBarras(string tipoInfoCodigo, int periCodi, int emprCodi, int cliCodi, string enumComparativoEntregaRetiros)
        {
            return Json(new SelectList(new BarraAppServicio().ListaBarraRetirosEntregaEmpresa(tipoInfoCodigo, periCodi, emprCodi, cliCodi, enumComparativoEntregaRetiros), "BarrCodi", "BarrNombre"));
        }

        [HttpPost]
        public ActionResult ListarCostoMarginalDesviacion(ComparacionEntregaRetirosFiltroDTO parametro)
        {

            JsonResult maxJson = new JsonResult();
            maxJson.Data = new ValorTransferenciaAppServicio().ListarComparativoEntregaRetiroValoDesviacion(parametro);
            maxJson.MaxJsonLength = int.MaxValue;

            return maxJson;
        }

        [HttpPost]
        public ActionResult ListarComparativoEntregaRetiroValorDET(ComparacionEntregaRetirosFiltroDTO parametro)
        {

            JsonResult maxJson = new JsonResult();
            maxJson.Data = new ValorTransferenciaAppServicio().ListarComparativoEntregaRetiroValorDET(parametro);
            maxJson.MaxJsonLength = int.MaxValue;

            return maxJson;

        }

        [HttpPost]
        public ActionResult ListarCodigos(int EmprCodi)
        {
            return Json(new ValorTransferenciaAppServicio().ListarCodigos(EmprCodi));
        }
        [HttpPost]
        public ActionResult ListarEmpresasAsociadas(ComparacionEntregaRetirosFiltroDTO parametro)
        {
            return Json(new ValorTransferenciaAppServicio().ListarEmpresasAsociadas(parametro));
        }
        #endregion CUP08
    }
}