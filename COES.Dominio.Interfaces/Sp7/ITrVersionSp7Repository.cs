using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;
using COES.Dominio.DTO.Sp7;

namespace COES.Dominio.Interfaces.Sp7
{
    /// <summary>
    /// Interface de acceso a datos de la tabla TR_VERSION_SP7
    /// </summary>
    public interface ITrVersionSp7Repository
    {
        int Save(TrVersionSp7DTO entity);
        void Update(TrVersionSp7DTO entity);
        void Delete(int vercodi);
        TrVersionSp7DTO GetById(int vercodi);
        List<TrVersionSp7DTO> List();
        List<TrVersionSp7DTO> List(DateTime verFecha);
        List<TrVersionSp7DTO> ListPendiente();
        List<TrVersionSp7DTO> GetByCriteria();
        int SaveTrVersionSp7Id(TrVersionSp7DTO entity);
        List<TrVersionSp7DTO> BuscarOperaciones(DateTime verFechaini,DateTime verFechafin,int nroPage, int pageSize);
        int ObtenerNroFilas(DateTime verFechaini,DateTime verFechafin);
        int GetVersion(DateTime verFecha);
    }
}
