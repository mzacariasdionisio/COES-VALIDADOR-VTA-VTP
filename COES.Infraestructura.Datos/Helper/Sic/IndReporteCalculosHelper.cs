using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IND_REPORTE_CALCULOS
    /// </summary>
    public class IndReporteCalculosHelper : HelperBase
    {
        #region Mapeo de Campos
        public string Irpcalcodi = "IRPCALCODI";
        public string Itotcodi = "ITOTCODI";
        public string Irpcaltipo = "IRPCALTIPO";
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
        public string Irpcalusucreacion = "IRPCALUSUCREACION";
        public string Irpcalfeccreacion = "IRPCALFECCREACION";
        #endregion

        #region Constructor
        public IndReporteCalculosHelper() : base(Consultas.IndReporteCalculosSql)
        {
        }
        #endregion

        #region Create
        public IndReporteCalculosDTO Create(IDataReader dr)
        {
            IndReporteCalculosDTO entity = new IndReporteCalculosDTO();

            int iIrpcalcodi = dr.GetOrdinal(this.Irpcalcodi);
            if (!dr.IsDBNull(iIrpcalcodi)) entity.Irpcalcodi = Convert.ToInt32(dr.GetValue(iIrpcalcodi));

            int iItotcodi = dr.GetOrdinal(this.Itotcodi);
            if (!dr.IsDBNull(iItotcodi)) entity.Itotcodi = Convert.ToInt32(dr.GetValue(iItotcodi));

            int iIrpcaltipo = dr.GetOrdinal(this.Irpcaltipo);
            if (!dr.IsDBNull(iIrpcaltipo)) entity.Irpcaltipo = Convert.ToInt32(dr.GetValue(iIrpcaltipo));

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

            int iIrpcalusucreacion = dr.GetOrdinal(this.Irpcalusucreacion);
            if (!dr.IsDBNull(iIrpcalusucreacion)) entity.Irpcalusucreacion = dr.GetString(iIrpcalusucreacion);

            int iIrpcalfeccreacion = dr.GetOrdinal(this.Irpcalfeccreacion);
            if (!dr.IsDBNull(iIrpcalfeccreacion)) entity.Irpcalfeccreacion = dr.GetDateTime(iIrpcalfeccreacion);

            return entity;
        }
        #endregion

        #region Consultas

        #endregion
    }
}
