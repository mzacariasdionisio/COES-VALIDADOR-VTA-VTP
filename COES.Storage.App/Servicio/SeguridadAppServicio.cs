// Decompiled with JetBrains decompiler
// Type: COES.Storage.App.Servicio.SeguridadAppServicio
// Assembly: COES.Storage.App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 08842ED7-FE70-4C24-AA93-4783F0BAE549
// Assembly location: C:\Users\jose.delgado\source\repos\ENPRUEBAS\MASTER\COES.MVC.Publico\bin\COES.Storage.App.dll

using COES.Storage.App.Metadata.Entidad;
using System;
using System.Collections.Generic;
using System.Linq;

namespace COES.Storage.App.Servicio
{
  public class SeguridadAppServicio
  {
    public List<WbBlobDTO> ListaBlobs = (List<WbBlobDTO>) null;

    public List<WbGrupoDTO> ListarGrupos() => FactoryDb.GetWbGrupoRepository().List();

    public WbGrupoDTO ObtenerGrupo(int grupoCodi) => FactoryDb.GetWbGrupoRepository().GetById(grupoCodi);

    public int GrabarGrupo(WbGrupoDTO entity, List<int> blobs)
    {
      try
      {
        int grupocodi;
        if (entity.Grupocodi == 0)
        {
          grupocodi = FactoryDb.GetWbGrupoRepository().Save(entity);
        }
        else
        {
          FactoryDb.GetWbGrupoRepository().Update(entity);
          grupocodi = entity.Grupocodi;
          FactoryDb.GetWbGrupousuarioRepository().Delete(grupocodi);
          FactoryDb.GetWbGrupoblobRepository().Delete(grupocodi);
        }
        foreach (FwUserDTO listaUsuario in entity.ListaUsuarios)
          FactoryDb.GetWbGrupousuarioRepository().Save(new WbGrupousuarioDTO()
          {
            Grupocodi = grupocodi,
            Usercode = listaUsuario.Usercode
          });
        foreach (int blob in blobs)
          FactoryDb.GetWbGrupoblobRepository().Save(new WbGrupoblobDTO()
          {
            Blobcodi = blob,
            Grupocodi = grupocodi
          });
        return grupocodi;
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex);
      }
    }

