//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace GroupBoizDAL.Data
//{
//    public static void Seed(AppDbContext context)
//    {
//        context.Database.Migrate(); // Áp dụng migrations nếu chưa chạy

//        // Kiểm tra xem đã có dữ liệu chưa, nếu có thì không thêm lại
//        if (!context.Categories.Any())
//        {
//            context.Categories.AddRange(
//                new Category { CategoryName = "Academic News", CategoryDesciption = "News related to academics.", IsActive = true },
//                new Category { CategoryName = "Student Affairs", CategoryDesciption = "News about student activities.", IsActive = true },
//                new Category { CategoryName = "Campus Safety", CategoryDesciption = "News about campus security.", IsActive = true },
//                new Category { CategoryName = "Alumni News", CategoryDesciption = "Updates about alumni achievements.", IsActive = true },
//                new Category { CategoryName = "Capstone Project News", CategoryDesciption = "News about student capstone projects.", IsActive = false }
//            );
//            context.SaveChanges();
//        }

//        if (!context.SystemAccounts.Any())
//        {
//            context.SystemAccounts.AddRange(
//                new SystemAccount { AccountName = "Emma William", AccountEmail = "EmmaWilliam@FUNewsManagement.org", AccountRole = 2, AccountPassword = "password1" },
//                new SystemAccount { AccountName = "Olivia James", AccountEmail = "OliviaJames@FUNewsManagement.org", AccountRole = 2, AccountPassword = "password2" },
//                new SystemAccount { AccountName = "Isabella David", AccountEmail = "IsabellaDavid@FUNewsManagement.org", AccountRole = 1, AccountPassword = "password3" },
//                new SystemAccount { AccountName = "Michael Charlotte", AccountEmail = "MichaelCharlotte@FUNewsManagement.org", AccountRole = 1, AccountPassword = "password4" },
//                new SystemAccount { AccountName = "Steve Paris", AccountEmail = "SteveParis@FUNewsManagement.org", AccountRole = 1, AccountPassword = "password5" }
//            );
//            context.SaveChanges();
//        }

//        if (!context.Tags.Any())
//        {
//            context.Tags.AddRange(
//                new Tag { TagName = "Education", Note = "Education-related content" },
//                new Tag { TagName = "Technology", Note = "Technology-related content" },
//                new Tag { TagName = "Research", Note = "Latest research news" },
//                new Tag { TagName = "Innovation", Note = "Innovative ideas and projects" },
//                new Tag { TagName = "Campus Life", Note = "Life on campus" },
//                new Tag { TagName = "Faculty", Note = "Faculty achievements" },
//                new Tag { TagName = "Alumni", Note = "News about alumni" },
//                new Tag { TagName = "Events", Note = "University events" },
//                new Tag { TagName = "Resources", Note = "Available resources for students" }
//            );
//            context.SaveChanges();
//        }

//        if (!context.NewsArticles.Any())
//        {
//            context.NewsArticles.AddRange(
//                new NewsArticle
//                {
//                    NewsTitle = "University FU Celebrates Alumni Success",
//                    Headline = "Celebrating FU Alumni",
//                    CreatedDate = DateTime.Now,
//                    NewsContent = "FU celebrates the achievements of its alumni.",
//                    NewsSource = "University Website",
//                    CategoryID = 4,
//                    NewsStatus = 1,
//                    CreatedByID = 1,
//                    UpdatedByID = 1,
//                    ModifiedDate = DateTime.Now
//                },
//                new NewsArticle
//                {
//                    NewsTitle = "Alumni Association Mentorship Program",
//                    Headline = "Alumni Helping Students",
//                    CreatedDate = DateTime.Now,
//                    NewsContent = "The Alumni Association launches a mentorship program.",
//                    NewsSource = "News Blog",
//                    CategoryID = 4,
//                    NewsStatus = 1,
//                    CreatedByID = 1,
//                    UpdatedByID = 1,
//                    ModifiedDate = DateTime.Now
//                },
//                new NewsArticle
//                {
//                    NewsTitle = "Academic Innovations Announced",
//                    Headline = "New Research Initiatives",
//                    CreatedDate = DateTime.Now,
//                    NewsContent = "FU introduces new research centers and academic programs.",
//                    NewsSource = "University Press",
//                    CategoryID = 1,
//                    NewsStatus = 1,
//                    CreatedByID = 2,
//                    UpdatedByID = 2,
//                    ModifiedDate = DateTime.Now
//                }
//            );
//            context.SaveChanges();
//        }

//        if (!context.NewsTags.Any())
//        {
//            context.NewsTags.AddRange(
//                new NewsTag { NewsArticleID = 1, TagID = 5 },
//                new NewsTag { NewsArticleID = 1, TagID = 7 },
//                new NewsTag { NewsArticleID = 2, TagID = 5 },
//                new NewsTag { NewsArticleID = 2, TagID = 7 },
//                new NewsTag { NewsArticleID = 3, TagID = 1 },
//                new NewsTag { NewsArticleID = 3, TagID = 8 }
//            );
//            context.SaveChanges();
//        }
//    }
//}
//}
