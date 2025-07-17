// Decompiled with JetBrains decompiler
// Type: wcfSicOperacion.Validador
// Assembly: wcfSicOperacion, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FB6DF14C-22DB-4213-A671-4E83B266BC5E
// Assembly location: C:\d\wsSICOES\wcfSicOperacion.dll

using Seguridad.Autenticacion;

namespace wcfSicOperacion
{
  internal class Validador
  {
    public static bool nf_valida_credencial(string as_credencial) => new CredencialValidador().nf_valida_credencial_enc(as_credencial);
  }
}
