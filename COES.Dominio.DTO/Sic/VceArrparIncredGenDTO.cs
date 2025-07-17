// =======================================================================================
//
// (c) COES Sinac. Todos los derechos reservados. 
//
// Autor: Henry Manuel Díaz Tuesta
// Acronimo: HDT
// Requerimiento: compensaciones
//
// Fecha creacion: 21/03/2017
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
    
    public class VceArrparIncredGenDTO
    {
        public int PecaCodi { get; set; }

        public int Grupocodi { get; set; }

        public DateTime Apinrefecha { get; set; }

        public int? Apinrenuminc { get; set; }

        public int? Apinrenumdis { get; set; }

        public string Apinreusucreacion { get; set; }

        public DateTime Apinrefeccreacion { get; set; }

        public string Apinreusumodificacion { get; set; }

        public DateTime? Apinrefecmodificacion { get; set; }

        public string Gruponomb { get; set; }

        public string ApinrefechaDesc { 
            get{
                return this.Apinrefecha.ToString("dd/MM/yyyy"); 
            }

        }

    }

}
