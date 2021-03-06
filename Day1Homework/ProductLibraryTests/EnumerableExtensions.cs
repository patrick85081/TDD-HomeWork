﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProductLibraryTests
{
    public static class EnumerableExtensions
    { 
        /// <summary>
        /// 計算連續群組 資料欄位加總
        /// </summary>
        /// <typeparam name="TSource">來源型別</typeparam>
        /// <param name="source">加總來源物件</param>
        /// <param name="groupCount">連續的數目</param>
        /// <param name="sumProperty">加總的資料欄位</param>
        /// <returns>加總群組</returns>
        public static int[] GetGroupSum<TSource>(
            this IEnumerable<TSource> source,
            int groupCount,
            Expression<Func<TSource, int>> sumProperty)
        {
            if (groupCount <= 0)
                throw new ArgumentException(
                    $"{nameof(groupCount)} must more than 0.");


            if (sumProperty == null)
                throw new ArgumentNullException(
                    $"{nameof(sumProperty)} can`t be null.");

            // 檢查是否為Product的屬性
            if (sumProperty.Body.NodeType == ExpressionType.MemberAccess)
                ;
            // 檢查是否是Product的屬性 再 轉型int
            else if (sumProperty.Body.NodeType == ExpressionType.Convert &&
                 (sumProperty.Body as UnaryExpression).Operand.NodeType == ExpressionType.MemberAccess)
                ;
            // 都不符合 以上條件
            else
                throw new ArgumentException(
                    $"{nameof(sumProperty)} must be property.");

            Func<TSource, int> mSumProperty = sumProperty.Compile();

            int[] result = null;

            result = source.Select((p, i) => new { Index = i, Item = p })
                .GroupBy(item => item.Index / groupCount)
                .Select(group => group.Select(g => g.Item).Sum(mSumProperty))
                .ToArray();

            return result;
        }
    }
}
