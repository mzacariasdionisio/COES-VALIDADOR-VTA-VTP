using COES.Base.Core;
using COES.Dominio.DTO.ReportesFrecuencia;
using System;
using System.Data;
using COES.Dominio.DTO.Sic;

namespace COES.Infraestructura.Datos.Helper.ReportesFrecuencia
{
    public class InformacionFrecuenciaHelper : HelperBase
    {
        public InformacionFrecuenciaHelper() : base(Consultas.InformacionFrecuenciaSql)
        {
        }
        private bool columnsExist(string columnName, IDataReader dr)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {

                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }
        private object valorReturn(IDataReader dr, string sColumna)
        {
            object resultado = null;
            int iIndex;
            if (columnsExist(sColumna, dr))
            {
                iIndex = dr.GetOrdinal(sColumna);
                if (!dr.IsDBNull(iIndex))
                    resultado = dr.GetValue(iIndex);
            }
            return resultado?.ToString();
        }
        public static T? ConvertToNull<T>(object x) where T : struct
        {
            return x == null ? null : (T?)Convert.ChangeType(x, typeof(T));
        }

        public LecturaDTO Create(IDataReader dr)
        {
            LecturaDTO entity = new LecturaDTO();

            entity.FechaHoraString = valorReturn(dr, FechaHora)?.ToString();
            entity.FechaHora = Convert.ToDateTime(valorReturn(dr, FechaHora) ?? null);
            entity.GPSCodi = Convert.ToInt32(valorReturn(dr, GPSCodi) ?? 0);
            entity.GPSNombre = valorReturn(dr, GPSNombre)?.ToString();
            entity.H0 = Convert.ToDecimal(valorReturn(dr, H0) ?? 0);
            entity.H1 = Convert.ToDecimal(valorReturn(dr, H1) ?? 0);
            entity.H2 = Convert.ToDecimal(valorReturn(dr, H2) ?? 0);
            entity.H3 = Convert.ToDecimal(valorReturn(dr, H3) ?? 0);
            entity.H4 = Convert.ToDecimal(valorReturn(dr, H4) ?? 0);
            entity.H5 = Convert.ToDecimal(valorReturn(dr, H5) ?? 0);
            entity.H6 = Convert.ToDecimal(valorReturn(dr, H6) ?? 0);
            entity.H7 = Convert.ToDecimal(valorReturn(dr, H7) ?? 0);
            entity.H8 = Convert.ToDecimal(valorReturn(dr, H8) ?? 0);
            entity.H9 = Convert.ToDecimal(valorReturn(dr, H9) ?? 0);
            entity.H10 = Convert.ToDecimal(valorReturn(dr, H10) ?? 0);
            entity.H11 = Convert.ToDecimal(valorReturn(dr, H11) ?? 0);
            entity.H12 = Convert.ToDecimal(valorReturn(dr, H12) ?? 0);
            entity.H13 = Convert.ToDecimal(valorReturn(dr, H13) ?? 0);
            entity.H14 = Convert.ToDecimal(valorReturn(dr, H14) ?? 0);
            entity.H15 = Convert.ToDecimal(valorReturn(dr, H15) ?? 0);
            entity.H16 = Convert.ToDecimal(valorReturn(dr, H16) ?? 0);
            entity.H17 = Convert.ToDecimal(valorReturn(dr, H17) ?? 0);
            entity.H18 = Convert.ToDecimal(valorReturn(dr, H18) ?? 0);
            entity.H19 = Convert.ToDecimal(valorReturn(dr, H19) ?? 0);
            entity.H20 = Convert.ToDecimal(valorReturn(dr, H20) ?? 0);
            entity.H21 = Convert.ToDecimal(valorReturn(dr, H21) ?? 0);
            entity.H22 = Convert.ToDecimal(valorReturn(dr, H22) ?? 0);
            entity.H23 = Convert.ToDecimal(valorReturn(dr, H23) ?? 0);
            entity.H24 = Convert.ToDecimal(valorReturn(dr, H24) ?? 0);
            entity.H25 = Convert.ToDecimal(valorReturn(dr, H25) ?? 0);
            entity.H26 = Convert.ToDecimal(valorReturn(dr, H26) ?? 0);
            entity.H27 = Convert.ToDecimal(valorReturn(dr, H27) ?? 0);
            entity.H28 = Convert.ToDecimal(valorReturn(dr, H28) ?? 0);
            entity.H29 = Convert.ToDecimal(valorReturn(dr, H29) ?? 0);
            entity.H30 = Convert.ToDecimal(valorReturn(dr, H30) ?? 0);
            entity.H31 = Convert.ToDecimal(valorReturn(dr, H31) ?? 0);
            entity.H32 = Convert.ToDecimal(valorReturn(dr, H32) ?? 0);
            entity.H33 = Convert.ToDecimal(valorReturn(dr, H33) ?? 0);
            entity.H34 = Convert.ToDecimal(valorReturn(dr, H34) ?? 0);
            entity.H35 = Convert.ToDecimal(valorReturn(dr, H35) ?? 0);
            entity.H36 = Convert.ToDecimal(valorReturn(dr, H36) ?? 0);
            entity.H37 = Convert.ToDecimal(valorReturn(dr, H37) ?? 0);
            entity.H38 = Convert.ToDecimal(valorReturn(dr, H38) ?? 0);
            entity.H39 = Convert.ToDecimal(valorReturn(dr, H39) ?? 0);
            entity.H40 = Convert.ToDecimal(valorReturn(dr, H40) ?? 0);
            entity.H41 = Convert.ToDecimal(valorReturn(dr, H41) ?? 0);
            entity.H42 = Convert.ToDecimal(valorReturn(dr, H42) ?? 0);
            entity.H43 = Convert.ToDecimal(valorReturn(dr, H43) ?? 0);
            entity.H44 = Convert.ToDecimal(valorReturn(dr, H44) ?? 0);
            entity.H45 = Convert.ToDecimal(valorReturn(dr, H45) ?? 0);
            entity.H46 = Convert.ToDecimal(valorReturn(dr, H46) ?? 0);
            entity.H47 = Convert.ToDecimal(valorReturn(dr, H47) ?? 0);
            entity.H48 = Convert.ToDecimal(valorReturn(dr, H48) ?? 0);
            entity.H49 = Convert.ToDecimal(valorReturn(dr, H49) ?? 0);
            entity.H50 = Convert.ToDecimal(valorReturn(dr, H50) ?? 0);
            entity.H51 = Convert.ToDecimal(valorReturn(dr, H51) ?? 0);
            entity.H52 = Convert.ToDecimal(valorReturn(dr, H52) ?? 0);
            entity.H53 = Convert.ToDecimal(valorReturn(dr, H53) ?? 0);
            entity.H54 = Convert.ToDecimal(valorReturn(dr, H54) ?? 0);
            entity.H55 = Convert.ToDecimal(valorReturn(dr, H55) ?? 0);
            entity.H56 = Convert.ToDecimal(valorReturn(dr, H56) ?? 0);
            entity.H57 = Convert.ToDecimal(valorReturn(dr, H57) ?? 0);
            entity.H58 = Convert.ToDecimal(valorReturn(dr, H58) ?? 0);
            entity.H59 = Convert.ToDecimal(valorReturn(dr, H59) ?? 0);
            return entity;
        }

