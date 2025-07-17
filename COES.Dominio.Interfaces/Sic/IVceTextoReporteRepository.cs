// =======================================================================================
//
// (c) COES Sinac. Todos los derechos reservados. 
//
// Autor: Daniel Sanchez Hermosa
// Acronimo: DSH
// Requerimiento: compensaciones
//
// Fecha creacion: 18/05/2017
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
    /// Interface de acceso a datos de la tabla VCE_TEXTO_REPORTE
    /// </summary>
    public interface IVceTextoReporteRepository
    {
        void Save(VceTextoReporteDTO entity);

        void Update(VceTextoReporteDTO entity);

        void Delete(VceTextoReporteDTO entity);

        VceTextoReporteDTO GetById(int PecaCodi, string Txtrepcodreporte, string Txtrepcodtexto);

        List<VceTextoReporteDTO> ListByPeriodo(int PecaCodi);

        // DSH 27-06-2017 : Se agrego por requerimiento
        void DeleteByVersion(int pecacodi);
        void SaveFromOtherVersion(int pecacodiDestino, int pecacodiOrigen, string usuCreacion);
    }
}
