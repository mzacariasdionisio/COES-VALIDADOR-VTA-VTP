using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.PotenciaFirme;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.PotenciaFirmeRemunerable.Helper
{
    class UtilPotenciaFirmeRemunerable
    {
        public static string GenerarEntradaGams(List<PfrEntidadDTO> lbarra, List<PestaniaDemanda> ldemanda, List<PfrEntidadDTO>  lgeneracion,
             List<PfrEntidadDTO> lcompreactiva, List<PfrEntidadDTO> lenlace, List<PfrEntidadDTO>  lcongestion, List<PfrEntidadDTO> lpenalidad)
        {
            string log = "";
            string fila = string.Empty;

            #region Barras Dat
            AgregaLinea(ref log, "*********************************************");
            AgregaLinea(ref log, "* Barras");
            AgregaLinea(ref log, "*********************************************");
            AgregaLinea(ref log, "SET NB /");
            /// Escribir las barras de base de datos
           foreach(var reg in lbarra)
            {
                fila = ConstantesPotenciaFirmeRemunerable.MargenIzquierdoDat;
                
                fila += WriteArchivoGams_(reg.Pfrentid, 6, 1);
                fila += WriteArchivoGams_(reg.Pfrentnomb, (reg.Pfrentnomb.Length), 0);
                AgregaLinea(ref log, fila);
            }

            AgregaLinea(ref log, "/;");
            AgregaLinea(ref log, "");

            #endregion

            #region Demanda Dat
            AgregaLinea(ref log, "*********************************************");
            AgregaLinea(ref log, "* Demandas");
            AgregaLinea(ref log, "*********************************************");
            AgregaLinea(ref log, "TABLE Demanda(NB,*)");
            ///Cabecera
            fila = ConstantesPotenciaFirmeRemunerable.MargenIzquierdoDat;
            
            fila += WriteArchivoGams_("", 9, 0);
            fila += WriteArchivoGams_("V", 11, 0);
            fila += WriteArchivoGams_("Angulo", 13, 0);
            fila += WriteArchivoGams_("Pc", 8, 0);
            fila += WriteArchivoGams_("Qc", 10, 0);
            fila += WriteArchivoGams_("ShuntR", 8, 0);
            fila += WriteArchivoGams_("ShuntI", 6, 0);
            AgregaLinea(ref log, fila);
            /// Escribir Demanda de Base de Datos
            foreach (var reg in ldemanda)
            {
                var comReac = reg.CompReactiva;
                string strCompReactiva = comReac != null ? Decimal.ToDouble(reg.CompReactiva.Value).ToString("000.000") : "";

                fila = ConstantesPotenciaFirmeRemunerable.MargenIzquierdoDat;
                
                fila += WriteArchivoGams_(reg.IdBarra, 8, 3);
                fila += WriteArchivoGams_("1.000000", 11, 3);
                fila += WriteArchivoGams_("000.00000", 12, 3);
                fila += WriteArchivoGams_((reg.P != null) ? Decimal.ToDouble(reg.P.Value).ToString("000.000") : "", 10, 3);
                fila += WriteArchivoGams_((reg.Q != null) ? Decimal.ToDouble(reg.Q.Value).ToString("000.000") : "", 10, 3);
                fila += WriteArchivoGams_("000.000", 10, 3);
                
                fila += WriteArchivoGams_(strCompReactiva, strCompReactiva.Length, 0);
                AgregaLinea(ref log, fila);
            }
            AgregaLinea(ref log, ";");
            AgregaLinea(ref log, "");

            #endregion

            #region Unidades de Generacion Dat

            AgregaLinea(ref log, "********************************************* /");
            AgregaLinea(ref log, "* Unidades generacion /");
            AgregaLinea(ref log, "********************************************* /");
            AgregaLinea(ref log, "SET  GT  /");
            /// Escribir CT de Base de Datos
            foreach(var reg in lgeneracion)
            {
                fila = ConstantesPotenciaFirmeRemunerable.MargenIzquierdoDat;
                
                fila += WriteArchivoGams_(reg.Pfrentid, 5, 1);
                fila += WriteArchivoGams_(reg.Pfrentnomb, (reg.Pfrentnomb.Length) + 1, 0);
                fila += WriteArchivoGams_(reg.Numunidad.Value.ToString(), 1, 0);
                AgregaLinea(ref log, fila);
            }
            AgregaLinea(ref log, "/;");
            AgregaLinea(ref log, "");

            AgregaLinea(ref log, "SET  PgBus(GT,NB) /");
            /// Escribir CT de Base de Datos
            foreach(var reg in lgeneracion)
            {
                fila = ConstantesPotenciaFirmeRemunerable.MargenIzquierdoDat;
                
                fila += WriteArchivoGams_(reg.Pfrentid + "." + reg.Idbarra1, 10, 0);
                AgregaLinea(ref log, fila);
            }
            
            AgregaLinea(ref log, "/;");
            AgregaLinea(ref log, "");

            AgregaLinea(ref log, "TABLE PgData(GT,*)");
            /// Escribir cabecera
            fila = ConstantesPotenciaFirmeRemunerable.MargenIzquierdoDat;
            
            fila += WriteArchivoGams_("", 11, 3);
            fila += WriteArchivoGams_("Pgen", 14, 3);
            fila += WriteArchivoGams_("Qgen", 14, 3);
            fila += WriteArchivoGams_("Pmin", 14, 3);
            fila += WriteArchivoGams_("Pmax", 14, 3);
            fila += WriteArchivoGams_("Qmin", 13, 3);
            fila += WriteArchivoGams_("Qmax", 12, 3);
            fila += WriteArchivoGams_("CI1", 9, 3);
            fila += WriteArchivoGams_("Forzada", 12, 3);
            fila += WriteArchivoGams_("Sist", 6, 2);
            fila += WriteArchivoGams_("Calif", 7, 2);
            fila += WriteArchivoGams_("Tipo", 5, 1);
            fila += WriteArchivoGams_("Conec", 7, 2);
            fila += WriteArchivoGams_("Ref", 3, 0);
            AgregaLinea(ref log, fila);
            /// Escribir de Base de Datos
            foreach(var reg in lgeneracion)
            {
                fila = ConstantesPotenciaFirmeRemunerable.MargenIzquierdoDat;
                
                fila += WriteArchivoGams_(reg.Pfrentid, 7, 3);
                fila += WriteArchivoGams_("0.00000E+00", 14, 3);
                fila += WriteArchivoGams_("0.00000E+00", 14, 3);
                fila += WriteArchivoGams_("0.00000E+00", 14, 3); //PMin
                //fila += WriteArchivoGams_((reg.Pfrrgepmax != null) ? ((decimal)reg.Pfrrgepmax).ToString("E5", CultureInfo.InvariantCulture) : "", 13, 2);
                //fila += WriteArchivoGams_((reg.Pfrrgeqmin != null) ? ((decimal)reg.Pfrrgeqmin).ToString("E5", CultureInfo.InvariantCulture) : "", 15, 3);
                //fila += WriteArchivoGams_((reg.Pfrrgeqmax != null) ? ((decimal)reg.Pfrrgeqmax).ToString("E5", CultureInfo.InvariantCulture) : "", 12, 2);
                fila += WriteArchivoGams_(DarFormatoExponencial(reg.Potenciamax, "E5"), 13, 2);
                fila += WriteArchivoGams_(DarFormatoExponencial(reg.Qmin, "E5"), 15, 3);
                fila += WriteArchivoGams_(DarFormatoExponencial(reg.Qmax, "E4"), 12, 2);
                
                fila += WriteArchivoGams_((reg.Costov != null) ? ((decimal)reg.Costov).ToString("0000.0000") : "", 14, 5);
                fila += WriteArchivoGams_("no", 10, 8);
                fila += WriteArchivoGams_("1", 6, 5);
                fila += WriteArchivoGams_("0", 8, 7);
                fila += WriteArchivoGams_("1", 4, 3);
                fila += WriteArchivoGams_("01", 6, 4);
                fila += WriteArchivoGams_((reg.Ref != null) ? (reg.Ref == 1 ? "yes" : "no") : "no", (reg.Ref != null) ? (reg.Ref == 1 ? 3 : 2) : 2, 0);
                AgregaLinea(ref log, fila);
            }
            AgregaLinea(ref log, ";");
            AgregaLinea(ref log, "");

            AgregaLinea(ref log, "TABLE Tension(NB,*)");
            /// Escribir cabecera
            fila = ConstantesPotenciaFirmeRemunerable.MargenIzquierdoDat;
            
            fila += WriteArchivoGams_("", 10, 3);
            fila += WriteArchivoGams_("Vmin", 11, 3);
            fila += WriteArchivoGams_("Vmax", 8, 3);
            fila += WriteArchivoGams_("Tipo", 4, 0);
            AgregaLinea(ref log, fila);
            //Escribir de base de datos
            foreach(var reg in lbarra)
            {
                fila = ConstantesPotenciaFirmeRemunerable.MargenIzquierdoDat;
                
                fila += WriteArchivoGams_(reg.Pfrentid, 8, 3);
                fila += WriteArchivoGams_((reg.Vmin != null) ? ((decimal)reg.Vmin).ToString("0.000000") : "", 11, 3);
                fila += WriteArchivoGams_((reg.Vmax != null) ? ((decimal)reg.Vmax).ToString("0.000000") : "", 12, 4);
                fila += WriteArchivoGams_("2", 1, 0);
                AgregaLinea(ref log, fila);
            }

            AgregaLinea(ref log, ";");
            AgregaLinea(ref log, "");

            #endregion

            #region Compensacion Reactiva dinámica

            AgregaLinea(ref log, "********************************************* /");
            AgregaLinea(ref log, "* Compensacion Reactiva dinámica /");
            AgregaLinea(ref log, "********************************************* /");
            AgregaLinea(ref log, "SET  CR  /");
            ///Escribir Comp Reactiva de Base de Datos
            foreach(var reg in lcompreactiva)
            {
                fila = ConstantesPotenciaFirmeRemunerable.MargenIzquierdoDat;
                
                fila += WriteArchivoGams_(reg.Pfrentid, 5, 1);
                fila += WriteArchivoGams_(reg.Idbarra1desc, (reg.Idbarra1desc.Length), 0); 
                AgregaLinea(ref log, fila);
            }
            AgregaLinea(ref log, "/;");
            AgregaLinea(ref log, "");

            AgregaLinea(ref log, "SET  CRBus(CR,NB) /");
            ///Escribir Comp Reactiva y Barra de Base de Datos
            foreach(var reg in lcompreactiva)
            {
                fila = ConstantesPotenciaFirmeRemunerable.MargenIzquierdoDat;
                
                fila += WriteArchivoGams_(reg.Pfrentid + "." + reg.Idbarra1, 10, 0);
                AgregaLinea(ref log, fila);
            }

            AgregaLinea(ref log, "/;");
            AgregaLinea(ref log, "");

            AgregaLinea(ref log, "TABLE CRData(CR,*)");
            /// Escribir cabecera
            fila = ConstantesPotenciaFirmeRemunerable.MargenIzquierdoDat;
            
            fila += WriteArchivoGams_("", 11, 0);
            fila += WriteArchivoGams_("Q", 13, 0);
            fila += WriteArchivoGams_("Qmin", 12, 0);
            fila += WriteArchivoGams_("Qmax", 11, 0);
            fila += WriteArchivoGams_("Conec", 5, 0);
            AgregaLinea(ref log, fila);
            ///Escribir de base de datos
            foreach (var reg in lcompreactiva)
            {
                fila = ConstantesPotenciaFirmeRemunerable.MargenIzquierdoDat;
                
                fila += WriteArchivoGams_(reg.Pfrentid, 8, 4);
                fila += WriteArchivoGams_("0.000E+00", 12, 3);
                //fila += WriteArchivoGams_((reg.Pfreqpqmin != null) ? ((decimal)reg.Pfreqpqmin).ToString("E3", CultureInfo.InvariantCulture) : "", 14, 4);
                //fila += WriteArchivoGams_((reg.Pfreqpqmax != null) ? ((decimal)reg.Pfreqpqmax).ToString("E3", CultureInfo.InvariantCulture) : "", 13, 4);
                fila += WriteArchivoGams_(DarFormatoExponencial(reg.Qmin, "E3"), 14, 4);
                fila += WriteArchivoGams_(DarFormatoExponencial(reg.Qmax, "E3"), 13, 4);
                fila += WriteArchivoGams_("01", 2, 0);

                AgregaLinea(ref log, fila);
            }

            AgregaLinea(ref log, ";");
            AgregaLinea(ref log, "");

            #endregion

            #region Enlaces
            AgregaLinea(ref log, "*********************************************");
            AgregaLinea(ref log, "* Enlaces");
            AgregaLinea(ref log, "*********************************************");
            AgregaLinea(ref log, "SET  ENL  /");
            ///Escribir Enlaces de Base de Datos
            foreach(var reg in lenlace)
            {
                fila = ConstantesPotenciaFirmeRemunerable.MargenIzquierdoDat;
                
                fila += WriteArchivoGams_(reg.Pfrentid, 9, 4);
                fila += WriteArchivoGams_(reg.Idbarra1desc + "-" + reg.Idbarra2desc, (reg.Idbarra1desc.Length + reg.Idbarra2desc.Length + 1), 0);

                AgregaLinea(ref log, fila);
            }
            AgregaLinea(ref log, "/;");
            AgregaLinea(ref log, "");

            AgregaLinea(ref log, "SET  FBus(ENL,NB,NB) /");
            //Escribir enlace barra de base de datos
            foreach (var reg in lenlace)
            {
                fila = ConstantesPotenciaFirmeRemunerable.MargenIzquierdoDat;
                
                fila += WriteArchivoGams_(reg.Pfrentid + "." + reg.Idbarra1 + "." + reg.Idbarra2, 17, 0);
                AgregaLinea(ref log, fila);
            }
            AgregaLinea(ref log, "/;");
            AgregaLinea(ref log, "");

            AgregaLinea(ref log, "TABLE  FData(ENL,*)");
            /// Escribir cabecera
            fila = ConstantesPotenciaFirmeRemunerable.MargenIzquierdoDat;
            
            fila += WriteArchivoGams_("", 13, 0);
            fila += WriteArchivoGams_("R0", 12, 0);
            fila += WriteArchivoGams_("X0", 14, 0);
            fila += WriteArchivoGams_("G0", 13, 0);
            fila += WriteArchivoGams_("B0", 8, 0);
            fila += WriteArchivoGams_("Tap1", 8, 0);
            fila += WriteArchivoGams_("Tap2", 8, 0);
            fila += WriteArchivoGams_("Pmax", 9, 0);
            fila += WriteArchivoGams_("Cong", 6, 0);
            fila += WriteArchivoGams_("Envio", 6, 0);
            fila += WriteArchivoGams_("Tipo", 5, 0);
            fila += WriteArchivoGams_("Conec", 8, 0);
            fila += WriteArchivoGams_("Sis", 3, 0);
            AgregaLinea(ref log, fila);
            /// Escribir de Base de Datos
            foreach (var reg in lenlace)
            {
                fila = ConstantesPotenciaFirmeRemunerable.MargenIzquierdoDat;
                
                decimal? valorNulo = null;
                decimal valDefault = -9999999999;

                var resistencia = (reg.Resistencia != null) ? (decimal)reg.Resistencia : valorNulo;
                decimal decResistencia = resistencia != valorNulo ? resistencia.Value : valDefault;
                //string strResistencia = decResistencia != valDefault ? (decResistencia < 0 ? decResistencia.ToString("E5", CultureInfo.InvariantCulture) : (" " + decResistencia.ToString("E5", CultureInfo.InvariantCulture))) : "";
                string strResistencia = decResistencia != valDefault ? (decResistencia < 0 ? DarFormatoExponencial(decResistencia, "E5") : (" " + DarFormatoExponencial(decResistencia, "E5"))) : "";                

                var reactancia = (reg.Reactancia != null) ? (decimal)reg.Reactancia : valorNulo;
                decimal decReactancia = reactancia != valorNulo ? reactancia.Value : valDefault;
                //string strReactancia = decReactancia != valDefault ? (decReactancia < 0 ? decReactancia.ToString("E5", CultureInfo.InvariantCulture) : (" " + decReactancia.ToString("E5", CultureInfo.InvariantCulture))) : "";
                string strReactancia = decReactancia != valDefault ? (decReactancia < 0 ? DarFormatoExponencial(decReactancia, "E5") : (" " + DarFormatoExponencial(decReactancia, "E5"))) : "";

                var admitancia = (reg.Admitancia != null) ? (decimal)reg.Admitancia : valorNulo;
                decimal decAdmitancia = admitancia != valorNulo ? admitancia.Value : valDefault;
                //string strAdmitancia = decAdmitancia != valDefault ? (decAdmitancia < 0 ? decAdmitancia.ToString("E5", CultureInfo.InvariantCulture) : (" " + decAdmitancia.ToString("E5", CultureInfo.InvariantCulture))) : "";
                string strAdmitancia = decAdmitancia != valDefault ? (decAdmitancia < 0 ? DarFormatoExponencial(decAdmitancia, "E5") : (" " + DarFormatoExponencial(decAdmitancia, "E5"))) : "";


                fila += WriteArchivoGams_(reg.Pfrentid, 8, 3);
                fila += WriteArchivoGams_(strResistencia, 13, 1);
                fila += WriteArchivoGams_(strReactancia, 14, 2);
                //fila += WriteArchivoGams_((reg.Pfreqpconductancia != null) ? ((decimal)reg.Pfreqpconductancia).ToString("E5", CultureInfo.InvariantCulture) : "", 12, 1);
                fila += WriteArchivoGams_(DarFormatoExponencial(reg.Conductancia, "E5"), 12, 1);
                
                fila += WriteArchivoGams_(strAdmitancia, 13, 1);
                fila += WriteArchivoGams_((reg.Tap1 != null) ? ((decimal)reg.Tap1).ToString("0.00000") : "", 8, 1);
                fila += WriteArchivoGams_((reg.Tap2 != null) ? ((decimal)reg.Tap2).ToString("0.00000") : "", 8, 1);
                fila += WriteArchivoGams_((reg.Potenciamax != null) ? ((decimal)reg.Potenciamax).ToString("0000.00") : "", 11, 4);
                fila += WriteArchivoGams_("no", 6, 4);
                fila += WriteArchivoGams_("yes", 6, 3);
                fila += WriteArchivoGams_("2", 6, 5);
                fila += WriteArchivoGams_("01", 6, 4);
                fila += WriteArchivoGams_("1", 1, 0);
                AgregaLinea(ref log, fila);
            }
            AgregaLinea(ref log, ";");
            AgregaLinea(ref log, "");


            #endregion

            #region Congestion de varios enlaces en simultaneo

            AgregaLinea(ref log, "*********************************************");
            AgregaLinea(ref log, "* Congestion de varios enlaces en simultaneo");
            AgregaLinea(ref log, "*********************************************");
            AgregaLinea(ref log, "SET CONG /");
            ///Escribir Congestion de Base de Datos
            foreach (var reg in lcongestion)
            {
                fila = "       ";                
                fila += WriteArchivoGams_(reg.Pfrentid, 8, 4);
                fila += WriteArchivoGams_(reg.Pfrentnomb, (reg.Pfrentnomb.Length), 0);
                AgregaLinea(ref log, fila);
            }

            AgregaLinea(ref log, "        /;");
            AgregaLinea(ref log, "SET CONGRuta(CONG,ENL)");
            AgregaLinea(ref log, "      /");
            ///Escribir Congestion - Enlace de Base de Datos
            string lineast = "";
            foreach (var reg in lcongestion)
            {
                fila = "      ";
                lineast = reg.Lineasdesc.Replace('-',',');
                fila += reg.Pfrentid + ".(" + lineast + ")";
                AgregaLinea(ref log, fila);
            }
            AgregaLinea(ref log, "      /;");
            AgregaLinea(ref log, "TABLE CONGmax(CONG,*)");

            /// Escribir cabecera
            fila = "      ";            
            fila += WriteArchivoGams_("", 11, 0);
            fila += WriteArchivoGams_("Pmax", 9, 0);
            fila += WriteArchivoGams_("Cong", 7, 0);
            fila += WriteArchivoGams_("Tipo", 6, 0);
            fila += WriteArchivoGams_("Envio", 5, 0);
            AgregaLinea(ref log, fila);
            ///Escribir Congestion - Enlace de Base de Datos
            foreach (var reg in lcongestion)
            {
                fila = "      ";                
                fila += WriteArchivoGams_(reg.Pfrentid, 8, 4);
                fila += WriteArchivoGams_((reg.Pmax != null) ? ((decimal)reg.Pmax).ToString("0000.000") : "", 12, 4);
                fila += WriteArchivoGams_("yes", 8, 5);
                fila += WriteArchivoGams_("2", 7, 6);
                fila += WriteArchivoGams_("1", 1, 0);
                AgregaLinea(ref log, fila);
            }
            AgregaLinea(ref log, ";");

            #endregion

            #region Penalidades

            AgregaLinea(ref log, "SCALAR");
            foreach (var reg in lpenalidad)
            {
                
                fila = WriteArchivoGams_("  " + reg.Pfrentid + "  /" + reg.Penalidad + "/", (reg.Pfrentid.Length + reg.Penalidad.Length + 6), 0);

                AgregaLinea(ref log, fila);
            }

            #endregion

            return log;
        }

        public static string DarFormatoExponencial(decimal? valor, string tipo)
        {
            string salida = "";
            if (valor != null)
            {
                string strValReal = ((decimal)valor).ToString(tipo, CultureInfo.InvariantCulture);
                salida = ConversionADosCifrasExponenciales(strValReal);
            }
            
            return salida;
        }

        private static string ConversionADosCifrasExponenciales(string strValReal)
        {
            string salidaEditadaa = "";
            string copia = strValReal;
            salidaEditadaa = copia.Replace("E+0", "E+").Replace("E-0", "E-");
            return salidaEditadaa;
        }

        public static string WriteArchivoGams_(string campo, int ancho, int espacio)
        {
            int sizeColumna = campo.Length;

            if (sizeColumna > ancho - espacio)
                campo = campo.Substring(0, ancho - espacio);

            return campo.PadRight(ancho);
        }

        /// <summary>
        /// Agregar una nueva linea al formato de salida
        /// </summary>
        /// <param name="log"></param>
        /// <param name="linea"></param>
        private static void AgregaLinea(ref string log, string linea)
        {
            log += linea + "\r\n";
        }

    }
}
