using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Eventos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Servicios.Aplicacion.PruebasAleatorias;
using COES.MVC.Intranet.Helper;
using System.Globalization;
using COES.Framework.Base.Core;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.Helper;

namespace COES.MVC.Intranet.Areas.Eventos.Controllers
{
    public class PruebasAleatoriasController : BaseController
    {
        PruebasAleatoriasAppServicio servicio = new PruebasAleatoriasAppServicio();

        public ActionResult Index()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            BusquedaPAleatoriasModel model = new BusquedaPAleatoriasModel();
            model.isBotonNuevoHabilitado = DateTime.Now.Hour >= 14 ? 1 : 0;
            model.FechaIni = DateTime.Now.AddDays(-30).ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.AccionNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
            model.AccionEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.AccionEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);
            return View(model);
        }



        /// <summary>
        /// Permite convertir una fecha en formato DDMMYYYY a tipo fecha
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DateTime FechaConsulta(int id)
        {
            string fechaCad = id.ToString();
            string anio = fechaCad.Substring(0, 4);
            string mes = fechaCad.Substring(4, 2);
            string dia = fechaCad.Substring(6, 2);
            DateTime fecha = DateTime.ParseExact(dia + "/" + mes + "/" + anio, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            return fecha;
        }



        /// <summary>
        /// Editar registro con parámetro
        /// </summary>
        /// <param name="id">id de Pruebas aleatorias</param>
        /// <param name="accion">0: ver, 1: editar</param>
        /// <returns>modelo</returns>
        public ActionResult Editar(int id, int accion)
        {
            DateTime fecha;

            if (id == 0)
                fecha = FechaConsulta(Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")));
            else
                fecha = FechaConsulta(id);

            EvePAleatoriasModel model = new EvePAleatoriasModel();

            model.ListaProgramador = this.servicio.ListarProgramador();

            EvePaleatoriaDTO paleatoria = this.servicio.GetByIdEvePaleatoria(fecha);

            if (paleatoria == null)
            {
                //crear prueba aleatoria
                paleatoria = new EvePaleatoriaDTO();
                paleatoria.Pafecha = fecha;
                paleatoria.Final = "N";
                paleatoria.Lastuser = User.Identity.Name;
                paleatoria.Lastdate = DateTime.Now;
                servicio.SaveEvePaleatoria(paleatoria);
            }

            model.Sic2hop = ((paleatoria.Sic2hop == null) ? "N" : paleatoria.Sic2hop);
            model.Hop2ut30d = ((paleatoria.Hop2ut30d == null) ? "N" : paleatoria.Hop2ut30d);
            model.Ut30d2sort = ((paleatoria.Ut30d2sort == null) ? "N" : paleatoria.Ut30d2sort);
            model.Sort2prue = ((paleatoria.Sort2prue == null) ? "N" : paleatoria.Sort2prue);
            model.Prueno2pa = ((paleatoria.Prueno2pa == null) ? "N" : paleatoria.Prueno2pa);
            model.Pa2fin = ((paleatoria.Pa2fin == null) ? "N" : paleatoria.Pa2fin);
            model.Pruesi2gprue = ((paleatoria.Pruesi2gprue == null) ? "N" : paleatoria.Pruesi2gprue);
            model.Gprueno2nprue = ((paleatoria.Gprueno2nprue == null) ? "N" : paleatoria.Gprueno2nprue);
            model.Nprue2fin = ((paleatoria.Nprue2fin == null) ? "N" : paleatoria.Nprue2fin);
            model.Gpruesi2uprue = ((paleatoria.Gpruesi2uprue == null) ? "N" : paleatoria.Gpruesi2uprue);
            model.Uprue2rprue = ((paleatoria.Uprue2rprue == null) ? "N" : paleatoria.Uprue2rprue);
            model.Rprue2oa = ((paleatoria.Rprue2oa == null) ? "N" : paleatoria.Rprue2oa);
            model.Oa2priarr = ((paleatoria.Oa2priarr == null) ? "N" : paleatoria.Oa2priarr);
            model.Priarrsi2exit = ((paleatoria.Priarrsi2exit == null) ? "N" : paleatoria.Priarrsi2exit);
            model.Priarrno2rearr = ((paleatoria.Priarrno2rearr == null) ? "N" : paleatoria.Priarrno2rearr);
            model.Rearrno2noexit = ((paleatoria.Rearrno2noexit == null) ? "N" : paleatoria.Rearrno2noexit);
            model.Rearrsi2segarr = ((paleatoria.Rearrsi2segarr == null) ? "N" : paleatoria.Rearrsi2segarr);
            model.Segarrno2noexit = ((paleatoria.Segarrno2noexit == null) ? "N" : paleatoria.Segarrno2noexit);
            model.Segarrsi2exit = ((paleatoria.Segarrsi2exit == null) ? "N" : paleatoria.Segarrsi2exit);
            model.Exitno2fallunid = ((paleatoria.Exitno2fallunid == null) ? "N" : paleatoria.Exitno2fallunid);
            model.Fallunidsi2noexit = ((paleatoria.Fallunidsi2noexit == null) ? "N" : paleatoria.Fallunidsi2noexit);
            model.Exitsi2resprue = ((paleatoria.Exitsi2resprue == null) ? "N" : paleatoria.Exitsi2resprue);
            model.Fallunidno2pabort = ((paleatoria.Fallunidno2pabort == null) ? "N" : paleatoria.Fallunidno2pabort);
            model.Pabort2resprue = ((paleatoria.Pabort2resprue == null) ? "N" : paleatoria.Pabort2resprue);
            model.Resprue2fin = ((paleatoria.Resprue2fin == null) ? "N" : paleatoria.Resprue2fin);
            model.Noexit2resreslt = ((paleatoria.Noexit2resreslt == null) ? "N" : paleatoria.Noexit2resreslt);
            model.Resreslt2fin = ((paleatoria.Resreslt2fin == null) ? "N" : paleatoria.Resreslt2fin);
            model.Final = ((paleatoria.Final == null) ? "N" : paleatoria.Final);

            model.Programador = ((paleatoria.Programador == null) ? "0" : paleatoria.Programador);
            model.Paobservacion = ((paleatoria.Paobservacion == null) ? "" : paleatoria.Paobservacion);
            model.Paverifdatosing = ((paleatoria.Paverifdatosing == null) ? "N" : paleatoria.Paverifdatosing);

            model.Pafecha = paleatoria.Pafecha.ToString("yyyyMMdd");
            model.Accion = accion;

            return View(model);
        }



        /// <summary>
        /// Permite actualizar el paso inicial del flujograma
        /// </summary>
        /// <param name="id">identificador</param>
        /// <param name="paso">Nombre de paso</param>
        /// <param name="programador">Programador responsable de la prueba aleatoria</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActualizarPasoInicial(int id, string paso, string programador)
        {

            DateTime fecha = FechaConsulta(id);
            paso = paso.ToUpper();

            EvePaleatoriaDTO paleatoria = this.servicio.GetByIdEvePaleatoria(fecha);

            if (paleatoria.Final == "S")
                return Json("-1");

            switch (paso)
            {
                case "SIC2HOP":
                    //predecesor
                    //actualizar BD
                    paleatoria.Programador = programador;
                    paleatoria.Sic2hop = "S";
                    paleatoria.Lastuser = User.Identity.Name;
                    paleatoria.Lastdate = DateTime.Now;
                    servicio.UpdateEvePaleatoria(paleatoria);
                    return Json("");
                    break;
            }

            return Json("-1");
        }



        /// <summary>
        /// Permite actualizar un paso del flujograma
        /// </summary>
        /// <param name="id">Fecha</param>
        /// <param name="paso">Paso del flujograma</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActualizarPaso(int id, string paso)
        {

            DateTime fecha = FechaConsulta(id);
            paso = paso.ToUpper();

            EvePaleatoriaDTO paleatoria = this.servicio.GetByIdEvePaleatoria(fecha);

            if (paleatoria.Final == "S")
                return Json("-1");

            switch (paso)
            {
                case "HOP2UT30D":
                    //predecesor
                    paleatoria.Hop2ut30d = "S";
                    break;
                case "UT30D2SORT": paleatoria.Ut30d2sort = "S"; break;

                case "UPRUE2RPRUE": paleatoria.Uprue2rprue = "S"; break;
                case "RPRUE2OA": paleatoria.Rprue2oa = "S"; break;
                case "PABORT2RESPRUE": paleatoria.Pabort2resprue = "S"; break;
                case "SORT2PRUE": paleatoria.Sort2prue = "S"; break;
                case "OA2PRIARR": paleatoria.Oa2priarr = "S"; break;

                case "PRUENO2PA": paleatoria.Prueno2pa = "S"; break;
                case "GPRUESI2UPRUE": paleatoria.Gpruesi2uprue = "S"; break;
                case "REARRNO2NOEXIT": paleatoria.Rearrno2noexit = "S"; break;
                case "SEGARRNO2NOEXIT": paleatoria.Segarrno2noexit = "S"; break;
                case "FALLUNIDSI2NOEXIT": paleatoria.Fallunidsi2noexit = "S"; break;


                case "EXITSI2RESPRUE": paleatoria.Exitsi2resprue = "S"; break;
                case "FALLUNIDNO2PABORT": paleatoria.Fallunidno2pabort = "S"; break;
                case "PA2FIN": paleatoria.Pa2fin = "S"; paleatoria.Final = "S"; break;
                case "NPRUE2FIN": paleatoria.Nprue2fin = "S"; paleatoria.Final = "S"; break;
                case "RESRESLT2FIN": paleatoria.Resreslt2fin = "S"; paleatoria.Final = "S"; break;

                case "PRUESI2GPRUE": paleatoria.Pruesi2gprue = "S"; break;
                case "GPRUENO2NPRUE": paleatoria.Gprueno2nprue = "S"; break;
                case "PRIARRSI2EXIT": paleatoria.Priarrsi2exit = "S"; break;
                case "PRIARRNO2REARR": paleatoria.Priarrno2rearr = "S"; break;
                case "REARRSI2SEGARR": paleatoria.Rearrsi2segarr = "S"; break;


                case "EXITNO2FALLUNID": paleatoria.Exitno2fallunid = "S"; break;
                case "SEGARRSI2EXIT": paleatoria.Segarrsi2exit = "S"; break;
                case "RESPRUE2FIN": paleatoria.Resprue2fin = "S"; break;
                case "NOEXIT2RESRESLT": paleatoria.Noexit2resreslt = "S"; break;
                case "PAVERIFDATOSING": paleatoria.Paverifdatosing = "S"; paleatoria.Final = "S"; break;

                default:
                    break;

            }


            paleatoria.Lastuser = User.Identity.Name;
            paleatoria.Lastdate = DateTime.Now;
            servicio.UpdateEvePaleatoria(paleatoria);
            return Json("");


        }



        /// <summary>
        /// Permite grabar la observación del flujograma
        /// </summary>
        /// <param name="id"></param>
        /// <param name="observacion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarObservacion(int id, string observacion)
        {

            try
            {
                DateTime fecha = FechaConsulta(id);
                EvePaleatoriaDTO paleatoria = this.servicio.GetByIdEvePaleatoria(fecha);

                //actualizar BD
                paleatoria.Paobservacion = observacion;
                paleatoria.Lastuser = User.Identity.Name;
                paleatoria.Lastdate = DateTime.Now;

                servicio.UpdateEvePaleatoria(paleatoria);

                return Json("");

            }
            catch
            {
                return Json("-1");
            }
        }



        /// <summary>
        /// Listado de Pruebas aleatorias según Fecha inicial, Fecha final y número de página
        /// </summary>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <param name="nroPage">Nro. de página</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(string fechaIni, string fechaFin, int nroPage)
        {
            BusquedaPAleatoriasModel model = new BusquedaPAleatoriasModel();
            DateTime fechaInicio = DateTime.Now.AddDays(-30);
            DateTime fechaFinal = DateTime.Now;

            if (fechaIni != null)
            {
                fechaInicio = DateTime.ParseExact(fechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            if (fechaFin != null)
            {
                fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }


            model.FechaIni = fechaInicio.ToString(Constantes.FormatoFecha);
            model.FechaFin = fechaFinal.ToString(Constantes.FormatoFecha);

            model.ListaPAleatoria =
                servicio.BuscarOperaciones(fechaInicio, fechaFinal, nroPage, Constantes.PageSizeEvento).ToList();

            model.AccionEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.AccionEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);

            return PartialView(model);
        }



        /// <summary>
        /// Permite mostrar el paginado de una lista
        /// </summary>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns>modelo de paginado</returns>
        [HttpPost]
        public PartialViewResult Paginado(string fechaIni, string fechaFin)
        {
            Paginacion model = new Paginacion();
            DateTime fechaInicio = DateTime.Now.AddDays(-30);
            DateTime fechaFinal = DateTime.Now;

            if (fechaIni != null)
            {
                fechaInicio = DateTime.ParseExact(fechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (fechaFin != null)
            {
                fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            int nroRegistros = servicio.ObtenerNroFilas(fechaInicio, fechaFinal, Constantes.NroPageShow,
                Constantes.PageSizeEvento);

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



        /// <summary>
        /// Permite eliminar el registro de pruebas aleatorias
        /// </summary>
        /// <param name="id">Fecha de Prueba aleatoria</param>
        /// <returns>1: proceso correcto. -1: si hay error</returns>
        [HttpPost]
        public JsonResult Eliminar(int id)
        {
            DateTime fecha = FechaConsulta(id);

            try
            {
                (new PruebasAleatoriasAppServicio()).Eliminar(fecha);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }


    }



}
