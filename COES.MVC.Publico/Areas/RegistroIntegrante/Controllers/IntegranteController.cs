using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using log4net;
using COES.Servicios.Aplicacion.RegistroIntegrantes;
using COES.MVC.Publico.Areas.RegistroIntegrante.Models;
using COES.Dominio.DTO.Sic;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Configuration;
using COES.Framework.Base.Tools;
using System.IO;

namespace COES.MVC.Publico.Areas.RegistroIntegrante.Controllers
{
    public class IntegranteController : Controller
    {
        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>  
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(IntegranteController));

        /// <summary>
        /// Instancia de la clase de aplicación
        /// </summary>
        EmpresaAppServicio servicio = new EmpresaAppServicio();


        /// <summary>
        /// Instancia de la clase RegistroIntegrantesAppServicio
        /// </summary>
        private RegistroIntegrantesAppServicio appRegistroIntegrante = new RegistroIntegrantesAppServicio();
        private ReportesAppServicio appRegistroIntegranteReporte = new ReportesAppServicio();

        public IntegranteController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("IntegranteController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("IntegranteController", ex);
                throw;
            }
        }


        /// <summary>
        /// Retorna la vista de Registro de Ingreso para edición
        /// </summary>
        /// <returns></returns>
        public ActionResult Edicion(string codigo)
        {
            IntegranteModel model = new IntegranteModel();


            try
            {

                int emprcodi = Convert.ToInt32(RegistroIntegrantesHelper.DeCodificar(codigo));
                model.EmpresaCondicionVarianteGenerador = ConstantesRegistroIntegrantes.EmpresaCondicionVarianteGenerador;
                model.EmpresaCondicionVarianteTransmisor = ConstantesRegistroIntegrantes.EmpresaCondicionVarianteTransmisor;
                model.EmpresaCondicionVarianteDistribuidor = ConstantesRegistroIntegrantes.EmpresaCondicionVarianteDistribuidor;
                model.EmpresaCondicionVarianteUsuarioLibre = ConstantesRegistroIntegrantes.EmpresaCondicionVarianteUsuarioLibre;

                var objEmpresaRevi = appRegistroIntegrante.GetEmpresaByIdConRevision(emprcodi);
                if (objEmpresaRevi == null)
                {
                    return RedirectToAction("Index");
                }

                if (objEmpresaRevi.ReviEstadoDJR == ConstantesRegistroIntegrantes.RevisionEstadoObservado || objEmpresaRevi.ReviEstadoSGI == ConstantesRegistroIntegrantes.RevisionEstadoObservado)
                {

                    var TipoEmpresa = appRegistroIntegrante.ListTipoEmpresa();
                    model.TipoEmpresa = TipoEmpresa;

                    var revisionSGI = appRegistroIntegrante.GetByIdRiRevision(objEmpresaRevi.ReviCodiSGI);
                    var ListDetallesSGI = appRegistroIntegrante.ListRiDetalleRevisionByRevicodi(revisionSGI.Revicodi);

                    var revisionDJR = appRegistroIntegrante.GetByIdRiRevision(objEmpresaRevi.ReviCodiDJR);
                    var ListDetallesDJR = appRegistroIntegrante.ListRiDetalleRevisionByRevicodi(revisionDJR.Revicodi);

                    var Representante = appRegistroIntegrante.GetRepresentantesByEmprcodi(emprcodi);
                    var TipoComportamiento = appRegistroIntegrante.ListSiTipoComportamientoByEmprcodi(emprcodi);

                    model.RepresentanteLegal = Representante.FindAll(x => x.Rptetipo == ConstantesRegistroIntegrantes.RepresentanteTipoLegal && x.Rpteinicial == ConstantesRegistroIntegrantes.RepresentanteInicialSi);
                    model.PersonaContacto = Representante.Find(x => x.Rptetipo == ConstantesRegistroIntegrantes.RepresentanteTipoContacto && x.Rpteinicial == ConstantesRegistroIntegrantes.RepresentanteInicialSi);
                    model.ResponsableTramite = Representante.Find(x => x.Rptetipo == ConstantesRegistroIntegrantes.RepresentanteTipoResponsableTramite && x.Rpteinicial == ConstantesRegistroIntegrantes.RepresentanteInicialSi);
                    model.TipoComportamientoPrincipal = TipoComportamiento.Find(x => x.Tipoprincipal == ConstantesRegistroIntegrantes.TipoComportamientoPrincipalSi && x.Tipoinicial == ConstantesRegistroIntegrantes.TipoAgenteInicialSi);
                    model.Empresa = objEmpresaRevi;


                    List<TipoDocumentoSustentarioModel> TipoDocumentoSustentario = new List<TipoDocumentoSustentarioModel>();


                    if (model.TipoComportamientoPrincipal.Tipoemprcodi == ConstantesRegistroIntegrantes.TipoComportamientoGeneradorCodigo)
                    {
                        TipoDocumentoSustentario.Add(new TipoDocumentoSustentarioModel()
                        {
                            Id = ConstantesRegistroIntegrantes.TipoDocumentoSustentatorioCodigoAutorizacion,
                            Nombre = ConstantesRegistroIntegrantes.TipoDocumentoSustentatorioDescripcionAutorizacion
                        });

                        TipoDocumentoSustentario.Add(new TipoDocumentoSustentarioModel()
                        {
                            Id = ConstantesRegistroIntegrantes.TipoDocumentoSustentatorioCodigoConcesion,
                            Nombre = ConstantesRegistroIntegrantes.TipoDocumentoSustentatorioDescripcionConcesion
                        });
                    }
                    else if (model.TipoComportamientoPrincipal.Tipoemprcodi == ConstantesRegistroIntegrantes.TipoComportamientoTrasmisorCodigo)
                    {
                        TipoDocumentoSustentario.Add(new TipoDocumentoSustentarioModel()
                        {
                            Id = ConstantesRegistroIntegrantes.TipoDocumentoSustentatorioCodigoAutorizacion,
                            Nombre = ConstantesRegistroIntegrantes.TipoDocumentoSustentatorioDescripcionAutorizacion
                        });

                        TipoDocumentoSustentario.Add(new TipoDocumentoSustentarioModel()
                        {
                            Id = ConstantesRegistroIntegrantes.TipoDocumentoSustentatorioCodigoConcesion,
                            Nombre = ConstantesRegistroIntegrantes.TipoDocumentoSustentatorioDescripcionConcesion
                        });
                    }
                    else if (model.TipoComportamientoPrincipal.Tipoemprcodi == ConstantesRegistroIntegrantes.TipoComportamientoDistribuidorCodigo)
                    {
                        TipoDocumentoSustentario.Add(new TipoDocumentoSustentarioModel()
                        {
                            Id = ConstantesRegistroIntegrantes.TipoDocumentoSustentatorioCodigoAutorizacion,
                            Nombre = ConstantesRegistroIntegrantes.TipoDocumentoSustentatorioDescripcionAutorizacion
                        });
                    }
                    else if (model.TipoComportamientoPrincipal.Tipoemprcodi == ConstantesRegistroIntegrantes.TipoComportamientoUsuarioLibreCodigo)
                    {
                        TipoDocumentoSustentario.Add(new TipoDocumentoSustentarioModel()
                        {
                            Id = ConstantesRegistroIntegrantes.TipoDocumentoSustentatorioCodigoDeclaracionJurada,
                            Nombre = ConstantesRegistroIntegrantes.TipoDocumentoSustentatorioDescripcionDeclaracionJurada
                        });
                    }

                    if (model.TipoComportamientoPrincipal.Tipodocsustentatorio == ConstantesRegistroIntegrantes.TipoDocumentoSustentatorioDescripcionAutorizacion)
                    {
                        model.TipoComportamientoPrincipal.IdTipodocsustentatorio = ConstantesRegistroIntegrantes.TipoDocumentoSustentatorioCodigoAutorizacion;
                    }
                    else if (model.TipoComportamientoPrincipal.Tipodocsustentatorio == ConstantesRegistroIntegrantes.TipoDocumentoSustentatorioDescripcionAutorizacion)
                    {
                        model.TipoComportamientoPrincipal.IdTipodocsustentatorio = ConstantesRegistroIntegrantes.TipoDocumentoSustentatorioCodigoConcesion;
                    }
                    else if (model.TipoComportamientoPrincipal.Tipodocsustentatorio == ConstantesRegistroIntegrantes.TipoDocumentoSustentatorioDescripcionDeclaracionJurada)
                    {
                        model.TipoComportamientoPrincipal.IdTipodocsustentatorio = ConstantesRegistroIntegrantes.TipoDocumentoSustentatorioCodigoDeclaracionJurada;
                    }


                    model.TipoDocumentoSustentario = TipoDocumentoSustentario;


                    if (ListDetallesDJR != null && ListDetallesDJR.Count > 0)
                    {
                        model.ReviiteracionDJR = revisionDJR.Reviiteracion;
                    }
                    else
                    {
                        model.ReviiteracionDJR = 0;
                    }


                    if (ListDetallesSGI != null && ListDetallesSGI.Count > 0)
                    {
                        model.ReviiteracionSGI = revisionSGI.Reviiteracion;
                    }
                    else
                    {
                        model.ReviiteracionSGI = 0;
                    }



                    var ObjDOcS = model.TipoDocumentoSustentario.Find(x => x.Nombre == model.TipoComportamientoPrincipal.Tipodocsustentatorio);
                    if (ObjDOcS != null)
                    {
                        model.TipoComportamientoPrincipal.IdTipodocsustentatorio = ObjDOcS.Id;
                    }


                    if (ListDetallesSGI != null && ListDetallesSGI.Count > 0)
                    {
                        foreach (var item in ListDetallesSGI)
                        {
                            if (item.Dervcampo == ConstantesRegistroIntegrantes.DocumentoSustentatorio)
                            {
                                model.TipoComportamientoPrincipal.TipodocsustentatorioComentario = item.Dervobservacion;
                            }
                            else if (item.Dervcampo == ConstantesRegistroIntegrantes.ArchivoDigitalizado)
                            {
                                model.TipoComportamientoPrincipal.TipoarcdigitalizadoComentario = item.Dervobservacion;
                            }
                            else if (item.Dervcampo == ConstantesRegistroIntegrantes.PotenciaInstalada)
                            {
                                model.TipoComportamientoPrincipal.TipopotenciainstaladaComentario = item.Dervobservacion;
                            }
                            else if (item.Dervcampo == ConstantesRegistroIntegrantes.NumeroCentrales)
                            {
                                model.TipoComportamientoPrincipal.TiponrocentralesComentario = item.Dervobservacion;
                            }
                            else if (item.Dervcampo == ConstantesRegistroIntegrantes.TotalLineaTransmision)
                            {
                                model.TipoComportamientoPrincipal.TipototallineastransmisionComentario = item.Dervobservacion;
                            }
                            else if (item.Dervcampo == ConstantesRegistroIntegrantes.MaximaDemandaCoincidente)
                            {
                                model.TipoComportamientoPrincipal.TipomaxdemandacoincidenteComentario = item.Dervobservacion;
                            }
                            else if (item.Dervcampo == ConstantesRegistroIntegrantes.MaximaDemandaContratada)
                            {
                                model.TipoComportamientoPrincipal.TipomaxdemandacontratadaComentario = item.Dervobservacion;
                            }
                            else if (item.Dervcampo == ConstantesRegistroIntegrantes.NumeroSuministrador)
                            {
                                model.TipoComportamientoPrincipal.TiponumsuministradorComentario = item.Dervobservacion;
                            }
                        }
                    }


                    foreach (var item in model.RepresentanteLegal)
                    {
                        var Rep = ListDetallesDJR.FindAll(x => x.Dervcampo == ConstantesRegistroIntegrantes.DerCampoRepresentante);
                        if (Rep.Count > 0)
                        {
                            int index = Rep.FindIndex(x => int.Parse(x.Dervvalor) == item.Rptecodi);
                            if (index >= 0)
                            {
                                item.RpteObservacion = Rep[index].Dervobservacion;
                            }
                        }
                    }

                    List<TitulosAdicionalesModel> ListTitulosAdicionales = new List<TitulosAdicionalesModel>();

                    if (TipoComportamiento.Count > 0)
                    {
                        var TipoPrincipal = TipoComportamiento.Find(x => x.Tipoprincipal == ConstantesRegistroIntegrantes.PrincipalTipoComportamiento);

                        if (TipoPrincipal != null)
                        {
                            TitulosAdicionalesModel oTitulosAdicionalesModel = null;
                            if (!string.IsNullOrEmpty(TipoPrincipal.Tipodocname1) && !string.IsNullOrEmpty(TipoPrincipal.Tipodocadjfilename1))
                            {
                                oTitulosAdicionalesModel = new TitulosAdicionalesModel();
                                oTitulosAdicionalesModel.Numero = 1;
                                oTitulosAdicionalesModel.Nombre = TipoPrincipal.Tipodocname1;
                                oTitulosAdicionalesModel.FileName = TipoPrincipal.Tipodocadjfilename1;
                                ListTitulosAdicionales.Add(oTitulosAdicionalesModel);
                                if (!string.IsNullOrEmpty(TipoPrincipal.Tipodocname2) && !string.IsNullOrEmpty(TipoPrincipal.Tipodocadjfilename2))
                                {
                                    oTitulosAdicionalesModel = new TitulosAdicionalesModel();
                                    oTitulosAdicionalesModel.Numero = 2;
                                    oTitulosAdicionalesModel.Nombre = TipoPrincipal.Tipodocname2;
                                    oTitulosAdicionalesModel.FileName = TipoPrincipal.Tipodocadjfilename2;
                                    ListTitulosAdicionales.Add(oTitulosAdicionalesModel);
                                    if (!string.IsNullOrEmpty(TipoPrincipal.Tipodocname3) && !string.IsNullOrEmpty(TipoPrincipal.Tipodocadjfilename3))
                                    {
                                        oTitulosAdicionalesModel = new TitulosAdicionalesModel();
                                        oTitulosAdicionalesModel.Numero = 3;
                                        oTitulosAdicionalesModel.Nombre = TipoPrincipal.Tipodocname3;
                                        oTitulosAdicionalesModel.FileName = TipoPrincipal.Tipodocadjfilename3;
                                        ListTitulosAdicionales.Add(oTitulosAdicionalesModel);
                                        if (!string.IsNullOrEmpty(TipoPrincipal.Tipodocname4) && !string.IsNullOrEmpty(TipoPrincipal.Tipodocadjfilename4))
                                        {
                                            oTitulosAdicionalesModel = new TitulosAdicionalesModel();
                                            oTitulosAdicionalesModel.Numero = 4;
                                            oTitulosAdicionalesModel.Nombre = TipoPrincipal.Tipodocname4;
                                            oTitulosAdicionalesModel.FileName = TipoPrincipal.Tipodocadjfilename4;
                                            ListTitulosAdicionales.Add(oTitulosAdicionalesModel);
                                            if (!string.IsNullOrEmpty(TipoPrincipal.Tipodocname5) && !string.IsNullOrEmpty(TipoPrincipal.Tipodocadjfilename5))
                                            {
                                                oTitulosAdicionalesModel = new TitulosAdicionalesModel();
                                                oTitulosAdicionalesModel.Numero = 5;
                                                oTitulosAdicionalesModel.Nombre = TipoPrincipal.Tipodocname5;
                                                oTitulosAdicionalesModel.FileName = TipoPrincipal.Tipodocadjfilename5;
                                                ListTitulosAdicionales.Add(oTitulosAdicionalesModel);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    model.TitulosAdicionales = ListTitulosAdicionales;
                    if (model.TitulosAdicionales != null && model.TitulosAdicionales.Count > 0)
                    {
                        foreach (var item in model.TitulosAdicionales)
                        {
                            var Ti = ListDetallesDJR.FindAll(x => x.Dervcampo == ConstantesRegistroIntegrantes.DerCampoTA);
                            if (Ti.Count > 0)
                            {
                                int index = Ti.FindIndex(x => int.Parse(x.Dervvalor) == item.Numero);
                                if (index >= 0)
                                {
                                    item.RpteObservacion = Ti[index].Dervobservacion;
                                }
                            }
                        }
                    }

                }
                else
                {
                    return RedirectToAction("Index");
                }

                var TituloEmpresa = ConfigurationManager.AppSettings["riTituloEmpresa"].ToString();
                var TituloTipoIntegrante = ConfigurationManager.AppSettings["riTituloTipoIntegrante"].ToString();
                var TituloRepresentanteLegal = ConfigurationManager.AppSettings["riTituloRepresentanteLegal"].ToString();
                var TituloPersonaContacto = ConfigurationManager.AppSettings["riTituloPersonaContacto"].ToString();
                var TituloPersonaResponsable = ConfigurationManager.AppSettings["riTituloPersonaResponsable"].ToString();

                model.TituloEmpresa = TituloEmpresa;
                model.TituloTipoIntegrante = TituloTipoIntegrante;
                model.TituloRepresentanteLegal = TituloRepresentanteLegal;
                model.TituloPersonaContacto = TituloPersonaContacto;
                model.TituloPersonaResponsable = TituloPersonaResponsable;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return RedirectToAction("Index");
            }

            return View("Edicion", model);
        }

        /// <summary>
        /// Retorna la vista de Registro de Ingreso
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var TipoEmpresa = appRegistroIntegrante.ListTipoEmpresa();

            IntegranteModel model = new IntegranteModel();

            model.SiteKey = ConfigurationManager.AppSettings["siteKey"].ToString();
            model.TipoEmpresa = TipoEmpresa;

            List<TipoDocumentoSustentarioModel> TipoDocumentoSustentario = new List<TipoDocumentoSustentarioModel>();

            TipoDocumentoSustentario.Add(new TipoDocumentoSustentarioModel()
            {
                Id = ConstantesRegistroIntegrantes.TipoDocumentoSustentatorioCodigoAutorizacion,
                Nombre = ConstantesRegistroIntegrantes.TipoDocumentoSustentatorioDescripcionAutorizacion
            });

            TipoDocumentoSustentario.Add(new TipoDocumentoSustentarioModel()
            {
                Id = ConstantesRegistroIntegrantes.TipoDocumentoSustentatorioCodigoConcesion,
                Nombre = ConstantesRegistroIntegrantes.TipoDocumentoSustentatorioDescripcionConcesion
            });

            TipoDocumentoSustentario.Add(new TipoDocumentoSustentarioModel()
            {
                Id = ConstantesRegistroIntegrantes.TipoDocumentoSustentatorioCodigoDeclaracionJurada,
                Nombre = ConstantesRegistroIntegrantes.TipoDocumentoSustentatorioDescripcionDeclaracionJurada
            });

            model.TipoDocumentoSustentario = TipoDocumentoSustentario;

            model.EmpresaCondicionVarianteGenerador = ConstantesRegistroIntegrantes.EmpresaCondicionVarianteGenerador;
            model.EmpresaCondicionVarianteTransmisor = ConstantesRegistroIntegrantes.EmpresaCondicionVarianteTransmisor;
            model.EmpresaCondicionVarianteDistribuidor = ConstantesRegistroIntegrantes.EmpresaCondicionVarianteDistribuidor;
            model.EmpresaCondicionVarianteUsuarioLibre = ConstantesRegistroIntegrantes.EmpresaCondicionVarianteUsuarioLibre;


            var TituloEmpresa = ConfigurationManager.AppSettings["riTituloEmpresa"].ToString();
            var TituloTipoIntegrante = ConfigurationManager.AppSettings["riTituloTipoIntegrante"].ToString();
            var TituloRepresentanteLegal = ConfigurationManager.AppSettings["riTituloRepresentanteLegal"].ToString();
            var TituloPersonaContacto = ConfigurationManager.AppSettings["riTituloPersonaContacto"].ToString();
            var TituloPersonaResponsable = ConfigurationManager.AppSettings["riTituloPersonaResponsable"].ToString();

            model.TituloEmpresa = TituloEmpresa;
            model.TituloTipoIntegrante = TituloTipoIntegrante;
            model.TituloRepresentanteLegal = TituloRepresentanteLegal;
            model.TituloPersonaContacto = TituloPersonaContacto;
            model.TituloPersonaResponsable = TituloPersonaResponsable;

            return View(model);
        }

        /// <summary>
        /// Inserta el formulario de registro de integrantes
        /// </summary>
        /// <param name="model">model IntegranteModel</param>
        /// <returns></returns>
        public JsonResult Insertar(IntegranteModel model)
        {

            JsonResult jRespuesta;

            //if (!ValidarNoSoyRobot(model.Capcha))
            //{
            //    jRespuesta = Json(-1, JsonRequestBehavior.AllowGet);
            //    return jRespuesta;
            //}

            //if (!ValidacionEmpresa(model))
            //{
            //    jRespuesta = Json(-2, JsonRequestBehavior.AllowGet);
            //    return jRespuesta;
            //}

            string para = "";

            int id = 0;

            try
            {
                var Path = ConstantesRegistroIntegrantes.FolderUploadRutaCompleta;

                List<RepresentanteModel> Representante = new List<RepresentanteModel>();

                string[] RepresentanteArray = model.strRepresentateLegal.Split('|');

                RepresentanteModel oRepresentanteModel = null;
                foreach (var item in RepresentanteArray)
                {
                    var Datos = item.Split('*');
                    oRepresentanteModel = new RepresentanteModel();
                    oRepresentanteModel.Id = int.Parse(Datos[8]);
                    if (Datos[0].ToUpper() == ConstantesRegistroIntegrantes.RepresentanteLegalTitular)
                    {
                        oRepresentanteModel.Rptetiprepresentantelegal = ConstantesRegistroIntegrantes.RepresentanteLegalTipoTitular;
                    }
                    else
                    {
                        oRepresentanteModel.Rptetiprepresentantelegal = ConstantesRegistroIntegrantes.RepresentanteLegalTipoAlterno;
                    }
                    oRepresentanteModel.Rptedocidentidad = Datos[1];
                    oRepresentanteModel.Rptenombres = Datos[2];
                    oRepresentanteModel.Rpteapellidos = Datos[3];
                    oRepresentanteModel.Rptecargoempresa = Datos[4];
                    oRepresentanteModel.Rptetelefono = Datos[5];
                    oRepresentanteModel.Rptecorreoelectronico = Datos[6].ToLower();
                    oRepresentanteModel.Rptetelfmovil = Datos[7];
                    oRepresentanteModel.Rptetipo = ConstantesRegistroIntegrantes.RepresentanteTipoLegal;
                    Representante.Add(oRepresentanteModel);
                }

                string[] Files = Request.Files.AllKeys;
                for (int i = 0; i < Files.Length; i++)
                {
                    string item = Files[i];
                    if (item.Contains("flDNIRL"))
                    {
                        int idDNI = int.Parse(item.Replace("flDNIRL", ""));
                        Representante.Find(x => x.Id == idDNI).DNI = Request.Files[i];
                    }
                    else if (item.Contains("flVigenciaPoderRL"))
                    {
                        int idVP = int.Parse(item.Replace("flVigenciaPoderRL", ""));
                        Representante.Find(x => x.Id == idVP).VigenciaPoder = Request.Files[i];
                    }
                }

                oRepresentanteModel = new RepresentanteModel();
                oRepresentanteModel.Rptetipo = ConstantesRegistroIntegrantes.RepresentanteTipoResponsableTramite;
                oRepresentanteModel.Rptetiprepresentantelegal = ConstantesRegistroIntegrantes.RepresentanteLegalTipoTitular;
                oRepresentanteModel.Rptenombres = model.RTNombres;
                oRepresentanteModel.Rpteapellidos = model.RTApellidos;
                oRepresentanteModel.Rptecargoempresa = model.RTCargoEmpresa;
                oRepresentanteModel.Rptetelefono = model.RTTelefono;
                oRepresentanteModel.Rptetelfmovil = model.RTTelefonoMobil;
                oRepresentanteModel.Rptecorreoelectronico = model.RTCorreoElectronico.ToLower();
                Representante.Add(oRepresentanteModel);

                oRepresentanteModel = new RepresentanteModel();
                oRepresentanteModel.Rptetipo = ConstantesRegistroIntegrantes.RepresentanteTipoContacto;
                oRepresentanteModel.Rptetiprepresentantelegal = ConstantesRegistroIntegrantes.RepresentanteLegalTipoTitular;
                oRepresentanteModel.Rptenombres = model.PCNombres;
                oRepresentanteModel.Rpteapellidos = model.PCApellidos;
                oRepresentanteModel.Rptecargoempresa = model.PCCargoEmpresa;
                oRepresentanteModel.Rptetelefono = model.PCTelefono;
                oRepresentanteModel.Rptetelfmovil = model.PCTelefonoMobil;
                oRepresentanteModel.Rptecorreoelectronico = model.PCCorreoElectronico.ToLower();
                Representante.Add(oRepresentanteModel);

                SiEmpresaDTO entity = new SiEmpresaDTO();
                entity.Emprnomb = (model.Emprrazsocial.Length > 70) ? model.Emprrazsocial.Substring(0, 69) : model.Emprrazsocial;
                entity.Emprnombcomercial = model.Emprnombrecomercial;
                entity.Emprnombrecomercial = model.Emprnombrecomercial;
                entity.Tipoemprcodi = model.Tipoemprcodi;
                //entity.Emprdire = model.Emprdire;
                //entity.Emprtele = model.Emprtele;
                //entity.Emprnumedocu = model.Emprnumedocu;
                entity.Tipodocucodi = "";
                entity.Emprruc = model.Emprruc;
                entity.Emprabrev = model.Emprabrev;
                entity.Emprorden = model.Emprorden;
                entity.Emprdom = "";
                entity.Emprsein = "";
                entity.Emprrazsocial = model.Emprrazsocial;
                entity.Emprcoes = "";
                entity.Lastuser = "";
                entity.Lastdate = DateTime.Now;
                entity.Inddemanda = "";
                entity.Emprestado = ConstantesRegistroIntegrantes.EmpresaEstadoActivo;

                entity.Emprdomiciliolegal = model.Emprdire;
                entity.Emprsigla = model.Emprsigla;
                entity.Emprnumpartidareg = model.Emprnumedocu;
                entity.Emprtelefono = model.Emprtele;
                entity.Emprfax = model.Emprfax;
                entity.Emprpagweb = model.Emprpagweb;
                entity.Emprcartadjunto = model.RTCartaSolicitudIngreso.FileName;

                entity.Emprestadoregistro = ConstantesRegistroIntegrantes.EmpresaEstadoRegistroPendienteCodigo;
                entity.Emprfechainscripcion = DateTime.Now;
                entity.Emprfechacreacion = DateTime.Now;

                Random oAleatorio = new Random();
                string cExtension = "";
                cExtension = System.IO.Path.GetExtension(model.RTCartaSolicitudIngreso.FileName);
                string FileNameCartaAdjunto = string.Format("{0:yyyyMMddHHmmss}{1}{2}", DateTime.Now, oAleatorio.Next(10, 100), cExtension);
                entity.Emprcartadjuntofilename = FileNameCartaAdjunto;
                FileServer.UploadFromStream(model.RTCartaSolicitudIngreso.InputStream, ConstantesRegistroIntegrantes.FolderRI, FileNameCartaAdjunto, Path);

                SiTipoComportamientoDTO entityTC = new SiTipoComportamientoDTO();
                entityTC.Tipoprincipal = ConstantesRegistroIntegrantes.TipoComportamientoPrincipalSi;
                entityTC.Tipotipagente = model.Tipotipagente;
                entityTC.Tipodocsustentatorio = model.Tipodocsustentatorio;
                cExtension = System.IO.Path.GetExtension(model.Tipoarcdigitalizado.FileName);

                string FileNameArchivoDigitalizado = string.Format("{0:yyyyMMddHHmmss}{1}{2}", DateTime.Now, oAleatorio.Next(10, 100), cExtension);
                entityTC.Tipoarcdigitalizado = model.Tipoarcdigitalizado.FileName;
                FileServer.UploadFromStream(model.Tipoarcdigitalizado.InputStream, ConstantesRegistroIntegrantes.FolderRI, FileNameArchivoDigitalizado, Path);

                entityTC.Tipoarcdigitalizadofilename = FileNameArchivoDigitalizado;
                entityTC.Tipopotenciainstalada = model.Tipopotenciainstalada;
                entityTC.Tiponrocentrales = model.Tiponrocentrales;
                entityTC.Tipolineatrans500 = model.Tipolineatrans_500;
                entityTC.Tipolineatrans220 = model.Tipolineatrans_220;
                entityTC.Tipolineatrans138 = model.Tipolineatrans_138;
                entityTC.Tipolineatrans500km = model.Tipolineatrans_500km;
                entityTC.Tipolineatrans220km = model.Tipolineatrans_220km;
                entityTC.Tipolineatrans138km = model.Tipolineatrans_138km;
                entityTC.Tipolineatransmenor138 = model.Tipolineatrans_menor138;
                entityTC.Tipolineatransmenor138km = model.Tipolineatrans_menor138km;

                decimal TipoLT500 = 0;
                decimal TipoLT220 = 0;
                decimal TipoLT138 = 0;
                decimal TipoLTMenor138 = 0;

                if (model.Tipolineatrans_500km != null)
                {
                    TipoLT500 = decimal.Parse(model.Tipolineatrans_500km);
                }
                if (model.Tipolineatrans_220km != null)
                {
                    TipoLT220 = decimal.Parse(model.Tipolineatrans_220km);
                }
                if (model.Tipolineatrans_138km != null)
                {
                    TipoLT138 = decimal.Parse(model.Tipolineatrans_138km);
                }
                if (model.Tipolineatrans_menor138km != null)
                {
                    TipoLTMenor138 = decimal.Parse(model.Tipolineatrans_menor138km);
                }

                entityTC.Tipototallineastransmision = Convert.ToString(TipoLT500 + TipoLT220 + TipoLT138 + TipoLTMenor138);
                entityTC.Tipomaxdemandacoincidente = model.Tipomaxdemandacoincidente;
                entityTC.Tipomaxdemandacontratada = model.Tipomaxdemandacontratada;
                entityTC.Tiponumsuministrador = model.Tiponumsuministrador;
                entityTC.Tipofeccreacion = DateTime.Now;
                entityTC.Tipoemprcodi = model.Tipoemprcodi;
                entityTC.Tipobaja = ConstantesRegistroIntegrantes.RpteBajaNo;
                entityTC.Tipoinicial = ConstantesRegistroIntegrantes.TipoAgenteInicialSi;
                entityTC.Tipocomentario = model.Tipocomentario;

                entityTC.Emprcodi = id;

                if (model.Tipodocname1 != null)
                {
                    cExtension = System.IO.Path.GetExtension(model.Tipodocname1.FileName);
                    string FileNameTA1 = string.Format("{0:yyyyMMddHHmmss}{1}{2}", DateTime.Now, oAleatorio.Next(10, 100), cExtension);
                    entityTC.Tipodocname1 = model.Tipodocname1.FileName;
                    entityTC.Tipodocadjfilename1 = FileNameTA1;
                    FileServer.UploadFromStream(model.Tipodocname1.InputStream, ConstantesRegistroIntegrantes.FolderRI, FileNameTA1, Path);
                    if (model.Tipodocname2 != null)
                    {
                        cExtension = System.IO.Path.GetExtension(model.Tipodocname2.FileName);
                        string FileNameTA2 = string.Format("{0:yyyyMMddHHmmss}{1}{2}", DateTime.Now, oAleatorio.Next(10, 100), cExtension);
                        entityTC.Tipodocname2 = model.Tipodocname2.FileName;
                        entityTC.Tipodocadjfilename2 = FileNameTA2;
                        FileServer.UploadFromStream(model.Tipodocname2.InputStream, ConstantesRegistroIntegrantes.FolderRI, FileNameTA2, Path);
                        if (model.Tipodocname3 != null)
                        {
                            cExtension = System.IO.Path.GetExtension(model.Tipodocname3.FileName);
                            string FileNameTA3 = string.Format("{0:yyyyMMddHHmmss}{1}{2}", DateTime.Now, oAleatorio.Next(10, 100), cExtension);
                            entityTC.Tipodocname3 = model.Tipodocname3.FileName;
                            entityTC.Tipodocadjfilename3 = FileNameTA3;
                            FileServer.UploadFromStream(model.Tipodocname3.InputStream, ConstantesRegistroIntegrantes.FolderRI, FileNameTA3, Path);
                            if (model.Tipodocname4 != null)
                            {
                                cExtension = System.IO.Path.GetExtension(model.Tipodocname4.FileName);
                                string FileNameTA4 = string.Format("{0:yyyyMMddHHmmss}{1}{2}", DateTime.Now, oAleatorio.Next(10, 100), cExtension);
                                entityTC.Tipodocname4 = model.Tipodocname4.FileName;
                                entityTC.Tipodocadjfilename4 = FileNameTA4;
                                FileServer.UploadFromStream(model.Tipodocname4.InputStream, ConstantesRegistroIntegrantes.FolderRI, FileNameTA4, Path);
                                if (model.Tipodocname5 != null)
                                {
                                    cExtension = System.IO.Path.GetExtension(model.Tipodocname5.FileName);
                                    string FileNameTA5 = string.Format("{0:yyyyMMddHHmmss}{1}{2}", DateTime.Now, oAleatorio.Next(10, 100), cExtension);
                                    entityTC.Tipodocname5 = model.Tipodocname5.FileName;
                                    entityTC.Tipodocadjfilename5 = FileNameTA5;
                                    FileServer.UploadFromStream(model.Tipodocname5.InputStream, ConstantesRegistroIntegrantes.FolderRI, FileNameTA5, Path);
                                }
                            }
                        }
                    }
                }

                List<SiRepresentanteDTO> Representantes = new List<SiRepresentanteDTO>();
                SiRepresentanteDTO entityRepresentante = null;
                foreach (var item in Representante)
                {
                    entityRepresentante = new SiRepresentanteDTO();
                    if (item.Rptetipo == ConstantesRegistroIntegrantes.RepresentanteTipoLegal)
                    {
                        if (item.Rptedocidentidad != null)
                        {
                            if (item.Rptedocidentidad.Length == 8)
                            {
                                entityRepresentante.Rptetipdocidentidad = ConstantesRegistroIntegrantes.TipoDocumentoIdentidadDNI;
                            }
                            else
                            {
                                entityRepresentante.Rptetipdocidentidad = ConstantesRegistroIntegrantes.TipoDocumentoIdentidadCarneExtranjeria;
                            }
                        }

                        entityRepresentante.Rptedocidentidad = item.Rptedocidentidad;

                        cExtension = System.IO.Path.GetExtension(item.DNI.FileName);
                        string FileNameDNI = string.Format("{0:yyyyMMddHHmmss}{1}{2}", DateTime.Now, oAleatorio.Next(10, 100), cExtension);
                        entityRepresentante.Rptedocidentidadadj = item.DNI.FileName;
                        entityRepresentante.Rptedocidentidadadjfilename = FileNameDNI;
                        FileServer.UploadFromStream(item.DNI.InputStream, ConstantesRegistroIntegrantes.FolderRI, FileNameDNI, Path);

                        cExtension = System.IO.Path.GetExtension(item.VigenciaPoder.FileName);
                        string FileNameVigencia = string.Format("{0:yyyyMMddHHmmss}{1}{2}", DateTime.Now, oAleatorio.Next(10, 100), cExtension);
                        entityRepresentante.Rptevigenciapoder = item.VigenciaPoder.FileName;
                        entityRepresentante.Rptevigenciapoderfilename = FileNameVigencia;
                        FileServer.UploadFromStream(item.VigenciaPoder.InputStream, ConstantesRegistroIntegrantes.FolderRI, FileNameVigencia, Path);
                    }

                    entityRepresentante.Emprcodi = id;
                    entityRepresentante.Rptetipo = item.Rptetipo;
                    entityRepresentante.Rptetiprepresentantelegal = item.Rptetiprepresentantelegal;
                    entityRepresentante.Rptebaja = ConstantesRegistroIntegrantes.RepresentanteBajaNo;
                    entityRepresentante.Rptenombres = item.Rptenombres;
                    entityRepresentante.Rpteapellidos = item.Rpteapellidos;
                    entityRepresentante.Rptecargoempresa = item.Rptecargoempresa;
                    entityRepresentante.Rptetelefono = item.Rptetelefono;
                    entityRepresentante.Rptetelfmovil = item.Rptetelfmovil;
                    entityRepresentante.Rptecorreoelectronico = item.Rptecorreoelectronico.ToLower();
                    entityRepresentante.Rptefeccreacion = DateTime.Now;

                    entityRepresentante.Rpteinicial = ConstantesRegistroIntegrantes.RepresentanteInicialSi;
                    Representantes.Add(entityRepresentante);
                }

                entity.TipoComportamiento = entityTC;
                entity.Representante = Representantes;

                string condicion = "";
                int valor = 0;
                switch (entity.Tipoemprcodi)
                {
                    case (ConstantesRegistroIntegrantes.TipoComportamientoGeneradorCodigo):
                        try
                        {
                            valor = Convert.ToInt32(entityTC.Tipopotenciainstalada);
                        }
                        catch
                        {
                            valor = 0;
                        }

                        if (valor >= ConstantesRegistroIntegrantes.EmpresaCondicionVarianteGenerador)
                            condicion = ConstantesRegistroIntegrantes.EmpresaCondicionObligatorio;
                        else
                            condicion = ConstantesRegistroIntegrantes.EmpresaCondicionVoluntario;

                        break;
                    case (ConstantesRegistroIntegrantes.TipoComportamientoTrasmisorCodigo):
                        try
                        {
                            valor = Convert.ToInt32(entityTC.Tipolineatrans138km);
                            valor += Convert.ToInt32(entityTC.Tipolineatrans220km);
                            valor += Convert.ToInt32(entityTC.Tipolineatrans500km);
                        }
                        catch
                        {
                            valor = 0;
                        }

                        if (valor >= ConstantesRegistroIntegrantes.EmpresaCondicionVarianteTransmisor)
                            condicion = ConstantesRegistroIntegrantes.EmpresaCondicionObligatorio;
                        else
                            condicion = ConstantesRegistroIntegrantes.EmpresaCondicionVoluntario;

                        break;
                    case (ConstantesRegistroIntegrantes.TipoComportamientoDistribuidorCodigo):
                        try
                        {
                            valor = Convert.ToInt32(entityTC.Tipomaxdemandacoincidente);
                        }
                        catch
                        {
                            valor = 0;
                        }

                        if (valor >= ConstantesRegistroIntegrantes.EmpresaCondicionVarianteDistribuidor)
                            condicion = ConstantesRegistroIntegrantes.EmpresaCondicionObligatorio;
                        else
                            condicion = ConstantesRegistroIntegrantes.EmpresaCondicionVoluntario;

                        break;
                    case (ConstantesRegistroIntegrantes.TipoComportamientoUsuarioLibreCodigo):
                        try
                        {
                            valor = Convert.ToInt32(entityTC.Tipomaxdemandacontratada);
                        }
                        catch
                        {
                            valor = 0;
                        }

                        if (valor >= ConstantesRegistroIntegrantes.EmpresaCondicionVarianteUsuarioLibre)
                            condicion = ConstantesRegistroIntegrantes.EmpresaCondicionObligatorio;
                        else
                            condicion = ConstantesRegistroIntegrantes.EmpresaCondicionVoluntario;

                        break;
                }

                entity.Emprcondicion = condicion;
                entity.Emprcoes = "N";
                entity.Emprsein = "S";

                para = appRegistroIntegrante.SaveSIEmpresa(entity);
                id = int.Parse(para.Split(',')[0]);
                if (id != 0)
                {
                    try
                    {
                        //Enviar Correo al responsable del tramite

                        int correlativo = int.Parse(para.Split(',')[1]);
                        string toEmail = model.RTCorreoElectronico;
                        string ccEmail = ConfigurationManager.AppSettings["admRegistroIntegrantes"];

                        string msg = string.Empty;

                        string responsableTramite = model.RTNombres + ' ' + model.RTApellidos;
                        //Garantia 9Jul18 - nro 2 si no existe nombre debe mostrarse razon social
                        string empresa = (model.Emprnombrecomercial.Length > 3) ? model.Emprnombrecomercial : model.Emprrazsocial;
                        //fin Garantia 9Jul18 
                        msg = RegistroIntegrantesHelper.Registro_BodyMailAceptado(responsableTramite, empresa, correlativo.ToString());

                        log.Info("Inscripción - Registro - Envío de correo al resposable tramite");
                        COES.Base.Tools.Util.SendEmail(toEmail, ccEmail, "Notificacion - Registro de Integrantes", msg);

                    }
                    catch (Exception exCorreo)
                    {
                        log.Error(exCorreo);
                    }

                    try
                    {
                        //Enviar Correo al persona de contacto

                        int correlativo = int.Parse(para.Split(',')[1]);
                        string toEmail = model.PCCorreoElectronico;
                        string ccEmail = ConfigurationManager.AppSettings["admRegistroIntegrantes"];

                        string msg = string.Empty;

                        string personacontacto = model.PCNombres + ' ' + model.PCApellidos;
                        //Garantia 9Jul18 - nro 2 si no existe nombre debe mostrarse razon social
                        string empresa = (model.Emprnombrecomercial.Length > 3) ? model.Emprnombrecomercial : model.Emprrazsocial;
                        //fin Garantia 9Jul18 

                        msg = RegistroIntegrantesHelper.Registro_BodyMailAceptado(personacontacto, empresa, correlativo.ToString());

                        log.Info("Inscripción - Registro - Envío de correo a la persona de contacto");
                        COES.Base.Tools.Util.SendEmail(toEmail, ccEmail, "Notificacion - Registro de Integrantes", msg);

                    }
                    catch (Exception exCorreo)
                    {
                        log.Error(exCorreo);
                    }

                    try
                    {
                        //Enviar Correo a los representantes legales

                        int correlativo = int.Parse(para.Split(',')[1]);
                        string toEmail = "";
                        string ccEmail = ConfigurationManager.AppSettings["admRegistroIntegrantes"];

                        string msg = string.Empty;
                        //Garantia 9Jul18 - nro 2 si no existe nombre debe mostrarse razon social
                        string empresa = (model.Emprnombrecomercial.Length > 3) ? model.Emprnombrecomercial : model.Emprrazsocial;
                        //fin Garantia 9Jul18 
                        string representantelegales = "";

                        foreach (var item in RepresentanteArray)
                        {
                            var Datos = item.Split('*');
                            representantelegales = Datos[2] + ' ' + Datos[3];
                            toEmail = Datos[6];

                            msg = RegistroIntegrantesHelper.Registro_BodyMailAceptado(representantelegales, empresa, correlativo.ToString());

                            log.Info("Inscripción - Registro - Envío de correo al representante legal");
                            COES.Base.Tools.Util.SendEmail(toEmail, ccEmail, "Notificacion - Registro de Integrantes", msg);

                        }
                    }
                    catch (Exception exCorreo)
                    {
                        log.Error(exCorreo);
                    }
                }

            }

            catch (Exception ex)
            {
                log.Error(ex);
                id = 0;
                para = "";
            }

            jRespuesta = Json(para, JsonRequestBehavior.AllowGet);
            return jRespuesta;
        }



        /// <summary>
        /// Reenvia el formulario de registro de integrantes
        /// </summary>
        /// <param name="model">model IntegranteModel</param>
        /// <returns></returns>
        public JsonResult Reenviar(IntegranteModel model)
        {

            JsonResult jRespuesta;


            int id = model.Emprcodi;

            try
            {

                var Path = ConstantesRegistroIntegrantes.FolderUploadRutaCompleta;

                List<RepresentanteModel> Representante = new List<RepresentanteModel>();

                string[] RepresentanteArray = model.strRepresentateLegal.Split('|');

                RepresentanteModel oRepresentanteModel = null;
                foreach (var item in RepresentanteArray)
                {
                    var Datos = item.Split('*');
                    oRepresentanteModel = new RepresentanteModel();
                    oRepresentanteModel.Id = int.Parse(Datos[8]);
                    if (Datos[0].ToUpper() == ConstantesRegistroIntegrantes.RepresentanteLegalTitular)
                    {
                        oRepresentanteModel.Rptetiprepresentantelegal = ConstantesRegistroIntegrantes.RepresentanteLegalTipoTitular;
                    }
                    else
                    {
                        oRepresentanteModel.Rptetiprepresentantelegal = ConstantesRegistroIntegrantes.RepresentanteLegalTipoAlterno;
                    }
                    oRepresentanteModel.Rptedocidentidad = Datos[1];
                    oRepresentanteModel.Rptenombres = Datos[2];
                    oRepresentanteModel.Rpteapellidos = Datos[3];
                    oRepresentanteModel.Rptecargoempresa = Datos[4];
                    oRepresentanteModel.Rptetelefono = Datos[5];
                    oRepresentanteModel.Rptecorreoelectronico = Datos[6];
                    oRepresentanteModel.Rptetelfmovil = Datos[7];
                    oRepresentanteModel.Rptetipo = ConstantesRegistroIntegrantes.RepresentanteTipoLegal;
                    oRepresentanteModel.RpteObservacion = Datos[9];
                    Representante.Add(oRepresentanteModel);
                }

                string[] Files = Request.Files.AllKeys;
                for (int i = 0; i < Files.Length; i++)
                {
                    string item = Files[i];
                    if (item.Contains("flDNIRL"))
                    {
                        int idDNI = int.Parse(item.Replace("flDNIRL", ""));
                        Representante.Find(x => x.Id == idDNI).DNI = Request.Files[i];
                    }
                    else if (item.Contains("flVigenciaPoderRL"))
                    {
                        int idVP = int.Parse(item.Replace("flVigenciaPoderRL", ""));
                        Representante.Find(x => x.Id == idVP).VigenciaPoder = Request.Files[i];
                    }
                }



                SiEmpresaDTO entity = new SiEmpresaDTO();
                entity.Emprcodi = model.Emprcodi;
                entity.Emprnomb = model.Emprnomb;
                entity.Emprnombcomercial = model.Emprnombrecomercial;
                entity.Emprnombrecomercial = model.Emprnombrecomercial;
                entity.Tipoemprcodi = model.Tipoemprcodi;
                entity.Emprdire = model.Emprdire;
                entity.Emprtele = model.Emprtele;
                entity.Emprnumedocu = model.Emprnumedocu;
                entity.Tipodocucodi = "";
                entity.Emprruc = model.Emprruc;
                entity.Emprabrev = model.Emprabrev;
                entity.Emprorden = model.Emprorden;
                entity.Emprdom = "";
                entity.Emprsein = "";
                entity.Emprrazsocial = model.Emprrazsocial;
                entity.Emprcoes = "";
                entity.Lastuser = "";
                entity.Lastdate = DateTime.Now;
                entity.Inddemanda = "";
                entity.Emprestado = ConstantesRegistroIntegrantes.EmpresaEstadoActivo;

                entity.Emprdomiciliolegal = model.Emprdire;
                entity.Emprsigla = model.Emprsigla;
                entity.Emprnumpartidareg = model.Emprnumedocu;
                entity.Emprtelefono = model.Emprtele;
                entity.Emprfax = model.Emprfax;
                entity.Emprpagweb = model.Emprpagweb;

                entity.Emprestadoregistro = ConstantesRegistroIntegrantes.EmpresaEstadoRegistroPendienteCodigo;
                entity.Emprfechainscripcion = DateTime.Now;
                entity.Emprfechacreacion = DateTime.Now;

                entity.ReviiteracionSGI = model.ReviiteracionSGI;
                entity.ReviiteracionDRJ = model.ReviiteracionDJR;

                Random oAleatorio = new Random();
                string cExtension = "";

                SiTipoComportamientoDTO entityTC = new SiTipoComportamientoDTO();

                entityTC.Tipocodi = model.Tipocodi;
                entityTC.Tipoprincipal = ConstantesRegistroIntegrantes.TipoComportamientoPrincipalSi;
                entityTC.Tipotipagente = model.Tipotipagente;
                entityTC.Tipodocsustentatorio = model.Tipodocsustentatorio;

                string FileNameArchivoDigitalizado = "";
                if (model.Tipoarcdigitalizado != null)
                {
                    cExtension = System.IO.Path.GetExtension(model.Tipoarcdigitalizado.FileName);
                    FileNameArchivoDigitalizado = string.Format("{0:yyyyMMddHHmmss}{1}{2}", DateTime.Now, oAleatorio.Next(10, 100), cExtension);
                    entityTC.Tipoarcdigitalizado = model.Tipoarcdigitalizado.FileName;
                    entityTC.Tipoarcdigitalizadofilename = FileNameArchivoDigitalizado;
                    FileServer.UploadFromStream(model.Tipoarcdigitalizado.InputStream, ConstantesRegistroIntegrantes.FolderRI, FileNameArchivoDigitalizado, Path);
                }
                else
                {
                    entityTC.Tipoarcdigitalizado = model.Tipoarchivodigitalizado;
                    entityTC.Tipoarcdigitalizadofilename = model.Tipoarchivodigitalizadofilename;
                }


                entityTC.Tipopotenciainstalada = model.Tipopotenciainstalada;
                entityTC.Tiponrocentrales = model.Tiponrocentrales;
                entityTC.Tipolineatrans500 = model.Tipolineatrans_500;
                entityTC.Tipolineatrans220 = model.Tipolineatrans_220;
                entityTC.Tipolineatrans138 = model.Tipolineatrans_138;
                entityTC.Tipolineatrans500km = model.Tipolineatrans_500km;
                entityTC.Tipolineatrans220km = model.Tipolineatrans_220km;
                entityTC.Tipolineatrans138km = model.Tipolineatrans_138km;

                decimal TipoLT500 = 0;
                decimal TipoLT220 = 0;
                decimal TipoLT138 = 0;

                if (model.Tipolineatrans_500km != null)
                {
                    TipoLT500 = decimal.Parse(model.Tipolineatrans_500km);
                }
                if (model.Tipolineatrans_220km != null)
                {
                    TipoLT220 = decimal.Parse(model.Tipolineatrans_220km);
                }
                if (model.Tipolineatrans_138km != null)
                {
                    TipoLT138 = decimal.Parse(model.Tipolineatrans_138km);
                }
                entityTC.Tipototallineastransmision = Convert.ToString(TipoLT500 + TipoLT220 + TipoLT138);
                entityTC.Tipomaxdemandacoincidente = model.Tipomaxdemandacoincidente;
                entityTC.Tipomaxdemandacontratada = model.Tipomaxdemandacontratada;
                entityTC.Tiponumsuministrador = model.Tiponumsuministrador;
                entityTC.Tipofeccreacion = DateTime.Now;
                entityTC.Tipoemprcodi = model.Tipoemprcodi;
                entityTC.Tipobaja = ConstantesRegistroIntegrantes.RpteBajaNo;
                entityTC.Tipoinicial = ConstantesRegistroIntegrantes.TipoAgenteInicialSi;
                entityTC.Tipocomentario = model.Tipocomentario;

                entityTC.Emprcodi = id;
                string[] TipoTASTR = null;

                if (model.Tipotastr != null)
                {
                    TipoTASTR = model.Tipotastr.Split('|');

                    foreach (var item in TipoTASTR)
                    {
                        var campos = item.Split('^');
                        switch (campos[3])
                        {
                            case "1":
                                if (campos[0] != "")
                                {
                                    cExtension = System.IO.Path.GetExtension(model.Tipodocname1.FileName);
                                    string FileNameTA1 = string.Format("{0:yyyyMMddHHmmss}{1}{2}", DateTime.Now, oAleatorio.Next(10, 100), cExtension);
                                    entityTC.Tipodocname1 = model.Tipodocname1.FileName;
                                    entityTC.Tipodocadjfilename1 = FileNameTA1;
                                    FileServer.UploadFromStream(model.Tipodocname1.InputStream, ConstantesRegistroIntegrantes.FolderRI, FileNameTA1, Path);
                                }
                                else
                                {
                                    entityTC.Tipodocname1 = campos[1];
                                    entityTC.Tipodocadjfilename1 = campos[2];
                                }
                                break;
                            case "2":
                                if (campos[0] != "")
                                {
                                    cExtension = System.IO.Path.GetExtension(model.Tipodocname2.FileName);
                                    string FileNameTA2 = string.Format("{0:yyyyMMddHHmmss}{1}{2}", DateTime.Now, oAleatorio.Next(10, 100), cExtension);
                                    entityTC.Tipodocname2 = model.Tipodocname2.FileName;
                                    entityTC.Tipodocadjfilename2 = FileNameTA2;
                                    FileServer.UploadFromStream(model.Tipodocname2.InputStream, ConstantesRegistroIntegrantes.FolderRI, FileNameTA2, Path);
                                }
                                else
                                {
                                    entityTC.Tipodocname2 = campos[1];
                                    entityTC.Tipodocadjfilename2 = campos[2];
                                }
                                break;
                            case "3":
                                if (campos[0] != "")
                                {
                                    cExtension = System.IO.Path.GetExtension(model.Tipodocname3.FileName);
                                    string FileNameTA3 = string.Format("{0:yyyyMMddHHmmss}{1}{2}", DateTime.Now, oAleatorio.Next(10, 100), cExtension);
                                    entityTC.Tipodocname3 = model.Tipodocname3.FileName;
                                    entityTC.Tipodocadjfilename3 = FileNameTA3;
                                    FileServer.UploadFromStream(model.Tipodocname3.InputStream, ConstantesRegistroIntegrantes.FolderRI, FileNameTA3, Path);
                                }
                                else
                                {
                                    entityTC.Tipodocname3 = campos[1];
                                    entityTC.Tipodocadjfilename3 = campos[2];
                                }
                                break;
                            case "4":
                                if (campos[0] != "")
                                {
                                    cExtension = System.IO.Path.GetExtension(model.Tipodocname4.FileName);
                                    string FileNameTA4 = string.Format("{0:yyyyMMddHHmmss}{1}{2}", DateTime.Now, oAleatorio.Next(10, 100), cExtension);
                                    entityTC.Tipodocname4 = model.Tipodocname4.FileName;
                                    entityTC.Tipodocadjfilename4 = FileNameTA4;
                                    FileServer.UploadFromStream(model.Tipodocname4.InputStream, ConstantesRegistroIntegrantes.FolderRI, FileNameTA4, Path);
                                }
                                else
                                {
                                    entityTC.Tipodocname4 = campos[1];
                                    entityTC.Tipodocadjfilename4 = campos[2];
                                }
                                break;
                            case "5":
                                if (campos[0] != "")
                                {
                                    cExtension = System.IO.Path.GetExtension(model.Tipodocname5.FileName);
                                    string FileNameTA5 = string.Format("{0:yyyyMMddHHmmss}{1}{2}", DateTime.Now, oAleatorio.Next(10, 100), cExtension);
                                    entityTC.Tipodocname5 = model.Tipodocname5.FileName;
                                    entityTC.Tipodocadjfilename5 = FileNameTA5;
                                    FileServer.UploadFromStream(model.Tipodocname5.InputStream, ConstantesRegistroIntegrantes.FolderRI, FileNameTA5, Path);
                                }
                                else
                                {
                                    entityTC.Tipodocname5 = campos[1];
                                    entityTC.Tipodocadjfilename5 = campos[2];
                                }
                                break;
                        }
                    }
                }


                List<SiRepresentanteDTO> Representantes = new List<SiRepresentanteDTO>();
                SiRepresentanteDTO entityRepresentante = null;
                foreach (var item in Representante)
                {
                    entityRepresentante = new SiRepresentanteDTO();
                    if (item.Rptetipo == ConstantesRegistroIntegrantes.RepresentanteTipoLegal)
                    {
                        if (item.Rptedocidentidad != null)
                        {
                            if (item.Rptedocidentidad.Length == 8)
                            {
                                entityRepresentante.Rptetipdocidentidad = ConstantesRegistroIntegrantes.TipoDocumentoIdentidadDNI;
                            }
                            else
                            {
                                entityRepresentante.Rptetipdocidentidad = ConstantesRegistroIntegrantes.TipoDocumentoIdentidadCarneExtranjeria;
                            }
                        }

                        entityRepresentante.Rptedocidentidad = item.Rptedocidentidad;

                        if (item.RpteObservacion != "")
                        {
                            cExtension = System.IO.Path.GetExtension(item.DNI.FileName);
                            string FileNameDNI = string.Format("{0:yyyyMMddHHmmss}{1}{2}", DateTime.Now, oAleatorio.Next(10, 100), cExtension);
                            entityRepresentante.Rptedocidentidadadj = item.DNI.FileName;
                            entityRepresentante.Rptedocidentidadadjfilename = FileNameDNI;
                            FileServer.UploadFromStream(item.DNI.InputStream, ConstantesRegistroIntegrantes.FolderRI, FileNameDNI, Path);

                            cExtension = System.IO.Path.GetExtension(item.VigenciaPoder.FileName);
                            string FileNameVigencia = string.Format("{0:yyyyMMddHHmmss}{1}{2}", DateTime.Now, oAleatorio.Next(10, 100), cExtension);
                            entityRepresentante.Rptevigenciapoder = item.VigenciaPoder.FileName;
                            entityRepresentante.Rptevigenciapoderfilename = FileNameVigencia;
                            FileServer.UploadFromStream(item.VigenciaPoder.InputStream, ConstantesRegistroIntegrantes.FolderRI, FileNameVigencia, Path);
                        }
                    }
                    entityRepresentante.Rptecodi = item.Id;
                    entityRepresentante.Emprcodi = id;
                    entityRepresentante.Rptetipo = item.Rptetipo;
                    entityRepresentante.Rptetiprepresentantelegal = item.Rptetiprepresentantelegal;
                    entityRepresentante.Rptebaja = ConstantesRegistroIntegrantes.RepresentanteBajaNo;
                    entityRepresentante.Rptenombres = item.Rptenombres;
                    entityRepresentante.Rpteapellidos = item.Rpteapellidos;
                    entityRepresentante.Rptecargoempresa = item.Rptecargoempresa;
                    entityRepresentante.Rptetelefono = item.Rptetelefono;
                    entityRepresentante.Rptetelfmovil = item.Rptetelfmovil;
                    entityRepresentante.Rptecorreoelectronico = item.Rptecorreoelectronico;
                    entityRepresentante.Rptefeccreacion = DateTime.Now;
                    entityRepresentante.RpteObservacion = item.RpteObservacion;
                    entityRepresentante.Rpteinicial = ConstantesRegistroIntegrantes.RepresentanteInicialSi;
                    Representantes.Add(entityRepresentante);
                }

                entity.TipoComportamiento = entityTC;
                entity.Representante = Representantes;

                string condicion = "";
                int valor = 0;
                switch (entity.Tipoemprcodi)
                {
                    case (ConstantesRegistroIntegrantes.TipoComportamientoGeneradorCodigo):
                        try
                        {
                            valor = Convert.ToInt32(entityTC.Tipopotenciainstalada);
                        }
                        catch
                        {
                            valor = 0;
                        }

                        if (valor >= ConstantesRegistroIntegrantes.EmpresaCondicionVarianteGenerador)
                            condicion = ConstantesRegistroIntegrantes.EmpresaCondicionObligatorio;
                        else
                            condicion = ConstantesRegistroIntegrantes.EmpresaCondicionVoluntario;

                        break;
                    case (ConstantesRegistroIntegrantes.TipoComportamientoTrasmisorCodigo):
                        try
                        {
                            valor = Convert.ToInt32(entityTC.Tipolineatrans138km);
                            valor += Convert.ToInt32(entityTC.Tipolineatrans220km);
                            valor += Convert.ToInt32(entityTC.Tipolineatrans500km);
                        }
                        catch
                        {
                            valor = 0;
                        }

                        if (valor >= ConstantesRegistroIntegrantes.EmpresaCondicionVarianteTransmisor)
                            condicion = ConstantesRegistroIntegrantes.EmpresaCondicionObligatorio;
                        else
                            condicion = ConstantesRegistroIntegrantes.EmpresaCondicionVoluntario;

                        break;
                    case (ConstantesRegistroIntegrantes.TipoComportamientoDistribuidorCodigo):
                        try
                        {
                            valor = Convert.ToInt32(entityTC.Tipomaxdemandacoincidente);
                        }
                        catch
                        {
                            valor = 0;
                        }

                        if (valor >= ConstantesRegistroIntegrantes.EmpresaCondicionVarianteDistribuidor)
                            condicion = ConstantesRegistroIntegrantes.EmpresaCondicionObligatorio;
                        else
                            condicion = ConstantesRegistroIntegrantes.EmpresaCondicionVoluntario;

                        break;
                    case (ConstantesRegistroIntegrantes.TipoComportamientoUsuarioLibreCodigo):
                        try
                        {
                            valor = Convert.ToInt32(entityTC.Tipomaxdemandacontratada);
                        }
                        catch
                        {
                            valor = 0;
                        }

                        if (valor >= ConstantesRegistroIntegrantes.EmpresaCondicionVarianteUsuarioLibre)
                            condicion = ConstantesRegistroIntegrantes.EmpresaCondicionObligatorio;
                        else
                            condicion = ConstantesRegistroIntegrantes.EmpresaCondicionVoluntario;

                        break;
                }

                entity.Emprcondicion = condicion;

                id = appRegistroIntegrante.SaveSIEmpresaRe(entity);

                try
                {
                    //Enviar Correo al responsable del tramite
                    var representantes = appRegistroIntegrante.GetRepresentantesByEmprcodi(id);
                    //var dtoResponsableTramite = representantes.Find(x => x.Rptetipo == ConstantesRegistroIntegrantes.RepresentanteTipoResponsableTramite && x.Rpteinicial == ConstantesRegistroIntegrantes.RepresentanteInicialSi);
                    var listadtoRepresentantes = representantes.Where(x => x.Rpteinicial == ConstantesRegistroIntegrantes.RepresentanteInicialSi);

                    foreach (var dtoRepresentante in listadtoRepresentantes)
                    {
                        string toEmail = dtoRepresentante.Rptecorreoelectronico;
                        string ccEmail = ConfigurationManager.AppSettings["admRegistroIntegrantes"];

                        string msg = string.Empty;
                        string correlativo = id.ToString();
                        string responsableTramite = dtoRepresentante.Rptenombres + ' ' + dtoRepresentante.Rpteapellidos;
                        //Garantia 9Jul18 - nro 2 si no existe nombre debe mostrarse razon social
                        string empresa = (model.Emprnombrecomercial.Length > 3) ? model.Emprnombrecomercial : model.Emprrazsocial;
                        //fin Garantia 9Jul18 

                        msg = RegistroIntegrantesHelper.Registro_BodyMailAceptado(responsableTramite, empresa, correlativo);

                        log.Info("Inscripción - Registro - Envío de correo al resposable tramite");
                        COES.Base.Tools.Util.SendEmail(toEmail, ccEmail, "Notificacion - Registro de Integrantes", msg);
                    }

                }
                catch (Exception exCorreo)
                {
                    log.Error(exCorreo);
                }

            }

            catch (Exception ex)
            {
                log.Error(ex);
                id = 0;
            }

            jRespuesta = Json(id, JsonRequestBehavior.AllowGet);
            return jRespuesta;
        }

        /// <summary>
        /// Edita el formulario de registro de integrantes
        /// </summary>
        /// <param name="model">model IntegranteModel</param>
        /// <returns></returns>
        public JsonResult Editar(IntegranteModel model)
        {
            JsonResult jRespuesta;

            jRespuesta = Json(null, JsonRequestBehavior.AllowGet);
            return jRespuesta;
        }

        /// <summary>
        /// Bajar archivo
        /// </summary>
        /// <param name="url">ruta del archivo</param>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Download(string url)
        {
            string[] par = url.Split('*');

            Stream str = FileServer.DownloadToStream(ConstantesRegistroIntegrantes.FolderRI + par[1], ConstantesRegistroIntegrantes.FolderUploadRutaCompleta + ConstantesRegistroIntegrantes.FolderRI);
            return File(str, "application/pdf", par[0]);
        }


        /// <summary>
        /// Exporta a PDF la constancia de registro
        /// </summary>
        /// <param name="id">codigo de empresa</param>
        /// <returns></returns>
        public FileResult ExportarPDF(int id)
        {
            byte[] bytes = new byte[0];
            string FileName = "Error.pdf";
            int nroConstancia = 0;
            try
            {
                bytes = appRegistroIntegranteReporte.ExportarPDF(id, out nroConstancia);
                FileName = string.Format("Empresa-{0}", nroConstancia);
            }
            catch
            {
                bytes = null;
            }
            return File(bytes, "application/pdf", FileName + ".pdf");
        }

        /// <summary>
        /// Permite obtener los datos de SUNAT
        /// </summary>
        /// <param name="ruc"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult obtenerdatos(string ruc)
        {
            try
            {
                BeanEmpresa empresa = this.servicio.ConsultarPorRUC(ruc);

                if (empresa == null)
                {
                    return Json(-2); //- RUC no Existe            
                }
                else
                {
                    if (string.IsNullOrEmpty(empresa.RUC))
                    {
                        return Json(-2); //- RUC no Existe            
                    }
                    else
                    {

                        if (empresa.Estado.ToUpper().Trim() != "ACTIVO")
                            return Json(-3); //- RUC de Baja            
                        else
                            return Json(empresa);
                    }
                }
            }
            catch
            {
                return Json(-1); //- Error en el proceso
            }
        }


        #region funciones

        /// <summary>
        /// Permite validar el capcha con el site de google
        /// </summary>
        /// <param name="response">response de la pagina para validar</param>
        /// <returns></returns>
        public bool ValidarNoSoyRobot(string response)
        {
            //Validate Google recaptcha here
            //var response = Request["g-recaptcha-response"];
            string secretKey = ConfigurationManager.AppSettings["secretKey"].ToString();
            var client = new WebClient();
            var result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));
            var obj = JObject.Parse(result);
            var status = (bool)obj.SelectToken("success");

            return status;
        }

        /// <summary>
        /// Permite validar que el ruc y el nombre no existan
        /// </summary>
        /// <param name="IntegranteModel">model con los datos</param>
        /// <returns></returns>
        public bool ValidacionEmpresa(IntegranteModel model)
        {
            return this.servicio.ValidaEmpresa(model.Emprnomb, model.Emprruc);
        }

        #endregion
    }
}