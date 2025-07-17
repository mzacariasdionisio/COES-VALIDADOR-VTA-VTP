using System;
using System.Collections;
using System.Windows.Forms;

namespace fwapp
{
   public class n_parameter : Hashtable
   {
      public n_parameter in_parameter = null;

      public n_parameter()
      {
      }

      public n_parameter(string as_varname, object a_data)
      {
         SetData(as_varname, a_data);
      }

      public int SetData(string as_varname, object a_data)
      {
         this[as_varname.Trim().ToUpper()] = a_data;
         return 1;
      }

      public object GetData(string as_varname)
      {
         if (base.Contains(as_varname.Trim().ToUpper()))
            return this[as_varname.Trim().ToUpper()];
         if (in_parameter != null)
            return in_parameter.GetData(as_varname);
         MessageBox.Show("No existe la variable : " + as_varname,"Modo depuración");
         return null;
      }

      public void SetParameters(n_parameter ai_parameter)
      {
         in_parameter = ai_parameter;
      }

      public string GetString(string as_varname)
      {
         return (string)GetData(as_varname);
      }

      public double GetNumber(string as_varname)
      {
         return Convert.ToDouble(GetData(as_varname));
      }

      public int GetInteger(string as_varname)
      {
         return Convert.ToInt32(GetData(as_varname));
      }

      public bool GetBool(string as_varname)
      {
         return (bool)GetData(as_varname);
      }

      public DateTime GetDate(string as_varname)
      {
         return (DateTime)GetData(as_varname);
      }

      public bool Contains(string as_varname)
      {
         if (base.Contains(as_varname.ToUpper()))
            return true;
         if (in_parameter != null)
            return in_parameter.Contains(as_varname);
         return false;
      }

      public string GetExpression(string as_varname)
      {
         if (Contains(as_varname))
         {
            string l_cadena = GetString(as_varname).Trim().Replace(" ", "");
            return EvaluateFormula(l_cadena);
         }         
         return as_varname;
      }

      public string EvaluateFormula(string as_formula)
      {
         string l_cadena = as_formula;
         int inicio = 0;
         bool b_IsVariable = false;


         while (l_cadena.Substring(0, 1) == "+" || l_cadena.Substring(0, 1) == "=")
            l_cadena = l_cadena.Substring(1);

         int li_cpp = l_cadena.LastIndexOf("++");
         while (li_cpp >= 0)
         {
            l_cadena = l_cadena.Substring(0, li_cpp + 1) + l_cadena.Substring(li_cpp + 2);
            li_cpp = l_cadena.LastIndexOf("++");
         }

         li_cpp = l_cadena.LastIndexOf("(+");
         while (li_cpp >= 0)
         {
            l_cadena = l_cadena.Substring(0, li_cpp + 1) + l_cadena.Substring(li_cpp + 2);
            li_cpp = l_cadena.LastIndexOf("(+");
         }

         for (int i = 0; i < l_cadena.Length; i++)
         {

            if (char.IsLetterOrDigit(l_cadena[i]) || l_cadena[i] == '_')
            {
               if (char.IsLetter(l_cadena[i]))
               {
                  if (inicio == i) b_IsVariable = true;
               }
               else
                  if (char.IsDigit(l_cadena[i]))
                  {
                     if (inicio == i) b_IsVariable = false;
                  }
            }
            else
            {
               if (b_IsVariable)
               {
                  string s_variable = "(" + GetExpression(l_cadena.Substring(inicio, i - inicio)) + ")";
                  l_cadena = l_cadena.Substring(0, inicio) + s_variable + l_cadena.Substring(i);
                  i = inicio + s_variable.Length;
                  b_IsVariable = false;
               }
               inicio = i + 1;
            }
         }
         if (b_IsVariable)
         {
            string s_variable = GetExpression(l_cadena.Substring(inicio, l_cadena.Length - inicio));
            l_cadena = l_cadena.Substring(0, inicio) + s_variable;
         }
         //****ESTO ACTUALIZA LA VARIABLE CON LA EXPRESION DESARROLLADA
         //SetData(as_varname, l_cadena);
         if (l_cadena.Trim().Length == 0)
            return "0";
         else
            return l_cadena;
      }

      public double GetEvaluate(string as_varname)
      {
         string stemp;
         EPFunction func = new EPFunction();
         if(Contains(as_varname))
            stemp = GetExpression(as_varname);
         else
            stemp = EvaluateFormula(as_varname);
         func.Parse(stemp);
         func.Infix2Postfix();
         func.EvaluatePostfix();
         if (func.Error)
         {
            MessageBox.Show("La expresión: \n" + GetExpression(as_varname) + "\n contiene componentes no numéricos.", func.ErrorDescription);
            return -1;
         }
         return func.Result;
      }
   }

