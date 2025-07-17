using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using COES.Servicios.Aplicacion.Pruebaunidad;
using COES.Servicios.Aplicacion.Helper;
using COES.MVC.Intranet.Helper;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Areas.Eventos.Models;
using COES.Framework.Base.Core;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.MVC.Intranet.Areas.Eventos.Helper;
using COES.Servicios.Aplicacion.Medidores;


namespace COES.MVC.Intranet.Areas.Eventos.Controllers
{
    public class PruebaunidadController : BaseController
    {

        /// <summary>
        /// Fecha de Inicio de consulta
        /// </summary>
        public DateTime? FechaInicioPrUnd
        {
            get
            {
                return (Session[ConstantesEventos.FechaConsultaInicioPrUnd] != null) ?
                    (DateTime?)(Session[ConstantesEventos.FechaConsultaInicioPrUnd]) : null;
            }
            set
            {
                Session[ConstantesEventos.FechaConsultaInicioPrUnd] = value;
            }
        }

        /// <summary>
        /// Fecha de Fin de Consulta
        /// </summary>
        public DateTime? FechaFinalPrUnd
        {
            get
            {
                return (Session[ConstantesEventos.FechaConsultaFinPrUnd] != null) ?
                  (DateTime?)(Session[ConstantesEventos.FechaConsultaFinPrUnd]) : null;
            }
            set
            {
                Session[ConstantesEventos.FechaConsultaFinPrUnd] = value;
            }
        }


        PruebaunidadAppServicio servPruebaunidad = new PruebaunidadAppServicio();
        DespachoAppServicio servdespacho = new DespachoAppServicio();


        /// <summary>
        /// Inicio
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();

            BusquedaEvePruebaunidadModel model = new BusquedaEvePruebaunidadModel();
            //model.FechaIni = DateTime.Now.ToString(Constantes.FormatoFecha);
            //model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);

            model.FechaIni = (this.FechaInicioPrUnd != null) ? ((DateTime)this.FechaInicioPrUnd).ToString(Constantes.FormatoFecha) :
   DateTime.Now.AddDays(-15).ToString(Constantes.FormatoFecha);
            model.FechaFin = (this.FechaFinalPrUnd != null) ? ((DateTime)this.FechaFinalPrUnd).ToString(Constantes.FormatoFecha) :
               DateTime.Now.ToString(Constantes.FormatoFecha);

            model.AccionNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
            model.AccionEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.AccionEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);

