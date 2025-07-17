//using COES.Dominio.DTO.Sic;
//using COES.Servicios.Aplicacion.Helper;
//using COES.MVC.Intranet.SeguridadServicio;
//using log4net;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Web;
//using System.Web.Mvc;
//using System.Web.Security;
//using COES.MVC.Intranet.Helper;
//using System.Configuration;
//using COES.MVC.Intranet.Areas.PMPO.Models;
//using OfficeOpenXml;
//using System.IO;
//using System.Web.Script.Serialization;
//using COES.Servicios.Aplicacion.PMPO;
//using COES.MVC.Intranet.Areas.PMPO.Helper;
//using Newtonsoft.Json;
//using System.Globalization;
//using COES.Servicios.Aplicacion.PMPO.Helper;

//namespace COES.MVC.Intranet.Areas.PMPO.Controllers
//{
//    /// <summary>
//    /// Clase controladora de las obras
//    /// </summary>
//    public class ObrasController : BaseController
//    {
//        #region  [ Fields ]

//        /// <summary>
//        /// Declaración y creación del servicio de seguridad.
//        /// </summary>
//        SeguridadServicioClient servicio = new SeguridadServicioClient();

//        /// <summary>
//        /// Declaración y creación del servicio de aplicación.
//        /// </summary>
//        ProgramacionAppServicio pmpo = new ProgramacionAppServicio();

//        /// <summary>
//        /// Declaración y creación del objeto de trazabilidad.
//        /// </summary>
//        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

//        #endregion

//        #region [ Constructors ]

//        #region ObrasController

//        /// <summary>
//        /// Constructor de la clase
//        /// </summary>
//        public ObrasController()
//        {
//            log4net.Config.XmlConfigurator.Configure();
//        }

//        #endregion

//        #endregion

//        #region OnException

//        /// <summary>
//        /// Sobrecarga el método para la trazabilidad
//        /// </summary>
//        /// <param name="filterContext"></param>
//        protected override void OnException(ExceptionContext filterContext)
//        {
//            try
//            {
//                log4net.Config.XmlConfigurator.Configure();
//                Exception objErr = filterContext.Exception;
//                Log.Error("Error", objErr);
//            }
//            catch (Exception ex)
//            {
//                Log.Fatal("Error", ex);
//                throw;
//            }
//        }
//        #endregion

//        #region ValidateUser

//        /// <summary>
//        /// Valida Session true
//        /// </summary>
//        public void ValidateUser()
//        {
//            HttpCookie cookie = Request.Cookies[DatosSesion.InicioSesion];

//            if (cookie != null)
//            {
//                if (cookie[DatosSesion.SesionUsuario] != null)
//                {
//                    Session[DatosSesion.SesionMapa] = Constantes.NodoPrincipal;
//                    Session[DatosSesion.SesionUsuario] = this.servicio.ObtenerUsuarioPorLogin(cookie[DatosSesion.SesionUsuario].ToString());
//                    FormsAuthentication.SetAuthCookie(cookie[DatosSesion.SesionUsuario].ToString(), false);

//                }
//            }
//            else
//            {
//                if (Session[DatosSesion.SesionUsuario] != null)
//                {
//                    Session[DatosSesion.SesionMapa] = Constantes.NodoPrincipal;
//                }
//                else
//                {
//                    Response.Redirect("~/");
//                }
//            }
//        }

//        #endregion

//        #region Index

//        /// <summary>
//        /// Muestra Ventana de Configuración de Parametro
//        /// </summary>
//        public ActionResult Index()
//        {
//             IList<ListItem> companies = this.FetchCompanies();
//            ViewData["cboEmpresa"] = new SelectList(companies, "ID", "Text", 0);

//            List<PmpoTipoObraDTO> obraTypes = pmpo.ListTipoObra();
//            obraTypes.Insert(0, new PmpoTipoObraDTO() { TObracodi = 0, TObradescripcion = "[SELECCIONE]" });
//            ViewData["cboTipoObra"] = new SelectList(obraTypes, "TObracodi", "TObradescripcion", 0);

//            DateTime FechaPlazo = pmpo.GetPlazo(1);
//            //DateTime FechaPlazo = new DateTime(fecha.Year, fecha.Month, Convert.ToInt32(Formatdiafinplazo), 0, 0, 0);

