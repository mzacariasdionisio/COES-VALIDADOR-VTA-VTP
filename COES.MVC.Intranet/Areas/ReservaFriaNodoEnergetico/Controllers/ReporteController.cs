using COES.MVC.Intranet.Areas.ReservaFriaNodoEnergetico.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Servicios.Aplicacion.ReservaFriaNodoEnergetico.Helper;
using COES.MVC.Intranet.Areas.ReservaFriaNodoEnergetico.Helper;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Medidores;
using System.Globalization;
using COES.MVC.Intranet.Areas.Eventos.Models;
using COES.Servicios.Aplicacion.ReservaFriaNodoEnergetico;
using COES.Servicios.Aplicacion.Equipamiento;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;


namespace COES.MVC.Intranet.Areas.ReservaFriaNodoEnergetico.Controllers
{
    public class ReporteController : BaseController
    {

        /// <summary>
        /// Fecha de Inicio de consulta
        /// </summary>
        public DateTime? FechaInicioNrPer
        {
            get
            {
                return (Session[ConstanteReservaFriaNodoEnergetico.FechaConsultaInicioNrPer] != null) ?
                    (DateTime?)(Session[ConstanteReservaFriaNodoEnergetico.FechaConsultaInicioNrPer]) : null;
            }
            set
            {
                Session[ConstanteReservaFriaNodoEnergetico.FechaConsultaInicioNrPer] = value;
            }
        }

        /// <summary>
        /// Fecha de Fin de Consulta
        /// </summary>
        public DateTime? FechaFinalNrPer
        {
            get
            {
                return (Session[ConstanteReservaFriaNodoEnergetico.FechaConsultaFinNrPer] != null) ?
                  (DateTime?)(Session[ConstanteReservaFriaNodoEnergetico.FechaConsultaFinNrPer]) : null;
            }
            set
            {
                Session[DatosSesion.FechaConsultaFin] = value;
            }
        }

        ReservaFriaNodoEnergeticoAppServicio servReservaNodo = new ReservaFriaNodoEnergeticoAppServicio();


        /// <summary>
        /// Muestra la pantalla inicial del módulo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();

            BusquedaReporteModel model = new BusquedaReporteModel();
            model.FechaGeneral = DateTime.Now.AddDays(-30 * 0).ToString(ConstanteReservaFriaNodoEnergetico.FormatoAnioMes);
            model.Submodulo = ConstantesReservaFriaNodoEnergetico.ModuloReservaFria;
            model.ListaNrSubmodulo = servReservaNodo.ListNrSubmodulos();

            DateTime fechaIni = Convert.ToDateTime("2000-01-01");
            DateTime fechaFin = Convert.ToDateTime("2025-01-01");

            List<NrPeriodoDTO> listaPeriodo = servReservaNodo.BuscarOperaciones(
                ConstantesReservaFriaNodoEnergetico.Vigente, fechaIni, fechaFin, -1, -1);
            model.ListaNrPeriodo = listaPeriodo.OrderByDescending(x => x.Nrpermes).ToList();

            return View(model);
        }


