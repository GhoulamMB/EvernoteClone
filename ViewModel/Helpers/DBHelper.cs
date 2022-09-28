using EvernoteClone.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EvernoteClone.ViewModel.Helpers
{
    public class DBHelper
    {
        //public static readonly string DBPath = Path.Combine(Environment.CurrentDirectory,"NotesDB.db3");
        public static readonly string DBHost = "https://notesappwpf-7041f-default-rtdb.europe-west1.firebasedatabase.app/";

        public static async Task<bool> Insert<T>(T item)
        {
            //using(SQLiteConnection connection = new (DBPath))
            //{
            //    connection.CreateTable<T>();
            //    if(connection.Insert(item) > 0)
            //    {
            //        return true;
            //    }
            //}
            //return false;
            string url = $"{DBHost}{item!.GetType().Name.ToLower()}.json";
            var JSONBody = JsonConvert.SerializeObject(item);
            using (HttpClient client = new())
            {
                var data = new StringContent(JSONBody,System.Text.Encoding.UTF8,"application/json");
                var request = await client.PostAsync(url, data);

                if (request.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            return false;
        }

        public async static Task<bool> Update<T>(T item) where T : HasId
        {
            //using (SQLiteConnection connection = new(DBPath))
            //{
            //    connection.CreateTable<T>();
            //    if (connection.Update(item) > 0)
            //    {
            //        return true;
            //    }
            //}
            //return false;
            var JSONbody = JsonConvert.SerializeObject(item);
            var content = new StringContent(JSONbody,System.Text.Encoding.UTF8,"application/json");
            using (HttpClient client = new())
            {
                var request = await client.PatchAsync($"{DBHost}{item.GetType().Name.ToLower()}/{item.Id}.json",content);
                if(request.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            return false;
        }

        public async static Task<bool> Delete<T>(T item) where T : HasId
        {
            //using (SQLiteConnection connection = new(DBPath))
            //{
            //    connection.CreateTable<T>();
            //    if (connection.Delete(item) > 0)
            //    {
            //        return true;
            //    }
            //}
            //return false;
            using (HttpClient client = new())
            {
                var request = await client.DeleteAsync($"{DBHost}{item.GetType().Name.ToLower()}/{item.Id}.json");
                if (request.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            return false;
        }

        public static async Task<List<T>> Read<T>() where T : HasId
        {
            //List<T> list;
            //using (SQLiteConnection connection = new(DBPath))
            //{
            //    connection.CreateTable<T>();
            //    list = connection.Table<T>().ToList();
            //}
            //return list;
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync($"{DBHost}{typeof(T).Name.ToLower()}.json");
                var jsonResult = await result.Content.ReadAsStringAsync();

                if (result.IsSuccessStatusCode && jsonResult != null)
                {
                    var objects = JsonConvert.DeserializeObject<Dictionary<string, T>>(jsonResult);

                    List<T> list = new List<T>();
                    foreach (var o in objects)
                    {
                        o.Value.Id = o.Key;
                        list.Add(o.Value);
                    }

                    return list;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
