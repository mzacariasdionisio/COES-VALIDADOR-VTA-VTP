using System;
using System.Collections;

namespace fwapp
{
   /// <summary>
   /// </summary>
   /// 
   public enum EPType { Variable, Value, Operator, Function, Result, Bracket, Comma, Error }
   public struct EPSymbol
   {
      public string m_name;
      public double m_value;
      public EPType m_type;
      public override string ToString()
      {
         return m_name;
      }
   }
   public delegate EPSymbol EvaluateFunctionDelegate(string name, params Object[] args);
   public class EPFunction
   {
      public double Result
      {
         get
         {
            return m_result;
         }
      }

      public ArrayList Equation
      {
         get
         {
            return (ArrayList)m_equation.Clone();
         }
      }
      public ArrayList Postfix
      {
         get
         {
            return (ArrayList)m_postfix.Clone();
         }
      }

      public EvaluateFunctionDelegate DefaultFunctionEvaluation
      {
         set
         {
            m_defaultFunctionEvaluation = value;
         }
      }

      public bool Error
      {
         get
         {
            return m_bError;
         }
      }

      public string ErrorDescription
      {
         get
         {
            return m_sErrorDescription;
         }
      }

      public ArrayList Variables
      {
         get
         {
            ArrayList var = new ArrayList();
            foreach (EPSymbol sym in m_equation)
            {
               if ((sym.m_type == EPType.Variable) && (!var.Contains(sym)))
                  var.Add(sym);
            }
            return var;
         }
         set
         {
            foreach (EPSymbol sym in value)
            {
               for (int i = 0; i < m_postfix.Count; i++)
               {
                  if ((sym.m_name == ((EPSymbol)m_postfix[i]).m_name) && (((EPSymbol)m_postfix[i]).m_type == EPType.Variable))
                  {
                     EPSymbol sym1 = (EPSymbol)m_postfix[i];
                     sym1.m_value = sym.m_value;
                     m_postfix[i] = sym1;
                  }
               }
            }
         }
      }

      public EPFunction()
      { }

