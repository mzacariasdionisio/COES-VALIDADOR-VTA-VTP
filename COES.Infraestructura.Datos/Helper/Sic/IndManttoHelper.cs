using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IND_MANTTO
    /// </summary>
    public class IndManttoHelper : HelperBase
    {
        public IndManttoHelper()
            : base(Consultas.IndManttoSql)
        {
        }

        public IndManttoDTO Create(IDataReader dr)
        {
            IndManttoDTO entity = new IndManttoDTO();

            int iIndmancodi = dr.GetOrdinal(this.Indmancodi);
            if (!dr.IsDBNull(iIndmancodi)) entity.Indmancodi = Convert.ToInt32(dr.GetValue(iIndmancodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iManttocodi = dr.GetOrdinal(this.Manttocodi);
            if (!dr.IsDBNull(iManttocodi)) entity.Manttocodi = Convert.ToInt32(dr.GetValue(iManttocodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iEvenclasecodi = dr.GetOrdinal(this.Evenclasecodi);
            if (!dr.IsDBNull(iEvenclasecodi)) entity.Evenclasecodi = Convert.ToInt32(dr.GetValue(iEvenclasecodi));

            int iTipoevencodi = dr.GetOrdinal(this.Tipoevencodi);
            if (!dr.IsDBNull(iTipoevencodi)) entity.Tipoevencodi = Convert.ToInt32(dr.GetValue(iTipoevencodi));

            int iIndmanfecini = dr.GetOrdinal(this.Indmanfecini);
            if (!dr.IsDBNull(iIndmanfecini)) entity.Indmanfecini = dr.GetDateTime(iIndmanfecini);

            int iIndmanfecfin = dr.GetOrdinal(this.Indmanfecfin);
            if (!dr.IsDBNull(iIndmanfecfin)) entity.Indmanfecfin = dr.GetDateTime(iIndmanfecfin);

            int iIndmantipoindisp = dr.GetOrdinal(this.Indmantipoindisp);
            if (!dr.IsDBNull(iIndmantipoindisp)) entity.Indmantipoindisp = dr.GetString(iIndmantipoindisp);

            int iIndmanpr = dr.GetOrdinal(this.Indmanpr);
            if (!dr.IsDBNull(iIndmanpr)) entity.Indmanpr = Convert.ToDecimal(dr.GetValue(iIndmanpr));

            int iIndmanasocproc = dr.GetOrdinal(this.Indmanasocproc);
            if (!dr.IsDBNull(iIndmanasocproc)) entity.Indmanasocproc = dr.GetString(iIndmanasocproc);

            int iIndmanusarencalculo = dr.GetOrdinal(this.Indmanusarencalculo);
            if (!dr.IsDBNull(iIndmanusarencalculo)) entity.Indmanusarencalculo = dr.GetString(iIndmanusarencalculo);

            int iIndmancomentario = dr.GetOrdinal(this.Indmancomentario);
            if (!dr.IsDBNull(iIndmancomentario)) entity.Indmancomentario = dr.GetString(iIndmancomentario);

            int iIndmanestado = dr.GetOrdinal(this.Indmanestado);
            if (!dr.IsDBNull(iIndmanestado)) entity.Indmanestado = dr.GetString(iIndmanestado);

            int iIndmantipoaccion = dr.GetOrdinal(this.Indmantipoaccion);
            if (!dr.IsDBNull(iIndmantipoaccion)) entity.Indmantipoaccion = dr.GetString(iIndmantipoaccion);

            int iIndmanindispo = dr.GetOrdinal(this.Indmanindispo);
            if (!dr.IsDBNull(iIndmanindispo)) entity.Indmanindispo = dr.GetString(iIndmanindispo);

            int iIndmaninterrup = dr.GetOrdinal(this.Indmaninterrup);
            if (!dr.IsDBNull(iIndmaninterrup)) entity.Indmaninterrup = dr.GetString(iIndmaninterrup);

            int iIndmandescripcion = dr.GetOrdinal(this.Indmandescripcion);
            if (!dr.IsDBNull(iIndmandescripcion)) entity.Indmandescripcion = dr.GetString(iIndmandescripcion);

            int iIndmanusucreacion = dr.GetOrdinal(this.Indmanusucreacion);
            if (!dr.IsDBNull(iIndmanusucreacion)) entity.Indmanusucreacion = dr.GetString(iIndmanusucreacion);

            int iIndmanfeccreacion = dr.GetOrdinal(this.Indmanfeccreacion);
            if (!dr.IsDBNull(iIndmanfeccreacion)) entity.Indmanfeccreacion = dr.GetDateTime(iIndmanfeccreacion);

            int iIndmanusumodificacion = dr.GetOrdinal(this.Indmanusumodificacion);
            if (!dr.IsDBNull(iIndmanusumodificacion)) entity.Indmanusumodificacion = dr.GetString(iIndmanusumodificacion);

            int iIndmanfecmodificacion = dr.GetOrdinal(this.Indmanfecmodificacion);
            if (!dr.IsDBNull(iIndmanfecmodificacion)) entity.Indmanfecmodificacion = dr.GetDateTime(iIndmanfecmodificacion);

            int IIndmancodiold = dr.GetOrdinal(this.Indmancodiold);
            if (!dr.IsDBNull(IIndmancodiold)) entity.Indmancodiold = Convert.ToInt32(dr.GetValue(IIndmancodiold));

            int iIndmanomitir7d = dr.GetOrdinal(this.Indmanomitir7d);
            if (!dr.IsDBNull(iIndmanomitir7d)) entity.Indmanomitir7d = dr.GetString(iIndmanomitir7d);

            int iIndmanomitirexcesopr = dr.GetOrdinal(this.Indmanomitirexcesopr);
            if (!dr.IsDBNull(iIndmanomitirexcesopr)) entity.Indmanomitirexcesopr = dr.GetString(iIndmanomitirexcesopr);

            return entity;
        }


        #region Mapeo de Campos

        public string Indmancodi = "INDMANCODI";
        public string Manttocodi = "MANTTOCODI";
        public string Equicodi = "EQUICODI";
        public string Evenclasecodi = "EVENCLASECODI";
        public string Indmanfecini = "INDMANFECINI";
        public string Indmanfecfin = "INDMANFECFIN";
        public string Indmandescripcion = "INDMANDESCRIPCION";
        public string Indmanusucreacion = "INDMANUSUCREACION";
        public string Indmanfeccreacion = "INDMANFECCREACION";
        public string Indmanusumodificacion = "INDMANUSUMODIFICACION";
        public string Indmanfecmodificacion = "INDMANFECMODIFICACION";
        public string Indmanestado = "INDMANESTADO";
        public string Tipoevencodi = "TIPOEVENCODI";
        public string Indmanindispo = "INDMANINDISPO";
        public string Indmaninterrup = "INDMANINTERRUP";

        public string Equipadre = "Equipadre";
        public string Emprcodi = "Emprcodi";
        public string Equiabrev = "Equiabrev";
        public string Grupocodi = "Grupocodi";
        public string Emprnomb = "EMPRNOMB";
        public string Emprabrev = "EMPRABREV";
        public string Evenclasedesc = "EVENCLASEDESC";
        public string Areacodi = "AREACODI";
        public string Areanomb = "AREANOMB";
        public string Areadesc = "AREADESC";
        public string Famcodi = "FAMCODI";
        public string Famnomb = "FAMNOMB";
        public string Equitension = "EQUITENSION";
        public string Tipoevenabrev = "TIPOEVENABREV";
        public string Tipoevendesc = "TIPOEVENDESC";
        public string Tipoemprdesc = "TIPOEMPRDESC";
        public string Osigrupocodi = "OSIGRUPOCODI";
        public string Equinomb = "EQUINOMB";
        public string Evenclaseabrev = "EVENCLASEABREV";
        public string Famabrev = "FAMABREV";
        public string Tipoemprcodi = "TIPOEMPRCODI";
        public string Central = "CENTRAL";
        public string Grupotipocogen = "GRUPOTIPOCOGEN";
        public string Indmantipoindisp = "INDMANTIPOINDISP";
        public string Indmanusarencalculo = "INDMANUSARENCALCULO";
        public string Indmantipoaccion = "INDMANTIPOACCION";
        public string Indmanpr = "INDMANPR";
        public string Indmanasocproc = "INDMANASOCPROC";
        public string Indmancomentario = "INDMANCOMENTARIO";
        public string Indmancodiold = "INDMANCODIOLD";
        public string Indmanomitir7d = "INDMANOMITIR7D";
        public string Indmanomitirexcesopr = "INDMANOMITIREXCESOPR";

        #endregion

        public string SqlIndisponibilidadesIndmanto
        {
            get { return base.GetSqlXml("IndisponibilidadesIndmanto"); }
        }

        public string SqlIndisponibilidadesIndmantoCuadro4
        {
            get { return base.GetSqlXml("IndisponibilidadesIndmantoCuadro4"); }
        }

        public string SqlGetById2
        {
            get { return base.GetSqlXml("GetById2"); }
        }

        public string SqlReporteIndMantto
        {
            get { return base.GetSqlXml("ReporteIndMantto"); }
        }

        public string SqlListarIndManttoByEveMantto
        {
            get { return base.GetSqlXml("ListarIndManttoByEveMantto"); }
        }

        public string SqlListHistoricoByIndmacodi
        {
            get { return base.GetSqlXml("ListHistoricoByIndmacodi"); }
        }

        public string SqlListarIndManttoAppPR25
        {
            get { return base.GetSqlXml("ListarIndManttoAppPR25"); }
        }

    }
}
