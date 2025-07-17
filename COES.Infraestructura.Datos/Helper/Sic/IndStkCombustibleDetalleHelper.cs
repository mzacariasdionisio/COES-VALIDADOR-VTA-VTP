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
    /// Clase que contiene el mapeo de la tabla IND_STKCOMBUSTIBLE_DETALLE
    /// </summary>
    public class IndStkCombustibleDetalleHelper : HelperBase
    {
        #region Mapeo de Campos
        //tabla
        public string Stkdetcodi = "STKDETCODI";
        public string Stkcmtcodi = "STKCMTCODI";
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
        public string Stkdetusucreacion = "STKDETUSUCREACION";
        public string Stkdetfeccreacion = "STKDETFECCREACION";
        public string Stkdetusumodificacion = "STKDETUSUMODIFICACION";
        public string Stkdetfecmodificacion = "STKDETFECMODIFICACION";


        //additional
        public string Ipericodi = "IPERICODI";
        public string Emprcodi = "EMPRCODI";
        public string Equicodicentral = "EQUICODICENTRAL";
        public string Equicodiunidad = "EQUICODIUNIDAD";
        public string Tipoinfocodi = "TIPOINFOCODI";
        public string Emprnomb = "EMPRNOMB";
        public string Equinombcentral = "EQUINOMBCENTRAL";
        public string Equinombunidad = "EQUINOMBUNIDAD";
        public string Tipoinfodesc = "TIPOINFODESC";

        #endregion

        #region Constructor
        public IndStkCombustibleDetalleHelper() : base(Consultas.IndStkCombustibleDetalleSql)
        {
        }
        #endregion

        #region Crear datos
        public IndStkCombustibleDetalleDTO Create(IDataReader dr)
        {
            IndStkCombustibleDetalleDTO entity = new IndStkCombustibleDetalleDTO();
            SetCreate(dr, entity);

            return entity;
        }

        public IndStkCombustibleDetalleDTO CreateByPeriod(IDataReader dr)
        {
            IndStkCombustibleDetalleDTO entity = new IndStkCombustibleDetalleDTO();
            SetCreate(dr, entity);

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

        private void SetCreate(IDataReader dr, IndStkCombustibleDetalleDTO entity)
        {
            int iStkdetcodi = dr.GetOrdinal(this.Stkdetcodi);
            if (!dr.IsDBNull(iStkdetcodi)) entity.Stkdetcodi = Convert.ToInt32(dr.GetValue(iStkdetcodi));

            int iStkcmtcodi = dr.GetOrdinal(this.Stkcmtcodi);
            if (!dr.IsDBNull(iStkcmtcodi)) entity.Stkcmtcodi = Convert.ToInt32(dr.GetValue(iStkcmtcodi));

            int iStkdettipo = dr.GetOrdinal(this.Stkdettipo);
            if (!dr.IsDBNull(iStkdettipo)) entity.Stkdettipo = dr.GetString(iStkdettipo);

            int iD1 = dr.GetOrdinal(this.D1);
            if (!dr.IsDBNull(iD1)) entity.D1 = dr.GetString(iD1);

            int iD2 = dr.GetOrdinal(this.D2);
            if (!dr.IsDBNull(iD2)) entity.D2 = dr.GetString(iD2);

            int iD3 = dr.GetOrdinal(this.D3);
            if (!dr.IsDBNull(iD3)) entity.D3 = dr.GetString(iD3);

            int iD4 = dr.GetOrdinal(this.D4);
            if (!dr.IsDBNull(iD4)) entity.D4 = dr.GetString(iD4);

            int iD5 = dr.GetOrdinal(this.D5);
            if (!dr.IsDBNull(iD5)) entity.D5 = dr.GetString(iD5);

            int iD6 = dr.GetOrdinal(this.D6);
            if (!dr.IsDBNull(iD6)) entity.D6 = dr.GetString(iD6);

            int iD7 = dr.GetOrdinal(this.D7);
            if (!dr.IsDBNull(iD7)) entity.D7 = dr.GetString(iD7);

            int iD8 = dr.GetOrdinal(this.D8);
            if (!dr.IsDBNull(iD8)) entity.D8 = dr.GetString(iD8);

            int iD9 = dr.GetOrdinal(this.D9);
            if (!dr.IsDBNull(iD9)) entity.D9 = dr.GetString(iD9);

            int iD10 = dr.GetOrdinal(this.D10);
            if (!dr.IsDBNull(iD10)) entity.D10 = dr.GetString(iD10);

            int iD11 = dr.GetOrdinal(this.D11);
            if (!dr.IsDBNull(iD11)) entity.D11 = dr.GetString(iD11);

            int iD12 = dr.GetOrdinal(this.D12);
            if (!dr.IsDBNull(iD12)) entity.D12 = dr.GetString(iD12);

            int iD13 = dr.GetOrdinal(this.D13);
            if (!dr.IsDBNull(iD13)) entity.D13 = dr.GetString(iD13);

            int iD14 = dr.GetOrdinal(this.D14);
            if (!dr.IsDBNull(iD14)) entity.D14 = dr.GetString(iD14);

            int iD15 = dr.GetOrdinal(this.D15);
            if (!dr.IsDBNull(iD15)) entity.D15 = dr.GetString(iD15);

            int iD16 = dr.GetOrdinal(this.D16);
            if (!dr.IsDBNull(iD16)) entity.D16 = dr.GetString(iD16);

            int iD17 = dr.GetOrdinal(this.D17);
            if (!dr.IsDBNull(iD17)) entity.D17 = dr.GetString(iD17);

            int iD18 = dr.GetOrdinal(this.D18);
            if (!dr.IsDBNull(iD18)) entity.D18 = dr.GetString(iD18);

            int iD19 = dr.GetOrdinal(this.D19);
            if (!dr.IsDBNull(iD19)) entity.D19 = dr.GetString(iD19);

            int iD20 = dr.GetOrdinal(this.D20);
            if (!dr.IsDBNull(iD20)) entity.D20 = dr.GetString(iD20);

            int iD21 = dr.GetOrdinal(this.D21);
            if (!dr.IsDBNull(iD21)) entity.D21 = dr.GetString(iD21);

            int iD22 = dr.GetOrdinal(this.D22);
            if (!dr.IsDBNull(iD22)) entity.D22 = dr.GetString(iD22);

            int iD23 = dr.GetOrdinal(this.D23);
            if (!dr.IsDBNull(iD23)) entity.D23 = dr.GetString(iD23);

            int iD24 = dr.GetOrdinal(this.D24);
            if (!dr.IsDBNull(iD24)) entity.D24 = dr.GetString(iD24);

            int iD25 = dr.GetOrdinal(this.D25);
            if (!dr.IsDBNull(iD25)) entity.D25 = dr.GetString(iD25);

            int iD26 = dr.GetOrdinal(this.D26);
            if (!dr.IsDBNull(iD26)) entity.D26 = dr.GetString(iD26);

            int iD27 = dr.GetOrdinal(this.D27);
            if (!dr.IsDBNull(iD27)) entity.D27 = dr.GetString(iD27);

            int iD28 = dr.GetOrdinal(this.D28);
            if (!dr.IsDBNull(iD28)) entity.D28 = dr.GetString(iD28);

            int iD29 = dr.GetOrdinal(this.D29);
            if (!dr.IsDBNull(iD29)) entity.D29 = dr.GetString(iD29);

            int iD30 = dr.GetOrdinal(this.D30);
            if (!dr.IsDBNull(iD30)) entity.D30 = dr.GetString(iD30);

            int iD31 = dr.GetOrdinal(this.D31);
            if (!dr.IsDBNull(iD31)) entity.D31 = dr.GetString(iD31);

            int iStkdetusucreacion = dr.GetOrdinal(this.Stkdetusucreacion);
            if (!dr.IsDBNull(iStkdetusucreacion)) entity.Stkdetusucreacion = dr.GetString(iStkdetusucreacion);

            int iStkdetfeccreacion = dr.GetOrdinal(this.Stkdetfeccreacion);
            if (!dr.IsDBNull(iStkdetfeccreacion)) entity.Stkdetfeccreacion = dr.GetDateTime(iStkdetfeccreacion);

            int iStkdetusumodificacion = dr.GetOrdinal(this.Stkdetusumodificacion);
            if (!dr.IsDBNull(iStkdetusumodificacion)) entity.Stkdetusumodificacion = dr.GetString(iStkdetusumodificacion);

            int iStkdetfecmodificacion = dr.GetOrdinal(this.Stkdetfecmodificacion);
            if (!dr.IsDBNull(iStkdetfecmodificacion)) entity.Stkdetfecmodificacion = dr.GetDateTime(iStkdetfecmodificacion);

        }
        #endregion

        #region Consultas
        public string SqlUpdateDays
        {
            get { return base.GetSqlXml("UpdateDays"); }
        }

        public string SqlGetByPeriod
        {
            get { return base.GetSqlXml("GetByPeriod"); }
        }
        #endregion
    }
}
