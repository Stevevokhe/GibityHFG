public static class SavingManager
{
    private static IPlatformSavingManager instance;

    /// <summary>
    /// It returns SaveManager of the currently platform.
    /// </summary>
    public static IPlatformSavingManager Instance
    {
        get
        {
            instance ??= new LocalSavingManager();
            return instance;
        }
    }
}
