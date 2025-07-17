using COES.MVC.Extranet.Areas.Account.Helper;
using COES.MVC.Extranet.Areas.Account.Models;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Helper;
using COES.MVC.Extranet.SeguridadServicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.Account.Controllers
{
    public class SolicitarController : BaseController
    {
        /// <summary>
        /// Referencia al servicio web de seguridad
        /// </summary>
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();

        /// <summary>
        /// Usurio del representante
        /// </summary>
        public string UserRepresentabte
        {
            get 
            {
                if (Session[DatosSesion.UserRepresentante] != null)
                {
                    return Session[DatosSesion.UserRepresentante].ToString();
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// Lista de los modulos
        /// </summary>
        public List<int> ListaModulos
        {
            get
            {
                return (Session[ConstantesAdmin.SesionModulos] != null) ?
                (List<int>)Session[ConstantesAdmin.SesionModulos] : new List<int>();
            }
            set 
            {
                Session[ConstantesAdmin.SesionModulos] = value;
            }
        }

        /// <summary>
        /// Muestra la pantalla inicial del modulo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            SolicitudUsuarioModel model = new SolicitudUsuarioModel();
          
            EmpresaDTO empresa = this.seguridad.ObtenerEmpresa(base.EmpresaId);
            model.EmpresaNombre = empresa.EMPRNOMB;
            model.EmpresaId = empresa.EMPRCODI;

            /***INC 2024 - 001669 - Deshabilitar el acceso a la vista***/
            //return View(model);
            return RedirectToAction("NotFound", "Error");

        }

        /// <summary>
        /// Permite mostrar la vista de nuevas solicitudes
        /// </summary>
        /// <returns></returns>
        public ActionResult Nuevo(int id)
        {
            SolicitudUsuarioModel model = new SolicitudUsuarioModel();            
            EmpresaDTO empresa = this.seguridad.ObtenerEmpresa(base.EmpresaId);
            model.EmpresaNombre = empresa.EMPRNOMB;
            model.EmpresaId = empresa.EMPRCODI;

            if (id > 0)
            {
                model.Entidad = this.seguridad.ObtenerSolicitudExtranet(id);
                model.IdSolicitud = id;
                model.TipoSolicitud = model.Entidad.SolicTipo;
            }
            else
            {
                model.Entidad = new SolicitudExtDTO();
                model.IdSolicitud = 0;
                model.TipoSolicitud = string.Empty;
            }

            return View(model);
        }

        /// <summary>
        /// Lista las solicitudes por empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Grilla(int idEmpresa, string estado)
        {
            SolicitudUsuarioModel model = new SolicitudUsuarioModel();
            model.ListaSolicitud = this.seguridad.ListarSolicituExtranet(idEmpresa, estado).ToList();           
            return PartialView(model);
        }

        /// <summary>
        /// Permite anular la solicitud
        /// </summary>
        /// <param name="idSolicitud"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AnularSolicitud(int idSolicitud)
        {
            try
            {
                this.seguridad.AprobarSolicitudExtranet(idSolicitud, this.UserRepresentabte, ConstantesAdmin.EstadoAnulado);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }


        /// <summary>
        /// Permite obtener el header de la grilla
        /// </summary>
        /// <param name="cadena"></param>
        /// <returns></returns>
        protected string ObtenerHeader(string cadena)
        {
            string resultado = string.Empty;
            for (int i = 0; i < cadena.Length; i++)
            {
                resultado = resultado + (cadena[i]).ToString().ToUpper();
                if (i != cadena.Length - 1) { resultado = resultado + "<br />"; }
            }

            return resultado;
        }
        
        /// <summary>
        /// Permite pintar la grilla en handson
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PintarExcel(int idEmpresa, int idSolicitud)
        {
            ExcelModel model = new ExcelModel();
            List<ModuloDTO> listModulo = this.seguridad.ListarModulos().ToList();
            List<int> listIdModulos = listModulo.OrderBy(x=>x.ModNombre).Select(x => (int)x.ModCodi).ToList();
            this.ListaModulos = listIdModulos;
            model.Headers = listModulo.OrderBy(x=>x.ModNombre).Select(x => this.ObtenerHeader(x.ModNombre) ).ToArray();
            model.Widths = listModulo.Select(x => 40).ToArray();
            string[][] data = new string[15][];

            for (int i = 0; i < data.Length; i++ )
            {
                data[i] = new string[3 + listModulo.Count];
                
                if (i == 0) {
                    data[i][0] = "Datos del Uuuario";
                    data[i][3] = "Módulos solicitados";
                }
                else if (i == 1){
                    data[i][0] = "Nombres";
                    data[i][1] = "Correo";
                    data[i][2] = "Teléfono";

                    int k = 0;
                    foreach (string item in model.Headers)
                    {
                        data[i][k + 3] = item;
                        k++;
                    }
                }
                else
                {
                    data[i][0] = "";
                    data[i][1] = "";
                    data[i][2] = "";
                    for (int j = 2; j < data[i].Length; j++)
                    {
                        data[i][j] = "";
                    }
                }
            }

            model.Data = data;

            object[] configuracion = new object[model.Headers.Length];

            int index = 0;
            foreach (string columna in model.Headers)
            {
                configuracion[index] = new
                {
                    type = "text",                    
                    readOnly = false
                };

                index++;
            }

            model.Columnas = configuracion; 

            return Json(model);
        }

        /// <summary>
        /// Muestra la grilla de edición
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PintarExcelEdicion(int idEmpresa, int idSolicitud)
        {
            ExcelModel model = new ExcelModel();
            List<ModuloDTO> listModulo = this.seguridad.ListarModulos().ToList();
            List<int> listIdModulos = listModulo.OrderBy(x => x.ModNombre).Select(x => (int)x.ModCodi).ToList();
            this.ListaModulos = listIdModulos;
            model.Headers = listModulo.OrderBy(x => x.ModNombre).Select(x => this.ObtenerHeader(x.ModNombre)).ToArray();
            model.Widths = listModulo.Select(x => 40).ToArray();

            List<UserDTO> listUsuarios = this.seguridad.ListarUsuariosPorEmpresa(idEmpresa).
                 Where(x => x.UserState == ConstantesAdmin.EstadoActivo).ToList();

            string[][] data = new string[2 + listUsuarios.Count][];


            data[0] = new string[3 + listModulo.Count];
            data[1] = new string[3 + listModulo.Count];


            data[0][0] = "Datos del Uuuario";
            data[0][3] = "Módulos solicitados";

            data[1][0] = "Nombres";
            data[1][1] = "Correo";
            data[1][2] = "Teléfono";

            int k = 0;
            foreach (string item in model.Headers)
            {
                data[1][k + 3] = item;
                k++;
            }

            int i = 2;
            foreach (UserDTO item in listUsuarios)
            {
                data[i] = new string[3 + listModulo.Count];
                data[i][0] = item.UsernName;
                data[i][1] = item.UserLogin;
                data[i][2] = item.UserTlf;

                List<int> idModuloSelected = this.seguridad.ObtenerModulosPorUsuarioSelecion(item.UserCode).
                    Where(x => x.Selected > 0).Select(x => (int)x.ModCodi).ToList();

                for (int j = 0; j < listIdModulos.Count; j++)
                {
                    if (idModuloSelected.Contains(listIdModulos[j]))
                    {
                        data[i][3 + j] = "X";
                    }
                    else 
                    {
                        data[i][3 + j] = "";
                    }
                }
                i++;
            }
            
            model.Data = data;
            object[] configuracion = new object[model.Headers.Length];

            int index = 0;
            foreach (string columna in model.Headers)
            {
                configuracion[index] = new
                {
                    type = "text",
                    readOnly = false
                };

                index++;
            }

            model.Columnas = configuracion;

            return Json(model);
        }

        /// <summary>
        /// Permite mostrar la grilla de visualizacion
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idSolicitud"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EdicionExcel(int idEmpresa, int idSolicitud)
        {
            ExcelModel model = new ExcelModel();
            List<ModuloDTO> listModulo = this.seguridad.ListarModulos().ToList();
            List<int> listIdModulos = listModulo.OrderBy(x => x.ModNombre).Select(x => (int)x.ModCodi).ToList();
            this.ListaModulos = listIdModulos;
            model.Headers = listModulo.OrderBy(x => x.ModNombre).Select(x => this.ObtenerHeader(x.ModNombre)).ToArray();
            model.Widths = listModulo.Select(x => 40).ToArray();

            SolicitudExtDTO solicitud = this.seguridad.ObtenerDetalleSolicitudExtranet(idSolicitud);

            List<SolicitudDetalleDTO> listUsuarios = solicitud.ListaUsuarios.ToList();

            string[][] data = new string[2 + listUsuarios.Count][];


            data[0] = new string[3 + listModulo.Count];
            data[1] = new string[3 + listModulo.Count];


            data[0][0] = "Datos del Uuuario";
            data[0][3] = "Módulos solicitados";

            data[1][0] = "Nombres";
            data[1][1] = "Correo";
            data[1][2] = "Teléfono";

            int k = 0;
            foreach (string item in model.Headers)
            {
                data[1][k + 3] = item;
                k++;
            }

            int i = 2;
            foreach (SolicitudDetalleDTO item in listUsuarios)
            {
                data[i] = new string[3 + listModulo.Count];
                data[i][0] = item.UserName;
                data[i][1] = item.UserEmail;
                data[i][2] = item.UserTlf;

                List<int> idModuloSelected = item.ListaModulos.Select(x => (int)x.ModCodi).ToList();

                for (int j = 0; j < listIdModulos.Count; j++)
                {
                    if (idModuloSelected.Contains(listIdModulos[j]))
                    {
                        data[i][3 + j] = "X";
                    }
                }
                i++;
            }

            model.Data = data;
            object[] configuracion = new object[model.Headers.Length];

            int index = 0;
            foreach (string columna in model.Headers)
            {
                configuracion[index] = new
                {
                    type = "text",
                    readOnly = false
                };

                index++;
            }

            model.Columnas = configuracion;

            return Json(model);
        }

        /// <summary>
        /// Permite grabar los metadatos ingresados
        /// </summary>
        /// <param name="baseDirectory"></param>
        /// <param name="url"></param>
        /// <param name="datos"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarSolicitud(string[][] datos, int idEmpresa, string indicador)
        {
            try
            {
                SolicitudExtDTO solicitud = new SolicitudExtDTO();
                solicitud.SolicEstado = ConstantesAdmin.EstadoPendiente;
                solicitud.SolicFecha = DateTime.Now;
                solicitud.SolicTipo = indicador;
                solicitud.UserCreate = this.UserRepresentabte;
                solicitud.LastDate = DateTime.Now;
                solicitud.LastUser = this.UserRepresentabte;
                solicitud.EmprCodi = (short)idEmpresa;

                List<SolicitudDetalleDTO> listDetalle = new List<SolicitudDetalleDTO>();

                for (int i = 0; i < datos.Length; i++)
                {
                    if (datos[i][0] != string.Empty)
                    {
                        SolicitudDetalleDTO detalle = new SolicitudDetalleDTO();
                        detalle.UserName = datos[i][0];
                        detalle.UserEmail = datos[i][1];
                        detalle.UserTlf = datos[i][2];

                        List<SolicitudModuloDTO> listModulo = new List<SolicitudModuloDTO>();

                        for (int j = 3; j < datos[i].Length; j++)
                        {
                            if (datos[i][j] != string.Empty)
                            {
                                SolicitudModuloDTO modulo = new SolicitudModuloDTO();
                                modulo.ModCodi = (short)this.ListaModulos[j - 3];                                
                                listModulo.Add(modulo);
                            }
                        }

                        detalle.ListaModulos = listModulo.ToArray();
                        listDetalle.Add(detalle);
                    }
                }

                solicitud.ListaUsuarios = listDetalle.ToArray();

                int resultado = this.seguridad.GrabarSolicitudExtranet(solicitud);

                return Json(resultado);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite generar las solicitudes de baja de usuarios
        /// </summary>
        /// <param name="usuarios"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarBaja(string usuarios, int idEmpresa)
        {
            try
            {
                SolicitudExtDTO solicitud = new SolicitudExtDTO();
                solicitud.SolicEstado = ConstantesAdmin.EstadoPendiente;
                solicitud.SolicFecha = DateTime.Now;
                solicitud.SolicTipo = ConstantesAdmin.TipoBajaUsuario;
                solicitud.UserCreate = this.UserRepresentabte;
                solicitud.LastDate = DateTime.Now;
                solicitud.LastUser = this.UserRepresentabte;
                solicitud.EmprCodi = (short)idEmpresa;
                string users = usuarios.Remove(usuarios.Length - 1, 1);

                List<SolicitudDetalleDTO> listDetalle = new List<SolicitudDetalleDTO>();
                List<int> listUsuario = users.Split(',').Select(int.Parse).ToList();

                for (int i = 0; i < listUsuario.Count; i++)
                {
                    SolicitudDetalleDTO detalle = new SolicitudDetalleDTO();
                    detalle.UserCode = (short)listUsuario[i];
                    detalle.ListaModulos = (new List<SolicitudModuloDTO>()).ToArray();
                    listDetalle.Add(detalle);
                }

                solicitud.ListaUsuarios = listDetalle.ToArray();
                int resultado = this.seguridad.GrabarSolicitudExtranet(solicitud);

                return Json(resultado);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite listar los usuarios para ser dados de baja
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Usuarios(int idEmpresa, int idSolicitud)
        {
            RepresentanteModel model = new RepresentanteModel();

            if (idSolicitud == 0)
            {
                model.ListaUsuarios = this.seguridad.ListarUsuariosPorEmpresa(idEmpresa).
                    Where(x => x.UserState == ConstantesAdmin.EstadoActivo).ToList();
            }
            else
            {
                model.ListaUsuarios = this.seguridad.ListarUsuarioSolicitudBaja(idSolicitud).ToList();   
            }

            model.IdSolicitud = idSolicitud;

            return PartialView(model);
        }

    }

}
