using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FT_EXT_REL_AREAREQ
    /// </summary>
    public class FtExtRelAreareqHelper : HelperBase
    {
        public FtExtRelAreareqHelper(): base(Consultas.FtExtRelAreareqSql)
        {
        }

        public FtExtRelAreareqDTO Create(IDataReader dr)
        {
            FtExtRelAreareqDTO entity = new FtExtRelAreareqDTO();

            int iFaremcodi = dr.GetOrdinal(this.Faremcodi);
            if (!dr.IsDBNull(iFaremcodi)) entity.Faremcodi = Convert.ToInt32(dr.GetValue(iFaremcodi));

            int iFevrqcodi = dr.GetOrdinal(this.Fevrqcodi);
            if (!dr.IsDBNull(iFevrqcodi)) entity.Fevrqcodi = Convert.ToInt32(dr.GetValue(iFevrqcodi));

            int iFrracodi = dr.GetOrdinal(this.Frracodi);
            if (!dr.IsDBNull(iFrracodi)) entity.Frracodi = Convert.ToInt32(dr.GetValue(iFrracodi));

            int iFrraestado = dr.GetOrdinal(this.Frraestado);
            if (!dr.IsDBNull(iFrraestado)) entity.Frraestado = dr.GetString(iFrraestado);

            int iFrrafeccreacion = dr.GetOrdinal(this.Frrafeccreacion);
            if (!dr.IsDBNull(iFrrafeccreacion)) entity.Frrafeccreacion = dr.GetDateTime(iFrrafeccreacion);

            int iFrrausucreacion = dr.GetOrdinal(this.Frrausucreacion);
            if (!dr.IsDBNull(iFrrausucreacion)) entity.Frrausucreacion = dr.GetString(iFrrausucreacion);

            int iFrrafecmodificacion = dr.GetOrdinal(this.Frrafecmodificacion);
            if (!dr.IsDBNull(iFrrafecmodificacion)) entity.Frrafecmodificacion = dr.GetDateTime(iFrrafecmodificacion);

            int iFrrausumodificacion = dr.GetOrdinal(this.Frrausumodificacion);
            if (!dr.IsDBNull(iFrrausumodificacion)) entity.Frrausumodificacion = dr.GetString(iFrrausumodificacion);

            int iFrraflaghidro = dr.GetOrdinal(this.Frraflaghidro);
            if (!dr.IsDBNull(iFrraflaghidro)) entity.Frraflaghidro = dr.GetString(iFrraflaghidro);

            int iFevrqflagtermo = dr.GetOrdinal(this.Frraflagtermo);
            if (!dr.IsDBNull(iFevrqflagtermo)) entity.Frraflagtermo = dr.GetString(iFevrqflagtermo);

            int iFrraflagsolar = dr.GetOrdinal(this.Frraflagsolar);
            if (!dr.IsDBNull(iFrraflagsolar)) entity.Frraflagsolar = dr.GetString(iFrraflagsolar);

            int iFrraflageolico = dr.GetOrdinal(this.Frraflageolico);
            if (!dr.IsDBNull(iFrraflageolico)) entity.Frraflageolico = dr.GetString(iFrraflageolico);

            return entity;
        }


        #region Mapeo de Campos

        public string Faremcodi = "FAREMCODI";
        public string Fevrqcodi = "FEVRQCODI";
        public string Frracodi = "FRRACODI";
        public string Frraestado = "FRRAESTADO";
        public string Frrafeccreacion = "FRRAFECCREACION";
        public string Frrausucreacion = "FRRAUSUCREACION";
        public string Frrafecmodificacion = "FRRAFECMODIFICACION";
        public string Frrausumodificacion = "FRRAUSUMODIFICACION";
        public string Frraflaghidro = "FRRAFLAGHIDRO";
        public string Frraflagtermo = "FRRAFLAGTERMO";
        public string Frraflagsolar = "FRRAFLAGSOLAR";
        public string Frraflageolico = "FRRAFLAGEOLICO";

        public string Faremnombre = "FAREMNOMBRE";
        #endregion

        public string SqlListarPorAreas
        {
            get { return GetSqlXml("ListarPorAreas"); }
        }
    }
}
