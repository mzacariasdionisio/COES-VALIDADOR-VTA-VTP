using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VCR_ASIGNACIONPAGO
    /// </summary>
    public class VcrAsignacionpagoDTO : EntityBase
    {
        public int Vcrapcodi { get; set; } 
        public int Vcrecacodi { get; set; }
        public int Emprcodi { get; set; }
        public int Equicodicen { get; set; }
        public int Equicodiuni { get; set; }
        public DateTime Vcrapfecha { get; set; }
        public decimal Vcrapasignpagorsf { get; set; } 
        public string Vcrapusucreacion { get; set; } 
        public DateTime Vcrapfeccreacion { get; set; }

        //atributos de consulta    
        public string Emprnomb { get; set; }
        public string Equinombcen { get; set; }
        public string Equinombuni { get; set; }
    }
}