//            ReportSubmissionModel model = new Models.ReportSubmissionModel();
//            model.DeadlineForSubmission = FechaPlazo;
//            model.DateForSubmission = DateTime.ParseExact(String.Format("{0:dd/MM/yyyy HH:mm}", FechaPlazo), "dd/MM/yyyy HH:mm", DateTimeFormatInfo.InvariantInfo);
//            model.Period.Start = FechaPlazo;
//            model.Period.End = FechaPlazo.AddMonths(11);

//            return View(model);
//        }

//        #endregion

//        #region FetchObrasJsonResult

//        /// <summary>
//        /// Expone el método para traer obras
//        /// </summary>
//        public JsonDataContractActionResult FetchObrasJsonResult(FormCollection collection)
//        {
//            IList<ObraListItemModel> obras;
//            IEnumerable<PmpoObraDTO> arrObras;
//            string idEmpresa = collection["idEmpresa"];
//            string idTipoObra = collection["idTipoObra"];
//            string formatList = string.Join(",", ConstantesPMPO.ListadoFormatcodiPMPO);
//            string indFechaHist = collection["indFechaHist"];
//            DateTime fechaIni = DateTime.Now;
//            DateTime fechaFin = DateTime.Now;

//            if (indFechaHist.ToUpper().Equals("S"))
//            {
//                DateTime.TryParse(collection["fechaIni"], new CultureInfo("es-ES"), DateTimeStyles.None, out fechaIni);
//                DateTime.TryParse(collection["fechaFin"], new CultureInfo("es-ES"), DateTimeStyles.None, out fechaFin);
//                arrObras = pmpo.ListarObra(idEmpresa, Convert.ToInt32(idTipoObra), fechaIni, fechaFin, formatList);
//            }
//            else
//            {
//                DateTime.TryParse(collection["fechaPer"], new CultureInfo("es-ES"), DateTimeStyles.None, out fechaIni);
//                arrObras = pmpo.ListarObra(idEmpresa, Convert.ToInt32(idTipoObra), fechaIni, fechaFin, formatList);
//            }


            
            


//            if (arrObras.Any())
//            {
//                obras = new ObraListItemModel[arrObras.Count()];

//                int obraIndex = 0;
//                foreach (PmpoObraDTO obraDTO in arrObras)
//                {
//                    obras[obraIndex] = new ObraListItemModel()
//                    {
//                        Id = obraDTO.Obracodi,
//                        IdDet = obraDTO.ObraDetcodi,
//                        CompanyName = obraDTO.Emprnomb,
//                        ObraTypeId = obraDTO.TObracodi,
//                        Equinomb = obraDTO.Equinomb,
//                        Gruponomb = obraDTO.GrupoNomb,
//                        Barranomb = obraDTO.BarraNomb,
//                        ObraTypeName = obraDTO.TObradescripcion,
//                        PlannedDate = obraDTO.Obrafechaplanificada.Value.ToString(Constantes.FormatoFecha),
//                        Notes = obraDTO.Obradescripcion,
//                        ObraEnFormato = (obraDTO.ObraFlagFormat == 1 ? "SI":"NO")
//                    };

//                    obraIndex++;
//                }
//            }
//            else
//            {
//                obras = new ObraListItemModel[0];
//            }

//            return Json(obras);
//        }

//        #endregion

//        #region FetchJsonResult

//        /// <summary>
//        /// Expone el método para traer los datos de la obra
//        /// </summary>
//        [HttpPost]

//        public JsonDataContractActionResult FetchJsonResult(int obraId, int tObraId)
//        {
//            Models.ObraModel obra;


//            PmpoObraDTO obraDTO = pmpo.GetByIdPmpoObra(obraId, tObraId);

//            List<PmpoObraDetalleDTO> obraDetDTO = pmpo.GetByCriteriaPmpoObraDet(obraId, tObraId, obraDTO.Emprcodi);

//            obra = new Models.ObraModel()
//            {
//                Id = obraDTO.Obracodi,
//                CompanyId = obraDTO.Emprcodi,
//                ObraTypeId = obraDTO.TObracodi,
//                PlannedDate = obraDTO.Obrafechaplanificada.Value.ToString(Constantes.FormatoFecha),
//                Notes = obraDTO.Obradescripcion,
//                ObraFlagFormat = obraDTO.ObraFlagFormat
//            };


//            if (obraDetDTO != null && obraDetDTO.Count > 0)
//            {
//                obra.Details = new Models.ObraDetailModel[obraDetDTO.Count()];

//                int obraDetailIndex = 0;

