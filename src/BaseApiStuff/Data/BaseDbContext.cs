using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BaseApiStuff.Data;

public abstract class BaseDbContext<TUser>(DbContextOptions<BaseDbContext<TUser>> options) : IdentityDbContext<TUser>(options)
 where TUser : IdentityUser
{
    
}