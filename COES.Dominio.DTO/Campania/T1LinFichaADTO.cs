using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class T1LinFichaADTO
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
        public decimal? NivTension { get; set; }
        public decimal? CapCorriente { get; set; }
        public decimal? CapCorrienteA { get; set; }
        public int? TpoSobreCarga { get; set; }
        public decimal? NumTemas { get; set; }
        public decimal? LongTotal { get; set; }
        public decimal? LongVanoPromedio { get; set; }
        public string TipMatSop { get; set; }
        public string DesProtecPrincipal { get; set; }
        public string DesProtecRespaldo { get; set; }
        public string DesGenProyecto { get; set; }
        public List<T1LinFichaADet1DTO> LineasFichaADet1DTO { get; set; }
        public List<T1LinFichaADet2DTO> LineasFichaADet2DTO { get; set; }
        public string UsuCreacion { get; set; }
        public DateTime FecCreacion { get; set; }
        public string UsuModificacion { get; set; }
        public DateTime FecModificacion { get; set; }
        public string IndDel { get; set; }
    }
}
