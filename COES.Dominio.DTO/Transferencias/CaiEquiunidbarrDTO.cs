using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla CAI_EQUIUNIDBARR
    /// </summary>
    public class CaiEquiunidbarrDTO : EntityBase
    {
        public int Caiunbcodi { get; set; } 
        public int Ptomedicodi { get; set; } 
        public int Emprcodi { get; set; } 
        public int Equicodicen { get; set; } 
        public int Equicodiuni { get; set; } 
        public int Barrcodi { get; set; }
        public object Caiunbbarra { get; set; }
        public DateTime Caiunbfecvigencia { get; set; } 
        public string Caiunbusucreacion { get; set; } 
        public DateTime Caiunbfeccreacion { get; set; } 
        public string Caiunbusumodificacion { get; set; } 
        public DateTime Caiunbfecmodificacion { get; set; }

        //variable de consulta
        public string Emprnomb { get; set; }
        public string Equinombcen { get; set; }
        public string Equinombuni { get; set; }
        public string Ptomedielenomb { get; set; }
        public string Barrnombre { get; set; }
        
    }
}
