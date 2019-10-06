## TextDB
A simple database based on text file. No setup is required.

Nuget:
[Get it from nuget](https://www.nuget.org/packages/TextDB/)

### Insert records into databse
```
Notepad.InsertValue(entity1);
```

### Read it from database 
```
var studentList = Notepad.Select<Student>();
var student = Notepad.Select<Student>(stud => stud.Name == "Nivash");
```