        /// <summary>
        /// Permite obtener la EDE de un periodo
        /// </summary>
        /// <param name="idEde">identificador EDE</param>
        /// <param name="periodo">Periodo yyyy-mm</param>
        /// <param name="tag">etiqueta de reporte</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListaEde(int idEde, int periodo, string tag)
        {
            DateTime fechaInicial;
            DateTime fechaFinal;
            int lectcodi96 = -1;
            int origlectcodi = -1;

            switch (idEde)
            {
                case 1: //Reserva Fria
                    lectcodi96 = ConstantesReservaFriaNodoEnergetico.Lectcodi96EdeReservaFria;
                    origlectcodi = ConstantesReservaFriaNodoEnergetico.OrigenLecturaReservaFria;
                    break;
                case 2://IPP
                    lectcodi96 = ConstantesReservaFriaNodoEnergetico.Lectcodi96EdeNodoIndispParcProg;
                    origlectcodi = ConstantesReservaFriaNodoEnergetico.OrigenLecturaNodoEnergetico;
                    break;
                case 3://IPF
                    lectcodi96 = ConstantesReservaFriaNodoEnergetico.Lectcodi96EdeNodoIndispParcFort;
                    origlectcodi = ConstantesReservaFriaNodoEnergetico.OrigenLecturaNodoEnergetico;
                    break;
                case 4://ITP
                    lectcodi96 = ConstantesReservaFriaNodoEnergetico.Lectcodi96EdeNodoIndispTotalProg;
                    origlectcodi = ConstantesReservaFriaNodoEnergetico.OrigenLecturaNodoEnergetico;
                    break;
                case 5://ITF
                    lectcodi96 = ConstantesReservaFriaNodoEnergetico.Lectcodi96EdeNodoIndispTotalFort;
                    origlectcodi = ConstantesReservaFriaNodoEnergetico.OrigenLecturaNodoEnergetico;
                    break;
            }

            List<MeMedicion96DTO> listaEde96 = new List<MeMedicion96DTO>();

            if (lectcodi96 > 0 && origlectcodi > 0)
            {
                ObtenerFechas(periodo, out fechaInicial, out fechaFinal);
                //obtener los puntos de medicion
                List<MePtomedicionDTO> listaPtoMedicion = this.servReservaNodo.ListPtoMedicion(origlectcodi.ToString());
                //obtener empresa
                List<SiEmpresaDTO> listaEmpresa = servReservaNodo.ListarMaestroEmpresas();
                listaEde96 = ListadoEde(fechaInicial, fechaFinal, listaPtoMedicion, listaEmpresa, lectcodi96);
            }

            BusquedaReporteModel model = new BusquedaReporteModel();
            model.ListaMeMedicion96 = listaEde96;
            model.Tag = tag;

            return PartialView(model); ;
        }


        /// <summary>
        /// Permite realizar un listado de Energía dejada de entregar
        /// </summary>
        /// <param name="fechaInicial">Fecha inicial</param>
        /// <param name="fechaFinal">Fecha final</param>
        /// <param name="listaPtoMedicion">Lista de puntos de medición</param>
        /// <param name="listaEmpresa">Lista de empresas</param>
        /// <param name="lectcodi96">Código de lectura  15"</param>
        /// <returns></returns>
        private List<MeMedicion96DTO> ListadoEde(DateTime fechaInicial, DateTime fechaFinal,
            List<MePtomedicionDTO> listaPtoMedicion, List<SiEmpresaDTO> listaEmpresa, int lectcodi96)
        {
            List<MeMedicion96DTO> listaEde96 = new List<MeMedicion96DTO>();

            listaEde96 = this.servReservaNodo.GetByCriteriaMedicion96(ConstantesReservaFriaNodoEnergetico.TipoinfocodiEnergiaActiva,
                Convert.ToInt32(ConstanteReservaFriaNodoEnergetico.ParametroTodos), lectcodi96, fechaInicial, fechaFinal);

            foreach (var itemEde in listaEde96)
            {
                MePtomedicionDTO itemptomedi =
                    listaPtoMedicion.Where(x => x.Ptomedicodi == itemEde.Ptomedicodi).FirstOrDefault();

                SiEmpresaDTO itemEmpresa = null;

                if (itemptomedi != null)
                {
                    itemEmpresa = listaEmpresa.Where(x => x.Emprcodi == itemptomedi.Emprcodi).FirstOrDefault();
                }

                itemEde.Ptomedielenomb = (itemptomedi != null
                    ? itemptomedi.Ptomedielenomb
                    : Constantes.Indeterminado);

                itemEde.Emprnomb = (itemEmpresa != null
                    ? itemEmpresa.Emprnomb
                    : Constantes.Indeterminado);
            }

            return listaEde96;
        }


