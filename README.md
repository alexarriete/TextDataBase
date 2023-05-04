[![Getting Started](https://img.shields.io/badge/lang-en-red.svg)](https://github.com/alexarriete/TextDataBase/blob/master/README.md) [![Getting Started](https://img.shields.io/badge/lang-es-yellow.svg)](https://github.com/alexarriete/TextDataBase/blob/master/Readme.es.md)

# What is this project about?

TextDatabase is a class library implemented in C#. Contains several methods for the most common interactions between a database and applications in .Net. It can be used in small console and desktop projects.

# Dependencies

_In current version_

- net7.0
- Newtonsoft.Json 13.03

# How it works?

After installing the [Nuget package](https://www.nuget.org/packages/TextDataBase) we will be able to use the FileHandler class. Through this, the most common CRUD operations can be performed.

The product is strongly typed and needs to follow a series of rules for its proper functioning.

- The database will be created automatically when you call the FileHandler.GetInstance(appName) method. Its location is C:\Users\{username}\AppData\Local\{appName}
- The tables will be created with the first Insert and will bear the name of the class that is inserted.
- All objects to be inserted must belong to classes that inherit from TextDatabase.IAR and will carry an integer Id that will be the primary key and therefore must be unique.

```csharp
using TextDataBase;
...

```

## CRUD examples

##### Multiple Insert example

```csharp
   public async Task InsertListAsyn(List<User> users)
  {
        FileHandler fh = FileHandler.GetInstance("MyApp");
        await fh.InsertListAsync<User>(users);
  }
```

##### Get

```csharp
  public async Task GetUserList()
  {
        FileHandler fh = FileHandler.GetInstance("MyApp");
        await fh.Get<User>();
  }
```

##### Update

```csharp
  public async Task UpdateAsync(User user)
  {
       FileHandler fh = FileHandler.GetInstance("MyApp");
        await fh.UpdateAsync<User>(user);
  }
```

##### Delete

```csharp
  public async Task DeleteAsync(List<User> user)
  {
         FileHandler fh = FileHandler.GetInstance("MyApp");
        await fh.Delete<USser>(users, tableName);
  }
```

## Other examples

##### Backup example

```csharp
     public static bool BackupDatabase(string path)
        {
            try
            {
                DataBaseHandler.CreateBackup("MyApp", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), true);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;

        }
```

## Summary

Although we tested thousands of records, we do not consider it a substitute for databases. TextDatabase can work well with a low-complexity structure and a small volume of data. Specifically, when it is required to store temporary or configuration data, etc.
