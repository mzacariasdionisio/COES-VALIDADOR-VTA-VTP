using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FT_FICTECDET
    /// </summary>
    public class FtFictecDetHelper : HelperBase
    {
        public FtFictecDetHelper()
            : base(Consultas.FtFictecDetSql)
        {
        }

        public FtFictecDetDTO Create(IDataReader dr)
        {
            FtFictecDetDTO entity = new FtFictecDetDTO();

            int iFteccodi = dr.GetOrdinal(this.Fteccodi);
            if (!dr.IsDBNull(iFteccodi)) entity.Fteccodi = Convert.ToInt32(dr.GetValue(iFteccodi));

            int iFteqcodi = dr.GetOrdinal(this.Fteqcodi);
            if (!dr.IsDBNull(iFteqcodi)) entity.Fteqcodi = Convert.ToInt32(dr.GetValue(iFteqcodi));

            int iFtecdcodi = dr.GetOrdinal(this.Ftecdcodi);
            if (!dr.IsDBNull(iFtecdcodi)) entity.Ftecdcodi = Convert.ToInt32(dr.GetValue(iFtecdcodi));

            int iFtecdfecha = dr.GetOrdinal(this.Ftecdfecha);
            if (!dr.IsDBNull(iFtecdfecha)) entity.Ftecdfecha = dr.GetDateTime(iFtecdfecha);

            int iFtecdusuario = dr.GetOrdinal(this.Ftecdusuario);
            if (!dr.IsDBNull(iFtecdusuario)) entity.Ftecdusuario = dr.GetString(iFtecdusuario);

            return entity;
        }

        #region Mapeo de Campos

        public string Fteccodi = "FTECCODI";
        public string Fteqcodi = "FTEQCODI";
        public string Ftecdcodi = "FTECDCODI";
        public string Ftecdfecha = "FTECDFECHA";
        public string Ftecdusuario = "FTECDUSUARIO";

        #endregion

        #region Mapeo de Consultas

        public string SqlDeleteByFteccodi
        {
            get { return base.GetSqlXml("DeleteByFteccodi"); }
        }

        #endregion

    }
}
