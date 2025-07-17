using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SIO_COLUMNAPRIE
    /// </summary>
    public class SioColumnaprieHelper : HelperBase
    {
        public SioColumnaprieHelper(): base(Consultas.SioColumnaprieSql)
        {
        }

        public SioColumnaprieDTO Create(IDataReader dr)
        {
            SioColumnaprieDTO entity = new SioColumnaprieDTO();

            int iCpriecodi = dr.GetOrdinal(this.Cpriecodi);
            if (!dr.IsDBNull(iCpriecodi)) entity.Cpriecodi = Convert.ToInt32(dr.GetValue(iCpriecodi));

            int iCprienombre = dr.GetOrdinal(this.Cprienombre);
            if (!dr.IsDBNull(iCprienombre)) entity.Cprienombre = dr.GetString(iCprienombre);

            int iCpriedescripcion = dr.GetOrdinal(this.Cpriedescripcion);
            if (!dr.IsDBNull(iCpriedescripcion)) entity.Cpriedescripcion = dr.GetString(iCpriedescripcion);

            int iCprietipo = dr.GetOrdinal(this.Cprietipo);
            if (!dr.IsDBNull(iCprietipo)) entity.Cprietipo = Convert.ToInt32(dr.GetValue(iCprietipo));

            int iCprielong1 = dr.GetOrdinal(this.Cprielong1);
            if (!dr.IsDBNull(iCprielong1)) entity.Cprielong1 = Convert.ToInt32(dr.GetValue(iCprielong1));

            int iCprielong2 = dr.GetOrdinal(this.Cprielong2);
            if (!dr.IsDBNull(iCprielong2)) entity.Cprielong2 = Convert.ToInt32(dr.GetValue(iCprielong2));

            int iTpriecodi = dr.GetOrdinal(this.Tpriecodi);
            if (!dr.IsDBNull(iTpriecodi)) entity.Tpriecodi = Convert.ToInt32(dr.GetValue(iTpriecodi));

            int iCprieusumodificacion = dr.GetOrdinal(this.Cprieusumodificacion);
            if (!dr.IsDBNull(iCprieusumodificacion)) entity.Cprieusumodificacion = dr.GetString(iCprieusumodificacion);

            int iCpriefecmodificacion = dr.GetOrdinal(this.Cpriefecmodificacion);
            if (!dr.IsDBNull(iCpriefecmodificacion)) entity.Cpriefecmodificacion = dr.GetDateTime(iCpriefecmodificacion);

            int iCprieusucreacion = dr.GetOrdinal(this.Cprieusucreacion);
            if (!dr.IsDBNull(iCprieusucreacion)) entity.Cprieusucreacion = dr.GetString(iCprieusucreacion);

            int iCpriefeccreacion = dr.GetOrdinal(this.Cpriefeccreacion);
            if (!dr.IsDBNull(iCpriefeccreacion)) entity.Cpriefeccreacion = dr.GetDateTime(iCpriefeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Cpriecodi = "CPRIECODI";
        public string Cprienombre = "CPRIENOMBRE";
        public string Cpriedescripcion = "CPRIEDESCRIPCION";
        public string Cprietipo = "CPRIETIPO";
        public string Cprielong1 = "CPRIELONG1";
        public string Cprielong2 = "CPRIELONG2";
        public string Tpriecodi = "TPRIECODI";
        public string Cprieusumodificacion = "CPRIEUSUMODIFICACION";
        public string Cpriefecmodificacion = "CPRIEFECMODIFICACION";
        public string Cprieusucreacion = "CPRIEUSUCREACION";
        public string Cpriefeccreacion = "CPRIEFECCREACION";

        #endregion
    }
}
