using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using System.Reflection;
using COES.MVC.Extranet.Areas.Medidores.Models;

namespace COES.Web.MVC.AppMedidoresMD.Utilities
{
    public class SelectListHelper
    {

       public static SelectList SelectList(IList lstItems, string dataValueField, string dataTextField, bool tieneSel = true, bool tieneTodos = false)
       
        {
            SelectList resultSlst = null;
            bool esVacio = false;
            if (lstItems == null || lstItems.Count == 0)
            {
                esVacio = true;
            }
            if (!esVacio)
            {
                var itmType = lstItems[0].GetType();
                PropertyInfo pDataInfo = itmType.GetProperty(dataValueField);
                object valSel = null;
                object valTodos = null;
                if (pDataInfo.PropertyType.Equals(typeof(int)))
                {
                    valSel = -1;
                    valTodos = 0;
                }
                else if (pDataInfo.PropertyType.Equals(typeof(string)))
                {
                    valSel = string.Empty;
                    valTodos = "TODOS";
                }
                PropertyInfo pTextInfo = itmType.GetProperty(dataTextField);
                ArrayList alstItems = new ArrayList(lstItems);
                // Seleccione...
                if (tieneSel)
                {
                    var itmSel = Activator.CreateInstance(itmType);
                    pDataInfo.SetValue(itmSel, valSel, null);
                    pTextInfo.SetValue(itmSel, "Seleccione...", null);
                    alstItems.Insert(0, itmSel);
                }
                // Todos
                if (tieneTodos)
                {
                    var itmTodos = Activator.CreateInstance(itmType);
                    pDataInfo.SetValue(itmTodos, valTodos, null);
                    pTextInfo.SetValue(itmTodos, "Todos", null);
                    alstItems.Add(itmTodos);
                }
                resultSlst = new SelectList(alstItems, dataValueField, dataTextField);
            }
            else
            {
                List<ParValorDescripcion<int>> lstOpciones = new List<ParValorDescripcion<int>>();
                ParValorDescripcion<int> opcionVacio = CrearOpcion<int>(0, "<Vacío>");
                lstOpciones.Add(opcionVacio);
                return new SelectList(lstOpciones, "Valor", "Descripcion");
            }
            return resultSlst;
        }

        public static ParValorDescripcion<T> CrearOpcion<T>(T valor, string descripcion)
        {
            ParValorDescripcion<T> opcion = new ParValorDescripcion<T>();
            opcion.Valor = valor;
            opcion.Descripcion = descripcion;
            return opcion;
        }

        public static SelectList SelectList(Type enumType, bool textValue = false, bool tieneSel = true, bool tieneTodos = false)
        {
            object valSel = null;
            object valTodos = null;
            if (!textValue)
            {
                valSel = -1;
                valTodos = 0;
            }
            else
            {
                valSel = string.Empty;
                valTodos = "TODOS";
            }


            Array arrEnumValues = Enum.GetValues(enumType);
            List<ParValorDescripcion<string>> lstOpciones = new List<ParValorDescripcion<string>>();
            foreach (object enumValue in arrEnumValues)
            {
                int valor = (int)enumValue;
                if (valor != 0)
                {
                    ParValorDescripcion<string> opcion = new ParValorDescripcion<string>();
                    if (!textValue)
                    {
                        opcion.Valor = valor.ToString();
                    }
                    else
                    {
                        opcion.Valor = enumValue.ToString().ToUpper();
                    }
                    opcion.Descripcion = enumValue.ToString();
                    lstOpciones.Add(opcion);
                }
            }
            // Seleccione...
            if (tieneSel)
            {
                ParValorDescripcion<string> opcionSel = CrearOpcion<string>(valSel.ToString(), "Seleccione...");
                lstOpciones.Insert(0, opcionSel);
            }
            // Todos
            if (tieneTodos)
            {
                ParValorDescripcion<string> opcionTodos = CrearOpcion<string>(valTodos.ToString(), "Todos");
                lstOpciones.Add(opcionTodos);
            }
            return new SelectList(lstOpciones, "Valor", "Descripcion");
        }


        public static SelectList SelectList(List<string> lstItems, bool tieneSel = true, bool tieneTodos = false)
        {
            SelectList resultSlst = null;
            List<ParValorDescripcion<int>> lstOpciones = new List<ParValorDescripcion<int>>();

            bool esVacio = false;
            if (lstItems == null || lstItems.Count == 0)
            {
                esVacio = true;
            }

            if (!esVacio)
            {
                int iCant = 1;
                foreach (var item in lstItems)
                {
                    //ParValorDescripcion<int> opcion = CrearOpcion<int>(item, item.ToString());
                    ParValorDescripcion<int> opcion = CrearOpcion<int>(iCant, item);
                    iCant++;
                    lstOpciones.Add(opcion);
                }
                // Seleccione...
                if (tieneSel)
                {
                    ParValorDescripcion<int> opcionSel = CrearOpcion<int>(-1, "Seleccione...");
                    lstOpciones.Insert(0, opcionSel);
                }
                // Todos
                if (tieneTodos)
                {
                    ParValorDescripcion<int> opcionTodos = CrearOpcion<int>(0, "Todos");
                    lstOpciones.Add(opcionTodos);
                }
            }
            else
            {
                ParValorDescripcion<int> opcionVacio = CrearOpcion<int>(0, "<Vacío>");
                lstOpciones.Add(opcionVacio);
            }
            resultSlst = new SelectList(lstOpciones, "Valor", "Descripcion");
            return resultSlst;
        }

        public static object SelectList(List<int> lstItems, bool tieneSel = true, bool tieneTodos = false)
        {
            SelectList resultSlst = null;
            List<ParValorDescripcion<int>> lstOpciones = new List<ParValorDescripcion<int>>();

            bool esVacio = false;
            if (lstItems == null || lstItems.Count == 0)
            {
                esVacio = true;
            }

            if (!esVacio)
            {
                foreach (var item in lstItems)
                {
                    ParValorDescripcion<int> opcion = CrearOpcion<int>(item, item.ToString());
                    lstOpciones.Add(opcion);
                }
                // Seleccione...
                if (tieneSel)
                {
                    ParValorDescripcion<int> opcionSel = CrearOpcion<int>(-1, "Seleccione...");
                    lstOpciones.Insert(0, opcionSel);
                }
                // Todos
                if (tieneTodos)
                {
                    ParValorDescripcion<int> opcionTodos = CrearOpcion<int>(0, "Todos");
                    lstOpciones.Add(opcionTodos);
                }
            }
            else
            {
                ParValorDescripcion<int> opcionVacio = CrearOpcion<int>(0, "<Vacío>");
                lstOpciones.Add(opcionVacio);
            }
            resultSlst = new SelectList(lstOpciones, "Valor", "Descripcion");
            return resultSlst;
        }


    }
}