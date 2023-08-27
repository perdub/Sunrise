namespace Sunrise.Storage;

public interface IServer{
    //инициализация хранилища(создание папки, подключение к серверу)
    void Init();
    //сохранение элемента
    Task<Types.FileInfo> SaveItem(
        //конвертор
        Sunrise.Convert.AbstractConvert converter,
        //уникальный ид для файла
        Guid id,
        //сам элемент
        byte[] arr,
        //расширение файла
        string fileExtension
    );
}