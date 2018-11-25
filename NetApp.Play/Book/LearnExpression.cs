using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;

namespace NetApp.Play.Book
{
    public class LearnExpression
    {
        //1. orm lambda转sql

        public void Hello()
        {
            Expression<Func<int, bool>> lambda = num => num >= 5;

            //创建lambda表达式 num=>num>=5
            //第一步创建输入参数,参数名为num，类型为int类型
            ParameterExpression numParameter = Expression.Parameter(typeof(int), "num");
            //第二步创建常量表达式5，类型int
            ConstantExpression constant = Expression.Constant(5, typeof(int));
            //第三步创建比较运算符>=,大于等于,并将输入参数表达式和常量表达式输入
            //表示包含二元运算符的表达式。BinaryExpression继承自Expression
            BinaryExpression greaterThanOrEqual = Expression.GreaterThanOrEqual(numParameter, constant);
            var lambda2 = Expression.Lambda<Func<int, bool>>(greaterThanOrEqual, numParameter);

            ParameterExpression i = Expression.Parameter(typeof(int), "i");
            ConstantExpression constExpr = Expression.Constant(5, typeof(int));
            BinaryExpression binaryExpression = Expression.Assign(i, constExpr);
        }

    }
}
