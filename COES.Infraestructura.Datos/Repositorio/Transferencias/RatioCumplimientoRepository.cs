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
   public class RatioCumplimientoRepository: RepositoryBase, IRatioCumplimientoRepository
    {
             public RatioCumplimientoRepository(string strConn)
            : base(strConn)
        {
        }

        RatioCumplimientoHelper helper = new RatioCumplimientoHelper();

        public List<RatioCumplimientoDTO> GetByCodigo(int? tipoemprcodi, int? pericodi,int version)
        {
            List<RatioCumplimientoDTO> entitys = new List<RatioCumplimientoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCodigo);


            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Tipoemprcodi, DbType.Int32, tipoemprcodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Tipoemprcodi, DbType.Int32, tipoemprcodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Tipoemprcodi, DbType.Int32, tipoemprcodi);
            dbProvider.AddInParameter(command, helper.Version, DbType.Int32, version);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Tipoemprcodi, DbType.Int32, tipoemprcodi);
            dbProvider.AddInParameter(command, helper.Version, DbType.Int32, version);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Tipoemprcodi, DbType.Int32, tipoemprcodi);
            dbProvider.AddInParameter(command, helper.Version, DbType.Int32, version);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Tipoemprcodi, DbType.Int32, tipoemprcodi);
            dbProvider.AddInParameter(command, helper.Version, DbType.Int32, version);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RatioCumplimientoDTO entity = new RatioCumplimientoDTO();

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.EmprCodi = dr.GetInt32(iEmprcodi);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.EmprNomb = dr.GetString(iEmprnomb);

                    int iTipoemprcodi = dr.GetOrdinal(this.helper.Tipoemprcodi);
                    if (!dr.IsDBNull(iTipoemprcodi)) entity.TipoEmprCodi = dr.GetInt32(iTipoemprcodi);

                    int iRequerido = dr.GetOrdinal(this.helper.Requerido);
                    if (!dr.IsDBNull(iRequerido)) entity.Requerido = dr.GetDecimal(iRequerido);

                    int iInformado = dr.GetOrdinal(this.helper.Informado);
                    if (!dr.IsDBNull(iInformado)) entity.Informado = dr.GetDecimal(iInformado);

                    int iInfofinal = dr.GetOrdinal(this.helper.Infofinal);
                    if (!dr.IsDBNull(iInfofinal)) entity.Infofinal = dr.GetDecimal(iInfofinal);

                    entitys.Add(entity);
                    
                }
            }

            return entitys;
        }

        public List<RatioCumplimientoDTO> GetByCriteria(string nombre)
        {
            List<RatioCumplimientoDTO> entitys = new List<RatioCumplimientoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Emprnomb, DbType.String, nombre);

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