   public class n_parameter2
   {
      n_parameter2 in_parameter = null;
      private ArrayList iAL_VarName = new ArrayList();
      private ArrayList iAL_VarValue = new ArrayList();

      public n_parameter2()
      {
      }

      public n_parameter2(string as_varname, object a_data)
      {
         SetData(as_varname, a_data);
      }

      public int SetData(string as_varname, object a_data)
      {
         for (int i = 0; i < this.iAL_VarName.Count; i++)
         {
            if (iAL_VarName[i].ToString() == as_varname.ToUpper())
            {
               iAL_VarValue[i] = a_data;
               return 1;
            }
         }
         iAL_VarName.Add(as_varname.ToUpper());
         iAL_VarValue.Add(a_data);
         return 1;
      }

      public object GetData(string as_varname)
      {
         for (int i = 0; i < iAL_VarName.Count; i++)
         {
            if (iAL_VarName[i].ToString() == as_varname.ToUpper()) return iAL_VarValue[i];
         }
         if (in_parameter != null)
            return in_parameter.GetData(as_varname);
         //MessageBox.Show("Modo depuración", "No existe la variable : " + as_varname);
         MessageBox.Show("No existe la variable : " + as_varname, "Modo depuración");
         return null;
      }
      public void AddParameter(n_parameter2 ai_parameter)
      {
         in_parameter = ai_parameter;
      }

      public string GetString(string as_varname)
      {
         return (string)GetData(as_varname);
      }

      public double GetNumber(string as_varname)
      {
         return (double)GetData(as_varname);
      }

      public int GetInteger(string as_varname)
      {
         return Convert.ToInt32(GetData(as_varname));
      }

      public bool GetBool(string as_varname)
      {
         return (bool)GetData(as_varname);
      }

      public DateTime GetDate(string as_varname)
      {
         return (DateTime)GetData(as_varname);
      }

      public bool Contains(string as_varname)
      {
         for (int i = 0; i < this.iAL_VarName.Count; i++)
         {
            if (iAL_VarName[i].ToString() == as_varname.ToUpper())
            {
               return true;
            }
         }
         if (in_parameter != null)
            return in_parameter.Contains(as_varname);
         return false;
      }

      public string GetExpression(string as_varname)
      {
         if (Contains(as_varname))
         {
            string l_cadena = GetString(as_varname).Trim().Replace(" ", "");
            int inicio = 0;
            bool b_IsVariable = false;


            while (l_cadena.Substring(0, 1) == "+" || l_cadena.Substring(0, 1) == "=")
               l_cadena = l_cadena.Substring(1);

            int li_cpp = l_cadena.LastIndexOf("++");
            while (li_cpp >= 0)
            {
               l_cadena = l_cadena.Substring(0, li_cpp + 1) + l_cadena.Substring(li_cpp + 2);
               li_cpp = l_cadena.LastIndexOf("++");
            }

            li_cpp = l_cadena.LastIndexOf("(+");
            while (li_cpp >= 0)
            {
               l_cadena = l_cadena.Substring(0, li_cpp + 1) + l_cadena.Substring(li_cpp + 2);
               li_cpp = l_cadena.LastIndexOf("(+");
            }

            for (int i = 0; i < l_cadena.Length; i++)
            {

               if (char.IsLetterOrDigit(l_cadena[i]) || l_cadena[i] == '_')
               {
                  if (char.IsLetter(l_cadena[i]))
                  {
                     if (inicio == i) b_IsVariable = true;
                  }
                  else
                     if (char.IsDigit(l_cadena[i]))
                     {
                        if (inicio == i) b_IsVariable = false;
                     }
               }
               else
               {
                  if (b_IsVariable)
                  {
                     string s_variable = "(" + GetExpression(l_cadena.Substring(inicio, i - inicio)) + ")";
                     l_cadena = l_cadena.Substring(0, inicio) + s_variable + l_cadena.Substring(i);
                     i = inicio + s_variable.Length;
                     b_IsVariable = false;
                  }
                  inicio = i + 1;
               }
            }
            if (b_IsVariable)
            {
               string s_variable = GetExpression(l_cadena.Substring(inicio, l_cadena.Length - inicio));
               l_cadena = l_cadena.Substring(0, inicio) + s_variable;
            }
            //****ESTO ACTUALIZA LA VARIABLE CON LA EXPRESION DESARROLLADA
            //SetData(as_varname, l_cadena);
            if (l_cadena.Trim().Length == 0)
               return "0";
            else
               return l_cadena;
         }
         return as_varname;
      }

      public double GetEvaluate(string as_varname)
      {
         EPFunction func = new EPFunction();
         string stemp = GetExpression(as_varname);
         func.Parse(stemp);
         func.Infix2Postfix();
         func.EvaluatePostfix();
         if (func.Error)
         {
            MessageBox.Show(func.ErrorDescription);
            return -1;
         }
         return func.Result;
      }
   }


}
