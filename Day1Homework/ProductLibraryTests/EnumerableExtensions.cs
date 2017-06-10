using System;
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
        /// <typeparam name="TProperty">需要加總的型別，限定數值型別，實際計算會轉型為<see cref="int"/></typeparam>
        /// <param name="dataSource">加總來源物件</param>
        /// <param name="seqGroupCount">連續的數目</param>
        /// <param name="sumProperty">加總的資料欄位</param>
        /// <returns>加總群組</returns>
        public static int[] ClcSeqGroupSum<TSource, TProperty>(
            this IEnumerable<TSource> dataSource, 
            int seqGroupCount,
            Expression<Func<TSource, TProperty>> sumProperty)
                where TSource : class
                where TProperty : struct
        {
            if (sumProperty == null)
                throw new ArgumentNullException(
                    $"{nameof(sumProperty)} can`t be null.");

            // 檢查是否為Product的屬性
            if (sumProperty.Body.NodeType != ExpressionType.MemberAccess)
                throw new ArgumentException(
                    $"{nameof(sumProperty)} must be property.");

            UnaryExpression converterProperty = null;
            try
            {
                // 將 (p) => p.Property
                // 更改為 (p) => (int)p.Property
                converterProperty = Expression.Convert(
                    sumProperty.Body, typeof(int));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(
                    $"{nameof(sumProperty)} is not support calculation.", ex);
            }
            var converProperty = Expression.Lambda<Func<TSource, int>>(
                converterProperty, sumProperty.Parameters);
            //Func<TSource, int> test = converProperty.Compile();

            return dataSource.ClcSeqGroupSum(seqGroupCount, converProperty);
        }

        /// <summary>
        /// 計算連續群組 資料欄位加總
        /// </summary>
        /// <typeparam name="TSource">來源型別</typeparam>
        /// <param name="dataSource">加總來源物件</param>
        /// <param name="seqGroupCount">連續的數目</param>
        /// <param name="sumProperty">加總的資料欄位</param>
        /// <returns>加總群組</returns>
        public static int[] ClcSeqGroupSum<TSource>(
            this IEnumerable<TSource> dataSource,
            int seqGroupCount,
            Expression<Func<TSource, int>> sumProperty)
        {
            if (seqGroupCount < 0)
                throw new ArgumentException(
                    $"{nameof(seqGroupCount)} must more than 0.");
            else if (seqGroupCount == 0)
                return new int[] { 0 };


            if (sumProperty == null)
                throw new ArgumentNullException(
                    $"{nameof(sumProperty)} can`t be null.");

            /* 原本只有考慮int的屬性，只有檢查是否符合 (Product p) => p.Property
             * 但條件是 1.任何型別 2.結果型別可直接用int
             * 所以改成 也接受 (Product p) => (int)p.Property
             * 因為要做數值加總 限定Property為Value Type
             * 統一轉成int做計算
            if (sumProperty.Body.NodeType != ExpressionType.MemberAccess)
                throw new ArgumentException(
                    $"{nameof(sumProperty)} must be property.");
            */

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

            result = dataSource.Select((p, i) => new { Index = i, Item = p })
                .GroupBy(item => item.Index / seqGroupCount)
                .Select(group => group.Sum(item => mSumProperty.Invoke(item.Item)))
                .ToArray();

            return result;
        }
    }
}
