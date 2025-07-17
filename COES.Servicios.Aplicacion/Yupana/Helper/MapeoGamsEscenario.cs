using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Yupana.Helper
{
    public class MapeoGamsEscenario
    {
        public static List<CruceIdGamsCoes> ListaMapeo { get; set; }

        public static void AddItem(int idGam, int idCodigo, int tipo, int tipo2, bool? ficticio = null)
        {
            CruceIdGamsCoes reg = new CruceIdGamsCoes();
            reg.IdGams = idGam;
            reg.IdEquipo = idCodigo;
            reg.Tipo = tipo;
            reg.Tipo2 = tipo2;
            reg.Ficticio = ficticio;
            if (ListaMapeo == null)
                ListaMapeo = new List<CruceIdGamsCoes>();
            ListaMapeo.Add(reg);
        }

        public static int FindIdEquipo(int idGams, int tipo)
        {
            int idEquipo = 0;
            if (ListaMapeo != null)
            {
                var find = ListaMapeo.Find(x => x.IdGams == idGams && x.Tipo == tipo);
                if (find != null)
                    idEquipo = find.IdEquipo;
            }
            return idEquipo;
        }

        public static int FindIdGams(int idEquipo, int tipo)
        {
            int idGams = 0;
            if (ListaMapeo != null)
            {
                switch (tipo)
                {
                    case ConstantesBase.Trafo2D:
                    case ConstantesBase.Trafo3D:
                        var find2 = ListaMapeo.Find(x => x.IdEquipo == idEquipo && x.Tipo2 == tipo);
                        if (find2 != null)
                            idGams = find2.IdGams;
                        break;
                    default:
                        var find = ListaMapeo.Find(x => x.IdEquipo == idEquipo && x.Tipo == tipo);
                        if (find != null)
                            idGams = find.IdGams;
                        break;
                }

            }
            return idGams;
        }

        public static int FindCategoria2(int idGams, int tipo)
        {
            int idCat2 = 0;
            if (ListaMapeo != null)
            {
                var find = ListaMapeo.Find(x => x.IdGams == idGams && x.Tipo == tipo);
                if (find != null)
                    idCat2 = find.Tipo2;
            }
            return idCat2;
        }

        public static void Inicio()
        {
            ListaMapeo = null;
        }

        public static List<CruceIdGamsCoes> RetornaLista()
        {
            return ListaMapeo;
        }
    }

    public class CruceIdGamsCoes
    {
        public int IdGams { get; set; }
        public int IdEquipo { get; set; }
        public int Tipo { get; set; }
        public bool? Ficticio { get; set; }
        public int Tipo2 { get; set; } //para guaradar tipo linea, trafo2,trafo3
    }
}
