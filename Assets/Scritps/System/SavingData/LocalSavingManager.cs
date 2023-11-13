using System;
using Newtonsoft.Json;
using UnityEngine;

public class LocalSavingManager : IPlatformSavingManager
{
    #region Base Get/Set
    //INT
    protected void SetLocalValue(string type, int value) => 
        PlayerPrefs.SetInt(type, value);

    protected int GetLocalValue(string type, int defaultValue) => 
        PlayerPrefs.GetInt(type, defaultValue);

    //FLOAT
    protected void SetLocalValue(string type, float value) => 
        PlayerPrefs.SetFloat(type, value);

    protected float GetLocalValue(string type, float defaultValue) => 
        PlayerPrefs.GetFloat(type, defaultValue);

    //STRING
    protected void SetLocalValue(string type, string value) => 
        PlayerPrefs.SetString(type, value);

    protected string GetLocalValue(string type, string defaultValue) => 
        PlayerPrefs.GetString(type, defaultValue);

    //BOOL
    protected void SetLocalValue(string type, bool value) => 
        PlayerPrefs.SetInt(type, value ? 1 : 0);

    protected bool GetLocalValue(string type, bool defaultValue) => 
        1 == PlayerPrefs.GetInt(type, defaultValue ? 1 : 0);
    #endregion

    #region Save/Delete
    public virtual void Save() => 
        PlayerPrefs.Save();

    public virtual void DeleteSavedData() => 
        PlayerPrefs.DeleteAll();
    #endregion

    #region Volume
    public virtual void SetMusicVolume(float value) => SetLocalValue(SavingManagerType.MusicVolume, value);

    public virtual float GetMusicVolume(float defaultValue) => GetLocalValue(SavingManagerType.MusicVolume, defaultValue);

    public virtual void SetSFXVolume(float value) => SetLocalValue(SavingManagerType.SFXVolume, value);

    public virtual float GetSFXVolume(float defaultValue) => GetLocalValue(SavingManagerType.SFXVolume, defaultValue);
    #endregion

    #region GameProgress

    public virtual void SetGameProgress(GameProgress gameProgress, Action<SavingManagerResponseStatus> callback)
    {
        SetLocalValue(SavingManagerType.GameProgress, JsonConvert.SerializeObject(gameProgress));
        callback(SavingManagerResponseStatus.Success);
    }

    public virtual void GetGameProgress(Action<GameProgress, SavingManagerResponseStatus> callback)
    {
        var jsonText = GetLocalValue(SavingManagerType.GameProgress, string.Empty);
        if (string.IsNullOrEmpty(jsonText)) //if the player has never saved.
        {
            callback(new GameProgress(), SavingManagerResponseStatus.Success);
            return;
        }

        var gameProgress = JsonConvert.DeserializeObject<GameProgress>(jsonText);
        if (gameProgress == null)
        {
            callback(new GameProgress(), SavingManagerResponseStatus.LocalError);
        }
        else
        {
            callback(gameProgress, SavingManagerResponseStatus.Success);
        }
    }
    #endregion
}
