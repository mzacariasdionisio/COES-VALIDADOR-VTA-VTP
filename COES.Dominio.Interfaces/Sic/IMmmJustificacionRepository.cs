using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla MMM_JUSTIFICACION
    /// </summary>
    public interface IMmmJustificacionRepository
    {
        int Save(MmmJustificacionDTO entity);
        void Update(MmmJustificacionDTO entity);
        void Delete(int mjustcodi);
        MmmJustificacionDTO GetById(int mjustcodi);
        List<MmmJustificacionDTO> List();
        List<MmmJustificacionDTO> GetByCriteria();
        List<MmmJustificacionDTO> ListByFechaAndIndicador(int immecodi, DateTime fechaIni, DateTime fechaFin);
    }
}