        public LecturaDTO CreateLectura(IDataReader dr)
        {
            LecturaDTO entity = new LecturaDTO();

            entity.FechaHoraString = valorReturn(dr, FechaHora)?.ToString();
            entity.FechaHora = Convert.ToDateTime(valorReturn(dr, FechaHora) ?? null);
            entity.GPSCodi = Convert.ToInt32(valorReturn(dr, GPSCodi) ?? 0);
            entity.H0 = Convert.ToDecimal(valorReturn(dr, H0) ?? 0);
            entity.H1 = Convert.ToDecimal(valorReturn(dr, H1) ?? 0);
            entity.H2 = Convert.ToDecimal(valorReturn(dr, H2) ?? 0);
            entity.H3 = Convert.ToDecimal(valorReturn(dr, H3) ?? 0);
            entity.H4 = Convert.ToDecimal(valorReturn(dr, H4) ?? 0);
            entity.H5 = Convert.ToDecimal(valorReturn(dr, H5) ?? 0);
            entity.H6 = Convert.ToDecimal(valorReturn(dr, H6) ?? 0);
            entity.H7 = Convert.ToDecimal(valorReturn(dr, H7) ?? 0);
            entity.H8 = Convert.ToDecimal(valorReturn(dr, H8) ?? 0);
            entity.H9 = Convert.ToDecimal(valorReturn(dr, H9) ?? 0);
            entity.H10 = Convert.ToDecimal(valorReturn(dr, H10) ?? 0);
            entity.H11 = Convert.ToDecimal(valorReturn(dr, H11) ?? 0);
            entity.H12 = Convert.ToDecimal(valorReturn(dr, H12) ?? 0);
            entity.H13 = Convert.ToDecimal(valorReturn(dr, H13) ?? 0);
            entity.H14 = Convert.ToDecimal(valorReturn(dr, H14) ?? 0);
            entity.H15 = Convert.ToDecimal(valorReturn(dr, H15) ?? 0);
            entity.H16 = Convert.ToDecimal(valorReturn(dr, H16) ?? 0);
            entity.H17 = Convert.ToDecimal(valorReturn(dr, H17) ?? 0);
            entity.H18 = Convert.ToDecimal(valorReturn(dr, H18) ?? 0);
            entity.H19 = Convert.ToDecimal(valorReturn(dr, H19) ?? 0);
            entity.H20 = Convert.ToDecimal(valorReturn(dr, H20) ?? 0);
            entity.H21 = Convert.ToDecimal(valorReturn(dr, H21) ?? 0);
            entity.H22 = Convert.ToDecimal(valorReturn(dr, H22) ?? 0);
            entity.H23 = Convert.ToDecimal(valorReturn(dr, H23) ?? 0);
            entity.H24 = Convert.ToDecimal(valorReturn(dr, H24) ?? 0);
            entity.H25 = Convert.ToDecimal(valorReturn(dr, H25) ?? 0);
            entity.H26 = Convert.ToDecimal(valorReturn(dr, H26) ?? 0);
            entity.H27 = Convert.ToDecimal(valorReturn(dr, H27) ?? 0);
            entity.H28 = Convert.ToDecimal(valorReturn(dr, H28) ?? 0);
            entity.H29 = Convert.ToDecimal(valorReturn(dr, H29) ?? 0);
            entity.H30 = Convert.ToDecimal(valorReturn(dr, H30) ?? 0);
            entity.H31 = Convert.ToDecimal(valorReturn(dr, H31) ?? 0);
            entity.H32 = Convert.ToDecimal(valorReturn(dr, H32) ?? 0);
            entity.H33 = Convert.ToDecimal(valorReturn(dr, H33) ?? 0);
            entity.H34 = Convert.ToDecimal(valorReturn(dr, H34) ?? 0);
            entity.H35 = Convert.ToDecimal(valorReturn(dr, H35) ?? 0);
            entity.H36 = Convert.ToDecimal(valorReturn(dr, H36) ?? 0);
            entity.H37 = Convert.ToDecimal(valorReturn(dr, H37) ?? 0);
            entity.H38 = Convert.ToDecimal(valorReturn(dr, H38) ?? 0);
            entity.H39 = Convert.ToDecimal(valorReturn(dr, H39) ?? 0);
            entity.H40 = Convert.ToDecimal(valorReturn(dr, H40) ?? 0);
            entity.H41 = Convert.ToDecimal(valorReturn(dr, H41) ?? 0);
            entity.H42 = Convert.ToDecimal(valorReturn(dr, H42) ?? 0);
            entity.H43 = Convert.ToDecimal(valorReturn(dr, H43) ?? 0);
            entity.H44 = Convert.ToDecimal(valorReturn(dr, H44) ?? 0);
            entity.H45 = Convert.ToDecimal(valorReturn(dr, H45) ?? 0);
            entity.H46 = Convert.ToDecimal(valorReturn(dr, H46) ?? 0);
            entity.H47 = Convert.ToDecimal(valorReturn(dr, H47) ?? 0);
            entity.H48 = Convert.ToDecimal(valorReturn(dr, H48) ?? 0);
            entity.H49 = Convert.ToDecimal(valorReturn(dr, H49) ?? 0);
            entity.H50 = Convert.ToDecimal(valorReturn(dr, H50) ?? 0);
            entity.H51 = Convert.ToDecimal(valorReturn(dr, H51) ?? 0);
            entity.H52 = Convert.ToDecimal(valorReturn(dr, H52) ?? 0);
            entity.H53 = Convert.ToDecimal(valorReturn(dr, H53) ?? 0);
            entity.H54 = Convert.ToDecimal(valorReturn(dr, H54) ?? 0);
            entity.H55 = Convert.ToDecimal(valorReturn(dr, H55) ?? 0);
            entity.H56 = Convert.ToDecimal(valorReturn(dr, H56) ?? 0);
            entity.H57 = Convert.ToDecimal(valorReturn(dr, H57) ?? 0);
            entity.H58 = Convert.ToDecimal(valorReturn(dr, H58) ?? 0);
            entity.H59 = Convert.ToDecimal(valorReturn(dr, H59) ?? 0);
            entity.GPSNombre = valorReturn(dr, GPSNombre)?.ToString(); ;
            return entity;
        }





