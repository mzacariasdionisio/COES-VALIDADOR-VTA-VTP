using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_FUENTEENERGIA
    /// </summary>
    public class SiFuenteenergiaHelper : HelperBase
    {
        public SiFuenteenergiaHelper()
            : base(Consultas.SiFuenteenergiaSql)
        {
        }

        public SiFuenteenergiaDTO Create(IDataReader dr)
        {
            SiFuenteenergiaDTO entity = new SiFuenteenergiaDTO();

            int iFenergcodi = dr.GetOrdinal(this.Fenergcodi);
            if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));

            int iFenergabrev = dr.GetOrdinal(this.Fenergabrev);
            if (!dr.IsDBNull(iFenergabrev)) entity.Fenergabrev = dr.GetString(iFenergabrev);

            int iFenergnomb = dr.GetOrdinal(this.Fenergnomb);
            if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

            int iTgenercodi = dr.GetOrdinal(this.Tgenercodi);
            if (!dr.IsDBNull(iTgenercodi)) entity.Tgenercodi = Convert.ToInt32(dr.GetValue(iTgenercodi));

            int iFenercolor = dr.GetOrdinal(this.Fenergcolor);
            if (!dr.IsDBNull(iFenercolor)) entity.Fenergcolor = dr.GetString(iFenercolor);

            int iOsinergcodi = dr.GetOrdinal(this.Osinergcodi);
            if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);

            int iEstcomcodi = dr.GetOrdinal(this.Estcomcodi);
            if (!dr.IsDBNull(iEstcomcodi)) entity.Estcomcodi = Convert.ToInt32(dr.GetValue(iEstcomcodi));

            int iTinfcoes = dr.GetOrdinal(this.Tinfcoes);
            if (!dr.IsDBNull(iTinfcoes)) entity.Tinfcoes = Convert.ToInt32(dr.GetValue(iTinfcoes));

            int iTinfosi = dr.GetOrdinal(this.Tinfosi);
            if (!dr.IsDBNull(iTinfosi)) entity.Tinfosi = Convert.ToInt32(dr.GetValue(iTinfosi));

            return entity;
        }


        #region Mapeo de Campos

        public string Fenergcodi = "FENERGCODI";
        public string Fenergabrev = "FENERGABREV";
        public string Fenergnomb = "FENERGNOMB";
        public string Tgenercodi = "TGENERCODI";
        public string Fenergcolor = "FENERCOLOR";
        public string Estcomcodi = "ESTCOMCODI";
        public string Tgenernomb = "TGENERNOMB";
        public string Grupocomb = "GRUPOCOMB";
        public string Osinergcodi = "OSINERGCODI";
        public string Tinfcoes = "TINFCOES";
        public string Tinfosi = "TINFOSI";
        public string Tinfcoesabrev = "TINFCOESABREV";
        public string Tinfosiabrev = "TINFOSIABREV";
        public string Tinfcoesdesc = "TINFCOESdesc";
        public string Tinfosidesc = "TINFOSIdesc";

        #region SIOSEIN
        public string Promedio = "PROMEDIO";
        #endregion

        #endregion

        public string SqlTipoCombustibleXTipoCentral
        {
            get { return base.GetSqlXml("TipoCombustibleXTipoCentral"); }
        }

        #region PR5

        public string SqlTipoCombustibleXEquipo
        {
            get { return base.GetSqlXml("TipoCombustibleXEquipo"); }
        }
        #endregion

        #region SIOSEIN
         
        public string SqlPromedioEnergiaActivaPorTipodeRecursoYrangoFechas
        {
            get { return base.GetSqlXml("PromedioEnergiaActivaPorTipodeRecursoYrangoFechas"); }
        }

        #endregion
    }
}
