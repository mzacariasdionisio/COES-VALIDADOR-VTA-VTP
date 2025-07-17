// =======================================================================================
//
// (c) COES Sinac. Todos los derechos reservados. 
//
// Autor: Henry Manuel Díaz Tuesta
// Acronimo: HDT
// Requerimiento: compensaciones
//
// Fecha creacion: 29/03/2017
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

    public class VceArrparGrupoCabDTO
    {
        public int PecaCodi { get; set; }

        public int Grupocodi { get; set; }

        public string Apgcfccodi { get; set; } 

        public decimal? Apgcabccbef { get; set; }

        public decimal? Apgcabccmarr { get; set; }

        public decimal? Apgcabccadic { get; set; }

        public string Apgcabflagcalcmanual { get; set; }

        public string Gruponomb { get; set; }

        // Inicio de Agregado 31-05-2017 - Sistema de Compensaciones
        public string Emprnomb { get; set; }
        public decimal Apgcabtotal { get; set; }
        // Fin de Agregado - Sistema de Compensaciones
    }

}
