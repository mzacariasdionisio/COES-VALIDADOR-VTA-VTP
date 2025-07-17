using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Text;

namespace COES.Servicios.Aplicacion.IntercambioOsinergmin.Helper
{
    public class ArbolHelper
    {

        private static IEnumerable<ArbolMaestrosEnum> GetNodosPadresArbol()
        {
            return new List<ArbolMaestrosEnum>
            {
                ArbolMaestrosEnum.Maestros, ArbolMaestrosEnum.Sincronizacion
            };
        }

        private static string GetNodoPadreDisplay(ArbolMaestrosEnum arbolEnum)
        {
            switch (arbolEnum)
            {
                case ArbolMaestrosEnum.Maestros:
                    return ConstantesIio.MaestrosDisplayName;
                case ArbolMaestrosEnum.Sincronizacion:
                    return ConstantesIio.SincronizacionDisplayName;
                default:
                    throw new ArgumentOutOfRangeException("arbolEnum", arbolEnum, null);
            }
        }

        public static List<NodoArbolMaestrosDto> IniciArbolMaestrosDtos()
        {
            var arbol = new List<NodoArbolMaestrosDto>();
            var maestros = new List<NodoArbolMaestrosDto>();
            var pendientesAsignacion = new List<NodoArbolMaestrosDto>();

            var entidades = EntidadesHelper.GetEntidadesPorProceso(ProcesoEnum.Maestros);
            var nodosPadres = GetNodosPadresArbol();

            foreach (var entidadEnum in entidades)
            {
                var nodo = new NodoArbolMaestrosDto
                {
                    Title = EntidadesHelper.GetEntidadDisplayName(entidadEnum),
                    Key = (int) entidadEnum, 
                    Selected = (entidadEnum.Equals(ConstantesIio.EntidadMaestroSeleccionadaPorDefecto))
                };
                if (!EntidadesHelper.IsEntidadConAsignacionesPendiente(entidadEnum))
                    maestros.Add(nodo);
                else
                    pendientesAsignacion.Add(nodo);
            }

            foreach (var nodoPadre in nodosPadres)
            {
                var nodo = new NodoArbolMaestrosDto
                {
                    Title = GetNodoPadreDisplay(nodoPadre),
                    Key = (int) nodoPadre,
                    Selected = false
                };
                switch (nodoPadre)
                {
                    case ArbolMaestrosEnum.Maestros:
                        nodo.Children = maestros;
                        break;
                    case ArbolMaestrosEnum.Sincronizacion:
                        nodo.Children = pendientesAsignacion;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                arbol.Add(nodo);
            }

            return arbol;
        }

        public static string ObtenerEstructuraDatos(List<NodoArbolMaestrosDto> nodos)
        {
            var strHtml = new StringBuilder();
            strHtml.Append("[\n");
            
            int contador = 0;
            foreach (var item in nodos)
            {
                strHtml.Append("   {'key': '" + item.Key + "', 'title': '" +
                               item.Title + "' , selected : " + item.Selected.ToString().ToLower());

                if (item.Children != null && item.Children.Count > 0)
                {
                    strHtml.Append(", 'children':\n");
                    strHtml.Append(ObtenerEstructuraDatos(item.Children));
                }

                strHtml.Append(contador < nodos.Count - 1 ? "   },\n" : "   }\n");

                contador++;
            }

            strHtml.Append("]");

            return strHtml.ToString();
        }
    }
}