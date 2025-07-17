using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EVE_HO_EQUIPOREL
    /// </summary>
    public class EveHoEquiporelHelper : HelperBase
    {
        public EveHoEquiporelHelper()
            : base(Consultas.EveHoEquiporelSql)
        {
        }

        public EveHoEquiporelDTO Create(IDataReader dr)
        {
            EveHoEquiporelDTO entity = new EveHoEquiporelDTO();

            int iHoequicodi = dr.GetOrdinal(this.Hoequicodi);
            if (!dr.IsDBNull(iHoequicodi)) entity.Hoequicodi = Convert.ToInt32(dr.GetValue(iHoequicodi));

            int iHopcodi = dr.GetOrdinal(this.Hopcodi);
            if (!dr.IsDBNull(iHopcodi)) entity.Hopcodi = Convert.ToInt32(dr.GetValue(iHopcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iIccodi = dr.GetOrdinal(this.Iccodi);
            if (!dr.IsDBNull(iIccodi)) entity.Iccodi = Convert.ToInt32(dr.GetValue(iIccodi));

            int iHoequitipo = dr.GetOrdinal(this.Hoequitipo);
            if (!dr.IsDBNull(iHoequitipo)) entity.Hoequitipo = Convert.ToInt32(dr.GetValue(iHoequitipo));

            return entity;
        }

        #region Mapeo de Campos

        public string Hopcodi = "HOPCODI";
        public string Equicodi = "EQUICODI";
        public string Iccodi = "ICCODI";
        public string Hoequicodi = "HOEQUICODI";
        public string Hoequitipo = "HOEQUITIPO";

        public string Subcausacodi = "SUBCAUSACODI";
        public string Subcausadesc = "SUBCAUSADESC";
        public string Ichorini = "ICHORINI";
        public string Ichorfin = "ICHORFIN";
        public string Icvalor1 = "ICVALOR1";

        #endregion

        #region Querys

        public string SqlDeleteByHopcodi
        {
            get { return base.GetSqlXml("DeleteByHopcodi"); }
        }

        public string SqlListaByHopcodi
        {
            get { return base.GetSqlXml("ListaByHopcodi"); }
        }

        #endregion
    }
}
