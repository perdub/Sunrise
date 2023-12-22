namespace Sunrise.Types;

using System.ComponentModel.DataAnnotations;
using System.Text;
using SharpHash.Base;

public class Account{
    public string Username {get; private set;}
    [Key]
    public Guid AccountId {get;private set;} = Guid.Empty;

    public string PasswordHash {get;private set;}

    public Sunrise.Types.Enums.PrivilegeLevel PrivilegeLevel{get;set;} = Enums.PrivilegeLevel.Default;
    //два значения которые отвечают за уровнь доступа
    public bool CheckedUser {get;set;} //true если пользователь прошёл верификацию по емайлу 
    public bool VerifyUser {get;set;} //true когда пользователь загрузил некоторое количество постов и вызывает доверие

    public string Email {get;private set;}

    public List<Post> Posts{get;private set;}
    public List<Session> Sessions{get;private set;}

    public DateTime RegistrationDate{get;private set;}

    private Account(){

    }

    public Account(string username, string password, string globalSalt, string email)
    {
        AccountId = Guid.NewGuid();
        RegistrationDate = DateTime.UtcNow;
        
        Email = email;
        Username = username;
        PasswordHash = HashPassword(username, password, globalSalt);
    }

    public string HashPassword(string username, string password, string globalSalt){
        //конвертация данных в массивы байтов
        byte[] passBA = Encoding.UTF8.GetBytes(password);
        byte[] salt = Encoding.UTF8.GetBytes(username+globalSalt);

        var hash = HashFactory.KDF.PBKDF_Scrypt.CreatePBKDF_Scrypt(passBA, salt, 4096, 8, 2);

        var stringHash = Encoding.UTF8.GetString(hash.GetBytes(32));

        return stringHash;
    }

    public bool CheckPassword(string password, string globalSalt){
        //проверкка значений
        if(string.IsNullOrEmpty(PasswordHash)){
            throw new ApplicationException("User object have a empry password hash.");
        }
        if(string.IsNullOrEmpty(password)){
            throw new InvalidDataException("Password cant be a null.");
        }

        //хеширование и сравнение
        var actualyHash = HashPassword(Username, password, globalSalt);
        
        bool isValid = actualyHash.Equals(PasswordHash);

        return isValid;
    }
}