using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla MAN_REGISTRO
    /// </summary>
    public class ManRegistroHelper : HelperBase
    {
        public ManRegistroHelper()
            : base(Consultas.ManRegistroSql)
        {
        }

        public ManRegistroDTO Create(IDataReader dr)
        {
            ManRegistroDTO entity = new ManRegistroDTO();

            int iRegcodi = dr.GetOrdinal(this.Regcodi);
            if (!dr.IsDBNull(iRegcodi)) entity.Regcodi = Convert.ToInt32(dr.GetValue(iRegcodi));

            int iRegabrev = dr.GetOrdinal(this.Regabrev);
            if (!dr.IsDBNull(iRegabrev)) entity.Regabrev = dr.GetString(iRegabrev);

            int iRegnomb = dr.GetOrdinal(this.Regnomb);
            if (!dr.IsDBNull(iRegnomb)) entity.Regnomb = dr.GetString(iRegnomb);

            int iFechaini = dr.GetOrdinal(this.Fechaini);
            if (!dr.IsDBNull(iFechaini)) entity.Fechaini = dr.GetDateTime(iFechaini);

            int iFechafin = dr.GetOrdinal(this.Fechafin);
            if (!dr.IsDBNull(iFechafin)) entity.Fechafin = dr.GetDateTime(iFechafin);

            int iTregcodi = dr.GetOrdinal(this.Tregcodi);
            if (!dr.IsDBNull(iTregcodi)) entity.Tregcodi = Convert.ToInt32(dr.GetValue(iTregcodi));

            int iEvenclasecodi = dr.GetOrdinal(this.Evenclasecodi);
            if (!dr.IsDBNull(iEvenclasecodi)) entity.Evenclasecodi = Convert.ToInt32(dr.GetValue(iEvenclasecodi));

            int iVersion = dr.GetOrdinal(this.Version);
            if (!dr.IsDBNull(iVersion)) entity.Version = Convert.ToInt32(dr.GetValue(iVersion));

            int iSololectura = dr.GetOrdinal(this.Sololectura);
            if (!dr.IsDBNull(iSololectura)) entity.Sololectura = Convert.ToInt32(dr.GetValue(iSololectura));

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iFechalim = dr.GetOrdinal(this.Fechalim);
            if (!dr.IsDBNull(iFechalim)) entity.Fechalim = dr.GetDateTime(iFechalim);

            return entity;
        }


        public string SqlBuscarManRegistro
        {
            get { return base.GetSqlXml("SqlBuscarManRegistro"); }
        }


        #region Mapeo de Campos

        public string Regcodi = "REGCODI";
        public string Regabrev = "REGABREV";
        public string Regnomb = "REGNOMB";
        public string Fechaini = "FECHAINI";
        public string Fechafin = "FECHAFIN";
        public string Tregcodi = "TREGCODI";
        public string Evenclasecodi = "EVENCLASECODI";
        public string Version = "VERSION";
        public string Sololectura = "SOLOLECTURA";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        public string Fechalim = "FECHALIM";

        #endregion
    }
}

