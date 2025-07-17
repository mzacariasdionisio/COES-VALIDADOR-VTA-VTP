using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla TRN_INFOADICIONAL
    /// </summary>
    public class TrnInfoadicionalDTO : EntityBase
    {
        public int? Emprcodi { get; set; } 
        public int Infadicodi { get; set; } 
        public string Infadinomb { get; set; } 
        public int? Tipoemprcodi { get; set; }
        public string Infadicodosinergmin { get; set; }
        //Atributos agregados para Empresas con dos comportamientos
        public string Emprnomb { get; set; }
        public string Tipoemprdesc { get; set; }
        public string Emprcodosinergmin { get; set; }
        public DateTime Fechacorte { get; set; }
        public string Fechacortedesc { get; set; }
        public string Infadiestado { get; set; }

        public string UsuUpdate { get; set; }
        public string UsuCreacion { get; set; }
        public DateTime DateCreacion { get; set; }  
        public DateTime DateUpdate { get; set; }

    }
}