      public void Parse(string equation)
      {
         int state = 1;
         string temp = "";
         EPSymbol ctSymbol;

         m_bError = false;
         m_sErrorDescription = "None";

         m_equation.Clear();
         m_postfix.Clear();

         int nPos = 0;
         //-- Remove all white spaces from the equation string --
         s_equation = equation = equation.Trim();

         while ((nPos = equation.IndexOf(' ')) != -1)
            equation = equation.Remove(nPos, 1);

         for (int i = 0; i < equation.Length; i++)
         {
            switch (state)
            {
               case 1:
                  if (Char.IsNumber(equation[i]))
                  {
                     state = 2;
                     temp += equation[i];
                  }
                  else if (Char.IsLetter(equation[i]))
                  {
                     state = 3;
                     temp += equation[i];
                  }
                  else
                  {
                     ctSymbol.m_name = equation[i].ToString();
                     ctSymbol.m_value = 0;
                     switch (ctSymbol.m_name)
                     {
                        case ",":
                           ctSymbol.m_type = EPType.Comma;
                           break;
                        case "(":
                        case ")":
                        case "[":
                        case "]":
                        case "{":
                        case "}":
                           ctSymbol.m_type = EPType.Bracket;
                           break;
                        default:
                           ctSymbol.m_type = EPType.Operator;
                           break;
                     }
                     m_equation.Add(ctSymbol);
                  }
                  break;
               case 2:
                  if ((Char.IsNumber(equation[i])) || (equation[i] == '.'))
                     temp += equation[i];
                  else if (!Char.IsLetter(equation[i]))
                  {
                     state = 1;
                     ctSymbol.m_name = temp;
                     ctSymbol.m_value = Double.Parse(temp);
                     ctSymbol.m_type = EPType.Value;
                     m_equation.Add(ctSymbol);
                     ctSymbol.m_name = equation[i].ToString();
                     ctSymbol.m_value = 0;
                     switch (ctSymbol.m_name)
                     {
                        case ",":
                           ctSymbol.m_type = EPType.Comma;
                           break;
                        case "(":
                        case ")":
                        case "[":
                        case "]":
                        case "{":
                        case "}":
                           ctSymbol.m_type = EPType.Bracket;
                           break;
                        default:
                           ctSymbol.m_type = EPType.Operator;
                           break;
                     }
                     m_equation.Add(ctSymbol);
                     temp = "";
                  }
                  break;
               case 3:
                  if (Char.IsLetterOrDigit(equation[i]))
                     temp += equation[i];
                  else
                  {
                     state = 1;
                     ctSymbol.m_name = temp;
                     ctSymbol.m_value = 0;
                     if (equation[i] == '(')
                        ctSymbol.m_type = EPType.Function;
                     else
                     {
                        if (ctSymbol.m_name == "pi")
                           ctSymbol.m_value = System.Math.PI;
                        else if (ctSymbol.m_name == "e")
                           ctSymbol.m_value = System.Math.E;
                        ctSymbol.m_type = EPType.Variable;
                     }
                     m_equation.Add(ctSymbol);
                     ctSymbol.m_name = equation[i].ToString();
                     ctSymbol.m_value = 0;
                     switch (ctSymbol.m_name)
                     {
                        case ",":
                           ctSymbol.m_type = EPType.Comma;
                           break;
                        case "(":
                        case ")":
                        case "[":
                        case "]":
                        case "{":
                        case "}":
                           ctSymbol.m_type = EPType.Bracket;
                           break;
                        default:
                           ctSymbol.m_type = EPType.Operator;
                           break;
                     }
                     m_equation.Add(ctSymbol);
                     temp = "";
                  }
                  break;
            }
         }
         if (temp != "")
         {
            ctSymbol.m_name = temp;
            if (state == 2)
            {
               ctSymbol.m_value = Double.Parse(temp);
               ctSymbol.m_type = EPType.Value;
            }
            else
            {
               if (ctSymbol.m_name == "pi")
                  ctSymbol.m_value = System.Math.PI;
               else if (ctSymbol.m_name == "e")
                  ctSymbol.m_value = System.Math.E;
               else
                  ctSymbol.m_value = 0;
               ctSymbol.m_type = EPType.Variable;
            }
            m_equation.Add(ctSymbol);
         }
      }

      public void Infix2Postfix()
      {
         EPSymbol tpSym;
         Stack tpStack = new Stack();
         foreach (EPSymbol sym in m_equation)
         {
            if ((sym.m_type == EPType.Value) || (sym.m_type == EPType.Variable))
               m_postfix.Add(sym);
            else if ((sym.m_name == "(") || (sym.m_name == "[") || (sym.m_name == "{"))
               tpStack.Push(sym);
            else if ((sym.m_name == ")") || (sym.m_name == "]") || (sym.m_name == "}"))
            {
               if (tpStack.Count > 0)
               {
                  tpSym = (EPSymbol)tpStack.Pop();
                  while ((tpSym.m_name != "(") && (tpSym.m_name != "[") && (tpSym.m_name != "{"))
                  {
                     m_postfix.Add(tpSym);
                     tpSym = (EPSymbol)tpStack.Pop();
                  }
               }
            }
            else
            {
               if (tpStack.Count > 0)
               {
                  tpSym = (EPSymbol)tpStack.Pop();
                  while ((tpStack.Count != 0) && ((tpSym.m_type == EPType.Operator) || (tpSym.m_type == EPType.Function) || (tpSym.m_type == EPType.Comma)) && (Precedence(tpSym) >= Precedence(sym)))
                  {
                     m_postfix.Add(tpSym);
                     tpSym = (EPSymbol)tpStack.Pop();
                  }
                  if (((tpSym.m_type == EPType.Operator) || (tpSym.m_type == EPType.Function) || (tpSym.m_type == EPType.Comma)) && (Precedence(tpSym) >= Precedence(sym)))
                     m_postfix.Add(tpSym);
                  else
                     tpStack.Push(tpSym);
               }
               tpStack.Push(sym);
            }
         }
         while (tpStack.Count > 0)
         {
            tpSym = (EPSymbol)tpStack.Pop();
            m_postfix.Add(tpSym);
         }
      }

