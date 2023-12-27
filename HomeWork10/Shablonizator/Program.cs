using Shablonizator;
using Shablonizator.Models;
        
Console.WriteLine(PhrasesToStudents.Ex1("Лейсан"));
Console.WriteLine();
Console.WriteLine(PhrasesToStudents.Ex2(new StudentEx2{Address = "Ул.Пушкина"}));
Console.WriteLine();
Console.WriteLine(PhrasesToStudents.Ex3(new StudentEx3()));
Console.WriteLine();

var table = new Table();
table.Students.Add(new StudentForTable{FullName = "Лейсан Нонская" , Grade = 0});
table.Students.Add(new StudentForTable{FullName = "Никита Евстягин" , Grade = 100});
table.Students.Add(new StudentForTable{FullName = "Иван Сосорин" , Grade = 71});
table.Students.Add(new StudentForTable{FullName = "Лев Коснырев" , Grade = 56});

Console.WriteLine(PhrasesToStudents.Ex4(table));