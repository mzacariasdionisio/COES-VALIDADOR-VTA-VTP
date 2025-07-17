using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Scada;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class SubCausaEventoRepository: RepositoryBase
    {
        public SubCausaEventoRepository(string strConn)
            : base(strConn)
        {

        }

        SubCausaEventoHelper helper = new SubCausaEventoHelper();               

        public List<SubCausaEventoDTO> ObtenerCausaEvento(int? idTipoEvento)
        {
            List<SubCausaEventoDTO> entitys = new List<SubCausaEventoDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(this.helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.TIPOEVENCODI, DbType.Int32, idTipoEvento);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(this.helper.Create(dr));
                }
            }

            return entitys;
        }
    }
}
