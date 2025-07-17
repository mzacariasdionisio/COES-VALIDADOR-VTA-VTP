using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using System.Web.Mvc;
using COES.MVC.Intranet.Areas.PrimasRER.Models;
using COES.Servicios.Aplicacion.PrimasRER;
using System.Reflection;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Transferencias;
using COES.Servicios.Aplicacion.TransfPotencia;

namespace COES.MVC.Intranet.Areas.PrimasRER.Controllers
{
    public class ParametroRERController : Controller
    {
        RecalculoAppServicio servicioRecalculo = new RecalculoAppServicio();
        TransfPotenciaAppServicio servicioTransfPotencia = new TransfPotenciaAppServicio();
        // GET: /PrimasRER/ParametroRER/

        public ParametroRERController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error("Error", objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal("Error", ex);
                throw;
            }
        }

        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>
        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        PrimasRERAppServicio servicioPrimasRER = new PrimasRERAppServicio();


        /// <summary>
        /// Muestra la pantalla principal de los Anios Tarifarios
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Muestra la lista de Anios Tarifarios
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Lista()
        {
            PrimasRERModel model = new PrimasRERModel();
            model.ListaAniosVersion = this.servicioPrimasRER.ListRerAnioVersiones().Where(x => x.Reravaniotarif > 2020).GroupBy(x => x.Reravaniotarif).Select(g => g.First()).ToList();

            return PartialView(model);
        }

        /// <summary>
        /// Muestra el formulario del Anio Tarifario (versiones e inflaciones)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult New()
        {
            PrimasRERModel model = new PrimasRERModel();
            return PartialView(model);
        }

        /// <summary>
        /// Muestra el valor de la inflacion y los estados de las versiones de un Anio Tarifario
        /// </summary>
        /// <param name="Reravcodi">Id del Anio Tarifario</param>
        /// <returns></returns>
        public PartialViewResult Edit(int Reravcodi = 0)
        {
            PrimasRERModel model = new PrimasRERModel();
            model.AnioVersion = this.servicioPrimasRER.GetByIdRerAnioVersion(Reravcodi);
            model.ListaAniosVersion = this.servicioPrimasRER.ListRerAnioVersiones().Where(det => det.Reravaniotarif == model.AnioVersion.Reravaniotarif).ToList();
            return PartialView(model);
        }

        /// <summary>
        /// Crea y guarda las 6 versiones de un Anio Tarifario (Anual, 1er, 2do, 3er, 4to ajuste trimestral y Liquidación), asi como los 12 meses por cada versión
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Save(PrimasRERModel model)
        {
            try
            {
                #region Validacion de inflaciones
                // Validando los valores de la inflacion de cada version
                bool isValidInflaciones = true;
                isValidInflaciones = this.servicioPrimasRER.ValidarInflaciones(model.ListaAniosVersion);

                if (!isValidInflaciones)
                {
                    return Json(-3);
                }
                #endregion

                DateTime fechaActual = DateTime.Now;
                int anioActual = fechaActual.Year;
                int mesActual = fechaActual.Month;

                List<RerAnioVersionDTO> ListaAniosVersion = this.servicioPrimasRER.ListRerAnioVersiones().Where(det => det.Reravaniotarif == anioActual).ToList();
                bool existeAnioTarifario = (ListaAniosVersion != null && ListaAniosVersion.Count != 0);

                if (existeAnioTarifario) {
                    return Json(-2);
                } else{
                    string userName= User.Identity.Name;

                    List<string> mesesDescripcion = this.servicioPrimasRER.obtenerMesesDescripcion(anioActual);

                    int[] numeroMes = new int[]
                    { 5, 6, 7, 8, 9, 10, 11, 12, 1, 2, 3, 4};

                    //Creamos el objeto de un Anio Tarifario
                    var oNuevoRerAnioVersion = new RerAnioVersionDTO()
                    {
                        Reravversion = "0",
                        Reravestado = "1",
                        Reravaniotarif = anioActual,                                    // Año del parámetro de prima RER
                        Reravaniotarifdesc = "Mayo." + Convert.ToString(anioActual) + "-" + "Abril." + Convert.ToString(anioActual + 1),                         // Descripción del rango de meses del año tarifario
                        Reravinflacion = 0,                                            // Valor de la inflación correspondiente al valor al mes de abril del año tarifario anterior
                        Reravusucreacion = userName,
                        Reravfeccreacion = fechaActual,
                        Reravusumodificacion = userName,
                        Reravfecmodificacion = fechaActual
                    };

                    // Creamos el objeto de un mes de un Anio Tarifario
                    var oNuevoRerParametroPrima = new RerParametroPrimaDTO()
                    {
                        Rerpprmes = 0,
                        Rerpprmesaniodesc = null,
                        Rerpprtipocambio = null,
                        Rerpprorigen = null,
                        Rerpprrevision = null,
                        Rerpprusucreacion = userName,
                        Rerpprfeccreacion = fechaActual,
                        Rerpprusumodificacion = userName,
                        Rerpprfecmodificacion = fechaActual
                    };

                    for (int i = 0; i < model.ListaAniosVersion.Count; i++)
                    {
                        int Reravcodi = 0;
                        oNuevoRerAnioVersion.Reravversion = i.ToString();
                        oNuevoRerAnioVersion.Reravestado = model.ListaAniosVersion[i].Reravestado;
                        oNuevoRerAnioVersion.Reravinflacion = model.ListaAniosVersion[i].Reravinflacion;
                        Reravcodi = this.servicioPrimasRER.SaveRerAnioVersion(oNuevoRerAnioVersion);
                        for (int j = 0; j < mesesDescripcion.Count; j++)
                        {
                            oNuevoRerParametroPrima.Reravcodi = Reravcodi;
                            oNuevoRerParametroPrima.Rerpprmes = numeroMes[j];
                            oNuevoRerParametroPrima.Rerpprmesaniodesc = mesesDescripcion[j];
                            this.servicioPrimasRER.SaveRerParametroPrima(oNuevoRerParametroPrima);
                        }
                    }

                    return Json(1);

                }
            }
            catch (Exception)
            {
                return Json(-1);
            }

        }

