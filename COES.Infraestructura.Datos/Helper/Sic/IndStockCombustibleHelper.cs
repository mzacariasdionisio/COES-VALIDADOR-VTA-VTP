using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IND_STOCK_COMBUSTIBLE
    /// </summary>
    public class IndStockCombustibleHelper : HelperBase
    {

        #region Mapeo de Campos
        //table
        public string Stkcmtcodi = "STKCMTCODI";
        public string Ipericodi = "IPERICODI";
        public string Emprcodi = "EMPRCODI";
        public string Equicodicentral = "EQUICODICENTRAL";
        public string Equicodiunidad = "EQUICODIUNIDAD";
        public string Tipoinfocodi = "TIPOINFOCODI";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Stkcmtusucreacion = "STKCMTUSUCREACION";
        public string Stkcmtfeccreacion = "STKCMTFECCREACION";
        public string Stkcmtusumodificacion = "STKCMTUSUMODIFICACION";
        public string Stkcmtfecmodificacion = "STKCMTFECMODIFICACION";

        //additional
        public string Iperinombre = "IPERINOMBRE";
        public string Emprnomb = "EMPRNOMB";
        public string Equinombcentral = "EQUINOMBCENTRAL";
        public string Equinombunidad = "EQUINOMBUNIDAD";
        public string Tipoinfodesc = "TIPOINFODESC";

        public string Stkdetcodi = "STKDETCODI";
        public string Stkdettipo = "STKDETTIPO";
        public string D1 = "D1";
        public string D2 = "D2";
        public string D3 = "D3";
        public string D4 = "D4";
        public string D5 = "D5";
        public string D6 = "D6";
        public string D7 = "D7";
        public string D8 = "D8";
        public string D9 = "D9";
        public string D10 = "D10";
        public string D11 = "D11";
        public string D12 = "D12";
        public string D13 = "D13";
        public string D14 = "D14";
        public string D15 = "D15";
        public string D16 = "D16";
        public string D17 = "D17";
        public string D18 = "D18";
        public string D19 = "D19";
        public string D20 = "D20";
        public string D21 = "D21";
        public string D22 = "D22";
        public string D23 = "D23";
        public string D24 = "D24";
        public string D25 = "D25";
        public string D26 = "D26";
        public string D27 = "D27";
        public string D28 = "D28";
        public string D29 = "D29";
        public string D30 = "D30";
        public string D31 = "D31";
        #endregion

        public IndStockCombustibleHelper() : base(Consultas.IndStockCombustibleSql)
        {
        }

        public IndStockCombustibleDTO Create(IDataReader dr)
        {
            IndStockCombustibleDTO entity = new IndStockCombustibleDTO();
            SetCreate(dr, entity);
            return entity;
        }

        public IndStockCombustibleDTO CreateGetById(IDataReader dr)
        {
            IndStockCombustibleDTO entity = new IndStockCombustibleDTO();
            SetCreate(dr, entity);
            
            int iIperinombre = dr.GetOrdinal(this.Iperinombre);
            if (!dr.IsDBNull(iIperinombre)) entity.Iperinombre = dr.GetString(iIperinombre);

            int iEmprnomb = dr.GetOrdinal(this.Emprnomb);
            if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

            int iEquinombcentral = dr.GetOrdinal(this.Equinombcentral);
            if (!dr.IsDBNull(iEquinombcentral)) entity.Equinombcentral = dr.GetString(iEquinombcentral);

            int iEquinombunidad = dr.GetOrdinal(this.Equinombunidad);
            if (!dr.IsDBNull(iEquinombunidad)) entity.Equinombunidad = dr.GetString(iEquinombunidad);

            int iTipoinfodesc = dr.GetOrdinal(this.Tipoinfodesc);
            if (!dr.IsDBNull(iTipoinfodesc)) entity.Tipoinfodesc = dr.GetString(iTipoinfodesc);

            return entity;
        }

        private void SetCreate(IDataReader dr, IndStockCombustibleDTO entity) 
        {
            int iStkcmtcodi = dr.GetOrdinal(this.Stkcmtcodi);
            if (!dr.IsDBNull(iStkcmtcodi)) entity.Stkcmtcodi = Convert.ToInt32(dr.GetValue(iStkcmtcodi));

            int iIpericodi = dr.GetOrdinal(this.Ipericodi);
            if (!dr.IsDBNull(iIpericodi)) entity.Ipericodi = Convert.ToInt32(dr.GetValue(iIpericodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEquicodicentral = dr.GetOrdinal(this.Equicodicentral);
            if (!dr.IsDBNull(iEquicodicentral)) entity.Equicodicentral = Convert.ToInt32(dr.GetValue(iEquicodicentral));

            int iEquicodiunidad = dr.GetOrdinal(this.Equicodiunidad);
            if (!dr.IsDBNull(iEquicodiunidad)) entity.Equicodiunidad = Convert.ToInt32(dr.GetValue(iEquicodiunidad));

            int iTipoinfocodi = dr.GetOrdinal(this.Tipoinfocodi);
            if (!dr.IsDBNull(iTipoinfocodi)) entity.Tipoinfocodi = Convert.ToInt32(dr.GetValue(iTipoinfocodi));

            int iStkcmtusucreacion = dr.GetOrdinal(this.Stkcmtusucreacion);
            if (!dr.IsDBNull(iStkcmtusucreacion)) entity.Stkcmtusucreacion = dr.GetString(iStkcmtusucreacion);

            int iStkcmtfeccreacion = dr.GetOrdinal(this.Stkcmtfeccreacion);
            if (!dr.IsDBNull(iStkcmtfeccreacion)) entity.Stkcmtfeccreacion = dr.GetDateTime(iStkcmtfeccreacion);

            int iStkcmtusumodificacion = dr.GetOrdinal(this.Stkcmtusumodificacion);
            if (!dr.IsDBNull(iStkcmtusumodificacion)) entity.Stkcmtusumodificacion = dr.GetString(iStkcmtusumodificacion);

            int iStkcmtfecmodificacion = dr.GetOrdinal(this.Stkcmtfecmodificacion);
            if (!dr.IsDBNull(iStkcmtfecmodificacion)) entity.Stkcmtfecmodificacion = dr.GetDateTime(iStkcmtfecmodificacion);
        }

        public string SqlListStockByAnioMes
        {
            get { return base.GetSqlXml("ListStockByAnioMes"); }
        }
    }
}