    public void EliminarGrupo(int grupoCodi)
    {
      try
      {
        FactoryDb.GetWbGrupoRepository().Delete(grupoCodi);
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex);
      }
    }

    public List<FwUserDTO> ListarUsuarios() => FactoryDb.GetFwUserRepository().List();

    public List<FwUserDTO> ObtenerUsuariosPorGrupo(int grupoCodi) => FactoryDb.GetFwUserRepository().GetByCriteria(grupoCodi);

    public FwUserDTO OtenerUsuario(int userCode) => FactoryDb.GetFwUserRepository().GetById(userCode);

    public List<WbBlobDTO> ObtenerCarpetasUsuario(int userCode)
    {
      this.ListaBlobs = new List<WbBlobDTO>();
      List<WbBlobDTO> source = FactoryDb.GetWbBlobRepository().ObtenerCarpetasUsuario(userCode);
      this.ListaBlobs.AddRange((IEnumerable<WbBlobDTO>) source);
      List<int> list1 = source.Where<WbBlobDTO>((Func<WbBlobDTO, bool>) (x =>
      {
        int? padrecodi = x.Padrecodi;
        int num = 1;
        return !(padrecodi.GetValueOrDefault() == num & padrecodi.HasValue);
      })).Select<WbBlobDTO, int>((Func<WbBlobDTO, int>) (x => x.Padrecodi.Value)).Distinct<int>().ToList<int>();
      if (list1.Count > 0)
      {
        List<WbBlobDTO> list2 = FactoryDb.GetWbBlobRepository().ObtenerBlobs(string.Join<int>(",", (IEnumerable<int>) list1));
        foreach (WbBlobDTO wbBlobDto in list2)
        {
          WbBlobDTO item = wbBlobDto;
          if (this.ListaBlobs.Where<WbBlobDTO>((Func<WbBlobDTO, bool>) (x => x.Blobcodi == item.Blobcodi)).Count<WbBlobDTO>() == 0)
            this.ListaBlobs.Add(item);
        }
        if (list2.Count > 0)
          this.ObtenerEstructuraFolder(list2);
      }
      return this.ListaBlobs.Distinct<WbBlobDTO>().ToList<WbBlobDTO>();
    }

    public List<WbBlobDTO> ObtenerCarpetasPrincipales(int grupoCodi)
    {
      this.ListaBlobs = new List<WbBlobDTO>();
      List<WbBlobDTO> source = FactoryDb.GetWbBlobRepository().ObtenerFoldersPrincipales();
      this.ListaBlobs.AddRange((IEnumerable<WbBlobDTO>) source);
      List<int> list1 = source.Where<WbBlobDTO>((Func<WbBlobDTO, bool>) (x =>
      {
        int? padrecodi = x.Padrecodi;
        int num = 1;
        return !(padrecodi.GetValueOrDefault() == num & padrecodi.HasValue);
      })).Select<WbBlobDTO, int>((Func<WbBlobDTO, int>) (x => x.Padrecodi.Value)).Distinct<int>().ToList<int>();
      if (list1.Count > 0)
      {
        List<WbBlobDTO> list2 = FactoryDb.GetWbBlobRepository().ObtenerBlobs(string.Join<int>(",", (IEnumerable<int>) list1));
        foreach (WbBlobDTO wbBlobDto in list2)
        {
          WbBlobDTO item = wbBlobDto;
          if (this.ListaBlobs.Where<WbBlobDTO>((Func<WbBlobDTO, bool>) (x => x.Blobcodi == item.Blobcodi)).Count<WbBlobDTO>() == 0)
            this.ListaBlobs.Add(item);
        }
        if (list2.Count > 0)
          this.ObtenerEstructuraFolder(list2);
      }
      List<WbGrupoblobDTO> byCriteria = FactoryDb.GetWbGrupoblobRepository().GetByCriteria(grupoCodi);
      foreach (WbBlobDTO listaBlob in this.ListaBlobs)
      {
        WbBlobDTO entity = listaBlob;
        if (byCriteria.Where<WbGrupoblobDTO>((Func<WbGrupoblobDTO, bool>) (x => x.Blobcodi == entity.Blobcodi)).Count<WbGrupoblobDTO>() > 0)
          entity.IndSeleccion = true;
      }
      return this.ListaBlobs.Distinct<WbBlobDTO>().ToList<WbBlobDTO>();
    }

    private void ObtenerEstructuraFolder(List<WbBlobDTO> list)
    {
      List<int> list1 = list.Where<WbBlobDTO>((Func<WbBlobDTO, bool>) (x =>
      {
        int? padrecodi = x.Padrecodi;
        int num = 1;
        return !(padrecodi.GetValueOrDefault() == num & padrecodi.HasValue);
      })).Select<WbBlobDTO, int>((Func<WbBlobDTO, int>) (x => x.Padrecodi.Value)).Distinct<int>().ToList<int>();
      if (list1.Count <= 0)
        return;
      List<WbBlobDTO> list2 = FactoryDb.GetWbBlobRepository().ObtenerBlobs(string.Join<int>(",", (IEnumerable<int>) list1));
      foreach (WbBlobDTO wbBlobDto in list2)
      {
        WbBlobDTO item = wbBlobDto;
        if (this.ListaBlobs.Where<WbBlobDTO>((Func<WbBlobDTO, bool>) (x => x.Blobcodi == item.Blobcodi)).Count<WbBlobDTO>() == 0)
          this.ListaBlobs.Add(item);
      }
      if (list2.Count > 0)
        this.ObtenerEstructuraFolder(list2);
    }
  }
}
