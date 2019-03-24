using NetApp.PlayASPAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace NetApp.PlayASPAPI.Services
{
    public interface INetAppWCFCallback
    {
        [OperationContract(IsOneWay = true)]
        void AckMessage(string message);
    }

    [ServiceContract]
    public interface INetAppWCF
    {
        /// <summary>
        /// 请求与答复模式
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<Product> Products(int startIndex, int count);

        [OperationContract]
        void UpdateProduct(Product product);

        /// <summary>
        /// 单向模式
        /// </summary>
        /// <returns></returns>
        [OperationContract(IsOneWay = true)]
        void SayHello(string message);
    }
}
