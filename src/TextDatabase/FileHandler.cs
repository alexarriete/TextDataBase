using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace TextDatabase
{
    public class FileHandler
    {
        public static string? BasePath { get; set; }
        private static Lazy<FileHandler> instance = new Lazy<FileHandler>(() => new FileHandler());
        public string? AppName { get; set; }

        public static FileHandler GetInstance(string? appName = null)
        {
            if (string.IsNullOrEmpty(BasePath))
            {
                if (string.IsNullOrEmpty(appName))
                {
                    StackTrace stackTrace = new StackTrace();
                    appName = stackTrace.GetFrame(1)?.GetMethod()?.Module.Name.Replace(".dll", "");
                    if (appName == null)
                        throw new ArgumentException("appName can't be null");
                }                                                
                instance.Value.AppName = appName;
            }
            BasePath = DataBaseHandler.CreateDataBase(instance.Value.AppName);
            return instance.Value;
        }

      

        private FileHandler() { }
        private string? Read(string docName)
        {
            try
            {
                string path = Path.Combine(BasePath, $"{docName}.txt");
                FileInfo fileInfo = new FileInfo(path);
                if (fileInfo.Exists)
                {
                    using (StreamReader sr = File.OpenText(Path.Combine(BasePath, $"{docName}.txt")))
                    {
                        var doc = sr.ReadToEnd();
                        return doc;
                    }
                }
                return null;

            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }


        private string Write(string lines, string docName)
        {
            try
            {
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(BasePath, $"{docName}.txt")))
                {
                    outputFile.Write(lines);
                    return "";
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }

        }




        #region --GET    
        /// <summary>
        /// Gets a collection of object of type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>IEnumerable<T></returns>
        public IEnumerable<T>? Get<T>()
        {
           return GetList<T>().Cast<T>();
        }

        private IEnumerable<IAR>? GetList<T>()
        {
            var obj = (T)Activator.CreateInstance(typeof(T));
            string doc = Read(obj.GetType().Name);
            if (doc != null)
            {               
                List<IAR> list = JsonConvert.DeserializeObject<List<T>>(doc).Cast<IAR>().ToList();
                return list;
            }

            return null;
        }
        /// <summary>
        /// Gets an object of type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">object's Id</param>
        /// <returns>T</returns>
        public T? GetById<T>(int id)
        {
            IEnumerable<IAR> list = GetList<T>();
            return list.Where(x => x.Id == id).Cast<T>().FirstOrDefault();            
        }
        /// <summary>
        /// Get a subset of T that match the condition.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName">Name of the property to filter</param>
        /// <param name="value">Value to search</param>
        /// <returns>IEnumerable<T></returns>
        public IEnumerable<T>? GetByPropertyValue<T>(string propertyName, string value)
        {
            IEnumerable<IAR> list = GetList<T>();

            return list?.Where(x => x.GetType().GetProperty(propertyName).GetValue(x).ToString() == value.ToString()).Cast<T>();
        }


        /// <summary>
        /// Gets a collection of object of type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>IEnumerable<T></returns>
        public async Task<IEnumerable<IAR>?> GetAsync<T>()
        {
            return await Task.Run(() => GetList<T>());
        }

        /// <summary>
        /// Gets an object of type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">object's Id</param>
        /// <returns>T</returns>
        public async Task<T?> GetByIdAsync<T>(int id)
        {

            return await Task.Run(() => GetById<T>(id));
        }
        /// <summary>
        /// Get a subset of T that match the condition.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName">Name of the property to filter</param>
        /// <param name="value">Value to search</param>
        /// <returns>IEnumerable<T></returns>
        public async Task<IEnumerable<T>?> GetByPropertyValueAsync<T>(string propertyName, string value)
        {

            return await Task.Run(() => GetByPropertyValue<T>(propertyName, value));
        }


        #endregion


        #region -- UPDATE

        /// <summary>
        /// Updates a record in the file named as the object class.
        /// </summary>
        /// <param name="obj">object of type IAR</param>        
        /// <returns>Returns the number of rows updated.</returns>
        public int Update<T>(IAR obj)
        {
            try
            {
                var list = GetList<T>();
                var selectedObject = list?.FirstOrDefault(x => x.Id == obj.Id);
                if (selectedObject != null)
                {
                    foreach (PropertyInfo prop in selectedObject.GetType().GetProperties())
                    {
                        prop.SetValue(selectedObject, obj.GetType().GetProperty(prop.Name).GetValue(obj));
                    }
                    Write(JsonConvert.SerializeObject(list), obj.GetType().Name);
                    return 1;
                }               
                return 0;                
            }
            catch
            {
                return 0;
            }
        }


        /// <summary>
        /// Updates a record in the file named as the object class.
        /// </summary>
        /// <param name="obj">object of type IAR</param>        
        /// <returns>Returns the number of rows updated.</returns>
        public async Task<int> UpdateAsync<T>(IAR obj)
        {
            return await Task.Run(() => Update<T>(obj));
        }
        #endregion


        #region -- INSERT


        /// <summary>
        /// Inserts an object into the Table named as object type.
        /// </summary>
        /// <param name="obj">Its type must match with the name of the file</param>        
        /// <returns> Inserted object</returns>
        public T? Insert<T>(IAR obj)
        {
            List<IAR> objs = new List<IAR>() { obj };
            return InsertList<T>(objs).FirstOrDefault();
        }


        /// <summary>
        /// Inserts an object into the Table named as object type.
        /// </summary>
        /// <param name="obj">Its type must match with the name of the file</param>        
        /// <returns> Inserted object</returns>
        public async Task<T?> InsertAsync<T>(IAR obj)
        {
            return await Task.Run(() => Insert<T>(obj));
        }


        /// <summary>
        /// Inserts a list of object into the Table named as object type.
        /// </summary>
        /// <param name="items">items to insert. Its type has to match with the name of the table in the database</param>
        /// <param name="tableName">Fill it in if tableName is diferent to obj type name</param>
        /// <returns> A collection of inserted items</returns>
        public IEnumerable<T>? InsertList<T>(IEnumerable<IAR> items)
        {
            var list = GetList<T>()?.ToList();
            
            if (list != null)
            {
                IAR? repeated = list.FirstOrDefault(x => items.Any(n => n.Id == x.Id));
                if (repeated != null)
                {
                    throw new InvalidOperationException($"Error: Item whith id {repeated.Id} already exists.");
                }            
            }
            else
            {
                list = new List<IAR>();
            }
            int maxId = list.Count== 0 ? 0 : list.Max(x => x.Id);
            foreach (var item in items)
            {
                if(item.Id == 0)
                {                    
                    maxId = maxId+1;
                    item.Id = maxId;
                }
                else
                {
                    if (list.Any(x => x.Id == item.Id))                    
                        throw new InvalidOperationException($"Error: Item whith id {item.Id} already exists.");
                }   

                list.Add(item);
                maxId = list.Max(x => x.Id);
            }

            Write(JsonConvert.SerializeObject(list), items.FirstOrDefault().GetType().Name);

            return items.Cast<T>();
        }


        /// <summary>
        /// Inserts a list of object into the Table named as object type.
        /// </summary>
        /// <param name="items">items to insert. Its type has to match with the name of the table in the database</param>
        /// <param name="tableName">Fill it in if tableName is diferent to obj type name</param>
        /// <returns> A collection of inserted items</returns>
        public async Task<IEnumerable<T>?> InsertListAsync<T>(IEnumerable<IAR> items)
        {
            return await Task.Run(() => InsertList<T>(items));
        }

        #endregion


        #region -- DELETE

        /// <summary>
        /// Deletes a row from the file named as the object type.
        /// </summary>
        /// <param name="obj"></param>        
        /// <returns>Returns the number of rows deleted</returns>
        public int Delete<T>(IAR obj)
        {
            List<IAR> list = GetList<T>().ToList();
            int id = obj.Id;
            if (id != 0 && list.Any(x=>x.Id == id))
            {
                list = list.Where(x => x.Id != id).ToList();                
                Write(JsonConvert.SerializeObject(list), obj.GetType().Name);
                return id;
            }
            return 0;

        }


        /// <summary>
        /// Deletes a row from the file named as the object type.
        /// </summary>
        /// <param name="obj"></param>        
        /// <returns>Returns the number of rows deleted</returns>
        public async Task<int> DeleteAsync<T>(IAR obj)
        {
            return await Task.Run(() => Delete<T>(obj));
        }



        /// <summary>
        /// Deletes set of rows from the file named as the object type.
        /// </summary>
        /// <param name="objs">IEnumerable<IAR></param>              
        /// <returns>Returns the number of rows deleted</returns>
        public int DeleteRange<T>(IEnumerable<IAR> objs)
        {
            List<IAR> list = GetList<T>().ToList();
            List<int> ids = objs.Select(x => x.Id).ToList();
            if (ids != null)
            {
                list = list.Where(x => !ids.Any(l => l == x.Id)).ToList();              

                Write(JsonConvert.SerializeObject(list), objs.FirstOrDefault().GetType().Name);
                return ids.Count;
            }

            return 0;

        }

        /// <summary>
        /// Deletes set of rows from the file named as the object type.
        /// </summary>
        /// <param name="objs">IEnumerable<IAR></param>        
        /// <returns>Returns the number of rows deleted</returns>
        public async Task<int> DeleteRangeAsync<T>(IEnumerable<IAR> objs)
        {
            return await Task.Run(() => DeleteRange<T>(objs));
        }

        #endregion
    }
}
