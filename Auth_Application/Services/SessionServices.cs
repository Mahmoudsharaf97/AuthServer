using IdentityApplication.Interface;
namespace IdentityApplication.Services
{
    public class SessionServices : ISessionServices
    {
        //public IRepository<SessionStatus> _sesssionRepo { get; }
        //public SessionServices( IRepository<SessionStatus> sesssionRepo)
        //{
        //    _sesssionRepo = sesssionRepo;
        //}
        //public async Task <SessionStatus> GetUsrerSessionAsync(string userId)
        //{
        //  return  await _sesssionRepo.GetFirstOrDefaultAsync(a => a.UserId == userId);
        //}
   
        //public  List<SessionStatus> GetUsrerAllSessions(string userId)
        //{
        //  return   _sesssionRepo.GetAll().Where(a=>a.UserId==userId).ToList();
        //}   
        //public async Task Removeasync(SessionStatus entity)
        //{
        //     await _sesssionRepo.DeleteAsync(entity);
        //}
        //public async Task AddSessionAsync(SessionStatus session)
        //{
        //    await _sesssionRepo.AddAsync(session);
        //}
    }
}
