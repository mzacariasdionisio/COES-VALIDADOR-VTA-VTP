using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class PrnExogenamedicionHelper : HelperBase
    {
        public PrnExogenamedicionHelper()
            : base(Consultas.PrnExogenamedicionSql)
        {
        }

        public PrnExogenamedicionDTO Create(IDataReader dr)
        {
            PrnExogenamedicionDTO entity = new PrnExogenamedicionDTO();

            int iExmedicodi = dr.GetOrdinal(this.Exmedicodi);
            if (!dr.IsDBNull(iExmedicodi)) entity.Exmedicodi = Convert.ToInt32(dr.GetValue(iExmedicodi));

            int iVarexocodi = dr.GetOrdinal(this.Varexocodi);
            if (!dr.IsDBNull(iVarexocodi)) entity.Varexocodi = Convert.ToInt32(dr.GetValue(iVarexocodi));

            int iAremedcodi = dr.GetOrdinal(this.Aremedcodi);
            if (!dr.IsDBNull(iAremedcodi)) entity.Aremedcodi = Convert.ToInt32(dr.GetValue(iAremedcodi));

            int iExmedifecha = dr.GetOrdinal(this.Exmedifecha);
            if (!dr.IsDBNull(iExmedifecha)) entity.Exmedifecha = dr.GetDateTime(iExmedifecha);

            int iTipoinfocodi = dr.GetOrdinal(this.Tipoinfocodi);
            if (!dr.IsDBNull(iTipoinfocodi)) entity.Tipoinfocodi = Convert.ToInt32(dr.GetValue(iTipoinfocodi));

            int iH1 = dr.GetOrdinal(this.H1);
            if (!dr.IsDBNull(iH1)) entity.H1 = dr.GetDecimal(iH1);

            int iExmeditotal = dr.GetOrdinal(this.Exmeditotal);
            if (!dr.IsDBNull(iExmeditotal)) entity.Exmeditotal = dr.GetDecimal(iExmeditotal);

            int iH2 = dr.GetOrdinal(this.H2);
            if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

            int iH3 = dr.GetOrdinal(this.H3);
            if (!dr.IsDBNull(iH1)) entity.H3 = dr.GetDecimal(iH3);

            int iH4 = dr.GetOrdinal(this.H4);
            if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

            int iH5 = dr.GetOrdinal(this.H5);
            if (!dr.IsDBNull(iH5)) entity.H5 = dr.GetDecimal(iH5);

            int iH6 = dr.GetOrdinal(this.H6);
            if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

            int iH7 = dr.GetOrdinal(this.H7);
            if (!dr.IsDBNull(iH7)) entity.H7 = dr.GetDecimal(iH7);

            int iH8 = dr.GetOrdinal(this.H8);
            if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);

            int iH9 = dr.GetOrdinal(this.H9);
            if (!dr.IsDBNull(iH9)) entity.H9 = dr.GetDecimal(iH9);

            int iH10 = dr.GetOrdinal(this.H10);
            if (!dr.IsDBNull(iH1)) entity.H10 = dr.GetDecimal(iH10);

            int iH11 = dr.GetOrdinal(this.H11);
            if (!dr.IsDBNull(iH11)) entity.H11 = dr.GetDecimal(iH11);

            int iH12 = dr.GetOrdinal(this.H12);
            if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

            int iH13 = dr.GetOrdinal(this.H13);
            if (!dr.IsDBNull(iH13)) entity.H13 = dr.GetDecimal(iH13);

            int iH14 = dr.GetOrdinal(this.H14);
            if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

            int iH15 = dr.GetOrdinal(this.H15);
            if (!dr.IsDBNull(iH15)) entity.H15 = dr.GetDecimal(iH15);

            int iH16 = dr.GetOrdinal(this.H16);
            if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

            int iH17 = dr.GetOrdinal(this.H17);
            if (!dr.IsDBNull(iH17)) entity.H17 = dr.GetDecimal(iH17);

            int iH18 = dr.GetOrdinal(this.H18);
            if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

            int iH19 = dr.GetOrdinal(this.H19);
            if (!dr.IsDBNull(iH19)) entity.H19 = dr.GetDecimal(iH19);

            int iH20 = dr.GetOrdinal(this.H20);
            if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);

            int iH21 = dr.GetOrdinal(this.H21);
            if (!dr.IsDBNull(iH21)) entity.H21 = dr.GetDecimal(iH21);

            int iH22 = dr.GetOrdinal(this.H22);
            if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

            int iH23 = dr.GetOrdinal(this.H23);
            if (!dr.IsDBNull(iH23)) entity.H23 = dr.GetDecimal(iH23);

            int iH24 = dr.GetOrdinal(this.H24);
            if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

            int iExmedifeccreacion = dr.GetOrdinal(this.Exmedifeccreacion);
            if (!dr.IsDBNull(iExmedifeccreacion)) entity.Exmedifeccreacion = dr.GetDateTime(iExmedifeccreacion);

            return entity;
        }

        public PrnExogenamedicionDTO ListExomedicionByCiudadDate(IDataReader dr)
        {
            PrnExogenamedicionDTO entity = new PrnExogenamedicionDTO();

            int iExmedicodi = dr.GetOrdinal(this.Exmedicodi);
            if (!dr.IsDBNull(iExmedicodi)) entity.Exmedicodi = Convert.ToInt32(dr.GetValue(iExmedicodi));

            int iVarexocodi = dr.GetOrdinal(this.Varexocodi);
            if (!dr.IsDBNull(iVarexocodi)) entity.Varexocodi = Convert.ToInt32(dr.GetValue(iVarexocodi));

            int iAremedcodi = dr.GetOrdinal(this.Aremedcodi);
            if (!dr.IsDBNull(iAremedcodi)) entity.Aremedcodi = Convert.ToInt32(dr.GetValue(iAremedcodi));

            int iExmedifecha = dr.GetOrdinal(this.Exmedifecha);
            if (!dr.IsDBNull(iExmedifecha)) entity.Exmedifecha = dr.GetDateTime(iExmedifecha);

            int iTipoinfocodi = dr.GetOrdinal(this.Tipoinfocodi);
            if (!dr.IsDBNull(iTipoinfocodi)) entity.Tipoinfocodi = Convert.ToInt32(dr.GetValue(iTipoinfocodi));

            int iH1 = dr.GetOrdinal(this.H1);
            if (!dr.IsDBNull(iH1)) entity.H1 = dr.GetDecimal(iH1);

            int iExmeditotal = dr.GetOrdinal(this.Exmeditotal);
            if (!dr.IsDBNull(iExmeditotal)) entity.Exmeditotal = dr.GetDecimal(iExmeditotal);

            int iH2 = dr.GetOrdinal(this.H2);
            if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

            int iH3 = dr.GetOrdinal(this.H3);
            if (!dr.IsDBNull(iH1)) entity.H3 = dr.GetDecimal(iH3);

            int iH4 = dr.GetOrdinal(this.H4);
            if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

            int iH5 = dr.GetOrdinal(this.H5);
            if (!dr.IsDBNull(iH5)) entity.H5 = dr.GetDecimal(iH5);

            int iH6 = dr.GetOrdinal(this.H6);
            if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

            int iH7 = dr.GetOrdinal(this.H7);
            if (!dr.IsDBNull(iH7)) entity.H7 = dr.GetDecimal(iH7);

            int iH8 = dr.GetOrdinal(this.H8);
            if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);

            int iH9 = dr.GetOrdinal(this.H9);
            if (!dr.IsDBNull(iH9)) entity.H9 = dr.GetDecimal(iH9);

            int iH10 = dr.GetOrdinal(this.H10);
            if (!dr.IsDBNull(iH1)) entity.H10 = dr.GetDecimal(iH10);

            int iH11 = dr.GetOrdinal(this.H11);
            if (!dr.IsDBNull(iH11)) entity.H11 = dr.GetDecimal(iH11);

            int iH12 = dr.GetOrdinal(this.H12);
            if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

            int iH13 = dr.GetOrdinal(this.H13);
            if (!dr.IsDBNull(iH13)) entity.H13 = dr.GetDecimal(iH13);

            int iH14 = dr.GetOrdinal(this.H14);
            if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

            int iH15 = dr.GetOrdinal(this.H15);
            if (!dr.IsDBNull(iH15)) entity.H15 = dr.GetDecimal(iH15);

            int iH16 = dr.GetOrdinal(this.H16);
            if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

            int iH17 = dr.GetOrdinal(this.H17);
            if (!dr.IsDBNull(iH17)) entity.H17 = dr.GetDecimal(iH17);

            int iH18 = dr.GetOrdinal(this.H18);
            if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

            int iH19 = dr.GetOrdinal(this.H19);
            if (!dr.IsDBNull(iH19)) entity.H19 = dr.GetDecimal(iH19);

            int iH20 = dr.GetOrdinal(this.H20);
            if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);

            int iH21 = dr.GetOrdinal(this.H21);
            if (!dr.IsDBNull(iH21)) entity.H21 = dr.GetDecimal(iH21);

            int iH22 = dr.GetOrdinal(this.H22);
            if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

            int iH23 = dr.GetOrdinal(this.H23);
            if (!dr.IsDBNull(iH23)) entity.H23 = dr.GetDecimal(iH23);

            int iH24 = dr.GetOrdinal(this.H24);
            if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

            int iExmedifeccreacion = dr.GetOrdinal(this.Exmedifeccreacion);
            if (!dr.IsDBNull(iExmedifeccreacion)) entity.Exmedifeccreacion = dr.GetDateTime(iExmedifeccreacion);

            int iTipoinfoabrev = dr.GetOrdinal(this.Tipoinfoabrev);
            if (!dr.IsDBNull(iTipoinfoabrev)) entity.Tipoinfoabrev = dr.GetString(iTipoinfoabrev);

            int iAreanomb = dr.GetOrdinal(this.Areanomb);
            if (!dr.IsDBNull(iAreanomb)) entity.AreaNomb = dr.GetString(iAreanomb);

            return entity;
        }

        public PrnHorasolDTO CreateHorasol(IDataReader dr)
        {
            PrnHorasolDTO entity = new PrnHorasolDTO();

            int iAreacodi = dr.GetOrdinal(this.Areacodi);
            if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

            int iPrnhsfecha = dr.GetOrdinal(this.Prnhsfecha);
            if (!dr.IsDBNull(iPrnhsfecha)) entity.Prnhsfecha = dr.GetDateTime(iPrnhsfecha);

            int iPrnhssalida = dr.GetOrdinal(this.Prnhssalida);
            if (!dr.IsDBNull(iPrnhssalida)) entity.Prnhssalida = dr.GetDateTime(iPrnhssalida);

            int iPrnhspuesta = dr.GetOrdinal(this.Prnhspuesta);
            if (!dr.IsDBNull(iPrnhspuesta)) entity.Prnhspuesta = dr.GetDateTime(iPrnhspuesta);

            int iPrnhshorassol = dr.GetOrdinal(this.Prnhshorassol);
            if (!dr.IsDBNull(iPrnhshorassol)) entity.Prnhshorassol = dr.GetDateTime(iPrnhshorassol);

            int iPrnhsusucreacion = dr.GetOrdinal(this.Prnhsusucreacion);
            if (!dr.IsDBNull(iPrnhsusucreacion)) entity.Prnhsusucreacion = dr.GetString(iPrnhsusucreacion);

            int iPrnhsfeccreacion = dr.GetOrdinal(this.Prnhsfeccreacion);
            if (!dr.IsDBNull(iPrnhsfeccreacion)) entity.Prnhsfeccreacion = dr.GetDateTime(iPrnhsfeccreacion);
            
            return entity;
        }

        #region Mapeo de los campos

        public string Exmedicodi = "EXMEDICODI";
        public string Varexocodi = "VAREXOCODI";
        public string Aremedcodi = "AREMEDCODI";
        public string Exmedifecha = "EXMEDIFECHA";
        public string Tipoinfocodi = "TIPOINFOCODI";
        public string H1 = "H1";
        public string Exmeditotal = "EXMEDITOTAL";
        public string H2 = "H2";
        public string H3 = "H3";
        public string H4 = "H4";
        public string H5 = "H5";
        public string H6 = "H6";
        public string H7 = "H7";
        public string H8 = "H8";
        public string H9 = "H9";
        public string H10 = "H10";
        public string H11 = "H11";
        public string H12 = "H12";
        public string H13 = "H13";
        public string H14 = "H14";
        public string H15 = "H15";
        public string H16 = "H16";
        public string H17 = "H17";
        public string H18 = "H18";
        public string H19 = "H19";
        public string H20 = "H20";
        public string H21 = "H21";
        public string H22 = "H22";
        public string H23 = "H23";
        public string H24 = "H24";
        public string Exmedifeccreacion = "EXMEDIFECCREACION";

        public string Tipoinfoabrev = "TIPOINFOABREV";
        public string Areanomb = "AREANOMB";

        //CAMPOS DE LA TABLA PRN_HORASOL
        public string Areacodi = "AREACODI";
        public string Prnhsfecha = "PRNHSFECHA";
        public string Prnhssalida = "PRNHSSALIDA";
        public string Prnhspuesta = "PRNHSPUESTA";
        public string Prnhshorassol = "PRNHSHORASSOL";
        public string Prnhsusucreacion = "PRNHSUSUCREACION";
        public string Prnhsfeccreacion = "PRNHSFECCREACION";

        #endregion

        #region Consultas

        public string SqlListExomedicionByCiudadDate
        {
            get { return base.GetSqlXml("ListExomedicionByCiudadDate"); }
        }

        public string SqlListHorasol
        {
            get { return base.GetSqlXml("ListHorasol"); }
        }

        #endregion
    }
}