//                foreach (PmpoObraDetalleDTO obraDetailDTO in obraDetDTO)
//                {
//                    obra.Details[obraDetailIndex] = new ObraDetailModel()
//                    {
//                        BarId = obraDetailDTO.Barrcodi,
//                        GroupId = obraDetailDTO.Grupocodi,
//                        TeamId = obraDetailDTO.Equicodi,
//                        Description = obraDetailDTO.Obradetdescripcion,
//                        Obradetcodi = obraDetailDTO.Obradetcodi
//                    };

//                    obraDetailIndex++;
//                }
//            }

//                return Json(obra);
//        }

//        #endregion

//        #region Edit

//        /// <summary>
//        /// Vista de edición de  la obra
//        /// </summary>
//        /// <param name="obraId"></param>
//        /// <param name="obraTypeId"></param>
//        /// <returns></returns>
//        public ActionResult Edit(int? obraId, int? obraTypeId)
//        {
//            ViewData["obraId"] = obraId;
//            ViewData["obraTypeId"] = obraTypeId;

//            return View();
//        }

//        #endregion

//        #region FetchCompaniesJsonResult

//        /// <summary>
//        /// Expone el método para traer las empresas
//        /// </summary>
//        /// <returns></returns>
//        [HttpPost]
//        public JsonDataContractActionResult FetchCompaniesJsonResult()
//        {
//            IEnumerable<ListItem> arrItems = this.FetchCompanies();

//            return Json(arrItems);
//        }

//        #endregion

//        #region FetchBarsJsonResult

//        /// <summary>
//        /// Expone el método para traer las barras
//        /// </summary>
//        /// <param name="companyId"></param>
//        /// <returns></returns>
//        [HttpPost]
//        public JsonDataContractActionResult FetchBarsJsonResult(int companyId)
//        {
//            IEnumerable<ListItem> arrItems = this.FetchBars(companyId);

//            return Json(arrItems);
//        }

//        #endregion

//        #region FetchTeamsJsonResult

//        /// <summary>
//        /// Expone el método para traer los equipos
//        /// </summary>
//        /// <param name="companyId"></param>
//        /// <returns></returns>
//        [HttpPost]
//        public JsonDataContractActionResult FetchTeamsJsonResult(int companyId)
//        {
//            IEnumerable<ListItem> arrItems = this.FetchTeams(companyId);

//            return Json(arrItems);
//        }

//        #endregion

//        #region FetchGroupsJsonResult

//        /// <summary>
//        /// Expone el método para traer los grupos
//        /// </summary>
//        /// <param name="companyId"></param>
//        /// <returns></returns>
//        [HttpPost]
//        public JsonDataContractActionResult FetchGroupsJsonResult(int companyId)
//        {
//            IEnumerable<ListItem> arrItems = this.FetchGroups(companyId);

//            return Json(arrItems);
//        }

//        #endregion

//        #region FetchObraTypesJsonResult

//        /// <summary>
//        /// Expon el método para traer los tipos de obra
//        /// </summary>
//        /// <returns></returns>
//        [HttpPost]
//        public JsonDataContractActionResult FetchObraTypesJsonResult()
//        {
//            IEnumerable<ListItem> arrItems = this.FetchObraTypes();

//            return Json(arrItems);
//        }

//        #endregion

//        //#region FetchObraFlagformatJsonResult

//        ///// <summary>
//        ///// Expon el método para traer los tipos de obra
//        ///// </summary>
//        ///// <returns></returns>
//        //[HttpPost]
//        //public JsonDataContractActionResult FetchObraFlagformatJsonResult()
//        //{
//        //    IEnumerable<ListItem> arrItems = this.FetchObraFlagformat();

//        //    return Json(arrItems);
//        //}

//        //#endregion

//        #region SaveObraJsonResult

//        /// <summary>
//        /// Expone el método para registrar la obra
//        /// </summary>
//        /// <param name="value"></param>
//        /// <returns></returns>
//        [HttpPost]
//        public JsonDataContractActionResult SaveObraJsonResult(string value)
//        {
//            JavaScriptSerializer serializer = new JavaScriptSerializer();

//            ObraModel obra = JsonConvert.DeserializeObject<ObraModel>(value);
//            string user = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin;

//            PmpoObraDTO obraDTO = new PmpoObraDTO()
//            {
//                Emprcodi = obra.CompanyId,
//                TObracodi = obra.ObraTypeId,
//                Obrafechaplanificada = DateTime.ParseExact(obra.PlannedDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture),
//                Obradescripcion = obra.Notes,
//                ObraFlagFormat = obra.Formato,
//                Obrausucreacion = user
//            };

