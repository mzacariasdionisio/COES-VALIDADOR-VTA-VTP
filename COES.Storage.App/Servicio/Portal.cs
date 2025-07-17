// Decompiled with JetBrains decompiler
// Type: COES.Storage.App.Servicio.Portal
// Assembly: COES.Storage.App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 08842ED7-FE70-4C24-AA93-4783F0BAE549
// Assembly location: C:\Users\jose.delgado\source\repos\ENPRUEBAS\MASTER\COES.MVC.Publico\bin\COES.Storage.App.dll

using COES.Storage.App.Metadata.Entidad;
using System;
using System.Collections.Generic;
using System.Linq;

namespace COES.Storage.App.Servicio
{
  public class Portal
  {
    public int SaveWbBanner(WbBannerDTO entity)
    {
      try
      {
        int num;
        if (entity.Banncodi == 0)
        {
          num = FactoryDb.GetWbBannerRepository().Save(entity);
        }
        else
        {
          FactoryDb.GetWbBannerRepository().Update(entity);
          num = entity.Banncodi;
        }
        return num;
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex);
      }
    }

    public void DeleteWbBanner(int banncodi)
    {
      try
      {
        FactoryDb.GetWbBannerRepository().Delete(banncodi);
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex);
      }
    }

    public void ActualizarOrden(List<WbBannerDTO> list)
    {
      try
      {
        foreach (WbBannerDTO entity in list)
          FactoryDb.GetWbBannerRepository().ActualizarOrden(entity);
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex);
      }
    }

    public WbBannerDTO GetByIdWbBanner(int banncodi) => FactoryDb.GetWbBannerRepository().GetById(banncodi);

    public List<WbBannerDTO> ListWbBanners() => FactoryDb.GetWbBannerRepository().List();

    public List<WbBannerDTO> ObtenerBannerPortal() => FactoryDb.GetWbBannerRepository().GetByCriteria();

    public int GrabarOpcion(WbMenuDTO entity)
    {
      try
      {
        int num;
        if (entity.Menucodi == 0)
        {
          num = FactoryDb.GetWbMenuRepository().Save(entity);
        }
        else
        {
          FactoryDb.GetWbMenuRepository().Update(entity);
          num = entity.Menucodi;
        }
        return num;
      }
      catch (Exception ex)
      {
        return -1;
      }
    }

    public void EliminarOpcion(int id)
    {
      try
      {
        FactoryDb.GetWbMenuRepository().Delete(id);
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex);
      }
    }

    public WbMenuDTO ObtenerOpcion(int id) => FactoryDb.GetWbMenuRepository().GetById(id);

    public List<WbMenuDTO> ListarNodosPortal() => FactoryDb.GetWbMenuRepository().GetByCriteria();

    public List<WbMenuDTO> ObtenerMenuPortal() => FactoryDb.GetWbMenuRepository().GetByCriteria().Where<WbMenuDTO>((Func<WbMenuDTO, bool>) (x => x.Menuestado == "A")).ToList<WbMenuDTO>();

    public void ActualizarNodoOpcion(int opcionId, int padreId, int nroOrden)
    {
      try
      {
        FactoryDb.GetWbMenuRepository().ActualizarNodoOpcion(opcionId, padreId, nroOrden);
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex);
      }
    }

    public int ObtenerNroItemPorPadre(int idPadre) => FactoryDb.GetWbMenuRepository().ObtenerNroItemPorPadre(idPadre);
  }
}
