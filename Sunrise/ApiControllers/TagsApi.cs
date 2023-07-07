using Microsoft.AspNetCore.Mvc;

namespace Sunrise.Api;

[Route("api/tags")]
public class TagsApi : Controller
{
    //сохранение доступа к контексту дб локально
    CacheService cs;
    public TagsApi(CacheService c)
    {
        cs = c;
    }

    //получение пользователя через его имя
    [Route("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Types.Tag))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTag(int id)
    {
        try
        {
            return Ok(await cs.GetTagAsync(id));
        }
        catch (Sunrise.Types.Exceptions.NotFoundObjectException)
        {
            return NotFound();
        }
    }

    [Route("convert")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Types.Tag[]))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTag()
    {
        //метод который возвращает теги по их поисковому запросу

        //чтение тела и разбивка его на отдельные теги (оно следующего формата (тег1 тег2 ... тегN))
        var q = new StreamReader(HttpContext.Request.Body);
        var raw = await q.ReadToEndAsync();
        string[] arr = raw.Split();
        q.Dispose();

        var tags = await GetTagsAsync(arr, cs);

        return Ok(tags);
    }
    //получение тегов через бд
    public async Task<Types.Tag[]> GetTagsAsync(string[] search, CacheService cs)
    {//создание массивов для результата и тасков
        //массив тасков больше потомучто он последний элемент это таск асинхронного сохранения изменений в бд
        
        Types.Tag[] resultArr = new Types.Tag[search.Length];
        Task[] taskArr = new Task[search.Length + 1];

        for (int i = 0; i < search.Length; i++)
        {
            //цикл создающий и запускающий задачи
            taskArr[i] = getorcreatetag(i);
        }
        //добавление таска сохранения
        taskArr[^1] = cs.dbcontext.SaveChangesAsync();
        await Task.WhenAll(taskArr);

        return resultArr;
        async Task getorcreatetag(int i)
        {
            //получение тега
            var res = await cs.GetTagAsync(search[i]);
            if (res == null)
            {
                //если результат нулл, то мы создаём новый тег и добавляем его в бд
                res = new Types.Tag(search[i]);
                cs.dbcontext.Tags.Add(res);
            }
            //добавление в массив результатов
            resultArr[i] = res;

        }
    }
}