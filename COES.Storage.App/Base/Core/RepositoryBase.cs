// Decompiled with JetBrains decompiler
// Type: COES.Storage.App.Base.Core.RepositoryBase
// Assembly: COES.Storage.App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 08842ED7-FE70-4C24-AA93-4783F0BAE549
// Assembly location: C:\Users\jose.delgado\source\repos\ENPRUEBAS\MASTER\COES.MVC.Publico\bin\COES.Storage.App.dll

using COES.Storage.App.Base.Datahelper;

namespace COES.Storage.App.Base.Core
{
  public abstract class RepositoryBase
  {
    public DbProvider dbProvider;

    public RepositoryBase(string strConn) => this.dbProvider = new DbProvider(strConn);

    public RepositoryBase()
    {
    }
  }
}
