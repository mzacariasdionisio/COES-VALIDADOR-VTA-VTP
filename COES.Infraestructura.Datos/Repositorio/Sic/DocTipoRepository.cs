using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla DOC_TIPO
    /// </summary>
    public class DocTipoRepository: RepositoryBase, IDocTipoRepository
    {
        public DocTipoRepository(string strConn): base(strConn)
        {
        }

        DocTipoHelper helper = new DocTipoHelper();

         public List<DocTipoDTO> List()
        {
            List<DocTipoDTO> entitys = new List<DocTipoDTO>();
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

        public List<DocTipoDTO> GetByCriteria()
        {
            List<DocTipoDTO> entitys = new List<DocTipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public DocTipoDTO GetById(int tipoDoc)
        {
            DocTipoDTO entity = new DocTipoDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);
            dbProvider.AddInParameter(command, helper.Tipcodi, DbType.Int32, tipoDoc);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }
    }
}
