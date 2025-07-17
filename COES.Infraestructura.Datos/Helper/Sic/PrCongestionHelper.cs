using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PR_CONGESTION
    /// </summary>
    public class PrCongestionHelper : HelperBase
    {
        public PrCongestionHelper()
            : base(Consultas.PrCongestionSql)
        {
        }

        public PrCongestionDTO Create(IDataReader dr)
        {
            PrCongestionDTO entity = new PrCongestionDTO();

            int iCongescodi = dr.GetOrdinal(this.Congescodi);
            if (!dr.IsDBNull(iCongescodi)) entity.Congescodi = Convert.ToInt32(dr.GetValue(iCongescodi));

            int iCongesfecinicio = dr.GetOrdinal(this.Congesfecinicio);
            if (!dr.IsDBNull(iCongesfecinicio)) entity.Congesfecinicio = dr.GetDateTime(iCongesfecinicio);

            int iCongesfecfin = dr.GetOrdinal(this.Congesfecfin);
            if (!dr.IsDBNull(iCongesfecfin)) entity.Congesfecfin = dr.GetDateTime(iCongesfecfin);

            int iConfigcodi = dr.GetOrdinal(this.Configcodi);
            if (!dr.IsDBNull(iConfigcodi)) entity.Configcodi = Convert.ToInt32(dr.GetValue(iConfigcodi));

            int iGrulincodi = dr.GetOrdinal(this.Grulincodi);
            if (!dr.IsDBNull(iGrulincodi)) entity.Grulincodi = Convert.ToInt32(dr.GetValue(iGrulincodi));

            int iIndtipo = dr.GetOrdinal(this.Indtipo);
            if (!dr.IsDBNull(iIndtipo)) entity.Indtipo = dr.GetString(iIndtipo);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iCongesmotivo = dr.GetOrdinal(this.Congesmotivo);
            if (!dr.IsDBNull(iCongesmotivo)) entity.Congesmotivo = dr.GetString(iCongesmotivo);

            int iIccodi = dr.GetOrdinal(this.Iccodi);
            if (!dr.IsDBNull(iIccodi)) entity.Iccodi = Convert.ToInt32(dr.GetValue(iIccodi));

            int iRegsegcodi = dr.GetOrdinal(this.Regsegcodi);
            if (!dr.IsDBNull(iRegsegcodi)) entity.Regsegcodi = Convert.ToInt32(dr.GetValue(iRegsegcodi));

            int iHopcodi = dr.GetOrdinal(this.Hopcodi);
            if (!dr.IsDBNull(iHopcodi)) entity.Hopcodi = Convert.ToInt32(dr.GetValue(iHopcodi));

            return entity;
        }

        #region Mapeo de Campos

        public string Congescodi = "CONGESCODI";
        public string Congesfecinicio = "CONGESFECINICIO";
        public string Congesfecfin = "CONGESFECFIN";
        public string Configcodi = "CONFIGCODI";
        public string Grulincodi = "GRULINCODI";
        public string Indtipo = "INDTIPO";
        public string Equinomb = "EQUINOMB";
        public string Grulinnombre = "GRULINNOMBRE";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        public string Equicodi = "EQUICODI";
        public string Nodobarra1 = "NODOBARRA1";
        public string Nodobarra2 = "NODOBARRA2";
        public string Nodobarra3 = "NODOBARRA3";
        public string Famcodi = "FAMCODI";
        public string Idems = "IDEMS";
        public string Congesmotivo = "Congesmotivo";
        public string Iccodi = "ICCODI";
        public string Areanomb = "Areanomb";
        public string Famabrev = "Famabrev";
        public string Areaoperativa = "Areaoperativa";
        public string Nombretna1 = "NOMBRETNA1";
        public string Nombretna2 = "NOMBRETNA2";
        public string Nombretna3 = "NOMBRETNA3";
        public string Regsegcodi = "REGSEGCODI";
        public string Regsegvalorm = "REGSEGVALORM";
        public string Regsegdirec = "REGSEGDIREC";
        public string Regsegnombre = "REGSEGNOMBRE";
        public string Hopcodi = "HOPCODI";
        

        #region SIOSEIN2
        public string Areacodi = "Areacodi";
        #endregion

        #region CMgCP_PR07
        public string Grulintipo = "GRULINTIPO";
        #endregion

        public string SqlObtenerCongestionSimple
        {
            get { return base.GetSqlXml("ObtenerCongestionSimple"); }
        }

        public string SqlObtenerCongestionConjunto
        {
            get { return base.GetSqlXml("ObtenerCongestionConjunto"); }
        }              

        public string SqlObtenerCongestionRegistro
        {
            get { return base.GetSqlXml("ObtenerCongestionRegistro"); }
        }

        public string SqlObtenerCongestionConjuntoRegistro
        {
            get { return base.GetSqlXml("ObtenerCongestionConjuntoRegistro"); }
        }

        public string SqlObtenerCongestionRegionSeguridad
        {
            get { return base.GetSqlXml("ObtenerCongestionRegionSeguridad"); }
        }

        public string SqlObtenerCongestion
        {
            get { return base.GetSqlXml("ObtenerCongestion"); }
        }

        public string SqlListaCongestionConjunto
        {
            get { return base.GetSqlXml("ListaCongestionConjunto"); }
        }

        #endregion

        public string SqlVerificarExistenciaCongestion
        {
            get { return base.GetSqlXml("VerificarExistenciaCongestion"); }
        }
    }
}
