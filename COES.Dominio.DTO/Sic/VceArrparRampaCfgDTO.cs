// =======================================================================================
//
// (c) COES Sinac. Todos los derechos reservados. 
//
// Autor: Henry Manuel Díaz Tuesta
// Acronimo: HDT
// Requerimiento: compensaciones
//
// Fecha creacion: 05/04/2017
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

    public class VceArrparRampaCfgDTO
    {

        public int Apramcodi { get; set; }

        public int Grupocodi { get; set; }

        //public string Apramtipo { get; set; }

        public string Apstocodi { get; set; }

        public decimal? Apramhorasacum { get; set; }

        public decimal? Aprampotenciabruta { get; set; }

        public decimal? Apramenergiaacum { get; set; }

        public decimal? Apramconsumobloqued2 { get; set; }

        public decimal? Apramconsumobloquecarb { get; set; }

        public decimal? Apramconsumoacumd2 { get; set; }

        public decimal? Apramconsumoacumcarb { get; set; }

        public string Apramusucreacion { get; set; }

        public DateTime Apramfeccreacion { get; set; }

        public string Apramusumodificacion { get; set; }

        public DateTime Apramfecmodificacion { get; set; }

    }

}
