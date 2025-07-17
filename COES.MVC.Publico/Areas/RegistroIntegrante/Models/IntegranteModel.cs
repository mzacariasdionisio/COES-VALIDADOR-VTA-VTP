using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;
namespace COES.MVC.Publico.Areas.RegistroIntegrante.Models
{
    public class IntegranteModel
    {
        public int Codigo { get; set; }
        public int CodigoTipoAgente { get; set; }
        public List<SiTipoempresaDTO> TipoEmpresa { get; set; }
        public List<TipoDocumentoSustentarioModel> TipoDocumentoSustentario { get; set; }

        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }
        public int Compcode { get; set; }
        public string Emprdire { get; set; }
        public string Emprtele { get; set; }
        public string Emprnumedocu { get; set; }
        public string Tipodocucodi { get; set; }
        public string Emprruc { get; set; }
        public string Emprabrev { get; set; }
        public int Emprorden { get; set; }
        public string Emprdom { get; set; }
        public string Emprsein { get; set; }
        public string Emprrazsocial { get; set; }
        public string Emprcoes { get; set; }
        public string Lastuser { get; set; }
        public DateTime Lastdate { get; set; }
        public string Inddemanda { get; set; }
        public string Emprestado { get; set; }
        public string Emprcodosinergmin { get; set; }
        public string Emprnombrecomercial { get; set; }
        public string Emprdomiciliolegal { get; set; }
        public string Emprsigla { get; set; }
        public string Emprnumpartidareg { get; set; }
        public string Emprtelefono { get; set; }
        public string Emprfax { get; set; }
        public string Emprpagweb { get; set; }
        public string Emprcartaadjunto { get; set; }
        public string Emprestadoregistro { get; set; }
        public DateTime Emprfecinscripcion { get; set; }
        public string Emprusucreacion { get; set; }
        public DateTime Emprfeccreacion { get; set; }
        public string Emprusumodificacion { get; set; }
        public DateTime Emprfecmodificacion { get; set; }
        public int Tipoemprcodi { get; set; }


        // Tipo
        public int Tipocodi { get; set; }
        public string Tipoprincipal { get; set; }
        public string Tipotipagente { get; set; }
        public string Tipodocsustentatorio { get; set; }
        public HttpPostedFileBase Tipoarcdigitalizado { get; set; }
        public string Tipopotenciainstalada { get; set; }
        public string Tiponrocentrales { get; set; }
        public string Tipolineatrans_500 { get; set; }
        public string Tipolineatrans_220 { get; set; }
        public string Tipolineatrans_138 { get; set; }
        public string Tipolineatrans_500km { get; set; }
        public string Tipolineatrans_220km { get; set; }
        public string Tipolineatrans_138km { get; set; }
        public string Tipolineatrans_menor138km { get; set;  }
        public string Tipototallineastransmision { get; set; }
        public string Tipomaxdemandacoincidente { get; set; }
        public string Tipomaxdemandacontratada { get; set; }
        public string Tiponumsuministrador { get; set; }
        public string Tipousucreacion { get; set; }
        public DateTime Tipofeccreacion { get; set; }
        public string Tipousumodificacion { get; set; }
        public DateTime Tipofecmodificacion { get; set; }
        public string Tipocomentario { get; set; }
        public string Tipoarchivodigitalizado { get; set; }
        public string Tipoarchivodigitalizadofilename { get; set; }


        public string RTNombres { get; set; }
        public string RTApellidos { get; set; }
        public string RTCargoEmpresa { get; set; }
        public string RTTelefono { get; set; }
        public string RTTelefonoMobil { get; set; }
        public string RTCorreoElectronico { get; set; }
        public string strRepresentateLegal { get; set; }
        public HttpPostedFileBase RTCartaSolicitudIngreso { get; set; }

        public string PCNombres { get; set; }
        public string PCApellidos { get; set; }
        public string PCCargoEmpresa { get; set; }
        public string PCTelefono { get; set; }
        public string PCTelefonoMobil { get; set; }
        public string PCCorreoElectronico { get; set; }


        // Titulos Adicionales

        public HttpPostedFileBase Tipodocname1 { get; set; }
        public string Tipodocadjfilename1 { get; set; }
        public HttpPostedFileBase Tipodocname2 { get; set; }
        public string Tipodocadjfilename2 { get; set; }
        public HttpPostedFileBase Tipodocname3 { get; set; }
        public string Tipodocadjfilename3 { get; set; }
        public HttpPostedFileBase Tipodocname4 { get; set; }
        public string Tipodocadjfilename4 { get; set; }
        public HttpPostedFileBase Tipodocname5 { get; set; }
        public string Tipodocadjfilename5 { get; set; }

        public List<RepresentanteModel> Representante { get; set; }
        public List<SiRepresentanteDTO> RepresentanteLegal { get; set; }
        public List<HttpFileCollectionWrapper> RLVigenciaPoder { get; set; }

        public string Capcha { get; set; }
        public string SiteKey { get; set; }

        public SiRepresentanteDTO PersonaContacto { get; set; }
        public SiRepresentanteDTO ResponsableTramite { get; set; }
        public SiTipoComportamientoDTO TipoComportamientoPrincipal { get; set; }
        public SiEmpresaDTO Empresa { get; set; }
        public List<TitulosAdicionalesModel> TitulosAdicionales { get; set; }

        public int ReviiteracionSGI { get; set; }
        public int ReviiteracionDJR { get; set; }
        public string Tipotastr { get; set; }
        public int EmpresaCondicionVarianteGenerador { get; set; }
        public int EmpresaCondicionVarianteTransmisor { get; set; }
        public int EmpresaCondicionVarianteDistribuidor { get; set; }
        public int EmpresaCondicionVarianteUsuarioLibre { get; set; }        

        public string TituloEmpresa { get; set; }
        public string TituloTipoIntegrante { get; set; }
        public string TituloRepresentanteLegal { get; set; }
        public string TituloPersonaContacto { get; set; }
        public string TituloPersonaResponsable { get; set; }
        public string Tipolineatrans_menor138 { get; internal set; }
    }
}