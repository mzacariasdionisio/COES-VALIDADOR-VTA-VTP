using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VCE_LOG_CARGA_DET
    /// </summary>
    public interface IVceLogCargaDetRepository
    {
        void Save(VceLogCargaDetDTO entity);
        void Update(VceLogCargaDetDTO entity);
        void Delete(DateTime crlcdhoraimport, int crlcccodi);
        void DeleteDetPeriodo(int peracodi);
        VceLogCargaDetDTO GetById(DateTime crlcdhoraimport, int crlcccodi);
        List<VceLogCargaDetDTO> List();
        List<VceLogCargaDetDTO> GetByCriteria();

        //- conpensaciones.JDEL - Inicio 03/01/2017: Cambio para atender el requerimiento.
        List<VceLogCargaDetDTO> ListDetalle(int pecacodi);
        void SaveDetalle(int codigo, string usuario, string tabla, int pecacodi);
        //- JDEL Fin
        
    }
}