        /// <summary>
        /// PrimasRER.2023
        /// Actualzar el valor de la inflación y el estado de cada version del Anio Tarifario
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Update(PrimasRERModel model)
        {
            try
            {
                // Validando los valores de la inflacion de cada version
                bool isValidInflaciones = true;
                isValidInflaciones = this.servicioPrimasRER.ValidarInflaciones(model.ListaAniosVersion);

                if (!isValidInflaciones)
                {
                    return Json(-3);
                }

                PrimasRERModel modelUpdate = new PrimasRERModel();
                modelUpdate.ListaAniosVersion = this.servicioPrimasRER.ListRerAnioVersiones().Where(det => (det.Reravaniotarif == model.AnioVersion.Reravaniotarif)).ToList();

                DateTime fechaActual = DateTime.Now;
                string userName = User.Identity.Name;
                int i = 0;
                foreach (var AnioVersion in modelUpdate.ListaAniosVersion)
                {
                    AnioVersion.Reravinflacion = model.ListaAniosVersion[i].Reravinflacion;
                    AnioVersion.Reravestado = model.ListaAniosVersion[i].Reravestado;
                    AnioVersion.Reravfecmodificacion = fechaActual;
                    AnioVersion.Reravusumodificacion = userName;
                    this.servicioPrimasRER.UpdateRerAnioVersion(AnioVersion);
                    i++;
                }

                return Json(1);
            }
            catch (Exception)
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// PrimasRER.2023
        /// Página inicial de los 12 meses de un año tarifario (6 versiones por mes)
        /// </summary>
        /// <param name="Reravcodi">Id del Anio Tarifario</param>
        /// <returns></returns>
        public ActionResult IndexMeses(int Reravcodi = 0)
        {
            PrimasRERModel model = new PrimasRERModel();
            model.AnioVersion = this.servicioPrimasRER.GetByIdRerAnioVersion(Reravcodi);
            model.ListaAniosVersion = this.servicioPrimasRER.ListRerAnioVersiones().Where(det => det.Reravaniotarif == model.AnioVersion.Reravaniotarif).ToList();
            return View(model);
        }

        /// <summary>
        /// Mostrar un listado de los meses de un año tarifario
        /// </summary>
        /// <param name="Reravcodi">Id del Anio Tarifario</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListaMeses(int Reravcodi = 0)
        {
            PrimasRERModel model = new PrimasRERModel();
            model.ListParametroDTO = this.servicioPrimasRER.ListRerParametroPrimas().Where(det => det.Reravcodi == Reravcodi).ToList();
            foreach (var parametroDTO in model.ListParametroDTO)
            {
                if (parametroDTO.Pericodi == null) {
                    parametroDTO.Rerpprrevision = null;
                }
            }
            return PartialView(model);
        }

        /// <summary>
        /// Mostrar un popup con los datos de un mes de una version de un año tarifario
        /// </summary>
        /// <param name="Rerpprcodi">Id del mes de una version de un Anio Tarifario</param>
        /// <returns></returns>
        public PartialViewResult EditMeses(int Rerpprcodi = 0)
        {
            PrimasRERModel model = new PrimasRERModel();
            model.ParametroRER = this.servicioPrimasRER.GetByIdRerParametroPrima(Rerpprcodi);
            model.AnioVersion = this.servicioPrimasRER.GetByIdRerAnioVersion(model.ParametroRER.Reravcodi);
            int anio = model.AnioVersion.Reravaniotarif;
            if (model.ParametroRER.Rerpprmes < 5) {
                anio += 1;
            }
            
            model.ListaRecalculo = this.servicioRecalculo.ListRecalculosByAnioMes(anio, model.ParametroRER.Rerpprmes);
            if (model.ParametroRER.Rerpprorigen == null)
            {
                model.ParametroRER.Rerpprorigen = "VTEA";       // Valor por defecto que aparece en el popup
            }
            if (model.ParametroRER.Rerpprrevision == null )
            {
                if (model.ListaRecalculo.Count != 0) {
                    model.ParametroRER.Rerpprrevision = model.ListaRecalculo[0].RecaNombre;
                } else {
                    model.ParametroRER.Rerpprrevision = null;
                }
            }
            return PartialView(model);
        }

        /// <summary>
        /// PrimasRER.2023
        /// Actualizar un mes (de una versión) de un año tarifario
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateMeses(PrimasRERModel model, string recaPeriCodi)
        {
            try
            {
                PrimasRERModel modelUpdate = new PrimasRERModel();
                modelUpdate.ParametroRER = this.servicioPrimasRER.GetByIdRerParametroPrima(model.ParametroRER.Rerpprcodi);
               
                DateTime fechaActual = DateTime.Now;
                string usuarioActual = User.Identity.Name;
                modelUpdate.ParametroRER.Rerpprtipocambio = model.ParametroRER.Rerpprtipocambio;
                modelUpdate.ParametroRER.Rerpprorigen = model.ParametroRER.Rerpprorigen;

                if (recaPeriCodi != "null")
                {
                    var ids = recaPeriCodi.Split('-');
                    int iRecaCodi = int.Parse(ids[0]);
                    int iPericodi = int.Parse(ids[1]);

                    RecalculoDTO dtoRecalculo = servicioRecalculo.GetByIdRecalculo(iPericodi, iRecaCodi);
                    if (dtoRecalculo != null)
                    {
                        modelUpdate.ParametroRER.Recacodi = iRecaCodi;
                        modelUpdate.ParametroRER.Pericodi = iPericodi;
                        modelUpdate.ParametroRER.Rerpprrevision = dtoRecalculo.RecaNombre;
                    }
                }
                else {
                    modelUpdate.ParametroRER.Recacodi = null;
                    modelUpdate.ParametroRER.Pericodi = null;
                    modelUpdate.ParametroRER.Rerpprrevision = null;
                }
                
                modelUpdate.ParametroRER.Rerpprfecmodificacion = fechaActual;
                modelUpdate.ParametroRER.Rerpprusumodificacion = usuarioActual;
                this.servicioPrimasRER.UpdateRerParametroPrima(modelUpdate.ParametroRER);
                return Json(1);
            }
            catch (Exception)
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Listar las revisiones del periodo VTEA y VTP un mes de una version de un Año Tarifario
        /// </summary>
        /// <param name="Rerpprcodi">Id del mes de una versión de un Anio Tarifario</param>
        /// <returns></returns>
        public PartialViewResult indexRevisiones(int Rerpprcodi = 0)
        {
            PrimasRERModel model = new PrimasRERModel();
            model.ParametroRER = this.servicioPrimasRER.GetByIdRerParametroPrima(Rerpprcodi);

            // Registros de la tabla VTEA
            model.ListaRevisionVTEA = this.servicioPrimasRER.ListRerParametroRevisionesByRerpprcodiByTipo(Rerpprcodi, "VTEA");
            // Registros de la tabla VTP
            model.ListaRevisionVTP = this.servicioPrimasRER.ListRerParametroRevisionesByRerpprcodiByTipo(Rerpprcodi, "VTP");
            
            return PartialView(model);
        }

        /// <summary>
        /// PrimasRER.2023
        /// Obtiene las revisiones LVTEA y VTP de un Mes de un Anio Tarifarios
        /// </summary>
        /// <param name="Rerpprcodi"></param>
        /// <returns></returns>
        public ActionResult RecargarRevisiones(int Rerpprcodi = 0)
        {
            PrimasRERModel model = new PrimasRERModel();

            //Eliminamos las relaciones de revisiones que existen en este mes del Año Tarifario
            this.servicioPrimasRER.DeleteAllRerParametroRevisionByRerpprcodi(Rerpprcodi);

            model.ParametroRER = this.servicioPrimasRER.GetByIdRerParametroPrima(Rerpprcodi);
            model.AnioVersion = this.servicioPrimasRER.GetByIdRerAnioVersion(model.ParametroRER.Reravcodi);
            int anio = model.AnioVersion.Reravaniotarif;
            if (model.ParametroRER.Rerpprmes < 5)
            {
                anio += 1;
            }

            DateTime fechaActual = DateTime.Now;
            string usuarioActual = User.Identity.Name;

            // Obtengo los registros de la tabla VTEA
            model.ListaRecalculo = this.servicioRecalculo.ListVTEAByAnioMes(anio, model.ParametroRER.Rerpprmes);
            model.ListaRevisionVTEA = new List<RerParametroRevisionDTO>();
            foreach (var itemVTEA in model.ListaRecalculo)
            {
                RerParametroRevisionDTO RevisionVTEA = new RerParametroRevisionDTO
                {
                    Rerpprcodi = Rerpprcodi,
                    Perinombre = itemVTEA.PeriNombre,
                    Recanombre = itemVTEA.RecaNombre,
                    Pericodi = itemVTEA.RecaPeriCodi,
                    Recacodi = itemVTEA.RecaCodi,
                    Rerpretipo = "VTEA",
                    Rerprefeccreacion = fechaActual,
                    Rerpreusucreacion = usuarioActual
                };
                this.servicioPrimasRER.SaveRerParametroRevision(RevisionVTEA);
                model.ListaRevisionVTEA.Add(RevisionVTEA);
            }

            // Obtengo los registros de la tabla VTP
            model.ListaRecalculoPotencia = this.servicioTransfPotencia.ListVTP(anio, model.ParametroRER.Rerpprmes);     // Tabla VTP
            model.ListaRevisionVTP = new List<RerParametroRevisionDTO>();
            foreach (var itemVTP in model.ListaRecalculoPotencia)
            {
                RerParametroRevisionDTO RevisionVTP = new RerParametroRevisionDTO
                {
                    Rerpprcodi = Rerpprcodi,
                    Perinombre = itemVTP.Perinombre,
                    Recanombre = itemVTP.Recpotnombre,
                    Pericodi = itemVTP.Pericodi,
                    Recacodi = itemVTP.Recpotcodi,
                    Rerpretipo = "VTP",
                    Rerprefeccreacion = fechaActual,
                    Rerpreusucreacion = usuarioActual
                };
                this.servicioPrimasRER.SaveRerParametroRevision(RevisionVTP);
                model.ListaRevisionVTP.Add(RevisionVTP);
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Permite eliminar una revision de un mes de un Anio Tarifario
        /// </summary>
        /// <param name="id">Id del parametro revision relacionado a un mes de un Anio Tarifario</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public string Delete(int id = 0)
        {
            try
            {
                this.servicioPrimasRER.DeleteRerParametroRevision(id);
                return "true";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        /// <summary>
        /// Copiar el "tipo de cambio", "origen", "revisión" y relaciones con CentralRER de una verión anterior a la actual en un año tarifario
        /// </summary>
        /// <param name="Reravcodi">Id del Anio Tarifario</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult copiarVersionAnterior(int Reravcodi )
        {
            try
            {
                RerAnioVersionDTO AnioVersionActual = this.servicioPrimasRER.GetByIdRerAnioVersion(Reravcodi);

                #region Valido que la version no sea Anual
                if (AnioVersionActual.Reravversion == "0")
                {
                    return Json(-2);
                }
                #endregion

                string versionAnterior = "-1";
                if (int.TryParse(AnioVersionActual.Reravversion, out int numero))
                {
                    versionAnterior = (numero > 0) ? (numero - 1).ToString() : "0";
                }
                else {
                    return Json(-1);
                }

                RerAnioVersionDTO AnioVersionAnterior = this.servicioPrimasRER.ListRerAnioVersiones().Where(item => (item.Reravaniotarif == AnioVersionActual.Reravaniotarif) && (item.Reravversion == versionAnterior)).ToList().First();

                List<RerParametroPrimaDTO> ListParametroActual = this.servicioPrimasRER.ListRerParametroPrimas().Where(item => item.Reravcodi == AnioVersionActual.Reravcodi).ToList();
                List<RerParametroPrimaDTO> ListParametroAnterior = this.servicioPrimasRER.ListRerParametroPrimas().Where(item => item.Reravcodi == AnioVersionAnterior.Reravcodi).ToList();

                DateTime fechaActual = DateTime.Now;
                string usuarioActual = User.Identity.Name;

                #region Copiamos el tipo de cambio, origen y revision
                int i = 0;
                foreach (var ParametroActual in ListParametroActual)
                {
                    ParametroActual.Rerpprtipocambio = ListParametroAnterior[i].Rerpprtipocambio;
                    ParametroActual.Rerpprorigen = ListParametroAnterior[i].Rerpprorigen;
                    ParametroActual.Rerpprrevision = ListParametroAnterior[i].Rerpprrevision;
                    ParametroActual.Recacodi = ListParametroAnterior[i].Recacodi;
                    ParametroActual.Pericodi = ListParametroAnterior[i].Pericodi;
                    ParametroActual.Rerpprfecmodificacion = fechaActual;
                    ParametroActual.Rerpprusumodificacion = usuarioActual;
                    this.servicioPrimasRER.UpdateRerParametroPrima(ParametroActual);
                    i++;
                }
                #endregion

                #region Copiamos las relaciones con las revisiones con el mes del Anio Tarifario
                
                i = 0;
                foreach (var ParametroActual in ListParametroActual)
                {
                    this.servicioPrimasRER.DeleteAllRerParametroRevisionByRerpprcodi(ParametroActual.Rerpprcodi);
                    List<RerParametroRevisionDTO> ParametroRevisionesAnterior = this.servicioPrimasRER.ListRerParametroRevisiones().Where(item => item.Rerpprcodi == ListParametroAnterior[i].Rerpprcodi).ToList();
                    foreach (var ParametroRevisionAnterior in ParametroRevisionesAnterior)
                    {
                        ParametroRevisionAnterior.Rerpprcodi = ParametroActual.Rerpprcodi;
                        ParametroRevisionAnterior.Rerprefeccreacion = fechaActual;
                        ParametroRevisionAnterior.Rerpreusucreacion = usuarioActual;
                        this.servicioPrimasRER.SaveRerParametroRevision(ParametroRevisionAnterior);
                    }
                    i++;
                }
                #endregion

                #region Copiamos las relacions de codigos de retiro y central RER con los meses del Anio Tarifario
                i = 0;
                foreach (var ParametroActual in ListParametroActual)
                {
                    this.servicioPrimasRER.DeleteAllRerCentralCodRetiroByRerpprcodiRercencodi(ParametroActual.Rerpprcodi, -2);
                    List<RerCentralCodRetiroDTO> RelacionesCodRetiroAnterior = this.servicioPrimasRER.ListRerCentralCodRetiros().Where(item => item.Rerpprcodi == ListParametroAnterior[i].Rerpprcodi).ToList();
                    foreach (var RelacionCodRetiroAnterior in RelacionesCodRetiroAnterior)
                    {
                        RelacionCodRetiroAnterior.Rerpprcodi = ParametroActual.Rerpprcodi;
                        RelacionCodRetiroAnterior.Rerccrfeccreacion = fechaActual;
                        RelacionCodRetiroAnterior.Rerccrusucreacion = usuarioActual;
                        this.servicioPrimasRER.SaveRerCentralCodRetiro(RelacionCodRetiroAnterior);
                    }
                    i++;
                }
                #endregion

                return Json(1);
                
            }
            catch (Exception)
            {
                return Json(-1);
            }
        }
    }
}