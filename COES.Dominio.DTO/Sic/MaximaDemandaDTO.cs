using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    [Serializable]
    public class MaximaDemandaDTO
    {
        public int CodigoHorario { get; set; }
        public string TipoHorario { get; set; }
        public DateTime FechaHora { get; set; }
        public decimal Valor { get; set; }
        public short Ubicacion { get; set; }
        public decimal ValorInter { get; set; }
        public string FechaHoraFull { get; set; }
        public string FechaOnlyHora { get; set; }
        public string FechaOnlyDia { get; set; }
        public int HDemanda { get; set; }

        #region PR5
        public int Tgenercodi { get; set; }
        public int Fenergcodi { get; set; }
        public int Semana { get; set; }
        public string Emprnomb { get; set; }
        public int Emprcodi { get; set; }
        public string Ptomedidesc { get; set; }
        public int Equicodi { get; set; }
        public int TipoResultadoFecha { get; set; }
        public string SemanaFechaDesc { get; set; }
        public string FechaDesc { get; set; }
        public bool ExisteMD { get; set; }
        public int TipoDemanda { get; set; }
        public int Ptomedicodi { get; set; }
        public int Anio { get; set; }
        #endregion

        public bool TieneCalculoMDenHP { get; set; }
        public string HorarioHPDesc { get; set; }
    }
}
