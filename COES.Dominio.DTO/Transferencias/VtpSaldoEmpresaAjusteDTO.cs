using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VTP_SALDO_EMPRESA_AJUSTE
    /// </summary>
    public class VtpSaldoEmpresaAjusteDTO : EntityBase
    {
        public int Potseacodi { get; set; }
        public int Pericodi { get; set; }
        public int Emprcodi { get; set; }
        public decimal Potseaajuste { get; set; }
        public string Potseausucreacion { get; set; }
        public DateTime Potseafeccreacion { get; set; }
        //MAPEA ATRIBUTOS ADIONALES USADOS EN VISTAS Y CONSULTAS
        public string Emprnomb { get; set; }
    }
}
