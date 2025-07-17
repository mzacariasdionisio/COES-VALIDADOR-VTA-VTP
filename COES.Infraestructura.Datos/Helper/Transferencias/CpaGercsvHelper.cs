using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CPA_GERCSV
    /// </summary>
    public class CpaGercsvHelper : HelperBase
    {
        public CpaGercsvHelper() : base(Consultas.CpaGercsvSql)
        {
        }

        public CpaGercsvDTO Create(IDataReader dr)
        {
            CpaGercsvDTO entity = new CpaGercsvDTO();

            int iCpagercodi = dr.GetOrdinal(Cpagercodi);
            if (!dr.IsDBNull(iCpagercodi)) entity.Cpagercodi = dr.GetInt32(iCpagercodi);

            int iCpsddpcodi = dr.GetOrdinal(Cpsddpcodi);
            if (!dr.IsDBNull(iCpsddpcodi)) entity.Cpsddpcodi = dr.GetInt32(iCpsddpcodi);

            int iCpagergndarchivo = dr.GetOrdinal(Cpagergndarchivo);
            if (!dr.IsDBNull(iCpagergndarchivo)) entity.Cpagergndarchivo = dr.GetString(iCpagergndarchivo);

            int iCpagerhidarchivo = dr.GetOrdinal(Cpagerhidarchivo);
            if (!dr.IsDBNull(iCpagerhidarchivo)) entity.Cpagerhidarchivo = dr.GetString(iCpagerhidarchivo);

            int iCpagerterarchivo = dr.GetOrdinal(Cpagerterarchivo);
            if (!dr.IsDBNull(iCpagerterarchivo)) entity.Cpagerterarchivo = dr.GetString(iCpagerterarchivo);

            int iCpagerdurarchivo = dr.GetOrdinal(Cpagerdurarchivo);
            if (!dr.IsDBNull(iCpagerdurarchivo)) entity.Cpagerdurarchivo = dr.GetString(iCpagerdurarchivo);

            int iCpagerusucreacion = dr.GetOrdinal(Cpagerusucreacion);
            if (!dr.IsDBNull(iCpagerusucreacion)) entity.Cpagerusucreacion = dr.GetString(iCpagerusucreacion);

            int iCpagerfeccreacion = dr.GetOrdinal(Cpagerfeccreacion);
            if (!dr.IsDBNull(iCpagerfeccreacion)) entity.Cpagerfeccreacion = dr.GetDateTime(iCpagerfeccreacion);

            return entity;
        }

        #region Mapeo de Campos
        public string Cpagercodi = "CPAGERCODI";
        public string Cpsddpcodi = "CPSDDPCODI";
        public string Cpagergndarchivo = "CPAGERGNDARCHIVO";
        public string Cpagerhidarchivo = "CPAGERHIDARCHIVO";
        public string Cpagerterarchivo = "CPAGERTERARCHIVO";
        public string Cpagerdurarchivo = "CPAGERDURARCHIVO";
        public string Cpagerusucreacion = "CPAGERUSUCREACION";
        public string Cpagerfeccreacion = "CPAGERFECCREACION";
        #endregion
    }
}

