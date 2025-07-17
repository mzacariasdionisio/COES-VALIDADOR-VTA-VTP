// Decompiled with JetBrains decompiler
// Type: COES.Storage.App.Servicio.FactoryFile
// Assembly: COES.Storage.App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 08842ED7-FE70-4C24-AA93-4783F0BAE549
// Assembly location: C:\Users\jose.delgado\source\repos\ENPRUEBAS\MASTER\COES.MVC.Publico\bin\COES.Storage.App.dll

using COES.Storage.App.FileManager;

namespace COES.Storage.App.Servicio
{
  public class FactoryFile
  {
    public static string StrFuente = "Local";

    public static IFileManager GetFileManager() => FactoryFile.StrFuente == "Local" ? (IFileManager) new COES.Storage.App.FileServer.FileServer() : (IFileManager) null;
  }
}
