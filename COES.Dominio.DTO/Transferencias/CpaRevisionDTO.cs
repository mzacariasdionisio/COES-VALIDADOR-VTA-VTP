using System;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla CPA_REVISION
    /// </summary>
    public class CpaRevisionDTO
    {
        public int Cparcodi { get; set; }              // Mapeo de CPARCODI
        public int Cpaapcodi { get; set; }             // Mapeo de CPAAPCODI
        public string Cparrevision { get; set; }       // Mapeo de CPARREVISION
        public int Cparcorrelativo { get; set; }       // Mapeo de CPARCORRELATIVO
        public string Cparestado { get; set; }        // Mapeo de CPARPESTADO
        public string Cparultimo { get; set; }         // Mapeo de CPARULTIMO
        public int Cparcmpmpo { get; set; }             // Mapeo de CPARCMPMPO
        public string Cparusucreacion { get; set; }    // Mapeo de CPARUSUCREACION
        public DateTime Cparfeccreacion { get; set; }  // Mapeo de CPARFECCREACION
        public string Cparusumodificacion { get; set; } // Mapeo de CPARUSUMODIFICACION
        public DateTime? Cparfecmodificacion { get; set; } // Mapeo de CPARFECMODIFICACION

        //Additional
        public int Cpaapanio { get; set; }        
        public string Cpaapajuste { get; set; }   
        public int Cpaapanioejercicio { get; set; }
        public string CparestadoDesc { get; set; }
        public string CparfeccreacionDesc { get; set; }
        public string CparfecmodificacionDesc { get; set; }
    }
}