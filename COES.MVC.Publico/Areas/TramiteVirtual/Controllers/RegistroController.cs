using COES.Dominio.DTO.Sic;
using COES.MVC.Publico.Areas.TramiteVirtual.Models;
using COES.Servicios.Aplicacion.General;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Publico.Areas.TramiteVirtual.Controllers
{
    public class RegistroController : Controller
    {
        // GET: TramiteVirtual/Registro
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Principal()
        {
            return View();
        }

        /// <summary>
        /// Permite obtener los datos de SUNAT
        /// </summary>
        /// <param name="ruc"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerDatos(string ruc)
        {
            int resultado = 1;
            BeanEmpresa bean = null;

            try
            {
               
                bool flagValido = (new EmpresaAppServicio()).ValidarRuc(ruc);

                if (flagValido)
                {

                    bool flag = (new TramiteVirtualAppServicio()).ValidarAccesoEmpresaCyDOC(ruc);

                    if (!flag)
                    {
                        try
                        {
                            bean = (new EmpresaAppServicio()).ConsultarPorRUC(ruc);

                            if (bean == null)
                            {
                                resultado = 3; //- Empresa no existen en sunat
                            }
                            else
                            {
                                resultado = 1; //- Empresa correcta
                            }
                        }
                        catch
                        {
                            SiEmpresaDTO entity = (new EmpresaAppServicio()).ObtenerEmpresaPorRUC(ruc);

                            if (entity != null)
                            {
                                bean = new BeanEmpresa
                                {
                                    RazonSocial = entity.Emprrazsocial,
                                    NombreComercial = !string.IsNullOrEmpty(entity.Emprnombcomercial) ? entity.Emprnombcomercial : entity.Emprnomb
                                };

                                resultado = 6; //- Empresa existente en base de datos del COES
                            }
                            else
                            {
                                resultado = 4; //-Ingresar los datos de la empresa
                            }
                        }
                    }
                    else
                    {
                        resultado = 2; //- Empresa ya cuenta con acceso
                    }
                }
                else
                {
                    resultado = 5; //- ruc no valido
                }              
            }
            catch 
            {
                resultado = -1 ; //- Ha habido 
            }

            return Json(new { Resultado = resultado, Entidad = bean });
        }

        /// <summary>
        /// Permite grabar los datos del representante
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult GrabarDatos(RegistroModel model)
        {
            try
            {
                if (model != null) { if (model.NumeroRUC.Length != 11) return Json(-2); }

                SiEmpresaDTO entity = new SiEmpresaDTO
                {
                    Emprruc = model.NumeroRUC,
                    Emprrazsocial = model.RazonSocial.ToUpper(),
                    Emprnomb = model.NombreComercial.ToUpper(),
                    Emprdomiciliolegal = model.DireccionFiscal.ToUpper()
                };

                //this line create a new TextInfo based on en-US culture
                TextInfo info = new CultureInfo("en-US", false).TextInfo;

                SiRepresentanteDTO representante = new SiRepresentanteDTO
                {
                    Rptenombres = info.ToTitleCase(model.NombreRepresentante.ToLower()),
                    Rpteapellidos = info.ToTitleCase(model.ApellidoRepresentante.ToLower()),
                    Rptecargoempresa = info.ToTitleCase(model.CargorRepresentante.ToLower()),
                    Rptecorreoelectronico = model.CorreoRepresentante.ToLower(),
                    Rptetelefono = model.TelefonoRepresentante,
                    Rptetelfmovil = model.MovilRepresentante,
                    Rptetiprepresentantelegal = model.TipoRepresentacion
                };

                SiEmpresaCorreoDTO contacto1 = new SiEmpresaCorreoDTO
                {
                    Empcornomb = info.ToTitleCase((model.NombreContacto + " " + model.ApellidoContacto).ToLower()),
                    Empcorcargo = info.ToTitleCase(model.CargoContacto.ToLower()),
                    Empcoremail = model.CorreoContacto.ToLower(),
                    Empcortelefono = model.TelefonoContacto,
                    Empcormovil = model.MovilContacto
                };

                SiEmpresaCorreoDTO contacto2 = null;
                SiEmpresaCorreoDTO contacto3 = null;

                if (!string.IsNullOrEmpty(model.CorreoContacto1))
                {

                    contacto2 = new SiEmpresaCorreoDTO
                    {
                        Empcornomb = info.ToTitleCase((model.NombreContacto1 + " " + model.ApellidoContacto1).ToLower()),
                        Empcorcargo = info.ToTitleCase(model.CargoContacto1.ToLower()),
                        Empcoremail = model.CorreoContacto1.ToLower(),
                        Empcortelefono = model.TelefonoContacto1,
                        Empcormovil = model.MovilContacto1
                    };
                }

                if (!string.IsNullOrEmpty(model.CorreoContacto2))
                {

                    contacto3 = new SiEmpresaCorreoDTO
                    {
                        Empcornomb = info.ToTitleCase((model.NombreContacto2 + " " + model.ApellidoContacto2).ToLower()),
                        Empcorcargo = info.ToTitleCase(model.CargoContacto2.ToLower()),
                        Empcoremail = model.CorreoContacto2.ToLower(),
                        Empcortelefono = model.TelefonoContacto2,
                        Empcormovil = model.MovilContacto2
                    };

                }

                int result = (new TramiteVirtualAppServicio()).GrabarSolicitudNoIntegrante(entity,
                    representante, contacto1, contacto2, contacto3, "usuarioweb");

                return Json(1);
            }
            catch (Exception ex)
            {
                return Json(-1);
            }
        }

    }
}