using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_MENSAJE
    /// </summary>
    public interface IMeMensajeRepository
    {
        int Save(MeMensajeDTO entity);
        void Update(MeMensajeDTO entity);
        MeMensajeDTO GetById(int msjcodi);
        List<MeMensajeDTO> GetListaMensajes(string formatcodi, string emprcodi, DateTime fechaPeriodo);
        List<MeMensajeDTO> GetListaMensajes(DateTime fechaComentario, string idsEmpresa);
        void Update(DateTime fechaPeriodo, int emprcodi, int formatcodi, int areacode);
        
    }
}