        /// <summary>
        /// Permite obtener fecha inicial y final de acuerdo a un periodo
        /// </summary>
        /// <param name="fechaym">Periodo</param>
        /// <param name="fechaInicial">Fecha inicial</param>
        /// <param name="fechaFinal">Fecha final</param>
        private void ObtenerFechas(int fechaym, out DateTime fechaInicial, out DateTime fechaFinal)
        {

            NrPeriodoDTO periodo = servReservaNodo.ListNrPeriodos().Where(x => x.Nrpercodi == fechaym).FirstOrDefault();

            //fechaInicial = DateTime.ParseExact(fechaym, Constantes.FormatoAnioMes, CultureInfo.InvariantCulture);
            fechaInicial = periodo.Nrpermes;
            fechaFinal = fechaInicial.AddDays(35);
            fechaFinal = DateTime.ParseExact(fechaFinal.Year + "-" + (fechaFinal.Month < 10 ? "0" : "") + fechaFinal.Month, ConstanteReservaFriaNodoEnergetico.FormatoAnioMes, CultureInfo.InvariantCulture);
            fechaFinal = fechaFinal.AddDays(-1);
        }


        /// <summary>
        /// Permite obtener el listado del proceso
        /// </summary>
        /// <param name="idProceso">Identificador del proceso</param>
        /// <param name="periodo">Periodo</param>
        /// <param name="tag">Nombre de la etiqueta de la tabla</param>
        /// <param name="hora">Es reporte de horas?</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListaProceso(int idProceso, int periodo, string tag, bool hora)
        {
            int nrcptcodi = -1;
            DateTime fechaInicial;
            DateTime fechaFinal;

            switch (idProceso)
            {
                case 1: //Reserva Fría. HDA
                    nrcptcodi = ConstantesReservaFriaNodoEnergetico.RfDemoraEnElArranque;
                    break;
                case 2: //Reserva Fría. HMPE
                    nrcptcodi = ConstantesReservaFriaNodoEnergetico.RfHorasMantenimientoProgramadoEjecutado;
                    break;
                case 3: //Reserva Fría. HMCE
                    nrcptcodi = ConstantesReservaFriaNodoEnergetico.RfHorasMantenimientoCorrectivoEjecutado;
                    break;
                case 4: //Nodo Energetico. sobrecosto itf
                    nrcptcodi = ConstantesReservaFriaNodoEnergetico.NeSobrecostoIndispTotalFortuita;
                    break;
                case 5: //Nodo Energetico. sobrecosto ipf
                    nrcptcodi = ConstantesReservaFriaNodoEnergetico.NeSobrecostoIndispParcialFortuita;
                    break;
            }

            ObtenerFechas(periodo, out fechaInicial, out fechaFinal);

            NrPeriodoDTO periodoDto =
                servReservaNodo.BuscarOperaciones(ConstantesReservaFriaNodoEnergetico.Vigente, fechaInicial, fechaFinal, -1,
                    -1).FirstOrDefault();
            List<NrProcesoDTO> listaProceso = new List<NrProcesoDTO>();
            if (nrcptcodi > 0)
            {
                if (periodoDto != null)
                {
                    listaProceso = ListaProceso(periodoDto.Nrpercodi, nrcptcodi, fechaInicial, fechaFinal);
                }
            }

            BusquedaReporteModel model = new BusquedaReporteModel();
            model.ListaNrProceso = listaProceso;
            model.Tag = tag;
            model.Hora = hora;
            return PartialView(model);
        }


        /// <summary>
        /// Permite listar los procesos
        /// </summary>
        /// <param name="nrpercodi">Código de periodo</param>
        /// <param name="nrcptcodi">Código de concepto</param>
        /// <param name="fechaInicial">Fecha inicial</param>
        /// <param name="fechaFinal">Fecha final</param>
        /// <returns></returns>
        private List<NrProcesoDTO> ListaProceso(int nrpercodi, int nrcptcodi, DateTime fechaInicial, DateTime fechaFinal)
        {
            List<NrProcesoDTO> listaProceso = new List<NrProcesoDTO>();

            listaProceso = servReservaNodo.BuscarOperaciones("N", nrpercodi,
                    ConstantesReservaFriaNodoEnergetico.Todos, nrcptcodi, fechaInicial, fechaFinal, -1, -1)
                .Where(
                    x =>
                        x.Nrprcexceptuacoes == "N" && x.Nrprcexceptuaosinergmin == "N" && x.Nrprcfiltrado == "N")
                .ToList();

            foreach (var itemProceso in listaProceso)
            {
                PrGrupoDTO itemGrupo = servReservaNodo.ObtenerModoOperacion((int)itemProceso.Grupocodi);
                itemProceso.Emprnomb = (itemGrupo != null ? itemGrupo.EmprNomb : Constantes.Indeterminado);
            }

            return listaProceso;
        }


