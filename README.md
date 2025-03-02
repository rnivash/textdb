# TextDB

TextDB is a simple, text file-based database that requires no setup. It provides basic CRUD operations and supports migrations. This makes it an ideal choice for lightweight applications or for use in environments where a full-fledged database is not necessary.

[Get from NuGet](https://www.nuget.org/packages/TextDB/)

## Features

- **No Setup Required**: Just include the library and start using it.
- **CRUD Operations**: Easily insert, read, update, and delete records.
- **Migration Support**: Migrate your data schema with ease.

## Installation

To install TextDB, run the following command in the NuGet Package Manager Console:

```sh
Install-Package TextDB

Usage
Insert Records
To insert a record into the database, use the InsertValue method:

Notepad.InsertValue(entity);

Read Records
To read records from the database, use the Select method:

var allStudents = Notepad.Select<Student>();
var filteredStudents = Notepad.Select<Student>(stud => stud.Result == "Pass");

Delete Records
To delete records from the database, use the Delete method:

Notepad.Delete<Student>(entity);
Notepad.Delete<Student>(std => std.Age == 34);

Update Records
To update records in the database, use the Update method:

Notepad.Update<Student>(newEntity, key => key.Name == newEntity.Name);

Migration Support
To migrate your data schema, use the Migrate method:

Notepad.Migrate<Student>((pi, originalRecord) => 
{
    if (pi.Name == "Name")
    {
        return "Moon";
    }
    if (pi.Name == "Age")
    {
        return originalRecord[1];
    }
    return "";
}, new Guid("a17a9ea5-e91d-4238-98ff-4623780263af"));

Contributing
Contributions are welcome! Please feel free to submit a pull request or open an issue.

License
This project is licensed under the MIT License. See the LICENSE file for details.

