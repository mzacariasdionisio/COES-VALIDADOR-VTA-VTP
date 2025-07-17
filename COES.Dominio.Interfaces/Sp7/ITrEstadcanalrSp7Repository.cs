using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;
using COES.Dominio.DTO.Sp7;

namespace COES.Dominio.Interfaces.Sp7
{
    /// <summary>
    /// Interface de acceso a datos de la tabla TR_ESTADCANALR_SP7
    /// </summary>
    public interface ITrEstadcanalrSp7Repository
    {
        int Save(TrEstadcanalrSp7DTO entity);
        void Update(TrEstadcanalrSp7DTO entity);
        void Delete(int estcnlcodi);
        void DeleteVersion(int vercodi);
        TrEstadcanalrSp7DTO GetById(int estcnlcodi);
        List<TrEstadcanalrSp7DTO> List();
        List<TrEstadcanalrSp7DTO> List(int vercodi,DateTime fecha);
        List<TrEstadcanalrSp7DTO> GetByCriteria();
        int SaveTrEstadcanalrSp7Id(TrEstadcanalrSp7DTO entity);
        List<TrEstadcanalrSp7DTO> BuscarOperaciones(int verCodi, DateTime estcnlFechaIni, DateTime estcnlFechaFin, int nroPage, int pageSize);
        List<TrEstadcanalrSp7DTO> BuscarOperaciones(int verCodi, int emprcodi, int zonacodi, int canalcodi, int segundosDia, DateTime estcnlFechaIni, DateTime estcnlFechaFin, int nroPage, int pageSize);
        int ObtenerNroFilas(int verCodi, DateTime estcnlFechaIni, DateTime estcnlFechaFin);
        int ObtenerNroFilas(int verCodi, int emprcodi, int zonacodi, int canalcodi, DateTime estcnlFechaIni, DateTime estcnlFechaFin);
        List<TrEstadcanalrSp7DTO> GetDispDiaSignal(DateTime fecha, int emprcodi);
    }
}
