using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class LineasFichaADTO
    {
        public int LinFichaACodi { get; set; }
        public int ProyCodi { get; set; }
        public string NombreLinea { get; set; }
        public string FecPuestaServ { get; set; }
        public string SubInicio { get; set; }
        public string OtroSubInicio { get; set; }
        public string SubFin { get; set; }
        public string OtroSubFin { get; set; }
        public string EmpPropietaria { get; set; }
        public Decimal? NivTension { get; set; }
        public Decimal? CapCorriente { get; set; }
        public Decimal? CapCorrienteA { get; set; }
        public int? TpoSobreCarga { get; set; }
        public Decimal? NumTemas { get; set; }
        public Decimal? LongTotal { get; set; }
        public Decimal? LongVanoPromedio { get; set; }
        public string TipMatSop { get; set; }
        public string DesProtecPrincipal { get; set; }
        public string DesProtecRespaldo { get; set; }
        public string DesGenProyecto { get; set; }
        public List<LineasFichaADet1DTO> LineasFichaADet1DTO { get; set; }
        public List<LineasFichaADet2DTO> LineasFichaADet2DTO { get; set; }
        public string UsuCreacion { get; set; }
        public DateTime FecCreacion { get; set; }
        public string UsuModificacion { get; set; }
        public DateTime FecModificacion { get; set; }
        public string IndDel { get; set; }
        public string Empresa { get; set; }
        public string NombreProyecto { get; set; }
        public string TipoProyecto { get; set; }
        public string SubTipoProyecto { get; set; }
        public string DetalleProyecto { get; set; }
        public string Condifencial { get; set; }
    }
}
