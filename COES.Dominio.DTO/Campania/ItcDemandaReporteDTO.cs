using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class ItcDemandaReporteDTO
    {
        public List<Itcdf104DTO> lista104 {  get; set; }
        public List<Itcdf108DTO> lista108 {  get; set; }
        public List<Itcdfp011DTO> listaP011 { get; set; }
        public List<Itcdfp012DTO> listaP012 {  get; set; }
        public List<Itcdfp013DTO> listaP013 {  get; set; }
        public List<Itcdf110DTO> lista110 {  get; set; }
        public List<Itcdf116DTO> lista116 {  get; set; }
        public List<Itcdf121DTO> lista121 {  get; set; }
        public List<Itcdf123DTO> lista123 { get; set; }
        

        public ItcDemandaReporteDTO()
        {
            lista104 = new List<Itcdf104DTO>();
            lista108 = new List<Itcdf108DTO>();
            listaP011 = new List<Itcdfp011DTO>();
            listaP012 = new List<Itcdfp012DTO>();
            listaP013 = new List<Itcdfp013DTO>();
            lista110 = new List<Itcdf110DTO>();
            lista116 = new List<Itcdf116DTO>();
            lista121 = new List<Itcdf121DTO>();
            lista123 = new List<Itcdf123DTO>();
        }
    }
}
