using DataLayer.EntityModels;
using FriendlyRS1.Repository.RepostorySetup;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace FriendlyRS1.Repository.Repositories
{
    public class PostRepository : Repository<Post>
    {
        private ApplicationDbContext _context;
        private DbSet<PostRepository> _dbSet;
        public PostRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;

        }

        public List<Post> ConectionsPosts(int id, int take, int skip)
        {

            var list = new int[] { id }.AsQueryable();


            List<Post> posts = (from p in _context.Post.Select(p => new Post
            {
                Author = p.Author,
                Hobby = p.Hobby,
                Text = p.Text,
                CreatedDate = p.CreatedDate,
                AuthorId = p.AuthorId,
                Id = p.Id
            })
                                join t2 in (from f1 in _context.Friendship where f1.User2Id == id && f1.Status.Status == 1 select f1.User1Id)
                                            .Union(from f1 in _context.Friendship where f1.User1Id == id && f1.Status.Status == 1 select f1.User2Id)
                                            .Union(from f1 in _context.Users where f1.Id == id select f1.Id)
                                            on p.AuthorId equals t2
                                select p)
                .Skip(skip)
                .Take(take)
                .AsNoTracking()
                .ToList();

            return posts;
        }

        public List<Post> NearbyPost(int id, double lat1, double long1, List<Hobby> hobbies, int skip, int take)
        {
            List<Post> p = _context.Post
                .AsEnumerable()
                .Where(x => CalcDistance(x.Latitude, x.Longitude, lat1, long1) != -1 && hobbies.Any(y => y.Id == x.HobbyId) && x.AuthorId != id)
                .Skip(skip)
                .Take(take)
                .ToList();

            return p;
        }

        public double CalcDistance(string la1, string lo1, double lat2, double lon2)
        {
            if (la1 == null || lo1 == null || lat2 == 0 || lon2 == 0)
                return -1;

            double lat1 = double.Parse(la1);
            double lon1 = double.Parse(lo1);

            var R = 6371; // km
            var dLat = toRad(lat2 - lat1);
            var dLon = toRad(lon2 - lon1);
            var latitude1 = toRad(lat1);
            var latitude2 = toRad(lat2);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(latitude1) * Math.Cos(latitude2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c;

            if (d <= 10)
            {
                return d;
            }
            else
            {
                return -1;
            }

        }
        public double toRad(double Value)
        {
            return Value * Math.PI / 180;
        }

        public List<Post> GetMyPosts(int id, int skip, int take)
        {
            List<Post> p = _context.Post.Where(x => x.AuthorId == id).Select(p => new Post
            {
                Author = p.Author,
                Hobby = p.Hobby,
                Text = p.Text,
                CreatedDate = p.CreatedDate,
                AuthorId = p.AuthorId,
                Id = p.Id
            })
            .OrderByDescending(x => x.CreatedDate)
            .Skip(skip)
            .Take(take)
            .ToList();

            return p;
        }
    }
}