            return View(model);
        }


        /// <summary>
        /// Edición de Prueba de unidad
        /// </summary>
        /// <param name="id">Código</param>
        /// <param name="accion">Acción</param>
        /// <returns></returns>
        public ActionResult Editar(int id, int accion)
        {

            EvePruebaunidadModel model = new EvePruebaunidadModel();
            EvePruebaunidadDTO evePruebaunidad = null;

            model.ListaGrupo = new List<PrGrupoDTO>();

            if (id != 0)
                evePruebaunidad = servPruebaunidad.GetByIdEvePruebaunidad(id);

            if (evePruebaunidad != null)
            {
                model.EvePruebaunidad = evePruebaunidad;
            }
            else
            {
                evePruebaunidad = new EvePruebaunidadDTO();
                evePruebaunidad.Prundfecha = DateTime.ParseExact(DateTime.Now.ToString(Constantes.FormatoFecha), Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                evePruebaunidad.Prundescenario = 1;
                model.EvePruebaunidad = evePruebaunidad;

            }

            model.Accion = accion;
            return View(model);
        }


        /// <summary>
        /// Permite obtener la unidad sorteada de acuerdo a una fecha seleccionada
        /// </summary>
        /// <param name="fecha">Fecha</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UnidadSorteada(string fecha)
        {
            try
            {
                DateTime fechaUnidad = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                EqEquipoDTO equipo = servPruebaunidad.ObtenerUnidadSorteada(fechaUnidad);
                return Json(equipo.Equicodi + "," + equipo.Equinomb);
            }
            catch
            {
                return Json("-1,  ");
            }
        }

        /// <summary>
        /// Permite obtener la unidad sorteada de acuerdo a una fecha seleccionada
        /// </summary>
        /// <param name="fecha">Fecha</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ValidarSorteo()
        {
            try
            {
                DateTime fechaUnidad = DateTime.ParseExact(DateTime.Now.ToString(Constantes.FormatoFecha), Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                List<EvePruebaunidadDTO> lista = servPruebaunidad.BuscarOperaciones("N", fechaUnidad, fechaUnidad, -1, -1).ToList();

                if (lista.Count == 0)
                {
                    EqEquipoDTO equipo = servPruebaunidad.ObtenerUnidadSorteada(fechaUnidad);
                    return Json(equipo.Equicodi + "," + equipo.Equinomb);
                }
                else
                {
                    return Json("-2,  ");
                }

            }
            catch
            {
                return Json("-1,  ");
            }
        }


        /// <summary>
        /// Permite grabar un registro pero sin unidad sorteada
        /// </summary>
        /// <returns></returns>
        public JsonResult CrearRegistroSinSorteo()
        {

            try
            {
                EvePruebaunidadDTO entity = new EvePruebaunidadDTO();
                DateTime fechaUnidad = DateTime.ParseExact(DateTime.Now.ToString(Constantes.FormatoFecha), Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                entity.Prundfecha = fechaUnidad;
                entity.Prundusucreacion = base.UserName;
                entity.Prundfeccreacion = DateTime.Now;

                entity.Prundescenario = 1;
                entity.Prundeliminado = "N";
                entity.Grupocodi = 0;

                int id = this.servPruebaunidad.SaveEvePruebaunidadId(entity);
                return Json(id.ToString());

            }
            catch
            {
                return Json("-1");
            }
        }




        /// <summary>
        /// Permite obtener la unidad sorteada de acuerdo a una fecha seleccionada
        /// </summary>
        /// <param name="equicodi">Código de equipo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ModoUnidadSorteada(int equicodi)
        {
            EvePruebaunidadModel model = new EvePruebaunidadModel();

            try
            {
                model.ListaGrupo = servdespacho.ListarModoOperacionDeEquipo(equicodi, ConstantesEventos.CatecodiGrupoFuncional);
                return Json(model.ListaGrupo);
            }
            catch
            {
                return Json("-1");
            }
        }


        /// <summary>
        /// Permite obtener los parámetros de base de datos de un modo de operación
        /// </summary>
        /// <param name="grupocodi">Código de grupo</param>
        /// <param name="fecha">Fecha</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ParametroUnidadSorteada(int grupocodi, string fecha, int equicodi)
        {

            EvePruebaunidadModel model = new EvePruebaunidadModel();

            try
            {
                DateTime fechaModo = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                //obtiene la potencia efectiva de la unidad
                decimal potenciaUnidad = servPruebaunidad.ObtenerValorParametroEquipo(equicodi, ConstantesEventos.ConcepcodiPotenciaEfectiva, fechaModo);

                if (potenciaUnidad < 0)
                {
                    model.PotenciaEfectiva = servPruebaunidad.ObtenerValorParametroGrupo(grupocodi, ConstantesEventos.ConcepcodiPotenciaEfectiva, fechaModo);
                }
                else
                {
                    model.PotenciaEfectiva = potenciaUnidad;
                }

                try
                {
                    model.TiempoEntreArranques = servPruebaunidad.ObtenerValorParametroGrupo(grupocodi, ConstantesEventos.ConcepcodiTiempoEntreArranques, fechaModo);
                }
                catch
                {
                    model.TiempoEntreArranques = 0;
                }
                model.TiempoArranqueSinc = servPruebaunidad.ObtenerValorParametroGrupo(grupocodi, ConstantesEventos.ConcepcodiTiempoArranqueSinc, fechaModo);
                model.TiempoSincPotEf = servPruebaunidad.ObtenerValorParametroGrupo(grupocodi, ConstantesEventos.ConcepcodiTiempoSincPotEf, fechaModo);

                //rpf
                model.Rpf = servPruebaunidad.ObtenerValorParametroGrupo(0, ConstantesEventos.ConcepcodiRpf, fechaModo);

                //tiempo de prueba aleatoria
                model.TiempoPrueba = servPruebaunidad.ObtenerValorParametroGrupo(0, ConstantesEventos.ConcepcodiTiempoPruebaAleat, fechaModo);

                //ratio
                model.PrundRatio = servPruebaunidad.ObtenerValorParametroGrupo(0, ConstantesEventos.ConcepcodiRatioPcalPefect, fechaModo);

                return Json(model);
            }
            catch
            {
                return Json("-1");
            }
        }


        /// <summary>
        /// Permite eliminar un registro
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Eliminar(int id)
        {
            try
            {
                servPruebaunidad.DeleteEvePruebaunidad(id);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }


        /// <summary>
        /// Permite deshabilitar un periodo configurado
        /// </summary>
        /// <param name="id">Código de periodo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Desactivar(int id)
        {
            try
            {
                EvePruebaunidadDTO entity = null;

                if (id != 0)
                {
                    entity = servPruebaunidad.GetByIdEvePruebaunidad(id);

                    entity.Prundusumodificacion = base.UserName;
                    entity.Prundfecmodificacion = DateTime.Now;
                    entity.Prundeliminado = "S";

                    servPruebaunidad.UpdateEvePruebaunidad(entity);
                    return Json(1);
                }
                return Json(-1);
            }
            catch
            {
                return Json(-1);
            }
        }


        /// <summary>
        /// Permite grabar un registro
        /// </summary>
        /// <param name="model">Modelo del tipo Prueba de Unidad</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Grabar(EvePruebaunidadModel model)
        {
            string mensajeJs = ConstantesEventos.MensajeGeneralEscenario;

            try
            {
                DateTime fechaPrueba = DateTime.ParseExact(model.PrundFecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                EvePruebaunidadDTO entity = new EvePruebaunidadDTO();

                if (model.PrundCodi == 0)
                {
                    entity.Prundusucreacion = base.UserName;
                    entity.Prundfeccreacion = DateTime.Now;
                }
                else
                {

                    if (model.PrundUsucreacion != null)
                    {

                        entity.Prundusucreacion = model.PrundUsucreacion;
                    }

                    if (model.PrundFeccreacion != null)
                    {
                        entity.Prundfeccreacion = DateTime.ParseExact(model.PrundFeccreacion, Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                    }

                    entity.Prundusumodificacion = base.UserName;
                    entity.Prundfecmodificacion = DateTime.Now;
                }


                bool fallaRegistrada = (model.PrundFallaotranosincronz == "S") || model.PrundFallaotraunidsincronz == "S" || model.PrundFallaequiposinreingreso == "S";



                entity.Prundcodi = model.PrundCodi;
                entity.Prundfecha = fechaPrueba;
                entity.Prundescenario = model.PrundEscenario;

                if (model.Grupocodi != null)
                {
                    entity.Grupocodi = model.Grupocodi;
                }

                // HO
                if (model.PrundHoraordenarranque != null && (model.PrundEscenario == 1 || model.PrundEscenario == 2 || model.PrundEscenario == 3 || model.PrundEscenario == 4))
                {
                    model.PrundHoraordenarranque = CompletarHora(model.PrundHoraordenarranque, model.PrundFecha);
                    entity.Prundhoraordenarranque = DateTime.ParseExact(model.PrundHoraordenarranque, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                }

                // HS
                if (model.PrundHorasincronizacion != null && (model.PrundEscenario == 1 || model.PrundEscenario == 3 || model.PrundEscenario == 4))
                {
                    model.PrundHorasincronizacion = CompletarHora(model.PrundHorasincronizacion, model.PrundFecha);
                    entity.Prundhorasincronizacion = DateTime.ParseExact(model.PrundHorasincronizacion, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                }

                // HPE
                if (model.PrundHorainiplenacarga != null && (model.PrundEscenario == 1 || model.PrundEscenario == 4))
                {
                    model.PrundHorainiplenacarga = CompletarHora(model.PrundHorainiplenacarga, model.PrundFecha);
                    entity.Prundhorainiplenacarga = DateTime.ParseExact(model.PrundHorainiplenacarga, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                }

                // HFalla
                if (model.PrundHorafalla != null && (model.PrundEscenario == 2 || model.PrundEscenario == 3 || model.PrundEscenario == 4))
                {
                    model.PrundHorafalla = CompletarHora(model.PrundHorafalla, model.PrundFecha);
                    entity.Prundhorafalla = DateTime.ParseExact(model.PrundHorafalla, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                }

                // HO2 
                if (model.PrundHoraordenarranque2 != null && (model.PrundEscenario == 2 || model.PrundEscenario == 3 || model.PrundEscenario == 4))
                {
                    model.PrundHoraordenarranque2 = CompletarHora(model.PrundHoraordenarranque2, model.PrundFecha);
                    entity.Prundhoraordenarranque2 = DateTime.ParseExact(model.PrundHoraordenarranque2, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                }

                // HS2
                if (model.PrundHorasincronizacion2 != null && (model.PrundEscenario == 2 || model.PrundEscenario == 3 || model.PrundEscenario == 4))
                {
                    model.PrundHorasincronizacion2 = CompletarHora(model.PrundHorasincronizacion2, model.PrundFecha);
                    entity.Prundhorasincronizacion2 = DateTime.ParseExact(model.PrundHorasincronizacion2, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                }

                //HPE2
                if (model.PrundHorainiplenacarga2 != null && (model.PrundEscenario == 2 || model.PrundEscenario == 3 || model.PrundEscenario == 4))
                {
                    model.PrundHorainiplenacarga2 = CompletarHora(model.PrundHorainiplenacarga2, model.PrundFecha);
                    entity.Prundhorainiplenacarga2 = DateTime.ParseExact(model.PrundHorainiplenacarga2, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                }


                entity.Prundsegundadesconx = model.PrundSegundadesconx;
                entity.Prundfallaotranosincronz = model.PrundFallaotranosincronz;
                entity.Prundfallaotraunidsincronz = model.PrundFallaotraunidsincronz;
                entity.Prundfallaequiposinreingreso = model.PrundFallaequiposinreingreso;

                if (model.PrundCalchayregmedid != null && model.PrundCalchayregmedid != "X")
                {
                    entity.Prundcalchayregmedid = model.PrundCalchayregmedid;
                }

                if (model.PrundCalcperiodoprogprueba != null)
                {
                    entity.Prundcalcperiodoprogprueba = model.PrundCalcperiodoprogprueba;
                }

                if (model.PrundCalccondhoratarr != null && model.PrundCalccondhoratarr != "X")
                {
                    entity.Prundcalccondhoratarr = model.PrundCalccondhoratarr;
                }


                if (model.PrundCalccondhoraprogtarr != null && model.PrundCalccondhoraprogtarr != "X")
                {
                    entity.Prundcalccondhoraprogtarr = model.PrundCalccondhoraprogtarr;
                }


                if (model.PrundCalchorafineval != null && model.PrundEscenario == 1)
                {
                    entity.Prundcalchorafineval = DateTime.ParseExact(model.PrundCalchorafineval, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                }

                if (model.PrundCalhayindisp != null && model.PrundCalhayindisp != "X")
                {
                    entity.Prundcalhayindisp = model.PrundCalhayindisp;
                }

                if (model.PrundCalcindispprimtramo != null && model.PrundCalcindispprimtramo != "X")
                {
                    entity.Prundcalcindispprimtramo = model.PrundCalcindispprimtramo;
                }

                if (model.PrundCalcindispsegtramo != null && model.PrundCalcindispsegtramo != "X")
                {
                    entity.Prundcalcindispsegtramo = model.PrundCalcindispsegtramo;
                }


                if (model.PrundCalcpruebaexitosa != null && model.PrundCalcpruebaexitosa != "X")
                {
                    entity.Prundcalcpruebaexitosa = model.PrundCalcpruebaexitosa;
                }


                if (model.PrundRpf != null)
                {
                    entity.Prundrpf = model.PrundRpf;
                }

                if (model.PrundTiempoprueba != null)
                {
                    entity.Prundtiempoprueba = model.PrundTiempoprueba;
                }

                //inicio de evaluacion
                entity.Prundcalcpruebaexitosa = "X";


                //1. reglas si fallo la unidad
                //1.1. Falla en otra instalación que imposibilita sincronizar
                if (model.PrundFallaotranosincronz == "S")
                {
                    //prueba exitosa
                    entity.Prundcalcpruebaexitosa = "S";
                }
                //1.2. Falla en otra instalación cuando la unidad está sincronizada
                if (model.PrundFallaotraunidsincronz == "S")
                {
                    //prueba exitosa
                    entity.Prundcalcpruebaexitosa = "S";
                }

                //1.3. Falla de equipo sin reingreso	
                if (model.PrundFallaequiposinreingreso == "S")
                {
                    //prueba NO exitosa
                    entity.Prundcalcpruebaexitosa = "N";
                }




                if (model.PotenciaEfectiva != null)
                {
                    entity.Prundpotefectiva = Convert.ToDecimal(model.PotenciaEfectiva);
                }

                if (model.TiempoEntreArranques != null)
                {
                    entity.Prundtiempoentarranq = Convert.ToDecimal(model.TiempoEntreArranques);
                }

                if (model.TiempoArranqueSinc != null)
                {
                    entity.Prundtiempoarranqasinc = Convert.ToDecimal(model.TiempoArranqueSinc);
                }

                if (model.TiempoSincPotEf != null)
                {
                    entity.Prundtiemposincapotefect = Convert.ToDecimal(model.TiempoSincPotEf);
                }



                int cero = 0;


                if (fallaRegistrada)
                {

                    if (model.PrundFallaotranosincronz == "S" || model.PrundFallaotraunidsincronz == "S")
                    {
                        entity.Prundcalcpruebaexitosa = "S";
                    }
                    else
                    {
                        if (model.PrundFallaequiposinreingreso == "S")
                        {
                            entity.Prundcalcpruebaexitosa = "N";
                        }
                    }

                }
                else
                {


                    #region Evaluacion de Escenarios
                    List<MeMedicion96DTO> datos96;
                    DateTime? horaFinEvaluacion;
                    DateTime? ordenArranque;
                    DateTime? horaSincronz;
                    int ptomedicodi;
                    decimal tiempoPrueba;
                    DateTime? horaFalla;
                    DateTime? ordenArranque2;
                    DateTime? horaSincroniz2;
                    DateTime? horaIniPlenaCarga2;
                    decimal tiempoEntreArranque;

                    DateTime ordenArranque2NoNulo;
                    DateTime ordenArranqueNoNulo;
                    DateTime horaFallaNoNulo;
                    DateTime horaSincroniz2NoNulo;
                    DateTime horaFinEvaluacionNoNulo;
                    DateTime horaIniPlenaCarga2NoNulo;
                    DateTime horaInicioPlenaCargaNoNulo;
                    DateTime horaSincronzNoNulo;

                    DateTime? horaInicioPlenaCarga;

                    switch (model.PrundEscenario)
                    {
                        case 1:

                            mensajeJs = ConstantesEventos.MensajeHorasEscHoHsHpe;// "Revisar horas ingresadas (HO), (HS), (HPE). ";

                            entity.Prundcalchayregmedid = "X";
                            entity.Prundcalhayindisp = "X";
                            entity.Prundcalcpruebaexitosa = "X";

                            ordenArranqueNoNulo = DateTime.ParseExact(model.PrundHoraordenarranque, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                            DateTime horaSincronizacion = DateTime.ParseExact(model.PrundHorasincronizacion, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                            horaInicioPlenaCarga = DateTime.ParseExact(model.PrundHorainiplenacarga, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);

                            string cadenaFecha = "";

                            if (model.PrundFallaotranosincronz == "S" || model.PrundFallaotraunidsincronz == "S" || model.PrundFallaequiposinreingreso == "S")
                            {
                                string mensajeHora = "";

                                //se validan fechas independientes para el escenario: hora de arranque
                                if (model.PrundHoraordenarranque != null & model.PrundHoraordenarranque != "")
                                {
                                    mensajeHora = CompararFecha(ordenArranqueNoNulo, (DateTime)entity.Prundfecha, "(HO)");
                                    if (mensajeHora != "")
                                    {
                                        cadenaFecha += mensajeHora == "" ? "" : "," + mensajeHora;
                                    }
                                }

                                //se validan fechas independientes para el escenario: hora sincronización
                                if (model.PrundHorasincronizacion != null & model.PrundHorasincronizacion != "")
                                {
                                    mensajeHora = CompararFecha(horaSincronizacion, (DateTime)entity.Prundfecha, "(HS)");
                                    if (mensajeHora != "")
                                    {
                                        cadenaFecha += mensajeHora == "" ? "" : "," + mensajeHora;
                                    }
                                }

                                //se validan fechas independientes para el escenario: hora inicio Plena carga
                                if (model.PrundHorainiplenacarga != null & model.PrundHorainiplenacarga != "")
                                {
                                    horaInicioPlenaCargaNoNulo = (DateTime)horaInicioPlenaCarga;
                                    mensajeHora = CompararFecha(horaInicioPlenaCargaNoNulo, (DateTime)entity.Prundfecha, "(HPE)");
                                    if (mensajeHora != "")
                                    {
                                        cadenaFecha += mensajeHora == "" ? "" : "," + mensajeHora;
                                    }
                                }

                                if (cadenaFecha != "")
                                {
                                    cadenaFecha = ConstantesEventos.MensajeDiaPruebaNoCoincide + " : " + cadenaFecha;
                                }
                                mensajeJs = cadenaFecha;
                                var temp = 1 / cero;
                            }
                            else
                            {
                                horaInicioPlenaCargaNoNulo = (DateTime)horaInicioPlenaCarga;
                                string mensajeEscenario1 = CompararFecha(ordenArranqueNoNulo, horaSincronizacion, horaInicioPlenaCargaNoNulo, (DateTime)entity.Prundfecha);
                                mensajeJs += mensajeEscenario1;
                                decimal valorEsc1 = mensajeEscenario1 == "" ? 0 : 1 / cero;
                            }



                            //Hora Final Evaluación
                            tiempoPrueba = (decimal)model.PrundTiempoprueba;
                            horaInicioPlenaCargaNoNulo = (DateTime)horaInicioPlenaCarga;
                            horaFinEvaluacion = horaInicioPlenaCargaNoNulo.AddHours((double)tiempoPrueba);

                            //Presenta Indisponibilidad
                            horaFinEvaluacionNoNulo = (DateTime)horaFinEvaluacion;


                            //--------------------
                            //--- Mediciones 15"

                            //Hay registro de Medidores
                            //a. detectar ptomedicion
                            //b. ver si hay datos

                            ptomedicodi = servPruebaunidad.ObtenerPtomedicionSorteo((DateTime)entity.Prundfecha, ConstantesEventos.OriglectcodiDespacho);

                            //se obtienen mediciones de 15 minutos
                            //datos96 = servPruebaunidad.GetByCriteriaMedicion96(ConstantesEventos.TipoInfoMw, ptomedicodi, ConstantesEventos.LectcodiPruebaAleat,
                            //(DateTime)entity.Prundfecha, (DateTime)entity.Prundfecha);

                            datos96 = servPruebaunidad.GetByCriteriaMedicion96(ConstantesEventos.TipoInfoMw, ptomedicodi, ConstantesEventos.LectcodiPruebaAleat,
                            fechaPrueba, fechaPrueba);


                            if (datos96 == null)
                            {
                                entity.Prundcalchayregmedid = "N";
                            }
                            else
                            {
                                if (datos96.Count > 0)
                                {
                                    entity.Prundcalchayregmedid = "S";
                                }
                                else
                                {
                                    entity.Prundcalchayregmedid = "N";
                                }
                            }
                            //---
                            //--------------------


                            entity.Prundcalhayindisp = ObtenerIndisponibilidad(datos96, horaInicioPlenaCargaNoNulo, horaFinEvaluacionNoNulo, (decimal)model.PotenciaEfectiva, (decimal)model.PrundRpf, (decimal)model.PrundRatio);

                            //Resultado: Prueba Exitosa
                            entity.Prundcalcpruebaexitosa = entity.Prundcalhayindisp == "N" ? "S" : "N";

                            entity.Prundcalchorafineval = horaFinEvaluacionNoNulo;


                            break;
                        case 2:

                            mensajeJs = ConstantesEventos.MensajeHorasEscHoHfallaHo2Hs2Hpe2;//"Revisar horas ingresadas (HO), (HFalla), (HO2), (HS2), (HPE2). ";

                            entity.Prundcalchayregmedid = "X";
                            entity.Prundcalccondhoratarr = "X";
                            entity.Prundcalhayindisp = "X";
                            entity.Prundcalcpruebaexitosa = "X";
                            ordenArranque2 = null;
                            ordenArranque = null;
                            horaFalla = null;
                            horaSincroniz2 = null;
                            horaFinEvaluacion = null;
                            horaIniPlenaCarga2 = null;


                            if (model.PrundHoraordenarranque != null)
                            {
                                ordenArranque = DateTime.ParseExact(model.PrundHoraordenarranque, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                            }

                            if (model.PrundHorafalla != null)
                            {
                                horaFalla = DateTime.ParseExact(model.PrundHorafalla, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                            }

                            if (model.PrundHoraordenarranque2 != null)
                            {
                                ordenArranque2 = DateTime.ParseExact(model.PrundHoraordenarranque2, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                            }

                            if (model.PrundHorasincronizacion2 != null)
                            {
                                horaSincroniz2 = DateTime.ParseExact(model.PrundHorasincronizacion2, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                            }

                            if (model.PrundHorainiplenacarga2 != null)
                            {
                                horaIniPlenaCarga2 = DateTime.ParseExact(model.PrundHorainiplenacarga2, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                            }

                            if (model.PrundSegundadesconx == "N")
                            {
                                ordenArranqueNoNulo = (DateTime)ordenArranque;
                                ordenArranque2NoNulo = (DateTime)ordenArranque2;
                                horaFallaNoNulo = (DateTime)horaFalla;
                                horaSincroniz2NoNulo = (DateTime)horaSincroniz2;
                                horaIniPlenaCarga2NoNulo = (DateTime)horaIniPlenaCarga2;
                                string mensajeEscenario2 = CompararFecha(ordenArranqueNoNulo, horaFallaNoNulo, ordenArranque2NoNulo, horaSincroniz2NoNulo, horaIniPlenaCarga2NoNulo, (DateTime)entity.Prundfecha);
                                mensajeJs += mensajeEscenario2;
                                decimal valorEsc2 = mensajeEscenario2 == "" ? 0 : 1 / cero;
                            }


                            //Hay registro de Medidores
                            //a. detectar ptomedicion
                            //b. ver si hay datos

                            ptomedicodi = servPruebaunidad.ObtenerPtomedicionSorteo((DateTime)entity.Prundfecha, ConstantesEventos.OriglectcodiDespacho);

                            //se obtienen mediciones de 15 minutos
                            //datos96 = servPruebaunidad.GetByCriteriaMedicion96(ConstantesEventos.TipoInfoMw, ptomedicodi, ConstantesEventos.LectcodiPruebaAleat,
                            //(DateTime)entity.Prundfecha, (DateTime)entity.Prundfecha);


                            entity.Prundsegundadesconx = model.PrundSegundadesconx;

                            tiempoEntreArranque = Convert.ToDecimal(model.TiempoEntreArranques);

                            //condicion: if (H.O 2 - Hfalla) > {Tarr}
                            if (ordenArranque2 != null && horaFalla != null)//if (entity.Prundsegundadesconx == "N")
                            {
                                ordenArranque2NoNulo = (DateTime)ordenArranque2;
                                horaFallaNoNulo = (DateTime)horaFalla;
                                entity.Prundcalccondhoratarr =
                                    (decimal)((ordenArranque2NoNulo - horaFallaNoNulo).TotalMinutes) > tiempoEntreArranque ? "S" : "N";
                            }
                            else
                            {
                                entity.Prundcalccondhoratarr = "X";
                            }

                            //Hora Final Evaluación
                            tiempoPrueba = (decimal)model.PrundTiempoprueba;

                            if (horaIniPlenaCarga2 != null)//if (entity.Prundsegundadesconx == "N")
                            {
                                horaIniPlenaCarga2NoNulo = (DateTime)horaIniPlenaCarga2;
                                horaFinEvaluacion = horaIniPlenaCarga2NoNulo.AddHours((double)tiempoPrueba);
                            }

                            entity.Prundcalchayregmedid = "X";

                            //Presenta Indisponibilidad
                            if (horaFinEvaluacion != null && horaIniPlenaCarga2 != null)//if (entity.Prundsegundadesconx == "N")
                            {
                                horaFinEvaluacionNoNulo = (DateTime)horaFinEvaluacion;
                                horaIniPlenaCarga2NoNulo = (DateTime)horaIniPlenaCarga2;


                                datos96 = servPruebaunidad.GetByCriteriaMedicion96(ConstantesEventos.TipoInfoMw, ptomedicodi,
                                    ConstantesEventos.LectcodiPruebaAleat, horaIniPlenaCarga2NoNulo, horaFinEvaluacionNoNulo);

                                if (datos96 == null)
                                {
                                    entity.Prundcalchayregmedid = "N";
                                }
                                else
                                {
                                    if (datos96.Count > 0)
                                    {
                                        entity.Prundcalchayregmedid = "S";
                                    }
                                    else
                                    {
                                        entity.Prundcalchayregmedid = "N";
                                    }
                                }

                                entity.Prundcalhayindisp = ObtenerIndisponibilidad(datos96, horaIniPlenaCarga2NoNulo, horaFinEvaluacionNoNulo, (decimal)model.PotenciaEfectiva, (decimal)model.PrundRpf, (decimal)model.PrundRatio);
                            }
                            else
                            {
                                entity.Prundcalhayindisp = "X";
                            }

                            //Resultado: Prueba Exitosa
                            entity.Prundcalcpruebaexitosa = entity.Prundcalhayindisp == "N" && entity.Prundcalccondhoratarr == "N" && entity.Prundsegundadesconx == "N" ? "S" : "N";

                            if (horaFinEvaluacion != null)//if (entity.Prundsegundadesconx == "N")
                            {
                                horaFinEvaluacionNoNulo = (DateTime)horaFinEvaluacion;
                                entity.Prundcalchorafineval = horaFinEvaluacionNoNulo;
                            }

                            if (ordenArranque2 != null && horaIniPlenaCarga2 != null)//if (entity.Prundsegundadesconx == "N")
                            {
                                ordenArranque2NoNulo = (DateTime)ordenArranque2;
                                horaIniPlenaCarga2NoNulo = (DateTime)horaIniPlenaCarga2;
                                entity.Prundcalcperiodoprogprueba = ((decimal)((horaIniPlenaCarga2NoNulo - ordenArranque2NoNulo).TotalMinutes) + tiempoPrueba * 60) / 60;
                            }

                            break;
                        case 3:

                            mensajeJs = ConstantesEventos.MensajeHorasEscHoHsHfallaHo2Hs2Hpe2;// "Revisar horas ingresadas (HO), (HS), (HFalla), (HO2), (HS2), (HPE2). ";

                            entity.Prundcalchayregmedid = "X";
                            entity.Prundcalccondhoraprogtarr = "X";
                            entity.Prundcalhayindisp = "X";
                            entity.Prundcalcpruebaexitosa = "X";
                            ordenArranque2 = null;
                            ordenArranque = null;
                            horaFalla = null;
                            horaSincroniz2 = null;
                            horaFinEvaluacion = null;
                            horaIniPlenaCarga2 = null;
                            horaSincronz = null;

                            if (model.PrundHoraordenarranque != null)
                            {
                                ordenArranque = DateTime.ParseExact(model.PrundHoraordenarranque, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                            }

                            if (model.PrundHorasincronizacion != null)
                            {
                                horaSincronz = DateTime.ParseExact(model.PrundHorasincronizacion, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                            }

                            if (model.PrundHorafalla != null)
                            {
                                horaFalla = DateTime.ParseExact(model.PrundHorafalla, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                            }

                            if (model.PrundHoraordenarranque2 != null)//if (entity.Prundsegundadesconx == "N")
                            {
                                //ordenArranque2NoNulo = (DateTime)ordenArranque2;
                                ordenArranque2 = DateTime.ParseExact(model.PrundHoraordenarranque2, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                            }

                            if (model.PrundHorasincronizacion2 != null)
                            {
                                horaSincroniz2 = DateTime.ParseExact(model.PrundHorasincronizacion2, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                            }

                            if (model.PrundHorainiplenacarga2 != null)
                            {
                                horaIniPlenaCarga2 = DateTime.ParseExact(model.PrundHorainiplenacarga2, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                            }

                            if (model.PrundSegundadesconx == "N") //if (ordenArranque2 != null && ordenArranque != null && horaFalla != null && horaSincroniz2 != null && horaIniPlenaCarga2 != null && horaSincronz != null)//if (model.PrundSegundadesconx == "N")
                            {
                                ordenArranque2NoNulo = (DateTime)ordenArranque2;
                                ordenArranqueNoNulo = (DateTime)ordenArranque;
                                horaFallaNoNulo = (DateTime)horaFalla;
                                horaSincroniz2NoNulo = (DateTime)horaSincroniz2;
                                horaIniPlenaCarga2NoNulo = (DateTime)horaIniPlenaCarga2;
                                horaSincronzNoNulo = (DateTime)horaSincronz;

                                string mensajeEscenario3 = CompararFecha(ordenArranqueNoNulo, horaSincronzNoNulo, horaFallaNoNulo, ordenArranque2NoNulo, horaSincroniz2NoNulo, horaIniPlenaCarga2NoNulo, (DateTime)entity.Prundfecha);
                                mensajeJs += mensajeEscenario3;
                                decimal valorEsc3 = mensajeEscenario3 == "" ? 0 : 1 / cero;
                            }

                            //Hay registro de Medidores
                            //a. detectar ptomedicion
                            //b. ver si hay datos

                            ptomedicodi = servPruebaunidad.ObtenerPtomedicionSorteo((DateTime)entity.Prundfecha, ConstantesEventos.OriglectcodiDespacho);

                            //se obtienen mediciones de 15 minutos
                            /*
                            datos96 = servPruebaunidad.GetByCriteriaMedicion96(ConstantesEventos.TipoInfoMw, ptomedicodi, ConstantesEventos.LectcodiPruebaAleat,
                                (DateTime)entity.Prundfecha, (DateTime)entity.Prundfecha);

                            if (datos96 == null)
                            {
                                entity.Prundcalchayregmedid = "N";
                            }
                            else
                            {
                                if (datos96.Count > 0)
                                {
                                    entity.Prundcalchayregmedid = "S";
                                }
                                else
                                {
                                    entity.Prundcalchayregmedid = "N";
                                }
                            }
                            */

                            entity.Prundsegundadesconx = model.PrundSegundadesconx;

                            tiempoEntreArranque = Convert.ToDecimal(model.TiempoEntreArranques);

                            //condicion: HO2 - HFalla > min(T,Tarr)
                            tiempoPrueba = (decimal)model.PrundTiempoprueba;

                            if (ordenArranque2 != null && horaFalla != null)//if (entity.Prundsegundadesconx == "N")
                            {
                                ordenArranque2NoNulo = (DateTime)ordenArranque2;
                                horaFallaNoNulo = (DateTime)horaFalla;

                                entity.Prundcalccondhoraprogtarr =
                                    (decimal)((ordenArranque2NoNulo - horaFallaNoNulo).TotalMinutes) > Math.Min(tiempoEntreArranque, tiempoPrueba * 60) ? "S" : "N";
                            }
                            else
                            {
                                entity.Prundcalccondhoraprogtarr = "X";
                            }

                            //Periodo Programado Prueba (T) h.	
                            if (ordenArranque2 != null && horaIniPlenaCarga2 != null)//if (entity.Prundsegundadesconx == "N")
                            {
                                ordenArranque2NoNulo = (DateTime)ordenArranque2;
                                horaIniPlenaCarga2NoNulo = (DateTime)horaIniPlenaCarga2;

                                entity.Prundcalcperiodoprogprueba = ((decimal)((horaIniPlenaCarga2NoNulo - ordenArranque2NoNulo).TotalMinutes) + tiempoPrueba * 60) / 60;
                            }

                            //Hora Final Evaluación
                            if (ordenArranque2 != null && ordenArranque != null && horaFalla != null && entity.Prundcalcperiodoprogprueba != null)//if (entity.Prundsegundadesconx == "N")
                            {
                                ordenArranque2NoNulo = (DateTime)ordenArranque2;
                                ordenArranqueNoNulo = (DateTime)ordenArranque;
                                horaFallaNoNulo = (DateTime)horaFalla;

                                horaFinEvaluacion = ordenArranque2NoNulo.AddHours((double)entity.Prundcalcperiodoprogprueba - (horaFallaNoNulo - ordenArranqueNoNulo).TotalHours);
                                entity.Prundcalchorafineval = horaFinEvaluacion;
                            }

                            entity.Prundcalchayregmedid = "X";

                            //Presenta Indisponibilidad
                            if (horaFinEvaluacion != null && horaIniPlenaCarga2 != null)//if (entity.Prundsegundadesconx == "N")
                            {
                                horaFinEvaluacionNoNulo = (DateTime)horaFinEvaluacion;
                                horaIniPlenaCarga2NoNulo = (DateTime)horaIniPlenaCarga2;

                                datos96 = servPruebaunidad.GetByCriteriaMedicion96(ConstantesEventos.TipoInfoMw, ptomedicodi, ConstantesEventos.LectcodiPruebaAleat,
                                horaIniPlenaCarga2NoNulo, horaFinEvaluacionNoNulo);

                                if (datos96 == null)
                                {
                                    entity.Prundcalchayregmedid = "N";
                                }
                                else
                                {
                                    if (datos96.Count > 0)
                                    {
                                        entity.Prundcalchayregmedid = "S";
                                    }
                                    else
                                    {
                                        entity.Prundcalchayregmedid = "N";
                                    }
                                }

                                entity.Prundcalhayindisp = ObtenerIndisponibilidad(datos96, horaIniPlenaCarga2NoNulo, horaFinEvaluacionNoNulo, (decimal)model.PotenciaEfectiva, (decimal)model.PrundRpf, (decimal)model.PrundRatio);
                            }
                            else
                            {
                                entity.Prundcalhayindisp = "X";
                            }

                            //Resultado: Prueba Exitosa
                            entity.Prundcalcpruebaexitosa = entity.Prundcalhayindisp == "N" && entity.Prundcalccondhoraprogtarr == "N" && entity.Prundsegundadesconx == "N" ? "S" : "N";

                            /*
                            if (entity.Prundsegundadesconx == "N")
                            {
                                entity.Prundcalchorafineval = horaFinEvaluacion;
                            }
                            */


                            break;
                        case 4:
                            ordenArranque2 = null;

                            mensajeJs = ConstantesEventos.MensajeHorasEscHoHsHpeHfallaHo2Hs2Hpe2;// "Revisar horas ingresadas (HO), (HS), (HPE), (HFalla), (HO2), (HS2), (HPE2). ";

                            entity.Prundcalchayregmedid = "X";
                            entity.Prundcalccondhoraprogtarr = "X";
                            entity.Prundcalcindispprimtramo = "X";
                            entity.Prundcalcindispsegtramo = "X";
                            entity.Prundcalcpruebaexitosa = "X";
                            ordenArranque = null;
                            horaFalla = null;
                            horaSincroniz2 = null;
                            horaFinEvaluacion = null;
                            horaIniPlenaCarga2 = null;
                            horaInicioPlenaCarga = null;
                            horaSincronz = null;

                            if (model.PrundHoraordenarranque != null)
                            {
                                ordenArranque = DateTime.ParseExact(model.PrundHoraordenarranque, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                            }

                            if (model.PrundHorasincronizacion != null)
                            {
                                horaSincronz = DateTime.ParseExact(model.PrundHorasincronizacion, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                            }

                            if (model.PrundHorainiplenacarga != null)
                            {
                                horaInicioPlenaCarga = DateTime.ParseExact(model.PrundHorainiplenacarga, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                            }

                            if (model.PrundHorafalla != null)
                            {
                                horaFalla = DateTime.ParseExact(model.PrundHorafalla, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                            }

                            if (model.PrundHoraordenarranque2 != null)
                            {
                                ordenArranque2 = DateTime.ParseExact(model.PrundHoraordenarranque2, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                            }

                            if (model.PrundHorasincronizacion2 != null)
                            {
                                horaSincroniz2 = DateTime.ParseExact(model.PrundHorasincronizacion2, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                            }

                            if (model.PrundHorainiplenacarga2 != null)
                            {
                                horaIniPlenaCarga2 = DateTime.ParseExact(model.PrundHorainiplenacarga2, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                            }

                            if (model.PrundSegundadesconx == "N") //if (ordenArranque2NoNulo != null && ordenArranqueNoNulo != null && horaFallaNoNulo != null && horaSincroniz2NoNulo != null && horaIniPlenaCarga2NoNulo != null && horaInicioPlenaCargaNoNulo != null && horaSincronzNoNulo !=null)
                            {
                                ordenArranque2NoNulo = (DateTime)ordenArranque2;
                                ordenArranqueNoNulo = (DateTime)ordenArranque;
                                horaFallaNoNulo = (DateTime)horaFalla;
                                horaSincroniz2NoNulo = (DateTime)horaSincroniz2;
                                horaIniPlenaCarga2NoNulo = (DateTime)horaIniPlenaCarga2;
                                horaInicioPlenaCargaNoNulo = (DateTime)horaInicioPlenaCarga;
                                horaSincronzNoNulo = (DateTime)horaSincronz;

                                string mensajeEscenario4 = CompararFecha(ordenArranqueNoNulo, horaSincronzNoNulo, horaInicioPlenaCargaNoNulo, horaFallaNoNulo, ordenArranque2NoNulo, horaSincroniz2NoNulo, horaIniPlenaCarga2NoNulo, (DateTime)entity.Prundfecha);
                                mensajeJs += mensajeEscenario4;
                                decimal valorEsc4 = mensajeEscenario4 == "" ? 0 : 1 / cero;
                            }


                            ptomedicodi = servPruebaunidad.ObtenerPtomedicionSorteo((DateTime)entity.Prundfecha, ConstantesEventos.OriglectcodiDespacho);

                            //se obtienen mediciones de 15 minutos
                            /*
                            datos96 = servPruebaunidad.GetByCriteriaMedicion96(ConstantesEventos.TipoInfoMw, ptomedicodi, ConstantesEventos.LectcodiPruebaAleat,
                                (DateTime)entity.Prundfecha, (DateTime)entity.Prundfecha);

                            if (datos96 == null)
                            {
                                entity.Prundcalchayregmedid = "N";
                            }
                            else
                            {
                                if (datos96.Count > 0)
                                {
                                    entity.Prundcalchayregmedid = "S";
                                }
                                else
                                {
                                    entity.Prundcalchayregmedid = "N";
                                }
                            }
                            */

                            entity.Prundsegundadesconx = model.PrundSegundadesconx;

                            tiempoEntreArranque = Convert.ToDecimal(model.TiempoEntreArranques);

                            //condicion: HO2 - HFalla > min(T,Tarr)
                            tiempoPrueba = (decimal)model.PrundTiempoprueba;

                            if (ordenArranque2 != null && horaFalla != null)//if (entity.Prundsegundadesconx == "N")
                            {
                                ordenArranque2NoNulo = (DateTime)ordenArranque2;
                                horaFallaNoNulo = (DateTime)horaFalla;

                                entity.Prundcalccondhoraprogtarr =
                                    (decimal)((ordenArranque2NoNulo - horaFallaNoNulo).TotalMinutes) > Math.Min(tiempoEntreArranque, tiempoPrueba * 60) ? "S" : "N";
                            }
                            else
                            {
                                entity.Prundcalccondhoraprogtarr = "X";
                            }

                            //Periodo Programado Prueba (T) h.	
                            if (ordenArranque2 != null && horaIniPlenaCarga2 != null)//if (entity.Prundsegundadesconx == "N")
                            {
                                ordenArranque2NoNulo = (DateTime)ordenArranque2;
                                horaIniPlenaCarga2NoNulo = (DateTime)horaIniPlenaCarga2;

                                entity.Prundcalcperiodoprogprueba = ((decimal)((horaIniPlenaCarga2NoNulo - ordenArranque2NoNulo).TotalMinutes) + tiempoPrueba * 60) / 60;
                            }

                            //Hora Final Evaluación
                            if (ordenArranque2 != null && ordenArranque != null && horaFalla != null && entity.Prundcalcperiodoprogprueba != null)//if (entity.Prundsegundadesconx == "N")
                            {
                                ordenArranque2NoNulo = (DateTime)ordenArranque2;
                                ordenArranqueNoNulo = (DateTime)ordenArranque;
                                horaFallaNoNulo = (DateTime)horaFalla;

                                horaFinEvaluacion = ordenArranque2NoNulo.AddHours((double)entity.Prundcalcperiodoprogprueba - (horaFallaNoNulo - ordenArranqueNoNulo).TotalHours);
                            }

                            entity.Prundcalchayregmedid = "X";

                            string hayRegMedidores1 = "X";
                            string hayRegMedidores2 = "X";

                            //Presenta Indisponibilidad
                            if (horaInicioPlenaCarga != null && horaFalla != null)//if (entity.Prundsegundadesconx == "N")
                            {
                                horaFallaNoNulo = (DateTime)horaFalla;
                                horaInicioPlenaCargaNoNulo = (DateTime)horaInicioPlenaCarga;

                                datos96 = servPruebaunidad.GetByCriteriaMedicion96(ConstantesEventos.TipoInfoMw, ptomedicodi, ConstantesEventos.LectcodiPruebaAleat,
                                            horaInicioPlenaCargaNoNulo, horaFallaNoNulo);

                                if (datos96 == null)
                                {
                                    hayRegMedidores1 = "N";// entity.Prundcalchayregmedid = "N";
                                }
                                else
                                {
                                    if (datos96.Count > 0)
                                    {
                                        hayRegMedidores1 = "S";//entity.Prundcalchayregmedid = "S";
                                    }
                                    else
                                    {
                                        hayRegMedidores1 = "N";////entity.Prundcalchayregmedid = "N";
                                    }
                                }

                                entity.Prundcalcindispprimtramo = ObtenerIndisponibilidad(datos96, horaInicioPlenaCargaNoNulo, horaFallaNoNulo, (decimal)model.PotenciaEfectiva, (decimal)model.PrundRpf, (decimal)model.PrundRatio);
                            }
                            else
                            {
                                entity.Prundcalcindispprimtramo = "X";
                            }

                            if (horaFinEvaluacion != null && horaIniPlenaCarga2 != null)
                            {
                                horaFinEvaluacionNoNulo = (DateTime)horaFinEvaluacion;
                                horaIniPlenaCarga2NoNulo = (DateTime)horaIniPlenaCarga2;

                                datos96 = servPruebaunidad.GetByCriteriaMedicion96(ConstantesEventos.TipoInfoMw, ptomedicodi, ConstantesEventos.LectcodiPruebaAleat,
                                            horaIniPlenaCarga2NoNulo, horaFinEvaluacionNoNulo);

                                if (datos96 == null)
                                {
                                    hayRegMedidores2 = "N";//entity.Prundcalchayregmedid = "N";
                                }
                                else
                                {
                                    if (datos96.Count > 0)
                                    {
                                        hayRegMedidores2 = "S";//entity.Prundcalchayregmedid = "S";
                                    }
                                    else
                                    {
                                        hayRegMedidores2 = "N";//entity.Prundcalchayregmedid = "N";
                                    }
                                }

                                entity.Prundcalcindispsegtramo = ObtenerIndisponibilidad(datos96, horaIniPlenaCarga2NoNulo, horaFinEvaluacionNoNulo, (decimal)model.PotenciaEfectiva, (decimal)model.PrundRpf, (decimal)model.PrundRatio);
                            }
                            else
                            {
                                entity.Prundcalcindispsegtramo = "X";
                            }

                            //XX
                            //XS

                            entity.Prundcalchayregmedid = "X";

                            if (hayRegMedidores1 == "S" && hayRegMedidores2 == "S")
                            {
                                entity.Prundcalchayregmedid = "S";
                            }
                            else
                            {
                                if (hayRegMedidores1 == "N" && hayRegMedidores2 == "N")
                                {
                                    entity.Prundcalchayregmedid = "N";
                                }
                                else
                                {
                                    if (hayRegMedidores1 == "X" || hayRegMedidores2 == "X")
                                    {
                                        entity.Prundcalchayregmedid = "X";
                                    }
                                    else
                                    {
                                        if (hayRegMedidores1 == "S" || hayRegMedidores2 == "N")
                                        {
                                            entity.Prundcalchayregmedid = "N";
                                        }
                                        else
                                        {
                                            entity.Prundcalchayregmedid = "X";
                                        }
                                    }
                                }
                            }



                            //Resultado: Prueba Exitosa
                            entity.Prundcalcpruebaexitosa = entity.Prundcalcindispprimtramo == "N" && entity.Prundcalcindispsegtramo == "N" && entity.Prundcalccondhoraprogtarr == "N" && entity.Prundsegundadesconx == "N" ? "S" : "N";

                            entity.Prundcalchorafineval = horaFinEvaluacion;



                            break;
                        default:

                            break;
                    }

                    #endregion
                }

                entity.Prundeliminado = model.Prundeliminado;

                int id = this.servPruebaunidad.SaveEvePruebaunidadId(entity);
                return Json(id.ToString());

            }
            catch
            {

                switch (model.PrundEscenario)
                {
                    case 1:

                        break;
                    case 2:

                        break;
                    case 3:

                        break;
                    case 4:

                        break;
                    default:
                        mensajeJs = ConstantesEventos.MensajeGeneralEscenario; //"Ha ocurrido un error";
                        break;
                }

                return Json("-1," + mensajeJs);
            }
        }


        /// <summary>
        /// Permite completar la hora dado sus valores de hora y minuto
        /// </summary>
        /// <param name="fechaHora">Fecha-hora</param>
        /// <returns></returns>
        private string CompletarHora(string fechaHora, string fechaRef)
        {
            string rpta = fechaHora.Replace("h", "0");
            rpta = rpta.Replace("m", "0");

            /*
            if (fechaHora.IndexOf("d") >= 0 || fechaHora.IndexOf("/m") >= 0 || fechaHora.IndexOf("m/") >= 0 || fechaHora.IndexOf("yyyy") >= 0)
            {
                rpta = fechaRef + " " + rpta.Substring(11);
            }
            */

            return rpta;
        }


        /// <summary>
        /// Permite comparar fechas
        /// </summary>
        /// <param name="orden">Orden</param>
        /// <param name="fechaPrueba">Fecha de prueba</param>
        /// <returns></returns>
        private string CompararFecha(DateTime orden, DateTime fechaPrueba, string retorno)
        {
            if (fechaPrueba.ToString(Constantes.FormatoFecha) == orden.ToString(Constantes.FormatoFecha))
            {
                return "";
            }
            else
            {
                return retorno; //"Día de prueba no coincide con fechas ingresadas.";
            }
        }

        /// <summary>
        /// Permite comparar fechas
        /// </summary>
        /// <param name="ordenArranque">Orden de arranque</param>
        /// <param name="horaSincronizacion">Hora de Sincronización</param>
        /// <param name="horaInicioPlenaCarga">Hora de inicio a Plena carga</param>
        /// <param name="fechaPrueba">Fecha de prueba</param>
        /// <returns></returns>
        private string CompararFecha(DateTime ordenArranque, DateTime horaSincronizacion, DateTime horaInicioPlenaCarga, DateTime fechaPrueba)
        {
            if (ordenArranque < horaSincronizacion && horaSincronizacion < horaInicioPlenaCarga)
            {
                if (fechaPrueba.ToString(Constantes.FormatoFecha) == ordenArranque.ToString(Constantes.FormatoFecha))
                {
                    return "";
                }
                else
                {
                    return ConstantesEventos.MensajeDiaPruebaNoCoincide; //"Día de prueba no coincide con fechas ingresadas.";
                }
            }
            else
            {
                return ConstantesEventos.MensajeInconsistFechaHora;// "Inconsistencia de fecha-hora.";
            }

        }


        /// <summary>
        /// Permite comparar fechas
        /// </summary>
        /// <param name="ordenArranque">Orden de arranque</param>
        /// <param name="horaFalla">Hora de Falla</param>
        /// <param name="ordenArranque2">Orden de Arranque 2</param>
        /// <param name="horaSincroniz2">Hora de Sincronización 2</param>
        /// <param name="horaIniPlenaCarga2">Hora de Inicio a Plena carga 2</param>
        /// <param name="fechaPrueba">Fecha de Prueba</param>
        /// <returns></returns>
        private string CompararFecha(DateTime ordenArranque, DateTime horaFalla, DateTime ordenArranque2, DateTime horaSincroniz2, DateTime horaIniPlenaCarga2, DateTime fechaPrueba)
        {

            if (ordenArranque < horaFalla && horaFalla < ordenArranque2 && ordenArranque2 < horaSincroniz2 && horaSincroniz2 < horaIniPlenaCarga2)
            {
                if (fechaPrueba.ToString(Constantes.FormatoFecha) == ordenArranque.ToString(Constantes.FormatoFecha))
                {
                    return "";
                }
                else
                {
                    return ConstantesEventos.MensajeDiaPruebaNoCoincide; //"Día de prueba no coincide con fechas ingresadas.";
                }
            }
            else
            {
                return ConstantesEventos.MensajeInconsistFechaHora;//"Inconsistencia de fecha-hora.";
            }

        }


        /// <summary>
        /// Permite comparar fechas
        /// </summary>
        /// <param name="ordenArranque">Orden de arranque</param>
        /// <param name="horaSincroniz">Hora de Sincronización</param>
        /// <param name="horaFalla">Hora de Falla</param>
        /// <param name="ordenArranque2">Orden de Arranque 2</param>
        /// <param name="horaSincroniz2">Hora de Sincronización 2</param>
        /// <param name="horaIniPlenaCarga2">Hora de Inicio a Plena carga 2</param>
        /// <param name="fechaPrueba">Fecha de Prueba</param>
        /// <returns></returns>
        private string CompararFecha(DateTime ordenArranque, DateTime horaSincroniz, DateTime horaFalla, DateTime ordenArranque2, DateTime horaSincroniz2, DateTime horaIniPlenaCarga2, DateTime fechaPrueba)
        {

            if (ordenArranque < horaSincroniz && horaSincroniz < horaFalla && horaFalla < ordenArranque2 && ordenArranque2 < horaSincroniz2 && horaSincroniz2 < horaIniPlenaCarga2)
            {
                if (fechaPrueba.ToString(Constantes.FormatoFecha) == ordenArranque.ToString(Constantes.FormatoFecha))
                {
                    return "";
                }
                else
                {
                    return ConstantesEventos.MensajeDiaPruebaNoCoincide; //"Día de prueba no coincide con fechas ingresadas.";
                }
            }
            else
            {
                return ConstantesEventos.MensajeInconsistFechaHora;//"Inconsistencia de fecha-hora.";
            }

        }


        /// <summary>
        /// Permite comparar fechas
        /// </summary>
        /// <param name="ordenArranque">Orden de arranque</param>
        /// <param name="horaSincroniz">Hora de Sincronización</param>
        /// <param name="horaInicioPlenaCarga">Hora de Inicio a Plena carga</param>
        /// <param name="horaFalla">Hora de Falla</param>
        /// <param name="ordenArranque2">Orden de Arranque 2</param>
        /// <param name="horaSincroniz2">Hora de Sincronización 2</param>
        /// <param name="horaIniPlenaCarga2">Hora de Inicio a Plena carga 2</param>
        /// <param name="fechaPrueba">Fecha de Prueba</param>
        /// <returns></returns>
        private string CompararFecha(DateTime ordenArranque, DateTime horaSincroniz, DateTime horaInicioPlenaCarga, DateTime horaFalla, DateTime ordenArranque2, DateTime horaSincroniz2, DateTime horaIniPlenaCarga2, DateTime fechaPrueba)
        {

            if (ordenArranque < horaSincroniz && horaSincroniz < horaInicioPlenaCarga && horaInicioPlenaCarga < horaFalla && horaFalla < ordenArranque2 && ordenArranque2 < horaSincroniz2 && horaSincroniz2 < horaIniPlenaCarga2)
            {
                if (fechaPrueba.ToString(Constantes.FormatoFecha) == ordenArranque.ToString(Constantes.FormatoFecha))
                {
                    return "";
                }
                else
                {
                    return ConstantesEventos.MensajeDiaPruebaNoCoincide; //"Día de prueba no coincide con fechas ingresadas.";
                }
            }
            else
            {
                return ConstantesEventos.MensajeInconsistFechaHora;//"Inconsistencia de fecha-hora.";
            }

        }


        /// <summary>
        /// Permite obtener la indisponibilidad
        /// </summary>
        /// <param name="datos96">Registro de 15 minutos</param>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <param name="potenciaEfectiva">Potencia efectiva</param>
        /// <param name="rpf">Valor de RPF</param>
        /// <param name="ratio">Ratio calculado/efectivo </param>
        /// <returns></returns>
        public string ObtenerIndisponibilidad(List<MeMedicion96DTO> datos96, DateTime fechaIni, DateTime fechaFin, decimal potenciaEfectiva, decimal rpf, decimal ratio)
        {

            int h0 = 0;
            int h1 = 0;
            decimal total = 0;
            int registros = 0;
            decimal promedio = 0;



            //lista ordenada
            datos96 = datos96.OrderBy(o => o.Medifecha).ToList();
            fechaIni = fechaIni.AddDays(-(fechaIni - (DateTime)datos96.First().Medifecha).Days);
            fechaFin = fechaFin.AddDays(-(fechaFin - (DateTime)datos96.First().Medifecha).Days);


            foreach (MeMedicion96DTO item in datos96)
            {
                DateTime fechaActual = (DateTime)item.Medifecha;

                if (fechaActual.ToString(Constantes.FormatoFecha) != fechaIni.ToString(Constantes.FormatoFecha))
                {
                    h0 = 1;
                }
                else
                {
                    h0 = fechaIni.Hour * 4 + fechaIni.Minute / 15;
                    h0++;

                    if (h0 >= 96)
                    {
                        h0 = 96;
                    }
                }



                if (fechaActual.ToString(Constantes.FormatoFecha) != fechaFin.ToString(Constantes.FormatoFecha))
                {
                    h1 = 96;
                }
                else
                {
                    h1 = fechaFin.Hour * 4 + (fechaFin.Minute / 15);
                    if (h1 == 0) h1 = 1;
                }


                for (int i = h0; i <= h1; i++)
                {
                    var valor = item.GetType().GetProperty("H" + i.ToString()).GetValue(item, null);
                    total += valor == null ? 0 : (decimal)valor;
                    registros += 1;
                }
            }

            promedio = registros == 0 ? 0 : (decimal)(total / ((decimal)(registros * 1.0)));

            //Potencia efectiva- (Promedio MW 15m desde inicio plena carga hasta fin de 
            //prueba)*(1+%rpf)>Potencia efectiva*ratio

            return (potenciaEfectiva - promedio * (1 + rpf) > potenciaEfectiva * ratio) ? "S" : "N";


        }


        /// <summary>
        /// Permite listar las unidades
        /// </summary>
        /// <param name="prundFechaIni">Fecha Inicial</param>
        /// <param name="prundFechaFin">Fecha Final</param>
        /// <param name="nroPage">Número de página</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(string estado, string prundFechaIni, string prundFechaFin, int nroPage)
        {
            BusquedaEvePruebaunidadModel model = new BusquedaEvePruebaunidadModel();

            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFinal = DateTime.Now;

            if (prundFechaIni != null)
            {
                fechaInicio = DateTime.ParseExact(prundFechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            if (prundFechaFin != null)
            {
                fechaFinal = DateTime.ParseExact(prundFechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            this.FechaInicioPrUnd = fechaInicio;
            this.FechaFinalPrUnd = fechaFinal;

            model.ListaEvePruebaunidad = servPruebaunidad.BuscarOperaciones(estado, fechaInicio, fechaFinal, nroPage, Constantes.PageSizeEvento).ToList();
            model.AccionEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.AccionEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);
            return PartialView(model);
        }


        /// <summary>
        /// Permite obtener el paginado
        /// </summary>
        /// <param name="prundFechaIni">Fecha Inicial</param>
        /// <param name="prundFechaFin">fecha Final</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(string estado, string prundFechaIni, string prundFechaFin)
        {
            Paginacion model = new Paginacion();

            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFinal = DateTime.Now;

            if (prundFechaIni != null)
            {
                fechaInicio = DateTime.ParseExact(prundFechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            if (prundFechaFin != null)
            {
                fechaFinal = DateTime.ParseExact(prundFechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }


            int nroRegistros = servPruebaunidad.ObtenerNroFilas(estado, fechaInicio, fechaFinal);

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSizeEvento;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return base.Paginado(model);
        }
    }
}
