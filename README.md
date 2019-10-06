## TextDB
A simple database based on text file. No setup is required.

[Get from nuget](https://www.nuget.org/packages/TextDB/)

#### Insert records into databse
```
Notepad.InsertValue(entity);
```

#### Read records from database 
```
var allStudents = Notepad.Select<Student>();
var filteredStudents = Notepad.Select<Student>(stud => stud.Result == "Pass");
```

#### Delete Records
```
Notepad.Delete<Student>(entity);
Notepad.Delete<Student>(std => std.Age == 34);
```

#### Update Records
```
Notepad.Update<Student>(newentity, key => key.Name == newentity.Name);
```

#### Migration Support
```
Notepad.Migrate<Student>(delegate(System.Reflection.PropertyInfo pi, string[] originalRecord) 
{
    if(pi.Name == "Name")
    {
        return "Moon";
    }
    if (pi.Name == "Age")
    {
        return originalRecord[1];
    }               
    return "";
}, new Guid("a17a9ea5-e91d-4238-98ff-4623780263af"));
```