      public void EvaluatePostfix()
      {
         EPSymbol tpSym1, tpSym2, tpResult;
         Stack tpStack = new Stack();
         ArrayList fnParam = new ArrayList();
         m_bError = false;
         foreach (EPSymbol sym in m_postfix)
         {
            if ((sym.m_type == EPType.Value) || (sym.m_type == EPType.Variable) || (sym.m_type == EPType.Result))
               tpStack.Push(sym);
            else if (sym.m_type == EPType.Operator)
            {
               tpSym1 = (EPSymbol)tpStack.Pop();
               tpSym2 = (EPSymbol)tpStack.Pop();
               tpResult = Evaluate(tpSym2, sym, tpSym1);
               if (tpResult.m_type == EPType.Error)
               {
                  m_bError = true;
                  m_sErrorDescription = tpResult.m_name;
                  return;
               }
               tpStack.Push(tpResult);
            }
            else if (sym.m_type == EPType.Function)
            {
               fnParam.Clear();
               tpSym1 = (EPSymbol)tpStack.Pop();
               if ((tpSym1.m_type == EPType.Value) || (tpSym1.m_type == EPType.Variable) || (tpSym1.m_type == EPType.Result))
               {
                  tpResult = EvaluateFunction(sym.m_name, tpSym1);
                  if (tpResult.m_type == EPType.Error)
                  {
                     m_bError = true;
                     m_sErrorDescription = tpResult.m_name;
                     return;
                  }
                  tpStack.Push(tpResult);
               }
               else if (tpSym1.m_type == EPType.Comma)
               {
                  while (tpSym1.m_type == EPType.Comma)
                  {
                     tpSym1 = (EPSymbol)tpStack.Pop();
                     fnParam.Add(tpSym1);
                     tpSym1 = (EPSymbol)tpStack.Pop();
                  }
                  fnParam.Add(tpSym1);
                  tpResult = EvaluateFunction(sym.m_name, fnParam.ToArray());
                  if (tpResult.m_type == EPType.Error)
                  {
                     m_bError = true;
                     m_sErrorDescription = tpResult.m_name;
                     return;
                  }
                  tpStack.Push(tpResult);
               }
               else
               {
                  tpStack.Push(tpSym1);
                  tpResult = EvaluateFunction(sym.m_name);
                  if (tpResult.m_type == EPType.Error)
                  {
                     m_bError = true;
                     m_sErrorDescription = tpResult.m_name;
                     return;
                  }
                  tpStack.Push(tpResult);
               }
            }
         }
         if (tpStack.Count == 1)
         {
            tpResult = (EPSymbol)tpStack.Pop();
            m_result = tpResult.m_value;
         }
      }

      protected int Precedence(EPSymbol sym)
      {
         switch (sym.m_type)
         {
            case EPType.Bracket:
               return 5;
            case EPType.Function:
               return 4;
            case EPType.Comma:
               return 0;
         }
         switch (sym.m_name)
         {
            case "^":
               return 3;
            case "/":
            case "*":
            case "%":
               return 2;
            case "+":
            case "-":
               return 1;
         }
         return -1;
      }

