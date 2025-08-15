
using System.Collections.Generic;

namespace COES.Dominio.DTO.ValidacionVTEAVTP
{
    public class VtpVersionDTO
    {
        public int Resultado { get; set; }
        public string Mensaje { get; set; }
        public List<TableVersionVtpDTO> Versiones { get; set; }

    }
    public class TableVersionVtpDTO
    {
        public int RecPortCodi { get; set; }
        public string RecPotNombre { get; set; }
    }
 }
