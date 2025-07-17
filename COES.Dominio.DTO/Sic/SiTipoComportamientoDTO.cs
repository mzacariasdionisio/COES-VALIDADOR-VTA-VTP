using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_TIPO_COMPORTAMIENTO
    /// </summary>
    public class SiTipoComportamientoDTO : EntityBase
    {
        public int Tipocodi { get; set; } 
        public string Tipoprincipal { get; set; } 
        public string Tipotipagente { get; set; } 
        public string Tipodocsustentatorio { get; set; }
        public string TipodocsustentatorioComentario { get; set; }
        public string Tipoarcdigitalizado { get; set; }
        public string TipoarcdigitalizadoComentario { get; set; }
        public string Tipoarcdigitalizadofilename { get; set; }
        public string Tipopotenciainstalada { get; set; }
        public string TipopotenciainstaladaComentario { get; set; }
        public string Tiponrocentrales { get; set; }
        public string TiponrocentralesComentario { get; set; }
        public string Tipolineatrans500 { get; set; } 
        public string Tipolineatrans220 { get; set; } 
        public string Tipolineatrans138 { get; set; } 
        public string Tipolineatrans500km { get; set; } 
        public string Tipolineatrans220km { get; set; } 
        public string Tipolineatrans138km { get; set; } 
        public string Tipolineatransmenor138 { get; set;  }
        public string Tipototallineastransmision { get; set; }
        public string TipototallineastransmisionComentario { get; set; }
        public string Tipomaxdemandacoincidente { get; set; }
        public string TipomaxdemandacoincidenteComentario { get; set; }
        public string Tipomaxdemandacontratada { get; set; }
        public string TipomaxdemandacontratadaComentario { get; set; }
        public string Tiponumsuministrador { get; set; }
        public string TiponumsuministradorComentario { get; set; }
        public string Tipousucreacion { get; set; } 
        public DateTime? Tipofeccreacion { get; set; } 
        public string Tipousumodificacion { get; set; } 
        public DateTime? Tipofecmodificacion { get; set; } 
        public int? Tipoemprcodi { get; set; } 
        public int? Emprcodi { get; set; }
        public string Tipobaja { get; set; }
        public string Tipoinicial { get; set; }
        //Agregados
        public string SoloLectura { get; set; }
        public string Accion { get; set; }
        public int Id { get; set; }
        public int IdTipodocsustentatorio { get; set; }
        public string Tipodocname1 { get; set; }
        public string Tipodocadjfilename1 { get; set; }
        public string Tipodocname2 { get; set; }
        public string Tipodocadjfilename2 { get; set; }
        public string Tipodocname3 { get; set; }
        public string Tipodocadjfilename3 { get; set; }
        public string Tipodocname4 { get; set; }
        public string Tipodocadjfilename4 { get; set; }
        public string Tipodocname5 { get; set; }
        public string Tipodocadjfilename5 { get; set; }
        public string Tipocomentario { get; set; }
        public string Tipolineatransmenor138km { get; set; }
    }
}
