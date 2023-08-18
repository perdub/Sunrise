using Sunrise.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Sunrise.Controllers;

public class FindController : Controller
{
    SunriseContext sc;
    ILogger<FindController> logger;

    public FindController(SunriseContext sunriseContext, ILogger<FindController> l)
    {
        sc = sunriseContext;
        logger = l;
    }

    [Route("/find")]
    public async Task<IActionResult> Find(
        string[] tag, //набор тегов для поиска
        int offset = 0, //сколько постов надо пропустить
        int count = Constants.POST_PER_SCROLL, //сколько постов надо получить
        bool randomOrder = false //сортировать в случайном порядке
        )
    {
        //все посты которые будут отданы методом
        List<Post> final = new List<Post>(count);
        int skiped = 0;
        
        //проверка на то есть ли в массиве с тегами сами теги и на то пустые ли они
        if (tag.Length != 0 && !tag.All(a=>string.IsNullOrWhiteSpace(a)))
        {
            //загрузка всех тегов из бд
            Tag[] tags = new Tag[tag.Length];
            for (int i = 0; i < tags.Length; i++)
            {
                //todo: change load type(from eager to lazy/explict)
                tags[i] = sc.Tags
                    .Include(a => a.Post)
                        .ThenInclude(b => b.File)
                    .Where(a => a.SearchText == tag[i])
                    .First();
            }
            //сортировка тегов по количеству постов(сначала тег с самым маленьким количеством)
            tags = tags.OrderBy(a => a.PostCount).ToArray();
            //извлечение всех постов и сортировка
            List<Post> posts = new List<Post>(tags[0].Post);
            if(randomOrder){
                Random r = new ();
                posts = posts
                    .Where(a=>(int)a.Status>0)
                    .OrderBy(a => r.NextDouble())
                    .ToList();
            }
            else{
                posts = posts
                    .Where(a=>(int)a.Status>0)
                    .OrderByDescending(a=>a.PostCreationTime)
                    .ToList();
            }

            for (int i = 0; i < posts.Count(); i++)
            {
                //содержит ли пост в себе все теги для поиска
                bool containsAllTags = true;
                for (int j = 0; j < tags.Length; j++)
                {
                    if (!posts[i].Tags.Contains(tags[j]))
                    {
                        containsAllTags = false;
                        break;
                    }
                }
                if (containsAllTags)
                {
                    //если да то

                    if (offset != 0 && skiped < offset)
                    {
                        //если количество постов для пропуска не равно нулю и счётчик пропущеных постов меньше чем нужное количество то инкреметируем счётчик и переходим к следующей итерации
                        skiped++;
                        continue;
                    }
                    //добавляем пост
                    final.Add(posts[i]);
                    
                    if (final.Count() == count)
                    {
                        //если количество постов совпадает  с нужным, завершаем цикл
                        break;
                    }
                }
            }
        }
        else
        {
            //если нет, мы просто сортируем посты, пропускаем и забираем их
            var posts = sc.Posts
                .Where(a=>(int)a.Status>0)
                .Include(a=>a.File);
            System.Linq.IOrderedQueryable<Sunrise.Types.Post> tempQuery;
            System.Linq.IQueryable<Sunrise.Types.Post> finalCol;

            //сортировка
            if(randomOrder){
                //в случайном порядке
                tempQuery = posts.OrderBy(a=>EF.Functions.Random());
            }
            else{
                //по дате создания(самые новые идут первыми)
                tempQuery = posts.OrderByDescending(a => a.PostCreationTime);
            }

            finalCol = tempQuery
                .Skip(offset)
                .Take(count);
            final.AddRange(finalCol);
        }
        //создание промежуточных обьектов
        List<ScrollItem> items = new List<ScrollItem>(final.Capacity);
        foreach(var f in final){
            items.Add(new ScrollItem(
                f.Id,
                f.File.Paths[1],
                (int)f.File.ContentType
            ));
        }
        return Ok(items);
    }
}