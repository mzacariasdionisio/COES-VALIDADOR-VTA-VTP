using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IND_INSUMO_LOG
    /// </summary>
    public class IndInsumoLogDTO : EntityBase
    {
        public int Ilogcodi { get; set; }
        public int Irptcodi { get; set; }
        public int Iloginsumo { get; set; }
        public int Ilogcodigo { get; set; }
    }
}
