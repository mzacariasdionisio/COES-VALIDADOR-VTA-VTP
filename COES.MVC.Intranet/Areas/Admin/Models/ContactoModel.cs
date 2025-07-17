using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Admin.Models
{
    public class ContactoModel
    {
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<SiTipoempresaDTO> ListaTipoEmpresas { get; set; }
        public List<WbContactoDTO> ListaContactos { get; set; }
        public WbContactoDTO Entidad { get; set; }
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Cargo { get; set; }
        public string Telefono { get; set; }
        public string Movil { get; set; }
        public string Documento { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Comentario { get; set; }
        public string Area { get; set; }
        public string Estado { get; set; }
        public int IdEmpresa { get; set; }
        public int IdTipoEmpresa { get; set; }
        public string IndicadorEdicion { get; set; }
        public string Fuente { get; set; }
        public string IndPublico { get; set; }

        public List<WbComiteContactoDTO> ListaComitecontacto { get; set; }
        public List<WbComiteDTO> ListaComite { get; set; }
        public string Comites { get; set; }

        public List<WbComiteListaContactoDTO> ListaComiteListaContacto { get; set; }
        public List<WbComiteListaDTO> ListaCorreos { get; set; }
        public string Correos { get; set; }

        public List<WbProcesoContactoDTO> ListaProcesocontacto { get; set; }
        public List<WbProcesoDTO> ListaProceso { get; set; }
        public string Procesos { get; set; }
    }

    /// <summary>
    /// Model para los comités
    /// </summary>
    public class ComiteModel
    {
        public List<WbComiteDTO> ListaComite { get; set; }
        public WbComiteDTO Entidad { get; set; }
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public string Estado { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

    }


    public class ComiteListaModel
    {
        public WbComiteListaDTO Entidad { get; set; }
        public int ComiteCodigo { get; set; }
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public string Estado { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public List<WbComiteListaDTO> Lista { get; set; }

    }

    /// <summary>
    /// Model para los comités
    /// </summary>
    public class ProcesoModel
    {
        public List<WbProcesoDTO> ListaProceso { get; set; }
        public WbProcesoDTO Entidad { get; set; }
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public string Estado { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

    }
}