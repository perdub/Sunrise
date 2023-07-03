namespace Sunrise.Types;
using Sunrise.Utilities;
//пользователь
public class User : IApiView
{
    //имя
    public string Name {get; private set;}
    //пароль (хеш)
    public string PasswordHash {get;private set;}

    public string Email {get;private set;}

    public Guid Id {get;private set;}
    //дата создания аккаунта
    public DateTime AccountCreationTime{get;private set;}
    //уровень доступа
    public PrivilegeLevel Level {get; private set;}

    public List<Post> Posts {get;private set;}

    public User(string name, string password, string email)
    {
        Name = name;
        PasswordHash = password.GetSha512();
        Email = email;
        addData();
    }

    public User(UserRegistrationInfo uri){
        if(string.IsNullOrWhiteSpace(uri.name) || string.IsNullOrWhiteSpace(uri.password) || string.IsNullOrWhiteSpace(uri.email)){
            throw new Exceptions.InvalidParamsException("Fall to create user.");
        }
        Name = uri.name;
        PasswordHash = uri.password.GetSha512();
        Email = uri.email;
        addData();
    }
    private User()
    {

    }
    void addData(){
        //этот метод устанавлевает неизменяемые данные и используется в конструкторах
        Id = Guid.NewGuid();
        AccountCreationTime = DateTime.UtcNow;
        Level = PrivilegeLevel.Default;
    }

    public ApiView GetApiView()
    {
        return new UserApiView{
            Name = this.Name,
            AccountCreationTime = this.AccountCreationTime,
            Id = this.Id
        };
    }

    public bool CheckPassword(string password){
        return PasswordHash == password.GetSha512();
    }
}
//класс-запись для представление данных для регистрации в json
public record class UserRegistrationInfo(string name, string password, string email);
//класс-запись для представление данных для Входа в json
public record class UserLoginInfo(string name, string password, bool rememberMe = false);
//клаасс-запись возвращающий результат попытки входа
public record class UserLoginResult(LoginResult result, string message, string sessionId);