using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VTP_PEAJE_EMPRESA_AJUSTE
    /// </summary>
    public class VtpPeajeEmpresaAjusteDTO : EntityBase
    {
        public int Pempajcodi { get; set; }
        public int Pericodi { get; set; }
        public int Emprcodipeaje { get; set; }
        public int Pingcodi { get; set; }
        public int Emprcodicargo { get; set; }
        public decimal Pempajajuste { get; set; }
        public string Pempajusucreacion { get; set; }
        public DateTime Pempajfeccreacion { get; set; }
        //MAPEA ATRIBUTOS ADIONALES USADOS EN VISTAS Y CONSULTAS
        public string Emprnombpeaje { get; set; }
        public string Emprnombcargo { get; set; }
        public string Pingnombre { get; set; }
    }
}
