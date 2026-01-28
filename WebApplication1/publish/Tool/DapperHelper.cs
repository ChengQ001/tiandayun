using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using Dapper;
using MySql.Data.MySqlClient;

namespace WebApplication1.Tool
{
    public class DapperHelper
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private static readonly string connectionString;

        /// <summary>
        /// 静态构造函数，初始化连接字符串
        /// </summary>
        static DapperHelper()
        {
            connectionString = ConfigurationManager.AppSettings["ConnectionString"];
        }

        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        public static string ConnectionString => connectionString;

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <returns>IDbConnection</returns>
        public static IDbConnection GetConnection()
        {
            // 直接返回一个新的连接，不使用连接池
            var connection = new MySqlConnection(connectionString.Replace("Pooling=true", "Pooling=false"));
            connection.Open();
            return connection;
        }

        /// <summary>
        /// 查询多条记录
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="sql">SQL语句</param>
        /// <param name="param">参数</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>实体列表</returns>
        public static IEnumerable<T> Query<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            // 如果有事务，使用事务的连接，否则创建新连接
            if (transaction != null)
            {
                return transaction.Connection.Query<T>(sql, param, transaction, true, commandTimeout, commandType);
            }
            else
            {
                using (var connection = GetConnection())
                {
                    return connection.Query<T>(sql, param, transaction, true, commandTimeout, commandType);
                }
            }
        }

        /// <summary>
        /// 查询单条记录
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="sql">SQL语句</param>
        /// <param name="param">参数</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>实体</returns>
        public static T QueryFirstOrDefault<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            // 如果有事务，使用事务的连接，否则创建新连接
            if (transaction != null)
            {
                return transaction.Connection.QueryFirstOrDefault<T>(sql, param, transaction, commandTimeout, commandType);
            }
            else
            {
                using (var connection = GetConnection())
                {
                    return connection.QueryFirstOrDefault<T>(sql, param, transaction, commandTimeout, commandType);
                }
            }
        }
        
        /// <summary>
        /// 查询单条记录（非泛型版本）
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="param">参数</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>结果对象</returns>
        public static object QueryFirstOrDefault(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            // 如果有事务，使用事务的连接，否则创建新连接
            if (transaction != null)
            {
                return transaction.Connection.QueryFirstOrDefault(sql, param, transaction, commandTimeout, commandType);
            }
            else
            {
                using (var connection = GetConnection())
                {
                    return connection.QueryFirstOrDefault(sql, param, transaction, commandTimeout, commandType);
                }
            }
        }

        /// <summary>
        /// 执行增删改操作
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="param">参数</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>影响行数</returns>
        public static int Execute(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            // 如果有事务，使用事务的连接，否则创建新连接
            if (transaction != null)
            {
                return transaction.Connection.Execute(sql, param, transaction, commandTimeout, commandType);
            }
            else
            {
                using (var connection = GetConnection())
                {
                    return connection.Execute(sql, param, transaction, commandTimeout, commandType);
                }
            }
        }

        /// <summary>
        /// 执行查询并返回单行单列结果
        /// </summary>
        /// <typeparam name="T">结果类型</typeparam>
        /// <param name="sql">SQL语句</param>
        /// <param name="param">参数</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>结果</returns>
        public static T ExecuteScalar<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            // 如果有事务，使用事务的连接，否则创建新连接
            if (transaction != null)
            {
                return transaction.Connection.ExecuteScalar<T>(sql, param, transaction, commandTimeout, commandType);
            }
            else
            {
                using (var connection = GetConnection())
                {
                    return connection.ExecuteScalar<T>(sql, param, transaction, commandTimeout, commandType);
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="param">参数</param>
        /// <returns>影响行数</returns>
        public static int ExecuteMultiple(string sql, object param = null)
        {
            using (var connection = GetConnection())
            {
                return connection.Execute(sql, param);
            }
        }
    }
}
