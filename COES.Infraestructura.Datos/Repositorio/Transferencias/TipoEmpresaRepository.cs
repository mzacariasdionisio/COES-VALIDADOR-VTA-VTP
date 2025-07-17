using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase que contiene las operaciones con la base de datos
    /// </summary>
   public  class TipoEmpresaRepository: RepositoryBase, ITipoEmpresaRepository
    {
         public TipoEmpresaRepository(string strConn)
            : base(strConn)
        {
        }

         TipoEmpresaHelper helper = new TipoEmpresaHelper();

         public List<TipoEmpresaDTO> List()
         {
             List<TipoEmpresaDTO> entitys = new List<TipoEmpresaDTO>();
             DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

             using (IDataReader dr = dbProvider.ExecuteReader(command))
             {
                 while (dr.Read())
                 {
                     entitys.Add(helper.Create(dr));
                 }
             }

             return entitys;
         }
    }
}
