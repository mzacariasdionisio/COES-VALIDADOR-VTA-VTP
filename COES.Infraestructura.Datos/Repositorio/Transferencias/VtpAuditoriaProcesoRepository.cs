using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    public class VtpAuditoriaProcesoRepository : RepositoryBase, IVtpAuditoriaProcesoRepository
    {
        public VtpAuditoriaProcesoRepository(string strConn)
          : base(strConn)
        {
        }

        VtpAuditoriaProcesoHelper helper = new VtpAuditoriaProcesoHelper();

        public int Save(VtpAuditoriaProcesoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);
         	
            dbProvider.AddInParameter(command, helper.Audprocodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Tipprocodi, DbType.Int32, entity.Tipprocodi);
            dbProvider.AddInParameter(command, helper.Estdcodi, DbType.Int32, entity.Estdcodi);
            dbProvider.AddInParameter(command, helper.Audproproceso, DbType.String, entity.Audproproceso);
            dbProvider.AddInParameter(command, helper.Audprodescripcion, DbType.String, entity.Audprodescripcion);
            dbProvider.AddInParameter(command, helper.Audprousucreacion, DbType.String, entity.Audprousucreacion);
            dbProvider.AddInParameter(command, helper.Audprofeccreacion, DbType.DateTime, entity.Audprofeccreacion);
            dbProvider.AddInParameter(command, helper.Audprousumodificacion, DbType.String, entity.Audprousumodificacion);
            dbProvider.AddInParameter(command, helper.Audprofecmodificacion, DbType.DateTime, entity.Audprofecmodificacion);


            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public List<VtpAuditoriaProcesoDTO> ListAuditoriaProcesoByFiltro(string audprousucreacion,int tipprocodi,DateTime audprofeccreacioni, DateTime audprofeccreacionf,int NroPagina,int PageSize)
        {
            List<VtpAuditoriaProcesoDTO> entitys = new List<VtpAuditoriaProcesoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListAuditoriaProcesoByFiltro);
            dbProvider.AddInParameter(command, helper.Tipprocodi, DbType.Int32, tipprocodi);
            dbProvider.AddInParameter(command, helper.Audprousucreacion, DbType.String, audprousucreacion);
            dbProvider.AddInParameter(command, helper.Audprousucreacion, DbType.String, audprousucreacion);
            dbProvider.AddInParameter(command, helper.Audprofeccreacion, DbType.String, audprofeccreacioni.ToString(ConstantesBase.FormatoFecha));
            dbProvider.AddInParameter(command, helper.Audprofeccreacion, DbType.String, audprofeccreacionf.ToString(ConstantesBase.FormatoFecha));
            dbProvider.AddInParameter(command, helper.NroPagina, DbType.Int32, NroPagina);
            dbProvider.AddInParameter(command, helper.PageSize, DbType.Int32, PageSize);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpAuditoriaProcesoDTO entity = new VtpAuditoriaProcesoDTO();
                    
                    int iAudprocodi = dr.GetOrdinal(this.helper.Audprocodi);
                    if (!dr.IsDBNull(iAudprocodi)) entity.Audprocodi = Convert.ToInt32(dr.GetValue(iAudprocodi));

                    int iTipprodescripcion = dr.GetOrdinal(this.helper.Tipprodescripcion);
                    if (!dr.IsDBNull(iTipprodescripcion)) entity.Tipprodescripcion = Convert.ToString(dr.GetValue(iTipprodescripcion));

                    int iEstddescripcion = dr.GetOrdinal(this.helper.Estddescripcion);
                    if (!dr.IsDBNull(iEstddescripcion)) entity.Estddescripcion = Convert.ToString(dr.GetValue(iEstddescripcion));

                    int iAudproproceso = dr.GetOrdinal(this.helper.Audproproceso);
                    if (!dr.IsDBNull(iAudproproceso)) entity.Audproproceso = Convert.ToString(dr.GetValue(iAudproproceso));

                    int iAudprodescripcion = dr.GetOrdinal(this.helper.Audprodescripcion);
                    if (!dr.IsDBNull(iAudprodescripcion)) entity.Audprodescripcion = Convert.ToString(dr.GetValue(iAudprodescripcion));

                    int iAudprousucreacion = dr.GetOrdinal(this.helper.Audprousucreacion);
                    if (!dr.IsDBNull(iAudprousucreacion)) entity.Audprousucreacion = Convert.ToString(dr.GetValue(iAudprousucreacion));

                    int iAudprofeccreacion = dr.GetOrdinal(this.helper.Audprofeccreacion);
                    if (!dr.IsDBNull(iAudprofeccreacion)) entity.Audprofeccreacion = dr.GetDateTime(iAudprofeccreacion);
     
                    int iAudprousumodificacion = dr.GetOrdinal(this.helper.Audprousumodificacion);
                    if (!dr.IsDBNull(iAudprousumodificacion)) entity.Audprousumodificacion = Convert.ToString(dr.GetValue(iAudprousumodificacion));
                
                    int iAudprofecmodificacion = dr.GetOrdinal(this.helper.Audprofecmodificacion);
                    if (!dr.IsDBNull(iAudprofecmodificacion)) entity.Audprofecmodificacion = dr.GetDateTime(iAudprofecmodificacion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int NroRegistroAuditoriaProcesoByFiltro(string audprousucreacion, int tipprocodi, DateTime audprofeccreacioni, DateTime audprofeccreacionf)
        {
            int NroRegistros = 0;
            List<VtpAuditoriaProcesoDTO> entitys = new List<VtpAuditoriaProcesoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlNroRegistrosAuditoriaProceso);
            dbProvider.AddInParameter(command, helper.Tipprocodi, DbType.Int32, tipprocodi);
            dbProvider.AddInParameter(command, helper.Audprousucreacion, DbType.String, audprousucreacion);
            dbProvider.AddInParameter(command, helper.Audprousucreacion, DbType.String, audprousucreacion);
            dbProvider.AddInParameter(command, helper.Audprofeccreacion, DbType.String, audprofeccreacioni.ToString(ConstantesBase.FormatoFecha));
            dbProvider.AddInParameter(command, helper.Audprofeccreacion, DbType.String, audprofeccreacionf.ToString(ConstantesBase.FormatoFecha));


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    NroRegistros = Convert.ToInt32(dr["NroRegistros"].ToString().Trim());
                }
            }

            return NroRegistros;
        }




    }
}
