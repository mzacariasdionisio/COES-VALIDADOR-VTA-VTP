//using COES.Infraestructura.Core.Ado;
using COES.Base.Core;
//using COES.Aplicacion.CortoPlazo.DTO;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;
//using COES.Infraestructura.Datos.CortoPlazo.Modelo.Sql;


namespace COES.Infraestructura.Datos.Respositorio.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CP_GRUPORECURSO
    /// </summary>
    public class CpReprogramaHelper : HelperBase
    {
        public CpReprogramaHelper()
            : base(Consultas.CpReprogramaSql)
        {
        }
        public CpReprogramaDTO Create(IDataReader dr)
        {
            CpReprogramaDTO entity = new CpReprogramaDTO();



            int iReprogcodi = dr.GetOrdinal(this.Reprogcodi);
            if (!dr.IsDBNull(iReprogcodi)) entity.Reprogcodi = Convert.ToInt32(dr.GetValue(iReprogcodi));

            int iTopcodi1 = dr.GetOrdinal(this.Topcodi1);
            if (!dr.IsDBNull(iTopcodi1)) entity.Topcodi1 = Convert.ToInt32(dr.GetValue(iTopcodi1));

            int iTopcodi2 = dr.GetOrdinal(this.Topcodi2);
            if (!dr.IsDBNull(iTopcodi2)) entity.Topcodi2 = Convert.ToInt32(dr.GetValue(iTopcodi2));

            int iReprogorden = dr.GetOrdinal(this.Reprogorden);
            if (!dr.IsDBNull(iReprogorden)) entity.Reprogorden = Convert.ToInt32(dr.GetValue(iReprogorden));


            return entity;
        }


        #region Mapeo de Campos

        public string Reprogcodi = "REPROGCODI";
        public string Topcodi1 = "TOPCODI1";
        public string Topcodi2 = "TOPCODI2";
        public string Reprogorden = "REPROGORDEN";
        public string Topnombre = "TOPNOMBRE";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        public string Topiniciohora = "TOPINICIOHORA";
        public string Topfecha = "TOPFECHA";
        public string Tophorareprog = "TOPHORAREPROG";
        public string Topuserdespacho = "TOPUSERDESPACHO";
        #endregion

        public string SqlDeleteAll
        {
            get { return base.GetSqlXml("DeleteAll"); }
        }


        public string SqlListTopPrincipal
        {
            get { return base.GetSqlXml("ListTopPrincipal"); }
        }

    }
}
