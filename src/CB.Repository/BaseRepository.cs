// using System.Data.Common;

// namespace CB.Repository
// {
//     public abstract class BaseRepository
//     {
//         protected readonly ISqlProvider SqlProvider;
//         protected BaseRepository(ISqlProvider sqlProvider)
//         {
//             SqlProvider = sqlProvider;
//         }
//         protected async Task<int> ReadTotalWithoutDisposingReaderAsync(DbDataReader reader)
//         {
//             if (await SqlProvider.ReadAsync(reader))
//             {
//                 return Convert.ToInt32(await SqlProvider.GetFieldValueAsync<Int64>(reader, "total"));
//             }
//             return 0;
//         }
//     }
// }
