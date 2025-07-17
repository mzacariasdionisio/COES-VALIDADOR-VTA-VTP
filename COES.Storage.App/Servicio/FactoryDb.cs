// Decompiled with JetBrains decompiler
// Type: COES.Storage.App.Servicio.FactoryDb
// Assembly: COES.Storage.App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 08842ED7-FE70-4C24-AA93-4783F0BAE549
// Assembly location: C:\Users\jose.delgado\source\repos\ENPRUEBAS\MASTER\COES.MVC.Publico\bin\COES.Storage.App.dll

using COES.Storage.App.Metadata.Data;

namespace COES.Storage.App.Servicio
{
  public class FactoryDb
  {
    public static string StrConexion = "ContextoSIC";

    public static WbBlobRepository GetWbBlobRepository() => new WbBlobRepository(FactoryDb.StrConexion);

    public static WbBlobcolumnRepository GetWbBlobcolumnRepository() => new WbBlobcolumnRepository(FactoryDb.StrConexion);

    public static WbBlobconfigRepository GetWbBlobconfigRepository() => new WbBlobconfigRepository(FactoryDb.StrConexion);

    public static WbBlobmetadataRepository GetWbBlobmetadataRepository() => new WbBlobmetadataRepository(FactoryDb.StrConexion);

    public static WbColumntypeRepository GetWbColumntypeRepository() => new WbColumntypeRepository(FactoryDb.StrConexion);

    public static WbColumnitemRepository GetWbColumnitemRepository() => new WbColumnitemRepository(FactoryDb.StrConexion);

    public static WbConfigcolumnRepository GetWbConfigcolumnRepository() => new WbConfigcolumnRepository(FactoryDb.StrConexion);

    public static WbGrupoRepository GetWbGrupoRepository() => new WbGrupoRepository(FactoryDb.StrConexion);

    public static WbGrupoblobRepository GetWbGrupoblobRepository() => new WbGrupoblobRepository(FactoryDb.StrConexion);

    public static WbGrupousuarioRepository GetWbGrupousuarioRepository() => new WbGrupousuarioRepository(FactoryDb.StrConexion);

    public static FwUserRepository GetFwUserRepository() => new FwUserRepository(FactoryDb.StrConexion);

    public static WbBannerRepository GetWbBannerRepository() => new WbBannerRepository(FactoryDb.StrConexion);

    public static WbMenuRepository GetWbMenuRepository() => new WbMenuRepository(FactoryDb.StrConexion);
  }
}
