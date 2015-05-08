using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyChy.Frame.Data
{
    /// <summary>
    /// 业务单元操作接口
    /// </summary>
    public interface IUnitOfWork
    {
        #region 属性

        /// <summary>
        /// 获取 当前单元操作是否已被提交
        /// </summary>
        bool IsCommitted { get; }

        #endregion

        #region 方法

        /// <summary>
        /// 提交当前单元操作的结果
        /// </summary>
        /// <param name="validateOnSaveEnabled">保存时是否自动验证跟踪实体</param>
        /// <returns></returns>
        int Commit(bool validateOnSaveEnabled = true);

        /// <summary>
        /// 把当前单元操作回滚成未提交状态
        /// </summary>
        void Rollback();


        ///<summary>
        ///执行Sql语句
        ///</summary>
        ///<param name="sqlstring">sp_Userinfos_deleteByID @ID 有几个参数，存储后面要带几个参数以逗号分隔</param>
        ///<param name="parameter">SqlParameter[] para = new SqlParameter[] {new SqlParameter("@ID",id)};</param>
        ///<returns></returns>
        int Execute(string sqlstring, object[] parameter);


        ///<summary>
        ///执行Sql语句
        ///</summary>
        ///<param name="sqlstring">delete UserInfoes  where id=@ID</param>
        ///<param name="parameter">SqlParameter[] para = new SqlParameter[] {new SqlParameter("@ID",id)}; </param>
        ///<returns></returns>
        IList<T> Execute<T>(string sqlstring, object[] parameter);

        #endregion

        #region 执行存储过程



        #endregion
    }
}
