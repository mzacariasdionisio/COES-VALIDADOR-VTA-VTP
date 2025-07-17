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
    /// Interface de acceso a datos de la tabla VCE_ARRPAR_COMP_ESP.
    /// </summary>
    public interface IVceArrparCompEspRepository
    {
        void Save(VceArrparCompEspDTO entity);

        void Update(VceArrparCompEspDTO entity);

        void Delete(VceArrparCompEspDTO entity);

        VceArrparCompEspDTO GetById(int periodo, int grupo, string fechaDesc, string apstocodi);

        List<VceArrparCompEspDTO> List();

        List<VceArrparCompEspDTO> GetByPeriod(int periodo);
        
    }
}
