using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SMA_INDISPONIBILIDAD_TEMP_DET
    /// </summary>
    public interface ISmaIndisponibilidadTempDetRepository
    {
        int Save(SmaIndisponibilidadTempDetDTO entity);
        int SaveTransaccional(SmaIndisponibilidadTempDetDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(SmaIndisponibilidadTempDetDTO entity);
        void Delete(int intdetcodi);
        void DeletePorCabTransaccional(string intcabcodis, IDbConnection conn, DbTransaction tran);
        
        SmaIndisponibilidadTempDetDTO GetById(int intdetcodi);
        List<SmaIndisponibilidadTempDetDTO> List();
        List<SmaIndisponibilidadTempDetDTO> GetByCriteria();
        List<SmaIndisponibilidadTempDetDTO> ListarPorFecha(DateTime fecha);
    }
}
