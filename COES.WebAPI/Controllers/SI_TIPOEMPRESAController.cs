using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.General;

namespace COES.WebAPI.Controllers
{
    public class SI_TIPOEMPRESAController : ApiController
    {
        EmpresaAppServicio service = new EmpresaAppServicio();


        public IHttpActionResult Get()
        {
            return Ok(
               this.service.ListarTipoEmpresas()
            );
        }

        public IHttpActionResult GetbyRUC(string ruc)
        {
            return Ok(
               this.service.ConsultarPorRUC(ruc)
            );
        }

        public IHttpActionResult BuscarEmpresas(string nombre, string nroRuc, int idTipoEmpresa, string empresaSein,
            string estado, int nroPagina, int nroFilas)
        {
            return Ok(
               this.service.BuscarEmpresas(nombre, nroRuc, idTipoEmpresa, empresaSein, estado,
                nroPagina, nroFilas)
            );
        }
        public IHttpActionResult ObtenerNroRegistrosBusqueda(string nombre, string nroRuc, int idTipoEmpresa, string empresaSein,
            string estado)
        {
            return Ok(
               this.service.ObtenerNroRegistrosBusqueda(nombre, nroRuc, idTipoEmpresa, empresaSein,
                estado)
            );
        }
        public IHttpActionResult ExportarEmpresas(string nombre, string nroRuc, int idTipoEmpresa, string empresaSein,
            string estado)
        {
            return Ok(
               this.service.ExportarEmpresas(nombre, nroRuc, idTipoEmpresa, empresaSein, estado)
            );
        }

        public IHttpActionResult ObtenerEmpresa(int idEmpresa)
        {
            return Ok(
               this.service.ObtenerEmpresa(idEmpresa)
            );
        }

        //public IHttpActionResult DarBajaEmpresa(int idEmpresa, string usuario)
        //{
        //    return Ok(
        //       this.service.DarBajaEmpresa(idEmpresa, usuario)
        //    );
        //}
        //public IHttpActionResult GrabarEmpresa(SiEmpresaDTO entity, out int id, out List<string> listValidaciones, out string ruc)
        //{
        //    return Ok(
        //       this.service.GrabarEmpresa()
        //    );
        //}
        //public IHttpActionResult ActualizarEstadoEmpresa(SiEmpresaDTO entity)
        //{
        //    return Ok(
        //        this.service.ActualizarEstadoEmpresa(entity)
        //    );

        //}

        public IHttpActionResult ValidarDocumentoIdentificacion(string identificationDocument)
        {
            return Ok(
               this.service.ValidarDocumentoIdentificacion(identificationDocument)
            );
        }
    }
}