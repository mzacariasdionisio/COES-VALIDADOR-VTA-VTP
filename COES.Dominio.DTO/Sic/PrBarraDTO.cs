// =======================================================================================
//
// (c) COES Sinac. Todos los derechos reservados. 
//
// Autor: Henry Manuel Díaz Tuesta
// Acronimo: HDT
// Requerimiento: alpha
//
// Fecha creacion: 27/10/2016
// Descripcion: Archivo para la atencion del requerimiento.
//
// Historial de cambios:
// 
// Correlativo	Fecha		Requerimiento		Comentario
//
// =======================================================================================


using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{

    /// <summary>
    /// Clase que mapea la tabla IIO_ASIGNACION_PENDIENTE
    /// </summary>
    public class PrBarraDTO : EntityBase
    {

        public int Barrcodi { get; set; } 

        public string Barrnombre { get; set; } 

        public string Barrtension { get; set; } 

        public string Barrpuntosuministrorer { get; set; } 

        public string Barrbarrabgr { get; set; } 

        public string Barrestado { get; set; } 

        public string Barrflagbarratransferencia { get; set; } 

        public int? Areacodi { get; set; } 

        public string Barrflagdesbalance { get; set; } 

        public string Barrbarratransferencia { get; set; } 

        public string Barrusername { get; set; }

        public DateTime Barrfecins { get; set; } 

        public DateTime Barrfecact { get; set; }

        public string Osinergcodi { get; set; }

        //- alpha.HDT - 08/07/2017: Cambio para atender el requerimiento. 
        public int Barrbarratransf { get; set; } 

        public int Barrflagbarracompensa { get; set; } 

    }

}
