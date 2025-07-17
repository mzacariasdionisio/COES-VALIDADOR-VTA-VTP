using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SMA_MODO_OPER_VAL
    /// </summary>
    public class SmaModoOperValHelper : HelperBase
    {
        public SmaModoOperValHelper(): base(Consultas.SmaModoOperValSql)
        {
        }

        public SmaModoOperValDTO Create(IDataReader dr)
        {
            SmaModoOperValDTO entity = new SmaModoOperValDTO();

            int iMopvcodi = dr.GetOrdinal(this.Mopvcodi);
            if (!dr.IsDBNull(iMopvcodi)) entity.Mopvcodi = Convert.ToInt32(dr.GetValue(iMopvcodi));

            int iMopvusucreacion = dr.GetOrdinal(this.Mopvusucreacion);
            if (!dr.IsDBNull(iMopvusucreacion)) entity.Mopvusucreacion = dr.GetString(iMopvusucreacion);

            int iMopvfeccreacion = dr.GetOrdinal(this.Mopvfeccreacion);
            if (!dr.IsDBNull(iMopvfeccreacion)) entity.Mopvfeccreacion = dr.GetDateTime(iMopvfeccreacion);

            int iMopvusumodificacion = dr.GetOrdinal(this.Mopvusumodificacion);
            if (!dr.IsDBNull(iMopvusumodificacion)) entity.Mopvusumodificacion = dr.GetString(iMopvusumodificacion);

            int iMopvfecmodificacion = dr.GetOrdinal(this.Mopvfecmodificacion);
            if (!dr.IsDBNull(iMopvfecmodificacion)) entity.Mopvfecmodificacion = dr.GetDateTime(iMopvfecmodificacion);

            int iMopvgrupoval = dr.GetOrdinal(this.Mopvgrupoval);
            if (!dr.IsDBNull(iMopvgrupoval)) entity.Mopvgrupoval = Convert.ToInt32(dr.GetValue(iMopvgrupoval));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iGruponomb = dr.GetOrdinal(this.Gruponomb);
            if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

            int iGrupotipo = dr.GetOrdinal(this.Grupotipo);
            if (!dr.IsDBNull(iGrupotipo)) entity.Grupotipo = dr.GetString(iGrupotipo);

            int iMopvestado = dr.GetOrdinal(this.Mopvestado);
            if (!dr.IsDBNull(iMopvestado)) entity.Mopvestado = dr.GetString(iMopvestado);

            int iUrscodi = dr.GetOrdinal(this.Urscodi);
            if (!dr.IsDBNull(iUrscodi)) entity.Urscodi = Convert.ToInt32(dr.GetValue(iUrscodi));

            int iUrsnomb = dr.GetOrdinal(this.Ursnomb);
            if (!dr.IsDBNull(iUrsnomb)) entity.Ursnomb = dr.GetString(iUrsnomb);

            return entity;
        }

        public SmaModoOperValDTO CreateList(IDataReader dr)
        {
            SmaModoOperValDTO entity = new SmaModoOperValDTO();

            int iMopvcodi = dr.GetOrdinal(this.Mopvcodi);
            if (!dr.IsDBNull(iMopvcodi)) entity.Mopvcodi = Convert.ToInt32(dr.GetValue(iMopvcodi));

            int iMopvusucreacion = dr.GetOrdinal(this.Mopvusucreacion);
            if (!dr.IsDBNull(iMopvusucreacion)) entity.Mopvusucreacion = dr.GetString(iMopvusucreacion);

            int iMopvfeccreacion = dr.GetOrdinal(this.Mopvfeccreacion);
            if (!dr.IsDBNull(iMopvfeccreacion)) entity.Mopvfeccreacion = dr.GetDateTime(iMopvfeccreacion);

            int iMopvusumodificacion = dr.GetOrdinal(this.Mopvusumodificacion);
            if (!dr.IsDBNull(iMopvusumodificacion)) entity.Mopvusumodificacion = dr.GetString(iMopvusumodificacion);

            int iMopvfecmodificacion = dr.GetOrdinal(this.Mopvfecmodificacion);
            if (!dr.IsDBNull(iMopvfecmodificacion)) entity.Mopvfecmodificacion = dr.GetDateTime(iMopvfecmodificacion);

            int iMopvgrupoval = dr.GetOrdinal(this.Mopvgrupoval);
            if (!dr.IsDBNull(iMopvgrupoval)) entity.Mopvgrupoval = Convert.ToInt32(dr.GetValue(iMopvgrupoval));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iMopvestado = dr.GetOrdinal(this.Mopvestado);
            if (!dr.IsDBNull(iMopvestado)) entity.Mopvestado = dr.GetString(iMopvestado);


            return entity;
        }

        public SmaModoOperValDTO CreateListMOVal(IDataReader dr)
        {
            SmaModoOperValDTO entity = new SmaModoOperValDTO();

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iGruponomb = dr.GetOrdinal(this.Gruponomb);
            if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

            return entity;
        }

        public SmaModoOperValDTO CreateMOValxUrs(IDataReader dr)
        {
            SmaModoOperValDTO entity = new SmaModoOperValDTO();

            int iUrscodi = dr.GetOrdinal(this.Urscodi);
            if (!dr.IsDBNull(iUrscodi)) entity.Urscodi = Convert.ToInt32(dr.GetValue(iUrscodi));

            int iMopvgrupoval = dr.GetOrdinal(this.Mopvgrupoval);
            if (!dr.IsDBNull(iMopvgrupoval)) entity.Mopvgrupoval = Convert.ToInt32(dr.GetValue(iMopvgrupoval));

            int iMopvListMOVal = dr.GetOrdinal(this.MopvListMOVal);
            if (!dr.IsDBNull(iMopvListMOVal)) entity.MopvListMOVal = dr.GetString(iMopvListMOVal);

            return entity;
        }


        #region Mapeo de Campos

        public string Mopvcodi = "MOPVCODI";
        public string Mopvusucreacion = "MOPVUSUCREACION";
        public string Mopvfeccreacion = "MOPVFECCREACION";
        public string Mopvusumodificacion = "MOPVUSUMODIFICACION";
        public string Mopvfecmodificacion = "MOPVFECMODIFICACION";
        public string Mopvgrupoval = "MOPVGRUPOVAL";
        public string MopvListMOVal = "MOPVLISTMOVAL";
        public string Grupocodi = "GRUPOCODI";
        public string Gruponomb = "GRUPONOMB";
        public string Grupotipo = "GRUPOTIPO";
        public string Mopvestado = "MOPVESTADO";
        public string Urscodi = "URSCODI";
        public string Ursnomb = "URSNOMB";


        #endregion


        public string SqlListAll
        {
            get { return base.GetSqlXml("ListAll"); }
        }

        public string SqlGetNumVal
        {
            get { return GetSqlXml("GetNumVal"); }
        }

        public string SqlListMOVal
        {
            get { return GetSqlXml("ListMOVal"); }
        }

        public string SqlGetListMOValxUrs
        {
            get { return GetSqlXml("GetListMOValxUrs"); }
        }

    }
}
