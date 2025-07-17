using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.Servicios.Aplicacion.Intervenciones.Helper
{
    public class ParametrosIntervencionesAppServicio
    {
        private static List<InIntervencionDTO> ListaIntervencionesProgramadas;
        private static List<InIntervencionDTO> ListaIntervencionesEjecutadas;

        private static int TotalProgramados;
        private static int TotalEjecutados;
        private static int TotalAPE;
        private static int TotalAPNE;
        private static int TotalAENP;

        public static List<InIntervencionDTO> obtenerListaIntervencionesProgramadas()
        {
            return ListaIntervencionesProgramadas;
        }

        public static void asignarListaIntervencionesProgramadas(List<InIntervencionDTO> valor)
        {
            ListaIntervencionesProgramadas = valor;
        }

        public static List<InIntervencionDTO> obtenerListaIntervencionesEjecutadas()
        {
            return ListaIntervencionesEjecutadas;
        }

        public static void asignarListaIntervencionesEjecutadas(List<InIntervencionDTO> valor)
        {
            ListaIntervencionesEjecutadas = valor;
        }

        // ------------------------------------------------------------------------------------

        public static int obtenerTotalProgramados()
        {
            return TotalProgramados;
        }

        public static void asignarTotalProgramados(int valor)
        {
            TotalProgramados = valor;
        }

        public static int obtenerTotalEjecutados()
        {
            return TotalEjecutados;
        }

        public static void asignarTotalEjecutados(int valor)
        {
            TotalEjecutados = valor;
        }
        
        public static int obtenerTotalAPE()
        {
            return TotalAPE;
        }

        public static void asignarTotalAPE(int valor)
        {
            TotalAPE = valor;
        }

        public static int obtenerTotalAPNE()
        {
            return TotalAPNE;
        }

        public static void asignarTotalAPNE(int valor)
        {
            TotalAPNE = valor;
        }

        public static int obtenerTotalAENP()
        {
            return TotalAENP;
        }

        public static void asignarTotalAENP(int valor)
        {
            TotalAENP = valor;
        }

    }
}
