using System;

public enum SavingManagerResponseStatus { Success, LocalError, NetworkError, LocalandNetworkError, HasntSave }

public interface IPlatformSavingManager
{
    void Save();

    void DeleteSavedData();

    #region Volume

    void SetMusicVolume(float value);

    float GetMusicVolume(float defaultValue);

    void SetSFXVolume(float value);

    float GetSFXVolume(float defaultValue);
    #endregion

    #region GameProgress
    void SetGameProgress(GameProgress gameProgress, Action<SavingManagerResponseStatus> callback);

    void GetGameProgress(Action<GameProgress, SavingManagerResponseStatus> callback);
    #endregion
}
