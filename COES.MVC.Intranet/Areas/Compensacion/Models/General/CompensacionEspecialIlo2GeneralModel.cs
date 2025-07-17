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

namespace COES.MVC.Intranet.Areas.Compensacion.Models
{
    public class CompensacionEspecialIlo2GeneralModel
    {

        /// <summary>
        /// Periodo.
        /// </summary>
        public PeriodoDTO TrnPeriodoDTO { get; set; }

        /// <summary>
        /// Lista de modos de operación.
        /// </summary>
        // DSH 26-04-2017 : Se agrego por requerimiento
        public VcePeriodoCalculoDTO VcePeriodoCalculoDTO { get; set; }

        public List<PrGrupodatDTO> ListPrGrupodatDTO { get; set; }

        /// <summary>
        /// Objeto Compensación especial de arranque y parada (APESP)
        /// </summary>
        public VceArrparCompEspDTO VceArrparCompEspDTO { get; set; }

        /// <summary>
        /// Lista de objetos Compensación especial de arranque y parada (APESP)
        /// </summary>
        public List<VceArrparCompEspDTO> ListVceArrparCompEspDTO { get; set; }

        /// <summary>
        /// Lista de objetos Tipo de condición de operación del arranque/parada (APTOP)
        /// </summary>
        public List<VceArrparTipoOperaDTO> ListVceArrparTipoOperaDTO { get; set; }

    }
}