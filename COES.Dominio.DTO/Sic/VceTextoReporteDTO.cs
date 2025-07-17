// =======================================================================================
//
// (c) COES Sinac. Todos los derechos reservados. 
//
// Autor: Daniel Sanchez Hermosa
// Acronimo: DSH
// Requerimiento: compensaciones
//
// Fecha creacion: 18/05/2017
// Descripcion: Archivo para la atencion del requerimiento.
//
// Historial de cambios:
// 
// Correlativo	Fecha		Requerimiento		Comentario
//
// =======================================================================================


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{

    public class VceTextoReporteDTO
    {
        public int PecaCodi { get; set; }

        public string Txtrepcodreporte { get; set; }

        public string Txtrepcodtexto { get; set; }
        
        public string Txtreptexto { get; set; }
        
        public string Txtrepusucreacion { get; set; }

        public DateTime Txtrepfeccreacion { get; set; }

        public string Txtrepusumodificacion { get; set; }

        public DateTime? Txtrepfecmodificacion { get; set; }


    }

}
