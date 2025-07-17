using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EPO_REVISION_EPO
    /// </summary>
    public class EpoRevisionEpoHelper : HelperBase
    {
        public EpoRevisionEpoHelper(): base(Consultas.EpoRevisionEpoSql)
        {
        }

        public string SqlGetByCriteriaRevisionEstudio
        {
            get { return base.GetSqlXml("GetByCriteriaRevisionEstudio"); }
        }

        public string SqlGetByCriteriaEnvioTerceroInv
        {
            get { return base.GetSqlXml("GetByCriteriaEnvioTerceroInv"); }
        }

        public EpoRevisionEpoDTO Create(IDataReader dr)
        {
            EpoRevisionEpoDTO entity = new EpoRevisionEpoDTO();

            int iRevepocodi = dr.GetOrdinal(this.Revepocodi);
            if (!dr.IsDBNull(iRevepocodi)) entity.Revepocodi = Convert.ToInt32(dr.GetValue(iRevepocodi));

            int iEstepocodi = dr.GetOrdinal(this.Estepocodi);
            if (!dr.IsDBNull(iEstepocodi)) entity.Estepocodi = Convert.ToInt32(dr.GetValue(iEstepocodi));

            int iReveporevcoesfechaini = dr.GetOrdinal(this.Reveporevcoesfechaini);
            if (!dr.IsDBNull(iReveporevcoesfechaini)) entity.Reveporevcoesfechaini = dr.GetDateTime(iReveporevcoesfechaini);

            int iReveporevcoescartarevisiontit = dr.GetOrdinal(this.Reveporevcoescartarevisiontit);
            if (!dr.IsDBNull(iReveporevcoescartarevisiontit)) entity.Reveporevcoescartarevisiontit = dr.GetString(iReveporevcoescartarevisiontit);

            int iReveporevcoescartarevisionenl = dr.GetOrdinal(this.Reveporevcoescartarevisionenl);
            if (!dr.IsDBNull(iReveporevcoescartarevisionenl)) entity.Reveporevcoescartarevisionenl = dr.GetString(iReveporevcoescartarevisionenl);

            int iReveporevcoescartarevisionobs = dr.GetOrdinal(this.Reveporevcoescartarevisionobs);
            if (!dr.IsDBNull(iReveporevcoescartarevisionobs)) entity.Reveporevcoescartarevisionobs = dr.GetString(iReveporevcoescartarevisionobs);

            int iReveporevcoesfinalizado = dr.GetOrdinal(this.Reveporevcoesfinalizado);
            if (!dr.IsDBNull(iReveporevcoesfinalizado)) entity.Reveporevcoesfinalizado = dr.GetString(iReveporevcoesfinalizado);

            int iRevepocoesfechafin = dr.GetOrdinal(this.Revepocoesfechafin);
            if (!dr.IsDBNull(iRevepocoesfechafin)) entity.Revepocoesfechafin = dr.GetDateTime(iRevepocoesfechafin);

            int iRevepoenvesttercinvfechaini = dr.GetOrdinal(this.Revepoenvesttercinvfechaini);
            if (!dr.IsDBNull(iRevepoenvesttercinvfechaini)) entity.Revepoenvesttercinvfechaini = dr.GetDateTime(iRevepoenvesttercinvfechaini);

            int iRevepoenvesttercinvtit = dr.GetOrdinal(this.Revepoenvesttercinvtit);
            if (!dr.IsDBNull(iRevepoenvesttercinvtit)) entity.Revepoenvesttercinvtit = dr.GetString(iRevepoenvesttercinvtit);

            int iRevepoenvesttercinvenl = dr.GetOrdinal(this.Revepoenvesttercinvenl);
            if (!dr.IsDBNull(iRevepoenvesttercinvenl)) entity.Revepoenvesttercinvenl = dr.GetString(iRevepoenvesttercinvenl);

            int iRevepoenvesttercinvobs = dr.GetOrdinal(this.Revepoenvesttercinvobs);
            if (!dr.IsDBNull(iRevepoenvesttercinvobs)) entity.Revepoenvesttercinvobs = dr.GetString(iRevepoenvesttercinvobs);

            int iRevepoenvesttercinvfinalizado = dr.GetOrdinal(this.Revepoenvesttercinvfinalizado);
            if (!dr.IsDBNull(iRevepoenvesttercinvfinalizado)) entity.Revepoenvesttercinvfinalizado = dr.GetString(iRevepoenvesttercinvfinalizado);

            int iRevepoenvesttercinvinvfechafin = dr.GetOrdinal(this.Revepoenvesttercinvinvfechafin);
            if (!dr.IsDBNull(iRevepoenvesttercinvinvfechafin)) entity.Revepoenvesttercinvinvfechafin = dr.GetDateTime(iRevepoenvesttercinvinvfechafin);

            int iReveporevterinvfechaini = dr.GetOrdinal(this.Reveporevterinvfechaini);
            if (!dr.IsDBNull(iReveporevterinvfechaini)) entity.Reveporevterinvfechaini = dr.GetDateTime(iReveporevterinvfechaini);

            int iReveporevterinvtit = dr.GetOrdinal(this.Reveporevterinvtit);
            if (!dr.IsDBNull(iReveporevterinvtit)) entity.Reveporevterinvtit = dr.GetString(iReveporevterinvtit);

            int iReveporevterinvenl = dr.GetOrdinal(this.Reveporevterinvenl);
            if (!dr.IsDBNull(iReveporevterinvenl)) entity.Reveporevterinvenl = dr.GetString(iReveporevterinvenl);

            int iReveporevterinvobs = dr.GetOrdinal(this.Reveporevterinvobs);
            if (!dr.IsDBNull(iReveporevterinvobs)) entity.Reveporevterinvobs = dr.GetString(iReveporevterinvobs);

            int iReveporevterinvfinalizado = dr.GetOrdinal(this.Reveporevterinvfinalizado);
            if (!dr.IsDBNull(iReveporevterinvfinalizado)) entity.Reveporevterinvfinalizado = dr.GetString(iReveporevterinvfinalizado);

            int iReveporevterinvfechafin = dr.GetOrdinal(this.Reveporevterinvfechafin);
            if (!dr.IsDBNull(iReveporevterinvfechafin)) entity.Reveporevterinvfechafin = dr.GetDateTime(iReveporevterinvfechafin);

            int iRevepolevobsfechaini = dr.GetOrdinal(this.Revepolevobsfechaini);
            if (!dr.IsDBNull(iRevepolevobsfechaini)) entity.Revepolevobsfechaini = dr.GetDateTime(iRevepolevobsfechaini);

            int iRevepolevobstit = dr.GetOrdinal(this.Revepolevobstit);
            if (!dr.IsDBNull(iRevepolevobstit)) entity.Revepolevobstit = dr.GetString(iRevepolevobstit);

            int iRevepolevobsenl = dr.GetOrdinal(this.Revepolevobsenl);
            if (!dr.IsDBNull(iRevepolevobsenl)) entity.Revepolevobsenl = dr.GetString(iRevepolevobsenl);

            int iRevepolevobsobs = dr.GetOrdinal(this.Revepolevobsobs);
            if (!dr.IsDBNull(iRevepolevobsobs)) entity.Revepolevobsobs = dr.GetString(iRevepolevobsobs);

            int iRevepolevobsfinalizado = dr.GetOrdinal(this.Revepolevobsfinalizado);
            if (!dr.IsDBNull(iRevepolevobsfinalizado)) entity.Revepolevobsfinalizado = dr.GetString(iRevepolevobsfinalizado);

            int iRevepolevobsfechafin = dr.GetOrdinal(this.Revepolevobsfechafin);
            if (!dr.IsDBNull(iRevepolevobsfechafin)) entity.Revepolevobsfechafin = dr.GetDateTime(iRevepolevobsfechafin);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iReveporevcoesampl = dr.GetOrdinal(this.Reveporevcoesampl);
            if (!dr.IsDBNull(iReveporevcoesampl)) entity.Reveporevcoesampl = Convert.ToInt32(dr.GetValue(iReveporevcoesampl));

            int iReveporevterinvampl = dr.GetOrdinal(this.Reveporevterinvampl);
            if (!dr.IsDBNull(iReveporevterinvampl)) entity.Reveporevterinvampl = Convert.ToInt32(dr.GetValue(iReveporevterinvampl));

            int iRevepopreampl = dr.GetOrdinal(this.Revepopreampl);
            if (!dr.IsDBNull(iRevepopreampl)) entity.Revepopreampl = Convert.ToInt32(dr.GetValue(iRevepopreampl));


            return entity;
        }


        #region Mapeo de Campos

        public string Revepocodi = "REVEPOCODI";
        public string Estepocodi = "ESTEPOCODI";
        public string Reveporevcoesfechaini = "REVEPOREVCOESFECHAINI";
        public string Reveporevcoescartarevisiontit = "REVEPOREVCOESCARTAREVISIONTIT";
        public string Reveporevcoescartarevisionenl = "REVEPOREVCOESCARTAREVISIONENL";
        public string Reveporevcoescartarevisionobs = "REVEPOREVCOESCARTAREVISIONOBS";
        public string Reveporevcoesfinalizado = "REVEPOREVCOESFINALIZADO";
        public string Revepocoesfechafin = "REVEPOCOESFECHAFIN";
        public string Revepoenvesttercinvfechaini = "REVEPOENVESTTERCINVFECHAINI";
        public string Revepoenvesttercinvtit = "REVEPOENVESTTERCINVTIT";
        public string Revepoenvesttercinvenl = "REVEPOENVESTTERCINVENL";
        public string Revepoenvesttercinvobs = "REVEPOENVESTTERCINVOBS";
        public string Revepoenvesttercinvfinalizado = "REVEPOENVESTTERCINVFINALIZADO";
        public string Revepoenvesttercinvinvfechafin = "REVEPOENVESTTERCINVINVFECHAFIN";
        public string Reveporevterinvfechaini = "REVEPOREVTERINVFECHAINI";
        public string Reveporevterinvtit = "REVEPOREVTERINVTIT";
        public string Reveporevterinvenl = "REVEPOREVTERINVENL";
        public string Reveporevterinvobs = "REVEPOREVTERINVOBS";
        public string Reveporevterinvfinalizado = "REVEPOREVTERINVFINALIZADO";
        public string Reveporevterinvfechafin = "REVEPOREVTERINVFECHAFIN";
        public string Revepolevobsfechaini = "REVEPOLEVOBSFECHAINI";
        public string Revepolevobstit = "REVEPOLEVOBSTIT";
        public string Revepolevobsenl = "REVEPOLEVOBSENL";
        public string Revepolevobsobs = "REVEPOLEVOBSOBS";
        public string Revepolevobsfinalizado = "REVEPOLEVOBSFINALIZADO";
        public string Revepolevobsfechafin = "REVEPOLEVOBSFECHAFIN";
        public string Lastdate = "LASTDATE";
        public string Lastuser = "LASTUSER";
        public string Reveporevcoesampl = "REVEPOREVCOESAMPL";
        public string Reveporevterinvampl = "REVEPOREVTERINVAMPL";

        public string Estepocodiusu = "estepocodiusu";
        public string Esteponomb = "esteponomb";

        public string Revepopreampl = "revepopreampl";

        

        #endregion
    }
}
