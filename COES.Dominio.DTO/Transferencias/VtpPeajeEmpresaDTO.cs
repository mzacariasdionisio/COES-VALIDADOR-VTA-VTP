using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VTP_PEAJE_EMPRESA
    /// </summary>
    public class VtpPeajeEmpresaDTO : EntityBase
    {
        public int Pempcodi { get; set; }
        public int Pericodi { get; set; }
        public int Recpotcodi { get; set; }
        public int Emprcodi { get; set; }
        public decimal Pemptotalrecaudacion { get; set; }
        public decimal Pempporctrecaudacion { get; set; }
        public string Pempusucreacion { get; set; }
        public DateTime Pempfeccreacion { get; set; }
        //MAPEA ATRIBUTOS ADIONALES USADOS EN VISTAS Y CONSULTAS
        public string Emprnomb { get; set; }
    }
}
