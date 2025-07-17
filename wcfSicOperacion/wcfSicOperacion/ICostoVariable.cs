// Decompiled with JetBrains decompiler
// Type: wcfSicOperacion.ICostoVariable
// Assembly: wcfSicOperacion, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FB6DF14C-22DB-4213-A671-4E83B266BC5E
// Assembly location: C:\d\wsSICOES\wcfSicOperacion.dll

using System;
using System.Data;
using System.ServiceModel;

namespace wcfSicOperacion
{
  [ServiceContract]
  public interface ICostoVariable
  {
    [OperationContract]
    DataTable ObtenerCostosVariables(DateTime af_rep);

    [OperationContract]
    DataTable ObtenerHistorico(int ai_grupo_cod);

    [OperationContract]
    DataSet ObtenerHistoricoDesc(int ai_grupo_cod);
  }
}
