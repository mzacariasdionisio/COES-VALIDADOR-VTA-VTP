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

using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{

    /// <summary>
    /// Interface que especifica la firma de métodos que se requiere para la funcionalidad de las barras.
    /// </summary>
    public interface IPrBarraRepository
    {

        /// <summary>
        /// Obtiene la lista de barras.
        /// </summary>
        /// <returns></returns>
        List<PrBarraDTO> List();


        //- alpha.JDEL - Inicio 03/11/2016: Cambio para atender el requerimiento.
        PrBarraDTO GetByCodOsinergmin(string codOsinergmin);
        //- JDEL Fin

        //- alpha.HDT - 08/07/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite crear un nuevo registro en la tabla de barras del COES.
        /// </summary>
        /// <param name="bar"></param>
        /// <returns></returns>
        int InsertarBarra(PrBarraDTO bar);

    }
}
