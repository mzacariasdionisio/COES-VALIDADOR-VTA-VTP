// =======================================================================================
//
// (c) COES Sinac. Todos los derechos reservados. 
//
// Autor: Henry Manuel Díaz Tuesta
// Acronimo: HDT
// Requerimiento: compensaciones
//
// Fecha creacion: 29/03/2016
// Descripcion: Archivo para la atencion del requerimiento.
//
// Historial de cambios:
// 
// Correlativo	Fecha		Requerimiento		Comentario
//
// =======================================================================================


using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Compensacion.Models.General
{
    public class RegistroCalculoManualModel
    {

        /// <summary>
        /// Periodo.
        /// </summary>
        public PeriodoDTO TrnPeriodoDTO { get; set; }
        /// <summary>
        /// Version del Periodo
        /// </summary>
        public VcePeriodoCalculoDTO VcePeriodoCalculoDTO { get; set; }
        /// <summary>
        /// Lista de modos de operación.
        /// </summary>
        public List<PrGrupodatDTO> ListPrGrupodatDTO { get; set; }

        /// <summary>
        /// Objeto Cabecera de los Grupos de Compensación por Arranques y Paradas (APGCAB)
        /// </summary>
        public VceArrparGrupoCabDTO VceArrparGrupoCabDTO { get; set; }

        /// <summary>
        /// Lista de Objetos Cabecera de los Grupos de Compensación por Arranques y Paradas (APGCAB)
        /// </summary>
        public List<VceArrparGrupoCabDTO> ListVceArrparGrupoCabDTO { get; set; }

    }
}