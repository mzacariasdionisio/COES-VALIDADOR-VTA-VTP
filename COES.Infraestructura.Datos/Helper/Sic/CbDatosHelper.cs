using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CB_DATOS
    /// </summary>
    public class CbDatosHelper : HelperBase
    {
        public CbDatosHelper() : base(Consultas.CbDatosSql)
        {
        }

        public CbDatosDTO Create(IDataReader dr)
        {
            CbDatosDTO entity = new CbDatosDTO();

            int iCbvercodi = dr.GetOrdinal(this.Cbvercodi);
            if (!dr.IsDBNull(iCbvercodi)) entity.Cbvercodi = Convert.ToInt32(dr.GetValue(iCbvercodi));

            int iCbevdacodi = dr.GetOrdinal(this.Cbevdacodi);
            if (!dr.IsDBNull(iCbevdacodi)) entity.Cbevdacodi = Convert.ToInt32(dr.GetValue(iCbevdacodi));

            int iCcombcodi = dr.GetOrdinal(this.Ccombcodi);
            if (!dr.IsDBNull(iCcombcodi)) entity.Ccombcodi = Convert.ToInt32(dr.GetValue(iCcombcodi));

            int iCbevdavalor = dr.GetOrdinal(this.Cbevdavalor);
            if (!dr.IsDBNull(iCbevdavalor)) entity.Cbevdavalor = dr.GetString(iCbevdavalor);

            int iCbevdatipo = dr.GetOrdinal(this.Cbevdatipo);
            if (!dr.IsDBNull(iCbevdatipo)) entity.Cbevdatipo = dr.GetString(iCbevdatipo);

            int iCbevdavalor2 = dr.GetOrdinal(this.Cbevdavalor2);
            if (!dr.IsDBNull(iCbevdavalor2)) entity.Cbevdavalor2 = dr.GetString(iCbevdavalor2);

            int iCbevdatipo2 = dr.GetOrdinal(this.Cbevdatipo2);
            if (!dr.IsDBNull(iCbevdatipo2)) entity.Cbevdatipo2 = dr.GetString(iCbevdatipo2);

            int iCbcentcodi = dr.GetOrdinal(this.Cbcentcodi);
            if (!dr.IsDBNull(iCbcentcodi)) entity.Cbcentcodi = Convert.ToInt32(dr.GetValue(iCbcentcodi));

            int iCbevdaconfidencial = dr.GetOrdinal(this.Cbevdaconfidencial);
            if (!dr.IsDBNull(iCbevdaconfidencial)) entity.Cbevdaconfidencial = Convert.ToInt32(dr.GetValue(iCbevdaconfidencial));

            int iCbevdaestado = dr.GetOrdinal(this.Cbevdaestado);
            if (!dr.IsDBNull(iCbevdaestado)) entity.Cbevdaestado = Convert.ToInt32(dr.GetValue(iCbevdaestado));

            int iCbevdanumdecimal = dr.GetOrdinal(this.Cbevdanumdecimal);
            if (!dr.IsDBNull(iCbevdanumdecimal)) entity.Cbevdanumdecimal = Convert.ToInt32(dr.GetValue(iCbevdanumdecimal));

            return entity;
        }


        #region Mapeo de Campos

        public string Cbvercodi = "CBVERCODI";
        public string Cbevdacodi = "CBEVDACODI";
        public string Ccombcodi = "CCOMBCODI";
        public string Cbevdavalor = "CBEVDAVALOR";
        public string Cbevdatipo = "CBEVDATIPO";
        public string Cbevdavalor2 = "CBEVDAVALOR2";
        public string Cbevdatipo2 = "CBEVDATIPO2";
        public string Cbevdaconfidencial = "CBEVDACONFIDENCIAL";
        public string Cbevdaestado = "CBEVDAESTADO";
        public string Cbevdanumdecimal = "CBEVDANUMDECIMAL";
        public string Cbcentcodi = "CBCENTCODI";
        public string Equicodi = "EQUICODI";

        #endregion

        public string SqlGetCostoCombustibleSolicitado
        {
            get { return base.GetSqlXml("GetCostoCombustibleSolicitado"); }
        }

        public string SqlGetDataReporteCV
        {
            get { return base.GetSqlXml("GetDataReporteCV"); }
        }
    }
}