//            obraDTO.ObraDet = new List<PmpoObraDetalleDTO>();

//            if (obra.Details != null && obra.Details.Any())
//            {
//                foreach (ObraDetailModel obraDetail in obra.Details)
//                {
//                    obraDTO.ObraDet.Add(new PmpoObraDetalleDTO()
//                    {
//                        Obradetcodi = obraDetail.Obradetcodi,
//                        Barrcodi = (obraDetail.BarId == null ? -1 : obraDetail.BarId.Value),
//                        Grupocodi = (obraDetail.GroupId == null ? -1 : obraDetail.GroupId.Value),
//                        Equicodi = (obraDetail.TeamId == null ? -1 : obraDetail.TeamId.Value),
//                        Obradetusucreacion = user,
//                        Obradetfeccreacion = DateTime.Now,
//                        Obradetdescripcion = obraDetail.Description
//                    });
//                }
//            }

//            if (obra.Id != null)
//            {
//                obraDTO.Obracodi = obra.Id.Value;
//                obraDTO.Obrafecmodificacion = DateTime.Now;

//                //Actualiza Detalle Obra
//                for (int i = 0; i < obraDTO.ObraDet.Count; i++)
//                {
//                    pmpo.UpdatePmpoObraDet(obraDTO.ObraDet[i]);
//                }
//                //Actualiza Obra
//                pmpo.UpdatePmpoObra(obraDTO);
//            }
//            else
//            {
//                obraDTO.Obrafeccreacion = DateTime.Now;

//                int obracodi= pmpo.SavePmpoObra(obraDTO);

//                //Grabar Detalle
//                pmpo.SavePmpoObraDetalle(obraDTO.ObraDet, obracodi);
//            }

//            return Json("OK");
//        }

//        #endregion

//        #region DeleteObraJsonResult

//        /// <summary>
//        /// Expone el método para eliminar la obra
//        /// </summary>
//        /// <param name="obraId"></param>
//        /// <param name="obraTypeId"></param>
//        /// <returns></returns>
//        [HttpPost]
//        public JsonDataContractActionResult DeleteObraJsonResult(int obraId, int obraTypeId)
//        {
//            JavaScriptSerializer serializer = new JavaScriptSerializer();

//            string user = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin;

//            // Aqui la llamada al servicio para eliminar la obra
//            pmpo.DeletePmpoObra(obraId);

//            return Json("OK");
//        }

//        #endregion

//        #region FetchCompanies

//        /// <summary>
//        /// Traer la lista de empresas
//        /// </summary>
//        /// <returns></returns>
//        private IList<ListItem> FetchCompanies()
//        {

//            List<ListItem> lista;
//            List<COES.Dominio.DTO.Sic.MeFormatoDTO> list = new List<MeFormatoDTO>();

//            if (list != null && list.Count() > 0)
//            {
//                lista = new List<Helper.ListItem>();

//                foreach (COES.Dominio.DTO.Sic.MeFormatoDTO company in list)
//                {
//                    lista.Add(new ListItem()
//                    {
//                        ID = company.Emprcodi,
//                        Text = company.Emprnomb
//                    });
//                }
//            }
//            else
//            {
//                lista = null;
//            }

//            return lista;
//        }

//        #endregion

//        #region FetchObraTypes

//        /// <summary>
//        /// Traer la lista de tipos de obra
//        /// </summary>
//        /// <returns></returns>
//        private IEnumerable<ListItem> FetchObraTypes()
//        {

//            List<ListItem> arrItems = new List<ListItem>();
//            List<PmpoTipoObraDTO> arrPmpoObraTypes = pmpo.ListTipoObra();

//            if (arrPmpoObraTypes != null && arrPmpoObraTypes.Count > 0)
//            {
//                foreach (var item in arrPmpoObraTypes)
//                {
//                    arrItems.Add(new ListItem()
//                    {
//                        ID = item.TObracodi,
//                        Text = item.TObradescripcion
//                    });
//                }

//            }
//            else
//            {
//                arrItems = null;
//            }


//            return arrItems.ToArray();
//        }

//        #endregion

//        //#region FetchObraFlagformat

//        ///// <summary>
//        ///// Traer la lista de tipos de Flagformat
//        ///// </summary>
//        ///// <returns></returns>
//        //private IEnumerable<ListItem> FetchObraFlagformat()
//        //{

//        //    List<ListItem> arrItems = new List<ListItem>();
//        //    List<PmpoObraDTO> arrPmpoFlagFormat = pmpo.ListFlagFormat();

