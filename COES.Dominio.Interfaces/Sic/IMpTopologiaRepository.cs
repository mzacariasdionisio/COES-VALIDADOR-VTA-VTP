using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla MP_TOPOLOGIA
    /// </summary>
    public interface IMpTopologiaRepository
    {
        int Save(MpTopologiaDTO entity);
        void Update(MpTopologiaDTO entity);
        void Delete(int mtopcodi);
        MpTopologiaDTO GetById(int mtopcodi);
        List<MpTopologiaDTO> List();
        List<MpTopologiaDTO> GetByCriteria();
        int Save(MpTopologiaDTO entity, IDbConnection connection, IDbTransaction transaction);
        void Update(MpTopologiaDTO entity, IDbConnection connection, IDbTransaction transaction);
        List<MpTopologiaDTO> ListarEscenariosSddp(string fechaPeriodo, string resolucion, int identificador);
    }
}
