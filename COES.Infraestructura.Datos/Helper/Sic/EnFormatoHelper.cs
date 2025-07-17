using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EN_FORMATO
    /// </summary>
    public class EnFormatoHelper : HelperBase
    {
        public EnFormatoHelper()
            : base(Consultas.EnFormatoSql)
        {
        }

        public EnFormatoDTO Create(IDataReader dr)
        {
            EnFormatoDTO entity = new EnFormatoDTO();

            int iFormatocodi = dr.GetOrdinal(this.Formatocodi);
            if (!dr.IsDBNull(iFormatocodi)) entity.Formatocodi = Convert.ToInt32(dr.GetValue(iFormatocodi));

            int iFormatodesc = dr.GetOrdinal(this.Formatodesc);
            if (!dr.IsDBNull(iFormatodesc)) entity.Formatodesc = dr.GetString(iFormatodesc);

            int iFormatotipoarchivo = dr.GetOrdinal(this.Formatotipoarchivo);
            if (!dr.IsDBNull(iFormatotipoarchivo)) entity.Formatotipoarchivo = Convert.ToInt32(dr.GetValue(iFormatotipoarchivo));

            int iFormatopadre = dr.GetOrdinal(this.Formatopadre);
            if (!dr.IsDBNull(iFormatopadre)) entity.Formatopadre = Convert.ToInt32(dr.GetValue(iFormatopadre));

            int iFormatoprefijo = dr.GetOrdinal(this.Formatoprefijo);
            if (!dr.IsDBNull(iFormatoprefijo)) entity.Formatoprefijo = dr.GetString(iFormatoprefijo);

            int iFormatonumero = dr.GetOrdinal(this.Formatonumero);
            if (!dr.IsDBNull(iFormatonumero)) entity.Formatonumero = dr.GetDecimal(iFormatonumero);

            int iFormatoestado = dr.GetOrdinal(this.Formatoestado);
            if (!dr.IsDBNull(iFormatoestado)) entity.Formatoestado = Convert.ToInt32(dr.GetValue(iFormatoestado));

            return entity;
        }


        #region Mapeo de Campos

        public string Formatocodi = "ENFMTCODI";
        public string Formatodesc = "ENFMTDESC";
        public string Formatotipoarchivo = "ENFMTTIPOARCHIVO";
        public string Formatopadre = "ENFMTPADRE";
        public string Formatoprefijo = "ENFMTPREFIJO";
        public string Formatonumero = "ENFMTNUMERO";
        public string Formatoestado = "ENFMTESTADO";

        #endregion

        public string SqlListarFormatosActuales
        {
            get { return base.GetSqlXml("ListarFormatosActuales"); }
        }
        public string SqlListarFormatosActualesTodos
        {
            get { return base.GetSqlXml("ListarFormatosActualesTodos"); }
        }

        public string SqlListarFormatosPorPadre
        {
            get { return base.GetSqlXml("ListarFormatosPorPadre"); }
        }
        public string SqlListarFormatosActivos
        {
            get { return GetSqlXml("FormatosActivos"); }
        }

    }
}
