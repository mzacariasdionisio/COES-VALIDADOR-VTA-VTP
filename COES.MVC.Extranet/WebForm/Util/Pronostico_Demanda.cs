using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSIC2010.Util
{

    public struct Valor
    {
         public double? ld_valor;
    }

    public class Pronostico_Demanda
    {
        private const int TIPOINFOCODI = 20;

        private int code;
        private int lectoCodi;
        private string lugar;
        private string nombreCarga;
        private string descripcion;
        private string codigoBarra;
        private string tensionBarra;
        private string fuenteInfo;
        private int tipoInfo = TIPOINFOCODI;
        private int infoCodi;
        private DateTime ldt_fecha;
        public double[] ld_array_demanda96;
        public double[] ld_array_demanda48;

        public int Codigo
        {
            get
            {
                return code;
            }
            set
            {
                code = value;
            }
        }

        public int LectoCodi
        {
            get
            {
                return lectoCodi;
            }
            set
            {
                lectoCodi = value;
            }
        }

        public string Lugar
        {
            get
            {
                return lugar;
            }
            set
            {
                lugar = value;
            }
        }

        public string Carga
        {
            get
            {
                return nombreCarga;
            }
            set
            {
                nombreCarga = value;
            }
        }

        public string Descripcion
        {
            get
            {
                return descripcion;
            }
            set
            {
                descripcion = value;
            }
        }

        public string CodigoBarra
        {
            get
            {
                return codigoBarra;
            }
            set
            {
                codigoBarra = value;
            }
        }

        public string TensionBarra
        {
            get
            {
                return tensionBarra;
            }
            set
            {
                tensionBarra = value;
            }
        }

        public string FuenteInfo
        {
            get
            {
                return fuenteInfo;
            }
            set
            {
                fuenteInfo = value;
            }
        }

        public int TipoInfo
        {
            get
            {
                return tipoInfo;
            }
            set
            {
                tipoInfo = value;
            }
        }

        public DateTime Fecha
        {
            get
            {
                return ldt_fecha;
            }
            set
            {
                ldt_fecha = value;
            }
        }

        public int InfoCodi
        {
            get
            {
                return infoCodi;
            }
        }

    }

    public class Pronostico_Demanda2
    {
        private const int TIPOINFOCODI = 20;

        private int code;
        private int lectoCodi;
        private string lugar;
        private string nombreCarga;
        private string descripcion;
        private string codigoBarra;
        private string tensionBarra;
        private string fuenteInfo;
        private int tipoInfo = TIPOINFOCODI;
        private int infoCodi;
        private DateTime ldt_fecha;
        public Valor[] ld_array_demanda96;
        public Valor[] ld_array_demanda48;

        public int Codigo
        {
            get
            {
                return code;
            }
            set
            {
                code = value;
            }
        }

        public int LectoCodi
        {
            get
            {
                return lectoCodi;
            }
            set
            {
                lectoCodi = value;
            }
        }

        public string Lugar
        {
            get
            {
                return lugar;
            }
            set
            {
                lugar = value;
            }
        }

        public string Carga
        {
            get
            {
                return nombreCarga;
            }
            set
            {
                nombreCarga = value;
            }
        }

        public string Descripcion
        {
            get
            {
                return descripcion;
            }
            set
            {
                descripcion = value;
            }
        }

        public string CodigoBarra
        {
            get
            {
                return codigoBarra;
            }
            set
            {
                codigoBarra = value;
            }
        }

        public string TensionBarra
        {
            get
            {
                return tensionBarra;
            }
            set
            {
                tensionBarra = value;
            }
        }

        public string FuenteInfo
        {
            get
            {
                return fuenteInfo;
            }
            set
            {
                fuenteInfo = value;
            }
        }

        public int TipoInfo
        {
            get
            {
                return tipoInfo;
            }
            set
            {
                tipoInfo = value;
            }
        }

        public DateTime Fecha
        {
            get
            {
                return ldt_fecha;
            }
            set
            {
                ldt_fecha = value;
            }
        }

        public int InfoCodi
        {
            get
            {
                return infoCodi;
            }
        }

    }
}