using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Despacho.Models
{
    public class GrupoCurvaModel
    {

        public List<PrCurvaDTO> ListaCentral { get; set; }

    }

    public class EntidadGrupoCurvaModel
    {
        public int Codigo { get; set; }
        public string Nombres { get; set; }
    }

    public class DetalleGrupoCurvaModel
    {
        public int Codigo { get; set; }
        public string Nombres { get; set; }
        public int GrupoPrincipal { get; set; }
        public List<EntidadGrupoCurvaModel> Combo { get; set; }
        public List<PrGrupoDTO> ListaDetalle { get; set; }
    }

    public class EntidadDetalleGrupoCurvaModel
    {
        public int codigoCurva { get; set; }
        public int codigoGrupo { get; set; }
        public string grupoPrincipal { get; set; }
    }

    public class ReporteModel
    {
        public string Fecha { get; set; }
        public List<EntidadReporteModel> Lista { get; set; }

    }

    public class EntidadReporteModel
    {
        public string Empresa { get; set; }
        public string GrupoModoOperacion { get; set; }
        public string CEC { get; set; }
        public string Pe { get; set; }
        public string Rendimiento { get; set; }
        public string Precio { get; set; }
        public string CVNC { get; set; }
        public string CVC { get; set; }
        public string CV { get; set; }
        public string Tramo1 { get; set; }
        public string Cincrem1 { get; set; }
        public string Tramo2 { get; set; }
        public string Cincrem2 { get; set; }
        public string Tramo3 { get; set; }
        public string Cincrem3 { get; set; }

        public string TipoCombustible { get; set; }

    }
}