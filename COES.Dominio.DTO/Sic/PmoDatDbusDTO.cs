using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class PmoDatDbusDTO : EntityBase
    {
        public int PmDbusCodi { get; set; }
        public int? GrupoCodiSddp { get; set; }
        public string GrupoNomb { get; set; }
        public string PmDbusIdSistema { get; set; }
        public int? PmDbusNroSecGen { get; set; }
        public int? CodCentral { get; set; }
        public string NombCentral { get; set; }
        public string PmDbusNroArea { get; set; }


        //entidades para la generacion DAT
        public string Num { get; set; }
        public string Tp { get; set; }
        public string NombreB { get; set; }
        public string Id { get; set; }
        public string Numeral { get; set; }
        public int? Tg { get; set; }
        public string Plnt { get; set; }
        public string NombreGener { get; set; }
        public string Area { get; set; }
        public string Per1 { get; set; }
        public string Ploa1 { get; set; }
        public string Pind1 { get; set; }
        public string PerF1 { get; set; }
        public string Per2 { get; set; }
        public string Ploa2 { get; set; }
        public string Pind2 { get; set; }
        public string PerF2 { get; set; }
        public string Per3 { get; set; }
        public string Ploa3 { get; set; }
        public string Pind3 { get; set; }
        public string PerF3 { get; set; }
        public string Per4 { get; set; }
        public string Ploa4 { get; set; }
        public string Pind4 { get; set; }
        public string PerF4 { get; set; }
        public string Per5 { get; set; }
        public string Ploa5 { get; set; }
        public string Pind5 { get; set; }
        public string PerF5 { get; set; }
        public string Icca { get; set; }

    }
}
