// namespace CB.Repository;
// public class UserRepository : BaseRepository
// {
//     private readonly ILogger<UserRepository> _logger;
//     public UserRepository(ISqlProvider sqlProvider, ILogger<UserRepository> logger) : base(sqlProvider)
//     {
//         _logger = logger;
//     }
//     public async Task DeleteUserAsync(int userId)
//     {
//         var command = SqlProvider.CreateCommand(Routines.DeleteUser);
//         SqlProvider.AddParameterWithValue(command, "vUserId", userId);
//         using (command)
//         {
//             await SqlProvider.ExecuteScalarAsync(command);
//         }
//     }
// }
