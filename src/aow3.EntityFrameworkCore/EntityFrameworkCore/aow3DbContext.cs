using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using aow3.Authorization.Roles;
using aow3.Authorization.Users;
using aow3.MultiTenancy;
using aow3.CayGold;
using Aow.Facebook;
using Aow.Facebook.Comments;
using Aow.Facebook.Attachments;
using Aow.VongQuayMayMans;
using aow3.Models.Forum;
using aow3.Todo;

namespace aow3.EntityFrameworkCore
{
    public class aow3DbContext : AbpZeroDbContext<Tenant, Role, User, aow3DbContext>
    {
        /* Define a DbSet for each entity of the application */

        public virtual DbSet<TodoItem> TodoItem { get; set; }

        public virtual DbSet<DangKyCayGold> DangKyCayGold { get; set; }
        public virtual DbSet<PostFaceBook> PostFaceBooks { get; set; }
        public virtual DbSet<ViewPostFanpageFaceBook> ViewPostFanpageFaceBook { get; set; }
        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<Attachments> Attachments { get; set; }
        public virtual DbSet<VongQuayMayMan> VongQuayMayMan { get; set; }
        public virtual DbSet<ViewVongQuayMayMan> ViewVongQuayMayMan { get; set; }
        public virtual DbSet<PhanThuongVongQuay> PhanThuongVongQuay { get; set; }

        public virtual DbSet<BaiDangForum> BaiDangForum { get; set; }
        public aow3DbContext(DbContextOptions<aow3DbContext> options)
            : base(options)
        {
        }
    }
}
