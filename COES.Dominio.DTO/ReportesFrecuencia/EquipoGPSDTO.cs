using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.ReportesFrecuencia
{
    public class EquipoGPSDTO
    {
        public int GPSCodi { get; set; }
        public int GPSCodiOriginal { get; set; }
        public int EmpresaCodi { get; set; }
        public string Empresa { get; set; }
        public int EquipoCodi { get; set; }
        public string NombreEquipo { get; set; }
        public string GPSOficial { get; set; }
        public string GPSDescOficial { get; set; }
        public string GPSOSINERG { get; set; }
        public string GPSTipo { get; set; }
        public string GPSGenAlarma { get; set; }
        public string GPSDescGenAlarma { get; set; }
        public string GPSEstado { get; set; }
        public string GPSDescEstado { get; set; }
        public DateTime? GPSFecRegi { get; set; }
        public string GPSUsuarioRegi { get; set; }
        public DateTime? GPSFecAct { get; set; }
        public string GPSUsuarioAct { get; set; }
        public string GPSRespaldo { get; set; }
        public string Mensaje { get; set; }
        public string RutaFile { get; set; }

    }
}
