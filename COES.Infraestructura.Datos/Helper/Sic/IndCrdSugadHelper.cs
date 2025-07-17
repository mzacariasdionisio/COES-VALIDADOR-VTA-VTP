using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class IndCrdSugadHelper : HelperBase
    {
        public IndCrdSugadHelper() : base(Consultas.IndCrdSugadSql)
        {
        }

        #region Mapeo de Campos
        public string Crdsgdcodi = "CRDSGDCODI";
        public string Indcbrcodi = "INDCBRCODI";
        public string Equicodicentral = "EQUICODICENTRAL";
        public string Equicodiunidad = "EQUICODIUNIDAD";
        public string Grupocodi = "GRUPOCODI";
        public string Famcodi = "FAMCODI";
        public string Crdsgdtipo = "CRDSGDTIPO";
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
        public string E1 = "E1";
        public string E2 = "E2";
        public string E3 = "E3";
        public string E4 = "E4";
        public string E5 = "E5";
        public string E6 = "E6";
        public string E7 = "E7";
        public string E8 = "E8";
        public string E9 = "E9";
        public string E10 = "E10";
        public string E11 = "E11";
        public string E12 = "E12";
        public string E13 = "E13";
        public string E14 = "E14";
        public string E15 = "E15";
        public string E16 = "E16";
        public string E17 = "E17";
        public string E18 = "E18";
        public string E19 = "E19";
        public string E20 = "E20";
        public string E21 = "E21";
        public string E22 = "E22";
        public string E23 = "E23";
        public string E24 = "E24";
        public string E25 = "E25";
        public string E26 = "E26";
        public string E27 = "E27";
        public string E28 = "E28";
        public string E29 = "E29";
        public string E30 = "E30";
        public string E31 = "E31";
        public string Crdsgdusucreacion = "CRDSGDUSUCREACION";
        public string Crdsgdfeccreacion = "CRDSGDFECCREACION";
        public string Crdsgdusumodificacion = "CRDSGDUSUMODIFICACION";
        public string Crdsgdfecmodificacion = "CRDSGDFECMODIFICACION";
        //Adicionales
        public string Emprcodi = "EMPRCODI";
        public string Ipericodi = "IPERICODI";
        public string Indcbrtipo = "INDCBRTIPO";
        public string Emprnomb = "EMPRNOMB";
        public string Equinombcentral = "EQUINOMBCENTRAL";
        public string Equinombunidad = "EQUINOMBUNIDAD";
        public string Indcbrusucreacion = "INDCBRUSUCREACION";
        public string Indcbrfeccreacion = "INDCBRFECCREACION";
        public string Indcbrfecha = "INDCBRFECHA";
        public string Cumplimiento = "CUMPLIMIENTO";
        public string Porcentaje = "PORCENTAJE";
        #endregion

        #region Consultas
        public string SqlListCrdSugadByCabecera
        {
            get { return base.GetSqlXml("ListCrdSugadByCabecera"); }
        }        
        public string SqlListCrdSugadJoinCabecera
        {
            get { return base.GetSqlXml("ListCrdSugadJoinCabecera"); }
        }   
        public string SqlUpdateIndCrdSugad
        {
            get { return base.GetSqlXml("UpdateIndCrdSugad"); }
        }

        public string SqlUpdateIndCrdEstado
        {
            get { return base.GetSqlXml("UpdateIndCrdEstado"); }
        }

        public string SqlListByCriteria
        {
            get { return base.GetSqlXml("ListByCriteria"); }
        }
        public string SqlListCumplimientoDiario
        {
            get { return base.GetSqlXml("ListCumplimientoDiario"); }
        }
        #endregion

        #region Crear datos
        private void SetCreateBase(IDataReader dr, IndCrdSugadDTO entity)
        {
            int iCrdsgdcodi = dr.GetOrdinal(this.Crdsgdcodi);
            if (!dr.IsDBNull(iCrdsgdcodi)) entity.Crdsgdcodi = Convert.ToInt32(dr.GetValue(iCrdsgdcodi));

            int iIndcbrcodi = dr.GetOrdinal(this.Indcbrcodi);
            if (!dr.IsDBNull(iIndcbrcodi)) entity.Indcbrcodi = Convert.ToInt32(dr.GetValue(iIndcbrcodi));

            int iEquicodicentral = dr.GetOrdinal(this.Equicodicentral);
            if (!dr.IsDBNull(iEquicodicentral)) entity.Equicodicentral = Convert.ToInt32(dr.GetValue(iEquicodicentral));

            int iEquicodiunidad = dr.GetOrdinal(this.Equicodiunidad);
            if (!dr.IsDBNull(iEquicodiunidad)) entity.Equicodiunidad = Convert.ToInt32(dr.GetValue(iEquicodiunidad));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iFamcodi = dr.GetOrdinal(this.Famcodi);
            if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

            int iCrdsgdtipo = dr.GetOrdinal(this.Crdsgdtipo);
            if (!dr.IsDBNull(iCrdsgdtipo)) entity.Crdsgdtipo = Convert.ToInt32(dr.GetValue(iCrdsgdtipo));

        }

        private void SetCreateDays(IDataReader dr, IndCrdSugadDTO entity)
        {
            int iD1 = dr.GetOrdinal(this.D1);
            if (!dr.IsDBNull(iD1)) entity.D1 = dr.GetDecimal(iD1);

            int iD2 = dr.GetOrdinal(this.D2);
            if (!dr.IsDBNull(iD2)) entity.D2 = dr.GetDecimal(iD2);

            int iD3 = dr.GetOrdinal(this.D3);
            if (!dr.IsDBNull(iD3)) entity.D3 = dr.GetDecimal(iD3);

            int iD4 = dr.GetOrdinal(this.D4);
            if (!dr.IsDBNull(iD4)) entity.D4 = dr.GetDecimal(iD4);

            int iD5 = dr.GetOrdinal(this.D5);
            if (!dr.IsDBNull(iD5)) entity.D5 = dr.GetDecimal(iD5);

            int iD6 = dr.GetOrdinal(this.D6);
            if (!dr.IsDBNull(iD6)) entity.D6 = dr.GetDecimal(iD6);

            int iD7 = dr.GetOrdinal(this.D7);
            if (!dr.IsDBNull(iD7)) entity.D7 = dr.GetDecimal(iD7);

            int iD8 = dr.GetOrdinal(this.D8);
            if (!dr.IsDBNull(iD8)) entity.D8 = dr.GetDecimal(iD8);

            int iD9 = dr.GetOrdinal(this.D9);
            if (!dr.IsDBNull(iD9)) entity.D9 = dr.GetDecimal(iD9);

            int iD10 = dr.GetOrdinal(this.D10);
            if (!dr.IsDBNull(iD10)) entity.D10 = dr.GetDecimal(iD10);

            int iD11 = dr.GetOrdinal(this.D11);
            if (!dr.IsDBNull(iD11)) entity.D11 = dr.GetDecimal(iD11);

            int iD12 = dr.GetOrdinal(this.D12);
            if (!dr.IsDBNull(iD12)) entity.D12 = dr.GetDecimal(iD12);

            int iD13 = dr.GetOrdinal(this.D13);
            if (!dr.IsDBNull(iD13)) entity.D13 = dr.GetDecimal(iD13);

            int iD14 = dr.GetOrdinal(this.D14);
            if (!dr.IsDBNull(iD14)) entity.D14 = dr.GetDecimal(iD14);

            int iD15 = dr.GetOrdinal(this.D15);
            if (!dr.IsDBNull(iD15)) entity.D15 = dr.GetDecimal(iD15);

            int iD16 = dr.GetOrdinal(this.D16);
            if (!dr.IsDBNull(iD16)) entity.D16 = dr.GetDecimal(iD16);

            int iD17 = dr.GetOrdinal(this.D17);
            if (!dr.IsDBNull(iD17)) entity.D17 = dr.GetDecimal(iD17);

            int iD18 = dr.GetOrdinal(this.D18);
            if (!dr.IsDBNull(iD18)) entity.D18 = dr.GetDecimal(iD18);

            int iD19 = dr.GetOrdinal(this.D19);
            if (!dr.IsDBNull(iD19)) entity.D19 = dr.GetDecimal(iD19);

            int iD20 = dr.GetOrdinal(this.D20);
            if (!dr.IsDBNull(iD20)) entity.D20 = dr.GetDecimal(iD20);

            int iD21 = dr.GetOrdinal(this.D21);
            if (!dr.IsDBNull(iD21)) entity.D21 = dr.GetDecimal(iD21);

            int iD22 = dr.GetOrdinal(this.D22);
            if (!dr.IsDBNull(iD22)) entity.D22 = dr.GetDecimal(iD22);

            int iD23 = dr.GetOrdinal(this.D23);
            if (!dr.IsDBNull(iD23)) entity.D23 = dr.GetDecimal(iD23);

            int iD24 = dr.GetOrdinal(this.D24);
            if (!dr.IsDBNull(iD24)) entity.D24 = dr.GetDecimal(iD24);

            int iD25 = dr.GetOrdinal(this.D25);
            if (!dr.IsDBNull(iD25)) entity.D25 = dr.GetDecimal(iD25);

            int iD26 = dr.GetOrdinal(this.D26);
            if (!dr.IsDBNull(iD26)) entity.D26 = dr.GetDecimal(iD26);

            int iD27 = dr.GetOrdinal(this.D27);
            if (!dr.IsDBNull(iD27)) entity.D27 = dr.GetDecimal(iD27);

            int iD28 = dr.GetOrdinal(this.D28);
            if (!dr.IsDBNull(iD28)) entity.D28 = dr.GetDecimal(iD28);

            int iD29 = dr.GetOrdinal(this.D29);
            if (!dr.IsDBNull(iD29)) entity.D29 = dr.GetDecimal(iD29);

            int iD30 = dr.GetOrdinal(this.D30);
            if (!dr.IsDBNull(iD30)) entity.D30 = dr.GetDecimal(iD30);

            int iD31 = dr.GetOrdinal(this.D31);
            if (!dr.IsDBNull(iD31)) entity.D31 = dr.GetDecimal(iD31);

        }

        public IndCrdSugadDTO CreateListByCriteria(IDataReader dr)
        {
            IndCrdSugadDTO entity = new IndCrdSugadDTO();
            SetCreateBase(dr, entity);
            SetCreateDays(dr, entity);

            return entity;
        }

        #endregion
    }
}