        /// <summary>
        /// Permite obtener el listado del proceso de las horas del Nodo Energético
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <param name="tag">Nombre de la etiqueta de la tabla</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListaProcesoHoraNodo(int periodo, string tag)
        {

            DateTime fechaInicial;
            DateTime fechaFinal;

            List<NrProcesoDTO> listaProcesoFinal = new List<NrProcesoDTO>();

            ObtenerFechas(periodo, out fechaInicial, out fechaFinal);

            NrPeriodoDTO periodoDto =
                servReservaNodo.BuscarOperaciones(ConstantesReservaFriaNodoEnergetico.Vigente, fechaInicial, fechaFinal, -1,
                    -1).FirstOrDefault();

            if (periodoDto != null)
            {
                listaProcesoFinal = ListaHoraNodo(periodoDto.Nrpercodi, fechaInicial, fechaFinal);
            }

            BusquedaReporteModel model = new BusquedaReporteModel();
            model.ListaNrProceso = listaProcesoFinal;
            model.Tag = tag;

            return PartialView(model);
        }


        /// <summary>
        /// Permite obtener el listado de las horas del Nodo Energético
        /// </summary>
        /// <param name="nrpercodi">Código de periodo</param>
        /// <param name="fechaInicial">Fecha inicial</param>
        /// <param name="fechaFinal">Fecha final</param>
        /// <returns></returns>
        public List<NrProcesoDTO> ListaHoraNodo(int nrpercodi, DateTime fechaInicial, DateTime fechaFinal)
        {
            List<NrProcesoDTO> listaProceso = new List<NrProcesoDTO>();
            List<NrProcesoDTO> listaProcesoFinal = new List<NrProcesoDTO>();

            //List<SiEmpresaDTO> listaEmpresa = servSgdoc.ListarMaestroEmpresas();

            listaProceso = servReservaNodo.BuscarOperaciones("N", nrpercodi,
                    ConstantesReservaFriaNodoEnergetico.Todos, ConstantesReservaFriaNodoEnergetico.Todos,
                    fechaInicial, fechaFinal, -1, -1)
                .Where(
                    x =>
                        x.Nrprcexceptuacoes == "N" && x.Nrprcexceptuaosinergmin == "N" && x.Nrprcfiltrado == "N")
                .ToList();

            //reduciendo a horas del nodo energetico
            listaProceso =
                listaProceso.Where(
                        x =>
                            x.Nrcptcodi == ConstantesReservaFriaNodoEnergetico.NeHorasIndispParcialFortuita ||
                            x.Nrcptcodi == ConstantesReservaFriaNodoEnergetico.NeHorasIndispParcialProgramada ||
                            x.Nrcptcodi == ConstantesReservaFriaNodoEnergetico.NeHorasIndispTotalFortuita ||
                            x.Nrcptcodi == ConstantesReservaFriaNodoEnergetico.NeHorasIndispTotalProgramada)
                    .ToList();

            foreach (var itemProceso in listaProceso)
            {

                DateTime fecha =
                    DateTime.ParseExact(
                        ((DateTime)itemProceso.Nrprcfechainicio).ToString(Constantes.FormatoFecha),
                        Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                NrProcesoDTO itemFinal =
                    listaProcesoFinal.Where(
                        x =>
                            (int)x.Grupocodi == (int)itemProceso.Grupocodi &&
                            (DateTime)x.Nrprcfechainicio == fecha).FirstOrDefault();

                if (itemFinal == null)
                {
                    NrProcesoDTO itemFin = new NrProcesoDTO();
                    itemFin.Grupocodi = itemProceso.Grupocodi;

                    PrGrupoDTO itemGrupo = servReservaNodo.ObtenerModoOperacion((int)itemProceso.Grupocodi);
                    itemFin.Emprnomb = (itemGrupo != null ? itemGrupo.EmprNomb : Constantes.Indeterminado);
                    itemFin.Gruponomb = (itemGrupo != null ? itemGrupo.Gruponomb : Constantes.Indeterminado);
                    itemFin.Nrprcfechainicio = fecha;

                    if (itemProceso.Nrcptcodi ==
                        ConstantesReservaFriaNodoEnergetico.NeHorasIndispParcialProgramada ||
                        itemProceso.Nrcptcodi ==
                        ConstantesReservaFriaNodoEnergetico.NeHorasIndispTotalProgramada)
                    {
                        itemFin.Nrprchoraunidad = itemProceso.Nrprchoracentral;
                        itemFin.Nrprchoracentral = 0;
                    }
                    else
                    {
                        itemFin.Nrprchoraunidad = 0;
                        itemFin.Nrprchoracentral = itemProceso.Nrprchoracentral;
                    }

                    listaProcesoFinal.Add(itemFin);
                }
                else
                {
                    if (itemProceso.Nrcptcodi ==
                        ConstantesReservaFriaNodoEnergetico.NeHorasIndispParcialProgramada ||
                        itemProceso.Nrcptcodi ==
                        ConstantesReservaFriaNodoEnergetico.NeHorasIndispTotalProgramada)
                    {
                        itemFinal.Nrprchoraunidad += itemProceso.Nrprchoracentral;
                    }
                    else
                    {
                        itemFinal.Nrprchoracentral += itemProceso.Nrprchoracentral;
                    }
                }

            }

            listaProcesoFinal = listaProcesoFinal.OrderBy(x => x.Gruponomb + x.Nrprcfechainicio).ToList();
            return listaProcesoFinal;
        }


        /// <summary>
        /// Permite elaborar el reporte de acuerdo al submódulo seleccionado
        /// </summary>
        /// <param name="nrsmodcodi">Código de submódulo</param>
        /// <param name="nrpercodi">Código de periodo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Reporte(int nrsmodcodi, int nrpercodi)
        {
            int result = 1;

            DateTime fechaInicial;
            DateTime fechaFinal;

            //ObtenerFechas(nrpercodi, out fechaInicial, out fechaFinal);
            int lectcodi96 = -1;
            int origlectcodi = -1;

            if (nrsmodcodi == 1)
            {
                origlectcodi = ConstantesReservaFriaNodoEnergetico.OrigenLecturaReservaFria;
            }
            else
            {
                origlectcodi = ConstantesReservaFriaNodoEnergetico.OrigenLecturaNodoEnergetico;
            }

            try
            {
                ObtenerFechas(nrpercodi, out fechaInicial, out fechaFinal);
                //obtener los puntos de medicion
                List<MePtomedicionDTO> listaPtoMedicion = servReservaNodo.ListPtoMedicion(origlectcodi.ToString());
                //obtener empresa
                List<SiEmpresaDTO> listaEmpresa = servReservaNodo.ListarMaestroEmpresas();

                BusquedaReporteModel model = new BusquedaReporteModel();


                //Nodo Energético
                switch (nrsmodcodi)
                {
                    case ConstantesReservaFriaNodoEnergetico.ModuloNodoEnergetico:
                        //EDE IPP
                        lectcodi96 = ConstantesReservaFriaNodoEnergetico.Lectcodi96EdeNodoIndispParcProg;
                        model.ListaMeMedicion96 = ListadoEde(fechaInicial, fechaFinal, listaPtoMedicion, listaEmpresa,
                            lectcodi96);

                        //EDE IPF
                        lectcodi96 = ConstantesReservaFriaNodoEnergetico.Lectcodi96EdeNodoIndispParcFort;
                        model.ListaMeMedicion96Ipf = ListadoEde(fechaInicial, fechaFinal, listaPtoMedicion, listaEmpresa,
                            lectcodi96);

                        //EDE ITP
                        lectcodi96 = ConstantesReservaFriaNodoEnergetico.Lectcodi96EdeNodoIndispTotalProg;
                        model.ListaMeMedicion96Itp = ListadoEde(fechaInicial, fechaFinal, listaPtoMedicion, listaEmpresa,
                            lectcodi96);

                        //EDE ITF
                        lectcodi96 = ConstantesReservaFriaNodoEnergetico.Lectcodi96EdeNodoIndispTotalFort;
                        model.ListaMeMedicion96Itf = ListadoEde(fechaInicial, fechaFinal, listaPtoMedicion, listaEmpresa,
                            lectcodi96);

                        //horas
                        model.ListaNrProceso = ListaHoraNodo(nrpercodi, fechaInicial, fechaFinal);

                        //Sobrecosto ITF
                        model.ListaSitf = ListaProceso(nrpercodi, ConstantesReservaFriaNodoEnergetico.NeSobrecostoIndispTotalFortuita, fechaInicial, fechaFinal);

                        //Sobrecosto IPF
                        model.ListaSipf = ListaProceso(nrpercodi, ConstantesReservaFriaNodoEnergetico.NeSobrecostoIndispParcialFortuita, fechaInicial, fechaFinal);


                        ExcelDocument.GenerarArchivoNodoEnergetico(model.ListaMeMedicion96, model.ListaMeMedicion96Ipf,
                            model.ListaMeMedicion96Itp, model.ListaMeMedicion96Itf, fechaInicial, fechaFinal, true, model.ListaNrProceso, model.ListaSitf, model.ListaSipf);
                        break;
                    case ConstantesReservaFriaNodoEnergetico.ModuloReservaFria:
                        //EDE
                        lectcodi96 = ConstantesReservaFriaNodoEnergetico.Lectcodi96EdeReservaFria;
                        model.ListaMeMedicion96 = ListadoEde(fechaInicial, fechaFinal, listaPtoMedicion, listaEmpresa,
                            lectcodi96);

                        //HDA
                        model.ListaHda = ListaProceso(nrpercodi, ConstantesReservaFriaNodoEnergetico.RfDemoraEnElArranque, fechaInicial, fechaFinal);
                        //HMPE
                        model.ListaHmpe = ListaProceso(nrpercodi, ConstantesReservaFriaNodoEnergetico.RfHorasMantenimientoProgramadoEjecutado, fechaInicial, fechaFinal);
                        //HMCE
                        model.ListaHmce = ListaProceso(nrpercodi, ConstantesReservaFriaNodoEnergetico.RfHorasMantenimientoCorrectivoEjecutado, fechaInicial, fechaFinal);

                        ExcelDocument.GenerarArchivoReservaFria(model.ListaMeMedicion96, model.ListaHda,
                            model.ListaHmpe, model.ListaHmce, fechaInicial, fechaFinal, true);
                        break;
                }
                result = 1;
            }
            catch
            {
                result = -1;
            }

            return Json(result);
        }


        /// <summary>
        /// Permite exportar el reporte generado del Nodo Energético
        /// </summary>
        /// <returns>Archivo a descar</returns>
        [HttpGet]
        public virtual ActionResult DescargarNodo()
        {
            string file = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaInformeReservaNodo + NombreArchivo.ReporteNodoEnergetico;
            return File(file, Constantes.AppExcel, NombreArchivo.ReporteNodoEnergetico);
        }

        /// <summary>
        /// Permite exportar el reporte generado del Reserva Fria
        /// </summary>
        /// <returns>Archivo a descargar</returns>
        [HttpGet]
        public virtual ActionResult DescargarReserva()
        {
            string file = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaInformeReservaNodo + NombreArchivo.ReporteReservaFria;
            return File(file, Constantes.AppExcel, NombreArchivo.ReporteReservaFria);
        }
    }    
}