        #region Mapeo de Campos


        //CargaVirtual
        public string FechaHora = "FECHAHORA";
        public string GPSCodi = "GPSCODI";
        public string GPSNombre = "GPSNOMBRE";
        public string H0 = "H0";
        public string H1 = "H1";
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
        public string H25 = "H25";
        public string H26 = "H26";
        public string H27 = "H27";
        public string H28 = "H28";
        public string H29 = "H29";
        public string H30 = "H30";
        public string H31 = "H31";
        public string H32 = "H32";
        public string H33 = "H33";
        public string H34 = "H34";
        public string H35 = "H35";
        public string H36 = "H36";
        public string H37 = "H37";
        public string H38 = "H38";
        public string H39 = "H39";
        public string H40 = "H40";
        public string H41 = "H41";
        public string H42 = "H42";
        public string H43 = "H43";
        public string H44 = "H44";
        public string H45 = "H45";
        public string H46 = "H46";
        public string H47 = "H47";
        public string H48 = "H48";
        public string H49 = "H49";
        public string H50 = "H50";
        public string H51 = "H51";
        public string H52 = "H52";
        public string H53 = "H53";
        public string H54 = "H54";
        public string H55 = "H55";
        public string H56 = "H56";
        public string H57 = "H57";
        public string H58 = "H58";
        public string H59 = "H59";


        public string Mensaje = "MENSAJE";
        public string Resultado = "RESULTADO";

        #endregion Mapeo de Campos


        #region SQL
        public string SqlReporteFrecuenciaDesviacion
        {
            get { return base.GetSqlXml("GetReporteFrecuenciaDesviacion"); }
        }

        public string SqlReporteEventosFrecuencia
        {
            get { return base.GetSqlXml("GetReporteEventosFrecuencia"); }
        }


        #endregion SQL
    }
}
