// =======================================================================================
//
// (c) COES Sinac. Todos los derechos reservados. 
//
// Autor: Henry Manuel Díaz Tuesta
// Acronimo: HDT
// Requerimiento: alpha
//
// Fecha creacion: 26/04/2017
// Descripcion: Archivo para la atencion del requerimiento.
//
// Historial de cambios:
// 
// Correlativo	Fecha		Requerimiento		Comentario
//
// =======================================================================================

using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase auxiliar que sostiene las propiedades de cada registro de la tabla 05.
    /// </summary>
    public class IioTabla05DTO
    {

        public int? IdSuministrador { get; set; }

        public string CodSuministradorSicli { get; set; }

        public string SuministradorSicli { get; set; }

        public string Ruc { get; set; }

        public string UsuarioLibre { get; set; }

        public string CodUsuarioLibre { get; set; }

        public string BarraSuministro { get; set; }

        public int AreaDemanda { get; set; }

        public string PagaVad { get; set; }

        public decimal ConsumoEnergiaHp { get; set; }

        public decimal ConsumoEnergiaHfp { get; set; }

        public decimal MaximaDemandaHp { get; set; }

        public decimal MaximaDemandaHfp { get; set; }

    }
}