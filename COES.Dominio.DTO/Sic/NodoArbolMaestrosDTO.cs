using System.Collections.Generic;

namespace COES.Dominio.DTO.Sic
{
    public class NodoArbolMaestrosDto
    {
        public int Key { get; set; }
        public bool Selected { get; set; }
        public string Title { get; set; }
        public List<NodoArbolMaestrosDto> Children { get; set; }
    }
}