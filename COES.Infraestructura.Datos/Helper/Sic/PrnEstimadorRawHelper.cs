using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;


namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class PrnEstimadorRawHelper : HelperBase
    {
        public PrnEstimadorRawHelper() : base(Consultas.PrnEstimadorRawSql)
        {
        }

        public PrnEstimadorRawDTO Create(IDataReader dr)
        {
            PrnEstimadorRawDTO entity = new PrnEstimadorRawDTO();

            int iEtmrawcodi = dr.GetOrdinal(this.Etmrawcodi);
            if (!dr.IsDBNull(iEtmrawcodi)) entity.Etmrawcodi = Convert.ToInt32(dr.GetValue(iEtmrawcodi));

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iPrnvarcodi = dr.GetOrdinal(this.Prnvarcodi);
            if (!dr.IsDBNull(iPrnvarcodi)) entity.Prnvarcodi = Convert.ToInt32(dr.GetValue(iPrnvarcodi));

            int iEtmrawfuente = dr.GetOrdinal(this.Etmrawfuente);
            if (!dr.IsDBNull(iEtmrawfuente)) entity.Etmrawfuente = Convert.ToInt32(dr.GetValue(iEtmrawfuente));

            int iEtmrawtipomedi = dr.GetOrdinal(this.Etmrawtipomedi);
            if (!dr.IsDBNull(iEtmrawtipomedi)) entity.Etmrawtipomedi = Convert.ToInt32(dr.GetValue(iEtmrawtipomedi));

            int iEtmrawfecha = dr.GetOrdinal(this.Etmrawfecha);
            if (!dr.IsDBNull(iEtmrawfecha)) entity.Etmrawfecha = dr.GetDateTime(iEtmrawfecha);

            int iEtmrawvalor = dr.GetOrdinal(this.Etmrawvalor);
            if (!dr.IsDBNull(iEtmrawvalor)) entity.Etmrawvalor = dr.GetDecimal(iEtmrawvalor);

            return entity;
        }

        #region Mapeo de los campos
        public string Etmrawcodi = "ETMRAWCODI";
        public string Asociacodi = "ASOCIACODI";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Prnvarcodi = "PRNVARCODI";
        public string Etmrawfuente = "ETMRAWFUENTE";
        public string Etmrawfecha = "ETMRAWFECHA";
        public string Etmrawtipomedi = "ETMRAWTIPOMEDI";
        public string Etmrawhora = "ETMRAWHORA";
        public string Etmrawvalor = "ETMRAWVALOR";
        public string Etmrawusucreacion = "ETMRAWUSUCREACION";
        public string Etmrawfeccreacion = "ETMRAWFECCREACION";
        public string Etmrawusumodificacion = "ETMRAWUSUMODIFICACION";
        public string Etmrawfecmodificacion = "ETMRAWFECMODIFICACION";
        public string TableName = "PRN_ESTIMADORRAW";
        #endregion

        #region Consultas a la BD
        public string SqlGetFromDemandaTna
        {
            get { return base.GetSqlXml("GetFromDemandaTna"); }
        }

        public string SqlListDemandaTnaPorUnidad
        {
            get { return base.GetSqlXml("ListDemandaTnaPorUnidad"); }
        }
        
        public string SqlListEstimadorRawPorRangoPorUnidad
        {
            get { return base.GetSqlXml("ListEstimadorRawPorRangoPorUnidad"); }
        }

        public string SqlListEstimadorRawPorRangoPorAsociacion
        {
            get { return base.GetSqlXml("ListEstimadorRawPorRangoPorAsociacion"); }
        }

        public string SqlListEstimadorRawPorUnidad
        {
            get { return base.GetSqlXml("ListEstimadorRawPorUnidad"); }
        }

        public string SqlListEstimadorRawPorAsociacion
        {
            get { return base.GetSqlXml("ListEstimadorRawPorAsociacion"); }
        }
        public string SqlDeletePorFechaIntervalo
        {
            get { return base.GetSqlXml("DeletePorFechaIntervalo"); }
        }
        public string SqlGetMaxIdSco
        {
            get { return base.GetSqlXml("GetMaxIdSco"); }
        }
        public string SqlInsertTableIntoPrnEstimadorRaw
        {
            get { return base.GetSqlXml("InsertTableIntoPrnEstimadorRaw"); }
        }
        public string SqlTruncateTablaTemporal
        {
            get { return base.GetSqlXml("TruncateTablaTemporal"); }
        }
        #endregion
    }
}
