using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using System.Data;
using COES.Dominio.DTO.Sic;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CP_TERMINAL
    /// </summary>
    public class CpTerminalHelper : HelperBase
    {
        public CpTerminalHelper(): base(Consultas.CpTerminalSql)
        {
        }

        public CpTerminalDTO Create(IDataReader dr)
        {
            CpTerminalDTO entity = new CpTerminalDTO();

            int iTermcodi = dr.GetOrdinal(this.Termcodi);
            if (!dr.IsDBNull(iTermcodi)) entity.Termcodi = Convert.ToInt32(dr.GetValue(iTermcodi));

            int iTermnombre = dr.GetOrdinal(this.Termnombre);
            if (!dr.IsDBNull(iTermnombre)) entity.Termnombre = dr.GetString(iTermnombre);

            int iTtermcodi = dr.GetOrdinal(this.Ttermcodi);
            if (!dr.IsDBNull(iTtermcodi)) entity.Ttermcodi = Convert.ToInt16(dr.GetValue(iTtermcodi));

            int iRecurcodi = dr.GetOrdinal(this.Recurcodi);
            if (!dr.IsDBNull(iRecurcodi)) entity.Recurcodi = Convert.ToInt32(dr.GetValue(iRecurcodi));

            int iNodocodi = dr.GetOrdinal(this.Nodocodi);
            if (!dr.IsDBNull(iNodocodi)) entity.Nodocodi = Convert.ToInt32(dr.GetValue(iNodocodi));

            int iTopcodi = dr.GetOrdinal(this.Topcodi);
            if (!dr.IsDBNull(iTopcodi)) entity.Topcodi = Convert.ToInt32(dr.GetValue(iTopcodi));

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            return entity;
        }


        #region Mapeo de Campos

        public string Termcodi = "TERMCODI";
        public string Termnombre = "TERMNOMBRE";
        public string Ttermcodi = "TTERMCODI";
        public string Recurcodi = "RECURCODI";
        public string Nodocodi = "nodocodi";
        public string Topcodi = "TOPCODI";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        //public string Termcodisicoes = "TERMCODISICOES";

        #endregion

        public string SqlGetNodoTopologico
        {
            get { return base.GetSqlXml("GetNodoTopologico"); }
        }

        public string SqlCrearCopia
        {
            get { return base.GetSqlXml("CrearCopia"); }
        }

        public string SqlCrearCopiaNodoConectividad
        {
            get { return base.GetSqlXml("CrearCopiaNodoConectividad"); }
        }

    }
}
