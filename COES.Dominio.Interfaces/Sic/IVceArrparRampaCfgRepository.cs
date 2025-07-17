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

using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VCE_ARRPAR_RAMPA_CFG.
    /// </summary>
    public interface IVceArrparRampaCfgRepository
    {

        VceArrparRampaCfgDTO ObtenerRangoInferiorPar(int grupocodi
                                                   , string aptopsubtipo
                                                   , decimal apespcargafinal);


        VceArrparRampaCfgDTO ObtenerRangoSuperiorPar(int grupocodi
                                                   , string aptopsubtipo
                                                   , decimal apespcargafinal);


        VceArrparRampaCfgDTO ObtenerRangoInferiorArr(int grupocodi
                                                   , string aptopsubtipo
                                                   , decimal apespcargafinal);

        VceArrparRampaCfgDTO ObtenerRangoSuperiorArr(int grupocodi
                                                   , string aptopsubtipo
                                                   , decimal apespcargafinal);
    }
}
