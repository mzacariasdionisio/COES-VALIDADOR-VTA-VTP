using System;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla CPA_PARAMETRO
    /// </summary>
    public class CpaParametroHistoricoDTO
    {
        public int Cpaphscodi { get; set; }            
        public int Cpaprmcodi { get; set; }              
        public string Cpaphstipo { get; set; }       
        public string Cpaphsusuario { get; set; }       
        public DateTime Cpaphsfecha { get; set; }    
    }
}

