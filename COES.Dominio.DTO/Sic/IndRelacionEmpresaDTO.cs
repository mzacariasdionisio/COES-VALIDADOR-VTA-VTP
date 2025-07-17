using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class IndRelacionEmpresaDTO : EntityBase
    {
        public int Relempcodi { get; set; }
        public int Emprcodi { get; set; }
        public int Equicodicentral { get; set; }
        public int Equicodiunidad { get; set; }
        public int Grupocodi { get; set; }
        public int Famcodi { get; set; }
        public string Relempunidadnomb { get; set; }
        public int Gaseoductoequicodi { get; set; }
        public int? Grupocodicn2 { get; set; }
        public string Relempcuadro1 { get; set; }
        public string Relempcuadro2 { get; set; }
        public string Relempsucad { get; set; }
        public string Relempsugad { get; set; }
        public string Relempestado { get; set; }
        public int Relemptecnologia { get; set; }
        public string Relempusucreacion { get; set; }
        public DateTime Relempfeccreacion { get; set; }
        public string Relempusumodificacion { get; set; }
        public DateTime Relempfecmodificacion { get; set; }

        //Atributos de consulta
        public string Emprnomb { get; set; }
        public string Equinomb { get; set; }
        public string Equinombcentral { get; set; }
        public string Equinombunidad { get; set; }
        public string Gaseoductoequinomb { get; set; }
        public string Unidadnomb { get; set; }
        public string Centralnomb { get; set; }
        public string Famnomb { get; set; }
        public string Gruponomb { get; set; }
        public int? Gruporeservafria { get; set; }

    }
}
