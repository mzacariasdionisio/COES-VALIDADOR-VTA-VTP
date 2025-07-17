using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IN_FACTOR_VERSION_MMAYOR
    /// </summary>
    public class InFactorVersionMmayorHelper : HelperBase
    {
        public InFactorVersionMmayorHelper() : base(Consultas.InFactorVersionMmayorSql)
        {
        }

        public InFactorVersionMmayorDTO Create(IDataReader dr)
        {
            InFactorVersionMmayorDTO entity = new InFactorVersionMmayorDTO();

            int iInfmmcodi = dr.GetOrdinal(this.Infmmcodi);
            if (!dr.IsDBNull(iInfmmcodi)) entity.Infmmcodi = Convert.ToInt32(dr.GetValue(iInfmmcodi));

            int iInfvercodi = dr.GetOrdinal(this.Infvercodi);
            if (!dr.IsDBNull(iInfvercodi)) entity.Infvercodi = Convert.ToInt32(dr.GetValue(iInfvercodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iInfmmdescrip = dr.GetOrdinal(this.Infmmdescrip);
            if (!dr.IsDBNull(iInfmmdescrip)) entity.Infmmdescrip = dr.GetString(iInfmmdescrip);

            int iInfmmfechaini = dr.GetOrdinal(this.Infmmfechaini);
            if (!dr.IsDBNull(iInfmmfechaini)) entity.Infmmfechaini = dr.GetDateTime(iInfmmfechaini);

            int iInfmmfechafin = dr.GetOrdinal(this.Infmmfechafin);
            if (!dr.IsDBNull(iInfmmfechafin)) entity.Infmmfechafin = dr.GetDateTime(iInfmmfechafin);

            int iInfmmduracion = dr.GetOrdinal(this.Infmmduracion);
            if (!dr.IsDBNull(iInfmmduracion)) entity.Infmmduracion = Convert.ToDecimal(dr.GetValue(iInfmmduracion));

            int iClaprocodi = dr.GetOrdinal(this.Claprocodi);
            if (!dr.IsDBNull(iClaprocodi)) entity.Claprocodi = Convert.ToInt32(dr.GetValue(iClaprocodi));

            int iTipoevencodi = dr.GetOrdinal(this.Tipoevencodi);
            if (!dr.IsDBNull(iTipoevencodi)) entity.Tipoevencodi = Convert.ToInt32(dr.GetValue(iTipoevencodi));

            int iInfmmhoja = dr.GetOrdinal(this.Infmmhoja);
            if (!dr.IsDBNull(iInfmmhoja)) entity.Infmmhoja = Convert.ToInt32(dr.GetValue(iInfmmhoja));

            int iInfmmobspm = dr.GetOrdinal(this.Infmmobspm);
            if (!dr.IsDBNull(iInfmmobspm)) entity.Infmmobspm = dr.GetString(iInfmmobspm);

            int iInfmmorigen = dr.GetOrdinal(this.Infmmorigen);
            if (!dr.IsDBNull(iInfmmorigen)) entity.Infmmorigen = dr.GetString(iInfmmorigen);

            int iInfmmjustif = dr.GetOrdinal(this.Infmmjustif);
            if (!dr.IsDBNull(iInfmmjustif)) entity.Infmmjustif = dr.GetString(iInfmmjustif);

            int iInfmmobsps = dr.GetOrdinal(this.Infmmobsps);
            if (!dr.IsDBNull(iInfmmobsps)) entity.Infmmobsps = dr.GetString(iInfmmobsps);

            int iInfmmobspd = dr.GetOrdinal(this.Infmmobspd);
            if (!dr.IsDBNull(iInfmmobspd)) entity.Infmmobspd = dr.GetString(iInfmmobspd);

            int iInfmmobse = dr.GetOrdinal(this.Infmmobse);
            if (!dr.IsDBNull(iInfmmobse)) entity.Infmmobse = dr.GetString(iInfmmobse);

            int iInfmmusumodificacion = dr.GetOrdinal(this.Infmmusumodificacion);
            if (!dr.IsDBNull(iInfmmusumodificacion)) entity.Infmmusumodificacion = dr.GetString(iInfmmusumodificacion);

            int iInfmmfecmodificacion = dr.GetOrdinal(this.Infmmfecmodificacion);
            if (!dr.IsDBNull(iInfmmfecmodificacion)) entity.Infmmfecmodificacion = dr.GetDateTime(iInfmmfecmodificacion);

            return entity;
        }

        #region Mapeo de Campos

        public string Infmmcodi = "INFMMCODI";
        public string Infvercodi = "INFVERCODI";
        public string Equicodi = "EQUICODI";
        public string Emprcodi = "EMPRCODI";

        public string Emprnomb = "EMPRNOMB";
        public string Areanomb = "AREANOMB";
        public string Equiabrev = "EQUIABREV";
        public string Famabrev = "FAMABREV";
        public string Tipoevendesc = "TIPOEVENDESC";
        public string Clapronombre = "CLAPRONOMBRE";

        public string Areacodi = "AREACODI";
        public string Infmmdescrip = "INFMMDESCRIP";
        public string Infmmfechaini = "INFMMFECHAINI";
        public string Infmmfechafin = "INFMMFECHAFIN";
        public string Infmmduracion = "INFMMDURACION";
        public string Claprocodi = "CLAPROCODI";
        public string Tipoevencodi = "TIPOEVENCODI";
        public string Infmmhoja = "INFMMHOJA";
        public string Infmmobspm = "INFMMOBSPM";
        public string Infmmorigen = "INFMMORIGEN";
        public string Infmmjustif = "INFMMJUSTIF";
        public string Infmmobsps = "INFMMOBSPS";
        public string Infmmobspd = "INFMMOBSPD";
        public string Infmmobse = "INFMMOBSE";
        public string Infmmusumodificacion = "INFMMUSUMODIFICACION";
        public string Infmmfecmodificacion = "INFMMFECMODIFICACION";

        #endregion

        public string SqlGetEmpresasByInfvercodi
        {
            get { return GetSqlXml("GetEmpresasByInfvercodi"); }
        }
    }
}
