//using COES.Dominio.DTO.Sic;
//using COES.Servicios.Aplicacion.Helper;
//using COES.MVC.Extranet.SeguridadServicio;
//using log4net;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Web;
//using System.Web.Mvc;
//using System.Web.Security;
//using COES.MVC.Extranet.Helper;
//using System.Configuration;
//using COES.MVC.Extranet.Areas.PMPO.Models;
//using OfficeOpenXml;
//using System.IO;
//using System.Web.Script.Serialization;
//using COES.Servicios.Aplicacion.PMPO;
//using COES.MVC.Extranet.Areas.PMPO.Helper;
//using Newtonsoft.Json;
//using System.Globalization;
//using COES.Servicios.Aplicacion.PMPO.Helper;

//namespace COES.MVC.Extranet.Areas.PMPO.Controllers
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

//        #region Index

//        /// <summary>
//        /// Muestra Ventana de Configuración de Parametro
//        /// </summary>
//        public ActionResult Index()
//        {

//            ReportSubmissionModel model = new Models.ReportSubmissionModel();
//            DateTime FechaPlazo = pmpo.GetPlazo(1);
//            model.DeadlineForSubmission = FechaPlazo;
//            model.DateForSubmission = DateTime.ParseExact(String.Format("{0:dd/MM/yyyy HH:mm}", FechaPlazo), "dd/MM/yyyy HH:mm", DateTimeFormatInfo.InvariantInfo);
//            model.Period.Start = FechaPlazo;
//            model.Period.End = FechaPlazo.AddMonths(11);

//            IList<ListItem> companies = this.FetchCompanies();
//            ViewData["cboEmpresa"] = new SelectList(companies, "ID", "Text", 0);

//            List<PmpoTipoObraDTO> obraTypes = pmpo.ListTipoObra();
//            obraTypes.Insert(0, new PmpoTipoObraDTO() { TObracodi = 0, TObradescripcion = "[SELECCIONE]" });
//            ViewData["cboTipoObra"] = new SelectList(obraTypes, "TObracodi", "TObradescripcion", 0);

//            return View(model);
//        }

//        #endregion

//        #region FetchObrasJsonResult

//        /// <summary>
//        /// Muestra Ventana de Configuración de Parametro
//        /// </summary>
//        public JsonDataContractActionResult FetchObrasJsonResult(FormCollection collection)
//        {
//            string user = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin;
//            int userEmprcodi = (int)servicio.ObtenerUsuarioPorLogin(user).EmprCodi;
//            DateTime fechaIni = Convert.ToDateTime(collection["fechaIni"]),
//            fechaFin = Convert.ToDateTime(collection["fechaFin"]);
//            string idEmpresa = collection["idEmpresa"];
//            string idTipoObra = collection["idTipoObra"];
//            string formatList = string.Join(",", ConstantesPMPO.ListadoFormatcodiPMPO);

//            IList<ObraListItemModel> obras;
//            IEnumerable<PmpoObraDTO> arrObras = pmpo.ListarObra(userEmprcodi.ToString(), Convert.ToInt32(idTipoObra), fechaIni, fechaFin, formatList);

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
//                        ObraEnFormato = (obraDTO.ObraFlagFormat == 1 ? "SI" : "NO")
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
//        /// Muestra Ventana de Configuración de Parametro
//        /// </summary>
//        [HttpPost]
//        public JsonDataContractActionResult FetchJsonResult(int obraId, int tObraId)
//        {
//            Models.ObraModel obra;

//            PmpoObraDTO obraDTO = pmpo.GetByIdPmpoObra(obraId, tObraId);

//            List<PmpoObraDetalleDTO> obraDetDTO = pmpo.GetByCriteriaPmpoObraDet(obraId, tObraId, obraDTO.Emprcodi);

//            obra = new Models.ObraModel()
//            {
//                CompanyName = obraDTO.Emprnomb,
//                Id = obraDTO.Obracodi,
//                CompanyId = obraDTO.Emprcodi,
//                ObraTypeId = obraDTO.TObracodi,
//                PlannedDate = obraDTO.Obrafechaplanificada.Value.ToString(Constantes.FormatoFecha),
//                Notes = obraDTO.Obradescripcion
//            };


//            if (obraDetDTO != null && obraDetDTO.Count>0)
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
//                        Description = obraDetailDTO.Obradetdescripcion
//                    };

//                    obraDetailIndex++;
//                }
//            }

//            return Json(obra);
//        }

//        #endregion

//        #region Edit

//        /// <summary>
//        /// Vista de edición
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

//        #region FetchBarsJsonResult

//        /// <summary>
//        /// Expone la lista de Barras
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
//        /// Expone la lista de equipos
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
//        /// Expone la lista de grupos
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
//        /// Expone la lista de los tipos de obra
//        /// </summary>
//        /// <returns></returns>
//        [HttpPost]
//        public JsonDataContractActionResult FetchObraTypesJsonResult()
//        {
//            IEnumerable<ListItem> arrItems = this.FetchObraTypes();

//            return Json(arrItems);
//        }

//        #endregion

//        #region SaveObraJsonResult

//        /// <summary>
//        /// Graba la obra
//        /// </summary>
//        /// <param name="value"></param>
//        /// <returns></returns>
//        [HttpPost]
//        public JsonDataContractActionResult SaveObraJsonResult(string value)
//        {
//            JavaScriptSerializer serializer = new JavaScriptSerializer();

//            ObraModel obra = JsonConvert.DeserializeObject<ObraModel>(value);

