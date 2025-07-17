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
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VCE_ARRPAR_INCRED_GEN.
    /// </summary>
    public interface IVceArrparIncredGenRepository
    {
        void Save(VceArrparIncredGenDTO entity);

        void Update(VceArrparIncredGenDTO entity);

        void Delete(VceArrparIncredGenDTO entity);

        List<VceArrparIncredGenDTO> GetByPeriod(int codPeriodo);

        List<VceArrparIncredGenDTO> List();

        List<VceArrparIncredGenDTO> GetByCriteria();

        VceArrparIncredGenDTO GetById(int PecaCodi, int GrupoCodi, string ApinrefechaDesc);

    }
}
