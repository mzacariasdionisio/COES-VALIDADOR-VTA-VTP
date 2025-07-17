// =======================================================================================
//
// (c) COES Sinac. Todos los derechos reservados. 
//
// Autor: Henry Manuel Díaz Tuesta
// Acronimo: HDT
// Requerimiento: compensaciones
//
// Fecha creacion: 02/03/2017
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
    /// <summary>
    /// Clase que mapea la tabla VCE_CFGDATCALCULO
    /// </summary>
    public class VceCfgDatCalculoDTO
    {

        public int Fenergcodi { get; set; }

        public int Concepcodi { get; set; }

        public string Cfgdccamponomb { get; set; }

        public string Cfgdctipoval { get; set; }

        public string Cfgdccondsql { get; set; }

        public string Cfgdcestregistro { get; set; }

    }
}