//            PmpoObraDTO obraDTO = new PmpoObraDTO()
//            {
//                TObracodi = obra.ObraTypeId,
//                Obrafechaplanificada = DateTime.ParseExact(obra.PlannedDate, Constantes.FormatoFecha, System.Globalization.CultureInfo.InvariantCulture),
//                Obradescripcion = obra.Notes,
//                Obrausucreacion = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin
//        };

//            obraDTO.ObraDet = new List<PmpoObraDetalleDTO>();

//            if (obra.Details != null && obra.Details.Any())
//            {
//                foreach (ObraDetailModel obraDetail in obra.Details)
//                {
//                    obraDTO.ObraDet.Add(new PmpoObraDetalleDTO()
//                    {
//                        Barrcodi = (obraDetail.BarId == null ? -1 : obraDetail.BarId.Value),
//                        Grupocodi = (obraDetail.GroupId == null ? -1 : obraDetail.GroupId.Value),
//                        Equicodi = (obraDetail.TeamId == null ? -1 : obraDetail.TeamId.Value),
//                        Obradetdescripcion = obraDetail.Description
//                    });
//                }
//            }

//            if (obra.Id != null)
//            {
//                obraDTO.Obracodi = obra.Id.Value;
//                obraDTO.Obrafecmodificacion = DateTime.Now;

//                pmpo.UpdatePmpoObra(obraDTO);
//            }
//            else
//            {
//                obraDTO.Obrafeccreacion = DateTime.Now;

//                pmpo.SavePmpoObra(obraDTO);
//            }

//            return Json("OK");
//        }

//        #endregion

//        #region FetchCompanies

//        /// <summary>
//        /// Lista el arreglo de empresas
//        /// </summary>
//        /// <returns></returns>
//        private IList<ListItem> FetchCompanies()
//        {

//            string user = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin;
//            int userEmprcodi = (int)servicio.ObtenerUsuarioPorLogin(user).EmprCodi;
//            List<ListItem> lista;
//            List<COES.Dominio.DTO.Sic.MeFormatoDTO> list = pmpo.GetListaEmpresasPMPO();

//            if (list != null && list.Count() > 0)
//            {
//                lista = new List<Helper.ListItem>();
//                lista.Add(new ListItem() { ID = 0, Text = "(TODOS)" });


//                foreach (COES.Dominio.DTO.Sic.MeFormatoDTO company in list)
//                {
//                    if(company.Emprcodi == userEmprcodi)
//                        lista.Add(new ListItem()
//                        {
//                            ID = company.Emprcodi,
//                            Text = company.Emprnomb
//                        });
//                }
//            }
//            else
//            {
//                lista = new List<Helper.ListItem>();
//                lista.Add(new ListItem() { ID = 0, Text = "(NINGUNO)" });
//            }

//            return lista;
//        }

//        #endregion

//        #region FetchObraTypes

//        /// <summary>
//        /// Lista el arreglo de Tipos de Obra
//        /// </summary>
//        /// <returns></returns>
//        private IEnumerable<ListItem> FetchObraTypes()
//        {
//            ListItem[] arrItems;
//            List<PmpoTipoObraDTO> arrPmpoObraTypes =  pmpo.ListTipoObra();

//            if (arrPmpoObraTypes != null && arrPmpoObraTypes.Count > 0)
//            {
//                arrItems = new ListItem[arrPmpoObraTypes.Count];
//                int i = 0;
//                foreach (PmpoTipoObraDTO pmpoObraType in arrPmpoObraTypes)
//                {
//                    arrItems[i] = new ListItem()
//                    {
//                        ID = pmpoObraType.TObracodi,
//                        Text = pmpoObraType.TObradescripcion
//                    };

//                    i++;
//                }
//            }
//            else
//            {
//                arrItems = null;
//            }

//            return arrItems;
//        }

//        #endregion

//        #region FetchGroups

//        /// <summary>
//        /// Lista el arreglo de Grupos
//        /// </summary>
//        /// <param name="companyId"></param>
//        /// <returns></returns>
//        private IEnumerable<ListItem> FetchGroups(int companyId)
//        {
//            List<ListItem> arrItems;
//            List<PmpoObraDetalleDTO> arrPmpoGrupos = pmpo.GetGrupo(companyId); //TEMPORAL

//            if (arrPmpoGrupos != null && arrPmpoGrupos.Count > 0)
//            {
//                arrItems = new List<Helper.ListItem>();
//                foreach (PmpoObraDetalleDTO pmpoGrupo in arrPmpoGrupos)
//                {
//                    arrItems.Add(new ListItem()
//                    {
//                        ID = pmpoGrupo.Grupocodi,
//                        Text = pmpoGrupo.Gruponombre
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
//        /// Lista el arreglo de barras.
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
//                        Text = pmpoBar.Barrnombre
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
//        /// Lista el arreglo de equipos
//        /// </summary>
//        /// <param name="companyId"></param>
//        /// <returns></returns>
//        private IEnumerable<ListItem> FetchTeams(int companyId)
//        {
//            List<ListItem> arrItems;
//            List<PmpoObraDetalleDTO> arrPmpoTeams = pmpo.GetEquipo(companyId); //TEMPORAL


//            if (arrPmpoTeams != null && arrPmpoTeams.Count > 0)
//            {
//                arrItems = new List<Helper.ListItem>();
//                foreach (PmpoObraDetalleDTO pmpoTeam in arrPmpoTeams)
//                {
//                    arrItems.Add(new ListItem()
//                    {
//                        ID = pmpoTeam.Equicodi,
//                        Text = pmpoTeam.Equinomb
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
//    }
//}
