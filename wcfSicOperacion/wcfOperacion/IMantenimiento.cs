// Decompiled with JetBrains decompiler
// Type: wcfOperacion.IMantenimiento
// Assembly: wcfSicOperacion, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FB6DF14C-22DB-4213-A671-4E83B266BC5E
// Assembly location: C:\d\wsSICOES\wcfSicOperacion.dll

using System.ServiceModel;

namespace wcfOperacion
{
  [ServiceContract]
  public interface IMantenimiento
  {
    [OperationContract]
    string GetData(int value);

    [OperationContract]
    CompositeType GetDataUsingDataContract(CompositeType composite);
  }
}
