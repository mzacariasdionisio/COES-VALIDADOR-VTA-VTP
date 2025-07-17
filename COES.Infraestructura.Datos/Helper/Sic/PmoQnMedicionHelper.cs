using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PMO_QN_MEDICION
    /// </summary>
    public class PmoQnMedicionHelper : HelperBase
    {
        public PmoQnMedicionHelper() : base(Consultas.PmoQnMedicionSql)
        {
        }

        public PmoQnMedicionDTO Create(IDataReader dr)
        {
            PmoQnMedicionDTO entity = new PmoQnMedicionDTO();

            int iQnmedcodi = dr.GetOrdinal(this.Qnmedcodi);
            if (!dr.IsDBNull(iQnmedcodi)) entity.Qnmedcodi = Convert.ToInt32(dr.GetValue(iQnmedcodi));

            int iSddpcodi = dr.GetOrdinal(this.Sddpcodi);
            if (!dr.IsDBNull(iSddpcodi)) entity.Sddpcodi = Convert.ToInt32(dr.GetValue(iSddpcodi));

            int iQnlectcodi = dr.GetOrdinal(this.Qnlectcodi);
            if (!dr.IsDBNull(iQnlectcodi)) entity.Qnlectcodi = Convert.ToInt32(dr.GetValue(iQnlectcodi));

            int iQnbenvcodi = dr.GetOrdinal(this.Qnbenvcodi);
            if (!dr.IsDBNull(iQnbenvcodi)) entity.Qnbenvcodi = Convert.ToInt32(dr.GetValue(iQnbenvcodi));

            int iQnmedfechaini = dr.GetOrdinal(this.Qnmedfechaini);
            if (!dr.IsDBNull(iQnmedfechaini)) entity.Qnmedfechaini = dr.GetDateTime(iQnmedfechaini);

            int iQnmedfechafin = dr.GetOrdinal(this.Qnmedfechafin);
            if (!dr.IsDBNull(iQnmedfechafin)) entity.Qnmedfechafin = dr.GetDateTime(iQnmedfechafin);

            int iQnmedsemini = dr.GetOrdinal(this.Qnmedsemini);
            if (!dr.IsDBNull(iQnmedsemini)) entity.Qnmedsemini = Convert.ToInt32(dr.GetValue(iQnmedsemini));

            int iQnmedsemfin = dr.GetOrdinal(this.Qnmedsemfin);
            if (!dr.IsDBNull(iQnmedsemfin)) entity.Qnmedsemfin = Convert.ToInt32(dr.GetValue(iQnmedsemfin));

            int iQnmedanio = dr.GetOrdinal(this.Qnmedanio);
            if (!dr.IsDBNull(iQnmedanio)) entity.Qnmedanio = dr.GetDateTime(iQnmedanio);

            int iQnmedh1 = dr.GetOrdinal(this.Qnmedh1);
            if (!dr.IsDBNull(iQnmedh1)) entity.Qnmedh1 = dr.GetDecimal(iQnmedh1);

            int iQnmedh2 = dr.GetOrdinal(this.Qnmedh2);
            if (!dr.IsDBNull(iQnmedh2)) entity.Qnmedh2 = dr.GetDecimal(iQnmedh2);

            int iQnmedh3 = dr.GetOrdinal(this.Qnmedh3);
            if (!dr.IsDBNull(iQnmedh3)) entity.Qnmedh3 = dr.GetDecimal(iQnmedh3);

            int iQnmedh4 = dr.GetOrdinal(this.Qnmedh4);
            if (!dr.IsDBNull(iQnmedh4)) entity.Qnmedh4 = dr.GetDecimal(iQnmedh4);

            int iQnmedh6 = dr.GetOrdinal(this.Qnmedh6);
            if (!dr.IsDBNull(iQnmedh6)) entity.Qnmedh6 = dr.GetDecimal(iQnmedh6);

            int iQnmedh5 = dr.GetOrdinal(this.Qnmedh5);
            if (!dr.IsDBNull(iQnmedh5)) entity.Qnmedh5 = dr.GetDecimal(iQnmedh5);

            int iQnmedh7 = dr.GetOrdinal(this.Qnmedh7);
            if (!dr.IsDBNull(iQnmedh7)) entity.Qnmedh7 = dr.GetDecimal(iQnmedh7);

            int iQnmedh8 = dr.GetOrdinal(this.Qnmedh8);
            if (!dr.IsDBNull(iQnmedh8)) entity.Qnmedh8 = dr.GetDecimal(iQnmedh8);

            int iQnmedh9 = dr.GetOrdinal(this.Qnmedh9);
            if (!dr.IsDBNull(iQnmedh9)) entity.Qnmedh9 = dr.GetDecimal(iQnmedh9);

            int iQnmedh10 = dr.GetOrdinal(this.Qnmedh10);
            if (!dr.IsDBNull(iQnmedh10)) entity.Qnmedh10 = dr.GetDecimal(iQnmedh10);

            int iQnmedh11 = dr.GetOrdinal(this.Qnmedh11);
            if (!dr.IsDBNull(iQnmedh11)) entity.Qnmedh11 = dr.GetDecimal(iQnmedh11);

            int iQnmedh12 = dr.GetOrdinal(this.Qnmedh12);
            if (!dr.IsDBNull(iQnmedh12)) entity.Qnmedh12 = dr.GetDecimal(iQnmedh12);

            int iQnmedh13 = dr.GetOrdinal(this.Qnmedh13);
            if (!dr.IsDBNull(iQnmedh13)) entity.Qnmedh13 = dr.GetDecimal(iQnmedh13);

            int iQnmedo1 = dr.GetOrdinal(this.Qnmedo1);
            if (!dr.IsDBNull(iQnmedo1)) entity.Qnmedo1 = Convert.ToInt32(dr.GetValue(iQnmedo1));

            int iQnmedo2 = dr.GetOrdinal(this.Qnmedo2);
            if (!dr.IsDBNull(iQnmedo2)) entity.Qnmedo2 = Convert.ToInt32(dr.GetValue(iQnmedo2));

            int iQnmedo3 = dr.GetOrdinal(this.Qnmedo3);
            if (!dr.IsDBNull(iQnmedo3)) entity.Qnmedo3 = Convert.ToInt32(dr.GetValue(iQnmedo3));

            int iQnmedo4 = dr.GetOrdinal(this.Qnmedo4);
            if (!dr.IsDBNull(iQnmedo4)) entity.Qnmedo4 = Convert.ToInt32(dr.GetValue(iQnmedo4));

            int iQnmedo5 = dr.GetOrdinal(this.Qnmedo5);
            if (!dr.IsDBNull(iQnmedo5)) entity.Qnmedo5 = Convert.ToInt32(dr.GetValue(iQnmedo5));

            int iQnmedo6 = dr.GetOrdinal(this.Qnmedo6);
            if (!dr.IsDBNull(iQnmedo6)) entity.Qnmedo6 = Convert.ToInt32(dr.GetValue(iQnmedo6));

            int iQnmedo7 = dr.GetOrdinal(this.Qnmedo7);
            if (!dr.IsDBNull(iQnmedo7)) entity.Qnmedo7 = Convert.ToInt32(dr.GetValue(iQnmedo7));

            int iQnmedo8 = dr.GetOrdinal(this.Qnmedo8);
            if (!dr.IsDBNull(iQnmedo8)) entity.Qnmedo8 = Convert.ToInt32(dr.GetValue(iQnmedo8));

            int iQnmedo9 = dr.GetOrdinal(this.Qnmedo9);
            if (!dr.IsDBNull(iQnmedo9)) entity.Qnmedo9 = Convert.ToInt32(dr.GetValue(iQnmedo9));

            int iQnmedo10 = dr.GetOrdinal(this.Qnmedo10);
            if (!dr.IsDBNull(iQnmedo10)) entity.Qnmedo10 = Convert.ToInt32(dr.GetValue(iQnmedo10));

            int iQnmedo11 = dr.GetOrdinal(this.Qnmedo11);
            if (!dr.IsDBNull(iQnmedo11)) entity.Qnmedo11 = Convert.ToInt32(dr.GetValue(iQnmedo11));

            int iQnmedo12 = dr.GetOrdinal(this.Qnmedo12);
            if (!dr.IsDBNull(iQnmedo12)) entity.Qnmedo12 = Convert.ToInt32(dr.GetValue(iQnmedo12));

            int iQnmedo13 = dr.GetOrdinal(this.Qnmedo13);
            if (!dr.IsDBNull(iQnmedo13)) entity.Qnmedo13 = Convert.ToInt32(dr.GetValue(iQnmedo13));

            return entity;
        }


        #region Mapeo de Campos

        public string Qnmedcodi = "QNMEDCODI";
        public string Sddpcodi = "SDDPCODI";
        public string Qnlectcodi = "QNLECTCODI";
        public string Qnbenvcodi = "QNBENVCODI";
        public string Qnmedfechaini = "QNMEDFECHAINI";
        public string Qnmedfechafin = "QNMEDFECHAFIN";
        public string Qnmedsemini = "QNMEDSEMINI";
        public string Qnmedsemfin = "QNMEDSEMFIN";
        public string Qnmedanio = "QNMEDANIO";
        public string Qnmedh1 = "QNMEDH1";
        public string Qnmedh2 = "QNMEDH2";
        public string Qnmedh3 = "QNMEDH3";
        public string Qnmedh4 = "QNMEDH4";
        public string Qnmedh6 = "QNMEDH6";
        public string Qnmedh5 = "QNMEDH5";
        public string Qnmedh7 = "QNMEDH7";
        public string Qnmedh8 = "QNMEDH8";
        public string Qnmedh9 = "QNMEDH9";
        public string Qnmedh10 = "QNMEDH10";
        public string Qnmedh11 = "QNMEDH11";
        public string Qnmedh12 = "QNMEDH12";
        public string Qnmedh13 = "QNMEDH13";
        public string Qnmedo1 = "QNMEDO1";
        public string Qnmedo2 = "QNMEDO2";
        public string Qnmedo3 = "QNMEDO3";
        public string Qnmedo4 = "QNMEDO4";
        public string Qnmedo5 = "QNMEDO5";
        public string Qnmedo6 = "QNMEDO6";
        public string Qnmedo7 = "QNMEDO7";
        public string Qnmedo8 = "QNMEDO8";
        public string Qnmedo9 = "QNMEDO9";
        public string Qnmedo10 = "QNMEDO10";
        public string Qnmedo11 = "QNMEDO11";
        public string Qnmedo12 = "QNMEDO12";
        public string Qnmedo13 = "QNMEDO13";

        #endregion

        public string SqlDeletexEnvio
        {
            get { return GetSqlXml("DeleteMedicionXEnvio"); }
        }
    }
}
