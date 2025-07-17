using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PF_CUADRO
    /// </summary>
    public class PfCuadroDTO : EntityBase
    {
        public int Pfcuacodi { get; set; } 
        public string Pfcuanombre { get; set; } 
        public string Pfcuatitulo { get; set; } 
        public string Pfcuasubtitulo { get; set; } 
    }
}
