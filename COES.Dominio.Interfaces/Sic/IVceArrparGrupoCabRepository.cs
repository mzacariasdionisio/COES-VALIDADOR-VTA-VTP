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
    /// Interface de acceso a datos de la tabla VCE_ARRPAR_GRUPO_CAB.
    /// </summary>
    public interface IVceArrparGrupoCabRepository
    {
        void Save(VceArrparGrupoCabDTO entity);

        void Update(VceArrparGrupoCabDTO entity);

        void Delete(VceArrparGrupoCabDTO entity);

        List<VceArrparGrupoCabDTO> GetByPeriod(int codPeriodo);

        List<VceArrparGrupoCabDTO> List();

        List<VceArrparGrupoCabDTO> GetByCriteria();

        VceArrparGrupoCabDTO GetById(int PecaCodi, int GrupoCodi, string Apgcfccodi);

        void DeleteEditCalculo(VceArrparGrupoCabDTO oVceArrparGrupoCabDTO);
        
        // DSH 05-05-2017,06-05-2017 : Se agrego por requerimiento
        void SaveFromOtherVersion(int pecacodiDestino, int pecacodiOrigen);
        void DeleteByVersion(int pecacodi);
        
        // DSH 31-05-2017 : Cambio por requerimiento
        List<VceArrparGrupoCabDTO> ListByGroupCompArrpar(int pecacodi, string Apgcfccodi);
    }
}
