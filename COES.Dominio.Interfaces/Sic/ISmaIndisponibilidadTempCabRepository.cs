using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SMA_INDISPONIBILIDAD_TEMP_CAB
    /// </summary>
    public interface ISmaIndisponibilidadTempCabRepository
    {
        int Save(SmaIndisponibilidadTempCabDTO entity);
        int SaveTransaccional(SmaIndisponibilidadTempCabDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(SmaIndisponibilidadTempCabDTO entity);
        void UpdateTransaccional(SmaIndisponibilidadTempCabDTO entity, IDbConnection conn, DbTransaction tran);

        void Delete(int intcabcodi);
        SmaIndisponibilidadTempCabDTO GetById(int intcabcodi);
        List<SmaIndisponibilidadTempCabDTO> List();
        List<SmaIndisponibilidadTempCabDTO> GetByCriteria();
        SmaIndisponibilidadTempCabDTO ObtenerPorFecha(DateTime fecha);
    }
}
