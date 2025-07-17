// Decompiled with JetBrains decompiler
// Type: COES.Storage.App.Servicio.Tools
// Assembly: COES.Storage.App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 08842ED7-FE70-4C24-AA93-4783F0BAE549
// Assembly location: C:\Users\jose.delgado\source\repos\ENPRUEBAS\MASTER\COES.MVC.Publico\bin\COES.Storage.App.dll

namespace COES.Storage.App.Servicio
{
  public class Tools
  {
    public static string ObtenerFileName(string fileName)
    {
      if (!string.IsNullOrEmpty(fileName))
      {
        int num = fileName.LastIndexOf('/');
        if (num > 0)
          return fileName.Substring(num + 1, fileName.Length - num - 1);
      }
      return fileName;
    }

    public static string ObtenerExtension(string fileName)
    {
      if (string.IsNullOrEmpty(fileName))
        return string.Empty;
      int startIndex = fileName.LastIndexOf('.');
      return fileName.Substring(startIndex, fileName.Length - startIndex);
    }

    public static string ObtenerIcono(string tipo, string extension)
    {
      string empty = string.Empty;
      string str;
      if (tipo == "F")
      {
        switch (extension.ToLower())
        {
          case ".csv":
            str = "csvicon.png";
            break;
          case ".doc":
            str = "docicon.png";
            break;
          case ".docx":
            str = "docicon.png";
            break;
          case ".gif":
            str = "imgicon.gif";
            break;
          case ".jpg":
            str = "imgicon.gif";
            break;
          case ".pdf":
            str = "pdficon.png";
            break;
          case ".png":
            str = "imgicon.gif";
            break;
          case ".ppt":
            str = "ppticon.png";
            break;
          case ".pptx":
            str = "ppticon.png";
            break;
          case ".rar":
            str = "zipicon.gif";
            break;
          case ".xls":
            str = "xlsicon.png";
            break;
          case ".xlsx":
            str = "xlsicon.png";
            break;
          case ".zip":
            str = "zipicon.gif";
            break;
          default:
            str = "defaulticon.png";
            break;
        }
      }
      else
        str = "foldericon.gif";
      return str;
    }
  }
}
