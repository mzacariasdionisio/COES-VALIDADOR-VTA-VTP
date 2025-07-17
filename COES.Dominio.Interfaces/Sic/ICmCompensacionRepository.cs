using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CM_COMPENSACION
    /// </summary>
    public interface ICmCompensacionRepository
    {
        int Save(CmCompensacionDTO entity);
        void Update(CmCompensacionDTO entity);
        void Delete(int compcodi);
        CmCompensacionDTO GetById(int compcodi);
        List<CmCompensacionDTO> List();
        List<CmCompensacionDTO> GetByCriteria();
        void DeleteByCriteria(int intervalo,DateTime fecha);

        List<CmCompensacionDTO> GetCompensacionporCalificacion(DateTime fecha, int subCausaEvenCodi);
        List<CmCompensacionDTO> GetCompensacionporCalificacionParticipante(DateTime fecha, int subCausaEvenCodi, int empcodi);
    }
}
