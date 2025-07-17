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
    /// Clase que contiene el mapeo de la tabla IND_INSUMOS_FACTORK_DETALLE
    /// </summary>
    public class IndInsumosFactorKDetalleHelper : HelperBase
    {
        #region Mapeo de Campos
        //table
        public string Infkdtcodi = "INFKDTCODI";
        public string Insfckcodi = "INSFCKCODI";
        public string Infkdttipo = "INFKDTTIPO";
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
        public string Infkdtusucreacion = "INFKDTUSUCREACION";
        public string Infkdtfeccreacion = "INFKDTFECCREACION";
        public string Infkdtusumodificacion = "INFKDTUSUMODIFICACION";
        public string Infkdtfecmodificacion = "INFKDTFECMODIFICACION";

        //aditional
        public string Ipericodi = "IPERICODI";
        public string Emprcodi = "EMPRCODI";
        public string Equicodicentral = "EQUICODICENTRAL";
        public string Equicodiunidad = "EQUICODIUNIDAD";
        public string Grupocodi = "GRUPOCODI";
        public string Famcodi = "FAMCODI";
        #endregion

        #region Constructor
        public IndInsumosFactorKDetalleHelper() : base(Consultas.IndInsumosFactorKDetalleSql)
        {
        }
        #endregion

        #region Crear Datos
        public IndInsumosFactorKDetalleDTO Create(IDataReader dr)
        {
            IndInsumosFactorKDetalleDTO entity = new IndInsumosFactorKDetalleDTO();
            SetCreate(dr, entity);
            return entity;
        }

        private void SetCreate(IDataReader dr, IndInsumosFactorKDetalleDTO entity) 
        {
            int iInfkdtcodi = dr.GetOrdinal(this.Infkdtcodi);
            if (!dr.IsDBNull(iInfkdtcodi)) entity.Infkdtcodi = Convert.ToInt32(dr.GetValue(iInfkdtcodi));

            int iInsfckcodi = dr.GetOrdinal(this.Insfckcodi);
            if (!dr.IsDBNull(iInsfckcodi)) entity.Insfckcodi = Convert.ToInt32(dr.GetValue(iInsfckcodi));

            int iInfkdttipo = dr.GetOrdinal(this.Infkdttipo);
            if (!dr.IsDBNull(iInfkdttipo)) entity.Infkdttipo = Convert.ToInt32(dr.GetValue(iInfkdttipo));

            int iD1 = dr.GetOrdinal(this.D1);
            if (!dr.IsDBNull(iD1)) entity.D1 = Convert.ToDecimal(dr.GetValue(iD1));

            int iD2 = dr.GetOrdinal(this.D2);
            if (!dr.IsDBNull(iD2)) entity.D2 = Convert.ToDecimal(dr.GetValue(iD2));

            int iD3 = dr.GetOrdinal(this.D3);
            if (!dr.IsDBNull(iD3)) entity.D3 = Convert.ToDecimal(dr.GetValue(iD3));

            int iD4 = dr.GetOrdinal(this.D4);
            if (!dr.IsDBNull(iD4)) entity.D4 = Convert.ToDecimal(dr.GetValue(iD4));

            int iD5 = dr.GetOrdinal(this.D5);
            if (!dr.IsDBNull(iD5)) entity.D5 = Convert.ToDecimal(dr.GetValue(iD5));

            int iD6 = dr.GetOrdinal(this.D6);
            if (!dr.IsDBNull(iD6)) entity.D6 = Convert.ToDecimal(dr.GetValue(iD6));

            int iD7 = dr.GetOrdinal(this.D7);
            if (!dr.IsDBNull(iD7)) entity.D7 = Convert.ToDecimal(dr.GetValue(iD7));

            int iD8 = dr.GetOrdinal(this.D8);
            if (!dr.IsDBNull(iD8)) entity.D8 = Convert.ToDecimal(dr.GetValue(iD8));

            int iD9 = dr.GetOrdinal(this.D9);
            if (!dr.IsDBNull(iD9)) entity.D9 = Convert.ToDecimal(dr.GetValue(iD9));

            int iD10 = dr.GetOrdinal(this.D10);
            if (!dr.IsDBNull(iD10)) entity.D10 = Convert.ToDecimal(dr.GetValue(iD10));

            int iD11 = dr.GetOrdinal(this.D11);
            if (!dr.IsDBNull(iD11)) entity.D11 = Convert.ToDecimal(dr.GetValue(iD11));

            int iD12 = dr.GetOrdinal(this.D12);
            if (!dr.IsDBNull(iD12)) entity.D12 = Convert.ToDecimal(dr.GetValue(iD12));

            int iD13 = dr.GetOrdinal(this.D13);
            if (!dr.IsDBNull(iD13)) entity.D13 = Convert.ToDecimal(dr.GetValue(iD13));

            int iD14 = dr.GetOrdinal(this.D14);
            if (!dr.IsDBNull(iD14)) entity.D14 = Convert.ToDecimal(dr.GetValue(iD14));

            int iD15 = dr.GetOrdinal(this.D15);
            if (!dr.IsDBNull(iD15)) entity.D15 = Convert.ToDecimal(dr.GetValue(iD15));

            int iD16 = dr.GetOrdinal(this.D16);
            if (!dr.IsDBNull(iD16)) entity.D16 = Convert.ToDecimal(dr.GetValue(iD16));

            int iD17 = dr.GetOrdinal(this.D17);
            if (!dr.IsDBNull(iD17)) entity.D17 = Convert.ToDecimal(dr.GetValue(iD17));

            int iD18 = dr.GetOrdinal(this.D18);
            if (!dr.IsDBNull(iD18)) entity.D18 = Convert.ToDecimal(dr.GetValue(iD18));

            int iD19 = dr.GetOrdinal(this.D19);
            if (!dr.IsDBNull(iD19)) entity.D19 = Convert.ToDecimal(dr.GetValue(iD19));

            int iD20 = dr.GetOrdinal(this.D20);
            if (!dr.IsDBNull(iD20)) entity.D20 = Convert.ToDecimal(dr.GetValue(iD20));

            int iD21 = dr.GetOrdinal(this.D21);
            if (!dr.IsDBNull(iD21)) entity.D21 = Convert.ToDecimal(dr.GetValue(iD21));

            int iD22 = dr.GetOrdinal(this.D22);
            if (!dr.IsDBNull(iD22)) entity.D22 = Convert.ToDecimal(dr.GetValue(iD22));

            int iD23 = dr.GetOrdinal(this.D23);
            if (!dr.IsDBNull(iD23)) entity.D23 = Convert.ToDecimal(dr.GetValue(iD23));

            int iD24 = dr.GetOrdinal(this.D24);
            if (!dr.IsDBNull(iD24)) entity.D24 = Convert.ToDecimal(dr.GetValue(iD24));

            int iD25 = dr.GetOrdinal(this.D25);
            if (!dr.IsDBNull(iD25)) entity.D25 = Convert.ToDecimal(dr.GetValue(iD25));

            int iD26 = dr.GetOrdinal(this.D26);
            if (!dr.IsDBNull(iD26)) entity.D26 = Convert.ToDecimal(dr.GetValue(iD26));

            int iD27 = dr.GetOrdinal(this.D27);
            if (!dr.IsDBNull(iD27)) entity.D27 = Convert.ToDecimal(dr.GetValue(iD27));

            int iD28 = dr.GetOrdinal(this.D28);
            if (!dr.IsDBNull(iD28)) entity.D28 = Convert.ToDecimal(dr.GetValue(iD28));

            int iD29 = dr.GetOrdinal(this.D29);
            if (!dr.IsDBNull(iD29)) entity.D29 = Convert.ToDecimal(dr.GetValue(iD29));

            int iD30 = dr.GetOrdinal(this.D30);
            if (!dr.IsDBNull(iD30)) entity.D30 = Convert.ToDecimal(dr.GetValue(iD30));

            int iD31 = dr.GetOrdinal(this.D31);
            if (!dr.IsDBNull(iD31)) entity.D31 = Convert.ToDecimal(dr.GetValue(iD31));

            int iInfkdtusucreacion = dr.GetOrdinal(this.Infkdtusucreacion);
            if (!dr.IsDBNull(iInfkdtusucreacion)) entity.Infkdtusucreacion = dr.GetString(iInfkdtusucreacion);

            int iInfkdtfeccreacion = dr.GetOrdinal(this.Infkdtfeccreacion);
            if (!dr.IsDBNull(iInfkdtfeccreacion)) entity.Infkdtfeccreacion = dr.GetDateTime(iInfkdtfeccreacion);

            int iInfkdtusumodificacion = dr.GetOrdinal(this.Infkdtusumodificacion);
            if (!dr.IsDBNull(iInfkdtusumodificacion)) entity.Infkdtusumodificacion = dr.GetString(iInfkdtusumodificacion);

            int iInfkdtfecmodificacion = dr.GetOrdinal(this.Infkdtfecmodificacion);
            if (!dr.IsDBNull(iInfkdtfecmodificacion)) entity.Infkdtfecmodificacion = dr.GetDateTime(iInfkdtfecmodificacion);
        }
        #endregion

        #region Consultas
        public string SqlUpdateDays
        {
            get { return base.GetSqlXml("UpdateDays"); }
        }

        public string SqlDeleteByCriteria
        {
            get { return base.GetSqlXml("DeleteByCriteria"); }
        }

        public string SqlGetByInsumosFactorK
        {
            get { return base.GetSqlXml("GetByInsumosFactorK"); }
        }

        public string SqlGetByPeriodo
        {
            get { return base.GetSqlXml("GetByPeriodo"); }
        }
        #endregion
    }
}
