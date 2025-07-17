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

using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Compensacion.Models.General
{
    public class IncrementosReduccionesModel
    {
        // DSH 14-04-2017 : Cambios por Requerimiento
        public string mensaje { get; set; }
        public int resultado { get; set; }

        /// <summary>
        /// Periodo.
        /// </summary>
        public PeriodoDTO TrnPeriodoDTO { get; set; }

        /// <summary>
        /// Pecacodi
        /// </summary>
        public VcePeriodoCalculoDTO VcePeriodoCalculoDTO { get; set; }

        /// <summary>
        /// Objetivo en edicición.
        /// </summary>
        public VceArrparIncredGenDTO VceArrparIncredGenDTO { get; set; }

        /// <summary>
        /// Lista de registros de incrementos / reducciones.
        /// </summary>
        public List<VceArrparIncredGenDTO> ListVceArrparIncredGenDTO { get; set; }

        /// <summary>
        /// Lista de modos de operación.
        /// </summary>
        public List<PrGrupodatDTO> ListPrGrupodatDTO { get; set; }

    }
}