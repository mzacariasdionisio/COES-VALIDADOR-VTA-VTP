using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Equipamiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace COES.WebAPI.Controllers
{
    public class EquipamientoController : ApiController
    {
        EquipamientoAppServicio service = new EquipamientoAppServicio();

        public IHttpActionResult POSTSaveEqEquipo(EqEquipoDTO entity)
        {
            return Ok(
               this.service.SaveEqEquipo(entity)
            );
        }
        //public IHttpActionResult PUTUpdateEqEquipo(EqEquipoDTO entity)
        //{
        //    return Ok(
        //       this.service.UpdateEqEquipo(entity)
        //    );
        //}
        //public IHttpActionResult DeleteEqEquipo(int equicodi)
        //{
        //    return Ok(
        //       this.service.DeleteEqEquipo(equicodi)
        //    );
        //}

        public IHttpActionResult GetObtenerEquipoPadresHidrologicosCuenca()
        {
            return Ok(
               this.service.ObtenerEquipoPadresHidrologicosCuenca()
            );
        }
        public IHttpActionResult GetObtenerEquipoPorAreaEmpresaTodos(int idEmpresa, int idArea)
        {
            return Ok(
               this.service.ObtenerEquipoPorAreaEmpresaTodos(idEmpresa, idArea)
            );
        }
        public IHttpActionResult GetObtenerEquipoPorAreaEmpresa(int idEmpresa, int idArea)
        {
            return Ok(
               this.service.ObtenerEquipoPorAreaEmpresa(idEmpresa, idArea)
            );
        }
        public IHttpActionResult GetObtenerEquipoPadresHidrologicosCuencaTodos()
        {
            return Ok(
               this.service.ObtenerEquipoPadresHidrologicosCuencaTodos()
            );
        }
        public IHttpActionResult GetByIdEqEquipo(int equicodi)
        {
            return Ok(
               this.service.GetByIdEqEquipo(equicodi)
            );
        }
        public IHttpActionResult GetListEqEquipos()
        {
            return Ok(
               this.service.ListEqEquipos()
            );
        }
        public IHttpActionResult GetByCriteriaEqEquipos()
        {
            return Ok(
               this.service.GetByCriteriaEqEquipos()
            );
        }
        public IHttpActionResult GetListadoCentralesOsinergmin()
        {
            return Ok(
               this.service.ListadoCentralesOsinergmin()
            );
        }
        public IHttpActionResult GetListarEquiposxFiltro(int idEmpresa, string sEstado, int idTipoEquipo, int idTipoEmpresa,
            string sNombreEquipo, int idPadre)
        {
            return Ok(
               this.service.ListarEquiposxFiltro(idEmpresa, sEstado, idTipoEquipo, idTipoEmpresa, sNombreEquipo, idPadre)
            );
        }
        public IHttpActionResult GetListarEquiposxFiltroPaginado(int idEmpresa, string sEstado, int idTipoEquipo,
            int idTipoEmpresa, string sNombreEquipo, int idPadre, int nroPagina, int nroFilas, ref int totalPaginas,
            ref int totalRegistros)
        {
            return Ok(
               this.service.ListarEquiposxFiltroPaginado(idEmpresa, sEstado, idTipoEquipo, idTipoEmpresa, sNombreEquipo, idPadre,
                nroPagina, nroFilas, ref totalPaginas, ref totalRegistros)
            );
        }
        public IHttpActionResult GetObtenerDetalleEquipo(int idEquipo)
        {
            return Ok(
               this.service.ObtenerDetalleEquipo(idEquipo)
            );
        }
        public IHttpActionResult GetBuscarEquipoxNombre(string coincidencia, int nroPagina, int nroFilas)
        {
            return Ok(
               this.service.BuscarEquipoxNombre(coincidencia, nroPagina, nroFilas)
            );
        }
        public IHttpActionResult GetListarEquipoxFamilias(params int[] iCodFamilias)
        {
            return Ok(
               this.service.ListarEquipoxFamilias(iCodFamilias)
            );
        }
        public IHttpActionResult GetListarEquipoxFamiliasxEmpresas(int[] iCodFamilias, int[] iEmpresas)
        {
            return Ok(
               this.service.ListarEquipoxFamiliasxEmpresas(iCodFamilias, iEmpresas)
            );
        }
        public IHttpActionResult GetListaEquipamientoPaginado(int iEmpresa, int iFamilia, int iTipoEmpresa, int iEquipo,
            string sEstado, string nombre, int nroPagina, int nroFilas)
        {
            return Ok(
               this.service.ListaEquipamientoPaginado(iEmpresa, iFamilia, iTipoEmpresa, iEquipo, sEstado, nombre, nroPagina, nroFilas)
            );
        }
        public IHttpActionResult GetTotalEquipamiento(int iEmpresa, int iFamilia, int iTipoEmpresa, int iEquipo, string sEstado, string nombre)
        {
            return Ok(
                   this.service.TotalEquipamiento(iEmpresa, iFamilia, iTipoEmpresa, iEquipo, sEstado, nombre)
                );
        }
        public IHttpActionResult GetObtenerEquiposProcManiobras(int famCodi, int propCodi)
        {
            return Ok(
               this.service.ObtenerEquiposProcManiobras(famCodi, propCodi)
            );
        }
        //public IHttpActionResult ObtenerCoordenadasEquipo(int idEquipo, out string coordenadaX, out string coordenadaY)
        //{
        //    return Ok(
        //       this.service.ObtenerCoordenadasEquipo(coordenadaX, coordenadaY)
        //    );
        //}
        public IHttpActionResult ObtenerEquipoPorAreaEmpresa(int idEmpresa, int idArea)
        {
            return Ok(
               this.service.ObtenerEquipoPorAreaEmpresa(idEmpresa, idArea)
            );
        }
        public IHttpActionResult GetListaEquipoXEmpresaYFamilia(int emprcodi, int famcodi)
        {
            return Ok(
               this.service.GetListaEquipoXEmpresaYFamilia(emprcodi, famcodi)
            );
        }
        public IHttpActionResult GetByCriteriaEqequipo(int emprcodi, int famcodi)
        {
            return Ok(
               this.service.GetByCriteriaEqequipo(emprcodi, famcodi)
            );
        }
        public IHttpActionResult GetListarEquiposPropiedadesAGC(string fecha)
        {
            return Ok(
               this.service.ListarEquiposPropiedadesAGC(fecha)
            );
        }
        public IHttpActionResult GetObtenerValorPropiedadEquipoFecha(int propcodi, int equicodi, string fecha)
        {
            return Ok(
               this.service.ObtenerValorPropiedadEquipoFecha(propcodi, equicodi, fecha)
            );
        }
        public IHttpActionResult GetListarEquiposAGC()
        {
            return Ok(
               this.service.ListarEquiposAGC()
            );
        }
        public IHttpActionResult GetListadoEquipoNombre()
        {
            return Ok(
               this.service.ListadoEquipoNombre()
            );
        }

        public IHttpActionResult GetListarEquiposPorPadre(int equipadre)
        {
            return Ok(
               this.service.ListarEquiposPorPadre(equipadre)
            );
        }
        public IHttpActionResult GetListarEquiposPropiedades(int propcodi, DateTime fecha, int emprCodi, int areacodi, int famCodi, int nroPage, int pageSize)
        {
            return Ok(
               this.service.ListarEquiposPropiedades(propcodi, fecha, emprCodi, areacodi, famCodi, nroPage, pageSize)
            );
        }
    }
}