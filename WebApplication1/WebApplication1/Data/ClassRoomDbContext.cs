using ClassRoomMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ClassRoomMVC.Data;
public class ClassRoomDbContext : IdentityDbContext<User,IdentityRole,string>
{
    public DbSet<TaskModel>? Tasks { get; set; }
    public DbSet<Group>? Groups { get; set; }

    public ClassRoomDbContext(DbContextOptions options) : base(options) { }
}
