// Decompiled with JetBrains decompiler
// Type: COES.Storage.App.Servicio.Helper
// Assembly: COES.Storage.App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 08842ED7-FE70-4C24-AA93-4783F0BAE549
// Assembly location: C:\Users\jose.delgado\source\repos\ENPRUEBAS\MASTER\COES.MVC.Publico\bin\COES.Storage.App.dll

namespace COES.Storage.App.Servicio
{
  public class Helper
  {
    public const string Activo = "A";

    public static string ObtenerTipoCelda(int tipoDato)
    {
      if (tipoDato == 3 || tipoDato == 4 || tipoDato == 7)
        return "text";
      if (tipoDato == 1 || tipoDato == 2)
        return "numeric";
      return tipoDato == 5 ? "date" : "dropdown";
    }

    public static int ObtenerLibreriaPorFuente(int idFuente)
    {
      int num = 0;
      switch (idFuente)
      {
        case 1:
          num = 1;
          break;
        case 3:
          num = 74;
          break;
        case 4:
          num = 78;
          break;
      }
      return num;
    }
  }
}
