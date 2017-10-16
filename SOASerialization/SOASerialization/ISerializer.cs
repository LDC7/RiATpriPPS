namespace SOASerialization
{
    interface ISerializer
    {
        string Serialize<T>(T obj);
        T Deserialize<T>(string str);
    }
}