//        //    if (arrPmpoFlagFormat != null && arrPmpoFlagFormat.Count > 0)
//        //    {
//        //        foreach (var item in arrPmpoFlagFormat)
//        //        {
//        //            arrItems.Add(new ListItem()
//        //            {
//        //                ID = item.ObraFlagFormat,
//        //                Text = ((item.ObraFlagFormat == 1)? "SI" :"NO")
//        //            });
//        //        }

//        //    }
//        //    else
//        //    {
//        //        arrItems = null;
//        //    }


//        //    return arrItems.ToArray();
//        //}

//        //#endregion

//        #region FetchGroups

//        /// <summary>
//        /// Trae la lista de grupos
//        /// </summary>
//        /// <param name="companyId"></param>
//        /// <returns></returns>
//        private IEnumerable<ListItem> FetchGroups(int companyId)
//        {
//            List<ListItem> arrItems;
//            List<PmpoObraDetalleDTO> arrPmpoGrupos = pmpo.GetGrupo(companyId);

//            if (arrPmpoGrupos != null && arrPmpoGrupos.Count > 0)
//            {
//                arrItems = new List<Helper.ListItem>();
//                foreach (PmpoObraDetalleDTO pmpoGrupo in arrPmpoGrupos)
//                {
//                    arrItems.Add(new ListItem()
//                    {
//                        ID = pmpoGrupo.Grupocodi,
//                        Text = (pmpoGrupo.Gruponombre==null ) ? " " : pmpoGrupo.Gruponombre
//                    });
//                }
//            }
//            else
//            {
//                arrItems = null;
//            }

//            return arrItems;
//        }

//        #endregion

//        #region FetchBars

//        /// <summary>
//        /// Trae la lista de barras
//        /// </summary>
//        /// <param name="companyId"></param>
//        /// <returns></returns>
//        private IEnumerable<ListItem> FetchBars(int companyId)
//        {
//            List<ListItem> arrItems;
//            List<PmpoObraDetalleDTO> arrPmpoBars = pmpo.GetBarra(companyId);

//            if (arrPmpoBars != null && arrPmpoBars.Count > 0)
//            {
//                arrItems = new List<Helper.ListItem>();
//                foreach (PmpoObraDetalleDTO pmpoBar in arrPmpoBars)
//                {
//                    arrItems.Add(new ListItem()
//                    {
//                        ID = pmpoBar.Barrcodi,
//                        Text = (pmpoBar.Barrnombre==null) ? " ": pmpoBar.Barrnombre
//                    });
//                }
//            }
//            else
//            {
//                arrItems = null;
//            }

//            return arrItems;
//        }

//        #endregion

//        #region FetchTeams

//        /// <summary>
//        /// Traer la lista de equipos
//        /// </summary>
//        /// <param name="companyId"></param>
//        /// <returns></returns>
//        private IEnumerable<ListItem> FetchTeams(int companyId)
//        {
//            List<ListItem> arrItems;
//            List<PmpoObraDetalleDTO> arrPmpoTeams = pmpo.GetEquipo(companyId);


//            if (arrPmpoTeams != null && arrPmpoTeams.Count > 0)
//            {
//                arrItems = new List<Helper.ListItem>();
//                foreach (PmpoObraDetalleDTO pmpoTeam in arrPmpoTeams)
//                {
//                    arrItems.Add(new ListItem()
//                    {
//                        ID = pmpoTeam.Equicodi,
//                        Text = (pmpoTeam.Equinomb == null)? " " : pmpoTeam.Equinomb
//                    });
//                }
//            }
//            else
//            {
//                arrItems = null;
//            }

//            return arrItems;
//        }

//        #endregion
		
//		#region GetDiffPeriodDate

//        public JsonDataContractActionResult GetDiffPeriodDate(int year, int month)
//        {
//            int diff = 0;
//            DateTime fechaSeleccionada;
//            DateTime.TryParse("01/" + month.ToString() + "/" + year.ToString(), new CultureInfo("es-ES"), DateTimeStyles.None, out fechaSeleccionada);

//            ReportSubmissionModel model = new Models.ReportSubmissionModel();

//            DateTime FechaPlazo = pmpo.GetPlazo(1);
//            model.DeadlineForSubmission = FechaPlazo;

//            if ((fechaSeleccionada.Year == FechaPlazo.Year) && (fechaSeleccionada.Month == FechaPlazo.Month))
//                diff = 1;

//            return Json(diff);
//        }
//        #endregion
//    }
//}
