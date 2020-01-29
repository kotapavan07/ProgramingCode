using InService.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace InService.Util
{
    public class InventoryAgent
    {
        public InventoryAgent()
        {
        }

        public static void SetIntParam(IDbCommand command, string paramName,int paramValue)
        {
            IDbDataParameter param = command.CreateParameter();
            param.ParameterName = paramName;
            param.DbType = DbType.Int32;
            if (paramValue != int.MinValue)
            {
                param.Value = paramValue;
            }
            else
            {
                param.Value = DBNull.Value;
            }

            command.Parameters.Add(param);
        }

        public List<Inventory> GetInInventories(int locationId, int departmentId, int categoryId, int subCategoryId)
        {
            List<Inventory> list = new List<Inventory>();
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            IDbCommand dataCommand = null;
            IDataReader dataReader = null;
            StringBuilder sqlQuery = new StringBuilder();

            sqlQuery.Append(" with Subcategory_result as ");
            sqlQuery.Append(" (select sc.SubCategoryId,sc.SubCategoryName,c.CategoryName,d.DepartmentName,l.LocationName from SubCategory sc ");
            sqlQuery.Append(" inner join(select CategoryId, CategoryName, DepartmentId from Category ");
            sqlQuery.Append(" where CategoryId = @categoryId ");
            //sqlQuery.Append(" where CategoryName = 'Bakery Bread' ");
            sqlQuery.Append(" ) c on c.CategoryId = sc.CategoryId ");
            sqlQuery.Append(" inner join(select DepartmentId, DepartmentName, LocationId from Department ");
            sqlQuery.Append(" where DepartmentId = @departmentId ");
            //sqlQuery.Append(" where DepartmentName = 'Bakery' ");
            sqlQuery.Append(" ) d on d.DepartmentId = c.DepartmentId ");
            sqlQuery.Append(" inner join(select LocationId, LocationName from Location ");
            sqlQuery.Append(" where LocationId = @locationId ");
            //sqlQuery.Append(" where LocationName = 'Perimeter' ");
            sqlQuery.Append(" ) l on l.LocationId = d.LocationId ");
            sqlQuery.Append(" where sc.SubCategoryId = @subCategoryId ");
            //sqlQuery.Append(" where sc.SubCategoryName = 'Bagels' ");
            sqlQuery.Append(" ) ");
            sqlQuery.Append(" select ii.SkuId,ii.SkuName,scr.LocationName,scr.DepartmentName,scr.CategoryName,scr.SubCategoryName from Inventory ii ");
            sqlQuery.Append(" inner join Subcategory_result scr on ii.SubCategoryId = scr.SubCategoryId; ");

            try
            {
                connection = DBUtil.GetOpenConnection();
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);
                dataCommand = connection.CreateCommand();
                dataCommand.CommandText = sqlQuery.ToString();
                dataCommand.Transaction = transaction;
                SetIntParam(dataCommand, "@locationId", locationId);
                SetIntParam(dataCommand, "@departmentId", departmentId);
                SetIntParam(dataCommand, "@categoryId", categoryId);
                SetIntParam(dataCommand, "@subCategoryId", subCategoryId);
                dataReader = dataCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    Inventory details = new Inventory();
                    details.SkuId = dataReader.IsDBNull(0) ? int.MinValue : dataReader.GetInt32(0);
                    details.SkuName = dataReader.IsDBNull(1) ? null : dataReader.GetString(1);
                    details.LocationName = dataReader.IsDBNull(2) ? null : dataReader.GetString(2);
                    details.DepartmentName = dataReader.IsDBNull(3) ? null : dataReader.GetString(3);
                    details.CategoryName = dataReader.IsDBNull(4) ? null : dataReader.GetString(4);
                    details.SubCategoryName = dataReader.IsDBNull(5) ? null : dataReader.GetString(5);
                    list.Add(details);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                    dataReader = null;
                }
                if (dataCommand != null)
                {
                    dataCommand.Dispose();
                    dataCommand = null;
                }
            }
            return list;
        }

        public Inventory GetSku(int skuId)
        {
            Inventory details = null;
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            IDbCommand dataCommand = null;
            IDataReader dataReader = null;
            StringBuilder sqlQuery = new StringBuilder();

            sqlQuery.Append(" select ii.SkuId,ii.SkuName,l.LocationName,d.DepartmentName,c.CategoryName,sc.SubCategoryName from Inventory ii ");
            sqlQuery.Append(" left join SubCategory sc on sc.SubCategoryId = ii.SubCategoryId ");
            sqlQuery.Append(" left join Category c on c.CategoryId = sc.CategoryId ");
            sqlQuery.Append(" left join Department d on d.DepartmentId = c.DepartmentId ");
            sqlQuery.Append(" left join Location l on l.LocationId = d.LocationId ");
            sqlQuery.Append(" where ii.SkuId = @SkuId");

            try
            {
                connection = DBUtil.GetOpenConnection();
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);
                dataCommand = connection.CreateCommand();
                dataCommand.CommandText = sqlQuery.ToString();
                dataCommand.Transaction = transaction;
                //SetIntParam(dataCommand, "@locationId", locationId);
                //SetIntParam(dataCommand, "@departmentId", departmentId);
                //SetIntParam(dataCommand, "@categoryId", categoryId);
                SetIntParam(dataCommand, "@SkuId", skuId);
                dataReader = dataCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    details = new Inventory();
                    details.SkuId = dataReader.IsDBNull(0) ? int.MinValue : dataReader.GetInt32(0);
                    details.SkuName = dataReader.IsDBNull(1) ? null : dataReader.GetString(1);
                    details.LocationName = dataReader.IsDBNull(2) ? null : dataReader.GetString(2);
                    details.DepartmentName = dataReader.IsDBNull(3) ? null : dataReader.GetString(3);
                    details.CategoryName = dataReader.IsDBNull(4) ? null : dataReader.GetString(4);
                    details.SubCategoryName = dataReader.IsDBNull(5) ? null : dataReader.GetString(5);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                    dataReader = null;
                }
                if (dataCommand != null)
                {
                    dataCommand.Dispose();
                    dataCommand = null;
                }
            }
            return details;
        }

        public bool DepartmentExists(int locationId, int departmentId)
        {
            bool dataAvailable = false;

            List<Inventory> list = new List<Inventory>();
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            IDbCommand dataCommand = null;
            StringBuilder sqlQuery = new StringBuilder();

            sqlQuery.Append("select count(*) from Department d ");
            sqlQuery.Append("inner join Location l on l.LocationId = d.LocationId ");
            sqlQuery.Append("where l.LocationId = @locationId and d.DepartmentId = @departmentId ");
            try
            {
                connection = DBUtil.GetOpenConnection();
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);
                dataCommand = connection.CreateCommand();
                dataCommand.CommandText = sqlQuery.ToString();
                dataCommand.Transaction = transaction;
                SetIntParam(dataCommand, "@locationId", locationId);
                SetIntParam(dataCommand, "@departmentId", departmentId);
                int count = (int)dataCommand.ExecuteScalar();
                if (count > 0)
                    dataAvailable = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (dataCommand != null)
                {
                    dataCommand.Dispose();
                    dataCommand = null;
                }
            }
            return dataAvailable;
        }

        public bool CategoryExists(int locationId, int departmentId, int categoryId)
        {
            bool dataAvailable = false;

            List<Inventory> list = new List<Inventory>();
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            IDbCommand dataCommand = null;
            StringBuilder sqlQuery = new StringBuilder();

            sqlQuery.Append(" select count(*) from Category c ");
            sqlQuery.Append(" inner join Department d on d.DepartmentId = c.DepartmentId ");
            sqlQuery.Append(" inner join Location l on l.LocationId = d.LocationId ");
            sqlQuery.Append(" where l.LocationId = @locationId and d.DepartmentId = @departmentId and c.CategoryId = @categoryId ");
            try
            {
                connection = DBUtil.GetOpenConnection();
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);
                dataCommand = connection.CreateCommand();
                dataCommand.CommandText = sqlQuery.ToString();
                dataCommand.Transaction = transaction;
                SetIntParam(dataCommand, "@locationId", locationId);
                SetIntParam(dataCommand, "@departmentId", departmentId);
                SetIntParam(dataCommand, "@categoryId", categoryId);
                int count = (int)dataCommand.ExecuteScalar();
                if (count > 0)
                    dataAvailable = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (dataCommand != null)
                {
                    dataCommand.Dispose();
                    dataCommand = null;
                }
            }
            return dataAvailable;
        }

        public bool SubCategoryExists(int locationId, int departmentId, int categoryId, int subCategoryId)
        {
            bool dataAvailable = false;

            List<Inventory> list = new List<Inventory>();
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            IDbCommand dataCommand = null;
            StringBuilder sqlQuery = new StringBuilder();

            sqlQuery.Append(" select count(*) from SubCategory sc ");
            sqlQuery.Append(" inner join Category c on c.CategoryId = sc.CategoryId ");
            sqlQuery.Append(" inner join Department d on d.DepartmentId = c.DepartmentId ");
            sqlQuery.Append(" inner join Location l on l.LocationId = d.LocationId ");
            sqlQuery.Append(" where l.LocationId = @locationId and d.DepartmentId = @departmentId and c.CategoryId = @categoryId and sc.SubCategoryId = @subCategoryId ");
            try
            {
                connection = DBUtil.GetOpenConnection();
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);
                dataCommand = connection.CreateCommand();
                dataCommand.CommandText = sqlQuery.ToString();
                dataCommand.Transaction = transaction;
                SetIntParam(dataCommand, "@locationId", locationId);
                SetIntParam(dataCommand, "@departmentId", departmentId);
                SetIntParam(dataCommand, "@categoryId", categoryId);
                SetIntParam(dataCommand, "@subCategoryId", subCategoryId);
                int count = (int)dataCommand.ExecuteScalar();
                if (count > 0)
                    dataAvailable = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (dataCommand != null)
                {
                    dataCommand.Dispose();
                    dataCommand = null;
                }
            }
            return dataAvailable;
        }
    }
}