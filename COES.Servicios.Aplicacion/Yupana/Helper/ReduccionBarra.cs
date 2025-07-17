using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Yupana.Helper
{
    public class ReduccionBarra
    {
        public static List<BarraFict> ListaRecursoReduccion { get; set; }
        private static int IdIncio = 40000;
        public static void AddItem(int idBarra1, int idBarra2, string reducNombre, string tension)
        {
            BarraFict reg = new BarraFict();
            if (ListaRecursoReduccion == null)
                reg.RecurReduccion = IdIncio;
            else
            {
                int maximo = ListaRecursoReduccion.Max(x => x.RecurReduccion);
                reg.RecurReduccion = maximo + 1;
            }
            reg.Recurnombre = reducNombre;
            reg.Tension = tension;
            GrupoBarraReduccion registro = new GrupoBarraReduccion();
            registro.Recurcodi = reg.RecurReduccion;
            registro.RecurReduccion = idBarra1;
            reg.ListaReduccion = new List<GrupoBarraReduccion>();
            reg.ListaReduccion.Add(registro);
            registro = new GrupoBarraReduccion();
            registro.Recurcodi = reg.RecurReduccion;
            registro.RecurReduccion = idBarra2;
            reg.ListaReduccion.Add(registro);
            if (ListaRecursoReduccion == null)
            {
                ListaRecursoReduccion = new List<BarraFict>();
            }

            ListaRecursoReduccion.Add(reg);
        }

        public static int FindEquipo(int recurcodi)
        {
            int equipo = 0;
            if (ListaRecursoReduccion != null)
                foreach (var reg in ListaRecursoReduccion)
                {
                    var find = reg.ListaReduccion.Find(x => x.RecurReduccion == recurcodi);
                    if (find != null)
                    {
                        equipo = reg.RecurReduccion;
                        break;
                    }
                }
            return equipo;
        }

        public static void Inicio()
        {
            ListaRecursoReduccion = null;
        }
    }

    public class BarraFict
    {
        public int RecurReduccion { get; set; }
        public string Recurnombre { get; set; }
        public string Tension { get; set; }
        public List<GrupoBarraReduccion> ListaReduccion { get; set; }
    }

    public class GrupoBarraReduccion
    {
        public int RecurReduccion { get; set; }
        public int Recurcodi { get; set; }
    }
}
