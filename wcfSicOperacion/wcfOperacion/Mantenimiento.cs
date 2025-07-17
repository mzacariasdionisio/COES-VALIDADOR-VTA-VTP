// Decompiled with JetBrains decompiler
// Type: wcfOperacion.Mantenimiento
// Assembly: wcfSicOperacion, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FB6DF14C-22DB-4213-A671-4E83B266BC5E
// Assembly location: C:\d\wsSICOES\wcfSicOperacion.dll

using System;

namespace wcfOperacion
{
  public class Mantenimiento : IMantenimiento
  {
    public string GetData(int value) => string.Format("You entered: {0}", (object) value);

    public CompositeType GetDataUsingDataContract(CompositeType composite)
    {
      if (composite == null)
        throw new ArgumentNullException(nameof (composite));
      if (composite.BoolValue)
        composite.StringValue += "Suffix";
      return composite;
    }
  }
}
