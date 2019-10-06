## TextDB
A simple database based on text file. No setup is required.

[Get from nuget](https://www.nuget.org/packages/TextDB/)

### Insert records into databse
```
Notepad.InsertValue(entity);
```

### Read it from database 
```
var allStudents = Notepad.Select<Student>();
var filteredStudents = Notepad.Select<Student>(stud => stud.Result == "Pass");
```
