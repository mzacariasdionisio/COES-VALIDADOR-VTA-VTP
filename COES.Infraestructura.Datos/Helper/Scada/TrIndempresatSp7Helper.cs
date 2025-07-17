using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Scada
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla TR_INDEMPRESAT_SP7
    /// </summary>
    public class TrIndempresatSp7Helper : HelperBase
    {
        public TrIndempresatSp7Helper(): base(Consultas.TrIndempresatSp7Sql)
        {
        }

        public TrIndempresatSp7DTO Create(IDataReader dr)
        {
            TrIndempresatSp7DTO entity = new TrIndempresatSp7DTO();

            int iFecha = dr.GetOrdinal(this.Fecha);
            if (!dr.IsDBNull(iFecha)) entity.Fecha = dr.GetDateTime(iFecha);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iMedia = dr.GetOrdinal(this.Media);
            if (!dr.IsDBNull(iMedia)) entity.Media = Convert.ToInt32(dr.GetValue(iMedia));

            int iFactor = dr.GetOrdinal(this.Factor);
            if (!dr.IsDBNull(iFactor)) entity.Factor = Convert.ToInt32(dr.GetValue(iFactor));

            int iMedia2 = dr.GetOrdinal(this.Media2);
            if (!dr.IsDBNull(iMedia2)) entity.Media2 = Convert.ToInt32(dr.GetValue(iMedia2));

            int iFactor2 = dr.GetOrdinal(this.Factor2);
            if (!dr.IsDBNull(iFactor2)) entity.Factor2 = Convert.ToInt32(dr.GetValue(iFactor2));

            int iFindispon = dr.GetOrdinal(this.Findispon);
            if (!dr.IsDBNull(iFindispon)) entity.Findispon = Convert.ToInt32(dr.GetValue(iFindispon));

            int iCiccpe = dr.GetOrdinal(this.Ciccpe);
            if (!dr.IsDBNull(iCiccpe)) entity.Ciccpe = Convert.ToInt32(dr.GetValue(iCiccpe));

            int iCiccpea = dr.GetOrdinal(this.Ciccpea);
            if (!dr.IsDBNull(iCiccpea)) entity.Ciccpea = Convert.ToInt32(dr.GetValue(iCiccpea));

            int iFactorg = dr.GetOrdinal(this.Factorg);
            if (!dr.IsDBNull(iFactorg)) entity.Factorg = Convert.ToInt32(dr.GetValue(iFactorg));

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            return entity;
        }


        #region Mapeo de Campos

        public string Fecha = "FECHA";
        public string Emprcodi = "EMPRCODI";
        public string Media = "MEDIA";
        public string Factor = "FACTOR";
        public string Media2 = "MEDIA2";
        public string Factor2 = "FACTOR2";
        public string Findispon = "FINDISPON";
        public string Ciccpe = "CICCPE";
        public string Ciccpea = "CICCPEA";
        public string Factorg = "FACTORG";
        public string Lastdate = "LASTDATE";

        #endregion

        public string SqlGetDispMensual
        {
            get { return base.GetSqlXml("GetDispMensual"); }
        }

        
        public string SqlObtenerPaginado
        {
            get { return base.GetSqlXml("ObtenerPaginado"); }
        }


    }
}
