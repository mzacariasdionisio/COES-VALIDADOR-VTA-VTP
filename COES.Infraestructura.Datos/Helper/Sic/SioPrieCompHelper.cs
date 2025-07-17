using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SIO_PRIE_COMP
    /// </summary>
    public class SioPrieCompHelper : HelperBase
    {
        public SioPrieCompHelper(): base(Consultas.SioPrieCompSql)
        {
        }

        public SioPrieCompDTO Create(IDataReader dr)
        {
            SioPrieCompDTO entity = new SioPrieCompDTO();

            int iTbcompcodi = dr.GetOrdinal(this.Tbcompcodi);
            if (!dr.IsDBNull(iTbcompcodi)) entity.Tbcompcodi = Convert.ToInt32(dr.GetValue(iTbcompcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iTbcompfecperiodo = dr.GetOrdinal(this.Tbcompfecperiodo);
            if (!dr.IsDBNull(iTbcompfecperiodo)) entity.Tbcompfecperiodo = dr.GetDateTime(iTbcompfecperiodo);

            int iTbcompte = dr.GetOrdinal(this.Tbcompte);
            if (!dr.IsDBNull(iTbcompte)) entity.Tbcompte = dr.GetDecimal(iTbcompte);

            int iTbcomppsr = dr.GetOrdinal(this.Tbcomppsr);
            if (!dr.IsDBNull(iTbcomppsr)) entity.Tbcomppsr = dr.GetDecimal(iTbcomppsr);

            int iTbcomprscd = dr.GetOrdinal(this.Tbcomprscd);
            if (!dr.IsDBNull(iTbcomprscd)) entity.Tbcomprscd = dr.GetDecimal(iTbcomprscd);

            int iTbcomprscul = dr.GetOrdinal(this.Tbcomprscul);
            if (!dr.IsDBNull(iTbcomprscul)) entity.Tbcomprscul = dr.GetDecimal(iTbcomprscul);

            int iTbcompcbec = dr.GetOrdinal(this.Tbcompcbec);
            if (!dr.IsDBNull(iTbcompcbec)) entity.Tbcompcbec = dr.GetDecimal(iTbcompcbec);

            int iTbcompcrf = dr.GetOrdinal(this.Tbcompcrf);
            if (!dr.IsDBNull(iTbcompcrf)) entity.Tbcompcrf = dr.GetDecimal(iTbcompcrf);

            int iTbcompcio = dr.GetOrdinal(this.Tbcompcio);
            if (!dr.IsDBNull(iTbcompcio)) entity.Tbcompcio = dr.GetDecimal(iTbcompcio);

            int iTbcompsma = dr.GetOrdinal(this.Tbcompsma);
            if (!dr.IsDBNull(iTbcompsma)) entity.Tbcompsma = dr.GetDecimal(iTbcompsma);

            int iTbcompoc = dr.GetOrdinal(this.Tbcompoc);
            if (!dr.IsDBNull(iTbcompoc)) entity.Tbcompoc = dr.GetDecimal(iTbcompoc);

            int iTbcompusucreacion = dr.GetOrdinal(this.Tbcompusucreacion);
            if (!dr.IsDBNull(iTbcompusucreacion)) entity.Tbcompusucreacion = dr.GetString(iTbcompusucreacion);

            int iTbcompfeccreacion = dr.GetOrdinal(this.Tbcompfeccreacion);
            if (!dr.IsDBNull(iTbcompfeccreacion)) entity.Tbcompfeccreacion = dr.GetDateTime(iTbcompfeccreacion);

            int iTbcompusumodificacion = dr.GetOrdinal(this.Tbcompusumodificacion);
            if (!dr.IsDBNull(iTbcompusumodificacion)) entity.Tbcompusumodificacion = dr.GetString(iTbcompusumodificacion);

            int iTbcompfecmodificacion = dr.GetOrdinal(this.Tbcompfecmodificacion);
            if (!dr.IsDBNull(iTbcompfecmodificacion)) entity.Tbcompfecmodificacion = dr.GetDateTime(iTbcompfecmodificacion);

            int iTbcompcpa = dr.GetOrdinal(this.Tbcompcpa);
            if (!dr.IsDBNull(iTbcompcpa)) entity.Tbcompcpa = dr.GetDecimal(iTbcompcpa);

            int iTbcompcodosinergmin = dr.GetOrdinal(this.Tbcompcodosinergmin);
            if (!dr.IsDBNull(iTbcompcodosinergmin)) entity.Tbcompcodosinergmin = dr.GetString(iTbcompcodosinergmin);

            return entity;
        }

        #region Mapeo de Campos

        public string Tbcompcodi = "TBCOMPCODI";
        public string Emprcodi = "EMPRCODI";
        public string Tbcompfecperiodo = "TBCOMPFECPERIODO";
        public string Tbcompte = "TBCOMPTE";
        public string Tbcomppsr = "TBCOMPPSR";
        public string Tbcomprscd = "TBCOMPRSCD";
        public string Tbcomprscul = "TBCOMPRSCUL";
        public string Tbcompcbec = "TBCOMPCBEC";
        public string Tbcompcrf = "TBCOMPCRF";
        public string Tbcompcio = "TBCOMPCIO";
        public string Tbcompsma = "TBCOMPSMA";
        public string Tbcompoc = "TBCOMPOC";
        public string Tbcompusucreacion = "TBCOMPUSUCREACION";
        public string Tbcompfeccreacion = "TBCOMPFECCREACION";
        public string Tbcompusumodificacion = "TBCOMPUSUMODIFICACION";
        public string Tbcompfecmodificacion = "TBCOMPFECMODIFICACION";
        public string Tbcompcpa = "TBCOMPCPA";
        public string Tbcompcodosinergmin = "TBCOMPCODOSINERGMIN";

        public string Emprnomb = "EMPRNOMB";
        public string Emprcodosinergmin = "EMPRCODOSINERGMIN";

        #endregion
    }
}
