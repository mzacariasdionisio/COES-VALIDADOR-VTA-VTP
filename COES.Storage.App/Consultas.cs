// Decompiled with JetBrains decompiler
// Type: COES.Storage.App.Consultas
// Assembly: COES.Storage.App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 08842ED7-FE70-4C24-AA93-4783F0BAE549
// Assembly location: C:\Users\jose.delgado\source\repos\ENPRUEBAS\MASTER\COES.MVC.Publico\bin\COES.Storage.App.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace COES.Storage.App
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class Consultas
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Consultas()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (Consultas.resourceMan == null)
          Consultas.resourceMan = new ResourceManager("COES.Storage.App.Consultas", typeof (Consultas).Assembly);
        return Consultas.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => Consultas.resourceCulture;
      set => Consultas.resourceCulture = value;
    }

    internal static string FwUserSql => Consultas.ResourceManager.GetString(nameof (FwUserSql), Consultas.resourceCulture);

    internal static string WbBannerSql => Consultas.ResourceManager.GetString(nameof (WbBannerSql), Consultas.resourceCulture);

    internal static string WbBlobcolumnSql => Consultas.ResourceManager.GetString(nameof (WbBlobcolumnSql), Consultas.resourceCulture);

    internal static string WbBlobconfigSql => Consultas.ResourceManager.GetString(nameof (WbBlobconfigSql), Consultas.resourceCulture);

    internal static string WbBlobmetadataSql => Consultas.ResourceManager.GetString(nameof (WbBlobmetadataSql), Consultas.resourceCulture);

    internal static string WbBlobSql => Consultas.ResourceManager.GetString(nameof (WbBlobSql), Consultas.resourceCulture);

    internal static string WbColumnitemSql => Consultas.ResourceManager.GetString(nameof (WbColumnitemSql), Consultas.resourceCulture);

    internal static string WbColumntypeSql => Consultas.ResourceManager.GetString(nameof (WbColumntypeSql), Consultas.resourceCulture);

    internal static string WbConfigcolumnSql => Consultas.ResourceManager.GetString(nameof (WbConfigcolumnSql), Consultas.resourceCulture);

    internal static string WbGrupoblobSql => Consultas.ResourceManager.GetString(nameof (WbGrupoblobSql), Consultas.resourceCulture);

    internal static string WbGrupoSql => Consultas.ResourceManager.GetString(nameof (WbGrupoSql), Consultas.resourceCulture);

    internal static string WbGrupousuarioSql => Consultas.ResourceManager.GetString(nameof (WbGrupousuarioSql), Consultas.resourceCulture);

    internal static string WbMenuSql => Consultas.ResourceManager.GetString(nameof (WbMenuSql), Consultas.resourceCulture);
  }
}
