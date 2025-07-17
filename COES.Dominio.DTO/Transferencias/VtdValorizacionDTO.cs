using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    public class VtdValorizacionDTO
    {
        public int Valocodi { get; set; }
        public DateTime?  Valofecha{ get; set; }
        public decimal  Valomr{ get; set; }
        public decimal  Valopreciopotencia{ get; set; }
        public decimal  Valodemandacoes{ get; set; }
        public decimal  Valofactorreparto{ get; set; }
        public decimal  Valoporcentajeperdida { get; set; }        
        public decimal  Valofrectotal { get; set; }
        public decimal  Valootrosequipos { get; set; }
        public decimal  Valocostofuerabanda { get; set; }
        public decimal  Valoco { get; set; }
        public decimal  Valora { get; set; }
        public decimal ValoraSub { get; set; }
        public decimal ValoraBaj { get; set; }
        public decimal  Valoofmax { get; set; }
        public decimal ValoofmaxBaj { get; set; }
        public decimal  Valocompcostosoper { get; set; }
        public decimal  Valocomptermrt { get; set; }
        public char  Valoestado{ get; set; }
        public string  Valousucreacion{ get; set; }
        public DateTime?   Valofeccreacion{ get; set; }
        public string  Valousumodificacion{ get; set; }
        public DateTime?  Valofecmodificacion{ get; set; }

    }
}