      protected EPSymbol Evaluate(EPSymbol sym1, EPSymbol opr, EPSymbol sym2)
      {
         EPSymbol result;
         result.m_name = sym1.m_name + opr.m_name + sym2.m_name;
         result.m_type = EPType.Result;
         result.m_value = 0;
         switch (opr.m_name)
         {
            case "^":
               result.m_value = System.Math.Pow(sym1.m_value, sym2.m_value);
               break;
            case "/":
               {
                  if (sym2.m_value > 0)
                     result.m_value = sym1.m_value / sym2.m_value;
                  else
                  {
                     result.m_name = "Divide by Zero.";
                     result.m_type = EPType.Error;
                  }
                  break;
               }
            case "*":
               result.m_value = sym1.m_value * sym2.m_value;
               break;
            case "%":
               result.m_value = sym1.m_value % sym2.m_value;
               break;
            case "+":
               result.m_value = sym1.m_value + sym2.m_value;
               break;
            case "-":
               result.m_value = sym1.m_value - sym2.m_value;
               break;
            default:
               result.m_type = EPType.Error;
               result.m_name = "Undefine operator: " + opr.m_name + ".";
               break;
         }
         return result;
      }

      protected EPSymbol EvaluateFunction(string name, params Object[] args)
      {
         EPSymbol result;
         result.m_name = "";
         result.m_type = EPType.Result;
         result.m_value = 0;
         switch (name)
         {
            case "cos":
               if (args.Length == 1)
               {
                  result.m_name = name + "(" + ((EPSymbol)args[0]).m_value.ToString() + ")";
                  result.m_value = System.Math.Cos(((EPSymbol)args[0]).m_value);
               }
               else
               {
                  result.m_name = "Invalid number of parameters in: " + name + ".";
                  result.m_type = EPType.Error;
               }
               break;
            case "sin":
               if (args.Length == 1)
               {
                  result.m_name = name + "(" + ((EPSymbol)args[0]).m_value.ToString() + ")";
                  result.m_value = System.Math.Sin(((EPSymbol)args[0]).m_value);
               }
               else
               {
                  result.m_name = "Invalid number of parameters in: " + name + ".";
                  result.m_type = EPType.Error;
               }
               break;
            case "tan":
               if (args.Length == 1)
               {
                  result.m_name = name + "(" + ((EPSymbol)args[0]).m_value.ToString() + ")";
                  result.m_value = System.Math.Tan(((EPSymbol)args[0]).m_value);
               }
               else
               {
                  result.m_name = "Invalid number of parameters in: " + name + ".";
                  result.m_type = EPType.Error;
               }
               break;
            case "cosh":
               if (args.Length == 1)
               {
                  result.m_name = name + "(" + ((EPSymbol)args[0]).m_value.ToString() + ")";
                  result.m_value = System.Math.Cosh(((EPSymbol)args[0]).m_value);
               }
               else
               {
                  result.m_name = "Invalid number of parameters in: " + name + ".";
                  result.m_type = EPType.Error;
               }
               break;
            case "sinh":
               if (args.Length == 1)
               {
                  result.m_name = name + "(" + ((EPSymbol)args[0]).m_value.ToString() + ")";
                  result.m_value = System.Math.Sinh(((EPSymbol)args[0]).m_value);
               }
               else
               {
                  result.m_name = "Invalid number of parameters in: " + name + ".";
                  result.m_type = EPType.Error;
               }
               break;
            case "tanh":
               if (args.Length == 1)
               {
                  result.m_name = name + "(" + ((EPSymbol)args[0]).m_value.ToString() + ")";
                  result.m_value = System.Math.Tanh(((EPSymbol)args[0]).m_value);
               }
               else
               {
                  result.m_name = "Invalid number of parameters in: " + name + ".";
                  result.m_type = EPType.Error;
               }
               break;
            case "log":
               if (args.Length == 1)
               {
                  result.m_name = name + "(" + ((EPSymbol)args[0]).m_value.ToString() + ")";
                  result.m_value = System.Math.Log10(((EPSymbol)args[0]).m_value);
               }
               else
               {
                  result.m_name = "Invalid number of parameters in: " + name + ".";
                  result.m_type = EPType.Error;
               }
               break;
            case "ln":
               if (args.Length == 1)
               {
                  result.m_name = name + "(" + ((EPSymbol)args[0]).m_value.ToString() + ")";
                  result.m_value = System.Math.Log(((EPSymbol)args[0]).m_value, 2);
               }
               else
               {
                  result.m_name = "Invalid number of parameters in: " + name + ".";
                  result.m_type = EPType.Error;
               }
               break;
            case "logn":
               if (args.Length == 2)
               {
                  result.m_name = name + "(" + ((EPSymbol)args[0]).m_value.ToString() + "'" + ((EPSymbol)args[1]).m_value.ToString() + ")";
                  result.m_value = System.Math.Log(((EPSymbol)args[0]).m_value, ((EPSymbol)args[1]).m_value);
               }
               else
               {
                  result.m_name = "Invalid number of parameters in: " + name + ".";
                  result.m_type = EPType.Error;
               }
               break;
            case "sqrt":
               if (args.Length == 1)
               {
                  result.m_name = name + "(" + ((EPSymbol)args[0]).m_value.ToString() + ")";
                  result.m_value = System.Math.Sqrt(((EPSymbol)args[0]).m_value);
               }
               else
               {
                  result.m_name = "Invalid number of parameters in: " + name + ".";
                  result.m_type = EPType.Error;
               }
               break;
            case "abs":
               if (args.Length == 1)
               {
                  result.m_name = name + "(" + ((EPSymbol)args[0]).m_value.ToString() + ")";
                  result.m_value = System.Math.Abs(((EPSymbol)args[0]).m_value);
               }
               else
               {
                  result.m_name = "Invalid number of parameters in: " + name + ".";
                  result.m_type = EPType.Error;
               }
               break;
            case "acos":
               if (args.Length == 1)
               {
                  result.m_name = name + "(" + ((EPSymbol)args[0]).m_value.ToString() + ")";
                  result.m_value = System.Math.Acos(((EPSymbol)args[0]).m_value);
               }
               else
               {
                  result.m_name = "Invalid number of parameters in: " + name + ".";
                  result.m_type = EPType.Error;
               }
               break;
            case "asin":
               if (args.Length == 1)
               {
                  result.m_name = name + "(" + ((EPSymbol)args[0]).m_value.ToString() + ")";
                  result.m_value = System.Math.Asin(((EPSymbol)args[0]).m_value);
               }
               else
               {
                  result.m_name = "Invalid number of parameters in: " + name + ".";
                  result.m_type = EPType.Error;
               }
               break;
            case "atan":
               if (args.Length == 1)
               {
                  result.m_name = name + "(" + ((EPSymbol)args[0]).m_value.ToString() + ")";
                  result.m_value = System.Math.Atan(((EPSymbol)args[0]).m_value);
               }
               else
               {
                  result.m_name = "Invalid number of parameters in: " + name + ".";
                  result.m_type = EPType.Error;
               }
               break;
            case "exp":
               if (args.Length == 1)
               {
                  result.m_name = name + "(" + ((EPSymbol)args[0]).m_value.ToString() + ")";
                  result.m_value = System.Math.Exp(((EPSymbol)args[0]).m_value);
               }
               else
               {
                  result.m_name = "Invalid number of parameters in: " + name + ".";
                  result.m_type = EPType.Error;
               }
               break;
            default:
               if (m_defaultFunctionEvaluation != null)
                  result = m_defaultFunctionEvaluation(name, args);
               else
               {
                  result.m_name = "Function: " + name + ", not found.";
                  result.m_type = EPType.Error;
               }
               break;
         }
         return result;
      }
      public string s_equation = "";
      protected bool m_bError = false;
      protected string m_sErrorDescription = "None";
      protected double m_result = 0;
      protected ArrayList m_equation = new ArrayList();
      protected ArrayList m_postfix = new ArrayList();
      protected EvaluateFunctionDelegate m_defaultFunctionEvaluation;
   }
}
