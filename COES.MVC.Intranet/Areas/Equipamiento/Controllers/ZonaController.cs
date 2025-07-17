using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.MVC.Intranet.Areas.Equipamiento.Models;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Dominio.DTO.Sic;
using log4net;

namespace COES.MVC.Intranet.Areas.Equipamiento.Controllers
{
    public class ZonaController : Controller
    {
        //
        // GET: /Equipamiento/Zona/
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(ZonaController));
        EquipamientoAppServicio Equipamientoservicio = new EquipamientoAppServicio();

        /// <summary>
        /// Carga la primera pantalla
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ZonaModel model = new ZonaModel();
            model.ListaNiveles = this.Equipamientoservicio.ListEqAreanivels();
            return View(model);
        }

        
        [HttpPost]
        public PartialViewResult ListaZonas()
        {
            ListaZonasModel model = new ListaZonasModel();
            model.ListaZonas = this.Equipamientoservicio.ListarZonasActivas();
            return PartialView(model);
        }

        [HttpPost]
        public PartialViewResult ListaZonasxNivel(int anivelcodi)
        {
            ListaZonasModelxNivel model = new ListaZonasModelxNivel();
            model.ListaZonasxNivel = this.Equipamientoservicio.ListarZonasxNivel(anivelcodi);
            return PartialView(model);
        }

        [HttpPost]
        public PartialViewResult DetalleZona(int areacodi)
        {
            var detalleZona = Equipamientoservicio.GetByIdEqArea(areacodi);
            var ZonaNombANivel = Equipamientoservicio.GetByIdEqAreanivel(detalleZona.ANivelCodi);
            detalleZona.ANivelNomb = ZonaNombANivel.ANivelNomb;
            var listaAreasDeZona = Equipamientoservicio.ListarAreasxAreapadre(areacodi);
            DetalleZonaModel modelo = new DetalleZonaModel
            {
                
                DetalleZona = detalleZona,
                ListaAreasDeZona = listaAreasDeZona
            };
            return PartialView(modelo);
        }
         
        [HttpPost]
        public JsonResult BuscarAreas(int iCodigo)
        {
            List<int> lista = new List<int>();
            try
            {
                var AreasEncontradas = Equipamientoservicio.ListarAreasxAreapadre(iCodigo);
                foreach (var item in AreasEncontradas)
                {                   
                    lista.Add(item.AreaCodi);
                }
                return Json(new { lista = lista});
            }
            catch (Exception e)
            {
                log.Error("EditarZona", e);    
                return Json(-1);
            } 
        }

        [HttpPost]
        public PartialViewResult EditarZona(int areacodi, int idTarea)
        {
            var listaNiveles = Equipamientoservicio.ListEqAreanivels();
            var listaAreas = Equipamientoservicio.ListaSoloAreasActivas(idTarea);
            var zonaEditar = Equipamientoservicio.GetByIdEqArea(areacodi);
            EditarZonaModel modelo = new EditarZonaModel
            {
                ANivelCodi=zonaEditar.ANivelCodi,
                ListaAreasEditar = listaAreas,
                ZonaEditar = zonaEditar,
                ListaNivelesEditar=listaNiveles
            };
            return PartialView(modelo);
        }

        [HttpPost]
        public JsonResult ActualizarZona(int iCodigo,int Nivelcodi, string sNombre, string sAbrevitura, string AreasEditar)
        {
            List<int> listaNuevos = new List<int>();
            List<int> listaOriginal = new List<int>();
            List<int> arearelCodis = new List<int>();
            try
            {
                var ZonaActualizar = Equipamientoservicio.GetByIdEqArea(iCodigo);
                ZonaActualizar.ANivelCodi = Nivelcodi;
                ZonaActualizar.Areanomb = sNombre.ToUpper();
                ZonaActualizar.Areaabrev = sAbrevitura.ToUpper();
                ZonaActualizar.UsuarioUpdate = User.Identity.Name;
                ZonaActualizar.FechaUpdate = DateTime.Now;
                Equipamientoservicio.UpdateEqArea(ZonaActualizar);

                var AreasDeZona = Equipamientoservicio.ListarAreasxAreapadre(iCodigo);
                foreach (var item in AreasDeZona)
                {
                    listaOriginal.Add(item.AreaCodi);
                }

                if (!string.IsNullOrEmpty(AreasEditar))
                {
                    var codigoAreas = AreasEditar.Split('&');
                    foreach (var codi in codigoAreas)
                    {
                        listaNuevos.Add(int.Parse(codi));
                    }
                }
                var ListaAgregar=listaNuevos.Except(listaOriginal);
                var ListaEliminar=listaOriginal.Except(listaNuevos);

                foreach (var areacodi in ListaEliminar)
                {
                    var arearlcodi = Equipamientoservicio.GetxAreapadrexAreacodi(iCodigo, areacodi);

                    arearelCodis.Add(arearlcodi.AreaRlCodi);
                }

                foreach (var codigoE in arearelCodis)    //se eliminan las areas que pertenecian a la zona y se desmarcaron
                {
                    Equipamientoservicio.DeleteEqArearel(codigoE, User.Identity.Name);
                }

                foreach (var codigoA in ListaAgregar)    //se asignan las nuevas areas marcadas que pertenecerán a la zona
                {
                    EqAreaRelDTO nuevoRel = new EqAreaRelDTO();

                    nuevoRel.AreaPadre = iCodigo;
                    nuevoRel.AreaCodi = codigoA;
                    nuevoRel.FechaDat = DateTime.Today;
                    nuevoRel.LastCodi = -1;
                    nuevoRel.Arearlusumodificacion = User.Identity.Name;
                    Equipamientoservicio.SaveEqArearel(nuevoRel);
                }         
            }
            catch (Exception e)
            {
                log.Error("EditarZona", e);       //nombre de EditarZona y no de ActualizarZona
                return Json(-1);
            }
            return Json(1);
        }

        [HttpPost]
        public PartialViewResult NuevaZona(int idTarea)
        {
           
            var listaNiveles = Equipamientoservicio.ListEqAreanivels();
            var listaAreas = Equipamientoservicio.ListaSoloAreasActivas(idTarea);
            NuevaZonaModel modelo = new NuevaZonaModel
            {
                ListaAreas = listaAreas,
                Zona = new EqAreaDTO(),
                ListaNiveles = listaNiveles
            };
            return PartialView(modelo);
        }

      
        public PartialViewResult ListasZonasArea2(int idPrueba)
        {

            var listaNiveles = Equipamientoservicio.ListEqAreanivels();
            var listaAreas = Equipamientoservicio.ListaSoloAreasActivas(idPrueba);
            NuevaZonaModel modelo = new NuevaZonaModel
            {
                ListaAreas = listaAreas,
                Zona = new EqAreaDTO(),
                ListaNiveles = listaNiveles

            };
            return PartialView(modelo);
        }

        [HttpPost]
        public JsonResult GrabarZona(int anivelcodi, string sNombre, string sAbrevitura, string areas)
        {
            try
            {  
                EqAreaDTO nuevaZona = new EqAreaDTO();
                
                nuevaZona.Tareacodi =12;
                nuevaZona.Areaabrev = sAbrevitura.ToUpper();
                nuevaZona.Areanomb = sNombre.ToUpper();
                nuevaZona.Areapadre = -1;
                nuevaZona.Areaestado = "A";
                nuevaZona.UsuarioCreacion = User.Identity.Name;
                nuevaZona.FechaCreacion = DateTime.Now;
                nuevaZona.UsuarioUpdate = User.Identity.Name;
                nuevaZona.FechaUpdate = DateTime.Now;
                nuevaZona.ANivelCodi = anivelcodi;

                int codigoNuevaZona = Equipamientoservicio.InsertarEqArea(nuevaZona);

                if (!string.IsNullOrEmpty(areas))
                {
                    var codigoAreas = areas.Split('&');
                    foreach (var codigo in codigoAreas)
                    {
                        EqAreaRelDTO nuevaRel = new EqAreaRelDTO();

                        nuevaRel.AreaPadre = codigoNuevaZona;
                        nuevaRel.AreaCodi = int.Parse(codigo);
                        nuevaRel.FechaDat = DateTime.Today;
                        nuevaRel.LastCodi = -1;
                        nuevaRel.Arearlusumodificacion = User.Identity.Name;

                        Equipamientoservicio.SaveEqArearel(nuevaRel);
                    }
                }
            }
            catch (Exception e)
            {
                log.Error("GrabarZona", e);
                return Json(-1);
            }
            return Json(1);
        }


    }
    

    


}
