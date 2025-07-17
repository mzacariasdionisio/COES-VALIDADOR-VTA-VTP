using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EPO_REVISION_EO
    /// </summary>
    public class EpoRevisionEoHelper : HelperBase
    {
        public EpoRevisionEoHelper(): base(Consultas.EpoRevisionEoSql)
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

        public EpoRevisionEoDTO Create(IDataReader dr)
        {
            EpoRevisionEoDTO entity = new EpoRevisionEoDTO();

            int iReveocodi = dr.GetOrdinal(this.Reveocodi);
            if (!dr.IsDBNull(iReveocodi)) entity.Reveocodi = Convert.ToInt32(dr.GetValue(iReveocodi));

            int iEsteocodi = dr.GetOrdinal(this.Esteocodi);
            if (!dr.IsDBNull(iEsteocodi)) entity.Esteocodi = Convert.ToInt32(dr.GetValue(iEsteocodi));

            int iReveorevcoesfechaini = dr.GetOrdinal(this.Reveorevcoesfechaini);
            if (!dr.IsDBNull(iReveorevcoesfechaini)) entity.Reveorevcoesfechaini = dr.GetDateTime(iReveorevcoesfechaini);

            int iReveorevcoescartarevisiontit = dr.GetOrdinal(this.Reveorevcoescartarevisiontit);
            if (!dr.IsDBNull(iReveorevcoescartarevisiontit)) entity.Reveorevcoescartarevisiontit = dr.GetString(iReveorevcoescartarevisiontit);

            int iReveorevcoescartarevisionenl = dr.GetOrdinal(this.Reveorevcoescartarevisionenl);
            if (!dr.IsDBNull(iReveorevcoescartarevisionenl)) entity.Reveorevcoescartarevisionenl = dr.GetString(iReveorevcoescartarevisionenl);

            int iReveorevcoescartarevisionobs = dr.GetOrdinal(this.Reveorevcoescartarevisionobs);
            if (!dr.IsDBNull(iReveorevcoescartarevisionobs)) entity.Reveorevcoescartarevisionobs = dr.GetString(iReveorevcoescartarevisionobs);

            int iReveorevcoesfinalizado = dr.GetOrdinal(this.Reveorevcoesfinalizado);
            if (!dr.IsDBNull(iReveorevcoesfinalizado)) entity.Reveorevcoesfinalizado = dr.GetString(iReveorevcoesfinalizado);

            int iReveocoesfechafin = dr.GetOrdinal(this.Reveocoesfechafin);
            if (!dr.IsDBNull(iReveocoesfechafin)) entity.Reveocoesfechafin = dr.GetDateTime(iReveocoesfechafin);

            int iReveoenvesttercinvfechaini = dr.GetOrdinal(this.Reveoenvesttercinvfechaini);
            if (!dr.IsDBNull(iReveoenvesttercinvfechaini)) entity.Reveoenvesttercinvfechaini = dr.GetDateTime(iReveoenvesttercinvfechaini);

            int iReveoenvesttercinvtit = dr.GetOrdinal(this.Reveoenvesttercinvtit);
            if (!dr.IsDBNull(iReveoenvesttercinvtit)) entity.Reveoenvesttercinvtit = dr.GetString(iReveoenvesttercinvtit);

            int iReveoenvesttercinvenl = dr.GetOrdinal(this.Reveoenvesttercinvenl);
            if (!dr.IsDBNull(iReveoenvesttercinvenl)) entity.Reveoenvesttercinvenl = dr.GetString(iReveoenvesttercinvenl);

            int iReveoenvesttercinvobs = dr.GetOrdinal(this.Reveoenvesttercinvobs);
            if (!dr.IsDBNull(iReveoenvesttercinvobs)) entity.Reveoenvesttercinvobs = dr.GetString(iReveoenvesttercinvobs);

            int iReveoenvesttercinvfinalizado = dr.GetOrdinal(this.Reveoenvesttercinvfinalizado);
            if (!dr.IsDBNull(iReveoenvesttercinvfinalizado)) entity.Reveoenvesttercinvfinalizado = dr.GetString(iReveoenvesttercinvfinalizado);

            int iReveoenvesttercinvinvfechafin = dr.GetOrdinal(this.Reveoenvesttercinvinvfechafin);
            if (!dr.IsDBNull(iReveoenvesttercinvinvfechafin)) entity.Reveoenvesttercinvinvfechafin = dr.GetDateTime(iReveoenvesttercinvinvfechafin);

            int iReveorevterinvfechaini = dr.GetOrdinal(this.Reveorevterinvfechaini);
            if (!dr.IsDBNull(iReveorevterinvfechaini)) entity.Reveorevterinvfechaini = dr.GetDateTime(iReveorevterinvfechaini);

            int iReveorevterinvtit = dr.GetOrdinal(this.Reveorevterinvtit);
            if (!dr.IsDBNull(iReveorevterinvtit)) entity.Reveorevterinvtit = dr.GetString(iReveorevterinvtit);

            int iReveorevterinvenl = dr.GetOrdinal(this.Reveorevterinvenl);
            if (!dr.IsDBNull(iReveorevterinvenl)) entity.Reveorevterinvenl = dr.GetString(iReveorevterinvenl);

            int iReveorevterinvobs = dr.GetOrdinal(this.Reveorevterinvobs);
            if (!dr.IsDBNull(iReveorevterinvobs)) entity.Reveorevterinvobs = dr.GetString(iReveorevterinvobs);

            int iReveorevterinvfinalizado = dr.GetOrdinal(this.Reveorevterinvfinalizado);
            if (!dr.IsDBNull(iReveorevterinvfinalizado)) entity.Reveorevterinvfinalizado = dr.GetString(iReveorevterinvfinalizado);

            int iReveorevterinvfechafin = dr.GetOrdinal(this.Reveorevterinvfechafin);
            if (!dr.IsDBNull(iReveorevterinvfechafin)) entity.Reveorevterinvfechafin = dr.GetDateTime(iReveorevterinvfechafin);

            int iReveolevobsfechaini = dr.GetOrdinal(this.Reveolevobsfechaini);
            if (!dr.IsDBNull(iReveolevobsfechaini)) entity.Reveolevobsfechaini = dr.GetDateTime(iReveolevobsfechaini);

            int iReveolevobstit = dr.GetOrdinal(this.Reveolevobstit);
            if (!dr.IsDBNull(iReveolevobstit)) entity.Reveolevobstit = dr.GetString(iReveolevobstit);

            int iReveolevobsenl = dr.GetOrdinal(this.Reveolevobsenl);
            if (!dr.IsDBNull(iReveolevobsenl)) entity.Reveolevobsenl = dr.GetString(iReveolevobsenl);

            int iReveolevobsobs = dr.GetOrdinal(this.Reveolevobsobs);
            if (!dr.IsDBNull(iReveolevobsobs)) entity.Reveolevobsobs = dr.GetString(iReveolevobsobs);

            int iReveolevobsfinalizado = dr.GetOrdinal(this.Reveolevobsfinalizado);
            if (!dr.IsDBNull(iReveolevobsfinalizado)) entity.Reveolevobsfinalizado = dr.GetString(iReveolevobsfinalizado);

            int iReveolevobsfechafin = dr.GetOrdinal(this.Reveolevobsfechafin);
            if (!dr.IsDBNull(iReveolevobsfechafin)) entity.Reveolevobsfechafin = dr.GetDateTime(iReveolevobsfechafin);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iReveorevcoesampl = dr.GetOrdinal(this.Reveorevcoesampl);
            if (!dr.IsDBNull(iReveorevcoesampl)) entity.Reveorevcoesampl = Convert.ToInt32(dr.GetValue(iReveorevcoesampl));

            int iReveopreampl = dr.GetOrdinal(this.Reveopreampl);
            if (!dr.IsDBNull(iReveopreampl)) entity.Reveopreampl = Convert.ToInt32(dr.GetValue(iReveopreampl));
            #region Mejoras EO-EPO
            int iReveorevterinvampl = dr.GetOrdinal(this.Reveorevterinvampl);
            if (!dr.IsDBNull(iReveorevterinvampl)) entity.Reveorevterinvampl = Convert.ToInt32(dr.GetValue(iReveorevterinvampl));
            #endregion
            return entity;
        }


        #region Mapeo de Campos

        public string Reveocodi = "REVEOCODI";
        public string Esteocodi = "ESTEOCODI";
        public string Reveorevcoesfechaini = "REVEOREVCOESFECHAINI";
        public string Reveorevcoescartarevisiontit = "REVEOREVCOESCARTAREVISIONTIT";
        public string Reveorevcoescartarevisionenl = "REVEOREVCOESCARTAREVISIONENL";
        public string Reveorevcoescartarevisionobs = "REVEOREVCOESCARTAREVISIONOBS";
        public string Reveorevcoesfinalizado = "REVEOREVCOESFINALIZADO";
        public string Reveocoesfechafin = "REVEOCOESFECHAFIN";
        public string Reveoenvesttercinvfechaini = "REVEOENVESTTERCINVFECHAINI";
        public string Reveoenvesttercinvtit = "REVEOENVESTTERCINVTIT";
        public string Reveoenvesttercinvenl = "REVEOENVESTTERCINVENL";
        public string Reveoenvesttercinvobs = "REVEOENVESTTERCINVOBS";
        public string Reveoenvesttercinvfinalizado = "REVEOENVESTTERCINVFINALIZADO";
        public string Reveoenvesttercinvinvfechafin = "REVEOENVESTTERCINVINVFECHAFIN";
        public string Reveorevterinvfechaini = "REVEOREVTERINVFECHAINI";
        public string Reveorevterinvtit = "REVEOREVTERINVTIT";
        public string Reveorevterinvenl = "REVEOREVTERINVENL";
        public string Reveorevterinvobs = "REVEOREVTERINVOBS";
        public string Reveorevterinvfinalizado = "REVEOREVTERINVFINALIZADO";
        public string Reveorevterinvfechafin = "REVEOREVTERINVFECHAFIN";
        public string Reveolevobsfechaini = "REVEOLEVOBSFECHAINI";
        public string Reveolevobstit = "REVEOLEVOBSTIT";
        public string Reveolevobsenl = "REVEOLEVOBSENL";
        public string Reveolevobsobs = "REVEOLEVOBSOBS";
        public string Reveolevobsfinalizado = "REVEOLEVOBSFINALIZADO";
        public string Reveolevobsfechafin = "REVEOLEVOBSFECHAFIN";
        public string Lastdate = "LASTDATE";
        public string Lastuser = "LASTUSER";
        public string Reveorevcoesampl = "REVEOREVCOESAMPL";
        public string Reveorevterinvampl = "REVEOREVTERINVAMPL";

        public string Esteocodiusu = "esteocodiusu";
        public string Esteonomb = "esteonomb";

        public string Reveopreampl = "reveopreampl";

        #endregion
    }
}
