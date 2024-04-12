namespace E_Commerce_Bot.Services;

public interface ILocalizationHandler
{
    string GetValue(string key, params string[] arguments);
}