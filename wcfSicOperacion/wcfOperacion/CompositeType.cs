// Decompiled with JetBrains decompiler
// Type: wcfOperacion.CompositeType
// Assembly: wcfSicOperacion, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FB6DF14C-22DB-4213-A671-4E83B266BC5E
// Assembly location: C:\d\wsSICOES\wcfSicOperacion.dll

using System.Runtime.Serialization;

namespace wcfOperacion
{
  [DataContract]
  public class CompositeType
  {
    private bool boolValue = true;
    private string stringValue = "Hello ";

    [DataMember]
    public bool BoolValue
    {
      get => this.boolValue;
      set => this.boolValue = value;
    }

    [DataMember]
    public string StringValue
    {
      get => this.stringValue;
      set => this.stringValue = value;
    }
  }
}
